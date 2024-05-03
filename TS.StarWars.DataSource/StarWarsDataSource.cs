using Microsoft.Extensions.Logging;
using TS.StarWars.Infrastructure;
using TS.StarWars.Infrastructure.Services;
using TS.SWAPI;
using TS.SWAPI.Models.Films;
using TS.SWAPI.Models.People;
using TS.SWAPI.Models.Search;
using TS.SWAPI.Models.Starships;
using TS.SWAPI.Models.Vehicles;

namespace TS.StarWars.DataSource;

public class StarWarsDataSource : IStarWarsDataSource
{
    private readonly ILogger<StarWarsDataSource> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStarWarsDataSourceSettings _starWarsDataSourceSettings;
    private readonly SWAPIClientFactory _swApiClientFactory;

    public StarWarsDataSource(ILogger<StarWarsDataSource> logger, IHttpClientFactory httpClientFactory, IStarWarsDataSourceSettings starWarsDataSourceSettings)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _starWarsDataSourceSettings = starWarsDataSourceSettings;
        _swApiClientFactory = new SWAPIClientFactory();
    }

    private ISWAPIClient GetSWAPIClient() => _swApiClientFactory.Create(_httpClientFactory.CreateClient());

    public async Task<IEnumerable<IStarWarsCharacter>> GetStarWarsCharactersAsync(string searchCriteria)
    {
        try
        {
            _logger.LogInformation($"Search for people based on the following criteria '{searchCriteria}'...");
            var searchResults = await GetSWAPIClient().GetAsync<Search>(SWAPIUrlBuilder.GetSearchPeopleUri(_starWarsDataSourceSettings.ApiRootUrl, searchCriteria));
            _logger.LogInformation($"Search complete. {searchResults.Results.Count} people found");

            IEnumerable<IStarWarsCharacter> starWarsCharacters = Enumerable.Empty<IStarWarsCharacter>();
            if (searchResults.Results.Count > 0)
            {
                starWarsCharacters = await Task.WhenAll(searchResults.Results.Select(result => GetStarWarsCharacterAsync(result.Url)));
            }
            return starWarsCharacters;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Search error!");
            throw;
        }
    }

    private async Task<StarWarsCharacter> GetStarWarsCharacterAsync(string url)
    {
        People people = await GetPeopleAsync(url);
        var films = await Task.WhenAll(people.Films.Select(film => GetFilmTitleAsync(film as string)));
        var starships = await Task.WhenAll(people.Starships.Select(starship => GetStarshipNameAsync(starship as string)));
        var vehicles = await Task.WhenAll(people.Vehicles.Select(vehicle => GetVehicleNameAsync(vehicle as string)));

        return MapPeopleToStarWarsCharacter(people, films, starships, vehicles);
    }

    private static StarWarsCharacter MapPeopleToStarWarsCharacter(People people, string[] films, string[] starships, string[] vehicles)
    {
        return new StarWarsCharacter
        {
            BirthYear = people.Birth_year,
            EyeColor = people.Eye_color,
            Gender = people.Gender,
            HairColor = people.Hair_color,
            Height = people.Height,
            Homeworld = people.Homeworld,
            Mass = people.Mass,
            Name = people.Name,
            SkinColor = people.Skin_color,

            Films = films,
            Starships = starships,
            Vehicles = vehicles
        };
    }

    private async Task<People> GetPeopleAsync(string url)
    {
        _logger.LogInformation($"Loading person data: {url}");
        return await GetSWAPIClient().GetAsync<People>(new Uri(url));
    }

    private async Task<string> GetFilmTitleAsync(string? url)
    {
        _logger.LogInformation($"Loading film title: {url}");
        if (url == null) throw new ArgumentNullException("Film url is null!");

        var film = await GetSWAPIClient().GetAsync<Film>(new Uri(url));
        return film.Title;
    }

    private async Task<string> GetStarshipNameAsync(string? url)
    {
        _logger.LogInformation($"Loading starship name: {url}");
        if (url == null) throw new ArgumentNullException("Starship url is null!");

        var starship = await GetSWAPIClient().GetAsync<Starship>(new Uri(url));
        return starship.Name;
    }

    private async Task<string> GetVehicleNameAsync(string? url)
    {
        _logger.LogInformation($"Loading vehicle name: {url}");
        if (url == null) throw new ArgumentNullException("Vehicle url is null!");

        var vehicle = await GetSWAPIClient().GetAsync<Vehicle>(new Uri(url));
        return vehicle.Name;
    }
}

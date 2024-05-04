using Microsoft.Extensions.Logging;
using TS.StarWars.Infrastructure;
using TS.StarWars.Infrastructure.Services;
using TS.SWAPI;
using TS.SWAPI.Models.Films;
using TS.SWAPI.Models.People;
using TS.SWAPI.Models.Resources;
using TS.SWAPI.Models.Search;
using TS.SWAPI.Models.Starships;
using TS.SWAPI.Models.Vehicles;
using TS.Tasks;

namespace TS.StarWars.DataSource;

public class StarWarsDataSource : IStarWarsDataSource
{
    private readonly ILogger<StarWarsDataSource> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStarWarsDataSourceSettings _starWarsDataSourceSettings;
    private readonly SWAPIClientFactory _swApiClientFactory;
    private readonly TaskExecutionManager _taskExecutionManager;

    public StarWarsDataSource(ILogger<StarWarsDataSource> logger, IHttpClientFactory httpClientFactory, IStarWarsDataSourceSettings starWarsDataSourceSettings)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _starWarsDataSourceSettings = starWarsDataSourceSettings;
        _swApiClientFactory = new SWAPIClientFactory();
        _taskExecutionManager = new TaskExecutionManager(_starWarsDataSourceSettings.MaxDegreeOfParallelism);
    }

    private ISWAPIClient GetSWAPIClient() => _swApiClientFactory.Create(_httpClientFactory.CreateClient());

    private async Task<SWAPIUrlBuilder> GetSWAPIUrlBuilder() => new SWAPIUrlBuilder(await GetSWAPIClient().GetAsync<Resource>(new Uri(_starWarsDataSourceSettings.ApiRootUrl)));

    public async Task<IEnumerable<IStarWarsCharacter>> GetStarWarsCharactersAsync(string searchCriteria)
    {
        try
        {
            _logger.LogInformation($"Loading SWAPI resources information...");
            var swApiUrlBuilder = await GetSWAPIUrlBuilder();

            _logger.LogInformation($"Search for people based on the following criteria '{searchCriteria}'...");
            var searchResults = await _taskExecutionManager.Run(() => GetSWAPIClient().GetAsync<Search>(swApiUrlBuilder.GetSearchPeopleUri(searchCriteria)));
            _logger.LogInformation($"Search complete. {searchResults.Results.Count} people found");

            IEnumerable<IStarWarsCharacter> starWarsCharacters = [];
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
        return await _taskExecutionManager.Run(() => GetSWAPIClient().GetAsync<People>(new Uri(url)));
    }

    private async Task<string> GetFilmTitleAsync(string? url)
    {
        _logger.LogInformation($"Loading film title: {url}");
        ArgumentNullException.ThrowIfNull(url);

        var film = await _taskExecutionManager.Run(() => GetSWAPIClient().GetAsync<Film>(new Uri(url)));
        return film.Title;
    }

    private async Task<string> GetStarshipNameAsync(string? url)
    {
        _logger.LogInformation($"Loading starship name: {url}");
        ArgumentNullException.ThrowIfNull(url);

        var starship = await _taskExecutionManager.Run(() => GetSWAPIClient().GetAsync<Starship>(new Uri(url)));
        return starship.Name;
    }

    private async Task<string> GetVehicleNameAsync(string? url)
    {
        _logger.LogInformation($"Loading vehicle name: {url}");
        ArgumentNullException.ThrowIfNull(url);

        var vehicle = await _taskExecutionManager.Run(() => GetSWAPIClient().GetAsync<Vehicle>(new Uri(url)));
        return vehicle.Name;
    }
}

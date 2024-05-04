using TS.SWAPI.Models.Films;
using TS.SWAPI.Models.People;
using TS.SWAPI.Models.Resources;
using TS.SWAPI.Models.Search;
using TS.SWAPI.Models.Starships;
using TS.SWAPI.Models.Vehicles;

namespace TS.SWAPI.Tests
{
    [TestClass]
    public class SchemaTests
    {
        private const string ApiRootUrl = "https://swapi.py4e.com/api/";

        [TestMethod]
        [DataRow(ApiRootUrl)]
        public async Task PeopleSchema_CanDeserializeRemotePeopleResources_ReturnsPeople(string apiRootUrl)
        {
            using var httpClient = new HttpClient();
            var client = new SWAPIClientFactory().Create(httpClient);
            var resources = await client.GetAsync<Resource>(new Uri(apiRootUrl));
            var searchPeople = await client.GetAsync<Search>(new Uri(resources.People));
            var people = searchPeople.Results.Any() ? await client.GetAsync<People>(new Uri(searchPeople.Results.First().Url)) : null;

            Assert.IsNotNull(people);
        }

        [TestMethod]
        [DataRow(ApiRootUrl)]
        public async Task FilmSchema_CanDeserializeRemoteFilmResources_ReturnsFilm(string apiRootUrl)
        {
            using var httpClient = new HttpClient();
            var client = new SWAPIClientFactory().Create(httpClient);
            var resources = await client.GetAsync<Resource>(new Uri(apiRootUrl));
            var searchFilm = await client.GetAsync<Search>(new Uri(resources.Films));
            var film = searchFilm.Results.Any() ? await client.GetAsync<Film>(new Uri(searchFilm.Results.First().Url)) : null;

            Assert.IsNotNull(film);
        }

        [TestMethod]
        [DataRow(ApiRootUrl)]
        public async Task StarshipSchema_CanDeserializeRemoteStarshipResources_ReturnsStarship(string apiRootUrl)
        {
            using var httpClient = new HttpClient();
            var client = new SWAPIClientFactory().Create(httpClient);
            var resources = await client.GetAsync<Resource>(new Uri(apiRootUrl));
            var searchStarship = await client.GetAsync<Search>(new Uri(resources.Starships));
            var starship =  searchStarship.Results.Any() ? await client.GetAsync<Starship>(new Uri(searchStarship.Results.First().Url)) : null;

            Assert.IsNotNull(starship); 
        }

        [TestMethod]
        [DataRow(ApiRootUrl)]
        public async Task VehicleSchema_CanDeserializeRemoteVehicleResources_ReturnsVehicle(string apiRootUrl)
        {
            using var httpClient = new HttpClient();
            var client = new SWAPIClientFactory().Create(httpClient);
            var resources = await client.GetAsync<Resource>(new Uri(apiRootUrl));
            var searchVehicle = await client.GetAsync<Search>(new Uri(resources.Vehicles));
            var vehicle = searchVehicle.Results.Any() ? await client.GetAsync<Vehicle>(new Uri(searchVehicle.Results.First().Url)) : null;

            Assert.IsNotNull(vehicle);
        }
    }
}

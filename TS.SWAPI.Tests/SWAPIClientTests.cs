using TS.SWAPI.Models.Resources;

namespace TS.SWAPI.Tests;

[TestClass]
public class SWAPIClientTests
{
    private const string ApiRootUrl = "https://swapi.py4e.com/api/";

    [TestMethod]
    [DataRow(ApiRootUrl)]
    public async Task GetAsync_GetResourcesDefinition_ReturnsResources(string apiRootUrl) 
    {
        using var httpClient = new HttpClient();
        var client = new SWAPIClientFactory().Create(httpClient);

        var resources = await client.GetAsync<Resource>(new Uri(apiRootUrl));
        Assert.IsNotNull(resources);
    }
}

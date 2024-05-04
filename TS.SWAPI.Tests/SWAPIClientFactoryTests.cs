namespace TS.SWAPI.Tests;

[TestClass]
public class SWAPIClientFactoryTests
{
    [TestMethod]
    public void Create_ReturnsClient()
    {
        using var httpClient = new HttpClient();
        var client = new SWAPIClientFactory().Create(httpClient);

        Assert.IsNotNull(client);
    }
}

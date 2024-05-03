using Microsoft.Extensions.Configuration;
using TS.StarWars.Infrastructure.Services;

namespace TS.StarWars.App.Configuration;

internal class StarWarsDataSourceSettings : IStarWarsDataSourceSettings
{
    public StarWarsDataSourceSettings(IConfiguration configuration)
    {
        ApiRootUrl = configuration["SWAPI:ApiRootUrl"] ?? throw new Exception("Configuration error: missing ApiRootUrl!");
    }
    public string ApiRootUrl { get; }
}

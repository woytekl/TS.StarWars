using Microsoft.Extensions.Configuration;
using TS.StarWars.Infrastructure.Services;

namespace TS.StarWars.App.Configuration;

internal class StarWarsDataSourceSettings : IStarWarsDataSourceSettings
{
    public StarWarsDataSourceSettings(IConfiguration configuration)
    {
        ApiRootUrl = configuration["SWAPI:ApiRootUrl"] ?? throw new Exception("Configuration error: missing ApiRootUrl!");
        string maxDegreeOfParallelismString = configuration["SWAPI:MaxDegreeOfParallelism"] ?? throw new Exception("Configuration error: missing MaxDegreeOfParallelism!");

        int maxDegreeOfParallelism;
        MaxDegreeOfParallelism = Int32.TryParse(maxDegreeOfParallelismString, out maxDegreeOfParallelism) ?
            maxDegreeOfParallelism : throw new Exception("Configuration error: invalid MaxDegreeOfParallelism!");
    }
    public string ApiRootUrl { get; }

    public int MaxDegreeOfParallelism { get; }
}

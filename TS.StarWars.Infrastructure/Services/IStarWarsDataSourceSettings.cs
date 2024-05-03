namespace TS.StarWars.Infrastructure.Services;

public interface IStarWarsDataSourceSettings
{
    string ApiRootUrl { get; }

    int MaxDegreeOfParallelism { get; }
}

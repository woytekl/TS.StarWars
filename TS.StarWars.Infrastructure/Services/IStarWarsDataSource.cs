namespace TS.StarWars.Infrastructure.Services;

public interface IStarWarsDataSource
{
    Task<IEnumerable<IStarWarsCharacter>> GetStarWarsCharactersAsync(string searchCriteria);
}

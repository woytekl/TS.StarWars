namespace TS.StarWars.Infrastructure.Services;

internal interface IStarWarsDataSource
{
    Task<IEnumerable<IStarWarsCharacter>> GetStarWarsCharacterAsync(string searchCriteria);
}

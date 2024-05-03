namespace TS.StarWars.Infrastructure.Services;

public interface ISerializationService
{
    Task<string> SerializeAsync<T>(T obj);
}

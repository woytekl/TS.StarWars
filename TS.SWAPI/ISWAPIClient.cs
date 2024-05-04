
namespace TS.SWAPI;

public interface ISWAPIClient
{
    Task<T> GetAsync<T>(Uri requestUri) where T : class, new();
}
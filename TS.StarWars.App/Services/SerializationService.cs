using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TS.StarWars.Infrastructure.Services;

namespace TS.StarWars.App.Services;

internal class SerializationService : ISerializationService
{
    private readonly ILogger<SerializationService> _logger;

    public SerializationService(ILogger<SerializationService> logger)
    {
        _logger = logger;
    }

    public Task<string> SerializeAsync<T>(T obj)
    {
        try
        {
            _logger.LogInformation($"{typeof(T).Name} serialization started...");
            var serializedObj = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                Formatting = Formatting.Indented
            });
            _logger.LogInformation($"{typeof(T).Name} serialization completed successfully");
            return Task.FromResult(serializedObj);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Serialization failed!");
            throw;
        }
    }
}

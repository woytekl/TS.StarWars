using Microsoft.Extensions.Logging;
using TS.StarWars.Infrastructure.Services;

namespace TS.StarWars.App.Services;

internal class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public async Task WriteTextAsync(string filePath, string content)
    {
        try
        {
            _logger.LogInformation($"Saving data to the '{filePath}' file...");
            await File.WriteAllTextAsync(filePath, content);
            _logger.LogInformation("Writing to file completed successfully");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Write error!");
            throw;
        }
    }
}

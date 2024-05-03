namespace TS.StarWars.Infrastructure.Services;

public interface IFileService
{
    Task WriteTextAsync(string filePath, string content);
}

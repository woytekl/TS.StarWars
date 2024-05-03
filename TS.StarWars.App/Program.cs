using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using TS.StarWars.App.Configuration;
using TS.StarWars.App.Services;
using TS.StarWars.DataSource;
using TS.StarWars.Infrastructure.Services;

namespace TS.StarWars.App;

internal class Program
{
    private readonly ILogger<Program> _logger;
    private readonly IStarWarsDataSource _starWarsDataSource;
    private readonly ISerializationService _serializationService;
    private readonly IFileService _fileService;

    public Program(ILogger<Program> logger, IStarWarsDataSource starWarsDataSource, ISerializationService serializationService, IFileService fileService)
    {
        _logger = logger;
        _starWarsDataSource = starWarsDataSource;
        _serializationService = serializationService;
        _fileService = fileService;

        _logger.LogInformation("Application initialized successfully");
    }

    private async Task Run() 
    {
        try
        {
            var luke = await _starWarsDataSource.GetStarWarsCharactersAsync("Luke Skywalker");
            var serializedLuke = await _serializationService.SerializeAsync(luke);
            await _fileService.WriteTextAsync(Path.GetTempFileName(), serializedLuke);
        }
        catch 
        {
            _logger.LogCritical("Application execution failed!");
            throw;
        }
    }

    static async Task Main(string[] args)
    {
        using IHost host = BuildAppHost(args);
        await host.Services.GetRequiredService<Program>().Run();
    }

    private static IHost BuildAppHost(string[] args)
    {
        var builder = new HostBuilder()
        .ConfigureAppConfiguration(config =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional:false);
            config.AddCommandLine(args);
        })
        .ConfigureLogging((context, builder) => builder.AddNLog(context.Configuration))
        .ConfigureServices((hostingContext, services) =>
        {
            services.AddHttpClient();
            services.AddSingleton<IStarWarsDataSourceSettings, StarWarsDataSourceSettings>();
            services.AddTransient<IStarWarsDataSource, StarWarsDataSource>();
            services.AddTransient<ISerializationService, SerializationService>();
            services.AddTransient<IFileService, FileService>();
            services.AddSingleton<Program>();
        });
        return builder.Build();
    }
}

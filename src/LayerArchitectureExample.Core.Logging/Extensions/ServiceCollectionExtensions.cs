namespace LayerArchitectureExample.Core.Logging.Extensions;

using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

using static Serilog.Log;

public static class ServiceCollectionExtensions
{
    public static void AddSerilogLogging(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        services.AddLogging(c => c.ClearProviders());

        services.AddSerilogServices(configuration, environment);
    }

    private static void AddSerilogServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var loggingConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Environment", environment.EnvironmentName);

        try
        {
            ConfigureElasticSearch(configuration, environment, ref loggingConfiguration);
        }
        catch (Exception e)
        {
            Error(e, "Could not configure ElasticSearch: {ExceptionMessage}", e.Message);
        }

        services.AddLogging(builder => builder.AddSerilog());

        Logger = loggingConfiguration.CreateLogger();
        AppDomain.CurrentDomain.ProcessExit += (_, _) => CloseAndFlushAsync();

        services.AddSingleton(Logger);
    }

    private static void ConfigureElasticSearch(IConfiguration configuration, IHostEnvironment environment,  ref LoggerConfiguration loggerConfiguration)
    {
        var elasticSearchUrl = configuration["ElasticSearch:Uri"];
        var applicationName = configuration["ApplicationName"];
        var environmentName = environment.EnvironmentName?.ToLowerInvariant().Replace('.', '-');

        if (string.IsNullOrWhiteSpace(elasticSearchUrl) ||
            string.IsNullOrWhiteSpace(applicationName) ||
            string.IsNullOrWhiteSpace(applicationName))
        {
            Warning("Failed to configure ElasticSearch");
            return;
        }

        loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
        {
            IndexFormat = $"{applicationName}-{environmentName}-{DateTime.UtcNow:yyyy-MM}",
            AutoRegisterTemplate = true,
            NumberOfShards = 2,
            NumberOfReplicas = 1,
            CustomFormatter = new ElasticsearchJsonFormatter(),
        });
    }
}

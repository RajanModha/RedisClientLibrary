using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisClientLibrary.Implementations;
using RedisClientLibrary.Interfaces;
using RedisClientLibrary.Model;
using StackExchange.Redis;

namespace RedisClientLibrary;

public static class RedisClientExtensions
{
    /// <summary>
    /// Registers the Redis client service with the specified connection string and optional key format enforcement.
    /// </summary>
    /// <param name="services">The service collection to add the Redis client to.</param>
    /// <param name="connectionString">The connection string for Redis.</param>
    /// <param name="enforceKeyFormat">Optional regex string to enforce key format rules.</param>
    /// <param name="serviceLifetime">Optional ServiceLifetime to register IRedisClient, default ServiceLifetime is ServiceLifetime.Scoped.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRedisClient(
        this IServiceCollection services,
        string connectionString,
        string? enforceKeyFormat = null,
        string? serviceLifetime = null
    )
    {
        var configurationModel = new ConfigurationModel(connectionString, enforceKeyFormat, serviceLifetime);
        return AddRedisClient(services, configurationModel);
    }

    
    /// <summary>
    /// Registers the Redis client service using configuration from appsettings.
    /// </summary>
    /// <param name="services">The service collection to add the Redis client to application DI pool.</param>
    /// <param name="configuration">The application configuration to read Redis settings from appsettings.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRedisClient(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationModel = configuration.Get<ConfigurationModel>("RedisSettings");
        return AddRedisClient(services, configurationModel);
    }

    private static IServiceCollection AddRedisClient(IServiceCollection services, ConfigurationModel configurationModel)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(configurationModel.ConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        switch (configurationModel.ClientServiceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton<IRedisClient>(_ => new RedisClient(connectionMultiplexer, configurationModel.KeyFormatRegex));
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<IRedisClient>(_ => new RedisClient(connectionMultiplexer, configurationModel.KeyFormatRegex));
                break;
            default:
                services.AddScoped<IRedisClient>(_ => new RedisClient(connectionMultiplexer, configurationModel.KeyFormatRegex));
                break;
        }

        return services;
    }
}
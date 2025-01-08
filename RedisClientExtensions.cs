using Microsoft.Extensions.DependencyInjection;
using RedisClientLibrary.Implementations;
using RedisClientLibrary.Interfaces;
using StackExchange.Redis;
using System.Text.RegularExpressions;

namespace RedisClientLibrary;

public static class RedisClientExtensions
{
    /// <summary>
    /// Registers the Redis client service with the specified connection string and optional key format enforcement.
    /// </summary>
    /// <param name="services">The service collection to add the Redis client to.</param>
    /// <param name="connectionString">The connection string for Redis.</param>
    /// <param name="enforceKeyFormat">Optional regex string to enforce key format rules.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRedisClient(this IServiceCollection services, string connectionString, string? enforceKeyFormat = null)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
        }

        var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

        Regex? keyFormatRegex = null;
        if (!string.IsNullOrEmpty(enforceKeyFormat))
        {
            keyFormatRegex = new Regex(enforceKeyFormat, RegexOptions.Compiled);
        }

        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        services.AddTransient<IRedisClient>(_ => new RedisClient(connectionMultiplexer, keyFormatRegex));

        return services;
    }
}
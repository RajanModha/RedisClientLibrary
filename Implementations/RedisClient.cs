using RedisClientLibrary.Interfaces;
using StackExchange.Redis;
using System.Text.RegularExpressions;

namespace RedisClientLibrary.Implementations;

/// <summary>
/// Initializes a new instance of the <see cref="RedisClient"/> class.
/// </summary>
/// <param name="connectionMultiplexer">The Redis connection multiplexer.</param>
/// <param name="keyFormatRegex">Optional regex to enforce key formatting rules.</param>
internal class RedisClient(IConnectionMultiplexer connectionMultiplexer, Regex? keyFormatRegex = null) : IRedisClient
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly Regex? _keyFormatRegex = keyFormatRegex;

    private void ValidateKey(string key)
    {
        if (_keyFormatRegex != null && !_keyFormatRegex.IsMatch(key))
        {
            throw new ArgumentException($"Key '{key}' does not match the enforced key format.");
        }
    }

    public async Task<T?> Get<T>(string key)
    {
        ValidateKey(key);
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
        {
            return default;
        }
        return System.Text.Json.JsonSerializer.Deserialize<T>(value);
    }

    public async Task Push<T>(string key, T value, TimeSpan? expiry = null)
    {
        ValidateKey(key);
        var serializedValue = System.Text.Json.JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiry);
    }

    public async Task<bool> KeyExists(string key)
    {
        ValidateKey(key);
        return await _database.KeyExistsAsync(key);
    }

    public async Task Update<T>(string key, T value, TimeSpan? expiry = null)
    {
        ValidateKey(key);
        if (!await KeyExists(key))
        {
            throw new InvalidOperationException($"Key '{key}' does not exist.");
        }
        await Push(key, value, expiry);
    }

    public async Task Delete(string key)
    {
        ValidateKey(key);
        await _database.KeyDeleteAsync(key);
    }
}

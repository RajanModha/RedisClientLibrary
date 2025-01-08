namespace RedisClientLibrary.Interfaces;

/// <summary>
/// Interface for Redis client providing methods for data operations in Redis.
/// </summary>
public interface IRedisClient
{
    /// <summary>
    /// Retrieves the value associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key to look up in Redis.</param>
    /// <returns>The value of type <typeparamref name="T"/> if found; otherwise, default.</returns>
    Task<T?> Get<T>(string key);

    /// <summary>
    /// Stores a value in Redis with the specified key and optional expiration time.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    /// <param name="key">The key under which to store the value.</param>
    /// <param name="value">The value to store.</param>
    /// <param name="expiry">The optional expiration time for the key.</param>
    Task Push<T>(string key, T value, TimeSpan? expiry = null);

    /// <summary>
    /// Checks if a key exists in Redis.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the key exists; otherwise, false.</returns>
    Task<bool> KeyExists(string key);

    /// <summary>
    /// Updates the value of an existing key in Redis.
    /// </summary>
    /// <typeparam name="T">The type of the value to update.</typeparam>
    /// <param name="key">The key whose value to update.</param>
    /// <param name="value">The new value to set.</param>
    /// <param name="expiry">The optional expiration time for the key.</param>
    Task Update<T>(string key, T value, TimeSpan? expiry = null);

    /// <summary>
    /// Deletes a key from Redis.
    /// </summary>
    /// <param name="key">The key to delete.</param>
    Task Delete(string key);
}

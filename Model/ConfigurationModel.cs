using Microsoft.Extensions.DependencyInjection;
using RedisClientLibrary.Helpers;
using System.Text.RegularExpressions;

namespace RedisClientLibrary.Model
{
    internal class ConfigurationModel
    {
        public ConfigurationModel(string connectionString, string? keyFormatRegex, string? clientServiceLifetime)
        {
            ConnectionString = ValidateConnectionString(connectionString);
            KeyFormatRegex = ValidateKeyFormatRegex(keyFormatRegex);
            ClientServiceLifetime = ParseServiceLifetime(clientServiceLifetime);
        }

        public string ConnectionString { get; private set; }
        public Regex? KeyFormatRegex { get; private set; }
        public ServiceLifetime ClientServiceLifetime { get; private set; }

        private static string ValidateConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }
            return connectionString;
        }

        private static Regex? ValidateKeyFormatRegex(string? keyFormatRegex)
        {
            if (keyFormatRegex == null)
            {
                return null;
            }

            if (!RegexExtensions.IsValidRegex(keyFormatRegex))
            {
                throw new ArgumentException("Invalid regex for key format", nameof(keyFormatRegex));
            }

            return new Regex(keyFormatRegex, RegexOptions.Compiled);
        }

        private static ServiceLifetime ParseServiceLifetime(string? clientServiceLifetime)
        {
            if (string.IsNullOrWhiteSpace(clientServiceLifetime))
            {
                return ServiceLifetime.Scoped;
            }

            if (Enum.TryParse(typeof(ServiceLifetime), clientServiceLifetime, true, out var result))
            {
                return (ServiceLifetime)result;
            }

            throw new ArgumentException("Unsupported ScopeLifeTime to register client.", nameof(clientServiceLifetime));
        }
    }

}

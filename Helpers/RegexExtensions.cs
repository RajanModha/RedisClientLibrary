using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedisClientLibrary.Helpers
{
    internal static class RegexExtensions
    {
        /// <summary>
        /// Checks if the given string is a valid regular expression.
        /// </summary>
        /// <param name="pattern">The regex pattern to validate.</param>
        /// <returns>True if the pattern is valid, otherwise false.</returns>
        public static bool IsValidRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return false;

            try
            {
                Regex.Match("", pattern); // Try using the pattern
                return true;
            }
            catch (ArgumentException)
            {
                return false; // Exception indicates an invalid regex
            }
        }
    }
}

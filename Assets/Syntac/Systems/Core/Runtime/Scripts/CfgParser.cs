using System.Collections.Generic;
using System.Text;

namespace Syntac.Core
{
    /// <summary>
    /// Parser for Half-Life 2 style configuration files.
    /// Format: key value (space-separated, one per line).
    /// Lines starting with // are treated as comments.
    /// </summary>
    public static class CfgParser
    {
        /// <summary>
        /// Parses a configuration file content into a key-value dictionary.
        /// </summary>
        /// <param name="content">The raw content of the configuration file.</param>
        /// <returns>A dictionary containing the parsed key-value pairs.</returns>
        public static Dictionary<string, string> Parse(string content)
        {
            Dictionary<string, string> result = new();

            foreach (string line in content.Split('\n'))
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("//"))
                    continue;

                int spaceIndex = trimmed.IndexOf(' ');
                if (spaceIndex <= 0)
                    continue;

                string key = trimmed[..spaceIndex];
                string value = trimmed[(spaceIndex + 1)..];
                result[key] = value;
            }

            return result;
        }

        /// <summary>
        /// Serializes key-value pairs into configuration file format.
        /// </summary>
        /// <param name="header">Header comment for the file.</param>
        /// <param name="data">The key-value pairs to serialize.</param>
        /// <returns>A string in configuration file format.</returns>
        public static string Serialize(string header, IEnumerable<KeyValuePair<string, string>> data)
        {
            StringBuilder sb = new();
            sb.AppendLine($"// {header}");

            foreach (KeyValuePair<string, string> kvp in data)
                sb.AppendLine($"{kvp.Key} {kvp.Value}");

            return sb.ToString();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Utilities.Core
{
    /// <summary>
    /// Provides several common string functions to prevent needing to rewrite common code.
    /// </summary>
    public static class StringUtilities
    {
        /// <summary>
        /// Will compare the two strings using Equals(System.StringComparision.CulutureIgnoreCase)
        /// </summary>
        /// <returns>Are they equal?</returns>
        public static bool CompareCaseInsensitive(string first, string second)
        {
            return first.Trim().Equals(second.Trim(), System.StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Will convert a string from pascal case to a split string.
        /// Ex. ThisIsAString => This Is A String
        /// </summary>
        /// <returns>The split string</returns>
        public static string SplitPascalCase(string original)
        {
            if (string.IsNullOrEmpty(original)) return string.Empty;

            string pattern = @"([A-Z][a-z]+|[A-Z]+(?![a-z]))";
            var matches = Regex.Matches(original, pattern);
            List<string> words = matches.Select(m => m.Value).ToList();

            return string.Join(" ", words);
        }

        /// <summary>
        /// Will split the csv line into multiple different entries
        /// </summary>
        public static List<string> SplitCSV(string csvLine)
        {
            string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

            string[] rawFields = Regex.Split(csvLine, pattern);

            List<string> entries = new();
            foreach (string field in rawFields)
            {
                string cleanedField = field.Trim();
                if (cleanedField.StartsWith("\"") && cleanedField.EndsWith("\"") && cleanedField.Length > 1)
                {
                    cleanedField = cleanedField[1..^1];
                    cleanedField = cleanedField.Replace("\"\"", "\"");
                }
                entries.Add(cleanedField);
            }

            return entries;
        }

        /// <summary>
        /// Checks if a string is null or whitespace. This uses C# extension methods to 
        /// allow for doing myString.IsNullOrWhiteSpace() instead of string.IsNullOrWhiteSpace(myString).
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
        
        /// <summary>
        /// Will take the provided string and return a list of its tab-separated values.
        /// </summary>
        public static List<string> SplitTSV(string line)
        {
            string pattern = "\t(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

            string[] rawFields = Regex.Split(line, pattern);

            List<string> entries = new();
            foreach (string field in rawFields)
            {
                string cleanedField = field.Trim();
                if (cleanedField.StartsWith("\"") && cleanedField.EndsWith("\"") && cleanedField.Length > 1)
                {
                    cleanedField = cleanedField[1..^1];
                    cleanedField = cleanedField.Replace("\"\"", "\"");
                }

                entries.Add(cleanedField);
            }

            return entries;
        }
    }
}
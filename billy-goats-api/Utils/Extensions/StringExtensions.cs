using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BillyGoats.Api.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string CamelToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string CamelToPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (input.Length > 1)
            {
                return input.Substring(0, 1).ToUpper() + input.Substring(1);
            }
            else
            {
                return input.ToTitleCase();
            }
        }

        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (input.Length > 1)
            {
                return Char.ToLowerInvariant(input[0]) + input.Substring(1);
            }
            else
            {
                return Char.ToLowerInvariant(input[0]).ToString();
            }
        }

        public static string ToTitle(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (input.Length > 1)
            {
                string tmp = input.CamelToSnakeCase();
                string newString = string.Empty;
                string[] parts = tmp.Split('_');
                foreach (var part in parts)
                {
                    newString = newString + ' ' +  Char.ToUpperInvariant(part[0]) + part.Substring(1);
                }
                return newString.Trim();
            }
            else
            {
                return input.ToUpper();
            }
        }

        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}

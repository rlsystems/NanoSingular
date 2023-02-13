using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

// Helper utility methods
namespace NanoSingular.Infrastructure.Utility
{
    public static class NanoHelpers
    {
        // Hex Generator
        static Random random = new Random();
        public static string GenerateHex(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }


        // Retrieve enum descriptions
        public static string GetEnumDescription(this Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }


        // Remove whitespace from strings
        private static readonly Regex _whitespace = new(@"\s+");

        public static string ReplaceWhitespace(this string input, string replacement)
        {
            return _whitespace.Replace(input, replacement);
        }
    }
}

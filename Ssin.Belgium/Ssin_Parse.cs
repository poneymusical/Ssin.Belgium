using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Ssin.Belgium
{
    public partial struct Ssin
    {
        public static Ssin Parse(string source)
            => TryParse(source, out var ssin)
                ? ssin
                : throw new FormatException($"{0} is not a valid SSIN.");

        public static Ssin ParseExact(string source, SsinFormat format)
            => TryParseExact(source, out var ssin, format)
                ? ssin
                : throw new FormatException($"{0} is not a valid SSIN.");

        public static bool TryParse(string source, out Ssin parsed)
            => TryParseExact(source, out parsed, SsinFormat.Raw)
               || TryParseExact(source, out parsed, SsinFormat.Formatted);

        public static bool TryParseExact(string source, out Ssin parsed, SsinFormat format)
        {
            var str = string.Copy(source);

            if (!CheckFormat(source, format))
            {
                parsed = default;
                return false;
            }

            if (format == SsinFormat.Formatted)
                str = str.Replace(".", "").Replace("-", "");

            if (string.IsNullOrWhiteSpace(str) || !CheckLength(str))
            {
                parsed = default;
                return false;
            }

            if (!TryParsePart(str.Substring(0, 2), out var year)
                || !TryParsePart(str.Substring(2, 2), out var month)
                || !TryParsePart(str.Substring(4, 2), out var day)
                || !TryParsePart(str.Substring(6, 3), out var registrationIndex)
                || !TryParsePart(str.Substring(9, 2), out var control))
            {
                parsed = default;
                return false;
            }

            parsed = new Ssin
            {
                Year = year,
                Month = month,
                Day = day,
                RegistrationIndex = registrationIndex,
                Control = control
            };
            return true;
        }

        private static bool CheckFormat(string source, SsinFormat format)
            => format == SsinFormat.Formatted && source.Length == 15 && Regex.IsMatch(source, "[0-9]{2}.[0-9]{2}.[0-9]{2}-[0-9]{3}.[0-9]{2}")
               || format == SsinFormat.Raw && source.Length == 11 && Regex.IsMatch(source, "[0-9]{11}");

        private static bool CheckLength(string source)
            => source.Length == 11;

        private static bool TryParsePart(string part, out int parsed)
            => int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsed);
    }
}
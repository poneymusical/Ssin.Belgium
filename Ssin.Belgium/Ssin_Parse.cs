using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Ssin.Belgium
{
    public partial struct Ssin
    {
        /// <summary>
        /// Converts the string representation of a SSIN to its Ssin object equivalent.
        /// </summary>
        /// <param name="source">A string that contains a SSIN to convert.</param>
        /// <returns>An object that is equivalent to the SSIN contained in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="source"/> does not contain a valid string representation of a SSIN.</exception>
        public static Ssin Parse(string source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            return TryParse(source, out var ssin)
                ? ssin
                : throw new FormatException($"{source} is not a valid SSIN.");
        }

        /// <summary>
        /// Converts the string representation of a SSIN to its Ssin object equivalent.
        /// </summary>
        /// <param name="source">A string that contains a SSIN to convert.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="source"/>.</param>
        /// <returns>An object that is equivalent to the SSIN contained in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="source"/> does not contain a valid string representation of a SSIN.</exception>
        public static Ssin ParseExact(string source, SsinFormat format)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            return TryParseExact(source, out var ssin, format)
                ? ssin
                : throw new FormatException($"{source} is not a valid SSIN.");
        }

        /// <summary>
        /// Converts the specified string representation of a SSIN to its Ssin object equivalent and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">A string that contains a SSIN to convert.</param>
        /// <param name="parsed">When this method returns, contains the Ssin value equivalent to the SSIN contained in <paramref name="source"/>, if the conversion succeeded, or the default value if the conversion failed. The conversion fails if the <paramref name="source"/> parameter is null, is an empty string (""), or does not contain a valid string representation of a SSIN. This parameter is passed uninitialized.</param>
        /// <returns>true if the <paramref name="source">s</paramref> parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string source, out Ssin parsed)
            => TryParseExact(source, out parsed, SsinFormat.Raw)
               || TryParseExact(source, out parsed, SsinFormat.Formatted);
        
        /// <summary>
        /// Converts the specified string representation of a SSIN to its Ssin object equivalent. The format of the string representation must match a specified format exactly. The method returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">A string that contains a SSIN to convert.</param>
        /// <param name="parsed">When this method returns, contains the Ssin value equivalent to the SSIN contained in <paramref name="source"/>, if the conversion succeeded, or the default value if the conversion failed. The conversion fails if the <paramref name="source"/> parameter is null, is an empty string (""), , or does not contain a SSIN that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
        /// <param name="format">A format specifier that defines the required format of <paramref name="source"/>.</param>
        /// <returns>true if the <paramref name="source">s</paramref> parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParseExact(string source, out Ssin parsed, SsinFormat format)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                parsed = default;
                return false;
            }

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

            parsed = new Ssin(year, month, day, registrationIndex, control);
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
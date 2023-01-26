using System;

namespace Ssin.Belgium
{
    public partial struct Ssin
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int RegistrationIndex { get; set; }
        public int Control { get; set; }

        /// <summary>
        /// Default ctor.
        /// </summary>
        /// <param name="year">The year part of the "birth date"</param>
        /// <param name="month">The month part of the "birth date"</param>
        /// <param name="day">The day part of the "birth date"</param>
        /// <param name="registrationIndex">The incremental registration index, based on order of registration and gender of the person</param>
        /// <param name="control">The control number of the SSIN, based on modulo 97 rule</param>
        public Ssin(int year, int month, int day, int registrationIndex, int control)
        {
            Year = year;
            Month = month;
            Day = day;
            RegistrationIndex = registrationIndex;
            Control = control;
        }

        /// <summary>
        /// Returns the SSIN in a human-readable format.
        /// </summary>
        /// <returns>The SSIN as a string, formatted as "12345678901".</returns>
        public override string ToString() 
            => ToString(SsinFormat.Raw);

        /// <summary>
        /// Returns the SSIN in a human-readable format. The output depends on the specified format.
        /// </summary>
        /// <param name="format">The output format of the SSIN</param>
        /// <returns>The SSIN as a string, according to <paramref name="format"/>.</returns>
        public string ToString(SsinFormat format)
        {
            switch (format)
            {
                case SsinFormat.Raw:
                    return $"{Year:D2}{Month:D2}{Day:D2}{RegistrationIndex:D3}{Control:D2}";
                case SsinFormat.Formatted:
                    return $"{Year:D2}.{Month:D2}.{Day:D2}-{RegistrationIndex:D3}.{Control:D2}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}

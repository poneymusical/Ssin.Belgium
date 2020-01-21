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

        public Ssin(int year, int month, int day, int registrationIndex, int control)
        {
            Year = year;
            Month = month;
            Day = day;
            RegistrationIndex = registrationIndex;
            Control = control;
        }

        public override string ToString() 
            => ToString(SsinFormat.Raw);

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

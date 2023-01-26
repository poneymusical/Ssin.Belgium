using System;

namespace Ssin.Belgium
{
    public partial struct Ssin
    {
        /// <summary>
        /// Returns the birth date based on the user SSIN.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetBirthdate()
        {
            if (!IsDatePartValid())
            {
                return null;
            }
            
            var year = Year;
            year += (Is20XX()) ? 2000 : 1900;
            return new DateTime(year, GetMonth(), Day);
        }

        private int GetMonth()
        {
            var month = Month;
            while (month > 12)
                month -= 20;
            return month;
        }
    }
}
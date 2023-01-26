using System;

namespace Ssin.Belgium
{
    public partial struct Ssin
    {
        /// <summary>
        /// Returns a boolean value that indicates whether the current object represents a valid SSIN.
        /// </summary>
        /// <returns>true if the current Ssin represents a valid SSIN, false otherwise</returns>
        public bool IsValid()
            => IsDatePartValid() && IsRegistrationIndexValid() && IsControlValid();

        /// <summary>
        /// Returns a boolean value that indicates whether the string representation of a SSIN represents a valid SSIN.
        /// </summary>
        /// <returns>true if <paramref name="ssin"/> represents a valid SSIN, false otherwise</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ssin"/> is null.</exception>
        public static bool IsValid(string ssin)
        {
            if (null == ssin)
                throw new ArgumentNullException(nameof(ssin));

            return TryParse(ssin, out var parsed) && parsed.IsValid();
        }

        private bool IsDatePartValid()
        {
            //Based on https://www.ibz.rrn.fgov.be/fileadmin/user_upload/fr/rn/instructions/liste-TI/TI000_Numerodidentification.pdf

            //Numéro BIS: le mois est simplement augmenté de 20 ou 40
            //Le mois peut être compris entre 0 et 12, entre 20 et 32 ou entre 40 et 52
            //Calculer mois réel
            if (Month > 52)
                return false;

            var month = GetMonth();
            
            //Si un nombre est négatif => invalide d'office
            if (Year < 0 || month < 0 || Day < 0)
                return false;
            
            //Date de naissance connue entièrement
            if (Year >= 0 && month > 0 && Day > 0)
            {
                try
                {
                    var year = Year > 0 ? Year : 2000; // Si l'année est 0, on considère que c'est 2000
                    var _ = new DateTime(year, month, Day);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }

            //Date de naissance totalement inconnue : Year = 0, Month = 0, Day = 1
            if (Year == 0 && month == 0 && Day > 1)
                return true;

            //On connaît juste l'année ou juste l'année et le mois de naissance : Year > 0, Month = 0, Day >= 0
            if (Year > 0 && month == 0 & Day >= 0)
                return true;

            return false;
        }

        private bool IsRegistrationIndexValid()
        {
            //De 001 à 997 pour les hommes, de 002 à 998 pour les femmes 
            return RegistrationIndex > 0 && RegistrationIndex < 998;
        }

        private bool IsControlValid()
        {
            //Rule changes according to century of birth, but processes are inexpensive
            //so we can always compute them both instead of trying to guess which rule should be used
            return Is19XX() || Is20XX();
        }

        private bool Is19XX()
        {
            var control19XX = ComputeControlFor19XX();
            return control19XX == Control;
        }

        private bool Is20XX()
        {
            var control20XX = ComputeControlFor20XX();
            return control20XX == Control;
        }

        private long ComputeControlFor19XX()
        {
            var composite = long.Parse($"{Year:D2}{GetMonth():D2}{Day:D2}{RegistrationIndex:D3}");
            return ComputeControlFromComposite(composite);
        }

        private long ComputeControlFor20XX()
        {
            var composite = long.Parse($"2{Year:D2}{GetMonth():D2}{Day:D2}{RegistrationIndex:D3}");
            return ComputeControlFromComposite(composite);
        }

        private static long ComputeControlFromComposite(long composite)
            => 97 - (composite % 97);
    }
}
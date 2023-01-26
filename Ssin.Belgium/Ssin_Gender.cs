namespace Ssin.Belgium
{
    public partial struct Ssin
    {
        private const int SexUnknownIncrement = 20; 
        private const int SexKnownIncrement =  40;

        public SsinGender? GetGender()
        {
            if (IsSexUnknown())
                return null;
            
            // Si le numéro est un BIS (cfr IsBis()), le sexe est
            //  - inconnu si le mois est incrémenté de +20
            //  - connu si le mois est incrémenté de +40
            // https://housinganywhere.com/Belgium/belgian-national-number
            return RegistrationIndex % 2 == 0 ? SsinGender.Female : SsinGender.Male;
        }
        
        private bool IsSexKnownBase(int increment)
        {
            return Month >= (1 + increment) && Month <= (12 + increment);
        }
        
        private bool IsSexUnknown() => IsSexKnownBase(SexUnknownIncrement);
    }
}
namespace Ssin.Belgium
{
    public partial struct Ssin
    {
        public SsinGender GetGender()
        {
            return RegistrationIndex % 2 == 0 ? SsinGender.Female : SsinGender.Male;
        }
    }
}
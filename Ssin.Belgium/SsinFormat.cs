namespace Ssin.Belgium
{
    public enum SsinFormat
    {
        /// <summary>
        /// Indicates that the SSIN is represented with the digits only, e.g. 12345678901
        /// </summary>
        Raw,

        /// <summary>
        /// Indicates that the SSIN is represented as printed on belgian ID cards, e.g. 12.34.56-789.01
        /// </summary>
        Formatted
    }
}
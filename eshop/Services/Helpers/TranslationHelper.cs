using System.Globalization;

namespace eshop.Services.Helpers
{
    public class TranslationHelper
    {
        public static bool IsLanguageCodeValid(string languageCode)
        {
            return CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Any(culture => culture.TwoLetterISOLanguageName == languageCode);
        }
    }
}

using System.Globalization;

namespace SimpleProject.Helpers
{
    public class LocalizableEntity
    {
        public string Localize(string nameAr, string nameEn)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            if (culture.TwoLetterISOLanguageName.ToLower().Equals("ar"))
                return nameAr;
            return nameEn;
        }
    }
}

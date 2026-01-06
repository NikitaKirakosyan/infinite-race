using Southbyte.LocalizationSystem;
using UnityEngine;

namespace Southbyte
{
    public static class SystemLanguageExtensions
    {
        public static string GetLanguageCode(this SystemLanguage language)
        {
            switch(language)
            {
                case SystemLanguage.Russian:
                    return LanguageCodes.RuCode;

                case SystemLanguage.French:
                    return LanguageCodes.FrCode;

                case SystemLanguage.German:
                    return LanguageCodes.DeCode;

                case SystemLanguage.Italian:
                    return LanguageCodes.ItCode;

                case SystemLanguage.Spanish:
                    return LanguageCodes.EsCode;

                case SystemLanguage.Turkish:
                    return LanguageCodes.TrCode;

                default:
                    return LanguageCodes.EnCode;
            }
        }
    }
}
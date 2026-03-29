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

                default:
                    return LanguageCodes.EnCode;
            }
        }
    }
}
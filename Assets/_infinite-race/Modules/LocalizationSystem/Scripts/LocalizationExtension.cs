using NKLogger;

namespace Southbyte.LocalizationSystem
{
    public static class LocalizationExtension
    {
        private const string LogChannel = nameof(LocalizationExtension);
        private static LocalizationManager _localizationManager;
        
        public static void SetLocalizationManager(LocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }
        
        public static string Localize(this string key)
        {
            if (_localizationManager == null)
            {
                DebugPro.LogError("Null reference to LocalizationManager", LogChannel);
                return key;
            }
            
            return _localizationManager.GetLocalizedValue(key);
        }
        
        public static string Localize(this string key, string substitutionKey, string substitutionValue)
        {
            if (_localizationManager == null)
            {
                DebugPro.LogError("Null reference to LocalizationManager", LogChannel);
                return key;
            }
            
            return _localizationManager.GetLocalizedValue(key, substitutionKey, substitutionValue);
        }
        
        public static string Localize<T>(this string key, string substitutionKey, T substitutionValue) where T : struct
        {
            return Localize(key, substitutionKey, substitutionValue.ToString());
        }
        
        public static string Localize(this string key, LocPair pair1, LocPair pair2)
        {
            if (_localizationManager == null)
            {
                DebugPro.LogError("Null reference to LocalizationManager", LogChannel);
                return key;
            }
            
            return _localizationManager.GetLocalizedValue(key, pair1, pair2);
        }
        
        public static string Localize(this string key, params LocPair[] pairs)
        {
            if (_localizationManager == null)
            {
                DebugPro.LogError("Null reference to LocalizationManager", LogChannel);
                return key;
            }
            
            return _localizationManager.GetLocalizedValue(key, pairs);
        }
    }
}
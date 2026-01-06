using System;
using System.Text;
using Newtonsoft.Json;
using NKLogger;
using Southbyte.DIConfiguration;
using UnityEngine;

namespace Southbyte.LocalizationSystem
{
    [EarlyInitialization]
    public class LocalizationManager
    {
        public event Action OnLanguageChanged;
        
        private LocalizationData _localizationData;
        private static readonly StringBuilder KeyBuilder = new ();
        
        public string CurrentLanguageCode
        {
            get
            {
#if Localization_yg
                return YG.YG2.lang;
#else
                return SaveSystem.PlayerPrefsManager.GetSavedLanguageCode();
#endif
            }
            
            private set
            {
#if Localization_yg
                YG.YG2.SwitchLanguage(value);
#else
                SaveSystem.PlayerPrefsManager.SaveLanguageCode(value);
#endif
            }
        }
        
        
        public LocalizationManager()
        {
            LocalizationExtension.SetLocalizationManager(this);
        }
        
        ~LocalizationManager()
        {
            LocalizationExtension.SetLocalizationManager(null);
        }
        
        
        private void Initialize()
        {
            if(_localizationData != null)
            {
                DebugPro.LogError("LocalizationManager is already initialized!");
                return;
            }
            
            var localizationJson = Resources.Load<TextAsset>("LocalizationData/LocalizationData");
            _localizationData = JsonConvert.DeserializeObject<LocalizationData>(localizationJson.text);
            
#if Localization_yg
            YG.YG2.onCorrectLang += SetLanguage;
#endif
            
            DebugPro.Log($"LocalizationManager is initialized! Language: {CurrentLanguageCode}");
        }
        
        
        public void SetLanguage(string languageCode)
        {
            if(CurrentLanguageCode == languageCode)
                return;
            
            DebugPro.Log($"Language changed from {CurrentLanguageCode} to {languageCode}");
            CurrentLanguageCode = languageCode;
            OnLanguageChanged?.Invoke();
        }
        
        public string GetLocalizedValue(string localizationKey)
        {
            if(_localizationData == null)
                Initialize();
            
            if(!_localizationData.translationDictionary.TryGetValue(CurrentLanguageCode, out var localizedValues))
            {
                DebugPro.LogError($"Unable to find language with code: [{CurrentLanguageCode}]! Switched to en...", this);
                SetLanguage(LanguageCodes.EnCode);
            }
            
            if(!localizedValues.TryGetValue(localizationKey, out var localizedValue))
            {
                DebugPro.LogError($"Unable to find localization key: [{localizationKey}]!", this);
                return null;
            }
            
            return localizedValue;
        }
        
        public string GetLocalizedValue(string localizationKey, params LocPair[] pairs)
        {
            var value = GetLocalizedValue(localizationKey);
            
            if(value.IsNullOrEmptyOrWhiteSpace())
                return value;
            
            if(pairs != null)
            {
                foreach(var pair in pairs)
                    value = ApplyLocPair(value, pair);
            }
            
            return value;
        }
        
        public string GetLocalizedValue(string localizationKey, string substitutionKey, string substitutionValue)
        {
            var locPair = new LocPair(substitutionKey, substitutionValue);
            return GetLocalizedValue(localizationKey, locPair);
        }
        
        
        private string PrepareSubstitutionKey(string key)
        {
            KeyBuilder.Clear();
            KeyBuilder.Append('$').Append('{').Append(key).Append('}');
            
            return KeyBuilder.ToString();
        }
        
        private string ApplyLocPair(string value, LocPair pair)
        {
            var preparedKey = PrepareSubstitutionKey(pair.key);
            return value.Replace(preparedKey, pair.substitution).TrimStart();
        }
    }
}
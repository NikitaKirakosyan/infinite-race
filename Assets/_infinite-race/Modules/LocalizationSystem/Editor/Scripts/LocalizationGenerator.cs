#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoogleSheetsToUnity;
using Newtonsoft.Json;
using NKLogger;
using UnityEditor;
using UnityEngine;

namespace Southbyte.LocalizationSystem
{
    public static class LocalizationGenerator
    {
        private const string WorksheetName = "Main";
        private const string LocalizationAssetName = "LocalizationData.json";

        private static readonly string LocalizationLocalDirectoryPath = Path.Combine(Application.dataPath, "Resources/LocalizationData");
        private static readonly string LocalizationLocalPath = Path.Combine(LocalizationLocalDirectoryPath, LocalizationAssetName);
        private static readonly string[] TargetPaths = { LocalizationLocalPath };
        private static readonly char[] CharsToTrim = { ' ', '\r', '\n', '\t' };
        
        private static GoogleSheetsToUnityConfig _config;
        
        private static GoogleSheetsToUnityConfig Config
        {
            get
            {
                if(!_config)
                    _config = Resources.Load<GoogleSheetsToUnityConfig>("GSTU_Config");
                
                return _config;
            }
        }
        
        
        
        [MenuItem(EditorMenuNames.LocalizationRoot + "Generate localization from Google Sheets")]
        public static void UpdateLocalLocalization()
        {
            DebugPro.Log("Localization updating started...");
            SpreadsheetManager.Read(new GSTU_Search(Config.SheetId, WorksheetName), OnSpreadsheetRead);
        }
        
        private static void OnSpreadsheetRead(GstuSpreadSheet ss)
        {
            foreach(var path in TargetPaths)
            {
                var directory = Path.GetDirectoryName(path);
                if(directory != null && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            var localizationData = new LocalizationData { translationDictionary = new Dictionary<string, Dictionary<string, string>>() };

            foreach(var language in ss.columns.secondaryKeyLink.Skip(1))
            {
                var oneLanguageLocalizationData = new Dictionary<string, string>();

                foreach(var cell in ss.columns.GetValueFromEither(language.Key).Skip(1))
                {
                    var key = cell.rowId;
                    var value = cell.value;

                    if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                        continue;

                    var trimmedKey = key.Trim(CharsToTrim);
                    var trimmedValue = value.Trim(CharsToTrim);

                    if(oneLanguageLocalizationData.TryGetValue(trimmedKey, out var existingValue))
                    {
                        if(existingValue == trimmedValue)
                            DebugPro.LogError($"Attempt to add duplicated key with same value, key = {trimmedKey}, value = {trimmedValue}");
                        else
                            DebugPro.LogError(
                                $"Duplicated key with different value found, new value skipped, key = {trimmedKey}, newValue = {trimmedValue}, oldValue = {existingValue}");

                        return;
                    }

                    oneLanguageLocalizationData.Add(trimmedKey, trimmedValue);
                }

                localizationData.translationDictionary.Add(language.Key, oneLanguageLocalizationData);
            }

            var localizationJson = JsonConvert.SerializeObject(localizationData, Formatting.Indented);
            localizationJson = localizationJson.Replace(@"\\n", @"\n");

            foreach(var path in TargetPaths)
                File.WriteAllText(path, localizationJson);

            AssetDatabase.Refresh();
            DebugPro.Log("Localization data successfully updated.");

            LocalizationKeysGenerator.GenerateLocalizationVariablesFromSheet(ss);
            LanguageCodesGenerator.GenerateLocalizationVariablesFromSheet(ss);
        }
    }
}
#endif
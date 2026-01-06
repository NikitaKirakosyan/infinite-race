#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GoogleSheetsToUnity;
using NKLogger;
using UnityEditor;
using UnityEngine;

namespace Southbyte.LocalizationSystem
{
    public class LanguageCodesGenerator : MonoBehaviour
    {
        private const string LanguageCodesFileName = "LanguageCodes";

        private static string LocalizedKeysScriptFilePath;
        private static Dictionary<string, string> LanguageCodeValueByLanguageCode;


        /// <summary>
        /// Is used to extract from downloaded sheet language codes.
        /// And to write all language codes in LanguageCodes.cs file.
        /// </summary>
        /// <param name="sheet"></param>
        public static void GenerateLocalizationVariablesFromSheet(GstuSpreadSheet sheet)
        {
            if(!TryFindLocalizationKeysScriptFile())
                return;

            var locKeys = sheet.rows.primaryDictionary.First().Value.Skip(1).ToList();
            var languageKeys = sheet.rows.primaryDictionary.Skip(1).First().Value.Skip(1).ToList();

            if(locKeys.Count != languageKeys.Count)
            {
                DebugPro.LogError($"Something went wrong : \n There are {locKeys.Count} and {languageKeys.Count} EN value");
                return;
            }

            LanguageCodeValueByLanguageCode = new Dictionary<string, string>();

            for(var i = 0; i < locKeys.Count; i++)
                LanguageCodeValueByLanguageCode.Add(locKeys[i].value.Trim(), languageKeys[i].value.Trim());

            var stringBuilder = CreateContentForClassOfConstLocKeyVariables();

            File.WriteAllText(LocalizedKeysScriptFilePath, stringBuilder.ToString());
            AssetDatabase.Refresh();

            DebugPro.Log($"File {LanguageCodesFileName} is updated! \n You can check it on path {LocalizedKeysScriptFilePath}");
        }

        /// <summary>
        /// Convert string format.
        /// </summary>
        /// <returns>Return the string format out_of_profiles_meet => OutOfProfilesMeet</returns>
        private static string ReformatValueToName(string nameForVariable)
        {
            if(!nameForVariable.Contains("_"))
                return $"{nameForVariable[0].ToString().ToUpper()}{nameForVariable[1..]}";

            var characters = nameForVariable.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            characters = characters.Select(word => char.ToUpper(word[0]) + word[1..].ToLower()).ToArray();
            return string.Join(string.Empty, characters);
        }

        /// <summary>
        /// Find all script paths to select path for 'LanguageCodes.cs' file.
        /// </summary>
        /// <returns>Return true in case the file is found</returns>
        private static bool TryFindLocalizationKeysScriptFile()
        {
            var allScriptsGUIDs = AssetDatabase.FindAssets("t:script", null);

            if(allScriptsGUIDs.IsNullOrEmpty())
            {
                DebugPro.LogError("No assets with type of script are found");
                return false;
            }

            LocalizedKeysScriptFilePath = AssetDatabase.GUIDToAssetPath(Array.Find(allScriptsGUIDs, path =>
            {
                var guidToAssetPath = AssetDatabase.GUIDToAssetPath(path);
                return guidToAssetPath.Contains(LanguageCodesFileName) && !guidToAssetPath.Contains(nameof(LanguageCodesGenerator));
            }));

            if(string.IsNullOrEmpty(LocalizedKeysScriptFilePath))
            {
                DebugPro.LogError($"File {LanguageCodesFileName} is not found");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create content for LanguageCodes.cs : public static class LanguageCodes.
        /// </summary>
        private static StringBuilder CreateContentForClassOfConstLocKeyVariables()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("namespace Southbyte.LocalizationSystem\n{");
            stringBuilder.AppendLine("\tpublic static class LanguageCodes\n\t{");

            var index = 0;
            foreach(var locKey in LanguageCodeValueByLanguageCode.Keys)
            {
                stringBuilder.AppendLine($"\t\tpublic const string {ReformatValueToName(locKey)}Code = \"{locKey}\";");

                if(index < LanguageCodeValueByLanguageCode.Count - 1)
                    stringBuilder.AppendLine();

                index++;
            }

            stringBuilder.AppendLine("\t}\n}");
            return stringBuilder;
        }
    }
}
#endif
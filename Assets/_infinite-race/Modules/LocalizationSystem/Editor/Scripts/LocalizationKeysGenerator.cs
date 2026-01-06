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
    public class LocalizationKeysGenerator : MonoBehaviour
    {
        private const string LocalizationKeysFileName = "LocalizationKeys";

        private static string LocalizedKeysScriptFilePath;
        private static Dictionary<string, string> ENValueByLocKey;


        /// <summary>
        /// Is used to extract from downloaded sheet LocKeys and en variant (for summary).
        /// And to write all LocKeys in file.
        /// </summary>
        /// <param name="sheet"></param>
        public static void GenerateLocalizationVariablesFromSheet(GstuSpreadSheet sheet)
        {
            if(!TryFindLocalizationKeysScriptFile())
                return;

            var locKeys = sheet.columns.primaryDictionary.First().Value.Skip(1).ToList();
            var enKeys = sheet.columns.primaryDictionary.Skip(1).First().Value.Skip(1).ToList();

            if(locKeys.Count != enKeys.Count)
            {
                DebugPro.LogError($"Something went wrong : \n There are {locKeys.Count} and {enKeys.Count} EN value");
                return;
            }

            ENValueByLocKey = new Dictionary<string, string>();

            for(var i = 0; i < locKeys.Count; i++)
                ENValueByLocKey.Add(locKeys[i].value.Trim(), enKeys[i].value.Trim());

            var stringBuilder = CreateContentForClassOfConstLocKeyVariables();

            File.WriteAllText(LocalizedKeysScriptFilePath, stringBuilder.ToString());
            AssetDatabase.Refresh();

            DebugPro.Log($"File {LocalizationKeysFileName} is updated! \n You can check it on path {LocalizedKeysScriptFilePath}");
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
        /// Find all script paths to select path for 'LocalizationKeys.cs' file.
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
                return guidToAssetPath.Contains(LocalizationKeysFileName) && !guidToAssetPath.Contains(nameof(LocalizationKeysGenerator));
            }));

            if(string.IsNullOrEmpty(LocalizedKeysScriptFilePath))
            {
                DebugPro.LogError($"File {LocalizationKeysFileName} is not found");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create content for LocalizationKeys.cs : public static class LocalizationKeys.
        /// { here all keys from LocKeys sheet }
        /// </summary>
        /// <returns>Return a string of localization keys with description summary</returns>
        private static StringBuilder CreateContentForClassOfConstLocKeyVariables()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("namespace Southbyte.LocalizationSystem\n{");
            stringBuilder.AppendLine("\tpublic static class LocalizationKeys\n\t{");

            var index = 0;
            foreach(var locKey in ENValueByLocKey.Keys)
            {
                var enSummary = ENValueByLocKey[locKey];

                stringBuilder.AppendLine("\t\t/// <summary>");

                if(!TryToConvertTextIntoParagraphs(enSummary, out var paragraphs))
                {
                    stringBuilder.AppendLine($"\t\t/// {enSummary}");
                }
                else
                {
                    foreach(var paragraph in paragraphs)
                        stringBuilder.AppendLine($"\t\t/// {paragraph}");
                }

                stringBuilder.AppendLine("\t\t/// </summary>");
                stringBuilder.AppendLine($"\t\tpublic const string {ReformatValueToName(locKey)} = \"{locKey}\";");

                if(index < ENValueByLocKey.Count - 1)
                    stringBuilder.AppendLine();

                index++;
            }

            stringBuilder.AppendLine("\t}\n}");
            return stringBuilder;
        }

        /// <summary>
        /// Is used to validate if cell content (a line) contains several paragraphs and return list of sublines
        /// </summary>
        /// <param name="text"></param>
        /// <param name="paragraphs"></param>
        /// <returns></returns>
        private static bool TryToConvertTextIntoParagraphs(string text, out List<string> paragraphs)
        {
            paragraphs = null;

            if(text.Contains("\n"))
            {
                paragraphs = text.Split('\n').Where(paragraph => paragraph.IsNullOrEmpty()).ToList();
                if(paragraphs.Count > 1)
                    return true;
            }

            return false;
        }
    }
}
#endif
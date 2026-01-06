using System;
using System.Collections.Generic;
using System.Linq;
using NKLogger;
using UnityEditor;

namespace Southbyte.Editor
{
    public static class BuildDefinitionsUtility
    {
        public static void EditBuildDefinitions(Action<List<string>> editAction)
        {
            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var defines = currentDefines.Split(';').ToList();
            
            editAction?.Invoke(defines);
            
            var newDefines = string.Join(";", defines);
            DebugPro.Log(newDefines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
        }
        
        public static bool IsDefined(string definition)
        {
            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';');
            return currentDefines.ContainsIgnoreCase(definition);
        }
    }
}

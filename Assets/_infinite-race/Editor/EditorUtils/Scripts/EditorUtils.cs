#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using YG;

namespace Southbyte.Editor
{
    public static class EditorUtils
    {
        [MenuItem(itemName: EditorMenuNames.EditorUtilsRoot + "Clean All", isValidateFunction: false, priority: 0)]
        public static void CleanAll()
        {
#if Storage_yg
            YG2.SetDefaultSaves();
#endif
            
            CleanCache();
            CleanPersistentDataFolder();
        }

        [MenuItem(itemName: EditorMenuNames.EditorUtilsRoot + "Clean Unity Cache", isValidateFunction: false, priority: 1)]
        public static void CleanCache()
        {
            Caching.ClearCache();
        }

        [MenuItem(itemName: EditorMenuNames.EditorUtilsRoot + "Clean PlayerPrefs", isValidateFunction: false, priority: 2)]
        public static void CleanPlayerPrefs()
        {
            AppPlayerPrefs.DeleteAll();
        }

        [MenuItem(itemName: EditorMenuNames.EditorUtilsRoot + "Clean PersistentDataPath", isValidateFunction: false, priority: 20)]
        public static void CleanPersistentDataFolder()
        {
            var path = Application.persistentDataPath;
            if(Directory.Exists(path))
            {
                foreach(var file in Directory.GetFiles(path))
                    File.Delete(file);
                foreach(var directory in Directory.GetDirectories(path))
                    Directory.Delete(directory, true);
            }
        }

        [MenuItem(itemName: EditorMenuNames.EditorUtilsRoot + "Open PersistentDataPath", isValidateFunction: false, priority: 21)]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        
        [MenuItem(itemName: EditorMenuNames.EditorUtilsRoot + "Open DataPath", isValidateFunction: false, priority: 22)]
        public static void OpenDataPath()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }

        [MenuItem(itemName: EditorMenuNames.EditorUtilsRoot + "Create Screenshot #&s", isValidateFunction: false, priority: 36)]
        public static void CreateScreenshot()
        {
            var path = $"{Path.GetDirectoryName(Application.dataPath)}/Screenshots";
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var dateString = $"{DateTime.Now:yyyy-mm-dd}_{DateTime.Now:hh-mm-ss}";
            ScreenCapture.CaptureScreenshot($"{path}/Screenshot-{SceneManager.GetActiveScene().name}-{Screen.width}x{Screen.height}-timestamp-{dateString}.png");
            EditorUtility.RevealInFinder(path + "/");
        }
    }
}
#endif
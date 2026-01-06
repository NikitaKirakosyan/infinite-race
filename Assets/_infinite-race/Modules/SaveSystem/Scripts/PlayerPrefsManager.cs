using UnityEngine;

namespace Southbyte.SaveSystem
{
    public static class PlayerPrefsManager
    {
        private const string SavedLanguageCodeKey = "saved_language_code";
        private const string SavedSoundVolumeKey = "saved_sound_enabled";
        private const string SavedMusicVolumeKey = "saved_music_enabled";
        
        public const float DefaultSoundVolume = 0.7f;
        public const float DefaultMusicVolume = 0.5f;


        public static string GetSavedLanguageCode()
        {
            return AppPlayerPrefs.GetString(SavedLanguageCodeKey, Application.systemLanguage.GetLanguageCode());
        }

        public static void SaveLanguageCode(string langCode)
        {
            AppPlayerPrefs.SetString(SavedLanguageCodeKey, langCode);
        }
        
        public static float GetSoundVolume()
        {
            return AppPlayerPrefs.GetFloat(SavedSoundVolumeKey, DefaultSoundVolume);
        }

        public static void SaveSoundVolume(float value)
        {
            AppPlayerPrefs.SetFloat(SavedSoundVolumeKey, value);
        }
        
        public static float GetMusicVolume()
        {
            return AppPlayerPrefs.GetFloat(SavedMusicVolumeKey, DefaultMusicVolume);
        }

        public static void SaveMusicVolume(float value)
        {
            AppPlayerPrefs.SetFloat(SavedMusicVolumeKey, value);
        }
    }
}
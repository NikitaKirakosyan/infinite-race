using System;
using System.Collections.Generic;
using System.Globalization;
using NKLogger;
using UnityEngine;

#if RedefinePlayerPrefs_yg
using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

namespace Southbyte
{
    public static class AppPlayerPrefs
    {
        private static readonly Dictionary<string, float> CachedFloatValues = new ();
        private static readonly Dictionary<string, int> CachedIntValues = new ();
        private static readonly Dictionary<string, string> CachedStringValues = new ();


        public static void Save()
        {
            PlayerPrefs.Save();
        }

        public static void DeleteAll()
        {
            DebugPro.Log("Reset all keys");

            CachedFloatValues.Clear();
            CachedIntValues.Clear();
            CachedStringValues.Clear();

            PlayerPrefs.DeleteAll();
        }

        public static void DeleteKey(string key)
        {
            CachedFloatValues.Remove(key);
            CachedIntValues.Remove(key);
            CachedStringValues.Remove(key);

            PlayerPrefs.DeleteKey(key);
        }

        public static bool HasKey(string key)
        {
            if(CachedFloatValues.ContainsKey(key) || CachedIntValues.ContainsKey(key) ||
               CachedStringValues.ContainsKey(key))
                return true;

            return PlayerPrefs.HasKey(key);
        }

        public static float GetFloat(string key, float defaultValue = 0)
        {
            var result = defaultValue;

            if(CachedFloatValues.TryGetValue(key, out var value))
            {
                result = value;
                return result;
            }

            if(PlayerPrefs.HasKey(key))
            {
                result = PlayerPrefs.GetFloat(key);
                CachedFloatValues[key] = result;
            }

            return result;
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            var result = defaultValue;

            if(CachedIntValues.TryGetValue(key, out var value))
            {
                result = value;
                return result;
            }

            if(PlayerPrefs.HasKey(key))
            {
                result = PlayerPrefs.GetInt(key);
                CachedIntValues[key] = result;
            }

            return result;
        }

        public static string GetString(string key, string defaultValue = null)
        {
            string result = defaultValue;

            if(CachedStringValues.TryGetValue(key, out var value))
            {
                result = value;
                return result;
            }

            if(PlayerPrefs.HasKey(key))
            {
                result = PlayerPrefs.GetString(key);
                CachedStringValues[key] = result;
            }

            return result;
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            return GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public static bool ToggleBool(string key, bool defaultValue = false)
        {
            var value = GetBool(key, defaultValue);
            value = !value;
            SetBool(key, value);
            return value;
        }

        public static DateTime GetDate(string key, DateTime defaultValue = default)
        {
            string value = GetString(key);

            if(!string.IsNullOrEmpty(value) && long.TryParse(value, out var dateTimeLong))
            {
                var valueUtc = DateTime.FromBinary(dateTimeLong);
                var valueLocal = valueUtc.ToLocalTime();

                return valueLocal;
            }

            return defaultValue;
        }

        public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue = default)
        {
            var value = GetString(key);

            if(!string.IsNullOrEmpty(value) && TimeSpan.TryParse(value, out var timeSpan))
            {
                return timeSpan;
            }

            return defaultValue;
        }

        public static void SetFloat(string key, float value)
        {
            if(CachedIntValues.ContainsKey(key) || CachedStringValues.ContainsKey(key))
            {
                DebugPro.LogError($"Possible PlayerPrefs value override by key: '{key}' with value: '{value}'");
                throw new Exception($"Possible PlayerPrefs value override by key: '{key}' with value: '{value}'");
            }

            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();

            DebugPro.Log($"Set key (float) '{key}' to value '{value}'");

            CachedFloatValues[key] = value;
        }

        public static void SetInt(string key, int value)
        {
            if(CachedFloatValues.ContainsKey(key) || CachedStringValues.ContainsKey(key))
            {
                DebugPro.LogError($"Possible PlayerPrefs value override by key: '{key}' with value: '{value}'");
                throw new Exception($"Possible PlayerPrefs value override by key: '{key}' with value: '{value}'");
            }

            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();

            DebugPro.Log($"Set key (int) '{key}' to value '{value}'");

            CachedIntValues[key] = value;
        }

        public static void SetString(string key, string value)
        {
            if(CachedFloatValues.ContainsKey(key) || CachedIntValues.ContainsKey(key))
            {
                DebugPro.LogError($"Possible PlayerPrefs value override by key: '{key}' with value: '{value}'");
                throw new Exception($"Possible PlayerPrefs value override by key: '{key}' with value: '{value}'");
            }

            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();

            DebugPro.Log($"Set key (string) '{key}' to value '{value}'");

            CachedStringValues[key] = value;
        }

        public static void SetBool(string key, bool value)
        {
            SetInt(key, value ? 1 : 0);
        }

        public static void SetDate(string key, DateTime value)
        {
            var valueUTC = value.ToUniversalTime();
            var valueBinary = valueUTC.ToBinary();

            SetString(key, valueBinary.ToString());
        }

        public static void SetTimeSpan(string key, TimeSpan value)
        {
            SetString(key, value.ToString());
        }

        public static double GetDouble(string key)
        {
            var value = GetString(key, "0.0");
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0.0;
        }

        public static void SetDouble(string key, double value)
        {
            SetString(key, value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
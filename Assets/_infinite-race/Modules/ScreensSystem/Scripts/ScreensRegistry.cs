using System.Collections.Generic;
using NKLogger;

namespace Southbyte.ScreensSystem
{
    public static class ScreensRegistry
    {
        private const string LogChannel = nameof(ScreensRegistry);
        
        private static readonly Dictionary<string, string> Registries = new ()
        {
            //Screens
            { ScreenIds.InitialScreen, $"Screens/{ScreenIds.InitialScreen}" },
            { ScreenIds.MainScreen, $"Screens/{ScreenIds.MainScreen}" },
            { ScreenIds.EndScreen, $"Screens/{ScreenIds.EndScreen}" },
            //Popups
            { ScreenIds.AuthorizationPopup, $"Popups/{ScreenIds.AuthorizationPopup}" },
            { ScreenIds.RateUsPopup, $"Popups/{ScreenIds.RateUsPopup}" },
            { ScreenIds.NewsPopup, $"Popups/{ScreenIds.NewsPopup}" },
        };


        public static string GetScreenPath(string screenId)
        {
            if(Registries.TryGetValue(screenId, out var path))
                return path;

            DebugPro.LogError("Not registered! " + screenId, prefix: LogChannel);
            return null;
        }
    }
}
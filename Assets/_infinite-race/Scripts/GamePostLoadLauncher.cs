using Southbyte.DIConfiguration;
using Southbyte.ScreensSystem;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    [RegularInitialization]
    public class GamePostLoadLauncher
    {
        [Inject]
        public GamePostLoadLauncher(DiContainer diContainer, GameManager gameManager, ScreenManager screenManager)
        {
            gameManager.InitAfterLoad(screenManager);
            screenManager.Open<MainScreen>(ScreenIds.MainScreen);
            Object.FindAnyObjectByType<HUDController>(FindObjectsInactive.Include).Init(diContainer);
        }
    }
}
using Southbyte.DIConfiguration;
using Southbyte.ScreensSystem;
using Zenject;

namespace Southbyte
{
    [RegularInitialization]
    public class GamePostLoadLauncher
    {
        [Inject]
        public GamePostLoadLauncher(DiContainer diContainer, ScreenManager screenManager)
        {
            screenManager.Open<MainScreen>(ScreenIds.MainScreen);
        }
    }
}
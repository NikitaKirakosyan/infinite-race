using Zenject;

namespace Southbyte.ScreensSystem
{
    public class MainScreen : ScreenBase
    {
        [Inject] private ScreenManager _screenManager;
        
        public override string Id => ScreenIds.MainScreen;
        
        
        protected override void Awake()
        {
            base.Awake();
            
        }
    }
}
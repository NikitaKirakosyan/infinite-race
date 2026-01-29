using UnityEngine;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class MainScreen : ScreenBase
    {
        [SerializeField] private MainTab _mainTab;
        [SerializeField] private TuningTab _tuningTab;
        
        [Inject] private ScreenManager _screenManager;
        
        public override string Id => ScreenIds.MainScreen;
        
        
        protected override void Awake()
        {
            base.Awake();
            SelectTab(TabType.MainTab);
        }
        
        
        public void SelectTab(TabType tabType)
        {
            _mainTab.gameObject.SetActive(tabType == TabType.MainTab);
            _tuningTab.gameObject.SetActive(tabType == TabType.TuningTab);
        }
        
        
        public enum TabType
        {
            MainTab,
            TuningTab,
        }
    }
}
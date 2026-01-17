using NKLogger;
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
            switch(tabType)
            {
                case TabType.MainTab:
                    _mainTab.gameObject.SetActive(true);
                    break;
                
                case TabType.TuningTab:
                    _tuningTab.gameObject.SetActive(true);
                    break;
            }
            
            DebugPro.LogError($"Unexpected tab type {tabType}!");
        }
        
        
        public enum TabType
        {
            MainTab,
            TuningTab,
        }
    }
}
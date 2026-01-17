using UnityEngine;
using UnityEngine.UI;

namespace Southbyte.ScreensSystem
{
    public class TuningTab : ScreenTabBase<MainScreen>
    {
        [SerializeField] private Button _closeButton;
        
        
        protected override void Awake()
        {
            base.Awake();
            _closeButton.onClick.AddListener(() => ScreenRoot.SelectTab(MainScreen.TabType.MainTab));
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Southbyte.ScreensSystem
{
    public abstract class PopupBase : ScreenBase
    {
        [SerializeField] protected Button _closeButton;
        
        public override RectTransform ParentLayer => _uiLayers.OverlayRoot;
        
        
        protected override void Awake()
        {
            base.Awake();
            _closeButton.onClick.AddListener(Close);
        }
    }
}
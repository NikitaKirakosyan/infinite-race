using UnityEngine;

namespace Southbyte.ScreensSystem
{
    public sealed class UILayers : MonoBehaviour
    {
        [SerializeField] private RectTransform _screensRoot;
        [SerializeField] private RectTransform _overlayRoot;

        public RectTransform ScreensRoot => _screensRoot;
        public RectTransform OverlayRoot => _overlayRoot;
    }
}
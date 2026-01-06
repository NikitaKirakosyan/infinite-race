using UnityEngine;
using UnityEngine.UI;

namespace Southbyte.ScreensSystem
{
    public class InitialScreen : ScreenBase
    {
        [Header(nameof(InitialScreen))]
        [SerializeField] private Slider _progressBarSlider;
        
        public override string Id => ScreenIds.InitialScreen;
        
        
        protected override void Awake()
        {
            base.Awake();
            _progressBarSlider.minValue = 0;
            _progressBarSlider.maxValue = 1;
            _progressBarSlider.value = 0;
        }
        
        
        public void SetProgress(float loadingValue)
        {
            _progressBarSlider.value = loadingValue;
        }
    }
}
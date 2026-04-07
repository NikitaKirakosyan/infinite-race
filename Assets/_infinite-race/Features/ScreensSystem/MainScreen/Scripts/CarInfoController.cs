using Southbyte.LocalizationSystem;
using Southbyte.RaceSystem;
using Southbyte.ScreensSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    public class CarInfoController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _carNameText;
        [SerializeField] private CarInfoView _maxSpeedView;
        [SerializeField] private CarInfoView _powerView;
        [SerializeField] private CarInfoView _brakeView;
        [SerializeField] private CarInfoView _handlingView;
        
        [Inject] private CarProgressManager _carProgressManager;
        [Inject] private CarConfigsManager _carConfigsManager;
        [Inject] private LocalizationManager _localizationManager;
        
        
        private void Awake()
        {
            RefreshInternal();
            
            MainTab.OnCarIndexChanged += OnCarIndexChanged;
            _localizationManager.OnLanguageChanged += OnLanguageChanged;
        }
        
        private void OnDestroy()
        {
            MainTab.OnCarIndexChanged -= OnCarIndexChanged;
            _localizationManager.OnLanguageChanged -= OnLanguageChanged;
        }
        
        
        public void Refresh(CarConfig carConfig, CarProgress progress)
        {
            if(_carNameText)
                _carNameText.text = carConfig.carNameLocalizationKey.Localize();
            
            _maxSpeedView.Setup(CarStatsResolver.MaxSpeed(carConfig, progress), carConfig.maxMaxSpeed);
            _powerView.Setup(CarStatsResolver.Power(carConfig, progress), carConfig.maxPower);
            _brakeView.Setup(CarStatsResolver.Brake(carConfig, progress), carConfig.maxBrake);
            _handlingView.Setup(CarStatsResolver.Handling(carConfig, progress), carConfig.maxHandling);
        }
        
        
        private void RefreshInternal()
        {
            var currentCarConfig = _carConfigsManager.CarConfigs[MainTab.SelectedCarIndex];
            var currentCarProgress = _carProgressManager.Get(currentCarConfig.carId);
            Refresh(currentCarConfig, currentCarProgress);
        }
        
        private void OnCarIndexChanged(int carIndex)
        {
            RefreshInternal();
        }
        
        private void OnLanguageChanged()
        {
            RefreshInternal();
        }
    }
}
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
        
        
        private void Awake()
        {
            var currentCarConfig = _carConfigsManager.CarConfigs[MainTab.SelectedCarIndex];
            var currentCarProgress = _carProgressManager.Get(currentCarConfig.carId);
            Refresh(currentCarConfig, currentCarProgress);
            
            MainTab.OnCarIndexChanged += OnCarIndexChanged;
        }
        
        private void OnDestroy()
        {
            MainTab.OnCarIndexChanged -= OnCarIndexChanged;
        }
        
        
        public void Refresh(CarConfig c, CarProgress p)
        {
            _carNameText.text = c.carId;
            
            _maxSpeedView.Setup(CarStatsResolver.MaxSpeed(c, p), c.maxMaxSpeed);
            _powerView.Setup(CarStatsResolver.Power(c, p), c.maxPower);
            _brakeView.Setup(CarStatsResolver.Brake(c, p), c.maxBrake);
            _handlingView.Setup(CarStatsResolver.Handling(c, p), c.maxHandling);
        }
        
        
        private void OnCarIndexChanged(int carIndex)
        {
            var currentCarConfig = _carConfigsManager.CarConfigs[carIndex];
            var currentCarProgress = _carProgressManager.Get(currentCarConfig.carId);
            Refresh(currentCarConfig, currentCarProgress);
        }
    }
}
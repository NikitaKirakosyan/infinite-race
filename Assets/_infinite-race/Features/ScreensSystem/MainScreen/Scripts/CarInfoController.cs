using System;
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

        private bool _isTuningMode;
        private Func<CarStatType, bool> _canBuyUpgrade;
        private Action<CarStatType> _onBuyUpgrade;


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
            _isTuningMode = false;

            if(_carNameText)
                _carNameText.text = carConfig.carNameLocalizationKey.Localize();

            _maxSpeedView.Setup(CarStatsResolver.MaxSpeed(carConfig, progress), carConfig.maxMaxSpeed);
            _powerView.Setup(CarStatsResolver.Power(carConfig, progress), carConfig.maxPower);
            _brakeView.Setup(CarStatsResolver.Brake(carConfig, progress), carConfig.maxBrake);
            _handlingView.Setup(CarStatsResolver.Handling(carConfig, progress), carConfig.maxHandling);
        }

        public void RefreshTuning(
            CarConfig carConfig,
            CarProgress progress,
            Func<CarStatType, bool> canBuyUpgrade,
            Action<CarStatType> onBuyUpgrade)
        {
            _isTuningMode = true;
            _canBuyUpgrade = canBuyUpgrade;
            _onBuyUpgrade = onBuyUpgrade;

            if(_carNameText)
                _carNameText.text = carConfig.carNameLocalizationKey.Localize();

            SetupTuningView(_maxSpeedView, carConfig, progress, CarStatType.Speed, canBuyUpgrade, onBuyUpgrade);
            SetupTuningView(_powerView, carConfig, progress, CarStatType.Power, canBuyUpgrade, onBuyUpgrade);
            SetupTuningView(_brakeView, carConfig, progress, CarStatType.Brake, canBuyUpgrade, onBuyUpgrade);
            SetupTuningView(_handlingView, carConfig, progress, CarStatType.Handling, canBuyUpgrade, onBuyUpgrade);
        }


        private void RefreshInternal()
        {
            var currentCarConfig = _carConfigsManager.CarConfigs[MainTab.SelectedCarIndex];
            var currentCarProgress = _carProgressManager.Get(currentCarConfig.carId);

            if(_isTuningMode)
                RefreshTuning(currentCarConfig, currentCarProgress, _canBuyUpgrade, _onBuyUpgrade);
            else
                Refresh(currentCarConfig, currentCarProgress);
        }

        private static void SetupTuningView(
            CarInfoView view,
            CarConfig carConfig,
            CarProgress progress,
            CarStatType type,
            Func<CarStatType, bool> canBuyUpgrade,
            Action<CarStatType> onBuyUpgrade)
        {
            var currentValue = GetCurrentValue(carConfig, progress, type);
            var maxValue = GetMaxValue(carConfig, type);

            if(!CarProgressManager.CanUpgrade(carConfig, progress, type))
            {
                view.Setup(currentValue, maxValue);
                return;
            }

            var nextValue = GetNextValue(carConfig, progress, type);
            var price = CarProgressManager.GetUpgradePrice(carConfig, progress, type);
            var isBuyAvailable = canBuyUpgrade?.Invoke(type) ?? true;
            view.Setup(currentValue, maxValue, price, nextValue, isBuyAvailable, () => onBuyUpgrade?.Invoke(type));
        }

        private static float GetCurrentValue(CarConfig carConfig, CarProgress progress, CarStatType type)
        {
            switch(type)
            {
                case CarStatType.Speed:
                    return CarStatsResolver.MaxSpeed(carConfig, progress);

                case CarStatType.Power:
                    return CarStatsResolver.Power(carConfig, progress);

                case CarStatType.Brake:
                    return CarStatsResolver.Brake(carConfig, progress);

                case CarStatType.Handling:
                    return CarStatsResolver.Handling(carConfig, progress);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static float GetMaxValue(CarConfig carConfig, CarStatType type)
        {
            switch(type)
            {
                case CarStatType.Speed:
                    return carConfig.maxMaxSpeed;

                case CarStatType.Power:
                    return carConfig.maxPower;

                case CarStatType.Brake:
                    return carConfig.maxBrake;

                case CarStatType.Handling:
                    return carConfig.maxHandling;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static float GetNextValue(CarConfig carConfig, CarProgress progress, CarStatType type)
        {
            var previewProgress = new CarProgress
            {
                speed = progress.speed,
                power = progress.power,
                brake = progress.brake,
                handling = progress.handling,
            };

            switch(type)
            {
                case CarStatType.Speed:
                    previewProgress.speed++;
                    break;

                case CarStatType.Power:
                    previewProgress.power++;
                    break;

                case CarStatType.Brake:
                    previewProgress.brake++;
                    break;

                case CarStatType.Handling:
                    previewProgress.handling++;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return GetCurrentValue(carConfig, previewProgress, type);
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

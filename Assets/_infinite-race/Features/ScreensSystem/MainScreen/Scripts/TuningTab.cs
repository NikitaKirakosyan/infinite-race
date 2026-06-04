using Southbyte.CurrenciesSystem;
using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class TuningTab : ScreenTabBase<MainScreen>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private CarInfoController _carInfoController;

        [Inject] private CarConfigsManager _carConfigsManager;
        [Inject] private CarProgressManager _carProgressManager;
        [Inject] private CurrenciesManager _currenciesManager;


        protected override void Awake()
        {
            base.Awake();
            _closeButton.onClick.AddListener(() => ScreenRoot.SelectTab(MainScreen.TabType.MainTab));
        }

        private void OnEnable()
        {
            _currenciesManager.OnCurrencyChanged += OnCurrencyChanged;
            Refresh();
        }

        private void OnDisable()
        {
            if(_currenciesManager != null)
                _currenciesManager.OnCurrencyChanged -= OnCurrencyChanged;
        }


        private void Refresh()
        {
            var carConfig = _carConfigsManager.CarConfigs[MainTab.SelectedCarIndex];
            var carProgress = _carProgressManager.Get(carConfig.carId);
            _carInfoController.RefreshTuning(carConfig, carProgress, CanBuyUpgrade, OnBuyUpgrade);
        }

        private bool CanBuyUpgrade(CarStatType type)
        {
            var carConfig = _carConfigsManager.CarConfigs[MainTab.SelectedCarIndex];
            var carProgress = _carProgressManager.Get(carConfig.carId);
            var price = CarProgressManager.GetUpgradePrice(carConfig, carProgress, type);
            return _currenciesManager.GetCurrency(CurrencyType.Money) >= price;
        }

        private void OnBuyUpgrade(CarStatType type)
        {
            var carConfig = _carConfigsManager.CarConfigs[MainTab.SelectedCarIndex];
            var carProgress = _carProgressManager.Get(carConfig.carId);
            var price = CarProgressManager.GetUpgradePrice(carConfig, carProgress, type);

            if(_currenciesManager.GetCurrency(CurrencyType.Money) < price)
            {
                _currenciesManager.AnimateAll(CurrencyType.Money);
                Refresh();
                return;
            }

            if(!_carProgressManager.Upgrade(carConfig.carId, type, carConfig))
            {
                Refresh();
                return;
            }

            _currenciesManager.ChangeCurrency(CurrencyType.Money, -price);
            YG2.SaveProgress();
            Refresh();
        }

        private void OnCurrencyChanged(CurrencyType currencyType)
        {
            if(currencyType == CurrencyType.Money)
                Refresh();
        }
    }
}

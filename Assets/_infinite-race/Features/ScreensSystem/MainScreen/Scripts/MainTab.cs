using System;
using Southbyte.AdsSystem;
using Southbyte.CurrenciesSystem;
using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class MainTab : ScreenTabBase<MainScreen>
    {
        public static event Action<int> OnCarIndexChanged;
        
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _previousCarButton;
        [SerializeField] private Button _nextCarButton;
        [SerializeField] private Button _tuningButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _testDriveButton;
        
        [Inject] private GameManager _gameManager;
        [Inject] private CarConfigsManager _carConfigsManager;
        [Inject] private CurrenciesManager _currenciesManager;
        [Inject] private AdsManager _adsManager;
        
        public static int SelectedCarIndex;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _previousCarButton.onClick.AddListener(PreviousCar);
            _nextCarButton.onClick.AddListener(NextCar);
            _tuningButton.onClick.AddListener(OnTuningButtonClick);
            _buyButton.onClick.AddListener(OnBuyButtonClick);
            _testDriveButton.onClick.AddListener(OnTestDriveButtonClick);
            
            for(var i = 0; i < _carConfigsManager.CarConfigs.Count; i++)
            {
                var carConfig = _carConfigsManager.CarConfigs[i];
                if(carConfig.carId == YG2.saves.selectedCarId)
                {
                    SelectedCarIndex = i;
                    break;
                }
            }
            
            Refresh();
        }
        
        protected override void Start()
        {
            base.Start();
            OnCarIndexChanged?.Invoke(SelectedCarIndex);
        }
        
        
        private void SelectCar()
        {
            for(var i = 0; i < _carConfigsManager.CarConfigs.Count; i++)
            {
                var carConfig = _carConfigsManager.CarConfigs[i];
                if(SelectedCarIndex == i)
                {
                    YG2.saves.selectedCarId = carConfig.carId;
                    break;
                }
            }
        }
        
        private void OnPlayButtonClick()
        {
            _screenManager.Close(ScreenIds.MainScreen);
            _gameManager.Play();
        }
        
        private void Refresh()
        {
            var selectedCar = _carConfigsManager.CarConfigs[SelectedCarIndex];
            var isCarPurchased = YG2.saves.purchasedCarIds.Contains(selectedCar.carId);
            _playButton.gameObject.SetActive(isCarPurchased);
            _tuningButton.gameObject.SetActive(isCarPurchased);
            _testDriveButton.gameObject.SetActive(!isCarPurchased);
            _buyButton.gameObject.SetActive(!isCarPurchased);
            _buyButton.interactable = _currenciesManager.GetCurrency(CurrencyType.Money) >= selectedCar.price;
        }
        
        private void PreviousCar()
        {
            SelectedCarIndex--;
            if(SelectedCarIndex < 0)
                SelectedCarIndex = _carConfigsManager.CarConfigs.Count - 1;
            
            SelectCar();
            Refresh();
            OnCarIndexChanged?.Invoke(SelectedCarIndex);
        }
        
        private void NextCar()
        {
            SelectedCarIndex++;
            if(SelectedCarIndex >= _carConfigsManager.CarConfigs.Count)
                SelectedCarIndex = 0;
            
            SelectCar();
            Refresh();
            OnCarIndexChanged?.Invoke(SelectedCarIndex);
        }
        
        private void OnTuningButtonClick()
        {
            
        }
        
        private void OnBuyButtonClick()
        {
            var selectedCar = _carConfigsManager.CarConfigs[SelectedCarIndex];
            _currenciesManager.ChangeCurrency(CurrencyType.Money, -selectedCar.price);
            YG2.saves.purchasedCarIds.Add(selectedCar.carId);
            Refresh();
        }
        
        private void OnTestDriveButtonClick()
        {
            _adsManager.ShowRewardedVideoAd(() =>
            {
                OnPlayButtonClick();
                Refresh();
            });
        }
    }
}
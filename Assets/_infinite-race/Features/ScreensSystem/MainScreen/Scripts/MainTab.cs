using System;
using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class MainTab : ScreenTabBase
    {
        public static event Action<int> OnCarIndexChanged;
        
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _previousCarButton;
        [SerializeField] private Button _nextCarButton;
        
        [Inject] private GameManager _gameManager;
        [Inject] private CarConfigsManager _carConfigsManager;
        
        public static int SelectedCarIndex;
        
        
        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _previousCarButton.onClick.AddListener(PreviousCar);
            _nextCarButton.onClick.AddListener(NextCar);
            
            for(var i = 0; i < _carConfigsManager.CarConfigs.Count; i++)
            {
                var carConfig = _carConfigsManager.CarConfigs[i];
                if(carConfig.carId == YG2.saves.selectedCarId)
                {
                    SelectedCarIndex = i;
                    break;
                }
            }
        }
        
        private void Start()
        {
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
        
        private void PreviousCar()
        {
            SelectedCarIndex--;
            if(SelectedCarIndex < 0)
                SelectedCarIndex = _carConfigsManager.CarConfigs.Count - 1;
            
            SelectCar();
            OnCarIndexChanged?.Invoke(SelectedCarIndex);
        }
        
        private void NextCar()
        {
            SelectedCarIndex++;
            if(SelectedCarIndex >= _carConfigsManager.CarConfigs.Count)
                SelectedCarIndex = 0;
            
            SelectCar();
            OnCarIndexChanged?.Invoke(SelectedCarIndex);
        }
    }
}
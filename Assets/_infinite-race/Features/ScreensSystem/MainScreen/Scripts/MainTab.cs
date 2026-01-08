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
        
        private int _selectedCarIndex;
        
        
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
                    _selectedCarIndex = i;
                    break;
                }
            }
        }
        
        private void Start()
        {
            OnCarIndexChanged?.Invoke(_selectedCarIndex);
        }
        
        
        private void OnPlayButtonClick()
        {
            _screenManager.Close(ScreenIds.MainScreen);
            _gameManager.Play();
        }
        
        private void PreviousCar()
        {
            _selectedCarIndex--;
            if(_selectedCarIndex < 0)
                _selectedCarIndex = _carConfigsManager.CarConfigs.Count - 1;
            
            OnCarIndexChanged?.Invoke(_selectedCarIndex);
        }
        
        private void NextCar()
        {
            _selectedCarIndex++;
            if(_selectedCarIndex >= _carConfigsManager.CarConfigs.Count)
                _selectedCarIndex = 0;
            
            OnCarIndexChanged?.Invoke(_selectedCarIndex);
        }
    }
}
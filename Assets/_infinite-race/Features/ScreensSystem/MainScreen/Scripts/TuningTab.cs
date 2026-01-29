using System;
using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.ScreensSystem
{
    public class TuningTab : ScreenTabBase<MainScreen>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private CarInfoController _carInfoController;
        
        [Inject] private CarConfigsManager _carConfigsManager;
        [Inject] private CarProgressManager _carProgressManager;
        
        
        protected override void Awake()
        {
            base.Awake();
            _closeButton.onClick.AddListener(() => ScreenRoot.SelectTab(MainScreen.TabType.MainTab));
        }
        
        private void OnEnable()
        {
            var carConfig = _carConfigsManager.CarConfigs[MainTab.SelectedCarIndex];
            var carProgress = _carProgressManager.Get(carConfig.carId);
            _carInfoController.Refresh(carConfig, carProgress);
        }
    }
}

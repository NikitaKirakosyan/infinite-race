using Southbyte.BuildingSystem;
using Southbyte.CurrenciesSystem;
using UnityEngine;
using YG;
using Zenject;

namespace Southbyte
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private BuildingCardsContainerController _buildingCardsContainerController;
        [Inject] private BuildingManager _buildingManager;
        [Inject] private CurrenciesManager _currenciesManager;
        
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if(!hasFocus && YG2.isSDKEnabled)
                YG2.SaveProgress();
        }
        
        
        public void Play()
        {
            _buildingCardsContainerController.Setup(_buildingManager.AllBuildingSettings);
            _currenciesManager.SetCurrency(CurrencyType.Money, 100);
        }
    }
}
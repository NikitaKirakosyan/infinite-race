using Southbyte.CurrenciesSystem;
using Southbyte.RaceSystem;
using UnityEngine;
using YG;
using Zenject;

namespace Southbyte
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private CurrenciesManager _currenciesManager;
        
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if(!hasFocus && YG2.isSDKEnabled)
                YG2.SaveProgress();
        }
        
        
        public void Play()
        {
            FindFirstObjectByType<CarController>().StartEngine();
        }
        
        public void GameOver()
        {
            
        }
    }
}
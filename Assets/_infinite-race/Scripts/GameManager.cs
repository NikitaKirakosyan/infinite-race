using Southbyte.CurrenciesSystem;
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
            _currenciesManager.SetCurrency(CurrencyType.Money, 100);
        }
    }
}
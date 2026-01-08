using System;
using Southbyte.CurrenciesSystem;
using UnityEngine;
using YG;
using Zenject;

namespace Southbyte
{
    public class GameManager : MonoBehaviour
    {
        public event Action OnGameStarted;
        public event Action OnGameOver;
        
        [Inject] private CurrenciesManager _currenciesManager;
        
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if(!hasFocus && YG2.isSDKEnabled)
                YG2.SaveProgress();
        }
        
        
        public void Play()
        {
            OnGameStarted?.Invoke();
        }
        
        public void GameOver()
        {
            OnGameOver?.Invoke();
        }
    }
}
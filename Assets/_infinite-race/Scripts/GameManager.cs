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
        
        [SerializeField] private Camera _mainCamera;
        
        [Inject] private CurrenciesManager _currenciesManager;
        
        public bool IsPlaying { get; private set; }
        
        
        private void Awake()
        {
            _mainCamera.gameObject.SetActive(false);
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if(!hasFocus && YG2.isSDKEnabled)
                YG2.SaveProgress();
        }
        
        
        public void Play()
        {
            _mainCamera.gameObject.SetActive(true);
            IsPlaying = true;
            OnGameStarted?.Invoke();
            YG2.SaveProgress();
        }
        
        public void GameOver()
        {
            IsPlaying = false;
            OnGameOver?.Invoke();
            YG2.SaveProgress();
        }
    }
}
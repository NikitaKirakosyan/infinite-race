using System;
using Southbyte.CurrenciesSystem;
using Southbyte.DIConfiguration;
using Southbyte.ScreensSystem;
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
        
        private ScreenManager _screenManager;
        
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
        
        
        public void InitAfterLoad(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }
        
        public void Play()
        {
            if(_screenManager.TryGetScreen<EndScreen>(ScreenIds.EndScreen, out var screen))
                screen.Close();
            
            _mainCamera.gameObject.SetActive(true);
            IsPlaying = true;
            OnGameStarted?.Invoke();
            YG2.SaveProgress();
        }
        
        public void GameOver()
        {
            _screenManager.Open<EndScreen>(ScreenIds.EndScreen);
            IsPlaying = false;
            OnGameOver?.Invoke();
            YG2.SaveProgress();
        }
    }
}
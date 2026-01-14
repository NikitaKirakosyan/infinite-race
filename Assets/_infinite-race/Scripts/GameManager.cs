using System;
using Southbyte.CurrenciesSystem;
using Southbyte.RaceSystem;
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
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        public event Action OnGameRestarted;
        public event Action OnGameBraked;
        
        [SerializeField] private Camera _mainCamera;
        
        [Inject] private CurrenciesManager _currenciesManager;
        [Inject] private ScoreManager _scoreManager;
        
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
            Time.timeScale = 1;
            
            if(_scoreManager.Score > YG2.saves.bestScore)
                YG2.saves.bestScore =  _scoreManager.Score;
            
            if(_scoreManager.Distance > YG2.saves.bestDistance)
                YG2.saves.bestDistance = _scoreManager.Distance;
            
            var endScreen = _screenManager.Open<EndScreen>(ScreenIds.EndScreen);
            endScreen.Setup(_scoreManager.Score, _scoreManager.Distance, 1000, YG2.saves.bestScore, YG2.saves.bestDistance);
            
            IsPlaying = false;
            OnGameOver?.Invoke();
            YG2.SaveProgress();
        }
        
        public void Restart()
        {
            FindFirstObjectByType<CarSpawner>().transform.position = Vector3.zero;
            
            Time.timeScale = 1;
            IsPlaying = true;
            OnGameRestarted?.Invoke();
            OnGameStarted?.Invoke();
        }
        
        public void Pause()
        {
            Time.timeScale = 0;
            IsPlaying = false;
            OnGamePaused?.Invoke();
        }
        
        public void Resume()
        {
            Time.timeScale = 1;
            IsPlaying = true;
            OnGameResumed?.Invoke();
        }
        
        public void Brake()
        {
            Time.timeScale = 1;
            IsPlaying = false;
            OnGameBraked?.Invoke();
        }
    }
}
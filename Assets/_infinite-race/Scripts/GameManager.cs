using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Southbyte.CurrenciesSystem;
using Southbyte.DIConfiguration;
using Southbyte.RaceSystem;
using Southbyte.ScreensSystem;
using UnityEngine;
using YG;
using Zenject;
using Object = UnityEngine.Object;

namespace Southbyte
{
    [EarlyInitialization]
    public class GameManager : AsyncInitializationServiceBase, IInitializable
    {
        public event Action OnGameStarted;
        public event Action OnGameOver;
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        public event Action OnGameRestarted;
        public event Action OnGameBraked;
        
        private Camera _mainCamera;
        
        [Inject] private CurrenciesManager _currenciesManager;
        [Inject] private ScoreManager _scoreManager;
        
        private ScreenManager _screenManager;
        protected override List<Task> DependentServices => new List<Task>()
        {
            _currenciesManager.InitializationTask,
        };
        
        public bool IsPlaying { get; private set; }
        
        
        void IInitializable.Initialize()
        {
            _mainCamera = Camera.main;
            _mainCamera.gameObject.SetActive(false);
        }
        
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            TrySetInitializationResult(true);
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
            
            var money = Mathf.RoundToInt(_scoreManager.Distance * 2);
            
            var endScreen = _screenManager.Open<EndScreen>(ScreenIds.EndScreen);
            endScreen.Setup(_scoreManager.Score, _scoreManager.Distance, money, YG2.saves.bestScore, YG2.saves.bestDistance);
            
            IsPlaying = false;
            OnGameOver?.Invoke();
            YG2.SaveProgress();
        }
        
        public void Restart()
        {
            Object.FindFirstObjectByType<CarSpawner>().transform.position = Vector3.zero;
            
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
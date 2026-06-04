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
    public class GameManager : AsyncInitializationServiceBase, IInitializable, ITickable
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
        
        private float _timer;
        private float _passedSeconds;
        private GameMode currentMode;
        private GameModeConfig modeConfig;
        private CarController car;
        private ScreenManager _screenManager;
        
        public float Timer => _timer;
        
        public float RemainedTimer
        {
            get
            {
                if(CurrentMode == GameMode.TimeAttack)
                {
                    return modeConfig.timeLimit - _timer;
                }
                
                return 0;
            }
        }
        public GameMode CurrentMode => currentMode;
        public float ScorePerDistance => modeConfig ? modeConfig.scorePerDistance : 1f;
        
        
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
        
        void ITickable.Tick()
        {
            if(!IsPlaying)
                return;
            
            switch (currentMode)
            {
                case GameMode.CrashUntil:
                    UpdateCrashUntil();
                    break;
                
                case GameMode.TimeAttack:
                    UpdateTimeAttack();
                    break;
                
                case GameMode.Endless:
                    UpdateEndless();
                    break;
            }
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
            
            _timer = 0;
            _passedSeconds = 0;
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
            
            var money = CalculateMoneyReward();
            
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
        
        public void SetMode(int modeIndex)
        {
            currentMode = (GameMode)modeIndex;
            modeConfig = GameModeConfig.GetConfig(currentMode);
        }
        
        
        private void UpdateCrashUntil()
        {
            _passedSeconds += Time.deltaTime;
            
            if(_passedSeconds >= 1f)
            {
                _scoreManager.AddScore(modeConfig.scorePerSecond);
                _passedSeconds = 0;
            }
        }

        
        private void UpdateTimeAttack()
        {
            _timer += Time.deltaTime;
            _passedSeconds += Time.deltaTime;
            
            if(_passedSeconds >= 1f)
            {
                _scoreManager.AddScore(modeConfig.scorePerSecond);
                _passedSeconds = 0;
            }
            
            if (_timer >= modeConfig.timeLimit)
            {
                GameOver();
            }
        }
        
        private void UpdateEndless()
        {
            _passedSeconds += Time.deltaTime;
            
            if(_passedSeconds >= 1f)
            {
                _scoreManager.AddScore(modeConfig.scorePerSecond);
                _passedSeconds = 0;
            }
        }

        private int CalculateMoneyReward()
        {
            if(!modeConfig)
                return 0;

            var distanceReward = _scoreManager.Distance * modeConfig.moneyPerDistance;
            var scoreReward = _scoreManager.Score * modeConfig.moneyPerScore;
            var reward = (modeConfig.baseMoneyReward + distanceReward + scoreReward) * _scoreManager.MoneyMultiplier;
            return Mathf.Max(0, Mathf.RoundToInt(reward));
        }
    }
}

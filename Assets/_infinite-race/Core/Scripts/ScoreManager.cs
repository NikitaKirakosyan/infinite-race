using System.Collections.Generic;
using System.Threading.Tasks;
using Southbyte.DIConfiguration;
using Southbyte.RaceSystem;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    [EarlyInitialization]
    public class ScoreManager : AsyncInitializationServiceBase
    {
        [Inject] private GameManager _gameManager;
        
        private float _scoreMultiplier = 1f;
        private int _score;
        
        protected override List<Task> DependentServices => new List<Task>()
        {
            _gameManager.InitializationTask,
        };
        public CarController player => Object.FindAnyObjectByType<CarController>();
        public float Distance => player ? player.transform.position.z : 0;
        public float MoneyMultiplier => player ? player.config.moneyMultiplier : 1f;
        public int Score => Mathf.FloorToInt(Distance * _gameManager.ScorePerDistance * _scoreMultiplier) +
                            Mathf.FloorToInt(_score * _scoreMultiplier);
        
        
        public override async Task StartInitializationAsync()
        {
            await base.StartInitializationAsync();
            _gameManager.OnGameStarted += ResetScore;
            TrySetInitializationResult(true);
        }
        
        
        public void SetMultiplier(float multiplier)
        {
            _scoreMultiplier = multiplier;
        }
        
        public void AddScore(int score)
        {
            _score += score;
        }
        
        
        private void ResetScore()
        {
            _score = 0;
        }
    }
}

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
        
        public CarController player => Object.FindFirstObjectByType<CarController>();
        public float Distance => player ? player.transform.position.z : 0;
        public int Score => Mathf.FloorToInt(Distance * _scoreMultiplier) + Mathf.FloorToInt(_score * _scoreMultiplier);
        
        
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
            Debug.LogError(score);
            _score += score;
        }
        
        
        private void ResetScore()
        {
            _score = 0;
        }
    }
}
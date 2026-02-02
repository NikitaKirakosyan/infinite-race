using TMPro;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    public class TimerText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        [Inject] private GameManager _gameManager;
        
        
        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        private void Awake()
        {
            _gameManager.OnGameStarted += OnGameStarted;
            _gameManager.OnGameOver += Cleanup;
        }
        
        private void LateUpdate()
        {
            if(_gameManager.CurrentMode != GameMode.TimeAttack)
            {
                _text.enabled = false;
                return;
            }
            
            _text.text = $"Timer: {_gameManager.RemainedTimer:0}";
        }
        
        
        private void OnGameStarted()
        {
            _text.enabled = _gameManager.CurrentMode == GameMode.TimeAttack;
        }
        
        private void Cleanup()
        {
            _text.enabled = false;
        }
    }
}
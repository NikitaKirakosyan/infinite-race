using Southbyte.LocalizationSystem;
using Southbyte.RaceSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    public class SpeedText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        [Inject] private GameManager _gameManager;
        
        private CarController _carController;
        
        
        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        private void Awake()
        {
            _gameManager.OnGameOver += Cleanup;
        }
        
        private void LateUpdate()
        {
            if(_gameManager.IsPlaying && _carController == null)
                _carController = FindAnyObjectByType<CarController>();
            
            if(_carController)
            {
                var speed = $"{_carController.CurrentSpeed * 3.6f:0}";
                _text.text = LocalizationKeys.Speed.Localize("num", speed);
            }
        }
        
        private void Cleanup()
        {
            _carController = null;
            _text.text = LocalizationKeys.Speed.Localize("num", 0);
        }
    }
}
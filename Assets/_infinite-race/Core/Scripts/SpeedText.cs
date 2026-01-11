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
                _carController = FindFirstObjectByType<CarController>();
            
            if(_carController)
                _text.text = $"Speed: {_carController.CurrentSpeed * 3.6f:0}km/h";
        }
        
        private void Cleanup()
        {
            _carController = null;
            _text.text = "Speed: 0";
        }
    }
}
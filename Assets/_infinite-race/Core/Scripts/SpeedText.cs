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
            _gameManager.OnGameStarted += Setup;
            _gameManager.OnGameOver += Cleanup;
        }
        
        private void LateUpdate()
        {
            if(_carController)
                _text.text = $"Speed: {_carController.CurrentSpeed:0}km/h";
        }
        
        
        private void Setup()
        {
            _carController = FindObjectOfType<CarController>();
        }
        
        private void Cleanup()
        {
            _carController = null;
            _text.text = "Speed: 0";
        }
    }
}

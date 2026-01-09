
using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private float _minSpeed = 50;
        [SerializeField] private float _maxSpeed = 70;
        [SerializeField] private float _acceleration = 15;
        [SerializeField] private float _deceleration = 25;
        [SerializeField] private float _steerSpeed = 6;
        [SerializeField] private float _laneLimit = 4;
        [SerializeField] private GameObject _headlights;
        
        [Inject] private GameManager _gameManager;
        [Inject] private ScoreManager _scoreManager;
        
        private float _currentSpeed;
        private bool _isEngineStarted;
        
        public float CurrentSpeed => _currentSpeed * 4;
        
        public CarConfig config;
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += StartEngine;
            _gameManager.OnGameOver += StopEngine;
            SetHeadlightsActive(false);
        }
        
        private void Update()
        {
            if(!_isEngineStarted)
                return;
            
            // Speed
            if (Input.GetKey(KeyCode.W))
                _currentSpeed += _acceleration * Time.deltaTime;
            
            if (Input.GetKey(KeyCode.S))
                _currentSpeed -= _deceleration * Time.deltaTime;
            
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed);
            
            // Forward move
            transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);
            
            // Steering
            var steer = 0f;
            
            if (Input.GetKey(KeyCode.A)) 
                steer = -1f;
            
            if (Input.GetKey(KeyCode.D)) 
                steer = 1f;
            
            var pos = transform.position;
            pos.x += steer * _steerSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -_laneLimit, _laneLimit);
            transform.position = pos;
            
            // Lights
            if (Input.GetKeyDown(KeyCode.L))
                _headlights.SetActive(!_headlights.activeSelf);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            _gameManager.GameOver();
        }
        
        
        public void StartEngine()
        {
            _isEngineStarted = true;
            ApplyConfig();
            _currentSpeed = _minSpeed;
            SetHeadlightsActive(true);
        }
        
        public void StopEngine()
        {
            _isEngineStarted = false;
            _currentSpeed = 0;
            SetHeadlightsActive(false);
        }
        
        public void SetHeadlightsActive(bool value)
        {
            _headlights.SetActive(value);
        }
        
        
        private void ApplyConfig()
        {
            _minSpeed = config.minSpeed;
            _maxSpeed = config.maxSpeed;
            _acceleration = config.acceleration;
            _deceleration = config.deceleration;
            _steerSpeed = config.steerSpeed;
            _laneLimit = config.laneLimit;
            
            _scoreManager.SetMultiplier(config.scoreMultiplier);
        }
    }
}
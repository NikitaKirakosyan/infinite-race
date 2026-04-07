using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private float _laneLimit = 4;
        [SerializeField] private GameObject _headlights;
        
        [Header("Wheels")]
        [SerializeField] private Transform _frontLeftWheel;
        [SerializeField] private Transform _frontRightWheel;
        [SerializeField] private Transform _rearLeftWheel;
        [SerializeField] private Transform _rearRightWheel;
        [SerializeField] private float _wheelRotateSpeed = 360;
        [SerializeField] private float _maxSteerAngle = 30;
        [SerializeField] private float _steerReturnSpeed = 8;
        
        [Header("Steering")]
        [SerializeField] private float steerResponsiveness = 6f; // как быстро реагирует
        [SerializeField] private float yawStability = 10f;         // возврат корпуса
        [SerializeField] private float maxBodyRoll = 12f;
        [SerializeField] private float rollSpeed = 12f;
        
        [Inject] private GameManager _gameManager;
        [Inject] private ScoreManager _scoreManager;
        [Inject] private CarProgressManager _carProgressManager;
        
        private bool _isEngineStarted;
        
        private float _currentSpeed;
        private float _currentSteerAngle;
        private float _minSpeed = 50;
        private float _maxSpeed = 70;
        private float _acceleration = 15;
        private float _deceleration = 25;
        private float _steerSpeed = 6f;
        private float _lateralSpeed = 6f;
        
        private CarProgress _carProgress;
        private MobileInput _mobileInput;
        
        public float CurrentSpeed => _currentSpeed;
        
        public CarConfig config;
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += StartEngine;
            _gameManager.OnGameOver += OnGameOver;
            _gameManager.OnGameResumed += OnGameResumed;
            _gameManager.OnGamePaused += OnGamePaused;
            _gameManager.OnGameRestarted += OnGameRestarted;
            _gameManager.OnGameBraked += OnGameBraked;
            
            SetHeadlightsActive(false);
            
            if(_gameManager.IsPlaying)
                StartEngine();
            
            _mobileInput = FindAnyObjectByType<MobileInput>(FindObjectsInactive.Include);
        }
        
        private void OnDestroy()
        {
            _gameManager.OnGameStarted -= StartEngine;
            _gameManager.OnGameOver -= OnGameOver;
            _gameManager.OnGameResumed -= OnGameResumed;
            _gameManager.OnGamePaused -= OnGamePaused;
            _gameManager.OnGameRestarted -= OnGameRestarted;
            _gameManager.OnGameBraked -= OnGameBraked;
        }
        
        private void Update()
        {
            if(!_isEngineStarted)
                return;
            
            // Speed
            if (Input.GetKey(KeyCode.W) || _mobileInput.IsGasButtonDown)
                _currentSpeed += _acceleration * Time.deltaTime;
            else
                _currentSpeed -= _deceleration / 10f * Time.deltaTime;
            
            if (Input.GetKey(KeyCode.S) ||  _mobileInput.IsBrakeButtonDown)
                _currentSpeed -= _deceleration * Time.deltaTime;
            
            _currentSpeed = Mathf.Clamp(_currentSpeed, CarConfig.MinSpeed, _maxSpeed);
            
            //Camera fov
            var t = Mathf.InverseLerp(CarConfig.MinSpeed, _maxSpeed, _currentSpeed);
            var targetFov = Mathf.Lerp(40, 80, t);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFov, 5f * Time.deltaTime);
            
            var steerInput = Input.GetAxis("Horizontal"); // A/D
            if(_mobileInput)
            {
                if(_mobileInput.IsLeftButtonDown)
                    steerInput = -1f;
                
                if(_mobileInput.IsRightButtonDown)
                    steerInput = 1f;
            }
            
            //Yaw rotation
            var targetYaw = steerInput * _maxSteerAngle;
            
            var currentYaw = Mathf.LerpAngle(
                transform.localEulerAngles.y,
                targetYaw,
                steerResponsiveness * Time.deltaTime
            );
            
            var stabilizedYaw = Mathf.LerpAngle(
                currentYaw,
                0f,
                yawStability * Time.deltaTime
            );
            
            transform.localRotation = Quaternion.Euler(
                transform.localEulerAngles.x,
                stabilizedYaw,
                transform.localEulerAngles.z
            );
            
            //Roll
            var targetRoll = -steerInput * maxBodyRoll;
            
            var currentRoll = Mathf.Lerp(
                transform.localEulerAngles.z > 180
                    ? transform.localEulerAngles.z - 360
                    : transform.localEulerAngles.z,
                targetRoll,
                rollSpeed * Time.deltaTime
            );
            
            transform.localRotation = Quaternion.Euler(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y,
                currentRoll
            );
            
            //Front wheel rotation
            var targetSteer = steerInput * _maxSteerAngle;
            
            _currentSteerAngle = Mathf.Lerp(
                _currentSteerAngle,
                targetSteer,
                _steerReturnSpeed * Time.deltaTime
            );
            
            _frontLeftWheel.localRotation = Quaternion.Euler(_frontLeftWheel.localEulerAngles.x, _currentSteerAngle, 0);
            _frontRightWheel.localRotation = Quaternion.Euler(_frontRightWheel.localEulerAngles.x, _currentSteerAngle, 0);
            
            // All wheels spinning
            var rotation = _currentSpeed * _wheelRotateSpeed * Time.deltaTime;
            _frontLeftWheel.Rotate(Vector3.right, rotation);
            _frontRightWheel.Rotate(Vector3.right, rotation);
            _rearLeftWheel.Rotate(Vector3.right, rotation);
            _rearRightWheel.Rotate(Vector3.right, rotation);
            
            // Move
            transform.root.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);
            transform.root.Translate(Vector3.right * steerInput * _steerSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0f, stabilizedYaw, currentRoll);
            
            // Lights
            if (Input.GetKeyDown(KeyCode.L))
                _headlights.SetActive(!_headlights.activeSelf);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if(!_gameManager.IsPlaying)
                return;
            
            _gameManager.GameOver();
            FindFirstObjectByType<CameraController>().LateFly();
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
            _carProgress = _carProgressManager.Get(config.carId);
            
            _minSpeed = CarConfig.MinSpeed;
            _maxSpeed = CarStatsResolver.MaxSpeed(config, _carProgress);
            _acceleration = CarStatsResolver.Power(config, _carProgress);
            _deceleration = CarStatsResolver.Brake(config, _carProgress);
            _steerSpeed = CarStatsResolver.Handling(config, _carProgress);
            
            _scoreManager.SetMultiplier(config.scoreMultiplier);
        }
        
        private void OnGameOver()
        {
            StopEngine();
        }
        
        private void OnGameResumed()
        {
            _isEngineStarted = true;
        }
        
        private void OnGamePaused()
        {
            _isEngineStarted = false;
        }
        
        private void OnGameRestarted()
        {
            transform.root.position = Vector3.zero;
            transform.root.localEulerAngles = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }
        
        private void OnGameBraked()
        {
            transform.root.position = Vector3.zero;
            transform.root.localEulerAngles = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            
            StopEngine();
            Destroy(gameObject);
        }
    }
}
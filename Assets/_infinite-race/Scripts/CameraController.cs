using System.Collections;
using HandyEditorExtensions;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _lateFlyDuration = 2f;
        [SerializeField] private float _lateFlySpeed = 2f;
        [SerializeField] private bool _autoPosition = true;
        [SerializeField, HideIf(nameof(_autoPosition))] private Vector3 _offsetPosition;
        
        [Inject] private GameManager _gameManager;
        
        private Coroutine _lateFlyRoutine;
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += ResetLateFly;
            _gameManager.OnGameOver += ResetLateFly;
            
            if(_target && _autoPosition)
                _offsetPosition = _target.position - transform.position;
        }
        
        private void LateUpdate()
        {
            if(!_target || _lateFlyRoutine != null)
                return;
            
            var targetPosition = _target.position - _offsetPosition;
            transform.position = targetPosition;
        }
        
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        public void LateFly()
        {
            ResetLateFly();
            _lateFlyRoutine = StartCoroutine(LateFlyRoutine());
        }
        
        
        private void ResetLateFly()
        {
            StopAllCoroutines();
            _lateFlyRoutine = null;
        }
        
        private IEnumerator LateFlyRoutine()
        {
            var elapsedTime = 0f;
            while(elapsedTime < _lateFlyDuration)
            {
                transform.Translate(Vector3.forward * _lateFlySpeed * Time.deltaTime, Space.World);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
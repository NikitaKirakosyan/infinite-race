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
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += StopAllCoroutines;
            _gameManager.OnGameOver += StopAllCoroutines;
            
            if(_target && _autoPosition)
                _offsetPosition = _target.position - transform.position;
        }
        
        private void LateUpdate()
        {
            if(!_target)
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
            StartCoroutine(LateFlyRoutine());
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
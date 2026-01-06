using System;
using HandyEditorExtensions;
using UnityEngine;

namespace Southbyte
{
    [ExecuteAlways]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 5;
        [SerializeField] private bool _autoPosition = true;
        [SerializeField, HideIf(nameof(_autoPosition))] private Vector3 _offsetPosition;
        
        
        private void Awake()
        {
            if(_target && _autoPosition)
                _offsetPosition = _target.position - transform.position;
        }
        
        private void LateUpdate()
        {
            if(!_target)
                return;
            
            var targetPosition = _target.position - _offsetPosition;
            
#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                transform.position = targetPosition;
                return;
            }
#endif
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        }
    }
}
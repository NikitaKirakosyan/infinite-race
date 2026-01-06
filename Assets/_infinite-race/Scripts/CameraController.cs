using HandyEditorExtensions;
using UnityEngine;

namespace Southbyte
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
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
            transform.position = targetPosition;
        }
        
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}
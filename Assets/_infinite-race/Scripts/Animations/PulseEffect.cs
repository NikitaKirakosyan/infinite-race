using DG.Tweening;
using UnityEngine;

namespace Southbyte
{
    public class PulseEffect : MonoBehaviour
    {
        [SerializeField] private float _scale = 1.2f;
        [SerializeField] private float _duration = 3f;
        
        
        private void Awake()
        {
            ScaleUp();
        }
        
        
        private void ScaleUp()
        {
            transform.DOScale(new Vector3(_scale, _scale, _scale), _duration).OnComplete(ScaleDown).SetLink(gameObject);
        }
        
        private void ScaleDown()
        {
            transform.DOScale(Vector3.one, _duration).OnComplete(ScaleUp).SetLink(gameObject);
        }
    }
}
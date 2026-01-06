using UnityEngine;

namespace Southbyte
{
    public class SpinnerEffect : MonoBehaviour
    {
        [SerializeField] private float _rotationalSpeed = 45f;
        
        
        private void Update()
        {
            transform.Rotate(Vector3.up * (Time.deltaTime * _rotationalSpeed));
        }
    }
}
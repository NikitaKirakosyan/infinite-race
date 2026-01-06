using Southbyte.RaceSystem;
using TMPro;
using UnityEngine;

namespace Southbyte
{
    public class SpeedText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        
        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        private void LateUpdate()
        {
            _text.text = $"Speed: {FindFirstObjectByType<CarController>().CurrentSpeed:0}km/h";
        }
    }
}

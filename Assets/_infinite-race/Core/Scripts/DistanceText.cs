using Southbyte.LocalizationSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    public class DistanceText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        [Inject] private ScoreManager _scoreManager;
        
        
        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        private void LateUpdate()
        {
            var distance = $"{_scoreManager.Distance:0}m";
            _text.text = LocalizationKeys.Distance.Localize("num", distance);
        }
    }
}
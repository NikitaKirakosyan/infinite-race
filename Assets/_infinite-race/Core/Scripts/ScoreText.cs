using TMPro;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        [Inject] private ScoreManager _scoreManager;
        
        
        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        private void LateUpdate()
        {
            _text.text = $"Score: {_scoreManager.Score}";
        }
    }
}

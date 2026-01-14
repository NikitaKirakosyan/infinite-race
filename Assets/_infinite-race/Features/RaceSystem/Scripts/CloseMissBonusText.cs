using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Southbyte
{
    public class CloseMissBonusText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        private Color _color;
        
        
        private void Awake()
        {
            _color = _text.color;
        }
        
        
        public void Show(int value)
        {
            _text.DOKill();
            _text.color = _color;
            _text.rectTransform.anchoredPosition = new Vector2(0, -100);
            _text.text = $"CLOSE MISS!\nBONUS SCORE +{value}";
            _text.DOFade(0, 0.5f).SetDelay(0.5f).SetLink(_text.gameObject).OnComplete(() => _text.rectTransform.anchoredPosition = new Vector2(0, 400));
        }
    }
}
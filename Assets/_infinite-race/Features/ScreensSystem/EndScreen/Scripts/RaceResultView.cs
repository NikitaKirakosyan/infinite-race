using System.Collections;
using DG.Tweening;
using Southbyte.LocalizationSystem;
using TMPro;
using UnityEngine;

namespace Southbyte.ScreensSystem
{
    public class RaceResultView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private TextMeshProUGUI _bestDistanceText;
        
        [Header("Animation")]
        [SerializeField] private float _colorizeDuration = 0.3f;
        [SerializeField] private float _delayBetweenTexts = 0.1f;
        
        private Color _scoreColor;
        private Color _distanceColor;
        private Color _moneyColor;
        private Color _bestScoreColor;
        private Color _bestDistanceColor;
        private string _score;
        private string _distance;
        private string _money;
        private string _bestScore;
        private string _bestDistance;
        
        
        private void Awake()
        {
            _scoreColor = _scoreText.color;
            _distanceColor = _distanceText.color;
            _moneyColor = _moneyText.color;
            _bestScoreColor = _bestScoreText.color;
            _bestDistanceColor = _bestDistanceText.color;
        }
        
        
        public void Setup(float score, float distance, float money, float bestScore, float bestDistance)
        {
            StopAllCoroutines();
            
            _scoreText.color = Color.clear;
            _distanceText.color = Color.clear;
            _moneyText.color = Color.clear;
            _bestScoreText.color = Color.clear;
            _bestDistanceText.color = Color.clear;
            
            _score = $"{score:0}";
            _distance = $"{distance:0.00}m";
            _money = $"${money:0}";
            _bestScore = $"{bestScore:0}";
            _bestDistance = $"{bestDistance:0.00}m";
            
            Refresh();
            StartCoroutine(ColorizeTextsRoutine());
        }
        
        
        private void Refresh()
        {
            _scoreText.text = LocalizationKeys.Score.Localize("num", _score);
            _distanceText.text = LocalizationKeys.Distance.Localize("num", _distance);
            _moneyText.text = LocalizationKeys.Money.Localize("num", _money);
            _bestScoreText.text = LocalizationKeys.BestScore.Localize("num", _bestScore);
            _bestDistanceText.text = LocalizationKeys.BestDistance.Localize("num", _bestDistance);
        }
        
        private IEnumerator ColorizeTextsRoutine()
        {
            _scoreText.DOColor(_scoreColor, _colorizeDuration).SetLink(_scoreText.gameObject);
            yield return new WaitForSeconds(_delayBetweenTexts);
            _distanceText.DOColor(_distanceColor, _colorizeDuration).SetLink(_distanceText.gameObject);
            yield return new WaitForSeconds(_delayBetweenTexts);
            _moneyText.DOColor(_moneyColor, _colorizeDuration).SetLink(_moneyText.gameObject);
            yield return new WaitForSeconds(_delayBetweenTexts);
            _bestScoreText.DOColor(_bestScoreColor, _colorizeDuration).SetLink(_bestScoreText.gameObject);
            yield return new WaitForSeconds(_delayBetweenTexts);
            _bestDistanceText.DOColor(_bestDistanceColor, _colorizeDuration).SetLink(_bestDistanceText.gameObject);
            yield return new WaitForSeconds(_delayBetweenTexts);
        }
    }
}
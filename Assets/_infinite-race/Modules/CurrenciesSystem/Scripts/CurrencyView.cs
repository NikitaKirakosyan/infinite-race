using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.CurrenciesSystem
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TextMeshProUGUI _currencyCountText;
        
        [Inject] private CurrenciesManager _currenciesManager;
        
        private Vector2? _imagePosition;
        
        public CurrencyType CurrencyType => _currencyType;
        public CurrenciesConfig CurrenciesConfig => _currenciesManager.CurrenciesConfig;
        
        public CurrencySettings Settings
        {
            get
            {
                if(CurrenciesConfig.TryGetSettings(_currencyType, out CurrencySettings settings))
                    return settings;
                
                return default;
            }
        }
        
        
        private void Reset()
        {
            _currencyCountText = GetComponentInChildren<TextMeshProUGUI>(true);
        }
        
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _currenciesManager.IsInitializationCompleted);
            
            if(_currenciesManager.TryGetCurrencyIconSprite(_currencyType, out Sprite sprite))
                _currencyImage.sprite = sprite;
        }
        
        private void OnDisable()
        {
            if(_currenciesManager != null)
                _currenciesManager.OnCurrencyChanged -= OnCurrencyChanged;
        }
        
        private void OnEnable()
        {
            Refresh();
            
            if(_currenciesManager != null)
                _currenciesManager.OnCurrencyChanged += OnCurrencyChanged;
        }
        
        
        public void Refresh()
        {
            if(_currencyType == CurrencyType.Money)
                _currencyCountText.text = $"${_currenciesManager.GetCurrency(_currencyType).To1kString()}";
            else
                _currencyCountText.text = _currenciesManager.GetCurrency(_currencyType).To1kString();
            
            _currencyCountText.color = Settings.color;
        }
        
        public void Animate()
        {
            _currencyImage.transform.DOKill(true);
            _currencyImage.transform.DOPunchScale(CurrenciesConfig.punchScale, CurrenciesConfig.punchDuration, CurrenciesConfig.vibrato, CurrenciesConfig.elasticity).SetLink(_currencyImage.gameObject);
            
            _currencyCountText.transform.DOKill(true);
            _currencyCountText.transform.DOPunchScale(CurrenciesConfig.punchScale, CurrenciesConfig.punchDuration, CurrenciesConfig.vibrato, CurrenciesConfig.elasticity).SetLink(_currencyImage.gameObject);
        }
        
        public Vector2 GetImagePosition()
        {
            return _currencyImage.rectTransform.position;
        }
        
        public Vector2 GetImagePositionToWorldPoint()
        {
            if(_imagePosition.HasValue)
                return _imagePosition.Value;
            
            var rt = _currencyImage.rectTransform;
            var localCenter = new Vector3(rt.rect.width * (0.5f - rt.pivot.x), rt.rect.height * (0.5f - rt.pivot.y), 0f);
            
            _imagePosition = Camera.main.ScreenToWorldPoint(rt.TransformPoint(localCenter));
            return _imagePosition.Value;
        }
        
        
        private void OnCurrencyChanged(CurrencyType currencyType)
        {
            if(_currencyType != currencyType)
                return;

            Refresh();
            Animate();
        }
    }
}
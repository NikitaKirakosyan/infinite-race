using System.Collections.Generic;
using DG.Tweening;
using HandyEditorExtensions;
using NKLogger;
using UnityEngine;

namespace Southbyte.CurrenciesSystem
{
    public class CurrenciesBarView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private List<SerializablePair<CurrencyType, CurrencyView>> _currencyViews;
        
        [Header("Animations")]
        [SerializeField] private float _fadeDuration = 0.2f;
        
        
        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            
            var allViews = GetComponentsInChildren<CurrencyView>(true);
            foreach(var currencyView in allViews)
                _currencyViews.Add(new SerializablePair<CurrencyType, CurrencyView>(currencyView.CurrencyType, currencyView));
        }
        
        
        [Button]
        public void Show()
        {
            SetVisible(true);
        }
        
        [Button]
        public void Hide()
        {
            SetVisible(false);
        }
        
        public void Animate(CurrencyType currencyType)
        {
            foreach(var viewByCurrency in _currencyViews)
            {
                if(viewByCurrency.Key != currencyType)
                    continue;
                
                viewByCurrency.Value.Animate();
            }
        }
        
        public Vector2 GetCurrencyIconPosition(CurrencyType currencyType)
        {
            foreach(var viewByCurrency in _currencyViews)
            {
                if(viewByCurrency.Key != currencyType)
                    continue;
                
                return viewByCurrency.Value.GetImagePosition();
            }
            
            DebugPro.LogError($"Unexpected item type: {currencyType}! Unable get ore icon.");
            return Vector2.zero;
        }
        
        public Vector2 GetCurrencyIconWorldPosition(CurrencyType currencyType)
        {
            foreach(var viewByCurrency in _currencyViews)
            {
                if(viewByCurrency.Key != currencyType)
                    continue;
                
                return viewByCurrency.Value.GetImagePositionToWorldPoint();
            }
            
            DebugPro.LogError($"Unexpected item type: {currencyType}! Unable get ore icon.");
            return Vector2.zero;
        }
        
        
        private void SetVisible(bool isVisible)
        {
            var alpha = isVisible ? 1f : 0f;
            _canvasGroup.interactable = isVisible;
            _canvasGroup.blocksRaycasts = isVisible;
            
#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                _canvasGroup.alpha = alpha;
                return;
            }
#endif
            
            _canvasGroup.DOKill(true);
            _canvasGroup.DOFade(alpha, _fadeDuration).SetLink(_canvasGroup.gameObject);
        }
    }
}
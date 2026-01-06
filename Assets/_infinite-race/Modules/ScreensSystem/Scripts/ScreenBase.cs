using System;
using DG.Tweening;
using HandyEditorExtensions;
using NKLogger;
using UnityEngine;
using Zenject;

namespace Southbyte.ScreensSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ScreenBase : MonoBehaviour
    {
        private const float BaseAnimationFadeDuration = 0.2f;

        public event Action OnOpened;
        public event Action OnClosed;

        [SerializeField] private bool _isBaseOpeningAnimationAllowed = true;
        [SerializeField] private bool _isBaseClosingAnimationAllowed = true;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [Inject] protected UILayers _uiLayers;

        private ScreenState _screenState;
        private RectTransform _rectTransform;
        private bool _isAnimationsBlocked;
        
        public abstract string Id { get; }
        public virtual bool CanBeClosedByEscape => true;
        public virtual bool IsOverlay => false;
        public ScreenState ScreenState => _screenState;
        public RectTransform RectTransform => _rectTransform ??= (RectTransform)transform;
        public virtual RectTransform ParentLayer => _uiLayers.ScreensRoot;
        
        
        private void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnDestroy()
        {
        }


        public virtual void OnScreenFocus(bool isFocused)
        {
            DebugPro.Log($"Screen focus changed on: {isFocused} for screen with id: {Id}");
        }

        public virtual T Open<T>() where T : ScreenBase
        {
            if(_screenState == ScreenState.Opened)
            {
                DebugPro.LogWarning($"Screen [{Id}] is already opened. Unnecessary method call.", this);
                return (T)this;
            }

            DebugPro.Log($"Screen [{Id}] opened.", this);

            _screenState = ScreenState.Opened;
            OnScreenFocus(true);
            PlayBaseOpeningAnimation();

            OnOpened?.Invoke();
            return (T)this;
        }

        [Button]
        public virtual T OpenImmediately<T>() where T : ScreenBase
        {
            _isAnimationsBlocked = true;
            var openedScreen = Open<T>();
            _isAnimationsBlocked = false;
            return openedScreen;
        }


        [Button]
        public virtual void Close()
        {
            if(_screenState == ScreenState.Closed)
            {
                DebugPro.LogWarning($"Screen [{Id}] is already closed. Unnecessary method call.", this);
                return;
            }

            DebugPro.Log($"Screen [{Id}] closed.", this);

            _screenState = ScreenState.Closed;
            OnScreenFocus(false);
            PlayBaseClosingAnimation();

            OnClosed?.Invoke();
        }

        [Button]
        public virtual void CloseImmediately()
        {
            _isAnimationsBlocked = true;
            Close();
            _isAnimationsBlocked = false;
        }

        public virtual void WakeUp()
        {
            if(_screenState == ScreenState.WokenUp)
            {
                DebugPro.LogWarning($"Screen [{Id}] is already awake. Unnecessary method call.", this);
                return;
            }

            DebugPro.Log($"Screen [{Id}] woke up.", this);

            _screenState = ScreenState.WokenUp;
            OnScreenFocus(true);
            PlayBaseOpeningAnimation();
        }

        public virtual void Sleep()
        {
            if(_screenState == ScreenState.Asleep)
            {
                DebugPro.LogWarning($"Screen [{Id}] is already closed. Unnecessary method call.", this);
                return;
            }

            DebugPro.Log($"Screen [{Id}] fell asleep.", this);

            _screenState = ScreenState.Asleep;
            OnScreenFocus(false);
            PlayBaseClosingAnimation();
        }


        private void PlayBaseOpeningAnimation()
        {
            if(_isBaseOpeningAnimationAllowed && !_isAnimationsBlocked)
            {
                if(_canvasGroup == null && !TryGetComponent(out _canvasGroup))
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.alpha = 0;
                _canvasGroup.DOKill();
                _canvasGroup.DOFade(1, BaseAnimationFadeDuration).SetUpdate(true).SetLink(gameObject).OnComplete(() =>
                {
                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;
                });
            }

            gameObject.SetActive(true);
        }

        private void PlayBaseClosingAnimation()
        {
            if(_isBaseClosingAnimationAllowed && !_isAnimationsBlocked)
            {
                _canvasGroup ??= gameObject.AddComponent<CanvasGroup>();
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.DOKill();
                _canvasGroup.DOFade(0, BaseAnimationFadeDuration).SetUpdate(true).SetLink(gameObject).OnComplete(() => gameObject.SetActive(false));
                return;
            }

            gameObject.SetActive(false);
        }
    }
}
using NKLogger;
using Southbyte.LocalizationSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.StoreSystem
{
    public abstract class StoreItemViewBase : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] protected Image _iconImage;
        [SerializeField] protected TextMeshProUGUI _titleText;
        [SerializeField] protected TextMeshProUGUI _descriptionText;
        [SerializeField] protected TextMeshProUGUI _priceText;
        
        [Inject] private LocalizationManager _localizationManager;
        [Inject] private StoreManager _storeManager;
        
        protected StoreItemData _data;
        
        
        private void OnDestroy()
        {
            _localizationManager.OnLanguageChanged -= OnLanguageChanged;
            _storeManager.OnPurchaseSuccess -= OnPurchaseSuccess;
        }
        
        private void Awake()
        {
            _localizationManager.OnLanguageChanged += OnLanguageChanged;
            _storeManager.OnPurchaseSuccess += OnPurchaseSuccess;
        }
        
        
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if(_data == null)
            {
                DebugPro.Log($"Unable to purchase store item, data is null!");
                return;
            }
            
            _storeManager.Purchase(_data.id);
        }
        
        
        public virtual void Initialize(StoreItemData data, string price)
        {
            _data = data;
            
            _iconImage.sprite = _data.icon;
            if(!_data.iconHexColor.IsNullOrEmptyOrWhiteSpace())
                _iconImage.color = ColorUtility.TryParseHtmlString(_data.iconHexColor, out var color) ? color : Color.white;
            
            _titleText.text = _data.nameLocKey.Localize();
            _descriptionText.text = _data.descriptionLocKey.Localize();
            _priceText.text = price;
        }
        
        
        protected abstract void ProcessSuccessPurchase();
        
        
        private void OnLanguageChanged()
        {
            if(_data == null)
                return;
            
            _titleText.text = _data.nameLocKey.Localize();
            _descriptionText.text = _data.descriptionLocKey.Localize();
        }
        
        private void OnPurchaseSuccess(string purchaseId)
        {
            if(_data == null || _data.id != purchaseId)
                return;
            
            ProcessSuccessPurchase();
        }
    }
}
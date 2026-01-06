using NKLogger;
using TMPro;
using UnityEngine;
using Zenject;

namespace Southbyte.LocalizationSystem
{
    public class TextLocalizer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _localizationKey;
        [SerializeField] private bool _localizeOnAwake = true;

        [Inject] private LocalizationManager _localizationManager;
        
        public string LocalizationKey
        {
            get => _localizationKey; set => _localizationKey = value;
        }


        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Awake()
        {
            if(_localizeOnAwake)
                Localize();

            _localizationManager.OnLanguageChanged += Localize;
        }

        private void OnDestroy()
        {
            _localizationManager.OnLanguageChanged -= Localize;
        }


        public void Localize()
        {
            if(LocalizationKey.IsNullOrEmptyOrWhiteSpace())
            {
                DebugPro.LogError($"Unable to localize. Localization key is empty or null! Parent: {transform?.parent?.name}. Object: {name}", context: gameObject);
                enabled = false;
                return;
            }
            
            var localizedValue = LocalizationKey.Localize();
            _text.text = localizedValue.IsNullOrEmptyOrWhiteSpace() ? _text.text : localizedValue;
        }
    }
}
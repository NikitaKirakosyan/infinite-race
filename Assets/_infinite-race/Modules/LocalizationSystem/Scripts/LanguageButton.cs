using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.LocalizationSystem
{
    public class LanguageButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
        
        [Inject] private LocalizationManager _localizationManager;
        
        private int _languageIndex;
        private readonly List<string> _languageOptions = new ();
        
        private readonly string[] _languages = new []
        {
            LanguageCodes.EnCode,
            LanguageCodes.RuCode,
            LanguageCodes.FrCode,
            LanguageCodes.DeCode,
            LanguageCodes.ItCode,
            LanguageCodes.EsCode,
            LanguageCodes.TrCode,
        };
        
        
        private void Reset()
        {
            _button = GetComponent<Button>();
        }
        
        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClick);
            var currLangCode = _localizationManager.CurrentLanguageCode;
            
            for(var i = 0; i < _languages.Length; i++)
            {
                var langCode = _languages[i];
                var culture = CultureInfo.GetCultureInfo(langCode);
                _languageOptions.Add(culture.TextInfo.ToTitleCase(culture.NativeName));
                
                if(_languages[i] == currLangCode)
                    _languageIndex = i;
            }
            
            Refresh();
        }
        
        
        private void Refresh()
        {
            _buttonText.text = _languageOptions[_languageIndex];
        }
        
        private void OnButtonClick()
        {
            _languageIndex++;
            if(_languageIndex >= _languageOptions.Count)
                _languageIndex = 0;
            
            _localizationManager.SetLanguage(_languages[_languageIndex]);
            Refresh();
        }
    }
}
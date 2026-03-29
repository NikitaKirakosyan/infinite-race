using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte.LocalizationSystem
{
    public class LanguageButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private List<Sprite> _languageIcons;
        
        [Inject] private LocalizationManager _localizationManager;
        
        private int _languageIndex;
        
        private readonly string[] _languages = new []
        {
            LanguageCodes.EnCode,
            LanguageCodes.RuCode,
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
                
                if(_languages[i] == currLangCode)
                    _languageIndex = i;
            }
            
            Refresh();
        }
        
        
        private void Refresh()
        {
            _buttonImage.sprite = _languageIcons[_languageIndex];
        }
        
        private void OnButtonClick()
        {
            _languageIndex++;
            if(_languageIndex >= _languageIcons.Count)
                _languageIndex = 0;
            
            _localizationManager.SetLanguage(_languages[_languageIndex]);
            Refresh();
        }
    }
}
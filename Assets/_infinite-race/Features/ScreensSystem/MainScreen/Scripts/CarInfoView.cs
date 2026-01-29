using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Southbyte
{
    public class CarInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _valuesText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Slider _currentValueSlider;
        [SerializeField] private Slider _nextValueSlider;
        [SerializeField] private Button _buyButton;
        
        
        public void Setup(float currentValue, float maxValue)
        {
            _valuesText.text = $"{currentValue}/{maxValue}";
            
            _currentValueSlider.minValue = 0;
            _currentValueSlider.maxValue = maxValue;
            _currentValueSlider.value = currentValue;
            
            _priceText.gameObject.SetActive(false);
            _nextValueSlider.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(false);
        }
        
        public void Setup(float currentValue, float maxValue, float price, float nextValue)
        {
            Setup(currentValue, maxValue);
            
            _priceText.gameObject.SetActive(true);
            _priceText.text = $"${price}";
            
            _nextValueSlider.gameObject.SetActive(true);
            _nextValueSlider.minValue = 0;
            _nextValueSlider.maxValue = maxValue;
            _nextValueSlider.value = nextValue;
            
            _buyButton.gameObject.SetActive(true);
        }
    }
}
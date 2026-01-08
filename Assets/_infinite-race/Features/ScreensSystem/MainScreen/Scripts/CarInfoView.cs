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
        
        
        public void Setup()
        {
            
        }
    }
}
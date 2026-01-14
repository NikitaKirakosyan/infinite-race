using Southbyte.RaceSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Southbyte
{
    public class DayNightButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _dayColor = Color.yellow;
        [SerializeField] private Color _nightColor = Color.black;
        
        [Inject] private DayNightManager _dayNightManager;
        
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ToggleDayNight);
        }
        
        
        private void ToggleDayNight()
        {
            _dayNightManager.Toggle();
            _image.color = _dayNightManager.IsNight ? _nightColor : _dayColor;
        }
    }
}
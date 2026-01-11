using Southbyte.ScreensSystem;
using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class CarShowcaseController : MonoBehaviour
    {
        [Inject] private CarConfigsManager _carConfigsManager;
        [Inject] private IInstantiator _instantiator;
        
        private CarController _currentCar;
        
        
        private void Awake()
        {
            MainTab.OnCarIndexChanged += OnCarIndexChanged;
        }
        
        private void OnDestroy()
        {
            MainTab.OnCarIndexChanged -= OnCarIndexChanged;
        }
        
        
        private void OnCarIndexChanged(int selectedCarIndex)
        {
            var config = _carConfigsManager.CarConfigs[selectedCarIndex];
            
            if (_currentCar != null)
                Destroy(_currentCar.gameObject);
            
            _currentCar = _instantiator.InstantiatePrefabForComponent<CarController>(config.prefab, transform.position, Quaternion.identity, transform);
            Destroy(_currentCar.GetComponent<CarController>());
            _currentCar.transform.localEulerAngles = Vector3.zero;
        }
    }
}
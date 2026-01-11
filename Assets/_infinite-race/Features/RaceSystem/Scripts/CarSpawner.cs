using System;
using Southbyte.ScreensSystem;
using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class CarSpawner : MonoBehaviour
    {
        public event Action<CarController> OnCarSpawned;
        
        public CameraController cameraController;
        private CarController currentCar;
        
        [Inject] private IInstantiator _instantiator;
        [Inject] private GameManager _gameManager;
        [Inject] private CarConfigsManager _carConfigsManager;
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += Spawn;
        }
        
        
        public void Spawn()
        {
            var config = _carConfigsManager.CarConfigs[MainTab.SelectedCarIndex];
            var position = transform.position;
            
            if (currentCar != null)
            {
                position = currentCar.transform.position;
                Destroy(currentCar.gameObject);
            }
            
            currentCar = _instantiator.InstantiatePrefabForComponent<CarController>(config.prefab, position, Quaternion.identity, transform);
            cameraController.SetTarget(currentCar.transform);
            OnCarSpawned?.Invoke(currentCar);
        }
    }
}

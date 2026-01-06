using System;
using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class CarSpawner : MonoBehaviour
    {
        public event Action<CarController> OnCarSpawned;
        
        public CarConfig startCar;
        public CameraController cameraController;
        private CarController currentCar;
        
        [Inject] private IInstantiator _instantiator;
        
        
        void Start()
        {
            Spawn(startCar);
        }
        
        
        public void Spawn(CarConfig config)
        {
            if (currentCar != null)
                Destroy(currentCar);
            
            currentCar = _instantiator.InstantiatePrefabForComponent<CarController>(config.prefab, transform.position, Quaternion.identity, null);
            cameraController.SetTarget(currentCar.transform);
            OnCarSpawned?.Invoke(currentCar);
        }
    }
}

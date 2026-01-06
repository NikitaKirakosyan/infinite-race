using System;
using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class CarSpawner : MonoBehaviour
    {
        public event Action<CarController> OnCarSpawned;
        
        public CarConfig[] carConfigs;
        public CameraController cameraController;
        private CarController currentCar;
        
        [Inject] private IInstantiator _instantiator;
        
        
        void Start()
        {
            Spawn();
        }
        
        
        public void Spawn()
        {
            var config = carConfigs.GetRandomElement();
            var position = transform.position;
            
            if (currentCar != null)
            {
                position = currentCar.transform.position;
                Destroy(currentCar.gameObject);
            }
            
            currentCar = _instantiator.InstantiatePrefabForComponent<CarController>(config.prefab, position, Quaternion.identity, null);
            cameraController.SetTarget(currentCar.transform);
            OnCarSpawned?.Invoke(currentCar);
        }
    }
}

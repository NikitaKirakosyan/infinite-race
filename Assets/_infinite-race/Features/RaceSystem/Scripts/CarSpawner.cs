using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class PlayerSpawner : MonoBehaviour
    {
        public CarConfig startCar;
        public CameraController cameraController;
        private GameObject currentCar;
        
        [Inject] private IInstantiator _instantiator;
        
        
        void Start()
        {
            Spawn(startCar);
        }
        
        
        public void Spawn(CarConfig config)
        {
            if (currentCar != null)
                Destroy(currentCar);
            
            currentCar = _instantiator.InstantiatePrefab(config.prefab, transform.position, Quaternion.identity, null);
            cameraController.SetTarget(currentCar.transform);
        }
    }
}

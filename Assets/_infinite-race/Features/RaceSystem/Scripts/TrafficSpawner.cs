using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Southbyte.RaceSystem
{
    public class TrafficSpawner : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        
        public Transform player;
        public TrafficCar[] trafficPrefabs;
        public CarSpawner carSpawner;
        
        public float spawnDistance = 60f;
        public float spawnInterval = 1.2f;
        
        public float laneOffset = 2.5f;
        public int lanes = 3;
        
        private List<TrafficCar> _spawnedCars = new ();
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += StartSpawning;
            _gameManager.OnGameOver += StopSpawning;
            _gameManager.OnGameBraked += StopSpawning;
            
            carSpawner.OnCarSpawned += (car) => player = car.transform;
        }
        
        
        public void CleanTraffic()
        {
            foreach(var spawnedCar in _spawnedCars)
                Destroy(spawnedCar.gameObject);
            
            _spawnedCars.Clear();
        }
        
        
        float GetLaneX(int laneIndex, int lanes, float laneOffset)
        {
            if (lanes == 1)
                return 0f;
            
            var half = (lanes - 1) * 0.5f;
            return (laneIndex - half) * laneOffset;
        }
        
        void SpawnTraffic()
        {
            var lane = Random.Range(0, lanes);
            var x = GetLaneX(lane, lanes, laneOffset);
            
            var oncoming = lane == 0;
            
            var pos = player.position + Vector3.forward * spawnDistance;
            if (oncoming)
                pos += Vector3.forward * 40f;
            
            pos.x = x;
            var car = Instantiate(trafficPrefabs[Random.Range(0, trafficPrefabs.Length)], pos, Quaternion.identity);
            
            var tc = car.GetComponent<TrafficCar>();
            tc.IsOncoming = oncoming;
            _spawnedCars.Add(tc);
        }
        
        private IEnumerator SpawnTrafficRoutine()
        {
            while(true)
            {
                SpawnTraffic();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        
        private void StartSpawning()
        {
            CleanTraffic();
            StartCoroutine(SpawnTrafficRoutine());
        }
        
        private void StopSpawning()
        {
            StopAllCoroutines();
        }
    }
}
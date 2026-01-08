using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Southbyte.RaceSystem
{
    public class TrafficSpawner : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        
        public Transform player;
        public GameObject[] trafficPrefabs;
        public CarSpawner carSpawner;
        
        public float spawnDistance = 120f;
        public float spawnInterval = 1.2f;
        
        public float laneOffset = 2.5f;
        public int lanes = 3;
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += StartSpawning;
            _gameManager.OnGameOver += StopSpawning;
            
            carSpawner.OnCarSpawned += (car) => player = car.transform;
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
            tc.isOncoming = oncoming;
            tc.speed = Random.Range(8f, 14f);
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
            StartCoroutine(SpawnTrafficRoutine());
        }
        
        private void StopSpawning()
        {
            StopAllCoroutines();
        }
    }
}
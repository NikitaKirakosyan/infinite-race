using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Southbyte.RaceSystem
{
    public class TrafficSpawner : MonoBehaviour
    {
        public Transform player;
        public GameObject[] trafficPrefabs;
        public CarSpawner carSpawner;
        
        public float spawnDistance = 120f;
        public float spawnInterval = 1.2f;
        
        public float laneOffset = 2.5f;
        public int lanes = 3;
        
        private float timer;
        
        
        private void Awake()
        {
            carSpawner.OnCarSpawned += (car) => player = car.transform;
        }
        
        void Update()
        {
            timer += Time.deltaTime;
            if (timer < spawnInterval) return;
            timer = 0f;
            
            SpawnTraffic();
        }
        
        
        float GetLaneX(int laneIndex, int lanes, float laneOffset)
        {
            if (lanes == 1)
                return 0f;
            
            float half = (lanes - 1) * 0.5f;
            return (laneIndex - half) * laneOffset;
        }
        
        void SpawnTraffic()
        {
            int lane = Random.Range(0, lanes);
            float x = GetLaneX(lane, lanes, laneOffset);
            
            bool oncoming = lane == 0;
            
            Vector3 pos = player.position + Vector3.forward * spawnDistance;
            if (oncoming)
                pos += Vector3.forward * 40f;
            
            pos.x = x;
            
            GameObject car = Instantiate(
                trafficPrefabs[Random.Range(0, trafficPrefabs.Length)],
                pos,
                Quaternion.identity
            );
            
            TrafficCar tc = car.GetComponent<TrafficCar>();
            tc.isOncoming = oncoming;
            tc.speed = Random.Range(8f, 14f);
        }
    }
}
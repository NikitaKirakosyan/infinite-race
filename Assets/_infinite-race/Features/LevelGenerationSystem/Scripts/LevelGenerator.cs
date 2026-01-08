using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Southbyte.RaceSystem;
using Zenject;

namespace Southbyte.LevelGenerationSystem
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        
        [Inject] private GameManager _gameManager;
        
        public Transform player;
        public CarSpawner carSpawner;
        public GameObject[] roadTiles;
        public float tileLength = 40f;
        public int tilesAhead = 6;
        
        private float spawnZ = 0f;
        private Queue<GameObject> spawned = new Queue<GameObject>();
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += StartSpawning;
            _gameManager.OnGameOver += StopSpawning;
            carSpawner.OnCarSpawned += (car) => player = car.transform;
        }
        
        
        void SpawnTile()
        {
            var tile = Instantiate(roadTiles.GetRandomElement(), Vector3.forward * spawnZ, Quaternion.identity);
            tile.transform.SetParent(_container);
            
            spawned.Enqueue(tile);
            spawnZ += tileLength;
            
            if (spawned.Count > tilesAhead + 2)
                Destroy(spawned.Dequeue());
        }
        
        private IEnumerator SpawnTileRoutine()
        {
            while(true)
            {
                yield return new WaitUntil(() => player.position.z + tilesAhead * tileLength > spawnZ);
                SpawnTile();
            }
        }
        
        private void StartSpawning()
        {
            _container.gameObject.SetActive(true);
            
            for (var i = 0; i < tilesAhead; i++)
                SpawnTile();
            
            StartCoroutine(SpawnTileRoutine());
        }
        
        private void StopSpawning()
        {
            _container.gameObject.SetActive(false);
            
            for(var i = spawned.Count - 1; i >= 0; i--)
                Destroy(spawned.Dequeue());
            
            StopAllCoroutines();
        }
    }
}
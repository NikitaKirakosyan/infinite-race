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
        
        private float spawnZ;
        private Queue<GameObject> spawned = new Queue<GameObject>();
        
        
        private void Awake()
        {
            _gameManager.OnGameStarted += StartSpawning;
            _gameManager.OnGameOver += StopSpawning;
            _gameManager.OnGameBraked += StopSpawning;
            
            carSpawner.OnCarSpawned += (car) => player = car.transform;
        }
        
        
        public void SetLevelActive(bool value)
        {
            _container.gameObject.SetActive(value);
        }
        
        
        private void SpawnTile()
        {
            var tile = Instantiate(roadTiles.GetRandomElement(), Vector3.forward * spawnZ, Quaternion.identity);
            tile.transform.SetParent(_container);
            var pos = tile.transform.localPosition;
            pos.x = 0;
            tile.transform.localPosition = pos;
            
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
            for(var i = spawned.Count - 1; i >= 0; i--)
                Destroy(spawned.Dequeue());
            
            _container.gameObject.SetActive(true);
            spawnZ = 0;
            
            for (var i = 0; i < tilesAhead; i++)
                SpawnTile();
            
            StartCoroutine(SpawnTileRoutine());
        }
        
        private void StopSpawning()
        {
            StopAllCoroutines();
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace Southbyte.LevelGenerationSystem
{
    public class LevelGenerator : MonoBehaviour
    {
        public Transform player;
        [SerializeField] private Transform _container;
        public GameObject[] roadTiles;
        public float tileLength = 40f;
        public int tilesAhead = 6;
        
        private float spawnZ = 0f;
        private Queue<GameObject> spawned = new Queue<GameObject>();
        
        
        void Start()
        {
            for (int i = 0; i < tilesAhead; i++)
                SpawnTile();
        }
        
        void Update()
        {
            if (player.position.z + tilesAhead * tileLength > spawnZ)
                SpawnTile();
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
    }
}
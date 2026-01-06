using UnityEngine;

namespace Southbyte.RaceSystem
{
    [CreateAssetMenu(fileName = nameof(CarConfig), menuName = EditorMenuNames.RaceSystemRoot + "Car Config")]
    public class CarConfig : ScriptableObject
    {
        [Header("Visual")]
        public GameObject prefab;
        
        [Header("Stats")]
        public float minSpeed = 40f;
        public float maxSpeed = 40f;
        public float acceleration = 15f;
        public float deceleration = 25f;
        public float steerSpeed = 6f;
        public float laneLimit = 4f;
        
        [Header("Score")]
        public float scoreMultiplier = 1f;
    }

}

using UnityEngine;

namespace Southbyte.RaceSystem
{
    [CreateAssetMenu(fileName = nameof(CarConfig), menuName = EditorMenuNames.RaceSystemRoot + "Car Config")]
    public class CarConfig : ScriptableObject
    {
        public const float MinSpeed = 18f;
        
        public string carId;
        public string carNameLocalizationKey;
        public int price = 1000;
        public GameObject prefab;
        
        [Header("Base Stats")]
        public float baseMaxSpeed = 20; // 20 ~ 72 km/h (20*3.6)
        public float basePower = 10;
        public float baseBrake = 10;
        public float baseHandling = 5;
        
        [Header("Max Stats")]
        public float maxMaxSpeed = 85;
        public float maxPower = 20;
        public float maxBrake = 30;
        public float maxHandling = 10;
        
        [Header("Upgrade step")]
        public float speedStep = 2f;
        public float powerStep = 1.5f;
        public float brakeStep = 2;
        public float handlingStep = 1;
        
        [Header("Score")]
        public float scoreMultiplier = 1f;
    }
}
using Southbyte.DIConfiguration;
using Southbyte.RaceSystem;
using UnityEngine;
using Zenject;

namespace Southbyte
{
    [EarlyInitialization]
    public class ScoreManager
    {
        public CarController player => Object.FindFirstObjectByType<CarController>();
        public float scoreMultiplier = 1f;
        
        public float Distance => player.transform.position.z;
        public int Score => Mathf.FloorToInt(Distance * scoreMultiplier);
        
        
        public void SetMultiplier(float multiplier)
        {
            scoreMultiplier = multiplier;
        }
    }
}
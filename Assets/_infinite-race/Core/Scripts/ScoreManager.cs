using Southbyte.DIConfiguration;
using UnityEngine;

namespace Southbyte
{
    [EarlyInitialization]
    public class ScoreManager
    {
        public Transform player;
        public float scoreMultiplier = 1f;
        
        public float Distance => player.position.z;
        public int Score => Mathf.FloorToInt(Distance * scoreMultiplier);
        
        
        public void SetMultiplier(float multiplier)
        {
            scoreMultiplier = multiplier;
        }
    }
}
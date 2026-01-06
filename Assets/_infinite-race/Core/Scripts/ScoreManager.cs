using UnityEngine;

namespace Southbyte
{
    public class ScoreManager : MonoBehaviour
    {
        public Transform player;
        public float scoreMultiplier = 1f;
        
        public float Distance { get; private set; }
        public int Score { get; private set; }
        
        
        private void Update()
        {
            Distance = player.position.z;
            Score = Mathf.FloorToInt(Distance * scoreMultiplier);
        }
    }
}
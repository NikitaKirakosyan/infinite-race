using UnityEngine;
using Zenject;

namespace Southbyte.RaceSystem
{
    public class CloseMissDetector : MonoBehaviour
    {
        private const float MissDistance = 6f;
        private const int MissScore = 50;
        
        [Inject] private ScoreManager _scoreManager;
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Traffic"))
                return;
            
            var d = Vector3.Distance(transform.position, other.transform.position);
            if (d <= MissDistance)
            {
                _scoreManager.AddScore(MissScore);
                FindFirstObjectByType<CloseMissBonusText>().Show(MissScore);
            }
        }
    }
}
using UnityEngine;

namespace Southbyte.RaceSystem
{
    public class TrafficCar : MonoBehaviour
    {
        public bool IsOncoming { get; set; }
        
        private void Start()
        {
            transform.localEulerAngles = new Vector3(0, IsOncoming ? 180 : 0, 0);
        }
    }
}

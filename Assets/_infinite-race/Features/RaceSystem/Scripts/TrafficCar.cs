using System;
using UnityEngine;

namespace Southbyte.RaceSystem
{
    public class TrafficCar : MonoBehaviour
    {
        public float Speed { get; set; }
        public bool IsOncoming { get; set; }
        
        private void Start()
        {
            transform.localEulerAngles = new Vector3(0, IsOncoming ? 180 : 0, 0);
        }
        
        void Update()
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
    }
}

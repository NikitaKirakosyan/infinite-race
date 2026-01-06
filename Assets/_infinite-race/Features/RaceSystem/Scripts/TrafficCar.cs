using System;
using UnityEngine;

namespace Southbyte.RaceSystem
{
    public class TrafficCar : MonoBehaviour
    {
        public float speed;
        public bool isOncoming;
        
        private void Start()
        {
            transform.localEulerAngles = new Vector3(0, isOncoming ? 180 : 0, 0);
        }
        
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}

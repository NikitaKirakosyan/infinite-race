using UnityEngine;

namespace Southbyte
{
    public static class VectorExtensions
    {
        public static Vector3 Randomize(this Vector3 vector3)
        {
            return new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f));
        }
        
        public static Vector2 Randomize(this Vector2 vector3)
        {
            return new Vector2(Random.Range(-360f, 360f), Random.Range(-360f, 360f));
        }
        
        public static float DistanceOptimized(this Vector3 origin, Vector3 targetPosition)
        {
            Vector3 heading;
            heading.x = origin.x - targetPosition.x;
            heading.y = origin.y - targetPosition.y;
            heading.z = origin.z - targetPosition.z;
            
            var distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;
            var distance = Mathf.Sqrt(distanceSquared);
            
            return distance;
        }
    }
}
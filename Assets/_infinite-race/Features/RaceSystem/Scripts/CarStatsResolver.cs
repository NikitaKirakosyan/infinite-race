using UnityEngine;

namespace Southbyte.RaceSystem
{
    public static class CarStatsResolver
    {
        public static float MaxSpeed(CarConfig c, CarProgress p)
        {
            return Mathf.Min(
                c.baseMaxSpeed + p.speed * c.speedStep,
                c.maxMaxSpeed
            );
        }
        
        public static float Power(CarConfig c, CarProgress p)
        {
            return Mathf.Min(
                c.basePower + p.power * c.powerStep,
                c.maxPower
            );
        }
        
        public static float Brake(CarConfig c, CarProgress p)
        {
            return Mathf.Min(
                c.baseBrake + p.brake * c.brakeStep,
                c.maxBrake
            );
        }
        
        public static float Handling(CarConfig c, CarProgress p)
        {
            return Mathf.Min(
                c.baseHandling + p.handling * c.handlingStep,
                c.maxHandling
            );
        }
    }
}

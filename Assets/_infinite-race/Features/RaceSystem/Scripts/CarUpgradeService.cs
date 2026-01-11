namespace Southbyte.RaceSystem
{
    public static class CarUpgradeService
    {
        public static bool UpgradeSpeed(CarConfig c, CarProgress p)
        {
            if (CarStatsResolver.MaxSpeed(c, p) >= c.maxMaxSpeed)
                return false;
            
            p.speed++;
            return true;
        }
        
        public static bool UpgradePower(CarConfig c, CarProgress p)
        {
            if (CarStatsResolver.Power(c, p) >= c.maxPower)
                return false;
            
            p.power++;
            return true;
        }
        
        public static bool UpgradeBrake(CarConfig c, CarProgress p)
        {
            if (CarStatsResolver.Brake(c, p) >= c.maxBrake)
                return false;
            
            p.brake++;
            return true;
        }
        
        public static bool UpgradeHandling(CarConfig c, CarProgress p)
        {
            if (CarStatsResolver.Handling(c, p) >= c.maxHandling)
                return false;
            
            p.handling++;
            return true;
        }
    }
}
using System.Collections.Generic;
using Southbyte.DIConfiguration;
using Southbyte.RaceSystem;

namespace Southbyte
{
    [EarlyInitialization]
    public class CarProgressManager
    {
        private Dictionary<string, CarProgress> _progressById = new ();
        
        
        public CarProgress Get(string carId)
        {
            if (!_progressById.TryGetValue(carId, out var progress))
            {
                progress = new CarProgress();
                _progressById.Add(carId, progress);
            }
            
            return progress;
        }
        
        public bool Upgrade(string carId, CarStatType type, CarConfig config)
        {
            var p = Get(carId);
            
            switch (type)
            {
                case CarStatType.Speed:
                    if (CarStatsResolver.MaxSpeed(config, p) >= config.maxMaxSpeed)
                        return false;
                    p.speed++;
                    break;
                
                case CarStatType.Power:
                    if (CarStatsResolver.Power(config, p) >= config.maxPower)
                        return false;
                    p.power++;
                    break;
                
                case CarStatType.Brake:
                    if (CarStatsResolver.Brake(config, p) >= config.maxBrake)
                        return false;
                    p.brake++;
                    break;
                
                case CarStatType.Handling:
                    if (CarStatsResolver.Handling(config, p) >= config.maxHandling)
                        return false;
                    p.handling++;
                    break;
            }
            
            return true;
        }
    }
}

using Southbyte.DIConfiguration;
using Southbyte.RaceSystem;
using UnityEngine;
using YG;

namespace Southbyte
{
    [EarlyInitialization]
    public class CarProgressManager
    {
        private const float UpgradePriceCarPricePart = 0.1f;


        public CarProgress Get(string carId)
        {
            if(YG2.saves.carProgress == null)
                YG2.saves.carProgress = new ();

            for(var i = 0; i < YG2.saves.carProgress.Count; i++)
            {
                var savedProgress = YG2.saves.carProgress[i];
                if(savedProgress.Key != carId)
                    continue;

                if(savedProgress.Value == null)
                {
                    savedProgress.Value = new CarProgress();
                    YG2.saves.carProgress[i] = savedProgress;
                }

                return savedProgress.Value;
            }

            var progress = new CarProgress();
            YG2.saves.carProgress.Add(new SerializablePair<string, CarProgress>(carId, progress));
            return progress;
        }

        public bool Upgrade(string carId, CarStatType type, CarConfig config)
        {
            var p = Get(carId);

            if(!CanUpgrade(config, p, type))
                return false;

            switch (type)
            {
                case CarStatType.Speed:
                    p.speed++;
                    break;

                case CarStatType.Power:
                    p.power++;
                    break;

                case CarStatType.Brake:
                    p.brake++;
                    break;

                case CarStatType.Handling:
                    p.handling++;
                    break;
            }

            return true;
        }

        public static bool CanUpgrade(CarConfig config, CarProgress progress, CarStatType type)
        {
            switch(type)
            {
                case CarStatType.Speed:
                    return CarStatsResolver.MaxSpeed(config, progress) < config.maxMaxSpeed;

                case CarStatType.Power:
                    return CarStatsResolver.Power(config, progress) < config.maxPower;

                case CarStatType.Brake:
                    return CarStatsResolver.Brake(config, progress) < config.maxBrake;

                case CarStatType.Handling:
                    return CarStatsResolver.Handling(config, progress) < config.maxHandling;

                default:
                    return false;
            }
        }

        public static int GetUpgradePrice(CarConfig config, CarProgress progress, CarStatType type)
        {
            var basePrice = Mathf.Max(1, Mathf.RoundToInt(config.price * UpgradePriceCarPricePart));
            return basePrice * (GetLevel(progress, type) + 1);
        }

        private static int GetLevel(CarProgress progress, CarStatType type)
        {
            switch(type)
            {
                case CarStatType.Speed:
                    return progress.speed;

                case CarStatType.Power:
                    return progress.power;

                case CarStatType.Brake:
                    return progress.brake;

                case CarStatType.Handling:
                    return progress.handling;

                default:
                    return 0;
            }
        }
    }
}

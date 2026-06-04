using System.Collections.Generic;
using Southbyte;
using Southbyte.CurrenciesSystem;
using Southbyte.RaceSystem;
using Southbyte.StoreSystem;

namespace YG
{
    public partial class SavesYG
    {
        public string lastReleaseVersionViewed;
        public List<SerializablePair<CurrencyType, int>> currencies = new ();
        public StoreSaveData storeSaveData = new ();
        public string selectedCarId = "pickup_car";
        public float bestScore;
        public float bestDistance;
        public List<string> purchasedCarIds = new () { "pickup_car"};
        public List<SerializablePair<string, CarProgress>> carProgress = new ();
    }
}

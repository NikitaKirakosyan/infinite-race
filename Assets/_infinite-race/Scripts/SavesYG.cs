using System.Collections.Generic;
using Southbyte;
using Southbyte.CurrenciesSystem;
using Southbyte.StoreSystem;

namespace YG
{
    public partial class SavesYG
    {
        public string lastReleaseVersionViewed;
        public List<SerializablePair<CurrencyType, int>> currencies = new ();
        public StoreSaveData storeSaveData = new ();
        public string selectedCarId;
        public float bestScore;
        public float bestDistance;
    }
}
using System;
using System.Collections.Generic;

namespace Southbyte.StoreSystem
{
    [Serializable]
    public class StoreSaveData
    {
        public bool wasPurchaseBonusUsed;
        public List<string> purchasedStoreItemIds = new ();
    }
}
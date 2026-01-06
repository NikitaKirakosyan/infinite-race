using System;
using System.Collections.Generic;
using HandyEditorExtensions;
using Southbyte.CurrenciesSystem;
using UnityEngine;

namespace Southbyte.StoreSystem
{
    [CreateAssetMenu(fileName = "StoreItemResources", menuName = EditorMenuNames.StoreSystemRoot + "Store Item Resources")]
    public class StoreItemResources : ScriptableObject
    {
        [SerializeField] private StoreItemData[] _shopItems;
        
        private Dictionary<string, StoreItemData> _storeItemsById;
        
        
        public bool TryGetStoreItemData(string id, out StoreItemData storeItemData)
        {
            if(_storeItemsById == null)
            {
                _storeItemsById = new Dictionary<string, StoreItemData>();
                foreach(var shopItem in _shopItems)
                    _storeItemsById.Add(shopItem.id, shopItem);
            }
            
            return _storeItemsById.TryGetValue(id, out storeItemData);
        }
    }
    
    [Serializable]
    public class StoreItemData
    {
        [Dropdown(typeof(StoreItemIds))] public string id;
        public string nameLocKey;
        public string descriptionLocKey;
        public string iconHexColor;
        public Sprite icon;
        public StoreItemPurchaseType purchaseType;
        
        [Header("Rewards")]
        public List<SerializablePair<CurrencyType, int>> currencyRewards;
    }
}
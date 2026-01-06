using System;
using System.Collections;
using System.Collections.Generic;
using HandyEditorExtensions;
using UnityEngine;

namespace Southbyte.StoreSystem
{
    [CreateAssetMenu(fileName = "StoreItemViewsSettings", menuName = EditorMenuNames.StoreSystemRoot + "Store Item Views Settings")]
    public class StoreItemViewsSettings : ScriptableObject, IEnumerable
    {
        [SerializeField] private List<StoreScreenItemViewData> _storeScreenItemViews = new ();
        
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _storeScreenItemViews.GetEnumerator();
        }
    }
    
    [Serializable]
    public struct StoreScreenItemViewData
    {
        [Dropdown(typeof(StoreItemIds))] public string id;
        public StoreItemViewBase prefab;
    }
}
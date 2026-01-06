using HandyEditorExtensions;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Southbyte.AdsSystem
{
    [CreateAssetMenu(fileName = "AdsConfig", menuName = EditorMenuNames.AdsSystemRoot + "Ads Config")]
    public class AdsConfig : ScriptableObject
    {
        [SerializeField] private AdConfigItemData[] _adItems;
        
        
        public bool TryGetItem(string id, out AdConfigItemData item)
        {
            item = null;
            
            foreach(var data in _adItems)
            {
                if(data.id == id)
                {
                    item = data;
                    return true;
                }
            }
            
            return false;
        }
        
        
#if UNITY_EDITOR
        [Button]
        private void ToJson()
        {
            var json = JsonConvert.SerializeObject(_adItems, Formatting.Indented);
            EditorGUIUtility.systemCopyBuffer = json;
        }
#endif
    }
}
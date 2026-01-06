using System;
using UnityEngine;

namespace Southbyte.CurrenciesSystem
{
    [CreateAssetMenu(fileName = "CurrenciesConfig", menuName = EditorMenuNames.CurrencySystemRoot + "Currencies Config")]
    public class CurrenciesConfig : ScriptableObject
    {
        public Vector3 punchScale = new (0.1f, 0.1f, 0.1f);
        public float punchDuration = 0.3f;
        public int vibrato = 10;
        public float elasticity = 1;
        public CurrencySettings[] currencySettings;
        
        
        public bool TryGetSettings(CurrencyType type, out CurrencySettings result)
        {
            result = default;
            
            foreach(var settings in currencySettings)
            {
                if(settings.type == type)
                {
                    result = settings;
                    return true;
                }
            }
            
            return false;
        }
    }
    
    [Serializable]
    public struct CurrencySettings
    {
        public CurrencyType type;
        public Sprite icon;
    }
}
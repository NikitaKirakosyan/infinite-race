using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Southbyte.UIModules
{
    [Serializable]
    public class TabToggle<T> where T : Object
    {
        public T Tab;
        public Toggle Toggle;
        
        public bool IsOn
        {
            get => Toggle.isOn;
            set => Toggle.isOn = value;
        }
    }
}
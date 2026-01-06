using System;
using HandyEditorExtensions;

namespace Southbyte.AdsSystem
{
    [Serializable]
    public class AdConfigItemData
    {
        [Dropdown(typeof(AdPlacementIds))] public string id;
        public float secondsDelay;
        public int actionsDelay;
    }
}
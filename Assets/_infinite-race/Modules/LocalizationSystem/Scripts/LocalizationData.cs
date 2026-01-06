using System.Collections.Generic;
using Newtonsoft.Json;

namespace Southbyte.LocalizationSystem
{
    public class LocalizationData
    {
        [JsonProperty("localization")]
        public Dictionary<string, Dictionary<string, string>> translationDictionary;
    }
}
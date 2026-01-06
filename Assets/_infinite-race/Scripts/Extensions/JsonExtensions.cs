using Newtonsoft.Json;

namespace Southbyte
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj, bool formatted = false)
        {
            var formatting = formatted ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(obj, formatting);
        }

        public static T FromJson<T>(this string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
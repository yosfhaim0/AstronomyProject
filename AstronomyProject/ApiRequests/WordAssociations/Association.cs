using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ApiRequests.WordAssociations
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Pos
    {
        [JsonProperty("noun")]
        Noun = 0,
        [JsonProperty("adjective")]
        Adjective = 1,
        [JsonProperty("verb")]
        Verb = 2,
        [JsonProperty("adverb")]
        Adverb = 3,
    }

    public class Association
    {
        [JsonProperty("item")]
        public string Word { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("pos")]
        public Pos Pos { get; set; }
    }
}
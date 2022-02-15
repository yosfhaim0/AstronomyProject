using Newtonsoft.Json;
using System;

namespace Models
{
    public record Satellite
    {
        [JsonProperty("satelliteId")]
        public int SatelliteId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("line1")]
        public string Line1 { get; set; }

        [JsonProperty("line2")]
        public string Line2 { get; set; }
    }
}

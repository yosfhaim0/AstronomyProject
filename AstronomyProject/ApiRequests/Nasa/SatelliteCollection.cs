using Models;
using Models.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace ApiRequests.Nasa
{
    internal class SatelliteCollection
    {
        [JsonProperty("member")]
        public List<Satellite> Satellites { get; set; }
    }

    internal class NearAstridCollection
    {
        [JsonProperty("near_earth_objects")]
        public Dictionary<string, List<GetNANasaDto>> NearAstroids { get; set; }
    }
    public class Item
    {
        [JsonProperty("href")]
        public string href { get; set; }
    }

    public class Collection
    {
        [JsonProperty("version")]
        public string version { get; set; }
        [JsonProperty("href")]
        public string href { get; set; }

        [JsonProperty("items")]
        public List<Item> items { get; set; }
    }

    public class Root
    {
        [JsonProperty("collection")]
        public Collection collection { get; set; }
    }
}

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
}

using Models;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace DataAccess.ApiRequests.Nasa
{
    internal class SatelliteCollection
    {
        [JsonProperty("member")]
        public List<Satellite> Satellites { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class GetAPODNasaDto
    {
        [JsonProperty("url")]
        public string ImageUrl { get; set; }

        [JsonProperty("explanation")]
        public string Explanation { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }
    }
}

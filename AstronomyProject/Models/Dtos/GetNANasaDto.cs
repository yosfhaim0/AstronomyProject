using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public record GetNANasaDto
    {
        [JsonProperty("neo_reference_id")]
        public string NeoReferenceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nasa_jpl_url")]
        public string NasaJplUrl { get; set; }

        [JsonProperty("absolute_magnitude_h")]
        public double AbsoluteMagnitudeH { get; set; }

        [JsonProperty("estimated_diameter")]
        public EstimatedDiameterDto EstimatedDiameterDto { get; set; }

        [JsonProperty("is_potentially_hazardous_asteroid")]
        public bool IsPotentiallyHazardousAsteroid { get; set; }


        [JsonProperty("is_sentry_object")]
        public bool IsSentryObject { get; set; }

        public double EstimatedDiameterMax { get => EstimatedDiameterDto.meters.estimated_diameter_max; }

        public double EstimatedDiameterMin { get => EstimatedDiameterDto.meters.estimated_diameter_min; }

    }
}

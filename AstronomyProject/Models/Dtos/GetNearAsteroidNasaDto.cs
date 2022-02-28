using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public record GetNearAsteroidNasaDto
    {
        public Links links { get; set; }

        [JsonProperty("neo_reference_id")]
        public int Id { get; set; }

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

        public List<CloseApproachDto> close_approach_data { get; set; }
        
        public List<CloseApproach> CloseApproachData
        {
            get
            {
                if(close_approach_data == null)
                {
                    return null;
                }
                return (from a in close_approach_data
                       select a.CopyPropertiesToNew(typeof(CloseApproach)) 
                       as CloseApproach)
                       .ToList();
            }
        }

        public string NasaUrlQuery { get => links.self; }

        public double EstimatedDiameterMax { get => EstimatedDiameterDto.meters.estimated_diameter_max; }

        public double EstimatedDiameterMin { get => EstimatedDiameterDto.meters.estimated_diameter_min; }

    }

    public class Links
    {
        public string self { get; set; }
    }

    public class CloseApproachDto
    {
        [JsonProperty("close_approach_date")]
        public DateTime CloseApproachDate { get; set; }

        public RelativeVelocity relative_velocity { get; set; }
        public MissDistance miss_distance { get; set; }

        [JsonProperty("orbiting_body")]
        public string OrbitingBody { get; set; }

        public double RelativeVelocity { get => relative_velocity.kilometers_per_second; }

        public double MissDistance { get => miss_distance.kilometers; }
    }

    public class RelativeVelocity
    {
        public double kilometers_per_second { get; set; }
    }

    public class MissDistance
    {
        public double kilometers { get; set; }
    }
}

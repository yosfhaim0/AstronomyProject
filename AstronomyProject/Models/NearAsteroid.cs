using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public record NearAsteroid
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [Required]
        public double AbsoluteMagnitudeH { get; set; }

        [Required]
        public bool IsPotentiallyHazardousAsteroid { get; set; }

        [Required]
        public bool IsSentryObject { get; set; }

        [Required]
        public double EstimatedDiameterMax { get; set; }

        [Required]
        public double EstimatedDiameterMin { get; set; }

        public List<CloseApproach> CloseApproachData { get; set; }

        [MaxLength(500)]
        [Required]
        public string NasaUrlQuery { get; set; }
    }
}

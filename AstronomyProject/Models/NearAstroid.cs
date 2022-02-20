using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public record NearAstroid
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string NeoReferenceId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string NasaJplUrl { get; set; }

        public double AbsoluteMagnitudeH { get; set; }

        [Required]
        public bool IsPotentiallyHazardousAsteroid { get; set; }

        [Required]
        public bool IsSentryObject { get; set; }

        [Required]
        public double EstimatedDiameterMax { get; set; }

        [Required]
        public double EstimatedDiameterMin { get; set; }

    }
}

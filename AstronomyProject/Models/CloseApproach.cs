using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public record CloseApproach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CloseApproachDate { get; set; }

        /// <summary>
        /// Relative velocity, kilometers per second
        /// </summary>
        [Required]
        public double RelativeVelocity { get; set; }

        /// <summary>
        /// Miss distance in kilometers
        /// </summary>
        [Required]
        public double MissDistance { get; set; }

        [MaxLength(50)]
        [Required]
        public string OrbitingBody { get; set; }

        public int NearAsteroidId { get; set; }
    }
}

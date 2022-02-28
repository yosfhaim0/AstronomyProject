using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class CloseApproach
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string NeoReferenceId { get; set; }

        [Required]
        public DateTime CloseApproachDate { get; set; }

        [Required]
        public double RelativeVelocity { get; set; }

        [Required]
        public double MissDistance { get; set; }

        [MaxLength(50)]
        [Required]
        public string OrbitingBody { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public record MediaGroupe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Url { get; set; }

        [Required]
        [MaxLength(500)]
        public string PreviewUrl { get; set; }

        [Required]
        [MaxLength(1000)]
        public string  Title { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(10)")]
        public string MediaType { get; set; }

        public List<MediaItem> MediaItems { get; set; } = new();

        public List<ImaggaTag> Tags { get; set; } = new();

        public List<SearchWordModel> SearchWords { get; set; } = new();
    }
}


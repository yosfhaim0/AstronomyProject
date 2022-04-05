using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class MediaItem
    {
       
        [Key]
        public int Id { get; set; }
        [MaxLength(700)]
        [Required]
        public string Url { get; set; }

    }
}
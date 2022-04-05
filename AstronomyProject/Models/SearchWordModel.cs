using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class SearchWordModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string SearchWord { get; set; }

        public int MediaGroupeId { get; set; }

    }
}
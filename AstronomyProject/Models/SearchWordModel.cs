using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class SearchWordModel
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string SearchWord { get; set; }

        [ForeignKey("Media")]
        public int MediaGroupeId { get; set; }

    }
}
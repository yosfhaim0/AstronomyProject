using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public record ImageOfTheDay
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Url { get; set; }

        [MaxLength(5000)]
        public string Explanation { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }

        //public string 

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(10)")]
        public string MediaType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public record Media
    {   
        [Key]
        [MaxLength(500)]
        public string Url { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public record ImaggaTag
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Tag { get; set; }

        [Required]
        public double Confidence { get; set; }

        [ForeignKey("Media")]
        public int MediaGroupeId { get; set; }
    }
}

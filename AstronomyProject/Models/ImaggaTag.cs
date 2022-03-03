using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ImaggaTag
    {
        public int Id { get; set; }

        public string Tag { get; set; }

        public double Confidence { get; set; }
    }
}

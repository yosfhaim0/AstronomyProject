using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class Datum
    {
        public string description { get; set; }
        public string title { get; set; }
        public string photographer { get; set; }
        public List<string> keywords { get; set; }
        public string location { get; set; }
        public string nasa_id { get; set; }
        public string media_type { get; set; }
        public DateTime date_created { get; set; }
        public string center { get; set; }
        public List<string> album { get; set; }
        public string secondary_creator { get; set; }
    }

    public class Link
    {
        public string href { get; set; }

        public string  rel { get; set; }
    }

    public class MediaItemDto
    {
        public string href { get; set; }
        public List<Datum> data { get; set; }
        public List<Link> links { get; set; }
    }

    public class Collection
    {
        public List<MediaItemDto> items { get; set; }
    }

    public class MediasJsonToCS
    {
        public Collection collection { get; set; }
    }
    public class Harf
    {
        public List<string> MyArray { get; set; }
    }


}

using System.Collections.Generic;

namespace Models
{
    public record Media
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string FirebaseName { get; set; }
        public List<string> Links { get; set; }
        public string Discription { get; set; }
        public List<ImaggaTag> ImaggaTags { get; set; }
        public List<SearchWordModel> SearchWords { get; set;}
    }
}


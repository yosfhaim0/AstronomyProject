using System.Collections.Generic;

namespace Models
{
    public class SearchWordModel
    {
        public int Id { get; set; }

        public string SearchWord { get; set; }

        public List<SearchWordModel> ReletedWords { get; set; }

        public List<ImaggaTag> ImaggaTags { get; set; }

    }
}
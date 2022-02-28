using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRequests.Imagga
{
    public class Tag2
    {
        public string en { get; set; }
    }

    public class Tag
    {
        public double confidence { get; set; }
        [JsonProperty("tag")]
        public Tag2 tag { get; set; }
    }

    public class Result
    {
        public List<Tag> tags { get; set; }
    }

    public class Status
    {
        public string text { get; set; }
        public string type { get; set; }
    }

    public class Root
    {
        public Result result { get; set; }
        public Status status { get; set; }
    }
}

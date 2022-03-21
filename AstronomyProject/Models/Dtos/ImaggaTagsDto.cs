using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class Status
    {
        public string text { get; set; }
        public string type { get; set; }
    }

    public class Tag2
    {
        public string en { get; set; }
    }

    public class Tag
    {
        public double confidence { get; set; }
        public Tag2 tag { get; set; }
    }

    public class Result
    {
        public List<Tag> tags { get; set; }
    }

    public class ImaggaTagsDto
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }

        public List<ImaggaTag> GetTags()
        {
            if (Status.type != "success")
            {
                return null;
            }
            List<ImaggaTag> tags = new();
            if (Result != null)
            {
                tags.AddRange(Result.tags.OrderByDescending(t => t.confidence)
                    .Take(10)
                    .Select(t => new ImaggaTag
                    {
                        Confidence = t.confidence,
                        Tag = t.tag.en
                    }));
            }
            return tags;
        }

    }
}

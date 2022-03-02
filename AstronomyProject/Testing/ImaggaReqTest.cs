using ApiRequests.FireBaseStorage;
using ApiRequests.Imagga;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    internal class ImaggaReqTest
    {
        readonly FireBase b = new();

        readonly ImaggaReq i = new();

        public async Task getJson()
        {
            // var t = await b.Get("apod.nasa.gov/apod/image/2203/DuelingBands_Dai_960.jpg");
            //p(t);
            p("");
        }

        private void p(string t)
        {
           var x = i.AutoTagging(@"https://apod.nasa.gov/apod/image/2203/DuelingBands_Dai_960.jpg");
            var y = 0;
        }
        private void o(string t)
        {


            // Add a reference to System.Web.Extensions.dll.
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var dict =
                JsonConvert.DeserializeObject<Root>(t);
            var e = dict.GetType();
           
        }

    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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

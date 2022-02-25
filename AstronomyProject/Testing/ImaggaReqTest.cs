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
            var t = await b.Get("5.jpg");
            p(t);

        }

        private void p(string t)
        {
            o(i.autoTagging(@t));
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

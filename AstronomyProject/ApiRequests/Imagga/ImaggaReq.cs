using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRequests.Imagga
{
    public class ImaggaReq
    {
        /// <summary>
        /// This is my personal details on the server
        /// </summary>
        private const string API_KEY = "acc_f7861a318066621";
        private const string API_SECRET = "6eac13c97907f0f65afd406365102c10";
        public ImaggaReq()
        {

        }
        /// <summary>
        /// With just a simple GET request to the auto-tagging endpoint (/tags) 
        /// you receive numerous keywords describing the given photo.
        /// </summary>
        /// <param name="imageUrl">Local or remote location</param>
        /// <returns>json file</returns>
        public List<Tag> autoTagging(String imageUrl)
        {
            try
            {
                string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", API_KEY, API_SECRET)));

                var client = new RestClient("https://api.imagga.com/v2/tags");
                client.Timeout = -1;

                var request = new RestRequest(Method.GET);
                request.AddParameter("image_url", imageUrl);
                request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));

                IRestResponse response = client.Execute(request);
                return (JsonConvert.DeserializeObject<Root>(response.Content)).result.tags;
                
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}

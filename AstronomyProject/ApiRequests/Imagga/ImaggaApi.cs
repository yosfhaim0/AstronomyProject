using Models.Dtos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRequests.Imagga
{
    public class ImaggaApi
    {   
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public ImaggaApi(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }
        
        /// <summary>
        /// With just a simple GET request to the auto-tagging endpoint (/tags) 
        /// you receive numerous keywords describing the given photo.
        /// </summary>
        /// <param name="imageUrl">Local or remote location</param>
        /// <returns>json file</returns>
        public async Task<ImaggaTagsDto> AutoTagging(string imageUrl)
        {
            try
            {
                string basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _apiKey, _apiSecret)));

                var client = new RestClient("https://api.imagga.com/v2/tags")
                {
                    Timeout = -1
                };

                var request = new RestRequest(Method.GET);
                request.AddParameter("image_url", imageUrl);
                request.AddHeader("Authorization", string.Format("Basic {0}", basicAuthValue));

                IRestResponse response = await client.ExecuteAsync(request);

                var result = JsonConvert.DeserializeObject<ImaggaTagsDto>(response.Content);
                return result;
                
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}

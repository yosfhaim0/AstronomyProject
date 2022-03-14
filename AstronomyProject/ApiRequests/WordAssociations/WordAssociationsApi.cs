using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRequests.WordAssociations
{
    class AssociationsRoot
    {
        [JsonProperty("response")]
        public List<AssociationsResponse> Response { get; set; }

    }

    class AssociationsResponse
    {
        [JsonProperty("items")]
        public List<Association> Associations { get; set; }
    }

    public class WordAssociationsApi
    {
        private readonly string _apiKey;

        readonly HttpGet _httpClient = new();

        const string BASE_QUERY = @"https://api.wordassociations.net/associations/v1.0/json/search?apikey=API_KEY&text=WORD&lang=en";

        public WordAssociationsApi(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<List<Association>> GetAssociations(string word, params Pos[] pos)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentException($"'{nameof(word)}' cannot be null or empty.", nameof(word));
            }

            var query = BASE_QUERY
                .Replace("API_KEY", _apiKey)
                .Replace("WORD", word);
            
            var jsonString = await _httpClient.GetAsync(query);

            var result = JsonConvert.DeserializeObject<AssociationsRoot>(jsonString);

            return result.Response[0].Associations;
        }
    }
}

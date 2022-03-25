using Models;
using Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Tools;
namespace ApiRequests.Nasa
{
    public class NasaApi
    {
        const string API_KEY = @"tytdjk9rjM9VFGudlmOf7tnLyMYeOTFZjRp36YjU";

        const string GET_TLE = @"https://tle.ivanstanojevic.me/api/tle/";
        
        //
        const string GET_IMAGE_LIB_BASE = @"https://images-api.nasa.gov";
        
        //get nasa picture of the day 
        const string GET_APOD = @"https://api.nasa.gov/planetary/apod?api_key=";
        
        //astrid closest to Earth
        string GET_AC = @"https://api.nasa.gov/neo/rest/v1/feed?start_date=START_DATE&end_date=END_DATE&api_key=API_KEY";

        string GET_ASTROID_BY_ID = "http://www.neowsapp.com/rest/v1/neo/ASTROID_ID?api_key=API_KEY";

        readonly HttpGet client = new();

        readonly string _apiKey;

        public NasaApi()
        {

        }

        public NasaApi(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<List<Satellite>> GetSatellites()
        {
            try
            {
                var jsonString = await client.GetAsync(GET_TLE);
                var result = JsonConvert.DeserializeObject<SatelliteCollection>(jsonString);
                return result.Satellites;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GetNearAsteroidNasaDto> GetAstroidById(string astroidId)
        {
            try
            {
                var query = GET_ASTROID_BY_ID
                    .Replace("ASTROID_ID", astroidId)
                    .Replace("API_KEY", API_KEY);

                var jsonString = await client.GetAsync(query);

                var result = JsonConvert.DeserializeObject<GetNearAsteroidNasaDto>(jsonString);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetNearAsteroidNasaDto> GetAstroidByQuery(string query)
        {
            try
            {
                var jsonString = await client.GetAsync(query);

                var result = JsonConvert.DeserializeObject<GetNearAsteroidNasaDto>(jsonString);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> SearchImage(string keyWord)
        {
            var query = $"{GET_IMAGE_LIB_BASE}/search?q={keyWord}";
            
            var content = await client.GetAsync(query);
            
            var root = JsonConvert.DeserializeObject<MediaDto>(content);
            
            var result = from i in root.collection.items
                         where i.links != null
                         from img in i.links
                         where img.href != null && img.href.EndsWith(".jpg")
                         select img.href;
            
            return result;
        }


        public async Task<GetAPODNasaDto> GetImageOfTheDay()
        {
            try
            {
                var query = $"{GET_APOD}{API_KEY}";

                var jsonString = await client.GetAsync(query);

                var result = JsonConvert.DeserializeObject<GetAPODNasaDto>(jsonString);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<GetNearAsteroidNasaDto>> GetClosestAsteroids(DateTime start, DateTime end)
        {
            if((end - start).Days > 7)
            {
                throw new ArgumentOutOfRangeException("The maximum is 7 days");
            }

            var startFormat = start
                .ToString("yyyy-MM-dd");
            var endFormat = end
                .ToString("yyyy-MM-dd");
            var query = GET_AC
                .Replace("START_DATE", startFormat)
                .Replace("END_DATE", endFormat)
                .Replace("API_KEY", API_KEY);

            try
            {
                var jsonString = await client.GetAsync(query);
                var dict = JsonConvert.DeserializeObject<NearAstridCollection>(jsonString);

                var result = new List<GetNearAsteroidNasaDto>();
                foreach (var item in dict.NearAstroids)
                {
                    result.AddRange(item.Value);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<string>> GetMediaBy(string keyWord)
        {
            var query = $"{GET_IMAGE_LIB_BASE}/asset/{keyWord}";
            var content = await client.GetAsync(query);
            var a = JsonConvert.DeserializeObject<Root>(content);
            return DeserialObjMedia(a);
        }

        private static List<string> DeserialObjMedia(Root content)
        {
            
            var result = new List<string>();
            foreach (var item in content.collection.items)
            {
                result.Add(item.href);
            }
            return result;
        }
       

    }
}

using Models;
using Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Tools;
//DhNaairgwfYK2IQNrSuY9KIOHV4SDMxR40YJYQ54
//my api (yosef haim)

// tytdjk9rjM9VFGudlmOf7tnLyMYeOTFZjRp36YjU
// noam api
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

        public async Task<string> GetImageBy(string keyWord)
        {
            var query = $"{GET_IMAGE_LIB_BASE}/search?q={keyWord}";
            var content = await client.GetAsync(query);
            return content;
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
        public async Task<List<GetNANasaDto>> GetClosestAsteroids(DateTime start, DateTime end)
        {
            var startFormat = start.ToString("yyyy-MM-dd");
            var endFormat = end.ToString("yyyy-MM-dd");
            var query = GET_AC.Replace("START_DATE", startFormat)
                .Replace("END_DATE", endFormat)
                .Replace("API_KEY", API_KEY);

            var jsonString = await client.GetAsync(query);
            var dict = JsonConvert.DeserializeObject<NearAstridCollection>(jsonString);

            var result = new List<GetNANasaDto>();
            foreach (var item in dict.NearAstroids)
            {
                result.AddRange(item.Value);
            }

            return result;
        }

        private static void NewMethod(DateTime date)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime dateVal = DateTime.ParseExact(date.ToShortDateString(), "yyyy-MM-dd", culture);
            //return dateVal;
        }


    }
}

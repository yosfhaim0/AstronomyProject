using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//DhNaairgwfYK2IQNrSuY9KIOHV4SDMxR40YJYQ54
//my api (yosef haim)

// tytdjk9rjM9VFGudlmOf7tnLyMYeOTFZjRp36YjU
// noam api
namespace DataAccess.ApiRequests.Nasa
{
    public class NasaApi
    {
        const string GET_TLE = @"https://tle.ivanstanojevic.me/api/tle/";
        
        const string GET_IMAGE_LIB_BASE = @"https://images-api.nasa.gov";

        const string GET_APOD = @"https://api.nasa.gov/planetary/apod?api_key=";


        readonly HttpGet client = new();
        
        private async Task<string> GetTle()
        {
            try
            {
                return await client.GetAsync(GET_TLE);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<Satellite>> GetSatellites()
        {
            try
            {
                var jsonString = await GetTle();
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

        public async Task<NASAImageOfTheDay> GetImageOfTheDay()
        {
            try
            {
                var query = $"{GET_APOD}tytdjk9rjM9VFGudlmOf7tnLyMYeOTFZjRp36YjU";

                var jsonString = await client.GetAsync(query);

                var result = JsonConvert.DeserializeObject<NASAImageOfTheDay>(jsonString);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}

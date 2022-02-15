using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//DhNaairgwfYK2IQNrSuY9KIOHV4SDMxR40YJYQ54
//my api (yosef haim)
namespace DataAccess.ApiRequests.Nasa
{
    public class NasaApi
    {
        const string getAllTle = @"https://tle.ivanstanojevic.me/api/tle/";
        const string getAllImage = @"https://images-api.nasa.gov";
        
        readonly HttpGet client = new();
        
        private async Task<string> GetTle()
        {
            try
            {
                return await client.GetAsync(getAllTle);
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

        public async Task<string> GetImage(string keyWord)
        {
            var query = $"{getAllImage}/search?q={keyWord}";
            var result = await client.GetAsync(query);
            return result;
        }
    }
}

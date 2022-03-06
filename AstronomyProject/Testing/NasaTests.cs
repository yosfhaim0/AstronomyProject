using ApiRequests.Nasa;
using System;
using System.Threading.Tasks;
using Tools;
using Models;
using System.Linq;

namespace Testing
{
    internal class NasaTests
    {
        readonly NasaApi _nasaClient = new(); 

        public async Task GetImageTest()
        {  
            var result = await _nasaClient.SearchImage("mars");
            Console.WriteLine(result);
        }
        public async Task GetMediaTest()
        {
            var result = await _nasaClient.GetMediaBy("as11-40-5874");
            Console.WriteLine(result);
        }
        public Task GetTest()
        {
            //await _nasaClient.Get();
            // Console.WriteLine();
            return null;
        }

        public async Task TestSatellaitGet()
        {
            var result = await _nasaClient.GetSatellites();
            var output = string.Join("\n", result);
            Console.WriteLine(output);
        }

        public async Task GetImageOfTheDayTest()
        {
            var result = await _nasaClient.GetImageOfTheDay();
            Console.WriteLine(result);
        }
        public async Task GetAstroid()
        {
            var fromDate = DateTime.Parse("21.02.2022");
            var to = DateTime.Now;
            
            var result = await _nasaClient.GetClosestAsteroids(fromDate, to);
            
            var list = (from a in result
                        select 
                        a.CopyPropertiesToNew(typeof(NearAsteroid))
                        as NearAsteroid)
                        .ToList();
            var output = string.Join("\n", result);

            Console.WriteLine(output);
        }

        public async Task GetAsteroidById(int id)
        {
            var a = await _nasaClient.GetAstroidById(id.ToString());
            Console.WriteLine(a);
            foreach(var d in a.CloseApproachs)
            {
                Console.WriteLine($"{d.CloseApproachDate}");
            }
        }

    }
}
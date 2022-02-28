using ApiRequests.Nasa;
using System;
using System.Threading.Tasks;

namespace Testing
{
    internal class NasaTests
    {
        readonly NasaApi _nasaClient = new(); 

        public async Task GetImageTest()
        {  
            var result = await _nasaClient.GetImageBy("as11-40-5874");
            Console.WriteLine(result);
        }
        public async Task GetMediaTest()
        {
            var result = await _nasaClient.GetMediaBy("as11-40-5874");
            Console.WriteLine(result);
        }
        public async Task GetTest()
        {
            await _nasaClient.Get();
            Console.WriteLine();
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
            var result = await _nasaClient.GetClosestAsteroids(DateTime.Parse("16.02.2022"),DateTime.Now);
            
            var output = string.Join("\n", result);

            Console.WriteLine(output);
        }

    }
}
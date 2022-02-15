using DataAccess.ApiRequests.Nasa;
using System;
using System.Threading.Tasks;

namespace Testing
{
    internal class NasaTests
    {
        readonly NasaApi _nasaClient = new(); 

        public async Task GetImageTest()
        {  
            var result = await _nasaClient.GetImage("jupiter");
            Console.WriteLine(result);
        }

        public async Task TestSatellaitGet()
        {
            var result = await _nasaClient.GetSatellites();
            var output = string.Join("\n", result);
            Console.WriteLine(output);
        }
    }
}
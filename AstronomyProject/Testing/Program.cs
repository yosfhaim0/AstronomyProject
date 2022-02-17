using ApiRequests;
using ApiRequests.Nasa;
using System;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        static readonly NasaTests _nasaTest = new();

        static async Task Main(string[] args)
        {
            //await _nasaTest.TestSatellaitGet();
            //await _nasaTest.GetImageTest();
            await _nasaTest.GetAstroid();
        }
    }
}

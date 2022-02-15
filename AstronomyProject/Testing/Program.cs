using DataAccess.ApiRequests;
using DataAccess.ApiRequests.Nasa;
using System;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        static readonly NasaTests _nasaTest = new();

        static async Task Main(string[] args)
        {
            await _nasaTest.TestSatellaitGet();
            await _nasaTest.GetImageTest();
        }
    }
}

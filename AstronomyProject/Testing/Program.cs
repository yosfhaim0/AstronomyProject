using DataAccess.ApiRequests;
using DataAccess.ApiRequests.Nasa;
using System;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await TestSatellaitGet();
            await getImageTest();

        }

        private static async Task getImageTest()
        {
            var nasa = new NasaApi();
            var b = await nasa.GetImage("jupiter");
            Console.WriteLine(b);
        }

        private static async Task TestSatellaitGet()
        {
            var a = new NasaApi();
            var b = await a.GetSatellites();
            Console.WriteLine(b);
        }
    }
}

using ApiRequests;
using ApiRequests.Nasa;
using DataAccess.UnitOfWork;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        static readonly NasaTests _nasaTest = new();
        static readonly FireBaseTest _fireBaseTest = new();

        static async Task Main(string[] args)
        {
            //await _nasaTest.TestSatellaitGet();
            //await _nasaTest.GetImageTest();
            //await _nasaTest.GetMediaTest();
            await _fireBaseTest.PushImage();
            //await _fireBaseTest.DeleteImage();
            //await _nasaTest.GetImageOfTheDayTest();

            //Console.WriteLine(await _fireBaseTest.get("1.jpg"));
        }
       
        

    }
}

using ApiRequests;
using ApiRequests.Nasa;
using DataAccess.UnitOfWork;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using RestSharp;

namespace Testing
{
    class Program
    {
        static readonly NasaTests _nasaTest = new();
        static readonly FireBaseTest _fireBaseTest = new();
        static readonly ImaggaReqTest imaggaReqTest = new();

        static async Task Main(string[] args)
        {
            //await _nasaTest.TestSatellaitGet();
            //await _nasaTest.GetImageTest();
            //await _nasaTest.GetMediaTest();
            //await _fireBaseTest.PushImage();
            //await _fireBaseTest.DeleteImage();
            //var v=await _fireBaseTest.get("1.jpg");

            //await _nasaTest.GetImageOfTheDayTest();
            await imaggaReqTest.getJson();
            //await _nasaTest.GetAstroid();



        }
        // This examples is using RestSharp as a REST client - http://restsharp.org











    }
}

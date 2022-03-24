using ApiRequests;
using ApiRequests.Nasa;
using DataAccess.UnitOfWork;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using RestSharp;
using DomainModel.Services;

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
            //await _fireBaseTest.PushImage();
            //await _fireBaseTest.DeleteImage();
            //var v=await _fireBaseTest.get("1.jpg");

            var c=await _fireBaseTest.get("d");
            //await _nasaTest.GetImageOfTheDayTest();
            //EightPlanets a = new();
            //var c= a.GetEightPlanetsInfo();
            //var t = 0;
            //await _nasaTest.GetAstroid();
            //await _nasaTest.GetAsteroidById(54245556);
            var t = 0;

        }
        // This examples is using RestSharp as a REST client - http://restsharp.org











    }
}

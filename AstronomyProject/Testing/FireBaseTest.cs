using ApiRequests.FireBaseStorage;
using ApiRequests.Nasa;
using DomainModel.Services;
using Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    internal class FireBaseTest
    {
        EightPlanetsService b = new();
        readonly FireBase a = new();
        //readonly NasaApi _nasaClient = new();
        public async Task PushImage()
        {
            // var result = await a.Insert(@"C:\Users\yosef\1.jpg", "a");
            //var b = await _nasaClient.GetImageOfTheDay();
            var result1 = await a.Insert(b.Url, "a");
        }
        public async Task DeleteImage()
        {
            await a.Delete("a");
        }
        public async Task<List<string>> get(String A)
        {
            b.GetEightPlanetsInfo();

            List<string> vs = new List<string>();
            foreach(var i in b.GetEightPlanetsInfo())
            {
                vs.Add(await a.Get(i.Name+".jpg")+i.Name);
            }
            return vs;
        }
    }
}

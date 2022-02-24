using ApiRequests.FireBaseStorage;
using ApiRequests.Nasa;
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
        readonly FireBase a = new();
        readonly NasaApi _nasaClient = new();
        public async Task PushImage()
        {
            // var result = await a.Insert(@"C:\Users\yosef\1.jpg", "a");
            var b = await _nasaClient.GetImageOfTheDay();
            var result1 = await a.Insert(b.Url, "a");
        }
        public async Task DeleteImage()
        {
            await a.Delete("a");
        }
        public async Task<String> get(String A)
        {
            return await a.Get(A);
        }
    }
}

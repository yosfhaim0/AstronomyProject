using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ApiRequests
{
    public class HttpGet
    {
        public async Task<string> GetAsync(string uri)
        {
            try
            {
                using var client = new HttpClient();

                var response = await client.GetAsync(uri);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

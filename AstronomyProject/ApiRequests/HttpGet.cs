using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiRequests
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
            catch (HttpRequestException)
            {
                throw new Exception("No internt conection...");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

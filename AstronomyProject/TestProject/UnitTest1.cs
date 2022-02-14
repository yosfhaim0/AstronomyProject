using DataAccess.ApiRequests.Nasa;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var a = new NasaApi();
            var contant = await a.GetTle();
            Debug.Print(contant);
        }
    }
}

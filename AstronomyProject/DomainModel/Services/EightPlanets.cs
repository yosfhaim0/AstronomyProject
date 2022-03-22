using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class EightPlanets:IEightPlanets
    {
        public List<Planet> GetEightPlanetsInfo()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EightPlanets.json");
            var jsonString = File.ReadAllText(path);
            var EightPlanetsInfo = JsonConvert.DeserializeObject<List<Planet>>(jsonString);
            return EightPlanetsInfo;
        }
    }
}

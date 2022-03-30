using Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class EightPlanets : IEightPlanets
    {
        IDictionary dict;
        public EightPlanets()
        {
            dict = new Dictionary<string, string>()
            {
                ["Mass"] = "(1024kg)",
                ["Diameter"] =
                    "(km)",
                ["Density"] =
                    "(kg / m3)",
                ["Gravity"] =
                   "(m / s2)",
                ["EscapeVelocity"]
                    = "(km/ s)",
                ["RotationPeriod"]
                    = "(hours)",
                ["LengthofDay"]
                    = "(hours)",
                ["DistanceFromSun"]
                    = "(106 km)",
                ["Perihelion"]
                    = "(106 km)",
                ["Aphelion"]
                    = "(106 km)",
                ["OrbitalPeriod"]
                    = "(days)",
                ["OrbitalVelocity"]
                    = "(km/ s)",
                ["OrbitalInclination"]
                    = "(degrees)",
                ["ObliquityToOrbit"]
                    = "(degrees)",
                ["MeanTemperature"]
                    = "(C)",
                ["SurfacePressure"]
                    = "(bars)"
            };
        }
        public List<Planet> GetEightPlanetsInfo()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EightPlanets.json");
            var jsonString = File.ReadAllText(path);
            var EightPlanetsInfo = JsonConvert.DeserializeObject<List<Planet>>(jsonString);

            return EightPlanetsInfo;
        }
        public string findMida(string value)
        {
            if (dict.Contains(value))
                return dict[value].ToString();
            return "";

        }

    }
}

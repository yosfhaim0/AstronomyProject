using Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools;

namespace DomainModel.Services
{
    public class EightPlanetsService
    {
        readonly IDictionary _dictMida;
        readonly string _path;


        public EightPlanetsService()
        {
            _dictMida = new Dictionary<string, string>()
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
                ["LengthOfDay"]
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

            _path = Environment.CurrentDirectory;
            _path = _path[..(_path.LastIndexOf("AstronomyProject") + "AstronomyProject".Length)] + @"\DomainModel\Services\ExplanImages\";

        }

        public List<Planet> GetEightPlanetsInfo()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EightPlanets.json");
            var jsonString = File.ReadAllText(path);
            var eightPlanetsInfo = JsonConvert.DeserializeObject<List<Planet>>(jsonString);

            return eightPlanetsInfo;
        }

        public string FindMida(string parmeterProperty)
        {
            if (_dictMida.Contains(parmeterProperty))
                return _dictMida[parmeterProperty].ToString();
            return "";

        }

        public string GetExplanImageList(string propNames)
        {  
            return _path + propNames + ".jpg";
        }

        public IEnumerable<PropertyToolTipPair> GetPropertyToolTipPairs()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ToolTipPropEightPlanet.json");
            var jsonString = File.ReadAllText(path);
            var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            return from p in content
                   select new PropertyToolTipPair
                   {
                       PropertyName = p.Key,
                       ToolTip = p.Value.SpliceText(10)+ "\n (Source: Wikipedia or first search result on Google)",
                   };
        }
    }

    public class PropertyToolTipPair
    {
        public string PropertyName { get; set; }

        public string ToolTip { get; set; }

        string _property;
        public string Property { get => _property ??= PropertyName.Replace(" ", ""); }
    }
}

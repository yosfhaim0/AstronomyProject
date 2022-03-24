using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Planet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Mass { get; set; }
        public double Diameter { get; set; }
        public double Density { get; set; }
        public double Gravity { get; set; }
        public double EscapeVelocity { get; set; }
        public double RotationPeriod { get; set; }
        public double LengthOfDay { get; set; }
        public double DistanceFromSun { get; set; }
        public double Perihelion { get; set; }
        public double Aphelion { get; set; }
        public double OrbitalPeriod { get; set; }
        public double OrbitalVelocity { get; set; }
        public double OrbitalInclination { get; set; }
        public double OrbitalEccentricity { get; set; }
        public double ObliquityToOrbit { get; set; }
        public double MeanTemperature { get; set; }
        public double? SurfacePressure { get; set; }
        public int NumberOfMoons { get; set; }
        public bool HasRingSystem { get; set; }
        public bool HasGlobalMagneticField { get; set; }

        public string Url { get; set; }
    }
}

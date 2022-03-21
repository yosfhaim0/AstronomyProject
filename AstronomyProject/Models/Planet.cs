using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Planet
    {
        public int id { get; set; }
        public string name { get; set; }
        public double mass { get; set; }
        public double diameter { get; set; }
        public double density { get; set; }
        public double gravity { get; set; }
        public double escapeVelocity { get; set; }
        public double rotationPeriod { get; set; }
        public double lengthOfDay { get; set; }
        public double distanceFromSun { get; set; }
        public double perihelion { get; set; }
        public double aphelion { get; set; }
        public double orbitalPeriod { get; set; }
        public double orbitalVelocity { get; set; }
        public double orbitalInclination { get; set; }
        public double orbitalEccentricity { get; set; }
        public double obliquityToOrbit { get; set; }
        public double meanTemperature { get; set; }
        public double? surfacePressure { get; set; }
        public int numberOfMoons { get; set; }
        public bool hasRingSystem { get; set; }
        public bool hasGlobalMagneticField { get; set; }
    }
}

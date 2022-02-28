using System;

namespace Models.Dtos
{
    public class EstimatedDiameterDto
    {
        public Meters meters { get; set; }

    }

    public class Meters
    {
        public double estimated_diameter_min { get; set; }
        public double estimated_diameter_max { get; set; }
    }


}

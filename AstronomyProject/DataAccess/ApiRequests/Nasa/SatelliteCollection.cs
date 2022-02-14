using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ApiRequests.Nasa
{
    public class SatelliteCollection
    {
        //public object view { get; set; }

        //public object parameters  { get; set; }

        //public int totalItems { get; set; }

        public List<Satellite> member { get; set; }
    }
}

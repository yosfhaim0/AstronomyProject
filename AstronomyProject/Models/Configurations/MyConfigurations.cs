using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configurations
{
    public class MyConfigurations
    {
        public string Admin { get; set; }

        public Dictionary<string, string> NasaApiKeys { get; set; }

        public string CurrentNasaApiKey { get => NasaApiKeys[Admin]; }
        
        public Dictionary<string, string> ConnectionStrings { get; set; }

        public string CurrentConnectionStrings { get => ConnectionStrings[Admin]; }

    }
}

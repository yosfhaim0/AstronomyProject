﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public record ImageOfTheDay
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Explanation { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string MediaType { get; set; }
    }
}

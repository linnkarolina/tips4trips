using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationFinal.Models
{
    public class AddTripClass
    {
        public string trip_name { get; set; }
        public int trip_length { get; set; }
        public string difficulty { get; set; }
        public string description { get; set; }
        public string city { get; set; }
        public string website { get; set; }
        public int latitude { get; set; }
        public string longitude { get; set; }
        public string map_route { get; set; }
        public string image { get; set; }
        public string type_of_trip { get; set; }
        public string tag { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationFinal.Models
{
    public class ExploreClass
    {
        public string trip_id { get; set; }
        public string trip_name { get; set; }
       
        public string length { get; set; }
        public string difficulty { get; set; }
        public string description { get; set; }
        public string city { get; set; }
        public string website { get; set; }
        public byte[] image { get; set; }
        public string tags { get; set; }
        public string trip_area { get; set; }
        public string diff { get; set; }

       public string ams { get; set; }
        public string route { get; set; }

        public int rating { get; set; }
    
    }

}

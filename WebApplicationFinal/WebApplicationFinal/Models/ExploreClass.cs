using System.Web.Mvc;

namespace WebApplicationFinal.Models
{
    public class ExploreClass
    {
        [AllowHtml]
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
        public string type_of_trip { get; set; }

        public int rating { get; set; }
        public string img_ID { get; set; }
        public string username { get; set; }


        public string startLongitude { get; set; }
        public string startLatitude { get; set; }
        public string endLongitude { get; set; }

        public string endLatitude { get; set; }

        public string bar { get; set; }







    }

}

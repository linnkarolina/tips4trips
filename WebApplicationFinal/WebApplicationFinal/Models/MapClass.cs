using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationFinal.Models
{
    public class MapClass
    {
        public string Title { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WebApplicationFinal.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            var cities = new List<MapClass>();
            cities.Add(new MapClass() { Title = "Paris", Lat = 48.855901, Lng = 2.349272 });
            cities.Add(new MapClass() { Title = "Berlin", Lat = 52.520413, Lng = 13.402794 });
            cities.Add(new MapClass() { Title = "Rome", Lat = 41.907074, Lng = 12.498474 });
            return View(cities);
        }
        [HttpPost]
        public JsonResult GetAnswer(string question)
        {
            int index = _rnd.Next(_db.Count);
            var answer = _db[index];
            return Json(answer);
        }

        private static Random _rnd = new Random();

        private static List<string> _db = new List<string> { "Yes", "No", "Definitely, yes", "I don't know", "Looks like, yes" };
    

}
}
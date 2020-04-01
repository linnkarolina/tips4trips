using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;



namespace WebApplicationFinal.Controllers
{
    public class ExploreController : Controller
    {
        public ActionResult Index()
        {
            List<ExploreClass> images = new List<ExploreClass>();

            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);

            string query = "SELECT * FROM trip";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                images.Add(new ExploreClass
                {
                    area = dr["area"].ToString(),
                    area_ID_area = dr["area_ID_area"].ToString(),
                    type_of_trip = dr["type_of_trip"].ToString(),
                    length_of_trip = dr["length_of_trip"].ToString(),
                    difficulty = dr["difficulty"].ToString(),
                    description = dr["description"].ToString(),
                    location = dr["location"].ToString(),
                    attraction_website = dr["attraction_website"].ToString(),
                    image = (byte[])dr["Image"],
                }
                );
            }
            ViewData["List1"] = images;
            return View("Index");
        }

        [HttpPost]
        public ActionResult TripSearch(ExploreClass search ) {
            List<ExploreClass> images = new List<ExploreClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            int i = 0;
            string query = "SELECT * FROM trip left join trip_has_stored_tag on trip.area = trip_has_stored_tag.Trip_area inner join stored_tag on stored_tag.ID_tag= trip_has_stored_tag.Stored_tag_ID_tag where ";
            string trip = search.area;
            string tags = search.tags;
            string diff = search.diff;

            if (!string.IsNullOrEmpty(trip))
            {
                if (i == 0)
                {
                    query += "area_ID_area ='" + trip + "'";
                    i = 1;
                }
                else
                {
                    query += "AND area_ID_area ='" + trip + "'";
                    i = 1;
                }
            }

            if (!string.IsNullOrEmpty(tags))
            {
                if (i == 0)
                {
                    query += "name LIKE'" + tags + "'";
                    i = 1;
                }
                else
                {
                    query += "AND name LIKE'" + tags + "'";
                    i = 1;
                }
            }
            if (!string.IsNullOrEmpty(diff))
            {
                if (i == 0)
                {
                    query += "difficulty ='" + diff + "'";
                    i = 1;
                }
                else
                {
                    query += "AND difficulty ='" + diff + "'";
                    i = 1;
                }

            }

            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                images.Add(new ExploreClass
                {
                    area = dr["area"].ToString(),
                    area_ID_area = dr["area_ID_area"].ToString(),
                    type_of_trip = dr["type_of_trip"].ToString(),
                    length_of_trip = dr["length_of_trip"].ToString(),
                    difficulty = dr["difficulty"].ToString(),
                    description = dr["description"].ToString(),
                    location = dr["location"].ToString(),
                    attraction_website = dr["attraction_website"].ToString(),
                    image = (byte[])dr["Image"],
                }
                );
            }
            ViewData["List1"] = images;
            return View();

        }


       
        public ActionResult Trip(ExploreClass off)
        {
            List<ExploreClass> images = new List<ExploreClass>();

            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);

            string query = "SELECT * FROM image LEFT JOIN trip ON trip.area = image.trip_area LEFT JOIN map_route on trip.area = map_route.trip_ID_area WHERE area_id_area='" + off.ams+"'" ;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                images.Add(new ExploreClass
                {
                    area = dr["area"].ToString(),
                    area_ID_area = dr["area_ID_area"].ToString(),
                    type_of_trip = dr["type_of_trip"].ToString(),
                    length_of_trip = dr["length_of_trip"].ToString(),
                    difficulty = dr["difficulty"].ToString(),
                    description = dr["description"].ToString(),
                    location = dr["location"].ToString(),
                    attraction_website = dr["attraction_website"].ToString(),
                    image = (byte[])dr["picture"],
                }
                );
            }
            ViewData["List1"] = images;
            return View("Trip");
        }
    }
   }
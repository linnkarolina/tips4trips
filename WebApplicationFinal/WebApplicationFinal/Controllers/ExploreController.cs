﻿using System.Collections.Generic;
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
                    trip_id = dr["trip_id"].ToString(),
                    trip_name = dr["trip_name"].ToString(),
                    length = dr["length"].ToString(),
                    difficulty = dr["difficulty"].ToString(),
                    description = dr["description"].ToString(),
                    city = dr["city"].ToString(),
                    website = dr["website"].ToString(),
                    image = (byte[])dr["Image"],
                }
                );
                getTag();
            }    
            ViewData["List1"] = images;
            return View("Index");
        }

        [HttpPost]
        public ActionResult TripSearch(ExploreClass search)
        {
            List<ExploreClass> images = new List<ExploreClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            int i = 0;
            string query = "SELECT * FROM trip left join trip_tag on trip.trip_id = trip_tag.Trip_id inner join tag on tag.tag= trip_tag.tag where ";
            string city = search.city;
            string tags = search.tags;
            string diff = search.diff;

            if (!string.IsNullOrEmpty(city))
            {
                if (i == 0)
                {
                    query += " city = '" + city + "'";
                    i = 1;
                }
                else
                {
                    query += " AND city = '" + city + "'";
                    i = 1;
                }
            }

            if (!string.IsNullOrEmpty(tags))
            {
                if (i == 0)
                {
                    query += " tag.tag LIKE '" + tags + "'";
                    i = 1;
                }
                else
                {
                    query += " AND tag.tag LIKE '" + tags + "'";
                    i = 1;
                }
            }
            if (!string.IsNullOrEmpty(diff))
            {
                if (i == 0)
                {
                    query += " difficulty LIKE '" + diff + "'";
                    i = 1;
                }
                else
                {
                    query += " AND difficulty LIKE '" + diff + "'";
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
                    trip_id = dr["trip_id"].ToString(),
                    trip_name = dr["trip_name"].ToString(),
                    length = dr["length"].ToString(),
                    difficulty = dr["difficulty"].ToString(),
                    description = dr["description"].ToString(),
                    city = dr["city"].ToString(),
                    website = dr["website"].ToString(),
                    image = (byte[])dr["Image"],
                }
                );
            }
            getTag();
            ViewData["List1"] = images;
            return View();

        }



        public ActionResult Trip(ExploreClass off)
        {
            List<ExploreClass> images = new List<ExploreClass>();

            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);

            string query = "SELECT * FROM image as i " +
                "LEFT JOIN trip as t ON t.trip_id = i.trip_id " +
                "LEFT JOIN map_coordinates AS m ON t.trip_id = m.trip_id " +
                "LEFT JOIN trip_with_type AS tw ON tw.trip_ID=t.trip_ID " +
                "WHERE i.trip_id='" + off.ams + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                images.Add(new ExploreClass
                {
                    trip_id = dr["trip_id"].ToString(),
                    trip_name = dr["trip_name"].ToString(),
                    length = dr["length"].ToString(),
                    difficulty = dr["difficulty"].ToString(),
                    description = dr["description"].ToString(),
                    city = dr["city"].ToString(),
                    website = dr["website"].ToString(),
                    type_of_trip = dr["type_of_trip"].ToString(),
                    image = (byte[])dr["Image"],
                    route = dr["route"].ToString(),

                }
                ) ;
                GetIcon(dr["type_of_trip"].ToString());
                GetDiff(dr["difficulty"].ToString());
            }
           
  
            ViewData["List1"] = images;
            return View("Trip");
        }

        public ActionResult GetDiff(string diff)
        {
            string str = diff.ToLower();
            if (str.Contains("easy")  == true)
            {
                ViewBag.diffIcon = "<div class='dot green  dot--full'></div>";
            }
            if (str.Contains("medium") == true)
            {
                ViewBag.diffIcon = "<div class='dot yellow  dot--full'></div>";
            }
            if (str.Contains("hard") == true)
            {
                ViewBag.diffIcon = "<div class='dot red  dot--full'></div>";
            }
            return null;
        }
          

            public ActionResult GetIcon(string type_of_trip) {
            string str = type_of_trip.ToLower();
            if (str.Contains("cyc") || str.Contains("bik") == true) {


                ViewBag.icon = "<i class='fas fa-bicycle' style='font-size:24px'></i>";
            }

            if (str.Contains("swim") == true)
            {


                ViewBag.icon = "<i class='fas fa-swimmer' style='font-size:24px'></i>";
            }

            if (str.Contains("tram") == true)
            {


                ViewBag.icon = "<i class='fas fa-tram' style='font-size:24px'></i>";
            }

            if (str.Contains("skat") == true)
            {


                ViewBag.icon = "<i class='fas fa-skating' style='font-size:24px'></i>";
            }

            if (str.Contains("ski") == true)
            {


                ViewBag.icon = "<i class='fas fa-skiing-nordic' style='font-size:24px'></i>";
            }

            if (str.Contains("run") == true)
            {


                ViewBag.icon = "<i class='fas fa-walking' style='font-size:24px'></i>";
            }



            return null;
        }

        [HttpPost]
        public ActionResult LeaveReview(ExploreClass amv)
        {
            List<ExploreClass> images = new List<ExploreClass>();

            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);

            string query = "SELECT * FROM image LEFT JOIN trip ON trip.trip_id = image.trip_id LEFT JOIN map_coordinates on trip.trip_id = map_coordinates.trip_id WHERE image.trip_id='" + amv.ams + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                images.Add(new ExploreClass
                {
                    trip_id = dr["trip_id"].ToString(),
                    trip_name = dr["trip_name"].ToString(),
                    length = dr["length"].ToString(),
                    difficulty = dr["difficulty"].ToString(),
                    description = dr["description"].ToString(),
                    city = dr["city"].ToString(),
                    website = dr["website"].ToString(),
                    image = (byte[])dr["Image"],
                    route = dr["route"].ToString(),

                }
                );
            }
            mysql.Close();
            string markers = "[";
            string conString = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlCommand cmd = new MySqlCommand("Select * from map_coordinates inner join trip on trip.trip_id=map_coordinates.trip_id");
            using (MySqlConnection con = new MySqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        markers += "{";
                        markers += string.Format("'title': '{0}',", sdr["trip_id"]);
                        markers += string.Format("'lat': '{0}',", sdr["Latitude"]);
                        markers += string.Format("'lng': '{0}',", sdr["Longitude"]);
                        markers += string.Format("'description': '{0}',", sdr["description"]);
                        markers += string.Format("'image': '{0}',", (byte[])sdr["Image"]);
                        markers += string.Format("'website': '{0}'", sdr["website"]);
                        markers += "},";
                    }
                }
                con.Close();
            }

            markers += "];";
            ViewBag.Markers = markers;

            ViewData["List1"] = images;


            string mainconni = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysqli = new MySqlConnection(mainconn);
            string user = Request.Cookies["UserCookie"].Value;
            string queryi = "INSERT INTO review VALUES(" + amv.ams + ", '" + user + "', " + amv.rating + " );";
            MySqlCommand commi = new MySqlCommand(queryi);
            commi.Connection = mysqli;
            mysqli.Open();
            int dri = commi.ExecuteNonQuery();
            return View();
        }

        public void getTag()
        {
                List<Account> listTag = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query1 = "SELECT * FROM trip_tag RIGHT JOIN trip ON trip_tag.trip_id = trip.trip_id;";
                MySqlCommand comm1 = new MySqlCommand(query1);
                comm1.Connection = mysql;
                mysql.Open();
                MySqlDataReader mr = comm1.ExecuteReader();
                while (mr.Read())
                {
                    listTag.Add(new Account
                    {
                        tagname = mr["tag"].ToString(),
                        idtag = mr.GetInt32(mr.GetOrdinal("trip_ID")),


                    });
                }

                ViewData["listTag"] = listTag;

                
            }


        
    }
}
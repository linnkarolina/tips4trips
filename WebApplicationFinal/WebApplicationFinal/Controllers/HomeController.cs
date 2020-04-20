using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.IO;
namespace WebApplicationFinal.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
      
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

                });

            }

            ViewBag.ExploreClass = images;
            getImage();
            getTag();
            getRating();
            var thisMenu = RecommendTrips();
            var ok = RecommendNearby();
            return View(images);
        }


        public ActionResult RecommendTrips()
        {


            List<ExploreClass> recommend = new List<ExploreClass>();

            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            try
            {
                String name = Request.Cookies["UserCookie"].Value;
                string query = "SELECT * FROM trip AS t LEFT JOIN trip_tag as b on t.trip_ID =b.trip_ID LEFT JOIN tag AS c ON b.tag = c.tag LEFT JOIN user_tag AS u ON b.tag =u.tag WHERE username='" + name + "'";
                MySqlCommand comm = new MySqlCommand(query);
                comm.Connection = mysql;
                mysql.Open();

                MySqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    recommend.Add(new ExploreClass
                    {
                        trip_id = dr["trip_id"].ToString(),
                        trip_name = dr["trip_name"].ToString(),
                        length = dr["length"].ToString(),
                        difficulty = dr["difficulty"].ToString(),
                        description = dr["description"].ToString(),
                        city = dr["city"].ToString(),
                        website = dr["website"].ToString(),
                     
                    });

                }

                ViewBag.ExploreClass = recommend;
            }
            catch { }
            return null;
        }

        public ActionResult RecommendNearby()
        {


            List<ExploreClass> recommendCity = new List<ExploreClass>();

            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            try
            {
                String name = Request.Cookies["UserCookie"].Value;
                string query = "SELECT * FROM trip AS t LEFT JOIN location AS l ON l.city= t.city LEFT JOIN admin AS a ON a.city=l.city WHERE username='abc123'";
                MySqlCommand comm = new MySqlCommand(query);
                comm.Connection = mysql;
                mysql.Open();

                MySqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    recommendCity.Add(new ExploreClass
                    {
                        trip_id = dr["trip_id"].ToString(),
                        trip_name = dr["trip_name"].ToString(),
                        length = dr["length"].ToString(),
                        difficulty = dr["difficulty"].ToString(),
                        description = dr["description"].ToString(),
                        city = dr["city"].ToString(),
                        website = dr["website"].ToString(),
                        
                    });

                }

                ViewData["recommendCity"] = recommendCity;
            }
            catch { }
            return null;
        }

        public void getImage() {
            List<ExploreClass> listImage = new List<ExploreClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query1 = "SELECT * FROM image";
            MySqlCommand comm1 = new MySqlCommand(query1);
            comm1.Connection = mysql;
            mysql.Open();
            MySqlDataReader mr = comm1.ExecuteReader();
            while (mr.Read())
            {
                listImage.Add(new ExploreClass
                {
                    image = (byte[])mr["Image"],
                    
                    img_ID = mr["trip_ID"].ToString(),


                });
            }

            ViewData["listImage"] = listImage;

        }
        public void getTag()
        {
            List<Account> listTag = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query1 = "SELECT * FROM trip_tag LEFT JOIN trip ON trip_tag.trip_id = trip.trip_id;";
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

        public ActionResult getRating() {
            List<Account> listReview = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query1 = "SELECT * FROM review;";
            MySqlCommand comm1 = new MySqlCommand(query1);
            comm1.Connection = mysql;
            mysql.Open();
            MySqlDataReader mr = comm1.ExecuteReader();
            while (mr.Read())
            {
                listReview.Add(new Account
                {
                    idtag = mr.GetInt32(mr.GetOrdinal("trip_ID")),
                    rating = mr.GetInt32(mr.GetOrdinal("rating")),
                });
            }

            ViewData["listReview"] = listReview;
            return null;
        }


    }
}
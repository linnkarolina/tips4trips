﻿using System;
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
                    image = (byte[])dr["Image"],




                });

            }
            var thisMenu = RecommendTrips();
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
                        image = (byte[])dr["Image"],
                    });

                }

                ViewBag.ExploreClass = recommend;
            }
            catch { }
            return null;
        }

    }
}
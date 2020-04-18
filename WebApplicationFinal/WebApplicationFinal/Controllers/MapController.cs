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
            string markers = "[";
            string conString = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlCommand cmd = new MySqlCommand("Select * from map_coordinates left join trip on trip.trip_id=map_coordinates.trip_ID");
            using (MySqlConnection con = new MySqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        markers += "{";
                        markers += string.Format("'title': '{0}',", sdr["trip_name"]);
                        markers += string.Format("'lat': '{0}',", sdr["startLatitude"]);
                        markers += string.Format("'lng': '{0}',", sdr["startLongitude"]);
                        markers += string.Format("'description': '{0}',", sdr["description"]);
                        int trip_id = sdr.GetInt32(sdr.GetOrdinal("trip_id"));
                        GetImage(trip_id);
                        foreach (var detail in ViewBag.listImage)
                        {
                            markers += string.Format("'image': '{0}',", System.Convert.ToBase64String(detail.image));
                            break;
                        }

                        
                        markers += string.Format("'website': '{0}'", sdr["website"]);
                        markers += "},";

                    }
                }
                con.Close();
            }



            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }

        public void  GetImage(int trip_id)
        {
            List<ExploreClass> listImage = new List<ExploreClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query1 = "SELECT * FROM image where trip_id='" + trip_id + "'";
            MySqlCommand comm1 = new MySqlCommand(query1);
            comm1.Connection = mysql;
            mysql.Open();
            MySqlDataReader mr = comm1.ExecuteReader();
            while (mr.Read())
            {
                listImage.Add(new ExploreClass
                {
                    image = (byte[])mr["Image"],

                });
            }

            ViewData["listImage"] = listImage;
            
        }
   }
 }

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
    public class ExploreController : Controller
    {  

        public ActionResult Index()
        {
            List<ExploreClass> list1 = new List<ExploreClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);

            string query = "SELECT * FROM trip";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new ExploreClass
                {
                    area = dr["area"].ToString(),
                    area_ID_area = dr["area_ID_area"].ToString(),
                    type_of_trip = dr["type_of_trip"].ToString(),
                    length_of_trip = dr["length_of_trip"].ToString(),
                    difficulty = dr["difficulty"].ToString(),
                    description = dr["description"].ToString(),
                    location = dr["location"].ToString(),
                    attraction_website = dr["attraction_website"].ToString(),
                    image = (byte[])dr["image"],

            });
            }
            mysql.Close();
            return View(list1);
        }
    }
}
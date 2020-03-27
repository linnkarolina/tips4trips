using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.IO;
using System.Data;

using System.Data.SqlClient;


namespace WebApplicationFinal.Controllers
{
    public class ExploreController : Controller
    {


        // GET: Home
        public ActionResult Index()
        {
            List<ExploreClass> images = GetImages();
            return View(images);
        }

        [HttpPost]
        public ActionResult Index(int imageId)
        {
            List<ExploreClass> images = GetImages();
            ExploreClass image = images.Find(p => p.Id == imageId);
            if (image != null)
            {
                image.IsSelected = true;
                ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(image.Data, 0, image.Data.Length);
            }
            return View(images);
        }

        private List<ExploreClass> GetImages()
        {

            List<ExploreClass> images = new List<ExploreClass>();

            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);

            string query = "SELECT * FROM trip";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader sdr = comm.ExecuteReader();


                     while (sdr.Read())
                     {
                    images.Add(new ExploreClass
                         {
                        Id = Convert.ToInt32(sdr["area"]),
                        Name = sdr["area_ID_area"].ToString(),
                        ContentType = sdr["type_of_trip"].ToString(),

                        Data = (byte[])sdr["image"]
                });
                    }
        
                    mysql.Close();
            return images;
            }

                
            }
        }

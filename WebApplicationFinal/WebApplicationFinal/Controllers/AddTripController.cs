using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Web;
using System.IO;

namespace WebApplicationFinal.Controllers
{
    public class AddTripController : Controller
    {
        // GET: AddTrip
        public ActionResult Index()
        {
            getTripType();
            getCity();
            getTag();
           

            return View();

        }

        public ActionResult getTripType()
        {
            List<AddTripClass> ListType = new List<AddTripClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "SELECT * FROM type_of_trip ;";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                ListType.Add(new AddTripClass
                {
                    type_of_trip = dr["type_of_trip"].ToString(),



                });
            }
            mysql.Close();
            ViewData["ListType"] = ListType;

            return null;

        }

        public ActionResult getCity()
        {
            List<AddTripClass> ListCity = new List<AddTripClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "SELECT * FROM location ;";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                ListCity.Add(new AddTripClass
                {
                    city = dr["city"].ToString(),



                });
            }
            mysql.Close();
            ViewData["ListCity"] = ListCity;

            return null;

        }

        public ActionResult getTag()
        {
            List<AddTripClass> ListTag = new List<AddTripClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "SELECT * FROM tag ;";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                ListTag.Add(new AddTripClass
                {
                    tag = dr["tag"].ToString(),



                });
            }
            mysql.Close();
            ViewData["ListTag"] = ListTag;

            return null;

        }

        public ActionResult UserAddedTrip(AddTripClass file)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "INSERT INTO trip VALUES (null, '" + file.trip_name + "', '" + file.trip_length + "', '" + file.difficulty + "','" + file.description + "','" + file.city + "', '" + file.website + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            long id = comm.LastInsertedId;
           



            try

            {

                Byte[] bytes = null;

                if (file.Filepic.FileName != null)

                {

                    Stream fs = file.Filepic.InputStream;

                    BinaryReader br = new BinaryReader(fs);

                    bytes = br.ReadBytes((Int32)fs.Length);

                    string connectionstring = Convert.ToString(ConfigurationManager.ConnectionStrings["ConString"]);

                    MySqlConnection con = new MySqlConnection(connectionstring);

                    MySqlCommand cmd = new MySqlCommand("Insert into INSERT INTO image VALUES (null, '" + id + "', '" + file.Filepic + "')", con);

                    



                    con.Open();

                    cmd.ExecuteNonQuery();

                    con.Close();

                    ViewBag.Image = ViewImage(bytes);

                }

            }

            catch (Exception)

            {

                throw;

            }

            return View();




        }



  

    

        private string ViewImage(byte[] arrayImage)

        {

            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);

            return "data:image/png;base64," + base64String;

        }
    }
        }

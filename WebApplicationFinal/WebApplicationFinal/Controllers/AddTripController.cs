using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;


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

        public ActionResult UserAddedTrip(AddTripClass trp)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "INSERT INTO trip VALUES (null, '" + trp.trip_name + "', '" + trp.trip_length + "', '" + trp.difficulty + "','" + trp.description + "','" + trp.city + "', '" + trp.website + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            long id = comm.LastInsertedId;
            insertImage(id, trp.image);
            return View("Index");
        }

        public ActionResult insertImage(long trip_id, string image) {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "INSERT INTO image VALUES (null, '"+trip_id+"');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            return null;
        }
    }
}
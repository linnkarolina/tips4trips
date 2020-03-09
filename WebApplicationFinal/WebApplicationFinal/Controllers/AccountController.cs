using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using MySql.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WebApplicationFinal.Controllers
{
    public class AccountController : Controller
    {
        
        // GET: Account
       // [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "SELECT * FROM user where username='"+acc.Name+"' and password='"+acc.Password+"'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
                mysql.Open();
                MySqlDataReader dr = comm.ExecuteReader();
           if(dr.Read())
            {
                mysql.Close();
                return View("Create");
            }
           else
            {
                mysql.Close();
                return View("Error");
            }
           
        }
        public ActionResult RegisterVerify(Account acc)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "INSERT INTO user VALUES( username='" + acc.Name + "', password='" + acc.Password + "', location='" + acc.location + "', email='" + acc.email + "', full_name='" + acc.full_name + "', phone_NR='" + acc.phone_NR + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            if (dr.Read())
            {
                mysql.Close();
                return View("Create");
            }
            else
            {
                mysql.Close();
                return View("Error");
            }

        }
    }
}
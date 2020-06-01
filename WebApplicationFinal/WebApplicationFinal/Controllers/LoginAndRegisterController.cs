using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Web;
using System.Web.Security;

namespace WebApplicationFinal.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            getLocation();
            return View();
        }

        public ActionResult getLocation()
        {
            List<Account> list1 = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
           
            string query = "SELECT * FROM location";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new Account
                {
                    city = dr["city"].ToString(),
                });
            }
            mysql.Close();
            ViewData["list1"] = list1;
            return null;
        }

        [HttpPost]
        public ActionResult LoginVerify(Account acc)
        {
            checkLoginForm(acc);
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "SELECT * FROM user where username='" + acc.Name + "' and password='" + acc.Password + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            if (dr.Read())
            {
                HttpCookie Cridentials = new HttpCookie("UserCookie");
                Cridentials.Expires = DateTime.Now.AddHours(5);
                Response.Cookies.Add(Cridentials);
                Response.Cookies["UserCookie"].Value = acc.Name;
                mysql.Close();
                
                Response.Redirect("../Home/Index", false);
               
                return null;
            }
            else
            {
                mysql.Close();
                ViewBag.wrongPassword = "Wrong username or password";
                return View("Login");

            }

        }



        public ActionResult RegisterVerify(Account acc)
        {
           checkregisterForm(acc);
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "INSERT IGNORE INTO user (username, password, city, email, full_name, phone_NR) VALUES ('" + acc.Name + "', '" + acc.Password + "', '" + acc.location + "', '" + acc.email + "', '" + acc.full_name + "', '" + acc.phone_NR + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            if (dr != 0)
            {
                HttpCookie cridentials = new HttpCookie("UserCookie");
                cridentials.Expires = DateTime.Now.AddHours(5);
                Response.Cookies.Add(cridentials);
                Response.Cookies["UserCookie"].Value = acc.Name;
                mysql.Close();
                Response.Redirect("../Home/Index", false);
                return null;
            }
            else
            {
                mysql.Close();
                ViewBag.usernameIsTaken = "Username is taken!";
                return View("Register");
            }


        }
        
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Response.Cookies["UserCookie"].Expires = DateTime.Now.AddDays(-1);

            Response.Cookies["UserCookie"].Value = null;

            Response.Redirect("../Home/Index", false);
            return null;
        }

        public ActionResult checkLoginForm(Account acc) {
            if (acc.Name == null)
            {
                ViewBag.emptyForm = "You forgot to fill out your username";
                getLocation();
                return RedirectToAction("Register");
            }

            if (acc.Password == null)
            {
                ViewBag.emptyForm = "You forgot to fill out your password";
                getLocation();
                return RedirectToAction("Register");
            }
            return null;
        }

        public ActionResult checkregisterForm(Account acc) {
           
            if (acc.Name == null)
            {
                ViewBag.emptyForm= "You forgot to fill out your username";
                getLocation();
                return RedirectToAction("Register");
            }

            if (acc.Password == null) {
                ViewBag.emptyForm = "You forgot to fill out your password";
                getLocation();
               return RedirectToAction("Register");
            }

             if (acc.location == null)
            {
                ViewBag.emptyForm = "You forgot to fill out your location";
                getLocation();
                return RedirectToAction("Register");
            }
             if (acc.email == null)
            {
                ViewBag.emptyForm = "You forgot to fill out your e-mail";
                getLocation();
                return RedirectToAction("Register");
            }
             if (acc.full_name == null)
            {
                ViewBag.emptyForm = "You forgot to fill out your full name";
                getLocation();
                return RedirectToAction("Register");
            }
             if (acc.phone_NR == 0)
            {
                ViewBag.emptyForm = "You forgot to fill out your phone nr";
                getLocation();
                return RedirectToAction("Register");

            }

            return null ;
        }
    }
}
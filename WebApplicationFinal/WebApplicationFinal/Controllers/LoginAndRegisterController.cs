using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplicationFinal.Models;

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

        private void getLocation()
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
          
                CheckregisterForm(acc);
            CheckIfUserExist(acc);
            if (ViewBag.usernameIsTaken == null && ViewBag.emptyForm == null)
            {
                string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
                MySqlConnection mysql = new MySqlConnection(mainconn);
                string query = "INSERT INTO user (username, password, city, email, full_name, phone_NR) VALUES ('" + acc.Name + "', '" + acc.Password + "', '" + acc.location + "', '" + acc.email + "', '" + acc.full_name + "', '" + acc.phone_NR + "');";
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
                    return View("Register");
                }
            }
            else {
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

        public ActionResult checkLoginForm(Account acc)
        {
            if (string.IsNullOrWhiteSpace(acc.Name))
            {
                ViewBag.emptyForm = "You forgot to fill out your username";
                getLocation();
                return RedirectToAction("Register");
            }

            if (string.IsNullOrWhiteSpace(acc.Password))
            {
                ViewBag.emptyForm = "You forgot to fill out your password";
                getLocation();
                return RedirectToAction("Register");
            }
            return null;
        }

        public ActionResult CheckregisterForm(Account acc)
        {

            if (string.IsNullOrWhiteSpace(acc.Name))
            {
                ViewBag.emptyForm = "You forgot to fill out your username";
                getLocation();
                return RedirectToAction("Register");
            }

            if (string.IsNullOrWhiteSpace(acc.Password))
            {
                ViewBag.emptyForm = "You forgot to fill out your password";
                getLocation();
                return RedirectToAction("Register");
            }

            if (string.IsNullOrWhiteSpace(acc.location))
            {
                ViewBag.emptyForm = "You forgot to fill out your location";
                getLocation();
                return RedirectToAction("Register");
            }

            if (string.IsNullOrWhiteSpace(acc.email))
            {
                ViewBag.emptyForm = "You forgot to fill out your e-mail";
                getLocation();
                return RedirectToAction("Register");
            }

            if (string.IsNullOrWhiteSpace(acc.full_name))
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

            return null;
        }

        public ActionResult CheckIfUserExist(Account acc) {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "SELECT username FROM user where username= '"+ acc.Name + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            if (dr.Read()) {
               
                ViewBag.usernameIsTaken = "Username is taken!";
                return View("Register");
            } else{

                return null;
            }

        }
    }
}
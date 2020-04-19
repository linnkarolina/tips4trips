using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using MySql.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Security;
using System.Diagnostics;

namespace WebApplicationFinal.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public ActionResult LoginVerify(Account acc)
        {
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

                return View("Login");

            }

        }

        public ActionResult RegisterVerify(Account acc)
        {
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


        public ActionResult MyAccount()
        {
            List<Account> list1 = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM user where username='" + name + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new Account
                {
                    Name = dr["username"].ToString(),
                    Password = dr["password"].ToString(),
                    location = dr["city"].ToString(),
                    email = dr["email"].ToString(),
                    full_name = dr["full_name"].ToString(),
                    phone_NR = dr.GetInt32(dr.GetOrdinal("phone_NR")),


                });
            }
            mysql.Close();
            List<Account> list2 = new List<Account>();
            string query1 = "SELECT * FROM user_tag INNER JOIN tag ON user_tag.tag = tag.tag WHERE user_tag.username ='" + name + "';";
            MySqlCommand comm1 = new MySqlCommand(query1);
            comm1.Connection = mysql;
            mysql.Open();
            MySqlDataReader mr = comm1.ExecuteReader();
            while (mr.Read())
            {
                list2.Add(new Account
                {
                    tagname = mr["username"].ToString(),



                });
            }

            ViewData["List1"] = list1;
            ViewData["List2"] = list2;
            return View();


        }

        public ActionResult writeFeedback()
        {

            return View("writeFeedback");
        }


        [HttpPost]
        public ActionResult storeFeedback(Account adm)
        {




            string mainconni = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysqli = new MySqlConnection(mainconni);
            string bruker = Request.Cookies["UserCookie"].Value;
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            string queryi = "INSERT INTO admin_inbox VALUES(null, '" + bruker + "', '" + adm.subject + "','" + adm.feedback_text + "','" + Timestamp + "',false );";
            MySqlCommand commi = new MySqlCommand(queryi);
            commi.Connection = mysqli;
            mysqli.Open();
            int dri = commi.ExecuteNonQuery();


            mysqli.Close();
            return RedirectToAction("MyMessages", "Account");


        }


        public ActionResult MyMessages()
        {
            List<Account> sent = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM admin_inbox where user_username='" + name + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                sent.Add(new Account
                {
                    message_ID = dr.GetInt32(dr.GetOrdinal("message_ID")),
                    subject = dr["subject"].ToString(),
                    feedback_text = dr["message"].ToString(),



                });
            }
            mysql.Close();

            ViewData["sent"] = sent;
            answaredMessage();
            return View("MyMessages");
        }

        public void answaredMessage()
        {
            List<Account> answared = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM user_inbox where user_username='" + name + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                answared.Add(new Account
                {
                    message_ID = dr.GetInt32(dr.GetOrdinal("message_ID")),
                    admin = dr["admin_username"].ToString(),
                    user = dr["user_username"].ToString(),
                    subject = dr["subject"].ToString(),
                    feedback_text = dr["message"].ToString(),



                });
            }
            mysql.Close();

            ViewData["answared"] = answared;

        }

        public ActionResult ShowAccount()
        {
            List<Account> list1 = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM user where username='" + name + "';";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new Account
                {
                    Name = dr["Username"].ToString(),
                    Password = dr["password"].ToString(),
                    location = dr["city"].ToString(),
                    email = dr["email"].ToString(),
                    full_name = dr["full_name"].ToString(),
                    phone_NR = dr.GetInt32(dr.GetOrdinal("phone_NR")),
                });
            }
           
            mysql.Close();
            ViewData["list1"] = list1;
            return View("ShowAccount");

        }

       

        [HttpPost]
        public ActionResult EditAccount(Account user)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "UPDATE user set city='" + user.location + "', email='" + user.email + "', full_name='" + user.full_name + "', phone_nr='" + user.phone_NR + "'  WHERE username='" + user.Name + "';";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            mysql.Close();
            MyAccount();
            return View("MyAccount");
        }
        public ActionResult ShowTags()
        {
            List<Account> list1 = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM tag;";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new Account
                {
                    tagname = dr["tag"].ToString(),
                });
            }
            mysql.Close();
            getUserTag();
            ViewData["list1"] = list1;
            return View("ShowTags");

        }

        public ActionResult getUserTag()
        {
            List<Account> list2 = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM user_tag where username='" + name + "';";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list2.Add(new Account
                {
                    tagname = dr["tag"].ToString(),
                });
            }
            ViewData["list2"] = list2;
            return null;
        }
        [HttpPost]
        public ActionResult EditTags(Account user)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "INSERT INTO user_tag VALUES ('" + user.tagname + "' , '" + name + "')";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            mysql.Close();
            MyAccount();
            return View("MyAccount");
        }
        [HttpPost]
        public ActionResult DeleteTags(Account user)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "DELETE FROM user_tag WHERE tag = '"+user.tagname +"' AND username ='"+name +"';";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            mysql.Close();
            MyAccount();
            return View("MyAccount");

        }
    }
}
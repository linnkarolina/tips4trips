using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace WebApplicationFinal.Controllers
{
    public class AccountController : Controller
    {

     

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
            getUserTag();
            ViewData["List1"] = list1;
            
            return View();


        }

     




       

       
        public ActionResult ShowAccount(){
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
            getUserTag();
            return View("ShowAccount");
            
        }

       

        [HttpPost]
        public ActionResult EditAccount(Account user)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "UPDATE user set city='"+user.location+"', email='"+user.email+"', full_name='"+user.full_name+"', phone_nr='"+user.phone_NR+"'  WHERE username='" + name + "';";
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
            string query = "SELECT tag.* FROM tag WHERE tag.tag NOT IN(SELECT user_tag.tag FROM user_tag where user_tag.username = '"+name+"');";
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
            mysql.Close();
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

        public ActionResult DeleteTags(Account user)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "DELETE FROM user_tag WHERE tag='" + user.tagname + "' and username= '" + name + "';";
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

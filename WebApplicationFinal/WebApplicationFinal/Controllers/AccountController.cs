﻿using System;
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
            string query = "SELECT * FROM user where username='"+acc.Name+"' and password='"+acc.Password+"'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
           if(dr.Read())
            {
                HttpCookie Cridentials = new HttpCookie("UserCookie");
                Cridentials["name"] = acc.Name;
                Cridentials.Expires = DateTime.Now.AddHours(5);
                Response.Cookies.Add(Cridentials);
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
            string query = "INSERT IGNORE INTO user (username, password, location, email, full_name, phone_NR) VALUES ('" + acc.Name + "', '" + acc.Password + "', '" + acc.location + "', '" + acc.email + "', '" + acc.full_name + "', '" + acc.phone_NR + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            if (dr != 0)
            {
                HttpCookie cridentials = new HttpCookie("UserCookie");
                cridentials["name"] = acc.Name;
                cridentials.Expires = DateTime.Now.AddHours(5);
                Response.Cookies.Add(cridentials);
                Response.Cookies["UserCookie"].Value = "name=" + acc.Name;
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
            
            Response.Cookies["UserCookie"].Value = null;
           
            Response.Redirect("../Home/Index", false);
            return null;
        }
    }
    }

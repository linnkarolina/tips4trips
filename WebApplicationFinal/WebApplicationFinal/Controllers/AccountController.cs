﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using MySql.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
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
            string query = "INSERT INTO user (username, password, location, email, full_name, phone_NR) VALUES ('" + acc.Name + "', '" + acc.Password + "', '" + acc.location + "', '" + acc.email + "', '" + acc.full_name + "', '" + acc.phone_NR + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            if (dr!=null)
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
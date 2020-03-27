using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplicationFinal.Models;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WebApplicationFinal.Controllers
{
    public class MysqlController : Controller
    {
        // GET: Mysql

      
        public ActionResult ShowAdmin()
        {
            List<MysqlClass> list1 = new List<MysqlClass>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
          
            string query = "SELECT * FROM admin";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read()) {
                list1.Add(new MysqlClass
                {
                    ID_admin = dr["ID_admin"].ToString(),
                    Username = dr["Username"].ToString(),
                    Password = dr["password"].ToString(),

                }); 
            }
            mysql.Close();
            return View(list1);
        }

      [HttpPost]
        public ActionResult DeleteAdmin(MysqlClass adm)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "DELETE FROM admin WHERE username='" + adm.Name + "';";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            if (dr != null)
            {
                mysql.Close();
                return View("Success");
            }
            else
            {
                mysql.Close();
                return View("Fail");
            }

        }

        [HttpPost]
        public ActionResult UpdateAdmin(MysqlClass adm)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "Update admin set password='"+adm.Pass+"'  WHERE username='" + adm.Name + "';";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            int dr = comm.ExecuteNonQuery();
            if (dr != null)
            {
                mysql.Close();
                return View("Success");
            }
            else
            {
                mysql.Close();
                return View("Fail");
            }

        }



    }
}
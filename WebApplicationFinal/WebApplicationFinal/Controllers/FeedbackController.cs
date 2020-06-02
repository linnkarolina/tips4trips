using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using WebApplicationFinal.Models;
namespace WebApplicationFinal.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: AccountEdit
        public ActionResult Index()
        {
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
            DateTime time = DateTime.Now;              // Use current time
            string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 
            string queryi = "INSERT INTO admin_inbox VALUES(null, '" + bruker + "', '" + adm.subject + "','" + adm.feedback_text + "','" + time.ToString(format) + "',false );";
            MySqlCommand commi = new MySqlCommand(queryi);
            commi.Connection = mysqli;
            mysqli.Open();
            int dri = commi.ExecuteNonQuery();


            mysqli.Close();
            return RedirectToAction("MyMessages", "Feedback");


        }

        public ActionResult MyMessages()
        {
            inbox();
            outbox();
            return View("MyMessages");
        }

        private void inbox()
        {
            List<Account> inbox = new List<Account>();
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
                inbox.Add(new Account
                {
                    message_ID = dr.GetInt32(dr.GetOrdinal("message_ID")),
                    subject = dr["subject"].ToString(),

                    feedback_text = dr["message"].ToString(),
                });
            }
            mysql.Close();

            ViewData["inbox"] = inbox;
        }

        private void outbox()
        {
            List<Account> outbox = new List<Account>();
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
                outbox.Add(new Account
                {
                    message_ID = dr.GetInt32(dr.GetOrdinal("message_ID")),
                    subject = dr["subject"].ToString(),
                    feedback_text = dr["message"].ToString(),
                });
            }
            mysql.Close();

            ViewData["outbox"] = outbox;
        }

        public ActionResult Message(Account acc)
        {
            OutboxMessage(acc);
            InboxMessage(acc);
           
            return View("Message");
        }

        private void OutboxMessage(Account acc)
        {
            List<Account> spesificOutbox = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM admin_inbox where user_username='" + name + "' and message_ID='" + acc.clicked_value + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                spesificOutbox.Add(new Account
                {
                    message_ID = dr.GetInt32(dr.GetOrdinal("message_ID")),
                    
                    subject = dr["subject"].ToString(),
                    feedback_text = dr["message"].ToString(),



                });

            }
            mysql.Close();

            ViewData["spesificOutbox"] = spesificOutbox;
        }

        private void InboxMessage(Account acc)
        {
            List<Account> spesificInbox = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM user_inbox where user_username='" + name + "' and message_ID='" + acc.clicked_value + "'";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                spesificInbox.Add(new Account
                {
                    message_ID = dr.GetInt32(dr.GetOrdinal("message_ID")),
                    admin = dr["admin_username"].ToString(),
                    subject = dr["subject"].ToString(),
                    feedback_text = dr["message"].ToString(),



                });

            }
            mysql.Close();

            ViewData["spesificInbox"] = spesificInbox;
        }

    }
}
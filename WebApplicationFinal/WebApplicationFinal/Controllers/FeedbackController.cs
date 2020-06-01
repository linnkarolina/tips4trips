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
            return View("MyMessages");
        }

        public ActionResult Message(Account acc)
        {
            List<Account> sent = new List<Account>();
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
                sent.Add(new Account
                {
                    message_ID = dr.GetInt32(dr.GetOrdinal("message_ID")),
                    subject = dr["subject"].ToString(),
                    feedback_text = dr["message"].ToString(),



                });

            }
            mysql.Close();

            ViewData["sent"] = sent;
            answaredMessage(acc.subject);
            return View("Message");
        }

        public void answaredMessage(String Subject)
        {
            List<Account> answared = new List<Account>();
            string mainconn = ConfigurationManager.ConnectionStrings["app2000"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string name = Request.Cookies["UserCookie"].Value;
            string query = "SELECT * FROM user_inbox where user_username='" + name + "' and subject='" + Subject + "'; ";
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

    }
}
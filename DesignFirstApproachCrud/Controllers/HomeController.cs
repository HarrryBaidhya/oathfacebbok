using DesignFirstApproachCrud.Models;
using Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DesignFirstApproachCrud.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TechDBEntities"].ConnectionString);

        public ActionResult Index()

        {
            Session["language"] = "en";
            if (Session["UserName"] == null)
            {
                AdminUser Model = new AdminUser();
                return RedirectToAction("LoginAdmin", "Home");
            }
            if (Session["Username"] != null)
            {
                ViewData["UserName"] = Session["Username"];
                return View();
            }
            return RedirectToAction("LoginAdmin");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LoginAdmin()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "1299702418011110",
                redirect_uri = "https://localhost:44386/Home/Facebook",
                scope = "public_profile,email"
            });
            ViewBag.Url = loginUrl;
            return View();
        }

        [OverrideActionFilters]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index1(AdminUser model, string submit)
        {
            if (ModelState.IsValid)
            {
                var x = Login(model);
                return RedirectToAction(x.Item1, x.Item2, new { area = x.Item3 });

            }
            return View();
        }

        public Tuple<string, string, string> Login(AdminUser common)
        {

            //var sGuid = new Guid().ToString();
            var sGuid = Session.SessionID;
            common.session_id = sGuid;
            SqlDataAdapter Da = new SqlDataAdapter("sproc_LoginAdmin", conn);
            Da.SelectCommand.CommandType = CommandType.StoredProcedure;
            Da.SelectCommand.Parameters.AddWithValue("@UserName", common.UserName);
            Da.SelectCommand.Parameters.AddWithValue("@Password", common.Password);
            DataTable dt = new DataTable();
            Da.Fill(dt);


            List<AdminUser> listLo = new List<AdminUser>();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AdminUser lc = new AdminUser();
                    lc.UserName = dt.Rows[i]["UserName"].ToString();
                    lc.AId = dt.Rows[i]["UserID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["UserID"]) : 0;
                    //lc.AId = Convert.ToInt32(dt.Rows[i]["UserID"]):dt.Rows[i]["UserID"].ToString();
                    lc.Code = Convert.ToInt32(dt.Rows[i]["code"]);
                    listLo.Add(lc);
                }


            }

            var kk = listLo;
            var obj = Newtonsoft.Json.JsonConvert.SerializeObject(listLo);

            var dbres = new AdminUser
            {
                AId = listLo[0].AId,
                UserName = listLo[0].UserName,
                Message = listLo[0].Message,
                Code = listLo[0].Code,
            };
            try
            {

                if (dbres.Code == 0)
                {

                    //Session["SessionGuid"] = data.SessionId;
                    Session["SessionGuid"] = sGuid;
                    Session["UserId"] = dbres.AId;
                    Session["UserName"] = dbres.UserName;
                    Session["language"] = "en";
                    return new Tuple<string, string, string>("Index", "Home", "");
                }
                TempData["msg"] = dbres.Message;
                return new Tuple<string, string, string>("LoginAdmin", "Home", "");
            }
            catch (Exception)
            {
                TempData["msg"] = "Something Went Wrong";
                return new Tuple<string, string, string>("LoginAdmin", "Home", "");

            }

        }

        [OverrideActionFilters]
        public ActionResult Signout()
        {
            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();
            AbandonSession();
            return RedirectToAction("", "Home");
        }

        public void AbandonSession()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
        }


        [Route("Facebook")]
        public ActionResult Facebook(string code)
        {
          
            var fb = new FacebookClient();
            dynamic result = fb.Get("/oauth/access_token", new
            {
                client_id = "1299702418011110",
                client_secret = "9f96f460c9aab07d47953675f7c64daf",
                redirect_uri = "https://localhost:44386/Home/Facebook",
                code = code
            });

            fb.AccessToken = result.access_token;

            dynamic me = fb.Get("/me?fields=name,email,gender");
            string name = me.name;
            string email = me.email;
            string gender = me.gender;
            string phone = me.phone;
            Session["UserName"] = me.name;
            return RedirectToAction("Index");
            return View();
        }




    }
}
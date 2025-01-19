using DesignFirstApproachCrud.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DesignFirstApproachCrud.Controllers
{
    public class UserSettingController : Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TechDBEntities"].ConnectionString);
        
        // GET: UserSetting
        public ActionResult Index()
        {
            SqlDataAdapter Da = new SqlDataAdapter("sproc_UserBroadcast", conn);
            UserSetting loc = new UserSetting();
            Da.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            Da.Fill(dt);
            List<UserSetting> listLo = new List<UserSetting>();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserSetting lc = new UserSetting();
                    lc.FirstName = dt.Rows[i]["FirstName"].ToString();
                    //lc.UserId = Convert.ToInt32(dt.Rows[i]["Id"]);
                    lc.UserId = dt.Rows[i]["Id"].ToString();
                    lc.Email = dt.Rows[i]["Email"].ToString();
                    lc.LastName = dt.Rows[i]["LastName"].ToString();
                    lc.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    listLo.Add(lc);
                }

            }
            if (listLo.Count > 0)
            {
               return View(listLo);

            }
            else
            {
             return View();

            }




        }
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(UserSetting user)
        {
            string message = " Password Changes succesfully";

            TempData["msg"] = message;


            return View();
        }

    }
}
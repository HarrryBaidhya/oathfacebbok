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
    public class GamesController : Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TechDBEntities"].ConnectionString);
        // GET: Games
        public ActionResult Index()
        {

            SqlDataAdapter Da = new SqlDataAdapter("Sproc_Gamelis", conn);
            Game loc = new Game();
            Da.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            Da.Fill(dt);
            List<Game> listLo = new List<Game>();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Game lc = new Game();
                    lc.GameName = dt.Rows[i]["GameName"].ToString();
                    lc.GID = Convert.ToInt32(dt.Rows[i]["GId"]);
                    lc.GameTypes = dt.Rows[i]["GameType"].ToString();
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

        public ActionResult AddGames()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddGames(Game gm)
        {
            string msg = "";
            if (gm != null)
            {
                SqlCommand cmd = new SqlCommand("GameInsert", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", gm.GameName);
                cmd.Parameters.AddWithValue("@Gametype", gm.GameTypes);
                cmd.Parameters.AddWithValue("@Video", gm.Video);
                cmd.Parameters.AddWithValue("@flag", 'I');

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                if (i < 0)
                {
                    msg = "Data Save succesfully";
                    return RedirectToAction("Index", "Games");
                }
                else
                {
                    msg = "error";
                }

            }
            return View();
        }


        public ActionResult ScheduleManagent()
        {
            return View() ;

        }
    }
}
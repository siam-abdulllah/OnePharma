using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.Reports.Models.DAO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Reports.Controllers
{
    [LogInChecker]
    public class AMWiseSalesAchievementController : Controller
    {
        AMWiseSalesAchievementDAO amWiseSalesAchievementDao = new AMWiseSalesAchievementDAO();
        public ActionResult frmAMWiseSalesValueAchievement()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetAMWiseSalesValueAchievement(string fromDate, string toDate)
        {
            var data = amWiseSalesAchievementDao.GetAMWiseSalesValueAchievement(fromDate, toDate);
            if (data.Count > 0)
            {
                return Json(new { Data = data, Status = "Ok" });
            }

            else
            {
                return Json(new { Data = data, Status = "Not Ok" });
            }

        }
    }
}
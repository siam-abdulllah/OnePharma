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
    public class ZoneWiseSalesAchievementController : Controller
    {
        ZoneWiseSalesAchievementDAO zoneWiseSalesAchievementDao=new ZoneWiseSalesAchievementDAO();
        public ActionResult frmZoneWiseSalesValueAchievement()
        {
            return View();
        }
        public ActionResult frmZoneWiseSalesProductAchievement()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetZoneWiseSalesValueAchievement(string fromDate, string toDate)
        {
            var data = zoneWiseSalesAchievementDao.GetZoneWiseSalesValueAchievement(fromDate,toDate);
            if (data.Count > 0)
            {
                return Json(new { Data = data, Status = "Ok" });
            }

            else
            {
                return Json(new { Data = data, Status = "Not Ok" });
            }

        }
        //[HttpPost]
        //public ActionResult GetZoneWiseSalesProductAchievement(string fromDate, string toDate)
        //{
        //    var data = zoneWiseSalesAchievementDao.GetZoneWiseSalesProductAchievement(fromDate,toDate);
        //    if (data.Count > 0)
        //    {
        //        return Json(new { Data = data, Status = "Ok" });
        //    }

        //    else
        //    {
        //        return Json(new { Data = data, Status = "Not Ok" });
        //    }

        //}
        [HttpPost]
        public ActionResult GetDashboardData()
        {
            var data = zoneWiseSalesAchievementDao.GetDashboardData();
            //return Json(data, JsonRequestBehavior.AllowGet);
            return Json(new { Data = data, Status = "Ok" });
        }
    }
}
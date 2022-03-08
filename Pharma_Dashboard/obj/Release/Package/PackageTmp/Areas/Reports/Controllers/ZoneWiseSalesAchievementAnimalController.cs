using PAsia_Dashboard.Areas.Reports.Models.DAO;
using PAsia_Dashboard.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAsia_Dashboard.Areas.Reports.Controllers
{
    [LogInChecker]
    public class ZoneWiseSalesAchievementAnimalController : Controller
    {
        ZoneWiseSalesAchievementAnimalDAO zoneWiseSalesAchievementAnimalDao = new ZoneWiseSalesAchievementAnimalDAO();
        public ActionResult frmZoneWiseSalesAchievementAnimal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetZoneWiseSalesValueAchievementAnimal(string fromDate, string toDate)
        {
            var data = zoneWiseSalesAchievementAnimalDao.GetZoneWiseSalesValueAchievementAnimal(fromDate, toDate);
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
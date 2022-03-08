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
    public class RSMWiseSalesAchievementAnimalController : Controller
    {
        RSMWiseSalesAchievementAnimalDAO rsmWiseSalesAchievementAnimalDao = new RSMWiseSalesAchievementAnimalDAO();
        public ActionResult frmRSMWiseSalesAchievementAnimal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetRSMWiseSalesValueAchievementAnimal(string fromDate, string toDate)
        {
            var data = rsmWiseSalesAchievementAnimalDao.GetRSMWiseSalesValueAchievementAnimal(fromDate, toDate);
            return data.Count > 0 ? Json(new { Data = data, Status = "Ok" }) : Json(new { Data = data, Status = "Not Ok" });

        }
    }
}
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
    public class MPOWiseSalesAchievementAnimalController : Controller
    {
        MPOWiseSalesAchievementAnimalDAO mpoWiseSalesAchievementAnimalDao = new MPOWiseSalesAchievementAnimalDAO();
        public ActionResult frmMPOWiseSalesValueAchievementAnimal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetMPOWiseSalesValueAchievementAnimal(string fromDate, string toDate)
        {
            var data = mpoWiseSalesAchievementAnimalDao.GetMPOWiseSalesValueAchievementAnimal(fromDate, toDate);
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
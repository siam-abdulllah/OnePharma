using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.Reports.Models.DAO;

namespace PAsia_Dashboard.Areas.Reports.Controllers
{
    public class ProdWiseSalesAchievementController : Controller
    {
        ProdWiseSalesAchievementDAO prodWiseSalesAchievementDao = new ProdWiseSalesAchievementDAO();
        [HttpPost]
        public ActionResult GetProdWiseSalesAchievement(string fromDate, string toDate)
        {
            var data = prodWiseSalesAchievementDao.GetProdWiseSalesAchievement(fromDate, toDate);
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
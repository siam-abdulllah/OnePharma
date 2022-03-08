using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.Reports.Models.DAO;

namespace PAsia_Dashboard.Areas.Reports.Controllers
{
    public class ProdWiseSalesAchievementAnimalController : Controller
    {
        ProdWiseSalesAchievementAnimalDAO prodWiseSalesAchievementAnimalDao = new ProdWiseSalesAchievementAnimalDAO();
        [HttpPost]
        public ActionResult GetProdWiseSalesAchievementAnimal(string fromDate, string toDate)
        {
            var data = prodWiseSalesAchievementAnimalDao.GetProdWiseSalesAchievementAnimal(fromDate, toDate);
            return data.Count > 0 ? Json(new { Data = data, Status = "Ok" }) : Json(new { Data = data, Status = "Not Ok" });

        }
    }
}
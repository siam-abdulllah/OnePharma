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
    public class DepotCommCurrentStockController : Controller
    {
        // GET: Reports/DepotCommCurrentStock
        DepotCommCurrentStockDAO depotCommCurrentStockDao = new DepotCommCurrentStockDAO();
        public ActionResult frmDepotCommCurrentStock()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDepotCommCurrentStock(string dateParam)
        {
            var data = depotCommCurrentStockDao.GetDepotCommCurrentStock(dateParam);
            if (data.Count > 0)
            {
                return Json(new { Data = data, Status = "Ok" });
            }
            else
            {
                return Json(new { Data = data, Status = "Not Ok" });
            }
           

        }
        [HttpPost]
        public ActionResult GetDashboardData()
        {
            var data = depotCommCurrentStockDao.GetDashboardData();
            return Json(new { Data = data, Status = "Ok" });
        }
    }
}
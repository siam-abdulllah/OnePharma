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
    public class DepotPPMCurrentStockController : Controller
    {
       DepotPPMCurrentStockDAO depotPpmCurrentStockDao=new DepotPPMCurrentStockDAO();
        public ActionResult frmDepotPPMCurrentStock()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDepotPPMCurrentStock(string dateParam)
        {
            var data = depotPpmCurrentStockDao.GetDepotPPMCurrentStock(dateParam);
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
            var data = depotPpmCurrentStockDao.GetDashboardData();
            return Json(new { Data = data, Status = "Ok" });
        }
    }
}
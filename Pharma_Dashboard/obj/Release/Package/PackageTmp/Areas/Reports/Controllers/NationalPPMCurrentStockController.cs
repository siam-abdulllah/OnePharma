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
    public class NationalPPMCurrentStockController : Controller
    {
        NationalPPMCurrentStockDAO nationalPpmCurrentStockDao=new NationalPPMCurrentStockDAO();
        // GET: Reports/NationalPPMCurrentStock
        public ActionResult frmNationalPPMCurrentStock()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetNationalPPMCurrentStock(string dateParam)
        {
            var data = nationalPpmCurrentStockDao.GetNationalPPMCurrentStock(dateParam);
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
            var data = nationalPpmCurrentStockDao.GetDashboardData();
            //return Json(data, JsonRequestBehavior.AllowGet);
            return Json(new { Data = data, Status = "Ok" });
        }
    }
}
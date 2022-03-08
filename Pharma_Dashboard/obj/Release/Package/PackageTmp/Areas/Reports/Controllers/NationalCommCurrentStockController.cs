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
    public class NationalCommCurrentStockController : Controller 
    {
        //[LogInChecker]
        // GET: Reports/NationalCommCurrentStock
        NationalCommCurrentStockDAO nationalCommCurrentStockDao=new NationalCommCurrentStockDAO();
        public ActionResult frmNationalCommCurrentStock()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetNationalCommCurrentStock(string dateParam)
        {
            var data = nationalCommCurrentStockDao.GetNationalCommCurrentStock( dateParam);
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
            var data = nationalCommCurrentStockDao.GetDashboardData();
            //return Json(data, JsonRequestBehavior.AllowGet);
            return Json(new {Data = data, Status = "Ok" });
        }
    }
}
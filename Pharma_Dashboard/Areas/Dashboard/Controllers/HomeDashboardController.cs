using PAsia_Dashboard.Areas.Dashboard.Models.DAO;
using PAsia_Dashboard.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAsia_Dashboard.Areas.Dashboard.Controllers
{
    [LogInChecker]
    public class HomeDashboardController : Controller
    {
        
        HomeDashboardDAO homeDashboardDao = new HomeDashboardDAO();
        public ActionResult frmHomeDashboard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDashboardData(string toDate)
        {
            var data = homeDashboardDao.GetDashboardData(toDate);
            return Json(new { BarChartData = homeDashboardDao.ListReturn, Data = data, Status= "Ok" });
        }
        //[HttpGet]
        //public ActionResult GetBarChartData()
        //{
        //    var data = homeDashboardDao.GetBarChartData();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        
    }
}
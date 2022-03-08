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
    public class DashboardAnimalController : Controller
    {

        DashboardAnimalDAO dashboardAnimalDao = new DashboardAnimalDAO();
        public ActionResult frmDashboardAnimal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDashboardData(string toDate)
        {
            var data = dashboardAnimalDao.GetDashboardData(toDate);
            return Json(new { BarChartData = dashboardAnimalDao.ListReturn, Data = data, Status = "Ok" });
        }
    }
}
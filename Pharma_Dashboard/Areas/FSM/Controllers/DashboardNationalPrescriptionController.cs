using PAsia_Dashboard.Areas.FSM.Models.DAL;
using PAsia_Dashboard.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAsia_Dashboard.Areas.FSM.Controllers
{
    [LogInChecker]
    public class DashboardNationalPrescriptionController : Controller
    {
        DashboardNationalPrescriptionDAO PrimaryDAO = new DashboardNationalPrescriptionDAO();
        public ActionResult frmDashboardNationalPrescription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetNationalPrescriptionData()
        {
            var data = PrimaryDAO.GetNationalPrescriptionData();
            return Json(new { Data = data, Status = "Ok" });
        }


        [HttpPost]
        public ActionResult GetGridData()
        {
            var data = PrimaryDAO.GetGridData();
            return Json(new { Data = data, Status = "Ok" });
        }
    }
}
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
    public class DashboardDoctorPrescriptionController : Controller
    {
       
        DashboardDoctorPrescriptionDAO PrimaryDAO = new DashboardDoctorPrescriptionDAO();
        // GET: /FSM/DoctorPrescriptionDashboard/
        public ActionResult frmDashboardDoctorPrescription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDoctorPrescriptionData()
        {
            var data = PrimaryDAO.GetDoctorPrescriptionData();
            return Json(new { Data = data, Status = "Ok" });
        }


        [HttpPost]
        public ActionResult GetGridData()
        {
            var listData = PrimaryDAO.GetGridData();
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            //return Json(new { Data = data, Status = "Ok" });
            return data;




        }
	}
}
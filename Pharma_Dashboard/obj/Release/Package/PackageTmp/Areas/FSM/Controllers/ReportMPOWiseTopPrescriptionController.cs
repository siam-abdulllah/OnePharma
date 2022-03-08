using PAsia_Dashboard.Areas.FSM.Models.DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.FSM.Controllers
{
    [LogInChecker]
    public class ReportMPOWiseTopPrescriptionController : Controller
    {
        ReportMPOWiseTopPrescriptionDAO reportMpoWiseTopPrescriptionDAO=new ReportMPOWiseTopPrescriptionDAO();
        public ActionResult frmReportMPOWiseTopPrescription()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetMPOWiseTopPrescriptionData(string depotCode, string zoneCode, string regionCode, string areaCode, string territoryCode, string fromDate, string toDate)
        {
            var listData = reportMpoWiseTopPrescriptionDAO.GetMPOWiseTopPrescriptionData(depotCode, zoneCode, regionCode, areaCode, territoryCode, fromDate, toDate);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

        }
    }
}
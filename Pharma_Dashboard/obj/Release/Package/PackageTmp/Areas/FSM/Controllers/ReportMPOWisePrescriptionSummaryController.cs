using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.FSM.Models.DAL.DAO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.FSM.Controllers
{
    [LogInChecker]
    public class ReportMPOWisePrescriptionSummaryController : Controller
    {
        ReportMPOWisePrescriptionSummaryDAO reportMpoWisePrescriptionSummaryDAo=new ReportMPOWisePrescriptionSummaryDAO();
        public ActionResult frmReportMPOWisePrescriptionSummary()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetMPOWisePrescriptionSummaryData(string depotCode, string zoneCode, string regionCode, string areaCode, string territoryCode, string fromDate, string toDate)
        {
            try
            {
                var listData = reportMpoWisePrescriptionSummaryDAo.GetMPOWisePrescriptionSummaryData(depotCode, zoneCode, regionCode, areaCode, territoryCode, fromDate, toDate);
                if (listData != null && reportMpoWisePrescriptionSummaryDAo.ExceptionReturn == null)
                {
                    var data = Json(listData, JsonRequestBehavior.AllowGet);
                    data.MaxJsonLength = int.MaxValue;
                    return data;
                }
                else
                    return Json(new { Status = reportMpoWisePrescriptionSummaryDAo.ExceptionReturn });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { Status = e.Message });
            }

        }

    }
}
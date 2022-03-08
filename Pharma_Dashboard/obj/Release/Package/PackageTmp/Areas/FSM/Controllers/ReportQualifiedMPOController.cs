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
    public class ReportQualifiedMPOController : Controller
    {
        private readonly ReportQualifiedMPODAO reportQualifiedMpoDAO=new ReportQualifiedMPODAO();
        
        public ActionResult frmReportQualifiedMPO()
        {
            return View();
        }
        [HttpPost]
        //public ActionResult GetReportQualifiedMPOData(string depotCode, string zoneCode, string regionCode, string areaCode, string territoryCode, string fromDate, string toDate)
        public ActionResult GetQualifiedMPOData(string fromDate, string toDate)
        {
            try
            {
                //var listData = reportQualifiedMpoDAO.GetReportQualifiedMPOData(depotCode, zoneCode, regionCode, areaCode, territoryCode, fromDate, toDate);
                var listData = reportQualifiedMpoDAO.GetQualifiedMPOData( fromDate, toDate);
                if (listData != null && reportQualifiedMpoDAO.ExceptionReturn == null)
                {
                    var data = Json(listData, JsonRequestBehavior.AllowGet);
                    data.MaxJsonLength = int.MaxValue;
                    return data;
                }
                else
                    return Json(new { Status = reportQualifiedMpoDAO.ExceptionReturn });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { Status = e.Message });
            }

        }

    }
}
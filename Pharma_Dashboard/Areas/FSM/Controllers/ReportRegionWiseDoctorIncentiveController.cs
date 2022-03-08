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
    public class ReportRegionWiseDoctorIncentiveController : Controller
    {
        ReportRegionWiseDoctorIncentiveDAO regionWiseDoctorIncentiveDAO=new ReportRegionWiseDoctorIncentiveDAO();
        public ActionResult frmRegionWiseDoctorIncentive()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetRegionWiseDoctorIncentiveData(string fromDate, string toDate)
        {
            try
            {
                var listData = regionWiseDoctorIncentiveDAO.GetRegionWiseDoctorIncentiveData( fromDate, toDate);
                if (listData != null && regionWiseDoctorIncentiveDAO.ExceptionReturn == null)
                {
                    var data = Json(listData, JsonRequestBehavior.AllowGet);
                    data.MaxJsonLength = int.MaxValue;
                    return data;
                }
                else
                    return Json(new { Status = regionWiseDoctorIncentiveDAO.ExceptionReturn });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { Status = e.Message });
            }

        }

    }
}
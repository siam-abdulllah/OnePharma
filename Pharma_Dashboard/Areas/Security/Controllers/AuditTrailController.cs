using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.Security.DAO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.Controllers
{
    public class AuditTrailController : Controller
    {
        private readonly AuditTrailDAO auditTrailDao = new AuditTrailDAO();
        public ActionResult frmRptAuditTrail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetAuditTrail(string FromDate, string ToDate, string Action_By, string Action_Type)
        {
            var data = auditTrailDao.GetAuditTrail(FromDate,ToDate, Action_By, Action_Type);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

    }
}
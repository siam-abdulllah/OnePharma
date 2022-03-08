using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.Security.DAO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.Controllers
{
    [LogInChecker]
    public class ChangPassController : Controller
    {
        private readonly ChngPassDAO chngPassDAO = new ChngPassDAO();
        // GET: Security/ChangPass
        public ActionResult frmChngPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckCurrentPassword(string currentPassword)
        {
            bool status = chngPassDAO.CheckCurrentPassword(currentPassword);
            return Json(new { Stat = status }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdatePassword(string Password)
        {
            try
            {
                if (chngPassDAO.UpdatePassword(Password))
                {
                    return Json(new { Mode = chngPassDAO.IUMode, Status = "Yes" });
                }
                return View("frmChngePass");
            }
            catch (Exception)
            {
                return View("frmChngePass");
            }
        }
    }
}
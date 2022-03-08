using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.Security.DAO;
using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.Controllers
{
    [LogInChecker]
    public class RLController : Controller
    {
        // GET: Security/RL
        private readonly RLDAO rlDao = new RLDAO();
        public ActionResult frmRL()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetRoleList()
        {
            var data = rlDao.GetRoleList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OperationsMode(SecRL master)
        {
            try
            {
                if (rlDao.SaveUpdate(master))
                {
                    return Json(new { ID = rlDao.MaxID, Mode = rlDao.IUMode, Status = "Yes" });
                }
                return View("frmMHConfig");
            }
            catch (Exception e)
            {
                if (e.Message.Substring(0, 9) == "ORA-00001")
                    return Json(new { Status = "Error:ORA-00001,Data already exists!" });//Unique Identifier.
                if (e.Message.Substring(0, 9) == "ORA-02292")
                    return Json(new { Status = "Error:ORA-02292,Data already exists!" });//Child Record Found.
                if (e.Message.Substring(0, 9) == "ORA-12899")
                    return Json(new { Status = "Error:ORA-12899,Data Value Too Large!" });//Value Too Large.
                return Json(new { Status = "! Error : Error Code:" + e.Message.Substring(0, 9) });//Other Wise Error Found
            }
        }
    }
}
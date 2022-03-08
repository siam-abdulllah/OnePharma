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
    public class RoleConfController : Controller
    {
        private readonly RoleConfDAO roleConfDAO = new RoleConfDAO();
        public ActionResult frmRoleConf()
        {
            return View();
        }
        public ActionResult GetActiveEmployeeInfoList()
        {
            var data = roleConfDAO.GetActiveEmployeeInfoList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetEmployeeByRoleList(string roleId)
        {
            var data = roleConfDAO.GetEmployeeByRoleList(roleId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveRLConf(SecRoleConf master)
        {
            try
            {
                if (roleConfDAO.SaveRLConf(master))
                {
                    return Json(new { ID = roleConfDAO.MaxID, Mode = roleConfDAO.IUMode, Status = "Yes" });
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
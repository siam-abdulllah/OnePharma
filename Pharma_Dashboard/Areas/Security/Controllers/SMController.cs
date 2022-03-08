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
    public class SMController : Controller
    {
        private readonly SMDAO primaryDAO = new SMDAO();
        public ActionResult frmSM()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetSubMenuList()
        {
            var data = primaryDAO.GetSubMenuList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetSubMenuListNyMHID(string RL_ID, string MH_ID)
        {
            var data = primaryDAO.GetSubMenuListByMenuHeadId(RL_ID, MH_ID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OperationsMode(SecSM master)
        {
            try
            {
                if (primaryDAO.SaveUpdate(master))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                return View("frmSMConfig");
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
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
    public class MNConfController : Controller
    {
        private readonly MNConfDAO mNConfDAO = new MNConfDAO();
        public ActionResult frmMNConf()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetMNConfListByMHRL(string RL_ID, string MH_ID)
        {
            var data = mNConfDAO.GetMNConfListByMHRL(Convert.ToInt32(RL_ID), Convert.ToInt32(MH_ID));
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult OperationsMode(SecRoleMenuConf master)
        {
            try
            {
                if (mNConfDAO.SaveUpdate(master))
                {
                    return Json(new { ID = mNConfDAO.MaxID, Mode = mNConfDAO.IUMode, Status = "Yes" });
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
        public ActionResult DeleteMNConf(int ID)
        {
            if (mNConfDAO.DeleteMNConf(ID))
            {
                return Json(new { Code = mNConfDAO.MaxCode, ID = mNConfDAO.MaxID, Mode = mNConfDAO.IUMode, Status = "Yes" });
            }

            return Json(new { Status = "No" });
        }
    }
}
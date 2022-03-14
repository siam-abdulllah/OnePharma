using PAsia_Dashboard.Areas.Security.DAO;
using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAsia_Dashboard.Areas.Security.Controllers
{
    [LogInChecker]
    public class UserCreationController : Controller
    {
        // GET: Security/UserCreation
        private readonly UserCreationDAO userCreationDAO = new UserCreationDAO();
        public ActionResult frmUserCreation()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetActiveEmployeeInfoList()
        {
            var data = userCreationDAO.GetActiveEmployeeInfoList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserList()
        {
            var data = userCreationDAO.GetUserList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult OperationsMode(UserLogin userLogin, string userId)
        {
            try
            {
                if (userCreationDAO.SaveUpdate(userLogin, userId))
                {
                    return Json(new { ID = userCreationDAO.MaxID, Mode = userCreationDAO.IUMode, Status = "Yes" });
                }
                return View("frmUserCreation");
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
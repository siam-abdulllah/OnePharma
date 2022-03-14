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
    public class UserCreationOPAController : Controller
    {

        private readonly UserCreationOPADAO UserCreationOPADAO = new UserCreationOPADAO();

        // GET: Security/UserCreationOPA
        public ActionResult frmUserCreationOPA()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetActiveEmployeeInfoListOPA()
        {
            var data = UserCreationOPADAO.GetActiveEmployeeInfoListOPA();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserListOPA()
        {
            var data = UserCreationOPADAO.GetUserListOPA();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult OperationsMode(UserLogin userLogin, string userId)
        {
            try
            {
                if (UserCreationOPADAO.SaveUpdate(userLogin, userId))
                {
                    return Json(new { ID = UserCreationOPADAO.MaxID, Mode = UserCreationOPADAO.IUMode, Status = "Yes" });
                }
                return View("frmUserCreationOPA");
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
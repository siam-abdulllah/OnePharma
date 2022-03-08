using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAsia_Dashboard.Universal.Gateway
{
    public class ExceptionHandler:Controller
    {
        public ActionResult ErrorMsg(Exception e)
        {
            if (e.Message.Substring(0, 9) == "ORA-00001")
                return Json(new { Status = "Error:ORA-00001,Data already exists!" });//Unique Identifier.
            if (e.Message.Substring(0, 9) == "ORA-02292")
                return Json(new { Status = "Error:ORA-02292,Data already exists!" });//Child Record Found.
            if (e.Message.Substring(0, 9) == "ORA-12899")
                return Json(new { Status = "Error:ORA-12899,Data Value Too Large!" });//Value Too Large.
            return Json(new { Status = "! Error :" + e.Message.Substring(0, 150) });//Other Wise Error Found
        }
    }
}
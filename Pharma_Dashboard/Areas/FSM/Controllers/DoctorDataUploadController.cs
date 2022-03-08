using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.FSM.Models.BEL.BEO;
using PAsia_Dashboard.Areas.FSM.Models.DAL.DAO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.FSM.Controllers
{
    [LogInChecker]
    public class DoctorDataUploadController : Controller
    {
        
        private readonly DoctorDataUploadDAO doctorDataUploadDAO = new DoctorDataUploadDAO();
        public ActionResult frmDoctorDataUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                HttpPostedFileBase files = Request.Files[0];

                if (files == null)
                {
                    return Json(new { Status = "Not Ok" });
                }
                var fileName = Path.GetFileName(files.FileName);
                var physicalPath = Path.Combine(Server.MapPath("~/UploadDoc"), fileName);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
                files.SaveAs(physicalPath);
                return Json(new { fileName = fileName, physicalPath = physicalPath, Status = "Ok" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { Status = e.Message });
            }

        }
        [HttpPost]
        public ActionResult LoadExcelFile(string fileName, string physicalPath)
        {

            try
            {
                var excelData = doctorDataUploadDAO.LoadExcelFileDoctorData(fileName, physicalPath);
                if (excelData != null && doctorDataUploadDAO.ExceptionReturn==null)
                {
                    var data = Json(excelData, JsonRequestBehavior.AllowGet);
                    data.MaxJsonLength = int.MaxValue;
                    return data;
                }
                else
                return Json(new { Status = doctorDataUploadDAO.ExceptionReturn });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               // return Json(e.Message, JsonRequestBehavior.AllowGet);
                return Json(new { Status = e.Message });
            }

        }
        [HttpPost]
        public ActionResult OperationsMode(List<DoctorDataUploadInfo> model)
        {

            try
            {
                var message = doctorDataUploadDAO.SaveUpdate(model);
                return Json(new { Status = message });
            }

            catch (Exception e)
            {
                if (e.Message.Substring(0, 9) == "ORA-00001")
                    return Json(new { Status = "Error:ORA-00001,Data already exists!" });//Unique Identifier.
                else if (e.Message.Substring(0, 9) == "ORA-02292")
                    return Json(new { Status = "Error:ORA-02292,Data already exists!" });//Child Record Found.
                else if (e.Message.Substring(0, 9) == "ORA-12899")
                    return Json(new { Status = "Error:ORA-12899,Data Value Too Large!" });//Value Too Large.
                else
                    return Json(new { Status = "! Error : Error Code:" + e.Message.Substring(0, 9) });//Other Wise Error Found

            }
        }
        [HttpPost]
        public ActionResult GetDoctorGridData(string depotCode, string zoneCode, string regionCode, string areaCode, string territoryCode)
        {
            try
            {
                var listData = doctorDataUploadDAO.GetDoctorGridData(depotCode, zoneCode, regionCode, areaCode, territoryCode);
                if (listData != null && doctorDataUploadDAO.ExceptionReturn == null)
                {
                    var data = Json(listData, JsonRequestBehavior.AllowGet);
                data.MaxJsonLength = int.MaxValue;
                return data;
                }
                else
                    return Json(new { Status = doctorDataUploadDAO.ExceptionReturn });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { Status = e.Message });
            }

        }

    }
}
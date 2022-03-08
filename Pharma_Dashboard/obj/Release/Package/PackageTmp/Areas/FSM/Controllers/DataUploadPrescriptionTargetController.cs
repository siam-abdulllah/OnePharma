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
    public class DataUploadPrescriptionTargetController : Controller
    {
        
        private readonly DataUploadPrescriptionTargetDAO dataUploadPrescriptionTargetDAO = new DataUploadPrescriptionTargetDAO();
        DefaultDAO defaultDao=new DefaultDAO();
        public ActionResult frmDataUploadPrescriptionTarget()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoadExcelFile(string fileName, string physicalPath)
        {
            try
            {
                var excelData = dataUploadPrescriptionTargetDAO.LoadExcelFilePrescriptionData(fileName, physicalPath);
                if (excelData != null && dataUploadPrescriptionTargetDAO.ExceptionReturn == null)
                {
                    var data = Json(excelData, JsonRequestBehavior.AllowGet);
                    data.MaxJsonLength = int.MaxValue;
                    return data;
                }
                return Json(new { Status = dataUploadPrescriptionTargetDAO.ExceptionReturn });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(e.Message, JsonRequestBehavior.AllowGet);
               
            }
        }
        [HttpPost]
        public ActionResult OperationsMode(List<DataUploadPrescriptionTargetBEO> model,string  MonthNumber, string Year)
        {
            try
            {
                var status = dataUploadPrescriptionTargetDAO.SaveUpdate(model, MonthNumber, Year);
                return Json(new {Status = status});
            }
            catch (Exception e)
            {
                if (e.Message.Substring(0, 9) == "ORA-00001")
                    return Json(new {Status = "Error:ORA-00001,Data already exists!"}); //Unique Identifier.
                else if (e.Message.Substring(0, 9) == "ORA-02292")
                    return Json(new {Status = "Error:ORA-02292,Data already exists!"}); //Child Record Found.
                else if (e.Message.Substring(0, 9) == "ORA-12899")
                    return Json(new {Status = "Error:ORA-12899,Data Value Too Large!"}); //Value Too Large.
                else
                    return Json(new
                        {Status = "! Error : Error Code:" + e.Message.Substring(0, 9)}); //Other Wise Error Found
            }
        }
        [HttpPost]
        public ActionResult GetPrescriptionTargetData(string MonthNumber, string Year)
        {
            try
            {
                var listData = dataUploadPrescriptionTargetDAO.GetPrescriptionTargetData(  MonthNumber,  Year);
                if (listData != null && dataUploadPrescriptionTargetDAO.ExceptionReturn == null)
                {
                    var data = Json(listData, JsonRequestBehavior.AllowGet);
                data.MaxJsonLength = int.MaxValue;
                return data;
                }
                else
                    return Json(new { Status = dataUploadPrescriptionTargetDAO.ExceptionReturn });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { Status = e.Message });
            }
            

        }
        [HttpPost]
        public ActionResult GetMonthName()
        {
            try
            {
                var listData = defaultDao.GetMonthName();
                   return Json(listData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { Status = e.Message });
            }
            

        }
    }
}
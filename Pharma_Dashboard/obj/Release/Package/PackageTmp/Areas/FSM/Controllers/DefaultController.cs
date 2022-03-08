using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.FSM.Models.DAL.DAO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.FSM.Controllers
{
    [LogInChecker]
    public class DefaultController : Controller
    {
        private DefaultDAO defaultDAO = new DefaultDAO();

        [HttpPost]
        public ActionResult GetDepot()
        {
            var listData = defaultDAO.GetDepot();
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

        }
        [HttpPost]
        public ActionResult GetZoneByDepot(string DepotCode)
        {
            var listData = defaultDAO.GetZoneByDepot(DepotCode);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

        }
        [HttpPost]
        public ActionResult GetRegionByZone(string DepotCode,string ZoneCode)
        {
            var listData = defaultDAO.GetRegionByZone(DepotCode,ZoneCode);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

        }
        [HttpPost]
        public ActionResult GetAreaByRegion(string DepotCode, string ZoneCode,string RegionCode)
        {
            var listData = defaultDAO.GetAreaByRegion(DepotCode,ZoneCode,RegionCode);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

        }
        [HttpPost]
        public ActionResult GetTerritoryByArea(string DepotCode, string ZoneCode, string RegionCode,string AreaCode)
        {
            var listData = defaultDAO.GetTerritoryByArea(DepotCode,ZoneCode,RegionCode,AreaCode);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {


                HttpPostedFileBase files = Request.Files[0];

                if (files == null)
                {
                    return Json(new { Status = "Upload Failed!" });
                    //return Json("Upload Failed!", JsonRequestBehavior.AllowGet);
                }
                var fileName = Path.GetFileName(files.FileName);
                var physicalPath = Path.Combine(Server.MapPath("~/UploadDoc"), fileName);

                System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/UploadDoc"));

                foreach (FileInfo existFile in di.GetFiles())
                {
                    existFile.Delete();
                }
                //if (System.IO.File.Exists(physicalPath))
                //{
                //    System.IO.File.Delete(physicalPath);
                //}
                files.SaveAs(physicalPath);
                return Json(new { fileName = fileName, physicalPath = physicalPath, Status = "Ok" });
            }
            catch (Exception e)
            {
                return Json(new { Status = e.Message });
            }
        }

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.Reports.Models.DAO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Reports.Controllers
{
    [LogInChecker]
    public class AMWiseSalesAchievementAnimalController : Controller
    {
        AMWiseSalesAchievementAnimalDAO amWiseSalesAchievementAnimalDao = new AMWiseSalesAchievementAnimalDAO();
        public ActionResult frmAMWiseSalesValueAchievementAnimal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetAMWiseSalesValueAchievementAnimal(string fromDate, string toDate)
        {
            var data = amWiseSalesAchievementAnimalDao.GetAMWiseSalesValueAchievementAnimal(fromDate, toDate);
            if (data.Count > 0)
            {
                return Json(new { Data = data, Status = "Ok" });
            }

            else
            {
                return Json(new { Data = data, Status = "Not Ok" });
            }

        }
    }
}
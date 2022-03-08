using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.Dashboard.Models.BEL;
using PAsia_Dashboard.Areas.Dashboard.Models.DAO;
using PAsia_Dashboard.Areas.Reports.Models.BEl;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Reports.Models.DAO
{
    public class DepotPPMCurrentStockDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        HomeDashboardDAO homeDashboardDao = new HomeDashboardDAO();

        public List<DepotPPMCurrentStock> GetDepotPPMCurrentStock(string dateParam)
        {
            string CODE = HttpContext.Current.Session["CODE"].ToString();
            string ACCESS_LEVEL = HttpContext.Current.Session["ACCESS_LEVEL"].ToString();
            string accessLevelParam = "";
            if (ACCESS_LEVEL == "N" || ACCESS_LEVEL == null)
            {
                accessLevelParam = "";


                ///homeDashboard.ACCESS_LEVEL = "National";
            }
            
            else if (ACCESS_LEVEL == "D")
            {
                accessLevelParam = "AND DEPOT_CODE = '" + CODE + "'";
            }
            string DCSQry =
                "  SELECT ROW_NUMBER () OVER (ORDER BY S.PPM_CODE) SL_No," +
                "         S.DEPOT_CODE," +
                "         S.DEPOT_NAME," +
                "         S.PPM_CODE," +
                "         S.PPM_NAME,S.PPM_TYPE,S.PPM_TYPE_NAME," +
                "         S.PACK_SIZE," +
                "         ROUND (NVL (S.STOCK_QTY, 0), 0) STOCK_QTY" +
                "    FROM DATE_WISE_PPM_STOCK S," +
                "         (  SELECT DEPOT_CODE, PPM_CODE, MAX (STOCK_DATE) STOCK_DATE" +
                "              FROM DATE_WISE_PPM_STOCK" +
                "             WHERE TO_DATE (STOCK_DATE, 'DD/MM/RRRR') <=" +
                "                      TO_DATE ('" + dateParam + "', 'DD/MM/RRRR') " + accessLevelParam + " " +
                "          GROUP BY DEPOT_CODE, PPM_CODE) D" +
                "   WHERE     S.STOCK_DATE = D.STOCK_DATE" +
                "         AND S.DEPOT_CODE = D.DEPOT_CODE" +
                "         AND S.PPM_CODE = D.PPM_CODE AND ROUND (NVL (S.STOCK_QTY, 0), 0)>0 " +
                " ORDER BY S.PPM_CODE,S.PPM_TYPE";

            DataTable DCSdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), DCSQry);
            var depotPPMCurrentStock = (from DataRow row in DCSdt.Rows
                select new DepotPPMCurrentStock()
                {
                    SL_No = row["SL_No"].ToString(),
                    PPM_CODE = row["PPM_CODE"].ToString(),
                    PPM_NAME = row["PPM_NAME"].ToString(),
                    PPM_TYPE = row["PPM_TYPE"].ToString(),
                    PPM_TYPE_NAME = row["PPM_TYPE_NAME"].ToString(),
                    DEPOT_CODE = row["DEPOT_CODE"].ToString(),
                    DEPOT_NAME = row["DEPOT_NAME"].ToString(),
                    PACK_SIZE = row["PACK_SIZE"].ToString(),
                    STOCK_QTY = Convert.ToDouble(row["STOCK_QTY"].ToString())

                }).ToList();
            return depotPPMCurrentStock;
        }
        
        public HomeDashboard GetDashboardData()
        {
            string CODE = HttpContext.Current.Session["CODE"].ToString();
            string ACCESS_LEVEL = HttpContext.Current.Session["ACCESS_LEVEL"].ToString();
            string accessLevelParam = "";
            if (ACCESS_LEVEL == "N" || ACCESS_LEVEL == null)
            {
                accessLevelParam = "";


                ///homeDashboard.ACCESS_LEVEL = "National";
            }

            else if (ACCESS_LEVEL == "D")
            {
                accessLevelParam = "AND DEPOT_CODE = '" + CODE + "'";
            }
            HomeDashboard homeDashboard = new HomeDashboard
            {
                Commercial_Stock_Valuation = homeDashboardDao.GetCommercialStockValuation(accessLevelParam),
                Sample_Stock_Valuation = homeDashboardDao.GetSampleStockValuation(accessLevelParam),
               // PPM_Stock_Valuation = homeDashboardDao.GetPPMStockValuation(accessLevelParam),
               // Gift_Stock_Valuation = homeDashboardDao.GetGiftStockValuation(accessLevelParam)
            };
            return homeDashboard;
        }

    }
}
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
    public class DepotCommCurrentStockDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        HomeDashboardDAO homeDashboardDao = new HomeDashboardDAO();

        public List<DepotCommCurrentStock> GetDepotCommCurrentStock(string dateParam)
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
                "SELECT row_number() OVER (ORDER BY S.DEPOT_NAME) SL_No,S.DEPOT_CODE,S.DEPOT_NAME,S.PRODUCT_CODE,S.PRODUCT_NAME,S.PACK_SIZE,S.UNIT_TP,S.UNIT_VAT, " +
                " ROUND(NVL(S.FRESH_STOCK_QTY, 0),0) FRESH_STOCK_QTY,ROUND(NVL(S.DAMAGE_STOCK_QTY, 0),0) DAMAGE_STOCK_QTY, " +
                " ROUND(NVL(S.FRESH_STOCK_TP_VAL, 0),0) FRESH_STOCK_TP_VAL,ROUND(NVL(S.FRESH_STOCK_VAT_VAL, 0),0) FRESH_STOCK_VAT_VAL, " +
                " ROUND(NVL(S.FRESH_STOCK_TP_VAT_VAL, 0),0) FRESH_STOCK_TP_VAT_VAL " +
                " FROM DATE_WISE_FRESH_DAMAGE_STOCK S,(SELECT DEPOT_CODE, PRODUCT_CODE, MAX(STOCK_DATE) STOCK_DATE " +
                " FROM DATE_WISE_FRESH_DAMAGE_STOCK WHERE TO_DATE(STOCK_DATE, 'DD/MM/RRRR') <= " +
                " TO_DATE('"+dateParam+"', 'DD/MM/RRRR') "+accessLevelParam+" GROUP BY DEPOT_CODE, PRODUCT_CODE) D " +
                " WHERE  S.STOCK_DATE = D.STOCK_DATE AND S.DEPOT_CODE = D.DEPOT_CODE AND S.PRODUCT_CODE = D.PRODUCT_CODE ORDER BY S.DEPOT_NAME,S.PRODUCT_CODE";

            DataTable DCSdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), DCSQry);
            var depotCommCurrentStock = (from DataRow row in DCSdt.Rows
                select new DepotCommCurrentStock()
                {
                    SL_No = row["SL_No"].ToString(),
                    //PRODUCT_CODE = Convert.ToInt32(row["PRODUCT_CODE"].ToString()),
                    PRODUCT_CODE = row["PRODUCT_CODE"].ToString(),
                    PRODUCT_NAME = row["PRODUCT_NAME"].ToString(),
                    DEPOT_CODE = row["DEPOT_CODE"].ToString(),
                    DEPOT_NAME = row["DEPOT_NAME"].ToString(),
                    PACK_SIZE = row["PACK_SIZE"].ToString(),
                    UNIT_TP = row["UNIT_TP"].ToString(),
                    UNIT_VAT = row["UNIT_VAT"].ToString(),
                    FRESH_STOCK_QTY = Convert.ToDouble(row["FRESH_STOCK_QTY"].ToString()),
                    DAMAGE_STOCK_QTY = Convert.ToDouble(row["DAMAGE_STOCK_QTY"].ToString()),
                    FRESH_STOCK_TP_VAL = Convert.ToDouble(row["FRESH_STOCK_TP_VAL"].ToString()),
                    FRESH_STOCK_TP_VAT_VAL = Convert.ToDouble(row["FRESH_STOCK_TP_VAT_VAL"].ToString()),
                    FRESH_STOCK_VAT_VAL = Convert.ToDouble(row["FRESH_STOCK_VAT_VAL"].ToString())

                }).ToList();
            return depotCommCurrentStock;
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
                //Gift_Stock_Valuation = homeDashboardDao.GetGiftStockValuation(accessLevelParam)
            };
            return homeDashboard;
        }

    }
}
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
    public class ProdWiseSalesAchievementAnimalDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        HomeDashboardDAO homeDashboardDao = new HomeDashboardDAO();

        public List<ProdWiseSalesAchievement> GetProdWiseSalesAchievementAnimal(string fromDate, string toDate)
        {
            //string CODE = HttpContext.Current.Session["CODE"].ToString();
            //string ACCESS_LEVEL = HttpContext.Current.Session["ACCESS_LEVEL"].ToString();
            //string accessLevelParam = "";
            //if (ACCESS_LEVEL == "N" || ACCESS_LEVEL == null)
            //{
            //    accessLevelParam = "";


            //    ///homeDashboard.ACCESS_LEVEL = "National";
            //}

            //else if (ACCESS_LEVEL == "D")
            //{
            //    accessLevelParam = "AND DEPOT_CODE = '" + CODE + "'";
            //}
            string PQry =
            " SELECT row_number() OVER (ORDER BY PRODUCT_CODE) SL_NO,PRODUCT_CODE," +
            "       FN_PRODUCT_NAME(PRODUCT_CODE) PRODUCT_NAME," +
            "       FN_PACK_SIZE(PRODUCT_CODE) PACK_SIZE,    " +
            "       TO_DAY_SALES_BOX,       " +
            "       TO_DAY_SALES_VALUE," +
            "       CM_UPTO_SALES_BOX," +
            "       CM_UPTO_SALES_VALUE,           " +
            "       LM_UPTO_SALES_BOX," +
            "       LM_UPTO_SALES_VALUE," +
            "       CM_UPTO_SALES_BOX - LM_UPTO_SALES_BOX GROWTH_BOX," +
            "       CM_UPTO_SALES_VALUE - LM_UPTO_SALES_VALUE GROWTH_VALUE" +
            " FROM" +
            "   (" +
            "    SELECT PRODUCT_CODE,    " +
            "           SUM(NVL(TO_DAY_SALES_BOX,0)) TO_DAY_SALES_BOX,       " +
            "           SUM(NVL(TO_DAY_SALES_VALUE,0)) TO_DAY_SALES_VALUE," +
            "           SUM(NVL(CM_UPTO_SALES_BOX,0)) CM_UPTO_SALES_BOX," +
            "           SUM(NVL(CM_UPTO_SALES_VALUE,0)) CM_UPTO_SALES_VALUE,           " +
            "           SUM(NVL(LM_UPTO_SALES_BOX,0)) LM_UPTO_SALES_BOX," +
            "           SUM(NVL(LM_UPTO_SALES_VALUE,0)) LM_UPTO_SALES_VALUE " +
            "    FROM" +
            "       (" +
            "        SELECT PRODUCT_CODE,    " +
            "               SUM(NVL(NET_BOX,0)) TO_DAY_SALES_BOX,       " +
            "               SUM(NVL(NET_VALUE,0)) TO_DAY_SALES_VALUE," +
            "               0 CM_UPTO_SALES_BOX," +
            "               0 CM_UPTO_SALES_VALUE,           " +
            "               0 LM_UPTO_SALES_BOX," +
            "               0 LM_UPTO_SALES_VALUE       " +
            "        FROM  DATE_WISE_FIRST_PROD_SALES A" +
            "        WHERE TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') = TO_DATE('"+toDate + "','DD/MM/RRRR')" +
            "        GROUP BY PRODUCT_CODE" +
            "        UNION ALL" +
            "        SELECT PRODUCT_CODE,    " +
            "               0 TO_DAY_SALES_BOX,       " +
            "               0 TO_DAY_SALES_VALUE," +
            "               SUM(NVL(NET_BOX,0)) CM_UPTO_SALES_BOX," +
            "               SUM(NVL(NET_VALUE,0)) CM_UPTO_SALES_VALUE,           " +
            "               0 LM_UPTO_SALES_BOX," +
            "               0 LM_UPTO_SALES_VALUE       " +
            "        FROM  DATE_WISE_FIRST_PROD_SALES A" +
            "        WHERE TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN  TO_DATE('"+fromDate + "','DD/MM/RRRR') AND TO_DATE('"+toDate + "','DD/MM/RRRR')" +
            "        GROUP BY PRODUCT_CODE" +
            "        UNION ALL" +
            "        SELECT PRODUCT_CODE,    " +
            "               0 TO_DAY_SALES_BOX,       " +
            "               0 TO_DAY_SALES_VALUE," +
            "               0 CM_UPTO_SALES_BOX," +
            "               0 CM_UPTO_SALES_VALUE,           " +
            "               SUM(NVL(NET_BOX,0)) LM_UPTO_SALES_BOX," +
            "               SUM(NVL(NET_VALUE,0)) LM_UPTO_SALES_VALUE       " +
            "        FROM  DATE_WISE_FIRST_PROD_SALES A" +
            "        WHERE TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN  ADD_MONTHS(TO_DATE('"+fromDate + "','DD/MM/RRRR'),-1) AND ADD_MONTHS(TO_DATE('"+toDate + "','DD/MM/RRRR'),-1)" +
            "        GROUP BY PRODUCT_CODE" +
            "        )" +
            "    GROUP BY PRODUCT_CODE " +
            ")   ";

            DataTable DCSdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("DashboardAnimal"), PQry);
            var prodWiseSalesAchievement = (from DataRow row in DCSdt.Rows
                                            select new ProdWiseSalesAchievement()
                                            {
                                                SL_No = row["SL_No"].ToString(),
                                                //PRODUCT_CODE = Convert.ToInt32(row["PRODUCT_CODE"].ToString()),
                                                PRODUCT_CODE = row["PRODUCT_CODE"].ToString(),
                                                PRODUCT_NAME = row["PRODUCT_NAME"].ToString(),
                                                PACK_SIZE = row["PACK_SIZE"].ToString(),
                                                TO_DAY_SALES_BOX = row["TO_DAY_SALES_BOX"].ToString(),
                                                TO_DAY_SALES_VALUE = row["TO_DAY_SALES_VALUE"].ToString(),
                                                CM_UPTO_SALES_BOX = row["CM_UPTO_SALES_BOX"].ToString(),
                                                CM_UPTO_SALES_VALUE = row["CM_UPTO_SALES_VALUE"].ToString(),
                                                LM_UPTO_SALES_BOX = row["LM_UPTO_SALES_BOX"].ToString(),
                                                LM_UPTO_SALES_VALUE = row["LM_UPTO_SALES_VALUE"].ToString(),
                                                GROWTH_BOX =row["GROWTH_BOX"].ToString(),
                                                GROWTH_VALUE = row["GROWTH_VALUE"].ToString()

                                            }).ToList();
            return prodWiseSalesAchievement;
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
                PPM_Stock_Valuation = homeDashboardDao.GetPPMStockValuation(accessLevelParam),
                Gift_Stock_Valuation = homeDashboardDao.GetGiftStockValuation(accessLevelParam)
            };
            return homeDashboard;
        }

    }
}
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
    
    public class NationalPPMCurrentStockDAO
    {
        DBHelper dbHelper=new DBHelper();
        DBConnection dbConn=new DBConnection();
        HomeDashboardDAO homeDashboardDao = new HomeDashboardDAO();
        public List<NationalPPMCurrentStock> GetNationalPPMCurrentStock(string dateParam)
        {
           
            string NCSQry = "SELECT ROW_NUMBER () OVER (ORDER BY S.PPM_CODE) SL_No,S.PPM_CODE,S.PPM_NAME,S.PACK_SIZE,S.PPM_TYPE,S.PPM_TYPE_NAME,NVL(ROUND(SUM(S.STOCK_QTY), 0), 0) STOCK_QTY" +
                            " FROM DATE_WISE_PPM_STOCK S,(  SELECT DEPOT_CODE, PPM_CODE, MAX(STOCK_DATE) STOCK_DATE FROM DATE_WISE_PPM_STOCK WHERE TO_DATE(STOCK_DATE, 'DD/MM/RRRR') <=" +
                            " TO_DATE('"+ dateParam + "', 'DD/MM/RRRR') GROUP BY DEPOT_CODE, PPM_CODE) D WHERE  S.STOCK_DATE = D.STOCK_DATE AND S.DEPOT_CODE = D.DEPOT_CODE" +
                            " AND S.PPM_CODE = D.PPM_CODE AND NVL(ROUND (STOCK_QTY, 0), 0) > 0 GROUP BY S.PPM_CODE,S.PPM_NAME,S.PACK_SIZE,S.PPM_TYPE_NAME,S.PPM_TYPE" +
                            " ORDER BY S.PPM_CODE,S.PPM_TYPE";




            DataTable NCSdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), NCSQry);
            var NationalPPMCurrentStock = (from DataRow row in NCSdt.Rows
                select new NationalPPMCurrentStock()
                {
                    //PRODUCT_CODE = Convert.ToInt32(row["PRODUCT_CODE"].ToString()),
                    SL_No = row["SL_No"].ToString(),
                    PPM_CODE = row["PPM_CODE"].ToString(),
                    PPM_NAME = row["PPM_NAME"].ToString(),
                    PPM_TYPE = row["PPM_TYPE"].ToString(),
                    PPM_TYPE_NAME = row["PPM_TYPE_NAME"].ToString(),
                   // DEPOT_CODE = row["DEPOT_CODE"].ToString(),
                    //DEPOT_NAME = row["DEPOT_NAME"].ToString(),
                    PACK_SIZE = row["PACK_SIZE"].ToString(),
                    STOCK_QTY = Convert.ToDouble(row["STOCK_QTY"].ToString())
                }).ToList();
            return NationalPPMCurrentStock;
        }

        public HomeDashboard GetDashboardData()
        {
            string accessLevelParam = "";
            HomeDashboard homeDashboard=new HomeDashboard();
            homeDashboard.Commercial_Stock_Valuation = homeDashboardDao.GetCommercialStockValuation(accessLevelParam);
            homeDashboard.Sample_Stock_Valuation = homeDashboardDao.GetSampleStockValuation(accessLevelParam);
            //homeDashboard.PPM_Stock_Valuation = homeDashboardDao.GetPPMStockValuation(accessLevelParam);
            //homeDashboard.Gift_Stock_Valuation = homeDashboardDao.GetGiftStockValuation(accessLevelParam);
            return homeDashboard;
        }
    }
}
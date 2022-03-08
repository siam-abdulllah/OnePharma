using PAsia_Dashboard.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.Dashboard.Models.BEL;
using System.Data;
using System.Data.OracleClient;

namespace PAsia_Dashboard.Areas.Dashboard.Models.DAO
{
    public class HomeDashboardDAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConnection = new DBConnection();
        private DataRow row;
        private DataTable dt;

        public HomeDashboard GetDashboardData(string toDate)
        {
            try
            {


                HomeDashboard homeDashboard = new HomeDashboard();

                string MM = toDate.Split('/')[1];
                string YYYYMM = toDate.Split('/')[2]+toDate.Split('/')[1];
                string firstDayMonth = "01/"+toDate.Split('/')[1]+"/"+toDate.Split('/')[2];
                string CODE = HttpContext.Current.Session["CODE"].ToString();
                string ACCESS_LEVEL = HttpContext.Current.Session["ACCESS_LEVEL"].ToString();
                string accessLevelParam = "";
                string accessLevelParamEMployee = "";


                if (ACCESS_LEVEL == "N" || ACCESS_LEVEL == null)
                {
                    accessLevelParam = "";


                    homeDashboard.ACCESS_LEVEL = "National";
                }
                else if (ACCESS_LEVEL == "Z")
                {
                    accessLevelParam = "AND DSM_CODE = '" + CODE + "'";
                    accessLevelParamEMployee = "AND DSM_CODE = '" + CODE + "'";
                    homeDashboard.ACCESS_LEVEL = ACCESS_LEVEL;
                }
                else if (ACCESS_LEVEL == "D")
                {
                    accessLevelParam = "AND DEPOT_CODE = '" + CODE + "'";
                    //accessLevelParam = "WHERE DEPOT_CODE = '" + CODE + "'";
                    //accessLevelParamStock = "AND DEPOT_CODE = '" + CODE + "'";
                    //homeDashboard.ACCESS_LEVEL = "Depot";
                    //
                    string Depot_Name_Qry =
                        "SELECT UNIT_NAME FROM SC_COMP_UNIT WHERE UNIT_CODE NOT IN ('01','02') AND UNIT_CODE=" + CODE + " ";
                    //
                    row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Depot_Name_Qry).Rows[0];
                    homeDashboard.ACCESS_LEVEL = row["UNIT_NAME"].ToString();
                }
                else if (ACCESS_LEVEL == "R")
                {
                    accessLevelParam = "AND RSM_CODE = '" + CODE + "'";
                    //accessLevelParam = "WHERE RSM_CODE = '" + CODE + "'";
                    homeDashboard.ACCESS_LEVEL = ACCESS_LEVEL;
                }
                else if (ACCESS_LEVEL == "A")
                {
                    accessLevelParam = "AND AM_CODE = '" + CODE + "'";
                    //accessLevelParam = "WHERE AM_CODE = '" + CODE + "'";
                    homeDashboard.ACCESS_LEVEL = ACCESS_LEVEL;
                }
                else if (ACCESS_LEVEL == "T")
                {
                    accessLevelParam = "AND MPO_CODE = '" + CODE + "'";
                    //accessLevelParam = "WHERE MPO_CODE = '" + CODE + "'";
                    homeDashboard.ACCESS_LEVEL = ACCESS_LEVEL;
                }

                homeDashboard.Target = GetTargetCurrentMonth(accessLevelParam,YYYYMM);
                homeDashboard.TodaySale = GetTodaySales(accessLevelParam,toDate);
                homeDashboard.UpToMonthSale = GetUpToMonthSales(accessLevelParam,firstDayMonth, toDate);
                homeDashboard.LMUpToDate = GetLMUpToDateSales(accessLevelParam,toDate);
                homeDashboard.TotalMpo = GetTotalMpo(accessLevelParam,toDate);
                homeDashboard.Growth = "";
                homeDashboard.Today_Collection_Amount = GetToDayCollection(accessLevelParam,toDate);
                homeDashboard.UpTo_Month_Collection = GetUpToMonthCollection(accessLevelParam,  firstDayMonth,  toDate);
               // homeDashboard.TotalCustomer = GetTotalCustomer(accessLevelParam);
                homeDashboard.MaturedDue = GetMaturedDues(accessLevelParam,toDate);
                //homeDashboard.ActiveMaturedDue = GetActiveMaturedDues(accessLevelParam);
                //homeDashboard.DiscontinueMaturedDue = GetDiscontinueMaturedDues(accessLevelParam);
                homeDashboard.ImmaturedDue = GetImmaturedDue(accessLevelParam,toDate);
                homeDashboard.ProductValueSale = GetProductSalesValue(accessLevelParam,YYYYMM,toDate);
                //homeDashboard.DCCTotalSale = GetDCC_Sale(accessLevelParam);
                //homeDashboard.C0165Sale = GetProdSalesData(accessLevelParam);
                //homeDashboard.C0166Sale = GetProdSalesData(accessLevelParam, "C0166");
                //homeDashboard.MPOCreditLimit = GetMPOCreditLimit(accessLevelParam);
                homeDashboard.OldCollection = GetOldCollectionData(accessLevelParam,firstDayMonth,toDate);
                //homeDashboard.WorldCupOffer = GetWorldCupOffer(accessLevelParam);
                if (ACCESS_LEVEL == "N" || ACCESS_LEVEL == "D")
                {
                    //homeDashboard.Commercial_Stock_Valuation = GetCommercialStockValuation(accessLevelParam);
                    homeDashboard.Commercial_Stock_Valuation = GetCommercialStockValuationDash(accessLevelParam,toDate);
                    homeDashboard.Sample_Stock_Valuation = GetSampleStockValuation(accessLevelParam);
                    //homeDashboard.PPM_Stock_Valuation = GetPPMStockValuation(accessLevelParam);
                    //homeDashboard.Gift_Stock_Valuation = GetGiftStockValuation(accessLevelParam);
                }
                homeDashboard.Growth = "";
                ListReturn = GetBarChartData(accessLevelParam,firstDayMonth,toDate);
                return homeDashboard;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Target GetTargetCurrentMonth(string accessLevelParam,string YYYYMM)
        {
            try
            {
                //string target_Current_Month_Qry =
                //    "SELECT INITCAP(TO_CHAR(TO_DATE(MONTH_CODE, 'MM'), 'MONTH')) MONTH_NAME,ROUND(SUM(TARGET_AMT)/100000,2) TARGET_CURRENT_MONTH FROM TARGET_DTL_ALL WHERE TO_CHAR(TRUNC(SYSDATE),'yyyyMM')=YYYYMM  " +
                //    accessLevelParam + "  GROUP BY MONTH_CODE  ";
                string target_Current_Month_Qry =
                    "SELECT INITCAP(TO_CHAR(TO_DATE(MONTH_CODE, 'MM'), 'MONTH')) MONTH_NAME,ROUND(SUM(TARGET_AMT)/100000,2) TARGET_CURRENT_MONTH FROM TARGET_DTL_ALL WHERE YYYYMM='"+YYYYMM+"' " +
                    accessLevelParam + "  GROUP BY MONTH_CODE  ";
                Target target = new Target();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), target_Current_Month_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    target.Target_Current_Month = row["TARGET_CURRENT_MONTH"].ToString();
                    target.Target_Current_Month_Name = row["MONTH_NAME"].ToString();
                }
                //else
                //{
                //    target.Target_Current_Month = "";
                //    target.Target_Current_Month_Name = "";
                //}
                return target;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public TodaySale GetTodaySales(string accessLevelParam,string toDate)
        {

            try
            {
                string today_Sales_Qry =
                    "select ROUND(SUM(NET_INV_AMT_CA)/100000,2) TODAY_NET_INV_AMT_CA, ROUND(SUM(NET_INV_AMT_CR)/100000,2) TODAY_NET_INV_AMT_CR, ROUND((SUM(NET_INV_AMT_CA) + SUM(NET_INV_AMT_CR))/100000,2) TODAY_TOTAL_SALES" +
                    " from ( select MOP, SUM(NET_INV_AMT) NET_INV_AMT_CA, 0 NET_INV_AMT_CR  from DATE_WISE_SALES where TO_CHAR(INVOICE_DATE,'dd/MM/YYYY') = '"+toDate+"' AND MOP = 'CA'  " +
                    accessLevelParam + " " +
                    " GROUP BY  MOP  UNION ALL select MOP, 0 NET_INV_AMT_CA, SUM(NET_INV_AMT) NET_INV_AMT_CR  from DATE_WISE_SALES where  TO_CHAR(INVOICE_DATE,'dd/MM/YYYY')  = '" + toDate + "' AND MOP = 'CR' " +
                    accessLevelParam + " " +
                    " GROUP BY  MOP )";
                TodaySale todaySale = new TodaySale();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), today_Sales_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];

                    todaySale.Today_Sales_CA = row["TODAY_NET_INV_AMT_CA"].ToString();
                    todaySale.Today_Sales_CR = row["TODAY_NET_INV_AMT_CR"].ToString();
                    todaySale.Today_Sales = row["TODAY_TOTAL_SALES"].ToString();
                }

                return todaySale;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public UpToMonthSale GetUpToMonthSales(string accessLevelParam,string firstDayMonth,string toDate)
        {


            try
            {
                //string upToMonth_Sales_Amnt_Qry =
                //    "select ROUND(SUM(NET_INV_AMT_CA)/100000,2) UPTO_NET_INV_AMT_CA, ROUND(SUM(NET_INV_AMT_CR)/100000,2) UPTO_NET_INV_AMT_CR, ROUND((SUM(NET_INV_AMT_CA) + SUM(NET_INV_AMT_CR))/100000,2) UPTO_TOTAL_SALES" +
                //    " from ( select MOP, SUM(NET_INV_AMT) NET_INV_AMT_CA, 0 NET_INV_AMT_CR  from DATE_WISE_SALES where INVOICE_DATE between TRUNC (SYSDATE, 'MM') AND TRUNC (SYSDATE)  AND MOP = 'CA' " +
                //    accessLevelParam + " " +
                //    " GROUP BY  MOP  UNION ALL select MOP, 0 NET_INV_AMT_CA, SUM(NET_INV_AMT) NET_INV_AMT_CR  from DATE_WISE_SALES where INVOICE_DATE between TRUNC (SYSDATE, 'MM') AND TRUNC (SYSDATE)  AND MOP = 'CR' " +
                //    accessLevelParam + " " +
                //    " GROUP BY  MOP )";
                 string upToMonth_Sales_Amnt_Qry =
                    "select ROUND(SUM(NET_INV_AMT_CA)/100000,2) UPTO_NET_INV_AMT_CA, ROUND(SUM(NET_INV_AMT_CR)/100000,2) UPTO_NET_INV_AMT_CR, ROUND((SUM(NET_INV_AMT_CA) + SUM(NET_INV_AMT_CR))/100000,2) UPTO_TOTAL_SALES" +
                    " from ( select MOP, SUM(NET_INV_AMT) NET_INV_AMT_CA, 0 NET_INV_AMT_CR  from DATE_WISE_SALES where INVOICE_DATE between TO_DATE('"+ firstDayMonth + "','dd/MM/YYYY') AND TO_DATE('"+toDate+"','dd/MM/YYYY')  AND MOP = 'CA' " +
                    accessLevelParam + " " +
                    " GROUP BY  MOP  UNION ALL select MOP, 0 NET_INV_AMT_CA, SUM(NET_INV_AMT) NET_INV_AMT_CR  from DATE_WISE_SALES where INVOICE_DATE between TO_DATE('" + firstDayMonth + "','dd/MM/YYYY') AND TO_DATE('" + toDate + "','dd/MM/YYYY')  AND MOP = 'CR' " +
                    accessLevelParam + " " +
                    " GROUP BY  MOP )";

                UpToMonthSale upToMonthSale = new UpToMonthSale();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), upToMonth_Sales_Amnt_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), upToMonth_Sales_Amnt_Qry).Rows[0];
                    upToMonthSale.UpTo_Month_Total_Sales_CA = row["UPTO_NET_INV_AMT_CA"].ToString();
                    upToMonthSale.UpTo_Month_Total_Sales_CR = row["UPTO_NET_INV_AMT_CR"].ToString();
                    upToMonthSale.UpTo_Month_Total_Sales = row["UPTO_TOTAL_SALES"].ToString();
                }

                return upToMonthSale;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public LMUpToDate GetLMUpToDateSales(string accessLevelParam,string toDate)
        {


            try
            {
                //string upToMonth_Sales_Amnt_Qry =
                //    "select ROUND(SUM(NET_INV_AMT_CA)/100000,2) LMTO_NET_INV_AMT_CA, ROUND(SUM(NET_INV_AMT_CR)/100000,2) LMTO_NET_INV_AMT_CR, ROUND((SUM(NET_INV_AMT_CA) + SUM(NET_INV_AMT_CR))/100000,2) LMTO_TOTAL_SALES" +
                //    " from ( select MOP, SUM(NET_INV_AMT) NET_INV_AMT_CA, 0 NET_INV_AMT_CR  from DATE_WISE_SALES where INVOICE_DATE between TRUNC(ADD_MONTHS(SYSDATE, -1),'MM') AND TRUNC(add_months(sysdate, -1))  AND MOP = 'CA' " +
                //    accessLevelParam + " " +
                //    " GROUP BY  MOP  UNION ALL select MOP, 0 NET_INV_AMT_CA, SUM(NET_INV_AMT) NET_INV_AMT_CR  from DATE_WISE_SALES where INVOICE_DATE between TRUNC(ADD_MONTHS(SYSDATE, -1),'MM') AND TRUNC(add_months(sysdate, -1))  AND MOP = 'CR' " +
                //    accessLevelParam + " " +
                //    " GROUP BY  MOP )";
                string upToMonth_Sales_Amnt_Qry =
                    "select ROUND(SUM(NET_INV_AMT_CA)/100000,2) LMTO_NET_INV_AMT_CA, ROUND(SUM(NET_INV_AMT_CR)/100000,2) LMTO_NET_INV_AMT_CR, ROUND((SUM(NET_INV_AMT_CA) + SUM(NET_INV_AMT_CR))/100000,2) LMTO_TOTAL_SALES" +
                    " from ( select MOP, SUM(NET_INV_AMT) NET_INV_AMT_CA, 0 NET_INV_AMT_CR  from DATE_WISE_SALES where INVOICE_DATE between TRUNC(ADD_MONTHS(TO_DATE('" + toDate + "','dd/MM/YYYY'), -1),'MM') AND TRUNC(add_months(TO_DATE('" + toDate + "','dd/MM/YYYY'), -1))  AND MOP = 'CA' " +
                    accessLevelParam + " " +
                    " GROUP BY  MOP  UNION ALL select MOP, 0 NET_INV_AMT_CA, SUM(NET_INV_AMT) NET_INV_AMT_CR  from DATE_WISE_SALES where INVOICE_DATE between TRUNC(ADD_MONTHS(TO_DATE('" + toDate + "','dd/MM/YYYY'), -1),'MM') AND TRUNC(add_months(TO_DATE('" + toDate + "','dd/MM/YYYY'), -1))  AND MOP = 'CR' " +
                    accessLevelParam + " " +
                    " GROUP BY  MOP )";

                LMUpToDate lmUpToDate = new LMUpToDate();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), upToMonth_Sales_Amnt_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), upToMonth_Sales_Amnt_Qry).Rows[0];
                    lmUpToDate.LM_UP_ToDate_Total_Sales_CA = row["LMTO_NET_INV_AMT_CA"].ToString();
                    lmUpToDate.LM_UP_ToDate_Total_Sales_CR = row["LMTO_NET_INV_AMT_CR"].ToString();
                    lmUpToDate.LM_UP_ToDate_Total_Sales = row["LMTO_TOTAL_SALES"].ToString();
                }

                return lmUpToDate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public TotalMPO GetTotalMpo(string accessLevelParam,string toDate)
        {
            try
            {
                //string Total_Mpo_Qry = "SELECT SUM(DISTINCT LM_MPO) TOTAL_LM_MPO,SUM(DISTINCT CM_MPO) TOTAL_CM_MPO,SUM(TOTAL_ACTIVE_MPO) TOTAL_ACTIVE_MPO  FROM( " +
                //                       " SELECT 0 LM_MPO,COUNT(DISTINCT MPO_CODE) CM_MPO,0 TOTAL_ACTIVE_MPO FROM CM_TOTAL_MPO WHERE 1=1  " +
                //                       accessLevelParam + " " +
                //                       " UNION ALL " +
                //                       " SELECT COUNT(DISTINCT MPO_CODE) LM_MPO,0 CM_MPO,0 TOTAL_ACTIVE_MPO FROM LM_TOTAL_MPO WHERE 1=1  " +
                //                       accessLevelParam + " " +
                //                       " UNION ALL " +
                //                       " SELECT 0 LM_MPO,0 CM_MPO ,COUNT(EMPLOYEE_CODE) TOTAL_ACTIVE_MPO FROM EMPLOYEE_INFO WHERE STATUS='A' AND POSTING_LOCATION='T' )";
                string Total_Mpo_Qry = "SELECT SUM(DISTINCT LM_MPO) TOTAL_LM_MPO,SUM(DISTINCT CM_MPO) TOTAL_CM_MPO,SUM(TOTAL_ACTIVE_MPO) TOTAL_ACTIVE_MPO  FROM( " +
                                       " SELECT 0 LM_MPO,COUNT(DISTINCT MPO_CODE) CM_MPO,0 TOTAL_ACTIVE_MPO FROM TOTAL_MPO WHERE 1=1  AND YYYYMM=TO_CHAR(TO_DATE('" + toDate + "','dd/MM/YYYY'),'YYYYMM')" +
                                       accessLevelParam + " " +
                                       " UNION ALL " +
                                       " SELECT COUNT(DISTINCT MPO_CODE) LM_MPO,0 CM_MPO,0 TOTAL_ACTIVE_MPO FROM TOTAL_MPO WHERE 1=1    AND YYYYMM=TO_CHAR(TRUNC(add_months(TO_DATE('" + toDate + "','dd/MM/YYYY'), -1)),'YYYYMM') " +
                                       accessLevelParam + " " +
                                       " UNION ALL " +
                                       " SELECT 0 LM_MPO,0 CM_MPO ,COUNT(EMPLOYEE_CODE) TOTAL_ACTIVE_MPO FROM EMPLOYEE_INFO WHERE STATUS='A' AND POSTING_LOCATION='T'  AND ZONE_CODE IN ('A','B','C','D','E'))";


                TotalMPO totalMpo = new TotalMPO();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Total_Mpo_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Total_Mpo_Qry).Rows[0];
                    totalMpo.CM_Total_MPO = row["TOTAL_CM_MPO"].ToString();
                    totalMpo.LM_Total_MPO = row["TOTAL_LM_MPO"].ToString();
                    totalMpo.TOTAL_ACTIVE_MPO = row["TOTAL_ACTIVE_MPO"].ToString();
                }

                return totalMpo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetToDayCollection(string accessLevelParam,string toDate)
        {


            try
            {

                string TODAY_COLLECTION_AMT = "";
                string today_Collection_Amnt_Qry =
                    "select ROUND(SUM(COLLECTION_AMT)/100000,2) TODAY_COLLECTION_AMT from DATE_WISE_COLLECTION where TO_CHAR(COLLECT_DATE,'dd/MM/YYYY') = '" + toDate + "' " +
                    accessLevelParam + " ";
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), today_Collection_Amnt_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), today_Collection_Amnt_Qry).Rows[0];
                    TODAY_COLLECTION_AMT = row["TODAY_COLLECTION_AMT"].ToString();
                }

                return TODAY_COLLECTION_AMT;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetUpToMonthCollection(string accessLevelParam,string firstDayMonth,string toDate)
        {


            try
            {
                string UPTO_COLLECTION_AMT = "";
                string upToMonth_Collection_Amnt_Qry =
                    "select ROUND(SUM(COLLECTION_AMT)/100000,2) UPTO_COLLECTION_AMT from DATE_WISE_COLLECTION" +
                    //" where COLLECT_DATE between TO_DATE (TRUNC((SYSDATE),'MONTH'),'DD/MM/RRRR') AND TRUNC(SYSDATE) " +
                    " where COLLECT_DATE between TO_DATE('" + firstDayMonth + "','dd/MM/YYYY') AND TO_DATE('" + toDate + "','dd/MM/YYYY') " +
                    //" where COLLECT_DATE between TO_DATE('01/' || to_char(trunc(sysdate), 'mm/yyyy'), 'dd/mm/yyyy') AND TRUNC(SYSDATE) " +
                    accessLevelParam + " ";
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), upToMonth_Collection_Amnt_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), upToMonth_Collection_Amnt_Qry).Rows[0];
                    UPTO_COLLECTION_AMT = row["UPTO_COLLECTION_AMT"].ToString();
                }

                return UPTO_COLLECTION_AMT;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public TotalCustomer GetTotalCustomer(string accessLevelParam,string toDate)
        {


            try
            {
                string Total_Customer_Qry =
                    " SELECT SUM(LM_Customer) LM_Total_Customer,SUM(CM_Customer) Total_Customer FROM " +
                    " (SELECT 0 LM_Customer, COUNT(DISTINCT CUSTOMER_CODE) CM_Customer FROM Total_Customer WHERE 1 = 1 AND YYYYMM=TO_CHAR(TO_DATE('" + toDate + "','dd/MM/YYYY'),'YYYYMM')" +
                    accessLevelParam + " " +
                    " UNION ALL SELECT COUNT(DISTINCT CUSTOMER_CODE) LM_Customer, 0 CM_Customer FROM Total_Customer WHERE 1 = 1 AND YYYYMM=TO_CHAR(TRUNC(add_months(TO_DATE('" + toDate + "','dd/MM/YYYY'), -1)),'YYYYMM') " +
                    accessLevelParam + ") ";
                TotalCustomer totalCustomer = new TotalCustomer();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Total_Customer_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    // row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Total_Customer_Qry).Rows[0];
                    totalCustomer.CM_Total_Customer = row["CM_Total_Customer"].ToString();
                    totalCustomer.LM_Total_Customer = row["LM_Total_Customer"].ToString();
                }

                return totalCustomer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        
        public MaturedDue GetMaturedDues(string accessLevelParam,string toDate)
        {
            try
            {
                string Matured_Dues_Qry =
                    "select ROUND(sum(MATURE_DUES_AMT_CA)/100000,2) MATURE_DUES_AMT_CA, ROUND(sum(MATURE_DUES_AMT_CR)/100000,2) MATURE_DUES_AMT_CR, ROUND((sum(MATURE_DUES_AMT_CA) + sum(MATURE_DUES_AMT_CR))/100000,2) TOTAL_MATURE_DUES_AMT " +
                    " from ( select MODE_OF_PAYMENT, SUM(MATURE_DUES_AMT) MATURE_DUES_AMT_CA, 0 MATURE_DUES_AMT_CR  from MATURE_DUES where MODE_OF_PAYMENT = 'Cash' AND SALES_MATURITY_DATE<=TO_DATE('" + toDate + "','dd/MM/YYYY') " +
                    accessLevelParam + "" +
                    " GROUP BY  MODE_OF_PAYMENT UNION ALL select MODE_OF_PAYMENT, 0 MATURE_DUES_AMT_CA, SUM(MATURE_DUES_AMT) MATURE_DUES_AMT_CR  from MATURE_DUES where MODE_OF_PAYMENT = 'Credit'  AND SALES_MATURITY_DATE<=TO_DATE('" + toDate + "','dd/MM/YYYY') " +
                    accessLevelParam + "" +
                    " GROUP BY  MODE_OF_PAYMENT)  ";
                MaturedDue maturedDue = new MaturedDue();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Matured_Dues_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Matured_Dues_Qry).Rows[0];
                    maturedDue.Matured_Dues = row["TOTAL_MATURE_DUES_AMT"].ToString();
                    maturedDue.Matured_Dues_CA = row["MATURE_DUES_AMT_CA"].ToString();
                    maturedDue.Matured_Dues_CR = row["MATURE_DUES_AMT_CR"].ToString();
                }

                return maturedDue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActiveMaturedDue GetActiveMaturedDues(string accessLevelParam)
        {
            try
            {
                string Active_Matured_Dues_Qry =
                    "select ROUND(sum(MATURE_DUES_AMT_CA)/100000,2) Active_MATURE_DUES_AMT_CA, ROUND(sum(MATURE_DUES_AMT_CR)/100000,2) Active_MATURE_DUES_AMT_CR, ROUND((sum(MATURE_DUES_AMT_CA) + sum(MATURE_DUES_AMT_CR))/100000,2) TOTAL_ACTIVE_MATURE_DUES_AMT " +
                    " from ( select MODE_OF_PAYMENT, SUM(MATURE_DUES_AMT) MATURE_DUES_AMT_CA, 0 MATURE_DUES_AMT_CR  from ACTIVE_MATURE_DUES where MODE_OF_PAYMENT = 'Cash' " +
                    accessLevelParam + "" +
                    " GROUP BY  MODE_OF_PAYMENT UNION ALL select MODE_OF_PAYMENT, 0 MATURE_DUES_AMT_CA, SUM(MATURE_DUES_AMT) MATURE_DUES_AMT_CR  from ACTIVE_MATURE_DUES where MODE_OF_PAYMENT = 'Credit' " +
                    accessLevelParam + "" +
                    " GROUP BY  MODE_OF_PAYMENT)  ";
                //
                ActiveMaturedDue activeMaturedDue = new ActiveMaturedDue();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Active_Matured_Dues_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Active_Matured_Dues_Qry).Rows[0];
                    activeMaturedDue.Active_Matured_Dues = row["TOTAL_ACTIVE_MATURE_DUES_AMT"].ToString();
                    activeMaturedDue.Active_Matured_Dues_CA = row["Active_MATURE_DUES_AMT_CA"].ToString();
                    activeMaturedDue.Active_Matured_Dues_CR = row["Active_MATURE_DUES_AMT_CR"].ToString();
                }

                return activeMaturedDue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public DiscontinueMaturedDue GetDiscontinueMaturedDues(string accessLevelParam)
        {
            try
            {
                string Discontinue_Matured_Dues_Qry =
                    "select ROUND(sum(MATURE_DUES_AMT_CA)/100000,2) DIS_MATURE_DUES_AMT_CA, ROUND(sum(MATURE_DUES_AMT_CR)/100000,2) DIS_MATURE_DUES_AMT_CR, ROUND((sum(MATURE_DUES_AMT_CA) + sum(MATURE_DUES_AMT_CR))/100000,2) DIS_TOTAL_MATURE_DUES_AMT " +
                    " from ( select MODE_OF_PAYMENT, SUM(MATURE_DUES_AMT) MATURE_DUES_AMT_CA, 0 MATURE_DUES_AMT_CR  from DISCONTINUE_MATURE_DUES where MODE_OF_PAYMENT = 'Cash' " +
                    accessLevelParam + " " +
                    " GROUP BY  MODE_OF_PAYMENT UNION ALL select MODE_OF_PAYMENT, 0 MATURE_DUES_AMT_CA, SUM(MATURE_DUES_AMT) MATURE_DUES_AMT_CR  from DISCONTINUE_MATURE_DUES where MODE_OF_PAYMENT = 'Credit' " +
                    accessLevelParam + " " +
                    " GROUP BY  MODE_OF_PAYMENT)  ";
                //
                DiscontinueMaturedDue discontinueMaturedDue = new DiscontinueMaturedDue();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Discontinue_Matured_Dues_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Discontinue_Matured_Dues_Qry).Rows[0];
                    discontinueMaturedDue.Discontinue_Matured_Dues = row["DIS_TOTAL_MATURE_DUES_AMT"].ToString();
                    discontinueMaturedDue.Discontinue_Matured_Dues_CA = row["DIS_MATURE_DUES_AMT_CA"].ToString();
                    discontinueMaturedDue.Discontinue_Matured_Dues_CR = row["DIS_MATURE_DUES_AMT_CR"].ToString();
                }

                return discontinueMaturedDue;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public ImmaturedDue GetImmaturedDue(string accessLevelParam,string toDate)
        {
            try
            {
                string IMMatured_Dues_Qry =
                    "select ROUND(sum(MATURE_DUES_AMT_CA)/100000,2) IMMATURE_DUES_AMT_CA, ROUND(sum(MATURE_DUES_AMT_CR)/100000,2) IMMATURE_DUES_AMT_CR, ROUND((sum(MATURE_DUES_AMT_CA) + sum(MATURE_DUES_AMT_CR))/100000,2) TOTAL_IMMATURE_DUES_AMT " +
                    " from ( select MODE_OF_PAYMENT, SUM(MATURE_DUES_AMT) MATURE_DUES_AMT_CA, 0 MATURE_DUES_AMT_CR  from IMMATURE_DUES where MODE_OF_PAYMENT = 'Cash'  AND SALES_MATURITY_DATE>TO_DATE('" + toDate + "','dd/MM/YYYY') " +
                    accessLevelParam + " " +
                    " GROUP BY  MODE_OF_PAYMENT UNION ALL select MODE_OF_PAYMENT, 0 MATURE_DUES_AMT_CA, SUM(MATURE_DUES_AMT) MATURE_DUES_AMT_CR  from IMMATURE_DUES where MODE_OF_PAYMENT = 'Credit'  AND SALES_MATURITY_DATE>TO_DATE('" + toDate + "','dd/MM/YYYY') " +
                    accessLevelParam + " " +
                    " GROUP BY  MODE_OF_PAYMENT)  ";
                //
                ImmaturedDue immaturedDue = new ImmaturedDue();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), IMMatured_Dues_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), IMMatured_Dues_Qry).Rows[0];
                    immaturedDue.Immatured_Dues = row["TOTAL_IMMATURE_DUES_AMT"].ToString();
                    immaturedDue.Immatured_Dues_CA = row["IMMATURE_DUES_AMT_CA"].ToString();
                    immaturedDue.Immatured_Dues_CR = row["IMMATURE_DUES_AMT_CR"].ToString();
                }

                return immaturedDue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ProductValueSale GetProductSalesValue(string accessLevelParam,string YYYYMM,string toDate)
        {
            //string CM_Product_Value_Sales_Qry =
            //    "SELECT ROUND(SUM(NET_VALUE)/100000,2) CM_Product_Value_Sales FROM DATE_WISE_FIRST_PROD_SALES  WHERE 1=1 " +
            //    accessLevelParam + " ";
            try
            {
                string Product_value_sales_qry = "SELECT SUM(CM_Product_Value_Sales) CM_Total_Product_Value_Sales,SUM(LM_Product_Value_Sales) " +
                " LM_Total_Product_Value_Sales FROM" +
                " (SELECT ROUND(SUM(NET_VALUE) / 100000, 2) CM_Product_Value_Sales, 0 LM_Product_Value_Sales FROM DATE_WISE_FIRST_PROD_SALES" +
                " WHERE TO_CHAR(INVOICE_DATE, 'YYYYMM') = '"+YYYYMM+"' " + accessLevelParam + " " +
                "  UNION ALL" +
                " SELECT 0 CM_Product_Value_Sales, ROUND(SUM(NET_VALUE) / 100000, 2) LM_Product_Value_Sales FROM DATE_WISE_FIRST_PROD_SALES" +
                //" WHERE TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN TO_DATE (ADD_MONTHS (TRUNC ( SYSDATE,'MONTH'), -1),'DD/MM/RRRR') AND TO_DATE (ADD_MONTHS (SYSDATE,-1),'DD/MM/RRRR') " + accessLevelParam + " )";
                " WHERE TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN TRUNC(ADD_MONTHS(TO_DATE('" + toDate + "','dd/MM/YYYY'), -1),'MM') AND TRUNC(add_months(TO_DATE('" + toDate + "','dd/MM/YYYY'), -1))  " + accessLevelParam + " )";
                ProductValueSale productValueSale = new ProductValueSale();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Product_value_sales_qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), CM_Product_Value_Sales_Qry).Rows[0];
                    productValueSale.CM_Product_Value_Sales = row["CM_Total_Product_Value_Sales"].ToString();
                    productValueSale.LM_Product_Value_Sales = row["LM_Total_Product_Value_Sales"].ToString();
                }

                return productValueSale;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetCommercialStockValuation(string accessLevelParam)
        {
            try
            {
                string COMMERCIAL_STOCK_VAL = "";
                //string Commercial_Stock_Valuation_Qry =
                //    "SELECT ROUND(SUM(STOCK_VAl)/100000,2) COMMERCIAL_STOCK_VAL FROM (SELECT SUM(STOCK_VAl) STOCK_VAl  FROM PRODUCT_STOCK_QTY_VAL WHERE 1=1  " +
                //    accessLevelParam + "" +
                //    " UNION ALL SELECT SUM(STOCK_VAl) STOCK_VAl FROM PPM_STOCK_QTY_VAL WHERE PPM_TYPE = '001' " +
                //    accessLevelParam + " ) ";
                string Commercial_Stock_Valuation_Qry =
                    "SELECT ROUND(SUM(STOCK_VAl)/100000,2) COMMERCIAL_STOCK_VAL  FROM PRODUCT_STOCK_QTY_VAL WHERE PRODUCT_TYPE NOT IN ('S') " + accessLevelParam + "";
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Commercial_Stock_Valuation_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Commercial_Stock_Valuation_Qry).Rows[0];
                    COMMERCIAL_STOCK_VAL = row["COMMERCIAL_STOCK_VAL"].ToString();
                }

                //
                return COMMERCIAL_STOCK_VAL;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public string GetCommercialStockValuationDash(string accessLevelParam,string toDate)
        {
            try
            {
                string COMMERCIAL_STOCK_VAL = "";
                string Commercial_Stock_Valuation_Qry = " SELECT ROUND(SUM(STOCK_QTY)/100000,2) COMMERCIAL_STOCK_VAL FROM (SELECT  A.STOCK_DATE," +
                                                "        A.DEPOT_CODE," +
                                                "        A.S_PRODUCT_CODE PRODUCT_CODE," +
                                                "        C.TP_VAT," +
                                                "        A.CLOSING_PASSED_QTY STOCK_QTY," +
                                                "        NVL(C.TP_VAT,0)*NVL(A.CLOSING_PASSED_QTY,0) TP_VAT_VALUE" +
                                                " FROM  " +
                                                " DAILY_STOCK A," +
                                                " (  " +
                                                "                                         " +
                                                " SELECT  " +
                                                "      DEPOT_CODE," +
                                                "      S_PRODUCT_CODE PRODUCT_CODE," +
                                                "      TO_DATE(MAX(STOCK_DATE),'DD/MM/RRRR') STOCK_DATE" +
                                                " FROM " +
                                                "     DAILY_STOCK" +
                                                " WHERE STOCK_DATE<=TO_DATE('"+toDate+"','DD/MM/RRRR')  " + accessLevelParam + "   " +
                                                " GROUP BY DEPOT_CODE, S_PRODUCT_CODE " +
                                                "                                                    " +
                                                " ) B , PRODUCT_PRICE C      " +
                                                " WHERE   TO_DATE(A.STOCK_DATE,'DD/MM/RRRR')=TO_DATE(B.STOCK_DATE,'DD/MM/RRRR')" +
                                                " AND     A.DEPOT_CODE=B.DEPOT_CODE" +
                                                " AND     A.S_PRODUCT_CODE=B.PRODUCT_CODE" +
                                                " AND     A.S_PRODUCT_CODE=C.PRODUCT_CODE" +
                                                " AND     A.CLOSING_PASSED_QTY>0 )";

                //string Commercial_Stock_Valuation_Qry =
                //    "SELECT ROUND(SUM(STOCK_VAl)/100000,2) COMMERCIAL_STOCK_VAL  FROM PRODUCT_STOCK_QTY_VAL WHERE PRODUCT_TYPE NOT IN ('S') " + accessLevelParam + "";
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Commercial_Stock_Valuation_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Commercial_Stock_Valuation_Qry).Rows[0];
                    COMMERCIAL_STOCK_VAL = row["COMMERCIAL_STOCK_VAL"].ToString();
                }

                //
                return COMMERCIAL_STOCK_VAL;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetSampleStockValuation(string accessLevelParam)
        {
            try
            {
                string SAMPLE_STOCK_VAl = "";
                string Sample_Stock_Valuation_Qry =
                    "SELECT ROUND(SUM(STOCK_VAl)/100000,2) SAMPLE_STOCK_VAl FROM PRODUCT_STOCK_QTY_VAL WHERE PRODUCT_TYPE='S' " +
                    accessLevelParam + " ";
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Sample_Stock_Valuation_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Sample_Stock_Valuation_Qry).Rows[0];
                    SAMPLE_STOCK_VAl = row["SAMPLE_STOCK_VAl"].ToString();
                }

                //
                return SAMPLE_STOCK_VAl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetPPMStockValuation(string accessLevelParam)
        {
            try
            {
                string SAMPLE_STOCK_VAl = "";
                string PPM_Stock_Valuation_Qry =
                    "SELECT ROUND(SUM(STOCK_VAl)/100000,2) PPM_STOCK_VAl FROM PPM_STOCK_QTY_VAL WHERE PPM_TYPE = '003' " +
                    accessLevelParam + " ";
                //
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), PPM_Stock_Valuation_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), PPM_Stock_Valuation_Qry).Rows[0];
                    SAMPLE_STOCK_VAl = row["PPM_STOCK_VAl"].ToString();
                }

                //
                return SAMPLE_STOCK_VAl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetGiftStockValuation(string accessLevelParam)
        {
            try
            {
                string SAMPLE_STOCK_VAl = "";
                string Gift_Stock_Valuation_Qry =
                    "SELECT ROUND(SUM(STOCK_VAl)/100000,2) Gift_STOCK_VAl FROM PPM_STOCK_QTY_VAL WHERE PPM_TYPE = '004' " +
                    accessLevelParam + " ";
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Gift_Stock_Valuation_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Gift_Stock_Valuation_Qry).Rows[0];
                    SAMPLE_STOCK_VAl = row["Gift_STOCK_VAl"].ToString();
                }

                //
                return SAMPLE_STOCK_VAl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public DCCTotalSale GetDCC_Sale(string accessLevelParam)
        {
            try
            {
                string DCC_Sales_Qry =
                "SELECT ROUND(SUM(DCC_SALES_LM)/100000,2) DCC_TOTAL_SALES_LM,ROUND(SUM(DCC_SALES_CM)/100000,2) DCC_TOTAL_SALES_CM FROM ( SELECT 0 DCC_SALES_LM,SUM(NET_INV_AMT) DCC_SALES_CM FROM DATE_WISE_DCC_SALES " +
                " WHERE TO_CHAR (INVOICE_DATE, 'YYYYMM') = TO_CHAR(SYSDATE, 'YYYYMM') " + accessLevelParam + " " +
                " UNION ALL SELECT SUM(NET_INV_AMT) DCC_SALES_LM,0 DCC_SALES_CM FROM DATE_WISE_DCC_SALES" +
                " WHERE TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN TO_DATE (ADD_MONTHS (TRUNC ( SYSDATE,'MONTH'), -1),'DD/MM/RRRR') AND TO_DATE (ADD_MONTHS (SYSDATE,-1),'DD/MM/RRRR') " + accessLevelParam + " )";
                DCCTotalSale dccTotalSales = new DCCTotalSale();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), DCC_Sales_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    //row = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), DCC_Sales_Qry).Rows[0];
                    dccTotalSales.DCC_TOTAL_SALES_CM = row["DCC_TOTAL_SALES_CM"].ToString();
                    dccTotalSales.DCC_TOTAL_SALES_LM = row["DCC_TOTAL_SALES_LM"].ToString();
                }

                //
                return dccTotalSales;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public List<DashboardChart> GetBarChartData(string Param,string firstDayMonth, string toDate)
        {
            //string barChart_Qry = "SELECT RSM_CODE, ROUND(SUM (NET_INV_AMT)/100000,2) NET_INV_AMT FROM DATE_WISE_SALES WHERE INVOICE_DATE BETWEEN TRUNC(SYSDATE, 'MM') AND TRUNC(SYSDATE)" +
            //                        " GROUP BY RSM_CODE ORDER BY RSM_CODE";

            try
            {
                string barChart_Qry = "SELECT DISTINCT RSM_NAME,DWS.RSM_CODE, NET_INV_AMT,REGION_NAME,NULL COLOR FROM (SELECT RSM_CODE, ROUND(SUM (NET_INV_AMT)/ 100000,2) NET_INV_AMT FROM DATE_WISE_SALES " +
                                        " WHERE INVOICE_DATE BETWEEN TO_DATE('" + firstDayMonth + "','dd/MM/YYYY') AND TO_DATE('" + toDate + "','dd/MM/YYYY')  " + Param + " GROUP BY RSM_CODE ) DWS,VW_PAL_FIELD_FORCE_MIO VPFFM " +
                                        " WHERE DWS.RSM_CODE = VPFFM.RSM_CODE ORDER BY REGION_NAME";
                DataTable BarChartdt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), barChart_Qry);
                var barChartList = (from DataRow row in BarChartdt.Rows
                                    select new DashboardChart
                                    {
                                        Level = row["REGION_NAME"].ToString(),
                                        Data = row["NET_INV_AMT"].ToString(),
                                        Color = row["COLOR"].ToString(),
                                        BaloonText = row["RSM_NAME"].ToString()
                                    }).ToList();
                return barChartList;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ProdSale GetProdSalesData(string Param)
        {
            try
            {
                string Prod_Sales_Qry = "SELECT SUM(NET_QTY) PROD_TOTAL_QTY,ROUND(SUM(NET_VALUE)/100000, 2) PROD_TOTAL_VALUE,PRODUCT_CODE FROM DATE_WISE_PROD_SALES WHERE INVOICE_DATE BETWEEN " +
                                      "TO_DATE(TRUNC((SYSDATE), 'MONTH'), 'DD/MM/RRRR') AND TO_DATE(SYSDATE,'DD/MM/RRRR') AND PRODUCT_CODE in ('AZK005','AZK010','OFX200','OFX3','SKP250','SKP500','ONPC20','XELCOOD')  " + Param + " ";
                ProdSale prodSales = new ProdSale();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Prod_Sales_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    prodSales.PROD_TOTAL_QTY = row["PROD_TOTAL_QTY"].ToString();
                    prodSales.PROD_TOTAL_VALUE = row["PROD_TOTAL_VALUE"].ToString();
                }
                return prodSales;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public WorldCupOffer GetWorldCupOffer(string Param)
        {
            try
            {
                string World_Cup_offer_Qry = "select sum(nvl(qty,0)) NET_ISSUED_QTY,ROUND(sum(nvl(NET_INV_VALUE,0))/100000, 2) NET_INV_VALUE from (" +
                                             " select Distinct invoice_no, (count(ISSUED_QTY) / 4) qty, a.NET_INV_VALUE" +
                                             " from invoice_mst a, invoice_dtl b" +
                                             " where a.inv_mst_slno = b.inv_mst_slno" +
                                             " and   inv_type_code = 'INV001'" +
                                             " and   nvl(offer_type, 'N') = 'WorldCup'  " + Param + "  " +
                                             " group by invoice_no, a.NET_INV_VALUE)";
                WorldCupOffer worldCupOffer = new WorldCupOffer();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), World_Cup_offer_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    worldCupOffer.NET_ISSUED_QTY = row["NET_ISSUED_QTY"].ToString();
                    worldCupOffer.NET_INV_VALUE = row["NET_INV_VALUE"].ToString();
                }
                return worldCupOffer;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public MPOCreditLimit GetMPOCreditLimit(string Param)
        {
            try
            {
                string MPO_Credit_Limit_Qry = "SELECT ROUND(SUM(LIMIT_AMOUNT)/100000, 2)  TOTAL_LIMIT_AMOUNT FROM MPO_CREDIT_LIMIT WHERE LIMIT_YYYYMM = TO_CHAR(SYSDATE, 'YYYYMM')";
                MPOCreditLimit mPOCreditLimit = new MPOCreditLimit();
                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), MPO_Credit_Limit_Qry);
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    mPOCreditLimit.TOTAL_LIMIT_AMOUNT = row["TOTAL_LIMIT_AMOUNT"].ToString();

                }
                return mPOCreditLimit;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public OldCollection GetOldCollectionData(string accessLevelParam,string firstDayMonth,string toDate)
        {
            try
            {


                string Old_Collection_Data_Qry =
                    "SELECT SUM(OLD_TOTAL_NET_DUES_AMT) OLD_TOTAL_NET_DUES_AMT,SUM(OLD_UPTO_COLLECTION_AMT) OLD_UPTO_COLLECTION_AMT FROM(" +
                    " SELECT ROUND(SUM(NVL(NET_DUES_AMT,0))/ 100000,2) OLD_TOTAL_NET_DUES_AMT,0 OLD_UPTO_COLLECTION_AMT FROM  DUES_INVOICE_MST WHERE 1=1 " +
                    " AND INVOICE_DATE<=TO_DATE('" + toDate + "','dd/MM/YYYY')" +
                    accessLevelParam + "" +
                    " UNION ALL select 0 OLD_TOTAL_NET_DUES_AMT,ROUND(SUM(COLLECTION_AMT) / 100000, 2) OLD_UPTO_COLLECTION_AMT from " +
                    //" DATE_WISE_COLLECTION_OLD where COLLECT_DATE between TO_DATE(TRUNC((SYSDATE), 'MONTH'), 'DD/MM/RRRR') AND TRUNC(SYSDATE) " +
                    " DATE_WISE_COLLECTION_OLD where COLLECT_DATE between TO_DATE('" + firstDayMonth + "','dd/MM/YYYY') AND TO_DATE('" + toDate + "','dd/MM/YYYY') " +
                    accessLevelParam + " ) ";

                dt = dbHelper.GetDataTable(dbConnection.SAConnStrReader("Dashboard"), Old_Collection_Data_Qry);
                OldCollection oldCollection = new OldCollection();
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                    oldCollection.Total_Old_Dues = row["OLD_TOTAL_NET_DUES_AMT"].ToString();
                    oldCollection.Total_UpTo_Month_Collection = row["OLD_UPTO_COLLECTION_AMT"].ToString();

                }
                return oldCollection;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }


}
﻿using System;
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
    public class ZoneWiseSalesAchievementDAO
    {
        HomeDashboardDAO homeDashboardDao = new HomeDashboardDAO();
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        public object GetZoneWiseSalesProductAchievement(string fromDate, string toDate)
        {
            throw new NotImplementedException();
        }

        public List<ZoneWiseSalesAchievementValue> GetZoneWiseSalesValueAchievement(string fromDate, string toDate)
        {
            string CODE = HttpContext.Current.Session["CODE"].ToString();
            string ACCESS_LEVEL = HttpContext.Current.Session["ACCESS_LEVEL"].ToString();
            string accessLevelParam = " ";
            if (ACCESS_LEVEL == "N" || ACCESS_LEVEL == null)
            {
                accessLevelParam = " ";


                ///homeDashboard.ACCESS_LEVEL = " National" ;
            }

            else if (ACCESS_LEVEL == "Z")
            {
                accessLevelParam = " AND DSM_CODE = '" + CODE + "'";
            }
            //string ZDAValueQry = "SELECT row_number() OVER (ORDER BY ZONE_CODE) SL_NO, " +
            ////"       FN_DEPOT_NAME(DEPOT_CODE)DEPOT_NAME," +
            //"       (SELECT DISTINCT EMP_CODE DSM_CODE FROM VW_FIELD_FORCE WHERE LOC_CODE=A.ZONE_CODE AND POSTING_LOCATION='Z') DSM_CODE," +
            //"       FN_EMPLOYEE_NAME((SELECT DISTINCT EMP_CODE DSM_CODE FROM VW_FIELD_FORCE WHERE LOC_CODE=A.ZONE_CODE AND POSTING_LOCATION='Z')) DSM_NAME," +
            //"       A.ZONE_CODE,      " +
            //"       FN_ZONE_NAME(A.ZONE_CODE) ZONE_NAME," +
            //"       TARGET_AMT," +
            //"       TO_DAY_SALES," +
            //"       UPTO_SALES," +
            //"       ACH," +
            //"       LM_UPTO_SALES," +
            //"       GROWTH," +
            //"        CM_MPO," +
            //"        LM_MPO,       " +
            //"       CM_CUST," +
            //"       LM_CUST       " +
            //" FROM" +
            //"   ( " +
            //"    SELECT " +
            //"           ZONE_CODE," +
            //"           SUM(NVL(TARGET_AMT,0)) TARGET_AMT ," +
            //"           SUM(NVL(TO_DAY_SALES,0)) TO_DAY_SALES," +
            //"           SUM(NVL(UPTO_SALES,0)) UPTO_SALES," +
            //"           ROUND(DECODE(SUM(NVL(TARGET_AMT,0)),0,0,((SUM(NVL(UPTO_SALES,0)) * 100)/ SUM(NVL(TARGET_AMT,0)))),2) ACH," +
            //"           SUM(NVL(LM_UPTO_SALES,0)) LM_UPTO_SALES," +
            //"           SUM(NVL(UPTO_SALES,0)) - SUM(NVL(LM_UPTO_SALES,0)) GROWTH," +
            //"           SUM(NVL(CM_MPO,0)) CM_MPO," +
            //"           SUM(NVL(LM_MPO,0)) LM_MPO,                     " +
            //"           SUM(NVL(CM_CUST,0))CM_CUST," +
            //"           SUM(NVL(LM_CUST,0))LM_CUST       " +
            //"    FROM" +
            //"       (       " +
            //"    SELECT " +
            //"           ZONE_CODE," +
            //"           SUM(NVL(TARGET_AMT,0)) TARGET_AMT," +
            //"           0 TO_DAY_SALES," +
            //"           0 UPTO_SALES," +
            //"           0 LM_UPTO_SALES," +
            //"           0 CM_MPO ," +
            //"           0 LM_MPO,           " +
            //"           0 CM_CUST," +
            //"           0 LM_CUST         " +
            //"    FROM TARGET_DTL" +
            //"    WHERE YYYYMM BETWEEN TO_CHAR(TO_DATE('"+ fromDate+"','DD/MM/RRRR'),'YYYYMM') AND TO_CHAR(TO_DATE('"+ toDate+ "','DD/MM/RRRR'),'YYYYMM') " + accessLevelParam + " " +
            //"    GROUP BY ZONE_CODE" +
            //"    UNION ALL" +
            //"    SELECT " +
            //"           ZONE_CODE," +
            //"           0 TARGET_AMT," +
            //"           SUM(NVL(NET_INV_VALUE,0) - NVL(NET_RETURN_VALUE,0)) TO_DAY_SALES," +
            //"           0 UPTO_SALES," +
            //"           0 LM_UPTO_SALES," +
            //"           0 CM_MPO ," +
            //"           0 LM_MPO,           " +
            //"           0 CM_CUST," +
            //"           0 LM_CUST           " +
            //"    FROM  INVOICE_MST A" +
            //"    WHERE INV_TYPE_CODE IN ('INV001', 'INV002')" +
            //"    AND   TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') = TO_DATE('"+ toDate+ "','DD/MM/RRRR') " + accessLevelParam + " " +
            //"    GROUP BY ZONE_CODE" +
            //"    UNION ALL" +
            //"    SELECT " +
            //"           ZONE_CODE," +
            //"           0 TARGET_AMT," +
            //"           0 TO_DAY_SALES," +
            //"           SUM(NVL(NET_INV_VALUE,0) - NVL(NET_RETURN_VALUE,0)) UPTO_SALES ," +
            //"           0 LM_UPTO_SALES," +
            //"           0 CM_MPO ," +
            //"           0 LM_MPO,           " +
            //"           0 CM_CUST," +
            //"           0 LM_CUST                " +
            //"    FROM  INVOICE_MST A" +
            //"    WHERE INV_TYPE_CODE IN ('INV001', 'INV002')" +
            //"    AND   TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN  TO_DATE('"+ fromDate+"','DD/MM/RRRR') AND TO_DATE('"+ toDate+ "','DD/MM/RRRR') " + accessLevelParam + " " +
            //"    GROUP BY ZONE_CODE" +
            //"    UNION ALL" +
            //"    SELECT " +
            //"           ZONE_CODE," +
            //"           0 TARGET_AMT," +
            //"           0 TO_DAY_SALES," +
            //"           0 UPTO_SALES,          " +
            //"           SUM(NVL(NET_INV_VALUE,0) - NVL(NET_RETURN_VALUE,0)) LM_UPTO_SALES, " +
            //"           0 CM_MPO ," +
            //"           0 LM_MPO,            " +
            //"           0 CM_CUST," +
            //"           0 LM_CUST                 " +
            //"    FROM  INVOICE_MST A" +
            //"    WHERE INV_TYPE_CODE IN ('INV001', 'INV002')" +
            //"    AND   TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN  ADD_MONTHS(TO_DATE('"+ fromDate+"','DD/MM/RRRR'),-1) AND ADD_MONTHS(TO_DATE('"+ toDate+ "','DD/MM/RRRR'),-1) " + accessLevelParam + " " +
            //"    GROUP BY ZONE_CODE" +
            //"    UNION ALL" +
            //"    SELECT " +
            //"           ZONE_CODE," +
            //"           0 TARGET_AMT," +
            //"           0 TO_DAY_SALES," +
            //"           0 UPTO_SALES," +
            //"           0 LM_UPTO_SALES," +
            //"           0 CM_MPO ," +
            //"           0 LM_MPO,           " +
            //"           COUNT(DISTINCT CUSTOMER_CODE) CM_CUST," +
            //"           0 LM_CUST                 " +
            //"    FROM  INVOICE_MST A" +
            //"    WHERE INV_TYPE_CODE IN ('INV001', 'INV002')" +
            //"    AND   TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN  TO_DATE('"+ fromDate+"','DD/MM/RRRR') AND TO_DATE('"+ toDate+ "','DD/MM/RRRR') " + accessLevelParam + " " +
            //"    GROUP BY DEPOT_CODE,ZONE_CODE" +
            //"          " +
            //"    UNION ALL" +
            //"    SELECT " +
            //"           ZONE_CODE," +
            //"           0 TARGET_AMT," +
            //"           0 TO_DAY_SALES," +
            //"           0 UPTO_SALES," +
            //"           0 LM_UPTO_SALES," +
            //"           0 CM_MPO ," +
            //"           0 LM_MPO,           " +
            //"           0 CM_CUST," +
            //"           COUNT(DISTINCT CUSTOMER_CODE) LM_CUST                  " +
            //"    FROM  INVOICE_MST A" +
            //"    WHERE INV_TYPE_CODE IN ('INV001', 'INV002')" +
            //"    AND   TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN  ADD_MONTHS(TO_DATE('"+ fromDate+"','DD/MM/RRRR'),-1) AND ADD_MONTHS(TO_DATE('"+ toDate+ "','DD/MM/RRRR'),-1) " + accessLevelParam + " " +
            //"    GROUP BY DEPOT_CODE,ZONE_CODE" +
            //" UNION ALL" +
            //" SELECT " +
            //"       ZONE_CODE," +
            //"       0 TARGET_AMT," +
            //"       0 TO_DAY_SALES," +
            //"       0 UPTO_SALES," +
            //"       0 LM_UPTO_SALES," +
            //"       COUNT(DISTINCT MPO_CODE) CM_MPO ," +
            //"       0 LM_MPO," +
            //"       0 CM_CUST," +
            //"       0 LM_CUST                 " +
            //" FROM  INVOICE_MST A" +
            //" WHERE INV_TYPE_CODE IN ('INV001', 'INV002')" +
            //" AND   TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN  TO_DATE('"+ fromDate+"','DD/MM/RRRR') AND TO_DATE('"+ toDate+ "','DD/MM/RRRR') " + accessLevelParam + " " +
            //" AND MPO_CODE IN (SELECT EMPLOYEE_CODE" +
            //"                                     FROM EMPLOYEE_INFO" +
            //"                                   )" +
            //" GROUP BY ZONE_CODE    " +
            //" UNION ALL" +
            //" SELECT " +
            //"       ZONE_CODE," +
            //"       0 TARGET_AMT," +
            //"       0 TO_DAY_SALES," +
            //"       0 UPTO_SALES," +
            //"       0 LM_UPTO_SALES," +
            //"       0 CM_MPO," +
            //"       COUNT(DISTINCT MPO_CODE) LM_MPO," +
            //"       0 CM_CUST," +
            //"       0 LM_CUST                  " +
            //" FROM  INVOICE_MST A" +
            //" WHERE INV_TYPE_CODE IN ('INV001', 'INV002')" +
            //" AND   TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN  ADD_MONTHS(TO_DATE('"+ fromDate+"','DD/MM/RRRR'),-1) AND ADD_MONTHS(TO_DATE('"+ toDate+ "','DD/MM/RRRR'),-1) " + accessLevelParam + " " +
            //" AND MPO_CODE IN (SELECT EMPLOYEE_CODE" +
            //"                                     FROM EMPLOYEE_INFO" +
            //"                                    )" +
            //" GROUP BY ZONE_CODE" +
            //"  " +
            //"   " +
            //"    " +
            //"    " +
            //"  )" +
            //"  GROUP BY  ZONE_CODE" +
            //" ) A";
            string ZDAValueQry = "SELECT ROW_NUMBER () OVER (ORDER BY ZONE_CODE) SL_NO," +
"       (SELECT DISTINCT EMP_CODE DSM_CODE" +
"          FROM VW_FIELD_FORCE" +
"         WHERE LOC_CODE = A.ZONE_CODE AND POSTING_LOCATION = 'Z')" +
"          DSM_CODE," +
"       FN_EMPLOYEE_NAME (" +
"          (SELECT DISTINCT EMP_CODE DSM_CODE" +
"             FROM VW_FIELD_FORCE" +
"            WHERE LOC_CODE = A.ZONE_CODE AND POSTING_LOCATION = 'Z'))" +
"          DSM_NAME," +
"       A.ZONE_CODE," +
"       FN_ZONE_NAME (A.ZONE_CODE) ZONE_NAME," +
"       TARGET_AMT," +
"       TO_DAY_SALES," +
"       UPTO_SALES," +
"       ACH," +
"       LM_UPTO_SALES," +
"       GROWTH," +
"       CM_MPO," +
"       LM_MPO," +
"       CM_CUST," +
"       LM_CUST," +
"       TODATE_COLLECT_AMT," +
"       UPTO_COLLECT_AMT        " +
"  FROM (  SELECT ZONE_CODE," +
"                 SUM (NVL (TARGET_AMT, 0)) TARGET_AMT," +
"                 SUM (NVL (TO_DAY_SALES, 0)) TO_DAY_SALES," +
"                 SUM (NVL (UPTO_SALES, 0)) UPTO_SALES," +
"                 ROUND (" +
"                    DECODE (" +
"                       SUM (NVL (TARGET_AMT, 0))," +
"                       0, 0," +
"                       (  (SUM (NVL (UPTO_SALES, 0)) * 100)" +
"                        / SUM (NVL (TARGET_AMT, 0))))," +
"                    2)" +
"                    ACH," +
"                 SUM (NVL (LM_UPTO_SALES, 0)) LM_UPTO_SALES," +
"                 SUM (NVL (UPTO_SALES, 0)) - SUM (NVL (LM_UPTO_SALES, 0))" +
"                    GROWTH," +
"                 SUM (NVL (CM_MPO, 0)) CM_MPO," +
"                 SUM (NVL (LM_MPO, 0)) LM_MPO," +
"                 SUM (NVL (CM_CUST, 0)) CM_CUST," +
"                 SUM (NVL (LM_CUST, 0)) LM_CUST," +
"                 SUM(NVL(TODATE_COLLECT_AMT,0))TODATE_COLLECT_AMT," +
"                 SUM(NVL(UPTO_COLLECT_AMT,0))UPTO_COLLECT_AMT                  " +
"            FROM (  SELECT ZONE_CODE," +
"                           SUM (NVL (TARGET_AMT, 0)) TARGET_AMT," +
"                           0 TO_DAY_SALES," +
"                           0 UPTO_SALES," +
"                           0 LM_UPTO_SALES," +
"                           0 CM_MPO," +
"                           0 LM_MPO," +
"                           0 CM_CUST," +
"                           0 LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                           0 UPTO_COLLECT_AMT" +
"                      FROM TARGET_DTL_ALL" +
"                     WHERE YYYYMM BETWEEN TO_CHAR (TO_DATE ('" + fromDate + "', 'DD/MM/RRRR'),'YYYYMM') AND TO_CHAR (TO_DATE ('" + toDate + "', 'DD/MM/RRRR'),'YYYYMM')  " + accessLevelParam + "  " +
"                  GROUP BY ZONE_CODE" +
"                  UNION ALL" +
"                    SELECT ZONE_CODE," +
"                           0 TARGET_AMT," +
"                           SUM (" +
"                              NVL (NET_INV_VALUE, 0) - NVL (NET_RETURN_VALUE, 0))" +
"                              TO_DAY_SALES," +
"                           0 UPTO_SALES," +
"                           0 LM_UPTO_SALES," +
"                           0 CM_MPO," +
"                           0 LM_MPO," +
"                           0 CM_CUST," +
"                           0 LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                           0 UPTO_COLLECT_AMT" +
"                      FROM INVOICE_MST A" +
"                     WHERE     INV_TYPE_CODE IN ('INV001', 'INV002')" +
"                     AND TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') =TO_DATE ('" + toDate + "', 'DD/MM/RRRR')  " + accessLevelParam + "  " +
"                  GROUP BY ZONE_CODE" +
"                  UNION ALL" +
"                    SELECT ZONE_CODE," +
"                           0 TARGET_AMT," +
"                           0 TO_DAY_SALES," +
"                           SUM (" +
"                              NVL (NET_INV_VALUE, 0) - NVL (NET_RETURN_VALUE, 0))" +
"                              UPTO_SALES," +
"                           0 LM_UPTO_SALES," +
"                           0 CM_MPO," +
"                           0 LM_MPO," +
"                           0 CM_CUST," +
"                           0 LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                           0 UPTO_COLLECT_AMT" +
"                      FROM INVOICE_MST A" +
"                     WHERE     INV_TYPE_CODE IN ('INV001', 'INV002')" +
"                           AND TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN TO_DATE ('" + fromDate + "','DD/MM/RRRR') AND TO_DATE ('" + toDate + "','DD/MM/RRRR')  " + accessLevelParam + "  " +
"                  GROUP BY ZONE_CODE" +
"                  UNION ALL" +
"                    SELECT ZONE_CODE," +
"                           0 TARGET_AMT," +
"                           0 TO_DAY_SALES," +
"                           0 UPTO_SALES," +
"                           SUM (" +
"                              NVL (NET_INV_VALUE, 0) - NVL (NET_RETURN_VALUE, 0))" +
"                              LM_UPTO_SALES," +
"                           0 CM_MPO," +
"                           0 LM_MPO," +
"                           0 CM_CUST," +
"                           0 LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                           0 UPTO_COLLECT_AMT" +
"                      FROM INVOICE_MST A" +
"                     WHERE     INV_TYPE_CODE IN ('INV001', 'INV002')" +
"                           AND TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN ADD_MONTHS (TO_DATE ('" + fromDate + "','DD/MM/RRRR'),-1) AND ADD_MONTHS (TO_DATE ('" + toDate + "','DD/MM/RRRR'),-1)  " + accessLevelParam + "  " +
"                  GROUP BY ZONE_CODE" +
"                  UNION ALL" +
"                    SELECT ZONE_CODE," +
"                           0 TARGET_AMT," +
"                           0 TO_DAY_SALES," +
"                           0 UPTO_SALES," +
"                           0 LM_UPTO_SALES," +
"                           0 CM_MPO," +
"                           0 LM_MPO," +
"                           COUNT (DISTINCT CUSTOMER_CODE) CM_CUST," +
"                           0 LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                           0 UPTO_COLLECT_AMT" +
"                      FROM INVOICE_MST A" +
"                     WHERE     INV_TYPE_CODE IN ('INV001', 'INV002')" +
"                           AND TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN TO_DATE ('" + fromDate + "', 'DD/MM/RRRR') AND TO_DATE ('" + toDate + "','DD/MM/RRRR')  " + accessLevelParam + "  " +
"                  GROUP BY DEPOT_CODE, ZONE_CODE" +
"                  UNION ALL" +
"                    SELECT ZONE_CODE," +
"                           0 TARGET_AMT," +
"                           0 TO_DAY_SALES," +
"                           0 UPTO_SALES," +
"                           0 LM_UPTO_SALES," +
"                           0 CM_MPO," +
"                           0 LM_MPO," +
"                           0 CM_CUST," +
"                           COUNT (DISTINCT CUSTOMER_CODE) LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                           0 UPTO_COLLECT_AMT" +
"                      FROM INVOICE_MST A" +
"                     WHERE     INV_TYPE_CODE IN ('INV001', 'INV002')" +
"                           AND TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN ADD_MONTHS (TO_DATE ('" + fromDate + "','DD/MM/RRRR'),-1) AND ADD_MONTHS (TO_DATE ('" + toDate + "','DD/MM/RRRR'),-1)  " + accessLevelParam + "  " +
"                  GROUP BY DEPOT_CODE, ZONE_CODE" +
"                  UNION ALL" +
"                    SELECT ZONE_CODE," +
"                           0 TARGET_AMT," +
"                           0 TO_DAY_SALES," +
"                           0 UPTO_SALES," +
"                           0 LM_UPTO_SALES," +
"                           COUNT (DISTINCT MPO_CODE) CM_MPO," +
"                           0 LM_MPO," +
"                           0 CM_CUST," +
"                           0 LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                           0 UPTO_COLLECT_AMT" +
"                      FROM INVOICE_MST A" +
"                     WHERE     INV_TYPE_CODE IN ('INV001', 'INV002')" +
"                           AND TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN TO_DATE ('" + fromDate + "','DD/MM/RRRR') AND TO_DATE ('" + toDate + "','DD/MM/RRRR')  " + accessLevelParam + "  " +
"                           AND MPO_CODE IN (SELECT EMPLOYEE_CODE" +
"                                              FROM EMPLOYEE_INFO)" +
"                  GROUP BY ZONE_CODE" +
"                  UNION ALL" +
"                    SELECT ZONE_CODE," +
"                           0 TARGET_AMT," +
"                           0 TO_DAY_SALES," +
"                           0 UPTO_SALES," +
"                           0 LM_UPTO_SALES," +
"                           0 CM_MPO," +
"                           COUNT (DISTINCT MPO_CODE) LM_MPO," +
"                           0 CM_CUST," +
"                           0 LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                           0 UPTO_COLLECT_AMT" +
"                      FROM INVOICE_MST A" +
"                     WHERE     INV_TYPE_CODE IN ('INV001', 'INV002')" +
"                           AND TO_DATE (INVOICE_DATE, 'DD/MM/RRRR') BETWEEN ADD_MONTHS (TO_DATE ('" + fromDate + "','DD/MM/RRRR'),-1) AND ADD_MONTHS (TO_DATE ('" + toDate + "','DD/MM/RRRR'),-1)  " + accessLevelParam + "  " +
"                           AND MPO_CODE IN (SELECT EMPLOYEE_CODE" +
"                                              FROM EMPLOYEE_INFO)" +
"                  GROUP BY ZONE_CODE" +
"                  " +
"                  UNION ALL" +
"                  " +
"                  " +
"                  SELECT     " +
"                            C.ZONE_CODE," +
"                            0 TARGET_AMT," +
"                            0 TO_DAY_SALES," +
"                            0 UPTO_SALES," +
"                            0 LM_UPTO_SALES," +
"                            0 CM_MPO," +
"                            0 LM_MPO," +
"                            0 CM_CUST," +
"                            0 LM_CUST," +
"                            SUM(NVL(B.COLLECT_AMT,0))TODATE_COLLECT_AMT," +
"                            0 UPTO_COLLECT_AMT" +
"                FROM        COLLECT_MST A," +
"                            COLLECT_DTL B," +
"                            INVOICE_MST C" +
"                WHERE       A.COLLECT_MST_SLNO=B.COLLECT_MST_SLNO" +
"                AND         B.INVOICE_NO=C.INVOICE_NO " +
"                AND         TO_DATE (A.COLLECT_DATE, 'DD/MM/RRRR') =TO_DATE ('" + toDate + "', 'DD/MM/RRRR')  " + accessLevelParam + "  " +
"                AND         C.MPO_CODE IN (SELECT EMPLOYEE_CODE" +
"                                              FROM EMPLOYEE_INFO )" +
"                GROUP BY C.ZONE_CODE                               " +
"                                              " +
"                UNION ALL " +
"                " +
"                SELECT      " +
"                            C.ZONE_CODE," +
"                            0 TARGET_AMT," +
"                            0 TO_DAY_SALES," +
"                            0 UPTO_SALES," +
"                            0 LM_UPTO_SALES," +
"                            0 CM_MPO," +
"                            0 LM_MPO," +
"                            0 CM_CUST," +
"                            0 LM_CUST," +
"                            0 TODATE_COLLECT_AMT," +
"                            SUM(NVL(B.COLLECT_AMT,0))UPTO_COLLECT_AMT" +
"                FROM        COLLECT_MST A," +
"                            COLLECT_DTL B," +
"                            INVOICE_MST C" +
"                WHERE       A.COLLECT_MST_SLNO=B.COLLECT_MST_SLNO" +
"                AND         B.INVOICE_NO=C.INVOICE_NO " +
"                AND         TO_DATE (A.COLLECT_DATE, 'DD/MM/RRRR') BETWEEN TO_DATE ('" + fromDate + "','DD/MM/RRRR') AND TO_DATE ('" + toDate + "','DD/MM/RRRR')    " + accessLevelParam + "  " +
"                AND         C.MPO_CODE IN (SELECT EMPLOYEE_CODE" +
"                                              FROM EMPLOYEE_INFO )                                                           " +
"                                             " +
"                                              " +
"                GROUP BY C.ZONE_CODE  " +
"                  " +
"                  " +
"                  )" +
"        GROUP BY ZONE_CODE) A";


            DataTable DCSdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), ZDAValueQry);
            var depotCurrentStock = (from DataRow row in DCSdt.Rows
                                     select new ZoneWiseSalesAchievementValue()
                                     {
                                         SL_No = row["SL_NO"].ToString(),
                                         DSM_CODE = row["DSM_CODE"].ToString(),
                                         DSM_NAME = row["DSM_NAME"].ToString(),
                                         ZONE_CODE = row["ZONE_CODE"].ToString(),
                                         ZONE_NAME = row["ZONE_NAME"].ToString(),
                                         TARGET_AMT = row["TARGET_AMT"].ToString(),
                                         TO_DAY_SALES = row["TO_DAY_SALES"].ToString(),
                                         UPTO_SALES = row["UPTO_SALES"].ToString(),
                                         ACH = row["ACH"].ToString(),
                                         LM_UPTO_SALES = row["LM_UPTO_SALES"].ToString(),
                                         GROWTH = row["GROWTH"].ToString(),
                                         CM_MPO = row["CM_MPO"].ToString(),
                                         LM_MPO = row["LM_MPO"].ToString(),
                                         CM_CUST = row["CM_CUST"].ToString(),
                                         LM_CUST = row["LM_CUST"].ToString(),
                                         TO_DAY_COLLECTION = row["TODATE_COLLECT_AMT"].ToString(),
                                         UPTO_COLLECTION = row["UPTO_COLLECT_AMT"].ToString()


                                     }).ToList();
            return depotCurrentStock;
        }
        public HomeDashboard GetDashboardData()
        {
            string CODE = HttpContext.Current.Session["CODE"].ToString();
            string ACCESS_LEVEL = HttpContext.Current.Session["ACCESS_LEVEL"].ToString();
            string accessLevelParam = " ";
            if (ACCESS_LEVEL == " N" || ACCESS_LEVEL == null)
            {
                accessLevelParam = " ";


                ///homeDashboard.ACCESS_LEVEL = " National" ;
            }

            else if (ACCESS_LEVEL == " D")
            {
                accessLevelParam = " AND DEPOT_CODE = '" + CODE + " '";
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
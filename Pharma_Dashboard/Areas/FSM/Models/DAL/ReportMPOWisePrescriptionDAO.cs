using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.FSM.Models.BEL.BEO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.FSM.Models.DAL.DAO
{
    public class ReportMPOWisePrescriptionDAO
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();

        public List<ReportMPOWisePrescriptionInfoBEO> GetMPOWisePrescriptionData(string depotCode,string zoneCode, string regionCode, string areaCode, string territoryCode, string fromDate, string toDate)
        {
            try
            {
            string queryParam = " ";
            if (!string.IsNullOrEmpty(depotCode))
            {
                queryParam += "WHERE DEPOT_CODE='" + depotCode + "' ";
            } if (!string.IsNullOrEmpty(zoneCode))
            {
                queryParam += "AND ZONE_CODE='" + zoneCode + "' ";
            }
            if (!string.IsNullOrEmpty(regionCode))
            {
                queryParam += " AND REGION_CODE='" + regionCode + "' ";
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                queryParam += " AND AREA_CODE='" + areaCode + "' ";
            }
            if (!string.IsNullOrEmpty(territoryCode))
            {
                queryParam += " AND TERRITORY_CODE='" + territoryCode + "' ";
            }
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                queryParam += " AND TRUNC(CAPTURE_TIME) BETWEEN TO_DATE('" + fromDate + "','DD-MM-YYYY') AND TO_DATE('" + toDate + "','DD-MM-YYYY') ";
            }
            

            string Qry = "SELECT DISTINCT VFP.DOCTOR_NAME,VFP.DOCTOR_CODE,TO_CHAR(VFP.CAPTURE_TIME,'dd-MM-yyyy HH:mm:ss') CAPTURE_TIME, VFP.PRESCRIPTION_URL, VFP.PRESCRIPTION_TYPE, VFP.USER_ID,EI.EMPLOYEE_NAME, " +
                         " (SELECT COUNT(PRODUCT_CODE) FROM  FSM_PRESCRIPTION_DTL FPD WHERE   FPD.MST_SL=VFP.MST_SL GROUP BY FPD.MST_SL) TOTAL_PROD" +
                         " FROM VW_FSM_PRESCRIPTION  VFP INNER JOIN EMPLOYEE_INFO EI ON VFP.USER_ID = EI.EMPLOYEE_CODE " + queryParam+"";
            //string Qry = "SELECT  DOCTOR_CODE, DOCTOR_NAME, PRACTICING_DAY, PRESCRIPTION_PER_DAY,HONORARIUM_AMOUNT, TERRITORY_CODE_4P, TO_CHAR(SET_DATE,'DD-MM-YYYY') SET_DATE FROM VW_FSM_DOC_HONORARIUM " + queryParam+"";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Sales"), Qry);
            int count = 0;
            List<ReportMPOWisePrescriptionInfoBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportMPOWisePrescriptionInfoBEO
                    {
                        SL_NO = ++count,
                        //MST_SL = row["MST_SL"].ToString(),
                        //SET_DATE = row["SET_DATE"].ToString(),
                        DOCTOR_CODE = row["DOCTOR_CODE"].ToString(),
                        DOCTOR_NAME = row["DOCTOR_NAME"].ToString(),
                        CAPTURE_TIME = row["CAPTURE_TIME"].ToString(),
                        //CAPTURE_TIME = row["CAPTURE_TIME"].ToString() == ""
                        //    ? ""
                        //    : ((DateTime)row["CAPTURE_TIME"]).ToString("dd-MM-yyyy HH:mm:ss"),
                        PRESCRIPTION_URL = row["PRESCRIPTION_URL"].ToString().Replace("~",""),
                        PRESCRIPTION_TYPE = row["PRESCRIPTION_TYPE"].ToString(),
                        USER_ID = row["USER_ID"].ToString(),
                        EMPLOYEE_NAME = row["EMPLOYEE_NAME"].ToString(),
                        TOTAL_PROD = row["TOTAL_PROD"].ToString(),
                        
                        
                    }).ToList();
            return item;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}
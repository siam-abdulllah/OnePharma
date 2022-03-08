using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.FSM.Models.BEL.BEO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.FSM.Models.DAL.DAO
{
    public class ReportMPOWiseTopPrescriptionDAO
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();

        public List<ReportMPOWiseTopPrescriptionBEO> GetMPOWiseTopPrescriptionData(string depotCode,string zoneCode, string regionCode, string areaCode, string territoryCode,string fromDate,string toDate)
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
                queryParam += " AND TRUNC(SET_DATE) BETWEEN TO_DATE('" + fromDate + "','DD-MM-YYYY') AND TO_DATE('" + toDate + "','DD-MM-YYYY') ";
                }
                string Qry = "SELECT DISTINCT USER_ID,MIO_NAME,FN_MIO_DESIG_NAME(USER_ID) DESIG_NAME,TERRITORY_NAME,(SELECT COUNT(USER_ID) FROM FSM_PRESCRIPTION_MST WHERE USER_ID = fpm.USER_ID GROUP BY USER_ID )" +
                         " PRESCRIPTION_QTY FROM FSM_PRESCRIPTION_MST fpm INNER JOIN VW_PAL_FIELD_FORCE_MIO ffm ON FPM.USER_ID = FFM.MIO_CODE " + queryParam+"";
            //string Qry = "SELECT  DOCTOR_CODE, DOCTOR_NAME, PRACTICING_DAY, PRESCRIPTION_PER_DAY,HONORARIUM_AMOUNT, TERRITORY_CODE_4P, TO_CHAR(SET_DATE,'DD-MM-YYYY') SET_DATE FROM VW_FSM_DOC_HONORARIUM " + queryParam+"";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Sales"), Qry);
            int count = 0;
            List<ReportMPOWiseTopPrescriptionBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportMPOWiseTopPrescriptionBEO
                    {
                        SL_NO = ++count,
                        //MST_SL = row["MST_SL"].ToString(),
                        USER_ID = row["USER_ID"].ToString(),
                        MIO_NAME = row["MIO_NAME"].ToString(),
                        DESIG_NAME = row["DESIG_NAME"].ToString(),
                        TERRITORY_NAME = row["TERRITORY_NAME"].ToString(),
                        PRESCRIPTION_QTY = row["PRESCRIPTION_QTY"].ToString(),
                        
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
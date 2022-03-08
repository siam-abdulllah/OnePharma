using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.FSM.Models.BEL.BEO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.FSM.Models.DAL.DAO
{
    public class ReportQualifiedMPODAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();

        /// public object GetQualifiedMPOData(string depotCode, string zoneCode, string regionCode, string areaCode, string territoryCode, string fromDate, string toDate)
        public object GetQualifiedMPOData( string fromDate, string toDate)
        {
            try
            {
                //depotCode = depotCode ?? "";
                //zoneCode = zoneCode ?? "";
                //regionCode = regionCode ?? "";
                //areaCode = areaCode ?? "";
                //territoryCode = territoryCode ?? "";
                OracleConnection objConn = new OracleConnection(dbConn.SAConnStrReader("Sales"));
                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = "FN_QUALIFIED_MIO_LIST";//"get_count_emp_by_dept";
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Parameters.Add("vSTART_DATE", OracleType.VarChar).Value = fromDate;
                objCmd.Parameters.Add("vEND_DATE", OracleType.VarChar).Value = toDate;
                //objCmd.Parameters.Add("P_DEPOT_CODE", OracleType.VarChar).Value = depotCode.Trim();
                //objCmd.Parameters.Add("P_ZONE_CODE", OracleType.VarChar).Value = zoneCode.Trim();
                //objCmd.Parameters.Add("P_REGION_CODE", OracleType.VarChar).Value = regionCode.Trim();
                //objCmd.Parameters.Add("P_AREA_CODE", OracleType.VarChar).Value = areaCode.Trim();
                //objCmd.Parameters.Add("P_TERRITORY_CODE", OracleType.VarChar).Value = territoryCode.Trim();

                objCmd.Parameters.Add("return_value", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;
                objConn.Open();
                objCmd.ExecuteNonQuery();
                OracleDataReader rdr = objCmd.ExecuteReader();
                DataTable dt = new DataTable();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);

                }
                int count = 0;
                List<ReportQualifiedMPOBEO> item;
                item = (from DataRow row in dt.Rows
                        select new ReportQualifiedMPOBEO
                        {
                            SL_NO = ++count,
                            USER_NAME = row["USER_NAME"].ToString(),
                            REGION_NAME = row["REGION_NAME"].ToString(),
                            DEPOT_NAME = row["DEPOT_NAME"].ToString(),
                            AREA_NAME = row["AREA_NAME"].ToString(),
                            TERRITORY_NAME = row["TERRITORY_NAME"].ToString(),
                            TOTAL_XELPRO = row["TOTAL_XELPRO"].ToString(),
                            TOTAL_CARDOTEL = row["TOTAL_CARDOTEL"].ToString(),
                            TOTAL_FUXTIL = row["TOTAL_FUXTIL"].ToString(),
                            TOTAL_OTHERS = row["TOTAL_OTHERS"].ToString(),
                            TOTAL_PRES = row["TOTAL_PRES"].ToString(),
                            AWARDED_AMOUNT = row["AWARDED_AMOUNT"].ToString()
                        }).ToList();
                return item;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ExceptionReturn = e.Message;
            }
        }

    }
}
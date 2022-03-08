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
    public class ReportRegionWiseDoctorIncentiveDAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();

        public object GetRegionWiseDoctorIncentiveData( string fromDate, string toDate)
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
                objCmd.CommandText = "FN_DOCTOR_INCENTIVE_POLICY";//"get_count_emp_by_dept";
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
                var item = (from DataRow row in dt.Rows
                    select new ReportRegionWiseDoctorIncentiveBEO
                    {
                        SL_NO = ++count,
                        MIO_NAME = row["MIO_NAME"].ToString(),
                        MIO_DESIGNATION_NAME = row["MIO_DESIGNATION_NAME"].ToString(),
                        DOCTOR_NAME = row["DOCTOR_NAME"].ToString(),
                        DEGREES = row["DEGREES"].ToString(),
                        ADDRESS = row["ADDRESS"].ToString(),
                        TERRITORY_NAME = row["TERRITORY_NAME"].ToString(),
                        AREA_NAME = row["AREA_NAME"].ToString(),
                        REGION_NAME = row["REGION_NAME"].ToString(),
                        TOTAL_PRESCRIPTION = row["TOTAL_PRESCRIPTION"].ToString(),
                        MIO_INCENTIVE = row["MIO_INCENTIVE"].ToString(),
                        TYPE = row["TYPE"].ToString()
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
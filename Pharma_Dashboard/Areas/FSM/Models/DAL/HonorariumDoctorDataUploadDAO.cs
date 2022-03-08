using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.FSM.Models.BEL.BEO;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.FSM.Models.DAL.DAO
{
    public class HonorariumDoctorDataUploadDAO:ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();



        public List<HonorariumDoctorDataUploadInfo> GetDoctorGridData(string depotCode, string zoneCode, string regionCode, string areaCode, string territoryCode)
        {
            try
            {
                string queryParam = " ";
                if (!string.IsNullOrEmpty(depotCode))
                {
                    queryParam += "AND DEPOT_CODE='" + depotCode + "' ";
                }
                if (!string.IsNullOrEmpty(zoneCode))
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
                string Qry = "SELECT DOCTOR_CODE,DOCTOR_NAME, PRACTICING_DAY, PRESCRIPTION_PER_DAY,HONORARIUM_AMOUNT, TERRITORY_CODE_4P " +
                             //", TO_CHAR(SET_DATE,'DD-MM-YYYY') SET_DATE " +
                             "FROM VW_FSM_DOC_HONORARIUM  WHERE MONTH_YEAR='"+ DateTime.Now.ToString("MM-yyyy") + "' " + queryParam + " ";
                //string Qry = "SELECT  DOCTOR_CODE, DOCTOR_NAME, PRACTICING_DAY, PRESCRIPTION_PER_DAY,HONORARIUM_AMOUNT, TERRITORY_CODE_4P, TO_CHAR(SET_DATE,'DD-MM-YYYY') SET_DATE FROM VW_FSM_DOC_HONORARIUM " + queryParam+"";
                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Sales"), Qry);
                List<HonorariumDoctorDataUploadInfo> item;
                int count = 0;
                item = (from DataRow row in dt.Rows
                        select new HonorariumDoctorDataUploadInfo
                        {
                            SL_NO = ++count,
                            DOCTOR_CODE = row["DOCTOR_CODE"].ToString(),
                            DOCTOR_NAME = row["DOCTOR_NAME"].ToString(),
                            PRACTICING_DAY = row["PRACTICING_DAY"].ToString(),
                            PRESCRIPTION_PER_DAY = row["PRESCRIPTION_PER_DAY"].ToString(),
                            HONORARIUM_AMOUNT = row["HONORARIUM_AMOUNT"].ToString(),
                            TERRITORY_CODE_4P = row["TERRITORY_CODE_4P"].ToString(),
                            //SET_DATE = row["SET_DATE"].ToString()
                        }).ToList();
                return item;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string SaveUpdate(List<HonorariumDoctorDataUploadInfo> model)
        {
            string status = "No";
            try
            {
                if (model != null)
                {
                    foreach (var detail in model)
                    {
                        if (detail.SET_DATE == null)
                        {
                            detail.SET_DATE = DateTime.Now.ToString("dd-MM-yyyy");

                        }

                        detail.DOCTOR_CODE = detail.DOCTOR_CODE ?? "";
                        detail.DOCTOR_NAME = detail.DOCTOR_NAME ?? "";
                        detail.PRACTICING_DAY = detail.PRACTICING_DAY ?? "";
                        detail.PRESCRIPTION_PER_DAY = detail.PRESCRIPTION_PER_DAY?? "";
                        // detail.DEGREES = detail.DEGREES ?? "";
                        detail.HONORARIUM_AMOUNT = detail.HONORARIUM_AMOUNT ?? "";
                        detail.TERRITORY_CODE_4P = detail.TERRITORY_CODE_4P ?? "";

                        var query = "";
                        if (DoctorIDIsExist(detail))
                        {
                            //MaxID = ""; IUMode = "U";
                            query = "Update FSM_DOC_HONORARIUM set PRACTICING_DAY = '" + detail.PRACTICING_DAY.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                    ",PRESCRIPTION_PER_DAY = '" + detail.PRESCRIPTION_PER_DAY.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                    ",HONORARIUM_AMOUNT = '" + detail.HONORARIUM_AMOUNT.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                    ",TERRITORY_CODE_4P = '" + detail.TERRITORY_CODE_4P.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'  " +
                                    "WHERE TRIM(REPLACE(REPLACE(DOCTOR_CODE,'\r',''),'\n','')) = '" + detail.DOCTOR_CODE.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "' " +
                                    "and TO_CHAR(SET_DATE,'MM-YYYY')='" + DateTime.Now.ToString("MM-yyyy") + "'  ";
                        }
                        else
                        {
                            //MaxID = ""; IUMode = "I";
                            query = "Insert into FSM_DOC_HONORARIUM (DOCTOR_CODE, PRACTICING_DAY, PRESCRIPTION_PER_DAY,HONORARIUM_AMOUNT, TERRITORY_CODE_4P, SET_DATE) " +
                                "Values('" + detail.DOCTOR_CODE.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                "," + detail.PRACTICING_DAY.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "" +
                                "," + detail.PRESCRIPTION_PER_DAY.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "" +
                                "," + detail.HONORARIUM_AMOUNT.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "" +
                                ",'" + detail.TERRITORY_CODE_4P.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                ",TO_DATE('" + detail.SET_DATE + "','DD/MM/RRRR'))";

                        }
                        if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Sales"), query))
                        {
                            status = "Yes";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return status = e.Message;
            }
            return status;
        }


        private bool DoctorIDIsExist(HonorariumDoctorDataUploadInfo detail)
        {
            bool isTrue = false;
            string Qry = "SELECT DOCTOR_CODE FROM FSM_DOC_HONORARIUM WHERE TRIM(REPLACE(REPLACE(DOCTOR_CODE,'\r',''),'\n','')) = '" + detail.DOCTOR_CODE + "' and TO_CHAR(SET_DATE,'MM-YYYY')='" + DateTime.Now.ToString("MM-yyyy") + "'  ";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader("Sales"), Qry);
            if (dt2.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }


       
        public object LoadExcelFileHonorariumDoctorData(string fileName, string physicalPath)
        {
            Object data = null;
            DataSet dataSet = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                string connectionString = "";
                string[] d = fileName.Split('.');
                string fileExtension = "." + d[d.Length - 1].ToString();
                if (d.Length > 0)
                {
                    if (fileExtension == ".xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + physicalPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + physicalPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    }
                    else if (fileExtension == ".csv")
                    {
                        connectionString = string.Format(
                            @"Provider=Microsoft.Jet.OleDb.4.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
                            Path.GetDirectoryName(physicalPath)
                        );
                    }
                }
                //Create Connection to Excel work book and add oledb namespace
                OleDbConnection excelConnection = new OleDbConnection(connectionString);
                excelConnection.Open();
                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }
                String[] excelSheets = new String[dt.Rows.Count];
                int t = 0;
                //excel data saves in temp file here.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[t] = row["TABLE_NAME"].ToString();
                    t++;
                }
                OleDbConnection excelConnection1 = new OleDbConnection(connectionString);
                string query = string.Format("Select * from [{0}]", excelSheets[0]);
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                {
                    dataAdapter.Fill(dataSet);
                }
                dt = dataSet.Tables[0];
                excelConnection.Close();
                List<HonorariumDoctorDataUploadInfo> item = new List<HonorariumDoctorDataUploadInfo>();
                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    HonorariumDoctorDataUploadInfo hData = new HonorariumDoctorDataUploadInfo();
                    
                    hData.DOCTOR_CODE = row["DoctorCode"].ToString();
                    hData.PRACTICING_DAY = row["PracticingDay"].ToString();
                    hData.PRESCRIPTION_PER_DAY = row["PrescriptionPerDay"].ToString();
                    hData.HONORARIUM_AMOUNT = row["HonorariumAmount"].ToString();
                    hData.TERRITORY_CODE_4P = row["TerritoryCode4P"].ToString();
                    //hData.SET_DATE = row["Set Date"].ToString() == ""
                    //    ? ""
                    //    : ((DateTime)row["Set Date"]).ToString("dd-MM-yyyy");
                    if (!string.IsNullOrEmpty(hData.DOCTOR_CODE) && !string.IsNullOrEmpty(hData.PRACTICING_DAY) && !string.IsNullOrEmpty(hData.PRESCRIPTION_PER_DAY) &&
                        !string.IsNullOrEmpty(hData.HONORARIUM_AMOUNT))
                    {
                        hData.SL_NO = ++count;
                        item.Add(hData);
                    }

                }
                return item;
            }
            catch (Exception ex)
            {
                ExceptionReturn = ex.Message;
                return ExceptionReturn;
            }

        }
    }
}
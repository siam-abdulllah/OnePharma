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
    public class DoctorDataUploadDAO : ReturnData
    {
        private readonly DBConnection dbConn = new DBConnection();
        private readonly DBHelper dbHelper = new DBHelper();
        


        public object GetDoctorGridData(string depotCode, string zoneCode, string regionCode, string areaCode, string territoryCode)
        {
            try
            {

           
            string queryParam = " ";
            if (!string.IsNullOrEmpty(depotCode))
            {
                queryParam += "WHERE DEPOT_CODE='" + depotCode + "' ";
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

            string Qry = "SELECT DOCTOR_CODE,DOCTOR_NAME,CLASS_GROUP, ADDRESS,DEGREES,DESIGNATION,CONTRACT_NO,EMAIL,TERRITORY_CODE_4P,SPECIALTY FROM VW_FSM_DOC_DETAIL " + queryParam + " ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Sales"), Qry);
            List<DoctorDataUploadInfo> item;
            int count = 0;
            item = (from DataRow row in dt.Rows
                    select new DoctorDataUploadInfo
                    {
                        SL_NO = ++count,
                        DOCTOR_CODE = row["DOCTOR_CODE"].ToString(),
                        DOCTOR_NAME = row["DOCTOR_NAME"].ToString(),
                        CLASS_GROUP = row["CLASS_GROUP"].ToString(),
                        ADDRESS = row["ADDRESS"].ToString(),
                        DEGREES = row["DEGREES"].ToString(),
                        DESIGNATION = row["DESIGNATION"].ToString(),
                        CONTRACT_NO = row["CONTRACT_NO"].ToString(),
                        EMAIL = row["EMAIL"].ToString(),
                        TERRITORY_CODE_4P = row["TERRITORY_CODE_4P"].ToString(),
                        SPECIALTY = row["SPECIALTY"].ToString()

                    }).ToList();
            return item;
            }
            catch (Exception e)
            {
                ExceptionReturn = e.Message;
                return ExceptionReturn;
            }
        }

        public string SaveUpdate(List<DoctorDataUploadInfo> model)
        {
            string status = "No";
            try
            {
                if (model != null)
                {
                    foreach (var detail in model)
                    {
                        detail.DOCTOR_CODE = detail.DOCTOR_CODE  ?? "";
                        detail.DOCTOR_NAME = detail.DOCTOR_NAME  ?? "";
                        detail.CLASS_GROUP = detail.CLASS_GROUP  ?? "";
                        detail.ADDRESS = detail.ADDRESS  ?? "";
                        // detail.DEGREES = detail.DEGREES ?? "";
                        detail.DESIGNATION = detail.DESIGNATION  ?? "";
                        detail.CONTRACT_NO = detail.CONTRACT_NO  ?? "";
                        detail.EMAIL = detail.EMAIL  ?? "";
                        detail.TERRITORY_CODE_4P = detail.TERRITORY_CODE_4P  ?? "";
                        detail.SPECIALTY = detail.SPECIALTY  ?? "";
                        //detail.DOCTOR_CODE = detail.DOCTOR_CODE ?? "";
                        var query = "";
                        if (DoctorIDIsExist(detail))
                        {
                            //MaxID = ""; IUMode = "U";
                            query = "Update FSM_DOC_DETAIL set DOCTOR_NAME = '" + detail.DOCTOR_NAME.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "CLASS_GROUP = '" + detail.CLASS_GROUP.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "ADDRESS = '" + detail.ADDRESS.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "DEGREES = '" + detail.DEGREES.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "DESIGNATION = '" + detail.DESIGNATION.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "CONTRACT_NO = '" + detail.CONTRACT_NO.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "EMAIL = '" + detail.EMAIL.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "TERRITORY_CODE_4P = '" + detail.TERRITORY_CODE_4P.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "SPECIALTY = '" + detail.SPECIALTY.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "' " +
                                    "Where DOCTOR_CODE = '" + detail.DOCTOR_CODE.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "' ";
                        }
                        else
                        {
                            //MaxID = ""; IUMode = "I";
                            query = "Insert into FSM_DOC_DETAIL (DOCTOR_CODE,DOCTOR_NAME,CLASS_GROUP, ADDRESS,DEGREES,DESIGNATION,CONTRACT_NO,EMAIL,TERRITORY_CODE_4P,SPECIALTY) " +
                                    "Values('" + detail.DOCTOR_CODE.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.DOCTOR_NAME.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.CLASS_GROUP.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.ADDRESS.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.DEGREES.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.DESIGNATION.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.CONTRACT_NO.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.EMAIL.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.TERRITORY_CODE_4P.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'," +
                                    "'" + detail.SPECIALTY.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "')";

                        }
                        if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Sales"), query))
                        {
                            status = "Yes";
                        }
                    }
                }
                return status;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return status = e.Message;
            }
            
        }


        private bool DoctorIDIsExist(DoctorDataUploadInfo detail)
        {
            bool isTrue = false;
            try
            {
                //string Qry = "SELECT DOCTOR_CODE FROM FSM_DOC_DETAIL WHERE DOCTOR_CODE = '" + detail.DOCTOR_CODE + "' and TERRITORY_CODE_4P='" + detail.TERRITORY_CODE_4P + "' ";
                string Qry = "SELECT DOCTOR_CODE FROM FSM_DOC_DETAIL WHERE DOCTOR_CODE = '" + detail.DOCTOR_CODE + "' ";
                DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader("Sales"), Qry);
                if (dt2.Rows.Count > 0)
                {
                    isTrue = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                isTrue = false;
            }
            

            return isTrue;
        }
        
        public object LoadExcelFileDoctorData(string fileName, string physicalPath)
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
                List<DoctorDataUploadInfo> item = new List<DoctorDataUploadInfo>();
                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    DoctorDataUploadInfo dData = new DoctorDataUploadInfo();
                    
                    dData.DOCTOR_CODE = row["DoctorCode"].ToString();
                    dData.DOCTOR_NAME = row["DoctorName"].ToString();
                    // DOCTOR_CODE_4P = row["Doctor Code 4P"].ToString(),
                    dData.CLASS_GROUP = row["Class"].ToString();
                    dData.ADDRESS = row["Address"].ToString();
                    dData.DEGREES = row["Degrees"].ToString();
                    dData.DESIGNATION = row["Designation"].ToString();
                    dData.CONTRACT_NO = row["ContractNo"].ToString();
                    dData.EMAIL = row["Email"].ToString();
                    dData.TERRITORY_CODE_4P = row["TerritoryCode4P"].ToString();
                    dData.SPECIALTY = row["Specialty"].ToString();
                    if (!string.IsNullOrEmpty(dData.DOCTOR_CODE) && !string.IsNullOrEmpty(dData.DOCTOR_NAME) && !string.IsNullOrEmpty(dData.TERRITORY_CODE_4P))
                    {
                        dData.SL_NO = ++count;
                        item.Add(dData);
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
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
    public class DataUploadPrescriptionTargetDAO : ReturnData
    {
        private readonly DBConnection dbConn = new DBConnection();
        private readonly DBHelper dbHelper = new DBHelper();
        public object GetPrescriptionTargetData(string MonthNumber, string Year)
        {
            try
            {

            
            //string Qry = "SELECT FPT.MPO_CODE,EI.EMPLOYEE_NAME MPO_NAME, FPT.TERRITORY_CODE, FPT.PRESCRIPTION_QTY,TO_CHAR(FPT.SET_DATE,'DD-MM-YYYY') SET_DATE, FPT.YEAR, FPT.MONTH_NUMBER " +
            //             " FROM FSM_PRESCRIPTION_TARGET  FPT INNER JOIN EMPLOYEE_INFO EI ON FPT.MPO_CODE = EI.EMPLOYEE_CODE";
            string Qry = "SELECT A.MPO_CODE,B.EMPLOYEE_NAME MPO_NAME,A.TERRITORY_CODE,A.PRESCRIPTION_QTY,TO_CHAR(A.SET_DATE, 'DD-MM-YYYY') SET_DATE,A.YEAR,A.MONTH_NUMBER,C.MONTH_NAME" +
                         " FROM FSM_PRESCRIPTION_TARGET A INNER JOIN EMPLOYEE_INFO B ON A.MPO_CODE = B.EMPLOYEE_CODE INNER JOIN MONTH_INFO C ON A.MONTH_NUMBER = C.MONTH_CODE " +
                         " WHERE   MONTH_NUMBER='"+MonthNumber+"' AND YEAR="+ Year+" ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Sales"), Qry);
            int count = 0;
            var item = (from DataRow row in dt.Rows
                select new DataUploadPrescriptionTargetBEO
                {
                    SL_NO = ++count,
                    MPO_CODE = row["MPO_CODE"].ToString(),
                    MPO_NAME = row["MPO_NAME"].ToString(),
                    TERRITORY_CODE = row["TERRITORY_CODE"].ToString(),
                    PRESCRIPTION_QTY = row["PRESCRIPTION_QTY"].ToString(),
                    SET_DATE = row["SET_DATE"].ToString(),
                    YEAR = row["YEAR"].ToString(),
                    MONTH_NUMBER = row["MONTH_NUMBER"].ToString(),
                    MONTH_NAME = row["MONTH_NAME"].ToString()

                }).ToList();
            return item;
            }
            catch (Exception e)
            {
                ExceptionReturn = e.Message;
                return ExceptionReturn;
            }
        }

        public string SaveUpdate(List<DataUploadPrescriptionTargetBEO> model,string MonthNumber,string Year)
        {
            string status = "No";
            try
            {
                if (model != null)
                {
                    foreach (var detail in model)
                    {
                        //if (detail.SET_DATE == null)
                        //{
                        //    detail.SET_DATE = DateTime.Now.ToString("dd-MM-yyyy");
                        //}
                        var query = "";
                        detail.PRESCRIPTION_QTY = detail.PRESCRIPTION_QTY ?? "0";
                        detail.YEAR =Year ;
                        detail.MONTH_NUMBER = MonthNumber;
                        detail.MPO_CODE = detail.MPO_CODE?? "";
                        
                        if (PrescriptionTargetIsExist(detail))
                        {
                            //MaxID = ""; IUMode = "U";
                            query = "Update FSM_PRESCRIPTION_TARGET set PRESCRIPTION_QTY = '" + detail.PRESCRIPTION_QTY.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                    ",YEAR = " + detail.YEAR.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "," +
                                    "MONTH_NUMBER = '" + detail.MONTH_NUMBER.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "' " +
                                    "Where MPO_CODE = '" + detail.MPO_CODE.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "' and TO_CHAR(SET_DATE,'MM-YYYY')='" + DateTime.Now.ToString("MM-yyyy") + "'  ";
                        }
                        else
                        {
                            if (detail.SET_DATE == null)
                            {
                                detail.SET_DATE = DateTime.Now.ToString("M-d-yyyy");
                            }
                            query = "Insert into FSM_PRESCRIPTION_TARGET (MPO_CODE,TERRITORY_CODE,PRESCRIPTION_QTY,YEAR, MONTH_NUMBER,SET_DATE) " +
                                    "Values('" + detail.MPO_CODE.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                    ",'" + detail.TERRITORY_CODE.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                    "," + detail.PRESCRIPTION_QTY.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "" +
                                    "," + detail.YEAR.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "" +
                                    ",'" + detail.MONTH_NUMBER.Replace('\r', ' ').Replace('\n', ' ').Replace("'", "''") + "'" +
                                    ",TO_DATE('" + detail.SET_DATE + "','DD-MM-YYYY'))";
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


        private bool PrescriptionTargetIsExist(DataUploadPrescriptionTargetBEO detail)
        {
            bool isTrue = false;
            try
            {
                string qry = "SELECT MPO_CODE FROM FSM_PRESCRIPTION_TARGET WHERE MPO_CODE = '" + detail.MPO_CODE + "'  and TO_CHAR(SET_DATE,'MM-YYYY')='" + DateTime.Now.ToString("MM-yyyy") + "'  ";
                DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader("Sales"), qry);
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
        
        public object LoadExcelFilePrescriptionData(string fileName, string physicalPath)
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
                List<DataUploadPrescriptionTargetBEO> item = new List<DataUploadPrescriptionTargetBEO>();
                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    DataUploadPrescriptionTargetBEO pData = new DataUploadPrescriptionTargetBEO();
                  
                    pData.MPO_CODE = row["MPOCode"].ToString();
                    pData.TERRITORY_CODE = row["TerritoryCode4P"].ToString();
                    pData.PRESCRIPTION_QTY = row["PrescriptionQty"].ToString();
                    pData.SET_DATE = DateTime.Now.ToString("dd-MM-yyyy"); ;
                    
                    //if (!string.IsNullOrEmpty(pData.MPO_CODE)&&!string.IsNullOrEmpty(pData.TERRITORY_CODE))
                    if (!string.IsNullOrEmpty(pData.TERRITORY_CODE))
                    {
                        pData.SL_NO = ++count;
                        item.Add(pData);
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
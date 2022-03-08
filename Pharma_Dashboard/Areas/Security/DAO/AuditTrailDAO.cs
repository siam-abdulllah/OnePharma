using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.DAO
{
    public class AuditTrailDAO : ReturnData
    {
        private DBConnection dbConn = new DBConnection();
        private DBHelper dbHelper = new DBHelper();
        private IDGenerated idGenerated = new IDGenerated();
        public Boolean InsertAudit(string formName, string tableName, string actType, string trsId)
        {
            int EMPLOYEE_CODE = Convert.ToInt32(System.Web.HttpContext.Current.Session["EMPLOYEE_CODE"]);
            string ACCESS_LEVEL = System.Web.HttpContext.Current.Session["ACCESS_LEVEL"].ToString();
            string today = DateTime.Now.ToString("M/d/yyyy");
            long MaxID;
            string IUMode;
            try
            {



                MaxID = idGenerated.getMAXSL("SA_AUDIT_TRAIL", "AUDIT_ID", "Dashboard");
                IUMode = "I";
                string ip = GetLanIPAddress();
                string Qry = "Insert into PAL_DASH.SA_AUDIT_TRAIL (AUDIT_ID, ACTION_BY, TERMINAL,ACTION_DATE, ACTIVITY_TYPE, ACTION_FORM," +
                       "ACTION_TABLE, TRANSACTION_ID) Values(" + MaxID + ", " + EMPLOYEE_CODE + ",'" + ip + "',TO_DATE('" + today + "','MM/DD/RRRR'),'" + actType + "'," +
                       "'" + formName + "','" + tableName + "','" + trsId + "')";




                if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Dashboard"), Qry))
                {
                    return true;
                }
                return false;
            }
            catch (Exception errorException)
            {
                throw errorException;
            }
        }

        public String GetLanIPAddress()
        {
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            return ip;
        }
        public List<AuditTrail> GetAuditTrail(string FromDate, string ToDate, string Action_By, string Action_Type)
        {
            string query = "select sat.AUDIT_ID, sat.ACTION_BY, ei.EMPLOYEE_NAME ,sd.DESIG_DESC, sat.TERMINAL, TO_CHAR(sat.ACTION_DATE,'DD/MM/YYYY') ACTION_DATE, sat.ACTIVITY_TYPE, sat.ACTION_FORM, sat.ACTION_TABLE, sat.TRANSACTION_ID from SA_AUDIT_TRAIL sat INNER JOIN EMPLOYEE_INFO ei ON sat.ACTION_BY =  ei.EMPLOYEE_CODE LEFT JOIN SC_DESIGNATION sd ON ei.DESIGNATION = sd.DESIG_CODE where sat.ACTION_DATE between TO_DATE('" + FromDate + "','DD/MM/YYYY') and TO_DATE('"+ ToDate + "','DD/MM/YYYY')";
            if (!string.IsNullOrEmpty(Action_By))
            {
                query += " and sat.ACTION_BY = '"+ Action_By + "'";
            }
            if (!string.IsNullOrEmpty(Action_Type))
            {
                query += " and sat.ACTIVITY_TYPE = '" + Action_Type + "'";
            }
            query += " ORDER BY ACTION_DATE DESC";
            DataTable dataTable = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), query);
            var auditTrails = (from DataRow row in dataTable.Rows
                       select new AuditTrail()
                       {
                           EmployeeName = row["EMPLOYEE_NAME"].ToString(),
                           DesignationName = row["DESIG_DESC"].ToString(),
                           Terminal = row["TERMINAL"].ToString(),
                           Action_Date = row["ACTION_DATE"].ToString(),
                           Activity_Type = row["ACTIVITY_TYPE"].ToString(),
                           Action_Form = row["ACTION_FORM"].ToString(),
                           Action_Table = row["ACTION_TABLE"].ToString(),
                           Transaction_ID = row["TRANSACTION_ID"].ToString(),     
                       }).ToList();
            return auditTrails;
        }
    }
}
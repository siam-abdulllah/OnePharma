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
    public class RoleConfDAO : ReturnData
    {
        private DBConnection dbConn = new DBConnection();
        private DBHelper dbHelper = new DBHelper();
        private IDGenerated idGenerated = new IDGenerated();
        private readonly AuditTrailDAO _adt = new AuditTrailDAO();
        public List<EmployeeInfo> GetActiveEmployeeInfoList()
        {
            try
            {
                //string Qry = "select sul.USER_ID,ei.EMPLOYEE_CODE,ei.EMPLOYEE_NAME,ei.DESIGNATION,sd.DESIG_DESC  from SA_USER_LOGIN sul inner join EMPLOYEE_INFO ei on SUL.EMPLOYEE_CODE = ei.EMPLOYEE_CODE LEFT JOIN SC_DESIGNATION sd ON ei.DESIGNATION=sd.DESIG_CODE   where ei.STATUS='A'";
                string Qry = "SELECT sul.USER_ID,ei.EMPLOYEE_CODE,ei.EMPLOYEE_NAME,ei.DESIGNATION,sd.DESIG_NAME DESIG_DESC " +
                    " FROM SA_USER_LOGIN sul INNER JOIN EMPLOYEE_INFO ei ON SUL.EMPLOYEE_CODE = ei.EMPLOYEE_CODE LEFT JOIN " +
                    " SC_DESIGNATION sd ON ei.DESIGNATION = sd.DESIG_CODE WHERE ei.STATUS = 'A' UNION SELECT sul.USER_ID,ei.EMP_CODE " +
                    " EMPLOYEE_CODE,ei.EMPLOYEE_NAME,ei.DESIG_CODE,sd.DESIG_NAME DESIG_DESC FROM SA_USER_LOGIN sul INNER JOIN " +
                    " OPA_SCS.SC_EMPLOYEE ei ON SUL.EMPLOYEE_CODE = ei.EMP_CODE LEFT JOIN SC_DESIGNATION sd ON " +
                    " ei.DESIG_CODE = sd.DESIG_CODE WHERE ei.EMP_STATUS = 'A'";
                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);
                List<EmployeeInfo> item;

                item = (from DataRow row in dt.Rows
                        select new EmployeeInfo
                        {
                            UserID = row["USER_ID"].ToString(),
                            EmployeeCode = row["EMPLOYEE_CODE"].ToString(),
                            EmployeeName = row["EMPLOYEE_NAME"].ToString(),
                            DesignationCode = row["DESIGNATION"].ToString(),
                            DesignationDetail = row["DESIG_DESC"].ToString()
                        }).ToList();
                return item;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<EmployeeInfo> GetEmployeeByRoleList(string roleId)
        {
            try
            {
                //string Qry = "select rlc.RC_ID,rlc.USER_ID, ei.EMPLOYEE_CODE,ei.EMPLOYEE_NAME,ei.DESIGNATION,sd.DESIG_DESC from SA_ROLE_CONF rlc join SA_USER_LOGIN sul on rlc.USER_ID = sul.USER_ID JOIN  EMPLOYEE_INFO ei on sul.EMPLOYEE_CODE= ei.EMPLOYEE_CODE LEFT JOIN SC_DESIGNATION sd ON ei.DESIGNATION=sd.DESIG_CODE where ei.STATUS='A' and rlc.ROLE_ID = '" + roleId + "'";
                string Qry = "SELECT rlc.RC_ID,rlc.USER_ID,ei.EMPLOYEE_CODE,ei.EMPLOYEE_NAME,ei.DESIGNATION,sd.DESIG_NAME DESIG_DESC" +
                    " FROM SA_ROLE_CONF rlc JOIN SA_USER_LOGIN sul ON rlc.USER_ID = sul.USER_ID JOIN EMPLOYEE_INFO ei ON " +
                    " sul.EMPLOYEE_CODE = ei.EMPLOYEE_CODE LEFT JOIN SC_DESIGNATION sd ON ei.DESIGNATION = sd.DESIG_CODE " +
                    " WHERE ei.STATUS = 'A' and rlc.ROLE_ID = '" + roleId + "'  UNION ALL SELECT rlc.RC_ID,rlc.USER_ID,ei.EMP_CODE,ei.EMPLOYEE_NAME, ei.DESIG_CODE," +
                    " sd.DESIG_NAME DESIG_DESC FROM SA_ROLE_CONF rlc JOIN SA_USER_LOGIN sul ON rlc.USER_ID = sul.USER_ID " +
                    " INNER JOIN OPA_SCS.SC_EMPLOYEE ei ON SUL.EMPLOYEE_CODE = ei.EMP_CODE LEFT JOIN SC_DESIGNATION " +
                    " sd ON ei.DESIG_CODE = sd.DESIG_CODE WHERE ei.EMP_STATUS = 'A' and rlc.ROLE_ID = '" + roleId + "' ";
                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);
                List<EmployeeInfo> item;

                item = (from DataRow row in dt.Rows
                        select new EmployeeInfo
                        {
                            UserID = row["USER_ID"].ToString(),
                            EmployeeCode = row["EMPLOYEE_CODE"].ToString(),
                            EmployeeName = row["EMPLOYEE_NAME"].ToString(),
                            DesignationCode = row["DESIGNATION"].ToString(),
                            DesignationDetail = row["DESIG_DESC"].ToString()
                        }).ToList();
                return item;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        public bool SaveRLConf(SecRoleConf master)
        {
            if (master != null)
            {
                try
                {
                    string userId = "";
                    string qry = "";
                    string UserQry = "SELECT * FROM SA_ROLE_CONF src WHERE src.USER_ID='" + master.USER_ID + "'";
                    using (OracleConnection oracleConnection = new OracleConnection(dbConn.SAConnStrReader("Dashboard")))
                    {
                        oracleConnection.Open();
                        using (OracleCommand oracleCommand = new OracleCommand(UserQry, oracleConnection))
                        {
                            using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                            {
                                if (rdr.Read())
                                {
                                    userId = rdr["USER_ID"].ToString();
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(userId))
                    {
                        IUMode = "U";
                        qry = "Update SA_ROLE_CONF set ROLE_ID='" + master.RL_ID + "' Where USER_ID='" + master.USER_ID + "'";
                    }
                    else
                    {
                        MaxID = idGenerated.getMAXSL("SA_ROLE_CONF", "RC_ID", "Dashboard").ToString();
                        IUMode = "I";
                        qry = "Insert into SA_ROLE_CONF(RC_ID,ROLE_ID, USER_ID) Values('" + MaxID + "','" + master.RL_ID + "', '" + master.USER_ID + "')";
                    }

                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Dashboard"), qry))
                    {
                        // _adt.InsertAudit("frmRoleConf", "SA_ROLE_CONF", IUMode, MaxID);
                        return true;
                    }
                    return false;
                }
                catch (Exception errorException)
                {
                    throw errorException;
                }

            }
            return false;
        }
    }
}
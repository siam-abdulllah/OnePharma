using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.DAO
{
    public class ChngPassDAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        UserLogInDAO userLogInDAO = new UserLogInDAO();
        private readonly AuditTrailDAO _adt = new AuditTrailDAO();
        public bool CheckCurrentPassword(string currentPassword)
        {
            bool status = false;
            string userName = HttpContext.Current.Session["USER_NAME"].ToString();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(currentPassword))
            {
                string uQry = "Select * from SA_USER_LOGIN where USER_NAME= '" + userName + "' and PASSWORD= '" + currentPassword + "'";

                using (OracleConnection oracleConnection = new OracleConnection(dbConn.SAConnStrReader("Dashboard")))
                {
                    oracleConnection.Open();
                    using (OracleCommand oracleCommand = new OracleCommand(uQry, oracleConnection))
                    {
                        using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                if (rdr.Read())
                                {
                                    status = true;
                                }
                            }
                        }
                    }
                }
            }
            return status;
        }
        public bool UpdatePassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                try
                {
                    string trID="0";
                    string userId = HttpContext.Current.Session["USER_ID"].ToString();
                    string uQry = "Select * from SA_USER_LOGIN where USER_ID= '" + userId + "' and PASSWORD= '" + password + "'";

                    using (OracleConnection oracleConnection = new OracleConnection(dbConn.SAConnStrReader("Dashboard")))
                    {
                        oracleConnection.Open();
                        using (OracleCommand oracleCommand = new OracleCommand(uQry, oracleConnection))
                        {
                            using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    if (rdr.Read())
                                    {
                                        trID = rdr[0].ToString();
                                    }
                                }
                            }
                        }
                    }
                    string qry = "Update SA_USER_LOGIN set PASSWORD='" + password + "' Where USER_ID='" + userId + "'";
                    IUMode = "U";
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Dashboard"), qry))
                    {
                       // _adt.InsertAudit("frmChngPass", "SA_USER_LOGIN", IUMode, trID);
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
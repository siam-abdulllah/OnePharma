using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Security.DAO
{
    public class UserCreationDAO
    {
        public long MaxID { get; set; }
        public string IUMode { get; set; }

        private DBConnection dbConn = new DBConnection();
        private DBHelper dbHelper = new DBHelper();
        private IDGenerated idGenerated = new IDGenerated();

        public List<EmployeeInfo> GetActiveEmployeeInfoList()
        {
            try
            {
                string Qry = "  SELECT A.EMPLOYEE_CODE,A.EMPLOYEE_NAME,B.DESIG_NAME DESIGNATION, B.DESIG_DESC,A.POSTING_LOCATION, A.DEPOT_CODE " +
                    "FROM EMPLOYEE_INFO A " +
                    "LEFT JOIN SC_DESIGNATION B ON A.DESIGNATION = B.DESIG_CODE " +
                    "WHERE A.EMPLOYEE_CODE NOT IN(SELECT EMPLOYEE_CODE FROM SA_USER_LOGIN) AND A.STATUS = 'A' " +
                    "ORDER BY A.EMPLOYEE_NAME";
                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);
                List<EmployeeInfo> item;

                item = (from DataRow row in dt.Rows
                        select new EmployeeInfo
                        {
                            EmployeeCode = row["EMPLOYEE_CODE"].ToString(),
                            EmployeeName = row["EMPLOYEE_NAME"].ToString(),
                            DesignationCode = row["DESIGNATION"].ToString(),
                            DesignationDetail = row["DESIG_DESC"].ToString(),
                            PostingLocation = row["POSTING_LOCATION"].ToString(),
                            DepotCode = row["DEPOT_CODE"].ToString()

                        }).ToList();
                return item;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<UserEmployeeInfo> GetUserList()
        {
            try
            {
                string Qry = "SELECT DISTINCT A.EMPLOYEE_CODE,A.EMPLOYEE_NAME,B.DESIG_NAME DESIGNATION, B.DESIG_DESC,A.POSTING_LOCATION, A.DEPOT_CODE,C.GROUP_CODE " +
                    ",C.USER_ID,C.PASSWORD,C.STATUS,C.ACCESS_LEVEL " +
                    "FROM EMPLOYEE_INFO A " +
                    "LEFT JOIN SA_USER_LOGIN C ON A.EMPLOYEE_CODE = C.EMPLOYEE_CODE " +
                    "LEFT JOIN SC_DESIGNATION B ON A.DESIGNATION = B.DESIG_CODE " +
                    "WHERE C.STATUS = 'A' AND C.GROUP_CODE='01' " +
                    "ORDER BY A.EMPLOYEE_NAME";

                // string Qry = "select USER_NAME,PASSWORD,STATUS,ACCESS_LEVEL,EMPLOYEE_CODE,CODE FROM SA_USER_LOGIN";
                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);
                List<UserEmployeeInfo> item;

                item = (from DataRow row in dt.Rows
                        select new UserEmployeeInfo
                        {
                            EmployeeCode = row["EMPLOYEE_CODE"].ToString(),
                            EmployeeName = row["EMPLOYEE_NAME"].ToString(),
                            DesignationCode = row["DESIGNATION"].ToString(),
                            DesignationDetail = row["DESIG_DESC"].ToString(),
                            PostingLocation = row["POSTING_LOCATION"].ToString(),
                            DepotCode = row["DEPOT_CODE"].ToString(),
                            GroupCode = row["GROUP_CODE"].ToString(),
                            UserId = Convert.ToInt32(row["USER_ID"]),
                            Password = row["PASSWORD"].ToString(),
                            Status = row["STATUS"].ToString(),
                            AccessLevel = row["ACCESS_LEVEL"].ToString()
                        }).ToList();
                return item;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public bool SaveUpdate(UserLogin userLogin, string userId)
        {
            bool isTrue = false;
            using (OracleConnection con = new OracleConnection(dbConn.SAConnStrReader("Dashboard")))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    try
                    {
                        string Qry = "";
                        //
                        if (string.IsNullOrEmpty(userId))
                        {
                            MaxID = idGenerated.getMAXSL("SA_USER_LOGIN", "USER_ID", "Dashboard");

                            IUMode = "I";

                            if (userLogin.AccessLevel == "D")
                            {
                                userLogin.Code = userLogin.DepotCode;
                            }
                            else
                            {
                                userLogin.Code = userLogin.EmployeeCode;
                            }

                            Qry = "Insert into SA_USER_LOGIN (USER_ID, USER_NAME, PASSWORD,STATUS,ACCESS_LEVEL,EMPLOYEE_CODE,GROUP_CODE,CODE) " +
                                "       Values(:MaxID , :Username,:Password,:Status,:AccessLevel,:EmployeeCode,:GroupCode,:Code)";

                            OracleParameter[] paramUser = new OracleParameter[]
                               {
                                    new OracleParameter("MaxID",Convert.ToInt32(MaxID)),
                                    new OracleParameter("Username",userLogin.Username),
                                    new OracleParameter("Password",userLogin.Password),
                                    new OracleParameter("Status",userLogin.Status),
                                    new OracleParameter("AccessLevel",userLogin.AccessLevel),
                                    new OracleParameter("EmployeeCode", userLogin.EmployeeCode),
                                    new OracleParameter("GroupCode",userLogin.GroupCode),
                                    new OracleParameter("Code",userLogin.Code)
                               };
                            cmd.CommandText = Qry;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(paramUser);
                            isTrue = cmd.ExecuteNonQuery() > 0;
                        }
                        else
                        {
                            MaxID = userLogin.UserId;
                            IUMode = "U";

                            if (userLogin.AccessLevel == "D")
                            {
                                userLogin.Code = userLogin.DepotCode;
                            }
                            else
                            {
                                userLogin.Code = userLogin.EmployeeCode;
                            }
                            Qry = "Update SA_USER_LOGIN set PASSWORD=:Password," +
                                " STATUS=:Status, ACCESS_LEVEL=:AccessLevel,CODE=:Code Where USER_NAME=:Username ";

                            OracleParameter[] paramUser = new OracleParameter[]
                              {
                                    new OracleParameter("Username",userLogin.Username),
                                    new OracleParameter("Password",userLogin.Password),
                                    new OracleParameter("Status",userLogin.Status),
                                    new OracleParameter("AccessLevel",userLogin.AccessLevel),
                                    new OracleParameter("Code",userLogin.Code)
                              };
                            cmd.CommandText = Qry;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(paramUser);
                            isTrue = cmd.ExecuteNonQuery() > 0;
                        }

                        return isTrue;
                    }
                    catch (Exception errorException)
                    {
                        throw errorException;
                    }
                }
            }
        }
    }
}
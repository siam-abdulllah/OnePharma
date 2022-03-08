using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Universal.BEL;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Universal.DAL
{
    public class LoginRegistrationDAO
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();

        public List<LoginRegistrationBEL> CheckUserCredential()
        {
            try
            {


                string uQry = "SELECT USER_ID, USER_NAME, PASSWORD,EMPLOYEE_CODE,GROUP_CODE FROM SA_USER_LOGIN WHERE STATUS='A'";
                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), uQry);

                var item = (from DataRow row in dt.Rows
                    select new LoginRegistrationBEL
                    {
                        USER_ID = row["USER_ID"].ToString(),
                        USER_NAME = row["USER_NAME"].ToString(),
                        PASSWORD = row["PASSWORD"].ToString(),
                        EMPLOYEE_CODE = row["EMPLOYEE_CODE"].ToString(),
                        GROUP_CODE = row["GROUP_CODE"].ToString(),
                        //SupervisorID = row["SupervisorID"].ToString(),
                        //SupervisorName = row["SupervisorName"].ToString(),
                        //Designation = row["Designation"].ToString(),
                        //EmploymentDate = row["EmploymentDate"].ToString(),
                        //IsActive = Convert.ToBoolean(row["IsActive"].ToString())

                    }).ToList();
                return item;
            }
            catch (Exception e)
            {
                throw;
            }

        }
    }
}
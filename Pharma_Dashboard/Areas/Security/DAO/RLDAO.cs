
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.DAO
{
    public class RLDAO : ReturnData
    {
        private DBConnection dbConn = new DBConnection();
        private DBHelper dbHelper = new DBHelper();
        private IDGenerated idGenerated = new IDGenerated();
        private readonly AuditTrailDAO _adt = new AuditTrailDAO();
        public List<SecRL> GetRoleList()
        {
            string Qry = "SELECT ROLE_ID, ROLE_NAME FROM SA_ROLE Order by ROLE_ID";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);
            List<SecRL> item;

            item = (from DataRow row in dt.Rows
                    select new SecRL
                    {
                        ID = row["ROLE_ID"].ToString(),
                        Code = row["ROLE_ID"].ToString(),
                        Name = row["ROLE_NAME"].ToString(),
                    }).ToList();
            return item;
        }
        public bool SaveUpdate(SecRL secMh)
        {
            try
            {
                string Qry = "";
                //
                if (string.IsNullOrEmpty(secMh.ID))
                {
                    MaxID = idGenerated.getMAXSL("SA_ROLE", "ROLE_ID", "Dashboard").ToString();
                    IUMode = "I";
                    Qry = "Insert into SA_ROLE(ROLE_ID, ROLE_NAME) Values('" + MaxID + "','" + secMh.Name + "')";
                }
                else
                {//U for Insert
                    MaxID = secMh.ID;
                    IUMode = "U";
                    Qry = "Update SA_ROLE set ROLE_ID='" + secMh.Code + "', ROLE_NAME='" + secMh.Name + "' Where ROLE_ID='" + secMh.ID + "'";
                }

                if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Dashboard"), Qry))
                {
                   // _adt.InsertAudit("frmRL", "SA_ROLE", IUMode, MaxID);
                    return true;
                }
                return false;
            }
            catch (Exception errorException)
            {
                throw errorException;
            }
        }
    }
}
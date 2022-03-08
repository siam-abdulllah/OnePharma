
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.Security.Controllers;
using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.DAO
{
    public class MHDAO:ReturnData
    {
        private DBConnection dbConn = new DBConnection();
        private DBHelper dbHelper = new DBHelper();
        private IDGenerated idGenerated = new IDGenerated();
        private readonly AuditTrailDAO _adt = new AuditTrailDAO();
        public List<SecMH> GetHeadMenuList()
        {
            string Qry = "SELECT MH_ID, MH_NAME, MH_SEQ FROM SA_MENU_HEAD Order by MH_SEQ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);
            List<SecMH> item;

            item = (from DataRow row in dt.Rows
                    select new SecMH
                    {
                        ID = row["MH_ID"].ToString(),
                        Name = row["MH_NAME"].ToString(),
                        Seq = row["MH_SEQ"].ToString(),
                    }).ToList();
            return item;
        }
        public bool SaveUpdate(SecMH secMh)
        {
            try
            {
                string Qry = "";
                //
                if (string.IsNullOrEmpty(secMh.ID))
                {
                    MaxID = idGenerated.getMAXSL("SA_MENU_HEAD", "MH_ID","Dashboard").ToString();
                    IUMode = "I";
                    Qry = "Insert into SA_MENU_HEAD(MH_ID, MH_SEQ, MH_NAME) Values('" + MaxID + "', '" + secMh.Seq + "','" + secMh.Name + "')";

                }
                else
                {//U for Insert
                    MaxID = secMh.ID;
                    IUMode = "U";
                    Qry = "Update SA_MENU_HEAD set MH_NAME='" + secMh.Name + "', MH_SEQ='" + secMh.Seq + "' Where MH_ID='" + secMh.ID + "'";
                }

                if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Dashboard"), Qry))
                {
                   // _adt.InsertAudit("frmMH", "SA_MENU_HEAD", IUMode, MaxID);

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
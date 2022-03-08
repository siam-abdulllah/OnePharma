using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.DAO
{
    public class SMDAO : ReturnData
    {
        private DBConnection dbConn = new DBConnection();
        private DBHelper dbHelper = new DBHelper();
        private IDGenerated idGenerated = new IDGenerated();
        private readonly AuditTrailDAO _adt = new AuditTrailDAO();
        public List<SecSM> GetSubMenuList()
        {

            string Qry = "SELECT A.SM_ID, A.SM_NAME AS Subname, A.URL, A.SM_SEQ, A.MH_ID, B.MH_NAME FROM SA_SUB_MENU A " +
                         "JOIN SA_MENU_HEAD B ON B.MH_ID = A.MH_ID " +
                         "ORDER BY B.MH_SEQ, A.SM_SEQ";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);

            List<SecSM> item;

            item = (from DataRow row in dt.Rows
                    select new SecSM
                    {
                        ID = row["SM_ID"].ToString(),
                        Name = row["MH_NAME"].ToString(),
                        SubName = row["Subname"].ToString(),
                        MH_ID = row["MH_ID"].ToString(),
                        Url = row["URL"].ToString(),
                        Sequence = row["SM_SEQ"].ToString()
                    }).ToList();

            return item;
        }

        public List<SecSM> GetSubMenuListByMenuHeadId(string RL_ID,string MH_ID)
        {
            string Qry = "select A.SM_ID, A.SM_NAME AS Subname from SA_SUB_MENU a inner join SA_MENU_HEAD b on a.MH_ID=b.MH_ID where a.SM_ID NOT IN (SELECT c.SM_ID FROM SA_MENU_CONF c WHERE c.RL_ID='"+RL_ID+"' AND c.MH_ID='"+MH_ID+"') AND a.MH_ID='"+MH_ID+"'";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);

            List<SecSM> item;

            item = (from DataRow row in dt.Rows
                    select new SecSM
                    {
                        ID = row["SM_ID"].ToString(),
                        Name = row["Subname"].ToString(),
                    }).ToList();

            return item;
        }

        public bool SaveUpdate(SecSM secSm)
        {
            try
            {
                string Qry = "";
                //
                if (string.IsNullOrEmpty(secSm.ID))
                {
                    MaxID = idGenerated.getMAXSL("SA_SUB_MENU", "SM_ID", "Dashboard").ToString();
                    IUMode = "I";
                    Qry = "Insert into SA_SUB_MENU(SM_ID, SM_SEQ, SM_NAME, URL, MH_ID) Values('" + MaxID + "', '" + secSm.Sequence + "','" + secSm.Name + "' , '" + secSm.Url + "', " + secSm.MH_ID + ")";
                }
                else
                {//U for Insert
                    MaxID = secSm.ID;
                    IUMode = "U";
                    Qry = "Update SA_SUB_MENU set SM_NAME='" + secSm.Name + "', MH_ID='" + secSm.MH_ID + "' , SM_SEQ='" + secSm.Sequence + "' , URL='" + secSm.Url + "' Where SM_ID='" + secSm.ID + "'";
                }

                if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Dashboard"), Qry))
                {
                   // _adt.InsertAudit("frmSM", "SA_SUB_MENU", IUMode, MaxID);
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
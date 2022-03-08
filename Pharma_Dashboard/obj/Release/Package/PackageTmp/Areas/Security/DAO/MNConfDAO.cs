using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;

namespace PAsia_Dashboard.Areas.Security.DAO
{
    public class MNConfDAO : ReturnData
    {
        private DBConnection dbConn = new DBConnection();
        private DBHelper dbHelper = new DBHelper();
        private IDGenerated idGenerated = new IDGenerated();
        private readonly AuditTrailDAO _adt = new AuditTrailDAO();
        internal List<SecRoleMenuConf> GetMNConfListByMHRL(int RL_ID, int MH_ID)
        {

            string Qry = "SELECT a.MENU_ID,a.SM_ID,c.SM_NAME, a.MH_ID,b.MH_NAME,a.RL_ID,d.ROLE_NAME,a.SV,a.VW,a.DL from SA_MENU_CONF a join SA_MENU_HEAD b on a.MH_ID=b.MH_ID join SA_SUB_MENU c on a.SM_ID = c.SM_ID join SA_ROLE d on a.RL_ID=d.ROLE_ID where a.RL_ID=" + RL_ID+ " and a.MH_ID="+MH_ID+"";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), Qry);

            List<SecRoleMenuConf> item;

            item = (from DataRow row in dt.Rows
                    select new SecRoleMenuConf
                    {
                        ID = row["MENU_ID"].ToString(),
                        SM_ID = row["SM_ID"].ToString(),
                        SM_NM = row["SM_NAME"].ToString(),
                        RL_ID = row["RL_ID"].ToString(),
                        RL_NM = row["ROLE_NAME"].ToString(),
                        MH_ID = row["MH_ID"].ToString(),
                        MH_NM = row["MH_NAME"].ToString(),
                        Sv = row["SV"].ToString(),
                        Vw = row["VW"].ToString(),
                        Dl = row["DL"].ToString()
                    }).ToList();

            return item;
        }
        public bool SaveUpdate(SecRoleMenuConf secRoleMenuConf)
        {
            try
            {
                string Qry = "";
                if (string.IsNullOrEmpty(secRoleMenuConf.ID))
                {
                    MaxID = idGenerated.getMAXSL("SA_MENU_CONF", "MENU_ID", "Dashboard").ToString();
                    IUMode = "I";
                    Qry = "Insert into SA_MENU_CONF(MENU_ID,SM_ID, MH_ID, RL_ID,VW,SV,DL) Values('" + MaxID + "', '" + secRoleMenuConf.SM_ID + "','" + secRoleMenuConf.MH_ID + "' , '" + secRoleMenuConf.RL_ID + "','','"+secRoleMenuConf.Sv+"','"+ secRoleMenuConf .Dl+ "')";
                }
                else
                {
                    MaxID = secRoleMenuConf.ID;
                    IUMode = "U";
                    Qry = "Update SA_MENU_CONF set SM_ID='" + secRoleMenuConf.SM_ID + "', MH_ID='" + secRoleMenuConf.MH_ID + "' , RL_ID='" + secRoleMenuConf.RL_ID + "',SV='" + secRoleMenuConf.Sv + "', DL='" + secRoleMenuConf.Dl + "' Where MENU_ID='" + secRoleMenuConf.ID + "'";
                }

                if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Dashboard"), Qry))
                {
                   // _adt.InsertAudit("frmMNConf", "SA_MENU_CONF", IUMode, MaxID);
                    return true;
                }
                return false;
            }
            catch (Exception errorException)
            {
                throw errorException;
            }
        }
        public bool DeleteMNConf(int ID)
        {
            try
            {
                string qry = "delete from SA_MENU_CONF where MENU_ID='"+ ID + "'";
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader("Dashboard"), qry))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
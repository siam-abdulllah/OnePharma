using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Universal.Gateway
{
    public class IDGenerated
    {
        DBConnection dbConnection = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        public Int64 getMAXSL(string tableName, string columnName, string ConnString)
        {
            Int64 MAXID = 0;
            string QueryString = "select NVL(MAX(" + columnName + "),0)+1 id from " + tableName + "";
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.SAConnStrReader(ConnString)))
            {
                oracleConnection.Open();
                using (OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection))
                {
                    using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            MAXID = Convert.ToInt64(rdr["id"].ToString());
                        }
                    }
                }
            }
            return MAXID;
        }
        public string getMAXID(string tableName, string columnName, string fm9, string ConnString)
        {
            string MAXID = "";
            string QueryString = "select to_char((select NVL(MAX(" + columnName + "),0)+1 from " + tableName + " ), '" + fm9 + "') id from dual";
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.SAConnStrReader(ConnString)))
            {
                oracleConnection.Open();
                using (OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection))
                {
                    using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            MAXID = rdr[0].ToString();
                        }
                    }
                }
            }
            return MAXID;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Data;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Universal.Gateway
{
    public class DBHelper
    {
        DBConnection dbConnection = new DBConnection();
        public Boolean CmdExecute(string ConnString, string Qry)
        {
            bool isTrue = false;
            using (OracleConnection con = new OracleConnection(ConnString))
            {
                OracleCommand cmd = new OracleCommand(Qry, con);
                con.Open();
                int noOfRows = cmd.ExecuteNonQuery();

                if (noOfRows > 0)
                {
                    isTrue = true;

                }
            }
            return isTrue;
        }


        public Tuple<Boolean, string> IsExistsWithGetSL(string ConnString, string Qry)
        {
            string GetSL = ""; bool isTrue = false;

            DataTable dt = GetDataTable(ConnString, Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
                GetSL = dt.Rows[0][0].ToString();
            }
            return Tuple.Create(isTrue, GetSL);
        }
        public string GetSingleToken(string MPGRoup, string ConnType)
        {

            string GetToken = "";
            string Qry = "Select Token From Sa_UserToken Where MP_Group='" + MPGRoup + "'";
            DataTable dt = GetDataTable(dbConnection.SAConnStrReader(ConnType), Qry);
            if (dt.Rows.Count > 0)
            {
                GetToken = dt.Rows[0][0].ToString();
            }
            return GetToken;
        }
        public string GetMultipleToken(string MPGRoup, string ConnType)
        {
            string GetToken = "";
            string Qry = "Select Token From Sa_UserToken Where MP_Group in (" + MPGRoup + ")";
            DataTable dt = GetDataTable(dbConnection.SAConnStrReader(ConnType), Qry);
            if (dt.Rows.Count > 0)
            {

                GetToken = dt.Rows[0][0].ToString();
            }
            return "";
        }
        //public Int64 CmdExecute(string Qry, string ConnType)
        //{

        //    Int64 noOfRows = 0;
        //    using (OracleConnection con = new OracleConnection(dbConnection.SAConnStrReader(ConnType)))
        //    {
        //        OracleCommand cmd = new OracleCommand(Qry, con);
        //        con.Open();
        //        noOfRows = cmd.ExecuteNonQuery();
        //    }
        //    return noOfRows;
        //}

        public OracleDataReader GetDataCustom(string Qry, string ConnType)
        {
            string data = "";

            OracleConnection objConn = new OracleConnection(dbConnection.SAConnStrReader(ConnType));
            
            objConn.Open();
            OracleCommand objCmd = new OracleCommand(Qry, objConn);
            OracleDataReader rdr = objCmd.ExecuteReader();
            //if (rdr.Read())
            // {
            // data = rdr[peram].ToString();
            // }
            rdr.Read();
            objConn.Close();
            objCmd.Cancel();
            return rdr;


        }
        //public OracleDataReader GetDataCustom(string Qry)
        //{
        //    string data = "";
        //    OracleDataReader d;
        //    using (OracleConnection objConn = new OracleConnection(dbConnection.SAConnStrReader(ConnType)))
        //    {

        //        objConn.Open();
        //        using (OracleCommand objCmd = new OracleCommand(Qry, objConn))
        //        {
        //            using (OracleDataReader rdr = objCmd.ExecuteReader())
        //            {
        //                rdr.Read();
        //                d = rdr;
        //            }
        //        }
        //    }

        //    return d;


        //}
        public DataSet GetDataSet(string Qry, string ConnType)
        {
            OracleDataAdapter odbcDataAdapter = new OracleDataAdapter(Qry, dbConnection.SAConnStrReader(ConnType));
            DataSet ds = new DataSet();
            odbcDataAdapter.Fill(ds, "Results");
            return ds;
        }

        public DataTable ReturnCursorF1(string Conn, string fName, string hInput1, string vInput1)
        {
            try
            {
                OracleConnection objConn = new OracleConnection(Conn);
                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = fName;//"get_count_emp_by_dept";
                objCmd.CommandType = CommandType.StoredProcedure;
                //objCmd.Parameters.Add(hInput1, OracleType.VarChar).Value = vInput1;    
                objCmd.Parameters.Add("return_value", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;
                objConn.Open();
                objCmd.ExecuteNonQuery();
                OracleDataReader rdr = objCmd.ExecuteReader();
                DataTable dt = new DataTable();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);

                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
        }
        public DataTable GetDataTable(string ConnString, string Qry)
        {
            DataTable dt = new DataTable();
            using (OracleConnection objConn = new OracleConnection(ConnString))
            {
                using (OracleCommand objCmd = new OracleCommand())
                {
                    objCmd.CommandText = Qry;
                    objCmd.Connection = objConn;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            dt.Load(rdr);
                        }
                    }
                }
            }
            return dt;
        }

        public string GetValue(string ConnString, string Qry)
        {
            string Value = "";
            OracleConnection odbcConnection = new OracleConnection(ConnString);
            odbcConnection.Open();
            OracleCommand odbcCommand = new OracleCommand(Qry, odbcConnection);
            OracleDataReader rdr = odbcCommand.ExecuteReader();
            if (rdr.Read())
            {
                Value = rdr[0].ToString();
            }
            rdr.Close();
            odbcConnection.Close();
            return Value;
        }

        public DataTable GetDataTableRefCursorF1(string funName, string FieldName, string FieldValue, string ConnType)
        {
            DataTable dt = new DataTable();
            using (OracleConnection objConn = new OracleConnection(dbConnection.SAConnStrReader(ConnType)))
            {
                using (OracleCommand objCmd = new OracleCommand())
                {
                    objCmd.Connection = objConn;
                    objCmd.CommandText = funName;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add(FieldName, OracleType.VarChar).Value = FieldValue;
                    objCmd.Parameters.Add("ReturnValue", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            dt.Load(rdr);
                        }
                    }
                }
            }
            return dt;
        }
        public DataTable GetDataTableRefCursorF2(string funName, string FieldName1, string FieldName2, string FieldValue1, string FieldValue2, string ConnType)
        {
            DataTable dt = new DataTable();
            using (OracleConnection objConn = new OracleConnection(dbConnection.SAConnStrReader(ConnType)))
            {

                using (OracleCommand objCmd = new OracleCommand())
                {
                    objCmd.Connection = objConn;
                    objCmd.CommandText = funName;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add(FieldName1, OracleType.VarChar).Value = FieldValue1;
                    objCmd.Parameters.Add(FieldName2, OracleType.VarChar).Value = FieldValue2;
                    objCmd.Parameters.Add("ReturnValue", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            dt.Load(rdr);
                        }
                    }
                }
            }
            return dt;
        }


        public DataTable GetDataTableRefCursorF3(string funName, string FieldName1, string FieldName2, string FieldName3, string FieldValue1, string FieldValue2, string FieldValue3, string ConnType)
        {
            DataTable dt = new DataTable();
            using (OracleConnection objConn = new OracleConnection(dbConnection.SAConnStrReader(ConnType)))
            {

                using (OracleCommand objCmd = new OracleCommand())
                {
                    objCmd.Connection = objConn;
                    objCmd.CommandText = funName;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add(FieldName1, OracleType.VarChar).Value = FieldValue1;
                    objCmd.Parameters.Add(FieldName2, OracleType.VarChar).Value = FieldValue2;
                    objCmd.Parameters.Add(FieldName3, OracleType.VarChar).Value = FieldValue3;
                    objCmd.Parameters.Add("ReturnValue", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    using (OracleDataReader rdr = objCmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            dt.Load(rdr);
                        }
                    }
                }
            }
            return dt;
        }


        public Boolean CmdProcedureF1(string Qry, string SPName, string FieldName, string FieldValue, string ConnType)
        {
            bool isTrue = false;
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.SAConnStrReader(ConnType)))
            {
                using (OracleCommand oracleCommand = new OracleCommand())
                {
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.CommandText = SPName;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add(FieldName, OracleType.VarChar).Value = FieldValue;
                    oracleConnection.Open();
                    if (oracleCommand.ExecuteNonQuery() > 0)
                    {

                        isTrue = true;

                    }
                }
            }
            return isTrue;
        }

        public Boolean CmdProcedureF2(string SPName, string FieldName1, string FieldName2, string FieldValue1, string FieldValue2, string ConnType)
        {
            bool isTrue = false;
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.SAConnStrReader(ConnType)))
            {
                using (OracleCommand oracleCommand = new OracleCommand())
                {
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.CommandText = SPName;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add(FieldName1, OracleType.VarChar).Value = FieldValue1;
                    oracleCommand.Parameters.Add(FieldName2, OracleType.VarChar).Value = FieldValue2;

                    oracleConnection.Open();

                    if (oracleCommand.ExecuteNonQuery() > 0)
                    {
                        isTrue = true;
                    }
                }
            }
            return isTrue;
        }

        public Boolean CmdProcedureF3(string SPName, string FieldName1, string FieldName2, string FieldName3, string FieldValue1, string FieldValue2, string FieldValue3, string ConnType)
        {
            bool isTrue = false;
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.SAConnStrReader(ConnType)))
            {
                using (OracleCommand oracleCommand = new OracleCommand())
                {
                    oracleCommand.Connection = oracleConnection;
                    oracleCommand.CommandText = SPName;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add(FieldName1, OracleType.VarChar).Value = FieldValue1;
                    oracleCommand.Parameters.Add(FieldName2, OracleType.VarChar).Value = FieldValue2;
                    oracleCommand.Parameters.Add(FieldName3, OracleType.VarChar).Value = FieldValue3;
                    oracleConnection.Open();

                    if (oracleCommand.ExecuteNonQuery() > 0)
                    {
                        isTrue = true;
                    }
                }
            }
            return isTrue;
        }





        public DataTable dt { get; set; }
    }
}

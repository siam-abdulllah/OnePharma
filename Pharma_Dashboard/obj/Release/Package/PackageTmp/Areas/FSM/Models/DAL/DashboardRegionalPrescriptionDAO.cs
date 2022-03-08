using PAsia_Dashboard.Areas.FSM.Models.BEL;
using PAsia_Dashboard.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.DAL
{
    public class DashboardRegionalPrescriptionDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConnection = new DBConnection();

        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
        string CntMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.CurrentCulture);
        string LastMonthYear = DateTime.Now.AddMonths(-1).ToString("MM-yyyy", CultureInfo.CurrentCulture);

        public DashboardRegionalPrescriptionBEO GetRegionalPrescriptionData()
        {
            DashboardRegionalPrescriptionBEO model = new DashboardRegionalPrescriptionBEO();

            try
            {
                string ProductNameQry = "Select NVL(SUM(PRACTICING_DAY*PRESCRIPTION_PER_DAY),0) Total  from FSM_DOC_HONORARIUM ";
                string TodayPrescriptionQry = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where TO_CHAR(SET_DATE,'dd-mm-yyyy')='" + CntDate + "' and DOCTOR_CODE IN (Select distinct DOCTOR_CODE from FSM_DOC_HONORARIUM) ";

                string CumulativeQry = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where To_Date(SET_DATE,'dd-mm-yyyy')<=To_Date('" + CntDate + "','dd-mm-yyyy') and DOCTOR_CODE IN (Select distinct DOCTOR_CODE from FSM_DOC_HONORARIUM)";


                string LastMPSD = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where To_Date(SET_DATE,'dd-mm-yyyy') Between To_Date('" + "01-" + LastMonthYear + "','dd-mm-yyyy') AND To_Date('" + CntDate + "','dd-mm-yyyy') and DOCTOR_CODE IN (Select distinct DOCTOR_CODE from FSM_DOC_HONORARIUM)";
                string LastMonth = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where TO_CHAR(SET_DATE,'MM-YYYY')='" + LastMonthYear + "' and DOCTOR_CODE IN (Select distinct DOCTOR_CODE from FSM_DOC_HONORARIUM)";




                model.ProductName = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), ProductNameQry);
                model.TodayPrescription = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), TodayPrescriptionQry);
                model.Cumulative = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), CumulativeQry);
                model.Achievement = model.ProductName == "0" ? "0" : (Convert.ToDecimal(model.Cumulative) * 100 / Convert.ToDecimal(model.ProductName)).ToString("0.##");

                model.LastMPSD = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), LastMPSD);
                model.LastMonth = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), LastMonth);
                model.Growth = (Convert.ToDecimal(model.Cumulative) - Convert.ToDecimal(model.LastMPSD)).ToString("0.##");

                if (model.LastMPSD == "0")
                {
                    model.GrowthPercentage = "0";
                }
                else
                {
                    model.GrowthPercentage = (Convert.ToDecimal(model.Growth) * 100 / Convert.ToDecimal(model.LastMPSD)).ToString("0.##");
                }
                return model;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }




        public List<DashboardRegionalPrescriptionBEO> GetGridData()
        {
            string Qry = "Select 'r002' Region_Code,'Regon' Region_Name,'p002' Product_Code,'Product' Product_Name,'125' Today_Prescription,'15' Cumulative," +
                "'5' Achievement,'10' Last_MPSD,'2' Growth,'2' Growth_Percentage,'10' Existing_MPO,'5' Sending_MPO  from Dual";


            DataTable dt = dbHelper.ReturnCursorF1(dbConnection.SAConnStrReader("Sales"), "FN_REGION_WISE_XELPRO_SUM", "P_DATE ", "07-Apr-2029");


            List<DashboardRegionalPrescriptionBEO> item;
            item = (from DataRow row in dt.Rows
                    select new DashboardRegionalPrescriptionBEO
                    {
                        RegionCode = row["Region_Code"].ToString(),
                        RegionName = row["Region_Name"].ToString(),
                        ProductName = row["XELPRO_COMMITMENT"].ToString(),
                        TodayPrescription = row["TODAY_PRES"].ToString(),
                        Cumulative = row["CUMMULATIVE_PRES"].ToString(),
                        Achievement = row["Achievement"].ToString(),
                        LastMPSD = row["LAST_MSDP"].ToString(),
                        Growth = row["growth"].ToString(),
                        HonorariumAmount = row["HONORARIUM_AMOUNT"].ToString(),
                        ExistingMPO = row["NO_OF_MIO"].ToString(),
                        SendingMPO = row["NO_OF_SEND_MIO"].ToString(),

                    }).ToList();
            return item;
        }
    }
}
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
    public class DashboardDoctorPrescriptionDAO
    {

        DBHelper dbHelper = new DBHelper();
        DBConnection dbConnection = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
        string CntMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.CurrentCulture);
        string LastMonthYear = DateTime.Now.AddMonths(-1).ToString("MM-yyyy", CultureInfo.CurrentCulture);

        public DashboardDoctorPrescriptionBEO GetDoctorPrescriptionData()
        {
            DashboardDoctorPrescriptionBEO model = new DashboardDoctorPrescriptionBEO();
            try
            {
                string ProductNameQry =
                    "Select NVL(SUM(PRACTICING_DAY*PRESCRIPTION_PER_DAY),0) Total  from FSM_DOC_HONORARIUM ";
                string TodayPrescriptionQry =
                    "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where TO_CHAR(SET_DATE,'dd-mm-yyyy')='" +
                    CntDate + "' and DOCTOR_CODE IN (Select distinct DOCTOR_CODE from FSM_DOC_HONORARIUM) ";
                string CumulativeQry =
                    "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where To_Date(SET_DATE,'dd-mm-yyyy')<=To_Date('" +
                    CntDate + "','dd-mm-yyyy') and DOCTOR_CODE IN (Select distinct DOCTOR_CODE from FSM_DOC_HONORARIUM)";
                string LastMPSD =
                    "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where To_Date(SET_DATE,'dd-mm-yyyy') Between To_Date('" +
                    "01-" + LastMonthYear + "','dd-mm-yyyy') AND To_Date('" + CntDate +
                    "','dd-mm-yyyy') and DOCTOR_CODE IN (Select distinct DOCTOR_CODE from FSM_DOC_HONORARIUM)";
                string LastMonth =
                    "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where TO_CHAR(SET_DATE,'MM-YYYY')='" +
                    LastMonthYear + "' and DOCTOR_CODE IN (Select distinct DOCTOR_CODE from FSM_DOC_HONORARIUM)";

                model.ProductName = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), ProductNameQry);
                model.TodayPrescription = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), TodayPrescriptionQry);
                model.Cumulative = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), CumulativeQry);
                model.Achievement = model.ProductName == "0"
                    ? "0"
                    : (Convert.ToDecimal(model.Cumulative) * 100 / Convert.ToDecimal(model.ProductName)).ToString("0.##");
                model.LastMPSD = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), LastMPSD);
                model.LastMonth = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), LastMonth);
                model.Growth = (Convert.ToDecimal(model.Cumulative) - Convert.ToDecimal(model.LastMPSD)).ToString("0.##");
                if (model.LastMPSD == "0")
                {
                    model.GrowthPercentage = "0";
                }
                else
                {
                    model.GrowthPercentage =
                        (Convert.ToDecimal(model.Growth) * 100 / Convert.ToDecimal(model.LastMPSD)).ToString("0.##");
                    ;
                }

                return model;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<DashboardDoctorPrescriptionBEO> GetGridData()
        {
            string Qry = "Select 'd002' Doctor_Code,'Doctor' Doctor_Name,'10' Daily_Product,'10' LastDay_Prescription,'125' Monthly_Product,'15' Total," +
                 "'5' Monthly_Deficit,'10' Achievement,'2' Honorarium_Amt,'Territory' Terrirory_Name,'Area' Area_Name  from Dual";


            DataTable dt = dbHelper.ReturnCursorF1(dbConnection.SAConnStrReader("Sales"), "FN_DOCTOR_WISE_XELPRO_SUM", "P_DATE ", "07-Apr-2029");


            List<DashboardDoctorPrescriptionBEO> item;
            item = (from DataRow row in dt.Rows
                    select new DashboardDoctorPrescriptionBEO
                    {
                        DoctorCode = row["Doctor_Code"].ToString(),
                        DoctorName = row["Doctor_Name"].ToString(),
                        DailyProduct = row["XELPRO_DAILY_COMMIT"].ToString(),
                        LastDayPrescription = row["LAST_DAY_PRES"].ToString(),
                        MonthlyProduct = row["MONTHLY_COMMIT"].ToString(),
                        Total = row["CUMMULATIVE_PRES"].ToString(),
                        MonthlyDeficit = row["MONTHLY_DEFICLT"].ToString(),
                        Achievement = row["ACHIEVEMENT"].ToString(),
                        HonorariumAmt = row["HONORARIUM_AMOUNT"].ToString(),
                        TerriroryName = row["TERRITORY_NAME"].ToString(),
                        AreaName = row["Area_Name"].ToString()
                    }).ToList();
            return item;
        }
    }
}
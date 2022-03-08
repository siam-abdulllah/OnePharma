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
    public class DashboardNationalPrescriptionDAO
    {
       
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConnection = new DBConnection();

        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
        string CntMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.CurrentCulture);
        string LastMonthYear = DateTime.Now.AddMonths(-1).ToString("MM-yyyy", CultureInfo.CurrentCulture);

   
        public DashboardNationalPrescriptionBEO GetNationalPrescriptionData()
        {
            DashboardNationalPrescriptionBEO model = new DashboardNationalPrescriptionBEO();

            string MPOTargetQry = "Select SUM(PRESCRIPTION_QTY) PRESCRIPTION_QTY from FSM_PRESCRIPTION_TARGET";
            string TodayPrescriptionQry = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where TO_CHAR(SET_DATE,'dd-mm-yyyy')='" + CntDate + "'";

            string CumulativeQry = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where To_Date(SET_DATE,'dd-mm-yyyy')<=To_Date('" + CntDate + "','dd-mm-yyyy')";
            string LastMPSD = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where To_Date(SET_DATE,'dd-mm-yyyy') Between To_Date('" + "01-" + LastMonthYear + "','dd-mm-yyyy') AND To_Date('" + CntDate + "','dd-mm-yyyy') ";
            string LastMonth = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where TO_CHAR(SET_DATE,'MM-YYYY')='" + LastMonthYear + "'";

      
      
           
            model.MPOTarget = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), MPOTargetQry);
            model.TodayPrescription = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), TodayPrescriptionQry);
            model.Cumulative = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), CumulativeQry); ;
            model.Achievement = (Convert.ToDecimal(model.Cumulative) * 100 / Convert.ToDecimal(model.MPOTarget)).ToString("0.##");


            model.LastMPSD = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), LastMPSD); ;
            model.LastMonth = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), LastMonth); ;
            model.Growth = (Convert.ToDecimal(model.Cumulative) - Convert.ToDecimal(model.LastMPSD)).ToString("0.##");
            if (model.LastMPSD == "0")
            {
                model.GrowthPercentage = "0";
            }
            else
            {
                model.GrowthPercentage = (Convert.ToDecimal(model.Growth) * 100 / Convert.ToDecimal(model.LastMPSD)).ToString("0.##"); ;
            }

            //Monthly

            string TotalMPOQry = "Select Count(distinct MIO_CODE) from VW_PAL_FIELD_FORCE_CUST_ESO";
            string CumulativeSenderMPO = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where TO_CHAR(SET_DATE,'MM-YYYY')='" + CntMonthYear + "'";

            string LastMPSDSenderMPO = "Select COUNT(MST_SL) PRESCRIPTION_QTY from FSM_PRESCRIPTION_MST Where TO_CHAR(SET_DATE,'MM-YYYY')='" + LastMonthYear + "'";
            model.TotalMPO = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), TotalMPOQry);
            model.CumulativeSenderMPO = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), CumulativeSenderMPO);
            model.LastMPSDSenderMPO = dbHelper.GetValue(dbConnection.SAConnStrReader("Sales"), LastMPSDSenderMPO);
            model.NoOfWhoDidntSend = (Convert.ToDecimal(model.TotalMPO) - Convert.ToDecimal(model.CumulativeSenderMPO)).ToString("0.##");       
       

            return model;
        }

        public List<DashboardNationalPrescriptionBEO> GetGridData()
        {
            string Qry = "Select 'r002' Region_Code,'Regon' Region_Name,'100' Total_MPO,'50' NoOf_Prescription,'125' Today_Prescription,'15' Region_Wise_Target," +
                "'2' Current_Month_Prescription,'5' Achievement,'10' Last_MPSD,'2' Deficit  from Dual";


            DataTable dt = dbHelper.ReturnCursorF1(dbConnection.SAConnStrReader("Sales"), "FN_REGION_WISE_PRES_SUM", "P_DATE ", "07-Apr-2029");



            List<DashboardNationalPrescriptionBEO> item;
            item = (from DataRow row in dt.Rows
                    select new DashboardNationalPrescriptionBEO
                    {
                        RegionCode = row["Region_Code"].ToString(),
                        RegionName = row["Region_Name"].ToString(),

                        TotalMPO = row["TOTAL_EMPLOYEE"].ToString(),
                        NoOfPrescription = row["NO_OF_PRES"].ToString(),
                        TodayPrescription = row["TODAY_PRES"].ToString(),
                        RegionWiseTarget = row["REGION_WISE_TARGET"].ToString(),

                        CurrentMonthPrescription = row["CURRENT_MONTH_TOT_PRES"].ToString(),


                        LastMPSD = row["PREV_MONTH_TOT_PRES"].ToString(),
                        Achievement = row["Achievement"].ToString(),
                        Deficit = row["Deficit"].ToString(),

                    }).ToList();
            return item;
        }
    }
}
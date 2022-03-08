using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.BEL
{
        
    public class DashboardDoctorPrescriptionBEO
    {


        public string DoctorCode { get; set; }

        public string DoctorName { get; set; }

        public string ProductName { get; set; }

        public string TodayPrescription { get; set; }

        public string Cumulative { get; set; }

        public string Achievement { get; set; }

        public string LastMPSD { get; set; }

        public string Growth { get; set; }

        public string GrowthPercentage { get; set; }

        public string LastMonth { get; set; }

        public string DailyProduct { get; set; }

        public string LastDayPrescription { get; set; }

        public string MonthlyProduct { get; set; }

        public string Total { get; set; }

        public string MonthlyDeficit { get; set; }

        public string TerriroryName { get; set; }

        public string AreaName { get; set; }

        public string HonorariumAmt { get; set; }
    }
}
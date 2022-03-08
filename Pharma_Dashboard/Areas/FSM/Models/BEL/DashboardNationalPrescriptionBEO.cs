using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.BEL
{
    public class DashboardNationalPrescriptionBEO
    {
        public string MPOTarget { get; set; }

        public string TodayPrescription { get; set; }

        public string Cumulative { get; set; }

        public string Achievement { get; set; }

        public string LastMPSD { get; set; }

        public string LastMonth { get; set; }

        public string Growth { get; set; }

        public string GrowthPercentage { get; set; }

        public string TotalMPO { get; set; }

        public string CumulativeSenderMPO { get; set; }

        public string LastMPSDSenderMPO { get; set; }

        public string NoOfWhoDidntSend { get; set; }

        public string RegionCode { get; set; }

        public string RegionName { get; set; }

        public string NoOfPrescription { get; set; }

        public string RegionWiseTarget { get; set; }

        public string CurrentMonthPrescription { get; set; }

        public string Deficit { get; set; }
    }
}
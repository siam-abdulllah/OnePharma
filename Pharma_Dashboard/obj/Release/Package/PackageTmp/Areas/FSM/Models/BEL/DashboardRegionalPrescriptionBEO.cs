using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.BEL
{
    public class DashboardRegionalPrescriptionBEO
    {
        public virtual ICollection<DashboardRegionalPrescriptionBEO> ItemList { get; set; }
        public string RegionCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }   
        public string RegionName { get; set; }
        public string Cumulative { get; set; }
        public string Achievement { get; set; }
        public string LastMPSD { get; set; }
        public string Growth { get; set; }
        public string GrowthPercentage { get; set; }
        public string TodayPrescription { get; set; }
        public string ExistingMPO { get; set; }
        public string SendingMPO { get; set; }

        public string LastMonth { get; set; }

        public string HonorariumAmount { get; set; }
    }
}
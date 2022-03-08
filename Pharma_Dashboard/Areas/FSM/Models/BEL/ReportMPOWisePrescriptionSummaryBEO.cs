using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.BEL.BEO
{
    public class ReportMPOWisePrescriptionSummaryBEO
    {

        public int SL_NO { get; set; }
        public string USER_NAME { get; internal set; }
        public string REGION_NAME { get; internal set; }
        public string AREA_NAME { get; internal set; }
        public string TERRITORY_NAME { get; internal set; }
        public string NO_OF_OTHER_PRES { get; internal set; }
        public string TOTAL_PRES { get; internal set; }
        public string TOTAL_PRODUCT { get; internal set; }
        public string TOTAL_XELPRO { get; internal set; }
        public string TOTAL_XELPRO_MUPS { get; internal set; }
        public string TOTAL_CARDOTEL { get; internal set; }
        public string TOTAL_FUXTIL { get; internal set; }
        public string TOTAL_OTHERS { get; internal set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.BEL.BEO
{
    public class ReportRegionWiseDoctorIncentiveBEO
    {

        public string MST_SL { get; internal set; }
        public string USER_ID { get; internal set; }
        public string MIO_NAME { get; internal set; }
        public string DESIG_NAME { get; internal set; }
        public string TERRITORY_NAME { get; internal set; }
        public string PRESCRIPTION_QTY { get; internal set; }
        public int SL_NO { get; set; }
        public string DOCTOR_NAME { get; set; }
        public string DEGREES { get; set; }
        public string ADDRESS { get; set; }
        public string AREA_NAME { get; set; }
        public string REGION_NAME { get; set; }
        public string MIO_INCENTIVE { get; set; }
        public string MIO_DESIGNATION_NAME { get; set; }
        public string TYPE { get; set; }
        public string TOTAL_PRESCRIPTION { get; set; }
    }
    }

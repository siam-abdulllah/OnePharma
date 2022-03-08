using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.BEL.BEO
{
    public class HonorariumDoctorDataUploadBEO
    {
    }

    public class HonorariumDoctorDataUploadInfo
    {
            public string DOCTOR_CODE { get; set; }
            public string DOCTOR_NAME { get; set; }
            public string PRACTICING_DAY { get; set; }
            public string PRESCRIPTION_PER_DAY { get; set; }
            public string HONORARIUM_AMOUNT { get; set; }
            public string TERRITORY_CODE_4P { get; set; }
            public string SET_DATE { get; set; }
            public int SL_NO { get; set; }
    }
    }

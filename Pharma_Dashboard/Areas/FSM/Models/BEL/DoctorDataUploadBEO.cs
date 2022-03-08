using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.BEL.BEO
{
    public class DoctorDataUploadBEO
    {
    }

    public class DoctorDataUploadInfo
        {
            public string DOCTOR_CODE { get; set; }
            public string DOCTOR_NAME { get; set; }
            //public string DOCTOR_CODE_4P { get; set; }
            public string CLASS_GROUP { get; set; }
            public string ADDRESS { get; set; }
            public string DEGREES { get; set; }
            public string DESIGNATION { get; set; }
            public string CONTRACT_NO { get; set; }
            public string EMAIL { get; set; }
            
            public string TERRITORY_CODE_4P { get; set; }
            public string SPECIALTY { get; set; }
            public int SL_NO { get; set; }
        }
    }

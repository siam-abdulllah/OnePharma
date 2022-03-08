using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Universal.BEL
{
    public class LoginRegistrationBEL
    {
        public string USER_NAME { get; set; }
        public string EMPLOYEE_CODE { get; set; }
        public string USER_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string CODE { get; set; }
        public string ACCESS_LEVEL { get; set; }
        public string ROLE_NAME { get; set; }
        public string PASSWORD { get; set; }
        public string GROUP_CODE { get; internal set; }
    }
}
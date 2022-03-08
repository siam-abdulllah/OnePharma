using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Security.Models.BEL
{
    public class SecRoleConf
    {
        public string RC_ID { get; set; }
        public string RL_ID { get; set; }
        public string USER_ID { get; set; }
    }
    public class EmployeeInfo
    {
        public string UserID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationDetail { get; set; }
    }
}
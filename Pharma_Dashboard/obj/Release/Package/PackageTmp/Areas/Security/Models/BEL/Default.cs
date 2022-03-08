using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Security.Models.BEL
{
    public class MenuHead
    {
        public int MH_ID { get; set; }
        public string MH_SEQ { get; set; }
        public string MH_NAME { get; set; }
        public string MH_CSS_CLASS { get; set; }
        public SubMenu SubMenu { get; set; }
        public int RL_ID { get; set; }
    }
    
    public class SubMenu 
{
        public int SM_ID { get; set; }
        public string SM_SEQ { get; set; }
        public string SM_NAME { get; set; }
        public string URL { get; set; }
        public string SM_CSS_CLASS { get; set; }
        public int MH_ID { get; set; }
    }
    public class EventPermission
    {
        public int MH_ID { get; set; }
        public string MH_NAME { get; set; }
        public string SM_NAME { get; set; }
        public string SV { get; set; }
        public string VW { get; set; }
        public string DL { get; set; }
    }

    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string LogId { get; set; }
        public string Statua { get; set; }
        public string AccessLevel { get; set; }
        public string EmployeeCode { get; set; }
        public string GroupCode { get; set; }

    }
}
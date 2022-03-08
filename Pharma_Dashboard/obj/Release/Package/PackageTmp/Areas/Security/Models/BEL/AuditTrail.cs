using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Security.Models.BEL
{
    public class AuditTrail
    {
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Terminal { get; set; }
        public string Action_Date { get; set; }
        public string Activity_Type { get; set; }
        public string Action_Form { get; set; }
        public string Action_Table { get; set; }
        public string Transaction_ID { get; set; }
    }
}
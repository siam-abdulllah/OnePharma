using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Universal.Gateway
{
    public class ReturnData
    {
        public string MstID { get; set; }
        public string MaxID { get; set; }
        public string MaxCode { get; set; }
        public string IUMode { get; set; }
        public string MSG { get; set; }
        public string ExceptionReturn { get; set; }
        public object ListReturn { get; set; }
    }
}
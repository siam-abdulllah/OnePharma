using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Reports.Models.BEl
{
    public class DepotPPMCurrentStock
    {
        public string SL_No { get; set; }
        public string STOCK_DATE { get; set; }
        public string DEPOT_CODE { get; set; }
        public string DEPOT_NAME { get; set; }
        public string PPM_CODE { get; set; }
        public string PPM_NAME { get; set; }
        public string PPM_TYPE { get; set; }
        public string PPM_TYPE_NAME { get; set; }
        public string PACK_SIZE { get; set; }
        public double STOCK_QTY { get; set; }
    }
}
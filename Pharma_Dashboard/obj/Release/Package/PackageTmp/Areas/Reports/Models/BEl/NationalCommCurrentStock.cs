using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Reports.Models.BEl
{
    public class NationalCommCurrentStock
    {
        public string SL_No { get; set; }
        public string STOCK_DATE { get; set; }
        public string DEPOT_CODE { get; set; }
        public string DEPOT_NAME { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PACK_SIZE { get; set; }
        public string UNIT_TP { get; set; }
        public string UNIT_VAT { get; set; }
        public double FRESH_STOCK_QTY { get; set; }
        public double DAMAGE_STOCK_QTY { get; set; }
        public double FRESH_STOCK_TP_VAL { get; set; }
        public double FRESH_STOCK_VAT_VAL { get; set; }
        public double FRESH_STOCK_TP_VAT_VAL { get; set; }
    }
}
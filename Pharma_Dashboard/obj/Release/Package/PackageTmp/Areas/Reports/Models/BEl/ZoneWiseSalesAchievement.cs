using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Reports.Models.BEl
{
    public class ZoneWiseSalesAchievementProduct
    {
        public string SL_No { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
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
    public class ZoneWiseSalesAchievementValue 
    {
        public string SL_No { get; set; }
        public string DSM_CODE { get; set; }
        public string DSM_NAME { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public string TARGET_AMT { get; set; }
        public string TO_DAY_SALES { get; set; }
        public string UPTO_SALES { get; set; }
        public string ACH { get; set; }
        public string LM_UPTO_SALES { get; set; }
        public string GROWTH { get; set; }
        public string CM_MPO { get; set; }
        public string LM_MPO { get; set; }
        public string CM_CUST { get; set; }
        public string LM_CUST { get; set; }
        public string TO_DAY_COLLECTION { get; internal set; }
        public string UPTO_COLLECTION { get; internal set; }
    }
}
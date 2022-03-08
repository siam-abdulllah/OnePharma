using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Reports.Models.BEl
{
    public class ProdWiseSalesAchievement
    {
        public string SL_No { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PACK_SIZE { get; set; }
        public string TO_DAY_SALES_BOX { get; set; }
        public string TO_DAY_SALES_VALUE { get; set; }
        public string CM_UPTO_SALES_BOX { get; set; }
        public string CM_UPTO_SALES_VALUE { get; set; }
        public string LM_UPTO_SALES_BOX { get; set; }
        public string LM_UPTO_SALES_VALUE { get; set; }
        public string GROWTH_BOX { get; set; }
        public string GROWTH_VALUE { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.FSM.Models.BEL.BEO
{
    public class DefaultBEO
    {
    }

    public class DepotInfoBEO
    {
            public string DEPOT_CODE { get; set; }
            public string DEPOT_NAME { get; set; }
        }
 public class MonthInfoBEO
{
            public string MONTH_CODE { get; set; }
            public string MONTH_NAME { get; set; }
        }

        public class ZoneInfoBEO
        {
            public string ZONE_CODE { get; set; }
            public string ZONE_NAME { get; set; }
        }

        public class RegionInfoBEO
        {

            public string DSM_CODE { get; set; }
            public string DSM_NAME { get; set; }
            public string REGION_CODE { get; set; }
            public string REGION_NAME { get; set; }
            public string RSM_CODE { get; set; }
            public string RSM_NAME { get; set; }

        }

        public class AreaInfoBEO
        {
            public string AREA_CODE { get; set; }
            public string AREA_NAME { get; set; }
            public string AM_CODE { get; set; }
            public string AM_NAME { get; set; }
        }

        public class TerritoryInfoBEO
        {
            public string TERRITORY_CODE { get; set; }
            public string TERRITORY_NAME { get; set; }
            public string MIO_CODE { get; set; }
            public string MIO_NAME { get; set; }
        }
        public class MPOInfoBEO
        {
            
            public string MIO_CODE { get; set; }
            public string MIO_NAME { get; set; }
        }
    }

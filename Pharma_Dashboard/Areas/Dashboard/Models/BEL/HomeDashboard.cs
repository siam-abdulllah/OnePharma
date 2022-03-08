using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAsia_Dashboard.Areas.Dashboard.Models.BEL
{
    public class Target
    {
        public string Target_Current_Month { get; set; }
        public string Target_Current_Month_Name { get; set; }
    }
    public class TodaySale
    {
        public string Today_Sales { get; set; }
        public string Today_Sales_CA { get; set; }
        public string Today_Sales_CR { get; set; }
    }
    public class UpToMonthSale
    {
        public string UpTo_Month_Total_Sales { get; set; }
        public string UpTo_Month_Total_Sales_CA { get; set; }
        public string UpTo_Month_Total_Sales_CR { get; set; }
    }
    public class LMUpToDate
    {
        public string LM_UP_ToDate_Total_Sales { get; set; }
        public string LM_UP_ToDate_Total_Sales_CA { get; set; }
        public string LM_UP_ToDate_Total_Sales_CR { get; set; }
    }
    public class TodayReturn
    {
        public string Today_Return { get; set; }
        public string Today_Return_CA { get; set; }
        public string Today_Return_CR { get; set; }
    }
    public class UpToMonthReturn
    {
        public string UpTo_Month_Total_Return { get; set; }
        public string UpTo_Month_Total_Return_CA { get; set; }
        public string UpTo_Month_Total_Return_CR { get; set; }
    }

    
    public class TotalMPO
    {
        public string CM_Total_MPO { get; set; }
        public string LM_Total_MPO { get; set; }
        public string TOTAL_ACTIVE_MPO { get; set; }
    }
    public class TotalDue
    {
        public string Total_Due { get; set; }
        public string Total_Due_CA { get; set; }
        public string Total_Due_CR { get; set; }
    }public class MaturedDue
    {
        public string Matured_Dues { get; set; }
        public string Matured_Dues_CA { get; set; }
        public string Matured_Dues_CR { get; set; }
    }
    public class ActiveMaturedDue
    {
        public string Active_Matured_Dues { get; set; }
        public string Active_Matured_Dues_CA { get; set; }
        public string Active_Matured_Dues_CR { get; set; }
    }
    public class DiscontinueMaturedDue
    {
        public string Discontinue_Matured_Dues { get; set; }
        public string Discontinue_Matured_Dues_CA { get; set; }
        public string Discontinue_Matured_Dues_CR { get; set; }
    }
    // ReSharper disable once IdentifierTypo
    public class ImmaturedDue
    {
        public string Immatured_Dues { get; set; }
        public string Immatured_Dues_CA { get; set; }
        public string Immatured_Dues_CR { get; set; }
    }
    public class TotalCustomer
    {
        public string CM_Total_Customer { get; set; }
        public string LM_Total_Customer { get; set; }
    }
    public class ProductValueSale
    {
        public string CM_Product_Value_Sales { get; set; }
        public string LM_Product_Value_Sales { get; set; }
    }
    public class DCCTotalSale
    {
        public string DCC_TOTAL_SALES_CM { get; set; }
        public string DCC_TOTAL_SALES_LM { get; set; }
    }

    public class HomeDashboard
    {
        public TodaySale TodaySale { get; set; }
        public TodayReturn TodayReturn { get; set; }
        public UpToMonthSale UpToMonthSale { get; set; }
        public UpToMonthReturn UpToMonthReturn { get; set; }
        public Target Target { get; set; }
        public TotalMPO TotalMpo { get; set; }
        public MaturedDue MaturedDue { get; set; }
        public ActiveMaturedDue ActiveMaturedDue { get; set; }
        public DiscontinueMaturedDue DiscontinueMaturedDue { get; set; }
        public ImmaturedDue ImmaturedDue { get; set; }
        public TotalCustomer TotalCustomer { get; set; }
        public ProductValueSale ProductValueSale { get; set; }
        public DCCTotalSale DCCTotalSale { get; set; }
        public string Achievement { get; set; }
        public LMUpToDate LMUpToDate { get; set; }
        
        public string Growth { get; set; }
        public string Today_Collection_Amount { get; set; }
        public string UpTo_Month_Collection { get; set; }
        public string LM_UpTo_Month_Collection { get; set; }
        
        public string Commercial_Stock_Valuation { get; set; }
        public string Sample_Stock_Valuation { get; set; }
        public string PPM_Stock_Valuation { get; set; }
        public string Gift_Stock_Valuation { get; set; }
        public double Total_Stock_Valuation { get; set; }
        
        public string ACCESS_LEVEL { get; set; }
        public DashboardChart DashboardCharts { get; set; }
        public ProdSale C0165Sale { get; set; }
        public ProdSale C0166Sale { get; set; }
        public MPOCreditLimit MPOCreditLimit { get; set; }
        public WorldCupOffer WorldCupOffer { get; set; }
        public OldCollection OldCollection { get; set; }
        public TotalDue TotalDues { get; internal set; }
        public string LM_Collection_Amount { get; internal set; }
    }
    public class DashboardChart
    {
        public string Level { get; set; }
        public string Data { get; set; }
        public string Color { get; set; }
        public string BaloonText { get; set; }
    }
    public class ProdSale
    {
        public string PROD_TOTAL_QTY;
        public string PROD_TOTAL_VALUE;
    }
    public class WorldCupOffer
    {
        public string NET_ISSUED_QTY;
        public string NET_INV_VALUE;
    }
    public class MPOCreditLimit
    {
        public string TOTAL_LIMIT_AMOUNT;
    }
    public class OldCollection
    {
        public string Total_Old_Dues { get; set; }
        public string Total_UpTo_Month_Collection { get; set; }
    }
}
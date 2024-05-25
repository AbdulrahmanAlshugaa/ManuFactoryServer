using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{

    public partial class Sales_SalesOrderMaster
    {
        public int OrderID { get; set; }
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        public string CustomerMobile { get; set; }
        public string InvoiceDate { get; set; } 
        public double CustomerID { get; set; } 
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyPrice { get; set; }
        public decimal CurrencyEquivalent { get; set; }
        public int CostCenterID { get; set; }
        public int SellerID { get; set; }
        public double StoreID { get; set; }
        public double DelegateID { get; set; }
        public string Notes { get; set; }
        public decimal DiscountOnTotal { get; set; } 
         

        public int FromCashierScreen { get; set; }
        public int CloseCashier { get; set; }
        public double CloseCashierDate { get; set; }
        public string NetProcessID { get; set; }
        public string CheckID { get; set; }
        public double CheckAccount { get; set; }

        public string CheckSpendDate { get; set; }
        public string WarningDate { get; set; }
        public string ReceiveDate { get; set; }
         
        public string OrderType { get; set; }
        public int SectionID { get; set; }
        public int TableID { get; set; }
        public int NeedReview { get; set; }
        public string ReviewType { get; set; }
        public int IsSendReview { get; set; }
        public string WorkDetails { get; set; }
        public string Status { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; }
        public int Posted { get; set; }
        public double EmployeeID { get; set; }
        public double PateintID { get; set; }
        public double TempInvoiceID { get; set; }
        public double EnduranceRatio { get; set; }  
        public int RegistrationNo { get; set; }
        public Nullable<double> InsuranceAmmount { get; set; }
        public string VATID { get; set; }
        public double NetType { get; set; }
        public decimal AdditionaAmountTotal { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal RemaindAmount { get; set; }


        public string CostCenterName { get; set; }
        public string StoreName { get; set; }
        public string CustomerName { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<double> Vat { get; set; }
        public Nullable<double> Descount { get; set; }
        public Nullable<double> Net { get; set; }
        public string SaleMethod { get; set; }
        public Nullable<decimal> NetBalance { get; set; }
        public Nullable<decimal> InvoiceGoldTotal { get; set; }
        public Nullable<decimal> InvoiceEquivalenTotal { get; set; }

        public double EquivalentTotal { get; set; }
        public double GoldTotal { get; set; }
        public double DiamondTotal { get; set; }
        public double WeightDiamondTotal { get; set; }
        public double WeightGoldTotal { get; set; }
        public double WeightBagetTotal { get; set; }
        public double WeightStoneTotal { get; set; }


        public List<Sales_SalesOrderDetails> SaleDatails { get; set; }

        public string OperationTypeName { get; set; }

        public byte[] pic { get; set; }
        public int GoldUsing { get; set; }
        public int TypeGold { get; set; } 

    }
}

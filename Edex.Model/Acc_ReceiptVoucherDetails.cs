//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Edex.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Acc_ReceiptVoucherDetails
    {

        public string Barcode { get; set; }
        public string ItemName { get; set; }
        public int Calipar { get; set; }
     

        public int ID { get; set; }
        public int ReceiptVoucherID { get; set; }
        public int BranchID { get; set; }
        public int FACILITYID { get; set; }
        public double AccountID { get; set; }
        public string ArbAccountName { get; set; }
        public string EngAccountName { get; set; }
        public double CreditAmount { get; set; }
        public double Discount { get; set; }
        public string Declaration { get; set; }
        public int CostCenterID { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyPrice { get; set; }
        public double CurrencyEquivalent { get; set; }
        public decimal WeightGold { get; set; }
        public decimal QtyGoldEqulivent { get; set; }
        public Acc_ReceiptVoucherMaster ReceiptVoucherMaster { get; set; }



    }
}

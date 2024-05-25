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
    
    public partial class Acc_VariousVoucherMaster
    {
        public int VoucherID { get; set; }
        public int BranchID { get; set; }
        public string VoucherDate { get; set; }
        public int DocumentType { get; set; }
        public int DocumentID { get; set; }
        public string Notes { get; set; }
        public int UserID { get; set; }
        public double TotalCredit { get; set; }

        public double TotalDebit { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; }
        public int Posted { get; set; }
        public Nullable<int> DelegateID { get; set; }
        public Nullable<int> CanUpdate { get; set; }
        public Nullable<int> IsExpens { get; set; }
        public double RegistrationNo { get; set; }

        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyPrice { get; set; }
        public decimal CurrencyEquivalent { get; set; }
        public int FacilityID { get; set; }

        public string OperationTypeName { get; set; }
        public List<Acc_VariousVoucherDetails> VariousVoucherDetails { get; set; }


        public int TypeOpration { get; set; }
    }

    public partial class Acc_VariousVoucherMachinMaster
    {
        public int VoucherID { get; set; }
        public int BranchID { get; set; }
        public string VoucherDate { get; set; }
        public int DocumentType { get; set; }
        public int DocumentID { get; set; }
        public string Notes { get; set; }
        public int UserID { get; set; }
        public double TotalCredit { get; set; }

        public double TotalDebit { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; }
        public int Posted { get; set; }
        public Nullable<int> DelegateID { get; set; }
        public Nullable<int> CanUpdate { get; set; }
        public Nullable<int> IsExpens { get; set; }
        public double RegistrationNo { get; set; }

      
        public int FacilityID { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyPrice { get; set; }
        public decimal CurrencyEquivalent { get; set; }
        public string OperationTypeName { get; set; }
        public List<Acc_VariousVoucherMachinDetails> VariousVoucherDetails { get; set; }

    }
}

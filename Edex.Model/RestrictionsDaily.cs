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
    
    public partial class RestrictionsDaily
    {


        public int ID { get; set; }
        public double RegistrationNo { get; set; }
        public double TranNo { get; set; }
        public int TransType { get; set; }
        public int BranchNum { get; set; }
        public double RegistrationDate { get; set; }
        public double Master_code { get; set; }
        public double Discount { get; set; }
        public int AccountFinal { get; set; }
        public int CurrencyNum { get; set; }
        public int SellerNum { get; set; }
        public int DelegateNum { get; set; }
        public string DocumentNumber { get; set; }
        public string OperationType { get; set; }
        public string Remark { get; set; }
        public string AccountNumCorresponding { get; set; }
        public string Receivables { get; set; }
        public int posted { get; set; }
        public int Cancel { get; set; }
        public int FacilityID { get; set; }


        public double Debt { get; set; }
        public double Credit { get; set; }
        public double Acc_code { get; set; }
        public string ArbAccountName { get; set; }
        public string EngAccountName { get; set; }
        public string Release { get; set; }
        public int CostCenterNo { get; set; }
        
    }
}
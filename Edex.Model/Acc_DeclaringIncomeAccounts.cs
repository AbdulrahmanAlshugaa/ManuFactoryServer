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
    
    public partial class Acc_DeclaringIncomeAccounts
    {
        public int ID { get; set; }
        public double AccountID { get; set; }
        public string DeclareAccountName { get; set; }
        public string AccountArbName { get; set; }
        public string AccountName { get; set; }
        public string AccountEngName { get; set; }
        public int BranchID { get; set; }
        public int UserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public Nullable<int> FacilityID { get; set; }
    }
}

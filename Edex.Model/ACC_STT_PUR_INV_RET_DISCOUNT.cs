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
    
    public partial class ACC_STT_PUR_INV_RET_DISCOUNT
    {
        public string Declaration { get; set; }
        public double TheDate { get; set; }
        public string RecordType { get; set; }
        public string RecordTypeArb { get; set; }
        public string RecordTypeEng { get; set; }
        public int InvoiceID { get; set; }
        public double RegDate { get; set; }
        public string OppsiteAccountName { get; set; }
        public double DebitAccount { get; set; }
        public int Cancel { get; set; }
        public int BranchID { get; set; }
        public double InvoiceDate { get; set; }
        public Nullable<double> TotalBalance { get; set; }
        public Nullable<double> TotalDiscount { get; set; }
        public double DiscountDebitAccount { get; set; }
        public Nullable<double> AdditionalValue { get; set; }
        public Nullable<double> AdditionalAccount { get; set; }
    }
}

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
    
    public partial class ACC_STT_PUR_INV_DISCOUNT
    {
        public string RecordType { get; set; }
        public string RecordTypeArb { get; set; }
        public string RecordTypeEng { get; set; }
        public Nullable<double> Discount { get; set; }
        public int InvoiceID { get; set; }
        public Nullable<double> TheDate { get; set; }
        public string Declaration { get; set; }
        public string OppsiteAccountName { get; set; }
        public int Debit { get; set; }
        public Nullable<double> DISCOUNTCREDITACCOUNT { get; set; }
        public int BranchID { get; set; }
        public Nullable<double> RegDate { get; set; }
    }
}
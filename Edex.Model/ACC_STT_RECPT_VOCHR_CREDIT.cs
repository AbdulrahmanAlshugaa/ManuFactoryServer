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
    
    public partial class ACC_STT_RECPT_VOCHR_CREDIT
    {
        public string DECLARATION { get; set; }
        public Nullable<double> TheDate { get; set; }
        public string RecordType { get; set; }
        public string RecordTypeArb { get; set; }
        public string RecordTypeEng { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<double> DATECREATED { get; set; }
        public double SumCreditAmount { get; set; }
        public double AccountID { get; set; }
        public string OppsiteAccountName { get; set; }
        public Nullable<int> BRANCHID { get; set; }
    }
}

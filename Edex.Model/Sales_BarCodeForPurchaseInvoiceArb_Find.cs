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
    
    public partial class Sales_BarCodeForPurchaseInvoiceArb_Find
    {
        public string البـاركـود { get; set; }
        public int رقم_المادة { get; set; }
        public string اسـم_الـمـادة { get; set; }
        public int رقم_الوحدة { get; set; }
        public string اسـم_الوحدة { get; set; }
        public string تاريخ_الصلاحية { get; set; }
        public Nullable<int> BranchID { get; set; }
        public double SalePrice { get; set; }
    }
}
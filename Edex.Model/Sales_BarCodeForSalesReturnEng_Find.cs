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
    
    public partial class Sales_BarCodeForSalesReturnEng_Find
    {
        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public int StoreID { get; set; }
        public double Bones { get; set; }
        public double QTY { get; set; }
        public string BarCode { get; set; }
        public int Item_ID { get; set; }
        public string Item_Name { get; set; }
        public int Size_ID { get; set; }
        public string Size_Name { get; set; }
        public string Expiry_Date { get; set; }
        public string Description { get; set; }
        public double Sale_Price { get; set; }
    }
}
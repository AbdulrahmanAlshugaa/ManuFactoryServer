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
    
    public partial class Res_Orders_Details
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int BranchID { get; set; }
        public string BarCode { get; set; }
        public int ItemID { get; set; }
        public int SizeID { get; set; }
        public double QTY { get; set; }
        public double SalePrice { get; set; }
        public double Bones { get; set; }
        public string Description { get; set; }
        public int StoreID { get; set; }
        public double Discount { get; set; }
        public double ExpiryDate { get; set; }
        public double CostPrice { get; set; }
    }
}

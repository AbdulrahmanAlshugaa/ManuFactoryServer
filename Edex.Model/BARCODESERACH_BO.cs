using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edex.Model
{
    public  class BARCODESERACH_BO
    {

        public int ID { get; set; }
        public long ItemID { get; set; }
        public long InvoiceID { get; set; }
      
        public string BarCode { get; set; }
        public string ItemName { get; set; }
        public int SizeID { get; set; }
        public string SizeName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal QTY { get; set; }
        public decimal Bones { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal TheCount { get; set; }
        public string Serials { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Equivalen { get; set; }
        public decimal Caliber { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal AdditionalValue { get; set; }
        public decimal Net { get; set; }
    
        public double BAGET_W { get; set; }
        public double STONE_W { get; set; }
        public double DIAMOND_W { get; set; }
         
       
        public int FacilityID { get; set; }
        public int StoreID { get; set; }
        public int Cancel { get; set; }
        public int ItemStatus { get; set; }
        public int HavVat { get; set; }
        public string Description { get; set; }

      public    BARCODESERACH_BO()
        {

            ID = 0;
            ItemID = 0;
            InvoiceID = 0;
            BarCode = "";
            ItemName = "";
            SizeID = 1;
            SizeName = "";
            QTY = 1;
            Bones = 0;
            Height = 0;
            Width = 0;
            TheCount = 2;
            Serials = "";
            SalePrice = 0;
            CostPrice = 0;
            Equivalen = 0;
            Caliber = 0;
            Total = 0;
            Discount = 0;
            AdditionalValue = 0;
            Net = 0;
            BAGET_W = 0;
            STONE_W = 0;
            DIAMOND_W = 0;
            FacilityID = 1;
            StoreID = 1;
            Cancel = 0;
            ItemStatus = 1;
            HavVat = 0;
            Description = "";
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
     
    public partial class Sales_PurchaseOrderDetails
    {
        public int ID { get; set; }
        public int BranchID { get; set; }
        public long OrderID { get; set; }
        public string BarCode { get; set; }
        public long ItemID { get; set; }
        public string ArbItemName { get; set; }
        public string Color { get; set; }
        public string CLARITY { get; set; }


        public string Description { get; set; }
        public string EngItemName { get; set; }
        public int SizeID { get; set; }

        public double GroupID { get; set; }
        public string ArbGroupName { get; set; }
        public string EngGroupName { get; set; }



        public string ArbSizeName { get; set; }
        public string EngSizeName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal QTY { get; set; }
        public decimal PackingQty { get; set; }
        public decimal Bones { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal TheCount { get; set; }
        public string Serials { get; set; }
        public decimal CostPrice { get; set; }

        public decimal CostPriceUnite { get; set; }
        public decimal SpendPrice { get; set; }


        public decimal Equivalen { get; set; }
        public decimal Caliber { get; set; } 
        public decimal Total { get; set; } 
        public decimal Net { get; set; }
        public double BAGET_W { get; set; }
        public double STONE_W { get; set; }
        public double DIAMOND_W { get; set; }

        public int FacilityID { get; set; }
        public double StoreID { get; set; }
        public int Cancel { get; set; }
        public int ItemStatus { get; set; }
        public bool HavVat { get; set; }

        public decimal RemainQty { get; set; } 
        public long ExpiryDateStr { get; set; }
        public byte[] ItemImage { get; set; }
        public int TypeGold { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyPrice { get; set; }
        public double CurrencyEquivalent { get; set; }
        public Sales_PurchaseOrderMaster PurchaseMaster { get; set; }

        public Sales_PurchaseOrderDetails()
        {

            ID = 0;
            ItemID = 0;
            OrderID = 0;
            BarCode = "";
            EngItemName = "";
            ArbItemName = "";
            SizeID = 1;
            ArbSizeName = "";
            EngSizeName = "";
            QTY = 1;
            Bones = 0;
            Height = 0;
            Width = 0;
            TheCount = 2;
            Serials = ""; 
            CostPrice = 0;
            Equivalen = 0;
            Caliber = 0;
            Total = 0; 
            Net = 0;
            BAGET_W = 0;
            STONE_W = 0;
            DIAMOND_W = 0;
            FacilityID = 1;
            StoreID = 1;
            Cancel = 0;
            ItemStatus = 1;
            HavVat = true;
            Description = "";
            ExpiryDate = DateTime.Now;

        }
    }
}

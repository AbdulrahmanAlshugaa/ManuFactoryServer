using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{

    public class Stc_ItemsInonBail_Details
    {
        public Int32 ID { get; set; }
        public String BarCode { get; set; }

        public Int32 GroupID { get; set; }
        public String ArbGroupName { get; set; }
        public String EngGroupName { get; set; }

        public Int32 ItemID { get; set; }
        public String ArbItemName { get; set; }
        public String EngItemName { get; set; }

        public Int32 SizeID { get; set; }
        public String ArbSizeName { get; set; }
        public String EngSizeName { get; set; }

        public string CLARITY { get; set; }
        public string Color { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Decimal QTY { get; set; }

        public Decimal DIAMOND_W { get; set; }
        public Decimal STONE_W { get; set; }
        public Decimal BAGET_W { get; set; }
        public Decimal Caliber { get; set; }
        public Decimal PackingQty { get; set; }

        public Decimal CostPrice { get; set; }
        public Decimal SalePrice { get; set; }
        public Decimal SpendPrice { get; set; }
        public decimal CaratPrice { get; set; }
        
        public Decimal Total { get; set; }
        public String Description { get; set; }


        public Int32 PageNo { get; set; }

        public Decimal Height { get; set; }
        public Decimal Width { get; set; }

        public long ExpiryDateStr { get; set; }
      
      
        public Int32 TypeID { get; set; }
        public Decimal Equivalen { get; set; }
      
        public Int32 TheCount { get; set; }
        public Int32 InvoiceID { get; set; }
        public Int32 BranchID { get; set; }
        public Int32 FacilityID { get; set; }
        public Decimal Bones { get; set; }
        public Int32 StoreID { get; set; }
        public String Serials { get; set; }
        public Int32 Cancel { get; set; }
        public byte[] ItemImage { get; set; }
       
      
        public Stc_ItemsInonBail_Master ItemsInOnBailMaster { get; set; }
        public Stc_ItemsInonBail_Details()
        {
            ID = 0;
            BarCode = "";
            GroupID = 0;
            ArbGroupName = "";
            EngGroupName = "";

            ItemID = 0;
            ArbItemName = "";
            EngItemName = "";

            SizeID = 0;
            ArbSizeName = "";
            EngSizeName = "";
            ExpiryDate = DateTime.Now;
            QTY = 1;
            SalePrice = 0;
            CostPrice = 0;
            Description = "";
            Total = 0;
            BAGET_W = 0;
            STONE_W = 0;
            DIAMOND_W = 0;
            FacilityID = 1;
            StoreID = 1;
            Cancel = 0;
            Equivalen = 0;
            Caliber = 0;
            Bones = 0;
            Height = 0;
            Width = 0;
            TheCount = 1;
            Serials = "";
            InvoiceID = 0;
            ExpiryDateStr = 0;
        }
    }
     
}

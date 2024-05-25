using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{

    public class Stc_TransferMultipleStoresGold_Details
    {
        public Int32 ID { get; set; }
        public String BarCode { get; set; }

        public double GroupID { get; set; }
        public String ArbGroupName { get; set; }
        public String EngGroupName { get; set; }

        public Int32 ItemID { get; set; }
        public String ArbItemName { get; set; }
        public String EngItemName { get; set; }

        public Int32 SizeID { get; set; }
        public String ArbSizeName { get; set; }
        public String EngSizeName { get; set; }
        public double StoreAccountID { get; set; }
        public string StoreName { get; set; }
    
        public DateTime ExpiryDate { get; set; }
        public Decimal Caliber { get; set; }
        public Decimal QTY { get; set; }

      
        public Decimal PackingQty { get; set; }

        public Decimal CostPrice { get; set; }
        public Decimal SalePrice { get; set; }       
    
        public String Description { get; set; }


        public Int32 PageNo { get; set; }
         

        public long ExpiryDateStr { get; set; }

       
        public Int32 TypeID { get; set; }
        public Decimal CaliberEquivalen { get; set; }
        public Decimal Equivalen { get; set; }
      
        public Int32 TheCount { get; set; }
        public Int32 InvoiceID { get; set; }
        public Int32 BranchID { get; set; }
        public Int32 FacilityID { get; set; }
 
        public String Serials { get; set; }
        public Int32 Cancel { get; set; }
        public byte[] ItemImage { get; set; }
        public decimal TotalCost { get; set; }
        public Stc_TransferMultipleStoresGold_Master  TransferMultipleStoresGoldMaster { get; set; }
        public Stc_TransferMultipleStoresGold_Details()
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
      
          
            FacilityID = 1;
     
            Cancel = 0;
            Equivalen = 0;
             
          
            TheCount = 1;
            Serials = "";
            InvoiceID = 0;
            ExpiryDateStr = 0;
        }
    }
     
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{

    public partial class Sales_PurchaseOrderMaster
    {
        public int OrderID { get; set; }
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        public string InvoiceDate { get; set; } 
        public double SupplierID { get; set; }   
        public double StoreID { get; set; }
        public string Notes { get; set; }    
        public int CurencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyPrice { get; set; }
        public decimal CurrencyEquivalent { get; set; }
        public int GoldUsing { get; set; }


        public double TransportDebitAmount { get; set; }
        public string NetProcessID { get; set; }
        public string CheckID { get; set; }
        public string CheckSpendDate { get; set; }
        public string WarningDate { get; set; }
        public string ReceiveDate { get; set; }

        public byte[] InvoiceImage { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; } 
         


        public List<Sales_PurchaseOrderDetails> PurchaseOrderDatails { get; set; }
         
        public string SupplierName { get; set; }
    
    
        public string StoreName { get; set; }
        public int ItemStatus { get; set; }
        public int PageNo { get; set; }  
        public decimal InvoiceTotal { get; set; }
        public decimal InvoiceGoldTotal { get; set; }
        public decimal InvoiceEquivalenTotal { get; set; }
        public decimal InvoiceDiamondTotal { get; set; }

        public string OperationTypeName { get; set; }


        public string Mobile { get; set; }
        public int TypeGold { get; set; }

    }
}

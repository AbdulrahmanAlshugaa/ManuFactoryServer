 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public class Stc_GoldOutOnBail_Master
    {
        public int InvoiceID { get; set; }
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        public string InvoiceDate { get; set; }
        public int MethodeID { get; set; }
        public double SupplierID { get; set; }
        public int SupplierInvoiceID { get; set; }
        public int CostCenterID { get; set; }
        public double DelegateID { get; set; }
        public double StoreID { get; set; }
        public string Notes { get; set; }
        public double DiscountOnTotal { get; set; }
        public double DebitAccount { get; set; }
        public double CreditAccount { get; set; }
             
        public int CurencyID { get; set; }    
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
        public int Posted { get; set; }
  public int RegistrationNo { get; set; }

  public List<Stc_GoldOutOnBail_Details> GoldOutOnBailDatails { get; set; }
        public string Method { get; set; }

        public string SupplierName { get; set; }
        public string CostCenterName { get; set; }
        public string StoreName { get; set; }
        public Int32 CurrencyID { get; set; } 
        public Decimal InvoiceEquivalenTotal { get; set; }
        public Decimal InvoiceTotalGold { get; set; }
        public string OperationTypeName { get; set; }
        public byte[] ItemImage { get; set; }
    }
}


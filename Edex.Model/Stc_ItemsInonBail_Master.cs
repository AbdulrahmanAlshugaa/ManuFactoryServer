using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
   public  class Stc_ItemsInonBail_Master
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
        public int StoreID { get; set; }
        public string Notes { get; set; }
        public double DiscountOnTotal { get; set; }
        public double DebitAccount { get; set; }
        public double CreditAccount { get; set; }
        public double DiscountCreditAccount { get; set; }
        public double TransportDebitAccount { get; set; }
        public double CheckAccount { get; set; }
        public double AdditionalAccount { get; set; }
         public double DebitGoldAccount { get; set; }


        public double NetAccount { get; set; }
        public double NetAmount { get; set; }
        public decimal Total { get; set; }
        public decimal Net { get; set; }

        public int CurencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyPrice { get; set; }
        public decimal CurrencyEquivalent { get; set; }
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
        public int Posted { get; set; }
  public int RegistrationNo { get; set; }

        public List<Stc_ItemsInonBail_Details> ItemsInOnBailDatails { get; set; }
        public string Method { get; set; }

        public string SupplierName { get; set; }
        public string CostCenterName { get; set; }
        public string StoreName { get; set; }


      
        public Decimal InvoiceTotal { get; set; }
        public Decimal InvoiceEquivalenTotal { get; set; }
        public Decimal InvoiceTotalGold { get; set; }
        public string OperationTypeName { get; set; }
        public byte[] ItemImage { get; set; }
    }
}

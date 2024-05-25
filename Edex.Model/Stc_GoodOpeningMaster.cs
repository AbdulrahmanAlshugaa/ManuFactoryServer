using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public partial class Stc_GoodOpeningMaster
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
        public double CreditGoldAccountID { get; set; }
        public double DiscountCreditAccount { get; set; }
        public double TransportDebitAccount { get; set; }
        public double CheckAccount { get; set; }
        public double AdditionalAccount { get; set; }
        public double NetAccount { get; set; }
        public double NetAmount { get; set; }
        public double NetType { get; set; }
        public int CurencyID { get; set; }
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
        public int Posted { get; set; }

        public double DebitGoldAccountID { get; set; }

        public decimal TypeGold { get; set; }
        public List<Stc_GoodOpeningDetails> Datails { get; set; }
        public string Method { get; set; }
        public int RegistrationNo { get; set; }
        public string SupplierName { get; set; }
        public string VATID { get; set; }
        public string CostCenterName { get; set; }
        public string StoreName { get; set; }
        public int ItemStatus { get; set; }
        public int PageNo { get; set; }
        public double NetBalance { get; set; }
        public decimal AdditionaAmountTotal { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal InvoiceGoldTotal { get; set; }
        public decimal InvoiceEquivalenTotal { get; set; }


        public string OperationTypeName { get; set; }


        public string Mobile { get; set; }

    }

    
}

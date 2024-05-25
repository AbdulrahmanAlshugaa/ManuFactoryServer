using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public class SalesCashierClose
    {
        public int CloseCashierID { get; set; }

        public long CloseCashierDate { get; set; }

        public decimal WasteCost { get; set; }
        public decimal CashSum { get; set; }
        public decimal FutureSum { get; set; }
        public decimal PrevoiusCash { get; set; }
        public decimal NetSum { get; set; }
        public decimal EnterCost { get; set; }
        public int UserID { get; set; }
        public int SellerID { get; set; }
        public int FromSaleInvoice { get; set; }
        public int ToSaleInvoice { get; set; }
        public int FromSaleInvoiceReturn { get; set; }
        public int ToSaleInvoiceReturn { get; set; }
    }
}

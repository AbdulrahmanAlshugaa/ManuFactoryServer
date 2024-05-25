using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
     public    class SalseInvoicesReport
    {
         public string BranchID { get; set; }
         public string InvoiceID { get; set; }
         public string InvoiceDate { get; set; }
         public string SaleMethod { get; set; }
         public string StoreName { get; set; }
         public string CustomerName { get; set; }
         public string SellerName { get; set; }
         public string CostCenterName { get; set; }
         public string Total { get; set; }
         public string DescountTotal { get; set; }
         public string SumVAt { get; set; }
         public string NetBalance { get; set; }
         public string Profit { get; set; }
         public string Notes { get; set; }
         public string SaleDelegateName { get; set; }
         public string Net { get; set; }
         
             
    }
}

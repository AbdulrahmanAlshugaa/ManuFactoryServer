using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edex.RestaurantSystem.Transactions
{
    class MSgSettingsDetials
    {
        public int InvoiceID { get; set; }
        public string QTY { get; set; }
        public string ItemName { get; set; }
        public string Confirm { get; set; }
        public string BarCode { get; set; }
        
        public string Status { get; set; }
    }
}

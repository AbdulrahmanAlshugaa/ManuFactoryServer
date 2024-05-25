using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public class AccountVoucherReport
    {

      
        public string VoucherID { get; set; }
        public string VoucherDate { get; set; }
        public string Amount { get; set; }
        public string DocumentID { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string RegistrationNo { get; set; }
        public string BranchID { get; set; }
        public string CostCenterName { get; set; }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.HR.HRClasses
{ 
    public partial class MonthlyAllowance
    {
        public int SN { get; set; }
        public long EmployeeID { get; set; }
        public int AllowanceID { get; set; }
        public string AllowanceName { get; set; }
        public decimal AllowanceAmount { get; set; }

        public DateTime AllowanceValidFromDate { get; set; }
        public string AllowanceNotes { get; set; }
        public long AllowanceAccountID { get; set; }
        public string AllowanceAccountName { get; set; }
    }
    public partial class MonthlyDeduction
    {
        public int SN { get; set; }
        public long EmployeeID { get; set; }
        public int DeductionID { get; set; }
        public string DeductionName { get; set; }
        public decimal DeductionAmount { get; set; }
        public DateTime DeductionValidFromDate { get; set; }
        public string DeductionNotes { get; set; }

        public long DeductionAccountID { get; set; }
        public string DeductionAccountName { get; set; }
    }


    public partial class VacationBalance
    {
        public int SN { get; set; }
        public long EmployeeID { get; set; }
        public int Year { get; set; }
        public string AccuredVacation { get; set; }
        


    }
}

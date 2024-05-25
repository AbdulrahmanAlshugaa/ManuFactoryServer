using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
     
    public class Menu_ProductionExpensesMaster
    {
        public int CostCenterID;

        public Nullable<double> ComandID { get; set; }
        public Nullable<double>  CommandDate { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Cancel { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> FacilityID { get; set; }
        public Nullable<int> EditUserID { get; set; }
        public Nullable<float> EditDate { get; set; }
        public Nullable<double> EditTime { get; set; }
        public Nullable<float> RegDate { get; set; }
        public Nullable<float> UserID { get; set; }
        public string EditComputerInfo { get; set; }
        public string ComputerInfo { get; set; }
        public Nullable<double> RegTime { get; set; }
        public Nullable<double> ToDate { get; set; }
        public Nullable<double> FromDate { get; set; }

        public Nullable<float> QTYOrders { get; set; }
        public Nullable<int> NumberOrder { get; set; }

        public Nullable<int> NumberCups { get; set; }
        public Nullable<decimal> QTYGram { get; set; }
        public Nullable<int> CategoryOrders { get; set; }

        public List<Manu_ProductionExpensesDetails> Manu_ProductionExpenses { get; set; }
        public List<Menu_ProductionExpensesAcconts> Manu_AccountDetils { get; set; }

        public string OrderID { get; set; }

        public int CastingID { get; set; }

        public decimal QTYOrder { get; set; }

        public double DebitAccountID { get; set; }

        public double CreditAccountID { get; set; }

        public int CurencyID { get; set; }

        public decimal SalesPriceQram { get; set; }

        public int Posted { get; set; }
    }
}

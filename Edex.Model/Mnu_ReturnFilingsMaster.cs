 
namespace Edex.Model
{
    using System;
    using System.Collections.Generic;

    public class Mnu_ReturnFilingsMaster
    {
        public int CommandID { get; set; } 
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        public int UserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; }

        public double DateBefore { get; set; }
        public double DateAfter { get; set; }
        public double StoreIDBefore { get; set; }
        public double StoreIDAfter { get; set; }
        public int CostCenterID { get; set; }
        public double FactorID { get; set; }
        public int CurrencyID { get; set; }
        public string Notes { get; set; }  
        public string StoreMangerAfter { get; set; }

        public List<Mnu_ReturnFilingsDetails> Manu_CadWaxFactorys { get; set; } 
        public Nullable<int> TypeStageID { get; set; }

        public double CommandDate { get; set; }
    }
}

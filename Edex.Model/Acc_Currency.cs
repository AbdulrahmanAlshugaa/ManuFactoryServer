using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    public partial class Acc_Currency
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDefault { get; set; }
        public string Notes { get; set; }
        public double ExchangeRate { get; set; }
        public double MinRate { get; set; }
        public double MaxRate { get; set; }

        public int UserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; }
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
    }
}

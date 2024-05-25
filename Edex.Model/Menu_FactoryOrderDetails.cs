﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{
    
    public partial class Menu_FactoryOrderDetails
    {
        public int ID { get; set; }
        public Nullable<double> ComandID { get; set; }
        public string BarCode { get; set; }
        public Nullable<double> EmpPolishnID { get; set; }
        public Nullable<double> EmpPrentagID { get; set; }
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
        public int ItemID { get; set; }
        public string ArbItemName { get; set; }
        public string EngItemName { get; set; }
        public int SizeID { get; set; }
        public string ArbSizeName { get; set; }
        public string EngSizeName { get; set; }
        public Nullable<double> QTY { get; set; }
        public Nullable<double> Credit { get; set; }
       public DateTime DebitDate { get; set; }
        public string DebitTime { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public string Signature { get; set; }
        public int PeriodDay { get; set; }
        public string StateName { get; set; }
        public string EngStateName { get; set; } 
        public int TypeOpration { get; set; }
        public decimal CostPrice { get; set; }
        public Nullable<double> DIAMOND_WG { get; set; }
        public Nullable<double> DIAMOND_WC { get; set; }

    }
}
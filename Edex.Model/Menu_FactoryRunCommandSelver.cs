﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Edex.Model
{
    using System;
    using System.Collections.Generic;
    public partial class Menu_FactoryRunCommandSelver
    {
        public int ID { get; set; }
        public Nullable<double> ComandID { get; set; }


        public Nullable<int> MachinID { get; set; }
        public string BarcodeAdditional { get; set; }
        public string MachineName { get; set; }


      
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

        public Nullable<double> Debit { get; set; }

        public DateTime DebitDate { get; set; }

        public string DebitTime { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string EmpID { get; set; }
        public string EmpName { get; set; }

        public string Signature { get; set; }
        public Nullable<double> Credit { get; set; }
        public Nullable<double> Lost { get; set; }
        public decimal CostPrice { get; set; }
        public int TypeOpration { get; set; }
        public double EmpAdditionalID { get; set; }

        public bool ShownInNext { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Edex.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Others_EntryAndLogOutEmployees
    {
        public int ID { get; set; }
        public int BranchID { get; set; }
        public int UserID { get; set; }
        public string DateLogIn { get; set; }
        public string TimeLogIn { get; set; }
        public string Destination { get; set; }
        public string Reason { get; set; }
        public string ByOrder { get; set; }
        public string TimeLogOut { get; set; }
        public string TimeLogBack { get; set; }
        public string Mobil { get; set; }
        public string Notes { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; }
    }
}
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
    
    public partial class Hotels_LostAndFound
    {
        public int ID { get; set; }
        public int BranchID { get; set; }
        public string ItemName { get; set; }
        public int RoomID { get; set; }
        public string L_F_Where { get; set; }
        public double L_F_Date { get; set; }
        public int L_F_Time { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        public int UserID { get; set; }
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
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
    
    public partial class Hotels_RoomsTariffe
    {
        public int ID { get; set; }
        public int BranchID { get; set; }
        public int RoomTypeID { get; set; }
        public int RateTypeID { get; set; }
        public double WeakdayTariffe { get; set; }
        public double WeakdayMinTariffe { get; set; }
        public double WeakdayExtraAdult { get; set; }
        public double WeakdayExtraChild { get; set; }
        public double WeakendTariffe { get; set; }
        public double WeakendMinTariffe { get; set; }
        public double WeakendExtraAdult { get; set; }
        public double WeakendExtraChild { get; set; }
        public int UserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; }
        public string OperationType { get; set; }
    }
}

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
    
    public partial class Hotels_ReservedRooms
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        public int BranchID { get; set; }
        public int RoomID { get; set; }
        public double Tariffe { get; set; }
        public double MinTariffe { get; set; }
        public double ExtraAdult { get; set; }
        public double ExtraChild { get; set; }
    }
}
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
    
    public partial class Hotels_OperationAndStatus
    {
        public int StatusID { get; set; }
        public int Checkin { get; set; }
        public int Checkout { get; set; }
        public int Reservation { get; set; }
        public int CancelReservation { get; set; }
        public int Renewal { get; set; }
        public int EarlyDeparture { get; set; }
        public int OutOfServece { get; set; }
        public int GuestTransfer { get; set; }
    }
}

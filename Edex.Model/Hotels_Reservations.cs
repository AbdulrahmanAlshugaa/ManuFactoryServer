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
    
    public partial class Hotels_Reservations
    {
        public int ReservationID { get; set; }
        public int BranchID { get; set; }
        public double ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public double DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public int Nights { get; set; }
        public int AdultsNo { get; set; }
        public int ChildsNo { get; set; }
        public int RateType { get; set; }
        public double CustomerID { get; set; }
        public string CustomerArbName { get; set; }
        public string CustomerEngName { get; set; }
        public byte[] CustomerImage { get; set; }
        public int MethodID { get; set; }
        public string Notes { get; set; }
        public int MealBreakfast { get; set; }
        public int MealLunch { get; set; }
        public int MealDinner { get; set; }
        public int MealSuhoor { get; set; }
        public string CancelReasons { get; set; }
        public double CancelDate { get; set; }
        public int UserID { get; set; }
        public double RegDate { get; set; }
        public double RegTime { get; set; }
        public int EditUserID { get; set; }
        public double EditTime { get; set; }
        public double EditDate { get; set; }
        public string ComputerInfo { get; set; }
        public string EditComputerInfo { get; set; }
        public int Cancel { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public double SpecialDiscount { get; set; }
        public string ContactPerson { get; set; }
        public int Gender { get; set; }
        public int NationalityID { get; set; }
        public string IdentityNumber { get; set; }
        public int IdentityTypeID { get; set; }
        public double IdentityExpiryDate { get; set; }
        public string CustomerType { get; set; }
        public string TotalAccordingTo { get; set; }
    }
}

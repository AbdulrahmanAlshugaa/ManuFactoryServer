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
    
    public partial class Clinic_WaitList
    {
        public double ID { get; set; }
        public double PatientID { get; set; }
        public double DoctorID { get; set; }
        public double DateIn { get; set; }
        public double TimeIn { get; set; }
        public double DateOut { get; set; }
        public double TimeOut { get; set; }
        public int State { get; set; }
        public int BranchID { get; set; }
        public int Cancel { get; set; }
    }
}

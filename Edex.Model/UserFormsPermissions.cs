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
    
    public partial class UserFormsPermissions
    {

        public int UserID { get; set; }
        public string FormName { get; set; }
        public int FormView { get; set; }
        public int FormAdd { get; set; }
        public int FormDelete { get; set; }
        public int FormUpdate { get; set; }
        public int DaysAllowedForEdit { get; set; }
        public string ReportName { get; set; }
        public int ReportView { get; set; }
        public int ReportExport { get; set; }
        public int ShowReportInReportViewer { get; set; }
        public int BranchID { get; set; }
        public int FacilityID { get; set; }
        
    }
}

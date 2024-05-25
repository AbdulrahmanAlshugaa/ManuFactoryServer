using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Edex.ModelSystem;
using Edex.Model;
using Edex.DAL.Configuration;

namespace Edex.Reports
{
    public partial class rptCompanyHeaderEng : DevExpress.XtraReports.UI.XtraReport
    {
        public rptCompanyHeaderEng()
        {
            InitializeComponent();


            /**********************************************************************/
            var dtCompanyHeader = new dsReports.rptCompanyHeaderDataTable();
            var CompanyHeaderquery = CompanyHeaderDAL.GetDataByID(1, UserInfo.BRANCHID, UserInfo.FacilityID);
            var CompanyHeaderRow = dtCompanyHeader.NewRow();
            CompanyHeaderRow["CompanyArbName"] = CompanyHeaderquery.CompanyArbName;
            CompanyHeaderRow["CompanyEngName"] = CompanyHeaderquery.CompanyEngName;
            CompanyHeaderRow["ActivityArbName"] = CompanyHeaderquery.ActivityArbName;
            CompanyHeaderRow["ActivityEngName"] = CompanyHeaderquery.ActivityEngName;
            CompanyHeaderRow["ArbAddress"] = CompanyHeaderquery.ArbAddress;
            CompanyHeaderRow["EngAddress"] = CompanyHeaderquery.EngAddress;
            CompanyHeaderRow["ArbTel"] = CompanyHeaderquery.ArbTel;
            CompanyHeaderRow["EngTel"] = CompanyHeaderquery.EngTel;
            CompanyHeaderRow["ArbFax"] = CompanyHeaderquery.ArbFax;
            CompanyHeaderRow["EngFax"] = CompanyHeaderquery.EngFax;
            CompanyHeaderRow["pic"] = CompanyHeaderquery.pic;
            dtCompanyHeader.Rows.Add(CompanyHeaderRow);
            this.DataMember = "rptCompanyHeaderDataTable";
            this.DataSource = dtCompanyHeader;
            /*******************************************************************************/
        }

    }
}

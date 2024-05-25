using DevExpress.XtraReports.UI;
using Edex.DAL.Configuration;
using Edex.Model;
using Edex.Model.Language;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Edex.ModelSystem
{
    public class ReportComponent
    {
        private string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
        static public string GetReportPath() { return System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\"; }
        static public XtraReport CompanyHeader()
        {
            string rptCompanyHeaderName = "rptCompanyHeader";
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
            //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
            rptCompanyHeaderName += "Arb";
            XtraReport rptCompanyHeader = XtraReport.FromFile(Path + rptCompanyHeaderName + ".repx", true);
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
        
            CompanyHeaderRow["VATARB"] ="";
            CompanyHeaderRow["VATENG"] = "";
            dtCompanyHeader.Rows.Add(CompanyHeaderRow);
            rptCompanyHeader.DataMember = "rptCompanyHeaderDataTable";
            rptCompanyHeader.DataSource = dtCompanyHeader;
            return rptCompanyHeader;
        }

        static public XtraReport CompanyHeaderLand()
        {
            string rptCompanyHeaderName = "rptCompanyHeaderLand";
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
           // rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
            rptCompanyHeaderName += "Arb";
            XtraReport rptCompanyHeader = XtraReport.FromFile(Path + rptCompanyHeaderName + ".repx", true);
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
            rptCompanyHeader.DataMember = "rptCompanyHeaderDataTable";
            rptCompanyHeader.DataSource = dtCompanyHeader;
            return rptCompanyHeader;
        }


        static public XtraReport CompanyHeaderLand2()
        {
            string rptCompanyHeaderName = "rptCompanyHeaderLand";
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
            // rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");

            rptCompanyHeaderName += "Arb";

            XtraReport rptCompanyHeader = XtraReport.FromFile(Path + rptCompanyHeaderName + ".repx", true);

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
            CompanyHeaderRow["ArbFax"] = MySession.VAtCompnyGlobal + "    :الرقم الضـــريبي  ";
            CompanyHeaderRow["EngFax"] = "VATID   :" + MySession.VAtCompnyGlobal;
            dtCompanyHeader.Rows.Add(CompanyHeaderRow);
            rptCompanyHeader.DataMember = "rptCompanyHeaderDataTable";
            rptCompanyHeader.DataSource = dtCompanyHeader;
            return rptCompanyHeader;
        }
        static public DataTable SelectRecord( string strSQL)
        {
            string CONNECTION_STRING = ConfigurationManager.AppSettings["AccessDBConnection"].ToString();
                OleDbConnection con = new OleDbConnection(CONNECTION_STRING);
                DataTable dtGeneral = new DataTable();
                OleDbDataAdapter da = new OleDbDataAdapter();
                OleDbCommand comSelect = new OleDbCommand();
                comSelect.Connection = con;
                comSelect.CommandType = CommandType.Text;
                comSelect.CommandText = strSQL;
                da.SelectCommand = comSelect;
                da.Fill(dtGeneral);
                return dtGeneral;
   
        }

        static public int GettRecord(string SettingName)
        {
            string CONNECTION_STRING = ConfigurationManager.AppSettings["AccessDBConnection"].ToString();
            OleDbConnection con = new OleDbConnection(CONNECTION_STRING);
            DataTable dtGeneral = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbCommand comSelect = new OleDbCommand();
            comSelect.Connection = con;
            comSelect.CommandType = CommandType.Text;
            comSelect.CommandText = "SELECT SettingValue from ComputerSetting where SettingName='" + SettingName + "'";
            da.SelectCommand = comSelect;
            da.Fill(dtGeneral);
            int value = Comon.cInt(dtGeneral.Rows[0][0]);
            return value;

        }

    }
}

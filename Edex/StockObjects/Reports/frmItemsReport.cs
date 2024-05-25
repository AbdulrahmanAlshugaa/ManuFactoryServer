using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Edex.GeneralObjects.GeneralForms;
using Edex.Model;
using DevExpress.XtraSplashScreen;
using Edex.Model.Language;
using Edex.ModelSystem;
using DevExpress.XtraReports.UI;
using Edex.DAL.Stc_itemDAL;
using Edex.GeneralObjects.GeneralClasses;

namespace Edex.StockObjects.Reports
{
  
    public partial class frmItemsReport : BaseForm
    {
        #region Declare
       
        private string strSQL;
       public DataTable dt = new DataTable();
        #endregion
        public frmItemsReport()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;

            FillCombo.FillComboBox(cmbSizeID, "Stc_SizingUnits", "SizeID", "ArbName", "", "Cancel=0", "");
            FillCombo.FillComboBox(cmbItemGroup, "Stc_ItemsGroups", "GroupID", "ArbName", "", "Cancel=0 and AccountTypeID=" + 1 + " and BranchID=" + MySession.GlobalBranchID, "");
            DataTable dtr = new DataTable();
            dtr.Columns.Add("ID");
            dtr.Columns.Add("Name");
            DataRow newRow = dtr.NewRow();
            newRow["ID"] = "0";
            newRow["Name"] = "فعال";
            dtr.Rows.Add(newRow);
            newRow = dtr.NewRow();
            newRow["ID"] = "1";
            newRow["Name"] = "موقف";
            dtr.Rows.Add(newRow);
            cmbItemStatus.Properties.DataSource = dtr;
            cmbItemStatus.Properties.DisplayMember = "Name";
            cmbItemStatus.Properties.ValueMember = "ID";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        public void ClearFields()
        {
            try
            {
                dt.Clear();
                gridControl.DataSource = dt;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                bool IncludeHeader = true;
                ReportName = "‏‏rptItems";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /***************** Master *****************************/

                rptForm.RequestParameters = false;



                rptForm.Parameters["FromItemID"].Value = txtFromItemID.Text.Trim().ToString();
                rptForm.Parameters["ToItemID"].Value = txtToItemID.Text.Trim().ToString();


                rptForm.Parameters["SizeID"].Value = cmbSizeID.Text.Trim().ToString();
                rptForm.Parameters["GroupID"].Value = cmbItemGroup.Text.Trim().ToString();
                rptForm.Parameters["ItemStatus"].Value =cmbItemStatus.Text.Trim().ToString();
                /********************** Details ****************************/
                var dataTable = new Edex.ModelSystem.dsReports.rptItemsDataTable();
                
                for (int i = 0; i <=gridView1.DataRowCount- 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["n_invoice_serial"] = i + 1;
                    row["ItemID"] =gridView1.GetRowCellValue(i,"ItemID").ToString();
                    row["ArbItemName"] = gridView1.GetRowCellValue(i, "ArbItemName").ToString();
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["GroupID"] = gridView1.GetRowCellValue(i, "GroupID").ToString();
                    row["GroupName"] = gridView1.GetRowCellValue(i, "GroupName").ToString();
                    row["GroupParentID"] = gridView1.GetRowCellValue(i, "GroupParentID").ToString();
                    row["GroupParentName"] = gridView1.GetRowCellValue(i, "GroupParentName").ToString();
                    row["TypeName"] = gridView1.GetRowCellValue(i, "TypeName").ToString();
                    row["SizeID"] = gridView1.GetRowCellValue(i, "SizeID").ToString();
                    row["SizeName"] = gridView1.GetRowCellValue(i, "SizeName").ToString(); 
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptBalanceReview";
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = true;
                if (ShowReportInReportViewer)
                {
                    frmReportViewer frmRptViewer = new frmReportViewer();
                    frmRptViewer.documentViewer1.DocumentSource = rptForm;
                    frmRptViewer.ShowDialog();
                }
                else
                {
                    bool IsSelectedPrinter = false;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                    for (int i = 1; i < 6; i++)
                    {
                        string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                        if (!string.IsNullOrEmpty(PrinterName))
                        {
                            rptForm.PrinterName = PrinterName;
                            rptForm.Print(PrinterName);
                            IsSelectedPrinter = true;
                        }
                    }
                    SplashScreenManager.CloseForm(false);
                    if (!IsSelectedPrinter)
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnPrintItemUnitBarcode_Click(object sender, EventArgs e)
        {

            try
            {

                ClearFields();
                {
                    
                    List<Stc_ItemUnits> ListItems = new List<Stc_ItemUnits>();

                    ListItems = Stc_itemsDAL.GetAllData(Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID);

                    if (Comon.cLong(txtFromItemID.Text) > 0)
                        ListItems = ListItems.FindAll(x => x.ItemID >= Comon.cLong(txtFromItemID.Text));
                    if (Comon.cLong(txtToItemID.Text) > 0)
                        ListItems = ListItems.FindAll(x => x.ItemID <= Comon.cLong(txtToItemID.Text));
                    if (Comon.cLong(cmbSizeID.EditValue) > 0)
                        ListItems = ListItems.FindAll(x => x.SizeID == Comon.cLong(cmbSizeID.EditValue));
                    if (Comon.cLong(cmbItemGroup.EditValue) > 0)
                        ListItems = ListItems.FindAll(x => x.Stc_Items.GroupID <= Comon.cLong(cmbItemGroup.EditValue));
                    if (Comon.cLong(cmbItemStatus.EditValue) > 0)
                        ListItems = ListItems.FindAll(x => x.UnitCancel == Comon.cLong(cmbItemStatus.EditValue));
                    for (int i = 0; i <= ListItems.Count - 1; i++)
                    {
                        dt.NewRow();
                        dt.Rows.Add();
                        dt.Rows[dt.Rows.Count - 1]["n_invoice_serial"] = i + 1;

                        dt.Rows[dt.Rows.Count - 1]["ItemID"] = ListItems[i].ItemID;
                        dt.Rows[dt.Rows.Count - 1]["ArbItemName"] = ListItems[i].Stc_Items.ArbName;
                        dt.Rows[dt.Rows.Count - 1]["BarCode"] = ListItems[i].BarCode;
                        DataTable dtt = Lip.SelectRecord("select ArbName as GroupParentName , GroupID as GroupParentID from [Stc_ItemsGroups] where [GroupID] in(SELECT  [ParentAccountID] FROM  [Stc_ItemsGroups] where [GroupID]=" + ListItems[i].Stc_Items.GroupID + ")");
                        dt.Rows[dt.Rows.Count - 1]["GroupID"] = ListItems[i].Stc_Items.GroupID;
                        dt.Rows[dt.Rows.Count - 1]["GroupName"] = ListItems[i].Stc_Items.GroupName;
                        dt.Rows[dt.Rows.Count - 1]["GroupParentID"] = dtt.Rows[0]["GroupParentID"];
                        dt.Rows[dt.Rows.Count - 1]["GroupParentName"] = dtt.Rows[0]["GroupParentName"];
                        dt.Rows[dt.Rows.Count - 1]["TypeName"] = ListItems[i].Stc_Items.TypeName;
                        dt.Rows[dt.Rows.Count - 1]["SizeID"] = ListItems[i].SizeID;
                        dt.Rows[dt.Rows.Count - 1]["SizeName"] = ListItems[i].ArbSizeName;
                       
                    }
                    gridControl.DataSource = dt;
                    gridControl.RefreshDataSource();

                }
            }
               
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmItemsReport_Load(object sender, EventArgs e)
        {
            dt.Columns.Add(new DataColumn("n_invoice_serial", typeof(int)));
            dt.Columns.Add(new DataColumn("GroupParentID", typeof(string)));
            dt.Columns.Add(new DataColumn("GroupParentName", typeof(string)));
            dt.Columns.Add(new DataColumn("GroupID", typeof(string)));
            dt.Columns.Add(new DataColumn("GroupName", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemID", typeof(string)));
            dt.Columns.Add(new DataColumn("BarCode", typeof(string)));
            dt.Columns.Add(new DataColumn("ArbItemName", typeof(string)));
            dt.Columns.Add(new DataColumn("TypeName", typeof(string)));
            dt.Columns.Add(new DataColumn("SizeID", typeof(string)));
            dt.Columns.Add(new DataColumn("SizeName", typeof(string)));

          

        }
    }
}
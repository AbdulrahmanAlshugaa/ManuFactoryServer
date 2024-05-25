using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.Model;
using Edex.Model.Language;
using Edex.AccountsObjects.Transactions;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.SalesAndPurchaseObjects.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edex.AccountsObjects.Reports
{
    public partial class frmCheckReceiptVouchersReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        string frm = "frmAccountStatement";
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string langName = "";
        private string FormatType = "Gre";
        public DataTable _sampleData = new DataTable();
        public DataTable dt = new DataTable();

        public frmCheckReceiptVouchersReport()
        {
            try{
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            GridView1.OptionsBehavior.ReadOnly = true;
            GridView1.OptionsBehavior.Editable = false;
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
            where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + UserInfo.BRANCHID;
         
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            ///////////////////////////////////////////////////////
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";

        
            GridView1.OptionsView.EnableAppearanceEvenRow = true;
            GridView1.OptionsView.EnableAppearanceOddRow = true;
           // this.GridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);

            if (UserInfo.Language == iLanguage.English)
            {

                dgvColVoucherID.Caption = "Voucher ID ";
                dgvColVoucherDate.Caption = "Voucher Date";
                dgvColAmount.Caption = "Amount";
                dgvColDeclaration.Caption = "Declartion  ";
                dgvColnSN.Caption = "# ";
                dgvColDocNo.Caption = "Doc NO";
                dgvColRecordType.Caption = "Record Type";

                dgvColUserName.Caption = "User";

                btnShow.Text = "show";
                //  Label8.Text = btnShow.Tag.ToString();

            }
            }
            catch { }
        }
        private DataTable InitialFiveRows(DataTable dt, int RowsCount)
        {

            int currDatatableCount = dt.Rows.Count;
            for (int i = currDatatableCount; i < RowsCount + currDatatableCount; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i + 1;
                dr[5] = "";

                dt.Rows.Add(dr);
                makeGridBind(dt);
            }

            return dt;

        }

        protected override void DoNew()
        {
            try
            {
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtFromVoucherNo .Text = "";
                txtToVoucherNo.Text = "";
              
                txtFromDate.Text = "";
                txtToDate.Text = "";
            
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtToVoucherNo.Enabled = true;
                txtFromVoucherNo.Enabled = true;
              


            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
          


        }
        void makeGridBind(DataTable dt)
        {
            DataView dv = dt.DefaultView;
            _sampleData = dt;
            gridControl1.DataSource = dt;


        }
        private void PurchaseInvoice()
        {
            try
            {
                DataRow row;
                dt = Lip.SelectRecord(GetStrSQL());
                _sampleData.Clear();
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["SN"] = _sampleData.Rows.Count + 1;
                            row["Amount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["Amount"]) - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountAmount"])).ToString("N" + 2);
                            row["Declaration"] = dt.Rows[i]["Description"].ToString();
                            row["VoucherDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["VoucherDate"].ToString());
                            row["DocNo"] = dt.Rows[i]["DocumentID"].ToString();
                            row["UserName"] = dt.Rows[i]["UserName"].ToString();
                            row["VoucherID"] = dt.Rows[i]["VoucherID"].ToString();
                            _sampleData.Rows.Add(row);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);



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
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["FromVoucherID"].Value = txtFromVoucherNo.Text.Trim().ToString();
                rptForm.Parameters["ToVoucherID"].Value = txtToVoucherNo.Text.Trim().ToString();

                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptCheckReceiptVouchersReportDataTable();

                for (int i = 0; i <= GridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["Amount"] = GridView1.GetRowCellValue(i, "Amount").ToString();
                    row["Declaration"] = GridView1.GetRowCellValue(i, "Declaration").ToString();
                    row["VoucherDate"] = GridView1.GetRowCellValue(i, "VoucherDate").ToString();
                    row["DocNo"] = GridView1.GetRowCellValue(i, "DocNo").ToString();
                    row["UserName"] = GridView1.GetRowCellValue(i, "UserName").ToString();
                    row["VoucherID"] = GridView1.GetRowCellValue(i, "VoucherID").ToString();
                  //  row["Declaration"] = GridView1.GetRowCellValue(i, "Declaration").ToString();
                   
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptCheckReceiptVouchersReport";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);
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


        string GetStrSQL()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();

                string filter = "(.Acc_CheckReceiptVoucherMaster.BranchID = " + UserInfo.BRANCHID + ")  AND .Acc_CheckReceiptVoucherMaster.Cancel =0   AND";
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));




                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + ".Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + ".Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate <=" + ToDate + " AND ";

                ////// '''البائع''''العميل''''التكلفة''''المستودع
                if (txtFromVoucherNo.Text != string.Empty)
                    filter = filter + ".Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID >=" + txtFromVoucherNo.Text + " AND ";

               if (txtToVoucherNo.Text != string.Empty)
                   filter = filter + ".Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID <=" + txtToVoucherNo.Text + " AND ";

                ////if (txtPercentage.Text != string.Empty)
                ////    filter = filter + " .Sales_SalesDelegate.Percentage  =" + Comon.cInt(txtPercentage.Text) + "  AND ";


                ////// '''''''''''''
              filter = filter.Remove(filter.Length - 4, 4);
                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal, غير موجود في جدول مردود المشتريات

              strSQL = "SELECT (dbo.Acc_CheckReceiptVoucherMaster.DebitAmount) AS Amount, dbo.Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID AS VoucherID, Acc_CheckReceiptVoucherMaster.DiscountAmount,"
       + " dbo.Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate AS VoucherDate, dbo.Acc_CheckReceiptVoucherMaster.Notes AS Description, dbo.Users.ArbName AS UserName,"
        + " dbo.Acc_CheckReceiptVoucherMaster.DocumentID FROM dbo.Users INNER JOIN dbo.Acc_CheckReceiptVoucherMaster "

          + "  ON dbo.Users.UserID = dbo.Acc_CheckReceiptVoucherMaster.UserID  Where" + filter;
              strSQL = strSQL + "order by VoucherDate ASC";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return strSQL;

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            PurchaseInvoice();
            gridControl1.DataSource = _sampleData;
            if (GridView1.RowCount > 0)
            {
                btnShow.Visible = true;

                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                txtToVoucherNo.Enabled = false;
                txtFromVoucherNo.Enabled = false;



            }
            else
            {

                if (MySession.GlobalLanguageName == iLanguage.Arabic)
                    XtraMessageBox.Show("لايوجد بيانات لعرضها", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    XtraMessageBox.Show("There is no Data to show it", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                btnShow.Visible = true;

                DoNew();
            }


        }

        private void frmCheckReceiptVouchersReport_Load(object sender, EventArgs e)
        {
            try{
            _sampleData.Columns.Add(new DataColumn("SN", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Amount", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VoucherDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DocNo", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VoucherID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserName", typeof(string)));

            InitialFiveRows(_sampleData, 1);
            }
            catch { }
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                frmCheckReceiptVoucher frm = new frmCheckReceiptVoucher();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("VoucherID").ToString()) + 1, 8);
                }
                else
                    frm.Dispose();


            }
            catch { }
        }
        private void txtFromDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Comon.ConvertDateToSerial(txtFromDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtFromDate.Text = Lip.GetServerDate();
        }

        private void txtToDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Comon.ConvertDateToSerial(txtToDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtToDate.Text = Lip.GetServerDate();
        }

    }
}

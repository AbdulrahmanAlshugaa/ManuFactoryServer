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
using Edex.GeneralObjects.GeneralClasses;

namespace Edex.AccountsObjects.Reports
{
    public partial class frmSpendVouchersReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        string frm = "frmAccountStatement";
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string langName = "";
        private string FormatType = "Gre";
        public DataTable _sampleData = new DataTable();
        public DataTable dt = new DataTable();

        public frmSpendVouchersReport()
        {
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
            where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + MySession.GlobalBranchID;

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
                decimal TotalAmount = 0;
                decimal TotalGold = 0;
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["SN"] = _sampleData.Rows.Count + 1;
                            row["Amount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["Amount"]) - Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountAmount"])).ToString("N" + 2);
                            row["TotalGold"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["TotalGold"]).ToString("N" + 2);

                            row["Declaration"] = dt.Rows[i]["Description"].ToString();
                            row["VoucherDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["VoucherDate"].ToString());
                            row["DocNo"] = dt.Rows[i]["DocumentID"].ToString();
                            row["UserName"] = dt.Rows[i]["UserName"].ToString();
                            row["VoucherID"] = dt.Rows[i]["VoucherID"].ToString();
                            TotalAmount += Comon.cDec(row["Amount"]);
                            TotalGold += Comon.cDec(row["TotalGold"]);
                            _sampleData.Rows.Add(row);

                        }
                        txtTotalAmount.Text = TotalAmount + "";
                        txtTotalGold.Text = TotalGold + "";
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
                ReportName = "rptSpendVouchersReport";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
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
                var dataTable = new dsReports.rptSpendVouchersReportDataTable();

                for (int i = 0; i <= GridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["Amount"] = GridView1.GetRowCellValue(i, "Amount").ToString();
                    row["TotalGold"] = GridView1.GetRowCellValue(i, "TotalGold").ToString();
                  
                    row["Declaration"] = GridView1.GetRowCellValue(i, "Declaration").ToString();
                    row["VoucherDate"] = GridView1.GetRowCellValue(i, "VoucherDate").ToString();
                    row["DocNo"] = GridView1.GetRowCellValue(i, "DocNo").ToString();
                    row["UserName"] = GridView1.GetRowCellValue(i, "UserName").ToString();
                    row["VoucherID"] = GridView1.GetRowCellValue(i, "VoucherID").ToString();
                    //  row["Declaration"] = GridView1.GetRowCellValue(i, "Declaration").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptSpendVouchersReport";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                ShowReportInReportViewer = true;
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

                string filter = "(.Acc_SpendVoucherMaster.BranchID = " + cmbBranchesID.EditValue + ")  AND .Acc_SpendVoucherMaster.Cancel =0   AND ";

                if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                    filter = "(.Acc_SpendVoucherMaster.BranchID = " +Comon.cInt( cmbBranchesID.EditValue) + " )  AND dbo.Acc_SpendVoucherMaster.Cancel =0   AND ";
               
                
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + ".Acc_SpendVoucherMaster.SpendVoucherDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + ".Acc_SpendVoucherMaster.SpendVoucherDate <=" + ToDate + " AND ";

                ////// '''البائع''''العميل''''التكلفة''''المستودع
                if (txtFromVoucherNo.Text != string.Empty)
                    filter = filter + ".Acc_SpendVoucherMaster.SpendVoucherID >=" + txtFromVoucherNo.Text + " AND ";

                if (txtToVoucherNo.Text != string.Empty)
                    filter = filter + ".Acc_SpendVoucherMaster.SpendVoucherID <=" + txtToVoucherNo.Text + " AND ";

                ////if (txtPercentage.Text != string.Empty)
                ////    filter = filter + " .Sales_SalesDelegate.Percentage  =" + Comon.cInt(txtPercentage.Text) + "  AND ";


                ////// '''''''''''''
                filter = filter.Remove(filter.Length - 4, 4);
                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal, غير موجود في جدول مردود المشتريات
                 
             strSQL = "SELECT dbo.Acc_SpendVoucherMaster.TotalGold , dbo.Acc_SpendVoucherMaster.CreditAmount AS Amount, dbo.Acc_SpendVoucherMaster.SpendVoucherID AS VoucherID,Acc_SpendVoucherMaster.DiscountAmount,"
             + " dbo.Acc_SpendVoucherMaster.SpendVoucherDate AS VoucherDate, dbo.Acc_SpendVoucherMaster.Notes AS Description, dbo.Users.ArbName AS UserName,"
            + " dbo.Acc_SpendVoucherMaster.DocumentID  FROM dbo.Users INNER JOIN dbo.Acc_SpendVoucherMaster "
            + "  ON dbo.Users.UserID = dbo.Acc_SpendVoucherMaster.UserID  Where" + filter;

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
        protected override void DoNew()
        {
            try
            {
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtFromVoucherNo.Text = "";
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
            _sampleData.Columns.Add(new DataColumn("SN", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Amount", typeof(decimal)));
             _sampleData.Columns.Add(new DataColumn("TotalGold", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VoucherDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DocNo", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VoucherID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserName", typeof(string)));

            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue =MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            InitialFiveRows(_sampleData, 1);

             
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            try{
            GridView view = sender as GridView;
            frmSpendVoucher frm = new frmSpendVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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

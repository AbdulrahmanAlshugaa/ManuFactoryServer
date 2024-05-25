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
using Edex.Model;
using Edex.Model.Language;
using DevExpress.XtraTreeList;
using Edex.GeneralObjects.GeneralForms;
using DevExpress.XtraSplashScreen;
using Edex.ModelSystem;
using DevExpress.XtraReports.UI;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraGrid.Views.Grid;
using Edex.AccountsObjects.Transactions;
namespace Edex.AccountsObjects.Reports
{
    public partial class frmMachinVouchersReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {        
       
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string langName = "";
        private string FormatType = "Gre";
        public DataTable _sampleData = new DataTable();
        public DataTable dt = new DataTable();
        public frmMachinVouchersReport()
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
                dgvColAmount.Caption = "Debit";
                dvgColCredit.Caption = "Credit";
                dgvColDeclaration.Caption = "Declartion  ";
                dgvColnSN.Caption = "# ";
                dgvColDocNo.Caption = "Doc NO";
                dgvColRecordType.Caption = "Record Type";
                dgvColUserName.Caption = "User";
                btnShow.Text = "show";
                //  Label8.Text = btnShow.Tag.ToString();

            }
            this.GridView1.RowClick += GridView1_RowClick;
            this.btnShow.Click+=btnShow_Click;
            this.Load += frmMachinVouchersReport_Load;
            this.GridView1.DoubleClick+=GridView1_DoubleClick;
            this.txtFromDate.EditValueChanged+=txtFromDate_EditValueChanged;
            this.txtToDate.EditValueChanged += txtToDate_EditValueChanged;
            this.GridView1.RowCellStyle += gridView1_RowCellStyle;
            // اذا كان نوع الجرد مستمر 
            if (MySession.GlobalInventoryType == 1)
            {
               GridView1.Columns["CreditGold"].Visible = false;
               GridView1.Columns["DebitGold"].Visible = false;
               GridView1.Columns["CreditDiamond"].Visible = false;
               GridView1.Columns["DebitDiamond"].Visible = false;
              
            }

        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

            if (DeffirantIsValid(e.RowHandle))
            {
                e.Appearance.BackColor = Color.Red;
            }
            else
            {
                // Set the default background color of the row
                e.Appearance.BackColor = e.Appearance.BackColor;
            }
        }
        private bool DeffirantIsValid(int rowHandle)
        {
            string Barcode = "";
            decimal Debit =Comon.ConvertToDecimalPrice( GridView1.GetRowCellValue(rowHandle, "Debit"));
            decimal Credit = Comon.ConvertToDecimalPrice(GridView1.GetRowCellValue(rowHandle, "Credit"));
            decimal DebitGold = Comon.ConvertToDecimalQty(GridView1.GetRowCellValue(rowHandle, "DebitGold"));
            decimal CreditGold = Comon.ConvertToDecimalQty(GridView1.GetRowCellValue(rowHandle, "CreditGold"));
            decimal DebitDiamond = Comon.ConvertToDecimalQty(GridView1.GetRowCellValue(rowHandle, "DebitDiamond"));
            decimal CreditDiamond = Comon.ConvertToDecimalQty(GridView1.GetRowCellValue(rowHandle, "CreditDiamond"));

            if ((Debit - Credit) == 0 && (DebitGold - CreditGold) == 0 && (DebitDiamond - CreditDiamond) == 0)  
            {
                return false;
            }

           

            return true;  
        }

        private DataTable InitialFiveRows(DataTable dt, int RowsCount)
        {
            int currDatatableCount = dt.Rows.Count;
            for (int i = currDatatableCount; i < RowsCount + currDatatableCount; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i + 1;
                dr[9] = "";

                dt.Rows.Add(dr);
                makeGridBind(dt);
            }

            return dt;

        }
        void GridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
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
                        double debit = 0; double crdit = 0;
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["SN"] = _sampleData.Rows.Count + 1;
                            row["Debit"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["Debit"])).ToString("N" + 2);
                            row["Credit"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["Credit"])).ToString("N" + 2);

                            row["DebitGold"] = Comon.ConvertToDecimalQty(dt.Rows[i]["DebitGold"]);
                            row["CreditGold"] = Comon.ConvertToDecimalQty(dt.Rows[i]["CreditGold"]);


                            row["DebitDiamond"] = Comon.ConvertToDecimalQty(dt.Rows[i]["DebitDiamond"]);
                            row["CreditDiamond"] =Comon.ConvertToDecimalQty(dt.Rows[i]["CreditDiamond"]);


                            row["Declaration"] = dt.Rows[i]["Description"].ToString();
                            row["VoucherDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["VoucherDate"].ToString());
                            row["DocNo"] = dt.Rows[i]["DocumentID"].ToString();
                            row["UserName"] = dt.Rows[i]["UserName"].ToString();
                            row["VoucherID"] = dt.Rows[i]["VoucherID"].ToString();
                            debit += Comon.cDbl(row["Debit"]);
                            crdit += Comon.cDbl(row["Credit"]);

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
                ReportName = "‏‏rptMachinVouchersReport";
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
                var dataTable = new dsReports.rptMachinVouchersReportDataTable();

                for (int i = 0; i <= GridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["Debit"] = GridView1.GetRowCellValue(i, "Debit").ToString();
                    row["Credit"] = GridView1.GetRowCellValue(i, "Credit").ToString();
                    row["Declaration"] = GridView1.GetRowCellValue(i, "Declaration").ToString();
                    row["VoucherDate"] = GridView1.GetRowCellValue(i, "VoucherDate").ToString();
                    row["DocNo"] = GridView1.GetRowCellValue(i, "DocNo").ToString();
                    row["UserName"] = GridView1.GetRowCellValue(i, "UserName").ToString();
                    row["VoucherID"] = GridView1.GetRowCellValue(i, "VoucherID").ToString();
                    //  row["Declaration"] = GridView1.GetRowCellValue(i, "Declaration").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "‏‏rptMachinVouchersReport";

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


        string GetStrSQL()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();
                string filter="";
                if(UserInfo.ID!=1)
                  filter = "(dbo.Acc_VariousVoucherMachinMaster.BranchID = " +cmbBranchesID.EditValue+ ")  AND  dbo.Acc_VariousVoucherMachinMaster.Cancel =0   AND";
                else
                  filter = "  dbo.Acc_VariousVoucherMachinMaster.Cancel =0   AND";
                if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                    filter = "(dbo.Acc_VariousVoucherMachinMaster.BranchID = " + cmbBranchesID.EditValue + ")  AND dbo.Acc_VariousVoucherMachinMaster.Cancel =0   AND ";
               
                
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                 
                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + "dbo.Acc_VariousVoucherMachinMaster.VoucherDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + "dbo.Acc_VariousVoucherMachinMaster.VoucherDate <=" + ToDate + " AND ";

                ////// '''البائع''''العميل''''التكلفة''''المستودع
                if (txtFromVoucherNo.Text != string.Empty)
                    filter = filter + "dbo.Acc_VariousVoucherMachinMaster.VoucherID >=" + txtFromVoucherNo.Text + " AND ";

                if (txtToVoucherNo.Text != string.Empty)
                    filter = filter + "dbo.Acc_VariousVoucherMachinMaster.VoucherID <=" + txtToVoucherNo.Text + " AND ";

                ////if (txtPercentage.Text != string.Empty)
                ////    filter = filter + " .Sales_SalesDelegate.Percentage  =" + Comon.cInt(txtPercentage.Text) + "  AND ";


                ////// '''''''''''''
                filter = filter.Remove(filter.Length - 4, 4);
                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal, غير موجود في جدول مردود المشتريات

                strSQL =
        strSQL = "SELECT SUM(dbo.Acc_VariousVoucherMachinDetails.Debit) AS Debit,SUM(dbo.Acc_VariousVoucherMachinDetails.Credit) AS Credit,SUM(dbo.Acc_VariousVoucherMachinDetails.CreditDiamond) AS CreditDiamond,SUM(dbo.Acc_VariousVoucherMachinDetails.DebitDiamond) AS DebitDiamond,SUM(dbo.Acc_VariousVoucherMachinDetails.CreditGold) AS CreditGold,SUM(dbo.Acc_VariousVoucherMachinDetails.DebitGold) AS DebitGold, dbo.Acc_VariousVoucherMachinMaster.VoucherID AS VoucherID," 
       + " dbo.Acc_VariousVoucherMachinMaster.VoucherDate AS VoucherDate, dbo.Acc_VariousVoucherMachinMaster.Notes AS Description, dbo.Users.ArbName AS UserName," 
       + " dbo.Acc_VariousVoucherMachinMaster.DocumentID FROM dbo.Users INNER JOIN dbo.Acc_VariousVoucherMachinDetails INNER JOIN dbo.Acc_VariousVoucherMachinMaster ON " 
        + " dbo.Acc_VariousVoucherMachinDetails.VoucherID = dbo.Acc_VariousVoucherMachinMaster.VoucherID AND dbo.Acc_VariousVoucherMachinDetails.BranchID = " 
       + " dbo.Acc_VariousVoucherMachinMaster.BranchID ON dbo.Users.UserID = dbo.Acc_VariousVoucherMachinMaster.UserID Where "+filter
       + " GROUP BY dbo.Acc_VariousVoucherMachinMaster.VoucherID," 
       + " dbo.Acc_VariousVoucherMachinMaster.VoucherDate, dbo.Acc_VariousVoucherMachinMaster.Notes,"
       + " dbo.Users.ArbName, dbo.Acc_VariousVoucherMachinMaster.DocumentID Order by VoucherDate   ASC";
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
        
        
        protected override void  DoAddFrom ()
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
        private void frmMachinVouchersReport_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("SN", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));


            _sampleData.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));


            _sampleData.Columns.Add(new DataColumn("DebitDiamond", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("CreditDiamond", typeof(decimal)));


            _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VoucherDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DocNo", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VoucherID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserName", typeof(string)));

            InitialFiveRows(_sampleData, 1);
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            

        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                frmVariousVoucherMachin frm = new frmVariousVoucherMachin();
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

  

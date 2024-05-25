using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.StockObjects.StoresClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Edex.Model.Language;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraReports.UI;
using Edex.Reports;
using Edex.SalesAndPurchaseObjects.SalesClasses;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Columns;

namespace Edex.AccountsObjects.Reports
{
    public partial class frmBalanceReview : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        public DataTable _sampleData = new DataTable();
        public DataTable _sampleDataCustomer = new DataTable();
        DataTable dttem = new DataTable();
        private string PrimaryName;
        public frmBalanceReview()
        {
            try
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
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                PrimaryName = "ArbName";
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                GridView1.OptionsView.EnableAppearanceEvenRow = true;
                GridView1.OptionsView.EnableAppearanceOddRow = true;
                GridView1.OptionsBehavior.ReadOnly = true;
                GridView1.OptionsBehavior.Editable = false;
                if (UserInfo.Language == iLanguage.English)
                {
                    PrimaryName = "EngName";
                    dgvColAccountID.Caption = "Account NO ";
                    dgvColAccountName.Caption = "Account Name  ";
                    dgvColCredit.Caption = "Credit";
                    dgvColDebit.Caption = "Debit  ";
                    dgvColn_invoice_serial.Caption = "# ";
                    dgvColBalance.Caption = "Balance";
                    btnShow.Text = "show";
                  
                    //Label8.Text = btnShow.Tag.ToString();
                }
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
            }
            catch { }
        }
        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            Obj.EditValue = DateTime.Now;
        }
        private void frmAccountStatement_Load(object sender, EventArgs e)
        {
            if (chkViewWeght.Checked==false)
            {
                GridView1.Columns["DebitGold"].Visible = false;
                GridView1.Columns["CreditGold"].Visible = false;
                GridView1.Columns["DebitDiamond"].Visible = false;
                GridView1.Columns["CreditDiamond"].Visible = false;
                GridView1.Columns["AmountDebitCold"].Visible = false;
                GridView1.Columns["AmountCrditGold"].Visible = false;
                GridView1.Columns["AmountDebitDiamond"].Visible = false;
                GridView1.Columns["AmountCrditDiamond"].Visible = false;

            }
            try
            {
                FillCombo.FillComboBox(cmbLevelAccounts, "Acc_AccountsLevels", "LevelNumber", PrimaryName, "", "BranchID=" + Comon.cInt(cmbBranchesID.EditValue), (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

                DoAddFrom();

                 where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + MySession.GlobalBranchID;
                 _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Balance", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("BalanceDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("DebitDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("CreditDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("AmountCrditDiamond", typeof(decimal)));

                _sampleData.Columns.Add(new DataColumn("AmountDebitDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("AmountCrditGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("AmountDebitCold", typeof(decimal)));

                _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("BranchID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("BranchID", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("Balance", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("Credit", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("AccountID", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("CustomerName", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("Address", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("BalanceType", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("DebitBalance", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("CreditBalance", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("AccountBalance", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("BalanceDiamond", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("DebitDiamond", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("CreditDiamond", typeof(decimal)));

                _sampleDataCustomer.Columns.Add(new DataColumn("AmountCrditDiamond", typeof(decimal)));

                _sampleDataCustomer.Columns.Add(new DataColumn("AmountDebitDiamond", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("AmountCrditGold", typeof(decimal)));
                _sampleDataCustomer.Columns.Add(new DataColumn("AmountDebitCold", typeof(decimal)));

                _sampleDataCustomer.Columns.Add(new DataColumn("TotalDebit", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("TotalCredit", typeof(string)));

                _sampleDataCustomer.Columns.Add(new DataColumn("BalanceDebitEnd", typeof(string)));
                _sampleDataCustomer.Columns.Add(new DataColumn("BalanceCreditEnd", typeof(string)));


                InitialFiveRows(_sampleData, 1);           
            }
            catch 
            {
            }            
        }
        #region Functions

        void makeGridBind(DataTable dt)
        {
            DataView dv = dt.DefaultView;
            _sampleData = dt;
            gridControl1.DataSource = dt;
        }

        private DataTable GetEmptyDataTable()
        {
            strSQL = "SELECT 0 AS n_invoice_serial,'' AS Balance,'' AS Debit,'' AS Credit,'' AS Declaration,'' AS TheDate,'' AS OppsiteAccountName,"
            + " '' AS RecordType,'' AS ID,'' AS TempRecordType,'' AS RegTime FROM ACC_ACCOUNTS WHERE 1=2";
            //strSQL = Comon.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, lang);


            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            return dt;

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

        void GetDatasource()
        {

            DataTable dt = _sampleData.Clone();
            dt = InitialFiveRows(dt, 2);

            makeGridBind(dt);

        }

        private void TotalsAllCustomers()
        {
            try
            {
                decimal total = 0;
                decimal credit = 0;
                decimal debit = 0;

                decimal CreditGold = 0;
                decimal DebitGold = 0;

                decimal DebitDiamond = 0;
                decimal CreditDiamond = 0;

                decimal sum = 0;
                decimal BalanceDebitEnd = 0;
                decimal BalanceCreditEnd = 0;
                decimal DebitBalance = 0;
                decimal CreditBalance = 0;
                decimal BalanceType = 0;

                decimal TotalCredit = 0;
                decimal TotalDebit = 0;
                decimal AmountCrditDiamond = 0;
                decimal AmountDebitDiamond = 0;
                decimal AmountDebitCold = 0;
                decimal AmountCrditGold = 0;
                DataRow row;

                for (int i = 0; i <= _sampleDataCustomer.Rows.Count - 1; i++)
                {
                    credit += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["Credit"]));
                    debit += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["Debit"]));


                    CreditGold += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["CreditGold"]));
                    DebitGold += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["DebitGold"]));

                    DebitDiamond += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["DebitDiamond"]));
                    CreditDiamond += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["CreditDiamond"]));


                    AmountCrditDiamond += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["AmountCrditDiamond"]));
                    AmountDebitDiamond += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["AmountDebitDiamond"]));

                    AmountDebitCold += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["AmountDebitCold"]));
                    AmountCrditGold += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["AmountCrditGold"]));

                    DebitBalance += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["DebitBalance"]));
                    CreditBalance += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["CreditBalance"]));

                    BalanceDebitEnd += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["BalanceDebitEnd"]));
                    BalanceCreditEnd += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["BalanceCreditEnd"]));

                    TotalCredit += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["TotalCredit"]));
                    TotalDebit += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["TotalDebit"]));
                    

                    _sampleDataCustomer.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["TotalDebit"])) - (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["TotalCredit"]));
                    sum = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["Balance"]);
                }
                total = Comon.ConvertToDecimalPrice(( DebitBalance) - (CreditBalance));

                row = _sampleDataCustomer.NewRow();
                row["Debit"] = debit;
                row["Credit"] = credit;

                row["CreditGold"] = CreditGold;
                row["DebitGold"] = DebitGold;

                row["DebitDiamond"] = DebitDiamond;
                row["CreditDiamond"] = CreditDiamond;
                row["AmountCrditDiamond"] = AmountCrditDiamond;
                row["AmountDebitDiamond"] = AmountDebitDiamond;
                row["AmountCrditGold"] = AmountCrditGold;
                row["AmountDebitCold"] = AmountDebitCold;
                row["Balance"] = Math.Abs(total).ToString();
                row["BalanceDebitEnd"] = BalanceDebitEnd;
                row["BalanceCreditEnd"] = BalanceCreditEnd;
                row["TotalCredit"] = TotalCredit;
                row["TotalDebit"] = TotalDebit;
                row["DebitBalance"] = DebitBalance;
                row["CreditBalance"] = CreditBalance;
                row["n_invoice_serial"] = 0;

                if (total > 0)
                {
                    lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "  مدين");
                    row["BalanceType"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "  مدين");
                }
                else
                {
                    lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Credit" : "  دائن");
                    row["BalanceType"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "  مدين");

                }
                _sampleDataCustomer.Rows.Add(row);
                //------------------
                lblDebit.Text = (TotalDebit).ToString();
                lblCredit.Text =(TotalCredit).ToString();
                lblBalanceSum.Text = Math.Abs(TotalDebit - TotalCredit).ToString();
                //btnShow.Visible = true;
            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        public void Show(string message)
        {


        }

        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                bool IncludeHeader = true;
                ReportName = "rptBalanceReview";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                ReportName = "rptBalanceReview";
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /***************** Master *****************************/
               
                rptForm.RequestParameters = false;
                //rptForm.Parameters["MainAccountID"].Value = txtAccountID.Text.Trim().ToString();
                //rptForm.Parameters["MainAccountName"].Value = lblAccountName.Text.Trim().ToString();
                //rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["TotalDebit"].Value = lblDebit.Text.Trim().ToString();
                rptForm.Parameters["TotalCredit"].Value = lblCredit.Text.Trim().ToString();
                rptForm.Parameters["TotalBalance"].Value = lblBalanceSum.Text.Trim().ToString();
                ///
                rptForm.Parameters["TotalCredit"].Visible = false;
                rptForm.Parameters["FromAccountID"].Value = txtFromAccountID.Text.Trim().ToString();
                rptForm.Parameters["ToAccountID"].Value = txtToAccountID.Text.Trim().ToString();
                rptForm.Parameters["FromAccountName"].Value = lblFromAccountID.Text.Trim().ToString();
                rptForm.Parameters["ToAccountName"].Value = lblToAccountID.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = cmbBranchesID.Text.Trim().ToString();
                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                
                /********************** Details ****************************/
                var dataTable = new dsReports.rptBalanceReviewDataTable();

                for (int i = 0; i < GridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["n_invoice_serial"] = i + 1;
                    row["Balance"] = GridView1.GetRowCellValue(i, "Balance").ToString();
                    row["OppsiteAccountName"] = GridView1.GetRowCellValue(i, "CustomerName").ToString();
                    row["BalanceType"] = GridView1.GetRowCellValue(i, "BalanceType").ToString();

                    row["Debit"] = GridView1.GetRowCellValue(i, "Debit").ToString();
                    row["Credit"] = GridView1.GetRowCellValue(i, "Credit").ToString();
                    if (chkViewWeght.Checked)
                    {
                        row["DebitGold"] = GridView1.GetRowCellValue(i, "DebitGold").ToString();
                        row["CreditGold"] = GridView1.GetRowCellValue(i, "CreditGold").ToString();
                        row["DebitDiamond"] = GridView1.GetRowCellValue(i, "DebitDiamond").ToString();
                        row["CreditDiamond"] = GridView1.GetRowCellValue(i, "CreditDiamond").ToString();


                        row["AmountDebitCold"] = GridView1.GetRowCellValue(i, "AmountDebitCold").ToString();
                        row["AmountCrditGold"] = GridView1.GetRowCellValue(i, "AmountCrditGold").ToString();
                        row["AmountDebitDiamond"] = GridView1.GetRowCellValue(i, "AmountDebitDiamond").ToString();
                        row["AmountCrditDiamond"] = GridView1.GetRowCellValue(i, "AmountCrditDiamond").ToString();
                    }
                    else
                    {
                        row["DebitGold"] ="-";
                        row["CreditGold"] ="-";
                        row["DebitDiamond"] = "-";
                        row["CreditDiamond"] = "-";

                        row["AmountDebitCold"] = "-";
                        row["AmountCrditGold"] = "-";
                        row["AmountDebitDiamond"] = "-";
                        row["AmountCrditDiamond"] = "-";
                    }

                    row["DebitBalance"] = GridView1.GetRowCellValue(i, "DebitBalance").ToString();
                    row["CreditBalance"] = GridView1.GetRowCellValue(i, "CreditBalance").ToString();
                    row["TotalDebit"] = GridView1.GetRowCellValue(i, "BalanceDebitEnd").ToString();
                    row["TotalCredit"] = GridView1.GetRowCellValue(i, "BalanceCreditEnd").ToString();
                    //row["BalanceDebitEnd"] = GridView1.GetRowCellValue(i, "TotalDebit").ToString();
                    //row["BalanceCreditEnd"] = GridView1.GetRowCellValue(i, "TotalCredit").ToString();
                    row["BalanceType"] = GridView1.GetRowCellValue(i, "BalanceType").ToString();
                    // row["TheDate"] = GridView1.GetRowCellValue(i, "TheDate").ToString();
                    row["ID"] = GridView1.GetRowCellValue(i, "AccountID").ToString();
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
        protected override void DoAddFrom()
        {
            try
            {
                cmbLevelAccounts.ItemIndex = MySession.GlobalNoOfLevels-1;
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtFromAccountID.Text = "";
                txtToAccountID.Text = "";
                lblFromAccountID.Text = "";
                lblToAccountID.Text = "";
                txtFromAccountID.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";
                txtFromAccountID.Enabled = true;
                txtToAccountID.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtCostCenterID.Enabled = true;
                btnCostCenterSearch.Enabled = true;
                btnFromAcountID.Enabled = true;
                btnToAcountID.Enabled = true;
                gridControl1.DataSource = _sampleData;
            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void OpenWindow(BaseForm frm)
        {
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        public void ProcessFromDateToDate(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                
            double BeforeBalance = 0;
                double BeforeDebit = 0;
                double BeforeCredit = 0;
                string BeforeBalanceType = "";

                double periodBalance = 0;
                double periodDebit = 0;
                double periodCredit = 0;
                string periodBalanceType = "";
                long tempFromDate = FromDate;

                _sampleData.Rows.Clear();
                
                VariousVoucherMachin(AccountID, FromDate, ToDate);
                
                 Totals();
                //FilteringData(FromDate, ToDate);
                 
            }
            catch { }

        }
        private void VariousVoucherMachin(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                string strSQL = null; DataRow row;
                //strSQL = "SELECT Acc_VariousVoucherMachinDetails.Declaration, Acc_VariousVoucherMachinMaster.VoucherDate AS TheDate, Acc_VariousVoucherMachinMaster.VoucherID" + " AS ID, 'VariousVoucher' AS RecordType, ' ' AS OppsiteAccountName, Acc_VariousVoucherMachinDetails.AccountID, Acc_VariousVoucherMachinDetails.Debit, Acc_VariousVoucherMachinMaster.RegTime, " + " Acc_VariousVoucherMachinDetails.Credit FROM Acc_VariousVoucherMachinMaster INNER JOIN Acc_VariousVoucherMachinDetails ON Acc_VariousVoucherMachinMaster.VoucherID" + " = Acc_VariousVoucherMachinDetails.VoucherID AND Acc_VariousVoucherMachinMaster.BranchID = Acc_VariousVoucherMachinDetails.BranchID " + " WHERE (Acc_VariousVoucherMachinMaster.Cancel = 0) AND (Acc_VariousVoucherMachinMaster.BranchID = " + WT.GlobalBranchID + ")" + " AND (Acc_VariousVoucherMachinDetails.AccountID = " + txtAccountID.TextWT + ") ";
                strSQL = "SELECT Acc_VariousVoucherMachinMaster.BranchID, Acc_VariousVoucherMachinDetails.DECLARATION,Acc_VariousVoucherMachinMaster.VOUCHERDATE AS TheDate,Acc_VariousVoucherMachinMaster.VOUCHERID AS ID,"
                + " 'VariousVoucher' AS RecordType, ' ' AS OppsiteAccountName,Acc_VariousVoucherMachinDetails.ACCOUNTID,Acc_VariousVoucherMachinDetails.DEBIT,"
                + " Acc_VariousVoucherMachinMaster.RegTime,Acc_VariousVoucherMachinDetails.CREDIT,Acc_VariousVoucherMachinDetails.DebitGold,Acc_VariousVoucherMachinDetails.CreditGold,Acc_VariousVoucherMachinDetails.DebitDiamond,Acc_VariousVoucherMachinDetails.CreditDiamond FROM Acc_VariousVoucherMachinMaster INNER JOIN Acc_VariousVoucherMachinDetails"
                + " ON Acc_VariousVoucherMachinMaster.VOUCHERID= Acc_VariousVoucherMachinDetails.VOUCHERID AND Acc_VariousVoucherMachinMaster.BranchID= Acc_VariousVoucherMachinDetails.BranchID"
                + " AND Acc_VariousVoucherMachinMaster.FacilityID  = Acc_VariousVoucherMachinDetails.FacilityID WHERE Acc_VariousVoucherMachinDetails.ACCOUNTID = " + AccountID
                + " AND Acc_VariousVoucherMachinMaster.CANCEL = 0  ";
                if (Comon.cInt(cmbBranchesID.EditValue) == 0)
                    strSQL += " AND Acc_VariousVoucherMachinMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                else
                    strSQL += " AND Acc_VariousVoucherMachinMaster.BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + " AND Acc_VariousVoucherMachinMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                if (FromDate!=0)
                {

                    strSQL = strSQL + " AND  Acc_VariousVoucherMachinMaster.VoucherDate >=" + FromDate;
                }
                if (ToDate != 0)
                {

                    strSQL = strSQL + " AND  Acc_VariousVoucherMachinMaster.VoucherDate <=" + ToDate;
                }

                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_VariousVoucherMachinDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }


                strSQL = strSQL + " ORDER BY Acc_VariousVoucherMachinMaster.VoucherDate,Acc_VariousVoucherMachinMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtCredit = Lip.SelectRecord(strSQL);
                if (dtCredit.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                     
                        row["BranchID"] =  dtCredit.Rows[i]["BranchID"].ToString() ;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مذكورين" : "Mentioned");
                        row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                        row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "قيد يومي" : "Various Voucher");
                        row["ID"] = dtCredit.Rows[i]["ID"];
                        row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                        row["Credit"] =Comon.ConvertToDecimalPrice( dtCredit.Rows[i]["Credit"]);
                        row["Debit"] =Comon.ConvertToDecimalPrice( dtCredit.Rows[i]["Debit"]);

                        row["DebitGold"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["DebitGold"]);
                        row["CreditGold"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["CreditGold"]);

                        
                        row["DebitDiamond"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["DebitDiamond"]);
                        row["CreditDiamond"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["CreditDiamond"]);
                        _sampleData.Rows.Add(row);
                    }
                }
                dtCredit.Dispose();
                row = null;
            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

       
        private void Totals()
        {
            try
            {
                decimal total = 0;
                decimal totalGold = 0;
                decimal totalDiamond = 0;
                decimal credit = 0;
                decimal debit = 0;

                decimal DebitGold = 0;
                decimal CreditGold = 0;

                decimal DebitDiamond = 0;
                decimal CreditDiamond = 0;
                decimal sum = 0;
                DataRow row;
                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                {
                    credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                    debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));

                    DebitGold += (Comon.ConvertToDecimalQty(_sampleData.Rows[i]["DebitGold"]));
                    CreditGold += (Comon.ConvertToDecimalQty(_sampleData.Rows[i]["CreditGold"]));

                    DebitDiamond += (Comon.ConvertToDecimalQty(_sampleData.Rows[i]["DebitDiamond"]));
                    CreditDiamond += (Comon.ConvertToDecimalQty(_sampleData.Rows[i]["CreditDiamond"]));

                    _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));

                    _sampleData.Rows[i]["BalanceGold"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));


                    _sampleData.Rows[i]["BalanceDiamond"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditDiamond"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitDiamond"]));
                    sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                }
          
                total = credit - debit;
                totalGold = CreditGold - DebitGold;
                totalDiamond = CreditDiamond - DebitDiamond;
                row = _sampleData.NewRow();
                row["Debit"] = debit;
                row["Credit"] = credit;


                row["DebitGold"] = DebitGold;
                row["CreditGold"] = CreditGold;


                row["DebitDiamond"] = DebitDiamond;
                row["CreditDiamond"] = CreditDiamond;


                row["Balance"] = Math.Abs(total).ToString();


                row["BalanceGold"] = Math.Abs(totalGold).ToString();



                row["BalanceDiamond"] = Math.Abs(totalDiamond).ToString();
                 
                
                
                row["n_invoice_serial"] = 0;
                _sampleData.Rows.Add(row);



            }
            catch { }

        }

        private DataRow TotalsRow()
        {
            DataRow row;
            row = _sampleData.NewRow();
            decimal total = 0; decimal credit = 0; decimal debit = 0; decimal rowcredit = 0; decimal rowdebit = 0; decimal sum = 0;

            try
            {

                for (int i = 0; i < _sampleData.Rows.Count - 1; i++)
                {
                    rowcredit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                    rowdebit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                }

                credit = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["Credit"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["Credit"]));
                debit = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["Debit"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["Debit"]));
                total = credit - debit;

                row["Debit"] = debit;
                row["Credit"] = credit;
                row["Balance"] = Math.Abs(total).ToString();

                if (total < 0)
                {
                    row["Declaration"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    row["Declaration"] = (UserInfo.Language == iLanguage.Arabic ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }


                row["n_invoice_serial"] = _sampleData.Rows.Count + 1;

                lblDebit.Text = debit.ToString();
                lblCredit.Text = credit.ToString();
                lblBalanceSum.Text = Math.Abs(total).ToString();

                if (total < 0)
                {
                    lblBalanceType.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceType.Text = (UserInfo.Language == iLanguage.Arabic ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

            }
            catch { }
            return row;
        }



        #endregion

        #region Process
        public void addEvenRow()
        {
            DataRow row;
            row = _sampleData.NewRow();

            row["TheDate"] = null;
            row["OppsiteAccountName"] = null;
            row["RecordType"] = null;
            row["ID"] = null;
            row["Debit"] = 0;
            row["Balance"] = 0;
            row["Credit"] = 0;
            row["Declaration"] = (lang == "Eng" ? "Open Balance" : "الرصـيد حتى بـداية الـمـدة");
            _sampleData.Rows.Add(row);

        }
        private void FilteringData(long FromDate, long ToDate)
        {
            try
            {
                //string strFilter = "";
                DataRow row;
                decimal total = 0;
                decimal credit = 0;
                decimal debit = 0;
                decimal sum = 0;

                int endDAte = -1;

                if (FromDate != 0 && _sampleData.Rows.Count > 0)
                {
                    int index = -1;
                    if (ToDate == 0)
                        ToDate = Comon.cLong((Lip.GetServerDateSerial()));
                    for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                    {

                        string SearchDate = Comon.cStr(Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()))).ToString();
                        if (SearchDate == FromDate.ToString())
                        {
                            index = i;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                        else
                        {

                            credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                            debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                            sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);

                        }
                    }
                    int keys = 1;
                    if (_sampleData.Rows.Count > 1)
                        keys = 2;
                    if (index == -1)
                    {

                        if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[_sampleData.Rows.Count - keys]["TheDate"].ToString())) < FromDate)
                        {
                            _sampleData.Rows.Clear();
                            addEvenRow();
                            addEvenRow();
                            return;
                        }
                        else if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[0]["TheDate"].ToString())) > FromDate)
                        {
                            if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[0]["TheDate"].ToString())) > ToDate)
                            {
                                _sampleData.Rows.Clear();
                                addEvenRow();
                                addEvenRow();
                                return;
                            }
                            else
                            {
                                index = 0;
                            }

                        }
                        else
                        {

                            total = 0;
                            credit = 0;
                            debit = 0;
                            sum = 0;

                            for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                            {

                                //string SearchDate = Comon.cStr(Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()))).ToString();
                                if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString())) > FromDate)
                                {
                                    index = i;
                                    break; // TODO: might not be correct. Was : Exit For
                                }

                                credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                                debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                                sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);


                            }






                        }



                    }
                    if (ToDate != 0)
                    {

                        for (int i = index; i <= _sampleData.Rows.Count - 1; i++)
                        {

                            long SearchDate = Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()));
                            if (SearchDate > ToDate)
                            {
                                endDAte = i - 1;
                                break; // TODO: might not be correct. Was : Exit For
                            }

                        }

                    }
                    else ToDate = Comon.cLong((Lip.GetServerDateSerial()));
                    DataTable dt = new DataTable();
                    dt = _sampleData.Clone();
                    int x = 0;
                    int y = endDAte;
                    if (endDAte == -1)
                    {

                        if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[_sampleData.Rows.Count - keys]["TheDate"].ToString())) <= ToDate)
                        {
                            y = _sampleData.Rows.Count - keys;
                        }
                        else if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[0]["TheDate"].ToString())) > ToDate)
                        {

                            _sampleData.Rows.Clear();

                            return;


                        }
                        else
                        {

                            total = 0;
                            credit = 0;
                            debit = 0;
                            sum = 0;

                            for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                            {

                                //string SearchDate = Comon.cStr(Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()))).ToString();
                                if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString())) > ToDate)
                                {
                                    y = i - 1;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                          }
                          }


                    }

                    total = 0;
                    credit = 0;
                    debit = 0;
                    sum = 0;
                    for (int k = index; k <= y; k++)
                    {
                        if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[k]["TheDate"].ToString())) > ToDate)
                            break;
                        dt.Rows.Add();
                        dt.Rows[x]["Balance"] = _sampleData.Rows[k]["Balance"];
                        dt.Rows[x]["Debit"] = _sampleData.Rows[k]["Debit"];
                        dt.Rows[x]["Credit"] = _sampleData.Rows[k]["Credit"];
                        dt.Rows[x]["TheDate"] = _sampleData.Rows[k]["TheDate"];
                        dt.Rows[x]["OppsiteAccountName"] = _sampleData.Rows[k]["OppsiteAccountName"];
                        dt.Rows[x]["RecordType"] = _sampleData.Rows[k]["RecordType"];
                        dt.Rows[x]["ID"] = _sampleData.Rows[k]["ID"];
                        dt.Rows[x]["Declaration"] = _sampleData.Rows[k]["Declaration"];
                        dt.Rows[x]["TempRecordType"] = _sampleData.Rows[k]["TempRecordType"];
                        x += 1;
                    }

                    for (int i = 0; i <= index - 1; i++)
                    {

                        credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                        debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                        sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);

                    }
                    _sampleData.Rows.Clear();
                    total = credit - debit;


                    row = _sampleData.NewRow();
                    row["Debit"] = debit;
                    row["Credit"] = credit;
                    row["Balance"] = Math.Abs(total).ToString();
                    row["TheDate"] = null;
                    row["OppsiteAccountName"] = null;
                    row["RecordType"] = null;
                    row["ID"] = null;
                    row["Declaration"] = (lang == "Eng" ? "Open Balance" : "الرصـيد حتى بـداية الـمـدة");
                    _sampleData.Rows.Add(row);
                    decimal balance = 0;
                    total = 0;
                    credit = 0;
                    debit = 0;
                    sum = 0;
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        row = _sampleData.NewRow();
                        row["Debit"] = dt.Rows[i]["Debit"];
                        row["Credit"] = dt.Rows[i]["Credit"];
                        row["Declaration"] = dt.Rows[i]["Declaration"];
                        row["TheDate"] = dt.Rows[i]["TheDate"];
                        row["OppsiteAccountName"] = dt.Rows[i]["OppsiteAccountName"];
                        row["RecordType"] = dt.Rows[i]["RecordType"];
                        row["ID"] = dt.Rows[i]["ID"];
                        row["TempRecordType"] = dt.Rows[i]["TempRecordType"];
                        row["Balance"] = Math.Abs(Comon.ConvertToDecimalPrice(dt.Rows[i]["Balance"]));
                        balance += (Comon.ConvertToDecimalPrice(row["Balance"]));
                        credit += (Comon.ConvertToDecimalPrice(row["Credit"]));
                        debit += (Comon.ConvertToDecimalPrice(row["Debit"]));
                        row["Balance"] = credit - debit;
                        sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                        _sampleData.Rows.Add(row);

                    }
                    total = credit - debit;
                    row = _sampleData.NewRow();
                    row["Debit"] = debit;
                    row["Credit"] = credit;
                    row["Balance"] = Math.Abs(total).ToString();
                    row["TheDate"] = null;
                    row["OppsiteAccountName"] = null;
                    row["RecordType"] = null;
                    row["ID"] = null;
                    row["Declaration"] = (lang == "Eng" ? "End Balance" : "الرصـيد حتى نهاية المدة الـمـدة");
                    _sampleData.Rows.Add(row);

                    //_sampleData.Rows[0]["Debit"] = null;
                    //_sampleData.Rows[0]["Credit"] = null;

                    dt.Dispose();
                    row = null;
                }
                else
                {
                    if (FromDate == 0)
                    {
                        return;
                    }
                    addEvenRow();
                    addEvenRow();

                }
            }
            catch { }
        }
     
        #endregion

        #region Event

        private void btnShow_Click(object sender, EventArgs e)
        {
 
            try
            {
                ProgressBar.Value = 0;
                ProgressBar.Visible = true;
                long AccountID = 0;
                lblDebit.Text = "0";
                lblCredit.Text = "0";
                lblBalanceSum.Text = "0";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                int GlobalNoOfLevels = MySession.GlobalNoOfLevels;
                 
                DataTable dtCustomer = new DataTable();
                if (Comon.cInt(cmbBranchesID.EditValue) == 0)
                    strSQL = "SELECT AccountID," + PrimaryName + " as AccountName,BranchID FROM Acc_Accounts WHERE Cancel=0   AND AccountLevel=" + GlobalNoOfLevels;
                else
                    strSQL = "SELECT AccountID," + PrimaryName + " as AccountName,BranchID FROM Acc_Accounts WHERE Cancel=0 And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + GlobalNoOfLevels;

                if (checkFinancialPositionStatement.Checked == true && checkIncomeStatement.Checked == false &&Comon.cInt(cmbBranchesID.EditValue) != 0)
                    strSQL = "SELECT AccountID," + PrimaryName + " as AccountName,BranchID FROM Acc_Accounts WHERE Cancel=0 and (SUBSTRING(CAST(Acc_Accounts.AccountID AS VARCHAR(10)),1,1)= 1 OR SUBSTRING(CAST(Acc_Accounts.AccountID AS VARCHAR(10)),1,1)= 2) And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + GlobalNoOfLevels;
                else
                    if (checkFinancialPositionStatement.Checked == true && checkIncomeStatement.Checked == false && Comon.cInt(cmbBranchesID.EditValue) == 0)
                        strSQL = "SELECT AccountID," + PrimaryName + " as AccountName,BranchID FROM Acc_Accounts WHERE Cancel=0 and (SUBSTRING(CAST(Acc_Accounts.AccountID AS VARCHAR(10)),1,1)= 1 OR SUBSTRING(CAST(Acc_Accounts.AccountID AS VARCHAR(10)),1,1)= 2)   AND AccountLevel=" + GlobalNoOfLevels;

                if (checkFinancialPositionStatement.Checked == false && checkIncomeStatement.Checked == true &&Comon.cInt(cmbBranchesID.EditValue) != 0)
                    strSQL = "SELECT AccountID," + PrimaryName + " as AccountName,BranchID FROM Acc_Accounts WHERE Cancel=0 and (SUBSTRING(CAST(Acc_Accounts.AccountID AS VARCHAR(10)),1,1)= 3 OR SUBSTRING(CAST(Acc_Accounts.AccountID AS VARCHAR(10)),1,1)= 4) And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + GlobalNoOfLevels;
                else
                    if (checkFinancialPositionStatement.Checked == false && checkIncomeStatement.Checked == true && Comon.cInt(cmbBranchesID.EditValue) == 0)
                        strSQL = "SELECT AccountID," + PrimaryName + " as AccountName,BranchID FROM Acc_Accounts WHERE Cancel=0 and (SUBSTRING(CAST(Acc_Accounts.AccountID AS VARCHAR(10)),1,1)= 3 OR SUBSTRING(CAST(Acc_Accounts.AccountID AS VARCHAR(10)),1,1)= 4)   AND AccountLevel=" + GlobalNoOfLevels;
                 
                if (txtFromAccountID.Text != string.Empty && txtToAccountID.Text != string.Empty)
                {
                    strSQL = strSQL + "    And  (AccountID>= " + Comon.cDbl(txtFromAccountID.Text) + ") And (AccountID<= " + Comon.cDbl(txtToAccountID.Text) + ")";
                }
                dtCustomer = Lip.SelectRecord(strSQL);

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                dtCustomer = Lip.SelectRecord(strSQL);
                //if (dtCustomer.Rows.Count > 0)
                //    btnShow.Visible = false;

                gridControl1.Visible = false;

                Application.DoEvents();
                _sampleDataCustomer.Clear();
                gridControl1.DataSource = _sampleDataCustomer;
                gridControl1.RefreshDataSource();
                #region GetBalanceCustomer
                ProgressBar.Visible = true;

                ProgressBar.Visible = true;
                ProgressBar.Maximum = dtCustomer.Rows.Count;
                ProgressBar.Minimum = 0;
                ProgressBar.Value = 0;
                string BranchID = "";
                for (int i = 0; i <= dtCustomer.Rows.Count - 1; i++)
                {
                    ProgressBar.Value = ProgressBar.Value + 1;
                    AccountID = Comon.cLong(dtCustomer.Rows[i]["AccountID"].ToString());
                    BranchID = dtCustomer.Rows[i]["BranchID"].ToString();
                    long FromDate1 = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                    long ToDate1 = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                    ProcessFromDateToDate(AccountID.ToString(), FromDate, ToDate);
                    //lblBalanceType.Text = dtCustomer.Rows[i][1].ToString();
                   
                    decimal total = 0;
                    if (_sampleData.Rows.Count > 1)
                    {

                        if (Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["Balance"].ToString()) > 0 || Comon.ConvertToDecimalQty(_sampleData.Rows[_sampleData.Rows.Count - 1]["BalanceGold"].ToString()) > 0 || Comon.ConvertToDecimalQty(_sampleData.Rows[_sampleData.Rows.Count - 1]["BalanceDiamond"].ToString()) > 0)
                        {
                            _sampleDataCustomer.NewRow();
                            _sampleDataCustomer.Rows.Add();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["AccountID"] = AccountID.ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BranchID"] = BranchID.ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CustomerName"] = dtCustomer.Rows[i][1].ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Balance"] = _sampleData.Rows[_sampleData.Rows.Count - 1]["Balance"].ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"] = _sampleData.Rows[_sampleData.Rows.Count - 1]["Debit"].ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"] = _sampleData.Rows[_sampleData.Rows.Count - 1]["Credit"].ToString();
                            if (chkViewWeght.Checked)
                            {
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitGold"] = _sampleData.Rows[_sampleData.Rows.Count - 1]["DebitGold"].ToString();
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditGold"] = _sampleData.Rows[_sampleData.Rows.Count - 1]["CreditGold"].ToString();

                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitDiamond"] = _sampleData.Rows[_sampleData.Rows.Count - 1]["DebitDiamond"].ToString();
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditDiamond"] = _sampleData.Rows[_sampleData.Rows.Count - 1]["CreditDiamond"].ToString();
                            }
                            total = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"].ToString()) - Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"].ToString());
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["AccountBalance"] = total.ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "...";
                            if (Comon.cInt(cmbBranchesID.EditValue) == 0)
                            {
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitBalance"] = Comon.ConvertToDecimalPrice(Lip.GetValue("Select sum(Debit) From Acc_VariousVoucherDetails Where VoucherID =0 And  AccountID=" + AccountID ));
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditBalance"] = Comon.ConvertToDecimalPrice(Lip.GetValue("Select sum(Credit) From Acc_VariousVoucherDetails Where VoucherID =0 And  AccountID=" + AccountID  ));
                            }
                            else
                            {
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitBalance"] = Comon.ConvertToDecimalPrice(Lip.GetValue("Select sum(Debit) From Acc_VariousVoucherDetails Where VoucherID =0 And  AccountID=" + AccountID + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditBalance"] = Comon.ConvertToDecimalPrice(Lip.GetValue("Select sum(Credit) From Acc_VariousVoucherDetails Where VoucherID =0 And  AccountID=" + AccountID + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));
                            }

                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["TotalDebit"] = Comon.cDec(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"]) + Comon.cDec(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitBalance"]);
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["TotalCredit"] = Comon.cDec(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"]) + Comon.cDec(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditBalance"]);

                            decimal totalAll = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["TotalDebit"].ToString()) - Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["TotalCredit"].ToString());

                            if (totalAll > 0)
                            {
                                if (UserInfo.
                                Language == iLanguage.English)
                                    _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "Debit";
                                else
                                    _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "مدين";
                            }
                            else
                            {
                                if (UserInfo.
                                    Language == iLanguage.English)
                                    _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "Credit";
                                else
                                    _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "دائن";

                            }
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["n_invoice_serial"] = (i + 1).ToString();
                        }
                        else
                        {
                            decimal DebitBalance = 0;
                            decimal CreditBalance = 0;                        
                            {
                                DebitBalance = Comon.ConvertToDecimalPrice(Lip.GetValue("Select sum(Debit) From Acc_VariousVoucherDetails Where TypeOpration=1 And  AccountID=" + AccountID + " And BranchID=" + Comon.cInt(BranchID)));
                                CreditBalance = Comon.ConvertToDecimalPrice(Lip.GetValue("Select sum(Credit) From Acc_VariousVoucherDetails Where TypeOpration=1 And  AccountID=" + AccountID + " And BranchID=" + Comon.cInt(BranchID)));
                            }
                            _sampleDataCustomer.NewRow();
                            _sampleDataCustomer.Rows.Add();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["AccountID"] = AccountID.ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BranchID"] = BranchID.ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CustomerName"] = dtCustomer.Rows[i][1].ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Balance"] = "0";
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"] = "0";
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"] = "0";

                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitGold"] = "0";
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditGold"] = "0";

                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditDiamond"] = "0";
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitDiamond"] = "0";

                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "...";
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["AccountBalance"] = 0.ToString();
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitBalance"] = DebitBalance;
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditBalance"] = CreditBalance;
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["TotalDebit"] = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"]) + Comon.cDec(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitBalance"]);
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["TotalCredit"] = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"]) + Comon.cDec(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditBalance"]);
                            _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["n_invoice_serial"] = (i + 1).ToString();

                        }
                    }
                }

                for (int i = 0; i <= _sampleDataCustomer.Rows.Count - 1; i++)
                {
                   decimal total = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["TotalDebit"].ToString()) - Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["TotalCredit"].ToString());
                   if(total>0)
                    {
                        _sampleDataCustomer.Rows[i]["BalanceDebitEnd"] = total;
                        _sampleDataCustomer.Rows[i]["BalanceCreditEnd"] = 0;
                    }
                   else
                    {
                        _sampleDataCustomer.Rows[i]["BalanceDebitEnd"] = 0;
                        _sampleDataCustomer.Rows[i]["BalanceCreditEnd"] = -total;
                    }
                } 
                #endregion
                
               
                ProgressBar.Value = 0;
                //ProgressBar.Visible = false;
                //txtFromAccountID.Enabled = false;
                //txtToAccountID.Enabled = false;
                //txtFromDate.Enabled = false;
                //txtToDate.Enabled = false;
                //txtCostCenterID.Enabled = false;
                //btnFromAcountID.Enabled = false;
                //btnToAcountID.Enabled = false;
                gridControl1.Visible = true;

                int LevelAccount = Comon.cInt(cmbLevelAccounts.EditValue);
               
                if (LevelAccount > 0)
                {
                    dttem = _sampleDataCustomer.Copy();
                    FilterBylevel(LevelAccount);
                }

                 

                gridControl1.DataSource = _sampleDataCustomer;
                gridControl1.RefreshDataSource();
                TotalsAllCustomers();

            }
            catch(Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void FilterBylevel(int LevelAccount)
        {
            DataTable dt = null;

            strSQL = "SELECT AccountID," + PrimaryName + " as AccountName,BranchID,0 As DebitBefore,0 As CreditBefore,0 As BalanceBefore,"
                + " 0 As DebitPeriod,0 As CreditPeriod, 0 As BalancePeriod,"
                + " 0 As DebitTotal,0 As CreditTotal,0 As BalanceTotal";
            if (Comon.cInt(cmbBranchesID.EditValue) != 0)
                strSQL += " FROM Acc_Accounts WHERE Cancel=0 And BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " AND AccountLevel=" + LevelAccount;
            else
                strSQL += " FROM Acc_Accounts WHERE Cancel=0 And   AccountLevel=" + LevelAccount;
            dt = Lip.SelectRecord(strSQL);
            _sampleDataCustomer.Rows.Clear();
     
            DataRow row;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = _sampleDataCustomer.NewRow();
                row["AccountID"] = dt.Rows[i]["AccountID"];
                row["CustomerName"] = dt.Rows[i]["AccountName"];
                row["BranchID"] = dt.Rows[i]["BranchID"];
                row["Debit"] = 0;
                row["Credit"] = 0;
                row["DebitGold"] = 0;
                row["CreditGold"] = 0;
                row["DebitDiamond"] = 0;
                row["CreditDiamond"] = 0;
                row["Balance"] = 0;
                row["BalanceDebitEnd"] = 0;
                row["BalanceCreditEnd"] = 0;
                row["TotalCredit"] = 0;
                row["TotalDebit"] = 0;
                row["DebitBalance"] = 0;
                row["CreditBalance"] = 0;
                row["n_invoice_serial"] = 0;
                row["BalanceType"] = "...";
                _sampleDataCustomer.Rows.Add(row);
            }
            for (int i = 0; i <= dttem.Rows.Count - 1; i++)
            {

                for (int j = 0; j <= _sampleDataCustomer.Rows.Count - 1; j++)
                {
                    string AccountID = _sampleDataCustomer.Rows[j]["AccountID"].ToString() ;
                    string AccountIDGrid = dttem.Rows[i]["AccountID"].ToString() ;

                    if (AccountID.Trim()!=""&&AccountID == AccountIDGrid)
                    {  
                        _sampleDataCustomer.Rows[j]["Debit"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["Debit"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["Debit"].ToString()));
                        _sampleDataCustomer.Rows[j]["Credit"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["Credit"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["Credit"].ToString()));
                     
                        if (chkViewWeght.Checked == true)
                        {
                            _sampleDataCustomer.Rows[j]["DebitGold"] = Comon.ConvertToDecimalQty(Comon.ConvertToDecimalQty(_sampleDataCustomer.Rows[j]["DebitGold"]) + Comon.ConvertToDecimalQty(dttem.Rows[i]["DebitGold"].ToString()));
                            _sampleDataCustomer.Rows[j]["CreditGold"] = Comon.ConvertToDecimalQty(Comon.ConvertToDecimalQty(_sampleDataCustomer.Rows[j]["CreditGold"]) + Comon.ConvertToDecimalQty(dttem.Rows[i]["CreditGold"].ToString()));

                            _sampleDataCustomer.Rows[j]["DebitDiamond"] = Comon.ConvertToDecimalQty(Comon.ConvertToDecimalQty(_sampleDataCustomer.Rows[j]["DebitDiamond"]) + Comon.ConvertToDecimalQty(dttem.Rows[i]["DebitDiamond"].ToString()));
                            _sampleDataCustomer.Rows[j]["CreditDiamond"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalQty(_sampleDataCustomer.Rows[j]["CreditDiamond"]) + Comon.ConvertToDecimalQty(dttem.Rows[i]["CreditDiamond"].ToString()));
                        
                            _sampleDataCustomer.Rows[j]["AmountDebitCold"] =Comon.ConvertToDecimalPrice( Comon.ConvertToDecimalPrice(txtPriceGoldToday.Text) * Comon.ConvertToDecimalQty(_sampleDataCustomer.Rows[j]["DebitGold"]));
                            _sampleDataCustomer.Rows[j]["AmountCrditGold"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtPriceGoldToday.Text) * Comon.ConvertToDecimalQty(_sampleDataCustomer.Rows[j]["CreditGold"]));
                            _sampleDataCustomer.Rows[j]["AmountDebitDiamond"] =Comon.ConvertToDecimalPrice( Comon.ConvertToDecimalPrice(txtPriceDiamondToday.Text) * Comon.ConvertToDecimalQty(_sampleDataCustomer.Rows[j]["DebitDiamond"]));
                            _sampleDataCustomer.Rows[j]["AmountCrditDiamond"] =Comon.ConvertToDecimalPrice( Comon.ConvertToDecimalPrice(txtPriceDiamondToday.Text) * Comon.ConvertToDecimalQty(_sampleDataCustomer.Rows[j]["CreditDiamond"]));
                        }
                        _sampleDataCustomer.Rows[j]["Balance"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["Balance"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["Balance"].ToString()));
                        _sampleDataCustomer.Rows[j]["DebitBalance"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["DebitBalance"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["DebitBalance"].ToString()));

                        _sampleDataCustomer.Rows[j]["CreditBalance"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["CreditBalance"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["CreditBalance"].ToString()));
                        _sampleDataCustomer.Rows[j]["AccountBalance"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["AccountBalance"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["AccountBalance"].ToString()));
                         _sampleDataCustomer.Rows[j]["TotalDebit"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["TotalDebit"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["TotalDebit"].ToString()));
                        _sampleDataCustomer.Rows[j]["TotalCredit"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["TotalCredit"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["TotalCredit"].ToString()));

                        if (chkViewWeght.Checked )
                        {

                            _sampleDataCustomer.Rows[j]["TotalDebit"] = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["TotalDebit"]) + Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["AmountDebitCold"]) + Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["AmountDebitDiamond"]);
                            _sampleDataCustomer.Rows[j]["TotalCredit"] = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["TotalCredit"]) + Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["AmountCrditGold"]) + Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["AmountCrditDiamond"]);
                        }
                            _sampleDataCustomer.Rows[j]["BalanceDebitEnd"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["BalanceDebitEnd"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["BalanceDebitEnd"].ToString()));
                        _sampleDataCustomer.Rows[j]["BalanceCreditEnd"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["BalanceCreditEnd"]) + Comon.ConvertToDecimalPrice(dttem.Rows[i]["BalanceCreditEnd"].ToString()));
                         
                        decimal totalAll = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["TotalDebit"]) - Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[j]["TotalCredit"]);
                        if (totalAll > 0)
                        {
                            if (UserInfo.Language == iLanguage.English)
                                _sampleDataCustomer.Rows[j]["BalanceType"] = "Debit";
                            else
                                _sampleDataCustomer.Rows[j]["BalanceType"] = "مدين";
                        }
                        else
                        {

                            if (UserInfo.
                                Language == iLanguage.English)
                                _sampleDataCustomer.Rows[j]["BalanceType"] = "Credit";
                            else
                                _sampleDataCustomer.Rows[j]["BalanceType"] = "دائن";

                        }
                    }
                }
            }
            for (int i = 0; i <= _sampleDataCustomer.Rows.Count - 1; i++)
            {
                decimal total = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["BalanceDebitEnd"].ToString()) - Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["BalanceCreditEnd"].ToString());
                if (total > 0)
                {
                    _sampleDataCustomer.Rows[i]["BalanceDebitEnd"] = total;
                    _sampleDataCustomer.Rows[i]["BalanceCreditEnd"] = 0;
                }
                else
                {
                    _sampleDataCustomer.Rows[i]["BalanceDebitEnd"] = 0;
                    _sampleDataCustomer.Rows[i]["BalanceCreditEnd"] = -total;
                }
            }
            DataTable temp2 = new DataTable();
            temp2 = _sampleDataCustomer.Copy();
            _sampleDataCustomer.Rows.Clear();
            for (int i = 0; i < temp2.Rows.Count; i++)
            {
                row = _sampleDataCustomer.NewRow();
                row["AccountID"] = temp2.Rows[i]["AccountID"];
                row["CustomerName"] = temp2.Rows[i]["CustomerName"];
                row["BranchID"] = temp2.Rows[i]["BranchID"];
                row["Debit"] = temp2.Rows[i]["Debit"];
                row["Credit"] = temp2.Rows[i]["Credit"];
                row["Balance"] = temp2.Rows[i]["Balance"];
                if (chkViewWeght.Checked)
                {
                    row["CreditGold"] = temp2.Rows[i]["CreditGold"];
                    row["DebitGold"] = temp2.Rows[i]["DebitGold"];

                    row["DebitDiamond"] = temp2.Rows[i]["DebitDiamond"];
                    row["CreditDiamond"] = temp2.Rows[i]["CreditDiamond"];
                    row["AmountDebitCold"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtPriceGoldToday.Text) * Comon.ConvertToDecimalQty(row["DebitGold"]));
                    row["AmountCrditGold"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtPriceGoldToday.Text) * Comon.ConvertToDecimalQty(row["CreditGold"]));
                    row["AmountDebitDiamond"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtPriceDiamondToday.Text) * Comon.ConvertToDecimalQty(row["DebitDiamond"]));
                    row["AmountCrditDiamond"] = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtPriceDiamondToday.Text) * Comon.ConvertToDecimalQty(row["CreditDiamond"]));
                }
                row["BalanceDebitEnd"] = temp2.Rows[i]["BalanceDebitEnd"];
                row["BalanceCreditEnd"] = temp2.Rows[i]["BalanceCreditEnd"];
                row["TotalCredit"] = temp2.Rows[i]["TotalCredit"];
                row["TotalDebit"] = temp2.Rows[i]["TotalDebit"];
                if (chkViewWeght.Checked)
                {
                    row["TotalCredit"] = Comon.ConvertToDecimalPrice(temp2.Rows[i]["TotalCredit"])+Comon.ConvertToDecimalPrice(row["AmountCrditGold"])+Comon.ConvertToDecimalPrice(row["AmountCrditDiamond"]);
                    row["TotalDebit"] =Comon.ConvertToDecimalPrice(temp2.Rows[i]["TotalDebit"] )+ Comon.ConvertToDecimalPrice(row["AmountDebitCold"])+Comon.ConvertToDecimalPrice(row["AmountDebitDiamond"]);
                }
                row["DebitBalance"] = temp2.Rows[i]["DebitBalance"];
                row["CreditBalance"] = temp2.Rows[i]["CreditBalance"];
                row["n_invoice_serial"] = temp2.Rows[i]["n_invoice_serial"];
                row["BalanceType"] = temp2.Rows[i]["BalanceType"];

                decimal BalanceDebitEnd = Comon.ConvertToDecimalPrice(row["BalanceDebitEnd"]);
                decimal BalanceCreditEnd = Comon.ConvertToDecimalPrice(row["BalanceCreditEnd"]);

                if(BalanceDebitEnd > 0 || BalanceCreditEnd > 0||Comon.ConvertToDecimalQty( row["DebitDiamond"] )>0||Comon.ConvertToDecimalQty(row["CreditDiamond"] )>0
                    ||Comon.ConvertToDecimalQty(row["CreditGold"] )>0||Comon.ConvertToDecimalQty(row["DebitGold"])>0)
                _sampleDataCustomer.Rows.Add(row);
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DoPrint();
        }

        private void btnCostCenterSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة");
                else
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID");

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridColumn col;
                {
                    col = GridView1.Columns[1]; ;
                    var cellValue = GridView1.GetRowCellValue(GridView1.FocusedRowHandle, col);
                    if (cellValue != null)
                    {
                        frmAccountStatement frm = new frmAccountStatement();
                        OpenWindow(frm);
                        frm.txtAccountID.Text = cellValue.ToString();
                        frm.txtAccountID_Validating(null, null);
                        frm.cmbBranchesID.EditValue = cmbBranchesID.EditValue;
                        frm.btnShow_Click(null, null);
                    }
                }
            }
            catch { }
        }

        private void btnFromAcountID_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(txtFromAccountID, lblFromAccountID);
                txtFromAccountID_Validating(null, null);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        private void btnToAcountID_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(txtToAccountID, lblToAccountID);
                txtToAccountID_Validating(null, null);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void chkSupliar_CheckedChanged(object sender, EventArgs e)
        {

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
        #endregion

        private void txtFromAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Comon.cInt(cmbBranchesID.EditValue) == 0)
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE     (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtFromAccountID.Text) + ") ";
            else
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtFromAccountID.Text) + ") ";
                CSearch.ControlValidating(txtFromAccountID, lblFromAccountID, strSQL);
                if (GridView1.DataRowCount > 0)
                    btnShow_Click(null, null);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtToAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Comon.cInt(cmbBranchesID.EditValue) == 0)
                    strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtToAccountID.Text) + ") ";
                else
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtToAccountID.Text) + ") ";
                CSearch.ControlValidating(txtToAccountID, lblToAccountID, strSQL);
                if (GridView1.DataRowCount > 0)
                    btnShow_Click(null, null);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void cmbLevelAccounts_EditValueChanged(object sender, EventArgs e)
        {
            int LevelAccount = Comon.cInt(cmbLevelAccounts.EditValue);
            {
                FilterBylevel(LevelAccount);
                TotalsAllCustomers();
            }
        }

        private void chkViewWeght_CheckedChanged(object sender, EventArgs e)
        {
            if (chkViewWeght.Checked == false)
            {
                GridView1.Columns["DebitGold"].Visible = false;
                GridView1.Columns["CreditGold"].Visible = false;
                GridView1.Columns["DebitDiamond"].Visible = false;
                GridView1.Columns["CreditDiamond"].Visible = false;
                GridView1.Columns["AmountDebitCold"].Visible = false;
                GridView1.Columns["AmountCrditGold"].Visible = false;
                GridView1.Columns["AmountDebitDiamond"].Visible = false;
                GridView1.Columns["AmountCrditDiamond"].Visible = false;
            }
            else
            {
                GridView1.Columns["DebitGold"].Visible = true;
                GridView1.Columns["CreditGold"].Visible = true;
                GridView1.Columns["DebitDiamond"].Visible = true;
                GridView1.Columns["CreditDiamond"].Visible = true;
                GridView1.Columns["AmountDebitCold"].Visible = true;
                GridView1.Columns["AmountCrditGold"].Visible = true;
                GridView1.Columns["AmountDebitDiamond"].Visible = true;
                GridView1.Columns["AmountCrditDiamond"].Visible = true;
                GridView1.Columns["DebitGold"].VisibleIndex = GridView1.Columns["Credit"].VisibleIndex + 1;
                GridView1.Columns["CreditGold"].VisibleIndex = GridView1.Columns["DebitGold"].VisibleIndex + 1;
                GridView1.Columns["DebitDiamond"].VisibleIndex = GridView1.Columns["CreditGold"].VisibleIndex + 1;
                GridView1.Columns["CreditDiamond"].VisibleIndex = GridView1.Columns["DebitDiamond"].VisibleIndex + 1;
                GridView1.Columns["AmountDebitCold"].VisibleIndex = GridView1.Columns["CreditDiamond"].VisibleIndex + 1;
                GridView1.Columns["AmountCrditGold"].VisibleIndex = GridView1.Columns["AmountDebitCold"].VisibleIndex + 1;
                GridView1.Columns["AmountDebitDiamond"].VisibleIndex = GridView1.Columns["AmountCrditGold"].VisibleIndex + 1;
                GridView1.Columns["AmountCrditDiamond"].VisibleIndex = GridView1.Columns["AmountDebitDiamond"].VisibleIndex + 1;
            }
            int LevelAccount = Comon.cInt(cmbLevelAccounts.EditValue);
            {
                FilterBylevel(LevelAccount);
                TotalsAllCustomers();
            }
        }

        private void txtFromDate_Validating(object sender, CancelEventArgs e)
        {
            if (GridView1.DataRowCount > 0)
                btnShow_Click(null, null);
        }

        private void txtToDate_Validating(object sender, CancelEventArgs e)
        {
            if (GridView1.DataRowCount > 0)
                btnShow_Click(null, null);
        }

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            if (GridView1.DataRowCount > 0)
                btnShow_Click(null, null);
        }

         
    }
}


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
using DevExpress.XtraGrid.Views.Grid;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.StockObjects.Transactions;
using Edex.StockObjects.Codes;
using Edex.SalesAndSaleObjects.Transactions;
using Edex.AccountsObjects.Transactions;
using Edex.Manufacturing.Codes;
using Edex.Manufacturing;
namespace Edex.AccountsObjects.Reports
{
    public partial class frmAccountStatement : Edex.GeneralObjects.GeneralForms.BaseForm
    {
      
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string FocusedControl = "";
        private string PrimaryName;
        DataTable dtFactoryOprationType = new DataTable();
        public DataTable _sampleData = new DataTable();
        public frmAccountStatement(long AccountNO) {
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
                PrimaryName = "ArbName";
                GridView1.OptionsView.EnableAppearanceEvenRow = true;
                GridView1.OptionsView.EnableAppearanceOddRow = true;
                GridView1.OptionsBehavior.ReadOnly = true;
                GridView1.OptionsBehavior.Editable = false;
                dgvColOppsiteAccountName.Visible = false;
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                dgvColOppsiteAccountName.Visible = false;
                if (UserInfo.Language == iLanguage.English)
                {
                    dgvColOppsiteAccountName.Caption = "Oppsite Account Name ";
                    dgvColTheDate.Caption = "The Date";
                    dgvColDeclaration.Caption = "Declaration ";
                    dgvColCredit.Caption = "Credit";
                    dgvColDebit.Caption = "Debit  ";

                    dgvColCreditGold.Caption = "Credit Gold";
                    dgvColDebitGold.Caption = "Debit  Gold";

                    dvgColCreditDiamond.Caption = "Credit Diamond";
                    dvgColDiamond.Caption="Debit Diamond";
                    dgvColn_invoice_serial.Caption = "# ";
                    dgvColBalance.Caption = "Balance";

                    dgvColRecordType.Caption = "Record Type ";
                    dgvColID.Caption = "ID";
                    dgvColTempRecordType.Caption = "Total  Quntity ";
                    dgvColRegTime.Caption = "RegTime";
                    btnShow.Text = "show";
                    //  Label8.Text = btnShow.Tag.ToString();
                }
                 where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + Comon.cInt(cmbBranchesID.EditValue);
                _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Balance", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));
        
                _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));

                _sampleData.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));

                _sampleData.Columns.Add(new DataColumn("CreditDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("DebitDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("BalanceDiamond", typeof(decimal)));

                _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Posted", typeof(string)));

              
               
                InitialFiveRows(_sampleData, 1);
                DoNew();
                txtAccountID.Text = AccountNO.ToString();
                txtAccountID_Validating(null, null);
                txtFromDate.Text = "";
                txtToDate.Text = " ";
                btnShow_Click(null, null);

            }
            catch { }
        }
        public frmAccountStatement(long AccountNO, bool yes)
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
                PrimaryName = "ArbName";
                GridView1.OptionsView.EnableAppearanceEvenRow = true;
                GridView1.OptionsView.EnableAppearanceOddRow = true;
                GridView1.OptionsBehavior.ReadOnly = true;
                GridView1.OptionsBehavior.Editable = false;
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                dgvColOppsiteAccountName.Visible = false;
                if (UserInfo.Language == iLanguage.English)
                {
                    dgvColOppsiteAccountName.Caption = "Oppsite Account Name ";
                    dgvColTheDate.Caption = "The Date";
                    dgvColDeclaration.Caption = "Declaration ";
                    dgvColCredit.Caption = "Credit";
                    dgvColDebit.Caption = "Debit  ";
                    dgvColn_invoice_serial.Caption = "# ";
                    dgvColBalance.Caption = "Balance";
                    dgvColRecordType.Caption = "Record Type ";
                    dgvColID.Caption = "ID";
                    dgvColTempRecordType.Caption = "Total  Quntity ";
                    dgvColRegTime.Caption = "RegTime";
                    btnShow.Text = "show";
                    //  Label8.Text = btnShow.Tag.ToString();

                }
                 where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + Comon.cInt(cmbBranchesID.EditValue);
                _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Balance", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));

                _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("DebitDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("CreditDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("BalanceDiamond", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Posted", typeof(string)));
                InitialFiveRows(_sampleData, 1);
                DoNew();
                txtAccountID.Text = AccountNO.ToString();
                txtAccountID_Validating(null, null);
                txtFromDate.Text = "";
                txtToDate.Text = " ";
                btnShow_Click(null, null);
            }
            catch { }
        }
        public frmAccountStatement()
        {
            try
            {
                InitializeComponent();
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
                PrimaryName = "ArbName";
                GridView1.OptionsView.EnableAppearanceEvenRow = true;
                GridView1.OptionsView.EnableAppearanceOddRow = true;
                GridView1.OptionsBehavior.ReadOnly = true;
                GridView1.OptionsBehavior.Editable = false;
                dgvColOppsiteAccountName.Visible = false;
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                if (UserInfo.Language == iLanguage.English)
                {
                    PrimaryName = "EngName";
                    dgvColOppsiteAccountName.Caption = "Oppsite Account Name ";
                    dgvColTheDate.Caption = "The Date";
                    dgvColDeclaration.Caption = "Declaration ";
                    dgvColCredit.Caption = "Credit";
                    dgvColDebit.Caption = "Debit  ";
                    dgvColn_invoice_serial.Caption = "# ";
                    dgvColBalance.Caption = "Balance";
                    dgvColRecordType.Caption = "Record Type ";
                    dgvColID.Caption = "ID";
                    dgvColTempRecordType.Caption = "Total  Quntity ";
                    dgvColRegTime.Caption = "RegTime";
                    btnShow.Text = "show";
                    //  Label8.Text = btnShow.Tag.ToString();
                }
                DoAddFrom();
                FormsPrperties.PropertiesGridView(GridView1, this.Name); 
            }
            catch { }
        }

        private void frmAccountStatement_Load(object sender, EventArgs e)
        {
            try{

                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"),NameCol:UserInfo.Language==iLanguage.Arabic?"الكل ":"All");
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;

                FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;

             where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + Comon.cInt(cmbBranchesID.EditValue);
            _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Balance", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("DebitDiamond", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("CreditDiamond", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("BalanceDiamond", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Posted", typeof(string)));
            InitialFiveRows(_sampleData, 1);

            if (UserInfo.ID == 1)
            {
                cmbBranchesID.Visible = true;
                labelControl9.Visible = true;
            }

            else
            {
                cmbBranchesID.Visible = false;
                labelControl9.Visible = false;
            } 
             
            }
            catch { }
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "BranchID="+Comon.cInt(cmbBranchesID.EditValue), (UserInfo.Language == iLanguage.English ? "Select " : "حدد العملة "));
           
        }
  
        void makeGridBind(DataTable dt)
        {
            DataView dv = dt.DefaultView;
            _sampleData = dt;
            gridControl1.DataSource = dt;
        }
        private DataTable GetEmptyDataTable()
        {
            strSQL = "SELECT 0 AS n_invoice_serial,'' AS Balance,'' AS DebitGold,'' AS CreditGold,'','' AS Debit,'' AS Credit,'' AS Declaration,'' AS TheDate,'' AS OppsiteAccountName,"
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
        public void Show(string message)
        {

        }
        protected override void DoPrint()
        {
            try
            {
                if ( GridView1.GetRowCellValue(0, "ID").ToString() == "" && (GridView1.GetRowCellValue(0, "Credit").ToString() == ""||GridView1.GetRowCellValue(0, "Debit").ToString() == ""))
                {
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                ReportName = "‏‏‏‏rptAccountStatementGeneral";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["BranchName"].Value = cmbBranchesID.Text.Trim().ToString();

                rptForm.Parameters["MainAccountID"].Value = txtAccountID.Text.Trim().ToString();
                rptForm.Parameters["MainAccountName"].Value = lblAccountName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.ToString();
                //Comon.cDec(lblDebitGold.Text) +
                //Comon.cDec(lblCreditGold.Text) +
                //Comon.cDec(lblBalanceSumGold.Text) +
                rptForm.Parameters["TotalDebit"].Value =(  Comon.cDec(lblDebit.Text)).ToString();
                rptForm.Parameters["TotalCredit"].Value = ( Comon.cDec(lblCredit.Text)).ToString(); 
                
                rptForm.Parameters["TotalBalance"].Value = ( Comon.cDec(lblBalanceSum.Text)).ToString();
               
                rptForm.Parameters["TotalDebit1"].Value = lblDebitGold.Text.Trim().ToString();
                rptForm.Parameters["TotalCredit1"].Value = lblCreditGold.Text.Trim().ToString();
                rptForm.Parameters["TotalBalance1"].Value = lblBalanceSumGold.Text.Trim().ToString();

                rptForm.Parameters["TotalDebit2"].Value =lblDebitDiamond.Text.Trim().ToString();
                rptForm.Parameters["TotalCredit2"].Value =lblCreditDiamond.Text.Trim().ToString();
                rptForm.Parameters["TotalBalance2"].Value =lblBalanceSumDiamond.Text.Trim().ToString();

                rptForm.Parameters["CurrentBalance"].Value = lblBalanceTypeGold.Text.Trim().ToString();
                rptForm.Parameters["CurrentBalance1"].Value =lblBalanceTypeDiamond.Text.Trim().ToString();
                rptForm.Parameters["EndBalance"].Value = lblBalanceType.Text.Trim().ToString();

                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                for (int i = 0; i <= rptForm.Parameters.Count - 1; i++)
                {
                    rptForm.Parameters[i].Visible = false;
                }
                /********************** Details ****************************/
                var dataTable = new dsReports.rptAccountStatementDataTable();
                try
                {
                    for (int i = 1; i < GridView1.DataRowCount - 2; i++)
                    {
                        var row = dataTable.NewRow();
                        row["n_invoice_serial"] = i + 1;
                      
                        row["Credit"] = GridView1.GetRowCellValue(i, "Credit").ToString();
                        row["Debit"] =   GridView1.GetRowCellValue(i, "Debit").ToString();
                        row["Balance"] = GridView1.GetRowCellValue(i, "Balance").ToString();
                        row["DebitGold"] = GridView1.GetRowCellValue(i, "DebitGold").ToString();
                        row["CreditGold"] = GridView1.GetRowCellValue(i, "CreditGold").ToString();
                        //row["DebitDiamond"] = GridView1.GetRowCellValue(i, "DebitDiamond").ToString();
                        //row["CreditDiamond"] = GridView1.GetRowCellValue(i, "CreditDiamond").ToString();
                        row["BalanceGold"] = GridView1.GetRowCellValue(i, "BalanceGold").ToString();
                        //row["BalanceDiamond"] = GridView1.GetRowCellValue(i, "BalanceDiamond").ToString();
                        row["OppsiteAccountName"] = GridView1.GetRowCellValue(i, "OppsiteAccountName").ToString() + "(" + GridView1.GetRowCellValue(i, "RecordType").ToString() + ")";
                        row["TheDate"] = GridView1.GetRowCellValue(i, "TheDate").ToString();
                        row["ID"] = GridView1.GetRowCellValue(i, "ID").ToString();
                        row["Declaration"] = GridView1.GetRowCellValue(i, "Declaration").ToString();
                        dataTable.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptAccountStatement";
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                rptForm.RequestParameters = false;
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
        protected override void DoSearch()
        {
            try
            {
                txtAccountID.Focus();
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #region Other Function
        public void Find()
        {
            try
            {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;
            if (FocusedControl.Trim() == txtAccountID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            { 
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            GetSelectedSearchValue(cls);
            }
            catch { }
        }
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c == null) return null;
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }
            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }
            return c.Name;
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtAccountID.Name)
                {
                    txtAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountID_Validating(null, null);
                }

                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
            }
        }
        public void RemoveRecordsWithZeroCreditAndDebit()
        {
            try
            {
                for (int i = _sampleData.Rows.Count - 1; i >= 0; i += -1)
                {
                    if (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]) == 0)
                    {
                        if (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]) == 0)
                        {
                            if (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]) == 0)
                            {
                                if (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]) == 0)
                                {
                                    if (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitDiamond"]) == 0)
                                    {
                                        if (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditDiamond"]) == 0)
                                        {
                                            _sampleData.Rows.RemoveAt(i);
                                        }
                                    }
                                }

                               
                            }
                        }
                    }
                }
            }
            catch { }
        }
        public void GetAccountsDeclaration()
        {
            #region get accounts declaration
            DataTable dtDeclaration;
            List<Acc_DeclaringMainAccounts> AllAccounts = new List<Acc_DeclaringMainAccounts>();
            int BRANCHID = UserInfo.BRANCHID;
            if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                  BRANCHID = Comon.cInt(cmbBranchesID.EditValue);
            
           
            int FacilityID = UserInfo.FacilityID;

            dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(BRANCHID, FacilityID);
            if (dtDeclaration != null && dtDeclaration.Rows.Count > 0)
            {
                //حساب الصندوق
                DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
                if (row.Length > 0)
                {
                    txtAccountID.Text = row[0]["AccountID"].ToString();
                    lblAccountName.Text = row[0]["AccountName"].ToString();
                }
            }
            #endregion
        }
        private void SortData()
        { 
            try
            {
                DataTable dt = new DataTable(); DataRow row;
                dt = _sampleData.Copy();
                DataView view = dt.DefaultView;
                view.Sort = "TheDate ASC, RegTime ASC";
                _sampleData.Rows.Clear();
                for (int i = 0; i <= view.Count - 1; i++)
                {
                    row = _sampleData.NewRow();
                    row["n_invoice_serial"] = i + 1;
                    row["Debit"] = view[i]["Debit"];
                    row["Credit"] = view[i]["Credit"];
                    row["DebitGold"] = view[i]["DebitGold"];
                    row["CreditGold"] = view[i]["CreditGold"];

                    row["DebitDiamond"] = view[i]["DebitDiamond"];
                    row["CreditDiamond"] = view[i]["CreditDiamond"];
                   
                    row["Declaration"] = view[i]["Declaration"];
                    row["TempRecordType"] = view[i]["TempRecordType"];
                    row["TheDate"] = Comon.ConvertSerialDateTo(view[i]["TheDate"].ToString()); 
                    row["OppsiteAccountName"] = view[i]["OppsiteAccountName"];
                    row["ID"] = view[i]["ID"];
                    if (row["TempRecordType"].ToString() == "VariousVoucher")
                    { 
                    if(Comon.cInt(row["ID"].ToString())==0)
                        row["TempRecordType"] = "OpeningVoucher";
                    }
                    row["RecordType"] = view[i]["RecordType"];
                    row["RegTime"] = view[i]["RegTime"];
                    row["Posted"] = view[i]["Posted"];
                    _sampleData.Rows.Add(row);
                }
                if (_sampleData.Rows.Count > 0)
                {
                    for (int i = 0; i <= view.Count - 1; i++)
                    {
                        _sampleData.Rows[i]["Balance"] = Math.Abs(Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]));
                    }
                }
            }
            catch { }
        }
        private void Totals()
        {
            try
            {
                decimal total = 0;
                decimal credit = 0;
                decimal debit = 0;
                decimal sum = 0;

                decimal totalGold = 0;
                decimal creditGold = 0;
                decimal debitGold = 0;
                decimal sumGold = 0;

                DataRow row;

                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                {
                    credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                    debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));

                    creditGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));
                    debitGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));

                    _sampleData.Rows[i]["BalanceGold"] = sumGold + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
            
                    _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                    sumGold = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["BalanceGold"]);


                }
                total = credit - debit;
                totalGold = creditGold - debitGold;

                row = _sampleData.NewRow();
                row["Debit"] = debit;
                row["Credit"] = credit;
                row["Balance"] = Math.Abs(total).ToString();

                row["DebitGold"] = debitGold;
                row["CreditGold"] = creditGold;
                row["BalanceGold"] = Math.Abs(totalGold).ToString();


                if (total < 0)
                {
                    row["Declaration"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    row["Declaration"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");

                }

               


                row["n_invoice_serial"] = 0;
                _sampleData.Rows.Add(row);


                //------------------
                if (total < 0)
                {
                    lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

                if (totalGold < 0)
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

                lblDebit.Text = debit.ToString();
                lblCredit.Text = credit.ToString();
                lblBalanceSum.Text = Math.Abs(total).ToString();

                lblDebitGold.Text = debitGold.ToString();
                lblCreditGold.Text = creditGold.ToString();
                lblBalanceSumGold.Text = Math.Abs(totalGold).ToString();

             
            }
            catch { }

        }
        private DataRow TotalsRow()
        {
            DataRow row;
            row = _sampleData.NewRow();
            decimal total = 0; decimal credit = 0; decimal debit = 0; decimal rowcredit = 0; decimal rowdebit = 0; decimal sum = 0;
            decimal totalGold = 0; decimal creditGold = 0; decimal debitGold = 0; decimal rowcreditGold = 0; decimal rowdebitGold = 0; decimal sumGold = 0;
            decimal totalDiamond = 0; decimal creditDiamond = 0; decimal debitDiamond = 0; decimal rowCreditDiamond = 0; decimal rowDebitDiamond = 0; decimal sumDiamond = 0;
            try
            {

                for (int i = 0; i < _sampleData.Rows.Count - 1; i++)
                {
                    rowcredit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                    rowdebit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                  
                    rowcreditGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));
                    rowdebitGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));

                    rowCreditDiamond += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditDiamond"]));
                    rowDebitDiamond += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitDiamond"]));

                    _sampleData.Rows[i]["BalanceGold"] = sumGold + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                    _sampleData.Rows[i]["BalanceDiamond"] = sumDiamond + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditDiamond"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitDiamond"]));

                    _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                    sumGold = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["BalanceGold"]);
                    sumDiamond = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["BalanceDiamond"]);

                }

                credit = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["Credit"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["Credit"]));
                debit = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["Debit"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["Debit"]));

                creditGold = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["CreditGold"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["CreditGold"]));
                debitGold = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["DebitGold"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["DebitGold"]));


                creditDiamond = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["CreditDiamond"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["CreditDiamond"]));
                debitDiamond = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["DebitDiamond"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["DebitDiamond"]));

                total = credit - debit;
                totalGold = creditGold - debitGold;
                totalDiamond = creditDiamond - debitDiamond;

                row["Debit"] = debit;
                row["Credit"] = credit;
                row["Balance"] = Math.Abs(total).ToString();

                row["DebitGold"] = debitGold;
                row["CreditGold"] = creditGold;
                row["BalanceGold"] = Math.Abs(totalGold).ToString();

                row["CreditDiamond"] = debitDiamond;
                row["DebitDiamond"] = creditDiamond;
                row["BalanceDiamond"] = Math.Abs(totalDiamond).ToString();


                if (total < 0)
                {
                    row["Declaration"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    row["Declaration"] = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

                if (totalGold < 0)
                {
                    row["Declaration"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    row["Declaration"] = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

                if (totalDiamond < 0)
                {
                    row["Declaration"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    row["Declaration"] = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

                row["n_invoice_serial"] = _sampleData.Rows.Count + 1;

                lblDebit.Text = debit.ToString();
                lblCredit.Text = credit.ToString();
                lblBalanceSum.Text = Math.Abs(total).ToString();

                lblDebitGold.Text = debitGold.ToString();
                lblCreditGold.Text = creditGold.ToString();
                lblBalanceSumGold.Text = Math.Abs(totalGold).ToString();

                lblDebitDiamond.Text = debitDiamond.ToString();
                lblCreditDiamond.Text = creditDiamond.ToString();
                lblBalanceSumDiamond.Text = Math.Abs(totalDiamond).ToString();
                if (total < 0)
                {
                    lblBalanceType.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceType.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }
                //if(txtAccountID.Text== "12010000002")
                if (totalGold < 0)
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }
                //if (txtAccountID.Text == "12010000004")
                if (totalDiamond < 0)
                {
                    lblBalanceTypeDiamond.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceTypeDiamond.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }


            }
            catch { }
            return row;
        }
        protected override void DoAddFrom()
        {

            try
            {
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtAccountID.Text = "";
                lblAccountName.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";
                lblCredit.Text = "";
                lblCreditGold.Text = "";
                lblDebit.Text = "";
                lblDebitGold.Text = "";
                lblBalanceSum.Text = "";
                lblBalanceSumGold.Text = "";
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtAccountID.Enabled = true;
                txtFromDate.Enabled = true;
                btnCostCenterSearch.Enabled = true;
                btnDebitSearch.Enabled = true;
                btnCostCenterSearch.Enabled = true;
                txtCostCenterID.Enabled = true;
                txtAccountID.Focus();
                DataTable dtDeclaration;
                int BRANCHID = UserInfo.BRANCHID;
                if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                    BRANCHID = Comon.cInt(cmbBranchesID.EditValue);

                int FacilityID = UserInfo.FacilityID;
                List<Acc_DeclaringMainAccounts> AllAccounts = new List<Acc_DeclaringMainAccounts>();
                dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(BRANCHID, FacilityID);

                DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
                if (row.Length > 0)
                {
                    txtAccountID.Text = row[0]["AccountID"].ToString();
                    lblAccountName.Text = row[0]["AccountName"].ToString();
                }
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
            Obj.EditValue = DateTime.Now;
        }

      #endregion
        #region ProcessData

         
        public void ProcessWithOutDate(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                double BeforeBalance = 0;
                double BeforeDebit = 0;
                double BeforeCredit = 0;
                string BeforeBalanceType = "";

                double BeforeBalanceGold = 0;
                double BeforeDebitGold = 0;
                double BeforeCreditGold = 0;

                double BeforeBalanceDiamond = 0;
                double BeforeDebitDiamond = 0;
                double BeforeCreditDiamond = 0;

                string BeforeBalanceTypeGold = "";

                double periodBalance = 0;
                double periodDebit = 0;
                double periodCredit = 0;
                string periodBalanceType = "";
                long tempFromDate = FromDate;

                double periodBalanceGold = 0;
                double periodDebitGold = 0;
                double periodCreditGold = 0;

                double periodBalanceDiamond = 0;
                double periodDebitDiamond = 0;
                double periodCreditDiamond = 0;


                string periodBalanceTypeGold = "";


                ProgressBar.Visible = true;
                ProgressBar.Maximum = 170;
                ProgressBar.Minimum = 0;
                _sampleData.Rows.Clear();

 
                 VariousVoucherMachin(AccountID, FromDate, ToDate);

                ProgressBar.Value = ProgressBar.Value + 10;

                RemoveRecordsWithZeroCreditAndDebit();

                SortData();
                ProgressBar.Value = ProgressBar.Value + 10;
               // Totals();
                ProgressBar.Value = ProgressBar.Value + 10;

                //_sampleData.Rows.RemoveAt(_sampleData.Rows.Count - 1);
                //Totals();

               // FilteringData(FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 1;

                for (int i = 0; i <= _sampleData.Rows.Count - 2; i++)
                {
                    _sampleData.Rows[i]["Balance"] = Comon.ConvertToDecimalPrice(Math.Abs(Comon.cDbl(_sampleData.Rows[i]["Balance"])));
                    _sampleData.Rows[i]["BalanceGold"] = Comon.ConvertToDecimalPrice(Math.Abs(Comon.cDbl(_sampleData.Rows[i]["BalanceGold"])));
                    _sampleData.Rows[i]["BalanceDiamond"] = Comon.ConvertToDecimalPrice(Math.Abs(Comon.cDbl(_sampleData.Rows[i]["BalanceDiamond"])));
                }

                int inc = 0;
                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                {
                    if (Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()) < tempFromDate && Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()) != 0)
                    {
                        inc = inc + 1;
                        BeforeDebit = Comon.cDbl(Comon.ConvertToDecimalPrice(BeforeDebit) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                        BeforeCredit = Comon.cDbl(Comon.ConvertToDecimalPrice(BeforeCredit) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                        BeforeBalance = BeforeDebit - BeforeCredit;

                        BeforeDebitGold = Comon.cDbl(Comon.ConvertToDecimalPrice(BeforeDebitGold) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                        BeforeCreditGold = Comon.cDbl(Comon.ConvertToDecimalPrice(BeforeCreditGold) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));
                        BeforeBalanceGold = BeforeDebitGold - BeforeCreditGold;


                        BeforeDebitDiamond = Comon.cDbl(Comon.ConvertToDecimalPrice(BeforeDebitDiamond) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitDiamond"]));
                        BeforeCreditDiamond = Comon.cDbl(Comon.ConvertToDecimalPrice(BeforeCreditDiamond) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditDiamond"]));
                        BeforeBalanceDiamond = BeforeDebitDiamond - BeforeCreditDiamond;



                        if (BeforeDebit >= BeforeCredit)
                        {
                            BeforeBalanceType = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "الرصيد حتى بداية المدة مدين" : "Begin Balance Period Is Debit");
                        }
                        else
                        {
                            BeforeBalanceType = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "الرصيد حتى بداية المدة دائن" : "Begin Balance Period Is Credit");
                        }

                        //if (txtAccountID.Text == "12010000002")

                            if (BeforeDebitGold >= BeforeCreditGold)
                            {
                                BeforeBalanceType = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "الرصيد حتى بداية المدة مدين" : "Begin Balance Period Is Debit");
                            }
                            else
                            {
                                BeforeBalanceType = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "الرصيد حتى بداية المدة دائن" : "Begin Balance Period Is Credit");
                            }

                        //if (txtAccountID.Text == "12010000004")

                            if (BeforeDebitDiamond >= BeforeCreditDiamond)
                            {
                                BeforeBalanceType = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "الرصيد حتى بداية المدة مدين" : "Begin Balance Period Is Debit");
                            }
                            else
                            {
                                BeforeBalanceType = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "الرصيد حتى بداية المدة دائن" : "Begin Balance Period Is Credit");
                            }
                    }
                }

                while (inc > 0)
                {
                    _sampleData.Rows.RemoveAt(inc - 1);
                    inc = inc - 1;
                }
                 

                DataRow dr = _sampleData.NewRow();
                inc = 0;

                for (int i = _sampleData.Rows.Count - 1; i > 0; i--)
                {
                    if (ToDate>0 && Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()) > ToDate && Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()) != 0)
                    { 
                        inc = inc + 1;
                        _sampleData.Rows.RemoveAt(i);
                    }
                }
                //dr["Balance"] = BeforeBalance;
                dr["Debit"] = BeforeDebit;
                dr["Credit"] = BeforeCredit;

                dr["DebitGold"] = BeforeDebitGold;
                dr["CreditGold"] = BeforeCreditGold;

                dr["DebitDiamond"] = BeforeDebitDiamond;
                dr["CreditDiamond"] = BeforeCreditDiamond;

                dr["Declaration"] = BeforeBalanceType;
                dr["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                _sampleData.Rows.InsertAt(dr, 0);
                //رصيد الفترة من دون اول المدة
                for (int i = 1; i < _sampleData.Rows.Count; i++)
                {
                    if (Comon.cDbl(_sampleData.Rows[i]["ID"].ToString()) != 0)
                    {
                        periodDebit = Comon.cDbl(Comon.ConvertToDecimalPrice(periodDebit) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                        periodCredit = Comon.cDbl(Comon.ConvertToDecimalPrice(periodCredit) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));

                        periodDebitGold = Comon.cDbl(Comon.ConvertToDecimalPrice(periodDebitGold) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                        periodCreditGold = Comon.cDbl(Comon.ConvertToDecimalPrice(periodCreditGold) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));


                        periodDebitDiamond = Comon.cDbl(Comon.ConvertToDecimalPrice(periodDebitDiamond) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitDiamond"]));
                        periodCreditDiamond = Comon.cDbl(Comon.ConvertToDecimalPrice(periodCreditDiamond) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditDiamond"]));

                    }
                }
                periodBalance = periodDebit - periodCredit;

                periodBalanceGold = periodDebitGold - periodCreditGold;

                periodBalanceDiamond = periodDebitDiamond - periodCreditDiamond;

                if (periodDebit >= periodCredit)
                {
                    periodBalanceType = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "رصيد الفترة المحددة مدين" : "Selected Period Balance Is Debit");
                }
                else
                {
                    periodBalanceType = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "رصيد الفترة المحددة دائن" : "Selected Period Balance Is Credit");
                }
                DataRow r2 = _sampleData.NewRow();
                r2["Balance"] = periodBalance;
                r2["Debit"] = periodDebit;
                r2["Credit"] = periodCredit;

                r2["BalanceGold"] = periodBalanceGold;
                r2["DebitGold"] = periodDebitGold;
                r2["CreditGold"] = periodCreditGold;


                r2["BalanceDiamond"] = periodBalanceDiamond;
                r2["DebitDiamond"] = periodDebitDiamond;
                r2["CreditDiamond"] = periodCreditDiamond;

                r2["Declaration"] = periodBalanceType;
                r2["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                _sampleData.Rows.Add(r2);
                _sampleData.Rows.Add(TotalsRow());

                for (int i = 0; i < _sampleData.Rows.Count; i++)
                {
                    _sampleData.Rows[i]["Balance"] = Math.Abs(Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]));
                }

                ProgressBar.Value = ProgressBar.Value + 10;
                ProgressBar.Visible = false;

                //_sampleData.Rows(_sampleData.Rows.Count - 1).Cells(dgvColBalance.Name).Style.BackColor = Color.Aquamarine;
                //_sampleData.Rows(_sampleData.Rows.Count - 1).Cells(dgvColBalance.Name).Style.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
                //_sampleData.Rows(_sampleData.Rows.Count - 1).Cells(dgvColDeclaration.Name).Style.BackColor = Color.Aquamarine;

            }
            catch(Exception ex){ Messages.MsgError(Messages.TitleError, ex.Message); }
        }

        private void Totals(bool p)
        {
            try
            {
                decimal total = 0;
                decimal credit = 0;
                decimal debit = 0;
                decimal sum = 0;

                decimal totalGold = 0;
                decimal creditGold = 0;
                decimal debitGold = 0;
                decimal sumGold = 0;
                DataRow row;

                for (int i = 1; i <= _sampleData.Rows.Count - 1; i++)
                {
                   
                    sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["Balance"]);

                    credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                    debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                   

                    sumGold = Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["BalanceGold"]);
                    creditGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));
                    debitGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                    _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                   


                }
                total = credit - debit;

                totalGold = creditGold - debitGold;


                //------------------
                if (total < 0)
                {
                    lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance Current term Debit" : "الرصيد الفترة المحدده مدين");
                }
                else
                {
                    lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance Current term Credit" : "الرصيد الفترة المحدده دائن");
                }


                if (totalGold < 0)
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance Current term Debit" : "الرصيد الفترة المحدده مدين");
                }
                else
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance Current term Credit" : "الرصيد الفترة المحدده دائن");
                }


                lblDebit.Text = debit.ToString();
                lblCredit.Text = credit.ToString();
                lblBalanceSum.Text = Math.Abs(total).ToString();


                lblDebitGold.Text = debitGold.ToString();
                lblCreditGold.Text = creditGold.ToString();
                lblBalanceSumGold.Text = Math.Abs(totalGold).ToString();


            }
            catch { }
        }
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
                    if (ToDate == 0)
                        ToDate = Comon.cLong((Lip.GetServerDateSerial()));
                    int index = -1;

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
                            row = TotalsRow();
                           
                            //row["TheDate"] = null;
                            //row["OppsiteAccountName"] = null;
                            //row["RecordType"] = null;
                            //row["ID"] = null;
                            //row["Declaration"] = (lang == "Eng" ? "Open Balance" : "الرصـيد حتى بـداية الـمـدة");
                            //_sampleData.Rows.Add(row);
                            //return;
                             total = 0;
                             credit = 0;
                             debit = 0;
                             sum = 0;
                            for (int i = 0; i <= _sampleData.Rows.Count - keys; i++)
                            {
                                credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                                debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                                _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                                sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                            }
                            total = credit - debit;
                            _sampleData.Rows.Clear();
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
                            return;



                        }
                        else if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[0]["TheDate"].ToString())) > FromDate)
                        {

                            if (Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[0]["TheDate"].ToString())) > ToDate)
                            {
                                _sampleData.Rows.Clear();
                                //   addEvenRow();
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

                         //   string SearchDate = Comon.cStr().ToString();
                            long SearchDate = Comon.cLong(Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()));
                            if (SearchDate > ToDate)
                            {
                                endDAte = i-1;
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
                    decimal total1 = 0;
                    decimal credit1 = 0;
                    decimal debit1 = 0;
                    decimal sum1= 0;
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
                        credit1 += (Comon.ConvertToDecimalPrice(dt.Rows[x]["Credit"]));
                        debit1 += (Comon.ConvertToDecimalPrice(dt.Rows[x]["Debit"]));

                        x += 1;
                    }
                  

                    for (int i = 0; i <= index - 1; i++)
                    {

                        credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                        debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                        sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                     

                    }
                    credit1 = credit1 + credit;
                    debit1 = debit1 + debit;
                    total1 = credit1 - debit1;

                    if (total1 < 0)
                    {
                        lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                    }
                    else
                    {
                        lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                    }
                    lblDebit.Text = debit1.ToString();
                    lblCredit.Text = credit1.ToString();
                    lblBalanceSum.Text = Math.Abs(total1).ToString();
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
                        _sampleData.Rows.Add(row);

                    } 
                    dt.Dispose();
                    row = null;
                }
                else {
                    if (FromDate == 0)
                    {
                        return;
                    }
                    addEvenRow();
                }

            }
            catch { }
        }
        
        #endregion
        #region Events
        private void VariousVoucherMachin(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                string strSQLID = "Select [ID] ,[ArbCaptionType],[EngCaptionType] from Manu_ManuFactoryOprationType ";
                dtFactoryOprationType = Lip.SelectRecord(strSQLID);
                DataTable dtCredit = new DataTable();
                string strSQL = null; DataRow row;
                //strSQL = "SELECT Acc_VariousVoucherMachinDetails.Declaration, Acc_VariousVoucherMachinMaster.VoucherDate AS TheDate, Acc_VariousVoucherMachinMaster.VoucherID" + " AS ID, 'VariousVoucher' AS RecordType, ' ' AS OppsiteAccountName, Acc_VariousVoucherMachinDetails.AccountID, Acc_VariousVoucherMachinDetails.Debit, Acc_VariousVoucherMachinMaster.RegTime, " + " Acc_VariousVoucherMachinDetails.Credit FROM Acc_VariousVoucherMachinMaster INNER JOIN Acc_VariousVoucherMachinDetails ON Acc_VariousVoucherMachinMaster.VoucherID" + " = Acc_VariousVoucherMachinDetails.VoucherID AND Acc_VariousVoucherMachinMaster.BranchID = Acc_VariousVoucherMachinDetails.BranchID " + " WHERE (Acc_VariousVoucherMachinMaster.Cancel = 0) AND (Acc_VariousVoucherMachinMaster.BranchID = " + WT.GlobalBranchID + ")" + " AND (Acc_VariousVoucherMachinDetails.AccountID = " + txtAccountID.TextWT + ") ";
                strSQL = " SELECT Acc_VariousVoucherMachinMaster.DocumentID,Acc_VariousVoucherMachinMaster.Posted ,Acc_VariousVoucherMachinDetails.DECLARATION,Acc_VariousVoucherMachinMaster.VOUCHERDATE AS TheDate,Acc_VariousVoucherMachinMaster.VoucherID AS ID,"
                + " Acc_VariousVoucherMachinMaster.DocumentType , ' ' AS OppsiteAccountName,Acc_VariousVoucherMachinDetails.ACCOUNTID,Acc_VariousVoucherMachinDetails.Debit,Acc_VariousVoucherMachinDetails.DebitGold,Acc_VariousVoucherMachinDetails.CreditDiamond,Acc_VariousVoucherMachinDetails.DebitDiamond, "
                + " Acc_VariousVoucherMachinMaster.RegTime,Acc_VariousVoucherMachinDetails.CreditGold,Acc_VariousVoucherMachinDetails.Credit FROM Acc_VariousVoucherMachinMaster INNER JOIN Acc_VariousVoucherMachinDetails"
                + " ON Acc_VariousVoucherMachinMaster.VoucherID= Acc_VariousVoucherMachinDetails.VoucherID AND Acc_VariousVoucherMachinMaster.BranchID= Acc_VariousVoucherMachinDetails.BranchID"
                + " AND Acc_VariousVoucherMachinMaster.FacilityID  = Acc_VariousVoucherMachinDetails.FacilityID WHERE Acc_VariousVoucherMachinDetails.AccountID = " + AccountID
                + " AND Acc_VariousVoucherMachinMaster.CANCEL = 0  AND Acc_VariousVoucherMachinMaster.BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + " AND Acc_VariousVoucherMachinMaster.FacilityID =" + UserInfo.FacilityID;
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {
                    strSQL = strSQL + " AND  Acc_VariousVoucherMachinDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                if (Comon.cInt(cmbCurency.EditValue) > 0)
                {
                    strSQL = strSQL + " and Acc_VariousVoucherMachinDetails.CurrencyID =" + Comon.cInt(cmbCurency.EditValue);
                }
               if(Comon.cInt(cmbStatus.EditValue)>0)
               {
                   strSQL = strSQL + " and  Acc_VariousVoucherMachinMaster.Posted=" + Comon.cInt(cmbStatus.EditValue);
               }
                strSQL = strSQL + " ORDER BY Acc_VariousVoucherMachinMaster.VoucherDate,Acc_VariousVoucherMachinMaster.RegTime";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtCredit = Lip.SelectRecord(strSQL);
                if (dtCredit.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = i + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مذكورين" : "Mentioned");
                        row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtCredit.Rows[i]["DocumentType"];
                       

                        if(Comon.cInt(dtCredit.Rows[i]["Posted"])==1)
                            row["Posted"] = UserInfo.Language==iLanguage.Arabic?"معلق ":"Pending";
                        else if(Comon.cInt( dtCredit.Rows[i]["Posted"])==2)
                            row["Posted"] = UserInfo.Language == iLanguage.Arabic ? "غير مرحل " : "Not deported";
                        else if(Comon.cInt( dtCredit.Rows[i]["Posted"])==3)
                            row["Posted"] = UserInfo.Language == iLanguage.Arabic ? " مرحل " : " Deported";


                        DataRow[] rowtyp = dtFactoryOprationType.Select("ID =" + Comon.cInt(dtCredit.Rows[i]["DocumentType"]));
                        string DocumentTypeName = "";
                        if (rowtyp.Length > 0)
                        {
                            DocumentTypeName = UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? rowtyp[0]["ArbCaptionType"].ToString() : rowtyp[0]["EngCaptionType"].ToString();
                            row["RecordType"] = DocumentTypeName;
                        }
                        else
                        {
                            if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) ==0)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "قيد افتتاحي" : "Opening Voucher");
                            if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 1)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "قيد يومي" : "Various Voucher");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 2)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند صرف " : "Spend Vochare");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 3)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند قبض " : "Recipt Vochare");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 4)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "  فاتورة مشتريات الماس " : "Purches invoice almas");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 5)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " فاتورة مردود مشتريات الماس " : "Return Purches invoice almas");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 6)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات سلعية " : "Sales Invoice");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 7)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مردود مبيعات  " : "Sales Invoice");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 9)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات ذهب " : "sales Gold");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 10)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات ذهب " : "Purches invoice Gold");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 11)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " فاتورة مردود مشتريات ذهب " : "Return Purches invoice Gold");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 13)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " فاتورة سند العرض " : "  Purches save invoice  ");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 14)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " توريد مخزني- ذهب " : "InOn Store- Gold");

                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 15)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "بضاعة أول المدة" : "Goods Opening");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 16)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " صرف مخزني- ذهب " : "OutOn Store- Gold");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 17)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " توريد مخزني- مواد خام " : "InOn Store- Matirial");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 18)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " صرف مخزني- مواد خام " : "OutOn Store- Matirial");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 19)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "تحويل  مخزني متعدد - ذهب" : "Transefer Stoer Multi Gold");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 20)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "تحويل  مخزني متعدد - مواد خام" : "Transefer Stoer Multi Matirial");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 23)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات " : "Purchase Invoice ");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 24)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مردود مشتريات " : "Purchase Return Invoice ");
                            else if (Comon.cInt(dtCredit.Rows[i]["DocumentType"]) == 39)
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات خدمية  " : "Sales Invoice Service");
                        }

                        // else row["RecordType"] = dtCredit.Rows[i]["Declaration"].ToString(); 
                        row["ID"] = dtCredit.Rows[i]["DocumentID"];
                        row["Declaration"] = dtCredit.Rows[i]["Declaration"].ToString() != "" ? dtCredit.Rows[i]["Declaration"].ToString() : row["RecordType"].ToString();
                        row["Credit"] = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["Credit"]);
                        row["Debit"] = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["Debit"]);
                        row["CreditGold"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["CreditGold"]);
                        row["DebitGold"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["DebitGold"]);

                        row["CreditDiamond"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["CreditDiamond"]);
                        row["DebitDiamond"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["DebitDiamond"]);


                        _sampleData.Rows.Add(row);

                    }
                }
                dtCredit.Dispose();
                row = null;
            }
            catch (Exception ex) { Messages.MsgError(Messages.TitleError, ex.Message); }
        }

        protected void txtAccountID_TextChanged(object sender, EventArgs e)
        {
            Acc_Accounts Accounts = new Acc_Accounts();
            Accounts =  Acc_AccountsDAL.GetDataByID(Comon.cLong(txtAccountID.Text), 1, 1);
            if (Accounts != null)
            {
                txtAccountID.Text = Accounts.AccountID.ToString();
                lblAccountName.Text = Accounts.ArbName;
            }
            else
            {
                txtAccountID.Text = "";
                lblAccountName.Text = "";

            }
        }

        public void btnShow_Click(object sender, EventArgs e)
        {
            try{
            long AccountID = Comon.cLong(txtAccountID.Text.Trim());
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            if (txtAccountID.Text == string.Empty)
            {
                MessageBox.Show("يجب ادخال رقم الحساب");
                return;

            }
            if (FromDate != 0 && ToDate != 0)
            {
                FromDate = Comon.cLong(FromDate);
                ToDate = Comon.cLong(ToDate);
                ProcessWithOutDate(txtAccountID.Text, FromDate, ToDate);
            }
            else
            {
                ProcessWithOutDate(txtAccountID.Text, FromDate, ToDate);
            }

            makeGridBind(_sampleData);

            ProgressBar.Value = 0;
            ProgressBar.Visible = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtAccountID.Enabled = false;

            txtCostCenterID.Enabled = false;
            txtFromDate.Enabled = false;
            btnCostCenterSearch.Enabled = false;
            btnDebitSearch.Enabled = false;
            btnCostCenterSearch.Enabled = false;
            }
            catch { }
        }

        private void btnDebitSearch_Click(object sender, EventArgs e)
        {
            try
            {
                
                PrepareSearchQuery.SearchForAccounts(txtAccountID, lblAccountName,Comon.cInt(cmbBranchesID.EditValue));
                txtAccountID_Validating(null, null);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try{
            GridView view = sender as GridView;
              string Type=view.GetFocusedRowCellValue("TempRecordType").ToString();
           if(Type=="30"||Type=="31")
           {
               
                    frmCasting frmCasting = new frmCasting();
                    if (Permissions.UserPermissionsFrom(frmCasting, frmCasting.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmCasting);
                        frmCasting.Show();
                        frmCasting.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmCasting.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmCasting.Dispose();
                  
            }
           if (Type == "32" || Type == "33")
           {

               frmManufacturingCommand frmCommand = new frmManufacturingCommand();
               if (Permissions.UserPermissionsFrom(frmCommand, frmCommand.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
               {
                   if (UserInfo.Language == iLanguage.English)
                       ChangeLanguage.EnglishLanguage(frmCommand);
                   frmCommand.Show();
                   frmCommand.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                   frmCommand.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
               }
               else
                   frmCommand.Dispose();

           }
        
           if (Type == "34" || Type == "35"||Type == "36" || Type == "37")
           {

               frmManufacturingPrentag frmPrntage = new frmManufacturingPrentag();
               if (Permissions.UserPermissionsFrom(frmPrntage, frmPrntage.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
               {
                   if (UserInfo.Language == iLanguage.English)
                       ChangeLanguage.EnglishLanguage(frmPrntage);
                   if (Type == "34" || Type=="35")
                       frmPrntage.cmbPrntageTypeID.EditValue = 1;
                   else
                       frmPrntage.cmbPrntageTypeID.EditValue = 2;
                   frmPrntage.Show();
                   frmPrntage.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                   frmPrntage.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
               }
               else
                   frmPrntage.Dispose();

           }
           if (Type == "38" || Type== "39")
           {

               frmManufacturingCompond frmCompound = new frmManufacturingCompond();
               if (Permissions.UserPermissionsFrom(frmCompound, frmCompound.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
               {
                   if (UserInfo.Language == iLanguage.English)
                       ChangeLanguage.EnglishLanguage(frmCompound);
                   frmCompound.Show();
                   frmCompound.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                   frmCompound.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
               }
               else
                   frmCompound.Dispose();

           }
           if (Type == "44" || Type == "45")
           {

               frmManufactoryAdditional frmAddtional = new frmManufactoryAdditional();
               if (Permissions.UserPermissionsFrom(frmAddtional, frmAddtional.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
               {
                   if (UserInfo.Language == iLanguage.English)
                       ChangeLanguage.EnglishLanguage(frmAddtional);
                   frmAddtional.Show();
                   frmAddtional.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                   frmAddtional.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
               }
               else
                   frmAddtional.Dispose();

           }
           if (Type == "46" || Type == "47")
           {

               frmManufacturingDismantOrders frmDismant = new frmManufacturingDismantOrders();
               if (Permissions.UserPermissionsFrom(frmDismant, frmDismant.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
               {
                   if (UserInfo.Language == iLanguage.English)
                       ChangeLanguage.EnglishLanguage(frmDismant);
                   frmDismant.Show();
                   frmDismant.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                   frmDismant.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
               }
               else
                   frmDismant.Dispose();

           }
           if (Type == "40" ||Type == "41" || Type == "42" || Type == "43")
           {

               frmManufacturingTalmee frmTalmee = new frmManufacturingTalmee();
               if (Permissions.UserPermissionsFrom(frmTalmee, frmTalmee.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
               {
                   if (UserInfo.Language == iLanguage.English)
                       ChangeLanguage.EnglishLanguage(frmTalmee);
                 
                   if (Type == "34" || Type == "35")
                       frmTalmee.cmbPollutionTypeID.EditValue = 1;
                   else
                       frmTalmee.cmbPollutionTypeID.EditValue = 2;
                   frmTalmee.Show();
                   frmTalmee.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                   frmTalmee.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
               }
               else
                   frmTalmee.Dispose();

           }
            switch (view.GetFocusedRowCellValue("TempRecordType").ToString())
            {
                case "48":
                    frmManuExpencessOrder frmCost = new frmManuExpencessOrder();
                    if (Permissions.UserPermissionsFrom(frmCost, frmCost.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmCost);
                        frmCost.Show();
                        frmCost.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmCost.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmCost.Dispose();
                    break;
                case "25":
                    frmCadFactory frmCad = new frmCadFactory();
                    if (Permissions.UserPermissionsFrom(frmCad, frmCad.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmCad);
                        frmCad.Show();
                        frmCad.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmCad.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmCad.Dispose();
                    break;
                case "26":
                    frmWaxFactory frmWax = new frmWaxFactory();
                    if (Permissions.UserPermissionsFrom(frmWax, frmWax.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmWax);
                        frmWax.Show();
                        frmWax.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmWax.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmWax.Dispose();
                    break;
                case "27":
                    frmZirconeFactory frmZircon = new frmZirconeFactory();
                    if (Permissions.UserPermissionsFrom(frmZircon, frmZircon.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmZircon);
                        frmZircon.Show();
                        frmZircon.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmZircon.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmZircon.Dispose();
                    break;
                case "28":
                    frmDiamondFactory frmDiamond = new frmDiamondFactory();
                    if (Permissions.UserPermissionsFrom(frmDiamond, frmDiamond.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmDiamond);
                        frmDiamond.Show();
                        frmDiamond.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmDiamond.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmDiamond.Dispose();
                    break;

                case "29":
                    frmAfforestationFactory frmAfforest = new frmAfforestationFactory();
                    if (Permissions.UserPermissionsFrom(frmAfforest, frmAfforest.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmAfforest);
                        frmAfforest.Show();
                        frmAfforest.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmAfforest.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmAfforest.Dispose();
                    break;

                
                
                case "10":
                    frmCashierPurchaseGold frm = new frmCashierPurchaseGold();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                        frm.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm.Dispose();
                    break;

                case "4":
                    frmCashierPurchaseDaimond frm1 = new frmCashierPurchaseDaimond();
                    if (Permissions.UserPermissionsFrom(frm1, frm1.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm1);
                        frm1.Show();
                        frm1.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm1.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm1.Dispose();
                    break;
                


                         
                case "ItemsOutOnBail":
                    frmItemsOutOnBail frm11 = new frmItemsOutOnBail();
                    if (Permissions.UserPermissionsFrom(frm11, frm11.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm11);
                        frm11.Show();
                        frm11.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm11.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm11.Dispose();
                    break;


                case "ItemsInOnBail":
                    frmItemsInonBail frm12 = new frmItemsInonBail();
                    if (Permissions.UserPermissionsFrom(frm12, frm12.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm12);
                        frm12.Show();
                        frm12.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm12.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm12.Dispose();
                 break;


                case "GoodsOpening":
                    frmGoodsOpeningOld frm112 = new frmGoodsOpeningOld();
                    if (Permissions.UserPermissionsFrom(frm112, frm112.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm112);
                        frm112.Show();
                        frm112.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm112.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm112.Dispose();
                    break;
                //case "ItemsTransfer":
                //   frmItemsTransfer   frm =new frmItemsTransfer();
                //   //  Lip.Ch(frm, Language)
                //     frm.Show();
                //     frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString())+1,8);
                //    break;
                case "ItemsDismantling":
                    frmItemsDismantling frm10 = new frmItemsDismantling();
                    if (Permissions.UserPermissionsFrom(frm10, frm10.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm10);
                        frm10.Show();
                        frm10.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm10.Dispose();
                    break;
                     
                case "7":
                    frmSalesInvoiceReturn frm2 = new frmSalesInvoiceReturn();
                    if (Permissions.UserPermissionsFrom(frm2, frm2.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm2);
                        frm2.Show();
                        frm2.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm2.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm2.Dispose();
                    break;
           
                case "9":
                    frmCashierSalesGold frm3 = new frmCashierSalesGold();
                    if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm3);
                        frm3.Show();
                        frm3.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm3.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm3.Dispose();
                    break;


                case "6":
                    frmCashierSalesAlmas frm30 = new frmCashierSalesAlmas();
                    if (Permissions.UserPermissionsFrom(frm30, frm30.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm30);
                        frm30.Show();
                        frm30.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm30.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm30.Dispose();
                    break;

                  
                 
                case "5":
                    frmCashierPurchaseReturnDaimond frm4 = new frmCashierPurchaseReturnDaimond();
                    if (Permissions.UserPermissionsFrom(frm4, frm4.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm4);
                        frm4.Show();
                        frm4.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm4.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm4.Dispose();
                    break;
                case "11":
                    frmCashierPurchaseReturnGold frm15= new frmCashierPurchaseReturnGold();
                    if (Permissions.UserPermissionsFrom(frm15, frm15.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm15);
                        frm15.Show();
                        frm15.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm15.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm15.Dispose();
                    break;
               
                   

                case "3":
                    frmReceiptVoucher frm20 = new frmReceiptVoucher();
                    if (Permissions.UserPermissionsFrom(frm20, frm20.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm20);
                        frm20.Show();
                        frm20.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm20.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm20.Dispose();
                    break;

                case "2":
                    frmSpendVoucher frm24 = new frmSpendVoucher();
                    if (Permissions.UserPermissionsFrom(frm24, frm24.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm24);
                        frm24.Show();
                        frm24.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm24.Dispose();
                    break;


                case "CheckReceiptVoucher":
                    frmCheckReceiptVoucher frm23 = new frmCheckReceiptVoucher();
                    if (Permissions.UserPermissionsFrom(frm23, frm23.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm23);
                        frm23.Show();
                        frm23.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm23.Dispose();
                    break;

                case "1":
                    frmVariousVoucher frm22 = new frmVariousVoucher();
                    if (Permissions.UserPermissionsFrom(frm22, frm22.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm22);
                        frm22.Show();
                        frm22.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm22.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm22.Dispose();
                    break;

                case "0":
                    frmOpeningVoucher frm211 = new frmOpeningVoucher();
                    if (Permissions.UserPermissionsFrom(frm211, frm211.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm211);
                        frm211.Show();
                        frm211.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm211.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm211.Dispose();
                    break;


                case "13":
                    frmCashierPurchaseSaveDaimond frm201 = new frmCashierPurchaseSaveDaimond();
                    if (Permissions.UserPermissionsFrom(frm201, frm201.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm201);
                        frm201.Show();
                        frm201.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm201.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm201.Dispose();
                    break;

                case "14":
                    frmGoldInOnBail frm2101 = new frmGoldInOnBail();
                    if (Permissions.UserPermissionsFrom(frm2101, frm2101.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm2101);
                        frm2101.Show();
                        frm2101.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm2101.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm2101.Dispose();
                    break;
                case "15":
                    frmGoodsOpening frmGoodsOp = new frmGoodsOpening();
                    if (Permissions.UserPermissionsFrom(frmGoodsOp, frmGoodsOp.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmGoodsOp);

                        frmGoodsOp.Show();
                        frmGoodsOp.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmGoodsOp.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmGoodsOp.Dispose();
                   
                    break;
                case "16":
                    frmGoldOutOnBail frmGolOut= new frmGoldOutOnBail();
                    if (Permissions.UserPermissionsFrom(frmGolOut, frmGolOut.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmGolOut);

                        frmGolOut.Show();
                        frmGolOut.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmGolOut.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmGolOut.Dispose();

                    break;
                case "17":
                    frmMatirialInOnBail frmMatiralIn = new frmMatirialInOnBail();
                    if (Permissions.UserPermissionsFrom(frmMatiralIn, frmMatiralIn.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmMatiralIn);

                        frmMatiralIn.Show();
                        frmMatiralIn.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmMatiralIn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmMatiralIn.Dispose();

                    break;
                case "18":
                    frmMatirialOutOnBail frmMatiralOut = new frmMatirialOutOnBail();
                    if (Permissions.UserPermissionsFrom(frmMatiralOut, frmMatiralOut.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmMatiralOut);

                        frmMatiralOut.Show();
                        frmMatiralOut.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmMatiralOut.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmMatiralOut.Dispose();

                    break;
                case "19":
                    frmTransferMultipleStoresGold frmGoldMulti = new frmTransferMultipleStoresGold();
                    if (Permissions.UserPermissionsFrom(frmGoldMulti, frmGoldMulti.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmGoldMulti);

                        frmGoldMulti.Show();
                        frmGoldMulti.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmGoldMulti.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmGoldMulti.Dispose();

                    break;
                case "20":
                    frmTransferMultipleStoreMatirial frmMatiralMulti = new frmTransferMultipleStoreMatirial();
                    if (Permissions.UserPermissionsFrom(frmMatiralMulti, frmMatiralMulti.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmMatiralMulti);

                        frmMatiralMulti.Show();
                        frmMatiralMulti.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmMatiralMulti.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmMatiralMulti.Dispose();

                    break;
                case "23":

                    frmCashierPurchaseMatirial frmGoldMatirialInvoice = new frmCashierPurchaseMatirial();
                    if (Permissions.UserPermissionsFrom(frmGoldMatirialInvoice, frmGoldMatirialInvoice.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmGoldMatirialInvoice);

                        frmGoldMatirialInvoice.Show();
                        frmGoldMatirialInvoice.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmGoldMatirialInvoice.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmGoldMatirialInvoice.Dispose();
                    break;
                case "24":
                    frmCashierPurchaseReturnMatirial frmMatiralInvReturn = new frmCashierPurchaseReturnMatirial();
                    if (Permissions.UserPermissionsFrom(frmMatiralInvReturn, frmMatiralInvReturn.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmMatiralInvReturn);

                        frmMatiralInvReturn.Show();
                        frmMatiralInvReturn.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmMatiralInvReturn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmMatiralInvReturn.Dispose();

                    break;
                case "39":

                    frmCashierSales frmSales = new frmCashierSales();
                    if (Permissions.UserPermissionsFrom(frmSales, frmSales.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frmSales);

                        frmSales.Show();
                        //frmSales.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frmSales.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frmSales.Dispose();

                    break;
               
            }
            }
            catch { }
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

        public void txtAccountID_Validating(object sender, CancelEventArgs e)
        {
          try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + Comon.cDbl( txtAccountID.Text) + ") ";
                CSearch.ControlValidating(txtAccountID , lblAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
              try
            {
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmAccountStatement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
        }

        private void ProgressBar_Click(object sender, EventArgs e)
        {

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (txtAccountID.Text != "")
                DoPrint();
        }

        private void GridView1_DataSourceChanged(object sender, EventArgs e)
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

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        { }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            
            dgvColDebitGold.Visible = chkGold.Checked;
            dgvColCreditGold.Visible = chkGold.Checked;
            dgvColBalanceGold.Visible = chkGold.Checked;
           
            labelControl6.Visible = chkGold.Checked;
            labelControl1.Visible = chkGold.Checked;
            lblDebitGold.Visible= chkGold.Checked;
            labelControl7.Visible = chkGold.Checked;
            lblCreditGold.Visible = chkGold.Checked;
            labelControl5.Visible = chkGold.Checked;
            lblBalanceSumGold.Visible = chkGold.Checked;
            lblBalanceTypeGold.Visible = chkGold.Checked;


            dvgColDiamond.Visible = chkDiamond.Checked;
            dvgColCreditDiamond.Visible = chkDiamond.Checked;
            dvgColBalanceDiamond.Visible = chkDiamond.Checked;

            labelControl13.Visible = chkDiamond.Checked;
            lblDebitDiamond.Visible = chkDiamond.Checked;
            labelControl12.Visible = chkDiamond.Checked;
            lblCreditDiamond.Visible = chkDiamond.Checked;
            labelControl11.Visible = chkDiamond.Checked;
            lblBalanceSumDiamond.Visible = chkDiamond.Checked;
            labelControl10.Visible = chkDiamond.Checked;
            lblBalanceTypeDiamond.Visible = chkDiamond.Checked;



            dgvColDebit.Visible = chkChash.Checked;
            dgvColCredit.Visible = chkChash.Checked;
            dgvColBalance.Visible = chkChash.Checked;

            lblBalanceType.Visible = chkChash.Checked;
            labelControl4.Visible = chkChash.Checked;
            lblBalanceSum.Visible = chkChash.Checked;
            labelControl3.Visible = chkChash.Checked;
            lblCredit.Visible = chkChash.Checked;
            labelControl2.Visible = chkChash.Checked;
            lblDebit.Visible = chkChash.Checked;
            labelControl8.Visible = chkChash.Checked;
           
      

            //if (chkChash.Checked && chkDiamond.Checked && chkGold.Checked)
            //    chkAll.Checked = true;
            //else if (!chkChash.Checked && !chkDiamond.Checked && !chkGold.Checked)
            //    chkAll.Checked = true;
            //if (chkAll.Checked == false && chkChash.Checked == false && chkDiamond.Checked == false && chkGold.Checked == false)
            //    chkAll.Checked = true;
            if(chkAll.Checked)
            {
                chkChash.Checked = false; chkDiamond.Checked = false; chkGold.Checked = false;
                dgvColDebitGold.Visible = chkAll.Checked;
                dgvColCreditGold.Visible = chkAll.Checked;
                dgvColBalanceGold.Visible = chkAll.Checked;

                labelControl6.Visible = chkAll.Checked;
                labelControl1.Visible = chkAll.Checked;
                lblDebitGold.Visible = chkAll.Checked;
                labelControl7.Visible = chkAll.Checked;
                lblCreditGold.Visible = chkAll.Checked;
                labelControl5.Visible = chkAll.Checked;
                lblBalanceSumGold.Visible = chkAll.Checked;
                lblBalanceTypeGold.Visible = chkAll.Checked;


                dvgColDiamond.Visible = chkAll.Checked;
                dvgColCreditDiamond.Visible = chkAll.Checked;
                dvgColBalanceDiamond.Visible = chkAll.Checked;

                labelControl13.Visible = chkAll.Checked;
                lblDebitDiamond.Visible = chkAll.Checked;
                labelControl12.Visible = chkAll.Checked;
                lblCreditDiamond.Visible = chkAll.Checked;
                labelControl11.Visible = chkAll.Checked;
                lblBalanceSumDiamond.Visible = chkAll.Checked;
                labelControl10.Visible = chkAll.Checked;
                lblBalanceTypeDiamond.Visible = chkAll.Checked;



                dgvColDebit.Visible = chkAll.Checked;
                dgvColCredit.Visible = chkAll.Checked;
                dgvColBalance.Visible = chkAll.Checked;

                lblBalanceType.Visible = chkAll.Checked;
                labelControl4.Visible = chkAll.Checked;
                lblBalanceSum.Visible = chkAll.Checked;
                labelControl3.Visible = chkAll.Checked;
                lblCredit.Visible = chkAll.Checked;
                labelControl2.Visible = chkAll.Checked;
                lblDebit.Visible = chkAll.Checked;
                labelControl8.Visible = chkAll.Checked;
            }
        }
    }

}



 
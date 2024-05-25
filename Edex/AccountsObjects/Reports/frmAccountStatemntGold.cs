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

namespace Edex.AccountsObjects.Reports
{
   
     
    public partial class frmAccountStatemntGold : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string FocusedControl = "";
        private string PrimaryName;

        public DataTable _sampleData = new DataTable();
        public frmAccountStatemntGold(long AccountNO)
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
                 
                    dgvColCreditGold.Caption = "Credit Gold";
                    dgvColDebitGold.Caption = "Debit  Gold";

                 
                    dgvColn_invoice_serial.Caption = "# ";
                    
                    dgvColRecordType.Caption = "Record Type ";
                    dgvColID.Caption = "ID";
                    dgvColTempRecordType.Caption = "Total  Quntity ";
                    dgvColRegTime.Caption = "RegTime";
                    btnShow.Text = "show";
                    //  Label8.Text = btnShow.Tag.ToString();
                }
                where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + Comon.cInt(cmbBranchesID.EditValue);
                _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
           
                _sampleData.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));

            

                _sampleData.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));

             

                _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));


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
        public frmAccountStatemntGold(long AccountNO, bool yes)
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
                     
                    dgvColn_invoice_serial.Caption = "# ";
           
                    dgvColRecordType.Caption = "Record Type ";
                    dgvColID.Caption = "ID";
                    dgvColTempRecordType.Caption = "Total  Quntity ";
                    dgvColRegTime.Caption = "RegTime";
                    btnShow.Text = "show";
                    //  Label8.Text = btnShow.Tag.ToString();

                }
                where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + Comon.cInt(cmbBranchesID.EditValue);
                _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
             
                _sampleData.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));

         
                _sampleData.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));
            
            
                _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));
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
        public frmAccountStatemntGold()
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
                    dgvColOppsiteAccountName.Caption = "Oppsite Account Name ";
                    dgvColTheDate.Caption = "The Date";
                    dgvColDeclaration.Caption = "Declaration ";
                  
                    dgvColn_invoice_serial.Caption = "# ";
               
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
            try
            {

                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"), NameCol: UserInfo.Language == iLanguage.Arabic ? "الكل " : "All");
                cmbBranchesID.EditValue = UserInfo.BRANCHID;
                if (UserInfo.BRANCHID == 1)
                {
                    cmbBranchesID.Visible = true;
                    labelControl9.Visible = true;
                }

                else
                {
                    labelControl9.Visible = false;
                    cmbBranchesID.Visible = false;
                }

                where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + UserInfo.BRANCHID;
                _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
           
                _sampleData.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));
                
                _sampleData.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));

            
               
                _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));
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
        }

        void makeGridBind(DataTable dt)
        {
            DataView dv = dt.DefaultView;
            _sampleData = dt;
            gridControl1.DataSource = dt;
        }
        private DataTable GetEmptyDataTable()
        {
            strSQL = "SELECT 0 AS n_invoice_serial,'' AS Balance,'' AS DebitGold,'' AS CreditGold,'' ,'' AS Declaration,'' AS TheDate,'' AS OppsiteAccountName,"
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
                if (GridView1.GetRowCellValue(0, "ID").ToString() == "" && (GridView1.GetRowCellValue(0, "CreditGold").ToString() == "" || GridView1.GetRowCellValue(0, "DebitGold").ToString() == ""))
                {
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                ReportName = "‏‏rptAccountStatementGold";
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
                rptForm.Parameters["TotalDebit1"].Value = lblDebitGold.Text.Trim().ToString();
                rptForm.Parameters["TotalCredit1"].Value = lblCreditGold.Text.Trim().ToString();
                rptForm.Parameters["TotalBalance1"].Value = lblBalanceSumGold.Text.Trim().ToString();
                rptForm.Parameters["CurrentBalance"].Value = lblBalanceTypeGold.Text.Trim().ToString();
              
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
 
                        row["DebitGold"] = GridView1.GetRowCellValue(i, "DebitGold").ToString();
                        row["CreditGold"] = GridView1.GetRowCellValue(i, "CreditGold").ToString();
                       
                        row["BalanceGold"] = GridView1.GetRowCellValue(i, "BalanceGold").ToString(); 
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
                   
                  if (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]) == 0)
                            {
                    if (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]) == 0)
                                {                                     
                                            _sampleData.Rows.RemoveAt(i);
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
                    
                    row["DebitGold"] = view[i]["DebitGold"];
                    row["CreditGold"] = view[i]["CreditGold"];

                  

                    row["Declaration"] = view[i]["Declaration"];
                    row["TempRecordType"] = view[i]["TempRecordType"];
                    row["TheDate"] = Comon.ConvertSerialDateTo(view[i]["TheDate"].ToString());
                    row["OppsiteAccountName"] = view[i]["OppsiteAccountName"];
                    row["ID"] = view[i]["ID"];
                    if (row["TempRecordType"].ToString() == "VariousVoucher")
                    {
                        if (Comon.cInt(row["ID"].ToString()) == 0)
                            row["TempRecordType"] = "OpeningVoucher";
                    }
                    row["RecordType"] = view[i]["RecordType"];
                    row["RegTime"] = view[i]["RegTime"];
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
                
              

                decimal totalGold = 0;
                decimal creditGold = 0;
                decimal debitGold = 0;
                decimal sumGold = 0;

                DataRow row;

                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                {
                    
                    creditGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));
                    debitGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));

                    _sampleData.Rows[i]["BalanceGold"] = sumGold + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));

                      sumGold = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["BalanceGold"]);


                }
              
                totalGold = creditGold - debitGold;

                row = _sampleData.NewRow();
               

                row["DebitGold"] = debitGold;
                row["CreditGold"] = creditGold;
                row["BalanceGold"] = Math.Abs(totalGold).ToString();
                row["n_invoice_serial"] = 0;
                _sampleData.Rows.Add(row);


                //------------------
                if (totalGold < 0)
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

                
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
           
            decimal totalGold = 0; decimal creditGold = 0; decimal debitGold = 0; decimal rowcreditGold = 0; decimal rowdebitGold = 0; decimal sumGold = 0;
              try
            {

                for (int i = 0; i < _sampleData.Rows.Count - 1; i++)
                {
                  

                    rowcreditGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));
                    rowdebitGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));

                    
                    _sampleData.Rows[i]["BalanceGold"] = sumGold + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                    
                      sumGold = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["BalanceGold"]);                  
                } 
                creditGold = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["CreditGold"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["CreditGold"]));
                debitGold = (Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["DebitGold"]) + Comon.ConvertToDecimalPrice(_sampleData.Rows[_sampleData.Rows.Count - 1]["DebitGold"]));


                
              
                totalGold = creditGold - debitGold;
                 
                 
                row["DebitGold"] = debitGold;
                row["CreditGold"] = creditGold;
                row["BalanceGold"] = Math.Abs(totalGold).ToString();

                 

                 

                if (totalGold < 0)
                {
                    row["Declaration"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    row["Declaration"] = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

                

                row["n_invoice_serial"] = _sampleData.Rows.Count + 1;

               
                lblDebitGold.Text = debitGold.ToString();
                lblCreditGold.Text = creditGold.ToString();
                lblBalanceSumGold.Text = Math.Abs(totalGold).ToString();

             
              
                if (totalGold < 0)
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
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
               
                lblCreditGold.Text = "";
             
                lblDebitGold.Text = "";
             
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
                
               

                double BeforeBalanceGold = 0;
                double BeforeDebitGold = 0;
                double BeforeCreditGold = 0;

                string BeforeBalanceType = "";

                string BeforeBalanceTypeGold = "";

              
               
                long tempFromDate = FromDate;

                double periodBalanceGold = 0;
                double periodDebitGold = 0;
                double periodCreditGold = 0;

              


                string periodBalanceTypeGold = "";


                ProgressBar.Visible = true;
                ProgressBar.Maximum = 170;
                ProgressBar.Minimum = 0;
                _sampleData.Rows.Clear();


                //=
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
                 _sampleData.Rows[i]["BalanceGold"] = Comon.ConvertToDecimalPrice(Math.Abs(Comon.cDbl(_sampleData.Rows[i]["BalanceGold"])));
                     
                }

                int inc = 0;
                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                {
                    if (Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()) < tempFromDate && Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()) != 0)
                    {
                        inc = inc + 1;
                        
                        BeforeDebitGold = Comon.cDbl(Comon.ConvertToDecimalPrice(BeforeDebitGold) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                        BeforeCreditGold = Comon.cDbl(Comon.ConvertToDecimalPrice(BeforeCreditGold) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));
                        BeforeBalanceGold = BeforeDebitGold - BeforeCreditGold;

                        //if (txtAccountID.Text == "12010000002")

                        if (BeforeDebitGold >= BeforeCreditGold)
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
                    if (ToDate > 0 && Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()) > ToDate && Comon.ConvertDateToSerial(_sampleData.Rows[i]["TheDate"].ToString()) != 0)
                    {
                        inc = inc + 1;
                        _sampleData.Rows.RemoveAt(i);
                    }
                }
              

                dr["DebitGold"] = BeforeDebitGold;
                dr["CreditGold"] = BeforeCreditGold;

                dr["Declaration"] = BeforeBalanceType;
                dr["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                _sampleData.Rows.InsertAt(dr, 0);
                //رصيد الفترة من دون اول المدة
                for (int i = 1; i < _sampleData.Rows.Count; i++)
                {
                    if (Comon.cDbl(_sampleData.Rows[i]["ID"].ToString()) != 0)
                    {
                        
                        periodDebitGold = Comon.cDbl(Comon.ConvertToDecimalPrice(periodDebitGold) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                        periodCreditGold = Comon.cDbl(Comon.ConvertToDecimalPrice(periodCreditGold) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));


                       
                    }
                }
                
                periodBalanceGold = periodDebitGold - periodCreditGold;
 
               
                DataRow r2 = _sampleData.NewRow();
                r2["BalanceGold"] = periodBalanceGold;
                r2["DebitGold"] = periodDebitGold;
                r2["CreditGold"] = periodCreditGold;
                if (periodDebitGold >= periodCreditGold)
                {
                    periodBalanceTypeGold = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "رصيد الفترة المحددة مدين" : "Selected Period Balance Is Debit");
                }
                else
                {
                    periodBalanceTypeGold = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "رصيد الفترة المحددة دائن" : "Selected Period Balance Is Credit");
                }
              
                r2["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                r2["Declaration"] = periodBalanceTypeGold;
                _sampleData.Rows.Add(r2);
                _sampleData.Rows.Add(TotalsRow());

                for (int i = 0; i < _sampleData.Rows.Count; i++)
                {
                    _sampleData.Rows[i]["Balance"] = Math.Abs(Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]));
                }

                ProgressBar.Value = ProgressBar.Value + 10;
                ProgressBar.Visible = false;

                 
            }
            catch { }
        }

        private void Totals(bool p)
        {
            try
            {
               

                decimal totalGold = 0;
                decimal creditGold = 0;
                decimal debitGold = 0;
                decimal sumGold = 0;
                DataRow row;

                for (int i = 1; i <= _sampleData.Rows.Count - 1; i++)
                {
 
                   

                    sumGold = Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["BalanceGold"]);
                    creditGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["CreditGold"]));
                    debitGold += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["DebitGold"]));
                    


                }
                 

                totalGold = creditGold - debitGold;


                


                if (totalGold < 0)
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance Current term Debit" : "الرصيد الفترة المحدده مدين");
                }
                else
                {
                    lblBalanceTypeGold.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance Current term Credit" : "الرصيد الفترة المحدده دائن");
                }

 

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
      
            row["Balance"] = 0; 
            row["Declaration"] = (lang == "Eng" ? "Open Balance" : "الرصـيد حتى بـداية الـمـدة");
            _sampleData.Rows.Add(row);


        }
  
        #region processData

        private void VariousVoucherMachin(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                string strSQL = null; DataRow row;
                //strSQL = "SELECT Acc_VariousVoucherMachinDetails.Declaration, Acc_VariousVoucherMachinMaster.VoucherDate AS TheDate, Acc_VariousVoucherMachinMaster.VoucherID" + " AS ID, 'VariousVoucher' AS RecordType, ' ' AS OppsiteAccountName, Acc_VariousVoucherMachinDetails.AccountID, Acc_VariousVoucherMachinDetails.Debit, Acc_VariousVoucherMachinMaster.RegTime, " + " Acc_VariousVoucherMachinDetails.Credit FROM Acc_VariousVoucherMachinMaster INNER JOIN Acc_VariousVoucherMachinDetails ON Acc_VariousVoucherMachinMaster.VoucherID" + " = Acc_VariousVoucherMachinDetails.VoucherID AND Acc_VariousVoucherMachinMaster.BranchID = Acc_VariousVoucherMachinDetails.BranchID " + " WHERE (Acc_VariousVoucherMachinMaster.Cancel = 0) AND (Acc_VariousVoucherMachinMaster.BranchID = " + WT.GlobalBranchID + ")" + " AND (Acc_VariousVoucherMachinDetails.AccountID = " + txtAccountID.TextWT + ") ";
                strSQL = " SELECT Acc_VariousVoucherMachinMaster.DocumentID,Acc_VariousVoucherMachinDetails.DECLARATION,Acc_VariousVoucherMachinMaster.VOUCHERDATE AS TheDate,Acc_VariousVoucherMachinMaster.VoucherID AS ID,"
                + " Acc_VariousVoucherMachinMaster.DocumentType , ' ' AS OppsiteAccountName,Acc_VariousVoucherMachinDetails.ACCOUNTID,Acc_VariousVoucherMachinDetails.DebitGold, "
                + " Acc_VariousVoucherMachinMaster.RegTime,Acc_VariousVoucherMachinDetails.CreditGold FROM Acc_VariousVoucherMachinMaster INNER JOIN Acc_VariousVoucherMachinDetails"
                + " ON Acc_VariousVoucherMachinMaster.VoucherID= Acc_VariousVoucherMachinDetails.VoucherID AND Acc_VariousVoucherMachinMaster.BranchID= Acc_VariousVoucherMachinDetails.BranchID"
                + " AND Acc_VariousVoucherMachinMaster.FacilityID  = Acc_VariousVoucherMachinDetails.FacilityID WHERE Acc_VariousVoucherMachinDetails.AccountID = " + AccountID
                + " AND Acc_VariousVoucherMachinMaster.CANCEL = 0 AND Acc_VariousVoucherMachinMaster.BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + " AND Acc_VariousVoucherMachinMaster.FacilityID =" + UserInfo.FacilityID;
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
                        row["n_invoice_serial"] = i + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مذكورين" : "Mentioned");
                        row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtCredit.Rows[i]["DocumentType"];
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


                        // else row["RecordType"] = dtCredit.Rows[i]["Declaration"].ToString(); 
                        row["ID"] = dtCredit.Rows[i]["DocumentID"];
                        row["Declaration"] = dtCredit.Rows[i]["Declaration"].ToString() != "" ? dtCredit.Rows[i]["Declaration"].ToString() : row["RecordType"].ToString();
                        row["CreditGold"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["CreditGold"]);
                        row["DebitGold"] = Comon.ConvertToDecimalQty(dtCredit.Rows[i]["DebitGold"]);

                       

                        _sampleData.Rows.Add(row);

                    }
                }
                dtCredit.Dispose();
                row = null;
            }
            catch { }
        }

        #endregion
        #endregion
        #region Events

        protected void txtAccountID_TextChanged(object sender, EventArgs e)
        {
            Acc_Accounts Accounts = new Acc_Accounts();
            Accounts = Acc_AccountsDAL.GetDataByID(Comon.cLong(txtAccountID.Text), 1, 1);
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
            try
            {
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

                PrepareSearchQuery.SearchForAccounts(txtAccountID, lblAccountName, Comon.cInt(cmbBranchesID.EditValue));
                txtAccountID_Validating(null, null);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                switch (view.GetFocusedRowCellValue("TempRecordType").ToString())
                {

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
                        frmCashierPurchaseReturnGold frm15 = new frmCashierPurchaseReturnGold();
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
                        frmGoldOutOnBail frmGolOut = new frmGoldOutOnBail();
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
                        frmCashierPurchaseServicesEqv frmGoldMatirialInvoice = new frmCashierPurchaseServicesEqv();
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + Comon.cDbl(txtAccountID.Text) + ") ";
                CSearch.ControlValidating(txtAccountID, lblAccountName, strSQL);
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
        {

        }
    }
}
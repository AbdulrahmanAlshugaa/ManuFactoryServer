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
    public partial class frmNetworkCommission1 : Edex.GeneralObjects.GeneralForms.BaseForm
    {
      
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string FocusedControl = "";
        private string PrimaryName;

        public DataTable _sampleData = new DataTable();
        public frmNetworkCommission1(long AccountNO) {


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
                where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + UserInfo.BRANCHID;




                _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Balance", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));
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



        public frmNetworkCommission1(long AccountNO, bool yes)
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
                where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + UserInfo.BRANCHID;




                _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Balance", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));
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







        public frmNetworkCommission1()
        {
            try{
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
            FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

            DoNew();
            }
            catch { }
        }

        private void frmAccountStatement_Load(object sender, EventArgs e)
        {
            try{
            where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + UserInfo.BRANCHID;

          


            _sampleData.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Balance", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Debit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Credit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Declaration", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TheDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("OppsiteAccountName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));

            InitialFiveRows(_sampleData, 1);

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

                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["MainAccountID"].Value = cmbNetType.EditValue.ToString();
                rptForm.Parameters["MainAccountName"].Value = cmbNetType.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["TotalDebit"].Value = lblDebit.Text.Trim().ToString();
                rptForm.Parameters["TotalCredit"].Value = lblCredit.Text.Trim().ToString();
                rptForm.Parameters["TotalBalance"].Value = lblBalanceSum.Text.Trim().ToString();

                //rptForm.Parameters["TotalDebit1"].Value = lblDebit1.Text.Trim().ToString();
                //rptForm.Parameters["TotalCredit1"].Value = lblCredit1.Text.Trim().ToString();
                //rptForm.Parameters["TotalBalance1"].Value = lblBalanceSum1.Text.Trim().ToString();
                //rptForm.Parameters["CurrentBalance"].Value = lblBalanceType1.Text.Trim().ToString();
                //rptForm.Parameters["EndBalance"].Value = lblBalanceType.Text.Trim().ToString();
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
                    for (int i = 0; i <= GridView1.DataRowCount - 1; i++)
                    {
                        var row = dataTable.NewRow();

                        row["n_invoice_serial"] = i + 1;
                        row["Balance"] = GridView1.GetRowCellValue(i, "Balance").ToString();
                        row["Debit"] = GridView1.GetRowCellValue(i, "Debit").ToString();
                        row["Credit"] = GridView1.GetRowCellValue(i, "Credit").ToString();
                        row["OppsiteAccountName"] = GridView1.GetRowCellValue(i, "OppsiteAccountName").ToString();
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
            try{

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;

            if (FocusedControl.Trim() == txtAccountID.Name)
            {
                

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            { 
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
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
                // Copy data from GridView into DataTable----------------------
                DataTable dt = new DataTable(); DataRow row;
                ////DataColumn[] dcs = new DataColumn[];
                //foreach (DataGridViewColumn c in GridView.Columns) {
                //    DataColumn dc = new DataColumn();
                //    dc.ColumnName = c.Name;
                //    dc.DataType = System.Type.GetType("System.String");
                //    //c.ValueType
                //    dt.Columns.Add(dc);
                //}
                //foreach (DataGridViewRow r in GridView.Rows) {
                //    DataRow drow = dt.NewRow();
                //    foreach (DataGridViewCell cell in r.Cells) {
                //        drow(cell.OwningColumn.Name) = cell.Value;
                //    }
                //    dt.Rows.Add(drow);
                //}
                ////-------------------------------------------------------------
                dt = _sampleData.Copy();
                DataView view = dt.DefaultView;
                view.Sort = "TheDate ASC, RegTime ASC";
                _sampleData.Rows.Clear();

                for (int i = 0; i <= view.Count - 1; i++)
                {

                    row = _sampleData.NewRow();

                    row["n_invoice_serial"] = i + 1;
                    row["Debit"] = view[i]["Debit"];
                    row["Credit"] = SaveVouchersDiscount(Comon.cDbl(view[i]["Debit"].ToString()), Comon.cDbl(cmbNetType.EditValue)).ToString(); ;
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
                DataRow row;

                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                {
                    credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                    debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                }
                total = credit - debit;

                row = _sampleData.NewRow();
                row["Debit"] = debit;
                row["Credit"] = credit;
                row["Balance"] = Math.Abs(total).ToString();


                if (total < 0)
                {
                    row["Declaration"] = "الاجمالي";// (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    row["Declaration"] = "الاجمالي";// (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");

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
                lblDebit.Text = debit.ToString();

                lblCredit.Text = credit.ToString();

               // lblBalanceSum.Text = Math.Abs(total).ToString();
            }
            catch { }

        }
        private decimal SaveVouchersDiscount(double CreditAmount, double InvoiceID)
        {




            double AccountID = 0;
            double PercentDiscount;
            decimal dCreditAmount = 0;
            DataRow[] row;


            var sr = "Select * from NetType where NetTypeID =" + InvoiceID;
            var dt = Lip.SelectRecord(sr);
            if (dt.Rows.Count < 1) return 0;

            if (Comon.cInt(dt.Rows[0]["ISFixed"].ToString()) > 0)
            {

                if (Comon.cInt(dt.Rows[0]["ISamountFixed"].ToString()) > 0)
                {

                    CreditAmount = Comon.cDbl(dt.Rows[0]["amountFixed"].ToString());
                }
                else
                {

                    CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["percentFixed"].ToString()) / 100);

                }


            }
            else if (Comon.cInt(dt.Rows[0]["ISChange"].ToString()) > 0)
            {

                if (CreditAmount <= Comon.cDbl(dt.Rows[0]["CostLess"].ToString()))
                {
                    if (Comon.cInt(dt.Rows[0]["ISamountChangeLess"].ToString()) > 0)
                    {
                        CreditAmount = Comon.cDbl(dt.Rows[0]["amountChangeLess"].ToString());

                    }
                    else if (Comon.cInt(dt.Rows[0]["ISPercentChangeLess"].ToString()) > 0)
                    {

                        CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["PercentChangeLess"].ToString()) / 100);

                    }
                }
                else
                {
                    if (Comon.cInt(dt.Rows[0]["ISamountChangeMore"].ToString()) > 0)
                    {

                        CreditAmount = Comon.cDbl(dt.Rows[0]["amountChangeMore"].ToString());

                    }

                    else if (Comon.cInt(dt.Rows[0]["ISPercentChangeMore"].ToString()) > 0)
                    {

                        CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["PercentChangeMore"].ToString()) / 100);

                    }
                }


            }
            AccountID = Comon.cDbl(cmbNetType.EditValue);
            dCreditAmount = Comon.ConvertToDecimalPrice(CreditAmount);

            return dCreditAmount;

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
                    row["Declaration"] = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
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
                    lblBalanceType.Text = (UserInfo.Language == iLanguage.English ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                }

            }
            catch { }
            return row;
        }

        


        protected override void DoNew()
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
                lblCredit1.Text = "";
                lblDebit.Text = "";
                lblDebit1.Text = "";
                lblBalanceSum.Text = "";
                lblBalanceSum1.Text = "";
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
                FromDate = 0;
                //ToDate = Comon.cLong(txtToDate.serial_date);

                _sampleData.Rows.Clear();
                ProgressBar.Visible = true;
                ProgressBar.Maximum =150;
                ProgressBar.Minimum = 0;
                ProgressBar.Value = ProgressBar.Value + 10;
                PurchaseInvoice(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                DicountOnPurchaseInvoice(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                PurchaseInvoiceReturn(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                DicountOnPurchaseInvoiceReturn(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                TransportOnPurchaseInvoice(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                SalesInvoice(AccountID, FromDate, ToDate);

                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                DicountOnSalesInvoice(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                SalesInvoiceReturn(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                DicountOnSalesInvoiceReturn(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                ReceiptVoucher(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                SpendVoucher(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                CheckReceiptVoucher(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                CheckSpendVoucher(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;
                ProgressBar.Value = ProgressBar.Value + 10;
                VariousVoucher(AccountID, FromDate, ToDate);
                //ProgressBar.Value = ProgressBar.Value + 1;


                SortData();
                //ProgressBar.Value = ProgressBar.Value + 1;

                //Totals();
                //ProgressBar.Value = ProgressBar.Value + 1;

                //FilteringData()
                //ProgressBar.Value = ProgressBar.Value + 1;

                for (int i = 0; i <= _sampleData.Rows.Count - 2; i++)
                {
                    _sampleData.Rows[i]["Balance"] = Comon.ConvertToDecimalPrice(Math.Abs(Comon.cDbl(_sampleData.Rows[i]["Balance"])));
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
                        if (BeforeDebit >= BeforeCredit)
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

                //dr["Balance"] = BeforeBalance;
                dr["Debit"] = BeforeDebit;
                dr["Credit"] = BeforeCredit;
                dr["Declaration"] = BeforeBalanceType;
                dr["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                _sampleData.Rows.InsertAt(dr, 0);

                //رصيد الفترة من دون اول المدة
                for (int i = 1; i < _sampleData.Rows.Count; i++)
                {
                    periodDebit = Comon.cDbl(Comon.ConvertToDecimalPrice(periodDebit) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    periodCredit = Comon.cDbl(Comon.ConvertToDecimalPrice(periodCredit) + Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                }
                periodBalance = periodDebit - periodCredit;

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

            }
            catch { }
        }

        public void ProcessWithOutDate(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                ProgressBar.Visible = true;
                ProgressBar.Maximum = 170;
                ProgressBar.Minimum = 0;

                _sampleData.Rows.Clear();
                PurchaseInvoice(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                DicountOnPurchaseInvoice(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                PurchaseInvoiceReturn(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                DicountOnPurchaseInvoiceReturn(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                TransportOnPurchaseInvoice(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                SalesInvoice(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                DicountOnSalesInvoice(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                SalesInvoiceReturn(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                DicountOnSalesInvoiceReturn(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                ReceiptVoucher(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                SpendVoucher(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                CheckReceiptVoucher(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                CheckSpendVoucher(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;
                VariousVoucher(AccountID, FromDate, ToDate);
                ProgressBar.Value = ProgressBar.Value + 10;

                RemoveRecordsWithZeroCreditAndDebit();

                SortData();
                ProgressBar.Value = ProgressBar.Value + 10;
                Totals();
              ProgressBar.Value = ProgressBar.Value + 10;

                //_sampleData.Rows.RemoveAt(_sampleData.Rows.Count - 1);
                //Totals();

                FilteringData(FromDate, ToDate);
             ProgressBar.Value = ProgressBar.Value + 1;

                for (int i = 0; i <= _sampleData.Rows.Count - 1; i++)
                {
                    _sampleData.Rows[i]["Balance"] = Comon.ConvertToDecimalPrice(Math.Abs(Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"])));
                }
                if (FromDate != 0) {

                    Totals(true);
                
                
                }

                //_sampleData.Rows(_sampleData.Rows.Count - 1).Cells(dgvColBalance.Name).Style.BackColor = Color.Aquamarine;
                //_sampleData.Rows(_sampleData.Rows.Count - 1).Cells(dgvColBalance.Name).Style.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
                //_sampleData.Rows(_sampleData.Rows.Count - 1).Cells(dgvColDeclaration.Name).Style.BackColor = Color.Aquamarine;

            }
            catch { }
        }

        private void Totals(bool p)
        {
            try
            {
                decimal total = 0;
                decimal credit = 0;
                decimal debit = 0;
                decimal sum = 0;
                DataRow row;

                for (int i = 1; i <= _sampleData.Rows.Count - 1; i++)
                {
                    if(i==1)
                    sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[0]["Balance"]);

                    credit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"]));
                    debit += (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                   // _sampleData.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Debit"]));
                    sum = Comon.ConvertToDecimalPrice(_sampleData.Rows[i]["Balance"]);
                }
                total = credit - debit;

             


                //------------------
                if (total < 0)
                {
                    lblBalanceType1.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance Current term Debit" : "الرصيد الفترة المحدده مدين");
                }
                else
                {
                    lblBalanceType1.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance Current term Credit" : "الرصيد الفترة المحدده دائن");
                }
                lblDebit1.Text = debit.ToString();
                lblCredit1.Text = credit.ToString();
                lblBalanceSum1.Text = Math.Abs(total).ToString();
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
                            //row = TotalsRow();
                           
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
                            row["Debit"] = 0;
                            row["Credit"] = 0;
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
                    row["Debit"] = 0;
                    row["Credit"] = 0;
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

                    //_sampleData.Rows[0]["Debit"] = null;
                    //_sampleData.Rows[0]["Credit"] = null;

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
        #region processData
        private void PurchaseInvoice(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable();
                DataRow row;
                decimal Net = 0; DataSet ds = new DataSet();

                strSQL = "SELECT  SUM(Sales_PurchaseInvoiceDetails.QTY  * Sales_PurchaseInvoiceDetails.COSTPRICE) AS TotalBalance, SUM(Sales_PurchaseInvoiceDetails.DISCOUNT) "
                + " + Sales_PurchaseInvoiceMaster.DISCOUNTONTOTAL AS TotalDiscount,Sales_PurchaseInvoiceMaster.TRANSPORTDEBITAMOUNT,Sales_PurchaseInvoiceMaster.AdditionaAmountTotal,Sales_PurchaseInvoiceMaster.NOTES"
                + " AS Declaration,Sales_PurchaseInvoiceMaster.INVOICEDATE AS TheDate , Sales_PurchaseInvoiceMaster.NetAmount , 'PurchaseInvoice' AS RecordType,Sales_PurchaseInvoiceMaster.INVOICEID AS ID,"
                + " Sales_PurchaseInvoiceMaster.BranchID,Sales_PurchaseInvoiceMaster.RegTime,Sales_PurchaseInvoiceMaster.DEBITACCOUNT,Sales_PurchaseInvoiceMaster.CREDITACCOUNT,"
                + " Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_PurchaseInvoiceMaster INNER JOIN Sales_PurchaseInvoiceDetails ON Sales_PurchaseInvoiceMaster.INVOICEID"
                + " = Sales_PurchaseInvoiceDetails.INVOICEID AND Sales_PurchaseInvoiceMaster.BranchID= Sales_PurchaseInvoiceDetails.BranchID AND Sales_PurchaseInvoiceDetails.FacilityID"
                + " = Sales_PurchaseInvoiceMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceMaster.BranchID = Acc_Accounts.BranchID AND "

                + " Sales_PurchaseInvoiceMaster.DEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Sales_PurchaseInvoiceMaster.FacilityID = Acc_Accounts.FacilityID";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_PurchaseInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                strSQL = strSQL + " GROUP BY Sales_PurchaseInvoiceMaster.NetAmount  , Sales_PurchaseInvoiceMaster.AdditionaAmountTotal, Sales_PurchaseInvoiceMaster.TRANSPORTDEBITAMOUNT,Sales_PurchaseInvoiceMaster.NOTES,Sales_PurchaseInvoiceMaster.INVOICEDATE,Sales_PurchaseInvoiceMaster.INVOICEID,"
                + " Sales_PurchaseInvoiceMaster.BranchID,Sales_PurchaseInvoiceMaster.FacilityID,Sales_PurchaseInvoiceMaster.RegTime,Sales_PurchaseInvoiceMaster.DEBITACCOUNT,"
                + " Sales_PurchaseInvoiceMaster.CREDITACCOUNT,Acc_Accounts.ArbName,Sales_PurchaseInvoiceMaster.DISCOUNTONTOTAL,Sales_PurchaseInvoiceMaster.CANCEL,"
                + " Sales_PurchaseInvoiceDetails.CANCEL HAVING Sales_PurchaseInvoiceMaster.INVOICEDATE > 0 AND Sales_PurchaseInvoiceMaster.INVOICEID > 0 AND "
                + " Sales_PurchaseInvoiceMaster.BranchID=" + UserInfo.BRANCHID + " AND Sales_PurchaseInvoiceMaster.FacilityID =" + UserInfo.FacilityID
                + " AND Sales_PurchaseInvoiceMaster.CREDITACCOUNT =" + AccountID + " AND Sales_PurchaseInvoiceMaster.CANCEL= 0 AND Sales_PurchaseInvoiceDetails.CANCEL= 0";


                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_PurchaseInvoiceMaster.InvoiceDate,Sales_PurchaseInvoiceMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            // row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            if (dtCredit.Rows[i]["ID"].ToString() == "0")
                            {
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "بضاعة أول المدة" : "Goods Opening");
                            }
                            else
                            {
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات" : "Purchase Invoice");
                            }
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = (Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["TotalBalance"]) - Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["TotalDiscount"])) + Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["TransportDebitAmount"]) + Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["AdditionaAmountTotal"]) - Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["NetAmount"]);
                            row["Credit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                            row["Debit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }

                //----------------------------------------
                strSQL = "SELECT  SUM(Sales_PurchaseInvoiceDetails.QTY  * Sales_PurchaseInvoiceDetails.COSTPRICE)AS TotalBalance,SUM(Sales_PurchaseInvoiceDetails.DISCOUNT) + "
                + "  Sales_PurchaseInvoiceMaster.DISCOUNTONTOTAL AS TotalDiscount,Sales_PurchaseInvoiceMaster.TRANSPORTDEBITAMOUNT,Sales_PurchaseInvoiceMaster.NOTES AS"
                + "  Declaration,Sales_PurchaseInvoiceMaster.INVOICEDATE AS TheDate,'PurchaseInvoice'AS RecordType,Sales_PurchaseInvoiceMaster.INVOICEID AS ID,"
                + "  Sales_PurchaseInvoiceMaster.BranchID,Sales_PurchaseInvoiceMaster.RegTime,Sales_PurchaseInvoiceMaster.DEBITACCOUNT,Sales_PurchaseInvoiceMaster.CREDITACCOUNT,"
                + "  Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_PurchaseInvoiceMaster INNER JOIN Sales_PurchaseInvoiceDetails ON Sales_PurchaseInvoiceMaster.INVOICEID"
                + " = Sales_PurchaseInvoiceDetails.INVOICEID AND Sales_PurchaseInvoiceMaster.BranchID= Sales_PurchaseInvoiceDetails.BranchID AND Sales_PurchaseInvoiceDetails.FacilityID"
                + " = Sales_PurchaseInvoiceMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceMaster.BranchID= Acc_Accounts.BranchID AND Sales_PurchaseInvoiceMaster.CREDITACCOUNT"
                + " = Acc_Accounts.ACCOUNTID AND Sales_PurchaseInvoiceMaster.FacilityID= Acc_Accounts.FacilityID ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_PurchaseInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                strSQL = strSQL + " GROUP BY Sales_PurchaseInvoiceMaster.TRANSPORTDEBITAMOUNT,"
               + "  Sales_PurchaseInvoiceMaster.NOTES,Sales_PurchaseInvoiceMaster.INVOICEDATE,Sales_PurchaseInvoiceMaster.INVOICEID,Sales_PurchaseInvoiceMaster.BranchID,"
               + " Sales_PurchaseInvoiceMaster.FacilityID,Sales_PurchaseInvoiceMaster.RegTime,Sales_PurchaseInvoiceMaster.DEBITACCOUNT,Sales_PurchaseInvoiceMaster.CREDITACCOUNT,Acc_Accounts.ArbName,"
               + "  Sales_PurchaseInvoiceMaster.DISCOUNTONTOTAL,Sales_PurchaseInvoiceMaster.CANCEL,Sales_PurchaseInvoiceDetails.CANCEL HAVING Sales_PurchaseInvoiceMaster.INVOICEDATE > 0"
               + " AND Sales_PurchaseInvoiceMaster.INVOICEID > 0 AND Sales_PurchaseInvoiceMaster.BranchID=" + UserInfo.BRANCHID + " AND Sales_PurchaseInvoiceMaster.FacilityID =" + UserInfo.FacilityID.ToString()
               + " AND Sales_PurchaseInvoiceMaster.DEBITACCOUNT= " + AccountID + " AND Sales_PurchaseInvoiceMaster.CANCEL = 0";
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_PurchaseInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_PurchaseInvoiceMaster.InvoiceDate,Sales_PurchaseInvoiceMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtDebit = Lip.SelectRecord(strSQL);
                    if (dtDebit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            // row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtDebit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات" : "Sales Invoice");
                            row["ID"] = dtDebit.Rows[i]["ID"];
                            if (dtDebit.Rows[i]["ID"].ToString() == "0")
                            {
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "بضاعة أول المدة" : "Goods Opening");
                            }
                            else
                            {
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات" : "Purchase Invoice");
                            }
                            row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["TotalBalance"]);
                            row["Credit"] = 0;
                            row["Debit"] = Net; ;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);





                        }
                    }
                }


                //------------------------------------------
                strSQL = "SELECT  Sales_PurchaseInvoiceMaster.AdditionaAmountTotal ,Sales_PurchaseInvoiceMaster.NOTES "
              + " AS Declaration,Sales_PurchaseInvoiceMaster.INVOICEDATE AS TheDate, 'PurchaseInvoice' AS RecordType,Sales_PurchaseInvoiceMaster.INVOICEID AS ID,"
              + " Sales_PurchaseInvoiceMaster.BranchID,Sales_PurchaseInvoiceMaster.RegTime, "
              + " Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_PurchaseInvoiceMaster INNER JOIN Sales_PurchaseInvoiceDetails ON Sales_PurchaseInvoiceMaster.INVOICEID"
              + " = Sales_PurchaseInvoiceDetails.INVOICEID AND Sales_PurchaseInvoiceMaster.BranchID= Sales_PurchaseInvoiceDetails.BranchID AND Sales_PurchaseInvoiceDetails.FacilityID"
              + " = Sales_PurchaseInvoiceMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceMaster.BranchID = Acc_Accounts.BranchID AND "
              + " Sales_PurchaseInvoiceMaster.DEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Sales_PurchaseInvoiceMaster.FacilityID = Acc_Accounts.FacilityID";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_PurchaseInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                strSQL = strSQL + " GROUP BY Sales_PurchaseInvoiceMaster.AdditionaAmountTotal , Sales_PurchaseInvoiceMaster.AdditionalAccount ,Sales_PurchaseInvoiceMaster.NOTES,Sales_PurchaseInvoiceMaster.INVOICEDATE,Sales_PurchaseInvoiceMaster.INVOICEID,"
                 + " Sales_PurchaseInvoiceMaster.BranchID,Sales_PurchaseInvoiceMaster.FacilityID,Sales_PurchaseInvoiceMaster.RegTime,"
                 + " Acc_Accounts.ArbName,Sales_PurchaseInvoiceMaster.CANCEL,"
                 + " Sales_PurchaseInvoiceDetails.CANCEL HAVING Sales_PurchaseInvoiceMaster.INVOICEDATE > 0 AND Sales_PurchaseInvoiceMaster.INVOICEID > 0 AND "
                 + " Sales_PurchaseInvoiceMaster.BranchID=" + UserInfo.BRANCHID + " AND Sales_PurchaseInvoiceMaster.FacilityID =" + UserInfo.FacilityID.ToString()
                 + " AND Sales_PurchaseInvoiceMaster.AdditionalAccount =" + AccountID + " AND Sales_PurchaseInvoiceMaster.CANCEL= 0 AND Sales_PurchaseInvoiceDetails.CANCEL= 0";


                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_PurchaseInvoiceMaster.InvoiceDate,Sales_PurchaseInvoiceMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            // row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            if (dtCredit.Rows[i]["ID"].ToString() == "0")
                            {
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "بضاعة أول المدة" : "Goods Opening");
                            }
                            else
                            {
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات" : "Purchase Invoice");
                            }
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["AdditionaAmountTotal"]);
                            row["Credit"] = 0;
                            row["Debit"] = Net.ToString("N" + MySession.GlobalPriceDigits); ;
                            row["Balance"] = 0;

                            _sampleData.Rows.Add(row);
                            _sampleData.Rows.Add();


                        }
                    }
                }


                strSQL = "SELECT  SUM(Sales_PurchaseInvoiceDetails.QTY  * Sales_PurchaseInvoiceDetails.COSTPRICE) AS TotalBalance, SUM(Sales_PurchaseInvoiceDetails.DISCOUNT) "
                + " + Sales_PurchaseInvoiceMaster.DISCOUNTONTOTAL AS TotalDiscount,Sales_PurchaseInvoiceMaster.TRANSPORTDEBITAMOUNT,Sales_PurchaseInvoiceMaster.AdditionaAmountTotal,Sales_PurchaseInvoiceMaster.NOTES"
                + " AS Declaration,Sales_PurchaseInvoiceMaster.INVOICEDATE AS TheDate , Sales_PurchaseInvoiceMaster.NetAmount , 'PurchaseInvoice' AS RecordType,Sales_PurchaseInvoiceMaster.INVOICEID AS ID,"
                + " Sales_PurchaseInvoiceMaster.BranchID,Sales_PurchaseInvoiceMaster.RegTime,Sales_PurchaseInvoiceMaster.DEBITACCOUNT,Sales_PurchaseInvoiceMaster.CREDITACCOUNT,"
                + " Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_PurchaseInvoiceMaster INNER JOIN Sales_PurchaseInvoiceDetails ON Sales_PurchaseInvoiceMaster.INVOICEID"
                + " = Sales_PurchaseInvoiceDetails.INVOICEID AND Sales_PurchaseInvoiceMaster.BranchID= Sales_PurchaseInvoiceDetails.BranchID AND Sales_PurchaseInvoiceDetails.FacilityID"
                + " = Sales_PurchaseInvoiceMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceMaster.BranchID = Acc_Accounts.BranchID AND "
                + " Sales_PurchaseInvoiceMaster.DEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Sales_PurchaseInvoiceMaster.FacilityID = Acc_Accounts.FacilityID";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_PurchaseInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                strSQL = strSQL + " GROUP BY Sales_PurchaseInvoiceMaster.NetAccount , Sales_PurchaseInvoiceMaster.NetAmount  , Sales_PurchaseInvoiceMaster.AdditionaAmountTotal, Sales_PurchaseInvoiceMaster.TRANSPORTDEBITAMOUNT,Sales_PurchaseInvoiceMaster.NOTES,Sales_PurchaseInvoiceMaster.INVOICEDATE,Sales_PurchaseInvoiceMaster.INVOICEID,"
                + " Sales_PurchaseInvoiceMaster.BranchID,Sales_PurchaseInvoiceMaster.FacilityID,Sales_PurchaseInvoiceMaster.RegTime,Sales_PurchaseInvoiceMaster.DEBITACCOUNT,"
                + " Sales_PurchaseInvoiceMaster.CREDITACCOUNT,Acc_Accounts.ArbName,Sales_PurchaseInvoiceMaster.DISCOUNTONTOTAL,Sales_PurchaseInvoiceMaster.CANCEL,"
                + " Sales_PurchaseInvoiceDetails.CANCEL HAVING Sales_PurchaseInvoiceMaster.INVOICEDATE > 0 AND Sales_PurchaseInvoiceMaster.INVOICEID > 0 AND "
                + " Sales_PurchaseInvoiceMaster.BranchID=" + UserInfo.BRANCHID + " AND Sales_PurchaseInvoiceMaster.FacilityID =" + UserInfo.FacilityID
                + " AND Sales_PurchaseInvoiceMaster.NetAccount =" + AccountID + " AND Sales_PurchaseInvoiceMaster.CANCEL= 0 AND Sales_PurchaseInvoiceDetails.CANCEL= 0 And Sales_PurchaseInvoiceMaster.NetAmount >0";


                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_PurchaseInvoiceMaster.InvoiceDate,Sales_PurchaseInvoiceMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();

                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            if (dtCredit.Rows[i]["ID"].ToString() == "0")
                            {
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "بضاعة أول المدة" : "Goods Opening");
                            }
                            else
                            {
                                row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات" : "Purchase Invoice");
                            }
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = (Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["NetAmount"]));
                            row["Credit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                            row["Debit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }

                dtCredit.Dispose();
                dtDebit.Dispose();

                row = null;
            }
            catch { }
        }

        private void PurchaseInvoiceReturn(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable();
                string strSQL = ""; DataRow row;
                decimal Net = 0; DataSet ds = new DataSet();

                //strSQL = "SELECT SUM(Sales_PurchaseInvoiceReturnDetails.QTY * Sales_PurchaseInvoiceReturnDetails.CostPrice) AS TotalBalance, " + " SUM(Sales_PurchaseInvoiceReturnDetails.Discount) + Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal AS TotalDiscount, " + " Sales_PurchaseInvoiceReturnMaster.Notes AS Declaration, Sales_PurchaseInvoiceReturnMaster.InvoiceDate AS TheDate, " + " Sales_PurchaseInvoiceReturnMaster.RegTime, 'PurchaseInvoiceReturn' AS RecordType, Sales_PurchaseInvoiceReturnMaster.InvoiceID AS ID, " + " Sales_PurchaseInvoiceReturnMaster.BranchID, Sales_PurchaseInvoiceReturnMaster.CreditAccount, Sales_PurchaseInvoiceReturnMaster.DebitAccount, " + " Acc_Accounts.Arb_Name AS OppsiteAccountName" + " FROM Sales_PurchaseInvoiceReturnMaster INNER JOIN" + " Sales_PurchaseInvoiceReturnDetails ON Sales_PurchaseInvoiceReturnMaster.InvoiceID = Sales_PurchaseInvoiceReturnDetails.InvoiceID AND " + " Sales_PurchaseInvoiceReturnMaster.BranchID = Sales_PurchaseInvoiceReturnDetails.BranchID LEFT OUTER JOIN" + " Acc_Accounts ON Sales_PurchaseInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID AND " + " Sales_PurchaseInvoiceReturnMaster.DebitAccount = Acc_Accounts.AccountID" + " GROUP BY Sales_PurchaseInvoiceReturnMaster.InvoiceID, Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal, Sales_PurchaseInvoiceReturnMaster.Cancel, " + " Sales_PurchaseInvoiceReturnMaster.Cancel, Sales_PurchaseInvoiceReturnMaster.BranchID, Sales_PurchaseInvoiceReturnMaster.Notes, " + " Sales_PurchaseInvoiceReturnMaster.InvoiceDate, Sales_PurchaseInvoiceReturnMaster.RegTime, Sales_PurchaseInvoiceReturnMaster.InvoiceID, " + " Sales_PurchaseInvoiceReturnMaster.BranchID, Sales_PurchaseInvoiceReturnMaster.CreditAccount, Sales_PurchaseInvoiceReturnMaster.DebitAccount, " + " Acc_Accounts.Arb_Name " + " HAVING (Sales_PurchaseInvoiceReturnMaster.Cancel = 0) " + " AND (Sales_PurchaseInvoiceReturnMaster.BranchID = " + WT.GlobalBranchID + ") " + " And (Sales_PurchaseInvoiceReturnMaster.CreditAccount = " + txtAccountID.TextWT + ") ";

                strSQL = "SELECT SUM(Sales_PurchaseInvoiceReturnDetails.QTY  * Sales_PurchaseInvoiceReturnDetails.COSTPRICE) AS TOTALBALANCE,SUM(Sales_PurchaseInvoiceReturnDetails.DISCOUNT) + Sales_PurchaseInvoiceReturnMaster.DISCOUNTONTOTAL AS TOTALDISCOUNT,"
                + "Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal, Sales_PurchaseInvoiceReturnMaster.NOTES AS DECLARATION,Sales_PurchaseInvoiceReturnMaster.INVOICEDATE AS THEDATE,Sales_PurchaseInvoiceReturnMaster.RegTime,'PurchaseInvoiceReturn' AS RECORDTYPE,Sales_PurchaseInvoiceReturnMaster.INVOICEID AS ID,"
                + " Sales_PurchaseInvoiceReturnMaster.BranchID,Sales_PurchaseInvoiceReturnMaster.CREDITACCOUNT,Sales_PurchaseInvoiceReturnMaster.DEBITACCOUNT,ACC_ACCOUNTS.ArbName AS OPPSITEACCOUNTNAME FROM Sales_PurchaseInvoiceReturnMaster INNER JOIN"
                + " Sales_PurchaseInvoiceReturnDetails ON Sales_PurchaseInvoiceReturnMaster.INVOICEID = Sales_PurchaseInvoiceReturnDetails.INVOICEID AND Sales_PurchaseInvoiceReturnMaster.BranchID = Sales_PurchaseInvoiceReturnDetails.BranchID AND"
                + " Sales_PurchaseInvoiceReturnDetails.FacilityID = Sales_PurchaseInvoiceReturnMaster.FacilityID LEFT OUTER JOIN ACC_ACCOUNTS ON Sales_PurchaseInvoiceReturnMaster.BranchID = ACC_ACCOUNTS.BranchID AND Sales_PurchaseInvoiceReturnMaster.DEBITACCOUNT"
                + " = ACC_ACCOUNTS.ACCOUNTID AND ACC_ACCOUNTS.FacilityID = Sales_PurchaseInvoiceReturnMaster.FacilityID ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_PurchaseInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                strSQL = strSQL + "GROUP BY    Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal, Sales_PurchaseInvoiceReturnMaster.NOTES,Sales_PurchaseInvoiceReturnMaster.INVOICEDATE,Sales_PurchaseInvoiceReturnMaster.RegTime,"
                 + " Sales_PurchaseInvoiceReturnMaster.INVOICEID,Sales_PurchaseInvoiceReturnMaster.BranchID,Sales_PurchaseInvoiceReturnMaster.CREDITACCOUNT,Sales_PurchaseInvoiceReturnMaster.DEBITACCOUNT,ACC_ACCOUNTS.ArbName,Sales_PurchaseInvoiceReturnMaster.DISCOUNTONTOTAL,"
                 + " Sales_PurchaseInvoiceReturnMaster.CANCEL,Sales_PurchaseInvoiceReturnMaster.FacilityID HAVING Sales_PurchaseInvoiceReturnMaster.BranchID = " + UserInfo.BRANCHID
                 + " AND Sales_PurchaseInvoiceReturnMaster.CREDITACCOUNT = " + AccountID + "  AND Sales_PurchaseInvoiceReturnMaster.CANCEL = 0 AND Sales_PurchaseInvoiceReturnMaster.FacilityID = " + UserInfo.FacilityID.ToString();
                

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_PurchaseInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_PurchaseInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}



                strSQL = strSQL + " ORDER BY Sales_PurchaseInvoiceReturnMaster.InvoiceDate,Sales_PurchaseInvoiceReturnMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();

                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مشتريات" : "Purchase Invoice Return");
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["TotalBalance"]);
                            row["Credit"] = Net;
                            row["Debit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }


                //-------------------------
                //strSQL = "SELECT SUM(Sales_PurchaseInvoiceReturnDetails.QTY * Sales_PurchaseInvoiceReturnDetails.CostPrice) AS TotalBalance, " + " SUM(Sales_PurchaseInvoiceReturnDetails.Discount) + Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal AS TotalDiscount, " + " Sales_PurchaseInvoiceReturnMaster.Notes AS Declaration, Sales_PurchaseInvoiceReturnMaster.InvoiceDate AS TheDate, " + " Sales_PurchaseInvoiceReturnMaster.RegTime, 'PurchaseInvoiceReturn' AS RecordType, Sales_PurchaseInvoiceReturnMaster.InvoiceID AS ID, " + " Sales_PurchaseInvoiceReturnMaster.BranchID, Sales_PurchaseInvoiceReturnMaster.CreditAccount, Sales_PurchaseInvoiceReturnMaster.DebitAccount, " + " Acc_Accounts.Arb_Name AS OppsiteAccountName" + " FROM Sales_PurchaseInvoiceReturnMaster INNER JOIN" + " Sales_PurchaseInvoiceReturnDetails ON Sales_PurchaseInvoiceReturnMaster.InvoiceID = Sales_PurchaseInvoiceReturnDetails.InvoiceID AND " + " Sales_PurchaseInvoiceReturnMaster.BranchID = Sales_PurchaseInvoiceReturnDetails.BranchID LEFT OUTER JOIN" + " Acc_Accounts ON Sales_PurchaseInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID AND " + " Sales_PurchaseInvoiceReturnMaster.CreditAccount = Acc_Accounts.AccountID" + " GROUP BY Sales_PurchaseInvoiceReturnMaster.InvoiceID, Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal, Sales_PurchaseInvoiceReturnMaster.Cancel, " + " Sales_PurchaseInvoiceReturnMaster.Cancel, Sales_PurchaseInvoiceReturnMaster.BranchID, Sales_PurchaseInvoiceReturnMaster.Notes, " + " Sales_PurchaseInvoiceReturnMaster.InvoiceDate, Sales_PurchaseInvoiceReturnMaster.RegTime, Sales_PurchaseInvoiceReturnMaster.InvoiceID, " + " Sales_PurchaseInvoiceReturnMaster.BranchID, Sales_PurchaseInvoiceReturnMaster.CreditAccount, Sales_PurchaseInvoiceReturnMaster.DebitAccount, " + " Acc_Accounts.Arb_Name " + " HAVING (Sales_PurchaseInvoiceReturnMaster.Cancel = 0) " + " AND (Sales_PurchaseInvoiceReturnMaster.BranchID = " + WT.GlobalBranchID + ") " + " And (Sales_PurchaseInvoiceReturnMaster.DebitAccount = " + txtAccountID.TextWT + ") ";
                strSQL = "SELECT SUM(Sales_PurchaseInvoiceReturnDetails.QTY  * Sales_PurchaseInvoiceReturnDetails.COSTPRICE) AS TotalBalance,"
                + " SUM(Sales_PurchaseInvoiceReturnDetails.DISCOUNT) + Sales_PurchaseInvoiceReturnMaster.DISCOUNTONTOTAL AS TotalDiscount,"
                + " Sales_PurchaseInvoiceReturnMaster.NOTES AS Declaration,Sales_PurchaseInvoiceReturnMaster.INVOICEDATE AS TheDate,"
                + " Sales_PurchaseInvoiceReturnMaster.RegTime, Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal,'PurchaseInvoiceReturn' AS RecordType,Sales_PurchaseInvoiceReturnMaster.INVOICEID AS ID,"
                + " Sales_PurchaseInvoiceReturnMaster.BranchID,Sales_PurchaseInvoiceReturnMaster.CREDITACCOUNT,Sales_PurchaseInvoiceReturnMaster.DEBITACCOUNT,"
                + " Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_PurchaseInvoiceReturnMaster INNER JOIN Sales_PurchaseInvoiceReturnDetails"
                + " ON Sales_PurchaseInvoiceReturnMaster.INVOICEID = Sales_PurchaseInvoiceReturnDetails.INVOICEID AND Sales_PurchaseInvoiceReturnMaster.BranchID"
                + " = Sales_PurchaseInvoiceReturnDetails.BranchID AND Sales_PurchaseInvoiceReturnMaster.FacilityID = Sales_PurchaseInvoiceReturnDetails.FacilityID"
                + " LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID AND Sales_PurchaseInvoiceReturnMaster.CREDITACCOUNT"
                + " = Acc_Accounts.ACCOUNTID AND Sales_PurchaseInvoiceReturnMaster.FacilityID= Acc_Accounts.FacilityID ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where Sales_PurchaseInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                strSQL = strSQL + "  GROUP BY Sales_PurchaseInvoiceReturnMaster.NOTES, Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal,"
                + " Sales_PurchaseInvoiceReturnMaster.INVOICEDATE,Sales_PurchaseInvoiceReturnMaster.RegTime,Sales_PurchaseInvoiceReturnMaster.INVOICEID,Sales_PurchaseInvoiceReturnMaster.BranchID"
                + " ,Sales_PurchaseInvoiceReturnMaster.FacilityID, Sales_PurchaseInvoiceReturnMaster.CREDITACCOUNT,Sales_PurchaseInvoiceReturnMaster.DEBITACCOUNT, Acc_Accounts.ArbName,"
                + " Sales_PurchaseInvoiceReturnMaster.DISCOUNTONTOTAL,Sales_PurchaseInvoiceReturnMaster.CANCEL HAVING Sales_PurchaseInvoiceReturnMaster.BranchID = " + UserInfo.BRANCHID
                + " AND Sales_PurchaseInvoiceReturnMaster.FacilityID = " + UserInfo.FacilityID.ToString()
                + " AND Sales_PurchaseInvoiceReturnMaster.DEBITACCOUNT =" + AccountID + " AND Sales_PurchaseInvoiceReturnMaster.CANCEL = 0 ";

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_PurchaseInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_PurchaseInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}




                strSQL = strSQL + " ORDER BY Sales_PurchaseInvoiceReturnMaster.InvoiceDate,Sales_PurchaseInvoiceReturnMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                if (strSQL != null)
                {
                    dtDebit = Lip.SelectRecord(strSQL);
                    if (dtDebit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();

                            row["OppsiteAccountName"] = dtDebit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مشتريات" : "Purchase Invoice Return");
                            row["ID"] = dtDebit.Rows[i]["ID"];
                            row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["TotalBalance"]) - Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["TotalDiscount"]) + Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["AdditionaAmountTotal"]);
                            row["Credit"] = 0;
                            row["Debit"] = Net; ;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }


                strSQL = "SELECT Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal,"
               + " Sales_PurchaseInvoiceReturnMaster.NOTES AS DECLARATION,Sales_PurchaseInvoiceReturnMaster.INVOICEDATE AS THEDATE,Sales_PurchaseInvoiceReturnMaster.RegTime,'PurchaseInvoiceReturn' AS RECORDTYPE,Sales_PurchaseInvoiceReturnMaster.INVOICEID AS ID,"
               + " Sales_PurchaseInvoiceReturnMaster.BranchID,ACC_ACCOUNTS.ArbName AS OPPSITEACCOUNTNAME FROM Sales_PurchaseInvoiceReturnMaster INNER JOIN"
               + " Sales_PurchaseInvoiceReturnDetails ON Sales_PurchaseInvoiceReturnMaster.INVOICEID = Sales_PurchaseInvoiceReturnDetails.INVOICEID AND Sales_PurchaseInvoiceReturnMaster.BranchID = Sales_PurchaseInvoiceReturnDetails.BranchID AND"
               + " Sales_PurchaseInvoiceReturnDetails.FacilityID = Sales_PurchaseInvoiceReturnMaster.FacilityID LEFT OUTER JOIN ACC_ACCOUNTS ON Sales_PurchaseInvoiceReturnMaster.BranchID = ACC_ACCOUNTS.BranchID AND Sales_PurchaseInvoiceReturnMaster.DEBITACCOUNT"
               + " = ACC_ACCOUNTS.ACCOUNTID AND ACC_ACCOUNTS.FacilityID = Sales_PurchaseInvoiceReturnMaster.FacilityID";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_PurchaseInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                strSQL = strSQL + " GROUP BY Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal , Sales_PurchaseInvoiceReturnMaster.NOTES,Sales_PurchaseInvoiceReturnMaster.INVOICEDATE,Sales_PurchaseInvoiceReturnMaster.RegTime,"
               + " Sales_PurchaseInvoiceReturnMaster.INVOICEID,Sales_PurchaseInvoiceReturnMaster.BranchID,Sales_PurchaseInvoiceReturnMaster.AdditionalAccount,ACC_ACCOUNTS.ArbName,"
               + " Sales_PurchaseInvoiceReturnMaster.CANCEL,Sales_PurchaseInvoiceReturnMaster.FacilityID HAVING Sales_PurchaseInvoiceReturnMaster.BranchID = " + UserInfo.BRANCHID
               + " AND Sales_PurchaseInvoiceReturnMaster.AdditionalAccount = " + AccountID + "  AND Sales_PurchaseInvoiceReturnMaster.CANCEL = 0 AND Sales_PurchaseInvoiceReturnMaster.FacilityID = " + UserInfo.FacilityID.ToString();

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_PurchaseInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_PurchaseInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_PurchaseInvoiceReturnMaster.InvoiceDate,Sales_PurchaseInvoiceReturnMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();

                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مشتريات" : "Purchase Invoice Return");
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["AdditionaAmountTotal"]);
                            row["Credit"] = Net;
                            row["Debit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }

                dtCredit.Dispose();
                dtDebit.Dispose();

                row = null;

            }
            catch { }
        }

        private void SalesInvoice(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable(); DataRow row;
                string strSQL = null;
                decimal Net = 0;

                strSQL = "SELECT Sales_SalesInvoiceMaster.NOTES  AS Declaration, Sales_SalesInvoiceMaster.INVOICEDATE AS TheDate, 'SalesInvoice' AS RecordType, Sales_SalesInvoiceMaster.INVOICEID AS ID,Sales_SalesInvoiceMaster.CREDITACCOUNT, Sales_SalesInvoiceMaster.RegTime,Acc_Accounts.ArbName AS OppsiteAccountName,"
                + " Sales_SalesInvoiceMaster.DEBITACCOUNT,Sales_SalesInvoiceMaster.CANCEL,Sales_SalesInvoiceMaster.BranchID,Sales_SalesInvoiceMaster.INVOICEDATE,SUM(Sales_SalesInvoiceDetails.QTY  * Sales_SalesInvoiceDetails.SALEPRICE)  AS TotalBalance,SUM(Sales_SalesInvoiceDetails.DISCOUNT) + Sales_SalesInvoiceMaster.DISCOUNTONTOTAL AS TotalDiscount"
                + " FROM Sales_SalesInvoiceDetails RIGHT OUTER JOIN Sales_SalesInvoiceMaster ON Sales_SalesInvoiceDetails.INVOICEID = Sales_SalesInvoiceMaster.INVOICEID AND Sales_SalesInvoiceDetails.BranchID  = Sales_SalesInvoiceMaster.BranchID AND Sales_SalesInvoiceMaster.FacilityID = Sales_SalesInvoiceDetails.FacilityID"
                + " LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceMaster.DEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Sales_SalesInvoiceMaster.BranchID = Acc_Accounts.BranchID AND Sales_SalesInvoiceMaster.FacilityID   = Acc_Accounts.FacilityID WHERE 1=1  and  Sales_SalesInvoiceMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Sales_SalesInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                strSQL = strSQL + " GROUP BY Sales_SalesInvoiceMaster.NOTES,Sales_SalesInvoiceMaster.INVOICEDATE, Sales_SalesInvoiceMaster.INVOICEID,Sales_SalesInvoiceMaster.CREDITACCOUNT,Sales_SalesInvoiceMaster.RegTime, Acc_Accounts.ArbName, Sales_SalesInvoiceMaster.DEBITACCOUNT, Sales_SalesInvoiceMaster.CANCEL,"
                + " Sales_SalesInvoiceMaster.BranchID,Sales_SalesInvoiceMaster.DISCOUNTONTOTAL,Sales_SalesInvoiceMaster.FacilityID HAVING Sales_SalesInvoiceMaster.CREDITACCOUNT = " + AccountID + " AND Sales_SalesInvoiceMaster.CANCEL = 0 AND Sales_SalesInvoiceMaster.BranchID=" + UserInfo.BRANCHID;

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_SalesInvoiceMaster.InvoiceDate,Sales_SalesInvoiceMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();

                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات" : "Sales Invoice");
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["TotalBalance"]);
                            row["Credit"] = Net;
                            row["Debit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);

                        }

                    }
                }

                strSQL = "SELECT  Sales_SalesInvoiceMaster.NOTES  AS Declaration,Sales_SalesInvoiceMaster.INVOICEDATE AS TheDate,'SalesInvoice'  AS RecordType, Sales_SalesInvoiceMaster.INVOICEID AS ID,Sales_SalesInvoiceMaster.CREDITACCOUNT,Sales_SalesInvoiceMaster.RegTime,"
                + " Acc_Accounts.ArbName AS OppsiteAccountName,Sales_SalesInvoiceMaster.DEBITACCOUNT,Sales_SalesInvoiceMaster.CANCEL,Sales_SalesInvoiceMaster.BranchID, Sales_SalesInvoiceMaster.INVOICEDATE,SUM(Sales_SalesInvoiceDetails.QTY  * Sales_SalesInvoiceDetails.SALEPRICE) AS TotalBalance,"
                + " SUM(Sales_SalesInvoiceDetails.DISCOUNT)  + Sales_SalesInvoiceMaster.DISCOUNTONTOTAL  AS TotalDiscount ,Sales_SalesInvoiceMaster.AdditionaAmountTotal,Sales_SalesInvoiceMaster.NETAMOUNT FROM Sales_SalesInvoiceDetails RIGHT OUTER JOIN Sales_SalesInvoiceMaster ON Sales_SalesInvoiceDetails.INVOICEID = Sales_SalesInvoiceMaster.INVOICEID"
                + " AND Sales_SalesInvoiceDetails.BranchID = Sales_SalesInvoiceMaster.BranchID AND Sales_SalesInvoiceMaster.FacilityID = Sales_SalesInvoiceDetails.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceMaster.CREDITACCOUNT = Acc_Accounts.ACCOUNTID"
                + " AND Sales_SalesInvoiceMaster.BranchID = Acc_Accounts.BranchID AND Acc_Accounts.FacilityID = Sales_SalesInvoiceMaster.FacilityID     where 1=1  ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + "  And  Sales_SalesInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }


                strSQL = strSQL + "   GROUP BY    Sales_SalesInvoiceMaster.AdditionaAmountTotal , Sales_SalesInvoiceMaster.NOTES, Sales_SalesInvoiceMaster.INVOICEDATE,Sales_SalesInvoiceMaster.INVOICEID,"
              + " Sales_SalesInvoiceMaster.CREDITACCOUNT,Sales_SalesInvoiceMaster.RegTime,Acc_Accounts.ArbName,Sales_SalesInvoiceMaster.DEBITACCOUNT,Sales_SalesInvoiceMaster.CANCEL,Sales_SalesInvoiceMaster.BranchID,Sales_SalesInvoiceMaster.NETAMOUNT,Sales_SalesInvoiceMaster.FacilityID,"
              + " Sales_SalesInvoiceMaster.DISCOUNTONTOTAL HAVING Sales_SalesInvoiceMaster.DEBITACCOUNT =" + AccountID + " AND Sales_SalesInvoiceMaster.CANCEL = 0 AND Sales_SalesInvoiceMaster.BranchID =" + UserInfo.BRANCHID + " AND Sales_SalesInvoiceMaster.FacilityID =" + UserInfo.FacilityID.ToString();

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_SalesInvoiceMaster.InvoiceDate,Sales_SalesInvoiceMaster.RegTime";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                if (strSQL != null)
                {
                    dtDebit = Lip.SelectRecord(strSQL);
                    if (dtDebit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtDebit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات" : "Sales Invoice");
                            row["ID"] = dtDebit.Rows[i]["ID"];
                            row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["TotalBalance"]) - Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["TotalDiscount"]);
                            Net = Comon.ConvertToDecimalPrice(Net) + Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["AdditionaAmountTotal"]) - Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["NetAmount"]) ;
                            row["Debit"] = Net.ToString("N" + MySession.GlobalPriceDigits);
                            row["Credit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }

                strSQL = "SELECT  Sales_SalesInvoiceMaster.NOTES  AS Declaration, Sales_SalesInvoiceMaster.INVOICEDATE AS TheDate, 'SalesInvoice' AS RecordType, Sales_SalesInvoiceMaster.INVOICEID AS ID,Sales_SalesInvoiceMaster.CREDITACCOUNT,"
                + " Sales_SalesInvoiceMaster.RegTime, Acc_Accounts.ArbName AS OppsiteAccountName,Sales_SalesInvoiceMaster.DEBITACCOUNT,Sales_SalesInvoiceMaster.CANCEL,Sales_SalesInvoiceMaster.BranchID,Sales_SalesInvoiceMaster.INVOICEDATE,"
                + " SUM(Sales_SalesInvoiceDetails.QTY * Sales_SalesInvoiceDetails.SALEPRICE) AS TotalBalance,SUM(Sales_SalesInvoiceDetails.DISCOUNT)  + Sales_SalesInvoiceMaster.DISCOUNTONTOTAL AS TotalDiscount, Sales_SalesInvoiceMaster.NETACCOUNT,"
                + " Sales_SalesInvoiceMaster.NETAMOUNT FROM Sales_SalesInvoiceDetails RIGHT OUTER JOIN Sales_SalesInvoiceMaster ON Sales_SalesInvoiceDetails.INVOICEID = Sales_SalesInvoiceMaster.INVOICEID AND Sales_SalesInvoiceDetails.BranchID ="
                + " Sales_SalesInvoiceMaster.BranchID AND Sales_SalesInvoiceDetails.FacilityID = Sales_SalesInvoiceMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceMaster.CREDITACCOUNT = Acc_Accounts.ACCOUNTID"
                + " AND Sales_SalesInvoiceMaster.BranchID = Acc_Accounts.BranchID AND Acc_Accounts.FacilityID = Sales_SalesInvoiceMaster.FacilityID   where 1=1    ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " And  Sales_SalesInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }


                strSQL = strSQL + " GROUP BY  Sales_SalesInvoiceMaster.NOTES, Sales_SalesInvoiceMaster.INVOICEDATE,"
              + " Sales_SalesInvoiceMaster.INVOICEID, Sales_SalesInvoiceMaster.CREDITACCOUNT, Sales_SalesInvoiceMaster.RegTime,Acc_Accounts.ArbName, Sales_SalesInvoiceMaster.DEBITACCOUNT, Sales_SalesInvoiceMaster.CANCEL,"
              + " Sales_SalesInvoiceMaster.BranchID,Sales_SalesInvoiceMaster.NETACCOUNT, Sales_SalesInvoiceMaster.NETAMOUNT,Sales_SalesInvoiceMaster.DISCOUNTONTOTAL,Sales_SalesInvoiceMaster.FacilityID HAVING Sales_SalesInvoiceMaster.CANCEL= 0"
              + " AND Sales_SalesInvoiceMaster.BranchID =" + UserInfo.BRANCHID + " AND Sales_SalesInvoiceMaster.FacilityID =" + UserInfo.FacilityID.ToString() + " AND Sales_SalesInvoiceMaster.NETACCOUNT =" + AccountID;

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_SalesInvoiceMaster.InvoiceDate,Sales_SalesInvoiceMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtDebit = Lip.SelectRecord(strSQL);
                    if (dtDebit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtDebit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات" : "Sales Invoice");
                            row["ID"] = dtDebit.Rows[i]["ID"];
                            row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["NetAmount"].ToString());
                            row["Debit"] = Net;
                            row["Credit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);

                        }
                    }
                }

                strSQL = "SELECT Sales_SalesInvoiceMaster.NOTES  AS Declaration, Sales_SalesInvoiceMaster.INVOICEDATE AS TheDate, 'SalesInvoice' AS RecordType, Sales_SalesInvoiceMaster.INVOICEID AS ID,Sales_SalesInvoiceMaster.AdditionalAccount, Sales_SalesInvoiceMaster.RegTime,Acc_Accounts.ArbName AS OppsiteAccountName,"
                + " Sales_SalesInvoiceMaster.DEBITACCOUNT,Sales_SalesInvoiceMaster.CANCEL,Sales_SalesInvoiceMaster.BranchID,Sales_SalesInvoiceMaster.INVOICEDATE , Sales_SalesInvoiceMaster.AdditionaAmountTotal "
                + " FROM Sales_SalesInvoiceDetails RIGHT OUTER JOIN Sales_SalesInvoiceMaster ON Sales_SalesInvoiceDetails.INVOICEID = Sales_SalesInvoiceMaster.INVOICEID AND Sales_SalesInvoiceDetails.BranchID  = Sales_SalesInvoiceMaster.BranchID AND Sales_SalesInvoiceMaster.FacilityID = Sales_SalesInvoiceDetails.FacilityID"
                + " LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceMaster.DEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Sales_SalesInvoiceMaster.BranchID = Acc_Accounts.BranchID AND Sales_SalesInvoiceMaster.FacilityID   = Acc_Accounts.FacilityID WHERE   1=1 And Sales_SalesInvoiceMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Sales_SalesInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }


                strSQL = strSQL + " GROUP BY  Sales_SalesInvoiceMaster.AdditionaAmountTotal , Sales_SalesInvoiceMaster.NOTES,Sales_SalesInvoiceMaster.INVOICEDATE, Sales_SalesInvoiceMaster.INVOICEID,Sales_SalesInvoiceMaster.AdditionalAccount,Sales_SalesInvoiceMaster.RegTime, Acc_Accounts.ArbName, Sales_SalesInvoiceMaster.DEBITACCOUNT, Sales_SalesInvoiceMaster.CANCEL,"
                + " Sales_SalesInvoiceMaster.BranchID,Sales_SalesInvoiceMaster.DISCOUNTONTOTAL,Sales_SalesInvoiceMaster.FacilityID HAVING Sales_SalesInvoiceMaster.AdditionalAccount = " + AccountID + " AND Sales_SalesInvoiceMaster.CANCEL = 0 AND Sales_SalesInvoiceMaster.BranchID=" + UserInfo.BRANCHID;

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_SalesInvoiceMaster.InvoiceDate,Sales_SalesInvoiceMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات" : "Sales Invoice");
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["AdditionaAmountTotal"]);
                            row["Credit"] = Net;
                            row["Debit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);

                        }

                    }
                }

                dtCredit.Dispose();
                dtDebit.Dispose();

                row = null;
            }
            catch { }

        }

        private void SalesInvoiceReturn(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable();
                string strSQL = ""; DataRow row;
                decimal Net = 0;

                //strSQL = "SELECT SUM(Sales_SalesInvoiceReturnDetails.QTY * Sales_SalesInvoiceReturnDetails.SalePrice) AS TotalBalance, " + " SUM(Sales_SalesInvoiceReturnDetails.Discount) + Sales_SalesInvoiceReturnMaster.DiscountOnTotal AS TotalDiscount, " + " Sales_SalesInvoiceReturnMaster.Notes AS Declaration, Sales_SalesInvoiceReturnMaster.InvoiceDate AS TheDate, " + " Sales_SalesInvoiceReturnMaster.RegTime, 'SalesInvoiceReturn' AS RecordType, Sales_SalesInvoiceReturnMaster.InvoiceID AS ID, " + " Sales_SalesInvoiceReturnMaster.DebitAccount, Sales_SalesInvoiceReturnMaster.CreditAccount, Acc_Accounts.Arb_Name AS OppsiteAccountName" + " FROM Sales_SalesInvoiceReturnMaster INNER JOIN" + " Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID AND " + " Sales_SalesInvoiceReturnMaster.BranchID = Sales_SalesInvoiceReturnDetails.BranchID LEFT OUTER JOIN" + " Acc_Accounts ON Sales_SalesInvoiceReturnMaster.DebitAccount = Acc_Accounts.AccountID AND " + " Sales_SalesInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID" + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.DiscountOnTotal, Sales_SalesInvoiceReturnMaster.Cancel, " + " Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.Notes, Sales_SalesInvoiceReturnMaster.InvoiceDate, " + " Sales_SalesInvoiceReturnMaster.RegTime, Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.DebitAccount," + " Sales_SalesInvoiceReturnMaster.CreditAccount, Acc_Accounts.Arb_Name" + " HAVING (Sales_SalesInvoiceReturnMaster.Cancel = 0)" + " AND (Sales_SalesInvoiceReturnMaster.BranchID = " + WT.GlobalBranchID + ")" + " AND (Sales_SalesInvoiceReturnMaster.CreditAccount = " + txtAccountID.TextWT + ") ";
                strSQL = "SELECT SUM(Sales_SalesInvoiceReturnDetails.QTY  * Sales_SalesInvoiceReturnDetails.SALEPRICE) AS TotalBalance, SUM(Sales_SalesInvoiceReturnDetails.DISCOUNT) "
                + " + Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL AS TotalDiscount,Sales_SalesInvoiceReturnMaster.NOTES AS Declaration,Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal,Sales_SalesInvoiceReturnMaster.INVOICEDATE AS TheDate,"
                + " Sales_SalesInvoiceReturnMaster.NetAccount ,Sales_SalesInvoiceReturnMaster.NetAmount , Sales_SalesInvoiceReturnMaster.RegTime,'SalesInvoiceReturn' AS RecordType,Sales_SalesInvoiceReturnMaster.INVOICEID AS ID,Sales_SalesInvoiceReturnMaster.DEBITACCOUNT,"
                + " Sales_SalesInvoiceReturnMaster.CREDITACCOUNT,Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_SalesInvoiceReturnMaster INNER JOIN Sales_SalesInvoiceReturnDetails"
                + " ON Sales_SalesInvoiceReturnMaster.INVOICEID = Sales_SalesInvoiceReturnDetails.INVOICEID AND Sales_SalesInvoiceReturnMaster.BranchID = Sales_SalesInvoiceReturnDetails.BranchID"
                + " AND Sales_SalesInvoiceReturnDetails.FacilityID = Sales_SalesInvoiceReturnMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceReturnMaster.DEBITACCOUNT"
                + " = Acc_Accounts.ACCOUNTID AND Sales_SalesInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID AND Sales_SalesInvoiceReturnMaster.FacilityID = Acc_Accounts.FacilityID";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_SalesInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                strSQL = strSQL + " GROUP BY   Sales_SalesInvoiceReturnMaster.NetAccount ,Sales_SalesInvoiceReturnMaster.NetAmount , Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal,Sales_SalesInvoiceReturnMaster.NOTES,Sales_SalesInvoiceReturnMaster.INVOICEDATE,Sales_SalesInvoiceReturnMaster.RegTime,Sales_SalesInvoiceReturnMaster.INVOICEID,"
                  + " Sales_SalesInvoiceReturnMaster.DEBITACCOUNT,Sales_SalesInvoiceReturnMaster.CREDITACCOUNT,Acc_Accounts.ArbName,Sales_SalesInvoiceReturnMaster.FacilityID,"
                  + " Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL,Sales_SalesInvoiceReturnMaster.CANCEL,Sales_SalesInvoiceReturnMaster.BranchID HAVING Sales_SalesInvoiceReturnMaster.CREDITACCOUNT =" + AccountID
                  + " AND Sales_SalesInvoiceReturnMaster.CANCEL = 0 AND Sales_SalesInvoiceReturnMaster.BranchID =" + UserInfo.BRANCHID + " AND Sales_SalesInvoiceReturnMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_SalesInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_SalesInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_SalesInvoiceReturnMaster.InvoiceDate,Sales_SalesInvoiceReturnMaster.RegTime";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مبيعات" : "Sales Invoice Return");
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["TotalBalance"]) - Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["NetAmount"]) - Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["TotalDiscount"]) + Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["AdditionaAmountTotal"]);
                            row["Credit"] = Net;
                            row["Debit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }

                //------------------------------------------
                //strSQL = "SELECT SUM(Sales_SalesInvoiceReturnDetails.QTY * Sales_SalesInvoiceReturnDetails.SalePrice) AS TotalBalance, " + " SUM(Sales_SalesInvoiceReturnDetails.Discount) + Sales_SalesInvoiceReturnMaster.DiscountOnTotal AS TotalDiscount, " + " Sales_SalesInvoiceReturnMaster.Notes AS Declaration, Sales_SalesInvoiceReturnMaster.InvoiceDate AS TheDate, " + " Sales_SalesInvoiceReturnMaster.RegTime, 'SalesInvoiceReturn' AS RecordType, Sales_SalesInvoiceReturnMaster.InvoiceID AS ID, " + " Sales_SalesInvoiceReturnMaster.DebitAccount, Sales_SalesInvoiceReturnMaster.CreditAccount, Acc_Accounts.Arb_Name AS OppsiteAccountName" + " FROM Sales_SalesInvoiceReturnMaster INNER JOIN" + " Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID AND " + " Sales_SalesInvoiceReturnMaster.BranchID = Sales_SalesInvoiceReturnDetails.BranchID LEFT OUTER JOIN" + " Acc_Accounts ON Sales_SalesInvoiceReturnMaster.CreditAccount = Acc_Accounts.AccountID AND " + " Sales_SalesInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID" + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.DiscountOnTotal, Sales_SalesInvoiceReturnMaster.Cancel, " + " Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.Notes, Sales_SalesInvoiceReturnMaster.InvoiceDate, " + " Sales_SalesInvoiceReturnMaster.RegTime, Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.DebitAccount," + " Sales_SalesInvoiceReturnMaster.CreditAccount, Acc_Accounts.Arb_Name" + " HAVING (Sales_SalesInvoiceReturnMaster.Cancel = 0)" + " AND (Sales_SalesInvoiceReturnMaster.BranchID = " + WT.GlobalBranchID + ")" + " AND (Sales_SalesInvoiceReturnMaster.DebitAccount = " + txtAccountID.TextWT + ") ";

                strSQL = "SELECT SUM(Sales_SalesInvoiceReturnDetails.QTY  * Sales_SalesInvoiceReturnDetails.SALEPRICE) AS TOTALBALANCE,SUM(Sales_SalesInvoiceReturnDetails.DISCOUNT) + Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL AS TOTALDISCOUNT,"
                + " Sales_SalesInvoiceReturnMaster.NOTES AS DECLARATION,Sales_SalesInvoiceReturnMaster.INVOICEDATE AS THEDATE,Sales_SalesInvoiceReturnMaster.RegTime,'SalesInvoiceReturn' AS RECORDTYPE,Sales_SalesInvoiceReturnMaster.INVOICEID"
                + " AS ID,Sales_SalesInvoiceReturnMaster.DEBITACCOUNT, Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal,Sales_SalesInvoiceReturnMaster.CREDITACCOUNT,ACC_ACCOUNTS.ArbName AS OPPSITEACCOUNTNAME FROM Sales_SalesInvoiceReturnMaster INNER JOIN Sales_SalesInvoiceReturnDetails"
                + " ON Sales_SalesInvoiceReturnMaster.INVOICEID = Sales_SalesInvoiceReturnDetails.INVOICEID AND Sales_SalesInvoiceReturnMaster.BranchID=Sales_SalesInvoiceReturnDetails.BranchID AND Sales_SalesInvoiceReturnMaster.FacilityID"
                + " = Sales_SalesInvoiceReturnDetails.FacilityID LEFT OUTER JOIN ACC_ACCOUNTS ON Sales_SalesInvoiceReturnMaster.CREDITACCOUNT = ACC_ACCOUNTS.ACCOUNTID AND Sales_SalesInvoiceReturnMaster.BranchID = ACC_ACCOUNTS.BranchID"
                + " AND ACC_ACCOUNTS.FacilityID = Sales_SalesInvoiceReturnMaster.FacilityID ";

                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_SalesInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                strSQL = strSQL + "  GROUP BY Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal, Sales_SalesInvoiceReturnMaster.NOTES,Sales_SalesInvoiceReturnMaster.INVOICEDATE,Sales_SalesInvoiceReturnMaster.RegTime,"
                 + " Sales_SalesInvoiceReturnMaster.INVOICEID, Sales_SalesInvoiceReturnMaster.DEBITACCOUNT,Sales_SalesInvoiceReturnMaster.CREDITACCOUNT,ACC_ACCOUNTS.ArbName,Sales_SalesInvoiceReturnMaster.FacilityID,"
                 + " Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL,Sales_SalesInvoiceReturnMaster.CANCEL,Sales_SalesInvoiceReturnMaster.BranchID HAVING Sales_SalesInvoiceReturnMaster.DEBITACCOUNT =" + AccountID
                 + " AND Sales_SalesInvoiceReturnMaster.FacilityID =" + UserInfo.FacilityID.ToString() + " AND Sales_SalesInvoiceReturnMaster.CANCEL = 0 AND Sales_SalesInvoiceReturnMaster.BranchID=" + UserInfo.BRANCHID;

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_SalesInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_SalesInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_SalesInvoiceReturnMaster.InvoiceDate,Sales_SalesInvoiceReturnMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtDebit = Lip.SelectRecord(strSQL);
                    if (dtDebit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtDebit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مبيعات" : "Sales Invoice Return");
                            row["ID"] = dtDebit.Rows[i]["ID"];
                            row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);

                            Net = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["TotalBalance"]);
                            row["Credit"] = 0;
                            row["Debit"] = Net; ;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }

                strSQL = "SELECT Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal, Sales_SalesInvoiceReturnMaster.AdditionalAccount ,  "
               + " + Sales_SalesInvoiceReturnMaster.NOTES AS Declaration,Sales_SalesInvoiceReturnMaster.INVOICEDATE AS TheDate,"
               + " Sales_SalesInvoiceReturnMaster.RegTime,'SalesInvoiceReturn' AS RecordType,Sales_SalesInvoiceReturnMaster.INVOICEID AS ID,"
               + " Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_SalesInvoiceReturnMaster INNER JOIN Sales_SalesInvoiceReturnDetails"
               + " ON Sales_SalesInvoiceReturnMaster.INVOICEID = Sales_SalesInvoiceReturnDetails.INVOICEID AND Sales_SalesInvoiceReturnMaster.BranchID = Sales_SalesInvoiceReturnDetails.BranchID"
               + " AND Sales_SalesInvoiceReturnDetails.FacilityID = Sales_SalesInvoiceReturnMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceReturnMaster.DEBITACCOUNT"


         + " = Acc_Accounts.ACCOUNTID AND Sales_SalesInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID AND Sales_SalesInvoiceReturnMaster.FacilityID = Acc_Accounts.FacilityID";

                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_SalesInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }


                strSQL = strSQL + " GROUP BY Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal , Sales_SalesInvoiceReturnMaster.NOTES,Sales_SalesInvoiceReturnMaster.INVOICEDATE,Sales_SalesInvoiceReturnMaster.RegTime,Sales_SalesInvoiceReturnMaster.INVOICEID,"
                + " Sales_SalesInvoiceReturnMaster.AdditionalAccount,Acc_Accounts.ArbName,Sales_SalesInvoiceReturnMaster.FacilityID,"
                + " Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL,Sales_SalesInvoiceReturnMaster.CANCEL,Sales_SalesInvoiceReturnMaster.BranchID HAVING Sales_SalesInvoiceReturnMaster.AdditionalAccount =" + AccountID
                + " AND Sales_SalesInvoiceReturnMaster.CANCEL = 0 AND Sales_SalesInvoiceReturnMaster.BranchID =" + UserInfo.BRANCHID + " AND Sales_SalesInvoiceReturnMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_SalesInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_SalesInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_SalesInvoiceReturnMaster.InvoiceDate,Sales_SalesInvoiceReturnMaster.RegTime";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مبيعات" : "Sales Invoice Return");
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["AdditionaAmountTotal"]);
                            row["Credit"] = 0;
                            row["Debit"] = Net;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }
                //strSQL = "SELECT SUM(Sales_SalesInvoiceReturnDetails.QTY * Sales_SalesInvoiceReturnDetails.SalePrice) AS TotalBalance, " + " SUM(Sales_SalesInvoiceReturnDetails.Discount) + Sales_SalesInvoiceReturnMaster.DiscountOnTotal AS TotalDiscount, " + " Sales_SalesInvoiceReturnMaster.Notes AS Declaration, Sales_SalesInvoiceReturnMaster.InvoiceDate AS TheDate, " + " Sales_SalesInvoiceReturnMaster.RegTime, 'SalesInvoiceReturn' AS RecordType, Sales_SalesInvoiceReturnMaster.InvoiceID AS ID, " + " Sales_SalesInvoiceReturnMaster.DebitAccount, Sales_SalesInvoiceReturnMaster.CreditAccount, Acc_Accounts.Arb_Name AS OppsiteAccountName" + " FROM Sales_SalesInvoiceReturnMaster INNER JOIN" + " Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID AND " + " Sales_SalesInvoiceReturnMaster.BranchID = Sales_SalesInvoiceReturnDetails.BranchID LEFT OUTER JOIN" + " Acc_Accounts ON Sales_SalesInvoiceReturnMaster.DebitAccount = Acc_Accounts.AccountID AND " + " Sales_SalesInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID" + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.DiscountOnTotal, Sales_SalesInvoiceReturnMaster.Cancel, " + " Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.Notes, Sales_SalesInvoiceReturnMaster.InvoiceDate, " + " Sales_SalesInvoiceReturnMaster.RegTime, Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.DebitAccount," + " Sales_SalesInvoiceReturnMaster.CreditAccount, Acc_Accounts.Arb_Name" + " HAVING (Sales_SalesInvoiceReturnMaster.Cancel = 0)" + " AND (Sales_SalesInvoiceReturnMaster.BranchID = " + WT.GlobalBranchID + ")" + " AND (Sales_SalesInvoiceReturnMaster.CreditAccount = " + txtAccountID.TextWT + ") ";
                strSQL = "SELECT SUM(Sales_SalesInvoiceReturnDetails.QTY  * Sales_SalesInvoiceReturnDetails.SALEPRICE) AS TotalBalance, SUM(Sales_SalesInvoiceReturnDetails.DISCOUNT) "
                + " + Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL AS TotalDiscount,Sales_SalesInvoiceReturnMaster.NOTES AS Declaration,Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal,Sales_SalesInvoiceReturnMaster.INVOICEDATE AS TheDate,"
                + " Sales_SalesInvoiceReturnMaster.NetAccount ,Sales_SalesInvoiceReturnMaster.NetAmount , Sales_SalesInvoiceReturnMaster.RegTime,'SalesInvoiceReturn' AS RecordType,Sales_SalesInvoiceReturnMaster.INVOICEID AS ID,Sales_SalesInvoiceReturnMaster.DEBITACCOUNT,"
                + " Sales_SalesInvoiceReturnMaster.CREDITACCOUNT,Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_SalesInvoiceReturnMaster INNER JOIN Sales_SalesInvoiceReturnDetails"
                + " ON Sales_SalesInvoiceReturnMaster.INVOICEID = Sales_SalesInvoiceReturnDetails.INVOICEID AND Sales_SalesInvoiceReturnMaster.BranchID = Sales_SalesInvoiceReturnDetails.BranchID"
                + " AND Sales_SalesInvoiceReturnDetails.FacilityID = Sales_SalesInvoiceReturnMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceReturnMaster.DEBITACCOUNT"

             + " = Acc_Accounts.ACCOUNTID AND Sales_SalesInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID AND Sales_SalesInvoiceReturnMaster.FacilityID = Acc_Accounts.FacilityID";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_SalesInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                strSQL = strSQL + " GROUP BY   Sales_SalesInvoiceReturnMaster.NetAccount ,Sales_SalesInvoiceReturnMaster.NetAmount , Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal,Sales_SalesInvoiceReturnMaster.NOTES,Sales_SalesInvoiceReturnMaster.INVOICEDATE,Sales_SalesInvoiceReturnMaster.RegTime,Sales_SalesInvoiceReturnMaster.INVOICEID,"
               + " Sales_SalesInvoiceReturnMaster.DEBITACCOUNT,Sales_SalesInvoiceReturnMaster.CREDITACCOUNT,Acc_Accounts.ArbName,Sales_SalesInvoiceReturnMaster.FacilityID,"
               + " Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL,Sales_SalesInvoiceReturnMaster.CANCEL,Sales_SalesInvoiceReturnMaster.BranchID HAVING Sales_SalesInvoiceReturnMaster.NetAccount =" + AccountID
               + " AND Sales_SalesInvoiceReturnMaster.CANCEL = 0 AND Sales_SalesInvoiceReturnMaster.NetAmount > 0 AND Sales_SalesInvoiceReturnMaster.BranchID =" + UserInfo.BRANCHID + " AND Sales_SalesInvoiceReturnMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_SalesInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " And Sales_SalesInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_SalesInvoiceReturnMaster.InvoiceDate,Sales_SalesInvoiceReturnMaster.RegTime";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مبيعات" : "Sales Invoice Return");
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            Net = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["NetAmount"]);
                            row["Credit"] = Net;
                            row["Debit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }
                dtCredit.Dispose();
                dtDebit.Dispose();

                row = null;
            }
            catch { }
        }

        private void DicountOnPurchaseInvoice(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = ""; DataRow row;
                DataSet ds = new DataSet();

                strSQL = "SELECT 'PurchaseInvoice' AS RecordType,SUM(Sales_PurchaseInvoiceDetails.DISCOUNT) + Sales_PurchaseInvoiceMaster.DISCOUNTONTOTAL AS Discount,"
                + " Sales_PurchaseInvoiceMaster.INVOICEID AS ID,Sales_PurchaseInvoiceMaster.INVOICEDATE AS TheDate,Sales_PurchaseInvoiceMaster.NOTES AS Declaration,"
                + " Acc_Accounts.ArbName AS OppsiteAccountName, 0 AS Debit FROM Sales_PurchaseInvoiceMaster INNER JOIN Sales_PurchaseInvoiceDetails ON "
                + " Sales_PurchaseInvoiceMaster.INVOICEID = Sales_PurchaseInvoiceDetails.INVOICEID AND Sales_PurchaseInvoiceMaster.BranchID = "
                + " Sales_PurchaseInvoiceDetails.BranchID AND Sales_PurchaseInvoiceDetails.FacilityID = Sales_PurchaseInvoiceMaster.FacilityID"
                + " LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceMaster.BranchID= Acc_Accounts.BranchID AND Sales_PurchaseInvoiceMaster.DEBITACCOUNT "
                + " = Acc_Accounts.ACCOUNTID AND Sales_PurchaseInvoiceMaster.FacilityID  = Acc_Accounts.FacilityID ";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_PurchaseInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }


                strSQL = strSQL + " GROUP BY Sales_PurchaseInvoiceMaster.INVOICEID,"
               + " Sales_PurchaseInvoiceMaster.INVOICEDATE,Sales_PurchaseInvoiceMaster.NOTES,Acc_Accounts.ArbName,Sales_PurchaseInvoiceMaster.DISCOUNTONTOTAL,"
               + " Sales_PurchaseInvoiceMaster.BranchID,Sales_PurchaseInvoiceMaster.FacilityID,Sales_PurchaseInvoiceMaster.CANCEL,Sales_PurchaseInvoiceMaster.DISCOUNTCREDITACCOUNT"
               + " HAVING Sales_PurchaseInvoiceMaster.BranchID=" + UserInfo.BRANCHID + " AND Sales_PurchaseInvoiceMaster.FacilityID=" + UserInfo.FacilityID.ToString()
               + " AND Sales_PurchaseInvoiceMaster.CANCEL = 0 AND Sales_PurchaseInvoiceMaster.DISCOUNTCREDITACCOUNT =" + AccountID;
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dt.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dt.Rows[i]["OppsiteAccountName"];
                            row["TempRecordType"] = dt.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات" : "Purchase Invoice");
                            row["ID"] = dt.Rows[i]["ID"];
                            row["Credit"] = dt.Rows[i]["Discount"];
                            row["Debit"] = dt.Rows[i]["Debit"];
                            row["Declaration"] = (dt.Rows[i]["Declaration"].ToString() != string.Empty ? dt.Rows[i]["Declaration"] : dt.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dt.Rows[i]["ID"]) + lang == "Eng" ? "(Discount)" : " (خصم) ";
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }

                }
                dt.Dispose();

                row = null;
            }
            catch { }


        }

        private void TransportOnPurchaseInvoice(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = ""; DataRow row;
                DataSet ds = new DataSet();

                //strSQL = "SELECT 'PurchaseInvoice' AS RecordType, Sales_PurchaseInvoiceMaster.InvoiceID AS ID, Sales_PurchaseInvoiceMaster.InvoiceDate AS TheDate,Sales_PurchaseInvoiceMaster.RegTime AS RegTime, 0 AS Credit , " + " Sales_PurchaseInvoiceMaster.Notes AS Declaration, Acc_Accounts.Arb_Name AS OppsiteAccountName,Sales_PurchaseInvoiceMaster.TransportDebitAmount AS Debit " + " FROM Sales_PurchaseInvoiceMaster LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceMaster.BranchID = Acc_Accounts.BranchID AND " + " Sales_PurchaseInvoiceMaster.TransportDebitAccount = Acc_Accounts.AccountID WHERE (Sales_PurchaseInvoiceMaster.BranchID = " + WT.GlobalBranchID + ") " + " AND (Sales_PurchaseInvoiceMaster.Cancel = 0) AND " + " (Sales_PurchaseInvoiceMaster.TransportDebitAccount = " + txtAccountID.TextWT + ") ";
                strSQL = "SELECT 'PurchaseInvoice' AS RecordType,Sales_PurchaseInvoiceMaster.INVOICEID AS ID,Sales_PurchaseInvoiceMaster.INVOICEDATE AS TheDate,"
                + " Sales_PurchaseInvoiceMaster.RegTime AS RegTime, 0 AS Credit,Sales_PurchaseInvoiceMaster.NOTES AS Declaration,Acc_Accounts.ArbName AS OppsiteAccountName,"
                + " Sales_PurchaseInvoiceMaster.TRANSPORTDEBITAMOUNT AS Debit FROM Sales_PurchaseInvoiceMaster LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceMaster.BranchID"
                + " = Acc_Accounts.BranchID AND Sales_PurchaseInvoiceMaster.TRANSPORTDEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Sales_PurchaseInvoiceMaster.FacilityID"
                + " = Acc_Accounts.FacilityID WHERE Sales_PurchaseInvoiceMaster.BranchID =" + UserInfo.BRANCHID + " AND Sales_PurchaseInvoiceMaster.FacilityID=" + UserInfo.FacilityID.ToString()
                + " AND Sales_PurchaseInvoiceMaster.CANCEL  = 0 AND Sales_PurchaseInvoiceMaster.TRANSPORTDEBITACCOUNT =" + AccountID;
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Sales_PurchaseInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Sales_PurchaseInvoiceMaster.InvoiceDate,Sales_PurchaseInvoiceMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dt = dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["TheDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dt.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dt.Rows[i]["OppsiteAccountName"];
                            row["TempRecordType"] = dt.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مشتريات" : "Purchase Invoice");
                            row["ID"] = dt.Rows[i]["ID"];
                            row["Credit"] = dt.Rows[i]["Credit"];
                            row["Debit"] = dt.Rows[i]["Debit"];
                            row["Declaration"] = (dt.Rows[i]["Declaration"].ToString() != string.Empty ? dt.Rows[i]["Declaration"] : dt.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dt.Rows[i]["ID"]) + lang == "Eng" ? " (Other Expenses)" : " (مصاريف شراء ونقل)";
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }

                }
                dt.Dispose();

                row = null;
            }
            catch { }

        }

        private void DicountOnPurchaseInvoiceReturn(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = ""; DataRow row;
                //strSQL = "SELECT 'PurchaseInvoiceReturn' AS RecordType, SUM(Sales_PurchaseInvoiceReturnDetails.Discount) + Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal AS Discount," + " Sales_PurchaseInvoiceReturnMaster.InvoiceID AS ID, Sales_PurchaseInvoiceReturnMaster.InvoiceDate AS TheDate, Sales_PurchaseInvoiceReturnMaster.Notes AS Declaration," + " Acc_Accounts.Arb_Name AS OppsiteAccountName FROM Sales_PurchaseInvoiceReturnMaster INNER JOIN Sales_PurchaseInvoiceReturnDetails ON Sales_PurchaseInvoiceReturnMaster.InvoiceID " + " = Sales_PurchaseInvoiceReturnDetails.InvoiceID AND Sales_PurchaseInvoiceReturnMaster.BranchID = Sales_PurchaseInvoiceReturnDetails.BranchID LEFT OUTER JOIN" + " Acc_Accounts ON Sales_PurchaseInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID AND Sales_PurchaseInvoiceReturnMaster.DebitAccount = Acc_Accounts.AccountID" + " GROUP BY Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal, Sales_PurchaseInvoiceReturnMaster.InvoiceID, Sales_PurchaseInvoiceReturnMaster.BranchID," + " Sales_PurchaseInvoiceReturnMaster.InvoiceDate, Sales_PurchaseInvoiceReturnMaster.Notes, " + " Sales_PurchaseInvoiceReturnMaster.Cancel, Acc_Accounts.Arb_Name, Sales_PurchaseInvoiceReturnMaster.DiscountDebitAccount" + " HAVING (Sales_PurchaseInvoiceReturnMaster.BranchID = " + WT.GlobalBranchID + ") AND " + " (Sales_PurchaseInvoiceReturnMaster.Cancel = 0) AND (Sales_PurchaseInvoiceReturnMaster.DiscountDebitAccount = " + txtAccountID.TextWT + ") ";
                strSQL = "SELECT 'PurchaseInvoiceReturn' AS RecordType,SUM(Sales_PurchaseInvoiceReturnDetails.DISCOUNT) + Sales_PurchaseInvoiceReturnMaster.DISCOUNTONTOTAL AS Discount,"
                + " Sales_PurchaseInvoiceReturnMaster.INVOICEID AS ID,Sales_PurchaseInvoiceReturnMaster.INVOICEDATE AS TheDate,Sales_PurchaseInvoiceReturnMaster.NOTES AS Declaration,"
                + " Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_PurchaseInvoiceReturnMaster INNER JOIN Sales_PurchaseInvoiceReturnDetails ON Sales_PurchaseInvoiceReturnMaster.INVOICEID"
                + " = Sales_PurchaseInvoiceReturnDetails.INVOICEID AND Sales_PurchaseInvoiceReturnMaster.BranchID = Sales_PurchaseInvoiceReturnDetails.BranchID AND Sales_PurchaseInvoiceReturnDetails.FacilityID"
                + " = Sales_PurchaseInvoiceReturnMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_PurchaseInvoiceReturnMaster.BranchID= Acc_Accounts.BranchID"
                + " AND Sales_PurchaseInvoiceReturnMaster.DEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Acc_Accounts.FacilityID= Sales_PurchaseInvoiceReturnMaster.FacilityID";

                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_PurchaseInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }



                strSQL = strSQL + " GROUP BY Sales_PurchaseInvoiceReturnMaster.INVOICEID,Sales_PurchaseInvoiceReturnMaster.INVOICEDATE,Sales_PurchaseInvoiceReturnMaster.NOTES,Acc_Accounts.ArbName,"
               + " Sales_PurchaseInvoiceReturnMaster.DISCOUNTONTOTAL,Sales_PurchaseInvoiceReturnMaster.BranchID,Sales_PurchaseInvoiceReturnMaster.CANCEL,Sales_PurchaseInvoiceReturnMaster.DISCOUNTDEBITACCOUNT"
               + " ,Sales_PurchaseInvoiceReturnMaster.FacilityID HAVING Sales_PurchaseInvoiceReturnMaster.BranchID =" + UserInfo.BRANCHID + " AND Sales_PurchaseInvoiceReturnMaster.CANCEL = 0 "
               + " AND Sales_PurchaseInvoiceReturnMaster.FacilityID =" + UserInfo.FacilityID.ToString() + " AND Sales_PurchaseInvoiceReturnMaster.DiscountDebitAccount =" + AccountID;
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}
                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_PurchaseInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                if (strSQL != null)
                {
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dt.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dt.Rows[i]["OppsiteAccountName"];
                            row["TempRecordType"] = dt.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مشتريات" : "Purchase Invoice Return");
                            row["ID"] = dt.Rows[i]["ID"];
                            //row["Credit"] = dt.Rows[i]["Discount"];
                            row["Debit"] = dt.Rows[i]["Discount"];
                            row["Declaration"] = (dt.Rows[i]["Declaration"].ToString() != string.Empty ? dt.Rows[i]["Declaration"] : dt.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dt.Rows[i]["ID"]) + lang == "Eng" ? "(Discount)" : " (خصم) ";
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }

                }
                dt.Dispose();

                row = null;
            }
            catch { }
        }

        private void DicountOnSalesInvoice(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = null; DataRow row;

                strSQL = "SELECT 'SalesInvoice'AS RecordType,SUM(Sales_SalesInvoiceDetails.DISCOUNT) + Sales_SalesInvoiceMaster.DISCOUNTONTOTAL AS Discount,Sales_SalesInvoiceMaster.INVOICEID  AS ID, Sales_SalesInvoiceMaster.INVOICEDATE AS TheDate,"
                + " Sales_SalesInvoiceMaster.NOTES AS Declaration,Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_SalesInvoiceMaster INNER JOIN Sales_SalesInvoiceDetails ON Sales_SalesInvoiceMaster.INVOICEID = Sales_SalesInvoiceDetails.INVOICEID"
                + " AND Sales_SalesInvoiceMaster.BranchID = Sales_SalesInvoiceDetails.BranchID AND Sales_SalesInvoiceDetails.FacilityID = Sales_SalesInvoiceMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceMaster.BranchID = Acc_Accounts.BranchID"
                + " AND Sales_SalesInvoiceMaster.DEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Sales_SalesInvoiceMaster.FacilityID = Acc_Accounts.FacilityID";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_SalesInvoiceMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }



                strSQL = strSQL + "   GROUP BY Sales_SalesInvoiceMaster.INVOICEID,Sales_SalesInvoiceMaster.INVOICEDATE,"
               + " Sales_SalesInvoiceMaster.NOTES,Acc_Accounts.ArbName,Sales_SalesInvoiceMaster.DISCOUNTONTOTAL,Sales_SalesInvoiceMaster.BranchID,Sales_SalesInvoiceMaster.CANCEL,Sales_SalesInvoiceMaster.DISCOUNTDEBITACCOUNT,"
               + " Sales_SalesInvoiceMaster.FacilityID HAVING Sales_SalesInvoiceMaster.BranchID =" + UserInfo.BRANCHID + " AND Sales_SalesInvoiceMaster.CANCEL = 0 AND  Sales_SalesInvoiceMaster.FacilityID =" + UserInfo.FacilityID.ToString()
               + " AND Sales_SalesInvoiceMaster.DISCOUNTDEBITACCOUNT =" + AccountID;

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate;
                //}

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dt.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dt.Rows[i]["OppsiteAccountName"];
                            row["TempRecordType"] = dt.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "فاتورة مبيعات" : "Sales Invoice");
                            row["ID"] = dt.Rows[i]["ID"];

                            string str = UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? " (خصم)" : "(Discount)";
                            string str2 = lang == "Eng" ? "No." : " رقم ";

                            row["Declaration"] = (dt.Rows[i]["Declaration"].ToString() != string.Empty ? dt.Rows[i]["Declaration"] + str : dt.Rows[i]["RecordType"] + str2 + dt.Rows[i]["ID"] + str);

                            row["Debit"] = dt.Rows[i]["Discount"];
                            row["Credit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);

                        }
                    }
                }
                dt.Dispose();

                row = null;
            }
            catch { }
        }

        private void DicountOnSalesInvoiceReturn(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = ""; DataRow row;


                //strSQL = "SELECT 'SalesInvoiceReturn' AS RecordType, SUM(Sales_SalesInvoiceReturnDetails.Discount) + Sales_SalesInvoiceReturnMaster.DiscountOnTotal AS Discount, " + " Sales_SalesInvoiceReturnMaster.InvoiceID AS ID, Sales_SalesInvoiceReturnMaster.InvoiceDate AS TheDate, Sales_SalesInvoiceReturnMaster.Notes AS Declaration," + " Acc_Accounts.Arb_Name AS OppsiteAccountName FROM Sales_SalesInvoiceReturnMaster INNER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID" + " = Sales_SalesInvoiceReturnDetails.InvoiceID AND Sales_SalesInvoiceReturnMaster.BranchID = Sales_SalesInvoiceReturnDetails.BranchID LEFT OUTER JOIN " + " Acc_Accounts ON Sales_SalesInvoiceReturnMaster.BranchID = Acc_Accounts.BranchID AND Sales_SalesInvoiceReturnMaster.DebitAccount = Acc_Accounts.AccountID" + " GROUP BY Sales_SalesInvoiceReturnMaster.DiscountOnTotal, Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID," + " Sales_SalesInvoiceReturnMaster.InvoiceDate, Sales_SalesInvoiceReturnMaster.Notes, Sales_SalesInvoiceReturnMaster.Cancel," + " Acc_Accounts.Arb_Name, Sales_SalesInvoiceReturnMaster.DiscountCreditAccount HAVING (Sales_SalesInvoiceReturnMaster.BranchID = " + WT.GlobalBranchID + ") " + " AND (Sales_SalesInvoiceReturnMaster.Cancel = 0) " + " And (Sales_SalesInvoiceReturnMaster.DiscountCreditAccount = " + txtAccountID.TextWT + ") ";
                strSQL = "SELECT 'SalesInvoiceReturn' AS RecordType,SUM(Sales_SalesInvoiceReturnDetails.DISCOUNT) + Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL AS Discount,"
                + " Sales_SalesInvoiceReturnMaster.INVOICEID AS ID,Sales_SalesInvoiceReturnMaster.INVOICEDATE AS TheDate,Sales_SalesInvoiceReturnMaster.NOTES AS Declaration,"
                + " Acc_Accounts.ArbName AS OppsiteAccountName FROM Sales_SalesInvoiceReturnMaster INNER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.INVOICEID"
                + " = Sales_SalesInvoiceReturnDetails.INVOICEID AND Sales_SalesInvoiceReturnMaster.BranchID= Sales_SalesInvoiceReturnDetails.BranchID AND"
                + " Sales_SalesInvoiceReturnDetails.FacilityID = Sales_SalesInvoiceReturnMaster.FacilityID LEFT OUTER JOIN Acc_Accounts ON Sales_SalesInvoiceReturnMaster.BranchID"
                + " = Acc_Accounts.BranchID AND Sales_SalesInvoiceReturnMaster.DEBITACCOUNT = Acc_Accounts.ACCOUNTID AND Sales_SalesInvoiceReturnMaster.FacilityID = Acc_Accounts.FacilityID";
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " where  Sales_SalesInvoiceReturnMaster.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }




                strSQL = strSQL + " GROUP BY Sales_SalesInvoiceReturnMaster.INVOICEID, Sales_SalesInvoiceReturnMaster.INVOICEDATE, Sales_SalesInvoiceReturnMaster.NOTES, "
                + " Sales_SalesInvoiceReturnMaster.FacilityID, Acc_Accounts.ArbName, Sales_SalesInvoiceReturnMaster.DISCOUNTONTOTAL, Sales_SalesInvoiceReturnMaster.BranchID,"
                + " Sales_SalesInvoiceReturnMaster.CANCEL, Sales_SalesInvoiceReturnMaster.DISCOUNTCREDITACCOUNT HAVING Sales_SalesInvoiceReturnMaster.BranchID =" + UserInfo.BRANCHID
                + " AND Sales_SalesInvoiceReturnMaster.FacilityID =" + UserInfo.FacilityID.ToString() + " AND Sales_SalesInvoiceReturnMaster.CANCEL = 0 AND Sales_SalesInvoiceReturnMaster.DISCOUNTCREDITACCOUNT =" + AccountID;
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceReturnMaster.InvoiceDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Sales_SalesInvoiceReturnMaster.InvoiceDate <=" + ToDate;
                //}

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dt.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dt.Rows[i]["OppsiteAccountName"];
                            row["TempRecordType"] = dt.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مردود فاتورة مبيعات" : "Sales Invoice Return");
                            row["ID"] = dt.Rows[i]["ID"];
                            row["Credit"] = dt.Rows[i]["Discount"];
                            //row["Debit"] = dt.Rows[i]["Discount"];
                            row["Declaration"] = (dt.Rows[i]["Declaration"].ToString() != string.Empty ? dt.Rows[i]["Declaration"] : dt.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dt.Rows[i]["ID"]) + lang == "Eng" ? "(Discount)" : " (خصم) ";
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }

                }
                dt.Dispose();

                row = null;
            }
            catch { }
        }

        private void ReceiptVoucher(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable();
                DataTable dtDiscount = new DataTable();
                string strSQL = "";
                decimal NetBalance; DataRow row;

                //إضافة هذه الجملة الجديدة لاحتساب حساب الخصم المكتسب به ضمن سند القبض ، حيث يكون مدين
                strSQL = "SELECT Acc_ReceiptVoucherDetails.DECLARATION,Acc_ReceiptVoucherMaster.RECEIPTVOUCHERDATE AS TheDate,'ReceiptVoucher'  AS RecordType, Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID AS ID,"
                + " Acc_ReceiptVoucherMaster.DISCOUNTACCOUNTID,Acc_ReceiptVoucherMaster.RegTime, ' '  AS OppsiteAccountName,SUM(Acc_ReceiptVoucherDetails.DISCOUNT) AS SumDiscount, Acc_ReceiptVoucherMaster.FacilityID"
                + " FROM Acc_ReceiptVoucherMaster RIGHT OUTER JOIN Acc_ReceiptVoucherDetails ON Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID = Acc_ReceiptVoucherDetails.RECEIPTVOUCHERID AND Acc_ReceiptVoucherMaster.BranchID"
                + " = Acc_ReceiptVoucherDetails.BranchID AND Acc_ReceiptVoucherMaster.FacilityID = Acc_ReceiptVoucherDetails.FacilityID WHERE Acc_ReceiptVoucherMaster.CANCEL = 0 AND Acc_ReceiptVoucherMaster.BranchID = " + UserInfo.BRANCHID
                + " AND Acc_ReceiptVoucherMaster.DISCOUNTACCOUNTID =" + AccountID + " AND Acc_ReceiptVoucherMaster.FacilityID=" + UserInfo.FacilityID.ToString();
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_ReceiptVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }


                strSQL = strSQL + " GROUP BY Acc_ReceiptVoucherDetails.DECLARATION,"
                + " Acc_ReceiptVoucherMaster.RECEIPTVOUCHERDATE, Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID,Acc_ReceiptVoucherMaster.DISCOUNTACCOUNTID, Acc_ReceiptVoucherMaster.RegTime,"
                + " Acc_ReceiptVoucherMaster.FacilityID HAVING SUM(Acc_ReceiptVoucherDetails.DISCOUNT) > 0";

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_ReceiptVoucherMaster.ReceiptVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_ReceiptVoucherMaster.ReceiptVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Acc_ReceiptVoucherMaster.ReceiptVoucherDate,Acc_ReceiptVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtDiscount = Lip.SelectRecord(strSQL);
                    if (dtDiscount.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDiscount.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtDiscount.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDiscount.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = (lang == "Eng" ? "Mentioned" : "مذكورين");
                            row["RegTime"] = dtDiscount.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDiscount.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند قبض" : "Receipt Voucher");
                            row["ID"] = dtDiscount.Rows[i]["ID"];
                            row["Declaration"] = (dtDiscount.Rows[i]["Declaration"].ToString() != string.Empty ? dtDiscount.Rows[i]["Declaration"] : dtDiscount.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDiscount.Rows[i]["ID"]);
                            row["Debit"] = Comon.ConvertToDecimalPrice(dtDiscount.Rows[i]["SumDiscount"]);
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);

                        }
                    }
                }



                /////////////////////////////////////
                strSQL = "SELECT Acc_ReceiptVoucherDetails.DECLARATION,Acc_ReceiptVoucherMaster.RECEIPTVOUCHERDATE AS TheDate,'ReceiptVoucher'  AS RecordType, Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID AS ID,"
                + " Acc_ReceiptVoucherMaster.DISCOUNTACCOUNTID,Acc_ReceiptVoucherMaster.RegTime, ' '  AS OppsiteAccountName,SUM(Acc_ReceiptVoucherDetails.DISCOUNT) AS SumDiscount,SUM(Acc_ReceiptVoucherDetails.CreditAmount) AS SumCredit, Acc_ReceiptVoucherMaster.FacilityID"
                + " FROM Acc_ReceiptVoucherMaster RIGHT OUTER JOIN Acc_ReceiptVoucherDetails ON Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID = Acc_ReceiptVoucherDetails.RECEIPTVOUCHERID AND Acc_ReceiptVoucherMaster.BranchID"
                + " = Acc_ReceiptVoucherDetails.BranchID AND Acc_ReceiptVoucherMaster.FacilityID = Acc_ReceiptVoucherDetails.FacilityID WHERE Acc_ReceiptVoucherMaster.CANCEL = 0 AND Acc_ReceiptVoucherMaster.BranchID = " + UserInfo.BRANCHID
                + " AND Acc_ReceiptVoucherMaster.DEBITACCOUNTID =" + AccountID + " AND Acc_ReceiptVoucherMaster.FacilityID=" + UserInfo.FacilityID.ToString();
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_ReceiptVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }


                strSQL = strSQL + " GROUP BY Acc_ReceiptVoucherDetails.DECLARATION,"
                + " Acc_ReceiptVoucherMaster.RECEIPTVOUCHERDATE, Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID,Acc_ReceiptVoucherMaster.DISCOUNTACCOUNTID, Acc_ReceiptVoucherMaster.RegTime,"
                + " Acc_ReceiptVoucherMaster.FacilityID";

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_ReceiptVoucherMaster.ReceiptVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_ReceiptVoucherMaster.ReceiptVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Acc_ReceiptVoucherMaster.ReceiptVoucherDate,Acc_ReceiptVoucherMaster.RegTime";
                // مبارك هنا يتم احتساب حساب المدين الافتراضي وهو الصندوق
                //strSQL = "SELECT Acc_ReceiptVoucherMaster.DEBITAMOUNT - Acc_ReceiptVoucherMaster.DISCOUNTAMOUNT AS NetBalance, Acc_ReceiptVoucherMaster.NOTES  AS Declaration, Acc_ReceiptVoucherMaster.RECEIPTVOUCHERDATE  AS TheDate,"
                //+ " 'ReceiptVoucher'  AS RecordType,Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID AS ID,Acc_ReceiptVoucherMaster.DISCOUNTACCOUNTID, Acc_ReceiptVoucherMaster.RegTime,Acc_ReceiptVoucherMaster.DEBITACCOUNTID,"
                //+ " ' ' AS OppsiteAccountName,Acc_ReceiptVoucherMaster.FacilityID FROM Acc_ReceiptVoucherMaster WHERE Acc_ReceiptVoucherMaster.DEBITACCOUNTID =" + AccountID + " AND Acc_ReceiptVoucherMaster.CANCEL= 0"
                //+ " AND Acc_ReceiptVoucherMaster.BranchID =" + UserInfo.BRANCHID + " AND Acc_ReceiptVoucherMaster.FacilityID= " + UserInfo.FacilityID.ToString();
                ////if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                ////{

                ////    strSQL = strSQL + " AND  Acc_ReceiptVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                ////}

                ////if (FromDate != 0)
                ////{
                ////    strSQL = strSQL + " AND ReceiptVoucherDate >=" + FromDate;
                ////}

                ////if (ToDate != 0)
                ////{
                ////    strSQL = strSQL + " AND ReceiptVoucherDate <=" + ToDate;
                ////}

                //strSQL = strSQL + " ORDER BY ReceiptVoucherDate,RegTime";

                //  strSQL = Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, lang);
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                if (strSQL != null)
                {
                    dtDebit = Lip.SelectRecord(strSQL);
                    if (dtDebit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();

                            row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند قبض" : "Receipt Voucher");
                            row["ID"] = dtDebit.Rows[i]["ID"];
                            row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);
                            row["OppsiteAccountName"] = (lang == "Eng" ? "Mentioned" : "مذكورين");
                            NetBalance = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["SumCredit"]) - Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["SumDiscount"]);
                            row["Debit"] = NetBalance;
                            row["Credit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);

                        }
                    }
                }


                //هنا يتم احتساب حساب الدائن
                strSQL = "SELECT Acc_ReceiptVoucherDetails.DECLARATION,Acc_ReceiptVoucherMaster.RECEIPTVOUCHERDATE AS TheDate, 'ReceiptVoucher' AS RecordType, Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID AS ID,"
                + " Acc_ReceiptVoucherMaster.RegTime,Acc_ReceiptVoucherDetails.CREDITAMOUNT AS SumCreditAmount,Acc_ReceiptVoucherDetails.ACCOUNTID,Acc_Accounts.ArbName AS OppsiteAccountName"
                + " FROM Acc_ReceiptVoucherMaster INNER JOIN Acc_Accounts ON Acc_ReceiptVoucherMaster.BranchID  = Acc_Accounts.BranchID"
                + " AND Acc_ReceiptVoucherMaster.DEBITACCOUNTID = Acc_Accounts.ACCOUNTID AND Acc_ReceiptVoucherMaster.FacilityID  = Acc_Accounts.FacilityID RIGHT OUTER JOIN Acc_ReceiptVoucherDetails"
                + " ON Acc_ReceiptVoucherMaster.RECEIPTVOUCHERID = Acc_ReceiptVoucherDetails.RECEIPTVOUCHERID AND Acc_ReceiptVoucherMaster.BranchID = Acc_ReceiptVoucherDetails.BranchID"
                + " AND Acc_ReceiptVoucherMaster.FacilityID = Acc_ReceiptVoucherDetails.FacilityID WHERE Acc_ReceiptVoucherDetails.ACCOUNTID =" + AccountID
                + " AND Acc_ReceiptVoucherMaster.CANCEL = 0 AND Acc_ReceiptVoucherMaster.BranchID =" + UserInfo.BRANCHID + " AND Acc_ReceiptVoucherMaster.FacilityID=" + UserInfo.FacilityID.ToString();

                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_ReceiptVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_ReceiptVoucherMaster.ReceiptVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_ReceiptVoucherMaster.ReceiptVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " GROUP BY Acc_ReceiptVoucherDetails.Declaration, Acc_ReceiptVoucherMaster.ReceiptVoucherDate, Acc_ReceiptVoucherMaster.ReceiptVoucherID, "
                + " Acc_ReceiptVoucherMaster.RegTime, Acc_ReceiptVoucherDetails.CreditAmount, Acc_ReceiptVoucherDetails.AccountID, Acc_Accounts.ArbName";

                strSQL = strSQL + " ORDER BY Acc_ReceiptVoucherMaster.ReceiptVoucherDate,Acc_ReceiptVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);

                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند قبض" : "Receipt Voucher");
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            row["Credit"] = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["SumCreditAmount"]);
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);

                        }
                    }
                }
                dtDiscount.Dispose();
                dtCredit.Dispose();
                dtDebit.Dispose();

                row = null;
            }
            catch { }

        }

        private void SpendVoucher(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable();
                DataTable dtDiscount = new DataTable();
                string strSQL = ""; DataRow row;
                decimal NetBalance = 0;

                //إضافة هذه الجملة الجديدة لاحتساب حساب الخصم المكتسب به ضمن سند الصرف ، حيث يكون دائن
                //strSQL = "SELECT Acc_SpendVoucherDetails.Declaration, Acc_SpendVoucherMaster.SpendVoucherDate AS TheDate, " + " 'SpendVoucher' AS RecordType, Acc_SpendVoucherMaster.SpendVoucherID AS ID, Acc_SpendVoucherMaster.DiscountAccountID, " + " Acc_SpendVoucherMaster.RegTime, ' ' AS OppsiteAccountName, SUM(Acc_SpendVoucherDetails.Discount) AS SumDiscount" + " FROM Acc_SpendVoucherMaster RIGHT OUTER JOIN" + " Acc_SpendVoucherDetails ON Acc_SpendVoucherMaster.SpendVoucherID = Acc_SpendVoucherDetails.SpendVoucherID AND " + " Acc_SpendVoucherMaster.BranchID = Acc_SpendVoucherDetails.BranchID" + " WHERE Acc_SpendVoucherMaster.Cancel = 0 AND Acc_SpendVoucherMaster.BranchID = " + WT.GlobalBranchID + " AND " + " Acc_SpendVoucherMaster.DiscountAccountID = " + txtAccountID.TextWT + " GROUP BY Acc_SpendVoucherDetails.Declaration, Acc_SpendVoucherMaster.SpendVoucherDate, Acc_SpendVoucherMaster.SpendVoucherID, " + " Acc_SpendVoucherMaster.DiscountAccountID, Acc_SpendVoucherMaster.RegTime" + " HAVING (SUM(Acc_SpendVoucherDetails.Discount) > 0) ";
                strSQL = "SELECT ACC_SPENDVOUCHERDETAILS.DECLARATION, ACC_SPENDVOUCHERMASTER.SPENDVOUCHERDATE AS TheDate, 'SpendVoucher' AS RecordType,ACC_SPENDVOUCHERMASTER.SPENDVOUCHERID AS ID,"
                + " ACC_SPENDVOUCHERMASTER.DISCOUNTACCOUNTID,ACC_SPENDVOUCHERMASTER.RegTime, ' ' AS OppsiteAccountName,SUM(ACC_SPENDVOUCHERDETAILS.DISCOUNT) AS SumDiscount"
                + " FROM ACC_SPENDVOUCHERMASTER RIGHT OUTER JOIN ACC_SPENDVOUCHERDETAILS ON ACC_SPENDVOUCHERMASTER.SPENDVOUCHERID = ACC_SPENDVOUCHERDETAILS.SPENDVOUCHERID"
                + " AND ACC_SPENDVOUCHERMASTER.BranchID = ACC_SPENDVOUCHERDETAILS.BranchID AND ACC_SPENDVOUCHERDETAILS.FacilityID= ACC_SPENDVOUCHERMASTER.FacilityID"
                + " WHERE ACC_SPENDVOUCHERMASTER.CANCEL = 0 AND ACC_SPENDVOUCHERMASTER.BranchID =" + UserInfo.BRANCHID + " AND ACC_SPENDVOUCHERMASTER.FacilityID = " + UserInfo.FacilityID.ToString()
                + " AND ACC_SPENDVOUCHERMASTER.DISCOUNTACCOUNTID =" + AccountID;
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  ACC_SPENDVOUCHERDETAILS.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }




                strSQL = strSQL + " GROUP BY ACC_SPENDVOUCHERDETAILS.DECLARATION, ACC_SPENDVOUCHERMASTER.SPENDVOUCHERDATE,"
                + " ACC_SPENDVOUCHERMASTER.SPENDVOUCHERID, ACC_SPENDVOUCHERMASTER.DISCOUNTACCOUNTID, ACC_SPENDVOUCHERMASTER.RegTime HAVING SUM(ACC_SPENDVOUCHERDETAILS.DISCOUNT) > 0";
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND ACC_SPENDVOUCHERMASTER.SpendVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND ACC_SPENDVOUCHERMASTER.SpendVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY ACC_SPENDVOUCHERMASTER.SpendVoucherDate,ACC_SPENDVOUCHERMASTER.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtDiscount = Lip.SelectRecord(strSQL);
                if (dtDiscount.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDiscount.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtDiscount.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtDiscount.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مذكورين" : "Mentioned");
                        row["RegTime"] = dtDiscount.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtDiscount.Rows[i]["RecordType"];
                        row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند صرف" : "Spend Voucher");
                        row["ID"] = dtDiscount.Rows[i]["ID"];
                        row["Declaration"] = (dtDiscount.Rows[i]["Declaration"].ToString() != string.Empty ? dtDiscount.Rows[i]["Declaration"] : dtDiscount.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDiscount.Rows[i]["ID"]);
                        //Net = Comon.ConvertToDecimalPrice(dtDiscount.Rows[i]["SumDiscount"]);
                        row["Credit"] = Comon.ConvertToDecimalPrice(dtDiscount.Rows[i]["SumDiscount"]);// Net ;
                        //row["Debit"] = 0;
                        row["Balance"] = 0;
                        _sampleData.Rows.Add(row);
                    }
                }


//////////////////////////////////////////////////////////////////////////////////////////////////////////////

                strSQL = "SELECT ACC_SPENDVOUCHERDETAILS.DECLARATION, ACC_SPENDVOUCHERMASTER.SPENDVOUCHERDATE AS TheDate, 'SpendVoucher' AS RecordType,ACC_SPENDVOUCHERMASTER.SPENDVOUCHERID AS ID,"
             + " ACC_SPENDVOUCHERMASTER.DISCOUNTACCOUNTID,ACC_SPENDVOUCHERMASTER.RegTime, ' ' AS OppsiteAccountName,SUM(ACC_SPENDVOUCHERDETAILS.DISCOUNT) AS SumDiscount,SUM(ACC_SPENDVOUCHERDETAILS.DebitAmount) AS SumDebit"
             + " FROM ACC_SPENDVOUCHERMASTER RIGHT OUTER JOIN ACC_SPENDVOUCHERDETAILS ON ACC_SPENDVOUCHERMASTER.SPENDVOUCHERID = ACC_SPENDVOUCHERDETAILS.SPENDVOUCHERID"
             + " AND ACC_SPENDVOUCHERMASTER.BranchID = ACC_SPENDVOUCHERDETAILS.BranchID AND ACC_SPENDVOUCHERDETAILS.FacilityID= ACC_SPENDVOUCHERMASTER.FacilityID"
             + " WHERE ACC_SPENDVOUCHERMASTER.CANCEL = 0 AND ACC_SPENDVOUCHERMASTER.BranchID =" + UserInfo.BRANCHID + " AND ACC_SPENDVOUCHERMASTER.FacilityID = " + UserInfo.FacilityID.ToString()
             + " AND ACC_SPENDVOUCHERMASTER.CreditAccountID =" + AccountID;
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  ACC_SPENDVOUCHERDETAILS.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }




                strSQL = strSQL + " GROUP BY ACC_SPENDVOUCHERDETAILS.DECLARATION, ACC_SPENDVOUCHERMASTER.SPENDVOUCHERDATE,"
                + " ACC_SPENDVOUCHERMASTER.SPENDVOUCHERID, ACC_SPENDVOUCHERMASTER.DISCOUNTACCOUNTID, ACC_SPENDVOUCHERMASTER.RegTime";
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND ACC_SPENDVOUCHERMASTER.SpendVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND ACC_SPENDVOUCHERMASTER.SpendVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY ACC_SPENDVOUCHERMASTER.SpendVoucherDate,ACC_SPENDVOUCHERMASTER.RegTime";

                //هنا يتم احتساب حساب الدائن الافتراضي وهو الصندوق مبارك  
                ////strSQL = "SELECT  CreditAmount - DiscountAmount AS NetBalance,Notes AS Declaration, SpendVoucherDate AS TheDate, 'SpendVoucher' AS RecordType, SpendVoucherID AS ID, DiscountAccountID, RegTime , " + " CreditAccountID, ' ' AS OppsiteAccountName FROM Acc_SpendVoucherMaster  WHERE (Cancel = 0) AND (BranchID = " + WT.GlobalBranchID + ")" + " AND (CreditAccountID = " + txtAccountID.TextWT + ") ";
                //strSQL = "SELECT  CreditAmount - DiscountAmount AS NetBalance,Notes AS Declaration, SpendVoucherDate AS TheDate, 'SpendVoucher' AS RecordType, SpendVoucherID AS ID, DiscountAccountID, RegTime , CreditAccountID, ' ' AS OppsiteAccountName"
                //+ " FROM Acc_SpendVoucherMaster  WHERE Cancel = 0 AND BranchID =" + UserInfo.BRANCHID + " AND FacilityID =" + UserInfo.FacilityID.ToString() + "  AND CreditAccountID = " + AccountID;
                ////if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                ////{

                //    strSQL = strSQL + " AND  ACC_SPENDVOUCHERDETAILS.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                //}
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND SpendVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND SpendVoucherDate <=" + ToDate;
                //}

              //  strSQL = strSQL + " ORDER BY SpendVoucherDate,RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtCredit = Lip.SelectRecord(strSQL);
                if (dtCredit.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مذكورين" : "Mentioned");
                        row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                        row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند صرف" : "Spend Voucher");
                        row["ID"] = dtCredit.Rows[i]["ID"];
                        row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                        NetBalance = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["SumDebit"]) - Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["SumDiscount"]);
                        row["Credit"] = NetBalance;
                        row["Debit"] = 0;
                        row["Balance"] = 0;
                        _sampleData.Rows.Add(row);
                    }
                }

                //هنا يتم احتساب حساب المدين
                //strSQL = "SELECT Acc_SpendVoucherDetails.Declaration, Acc_SpendVoucherMaster.SpendVoucherDate AS TheDate, " + " 'SpendVoucher' AS RecordType, Acc_SpendVoucherMaster.SpendVoucherID AS ID, Acc_SpendVoucherMaster.RegTime, " + " Acc_SpendVoucherDetails.DebitAmount AS SumDebitAmount, Acc_SpendVoucherDetails.AccountID, Acc_Accounts.ArbName As OppsiteAccountName" + " FROM Acc_SpendVoucherMaster INNER JOIN" + " Acc_Accounts ON Acc_SpendVoucherMaster.BranchID = Acc_Accounts.BranchID AND " + " Acc_SpendVoucherMaster.CreditAccountID = Acc_Accounts.AccountID RIGHT OUTER JOIN" + " Acc_SpendVoucherDetails ON Acc_SpendVoucherMaster.SpendVoucherID = Acc_SpendVoucherDetails.SpendVoucherID AND " + " Acc_SpendVoucherMaster.BranchID = Acc_SpendVoucherDetails.BranchID" + " WHERE Acc_SpendVoucherMaster.Cancel = 0 AND Acc_SpendVoucherMaster.BranchID = " + WT.GlobalBranchID + " AND " + " Acc_SpendVoucherDetails.AccountID = " + txtAccountID.TextWT;
                strSQL = "SELECT Acc_SpendVoucherDetails.DECLARATION,Acc_SpendVoucherMaster.SPENDVOUCHERDATE AS TheDate, 'SpendVoucher' AS RecordType,Acc_SpendVoucherMaster.SPENDVOUCHERID AS ID,"
                + " Acc_SpendVoucherMaster.RegTime,Acc_SpendVoucherDetails.DEBITAMOUNT AS SumDebitAmount,Acc_SpendVoucherDetails.ACCOUNTID,Acc_Accounts.ArbName AS OppsiteAccountName"
                + " FROM Acc_SpendVoucherMaster INNER JOIN Acc_Accounts ON Acc_SpendVoucherMaster.BranchID = Acc_Accounts.BranchID AND Acc_SpendVoucherMaster.CREDITACCOUNTID ="
                + " Acc_Accounts.ACCOUNTID AND Acc_Accounts.FacilityID= Acc_SpendVoucherMaster.FacilityID RIGHT OUTER JOIN Acc_SpendVoucherDetails ON Acc_SpendVoucherMaster.SPENDVOUCHERID"
                + " = Acc_SpendVoucherDetails.SPENDVOUCHERID AND Acc_SpendVoucherMaster.BranchID= Acc_SpendVoucherDetails.BranchID AND Acc_SpendVoucherDetails.FacilityID="
                + " Acc_SpendVoucherMaster.FacilityID WHERE Acc_SpendVoucherDetails.ACCOUNTID  =" + AccountID + " AND Acc_SpendVoucherMaster.CANCEL  = 0"
                + " AND Acc_SpendVoucherMaster.BranchID =" + UserInfo.BRANCHID + " AND Acc_SpendVoucherMaster.FacilityID =" + UserInfo.FacilityID.ToString();


                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_SpendVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_SpendVoucherMaster.SpendVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_SpendVoucherMaster.SpendVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + "  GROUP BY Acc_SpendVoucherDetails.Declaration, Acc_SpendVoucherMaster.SpendVoucherDate, Acc_SpendVoucherMaster.SpendVoucherID, Acc_SpendVoucherMaster.RegTime,"
                + " Acc_SpendVoucherDetails.DebitAmount,Acc_SpendVoucherDetails.AccountID, Acc_Accounts.ArbName ORDER BY Acc_SpendVoucherMaster.SpendVoucherDate,Acc_SpendVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtDebit = Lip.SelectRecord(strSQL);
                if (dtDebit.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = dtDebit.Rows[i]["OppsiteAccountName"];
                        row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                        row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند صرف" : "Spend Voucher");
                        row["ID"] = dtDebit.Rows[i]["ID"];
                        row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);
                        row["Debit"] = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["SumDebitAmount"]);
                        row["Balance"] = 0;
                        _sampleData.Rows.Add(row);
                    }
                }
                dtCredit.Dispose();
                dtDebit.Dispose();
                dtDiscount.Dispose();
                row = null;
            }
            catch { }

        }

        private void CheckReceiptVoucher(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable();
                DataTable dtDiscount = new DataTable();
                string strSQL = "";
                decimal NetBalance; DataRow row;

                //إضافة هذه الجملة الجديدة لاحتساب حساب الخصم المسموح به ضمن سند قبض الشيكات ، حيث يكون مدين
                //strSQL = "SELECT Acc_CheckReceiptVoucherDetails.Declaration, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate AS TheDate, " + " 'CheckReceiptVoucher' AS RecordType, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID AS ID, Acc_CheckReceiptVoucherMaster.DiscountAccountID, " + " Acc_CheckReceiptVoucherMaster.RegTime, ' ' AS OppsiteAccountName, SUM(Acc_CheckReceiptVoucherDetails.Discount) AS SumDiscount" + " FROM Acc_CheckReceiptVoucherMaster RIGHT OUTER JOIN" + " Acc_CheckReceiptVoucherDetails ON Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID = Acc_CheckReceiptVoucherDetails.CheckReceiptVoucherID AND " + " Acc_CheckReceiptVoucherMaster.BranchID = Acc_CheckReceiptVoucherDetails.BranchID" + " WHERE Acc_CheckReceiptVoucherMaster.Cancel = 0 AND Acc_CheckReceiptVoucherMaster.BranchID = " + WT.GlobalBranchID + " AND " + " Acc_CheckReceiptVoucherMaster.DiscountAccountID = " + txtAccountID.TextWT;
                strSQL = "SELECT Acc_CheckReceiptVoucherDetails.Declaration, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate AS TheDate, 'CheckReceiptVoucher' AS RecordType,"
                + " Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID AS ID, Acc_CheckReceiptVoucherMaster.DiscountAccountID, Acc_CheckReceiptVoucherMaster.RegTime,"
                + " ' ' AS OppsiteAccountName, SUM(Acc_CheckReceiptVoucherDetails.Discount) AS SumDiscount FROM Acc_CheckReceiptVoucherMaster RIGHT OUTER JOIN "
                + " Acc_CheckReceiptVoucherDetails ON Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID = Acc_CheckReceiptVoucherDetails.CheckReceiptVoucherID"
                + " AND Acc_CheckReceiptVoucherMaster.BranchID = Acc_CheckReceiptVoucherDetails.BranchID AND Acc_CheckReceiptVoucherMaster.FacilityID ="
                + " Acc_CheckReceiptVoucherDetails.FacilityID WHERE Acc_CheckReceiptVoucherMaster.Cancel = 0 AND Acc_CheckReceiptVoucherMaster.BranchID =" + UserInfo.BRANCHID
                + " AND Acc_CheckReceiptVoucherMaster.FacilityID =" + UserInfo.FacilityID.ToString() + " AND Acc_CheckReceiptVoucherMaster.DiscountAccountID =" + AccountID;


                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_CheckReceiptVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " GROUP BY Acc_CheckReceiptVoucherDetails.Declaration, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID,"
                + " Acc_CheckReceiptVoucherMaster.DiscountAccountID, Acc_CheckReceiptVoucherMaster.RegTime HAVING (SUM(Acc_CheckReceiptVoucherDetails.Discount) > 0) "
                + " ORDER BY Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate,Acc_CheckReceiptVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtDiscount = Lip.SelectRecord(strSQL);
                    if (dtDiscount.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDiscount.Rows.Count; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtDiscount.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDiscount.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = (lang == "Eng" ? "Mentioned" : "مذكورين");
                            row["RegTime"] = dtDiscount.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDiscount.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند قبض شيك" : "Check Receipt Voucher");
                            row["ID"] = dtDiscount.Rows[i]["ID"];
                            row["Declaration"] = (dtDiscount.Rows[i]["Declaration"].ToString() != string.Empty ? dtDiscount.Rows[i]["Declaration"] : dtDiscount.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDiscount.Rows[i]["ID"]);
                            row["Debit"] = Comon.ConvertToDecimalPrice(dtDiscount.Rows[i]["SumDiscount"]);
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////

                strSQL = "SELECT Acc_CheckReceiptVoucherDetails.Declaration, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate AS TheDate, 'CheckReceiptVoucher' AS RecordType,"
              + " Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID AS ID, Acc_CheckReceiptVoucherMaster.DiscountAccountID, Acc_CheckReceiptVoucherMaster.RegTime,"
              + " ' ' AS OppsiteAccountName, SUM(Acc_CheckReceiptVoucherDetails.Discount) AS SumDiscount ,SUM(Acc_CheckReceiptVoucherDetails.CreditAmount) AS SumCredit FROM Acc_CheckReceiptVoucherMaster RIGHT OUTER JOIN "
              + " Acc_CheckReceiptVoucherDetails ON Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID = Acc_CheckReceiptVoucherDetails.CheckReceiptVoucherID"
              + " AND Acc_CheckReceiptVoucherMaster.BranchID = Acc_CheckReceiptVoucherDetails.BranchID AND Acc_CheckReceiptVoucherMaster.FacilityID ="
              + " Acc_CheckReceiptVoucherDetails.FacilityID WHERE Acc_CheckReceiptVoucherMaster.Cancel = 0 AND Acc_CheckReceiptVoucherMaster.BranchID =" + UserInfo.BRANCHID
              + " AND Acc_CheckReceiptVoucherMaster.FacilityID =" + UserInfo.FacilityID.ToString() + " AND Acc_CheckReceiptVoucherMaster.DebitAccountID =" + AccountID;


                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_CheckReceiptVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + "  GROUP BY Acc_CheckReceiptVoucherDetails.Declaration, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID,"
                + " Acc_CheckReceiptVoucherMaster.DiscountAccountID, Acc_CheckReceiptVoucherMaster.RegTime  "
                + " ORDER BY Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate,Acc_CheckReceiptVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


                //هنا يتم احتساب حساب المدين الافتراضي وهو البنك /مبارك
                //strSQL = "SELECT DebitAmount - DiscountAmount AS NetBalance,Notes AS Declaration, CheckReceiptVoucherDate AS TheDate, 'CheckReceiptVoucher' AS RecordType, CheckReceiptVoucherID AS ID, DiscountAccountID, RegTime , " + " DebitAccountID, ' ' AS OppsiteAccountName FROM Acc_CheckReceiptVoucherMaster  WHERE (Cancel = 0) AND (BranchID = " + WT.GlobalBranchID + ")" + " AND (DebitAccountID = " + txtAccountID.TextWT + ") ";
                //strSQL = "SELECT DebitAmount - DiscountAmount AS NetBalance,Notes AS Declaration, CheckReceiptVoucherDate AS TheDate, 'CheckReceiptVoucher' AS RecordType,"
                //+ " CheckReceiptVoucherID AS ID, DiscountAccountID, RegTime , DebitAccountID, ' ' AS OppsiteAccountName FROM Acc_CheckReceiptVoucherMaster  WHERE (Cancel = 0)"
                //+ " AND BranchID =" + UserInfo.BRANCHID + " AND FacilityID =" + UserInfo.FacilityID.ToString() + " AND DebitAccountID =" + AccountID;
                ////if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                ////{

                ////    strSQL = strSQL + " AND  Acc_CheckReceiptVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                ////}
                ////if (FromDate != 0)
                ////{
                ////    strSQL = strSQL + " AND CheckReceiptVoucherDate >=" + FromDate;
                ////}

                ////if (ToDate != 0)
                ////{
                ////    strSQL = strSQL + " AND CheckReceiptVoucherDate <=" + ToDate;
                ////}

                //strSQL = strSQL + " ORDER BY CheckReceiptVoucherDate,RegTime";
                //Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtDebit = Lip.SelectRecord(strSQL);
                    if (dtDebit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();
                            row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند قبض شيك" : "Check Receipt Voucher");
                            row["ID"] = dtDebit.Rows[i]["ID"];
                            row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);
                            row["OppsiteAccountName"] = (lang == "Eng" ? "Mentioned" : "مذكورين");
                            NetBalance = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["SumCredit"]) - Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["SumDiscount"]);
                            row["Debit"] = NetBalance;
                            row["Credit"] = 0;
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }

                //هنا يتم احتساب حساب الدائن
                // strSQL = "SELECT Acc_CheckReceiptVoucherDetails.Declaration, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate AS TheDate, " + " 'CheckReceiptVoucher' AS RecordType, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID AS ID, Acc_CheckReceiptVoucherMaster.RegTime, " + " Acc_CheckReceiptVoucherDetails.CreditAmount AS SumCreditAmount, Acc_CheckReceiptVoucherDetails.AccountID, Acc_Accounts.Arb_Name As OppsiteAccountName" + " FROM Acc_CheckReceiptVoucherMaster INNER JOIN" + " Acc_Accounts ON Acc_CheckReceiptVoucherMaster.BranchID = Acc_Accounts.BranchID AND " + " Acc_CheckReceiptVoucherMaster.DebitAccountID = Acc_Accounts.AccountID RIGHT OUTER JOIN" + " Acc_CheckReceiptVoucherDetails ON Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID = Acc_CheckReceiptVoucherDetails.CheckReceiptVoucherID AND " + " Acc_CheckReceiptVoucherMaster.BranchID = Acc_CheckReceiptVoucherDetails.BranchID" + " WHERE Acc_CheckReceiptVoucherMaster.Cancel = 0 AND Acc_CheckReceiptVoucherMaster.BranchID = " + WT.GlobalBranchID + " AND " + " Acc_CheckReceiptVoucherDetails.AccountID = " + txtAccountID.TextWT;
                strSQL = "SELECT Acc_CheckReceiptVoucherDetails.DECLARATION,Acc_CheckReceiptVoucherMaster.CHECKRECEIPTVOUCHERDATE AS TheDate, 'CheckReceiptVoucher' AS RecordType,"
                + " Acc_CheckReceiptVoucherMaster.CHECKRECEIPTVOUCHERID AS ID,Acc_CheckReceiptVoucherMaster.RegTime,Acc_CheckReceiptVoucherDetails.CREDITAMOUNT AS SumCreditAmount,"
                + " Acc_CheckReceiptVoucherDetails.ACCOUNTID,Acc_Accounts.ArbName AS OppsiteAccountName FROM Acc_CheckReceiptVoucherMaster INNER JOIN Acc_Accounts"
                + " ON Acc_CheckReceiptVoucherMaster.BranchID = Acc_Accounts.BranchID AND Acc_CheckReceiptVoucherMaster.DEBITACCOUNTID = Acc_Accounts.ACCOUNTID"
                + " AND Acc_Accounts.FacilityID= Acc_CheckReceiptVoucherMaster.FacilityID RIGHT OUTER JOIN Acc_CheckReceiptVoucherDetails ON Acc_CheckReceiptVoucherMaster.CHECKRECEIPTVOUCHERID"
                + " = Acc_CheckReceiptVoucherDetails.CHECKRECEIPTVOUCHERID AND Acc_CheckReceiptVoucherMaster.BranchID= Acc_CheckReceiptVoucherDetails.BranchID"
                + " AND Acc_CheckReceiptVoucherDetails.FacilityID= Acc_CheckReceiptVoucherMaster.FacilityID WHERE Acc_CheckReceiptVoucherDetails.ACCOUNTID =" + AccountID
                + " AND Acc_CheckReceiptVoucherMaster.CANCEL= 0 AND Acc_CheckReceiptVoucherMaster.BranchID = " + UserInfo.BRANCHID
                + " AND Acc_CheckReceiptVoucherMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_CheckReceiptVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + "  GROUP BY Acc_CheckReceiptVoucherDetails.Declaration, Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate, "
                + " Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherID, Acc_CheckReceiptVoucherMaster.RegTime, Acc_CheckReceiptVoucherDetails.CreditAmount,"
                + " Acc_CheckReceiptVoucherDetails.AccountID, Acc_Accounts.ArbName ORDER BY Acc_CheckReceiptVoucherMaster.CheckReceiptVoucherDate,Acc_CheckReceiptVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

                if (strSQL != null)
                {
                    dtCredit = Lip.SelectRecord(strSQL);
                    if (dtCredit.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                            row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                            row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                            row["OppsiteAccountName"] = dtCredit.Rows[i]["OppsiteAccountName"];
                            row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                            row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                            row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند قبض شيك" : "Check Receipt Voucher");
                            row["ID"] = dtCredit.Rows[i]["ID"];
                            row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                            row["Credit"] = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["SumCreditAmount"]);
                            row["Balance"] = 0;
                            _sampleData.Rows.Add(row);
                        }
                    }
                }
                dtDiscount.Dispose();
                dtCredit.Dispose();
                dtDebit.Dispose();

                row = null;
            }
            catch { }
        }

        private void CheckSpendVoucher(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                DataTable dtDebit = new DataTable();
                DataTable dtDiscount = new DataTable();
                string strSQL = ""; DataRow row;
                decimal NetBalance = 0;

                //إضافة هذه الجملة الجديدة لاحتساب حساب الخصم المكتسب به ضمن سند صرف الشيك ، حيث يكون دائن
                //strSQL = "SELECT Acc_CheckSpendVoucherDetails.Declaration, Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate AS TheDate, " + " 'CheckSpendVoucher' AS RecordType, Acc_CheckSpendVoucherMaster.CheckSpendVoucherID AS ID, Acc_CheckSpendVoucherMaster.DiscountAccountID, " + " Acc_CheckSpendVoucherMaster.RegTime, ' ' AS OppsiteAccountName, SUM(Acc_CheckSpendVoucherDetails.Discount) AS SumDiscount" + " FROM Acc_CheckSpendVoucherMaster RIGHT OUTER JOIN" + " Acc_CheckSpendVoucherDetails ON Acc_CheckSpendVoucherMaster.CheckSpendVoucherID = Acc_CheckSpendVoucherDetails.CheckSpendVoucherID AND " + " Acc_CheckSpendVoucherMaster.BranchID = Acc_CheckSpendVoucherDetails.BranchID" + " WHERE Acc_CheckSpendVoucherMaster.Cancel = 0 AND Acc_CheckSpendVoucherMaster.BranchID = " + WT.GlobalBranchID + " AND " + " Acc_CheckSpendVoucherMaster.DiscountAccountID = " + txtAccountID.TextWT + " GROUP BY Acc_CheckSpendVoucherDetails.Declaration, Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate, Acc_CheckSpendVoucherMaster.CheckSpendVoucherID, " + " Acc_CheckSpendVoucherMaster.DiscountAccountID, Acc_CheckSpendVoucherMaster.RegTime" + " HAVING (SUM(Acc_CheckSpendVoucherDetails.Discount) > 0)  ";
                strSQL = "SELECT Acc_CheckSpendVoucherDetails.DECLARATION,Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERDATE AS TheDate, 'CheckSpendVoucher' AS RecordType,"
                + " Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID AS ID,Acc_CheckSpendVoucherMaster.DISCOUNTACCOUNTID,Acc_CheckSpendVoucherMaster.RegTime,' ' "
                + " AS OppsiteAccountName,SUM(Acc_CheckSpendVoucherDetails.DISCOUNT) AS SumDiscount FROM Acc_CheckSpendVoucherMaster RIGHT OUTER JOIN"
                + " Acc_CheckSpendVoucherDetails ON Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID = Acc_CheckSpendVoucherDetails.CHECKSPENDVOUCHERID"
                + " AND Acc_CheckSpendVoucherMaster.BranchID= Acc_CheckSpendVoucherDetails.BranchID AND Acc_CheckSpendVoucherDetails.FacilityID"
                + " = Acc_CheckSpendVoucherMaster.FacilityID WHERE Acc_CheckSpendVoucherMaster.CANCEL = 0 AND Acc_CheckSpendVoucherMaster.BranchID=" + UserInfo.BRANCHID


                + " AND Acc_CheckSpendVoucherMaster.FacilityID =" + UserInfo.FacilityID.ToString() + " AND Acc_CheckSpendVoucherMaster.DISCOUNTACCOUNTID =" + AccountID;
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_CheckSpendVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }



                strSQL = strSQL + " GROUP BY Acc_CheckSpendVoucherDetails.DECLARATION, Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERDATE,Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID,"
                + " Acc_CheckSpendVoucherMaster.DISCOUNTACCOUNTID,Acc_CheckSpendVoucherMaster.RegTime HAVING SUM(Acc_CheckSpendVoucherDetails.DISCOUNT) > 0 ";
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate,Acc_CheckSpendVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtDiscount = Lip.SelectRecord(strSQL);
                if (dtDiscount.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDiscount.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtDiscount.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtDiscount.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مذكورين" : "Mentioned");
                        row["RegTime"] = dtDiscount.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtDiscount.Rows[i]["RecordType"];
                        row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند صرف شيك" : "Check Spend Voucher");
                        row["ID"] = dtDiscount.Rows[i]["ID"];
                        row["Declaration"] = (dtDiscount.Rows[i]["Declaration"].ToString() != string.Empty ? dtDiscount.Rows[i]["Declaration"] : dtDiscount.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDiscount.Rows[i]["ID"]);
                        row["Credit"] = Comon.ConvertToDecimalPrice(dtDiscount.Rows[i]["SumDiscount"]);
                        _sampleData.Rows.Add(row);
                    }
                }
                /////////////////////////////////////////////////////////////////////////
                strSQL = "SELECT Acc_CheckSpendVoucherDetails.DECLARATION,Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERDATE AS TheDate, 'CheckSpendVoucher' AS RecordType,"
              + " Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID AS ID,Acc_CheckSpendVoucherMaster.DISCOUNTACCOUNTID,Acc_CheckSpendVoucherMaster.RegTime,' ' "
              + " AS OppsiteAccountName,SUM(Acc_CheckSpendVoucherDetails.DISCOUNT) AS SumDiscount,SUM(Acc_CheckSpendVoucherDetails.DebitAmount) AS SumDebit FROM Acc_CheckSpendVoucherMaster RIGHT OUTER JOIN"
              + " Acc_CheckSpendVoucherDetails ON Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID = Acc_CheckSpendVoucherDetails.CHECKSPENDVOUCHERID"
              + " AND Acc_CheckSpendVoucherMaster.BranchID= Acc_CheckSpendVoucherDetails.BranchID AND Acc_CheckSpendVoucherDetails.FacilityID"
              + " = Acc_CheckSpendVoucherMaster.FacilityID WHERE Acc_CheckSpendVoucherMaster.CANCEL = 0 AND Acc_CheckSpendVoucherMaster.BranchID=" + UserInfo.BRANCHID


              + " AND Acc_CheckSpendVoucherMaster.FacilityID =" + UserInfo.FacilityID.ToString() + " AND Acc_CheckSpendVoucherMaster.CreditAccountID =" + AccountID;
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_CheckSpendVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }



                strSQL = strSQL + " GROUP BY Acc_CheckSpendVoucherDetails.DECLARATION, Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERDATE,Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID,"
                + " Acc_CheckSpendVoucherMaster.DISCOUNTACCOUNTID,Acc_CheckSpendVoucherMaster.RegTime  ";
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate,Acc_CheckSpendVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                // مبارك هنا يتم احتساب حساب الدائن الافتراضي وهو البنك
                //strSQL = "SELECT CreditAmount - DiscountAmount AS NetBalance,Notes AS Declaration, CheckSpendVoucherDate AS TheDate, 'CheckSpendVoucher' AS RecordType, CheckSpendVoucherID AS ID, DiscountAccountID, RegTime , " + " CreditAccountID, ' ' AS OppsiteAccountName FROM Acc_CheckSpendVoucherMaster  WHERE (Cancel = 0) AND (BranchID = " + WT.GlobalBranchID + ")" + " AND (CreditAccountID = " + txtAccountID.TextWT + ")  ";
                //strSQL = "SELECT CreditAmount - DiscountAmount AS NetBalance,Notes AS Declaration, CheckSpendVoucherDate AS TheDate, 'CheckSpendVoucher' AS RecordType, "
                //+ " CheckSpendVoucherID AS ID, DiscountAccountID, RegTime , CreditAccountID, ' ' AS OppsiteAccountName FROM Acc_CheckSpendVoucherMaster"
                //+ " WHERE Cancel = 0 AND BranchID =" + UserInfo.BRANCHID + " AND FacilityID =" + UserInfo.FacilityID.ToString() + " AND CreditAccountID = " + AccountID;
                ////if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                ////{

                ////    strSQL = strSQL + " AND  Acc_CheckSpendVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                ////}
                ////if (FromDate != 0)
                ////{
                ////    strSQL = strSQL + " AND CheckSpendVoucherDate >=" + FromDate;
                ////}

                ////if (ToDate != 0)
                ////{
                ////    strSQL = strSQL + " AND CheckSpendVoucherDate <=" + ToDate;
                ////}

                //strSQL = strSQL + " ORDER BY CheckSpendVoucherDate,RegTime";

                //Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtCredit = Lip.SelectRecord(strSQL);
                if (dtCredit.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مذكورين" : "Mentioned");
                        row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                        row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند صرف شيك" : "Check Spend Voucher");
                        row["ID"] = dtCredit.Rows[i]["ID"];
                        row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                        NetBalance = Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["SumDebit"]) - Comon.ConvertToDecimalPrice(dtCredit.Rows[i]["SumDiscount"]);
                        row["Credit"] = NetBalance;
                        row["Debit"] = 0;
                        _sampleData.Rows.Add(row);
                    }
                }

                //هنا يتم احتساب حساب المدين
                //strSQL = "SELECT Acc_CheckSpendVoucherDetails.Declaration, Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate AS TheDate, " + " 'CheckSpendVoucher' AS RecordType, Acc_CheckSpendVoucherMaster.CheckSpendVoucherID AS ID, Acc_CheckSpendVoucherMaster.RegTime, " + " Acc_CheckSpendVoucherDetails.DebitAmount AS SumDebitAmount, Acc_CheckSpendVoucherDetails.AccountID, Acc_Accounts.Arb_Name As OppsiteAccountName" + " FROM Acc_CheckSpendVoucherMaster INNER JOIN" + " Acc_Accounts ON Acc_CheckSpendVoucherMaster.BranchID = Acc_Accounts.BranchID AND " + " Acc_CheckSpendVoucherMaster.CreditAccountID = Acc_Accounts.AccountID RIGHT OUTER JOIN" + " Acc_CheckSpendVoucherDetails ON Acc_CheckSpendVoucherMaster.CheckSpendVoucherID = Acc_CheckSpendVoucherDetails.CheckSpendVoucherID AND " + " Acc_CheckSpendVoucherMaster.BranchID = Acc_CheckSpendVoucherDetails.BranchID" + " WHERE Acc_CheckSpendVoucherMaster.Cancel = 0 AND Acc_CheckSpendVoucherMaster.BranchID = " + WT.GlobalBranchID + " AND " + " Acc_CheckSpendVoucherDetails.AccountID = " + txtAccountID.TextWT;
                strSQL = "SELECT Acc_CheckSpendVoucherDetails.DECLARATION,Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERDATE AS TheDate,'CheckSpendVoucher' AS RecordType,"
                + " Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID AS ID,Acc_CheckSpendVoucherMaster.RegTime,Acc_CheckSpendVoucherDetails.DEBITAMOUNT AS "
                + " SumDebitAmount,Acc_CheckSpendVoucherDetails.ACCOUNTID,Acc_Accounts.ArbName AS OppsiteAccountName FROM Acc_CheckSpendVoucherMaster"
                + " INNER JOIN Acc_Accounts ON Acc_CheckSpendVoucherMaster.BranchID= Acc_Accounts.BranchID AND Acc_CheckSpendVoucherMaster.CREDITACCOUNTID"
                + " = Acc_Accounts.ACCOUNTID AND Acc_CheckSpendVoucherMaster.FacilityID= Acc_Accounts.FacilityID RIGHT OUTER JOIN Acc_CheckSpendVoucherDetails"
                + " ON Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID = Acc_CheckSpendVoucherDetails.CHECKSPENDVOUCHERID AND Acc_CheckSpendVoucherMaster.BranchID"
                + " = Acc_CheckSpendVoucherDetails.BranchID AND Acc_CheckSpendVoucherDetails.FacilityID = Acc_CheckSpendVoucherMaster.FacilityID"
                + " WHERE Acc_CheckSpendVoucherMaster.CANCEL = 0 AND Acc_CheckSpendVoucherMaster.BranchID =" + UserInfo.BRANCHID
                + " AND Acc_CheckSpendVoucherMaster.FacilityID= " + UserInfo.FacilityID.ToString()
                + " AND Acc_CheckSpendVoucherDetails.ACCOUNTID=" + AccountID;
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_CheckSpendVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }
                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + "  GROUP BY Acc_CheckSpendVoucherDetails.DECLARATION,Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERDATE,Acc_CheckSpendVoucherMaster.CHECKSPENDVOUCHERID,"
                + " Acc_CheckSpendVoucherMaster.RegTime,Acc_CheckSpendVoucherDetails.DEBITAMOUNT,Acc_CheckSpendVoucherDetails.ACCOUNTID,Acc_Accounts.ArbName"
                + " ORDER BY Acc_CheckSpendVoucherMaster.CheckSpendVoucherDate,Acc_CheckSpendVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtDebit = Lip.SelectRecord(strSQL);
                if (dtDebit.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDebit.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtDebit.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtDebit.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = dtDebit.Rows[i]["OppsiteAccountName"];
                        row["RegTime"] = dtDebit.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtDebit.Rows[i]["RecordType"];
                        row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "سند صرف شيك" : "Check Spend Voucher");
                        row["ID"] = dtDebit.Rows[i]["ID"];
                        row["Declaration"] = (dtDebit.Rows[i]["Declaration"].ToString() != string.Empty ? dtDebit.Rows[i]["Declaration"] : dtDebit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtDebit.Rows[i]["ID"]);
                        row["Debit"] = Comon.ConvertToDecimalPrice(dtDebit.Rows[i]["SumDebitAmount"]);
                        _sampleData.Rows.Add(row);
                    }
                }

                dtCredit.Dispose();
                dtDebit.Dispose();
                dtDiscount.Dispose();
                row = null;
            }
            catch { }
        }

        private void VariousVoucher(string AccountID, long FromDate, long ToDate)
        {
            try
            {
                DataTable dtCredit = new DataTable();
                string strSQL = null; DataRow row;
                //strSQL = "SELECT Acc_VariousVoucherDetails.Declaration, Acc_VariousVoucherMaster.VoucherDate AS TheDate, Acc_VariousVoucherMaster.VoucherID" + " AS ID, 'VariousVoucher' AS RecordType, ' ' AS OppsiteAccountName, Acc_VariousVoucherDetails.AccountID, Acc_VariousVoucherDetails.Debit, Acc_VariousVoucherMaster.RegTime, " + " Acc_VariousVoucherDetails.Credit FROM Acc_VariousVoucherMaster INNER JOIN Acc_VariousVoucherDetails ON Acc_VariousVoucherMaster.VoucherID" + " = Acc_VariousVoucherDetails.VoucherID AND Acc_VariousVoucherMaster.BranchID = Acc_VariousVoucherDetails.BranchID " + " WHERE (Acc_VariousVoucherMaster.Cancel = 0) AND (Acc_VariousVoucherMaster.BranchID = " + WT.GlobalBranchID + ")" + " AND (Acc_VariousVoucherDetails.AccountID = " + txtAccountID.TextWT + ") ";
                strSQL = "SELECT Acc_VariousVoucherDetails.DECLARATION,Acc_VariousVoucherMaster.VOUCHERDATE AS TheDate,Acc_VariousVoucherMaster.VOUCHERID AS ID,"
                + " 'VariousVoucher' AS RecordType, ' ' AS OppsiteAccountName,Acc_VariousVoucherDetails.ACCOUNTID,Acc_VariousVoucherDetails.DEBIT,"
                + " Acc_VariousVoucherMaster.RegTime,Acc_VariousVoucherDetails.CREDIT FROM Acc_VariousVoucherMaster INNER JOIN Acc_VariousVoucherDetails"
                + " ON Acc_VariousVoucherMaster.VOUCHERID= Acc_VariousVoucherDetails.VOUCHERID AND Acc_VariousVoucherMaster.BranchID= Acc_VariousVoucherDetails.BranchID"
                + " AND Acc_VariousVoucherMaster.FacilityID  = Acc_VariousVoucherDetails.FacilityID WHERE Acc_VariousVoucherDetails.ACCOUNTID = " + AccountID
                + " AND Acc_VariousVoucherMaster.CANCEL = 0 AND Acc_VariousVoucherMaster.BranchID =" + UserInfo.BRANCHID + " AND Acc_VariousVoucherMaster.FacilityID =" + UserInfo.FacilityID.ToString();
                if (!string.IsNullOrEmpty(txtCostCenterID.Text))
                {

                    strSQL = strSQL + " AND  Acc_VariousVoucherDetails.CostCenterID =" + Comon.cLong(txtCostCenterID.Text);
                }

                //if (FromDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_VariousVoucherMaster.VoucherDate >=" + FromDate;
                //}

                //if (ToDate != 0)
                //{
                //    strSQL = strSQL + " AND Acc_VariousVoucherMaster.VoucherDate <=" + ToDate;
                //}

                strSQL = strSQL + " ORDER BY Acc_VariousVoucherMaster.VoucherDate,Acc_VariousVoucherMaster.RegTime";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
                dtCredit = Lip.SelectRecord(strSQL);
                if (dtCredit.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtCredit.Rows.Count - 1; i++)
                    {
                        row = _sampleData.NewRow();
                        row["n_invoice_serial"] = _sampleData.Rows.Count + 1;
                        row["TheDate"] = Comon.ConvertSerialDateTo(dtCredit.Rows[i]["TheDate"].ToString());
                        row["TheDate"] = dtCredit.Rows[i]["TheDate"].ToString();
                        row["OppsiteAccountName"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "مذكورين" : "Mentioned");
                        row["RegTime"] = dtCredit.Rows[i]["RegTime"];
                        row["TempRecordType"] = dtCredit.Rows[i]["RecordType"];
                        row["RecordType"] = (UserInfo.Language.ToString() == iLanguage.Arabic.ToString() ? "قيد يومي" : "Various Voucher");
                        row["ID"] = dtCredit.Rows[i]["ID"];
                        row["Declaration"] = (dtCredit.Rows[i]["Declaration"].ToString() != string.Empty ? dtCredit.Rows[i]["Declaration"] : dtCredit.Rows[i]["RecordType"] + lang == "Eng" ? "No." : " رقم " + dtCredit.Rows[i]["ID"]);
                        row["Credit"] = dtCredit.Rows[i]["Credit"];
                        row["Debit"] = dtCredit.Rows[i]["Debit"];
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
            Accounts =   Acc_AccountsDAL.GetDataByID(Comon.cLong(txtAccountID.Text), 1, 1);
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
                double AccountID = Comon.cDbl(cmbNetType.EditValue);
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
                ProcessWithOutDate(AccountID.ToString(), FromDate, ToDate);
            }
            else
            {
                ProcessWithOutDate(AccountID.ToString(), FromDate, ToDate);
            }
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
                MySession.GlobalAccountsLevelDigits = 4;
                PrepareSearchQuery.SearchForAccounts(txtAccountID, lblAccountName);
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

            switch (view.GetFocusedRowCellValue("TempRecordType").ToString())
            {
                case "PurchaseInvoice":
                    frmPurchaseInvoice frm = new frmPurchaseInvoice();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                        frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm.Dispose();


                    break;

                case "ItemsOutOnBail":
                    frmItemsOutOnBail frm11 = new frmItemsOutOnBail();
                    if (Permissions.UserPermissionsFrom(frm11, frm11.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm11);
                        frm11.Show();
                        frm11.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm11.Dispose();


                    break;


                case "ItemsInOnBail":
                    frmItemsInonBail frm12 = new frmItemsInonBail();
                    if (Permissions.UserPermissionsFrom(frm12, frm12.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm12);
                        frm12.Show();
                        frm12.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm12.Dispose();


                    break;

                case "GoodsOpening":
                    frmGoodsOpeningOld frm1 = new frmGoodsOpeningOld();
                    if (Permissions.UserPermissionsFrom(frm1, frm1.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm1);
                        frm1.Show();
                        frm1.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm1.Dispose();
                    break;
                //case "ItemsTransfer":
                //   frmItemsTransfer   frm =new frmItemsTransfer();
                //   //  Lip.Ch(frm, Language)
                //     frm.Show();
                //     frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString())+1,8);
                //    break;
                case "ItemsDismantling":
                    frmItemsDismantling frm10 = new frmItemsDismantling();
                    if (Permissions.UserPermissionsFrom(frm10, frm10.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm10);
                        frm10.Show();
                        frm10.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm10.Dispose();
                    break;
                case "SalesInvoiceReturn":
                    frmSalesInvoiceReturn frm2 = new frmSalesInvoiceReturn();
                    if (Permissions.UserPermissionsFrom(frm2, frm2.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm2);
                        frm2.Show();
                        frm2.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm2.Dispose();
                    break;
                case "SalesInvoice":
                    frmSalesInvoice frm3 = new frmSalesInvoice();
                    if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm3);
                        frm3.Show();
                        frm3.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm3.Dispose();
                    break;
                case "PurchaseInvoiceReturn":
                    frmPurchaseInvoiceReturn frm4 = new frmPurchaseInvoiceReturn();
                    if (Permissions.UserPermissionsFrom(frm4, frm4.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm4);
                        frm4.Show();
                        frm4.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm4.Dispose();
                    break;

                case "ReceiptVoucher":
                    frmReceiptVoucher frm20 = new frmReceiptVoucher();
                    if (Permissions.UserPermissionsFrom(frm20, frm20.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm20);
                        frm20.Show();
                        frm20.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm20.Dispose();
                    break;

                case "CheckSpendVoucher":
                    frmCheckSpendVoucher frm24 = new frmCheckSpendVoucher();
                    if (Permissions.UserPermissionsFrom(frm24, frm24.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
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
                    if (Permissions.UserPermissionsFrom(frm23, frm23.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm23);
                        frm23.Show();
                        frm23.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm23.Dispose();
                    break;

                case "VariousVoucher":
                    frmVariousVoucher frm22 = new frmVariousVoucher();
                    if (Permissions.UserPermissionsFrom(frm22, frm22.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm22);
                        frm22.Show();
                        frm22.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm22.Dispose();
                    break;

                case "SpendVoucher":
                    frmSpendVoucher frm21 = new frmSpendVoucher();
                    if (Permissions.UserPermissionsFrom(frm21, frm21.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm21);
                        frm21.Show();
                        frm21.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm21.Dispose();
                    break;
                case "OpeningVoucher":
                    frmOpeningVoucher frm211 = new frmOpeningVoucher();
                    if (Permissions.UserPermissionsFrom(frm211, frm211.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm211);
                        frm211.Show();
                        //frm211.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("ID").ToString()) + 1, 8);
                    }
                    else
                        frm211.Dispose();
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + Comon.cDbl( txtAccountID.Text) + ") ";
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
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
    }

}



 
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
    public partial class frmCustomersAccountStatement : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        string frm = "frmCustomersAccountStatement";
        private string strSQL = "";
        private string where = "";
        private string lang = "";
        private string langName = "";
        private string FormatType = "Gre";
        private string PrimaryName;
        public DataTable _sampleData = new DataTable();
        public DataTable _sampleDataCustomer = new DataTable();
       
      
        public frmCustomersAccountStatement()
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
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            GridView1.OptionsView.EnableAppearanceEvenRow = true;
            GridView1.OptionsView.EnableAppearanceOddRow = true;
            GridView1.OptionsBehavior.ReadOnly = true;
            GridView1.OptionsBehavior.Editable = false;
            InitializeFormatDate(txtFromDate);
            InitializeFormatDate(txtToDate);
            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {

                dgvColAccountID.Caption = "Account NO ";
                dgvColAccountName.Caption = "Account Name  ";
                dgvColCredit.Caption = "Credit";
                dgvColDebit.Caption = "Debit  ";

                dgvColn_invoice_serial.Caption = "# ";
                dgvColBalance.Caption = "Balance";

                PrimaryName = "EngName";


                btnShow.Text = "show";
                //  Label8.Text = btnShow.Tag.ToString();

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

        private void frmAccountStatement_Load(object sender, EventArgs e)
        {
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue =MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;


            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;

            try
            {
            where = "FACILITYID=" + UserInfo.FacilityID + " AND BRANCHID=" + MySession.GlobalBranchID;
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
            _sampleData.Columns.Add(new DataColumn("Posted", typeof(string)));

            _sampleDataCustomer.Columns.Add(new DataColumn("n_invoice_serial", typeof(string)));
            _sampleDataCustomer.Columns.Add(new DataColumn("Balance", typeof(decimal)));
            _sampleDataCustomer.Columns.Add(new DataColumn("Debit", typeof(decimal)));
            _sampleDataCustomer.Columns.Add(new DataColumn("Credit", typeof(decimal)));
           _sampleDataCustomer.Columns.Add(new DataColumn("BalanceGold", typeof(decimal)));
           _sampleDataCustomer.Columns.Add(new DataColumn("DebitGold", typeof(decimal)));
           _sampleDataCustomer.Columns.Add(new DataColumn("CreditGold", typeof(decimal)));


           _sampleData.Columns.Add(new DataColumn("DebitDiamond", typeof(decimal)));
           _sampleData.Columns.Add(new DataColumn("CreditDiamond", typeof(decimal)));
           _sampleData.Columns.Add(new DataColumn("BalanceDiamond", typeof(decimal)));


           _sampleDataCustomer.Columns.Add(new DataColumn("DebitDiamond", typeof(decimal)));
           _sampleDataCustomer.Columns.Add(new DataColumn("CreditDiamond", typeof(decimal)));
           _sampleDataCustomer.Columns.Add(new DataColumn("BalanceDiamond", typeof(decimal)));

            _sampleDataCustomer.Columns.Add(new DataColumn("AccountID", typeof(string)));
            _sampleDataCustomer.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            _sampleDataCustomer.Columns.Add(new DataColumn("Address", typeof(string)));
            _sampleDataCustomer.Columns.Add(new DataColumn("BalanceType", typeof(string)));
           _sampleDataCustomer.Columns.Add(new DataColumn("CustomerBalance", typeof(string)));
           _sampleDataCustomer.Columns.Add(new DataColumn("Posted", typeof(string)));
          
            InitialFiveRows(_sampleData, 1);
            FormsPrperties.PropertiesGridView(GridView1, this.Name);
            PrimaryName = "ArbName";
            }
            catch { }
        }

        //long FromDate = 0;
        //long ToDate = 0;

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
         protected override void DoPrint()
        {
            try
            {
                if (GridView1.DataRowCount - 1 == 0 && GridView1.GetRowCellValue(0, "ID").ToString() == "")
                {
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                ReportName = "rptCustomersAccountStatement";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /***************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["NameReport"].Value =this.Text.Trim().ToString();
                //rptForm.Parameters["MainAccountName"].Value = lblAccountName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["TotalDebit"].Value = lblDebit.Text.Trim().ToString();
                rptForm.Parameters["TotalCredit"].Value = lblCredit.Text.Trim().ToString();
                rptForm.Parameters["TotalBalance"].Value = lblBalanceSum.Text.Trim().ToString();
                rptForm.Parameters["TotalDebitGold"].Value =lblDebitGold.Text.Trim().ToString();
                rptForm.Parameters["TotalCreditGold"].Value =lblCreditGold.Text.Trim().ToString();
                rptForm.Parameters["TotalBalanceGold"].Value =lblBalanceSumGold.Text.Trim().ToString();
                rptForm.Parameters["TotalDebitDiamond"].Value =lblDebitDiamond.Text.Trim().ToString();
                rptForm.Parameters["TotalCreditDiamond"].Value =lblCreditDiamond.Text.Trim().ToString();
                rptForm.Parameters["TotalBalanceDiamond"].Value =lblBalanceSumDiamond.Text.Trim().ToString();

                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                /********************** Details ****************************/
                var dataTable = new dsReports.rptCustomersAccountStatementDataTable();
                for (int i = 0; i <= rptForm.Parameters.Count - 1; i++)
                {
                    rptForm.Parameters[i].Visible = false;
                }
                try
                {

                    for (int i = 0; i < GridView1.DataRowCount - 1; i++)
                    {
                        var row = dataTable.NewRow();
                        row["n_invoice_serial"] = i + 1;
                        row["Balance"] = GridView1.GetRowCellValue(i, "Balance").ToString();
                        row["Debit"] = GridView1.GetRowCellValue(i, "Debit").ToString();
                        row["Credit"] = GridView1.GetRowCellValue(i, "Credit").ToString();
                        row["BalanceGold"] = GridView1.GetRowCellValue(i, "BalanceGold").ToString();
                        row["DebitGold"] = GridView1.GetRowCellValue(i, "DebitGold").ToString();
                        row["CreditGold"] = GridView1.GetRowCellValue(i, "CreditGold").ToString();

                        row["DebitDiamond"] = GridView1.GetRowCellValue(i, "DebitDiamond").ToString();
                        row["CreditDiamond"] = GridView1.GetRowCellValue(i, "CreditDiamond").ToString();

                       
                        row["BalanceDiamond"] = GridView1.GetRowCellValue(i, "BalanceDiamond").ToString();
                        row["OppsiteAccountName"] = GridView1.GetRowCellValue(i, "CustomerName").ToString();
                        row["CustomerBalance"] = GridView1.GetRowCellValue(i, "CustomerBalance").ToString();
                        // row["TheDate"] = GridView1.GetRowCellValue(i, "TheDate").ToString();
                        row["ID"] = GridView1.GetRowCellValue(i, "AccountID").ToString();
                        dataTable.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {

                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptCustomersAccountStatement";

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
                    if (dt.Rows.Count > 0)for (int i = 1; i < 6; i++)
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            
                DoPrint();
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

                if (FromDate != 0 && _sampleData.Rows.Count>0)
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
                    int keys = 0;
                    if (_sampleData.Rows.Count > 1)
                        keys = 1;
                    else if (_sampleData.Rows.Count < 1)
                        keys = 0;
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

                    if (FromDate == 0) {
                        return;
                    }
                    addEvenRow();
                    addEvenRow();

                }
            }
            catch { }
        }
     
        #endregion
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
                    row["Credit"] = view[i]["Credit"];
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
                lblDebit.Text = debit.ToString();
                lblCredit.Text = credit.ToString();
                lblBalanceSum.Text = Math.Abs(total).ToString();
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

        

        protected override void DoAddFrom()
        {
            try
            {
                _sampleDataCustomer.Clear();
                gridControl1.RefreshDataSource();

                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";

                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtCostCenterID.Enabled = true;
                btnCostCenterSearch.Enabled = true;



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
      
        private void btnShow_Click(object sender, EventArgs e)
        {
            gridColumn1.Caption = "رصيد العميل";
            ProgressBar.Value = 0;
            ProgressBar.Visible = true;
            long AccountID = 0;
            //strSQL = "";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            DataTable dtCustomer = new DataTable();
       
            if (chkCustomer.Checked)
            {
                strSQL = "SELECT AccountID, " + PrimaryName + " As AccountName   FROM  [Sales_Customers] where Cancel=0 and BranchID=" + cmbBranchesID.EditValue;
                gridColumn1.Caption = "رصيد العميل";
            }
            if (chkSupliar.Checked)
            {
                strSQL = "SELECT AccountID, " + PrimaryName + " As AccountName   FROM  [Sales_Suppliers] where Cancel=0 and BranchID=" + cmbBranchesID.EditValue;
                gridColumn1.Caption = "رصيد المورد";
            }
            dtCustomer = Lip.SelectRecord(strSQL);
            if (dtCustomer.Rows.Count < 1) return;

           
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL,UserInfo.Language.ToString ());
            dtCustomer = Lip.SelectRecord(strSQL);
            if (dtCustomer.Rows.Count > 0)
                btnShow.Visible = false;


            Application.DoEvents();
            _sampleDataCustomer.Clear();
            #region GetBalanceCustomer
            ProgressBar.Maximum = dtCustomer.Rows.Count;
           
            for (int i = 0; i <= dtCustomer.Rows.Count - 1; i++)
            {
                ProgressBar.Value = ProgressBar.Value + 1;
                AccountID = Comon.cLong(dtCustomer.Rows[i]["AccountID"].ToString());
                long FromDate1 = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate1 = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                // OpenWindow(frm);
                frmAccountStatement frm = new frmAccountStatement(AccountID);
                frm.cmbBranchesID.EditValue = cmbBranchesID.EditValue;
                frm.ProcessWithOutDate(AccountID.ToString(), FromDate1, ToDate1);

                lblBalanceType.Text = dtCustomer.Rows[i][1].ToString();
                decimal total = 0;

                if (frm._sampleData.Rows.Count > 1)
                {
                    if (Comon.ConvertToDecimalPrice(frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["Balance"].ToString()) > 0 || Comon.ConvertToDecimalPrice(frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["BalanceDiamond"].ToString()) > 0 || Comon.ConvertToDecimalPrice(frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["BalanceGold"].ToString()) > 0)
                    {
                        _sampleDataCustomer.NewRow();
                        _sampleDataCustomer.Rows.Add();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["AccountID"] = AccountID.ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CustomerName"] = dtCustomer.Rows[i][1].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Balance"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["Balance"].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["Debit"].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["Credit"].ToString();

                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceGold"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["BalanceGold"].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitGold"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["DebitGold"].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditGold"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["CreditGold"].ToString();

                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceDiamond"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["BalanceDiamond"].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditDiamond"]= frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["DebitDiamond"].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitDiamond"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["CreditDiamond"].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Posted"] = frm._sampleData.Rows[frm._sampleData.Rows.Count - 1]["Posted"].ToString();


                        if(chkCustomer.Checked==true)
                        total = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"].ToString()) - Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"].ToString());
                        else
                        total = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"].ToString()) - Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"].ToString());

                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CustomerBalance"] =  total;

                        if (total < 0)
                        {
                            if (UserInfo.Language == iLanguage.English)
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "Debit";
                            else
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "مدين";
                        }
                        else
                        {
                            if (UserInfo.Language == iLanguage.English)
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "Cridit";
                            else
                                _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "دائن";
                        }
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["n_invoice_serial"] = (i + 1).ToString();
                    }
                    else
                    {
                        _sampleDataCustomer.NewRow();
                        _sampleDataCustomer.Rows.Add();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["AccountID"] = AccountID.ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CustomerName"] = dtCustomer.Rows[i][1].ToString();
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Balance"] = "0";
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Debit"] = "0";
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["Credit"] = "0";

                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceGold"] = "0";
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitGold"] = "0";
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditGold"] = "0";

                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["DebitDiamond"] = "0";
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["CreditDiamond"] = "0";
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceDiamond"] = "0";

                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["BalanceType"] = "";
                        _sampleDataCustomer.Rows[_sampleDataCustomer.Rows.Count - 1]["n_invoice_serial"] = (i + 1).ToString();
                    }
                }
            }
            #endregion
            gridControl1.DataSource = _sampleDataCustomer;
            TotalsAllCustomers();
            ProgressBar.Visible = false;
            ProgressBar.Value = 0;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtCostCenterID.Enabled = false;
            btnCostCenterSearch.Enabled = false;
            gridControl1.Visible = true;
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

        private void TotalsAllCustomers()
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


                decimal totalDiamond = 0;
                decimal creditDiamond = 0;
                decimal debitDiamond = 0;
                decimal sumDiamond = 0;


                DataRow row;
                for (int i = 0; i <= _sampleDataCustomer.Rows.Count - 1; i++)
                {
                    credit += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["Credit"]));
                    debit += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["Debit"]));
                    _sampleDataCustomer.Rows[i]["Balance"] = sum + (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["Credit"])) - (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["Debit"]));
                    sum = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["Balance"]);


                    creditGold += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["CreditGold"]));
                    debitGold += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["DebitGold"]));
                    _sampleDataCustomer.Rows[i]["BalanceGold"] = sumGold + (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["CreditGold"])) - (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["DebitGold"]));
                    sumGold = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["BalanceGold"]);


                    creditDiamond += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["CreditDiamond"]));
                    debitDiamond += (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["DebitDiamond"]));
                    _sampleDataCustomer.Rows[i]["BalanceDiamond"] = sumDiamond + (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["CreditDiamond"])) - (Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["DebitDiamond"]));
                    sumDiamond = Comon.ConvertToDecimalPrice(_sampleDataCustomer.Rows[i]["BalanceDiamond"]);
                }
                total = credit - debit;
                totalGold = creditGold - debitGold;
                totalDiamond = creditDiamond - debitDiamond;
                row = _sampleDataCustomer.NewRow();
                row["Debit"] = debit;
                row["Credit"] = credit;
                row["Balance"] = Math.Abs(total).ToString();
                row["DebitGold"] = debitGold;
                row["CreditGold"] = creditGold;
                row["BalanceGold"] = Math.Abs(totalGold).ToString();
                row["DebitDiamond"] = debitDiamond;
                row["CreditDiamond"] = creditDiamond;
                row["BalanceDiamond"] = Math.Abs(totalDiamond).ToString();

                row["n_invoice_serial"] = 0;

                if (total < 0)
                {
                    lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                    row["BalanceType"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");
                }
                else
                {
                    lblBalanceType.Text = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Credit" : "الرصيد حتى نهاية المدة دائن");
                    row["BalanceType"] = (UserInfo.Language.ToString() == iLanguage.English.ToString() ? "Balance until the end of the term Debit" : "الرصيد حتى نهاية المدة مدين");

                }

                _sampleDataCustomer.Rows.Add(row);
                 
                //------------------
               
                lblDebit.Text = debit.ToString();
                lblCredit.Text = credit.ToString();
                lblBalanceSum.Text = Math.Abs(total).ToString();


                lblDebitGold.Text = debitGold.ToString();
                lblCreditGold.Text = creditGold.ToString();
                lblBalanceSumGold.Text = Math.Abs(totalGold).ToString();


                lblDebitDiamond.Text = debitDiamond.ToString();
                lblCreditDiamond.Text = creditDiamond.ToString();
                lblBalanceSumDiamond.Text = Math.Abs(totalDiamond).ToString();
                btnShow.Visible = true;
            }
            catch { }

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

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try{
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
                        frm.cmbBranchesID.EditValue =Comon.cInt( cmbBranchesID.EditValue.ToString());
                        frm.cmbBranchesID.ItemIndex = cmbBranchesID.ItemIndex;
                        frm.btnShow_Click(null, null);
                }
            }
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

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
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

namespace Edex.SalesAndPurchaseObjects.Reports
{
    public partial class frmGoodReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        private string strSQL = "";
        private string where = "";
        private string FocusedControl;
        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        public frmGoodReport()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ///////////////////////////////////////////////////////
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtFromDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;

            // this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtToDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;

            strSQL = "EngName";
            if (UserInfo.Language == iLanguage.Arabic)
                strSQL = "ArbName";
            FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1");
            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
            this.txtSupplierID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSupplierID_Validating);
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);

            if (UserInfo.Language == iLanguage.English)
            {
                dgvolSn.Caption = "# ";
                dgvColInvoiceID.Caption = "Invoice NO";
                dgvColInvoiceDate.Caption = "Invoice  Date ";
                dgvColTotal.Caption = "Total ";
                dgvColVatAmount.Caption = "Total VatAmount  ";

                dgvColMethodeName.Caption = "Method Purchase";
                dgvColNet.Caption = "Net";


                dgvColDiscount.Caption = "Discount ";

              
                dgvColVatID.Caption = "Vat  ID";
                dgvColStoreName.Caption = "Stotre   Name ";
                dgvColCostCenterName.Caption = "Cost Center";
                dgvColDelgateName.Caption = "Delgate Name ";

                dgvColNotes.Caption = "Notes";
           
         
                dgvColSupplierName.Caption = "Supplier Name  ";



                btnShow.Text = btnShow.Tag.ToString();
                //  Label8.Text = btnShow.Tag.ToString();















            }





        }
        protected override void DoNew()
        {
            try
            {

                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtCostCenterID.Text = "";
                txtCostCenterID_Validating(null, null);
                txtStoreID.Text = "";
                txtStoreID_Validating(null, null);
                txtSupplierID.Text = "";
                txtSupplierID_Validating(null, null);
                txtStoreID.Enabled = true;
                txtCostCenterID.Enabled = true;
                txtSupplierID.Enabled = true;
                cmbMethodID.Enabled = true; 
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtFromInvoiceNo.Enabled = true;
                txtToInvoicNo.Enabled = true;

                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtToInvoicNo .Text = "";
                txtFromInvoiceNo.Text = "";
                cmbMethodID.ItemIndex = -1;
              

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
            if (gridView1.RowCount > 0)
            {
                btnShow.Visible = true;

                txtStoreID.Enabled = false;
                txtCostCenterID.Enabled = false;
                txtSupplierID.Enabled = false;
                cmbMethodID.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                txtFromInvoiceNo.Enabled = false;
                txtToInvoicNo.Enabled = false;

            }
            else
            {

                Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");

                btnShow.Visible = true;
                DoNew();
            }

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
                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["InvoiceID"] = dt.Rows[i]["InvoiceID"].ToString();

                            row["nvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["InvoiceDate"].ToString());
                            row["Total"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Total"]).ToString("N" + 2);
                            row["Discount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountLines"]) + Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])).ToString("N" + 2);
                            row["VatAmount"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["SumVat"]).ToString("N" + 2);
                            row["Net"] = (Comon.ConvertToDecimalPrice(row["Total"]) - Comon.ConvertToDecimalPrice(row["Discount"]) + Comon.ConvertToDecimalPrice(row["VatAmount"])).ToString("N" + 2);

                            row["MethodeName"] = dt.Rows[i]["MethodeName"];
                            row["SupplierName"] = (dt.Rows[i]["SupplierName"].ToString() != string.Empty ? dt.Rows[i]["SupplierName"] : "");
                            row["VatID"] = (dt.Rows[i]["VatID"].ToString() != string.Empty ? dt.Rows[i]["VatID"] : "");
                            row["StoreName"] = dt.Rows[i]["StorName"];
                            row["CostCenterName"] = (dt.Rows[i]["CostCenter"].ToString() != string.Empty ? dt.Rows[i]["CostCenter"] : "");
                            row["DelgateName"] = (dt.Rows[i]["DelegateName"].ToString() != string.Empty ? dt.Rows[i]["DelegateName"] : "");
                            row["Notes"] = dt.Rows[i]["Notes"].ToString();
                            _sampleData.Rows.Add(row);

                        }
                    }
                }

            }
            catch { }
        }



        string GetStrSQL()
        {

            btnShow.Visible = false;
            Application.DoEvents();

            string filter = "(.Sales_PurchaseInvoiceMaster.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Sales_PurchaseInvoiceMaster.InvoiceID >0 AND dbo.Sales_PurchaseInvoiceMaster.Cancel =0   AND";
            strSQL = "";
            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

            DataTable dt;
            // Dim dtMethodeName As DataTable
            // حسب الرقم

            if (txtFromInvoiceNo.Text != string.Empty)
                filter = filter + " .Sales_PurchaseInvoiceMaster.InvoiceID >=" + txtFromInvoiceNo.Text + " AND ";

            if (txtToInvoicNo.Text != string.Empty)
                filter = filter + " .Sales_PurchaseInvoiceMaster.InvoiceID <=" + txtToInvoicNo.Text + " AND ";

            // حسب التاريخ
            if (FromDate != 0)
                filter = filter + " .Sales_PurchaseInvoiceMaster.InvoiceDate >=" + FromDate + " AND ";

            if (ToDate != 0)
                filter = filter + " .Sales_PurchaseInvoiceMaster.InvoiceDate <=" + ToDate + " AND ";

            // '''البائع''''العميل''''التكلفة''''المستودع
            if (txtStoreID.Text != string.Empty)
                filter = filter + " .Sales_PurchaseInvoiceMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

            if (txtCostCenterID.Text != string.Empty)
                filter = filter + " .Sales_PurchaseInvoiceMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

            if (txtSupplierID.Text != string.Empty)
                filter = filter + " .Sales_PurchaseInvoiceMaster.SupplierID  =" + Comon.cLong(Lip.GetValue(txtSupplierAccount())) + "  AND ";
            if (cmbMethodID.Text != string.Empty)
                filter = filter + " Sales_PurchaseInvoiceMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
            // '''''''''''''
            filter = filter.Remove(filter.Length - 4, 4);

            strSQL = " SELECT dbo.Sales_PurchaseInvoiceMaster.AdditionaAmountTotal As SumVat, dbo.Sales_PurchaseInvoiceMaster.InvoiceID, dbo.Sales_PurchaseInvoiceMaster.BranchID, dbo.Sales_PurchaseInvoiceMaster.DiscountOnTotal,"
            + " dbo.Sales_PurchaseInvoiceMaster.InvoiceDate, SUM(dbo.Sales_PurchaseInvoiceDetails.QTY * dbo.Sales_PurchaseInvoiceDetails.CostPrice) AS total, "
            + " Sum(Sales_PurchaseInvoiceDetails.Discount) As DiscountLines , dbo.Stc_Stores.ArbName AS storName, dbo.Sales_PurchaseInvoiceMaster.Notes, "
            + " dbo.Sales_PurchasesDelegate.ArbName AS DelegateName, dbo.Sales_Suppliers.VatID, dbo.Sales_Suppliers.ArbName AS SupplierName, dbo.Sales_PurchaseMethods.ArbName AS MethodeName,"
            + " dbo.Acc_CostCenters.ArbName AS CostCenter FROM dbo.Sales_PurchaseInvoiceMaster INNER JOIN dbo.Sales_PurchaseInvoiceDetails ON dbo.Sales_PurchaseInvoiceMaster.InvoiceID"
            + " = dbo.Sales_PurchaseInvoiceDetails.InvoiceID AND dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_PurchaseInvoiceDetails.BranchID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Sales_PurchaseInvoiceMaster.CostCenterID = "
            + " dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Sales_Suppliers ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Sales_Suppliers.BranchID AND "
            + " dbo.Sales_PurchaseInvoiceMaster.SupplierID = dbo.Sales_Suppliers.AccountID LEFT OUTER JOIN dbo.Sales_PurchasesDelegate ON dbo.Sales_PurchaseInvoiceMaster.BranchID"
            + " = dbo.Sales_PurchasesDelegate.BranchID AND dbo.Sales_PurchaseInvoiceMaster.DelegateID = dbo.Sales_PurchasesDelegate.DelegateID LEFT OUTER JOIN"
            + " dbo.Stc_Stores ON dbo.Sales_PurchaseInvoiceMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Sales_PurchaseInvoiceMaster.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN"
            + " dbo.Sales_PurchaseMethods ON dbo.Sales_PurchaseInvoiceMaster.MethodeID = dbo.Sales_PurchaseMethods.MethodID where " + filter + " GROUP BY dbo.Sales_Suppliers.VatID , dbo.Sales_PurchaseInvoiceMaster.InvoiceID,dbo.Sales_PurchaseInvoiceMaster.AdditionaAmountTotal ,"
            + " dbo.Sales_PurchaseInvoiceMaster.BranchID,dbo.Sales_PurchaseInvoiceMaster.DiscountOnTotal, dbo.Sales_PurchaseInvoiceMaster.InvoiceDate, "
            + " dbo.Stc_Stores.ArbName, dbo.Sales_PurchaseInvoiceMaster.Notes, dbo.Sales_PurchasesDelegate.ArbName, dbo.Sales_Suppliers.ArbName, "
            + " dbo.Sales_PurchaseMethods.ArbName, dbo.Acc_CostCenters.ArbName ";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            return  strSQL ;


        }
        private void frmPurchasesInvoiceReport_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("nvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Total", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Discount", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("VatAmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Net", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("MethodeName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("SupplierName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VatID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DelgateName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("F1", typeof(string)));

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

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }
        ////////////// print_COde//////////////////////////////////
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
                rptForm.Parameters["FromInvoiceNo"].Value = txtFromInvoiceNo.Text.Trim().ToString();
                rptForm.Parameters["ToInvoiceNo"].Value = txtToInvoicNo.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["SupplierName"].Value = lblSupplierName.Text.Trim().ToString();
                rptForm.Parameters["PurchasesMethod"].Value = cmbMethodID.Text.Trim().ToString();



                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();

                /********************** Details ****************************/
                var dataTable = new dsReports.rptPurchasesInvoiceReportDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["InvoiceID"] = gridView1.GetRowCellValue(i, "InvoiceID").ToString();
                    row["nvoiceDate"] = gridView1.GetRowCellValue(i, "nvoiceDate").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["VatAmount"] = gridView1.GetRowCellValue(i, "VatAmount").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["MethodeName"] = gridView1.GetRowCellValue(i, "MethodeName").ToString();
                    row["SupplierName"] = gridView1.GetRowCellValue(i, "SupplierName").ToString();
                    row["VatID"] = gridView1.GetRowCellValue(i, "VatID").ToString();
                    row["StoreName"] = gridView1.GetRowCellValue(i, "StoreName").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterName").ToString();
                    row["DelgateName"] = gridView1.GetRowCellValue(i, "DelgateName").ToString();
                    row["Notes"] = gridView1.GetRowCellValue(i, "Notes").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptPurchasesInvoiceReport";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeaderLand();
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

        /// <summary>
        /// //////////////////////////////////////////////////////////
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where BranchID=" + UserInfo.BRANCHID;

            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    //  PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسم الـمـســتـودع","رقم الـمـســتـودع");
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسـم الـمـســتـودع", "رقم الـمـســتـودع");
                else
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "Store Name", "Store ID");
            }



            else if (FocusedControl.Trim() == txtSupplierID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtSupplierID, lblSupplierName, "SupplierID", "اسـم الـمــــــــــورد", "رقم الـمــــــــــورد");
                else
                    PrepareSearchQuery.Search(txtSupplierID, lblSupplierName, "SupplierID", "Supplier Name", "Supplier ID");
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة");
                else
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID");
            }










        }
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "    as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
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
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "    as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {


            try
            {
                string strSql;
                DataTable dt;
                string PrimaryName = "EngName";
                if (UserInfo.Language == iLanguage.Arabic)
                    PrimaryName = "ArbName";
                //if (txtSupplierID.Text != string.Empty && txtSupplierID.Text != "0")
                //{

                    strSQL = "SELECT " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as SupplierName FROM Sales_Suppliers Where  Cancel =0 And  SupplierID =" + txtSupplierID.Text + " And BranchID =" + UserInfo.BRANCHID;
                    CSearch.ControlValidating(txtSupplierID, lblSupplierName, strSQL);


                //}
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        string txtSupplierAccount()
        {


            try
            {

                if (txtSupplierID.Text != string.Empty && txtSupplierID.Text != "0")
                {

                    strSQL = "SELECT AccountID FROM Sales_Suppliers Where  Cancel =0 And  SupplierID =" + txtSupplierID.Text + " And BranchID =" + UserInfo.BRANCHID;


                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }

        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
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
        protected override void DoSearch()
        {
            try
            {
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmPurchasesInvoiceReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }

        /////////////////////////////

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            frmPurchaseInvoice frm = new frmPurchaseInvoice();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("InvoiceID").ToString()) + 1, 8);
            }
            else
                frm.Dispose();
        }
        ///////////////////////////////
    }
}

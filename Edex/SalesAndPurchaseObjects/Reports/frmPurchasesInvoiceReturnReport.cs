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
using Edex.SalesAndSaleObjects.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Reports
{
    public partial class frmPurchasesInvoiceReturnReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        private string strSQL = "";
        private string where = "";
        string PrimaryName = "EngName";

        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        string FocusedControl;
        public frmPurchasesInvoiceReturnReport()
        {
            try{
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

            // this.txtToDate.EditValue = DateTime.Now;
     //       strSQL = "ArbName";
            if (UserInfo.Language == iLanguage.Arabic)
                PrimaryName = "ArbName";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
            FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", PrimaryName, "", "1=1");
            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
            this.txtSupplierID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSupplierID_Validating);
            this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);

            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;

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
            catch { }

        }

        private void frmPurchasesInvoiceReturnReport_Load(object sender, EventArgs e)
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
            _sampleData.Columns.Add(new DataColumn("DelegateName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VatID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DelgateName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("F1", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetPaid", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TotalGold", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("GoldUsing", typeof(string)));
            

            Validations.DoRoolBackRipon(this, ribbonControl1);
            Validations.EnabledControl(this, false);

        }
        private void PurchaseInvoice()
        {
            try
            {
                decimal netSum = 0;
                decimal netCashSum = 0;
                decimal caschPaidWithNet = 0;
                decimal cash = 0;
                decimal future = 0;
                decimal check1 = 0;
                decimal total = 0;
                decimal totalGold = 0;

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
                            row["DelegateName"] = dt.Rows[i]["DelegateName"];

                            row["MethodeName"] = dt.Rows[i]["MethodeName"];
                            row["SupplierName"] = (dt.Rows[i]["SupplierName"].ToString() != string.Empty ? dt.Rows[i]["SupplierName"] : "");
                            //row["VatID"] = (dt.Rows[i]["VatID"].ToString() != string.Empty ? dt.Rows[i]["VatID"] : "");
                            row["StoreName"] = dt.Rows[i]["StorName"];
                            row["CostCenterName"] = (dt.Rows[i]["CostCenterName"].ToString() != string.Empty ? dt.Rows[i]["CostCenterName"] : "");
                            row["DelgateName"] = (dt.Rows[i]["DelegateName"].ToString() != string.Empty ? dt.Rows[i]["DelegateName"] : "");
                            row["Notes"] = dt.Rows[i]["Notes"].ToString();
                            row["TotalGold"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["TotalGold"].ToString());
                            row["GoldUsing"] = dt.Rows[i]["GoldUsing"].ToString();
                             


                            total += Comon.ConvertToDecimalPrice(row["Net"]);
                            totalGold += Comon.ConvertToDecimalPrice(row["F1"]);

                            switch (Comon.cInt(dt.Rows[i]["MethodeID"].ToString()))
                            {

                                case (1):
                                    cash += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["NetPaid"] = "-";
                                    break;
                                case (2):
                                    future += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["NetPaid"] = "-";
                                    break;
                                case (3):
                                    netSum += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["Notes"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (4):
                                    check1 += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (5):
                                    netCashSum += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    caschPaidWithNet += Comon.ConvertToDecimalPrice(dt.Rows[i]["NetAmount"]);
                                    row["Notes"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                            }
                            _sampleData.Rows.Add(row);

                        }
                    }
                    lblCash.Text = cash.ToString();
                    lblNet2.Text = netSum.ToString();
                    lblFuture.Text = future.ToString();
                    lblCashNet.Text = netCashSum.ToString();
                    lblCash1.Text = (netCashSum - caschPaidWithNet).ToString();
                    lblNet1.Text = caschPaidWithNet.ToString();
                    lblCheck.Text = check1.ToString();
                    lblTotal.Text = total.ToString();
                    lblTotalGold.Text = totalGold.ToString();
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


        string GetStrSQL()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();

                string filter = "(.Sales_PurchaseInvoiceReturnMaster.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID >0 AND dbo.Sales_PurchaseInvoiceReturnMaster.Cancel =0   AND";
              
                if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                    filter = "(.Sales_PurchaseInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID >0 AND dbo.Sales_PurchaseInvoiceReturnMaster.Cancel =0   AND";
               
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

                DataTable dt;
                 
                // حسب الرقم

                if (txtFromInvoiceNo.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.InvoiceID >=" + txtFromInvoiceNo.Text + " AND ";

                if (txtToInvoicNo.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.InvoiceID <=" + txtToInvoicNo.Text + " AND ";

                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.InvoiceDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.InvoiceDate <=" + ToDate + " AND ";

                // '''البائع''''العميل''''التكلفة''''المستودع
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.StoreID  =" + Comon.cLong(txtStoreID.Text) + "  AND ";

                if (txtCostCenterID.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

                if (txtSupplierID.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.SupplierID  =" + Comon.cLong(Lip.GetValue(txtSupplierAccount())) + "  AND ";

                if (txtDelegateID.Text != string.Empty)
                    filter = filter + " .Sales_PurchaseInvoiceReturnMaster.DelegateID  =" + Comon.cInt(txtDelegateID.Text) + "  AND ";
                if (cmbMethodID.Text != string.Empty)
                    filter = filter + " Sales_PurchaseInvoiceReturnMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                // '''''''''''''
                filter = filter.Remove(filter.Length - 4, 4);
                //  , غير موجود في جدول مردود المشتريات

                strSQL = " SELECT Sales_PurchaseInvoiceReturnMaster.NetProcessID,Sales_PurchaseInvoiceReturnMaster.GoldUsing,dbo.Sales_PurchaseInvoiceReturnMaster.MethodeID, Sales_PurchaseInvoiceReturnMaster.NetAmount,  dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID,dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal as SumVat ,dbo.Sales_PurchaseInvoiceReturnMaster.BranchID, dbo.Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal,"
             + " dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate,SUM(dbo.Sales_PurchaseInvoiceReturnDetails.QTY) as TotalGold, SUM(dbo.Sales_PurchaseInvoiceReturnDetails.QTY * dbo.Sales_PurchaseInvoiceReturnDetails.CostPrice) AS total, "
             + "Sum(Sales_PurchaseInvoiceReturnDetails.DIAMOND_W) As DIAMOND_W ,Sum(Sales_PurchaseInvoiceReturnDetails.STONE_W) As STONE_W ,Sum(Sales_PurchaseInvoiceReturnDetails.BAGET_W) As BAGET_W , Sum(Sales_PurchaseInvoiceReturnDetails.Equivalen) As Equivalen , Sum(Sales_PurchaseInvoiceReturnDetails.Discount) As DiscountLines , dbo.Stc_Stores.ArbName AS storName, dbo.Sales_PurchaseInvoiceReturnMaster.Notes, "
             + " dbo.Sales_PurchasesDelegate.ArbName AS DelegateName, dbo.Sales_Suppliers.ArbName AS SupplierName, dbo.Sales_PurchaseMethods.ArbName AS MethodeName,"
             + " dbo.Acc_CostCenters.ArbName AS CostCenterName FROM dbo.Sales_PurchaseInvoiceReturnMaster INNER JOIN dbo.Sales_PurchaseInvoiceReturnDetails ON dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID"
             + " = dbo.Sales_PurchaseInvoiceReturnDetails.InvoiceID AND dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Sales_PurchaseInvoiceReturnDetails.BranchID LEFT OUTER JOIN"
             + " dbo.Acc_CostCenters ON dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Sales_PurchaseInvoiceReturnMaster.CostCenterID = "
             + " dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Sales_Suppliers ON dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Sales_Suppliers.BranchID AND "
             + " dbo.Sales_PurchaseInvoiceReturnMaster.SupplierID = dbo.Sales_Suppliers.AccountID LEFT OUTER JOIN dbo.Sales_PurchasesDelegate ON dbo.Sales_PurchaseInvoiceReturnMaster.BranchID"
             + " = dbo.Sales_PurchasesDelegate.BranchID AND dbo.Sales_PurchaseInvoiceReturnMaster.DelegateID = dbo.Sales_PurchasesDelegate.DelegateID LEFT OUTER JOIN"
             + " dbo.Stc_Stores ON dbo.Sales_PurchaseInvoiceReturnMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Sales_PurchaseInvoiceReturnMaster.StoreID = dbo.Stc_Stores.AccountID LEFT OUTER JOIN"
             + " dbo.Sales_PurchaseMethods ON dbo.Sales_PurchaseInvoiceReturnMaster.MethodeID = dbo.Sales_PurchaseMethods.MethodID where " + filter + " GROUP BY Sales_PurchaseInvoiceReturnMaster.NetProcessID,Sales_PurchaseInvoiceReturnMaster.GoldUsing,dbo.Sales_PurchaseInvoiceReturnMaster.MethodeID, Sales_PurchaseInvoiceReturnMaster.NetAmount, dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceID,"
             + " dbo.Sales_PurchaseInvoiceReturnMaster.BranchID,dbo.Sales_PurchaseInvoiceReturnMaster.DiscountOnTotal, dbo.Sales_PurchaseInvoiceReturnMaster.InvoiceDate, "
             + " dbo.Stc_Stores.ArbName, dbo.Sales_PurchaseInvoiceReturnMaster.Notes, dbo.Sales_PurchasesDelegate.ArbName, dbo.Sales_Suppliers.ArbName, "
             + " dbo.Sales_PurchaseMethods.ArbName, dbo.Acc_CostCenters.ArbName,dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmountTotal ";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return Lip.ConvertStrSQLLanguage(strSQL, iLanguage.English.ToString());

        }
        protected override void DoAddFrom()
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
                txtDelegateID.Text = "";
                txtDelegateID_Validating(null, null);
                txtStoreID.Enabled = true;
                txtCostCenterID.Enabled = true;
                txtSupplierID.Enabled = true;
                cmbMethodID.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtDelegateID.Enabled = true;
                txtFromInvoiceNo.Enabled = true;
                txtToInvoicNo.Enabled = true;

                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtToInvoicNo.Text = "";
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
          //  gridView1.Columns["GoldUsing"].Visible = false;
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
                txtDelegateID.Enabled = false ;
            }
            else
            {

                Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");

                btnShow.Visible = true;
                DoNew();
            }

        }
        ////////////// print_COde//////////////////////////////////
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
               ReportName = "rptPurchasesInvoiceReturnReport";
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
                rptForm.Parameters["CostCenter"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["SuppLierName"].Value = lblSupplierName.Text.Trim().ToString();
                // rptForm.Parameters["DelgeateName"].Value = lblDelgeateName.Text.Trim().ToString();
                rptForm.Parameters["MethodPurches"].Value = cmbMethodID.Text.Trim().ToString();
                rptForm.Parameters["CashSum"].Value = lblCash.Text.Trim().ToString();
                rptForm.Parameters["FutureSum"].Value = lblFuture.Text.Trim().ToString();
                rptForm.Parameters["NetSum"].Value = lblNet2.Text.Trim().ToString();
                rptForm.Parameters["CashNetSum"].Value = lblCashNet.Text.Trim().ToString();
                rptForm.Parameters["Net1"].Value = lblNet1.Text.Trim().ToString();
                rptForm.Parameters["Cash1"].Value = lblCash1.Text.Trim().ToString();
                rptForm.Parameters["CheckSum"].Value = lblCheck.Text.Trim().ToString();


                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptPurchasesInvoiceReturnReportDataTable();

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
                    row["TotalGold"] = gridView1.GetRowCellValue(i, "TotalGold").ToString();
                  
                    row["DIAMOND_W"] = gridView1.GetRowCellValue(i, "DIAMOND_W").ToString();
                    row["STONE_W"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();
                    row["BAGET_W"] = gridView1.GetRowCellValue(i, "BAGET_W").ToString();



                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptPurchasesInvoiceReturnReport";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeaderLand();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                SplashScreenManager.CloseForm(false);
                //strSQL = ("SELECT ShowReportInReportViewer  FROM UserReportsPermissions" + (" Where BranchID =" + (MySession.GlobalBranchID + (" And UserID=" + (UserInfo.ID.ToString() + (" And ReportName='" + (ReportName + "'")))))));
                //DataTable dReprt = new DataTable();
                //dReprt = Lip.SelectRecord(strSQL);
                //ShowReportInReportViewer = (Comon.cInt(dReprt.Rows[0]["ShowReportInReportViewer"].ToString()) == 1 ? true : false);
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

        private void frmPurchasesInvoiceReturnReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////
        /// 



        /**********************************/
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                strSQL = "SELECT  "  + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "     as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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

                //if (txtSupplierID.Text != string.Empty && txtSupplierID.Text != "0")
                //{

                    strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "    as SupplierName FROM Sales_Suppliers Where  Cancel =0 And  SupplierID =" + Comon.cInt(txtSupplierID.Text) + " And BranchID =" + UserInfo.BRANCHID;
                    CSearch.ControlValidating(txtSupplierID, lblSupplierName, strSQL);


                //}
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "    as DelegateName FROM Sales_PurchasesDelegate WHERE DelegateID =" + Comon.cInt(txtDelegateID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtDelegateID, lblDelgeateName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }



        /*********************************/
        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where BranchID=" + UserInfo.BRANCHID;

            FocusedControl = GetIndexFocusedControl();
           if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    //  PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسم الـمـســتـودع","رقم الـمـســتـودع");
                    PrepareSearchQuery.Search(txtDelegateID, lblDelgeateName, "PurchaseDelegateID", "اسـم المـندوب", "رقم المـندوب");
                else
                    PrepareSearchQuery.Search(txtDelegateID, lblDelgeateName, "PurchaseDelegateID", "Delegate Name", "Delegate ID");




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
            catch { }


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
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try{
            GridView view = sender as GridView;
          
            if (Comon.cInt(view.GetFocusedRowCellValue("GoldUsing").ToString()) == 3)
            {
                frmCashierPurchaseReturnDaimond frm = new frmCashierPurchaseReturnDaimond();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.ReadRecord(Comon.cLong(view.GetFocusedRowCellValue("InvoiceID").ToString()));
                }
                else
                    frm.Dispose();
            }
            else
            {
                frmCashierPurchaseReturnGold frm = new frmCashierPurchaseReturnGold();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.ReadRecord(Comon.cLong(view.GetFocusedRowCellValue("InvoiceID").ToString()));
                }
                else
                    frm.Dispose();
            }
            }
            catch (Exception ex){
                Messages.MsgError("Error ", "خطأ إنتقال "+ex.Message);
            }
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

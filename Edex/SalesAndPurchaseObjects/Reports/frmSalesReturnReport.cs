/////////////////////////////////////////////////////////////////////////////////////////////
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
    public partial class frmSalesReturnReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string FocusedControl;

        private string strSQL = "";
        private string where = "";
        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        public frmSalesReturnReport()
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

            // this.txtToDate.EditValue = DateTime.Now;

            strSQL = "EngName";
            if (UserInfo.Language == iLanguage.Arabic)
                strSQL = "ArbName";
            FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1");

            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
            this.txtSellerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSellerID_Validating);
            // this.txtSalesDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
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

                dgvColMethodeName.Caption = "Method Sale";
                dgvColNet.Caption = "Net";




                dgvColSellerName.Caption = "Seller Name ";
                dgvColVatID.Caption = "Vat  ID";
                dgvColStoreName.Caption = "Stotre   Name ";
                dgvColCostCenterName.Caption = "Cost Center";


                dgvColDiscount.Caption = "Discount ";
                dgvColNotes.Caption = "Notes";
           
                dgvCustomerName.Caption = "Customer Name  ";



                btnShow.Text = btnShow.Tag.ToString();
                //  Label8.Text = btnShow.Tag.ToString();
                 

            }



            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = UserInfo.BRANCHID;


        }


        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as CustomerName FROM Sales_Customers WHERE CustomerID=" + txtCustomerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCustomerID, lblCustomerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtSellerID_Validating(object sender, CancelEventArgs e)
        {
            try
            { 
                strSQL = "SELECT " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as SellerName FROM Sales_Sellers WHERE SellerID=" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtSellerID, lblSellerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void SalesInvoice()
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
                            row["Discount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountLines"]).ToString("N" + 2));
                            row["VatAmount"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["SumVat"]).ToString("N" + 2);
                            row["Net"] = (Comon.ConvertToDecimalPrice(row["Total"]) - Comon.ConvertToDecimalPrice(row["Discount"]) + Comon.ConvertToDecimalPrice(row["VatAmount"])).ToString("N" + 2);
                            row["SellerName"] = dt.Rows[i]["SellerName"];
                            row["MethodeName"] = dt.Rows[i]["MethodeName"];
                            row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                            row["VatID"] = (dt.Rows[i]["VatID"].ToString() != string.Empty ? dt.Rows[i]["VatID"] : "");
                            row["StoreName"] = dt.Rows[i]["StorName"];
                            row["CostCenterName"] = (dt.Rows[i]["CostCenterName"].ToString() != string.Empty ? dt.Rows[i]["CostCenterName"] : "");
                            row["DelgateName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                            row["TotalWhight"] = dt.Rows[i]["TotalWhight"];

                            row["DIAMOND_W"] = dt.Rows[i]["DIAMOND_W"];
                            row["STONE_W"] = dt.Rows[i]["STONE_W"];
                            row["BAGET_W"] = dt.Rows[i]["BAGET_W"];

                             



                            switch (Comon.cInt(dt.Rows[i]["MethodeID"].ToString()))
                            {

                                case (1):
                                    cash += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["CustomerName"] = (dt.Rows[i]["CustomerName1"].ToString() != string.Empty ? dt.Rows[i]["CustomerName1"] : "");
                                    row["NetPaid"] = "-";
                                    break;
                                case (2):
                                    future += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["NetPaid"] = "-";
                                    break;
                                case (3):
                                    netSum += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["CustomerName"] = (dt.Rows[i]["CustomerName1"].ToString() != string.Empty ? dt.Rows[i]["CustomerName1"] : "");
                                    row["Notes"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (4):
                                    check1 += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["CustomerName"] = (dt.Rows[i]["CustomerName1"].ToString() != string.Empty ? dt.Rows[i]["CustomerName1"] : "");
                                    // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (5):
                                    netCashSum += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    caschPaidWithNet += Comon.ConvertToDecimalPrice(dt.Rows[i]["NetAmount"]);
                                    row["CustomerName"] = (dt.Rows[i]["CustomerName1"].ToString() != string.Empty ? dt.Rows[i]["CustomerName1"] : "");
                                    row["Notes"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                            }
                            _sampleData.Rows.Add(row);

                        }
                    }
                    lblCash.Text = cash.ToString();
                    lblNet.Text = netSum.ToString();
                    lblFuture.Text = future.ToString();
                    lblCashNet.Text = netCashSum.ToString();
                    lblCash1.Text = (netCashSum - caschPaidWithNet).ToString();
                    lblNet1.Text = caschPaidWithNet.ToString();
                    lblCheck.Text = check1.ToString();
                    lblTotal.Text = total.ToString();
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

        string txtCustomerAccount()
        {
            try
            {
                strSQL = "SELECT AccountID  FROM Sales_Customers WHERE CustomerID=" + txtCustomerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);

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

                string filter = "(.Sales_SalesInvoiceReturnMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_SalesInvoiceReturnMaster.InvoiceID >0 AND dbo.Sales_SalesInvoiceReturnMaster.Cancel =0   AND";

                 
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

                DataTable dt;
                // Dim dtMethodeName As DataTable
                // حسب الرقم

                if (txtFromInvoiceNo.Text != string.Empty)
                    filter = filter + " Sales_SalesInvoiceReturnMaster.InvoiceID >=" + txtFromInvoiceNo.Text + " AND ";

                if (txtToInvoicNo.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.InvoiceID <=" + txtToInvoicNo.Text + " AND ";

                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.InvoiceDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.InvoiceDate <=" + ToDate + " AND ";

                // '''البائع''''العميل''''التكلفة''''المستودع
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                if (txtCostCenterID.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

                if (txtCustomerID.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.CustomerID  =" + Comon.cLong(Lip.GetValue(txtCustomerAccount())) + "  AND ";

                if (txtSellerID.Text != string.Empty)
                    filter = filter + " .Sales_SalesInvoiceReturnMaster.DelegateID  =" + Comon.cInt(txtSellerID.Text) + "  AND ";
                if (cmbMethodID.Text != string.Empty)
                    filter = filter + " Sales_SalesInvoiceReturnMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                // '''''''''''''
                filter = filter.Remove(filter.Length - 4, 4);
                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal,dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmmountTotal AS SumVat, غير موجود في جدول مردود المشتريات
                           strSQL = "SELECT   Sales_SalesInvoiceReturnMaster.NetProcessID,dbo.Sales_SalesInvoiceReturnMaster.MethodeID, Sales_SalesInvoiceReturnMaster.NetAmount,Sales_SalesInvoiceReturnMaster.CustomerName AS CustomerName1, " 
                         + " sum(dbo.Sales_SalesInvoiceReturnDetails.DIAMOND_W) AS DIAMOND_W   , sum(dbo.Sales_SalesInvoiceReturnDetails.STONE_W) AS STONE_W   , sum(dbo.Sales_SalesInvoiceReturnDetails.BAGET_W) AS BAGET_W   , sum(dbo.Sales_SalesInvoiceReturnDetails.Qty) AS TotalWhight   ,  (Sum(Sales_SalesInvoiceReturnDetails.Discount) + Sales_SalesInvoiceReturnMaster.DiscountOnTotal)  As DiscountLines,Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal AS SumVat, dbo.Sales_SalesInvoiceReturnMaster.InvoiceID, dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate, SUM(dbo.Sales_SalesInvoiceReturnDetails.Total) AS Total,"
                          + " dbo.Sales_SalesInvoiceReturnMaster.Notes,  dbo.Stc_Stores.ArbName AS Storname, dbo.Sales_SalesMethodes.ArbName AS MethodeName, dbo.Sales_Customers.ArbName AS CustomerName,"
                          + " dbo.Sales_SalesDelegate.ArbName AS DelegateName,dbo.Sales_Sellers.ArbName AS SellerName, dbo.Sales_Customers.VatID,dbo.Acc_CostCenters.ArbName AS CostCenterName, dbo.Sales_SalesInvoiceReturnMaster.CostCenterID FROM dbo.Sales_SalesInvoiceReturnDetails INNER JOIN"
                          + " dbo.Sales_SalesInvoiceReturnMaster ON dbo.Sales_SalesInvoiceReturnDetails.InvoiceID = dbo.Sales_SalesInvoiceReturnMaster.InvoiceID AND dbo.Sales_SalesInvoiceReturnDetails.BranchID ="
                          + " dbo.Sales_SalesInvoiceReturnMaster.BranchID LEFT OUTER JOIN dbo.Acc_CostCenters ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Acc_CostCenters.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.CostCenterID"
                          + " = dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN dbo.Sales_Customers ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Sales_Customers.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.CustomerID = "
                          + " dbo.Sales_Customers.AccountID LEFT OUTER JOIN dbo.Sales_Sellers ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Sales_Sellers.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.SellerID = "
                          + " dbo.Sales_Sellers.SellerID LEFT OUTER JOIN dbo.Sales_SalesDelegate ON dbo.Sales_SalesInvoiceReturnMaster.BranchID"
                          + " = dbo.Sales_SalesDelegate.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.DelegateID = dbo.Sales_SalesDelegate.DelegateID  LEFT OUTER JOIN dbo.Stc_Stores ON dbo.Sales_SalesInvoiceReturnMaster.BranchID = dbo.Stc_Stores.BranchID AND dbo.Sales_SalesInvoiceReturnMaster.StoreID = "
                          + " dbo.Stc_Stores.StoreID LEFT OUTER JOIN dbo.Sales_SalesMethodes ON dbo.Sales_SalesInvoiceReturnMaster.MethodeID = dbo.Sales_SalesMethodes.MethodID where " + filter
                          + "  GROUP BY  Sales_SalesInvoiceReturnMaster.NetProcessID,dbo.Sales_SalesInvoiceReturnMaster.MethodeID, Sales_SalesInvoiceReturnMaster.NetAmount, Sales_SalesInvoiceReturnMaster.CustomerName, Sales_SalesInvoiceReturnMaster.ComputerInfo , Sales_SalesInvoiceReturnMaster.DiscountOnTotal , dbo.Sales_SalesInvoiceReturnMaster.InvoiceID, dbo.Sales_SalesInvoiceReturnMaster.InvoiceDate, dbo.Sales_SalesInvoiceReturnMaster.Notes, dbo.Stc_Stores.ArbName, "
                          + "  dbo.Sales_SalesMethodes.ArbName, dbo.Sales_Customers.VatID,dbo.Sales_Customers.ArbName, dbo.Sales_Sellers.ArbName, dbo.Acc_CostCenters.ArbName, "
                          + "  dbo.Sales_SalesInvoiceReturnMaster.CostCenterID,dbo.Sales_SalesDelegate.ArbName , Sales_SalesInvoiceReturnMaster.AdditionaAmountTotal,dbo.Sales_SalesInvoiceReturnMaster.Cancel HAVING      (dbo.Sales_SalesInvoiceReturnMaster.Cancel = 0)";
                         Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return  strSQL ;

        }

        private void frmSalesReturnReport_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("nvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Total", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Discount", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("VatAmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Net", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("MethodeName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("SellerName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VatID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DelgateName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("F1", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetPaid", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TotalWhight", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("DIAMOND_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("STONE_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("BAGET_W", typeof(string)));

             

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
                txtCustomerID.Text = "";
                txtCustomerID_Validating(null, null);
                txtSellerID.Text = "";
                txtSellerID_Validating(null, null);
                txtStoreID.Enabled = true;
                txtCostCenterID.Enabled = true;
                txtSellerID.Enabled = true;
                txtCustomerID.Enabled = true;
                cmbMethodID.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
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
        public void btnShow_Click(object sender, EventArgs e)
        {
            SalesInvoice();
            gridControl1.DataSource = _sampleData;
            if (gridView1.RowCount > 0)
            {
                btnShow.Visible = true;
                txtStoreID.Enabled = false;
                txtCostCenterID.Enabled = false;
                txtSellerID.Enabled = false;
                txtCustomerID.Enabled = false;
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
        public void setDateFrom()
        {
            this.txtFromDate.EditValue = DateTime.Now;
            this.txtToDate.EditValue = DateTime.Now;
        }
        public decimal GetBalance()
        {
            decimal SumVat = 0;
            decimal total = 0;
            decimal Net = 0;
            decimal Discount = 0;
            // return Comon.ConvertToDecimalPrice(lblNetBalance.Text);
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                total += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                Net += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                Discount += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                SumVat += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "VatAmount").ToString());
            }
            return Comon.ConvertToDecimalPrice(total - Discount + SumVat);
        }
        #region Function
        ////////////// print_COde//////////////////////////////////
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                ReportName = "rptSalesReturnReport";
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
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["SellerName"].Value = lblSellerName.Text.Trim().ToString();
                rptForm.Parameters["MethodName"].Value = cmbMethodID.Text.Trim().ToString();
                rptForm.Parameters["CashSum"].Value = lblCash.Text.Trim().ToString();
                rptForm.Parameters["FutureSum"].Value = lblFuture.Text.Trim().ToString();
                rptForm.Parameters["NetSum"].Value = lblNet.Text.Trim().ToString();
                rptForm.Parameters["CashNetSum"].Value = lblCashNet.Text.Trim().ToString();
                rptForm.Parameters["Net1"].Value = lblNet1.Text.Trim().ToString();
                rptForm.Parameters["Cash1"].Value = lblCash1.Text.Trim().ToString();
                rptForm.Parameters["CheckSum"].Value = lblCheck.Text.Trim().ToString();

                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceReportDataTable();

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
                    row["Profit"] = gridView1.GetRowCellValue(i, "TotalWhight").ToString();
                    row["MethodeName"] = gridView1.GetRowCellValue(i, "MethodeName").ToString();
                    row["CustomerName"] = gridView1.GetRowCellValue(i, "CustomerName").ToString();
                    row["SellerName"] = gridView1.GetRowCellValue(i, "SellerName").ToString();
                    row["VatID"] = gridView1.GetRowCellValue(i, "VatID").ToString();
                    row["StoreName"] = gridView1.GetRowCellValue(i, "StoreName").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterName").ToString();
                    row["DelgateName"] = gridView1.GetRowCellValue(i, "DelgateName").ToString();
                    row["Notes"] = gridView1.GetRowCellValue(i, "Notes").ToString();
                   
                    row["DIAMOND_W"] = gridView1.GetRowCellValue(i, "DIAMOND_W").ToString();
                    row["STONE_W"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();
                    row["BAGET_W"] = gridView1.GetRowCellValue(i, "BAGET_W").ToString();


                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptSalesInvoiceReport";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeaderLand();
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
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();


            if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    //  PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسم الـمـســتـودع","رقم الـمـســتـودع");
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسـم الـمـســتـودع", "رقم الـمـســتـودع");
                else
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "Store Name", "Store ID");
            }


            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة");
                else
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID");
            }

            else if (FocusedControl.Trim() == txtSellerID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtSellerID, lblSellerName, "SellerID", "اسـم البائع", "رقم البائع");
                else
                    PrepareSearchQuery.Search(txtSellerID, lblSellerName, "SellerID", "Seller  Name", "Seller ID");
            }

            else if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "CustomerID", "اسـم الــعـــمـــيــل", "رقم الــعـــمـــيــل");
                else
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "CustomerID", "Customer Name", "Customer ID");
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


        #endregion
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

        private void frmSalesReturnReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try{
            GridView view = sender as GridView;
            frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.ReadRecord(Comon.cLong(view.GetFocusedRowCellValue("InvoiceID").ToString()));
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


using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;

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
    public partial class frmCloseCashierReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string FocusedControl;

        private string strSQL = "";
        private string where = "";
        DataTable dt = new DataTable();
        DataTable dtLocation = new DataTable();
        DataTable dtPrice = new DataTable();
        public DataTable _sampleData = new DataTable();
        public frmCloseCashierReport()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            //gridView1.OptionsBehavior.ReadOnly = true;
            //gridView1.OptionsBehavior.Editable = false;
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
            ///////////////////////////////////////////////////////
            this.txtFromCloseDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromCloseDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromCloseDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromCloseDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromCloseDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromCloseDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtFromCloseDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            // this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToCloseDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToCloseDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToCloseDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToCloseDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToCloseDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToCloseDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtToCloseDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            // this.txtToDate.EditValue = DateTime.Now;
            strSQL = "EngName";
            if (UserInfo.Language == iLanguage.Arabic)
                strSQL = "ArbName";

            FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1");

            /////////////////////////////////////////////
            ///////////////////////////
            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
            this.txtSellerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSellerID_Validating);
            this.txtSalesDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;




            if (UserInfo.Language == iLanguage.English)
            {
                dgvolSn.Caption = "# ";
            
                dgvColInvoiceDate.Caption = "Invoice  Date ";
                dgvColTotal.Caption = "Total ";
                dgvColVatAmount.Caption = "Total VatAmount  ";

                dgvColMethodeName.Caption = "Method Sale";
                dgvColNet.Caption = "Net";


                dgvColDiscount.Caption = "Discount ";


                 dgvColSellerName.Caption = "Seller Name ";
                dgvColVatID.Caption = "Vat  ID";
             



                btnShow.Text = btnShow.Tag.ToString();
                //  Label8.Text = btnShow.Tag.ToString();















            }





            //var sr = "select Sales_CustomersAddress.ID, Sales_CustomersAddress.ArbName,Sales_CustomersAddress.Location,Sales_CustomersAddress.EngName , HR_District.TransCost  from Sales_CustomersAddress inner join HR_District on Sales_CustomersAddress.Location=HR_District.ID   where Sales_CustomersAddress.Cancel=0 ";
            // dtLocation = Lip.SelectRecord(sr);

           












        }
        #region Function
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where BranchID=" + UserInfo.BRANCHID;

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "UserID", "اسم المستخدم", "رقم المستخدم");
                else
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "UserID", "اسم المستخدم", "رقم المستخدم");
            }


            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    //  PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسم الـمـســتـودع","رقم الـمـســتـودع");
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسـم الـمـســتـودع", "رقم الـمـســتـودع");
                else
                    PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "Store Name", "Store ID");
            }


            else if (FocusedControl.Trim() == txtOldBarCode.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtOldBarCode, lblBarCodeName, "BarCodeForSalesInvoice", "اسـم الـمـادة", "البـاركـود");
                else
                    PrepareSearchQuery.Search(txtOldBarCode, lblBarCodeName, "BarCodeForSalesInvoice", "Item Name", "BarCode");
            }

            else if (FocusedControl.Trim() == txtSellerID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtSellerID, lblSellerName, "UserID", "اسم المستخدم", "رقم المستخدم");
                else
                    PrepareSearchQuery.Search(txtSellerID, lblSellerName, "UserID", "اسم المستخدم", "رقم المستخدم");
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة");
                else
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID");
            }

            else if (FocusedControl.Trim() == txtSalesDelegateID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtSalesDelegateID, lblSalesDelegateName, "SaleDelegateID", "اسـم مندوب المبيعات", "رقم مندوب المبيعات");
                else
                    PrepareSearchQuery.Search(txtSalesDelegateID, lblSalesDelegateName, "SaleDelegateID", "Delegate Name", "Delegate ID");

            }







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

        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + Comon.cInt(txtSalesDelegateID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtSalesDelegateID, lblSalesDelegateName, strSQL);
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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as CustomerName FROM Sales_Customers WHERE CustomerID=" + txtCustomerID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtCustomerID, lblCustomerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        string txtCustomerAccount()
        {
            try
            {
                strSQL = "SELECT AccountID  FROM Sales_Customers WHERE CustomerID=" + txtCustomerID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }

        private void txtSellerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as SellerName FROM Users WHERE UserID=" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtSellerID, lblSellerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtOldBarcodeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //  =strSQL = "SELECT ArbName as ItemName FROM Stc_ItemsUnit WHERE SellerID=" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                // CSearch.ControlValidating(txtSellerID, lblSellerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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
        private void Label13_Click(object sender, EventArgs e)
        {

        }

        private void frmSalesInvoiceReport_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CloseCashierDate", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("UserID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("WasteCost", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("EnterCost", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("NetSum", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("CashSum", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("FutureSum", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("PrevoiusCash", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("SellerName", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("FromSaleInvoiceReturn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ToSaleInvoiceReturn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("FromSaleInvoice", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ToSaleInvoice", typeof(string)));


        
        }
        string GetStrSQL()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();

                string filter = " Where 1=1  AND";
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long EndFromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromCloseDate.Text));
                long EndToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToCloseDate.Text));

                DataTable dt;
                // Dim dtMethodeName As DataTable
                // حسب الرقم

             
                if (EndFromDate != 0)
                    filter = filter + " .SalesCashierClose.CloseCashierDate >=" + EndFromDate + " AND ";

                if (EndToDate != 0)
                    filter = filter + " .SalesCashierClose.CloseCashierDate <=" + EndToDate + " AND ";

                // '''البائع''''العميل''''التكلفة''''المستودع
              
                if (txtSellerID.Text != string.Empty)
                    filter = filter + " .SalesCashierClose.UserID  =" + Comon.cInt(txtSellerID.Text) + "  AND ";
               

                ////////////////////////////
                filter = filter.Remove(filter.Length - 4, 4);

                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal,dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmmountTotal AS SumVat, غير موجود في جدول مردود المشتريات
                // strSQL = "  Select SalesCashierClose.* ,Sales_Sellers.ArbName As SellerName FROM   Sales_Sellers RIGHT OUTER JOIN SalesCashierClose ON Sales_Sellers.SellerID = SalesCashierClose.SellerID ";
                strSQL = "  Select SalesCashierClose.* ,Users.ArbName As SellerName FROM   Users RIGHT OUTER JOIN SalesCashierClose ON Users.UserID = SalesCashierClose.UserID ";
                strSQL = strSQL + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());

            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return  strSQL ;

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
                            row["CloseCashierDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["CloseCashierDate"].ToString());
                            row["UserID"] = dt.Rows[i]["UserID"].ToString();
                            row["SellerName"] = dt.Rows[i]["SellerName"].ToString();
                            row["FromSaleInvoiceReturn"] = dt.Rows[i]["FromSaleInvoiceReturn"].ToString();
                            row["ToSaleInvoiceReturn"] = dt.Rows[i]["ToSaleInvoiceReturn"].ToString();
                            row["FromSaleInvoice"] = dt.Rows[i]["FromSaleInvoice"].ToString();
                            row["ToSaleInvoice"] = dt.Rows[i]["ToSaleInvoice"].ToString();
                            row["WasteCost"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["WasteCost"]).ToString("N" + 2);
                            row["EnterCost"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["EnterCost"]).ToString("N" + 2);
                            row["NetSum"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["NetSum"]).ToString("N" + 2);
                            row["CashSum"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["CashSum"]).ToString("N" + 2);
                            row["FutureSum"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["FutureSum"]).ToString("N" + 2);
                            row["PrevoiusCash"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["PrevoiusCash"]).ToString("N" + 2);

                            // row["ID"] = dt.Rows[i]["ID"].ToString();

                            // row["TransCost"] = dt.Rows[i]["TransCost"].ToString();

                            _sampleData.Rows.Add(row);
                        }
                        }}

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

                txtSalesDelegateID.Text = "";
                txtDelegateID_Validating(null, null);


                txtStoreID.Enabled = true;
                txtCostCenterID.Enabled = true;

                txtSellerID.Enabled = true;
                txtSalesDelegateID.Enabled = true;

                txtCustomerID.Enabled = true;
                cmbMethodID.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtFromInvoiceNo.Enabled = true;
                txtToInvoicNo.Enabled = true;
                txtFromCloseDate.Enabled = true;
                txtToCloseDate.Enabled = true;

                txtFromDate.Text = "";
                txtToDate.Text = "";

                txtFromCloseDate .Text = "";
                txtToCloseDate.Text = "";
                txtToInvoicNo.Text = "";
                txtFromInvoiceNo.Text = "";
                cmbMethodID.ItemIndex = -1;
                lblCash.Text = "";
                lblNet.Text = "";
                lblFuture.Text = "";
                lblCashNet.Text = "";
                lblCash1.Text = "";
                lblNet1.Text = "";
                lblCheck.Text = "";
                lblTotal.Text = "";

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }


        }
        private void btnShow_Click(object sender, EventArgs e)
        {

            SalesInvoice();
            gridControl1.DataSource = _sampleData;
            if (gridView1.RowCount > 0)
            {
                btnShow.Visible = true;

                txtStoreID.Enabled = false;
                txtCostCenterID.Enabled = false;

                txtSellerID.Enabled = false;
                txtSalesDelegateID.Enabled = false;

                txtCustomerID.Enabled = false;
                cmbMethodID.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                txtFromInvoiceNo.Enabled = false;
                txtToInvoicNo.Enabled = false;
                txtFromCloseDate.Enabled = false;
                txtToCloseDate.Enabled = false;

            }
            else
            {

                Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");

                btnShow.Visible = true;
                DoNew();
            }



        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {



        }

        private void frmSalesInvoiceReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();

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
                rptForm.Parameters["CostCenter"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["SellerName"].Value = lblSellerName.Text.Trim().ToString();
                rptForm.Parameters["SalesDelegateName"].Value = lblSalesDelegateName.Text.Trim().ToString();
                rptForm.Parameters["MethodName"].Value = cmbMethodID.Text.Trim().ToString();
                rptForm.Parameters["CashSum"].Value = lblCash.Text .Trim().ToString();
                rptForm.Parameters["FutureSum"].Value = lblFuture.Text.Trim().ToString();
                rptForm.Parameters["NetSum"].Value = lblNet.Text.Trim().ToString();
                rptForm.Parameters["CashNetSum"].Value = lblCashNet.Text.Trim().ToString();
                rptForm.Parameters["Net1"].Value = lblNet1.Text.Trim().ToString();
                rptForm.Parameters["Cash1"].Value = lblCash1.Text.Trim().ToString();
                 rptForm.Parameters["CheckSum"].Value = lblCheck.Text.Trim().ToString();
               

                rptForm.Parameters["FromCloseDate"].Value = txtFromCloseDate.Text.Trim().ToString();
                rptForm.Parameters["ToCloseDate"].Value = txtToCloseDate.Text.Trim().ToString();
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
                    row["CloseCashierDate"] = gridView1.GetRowCellValue(i, "CloseCashierDate").ToString();

                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["VatAmount"] = gridView1.GetRowCellValue(i, "VatAmount").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["Profit"] = gridView1.GetRowCellValue(i, "Profit").ToString();

                    row["MethodeName"] = gridView1.GetRowCellValue(i, "MethodeName").ToString();
                    row["CustomerName"] = gridView1.GetRowCellValue(i, "CustomerName").ToString();
                    row["SellerName"] = gridView1.GetRowCellValue(i, "SellerName").ToString();
                    row["VatID"] = gridView1.GetRowCellValue(i, "VatID").ToString();

                    row["StoreName"] = gridView1.GetRowCellValue(i, "StoreName").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterName").ToString();
                    row["DelgateName"] = gridView1.GetRowCellValue(i, "DelgateName").ToString();
                    row["Notes"] = gridView1.GetRowCellValue(i, "Notes").ToString();

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

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
         }

 

        

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try{
            GridView view = sender as GridView;
            frmSalesInvoice frm = new frmSalesInvoice();
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
            catch { }
        }

        private void labelControl3_Click(object sender, EventArgs e)
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
        private void txtFromDateClose_EditValueChanged(object sender, EventArgs e)
        {
            if (Comon.ConvertDateToSerial(txtFromCloseDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtFromCloseDate.Text = Lip.GetServerDate();
        }

        private void txtToDateClose_EditValueChanged(object sender, EventArgs e)
        {
            if (Comon.ConvertDateToSerial(txtToCloseDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtToCloseDate.Text = Lip.GetServerDate();
        }
        int getTotalValue(GridView view, int listSourceRowIndex)
        {
             int  unitPrice=0;
             var dr = dtLocation.Select("ID=" + Comon.cInt(view.GetListSourceRowCellValue(listSourceRowIndex, "IsSendReview")));
            if(dr.Length>0)
             unitPrice = Comon.cInt(dr[0]["Location"].ToString());
           // int quantity = Comon.cInt(view.GetListSourceRowCellValue(listSourceRowIndex, "TotalEarlyTime"));

            return unitPrice ;
        }

        string getTotalValueString(GridView view, int listSourceRowIndex)
        {
             string  unitPrice=" ";
             var dr = dtLocation.Select("ID=" + Comon.cInt(view.GetListSourceRowCellValue(listSourceRowIndex, "IsSendReview")));
            if(dr.Length>0)
             unitPrice = dr[0]["ArbName"].ToString();

           // int quantity = Comon.cInt(view.GetListSourceRowCellValue(listSourceRowIndex, "TotalEarlyTime"));

            return unitPrice ;
        }
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.Column.FieldName == "ID" && e.IsGetData) e.Value =
                  getTotalValue(view, e.ListSourceRowIndex);
                else if (e.Column.FieldName == "IsSendReview1" && e.IsGetData) e.Value =
                  getTotalValueString(view, e.ListSourceRowIndex);
                else if (e.Column.FieldName == "TransCost" && e.IsGetData) e.Value =
                 getTotalValueString1(view, e.ListSourceRowIndex);
            }
            catch { }
        }

       decimal getTotalValueString1(GridView view, int listSourceRowIndex)
        {
            decimal  unitPrice=0;
            var dr = dtLocation.Select("ID=" + Comon.cInt(view.GetListSourceRowCellValue(listSourceRowIndex, "IsSendReview")));
            if(dr.Length>0)
                unitPrice = Comon.ConvertToDecimalPrice(dr[0]["TransCost"].ToString());
           // int quantity = Comon.cInt(view.GetListSourceRowCellValue(listSourceRowIndex, "TotalEarlyTime"));

            return unitPrice ;
        }

       private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
       {
              GridView view = sender as GridView;
           // نعدل الفاتورة برقم السائق ونظبع نسختين موجود فيها اسمن السائق والحي والعنوان واسم العميل 
              var sr = " update Sales_SalesInvoiceMaster set DeliveryID=" + gridView1.GetFocusedRowCellValue("DeliveryID").ToString() + " where InvoiceID= " + gridView1.GetFocusedRowCellValue("InvoiceID").ToString();
              Lip.ExecututeSQL(sr);
             

       }


    }
}

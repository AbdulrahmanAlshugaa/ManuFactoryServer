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
    public partial class frmEmpolyMealReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string FocusedControl;

        private string strSQL = "";
        private string where = "";
        DataTable dt = new DataTable();

        public DataTable _sampleData = new DataTable();
        public DataTable _sampleDataQty = new DataTable();
        public frmEmpolyMealReport()
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
            gridView2.OptionsView.EnableAppearanceEvenRow = true;
            gridView2.OptionsView.EnableAppearanceOddRow = true;
            strSQL = "EngName";
            if (UserInfo.Language == iLanguage.Arabic)
                strSQL = "ArbName";

            FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1");
            FillCombo.FillComboBox(cmbOrderType, "Res_OrderType", "ID", strSQL, "", "1=1");
            Common.filllookupEDit(ref repositoryItemLookUpEdit1, "ID", "Res_OrderType", strSQL, "0=0");
            Common.filllookupEDit(ref repositoryItemLookUpEdit2, "UserID", "Users", "ArbName", "Cancel=0");

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
                dgvColInvoiceID.Caption = "Invoice NO";
                dgvColInvoiceDate.Caption = "Invoice  Date ";
                dgvColTotal.Caption = "Total ";
                dgvColVatAmount.Caption = "Total VatAmount  ";

                dgvColMethodeName.Caption = "Method Sale";
                dgvColNet.Caption = "Net";


                dgvColDiscount.Caption = "Discount ";


                 dgvColSellerName.Caption = "Seller Name ";
                dgvColVatID.Caption = "Vat  ID";
                dgvColStoreName.Caption = "Stotre   Name ";
                dgvColCostCenterName.Caption = "Cost Center";
               dgvColDelgateName.Caption = "Delgate Name ";

                    dgvColNotes.Caption = "Notes";
                dgvColCloseCashierDate.Caption = "Close  CashierDate ";
                dgvColProfite.Caption = " Profit";
                dgvCustomerName.Caption = "Customer Name  ";



                btnShow.Text = btnShow.Tag.ToString();
                //  Label8.Text = btnShow.Tag.ToString();















            }


            chkRemaind.Visible = true;

            chkRemaind.Checked = true;



















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
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "SellerID1", "اسـم الموظف", "رقم الموظف");
                else
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "SellerID1", "Employee  Name", "Employee ID");
            }


            if (FocusedControl.Trim() == txtDeliveryID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtDeliveryID, lblDeliveryName, "DriverID",  "اسم السائق", "رقم السائق");
                else
                    PrepareSearchQuery.Search(txtDeliveryID, lblDeliveryName, "DriverID", "Customer Name", "Customer ID");
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
                    PrepareSearchQuery.Search(txtSellerID, lblSellerName, "SellerID", "Seller Name", "Seller ID");
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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as CustomerName FROM Sales_Sellers WHERE SellerID=" + txtCustomerID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
                //strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as SellerName FROM Sales_Sellers WHERE SellerID=" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                //CSearch.ControlValidating(txtSellerID, lblSellerName, strSQL);
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
            _sampleData.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("nvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CloseCashierDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("OrderTypes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetPaid", typeof(string)));


            _sampleData.Columns.Add(new DataColumn("Total", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Profit", typeof(decimal)));

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


            ///////////////////////////////////////////////////


            _sampleDataQty.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleDataQty.Columns.Add(new DataColumn("BarCode", typeof(string)));
            _sampleDataQty.Columns.Add(new DataColumn("ItemID", typeof(string)));
            _sampleDataQty.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            _sampleDataQty.Columns.Add(new DataColumn("ItemName", typeof(string)));
            _sampleDataQty.Columns.Add(new DataColumn("GroupName", typeof(string)));


            _sampleDataQty.Columns.Add(new DataColumn("SizeName", typeof(string)));
            _sampleDataQty.Columns.Add(new DataColumn("SizeID", typeof(string)));
            _sampleDataQty.Columns.Add(new DataColumn("F1", typeof(string)));

            ///////////////////////////////
        }
        string GetStrSQL()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();

                string filter = "(.Sales_SalesEmployMealMaster.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Sales_SalesEmployMealMaster.InvoiceID >0 AND dbo.Sales_SalesEmployMealMaster.Cancel =0   AND";
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long EndFromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromCloseDate.Text));
                long EndToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToCloseDate.Text));
                DateEdit obj = new DateEdit();
                InitializeFormatDate(obj);
                obj.EditValue = ((DateTime)txtToDate.EditValue).AddDays(1);
                long ToDateForRamadan = Comon.cLong(Comon.ConvertDateToSerial(obj.Text));
                DataTable dt;
                // Dim dtMethodeName As DataTable
                // حسب الرقم
                //if(rdLocall.Checked==true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'محلي'  or Sales_SalesInvoiceMaster.OrderType =  'Local')    AND ";

                //if (rdTakeaway.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'TakeAway'  or Sales_SalesInvoiceMaster.OrderType =  'سـفري')    AND ";

                //if (rdHunger.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'HangerStation' or Sales_SalesInvoiceMaster.OrderType =  'هنجر ستيشن')    AND ";

                //if (rdDelivery.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'Delivery'  or Sales_SalesInvoiceMaster.OrderType =  'توصيل')    AND ";

                if (cmbOrderType.Text != string.Empty && Comon.cInt(cmbOrderType.EditValue) != 0)
                    filter = filter + " Sales_SalesEmployMealMaster.OrderType =" + cmbOrderType.EditValue + " AND ";

                if (txtFromInvoiceNo.Text != string.Empty)
                    filter = filter + " Sales_SalesEmployMealMaster.InvoiceID >=" + txtFromInvoiceNo.Text + " AND ";

                if (txtToInvoicNo.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.InvoiceID <=" + txtToInvoicNo.Text + " AND ";

                // حسب التاريخ
                if (!chkRamadan.Checked)
                {
                    if (FromDate != 0)
                        filter = filter + " .Sales_SalesEmployMealMaster.InvoiceDate >=" + FromDate + " AND ";

                    if (ToDate != 0)
                        filter = filter + " .Sales_SalesEmployMealMaster.InvoiceDate <=" + ToDate + " AND ";
                }
                else
                {
                    if (FromDate != 0 && ToDate != 0)
                    {
                        filter = filter + String.Format(@"    (
(dbo.Sales_SalesEmployMealMaster.InvoiceDate={0} and Sales_SalesEmployMealMaster .RegTime between 400 and 2359) or
 ( dbo.Sales_SalesEmployMealMaster.InvoiceDate < {1} and  dbo.Sales_SalesEmployMealMaster.InvoiceDate > {0})or
 (dbo.Sales_SalesEmployMealMaster.InvoiceDate={1} and Sales_SalesEmployMealMaster.RegTime between 400 and 2359) or
  (dbo.Sales_SalesEmployMealMaster.InvoiceDate={2} and Sales_SalesEmployMealMaster.RegTime between 0 and 359) or
    (dbo.Sales_SalesEmployMealMaster.InvoiceDate={1} and Sales_SalesEmployMealMaster.RegTime between 0 and 359 And {1}<>{0})
 
   )", FromDate, ToDate, ToDateForRamadan) + " AND ";

                    }
                }
                // الاغلاق حسب التاريخ
                if (EndFromDate != 0)
                    filter = filter + " .Sales_SalesEmployMealMaster.CloseCashierDate >=" + EndFromDate + " AND ";

                if (EndToDate != 0)
                    filter = filter + " .Sales_SalesEmployMealMaster.CloseCashierDate <=" + EndToDate + " AND ";

                // '''البائع''''العميل''''التكلفة''''المستودع
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";


                if (txtDeliveryID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.DeliveryID  =" + Comon.cInt(txtDeliveryID.Text) + "  AND ";






                if (txtCostCenterID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

                if (txtCustomerID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.CustomerID  =" + Comon.cLong(txtCustomerID.Text) + "  AND ";

                if (txtSellerID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.USerID  =" + Comon.cInt(txtSellerID.Text) + "  AND ";
                if (cmbMethodID.Text != string.Empty && Comon.cInt (cmbMethodID.EditValue ) !=0)
                    filter = filter + " Sales_SalesEmployMealMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                ////////f
                if (txtOldBarCode.Text != string.Empty)
                    filter = filter + "Sales_EmployMealDetails.InvoiceID in( SELECT DISTINCT Sales_EmployMealDetails.InvoiceID FROM    Stc_ItemUnits INNER JOIN     Sales_EmployMealDetails ON Stc_ItemUnits.BarCode = Sales_EmployMealDetails.BarCode "
                + " WHERE  (Stc_ItemUnits.BarCode = " + txtOldBarCode.Text + " )  )  AND ";
                ////////////////////////////

                if (txtSalesDelegateID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.DelegateID  =" + Comon.cInt(txtSalesDelegateID.Text) + "  AND ";
                /////////////////////////////
                if (cmbMethodID.Text != string.Empty && Comon.cInt(cmbMethodID.EditValue) != 0)
                    filter = filter + " Sales_SalesEmployMealMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                //// '''''''''''''
                //if (chkRemaind.Checked==true)
                //    filter = filter + " Sales_SalesEmployMealMaster.RemaindAmount < 0  AND ";

                
                filter = filter.Remove(filter.Length - 4, 4);

                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal,dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmmountTotal AS SumVat, غير موجود في جدول مردود المشتريات
                strSQL = "  SELECT   Sales_SalesEmployMealMaster.InsuranceAmmount, Sales_SalesEmployMealMaster.UserID,Sales_SalesEmployMealMaster.OrderType, Sales_SalesEmployMealMaster.VATID ,   Sales_SalesEmployMealMaster.CustomerName AS CustomerName1, dbo.Sales_SalesEmployMealMaster.InvoiceID,Sales_SalesEmployMealMaster.NetAmount ,Sales_SalesEmployMealMaster.Notes,Sales_SalesEmployMealMaster.NetProcessID,dbo.Sales_SalesEmployMealMaster.MethodeID,dbo.Sales_SalesEmployMealMaster.CloseCashierDate,Sales_SalesEmployMealMaster.DiscountOnTotal, dbo.Sales_SalesEmployMealMaster.InvoiceDate, "
            + "  SUM(dbo.Sales_EmployMealDetails.QTY * dbo.Sales_EmployMealDetails.SalePrice) AS Total , Sales_SalesEmployMealMaster.AdditionaAmountTotal AS SumVat ,Sum(Sales_EmployMealDetails.Discount) As DiscountLines, dbo.Sales_SalesEmployMealMaster.Notes, "
            + "  dbo.Stc_Stores.ArbName AS Storname,dbo.Sales_Customers.VatID ,dbo.Sales_SalesMethodes.ArbName AS MethodeName, dbo.Sales_Sellers.ArbName AS CustomerName , dbo.Sales_SalesEmployMealMaster.CustomerID, "
            + "  dbo.Sales_Sellers.ArbName AS SellerName,Sales_SalesDelegate.ArbName As SaleDelegateName, dbo.Acc_CostCenters.ArbName as CostCenterName , dbo.Sales_SalesEmployMealMaster.CostCenterID"
            + "  ,SUM(((dbo.Sales_EmployMealDetails.SalePrice - dbo.Sales_EmployMealDetails.CostPrice) * dbo.Sales_EmployMealDetails.QTY)-Sales_EmployMealDetails.Discount) AS Profit , Clinic_InsuranceCompany.ArbName AS CompanyName"
            + " FROM dbo.Sales_EmployMealDetails INNER JOIN"
            + " dbo.Sales_SalesEmployMealMaster ON dbo.Sales_EmployMealDetails.InvoiceID = dbo.Sales_SalesEmployMealMaster.InvoiceID AND "
            + " dbo.Sales_EmployMealDetails.BranchID = dbo.Sales_SalesEmployMealMaster.BranchID LEFT OUTER JOIN "
            + " Clinic_InsuranceCompany ON Sales_SalesEmployMealMaster.CustomerID = Clinic_InsuranceCompany.CompanyID AND "
            + " Sales_SalesEmployMealMaster.BranchID = Clinic_InsuranceCompany.BranchID "
            + " LEFT OUTER JOIN"
            + " dbo.Sales_SalesDelegate ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Sales_SalesDelegate.BranchID AND "
            + " dbo.Sales_SalesEmployMealMaster.DelegateID = dbo.Sales_SalesDelegate.DelegateID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Acc_CostCenters.BranchID AND"
            + " dbo.Sales_SalesEmployMealMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN"
            + " dbo.Sales_Customers ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Sales_Customers.BranchID AND"
            + " dbo.Sales_SalesEmployMealMaster.CustomerID = dbo.Sales_Customers.CustomerID LEFT OUTER JOIN"
            + " dbo.Sales_Sellers ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Sales_Sellers.BranchID AND"
            + " dbo.Sales_SalesEmployMealMaster.CustomerID = dbo.Sales_Sellers.SellerID LEFT OUTER JOIN"
            + " dbo.Stc_Stores ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Stc_Stores.BranchID AND"
            + " dbo.Sales_SalesEmployMealMaster.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN"
            + " dbo.Sales_SalesMethodes ON dbo.Sales_SalesEmployMealMaster.MethodeID = dbo.Sales_SalesMethodes.MethodID"
            + " where " + filter
            + "  GROUP BY  Sales_SalesEmployMealMaster.InsuranceAmmount,  Sales_SalesEmployMealMaster.UserID,  Sales_SalesEmployMealMaster.OrderType, Sales_SalesEmployMealMaster.VATID ,  Sales_SalesEmployMealMaster.CustomerName, Sales_SalesEmployMealMaster.Notes,Sales_SalesEmployMealMaster.NetAmount , Sales_SalesEmployMealMaster.AdditionaAmountTotal,Sales_SalesEmployMealMaster.NetProcessID,dbo.Sales_SalesEmployMealMaster.MethodeID,  dbo.Sales_SalesEmployMealMaster.InvoiceID,Sales_SalesEmployMealMaster.DiscountOnTotal, dbo.Sales_SalesEmployMealMaster.InvoiceDate, dbo.Sales_SalesEmployMealMaster.Notes, dbo.Stc_Stores.ArbName, "
            + "  dbo.Sales_SalesMethodes.ArbName,dbo.Sales_Customers.VatID,dbo.Sales_SalesEmployMealMaster.CloseCashierDate, dbo.Sales_Customers.ArbName, dbo.Sales_SalesEmployMealMaster.CustomerID, dbo.Sales_Sellers.ArbName,dbo.Sales_SalesDelegate.ArbName, dbo.Acc_CostCenters.ArbName, "
            + "  dbo.Sales_SalesEmployMealMaster.CostCenterID, dbo.Sales_SalesEmployMealMaster.Cancel , Clinic_InsuranceCompany.ArbName HAVING (dbo.Sales_SalesEmployMealMaster.Cancel = 0) ";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return Lip.ConvertStrSQLLanguage(strSQL, iLanguage.English.ToString());

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
            // Obj.EditValue = DateTime.Now;
        }

        string GetStrSQLQty()
        {

            try
            {
        
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();
                string filter = "(.Sales_SalesEmployMealMaster.BranchID = " + UserInfo.BRANCHID + ") AND dbo.Sales_SalesEmployMealMaster.InvoiceID >0 AND dbo.Sales_SalesEmployMealMaster.Cancel =0   AND";
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long EndFromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromCloseDate.Text));
                long EndToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToCloseDate.Text));
                DateEdit obj = new DateEdit();
                InitializeFormatDate(obj);
                obj.EditValue = ((DateTime)txtToDate.EditValue).AddDays(1);
                long ToDateForRamadan = Comon.cLong(Comon.ConvertDateToSerial(obj.Text));
                DataTable dt;
                // Dim dtMethodeName As DataTable
                // حسب الرقم
                //if(rdLocall.Checked==true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'محلي'  or Sales_SalesInvoiceMaster.OrderType =  'Local')    AND ";

                //if (rdTakeaway.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'TakeAway'  or Sales_SalesInvoiceMaster.OrderType =  'سـفري')    AND ";

                //if (rdHunger.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'HangerStation' or Sales_SalesInvoiceMaster.OrderType =  'هنجر ستيشن')    AND ";

                //if (rdDelivery.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'Delivery'  or Sales_SalesInvoiceMaster.OrderType =  'توصيل')    AND ";

                if (cmbOrderType.Text != string.Empty && Comon.cInt(cmbOrderType.EditValue) != 0)
                    filter = filter + " Sales_SalesEmployMealMaster.OrderType =" + cmbOrderType.EditValue + " AND ";

                if (txtFromInvoiceNo.Text != string.Empty)
                    filter = filter + " Sales_SalesEmployMealMaster.InvoiceID >=" + txtFromInvoiceNo.Text + " AND ";

                if (txtToInvoicNo.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.InvoiceID <=" + txtToInvoicNo.Text + " AND ";

                // حسب التاريخ
                if (!chkRamadan.Checked)
                {
                    if (FromDate != 0)
                        filter = filter + " .Sales_SalesEmployMealMaster.InvoiceDate >=" + FromDate + " AND ";

                    if (ToDate != 0)
                        filter = filter + " .Sales_SalesEmployMealMaster.InvoiceDate <=" + ToDate + " AND ";
                }
                else
                {
                    if (FromDate != 0 && ToDate != 0)
                    {
                        filter = filter + String.Format(@"    (
(dbo.Sales_SalesEmployMealMaster.InvoiceDate={0} and Sales_SalesEmployMealMaster .RegTime between 400 and 2359) or
 ( dbo.Sales_SalesEmployMealMaster.InvoiceDate < {1} and  dbo.Sales_SalesEmployMealMaster.InvoiceDate > {0})or
 (dbo.Sales_SalesEmployMealMaster.InvoiceDate={1} and Sales_SalesEmployMealMaster.RegTime between 400 and 2359) or
  (dbo.Sales_SalesEmployMealMaster.InvoiceDate={2} and Sales_SalesEmployMealMaster.RegTime between 0 and 359) or
    (dbo.Sales_SalesEmployMealMaster.InvoiceDate={1} and Sales_SalesEmployMealMaster.RegTime between 0 and 359 And {1}<>{0})
 
   )", FromDate, ToDate, ToDateForRamadan) + " AND ";

                    }
                }
                // الاغلاق حسب التاريخ
                if (EndFromDate != 0)
                    filter = filter + " .Sales_SalesEmployMealMaster.CloseCashierDate >=" + EndFromDate + " AND ";

                if (EndToDate != 0)
                    filter = filter + " .Sales_SalesEmployMealMaster.CloseCashierDate <=" + EndToDate + " AND ";

                // '''البائع''''العميل''''التكلفة''''المستودع
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";


                if (txtDeliveryID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.DeliveryID  =" + Comon.cInt(txtDeliveryID.Text) + "  AND ";






                if (txtCostCenterID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

                if (txtCustomerID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.CustomerID  =" + Comon.cLong(txtCustomerID.Text) + "  AND ";

                if (txtSellerID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.USerID  =" + Comon.cInt(txtSellerID.Text) + "  AND ";
                if (cmbMethodID.Text != string.Empty && Comon.cInt(cmbMethodID.EditValue) != 0)
                    filter = filter + " Sales_SalesEmployMealMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                ////////f
                if (txtOldBarCode.Text != string.Empty)
                    filter = filter + "Sales_EmployMealDetails.InvoiceID in( SELECT DISTINCT Sales_EmployMealDetails.InvoiceID FROM    Stc_ItemUnits INNER JOIN     Sales_EmployMealDetails ON Stc_ItemUnits.BarCode = Sales_EmployMealDetails.BarCode "
                + " WHERE  (Stc_ItemUnits.BarCode = " + txtOldBarCode.Text + " )  )  AND ";
                ////////////////////////////

                if (txtSalesDelegateID.Text != string.Empty)
                    filter = filter + " .Sales_SalesEmployMealMaster.DelegateID  =" + Comon.cInt(txtSalesDelegateID.Text) + "  AND ";
                /////////////////////////////
                if (cmbMethodID.Text != string.Empty && Comon.cInt(cmbMethodID.EditValue) != 0)
                    filter = filter + " Sales_SalesEmployMealMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                //// '''''''''''''
                //if (chkRemaind.Checked==true)
                //    filter = filter + " Sales_SalesEmployMealMaster.RemaindAmount < 0  AND ";


                filter = filter.Remove(filter.Length - 4, 4);



                strSQL = "SELECT   dbo.Sales_EmployMealDetails.BarCode, dbo.Sales_EmployMealDetails.ItemID, dbo.Stc_Items.ArbName AS ItemName,"
           + " dbo.Stc_SizingUnits.SizeID, dbo.Stc_SizingUnits.ArbName AS SizeName, SUM(dbo.Sales_EmployMealDetails.QTY + dbo.Sales_EmployMealDetails.Bones) AS QtyWithBonus,"
           + " dbo.Stc_ItemsGroups.ArbName AS GroupName FROM dbo.Sales_EmployMealDetails INNER JOIN dbo.Stc_SizingUnits ON "
           + " dbo.Sales_EmployMealDetails.SizeID = dbo.Stc_SizingUnits.SizeID INNER JOIN dbo.Stc_Items ON dbo.Sales_EmployMealDetails.ItemID = dbo.Stc_Items.ItemID LEFT OUTER JOIN"
           + " dbo.Stc_ItemsGroups ON dbo.Stc_Items.GroupID = dbo.Stc_ItemsGroups.GroupID LEFT OUTER JOIN dbo.Sales_SalesEmployMealMaster ON dbo.Sales_EmployMealDetails.InvoiceID ="
           + " dbo.Sales_SalesEmployMealMaster.InvoiceID AND dbo.Sales_EmployMealDetails.BranchID = dbo.Sales_SalesEmployMealMaster.BranchID"
           + " WHERE "+filter;
                strSQL = strSQL + " GROUP BY dbo.Sales_EmployMealDetails.BarCode, dbo.Sales_EmployMealDetails.ItemID, dbo.Stc_Items.ArbName,"
                + " dbo.Stc_SizingUnits.SizeID, dbo.Stc_SizingUnits.ArbName, dbo.Stc_ItemsGroups.ArbName" //,dbo.Sales_SalesInvoiceMaster.InvoiceDate
                + " ORDER BY QtyWithBonus  Desc";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());



                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal,dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmmountTotal AS SumVat, غير موجود في جدول مردود المشتريات
            //    strSQL = "  SELECT   Sales_SalesEmployMealMaster.InsuranceAmmount, Sales_SalesEmployMealMaster.UserID,Sales_SalesEmployMealMaster.OrderType, Sales_SalesEmployMealMaster.VATID ,   Sales_SalesEmployMealMaster.CustomerName AS CustomerName1, dbo.Sales_SalesEmployMealMaster.InvoiceID,Sales_SalesEmployMealMaster.NetAmount ,Sales_SalesEmployMealMaster.Notes,Sales_SalesEmployMealMaster.NetProcessID,dbo.Sales_SalesEmployMealMaster.MethodeID,dbo.Sales_SalesEmployMealMaster.CloseCashierDate,Sales_SalesEmployMealMaster.DiscountOnTotal, dbo.Sales_SalesEmployMealMaster.InvoiceDate, "
            //+ "  SUM(dbo.Sales_EmployMealDetails.QTY * dbo.Sales_EmployMealDetails.SalePrice) AS Total , Sales_SalesEmployMealMaster.AdditionaAmountTotal AS SumVat ,Sum(Sales_EmployMealDetails.Discount) As DiscountLines, dbo.Sales_SalesEmployMealMaster.Notes, "
            //+ "  dbo.Stc_Stores.ArbName AS Storname,dbo.Sales_Customers.VatID ,dbo.Sales_SalesMethodes.ArbName AS MethodeName, dbo.Sales_Sellers.ArbName AS CustomerName , dbo.Sales_SalesEmployMealMaster.CustomerID, "
            //+ "  dbo.Sales_Sellers.ArbName AS SellerName,Sales_SalesDelegate.ArbName As SaleDelegateName, dbo.Acc_CostCenters.ArbName as CostCenterName , dbo.Sales_SalesEmployMealMaster.CostCenterID"
            //+ "  ,SUM(((dbo.Sales_EmployMealDetails.SalePrice - dbo.Sales_EmployMealDetails.CostPrice) * dbo.Sales_EmployMealDetails.QTY)-Sales_EmployMealDetails.Discount) AS Profit , Clinic_InsuranceCompany.ArbName AS CompanyName"
            //+ " FROM dbo.Sales_EmployMealDetails INNER JOIN"
            //+ " dbo.Sales_SalesEmployMealMaster ON dbo.Sales_EmployMealDetails.InvoiceID = dbo.Sales_SalesEmployMealMaster.InvoiceID AND "
            //+ " dbo.Sales_EmployMealDetails.BranchID = dbo.Sales_SalesEmployMealMaster.BranchID LEFT OUTER JOIN "
            //+ " Clinic_InsuranceCompany ON Sales_SalesEmployMealMaster.CustomerID = Clinic_InsuranceCompany.CompanyID AND "
            //+ " Sales_SalesEmployMealMaster.BranchID = Clinic_InsuranceCompany.BranchID "
            //+ " LEFT OUTER JOIN"
            //+ " dbo.Sales_SalesDelegate ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Sales_SalesDelegate.BranchID AND "
            //+ " dbo.Sales_SalesEmployMealMaster.DelegateID = dbo.Sales_SalesDelegate.DelegateID LEFT OUTER JOIN"
            //+ " dbo.Acc_CostCenters ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Acc_CostCenters.BranchID AND"
            //+ " dbo.Sales_SalesEmployMealMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN"
            //+ " dbo.Sales_Customers ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Sales_Customers.BranchID AND"
            //+ " dbo.Sales_SalesEmployMealMaster.CustomerID = dbo.Sales_Customers.CustomerID LEFT OUTER JOIN"
            //+ " dbo.Sales_Sellers ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Sales_Sellers.BranchID AND"
            //+ " dbo.Sales_SalesEmployMealMaster.CustomerID = dbo.Sales_Sellers.SellerID LEFT OUTER JOIN"
            //+ " dbo.Stc_Stores ON dbo.Sales_SalesEmployMealMaster.BranchID = dbo.Stc_Stores.BranchID AND"
            //+ " dbo.Sales_SalesEmployMealMaster.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN"
            //+ " dbo.Sales_SalesMethodes ON dbo.Sales_SalesEmployMealMaster.MethodeID = dbo.Sales_SalesMethodes.MethodID"
            //+ " where " + filter
            //+ "  GROUP BY  Sales_SalesEmployMealMaster.InsuranceAmmount,  Sales_SalesEmployMealMaster.UserID,  Sales_SalesEmployMealMaster.OrderType, Sales_SalesEmployMealMaster.VATID ,  Sales_SalesEmployMealMaster.CustomerName, Sales_SalesEmployMealMaster.Notes,Sales_SalesEmployMealMaster.NetAmount , Sales_SalesEmployMealMaster.AdditionaAmountTotal,Sales_SalesEmployMealMaster.NetProcessID,dbo.Sales_SalesEmployMealMaster.MethodeID,  dbo.Sales_SalesEmployMealMaster.InvoiceID,Sales_SalesEmployMealMaster.DiscountOnTotal, dbo.Sales_SalesEmployMealMaster.InvoiceDate, dbo.Sales_SalesEmployMealMaster.Notes, dbo.Stc_Stores.ArbName, "
            //+ "  dbo.Sales_SalesMethodes.ArbName,dbo.Sales_Customers.VatID,dbo.Sales_SalesEmployMealMaster.CloseCashierDate, dbo.Sales_Customers.ArbName, dbo.Sales_SalesEmployMealMaster.CustomerID, dbo.Sales_Sellers.ArbName,dbo.Sales_SalesDelegate.ArbName, dbo.Acc_CostCenters.ArbName, "
            //+ "  dbo.Sales_SalesEmployMealMaster.CostCenterID, dbo.Sales_SalesEmployMealMaster.Cancel , Clinic_InsuranceCompany.ArbName HAVING (dbo.Sales_SalesEmployMealMaster.Cancel = 0) ";
            //    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return Lip.ConvertStrSQLLanguage(strSQL, iLanguage.English.ToString());

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
                            row["Notes"] = dt.Rows[i]["Notes"].ToString();
                            row["UserID"] = dt.Rows[i]["UserID"].ToString();
                            row["OrderTypes"] = dt.Rows[i]["OrderType"].ToString();
                            row["CloseCashierDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["CloseCashierDate"].ToString());

                            row["nvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["InvoiceDate"].ToString());
                            row["Total"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Total"]).ToString("N" + 2);
                            row["Discount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountLines"]) + Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])).ToString("N" + 2);
                            row["VatAmount"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["SumVat"]).ToString("N" + 2);
                            row["Net"] = (Comon.ConvertToDecimalPrice(row["Total"]) - Comon.ConvertToDecimalPrice(row["Discount"]) + Comon.ConvertToDecimalPrice(row["VatAmount"])).ToString("N" + 2);
                            row["SellerName"] = dt.Rows[i]["SellerName"];
                            row["Profit"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InsuranceAmmount"]);
                            row["MethodeName"] = dt.Rows[i]["MethodeName"];
                            row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                            row["VatID"] = (dt.Rows[i]["VATID"].ToString() != string.Empty ? dt.Rows[i]["VATID"] : "");
                            row["StoreName"] = dt.Rows[i]["StorName"];
                            row["CostCenterName"] = (dt.Rows[i]["CostCenterName"].ToString() != string.Empty ? dt.Rows[i]["CostCenterName"] : "");
                            row["DelgateName"] = (dt.Rows[i]["SaleDelegateName"].ToString() != string.Empty ? dt.Rows[i]["SaleDelegateName"] : "");
                            total += Comon.ConvertToDecimalPrice(row["Net"]);
                            switch (Comon.cInt(dt.Rows[i]["MethodeID"].ToString()))
                            {

                                case (1):
                                    cash += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                                    row["NetPaid"] = "-";
                                    break;
                                case (2):
                                    future += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["NetPaid"] = "-";
                                    break;
                                case (3):
                                    netSum += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                                    row["Notes"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (4):
                                    check1 += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                                   // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (5):
                                    netCashSum += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    caschPaidWithNet += Comon.ConvertToDecimalPrice(dt.Rows[i]["NetAmount"]);
                                    row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
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



        private void SalesInvoiceQty()
        {
            try
            {
                DataRow row;
                dt = Lip.SelectRecord(GetStrSQLQty());
                _sampleDataQty.Clear();
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleDataQty.NewRow();

                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                            row["SizeID"] = dt.Rows[i]["SizeID"].ToString();
                            row["ItemID"] = dt.Rows[i]["ItemID"].ToString();


                            row["Qty"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyWithBonus"]).ToString("N" + 2);
                            row["ItemName"] = dt.Rows[i]["ItemName"];

                            row["SizeName"] = (dt.Rows[i]["SizeName"].ToString() != string.Empty ? dt.Rows[i]["SizeName"] : "");
                            row["GroupName"] = (dt.Rows[i]["GroupName"].ToString() != string.Empty ? dt.Rows[i]["GroupName"] : "");

                            // row["Net"] = (Comon.ConvertToDecimalPrice(row["TotalPurchase"]) - Comon.ConvertToDecimalPrice(row["TotalDiscount"])).ToString("N" + 2);

                            //row["ItemName"] = dt.Rows[i]["ItemName"];


                            _sampleDataQty.Rows.Add(row);

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
                cmbMethodID.EditValue = 0;
                txtSellerID.Text = "";
                lblSellerName.Text = "";
                txtDeliveryID.Text = "";
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
                //cmbMethodID.ItemIndex = -1;
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
            if (chkRemaind.Checked == true)
            {
               SalesInvoiceQty();
               gridControl2.DataSource = _sampleDataQty;
                if (gridView2.RowCount > 0)
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
            else {


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
                if (chkRemaind.Checked == true)
                {
                    DoPrintQty();
                    return;
                }

                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
                string ReportName = "rptSalesInvoiceReportArb1";
                bool IncludeHeader = true;
                string rptFormName = ReportName;// (UserInfo
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["FromInvoiceNo"].Value = txtFromInvoiceNo.Text.Trim().ToString();
                rptForm.Parameters["ToInvoiceNo"].Value = txtToInvoicNo.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenter"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["SellerName"].Value = lblSellerName.Text.Trim().ToString();
                rptForm.Parameters["SalesDelegateName"].Value = lblDeliveryName.Text.Trim().ToString();
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
                    row["SellerName"] = gridView1.GetRowCellDisplayText(i, "UserID").ToString();
                    row["VatID"] = gridView1.GetRowCellValue(i, "VatAmount").ToString();

                    row["StoreName"] = gridView1.GetRowCellValue(i, "StoreName").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterName").ToString();
                    row["DelgateName"] = gridView1.GetRowCellValue(i, "DelgateName").ToString();
                    row["Notes"] = gridView1.GetRowCellDisplayText(i, "OrderTypes").ToString();

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
        public  void DoPrintQty()
        {
            try
            {
               
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
               // ReportName = "";
                bool IncludeHeader = true;
                string rptFormName = "rptMinAndMaxSoldItemsArb1";// (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                //if (UserInfo.Language == iLanguage.English)
                //    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.RequestParameters = false;
                rptForm.Parameters["GroupName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = lblSellerName.Text.Trim().ToString();
                rptForm.Parameters["rptName"].Value = ("تقرير وجبات الموظفين -كميات ");

                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptMinAndMaxSoldItemsDataTable();

                /********************** Details ****************************/
              //  var dataTable = new dsReports.rptMinAndMaxSoldItemsDataTable();

                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView2.GetRowCellValue(i, "ItemName").ToString();
                    row["GroupName"] = gridView2.GetRowCellValue(i, "GroupName").ToString();
                    row["SizeName"] = gridView2.GetRowCellValue(i, "SizeName").ToString();
                    row["ItemID"] = gridView2.GetRowCellValue(i, "ItemID").ToString();
                    row["SizeID"] = gridView2.GetRowCellValue(i, "SizeID").ToString();
                    row["QTY"] = gridView2.GetRowCellValue(i, "Qty").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptMinAndMaxSoldItems";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                SplashScreenManager.CloseForm(false);
 
                //strSQL = ("SELECT ShowReportInReportViewer, ReportView,ReportExport  FROM UserReportsPermissions" + (" Where BranchID =" + (MySession.GlobalBranchID + (" And UserID=" + (UserInfo.ID.ToString() + (" And ReportName='" + (ReportName + "'")))))));
                //DataTable dReprt = new DataTable();
                //dReprt = Lip.SelectRecord(strSQL);
                //ShowReportInReportViewer =( Comon.cInt(dReprt.Rows[0]["ShowReportInReportViewer"].ToString())==1?true:false);
                if (ShowReportInReportViewer=true)
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


                    if (dt.Rows.Count > 0) for (int i = 1; i < 6; i++)
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
            //try{
            //GridView view = sender as GridView;
            //frmSalesInvoice frm = new frmSalesInvoice();
            //if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            //{
            //    if (UserInfo.Language == iLanguage.English)
            //        ChangeLanguage.EnglishLanguage(frm);
            //    frm.Show();
            //    frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("InvoiceID").ToString()) + 1, 8);
            //}
            //else
            //    frm.Dispose();
            //}
            //catch { }
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

        private void txtDeliveryID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as SellerName FROM Rest_Drivers WHERE DriverID=" + txtDeliveryID.Text + " And Cancel =0";
                CSearch.ControlValidating(txtDeliveryID, lblDeliveryName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void chkRemaind_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRemaind.Checked == true)
            {
                gridControl1.Visible = false;
                gridControl2.Visible = true;
            }
            else {
                gridControl1.Visible = true;
                gridControl2.Visible = false;
            }
        }


    }
}

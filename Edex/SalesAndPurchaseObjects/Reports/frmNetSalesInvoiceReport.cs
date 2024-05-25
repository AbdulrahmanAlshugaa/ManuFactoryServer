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
    public partial class frmNetSalesInvoiceReport : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string FocusedControl;
        private string strSQL = "";
        private string where = "";
        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        public frmNetSalesInvoiceReport()
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
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
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
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
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
                dgvColAmountReturn.Caption = " Profit";
                dgvCustomerName.Caption = "Customer Name  ";
                btnShow.Text = btnShow.Tag.ToString();
                //  Label8.Text = btnShow.Tag.ToString();

            } 
        }
        #region Function
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where BranchID=" + Comon.cInt(cmbBranchesID.EditValue);

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "اسـم الــعـــمـــيــل", "رقم الــعـــمـــيــل");
                else
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "Customer Name","Customer ID");
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
                    PrepareSearchQuery.Search(txtSellerID, lblSellerName, "SellerID", "اسـم البائع", "رقم البائع");
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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + Comon.cInt(txtSalesDelegateID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as CustomerName FROM Sales_Customers WHERE AccountID=" + txtCustomerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                strSQL = "SELECT AccountID  FROM Sales_Customers WHERE CustomerID=" + txtCustomerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);

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
                strSQL = "SELECT  " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as SellerName FROM Sales_Sellers WHERE SellerID=" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                //  =strSQL = "SELECT ArbName as ItemName FROM Stc_ItemsUnit WHERE SellerID=" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
            comboTypeReport.SelectedIndex = 0;
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("nvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CloseCashierDate", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("NetPaid", typeof(string)));


            _sampleData.Columns.Add(new DataColumn("Total", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Profit", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("AmountReturn", typeof(decimal)));

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
            _sampleData.Columns.Add(new DataColumn("TotalWhight", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Qty", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("DIAMOND_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("STONE_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("BAGET_W", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("ReturnDIAMOND_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ReturnSTONE_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ReturnBAGET_W", typeof(string)));


            _sampleData.Columns.Add(new DataColumn("NetDIAMOND_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetSTONE_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetBAGET_W", typeof(string)));

             

            cmbBranchesID.EditValue = UserInfo.BRANCHID;

            if (UserInfo.ID == 1)
            {
                cmbBranchesID.Visible = true;
                labelControl9.Visible = true;
                gridView1.Columns["Profit"].Visible = true;
            }

            else
            {
                gridView1.Columns["Profit"].Visible = true;
                cmbBranchesID.Visible = false;
                labelControl9.Visible = false;
            }
        }
        string GetStrSQL()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();

                string filter = "(dbo.Sales_SalesInvoiceMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_SalesInvoiceMaster.InvoiceID >0 AND dbo.Sales_SalesInvoiceMaster.Cancel =0   AND";
                
                if( Comon.cInt( cmbBranchesID.EditValue)>0)
                    filter = "( dbo.Sales_SalesInvoiceMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Sales_SalesInvoiceMaster.InvoiceID >0 AND dbo.Sales_SalesInvoiceMaster.Cancel =0   AND";
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long EndFromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromCloseDate.Text));
                long EndToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToCloseDate.Text));
                DataTable dt;
                // Dim dtMethodeName As DataTable
                // حسب الرقم
                if (txtFromInvoiceNo.Text != string.Empty)
                    filter = filter + " Sales_SalesInvoiceMaster.InvoiceID >=" + txtFromInvoiceNo.Text + " AND ";

                if (txtToInvoicNo.Text != string.Empty)
                    filter = filter + "  dbo.Sales_SalesInvoiceMaster.InvoiceID <=" + txtToInvoicNo.Text + " AND ";

                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate + " AND ";
                // الاغلاق حسب التاريخ
                if (EndFromDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.CloseCashierDate >=" + EndFromDate + " AND ";

                if (EndToDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.CloseCashierDate <=" + EndToDate + " AND ";

                // '''البائع''''العميل''''التكلفة''''المستودع
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                if (txtCostCenterID.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

              
                if (txtCustomerMobile.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.CustomerMobile  =" + txtCustomerMobile.Text + "  AND ";
                 


                if (txtSellerID.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.SellerID  =" + Comon.cInt(txtSellerID.Text) + "  AND ";
                if (cmbMethodID.Text != string.Empty && Comon.cInt (cmbMethodID.EditValue ) !=0)
                    filter = filter + " Sales_SalesInvoiceMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                ////////f
                if (txtOldBarCode.Text != string.Empty)
                    filter = filter + "Sales_SalesInvoiceDetails.InvoiceID in( SELECT DISTINCT Sales_SalesInvoiceDetails.InvoiceID FROM    Stc_ItemUnits INNER JOIN     Sales_SalesInvoiceDetails ON Stc_ItemUnits.BarCode = Sales_SalesInvoiceDetails.BarCode "
                + " WHERE  (Stc_ItemUnits.BarCode = " + txtOldBarCode.Text + " )  )  AND ";
                ////////////////////////////

                if (txtSalesDelegateID.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.DelegateID  =" + Comon.cInt(txtSalesDelegateID.Text) + "  AND ";
                /////////////////////////////
                if (cmbMethodID.Text != string.Empty && Comon.cInt(cmbMethodID.EditValue) != 0)
                    filter = filter + " Sales_SalesInvoiceMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                // '''''''''''''
                if (chkRemaind.Checked==true)
                    filter = filter + " Sales_SalesInvoiceMaster.RemaindAmount < 0  AND ";

                
                 filter = filter + " dbo.Sales_SalesInvoiceMaster.Cancel = 0  AND ";

                 if (comboTypeReport.SelectedIndex == 3)
                     filter = filter + " Sales_SalesInvoiceMaster.GoldUsing=1  AND ";
                 if (comboTypeReport.SelectedIndex == 4)
                     filter = filter + " Sales_SalesInvoiceMaster.GoldUsing=2  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal,dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmmountTotal AS SumVat, غير موجود في جدول مردود المشتريات
                strSQL = "SELECT Sales_SalesInvoiceMaster.VATID , Sales_SalesInvoiceMaster.CustomerName AS CustomerName1, dbo.Sales_SalesInvoiceMaster.InvoiceID,Sales_SalesInvoiceMaster.NetAmount ,Sales_SalesInvoiceMaster.Notes,Sales_SalesInvoiceMaster.NetProcessID,dbo.Sales_SalesInvoiceMaster.MethodeID,dbo.Sales_SalesInvoiceMaster.CloseCashierDate,Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.InvoiceDate, "
                + "   Sales_SalesInvoiceMaster.NetBalance   ,Sales_SalesInvoiceMaster.InvoiceTotal AS Total , Sales_SalesInvoiceMaster.AdditionaAmountTotal AS SumVat  ,Sum(Sales_SalesInvoiceDetails.Discount) As DiscountLines, dbo.Sales_SalesInvoiceMaster.Notes, "
                + "   dbo.Stc_Stores.ArbName AS Storname,dbo.Sales_Customers.VatID ,dbo.Sales_SalesMethodes.ArbName AS MethodeName, dbo.Sales_Customers.ArbName AS CustomerName , dbo.Sales_SalesInvoiceMaster.CustomerID, "
                + "   dbo.Sales_Sellers.ArbName AS SellerName,Sales_SalesDelegate.ArbName As SaleDelegateName, dbo.Acc_CostCenters.ArbName as CostCenterName , dbo.Sales_SalesInvoiceMaster.CostCenterID"
               
                + " , ISNULL((SELECT  SUM(QTY)   FROM dbo.Sales_SalesInvoiceDetails WHERE(InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)   AND(BranchID = " + cmbBranchesID.EditValue + ") ), 0) AS Qty18   "
                + " , ISNULL((SELECT  SUM(DIAMOND_W)   FROM dbo.Sales_SalesInvoiceDetails WHERE(InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)  AND(BranchID = " + cmbBranchesID.EditValue + ") ), 0) AS QtyDIAMOND_W"
                + " , ISNULL((SELECT  SUM(STONE_W)   FROM dbo.Sales_SalesInvoiceDetails WHERE(InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)  AND(BranchID = " + cmbBranchesID.EditValue + ") ), 0) AS QtySTONE_W"
                + " , ISNULL((SELECT  SUM(BAGET_W)   FROM dbo.Sales_SalesInvoiceDetails WHERE(InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)   AND(BranchID = " + cmbBranchesID.EditValue + ")), 0) AS QtyBAGET_W"

                + " , ISNULL((SELECT  NetBalance  FROM dbo.Sales_SalesInvoiceReturnMaster  WHERE(CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID) AND(BranchID = " + cmbBranchesID.EditValue + ")), 0) AS NetBalanceReturn "
                + " , ISNULL((SELECT  AdditionaAmountTotal  FROM dbo.Sales_SalesInvoiceReturnMaster  WHERE(CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID) AND(BranchID = " + cmbBranchesID.EditValue + ")), 0) AS VatAmountReturn "

                + " , ISNULL((SELECT   SUM(Sales_SalesInvoiceReturnDetails.QTY)  FROM            Sales_SalesInvoiceReturnMaster LEFT OUTER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID "
                + "   where Sales_SalesInvoiceReturnDetails.BranchID = " + cmbBranchesID.EditValue + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.CustomerInvoiceID HAVING(Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue  + ") AND (Sales_SalesInvoiceReturnMaster.CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)), 0) AS QTYReturn "

                + " , ISNULL((SELECT SUM(Sales_SalesInvoiceReturnDetails.DIAMOND_W) FROM            Sales_SalesInvoiceReturnMaster LEFT OUTER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID "
                + "   where   Sales_SalesInvoiceReturnDetails.BranchID = " + cmbBranchesID.EditValue + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.CustomerInvoiceID HAVING(Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND(Sales_SalesInvoiceReturnMaster.CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)), 0) AS QTYReturnDIAMOND_W "
                    //RETURN STONE
                + " , ISNULL((SELECT  SUM(Sales_SalesInvoiceReturnDetails.STONE_W) FROM            Sales_SalesInvoiceReturnMaster LEFT OUTER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID "
                + "   where     Sales_SalesInvoiceReturnDetails.BranchID = " + cmbBranchesID.EditValue + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.CustomerInvoiceID HAVING(Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND(Sales_SalesInvoiceReturnMaster.CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)), 0) AS QTYReturnSTONE_W "

                + " , ISNULL((SELECT  SUM(Sales_SalesInvoiceReturnDetails.BAGET_W) FROM            Sales_SalesInvoiceReturnMaster LEFT OUTER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID "
                + "   where     Sales_SalesInvoiceReturnDetails.BranchID = " + cmbBranchesID.EditValue + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.CustomerInvoiceID HAVING(Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND(Sales_SalesInvoiceReturnMaster.CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)), 0) AS QTYReturnBAGET_W "


                + " ,sum(dbo.Sales_SalesInvoiceDetails.Qty) AS TotalWhight ,SUM(((dbo.Sales_SalesInvoiceDetails.SalePrice - dbo.Sales_SalesInvoiceDetails.CostPrice))-Sales_SalesInvoiceDetails.Discount) AS Profit , Clinic_InsuranceCompany.ArbName AS CompanyName "
                + " FROM dbo.Sales_SalesInvoiceDetails INNER JOIN"
                + " dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesInvoiceDetails.InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID AND "
                + " dbo.Sales_SalesInvoiceDetails.BranchID = dbo.Sales_SalesInvoiceMaster.BranchID LEFT OUTER JOIN "
                + " Clinic_InsuranceCompany ON Sales_SalesInvoiceMaster.CustomerID = Clinic_InsuranceCompany.CompanyID AND "
                + " Sales_SalesInvoiceMaster.BranchID = Clinic_InsuranceCompany.BranchID "
                + " LEFT OUTER JOIN "
                + " dbo.Sales_SalesDelegate ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesDelegate.BranchID AND "
                + " dbo.Sales_SalesInvoiceMaster.DelegateID = dbo.Sales_SalesDelegate.DelegateID LEFT OUTER JOIN"
                + " dbo.Acc_CostCenters ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Acc_CostCenters.BranchID AND"
                + " dbo.Sales_SalesInvoiceMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN"
                + " dbo.Sales_Customers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Customers.BranchID AND"
                + " dbo.Sales_SalesInvoiceMaster.CustomerID = dbo.Sales_Customers.CustomerID LEFT OUTER JOIN"
                + " dbo.Sales_Sellers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Sellers.BranchID AND"
                + " dbo.Sales_SalesInvoiceMaster.SellerID = dbo.Sales_Sellers.SellerID LEFT OUTER JOIN"
                + " dbo.Stc_Stores ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Stc_Stores.BranchID AND"
                + " dbo.Sales_SalesInvoiceMaster.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN"
                + " dbo.Sales_SalesMethodes ON dbo.Sales_SalesInvoiceMaster.MethodeID = dbo.Sales_SalesMethodes.MethodID"
                + " where " + filter
                + "  GROUP BY  Sales_SalesInvoiceMaster.NetBalance , Sales_SalesInvoiceMaster.InvoiceTotal , Sales_SalesInvoiceMaster.VATID ,  Sales_SalesInvoiceMaster.CustomerName, Sales_SalesInvoiceMaster.Notes,Sales_SalesInvoiceMaster.NetAmount , Sales_SalesInvoiceMaster.AdditionaAmountTotal,Sales_SalesInvoiceMaster.NetProcessID,dbo.Sales_SalesInvoiceMaster.MethodeID,  dbo.Sales_SalesInvoiceMaster.InvoiceID,Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.InvoiceDate, dbo.Sales_SalesInvoiceMaster.Notes, dbo.Stc_Stores.ArbName, "
                + "  dbo.Sales_SalesMethodes.ArbName,dbo.Sales_Customers.VatID,dbo.Sales_SalesInvoiceMaster.CloseCashierDate, dbo.Sales_Customers.ArbName, dbo.Sales_SalesInvoiceMaster.CustomerID, dbo.Sales_Sellers.ArbName,dbo.Sales_SalesDelegate.ArbName, dbo.Acc_CostCenters.ArbName, "
                + "  dbo.Sales_SalesInvoiceMaster.CostCenterID, dbo.Sales_SalesInvoiceMaster.Cancel , Clinic_InsuranceCompany.ArbName Order by Sales_SalesInvoiceMaster.InvoiceID   ";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL ;

        }
        string GetStrSQLSave()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();

                string filter = "(dbo.Sales_SalesInvoiceMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_SalesInvoiceMaster.InvoiceID >0 AND dbo.Sales_SalesInvoiceMaster.Cancel =0   AND";

                if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                    filter = "( dbo.Sales_SalesInvoiceMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Sales_SalesInvoiceMaster.InvoiceID >0 AND dbo.Sales_SalesInvoiceMaster.Cancel =0   AND";
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long EndFromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromCloseDate.Text));
                long EndToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToCloseDate.Text));
                DataTable dt;
                // Dim dtMethodeName As DataTable
                // حسب الرقم
                if (txtFromInvoiceNo.Text != string.Empty)
                    filter = filter + " Sales_SalesInvoiceMaster.InvoiceID >=" + txtFromInvoiceNo.Text + " AND ";

                if (txtToInvoicNo.Text != string.Empty)
                    filter = filter + "  dbo.Sales_SalesInvoiceMaster.InvoiceID <=" + txtToInvoicNo.Text + " AND ";

                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate + " AND ";
                // الاغلاق حسب التاريخ
                if (EndFromDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.CloseCashierDate >=" + EndFromDate + " AND ";

                if (EndToDate != 0)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.CloseCashierDate <=" + EndToDate + " AND ";

                // '''البائع''''العميل''''التكلفة''''المستودع
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                if (txtCostCenterID.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

              
                if (txtCustomerMobile.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.CustomerMobile  =" + txtCustomerMobile.Text + "  AND ";
                 



                if (txtSellerID.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.SellerID  =" + Comon.cInt(txtSellerID.Text) + "  AND ";
                if (cmbMethodID.Text != string.Empty && Comon.cInt(cmbMethodID.EditValue) != 0)
                    filter = filter + " Sales_SalesInvoiceMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                ////////f
                if (txtOldBarCode.Text != string.Empty)
                    filter = filter + "Sales_SalesInvoiceDetails.InvoiceID in( SELECT DISTINCT Sales_SalesInvoiceDetails.InvoiceID FROM    Stc_ItemUnits INNER JOIN     Sales_SalesInvoiceDetails ON Stc_ItemUnits.BarCode = Sales_SalesInvoiceDetails.BarCode "
                + " WHERE  (Stc_ItemUnits.BarCode = " + txtOldBarCode.Text + " )  )  AND ";
                ////////////////////////////

                if (txtSalesDelegateID.Text != string.Empty)
                    filter = filter + " dbo.Sales_SalesInvoiceMaster.DelegateID  =" + Comon.cInt(txtSalesDelegateID.Text) + "  AND ";
                /////////////////////////////
                if (cmbMethodID.Text != string.Empty && Comon.cInt(cmbMethodID.EditValue) != 0)
                    filter = filter + " Sales_SalesInvoiceMaster.MethodeID =" + cmbMethodID.EditValue + " AND ";
                // '''''''''''''
                if (chkRemaind.Checked == true)
                    filter = filter + " Sales_SalesInvoiceMaster.RemaindAmount < 0  AND ";


                filter = filter + " dbo.Sales_SalesInvoiceMaster.Cancel = 0  AND ";

                if (comboTypeReport.SelectedIndex == 1)
                    filter = filter + " dbo.Sales_SalesInvoiceDetails.BarCode LIKE 'Z%[^0-9]%[0-9]%' AND ";

                if (comboTypeReport.SelectedIndex == 2)
                    filter = filter + " dbo.Sales_SalesInvoiceDetails.BarCode Not LIKE 'Z%[^0-9]%[0-9]%' AND ";
                
                filter = filter.Remove(filter.Length - 4, 4);

                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal,dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmmountTotal AS SumVat, غير موجود في جدول مردود المشتريات
                strSQL = "SELECT Sales_SalesInvoiceMaster.VATID , Sales_SalesInvoiceMaster.CustomerName AS CustomerName1, dbo.Sales_SalesInvoiceMaster.InvoiceID,Sales_SalesInvoiceMaster.NetAmount ,Sales_SalesInvoiceMaster.Notes,Sales_SalesInvoiceMaster.NetProcessID,dbo.Sales_SalesInvoiceMaster.MethodeID,dbo.Sales_SalesInvoiceMaster.CloseCashierDate,Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.InvoiceDate, "
                + "   Sales_SalesInvoiceMaster.NetBalance   ,Sales_SalesInvoiceMaster.InvoiceTotal AS Total , Sales_SalesInvoiceMaster.AdditionaAmountTotal AS SumVat  ,Sum(Sales_SalesInvoiceDetails.Discount) As DiscountLines, dbo.Sales_SalesInvoiceMaster.Notes, "
                + "   dbo.Stc_Stores.ArbName AS Storname,dbo.Sales_Customers.VatID ,dbo.Sales_SalesMethodes.ArbName AS MethodeName, dbo.Sales_Customers.ArbName AS CustomerName , dbo.Sales_SalesInvoiceMaster.CustomerID, "
                + "   dbo.Sales_Sellers.ArbName AS SellerName,Sales_SalesDelegate.ArbName As SaleDelegateName, dbo.Acc_CostCenters.ArbName as CostCenterName , dbo.Sales_SalesInvoiceMaster.CostCenterID"

                + " , ISNULL((SELECT  SUM(QTY)   FROM dbo.Sales_SalesInvoiceDetails WHERE(InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)   AND(BranchID = " + cmbBranchesID.EditValue + ") ), 0) AS Qty18   "
                + " , ISNULL((SELECT  SUM(DIAMOND_W)   FROM dbo.Sales_SalesInvoiceDetails WHERE(InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)  AND(BranchID = " + cmbBranchesID.EditValue + ") ), 0) AS QtyDIAMOND_W"
                + " , ISNULL((SELECT  SUM(STONE_W)   FROM dbo.Sales_SalesInvoiceDetails WHERE(InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)  AND(BranchID = " + cmbBranchesID.EditValue + ") ), 0) AS QtySTONE_W"
                + " , ISNULL((SELECT  SUM(BAGET_W)   FROM dbo.Sales_SalesInvoiceDetails WHERE(InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)   AND(BranchID = " + cmbBranchesID.EditValue + ")), 0) AS QtyBAGET_W"

                + " , ISNULL((SELECT  NetBalance  FROM dbo.Sales_SalesInvoiceReturnMaster  WHERE(CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID) AND(BranchID = " + cmbBranchesID.EditValue + ")), 0) AS NetBalanceReturn "
                + " , ISNULL((SELECT  AdditionaAmountTotal  FROM dbo.Sales_SalesInvoiceReturnMaster  WHERE(CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID) AND(BranchID = " + cmbBranchesID.EditValue + ")), 0) AS VatAmountReturn "

                + " , ISNULL((SELECT   SUM(Sales_SalesInvoiceReturnDetails.QTY)  FROM            Sales_SalesInvoiceReturnMaster LEFT OUTER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID "
                + "   where Sales_SalesInvoiceReturnDetails.BranchID = " + cmbBranchesID.EditValue + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.CustomerInvoiceID HAVING(Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND (Sales_SalesInvoiceReturnMaster.CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)), 0) AS QTYReturn "

                + " , ISNULL((SELECT SUM(Sales_SalesInvoiceReturnDetails.DIAMOND_W) FROM            Sales_SalesInvoiceReturnMaster LEFT OUTER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID "
                + "   where   Sales_SalesInvoiceReturnDetails.BranchID = " + cmbBranchesID.EditValue + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.CustomerInvoiceID HAVING(Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND(Sales_SalesInvoiceReturnMaster.CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)), 0) AS QTYReturnDIAMOND_W "
                    //RETURN STONE
                + " , ISNULL((SELECT  SUM(Sales_SalesInvoiceReturnDetails.STONE_W) FROM            Sales_SalesInvoiceReturnMaster LEFT OUTER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID "
                + "   where     Sales_SalesInvoiceReturnDetails.BranchID = " + cmbBranchesID.EditValue + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.CustomerInvoiceID HAVING(Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND(Sales_SalesInvoiceReturnMaster.CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)), 0) AS QTYReturnSTONE_W "

                + " , ISNULL((SELECT  SUM(Sales_SalesInvoiceReturnDetails.BAGET_W) FROM            Sales_SalesInvoiceReturnMaster LEFT OUTER JOIN Sales_SalesInvoiceReturnDetails ON Sales_SalesInvoiceReturnMaster.InvoiceID = Sales_SalesInvoiceReturnDetails.InvoiceID "
                + "   where     Sales_SalesInvoiceReturnDetails.BranchID = " + cmbBranchesID.EditValue + " GROUP BY Sales_SalesInvoiceReturnMaster.InvoiceID, Sales_SalesInvoiceReturnMaster.BranchID, Sales_SalesInvoiceReturnMaster.CustomerInvoiceID HAVING(Sales_SalesInvoiceReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND(Sales_SalesInvoiceReturnMaster.CustomerInvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID)), 0) AS QTYReturnBAGET_W "


                + " ,sum(dbo.Sales_SalesInvoiceDetails.Qty) AS TotalWhight ,SUM(((dbo.Sales_SalesInvoiceDetails.SalePrice - dbo.Sales_SalesInvoiceDetails.CostPrice))-Sales_SalesInvoiceDetails.Discount) AS Profit , Clinic_InsuranceCompany.ArbName AS CompanyName "
                + " FROM dbo.Sales_SalesInvoiceDetails INNER JOIN"
                + " dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesInvoiceDetails.InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID AND "
                + " dbo.Sales_SalesInvoiceDetails.BranchID = dbo.Sales_SalesInvoiceMaster.BranchID LEFT OUTER JOIN "
                + " Clinic_InsuranceCompany ON Sales_SalesInvoiceMaster.CustomerID = Clinic_InsuranceCompany.CompanyID AND "
                + " Sales_SalesInvoiceMaster.BranchID = Clinic_InsuranceCompany.BranchID "
                + " LEFT OUTER JOIN "
                + " dbo.Sales_SalesDelegate ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesDelegate.BranchID AND "
                + " dbo.Sales_SalesInvoiceMaster.DelegateID = dbo.Sales_SalesDelegate.DelegateID LEFT OUTER JOIN"
                + " dbo.Acc_CostCenters ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Acc_CostCenters.BranchID AND"
                + " dbo.Sales_SalesInvoiceMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN"
                + " dbo.Sales_Customers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Customers.BranchID AND"
                + " dbo.Sales_SalesInvoiceMaster.CustomerID = dbo.Sales_Customers.CustomerID LEFT OUTER JOIN"
                + " dbo.Sales_Sellers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Sellers.BranchID AND"
                + " dbo.Sales_SalesInvoiceMaster.SellerID = dbo.Sales_Sellers.SellerID LEFT OUTER JOIN"
                + " dbo.Stc_Stores ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Stc_Stores.BranchID AND"
                + " dbo.Sales_SalesInvoiceMaster.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN"
                + " dbo.Sales_SalesMethodes ON dbo.Sales_SalesInvoiceMaster.MethodeID = dbo.Sales_SalesMethodes.MethodID"
                + " where " + filter 
                + "  GROUP BY  Sales_SalesInvoiceMaster.NetBalance , Sales_SalesInvoiceMaster.InvoiceTotal , Sales_SalesInvoiceMaster.VATID ,  Sales_SalesInvoiceMaster.CustomerName, Sales_SalesInvoiceMaster.Notes,Sales_SalesInvoiceMaster.NetAmount , Sales_SalesInvoiceMaster.AdditionaAmountTotal,Sales_SalesInvoiceMaster.NetProcessID,dbo.Sales_SalesInvoiceMaster.MethodeID,  dbo.Sales_SalesInvoiceMaster.InvoiceID,Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.InvoiceDate, dbo.Sales_SalesInvoiceMaster.Notes, dbo.Stc_Stores.ArbName, "
                + "  dbo.Sales_SalesMethodes.ArbName,dbo.Sales_Customers.VatID,dbo.Sales_SalesInvoiceMaster.CloseCashierDate, dbo.Sales_Customers.ArbName, dbo.Sales_SalesInvoiceMaster.CustomerID, dbo.Sales_Sellers.ArbName,dbo.Sales_SalesDelegate.ArbName, dbo.Acc_CostCenters.ArbName, "
                + "  dbo.Sales_SalesInvoiceMaster.CostCenterID, dbo.Sales_SalesInvoiceMaster.Cancel , Clinic_InsuranceCompany.ArbName Order by Sales_SalesInvoiceMaster.InvoiceID   ";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;

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
                            row["CloseCashierDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["CloseCashierDate"].ToString());

                            row["nvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["InvoiceDate"].ToString());
                            row["Total"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Total"]).ToString("N" + 2);
                            row["Discount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountLines"]) + Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])).ToString("N" + 2);

                            row["VatAmount"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["SumVat"]).ToString("N" + 2);
                            row["Net"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["NetBalance"]).ToString("N" + 2);
                            // row["SellerName"] = dt.Rows[i]["SellerName"];
                         
                             row["AmountReturn"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["NetBalanceReturn"].ToString()).ToString("N" + 2);
                             
                          
                            
                            row["DelgateName"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QTYReturn"].ToString());
                            row["VatAmount"] = (Comon.ConvertToDecimalPrice(row["VatAmount"].ToString()) - Comon.ConvertToDecimalPrice(dt.Rows[i]["VatAmountReturn"].ToString())).ToString("N" + 2);

                            row["MethodeName"] = dt.Rows[i]["MethodeName"];
                            row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                            
                            row["TotalWhight"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["TotalWhight"]).ToString("N" + 2); ; ;
                            row["CloseCashierDate"] = Comon.ConvertToDecimalPrice((Comon.ConvertToDecimalPrice(row["Net"]) - Comon.ConvertToDecimalPrice(row["AmountReturn"]))).ToString("N" + 2);
                            row["StoreName"] = Comon.ConvertToDecimalPrice(row["TotalWhight"]) - Comon.ConvertToDecimalPrice(row["DelgateName"]);

                            row["DIAMOND_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyDIAMOND_W"]);
                            row["STONE_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtySTONE_W"]);
                            row["BAGET_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyBAGET_W"]);

                             
                            row["ReturnDIAMOND_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnDIAMOND_W"]);
                            row["ReturnSTONE_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnSTONE_W"]);
                            row["ReturnBAGET_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnBAGET_W"]);
                             

                            row["NetDIAMOND_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyDIAMOND_W"]) - Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnDIAMOND_W"]);
                            row["NetSTONE_W"] =  Comon.ConvertToDecimalPrice(dt.Rows[i]["QtySTONE_W"]) - Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnSTONE_W"]);
                            row["NetBAGET_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyBAGET_W"]) - Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnBAGET_W"]);



                            total += Comon.ConvertToDecimalPrice(row["Net"]);
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

                            if (Comon.ConvertToDecimalPrice(row["CloseCashierDate"].ToString()) > 0)
                                row["Profit"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Profit"].ToString()).ToString("N" + 2);
                            else
                                row["Profit"] = 0;

                            if (UserInfo.ID != 1)
                                row["Profit"] = "0";
                            
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
        private void SalesInvoiceSave()
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
                dt = Lip.SelectRecord(GetStrSQLSave());
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
                            row["CloseCashierDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["CloseCashierDate"].ToString());

                            row["nvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["InvoiceDate"].ToString());
                            row["Total"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Total"]).ToString("N" + 2);
                            row["Discount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountLines"]) + Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])).ToString("N" + 2);

                            row["VatAmount"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["SumVat"]).ToString("N" + 2);
                            row["Net"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["NetBalance"]).ToString("N" + 2);
                            // row["SellerName"] = dt.Rows[i]["SellerName"];

                            row["AmountReturn"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["NetBalanceReturn"].ToString()).ToString("N" + 2);



                            row["DelgateName"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QTYReturn"].ToString());
                            row["VatAmount"] = (Comon.ConvertToDecimalPrice(row["VatAmount"].ToString()) - Comon.ConvertToDecimalPrice(dt.Rows[i]["VatAmountReturn"].ToString())).ToString("N" + 2);

                            row["MethodeName"] = dt.Rows[i]["MethodeName"];
                            row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");

                            row["TotalWhight"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["TotalWhight"]).ToString("N" + 2); ; ;
                            row["CloseCashierDate"] = Comon.ConvertToDecimalPrice((Comon.ConvertToDecimalPrice(row["Net"]) - Comon.ConvertToDecimalPrice(row["AmountReturn"]))).ToString("N" + 2);
                            row["StoreName"] = Comon.ConvertToDecimalPrice(row["TotalWhight"]) - Comon.ConvertToDecimalPrice(row["DelgateName"]);

                            row["DIAMOND_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyDIAMOND_W"]);
                            row["STONE_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtySTONE_W"]);
                            row["BAGET_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyBAGET_W"]);


                            row["ReturnDIAMOND_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnDIAMOND_W"]);
                            row["ReturnSTONE_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnSTONE_W"]);
                            row["ReturnBAGET_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnBAGET_W"]);


                            row["NetDIAMOND_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyDIAMOND_W"]) - Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnDIAMOND_W"]);
                            row["NetSTONE_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtySTONE_W"]) - Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnSTONE_W"]);
                            row["NetBAGET_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyBAGET_W"]) - Comon.ConvertToDecimalPrice(dt.Rows[i]["QtyReturnBAGET_W"]);



                            total += Comon.ConvertToDecimalPrice(row["Net"]);
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

                            if (Comon.ConvertToDecimalPrice(row["CloseCashierDate"].ToString()) > 0)
                                row["Profit"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Profit"].ToString()).ToString("N" + 2);
                            else
                                row["Profit"] = 0;

                            if (UserInfo.ID != 1)
                                row["Profit"] = "0";

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
                txtCustomerMobile.Text = "";

                cmbBranchesID.Enabled = true;

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }


        }
        private void GetByCaliper(int row, int InvoiceID)
        {
            decimal Qty18 = 0;
            decimal Qty21 = 0;
            decimal Qty24 = 0;
            decimal Qty22 = 0;
            DataTable dt = new DataTable();
            decimal InvoiceTotal = 0;
            decimal AdditionaAmmountTotal = 0;
            decimal Net = 0;
            decimal TotalDiscount = 0;
            decimal TheCount18 = 0;
            decimal TheCount21 = 0;
            decimal TheCount24 = 0;
            decimal Total18 = 0;
            decimal Total21 = 0;
            decimal Total24 = 0;
            decimal AdditionaAmmount18 = 0;
            decimal AdditionaAmmount21 = 0;
            decimal AdditionaAmmount24 = 0;
            decimal Net18 = 0;
            decimal Net21 = 0;
            decimal Net24 = 0;
             

            strSQL = "SELECT  InvoiceID ,  dbo.Sales_TotalSales.InvoiceDate , SUM(Qty18) as Qty18 , SUM(Qty21) as Qty21,SUM(Qty24) as Qty24,  SUM(Qty22) as Qty22 , "
            + " SUM(TheCount18) AS TheCount18, SUM(TheCount21) AS TheCount21 , SUM(TheCount24) AS TheCount24 , SUM(TheCount22) AS TheCount22 , "
            + " SUM(Total18) AS Total18 , SUM(Total21) AS Total21, SUM(Total24) AS Total24, SUM(Total22) AS Total22,"
            + " SUM(AdditionaAmmount18) AS AdditionaAmmount18 , SUM(AdditionaAmmount21) AS AdditionaAmmount21 ,  SUM(AdditionaAmmount24) AS AdditionaAmmount24 ,  SUM(AdditionaAmmount22) AS AdditionaAmmount22 "
            + " , SUM(Net18) AS Net18 , SUM(Net21) AS Net21  , SUM(Net24) AS Net24 , SUM(Net22) AS Net22"
            + " from Sales_TotalSales "
            + " WHERE (dbo.Sales_TotalSales.BranchID = " + UserInfo.BRANCHID + ") and InvoiceID=" + InvoiceID;

            long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
            long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            long EndFromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromCloseDate.Text));
            long EndToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToCloseDate.Text));


            if (txtFromDate.Text != string.Empty)
                strSQL = strSQL + " AND InvoiceDate >= " + FromDate;

            if (txtToDate.Text != string.Empty)
                strSQL = strSQL + " AND dbo.Sales_TotalSales.InvoiceDate <= " + ToDate;
            if (txtToInvoicNo.Text != string.Empty)
                strSQL = strSQL + " AND Sales_TotalSales.InvoiceID <=" + txtToInvoicNo.Text + " AND ";
            if (txtFromInvoiceNo.Text != string.Empty)
                strSQL = strSQL + " Sales_TotalSales.InvoiceID >=" + txtFromInvoiceNo.Text;



            strSQL = strSQL + " GROUP BY InvoiceDate,InvoiceID ";

          
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                     
                    Qty18 =Comon.cDec( Qty18) + Comon.cDec(dt.Rows[i]["Qty18"].ToString());
                    Qty21 = Comon.cDec(Qty21) + Comon.cDec(dt.Rows[i]["Qty21"].ToString());
                    Qty24 = Comon.cDec(Qty24) + Comon.cDec(dt.Rows[i]["Qty24"].ToString());
                    Qty22 = Comon.cDec(Qty22) + Comon.cDec(dt.Rows[i]["Qty22"].ToString());
                   
                }
            }

            //GridView.Rows[row[.Cells(dgvColQty18.Name).Value = Qty18.ToString("N" + GlobalPriceDigits);
            //GridView.Rows(row).Cells(dgvColQty21.Name).Value = Qty21.ToString("N" + GlobalPriceDigits);
            //GridView.Rows(row).Cells(dgvColQty24.Name).Value = Qty24.ToString("N" + GlobalPriceDigits);
            //GridView.Rows(row).Cells(dgvColQty22.Name).Value = Qty22.ToString("N" + GlobalPriceDigits);
        }
        private void btnShow_Click(object sender, EventArgs e)
        {


            if (comboTypeReport.SelectedIndex == 0 || comboTypeReport.SelectedIndex == 3 || comboTypeReport.SelectedIndex == 4)
            {
                SalesInvoice();
                gridControl1.DataSource = _sampleData;
            }
            else if (comboTypeReport.SelectedIndex == 1)
            {
              SalesInvoiceSave();
                gridControl1.DataSource = _sampleData;
            }
            else if (comboTypeReport.SelectedIndex == 2)
            {
                SalesInvoiceSave();
                gridControl1.DataSource = _sampleData;
            }
           
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
                ReportName = "rptSalesInvoiceNetReport";
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
                    row["CloseCashierDate"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CloseCashierDate").ToString());
                    row["Total"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                    row["Discount"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                    row["VatAmount"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "VatAmount").ToString());
                    row["Net"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                    if (UserInfo.ID == 1)
                        row["Profit"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Profit").ToString());
                    else
                        row["Profit"] = "0";
                    row["AmountReturn"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AmountReturn").ToString());
                    row["MethodeName"] = gridView1.GetRowCellValue(i, "MethodeName").ToString();
                    row["CustomerName"] = gridView1.GetRowCellValue(i, "CustomerName").ToString();
                    row["SellerName"] = gridView1.GetRowCellValue(i, "SellerName").ToString();
                    row["VatID"] = gridView1.GetRowCellValue(i, "VatAmount").ToString();
                    row["StoreName"] = gridView1.GetRowCellValue(i, "StoreName").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterName").ToString();
                    row["DelgateName"] = gridView1.GetRowCellValue(i, "DelgateName").ToString();
                    row["Notes"] = gridView1.GetRowCellValue(i, "TotalWhight").ToString();
                    row["TotalWhight"] =Comon.ConvertToDecimalQty( gridView1.GetRowCellValue(i, "TotalWhight").ToString());
                    row["Qty18"] = gridView1.GetRowCellValue(i, "DIAMOND_W").ToString();
                    row["Qty21"] = gridView1.GetRowCellValue(i, "NetSTONE_W").ToString();
                    row["Qty22"] = gridView1.GetRowCellValue(i, "NetBAGET_W").ToString();
                  

                    row["QTYReturn18"] = gridView1.GetRowCellValue(i, "ReturnDIAMOND_W").ToString();
                    row["QTYReturn21"] = gridView1.GetRowCellValue(i, "NetDIAMOND_W").ToString();
                    row["QTYReturn22"] = 0 ;
                    row["QTYReturn24"] =0 ;

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
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                int GoldUSing = Comon.cInt(Lip.GetValue("select GoldUsing from Sales_SalesInvoiceMaster where  InvoiceID=" + view.GetFocusedRowCellValue("InvoiceID").ToString()));
                if (GoldUSing == 1)
                {

                    frmCashierSalesAlmas frm = new frmCashierSalesAlmas();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                        frm.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm.ReadRecord(Comon.cLong(view.GetFocusedRowCellValue("InvoiceID").ToString()));

                    }
                    else
                        frm.Dispose();

                }
                else if (GoldUSing == 2)
                {
                    frmCashierSalesGold frm = new frmCashierSalesGold();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                        frm.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                        frm.ReadRecord(Comon.cLong(view.GetFocusedRowCellValue("InvoiceID").ToString()));

                    }
                    else
                        frm.Dispose();
                }
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
        private void Label3_Click(object sender, EventArgs e)
        {

        }
        private void cmbMethodID_EditValueChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            frmSalesInvoiceReportOrder frm = new frmSalesInvoiceReportOrder();
            frm.Show();
            frm.FormView = true;
            frm.FormAdd = true;

        }

        private void txtCustomerMobile_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DataTable dt;
                if (txtCustomerMobile.Text != string.Empty && txtCustomerMobile.Text != "0")
                {
                    strSQL = "SELECT  * FROM Sales_CustomerAnSublierListArb Where    Mobile =" + txtCustomerMobile.Text;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        txtCustomerID.Text = dt.Rows[0]["AcountID"].ToString();
                        txtCustomerID_Validating(null, null);
                        // txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();

                    }
                }


              

                else
                {
                    lblCustomerName.Text = "";
                    txtCustomerID.Text = "";
                    txtCustomerMobile.Text   = "";
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
    }
}

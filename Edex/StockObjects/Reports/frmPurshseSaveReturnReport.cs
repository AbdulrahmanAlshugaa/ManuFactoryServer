using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Edex.GeneralObjects.GeneralForms;
using DevExpress.XtraGrid.Views.Grid;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraReports.UI;
using Edex.SalesAndSaleObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Transactions;

namespace Edex.StockObjects.Reports
{
    public partial class frmPurshseSaveReturnReport :BaseForm
    {
        
        
        #region Declare 
        public string FocusedControl;
        private string strSQL = "";
        private string where = "";
        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        #endregion 

        public frmPurshseSaveReturnReport()
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


            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));

            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;

            if (UserInfo.Language == iLanguage.English)
            {
                dgvolSn.Caption = "# ";
                dgvColBarCode.Caption = "BarCode";
                

                dgvColItemName.Caption = "Method Sale";
               


              
                dgvColStoreName.Caption = "Stotre   Name ";
                dgvColCostCenterName.Caption = "Cost Center";
                dgvColDelgateName.Caption = "Delgate Name ";

                dgvColNotes.Caption = "Notes";
                dgvColCloseCashierDate.Caption = "ItemName ";
                dgvColProfite.Caption = " Profit";
                dgvCustomerName.Caption = "Customer Name  ";



             

            }
            
            //this is Event 
            this.KeyDown += frmPurshseSaveReturnReport_KeyDown;


            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
            this.txtSellerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSellerID_Validating);
            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtCustomerMobile.Validating+=txtCustomerMobile_Validating;
            this.gridView1.DoubleClick += gridView1_DoubleClick;
            this.Load+=frmPurshseSaveReturnReport_Load;
            this.txtGroupChar.KeyPress += txtGroupChar_KeyPress;
            this.txtBarCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtOldBarcodeID_Validating);
        }

        public void txtOldBarcodeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSQLForBarcode = " SELECT   TOP (1)  Stc_Items.ArbName AS ItemName   FROM  Stc_Items RIGHT OUTER JOIN       Sales_PurchaseInvoiceDetails LEFT OUTER JOIN "
             + " Stc_SizingUnits ON Sales_PurchaseInvoiceDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Sales_PurchaseInvoiceDetails.ItemID "
             + "  WHERE  (Sales_PurchaseInvoiceDetails.BarCode ='" + txtBarCode.Text + "') AND (Sales_PurchaseInvoiceDetails.Cancel = 0)";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQLForBarcode, "arb");

                DataTable barc = new DataTable();
                barc = Lip.SelectRecord(strSQLForBarcode);
                if (barc.Rows.Count > 0)
                {
                    lblBarCodeName.Text = barc.Rows[0][0].ToString().ToUpper();
                    txtBarCode.Text = txtBarCode.Text.ToString().ToUpper();
                }
                else
                {
                    lblBarCodeName.Text = "";
                    txtBarCode.Text = "";
                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
              
                GridView view = sender as GridView;
                    frmCashierPurchaseSaveDaimondReturn frm = new frmCashierPurchaseSaveDaimondReturn();

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
            catch { }
        }

      

        private void frmPurshseSaveReturnReport_Load(object sender, EventArgs e)
        {
            

            _sampleData = new DataTable();
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceID", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("BarCode", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ItemName", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("QTY", typeof(decimal)));


            _sampleData.Columns.Add(new DataColumn("SalePrice", typeof(decimal)));


            _sampleData.Columns.Add(new DataColumn("CostPrice", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("VatAmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Net", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("MethodeName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Mobile", typeof(string)));
       
            _sampleData.Columns.Add(new DataColumn("StorName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DelgateName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("F1", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TotalWhight", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("DIAMOND_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("STONE_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("BAGET_W", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("GoldUsing", typeof(decimal)));

            cmbBranchesID.EditValue = UserInfo.BRANCHID;

            if (UserInfo.ID == 1)
            {
                cmbBranchesID.Visible = true;
                labelControl1.Visible = true;             
            }
            else
            {
                cmbBranchesID.Visible = false;
                labelControl1.Visible = false;            
            }
            
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
                strSQL = "SELECT AccountID  FROM Sales_Customers WHERE AccountID=" + txtCustomerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);

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
        private void txtCustomerMobile_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
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
                        txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();

                    }

                }
                else
                {
                    lblCustomerName.Text = "";
                    txtCustomerID.Text = "";

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

         void frmPurshseSaveReturnReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();

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
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "CustomerID", "اسـم الــعـــمـــيــل", "رقم الــعـــمـــيــل");
                else
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "CustomerID", "Customer Name", "Customer ID");
            }


            else if (FocusedControl.Trim() == txtStoreID.Name)
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
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID", MySession.GlobalBranchID);
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
           string GetStrSQL()
           {
               try
               {
                   SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                   simpleButton3.Visible = false;
                   Application.DoEvents();

                   string filter = "(dbo.Sales_PurchaseInvoiceSaveReturnMaster.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_PurchaseInvoiceSaveReturnMaster.InvoiceID >0 AND dbo.Sales_PurchaseInvoiceSaveReturnMaster.Cancel =0   AND";

                   if (Comon.cInt(cmbBranchesID.EditValue) > 0)
                       filter = "(dbo.Sales_PurchaseInvoiceSaveReturnMaster.BranchID = " + cmbBranchesID.EditValue + ") AND dbo.Sales_PurchaseInvoiceSaveReturnMaster.InvoiceID >0 AND dbo.Sales_PurchaseInvoiceSaveReturnMaster.Cancel =0   AND";



                   strSQL = "";
                   long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                   long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                   
                   DataTable dt;
                   // Dim dtMethodeName As DataTable
                   // حسب الرقم

                   
                   // حسب التاريخ
                   if (FromDate != 0)
                       filter = filter + " dbo.Sales_PurchaseInvoiceSaveReturnMaster.InvoiceDate >=" + FromDate + " AND ";

                   if (ToDate != 0)
                       filter = filter + " dbo.Sales_PurchaseInvoiceSaveReturnMaster.InvoiceDate <=" + ToDate + " AND ";
                   

                   // '''البائع''''العميل''''التكلفة''''المستودع
                   if (txtStoreID.Text != string.Empty)
                       filter = filter + " dbo.Sales_PurchaseInvoiceSaveReturnMaster.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";

                   if (txtCostCenterID.Text != string.Empty)
                       filter = filter + " dbo.Sales_PurchaseInvoiceSaveReturnMaster.CostCenterID  =" + Comon.cInt(txtCostCenterID.Text) + "  AND ";

                   if (txtCustomerID.Text != string.Empty)
                       filter = filter + " dbo.Sales_PurchaseInvoiceSaveReturnMaster.SupplierID   =" + Comon.cLong(Lip.GetValue(txtCustomerAccount())) + "  AND ";


                   if (txtBarCode.Text != string.Empty)
                       filter = filter + " dbo.Sales_PurchaseSaveInvoiceReturnDetails.BarCode ='" + txtBarCode.Text.ToString() + "' AND ";
                   
                  filter = filter.Remove(filter.Length - 4, 4);



                  strSQL = "SELECT dbo.Sales_PurchaseSaveInvoiceReturnDetails.DIAMOND_W,Sales_PurchaseSaveInvoiceReturnDetails.InvoiceID, dbo.Sales_PurchaseSaveInvoiceReturnDetails.STONE_W, dbo.Sales_PurchaseSaveInvoiceReturnDetails.BAGET_W, dbo.Sales_PurchaseSaveInvoiceReturnDetails.CostPrice, "
                + " dbo.Sales_PurchaseSaveInvoiceReturnDetails.ItemID, dbo.Sales_PurchaseInvoiceSaveReturnMaster.SupplierID AS CustomerID  ,  dbo.Sales_PurchaseSaveInvoiceReturnDetails.Caliber, "
                +" dbo.Sales_PurchaseSaveInvoiceReturnDetails.QTY, dbo.Sales_PurchaseInvoiceSaveReturnMaster.InvoiceDate, dbo.Sales_PurchaseSaveInvoiceReturnDetails.BarCode, dbo.Acc_CostCenters.ArbName as CostCenterName, "
                +" dbo.Sales_PurchaseSaveInvoiceReturnDetails.Description AS ItemName, dbo.Stc_Stores.ArbName AS StorName, dbo.Sales_PurchaseInvoiceSaveReturnMaster.Mobile, "
                +" dbo.Sales_PurchaseInvoiceSaveReturnMaster.SupplierName AS CustomerName, dbo.Sales_PurchaseInvoiceSaveReturnMaster.Notes"
                + " FROM     dbo.Sales_PurchaseInvoiceSaveReturnMaster INNER JOIN"
                  + "  dbo.Sales_PurchaseSaveInvoiceReturnDetails ON dbo.Sales_PurchaseInvoiceSaveReturnMaster.InvoiceID = dbo.Sales_PurchaseSaveInvoiceReturnDetails.InvoiceID left outer JOIN"                 
                  + "  dbo.Acc_CostCenters ON dbo.Sales_PurchaseInvoiceSaveReturnMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID left outer JOIN"
                  + " dbo.Stc_Stores ON dbo.Sales_PurchaseInvoiceSaveReturnMaster.StoreID = dbo.Stc_Stores.StoreID "
                           
                   +" WHERE "+filter;
                   Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


               }

               catch (Exception ex)
               {
                   SplashScreenManager.CloseForm(false);


                   Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
               }

               return strSQL;

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



                   txtStoreID.Enabled = true;
                   txtCostCenterID.Enabled = true;

                   txtSellerID.Enabled = true;
                 


                   txtCustomerID.Enabled = true;
                  
                   txtFromDate.Enabled = true;
                   txtToDate.Enabled = true;
                

                   txtFromDate.Text = "";
                   txtToDate.Text = "";

              
                   txtTotalGold.Text = "";
                   txtTotalAlmas.Text = "";
                  

               }
               catch (Exception ex)
               {
                   //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
               }


           }




        #endregion


           private void txtGroupChar_KeyPress(object sender, KeyPressEventArgs e)
           {
               // تحقق من الأحرف العربية والأرقام ومفتاح الحذف (Backspace)
               if (!char.IsLetter(e.KeyChar)&& (e.KeyChar != (char)Keys.Back))
               {
                   e.Handled = true;
                   Messages.MsgHand("خطأ في الإدخال", "يجب أن يكون المدخل حرفاً واحداً فقط");
                   return;
               }

               // إذا تم إدخال أكثر من حرف واحد ، تتم إلغاء الأمر
               if (txtGroupChar.Text.Length >= 1)
               {
                   e.Handled = true;
               }
           }
 
           protected override void DoPrint()
           {
               
                   PrintPyCostPrice();
           } 
           void PrintPyCostPrice()
           {
               try
               {
                    
                   Application.DoEvents();
                   SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                   /******************** Report Body *************************/
                   ReportName = "‏‏rptPurchaseInvoiceSaveReturnReport";
                   bool IncludeHeader = true;
                   string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                   XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                   /********************** Master *****************************/
                   rptForm.RequestParameters = false;
              
                   /********Total*********/
                   rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                   rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                   rptForm.Parameters["CostCenter"].Value = lblCostCenterName.Text.Trim().ToString();
                   rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                   rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                   rptForm.Parameters["SellerName"].Value = lblSellerName.Text.Trim().ToString();

                   
               //    rptForm.Parameters["ReportName"].Value = "سند عرض";

                   for (int i = 0; i < rptForm.Parameters.Count; i++)
                       rptForm.Parameters[i].Visible = false;
                   /********************** Details ****************************/
                   var dataTable = new dsReports.rptPurchaseInvoiceSaveReportDataTable();
                  
                   for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                   {
                       var row = dataTable.NewRow();

                       row["#"] = i + 1;
                       row["InvoiceID"] = gridView1.GetRowCellValue(i, "InvoiceID").ToString();
                       row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                       row["ItemName"] = gridView1.GetRowCellValue(i, "ItemName").ToString();
                       row["invoiceDate"] = gridView1.GetRowCellValue(i, "InvoiceDate").ToString();
                      
                       row["CostPrice"] = gridView1.GetRowCellValue(i, "CostPrice").ToString();


                       
                        
                       row["CustomerName"] = gridView1.GetRowCellValue(i, "CustomerName").ToString();

                  

                       row["StoreName"] = gridView1.GetRowCellValue(i, "StorName").ToString();
                       row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterName").ToString();

                       row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();

                       row["DIAMOND"] = gridView1.GetRowCellValue(i, "DIAMOND_W").ToString();
                       row["STONE"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();
                       row["BAGET"] = gridView1.GetRowCellValue(i, "BAGET_W").ToString();



                       dataTable.Rows.Add(row);
                   }
                   rptForm.DataSource = dataTable;
                   rptForm.DataMember = ReportName;
                   /******************** Report Binding ************************/
                   XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                   subreport.Visible = IncludeHeader;
                   subreport.ReportSource = ReportComponent.CompanyHeader();
                   rptForm.ShowPrintStatusDialog = false;
                   rptForm.ShowPrintMarginsWarning = false;
                   rptForm.CreateDocument();

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
                       if (dt.Rows.Count > 0)
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
                           decimal Qty = 0; decimal Daimond = 0; decimal Stone = 0; decimal Bagit = 0; decimal CostPrice = 0;  

                           for (int i = 0; i <= dt.Rows.Count - 1; i++)
                           {
                               row = _sampleData.NewRow();
                               row["Sn"] = _sampleData.Rows.Count + 1;
                               row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                               row["InvoiceID"] = dt.Rows[i]["InvoiceID"].ToString();
                               row["Notes"] = dt.Rows[i]["Notes"].ToString();
                               row["QTY"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["QTY"].ToString());
                               row["InvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["InvoiceDate"].ToString());

                               row["ItemName"] = dt.Rows[i]["ItemName"].ToString();
                               
                               row["CostPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["CostPrice"]).ToString("N" + 2);
                           

                               row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                               
                               row["StorName"] = dt.Rows[i]["StorName"];
                               row["CostCenterName"] = (dt.Rows[i]["CostCenterName"].ToString() != string.Empty ? dt.Rows[i]["CostCenterName"] : "");
                               //row["Mobile"] = (dt.Rows[i]["SaleDelegateName"].ToString() != string.Empty ? dt.Rows[i]["SaleDelegateName"] : "");
                               row["Mobile"] = dt.Rows[i]["Mobile"];


                               row["DIAMOND_W"] = (dt.Rows[i]["DIAMOND_W"].ToString() != string.Empty ? dt.Rows[i]["DIAMOND_W"] : "");
                               row["STONE_W"] = (dt.Rows[i]["STONE_W"].ToString() != string.Empty ? dt.Rows[i]["STONE_W"] : "");
                               row["BAGET_W"] = (dt.Rows[i]["BAGET_W"].ToString() != string.Empty ? dt.Rows[i]["BAGET_W"] : "");
                               total += Comon.ConvertToDecimalPrice(row["Net"]);
                                
                                   _sampleData.Rows.Add(row);
                               Qty += Comon.ConvertToDecimalPrice(row["QTY"]);
                               Daimond += Comon.ConvertToDecimalPrice(row["DIAMOND_W"]);
                               Stone += Comon.ConvertToDecimalPrice(row["STONE_W"]);
                               Bagit += Comon.ConvertToDecimalPrice(row["BAGET_W"]);
                            
                               CostPrice += Comon.ConvertToDecimalPrice(row["CostPrice"]);

                           }
                           txtTotalGold.Text = Qty + "";
                           txtTotalAlmas.Text = Daimond + "";
                           txtTotalBagit.Text = Bagit + "";
                           txtTotalStone.Text = Stone + "";
                           txtTotalCostPrice.Text = CostPrice + "";
                           
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
          

           private void simpleButton3_Click_1(object sender, EventArgs e)
           {
               SalesInvoice();
               gridControl1.DataSource = _sampleData;

               if (gridView1.RowCount > 0)
               {
                   simpleButton3.Visible = true;

                   txtStoreID.Enabled = false;
                   txtCostCenterID.Enabled = false;

                   txtSellerID.Enabled = false;

                   txtCustomerID.Enabled = false;

                   txtFromDate.Enabled = false;
                   txtToDate.Enabled = false;

                 //  Validations.DoReadRipon(this, ribbonControl1);

               }
               else
               {

                   Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");

                   simpleButton3.Visible = true;
                   DoNew();
               }

           }

         


       
          


          

    }
}
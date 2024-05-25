﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Edex.GeneralObjects.GeneralForms;
using Edex.Model;
using System.IO;
using System.Globalization;
using DevExpress.XtraGrid.Menu;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraGrid;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.StockObjects.Codes;
using Edex.AccountsObjects.Codes;
using Edex.DAL;
using DevExpress.XtraReports.UI;
using Edex.DAL.Accounting;
using System.Data.SqlClient;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.SalseSystem;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmCashierPurchaseSaveDaimondReturn : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        CompanyHeader cmpheader = new CompanyHeader();
        public int DiscountCustomer = 0;
        #region Declare
        public const int DocumentType = 14;
        int Caliber = 18;
        bool IdPrint = false;
        string MethodName = "";
        string invoiceNo = " ";
        int MethodID = 0;
        DataTable dtDeclaration;
        int flagError = 0;
        DataTable dtSize;
        string barcodeLast = "";
        int rowIndex;
        public string strQty="";
        string QualityCasher;
        string FocusedControl = "";
        private string strSQL;
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
        private string GroupName;
        private string CaptionBarCode;
        private string CaptionItemID;
        private string CaptionItemName;
        private string CaptionSizeID;
        private string CaptionSizeName;
        private string CaptionExpiryDate;
        private string CaptionQTY;
        private string CaptionTotal;
        private string CaptionDiscount;
        private string CaptionAdditionalValue;
        private string CaptionNet;
        private string CaptionSalePrice;
        private string CaptionCartPrice;
        private string CaptionCostPrice;
    

        private string CaptionDescription;
        private string CaptionHavVat;
        private string CaptionRemainQty;
        DataTable dVat = new DataTable();
        public MemoryStream TheImage;
        public bool IsNewRecord;
        private  Sale_CashierPurchaseSaveDaimondReturnDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;
        bool falgPrint = false;
        public decimal QtyItem = 0;
        public CultureInfo culture = new CultureInfo("en-US");
        OpenFileDialog OpenFileDialog1 = null;
        DataTable dt = new DataTable();
        GridViewMenu menu;
        int ItemIDImage =0 ;
        public int GoldUsing = 4;

        int StoreItemID = 1;
        //all record master and detail
        BindingList<Sales_PurchaseSaveInvoiceReturnDetails> AllRecords = new BindingList<Sales_PurchaseSaveInvoiceReturnDetails>();

        //list detail
        BindingList<Sales_PurchaseSaveInvoiceReturnDetails> lstDetail = new BindingList<Sales_PurchaseSaveInvoiceReturnDetails>();

        //Detail
        Sales_PurchaseSaveInvoiceReturnDetails BoDetail = new Sales_PurchaseSaveInvoiceReturnDetails();
        string VAt = "Select CompanyVATID from  VATIDCOMPANY ";
        #endregion
        public frmCashierPurchaseSaveDaimondReturn()
        {
            try
            {

            InitializeComponent();
            
         
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                

                GroupName = "ArbGroupName";
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
               
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "العيار";
                CaptionExpiryDate = "تاريخ الصلاحية";
                CaptionQTY = "الوزن";
                CaptionTotal = "الإجمالي";
                CaptionDiscount = "الخصم";
                CaptionAdditionalValue = "الضريبة";
                CaptionNet = "الصافي";

                CaptionCartPrice = "سعر الكرت";
                CaptionCostPrice = "سعر تكلفة";
                CaptionSalePrice = " تكلفةالمحل";
                

                CaptionDescription = "البيان";
                CaptionHavVat = "عليه ضريبة";
                CaptionRemainQty = "الكمية المتبقية";

                lblNetBalance.BackColor = Color.WhiteSmoke;
               // lblNetBalance.ForeColor = Color.Black;
                strSQL = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    GroupName = "EngGroupName";
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    PrimaryName = "EngName";
                    CaptionBarCode = "Bar Code";
                    CaptionItemID = "Item ID";
                    CaptionItemName = "ItemName";
                    CaptionSizeID = "Size ID ";
                    CaptionSizeName = "Size Name";
                    CaptionExpiryDate = "Expiry Date";
                    CaptionQTY = "Quantity";
                    CaptionTotal = "Total";
                    CaptionDiscount = "Discount";
                    CaptionAdditionalValue = "Additional Value";
                    CaptionNet = "Net";
                    CaptionSalePrice = "Sale Price";
                    CaptionDescription = "Description";
                    CaptionHavVat = "Has VAT";
                    CaptionRemainQty = "Quantity Remaining";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");
                    
                    labelControl33.Text = labelControl33.Tag.ToString();
                    labelControl29.Text = labelControl29.Tag.ToString();
                    labelControl34.Text = labelControl34.Tag.ToString();
                    labelControl25.Text = labelControl25.Tag.ToString();
                    labelControl26.Text = labelControl26.Tag.ToString();
                    labelControl27.Text = labelControl27.Tag.ToString();
                    labelControl7.Text = labelControl7.Tag.ToString();
                    labelControl10.Text = labelControl10.Tag.ToString();
                    labelControl11.Text = labelControl11.Tag.ToString();
                }
                
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
                FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                //FillCombo.FillComboBox(cmbCurency, "Currency", "CurrencyID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
               // FillCombo.FillComboBox(cmbFormPrinting, "FormPrinting", "FormID", PrimaryName, "", " 1=1 ", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
               // FillCombo.FillComboBox(cmbBank, "[Acc_Banks]", "ID", PrimaryName, "", " 1=1 ", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                /***********************Component ReadOnly  ****************************/

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("NO", typeof(string)));
                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                DataRow row;
                row = dt.NewRow();
                row["NO"] = 0;
                row["Name"] = "---";
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["NO"] = 1;
                row["Name"] = "عربي";
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["NO"] = 2;
                row["Name"] = "English ";
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["NO"] = 3;
                row["Name"] = "عربي-English";
                dt.Rows.Add(row);

                cmbLanguagePrint.Properties.DataSource = dt.DefaultView;
                cmbLanguagePrint.Properties.DisplayMember = "Name";
                cmbLanguagePrint.Properties.ValueMember = "NO";

                cmbLanguagePrint.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
                cmbLanguagePrint.EditValue = 1;
                TextEdit[] txtEdit = new TextEdit[16];
                txtEdit[0] = lblStoreName;
                txtEdit[1] = lblStoreName;
                txtEdit[2] = lblCostCenterName;
                
                txtEdit[4] = lblDelegateName;
                txtEdit[5] = lblDebitAccountName;
                txtEdit[6] = lblCreditAccountName;
                txtEdit[7] = lblAdditionalAccountName;
                txtEdit[8] = lblDiscountDebitAccountName;
                txtEdit[9] = lblNetAccountName;
                txtEdit[10] = txtEnteredByUserID;
                txtEdit[11] = lblChequeAccountName;
                txtEdit[12] = lblEditedByUserName;
                txtEdit[13] = lblEnteredByUserName;
                txtEdit[14] = txtEditedByUserID;
                txtEdit[15] = lblSellerName;
                //foreach (TextEdit item in txtEdit)
                //{
                //    item.ReadOnly = true;
                //    item.Enabled = false;
                //    item.Properties.AppearanceDisabled.ForeColor = Color.Black;
                //    item.Properties.AppearanceDisabled.BackColor = Color.WhiteSmoke;
                //}
                /*********************** Date Format dd/MM/yyyy ****************************/
                InitializeFormatDate(txtInvoiceDate);
                InitializeFormatDate(txtWarningDate);
                InitializeFormatDate(txtCheckSpendDate);

                /************************  Form Printing ***************************************/
                cmbFormPrinting.EditValue = Comon.cInt(MySession.GlobalDefaultSaleFormPrintingID);

                /*********************** Roles From ****************************/
                txtInvoiceDate.ReadOnly =false;
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmSaleStoreID;
                txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCostCenterID;
                cmbMethodID.ReadOnly = !MySession.GlobalAllowChangefrmSalePayMethodID;
                cmbNetType.ReadOnly = !MySession.GlobalAllowChangefrmSaleNetTypeID;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmSaleCurencyID;
                
                txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDelegateID;
                txtSellerID.ReadOnly = false;
                /************TextEdit Account ID ***************/
                lblDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDebitAccountID;
                lblCreditAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCreditAccountID;
                lblAdditionalAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleAdditionalAccountID;
                lblChequeAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleChequeAccountID;
                lblDiscountDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID;
                lblNetAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleNetAccountID;
                /************ Button Search Account ID ***************/
                RolesButtonSearchAccountID();
                /********************* Event For Account Component ****************************/
                this.txtSupplierInvoiceID.Validating += txtSupplierInvoiceID_Validating;
                this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);
                this.btnCreditSearch.Click += new System.EventHandler(this.btnCreditSearch_Click);
                this.btnAdditionalSearch.Click += new System.EventHandler(this.btnAdditionalSearch_Click);
                this.btnNetSearch.Click += new System.EventHandler(this.btnNetSearch_Click);
                this.btnChequeSearch.Click += new System.EventHandler(this.btnChequeSearch_Click);
                this.btnDiscountDebitSearch.Click += new System.EventHandler(this.btnDiscountCreditSearch_Click);

                this.lblDebitAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblDebitAccountID_Validating);
                this.lblCreditAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblCreditAccountID_Validating);
                this.lblAdditionalAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblAdditionalAccountID_Validating);
                this.lblDiscountDebitAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblDiscountCreditAccountID_Validating);
                this.lblNetAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblNetAccountID_Validating);
                this.lblChequeAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblChequeAccountID_Validating);

                


                this.lblDebitAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblCreditAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblAdditionalAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblDiscountDebitAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblNetAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblChequeAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);


                /********************* Event For TextEdit Component **************************/
                if (MySession.GlobalAllowWhenEnterOpenPopup)
                {
                    this.txtInvoiceDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                    this.txtCheckSpendDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                    this.txtWarningDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);

                    this.cmbMethodID.Enter += new System.EventHandler(this.cmbMethodID_Enter);
                    this.cmbCurency.Enter += new System.EventHandler(this.PublicCombox_Enter);
                    this.cmbNetType.Enter += new System.EventHandler(this.PublicCombox_Enter);
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtInvoiceDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.txtCheckSpendDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.txtWarningDate.Click += new System.EventHandler(this.PublicTextEdit_Click);

                    this.cmbMethodID.Click += new System.EventHandler(this.cmbMethodID_Click);
                    this.cmbCurency.Click += new System.EventHandler(this.PublicCombox_Click);
                    this.cmbNetType.Click += new System.EventHandler(this.PublicCombox_Click);
                }


                this.txtInvoiceID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtStoreID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
               
                this.txtCheckID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetProcessID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetAmount.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                this.cmbMethodID.EditValueChanged += new System.EventHandler(this.cmbMethodID_EditValueChanged);
                this.cmbNetType.EditValueChanged += new System.EventHandler(this.cmbNetType_EditValueChanged);

                this.cmbBank.EditValueChanged += new System.EventHandler(this.cmbBank_EditValueChanged);


                this.chkForVat.EditValueChanged += new System.EventHandler(this.chForVat_EditValueChanged);

                this.txtDiscountOnTotal.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountOnTotal_Validating);
                this.txtDiscountPercent.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountPercent_Validating);
                this.txtInvoiceID.Validating += new System.ComponentModel.CancelEventHandler(this.txtInvoiceID_Validating);
                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
                this.txtSellerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSellerID_Validating);
                this.txtEnteredByUserID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEnteredByUserID_Validating);
                this.txtEditedByUserID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEditedByUserID_Validating);
                this.txtPaidAmount.Validating += new System.ComponentModel.CancelEventHandler(this.txtPaidAmount_Validating);
                this.txtSupplierID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSupplierID_Validating);
                this.txtDiscountOnTotal.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDiscountPercent.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);


                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
              //  this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSaleInvoice_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
                this.gridView1.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridView1_FocusedColumnChanged);
                this.gridView1.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanging);
                this.gridView1.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                this.gridView1.PopupMenuShowing += gridView1_PopupMenuShowing;
                /******************************************/

                this.KeyDown+=frmCashierPurchaseSaveDaimondReturn_KeyDown;
                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);


                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = UserInfo.BRANCHID;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
                
                DoNew();

                txtCost.Text = MySession.Cost.ToString();
                txtsumvalue.Text = MySession.sumvalue.ToString();

                SplashScreenManager.CloseForm(false);
            }
            catch (Exception ex)
            {

                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


        }
         

        public static long GetNewDialyID(int FacilityID, int BranchID, int USERCREATED)
        {
            long ID = 0;
            DataTable dt;
            string strSQL;

            strSQL = "SELECT Max(DailyID )+1 FROM  Sales_SalesInvoiceMaster Where  BranchID =" + BranchID + " And CostCenterID=" + USERCREATED;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                ID = Comon.cLong(dt.Rows[0][0].ToString());
                if (ID == 0) ID = 1;
            }
            return ID;
        }


        
        
      public  void txtSupplierInvoiceID_Validating(object sender, CancelEventArgs e)
        {
            if (txtSupplierInvoiceID != null && !string.IsNullOrWhiteSpace(txtSupplierInvoiceID.Text))
            {
                strSQL = "Select * from Sales_PurchaseInvoiceSaveReturnMaster where  SupplierInvoiceID=" + txtSupplierInvoiceID.Text + " And BranchID=" + MySession.GlobalBranchID + " And  Cancel=0";
                DataTable dtReturn = new DataTable();
                dtReturn = Lip.SelectRecord(strSQL);
                if (dtReturn.Rows.Count > 0)
                {

                    txtInvoiceID.Text = dtReturn.Rows[0]["InvoiceID"].ToString();

                    txtInvoiceID_Validating(null, null);
                    return;
                }
            }
}
        #region GridView
         void InitGrid()
        {
            lstDetail = new BindingList<Sales_PurchaseSaveInvoiceReturnDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;
            /******************* Columns Visible=false ********************/
            gridView1.Columns["BranchID"].Visible = false;
   
            gridView1.Columns["BAGET_W"].Visible = true;
            gridView1.Columns["STONE_W"].Visible = true;
            gridView1.Columns["DIAMOND_W"].Visible = true;

            gridView1.Columns["Equivalen"].Visible = false;
            gridView1.Columns["Caliber"].Visible = false;
            gridView1.Columns["SalePrice"].Visible = true;
         
            gridView1.Columns["Bones"].Visible = false;
            gridView1.Columns["Height"].Visible = false;
            gridView1.Columns["Width"].Visible = false;
            gridView1.Columns["TheCount"].Visible = false;

            gridView1.Columns["GroupID"].Visible = false;
            gridView1.Columns["CaratPrice"].Visible = false;

            gridView1.Columns["AdditionaAmmount"].Visible = false;
            gridView1.Columns["ItemStatus"].Visible = false;
            gridView1.Columns["PageNo"].Visible = false;

            gridView1.Columns["InvoiceID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["StoreID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["PurchaseSaveReturnMaster"].Visible = false;
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["BarCode"].Visible = true;
            gridView1.Columns["ExpiryDate"].Visible = false ;
            gridView1.Columns["Description"].Visible = false;

            gridView1.Columns[GroupName].Visible = true;

            gridView1.Columns["ArbGroupName"].Visible = gridView1.Columns["ArbGroupName"].Name == "col" + GroupName ? true : false;
            gridView1.Columns["EngGroupName"].Visible = gridView1.Columns["EngGroupName"].Name == "col" + GroupName ? true : false;

            gridView1.Columns["Total"].Visible = false;
            gridView1.Columns["AdditionalValue"].Visible = false;
            gridView1.Columns["Net"].Visible = false;

            gridView1.Columns["Color"].Caption = "اللون";
            gridView1.Columns["CLARITY"].Caption = "النقاء";
            gridView1.Columns["Bones"].Caption = "الأجـور";

            /******************* Columns Visible=true *******************/
            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;
            gridView1.Columns["SizeID"].Visible = false;
            gridView1.Columns["Discount"].Visible = false;
            gridView1.Columns["HavVat"].Visible = false;
         
            gridView1.Columns["ItemID"].Visible = false;
            gridView1.Columns["GroupID"].Visible = false;

            gridView1.Columns["SalePrice"].Caption = "سعر البيع";
            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 150;

            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["QTY"].Caption = CaptionQTY;
            gridView1.Columns["STONE_W"].Caption = "الأحجار";
            gridView1.Columns["BAGET_W"].Caption = "الباجيت";
            gridView1.Columns["DIAMOND_W"].Caption = "الألماس";
            gridView1.Columns["Total"].Caption = CaptionTotal;
            gridView1.Columns["Discount"].Caption = CaptionDiscount;
            gridView1.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView1.Columns["Net"].Caption = CaptionNet;

            gridView1.Columns["CostPrice"].Caption = CaptionCostPrice;
            gridView1.Columns["SalePrice"].Caption = CaptionCartPrice;
            gridView1.Columns["CaratPrice"].Caption = CaptionSalePrice;
      
            gridView1.Columns["Description"].Caption = CaptionDescription;
            gridView1.Columns["HavVat"].Caption = CaptionHavVat;
          

            gridView1.Columns["GroupID"].Caption = "رقم المجموعة";
           gridView1.Columns[GroupName].Caption = "اسم المجموعة";

            gridView1.Columns["Serials"].Caption = "رقم المرجع";
            gridView1.Focus();
            /*************************Columns Properties ****************************/
            //  gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
            gridView1.Columns["BarCode"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["Total"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["Total"].OptionsColumn.AllowFocus = false;
           // gridView1.Columns["Net"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
           // gridView1.Columns["Net"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            gridView1.Columns["AdditionalValue"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["AdditionalValue"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["QTY"].OptionsColumn.ReadOnly = false;

            /************************ Date Time **************************/

            RepositoryItemDateEdit RepositoryDateEdit = new RepositoryItemDateEdit();
            RepositoryDateEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            RepositoryDateEdit.Mask.EditMask = "dd/MM/yyyy";
            RepositoryDateEdit.Mask.UseMaskAsDisplayFormat = true;
            gridControl.RepositoryItems.Add(RepositoryDateEdit);
            gridView1.Columns["ExpiryDate"].ColumnEdit = RepositoryDateEdit;
            gridView1.Columns["ExpiryDate"].UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            gridView1.Columns["ExpiryDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.Columns["ExpiryDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridView1.Columns["ExpiryDate"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["ExpiryDate"].OptionsColumn.ReadOnly = false;
            /************************ Look Up Edit **************************/
            /////////////////////////Item COLOR
            ///
            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName   FROM Stc_ItemsColors");
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            gridControl.RepositoryItems.Add(riComboBoxitems);
            gridView1.Columns["Color"].ColumnEdit = riComboBoxitems;
            /////////////////////////


            /////////////////////////Item CLARITY
            DataTable dtitemsCLARITY = Lip.SelectRecord("SELECT   ArbName   FROM Stc_ItemsSizes");
            string[] companiesitemsCLARITY = new string[dtitemsCLARITY.Rows.Count];
            for (int i = 0; i <= dtitemsCLARITY.Rows.Count - 1; i++)
                companiesitemsCLARITY[i] = dtitemsCLARITY.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitemsCLARITY = new RepositoryItemComboBox();
            riComboBoxitemsCLARITY.Items.AddRange(companiesitemsCLARITY);

            gridControl.RepositoryItems.Add(riComboBoxitemsCLARITY);
            gridView1.Columns["CLARITY"].ColumnEdit = riComboBoxitemsCLARITY;
            /////////////////////////

            /////////////////////////Description
            DataTable dt = Lip.SelectRecord("SELECT ArbName FROM Stc_ItemsGroups WHERE Cancel=0");
            string[] companies = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companies[i] = dt.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBox = new RepositoryItemComboBox();
            riComboBox.Items.AddRange(companies);
            gridControl.RepositoryItems.Add(riComboBox);
            gridView1.Columns["Description"].ColumnEdit = riComboBox;
            gridView1.Columns["Description"].Width = 120;
            gridView1.Columns[ItemName].Width = 120;

            gridView1.Columns["BarCode"].Width = 90;
            gridView1.Columns["SalePrice"].Width = 96;
            ///////////////////
            ///

            /////////////////////////Item
            ///

            string[] companiesGroupitems = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companiesGroupitems[i] = dt.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxGroupitems = new RepositoryItemComboBox();
            riComboBoxGroupitems.Items.AddRange(companiesGroupitems);
            gridControl.RepositoryItems.Add(riComboBoxGroupitems);
            gridView1.Columns[GroupName].ColumnEdit = riComboBoxGroupitems;
            gridView1.Columns[GroupName].Width = 120;
            ///////////////////////////


            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 60;
            gridView1.BestFitColumns();

          
            gridView1.Columns[GroupName].VisibleIndex = 3;

            gridView1.Columns["SalePrice"].VisibleIndex = 18;


            /******************************** Menu ***************************************/
            menu = new GridViewMenu(gridView1);
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("أسعار الصنف", new EventHandler(Price_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("بيانات الصنف", new EventHandler(item_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("كرت الصنف"));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("باركود الصنف"));

        }
         void item_Click(object sender, EventArgs e) { }
        private void Price_Click(object sender, EventArgs e)
        {
            frmItemPricesAndCosts frm = new frmItemPricesAndCosts();
            var ItemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID");
            var SizeID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SizeID");
            frm.SizeID = Comon.cInt(SizeID);
            frm.ItemID = Comon.cLong(ItemID);
            
            frm.ShowDialog();
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], Comon.ConvertToDecimalPrice(frm.CelValue));

        }

        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo != null && (e.HitInfo.Column.Name == "colSalePrice" || e.HitInfo.Column.Name == "colExpiryDate"))
                    if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
                        e.Menu = menu;
            }
            catch { }
        }
        private void gridView1_ShownEditor(object sender, EventArgs e)
        {
            if (this.gridView1.ActiveEditor is CheckEdit)
                if (chkForVat.Checked)
                {
                    GridView view = sender as GridView;

                    view.ActiveEditor.IsModified = true;
                    view.ActiveEditor.ReadOnly = false;
                }
            HasColumnErrors = false;


            //CalculateRow();
        }
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (!gridView1.IsLastVisibleRow)
                    gridView1.MoveLast();
                if (DiscountCustomer != 0)
                {
                    txtDiscountPercent.Text = DiscountCustomer.ToString();
                    txtDiscountPercent_Validating(null, null);
                }
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "Net" || col.FieldName == "Total" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID" || col.FieldName == "SalePrice")
                    {

                        var val = gridView1.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                        else if (!(double.TryParse(val.ToString(), out num)) && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }
                        else
                        {
                            e.Valid = true;
                            gridView1.SetColumnError(col, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (this.gridView1.ActiveEditor is CheckEdit)
            {
                if (e.Value != null)
                {
                    gridView1.Columns["HavVat"].OptionsColumn.AllowEdit = true;
                    CalculateRow(gridView1.FocusedRowHandle, Comon.cbool(e.Value.ToString()));
                }
            }


            else if (this.gridView1.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "Bones" ||ColName == "BarCode" || ColName == "Net" || ColName == "SizeID" || ColName == "Total" || ColName == "ItemID" || ColName == "QTY" || ColName == "CostPrice")
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else if (!(double.TryParse(val.ToString(), out num)) && ColName != "BarCode")
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && ColName != "BarCode" && ColName != "Bones")
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }
                    else
                    {
                        e.Valid = true;
                        view.SetColumnError(gridView1.Columns[ColName], "");

                    }
                    /****************************************/
                    if ( ColName == "Bones")
                    {
                        bool HasVat = chkForVat.Checked;
                        decimal QTY = 1;
                        decimal Bones  = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(QTY) * Comon.ConvertToDecimalPrice(val.ToString()));
                        decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CostPrice").ToString());
                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((CostPrice * MySession.GlobalPercentVat) / 100);
                        if (Bones > 0)
                            additonalVAlue = Comon.ConvertToDecimalPrice((Bones * MySession.GlobalPercentVat) / 100);
                        if (HasVat == false)
                            additonalVAlue = 0;

                        decimal Net = Comon.ConvertToDecimalPrice(CostPrice + additonalVAlue);
                        //سعر تكلفة مع مصاريف
                        decimal SpendPrice = Comon.ConvertToDecimalPrice(CostPrice + Bones);
                        //سعر تكلفة المحل
                        decimal CaratPrice = Comon.ConvertToDecimalPrice(Comon.cDec(SpendPrice  * Comon.cDec(txtCost.Text)));
                        //سعر الكارت وهو البيع
                        decimal SalePrice = Comon.ConvertToDecimalPrice(CaratPrice * Comon.cDec(txtsumvalue.Text));
                        gridView1.SetFocusedRowCellValue("CaratPrice", CaratPrice.ToString());
                    
                        gridView1.SetFocusedRowCellValue("SalePrice", SalePrice.ToString());
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("Total", CostPrice.ToString());
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                        gridView1.SetFocusedRowCellValue("Bones", Bones.ToString());
                        }



                    if (ColName == "CostPrice")
                    {
                        bool HasVat = chkForVat.Checked;
                        decimal QTY = 1;
                        decimal CostPrice = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(QTY) * Comon.ConvertToDecimalPrice(val.ToString()));

                        decimal Bones = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Bones").ToString());

                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((CostPrice * MySession.GlobalPercentVat) / 100);
                        if (Bones > 0)
                            additonalVAlue = Comon.ConvertToDecimalPrice((Bones * MySession.GlobalPercentVat) / 100);
                        if (HasVat == false)
                            additonalVAlue = 0;

                        decimal Net = Comon.ConvertToDecimalPrice(CostPrice + additonalVAlue);
                        //سعر تكلفة مع مصاريف
                        decimal SpendPrice = Comon.ConvertToDecimalPrice(CostPrice + Bones); 
                        //سعر تكلفة المحل

                        decimal CaratPrice = Comon.ConvertToDecimalPrice(Comon.cDec(SpendPrice  * Comon.cDec(txtCost.Text)));
                        //سعر الكارت وهو البيع
                        decimal SalePrice = Comon.ConvertToDecimalPrice(CaratPrice * Comon.cDec(txtsumvalue.Text));

                        gridView1.SetFocusedRowCellValue("CaratPrice", CaratPrice.ToString());
                 
                        gridView1.SetFocusedRowCellValue("SalePrice", SalePrice.ToString());
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("Total", CostPrice.ToString());
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                        gridView1.SetFocusedRowCellValue("Bones", Bones.ToString());

                    }


                    if (ColName == "StoreID")
                    {
                        string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                        int ExpiryDate = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"]).ToString());
                        double Qty = Comon.cDbl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"]).ToString());
                        double RemindQty = 0;
                        if (MySession.GlobalAllowUsingDateItems)
                            RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQtyStockExpiryDate]('" + BarCode + "'," + StoreItemID + ",0,'" + ExpiryDate.ToString() + "') AS RemainQty").Rows[0]["RemainQty"].ToString());
                        else
                            RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQtyStock]('" + BarCode + "'," + StoreItemID + ",0) AS RemainQty").Rows[0]["RemainQty"].ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RemainQty"], RemindQty);
                        if (RemindQty - Qty < 0)
                        {
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                HasColumnErrors = true;
                                e.Valid = false;
                                gridView1.SetColumnError(gridView1.Columns["QTY"], "");
                                e.ErrorText = "لايوجد كمية متبقية في المستودع لهذا الصنف المحدد بتاريخ الصلاحية ";
                            }
                        }
                    }
                    
                   

                    if (ColName == "BarCode")
                    {

                        DataTable dt;
                        var flagb = false;
                        if (MySession.GlobalAllowUsingDateItems)
                        {
                            dt = Stc_itemsDAL.GetItemDataExpiry(val.ToString(), UserInfo.FacilityID);
                            if (dt.Rows.Count == 0)
                            {
                                dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                                flagb = true;

                            }
                        }
                        else
                            dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                           // e.Valid = false;
                           // HasColumnErrors = true;
                           // e.ErrorText = Messages.msgNoFoundThisBarCode;
                        }
                        else
                        {

                            //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
                            //gridView1.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);

                            if (flagb == true)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dt);
                            if (HasColumnErrors == false)
                            {
                                e.Valid = true;
                                view.SetColumnError(gridView1.Columns[ColName], "");
                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                            }
                            //else {
                            //   // HasColumnErrors = true;
                            //    e.Valid = false;
                            //    gridView1.SetColumnError(gridView1.Columns["QTY"], "");
                            //    e.ErrorText = "الكمية غير متوفرة";
                            //    gridView1.FocusedColumn = gridView1.VisibleColumns[6];

                            //}
                        }
                    }
                    else if (ColName == "ItemID")
                    {
                        DataTable dt = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(val.ToString()), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisBarCode;

                        }
                        else
                        {


                            //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(val.ToString()));
                            //gridView1.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dt);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }







                    }
                    else if (ColName == "SizeID")
                    {
                        int ItemID = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]).ToString());
                        DataTable dt = Stc_itemsDAL.GetItemDataByItemID_SizeID(ItemID, Comon.cInt(val.ToString()), UserInfo.FacilityID);

                        if (dt == null || dt.Rows.Count == 0)
                        {

                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundSizeForItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {
                            //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dt.Rows[0]["ItemID"].ToString()));
                            //gridView1.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dt);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                }
                else if (ColName == GroupName)
                {
                    DataTable dtGroupID = Lip.SelectRecord("Select GroupID, ArbName from Stc_ItemsGroups Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "')");
                    if (dtGroupID.Rows.Count > 0)
                    {
                        if (dtGroupID.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {

                           gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["GroupID"], dtGroupID.Rows[0]["GroupID"].ToString());
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtGroupID.Rows[0]["ArbName"].ToString());

                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                    else
                    {
                        //e.Valid = false;
                        //HasColumnErrors = true;
                        //e.ErrorText = Messages.msgNoFoundThisItem;
                        //view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }

                else if (ColName == "GroupID")
                {
                    DataTable dtGroupID = Lip.SelectRecord("Select GroupID, ArbName from Stc_ItemsGroups Where Cancel=0 And  GroupID = " + val.ToString() + "");
                    if (dtGroupID.Rows.Count > 0)
                    {
                        if (dtGroupID.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {

                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dtGroupID.Rows[0]["ArbName"].ToString());
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                    else
                    {
                        //e.Valid = false;
                        //HasColumnErrors = true;
                        //e.ErrorText = Messages.msgNoFoundThisItem;
                        //view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }

                else if (ColName == SizeName)
                {
                    DataTable dtSize = Lip.SelectRecord("Select SizeID, " + PrimaryName + " AS " + SizeName + " from Stc_SizingUnits Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtSize.Rows.Count > 0)
                    {
                        var ItemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID");
                        if (ItemID != null)
                        {
                            DataTable dt = Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(ItemID.ToString()), Comon.cInt(dtSize.Rows[0]["SizeID"].ToString()), UserInfo.FacilityID);
                            if (dt.Rows.Count == 0)
                            {
                                //e.Valid = false;
                                //HasColumnErrors = true;
                                //e.ErrorText = Messages.msgNoFoundSizeForItem;
                            }
                            else
                            {
                                if (MySession.GlobalAllowUsingDateItems)
                                {
                                    MySession.GlobalAllowUsingDateItems = false;
                                    FileItemData(dt);
                                    MySession.GlobalAllowUsingDateItems = true;
                                }
                                else
                                    FileItemData(dt);
                                e.Valid = true;
                                view.SetColumnError(gridView1.Columns[ColName], "");
                            }
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgInputIsRequired;
                            view.SetColumnError(gridView1.Columns["ItemID"], Messages.msgNoFoundSizeForItem);
                        }

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundSizeForItem);

                    }
                }
                else if (ColName == "Discount")
                {
                    decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString());
                    decimal SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SalePrice").ToString());
                    decimal Total = QTY * SalePrice;
                    decimal PercentDiscount = Total * (MySession.GlobalDiscountPercentOnItem / 100);
                    if (!(double.TryParse(val.ToString(), out num)))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) > 0 && (MySession.GlobalDiscountPercentOnItem <= 0)) { Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount); return; }

                    else if (Comon.ConvertToDecimalPrice(val.ToString()) > PercentDiscount)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNotAllowedPercentDiscount;
                    }
                }
            }
            CalculateRow();
        }
        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e)
        {
            try
            {
                var grid = sender as GridControl;
                var view = grid.FocusedView as GridView;
                if (view.FocusedColumn == null)
                    return;



                if (e.KeyCode == Keys.Escape)
                {
                    HasColumnErrors = false;
                }
                if (e.KeyValue == 107)
                {
                    if (this.gridView1.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        CalculateRow(gridView1.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));


                    }
                }
                else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {
                    if (view.ActiveEditor is TextEdit)
                    {
                        if (HasColumnErrors == true)
                            return;
                        double num;
                        HasColumnErrors = false;
                        var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                        string ColName = view.FocusedColumn.FieldName;
                        if (ColName == "BarCode" || ColName == "Net" || ColName == "Total" || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID" || ColName == "SalePrice")
                        {

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(gridView1.Columns[ColName], "");
                            }

                            if (ColName == "QTY")
                            {
                                string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                                double RemindQty = 0;
                                RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + BarCode + "'," + Comon.cInt(txtStoreID.Text) + ") AS RemainQty").Rows[0]["RemainQty"].ToString());
                                double Qty = Comon.cDbl(QtyItem);

                                if (RemindQty <= 0)
                                {

                                    if (MySession.GlobalWayOfOutItems == "PreventOutItemsWithOutBalance")
                                    {

                                        HasColumnErrors = true;
                                        view.SetColumnError(gridView1.Columns[ColName], "الكمية  غير متوفرة ");
                                        // view.DeleteRow(view.FocusedRowHandle);
                                        return;
                                    }
                                }

                            }

                        }

                    }
                }
                else if (e.KeyData == Keys.Delete)
                {
                    if (!IsNewRecord)
                    {
                        if (!FormDelete)
                        {
                            Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                            return;
                        }
                        else
                        {
                            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                            if (!Yes)
                                return;
                        }
                    }
                    int index = view.FocusedRowHandle;
                    view.DeleteSelectedRows();
                    e.Handled = true;
                    if (index > 0)
                    {
                        if (index > 0)
                            index = index - 1;
                        else if (index < 0)
                        {
                            index = view.DataRowCount;
                            index = index - 1;
                        }
                        view.SelectRow(index);
                        view.FocusedRowHandle = index;
                    }
                    CalculateRow();
                }
                else if (e.KeyData == Keys.F5)
                    grid.ShowPrintPreview();

            }
            catch (Exception ex)
            {
                e.Handled = false;
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = (e.ListSourceRowIndex + 1);
        }
        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (this.gridView1.ActiveEditor is CheckEdit)
            {
                gridView1.Columns["HavVat"].OptionsColumn.AllowEdit = true;
                CalculateRow(gridView1.FocusedRowHandle, Comon.cbool(e.Value.ToString()));
            }
            if (this.gridView1.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "QTY")
                {
                    QtyItem = Comon.cDec(e.Value);
                    Caliber = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, SizeName).ToString());
                }
            }
        }
        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
           

        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {


                if (e.FocusedRowHandle >= 0)
                {
                    ItemIDImage = Comon.cInt(gridView1.GetRowCellValue(e.FocusedRowHandle, "ItemID").ToString());
                    var dtimg = Lip.SelectRecord("Select * from Stc_Items Where ItemID=" + ItemIDImage);
                    if (dtimg.Rows.Count > 0)
                    {
                        byte[] imgByte = null;
                        if (DBNull.Value != dtimg.Rows[0]["ItemImage"])
                        {
                            imgByte = (byte[])dtimg.Rows[0]["ItemImage"];
                            picItemImage.Image = byteArrayToImage(imgByte);
                        }
                        else
                            picItemImage.Image = null;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void FileItemData(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
        

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName ], dt.Rows[0]["ArbName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());

                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                try
                {
                    if (Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()) <= 0)
                    {

                        if (Comon.ConvertToDecimalPrice(dt.Rows[0]["CostPrice"].ToString()) > 0)
                            dt.Rows[0]["SalePrice"] = dt.Rows[0]["CostPrice"];
                        else
                            dt.Rows[0]["SalePrice"] = 1;

                    }
                }
                catch { };
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], 0);
               
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
      
                try
                {
                    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemImage"], dt.Rows[0]["ItemImage"]);
                }
                catch { }
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Discount"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["STONE_W"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Color"], dt.Rows[0]["Color"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CLARITY"], dt.Rows[0]["CLARITY"].ToString());
                if (dt.Rows[0]["BarCode"].ToString()=="24")
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], false);
                else
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RemainQty"], 0);


            }
            else
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RemindQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["STONE_W"], 0);
            }

        }

      
        private void EnabledControl(bool Value)
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextEdit && ((!(item.Name.Contains("AccountID"))) && (!(item.Name.Contains("AccountName")))))
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {
                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    }
                }
                else if (item is TextEdit && (((item.Name.Contains("AccountID"))) || ((item.Name.Contains("AccountName")))))
                {
                    item.Enabled = Value;
                    ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                    ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    if (Value)
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                }
                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    ((SimpleButton)item).Enabled = Value;
                }
            }
            chkForVat.Enabled = Value;

            chkForVat.Properties.AppearanceDisabled.ForeColor = Color.Black;
            chkForVat.Properties.AppearanceDisabled.BackColor = Color.Transparent;
            foreach (GridColumn col in gridView1.Columns)
            {
                 
               gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                 
                 
            }
            if (Value)
                RolesButtonSearchAccountID();
            cmbFormPrinting.Enabled = true;
        }
        bool IsValidGrid()
        {
            double num;

            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            gridView1.MoveLast();

            int length = gridView1.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "Net" || col.FieldName == "Total" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID" || col.FieldName == "SalePrice")
                    {

                        var cellValue = gridView1.GetRowCellValue(i, col); ;

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;

                        else if (!(double.TryParse(cellValue.ToString(), out num)) )
                        {
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0 )
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #region Calculate
        private void CalculateRow(int Row = -1, bool IsHavVat = false)
        {
            try
            {
                SumTotalBalanceAndDiscount(Row, IsHavVat);
                //Remove Icon Validtion
                var Net = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net");
                var Total = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total");
                if ((Total != null && !(string.IsNullOrWhiteSpace(Total.ToString())) && Comon.ConvertToDecimalPrice(Total.ToString()) > 0))
                    gridView1.SetColumnError(gridView1.Columns["Total"], "");
                if ((Net != null && !(string.IsNullOrWhiteSpace(Net.ToString())) && Comon.ConvertToDecimalPrice(Net.ToString()) > 0))
                    gridView1.SetColumnError(gridView1.Columns["Net"], "");
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void SumTotalBalanceAndDiscount(int row = -1, bool IsHavVat = false)
        {
            try
            {

                decimal TotalAfterDiscount = 0;
                decimal TotalBeforeDiscount = 0;
                decimal Net = 0;
                decimal DiscountTotal = 0;
                decimal DiscountOnTotal = 0;
                decimal AdditionalAmount = 0;
                decimal DiscountRow = 0;
                decimal CaratPriceTotal = 0;
         
                decimal SalePriceTotal = 0;
                decimal OjorTotal = 0;

                
                decimal QTYRow = 0;
                decimal QTY18 = 0;
                decimal QTY21 = 0;

                decimal QTY22 = 0;
                decimal QTY24 = 0;

                decimal InvoiceTotalGold = 0;

                decimal SalePriceRow = 0;
                decimal TotalRow = 0;
                decimal NetRow = 0;
                decimal TotalBeforeDiscountRow = 0;
                decimal AdditionalAmountRow = 0;
                decimal TotalDaimond_W = 0;
                decimal TotalStown_W = 0;
                decimal TotalBagat_W = 0;
                decimal TotalGold_W = 0;
                bool HavVatRow = false;
                decimal  Gold_W = 0;
                MySession.UseNetINInvoiceSales = 1;
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    int Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                    QTYRow = 1;

                    Gold_W = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());

                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                    AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(SalePriceRow);

                    TotalDaimond_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DIAMOND_W"));
                    TotalStown_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "STONE_W"));
                    TotalBagat_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "BAGET_W"));
                    OjorTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones"));
                    TotalGold_W += Gold_W;


                    TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                    AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    TotalBeforeDiscountRow = TotalRow;

                    CaratPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CaratPrice"));
           
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice"));


                    if (Caliber == 18)
                        QTY18 = QTY18 + QtyItem  ;
                    if (Caliber == 21)
                        QTY21 = QTY21 + QtyItem  ;

                    if (Caliber == 22)
                        QTY22 = QTY22 + QtyItem  ;
                    if (Caliber == 24)
                        QTY24 = QTY24 + QtyItem  ;



                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }

                if (rowIndex < 0)
                {
                    var ResultCaliber = Comon.cInt(gridView1.GetRowCellValue(rowIndex, SizeName));
                    var ResultQTY = 1;
                    var ResultSalePrice = gridView1.GetRowCellValue(rowIndex, "SalePrice");
                    var ResultDiscount = gridView1.GetRowCellValue(rowIndex, "Discount");
                    var ResultHavVat = gridView1.GetRowCellValue(rowIndex, "HavVat");
                    var ResultGold = gridView1.GetRowCellValue(rowIndex, "QTY");

                    

                    QTYRow =   Comon.ConvertToDecimalPrice(ResultQTY.ToString());
                    SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);

                    CaratPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "CaratPrice"));
            
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "SalePrice"));

                    TotalDaimond_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "DIAMOND_W"));
                    TotalStown_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "STONE_W"));
                    TotalBagat_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "BAGET_W"));
                    OjorTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Bones"));
                    TotalGold_W += Comon.ConvertToDecimalPrice(ResultGold);


                    TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Total"));
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue")) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                        TotalBeforeDiscountRow = TotalRow;
                     

                    if (ResultCaliber == 18)
                        QTY18 = QTY18 + Comon.ConvertToDecimalPrice(QtyItem);
                    if (ResultCaliber == 21)
                        QTY21 = QTY21 + Comon.ConvertToDecimalPrice(QtyItem); ;

                    if (ResultCaliber == 22)
                        QTY22 = QTY22 + Comon.ConvertToDecimalPrice(QtyItem); ;
                    if (ResultCaliber == 24)
                        QTY24 = QTY24 + Comon.ConvertToDecimalPrice(QtyItem); ;


                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }
                lblUnitDiscount.Text = DiscountTotal.ToString("N" + MySession.GlobalPriceDigits);
                DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);

                
                Net = Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text) + AdditionalAmount;
                lblAdditionaAmmount.Text = Comon.ConvertToDecimalPrice(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
         

                decimal Eq = 0;


                Eq = Comon.ConvertTo21Caliber(QTY18, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY21, 21);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY22, 22);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY24, 24);

                lblInvoiceTotalGold.Text = Comon.ConvertToDecimalQty(Eq).ToString("N" + MySession.GlobalQtyDigits);

                lbl18.Text = Comon.ConvertToDecimalQty(QTY18).ToString("N" + MySession.GlobalQtyDigits);
                lbl21.Text = Comon.ConvertToDecimalQty(QTY21).ToString("N" + MySession.GlobalQtyDigits);

                lbl22.Text = Comon.ConvertToDecimalQty(QTY22).ToString("N" + MySession.GlobalQtyDigits);
                lbl24.Text = Comon.ConvertToDecimalQty(QTY24).ToString("N" + MySession.GlobalQtyDigits);


                lblTotalSalePrice17.Text = CaratPriceTotal.ToString("N" + MySession.GlobalQtyDigits); ;
     
                lblTotalSalePrice.Text = SalePriceTotal.ToString("N" + MySession.GlobalQtyDigits); ;

                lblTotalStown.Text = TotalStown_W.ToString("N" + MySession.GlobalQtyDigits);
                lblTotalDaimond.Text = TotalDaimond_W.ToString("N" + MySession.GlobalQtyDigits);
                lblTotalBagat.Text = TotalBagat_W.ToString("N" + MySession.GlobalQtyDigits);
                lblInvoiceTotalOjor.Text = OjorTotal.ToString("N" + MySession.GlobalQtyDigits);
                lbl18.Text = TotalGold_W.ToString("N" + MySession.GlobalQtyDigits);

            }

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void SumTotalBalanceAndDiscountread(int row = -1, bool IsHavVat = false)
        {
            try
            {

                decimal TotalAfterDiscount = 0;
                decimal TotalBeforeDiscount = 0;
                decimal Net = 0;
                decimal DiscountTotal = 0;
                decimal DiscountOnTotal = 0;
                decimal AdditionalAmount = 0;
                decimal DiscountRow = 0;
                decimal CaratPriceTotal = 0;
                         decimal SalePriceTotal = 0;

                decimal TotalDaimond_W = 0;
                decimal TotalStown_W = 0;
                decimal  TotalBagat_W = 0;

                decimal QTYRow = 0;
                decimal QTY18 = 0;
                decimal QTY21 = 0;

                decimal QTY22 = 0;
                decimal QTY24 = 0;

                decimal InvoiceTotalGold = 0;

                decimal SalePriceRow = 0;
                decimal TotalRow = 0;
                decimal NetRow = 0;
                decimal TotalBeforeDiscountRow = 0;
                decimal AdditionalAmountRow = 0;
                bool HavVatRow = false;
                MySession.UseNetINInvoiceSales = 1;
                gridView1.MoveLast();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    int Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                    QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                    AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));

                    TotalDaimond_W +=  Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DIAMOND_W"));
                    TotalStown_W +=  Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "STONE_W"));
                    TotalBagat_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "BAGET_W"));

                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
 

                    TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                    AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    TotalBeforeDiscountRow = TotalRow;

                    CaratPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CaratPrice"));
                  
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice"));


                    if (Caliber == 18)
                        QTY18 = QTY18 + QTYRow;
                    if (Caliber == 21)
                        QTY21 = QTY21 + QTYRow;

                    if (Caliber == 22)
                        QTY22 = QTY22 + QTYRow;
                    if (Caliber == 24)
                        QTY24 = QTY24 + QTYRow;



                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }

                if (rowIndex < 0)
                {
                    var ResultCaliber = Comon.cInt(gridView1.GetRowCellValue(rowIndex, SizeName));
                    var ResultQTY = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY");
                    var ResultSalePrice = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SalePrice");
                    var ResultDiscount = gridView1.GetRowCellValue(rowIndex, "Discount");
                    var ResultHavVat = gridView1.GetRowCellValue(rowIndex, "HavVat");
                    QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                    SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);

                    TotalDaimond_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "DIAMOND_W"));
                    TotalStown_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "STONE_W"));
                    TotalBagat_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "BAGET_W"));


                    if (Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net")) > 0 && MySession.UseNetINInvoiceSales == 1)
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Total"));
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue")) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                        TotalBeforeDiscountRow = TotalRow;
                    }
                    else
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);
                    }

                    if (ResultCaliber == 18)
                        QTY18 = QTY18 + Comon.ConvertToDecimalPrice(QTYRow);
                    if (ResultCaliber == 21)
                        QTY21 = QTY21 + Comon.ConvertToDecimalPrice(QTYRow); ;

                    if (ResultCaliber == 22)
                        QTY22 = QTY22 + Comon.ConvertToDecimalPrice(QTYRow); ;
                    if (ResultCaliber == 24)
                        QTY24 = QTY24 + Comon.ConvertToDecimalPrice(QTYRow); ;
                    CaratPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "CaratPrice"));
                 
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "SalePrice"));



                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }
                lblUnitDiscount.Text = DiscountTotal.ToString("N" + MySession.GlobalPriceDigits);
                DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);

                if (DiscountOnTotal > 0)
                {
                    decimal Total = TotalAfterDiscount - DiscountOnTotal;
                    if (AdditionalAmount > 0)
                        AdditionalAmount = (Total) / 100 * MySession.GlobalPercentVat;
                    Net = Comon.ConvertToDecimalPrice(Total + AdditionalAmount);
                }
                Net = Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text) + AdditionalAmount;
                lblAdditionaAmmount.Text = Comon.ConvertToDecimalPrice(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = Comon.ConvertToDecimalPrice(Net).ToString("N" + MySession.GlobalPriceDigits);

                decimal Eq = 0;


                Eq = Comon.ConvertTo21Caliber(QTY18, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY21, 21);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY22, 22);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY24, 24);

                lblInvoiceTotalGold.Text = Comon.ConvertToDecimalQty(Eq).ToString("N" + MySession.GlobalQtyDigits);

                lbl18.Text = Comon.ConvertToDecimalQty(QTY18).ToString("N" + MySession.GlobalQtyDigits);
                lbl21.Text = Comon.ConvertToDecimalQty(QTY21).ToString("N" + MySession.GlobalQtyDigits);

                lbl22.Text = Comon.ConvertToDecimalQty(QTY22).ToString("N" + MySession.GlobalQtyDigits);
                lbl24.Text = Comon.ConvertToDecimalQty(QTY24).ToString("N" + MySession.GlobalQtyDigits);

                lblTotalSalePrice17.Text = CaratPriceTotal.ToString("N" + MySession.GlobalQtyDigits);  
                lblTotalSalePrice.Text = SalePriceTotal.ToString("N" + MySession.GlobalQtyDigits); ;

                lblTotalStown.Text = TotalStown_W.ToString("N" + MySession.GlobalQtyDigits);
                lblTotalDaimond.Text = TotalDaimond_W.ToString("N" + MySession.GlobalQtyDigits);
                lblTotalBagat.Text = TotalBagat_W.ToString("N" + MySession.GlobalQtyDigits); 
            }

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        #endregion
        #region Function
        public void fillMAsterData(DataTable dd)
        {

            txtSupplierInvoiceID.Focus();
            txtStoreID.Text = dd.Rows[0]["StoreID"].ToString();
            txtCostCenterID.Text = dd.Rows[0]["CostCenterID"].ToString();

            StopSomeCode = true;
            cmbMethodID.EditValue = Comon.cInt(dd.Rows[0]["MethodeID"].ToString());
            StopSomeCode = false;
            cmbCurency.EditValue = Comon.cInt(dd.Rows[0]["CurrencyID"].ToString());
            cmbNetType.EditValue = Comon.cDbl(dd.Rows[0]["NetType"].ToString());

            txtSupplierID.Text = dd.Rows[0]["SupplierID"].ToString();
            txtSupplierID_Validating(null, null);
            
            lblSupplierName.Text = dd.Rows[0]["SupplierName"].ToString();


            txtDelegateID.Text = dd.Rows[0]["DelegateID"].ToString();
            txtDelegateID_Validating(null, null);

            txtEnteredByUserID.Text = dd.Rows[0]["UserID"].ToString();
            txtEnteredByUserID_Validating(null, null);

            txtEditedByUserID.Text = dd.Rows[0]["EditUserID"].ToString();
            txtEditedByUserID_Validating(null, null);
            decimal totalqtyEquivalen = 0;

            for (int i = 0; i <= dd.Rows.Count - 1; i++)
            {
                totalqtyEquivalen += Comon.cDec(dd.Rows[i]["Equivalen"].ToString());
            }
            lblInvoiceTotalGold.Text = Comon.cDec(totalqtyEquivalen.ToString()).ToString();


            //Masterdata
            //txtInvoiceID.Text = dd.Rows[0]["InvoiceID"].ToString();
            txtNotes.Text = "مردود سند عرض";
            txtDocumentID.Text = dd.Rows[0]["DocumentID"].ToString();
            txtRegistrationNo.Text = dd.Rows[0]["RegistrationNo"].ToString();

            //Date



            //Ammount

            txtCheckID.Text = dd.Rows[0]["CheckID"].ToString();

            txtNetAmount.Text = dd.Rows[0]["NetAmount"].ToString();
            txtNetProcessID.Text = dd.Rows[0]["NetProcessID"].ToString();

            txtVatID.Text = dd.Rows[0]["VatID"].ToString();

            txtDiscountOnTotal.Text = dd.Rows[0]["DiscountOnTotal"].ToString();

            //حقول محسوبة 
            lblUnitDiscount.Text = "0";
            lblDiscountTotal.Text = "0";

            lblInvoiceTotal.Text = dd.Rows[0]["InvoiceTotal"].ToString();
            txtDiscountOnTotal_Validating(null, null);


            lblAdditionaAmmount.Text = dd.Rows[0]["AdditionaAmountTotal"].ToString();
            lblNetBalance.Text = dd.Rows[0]["NetBalance"].ToString();

            if (Comon.cDbl(lblAdditionaAmmount.Text) > 0)
                chkForVat.Checked = true;
            else
                chkForVat.Checked = false;


            gridControl.DataSource = dd;

            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
           
         //  CalculateRow();


        }

        private void ShortcutOpen()
        {
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;

            if (FocusedControl.Trim() == txtSupplierID.Name)
            {
                frmSuppliers frm = new frmSuppliers();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }

            if (FocusedControl.Trim() == txtStoreID.Name)
            {
                frmStores frm = new frmStores();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                frmCostCenter frm = new frmCostCenter();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                 
            }
            else if (FocusedControl.Trim() == txtSellerID.Name)
            {
                frmSellers frm = new frmSellers();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {

                if (gridView1.FocusedColumn.Name == "colItemID" || gridView1.FocusedColumn.Name == "col" + ItemName || gridView1.FocusedColumn.Name == "colBarCode")
                {
                    frmItems frm = new frmItems();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        {
                            bool b = true;
                        };
                        //frm.Dispose();
                        if (frm.IsDisposed)
                        {
                            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
                            gridView1.Columns[ItemName].ColumnEdit = rItem;
                            gridControl.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                {
                    frmSizingUnits frm = new frmSizingUnits();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                    }
                    else
                        frm.Dispose();
                }
            }
        }
        private void AddRow()
        {
            try
            {
                if ((gridView1.IsNewItemRow(gridView1.FocusedRowHandle)))
                    gridView1.AddNewRow();
            }
            catch (Exception ex)
            {

            }

        }
        #region Other Function
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;


            if (FocusedControl.Trim() == txtSupplierID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "SublierID", "رقم المـــورد", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "SublierID", "SublierID ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Store ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtInvoiceID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtInvoiceID, null, "SalesInvoice", "رقـم الـفـاتـورة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtInvoiceID, null, "SalesInvoice", "Invoice ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtSellerID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleSellerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtSellerID, lblSellerName, "SellerID", "رقم البائع", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtSellerID, lblSellerName, "SellerID", "Seller ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;

                if (gridView1.FocusedColumn.Name == "colDIAMOND_W")
                {
                    frmPurchaseDaimondDetils frm = new frmPurchaseDaimondDetils();
                    frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                    frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                    frm.ReadData(txtInvoiceID.Text, gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BarCode").ToString(), gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DIAMOND_W").ToString(), txtStoreID.Text, txtSupplierID.Text, Comon.cInt(cmbBranchesID.EditValue));
                    frm.ReadRecord(2, 7);
                    frm.Show();
                }
               

                if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                }
                if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                {
                    var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                    var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]);
                    if (itemID != null && Barcode != null)
                    {

                        Condition += " And ItemID=" + Comon.cInt(itemID);
                        if (UserInfo.Language == iLanguage.Arabic)
                            PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "رقـم الـوحـــده", MySession.GlobalBranchID, Condition);
                        else
                            PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "Size ID", MySession.GlobalBranchID, Condition);
                    }
                }
            }
            GetSelectedSearchValue(cls);
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtSupplierID.Name)
                {
                    txtSupplierID.Text = cls.PrimaryKeyValue.ToString();
                    txtSupplierID_Validating(null, null);
                }

                else if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }

                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
                else if (FocusedControl == txtSellerID.Name)
                {
                    txtSellerID.Text = cls.PrimaryKeyValue.ToString();
                    txtSellerID_Validating(null, null);
                }
                else if (FocusedControl == txtInvoiceID.Name)
                {
                    txtInvoiceID.Text = cls.PrimaryKeyValue.ToString();
                    txtInvoiceID_Validating(null, null);
                }


                else if (FocusedControl == txtDelegateID.Name)
                {
                    txtDelegateID.Text = cls.PrimaryKeyValue.ToString();
                    txtDelegateID_Validating(null, null);
                }
                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));
                        CalculateRow();
                        gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
                        Find();
                    }
                    else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                    {

                        int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                        var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]);
                        if (itemID != null && Barcode != null)
                        {

                            if (Stc_itemsDAL.CheckIfStopItemUnit(Comon.cInt(itemID), SizeID, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                            {
                                Messages.MsgStop(Messages.TitleError, Messages.msgWorningThisUnitIsStop);
                                return;
                            }
                            FileItemData(Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(itemID), SizeID, UserInfo.FacilityID));
                            CalculateRow();
                        }

                    }
                }
            }

        }
       
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        public void GetAccountsDeclaration()
        {
            #region get accounts declaration

            List<Acc_DeclaringMainAccounts> AllAccounts = new List<Acc_DeclaringMainAccounts>();
            int BRANCHID = Comon.cInt(cmbBranchesID.EditValue);
            int FacilityID = UserInfo.FacilityID;

            dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(BRANCHID, FacilityID);
            if (dtDeclaration != null && dtDeclaration.Rows.Count > 0)
            {
                //حساب الصندوق
                DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
                if (row.Length > 0)
                {
                    lblDebitAccountID.Text = row[0]["AccountID"].ToString();
                    lblDebitAccountName.Text = row[0]["AccountName"].ToString();
                }
                //حساب مردود المشتريات العروض 
                DataRow[] row2 = dtDeclaration.Select("DeclareAccountName = 'PurchaseSaveReturnAccount'");
                if (row2.Length > 0)
                {
                    lblCreditAccountID.Text = row2[0]["AccountID"].ToString();
                    lblCreditAccountName.Text = row2[0]["AccountName"].ToString();
                }

                //حساب صندوق الذهب المدين
                DataRow[] row1 = dtDeclaration.Select("DeclareAccountName = 'CreditGoldAccountID'");
                if (row1.Length > 0)
                {
                    txtDebitGoldAccountID.Text = row1[0]["AccountID"].ToString();
                    lblDebitGoldAccountName.Text = row1[0]["AccountName"].ToString();
                }

                //حساب صندوق الذهب الدائن
                DataRow[] row8 = dtDeclaration.Select("DeclareAccountName = 'DebitGoldAccountID'");
                if (row8.Length > 0)
                {

                    txtCreditGoldAccountID.Text = row8[0]["AccountID"].ToString();
                    lblCreditGoldAccountName.Text = row8[0]["AccountName"].ToString();
                }

                //حساب الخصم المكتسب
                DataRow[] row3 = dtDeclaration.Select("DeclareAccountName = 'EarnedAccount'");
                if (row3.Length > 0)
                {
                    lblDiscountDebitAccountID.Text = row3[0]["AccountID"].ToString();
                    lblDiscountDebitAccountName.Text = row3[0]["AccountName"].ToString();

                }
                //حساب شبكة 
                DataRow[] row4 = dtDeclaration.Select("DeclareAccountName = 'NetAccount'");
                if (row4.Length > 0)
                {
                    lblNetAccountID.Text = row4[0]["AccountID"].ToString();
                    lblNetAccountName.Text = row4[0]["AccountName"].ToString();

                }
                //حساب الشيكات 
                DataRow[] row5 = dtDeclaration.Select("DeclareAccountName = 'ChequeAccount'");
                if (row5.Length > 0)
                {
                    lblChequeAccountID.Text = row5[0]["AccountID"].ToString();
                    lblChequeAccountName.Text = row5[0]["AccountName"].ToString();
                }

                //حساب القيمة 
                DataRow[] row6 = dtDeclaration.Select("DeclareAccountName = 'AddtionalAccount'");
                if (row6.Length > 0)
                {
                    lblAdditionalAccountID.Text = row6[0]["AccountID"].ToString();
                    lblAdditionalAccountName.Text = row6[0]["AccountName"].ToString();

                }
                DataRow[] row7 = dtDeclaration.Select("DeclareAccountName = 'CreditGoldAccountID'");
                if (row7.Length > 0)
                {
                    lblCreditGoldAccountID.Text = row7[0]["AccountID"].ToString();
                    lblAdditionalAccountName.Text = row7[0]["AccountName"].ToString();

                }

                DataRow[] row9 = dtDeclaration.Select("DeclareAccountName = 'DebitDiamondAccountID'");
                if (row9.Length > 0)
                {

                    txtDebitDiamondAccountID.Text = row9[0]["AccountID"].ToString();
                    lblDebitDiamondAccountName.Text = row9[0]["AccountName"].ToString();

                }

            }
            #endregion
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
        public void ClearFields()
        {
            try
            {
                chkPosted.Checked = false;
                lblTotalStown.Text = "0";
                lblTotalBagat.Text = "0";
                lblTotalDaimond.Text = "0";
                
                
                lblInvoiceTotalGold.Text = "0";
                chkNoSale.Checked = false;
                DiscountCustomer = 0;
                lblInvoiceTotalOjor.Text = "0";
                txtPaidAmount.Text = "";
                lblRemaindAmount.Text = "";
                txtVatID.Text = "";
                txtDocumentID.Text = "";
                lbl18.Text = "0";
                lbl21.Text = "0";
                lbl22.Text = "0";
                lbl24.Text = "0";
                txtDelegateID.Text = "";
                txtCustomerMobile.Text = "";
                lblDelegateName.Text = "";
                txtNotes.Text = "";
                txtSupplierInvoiceID.Text = "";
                /////////////////////////////
                txtNetProcessID.Tag = " ";
                cmbBank.Tag = " ";
                cmbNetType.Tag = " ";
                txtNetAmount.Tag = " ";
                txtCheckID.Tag = " ";
                /////////////////////////////////////////////////
                txtInvoiceDate.EditValue = DateTime.Now;
                txtWarningDate.EditValue = DateTime.Now;
                txtCheckSpendDate.EditValue = DateTime.Now;
                checkBox1.Checked = false;
                checkBox2.Checked = true;
                cmbMethodID.ItemIndex = 0;
                txtNotes.Text = "";
                lblDebitAccountID.Text = "";
                lblDebitAccountName.Text = "";
                lblInvoiceTotalBeforeDiscount.Text = "";
                lblCreditAccountID.Text = "";
                lblCreditAccountName.Text = "";
                lblAdditionalAccountID.Text = "";
                lblAdditionalAccountName.Text = "";
                lblAdditionalAccountID.Text = "";
                lblAdditionalAccountName.Text = "";
                lblInvoiceTotal.Text = "0";
                lblUnitDiscount.Text = "0";
                txtDiscountOnTotal.Text = "0";
                txtDiscountPercent.Text = "0";
                lblDiscountTotal.Text = "0";
                lblAdditionaAmmount.Text = "0";
                lblNetBalance.Text = "0";
                lblTotalSalePrice17.Text = "0";
                lblTotalSalePrice.Text = "0";
                picItemUnits.Image = null;
                GetAccountsDeclaration();
                txtEnteredByUserID.Text = UserInfo.ID.ToString();
                txtEnteredByUserID_Validating(null, null);
                txtEditedByUserID.Text = "0";
                txtEditedByUserID_Validating(null, null);
                txtDelegateID.Text = MySession.GlobalDefaultSaleDelegateID;
                txtDelegateID_Validating(null, null);
                txtCostCenterID.Text = MySession.GlobalDefaultCostCenterID;
                txtCostCenterID_Validating(null, null);
                txtSellerID.Text = MySession.GlobalDefaultSellerID;
                txtSellerID_Validating(null, null);
                txtStoreID.Text = MySession.GlobalDefaultStoreID;
                txtStoreID_Validating(null, null);
                cmbMethodID.EditValue = Comon.cInt("0");
                if (MySession.GlobalDefaultSalePayMethodID != "0")
                    cmbMethodID.EditValue = Comon.cInt(MySession.GlobalDefaultSalePayMethodID);
                else
                    cmbMethodID.EditValue = 1;
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultSaleCurencyID);
                lstDetail = new BindingList<Sales_PurchaseSaveInvoiceReturnDetails>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;
                dt = new DataTable();
                chkForVat.Checked = true;
                picItemImage.Image = null;
         

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                #region If
                if (FormView == true)
                {
                    SplashScreenManager.CloseForm(false);
                    strSQL = "SELECT TOP 1 * FROM " + Sale_CashierPurchaseSaveDaimondReturnDAL.TableName +" Where  GoldUsing= " + GoldUsing + " and Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Sale_CashierPurchaseSaveDaimondReturnDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Sale_CashierPurchaseSaveDaimondReturnDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Sale_CashierPurchaseSaveDaimondReturnDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Sale_CashierPurchaseSaveDaimondReturnDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Sale_CashierPurchaseSaveDaimondReturnDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Sale_CashierPurchaseSaveDaimondReturnDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Sale_CashierPurchaseSaveDaimondReturnDAL();
                    long InvoicIDTemp = Comon.cLong(txtInvoiceID.Text);
                    InvoicIDTemp = cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                    {
                        ReadRecord(InvoicIDTemp);
                        EnabledControl(false);
                    }
                    SendKeys.Send("{Escape}");
                }
                #endregion
                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                    return;
                }


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

 
        #endregion
        #region Do Function
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                txtInvoiceID.Text = Sale_CashierPurchaseSaveDaimondReturnDAL.GetNewID(Comon.cInt(cmbBranchesID.EditValue)).ToString();
                txtRegistrationNo.Text = txtInvoiceID.Text;
                txtDailyID.Text = GetNewDialyID(MySession.GlobalFacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCostCenterID.Text)).ToString();
                
                ClearFields();
                IdPrint = false;
                EnabledControl(true);
                cmbFormPrinting.EditValue = 1;
                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
                //  gridView1.ShowEditor();
           
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoLast()
        {
            try
            {
                MoveRec(0, xMoveLast);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoFirst()
        {
            try
            {
                MoveRec(0, xMoveFirst);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoNext()
        {
            try
            {
                MoveRec(Comon.cInt(txtInvoiceID.Text), xMoveNext);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoPrevious()
        {
            try
            {
                MoveRec(Comon.cInt(txtInvoiceID.Text), xMovePrev);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoSearch()
        {
            try
            {
                txtInvoiceID.Enabled = true;
                txtInvoiceID.Focus();
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        protected override void DoEdit()
        {

            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("SizeID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Description", System.Type.GetType("System.String"));
            dtItem.Columns.Add("StoreID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Discount", System.Type.GetType("System.String"));
            dtItem.Columns.Add("AdditionalValue", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Net", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Cancel", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));

            dtItem.Columns.Add("GroupID", System.Type.GetType("System.String"));
            dtItem.Columns.Add(GroupName, System.Type.GetType("System.String"));
             
             

            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Total", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDateStr", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.DateTime"));
            dtItem.Columns.Add("Bones", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("HavVat", System.Type.GetType("System.Boolean"));

            dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Serials", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BAGET_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("STONE_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("DIAMOND_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Color", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CLARITY", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CaratPrice", System.Type.GetType("System.Decimal"));
 

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem.Rows[i]["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i][ItemName] = gridView1.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i][SizeName] = gridView1.GetRowCellValue(i, SizeName).ToString();

                dtItem.Rows[i]["GroupID"] = gridView1.GetRowCellValue(i, "GroupID").ToString();
                dtItem.Rows[i][GroupName] = gridView1.GetRowCellValue(i, GroupName).ToString();
                 
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
                dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());
                dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString());
                dtItem.Rows[i]["ExpiryDate"] = "01/01/1900";
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView1.GetRowCellValue(i, "HavVat").ToString());
                dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                dtItem.Rows[i]["AdditionalValue"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                dtItem.Rows[i]["Serials"] =  gridView1.GetRowCellValue(i, "Serials").ToString();
                dtItem.Rows[i]["BAGET_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "BAGET_W").ToString());
                dtItem.Rows[i]["STONE_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "STONE_W").ToString());
                dtItem.Rows[i]["DIAMOND_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
                dtItem.Rows[i]["Color"] =  gridView1.GetRowCellValue(i, "Color").ToString();
                dtItem.Rows[i]["CLARITY"] = gridView1.GetRowCellValue(i, "CLARITY").ToString();
                dtItem.Rows[i]["CaratPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CaratPrice").ToString());
              

                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
           
        }
     
        protected override void DoSave()
        {
            try
            {
                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;
                }
                else if (!IsNewRecord)
                {
                    if (!FormUpdate)
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                        return;
                    }
                    else
                    {
                        bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, Messages.msgConfirmUpdate);
                        if (!Yes)
                            return;
                    }
                }


                gridView1.MoveLast();
               
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

               

                gridView1.MoveLast();



                if (!Validations.IsValidForm(this))
                    return;

    

                if (!IsValidGrid())
                    return;

                #region Save Diamond  Type
                Sales_PurchaseDiamondDetails objRecord = new Sales_PurchaseDiamondDetails();
                objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);

                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.FacilityID = UserInfo.FacilityID;

                objRecord.StoreID = Comon.cInt(txtStoreID.Text);
                objRecord.SupplierID = Comon.cDbl(txtSupplierID.Text);
                objRecord.Cancel = 0;

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    if (gridView1.GetRowCellValue(i, "DIAMOND_W").ToString() != "")
                    {
                        Sales_PurchaseDiamondDetails returned;
                        List<Sales_PurchaseDiamondDetails> listreturned = new List<Sales_PurchaseDiamondDetails>();
                        DataTable dtt = Sales_PurchaseDiamondDetailsDAL.frmGetDataDetalByBarCode(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, gridView1.GetRowCellValue(i, "BarCode").ToString(), 7);

                        for (int j = 0; j < dtt.Rows.Count; j++)
                        {

                            returned = new Sales_PurchaseDiamondDetails();
                            returned.FacilityID = UserInfo.FacilityID;
                            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                            returned.BarCode = dtt.Rows[j]["BarCode"].ToString();
                            returned.ItemID = Comon.cInt(dtt.Rows[j]["ItemID"]);
                            returned.BarCodeItem = dtt.Rows[j]["BarCodeItem"].ToString();
                            returned.ArbItemName = dtt.Rows[j][ItemName].ToString();
                            returned.WeightIn = 0;
                            returned.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                            returned.WeightOut = Comon.ConvertToDecimalQty(dtt.Rows[j]["WeightIn"]);
                            returned.TypeOpration = 8;
                            returned.CaptionOpration = "فاتورة مردود سند أمانات";
                            returned.PriceCarat = Comon.cDec(dtt.Rows[j]["PriceCarat"]);
                            returned.TotalPrice = Comon.cDec(dtt.Rows[j]["TotalPrice"]);
                            returned.SupplierID = Comon.cDbl(txtSupplierID.Text);
                            returned.StoreID = Comon.cInt(txtStoreID.Text);

                            if (returned.WeightOut <= 0 || returned.StoreID <= 0 || (returned.PriceCarat <= 0 && returned.TotalPrice <= 0) || returned.ItemID <= 0)
                                continue;
                            listreturned.Add(returned);
                        }
                        if (listreturned.Count > 0)
                        {
                            objRecord.DiamondDatails = listreturned;
                            string Result = Sales_PurchaseDiamondDetailsDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();

                        }
                    }
                }
                #endregion
           
                Save();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        public void ReadRecord(long InvoiceID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                   
                    dt = Sale_CashierPurchaseSaveDaimondReturnDAL.frmGetDataDetalByID(InvoiceID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        
                        //Validate
                       txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();

                       txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();

                        StopSomeCode = true;
                        cmbMethodID.EditValue = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());
                        StopSomeCode = false;

                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurencyID"].ToString());
                        cmbNetType.EditValue = Comon.cDbl(dt.Rows[0]["NetType"].ToString());

                        txtSupplierID.Text = dt.Rows[0]["SupplierID"].ToString();
                        txtSupplierID_Validating(null, null);
                      

                        //cmbSellerID.EditValue = dt.Rows[0]["SellerID"].ToString();


                        txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        txtDelegateID_Validating(null, null);

                        txtEnteredByUserID.Text = dt.Rows[0]["UserID"].ToString();
                        txtEnteredByUserID_Validating(null, null);

                        txtEditedByUserID.Text = dt.Rows[0]["EditUserID"].ToString();
                        txtEditedByUserID_Validating(null, null);

                        //Account
                        txtDebitGoldAccountID.Text = dt.Rows[0]["DebitGoldAccountID"].ToString();
                        txtCreditGoldAccountID.Text = dt.Rows[0]["CreditGoldAccountID"].ToString();


                        lblDebitAccountID.Text = dt.Rows[0]["DebitAccount"].ToString();
                        lblDebitAccountID_Validating(null, null);

                        lblCreditAccountID.Text = dt.Rows[0]["CreditAccount"].ToString();
                        lblCreditAccountID_Validating(null, null);

                        lblAdditionalAccountID.Text = dt.Rows[0]["AdditionalAccount"].ToString();
                        lblAdditionalAccountID_Validating(null, null);

                        //Masterdata
                        txtInvoiceID.Text =InvoiceID+"";
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();
                        txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();

   
                        //Date
                        txtInvoiceDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString());
                        txtWarningDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["WarningDate"].ToString());
                        txtCheckSpendDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["CheckSpendDate"].ToString());

                        //Ammount

                        txtCheckID.Text = dt.Rows[0]["CheckID"].ToString();
                        txtNetAmount.Text = dt.Rows[0]["NetAmount"].ToString();
                        txtNetProcessID.Text = dt.Rows[0]["NetProcessID"].ToString();
                       // txtVatID.Text = dt.Rows[0]["VatID"].ToString();

                        txtDiscountOnTotal.Text = dt.Rows[0]["DiscountOnTotal"].ToString();

                        //حقول محسوبة 
                        lblUnitDiscount.Text = "0";
                        lblDiscountTotal.Text = "0";
                    
                        lblInvoiceTotal.Text = dt.Rows[0]["InvoiceTotal"].ToString();
                        lblAdditionaAmmount.Text = dt.Rows[0]["AdditionaAmountTotal"].ToString();
                        lblNetBalance.Text = dt.Rows[0]["NetBalance"].ToString();
                        lblCreditGoldAccountID.Text = dt.Rows[0]["CreditGoldAccountID"].ToString();
                        if (Comon.cDbl(lblAdditionaAmmount.Text) > 0)
                            chkForVat.Checked = true;
                        else
                            chkForVat.Checked = false;
                        //GridVeiw
                        gridControl.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        CalculateRow();
                        this.pictureBox1.Image = Common.GenratCod("ID = " + txtInvoiceID.Text + " Date= " + txtInvoiceDate.Text);
                        

                       // Validations.DoReadRipon(this, ribbonControl1);
                       // ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtInvoiceID.Text;
                    }
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void Save()
        {

            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            Sales_PurchaseInvoiceSaveReturnMaster objRecord = new Sales_PurchaseInvoiceSaveReturnMaster();
            objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.MethodeID = Comon.cInt(cmbMethodID.EditValue);
            objRecord.CurencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.NetType = Comon.cInt(cmbNetType.EditValue);
            
            objRecord.SupplierInvoiceID = Comon.cInt(txtSupplierInvoiceID.Text);
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cInt(txtStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SupplierID = Comon.cDbl(txtSupplierID.Text);
            objRecord.SupplierName =lblSupplierName.Text;
        
          
         
            //objRecord.SellerID = Comon.cInt(cmbSellerID.EditValue);


          
           txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Purchase save return Invoice" : " فاتوره مردود مشتريات عروض  "));
             

            objRecord.Notes = txtNotes.Text;
            //Account
            objRecord.DebitAccount = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            //objRecord.DiscountCreditAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
            objRecord.CreditGoldAccountID = Comon.cDbl(txtCreditGoldAccountID.Text);
            objRecord.DebitGoldAccountID = Comon.cDbl(txtDebitGoldAccountID.Text);
            //objRecord.InvoiceGoldTotal= Comon.cDec(lblInvoiceTotalGold.Text);
            objRecord.CheckAccount = Comon.cDbl(lblChequeAccountID.Text);
            objRecord.NetAccount = Comon.cDbl(lblNetAccountID.Text);
            objRecord.AdditionalAccount = Comon.cDbl(lblAdditionalAccountID.Text);
            objRecord.NetProcessID = txtNetProcessID.Text;
            objRecord.CheckID = txtCheckID.Text;
            objRecord.VATID = txtVatID.Text;
             
            //Date
            objRecord.CheckSpendDate = Comon.ConvertDateToSerial(txtCheckSpendDate.Text).ToString();
            objRecord.WarningDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            objRecord.ReceiveDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            //Ammount
            objRecord.NetAmount = Comon.cDbl(txtNetAmount.Text);
            objRecord.DiscountOnTotal = Comon.cDbl(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = Comon.cDbl(lblInvoiceTotalBeforeDiscount.Text);
            objRecord.AdditionaAmountTotal = Comon.cDbl(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Comon.cDbl(lblNetBalance.Text);
            objRecord.Mobile = txtCustomerMobile.Text;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cBooleanToInt(chkPosted.Checked);
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";
            objRecord.GoldUsing = GoldUsing;
            if (IsNewRecord == false)
            {
                objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            Sales_PurchaseSaveInvoiceReturnDetails returned;
            List<Sales_PurchaseSaveInvoiceReturnDetails> listreturned = new List<Sales_PurchaseSaveInvoiceReturnDetails>();
            
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {

                returned = new Sales_PurchaseSaveInvoiceReturnDetails();
                returned.ID = i;
                 
                returned.FacilityID = UserInfo.FacilityID;
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.Color = gridView1.GetRowCellValue(i, "Color").ToString();
                 
                returned.CLARITY = gridView1.GetRowCellValue(i, "CLARITY").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.cDbl(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.STONE_W = Comon.cDbl(gridView1.GetRowCellValue(i, "STONE_W").ToString());
                returned.DIAMOND_W = Comon.cDbl(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
                returned.BAGET_W = Comon.cDbl(gridView1.GetRowCellValue(i, "BAGET_W").ToString());
                returned.Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                returned.Equivalen =Comon.cDbl( Comon.ConvertToDecimalPrice( Comon.ConvertTo21Caliber(Comon.cDec( returned.QTY), Comon.cInt(returned.Caliber), 21)));
                returned.Bones = Comon.cDbl(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.Description = gridView1.GetRowCellValue(i, ItemName).ToString();
                returned.StoreID = Comon.cInt(txtStoreID.Text);
                returned.Discount = Comon.cDec(gridView1.GetRowCellValue(i, "Discount").ToString());
        
                returned.CostPrice = Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice").ToString()); ;
                returned.SalePrice = Comon.cDec(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                returned.AdditionalValue = Comon.cDbl(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                returned.CaratPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CaratPrice").ToString()); ;
                
                returned.Serials = gridView1.GetRowCellValue(i, "Serials").ToString();
                if (returned.BarCode.Trim() == "24")
                    returned.AdditionalValue = 0;
                returned.Net = Comon.cDbl(gridView1.GetRowCellValue(i, "Net").ToString());
                returned.Total = Comon.cDbl(gridView1.GetRowCellValue(i, "Total").ToString());
                if (returned.AdditionalValue == 0)
                    returned.HavVat = 0;
                else
                    returned.HavVat = 1;
                returned.Cancel = 0;

                 
                listreturned.Add(returned);
            }
            if (listreturned.Count > 0)
            {

                objRecord.PurchaseSaveReturnDatails = listreturned;


                string Result = Sale_CashierPurchaseSaveDaimondReturnDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();

                //حفظ القيد الالي

                if (Comon.cInt(Result) > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result));
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Sale_CashierPurchaseSaveDaimondReturnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sale_CashierPurchaseSaveDaimondReturnDAL.PremaryKey + " = " + txtInvoiceID.Text);

                }
                SplashScreenManager.CloseForm(false);


                if (IsNewRecord == true)
                {
                    if ( Comon.cLong( Result )>0)
                    {

                        IsNewRecord = false;

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        txtInvoiceID.Text = Result.ToString();
                        if (falgPrint == true)
                            DoPrint();
                        DoNew();
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }

                }
                else
                {

                    if (Comon.cLong(Result) > 0)
                    {
                        txtInvoiceID_Validating(null, null);
                         
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                    }
                }

            }
            else
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);
            }
            
        }
        protected override void DoDelete()
        {
            try
            {
                if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                int TempID = Comon.cInt(txtInvoiceID.Text);
                //Check if The BarCode is have Opration 
                bool FlageChecKDelete = false;

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    FlageChecKDelete = Lip.CheckTheItemIsHaveTransactionByBarCode(gridView1.GetRowCellValue(i, "BarCode").ToString(), "Sales_PurchaseSaveInvoiceReturnDetails");
                    if (FlageChecKDelete)
                    {
                        SplashScreenManager.CloseForm();
                        Messages.MsgError("Error Delete ", "لا يمكن حذف الصنف بسبب وجود عمليات محاسبية علية");
                        return;
                    }
                }

               
                Sales_PurchaseInvoiceSaveReturnMaster model = new Sales_PurchaseInvoiceSaveReturnMaster();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                string Result = Sale_CashierPurchaseSaveDaimondReturnDAL.DeleteSales_PurchaseInvoiceSaveReturnMaster(model).ToString();
                if (Comon.cInt(Result) > 0)
                {
                    //حذف القيد الالي

                    int VoucherID = DeleteVariousVoucherMachin(Comon.cInt(Result));

                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية");

                }
                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) > 0)
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    ClearFields();
                    txtInvoiceID.Text = model.InvoiceID.ToString();
                    MoveRec(model.InvoiceID, xMovePrev);
                }
                else
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgErrorSave + " " + Result);
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        int DeleteVariousVoucherMachin(int DocumentID)
        {
            int VoucherID = 0;
            int Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(UserInfo.BRANCHID)));

            objRecord.VoucherID = VoucherID;
            objRecord.EditUserID = UserInfo.ID;
            objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            Result = VariousVoucherMachinDAL.DeleteAcc_VariousVoucherMachinMaster(objRecord);
            return Result;

        }

        void PrintPyCostPrice()
        {
           
            try
            {
                if (IsNewRecord)
                {
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforePrinting);
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                ReportName = "rptPurchaseInvoice";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["SupplierName"].Value = lblSupplierName.Text.Trim().ToString();
                if (Comon.cInt(cmbMethodID.EditValue) == 1)
                    rptForm.Parameters["MethodName"].Value = "نقدا";
                if (Comon.cInt(cmbMethodID.EditValue) == 2)
                    rptForm.Parameters["MethodName"].Value = "اجل";
                if (Comon.cInt(cmbMethodID.EditValue) == 5)
                    rptForm.Parameters["MethodName"].Value = "نقدأ/شبكة";
                if (Comon.cInt(cmbMethodID.EditValue) == 3)
                    rptForm.Parameters["MethodName"].Value = "شبكة";
                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                /********Total*********/
               
                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotal.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = lblUnitDiscount.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();  
                rptForm.Parameters["StoreName"].Value = txtCustomerMobile.Text.Trim().ToString();
               // rptForm.Parameters["TransportAmount"].Value = (Comon.ConvertToDecimalPrice(lblTotalSalePrice17.Text)).ToString();
                rptForm.Parameters["TotalCost"].Value = Comon.ConvertToDecimalQty(lblInvoiceTotalOjor.Text);
            
                //rptForm.Parameters["TotalSale"].Value = Comon.ConvertToDecimalQty(lblTotalCost.Text);
                rptForm.Parameters["TotalWightGold"].Value = (Comon.ConvertToDecimalQty(lbl18.Text) + Comon.ConvertToDecimalQty(lbl21.Text) + Comon.ConvertToDecimalQty(lbl22.Text) + Comon.ConvertToDecimalQty(lbl24.Text)).ToString();

               
               //rptForm.Parameters["TotalDaimond"].Value = Comon.ConvertToDecimalQty(lblTotalDaimond.Text) ;
               
                //rptForm.Parameters["TotalStown"].Value = Comon.ConvertToDecimalQty(lblTotalStown.Text);
               
               // rptForm.Parameters["TotalBagat"].Value = Comon.ConvertToDecimalQty(lblTotalBagat.Text);
                rptForm.Parameters["TotalSale"].Value =( Comon.ConvertToDecimalQty(lbl18.Text) * Comon.ConvertToDecimalQty(lblInvoiceTotal.Text)).ToString();
               // rptForm.Parameters["ReportName"].Value = "فاتورة مشتريات بسعر الشراء";
                rptForm.Parameters["G18"].Value = lbl18.Text;
                rptForm.Parameters["G21"].Value = lbl21.Text;
                rptForm.Parameters["G22"].Value = lbl22.Text;
                rptForm.Parameters["G24"].Value = lbl24.Text;
               
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptPurchaseInvoiceDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString();

                    row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["CostPrice"] = gridView1.GetRowCellValue(i, "CostPrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                    //row["STONE_W"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();
                   // row["DIAMOND_W"] = gridView1.GetRowCellValue(i, "DIAMOND_W").ToString();
                    row["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
                    row["Bones"] = gridView1.GetRowCellValue(i, "Bones").ToString();
                    row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();

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



        protected override void DoPrint()
        {
             
                PrintPyCostPrice();
        } 

        #endregion
        #endregion
        #region Event

        #region Validating


        private void txtRegistrationNo_Validated(object sender, EventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cLong(txtRegistrationNo.Text), true);
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }
        private void txtInvoiceID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cLong(txtInvoiceID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }

        }
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void txtEnteredByUserID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtEnteredByUserID.Text) + " And Cancel =0 And BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtEnteredByUserID, lblEnteredByUserName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void txtEditedByUserID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtEditedByUserID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEditedByUserID, lblEditedByUserName, strSQL);
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
        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {


          
        }

        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
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
                strSQL = "SELECT " + PrimaryName + " as SellerName FROM Sales_Sellers WHERE SellerID =" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtSellerID, lblSellerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtDiscountOnTotal_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (txtDiscountOnTotal.Text != string.Empty & lblInvoiceTotal.Text != string.Empty)
                {
                    decimal DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                    decimal whole = Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text);
                    decimal TotalUnitDiscount = Comon.ConvertToDecimalPrice(lblUnitDiscount.Text);
                    decimal TotalDiscount = DiscountOnTotal + TotalUnitDiscount;
                    if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) != 0)
                    {
                        txtDiscountPercent.Text = ((DiscountOnTotal / whole) * 100).ToString("N" + MySession.GlobalPriceDigits);
                        decimal TotalDiscountPercent = Comon.ConvertToDecimalPrice((((TotalDiscount) / whole) * 100).ToString("N" + MySession.GlobalPriceDigits));
                        if (TotalDiscountPercent > MySession.GlobalDiscountPercentOnTotal)
                        {
                            Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount);
                            txtDiscountPercent.Text = "0";
                            txtDiscountOnTotal.Text = "0";
                            txtDiscountOnTotal.Focus();
                            return;
                        }
                    }
                    else
                    {
                        txtDiscountPercent.Text = "0";
                    }
                    // if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) > 0 && !MySession.GlobalAllowedPercentDiscount) { Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount); return; }

                    DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                    lblDiscountTotal.Text = (DiscountOnTotal + TotalUnitDiscount).ToString("N" + MySession.GlobalPriceDigits);
                }
                CalculateRow();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }
        private void txtDiscountPercent_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (txtDiscountPercent.Text != string.Empty & lblInvoiceTotal.Text != string.Empty)
                {
                    decimal percent = Comon.ConvertToDecimalPrice(txtDiscountPercent.Text);
                    decimal whole = Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text);
                    if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) != Comon.ConvertToDecimalPrice(Math.Round(((percent * whole) / 100))) && Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text)==0)
                    {
                        txtDiscountOnTotal.Text = ((percent * whole) / 100).ToString("N" + MySession.GlobalPriceDigits);

                        decimal DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                        decimal UnitDiscount = Comon.ConvertToDecimalPrice(lblUnitDiscount.Text);
                        lblDiscountTotal.Text = (DiscountOnTotal + UnitDiscount).ToString("N" + MySession.GlobalPriceDigits);
                        txtDiscountOnTotal_Validating(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }

        }

        private void txtPaidAmount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (IsNewRecord == false)
                {
                    txtPaidAmount.Text = "0";
                    lblRemaindAmount.Text = "0";
                }
                else
                {
                    if (MethodID == 2)
                        txtPaidAmount.Text = "0";
                    else if (MethodID == 1)
                    {
                        lblRemaindAmount.Text = (Comon.cDbl(txtPaidAmount.Text) - Comon.cDbl(lblNetBalance.Text)).ToString();

                    }
                    else if (MethodID == 3)
                    {
                        lblRemaindAmount.Text = ((Comon.cDbl(txtPaidAmount.Text) + Comon.cDbl(txtNetAmount.Text)) - Comon.cDbl(lblNetBalance.Text)).ToString();

                    }
                }

                gridView1.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }

        }
        private void lblChequeAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + lblChequeAccountID.Text + ") ";
                CSearch.ControlValidating(lblChequeAccountID, lblChequeAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void lblDebitAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + lblDebitAccountID.Text + ") ";
                CSearch.ControlValidating(lblDebitAccountID, lblDebitAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void lblCreditAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + lblCreditAccountID.Text + ") ";
                CSearch.ControlValidating(lblCreditAccountID, lblCreditAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void lblAdditionalAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE     (Cancel = 0) AND (AccountID = " + lblAdditionalAccountID.Text + ") ";
                CSearch.ControlValidating(lblAdditionalAccountID, lblAdditionalAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void lblDiscountCreditAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + lblDiscountDebitAccountID.Text + ") ";
                CSearch.ControlValidating(lblDiscountDebitAccountID, lblDiscountDebitAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void lblNetAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + lblNetAccountID.Text + ") ";
                CSearch.ControlValidating(lblNetAccountID, lblNetAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        #region Search
        /***************************Event Search ***************************/
        private void btnDebitSearch_Click(object sender, EventArgs e)
        {
            try
            {
               
               
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void btnCreditSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(lblCreditAccountID, lblCreditAccountName);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void btnAdditionalSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(lblAdditionalAccountID, lblAdditionalAccountName);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void btnDiscountCreditSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(lblDiscountDebitAccountID, lblDiscountDebitAccountName);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void btnNetSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(lblNetAccountID, lblNetAccountName);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void btnChequeSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(lblChequeAccountID, lblChequeAccountName);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #endregion
        /************************Event From **************************/
        private void frmSaleInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();
        }
        /*******************Event CheckBoc***************************/
        public void chForVat_EditValueChanged(object sender, EventArgs e)
        {

        }

        #region Event TextEdit
        private void PublicTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void PublicTextEdit_Enter(object sender, EventArgs e)
        {
            (sender as DateEdit).ShowPopup();
        }
        private void PublicTextEdit_Click(object sender, EventArgs e)
        {
            (sender as DateEdit).ShowPopup();
        }

        #endregion
        #region Event Combox

        private void PublicCombox_Enter(object sender, EventArgs e)
        {
            (sender as LookUpEdit).ShowPopup();
        }
        private void PublicCombox_Click(object sender, EventArgs e)
        {
            (sender as LookUpEdit).ShowPopup();
        }
        private void cmbBank_EditValueChanged(object sender, EventArgs e)
        {
            double value = Comon.cDbl(cmbBank.EditValue.ToString());
            if (value == 0)
                return;
            lblDebitAccountID.Text = cmbBank.EditValue.ToString();
            lblDebitAccountID_Validating(null, null);
        }
        private void cmbNetType_EditValueChanged(object sender, EventArgs e)
        {
            double value = Comon.cDbl(cmbNetType.EditValue.ToString());
            if (value == 0)
                return;
            //if (Comon.cInt(cmbMethodID.EditValue) != 5)
            //{
            //    lblDebitAccountID.Text = cmbNetType.EditValue.ToString();
            //    lblDebitAccountID_Validating(null, null);
            //}
        }
        private void cmbMethodID_EditValueChanged(object sender, EventArgs e)
        {
              
 

 
        }
        private void cmbMethodID_Enter(object sender, EventArgs e)
        {
            
            (sender as LookUpEdit).ShowPopup();
        }
        private void cmbMethodID_Click(object sender, EventArgs e)
        {
            (sender as LookUpEdit).ShowPopup();
        }
        #endregion
        #endregion
        #region InitializeComponent
        private void RolesButtonSearchAccountID()
        {

            btnDebitSearch.Enabled = MySession.GlobalAllowChangefrmSaleDebitAccountID;
            btnCreditSearch.Enabled = MySession.GlobalAllowChangefrmSaleCreditAccountID;
            btnAdditionalSearch.Enabled = MySession.GlobalAllowChangefrmSaleAdditionalAccountID;
            btnNetSearch.Enabled = MySession.GlobalAllowChangefrmSaleNetAccountID;
            btnChequeSearch.Enabled = MySession.GlobalAllowChangefrmSaleChequeAccountID;
            btnDiscountDebitSearch.Enabled = MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID;


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
            Obj.EditValue = DateTime.Now;
        }

        
        #endregion

       
        

        
 

        private void btnThree_Click(object sender, EventArgs e)
        {
            strQty = strQty + "3";

        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            DoSave();
        
        }

      

  

        private void btnNine_Click(object sender, EventArgs e)
        {
            strQty = strQty + "9";
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            strQty = strQty + "8";
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
           
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"])) + Comon.ConvertToDecimalPrice(strQty.Trim ()));
            CalculateRow(gridView1.FocusedRowHandle, true);
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            strQty = "";

        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"])) - Comon.ConvertToDecimalPrice(strQty.Trim()));
            CalculateRow(gridView1.FocusedRowHandle, true);
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            strQty = "";
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            strQty = strQty + "7";
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            strQty = strQty + "4";
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            strQty = strQty + "5";
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            strQty = strQty + "6";
        }

        private void btnTow_Click(object sender, EventArgs e)
        {
            strQty = strQty + "2";
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            strQty = strQty + "1";
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            strQty = strQty + "0";
        }

        private void txtPaidAmount_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void frmCashierPurchaseSaveDaimondReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                falgPrint = true;
                DoSave();
            }
            if (e.KeyCode == Keys.F6)
            { 
                DoSave();
            }
        
            if (e.KeyCode == Keys.F3)
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {
           
        }

        private void txtInvoiceDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
                txtInvoiceDate.EditValue = DateTime.Now;
            if (Comon.ConvertDateToSerial(txtInvoiceDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtInvoiceDate.Text = Lip.GetServerDate();
        }

      

        private void showCustomers(bool p,int f )
        {
             
            txtVatID.Text = "";
            labelControl6.Visible = p;
            labelControl4.BringToFront();
            labelControl4.Visible = p;
            txtVatID.Visible = p;
            
        }

    

      
     

        

    
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtSupplierID.Text != string.Empty && txtSupplierID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtSupplierID.Text + " And BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();


                        if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                            txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                        else
                            txtVatID.Text = "";

                        
                            lblDebitAccountID.Text = txtSupplierID.Text;
                            lblDebitAccountName.Text = lblSupplierName.Text;
                           
                    
                    }
                    else
                    {
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtSupplierID.Text + " And BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {
                            
                            lblDebitAccountID.Text = txtSupplierID.Text;
                            lblDebitAccountName.Text = dt.Rows[0]["CustomerName"].ToString();
                            lblSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();
                            txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();

                            if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                                txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                            else
                                txtVatID.Text = "";
                        }
                        else
                        {
                            lblSupplierName.Text = "";
                            txtSupplierID.Text = "";
                            txtVatID.Text = "";
                        }
                    }
                }
                else
                {
                    lblSupplierName.Text = "";
                    txtSupplierID.Text = "";
                    txtVatID.Text = "";
                }
                
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

      
     
        private void SaveImage(byte[] data,int ItemID)
        {
            try
            {

                SqlConnection Con = new GlobalConnection().Conn;
                if (Con.State == ConnectionState.Closed)
                    Con.Open();

                SqlCommand sc;
                sc = new SqlCommand("Update  Stc_Items Set ItemImage=@p Where ItemID=" + ItemID, Con);
                sc.Parameters.AddWithValue("@p", data);
                sc.ExecuteNonQuery();

            }
            catch
            {

            }
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        private void lnkAddImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void chkForVatOjor_CheckedChanged(object sender, EventArgs e)
        {

            chForVat_EditValueChanged(null, null);
        }
        long SaveVariousVoucherMachin(int DocumentID)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = 1;

            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;
            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";
            if (IsNewRecord == false)
            {
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }
            Acc_VariousVoucherMachinDetails returned;
            List<Acc_VariousVoucherMachinDetails> listreturned = new List<Acc_VariousVoucherMachinDetails>();
            //Debit
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;        
            returned.CreditGold = 0;

            returned.DebitGold = Comon.cDbl(lblInvoiceTotalGold.Text); 
            returned.Debit = Comon.cDbl(lblInvoiceTotal.Text);
            returned.DebitDiamond = Comon.cDbl(lblTotalDaimond.Text);
             
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            

            //credit

            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 3;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(lblCreditAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(lblInvoiceTotal.Text);
            returned.Debit = 0;
            returned.DebitGold = 0;
            returned.CreditGold = 0;
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //credit gold
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 4;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtCreditGoldAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;
            returned.Debit = 0;
            returned.DebitGold = 0;
            returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            
            //Crdit Diamond
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 2;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtDebitDiamondAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;
            returned.CreditGold = 0;
            returned.DebitGold = 0;
            returned.Debit = 0;
            returned.CreditDiamond = Comon.cDbl(lblTotalDaimond.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //===

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        private void btnMachinResraction_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtInvoiceID.Text + " And DocumentType=" + DocumentType).ToString());
            if (ID > 0)
            {
                frmVariousVoucherMachin frm22 = new frmVariousVoucherMachin();
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm22);
                frm22.FormView = true;
                frm22.FormAdd = false;
                frm22.Show();
                frm22.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                frm22.ReadRecord(Comon.cLong(ID.ToString()));
            }
            else
                Messages.MsgError("تنبيه", "   لا يوجد قيد - الرجاء اعادة حفظ المستند ");
        }
        public void Transaction()
        {


            strSQL = "Select * from " + Sale_CashierPurchaseSaveDaimondReturnDAL.TableName + " where cancel=0 and GoldUsing=" + GoldUsing;
            DataTable dtSend = new DataTable();
            dtSend = Lip.SelectRecord(strSQL);
            if (dtSend.Rows.Count > 0)
            {
                for (int i = 0; i <= dtSend.Rows.Count - 1; i++)
                {
                    txtInvoiceID.Text = dtSend.Rows[i]["InvoiceID"].ToString();
                    cmbBranchesID.EditValue = Comon.cInt(dtSend.Rows[i]["BranchID"].ToString());
                    txtCostCenterID.Text = dtSend.Rows[i]["CostCenterID"].ToString();
                    txtStoreID.Text = dtSend.Rows[i]["StoreID"].ToString();
                    txtInvoiceID_Validating(null, null);
                    IsNewRecord = true;
                    if (Comon.cInt(txtInvoiceID.Text) > 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(Comon.cInt(txtInvoiceID.Text));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + Sale_CashierPurchaseSaveDaimondReturnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sale_CashierPurchaseSaveDaimondReturnDAL.PremaryKey + " = " + txtInvoiceID.Text + " AND BranchID=" + Comon.cInt(cmbBranchesID.EditValue));

                    }
                }

                this.Close();
            }
        }

        private void frmCashierPurchaseSaveDaimondReturn_Load(object sender, EventArgs e)
        {
            DoNew();

            if (IsNewRecord)
                Validations.DoNewRipon(this, ribbonControl1);
        }

    }
}
﻿using DAL;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.AccountsObjects.Codes;
using Edex.AccountsObjects.Transactions;
using Edex.DAL;
using Edex.DAL.Accounting;
using Edex.DAL.Configuration;
using Edex.DAL.SalseSystem;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.DAL.Stc_itemDAL;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.GeneralObjects.GeneralUserControls;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;
using Edex.RestaurantSystem.Transactions;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.SalesAndPurchaseObjects.SalesClasses;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.StockObjects.Codes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Edex.SalesAndSaleObjects.Transactions
{
    public partial class frmCashierSales : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        CompanyHeader cmpheader = new CompanyHeader();
        AlertCashier alert = new AlertCashier();
        public int DiscountCustomer = 0;
        #region Declare
        public DataTable dtPriceItemOffers = new DataTable();
        bool IdPrint = false;
        string MethodName = "";
        string invoiceNo = " ";
        int MethodID = 0;
        DataTable dtDeclaration;
        int flagError = 0;
        DataTable dtSize;
        string barcodeLast = "";
        int rowIndex;
        ctAddCustomers ctCustomers ;
        int FocusedRowHandle;
        public string strQty="";
        string QualityCasher;
        string FocusedControl = "";
        private string strSQL;
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
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
        private string CaptionDescription;
        private string CaptionHavVat;
        private string CaptionRemainQty;
        public string Barcode = "";
        public string ArbName = "";
        public string UnitID ="";
        public string Typevat = "1";
        public string GroupName = "";
        public string CaptionGroupName = "";
        DataTable dVat = new DataTable();
        public MemoryStream TheImage;
        private bool IsNewRecord;
        private Sales_SaleServiceInvoicesDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;
        public CultureInfo culture = new CultureInfo("en-US");
        OpenFileDialog OpenFileDialog1 = null;
        DataTable dt = new DataTable();
        GridViewMenu menu;
        //all record master and detail
        BindingList<Sales_SalesServiceInvoiceDetails> AllRecords = new BindingList<Sales_SalesServiceInvoiceDetails>();

        //list detail
        BindingList<Sales_SalesServiceInvoiceDetails> lstDetail = new BindingList<Sales_SalesServiceInvoiceDetails>();

        //Detail
        Sales_SalesServiceInvoiceDetails BoDetail = new Sales_SalesServiceInvoiceDetails();
        string VAt = "Select CompanyVATID from  VATIDCOMPANY ";

        #endregion
        public int DocumentType = 39;
        public void RefreshOffers()
        {

            string dateFrom = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            var spriceOffers = "SELECT        PriceItemsOffers.*  FROM     PriceItemsOffers"
            + "  where ((IsAmount>0)or (IsPercent>0)or(IsOffers>0))"
            + "   And"
            + " (FromDate<=" + dateFrom + ")And (ToDate>=" + dateFrom + ")";

            dtPriceItemOffers = Lip.SelectRecord(spriceOffers);
        }
        public frmCashierSales()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                InitializeComponent();

                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionGroupName = "اسم المجموعة";
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "الوحدة";
                CaptionExpiryDate = "تاريخ الصلاحية";
                CaptionQTY = "الكمية";
                CaptionTotal = "الإجمالي";
                CaptionDiscount = "الخصم";
                CaptionAdditionalValue = "الضريبة";
                CaptionNet = "الصافي";
                CaptionSalePrice = "السعر";
                CaptionDescription = "البيان";
                CaptionHavVat = "عليه ضريبة";
                CaptionRemainQty = "الكمية المتبقية";
                strSQL = "ArbName";
                GroupName = "ArbGroupName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    CaptionGroupName = "Group Name";
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
                    labelControl32.Text = labelControl32.Tag.ToString();
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
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbFormPrinting, "FormPrinting", "FormID", PrimaryName, "", " 1=1 ", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbBank, "[Acc_Banks]", "ID", PrimaryName, "", " 1=1 ", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
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
                cmbLanguagePrint.EditValue = MySession.PrintLAnguage;
                TextEdit[] txtEdit = new TextEdit[16];
                txtEdit[0] = lblStoreName;
                txtEdit[1] = lblStoreName;
                txtEdit[2] = lblCostCenterName;
                txtEdit[3] = lblCustomerName;
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
                foreach (TextEdit item in txtEdit)
                {
                    item.ReadOnly = true;
                    item.Enabled = false;
                    item.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    item.Properties.AppearanceDisabled.BackColor = Color.WhiteSmoke;
                }
                /*********************** Date Format dd/MM/yyyy ****************************/
                InitializeFormatDate(txtInvoiceDate);
                InitializeFormatDate(txtWarningDate);
                InitializeFormatDate(txtCheckSpendDate);
                /************************  Form Printing ***************************************/
                try
                {
                    cmbFormPrinting.EditValue = Comon.cInt(MySession.GlobalDefaultSaleFormPrintingID);
                    /*********************** Roles From ****************************/
                    txtInvoiceDate.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceDate;
                    txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmSaleStoreID;
                    txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCostCenterID;
                    cmbMethodID.ReadOnly = !MySession.GlobalAllowChangefrmSalePayMethodID;
                    cmbNetType.ReadOnly = !MySession.GlobalAllowChangefrmSaleNetTypeID;
                    cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmSaleCurencyID;
                    txtCustomerID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCustomerID;
                    txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDelegateID;
                    txtSellerID.ReadOnly = !MySession.GlobalAllowChangefrmSaleSellerID;
                    /************TextEdit Account ID ***************/
                    lblDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDebitAccountID;
                    lblCreditAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCreditAccountID;
                    lblAdditionalAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleAdditionalAccountID;
                    lblChequeAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleChequeAccountID;
                    lblDiscountDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID;
                    lblNetAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleNetAccountID;
                    /************ Button Search Account ID ***************/
                    RolesButtonSearchAccountID();
                }
                catch (Exception ex)
                { }
                /********************* Event For Account Component ****************************/
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

                alert.btnCancel.Click += btnCancel;
                alert.btnOK.Click += btnOK;

                this.lblDebitAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblCreditAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblAdditionalAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblDiscountDebitAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblNetAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblChequeAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtPaidAmount.EditValueChanged += new System.EventHandler(this.txtPaidAmount_EditValueChanged);


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
                this.txtCustomerID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCheckID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetProcessID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetAmount.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                this.cmbMethodID.EditValueChanged += new System.EventHandler(this.cmbMethodID_EditValueChanged);
                this.cmbNetType.EditValueChanged += new System.EventHandler(this.cmbNetType_EditValueChanged);

                this.cmbBank.EditValueChanged += new System.EventHandler(this.cmbBank_EditValueChanged);


                 

                this.txtDiscountOnTotal.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountOnTotal_Validating);
                this.txtDiscountPercent.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountPercent_Validating);
                this.txtInvoiceID.Validating += new System.ComponentModel.CancelEventHandler(this.txtInvoiceID_Validating);
                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
                this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
                this.txtSellerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSellerID_Validating);
                this.txtEnteredByUserID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEnteredByUserID_Validating);
                this.txtEditedByUserID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEditedByUserID_Validating);
                this.txtPaidAmount.Validating += new System.ComponentModel.CancelEventHandler(this.txtPaidAmount_Validating);

                this.txtDiscountOnTotal.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDiscountPercent.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);


                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSaleInvoice_KeyDown);
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
                this.txtCostSalseAccountID.Validating += txtCostSalseID_Validating;
                this.txtSalesRevenueAccountID.Validating += txtSalesRevenueID_Validating;
                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);
                ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;


                //RefreshOffers();
                DoNew();
              
                SplashScreenManager.CloseForm(false);

                string startupPath = Directory.GetCurrentDirectory() + "\\";
                 var  Type = new FileStream(@startupPath + "typevat.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(Type, Encoding.UTF8))
                {
                    Typevat = streamReader.ReadToEnd();
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
        void txtCostSalseID_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
            DataRow[] row = dt.Select("AccountID=" + txtCostSalseAccountID.Text);
            if (Comon.cInt(row.Length) > 0)
                lblCostSalseAccountName.Text = row[0]["ArbName"].ToString();

        }

        void txtSalesRevenueID_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
            DataRow[] row = dt.Select("AccountID=" + txtSalesRevenueAccountID.Text);
            if (Comon.cInt(row.Length) > 0)
                lblSalesRevenueAccountName.Text = row[0]["ArbName"].ToString();

        }
        private void btnOK(object sender, EventArgs e)
        {
            SendKeys.Send("{ESC}");
            switch (alert.lblAddress.Text.ToString())
            {
                case ("btnNext"): DoFirst(); break;
                case ("btnFirst"): DoPrevious(); break;
                case ("btnPervious"): DoNext(); break;
                case ("btnLast"): DoLast(); break;
                case ("btnSearch"): DoSearch(); break;
                case ("btnNew"):

                    gridControl.Enabled = true;
                    IsNewRecord = true;
                    txtInvoiceID.Text = Sales_SaleServiceInvoicesDAL.GetNewID(MySession.GlobalBranchID, MySession.GlobalFacilityID, MySession.UserID).ToString();
                    txtRegistrationNo.Text = RestrictionsDailyDAL.GetNewID(this.Name).ToString();
                    ClearFields();
                    IdPrint = false;
                    EnabledControl(true);
                    cmbFormPrinting.EditValue = 1;
                    gridView1.Focus();
                    gridView1.MoveNext();
                    gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                    //gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
                    //  gridView1.ShowEditor();
                    simpleButton1_Click(null, null);
                    SendKeys.Send("{Enter}");
                    break;
            }
        }

        private void btnCancel(object sender, EventArgs e)
        {
            SendKeys.Send("{ESC}");
        }
        #region GridView
        void InitGrid()
        {
            lstDetail = new BindingList<Sales_SalesServiceInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;
            /******************* Columns Visible=false ********************/
            gridView1.Columns["rowhandling"].Visible = false;
            gridView1.Columns["extension"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["PackingQty"].Visible = false;
            gridView1.Columns["BAGET_W"].Visible = false;
            gridView1.Columns["STONE_W"].Visible = false;
            gridView1.Columns["DIAMOND_W"].Visible = false;
            gridView1.Columns["Equivalen"].Visible = false;
            gridView1.Columns["Caliber"].Visible = false;
            gridView1.Columns["CostPrice"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["Bones"].Visible = false;
            gridView1.Columns["Height"].Visible = false;
            gridView1.Columns["Width"].Visible = false;
            gridView1.Columns["TheCount"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;
            gridView1.Columns["DateFirst"].Visible = false;
            gridView1.Columns["DateFirstStr"].Visible = false;
            gridView1.Columns["Serials"].Visible = false;
            gridView1.Columns["InvoiceID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["StoreID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["SaleMaster"].Visible = false;
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["GroupID"].Visible = false;
            gridView1.Columns["BarCode"].Visible = true;
            gridView1.Columns["ExpiryDate"].Visible = false ;
            gridView1.Columns["Description"].Visible = false;
            gridView1.Columns["rowhandling"].Visible = false;
            /******************* Columns Visible=true *******************/
            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;
            gridView1.Columns["SizeID"].Visible = false;
             
            gridView1.Columns["HavVat"].Visible = false;
            gridView1.Columns["RemainQty"].Visible = false;
            gridView1.Columns["ItemID"].Visible = false;
            gridView1.Columns["AdditionalValue"].Visible = false;
            gridView1.Columns["Net"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].Visible = false;
            gridView1.Columns["CurrencyPrice"].Visible = false;
            gridView1.Columns["CurrencyName"].Visible = false;
            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["Caliber"].Visible = false;
            gridView1.Columns["Color"].Visible = false;
            gridView1.Columns["SpendPrice"].Visible = false;
            gridView1.Columns["CaratPrice"].Visible = false;
            gridView1.Columns["CLARITY"].Visible = false;
            
            gridView1.Columns["ArbGroupName"].Visible = false;
            gridView1.Columns["EngGroupName"].Visible = false;
            gridView1.Columns[GroupName].Visible = true;
            gridView1.Columns[GroupName].Caption = CaptionGroupName;
            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 300;
            gridView1.Columns["BarCode"].Width = 130;
            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["QTY"].Caption = CaptionQTY;
            gridView1.Columns["Total"].Caption = CaptionTotal;
            gridView1.Columns["Discount"].Caption = CaptionDiscount;
            gridView1.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView1.Columns["Net"].Caption = CaptionNet;
            gridView1.Columns["SalePrice"].Caption = CaptionSalePrice;
            gridView1.Columns["Description"].Caption = CaptionDescription;
            gridView1.Columns["HavVat"].Caption = CaptionHavVat;
            gridView1.Columns["RemainQty"].Caption = CaptionRemainQty;
            gridView1.Focus();
            /*************************Columns Properties ****************************/
            //gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
            gridView1.Columns["Total"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["Total"].OptionsColumn.AllowFocus = false;
             

            gridView1.Columns["Net"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            gridView1.Columns["Net"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            gridView1.Columns["AdditionalValue"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["AdditionalValue"].OptionsColumn.AllowFocus = false;
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
            gridView1.Columns["QTY"].OptionsColumn.ReadOnly = true;
            //gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["CurrencyEquivalent"].Caption = "المقابل بالعملة المحلية";
            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemNameService();
            gridView1.Columns[ItemName].ColumnEdit = rItem;
            gridControl.RepositoryItems.Add(rItem);

            RepositoryItemLookUpEdit rBarCode = Common.LookUpEditBarCodeSirvice();
            gridView1.Columns["BarCode"].ColumnEdit = rBarCode;
            gridControl.RepositoryItems.Add(rBarCode);

            RepositoryItemLookUpEdit rItemID = Common.LookUpEditItemIDService();
            gridView1.Columns["ItemID"].ColumnEdit = rItemID;
            gridControl.RepositoryItems.Add(rItemID);
            DataTable dt = Lip.SelectRecord("SELECT ArbName FROM Stc_ItemsGroups WHERE Cancel=0 and AccountTypeID= "+1);
            string[] companies = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companies[i] = dt.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBox = new RepositoryItemComboBox();
            riComboBox.Items.AddRange(companies);
            gridControl.RepositoryItems.Add(riComboBox);
            gridView1.Columns[GroupName].ColumnEdit = riComboBox;
            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView1.BestFitColumns();
            /******************************** Menu ***************************************/
            menu = new GridViewMenu(gridView1);
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("أسعار الصنف", new EventHandler(Price_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("بيانات الصنف", new EventHandler(item_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("كرت الصنف"));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("باركود الصنف"));

        }
        private void item_Click(object sender, EventArgs e)
        {


        }

        private void Price_Click(object sender, EventArgs e)
        {
            frmItemPricesAndCosts frm = new frmItemPricesAndCosts();
            var ItemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID");
            var SizeID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SizeID");
            frm.SizeID = Comon.cInt(SizeID);
            frm.ItemID = Comon.cLong(ItemID);
            frm.CustomerID = Comon.cLong(txtCustomerID.Text);
            frm.ShowDialog();
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], Comon.ConvertToDecimalPrice(frm.CelValue));

        }

        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            //if (e.HitInfo != null && e.HitInfo.Column.Name == "colSalePrice")
            //    if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            //        e.Menu = menu;
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


            CalculateRow();
        }
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (!gridView1.IsLastVisibleRow)
                    gridView1.MoveLast();

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
                        if (col.FieldName == "BarCode")
                            return;
                        else if (!(double.TryParse(val.ToString(), out num)) )
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Description").ToString() == ""))
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
                decimal SalePriceMezan = 0;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "BarCode" || ColName == "Net" || ColName == "SizeID" || ColName == "Total" || ColName == "ItemID" || ColName == "QTY" || ColName == "SalePrice")
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else if (!(double.TryParse(val.ToString(), out num)))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && (gridView1.GetRowCellValue(view.FocusedRowHandle, "Description").ToString() == ""))
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
                  
                    if (ColName == "QTY")
                    {
                         
                        HasColumnErrors = false;
                        e.Valid = true;
                        gridView1.SetColumnError(gridView1.Columns["QTY"], "");
                        e.ErrorText = "";
                   
                        decimal additonalVAlue = 0;

                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());

                        decimal QTY = Comon.ConvertToDecimalPrice(val.ToString());
                     
                        decimal SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("SalePrice"));
 
                        decimal TotalSale = Comon.ConvertToDecimalPrice(QTY * SalePrice);
                        decimal Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount"));
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", Comon.ConvertToDecimalPrice(TotalSale) - Comon.ConvertToDecimalPrice(Discount));

                        decimal Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
                        if (HasVat == true)
                            additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());                          
                        decimal Net = Comon.ConvertToDecimalPrice(Total  + additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Total", Total.ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], Net);
                        CalculateRow();
                    }
                 
                    if (ColName == "SalePrice")
                    {
                        ClearOffers();
                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                        decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("QTY"));
                       
                        decimal SalePrice = Comon.ConvertToDecimalPrice(val.ToString());
                        decimal TotalSale = QTY * SalePrice;
                        decimal Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount"));
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", Comon.ConvertToDecimalPrice(TotalSale) - Comon.ConvertToDecimalPrice(Discount));
                        decimal Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        if (HasVat == true)
                            additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());         
                        decimal Net = Comon.ConvertToDecimalPrice(Total +   additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Total", Net.ToString());
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                        CalculateRow();
                    }
                    if (ColName == "BarCode")
                    {
                        decimal Qty = 1;

                        e.Valid = true;
                        view.SetColumnError(gridView1.Columns[ColName], "");
                        Barcode = val.ToString();
                         
                        {
                            DataTable dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                            if (dt.Rows.Count == 0)
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNoFoundThisBarCode;
                            }
                            else
                            {
                                if (Qty == 0)
                                    Qty = 1;
                                if (SalePriceMezan == 0)
                                    SalePriceMezan = Comon.cDec(dt.Rows[0]["Saleprice"]);

                                FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), 1, SalePriceMezan);

                                CalculateRow();
                                e.Valid = true;
                                view.SetColumnError(gridView1.Columns[ColName], "");

                            }

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
                            FileItemData(dt, 1);
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
                            if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"]).ToString() == "")
                                FileItemData(dt, 1);
                            else
                                FileItemDataOffers(dt, Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"]).ToString()), "ISOFFER2");
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                }
                else if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select ItemID from Stc_Items Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        DataTable dtItem = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(dtItemID.Rows[0]["ItemID"].ToString()), UserInfo.FacilityID);
                        if (dtItem.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {

                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dtItemID.Rows[0]["ItemID"].ToString()));
                            gridView1.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);
                            FileItemData(dtItem,1);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
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
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNoFoundSizeForItem;
                            }
                            else
                            {
                                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"]).ToString() == "")
                                    FileItemData(dt, 1);
                                else
                                    FileItemDataOffers(dt, Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"]).ToString()), "ISOFFER2");
                              //  FileItemData(dt,1);
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
                        Messages.MsgWarning(Messages.TitleWorning, "لا يمكن ان يكون الخصم أكبر من النسبة المسموح بها للخصم على مستوى الصنف ");
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNotAllowedPercentDiscount;
                    }
                    else
                    {
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", Comon.ConvertToDecimalPrice(Total) - Comon.ConvertToDecimalPrice(val.ToString()));
                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                       
                        Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        if (HasVat == true)
                            additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                     
                     
                        decimal Net = Comon.ConvertToDecimalPrice(Total +   additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Total", Net.ToString());

                    }
                    CalculateRow();
                }
            }
           CalculateRow(); 
        }
        void btnCilick(string Barcode, decimal QtyInput,int ItemID,decimal SalePrice=0)
        {
            try
            {
                int flag = 0;
                decimal SalePriceView=0;
                decimal TotalPriceView=0;
                decimal NetPriceView=0;
                decimal VatPriceView=0;
                gridView1.PostEditor();
                // gridView1.AddNewRow();
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], Barcode);
                var itemGroup = FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), QtyInput,SalePrice);
                decimal QtyIn = 0;
                //CalculateRow();
                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                gridView1.FocusedColumn = gridView1.VisibleColumns[0];

                //////////////////////
                if (gridView1.IsNewItemRow(rowIndex))
                {
                    decimal QtQld = 0;
                    int CurentRow = 0;

                    for (int j = 0; j < gridView1.RowCount - 1; ++j)
                    {

                        string BarcodeCurent = gridView1.GetRowCellValue(j, "BarCode").ToString();

                        if (BarcodeCurent.Equals(Barcode.ToString()))
                        {
                            QtQld = Comon.cDec(gridView1.GetRowCellValue(j, "QTY").ToString());
                            if (gridView1.IsNewItemRow(j - 2) == false)
                            {

                                if (BarcodeCurent.Equals(Barcode.ToString()))
                                {
                                    CurentRow = j;
                                    QtQld++;
                                }
                            }
                        }
                    }
                    if (QtQld > 0)
                    {
                        gridView1.SetRowCellValue(CurentRow, gridView1.Columns["QTY"], QtQld);
                        gridView1.DeleteRow(rowIndex);
                        decimal QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(CurentRow, "QTY").ToString());
                        decimal SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(CurentRow, "SalePrice").ToString());
                        decimal DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(CurentRow, "Discount"));
                        decimal TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                        decimal TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                        decimal AdditionalAmountRow = 0;
                        decimal NetRow = Comon.ConvertToDecimalQty(TotalRow + AdditionalAmountRow);
                        gridView1.SetRowCellValue(CurentRow, gridView1.Columns["Total"], TotalRow);
                        gridView1.SetRowCellValue(CurentRow, gridView1.Columns["AdditionalValue"], AdditionalAmountRow);
                        gridView1.SetRowCellValue(CurentRow, gridView1.Columns["Net"], NetRow);
                        CalculateRow();
                    }
                }
                //////////////////////
                for (int i = 0; i < gridView1.RowCount - 1; ++i)
                {
                    if (i == rowIndex)
                        
                        if (gridView1.IsNewItemRow(rowIndex))
                            continue;
                        else
                        {
                            if (gridView1.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()) && gridView1.GetRowCellValue(i, "Description").Equals(""))
                            {
                                //if (gridView1.IsNewItemRow(rowIndex))
                                //    gridView1.DeleteRow(rowIndex);
                                //QtyIn = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"])) + 1;
                                //gridView1.SetRowCellValue(i, gridView1.Columns["QTY"], QtyIn);

                                
                                if (gridView1.GetRowCellValue(i, "Description").Equals("IsPercent"))
                                {
                                    decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["Height"]).ToString()));
                                    decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                                    gridView1.SetRowCellValue(i, gridView1.Columns["Discount"], total);
                                    gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                    gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                                    flag = 1;
                                   // GetNewOffers(itemGroup, Barcode, QtyIn, Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"])),i);
                                    return;
                                }

                                else
                                {

                                    gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                    gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                                    flag = 1;
                                   // GetNewOffers(itemGroup, Barcode, QtyIn,Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"])),i);
                                    return;


                                }
                            }
                        }
                    if (i==gridView1.RowCount-2)
                    {
                        if (gridView1.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()) && gridView1.GetRowCellValue(i, "Description").Equals(""))
                        {
                            QtyIn = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"])) + 1;

                            if (Barcode.ToString().Substring(0, 1) == ".")
                            {
                                QtyIn = Comon.ConvertToDecimalPrice(Barcode.ToString().Substring(1, Barcode.Length));


                                gridView1.SetRowCellValue(i, gridView1.Columns["QTY"], QtyIn);
                                SalePriceView = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice"));
                                var VatPriceViewrow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));

                                TotalPriceView = Comon.ConvertToDecimalPrice(QtyIn * SalePriceView);
                                VatPriceView = Comon.ConvertToDecimalPrice(QtyIn * VatPriceViewrow);
                                NetPriceView = Comon.ConvertToDecimalPrice(TotalPriceView + VatPriceViewrow);

                                gridView1.SetRowCellValue(i, gridView1.Columns["Total"], TotalPriceView);
                                gridView1.SetRowCellValue(i, gridView1.Columns["AdditionalValue"], VatPriceView);
                                gridView1.SetRowCellValue(i, gridView1.Columns["Net"], NetPriceView);


                                if (gridView1.IsNewItemRow(rowIndex))
                                    gridView1.DeleteRow(rowIndex);
                            }
                            if (gridView1.GetRowCellValue(i, "Description").Equals("IsPercent"))
                            {
                                decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["Height"]).ToString()));
                                decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                                gridView1.SetRowCellValue(i, gridView1.Columns["Discount"], total);
                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                                flag = 1;
                                //  GetNewOffers(itemGroup, Barcode, QtyIn, Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"])),i);
                                return;
                            }

                            else
                            {

                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                                flag = 1;
                                ////    GetNewOffers(itemGroup, Barcode, QtyIn, Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"])),i);
                                return;


                            }

                        }
                        if (Barcode.ToString().Substring(0, 1) == "+")

                        {

                            QtyIn = Comon.ConvertToDecimalPrice(Barcode.ToString().Substring(1, Barcode.Length - 1));
                            string q = gridView1.GetRowCellValue(gridView1.RowCount - 2, "QTY").ToString();
                            QtyIn = Comon.ConvertToDecimalPrice(q) + 1;

                            gridView1.SetRowCellValue(gridView1.RowCount - 2, gridView1.Columns["QTY"], QtyIn);
                            if (gridView1.IsNewItemRow(rowIndex))
                                gridView1.DeleteRow(rowIndex);
                            if (gridView1.GetRowCellValue(i, "Description").Equals("IsPercent"))
                            {
                                decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["Height"]).ToString()));
                                decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                                gridView1.SetRowCellValue(i, gridView1.Columns["Discount"], total);
                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                                flag = 1;
                                //  GetNewOffers(itemGroup, Barcode, QtyIn, Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"])),i);
                                return;
                            }

                            else
                            {

                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                                flag = 1;
                                ////    GetNewOffers(itemGroup, Barcode, QtyIn, Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"])),i);
                                return;


                            }

                        }
                        if (Barcode.ToString().Substring(0, 1) == ".")
                        {
                            var BarcodeItem = gridView1.GetRowCellValue(i, "BarCode");

                            DataTable dt = Stc_itemsDAL.GetItemData(BarcodeItem.ToString(), UserInfo.FacilityID);

                            gridView1.SetRowCellValue(gridView1.RowCount - 2, gridView1.Columns["QTY"], QtyInput);
                            SalePriceView = Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString());
                            TotalPriceView = Comon.ConvertToDecimalPrice(QtyInput * SalePriceView);
                            NetPriceView = Comon.ConvertToDecimalPrice(QtyInput * SalePriceView);


                            gridView1.SetRowCellValue(i, gridView1.Columns["Total"], TotalPriceView);
                            gridView1.SetRowCellValue(i, gridView1.Columns["AdditionalValue"], VatPriceView);
                            gridView1.SetRowCellValue(i, gridView1.Columns["Net"], NetPriceView);

                            if (gridView1.IsNewItemRow(rowIndex))
                                gridView1.DeleteRow(rowIndex);
                            if (gridView1.GetRowCellValue(i, "Description").Equals("IsPercent"))
                            {
                                decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["Height"]).ToString()));
                                decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                                gridView1.SetRowCellValue(i, gridView1.Columns["Discount"], total);
                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                                flag = 1;
                                //  GetNewOffers(itemGroup, Barcode, QtyIn, Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"])),i);
                                return;
                            }

                            else
                            {

                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                                flag = 1;
                                ////    GetNewOffers(itemGroup, Barcode, QtyIn, Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"])),i);
                                return;


                            }

                        }
                    }
                }
            }
            catch
            {
            }
        }
        private DataRow[] getitemGroup(int groupID, int ItemID,int SizeID)
        {
            DataRow[] drgroupItem = null;
            drgroupItem = dtPriceItemOffers.Select("((FromGroupID<=" + groupID + "and ToGroupID>=" + groupID + " )AND(FromItemID<=" + ItemID + "and ToItemID>=" + ItemID + " ) and (FromSizeID<=" + SizeID + "and ToISizeID>=" + SizeID + " )) or((FromItemID<=" + ItemID + "and ToItemID>=" + ItemID + " ) and (FromSizeID<=" + SizeID + "and ToISizeID>=" + SizeID + " ))OR ((FromItemID<=" + ItemID + "and ToItemID>=" + ItemID + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + groupID + "and ToGroupID>=" + groupID + " ) ");
            return drgroupItem;
        }

        void GetNewOffers(DataRow[] itemGroup, string Barcode, decimal QtyIn,int ItemID,int index)
        { 

            if (itemGroup != null && itemGroup.Length > 0)
            {


                if (Comon.cInt(itemGroup[0]["IsOffers"].ToString()) > 0)
                {

                    decimal QtyOffers;
                    decimal QTYINOFFERS=checkIfExist(Barcode, 1);
                    if (Comon.cInt(itemGroup[0]["IsTakeOne"].ToString()) > 0 )
                    {
                        AddnewItem(Barcode, Comon.ConvertToDecimalPrice(1), "ISOFFER0");

                    }
                    else if (Comon.cInt(itemGroup[0]["IsGetSame"].ToString()) > 0 )
                    {

                        QtyOffers = Comon.ConvertToDecimalQty(itemGroup[0]["GetSameAmount"].ToString());
                        decimal qtyinpo = Comon.ConvertToDecimalQty(itemGroup[0]["SetSameAmount"].ToString());
                        decimal qtyinpo1 = 1;
                        //if (Comon.cInt(itemGroup[0]["FromSizeID"].ToString()) == 0 && Comon.cInt(itemGroup[0]["ToISizeID"].ToString()) == 0)
                        //{

                        //    QtyIn = GetQtyByItem(ItemID);
                        //}
                        QtyIn = QtyIn + QTYINOFFERS;
                        if (QtyIn > QtyOffers) {
                      
                                if (Comon.cInt(itemGroup[0]["ISRepeat"].ToString()) > 0)
                                    qtyinpo1 = Decimal.Truncate(QtyIn / QtyOffers);

                                qtyinpo = qtyinpo1 * Comon.ConvertToDecimalQty(itemGroup[0]["SetSameAmount"].ToString());
                                gridView1.SetRowCellValue(index, gridView1.Columns["QTY"], QtyIn - qtyinpo);
                            
                            AddnewItem(Barcode, qtyinpo, "ISOFFER1");
                        
                        }
                           


                    }

                    else if (Comon.cInt(itemGroup[0]["IsGetOnther"].ToString()) > 0 && checkIfExist(itemGroup[0]["BarCode"].ToString(), 1)>0)
                    {
                 //       QtyOffers = Comon.ConvertToDecimalQty(itemGroup[0]["GetOntherAmount"].ToString());
                       
                 //       decimal qtyinpo = Comon.ConvertToDecimalQty(itemGroup[0]["SetOntherAmount"].ToString());

                 ////    decimal qtyinpo1 = Decimal.Truncate(QtyIn / QtyOffers);
                 //       decimal qtyinpo1 = 1;
                 //    if (Comon.cInt(itemGroup[0]["FromSizeID"].ToString()) == 0 && Comon.cInt(itemGroup[0]["ToISizeID"].ToString()) == 0)
                 //    {
                         
                 //        QtyIn = GetQtyByItem(ItemID);
                 //    }

                 //       if (QtyIn >= QtyOffers)
                 //       {
                            
                 //           if(Comon.cInt(itemGroup[0]["ISRepeat"].ToString())>0)
                 //               qtyinpo1 = Decimal.Truncate(QtyIn / QtyOffers);
                                
                         
                 //           qtyinpo = (qtyinpo1 * Comon.ConvertToDecimalQty(itemGroup[0]["SetOntherAmount"].ToString()));
                 //           AddnewItem(itemGroup[0]["BarCode"].ToString(), qtyinpo, "ISOFFER2");


                 //       }
                    }



                }







            }









        }

        private decimal GetQtyByItem(int ItemID)
        {
          decimal QtyByItem = 0;
            //  bool descrption = false;
            for (int i = 0; i < gridView1.RowCount - 1; ++i)
            {
              
                    //if (gridView1.IsNewItemRow(rowIndex))
                    //    continue;
                    //else
                    //{
                        if (gridView1.GetRowCellValue(i, "ItemID").Equals(ItemID) && gridView1.GetRowCellValue(i, "Description").ToString()=="")
                        {
                            QtyByItem += Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString());
                            



                        }
                    //}


              



            }

            return QtyByItem;
        }

       void ClearOffers(){

           for (int i = 0; i < gridView1.RowCount - 1; ++i)
           {
               
                       if (gridView1.GetRowCellValue(i, "Description").Equals("ISOFFER2"))
                       {

                           gridView1.SetRowCellValue(i, gridView1.Columns["SalePrice"], gridView1.GetRowCellValue(i, "Height").ToString());
                           gridView1.SetRowCellValue(i, gridView1.Columns["Description"], "");

                       }
                   

           }
       
       
       
       
       
       }






        decimal checkIfExist(string Barcode, decimal QtyInput)
        {
            string descrption = "";
            decimal QtyIN = 0;
            //  bool descrption = false;
            for (int i = 0; i < gridView1.RowCount - 1; ++i)
            {
                if (i == rowIndex)
                    if (gridView1.IsNewItemRow(rowIndex))
                        continue;
                    else
                    {
                        if (gridView1.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                        {

                            switch (gridView1.GetRowCellValue(i, "Description").ToString())
                            {

                                case ("ISOFFER1"): QtyIN = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString()); gridView1.DeleteRow(i); return QtyIN;
                                case ("ISOFFER0"): QtyIN = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString()); gridView1.DeleteRow(i); return QtyIN;
                               // case ("ISOFFER2"): QtyIN = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString()); gridView1.DeleteRow(i); return QtyIN;
                            }




                        }
                    }






            }

            return QtyIN;




        }

        void AddnewItem(string Barcode, decimal QtyInput, string description)
        {

            try
            {

                int flag = 0;
                CalculateRow();
                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                gridView1.PostEditor();
                gridView1.AddNewRow();
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], Barcode);
                FileItemDataOffers(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), QtyInput, description);
                CalculateRow();
                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                flag = 1;
                if (flag == 1)
                    return;
                //gridView1.AddNewRow();
                //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], ItemBarCode);
                //FileItemData(Stc_itemsDAL.GetItemData1(ItemBarCode, UserInfo.FacilityID));
                //CalculateRow();
                //gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //gridView1.FocusedColumn = gridView1.VisibleColumns[0];

            }
            catch
            {



            }
        }

        private void FileItemDataOffers(DataTable dt, decimal QtyIn, string description)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], dt.Rows[0]["PackingQty"].ToString());

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["ArbSizeName"].ToString());

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
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"],0);

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
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], description);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], QtyIn);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], false);
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
            }

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
                            else if (!(double.TryParse(cellValue.ToString(), out num)))
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }

                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && (gridView1.GetRowCellValue(view.FocusedRowHandle, "Description").ToString() == ""))
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {


                                view.SetColumnError(gridView1.Columns[ColName], "");
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
                    //ClearOffers();
                    CalculateRow();
                }

                else if (e.KeyData == Keys.F5)
                    grid.ShowPrintPreview();
                try
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        gridView1.PostEditor();
                        gridView1.UpdateCurrentRow();
                         
                    }
                    if (e.KeyData == Keys.F7)
                    {
                        txtNetAmount.Text = "";
                        txtNetAmount.Focus();
                    }
                    if (e.KeyData == Keys.Add)
                    {

                    }

                }
                catch { }
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
            //if (barcodeLast != "")
            //    flagError = 0;
             

            if ( gridView1.GetFocusedRowCellValue("BarCode") != null )
            {
                //if (gridView1.GetFocusedRowCellValue("BarCode") != null)
                //    if (gridView1.GetFocusedRowCellValue("BarCode").ToString() != "+")
                //        barcodeLast = gridView1.GetFocusedRowCellValue("BarCode").ToString();
            //    flagError = 1;
            //    gridView1.MoveLast();
            //    gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            //    gridView1.FocusedColumn = gridView1.VisibleColumns[0];

            }
        }
        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            FocusedRowHandle = e.FocusedRowHandle;
            try
            {
                strQty = "0";
                byte[] imgByte = null;
                if (DBNull.Value != gridView1.GetFocusedRowCellValue("ItemImage"))
                {
                    imgByte = (byte[])gridView1.GetFocusedRowCellValue("ItemImage");
                    if (imgByte !=null)
                    picItemUnits .Image = byteArrayToImage(imgByte);
                    else
                        picItemUnits.Image = null;

                }
                else
                    picItemUnits.Image = null;
            }
            catch (Exception ex)
            {
               // Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private DataRow[] FileItemData(DataTable dt, decimal QtyIn, decimal SalePrice=0)
        {
            DataRow[] drgroupItem = null;
            decimal AdditionalValue = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Stc_itemsDAL.CheckIfStopItemUnit(dt.Rows[0]["BarCode"].ToString(), MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                {

                    Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    return null;
                }
                //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], dt.Rows[0]["PackingQty"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                //////waleed
                Barcode = dt.Rows[0]["BarCode"].ToString();
                ArbName = dt.Rows[0]["ArbName"].ToString();
                UnitID = dt.Rows[0]["SizeID"].ToString();

                if (Comon.cInt(dt.Rows[0]["InvoiceID"].ToString()) == -3)
                    QtyIn = Comon.ConvertToDecimalQty(dt.Rows[0]["QTY"].ToString());
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
                    if(SalePrice>0)
                        dt.Rows[0]["SalePrice"] = SalePrice;

                }

                catch { };

                if (Typevat == "1")
                {
                }
                else
                {
                    decimal SalePricewitouvat = Comon.cDec(dt.Rows[0]["SalePrice"].ToString());
                    AdditionalValue =   Comon.ConvertToDecimalPrice(((SalePricewitouvat * 100) / (100 + MySession.GlobalPercentVat)));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());
                }
                DataTable dtt = frmItems.GetItemMoving(Comon.cLong(dt.Rows[0]["ItemID"]), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreID.Text), Comon.cInt(MySession.GlobalBranchID), 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dtt.Rows[0]["CurentAverageCostPrice"]);
                    //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                


                try
                {
                    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemImage"], dt.Rows[0]["ItemImage"]);
                }

                catch { }

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], SalePrice);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Discount"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], AdditionalValue);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], dt.Rows[0]["Caliber"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], QtyIn);
              
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], Comon.cbool(dt.Rows[0]["IsVat"].ToString()));
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
            }
            CalculateRow();
            return drgroupItem;
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
                if (col.FieldName == "BarCode" || col.FieldName == "Description" || col.FieldName == "Discount" || col.FieldName == "ExpiryDate" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SalePrice" || col.FieldName == "Net" || col.FieldName == "Total")
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }
                else if (col.FieldName == "HavVat")
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !chkForVat.Checked;
                }
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
                decimal QTYRow = 0;
                decimal SalePriceRow = 0;
                decimal TotalRow = 0;
                decimal NetRow = 0;
                decimal TotalBeforeDiscountRow = 0;
                decimal AdditionalAmountRow = 0;
                bool HavVatRow = false;
                decimal TotalQty = 0;
                //RefreshOffers();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    TotalQty += QTYRow;
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                    //TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    TotalRow = Comon.ConvertToDecimalPrice(TotalBeforeDiscountRow - DiscountRow);
                    AdditionalAmountRow = IsHavVat == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                    
                    if (Typevat == "1")
                    {                    
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(TotalRow) / Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(100 )/ Comon.ConvertToDecimalPrice(MySession.GlobalPercentVat))) : 0;                     
                    }
                    NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);
                    //gridView1.SetRowCellValue(i, gridView1.Columns["Total"], NetRow.ToString());
                    if (Comon.cDec(txtCurrncyPrice.Text) > 0)
                        gridView1.SetRowCellValue(i, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(NetRow) * Comon.cDec(Comon.cDec(txtCurrncyPrice.Text))).ToString());

                    //gridView1.SetRowCellValue(i, gridView1.Columns["Net"], NetRow.ToString());
                   // gridView1.SetRowCellValue(i, gridView1.Columns["SalePrice"], SalePriceRow.ToString());
                    gridView1.SetRowCellValue(i, gridView1.Columns["#"], (i+1).ToString());
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }

                if (rowIndex < 0)
                {
                    var ResultQTY = gridView1.GetRowCellValue(rowIndex, "QTY");
                    var ResultSalePrice = gridView1.GetRowCellValue(rowIndex, "SalePrice");
                    var ResultDiscount = gridView1.GetRowCellValue(rowIndex, "Discount");
                    var ResultHavVat = gridView1.GetRowCellValue(rowIndex, "HavVat");
                     
                     QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                     TotalQty += QTYRow;
                     SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                     DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                     HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                     HavVatRow = row == rowIndex ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(rowIndex, "HavVat"));
                     TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                     TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                     AdditionalAmountRow   = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue")):0;
                 

                    if (Typevat == "1")
                    {
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(TotalRow) / Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(100) / Comon.ConvertToDecimalPrice(MySession.GlobalPercentVat))) : 0;
                       
                    }
                    NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);
                    
                    ////////

                    //gridView1.SetRowCellValue(rowIndex, gridView1.Columns["Total"], NetRow.ToString());
                    if (Comon.cDec(txtCurrncyPrice.Text) > 0)
                        gridView1.SetRowCellValue(rowIndex, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(NetRow) * Comon.cDec(Comon.cDec(txtCurrncyPrice.Text))).ToString());

                    //gridView1.SetRowCellValue(rowIndex, gridView1.Columns["Net"], NetRow.ToString());
                    //gridView1.SetRowCellValue(rowIndex, gridView1.Columns["SalePrice"], SalePriceRow.ToString());

                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }
                lblUnitDiscount.Text = DiscountTotal.ToString("N" + MySession.GlobalPriceDigits);
                DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                if (DiscountOnTotal > 0)
                {
                    decimal Total = TotalAfterDiscount - DiscountOnTotal;
                    AdditionalAmount = (Total) / 100 * MySession.GlobalPercentVat;
                    Net = Comon.ConvertToDecimalPrice(Total + AdditionalAmount);
                }

                if (Typevat == "1")
                {
                    TotalAfterDiscount = Comon.ConvertToDecimalPrice((Net )- DiscountOnTotal);
                    TotalBeforeDiscount = Comon.ConvertToDecimalPrice(Net) + Comon.ConvertToDecimalPrice(lblUnitDiscount.Text);
                }


                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);
                lblTotalQTY.Text = TotalQty + "";
                lblAdditionaAmmount.Text = Comon.ConvertToDecimalPrice(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = Comon.ConvertToDecimalPrice(Net).ToString("N" + MySession.GlobalPriceDigits);
                txtPrifitAmount.Text = Comon.ConvertToDecimalPrice(Comon.cDec(lblNetBalance.Text) / 100 * 2).ToString();
                if (Comon.cDbl(lblDiscountTotal.Text) > 0)
                {
                    lblDiscountDebitAccountID.Tag = "ImportantFieldGreaterThanZero";
                    lblDiscountCaption.Enabled = true;
                    lblDiscountDebitAccountID.Enabled = true;
                    lblDiscountDebitAccountName.Enabled = true;
                }
                else
                {
                    lblDiscountDebitAccountID.Tag = "IsNumber";
                    lblDiscountCaption.Enabled = false;
                    lblDiscountDebitAccountID.Enabled = false;
                    lblDiscountDebitAccountName.Enabled = false;
                }
                int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                if (isLocalCurrncy > 1)
                {
                    decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                    lblCurrencyEqv.Text = Comon.cDec(Comon.cDec(lblNetBalance.Text) * Comon.cDec(txtCurrncyPrice.Text)) + "";
                }
                else
                {
                    txtCurrncyPrice.Text = "1";
                    lblCurrencyEqv.Visible = false;
                    lblCurrncyPric.Visible = false;
                    lblcurrncyEquvilant.Visible = false;
                    txtCurrncyPrice.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private List<QTyArray> getQtyArry(int ItemID, int FromSizeID, int ToSizeID, int FromSizeOnther, int ToSizeOnther, int ItemIDOnther)
        {
            List<QTyArray> QTyArrayList = new List<QTyArray>();


            //   QTyArray qTyArray = new QTyArray();
            gridView1.PostEditor();
            decimal QTYIN = 0;
            decimal IsInOfferQty = 0;
              decimal QtyForOffers = 0;
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                if (Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"]).ToString()) == ItemID && (Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["SizeID"]).ToString()) >= FromSizeID && Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["SizeID"]).ToString()) <= ToSizeID))
                {

                    QTYIN = QTYIN + Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"]).ToString());

                }

                if (Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"]).ToString()) == ItemIDOnther && (Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["SizeID"]).ToString()) >= FromSizeOnther && Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["SizeID"]).ToString()) <= ToSizeOnther))
                {
                    if (gridView1.GetRowCellValue(i, gridView1.Columns["Description"]).ToString() == "ISOFFER2")
                        IsInOfferQty = IsInOfferQty + Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"]).ToString());
                    else {
                        QtyForOffers = QtyForOffers + Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"]).ToString());
                        QTyArray qr = new QTyArray(QTYIN, IsInOfferQty, QtyForOffers, i, Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, gridView1.Columns["QTY"]).ToString()));
                        QTyArrayList.Add(qr);
                    }
                }

            }
            if (QTyArrayList.Count < 1)
                QTyArrayList.Add(new QTyArray(QTYIN, IsInOfferQty, QtyForOffers, 0, 0));
            QTyArrayList[0].QTYIN = QTYIN;
            QTyArrayList[0].IsInOfferQty = IsInOfferQty;
            QTyArrayList[0].QtyForOffers = QtyForOffers;
            return QTyArrayList;
        }

        private decimal[] getQtyArry(int ItemID, int FromSizeID, int ToSizeID)
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion
        #region Function
        private void ShortcutOpen()
        {
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;
            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                frmCustomers frm = new frmCustomers();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                frmStores frm = new frmStores();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
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
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
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
                //frmSalesDelegates frm = new frmSalesDelegates();
                //if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                //{
                //    if (UserInfo.Language == iLanguage.English)
                //        ChangeLanguage.EnglishLanguage(frm);
                //    frm.Show();
                //}
                //else
                //    frm.Dispose();
            }
            else if (FocusedControl.Trim() == txtSellerID.Name)
            {
                frmSellers frm = new frmSellers();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
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
                try
                {
                    if (gridView1.FocusedColumn.Name == "colItemID" || gridView1.FocusedColumn.Name == "col" + ItemName || gridView1.FocusedColumn.Name == "colBarCode")
                    {

                        if (Barcode != string.Empty)
                        {
                            string val = Barcode;
                            frmRemindQtyItemFromCahier frm = new frmRemindQtyItemFromCahier();
                            frm.txtBarCode.Text = val.ToString();
                            frm.txtArbName.Text = ArbName.ToString();
                            frm.cmbUnits.EditValue = UnitID.ToString();
                            frm.ShowDialog();
                            val = frm.txtBarCode.Text;
                        }


                    }
                    else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                    {
                        frmSizingUnits frm = new frmSizingUnits();
                        if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm);
                            frm.Show();
                        }
                        else
                            frm.Dispose();
                    }
                }
                catch 
                { 
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
            string Condition = " Where 1=1";
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;
            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
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
            if (FocusedControl.Trim() == txtCostSalseAccountID.Name)
            {
                // if (!MySession.GlobalAllowChangefrmSaleCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostSalseAccountID, lblCostSalseAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostSalseAccountID, lblCostSalseAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtSalesRevenueAccountID.Name)
            {
                // if (!MySession.GlobalAllowChangefrmSaleCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtSalesRevenueAccountID, lblSalesRevenueAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtSalesRevenueAccountID, lblSalesRevenueAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }        
            else if (FocusedControl.Trim() == lblCreditAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleCreditAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblCreditAccountID, lblCreditAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblCreditAccountID, lblCreditAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == lblDebitAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleDebitAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblDebitAccountID, lblDebitAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblDebitAccountID, lblDebitAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == lblDiscountDebitAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblDiscountDebitAccountID, lblDiscountDebitAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblDiscountDebitAccountID, lblDiscountDebitAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == lblNetAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleNetAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblNetAccountID, lblNetAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblNetAccountID, lblNetAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == lblAdditionalAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleAdditionalAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblAdditionalAccountID, lblAdditionalAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblAdditionalAccountID, lblAdditionalAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;
                if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoiceIsService", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoiceIsService", "البـاركـود", MySession.GlobalBranchID);
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

                if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }

                if (FocusedControl == txtCostSalseAccountID.Name)
                {
                    txtCostSalseAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostSalseID_Validating(null, null);
                }
                if (FocusedControl == txtSalesRevenueAccountID.Name)
                {
                    txtSalesRevenueAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtSalesRevenueID_Validating(null, null);
                }
                else if (FocusedControl == lblDiscountDebitAccountID.Name)
                {
                    lblDiscountDebitAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblDiscountCreditAccountID_Validating(null, null);

                }

                else if (FocusedControl == lblNetAccountID.Name)
                {
                    lblNetAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblNetAccountID_Validating(null, null);
                }
                else if (FocusedControl == lblCreditAccountID.Name)
                {
                    lblCreditAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblCreditAccountID_Validating(null, null);
                }
                else if (FocusedControl == lblDebitAccountID.Name)
                {
                    lblDebitAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblDebitAccountID_Validating(null, null);
                }
                else if (FocusedControl == lblAdditionalAccountID.Name)
                {
                    lblAdditionalAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblAdditionalAccountID_Validating(null, null);
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
                else if (FocusedControl == txtCustomerID.Name)
                {
                    txtCustomerID.Text = cls.PrimaryKeyValue.ToString();
                    txtCustomerID_Validating(null, null);
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
                        DataTable dt= Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID);
                        FileItemData(dt, 1);
                        CalculateRow();
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
                            FileItemData(Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(itemID), SizeID, UserInfo.FacilityID),1);
                            CalculateRow();
                        }
                    }
                }
            }
        }
        public void ReadRecord(long InvoiceID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                   
                    dt = Sales_SaleServiceInvoicesDAL.frmGetDataDetalByID(InvoiceID, UserInfo.BRANCHID, UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        button1.Visible = true;
                        //btnShowCost.Visible = true;
                        //Validate
                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);
                        txtRegTime.Text = Comon.ConvertSerialToTime(dt.Rows[0]["RegTime"].ToString());
                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        StopSomeCode = true;
                        cmbMethodID.EditValue = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());
                        StopSomeCode = false;
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurencyID"].ToString());
                        cmbNetType.EditValue = Comon.cDbl(dt.Rows[0]["NetType"].ToString());
                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);
                        txtCostSalseAccountID.Text = dt.Rows[0]["CostSalseAccountID"].ToString();
                        txtCostSalseID_Validating(null, null);
                        txtSalesRevenueAccountID.Text = dt.Rows[0]["SalesRevenueAccountID"].ToString();
                        txtSalesRevenueID_Validating(null, null);
                        //lblCutAmount.Text = dt.Rows[0]["InsuranceAmmount"].ToString();
                        
                        txtMobileNo.Text = dt.Rows[0]["CustomerMobile"].ToString();
                        txtSellerID.Text = dt.Rows[0]["SellerID"].ToString();
                        txtSellerID_Validating(null, null);
                        txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        txtDelegateID_Validating(null, null);

                        txtEnteredByUserID.Text = dt.Rows[0]["UserID"].ToString();
                        txtEnteredByUserID_Validating(null, null);

                        txtEditedByUserID.Text = dt.Rows[0]["EditUserID"].ToString();
                        txtEditedByUserID_Validating(null, null);

                        //Account
                        lblDebitAccountID.Text = dt.Rows[0]["DebitAccount"].ToString();
                        lblDebitAccountID_Validating(null, null);

                        lblCreditAccountID.Text = dt.Rows[0]["CreditAccount"].ToString();
                        lblCreditAccountID_Validating(null, null);

                        lblAdditionalAccountID.Text = dt.Rows[0]["AdditionalAccount"].ToString();
                        lblAdditionalAccountID_Validating(null, null);

                        lblNetAccountID.Text = dt.Rows[0]["NetAccount"].ToString();
                        lblNetAccountID_Validating(null, null);
                        lblDiscountDebitAccountID.Text = dt.Rows[0]["DiscountDebitAccount"].ToString();
                        lblDiscountCreditAccountID_Validating(null, null);
                        //Masterdata
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();


                        //Date
                 
                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()) == "")
                            txtInvoiceDate.Text = "";

                        else

                            txtInvoiceDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);


                        //   txtInvoiceDate.DateTime = Convert.ToDateTime(Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()), culture);

                        //txtInvoiceDate.DateTime.Date.ToString ("dd/MM/yyyy");

                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["WarningDate"].ToString()) == "")
                            txtWarningDate.Text = "";

                        else

                            txtWarningDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["WarningDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);




                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["CheckSpendDate"].ToString()) == "")
                            txtCheckSpendDate.Text = "";

                        else
                            txtCheckSpendDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["CheckSpendDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

                        //txtInvoiceDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString());
                        //txtWarningDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["WarningDate"].ToString());
                        //txtCheckSpendDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["CheckSpendDate"].ToString());

                        //Ammount

                        txtCheckID.Text = dt.Rows[0]["CheckID"].ToString();

                        txtNetAmount.Text = dt.Rows[0]["NetAmount"].ToString();
                        txtNetProcessID.Text = dt.Rows[0]["NetProcessID"].ToString();

                        txtVatID.Text = dt.Rows[0]["VatID"].ToString();

                        txtDiscountOnTotal.Text = dt.Rows[0]["DiscountOnTotal"].ToString();

                        //حقول محسوبة 
                        lblUnitDiscount.Text = "0";
                        lblDiscountTotal.Text = "0";

                        lblInvoiceTotal.Text = dt.Rows[0]["InvoiceTotal"].ToString();
                        txtDiscountOnTotal_Validating(null, null);
                        lblAdditionaAmmount.Text = dt.Rows[0]["AdditionaAmountTotal"].ToString();
                        lblNetBalance.Text = dt.Rows[0]["NetBalance"].ToString();
                        //GridVeiw

                        gridControl.DataSource = dt;
                       // gridControl1.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        EnabledControl(false);
                        CalculateRow();
                        simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                        simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                        simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                        MethodID = Comon.cInt(cmbMethodID.EditValue.ToString());
                        if (MethodID == 1)
                        {
                            simpleButton1.Appearance.BackColor = Color.Goldenrod;
                            simpleButton1.Appearance.BackColor2 = Color.White;
                            simpleButton1.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;

                        }


                        if (MethodID == 3)
                        {
                            simpleButton2.Appearance.BackColor = Color.Goldenrod;
                            simpleButton2.Appearance.BackColor2 = Color.White;
                            simpleButton2.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
                        }

                        if (MethodID == 5)
                        {
                            simpleButton3.Appearance.BackColor = Color.Goldenrod;
                            simpleButton3.Appearance.BackColor2 = Color.White;
                            simpleButton3.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
                        }

                        Validations.DoReadRipon(this, ribbonControl1);
                    }
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public static void PropertiesGridView(DevExpress.XtraGrid.Views.Grid.GridView Grd, string frmName)
        {
            
            Grd.Appearance.OddRow.BackColor = Color.White;
            Grd.Appearance.EvenRow.BackColor = Color.LightSteelBlue;
            Grd.RowSeparatorHeight = 2;
            Grd.RowHeight = 25;
            string StrSQL = "Select * from ColorSettingGridVeiw Where FormName='" + frmName + "'";
            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(StrSQL);
            if (dt.Rows.Count > 0)
            {
                Grd.Appearance.OddRow.BackColor = Color.FromName(dt.Rows[0]["COLORODD"].ToString());
                Grd.Appearance.EvenRow.BackColor = Color.FromName(dt.Rows[0]["COLOREVEN"].ToString()); ;
            }
        }
        public void ReadRecordHand(long InvoiceID, bool flag = false)
        {
            try
            {

                ClearFields();
                {

                    dt = Sales_SaleServiceInvoicesDAL.frmGetDataDetalHandByID(InvoiceID, UserInfo.BRANCHID, UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {

                        IsNewRecord = false;
                        gridControl.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        CalculateRow();
                        DoEdit();
                        gridView1.MoveLast();
                        gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                        simpleButton1_Click(null, null);

                    }
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
            try
            {
                #region get accounts declaration
                if (string.IsNullOrEmpty(MySession.GlobalDefaultSaleCreditAccountID)==false)
   
                {
                    lblCreditAccountID.Text = MySession.GlobalDefaultSaleCreditAccountID;
                    txtStoreID.Text = MySession.GlobalDefaultSaleCreditAccountID;
                    txtStoreID_Validating(null, null);
                    lblCreditAccountID_Validating(null, null);
                }

                if (string.IsNullOrEmpty(MySession.GlobalDefaultSalesRevenueAccountID) == false)                 
                {
                    txtSalesRevenueAccountID.Text = MySession.GlobalDefaultSalesRevenueAccountID;
                    txtSalesRevenueID_Validating(null, null);
                }
                if (string.IsNullOrEmpty(MySession.GlobalDefaultSaleDebitAccountID) == false)                    
                {
                    lblDebitAccountID.Text = MySession.GlobalDefaultSaleDebitAccountID;
                    lblDebitAccountID_Validating(null, null);
                }

                if (string.IsNullOrEmpty(MySession.GlobalDefaultSaleNetTypeID) == false)               
                    lblNetAccountID.Text = MySession.GlobalDefaultSaleNetTypeID;
                if (string.IsNullOrEmpty(MySession.GlobalDefaultSalesAddtionalAccountID) == false)
                   
                    lblAdditionalAccountID.Text = MySession.GlobalDefaultSalesAddtionalAccountID;

                if (string.IsNullOrEmpty(MySession.GlobalDefaultCostSalseAccountID) == false)
                    
                {
                    txtCostSalseAccountID.Text = MySession.GlobalDefaultCostSalseAccountID;
                    txtCostSalseID_Validating(null, null);
                }
                if (string.IsNullOrEmpty(MySession.GlobalDefaultDiscountSalseAccountID) == false)
                    
                {
                    lblDiscountDebitAccountID.Text = MySession.GlobalDefaultDiscountSalseAccountID;
                    lblDiscountCreditAccountID_Validating(null, null);

                }
            }
            catch (Exception ex) { }
             
            
            
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
                button1.Visible = false;
                btnShowCost.Visible = false;
                userControl11.BarCode = "000000000";
                txtPrifitAmount.Text = "0";
                lblCbalance.Text = "0";
                lblCutAmount.Text = "0";
                txtMobileNo.Text = "";
                DiscountCustomer = 0;
                txtCustomerName.Text = "";
                txtPaidAmount.Text = "";
                lblRemaindAmount.Text = "";
                txtVatID.Text = "";
                txtDocumentID.Text = "";
                txtCustomerID.Text = "";
                txtDelegateID.Text = "";
                lblCustomerName.Text = "";
                lblDelegateName.Text = "";
                txtNotes.Text = "";
                /////////////////////////////
                txtCustomerID.Tag = " ";
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
                picItemUnits.Image = null;
                txtreqiramount.Text = "";
                txtCostSalseAccountID.Text = "";
                txtSalesRevenueAccountID.Text = "";
                lblCostSalseAccountName.Text = "";
                lblSalesRevenueAccountName.Text = "";
                lblDiscountDebitAccountID.Text = "";
                lblDiscountDebitAccountName.Text = "";
                lblNetAccountID.Text = "";
                lblNetAccountID_Validating(null, null);
                GetAccountsDeclaration();
                try
                {
                    txtEditedByUserID.Text = UserInfo.ID.ToString();
                    txtEditedByUserID_Validating(null, null);
                    txtDelegateID.Text = MySession.GlobalDefaultSaleDelegateID;
                    txtDelegateID_Validating(null, null);


                    txtCostCenterID.Text = MySession.GlobalDefaultCostCenterID;
                    txtCostCenterID_Validating(null, null);
                    txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCostCenterID;
                    txtSellerID.Text = MySession.GlobalDefaultSaleSellerID;

                    txtSellerID_Validating(null, null);
                    txtStoreID.Text = MySession.GlobalDefaultSaleStoreID;

                    txtStoreID_Validating(null, null);
                    cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultSaleCurencyID);

                    if (MySession.GlobalDefaultSalePayMethodID != "0")
                        cmbMethodID.EditValue = Comon.cInt(MySession.GlobalDefaultSalePayMethodID);
                    else
                        cmbMethodID.EditValue = 1;
                    txtCustomerName.Visible = true;
                    txtCustomerID.Visible = true;
                    lblCustomerName.Visible = true;

                    if (!MySession.GlobalAllowChangefrmSalePayMethodID)
                        switch (Comon.cInt(MySession.GlobalDefaultSalePayMethodID))
                        {
                            case 1:

                                simpleButton3.Enabled = false;
                                simpleButton2.Enabled = false;
                                simpleButton12.Enabled = false;
                                txtCustomerName.Visible = true;
                                 txtCustomerName.BringToFront();
                                break;
                            case 2:
                                simpleButton3.Enabled = false;
                                simpleButton2.Enabled = false;
                                simpleButton1.Enabled = false;
                                txtCustomerID.Visible = true;
                                lblCustomerName.Visible = true;
                                txtCustomerID.BringToFront();
                                lblCustomerName.BringToFront();
                                break;
                            case 3:
                                simpleButton3.Enabled = false;
                                simpleButton12.Enabled = false;
                                simpleButton1.Enabled = false;
                                break;
                            case 5:
                                simpleButton2.Enabled = false;
                                simpleButton12.Enabled = false;
                                simpleButton1.Enabled = false;
                                break;
                        }

                    cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultSaleCurencyID);
                }
                catch (Exception ex)
                {

                }
              
                
                txtreqiramount.Visible = false;
                lblReqiramount.Visible = false;
                
                lstDetail = new BindingList<Sales_SalesServiceInvoiceDetails>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;
                dt = new DataTable();
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
                    strSQL = "SELECT TOP 1 * FROM " + Sales_SaleServiceInvoicesDAL.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_SaleServiceInvoicesDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Sales_SaleServiceInvoicesDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Sales_SaleServiceInvoicesDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Sales_SaleServiceInvoicesDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Sales_SaleServiceInvoicesDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_SaleServiceInvoicesDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Sales_SaleServiceInvoicesDAL();

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
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                    return;
                }
                SplashScreenManager.CloseForm(false);
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

                if (IsNewRecord && gridView1.RowCount > 1)
                {
                    alert.lblAddress.Text = "btnNew";
                    FlyoutAction action = new FlyoutAction();

                    FlyoutProperties properties = new FlyoutProperties();

                    properties.Style = FlyoutStyle.Popup;

                    FlyoutDialog.Show(this, alert, action, properties);
                }
                else
                {

                    gridControl.Enabled = true;
                    IsNewRecord = true;
                    txtInvoiceID.Text = Sales_SaleServiceInvoicesDAL.GetNewID(MySession.GlobalBranchID, MySession.GlobalFacilityID, MySession.UserID).ToString();
                    txtRegistrationNo.Text = RestrictionsDailyDAL.GetNewID(this.Name).ToString();
                    ClearFields();
                    IdPrint = false;
                    EnabledControl(true);
                    cmbFormPrinting.EditValue = 1;
                    gridView1.Focus();
                    gridView1.MoveNext();
                    gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                    //gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly =  false;
                    //  gridView1.ShowEditor();
                    simpleButton1_Click(null, null);
                }
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
                if (IsNewRecord && gridView1.RowCount > 1)
                {
                    alert.lblAddress.Text = "btnLast";
                    FlyoutAction action = new FlyoutAction();

                    FlyoutProperties properties = new FlyoutProperties();

                    properties.Style = FlyoutStyle.Popup;

                    FlyoutDialog.Show(this, alert, action, properties);

                    SendKeys.Send("{ESC}");
                }
                else  
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
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Total", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDateStr", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.DateTime"));
            dtItem.Columns.Add("Bones", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("HavVat", System.Type.GetType("System.Boolean"));
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
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
                dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());
                dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString());
                dtItem.Rows[i]["ExpiryDate"] = gridView1.GetRowCellValue(i, "ExpiryDate");
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView1.GetRowCellValue(i, "HavVat").ToString());
                dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                dtItem.Rows[i]["AdditionalValue"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
        }

        protected override void DoSave()
        {
            try
            {
                
                gridView1.MoveLastVisible();
                if (!Validations.IsValidForm(this))
                    return;
                if (!IsValidGrid())
                    return;
                if (!Validations.IsValidFormCmb(cmbCurency))
                    return;

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

                //Application.DoEvents();
                //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                if (Comon.ConvertToDecimalPrice(lblNetBalance.Text) < Comon.ConvertToDecimalPrice(txtNetAmount.Text))
                {
                   // SplashScreenManager.CloseForm(false);
                    txtNetAmount.Focus();
                    txtNetAmount.ToolTip = "مبلغ الشبكة  اكبر من الصافي ";
                    Messages.MsgExclamationk(Messages.TitleInfo, "مبلغ الشبكة  اكبر من الصافي ");

                    Validations.ErrorText(txtNetAmount, txtNetAmount.ToolTip);
                    return;
                }
                if (Comon.ConvertToDecimalPrice(txtNetAmount.Text) <= 0 && Comon.cInt(cmbMethodID.EditValue) == 5)
                {
                   // SplashScreenManager.CloseForm(false);
                   
                    txtNetAmount.Focus();
                    txtNetAmount.ToolTip = "مبلغ الشبكة = 0 ";
                    Messages.MsgExclamationk(Messages.TitleInfo, "مبلغ الشبكة = 0 ");

                    Validations.ErrorText(txtNetAmount, txtNetAmount.ToolTip);
                    return;

                }
                if (Lip.CheckTheCustomerAllowAgeDebtOrNot(Comon.cDbl(txtCustomerID.Text), MySession.GlobalBranchID) == 1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, "لا يمكن الحفظ لسبب تجاوز عمر المديونية للعميل " + txtCustomerName.Text + " ولم يتم السداد");
                    return;
                }
                else if (Lip.CheckTheCustomerAllowAgeDebtOrNot(Comon.cDbl(txtCustomerID.Text), MySession.GlobalBranchID) == 2)
                {
                    SplashScreenManager.CloseForm(false);
                    bool Yes = Messages.MsgQuestionYesNo(Messages.TitleInfo, "لقد تجاوز عمر المديونية للعميل " + txtCustomerName.Text + " ولم يتم السداد ... هل تريد متابعة الحفظ ");
                    if (!Yes)
                        return;
                }
                decimal DebitAmount = Comon.cDec(Comon.cDec(lblNetBalance.Text) - Comon.cDec(lblDiscountTotal.Text));
                //Customer
                if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(txtCustomerID.Text), MySession.GlobalBranchID, Comon.cDec(DebitAmount), 1)==1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + txtCustomerName.Text);
                    return;
                }
                else if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(txtCustomerID.Text), MySession.GlobalBranchID, Comon.cDec(DebitAmount), 1) == 2)
                {
                    SplashScreenManager.CloseForm(false);
                    bool Yes = Messages.MsgQuestionYesNo(Messages.TitleInfo, Messages.msgAccountMaxLimitSaveOrNot + " " + txtCustomerName.Text);
                    if (!Yes)
                        return;
                }
                //box Cash
                if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblDebitAccountID.Text), MySession.GlobalBranchID, Comon.cDec(DebitAmount), 1)==1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + lblDebitAccountName.Text);
                    return;
                }
                else  if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblDebitAccountID.Text), MySession.GlobalBranchID, Comon.cDec(DebitAmount), 1) == 2)
                {
                    SplashScreenManager.CloseForm(false);
                    bool Yes = Messages.MsgQuestionYesNo(Messages.TitleInfo, Messages.msgAccountMaxLimitSaveOrNot + " " + lblDebitAccountName.Text);
                    if (!Yes)
                        return;
                }
                //Dicount Acount
                if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblDiscountDebitAccountID.Text), MySession.GlobalBranchID, Comon.cDec(lblDiscountTotal.Text), 1)==1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + lblDiscountDebitAccountName.Text);
                    return;
                }
                else if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblDiscountDebitAccountID.Text), MySession.GlobalBranchID, Comon.cDec(lblDiscountTotal.Text), 1) == 2)
                {
                    SplashScreenManager.CloseForm(false);
                    bool Yes = Messages.MsgQuestionYesNo(Messages.TitleInfo, Messages.msgAccountMaxLimitSaveOrNot + " " + lblDiscountDebitAccountName.Text);
                    if (!Yes)
                        return;
                }
                //Net Account
                if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblNetAccountID.Text), MySession.GlobalBranchID, Comon.cDec(DebitAmount), 1)==1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + lblNetAccountName.Text);
                    return;
                }
                else if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblNetAccountID.Text), MySession.GlobalBranchID, Comon.cDec(DebitAmount), 1) == 2)
                {
                    SplashScreenManager.CloseForm(false);
                    bool Yes = Messages.MsgQuestionYesNo(Messages.TitleInfo, Messages.msgAccountMaxLimitSaveOrNot + " " + lblNetAccountName.Text);
                    if (!Yes)
                        return;
                }
                Save();
            }
            catch (Exception ex)
            {

                // SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            finally
            {
                // SplashScreenManager.CloseForm(false);
            }
        }
        private int SaveStockMoveing(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(MySession.GlobalBranchID);
            objRecord.DocumentTypeID = DocumentType;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                returned.DocumentTypeID = DocumentType;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtCustomerID.Text);
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.OutPrice = Comon.cDbl(Comon.cDbl(gridView1.GetRowCellValue(i, "CostPrice").ToString())*Comon.cDbl(returned.QTY));
                returned.Bones = Comon.cDbl(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.InPrice = 0;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.Cancel = 0;
                listreturned.Add(returned);
            }
            if (listreturned.Count > 0)
            {

                objRecord.ObjDatails = listreturned;
                string Result = Stc_ItemsMoviingDAL.Insert(objRecord, IsNewRecord);

                return Comon.cInt(Result);
            }
            return 0;
        }
        private void Save()
        {
              
            gridView1.MoveLastVisible();
            if (DiscountCustomer != 0)
            {
                txtDiscountPercent.Text = DiscountCustomer.ToString();
                txtDiscountPercent_Validating(null, null);
            }


            //if (txtMobileNo.Text.Trim() == string.Empty && txtCustomerID.Text == string.Empty)
            //    addcustoe();

           //if (txtMobileNo.Text.Trim() == string.Empty && txtCustomerID.Text != string.Empty)
              //addcMobile();


            CalculateRow();  
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            txtInvoiceDate_EditValueChanged(null, null);
            Sales_SalesServiceInvoiceMaster objRecord = new Sales_SalesServiceInvoiceMaster();
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            invoiceNo = Sales_SaleServiceInvoicesDAL.GetNewID(MySession.GlobalBranchID, MySession.GlobalFacilityID, MySession.UserID).ToString();
            txtInvoiceID.Text = invoiceNo.ToString();
            objRecord.InvoiceID = Comon.cInt(invoiceNo);
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.MethodeID = Comon.cInt(cmbMethodID.EditValue);
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.NetType = Comon.cDbl(cmbNetType.EditValue);
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.CustomerName = txtCustomerName.Text.Trim();
            objRecord.CustomerMobile = txtMobileNo.Text.Trim();
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cLong(txtStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SellerID = Comon.cInt(txtSellerID.Text);
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Sale  Invoice" : "فاتوره  مبيعات ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Sale  Invoice" : " فاتوره  مبيعات "));
            objRecord.Notes = txtNotes.Text;
            //Account
            objRecord.DebitAccount = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            objRecord.DiscountDebitAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
            objRecord.CheckAccount = Comon.cDbl(lblChequeAccountID.Text);
            objRecord.NetAccount = Comon.cDbl(lblNetAccountID.Text);
            objRecord.AdditionalAccount = Comon.cDbl(lblAdditionalAccountID.Text);
            objRecord.SalesRevenueAccountID = Comon.cDbl(txtSalesRevenueAccountID.Text);
            objRecord.CostSalseAccountID = Comon.cDbl(txtCostSalseAccountID.Text);

            objRecord.NetProcessID = txtNetProcessID.Text;
            objRecord.CheckID = txtCheckID.Text;
            objRecord.VATID = txtVatID.Text;
            //Date
            objRecord.CheckSpendDate = Comon.ConvertDateToSerial(txtCheckSpendDate.Text).ToString();
            objRecord.WarningDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            objRecord.ReceiveDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            //Ammount

            objRecord.NetAmount = Comon.cDbl(txtNetAmount.Text);
            objRecord.DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = (Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text));
            objRecord.AdditionaAmountTotal = Comon.ConvertToDecimalPrice(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Comon.ConvertToDecimalPrice(lblNetBalance.Text);
            objRecord.InsuranceAmmount = Comon.cDbl(lblCutAmount.Text);
            objRecord.InsuranceAmmountAfter = Comon.cDec(txtPrifitAmount.Text);
            objRecord.Cancel = 0;
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
                objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            Sales_SalesServiceInvoiceDetails returned;
            List<Sales_SalesServiceInvoiceDetails> listreturned = new List<Sales_SalesServiceInvoiceDetails>();


            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {


                returned = new Sales_SalesServiceInvoiceDetails();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
                returned.Bones = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.Description = gridView1.GetRowCellValue(i, "Description").ToString();
                returned.StoreID = Comon.cLong(txtStoreID.Text);
                returned.Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                returned.ItemImage = null;
                 returned.ExpiryDateStr = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString().Substring(0, 10));
                returned.CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                returned.AdditionalValue = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                returned.Net = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                returned.Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                returned.HavVat = Comon.cbool(gridView1.GetRowCellValue(i, "HavVat").ToString());
                returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());

                returned.Cancel = 0;
                returned.Serials = "";
                if (returned.QTY <= 0 || returned.StoreID <= 0 || (returned.SalePrice <= 0 && returned.Description=="") || returned.SizeID <= 0 || returned.ItemID <= 0)
                    continue;
                listreturned.Add(returned);

            }

            if (listreturned.Count > 0)
            {
                string Result;
                objRecord.SaleDatails = listreturned;
                
               Result = Sales_SaleServiceInvoicesDAL.InsertUsingXML(objRecord, IsNewRecord);
               // حفظ الحركة المخزنية 
               if (Comon.cInt(Result) > 0)
               {
                   int MoveID = SaveStockMoveing(Comon.cInt(Result));
                   if (MoveID == 0)
                       Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
               }
               SplashScreenManager.CloseForm(false);
               if (Comon.cInt(Result) > 0)
               {
                   long VoucherID = 0;
                   if (MySession.GlobalInventoryType == 2)//جرد دوري 
                       VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result));
                   else if (MySession.GlobalInventoryType == 1)
                       VoucherID = SaveVariousVoucherMachinContinuousInv(Comon.cInt(Result));
                   if (VoucherID == 0)
                       Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                   else
                   {
                       Lip.ExecututeSQL("Update " + Sales_SaleServiceInvoicesDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sales_SaleServiceInvoicesDAL.PremaryKey + " = " + txtInvoiceID.Text);
                   }
               }
               if (IsNewRecord == true)
               {
                   if (Comon.cInt(Result) > 0)
                   {

                       
                     Validations.DoLoadRipon(this, ribbonControl1);
                   
                       if (falgPrint == true)
                       {
                           IsNewRecord = false;
                           txtInvoiceID.Text = Result.ToString();
                           DoPrint();
                       }
                      
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        IsNewRecord = false;
                   }
                   else
                   {
                       Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                   }
               }
               else
               {


                   if (Result != "0")
                   {
                       txtInvoiceID_Validating(null, null);
                       EnabledControl(false);
                       Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                       //if (Comon.cInt(cmbMethodID.EditValue) == 5)
                       //SaveVariousVoucher();

                   }
                   else
                   {
                       Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                   }
               }
              

               // SplashScreenManager.CloseForm(false);

                //if (IsNewRecord == true)
                //{

                //    if (Result != "0")
                //    {
                //        IsNewRecord = false;
                //        IdPrint = true;
                //        invoiceNo = Result;
                        

                //        if (Comon.cDbl(userControl11.BarCode.ToString()) > 0)
                //        {
                           
                //            decimal AddBalance = Comon.ConvertToDecimalPrice(Comon.cDec(lblNetBalance.Text) / 100 * 2);
                //            strSQL = " update Sales_Customers set Balance=Balance + " + AddBalance + "  Where Mobile='" + txtMobileNo.Text.Trim() + "'";
                //            Lip.ExecututeSQL(strSQL);

                //            //decimal Balance = Comon.ConvertToDecimalPrice(AddBalance +  Comon.cDec(lblCbalance.Text));
                //            //string Mobile =txtMobileNo.Text;
                //            //if(Mobile.Length==10)
                //            //Lip.UpdateUserBalance(Balance, Mobile);

                //            decimal Bal = Comon.ConvertToDecimalPrice(lblCutAmount.Text);
                //            if (Bal > 0)
                //            {
                //                strSQL = " update Sales_Customers set Balance=Balance - " + Bal + "  Where Mobile='" + txtMobileNo.Text.Trim() + "'";
                //                Lip.ExecututeSQL(strSQL);
                              

                //                txtMobileNo_Validating(null, null);
                //                lblCutAmount.Text = "0";
                //            }
                //        }
                //        DoPrint();
                //        
                //    }
                //    else
                //    {
                //        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                //    }

                //}
                 

            }
            else
            {
                 
               // SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }

        }
        long SaveVariousVoucherMachinContinuousInv(int DocumentID)
        {

            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType));
            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(MySession.GlobalBranchID);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = "فاتورة مبيعات سلعية";
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cBooleanToInt(chkPoste.Checked);

            // Set the UserID, RegDate, RegTime, and ComputerInfo properties of the objRecord object based on the UserInfo object and server date/time.
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            // Set the EditUserID, EditTime, EditDate, and EditComputerInfo properties of the objRecord object based on the UserInfo object and server date/time, if this is not a new record.
            if (IsNewRecord == false)
            {
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            // Create a new Acc_VariousVoucherMachinDetails object and set its properties.
            Acc_VariousVoucherMachinDetails returned;
            List<Acc_VariousVoucherMachinDetails> listreturned = new List<Acc_VariousVoucherMachinDetails>();

            //Debit
            // If the selected method ID is 1 or 2, create a new Acc_VariousVoucherMachinDetails object and set its properties.
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1 || Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
            {

                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the object's ID, branch ID, facility ID, account ID, credit, debit, declaration, and cost center ID properties based on the available controls.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblDebitAccountID.Text);

                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(lblDiscountTotal.Text));


                // Add the object to the list of returned objects.  

                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                listreturned.Add(returned);

            }
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 3)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblNetAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(lblDiscountTotal.Text));
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }

            // If the selected method ID is 4, create a new Acc_VariousVoucherMachinDetails object and set its properties.
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 4)
            {

                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the object's ID, branch ID, facility ID, account ID, and voucher ID properties.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblChequeAccountID.Text);
                returned.VoucherID = VoucherID;

                // Set the object's credit and debit properties based on the lblNetBalance control.
                returned.Credit = 0;
                returned.Debit = returned.Debit = Comon.cDbl(lblNetBalance.Text);

                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                // Add the object to the list of returned objects.
                listreturned.Add(returned);
            }

            // If the selected method ID is 5, create a new Acc_VariousVoucherMachinDetails object and set its properties.
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 5)
            {
                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the object's ID, branch ID, facility ID, account ID, and voucher ID properties.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
                returned.VoucherID = VoucherID;

                // Set the object's credit and debit properties based on the lblInvoiceTotal and lblNetAmount controls.
                returned.Credit = 0;
                returned.Debit = Comon.cDbl((Comon.cDbl(lblNetBalance.Text) - Comon.cDbl( lblDiscountTotal.Text)) - Comon.cDbl(txtNetAmount.Text));

                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);



                // Add the object to the list of returned objects.
                listreturned.Add(returned);

                // This code checks if the net amount is greater than zero and creates a new instance of the "Acc_VariousVoucherMachinDetails" class 
                // to represent the net amount in the accounting records. It then sets the relevant properties of the instance and adds it to the list of records.
                if (Comon.cDbl(txtNetAmount.Text) > 0)
                {
                    // Create a new instance of "Acc_VariousVoucherMachinDetails".
                    returned = new Acc_VariousVoucherMachinDetails();
                    // Set the properties of the instance.
                    returned.ID = 2;
                    returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.AccountID = Comon.cDbl(lblNetAccountID.Text);
                    returned.VoucherID = VoucherID;
                    returned.Credit = 0;
                    returned.Debit = Comon.cDbl(txtNetAmount.Text);
                    returned.Declaration = txtNotes.Text;
                    returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                    //// Add the instance to the list of records.
                    listreturned.Add(returned);
                }


            }
            // This code checks if the total discount is greater than zero and creates a new instance of the "Acc_VariousVoucherMachinDetails" class 
            // to represent the discount in the accounting records. It then sets the relevant properties of the instance and adds it to the list of records. 
            //Discount
            if (Comon.cDbl(lblDiscountTotal.Text) > 0)
            {
                // Create a new instance of "Acc_VariousVoucherMachinDetails".
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the properties of the instance.
                returned.ID = 4;
                returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDiscountDebitAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(lblDiscountTotal.Text);
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                // Add the instance to the list of records.
                listreturned.Add(returned);
            }


            returned = new Acc_VariousVoucherMachinDetails();
            // Set the properties of the instance.
            returned.ID = 3;
            returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtCostSalseAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;
            double TotalCost = 0;
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                TotalCost += Comon.cDbl(gridView1.GetRowCellValue(i, "CostPrice"));
            }
            returned.Debit = Comon.cDbl(TotalCost);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //// Add the instance to the list of records.
            listreturned.Add(returned);


            // This code creates a new instance of the "Acc_VariousVoucherMachinDetails" class to represent the credit sale in the accounting records. 
            // It sets the relevant properties of the instance and adds it to the list of records.
            //Credit Sale
            returned = new Acc_VariousVoucherMachinDetails();
            // Set the properties of the instance.
            returned.ID = 4;
            returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(TotalCost);
            //returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

            // Add the instance to the list of records.
            listreturned.Add(returned);

            //===
            //Vat Sale
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 5;
            returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblAdditionalAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(lblAdditionaAmmount.Text);

            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);
            //=

            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 5;
            returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtSalesRevenueAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit =Comon.cDbl( Comon.cDbl(lblInvoiceTotal.Text)-Comon.cDbl(lblAdditionaAmmount.Text));
            returned.Debit = 0;
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        long SaveVariousVoucherMachin(int DocumentID)
        {

            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType));
            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(UserInfo.BRANCHID);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cBooleanToInt(chkPoste.Checked);

            // Set the UserID, RegDate, RegTime, and ComputerInfo properties of the objRecord object based on the UserInfo object and server date/time.
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            // Set the EditUserID, EditTime, EditDate, and EditComputerInfo properties of the objRecord object based on the UserInfo object and server date/time, if this is not a new record.
            if (IsNewRecord == false)
            {
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            // Create a new Acc_VariousVoucherMachinDetails object and set its properties.
            Acc_VariousVoucherMachinDetails returned;
            List<Acc_VariousVoucherMachinDetails> listreturned = new List<Acc_VariousVoucherMachinDetails>();

            //Debit
            // If the selected method ID is 1 or 2, create a new Acc_VariousVoucherMachinDetails object and set its properties.
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1 || Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
            {

                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();
                // Set the object's ID, branch ID, facility ID, account ID, credit, debit, declaration, and cost center ID properties based on the available controls.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(UserInfo.BRANCHID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblDebitAccountID.Text);

                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(Comon.cDbl(lblInvoiceTotalBeforeDiscount.Text) - Comon.cDbl(lblDiscountTotal.Text));
                returned.DebitGold = 0;
                returned.CreditGold = 0;
                // Add the object to the list of returned objects.  
                if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                {
                    returned.DebitGold = 0;
                    // returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);
                }
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

                listreturned.Add(returned);
            }
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 3)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(UserInfo.BRANCHID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblNetAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(Comon.cDbl(lblInvoiceTotalBeforeDiscount.Text) - Comon.cDbl(lblDiscountTotal.Text));
                returned.DebitGold = 0;
                returned.CreditGold = 0;
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                listreturned.Add(returned);
            }
            // If the selected method ID is 4, create a new Acc_VariousVoucherMachinDetails object and set its properties.
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 4)
            {
                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the object's ID, branch ID, facility ID, account ID, and voucher ID properties.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(UserInfo.BRANCHID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblChequeAccountID.Text);
                returned.VoucherID = VoucherID;
                // Set the object's credit and debit properties based on the lblNetBalance control.
                returned.Credit = 0;
                returned.Debit = returned.Debit = Comon.cDbl(Comon.cDbl(lblInvoiceTotalBeforeDiscount.Text) - Comon.cDbl(lblDiscountTotal.Text));
                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                // Add the object to the list of returned objects.
                listreturned.Add(returned);
            }
            // If the selected method ID is 5, create a new Acc_VariousVoucherMachinDetails object and set its properties.
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 5)
            {
                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();
                // Set the object's ID, branch ID, facility ID, account ID, and voucher ID properties.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(UserInfo.BRANCHID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
                returned.VoucherID = VoucherID;
                // Set the object's credit and debit properties based on the lblInvoiceTotal and lblNetAmount controls.
                returned.Credit = 0;
                returned.Debit = (Comon.cDbl(Comon.cDbl(lblInvoiceTotalBeforeDiscount.Text) - Comon.cDbl(txtNetAmount.Text)) - Comon.cDbl(lblDiscountTotal.Text));
                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                // Add the object to the list of returned objects.
                listreturned.Add(returned);

                // This code checks if the net amount is greater than zero and creates a new instance of the "Acc_VariousVoucherMachinDetails" class 
                // to represent the net amount in the accounting records. It then sets the relevant properties of the instance and adds it to the list of records.
                if (Comon.cDbl(txtNetAmount.Text) > 0)
                {
                    // Create a new instance of "Acc_VariousVoucherMachinDetails".
                    returned = new Acc_VariousVoucherMachinDetails();
                    // Set the properties of the instance.
                    returned.ID = 2;
                    returned.BranchID = Comon.cInt(UserInfo.BRANCHID);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.AccountID = Comon.cDbl(lblNetAccountID.Text);
                    returned.VoucherID = VoucherID;
                    returned.Credit = 0;
                    returned.Debit = Comon.cDbl(txtNetAmount.Text);
                    returned.Declaration = txtNotes.Text;
                    returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

                    // Add the instance to the list of records.
                    listreturned.Add(returned);
                }


            }
            // This code checks if the total discount is greater than zero and creates a new instance of the "Acc_VariousVoucherMachinDetails" class 
            // to represent the discount in the accounting records. It then sets the relevant properties of the instance and adds it to the list of records. 
            //Discount
            if (Comon.cDbl(lblDiscountTotal.Text) > 0)
            {
                // Create a new instance of "Acc_VariousVoucherMachinDetails".
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the properties of the instance.
                returned.ID = 4;
                returned.BranchID = Comon.cInt(UserInfo.BRANCHID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDiscountDebitAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(lblDiscountTotal.Text);
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

                // Add the instance to the list of records.
                listreturned.Add(returned);
            }

            // This code creates a new instance of the "Acc_VariousVoucherMachinDetails" class to represent the credit sale in the accounting records. 
            // It sets the relevant properties of the instance and adds it to the list of records.
            //Credit Sale
            returned = new Acc_VariousVoucherMachinDetails();

            // Set the properties of the instance.
            returned.ID = 3;
            returned.BranchID = Comon.cInt(UserInfo.BRANCHID);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblCreditAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(Comon.cDbl(lblInvoiceTotalBeforeDiscount.Text) - Comon.cDbl(lblAdditionaAmmount.Text));
            returned.Debit = 0;
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

            // Add the instance to the list of records.
            listreturned.Add(returned);

            //===
            //Vat Sale
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 4;
            returned.BranchID = Comon.cInt(UserInfo.BRANCHID);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblAdditionalAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(lblAdditionaAmmount.Text);
            returned.Debit = 0;
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
            listreturned.Add(returned);
            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        private void SaveInvoiceHand()
        {

            gridView1.MoveLastVisible();
            if (DiscountCustomer != 0)
            {
                txtDiscountPercent.Text = DiscountCustomer.ToString();
                txtDiscountPercent_Validating(null, null);
            }

            

            CalculateRow();
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            txtInvoiceDate_EditValueChanged(null, null);
            Sales_SalesInvoiceMasterHand objRecord = new Sales_SalesInvoiceMasterHand();
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            invoiceNo = Sales_SaleServiceInvoicesDAL.GetNewID(MySession.GlobalBranchID, MySession.GlobalFacilityID, MySession.UserID).ToString();
            objRecord.InvoiceID = Comon.cInt(invoiceNo);
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.MethodeID = Comon.cInt(cmbMethodID.EditValue);
            objRecord.CurencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.NetType = Comon.cDbl(cmbNetType.EditValue);
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.CustomerName = txtCustomerName.Text.Trim();
            objRecord.CustomerMobile = txtMobileNo.Text.Trim();
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cInt(txtStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SellerID = Comon.cInt(txtSellerID.Text);
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Sale  Invoice" : "فاتوره  مبيعات ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Sale  Invoice" : " فاتوره  مبيعات "));
            objRecord.Notes = txtNotes.Text;
            //Account
            objRecord.DebitAccount = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            objRecord.DiscountDebitAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
            objRecord.CheckAccount = Comon.cDbl(lblChequeAccountID.Text);
            objRecord.NetAccount = Comon.cDbl(lblNetAccountID.Text);
            objRecord.AdditionalAccount = Comon.cDbl(lblAdditionalAccountID.Text);
            //objRecord.SaveAccountID = Comon.cDbl(txtSaveAccountID.Text);

            objRecord.NetProcessID = txtNetProcessID.Text;
            objRecord.CheckID = txtCheckID.Text;
            objRecord.VATID = txtVatID.Text;
            //Date
            objRecord.CheckSpendDate = Comon.ConvertDateToSerial(txtCheckSpendDate.Text).ToString();
            objRecord.WarningDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            objRecord.ReceiveDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            //Ammount

            objRecord.NetAmount = Comon.cDbl(txtNetAmount.Text);
            objRecord.DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = (Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text));
            objRecord.AdditionaAmountTotal = Comon.ConvertToDecimalPrice(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Comon.ConvertToDecimalPrice(lblNetBalance.Text);
            objRecord.InsuranceAmmount = Comon.cDbl(lblCutAmount.Text);
            objRecord.InsuranceAmmountAfter = Comon.cDec(txtPrifitAmount.Text);
            objRecord.Cancel = 0;
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Lip.GetServerDateSerial().ToString());

            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";

            if (IsNewRecord == false)
            {
                objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            Sales_SalesInvoiceDetailsHand returned;
            List<Sales_SalesInvoiceDetailsHand> listreturned = new List<Sales_SalesInvoiceDetailsHand>();


            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {


                returned = new Sales_SalesInvoiceDetailsHand();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
                returned.Bones = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.Description = gridView1.GetRowCellValue(i, "Description").ToString();
                returned.StoreID = Comon.cLong(txtStoreID.Text);
                returned.Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                returned.ItemImage = null;
                //returned.ExpiryDateStr = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString().Substring(0, 10));
                returned.CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                returned.AdditionalValue = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                returned.Net = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                returned.Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                if (returned.AdditionalValue == 0)
                    returned.HavVat = false;
                else
                    returned.HavVat = true;

                returned.Cancel = 0;
                returned.Serials = "";
                if (returned.QTY <= 0 || returned.StoreID <= 0 || (returned.SalePrice <= 0 && returned.Description == "") || returned.SizeID <= 0 || returned.ItemID <= 0)
                    continue;
                listreturned.Add(returned);

            }

            if (listreturned.Count > 0)
            {
                string Result;
                objRecord.SaleDatails = listreturned;

                Result = Sales_SaleServiceInvoicesDAL.InsertUsingHandInvoiceXML(objRecord, IsNewRecord);

                if (Comon.cInt(Result) > 0)
                { 
                
                }


            }
            

        }
      
        int DeleteStockMoving(int DocumentID)
        {
            int Result = 0;
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.DocumentTypeID = DocumentType;
            objRecord.TranseID = DocumentID;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            Result = Stc_ItemsMoviingDAL.Delete(objRecord);
            return Result;

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

                Sales_SalesServiceInvoiceMaster model = new Sales_SalesServiceInvoiceMaster();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                string Result = Sales_SaleServiceInvoicesDAL.DeleteSales_SalesServiceInvoiceMaster(model);
                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                    int MoveID = DeleteStockMoving(Comon.cInt(Result));
                    if (MoveID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                }
                if (Comon.cInt(Result) > 0)
                {
                    //حذف القيد الالي

                    int VoucherID = DeleteVariousVoucherMachin(Comon.cInt(Result));
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية");

                }
                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) >= 0)
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

     
        public void PrintDot()
        {

            try
            {
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb1");
                rptFormName = "rptSalesInvoiceArb1";
                //if (UserInfo.Language == iLanguage.English)
                //    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();

                if (Comon.cInt(cmbMethodID.EditValue) == 1)
                {

                    rptForm.Parameters["CustomerName"].Value = txtCustomerName.Text.ToString();
                }
                else if (Comon.cInt(cmbMethodID.EditValue) == 2)
                {
                    rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.ToString();


                }


                rptForm.Parameters["MethodName"].Value = "فاتورة مبيعات " + cmbMethodID.Text.Trim().ToString();
                // rptForm.Parameters["VATCOMPANY"].Value = MySession.VAtCompnyGlobal;
                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                //rptForm.Parameters["footer"].Value = MySession.footer ;
                rptForm.Parameters["Notes"].Value = txtNotes.Text.Trim().ToString();
                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotalBeforeDiscount.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();

                rptForm.Parameters["NumbToWord"].Value = Lip.ToWords(Convert.ToDecimal(lblNetBalance.Text.Trim().ToString()), 2);

                InvoiceViewModel x = new InvoiceViewModel();
                // معلومات الضريبة الخمسة الأولى
                x.ArbCompanyName = MySession.GlobalBranchName.ToUpper();
                x.CompanyVatCode = MySession.VAtCompnyGlobal;
                x.InvoiceDate = Comon.cDateTime(txtInvoiceDate.Text + ":" + txtRegTime.Text);
                x.NetTotal = Comon.cDec(lblNetBalance.Text);
                x.VatAmount = Comon.cDec(lblAdditionaAmmount.Text);
                string Base64 = ZATKAQREncryption.ZATCATLVBase64.GetBase64(x.ArbCompanyName, x.CompanyVatCode, x.InvoiceDate, Convert.ToDouble(x.NetTotal), Convert.ToDouble(x.VatAmount));
                rptForm.Parameters["DelegateName"].Value = Base64;
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["ItemName"] = gridView1.GetRowCellValue(i, "ArbItemName").ToString();
                    row["ExpiryDate"] = gridView1.GetRowCellValue(i, "EngItemName").ToString();
                    //if (Comon.cInt(cmbLanguagePrint.EditValue) == 2)
                    //    row["ItemName"] = gridView1.GetRowCellValue(i, "EngItemName").ToString();
                    //else if (Comon.cInt(cmbLanguagePrint.EditValue) == 3)
                    //    row["ItemName"] = gridView1.GetRowCellValue(i, "EngItemName").ToString() + "                          " + gridView1.GetRowCellValue(i, "ArbItemName").ToString();


                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();

                    row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                    row["Description"] = "5";
                    row["Bones"] = gridView1.GetRowCellValue(i, "PackingQty").ToString();
                    //row["ExpiryDate"] = Comon.ConvertSerialToDate(Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString()).ToString());
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;

                ///******************** Report Binding ************************/
                //XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                //subreport.Visible = ;
                //subreport.ReportSource = ReportComponent.CompanyHeader();
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
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='rptSalesInvoice'");
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

        protected  void DoPrintA4()
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
                                bool IncludeHeader = true;
                ReportName = "rptSalesInvoiceArb";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["MethodName"].Value = cmbMethodID.Text.Trim().ToString();
                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();


                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotal.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = lblUnitDiscount.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();

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
                    row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                    row["Bones"] = gridView1.GetRowCellValue(i, "Bones").ToString();
                    row["ExpiryDate"] = Comon.ConvertSerialToDate(Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString()).ToString());
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
                //rptForm = "rptCashierPrint";
                bool IncludeHeader = true;
                decimal TotalQty = 0;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                rptFormName = "rptCashierPrint";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text;
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["MethodName"].Value = MethodName;
                rptForm.Parameters["TheTime"].Value = txtRegTime.Text;
                rptForm.Parameters["CashierName"].Value = UserInfo.SYSUSERARBNAME.ToString();
                rptForm.Parameters["CompanyName"].Value = (UserInfo.Language == iLanguage.Arabic ? cmpheader.CompanyArbName : cmpheader.CompanyEngName);
                rptForm.Parameters["CompanyAddress"].Value = (UserInfo.Language == iLanguage.Arabic ? cmpheader.ArbAddress : cmpheader.ArbAddress);
                if (dVat.Rows.Count > 0)
                    rptForm.Parameters["CompanyVatID"].Value = Comon.cLong(dVat.Rows[0][0]);
                else
                    rptForm.Parameters["CompanyVatID"].Value = 0;
                switch (MethodID)
                {
                    case (1):
                        rptForm.Parameters["NetTotal"].Value = 0; break;
                    case (2):
                        rptForm.Parameters["NetTotal"].Value = lblNetBalance.Text.Trim().ToString(); break;

                    case (3):
                        rptForm.Parameters["NetTotal"].Value = txtNetAmount.Text; break;

                }
                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text;
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                rptForm.Parameters["Paid"].Value = txtPaidAmount.Text.Trim().ToString();
                rptForm.Parameters["CoreTotal"].Value = lblRemaindAmount.Text.Trim().ToString();
                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotalBeforeDiscount.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = lblUnitDiscount.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                // معلومات الضريبة الخمسة الأولى
                InvoiceViewModel x = new InvoiceViewModel();
                x.ArbCompanyName = MySession.GlobalFacilityName.ToUpper();
                x.CompanyVatCode = MySession.VAtCompnyGlobal;
                x.InvoiceDate = Comon.cDateTimeV2(txtInvoiceDate.Text + ":" + txtRegTime.Text);
                x.NetTotal = Comon.cDec(lblNetBalance.Text);
                x.VatAmount = Comon.cDec(lblAdditionaAmmount.Text);
                string Base64 = ZATKAQREncryption.ZATCATLVBase64.GetBase64(x.ArbCompanyName, x.CompanyVatCode, x.InvoiceDate, Convert.ToDouble(x.NetTotal), Convert.ToDouble(x.VatAmount));
                rptForm.Parameters["DelegateName"].Value = Base64;
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;

                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString();// +gridView1.GetRowCellValue(i, "BarCode").ToString() + gridView1.GetRowCellValue(i, SizeName).ToString() + gridView1.GetRowCellValue(i, "PackingQty").ToString();
                    if (Comon.cInt(cmbLanguagePrint.EditValue) == 2)
                        row["ItemName"] = gridView1.GetRowCellValue(i, "EngItemName").ToString() + gridView1.GetRowCellValue(i, "BarCode").ToString();
                    else if (Comon.cInt(cmbLanguagePrint.EditValue) == 3)
                        row["ItemName"] = gridView1.GetRowCellValue(i, "ArbItemName").ToString() + " " + gridView1.GetRowCellValue(i, "ArbSizeName").ToString();
                    // row["ItemName"] = gridView1.GetRowCellValue(i, ItemName).ToString() + gridView1.GetRowCellValue(i, "BarCode").ToString() + gridView1.GetRowCellValue(i, SizeName).ToString() + gridView1.GetRowCellValue(i, "PackingQty").ToString();
                    row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    //  row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                    //   row["ExpiryDate"] = Comon.ConvertSerialToDate(Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString()).ToString());
                    TotalQty += Comon.cDec(row["QTY"]);
                    dataTable.Rows.Add(row);
                }
                 
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;
                /******************** Report Binding ************************/
                //XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                //subreport.Visible = false;
                //subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

                rptForm.RollPaper = true;
                rptForm.ReportUnit = ReportUnit.HundredthsOfAnInch;
                rptForm.PageWidth = 320;

                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = false;
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
                                rptForm.Dispose();
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
        
        protected   void DoPrintOpenDraw()
        {

            try
            {
            

                /******************** Report Body *************************/
                //rptForm = "rptCashierPrint";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                rptFormName = "‏‏rptCashierDragPrint";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                if (IdPrint == true)
                    rptForm.Parameters["InvoiceID"].Value = Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" + invoiceNo;
                else
                    rptForm.Parameters["InvoiceID"].Value = Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" + txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["MethodName"].Value = MethodName;
                rptForm.Parameters["TheTime"].Value = Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":","").Trim());
                rptForm.Parameters["CashierName"].Value = UserInfo.SYSUSERARBNAME.ToString();
                rptForm.Parameters["CompanyName"].Value = (UserInfo.Language == iLanguage.Arabic ? cmpheader.CompanyArbName  : cmpheader.CompanyEngName);
                rptForm.Parameters["CompanyAddress"].Value = (UserInfo.Language == iLanguage.Arabic ? cmpheader.ArbAddress : cmpheader.ArbAddress);
              if (dVat.Rows.Count>0)
                  rptForm.Parameters["CompanyVatID"].Value = Comon.cLong (dVat.Rows [0][0]);
              else
                  rptForm.Parameters["CompanyVatID"].Value = 0;
              
 
                switch (MethodID ){
                    case(1):
 rptForm.Parameters["NetTotal"].Value = 0;break;
                    case (2):
                        rptForm.Parameters["NetTotal"].Value = lblNetBalance.Text.Trim().ToString(); break;

                    case (3):
                        rptForm.Parameters["NetTotal"].Value = txtNetAmount.Text; break;


                
                }




                rptForm.Parameters["InvoiceDate"].Value = Lip.GetServerDate();
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                rptForm.Parameters["Paid"].Value = txtPaidAmount.Text.Trim().ToString();
                rptForm.Parameters["CoreTotal"].Value = lblRemaindAmount.Text.Trim().ToString();
                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotalBeforeDiscount.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = lblUnitDiscount.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();


               {
                    var row = dataTable.NewRow();

                    row["#"] =   1;
                    row["BarCode"] = "";
                    row["ItemName"] ="";// +gridView1.GetRowCellValue(i, "BarCode").ToString() + gridView1.GetRowCellValue(i, SizeName).ToString() + gridView1.GetRowCellValue(i, "PackingQty").ToString();
              
                  
                  
                   
                    row["SizeName"] = "";
                    row["QTY"] = "";
                    row["Total"] = "";
                    row["Discount"] = "";
                 
                    row["Net"] = "";
                   row["SalePrice"] = "";
                
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;

                /******************** Report Binding ************************/
            //    XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
             //   subreport.Visible = false;
              //  subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

               
                 ShowReportInReportViewer = false;

                if (ShowReportInReportViewer)
                {
                    frmReportViewer frmRptViewer = new frmReportViewer();
                    frmRptViewer.documentViewer1.DocumentSource = rptForm;
                    frmRptViewer.ShowDialog();
                }
                else
                {
                    bool IsSelectedPrinter = false;
                
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
                   
                    if (!IsSelectedPrinter)
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                }
            }
            catch (Exception ex)
            {
                
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

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
        public void txtInvoiceID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
            {
                 
                ReadRecord(Comon.cLong(txtInvoiceID.Text));
                Validations.DoRoolBackRipon(this, ribbonControl1);
                Validations.EnabledControl(this, false);
            }
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
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtEnteredByUserID.Text) + " And Cancel =0 And BranchID =" + UserInfo.BRANCHID;
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
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
               
                string strSql;
                DataTable dt;
                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,SpecialDiscount , *  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtCustomerID.Text = dt.Rows[0]["AcountID"].ToString();
                        txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtMobileNo.Text = dt.Rows[0]["Mobile"].ToString();
                        txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();

                        if (Comon.cLong(dt.Rows[0]["SpecialDiscount"]) > 0)
                            DiscountCustomer = Comon.cInt(dt.Rows[0]["SpecialDiscount"].ToString());
                        if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                        {
                            lblDebitAccountID.Text = txtCustomerID.Text;
                            lblDebitAccountName.Text = lblCustomerName.Text;

                            if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                            {
                                chkForVat.Checked = true;
                                txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                            }
                            else
                            {

                                txtVatID.Text = "";
                                if (chkForVat.Checked == false)
                                    chkForVat.Checked = false;
                            }
                        }
                    }
                    else
                    {
                       
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtCustomerID.Text + " And BranchID =" + UserInfo.BRANCHID;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {
                            lblDebitAccountName.Text = dt.Rows[0]["CustomerName"].ToString();
                            lblDebitAccountID.Text = txtCustomerID.Text;
                            lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                            if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                            {
                                chkForVat.Checked = true;
                                txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                            }
                            else
                            {

                                txtVatID.Text = "";
                                if (chkForVat.Checked == false)
                                    chkForVat.Checked = false;
                            }


                        }
                        else
                        {
                            lblCustomerName.Text = "";
                            txtCustomerID.Text = "";
                            txtVatID.Text = "";
                            if (chkForVat.Checked == false)
                                chkForVat.Checked = false;
                        }
                    }
                }
                else
                {
                    lblCustomerName.Text = "";
                    txtCustomerID.Text = "";
                    txtVatID.Text = "";
                }
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
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
                strSQL = "SELECT " + PrimaryName + " as SellerName FROM Sales_Sellers WHERE SellerID =" + txtSellerID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
           

        }
        private void lblChequeAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblChequeAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblDebitAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblCreditAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblAdditionalAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblDiscountDebitAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblNetAccountID.Text + ") ";
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
                PrepareSearchQuery.SearchForAccounts(lblDebitAccountID, lblDebitAccountName);
                if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                {
                    lblCustomerName.Text = lblDebitAccountName.Text;
                    txtCustomerID.Text = lblDebitAccountID.Text;
                }
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
            if (e.KeyCode == Keys.Delete)
            { 
            
            }
            if (e.KeyCode == Keys.F3)
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();
        }

        /*******************Event CheckBoc***************************/
         



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
            int value = Comon.cInt(cmbMethodID.EditValue.ToString());
            if (value == 0)
                return;
            try
            {
                lblNetProcessID.Visible = false;
                txtNetProcessID.Visible = false;
                lblCheckSpendDate.Visible = false;
                txtCheckSpendDate.Visible = false;
                txtWarningDate.Visible = false;
                lblWarningDate.Visible = false;
                lblCheckID.Visible = false;
                txtCheckID.Visible = false;
                txtNetProcessID.Text = "";
                txtCheckID.Text = "";
                txtNetAmount.Text = "";
                txtCustomerID.Text = "";
                lblCustomerName.Text = "";
                cmbNetType.ItemIndex = -1;
                txtWarningDate.EditValue = DateTime.Now;
                txtCheckSpendDate.EditValue = DateTime.Now;
                txtNetAmount.Visible = false;
                lblNetAmount.Visible = false;
                lblnetType.Visible = false;
                cmbNetType.Visible = false;
                txtCustomerID.Tag = "IsNumber";
                txtCheckID.Tag = "IsNumber";
                cmbBank.Tag = " ";
                txtNetProcessID.Tag = "IsNumber";
                txtNetAmount.Tag = "IsNumber";
                txtNetProcessID.Tag = "IsNumber";
                txtNetAmount.Tag = "IsNumber";
                //lblCreditAccountID.Tag = "ImportantFieldGreaterThanZero";
                txtStoreID.Tag = "ImportantFieldGreaterThanZero";
                if (value == 1)
                {
                    lblCreditAccountID.Tag = "IsNumber";
                    txtCustomerID.Tag = "IsNumber";
                    lblDebitAccountID.Tag = "ImportantFieldGreaterThanZero";

                    {
                        lblNetAccountCaption.Enabled = false;
                        lblNetAccountID.Enabled = false;
                        lblNetAccountName.Enabled = false;
                        lblCachCaption.Enabled = true;
                        lblDebitAccountID.Enabled = true;
                        lblDebitAccountName.Enabled = true;
                    }
                    if (string.IsNullOrEmpty(MySession.GlobalDefaultSaleDebitAccountID) == false)
                    {
                        lblDebitAccountID.Text = MySession.GlobalDefaultSaleDebitAccountID;
                        lblDebitAccountID_Validating(null, null);
                    }

                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    // txtCustomerName.Focus();
                }
                else if (value == 2)
                {

                    lblCreditAccountID.Tag = "IsNumber";
                    lblDebitAccountID.Tag = "IsNumber";
                    txtCustomerID.Tag = "ImportantFieldGreaterThanZero";
                    {
                        lblNetAccountCaption.Enabled = false;
                        lblNetAccountID.Enabled = false;
                        lblNetAccountName.Enabled = false;
                        lblCachCaption.Enabled = false;
                        lblDebitAccountID.Enabled = false;
                        lblDebitAccountName.Enabled = false;
                    }
                    lblCustomerName.BringToFront();
                    txtCustomerID.BringToFront();
                    txtCustomerID.Focus();
                    //  Find();
                    lblDebitAccountID.Text = txtCustomerID.Text;
                    lblDebitAccountName.Text = lblCustomerName.Text;
                    lblCheckSpendDate.Visible = true;
                    txtCheckSpendDate.Visible = true;
                    txtWarningDate.Visible = true;
                    lblWarningDate.Visible = true;
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    if (StopSomeCode == false)
                    {
                        if (Comon.cLong(MySession.GlobalDefaultSaleCustomerID) > 0)
                        {
                            txtCustomerID.Text = MySession.GlobalDefaultSaleCustomerID;
                            txtCustomerID_Validating(null, null);

                        }
                        else
                        {
                            txtCustomerID.Focus();
                            Find();
                        }
                    }

                }
                else if (value == 3)
                {
                    if (string.IsNullOrEmpty(MySession.GlobalDefaultSaleNetTypeID) == false)
                    {
                        lblDebitAccountID.Text = MySession.GlobalDefaultSaleNetTypeID;
                        lblDebitAccountID_Validating(null, null);
                    }

                    lblCheckSpendDate.Visible = false;
                    txtCheckSpendDate.Visible = false;
                    lblCheckID.Visible = false;
                    txtCheckID.Visible = false;
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;

                    lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = false;
                    lblNetAmount.Visible = false;
                    lblnetType.Visible = true;
                    cmbNetType.Visible = true;
                    cmbNetType.ReadOnly = false;
                    cmbNetType.ItemIndex = 0;// Comon.cDbl(lblDebitAccountID.Text);
                    txtNetProcessID.Tag = "ImportantFieldGreaterThanZero";
                    //txtNetAmount.Tag = "ImportantFieldGreaterThanZero";
                    //cmbNetType.Tag = "ImportantField";
                    lblCreditAccountID.Tag = "IsNumber";
                    lblDebitAccountID.Tag = "IsNumber";
                    txtCustomerID.Tag = "IsNumber";
                    lblNetAccountID.Tag = "ImportantFieldGreaterThanZero";
                    {
                        lblNetAccountCaption.Enabled = true;
                        lblNetAccountID.Enabled = true;
                        lblNetAccountName.Enabled = true;
                        lblCachCaption.Enabled = false;
                        lblDebitAccountID.Enabled = false;
                        lblDebitAccountName.Enabled = false;
                    }
                }
                else if (value == 4)
                {


                    lblNetProcessID.Visible = false;
                    txtNetProcessID.Visible = false;
                    txtNetAmount.Visible = false;
                    lblNetAmount.Visible = false;
                    lblnetType.Visible = false;
                    cmbNetType.Visible = false;

                    lblCheckSpendDate.Visible = true;
                    txtCheckSpendDate.Visible = true;
                    lblCheckID.Visible = true;
                    txtCheckID.Visible = true;
                    lblBankName.Visible = true;
                    cmbBank.Visible = true;
                    cmbBank.Tag = "ImportantField";

                    cmbBank.EditValue = Comon.cDbl(lblDebitAccountID.Text);
                }
                else if (value == 5)
                {

                    lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = true;
                    lblNetAmount.Visible = true;
                    lblnetType.Visible = true;
                    cmbNetType.Visible = true;
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    cmbNetType.Visible = true;
                    cmbNetType.ReadOnly = false;
                    cmbNetType.ItemIndex = 0;

                    cmbNetType.EditValue = Comon.cDbl(MySession.GlobalDefaultSaleNetTypeID);
                    txtNetProcessID.Tag = " ";
                    txtCustomerID.Tag = "IsNumber";
                    txtNetAmount.Tag = "ImportantFieldGreaterThanZero";
                    lblNetAccountID.Tag = "ImportantFieldGreaterThanZero";
                    lblDebitAccountID.Tag = "ImportantFieldGreaterThanZero";
                    cmbNetType.Tag = "ImportantField";
                    {
                        lblNetAccountCaption.Enabled = true;
                        lblNetAccountID.Enabled = true;
                        lblNetAccountName.Enabled = true;
                        lblCachCaption.Enabled = true;
                        lblDebitAccountID.Enabled = true;
                        lblDebitAccountName.Enabled = true;
                    }
                }


            }
            catch (Exception ex)
            {
                //Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void cmbMethodID_Enter(object sender, EventArgs e)
        {
            PublicTextEdit_EditValueChanged(txtCustomerID, e);
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

        private void frmSaleInvoice_Load(object sender, EventArgs e)
        {
            ribbonControl1.Visible = false;
        }
        #endregion

        private void frmSalesInvoice_Load(object sender, EventArgs e)
        {
            cmpheader = CompanyHeaderDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
            if ( (cmpheader.pic)!=null)
            {
                TheImage = new MemoryStream(cmpheader.pic);
                if (TheImage.Length > 0)
                    picCompanyHeader.Image = Image.FromStream(TheImage, true);
            }
            DoNew();
            simpleButton1_Click(null, null);
            dVat = Lip.SelectRecord(VAt);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


           

            if (dt.Rows.Count < 1)
                return;

            // Check if the data table has any rows
            if (dt.Rows.Count < 1)
                return;

            // Query the sales invoice return master for any previous returns with the same CustomerInvoiceID and BranchID
            strSQL = "Select * from Sales_SalesInvoiceReturnMaster where CustomerInvoiceID=" + txtInvoiceID.Text + " And BranchID=" + MySession.GlobalBranchID + " and Cancel=0";
            DataTable dtReturn = new DataTable();
            dtReturn = Lip.SelectRecord(strSQL);

            // If a return for this invoice already exists, show an error message and open the SalesInvoiceReturn form in view mode with this return loaded
            if (dtReturn.Rows.Count > 0)
            {
                Messages.MsgError(Messages.TitleError, " يوجد فاتورة مردودات سابقة لهذه الفاتورة");
                frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormAdd = true;
                frm.FormUpdate = true;
                frm.FormView = true;
                frm.FormDelete = true;
                frm.Show();

                frm.cmbBranchesID.EditValue = UserInfo.BRANCHID;
                frm.txtCustomerInvoiceID.Text = txtInvoiceID.Text;
                frm.txtCustomerInvoiceID_Validating(null, null);
              //  frm.txtCustomerInvoiceID.Text = txtInvoiceID.Text;

            }
            else
            {
                frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);

                 
                    frm.FormAdd = true;
                    frm.FormUpdate = true;
                    frm.FormView = true;
                    frm.Show();
                    frm.fillMAsterData(dt);
                    frm.lblNetBalance.Text = lblNetBalance.Text;
                   frm.lblInvoiceTotalBeforeDiscount.Text = lblInvoiceTotalBeforeDiscount.Text;
                    frm.lblAdditionaAmmount.Text = lblAdditionaAmmount.Text;
                    frm.txtCustomerInvoiceID.Text = txtInvoiceID.Text;
                    
                   // frm.txtCustomerInvoiceID_Validating(null, null);

                }
                else
                    frm.Dispose();
            }
        }

        

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblCheckID_Click(object sender, EventArgs e)
        {

        }

        private void lblDebitAccountID_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl25_Click(object sender, EventArgs e)
        {

        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            strQty = strQty + "3";

        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            
            DoSave();
        
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            showCustomers(false,0);
            txtCustomerName.Visible = true;
            labelControl6.Visible = true;
            txtVatID.Visible = true;
            
            cmbMethodID.EditValue = 1;
            simpleButton1.Appearance.BackColor = Color.Goldenrod;
            simpleButton1.Appearance.BackColor2 = Color.White;
            simpleButton1.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical ;
            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "نقدا" : "Cash");
            MethodID = 1;
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];

            txtreqiramount.Visible = false;
            lblReqiramount.Visible = false;
            txtreqiramount.Text = "";

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
         
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            showCustomers(false,0);
            cmbMethodID.EditValue = 3;
            simpleButton2.Appearance.BackColor = Color.Goldenrod;
            simpleButton2.Appearance.BackColor2 = Color.White;
            simpleButton2.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة" : "Net");
            MethodID =2;
            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];

            txtreqiramount.Visible = false;
            lblReqiramount.Visible = false;
            txtreqiramount.Text = "";
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            txtCustomerID.Tag = " ";
            //txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            //cmbNetType.Tag = " ";
            //txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            showCustomers(false,0);
            cmbMethodID.EditValue = 5;
            simpleButton3.Appearance.BackColor = Color.Goldenrod;
            simpleButton3.Appearance.BackColor2 = Color.White;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            simpleButton3.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة/ نقد" : "Net/Cash");
            MethodID = 3;

            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];

            txtreqiramount.Visible = true;
            lblReqiramount.Visible = true;
            txtreqiramount.Text = "";
            txtreqiramount.Text = (Comon.cDec(lblNetBalance.Text) - Comon.cDec(lblCutAmount.Text) - Comon.cDec(txtNetAmount.Text)).ToString();

            txtNetAmount.Focus();

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

              //  gridView1.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        private void frmCashierSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                 
                DoSave();

            }

            if (e.KeyCode == Keys.F7)
            { 
                simpleButton3_Click(null, null);
                txtNetAmount.Focus();
                if (txtNetAmount.Text != string.Empty)
                    DoSave();

            }

            if (e.KeyCode == Keys.F8)
            {
                simpleButton2_Click(null, null);
                DoSave();

            }

            if (e.KeyCode == Keys.Delete)
            {
                btnDeletCurentRow_Click(null, null);
            }

            if (e.KeyCode == Keys.F11)
            {
                DoPrint();
            }


            if (e.KeyCode == Keys.F12)
            {
                DoPrintOpenDraw();
            }

            if (e.KeyCode == Keys.F1)
            {

                if (gridView1.RowCount > 1 && IsNewRecord==false)
                {
                    frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        BindingSource bs = new BindingSource();
                        bs.DataSource = gridControl.DataSource;

                        frm.Show();
                        frm.fillMAsterData(dt);
                        frm.GetAccountsDeclaration();
                        frm.gridControl.DataSource = bs;
                        frm.CalculateRow();
                    }
                    else
                        frm.Dispose();
                }
                else
                {
                    frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        BindingSource bs = new BindingSource();
                        bs.DataSource = gridControl.DataSource;
                        frm.Show();
                    }
                }
            }
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

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
          //  showCustomers(true, 1);
           
         
            cmbMethodID.EditValue = 2;
           
            simpleButton12.Appearance.BackColor = Color.Goldenrod;
            simpleButton12.Appearance.BackColor2 = Color.White;
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            simpleButton12.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "آجل" : "Future");
            MethodID = 4;


            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];

            txtreqiramount.Visible = false;
            lblReqiramount.Visible = false;
            txtreqiramount.Text = "";
        }

        private void showCustomers(bool p,int f )
        { 
            txtCustomerName.Text = "";
            txtCustomerID.Text = "";
            lblCustomerName.Text = "";
            txtVatID.Text = "";
            txtCustomerName.Visible = false;
            labelControl6.Visible = p;
            txtCustomerID.BringToFront();
            lblCustomerName.BringToFront();
            txtVatID.Visible = p;
            //labelControl6.Visible = p;
            if (f ==1) {
                
            }
        }

        void addcMobile()
        {
            //frmAddMobileFromCahier frm = new frmAddMobileFromCahier();
            //frm.lblCustomerName.Text = lblCustomerName.Text;
            //frm.txtCustomerID.Text = txtCustomerID.Text;
            //frm.ShowDialog();
            //txtMobileNo.Text = frm.txtMobile.Text;
        }
        void addcustoe()
        {
            ctCustomers = new ctAddCustomers();
            ctCustomers.simpleButton1.Click += simpleButton1111_Click;
            ctCustomers.IsNewRecord = true;
            FlyoutAction action = new FlyoutAction();
            FlyoutProperties properties = new FlyoutProperties();
            properties.Style = FlyoutStyle.Popup;
            ctCustomers.txtMobile.Text = txtMobileNo.Text.Trim();
            ctCustomers.txtArbName.Text = "عميل نقدي";
            FlyoutDialog.Show(this, ctCustomers, action, properties);
            if (lblCustomerName.Text.Trim() != string.Empty && txtMobileNo.Text.Trim() != string.Empty)
            {
                lblCustomerName.Text = ctCustomers.txtArbName.Text;
                txtMobileNo.Text = ctCustomers.txtMobile.Text.Trim();
                if (txtMobileNo.Text != string.Empty && txtMobileNo.Text.Trim().ToString().Length >= 9 && lblCustomerName.Text != string.Empty)
                {
                    strSQL = "SELECT * FROM Sales_Customers Where     Mobile='" + txtMobileNo.Text.Trim() + "' Or Fax ='" + txtMobileNo.Text.Trim() + "'";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        txtCustomerID.Text = dt.Rows[0]["AccountID"].ToString();
                        lblCustomerName.Text = dt.Rows[0]["ArbName"].ToString();
                        txtMobileNo.Text = dt.Rows[0]["Mobile"].ToString();
                        txtCustomerName.Text = dt.Rows[0]["ArbName"].ToString();
                        lblCbalance.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["Balance"].ToString()).ToString();
                        userControl11.BarCode = dt.Rows[0]["Mobile"].ToString();
                        //Lip.NewFields();
                        //Lip.AddStringField("UserName", lblCustomerName.Text);
                        //Lip.AddStringField("UserEmail", lblCustomerName.Text);
                        //Lip.AddStringField("Userphone", txtMobileNo.Text);
                        //Lip.AddStringField("UserPassword", "123");
                        //Lip.AddStringField("UserPhotoPath", "");
                        //Lip.AddNumericField("UserBalance", lblCbalance.Text);
                        //Lip.AddStringField("PasswordResetToken", "123");
                        //Lip.AddStringField("ResetTokenExpires", "");
                        //Lip.ExecuteInsertApi();
                    }
                    //txtCustomerID_Validating(null, null);
                    SendKeys.Send("{ESC}");
                }
            }
        }
        private void simpleButton1111_Click(object sender, EventArgs e)
        {

            txtCustomerID.Text = ctCustomers.txtCustomerID.Text;
            lblCustomerName.Text = ctCustomers.txtArbName.Text;
            txtCustomerID_Validating(null, null);
            SendKeys.Send("{ESC}");
        }
        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
           

        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked == true)
            {
                groupBox1.Visible = true;
                gridControl.Width = gridControl.Width - groupBox1.Width;
                gridControl.Location = new Point(241, gridControl.Location.Y);
            }
            else
            {
                groupBox1.Visible = false;
                gridControl.Width = gridControl.Width + groupBox1.Width;
                gridControl.Location = new Point(1, gridControl.Location.Y);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
                checkBox1.Checked = false;
            else
                checkBox1.Checked = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                checkBox2.Checked = false;
            else
                checkBox2.Checked = true;
        }

        private void labelControl27_Click(object sender, EventArgs e)
        {

        }

        

        private void btnDeletCurentRow_Click(object sender, EventArgs e)
        {
            gridView1.DeleteRow(FocusedRowHandle);
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            CalculateRow();

        }

        private void txtNetAmount_TextChanged(object sender, EventArgs e)
        {

            txtreqiramount.Text = (Comon.cDec(lblNetBalance.Text)   -Comon.cDec(lblCutAmount.Text) -  Comon.cDec(txtNetAmount.Text)).ToString();
             
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMobileNo_Validating(object sender, CancelEventArgs e)
        {
            if (txtMobileNo.Text.Trim() != string.Empty)
            {
                strSQL = "SELECT * FROM Sales_Customers Where     Mobile='" + txtMobileNo.Text.Trim() + "' Or Fax ='" + txtMobileNo.Text.Trim() + "'";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    txtCustomerID.Text = dt.Rows[0]["AccountID"].ToString();
                    lblCustomerName.Text = dt.Rows[0]["ArbName"].ToString();
                    txtMobileNo.Text = dt.Rows[0]["Mobile"].ToString();
                    txtCustomerName.Text = dt.Rows[0]["ArbName"].ToString();
                    lblCbalance.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["Balance"].ToString()).ToString();
                    userControl11.BarCode = dt.Rows[0]["Mobile"].ToString();
                    SendKeys.Send("{ESC}");

                }
                else
                {
                    //addcustoe();
                }
                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                CalculateRow();

                decimal Bal = Comon.ConvertToDecimalPrice(lblCbalance.Text);
                btnUseBalance.Visible = false;
                lblReqiramount.Visible = false;
                txtreqiramount.Visible = false;
                if (Bal > 0)
                {
                    btnUseBalance.Visible = true;
                    lblReqiramount.Visible = true;
                    txtreqiramount.Visible = true;

                }
            }
        }

        private void btnUseBalance_Click(object sender, EventArgs e)
        {
            decimal Cbalance = Comon.ConvertToDecimalPrice(lblCbalance.Text);
            decimal NetBal = Comon.ConvertToDecimalPrice(lblNetBalance.Text);

            if (NetBal <= Cbalance)
            {
                lblCutAmount.Text = Comon.ConvertToDecimalPrice(NetBal).ToString();
            }
            else
            {
                lblCutAmount.Text = Comon.ConvertToDecimalPrice(Cbalance).ToString();
            }

            decimal Bal = Comon.ConvertToDecimalPrice(lblCutAmount.Text);
             
            txtreqiramount.Text = (Comon.cDec(lblNetBalance.Text) - Comon.cDec(lblCutAmount.Text) - Comon.cDec(txtNetAmount.Text)).ToString();

            lblCutAmount.Focus();



        }

        private void userControl11_DoubleClick(object sender, EventArgs e)
        {
            btnUseBalance_Click(null, null);
        }

        private void lblCutAmount_Validating(object sender, CancelEventArgs e)
        {


            decimal Bal = Comon.ConvertToDecimalPrice(lblCutAmount.Text);
            if (  Comon.ConvertToDecimalPrice(lblCutAmount.Text) > Comon.ConvertToDecimalPrice(lblNetBalance.Text))
            {
                lblCutAmount.Text = lblNetBalance.Text;
                  
                txtreqiramount.Text = (Comon.cDec(lblNetBalance.Text) - Comon.cDec(lblCutAmount.Text) - Comon.cDec(txtNetAmount.Text)).ToString();

                return;
            }

            if (Comon.ConvertToDecimalPrice(lblCutAmount.Text) >= Comon.ConvertToDecimalPrice(lblCbalance.Text))
            {
                lblCutAmount.Text = lblCbalance.Text;
            }
            txtreqiramount.Text = (Comon.cDec(lblNetBalance.Text) - Comon.cDec(lblCutAmount.Text) - Comon.cDec(txtNetAmount.Text)).ToString();
        }

        private void txtSaveAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + txtSaveAccountID.Text + ") ";
                CSearch.ControlValidating(txtSaveAccountID, lblSaveAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnListHandInvoice_Click(object sender,EventArgs e)
        {
            frmHandInvoices frm = new frmHandInvoices();
            frm.ShowDialog();
            if ( frm.StoreID > 0)
            {
                txtInvoiceID.Text = frm.StoreID.ToString();
                ReadRecordHand(Comon.cLong(txtInvoiceID.Text));
                IsNewRecord = true;
                Lip.ExecututeSQL("Delete from Sales_SalesInvoiceDetailsHand Where InvoiceID=" + txtInvoiceID.Text);
                Lip.ExecututeSQL("Delete from Sales_SalesInvoiceMasterHand Where InvoiceID=" + txtInvoiceID.Text);
            }
        }
        private void btnCloseCashier_Click(object sender, EventArgs e)
        {

            frmCloseCashier frm = new frmCloseCashier();
            frm.Show();
        }

        private void btnHandInvoice_Click(object sender, EventArgs e)
        {
            SaveInvoiceHand();
            DoNew();
        }
        private void btnShowCost_Click(object sender, EventArgs e)
        {
            frmCashierSalesPublic frm = new frmCashierSalesPublic();
            int InvoiceID = Comon.cInt(txtInvoiceID.Text);
            frm.FormView = true;
            frm.Show();
            frm.FormView = true;

            frm.ReadRecord(InvoiceID);
        }

        private void btnMachinResraction_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;

            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(UserInfo.BRANCHID) + " And DocumentID=" + txtInvoiceID.Text + " And DocumentType=" + DocumentType).ToString());
            if (ID > 0)
            {
                frmVariousVoucherMachin frm22 = new frmVariousVoucherMachin();
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm22);
                frm22.FormView = true;
                frm22.FormAdd = false;
                frm22.Show();
                frm22.cmbBranchesID.EditValue = Comon.cInt(UserInfo.BRANCHID);
                frm22.ReadRecord(Comon.cLong(ID.ToString()));
            }
            else
                Messages.MsgError("تنبيه", "   لا يوجد قيد - الرجاء اعادة حفظ المستند ");
        }

        public void Transaction()
        {
            strSQL = "Select * from " + Sales_SaleServiceInvoicesDAL.TableName + " where Cancel=0 ";
            DataTable dtSend = new DataTable();
            dtSend = Lip.SelectRecord(strSQL);
            if (dtSend.Rows.Count > 0)
            {
                for (int i = 0; i <= dtSend.Rows.Count - 1; i++)
                {
                    txtInvoiceID.Text = dtSend.Rows[i]["InvoiceID"].ToString();
                    // comBranchesID.EditValue = Comon.cInt(dtSend.Rows[i]["BranchID"].ToString());

                    txtCostCenterID.EditValue = dtSend.Rows[i]["CostCenterID"].ToString();
                    txtStoreID.EditValue = dtSend.Rows[i]["StoreID"].ToString();
                    txtInvoiceID_Validating(null, null);
                    IsNewRecord = true;
                    if (Comon.cInt(txtInvoiceID.Text) > 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(Comon.cInt(txtInvoiceID.Text));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + Sales_SaleServiceInvoicesDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sales_SaleServiceInvoicesDAL.PremaryKey + " = " + txtInvoiceID.Text + " AND BranchID=" + Comon.cInt(dtSend.Rows[i]["BranchID"].ToString()));

                    }



                }

                this.Close();
            }
        }

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                txtCurrncyPrice.Text = CurrncyPrice + "";
                lblCurrencyEqv.Visible = true;
                lblCurrncyPric.Visible = true;
                lblcurrncyEquvilant.Visible = true;
                txtCurrncyPrice.Visible = true;
                gridView1.Columns["CurrencyEquivalent"].Visible = true;
            }
            else
            {
                txtCurrncyPrice.Text = "1";
                lblCurrencyEqv.Visible = false;
                lblCurrncyPric.Visible = false;
                lblcurrncyEquvilant.Visible = false;
                txtCurrncyPrice.Visible = false;
                gridView1.Columns["CurrencyEquivalent"].Visible = false;
            }
        }
    }
}
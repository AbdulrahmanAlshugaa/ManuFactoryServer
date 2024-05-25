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
using System.IO;
using DevExpress.XtraGrid.Views.Grid;
using Edex.Model;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Localization;
using Edex.DAL.SalseSystem;
using System.Globalization;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Columns;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.DAL.Accounting;
using DevExpress.XtraGrid;
using Edex.DAL.Stc_itemDAL;
using Edex.DAL;
using Edex.AccountsObjects.Codes;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.StockObjects.Codes;
using DevExpress.XtraEditors.Repository;
using Edex.SalesAndPurchaseObjects.Transactions;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Layout.Events;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Customization;
using Edex.DAL.Configuration;
using DevExpress.XtraGrid.Views.Base;
using Edex.TimeStaffScreens;
using DAL;
using Edex.GeneralObjects.GeneralUserControls;
//using Edex.SalesAndPurchaseObjects.UserControl;
//using Edex.TimeStaffScreens.TimeStaffClasess;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class frmDeliveryInvoice : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        public DataTable dtFillGrid = new DataTable();
        public DataTable filtering = new DataTable();
        public DataTable dtPrint2 = new DataTable();
        public DataTable dtPriceItemOffers = new DataTable();
        public DataTable dtPriceCustomersOffers = new DataTable();
        ctAddCustomers ctCustomers = new ctAddCustomers();
        public DataTable dtSpecialOffers = new DataTable();
        XtraForm2 frm = new XtraForm2();
        public int nextPage = 0;
        public string DeliveryName = "";
        public frmAddressCustomer frmAddressCust;
        public bool ShowReportInReportViewer;
        public bool FormAdd;
        public bool stopSave = false;
        public string languagename = "";
        public bool FormDelete;
        // public uAddExtension uc;
        public bool FormUpdate;
        public bool FormView;
        public bool ReportView;
        public bool ReportExport;
        public frmSizeItem frmSize;
        //  public SizeItemPop frmSize;
        public string ReportName;
        CompanyHeader cmpheader = new CompanyHeader();
        public int DiscountCustomer = 0;
        #region Declare
        bool IdPrint = false;
        string MethodName = "";
        string invoiceNo = " ";
        int MethodID = 0;
        DataTable dtDeclaration;
        int flagError = 0;
        DataTable dtSize;
        string barcodeLast = "";
        int rowIndex;
        public string strQty = "";
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
        private string OrderType = "0";
        private string OrderTypeArb = "محلي";
        private string OrderTypeEng = "Local";
        DataTable dVat = new DataTable();
        public MemoryStream TheImage;
        private bool IsNewRecord;
        private Sales_SaleInvoicesDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;
        public CultureInfo culture = new CultureInfo("en-US");
        OpenFileDialog OpenFileDialog1 = null;
        DataTable dt = new DataTable();
        DataTable dtPrint = new DataTable();
        GridViewMenu menu;

        //all record master and detail
        BindingList<Sales_SalesInvoiceDetails> AllRecords = new BindingList<Sales_SalesInvoiceDetails>();

        //list detail
        BindingList<Sales_SalesInvoiceDetails> lstDetail = new BindingList<Sales_SalesInvoiceDetails>();

        //Detail
        Sales_SalesInvoiceDetails BoDetail = new Sales_SalesInvoiceDetails();
        string VAt = "Select CompanyVATID from  VATIDCOMPANY ";


        #region ListItem
        BindingSource HangingOrder = new BindingSource();
        private DataTable dtGroups;
        private DataTable dtItems;
        private GridColumn theActiveColumn;
        private GridColumn thePreviousColumn;
        private int theActiveRow;
        private int thePreviousActiveRow;

        const int SizeItemPage = 20;
        int CountItemPage;
        int IndexItemPage = 0;
        private SimpleButton[] ArrbtnItems = new SimpleButton[SizeItemPage];

        const int SizeItemGroupPage = 10;
        int CountItemGroupPage;
        int IndexItemGroupPage = 0;
        private SimpleButton[] ArrbtnItemGroups = new SimpleButton[SizeItemGroupPage];
        #endregion
        public static Control theActiveControl = null;
        public static Control thePreviousControl = null;
        #endregion
        private object getNote(int p)
        {
            var dt = dtSpecialOffers.Select("ItemID=" + p.ToString());
            if (dt.Length < 1) return "";
            return dt[0]["Notes"].ToString();
        }
        public void RefreshOffers()
        {

            string dateFrom = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            var spriceOffers = "SELECT        PriceItemsOffers.*  FROM     PriceItemsOffers"
+ "  where ((IsAmount>0)or (IsPercent>0)or(IsOffers>0)) And ((OrderType=0)or(OrderType=" + OrderType + "))"
+ "   And"
+ " (FromDate<=" + dateFrom + ")And (ToDate>=" + dateFrom + ")";

            dtPriceItemOffers = Lip.SelectRecord(spriceOffers);

            var SpecialOffers = "SELECT        SpecialOffers_Master.*  FROM     SpecialOffers_Master"
+ "  where  (FromDate<=" + dateFrom + ")And (ToDate>=" + dateFrom + ")";

            dtSpecialOffers = Lip.SelectRecord(SpecialOffers);

        }
        public void fillGrid()
        {


            var sr = "Select  0.0 AS RemainQty ,"
               + "concat((Select Top 1 Stc_ItemUnits.SalePrice from Stc_ItemUnits where  Stc_ItemUnits.ItemID=Stc_Items.ItemID and  Stc_ItemUnits.unitCancel=0 order by Stc_ItemUnits.PackingQty Asc ),' -  ',(Select Top 1 Stc_SizingUnits." + languagename + " from Stc_ItemUnits inner join Stc_SizingUnits on Stc_SizingUnits.SizeID=Stc_ItemUnits.SizeID where  Stc_ItemUnits.ItemID=Stc_Items.ItemID and  Stc_ItemUnits.unitCancel=0 order by Stc_ItemUnits.PackingQty Asc ) ) AS SalePrice ,Stc_Items.TypeID, Stc_Items.GroupID, Stc_Items.ItemID, Stc_Items." + languagename + " as ItemName,Stc_Items.ItemImage from Stc_Items where (Stc_Items.TypeID=6) and   Cancel=0   ";
            dtFillGrid = Lip.SelectRecord(sr);
            if (dtFillGrid.Rows.Count > 0)
            {

                for (int i = 0; i <= dtFillGrid.Rows.Count - 1; ++i)
                {

                    byte[] imgByte = null;
                    imgByte = (byte[])dtFillGrid.Rows[i]["ItemImage"];
                    if (DBNull.Value == dtFillGrid.Rows[i]["ItemImage"] || imgByte.Length == 0)
                    {

                        dtFillGrid.Rows[i]["ItemImage"] = DefaultImage();
                    }

                }

            }


            RefreshOffers();
            string dateFrom = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            var CustomersOffers = "SELECT        PriceCustomersOffers.*  FROM     PriceCustomersOffers"
            + "  where ((ISBySaleTotal>0)or (ISForAll>0)or(IsByCustomersID>0))"
            + "   And"
                + " (FromDate<=" + dateFrom + ")And (ToDate>=" + dateFrom + ")";

            dtPriceCustomersOffers = Lip.SelectRecord(CustomersOffers);
        }



        public frmDeliveryInvoice()
        {
            // InitializeComponent();
            //Common.filllookupEDit(ref repositoryItemLookUpEdit2, "ID", "AdmAfr_Class", "ArbName", "Cancel=0");
            //Common.filllookupEDit(ref repositoryItemLookUpEdit3, "ID", "AdmAfr_Devision", "ArbName", "Cancel=0");
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                InitializeFormatDate(txtInvoiceDate);
                if (UserInfo.Language == iLanguage.English)
                    languagename = "EngName";
                else
                    languagename = "ArbName";
                // gridControl2.DataSource = filtering;
                //Common.filllookupEDit(ref repositoryItemLookUpEdit1, "GroupID", "AdmAfr_Groups", "ArbName", "Cancel=0");
                fillGrid();
                filtering = dtFillGrid.Copy();
                gridControl2.DataSource = filtering;


                var sr1 = "Select  0.0 AS RemainQty ,"
             + "concat((Select Top 1 Stc_ItemUnits.SalePrice from Stc_ItemUnits where  Stc_ItemUnits.ItemID=Stc_Items.ItemID and  Stc_ItemUnits.unitCancel=0 order by Stc_ItemUnits.PackingQty Asc ),' -  ',(Select Top 1 Stc_SizingUnits." + languagename + " from Stc_ItemUnits inner join Stc_SizingUnits on Stc_SizingUnits.SizeID=Stc_ItemUnits.SizeID where  Stc_ItemUnits.ItemID=Stc_Items.ItemID and  Stc_ItemUnits.unitCancel=0 order by Stc_ItemUnits.PackingQty Asc ) ) AS SalePrice ,Stc_Items.TypeID, Stc_Items.GroupID, Stc_Items.ItemID, Stc_Items." + languagename + " as ItemName,Stc_Items.ItemImage from Stc_Items where (Stc_Items.TypeID=6) and   Cancel=0   ";
                DataTable dtFillGrid2 = Lip.SelectRecord(sr1).Select("GroupID=1").CopyToDataTable();
                gridControl2.DataSource = dtFillGrid2;


                // string[] s = new string[] { "الكل", "ا", "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي" };
                var sr = "Select GroupID,ArbName from Stc_ItemsGroups where Cancel=0 and Notes<>'mat'";
                var dt2 = Lip.SelectRecord(sr);
                indexGridControl.DataSource = dt2;
                //GridLocalizer.Active = new MyGridLocalizer();
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = " الوحدة";
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

                lblNetBalance.BackColor = Color.Black;
                lblNetBalance.ForeColor = Color.GreenYellow;
                strSQL = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
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
                }
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
                // FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbCurency, "Currency", "CurrencyID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
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
                InitializeFormatDate(txtWarningDate);
                InitializeFormatDate(txtCheckSpendDate);
                /************************  Form Printing ***************************************/
                cmbFormPrinting.EditValue = Comon.cInt(MySession.GlobalDefaultSaleFormPrintingID);
                /*********************** Roles From ****************************/
                txtInvoiceDate.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceDate;
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmSaleStoreID;
                txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCostCenterID;
                //    cmbMethodID.ReadOnly = !MySession.GlobalAllowChangefrmSalePayMethodID;
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
                /********************* Event For Account Component ****************************/
                this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);
                this.btnCreditSearch.Click += new System.EventHandler(this.btnCreditSearch_Click);
                this.btnAdditionalSearch.Click += new System.EventHandler(this.btnAdditionalSearch_Click);
                this.btnNetSearch.Click += new System.EventHandler(this.btnNetSearch_Click);
                // this.btnChequeSearch.Click += new System.EventHandler(this.btnChequeSearch_Click);
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

                    //this.cmbMethodID.Enter += new System.EventHandler(this.cmbMethodID_Enter);
                    this.cmbCurency.Enter += new System.EventHandler(this.PublicCombox_Enter);
                    //this.cmbNetType.Enter += new System.EventHandler(this.PublicCombox_Enter);
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtInvoiceDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.txtCheckSpendDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.txtWarningDate.Click += new System.EventHandler(this.PublicTextEdit_Click);

                    //  this.cmbMethodID.Click += new System.EventHandler(this.cmbMethodID_Click);
                    this.cmbCurency.Click += new System.EventHandler(this.PublicCombox_Click);
                    // this.cmbNetType.Click += new System.EventHandler(this.PublicCombox_Click);
                }


                this.txtInvoiceID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtStoreID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCustomerID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                // this.txtCheckID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetProcessID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetAmount.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                // this.cmbMethodID.EditValueChanged += new System.EventHandler(this.cmbMethodID_EditValueChanged);
                this.cmbNetType.EditValueChanged += new System.EventHandler(this.cmbNetType_EditValueChanged);
                this.cmbBank.EditValueChanged += new System.EventHandler(this.cmbBank_EditValueChanged);

                this.chkForVat.EditValueChanged += new System.EventHandler(this.chForVat_EditValueChanged);

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
                this.gridView2.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView2_InitNewRow);
                this.gridView2.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView2_FocusedRowChanged);
                this.gridView2.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridView2_FocusedColumnChanged);
                this.gridView2.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanging);
                this.gridView2.ShownEditor += new System.EventHandler(this.gridView2_ShownEditor);
                this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
                this.gridView2.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView2_InvalidRowException);
                this.gridView2.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView2_ValidateRow);
                this.gridView2.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView2_CustomUnboundColumnData);
                this.gridView2.PopupMenuShowing += gridView2_PopupMenuShowing;
                /******************************************/
                //  this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
                txtInvoiceID.ReadOnly = false;
                DoNew();
                ListItemInit();
                FormView = true;
                dVat = Lip.SelectRecord(VAt);
                cmpheader = CompanyHeaderDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
                SplashScreenManager.CloseForm(false);
                ribbonControl1.Visible = false;
                frmWrningItemQty frm = new frmWrningItemQty();
                frm.Show();
                frm.Hide();
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
        public frmDeliveryInvoice(long invoiceIDx, string DeliveryName)
        {
            // InitializeComponent();
            //Common.filllookupEDit(ref repositoryItemLookUpEdit2, "ID", "AdmAfr_Class", "ArbName", "Cancel=0");
            //Common.filllookupEDit(ref repositoryItemLookUpEdit3, "ID", "AdmAfr_Devision", "ArbName", "Cancel=0");
            FormAdd = true;
            FormDelete = true;
            FormUpdate = true;
            FormView = true;
            ReportView = true;
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                fillGrid();
                filtering = dtFillGrid.Copy();
                gridControl2.DataSource = filtering;
                // gridControl2.DataSource = filtering;
                //Common.filllookupEDit(ref repositoryItemLookUpEdit1, "GroupID", "AdmAfr_Groups", "ArbName", "Cancel=0");

                // string[] s = new string[] { "الكل", "ا", "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي" };
                var sr = "Select GroupID,ArbName from Stc_ItemsGroups where Cancel=0";
                var dt2 = Lip.SelectRecord(sr);
                indexGridControl.DataSource = dt2;
                //GridLocalizer.Active = new MyGridLocalizer();
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "اسم الوحدة";
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
                lblNetBalance.BackColor = Color.WhiteSmoke;
                // lblNetBalance.ForeColor = Color.Black;
                strSQL = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
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

                }
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
                // FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbCurency, "Currency", "CurrencyID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
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
                cmbFormPrinting.EditValue = Comon.cInt(MySession.GlobalDefaultSaleFormPrintingID);

                /*********************** Roles From ****************************/
                txtInvoiceDate.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceDate;
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmSaleStoreID;
                txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCostCenterID;
                //    cmbMethodID.ReadOnly = !MySession.GlobalAllowChangefrmSalePayMethodID;
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
                /********************* Event For Account Component ****************************/

                this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);
                this.btnCreditSearch.Click += new System.EventHandler(this.btnCreditSearch_Click);
                this.btnAdditionalSearch.Click += new System.EventHandler(this.btnAdditionalSearch_Click);
                this.btnNetSearch.Click += new System.EventHandler(this.btnNetSearch_Click);
                // this.btnChequeSearch.Click += new System.EventHandler(this.btnChequeSearch_Click);
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

                    //this.cmbMethodID.Enter += new System.EventHandler(this.cmbMethodID_Enter);
                    this.cmbCurency.Enter += new System.EventHandler(this.PublicCombox_Enter);
                    this.cmbNetType.Enter += new System.EventHandler(this.PublicCombox_Enter);
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtInvoiceDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.txtCheckSpendDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.txtWarningDate.Click += new System.EventHandler(this.PublicTextEdit_Click);

                    //  this.cmbMethodID.Click += new System.EventHandler(this.cmbMethodID_Click);
                    this.cmbCurency.Click += new System.EventHandler(this.PublicCombox_Click);
                    this.cmbNetType.Click += new System.EventHandler(this.PublicCombox_Click);
                }


                this.txtInvoiceID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtStoreID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCustomerID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                // this.txtCheckID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetProcessID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetAmount.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                // this.cmbMethodID.EditValueChanged += new System.EventHandler(this.cmbMethodID_EditValueChanged);
                this.cmbNetType.EditValueChanged += new System.EventHandler(this.cmbNetType_EditValueChanged);

                this.cmbBank.EditValueChanged += new System.EventHandler(this.cmbBank_EditValueChanged);


                this.chkForVat.EditValueChanged += new System.EventHandler(this.chForVat_EditValueChanged);

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
                this.gridView2.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView2_InitNewRow);
                this.gridView2.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView2_FocusedRowChanged);
                this.gridView2.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridView2_FocusedColumnChanged);
                this.gridView2.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanging);
                this.gridView2.ShownEditor += new System.EventHandler(this.gridView2_ShownEditor);
                this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
                this.gridView2.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView2_InvalidRowException);
                this.gridView2.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView2_ValidateRow);
                this.gridView2.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView2_CustomUnboundColumnData);
                this.gridView2.PopupMenuShowing += gridView2_PopupMenuShowing;
                /******************************************/

                //  this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
                DoNew();

                ListItemInit();
                SplashScreenManager.CloseForm(false);
                dVat = Lip.SelectRecord(VAt);
                cmpheader = CompanyHeaderDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);


                this.DeliveryName = "السائق:" + DeliveryName;
                MoveRec(invoiceIDx + 1, 8);
                DoPrint();
                DoPrint();
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
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        private byte[] DefaultImage()
        {
            try
            {
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                Path = Path + @"\Images\379338-48.png";
                System.Drawing.Image img = System.Drawing.Image.FromFile(Path);
                MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
            catch { return null; }

        }

        private void XtraForm2_Load(object sender, EventArgs e)
        {

        }

        private void indexGridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                var filter = indexGridView.GetFocusedRowCellValue("GroupID").ToString();
                filtering = dtFillGrid.Copy();
                //if (filter == "الكل")
                //{

                //    gridControl2.DataSource = filtering;
                //    return;

                //}
                gridControl2.DataSource = null;
                if (filtering.Rows.Count > 0)
                {
                    DataRow dr;
                    for (int i = 0; i <= filtering.Rows.Count - 1; ++i)
                    {
                        if (DBNull.Value != filtering.Rows[i]["GroupID"] || !string.IsNullOrEmpty(filtering.Rows[i]["GroupID"].ToString()))
                        {
                            dr = filtering.Rows[i];
                            //if (dr["ArbName"].ToString().Substring(0, 1) == filter)
                            //{

                            //    DataRow row = filtering.NewRow();
                            //    row = dr;
                            //    filtering.Rows.Add(dt.Rows[i]);
                            //}
                            if (dr["GroupID"].ToString().Trim() != filter)
                                dr.Delete();
                        }
                    }
                    filtering.AcceptChanges();
                    if (filtering.Rows.Count < 1)
                    {
                        DataRow dr1;
                        dr1 = filtering.NewRow();
                        dr1["GroupID"] = 0;
                        dr1["ItemID"] = 0;
                        dr1["ItemName"] = "------";
                        dr1["ItemImage"] = null;
                        filtering.Rows.Add(dr1);
                    }
                    gridControl2.DataSource = filtering;
                    if (ShoeWrning.Checked == true)
                    {
                        frmWrningItemQty frm = new frmWrningItemQty();
                        frm.Show();
                        frm.Hide();
                    }

                }
            }
            catch { }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridControl2.MainView = gridView2;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gridControl2.MainView = layoutView1;
        }

        private void layoutView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            //   long ID = Comon.cLong(layoutView1.GetFocusedRowCellValue("EmployeeID").ToString());
            //EditTeacherInfo frm = new EditTeacherInfo(ID);
            //frm.Show();

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            //EditTeacherInfo frm = new EditTeacherInfo();
            //frm.Show();
        }

        private void gridItems_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            long ID = Comon.cLong(gridView2.GetFocusedRowCellValue("EmployeeID").ToString());
            //EditTeacherInfo frm = new EditTeacherInfo(ID);
            //frm.Show();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            fillGrid();
            filtering = dtFillGrid.Copy();
            gridControl2.DataSource = filtering;
        }

        private void gridItems_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.IsRowSelected(e.RowHandle))
            {
                e.Appearance.BackColor = System.Drawing.Color.Yellow;// System.Drawing.Color.FromArgb(25, 71, 138);
                e.Appearance.ForeColor = System.Drawing.Color.Black;
                e.HighPriority = true;
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            foreach (var rowHandle in gridView2.GetSelectedRows())
            {
                //EditTeacherInfo frm = new EditTeacherInfo(Comon.cLong(gridView2.GetRowCellValue(rowHandle, "EmployeeID").ToString()), true);
            }

            simpleButton3_Click(null, null);

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

            //try
            //{
            //    Application.DoEvents();
            //   SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            // //   gridControl2.ShowRibbonPrintPreview();
            //    /******************** Report Body *************************/

            //   bool IncludeHeader = true;
            //   string rptFormName = "rptEmpReport";


            //   XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

            //   /********************** Master *****************************/
            //   rptForm.RequestParameters = false;


            //   /********************** Details ****************************/
            //   var dataTable = new dsReports.TeacherAttenceDataTable();

            //   for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
            //   {
            //       var row = dataTable.NewRow();

            //       row["#"] = i + 1;

            //       row["TechNO"] = gridView2.GetRowCellValue(i, "EmployeeID").ToString();

            //       row["TechName"] = gridView2.GetRowCellValue(i, "ArbName").ToString();


            //       row["Date"] = gridView2.GetRowCellValue(i, "Telephone").ToString();

            //       row["LateMinute"] = gridView2.GetRowCellValue(i, "IdentityID").ToString();
            //       row["Earlyminute"] = gridView2.GetRowCellDisplayText(i, "DepartmentID").ToString();
            //       row["Status"] = gridView2.GetRowCellDisplayText(i, "SectionID").ToString();
            //       dataTable.Rows.Add(row);
            //   }
            //   rptForm.DataSource = dataTable;
            //   rptForm.DataMember = "TeacherAttence";
            //   /******************** Report Binding ************************/
            //   XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
            //   subreport.Visible = IncludeHeader;
            //   subreport.ReportSource = ReportComponent.CompanyHeader();
            //   rptForm.ShowPrintStatusDialog = false;
            //   rptForm.ShowPrintMarginsWarning = false;
            //   rptForm.CreateDocument();

            //   SplashScreenManager.CloseForm(false);
            //   if (ShowReportInReportViewer = true)
            //   {
            //       frmReportViewer frmRptViewer = new frmReportViewer();
            //       frmRptViewer.documentViewer1.DocumentSource = rptForm;
            //       frmRptViewer.ShowDialog();
            //   }
            //   else
            //   {
            //       bool IsSelectedPrinter = false;
            //       SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            //       DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + rptFormName + "'");
            //       if (dt.Rows.Count > 0) for (int i = 1; i < 6; i++)
            //           {
            //               string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
            //               if (!string.IsNullOrEmpty(PrinterName))
            //               {
            //                   rptForm.PrinterName = PrinterName;
            //                   rptForm.Print(PrinterName);
            //                   IsSelectedPrinter = true;
            //               }
            //           }
            //       SplashScreenManager.CloseForm(false);
            //       if (!IsSelectedPrinter)
            //           Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
            //   }

            //}
            //catch (Exception ex)
            //{
            //    SplashScreenManager.CloseForm(false);
            //    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            //}
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Application.DoEvents();
            //    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            //    foreach (var rowHandle in gridView2.GetSelectedRows())
            //    {
            //        EditTeacherInfo frm = new EditTeacherInfo(Comon.cLong(gridView2.GetRowCellValue(rowHandle, "EmployeeID").ToString()), Comon.cInt(gridView2.GetRowCellValue(rowHandle, "DepartmentID").ToString()));
            //    }
            //    SplashScreenManager.CloseForm(false);
            //    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

            //    simpleButton3_Click(null, null);
            //}
            //catch {
            //    SplashScreenManager.CloseForm(false);

        }
        private void layoutView1_CardClick(object sender, DevExpress.XtraGrid.Views.Layout.Events.CardClickEventArgs e)
        {

        }
        private void layoutViewSizing_CardClick(object sender, DevExpress.XtraGrid.Views.Layout.Events.CardClickEventArgs e)
        {
            frmSize.Dispose();
        }
        #region GridView
        void InitGrid()
        {
            lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/
            gridView2.Columns["DateFirstStr"].Visible = false;
            gridView2.Columns["DateFirst"].Visible = false;


            gridView2.Columns["BranchID"].Visible = false;
            gridView2.Columns["PackingQty"].Visible = false;
            gridView2.Columns["BAGET_W"].Visible = false;
            gridView2.Columns["STONE_W"].Visible = false;
            gridView2.Columns["rowhandling"].Visible = false;
            gridView2.Columns["extension"].Visible = false;
            gridView2.Columns["DIAMOND_W"].Visible = false;
            gridView2.Columns["Equivalen"].Visible = false;
            gridView2.Columns["Caliber"].Visible = false;
            gridView2.Columns["CostPrice"].Visible = false;
            gridView2.Columns["ExpiryDateStr"].Visible = false;
            gridView2.Columns["Bones"].Visible = false;
            gridView2.Columns["Height"].Visible = false;
            gridView2.Columns["Width"].Visible = false;
            gridView2.Columns["TheCount"].Visible = false;
            gridView2.Columns["ItemImage"].Visible = false;

            gridView2.Columns["Serials"].Visible = false;
            gridView2.Columns["InvoiceID"].Visible = false;
            gridView2.Columns["ID"].Visible = false;
            gridView2.Columns["FacilityID"].Visible = false;
            gridView2.Columns["StoreID"].Visible = false;
            gridView2.Columns["Cancel"].Visible = false;
            gridView2.Columns["SaleMaster"].Visible = false;
            gridView2.Columns["ArbItemName"].Visible = gridView2.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView2.Columns["EngItemName"].Visible = gridView2.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView2.Columns["ArbSizeName"].Visible = gridView2.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView2.Columns["EngSizeName"].Visible = gridView2.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;
            gridView2.Columns["BarCode"].Visible = MySession.GlobalAllowUsingBarcodeInInvoices;
            gridView2.Columns["ExpiryDate"].Visible = false;
            gridView2.Columns["Description"].Visible = false;
            /******************* Columns Visible=true *******************/

            gridView2.Columns[ItemName].Visible = true;
            gridView2.Columns[SizeName].Visible = true;
            gridView2.Columns["SizeID"].Visible = false;
            gridView2.Columns["Discount"].Visible = true;
            gridView2.Columns["HavVat"].Visible = false;
            gridView2.Columns["RemainQty"].Visible = false;
            gridView2.Columns["ItemID"].Visible = false;





            gridView2.Columns["BarCode"].Caption = CaptionBarCode;
            gridView2.Columns["ItemID"].Caption = CaptionItemID;
            gridView2.Columns["ItemID"].OptionsColumn.ReadOnly = true;
            gridView2.Columns["SizeID"].OptionsColumn.ReadOnly = true;
            gridView2.Columns["ItemID"].OptionsColumn.AllowEdit = true;
            gridView2.Columns["SizeID"].OptionsColumn.AllowEdit = true;
            gridView2.Columns[ItemName].Caption = CaptionItemName;
            gridView2.Columns[ItemName].Width = 200;
            gridView2.Columns["SizeID"].Caption = CaptionSizeID;
            gridView2.Columns[SizeName].Caption = CaptionSizeName;
            gridView2.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView2.Columns["QTY"].Caption = CaptionQTY;
            gridView2.Columns["Total"].Caption = CaptionTotal;
            gridView2.Columns["Discount"].Caption = CaptionDiscount;
            gridView2.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView2.Columns["Net"].Caption = CaptionNet;
            gridView2.Columns["SalePrice"].Caption = CaptionSalePrice;
            gridView2.Columns["Description"].Caption = CaptionDescription;
            gridView2.Columns["HavVat"].Caption = CaptionHavVat;
            gridView2.Columns["RemainQty"].Caption = CaptionRemainQty;
            gridView2.Focus();
            /*************************Columns Properties ****************************/
            gridView2.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
            gridView2.Columns["Total"].OptionsColumn.ReadOnly = true;
            gridView2.Columns["Total"].OptionsColumn.AllowFocus = false;
            gridView2.Columns["Net"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            gridView2.Columns["Net"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            gridView2.Columns["AdditionalValue"].OptionsColumn.ReadOnly = true;
            gridView2.Columns["AdditionalValue"].Visible = false;
            gridView2.Columns["Net"].Visible = false;



            gridView2.Columns["AdditionalValue"].OptionsColumn.AllowFocus = false;
            /************************ Date Time **************************/

            gridView2.Columns["ArbItemName"].Width = 200;
            gridView2.Columns["QTY"].Width = 60;
            gridView2.Columns[SizeName].Width = 110;

            gridView2.Columns["Total"].Width = 70;

            RepositoryItemDateEdit RepositoryDateEdit = new RepositoryItemDateEdit();
            RepositoryDateEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            RepositoryDateEdit.Mask.EditMask = "dd/MM/yyyy";
            RepositoryDateEdit.Mask.UseMaskAsDisplayFormat = true;
            gridControl.RepositoryItems.Add(RepositoryDateEdit);
            gridView2.Columns["ExpiryDate"].ColumnEdit = RepositoryDateEdit;
            gridView2.Columns["ExpiryDate"].UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            gridView2.Columns["ExpiryDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView2.Columns["ExpiryDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridView2.Columns["ExpiryDate"].OptionsColumn.AllowEdit = true;
            gridView2.Columns["ExpiryDate"].OptionsColumn.ReadOnly = false;


            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
            gridView2.Columns[ItemName].ColumnEdit = rItem;
            gridControl.RepositoryItems.Add(rItem);

            //RepositoryItemLookUpEdit rBarCode = Common.LookUpEditBarCode();
            //gridView2.Columns["BarCode"].ColumnEdit = rBarCode;
            //gridControl.RepositoryItems.Add(rBarCode);

            //RepositoryItemLookUpEdit rItemID = Common.LookUpEditItemID();
            //gridView2.Columns["ItemID"].ColumnEdit = rItemID;
            //gridControl.RepositoryItems.Add(rItemID);

            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView2.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView2.BestFitColumns();
            /******************************** Menu ***************************************/
            menu = new GridViewMenu(gridView2);
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

        }

        private void gridView2_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            //if (e.HitInfo != null && e.HitInfo.Column.Name == "colSalePrice")
            //    if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            //        e.Menu = menu;
        }
        private void gridView2_ShownEditor(object sender, EventArgs e)
        {
            setActiveControl(this.gridView2.ActiveEditor);
            setActiveColumn(this.gridView2.FocusedColumn);
            setActiveRowHandle(this.gridView2.FocusedRowHandle);

            if (this.gridView2.ActiveEditor is CheckEdit)
                if (chkForVat.Checked)
                {
                    GridView view = sender as GridView;

                    view.ActiveEditor.IsModified = true;

                    view.ActiveEditor.ReadOnly = false;
                }
            HasColumnErrors = false;


            CalculateRow();
        }
        private void gridView2_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {


                if (!gridView2.IsLastVisibleRow)
                    gridView2.MoveLast();

                foreach (GridColumn col in gridView2.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "Net" || col.FieldName == "Total" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID" || col.FieldName == "SalePrice")
                    {

                        var val = gridView2.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView2.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                        if (col.FieldName == "BarCode")
                            return;
                        else if (!(double.TryParse(val.ToString(), out num)))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView2.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView2.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }
                        else
                        {
                            e.Valid = true;
                            gridView2.SetColumnError(col, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {


            setActiveControl(this.gridView2.ActiveEditor);
            setActiveColumn(this.gridView2.FocusedColumn);
            setActiveRowHandle(this.gridView2.FocusedRowHandle);
            if (this.gridView2.ActiveEditor is CheckEdit)
            {
                if (e.Value != null)
                {
                    gridView2.Columns["HavVat"].OptionsColumn.AllowEdit = true;
                    CalculateRow(gridView2.FocusedRowHandle, Comon.cbool(e.Value.ToString()));
                }
            }
            else if (this.gridView2.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
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
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }
                    else
                    {
                        e.Valid = true;
                        view.SetColumnError(gridView2.Columns[ColName], "");

                    }
                    /****************************************/
                    if (ColName == "BarCode")
                    {
                        int flag = 0;
                        for (int i = 0; i < gridView2.RowCount - 1; ++i)
                        {
                            if (i == gridView2.FocusedRowHandle)
                                if (gridView2.IsNewItemRow(gridView2.FocusedRowHandle))
                                    continue;
                                else
                                {
                                    if (gridView2.GetRowCellValue(i, "BarCode").Equals(val.ToString()))
                                    {

                                        gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) + 1);
                                        //   if (gridView2.IsNewItemRow(gridView2.FocusedRowHandle))
                                        // gridView2.DeleteRow(gridView2.FocusedRowHandle);
                                        e.Valid = true;
                                        view.SetColumnError(gridView2.Columns[ColName], "");
                                        gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                        gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                                        flag = 1;
                                        break;
                                    }

                                }
                            if (gridView2.GetRowCellValue(i, "BarCode").Equals(val.ToString()))
                            {

                                gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) + 1);
                                if (gridView2.IsNewItemRow(gridView2.FocusedRowHandle))
                                    gridView2.DeleteRow(gridView2.FocusedRowHandle);
                                e.Valid = true;
                                view.SetColumnError(gridView2.Columns[ColName], "");
                                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                                flag = 1;
                                break;
                            }


                        }
                        if (flag == 1)
                            return;

                        DataTable dt = Stc_itemsDAL.GetItemData1(val.ToString(), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisBarCode;
                        }
                        else
                        {

                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                            gridView2.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);

                            FileItemData(dt, 1);
                            e.Valid = true;

                            view.SetColumnError(gridView2.Columns[ColName], "");
                            gridView2.MoveLastVisible();
                            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                            gridView2.FocusedColumn = gridView2.VisibleColumns[0];
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


                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(val.ToString()));
                            gridView2.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);
                            FileItemData(dt, 1);
                            e.Valid = true;
                            view.SetColumnError(gridView2.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        int ItemID = Comon.cInt(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"]).ToString());
                        DataTable dt = Stc_itemsDAL.GetItemDataByItemID_SizeID(ItemID, Comon.cInt(val.ToString()), UserInfo.FacilityID);

                        if (dt == null || dt.Rows.Count == 0)
                        {

                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundSizeForItem;
                            view.SetColumnError(gridView2.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {
                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                            gridView2.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);
                            FileItemData(dt, 1);
                            e.Valid = true;
                            view.SetColumnError(gridView2.Columns[ColName], "");
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
                            view.SetColumnError(gridView2.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {

                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dtItemID.Rows[0]["ItemID"].ToString()));
                            gridView2.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);
                            FileItemData(dtItem, 1);
                            e.Valid = true;
                            view.SetColumnError(gridView2.Columns[ColName], "");
                        }
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(gridView2.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtSize = Lip.SelectRecord("Select SizeID, " + PrimaryName + " AS " + SizeName + " from Stc_SizingUnits Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtSize.Rows.Count > 0)
                    {
                        var ItemID = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "ItemID");
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
                                FileItemData(dt, 1);
                                e.Valid = true;
                                view.SetColumnError(gridView2.Columns[ColName], "");
                            }
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgInputIsRequired;
                            view.SetColumnError(gridView2.Columns["ItemID"], Messages.msgNoFoundSizeForItem);
                        }

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(gridView2.Columns[ColName], Messages.msgNoFoundSizeForItem);

                    }
                }
                else if (ColName == "Discount")
                {
                    decimal QTY = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY").ToString());
                    decimal SalePrice = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "SalePrice").ToString());
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
                    if (this.gridView2.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        CalculateRow(gridView2.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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
                                view.SetColumnError(gridView2.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)))
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView2.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0)
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView2.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {


                                view.SetColumnError(gridView2.Columns[ColName], "");
                            }
                        }

                    }
                }

                else if (e.KeyData == Keys.Delete)
                {


                    if ((gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0) || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER1") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER0") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER2") || Comon.cInt(gridView2.GetFocusedRowCellValue("DIAMOND_W").ToString()) == 1)
                        return;
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
                try
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        gridView2.PostEditor();
                        gridView2.UpdateCurrentRow();
                        if (Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "SalePrice").ToString()) <= 0)
                        {

                            HasColumnErrors = true;
                            view.SetColumnError(gridView2.Columns["SalePrice"], Messages.msgInputIsGreaterThanZero);
                            return;
                        }


                        gridView2.FocusedRowHandle = GridControl.NewItemRowHandle;
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
        private void gridView2_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = (e.ListSourceRowIndex + 1);
        }
        private void gridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            setActiveControl(this.gridView2.ActiveEditor);
            setActiveColumn(this.gridView2.FocusedColumn);
            setActiveRowHandle(this.gridView2.FocusedRowHandle);
            if (this.gridView2.ActiveEditor is CheckEdit)
            {
                gridView2.Columns["HavVat"].OptionsColumn.AllowEdit = true;
                CalculateRow(gridView2.FocusedRowHandle, Comon.cbool(e.Value.ToString()));
            }
            //if (barcodeLast != "")
            //    flagError = 0;

            //if (gridView2.GetFocusedRowCellValue("BarCode") != null && flagError == 0)
            //{

            //    barcodeLast = gridView2.GetFocusedRowCellValue("BarCode").ToString();
            //    flagError = 1;
            //    gridView2.MoveLast();
            //    gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            //    gridView2.FocusedColumn = gridView2.VisibleColumns[0];

            //}
        }
        private void gridView2_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            try
            {
                setActiveControl(this.ActiveControl);
                setActiveColumn(this.gridView2.FocusedColumn);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            try
            {

                setActiveRowHandle(this.gridView2.FocusedRowHandle);

                strQty = "0";
                //byte[] imgByte = null;
                //if (DBNull.Value != gridView2.GetFocusedRowCellValue("ItemImage"))
                //{
                //    imgByte = (byte[])gridView2.GetFocusedRowCellValue("ItemImage");
                //    if (imgByte != null)
                //        picItemUnits.Image = byteArrayToImage(imgByte);
                //    else
                //        picItemUnits.Image = null;

                //}
                //else
                //    picItemUnits.Image = null;
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private DataRow[] FileItemData(DataTable dt, decimal QtyIn)
        {

            //int[] itemgroup = new int[] { 0, 0};
            DataRow[] drgroupItem = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PackingQty"], dt.Rows[0]["PackingQty"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], dt.Rows[0]["ArbSizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ArbItemName"], dt.Rows[0]["ArbName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ArbSizeName"], dt.Rows[0]["ArbSizeName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngSizeName"], dt.Rows[0]["SizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                try
                {
                    if (Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()) <= 0)
                    {

                        //if (Comon.ConvertToDecimalPrice(dt.Rows[0]["CostPrice"].ToString()) > 0)
                        //    dt.Rows[0]["SalePrice"] = dt.Rows[0]["CostPrice"];
                        //else
                        dt.Rows[0]["SalePrice"] = 0;

                    }
                }
                catch { };
                //if (Comon.cInt(dt.Rows[0]["TypeID"].ToString()) == 8)
                {

                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], getNote(Comon.cInt(dt.Rows[0]["ItemID"].ToString())));
                }
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BAGET_W"], dt.Rows[0]["SalePrice"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["extension"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["DIAMOND_W"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], "");


                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemainQty"], 0);


                //try
                //{
                //    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                //        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemImage"], dt.Rows[0]["ItemImage"]);
                //}
                //catch { }
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["StoreID"], txtStoreID.Text);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Discount"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AdditionalValue"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Cancel"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PageNo"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemStatus"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Caliber"], dt.Rows[0]["BrandID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Equivalen"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Net"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["TheCount"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Width"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Total"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], QtyIn);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"], Comon.cbool(dt.Rows[0]["IsVat"].ToString()));
                decimal ResultSalePrice = Comon.cDec(dt.Rows[0]["SalePrice"].ToString());



                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemainQty"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["extension"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["DIAMOND_W"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["rowhandling"], "");
                DataRow[] dr;


                if (dtPriceItemOffers.Rows.Count > 0)
                {

                    dr = dtPriceItemOffers.Select("((FromGroupID<=" + dt.Rows[0]["GroupID"].ToString() + "and ToGroupID>=" + dt.Rows[0]["GroupID"].ToString() + " )AND(FromItemID<=" + dt.Rows[0]["ItemID"].ToString() + "and ToItemID>=" + dt.Rows[0]["ItemID"].ToString() + " ) and (FromSizeID<=" + dt.Rows[0]["SizeID"].ToString() + "and ToISizeID>=" + dt.Rows[0]["SizeID"].ToString() + " )) or((FromItemID<=" + dt.Rows[0]["ItemID"].ToString() + "and ToItemID>=" + dt.Rows[0]["ItemID"].ToString() + " ) and (FromSizeID<=" + dt.Rows[0]["SizeID"].ToString() + "and ToISizeID>=" + dt.Rows[0]["SizeID"].ToString() + " ))OR ((FromItemID<=" + dt.Rows[0]["ItemID"].ToString() + "and ToItemID>=" + dt.Rows[0]["ItemID"].ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + dt.Rows[0]["GroupID"].ToString() + "and ToGroupID>=" + dt.Rows[0]["GroupID"].ToString() + " ) ");
                    if (dr.Length > 0 && gridView2.Columns["Description"].ToString() != "INS")
                    {
                        DateTime nowDate = DateTime.ParseExact(Comon.ConvertSerialDateTo(Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString()), "dd/MM/yyyy", culture);
                        int i = (int)nowDate.DayOfWeek;
                        int timenow = Comon.cInt(Lip.GetServerTimeSerial());
                        //  if (Comon.cInt(dr[0]["day" + i].ToString()) == 1 && (timenow >= Comon.cInt(dr[0]["FromTime"].ToString()) && timenow <= Comon.cInt(dr[0]["ToTime"].ToString())))
                        if (Comon.cInt(dr[0]["IsAmount"].ToString()) > 0)
                        {
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Discount"], dr[0]["AmountCost"].ToString());
                            // itemgroup[2] = 1;
                        }
                        else if (Comon.cInt(dr[0]["IsPercent"].ToString()) > 0)
                        {
                            decimal PercentAmount = Comon.ConvertToDecimalPrice(dr[0]["PercentCost"].ToString());
                            decimal total = Comon.ConvertToDecimalPrice(ResultSalePrice * QtyIn * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Discount"], total);
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], PercentAmount);
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "IsPercent");

                            //  itemgroup[2] = 1;
                        }

                        if (Comon.cInt(dr[0]["IsOffers"].ToString()) > 0)
                        {
                            drgroupItem = dr;
                            decimal QtyOffers;
                            if (Comon.cInt(dr[0]["IsTakeOne"].ToString()) > 0)
                            {
                                AddnewItem(dt.Rows[0]["BarCode"].ToString(), Comon.ConvertToDecimalPrice(1), "ISOFFER0");

                            }
                            else if (Comon.cInt(dr[0]["IsGetSame"].ToString()) > 0)
                            {

                                QtyOffers = Comon.ConvertToDecimalPrice(dr[0]["GetSameAmount"].ToString());
                                if (QtyOffers >= QtyIn)
                                    AddnewItem(dt.Rows[0]["BarCode"].ToString(), Comon.ConvertToDecimalPrice(dr[0]["SetSameAmount"].ToString()), "ISOFFER0");
                                else
                                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "ISOFFER1");

                            }

                            else if (Comon.cInt(dr[0]["IsGetOnther"].ToString()) > 0)
                            {
                                QtyOffers = Comon.ConvertToDecimalPrice(dr[0]["GetOntherAmount"].ToString());


                            }



                        }







                    }
                }

            }
            else
            {
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[ItemName], " ");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], " ");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CostPrice"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SizeID"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["StoreID"], txtStoreID.Text);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PageNo"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemStatus"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Caliber"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Equivalen"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Net"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["TheCount"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AdditionalValue"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Cancel"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Width"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Total"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"], true);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemindQty"], 0);
            }

            return drgroupItem;
        }





        private void FileItemDataOffers(DataTable dt, decimal QtyIn, string description)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PackingQty"], dt.Rows[0]["PackingQty"].ToString());

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], dt.Rows[0]["ArbSizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ArbItemName"], dt.Rows[0]["ArbName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ArbSizeName"], dt.Rows[0]["ArbSizeName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngSizeName"], dt.Rows[0]["SizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                try
                {
                    if (Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()) <= 0)
                    {

                        //if (Comon.ConvertToDecimalPrice(dt.Rows[0]["CostPrice"].ToString()) > 0)
                        //    dt.Rows[0]["SalePrice"] = dt.Rows[0]["CostPrice"];
                        //else
                        dt.Rows[0]["SalePrice"] = 0;

                    }
                }
                catch { };
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], 0);

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BAGET_W"], dt.Rows[0]["SalePrice"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["extension"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["DIAMOND_W"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], "");
                //try
                //{
                //    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                //        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemImage"], dt.Rows[0]["ItemImage"]);
                //}
                //catch { }
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["StoreID"], txtStoreID.Text);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Discount"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AdditionalValue"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Cancel"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], description);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PageNo"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemStatus"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Caliber"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Equivalen"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Net"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["TheCount"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Width"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Total"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], QtyIn);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"], false);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemainQty"], 0);


            }
            else
            {
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[ItemName], " ");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], " ");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CostPrice"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SizeID"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["StoreID"], txtStoreID.Text);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PageNo"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemStatus"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Caliber"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Equivalen"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Net"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["TheCount"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AdditionalValue"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Cancel"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Width"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Total"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"], true);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemindQty"], 0);
            }

        }


















        private void FileItemData1(DataTable dt, decimal QtyIn)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PackingQty"], dt.Rows[0]["PackingQty"].ToString());

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], dt.Rows[0]["ArbSizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ArbItemName"], dt.Rows[0]["ArbName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ArbSizeName"], dt.Rows[0]["ArbSizeName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngSizeName"], dt.Rows[0]["SizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                try
                {
                    if (Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()) <= 0)
                    {

                        //if (Comon.ConvertToDecimalPrice(dt.Rows[0]["CostPrice"].ToString()) > 0)
                        //    dt.Rows[0]["SalePrice"] = dt.Rows[0]["CostPrice"];
                        //else
                        dt.Rows[0]["SalePrice"] = 0;

                    }
                }
                catch { };
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], QtyIn);

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BAGET_W"], dt.Rows[0]["SalePrice"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["extension"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["DIAMOND_W"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], "");
                //try
                //{
                //    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                //        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemImage"], dt.Rows[0]["ItemImage"]);
                //}
                //catch { }
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["StoreID"], txtStoreID.Text);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Discount"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AdditionalValue"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Cancel"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "0Trans");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PageNo"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemStatus"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Caliber"], -1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Equivalen"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Net"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["TheCount"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Width"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Total"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"], false);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemainQty"], 0);


            }
            else
            {
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[ItemName], " ");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], " ");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CostPrice"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SizeID"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], "0");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["StoreID"], txtStoreID.Text);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PageNo"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemStatus"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Caliber"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Equivalen"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Net"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["TheCount"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AdditionalValue"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Cancel"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Width"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Total"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"], true);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemindQty"], 0);
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
            foreach (GridColumn col in gridView2.Columns)
            {
                if (col.FieldName == "BarCode" || col.FieldName == "Description" || col.FieldName == "Discount" || col.FieldName == "ExpiryDate" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SalePrice")
                {
                    gridView2.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView2.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView2.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }
                else if (col.FieldName == "HavVat")
                {
                    gridView2.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView2.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView2.Columns[col.FieldName].OptionsColumn.ReadOnly = !chkForVat.Checked;
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

            gridView2.MoveLast();

            int length = gridView2.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in gridView2.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "Net" || col.FieldName == "Total" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID" || col.FieldName == "SalePrice")
                    {

                        var cellValue = gridView2.GetRowCellValue(i, col); ;

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView2.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;

                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            gridView2.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            gridView2.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
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
                gridView2.PostEditor();
                SumTotalBalanceAndDiscount(Row, IsHavVat);
                //Remove Icon Validtion
                var Net = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Net");
                var Total = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Total");
                if ((Total != null && !(string.IsNullOrWhiteSpace(Total.ToString())) && Comon.ConvertToDecimalPrice(Total.ToString()) > 0))
                    gridView2.SetColumnError(gridView2.Columns["Total"], "");
                if ((Net != null && !(string.IsNullOrWhiteSpace(Net.ToString())) && Comon.ConvertToDecimalPrice(Net.ToString()) > 0))
                    gridView2.SetColumnError(gridView2.Columns["Net"], "");
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public decimal getTotalBeforeDiscount()
        {
            decimal TotalBeforeDiscount = 0;
            decimal QTYRow = 0;
            decimal SalePriceRow = 0;
            try
            {

                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    QTYRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                    TotalBeforeDiscount += Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);

                }
                return TotalBeforeDiscount;
            }
            catch { return TotalBeforeDiscount; }

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
                decimal InsurmentRow = 0;
                bool HavVatRow = false;

                //string text = System.IO.File.ReadAllText(Application.StartupPath + "\\typevat.txt");
                //if (text == "0")
                //    HavVatRow =   true;
                 
                
                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    DataRow[] dr;
                    QTYRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                    if (Comon.cInt(txtCustomerID.Text) > 0)
                    {
                        if (dtPriceCustomersOffers.Rows.Count > 0)
                        {
                            decimal beforTotal = getTotalBeforeDiscount();
                            dr = dtPriceCustomersOffers.Select("(FromCustomerID<=" + Comon.cInt(txtCustomerID.Text) + "and ToCustomerID>=" + Comon.cInt(txtCustomerID.Text) + " ) Or (ISForAll=1) or (FromSaleTotal<=" + beforTotal + "and ToSaleTotal>=" + beforTotal + "  and FromSaleTotal<>0 and ToSaleTotal<>0  )");

                            if (dr.Length > 0)
                            {
                                if (Comon.cInt(dr[0]["IsOffers"].ToString()) > 0)
                                {
                                    if (((Comon.cInt(dr[0]["FromGroupID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "Caliber")) && Comon.cInt(dr[0]["ToGroupID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "Caliber"))) && (Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID"))) && (Comon.cInt(dr[0]["FromSizeID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")) && Comon.cInt(dr[0]["ToISizeID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")))) || ((Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID"))) && (Comon.cInt(dr[0]["FromSizeID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")) && Comon.cInt(dr[0]["ToISizeID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")))) || ((Comon.cInt(dr[0]["FromGroupID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "Caliber")) && Comon.cInt(dr[0]["ToGroupID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "Caliber")))) || ((Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")))))
                                    {

                                        decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dr[0]["PercentOfferAmount"].ToString()));
                                        decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                                        gridView2.SetRowCellValue(i, gridView2.Columns["Discount"], total);
                                        gridView2.SetRowCellValue(i, gridView2.Columns["Description"], "OFFERCUSTOMER-" + Comon.cInt(txtCustomerID.Text));
                                    }

                                }

                            }
                        }
                    }
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount"));

                    var declerationINS = gridView2.GetRowCellValue(i, "Description").ToString();
                    if (declerationINS == "INS")
                    {
                        HavVatRow = false;
                        InsurmentRow += Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                    }

                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                    AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;

                    if (HavVatRow == false)
                        AdditionalAmountRow = 0;



                    NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);

                    gridView2.SetRowCellValue(i, gridView2.Columns["Total"], TotalRow.ToString());
                    gridView2.SetRowCellValue(i, gridView2.Columns["AdditionalValue"], AdditionalAmountRow.ToString());
                    gridView2.SetRowCellValue(i, gridView2.Columns["Net"], NetRow.ToString());

                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;

                }

                if (rowIndex < 0)
                {
                    var ResultQTY = gridView2.GetRowCellValue(rowIndex, "QTY");
                    var ResultSalePrice = gridView2.GetRowCellValue(rowIndex, "SalePrice");
                    var ResultDiscount = gridView2.GetRowCellValue(rowIndex, "Discount");
                    var ResultHavVat = gridView2.GetRowCellValue(rowIndex, "HavVat");

                    QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                    SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;


                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    HavVatRow = row == rowIndex ? IsHavVat : Comon.cbool(gridView2.GetRowCellValue(rowIndex, "HavVat"));

                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                    var declerationINS = gridView2.GetRowCellValue(rowIndex, "Description");
                    if (declerationINS == null)
                        HavVatRow = false;
                    else
                        if (declerationINS.ToString() == "INS")
                        {
                            HavVatRow = false;
                            InsurmentRow += Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                        }


                    if (HavVatRow == false)
                        AdditionalAmountRow = 0;


                    // AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;


                    //if (AdditionalAmountRow == 0)
                    //    AdditionalAmountRow = Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat);


                    NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);

                    gridView2.SetRowCellValue(rowIndex, gridView2.Columns["Total"], TotalRow.ToString());
                    gridView2.SetRowCellValue(rowIndex, gridView2.Columns["AdditionalValue"], AdditionalAmountRow.ToString());
                    gridView2.SetRowCellValue(rowIndex, gridView2.Columns["Net"], NetRow.ToString());

                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }

                lblUnitDiscount.Text = DiscountTotal.ToString("N" + MySession.GlobalPriceDigits);
                if (Comon.cInt(txtCustomerID.Text) > 0)
                {
                    DataRow[] dr;
                    if (dtPriceCustomersOffers.Rows.Count > 0)
                    {
                        dr = dtPriceCustomersOffers.Select("(FromCustomerID<=" + Comon.cInt(txtCustomerID.Text) + "and ToCustomerID>=" + Comon.cInt(txtCustomerID.Text) + " ) Or (ISForAll=1) or (FromSaleTotal<=" + TotalBeforeDiscount + "and ToSaleTotal>=" + TotalBeforeDiscount + ")");

                        if (dr.Length > 0)
                        {

                            if (Comon.cInt(dr[0]["IsAmount"].ToString()) > 0)
                            {
                                decimal PercentAmount = Comon.ConvertToDecimalPrice(dr[0]["AmountCost"].ToString());
                                // decimal whole = Comon.ConvertToDecimalPrice(TotalAfterDiscount);

                                txtDiscountOnTotal.Text = PercentAmount.ToString("N" + MySession.GlobalPriceDigits);
                            }
                            else if (Comon.cInt(dr[0]["IsPercent"].ToString()) > 0)
                            {
                                decimal PercentAmount = Comon.ConvertToDecimalPrice(dr[0]["PercentCost"].ToString());
                                decimal whole = Comon.ConvertToDecimalPrice(TotalAfterDiscount);

                                txtDiscountOnTotal.Text = ((PercentAmount * whole) / 100).ToString("N" + MySession.GlobalPriceDigits);

                                //  itemgroup[2] = 1;
                            }


                            else if (Comon.cInt(dr[0]["IsOffers"].ToString()) > 0)
                            {
                                //                              for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                                //                              {

                                //if (((Comon.cInt(dr[0]["FromGroupID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "GroupID")) && Comon.cInt(dr[0]["ToGroupID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "GroupID"))) && (Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID"))) && (Comon.cInt(dr[0]["FromSizeID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")) && Comon.cInt(dr[0]["ToISizeID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")))) || ((Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID"))) && (Comon.cInt(dr[0]["FromSizeID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")) && Comon.cInt(dr[0]["ToISizeID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")))) || ((Comon.cInt(dr[0]["FromGroupID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "GroupID")) && Comon.cInt(dr[0]["ToGroupID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "GroupID"))))||((Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")))))

                                //{

                                //    decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dr[0]["PercentOfferAmount"].ToString()));
                                //    decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                                //    gridView2.SetRowCellValue(i, gridView2.Columns["Discount"], total);
                                //}

                                //                              }

                            }



                        }





                    }



                }
                DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);
                txtInsuranceAmmount.Text = Comon.ConvertToDecimalPrice(InsurmentRow).ToString("N" + MySession.GlobalPriceDigits); ;
                if (DiscountOnTotal > 0)
                {
                    decimal Total = TotalAfterDiscount - DiscountOnTotal;
                    AdditionalAmount = (Total) / 100 * MySession.GlobalPercentVat;
                    Net = Comon.ConvertToDecimalPrice(Total + AdditionalAmount);
                }
                 

                AdditionalAmount = Comon.ConvertToDecimalPrice(Net - ((Net * 100) / (100 + MySession.GlobalPercentVat)));
                lblAdditionaAmmount.Text = Comon.ConvertToDecimalPrice(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = Comon.ConvertToDecimalPrice(Net).ToString("N" + MySession.GlobalPriceDigits);

                //if (text == "1")
               lblInvoiceTotalBeforeDiscount.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) -AdditionalAmount - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);


            }

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        //private void SumTotalBalanceAndDiscount(int row = -1, bool IsHavVat = false)
        //{
        //    try
        //    {
        //        decimal TotalAfterDiscount = 0;
        //        decimal TotalBeforeDiscount = 0;
        //        decimal Net = 0;
        //        decimal DiscountTotal = 0;
        //        decimal DiscountOnTotal = 0;
        //        decimal AdditionalAmount = 0;

        //        decimal DiscountRow = 0;
        //        decimal QTYRow = 0;
        //        decimal SalePriceRow = 0;
        //        decimal TotalRow = 0;
        //        decimal NetRow = 0;
        //        decimal TotalBeforeDiscountRow = 0;
        //        decimal AdditionalAmountRow = 0;
        //        decimal InsurmentRow = 0;
        //        bool HavVatRow = false;

        //        for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
        //        {
        //            DataRow[] dr;
        //            QTYRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
        //            SalePriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
        //            AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue").ToString());
        //            DiscountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount")) / QTYRow;
        //            HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView2.GetRowCellValue(i, "HavVat"));


        //            decimal PercentAmount = 20;

        //            if (HavVatRow == false)
        //            {

        //                decimal vat = Comon.cDec(Comon.cDec(SalePriceRow) / 100 *  MySession.GlobalPercentVat);



        //                DiscountRow =   Comon.cDec(Comon.cDec(SalePriceRow) / 100 * Comon.cDec(PercentAmount));

        //                gridView2.SetRowCellValue(rowIndex, gridView2.Columns["SalePrice"], SalePriceRow.ToString());
        //                gridView2.SetRowCellValue(rowIndex, gridView2.Columns["Discount"], DiscountRow.ToString());

        //                AdditionalAmountRow = Comon.ConvertToDecimalPrice(((Comon.cDec(SalePriceRow) - Comon.cDec(DiscountRow)) / 100 * MySession.GlobalPercentVat)) * Comon.cDec(QTYRow);

        //            }

        //            else
        //            {
        //                SalePriceRow = Comon.cDec(SalePriceRow) - (Comon.cDec(SalePriceRow) * Comon.cDec(1));

        //                AdditionalAmountRow = Comon.ConvertToDecimalPrice((Comon.cDec(SalePriceRow) / 100 * MySession.GlobalPercentVat)) * Comon.cDec(QTYRow);

        //            }



        //            if (Comon.cInt(txtCustomerID.Text) > 0)
        //            {
        //                if (dtPriceCustomersOffers.Rows.Count > 0)
        //                {
        //                    decimal beforTotal = getTotalBeforeDiscount();
        //                    dr = dtPriceCustomersOffers.Select("(FromCustomerID<=" + Comon.cInt(txtCustomerID.Text) + "and ToCustomerID>=" + Comon.cInt(txtCustomerID.Text) + " ) Or (ISForAll=1) or (FromSaleTotal<=" + beforTotal + "and ToSaleTotal>=" + beforTotal + "  and FromSaleTotal<>0 and ToSaleTotal<>0  )");

        //                    if (dr.Length > 0)
        //                    {
        //                        if (Comon.cInt(dr[0]["IsOffers"].ToString()) > 0)
        //                        {
        //                            if (((Comon.cInt(dr[0]["FromGroupID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "Caliber")) && Comon.cInt(dr[0]["ToGroupID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "Caliber"))) && (Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID"))) && (Comon.cInt(dr[0]["FromSizeID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")) && Comon.cInt(dr[0]["ToISizeID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")))) || ((Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID"))) && (Comon.cInt(dr[0]["FromSizeID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")) && Comon.cInt(dr[0]["ToISizeID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")))) || ((Comon.cInt(dr[0]["FromGroupID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "Caliber")) && Comon.cInt(dr[0]["ToGroupID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "Caliber")))) || ((Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")))))
        //                            {

        //                                  PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dr[0]["PercentOfferAmount"].ToString()));
        //                                decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
        //                                gridView2.SetRowCellValue(i, gridView2.Columns["Discount"], total);
        //                                gridView2.SetRowCellValue(i, gridView2.Columns["Description"], "OFFERCUSTOMER-" + Comon.cInt(txtCustomerID.Text));
        //                            }

        //                        }

        //                    }
        //                }
        //            }



        //            var declerationINS = gridView2.GetRowCellValue(i, "Description").ToString();
        //            if (declerationINS == "INS")
        //            {
        //                HavVatRow = false;
        //                InsurmentRow += Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
        //            }

        //            TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
        //            TotalRow = Comon.ConvertToDecimalPrice(QTYRow * (SalePriceRow - DiscountRow));


        //                //الضريبو شامل                   

        //            NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);


        //            gridView2.SetRowCellValue(i, gridView2.Columns["Total"], TotalRow.ToString());
        //            gridView2.SetRowCellValue(i, gridView2.Columns["AdditionalValue"], AdditionalAmountRow.ToString());
        //            gridView2.SetRowCellValue(i, gridView2.Columns["Net"], NetRow.ToString());

        //            TotalBeforeDiscount += TotalBeforeDiscountRow;
        //            TotalAfterDiscount += TotalRow;
        //            DiscountTotal += DiscountRow;
        //            AdditionalAmount += AdditionalAmountRow;
        //            Net += NetRow;

        //        }

        //        if (rowIndex < 0)
        //        {
        //            var ResultQTY = gridView2.GetRowCellValue(rowIndex, "QTY");
        //            var ResultSalePrice = gridView2.GetRowCellValue(rowIndex, "SalePrice");
        //            var ResultDiscount = gridView2.GetRowCellValue(rowIndex, "Discount");
        //            var ResultHavVat = gridView2.GetRowCellValue(rowIndex, "HavVat");
        //            var AdditionalAmountRow1 = gridView2.GetRowCellValue(rowIndex, "AdditionalValue");
        //            decimal PercentAmount = 20;




        //            if (Comon.cbool(ResultHavVat) == false)
        //            {

        //                decimal vat = (Comon.ConvertToDecimalPrice(Comon.cDec(ResultSalePrice) - ((Comon.cDec(ResultSalePrice) * 100) / (100 + MySession.GlobalPercentVat))));

        //                decimal vat1 = Comon.cDec(Comon.cDec(ResultSalePrice) / 100 * MySession.GlobalPercentVat);

        //                ResultSalePrice = Comon.cDec(Comon.cDec(ResultSalePrice) - vat);

        //                ResultDiscount = Comon.cDec(Comon.cDec(ResultSalePrice) / 100 * Comon.cDec(PercentAmount));

        //                gridView2.SetRowCellValue(rowIndex, gridView2.Columns["SalePrice"], ResultSalePrice.ToString());
        //                gridView2.SetRowCellValue(rowIndex, gridView2.Columns["Discount"], ResultDiscount.ToString());

        //                AdditionalAmountRow = Comon.ConvertToDecimalPrice(((Comon.cDec(ResultSalePrice) - Comon.cDec(ResultDiscount)) / 100 * MySession.GlobalPercentVat)) * Comon.cDec(ResultQTY);

        //            }

        //            else
        //            {


        //                AdditionalAmountRow = Comon.ConvertToDecimalPrice(((Comon.cDec(ResultSalePrice) - Comon.cDec(ResultDiscount)) / 100 * MySession.GlobalPercentVat)) * Comon.cDec(ResultQTY);

        //            }

        //            QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
        //            SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
        //            DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;


        //            HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
        //            HavVatRow = row == rowIndex ? IsHavVat : Comon.cbool(gridView2.GetRowCellValue(rowIndex, "HavVat"));

        //            TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * (SalePriceRow));
        //            TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);


        //            var declerationINS = gridView2.GetRowCellValue(rowIndex, "Description");
        //            if (declerationINS == null)
        //                HavVatRow = false;
        //            else
        //                if (declerationINS.ToString() == "INS")
        //                {
        //                    HavVatRow = false;
        //                    InsurmentRow += Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
        //                }



        //            NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);





        //            gridView2.SetRowCellValue(rowIndex, gridView2.Columns["Total"], TotalRow.ToString());
        //            gridView2.SetRowCellValue(rowIndex, gridView2.Columns["AdditionalValue"], AdditionalAmountRow.ToString());
        //            gridView2.SetRowCellValue(rowIndex, gridView2.Columns["Net"], NetRow.ToString());

        //            TotalBeforeDiscount += TotalBeforeDiscountRow;
        //            TotalAfterDiscount += TotalRow;
        //            DiscountTotal += DiscountRow;
        //            AdditionalAmount += AdditionalAmountRow;
        //            Net += NetRow;
        //        }

        //        lblUnitDiscount.Text = DiscountTotal.ToString("N" + MySession.GlobalPriceDigits);
        //        if (Comon.cInt(txtCustomerID.Text) > 0)
        //        {
        //            DataRow[] dr;
        //            if (dtPriceCustomersOffers.Rows.Count > 0)
        //            {
        //                dr = dtPriceCustomersOffers.Select("(FromCustomerID<=" + Comon.cInt(txtCustomerID.Text) + "and ToCustomerID>=" + Comon.cInt(txtCustomerID.Text) + " ) Or (ISForAll=1) or (FromSaleTotal<=" + TotalBeforeDiscount + "and ToSaleTotal>=" + TotalBeforeDiscount + ")");

        //                if (dr.Length > 0)
        //                {

        //                    if (Comon.cInt(dr[0]["IsAmount"].ToString()) > 0)
        //                    {
        //                        decimal PercentAmount = Comon.ConvertToDecimalPrice(dr[0]["AmountCost"].ToString());
        //                        // decimal whole = Comon.ConvertToDecimalPrice(TotalAfterDiscount);

        //                        txtDiscountOnTotal.Text = PercentAmount.ToString("N" + MySession.GlobalPriceDigits);
        //                    }
        //                    else if (Comon.cInt(dr[0]["IsPercent"].ToString()) > 0)
        //                    {
        //                        decimal PercentAmount = Comon.ConvertToDecimalPrice(dr[0]["PercentCost"].ToString());
        //                        decimal whole = Comon.ConvertToDecimalPrice(TotalAfterDiscount);

        //                        txtDiscountOnTotal.Text = ((PercentAmount * whole) / 100).ToString("N" + MySession.GlobalPriceDigits);

        //                        //  itemgroup[2] = 1;
        //                    }


        //                    else if (Comon.cInt(dr[0]["IsOffers"].ToString()) > 0)
        //                    {
        //                        //                              for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
        //                        //                              {

        //                        //if (((Comon.cInt(dr[0]["FromGroupID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "GroupID")) && Comon.cInt(dr[0]["ToGroupID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "GroupID"))) && (Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID"))) && (Comon.cInt(dr[0]["FromSizeID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")) && Comon.cInt(dr[0]["ToISizeID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")))) || ((Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID"))) && (Comon.cInt(dr[0]["FromSizeID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")) && Comon.cInt(dr[0]["ToISizeID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "SizeID")))) || ((Comon.cInt(dr[0]["FromGroupID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "GroupID")) && Comon.cInt(dr[0]["ToGroupID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "GroupID"))))||((Comon.cInt(dr[0]["FromItemID"].ToString()) <= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")) && Comon.cInt(dr[0]["ToItemID"].ToString()) >= Comon.cInt(gridView2.GetRowCellValue(i, "ItemID")))))

        //                        //{

        //                        //    decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dr[0]["PercentOfferAmount"].ToString()));
        //                        //    decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
        //                        //    gridView2.SetRowCellValue(i, gridView2.Columns["Discount"], total);
        //                        //}

        //                        //                              }

        //                    }



        //                }





        //            }



        //        }
        //        DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
        //        lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);




        //        lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount + DiscountTotal).ToString("N" + MySession.GlobalPriceDigits);

        //        lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);
        //        txtInsuranceAmmount.Text = Comon.ConvertToDecimalPrice(InsurmentRow).ToString("N" + MySession.GlobalPriceDigits); ;
        //        //if (DiscountOnTotal > 0)
        //        //{
        //        //    decimal Total = TotalAfterDiscount - DiscountOnTotal;
        //        //    AdditionalAmount = (Total) / 100 * MySession.GlobalPercentVat;
        //        //    Net = Comon.ConvertToDecimalPrice(Total + AdditionalAmount);
        //        //}



        //        lblAdditionaAmmount.Text = Comon.ConvertToDecimalPrice(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
        //        lblNetBalance.Text = Comon.ConvertToDecimalPrice(Net).ToString("N" + MySession.GlobalPriceDigits);
        //    }

        //    catch (Exception ex)
        //    {
        //        Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        //    }
        //}

        #endregion
        #endregion
        #region Function
        private void ShortcutOpen()
        {
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;

            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                simpleButton11_Click(null, null);
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

            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {

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

                if (gridView2.FocusedColumn.Name == "colItemID" || gridView2.FocusedColumn.Name == "col" + ItemName || gridView2.FocusedColumn.Name == "colBarCode")
                {
                    frmItems frm = new frmItems();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
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
                            gridView2.Columns[ItemName].ColumnEdit = rItem;
                            gridControl.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (gridView2.FocusedColumn.Name == "colSizeName" || gridView2.FocusedColumn.Name == "colSizeID")
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
        }
        private void AddRow()
        {
            try
            {
                if ((gridView2.IsNewItemRow(gridView2.FocusedRowHandle)))
                    gridView2.AddNewRow();
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

            if (FocusedControl.Trim() == txtCustomerID.Name || FocusedControl.Trim() == btnDelivery.Name)
            {
                //txtCustomerName.Visible = false;
                lblCustomerName.Visible = true;
                txtCustomerID.Visible = true;

                if (!MySession.GlobalAllowChangefrmSaleCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                {
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID1", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                    txtCustomerID_Validating(null, null);
                }
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID1", "SublierID ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Store ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtDriverID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDriverID, lblDriverName, "DriverID", "رقم الموصل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDriverID, lblDriverName, "DriverID", "Emp ID", MySession.GlobalBranchID);
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
                if (gridView2.FocusedColumn == null) return;

                if (gridView2.FocusedColumn.Name == "colBarCode" || gridView2.FocusedColumn.Name == "colItemName" || gridView2.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (gridView2.FocusedColumn.Name == "colSizeName" || gridView2.FocusedColumn.Name == "colSizeID")
                {
                    var itemID = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"]);
                    var Barcode = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"]);
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
                
                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
                 else if (FocusedControl == txtDriverID.Name)
                {
                    txtDriverID.Text = cls.PrimaryKeyValue.ToString();
                    txtDriverID_Validating(null, null);
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
                    if (gridView2.FocusedColumn.Name == "colBarCode" || gridView2.FocusedColumn.Name == "colItemName" || gridView2.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        gridView2.AddNewRow();
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), 1);
                        CalculateRow();
                        gridView2.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
                        Find();
                    }
                    else if (gridView2.FocusedColumn.Name == "colSizeName" || gridView2.FocusedColumn.Name == "colSizeID")
                    {

                        int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var itemID = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"]);
                        var Barcode = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"]);
                        if (itemID != null && Barcode != null)
                        {

                            if (Stc_itemsDAL.CheckIfStopItemUnit(Comon.cInt(itemID), SizeID, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                            {
                                Messages.MsgStop(Messages.TitleError, Messages.msgWorningThisUnitIsStop);
                                return;
                            }
                            FileItemData(Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(itemID), SizeID, UserInfo.FacilityID), 1);
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
                IdPrint = false;
                {
                    if (flag)
                        dt = Sales_SaleInvoicesDAL.frmGetDataDetalByRegistrationNo(InvoiceID, UserInfo.BRANCHID, UserInfo.FacilityID);
                    else
                        dt = Sales_SaleInvoicesDAL.frmGetDataDetalByID(InvoiceID, UserInfo.BRANCHID, UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Comon.cInt(dt.Rows[0]["UserID"]) != UserInfo.ID)
                        {
                            Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                            DoNew();
                            return;
                        }
                        IsNewRecord = false;

                        //Validate
                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);

                        txtDriverID.Text = dt.Rows[0]["PateintID"].ToString();
                        txtDriverID_Validating(null, null);



                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        txtAddressID.Text = dt.Rows[0]["IsSendReview"].ToString();
                        txtAddressID_Validating(null, null);
                        StopSomeCode = true;
                        cmbMethodID.EditValue = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());


                        StopSomeCode = false;
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurencyID"].ToString());
                        cmbNetType.EditValue = Comon.cDbl(dt.Rows[0]["NetType"].ToString());
                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);
                        //txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtInsuranceAmmount.Text = "0";
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

                        txtDailyID.Text = dt.Rows[0]["DailyID"].ToString();

                        //Masterdata
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();

                        //Type Order
                        //txtSellerID.Text = dt.Rows[0]["DeliveryID"].ToString();

                        //Date
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

                        //txtCheckID.Text = dt.Rows[0]["CheckID"].ToString();

                        txtNetAmount.Text = dt.Rows[0]["NetAmount"].ToString();
                        txtNetProcessID.Text = dt.Rows[0]["NetProcessID"].ToString();

                        txtVatID.Text = dt.Rows[0]["VatID"].ToString();

                        txtDiscountOnTotal.Text = dt.Rows[0]["DiscountOnTotal"].ToString();

                        //حقول محسوبة 
                        // lblUnitDiscount.Text = "0";
                        lblDiscountTotal.Text = "0";

                        lblInvoiceTotal.Text = dt.Rows[0]["InvoiceTotal"].ToString();
                        txtDiscountOnTotal_Validating(null, null);


                        lblAdditionaAmmount.Text = dt.Rows[0]["AdditionaAmountTotal"].ToString();
                        lblNetBalance.Text = dt.Rows[0]["NetBalance"].ToString();
                        txtInsuranceAmmount.Text = "0";


                        if (Comon.cDbl(lblAdditionaAmmount.Text) > 0)
                            chkForVat.Checked = true;
                        else
                            chkForVat.Checked = false;

                        //GridVeiw

                        gridControl.DataSource = dt;



                        // gridControl1.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;

                        CalculateRow();
                        if (Comon.cInt(dt.Rows[0]["MethodeID"].ToString()) > 1)
                            btnCash_Net_Click(null, null);
                        if (Comon.cLong(dt.Rows[0]["CustomerID"].ToString()) > 1)
                        {
                            btnDelivery_Click(null, null);
                            pnlDeliverContol.Visible = true;
                            txtCustomerID.Visible = true;
                            lblCustomerName.Visible = true;
                        }

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
        //public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
        //    return returnImage;
        //}
        public void GetAccountsDeclaration()
        {
            #region get accounts declaration

            List<Acc_DeclaringMainAccounts> AllAccounts = new List<Acc_DeclaringMainAccounts>();
            int BRANCHID = UserInfo.BRANCHID;
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
                if (Comon.cLong(MySession.GlobalDefaultDebitAccountID) > 0)
                    lblDebitAccountID.Text = MySession.GlobalDefaultDebitAccountID;
                //حساب المبيعات
                DataRow[] row2 = dtDeclaration.Select("DeclareAccountName = 'SalesAccount'");
                if (row2.Length > 0)
                {
                    lblCreditAccountID.Text = row2[0]["AccountID"].ToString();
                    lblCreditAccountName.Text = row2[0]["AccountName"].ToString();
                }
                //حساب الخصم المكتسب
                DataRow[] row3 = dtDeclaration.Select("DeclareAccountName = 'GivenDiscountAccount'");
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
                panelnotes.Visible = false;
                DiscountCustomer = 0;
                txtNotesInvoice.Text = "";
                //txtCustomerName.Text = "";
                txtPaidAmount.Text = "";
                lblRemaindAmount.Text = "";
                txtInsuranceAmmount.Text = "";
                txtDiscountOnTotal.ReadOnly = true;
                txtVatID.Text = "";
                txtDocumentID.Text = "";
                txtCustomerID.Text = "";
                txtDelegateID.Text = "";
                lblCustomerName.Text = "";
                lblDelegateName.Text = "";
                txtNotes.Text = "";
                txtNetAmount.Text = "";
                txtDailyID.Text = "";
                /////////////////////////////
                txtFloor.Text = "";
                txtBuilding.Text = "";
                txtApartment.Text = "";
                txtMobile.Text = "";
                txtAddressID.Text = "";
                txtAddressID_Validating(null, null);
                ///////////////////////////////////////
                txtCustomerID.Tag = " ";
                txtNetProcessID.Tag = " ";
                cmbBank.Tag = " ";
                cmbNetType.Tag = " ";
                txtNetAmount.Tag = " ";
                pnlNetControl.Visible = false;
                pnlDeliverContol.Visible = true;
                //txtCheckID.Tag = " ";
                /////////////////////////////////////////////////
                var dk = Lip.GetServerDate();
                txtInvoiceDate.Text = dk;
                txtWarningDate.Text = dk;
                txtCheckSpendDate.Text = dk;
                checkBox1.Checked = false;
                checkBox2.Checked = true;
                cmbMethodID.ItemIndex = 0;
                txtNotes.Text = "";
                lblDebitAccountID.Text = "";
                lblDebitAccountName.Text = "";

                lblCreditAccountID.Text = "";
                lblCreditAccountName.Text = "";
                lblAdditionalAccountID.Text = "";
                lblAdditionalAccountName.Text = "";
                lblAdditionalAccountID.Text = "";
                lblAdditionalAccountName.Text = "";
                lblInvoiceTotal.Text = "0";
                //lblUnitDiscount.Text = "0";
                lblUnitDiscount.Text = "0";
                lblInvoiceTotalBeforeDiscount.Text = "0";
                txtDiscountOnTotal.Text = "0";
                txtDiscountPercent.Text = "0";
                lblDiscountTotal.Text = "0";
                lblAdditionaAmmount.Text = "0";
                lblNetBalance.Text = "0";
                //picItemUnits.Image = null;

                txtDriverID.Text = "";
                lblDriverName.Text = "";
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

                lblDebitAccountID.Text = MySession.GlobalDefaultDebitAccountID;
                lblDebitAccountID_Validating(null, null);




                if (MySession.GlobalDefaultSalePayMethodID != "0")
                    cmbMethodID.EditValue = Comon.cInt(MySession.GlobalDefaultSalePayMethodID);
                else
                    cmbMethodID.EditValue = 1;

                //txtCustomerName.Visible = false;
                txtCustomerID.Visible = true;
                lblCustomerName.Visible = true;

                if (Comon.cInt(cmbMethodID.EditValue) == 1)
                {
                    //  txtCustomerName.Visible = true;
                    //  txtCustomerName.BringToFront();

                }
                else if (Comon.cInt(cmbMethodID.EditValue) == 2)
                {
                    txtCustomerID.Visible = true;
                    lblCustomerName.Visible = true;
                    txtCustomerID.BringToFront();
                    lblCustomerName.BringToFront();

                }
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultSaleCurencyID);

                lstDetail = new BindingList<Sales_SalesInvoiceDetails>();

                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;

                dt = new DataTable();

                txtInvoiceID.Text = Sales_SaleInvoicesDAL.GetNewID(MySession.GlobalFacilityID, MySession.GlobalBranchID, MySession.UserID).ToString();
                txtDailyID.Text = Sales_SaleInvoicesDAL.GetNewDialyIDPYcLOSEcASHIER(MySession.GlobalFacilityID, MySession.GlobalBranchID, MySession.UserID).ToString();

                //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtInvoiceID.Text;

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
                    strSQL = "SELECT TOP 1 * FROM " + Sales_SaleInvoicesDAL.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_SaleInvoicesDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Sales_SaleInvoicesDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Sales_SaleInvoicesDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Sales_SaleInvoicesDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Sales_SaleInvoicesDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_SaleInvoicesDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Sales_SaleInvoicesDAL();

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
        public void DoNew()
        {
            try
            {
                IsNewRecord = true;
                stopSave = false;
                txtInvoiceID.Text = Sales_SaleInvoicesDAL.GetNewID(MySession.GlobalFacilityID, MySession.GlobalBranchID, MySession.UserID).ToString();

                txtRegistrationNo.Text = "";// RestrictionsDailyDAL.GetNewID(this.Name).ToString();
                IdPrint = false;
                ClearFields();
                txtDailyID.Text = Sales_SaleInvoicesDAL.GetNewDialyIDPYcLOSEcASHIER(MySession.GlobalFacilityID, MySession.GlobalBranchID, MySession.UserID).ToString();
                EnabledControl(true);
                cmbFormPrinting.EditValue = 1;
                gridView2.Focus();
                gridView2.MoveNext();
                gridView2.FocusedColumn = gridView2.VisibleColumns[1];
                gridView2.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
                gridView2.Columns["SalePrice"].OptionsColumn.ReadOnly = true;// !MySession.GlobalCanChangeInvoicePrice;
                gridView2.Columns["Net"].OptionsColumn.ReadOnly = true;
                gridView2.Columns["SalePrice"].OptionsColumn.ReadOnly = true;// !MySession.GlobalCanChangeInvoicePrice;
                gridView2.Columns["SalePrice"].OptionsColumn.AllowEdit = false;
                gridView2.Columns["Net"].OptionsColumn.ReadOnly = true;
                gridView2.Columns["Net"].OptionsColumn.AllowEdit = false;
                gridView2.Columns[SizeName].OptionsColumn.ReadOnly = true;
                gridView2.Columns[SizeName].OptionsColumn.AllowEdit = false;
                gridView2.Columns[ItemName].OptionsColumn.ReadOnly = true;
                gridView2.Columns[ItemName].OptionsColumn.AllowEdit = false;
                gridView2.Columns["QTY"].OptionsColumn.ReadOnly = true;
                gridView2.Columns["QTY"].OptionsColumn.AllowEdit = false;
                gridView2.Columns["BarCode"].OptionsColumn.ReadOnly = true;
                gridView2.Columns["BarCode"].OptionsColumn.AllowEdit = false;
                gridView2.Columns["BarCode"].Visible = false;
                //  gridView2.ShowEditor();
                btnCash_Click(null, null);
                btnLocal_Click(null, null);

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void DoLast()
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
        public void DoFirst()
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
        public void DoNext()
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
        public void DoPrevious()
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
        public void DoSearch()
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

        public void DoEdit()
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
            for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem.Rows[i]["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView2.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView2.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i][ItemName] = gridView2.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i][SizeName] = gridView2.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString()); ;
                dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Bones").ToString());
                dtItem.Rows[i]["Description"] = gridView2.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView2.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount").ToString());
                dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView2.GetRowCellValue(i, "ExpiryDate").ToString());
                dtItem.Rows[i]["ExpiryDate"] = gridView2.GetRowCellValue(i, "ExpiryDate");
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView2.GetRowCellValue(i, "HavVat").ToString());
                dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Total").ToString());
                dtItem.Rows[i]["AdditionalValue"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue").ToString());
                dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net").ToString());
                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
        }
        protected override void DoSave()
        {
            try
            {
                FormAdd = true;
                FormView = true;
                FormUpdate = true;
                lblRemaindAmount.Focus();
                if (Comon.cDec(txtPaidAmount.Text) == 0)
                {
                    txtPaidAmount.Text = lblNetBalance.Text;
                    lblRemaindAmount.Text = "0";
                }


                stopSave = true;
                if (Comon.cInt(txtCustomerID.Text) >= 0)
                {
                    if (!Validations.IsValidForm(this))
                    {
                        stopSave = false;
                        return;
                    }
                    if (!IsValidGrid())
                    {
                        stopSave = false;
                        return;
                    }
                    if (IsNewRecord && !FormAdd)
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                        stopSave = false;
                        return;
                    }
                    else if (!IsNewRecord)
                    {
                        if (!FormUpdate)
                        {
                            Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                            stopSave = false;
                            return;
                        }
                        else
                        {
                            bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, Messages.msgConfirmUpdate);
                            if (!Yes)
                            {
                                stopSave = false;
                                return;

                            }
                        }

                    }

                    Application.DoEvents();
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    if (Comon.ConvertToDecimalPrice(txtNetAmount.Text) <= 0 && Comon.cInt(cmbMethodID.EditValue) == 5)
                    {
                        txtNetAmount.Focus();
                        txtNetAmount.ToolTip = "مبلغ الشبكة = 0 ";
                        Validations.ErrorText(txtNetAmount, txtNetAmount.ToolTip);
                        stopSave = false;
                        return;

                    }
                    if (Comon.ConvertToDecimalPrice(lblNetBalance.Text) < Comon.ConvertToDecimalPrice(txtNetAmount.Text))
                    {
                        txtNetAmount.Focus();
                        txtNetAmount.ToolTip = "مبلغ الشبكة  اكبر من الصافي ";
                        Validations.ErrorText(txtNetAmount, txtNetAmount.ToolTip);
                        stopSave = false;
                        return;
                    }
                    Save();

                }
                else
                {
                    txtCustomerID.Focus();
                    txtCustomerID.ToolTip = "يجب اختيار عميل  ";
                    Validations.ErrorText(txtCustomerID, txtCustomerID.ToolTip);
                    stopSave = false;
                    return;
                }
            }
            catch (Exception ex)
            {

                SplashScreenManager.CloseForm(false);
                stopSave = false;
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void Save()
        {

            gridView2.MoveLastVisible();
            if (DiscountCustomer != 0)
            {
                txtDiscountPercent.Text = DiscountCustomer.ToString();
                txtDiscountPercent_Validating(null, null);
            }
            // CalculateRow();
            gridView2.FocusedColumn = gridView2.VisibleColumns[1];

            var dk = Lip.GetServerDate();
            txtInvoiceDate.Text = dk;
            txtWarningDate.Text = dk;
            txtCheckSpendDate.Text = dk;
            Sales_SalesInvoiceMaster objRecord = new Sales_SalesInvoiceMaster();
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            invoiceNo = Sales_SaleInvoicesDAL.GetNewID(MySession.GlobalBranchID, MySession.GlobalFacilityID, MySession.UserID).ToString();
            txtDailyID.Text = Sales_SaleInvoicesDAL.GetNewDialyIDPYcLOSEcASHIER(MySession.GlobalBranchID, MySession.GlobalFacilityID, MySession.UserID).ToString();
            objRecord.InvoiceID = Comon.cInt(invoiceNo);
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.DailyID = Comon.cInt(txtDailyID.Text);
            objRecord.MethodeID = Comon.cInt(cmbMethodID.EditValue);
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.NetType = Comon.cDbl(cmbNetType.EditValue);
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.CustomerName =  lblCustomerName.Text.Trim();
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SellerID = Comon.cInt(txtSellerID.Text);
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Sale Cashar Invoice" : "فاتوره كاشر مبيعات ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Sale Cashar Invoice" : txtNotesInvoice.Text));
            objRecord.Notes = txtNotes.Text;
            //Account
            objRecord.DebitAccount = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            objRecord.DiscountDebitAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
            objRecord.CheckAccount = Comon.cDbl(lblChequeAccountID.Text);
            objRecord.NetAccount = Comon.cDbl(lblNetAccountID.Text);
            // objRecord.InsuranceAmmount = Comon.cDec(txtInsuranceAmmount.Text);
            objRecord.AdditionalAccount = Comon.cDbl(lblAdditionalAccountID.Text);
            objRecord.NetProcessID = txtNetProcessID.Text;
            objRecord.CheckID = "";// txtCheckID.Text;
            objRecord.VATID = txtVatID.Text;
            objRecord.PateintID = Comon.cInt(txtDriverID.Text);
            //Date
            objRecord.CheckSpendDate = Comon.ConvertDateToSerial(txtCheckSpendDate.Text).ToString();
            objRecord.WarningDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            objRecord.ReceiveDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();

            //Ammount

            objRecord.NetAmount = Comon.cDbl(txtNetAmount.Text);
            objRecord.DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = Math.Abs((Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text)) - Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text));
            objRecord.AdditionaAmountTotal = Comon.ConvertToDecimalPrice(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Math.Abs(Comon.ConvertToDecimalPrice(lblNetBalance.Text) - Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text));

            objRecord.OrderType = OrderType;
            double InsurentForNet = 0;
            if (Comon.cInt(cmbMethodID.EditValue) == 5)
            {
                if (objRecord.NetAmount >= Comon.cDbl(objRecord.NetBalance))
                {

                    objRecord.NetAmount = Comon.cDbl(objRecord.NetBalance);
                    InsurentForNet = Comon.cDbl(txtNetAmount.Text) - Comon.cDbl(objRecord.NetBalance);
                    objRecord.MethodeID = 3;
                    objRecord.DebitAccount = Comon.cDbl(lblNetAccountID.Text);

                }
                else InsurentForNet = 0;



            }


            //Order

            objRecord.IsSendReview = Comon.cInt(txtAddressID.Text);



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

            Sales_SalesInvoiceDetails returned;
            List<Sales_SalesInvoiceDetails> listreturned = new List<Sales_SalesInvoiceDetails>();

            Res_ItemsInsuranceReturn_Details returnedInsur;
            List<Res_ItemsInsuranceReturn_Details> listreturnedInsur = new List<Res_ItemsInsuranceReturn_Details>();
            for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
            {



                {
                    returned = new Sales_SalesInvoiceDetails();
                    returned.ID = i;
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.BarCode = gridView2.GetRowCellValue(i, "BarCode").ToString();
                    returned.ItemID = Comon.cInt(gridView2.GetRowCellValue(i, "ItemID").ToString());
                    returned.SizeID = Comon.cInt(gridView2.GetRowCellValue(i, "SizeID").ToString());
                    returned.QTY = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(i, "QTY").ToString());
                    returned.SalePrice = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString()); ;
                    returned.Bones = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Bones").ToString());
                    returned.Description = gridView2.GetRowCellValue(i, "Description").ToString();
                    returned.StoreID = Comon.cDbl(txtStoreID.Text);
                    returned.Discount = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount").ToString());
                    returned.ItemImage = null;
                    returned.ExpiryDateStr = Comon.ConvertDateToSerial(gridView2.GetRowCellValue(i, "ExpiryDate").ToString().Substring(0, 10));
                    returned.CostPrice = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());
                    returned.AdditionalValue = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue").ToString());
                    returned.Net = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net").ToString());
                    returned.Total = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Total").ToString());
                    returned.DIAMOND_W = Comon.cInt(gridView2.GetRowCellValue(i, "DIAMOND_W").ToString());
                    // returned.extension = gridView2.GetRowCellValue(i, "extension").ToString();
                    // returned.rowhandling = gridView2.GetRowCellValue(i, "rowhandling").ToString();
                    returned.Serials = gridView2.GetRowCellValue(i, "Serials").ToString();
                    if (returned.AdditionalValue == 0)
                        returned.HavVat = false;
                    else
                        returned.HavVat = true;

                    returned.Cancel = 0;
                    returned.Serials = "";
                    if (returned.QTY <= 0 || returned.StoreID <= 0 || returned.SizeID <= 0 || returned.ItemID <= 0)
                        continue;
                    listreturned.Add(returned);
                }

            }

            if (listreturned.Count > 0)
            {
                objRecord.SaleDatails = listreturned;
                string Result = Sales_SaleInvoicesDAL.InsertUsingXML(objRecord, IsNewRecord);
                SplashScreenManager.CloseForm(false);

                if (IsNewRecord == true)
                {

                    if (Comon.cInt(Result) >= 0)
                    {

                        IdPrint = true;
                        invoiceNo = Result;
                         DoSaveIn();
                        IsNewRecord = false;
                        //  Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        try
                        {
                            DoPrint();
                            // DoPrint2();
                        }
                        catch { }

                        DoNew();
                        RefreshGrid();
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                        stopSave = false;
                    }

                }
                else
                {


                    if (Result != "0")
                    {

                        txtInvoiceID_Validating(null, null);
                        EnabledControl(false);
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



        void DoSaveIn()
        {
            try
            {




                Savein();


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
        private void Savein()
        {
            gridView2.MoveLastVisible();
            //CalculateRow();
            gridView2.FocusedColumn = gridView2.VisibleColumns[1];
            txtInvoiceDate_EditValueChanged(null, null);


            Stc_ItemsOutonBail_Master objRecord = new Stc_ItemsOutonBail_Master();
            objRecord.InvoiceID = 0;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;

            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();

            objRecord.CurencyID = Comon.cInt(cmbCurency.EditValue);


           

            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cInt(txtStoreID.Text);

            objRecord.Notes = " فاتورة اخراج مواد لفاتورة مبيعات رقم " + " " + txtInvoiceID.Text;
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);

            //Account
            objRecord.DebitAccount = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);


            objRecord.Total = Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text);


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

            Stc_ItemsOutonBail_Details returned;
            List<Stc_ItemsOutonBail_Details> listreturned = new List<Stc_ItemsOutonBail_Details>();


            for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
            {

                string BarCode = gridView2.GetRowCellValue(i, "BarCode").ToString();


                strSQL = "Select * from Fact_factoryItemDetails where Barcode='" + BarCode + "'";
                DataTable dtfact = new DataTable();
                dtfact = Lip.SelectRecord(strSQL);

                for (int j = 0; j <= dtfact.Rows.Count - 1; j++)
                {

                    returned = new Stc_ItemsOutonBail_Details();
                    returned.ID = j;
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.BarCode = dtfact.Rows[j]["BarcodeMatrial"].ToString();
                    returned.ItemID = Comon.cInt(dtfact.Rows[j]["itemIDMatrial"].ToString());
                    returned.SizeID = Comon.cInt(dtfact.Rows[j]["SizeIDMatriel"].ToString());

                    returned.QTY = Comon.cDec(dtfact.Rows[j]["Qty"].ToString());
                    returned.QTY = returned.QTY + Comon.cDec(dtfact.Rows[j]["Hadr"].ToString());

                    decimal qty = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(i, "QTY").ToString());

                    returned.QTY = returned.QTY * qty;

                    returned.SalePrice = Comon.ConvertToDecimalPrice(dtfact.Rows[j]["SalePrice"].ToString()); ;
                  
                    returned.Bones = 0;
                    returned.Description = gridView2.GetRowCellValue(i, "Description").ToString();
                    returned.StoreID = Comon.cInt(txtStoreID.Text);

                    returned.ExpiryDateStr = 20190101;
                    returned.CostPrice = Comon.ConvertToDecimalPrice(dtfact.Rows[j]["CostPrice"].ToString());
                    returned.Total = returned.CostPrice * returned.QTY;


                    returned.Cancel = 0;
                    returned.Serials = "";
                    if (returned.QTY <= 0 || returned.StoreID <= 0 ||   returned.SizeID <= 0 || returned.ItemID <= 0)
                        continue;
                    listreturned.Add(returned);


                }

              
            }



        }

        private int SaveVouchers(double CreditAmount, double InsurentForNet)
        {

            double AccountID = 0;


            //if (Comon.cInt(cmbMethodID.EditValue) == 5)
            //{
            //    double net = Comon.cDbl(lblNetBalance.Text);//- Comon.cDbl(txtInsurmentAmount1.Text);

            //    if (net - Comon.cDbl(txtNetAmount.Text) >= Comon.cDbl(txtInsuranceAmmount.Text))
            //    {




            //    }
            //    else
            //    {
            //        row = dtDeclaration.Select("DeclareAccountName = 'NetAccount'");
            //        if (row.Length > 0)
            //        {
            //            AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            //        }


            //    }



            //}
            //else
            //{

            //    AccountID = Comon.cDbl(lblDebitAccountID.Text);

            //}
            DataRow[] row;
            AccountID = Comon.cDbl(lblDebitAccountID.Text);
            if (InsurentForNet > 0)
            {

                SaveVouchersForNet(InsurentForNet, InsurentForNet);

            }
            if (CreditAmount - InsurentForNet <= 0) return 0;
            else
                CreditAmount = CreditAmount - InsurentForNet;
            // AccountID = Comon.cDbl(lblCreditAccountID.Text);




            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();

            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());

            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text;
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);


            //Ammount
            // objRecord.TotalCredit = Comon.cDbl(lblTotalCredit.Text);
            // objRecord.TotalDebit = Comon.cDbl(lblTotalDebit.Text);

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

            //if (IsNewRecord == false)
            //{
            //    objRecord.VoucherID = 0;
            //    objRecord.EditUserID = UserInfo.ID;
            //    objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
            //    objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            //    objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            //}



            row = dtDeclaration.Select("DeclareAccountName = 'InsurmentItemsAccount'");
            if (row.Length < 1)
                return 0;


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();



            returned = new Acc_VariousVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            // returned.AccountID = Comon.cDbl(txtCustomerID.Text); ;
            returned.VoucherID = 0;
            returned.Credit = CreditAmount;
            returned.Debit = 0;
            returned.Declaration = "مبلغ-تأمين-" + invoiceNo;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            returned = new Acc_VariousVoucherDetails();
            returned.ID = 1;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            returned.VoucherID = 0;
            returned.Credit = 0;
            returned.Debit = CreditAmount;
            returned.Declaration = "مبلغ-تأمين-" + invoiceNo;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            int Result = 0;
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                long Result1 = VariousVoucherDAL.InsertUsingXML(objRecord, MySession.UserID);
                Result = Comon.cInt(Result1);
                SplashScreenManager.CloseForm(false);



            }
            else
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }
            return Result;
        }

        private int SaveVouchersForNet(double CreditAmount, double InsurentForNet)
        {

            double AccountID = 0;

            DataRow[] row;

            row = dtDeclaration.Select("DeclareAccountName = 'NetAccount'");
            if (row.Length > 0)
            {
                AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            }

            // AccountID = Comon.cDbl(lblCreditAccountID.Text);




            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();

            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());

            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text;
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);


            //Ammount
            // objRecord.TotalCredit = Comon.cDbl(lblTotalCredit.Text);
            // objRecord.TotalDebit = Comon.cDbl(lblTotalDebit.Text);

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

            //if (IsNewRecord == false)
            //{
            //    objRecord.VoucherID = 0;
            //    objRecord.EditUserID = UserInfo.ID;
            //    objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
            //    objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            //    objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            //}



            row = dtDeclaration.Select("DeclareAccountName = 'InsurmentItemsAccount'");
            if (row.Length < 1)
                return 0;


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();



            returned = new Acc_VariousVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            // returned.AccountID = Comon.cDbl(txtCustomerID.Text); ;
            returned.VoucherID = 0;
            returned.Credit = CreditAmount;
            returned.Debit = 0;
            returned.Declaration = "مبلغ-تأمين-" + invoiceNo;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            returned = new Acc_VariousVoucherDetails();
            returned.ID = 1;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            returned.VoucherID = 0;
            returned.Credit = 0;
            returned.Debit = CreditAmount;
            returned.Declaration = "مبلغ-تأمين-" + invoiceNo;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            int Result = 0;
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                long Result1 = VariousVoucherDAL.InsertUsingXML(objRecord, MySession.UserID);
                Result = Comon.cInt(Result1);
                SplashScreenManager.CloseForm(false);



            }
            else
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }
            return Result;
        }
        public void RefreshGrid()
        {


            fillGrid();
            filtering = dt.Copy();
            gridControl2.DataSource = filtering;
            indexGridView_RowClick(null, null);

        }
        public void DoDelete()
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

                Sales_SalesInvoiceMaster model = new Sales_SalesInvoiceMaster();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                string Result = Sales_SaleInvoicesDAL.DeleteSales_SalesInvoiceMaster(model);
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

                    rptForm.Parameters["CustomerName"].Value = "";// txtCustomerName.Text.ToString();
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


                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();

                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["ItemName"] = gridView2.GetRowCellValue(i, "ArbItemName").ToString();
                    row["ExpiryDate"] = gridView2.GetRowCellValue(i, "EngItemName").ToString();
                    //if (Comon.cInt(cmbLanguagePrint.EditValue) == 2)
                    //    row["ItemName"] = gridView2.GetRowCellValue(i, "EngItemName").ToString();
                    //else if (Comon.cInt(cmbLanguagePrint.EditValue) == 3)
                    //    row["ItemName"] = gridView2.GetRowCellValue(i, "EngItemName").ToString() + "                          " + gridView2.GetRowCellValue(i, "ArbItemName").ToString();


                    row["#"] = i + 1;
                    row["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();

                    row["SizeName"] = gridView2.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView2.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView2.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView2.GetRowCellValue(i, "Discount").ToString();
                    row["AdditionalValue"] = gridView2.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView2.GetRowCellValue(i, "Net").ToString();
                    row["SalePrice"] = gridView2.GetRowCellValue(i, "SalePrice").ToString();
                    row["Description"] = "5";
                    row["Bones"] = gridView2.GetRowCellValue(i, "PackingQty").ToString();
                    //row["ExpiryDate"] = Comon.ConvertSerialToDate(Comon.ConvertDateToSerial(gridView2.GetRowCellValue(i, "ExpiryDate").ToString()).ToString());
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
        public void
            DoPrint()
        {
            try
            {
                if (IsNewRecord)
                {
                    if (gridView2.DataRowCount > 0)
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "يوجد فاتورة جديدة - هل تريد متابعة الطباعة على اية حال ؟");
                        if (!Yes)
                            return;
                    }

                    FormView = true;
                    DoLast();
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                //rptForm = "rptCashierPrint";
                ReportName = "rptDeliveryInvoice";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                rptFormName = "rptCashierPrint";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                if (IdPrint == true)
                    rptForm.Parameters["InvoiceID"].Value = invoiceNo;// Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" + 
                else
                    rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString(); //Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" + 
                rptForm.Parameters["StoreName"].Value = UserInfo.SYSUSERARBNAME;
                rptForm.Parameters["DelegateName"].Value = txtDailyID.Text.Trim().ToString();
                var ssr = txtMobile.Text + "  " + lblCustomerName.Text.Trim().ToString();
                var ssr1 = lblAddressCustomerName.Text.Trim().ToString() + "  العمارة :" + txtBuilding.Text.Trim().ToString() + "  الطابق:" + txtApartment.Text.Trim().ToString() + "  الشقة :" + txtFloor.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = txtDailyID.Text;
                rptForm.Parameters["CustomerName"].Value = ssr;
                // rptForm.Parameters["AddressCustomers"].Value = ssr1;
                rptForm.Parameters["MethodName"].Value = MethodName;
                rptForm.Parameters["TheTime"].Value = Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":", "").Trim());
                rptForm.Parameters["CashierName"].Value = OrderTypeArb;
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
                        rptForm.Parameters["NetTotal"].Value = Comon.ConvertToDecimalPrice((lblNetBalance.Text.Trim()).ToString()); break;
                    case (3):
                        rptForm.Parameters["NetTotal"].Value = Comon.ConvertToDecimalPrice(txtNetAmount.Text); break;
                }
                rptForm.Parameters["InvoiceDate"].Value = Lip.GetServerDate();
                //  rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                rptForm.Parameters["Paid"].Value = Comon.ConvertToDecimalPrice(txtPaidAmount.Text.Trim().ToString());
                rptForm.Parameters["CoreTotal"].Value = lblRemaindAmount.Text.Trim().ToString();
                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value = Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text.Trim().ToString());
                rptForm.Parameters["UnitDiscount"].Value = txtApartment.Text;// lblUnitDiscount.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value =  lblDriverName.Text;

                rptForm.Parameters["AdditionalAmount"].Value = Comon.ConvertToDecimalPrice(lblAdditionaAmmount.Text.Trim().ToString());
                rptForm.Parameters["NetBalance"].Value = Comon.ConvertToDecimalPrice(lblNetBalance.Text.Trim().ToString());


                //rptForm.Parameters["Insurance"].Value = txtInsuranceAmmount.Text;
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();
                decimal TransCost = 0;
                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView2.GetRowCellValue(i, ItemName).ToString() + " " + gridView2.GetRowCellValue(i, SizeName).ToString();
                    if (gridView2.GetRowCellValue(i, "Description").ToString() == "0Trans")
                    {
                        TransCost += Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net").ToString());
                        continue;
                    }
                    //if (Comon.cInt(cmbLanguagePrint.EditValue) == 2)
                    //    row["ItemName"] = gridView2.GetRowCellValue(i, "EngItemName").ToString() + gridView2.GetRowCellValue(i, "BarCode").ToString();
                    //else if (Comon.cInt(cmbLanguagePrint.EditValue) == 3)
                    //    row["ItemName"] = gridView2.GetRowCellValue(i, "ArbItemName").ToString() + "-" + gridView2.GetRowCellValue(i, SizeName).ToString() ;
                    //row["ItemName"] = gridView2.GetRowCellValue(i, SizeName).ToString() + "    " + gridView2.GetRowCellValue(i, "ArbItemName").ToString();
                    row["SizeName"] = gridView2.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView2.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView2.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView2.GetRowCellValue(i, "Discount").ToString();
                    row["Net"] = gridView2.GetRowCellValue(i, "Net").ToString();
                    row["SalePrice"] = gridView2.GetRowCellValue(i, "SalePrice").ToString();
                    //row["SalePrice"] = gridView2.GetRowCellValue(i, "BAGET_W").ToString();
                    if (Comon.cDec(row["Total"]) == 0)
                        row["SalePrice"] = 0;

                    dataTable.Rows.Add(row);
                }
                rptForm.Parameters["AdditionalAmount"].Value = Comon.ConvertToDecimalPrice(lblAdditionaAmmount.Text);
                rptForm.Parameters["InvoiceTotal"].Value = Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text.Trim().ToString()) - TransCost - (Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text));
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;
                InvoiceViewModel x = new InvoiceViewModel();
                // معلومات الضريبة الخمسة الأولى
                x.ArbCompanyName = MySession.GlobalFacilityName.ToUpper();
                x.CompanyVatCode = MySession.VAtCompnyGlobal;
                x.InvoiceDate = Comon.cDateTime(txtInvoiceDate.Text + ":" + DateTime.Today.TimeOfDay);
                x.NetTotal = Comon.ConvertToDecimalPrice(lblNetBalance.Text);
                x.VatAmount = Comon.ConvertToDecimalPrice(lblAdditionaAmmount.Text);
                string Base64 = ZATKAQREncryption.ZATCATLVBase64.GetBase64(x.ArbCompanyName, x.CompanyVatCode, x.InvoiceDate, Convert.ToDouble(x.NetTotal), Convert.ToDouble(x.VatAmount));
                rptForm.Parameters["DelegateName"].Value = Base64;
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                /******************** Report Binding ************************/
                /******************** Report Binding ************************/
                //    XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                //   subreport.Visible = false;
                //  subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = false;
                if (ShowReportInReportViewer == true)
                {
                    frmReportViewer frmRptViewer = new frmReportViewer();
                    frmRptViewer.documentViewer1.DocumentSource = rptForm;
                    frmRptViewer.ShowDialog();
                }
                else
                {
                    bool IsSelectedPrinter = false;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + "rptSalesInvoice" + "'");
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
                    rptForm.Dispose();

                }

                dtPrint2 = ReportComponent.SelectRecord("Select * from ItemGroupsPrinters");
                dtPrint = ReportComponent.SelectRecord("Select * from ItemGroupsPrinters");

                for (int i = 0; i <= dtPrint2.Rows.Count - 1; i++)
                    if (dtPrint2.Rows[i]["PrinterName1"].ToString() != "0")
                        PrintToPrintersByItemsGroups(dtPrint2.Rows[i]["PrinterName1"].ToString(), dtPrint);

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            DoNew();
        }
        public void DoPrint2()
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
                ReportName = "rptDeliveryInvoice";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                rptFormName = "rptCashierPrint";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);


                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                if (IdPrint == true)
                    rptForm.Parameters["InvoiceID"].Value = invoiceNo;// Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" +
                else
                    rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();// Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" + 
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = txtDailyID.Text.Trim().ToString();


                var ssr = txtMobile.Text + "  " + lblCustomerName.Text.Trim().ToString();

                var ssr1 = lblAddressCustomerName.Text.Trim().ToString() + "  العمارة :" + txtBuilding.Text.Trim().ToString() + "  الطابق:" + txtApartment.Text.Trim().ToString() + "  الشقة :" + txtFloor.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = DeliveryName;
                rptForm.Parameters["CustomerName"].Value = ssr;
                rptForm.Parameters["AddressCustomers"].Value = ssr1;
                rptForm.Parameters["MethodName"].Value = MethodName;
                rptForm.Parameters["TheTime"].Value = Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":", "").Trim());
                rptForm.Parameters["CashierName"].Value = OrderTypeArb;
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
                rptForm.Parameters["InvoiceDate"].Value = Lip.GetServerDate();
                //  rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                rptForm.Parameters["Paid"].Value = txtPaidAmount.Text.Trim().ToString();
                rptForm.Parameters["CoreTotal"].Value = lblRemaindAmount.Text.Trim().ToString();
                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotalBeforeDiscount.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = "0";// lblUnitDiscount.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = (Comon.ConvertToDecimalPrice(lblDiscountTotal.Text.Trim().ToString())).ToString();
                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["Insurance"].Value = txtInsuranceAmmount.Text;
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();

                decimal TransCost = 0;

                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView2.GetRowCellValue(i, ItemName).ToString() + gridView2.GetRowCellValue(i, "BarCode").ToString() + gridView2.GetRowCellValue(i, SizeName).ToString() + gridView2.GetRowCellValue(i, "PackingQty").ToString();
                    if (gridView2.GetRowCellValue(i, "Description").ToString() == "0Trans")
                    {
                        TransCost += Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net").ToString());
                        continue;
                    }
                    if (Comon.cInt(cmbLanguagePrint.EditValue) == 2)
                        row["ItemName"] = gridView2.GetRowCellValue(i, "EngItemName").ToString() + gridView2.GetRowCellValue(i, "BarCode").ToString();
                    else if (Comon.cInt(cmbLanguagePrint.EditValue) == 3)
                        row["ItemName"] = gridView2.GetRowCellValue(i, "ArbItemName").ToString() + "-" + gridView2.GetRowCellValue(i, SizeName).ToString() + "    " + gridView2.GetRowCellValue(i, "EngItemName").ToString();
                    // row["ItemName"] = gridView2.GetRowCellValue(i, ItemName).ToString() + gridView2.GetRowCellValue(i, "BarCode").ToString() + gridView2.GetRowCellValue(i, SizeName).ToString() + gridView2.GetRowCellValue(i, "PackingQty").ToString();

                    row["ItemName"] = gridView2.GetRowCellValue(i, SizeName).ToString() + "    " + gridView2.GetRowCellValue(i, "ArbItemName").ToString();
                    row["ItemName"] = gridView2.GetRowCellValue(i, "EngSizeName").ToString() + " " + gridView2.GetRowCellValue(i, "EngItemName").ToString() + "\n" + gridView2.GetRowCellValue(i, "ArbSizeName").ToString() + " " + gridView2.GetRowCellValue(i, "ArbItemName").ToString() + "\n";// + gridView2.GetRowCellValue(i, "extension").ToString();
                    row["SizeName"] = gridView2.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView2.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView2.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView2.GetRowCellValue(i, "Discount").ToString();
                    //  row["AdditionalValue"] = gridView2.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView2.GetRowCellValue(i, "Net").ToString();
                    row["SalePrice"] = gridView2.GetRowCellValue(i, "SalePrice").ToString();
                    //  row["Description"] = gridView2.GetRowCellValue(i, "Description").ToString();
                    //   row["Bones"] = gridView2.GetRowCellValue(i, "Bones").ToString();
                    //   row["ExpiryDate"] = Comon.ConvertSerialToDate(Comon.ConvertDateToSerial(gridView2.GetRowCellValue(i, "ExpiryDate").ToString()).ToString());
                    dataTable.Rows.Add(row);
                }
                rptForm.Parameters["DliveryCost"].Value = TransCost;
                rptForm.Parameters["UnderVAT"].Value = (Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text.Trim().ToString())) - (Comon.ConvertToDecimalPrice(lblDiscountTotal.Text.Trim().ToString())) - TransCost - (Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text)); ;
                rptForm.Parameters["AdditionalAmount"].Value = ((Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text.Trim().ToString())) - (Comon.ConvertToDecimalPrice(lblDiscountTotal.Text.Trim().ToString())) - TransCost - (Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text))) * Comon.ConvertToDecimalPrice(0.05);
                rptForm.Parameters["InvoiceTotal"].Value = Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text.Trim().ToString()) - TransCost - (Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text));
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;

                /******************** Report Binding ************************/
                //    XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                //   subreport.Visible = false;
                //  subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();
                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = false;
                if (ShowReportInReportViewer = false)
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
                //if(IsNewRecord==true)
                //SaveVouchers(Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text));
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
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
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
                lblAddressCustomerName.Text = "";
                txtFloor.Text = "";
                txtApartment.Text = "";
                txtBuilding.Text = "";
                txtMobile.Text = "";
                txtAddressID.Text = "";

                string strSql;
                DataTable dt;
                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT * FROM Sales_Customers Where    AccountID =" + txtCustomerID.Text + " or CustomerID=" + txtCustomerID.Text + " or Mobile='" + txtCustomerID.Text + "'";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        txtCustomerID.Text = dt.Rows[0]["AccountID"].ToString();
                        lblCustomerName.Text = dt.Rows[0]["ArbName"].ToString();
                        txtMobile.Text = dt.Rows[0]["Mobile"].ToString();
                        txtApartment.Text = dt.Rows[0]["Address"].ToString();
                        txtAccountID.Text = dt.Rows[0]["AccountID"].ToString();
                        txtFloor.Text = dt.Rows[0]["Email"].ToString();

                        //--------------------------------------
                        //CSearch cls = new CSearch();
                        //cls.AddField("ID", "رقـم العنوان");
                        //cls.SQLStr = "SELECT ID as [رقـم العنوان], ArbName as [العنوان],  Location as [المكان]  FROM Sales_CustomersAddress Where CustomerID=" + txtCustomerID.Text;

                        //int[] ColumnWidth = new int[] { 100, 250, 250, 100, 100 };
                        //PrepareSearchQuery.FindCustomerAddress(ref cls, txtAddressID, lblAddressCustomerName, "CustomerAdress", "رقـم العنوان", MySession.GlobalBranchID, " Where CustomerID = " + txtCustomerID.Text);
                        //if (IsNewRecord)
                        //{
                        //    frmAddressCust = new frmAddressCustomer(txtCustomerID.Text, lblCustomerName.Text);
                        //    frmAddressCust.gridViewAddress.RowClick += gridViewAddress_RowClick;
                        //    frmAddressCust.ShowDialog();
                        //}
                        // كود استدعاء الاسم بدلالة الرقم للعنوان 
                        //--------------------------------------
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
                        lblCustomerName.Text = "";
                        txtCustomerID.Text = "";
                        txtVatID.Text = "";
                    }
                    txtAddressID_Validating(null, null);

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

        private void gridViewAddress_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView view = sender as GridView;

            try
            {

                txtAddressID.Text = view.GetRowCellValue(e.RowHandle, "ID").ToString();

                txtAddressID_Validating(null, null);
                decimal cost = Comon.ConvertToDecimalPrice(view.GetRowCellValue(e.RowHandle, "TransCost").ToString());
                var sr = "SELECT        Stc_ItemUnits.BarCode"
+ " FROM   Stc_ItemUnits LEFT OUTER JOIN"
                     + "    Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID"
+ "   WHERE        (Stc_SizingUnits.Notes = '0')";
                var dt = Lip.SelectRecord(sr);
                if (dt.Rows.Count > 0)
                {


                    btnCilick1(dt.Rows[0][0].ToString(), cost);
                    CalculateRow();




                }


                frmAddressCust.Close();



            }
            catch (Exception ex)
            {
                frmAddressCust.Close();
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
                    decimal TotalUnitDiscount = Comon.ConvertToDecimalPrice(0);
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
                    if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) != Comon.ConvertToDecimalPrice(Math.Round(((percent * whole) / 100))))
                    {
                        txtDiscountOnTotal.Text = ((percent * whole) / 100).ToString("N" + MySession.GlobalPriceDigits);

                        decimal DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                        decimal UnitDiscount = Comon.ConvertToDecimalPrice(0);
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

                gridView2.Focus();
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
            if (e.KeyCode == Keys.F3)
            {
                txtCustomerID.Focus();
                Validations.ErrorTextClear(txtCustomerID, txtCustomerID.ToolTip);
                frm = new XtraForm2();
                frm.simpleButton2.Click += searchResut_Click;
                frm.ShowDialog();
            }
            else if (e.KeyCode == Keys.F2)
            {
                txtCustomerID.Focus();
                ShortcutOpen();
            }
            else if (e.KeyCode == Keys.F7)
                txtPaidAmount.Focus();
            else if (e.KeyCode == Keys.F9)
                DoSave();

            else if (e.KeyCode == Keys.F8)
                btnNew_Click(null, null);


        }

        /*******************Event CheckBoc***************************/
        private void chForVat_EditValueChanged(object sender, EventArgs e)
        {

            decimal Total = 0;
            decimal Net = 0;
            decimal DiscountTotal = 0;
            decimal DiscountOnTotal = 0;
            decimal AdditionalAmount = 0;


            decimal DiscountRow = 0;
            decimal QTYRow = 0;
            decimal CostPriceRow = 0;
            decimal TotalRow = 0;
            decimal NetRow = 0;
            decimal AdditionalAmountRow = 0;

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

            for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
            {

                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem.Rows[i]["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView2.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView2.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i][ItemName] = gridView2.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i][SizeName] = gridView2.GetRowCellValue(i, SizeName).ToString();

                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString()); ;
                dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Bones").ToString());
                dtItem.Rows[i]["Description"] = gridView2.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView2.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount").ToString());
                dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView2.GetRowCellValue(i, "ExpiryDate").ToString());
                dtItem.Rows[i]["ExpiryDate"] = gridView2.GetRowCellValue(i, "ExpiryDate");
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView2.GetRowCellValue(i, "HavVat").ToString());
                dtItem.Rows[i]["HavVat"] = chkForVat.Checked;
                dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Total").ToString());
                dtItem.Rows[i]["Cancel"] = 0;
                CostPriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                QTYRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                DiscountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount").ToString());
                TotalRow = CostPriceRow * QTYRow;

                if (chkForVat.Checked == true)
                {

                    AdditionalAmountRow = (TotalRow - DiscountRow) / 100 * MySession.GlobalPercentVat;
                    NetRow = Comon.ConvertToDecimalPrice((TotalRow - DiscountRow) + AdditionalAmountRow);
                    dtItem.Rows[i]["AdditionalValue"] = AdditionalAmountRow.ToString("N" + MySession.GlobalPriceDigits);
                    dtItem.Rows[i]["Net"] = NetRow.ToString("N" + MySession.GlobalPriceDigits);

                    AdditionalAmount += AdditionalAmountRow;
                    DiscountTotal += DiscountRow;
                    Total += TotalRow;
                    Net += NetRow;


                }
                else
                {
                    AdditionalAmountRow = 0;
                    NetRow = TotalRow - DiscountRow;
                    dtItem.Rows[i]["AdditionalValue"] = 0;
                    dtItem.Rows[i]["Net"] = NetRow.ToString("N" + MySession.GlobalPriceDigits);

                    AdditionalAmountRow = 0;
                    DiscountTotal += DiscountRow;
                    Total += TotalRow;
                    Net += NetRow;
                }


            }
            DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
            lblAdditionaAmmount.Text = AdditionalAmount.ToString("N" + MySession.GlobalPriceDigits);
            lblNetBalance.Text = Net.ToString("N" + MySession.GlobalPriceDigits);

            gridView2.Columns["HavVat"].OptionsColumn.ReadOnly = !chkForVat.Checked;


            gridControl.DataSource = dtItem;

            // CalculateRow();

            //gridView2.Focus();
            //gridView2.FocusedColumn = gridView2.VisibleColumns[0];
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
                //lblCheckID.Visible = false;
                //txtCheckID.Visible = false;
                txtNetProcessID.Text = "";
                //txtCheckID.Text = "";
                txtNetAmount.Text = "";

                cmbNetType.ItemIndex = -1;
                txtWarningDate.EditValue = DateTime.Now;
                txtCheckSpendDate.EditValue = DateTime.Now;
                txtNetAmount.Visible = false;
                lblNetAmount.Visible = false;
                lblnetType.Visible = false;
                cmbNetType.Visible = false;
                txtCustomerID.Tag = "IsNumber";
                //txtCheckID.Tag = "IsNumber";
                cmbBank.Tag = " ";
                txtNetProcessID.Tag = "IsNumber";
                txtNetAmount.Tag = "IsNumber";
                if (value == 1)
                {
                    // حساب الصندوق
                    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
                    if (row.Length > 0)
                    {

                        lblDebitAccountID.Text = row[0]["AccountID"].ToString();
                        lblDebitAccountName.Text = row[0]["AccountName"].ToString();

                    }
                    if (Comon.cLong(MySession.GlobalDefaultDebitAccountID) > 0)
                        lblDebitAccountID.Text = MySession.GlobalDefaultDebitAccountID;
                    //txtCustomerName.BringToFront();
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    // txtCustomerName.Focus();
                }
                else if (value == 2)
                {
                    txtCustomerID.Visible = true;
                    lblCustomerName.Visible = true;
                    //txtCustomerName.Visible = false;
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
                    txtCustomerID.Tag = "ImportantFieldGreaterThanZero";
                }
                else if (value == 3)
                {
                    // حساب الشبكة 
                    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'NetAccount'");
                    if (row.Length > 0)
                    {
                        lblDebitAccountID.Text = row[0]["AccountID"].ToString();
                        lblDebitAccountName.Text = row[0]["AccountName"].ToString();

                    }

                    lblCheckSpendDate.Visible = false;
                    txtCheckSpendDate.Visible = false;
                    //lblCheckID.Visible = false;
                    //txtCheckID.Visible = false;
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;

                    lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = false;
                    lblNetAmount.Visible = false;
                    lblnetType.Visible = true;
                    cmbNetType.Visible = true;
                    cmbNetType.ReadOnly = false;
                    cmbNetType.EditValue = Comon.cDbl(MySession.GlobalDefaultSaleNetTypeID);// Comon.cDbl(lblDebitAccountID.Text);
                    //  txtNetProcessID.Tag = "ImportantFieldGreaterThanZero";
                    //  txtNetAmount.Tag = "ImportantFieldGreaterThanZero";
                    cmbNetType.Tag = "ImportantField";
                }
                else if (value == 4)
                {
                    // حساب الشيكات
                    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'ChequeAccount'");
                    if (row.Length > 0)
                    {
                        lblDebitAccountID.Text = row[0]["AccountID"].ToString();
                        lblDebitAccountName.Text = row[0]["AccountName"].ToString();

                    }

                    lblNetProcessID.Visible = false;
                    txtNetProcessID.Visible = false;
                    txtNetAmount.Visible = false;
                    lblNetAmount.Visible = false;
                    lblnetType.Visible = false;
                    cmbNetType.Visible = false;

                    lblCheckSpendDate.Visible = true;
                    txtCheckSpendDate.Visible = true;
                    //lblCheckID.Visible = true;
                    //txtCheckID.Visible = true;
                    lblBankName.Visible = true;
                    cmbBank.Visible = true;
                    cmbBank.Tag = "ImportantField";

                    cmbBank.EditValue = Comon.cDbl(lblDebitAccountID.Text);
                }
                else if (value == 5)
                {
                    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
                    if (row.Length > 0)
                    {

                        lblDebitAccountID.Text = row[0]["AccountID"].ToString();
                        lblDebitAccountName.Text = row[0]["AccountName"].ToString();
                        chkForVat.Checked = true;
                    }
                    if (Comon.cLong(MySession.GlobalDefaultDebitAccountID) > 0)
                        lblDebitAccountID.Text = MySession.GlobalDefaultDebitAccountID;
                    lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = true;
                    lblNetAmount.Visible = true;
                    lblnetType.Visible = true;
                    cmbNetType.Visible = true;
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    cmbNetType.EditValue = Comon.cDbl(MySession.GlobalDefaultSaleNetTypeID);
                    txtNetProcessID.Tag = "ImportantFieldGreaterThanZero";
                    txtNetAmount.Tag = "ImportantFieldGreaterThanZero";
                    cmbNetType.Tag = "ImportantField";
                }


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
            //btnChequeSearch.Enabled = MySession.GlobalAllowChangefrmSaleChequeAccountID;
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
            //ribbonControl1.Visible = false;
        }
        #endregion
        #region Numbers
        private void btnDelivery_Click(object sender, EventArgs e)
        {
            OrderType = btnDelivery.Text;
            // Find();
            pnlDeliverContol.Visible = true;
            btnDelivery.Appearance.BorderColor = Color.FromArgb(83, 68, 63);//LightSeaGreen
            btnLocal.Appearance.BorderColor = Color.Green;
            btnHangerStation.Appearance.BorderColor = Color.White;
            btnTakeAway.Appearance.BorderColor = Color.FromArgb(255, 128, 0); ;
            // 255, 128, 0 ,LightSeaGreen,Green
            btnDelivery.BackColor = Color.LightYellow;
            btnDelivery.ForeColor = Color.Black;
            btnLocal.BackColor = Color.Transparent;
            btnLocal.ForeColor = Color.Black;
            OrderType = "3";
            RefreshOffers();
            OrderTypeArb = "توصيل";
            OrderTypeEng = "Delivery";
            lblOrderType.Text = OrderTypeArb;

            txtDriverID.Focus();
            Find();

        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            DoPrint();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DoSave();

        }
        private void btnCash_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            //  txtCustomerID.Text = " ";
            txtNetProcessID.Text = " ";
            cmbBank.Text = " ";
            // cmbNetType.Text = " ";
            txtNetAmount.Text = " ";
            btnCash.Appearance.BorderColor = Color.FromArgb(83, 68, 63);
            btnNet.Appearance.BorderColor = Color.Transparent;
            btnCash_Net.Appearance.BorderColor = Color.Transparent;
            //  txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            //simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //   showCustomers(false, 0);
            //  pnlDeliverContol.Visible = false;
            pnlNetControl.Visible = false;
            //txtCustomerName.Visible = true;
            labelControl6.Visible = true;
            txtVatID.Visible = true;
            labelControl4.Visible = true;
            cmbMethodID.EditValue = 1;
            cmbMethodID_EditValueChanged(null, null);
            //btnSix.Appearance.BackColor = Color.Goldenrod;
            //btnSix.Appearance.BackColor2 = Color.White;
            //btnSix.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            //btnSix.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "نقدا" : "Cash");
            MethodID = 1;

            //btnNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView2.Focus();
            gridView2.MoveLastVisible();
            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView2.FocusedColumn = gridView2.VisibleColumns[1];

        }

        private void btnNet_Click(object sender, EventArgs e)
        {
            /////////////////////////////


            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            //    txtCustomerID.Text = " ";
            txtNetProcessID.Text = " ";
            cmbBank.Text = " ";
            //cmbNetType.Text = " ";
            txtNetAmount.Text = " ";
            cmbNetType.EditValue = 0;
            //   cmbNetType.Tag = "ImportantField";
            // cmbNetType.EditValue = Comon.cDbl("12020000003");
            //  pnlDeliverContol.Visible = false;
            pnlNetControl.Visible = true;
            //  txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            //simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //showCustomers(false, 0);
            cmbMethodID.EditValue = 3;
            cmbMethodID_EditValueChanged(null, null);
            btnCash.Appearance.BorderColor = Color.Transparent;
            btnNet.Appearance.BorderColor = Color.FromArgb(83, 68, 63);
            btnCash_Net.Appearance.BorderColor = Color.Transparent;

            //btnNet.Appearance.BackColor = Color.Goldenrod;
            //btnNet.Appearance.BackColor2 = Color.White;
            //btnNet.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            //btnNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة" : "Net");
            MethodID = 2;
            btnSix.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView2.Focus();
            gridView2.MoveLastVisible();
            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView2.FocusedColumn = gridView2.VisibleColumns[1];

        }

        private void btnCash_Net_Click(object sender, EventArgs e)
        {
            /////////////////////////////


            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            // txtCustomerID.Text = " ";
            txtNetProcessID.Text = " ";
            cmbBank.Text = " ";
            // cmbNetType.Text = " ";
            txtNetAmount.Text = " ";
            txtNetAmount.Tag = "IsNumber";

            // cmbNetType.Tag = "ImportantField";
            //   cmbNetType.EditValue = Comon.cLong(12020000003);
            //  txtCheckID.Tag = " ";
            // pnlDeliverContol.Visible = false;
            pnlNetControl.Visible = true;
            /////////////////////////////////////////////////
            showCustomers(false, 0);
            cmbMethodID.EditValue = 5;
            cmbMethodID_EditValueChanged(null, null);
            //btnCash_Net.Appearance.BackColor = Color.Goldenrod;
            //btnCash_Net.Appearance.BackColor2 = Color.White;
            //btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            //btnCash_Net.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            //btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            btnCash.Appearance.BorderColor = Color.Transparent;
            btnNet.Appearance.BorderColor = Color.Transparent;
            btnCash_Net.Appearance.BorderColor = Color.FromArgb(83, 68, 63);
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة/ نقد" : "Net/Cash");
            MethodID = 3;

            //simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //btnSix.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //btnNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView2.Focus();
            gridView2.MoveLastVisible();
            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView2.FocusedColumn = gridView2.VisibleColumns[1];
            txtNetAmount.Tag = "ImportantField";

        }
        private void btnPlus_Click(object sender, EventArgs e)
        {
            try
            {
                decimal value;
                FocusedControl = GetLastIndexFocusedControl();
                if (FocusedControl == null) return;
                // if (FocusedControl.Trim() == gridControl.Name)
                if (1 == 1)
                {
                    if (gridView2.FocusedColumn == null) return;
                    var obj = gridView2.GetFocusedValue();
                    if (obj == null)
                    {
                        if (gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0)
                            return;
                        gridView2.SetFocusedValue(Comon.ConvertToDecimalQty(1.ToString("N" + MySession.GlobalPriceDigits)));
                        var dr = dtPriceItemOffers.Select("((FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " )AND(FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " )) or((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " ))OR ((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " ) ");

                        if (gridView2.GetFocusedRowCellValue("Description").Equals("IsPercent"))
                        {

                            decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["Height"]).ToString()));
                            decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                            gridView2.SetFocusedRowCellValue(gridView2.Columns["Discount"], total);

                        }

                        GetNewOffers(dr, gridView2.GetFocusedRowCellValue(gridView2.Columns["BarCode"]).ToString(), Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["QTY"]).ToString()));



                        CalculateRow();
                    }
                    else
                    {
                        if (gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0)
                            return;
                        value = Comon.ConvertToDecimalQty(strQty);
                        if (value == 0)
                            value = 1;
                        decimal QtyValue = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY").ToString());
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], Comon.ConvertToDecimalQty(value.ToString("N" + MySession.GlobalPriceDigits)));

                        var dr = dtPriceItemOffers.Select("((FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " )AND(FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " )) or((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " ))OR ((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " ) ");

                        if (gridView2.GetFocusedRowCellValue("Description").Equals("IsPercent"))
                        {

                            decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["Height"]).ToString()));
                            decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["SalePrice"])) * value * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                            gridView2.SetFocusedRowCellValue(gridView2.Columns["Discount"], total);

                        }

                        GetNewOffers(dr, gridView2.GetFocusedRowCellValue(gridView2.Columns["BarCode"]).ToString(), value);






                        CalculateRow();
                    }
                }
                strQty = "";
                strQty = "";
                //txtTotal.Text = "";
                simpleButton1_Click_2(null, null);
            }
            catch { };
        }
        private void btnMinus_Click(object sender, EventArgs e)
        {
            decimal value;
            FocusedControl = GetLastIndexFocusedControl();
            if (FocusedControl == null) return;

            if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView2.FocusedColumn == null) return;
                var obj = gridView2.GetFocusedValue();
                if (obj == null)
                {
                    gridView2.SetFocusedValue(Comon.ConvertToDecimalQty(0.ToString("N" + MySession.GlobalPriceDigits)));
                    CalculateRow();
                }
                else
                {
                    value = Comon.ConvertToDecimalQty(strQty);
                    if (value == 0)
                        value = 1;
                    decimal QtyValue = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY").ToString());
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], Comon.ConvertToDecimalQty(1.ToString("N" + MySession.GlobalPriceDigits)));
                    CalculateRow();

                }
            }
            strQty = "";
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            strQty = strQty + "9";
            setValueToField("9");
        }
        private void btnEight_Click(object sender, EventArgs e)
        {
            strQty = strQty + "8";
        }
        private void btnSeven_Click(object sender, EventArgs e)
        {
            strQty = strQty + "7";
        }
        private void btnThree_Click(object sender, EventArgs e)
        {
            strQty = strQty + "3";

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
        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            removeValueToField();
        }
        void setFocus()
        {
            gridView2.FocusedRowHandle = thePreviousActiveRow;
            gridView2.SelectRow(thePreviousActiveRow);
            gridView2.Focus();
        }
        private void txtPaidAmount_Leave(object sender, EventArgs e)
        {
            setActiveControl(sender as Control);
        }
        private void txtPaidAmount_Enter(object sender, EventArgs e)
        {
            theActiveControl = sender as Control;
        }
        private void setActiveColumn(GridColumn ActiveEditor)
        {
            theActiveColumn = ActiveEditor;
        }
        private void setActiveRowHandle(int ActiveEditor)
        {
            thePreviousActiveRow = theActiveRow;
            theActiveRow = ActiveEditor;
            if (thePreviousActiveRow == null)
                thePreviousActiveRow = theActiveRow;

        }
        private void setActiveControl(Control ActiveEditor)
        {
            thePreviousControl = ActiveEditor;
            theActiveControl = ActiveEditor;
            if (thePreviousControl == null)
                thePreviousControl = theActiveControl;

        }
        string GetLastIndexFocusedControl()
        {
            Control c = thePreviousControl;
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
        void setValueToField(string value)
        {
            //string text = "";
            //FocusedControl = GetLastIndexFocusedControl();
            //if (FocusedControl == null) return;

            // if (FocusedControl.Trim() == gridControl.Name)
            //{

            //    //if (gridView2.FocusedColumn == null) return;

            //    //decimal  QTY = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY").ToString());
            //    // {

            //    //     decimal T = Comon.ConvertToDecimalQty(strQty) + QTY;

            //    //    CalculateRow();
            //    //} 

            //}
        }
        void removeValueToField()
        {
            string value = "";
            FocusedControl = GetLastIndexFocusedControl();
            if (FocusedControl == null) return;

            if (FocusedControl.Trim() == gridControl.Name)
            {

                CalculateRow();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if ((gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0) || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER1") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER0") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER2"))
                    return;

                checkIfExist(gridView2.GetFocusedRowCellValue("BarCode").ToString(), 1);
                gridView2.DeleteRow(gridView2.FocusedRowHandle);
                gridView2.MoveLast();
                CalculateRow();
            }
            catch { }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            DoNew();
            btnCash_Click(null, null);
        }
        #endregion
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

                gridView2.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }
        private void frmCashierSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
                DoSave();
            else if (e.KeyCode == Keys.F6)
                btnCash_Click(null, null);
            else if (e.KeyCode == Keys.F7)
                btnCash_Net_Click(null, null);
            else if (e.KeyCode == Keys.F8)
                btnNew_Click(null, null);
            else if (e.KeyCode == Keys.F1)
            {
                FocusedControl = GetIndexFocusedControl();
                if (FocusedControl == txtCustomerID.Name)
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
            // txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            showCustomers(true, 1);
            cmbMethodID.EditValue = 2;
            //simpleButton12.Appearance.BackColor = Color.Goldenrod;
            // simpleButton12.Appearance.BackColor2 = Color.White;
            //  simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            //  simpleButton12.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            //  simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "آجل" : "Future");
            MethodID = 4;


            btnSix.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            btnNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView2.Focus();
            gridView2.MoveLastVisible();
            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView2.FocusedColumn = gridView2.VisibleColumns[1];
        }
        private void showCustomers(bool p, int f)
        {

            //txtCustomerName.Text = "";
            //txtCustomerID.Text = "";
            //lblCustomerName.Text = "";

            //txtVatID.Text = "";
            ////txtCustomerName.Visible = false;

            //txtCustomerID.Visible = p;
            //lblCustomerName.Visible = p;
            //labelControl6.Visible = p;
            //txtCustomerID.BringToFront();
            //lblCustomerName.BringToFront();
            //labelControl4.BringToFront();

            //labelControl4.Visible = p;
            //txtVatID.Visible = p;

            //labelControl6.Visible = p;
            if (f == 1)
            {


            }


        }
        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {


        }
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkEdit1.Checked == true)
            //{
            //    groupBox1.Visible = true;
            //    //gridControl.Width = gridControl.Width - groupBox1.Width;
            //    //gridControl.Location = new Point(-241, gridControl.Location.Y);
            //}
            //else
            //{
            //    groupBox1.Visible = false;
            //    //gridControl.Width = gridControl.Width + groupBox1.Width;
            //    //gridControl.Location = new Point(1, gridControl.Location.Y);
            //}
        }

        private void btnTakeAway_Click(object sender, EventArgs e)
        {
            OrderType = btnTakeAway.Text;
            pnlDeliverContol.Visible = true;
            btnDelivery.Appearance.BorderColor = Color.LightSeaGreen;//LightSeaGreen
            btnLocal.Appearance.BorderColor = Color.Green;
            btnHangerStation.Appearance.BorderColor = Color.White;
            btnTakeAway.Appearance.BorderColor = Color.FromArgb(83, 68, 63); //(255, 128, 0)
            btnTakeAway.BackColor = Color.LightYellow;
            btnLocal.BackColor = Color.Transparent;
            OrderType = "2";
            RefreshOffers();
            OrderTypeArb = "سـفري";
            OrderTypeEng = "Takeaway";
            lblOrderType.Text = OrderTypeArb;
            txtDriverID.Text = "";
            lblDriverName.Text = "";
        }

        private void btnLocal_Click(object sender, EventArgs e)
        {
            OrderType = "1";
            btnLocal.BackColor = Color.LightYellow;//'LightYellow
            btnLocal.ForeColor = Color.Black;
            btnTakeAway.BackColor = Color.Transparent;
            btnTakeAway.BackColor = Color.Transparent;
            btnDelivery.Appearance.BorderColor = Color.LightSeaGreen;//LightSeaGreen
            btnLocal.Appearance.BorderColor = Color.FromArgb(83, 68, 63);
            btnHangerStation.Appearance.BorderColor = Color.White;
            btnTakeAway.Appearance.BorderColor = Color.FromArgb(255, 128, 0); //(255, 128, 0)
            pnlDeliverContol.Visible = true;
            RefreshOffers();
            OrderTypeArb = "محلي";
            OrderTypeEng = "Local";

            lblOrderType.Text = OrderTypeArb;
            txtDriverID.Text = "";
            lblDriverName.Text = "";

        }

        private void btnHangerStation_Click(object sender, EventArgs e)
        {
            btnHangerStation.BackColor = btnPlus.BackColor;
            btnHangerStation.BackColor = Color.LightSteelBlue;
            btnHangerStation.BackColor = btnPlus.BackColor;
            btnHangerStation.BackColor = btnPlus.BackColor;
            btnDelivery.Appearance.BorderColor = Color.LightSeaGreen;//LightSeaGreen
            btnLocal.Appearance.BorderColor = Color.Green;
            btnHangerStation.Appearance.BorderColor = Color.FromArgb(83, 68, 63);
            btnTakeAway.Appearance.BorderColor = Color.FromArgb(255, 128, 0); //(255, 128, 0)
            pnlDeliverContol.Visible = true;
            OrderType = "4";
            RefreshOffers();
            OrderTypeArb = "هنجر ستيشن";
            OrderTypeEng = "Hanger Station";
            lblOrderType.Text = OrderTypeArb;

            txtDriverID.Text = "";
            lblDriverName.Text = "";

        }



        private void txtAddressID_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                var sr = "SELECT Sales_Customers.CustomerID, Sales_Customers.ArbName, Sales_Customers.Tel, Sales_Customers.Mobile, Sales_CustomersAddress.ID, Sales_CustomersAddress.Location, Sales_CustomersAddress.Street, "
                    + " Sales_CustomersAddress.Building, Sales_CustomersAddress.ArbName as Notes, Sales_CustomersAddress.Floor, Sales_CustomersAddress.Apartment, HR_District.ArbName AS DistrictName, HR_Street.ArbName AS StreetName, HR_District.TransCost"
                    + "  FROM    HR_District RIGHT OUTER JOIN"
                    + " Sales_CustomersAddress ON HR_District.ID = Sales_CustomersAddress.Location LEFT OUTER JOIN"
                    + " HR_Street ON Sales_CustomersAddress.Street = HR_Street.ID RIGHT OUTER JOIN"
                    + " Sales_Customers ON Sales_CustomersAddress.CustomerID = Sales_Customers.CustomerID  where Sales_Customers.Cancel=0 And  Sales_CustomersAddress.ID=" + Comon.cInt(txtAddressID.Text.Trim());
                var dr = Lip.SelectRecord(sr);
                if (dr.Rows.Count > 0)
                {
                    decimal cost = Comon.ConvertToDecimalPrice(dr.Rows[0]["TransCost"].ToString());
                    lblAddressCustomerName.Text = dr.Rows[0]["DistrictName"].ToString() + "-" + dr.Rows[0]["StreetName"].ToString() + "-" + dr.Rows[0]["Notes"].ToString();
                    txtCustomerID.Text = dr.Rows[0]["CustomerID"].ToString();
                    lblCustomerName.Text = dr.Rows[0]["ArbName"].ToString();
                    txtFloor.Text = dr.Rows[0]["Floor"].ToString();
                    txtApartment.Text = dr.Rows[0]["Apartment"].ToString();
                    txtBuilding.Text = dr.Rows[0]["Building"].ToString();
                    txtMobile.Text = "Tel:" + dr.Rows[0]["Tel"].ToString() + "-Mob:" + dr.Rows[0]["Mobile"].ToString();
                    var sr1 = "SELECT        Stc_ItemUnits.BarCode"
                    + " FROM   Stc_ItemUnits LEFT OUTER JOIN"
                    + "    Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID"
                    + "   WHERE        (Stc_SizingUnits.Notes = '0')";
                    var dt = Lip.SelectRecord(sr1);
                    if (dt.Rows.Count > 0)
                    {
                        btnCilick1(dt.Rows[0][0].ToString(), cost);
                        CalculateRow();
                    }

                }
                else
                {

                }
                CalculateRow();
                // strSQL = "SELECT CONCAT(HR_District.ArbName,' - ',Sales_CustomersAddress.ArbName )  as AddressName FROM Sales_CustomersAddress inner join HR_District on Sales_CustomersAddress.Location =HR_District.ID  WHERE Sales_CustomersAddress.ID =" + Comon.cInt(txtAddressID.Text) + " And Sales_CustomersAddress.Cancel =0 ";
                // CSearch.ControlValidating(txtAddressID, lblAddressCustomerName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void PrintToPrintersByItemsGroups(string PrinterName, DataTable dtPrinterGroups)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtTest;
                bool IncludeHeader = true;
                ReportName = "rptSplitResturantInvoiceByItemsGroups";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                rptFormName = "rptSplitResturantInvoiceByItemsGroupsArb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                var dataTable = new DataTable();
                dataTable.Columns.Add("BarCode", System.Type.GetType("System.String"));
                dataTable.Columns.Add("ItemName", System.Type.GetType("System.String"));
                dataTable.Columns.Add("UnitName", System.Type.GetType("System.String"));
                dataTable.Columns.Add("Qty", System.Type.GetType("System.Decimal"));
                dataTable.Columns.Add("Total", System.Type.GetType("System.Decimal"));
                for (var x = 0; x <= dtPrinterGroups.Rows.Count - 1; x++)
                {
                    if (PrinterName == dtPrinterGroups.Rows[x]["PrinterName1"].ToString())
                    {
                        for (var i = 0; i <= gridView2.DataRowCount - 1; i++)
                        {
                            int lItemID = Comon.cInt(gridView2.GetRowCellValue(i, "ItemID").ToString());
                            int GroupID = Comon.cInt(gridView2.GetRowCellValue(i, "Caliber").ToString());
                            //strSQL = "Select Top 1 GroupID From Stc_Items Where ItemID=" + lItemID;
                            //dtTest = Lip.SelectRecord(strSQL);
                            //if (dtTest.Rows.Count > 0)
                            //{
                            // حقل ال ReportName هنا هو نفسه حقل ال GroupID
                            if (GroupID == Comon.cInt(dtPrinterGroups.Rows[x]["ReportName"].ToString()))
                            {
                                if (gridView2.GetRowCellValue(i, "Description").ToString() == "0Trans")
                                {
                                    continue;
                                }
                                var row = dataTable.NewRow();
                                row["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                                row["ItemName"] = gridView2.GetRowCellValue(i, "ArbItemName").ToString() + " " + gridView2.GetRowCellValue(i, "ArbSizeName").ToString();// + gridView2.GetRowCellValue(i, "extension").ToString();
                                //row["ItemName"] = gridView2.GetRowCellValue(i, SizeName).ToString() + " " + gridView2.GetRowCellValue(i, ItemName).ToString();
                                row["UnitName"] = gridView2.GetRowCellValue(i, SizeName).ToString();
                                row["Qty"] = gridView2.GetRowCellValue(i, "QTY").ToString();
                                row["Total"] = gridView2.GetRowCellValue(i, "Total").ToString();
                                dataTable.Rows.Add(row);
                            }
                            //}
                        }
                    }
                }

                if (dataTable.Rows.Count > 0)
                {
                    rptForm.Parameters["InvoiceID"].Value = invoiceNo;// Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" + invoiceNo;
                    //رقم الطاولة
                    rptForm.Parameters["VatID"].Value = " ";
                    rptForm.Parameters["StoreName"].Value = txtDailyID.Text;
                    rptForm.Parameters["InvoiceDate"].Value = Lip.GetServerDate() + "-" + Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":", "").Trim());
                    rptForm.Parameters["CashierName"].Value = OrderTypeArb;
                    rptForm.Parameters["CustomerName"].Value = txtMobile.Text + "  " + lblCustomerName.Text.Trim().ToString();
                    rptForm.Parameters["CostCenterName"].Value = txtDailyID.Text;
                    rptForm.Parameters["CustomerName"].Value = txtNotesInvoice.Text;

                    rptForm.DataSource = dataTable;
                    rptForm.DataMember = ReportName;
                    rptForm.ShowPrintStatusDialog = false;
                    rptForm.ShowPrintMarginsWarning = false;
                    rptForm.CreateDocument();
                    bool IsSelectedPrinter = false;
                    if (!string.IsNullOrEmpty(PrinterName))
                    {
                        rptForm.PrinterName = PrinterName;
                        rptForm.Print(PrinterName);
                        IsSelectedPrinter = true;
                        rptForm.Dispose();

                    }

                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }




        private void layoutView1_Click(object sender, EventArgs e)
        {
            //long ID = Comon.cLong(layoutView1.GetFocusedRowCellValue("ItemID").ToString());
            //frmSize = new frmSizeItem(ID);
            //frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;

            //frmSize.ShowDialog();
        }

        private void gridControl2_ClientSizeChanged(object sender, EventArgs e)
        {

        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

        private void layoutView1_Click_1(object sender, EventArgs e)
        {
            //LayoutView item = (sender as LayoutView);//.GetFocusedRow() as ActionItem;
            //if (item.GetFocusedDataRow() != null)
            //{
            //    long ID = Comon.cLong(layoutView1.GetFocusedRowCellValue("ItemID").ToString());
            //    frmSize = new frmSizeItem(ID);
            //    frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;

            //    frmSize.ShowDialog();
            //}
        }

        private void layoutView1_Click_1(object sender, CardClickEventArgs e)
        {
            //LayoutView item = (sender as LayoutView);//.GetFocusedRow() as ActionItem;
            //if (item.GetFocusedDataRow() != null)
            //{
            //    long ID = Comon.cLong(layoutView1.GetFocusedRowCellValue("ItemID").ToString());
            //    frmSize = new frmSizeItem(ID);
            //    frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;

            //    frmSize.ShowDialog();
            //}
        }

        private void layoutViewCard1_Click(object sender, EventArgs e)
        {
            //LayoutView item = (sender as LayoutView);//.GetFocusedRow() as ActionItem;
            //if (item.GetFocusedDataRow() != null)
            //{
            //    long ID = Comon.cLong(layoutView1.GetFocusedRowCellValue("ItemID").ToString());
            //    frmSize = new frmSizeItem(ID);
            //    frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;

            //    frmSize.ShowDialog();
            //}
        }
        private bool checkifOffers(long p)
        {
            var dt = dtSpecialOffers.Select("ItemID=" + p.ToString());
            if (dt.Length < 1) return false;
            return true;
        }
        void layoutView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (layoutView1.RowCount < 1)
                    return;
                var hiTinfo = layoutView1.CalcHitInfo(e.Location);
                if (hiTinfo.InFieldValue)
                {
                    if (hiTinfo.Column.FieldName == "ItemImage" || hiTinfo.Column.FieldName == "ItemName" || hiTinfo.Column.FieldName == "RemainQty" || hiTinfo.Column.FieldName == "SalePrice")
                    {
                        long ID = Comon.cLong(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "ItemID").ToString());
                        int typeID = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "TypeID").ToString());

                        if (typeID == 8 && !checkifOffers(ID))
                            frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;

                        FlyoutAction action = new FlyoutAction();

                        FlyoutProperties properties = new FlyoutProperties();
                        properties.Style = FlyoutStyle.Popup;
                        Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + dt.Rows[0]["BarCode"].ToString() + "'," + Comon.cInt(txtStoreID.Text) + ") AS RemainQty")).Rows[0]["RemainQty"].ToString()).ToString("N" + MySession.GlobalPriceDigits);
                        var srSize = "   SELECT dbo.RemindQtyStock(BarCode, " + Comon.cInt(txtStoreID.Text) + "," + Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString() + ") AS RemainQty  ," + layoutView1.GetRowCellValue(hiTinfo.RowHandle, "RemainQty").ToString() + " as RemainQtyParent,0 as PackingQtyParent "
                      + ",  Stc_Items.ArbName as ItemNAme,   Stc_Items.ItemImage  ,  Stc_ItemUnits.BarCode,Stc_ItemUnits.PackingQty, Stc_ItemUnits.SalePrice, Stc_SizingUnits.ArbName , Stc_ItemsGroups.Notes "
                 + " FROM            Stc_ItemUnits LEFT OUTER JOIN"
                  + "              Stc_Items ON Stc_ItemUnits.ItemID = Stc_Items.ItemID  LEFT OUTER JOIN "
                          + "              Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID   LEFT OUTER JOIN   Stc_ItemsGroups   ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID  "
                + " WHERE        (Stc_ItemUnits.ItemID =" + ID + ")  and Stc_ItemUnits.unitCancel=0   order by Stc_ItemUnits.PackingQty Asc ";

                        //if (Comon.ConvertToDecimalQty(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "RemainQty").ToString()) <= 0 && MySession.GlobalWayOfOutItems != "AllowOutItemsWithOutBalance")
                        //{

                        //    Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
                        //    return;
                        //}
                        var dtSize = Lip.SelectRecord(srSize);
                        if (dtSize.Rows.Count < 1)
                            return;
                        else if (dtSize.Rows.Count == 1)
                        {

                            if (Comon.ConvertToDecimalQty(dtSize.Rows[0]["RemainQty"].ToString()) >= 0 || MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
                            {
                                btnCilick(dtSize.Rows[0]["BarCode"].ToString(), 1);
                                CalculateRow();

                            }
                            else
                            {

                                Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
                            }

                            return;
                        }
                        frmSize = new frmSizeItem(dtSize);
                        frmSize.layoutView1.MouseDown += layoutViewItem_MouseDown;
                        // frmSize.Location.X = gridControl2.Location.X;
                        frmSize.Location = new Point(gridControl2.Location.X, gridControl2.Location.Y + 100);
                        //  frmSize.DesktopLocation = new Point(1, 1);

                        //  FlyoutDialog.Show(this, frmSize, action, properties);




                        frmSize.ShowDialog();

                    }


                }
            }
            catch { }
        }




        void layoutViewItem_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                LayoutView view = sender as LayoutView;
                var hiTinfo = view.CalcHitInfo(e.Location);
                if (hiTinfo.InFieldValue)
                {
                    if (hiTinfo.Column.FieldName == "ItemImage" || hiTinfo.Column.FieldName == "ArbName" || hiTinfo.Column.FieldName == "RemainQty" || hiTinfo.Column.FieldName == "SalePrice")
                    {

                        if (Comon.ConvertToDecimalQty(view.GetRowCellValue(hiTinfo.RowHandle, "RemainQtyParent").ToString()) >= Comon.ConvertToDecimalQty(Comon.ConvertToDecimalQty(view.GetRowCellValue(hiTinfo.RowHandle, "PackingQty").ToString()) / Comon.ConvertToDecimalQty(view.GetRowCellValue(hiTinfo.RowHandle, "PackingQtyParent").ToString())) || MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
                        {
                            string ID = view.GetRowCellValue(hiTinfo.RowHandle, "BarCode").ToString();
                            btnCilick(ID, 1);
                            CalculateRow();
                            frmSize.Close();

                        }
                        else
                        {

                            Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
                        }

                        return;


                    }
                }
            }
            catch { }

        }
        private void layoutView1_CustomFieldValueStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewFieldValueStyleEventArgs e)
        {
            //  return;  // Painting the content of the focused card only if the LayoutView itself has the focus.
            ColumnView view = sender as ColumnView;
            if (view == null) return;
            // if(view.get)
            decimal count = Comon.ConvertToDecimalQty(view.GetRowCellValue(e.RowHandle, "RemainQty").ToString());
            if (MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
            {

                e.Appearance.BackColor = Color.FromArgb(27, 96, 147);
                e.Appearance.BackColor2 = Color.FromArgb(27, 96, 147);
                e.Appearance.ForeColor = Color.Yellow;
                e.Appearance.Options.UseFont = true;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                return;


            }
            if (count > 0)
            {

                e.Appearance.BackColor = Color.FromArgb(27, 96, 147);
                e.Appearance.BackColor2 = Color.FromArgb(27, 96, 147);
                e.Appearance.ForeColor = Color.Yellow;
                e.Appearance.Options.UseFont = true;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
            else
            {


                e.Appearance.BackColor = Color.FromArgb(253, 101, 0);
                e.Appearance.BackColor2 = Color.FromArgb(253, 101, 0);
                e.Appearance.ForeColor = Color.White;
                e.Appearance.Options.UseFont = true;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

            }


        }
        void btnCilick(string Barcode, decimal QtyInput)
        {

            try
            {

                int flag = 0;
                gridView2.PostEditor();

                gridView2.AddNewRow();
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], Barcode);
                var itemGroup = FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), QtyInput);


                decimal QtyIn = 0;
                CalculateRow();
                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                //for (int i = 0; i < gridView2.RowCount - 1; ++i)
                //{
                //    if (i == rowIndex)
                //        if (gridView2.IsNewItemRow(rowIndex))
                //            continue;
                //        else
                //        {
                //            if (gridView2.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                //            {
                //                QtyIn = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"]));
                //                if (gridView2.GetRowCellValue(i, "extension").ToString() == "" && gridView2.GetRowCellValue(i, "Caliber").ToString() != "-1" && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER1") && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER0") && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER2") && Comon.cInt(gridView2.GetRowCellValue(i, "DIAMOND_W").ToString()) != 1)
                //                {

                //                    if (gridView2.IsNewItemRow(rowIndex))
                //                        gridView2.DeleteRow(rowIndex);
                //                    QtyIn = QtyIn + 1;
                //                }
                //                else continue;
                //                gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], QtyIn);

                //                if (gridView2.GetRowCellValue(i, "Description").Equals("IsPercent"))
                //                {
                //                    decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["Height"]).ToString()));
                //                    decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                //                    gridView2.SetRowCellValue(i, gridView2.Columns["Discount"], total);
                //                    gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //                    gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                //                    flag = 1;
                //                    GetNewOffers(itemGroup, Barcode, QtyIn);
                //                    return;
                //                }

                //                else
                //                {

                //                    gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //                    gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                //                    flag = 1;
                //                    GetNewOffers(itemGroup, Barcode, QtyIn);
                //                    return;


                //                }
                //            }
                //        }


                //    if (gridView2.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                //    {
                //        QtyIn = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"]));
                //        if (gridView2.GetRowCellValue(i, "extension").ToString() == "" && gridView2.GetRowCellValue(i, "Caliber").ToString() != "-1" && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER1") && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER0") && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER2") && Comon.cInt(gridView2.GetRowCellValue(i, "DIAMOND_W").ToString()) != 1)
                //        {

                //            if (gridView2.IsNewItemRow(rowIndex))
                //                gridView2.DeleteRow(rowIndex);
                //            QtyIn = QtyIn + 1;
                //        }
                //        else continue;
                //        gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], QtyIn);
                //        if (gridView2.GetRowCellValue(i, "Description").Equals("IsPercent"))
                //        {
                //            decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["Height"]).ToString()));
                //            decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                //            gridView2.SetRowCellValue(i, gridView2.Columns["Discount"], total);
                //            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //            gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                //            flag = 1;
                //            GetNewOffers(itemGroup, Barcode, QtyIn);
                //            return;
                //        }

                //        else
                //        {

                //            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //            gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                //            flag = 1;
                //            GetNewOffers(itemGroup, Barcode, QtyIn);
                //            return;


                //        }

                //    }



                //}

                //GetNewOffers(itemGroup, Barcode, QtyInput);

                //if (flag == 1)
                //    return;
                //gridView2.AddNewRow();
                //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], ItemBarCode);
                //FileItemData(Stc_itemsDAL.GetItemData1(ItemBarCode, UserInfo.FacilityID));
                //CalculateRow();
                //gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //gridView2.FocusedColumn = gridView2.VisibleColumns[0];

            }
            catch
            {



            }
        }


        void GetNewOffers(DataRow[] itemGroup, string Barcode, decimal QtyIn)
        {




            if (itemGroup != null && itemGroup.Length > 0)
            {


                if (Comon.cInt(itemGroup[0]["IsOffers"].ToString()) > 0)
                {

                    decimal QtyOffers;
                    if (Comon.cInt(itemGroup[0]["IsTakeOne"].ToString()) > 0 && !checkIfExist(Barcode, 1))
                    {
                        AddnewItem(Barcode, Comon.ConvertToDecimalPrice(1), "ISOFFER0");

                    }
                    else if (Comon.cInt(itemGroup[0]["IsGetSame"].ToString()) > 0 && !checkIfExist(Barcode, 1))
                    {

                        QtyOffers = Comon.ConvertToDecimalPrice(itemGroup[0]["GetSameAmount"].ToString());
                        if (QtyIn >= QtyOffers)
                            AddnewItem(Barcode, Comon.ConvertToDecimalPrice(itemGroup[0]["SetSameAmount"].ToString()), "ISOFFER1");


                    }

                    else if (Comon.cInt(itemGroup[0]["IsGetOnther"].ToString()) > 0 && !checkIfExist(itemGroup[0]["BarCode"].ToString(), 1))
                    {
                        QtyOffers = Comon.ConvertToDecimalPrice(itemGroup[0]["GetOntherAmount"].ToString());
                        if (QtyIn >= QtyOffers)
                            AddnewItem(itemGroup[0]["BarCode"].ToString(), Comon.ConvertToDecimalPrice(itemGroup[0]["SetOntherAmount"].ToString()), "ISOFFER2");

                    }



                }







            }









        }
        bool checkIfExist(string Barcode, decimal QtyInput)
        {
            string descrption = "";
            //  bool descrption = false;
            for (int i = 0; i < gridView2.RowCount - 1; ++i)
            {
                if (i == rowIndex)
                    if (gridView2.IsNewItemRow(rowIndex))
                        continue;
                    else
                    {
                        if (gridView2.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                        {

                            switch (gridView2.GetRowCellValue(i, "Description").ToString())
                            {

                                case ("ISOFFER1"): gridView2.DeleteRow(i); return false;
                                case ("ISOFFER0"): gridView2.DeleteRow(i); return false;
                                case ("ISOFFER2"): gridView2.DeleteRow(i); return false;
                            }




                        }
                    }


                if (gridView2.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                {

                    switch (gridView2.GetRowCellValue(i, "Description").ToString())
                    {

                        case ("ISOFFER1"): gridView2.DeleteRow(i); return false;
                        case ("ISOFFER0"): gridView2.DeleteRow(i); return false;
                        case ("ISOFFER2"): gridView2.DeleteRow(i); return false;
                    }



                }



            }

            return false;




        }

        void AddnewItem(string Barcode, decimal QtyInput, string description)
        {

            try
            {

                int flag = 0;
                CalculateRow();
                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                gridView2.PostEditor();
                gridView2.AddNewRow();
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], Barcode);
                FileItemDataOffers(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), QtyInput, description);
                CalculateRow();
                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                flag = 1;
                if (flag == 1)
                    return;
                //gridView2.AddNewRow();
                //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], ItemBarCode);
                //FileItemData(Stc_itemsDAL.GetItemData1(ItemBarCode, UserInfo.FacilityID));
                //CalculateRow();
                //gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //gridView2.FocusedColumn = gridView2.VisibleColumns[0];

            }
            catch
            {



            }
        }









        void btnCilick1(string Barcode, decimal QtyInput)
        {

            try
            {

                int flag = 0;
                gridView2.PostEditor();

                gridView2.AddNewRow();
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], Barcode);
                FileItemData1(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID), QtyInput);
                CalculateRow();
                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                for (int i = 0; i < gridView2.RowCount - 1; ++i)
                {
                    if (i == rowIndex)
                        if (gridView2.IsNewItemRow(rowIndex))
                            continue;
                        else
                        {
                            if (gridView2.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                            {
                                if (gridView2.IsNewItemRow(rowIndex))
                                    gridView2.DeleteRow(rowIndex);
                                gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])));
                                gridView2.SetRowCellValue(i, gridView2.Columns["SalePrice"], QtyInput);
                                //   if (gridView2.IsNewItemRow(gridView2.FocusedRowHandle))
                                // gridView2.DeleteRow(gridView2.FocusedRowHandle);

                                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                                flag = 1;
                                return;
                            }
                        }


                    if (gridView2.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                    {

                        gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])));
                        gridView2.SetRowCellValue(i, gridView2.Columns["SalePrice"], QtyInput);
                        if (gridView2.IsNewItemRow(rowIndex))
                            gridView2.DeleteRow(rowIndex);

                        gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                        gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                        flag = 1;
                        return;
                    }



                }
                if (flag == 1)
                    return;
                //gridView2.AddNewRow();
                //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], ItemBarCode);
                //FileItemData(Stc_itemsDAL.GetItemData1(ItemBarCode, UserInfo.FacilityID));
                //CalculateRow();
                //gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //gridView2.FocusedColumn = gridView2.VisibleColumns[0];

            }
            catch
            {



            }
        }

        #region ListItem Function
        void ListItemInit()
        {

            txtInvoiceDate.ReadOnly = true;
            //this.gprevious.Click += new System.EventHandler(this.gprevious_Click);
            //this.ipervious.Click += new System.EventHandler(this.ipervious_Click);
            //this.gnext.Click += new System.EventHandler(this.gnext_Click);
            //this.inext.Click += new System.EventHandler(this.inext_Click);

            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            this.btnOne.Click += new System.EventHandler(this.btnOne_Click);
            this.btnTwo.Click += new System.EventHandler(this.btnTow_Click);
            this.btnThree.Click += new System.EventHandler(this.btnThree_Click);
            this.btnFour.Click += new System.EventHandler(this.btnFour_Click);
            this.btnFive.Click += new System.EventHandler(this.btnFive_Click);
            this.btnSix.Click += new System.EventHandler(this.btnSix_Click);
            this.btnSeven.Click += new System.EventHandler(this.btnSeven_Click);
            this.btnEight.Click += new System.EventHandler(this.btnEight_Click);
            this.btnNine.Click += new System.EventHandler(this.btnNine_Click);
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //this.btnBackSpace.Click += new System.EventHandler(this.btnBackSpace_Click);
            //this.btnPendOrder.Click += new System.EventHandler(this.btnPendOrder_Click);
            //this.btnGetPendingOrder.Click += new System.EventHandler(this.btnGetPendingOrder_Click);
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            this.btnCash.Click += new System.EventHandler(this.btnCash_Click);
            this.btnNet.Click += new System.EventHandler(this.btnNet_Click);
            this.btnCash_Net.Click += new System.EventHandler(this.btnCash_Net_Click);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.txtPaidAmount.Leave += new System.EventHandler(this.txtPaidAmount_Leave);
            this.txtPaidAmount.Enter += new System.EventHandler(this.txtPaidAmount_Enter);
            this.txtCustomerID.Leave += new System.EventHandler(this.txtPaidAmount_Leave);
            this.txtCustomerID.Enter += new System.EventHandler(this.txtPaidAmount_Enter);
            this.txtNetProcessID.Leave += new System.EventHandler(this.txtPaidAmount_Leave);
            this.txtNetProcessID.Enter += new System.EventHandler(this.txtPaidAmount_Enter);
            this.txtNetAmount.Leave += new System.EventHandler(this.txtPaidAmount_Leave);
            this.txtNetAmount.Enter += new System.EventHandler(this.txtPaidAmount_Enter);
            this.txtInvoiceID.Leave += new System.EventHandler(this.txtPaidAmount_Leave);
            this.txtInvoiceID.Enter += new System.EventHandler(this.txtPaidAmount_Enter);
            this.txtDiscountPercent.Leave += new System.EventHandler(this.txtPaidAmount_Leave);
            this.txtDiscountPercent.Enter += new System.EventHandler(this.txtPaidAmount_Enter);
            this.txtDiscountOnTotal.Leave += new System.EventHandler(this.txtPaidAmount_Leave);
            this.txtDiscountOnTotal.Enter += new System.EventHandler(this.txtPaidAmount_Enter);
            this.btnDelivery.Click += new System.EventHandler(this.btnDelivery_Click);
            this.btnTakeAway.Click += new System.EventHandler(this.btnTakeAway_Click);
            this.btnLocal.Click += new System.EventHandler(this.btnLocal_Click);
            this.btnHangerStation.Click += new System.EventHandler(this.btnHangerStation_Click);

            //ArrbtnItemGroups[0] = group1;
            //ArrbtnItemGroups[1] = group2;
            //ArrbtnItemGroups[2] = group3;
            //ArrbtnItemGroups[3] = group4;
            //ArrbtnItemGroups[4] = group5;
            //ArrbtnItemGroups[5] = group6;
            //ArrbtnItemGroups[6] = group7;
            //ArrbtnItemGroups[7] = group8;
            //ArrbtnItemGroups[8] = group9;
            //ArrbtnItemGroups[9] = group10;

            //ArrbtnItems[0] = item1;
            //ArrbtnItems[1] = item2;
            //ArrbtnItems[2] = item3;
            //ArrbtnItems[3] = item4;
            //ArrbtnItems[4] = item5;
            //ArrbtnItems[5] = item6;
            //ArrbtnItems[6] = item7;
            //ArrbtnItems[7] = item8;
            //ArrbtnItems[8] = item9;
            //ArrbtnItems[9] = item10;
            //ArrbtnItems[10] = item11;
            //ArrbtnItems[11] = item12;
            //ArrbtnItems[12] = item13;
            //ArrbtnItems[13] = item14;
            //ArrbtnItems[14] = item15;
            //ArrbtnItems[15] = item16;
            //ArrbtnItems[16] = item17;
            //ArrbtnItems[17] = item18;
            //ArrbtnItems[18] = item19;
            //ArrbtnItems[19] = item20;

            //ButtonItemGroupEvent(ArrbtnItemGroups, SizeItemGroupPage);
            //ButtonItemEvent(ArrbtnItems, SizeItemPage);

            //dtGroups = Lip.SelectRecord("SELECT  [GroupID] , [ArbName], [EngName] FROM Stc_ItemsGroups WHERE Cancel=0");
            //CountItemGroupPage = getCountPage(dtGroups.Rows.Count, SizeItemGroupPage);
            //gprevious_Click(null, null);
            //btnItemGroup_Click(ArrbtnItemGroups[0], null);

            indexGridView.Columns[0].AppearanceCell.BackColor = Color.CadetBlue;


        }
        #region ListItem Function Event
        private void btnGetPendingOrder_Click(object sender, EventArgs e)
        {

            if (HangingOrder.Count > 0)
            {
                btnNew_Click(null, null);
                gridControl.DataSource = HangingOrder.DataSource;

            }

        }
        private void btnPendOrder_Click(object sender, EventArgs e)
        {
            int length = gridView2.RowCount;
            if (length > 0)
            {
                HangingOrder.DataSource = gridControl.DataSource;
                btnNew_Click(null, null);
            }
        }
        private void gprevious_Click(object sender, EventArgs e)
        {

            HideButton(ArrbtnItemGroups, SizeItemGroupPage);
            IndexItemGroupPage = getPreviousIndexPage(IndexItemGroupPage);
            int length = getLength(dtGroups.Rows.Count, SizeItemGroupPage, IndexItemGroupPage);
            for (int i = 0; i < length; i++)
            {
                ArrbtnItemGroups[i].Name = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["GroupID"].ToString();
                ArrbtnItemGroups[i].Tag = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["EngName"].ToString();
                ArrbtnItemGroups[i].Text = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ArbName"].ToString();
                ArrbtnItemGroups[i].Visible = true;
            }
        }
        private void gnext_Click(object sender, EventArgs e)
        {

            HideButton(ArrbtnItemGroups, SizeItemGroupPage);
            IndexItemGroupPage = getNextIndexPage(CountItemGroupPage, IndexItemGroupPage);
            int length = getLength(dtGroups.Rows.Count, SizeItemGroupPage, IndexItemGroupPage);
            for (int i = 0; i < length; i++)
            {
                ArrbtnItemGroups[i].Name = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["GroupID"].ToString();
                ArrbtnItemGroups[i].Tag = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["EngName"].ToString();
                ArrbtnItemGroups[i].Text = dtGroups.Rows[(SizeItemGroupPage * IndexItemGroupPage) + i]["ArbName"].ToString();
                ArrbtnItemGroups[i].Visible = true;
            }

        }
        void ButtonItemGroupEvent(SimpleButton[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
                arr[i].Click += new System.EventHandler(this.btnItemGroup_Click);
        }
        private void btnItemGroup_Click(object sender, EventArgs e)
        {
            IndexItemPage = 0;
            string ItemGroupID = ((SimpleButton)sender).Name;
            string ItemGroupArbName = ((SimpleButton)sender).Text;
            string ItemGroupEngName = ((SimpleButton)sender).Tag.ToString();

            string SQl = @"SELECT        dbo.Stc_ItemUnits.BarCode, dbo.Stc_Items.ItemID, dbo.Stc_Items.ArbName, dbo.Stc_Items.EngName ,dbo.Stc_Items.ItemImageName
                          FROM           dbo.Stc_Items INNER JOIN
                                         dbo.Stc_ItemUnits ON dbo.Stc_Items.ItemID = dbo.Stc_ItemUnits.ItemID
                          WHERE          (Cancel=0 AND dbo.Stc_Items.GroupID = '" + ItemGroupID + "')";

            dtItems = Lip.SelectRecord(SQl);
            CountItemPage = getCountPage(dtItems.Rows.Count, SizeItemPage);
            ipervious_Click(null, null);
        }
        string ReSizeImage(string ImageName)
        {
            //string PathFile = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            //string ImagePath = PathFile + @"\Images\ItemsImages\";
            //if (Imager.ImageIsFound(ImagePath, ImageName))
            //{
            //    string ResizeImage = "Resize_" + Path.GetFileNameWithoutExtension(ImageName) + ".jpg";
            //    if (Imager.ImageIsFound(ImagePath, ResizeImage))
            //        return ImagePath + ResizeImage;

            //    Imager.PerformImageResize(ImagePath, ImageName, Convert.ToInt16(70), Convert.ToInt16(70), ResizeImage);
            //    return ImagePath + ResizeImage;
            //}
            return " ";
        }
        string getPathImage()
        {
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            Path = Path + @"\Images\";
            return Path;
        }
        /*****************************************************/
        private void ipervious_Click(object sender, EventArgs e)
        {


            IndexItemPage = getPreviousIndexPage(IndexItemPage);
            HideButton(ArrbtnItems, SizeItemPage);
            int length = getLength(dtItems.Rows.Count, SizeItemPage, IndexItemPage);
            for (int i = 0; i < length; i++)
            {
                ArrbtnItems[i].Name = dtItems.Rows[(SizeItemPage * IndexItemPage) + i]["BarCode"].ToString();
                ArrbtnItems[i].Tag = dtItems.Rows[(SizeItemPage * IndexItemPage) + i]["EngName"].ToString();
                ArrbtnItems[i].Text = dtItems.Rows[(SizeItemPage * IndexItemPage) + i]["ArbName"].ToString();
                ArrbtnItems[i].ImageOptions.ImageUri = ReSizeImage(dtItems.Rows[(SizeItemPage * IndexItemPage) + i]["ItemImageName"].ToString());
                ArrbtnItems[i].Visible = true;
            }
        }
        private void inext_Click(object sender, EventArgs e)
        {
            IndexItemPage = getNextIndexPage(CountItemPage, IndexItemPage);
            HideButton(ArrbtnItems, SizeItemPage);
            int length = getLength(dtItems.Rows.Count, SizeItemPage, IndexItemPage);
            for (int i = 0; i < length; i++)
            {
                ArrbtnItems[i].Name = dtItems.Rows[(SizeItemPage * IndexItemPage) + i]["BarCode"].ToString();
                ArrbtnItems[i].Tag = dtItems.Rows[(SizeItemPage * IndexItemPage) + i]["EngName"].ToString();
                ArrbtnItems[i].Text = dtItems.Rows[(SizeItemPage * IndexItemPage) + i]["ArbName"].ToString();
                ArrbtnItems[i].ImageOptions.ImageUri = ReSizeImage(dtItems.Rows[(SizeItemPage * IndexItemPage) + i]["ItemImageName"].ToString());
                ArrbtnItems[i].Visible = true;
            }
        }
        void ButtonItemEvent(SimpleButton[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
                arr[i].Click += new System.EventHandler(this.btnItem_Click);
        }
        private void btnItem_Click(object sender, EventArgs e)
        {
            try
            {
                string ItemBarCode = ((SimpleButton)sender).Name;
                string ItemArbName = ((SimpleButton)sender).Text;
                string ItemEngName = ((SimpleButton)sender).Tag.ToString();
                int flag = 0;
                gridView2.PostEditor();
                //if (gridView2.RowCount==1)
                //{

                //    gridView2.AddNewRow();
                //    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], ItemBarCode);
                //    FileItemData(Stc_itemsDAL.GetItemData1(ItemBarCode, UserInfo.FacilityID));
                //    CalculateRow();
                //    gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //    gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                //    return;
                //}
                gridView2.AddNewRow();
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], ItemBarCode);
                FileItemData(Stc_itemsDAL.GetItemData1(ItemBarCode, UserInfo.FacilityID), 1);
                CalculateRow();
                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                for (int i = 0; i < gridView2.RowCount - 1; ++i)
                {
                    if (i == rowIndex)
                        if (gridView2.IsNewItemRow(rowIndex))
                            continue;
                        else
                        {
                            if (gridView2.GetRowCellValue(i, "BarCode").Equals(ItemBarCode.ToString()))
                            {
                                if (gridView2.IsNewItemRow(rowIndex))
                                    gridView2.DeleteRow(rowIndex);
                                gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) + 1);
                                //   if (gridView2.IsNewItemRow(gridView2.FocusedRowHandle))
                                // gridView2.DeleteRow(gridView2.FocusedRowHandle);

                                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                                flag = 1;
                                return;
                            }
                        }


                    if (gridView2.GetRowCellValue(i, "BarCode").Equals(ItemBarCode.ToString()))
                    {

                        gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) + 1);
                        if (gridView2.IsNewItemRow(rowIndex))
                            gridView2.DeleteRow(rowIndex);

                        gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                        gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                        flag = 1;
                        return;
                    }



                }
                if (flag == 1)
                    return;
                //gridView2.AddNewRow();
                //gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], ItemBarCode);
                //FileItemData(Stc_itemsDAL.GetItemData1(ItemBarCode, UserInfo.FacilityID));
                //CalculateRow();
                //gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                //gridView2.FocusedColumn = gridView2.VisibleColumns[0];

            }
            catch
            {



            }
        }
        /*****************************************************/
        int getCountPage(int Count, int SizePage)
        {
            return Count / SizePage;
        }
        int getPreviousIndexPage(int IndexPage)
        {
            IndexPage = IndexPage - 1;
            if (IndexPage < 1)
                IndexPage = 0;
            return IndexPage;
        }
        int getNextIndexPage(int CountPage, int IndexPage)
        {
            IndexPage = IndexPage + 1;
            if (IndexPage >= CountPage)
                IndexPage = CountPage;
            return IndexPage;
        }
        int getLength(int Count, int SizePage, int indexPage)
        {
            int remain = Count - (SizePage * indexPage);
            if (remain > SizePage)
                return SizePage;
            else
                return remain;
        }
        void HideButton(SimpleButton[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
                arr[i].Visible = false;
        }
        #endregion
        private void simpleButton1_Click_2(object sender, EventArgs e)
        {
            panelnotes.Visible = false;
            if (pnlCalcuate.Visible == false)
                pnlCalcuate.Visible = true;
            else
                pnlCalcuate.Visible = false;
        }
        #endregion

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void XtraForm22_LocationChanged(object sender, EventArgs e)
        {
            //this.Location = new Point(0, 0);
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {

            indexGridView.MovePrevPage();
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            indexGridView.MoveNextPage();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            nextPage = 0;
            try
            {
                var filter = indexGridView.GetFocusedRowCellValue("GroupID").ToString();
                filtering = dtFillGrid.Copy();
                gridControl2.DataSource = null;
                if (filtering.Rows.Count > 0)
                {
                    DataRow dr;
                    for (int i = 0; i <= filtering.Rows.Count - 1; ++i)
                    {

                        if (DBNull.Value != filtering.Rows[i]["GroupID"] || !string.IsNullOrEmpty(filtering.Rows[i]["GroupID"].ToString()))
                        {
                            dr = filtering.Rows[i];

                            if (dr["GroupID"].ToString().Trim() != filter)
                                dr.Delete();
                        }



                    }


                    filtering.AcceptChanges();
                    if (filtering.Rows.Count < 1)
                    {
                        DataRow dr1;
                        dr1 = filtering.NewRow();
                        dr1["GroupID"] = 0;
                        dr1["ItemID"] = 0;
                        dr1["ItemName"] = "------";
                        dr1["ItemImage"] = null;
                        filtering.Rows.Add(dr1);
                    }
                    gridControl2.DataSource = filtering;
                }
            }
            catch { }
        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {

            if (filtering.Rows.Count <= 12)
                return;
            nextPage = +12;
            try
            {
                var filter = indexGridView.GetFocusedRowCellValue("GroupID").ToString();
                //filtering = dtFillGrid.Copy();
                //if (filter == "الكل")
                //{

                //    gridControl2.DataSource = filtering;
                //    return;

                //}

                gridControl2.DataSource = null;
                if (filtering.Rows.Count > 0)
                {
                    DataRow dr;
                    for (int i = 0; i <= filtering.Rows.Count - 1; ++i)
                    {


                        dr = filtering.Rows[i];
                        if (filtering.Rows.IndexOf(dr) < nextPage)
                            dr.Delete();

                    }


                    filtering.AcceptChanges();
                    if (filtering.Rows.Count < 1)
                    {
                        DataRow dr1;
                        dr1 = filtering.NewRow();
                        dr1["GroupID"] = 0;
                        dr1["ItemID"] = 0;
                        dr1["ItemName"] = "------";
                        dr1["ItemImage"] = null;
                        filtering.Rows.Add(dr1);
                    }

                    gridControl2.DataSource = filtering;
                }
            }
            catch { }
        }

        private void simpleButton7_Click_1(object sender, EventArgs e)
        {

            var ss = "SELECT  * FROM  OrderOnTabletDetials  Where OrderID =" + 1;
            var dt = Lip.SelectRecord(ss);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow drow in dt.Rows)
                    btnCilick(drow["BarCode"].ToString(), Comon.ConvertToDecimalQty(drow["Qty"].ToString()));
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshGrid();
            }
            catch { }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT   ID as الرقم, ArbName as [اسم النوع] FROM  Res_OrderType  WHERE ID >4  ";

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT   IDas ID, EngName as [Type Name] FROM Res_OrderType  WHERE ID >4   ";

            ColumnWidth = new int[] { 50, 300 };

            if (cls.SQLStr != "")
            {
                frmSearch frm = new frmSearch();
                cls.strFilter = "الرقم";
                if (UserInfo.Language == iLanguage.English)
                    cls.strFilter = "ID";

                frm.AddSearchData(cls);
                frm.ColumnWidth = ColumnWidth;
                frm.ShowDialog();
                if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                {

                    try
                    {
                        OrderType = cls.PrimaryKeyValue.ToString();
                        var s = "Select * from Res_OrderType where ID =" + OrderType;
                        var ds = Lip.SelectRecord(s);
                        if (ds.Rows.Count > 0)
                        {
                            OrderTypeArb = ds.Rows[0][1].ToString();
                            OrderTypeEng = ds.Rows[0][2].ToString();
                        }
                        else
                        {
                            OrderType = "0";
                            OrderTypeArb = "محلي";
                            OrderTypeEng = "local";
                            lblOrderType.Text = OrderTypeArb;


                        }
                    }
                    catch
                    {

                        OrderType = "0";
                        OrderTypeArb = "محلي";
                        OrderTypeEng = "local";
                        lblOrderType.Text = OrderTypeArb;

                    }




                }
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            frm = new XtraForm2();
            frm.simpleButton2.Click += AddAddress1_Click;
            frm.ShowDialog();
        }

        private void AddAddress1_Click(object sender, EventArgs e)
        {
            txtAddressID.Text = frm.ID.ToString();
            txtAddressID_Validating(null, null);
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            ctCustomers = new ctAddCustomers();
            ctCustomers.simpleButton1.Click += simpleButton1111_Click;
            ctCustomers.btnClose.Click += simpleButton11111_Click;
            FlyoutAction action = new FlyoutAction();

            FlyoutProperties properties = new FlyoutProperties();

            properties.Style = FlyoutStyle.Popup;

            FlyoutDialog.Show(this, ctCustomers, action, properties);
            txtCustomerID.Text = ctCustomers.txtCustomerID.Text;
            lblCustomerName.Text = ctCustomers.txtArbName.Text;
            txtCustomerID_Validating(null, null);

        }
        private void simpleButton1111_Click(object sender, EventArgs e)
        {

            txtCustomerID.Text = ctCustomers.txtCustomerID.Text;
            lblCustomerName.Text = ctCustomers.txtArbName.Text;
            txtCustomerID_Validating(null, null);

            // txtAddressID.Text = ctCustomers.CustomerNo.ToString();
            // txtAddressID_Validating(null, null);
            // txtAddressID_Validating(null, null);


        }

        private void simpleButton11111_Click(object sender, EventArgs e)
        {

            //txtCustomerID.Text = ctCustomers.txtAccountID.Text;
            //lblCustomerName.Text = ctCustomers.txtArbName.Text;
            ////  txtCustomerID_Validating(null, null);
            //txtAddressID.Text = Comon.cInt(ctCustomers.cmbDestrict.EditValue).ToString();
            //// txtAddressID_Validating(null, null);

            //lblAddressCustomerName.Text = ctCustomers.cmbDestrict.Text + "-" + ctCustomers.txtAddress.Text;
            SendKeys.Send("{ESC}");
        }

        private void layoutView1_CardClick_1(object sender, CardClickEventArgs e)
        {
            try
            {
                ColumnView cardView = sender as ColumnView;

                //  LayoutView view = sender as LayoutView;
                if (layoutView1.RowCount < 1)
                    return;
                //   labelControl10.Text = e.Clicks.ToString();
                if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
                //layoutView1.
                //     e.Card
                //var hiTinfo = layoutView1.CalcHitInfo(e.Location);
                //if (hiTinfo.InField)
                //{

                var view = sender as DevExpress.XtraGrid.Views.Layout.LayoutView;
                view.CustomFieldValueStyle += YourLayoutView_CustomFieldValueStyle;
                //   view.CustomFieldEditingValueStyle += YourLayoutView_CustomFieldValueStyle;
                view.LayoutChanged();

                //if (hiTinfo.Column.FieldName == "ItemImage" || hiTinfo.Column.FieldName == "ItemName" || hiTinfo.Column.FieldName == "RemainQty" || hiTinfo.Column.FieldName == "SalePrice")
                //{
                long ID = Comon.cLong(layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "ItemID").ToString());
                int typeID = Comon.cInt(layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "TypeID").ToString());

                if (typeID == 8 && !checkifOffers(ID)) return;
                //  layoutView1.SetRowCellValue(hiTinfo.RowHandle, "IsSelect", 0);
                // frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;
                // FlyoutAction action = new FlyoutAction();

                // FlyoutProperties properties = new FlyoutProperties();
                //    properties.Style = FlyoutStyle.Popup;
                //Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + dt.Rows[0]["BarCode"].ToString() + "'," + Comon.cInt(txtStoreID.Text) + ") AS RemainQty")).Rows[0]["RemainQty"].ToString()).ToString("N" + MySession.GlobalPriceDigits));
                var srSize = "   SELECT dbo.RemindQtyStock(BarCode, " + Comon.cInt(txtStoreID.Text) + "," + Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString() + ") AS RemainQty  ," + layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "RemainQty").ToString() + " as RemainQtyParent,0 as PackingQtyParent "
              + ",   Stc_Items.TypeID, Stc_Items." + languagename + "  as ItemNAme,CONVERT(VARBINARY(MAX), '0xAAFF')  as ItemImage  ,  Stc_ItemUnits.BarCode,Stc_ItemUnits.PackingQty, Stc_ItemUnits.SalePrice, Stc_SizingUnits." + languagename + "  as ArbName , Stc_ItemsGroups.Notes "
         + " FROM            Stc_ItemUnits LEFT OUTER JOIN"
          + "              Stc_Items ON Stc_ItemUnits.ItemID = Stc_Items.ItemID  LEFT OUTER JOIN "
                  + "              Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID   LEFT OUTER JOIN   Stc_ItemsGroups   ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID  "
        + " WHERE        (Stc_ItemUnits.ItemID =" + ID + ")    and Stc_ItemUnits.unitCancel=0  order by Stc_ItemUnits.PackingQty Asc ";

                if (Comon.ConvertToDecimalQty(layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "RemainQty").ToString()) <= 0 && MySession.GlobalWayOfOutItems != "AllowOutItemsWithOutBalance")
                {

                    Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
                    return;
                }
                var dtSize = Lip.SelectRecord(srSize);
                if (dtSize.Rows.Count < 1)
                    return;
                else if (dtSize.Rows.Count == 1)
                {


                    if (Comon.ConvertToDecimalQty(dtSize.Rows[0]["RemainQty"].ToString()) > 0 || MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
                    {
                        btnCilick(dtSize.Rows[0]["BarCode"].ToString(), 1);
                        CalculateRow();

                    }
                    else
                    {

                        Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
                    }

                    return;
                }
                view.LayoutChanged();
                frmSize = new frmSizeItem(dtSize);
                frmSize.layoutView1.CardClick += layoutViewItem_CardClick_1;
                //   frmSize.layoutView1.CustomFieldValueStyle += layoutViewItems_CustomFieldValueStyle;
                layoutViewItems_CustomFieldValueStyle(null, null);
                // frmSize.Location.X = gridControl2.Location.X;
                frmSize.Location = new Point(gridControl2.Location.X, gridControl2.Location.Y + 100);
                frmSize.Height = 380;
                frmSize.simpleButton1.Top = 330;
                frmSize.ShowDialog();

                //    }//


                // }//
            }
            catch { }
        }

        private void layoutViewItem_CardClick_1(object sender, CardClickEventArgs e)
        {


            try
            {
                ColumnView cardView = sender as ColumnView;
                if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
                //var hiTinfo = view.CalcHitInfo(e.Location);
                //if (hiTinfo.InField)
                //{
                //    if (hiTinfo.Column.FieldName == "ItemImage" || hiTinfo.Column.FieldName == "ArbName" || hiTinfo.Column.FieldName == "RemainQty" || hiTinfo.Column.FieldName == "SalePrice")
                //    {
                var view = sender as DevExpress.XtraGrid.Views.Layout.LayoutView;
                view.CustomFieldValueStyle += layoutViewItems_CustomFieldValueStyle;
                //   view.CustomFieldEditingValueStyle += YourLayoutView_CustomFieldValueStyle;
                view.LayoutChanged();
                if (Comon.ConvertToDecimalQty(view.GetRowCellValue(cardView.FocusedRowHandle, "RemainQtyParent").ToString()) >= Comon.ConvertToDecimalQty(Comon.ConvertToDecimalQty(view.GetRowCellValue(cardView.FocusedRowHandle, "PackingQty").ToString()) / Comon.ConvertToDecimalQty(view.GetRowCellValue(cardView.FocusedRowHandle, "PackingQtyParent").ToString())) || MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
                {

                    string ID = view.GetRowCellValue(cardView.FocusedRowHandle, "BarCode").ToString();
                    btnCilick(ID, 1);

                    CalculateRow();
                    frmSize.Close();

                }
                else
                {

                    Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
                }

                return;


                //    }
                //}
            }
            catch { }






        }
        private void layoutViewItems_CustomFieldValueStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewFieldValueStyleEventArgs e)
        {
            ColumnView cardView = sender as ColumnView;
            if (cardView == null) return;
            //   cardView.Appearance.
            if (cardView.FocusedRowHandle == e.RowHandle && cardView.IsFocusedView)// && cardView.FocusedColumn == e.Column)
            {
                e.Appearance.BackColor = Color.Gold;
                e.Appearance.BackColor = Color.Gold;
                e.Appearance.BackColor2 = Color.Gold;
                e.Appearance.ForeColor = Color.Black;
                return;

            }
        }

        private void YourLayoutView_CustomFieldValueStyle(object sender, LayoutViewFieldValueStyleEventArgs e)
        {
            ColumnView cardView = sender as ColumnView;
            if (cardView.FocusedRowHandle == e.RowHandle && cardView.IsFocusedView)// && cardView.FocusedColumn == e.Column)
            {
                e.Appearance.BackColor = Color.Gold;
                e.Appearance.BackColor = Color.Gold;
                e.Appearance.BackColor2 = Color.Gold;
                e.Appearance.ForeColor = Color.Black;
                return;

            }
        }

        private void simpleButton12_Click_1(object sender, EventArgs e)
        {
            panelnotes.Visible = true ^ panelnotes.Visible;
            txtNotesInvoice.Text = "";
            txtNotesInvoice.Focus();
            pnlCalcuate.Visible = false;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{ESC}");
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            //gridView2.SetFocusedRowCellValue("extension", uAddExtension.extensionvar[0]);
            //gridView2.SetFocusedRowCellValue("Serials", uAddExtension.extensionvar[2]);
            //gridView2.SetFocusedRowCellValue("rowhandling", uAddExtension.row_handel);
            //gridView2.SetFocusedRowCellValue("SalePrice", Comon.ConvertToDecimalPrice(uAddExtension.extensionvar[1]) + Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("BAGET_W")));
            //uc.Dispose();
            SendKeys.Send("{ESC}");
            SendKeys.Send("{ESC}");
            CalculateRow();
        }

        private void btnOpenMap_Click(object sender, EventArgs e)
        {
            try
            {
                ctAddDelivery cctAddDelivery = new ctAddDelivery();
                FlyoutAction action = new FlyoutAction();
                FlyoutProperties properties = new FlyoutProperties();
                properties.Style = FlyoutStyle.Popup;

                FlyoutDialog.Show(this, cctAddDelivery, action, properties);
                txtDriverID.Text = cctAddDelivery.txtCustomerID.Text;
                lblDriverName.Text = cctAddDelivery.txtArbName.Text;
                txtDriverID_Validating(null, null);

            }
            catch
            {


            }

        }

        private void indexGridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.IsRowSelected(e.RowHandle))
            {
                e.Appearance.BackColor = System.Drawing.Color.Yellow;// System.Drawing.Color.FromArgb(25, 71, 138);
                e.Appearance.ForeColor = System.Drawing.Color.Black;
                e.HighPriority = true;
            }
        }
        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            //frmKDSMonitor frm = new frmKDSMonitor();
            //frm.Show();
            Validations.ErrorTextClear(txtCustomerID, txtCustomerID.ToolTip);
            frm = new XtraForm2();
            frm.simpleButton2.Click += searchResut_Click;
            frm.ShowDialog();
        }
        private void searchResut_Click(object sender, EventArgs e)
        {
            txtCustomerID.Text = frm.CustomerID.ToString();
            txtCustomerID_Validating(null, null);

        }

        private void labelControl11_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count < 1)
                return;
            frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();

            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                BindingSource bs = new BindingSource();
                bs.DataSource = gridControl.DataSource;
                frm.Show();
                frm.fillMAsterData(dt);
                frm.gridControl.DataSource = bs;
                frm.CalculateRow();
                frm.txtCustomerInvoiceID.Text = txtInvoiceID.Text;
                frm.lblAdditionaAmmount.Text = lblAdditionaAmmount.Text;
            }
            else
                frm.Dispose();
        }

        private void txtMobile_Validating(object sender, CancelEventArgs e)
        {

        }

        private void frmDeliveryInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Do you really want to exit " : "هل تريد اغلاق النافذة"));
                if (Yes)
                {
                    e.Cancel = false;
                    this.Dispose();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void txtDriverID_Validating(object sender, CancelEventArgs e)
        { 
            DataTable dt;
            if (txtDriverID.Text != string.Empty && txtDriverID.Text != "0")
            {
                strSQL = "SELECT " + PrimaryName + " as DriverName   FROM Sales_Drivers Where    DriverID =" + txtDriverID.Text;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    lblDriverName.Text = dt.Rows[0]["DriverName"].ToString();
                    OrderType = btnDelivery.Text;
                    // Find();
                    pnlDeliverContol.Visible = true;
                    btnDelivery.Appearance.BorderColor = Color.FromArgb(83, 68, 63);//LightSeaGreen
                    btnLocal.Appearance.BorderColor = Color.Green;
                    btnHangerStation.Appearance.BorderColor = Color.White;
                    btnTakeAway.Appearance.BorderColor = Color.FromArgb(255, 128, 0); ;
                    // 255, 128, 0 ,LightSeaGreen,Green
                    btnDelivery.BackColor = Color.LightYellow;
                    btnDelivery.ForeColor = Color.Black;
                    btnLocal.BackColor = Color.Transparent;
                    btnLocal.ForeColor = Color.Black;
                    OrderType = "3";
                    RefreshOffers();
                    OrderTypeArb = "توصيل";
                    OrderTypeEng = "Delivery";
                    lblOrderType.Text = OrderTypeArb;

                }
            }
        }

        private void ShoeWrning_CheckedChanged(object sender, EventArgs e)
        {
            if (ShoeWrning.Checked)
            {
                frmWrningItemQty frm = new frmWrningItemQty();
                frm.Show();


            }
           
        }

        private void txtPaidAmount_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            { 
                   
                      if (MethodID == 1)
                    {
                        lblRemaindAmount.Text = (Comon.cDbl(txtPaidAmount.Text) - Comon.cDbl(lblNetBalance.Text)).ToString();

                    }
                    else if (MethodID == 3)
                    {
                        lblRemaindAmount.Text = ((Comon.cDbl(txtPaidAmount.Text) + Comon.cDbl(txtNetAmount.Text)) - Comon.cDbl(lblNetBalance.Text)).ToString();

                    } 
                
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }
    }


}

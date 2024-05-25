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
using Edex.Model;
using System.IO;
using Edex.RestaurantSystem.UserControls;
using Edex.Model.Language;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraSplashScreen;
using Edex.GeneralObjects.GeneralForms;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraGrid.Menu;
using System.Globalization;
using DevExpress.XtraGrid.Columns;
using Edex.DAL.SalseSystem;
using Edex.TimeStaffScreens;
using Edex.DAL.Configuration;
using Edex.ModelSystem;
using Edex.StockObjects.Codes;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.AccountsObjects.Codes;
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
using Edex.DAL.UsersManagement;
using System.Runtime.InteropServices;
using DevExpress.XtraGrid.Views.Layout.ViewInfo;
using System.Windows.Forms;
using Edex.GeneralObjects.GeneralUserControls;
using Edex.AccountsObjects.Reports;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class frmNewPos : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        ucsubGroup ucSubgroup;

        #region Declare
        public int countCard = 15;
        public string sourceGroup = "";
        public DataTable dtFillGrid = new DataTable();
        public DataTable filtering = new DataTable();
        public DataTable dtPrint2 = new DataTable();
        public DataTable dtPriceItemOffers = new DataTable();
        public DataTable dtPriceCustomersOffers = new DataTable();
        ctAddCustomers ctCustomers = new ctAddCustomers();
        public DataTable dtSpecialOffers = new DataTable();
        public int nextPage = 0;
        XtraForm2 frm = new XtraForm2();
        confirmSaving confirm = new confirmSaving();
        public string DeliveryName = "";
        public bool stopSave = false;
        public frmAddressCustomer frmAddressCust;
        public bool ShowReportInReportViewer;
        public bool FormAdd;
        public bool FormDelete;
        public bool FormUpdate;
        public bool FormView;
        public bool ReportView;
        public string languagename = "";
        public bool ReportExport;
        public int TableID = 0;
        public frmSizeItem frmSize;
        //  public SizeItemPop frmSize;
        public string ReportName;
        CompanyHeader cmpheader = new CompanyHeader();
        public int DiscountCustomer = 0;
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
        private string OrderType = "1";
        private string OrderTypeArb = "محلي";
        private string OrderTypeEng = "Dine-In";
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


        public void RefreshOffers()
        {

            string dateFrom = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            var spriceOffers = "SELECT        PriceItemsOffers.*  FROM     PriceItemsOffers"
+ "  where ((IsAmount>0)or (IsPercent>0)or(IsOffers>0)) And ((OrderType=0)or(OrderType=" + OrderType + "))"
+ "   And"
+ " (FromDate<=" + dateFrom + ")And (ToDate>=" + dateFrom + ")";

            dtPriceItemOffers = Lip.SelectRecord(spriceOffers);





        }
        public void fillGrid()
        {
            ribbonControl1.Visible = false;

            var sr = "Select 0 RemainQty ,"
               + "concat((Select Top 1 Stc_ItemUnits.SalePrice from Stc_ItemUnits where  Stc_ItemUnits.ItemID=Stc_Items.ItemID and  Stc_ItemUnits.unitCancel=0 order by Stc_ItemUnits.PackingQty Desc ),'   ',(Select Top 1 Stc_SizingUnits." + languagename + " from Stc_ItemUnits inner join Stc_SizingUnits on Stc_SizingUnits.SizeID=Stc_ItemUnits.SizeID where  Stc_ItemUnits.ItemID=Stc_Items.ItemID and  Stc_ItemUnits.unitCancel=0 order by Stc_ItemUnits.PackingQty Desc ) ) AS SalePrice ,Stc_Items.TypeID, Stc_Items.GroupID, Stc_Items.ItemID, Stc_Items." + languagename + " as ItemName,Stc_Items.ItemImage from Stc_Items where (Stc_Items.TypeID=1) and   Cancel=0   ";
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


            var SpecialOffers = "SELECT        SpecialOffers_Master.*  FROM     SpecialOffers_Master"
+ "  where isActive>0"
+ "   And"
+ " (FromDate<=" + dateFrom + ")And (ToDate>=" + dateFrom + ")";

            dtSpecialOffers = Lip.SelectRecord(SpecialOffers);


        }
        #region InitializeComponent
       
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
          public string GroupsDefault = "";
        public frmNewPos()
        {
          
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                InitializeComponent();
                GroupsDefault = System.IO.File.ReadAllText(Application.StartupPath + "\\GroupsDefault.txt");

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
                // string[] s = new string[] { "الكل", "ا", "أ", "ب", "ت", "ث", "ج", "ح", "خ", "د", "ذ", "ر", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ك", "ل", "م", "ن", "ه", "و", "ي" };
               

                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionItemID = " الصنف";
                CaptionItemName = " الصنف";
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

              //  lblNetBalance.BackColor = Color.WhiteSmoke;
                // lblNetBalance.ForeColor = Color.Black;
              //  strSQL = "ArbName";
              //  Lip.ConvertStrSQLToEnglishOrArabicLanguage(languagename, "Arb");
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
                  //  Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");

                }
                InitGrid(); 
                /*********************** Fill Data ComboBox  ****************************/
                // FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbCurency, "Currency", "CurrencyID", languagename, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", languagename, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbFormPrinting, "FormPrinting", "FormID", languagename, "", " 1=1 ", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbBank, "[Acc_Banks]", "ID", languagename, "", " 1=1 ", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
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
               // RolesButtonSearchAccountID();
                /********************* Event For Account Component ****************************/
                /********************* Event For TextEdit Component **************************/
             
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

             //   this.txtDiscountOnTotal.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
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
                DoNew();
                ListItemInit();

                dVat = Lip.SelectRecord(VAt);
                cmpheader = CompanyHeaderDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);

                

                SplashScreenManager.CloseForm(false);
                //if ((cmpheader.pic) != null)
                //{
                //    TheImage = new MemoryStream(cmpheader.pic);
                //    if (TheImage.Length > 0)
                //        picCompanyHeader.Image = Image.FromStream(TheImage, true);
                //}
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


        #region GridView
        void InitGrid()
        {
            lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/
            gridView2.Columns["BranchID"].Visible = false;
            gridView2.Columns["PackingQty"].Visible = false;
            gridView2.Columns["BAGET_W"].Visible = false;
            gridView2.Columns["STONE_W"].Visible = false;
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
            gridView2.Columns["Discount"].Visible = false;
            gridView2.Columns["HavVat"].Visible = false;
            gridView2.Columns["RemainQty"].Visible = false;
            gridView2.Columns["ItemID"].Visible = false;

            gridView2.Columns["BarCode"].Visible = false;



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
            gridView2.Columns["AdditionalValue"].OptionsColumn.AllowFocus = false;
            /************************ Date Time **************************/

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

            gridView2.Columns[SizeName].Width = 50;
            gridView2.Columns[ItemName].Width = 120;
            gridView2.Columns["QTY"].Width = 50;
            gridView2.Columns["SalePrice"].Width = 40;
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
                strQty = "";
                txtTotal.Text = "";
                simpleButton1_Click_2(null, null);
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
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());

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

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PageNo"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemStatus"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Caliber"], dt.Rows[0]["GroupID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Equivalen"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Net"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["TheCount"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Width"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Total"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], QtyIn);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"], true);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemainQty"], 0);
                DataRow[] dr;
                if (dtPriceItemOffers.Rows.Count > 0)
                {

                    dr = dtPriceItemOffers.Select("((FromGroupID<=" + dt.Rows[0]["GroupID"].ToString() + "and ToGroupID>=" + dt.Rows[0]["GroupID"].ToString() + " )AND(FromItemID<=" + dt.Rows[0]["ItemID"].ToString() + "and ToItemID>=" + dt.Rows[0]["ItemID"].ToString() + " ) and (FromSizeID<=" + dt.Rows[0]["SizeID"].ToString() + "and ToISizeID>=" + dt.Rows[0]["SizeID"].ToString() + " )) or((FromItemID<=" + dt.Rows[0]["ItemID"].ToString() + "and ToItemID>=" + dt.Rows[0]["ItemID"].ToString() + " ) and (FromSizeID<=" + dt.Rows[0]["SizeID"].ToString() + "and ToISizeID>=" + dt.Rows[0]["SizeID"].ToString() + " ))OR ((FromItemID<=" + dt.Rows[0]["ItemID"].ToString() + "and ToItemID>=" + dt.Rows[0]["ItemID"].ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + dt.Rows[0]["GroupID"].ToString() + "and ToGroupID>=" + dt.Rows[0]["GroupID"].ToString() + " ) ");
                    if (dr.Length > 0 && gridView2.Columns["Description"].ToString() != "INS")
                    {

                        DateTime nowDate = DateTime.ParseExact(Comon.ConvertSerialDateTo(Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString()), "dd/MM/yyyy", culture);
                        int i = (int)nowDate.DayOfWeek;
                        int timenow = Comon.cInt(Lip.GetServerTimeSerial());
                        // if (Comon.cInt(dr[0]["day" + i].ToString()) == 1 && (timenow >= Comon.cInt(dr[0]["FromTime"].ToString()) && timenow <= Comon.cInt(dr[0]["ToTime"].ToString())))
                        if (Comon.cInt(dr[0]["IsAmount"].ToString()) > 0)
                        {
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Discount"], dr[0]["AmountCost"].ToString());
                            // itemgroup[2] = 1;
                        }
                        else if (Comon.cInt(dr[0]["IsPercent"].ToString()) > 0)
                        {
                            decimal PercentAmount = Comon.ConvertToDecimalPrice(dr[0]["PercentCost"].ToString());
                            decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()) * QtyIn * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Discount"], total);
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], PercentAmount);
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "IsPercent");

                            //  itemgroup[2] = 1;
                        }

                        else if (Comon.cInt(dr[0]["IsOffers"].ToString()) > 0)
                        {
                            drgroupItem = dr;
                            // decimal QtyOffers;
                            // if (Comon.cInt(dr[0]["IsTakeOne"].ToString()) > 0)
                            // {
                            //     AddnewItem(dt.Rows[0]["BarCode"].ToString(), Comon.ConvertToDecimalPrice(1));

                            //}
                            // else if (Comon.cInt(dr[0]["IsGetSame"].ToString()) > 0)
                            //{

                            //    QtyOffers = Comon.ConvertToDecimalPrice(dr[0]["GetSameAmount"].ToString());
                            //    if (QtyOffers >= QtyIn) 
                            //        AddnewItem(dt.Rows[0]["BarCode"].ToString(),Comon.ConvertToDecimalPrice(dr[0]["SetSameAmount"].ToString()));
                            //    else
                            //        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "ISOFFER1");

                            // }

                            // else if (Comon.cInt(dr[0]["IsGetOnther"].ToString()) > 0)
                            //{
                            //    QtyOffers = Comon.ConvertToDecimalPrice(dr[0]["GetOntherAmount"].ToString());


                            // }



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

        private object getNote(int p)
        {
            var dt = dtSpecialOffers.Select("ItemID=" + p.ToString());
            if (dt.Length < 1) return "";
            return dt[0]["Notes"].ToString();
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

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "");
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

                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    DataRow[] dr;
                    QTYRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                    if (Comon.cInt(txtCustomerID.Text) > 0)
                    {
                        if (dtPriceCustomersOffers.Rows.Count > 0)
                        {
                            dr = dtPriceCustomersOffers.Select("(FromCustomerID<=" + Comon.cInt(txtCustomerID.Text) + "and ToCustomerID>=" + Comon.cInt(txtCustomerID.Text) + " ) Or (ISForAll=1) or (FromSaleTotal<=" + TotalBeforeDiscount + "and ToSaleTotal>=" + TotalBeforeDiscount + "  and FromSaleTotal<>0 and ToSaleTotal<>0  )");

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

                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView2.GetRowCellValue(i, "HavVat"));
                    var declerationINS = gridView2.GetRowCellValue(i, "Description").ToString();
                    if (declerationINS == "INS")
                    {
                        HavVatRow = false;
                        InsurmentRow += Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                    }

                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                    AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
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
                    AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
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
                if (DiscountOnTotal > 0)
                {
                    decimal Total = TotalAfterDiscount - DiscountOnTotal;
                    AdditionalAmount = (Total) / 100 * MySession.GlobalPercentVat;
                    Net = Comon.ConvertToDecimalPrice(Total + AdditionalAmount);
                }
                lblAdditionaAmmount.Text = Comon.ConvertToDecimalPrice(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = Comon.ConvertToDecimalPrice(Net).ToString("N" + MySession.GlobalPriceDigits);

                if (Comon.ConvertToDecimalPrice(lblBalanceSum.Text) >= 0)
                {
                    if (lblBalanceSum.BackColor != Color.Red)
                    {
                        if (Comon.ConvertToDecimalPrice(lblBalanceSum.Text) >= Comon.ConvertToDecimalPrice(lblNetBalance.Text))
                        {
                            txtCutBalance.Text = lblNetBalance.Text;

                        }
                        else
                        {
                            txtCutBalance.Text = lblBalanceSum.Text;
                            lblRequireAmmut.Text = (Comon.ConvertToDecimalPrice(lblNetBalance.Text) - Comon.ConvertToDecimalPrice(txtCutBalance.Text)).ToString();
                        }

                    }


                    lblRequireAmmut.Text = (Comon.ConvertToDecimalPrice(lblNetBalance.Text) - Comon.ConvertToDecimalPrice(txtCutBalance.Text)).ToString();
                    if (lblBalanceSum.BackColor != Color.Red)
                        lblRemaindBalance.Text = (Comon.ConvertToDecimalPrice(lblBalanceSum.Text) - Comon.ConvertToDecimalPrice(txtCutBalance.Text)).ToString();

                    else
                    {
                        txtCutBalance.Text = "0";
                        lblRemaindBalance.Text = "0";
                    }

                    lblRequireAmmut.BackColor = Color.Transparent;
                    if (Comon.ConvertToDecimalPrice(lblRequireAmmut.Text) > 0)
                    {
                        lblRequireAmmut.BackColor = Color.Red;

                    }

                }
            }

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        #endregion

        private void panelControl7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelControl6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmNewPos_Load(object sender, EventArgs e)
        {
            
            ucSubgroup = new ucsubGroup();
            ButtonSubGroupEvent(ucSubgroup.ArrbtnItemGroups, ucsubGroup.SizeItemGroupPage);

            ucMainGroup ucmaingroup = new ucMainGroup();
            ButtonMainGroupEvent(ucmaingroup.ArrbtnItemGroups, ucMainGroup.SizeItemGroupPage);
            btnMainGroup_Click(ucmaingroup.ArrbtnItemGroups[0], null);
           ucmaingroup.Dock = DockStyle.Fill;
          //  pnlMainGroup.Controls.Add(ucmaingroup);
            ucSubgroup.Dock = DockStyle.Fill;
            pnSubGroup.Controls.Add(ucSubgroup);
            ucClockCashier cc=new ucClockCashier();
            cc.Dock = DockStyle.Left;
            pnlMainHeader.Controls.Add(cc);

            FormUpdate = true;
            FormAdd = true;
            FormView = true;


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

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }


        #region ButtonsGroups
        void ButtonMainGroupEvent(Button_WOC[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
                arr[i].Click += new System.EventHandler(this.btnMainGroup_Click);
        }
        void ButtonSubGroupEvent(Button_WOC[] arr, int SizePage)
        {
            for (int i = 0; i < SizePage; i++)
                arr[i].Click += new System.EventHandler(this.btnSubGroup_Click);
        }

        private void btnSubGroup_Click(object sender, EventArgs e)
        {
            try{
              string ItemGroupID = ((Button_WOC)sender).Name;
              sourceGroup = ItemGroupID;
            var filter = ItemGroupID;
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
        private void btnMainGroup_Click(object sender, EventArgs e)
        {
            
           
            //  string ItemGroupEngName = ((Button_WOC)sender).Tag.ToString();
          // ((Button_WOC)sender).ButtonColor = Color.Green;
            //  ((SimpleButton)sender).Appearance.BackColor2 = Color.Green;
          //  ((Button_WOC)sender).ForeColor = Color.White;
            #region filter Table By Section
            try
            {
                      

               //var filter = ItemGroupID;

                var sr = "Select GroupID as ColorID," + languagename + " as ArbName from Stc_ItemsGroups where Cancel=0 and GroupID in(" + GroupsDefault + ")";
                ucSubgroup.dtGroups = Lip.SelectRecord(sr);
                 ucSubgroup.CountItemGroupPage = ucSubgroup.getCountPage(ucSubgroup.dtGroups.Rows.Count, ucsubGroup.SizeItemGroupPage);
                ucSubgroup.gprevious_Click(null, null);
                ucSubgroup.btnItemGroup_Click(ucSubgroup.ArrbtnItemGroups[0], null);
                btnSubGroup_Click(ucSubgroup.ArrbtnItemGroups[0], null);

            }
            catch
            {
                ucSubgroup.CountItemGroupPage = ucSubgroup.getCountPage(0, ucsubGroup.SizeItemGroupPage);
                ucSubgroup.gprevious_Click(null, null);
                ucSubgroup.btnItemGroup_Click(ucSubgroup.ArrbtnItemGroups[0], null);
                btnSubGroup_Click(ucSubgroup.ArrbtnItemGroups[0], null);

            }



            #endregion
        }

        #endregion

       
        private void gridControl2_Resize(object sender, EventArgs e)
        {
            if (this.Width == 1024)
            {
                layoutView1.OptionsMultiRecordMode.MaxCardColumns = 5;
                layoutView1.OptionsMultiRecordMode.MaxCardRows =4;

            }
            else {

                layoutView1.OptionsMultiRecordMode.MaxCardColumns = 8;
                layoutView1.OptionsMultiRecordMode.MaxCardRows = 4;
            
            
            }
        }




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

            if (FocusedControl.Trim() == txtCustomerID.Name )
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
                DiscountCustomer = 0;
                lblRequireAmmut.Text = "0";
                lblRemaindBalance.Text = "0";
                lblDebit.Text = "0";
                lblCredit.Text = "0";
                lblBalanceSum.Text = "0";
                lblBalanceSum.BackColor = Color.Transparent;
                lblRequireAmmut.BackColor = Color.Transparent;
                txtFloor.Text = "";
                txtBuilding.Text = "";
                txtApartment.Text = "";
                txtMobile.Text = "";
                txtAddressID.Text = "";
                txtAccountID.Text = "";
                txtCustomerID.Text = "";
                lblCustomerName.Text = "";

               /// txtAddressID_Validating(null, null);
                txtCutBalance.Text ="0";
                lblRemaindBalance.Text ="0";
                lblUnitDiscount.Text ="0";
                txtDiscountOnTotal.Text = "0";
                //txtCustomerName.Text = "";
                txtPaidAmount.Text = "";
                lblRemaindAmount.Text = "";
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
                txtCustomerID.Tag = " ";
                txtNetProcessID.Tag = " ";
                cmbBank.Tag = " ";
                cmbNetType.Tag = " ";
                txtNetAmount.Tag = " ";
              //  pnlNetControl.Visible = false;
                pnlDeliverContol.Visible = false;
                //txtCheckID.Tag = " ";
                /////////////////////////////////////////////////
                var dk = Lip.GetServerDate();
                txtInvoiceDate.Text = dk;
                txtWarningDate.Text = dk;
                txtCheckSpendDate.Text = dk;
               // checkBox1.Checked = false;
               // checkBox2.Checked = true;
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
                //lblUnitDiscount.Text = "0";
                txtDiscountOnTotal.Text = "0";
                txtDiscountPercent.Text = "0";
                lblDiscountTotal.Text = "0";
                lblAdditionaAmmount.Text = "0";
                lblNetBalance.Text = "0";
                //picItemUnits.Image = null;

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

                //txtCustomerName.Visible = false;
                txtCustomerID.Visible = false;
                lblCustomerName.Visible = false;

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
                txtInvoiceID.Text = "";// Sales_SaleInvoicesDAL.GetNewID(MySession.GlobalFacilityID, MySession.GlobalBranchID, MySession.UserID).ToString();
                TableID = 0;
                txtRegistrationNo.Text = "";// RestrictionsDailyDAL.GetNewID(this.Name).ToString();
                IdPrint = false;
                ClearFields();
                txtDailyID.Text = "";// Sales_SaleInvoicesDAL.GetNewDialyID(MySession.GlobalFacilityID, MySession.GlobalBranchID, MySession.UserID).ToString();
                EnabledControl(true);
                cmbFormPrinting.EditValue = 1;
                gridView2.Focus();
                gridView2.MoveNext();
                gridView2.FocusedColumn = gridView2.VisibleColumns[1];
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
                if (MySession.PrintModel == 1)
                {
                    checkA4.Checked = false;
                    checkDt.Checked = false;
                }
                else
                    if (MySession.PrintModel == 2)
                    {
                        checkA4.Checked = false;
                        checkDt.Checked = true;
                    }
                    else
                    {
                        checkA4.Checked = true;
                        checkDt.Checked = false;
                    }

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
     









        public void detials(long orderID, int TableID)
        {
            // txtTableID.Text = TableID.ToString();
            //  OrderID = orderID;
            var ss = "SELECT  * FROM  Sales_SuspensionDetails  Where InvoiceID =" + orderID;
            var dt = Lip.SelectRecord(ss);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow drow in dt.Rows)
                {
                    btnCilick(drow["BarCode"].ToString(), Comon.ConvertToDecimalQty(drow["Qty"].ToString()));
                    CalculateRow();
                }
                strSQL = "Delete From Sales_SuspensionDetails Where InvoiceID  = " + orderID;

                strSQL += "Delete From Sales_SuspensionMaster Where InvoiceID  = " + orderID;
                Lip.ExecututeSQL(strSQL);
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




        private int SaveVouchersDiscount(double CreditAmount, string InvoiceID)
        {




            double AccountID = 0;
            double PercentDiscount;
            DataRow[] row;
            //if (Comon.cInt(cmbMethodID.EditValue) == 5)
            //{
            //    double net = Comon.cDbl(lblNetBalance.Text);//- Comon.cDbl(txtInsurmentAmount1.Text);

            //    if (net - Comon.cDbl(txtNetAmount.Text) >= Comon.cDbl(txtInsuranceAmmount.Text))
            //    {

            //        row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
            //        if (row.Length > 0)
            //        {
            //            AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            //        }


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

            var sr = "Select * from NetType where NetTypeID =" + Comon.cDbl(cmbNetType.EditValue);
            var dt = Lip.SelectRecord(sr);
            if (dt.Rows.Count < 1) return 0;

            if (Comon.cInt(dt.Rows[0]["ISFixed"].ToString()) > 0)
            {

                if (Comon.cInt(dt.Rows[0]["ISamountFixed"].ToString()) > 0)
                {

                    CreditAmount = Comon.cDbl(dt.Rows[0]["amountFixed"].ToString());
                }
                else
                {

                    CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["percentFixed"].ToString()) / 100);

                }


            }
            else if (Comon.cInt(dt.Rows[0]["ISChange"].ToString()) > 0)
            {

                if (CreditAmount < Comon.cDbl(dt.Rows[0]["CostLess"].ToString()))
                {
                    if (Comon.cInt(dt.Rows[0]["ISamountChangeLess"].ToString()) > 0)
                    {
                        CreditAmount = Comon.cDbl(dt.Rows[0]["amountChangeLess"].ToString());

                    }
                    else if (Comon.cInt(dt.Rows[0]["ISPercentChangeLess"].ToString()) > 0)
                    {

                        CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["PercentChangeLess"].ToString()) / 100);

                    }
                }
                else
                {
                    if (Comon.cInt(dt.Rows[0]["ISamountChangeMore"].ToString()) > 0)
                    {

                        CreditAmount = Comon.cDbl(dt.Rows[0]["amountChangeMore"].ToString());

                    }

                    else if (Comon.cInt(dt.Rows[0]["ISPercentChangeMore"].ToString()) > 0)
                    {

                        CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["PercentChangeMore"].ToString()) / 100);

                    }
                }


            }
            AccountID = Comon.cDbl(cmbNetType.EditValue);




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



            row = dtDeclaration.Select("DeclareAccountName = 'NetworkDiscount'");
            if (row.Length < 1)
                return 0;


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();



            returned = new Acc_VariousVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            // returned.AccountID = Comon.cDbl(txtCustomerID.Text); ;
            returned.VoucherID = 0;
            returned.Credit = CreditAmount;
            returned.Debit = 0;
            returned.Declaration = "مبلغ-خصم شبكة للفاتورة رقم-" + invoiceNo;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            returned = new Acc_VariousVoucherDetails();
            returned.ID = 1;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            returned.VoucherID = 0;
            returned.Credit = 0;
            returned.Debit = CreditAmount;
            returned.Declaration = "مبلغ-خصم شبكة للفاتورة رقم-" + invoiceNo;
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
            btnMainGroup_Click(null, null);

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
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='rptSalesInvoicePoint'");
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

        protected void DoPrint()
        {

            try
            {
                if (IsNewRecord)
                {
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforePrinting);
                    return;
                }

                Application.DoEvents();



                if (Comon.cLong(txtAccountID.Text) > 0)
                {
                    frmAccountStatement frm = new frmAccountStatement(Comon.cLong(txtAccountID.Text));
                    lblDebit.Text = frm.lblDebit.Text;
                    lblCredit.Text = frm.lblCredit.Text;
                    lblBalanceSum.Text = frm.lblBalanceSum.Text;
                    lblBalanceSum.BackColor = Color.Transparent;

                    if (Comon.cDec(lblDebit.Text) > Comon.cDec(lblCredit.Text))
                    {

                        lblBalanceSum.BackColor = Color.Red;
                    }
                    lblRemaindBalance.Text = "0";
                    if (lblBalanceSum.BackColor != Color.Red)
                        lblRemaindBalance.Text = (Comon.ConvertToDecimalPrice(lblBalanceSum.Text) - Comon.ConvertToDecimalPrice(txtCutBalance.Text)).ToString();
                    else
                        lblRemaindBalance.Text = (Comon.cDec(lblDebit.Text)-Comon.cDec(lblCredit.Text))*-1 +"";

                }




                /******************** Report Body *************************/
                //rptForm = "rptCashierPrint";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                rptFormName = "rptCashierPrint";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/

                rptForm.Parameters["CustomerMobile"].Value = txtMobile.Text;
                rptForm.Parameters["lblRemaindBalance"].Value = lblRemaindBalance.Text;

                rptForm.RequestParameters = false;
                if (IdPrint == true)
                {
                    rptForm.Parameters["InvoiceID"].Value = txtInvoiceDate.Text + " - " + invoiceNo;
                    rptForm.Parameters["InvoiceNo"].Value = invoiceNo;
                }
                else
                {
                    rptForm.Parameters["InvoiceID"].Value = txtInvoiceDate.Text + " - " + txtInvoiceID.Text.Trim().ToString();
                    rptForm.Parameters["InvoiceNo"].Value = txtInvoiceID.Text.Trim().ToString();
                }
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.Trim().ToString();
                rptForm.Parameters["MethodName"].Value = "";
                rptForm.Parameters["TheTime"].Value = Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":", "").Trim());
                rptForm.Parameters["CashierName"].Value = UserInfo.SYSUSERARBNAME.ToString();
                rptForm.Parameters["CompanyName"].Value = (UserInfo.Language == iLanguage.Arabic ? cmpheader.CompanyArbName : cmpheader.CompanyEngName);
                rptForm.Parameters["CompanyAddress"].Value = (UserInfo.Language == iLanguage.Arabic ? cmpheader.ArbAddress : cmpheader.ArbAddress);
                if (dVat.Rows.Count > 0)
                    rptForm.Parameters["CompanyVatID"].Value = Comon.cLong(dVat.Rows[0][0]);
                else
                    rptForm.Parameters["CompanyVatID"].Value = 0;

                rptForm.Parameters["NetTotal"].Value = 0;
                //switch (MethodID)
                //{
                //    case (1):
                //        rptForm.Parameters["NetTotal"].Value = 0; break;
                //    case (2):
                //        rptForm.Parameters["NetTotal"].Value = lblNetBalance.Text.Trim().ToString(); break;

                //    case (3):
                //        rptForm.Parameters["NetTotal"].Value = txtNetAmount.Text; break;



                //}



                decimal vat = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(lblNetBalance.Text.Trim().ToString()) - ((Comon.ConvertToDecimalPrice(lblNetBalance.Text.Trim().ToString()) * 100) / (100 + MySession.GlobalPercentVat)));
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
                rptForm.Parameters["AdditionalAmount"].Value = ((Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text.Trim().ToString())) - (Comon.ConvertToDecimalPrice(lblDiscountTotal.Text.Trim().ToString()))) * Comon.ConvertToDecimalPrice(MySession.GlobalPercentVat / 100);
                rptForm.Parameters["AdditionalAmount"].Value = vat;
                //rptForm.Parameters["InvoiceTotal"].Value = Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text.Trim().ToString()) - vat;
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();


                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView2.GetRowCellValue(i, "ArbItemName").ToString() + "   " + gridView2.GetRowCellValue(i, "ArbSizeName").ToString();// (gridView2.GetRowCellValue(i, "BarCode").ToString() + "@" + gridView2.GetRowCellValue(i, ItemName).ToString()).Replace("@",
                    //        Environment.NewLine);// +gridView2.GetRowCellValue(i, "BarCode").ToString() + gridView2.GetRowCellValue(i, SizeName).ToString() + gridView2.GetRowCellValue(i, "PackingQty").ToString();

                    if (Comon.cInt(cmbLanguagePrint.EditValue) == 2)
                        row["ItemName"] = gridView2.GetRowCellValue(i, "EngItemName").ToString() + gridView2.GetRowCellValue(i, "BarCode").ToString();
                    else if (Comon.cInt(cmbLanguagePrint.EditValue) == 3)
                        row["ItemName"] = gridView2.GetRowCellValue(i, "ArbItemName").ToString() + "    " + gridView2.GetRowCellValue(i, "EngItemName").ToString() + " " + gridView2.GetRowCellValue(i, "EngSizeName").ToString() + " " + gridView2.GetRowCellValue(i, "ArbSizeName").ToString();
                    // row["ItemName"] = gridView2.GetRowCellValue(i, ItemName).ToString() + gridView2.GetRowCellValue(i, "BarCode").ToString() + gridView2.GetRowCellValue(i, SizeName).ToString() + gridView2.GetRowCellValue(i, "PackingQty").ToString();
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
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message + " " + ex.Data);
            }

        }


        protected void DoPrintA4()
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
                ReportName = "rptSalesInvoicePoint";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");



                rptFormName = "rptSalesInvoiceArb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["RemaindAmount"].Value = lblRemaindAmount.Text.Trim().ToString();
                rptForm.Parameters["PaidAmount"].Value = txtPaidAmount.Text.Trim().ToString();
                if (Comon.cInt(cmbMethodID.EditValue) == 2)
                {

                    rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.ToString();
                }
                else //if (Comon.cInt(cmbMethodID.EditValue) == 2)
                {
                    rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.ToString();


                }
                if (Comon.cInt(cmbMethodID.EditValue) == 5)
                {
                    rptForm.Parameters["CashTotal"].Value = Comon.ConvertToDecimalPrice(lblNetBalance.Text.Trim().ToString()) - Comon.ConvertToDecimalPrice(txtNetAmount.Text.Trim().ToString());
                    rptForm.Parameters["NetTotal"].Value = txtNetAmount.Text.Trim().ToString();
                }
                else if (Comon.cInt(cmbMethodID.EditValue) == 3)
                {

                    rptForm.Parameters["CashTotal"].Value = 0;
                    rptForm.Parameters["NetTotal"].Value = lblNetBalance.Text.Trim().ToString();

                }
                else
                {

                    rptForm.Parameters["NetTotal"].Value = 0;
                    rptForm.Parameters["CashTotal"].Value = lblNetBalance.Text.Trim().ToString();


                }

                rptForm.Parameters["MethodName"].Value = "فاتورة مبيعات " + cmbMethodID.Text.Trim().ToString();
                rptForm.Parameters["VATCOMPANY"].Value = MySession.VAtCompnyGlobal;
                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                rptForm.Parameters["footer"].Value = MySession.footer;
                rptForm.Parameters["Notes"].Value = txtNotes.Text.Trim().ToString();

                rptForm.Parameters["CustomerMobile"].Value = txtMobile.Text.ToString();
                string Date = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
                int year = Convert.ToInt32(Date.Substring(0, 4));
                int month = Convert.ToInt32(Date.Substring(4, 2));
                int day = Convert.ToInt32(Date.Substring(6, 2));
                DateTime tempDate = new DateTime(year, month, day);
                rptForm.Parameters["HDate"].Value = Comon.ConvertFromEngDateToHijriDate(tempDate).Substring(0, 10);
                rptForm.Parameters["NumbToWord"].Value = Lip.ToWords(Convert.ToDecimal(lblNetBalance.Text.Trim().ToString()), 2);

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

                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["ItemName"] = gridView2.GetRowCellValue(i, "ArbItemName").ToString();
                    if (Comon.cInt(cmbLanguagePrint.EditValue) == 2)
                        row["ItemName"] = gridView2.GetRowCellValue(i, "EngItemName").ToString();
                    else if (Comon.cInt(cmbLanguagePrint.EditValue) == 3)
                        row["ItemName"] = gridView2.GetRowCellValue(i, "EngItemName").ToString() + gridView2.GetRowCellValue(i, "ArbItemName").ToString();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();

                    row["SizeName"] = gridView2.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView2.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView2.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView2.GetRowCellValue(i, "Discount").ToString();
                    row["AdditionalValue"] = gridView2.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView2.GetRowCellValue(i, "Net").ToString();
                    row["SalePrice"] = gridView2.GetRowCellValue(i, "SalePrice").ToString();
                    row["Description"] = gridView2.GetRowCellValue(i, "Description").ToString();
                    row["Bones"] = gridView2.GetRowCellValue(i, "Bones").ToString();
                    row["ExpiryDate"] = Comon.ConvertSerialToDate(Comon.ConvertDateToSerial(gridView2.GetRowCellValue(i, "ExpiryDate").ToString()).ToString());
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;


                rptForm.DataMember = ReportName;
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = true;
                /******************** Report Binding ************************/
                if (gridView2.Columns["Description"].Visible == true)
                {

                    subreport.ReportSource = ReportComponent.CompanyHeaderLand2();
                }
                else
                {
                    subreport.ReportSource = ReportComponent.CompanyHeader();
                }
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

                //if (MySession.PrintBuildPill == 1)
                //{
                //    PrintBill();
                //}
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

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
                rptForm.Parameters["MethodName"].Value = MethodName + "-" + OrderTypeArb;
                rptForm.Parameters["TheTime"].Value = Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":", "").Trim());
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

        private void pnlDeliverContol_Paint(object sender, PaintEventArgs e)
        {

        }
        #endregion
        #region Numbers
        private void btnDelivery_Click(object sender, EventArgs e)
        {
           
            //// 255, 128, 0 ,LightSeaGreen,Green
            ////btnDelivery.BackColor = Color.LightYellow;
            ////btnDelivery.ForeColor = Color.Black;
            ////btnLocal.BackColor = Color.Transparent;
            ////btnLocal.ForeColor = Color.Black;
            //OrderType = "3";
            //OrderTypeArb = "توصيل";
            //OrderTypeEng = "Delivery";
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            DoPrint();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            
            confirm = new confirmSaving(lblNetBalance.Text, lblBalanceSum.Text, lblRequireAmmut.Text, lblCustomerName.Text, txtCustomerID.Text, txtMobile.Text, lblBalanceSum.BackColor);
            confirm.btnSave.Click += confirmSave_Click;
            confirm.btnClose.Click += confirmClose_Click;
            FlyoutAction action = new FlyoutAction();
            FlyoutProperties properties = new FlyoutProperties();
            properties.Style = FlyoutStyle.Popup;
            FlyoutDialog.Show(this.ParentForm, confirm, action, properties);

        }

        private void confirmSave_Click(object sender, EventArgs e)
        {
            if (Comon.ConvertToDecimalPrice(confirm.txtNetAmount.Text) <= 0 && confirm.cmbMethodID == 5)
            {
                confirm.txtNetAmount.Focus();
                confirm.txtNetAmount.ToolTip = "مبلغ الشبكة = 0 ";
                Validations.ErrorText(confirm.txtNetAmount, confirm.txtNetAmount.ToolTip);
                return;

            }
         
            MethodID = confirm.MethodID;
            MethodName = confirm.MethodName;
            cmbNetType.EditValue = confirm.cmbNetType.EditValue;
            txtNetProcessID.Text = confirm.txtNetProcessID.Text;
            txtNetAmount.Text = confirm.txtNetAmount.Text;
            txtNotes.Text = confirm.textEdit1.Text;
            txtInsuranceAmmount.Text = confirm.txtCustomePaidAmount.Text;
            DoSave();



        }



        public void DoSave()
        {
            try
            {

                if (Comon.cInt(txtCustomerID.Text) >= 0)
                {

                    FormUpdate = true;
                    FormAdd = true;
                    FormView = true;

                    if (!Validations.IsValidForm(this))
                        return;
                    if (!IsValidGrid())
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

                    Application.DoEvents();
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                  
                    Save();





                }
                else
                {
                    txtCustomerID.Focus();
                    txtCustomerID.ToolTip = "يجب اختيار عميل  ";
                    Validations.ErrorText(txtCustomerID, txtCustomerID.ToolTip);
                    return;

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

        private void Save()
        {

            gridView2.MoveLastVisible();
            if (DiscountCustomer != 0)
            {
                txtDiscountPercent.Text = DiscountCustomer.ToString();
                txtDiscountPercent_Validating(null, null);
            }
            CalculateRow();
            gridView2.FocusedColumn = gridView2.VisibleColumns[1];

            var dk = Lip.GetServerDate();
            txtInvoiceDate.Text = dk;
            txtWarningDate.Text = dk;
            txtCheckSpendDate.Text = dk;
            Sales_SalesInvoiceMaster objRecord = new Sales_SalesInvoiceMaster();

            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;

            invoiceNo = Sales_SaleInvoicesDAL.GetNewID(MySession.GlobalBranchID, MySession.GlobalFacilityID, MySession.UserID).ToString();
           // txtDailyID.Text = Sales_SaleInvoicesDAL.GetNewDialyID(MySession.GlobalBranchID, MySession.GlobalFacilityID, MySession.UserID).ToString();
            objRecord.InvoiceID = Comon.cInt(invoiceNo);
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.DailyID = Comon.cInt(txtDailyID.Text);
          

            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.NetType = Comon.cDbl(cmbNetType.EditValue);

            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);

            objRecord.CustomerName = "";//txtCustomerName.Text.Trim();


            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);

            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SellerID = Comon.cInt(txtSellerID.Text);
            double customerID = Comon.cDbl(txtAccountID.Text);

            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Sale Cashar Invoice" : "فاتوره كاشر مبيعات ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Sale Cashar Invoice" : " فاتوره كاشر مبيعات "));
            objRecord.Notes = txtNotes.Text;

            //Account
            lblDebitAccountID.Text = customerID.ToString();
            objRecord.DebitAccount = customerID;
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            objRecord.DiscountDebitAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
            objRecord.CheckAccount = Comon.cDbl(lblChequeAccountID.Text);
            objRecord.NetAccount = Comon.cDbl(lblNetAccountID.Text);
            objRecord.InsuranceAmmount = Comon.cDbl(txtInsuranceAmmount.Text);

            objRecord.AdditionalAccount = Comon.cDbl(lblAdditionalAccountID.Text);
            objRecord.NetProcessID = txtNetProcessID.Text;
            objRecord.CheckID = "";// txtCheckID.Text;
            objRecord.VATID = txtVatID.Text;

            //Date
            objRecord.CheckSpendDate = Comon.ConvertDateToSerial(txtCheckSpendDate.Text).ToString();
            objRecord.WarningDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            objRecord.ReceiveDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();

            //Ammount

            objRecord.NetAmount = 0;// Comon.cDbl(txtNetAmount.Text);
            objRecord.DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = Math.Abs((Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text)) - Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text));
            objRecord.AdditionaAmountTotal = Comon.ConvertToDecimalPrice(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Math.Abs(Comon.ConvertToDecimalPrice(lblNetBalance.Text) - Comon.ConvertToDecimalPrice(txtInsuranceAmmount.Text));

            objRecord.OrderType = OrderType;
            double InsurentForNet = 0;
            cmbMethodID.EditValue = 2;
            objRecord.MethodeID = 2;
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
                    returned.StoreID = Comon.cInt(txtStoreID.Text);
                    returned.Discount = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount").ToString());
                    returned.ItemImage = null;
                    returned.ExpiryDateStr = Comon.ConvertDateToSerial(gridView2.GetRowCellValue(i, "ExpiryDate").ToString().Substring(0, 10));
                    returned.CostPrice = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());
                    returned.AdditionalValue = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue").ToString());
                    returned.Net = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net").ToString());
                    returned.Total = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Total").ToString());
                    returned.DIAMOND_W = Comon.cInt(gridView2.GetRowCellValue(i, "DIAMOND_W").ToString());

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

            if (listreturned.Count > 0)
            {
                objRecord.SaleDatails = listreturned;
             
                string Result = Sales_SaleInvoicesDAL.InsertUsingXML(objRecord, IsNewRecord);
                SplashScreenManager.CloseForm(false);

                if (IsNewRecord == true)
                {

                    if (Result != "0")
                    {
                        IsNewRecord = false;
                        IdPrint = true;
                        invoiceNo = Result;

                       // invoiceNo = Result.Split('-')[0];
                        //txtDailyID.Text = Result.Split('-')[1];
                        //  Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        if (Comon.cDbl(txtInsuranceAmmount.Text) > 0)
                        {
                            switch (MethodID)
                            {
                                case (1):
                                    objRecord.DocumentID = SaveRecipt(Comon.cDbl(txtInsuranceAmmount.Text), customerID);
                                    objRecord.RegistrationNo = 0;
                                    break;
                                case (3):
                                    objRecord.DocumentID = SaveRecipt(Comon.cDbl(txtInsuranceAmmount.Text) - Comon.cDbl(txtNetAmount.Text), customerID);
                                    objRecord.RegistrationNo = SaveVouchers1(Comon.cDbl(txtNetAmount.Text), customerID);
                                    break;
                                case (2):
                                    objRecord.DocumentID = 0;
                                    objRecord.RegistrationNo = SaveVouchers1(Comon.cDbl(txtInsuranceAmmount.Text), customerID);
                                    break;


                            }

                            //double disc = Math.Truncate(Comon.cDbl(txtInsuranceAmmount.Text)/100);
                            //if(disc>0)
                            //    SaveVouchersDiscount(disc*30, customerID);


                               
                        }
                        try
                        {

                            if (MySession.PrintModel == 1 || (checkA4.Checked == false && checkDt.Checked == true))
                                DoPrint();// كاشير

                            if (checkA4.Checked == true)
                                DoPrintA4(); // A4

                            if (checkDt.Checked == true)
                                PrintDot(); // نقطي

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

        private int SaveRecipt(double CreditAmount, double AccountID)
        {

            Acc_ReceiptVoucherMaster objRecord = new Acc_ReceiptVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.ReceiptVoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();

            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());

            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Receipt Voucher" : "سند القبض ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Receipt Voucher" : "سند القبض "));
            objRecord.Notes = "استلام مبلغ من عميل  : ";
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
            if (row.Length > 0)
            {
                objRecord.DebitAccountID = Comon.cDbl(row[0]["AccountID"].ToString());

            }
          
            objRecord.DiscountAccountID = Comon.cDbl(lblDiscountDebitAccountID.Text);
            //Ammount
            objRecord.DiscountAmount = Comon.cDbl(0);
            objRecord.DebitAmount = CreditAmount;

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
            //    objRecord.ReceiptVoucherID = VoucherID;
            //    objRecord.EditUserID = UserInfo.ID;
            //    objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
            //    objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            //    objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            //}
            //image


            objRecord.SpendImage = DefaultImage();


            Acc_ReceiptVoucherDetails returned;
            List<Acc_ReceiptVoucherDetails> listreturned = new List<Acc_ReceiptVoucherDetails>();

            returned = new Acc_ReceiptVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FACILITYID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            returned.ReceiptVoucherID = 0;
            returned.CreditAmount = Math.Abs(CreditAmount);
            returned.Discount = Comon.cDbl(0);
            returned.Declaration = "استلام مبلغ من عميل  : ";
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            int Result = 0;

            if (listreturned.Count > 0)
            {
                objRecord.ReceiptVoucherDetails = listreturned;
                Result = ReceiptVoucherDAL.InsertUsingXMLRecipt(objRecord, MySession.UserID);


                SplashScreenManager.CloseForm(false);



            }
            else
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }
            return Result;
        }
        private int SaveVouchers1(double CreditAmount, double AccountID)
        {



            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();

            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());

            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = "استلام مبلغ من عميل  : ";
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


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();



            returned = new Acc_VariousVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            returned.VoucherID = 0;
            returned.Credit = Math.Abs(CreditAmount);
            returned.Debit = 0;
            returned.Declaration = "استلام مبلغ من عميل  : ";
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            returned = new Acc_VariousVoucherDetails();
            returned.ID = 1;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblNetAccountID.Text);
            returned.VoucherID = 0;
            returned.Credit = 0;
            returned.Debit = Math.Abs(CreditAmount);
            returned.Declaration = "استلام مبلغ من عميل  : ";
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
        private int SaveVouchersDiscount(double CreditAmount, double AccountID)
        {



            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();

            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());

            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = "خصم مكتسب للعميل  ";
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


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();



            returned = new Acc_VariousVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            returned.VoucherID = 0;
            returned.Credit = Math.Abs(CreditAmount);
            returned.Debit = 0;
            returned.Declaration = "خصم مكتسب للعميل  ";
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            returned = new Acc_VariousVoucherDetails();
            returned.ID = 1;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblDiscountDebitAccountID.Text);
            returned.VoucherID = 0;
            returned.Credit = 0;
            returned.Debit = Math.Abs(CreditAmount);
            returned.Declaration = "خصم مكتسب للعميل  ";
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
        private void confirmClose_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{ESC}");
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
          
            //  txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            //simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //   showCustomers(false, 0);
            //  pnlDeliverContol.Visible = false;
            //txtCustomerName.Visible = true;
            txtDiscountOnTotal.Visible = true;
            txtVatID.Visible = true;
            lblAdditionaAmmount.Visible = true;
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
           // pnlNetControl.Visible = true;
          //  simpleButton12.Visible = false;
            //  txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            //simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //showCustomers(false, 0);
            cmbMethodID.EditValue = 3;
            cmbMethodID_EditValueChanged(null, null);
        
            //btnNet.Appearance.BackColor = Color.Goldenrod;
            //btnNet.Appearance.BackColor2 = Color.White;
            //btnNet.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            //btnNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة" : "Net");
            MethodID = 2;
            btnSix.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
         //   btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
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
         
            /////////////////////////////////////////////////
            showCustomers(false, 0);
            cmbMethodID.EditValue = 5;
            cmbMethodID_EditValueChanged(null, null);
            //btnCash_Net.Appearance.BackColor = Color.Goldenrod;
            //btnCash_Net.Appearance.BackColor2 = Color.White;
            //btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            //btnCash_Net.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            //btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
          
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
#region pluseCode
            //try
            //{
            //    decimal value;
            //    FocusedControl = GetLastIndexFocusedControl();
            //    if (FocusedControl == null) return;
            //    // if (FocusedControl.Trim() == gridControl.Name)
            //    if (1 == 1)
            //    {
            //        if (gridView2.FocusedColumn == null) return;
            //        var obj = gridView2.GetFocusedValue();
            //        if (obj == null)
            //        {
            //            if (gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0)
            //                return;
            //            gridView2.SetFocusedValue(Comon.ConvertToDecimalQty(1.ToString("N" + MySession.GlobalPriceDigits)));
            //            var dr = dtPriceItemOffers.Select("((FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " )AND(FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " )) or((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " ))OR ((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " ) ");

            //            if (gridView2.GetFocusedRowCellValue("Description").Equals("IsPercent"))
            //            {

            //                decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["Height"]).ToString()));
            //                decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
            //                gridView2.SetFocusedRowCellValue(gridView2.Columns["Discount"], total);

            //            }

            //            GetNewOffers(dr, gridView2.GetFocusedRowCellValue(gridView2.Columns["BarCode"]).ToString(), Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["QTY"]).ToString()));



            //            CalculateRow();
            //        }
            //        else
            //        {
            //            if (gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0)
            //                return;
            //            value = Comon.ConvertToDecimalQty(strQty);
            //            if (value == 0)
            //                value = 1;
            //            decimal QtyValue = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY").ToString());
            //            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], Comon.ConvertToDecimalQty(value.ToString("N" + MySession.GlobalPriceDigits)));

            //            var dr = dtPriceItemOffers.Select("((FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " )AND(FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " )) or((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " ))OR ((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " ) ");

            //            if (gridView2.GetFocusedRowCellValue("Description").Equals("IsPercent"))
            //            {

            //                decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["Height"]).ToString()));
            //                decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["SalePrice"])) * value * Comon.ConvertToDecimalPrice(PercentAmount / 100));
            //                gridView2.SetFocusedRowCellValue(gridView2.Columns["Discount"], total);

            //            }

            //            GetNewOffers(dr, gridView2.GetFocusedRowCellValue(gridView2.Columns["BarCode"]).ToString(), value);






            //            CalculateRow();
            //        }
            //    }
            //    strQty = "";
            //    txtTotal.Text = "";
            ////    simpleButton1_Click_2(null, null);
            //}
            //catch { };
#endregion
            if (strQty.Length < 1) return;
            strQty = strQty.Remove(strQty.Length - 1, 1);
            txtTotal.Text = strQty;
        }
        private void simpleButton1_Click_2(object sender, EventArgs e)
        {
            try
            {
                if ((gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0) || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER1") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER0") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER2") || Comon.cInt(gridView2.GetFocusedRowCellValue("DIAMOND_W").ToString()) == 1)
                    return;
                txtTotal.Text = "";
                strQty = "";
            }
            catch { }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (strQty.Length < 1) return;
            strQty = strQty.Remove(strQty.Length - 1, 1);
            txtTotal.Text = strQty;
        }
        private void btnNine_Click(object sender, EventArgs e)
        {
            strQty = strQty + "9";
            txtTotal.Text = strQty;
        }
        private void btnEight_Click(object sender, EventArgs e)
        {
            strQty = strQty + "8";
            txtTotal.Text = strQty;
        }
        private void btnSeven_Click(object sender, EventArgs e)
        {
            strQty = strQty + "7";
            txtTotal.Text = strQty;
        }

        private void btnDot_Click(object sender, System.EventArgs e)
        {
            strQty = strQty + ".";
            txtTotal.Text = strQty;
        }
        private void btnThree_Click(object sender, EventArgs e)
        {
            strQty = strQty + "3";
            txtTotal.Text = strQty;

        }
        private void btnFour_Click(object sender, EventArgs e)
        {
            strQty = strQty + "4";
            txtTotal.Text = strQty;
        }
        private void btnFive_Click(object sender, EventArgs e)
        {
            strQty = strQty + "5";
            txtTotal.Text = strQty;
        }
        private void btnSix_Click(object sender, EventArgs e)
        {
            strQty = strQty + "6";
            txtTotal.Text = strQty;
        }
        private void btnTow_Click(object sender, EventArgs e)
        {
            strQty = strQty + "2";
            txtTotal.Text = strQty;
        }
        private void btnOne_Click(object sender, EventArgs e)
        {
            strQty = strQty + "1";
            txtTotal.Text = strQty;
        }
        private void btnZero_Click(object sender, EventArgs e)
        {
            strQty = strQty + "0";
            txtTotal.Text = strQty;
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
            btnMainGroup_Click(null, null);

        }
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
            //if (FormView == true)
            //    ReadRecord(Comon.cLong(txtInvoiceID.Text));
            //else
            //{
            //    Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
            //    return;
            //}

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
                string strSql;
                DataTable dt;
                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,Mobile,VATID,SpecialDiscount FROM Sales_Customers Where    CustomerID =" + txtCustomerID.Text;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {

                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString() + "-" + dt.Rows[0]["Mobile"].ToString();


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
                        //if (Comon.cLong(dt.Rows[0]["SpecialDiscount"]) > 0)
                        //    DiscountCustomer = Comon.cInt(dt.Rows[0]["SpecialDiscount"].ToString());
                        //if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                        //{
                        //    lblDebitAccountID.Text = txtCustomerID.Text;
                        //    lblDebitAccountName.Text = lblCustomerName.Text;

                        //    if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                        //    {
                        //        chkForVat.Checked = true;
                        //        txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                        //    }
                        //    else
                        //    {

                        //        txtVatID.Text = "";
                        //        if (chkForVat.Checked == false)
                        //            chkForVat.Checked = false;
                        //    }
                        //}
                    }
                    else
                    {
                        lblCustomerName.Text = "";
                        txtCustomerID.Text = "";
                        txtVatID.Text = "";
                    }
                    // txtAddressID_Validating(null, null);

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
                        lblRemaindAmount.Text = Comon.ConvertToDecimalPrice((Comon.cDbl(txtPaidAmount.Text) - Comon.cDbl(lblNetBalance.Text))).ToString();

                    }
                    else if (MethodID == 3)
                    {
                        lblRemaindAmount.Text = Comon.ConvertToDecimalPrice(((Comon.cDbl(txtPaidAmount.Text) + Comon.cDbl(txtNetAmount.Text)) - Comon.cDbl(lblNetBalance.Text))).ToString();

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
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();
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
            lblNetAccountID.Text = cmbNetType.EditValue.ToString();
            lblNetAccountID_Validating(null, null);

            if (Comon.cInt(cmbMethodID.EditValue) == 3)
            {
                lblDebitAccountID.Text = cmbNetType.EditValue.ToString();
                lblDebitAccountID_Validating(null, null);
            }
        }
        private void cmbMethodID_EditValueChanged(object sender, EventArgs e)
        {

            int value = Comon.cInt(cmbMethodID.EditValue.ToString());
            if (value == 0)
                return;
            try
            {
              //  lblNetProcessID.Visible = false;
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
               // lblNetAmount.Visible = false;
               // lblnetType.Visible = false;
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
                 //   lblBankName.Visible = false;
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
                  //  lblBankName.Visible = false;
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
                  //  lblBankName.Visible = false;
                    cmbBank.Visible = false;

                   // lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = false;
                   // lblNetAmount.Visible = false;
                   // lblnetType.Visible = true;
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

                    //lblNetProcessID.Visible = false;
                    txtNetProcessID.Visible = false;
                    txtNetAmount.Visible = false;
                   // lblNetAmount.Visible = false;
                    //lblnetType.Visible = false;
                    cmbNetType.Visible = false;

                    lblCheckSpendDate.Visible = true;
                    txtCheckSpendDate.Visible = true;
                    //lblCheckID.Visible = true;
                    //txtCheckID.Visible = true;
                   // lblBankName.Visible = true;
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
                    //lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = true;
                    //lblNetAmount.Visible = true;
                    //lblnetType.Visible = true;
                    cmbNetType.Visible = true;
                   // lblBankName.Visible = false;
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
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);

            this.btnEight.Click += new System.EventHandler(this.btnEight_Click);
            this.btnNine.Click += new System.EventHandler(this.btnNine_Click);
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //this.btnBackSpace.Click += new System.EventHandler(this.btnBackSpace_Click);
            //this.btnPendOrder.Click += new System.EventHandler(this.btnPendOrder_Click);
            //this.btnGetPendingOrder.Click += new System.EventHandler(this.btnGetPendingOrder_Click);
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            //this.btnCash.Click += new System.EventHandler(this.btnCash_Click);
            //this.btnNet.Click += new System.EventHandler(this.btnNet_Click);
            //this.btnCash_Net.Click += new System.EventHandler(this.btnCash_Net_Click);
            //this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
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
          //  this.btnDelivery.Click += new System.EventHandler(this.btnDelivery_Click);
      

        }


        private void txtPaidAmount_EditValueChanged(object sender, EventArgs e)
        {

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
           
            gridView2.Focus();
            gridView2.MoveLastVisible();
            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView2.FocusedColumn = gridView2.VisibleColumns[1];
        }
        private void showCustomers(bool p, int f)
        {

            //txtCustomerName.Text = "";
            txtCustomerID.Text = "";
            lblCustomerName.Text = "";

            txtVatID.Text = "";
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
            pnlDeliverContol.Visible = false;
           // btnLocal.Appearance.BorderColor = Color.Green;
          
            //btnLocal.BackColor = Color.Transparent;
            OrderType = "2";
            RefreshOffers();
            OrderTypeArb = "سـفري";
            OrderTypeEng = "TakeOut";
        }


        private void btnLocal_Click(object sender, EventArgs e)
        {
            OrderType = "1";
            //btnLocal.BackColor = Color.LightYellow;//'LightYellow
            //btnLocal.ForeColor = Color.Black;
            //btnTakeAway.BackColor = Color.Transparent;
            //btnTakeAway.BackColor = Color.Transparent;
          //  btnLocal.Appearance.BorderColor = Color.Orange ;
           
            //btnTakeAway.Appearance.BorderColor = Color.FromArgb(255, 128, 0); //(255, 128, 0)
            pnlDeliverContol.Visible = false;
            RefreshOffers();
            OrderTypeArb = "محلي";
            OrderTypeEng = "Dine In";
        }

        private void btnHangerStation_Click(object sender, EventArgs e)
        {
            //btnHangerStation.BackColor = btnPlus.BackColor;
            //btnHangerStation.BackColor = Color.LightSteelBlue;
            //btnHangerStation.BackColor = btnPlus.BackColor;
            //btnHangerStation.BackColor = btnPlus.BackColor;
          
            pnlDeliverContol.Visible = false;
            OrderType = "4";
            RefreshOffers();
            OrderTypeArb = "هنجر ستيشن";
            OrderTypeEng = "Hanger Station";
           
        }

        private void txtAddressID_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                lblDebit.Text = "0";
                lblCredit.Text = "0";
                lblBalanceSum.Text = "0";

                var sr = "SELECT     Sales_Customers.AccountID,   Sales_Customers.CustomerID,Sales_Customers.Balance, Sales_Customers.ArbName, Sales_Customers.Tel, Sales_Customers.Mobile, Sales_CustomersAddress.ID, Sales_CustomersAddress.Location, Sales_CustomersAddress.Street, "
                    + "     Sales_CustomersAddress.Building, Sales_CustomersAddress.ArbName as Notes, Sales_CustomersAddress.Floor, Sales_CustomersAddress.Apartment, HR_District.ArbName AS DistrictName, HR_Street.ArbName AS StreetName, HR_District.TransCost"
  + "  FROM            HR_District RIGHT OUTER JOIN"
        + "                     Sales_CustomersAddress ON HR_District.ID = Sales_CustomersAddress.Location LEFT OUTER JOIN"
             + "                HR_Street ON Sales_CustomersAddress.Street = HR_Street.ID RIGHT OUTER JOIN"
               + "              Sales_Customers ON Sales_CustomersAddress.CustomerID = Sales_Customers.CustomerID  where Sales_Customers.Cancel=0 And  Sales_Customers.CustomerID=" + Comon.cInt(txtCustomerID.Text.Trim());
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
                    txtMobile.Text = dr.Rows[0]["Mobile"].ToString();
                    txtAccountID.Text = dr.Rows[0]["AccountID"].ToString();
                    if (Comon.cLong(txtAccountID.Text) > 0)
                    {
                        frmAccountStatement frm = new frmAccountStatement(Comon.cLong(txtAccountID.Text));
                        lblDebit.Text = frm.lblDebit.Text;
                        lblCredit.Text = frm.lblCredit.Text;
                        lblBalanceSum.Text = frm.lblBalanceSum.Text;
                        lblBalanceSum.BackColor = Color.Transparent;

                        if (Comon.cDec(lblDebit.Text) > Comon.cDec(lblCredit.Text))
                        {

                            lblBalanceSum.BackColor = Color.Red;


                        }
                    }


                }
                else
                {

                    lblAddressCustomerName.Text = "";
                    txtFloor.Text = "";
                    txtApartment.Text = "";
                    txtBuilding.Text = "";
                    txtAddressID.Text = "";

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
                ReportName = "rptSplitResturantInvoiceByItemsGroupsArb";
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
                                var row = dataTable.NewRow();


                                row["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                                row["ItemName"] = gridView2.GetRowCellValue(i, "EngSizeName").ToString() + " " + gridView2.GetRowCellValue(i, "EngItemName").ToString() + "\n" + gridView2.GetRowCellValue(i, "ArbSizeName").ToString() + " " + gridView2.GetRowCellValue(i, "ArbItemName").ToString() + "\n" + gridView2.GetRowCellValue(i, "extension").ToString() + "\n" + gridView2.GetRowCellValue(i, "Serials").ToString();
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
                    rptForm.Parameters["DailyID"].Value = txtDailyID.Text;
                    rptForm.Parameters["TableID"].Value = " ";
                    rptForm.Parameters["SaleDate"].Value = Lip.GetServerDate() + "-" + Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":", "").Trim());
                    rptForm.Parameters["OrderType"].Value = OrderTypeArb + "-" + OrderTypeEng;
                    rptForm.Parameters["CustomerName"].Value = "Waiter : " + UserInfo.SYSUSERARBNAME.ToString();

                    if (TableID > 0)
                    {
                        rptForm.Parameters["TableID"].Value = (UserInfo.Language == iLanguage.Arabic ? "طاولة رقم :" : "Table No : ") + TableID;
                    }
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
        void layoutView1_MouseDown(object sender, MouseEventArgs e)
        {

        }
        void layoutViewItem_MouseDown(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    LayoutView view = sender as LayoutView;
            //    var hiTinfo = view.CalcHitInfo(e.Location);
            //    if (hiTinfo.InField)
            //    {
            //        if (hiTinfo.Column.FieldName == "ItemImage" || hiTinfo.Column.FieldName == "ArbName" || hiTinfo.Column.FieldName == "RemainQty" || hiTinfo.Column.FieldName == "SalePrice")
            //        {

            //            if (Comon.ConvertToDecimalQty(view.GetRowCellValue(hiTinfo.RowHandle, "RemainQtyParent").ToString()) >= Comon.ConvertToDecimalQty(Comon.ConvertToDecimalQty(view.GetRowCellValue(hiTinfo.RowHandle, "PackingQty").ToString()) / Comon.ConvertToDecimalQty(view.GetRowCellValue(hiTinfo.RowHandle, "PackingQtyParent").ToString())) || MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
            //            {
            //                string ID = view.GetRowCellValue(hiTinfo.RowHandle, "BarCode").ToString();
            //                btnCilick(ID, 1);

            //                CalculateRow();
            //               // frmSize.Close();

            //            }
            //            else
            //            {

            //                Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
            //            }

            //            return;


            //        }
            //    }
            //}
            //catch { }

        }
        private void layoutViewItems_CardClick(object sender, DevExpress.XtraGrid.Views.Layout.Events.CardClickEventArgs e)
        {
            try
            {
                LayoutView view = sender as LayoutView;
                ColumnView cardView = sender as ColumnView;
                if (layoutView1.RowCount < 1)
                    return;

                //var hiTinfo = view.CalcHitInfo(e.Location);
                //  if (hiTinfo.InField)
                //  {
                //    if (hiTinfo.Column.FieldName == "ItemImage" || hiTinfo.Column.FieldName == "ArbName" || hiTinfo.Column.FieldName == "RemainQty" || hiTinfo.Column.FieldName == "SalePrice")
                //   {

                if (Comon.ConvertToDecimalQty(view.GetRowCellValue(cardView.FocusedRowHandle, "RemainQtyParent").ToString()) >= Comon.ConvertToDecimalQty(Comon.ConvertToDecimalQty(view.GetRowCellValue(cardView.FocusedRowHandle, "PackingQty").ToString()) / Comon.ConvertToDecimalQty(view.GetRowCellValue(cardView.FocusedRowHandle, "PackingQtyParent").ToString())) || MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
                {
                    string ID = view.GetRowCellValue(cardView.FocusedRowHandle, "BarCode").ToString();
                    btnCilick(ID, 1);

                    CalculateRow();
                    // frmSize.Close();

                }
                else
                {

                    Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
                }

                return;


                //   }
                //   }
            }
            catch { }

        }



        void SelectedLayoutView_VisibleRecordIndexChanged(object sender, LayoutViewVisibleRecordIndexChangedEventArgs e)
        {
            // LayoutView layoutView = sender as LayoutView;
            // LayoutViewInfo viewInfo = layoutView.GetViewInfo() as LayoutViewInfo;
            //bool  locked = false;
            //bool forward;
            //int visibleIndex=-1;
            // int visibleCards = viewInfo.VisibleCards.Count;
            // int firstRow = viewInfo.VisibleCards[0].VisibleRow;
            // int lastRow = viewInfo.VisibleCards[viewInfo.VisibleCards.Count - 1].VisibleRow;
            // int rowCount = lastRow - firstRow + 1;
            // int itemsInRow = visibleCards / rowCount;
            // if (locked) return;
            // locked = true;
            // forward = visibleIndex < (sender as LayoutView).VisibleRecordIndex ? true : false;
            // layoutView.VisibleRecordIndex = e.PrevVisibleRecordIndex;
            // if (forward)
            //     layoutView.VisibleRecordIndex += itemsInRow;
            // else
            //     layoutView.VisibleRecordIndex -= itemsInRow;
            // locked = false;
            // visibleIndex = layoutView.VisibleRecordIndex;
        }




        private void layoutView1_CustomFieldValueStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewFieldValueStyleEventArgs e)
        {
            ////  return;  // Painting the content of the focused card only if the LayoutView itself has the focus.
            //ColumnView view = sender as ColumnView;
            //if (view == null) return;
            //// if(view.get)
            //decimal count = Comon.ConvertToDecimalQty(view.GetRowCellValue(e.RowHandle, "RemainQty").ToString());
            ////   int IsSelect = Comon.cInt(view.GetRowCellValue(e.RowHandle, "IsSelect").ToString());
            ////ColumnView cardView = sender as ColumnView;
            //ColumnView cardView = sender as ColumnView;
            //if (cardView.FocusedRowHandle == e.RowHandle && cardView.IsFocusedView)// && cardView.FocusedColumn == e.Column)
            //{
            //    e.Appearance.BackColor = Color.Gold;
            //    e.Appearance.BackColor = Color.Gold;
            //    e.Appearance.BackColor2 = Color.Gold;
            //    e.Appearance.ForeColor = Color.Black;
            //    return;

            //}
            //if (MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
            //{

            //    e.Appearance.BackColor = Color.FromArgb(27, 96, 147);
            //    e.Appearance.BackColor2 = Color.FromArgb(27, 96, 147);
            //    e.Appearance.ForeColor = Color.Yellow;
            //    e.Appearance.Options.UseFont = true;
            //    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

            //    return;


            //}
            //if (count > 0)
            //{

            //    e.Appearance.BackColor = Color.FromArgb(27, 96, 147);
            //    e.Appearance.BackColor2 = Color.FromArgb(27, 96, 147);
            //    e.Appearance.ForeColor = Color.Yellow;
            //    e.Appearance.Options.UseFont = true;
            //    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            //}
            //else
            //{


            //    e.Appearance.BackColor = Color.FromArgb(253, 101, 0);
            //    e.Appearance.BackColor2 = Color.FromArgb(253, 101, 0);
            //    e.Appearance.ForeColor = Color.White;
            //    e.Appearance.Options.UseFont = true;
            //    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

            //}
            //if (cardView.FocusedRowHandle == e.RowHandle && cardView.FocusedColumn == e.Column)
            //{
            //    e.Appearance.BackColor = Color.Red;
            //    e.Appearance.BackColor = Color.Red;
            //    e.Appearance.BackColor2 = Color.Red;
            //    e.Appearance.ForeColor = Color.Black;

            //}
            //if (IsSelect > 0) {


            //    e.Appearance.BackColor = Color.Yellow;
            //    e.Appearance.BackColor2 = Color.Yellow;
            //    e.Appearance.ForeColor = Color.Black;
            //    e.Appearance.Options.UseFont = true;
            //    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

            //}

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
                for (int i = 0; i < gridView2.RowCount - 1; ++i)
                {
                    if (i == rowIndex)
                        if (gridView2.IsNewItemRow(rowIndex))
                            continue;
                        else
                        {
                            if (gridView2.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                            {
                                QtyIn = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"]));
                                if ( gridView2.GetRowCellValue(i, "Caliber").ToString() != "-1" && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER1") && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER0") && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER2") && Comon.cInt(gridView2.GetRowCellValue(i, "DIAMOND_W").ToString()) != 1)
                                {

                                    if (gridView2.IsNewItemRow(rowIndex))
                                        gridView2.DeleteRow(rowIndex);
                                    QtyIn = QtyIn + 1;

                                }
                                else continue;
                                gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], QtyIn);

                                if (gridView2.GetRowCellValue(i, "Description").Equals("IsPercent"))
                                {
                                    decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["Height"]).ToString()));
                                    decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                                    gridView2.SetRowCellValue(i, gridView2.Columns["Discount"], total);
                                    gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                    gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                                    flag = 1;
                                    GetNewOffers(itemGroup, Barcode, QtyIn);
                                    return;
                                }

                                else
                                {

                                    gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                    gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                                    flag = 1;
                                    GetNewOffers(itemGroup, Barcode, QtyIn);
                                    return;


                                }
                            }
                        }


                    if (gridView2.GetRowCellValue(i, "BarCode").Equals(Barcode.ToString()))
                    {
                        QtyIn = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"]));
                        if (  gridView2.GetRowCellValue(i, "Caliber").ToString() != "-1" && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER1") && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER0") && (gridView2.GetRowCellValue(i, "Description").ToString() != "ISOFFER2") && Comon.cInt(gridView2.GetRowCellValue(i, "DIAMOND_W").ToString()) != 1)
                        {
                            if (gridView2.IsNewItemRow(rowIndex))
                                gridView2.DeleteRow(rowIndex);
                            QtyIn = QtyIn + 1;
                        }
                        else continue;
                        gridView2.SetRowCellValue(i, gridView2.Columns["QTY"], QtyIn);

                        if (gridView2.GetRowCellValue(i, "Description").Equals("IsPercent"))
                        {
                            decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["Height"]).ToString()));
                            decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["SalePrice"])) * Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, gridView2.Columns["QTY"])) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                            gridView2.SetRowCellValue(i, gridView2.Columns["Discount"], total);
                            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                            gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                            flag = 1;
                            GetNewOffers(itemGroup, Barcode, QtyIn);
                            return;
                        }

                        else
                        {

                            gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                            gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                            flag = 1;
                            GetNewOffers(itemGroup, Barcode, QtyIn);
                            return;


                        }

                    }



                }

                GetNewOffers(itemGroup, Barcode, QtyInput);

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
                        AddnewItem(Barcode, Comon.ConvertToDecimalQty(1), "ISOFFER0");

                    }
                    else if (Comon.cInt(itemGroup[0]["IsGetSame"].ToString()) > 0 && !checkIfExist(Barcode, 1))
                    {

                        QtyOffers = Comon.ConvertToDecimalQty(itemGroup[0]["GetSameAmount"].ToString());
                        if (QtyIn >= QtyOffers)
                            AddnewItem(Barcode, Comon.ConvertToDecimalQty(itemGroup[0]["SetSameAmount"].ToString()), "ISOFFER1");


                    }

                    else if (Comon.cInt(itemGroup[0]["IsGetOnther"].ToString()) > 0 && !checkIfExist(itemGroup[0]["BarCode"].ToString(), 1))
                    {
                        QtyOffers = Comon.ConvertToDecimalPrice(itemGroup[0]["GetOntherAmount"].ToString());
                        if (QtyIn >= QtyOffers)
                            AddnewItem(itemGroup[0]["BarCode"].ToString(), Comon.ConvertToDecimalQty(itemGroup[0]["SetOntherAmount"].ToString()), "ISOFFER2");

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

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, UserInfo.Language == iLanguage.Arabic ? "تأكيد الخروج من شاشة الكاشير ؟" : "Do you want close POS ?");
            if (!Yes)
                return;
           // CLOSEOK = true;
            this.Close();
        }

        private void XtraForm22_LocationChanged(object sender, EventArgs e)
        {
            //this.Location = new Point(0, 0);
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {

           // indexGridView.MovePrevPage();
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
           // indexGridView.MoveNextPage();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            nextPage = 0;
            try
            {
                var filter = sourceGroup;
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
                }
            }
            catch { }

        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            if (filtering.Rows.Count <= countCard)
                return;
            nextPage = +countCard;
            try
            {
                var filter = sourceGroup;
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

            cls.SQLStr = "SELECT   ID as الرقم, ArbName as [اسم النوع] FROM  Res_OrderType  WHERE ID >3  ";

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT   ID as ID, EngName as [Type Name] FROM Res_OrderType  WHERE ID >3   ";

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
                           // btnOrderType.Text = lblOrderType.Text;
                        }
                        else
                        {
                            OrderType = "1";
                            OrderTypeArb = "محلي";
                            OrderTypeEng = "local";
                         //   btnOrderType.Text = lblOrderType.Text;

                        }
                    }
                    catch
                    {

                        OrderType = "1";
                        OrderTypeArb = "محلي";
                        OrderTypeEng = "local";
                       // btnOrderType.Text = lblOrderType.Text;

                    }




                }
                RefreshOffers();
            }
        }

        private void txtInvoiceDate_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            ctCalculater ctCustomers = new ctCalculater();
            ctCustomers.btnPlus.Click += plusPaidAmount;
            // ctCustomers.btnClose.Click += simpleButton11111_Click;
            FlyoutAction action = new FlyoutAction();

            FlyoutProperties properties = new FlyoutProperties();

            properties.Style = FlyoutStyle.Popup;

            FlyoutDialog.Show(this, ctCustomers, action, properties);
        }

        private void plusPaidAmount(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            txtPaidAmount.Text = btn.Tag.ToString();
            txtPaidAmount_Validating(null, null);
            SendKeys.Send("{ESC}");

        }
        private void XtraForm22_FormClosing(object sender, FormClosingEventArgs e)
        {
           // e.Cancel = !CLOSEOK;
        }
        private void simpleButton12_Click_1(object sender, EventArgs e)
        {
            ctCalculater ctCustomers = new ctCalculater();
            ctCustomers.btnPlus.Click += plusNetAmount;
            // ctCustomers.btnClose.Click += simpleButton11111_Click;
            FlyoutAction action = new FlyoutAction();

            FlyoutProperties properties = new FlyoutProperties();

            properties.Style = FlyoutStyle.Popup;

            FlyoutDialog.Show(this, ctCustomers, action, properties);
        }
        private void plusNetAmount(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            txtNetAmount.Text = btn.Tag.ToString();
            //txtPaidAmount_Validating(null, null);
            SendKeys.Send("{ESC}");
        }
        private void simpleButton13_Click(object sender, EventArgs e)
        {
           
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
           

        }

        private void ReturnSupnesion_Click(object sender, EventArgs e)
        {
            try
            {
                SimpleButton btn = sender as SimpleButton;

                detials(Comon.cLong(btn.Tag.ToString()), 1);
            }
            catch { }
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
           

        }

        private void gridControl2_Click_1(object sender, EventArgs e)
        {

        }

        private void btnTables_Click(object sender, EventArgs e)
        {
            try
            {
                //CalculateRow();
                gridView2.MoveLast();
                if (gridView2.DataRowCount > 0 && OrderType == "1")
                {

                    ctCalculater ctCustomers = new ctCalculater();
                    ctCustomers.btnPlus.Click += plusTableNO;
                    // ctCustomers.btnClose.Click += simpleButton11111_Click;
                    FlyoutAction action = new FlyoutAction();

                    FlyoutProperties properties = new FlyoutProperties();

                    properties.Style = FlyoutStyle.Popup;

                    FlyoutDialog.Show(this, ctCustomers, action, properties);


                }

                else
                {
                    
                }

            }
            catch { }


        }

        private void plusTableNO(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;

            TableID = Comon.cInt(btn.Tag.ToString());
            //txtPaidAmount_Validating(null, null);
            SendKeys.Send("{ESC}");
        }
      
        private void btnOrder_Click(object sender, EventArgs e)
        {
           

        }

        private void indexGridControl_Click(object sender, EventArgs e)
        {

        }

        private void panelControl12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelControl14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelControl12_Click(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblOrderType_Click(object sender, EventArgs e)
        {

        }

        private void lblInvoiceTotal_Click(object sender, EventArgs e)
        {

        }

        private void layoutView1_CustomDrawCardFieldValue(object sender, RowCellCustomDrawEventArgs e)
        {
            //LayoutView cardView = sender as LayoutView;
            //if (cardView.FocusedRowHandle == e.RowHandle && cardView.FocusedColumn == e.Column) {
            //    e.Appearance.BackColor = Color.Red;
            //    e.Appearance.BackColor = Color.Red;
            //    e.Appearance.BackColor2 = Color.Red;
            //    e.Appearance.ForeColor = Color.Black;

            //}

        }

        private void layoutView1_MouseUp(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    ColumnView cardView = sender as ColumnView;
            //    if (layoutView1.RowCount < 1)
            //        return;
            //    //var hiTinfo = layoutView1.CalcHitInfo(e.Location);
            //    //if (hiTinfo.InField)
            //    //{


            //        //if (hiTinfo.Column.FieldName == "ItemImage" || hiTinfo.Column.FieldName == "ItemName" || hiTinfo.Column.FieldName == "RemainQty" || hiTinfo.Column.FieldName == "SalePrice")
            //        //{
            //            long ID = Comon.cLong(layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "ItemID").ToString());
            //            //  layoutView1.SetRowCellValue(hiTinfo.RowHandle, "IsSelect", 0);
            //            // frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;
            //            // FlyoutAction action = new FlyoutAction();

            //            // FlyoutProperties properties = new FlyoutProperties();
            //            //    properties.Style = FlyoutStyle.Popup;
            //            //Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + dt.Rows[0]["BarCode"].ToString() + "'," + Comon.cInt(txtStoreID.Text) + ") AS RemainQty")).Rows[0]["RemainQty"].ToString()).ToString("N" + MySession.GlobalPriceDigits));
            //            var srSize = "   SELECT dbo.RemindQtyStock(BarCode, " + Comon.cInt(txtStoreID.Text) + "," + Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString() + ") AS RemainQty  ," + layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "RemainQty").ToString() + " as RemainQtyParent,0 as PackingQtyParent "
            //          + ",  Stc_Items.ArbName as ItemNAme,CONVERT(VARBINARY(MAX), '0xAAFF')  as ItemImage  ,  Stc_ItemUnits.BarCode,Stc_ItemUnits.PackingQty, Stc_ItemUnits.SalePrice, Stc_SizingUnits.ArbName , Stc_ItemsGroups.Notes "
            //     + " FROM            Stc_ItemUnits LEFT OUTER JOIN"
            //      + "              Stc_Items ON Stc_ItemUnits.ItemID = Stc_Items.ItemID  LEFT OUTER JOIN "
            //              + "              Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID   LEFT OUTER JOIN   Stc_ItemsGroups   ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID  "
            //    + " WHERE        (Stc_ItemUnits.ItemID =" + ID + ")     order by Stc_ItemUnits.PackingQty Asc ";

            //            if (Comon.ConvertToDecimalQty(layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "RemainQty").ToString()) <= 0 && MySession.GlobalWayOfOutItems != "AllowOutItemsWithOutBalance")
            //            {

            //                Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
            //                return;
            //            }
            //            var dtSize = Lip.SelectRecord(srSize);
            //            if (dtSize.Rows.Count < 1)
            //                return;
            //            else if (dtSize.Rows.Count == 1)
            //            {

            //                if (Comon.ConvertToDecimalQty(dtSize.Rows[0]["RemainQty"].ToString()) > 0 || MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
            //                {
            //                    btnCilick(dtSize.Rows[0]["BarCode"].ToString(), 1);
            //                    CalculateRow();

            //                }
            //                else
            //                {

            //                    Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
            //                }

            //                return;
            //            }

            //            frmSize = new frmSizeItem(dtSize);
            //            frmSize.layoutView1.MouseUp += layoutViewItem_MouseDown;
            //            frmSize.layoutView1.CustomFieldValueStyle += layoutViewItems_CustomFieldValueStyle;
            //            layoutViewItems_CustomFieldValueStyle(null, null);
            //            // frmSize.Location.X = gridControl2.Location.X;
            //            frmSize.Location = new Point(gridControl2.Location.X + 520, gridControl2.Location.Y + 100);
            //            //  frmSize.DesktopLocation = new Point(1, 1);

            //            //  FlyoutDialog.Show(this, frmSize, action, properties);




            //            frmSize.ShowDialog();

            //    }//


            //   // }//
            //}
            //catch { }
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

        private void layoutView1_Click_2(object sender, EventArgs e)
        {
            //try
            //{
            //    ColumnView cardView = sender as ColumnView;
            //    if (layoutView1.RowCount < 1)
            //        return;
            //    //var hiTinfo = layoutView1.CalcHitInfo(e.Location);
            //    //if (hiTinfo.InField)
            //    //{


            //        //if (hiTinfo.Column.FieldName == "ItemImage" || hiTinfo.Column.FieldName == "ItemName" || hiTinfo.Column.FieldName == "RemainQty" || hiTinfo.Column.FieldName == "SalePrice")
            //        //{
            //            long ID = Comon.cLong(layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "ItemID").ToString());
            //            //  layoutView1.SetRowCellValue(hiTinfo.RowHandle, "IsSelect", 0);
            //            // frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;
            //            // FlyoutAction action = new FlyoutAction();

            //            // FlyoutProperties properties = new FlyoutProperties();
            //            //    properties.Style = FlyoutStyle.Popup;
            //            //Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + dt.Rows[0]["BarCode"].ToString() + "'," + Comon.cInt(txtStoreID.Text) + ") AS RemainQty")).Rows[0]["RemainQty"].ToString()).ToString("N" + MySession.GlobalPriceDigits));
            //            var srSize = "   SELECT dbo.RemindQtyStock(BarCode, " + Comon.cInt(txtStoreID.Text) + "," + Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString() + ") AS RemainQty  ," + layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "RemainQty").ToString() + " as RemainQtyParent,0 as PackingQtyParent "
            //          + ",  Stc_Items.ArbName as ItemNAme,CONVERT(VARBINARY(MAX), '0xAAFF')  as ItemImage  ,  Stc_ItemUnits.BarCode,Stc_ItemUnits.PackingQty, Stc_ItemUnits.SalePrice, Stc_SizingUnits.ArbName , Stc_ItemsGroups.Notes "
            //     + " FROM            Stc_ItemUnits LEFT OUTER JOIN"
            //      + "              Stc_Items ON Stc_ItemUnits.ItemID = Stc_Items.ItemID  LEFT OUTER JOIN "
            //              + "              Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID   LEFT OUTER JOIN   Stc_ItemsGroups   ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID  "
            //    + " WHERE        (Stc_ItemUnits.ItemID =" + ID + ")     order by Stc_ItemUnits.PackingQty Asc ";

            //            if (Comon.ConvertToDecimalQty(layoutView1.GetRowCellValue(cardView.FocusedRowHandle, "RemainQty").ToString()) <= 0 && MySession.GlobalWayOfOutItems != "AllowOutItemsWithOutBalance")
            //            {

            //                Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
            //                return;
            //            }
            //            var dtSize = Lip.SelectRecord(srSize);
            //            if (dtSize.Rows.Count < 1)
            //                return;
            //            else if (dtSize.Rows.Count == 1)
            //            {

            //                if (Comon.ConvertToDecimalQty(dtSize.Rows[0]["RemainQty"].ToString()) > 0 || MySession.GlobalWayOfOutItems == "AllowOutItemsWithOutBalance")
            //                {
            //                    btnCilick(dtSize.Rows[0]["BarCode"].ToString(), 1);
            //                    CalculateRow();

            //                }
            //                else
            //                {

            //                    Messages.MsgError(Messages.TitleError, "لايوجد كمية متوفرة   ");
            //                }

            //                return;
            //            }

            //            frmSize = new frmSizeItem(dtSize);
            //            frmSize.layoutView1.MouseUp += layoutViewItem_MouseDown;
            //            frmSize.layoutView1.CustomFieldValueStyle += layoutViewItems_CustomFieldValueStyle;
            //            layoutViewItems_CustomFieldValueStyle(null, null);
            //            // frmSize.Location.X = gridControl2.Location.X;
            //            frmSize.Location = new Point(gridControl2.Location.X + 520, gridControl2.Location.Y + 100);
            //            //  frmSize.DesktopLocation = new Point(1, 1);

            //            //  FlyoutDialog.Show(this, frmSize, action, properties);




            //            frmSize.ShowDialog();

            //    }//


            //   // }//
            //}
            //catch { }
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
               
                //  layoutView1.SetRowCellValue(hiTinfo.RowHandle, "IsSelect", 0);
                // frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;
                // FlyoutAction action = new FlyoutAction();

                // FlyoutProperties properties = new FlyoutProperties();
                //    properties.Style = FlyoutStyle.Popup;
                //Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + dt.Rows[0]["BarCode"].ToString() + "'," + Comon.cInt(txtStoreID.Text) + ") AS RemainQty")).Rows[0]["RemainQty"].ToString()).ToString("N" + MySession.GlobalPriceDigits));
                var srSize = "   SELECT 0 AS RemainQty  ," + "0 as RemainQtyParent,0 as PackingQtyParent "
              + ",  Stc_Items." + languagename + "  as ItemNAme,CONVERT(VARBINARY(MAX), '0xAAFF')  as ItemImage  ,  Stc_ItemUnits.BarCode,Stc_ItemUnits.PackingQty, Stc_ItemUnits.SalePrice, Stc_SizingUnits." + languagename + "  as ArbName , Stc_ItemsGroups.Notes "
         + " FROM            Stc_ItemUnits LEFT OUTER JOIN"
          + "              Stc_Items ON Stc_ItemUnits.ItemID = Stc_Items.ItemID  LEFT OUTER JOIN "
                  + "              Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID   LEFT OUTER JOIN   Stc_ItemsGroups   ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID  "
        + " WHERE        (Stc_ItemUnits.ItemID =" + ID + ")    and Stc_ItemUnits.unitCancel=0  order by Stc_ItemUnits.PackingQty Asc ";

              
                var dtSize = Lip.SelectRecord(srSize);
                if (dtSize.Rows.Count < 1)
                    return;
                else if (dtSize.Rows.Count == 1)
                {


                    btnCilick(dtSize.Rows[0]["BarCode"].ToString(), 1);
                    CalculateRow();

                    return;
                }

               // view.LayoutChanged();
                frmSize = new frmSizeItem(dtSize);
                frmSize.layoutView1.CardClick += layoutViewItem_CardClick_1;

                //   frmSize.layoutView1.CustomFieldValueStyle += layoutViewItems_CustomFieldValueStyle;
               // layoutViewItems_CustomFieldValueStyle(null, null);
                // frmSize.Location.X = gridControl2.Location.X;
                if (UserInfo.Language == iLanguage.Arabic)
                    frmSize.Location = new Point(panelControl2.Location.X, (gridControl2.Location.Y + pnlMainGroup.Height + pnSubGroup.Height + 50));
                else
                {
                    frmSize.Location = new Point(gridControl2.Location.X, gridControl2.Location.Y + 100);
                    //  frmSize.gridControl2.RightToLeft = RightToLeft.No;
                }
                //  frmSize.DesktopLocation = new Point(1, 1);

                //  FlyoutDialog.Show(this, frmSize, action, properties);




                frmSize.ShowDialog();

                //    }//


                // }//
            }
            catch { }
        }

        private bool checkifOffers(long p)
        {
            var dt = dtSpecialOffers.Select("ItemID=" + p.ToString());
            if (dt.Length < 1) return false;
            return true;
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


            //   dr = dtPriceItemOffers.Select("((FromGroupID<=GroupID && ToGroupID>=GroupID )&&(FromItemID<=ItemID && ToItemID>=ItemID ) && (FromSizeID<=SizeID && ToISizeID>=SizeID )) ||((FromItemID<=ItemID && ToItemID>=ItemID ) && (FromSizeID<=SizeID && ToISizeID>=SizeID ))|| ((FromItemID<ItemID && ToItemID>=ItemID ) and (FromSizeID<=0 && ToISizeID>=0)) || (FromGroupID<=GroupID && ToGroupID>=GroupID ) ");

        }

        private void panelControl5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelControl33_Click(object sender, EventArgs e)
        {

        }

        private void panelControl13_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch { }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{ESC}");
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
           
        }

        private void panelControl26_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
           
        }

        private void AddAddress1_Click(object sender, EventArgs e)
        {
            txtCustomerID.Text = frm.ID.ToString();
            txtCustomerID_Validating(null, null);
            CalculateRow();
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
      

        private void XtraForm2_Load(object sender, EventArgs e)
        {
            //var sm = GetSystemMenu(Handle, false);
            //EnableMenuItem(sm, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED);
        }

        private void indexGridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            nextPage = 0;
            try
            {
                var filter = sourceGroup;
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

        private void btnPluss_Click(object sender, System.EventArgs e)
        {
            
            try
            {
                decimal value;
                if ((gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0) || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER1") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER0") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER2") || Comon.cInt(gridView2.GetFocusedRowCellValue("DIAMOND_W").ToString()) == 1)
                    return;
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
                        //value = Comon.ConvertToDecimalQty(strQty);
                        //if (value == 0)
                            value = 1;
                        decimal QtyValue = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY").ToString());
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], value + QtyValue);

                        var dr = dtPriceItemOffers.Select("((FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " )AND(FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " )) or((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " ))OR ((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " ) ");

                        if (gridView2.GetFocusedRowCellValue("Description").Equals("IsPercent"))
                        {

                            decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["Height"]).ToString()));
                            decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["SalePrice"])) *( value +QtyValue) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                            gridView2.SetFocusedRowCellValue(gridView2.Columns["Discount"], total);

                        }

                        GetNewOffers(dr, gridView2.GetFocusedRowCellValue(gridView2.Columns["BarCode"]).ToString(), value + QtyValue);






                        CalculateRow();
                    }
                }
                strQty = "";
                txtTotal.Text = "";
                simpleButton1_Click_2(null, null);
            }
            catch { };
        }

        private void btnMinus_Click_1(object sender, System.EventArgs e)
        {
            try
            {
                decimal value;
                if ((gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0) || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER1") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER0") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER2") || Comon.cInt(gridView2.GetFocusedRowCellValue("DIAMOND_W").ToString()) == 1)
                    return;
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
                        //value = Comon.ConvertToDecimalQty(strQty);
                        //if (value == 0)
                            value = 1;
                        decimal QtyValue = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY").ToString());
                        if (QtyValue - value <= 0) return;
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], QtyValue - value);

                        var dr = dtPriceItemOffers.Select("((FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " )AND(FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " )) or((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " ))OR ((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " ) ");

                        if (gridView2.GetFocusedRowCellValue("Description").Equals("IsPercent"))
                        {

                            decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["Height"]).ToString()));
                            decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["SalePrice"])) * (QtyValue - value) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                            gridView2.SetFocusedRowCellValue(gridView2.Columns["Discount"], total);

                        }

                        GetNewOffers(dr, gridView2.GetFocusedRowCellValue(gridView2.Columns["BarCode"]).ToString(), QtyValue-value );






                        CalculateRow();
                    }
                }
                strQty = "";
                txtTotal.Text = "";
                simpleButton1_Click_2(null, null);
            }
            catch { };
        }

        private void btnUp_Click(object sender, System.EventArgs e)
        {
            gridView2.MovePrev();
        }

        private void btnDown_Click(object sender, System.EventArgs e)
        {
            gridView2.MoveNext();
        }

        private void txtTotal_EditValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                decimal value;
                if ((gridView2.GetFocusedRowCellValue("Caliber").ToString() == "-1" && Comon.cInt(txtCustomerID.Text) > 0) || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER1") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER0") || (gridView2.GetFocusedRowCellValue("Description").ToString() == "ISOFFER2") || Comon.cInt(gridView2.GetFocusedRowCellValue("DIAMOND_W").ToString()) == 1)
                    return;
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
                        if (value == 0) return;
                           // value = 1;
                        decimal QtyValue = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY").ToString());
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], value);

                        var dr = dtPriceItemOffers.Select("((FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " )AND(FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " )) or((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + "and ToISizeID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["SizeID"]).ToString() + " ))OR ((FromItemID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + "and ToItemID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["ItemID"]).ToString() + " ) and (FromSizeID<=" + 0 + "and ToISizeID>=" + 0 + " )) or (FromGroupID<=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + "and ToGroupID>=" + gridView2.GetFocusedRowCellValue(gridView2.Columns["Caliber"]).ToString() + " ) ");

                        if (gridView2.GetFocusedRowCellValue("Description").Equals("IsPercent"))
                        {

                            decimal PercentAmount = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["Height"]).ToString()));
                            decimal total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue(gridView2.Columns["SalePrice"])) * (value ) * Comon.ConvertToDecimalPrice(PercentAmount / 100));
                            gridView2.SetFocusedRowCellValue(gridView2.Columns["Discount"], total);

                        }

                        GetNewOffers(dr, gridView2.GetFocusedRowCellValue(gridView2.Columns["BarCode"]).ToString(), value );






                        CalculateRow();
                    }
                }
               
            }
            catch { };
        }

        private void simpleButton1_Click_3(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void panelControl1_Paint_1(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void lblOrderType_Click_1(object sender, System.EventArgs e)
        {

        }

        private void btnSearchCustomers_Click(object sender, System.EventArgs e)
        {
            Validations.ErrorTextClear(txtCustomerID, txtCustomerID.ToolTip);

            ctCustomers = new ctAddCustomers();
            ctCustomers.simpleButton1.Click += acceptCustomer_Click;
            ctCustomers.btnClose.Click += closeCustomer_Click;
            FlyoutAction action = new FlyoutAction();

            FlyoutProperties properties = new FlyoutProperties();

            properties.Style = FlyoutStyle.Popup;

            FlyoutDialog.Show(this, ctCustomers, action, properties);
        }
        private void acceptCustomer_Click(object sender, EventArgs e)
        {

            txtCustomerID.Text = ctCustomers.txtCustomerID.Text;
            lblCustomerName.Text = ctCustomers.txtArbName.Text;
            //  txtCustomerID_Validating(null, null);
            txtAddressID.Text = ctCustomers.CustomerNo.ToString();
            // txtAddressID_Validating(null, null);
            txtAddressID_Validating(null, null);


        }

        private void closeCustomer_Click(object sender, EventArgs e)
        {

            //txtCustomerID.Text = ctCustomers.txtAccountID.Text;
            //lblCustomerName.Text = ctCustomers.txtArbName.Text;
            ////  txtCustomerID_Validating(null, null);
            //txtAddressID.Text = Comon.cInt(ctCustomers.cmbDestrict.EditValue).ToString();
            //// txtAddressID_Validating(null, null);

            //lblAddressCustomerName.Text = ctCustomers.cmbDestrict.Text + "-" + ctCustomers.txtAddress.Text;
            SendKeys.Send("{ESC}");
        }

        private void btnCustomerSearch_Click(object sender, System.EventArgs e)
        {
            Validations.ErrorTextClear(txtCustomerID, txtCustomerID.ToolTip);
            frm = new XtraForm2();
            frm.simpleButton2.Click += searchResut_Click;
            frm.ShowDialog();
        }

        private void searchResut_Click(object sender, EventArgs e)
        {
            txtCustomerID.Text = frm.CustomerID.ToString();
            txtAddressID_Validating(null, null);



        }

        private void btnSave_Click_1(object sender, System.EventArgs e)
        {

        }

        private void simpleButton10_Click(object sender, System.EventArgs e)
        {
            frmReturnInsurance frm = new frmReturnInsurance();
            frm.Show();
        }

        private void simpleButton2_Click_2(object sender, System.EventArgs e)
        {
           
            


        }

        private void panelControl7_Paint_1(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }





    }
}
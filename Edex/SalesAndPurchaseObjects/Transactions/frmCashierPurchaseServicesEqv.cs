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
using DevExpress.XtraSplashScreen;
using Edex.Model;
using System.IO;
using Edex.DAL.SalseSystem;
using System.Globalization;
using DevExpress.XtraGrid.Menu;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using DevExpress.XtraEditors.Repository;
using Edex.Model.Language;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraGrid;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.StockObjects.Codes;
using Edex.AccountsObjects.Codes;
using System.Data.OleDb;
using Edex.StockObjects.StoresClasses;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using Edex.DAL;
using Edex.SalesAndSaleObjects.Transactions;
using Edex.DAL.Accounting;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.StockObjects.Transactions;
using DevExpress.XtraTab;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmCashierPurchaseServicesEqv : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        CompanyHeader cmpheader = new CompanyHeader();
        int CountTrans = 0;
        public int DiscountCustomer = 0;
        #region Declare
        public const int DocumentType = 23;
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
        public string strQty = "";
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
        private string CaptionCostPriceUnite;
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
        private string CaptionSpendPrice;

        private string CaptionDescription;
        private string CaptionHavVat;
        private string CaptionRemainQty;
        DataTable dVat = new DataTable();
        public MemoryStream TheImage;
        private bool IsNewRecord;
        private Sales_PurchaseInvoicesDAL cClass;
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
        int ItemIDImage = 0;
        public int GoldUsing = 3;

        int StoreItemID = 1;
        //all record master and detail
        BindingList<Acc_SpendVoucherDetails> lstDetailSpend = new BindingList<Acc_SpendVoucherDetails>();
        BindingList<Sales_PurchaseInvoiceDetails> AllRecords = new BindingList<Sales_PurchaseInvoiceDetails>();

        //list detail
        BindingList<Sales_PurchaseInvoiceDetails> lstDetail = new BindingList<Sales_PurchaseInvoiceDetails>();

        //Detail
        Sales_PurchaseInvoiceDetails BoDetail = new Sales_PurchaseInvoiceDetails();
        string VAt = "Select CompanyVATID from  VATIDCOMPANY ";

        #endregion
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
        public frmCashierPurchaseServicesEqv()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                GroupName = "ArbGroupName";
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionCostPriceUnite = "إجمالي تكلفة الوحدة";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "الوحدة";
                CaptionExpiryDate = "تاريخ الصلاحية";
                CaptionQTY = "الوزن";
                CaptionTotal = "الإجمالي";
                CaptionDiscount = "الخصم";
                CaptionAdditionalValue = "الضريبة";
                CaptionNet = "الصافي";
                CaptionCartPrice = "سعر الكرت";
                CaptionCostPrice = "سعر الوحدة";
                CaptionSalePrice = " تكلفةالمحل";
                CaptionSpendPrice = "إجمالي التكلفة ";
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
                    CaptionCostPriceUnite = "Cost Price Unite";
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
                    CaptionCostPrice = "Unit CostPrice";
                    //Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");

                    labelControl33.Text = labelControl33.Tag.ToString();
                    labelControl34.Text = labelControl34.Tag.ToString();
                    labelControl25.Text = labelControl25.Tag.ToString();
                    labelControl26.Text = labelControl26.Tag.ToString();
                    labelControl27.Text = labelControl27.Tag.ToString();
                    labelControl7.Text = labelControl7.Tag.ToString();
                }
                InitGrid();
                InitGridSpend();


           
                 

                /*********************** Fill Data ComboBox  ****************************/
                FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", " BranchID =" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
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
                txtEdit[6] = lblCreditAccountName;
                txtEdit[7] = lblAdditionalAccountName;
                txtEdit[8] = lblDiscountDebitAccountName;
                txtEdit[9] = lblNetAccountName;
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
                /************************  Form Printing ***************************************/
                cmbFormPrinting.EditValue = Comon.cInt(MySession.GlobalDefaultSaleFormPrintingID);
                /*********************** Roles From ****************************/
                txtInvoiceDate.ReadOnly = false;
                lblStoreID.ReadOnly = !MySession.GlobalAllowChangefrmSaleStoreID;
                txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCostCenterID;
                cmbMethodID.ReadOnly = !MySession.GlobalAllowChangefrmSalePayMethodID;
                cmbNetType.ReadOnly = !MySession.GlobalAllowChangefrmSaleNetTypeID;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmSaleCurencyID;
                txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDelegateID;
                txtSellerID.ReadOnly = false;
                /************TextEdit Account ID ***************/
                lblAdditionalAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleAdditionalAccountID;
                lblNetAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleNetAccountID;
                /************ Button Search Account ID ***************/
                RolesButtonSearchAccountID();
                /********************* Event For Account Component ****************************/
               
           
         
                this.lblNetAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblNetAccountID_Validating);
                this.lblAdditionalAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblNetAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                /********************* Event For TextEdit Component **************************/
                if (MySession.GlobalAllowWhenEnterOpenPopup)
                {
                    this.txtInvoiceDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                    this.cmbMethodID.Enter += new System.EventHandler(this.cmbMethodID_Enter);
                    this.cmbCurency.Enter += new System.EventHandler(this.PublicCombox_Enter);
                    this.cmbNetType.Enter += new System.EventHandler(this.PublicCombox_Enter);
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtInvoiceDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.cmbMethodID.Click += new System.EventHandler(this.cmbMethodID_Click);
                    this.cmbCurency.Click += new System.EventHandler(this.PublicCombox_Click);
                    this.cmbNetType.Click += new System.EventHandler(this.PublicCombox_Click);
                }
                this.txtInvoiceID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblStoreID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                this.txtNetProcessID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetAmount.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                this.cmbMethodID.EditValueChanged += new System.EventHandler(this.cmbMethodID_EditValueChanged);
                this.cmbNetType.EditValueChanged += new System.EventHandler(this.cmbNetType_EditValueChanged);



                this.chkForVat.EditValueChanged += new System.EventHandler(this.chForVat_EditValueChanged);

                this.txtDiscountOnTotal.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountOnTotal_Validating);
                this.txtDiscountPercent.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountPercent_Validating);
                this.txtInvoiceID.Validating += new System.ComponentModel.CancelEventHandler(this.txtInvoiceID_Validating);
                this.lblStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                lblDiscountDebitAccountID.Validating += lblDiscountDebitAccountID_Validating;
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
                this.txtSellerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSellerID_Validating);
                this.txtPaidAmount.Validating += new System.ComponentModel.CancelEventHandler(this.txtPaidAmount_Validating);

                this.txtDiscountOnTotal.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDiscountPercent.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);


                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
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
                this.gridView1.RowUpdated += gridView1_RowUpdated;
                this.gridViewTransport.ValidatingEditor+=gridViewTransport_ValidatingEditor;
                this.gridViewTransport.RowUpdated += gridViewTransport_RowUpdated;
                /******************************************/


                txtOrderID.Validating += txtOrderID_Validating;
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;

                DoNew();
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

        void lblDiscountDebitAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cLong(lblDiscountDebitAccountID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(lblDiscountDebitAccountID, lblDiscountDebitAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        void gridViewTransport_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculatTotalTransPort();
        }

        void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculateRow();

        }

        public void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtOrderID.Text))
                    return;
                else
                {
                    ClearFields();
                    DoNew();
                    dt = Sales_PurchaseOrderDAL.frmGetDataDetalByID(Comon.cInt(txtOrderID.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                  
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = true;
                        button1.Visible = true;
                        //Validate

                        lblStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);

                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();

                        StopSomeCode = true;
                        StopSomeCode = false;

                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());

                        txtSupplierID.Text = dt.Rows[0]["SupplierID"].ToString();
                        txtSupplierID_Validating(null, null);


                        //cmbSellerID.EditValue = dt.Rows[0]["SellerID"].ToString();
                        //Masterdata
                        txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();

                        txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();

                        //Date
                        txtInvoiceDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString());

                        //Ammount
                        //حقول محسوبة  

                        lblInvoiceTotal.Text = dt.Rows[0]["InvoiceTotal"].ToString();
                        //GridVeiw
                        gridControl.DataSource = dt;

                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        CalculateRow();
                        this.pictureBox1.Image = Common.GenratCod("ID = " +txtInvoiceID.Text + " Date= " + txtInvoiceDate.Text);

                        //Validations.DoReadRipon(this, ribbonControl1);
                        chForVat_EditValueChanged(null, null);
                        // ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtOrderID.Text;
                    }
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #region GridView
        void InitGridSpend()
        {

            string AccountName;
            string CaptionDebitAmount;
            string CaptionAccountID;
            string CaptionAccountName;
            string CaptionDeclaration;
            string CaptionCostCenterID;


            string CaptionBarcode;
            string CaptionQtyGold;
            string CaptionQtyGoldEqulivent;


            AccountName = "ArbAccountName";
             //PrimaryName = "ArbName";
             if (UserInfo.Language == iLanguage.Arabic)
             {
                 CaptionDebitAmount = "الـمـبـلـغ";
                 CaptionAccountID = "رقم الحساب";
                 CaptionAccountName = "اسم الحساب";
                 CaptionDiscount = "الخصـم";
                 CaptionDeclaration = "الـبـيـــــان";
                 CaptionCostCenterID = "مركز تكلفة";
                 CaptionBarcode = "كود الصنف";
                 CaptionItemName = "اسم الصنف  ";
                 CaptionQtyGold = "الوزن";
                 CaptionQtyGoldEqulivent = "المعادل";

             }
             else
             {
                 CaptionDebitAmount = "Amount";
                 CaptionAccountID = "Account ID";
                 CaptionAccountName = "Account Name";
                 CaptionDiscount = "Discount";
                 CaptionDeclaration = "Description";
                 CaptionCostCenterID = "Cost center";
                 CaptionBarcode = "BarCode";
                 CaptionItemName = "Item Name";
                 CaptionQtyGold = "QTY";
                 CaptionQtyGoldEqulivent = "Equivalent";
             }

            lstDetailSpend = new BindingList<Acc_SpendVoucherDetails>();
            lstDetailSpend.AllowNew = true;
            lstDetailSpend.AllowEdit = true;
            lstDetailSpend.AllowRemove = true;
            gridControlTransport.DataSource = lstDetailSpend;

            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridViewTransport.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridViewTransport.BestFitColumns();
            /******************* Columns Visible=false ********************/
            gridViewTransport.Columns["SpendVoucherID"].Visible = false;
            gridViewTransport.Columns["ID"].Visible = false;
            gridViewTransport.Columns["FacilityID"].Visible = false;
            gridViewTransport.Columns["BranchID"].Visible = false;
            gridViewTransport.Columns["SpendVoucherMaster"].Visible = false;
            gridViewTransport.Columns["ArbAccountName"].Visible = false;
            gridViewTransport.Columns["EngAccountName"].Visible = false;
            /******************* Columns Visible=true ********************/
            gridViewTransport.Columns[AccountName].Visible = true;
            /******************* Columns Visible=true *******************/
            gridViewTransport.Columns["DebitAmount"].Caption = CaptionDebitAmount;
            gridViewTransport.Columns["AccountID"].Caption = CaptionAccountID;
            gridViewTransport.Columns[AccountName].Caption = CaptionAccountName;
            gridViewTransport.Columns[AccountName].Width = 150;
            gridViewTransport.Columns["Discount"].Caption = CaptionDiscount;
            gridViewTransport.Columns["Declaration"].Caption = CaptionDeclaration;
            gridViewTransport.Columns["Declaration"].Width = 150;
            gridViewTransport.Columns["CostCenterID"].Caption = CaptionCostCenterID;
            gridViewTransport.Columns["ItemName"].Width = 120;
            gridViewTransport.Columns["ItemName"].Visible = false;
            gridViewTransport.Columns["Barcode"].Visible = false;
            gridViewTransport.Columns["AccountID"].Width = 120;
            gridViewTransport.Columns["Barcode"].Caption = CaptionBarcode;
            gridViewTransport.Columns["ItemName"].Caption = CaptionItemName;
            gridViewTransport.Columns["WeightGold"].Caption = CaptionQtyGold;
            gridViewTransport.Columns["QtyGoldEqulivent"].Caption = CaptionQtyGoldEqulivent;

            gridViewTransport.Columns["Calipar"].Caption = "العيار";

            gridViewTransport.Columns["CurrencyID"].Visible = false;
            gridViewTransport.Columns["CurrencyEquivalent"].Visible = false;
            gridViewTransport.Columns["CurrencyPrice"].Visible = false;
            gridViewTransport.Columns["CurrencyName"].Visible = false;
            gridViewTransport.Columns["Discount"].Visible = false;
            gridViewTransport.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridViewTransport.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;
            DataTable dtitems = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Acc_Currency where Cancel=0 And BranchID =" + MySession.GlobalBranchID);
            string[] CurrncyName = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                CurrncyName[i] = dtitems.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(CurrncyName);
            gridControl.RepositoryItems.Add(riComboBoxitems);
            gridViewTransport.Columns["CurrencyName"].ColumnEdit = riComboBoxitems;
            gridViewTransport.Columns["CurrencyPrice"].Caption = "سعر العملة";
            gridViewTransport.Columns["CurrencyID"].Caption = "رقم العملة";
            gridViewTransport.Columns["CurrencyName"].Caption = "اسم العملة";
            gridViewTransport.Columns["CurrencyEquivalent"].Caption = "المقابل بالعملة المحلية ";
            if (UserInfo.Language == iLanguage.English)
            {
                gridViewTransport.Columns["Calipar"].Caption = "Calipar";
                gridViewTransport.Columns["CurrencyPrice"].Caption = "Currency Price  ";
                gridViewTransport.Columns["CurrencyID"].Caption = "Currency ID  ";
                gridViewTransport.Columns["CurrencyName"].Caption = "Currency Name";
                gridViewTransport.Columns["CurrencyEquivalent"].Caption = "Currency Equivalent";
            }

            gridViewTransport.Columns["Calipar"].Visible = false;
            gridViewTransport.Columns["QtyGoldEqulivent"].Visible = false;
            gridViewTransport.Columns["WeightGold"].Visible = false;
            gridViewTransport.Focus();
            /*************************Columns Properties ****************************/


            gridViewTransport.Columns["CostCenterID"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmSpendVoucherCostCenterID;
            gridViewTransport.Columns["CostCenterID"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmSpendVoucherCostCenterID;

            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridViewTransport.Columns[AccountName].ColumnEdit = rAccountName;
            gridControlTransport.RepositoryItems.Add(rAccountName);


            RepositoryItemLookUpEdit rCostCenter = new RepositoryItemLookUpEdit();
            gridViewTransport.Columns["CostCenterID"].OptionsColumn.AllowEdit = MySession.GlobalAllowChangefrmSpendVoucherCostCenterID;
            gridViewTransport.Columns["CostCenterID"].ColumnEdit = rCostCenter;
            gridControlTransport.RepositoryItems.Add(rCostCenter);
            FillCombo.FillComboBoxRepositoryItemLookUpEdit(rCostCenter, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "1=1");
        }
        void InitGrid()
        {
            lstDetail = new BindingList<Sales_PurchaseInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;
            /******************* Columns Visible=false ********************/
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["PackingQty"].Visible = false;
            gridView1.Columns["Caliber"].Visible = false;
            gridView1.Columns["SalePrice"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = true;
            gridView1.Columns["Bones"].Visible = true;
            gridView1.Columns["Height"].Visible = false;
            gridView1.Columns["Width"].Visible = false;
            gridView1.Columns["TheCount"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;


            gridView1.Columns["ArbGroupName"].Visible = false;
            gridView1.Columns["EngGroupName"].Visible = false;

            gridView1.Columns["SpendPrice"].Visible = false;
            gridView1.Columns["Serials"].Visible = false;

            gridView1.Columns["InvoiceID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["StoreID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["BarCode"].Visible = true;
            gridView1.Columns["ExpiryDate"].Visible = false;
            gridView1.Columns["Description"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;
            gridView1.Columns[GroupName].Visible = true;
            gridView1.Columns["ArbGroupName"].Visible = gridView1.Columns["ArbGroupName"].Name == "col" + GroupName ? true : false;
            gridView1.Columns["EngGroupName"].Visible = gridView1.Columns["EngGroupName"].Name == "col" + GroupName ? true : false;

            gridView1.Columns["Total"].Visible = true;
            gridView1.Columns["Net"].Visible = true;
            
                gridView1.Columns["Bones"].Caption = "المصاريف";
                gridView1.Columns["GroupID"].Caption = "رقم المجموعة";
                gridView1.Columns[GroupName].Caption = "اسم المجموعة";

                gridView1.Columns["Serials"].Caption = "رقم المرجع";
            
            /******************* Columns Visible=true *******************/
            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;


            gridView1.Columns["Color"].Visible = false;
            gridView1.Columns["BAGET_W"].Visible = false;
            gridView1.Columns["DIAMOND_W"].Visible = false;
            gridView1.Columns["STONE_W"].Visible = false;
            gridView1.Columns["CLARITY"].Visible = false;
            gridView1.Columns["Equivalen"].Visible = false;
            gridView1.Columns["CaratPrice"].Visible = false;
            gridView1.Columns["HavVat"].Visible = false;
            gridView1.Columns["RemainQty"].Visible = false;
            gridView1.Columns["ItemID"].Visible = false;
            gridView1.Columns["GroupID"].Visible = false;

            gridView1.Columns["ItemStatus"].Visible = false;
            gridView1.Columns["TypeGold"].Visible = false;
            gridView1.Columns["PurchaseMaster"].Visible = false;
            gridView1.Columns["SalePrice"].Visible = false;

            gridView1.Columns["SalePrice"].Caption = "سعر البيع";
            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 150;
            gridView1.Columns["CostPriceUnite"].Caption = CaptionCostPriceUnite;
            gridView1.Columns["CostPriceUnite"].Visible = false;
            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["QTY"].Caption = CaptionQTY;

            gridView1.Columns["Total"].Caption = CaptionTotal;
            gridView1.Columns["Discount"].Caption = CaptionDiscount;
            
            gridView1.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView1.Columns["Net"].Caption = CaptionNet;
            gridView1.Columns["Bones"].VisibleIndex = gridView1.Columns["Net"].VisibleIndex - 1;
            gridView1.Columns["CostPrice"].Caption = CaptionCostPrice;
            gridView1.Columns["SalePrice"].Caption = CaptionCartPrice;

            gridView1.Columns["SpendPrice"].Caption = CaptionSpendPrice;

            gridView1.Columns["Description"].Caption = CaptionDescription;
            gridView1.Columns["HavVat"].Caption = CaptionHavVat;
            gridView1.Columns["RemainQty"].Caption = CaptionRemainQty;



            DataTable dtCurrncy = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Acc_Currency where Cancel=0 And BranchID =" + MySession.GlobalBranchID);
            string[] CurrncyName = new string[dtCurrncy.Rows.Count];
            for (int i = 0; i <= dtCurrncy.Rows.Count - 1; i++)
                CurrncyName[i] = dtCurrncy.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems1 = new RepositoryItemComboBox();
            riComboBoxitems1.Items.AddRange(CurrncyName);
            gridControl.RepositoryItems.Add(riComboBoxitems1);
            gridView1.Columns["CurrencyName"].ColumnEdit = riComboBoxitems1;
            gridView1.Columns["CurrencyPrice"].Caption = "سعر العملة";
            gridView1.Columns["CurrencyID"].Caption = "رقم العملة";
            gridView1.Columns["CurrencyName"].Caption = "اسم العملة";
            gridView1.Columns["CurrencyEquivalent"].Caption = "المقابل بالعملة المحلية";
            if (UserInfo.Language == iLanguage.English)
            {
                gridView1.Columns["Caliber"].Caption = "Caliber";
                gridView1.Columns["CurrencyPrice"].Caption = "Currency Price  ";
                gridView1.Columns["CurrencyID"].Caption = "Currency ID  ";
                gridView1.Columns["CurrencyName"].Caption = "Currency Name";
                gridView1.Columns["CurrencyEquivalent"].Caption = "Currency Equivalent";
                gridView1.Columns["Bones"].Caption = "Expencess";
                gridView1.Columns["GroupID"].Caption = "Group ID";
                gridView1.Columns[GroupName].Caption = "Group Name";

                gridView1.Columns["Serials"].Caption = "Referance ID";
            }

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
            DataTable dtitems = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Stc_ItemsColors where BranchID =" + MySession.GlobalBranchID);
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i][PrimaryName].ToString();


            /////////////////////////


            /////////////////////////Item Size
            DataTable dtitemsCLARITY = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Stc_ItemsSizes where BranchID =" + MySession.GlobalBranchID);
            string[] companiesitemsCLARITY = new string[dtitemsCLARITY.Rows.Count];
            for (int i = 0; i <= dtitemsCLARITY.Rows.Count - 1; i++)
                companiesitemsCLARITY[i] = dtitemsCLARITY.Rows[i][PrimaryName].ToString();


            /////////////////////////
            RepositoryItemLookUpEdit rItemName = Common.LookUpEditItemName();
            gridView1.Columns[ItemName].ColumnEdit = rItemName;
            gridControl.RepositoryItems.Add(rItemName);
            /////////////////////////Description
            DataTable dt = Lip.SelectRecord("SELECT "+PrimaryName+" FROM Stc_ItemsGroups WHERE Cancel=0 and BranchID=" + MySession.GlobalBranchID + " and AccountTypeID=" + 1);
            string[] companies = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companies[i] = dt.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBox = new RepositoryItemComboBox();
            riComboBox.Items.AddRange(companies);
            gridControl.RepositoryItems.Add(riComboBox);
            gridView1.Columns["Description"].ColumnEdit = riComboBox;
            gridView1.Columns["Description"].Width = 120;
            gridView1.Columns[ItemName].Width = 120;

            gridView1.Columns["BarCode"].Width = 90;

            ///////////////////
            ///

            /////////////////////////Item
            ///

            string[] companiesGroupitems = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companiesGroupitems[i] = dt.Rows[i][PrimaryName].ToString();

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

            //gridView1.Columns["SalePrice"].VisibleIndex = 18;
            //gridView1.Columns["CurrencyEquivalent"].VisibleIndex = gridView1.Columns["SalePrice"].VisibleIndex + 1;


            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["CurrencyPrice"].Visible = false;
            gridView1.Columns["CurrencyName"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].Visible = false;

            /******************************** Menu ***************************************/
            menu = new GridViewMenu(gridView1);
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("أسعار الصنف", new EventHandler(Price_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("بيانات الصنف", new EventHandler(item_Click)));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("كرت الصنف"));
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem("باركود الصنف"));

            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["AdditionalValue"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["AdditionalValue"].OptionsColumn.AllowEdit = false;

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
            frm.ShowDialog();
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], Comon.ConvertToDecimalPrice(frm.CelValue));
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
                    if (col.FieldName == "BarCode"   || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID" )
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
                if (ColName == "Bones" || ColName == "BarCode"  || ColName == "SizeID"  || ColName == "ItemID" || ColName == "QTY"  || ColName == "CurrencyPrice")
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
                      

                   
                    if (ColName == "CurrencyPrice")
                    {
                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SpendPrice")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SpendPrice"))).ToString());

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
                            RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQtyStock]('" + BarCode + "'," + StoreItemID + ",0,0,"+MySession.GlobalBranchID+") AS RemainQty").Rows[0]["RemainQty"].ToString());
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
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        gridView1.SetColumnError(gridView1.Columns["QTY"], "");
                        e.ErrorText = "";
                        decimal PriceUnit = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("CostPrice"));
                        decimal additonalVAlue = 0;

                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());

                        decimal QTY = Comon.ConvertToDecimalPrice(val.ToString());
                        decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("CostPrice"));
                        decimal SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("SalePrice"));

                        decimal TotalCost = Comon.ConvertToDecimalPrice(QTY * CostPrice);
                        decimal TotalSale = Comon.ConvertToDecimalPrice(QTY * SalePrice);
                        decimal Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount"));
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", Comon.ConvertToDecimalPrice(TotalCost) - Comon.ConvertToDecimalPrice(Discount));

                        decimal Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
                        if (HasVat == true && chkForVat.Checked)
                            additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;
                        gridView1.SetFocusedRowCellValue("Width", Comon.ConvertToDecimalPrice(TotalSale).ToString());
                        gridView1.SetFocusedRowCellValue("SpendPrice", 0);
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("CostPrice", PriceUnit.ToString());

                        decimal ExpensesAmount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Bones"));
                        decimal Net = Comon.ConvertToDecimalPrice(Total + ExpensesAmount + additonalVAlue);


                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
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
                             e.Valid = false;
                             HasColumnErrors = true;
                             e.ErrorText = Messages.msgNoFoundThisBarCode;
                        }
                        else
                        {

                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                            gridView1.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);

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


                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(val.ToString()));
                            gridView1.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);
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
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from Stc_SizingUnits  Where SizeID=" + e.Value + " And BranchID =" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dtItemID.Rows[0][PrimaryName].ToString());
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = "رقم الوحدة غير موجود  ";
                        }

                    }
                }

                if (ColName == "CurrencyName")
                {
                    DataTable dt = Lip.SelectRecord("Select ID ,ExchangeRate from Acc_Currency Where Cancel=0 And BranchID =" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "')");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyID", dt.Rows[0]["ID"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice", dt.Rows[0]["ExchangeRate"]);
                    if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CostPrice")) > 0)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CostPrice"))).ToString());


                }
                if (ColName == "CostPrice")
                {
                    bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                    decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("QTY"));
                    decimal SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("SalePrice"));
                    decimal CostPrice = Comon.ConvertToDecimalPrice(val.ToString());

                    decimal TotalCost = QTY * CostPrice;
                    decimal TotalSale = QTY * SalePrice;

                    decimal Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount"));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Total", Comon.ConvertToDecimalPrice(TotalCost) - Comon.ConvertToDecimalPrice(Discount));
                    decimal Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));

                    decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);

                    if (HasVat == true && chkForVat.Checked)
                        additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                    else
                        additonalVAlue = 0;

                    gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                    gridView1.SetFocusedRowCellValue("SpendPrice", 0);
                    decimal ExpensesAmount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Bones"));
                    decimal Net = Comon.ConvertToDecimalPrice(Total + ExpensesAmount + additonalVAlue);
                    gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                    gridView1.SetFocusedRowCellValue("Width", TotalSale.ToString());
                    gridView1.SetFocusedRowCellValue("Height", TotalCost.ToString());

                    if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) > 0)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(TotalCost) * Comon.cDec(txtCurrncyPrice.Text)).ToString());

                }
                else if (ColName == GroupName)
                {
                    DataTable dtGroupID = Lip.SelectRecord("Select GroupID, "+PrimaryName+" from Stc_ItemsGroups Where Cancel=0 And BranchID =" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "')");
                    if (dtGroupID.Rows.Count > 0)
                    {
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"], cmbCurency.Text.ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);

                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SpendPrice")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(txtCurrncyPrice.Text) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SpendPrice"))).ToString());

                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
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
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dtGroupID.Rows[0][PrimaryName].ToString());

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
                    DataTable dtGroupID = Lip.SelectRecord("Select GroupID, "+PrimaryName+" from Stc_ItemsGroups Where Cancel=0 And BranchID =" + MySession.GlobalBranchID+" And  GroupID = " + val.ToString() + "");
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

                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dtGroupID.Rows[0][PrimaryName].ToString());
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
                    
                    DataTable dtSize = Lip.SelectRecord("Select SizeID, " + PrimaryName + "  from Stc_SizingUnits Where Cancel=0 And BranchID =" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtSize.Rows.Count > 0)
                    {
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dtSize.Rows[0]["SizeID"].ToString());
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
                    decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CostPrice").ToString());
                    decimal Total = QTY * CostPrice;
                    decimal PercentDiscount = Total * (MySession.GlobalDiscountPercentOnItem / 100);
                    if (!(double.TryParse(val.ToString(), out num)))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) > 0 && (MySession.GlobalDiscountPercentOnItem <= 0)) { Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount); return; }
                    else  if (Comon.ConvertToDecimalPrice(val.ToString()) > PercentDiscount)
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
                        decimal TotalCost = QTY * CostPrice;        
                          Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        if (HasVat == true)
                            additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;            
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("SpendPrice",0);
                        decimal ExpensesAmount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Bones"));
                        decimal Net = Comon.ConvertToDecimalPrice(Total + ExpensesAmount + additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                       
                    }
                }
                else if (ColName == ItemName)
                    {
                    DataTable dtItemID = Lip.SelectRecord("Select ItemID from Stc_Items Where Cancel=0 And BranchID =" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dtItem);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dtItem);
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
                        if (ColName == "BarCode"  || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID" || ColName == "SalePrice")
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

                            //if (ColName == "QTY")
                            //{
                            //    string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                            //    double RemindQty = 0;
                            //    RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + BarCode + "'," + Comon.cInt(lblStoreID.Text) + ") AS RemainQty").Rows[0]["RemainQty"].ToString());
                            //    double Qty = Comon.cDbl(QtyItem);

                            //    if (RemindQty <= 0)
                            //    {

                            //        if (MySession.GlobalWayOfOutItems == "PreventOutItemsWithOutBalance")
                            //        {

                            //            HasColumnErrors = true;
                            //            view.SetColumnError(gridView1.Columns[ColName], "الكمية  غير متوفرة ");
                            //            // view.DeleteRow(view.FocusedRowHandle);
                            //            return;
                            //        }
                            //    }

                            //}

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
                    //#region Delete Diamond Type
                    //Sales_PurchaseDiamondDetails modelDeletDimondType = new Sales_PurchaseDiamondDetails();
                    //modelDeletDimondType.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                    //modelDeletDimondType.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    //modelDeletDimondType.FacilityID = UserInfo.FacilityID;
                    //modelDeletDimondType.BarCodeItem = gridView1.GetRowCellValue(index, "BarCode").ToString();
                    //int ResultDeleteType = Sales_PurchaseDiamondDetailsDAL.Delete(modelDeletDimondType);
                    //#endregion
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
                if (Stc_itemsDAL.CheckIfStopItemUnit(dt.Rows[0]["BarCode"].ToString(), MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                {
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    return;
                }
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], "1");

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0][PrimaryName].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());

              if(UserInfo.Language==iLanguage.Arabic)
                   gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
              else
                  gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dt.Rows[0]["ArbGroupName"].ToString());

                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                {
               
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[GroupName], dt.Rows[0]["EngGroupName"].ToString());
                }
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

                //Get  AverageCostPrice
                decimal AverageCost = frmItems.GetItemAverageCostPrice(Comon.cLong(dt.Rows[0]["ItemID"]), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cInt(lblStoreID.Text), 0, 0, 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], AverageCost);
                ///////////////////
                ///
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"], cmbCurency.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyEquivalent"], Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtCurrncyPrice.Text) * Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString())));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
                try
                {
                    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemImage"], dt.Rows[0]["ItemImage"]);
                }
                catch { }
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], lblStoreID.Text);
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
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], dt.Rows[0]["IsVAT"]);
            
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
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], lblStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], 0);
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
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"], cmbCurency.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyEquivalent"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
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
            if (MethodID == 3 || MethodID == 5)
                cmbNetType.Enabled = true;
            cmbFormPrinting.Enabled = true;

            gridView1.Columns["Bones"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["Bones"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["Net"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["Net"].OptionsColumn.AllowEdit = false;

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
                    if (col.FieldName == "BarCode" || col.FieldName == "Net" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
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

                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
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
        public void CalculatTotalTransPort()
        {
            decimal CreditTotal = 0;
            decimal DebitAmountRow = 0;
            try
            {
                for (int i = 0; i <= gridViewTransport.DataRowCount - 1; i++)
                {
                    DebitAmountRow = Comon.ConvertToDecimalPrice(gridViewTransport.GetRowCellValue(i, "DebitAmount").ToString());
                    CreditTotal += DebitAmountRow;
                }
                txtTransportAmount.Text = CreditTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
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
                decimal SpendPriceTotal = 0;
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

                decimal TotalGold_W = 0;
                bool HavVatRow = false;
                decimal Gold_W = 0;
                MySession.UseNetINInvoiceSales = 1;
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    int Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                    QTYRow = 1;
                    Gold_W = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    QtyItem = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY"));
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                    AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
              
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(SalePriceRow);
                    OjorTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones"));
                    TotalGold_W += Gold_W;
                    TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                    gridView1.SetRowCellValue(i, "CostPriceUnite", Comon.cDbl(Comon.cDbl(OjorTotal + TotalRow) / Comon.cDbl(QtyItem)));
                    AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    TotalBeforeDiscountRow = TotalRow;
                    SpendPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice"));
                    if (Caliber == 18)
                        QTY18 = QTY18 + QtyItem;
                    if (Caliber == 21)
                        QTY21 = QTY21 + QtyItem;

                    if (Caliber == 22)
                        QTY22 = QTY22 + QtyItem;
                    if (Caliber == 24)
                        QTY24 = QTY24 + QtyItem;

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
                    QtyItem = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(rowIndex, "QTY"));


                    QTYRow = Comon.ConvertToDecimalPrice(ResultQTY.ToString());
                    SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
               
                    SpendPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "SalePrice"));


                    OjorTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Bones"));
                    TotalGold_W += Comon.ConvertToDecimalPrice(ResultGold);


                    TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Total"));
                    gridView1.SetRowCellValue(rowIndex, "CostPriceUnite", Comon.cDbl(Comon.cDbl(OjorTotal + TotalRow) / Comon.cDbl(QtyItem)));
                    AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue")) : 0;
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscountRow = TotalRow;


                    if (ResultCaliber == 18)
                        QTY18 = QTY18 + Comon.ConvertToDecimalPrice(QtyItem);
                    if (ResultCaliber == 21)
                        QTY21 = QTY21 + Comon.ConvertToDecimalPrice(QtyItem); 

                    if (ResultCaliber == 22)
                        QTY22 = QTY22 + Comon.ConvertToDecimalPrice(QtyItem); 
                    if (ResultCaliber == 24)
                        QTY24 = QTY24 + Comon.ConvertToDecimalPrice(QtyItem); 


                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }
                DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);
                AdditionalAmount = Comon.ConvertToDecimalPrice((Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text) * MySession.GlobalPercentVat) / 100);
          
                if (chkForVat.Checked == false)
                    AdditionalAmount = 0;
                Net = Comon.ConvertToDecimalPrice(lblInvoiceTotal.Text) + AdditionalAmount;
                lblAdditionaAmmount.Text = Comon.ConvertToDecimalPrice(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = (Net + Comon.ConvertToDecimalPrice(txtTransportAmount.Text)).ToString("N" + MySession.GlobalPriceDigits);

                decimal Eq = 0;

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

                Eq = Comon.ConvertTo21Caliber(QTY18, 18, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY21, 21, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY22, 22, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY24, 24, 18);

                lblTotalWeight.Text = Comon.ConvertToDecimalQty(QTY18).ToString("N" + MySession.GlobalQtyDigits);
                lbl21.Text = Comon.ConvertToDecimalQty(QTY21).ToString("N" + MySession.GlobalQtyDigits);

                lbl22.Text = Comon.ConvertToDecimalQty(QTY22).ToString("N" + MySession.GlobalQtyDigits);
                lbl24.Text = Comon.ConvertToDecimalQty(QTY24).ToString("N" + MySession.GlobalQtyDigits);



                lblInvoiceTotalOjor.Text = OjorTotal.ToString("N" + MySession.GlobalQtyDigits);
                lblTotalWeight.Text = TotalGold_W.ToString("N" + MySession.GlobalQtyDigits);


                int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 And BranchID =" + MySession.GlobalBranchID));
                if (isLocalCurrncy > 1)
                {
                    decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 And BranchID =" + MySession.GlobalBranchID));
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
                decimal SpendPriceTotal = 0;
                decimal SalePriceTotal = 0;

                decimal TotalDaimond_W = 0;
                decimal TotalStown_W = 0;
                decimal TotalBagat_W = 0;

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
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                    AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    TotalBeforeDiscountRow = TotalRow;
                    SpendPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpendPrice"));
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
                        QTY21 = QTY21 + Comon.ConvertToDecimalPrice(QTYRow);

                    if (ResultCaliber == 22)
                        QTY22 = QTY22 + Comon.ConvertToDecimalPrice(QTYRow);
                    if (ResultCaliber == 24)
                        QTY24 = QTY24 + Comon.ConvertToDecimalPrice(QTYRow);
                    SpendPriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "SpendPrice"));
                    SalePriceTotal += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "SalePrice"));



                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }
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


                Eq = Comon.ConvertTo21Caliber(QTY18, 18, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY21, 21, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY22, 22, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY24, 24, 18);


                lblTotalWeight.Text = Comon.ConvertToDecimalQty(QTY18).ToString("N" + MySession.GlobalQtyDigits);
                lbl21.Text = Comon.ConvertToDecimalQty(QTY21).ToString("N" + MySession.GlobalQtyDigits);

                lbl22.Text = Comon.ConvertToDecimalQty(QTY22).ToString("N" + MySession.GlobalQtyDigits);
                lbl24.Text = Comon.ConvertToDecimalQty(QTY24).ToString("N" + MySession.GlobalQtyDigits);



            }

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        #endregion
        #region Function
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

            if (FocusedControl.Trim() == lblStoreID.Name)
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
                if (!MySession.GlobalAllowChangefrmPurchaseSupplierID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "SublierID", "رقم المـــورد", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "SublierID", "SublierID ID", MySession.GlobalBranchID);
            }
 
            else if (FocusedControl.Trim() == lblStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblStoreID,lblSellerName, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, lblStoreID, lblSellerName, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == lblCreditAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblCreditAccountID, lblCreditAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblCreditAccountID, lblCreditAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == lblDiscountDebitAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblDiscountDebitAccountID, lblDiscountDebitAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblDiscountDebitAccountID, lblDiscountDebitAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == lblNetAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseNetAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblNetAccountID, lblNetAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblNetAccountID, lblNetAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == lblAdditionalAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseAdditionalAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblAdditionalAccountID, lblAdditionalAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblAdditionalAccountID, lblAdditionalAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtInvoiceID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtInvoiceID, null, "PurchaseInvoice", "رقـم الـفـاتـورة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtInvoiceID, null, "PurchaseInvoice", "Invoice ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

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
                if (!MySession.GlobalAllowChangefrmPurchaseCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;

                if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
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
                else if (gridView1.FocusedColumn.Name == "colQTY")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString(),lblStoreID.Text.ToString());
                        else
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "ارجاء اختيار صنف ومن  ثم اعادة عرض الكمية المتبقية" : "Please select an item and re-display the remaining quantity");
                            frm.Close();
                            return;
                        }
                    }
                    else
                        frm.Dispose();
                }
            }

            else if (FocusedControl.Trim() == gridControlTransport.Name)
            {

                if (gridViewTransport.FocusedColumn == null) return;
                if (gridViewTransport.FocusedColumn.Name == "colAccountID" || gridViewTransport.FocusedColumn.Name == "colAccountName")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "Account ID", MySession.GlobalBranchID);
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

                else if (FocusedControl == lblStoreID.Name)
                {
                    lblStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }
                else if (FocusedControl == lblDiscountDebitAccountID.Name)
                {
                    lblDiscountDebitAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblDiscountDebitAccountID_Validating(null, null);
                }
                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
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
                else if (FocusedControl == lblAdditionalAccountID.Name)
                {
                    lblAdditionalAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblAdditionalAccountID_Validating(null, null);
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

                else if (FocusedControl == gridControlTransport.Name)
                {
                    if (gridViewTransport.FocusedColumn.Name == "colAccountID" || gridViewTransport.FocusedColumn.Name == "colAccountName")
                    {
                        var ibdex = gridViewTransport.IsNewItemRow(gridViewTransport.FocusedRowHandle);
                        if (ibdex == false)
                        {

                            string Barcode = cls.PrimaryKeyValue.ToString();
                            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["AccountID"], Barcode);
                            DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                            DataRow[] row = dt.Select("AccountID=" + Barcode);
                            if (Comon.cInt(row.Length) > 0)
                                FileItemDataAccount(row[0]);
                            gridViewTransport.FocusedColumn = gridViewTransport.VisibleColumns[3];
                            gridViewTransport.ShowEditor();
                            CalculatTotalTransPort();
                        }
                        else
                        {
                            string Barcode = cls.PrimaryKeyValue.ToString();
                            gridViewTransport.AddNewRow();
                            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["AccountID"], Barcode);
                            DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                            DataRow[] row = dt.Select("AccountID=" + Barcode);
                            if (Comon.cInt(row.Length) > 0)
                                FileItemDataAccount(row[0]);
                            gridViewTransport.FocusedColumn = gridViewTransport.VisibleColumns[3];
                            gridViewTransport.ShowEditor();
                            CalculatTotalTransPort();

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
            try
            {
                lblNetAccountID.Text = MySession.GlobalDefaultPurchaseNetTypeID;
                lblAdditionalAccountID.Text = MySession.GlobalDefaultPurchaseAddtionalAccountID;
                lblCreditAccountID.Text = MySession.GlobalDefaultPurchaseCrditAccountID;
                lblDiscountDebitAccountID.Text = MySession.GlobalDefaultPurchaseDiscountAccountID;
                lblDiscountDebitAccountID_Validating(null,null);
                lblNetAccountID_Validating(null, null);
                lblCreditAccountID_Validating(null, null);
                lblAdditionalAccountID_Validating(null, null);
            }
            catch (Exception ex){}          
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
                txtTransportAmount.Enabled = true;
                chkPosted.Checked = false;
                txtTransportAmount.Text = "0";
                btnprintBarcode.Visible = false;
                button1.Visible = false;
                chkNoSale.Checked = false;
                DiscountCustomer = 0;
                lblInvoiceTotalOjor.Text = "0";
                txtPaidAmount.Text = "";
                lblRemaindAmount.Text = "";
                txtVatID.Text = "";
                txtDocumentID.Text = "";
                lblTotalWeight.Text = "0";
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
                cmbNetType.Tag = " ";
                txtNetAmount.Tag = " ";
                ///////////////////////////////////////////////// 
                InitializeFormatDate(txtInvoiceDate);
                txtInvoiceDate.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseInvoiceDate;
                checkBox1.Checked = false;
                checkBox2.Checked = true;
                cmbMethodID.ItemIndex = 0;
                txtNotes.Text = "";
                lblInvoiceTotalBeforeDiscount.Text = "";
                lblCreditAccountName.Text = "";
                lblAdditionalAccountID.Text = MySession.GlobalDefaultPurchaseAddtionalAccountID;
                lblAdditionalAccountName.Text = "";
                lblCreditAccountID.Text = MySession.GlobalDefaultPurchaseCrditAccountID;
                lblNetAccountID.Text = MySession.GlobalDefaultPurchaseNetTypeID;
                lblInvoiceTotal.Text = "0";
                txtDiscountOnTotal.Text = "0";
                txtDiscountPercent.Text = "0";
                lblDiscountTotal.Text = "0";
                lblAdditionaAmmount.Text = "0";
                lblNetBalance.Text = "0";
                picItemUnits.Image = null;
                GetAccountsDeclaration();

                txtDelegateID.Text = MySession.GlobalDefaultSaleDelegateID;
                txtDelegateID_Validating(null, null);

                txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseDelegateID;
                txtCostCenterID_Validating(null, null);

                txtSellerID.Text = MySession.GlobalDefaultSellerID;
                txtSellerID_Validating(null, null);


                lblStoreID.Text = MySession.GlobalDefaultPurchaseStoreID;
                txtStoreID_Validating(null, null);

                lblDiscountDebitAccountID.Text = MySession.GlobalDefaultPurchaseDiscountAccountID;
                 
                lblStoreID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseStoreID;

                txtCostCenterID.Text = MySession.GlobalDefaultCostCenterID;
                if (MySession.GlobalAllowChangefrmPurchaseCostCenterID == false)
                    txtCostCenterID.ReadOnly = true;
                if (MySession.GlobalDefaultPurchasePayMethodID != "0")
                    cmbMethodID.EditValue = Comon.cInt(MySession.GlobalDefaultPurchasePayMethodID);
                else
                    cmbMethodID.EditValue = 1;
                cmbMethodID_EditValueChanged(null, null);
                cmbMethodID.ReadOnly = !MySession.GlobalAllowChangefrmPurchasePayMethodID;
                if (!MySession.GlobalAllowChangefrmPurchasePayMethodID)
                    switch (Comon.cInt(MySession.GlobalDefaultPurchasePayMethodID))
                    {
                        case 1:
                            simpleCashNet.Enabled = false;
                            simpleNet.Enabled = false;
                            simpleAgel.Enabled = false;
                            break;
                        case 2:
                            simpleCashNet.Enabled = false;
                            simpleNet.Enabled = false;
                            simpleCash.Enabled = false;
                            break;
                        case 3:
                            simpleCashNet.Enabled = false;
                            simpleAgel.Enabled = false;
                            simpleCash.Enabled = false;
                            break;
                        case 5:
                            simpleNet.Enabled = false;
                            simpleAgel.Enabled = false;
                            simpleCash.Enabled = false;
                            break;
                    }
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultPurchaseCurencyID);
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseCurencyID;
                txtSupplierID.Text = MySession.GlobalDefaultPurchaseSupplierID;
                txtSupplierID_Validating(null, null);
                txtSupplierID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseSupplierID;
                lstDetail = new BindingList<Sales_PurchaseInvoiceDetails>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;
                dt = new DataTable();
                chkForVat.Checked = true;
                picItemImage.Image = null;


                lstDetailSpend = new BindingList<Acc_SpendVoucherDetails>();
                lstDetailSpend.AllowNew = true;
                lstDetailSpend.AllowEdit = true;
                lstDetailSpend.AllowRemove = true;
                gridControlTransport.DataSource = lstDetailSpend;


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
                    strSQL = "SELECT TOP 1 * FROM " + Sales_PurchaseInvoicesDAL.TableName + " Where   Cancel =0 And TypeInvoice=2 and BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_PurchaseInvoicesDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Sales_PurchaseInvoicesDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Sales_PurchaseInvoicesDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Sales_PurchaseInvoicesDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Sales_PurchaseInvoicesDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_PurchaseInvoicesDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Sales_PurchaseInvoicesDAL();
                    long InvoicIDTemp = Comon.cLong(txtInvoiceID.Text);
                    InvoicIDTemp = cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                    {
                        ReadRecord(InvoicIDTemp);
                        ReadRecordTransPort(InvoicIDTemp);
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
                txtInvoiceID.Text = Sales_PurchaseInvoicesDAL.GetNewID(MySession.GlobalFacilityID, Comon.cInt(cmbBranchesID.EditValue), MySession.UserID,2).ToString();
                ClearFields();
                IdPrint = false;
                EnabledControl(true);
                cmbFormPrinting.EditValue = 1;
                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];

                //simpleCash_Click(null, null);
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
            dtItem.Columns.Add("SpendPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPriceUnite", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CurrencyID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CurrencyEquivalent", System.Type.GetType("System.Decimal"));

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
                dtItem.Rows[i]["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
                dtItem.Rows[i]["SpendPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpendPrice").ToString());
                dtItem.Rows[i]["CostPriceUnite"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPriceUnite").ToString());

                dtItem.Rows[i]["CurrencyID"] = cmbCurency.EditValue.ToString();
                dtItem.Rows[i]["CurrencyName"] = cmbCurency.Text;
                dtItem.Rows[i]["CurrencyPrice"] = txtCurrncyPrice.Text;
                dtItem.Rows[i]["CurrencyEquivalent"] = Comon.ConvertToDecimalPrice(Comon.cDec(txtCurrncyPrice.Text) * Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpendPrice").ToString()));



                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);


        }

      
        private void EmportItems()
        {
            DoNew();
            OleDbConnection oledbConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtExcelPath.Text + ";Extended Properties=Excel 12.0");
            cItemsStores Store = new cItemsStores();
            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "تأكيد الاسنيراد  ؟");
            if (!Yes)
                return;

            Application.DoEvents();
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            oledbConn.Open();

            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet$]", oledbConn);

            OleDbDataAdapter oleda = new OleDbDataAdapter();
            oleda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            oleda.Fill(dt);
            oledbConn.Close();

            if (dt.Rows.Count < 1)
                return;
            lstDetail = new BindingList<Sales_PurchaseInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            Sales_PurchaseInvoiceDetails obj = new Sales_PurchaseInvoiceDetails();


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                obj = new Sales_PurchaseInvoiceDetails();
                obj.ArbItemName = dt.Rows[i]["ITEM_NAME"].ToString();
                obj.EngItemName = dt.Rows[i]["ITEM_NAME"].ToString();
                obj.GroupID = Comon.cDbl(dt.Rows[i]["GroupID"].ToString());
                obj.CostPrice = Comon.ConvertToDecimalPrice(dt.Rows[i]["price"].ToString());
                obj.QTY = Comon.ConvertToDecimalPrice(dt.Rows[i]["GOLD_GRAM_W"].ToString());
                obj.Caliber = Comon.cInt(dt.Rows[i]["GOLD_CALIBER"].ToString());
                obj.Serials = dt.Rows[i]["ITEM_NO"].ToString();
                obj.BarCode = dt.Rows[i]["BarCode"].ToString();

                string Barcode = Lip.GetValue("select itemid from Sales_PurchaseInvoiceDetails Where   BranchID =" + MySession.GlobalBranchID+" and Barcode='" + obj.BarCode + "'");
                if (Comon.cInt(Barcode) > 0)
                {
                    Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "الصنف موجود مسبقا" + " هل تريد الاستمرار " + obj.BarCode);
                    if (Yes)
                        continue;
                    else
                    {
                        SplashScreenManager.CloseForm(false);
                        return;
                    }
                }



                decimal CostPrice = Comon.ConvertToDecimalPrice(obj.CostPrice.ToString());
                decimal Bones = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(obj.CostPrice.ToString()));

                decimal additonalVAlue = Comon.ConvertToDecimalPrice((CostPrice * MySession.GlobalPercentVat) / 100);
                if (Bones > 0)
                    additonalVAlue = Comon.ConvertToDecimalPrice((Bones * MySession.GlobalPercentVat) / 100);

                //سعر تكلفة مع مصاريف
                decimal SpendPrice = Comon.ConvertToDecimalPrice(CostPrice);
                //سعر تكلفة المحل
                decimal CaratPrice = Comon.ConvertToDecimalPrice(Comon.cDec(SpendPrice * Comon.cDec(MySession.Cost)));
                //سعر الكارت وهو البيع
                decimal SalePrice = Comon.ConvertToDecimalPrice(CaratPrice * Comon.cDec(MySession.sumvalue));

                obj.AdditionalValue = additonalVAlue;
                obj.SalePrice = SalePrice;
                obj.SpendPrice = SpendPrice;

                obj.Total = CostPrice;
                obj.Net = SpendPrice;
                obj.ArbGroupName = Lip.GetValue("Select  " + PrimaryName + " from Stc_ItemsGroups Where GroupID=" + obj.GroupID + " And BranchID =" + MySession.GlobalBranchID);
                obj.EngGroupName = obj.ArbGroupName;


                obj.ArbSizeName = obj.Caliber.ToString();
                obj.EngSizeName = obj.Caliber.ToString();


                lstDetail.Add(obj);

            }
            SumTotalBalanceAndDiscountread();

            gridControl.DataSource = lstDetail;
            SplashScreenManager.CloseForm(false);
        }
     
        protected override void DoSave()
        {
            try
            {
                //    //using (TransactionScope scope = new TransactionScope())
                //{


                // Debit Account
                //for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                //{
                //    if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(gridViewTransport.GetRowCellValue(i, "AccountID")), Comon.cInt(cmbBranchesID.EditValue), Comon.cDec(gridViewTransport.GetRowCellValue(i, "DebitAmount")), 1))
                //    {
                //        SplashScreenManager.CloseForm(false);
                //        Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + gridViewTransport.GetRowCellValue(i, "ArbAccountName").ToString());
                //        return;
                //    }
                //}

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
                if (!Validations.IsValidForm(this.tabFroms))
                    return;
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    foreach (GridColumn col in gridView1.Columns)
                    {
                        if (col.FieldName == GroupName)
                        {

                            var cellValue = gridView1.GetRowCellValue(i, col);

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                                return;
                            }
                        }
                    }
                }
                //for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                //{

                //    String BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();

                //    if (AddItems(i, BarCode) == false)
                //    {

                //        long ItemID = Comon.cInt(Lip.GetValue(" Select ItemID from Stc_ItemUnits  where BarCode='" + BarCode.Trim() + "'"));
                //        Lip.ExecututeSQL("Delete from Stc_ItemUnits Where ItemID=" + ItemID);
                //        Lip.ExecututeSQL("Delete from Stc_Items Where ItemID=" + ItemID);
                //        Lip.ExecututeSQL("Delete from Sales_PurchaseInvoiceDetails Where ItemID=" + ItemID);
                //        Messages.MsgInfo("يرجى التاكد من بيانات الصنف ", BarCode);
                //        return;
                //    }
                //}
                if (!IsValidGrid())
                    return;
                if (Comon.ConvertToDecimalPrice(lblNetBalance.Text) < Comon.ConvertToDecimalPrice(txtNetAmount.Text))
                {
                    txtNetAmount.Focus();
                    txtNetAmount.ToolTip = "مبلغ الشبكة  اكبر من الصافي ";
                    Validations.ErrorText(txtNetAmount, txtNetAmount.ToolTip);
                    return;
                }
                // التاكد من أنه تم توزيع المصاريف 
                decimal TotalBones = 0;
                for (int i = 0; i <=gridView1.DataRowCount; i++)
                {
                    TotalBones += Comon.cDec(gridView1.GetRowCellValue(i, "Bones"));
                }
                if (Comon.cDec(txtTransportAmount.Text) != TotalBones)
                {
                    SplashScreenManager.CloseForm();
                    Messages.MsgWarning(Messages.TitleWorning, "الرجاء توزيع المصاريف على الأصناف قبل عملية الحفظ ");
                    tabControl.SelectedTabPageIndex = 1;
                    return;
                }

                if (!Validations.IsValidFormCmb(cmbCurency))
                    return;
                if (Comon.ConvertToDecimalPrice(lblDiscountTotal.Text) > 0)
                    lblDiscountDebitAccountID.Tag = "ImportantFieldGreaterThanZero";
                else
                    lblDiscountDebitAccountID.Tag = "IsNumber";
                Save();
                //    scope.Complete(); 
                //  }
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
        public void ReadRecordTransPort(long VoucherID, bool flag = false)
        {
            try
            {
                dt = SpendVoucherDAL.frmGetDataDetalTransPortByID(VoucherID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //GridVeiw
                    gridControlTransport.DataSource = dt;
                    lstDetail.AllowNew = true;
                    lstDetail.AllowEdit = true;
                    lstDetail.AllowRemove = true;
                }
                CalculatTotalTransPort();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void ReadRecord(long InvoiceID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                    dt = Sales_PurchaseInvoicesDAL.frmGetDataDetalByID(InvoiceID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,2);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        CountTrans = 0;
                        IsNewRecord = false;
                        button1.Visible = true;
                        //Validate
                        lblStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);
                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        StopSomeCode = true;
                        cmbMethodID.EditValue = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());
                        StopSomeCode = false;
                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        cmbNetType.EditValue = Comon.cDbl(dt.Rows[0]["NetType"].ToString());
                        txtSupplierID.Text = dt.Rows[0]["SupplierID"].ToString();
                        txtSupplierID_Validating(null, null);
                        //cmbSellerID.EditValue = dt.Rows[0]["SellerID"].ToString();
                        txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        txtDelegateID_Validating(null, null);
                        //Account
                        lblAdditionalAccountID.Text = dt.Rows[0]["AdditionalAccount"].ToString();
                        lblAdditionalAccountID_Validating(null, null);
                        //Masterdata
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();
                        //Date                      
                        txtInvoiceDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()), "dd/MM/yyyy", culture);
                        //Ammount
                        txtNetAmount.Text = dt.Rows[0]["NetAmount"].ToString();
                        txtNetProcessID.Text = dt.Rows[0]["NetProcessID"].ToString();
                        txtTransportAmount.Text = dt.Rows[0]["TransportDebitAmount"].ToString();
                        // txtVatID.Text = dt.Rows[0]["VatID"].ToString();
                        txtDiscountOnTotal.Text = dt.Rows[0]["DiscountOnTotal"].ToString();

                        lblNetAccountID.Text = dt.Rows[0]["NetAccount"].ToString();
                        lblNetAccountID_Validating(null, null);
                        //حقول محسوبة 
                        lblDiscountTotal.Text = "0";
                        lblInvoiceTotal.Text = dt.Rows[0]["InvoiceTotal"].ToString();
                        lblAdditionaAmmount.Text = dt.Rows[0]["AdditionaAmountTotal"].ToString();
                        lblNetBalance.Text = dt.Rows[0]["NetBalance"].ToString();
                        lblCreditAccountID.Text = dt.Rows[0]["CreditAccount"].ToString();
                        lblCreditAccountID_Validating(null, null);
                        lblDiscountDebitAccountID.Text = dt.Rows[0]["DiscountCreditAccount"].ToString();
                        lblDiscountDebitAccountID_Validating(null, null);
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
                        Validations.DoReadRipon(this, ribbonControl1);
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
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
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
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1 || Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(lblNetBalance.Text) + Comon.cDbl(lblAdditionaAmmount.Text);
                returned.CreditGold = Comon.cDbl(lblTotalWeight.Text);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 3)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblNetAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(lblNetBalance.Text) + Comon.cDbl(lblAdditionaAmmount.Text);
                returned.CreditGold = Comon.cDbl(lblTotalWeight.Text);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 5)
            {
                GetAccountsDeclaration();
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = (Comon.cDbl(lblNetBalance.Text) + Comon.cDbl(lblAdditionaAmmount.Text)) - Comon.cDbl(txtNetAmount.Text);

                returned.CreditGold = Comon.cDbl(lblTotalWeight.Text);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);

                if (Comon.cDbl(txtNetAmount.Text) > 0)
                {
                    returned = new Acc_VariousVoucherMachinDetails();
                    returned.ID = 2;
                    returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.AccountID = Comon.cDbl(lblNetAccountID.Text);
                    returned.VoucherID = VoucherID;
                    returned.Credit = Comon.cDbl(txtNetAmount.Text);
                    returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                    returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                    listreturned.Add(returned);
                }
            }
            //Discount
            if (Comon.cDbl(lblDiscountTotal.Text) > 0)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 4;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDiscountDebitAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(lblDiscountTotal.Text);

                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }
            //Debit Purchase          
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 3;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            //returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
            returned.VoucherID = VoucherID;
            //returned.Debit = Comon.cDbl(lblInvoiceTotal.Text) + Comon.cDbl(lblInvoiceTotalOjor.Text);
            returned.Debit = Comon.cDbl(lblNetBalance.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);
            //===
            //Vat Purchase
            if (chkForVat.Checked)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 4;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblAdditionalAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Debit = Comon.cDbl(lblAdditionaAmmount.Text);
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }
            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        long SaveVariousVoucherMachinContinuousInv(int DocumentID)
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
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = 1;

            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); 
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
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1 || Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID; 
                returned.Credit = Comon.cDbl(Comon.cDbl( Comon.cDec( lblNetBalance.Text)) - Comon.cDbl(lblDiscountTotal.Text))-Comon.cDbl(txtTransportAmount.Text);
                if (Comon.cInt(cmbCurency.EditValue) == 1)
                    returned.CreditGold = Comon.cDbl(lblTotalWeight.Text);
                else
                    returned.CreditMatirial = Comon.cDbl(lblTotalWeight.Text);
                returned.Debit = 0;
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 

                listreturned.Add(returned);
            }
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 3)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblNetAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(lblDiscountTotal.Text)) - Comon.cDbl(txtTransportAmount.Text);
                if (Comon.cInt(cmbCurency.EditValue) == 1)
                    returned.CreditGold = Comon.cDbl(lblTotalWeight.Text);
                else
                    returned.CreditMatirial = Comon.cDbl(lblTotalWeight.Text);
                returned.Debit = 0;

                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }

            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 5)
            {
                //GetAccountsDeclaration();
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit =Comon.cDbl( (Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(lblDiscountTotal.Text)) - Comon.cDbl(txtNetAmount.Text))-Comon.cDbl(txtTransportAmount.Text);
                if (Comon.cInt(cmbCurency.EditValue) == 1)
                    returned.CreditGold = Comon.cDbl(lblTotalWeight.Text);
                else
                    returned.CreditMatirial = Comon.cDbl(lblTotalWeight.Text);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);

                if (Comon.cDbl(txtNetAmount.Text) > 0)
                {
                    returned = new Acc_VariousVoucherMachinDetails();
                    returned.ID = 2;
                    returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.AccountID = Comon.cDbl(lblNetAccountID.Text);
                    returned.VoucherID = VoucherID;
                    returned.Credit = Comon.cDbl(txtNetAmount.Text);
                    returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                    returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                    returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                    returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                    returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                    listreturned.Add(returned);
                }

            }

            for (int i = 0; i < gridViewTransport.DataRowCount;i++ )
            {

                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 4;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridViewTransport.GetRowCellValue(i,"AccountID"));
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(gridViewTransport.GetRowCellValue(i,"DebitAmount"));
                returned.Declaration = gridViewTransport.GetRowCellValue(i,"Declaration").ToString();
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }

            //Discount
            if (Comon.cDbl(lblDiscountTotal.Text) > 0)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 4;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDiscountDebitAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(lblDiscountTotal.Text);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }
            //Debit Purchase
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 3;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;
            if(Comon.cInt(cmbCurency.EditValue)==1)
               returned.DebitGold = Comon.cDbl(lblTotalWeight.Text);
            else
                returned.DebitMatirial = Comon.cDbl(lblTotalWeight.Text);
            //returned.Debit = Comon.cDbl(lblInvoiceTotal.Text) + Comon.cDbl(lblInvoiceTotalOjor.Text);
            returned.Debit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text)-Comon.cDbl(lblAdditionaAmmount.Text));
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);
            //===
            //Vat Purchase
            if (chkForVat.Checked)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 4;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblAdditionalAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.DebitGold = 0;
                returned.CreditGold = 0;
                returned.Debit = Comon.cDbl(lblAdditionaAmmount.Text);
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }
            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        private void SaveTransPortAmountDetail()

        {
            Acc_SpendVoucherMaster objRecord = new Acc_SpendVoucherMaster();
            Acc_SpendVoucherDetails returned;
            List<Acc_SpendVoucherDetails> listreturned = new List<Acc_SpendVoucherDetails>();
            int VoucherID = Comon.cInt(txtInvoiceID.Text);
            objRecord.SpendVoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            for (int i = 0; i <= gridViewTransport.DataRowCount - 1; i++)
            {
                returned = new Acc_SpendVoucherDetails();
                returned.ID = i;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridViewTransport.GetRowCellValue(i, "AccountID").ToString());
                returned.SpendVoucherID = VoucherID;
                returned.DebitAmount = Comon.cDbl(gridViewTransport.GetRowCellValue(i, "DebitAmount").ToString());
                returned.Discount = Comon.cDbl(gridViewTransport.GetRowCellValue(i, "Discount").ToString());
                returned.Declaration = gridViewTransport.GetRowCellValue(i, "Declaration").ToString();
                returned.CostCenterID = Comon.cInt(gridViewTransport.GetRowCellValue(i, "CostCenterID").ToString());
                returned.CurrencyID = Comon.cInt(gridViewTransport.GetRowCellValue(i, "CurrencyID").ToString());
                returned.CurrencyName = gridViewTransport.GetRowCellValue(i, "CurrencyName").ToString();
                returned.CurrencyPrice = Comon.cDbl(gridViewTransport.GetRowCellValue(i, "CurrencyPrice").ToString());
                returned.CurrencyEquivalent = Comon.cDbl(gridViewTransport.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                returned.Barcode = "";
                returned.ItemName = "";
                returned.WeightGold = 0;
                returned.QtyGoldEqulivent = 0;
                returned.Calipar = 0;
                listreturned.Add(returned);
            }
            if (listreturned.Count > 0)
            {
                objRecord.SpendVoucherDetails = listreturned;
                 long Result = SpendVoucherDAL.InsertUsingXMLTransPortDetails(objRecord, IsNewRecord);
            }
        }
        private int SaveStockMoveing(int DocumentID)
        {   
            Stc_ItemsMoviing objRecord=new Stc_ItemsMoviing();
            objRecord.FacilityID=UserInfo.FacilityID;
            objRecord.BranchID=Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID  = DocumentType;
            objRecord.MoveType=1;             
            objRecord.MoveID = 0;
            objRecord.TranseID=DocumentID;
            Stc_ItemsMoviing returned;
              List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
			{
                 returned = new Stc_ItemsMoviing();
                 returned.ID = i+1;
                 returned.MoveDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();               
			     returned.MoveID = 0;
                 returned.TranseID=DocumentID;
                 returned.FacilityID=UserInfo.FacilityID;
                 returned.BranchID=Comon.cInt(cmbBranchesID.EditValue);
                 returned.DocumentTypeID  = DocumentType;
                 returned.MoveType=1;
                 returned.StoreID=Comon.cDbl(lblStoreID.Text.ToString());
                 returned.AccountID = Comon.cDbl(txtSupplierID.Text);
                 returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                 returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                 returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                 returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " And BranchID =" + MySession.GlobalBranchID));
                 returned.QTY = Comon.cDbl(gridView1.GetRowCellValue(i, "QTY").ToString());
                 returned.InPrice = (Comon.cDbl(gridView1.GetRowCellValue(i, "Total").ToString()) + Comon.cDbl(gridView1.GetRowCellValue(i, "Bones").ToString()) )/ returned.QTY;
            
                 //returned.Bones = Comon.cDbl(gridView1.GetRowCellValue(i, "Bones").ToString());
                 returned.OutPrice=0;
                 returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                 returned.Cancel = 0;
                 listreturned.Add(returned);
			}
            if (listreturned.Count > 0)
            {

                objRecord.ObjDatails = listreturned;
                string Result =  Stc_ItemsMoviingDAL.Insert(objRecord, IsNewRecord);
             
                return Comon.cInt( Result);
               }
            return 0;
        }
        private void Save()
        {
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            Sales_PurchaseInvoiceMaster objRecord = new Sales_PurchaseInvoiceMaster();
            objRecord.InvoiceID = 0;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.MethodeID = Comon.cInt(cmbMethodID.EditValue);
            objRecord.CurencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.TypeInvoice = 2;
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);
            objRecord.NetType = Comon.cInt(cmbNetType.EditValue) != 0 ? Comon.cDbl(cmbNetType.EditValue) : 1.0;
            objRecord.SupplierInvoiceID = Comon.cInt(txtSupplierInvoiceID.Text);
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cDbl(lblStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SupplierID = Comon.cDbl(txtSupplierID.Text);
            objRecord.SupplierName = lblSupplierName.Text;

            //objRecord.SellerID = Comon.cInt(cmbSellerID.EditValue);

            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Purchase  Invoice" : " فاتوره  مشتريات    ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Purchase  Invoice" : " فاتوره  مشتريات     "));


            objRecord.Notes = txtNotes.Text;
            //Account

            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            objRecord.DiscountCreditAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
            objRecord.WeightTotal = Comon.cDec(lblTotalWeight.Text);

            objRecord.NetAccount = Comon.cDbl(lblNetAccountID.Text);
            objRecord.AdditionalAccount = Comon.cDbl(lblAdditionalAccountID.Text);
            objRecord.NetProcessID = txtNetProcessID.Text;
            objRecord.VATID = txtVatID.Text;
            //Date 
            //Ammount
            objRecord.NetAmount = Comon.cDbl(txtNetAmount.Text);
            objRecord.DiscountOnTotal = Comon.cDbl(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = (Comon.cDec(lblInvoiceTotalBeforeDiscount.Text));
            objRecord.AdditionaAmountTotal = Comon.cDec(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Comon.cDbl(lblNetBalance.Text);
            objRecord.TransportDebitAmount = Comon.cDbl(txtTransportAmount.Text);
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
            if (IsNewRecord == false)
            {
                objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            Sales_PurchaseInvoiceDetails returned;
            List<Sales_PurchaseInvoiceDetails> listreturned = new List<Sales_PurchaseInvoiceDetails>();

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Sales_PurchaseInvoiceDetails();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                returned.Bones = Comon.cDec(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.Description = gridView1.GetRowCellValue(i, ItemName).ToString();
                returned.StoreID = Comon.cDbl(lblStoreID.Text);
                returned.Discount = Comon.cDec(gridView1.GetRowCellValue(i, "Discount").ToString());
                returned.ExpiryDateStr = 0;
                returned.CostPrice = Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice").ToString()); 
                returned.SalePrice = Comon.cDec(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                returned.AdditionalValue = Comon.cDec(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                returned.SpendPrice = Comon.cDec(gridView1.GetRowCellValue(i, "SpendPrice").ToString());
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue);
                returned.CurrencyName = cmbCurency.Text;
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
             
                returned.Serials = gridView1.GetRowCellValue(i, "Serials").ToString();
                if (returned.BarCode.Trim() == "24")
                    returned.AdditionalValue = 0;
                returned.Net = Comon.cDec(gridView1.GetRowCellValue(i, "Net").ToString());
                returned.CostPriceUnite = Comon.cDec(gridView1.GetRowCellValue(i, "CostPriceUnite").ToString());
                returned.Total = Comon.cDec(gridView1.GetRowCellValue(i, "Total").ToString());
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Total) * Comon.cDbl(returned.CurrencyPrice));
                if (returned.AdditionalValue == 0)
                    returned.HavVat = false;
                else
                    returned.HavVat = true;
                returned.Cancel = 0;


                listreturned.Add(returned);
            }
            if (listreturned.Count > 0)
            {

                objRecord.PurchaseDatails = listreturned;


                string Result = Sales_PurchaseInvoicesDAL.InsertUsingXML(objRecord, Comon.cInt(MySession.UserID), IsNewRecord).ToString();
               
                //حفظ القيد الالي
                if (Comon.cInt(Result) > 0)
                {
                    //حفظ مصاريف الشراء  
                    SaveTransPortAmountDetail();
                }
                //// حفظ الحركة المخزنية 
                //if (Comon.cInt(Result) > 0)
                //{
                //    int MoveID = SaveStockMoveing(Comon.cInt(Result));
                //    if (MoveID == 0)
                //        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
                //}
                
                if (Comon.cInt(Result) > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = 0;
                    if (MySession.GlobalInventoryType == 2)// جرد دوري 
                        VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result));
                    else if (MySession.GlobalInventoryType == 1)//جرد مستمر 
                        VoucherID = SaveVariousVoucherMachinContinuousInv(Comon.cInt(Result));
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + Sales_SaleInvoicesDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sales_SaleInvoicesDAL.PremaryKey + " = " + txtInvoiceID.Text + " And BranchID =" + MySession.GlobalBranchID);

                }
                SplashScreenManager.CloseForm(false);


                if (IsNewRecord == true)
                {
                    if (Comon.cLong(Result) > 0)
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
                //bool FlageChecKDelete = false;

                //for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                //{

                //    FlageChecKDelete = Lip.CheckTheItemIsHaveTransactionByBarCode(gridView1.GetRowCellValue(i, "BarCode").ToString(), "Sales_PurchaseInvoiceDetails");

                //    if (FlageChecKDelete)
                //    {
                //        SplashScreenManager.CloseForm();
                //        Messages.MsgError("Error Delete ", "لا يمكن حذف الصنف بسبب وجود عمليات محاسبية علية");
                //        return;
                //    }
                //}
                #region Delete Type Diamond
                //for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                //{
                //    Sales_PurchaseDiamondDetails modelDeletDimondType = new Sales_PurchaseDiamondDetails();
                //    modelDeletDimondType.InvoiceID = Comon.cInt(txtInvoiceID.Text);

                //    modelDeletDimondType.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                //    modelDeletDimondType.FacilityID = UserInfo.FacilityID;
                //    modelDeletDimondType.BarCodeItem = gridView1.GetRowCellValue(i, "BarCode").ToString();
                //    modelDeletDimondType.TypeOpration = 1;
                //    int ResultDeleteType = Sales_PurchaseDiamondDetailsDAL.Delete(modelDeletDimondType);
                //}
                 #endregion
                Sales_PurchaseInvoiceMaster model = new Sales_PurchaseInvoiceMaster();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.TypeInvoice = 2;
                int Result = Sales_PurchaseInvoicesDAL.DeleteSales_PurchaseInvoiceMaster(model);
                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                  int MoveID=  DeleteStockMoving(Comon.cInt(Result));
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
                ReportName = "rptPurchaseMatirial";
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
                rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = txtCustomerMobile.Text.Trim().ToString();
                // rptForm.Parameters["TransportAmount"].Value = (Comon.ConvertToDecimalPrice(lblTotalSalePrice17.Text)).ToString();
                rptForm.Parameters["TotalCost"].Value = Comon.ConvertToDecimalQty(lblInvoiceTotalOjor.Text);

                rptForm.Parameters["BranchName"].Value = cmbBranchesID.Text;
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text;
                rptForm.Parameters["TotalWightGold"].Value = (Comon.ConvertToDecimalQty(lblTotalWeight.Text) + Comon.ConvertToDecimalQty(lbl21.Text) + Comon.ConvertToDecimalQty(lbl22.Text) + Comon.ConvertToDecimalQty(lbl24.Text)).ToString();

                rptForm.Parameters["TotalGold"].Value = Comon.ConvertToDecimalQty(lblTotalWeight.Text);
                rptForm.Parameters["TotalSale"].Value = (Comon.ConvertToDecimalQty(lblInvoiceTotal.Text)).ToString();
                rptForm.Parameters["ReportName"].Value =this.Text.ToString();
                rptForm.Parameters["G18"].Value = lblTotalWeight.Text;
                rptForm.Parameters["G21"].Value = lbl21.Text;
                rptForm.Parameters["G22"].Value = lbl22.Text;
                rptForm.Parameters["G24"].Value = lbl24.Text;

                rptForm.Parameters["UserID"].Value = UserInfo.ID;
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
                   
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["CostPrice"] = gridView1.GetRowCellValue(i, "CostPrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();

                    row["Total"] = (Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(row["CostPrice"]) * Comon.ConvertToDecimalQty(row["QTY"])) + Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones"))) - (Comon.ConvertToDecimalPrice(row["Discount"]))).ToString();
                    row["Net"] = (Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(row["Total"]) + Comon.ConvertToDecimalPrice(row["AdditionalValue"]))).ToString();

                    row["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
                    row["Bones"] = gridView1.GetRowCellValue(i, "Bones").ToString();
                    //row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();

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

        void PrintPySalePrice()
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
                ReportName = "rptPurchaseInvoiceMatirialSalePricre";
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
                rptForm.Parameters["UnitDiscount"].Value = lblNetBalance.Text.Trim().ToString();

                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = txtCustomerMobile.Text.Trim().ToString();

                rptForm.Parameters["TotalGold"].Value = (Comon.ConvertToDecimalQty(lblTotalWeight.Text) + Comon.ConvertToDecimalQty(lbl21.Text) + Comon.ConvertToDecimalQty(lbl22.Text) + Comon.ConvertToDecimalQty(lbl24.Text)).ToString();

                // rptForm.Parameters["TotalGold"].Value = (Comon.ConvertToDecimalQty(lbl18.Text) + Comon.ConvertToDecimalQty(lbl21.Text) + Comon.ConvertToDecimalQty(lbl22.Text) + Comon.ConvertToDecimalQty(lbl24.Text)).ToString();

                rptForm.Parameters["ReportName"].Value = this.Text.ToString();
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
                    row["ExpiryDate"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["CostPrice"] = gridView1.GetRowCellValue(i, "CostPrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();

                    row["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
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
            if (chkprintpysale.Checked == true)
                PrintPySalePrice();
            else
                PrintPyCostPrice();
        }
        #endregion
        #endregion
        #region Event

        #region Validating
        private void txtInvoiceID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
            {
                ReadRecord(Comon.cLong(txtInvoiceID.Text));
                ReadRecordTransPort(Comon.cLong(txtInvoiceID.Text));
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
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cDbl(lblStoreID.Text) + " And Cancel =0 And BranchID =" + MySession.GlobalBranchID;
                CSearch.ControlValidating(lblStoreID, lblStoreName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

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

                if (txtDiscountOnTotal.Text != string.Empty & lblInvoiceTotalBeforeDiscount.Text != string.Empty)
                {
                    decimal DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                    decimal whole = Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text);
                    decimal TotalDiscount = DiscountOnTotal;
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
                    lblDiscountTotal.Text = (DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);

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
                if (!string.IsNullOrEmpty(txtDiscountPercent.Text) && !string.IsNullOrEmpty(lblInvoiceTotalBeforeDiscount.Text))
                {
                    decimal percent = Comon.ConvertToDecimalPrice(txtDiscountPercent.Text);
                    decimal whole = Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text);
                    decimal calculatedDiscount = Comon.ConvertToDecimalPrice((percent * whole) / 100);  

                    if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) != calculatedDiscount  )
                    {
                        txtDiscountOnTotal.Text = calculatedDiscount.ToString("N" + MySession.GlobalPriceDigits);
                        lblDiscountTotal.Text = calculatedDiscount.ToString("N" + MySession.GlobalPriceDigits);
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


        private void lblCreditAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                 
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) And BranchID =" + MySession.GlobalBranchID+" AND (AccountID = " + lblCreditAccountID.Text + ") ";
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
                
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE     (Cancel = 0) And BranchID =" + MySession.GlobalBranchID+" AND (AccountID = " + lblAdditionalAccountID.Text + ") ";
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
                 
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) And BranchID =" + MySession.GlobalBranchID+" AND (AccountID = " + lblDiscountDebitAccountID.Text + ") ";
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
               
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) And BranchID =" + MySession.GlobalBranchID+" AND (AccountID = " + lblNetAccountID.Text + ") ";
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
      
      
        


        #endregion
        /************************Event From **************************/
        /* *******************Event CheckBoc***************************/

        public void chForVat_EditValueChanged(object sender, EventArgs e)
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
            dtItem.Columns.Add("SpendPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPriceUnite", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CurrencyID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CurrencyEquivalent", System.Type.GetType("System.Decimal"));

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
                dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); 
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
                dtItem.Rows[i]["Serials"] = gridView1.GetRowCellValue(i, "Serials").ToString();
                dtItem.Rows[i]["SpendPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpendPrice").ToString());
                dtItem.Rows[i]["CostPriceUnite"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPriceUnite").ToString());

                dtItem.Rows[i]["CurrencyID"] = cmbCurency.EditValue.ToString();
                dtItem.Rows[i]["CurrencyName"] = cmbCurency.Text;
                dtItem.Rows[i]["CurrencyPrice"] = txtCurrncyPrice.Text;
                dtItem.Rows[i]["CurrencyEquivalent"] = Comon.ConvertToDecimalPrice(Comon.cDec(txtCurrncyPrice.Text) * Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SpendPrice").ToString()));


                //if (chkForVat.Checked == true)
                //    dtItem.Rows[i]["HavVat"] = true;
                //else
                //    dtItem.Rows[i]["HavVat"] = false;

              

                if (chkForVat.Checked == false)               
                {
                    dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(dtItem.Rows[i]["Bones"]) + Comon.ConvertToDecimalPrice(dtItem.Rows[i]["Total"]);
                    dtItem.Rows[i]["AdditionalValue"] = 0;
                }
                else  
                {
                    if (Comon.cbool(dtItem.Rows[i]["HavVat"])==true)
                      dtItem.Rows[i]["AdditionalValue"] =( Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString())*MySession.GlobalPercentVat)/100;
                    else
                        dtItem.Rows[i]["AdditionalValue"] = 0;
                    dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(dtItem.Rows[i]["Bones"]) + Comon.ConvertToDecimalPrice(dtItem.Rows[i]["Total"]) + Comon.ConvertToDecimalPrice(dtItem.Rows[i]["AdditionalValue"]);
                
                }


                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            gridView1.Focus();
            gridView1.MoveNext();
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            CalculateRow(IsHavVat:chkForVat.Checked);
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

                txtNetProcessID.Text = "";
                txtNetAmount.Text = "";

                cmbNetType.ItemIndex = -1;
                txtNetAmount.Visible = false;
                lblNetAmount.Visible = false;
                lblnetType.Visible = false;
                cmbNetType.Visible = false;


                txtNetProcessID.Tag = "IsNumber";
                txtNetAmount.Tag = "IsNumber";
                lblCreditAccountID.Tag = "ImportantFieldGreaterThanZero";
                lblStoreID.Tag = "ImportantFieldGreaterThanZero";
                if (value == 1)
                {
                    txtSupplierID.Tag = "IsNumber";
                    lblNetAccountID.Tag = "IsNumber";
                    lblCreditAccountID.Tag = "ImportantFieldGreaterThanZero";
                   if (string.IsNullOrEmpty(MySession.GlobalDefaultPurchaseCrditAccountID) == false)                        
                    {
                        lblCreditAccountID.Text = MySession.GlobalDefaultPurchaseCrditAccountID;
                        lblCreditAccountID_Validating(null, null);
                    }
                    cmbNetType.Tag = "";
                    simpleCash_Click(null, null);
                    // txtCustomerName.Focus();
                    {
                        lblNetAccountCaption.Enabled = false;
                        lblNetAccountID.Enabled = false;
                        lblNetAccountName.Enabled = false;
                        lblCachCaption.Enabled = true;
                        lblCreditAccountID.Enabled = true;
                        lblCreditAccountName.Enabled = true;
                    }
                }
                else if (value == 2)
                {
                    txtSupplierID.Visible = true;
                    lblSupplierName.Visible = true;

                    lblSupplierName.Text = "";
                    txtSupplierID.Text = "";

                    lblCreditAccountID.Tag = "IsNumber";
                    txtSupplierID.Tag = "ImportantFieldGreaterThanZero";
                   
                    if (StopSomeCode == false)
                    {

                    }
                    cmbNetType.Tag = "";
                    simpleAgel_Click(null, null);
                    {
                        lblNetAccountCaption.Enabled = false;
                        lblNetAccountID.Enabled = false;
                        lblNetAccountName.Enabled = false;
                        lblCachCaption.Enabled = false;
                        lblCreditAccountID.Enabled = false;
                        lblCreditAccountName.Enabled = false;
                    }
                    Find();
                }
                else if (value == 3)
                {
                    if (string.IsNullOrEmpty(MySession.GlobalDefaultPurchaseCrditAccountID) == false)
                       
                    {
                        lblNetAccountID.Text = MySession.GlobalDefaultPurchaseNetTypeID;
                        lblCreditAccountID_Validating(null, null);
                    }


                    lblCreditAccountID.Tag = "IsNumber";
                    txtSupplierID.Tag = "IsNumber";
                    lblNetAccountID.Tag = "ImportantFieldGreaterThanZero";
                    
                    lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = false;
                    lblNetAmount.Visible = false;
                    lblnetType.Visible = true;
                    cmbNetType.Visible = true;
                    cmbNetType.ReadOnly = false;
                    cmbNetType.ItemIndex = 0;// Comon.cDbl(lblDebitAccountID.Text);
                     //txtNetProcessID.Tag = "ImportantFieldGreaterThanZero";
                     //txtNetAmount.Tag = "ImportantFieldGreaterThanZero";
                    cmbNetType.Tag = "ImportantField";
                    cmbNetType.Enabled = true;
                    simpleNet_Click(null, null);
                    {
                        lblNetAccountCaption.Enabled = true;
                        lblNetAccountID.Enabled = true;
                        lblNetAccountName.Enabled = true;
                        lblCachCaption.Enabled = false;
                        lblCreditAccountID.Enabled = false;
                        lblCreditAccountName.Enabled = false;
                    }
                }
                else if (value == 4)
                {
                    // حساب الشيكات


                    lblNetProcessID.Visible = false;
                    txtNetProcessID.Visible = false;
                    txtNetAmount.Visible = false;
                    lblNetAmount.Visible = false;
                    lblnetType.Visible = false;
                    cmbNetType.Visible = false;

                    cmbNetType.Tag = "";


                }
                else if (value == 5)
                {

                    chkForVat.Checked = true;
                    txtSupplierID.Tag = "IsNumber";
                    lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = true;
                    lblNetAmount.Visible = true;
                    lblnetType.Visible = true;
                    cmbNetType.Visible = true;
                    cmbNetType.EditValue = Comon.cDbl(MySession.GlobalDefaultPurchaseNetTypeID);
                    txtNetProcessID.Tag = " ";
                    txtNetAmount.Tag = "ImportantFieldGreaterThanZero";
                    lblNetAccountID.Tag = "ImportantFieldGreaterThanZero";
                    lblCreditAccountID.Tag = "ImportantFieldGreaterThanZero";
                    cmbNetType.Tag = "ImportantField";
                   
                    simpleCashNet_Click(null, null);
                    {
                        lblNetAccountCaption.Enabled = true;
                        lblNetAccountID.Enabled = true;
                        lblNetAccountName.Enabled = true;
                        lblCachCaption.Enabled = true;
                        lblCreditAccountID.Enabled = true;
                        lblCreditAccountName.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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

            //btnDebitSearch.Enabled = MySession.GlobalAllowChangefrmSaleDebitAccountID;
            //btnCreditSearch.Enabled = MySession.GlobalAllowChangefrmSaleCreditAccountID;
            //btnAdditionalSearch.Enabled = MySession.GlobalAllowChangefrmSaleAdditionalAccountID;
            //btnNetSearch.Enabled = MySession.GlobalAllowChangefrmSaleNetAccountID;
            //btnChequeSearch.Enabled = MySession.GlobalAllowChangefrmSaleChequeAccountID;
            //btnDiscountDebitSearch.Enabled = MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID;

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



        private void button1_Click(object sender, EventArgs e)
        {
            dt = Sales_PurchaseInvoicesDAL.frmGetDataDetalByID(Comon.cInt(txtInvoiceID.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
            if (dt.Rows.Count < 1)
                return;
            strSQL = "Select * from  Sales_PurchaseInvoiceReturnMaster where SupplierInvoiceID=" + txtInvoiceID.Text + "  And BranchID =" + MySession.GlobalBranchID+" And BranchID=" + MySession.GlobalBranchID + " And  Cancel=0";
            DataTable dtReturn = new DataTable();
            dtReturn = Lip.SelectRecord(strSQL);

            if (dtReturn.Rows.Count > 0)
            {
                Messages.MsgError(Messages.TitleError, " يوجد فاتورة مردودات سابقة لهذه الفاتورة");
                frmCashierPurchaseReturnMatirial frm = new frmCashierPurchaseReturnMatirial();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.IsNewRecord = false;
                }
                else
                    frm.Dispose();
                frm.cmbBranchesID.EditValue = cmbBranchesID.EditValue;
                frm.txtSupplierInvoiceID.Text = txtInvoiceID.Text;
                frm.txtSupplierInvoiceID_Validating(null, null);
                //frm.txtSupplierInvoiceID.Text = txtInvoiceID.Text;
                frm.chkForVat.Checked = chkForVat.Checked;
                frm.lblTotalWeight.Text = lblTotalWeight.Text;
            }
            else
            {
                frmCashierPurchaseReturnMatirial frm = new frmCashierPurchaseReturnMatirial();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.IsNewRecord = true;
                }
                dt = Sales_PurchaseInvoicesDAL.frmGetDataDetalByID(Comon.cLong(txtInvoiceID.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                frm.fillMAsterData(dt);
                frm.lblInvoiceTotalBeforeDiscount.Text = lblInvoiceTotalBeforeDiscount.Text;
                frm.lblNetBalance.Text = lblNetBalance.Text;
                frm.lblAdditionaAmmount.Text = lblAdditionaAmmount.Text;
                frm.txtSupplierInvoiceID.Text = txtInvoiceID.Text;
                frm.cmbBranchesID.EditValue = cmbBranchesID.EditValue;
                frm.chkForVat.Checked = chkForVat.Checked;
                frm.lblTotalWeight.Text = lblTotalWeight.Text;
                //frm.CalculateRow(-1, true);

                //frm.txtwhghtGold.Text = txtwhghtGold.Text;

            }
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblCheckID_Click(object sender, EventArgs e)
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

            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"])) + Comon.ConvertToDecimalPrice(strQty.Trim()));
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




        private void txtInvoiceDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
                txtInvoiceDate.EditValue = DateTime.Now;
            //if (Comon.ConvertDateToSerial(txtInvoiceDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
            //    txtInvoiceDate.Text = Lip.GetServerDate();
            if (Lip.CheckDateISAvilable( txtInvoiceDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                txtInvoiceDate.Text = Lip.GetServerDate();
                return;
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {

        }

        private void showCustomers(bool p, int f)
        {

            txtVatID.Text = "";
            labelControl6.Visible = p;
            labelControl4.BringToFront();
            labelControl4.Visible = p;
            txtVatID.Visible = p;

        }

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {


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


        public void btnUsengGold_Click(object sender, EventArgs e)
        {


            frmSettingConnection frm = new frmSettingConnection();
            frm.ShowDialog();


            strSQL = @"SELECT    Sales_PurchaseInvoiceDetails.ID, Sales_PurchaseInvoiceDetails.InvoiceID, Sales_PurchaseInvoiceDetails.BranchID, Sales_PurchaseInvoiceDetails.ItemID, Sales_PurchaseInvoiceDetails.SizeID, 
                  Sales_PurchaseInvoiceDetails.QTY, Sales_PurchaseInvoiceDetails.CostPrice, Sales_PurchaseInvoiceDetails.Bones, Sales_PurchaseInvoiceDetails.StoreID, Sales_PurchaseInvoiceDetails.Discount, 
                  Sales_PurchaseInvoiceDetails.ExpiryDate, Sales_PurchaseInvoiceDetails.SalePrice, Sales_PurchaseInvoiceDetails.AdditionaAmmount, Sales_PurchaseInvoiceDetails.BarCode, Sales_PurchaseInvoiceDetails.Cancel, 
                  Sales_PurchaseInvoiceDetails.Serials, Sales_PurchaseInvoiceDetails.Caliber, Sales_PurchaseInvoiceDetails.Equivalen,    
                  Sales_PurchaseInvoiceDetails.pageNo, Sales_PurchaseInvoiceDetails.TheCount, Sales_PurchaseInvoiceDetails.Net,    
                  Sales_PurchaseInvoiceDetails.Total, Sales_PurchaseInvoiceDetails.Diamond_w, Sales_PurchaseInvoiceDetails.BAGET_W, 
                  Sales_PurchaseInvoiceDetails.STONE_W,    
                  Sales_PurchaseInvoiceDetails.Color, Sales_PurchaseInvoiceDetails.ThePurity, Stc_Items."+PrimaryName+@" AS ItemName, Stc_Items.GroupID, Stc_Items.TypeID, Stc_Items.ColorID, 
                  Stc_Items.BrandID, Stc_Items.BaseID, Sales_PurchaseInvoiceMaster.InvoiceDate, Sales_PurchaseInvoiceMaster.SupplierID, Sales_Suppliers."+PrimaryName+@" AS SupplierName
                  FROM   Sales_Suppliers RIGHT OUTER JOIN
                  Sales_PurchaseInvoiceMaster ON Sales_Suppliers.SupplierID = Sales_PurchaseInvoiceMaster.SupplierID RIGHT OUTER JOIN
                  Sales_PurchaseInvoiceDetails ON Sales_PurchaseInvoiceMaster.BranchID = Sales_PurchaseInvoiceDetails.BranchID AND Sales_PurchaseInvoiceMaster.InvoiceID = Sales_PurchaseInvoiceDetails.InvoiceID LEFT OUTER JOIN
                  Stc_Items ON Sales_PurchaseInvoiceDetails.ItemID = Stc_Items.ItemID
                  WHERE(Sales_PurchaseInvoiceDetails.InvoiceID <> -1) And Sales_PurchaseInvoiceDetails.BranchID =" + MySession.GlobalBranchID;

            Lip.NewFields();
            DataTable dt = new DataTable();
            cConnectionString.GetConnectionSetting();
            dt = Lip.SelectRecord(strSQL);
            EmportItemsFromDb(dt);


            Application.Exit();

        }

        private void EmportItemsFromDb(DataTable dtitems)
        {

            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "تأكيد الاسنيراد  ؟");
            if (!Yes)
                return;

            Application.DoEvents();

            if (dtitems.Rows.Count < 1)
                return;
            lstDetail = new BindingList<Sales_PurchaseInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            Sales_PurchaseInvoiceDetails obj = new Sales_PurchaseInvoiceDetails();


            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
            {
                obj = new Sales_PurchaseInvoiceDetails();
                obj.Serials = dtitems.Rows[i]["Serials"].ToString();
                obj.BarCode = dtitems.Rows[i]["BarCode"].ToString();
                obj.ArbItemName = dtitems.Rows[i]["ItemName"].ToString();
                obj.EngItemName = dtitems.Rows[i]["ItemName"].ToString();
                obj.GroupID = Comon.cDbl(dtitems.Rows[i]["GroupID"].ToString());
                obj.CostPrice = Comon.ConvertToDecimalPrice(dtitems.Rows[i]["CostPrice"].ToString());
                obj.QTY = Comon.ConvertToDecimalPrice(dtitems.Rows[i]["QTY"].ToString());
                obj.Caliber = 18;

                decimal CostPrice = Comon.ConvertToDecimalPrice(obj.CostPrice.ToString());
                decimal additonalVAlue = Comon.ConvertToDecimalPrice((CostPrice * MySession.GlobalPercentVat) / 100);
                //سعر تكلفة مع مصاريف
                decimal SpendPrice = Comon.ConvertToDecimalPrice(CostPrice);
                //سعر تكلفة المحل
                decimal CaratPrice = Comon.ConvertToDecimalPrice(SpendPrice * Comon.cDec(MySession.Cost));
                //سعر الكارت وهو البيع
                decimal SalePrice = Comon.ConvertToDecimalPrice(CaratPrice * Comon.cDec(MySession.sumvalue));

                obj.AdditionalValue = additonalVAlue;
                obj.SalePrice = SalePrice;
                obj.SpendPrice = SpendPrice;
                obj.Total = CostPrice;
                obj.Net = SpendPrice;
                obj.ArbGroupName = Lip.GetValue("Select  " + PrimaryName + " from Stc_ItemsGroups Where GroupID=" + obj.GroupID + " And BranchID =" + MySession.GlobalBranchID);
                obj.EngGroupName = obj.ArbGroupName;


                obj.ArbSizeName = obj.Caliber.ToString();
                obj.EngSizeName = obj.Caliber.ToString();


                lstDetail.Add(obj);

            }
            SumTotalBalanceAndDiscountread();

            gridControl.DataSource = lstDetail;
            SplashScreenManager.CloseForm(false);
        }
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtSupplierID.Text != string.Empty && txtSupplierID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtSupplierID.Text + " And BranchID =" + MySession.GlobalBranchID;
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

                        if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                        {
                            lblCreditAccountID.Text = txtSupplierID.Text;
                            lblCreditAccountName.Text = lblSupplierName.Text;

                        }
                    }
                    else
                    {
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtSupplierID.Text + " And BranchID =" + MySession.GlobalBranchID;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {
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

        private void btnemport_Click(object sender, EventArgs e)
        {
        label1: if (txtExcelPath.Text == string.Empty)
            {
                Messages.MsgError(Messages.TitleConfirm, "يجب تحديد مسار ملف الأكسل");
                txtExcelPath.Focus();
                simpleButton18_Click(null, null);
                goto label1;
            }

            EmportItems();
            txtExcelPath.Text = "";
            Messages.MsgInfo(Messages.TitleConfirm, "تم الاستيراد بنجاح - يجب حفظ عملية الاستيراد");

        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.Filter = "All Files|*.*";
                OpenFileDialog1.FileName = "";
                OpenFileDialog1.ShowDialog();
                if ((OpenFileDialog1.FileName != ""))
                {
                    txtExcelPath.Text = OpenFileDialog1.FileName;

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        private void btnprintBarcode_Click(object sender, EventArgs e)
        {
            frmPrintItemSticker frm = new frmPrintItemSticker();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                BindingSource bs = new BindingSource();
                bs.DataSource = gridControl.DataSource;
                frm.Show();
                frm.gridControl.DataSource = bs;

            }
            else
                frm.Dispose();
        }


        private void SaveImage(byte[] data, int ItemID)
        {
            try
            {

                SqlConnection Con = new GlobalConnection().Conn;
                if (Con.State == ConnectionState.Closed)
                    Con.Open();

                SqlCommand sc;
                sc = new SqlCommand("Update  Stc_Items Set ItemImage=@p Where ItemID=" + ItemID + " And BranchID =" + MySession.GlobalBranchID, Con);
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
            try
            {
                OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.Filter = "All Files|*.*|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg";
                OpenFileDialog1.FileName = "";
                OpenFileDialog1.ShowDialog();
                if ((OpenFileDialog1.FileName != ""))
                {
                    picItemImage.Image = Image.FromFile(OpenFileDialog1.FileName);
                    picItemImage.Visible = true;
                    byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                    SaveImage(Imagebyte, ItemIDImage);
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        private void chkForVatOjor_CheckedChanged(object sender, EventArgs e)
        {

            chForVat_EditValueChanged(null, null);
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


            strSQL = "Select * from " + Sales_PurchaseInvoicesDAL.TableName + " where cancel=0 And BranchID =" + MySession.GlobalBranchID+" and GoldUsing=" + GoldUsing;
            DataTable dtSend = new DataTable();
            dtSend = Lip.SelectRecord(strSQL);
            if (dtSend.Rows.Count > 0)
            {
                for (int i = 0; i <= dtSend.Rows.Count - 1; i++)
                {
                    txtInvoiceID.Text = dtSend.Rows[i]["InvoiceID"].ToString();
                    cmbBranchesID.EditValue = Comon.cInt(dtSend.Rows[i]["BranchID"].ToString());
                    txtCostCenterID.Text = dtSend.Rows[i]["CostCenterID"].ToString();
                    lblStoreID.Text = dtSend.Rows[i]["StoreID"].ToString();
                    txtInvoiceID_Validating(null, null);
                    IsNewRecord = true;
                    if (Comon.cInt(txtInvoiceID.Text) > 0)
                    {
                        //حفظ القيد الالي

                        long VoucherID = 0;
                        if (MySession.GlobalInventoryType == 2)// جرد دوري 
                            VoucherID = SaveVariousVoucherMachin(Comon.cInt(txtInvoiceID.Text));
                        else if (MySession.GlobalInventoryType == 1)//جرد مستمر 
                            VoucherID = SaveVariousVoucherMachinContinuousInv(Comon.cInt(txtInvoiceID.Text));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + Sales_PurchaseInvoicesDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sales_PurchaseInvoicesDAL.PremaryKey + " = " + txtInvoiceID.Text + " AND BranchID=" + Comon.cInt(cmbBranchesID.EditValue));

                    }



                }

                this.Close();
            }
        }

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " And BranchID =" + MySession.GlobalBranchID+" and Cancel=0"));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " And BranchID =" + MySession.GlobalBranchID+" and Cancel=0"));
                txtCurrncyPrice.Text = CurrncyPrice + "";
                lblCurrencyEqv.Visible = true;
                lblCurrncyPric.Visible = true;
                lblcurrncyEquvilant.Visible = true;
                txtCurrncyPrice.Visible = true;
                //gridView1.Columns["CurrencyEquivalent"].Visible = true;
            }
            else
            {
                txtCurrncyPrice.Text = "1";
                lblCurrencyEqv.Visible = false;
                lblCurrncyPric.Visible = false;
                lblcurrncyEquvilant.Visible = false;
                txtCurrncyPrice.Visible = false;
                //gridView1.Columns["CurrencyEquivalent"].Visible = false;
            }
        }

        private void simpleCash_Click(object sender, EventArgs e)
        {
            txtNetProcessID.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";

            /////////////////////////////////////////////////
            simpleAgel.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            // showCustomers(false,0);
            labelControl6.Visible = true;
            txtVatID.Visible = true;
            labelControl4.Visible = true;
            cmbMethodID.EditValue = 1;
            simpleCash.Appearance.BackColor = Color.Goldenrod;
            simpleCash.Appearance.BackColor2 = Color.White;
            simpleCash.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleCash.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "نقدا" : "Cash");
            MethodID = 1;
            simpleNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleCashNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
        }

        private void simpleNet_Click(object sender, EventArgs e)
        {

            txtNetProcessID.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            /////////////////////////////////////////////////
            simpleAgel.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //showCustomers(false,0);
            cmbMethodID.EditValue = 3;
            simpleNet.Appearance.BackColor = Color.Goldenrod;
            simpleNet.Appearance.BackColor2 = Color.White;
            simpleNet.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة" : "Net");
            MethodID = 2;
            simpleCash.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleCashNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
        }

        private void simpleCashNet_Click(object sender, EventArgs e)
        {
            /////////////////////////////        
            /////////////////////////////////////////////////
            // showCustomers(false,0);
            cmbMethodID.EditValue = 5;
            simpleCashNet.Appearance.BackColor = Color.Goldenrod;
            simpleCashNet.Appearance.BackColor2 = Color.White;
            simpleCashNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            simpleCashNet.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleCashNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة/ نقد" : "Net/Cash");
            MethodID = 3;
            simpleAgel.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleCash.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
        }

        private void simpleAgel_Click(object sender, EventArgs e)
        {
            txtNetProcessID.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            /////////////////////////////////////////////////
            showCustomers(true, 1);
            cmbMethodID.EditValue = 2;
            simpleAgel.Appearance.BackColor = Color.Goldenrod;
            simpleAgel.Appearance.BackColor2 = Color.White;
            simpleAgel.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            simpleAgel.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleAgel.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "آجل" : "Future");
            MethodID = 4;
            txtSupplierID.Visible = true;
            lblSupplierName.Visible = true;
            txtSupplierID.Focus();
            //Find();
            simpleCash.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleCashNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        }

        private void frmCashierPurchaseMatirial_Load(object sender, EventArgs e)
        {
            DoNew();

        }

        private void frmCashierPurchaseMatirial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();
            if (e.KeyCode == Keys.F9)
            {
                falgPrint = true;
                DoSave();
            }
            if (e.KeyCode == Keys.F6)
            {
                DoSave();
            }
            else if (e.KeyCode == Keys.F6)
                simpleCash_Click(null, null);
            else if (e.KeyCode == Keys.F7)
                simpleNet_Click(null, null);
            else if (e.KeyCode == Keys.F8)
                simpleCashNet_Click(null, null);

        }
        
        private void btnDividedSpendTransport_Click(object sender, EventArgs e)
        {
            //if (CountTrans == 0)
            {
                //CountTrans++;
                decimal Qty = 0;
                decimal Gold_W = 0;
                decimal CostPriceRow = 0;
                decimal PersentCostval = 0;
                decimal val = 0;
                decimal AmountTransport = Comon.ConvertToDecimalPrice(txtTransportAmount.Text);
                if (rdEqual.Checked)
                {
                    val = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtTransportAmount.Text) / gridView1.DataRowCount);
                    for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                    {
                        CostPriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                        decimal v = Comon.ConvertToDecimalPrice(CostPriceRow + val);
                        //gridView1.SetRowCellValue(i, "CostPrice", v);
                        gridView1.SetRowCellValue(i,"Bones", val);

                         UpdateCostPrice(i, v);
                    }
                }
                if (RdPyQty.Checked)
                {
                    if (Comon.ConvertToDecimalPrice(txtTransportAmount.Text) > 0)
                    {
                        for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                        {
                            Gold_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                            CostPriceRow += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                        }


                        PersentCostval = 0;

                        for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                        {
                            decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                            decimal Qtyrow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                            PersentCostval = Comon.ConvertToDecimalPrice(Qtyrow / Gold_W);
                            decimal newval = Comon.ConvertToDecimalPrice(AmountTransport * PersentCostval);
                            gridView1.SetRowCellValue(i, "Bones", newval);
                            UpdateCostPrice(i, newval);
                            //gridView1.SetRowCellValue(i, "CostPrice", Comon.ConvertToDecimalPrice(CostPrice + newval));
                        }
                    }
                }
                if (RdPyValue.Checked)
                {
                    if (Comon.ConvertToDecimalPrice(txtTransportAmount.Text) > 0)
                    {
                        for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                        {
                            Gold_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                            CostPriceRow += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                        }
                        PersentCostval = 0;
                        for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                        {
                            decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                            PersentCostval = Comon.ConvertToDecimalPrice(CostPrice / CostPriceRow);
                            decimal newval = Comon.ConvertToDecimalPrice(AmountTransport * PersentCostval);
                            gridView1.SetRowCellValue(i, "Bones", newval);
                            UpdateCostPrice(i, newval);
                            //gridView1.SetRowCellValue(i, "CostPrice", Comon.ConvertToDecimalPrice(CostPrice + newval));
                        }
                    }

                }
                txtTransportAmount.Enabled = false;

            }
        }


        private void UpdateCostPrice(int row, decimal value)
        {
            if (row >= 0)
            {
                bool HasVat = Comon.cbool(gridView1.GetRowCellValue(row, gridView1.Columns["HavVat"]).ToString());
                decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(row, gridView1.Columns["QTY"]).ToString());
                decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(row,"CostPrice"));
                decimal TotalCost = Comon.ConvertToDecimalPrice(QTY * CostPrice);
                decimal Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(row, "Discount"));
                gridView1.SetRowCellValue(row, "Total", Comon.ConvertToDecimalPrice(TotalCost) - Comon.ConvertToDecimalPrice(Discount));
                decimal Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(row, "Total"));
                decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                if (HasVat == true)
                    additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                else
                    additonalVAlue = 0;
                gridView1.SetRowCellValue(row, "AdditionalValue", additonalVAlue.ToString());
                gridView1.SetRowCellValue(row, "SpendPrice",0);

                decimal ExpensesAmount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(row, "Bones"));
                decimal Net = Comon.ConvertToDecimalPrice(Total + ExpensesAmount + additonalVAlue);


                gridView1.SetRowCellValue(row,"Net", Net.ToString());
                if (Comon.cDec(txtCurrncyPrice.Text) > 0)
                    gridView1.SetRowCellValue(row, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(Net) * Comon.cDec(txtCurrncyPrice.Text)).ToString());
                CalculateRow(row);
            }
        }
        private void gridViewTransport_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            double num;
            GridView view = sender as GridView;
            view.ClearColumnErrors();
            HasColumnErrors = false;
            string ColName = view.FocusedColumn.FieldName;

            if (ColName == "ArbAccountName" || ColName == "DebitAmount" || ColName == "CurrencyPrice")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;
                }

                /****************************************/
                if (ColName == "ArbAccountName")
                {
                    DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                     
                    DataRow[] row = dt.Select(""+PrimaryName+"='" + e.Value.ToString() + "'");
                    if (row.Length == 0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisAccountID;
                    }
                    else
                    {
                      
                        FileItemDataAccount(row[0]);
                    }
                }

                if (ColName == "DebitAmount")
                {
                    CalculatTotalTransPort();
                }

            }

        }
        private void FileItemDataAccount(DataRow dr)
        {
            if (Lip.CheckTheAccountIsStope(Comon.cDbl(dr["AccountID"]), Comon.cInt(cmbBranchesID.EditValue)))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                gridViewTransport.DeleteRow(gridViewTransport.FocusedRowHandle);
                return;
            }
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["SpendVoucherID"],Comon.cInt(txtInvoiceID.Text));
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["BranchID"], Comon.cInt(cmbBranchesID.EditValue));
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["FacilityID"], UserInfo.FacilityID);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["Discount"], 0);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["CurrencyID"], 0);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["CurrencyPrice"], 0);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["CurrencyName"], "");
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["CurrencyEquivalent"],0);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["Barcode"], "");
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["ItemName"], "");
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["WeightGold"],0);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["QtyGoldEqulivent"], 0);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["Calipar"], 0);
             

            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["AccountID"], dr["AccountID"]);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["ArbAccountName"], dr[PrimaryName]);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["CurrencyName"], cmbCurency.Text.ToString());
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["CurrencyEquivalent"], 0);
            gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
            if (UserInfo.Language == iLanguage.English)
                gridViewTransport.SetRowCellValue(gridViewTransport.FocusedRowHandle, gridViewTransport.Columns["EngAccountName"], dr["EngName"].ToString());
        }
        private void gridViewTransport_ValidatingEditor_1(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            double num;
            GridView view = sender as GridView;
            view.ClearColumnErrors();
            HasColumnErrors = false;
            string ColName = view.FocusedColumn.FieldName;

            if (ColName == "ArbAccountName" || ColName == "DebitAmount" || ColName == "CurrencyPrice")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;
                }

                /****************************************/
                if (ColName == "ArbAccountName")
                {
                    DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                    
                    DataRow[] row = dt.Select(""+PrimaryName+"='" + e.Value.ToString() + "'");
                    if (row.Length == 0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisAccountID;
                    }
                    else
                    {
                        
                        FileItemDataAccount(row[0]);
                    }
                }

                if (ColName == "DebitAmount")
                {
                    CalculatTotalTransPort();
                }

            }
        }

        private void gridControl_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmCashierPurchaseOrder frm = new frmCashierPurchaseOrder();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void chkForVat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForVat.Checked == false)
            {
                lblAdditionalAccountID.Tag = "";
                lblAdditionalAccountName.Enabled = false;
                lblAdditionalAccountID.Enabled = false;
                labelControl18.Enabled = false;
            }
            else
            {
                lblAdditionalAccountID.Tag = "ImportantFieldGreaterThanZero";
                lblAdditionalAccountName.Enabled = true;
                lblAdditionalAccountID.Enabled = true;
                labelControl18.Enabled = true;
            }
            CalculateRow(IsHavVat: chkForVat.Checked);
            
        }

        private void btnReciveOrder_Click(object sender, EventArgs e)
        {
            frmGoldInOnBail frm = new frmGoldInOnBail();
            if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();

            }
            else
                frm.Dispose();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            frmMatirialInOnBail frm = new frmMatirialInOnBail();
            if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();

            }
            else
                frm.Dispose();

        }

        private void tabFroms_Paint(object sender, PaintEventArgs e)
        {

        }

         
       

      
    }
}
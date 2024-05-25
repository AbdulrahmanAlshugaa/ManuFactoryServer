using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.DAL.SalseSystem;
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.AccountsObjects.Codes;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.StockObjects.Codes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Edex.StockObjects.StoresClasses;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.Accounting;
using System.Data.OleDb;

namespace Edex.SalesAndSaleObjects.Transactions
{
    public partial class frmCashierPurchaseGold : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        public bool PostToServer = false;
        CompanyHeader cmpheader = new CompanyHeader();
        public int DiscountCustomer = 0;
        #region Declare
        public const int DocumentType = 10;
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
        private string GroupName;
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

        public int GoldUsing = 1;

        int StoreItemID = 1;
        //all record master and detail
        BindingList<Sales_SalesInvoiceDetails> AllRecords = new BindingList<Sales_SalesInvoiceDetails>();

        //list detail
        BindingList<Sales_SalesInvoiceDetails> lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
        //list detail
        BindingList<Sales_SalesInvoiceDetails> lstDetail2 = new BindingList<Sales_SalesInvoiceDetails>();

        //Detail
        Sales_SalesInvoiceDetails BoDetail = new Sales_SalesInvoiceDetails();
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

        public frmCashierPurchaseGold()
        {
            try
            {
               
                FormAdd = true;
                FormView = true;
                ReportView=true ;
                ReportExport=true ;
                ShowReportInReportViewer=false ;

                
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                InitializeComponent();
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
                InitGrid2();
                /*********************** Fill Data ComboBox  ****************************/
                FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
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
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseStoreID;
                txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseCostCenterID;
                cmbMethodID.ReadOnly = !MySession.GlobalAllowChangefrmPurchasePayMethodID;
                cmbNetType.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseNetTypeID;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseCurencyID;
                
                txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseDelegateID;
                txtSellerID.ReadOnly = false;
                /************TextEdit Account ID ***************/
                lblDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseDebitAccountID;
                lblCreditAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseCreditAccountID;
                lblAdditionalAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseAdditionalAccountID;
                lblChequeAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseChequeAccountID;
                lblDiscountDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID;
                lblNetAccountID.ReadOnly = !MySession.GlobalAllowChangefrmPurchaseNetAccountID;
                /************ Button Search Account ID ***************/
                RolesButtonSearchAccountID();
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

                this.gridControl1.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl1_ProcessGridKey);
                this.gridView2.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView2.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanging);
                this.gridView2.ShownEditor += new System.EventHandler(this.gridView2_ShownEditor);
                this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
                this.gridView2.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView2.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView2_ValidateRow);
                this.gridView2.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                this.gridView2.PopupMenuShowing += gridView1_PopupMenuShowing;



                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);


                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = UserInfo.BRANCHID;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
               // ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
               // ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
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
        #region GridView
        void InitGrid()
        {
            lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["PackingQty"].Visible = false;
            gridView1.Columns["BAGET_W"].Visible = false;
            gridView1.Columns["STONE_W"].Visible = true;
            gridView1.Columns["DIAMOND_W"].Visible = false;
            gridView1.Columns["Equivalen"].Visible = false;
            gridView1.Columns["Caliber"].Visible = false;
            gridView1.Columns["SalePrice"].Visible = true;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["Bones"].Visible = false;
            gridView1.Columns["Height"].Visible = false;
            gridView1.Columns["Width"].Visible = false;
            gridView1.Columns["TheCount"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;
         
            gridView1.Columns["Color"].Visible = false;
            gridView1.Columns["CLARITY"].Visible = false;
            gridView1.Columns["GroupID"].Visible = false;
            gridView1.Columns["ArbGroupName"].Visible = false;
            gridView1.Columns["EngGroupName"].Visible = false;

            gridView1.Columns["SpendPrice"].Visible = false;
            gridView1.Columns["CaratPrice"].Visible = false;

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
            gridView1.Columns["BarCode"].Visible = true;
            gridView1.Columns["ExpiryDate"].Visible = false ;
            gridView1.Columns["Description"].Visible = true;

            gridView1.Columns["DateFirst"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;
            gridView1.Columns["DateFirstStr"].Visible = false;

            /******************* Columns Visible=true *******************/
            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;
            gridView1.Columns["SizeID"].Visible = false;
            gridView1.Columns["Discount"].Visible = false;
            gridView1.Columns["HavVat"].Visible = false;
            gridView1.Columns["RemainQty"].Visible = false;
            gridView1.Columns["ItemID"].Visible = false;

            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 200;
            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["QTY"].Caption = CaptionQTY;
            gridView1.Columns["STONE_W"].Caption = "الحجر";

            gridView1.Columns["Total"].Caption = CaptionTotal;
            gridView1.Columns["Discount"].Caption = CaptionDiscount;
            gridView1.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView1.Columns["Net"].Caption = CaptionNet;
            gridView1.Columns["CostPrice"].Caption = "الأجور";
            gridView1.Columns["SalePrice"].Caption = CaptionSalePrice;

            gridView1.Columns["Description"].Caption = CaptionDescription;
            gridView1.Columns["HavVat"].Caption = CaptionHavVat;
            gridView1.Columns["RemainQty"].Caption = CaptionRemainQty;
            gridView1.Focus();

            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["CurrencyPrice"].Visible = false;
            gridView1.Columns["CurrencyName"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;
        
            DataTable dtCurrncy = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Acc_Currency where Cancel=0 ");
            string[] CurrncyName = new string[dtCurrncy.Rows.Count];
            for (int i = 0; i <= dtCurrncy.Rows.Count - 1; i++)
                CurrncyName[i] = dtCurrncy.Rows[i]["ArbName"].ToString();
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
                gridView1.Columns["Calipar"].Caption = "Calipar";
                gridView1.Columns["CurrencyPrice"].Caption = "Currency Price  ";
                gridView1.Columns["CurrencyID"].Caption = "Currency ID  ";
                gridView1.Columns["CurrencyName"].Caption = "Currency Name";
                gridView1.Columns["CurrencyEquivalent"].Caption = "Currency Equivalent";
            }

            /*************************Columns Properties ****************************/
          //  gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
            gridView1.Columns["Total"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["Total"].OptionsColumn.AllowFocus = false;
           // gridView1.Columns["Net"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
           // gridView1.Columns["Net"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
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


            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
            gridView1.Columns[ItemName].ColumnEdit = rItem;
            gridControl.RepositoryItems.Add(rItem);

            /////////////////////////Item
            ///
           
            DataTable dtitems = Lip.SelectRecord("SELECT distinct ItemName AS ArbName FROM Sales_BarCodeForPurchaseInvoiceEng_Find");
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            gridControl.RepositoryItems.Add(riComboBoxitems);
            gridView1.Columns[ItemName].ColumnEdit = riComboBoxitems;
            ///////////////////////////

            /////////////////////////Description
            DataTable dt = Lip.SelectRecord("SELECT ArbName FROM Stc_ItemsGroups WHERE Cancel=0");
            string[] companies = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companies[i] = dt.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBox = new RepositoryItemComboBox();
            riComboBox.Items.AddRange(companies);
            gridControl.RepositoryItems.Add(riComboBox);
            gridView1.Columns["Description"].ColumnEdit = riComboBox;
            ///////////////////////////



            gridView1.Columns["Description"].Width = 150;
            gridView1.Columns[ItemName].Width = 150;

            gridView1.Columns["CurrencyEquivalent"].VisibleIndex = gridView1.Columns["SalePrice"].VisibleIndex + 1;

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

        void InitGrid2()
        {
            lstDetail2 = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail2.AllowNew = true;
            lstDetail2.AllowEdit = true;
            lstDetail2.AllowRemove = true;
            gridControl1.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/
            gridView2.Columns["BranchID"].Visible = false;
            gridView2.Columns["PackingQty"].Visible = false;
            gridView2.Columns["BAGET_W"].Visible = false;
            gridView2.Columns["STONE_W"].Visible = true;
            gridView2.Columns["DIAMOND_W"].Visible = false;
            gridView2.Columns["Equivalen"].Visible = false;
            gridView2.Columns["Caliber"].Visible = false;
            gridView2.Columns["SalePrice"].Visible = true;
            gridView2.Columns["ExpiryDateStr"].Visible = false;
           
            gridView2.Columns["Bones"].Visible = false;
            gridView2.Columns["Height"].Visible = false;
            gridView2.Columns["Width"].Visible = false;
            gridView2.Columns["TheCount"].Visible = false;
            gridView2.Columns["ItemImage"].Visible = false;
            gridView2.Columns["Color"].Visible = false;
            gridView2.Columns["CLARITY"].Visible = false;
            gridView2.Columns["GroupID"].Visible = false;
            gridView2.Columns["ArbGroupName"].Visible = false;
            gridView2.Columns["EngGroupName"].Visible = false;

            gridView2.Columns["SpendPrice"].Visible = false;
            gridView2.Columns["CaratPrice"].Visible = false;

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
            gridView2.Columns["BarCode"].Visible = true;
            gridView2.Columns["ExpiryDate"].Visible = false;
            gridView2.Columns["Description"].Visible = true;

            gridView2.Columns[GroupName].Visible = true;

            gridView2.Columns["ArbGroupName"].Visible = gridView2.Columns["ArbGroupName"].Name == "col" + GroupName ? true : false;
            gridView2.Columns["EngGroupName"].Visible = gridView2.Columns["EngGroupName"].Name == "col" + GroupName ? true : false;
            gridView2.Columns["GroupID"].Caption = "رقم المجموعة";
            gridView2.Columns[GroupName].Caption = "اسم المجموعة";
            gridView2.Columns["DateFirst"].Visible = false;
            gridView2.Columns["ExpiryDateStr"].Visible = false;
            gridView2.Columns["ItemImage"].Visible = false;
            gridView2.Columns["DateFirstStr"].Visible = false;

            /******************* Columns Visible=true *******************/
            gridView2.Columns[ItemName].Visible = true;
            gridView2.Columns[SizeName].Visible = true;
            gridView2.Columns["SizeID"].Visible = false;
            gridView2.Columns["Discount"].Visible = false;
            gridView2.Columns["HavVat"].Visible = false;
            gridView2.Columns["RemainQty"].Visible = false;
            gridView2.Columns["ItemID"].Visible = false;

            gridView2.Columns["BarCode"].Caption = CaptionBarCode;
            gridView2.Columns["ItemID"].Caption = CaptionItemID;
            gridView2.Columns[ItemName].Caption = CaptionItemName;
            gridView2.Columns[ItemName].Width = 200;
            gridView2.Columns["SizeID"].Caption = CaptionSizeID;
            gridView2.Columns[SizeName].Caption = CaptionSizeName;
            gridView2.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView2.Columns["QTY"].Caption = CaptionQTY;
            gridView2.Columns["STONE_W"].Caption = "الحجر";

            gridView2.Columns["Total"].Caption = CaptionTotal;
            gridView2.Columns["Discount"].Caption = CaptionDiscount;
            gridView2.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView2.Columns["Net"].Caption = CaptionNet;
            gridView2.Columns["CostPrice"].Caption = "سعر التكلفة";
            gridView2.Columns["SalePrice"].Caption = "سعر الكرت";

            gridView2.Columns["Description"].Caption = CaptionDescription;
            gridView2.Columns["HavVat"].Caption = CaptionHavVat;
            gridView2.Columns["RemainQty"].Caption = CaptionRemainQty;
            gridView2.Focus();
            /*************************Columns Properties ****************************/
            //  gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
            gridView2.Columns["Total"].OptionsColumn.ReadOnly = true;
            gridView2.Columns["Total"].OptionsColumn.AllowFocus = false;
            gridView2.Columns["Total"].Visible = false;
            // gridView1.Columns["Net"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            // gridView1.Columns["Net"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmSaleInvoiceNetPrice;
            gridView2.Columns["AdditionalValue"].OptionsColumn.ReadOnly = true;
            gridView2.Columns["AdditionalValue"].OptionsColumn.AllowFocus = false;
            /************************ Date Time **************************/

            RepositoryItemDateEdit RepositoryDateEdit = new RepositoryItemDateEdit();
            RepositoryDateEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            RepositoryDateEdit.Mask.EditMask = "dd/MM/yyyy";
            RepositoryDateEdit.Mask.UseMaskAsDisplayFormat = true;
            gridControl1.RepositoryItems.Add(RepositoryDateEdit);
            gridView2.Columns["ExpiryDate"].ColumnEdit = RepositoryDateEdit;
            gridView2.Columns["ExpiryDate"].UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            gridView2.Columns["ExpiryDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView2.Columns["ExpiryDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridView2.Columns["ExpiryDate"].OptionsColumn.AllowEdit = true;
            gridView2.Columns["ExpiryDate"].OptionsColumn.ReadOnly = false;


            gridView2.Columns["CurrencyID"].Visible = false;
            gridView2.Columns["CurrencyEquivalent"].Visible = false;
            gridView2.Columns["CurrencyPrice"].Visible = false;
            gridView2.Columns["CurrencyName"].Visible = false;
            gridView2.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridView2.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;
            DataTable dtCurrncy = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Acc_Currency where Cancel=0 ");
            string[] CurrncyName = new string[dtCurrncy.Rows.Count];
            for (int i = 0; i <= dtCurrncy.Rows.Count - 1; i++)
                CurrncyName[i] = dtCurrncy.Rows[i]["ArbName"].ToString();
            RepositoryItemComboBox riComboBoxitems1 = new RepositoryItemComboBox();
            riComboBoxitems1.Items.AddRange(CurrncyName);
            gridControl1.RepositoryItems.Add(riComboBoxitems1);
            gridView2.Columns["CurrencyName"].ColumnEdit = riComboBoxitems1;
            gridView2.Columns["CurrencyPrice"].Caption = "سعر العملة";
            gridView2.Columns["CurrencyID"].Caption = "رقم العملة";
            gridView2.Columns["CurrencyName"].Caption = "اسم العملة";
            gridView2.Columns["CurrencyEquivalent"].Caption = "المقابل";
            if (UserInfo.Language == iLanguage.English)
            {
                gridView2.Columns["Calipar"].Caption = "Calipar";
                gridView2.Columns["CurrencyPrice"].Caption = "Currency Price  ";
                gridView2.Columns["CurrencyID"].Caption = "Currency ID  ";
                gridView2.Columns["CurrencyName"].Caption = "Currency Name";
                gridView2.Columns["CurrencyEquivalent"].Caption = "Currency Equivalent";
            }
            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
            gridView2.Columns[ItemName].ColumnEdit = rItem;
            gridControl1.RepositoryItems.Add(rItem);

            /////////////////////////Item
            ///

            DataTable dtitems = Lip.SelectRecord("SELECT distinct ItemName AS ArbName FROM Sales_BarCodeForPurchaseInvoiceEng_Find");
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            gridControl1.RepositoryItems.Add(riComboBoxitems);
            gridView2.Columns[ItemName].ColumnEdit = riComboBoxitems;
            ///////////////////////////
            
            /////////////////////////Description
            DataTable dt = Lip.SelectRecord("SELECT ArbName FROM Stc_ItemsGroups WHERE Cancel=0");
            string[] companies = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companies[i] = dt.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBox = new RepositoryItemComboBox();
            riComboBox.Items.AddRange(companies);
            gridControl1.RepositoryItems.Add(riComboBox);
            gridView2.Columns["Description"].ColumnEdit = riComboBox;
            ///////////////////////////



            gridView2.Columns["Description"].Width = 150;
            gridView2.Columns[ItemName].Width = 150;
            string[] companiesGroupitems = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companiesGroupitems[i] = dt.Rows[i]["ArbName"].ToString();

            RepositoryItemComboBox riComboBoxGroupitems = new RepositoryItemComboBox();
            riComboBoxGroupitems.Items.AddRange(companiesGroupitems);
            gridControl1.RepositoryItems.Add(riComboBoxGroupitems);
            gridView2.Columns[GroupName].ColumnEdit = riComboBoxGroupitems;
            gridView2.Columns[GroupName].Width = 120;
            gridView2.Columns[GroupName].VisibleIndex = 1;
            gridView2.Columns["SalePrice"].VisibleIndex = gridView2.Columns["Net"].VisibleIndex+1;

            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView2.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView2.BestFitColumns();
            gridView2.Columns["CurrencyEquivalent"].VisibleIndex = gridView2.Columns["SalePrice"].VisibleIndex + 1;
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
        private void gridView2_ShownEditor(object sender, EventArgs e)
        {
            if (this.gridView2.ActiveEditor is CheckEdit)
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

        private void gridView2_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (!gridView2.IsLastVisibleRow)
                    gridView2.MoveLast();
                if (DiscountCustomer != 0)
                {
                    txtDiscountPercent.Text = DiscountCustomer.ToString();
                    txtDiscountPercent_Validating(null, null);
                }
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
                        else if (!(double.TryParse(val.ToString(), out num)) && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView2.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && col.FieldName != "BarCode")
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
                if (ColName == "BarCode" || ColName == "SalePrice" || ColName == "Net" || ColName == "SizeID" || ColName == "Total" || ColName == "ItemID" || ColName == "QTY" || ColName == "CostPrice" || ColName == "CurrencyPrice")
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
      
                    /****************************************/
                    if (ColName == "CostPrice")
                    {
                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                        decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("QTY"));
                        decimal SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("SalePrice"));
                        decimal CostPrice = Comon.ConvertToDecimalPrice(val.ToString());
                       
                        decimal TotalCost = QTY * CostPrice;
                        decimal TotalSale= QTY * SalePrice;

                        decimal Total = Comon.ConvertToDecimalPrice(TotalCost + TotalSale);

                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);

                        if (HasVat == true)
                            if (chkForVatCostOnly.Checked == true)
                                additonalVAlue = Comon.ConvertToDecimalPrice((TotalCost * MySession.GlobalPercentVat) / 100);
                            else
                                additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;

                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("Total", Comon.ConvertToDecimalPrice(TotalCost + TotalSale).ToString());
                        decimal Net = Comon.ConvertToDecimalPrice(TotalCost + TotalSale + additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                        gridView1.SetFocusedRowCellValue("Width", TotalSale.ToString());
                        gridView1.SetFocusedRowCellValue("Height", TotalCost.ToString());

                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(Net) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice"))).ToString());


                    }
                    if (ColName == "CurrencyPrice")
                    {
                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net"))).ToString());

                    }
                    if (ColName == "SalePrice")
                    {
                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                        decimal QTY = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("QTY"));
                        decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("CostPrice"));
                        decimal SalePrice = Comon.ConvertToDecimalPrice(val.ToString());

                        decimal TotalCost = Comon.ConvertToDecimalPrice(QTY * CostPrice);
                        decimal TotalSale = Comon.ConvertToDecimalPrice(QTY * SalePrice);

                        decimal Total = Comon.ConvertToDecimalPrice(TotalSale + TotalCost);
                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        if (HasVat == true)
                            if (chkForVatCostOnly.Checked == true)
                                additonalVAlue = Comon.ConvertToDecimalPrice((TotalCost * MySession.GlobalPercentVat) / 100);
                            else
                                additonalVAlue = Comon.ConvertToDecimalPrice(((TotalSale + CostPrice) * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;

                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("Total", Comon.ConvertToDecimalPrice(Total).ToString());

                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                        gridView1.SetFocusedRowCellValue("Height", Comon.ConvertToDecimalPrice(TotalCost).ToString());
                        gridView1.SetFocusedRowCellValue("Width", Comon.ConvertToDecimalPrice(TotalSale).ToString());

                    }
                    if (ColName == "StoreID")
                    {
                        string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                        int ExpiryDate = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"]).ToString());
                        double Qty = Comon.cDbl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"]).ToString());
                        double RemindQty = 0;

                        RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + BarCode + "'," + Comon.cInt(txtStoreID.Text) + "," + Comon.cInt(cmbBranchesID.EditValue) + ") AS RemainQty").Rows[0]["RemainQty"].ToString());

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
                        decimal additonalVAlue =0;
                        
                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());

                        decimal QTY = Comon.ConvertToDecimalPrice(val.ToString());
                        decimal CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("CostPrice"));
                        decimal SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("SalePrice"));

                        decimal TotalCost = Comon.ConvertToDecimalPrice( QTY * CostPrice);
                        decimal TotalSale = Comon.ConvertToDecimalPrice(QTY * SalePrice);
                        decimal Total = Comon.ConvertToDecimalPrice(TotalSale + TotalCost);


                        if (HasVat == true)
                            if (chkForVatCostOnly.Checked == true)
                                additonalVAlue = Comon.ConvertToDecimalPrice((TotalCost * MySession.GlobalPercentVat) / 100);
                            else
                                additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;


                        gridView1.SetFocusedRowCellValue("Width", Comon.ConvertToDecimalPrice(TotalSale).ToString());
                        gridView1.SetFocusedRowCellValue("Hight", Comon.ConvertToDecimalPrice(TotalCost).ToString());


                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("CostPrice", PriceUnit.ToString());
                        gridView1.SetFocusedRowCellValue("Total", (Total).ToString());

                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);


                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());

                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(Net) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice"))).ToString());
                    }
                    if (ColName == "BarCode")
                    {

                        DataTable dt;
                        var flagb = false;
                         
                        dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                           // e.Valid = false;
                           // HasColumnErrors = true;
                           // e.ErrorText = Messages.msgNoFoundThisBarCode;
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
                    if (ColName == "Net")
                    {
                        decimal additonalVAlue = 0;
                        if (Comon.ConvertToDecimalPrice(val.ToString()) > 0)
                        {
                            string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                            decimal Qty = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("QTY"));
                            bool HasVat = chkForVat.Checked;
                            decimal Net = (Comon.ConvertToDecimalPriceTree(val.ToString()) + Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("Discount")));
                            additonalVAlue = Comon.ConvertToDecimalPrice(Net - ((Net * 100) / (100 + MySession.GlobalPercentVat)));
                            if (BarCode == "24")
                                additonalVAlue = 0;
                            decimal CostPrice = Comon.ConvertToDecimalPrice((Net - additonalVAlue) / (Comon.ConvertToDecimalQty(gridView1.GetFocusedRowCellValue("QTY"))));
                            decimal Total = Comon.ConvertToDecimalPrice(Net) - additonalVAlue;
                            gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                            gridView1.SetFocusedRowCellValue("CostPrice", CostPrice.ToString());
                            gridView1.SetFocusedRowCellValue("Total", Total.ToString());
                            gridView1.SetFocusedRowCellValue("Net", val.ToString());
                        }
                    }
                }
                if (ColName == "CurrencyName")
                {
                    DataTable dt = Lip.SelectRecord("Select ID ,ExchangeRate from Acc_Currency Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "')");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyID", dt.Rows[0]["ID"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice", dt.Rows[0]["ExchangeRate"]);
                    if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net")) > 0)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net"))).ToString());
                }
                else if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select ItemID from Stc_Items Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"], cmbCurency.Text.ToString());
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyEquivalent"], 0);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
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

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
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
                if (ColName == "BarCode" || ColName == "SalePrice" || ColName == "Net" || ColName == "SizeID" || ColName == "Total" || ColName == "ItemID" || ColName == "QTY" || ColName == "CostPrice" || ColName == "CurrencyPrice")
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


                    /****************************************/
                    if (ColName == "CostPrice")
                    {
                        bool HasVat = Comon.cbool(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"]).ToString());
                        decimal QTY = Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("QTY"));
                        decimal SalePrice = Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("SalePrice"));
                        decimal CostPrice = Comon.ConvertToDecimalPrice(val.ToString());

                        decimal TotalCost =  CostPrice;
                        decimal TotalSale =   SalePrice;

                        decimal Total = Comon.ConvertToDecimalPrice(TotalCost );

                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);

                        if (HasVat == true)
                        {
                            if (chkForVatCostOnly.Checked == true)
                                additonalVAlue = Comon.ConvertToDecimalPrice((TotalCost * MySession.GlobalPercentVat) / 100);
                        }

                        else
                            additonalVAlue = 0;

                        gridView2.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView2.SetFocusedRowCellValue("Total", Comon.ConvertToDecimalPrice(TotalCost + TotalSale).ToString());
                        decimal Net = Comon.ConvertToDecimalPrice(TotalCost  + additonalVAlue);
                        gridView2.SetFocusedRowCellValue("Net", Net.ToString());
                        gridView2.SetFocusedRowCellValue("CaratPrice", TotalSale.ToString());
                        gridView2.SetFocusedRowCellValue("Width", TotalSale.ToString());
                        gridView2.SetFocusedRowCellValue("Height", TotalCost.ToString());

                        if (Comon.cDec(gridView2.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) > 0)
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(Net) * Comon.cDec(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "CurrencyPrice"))).ToString());

                    }
                    if (ColName == "SalePrice")
                    {
                        bool HasVat = Comon.cbool(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"]).ToString());
                        decimal QTY = Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("QTY"));
                        decimal CostPrice = Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("CostPrice"));
                        decimal SalePrice = Comon.ConvertToDecimalPrice(val.ToString());

                        decimal TotalCost = Comon.ConvertToDecimalPrice(CostPrice);
                        decimal TotalSale = Comon.ConvertToDecimalPrice( SalePrice);

                        decimal Total = Comon.ConvertToDecimalPrice( TotalCost);
                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        if (HasVat == true)
                        {
                            if (chkForVatCostOnly.Checked == true)
                                additonalVAlue = Comon.ConvertToDecimalPrice((TotalCost * MySession.GlobalPercentVat) / 100);
                        }
                        else
                            additonalVAlue = 0;

                        gridView2.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView2.SetFocusedRowCellValue("Total", Comon.ConvertToDecimalPrice(Total).ToString());

                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);
                        gridView2.SetFocusedRowCellValue("Net", Net.ToString());
                        gridView2.SetFocusedRowCellValue("CaratPrice", TotalSale.ToString());
                        gridView2.SetFocusedRowCellValue("Height", Comon.ConvertToDecimalPrice(TotalCost).ToString());
                        gridView2.SetFocusedRowCellValue("Width", Comon.ConvertToDecimalPrice(TotalSale).ToString());

                    }
                    if (ColName == "StoreID")
                    {
                        string BarCode = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"]).ToString();
                        int ExpiryDate = Comon.ConvertDateToSerial(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ExpiryDate"]).ToString());
                        double Qty = Comon.cDbl(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"]).ToString());
                        double RemindQty = 0;

                        RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + BarCode + "'," + Comon.cInt(txtStoreID.Text) + "," + Comon.cInt(cmbBranchesID.EditValue) + ") AS RemainQty").Rows[0]["RemainQty"].ToString());

                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["RemainQty"], RemindQty);
                        if (RemindQty - Qty < 0)
                        {
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                HasColumnErrors = true;
                                e.Valid = false;
                                gridView2.SetColumnError(gridView2.Columns["QTY"], "");
                                e.ErrorText = "لايوجد كمية متبقية في المستودع لهذا الصنف المحدد بتاريخ الصلاحية ";
                            }
                        }
                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        gridView2.SetColumnError(gridView2.Columns["QTY"], "");
                        e.ErrorText = "";
                        decimal PriceUnit = Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("CostPrice"));
                        decimal additonalVAlue = 0;

                        bool HasVat = Comon.cbool(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["HavVat"]).ToString());

                        decimal QTY = Comon.ConvertToDecimalPrice(val.ToString());
                        decimal CostPrice = Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("CostPrice"));
                        decimal SalePrice = Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("SalePrice"));

                        decimal TotalCost = Comon.ConvertToDecimalPrice(CostPrice);
                        decimal TotalSale = Comon.ConvertToDecimalPrice(SalePrice);
                        decimal Total = Comon.ConvertToDecimalPrice( TotalCost);


                        if (HasVat == true)
                        {
                            if (chkForVatCostOnly.Checked == true)
                                additonalVAlue = Comon.ConvertToDecimalPrice((TotalCost * MySession.GlobalPercentVat) / 100);
                        }

                        else
                            additonalVAlue = 0;


                        gridView2.SetFocusedRowCellValue("Width", Comon.ConvertToDecimalPrice(TotalSale).ToString());
                        gridView2.SetFocusedRowCellValue("Hight", Comon.ConvertToDecimalPrice(TotalCost).ToString());


                        gridView2.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView2.SetFocusedRowCellValue("CostPrice", PriceUnit.ToString());
                        gridView2.SetFocusedRowCellValue("Total", (Total).ToString());
                        gridView2.SetFocusedRowCellValue("CaratPrice", TotalSale.ToString());

                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);


                        gridView2.SetFocusedRowCellValue("Net", Net.ToString());

                        if (Comon.cDec(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "CurrencyPrice")) > 0)
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(Net) * Comon.cDec(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "CurrencyPrice"))).ToString());


                    }
                    if (ColName == "BarCode")
                    {

                        DataTable dt;
                        var flagb = false;

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
                            //gridView2.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);

                            if (flagb == true)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData2(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData2(dt);
                            if (HasColumnErrors == false)
                            {
                                e.Valid = true;
                                view.SetColumnError(gridView2.Columns[ColName], "");
                                gridView2.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView2.FocusedColumn = gridView2.VisibleColumns[0];
                            }
                            //else {
                            //   // HasColumnErrors = true;
                            //    e.Valid = false;
                            //    gridView2.SetColumnError(gridView2.Columns["QTY"], "");
                            //    e.ErrorText = "الكمية غير متوفرة";
                            //    gridView2.FocusedColumn = gridView2.VisibleColumns[6];

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
                            //gridView2.Columns[SizeName].ColumnEdit = rSize;
                            //gridControl.RepositoryItems.Add(rSize);
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData2(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData2(dt);
                            e.Valid = true;
                            view.SetColumnError(gridView2.Columns[ColName], "");
                        }
 

                    }
                    if (ColName == "CurrencyPrice")
                    {
                        if (Comon.cDec(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Net")) > 0)
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Net"))).ToString());

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
                            
                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData2(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData2(dt);
                            e.Valid = true;
                            view.SetColumnError(gridView2.Columns[ColName], "");
                        }
                    }
                    if (ColName == "Net")
                    {
                        decimal additonalVAlue = 0;
                        if (Comon.ConvertToDecimalPrice(val.ToString()) > 0)
                        {
                            string BarCode = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"]).ToString();
                            decimal Qty = Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("QTY"));
                            bool HasVat = chkForVat.Checked;
                            decimal Net = (Comon.ConvertToDecimalPriceTree(val.ToString()) + Comon.ConvertToDecimalPrice(gridView2.GetFocusedRowCellValue("Discount")));
                            additonalVAlue = Comon.ConvertToDecimalPrice(Net - ((Net * 100) / (100 + MySession.GlobalPercentVat)));
                            if (BarCode == "24")
                                additonalVAlue = 0;
                            decimal CostPrice = Comon.ConvertToDecimalPrice((Net - additonalVAlue) / (Comon.ConvertToDecimalQty(gridView2.GetFocusedRowCellValue("QTY"))));
                            decimal Total = Comon.ConvertToDecimalPrice(Net) - additonalVAlue;
                            gridView2.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                            gridView2.SetFocusedRowCellValue("CostPrice", CostPrice.ToString());
                            gridView2.SetFocusedRowCellValue("Total", Total.ToString());
                            gridView2.SetFocusedRowCellValue("Net", val.ToString());
                        }
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
                                //e.Valid = false;
                                //HasColumnErrors = true;
                                //e.ErrorText = Messages.msgNoFoundSizeForItem;
                            }
                            else
                            {
                                if (MySession.GlobalAllowUsingDateItems)
                                {
                                    MySession.GlobalAllowUsingDateItems = false;
                                    FileItemData2(dt);
                                    MySession.GlobalAllowUsingDateItems = true;
                                }
                                else
                                    FileItemData2(dt);
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
                    decimal Total =   SalePrice;
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
                else if (ColName == GroupName)
                {
                    DataTable dtGroupID = Lip.SelectRecord("Select GroupID, ArbName from Stc_ItemsGroups Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "')");
                    if (dtGroupID.Rows.Count > 0)
                    {
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CurrencyName"], cmbCurency.Text.ToString());
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CurrencyEquivalent"], 0);
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
                        if (dtGroupID.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridView2.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                        else
                        {

                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["GroupID"], dtGroupID.Rows[0]["GroupID"].ToString());
                            gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[ItemName], dtGroupID.Rows[0]["ArbName"].ToString());

                            e.Valid = true;
                            view.SetColumnError(gridView2.Columns[ColName], "");
                        }
                    }
                    else
                    {
                        //e.Valid = false;
                        //HasColumnErrors = true;
                        //e.ErrorText = Messages.msgNoFoundThisItem;
                        //view.SetColumnError(gridView2.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                if (ColName == "CurrencyName")
                {
                    DataTable dt = Lip.SelectRecord("Select ID ,ExchangeRate from Acc_Currency Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "')");
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "CurrencyID", dt.Rows[0]["ID"]);
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "CurrencyPrice", dt.Rows[0]["ExchangeRate"]);
                    if (Comon.cDec(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Net")) > 0)
                        gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "CurrencyPrice")) * Comon.cDec(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Net"))).ToString());
                }
            }
            CalculateRow2();
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
                                RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + BarCode + "'," + Comon.cInt(txtStoreID.Text) +  ") AS RemainQty").Rows[0]["RemainQty"].ToString());
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

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e)
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
                                view.SetColumnError(gridView2.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView2.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView2.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(gridView2.Columns[ColName], "");
                            }

                            if (ColName == "QTY")
                            {
                                string BarCode = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"]).ToString();
                                double RemindQty = 0;
                                RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + BarCode + "'," + Comon.cInt(txtStoreID.Text) + ") AS RemainQty").Rows[0]["RemainQty"].ToString());
                                double Qty = Comon.cDbl(QtyItem);

                                if (RemindQty <= 0)
                                {

                                    if (MySession.GlobalWayOfOutItems == "PreventOutItemsWithOutBalance")
                                    {

                                        HasColumnErrors = true;
                                        view.SetColumnError(gridView2.Columns[ColName], "الكمية  غير متوفرة ");
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
        private void gridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (this.gridView2.ActiveEditor is CheckEdit)
            {
                gridView2.Columns["HavVat"].OptionsColumn.AllowEdit = true;
                CalculateRow(gridView2.FocusedRowHandle, Comon.cbool(e.Value.ToString()));
            }
            if (this.gridView2.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "QTY")
                {
                    QtyItem = Comon.cDec(e.Value);
                    Caliber = Comon.cInt(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, SizeName).ToString());
                }
            }
        }
        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
           

        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
 

        }
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }
        private void FileItemData(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], "1");

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
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());

                //Get  AverageCostPrice
                decimal AverageCost = frmItems.GetItemAverageCostPrice(Comon.cLong(dt.Rows[0]["ItemID"]), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cInt(txtStoreID.Text), 0, 0, 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], AverageCost);
                ///////////////////
                ///
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
        private void FileItemData2(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PackingQty"], "1");

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["GroupID"], dt.Rows[0]["GroupID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[GroupName], dt.Rows[0][GroupName].ToString());
         
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());

                if (UserInfo.Language == iLanguage.English)
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());

                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["CostPrice"], 0);


                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["StoreID"], txtStoreID.Text);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Discount"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Bones"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["AdditionalValue"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Cancel"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Serials"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Description"], "");
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["PageNo"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["ItemStatus"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Caliber"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Equivalen"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Net"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["TheCount"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Height"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Width"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["Total"], 0);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["QTY"], 1);
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["STONE_W"], 0);

                
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
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["STONE_W"], 0);
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
                if (col.FieldName == "BarCode" || col.FieldName == "Description" || col.FieldName == "Discount" || col.FieldName == "ExpiryDate" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SalePrice")
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

            gridView1.MoveLast();

            int length = gridView1.RowCount - 1;
            if (length <= 0&& comboTypeInvoice.SelectedIndex==0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput+".. في نافذة شراء الأوزان ");
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

        bool IsValidGrid2()
        {
            double num;

            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            gridView2.MoveLast();

            int length = gridView2.RowCount - 1;
            if (length <= 0&& comboTypeInvoice.SelectedIndex==1)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput+" .. في نافذة شراء القطع ");
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in gridView2.Columns)
                {
                    if (  col.FieldName == "Net" || col.FieldName == "Total" || col.FieldName == "SizeID" || col.FieldName == "QTY"   || col.FieldName == "SalePrice")
                    {

                        var cellValue = gridView2.GetRowCellValue(i, col); 

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView2.SetColumnError(col, Messages.msgInputIsRequired  );
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                       

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
        private void CalculateRow2(int Row = -1, bool IsHavVat = false)
        {
            try
            {
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
                decimal QTY18 = 0;
                decimal QTY21 = 0;

                decimal QTY22 = 0;
                decimal QTY24 = 0;
                decimal InvoiceTotalGold = 0;
                decimal SalePriceRow = 0;
                decimal CostPriceRow = 0;
                decimal TotalRow = 0;
                decimal NetRow = 0;
                decimal TotalBeforeDiscountRow = 0;
                decimal AdditionalAmountRow = 0;
                bool HavVatRow = false;
                MySession.UseNetINInvoiceSales = 1;
                decimal TotalSale = 0;
                decimal TotalCostGold = 0;
                if (comboTypeInvoice.SelectedIndex == 0)
                {
                    for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                    {
                        int Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                        QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                        QtyItem = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY"));
                        SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                        CostPriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());

                        DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                        HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                        AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                        TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * (SalePriceRow + CostPriceRow));

                        TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPriceTree(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                        NetRow = Comon.ConvertToDecimalPriceTree(gridView1.GetRowCellValue(i, "Net"));
                        TotalBeforeDiscountRow = TotalRow;

                        if (Caliber == 18)
                            QTY18 = QTY18 + QtyItem;
                        if (Caliber == 21)
                            QTY21 = QTY21 + QtyItem;

                        if (Caliber == 22)
                            QTY22 = QTY22 + QtyItem;
                        if (Caliber == 24)
                            QTY24 = QTY24 + QtyItem;


                        TotalBeforeDiscount += TotalBeforeDiscountRow;
                        TotalAfterDiscount += TotalRow;
                        DiscountTotal += DiscountRow;
                        AdditionalAmount += AdditionalAmountRow;
                        Net += NetRow;

                        TotalSale += SalePriceRow;
                        TotalCostGold += CostPriceRow;
                    }
                    if (rowIndex < 0)
                    {
                        var ResultCaliber = Comon.cInt(gridView1.GetRowCellValue(rowIndex, SizeName));
                        var ResultQTY = gridView1.GetRowCellValue(rowIndex, "QTY");
                        QtyItem = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(rowIndex, "QTY"));
                        var ResultSalePrice = gridView1.GetRowCellValue(rowIndex, "SalePrice");
                        var ResultCostPrice = gridView1.GetRowCellValue(rowIndex, "CostPrice");
                        var ResultDiscount = gridView1.GetRowCellValue(rowIndex, "Discount");
                        var ResultHavVat = gridView1.GetRowCellValue(rowIndex, "HavVat");
                        QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                        SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                        CostPriceRow = ResultCostPrice != null ? Comon.ConvertToDecimalPrice(ResultCostPrice.ToString()) : 0;
                        DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                        HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                        AdditionalAmountRow = Comon.ConvertToDecimalPriceTree(gridView1.GetRowCellValue(rowIndex, "AdditionalValue"));
                        NetRow = Comon.ConvertToDecimalPriceTree(gridView1.GetRowCellValue(rowIndex, "Net"));
                        TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * (SalePriceRow + CostPriceRow));
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
                            AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPriceTree((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
                            NetRow = Comon.ConvertToDecimalPriceTree(TotalRow + AdditionalAmountRow);
                        }

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

                        TotalSale += SalePriceRow;
                        TotalCostGold += CostPriceRow;
                    }
                }
                else
                    if (comboTypeInvoice.SelectedIndex == 1)
                    {

                        for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                        {
                            int Caliber = Comon.cInt(gridView2.GetRowCellValue(i, SizeName).ToString());
                            QTYRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                            QtyItem = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(i, "QTY"));
                            SalePriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                            CostPriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());

                            DiscountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount"));
                            HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView2.GetRowCellValue(i, "HavVat"));
                            AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue"));
                            NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net"));
                            TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(    CostPriceRow);

                            TotalRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Total"));
                            if (chkForVat.Checked)
                                AdditionalAmountRow = Comon.ConvertToDecimalPriceTree(gridView2.GetRowCellValue(i, "AdditionalValue"));
                            else
                                AdditionalAmountRow = 0;
                            NetRow = Comon.ConvertToDecimalPriceTree(gridView2.GetRowCellValue(i, "Net"));
                            TotalBeforeDiscountRow = TotalRow;

                            if (Caliber == 18)
                                QTY18 = QTY18 + QtyItem;
                            if (Caliber == 21)
                                QTY21 = QTY21 + QtyItem;

                            if (Caliber == 22)
                                QTY22 = QTY22 + QtyItem;
                            if (Caliber == 24)
                                QTY24 = QTY24 + QtyItem;


                            TotalBeforeDiscount += TotalBeforeDiscountRow;
                            TotalAfterDiscount += TotalRow;
                            DiscountTotal += DiscountRow;
                            AdditionalAmount += AdditionalAmountRow;
                            Net += NetRow;

                            TotalSale += SalePriceRow;
                            TotalCostGold += CostPriceRow;
                        }
                        if (rowIndex < 0)
                        {
                            var ResultCaliber = Comon.cInt(gridView2.GetRowCellValue(rowIndex, SizeName));
                            var ResultQTY = gridView2.GetRowCellValue(rowIndex, "QTY");
                            QtyItem = Comon.ConvertToDecimalQty(gridView2.GetRowCellValue(rowIndex, "QTY"));
                            var ResultSalePrice = gridView2.GetRowCellValue(rowIndex, "SalePrice");
                            var ResultCostPrice = gridView2.GetRowCellValue(rowIndex, "CostPrice");
                            var ResultDiscount = gridView2.GetRowCellValue(rowIndex, "Discount");
                            var ResultHavVat = gridView2.GetRowCellValue(rowIndex, "HavVat");
                            QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                            SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                            CostPriceRow = ResultCostPrice != null ? Comon.ConvertToDecimalPrice(ResultCostPrice.ToString()) : 0;
                            DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                            HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                            AdditionalAmountRow = Comon.ConvertToDecimalPriceTree(gridView1.GetRowCellValue(rowIndex, "AdditionalValue"));
                            NetRow = Comon.ConvertToDecimalPriceTree(gridView2.GetRowCellValue(rowIndex, "Net"));
                            TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(  CostPriceRow);
                            if (Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Net")) > 0 && MySession.UseNetINInvoiceSales == 1)
                            {
                                TotalRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Total"));
                                 if (chkForVat.Checked)
                                  AdditionalAmountRow =   Comon.ConvertToDecimalPriceTree(gridView2.GetRowCellValue(rowIndex, "AdditionalValue")) ;
                                else
                                  AdditionalAmountRow=  0;
                               
                                NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Net"));
                                TotalBeforeDiscountRow = TotalRow;
                            }
                            else
                            {
                                TotalRow = Comon.ConvertToDecimalPrice(  CostPriceRow - DiscountRow);
                                AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPriceTree((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
                                NetRow = Comon.ConvertToDecimalPriceTree(TotalRow + AdditionalAmountRow);
                            }

                            if (ResultCaliber == 18)
                                QTY18 = QTY18 + Comon.ConvertToDecimalPrice(QtyItem);
                            if (ResultCaliber == 21)
                                QTY21 = QTY21 + Comon.ConvertToDecimalPrice(QtyItem); ;

                            if (ResultCaliber == 22)
                                QTY22 = QTY22 + Comon.ConvertToDecimalPrice(QtyItem); ;
                            if (ResultCaliber == 24)
                                QTY24 = QTY24 + Comon.ConvertToDecimalPrice(QtyItem); ;


                            NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Net"));
                            TotalBeforeDiscount += TotalBeforeDiscountRow;
                            TotalAfterDiscount += TotalRow;
                            DiscountTotal += DiscountRow;
                            AdditionalAmount += AdditionalAmountRow;
                            Net += NetRow;

                            TotalSale += SalePriceRow;
                            TotalCostGold += CostPriceRow;
                        }
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
                lblAdditionaAmmount.Text = Comon.ConvertToDecimalPriceTree(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = Comon.ConvertToDecimalPriceTree(Net).ToString("N" + 3);
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


                lblTotalSalePrice.Text = Comon.ConvertToDecimalPrice(TotalSale).ToString("N" + MySession.GlobalPriceDigits);
                lblTotalCost.Text = Comon.ConvertToDecimalPrice(TotalCostGold).ToString("N" + MySession.GlobalPriceDigits);


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
                decimal TotalSale = 0;
                decimal TotalCostGold = 0;

                decimal QTYRow = 0;
                decimal QTY18 = 0;
                decimal QTY21 = 0;

                decimal QTY22 = 0;
                decimal QTY24 = 0;

                decimal InvoiceTotalGold = 0;

                decimal SalePriceRow = 0;
                decimal CostPriceRow = 0;
                decimal TotalRow = 0;
                decimal NetRow = 0;
                decimal TotalBeforeDiscountRow = 0;
                decimal AdditionalAmountRow = 0;
                bool HavVatRow = false;
                MySession.UseNetINInvoiceSales = 1;
                if (comboTypeInvoice.SelectedIndex == 0)
                {
                    for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                    {
                        int Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                        QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());

                        SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                        CostPriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());

                        DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                        HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                        AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                        TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);



                        TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                        TotalBeforeDiscountRow = TotalRow;

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

                        TotalSale += SalePriceRow * QTYRow;
                        TotalCostGold += CostPriceRow * QTYRow;
                    }
                    if (rowIndex < 0)
                    {
                        var ResultCaliber = Comon.cInt(gridView1.GetRowCellValue(rowIndex, SizeName));
                        var ResultQTY = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY");

                        var ResultSalePrice = gridView1.GetRowCellValue(rowIndex, "SalePrice");
                        var ResultCostPrice = gridView1.GetRowCellValue(rowIndex, "CostPrice");


                        var ResultDiscount = gridView1.GetRowCellValue(rowIndex, "Discount");
                        var ResultHavVat = gridView1.GetRowCellValue(rowIndex, "HavVat");
                        QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                        SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                        CostPriceRow = ResultCostPrice != null ? Comon.ConvertToDecimalPrice(ResultCostPrice.ToString()) : 0;


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
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                        TotalBeforeDiscount += TotalBeforeDiscountRow;
                        TotalAfterDiscount += TotalRow;
                        DiscountTotal += DiscountRow;
                        AdditionalAmount += AdditionalAmountRow;
                        Net += NetRow;


                        TotalSale += SalePriceRow * QTYRow;
                        TotalCostGold += CostPriceRow * QTYRow;
                    }
                   
                }
                else
                    if (comboTypeInvoice.SelectedIndex == 1)
                    {
                        for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                        {
                            int Caliber = Comon.cInt(gridView2.GetRowCellValue(i, SizeName).ToString());
                            QTYRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());

                            SalePriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                            CostPriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());

                            DiscountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount"));
                            HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView2.GetRowCellValue(i, "HavVat"));
                            AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue"));
                            NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net"));
                            TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice( SalePriceRow);



                            TotalRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Total"));
                            AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue")) : 0;
                            NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net"));
                            TotalBeforeDiscountRow = TotalRow;

                            if (Caliber == 18)
                                QTY18 = QTY18 + QTYRow;
                            if (Caliber == 21)
                                QTY21 = QTY21 + QTYRow;

                            if (Caliber == 22)
                                QTY22 = QTY22 + QTYRow;
                            if (Caliber == 24)
                                QTY24 = QTY24 + QTYRow;



                            NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Net"));
                            TotalBeforeDiscount += TotalBeforeDiscountRow;
                            TotalAfterDiscount += TotalRow;
                            DiscountTotal += DiscountRow;
                            AdditionalAmount += AdditionalAmountRow;
                            Net += NetRow;

                            TotalSale += SalePriceRow ;
                            TotalCostGold += CostPriceRow ;
                        }


                        if (rowIndex < 0)
                        {
                            var ResultCaliber = Comon.cInt(gridView2.GetRowCellValue(rowIndex, SizeName));
                            var ResultQTY = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "QTY");

                            var ResultSalePrice = gridView2.GetRowCellValue(rowIndex, "SalePrice");
                            var ResultCostPrice = gridView2.GetRowCellValue(rowIndex, "CostPrice");


                            var ResultDiscount = gridView2.GetRowCellValue(rowIndex, "Discount");
                            var ResultHavVat = gridView2.GetRowCellValue(rowIndex, "HavVat");
                            QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                            SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                            CostPriceRow = ResultCostPrice != null ? Comon.ConvertToDecimalPrice(ResultCostPrice.ToString()) : 0;


                            DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                            HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                            AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "AdditionalValue"));
                            NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Net"));
                            TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice( SalePriceRow);
                            if (Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Net")) > 0 && MySession.UseNetINInvoiceSales == 1)
                            {
                                TotalRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Total"));
                                AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "AdditionalValue")) : 0;
                                NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Net"));
                                TotalBeforeDiscountRow = TotalRow;
                            }
                            else
                            {
                                TotalRow = Comon.ConvertToDecimalPrice( SalePriceRow - DiscountRow);
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
                            NetRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(rowIndex, "Net"));
                            TotalBeforeDiscount += TotalBeforeDiscountRow;
                            TotalAfterDiscount += TotalRow;
                            DiscountTotal += DiscountRow;
                            AdditionalAmount += AdditionalAmountRow;
                            Net += NetRow;


                            TotalSale += SalePriceRow ;
                            TotalCostGold += CostPriceRow ;
                        }
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

                lblTotalSalePrice.Text = Comon.ConvertToDecimalPrice(TotalSale).ToString("N" + MySession.GlobalPriceDigits);
                lblTotalCost.Text = Comon.ConvertToDecimalPrice(TotalCostGold).ToString("N" + MySession.GlobalPriceDigits);


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
        protected override void Find()
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
                if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtInvoiceID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtInvoiceID, null, "SalesInvoice", "رقـم الـفـاتـورة", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtInvoiceID, null, "SalesInvoice", "Invoice ID", Comon.cInt(cmbBranchesID.EditValue));
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
                    PrepareSearchQuery.Find(ref cls, txtSellerID, lblSellerName, "SellerID", "رقم البائع", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtSellerID, lblSellerName, "SellerID", "Seller ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;

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

            else if (FocusedControl.Trim() == gridControl1.Name)
            {
                if (gridView2.FocusedColumn == null) return;

                if (gridView2.FocusedColumn.Name == "colBarCode" || gridView2.FocusedColumn.Name == "colItemName" || gridView2.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
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

                else if (FocusedControl == gridControl1.Name)
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
                        FileItemData2(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));
                        CalculateRow();

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
                            FileItemData2(Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(itemID), SizeID, UserInfo.FacilityID));
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
                    lblCreditAccountID.Text = row[0]["AccountID"].ToString();
                    lblCreditAccountName.Text = row[0]["AccountName"].ToString();
                }

                //حساب المبيعات
                DataRow[] row2 = dtDeclaration.Select("DeclareAccountName = 'PurchaseAccount'");
                if (row2.Length > 0)
                {
                    lblDebitAccountID.Text = row2[0]["AccountID"].ToString();
                    lblDebitAccountName.Text = row2[0]["AccountName"].ToString();

                   
                }

                //حساب صندوق الذهب المدين
                DataRow[] row1 = dtDeclaration.Select("DeclareAccountName = 'DebitGoldAccountID'");
                if (row.Length > 0)
                {
                    txtDebitGoldAccountID.Text = row1[0]["AccountID"].ToString();
                    lblDebitGoldAccountName.Text = row1[0]["AccountName"].ToString();
                }

                //حساب صندوق الذهب الدائن
                DataRow[] row8 = dtDeclaration.Select("DeclareAccountName = 'CreditGoldAccountID'");
                if (row.Length > 0)
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
                if (row6.Length > 0)
                {
                    lblCreditGoldAccountID.Text = row7[0]["AccountID"].ToString();
                    lblAdditionalAccountName.Text = row7[0]["AccountName"].ToString();

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
                comboTypeInvoice.SelectedIndex = 0;   
                PostToServer = false;
                button1.Visible = false;
                lblInvoiceTotalGold.Text = "0";
                lblTotalSalePrice.Text = "0";
                chkNoSale.Checked = false;
                DiscountCustomer = 0;
                lblTotalCost.Text = "";
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
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultCurencyID);
                lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;

                lstDetail2 = new BindingList<Sales_SalesInvoiceDetails>();
                lstDetail2.AllowNew = true;
                lstDetail2.AllowEdit = true;
                lstDetail2.AllowRemove = true;
                gridControl1.DataSource = lstDetail2;
                dt = new DataTable();
                chkForVat.Checked = true;
                cmbBranchesID_EditValueChanged(null, null);
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
                    strSQL = "SELECT TOP 1 * FROM " + Sales_PurchaseInvoicesDAL.TableName + " Where  GoldUsing= " + GoldUsing + " and Cancel =0 And BranchID=" + Comon.cInt(cmbBranchesID.EditValue);
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
                txtInvoiceID.Text = Sales_PurchaseInvoicesDAL.GetNewID(MySession.GlobalFacilityID, Comon.cInt(cmbBranchesID.EditValue), MySession.UserID).ToString();
                txtRegistrationNo.Text = txtInvoiceID.Text;
                txtDailyID.Text = GetNewDialyID(MySession.GlobalFacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCostCenterID.Text)).ToString();
                ClearFields();
                IdPrint = false;
                EnabledControl(true);
                cmbFormPrinting.EditValue = 1;
                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                gridView2.Focus();
                gridView2.MoveNext();
                gridView2.FocusedColumn = gridView2.VisibleColumns[1];
                //  gridView1.ShowEditor();
                simpleButton1_Click(null, null);
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
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Total", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDateStr", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.DateTime"));
            dtItem.Columns.Add("Bones", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("HavVat", System.Type.GetType("System.Boolean"));
            dtItem.Columns.Add("STONE_W", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Height", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Width", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CaratPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CurrencyID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CurrencyEquivalent", System.Type.GetType("System.Decimal"));
            if (comboTypeInvoice.SelectedIndex == 0)
            {
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
                    dtItem.Rows[i]["STONE_W"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "STONE_W").ToString());

                    dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Bones").ToString());
                    dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                    dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());
                    dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                    dtItem.Rows[i]["ExpiryDateStr"] = "0";
                    dtItem.Rows[i]["ExpiryDate"] = DateTime.Now.ToShortDateString();
                    dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                    dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView1.GetRowCellValue(i, "HavVat").ToString());
                    dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                    dtItem.Rows[i]["AdditionalValue"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                    dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                    dtItem.Rows[i]["Height"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Height").ToString());
                    dtItem.Rows[i]["Width"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Width").ToString());
                    dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());

                    dtItem.Rows[i]["CurrencyID"] = gridView1.GetRowCellValue(i, "CurrencyID").ToString();
                    dtItem.Rows[i]["CurrencyName"] = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                    dtItem.Rows[i]["CurrencyPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                    dtItem.Rows[i]["CurrencyEquivalent"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                    dtItem.Rows[i]["Cancel"] = 0;
                }
                gridControl.DataSource = dtItem;
            }
            else if (comboTypeInvoice.SelectedIndex == 1)
            {
                dtItem.Columns.Add(GroupName, System.Type.GetType("System.String"));
                dtItem.Columns.Add("GroupID", System.Type.GetType("System.Decimal"));
                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[i]["ID"] = i;
                    dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; 
                    dtItem.Rows[i]["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                    dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView2.GetRowCellValue(i, "ItemID").ToString());
                    dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView2.GetRowCellValue(i, "SizeID").ToString());
                    
                    dtItem.Rows[i][ItemName] = gridView2.GetRowCellValue(i, ItemName).ToString();
                    dtItem.Rows[i][SizeName] = gridView2.GetRowCellValue(i, SizeName).ToString();
                    dtItem.Rows[i][GroupName] = gridView2.GetRowCellValue(i, GroupName).ToString();
                    dtItem.Rows[i]["GroupID"] = gridView2.GetRowCellValue(i, "GroupID").ToString();
                    dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                    dtItem.Rows[i]["STONE_W"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "STONE_W").ToString());

                    dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Bones").ToString());
                    dtItem.Rows[i]["Description"] = gridView2.GetRowCellValue(i, "Description").ToString();
                    dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView2.GetRowCellValue(i, "StoreID").ToString());
                    dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount").ToString());
                    dtItem.Rows[i]["ExpiryDateStr"] = "0";
                    dtItem.Rows[i]["ExpiryDate"] = DateTime.Now.ToShortDateString();
                    dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());
                    dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView2.GetRowCellValue(i, "HavVat").ToString());
                    dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Total").ToString());
                    dtItem.Rows[i]["AdditionalValue"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue").ToString());
                    dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net").ToString());
                    dtItem.Rows[i]["Height"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Height").ToString());
                    dtItem.Rows[i]["Width"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Width").ToString());
                    dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                    dtItem.Rows[i]["CaratPrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CaratPrice").ToString());

                    dtItem.Rows[i]["CurrencyID"] = gridView2.GetRowCellValue(i, "CurrencyID").ToString();
                    dtItem.Rows[i]["CurrencyName"] = gridView2.GetRowCellValue(i, "CurrencyName").ToString();
                    dtItem.Rows[i]["CurrencyPrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CurrencyPrice").ToString());
                    dtItem.Rows[i]["CurrencyEquivalent"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                    dtItem.Rows[i]["Cancel"] = 0;
                }
                gridControl1.DataSource = dtItem;
            }

            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
        }
      

        private bool AddItems(int Rowindex, string BarCode)
        {
            try
            {
                string[] ArrValues = new string[10000];
                DataTable dtTest = new DataTable();
                Application.DoEvents();
                //'إضافة المواد
                cItems Item = new cItems();
                Application.DoEvents();
                Lip.NewFields();
                Lip.Table = "Stc_Items";
                Boolean IsNewItem = false;
                long ItemID = Comon.cInt(Lip.GetValue(" Select ItemID from Stc_ItemUnits  where BarCode='" + BarCode.Trim() + "'"));

                int GroupID = Comon.cInt(gridView2.GetRowCellValue(Rowindex, "GroupID").ToString());

                int ItemG = 0;

                if (ItemID == 0)
                {
                    ItemID = Item.GetNewID();
                    ItemG = Lip.GetNewIDSaveItem(GroupID,5);
                    IsNewItem = true;
                }
                else
                    ItemG = Comon.cInt(Lip.GetValue(" Select ItemGroupID from Stc_Items Where ItemID=" + ItemID).ToString());

                Lip.AddNumericField("ItemID", ItemID.ToString());
                gridView2.SetRowCellValue(Rowindex, gridView2.Columns["ItemID"], ItemID.ToString());
                Lip.AddStringField("ArbName", gridView2.GetRowCellValue(Rowindex, ItemName).ToString());
                Lip.AddStringField("EngName", gridView2.GetRowCellValue(Rowindex, ItemName).ToString());
                Lip.AddNumericField("GroupID", GroupID);
                Lip.AddNumericField("ItemGroupID", ItemG);

                Lip.AddStringField("Notes", "");
                Lip.AddNumericField("TypeID", 5);
                Lip.AddNumericField("UserID", UserInfo.ID);
                Lip.AddNumericField("RegDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                Lip.AddNumericField("RegTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                Lip.AddNumericField("EditUserID", UserInfo.ID);
                Lip.AddNumericField("EditDate", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
                Lip.AddNumericField("EditTime", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
                Lip.AddStringField("ComputerInfo", UserInfo.ComputerInfo);
                Lip.AddStringField("EditComputerInfo", UserInfo.ComputerInfo);
                Lip.AddNumericField("Cancel", 0);
                Lip.AddStringField("IsVat", "1");
                Lip.AddNumericField("ColorID", 0);
                Lip.AddNumericField("BrandID", 0);
                Lip.AddNumericField("BaseID", 0);
                Lip.AddNumericField("BranchID", 0);

                Lip.AddNumericField("STONE_W", gridView2.GetRowCellValue(Rowindex, "STONE_W").ToString());
       
                Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                Lip.sCondition = " ItemID = " + ItemID;

                if (IsNewItem)
                    Lip.ExecuteInsert();
                else
                    Lip.ExecuteUpdate();

                //'إضافة وحدات المواد
                cItemsUnits ItemUnit = new cItemsUnits();

                strSQL = "delete from Stc_ItemUnits where BarCode='" + BarCode.Trim() + "'";
                Lip.ExecututeSQL(strSQL);

                Application.DoEvents();
                Lip.NewFields();
                Lip.Table = "Stc_ItemUnits";
                long SizeID = Comon.cLong(Lip.GetValue("Select Top 1 SizeID From Stc_SizingUnits Where ArbName='" + gridView2.GetRowCellValue(Rowindex, "ArbSizeName").ToString() + "'"));
                Lip.AddNumericField("ItemID", ItemID.ToString());

                strSQL = "Select Notes From Stc_ItemsGroups where GroupID=" + GroupID;
                DataTable dtGroup = Lip.SelectRecord(strSQL);
                string GroupName = dtGroup.Rows[0]["Notes"].ToString();

                if (BarCode == string.Empty)
                    BarCode = "G"+GroupName + ItemG.ToString().PadLeft(4, '0');


                Lip.AddNumericField("SizeID", SizeID.ToString());
                Lip.AddStringField("BarCode", BarCode);
                Lip.AddNumericField("PackingQty", 1);

                gridView2.SetRowCellValue(Rowindex, gridView2.Columns["BarCode"], BarCode.ToString());
                gridView2.SetRowCellValue(Rowindex, gridView2.Columns["SizeID"], SizeID.ToString());
                gridView2.SetRowCellValue(Rowindex, gridView2.Columns["ItemID"], ItemID.ToString());


                decimal CostPrice = Comon.cDec(gridView2.GetRowCellValue(Rowindex, "CostPrice").ToString());
                decimal Bones = Comon.cDec(gridView2.GetRowCellValue(Rowindex, "Bones").ToString());
                //سعر تكلفة مع مصاريف
                decimal SpendPrice = Comon.ConvertToDecimalPrice(CostPrice + Comon.cDec(Bones));
                //سعر تكلفة المحل
                decimal CaratPrice = Comon.ConvertToDecimalPrice(Comon.cDec(SpendPrice ));
                //سعر الكارت وهو البيع
                decimal SalePrice = Comon.ConvertToDecimalPrice(CaratPrice );
                Lip.AddNumericField("SalePrice", SalePrice.ToString());
                Lip.AddNumericField("STONE_W", gridView2.GetRowCellValue(Rowindex, "STONE_W").ToString());


                Lip.AddNumericField("MinLimitQty", gridView2.GetRowCellValue(Rowindex, "QTY").ToString());

                Lip.AddNumericField("MaxLimitQty", 0);
                Lip.AddNumericField("LastCostPrice", 0);
                Lip.AddNumericField("LastSalePrice", 0);
                Lip.AddNumericField("SpecialSalePrice", 0);
                Lip.AddNumericField("SpecialCostPrice", 0);
                Lip.AddNumericField("ItemProfit", 20);
                Lip.AddNumericField("AllowedPercentDiscount", 50);
                Lip.AddNumericField("UnitCancel", 0);
                Lip.AddNumericField("AverageCostPrice", 0);
                Lip.AddNumericField("BranchID", 0);
                Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                Lip.ExecuteInsert();

                strSQL = "delete from Sales_PurchaseInvoiceDetails where  InvoiceID= -1 And  BarCode='" + BarCode.Trim() + "'";
                Lip.ExecututeSQL(strSQL);

                {
                    Application.DoEvents();
                    Lip.NewFields();
                    Lip.Table = "Sales_PurchaseInvoiceDetails";
                    Lip.AddNumericField("InvoiceID", -1);
                    Lip.AddNumericField("BranchID", 0);
                    Lip.AddNumericField("FacilityID", UserInfo.FacilityID.ToString());
                    Lip.AddNumericField("ItemID", ItemID.ToString());
                    Lip.AddNumericField("SizeID", SizeID.ToString());
                    Lip.AddNumericField("QTY", gridView2.GetRowCellValue(Rowindex, "QTY").ToString());
                    Lip.AddNumericField("CostPrice", gridView2.GetRowCellValue(Rowindex, "CostPrice").ToString());
                    Lip.AddNumericField("Bones", 0);
                    Lip.AddNumericField("StoreID", 0);
                    Lip.AddNumericField("Discount", 0);
                    Lip.AddNumericField("ExpiryDate", 20201101);
                    Lip.AddNumericField("SalePrice", gridView2.GetRowCellValue(Rowindex, "SalePrice").ToString());
                    Lip.AddStringField("BarCode", BarCode);
                    Lip.AddStringField("Serials", "");
                    Lip.AddNumericField("Cancel", 0);
                    Lip.AddNumericField("ItemStatus", -1);
                    Lip.AddNumericField("AdditionalValue", 0);
                    Lip.AddNumericField("Caliber", Caliber);
                    Lip.AddNumericField("STONE_W", gridView2.GetRowCellValue(Rowindex, "STONE_W").ToString());

                    Lip.AddStringField("Description", gridView2.GetRowCellValue(Rowindex, "Description").ToString());

                    Lip.ExecuteInsert();

                }
                return true;
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return false;
            }
        }
        protected override void DoSave()
        {
            try
            {

               if (!Validations.IsValidForm(this))
                    return;
               if (comboTypeInvoice.SelectedIndex == 0)
                if (!IsValidGrid())
                    return;
               if(comboTypeInvoice.SelectedIndex==1)
                if (!IsValidGrid2())
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

                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    String BarCode = gridView2.GetRowCellValue(i, "BarCode").ToString();

                    if (AddItems(i, BarCode) == false)
                    {

                        long ItemID = Comon.cInt(Lip.GetValue(" Select ItemID from Stc_ItemUnits  where BarCode='" + BarCode.Trim() + "'"));
                        Lip.ExecututeSQL("Delete from Stc_ItemUnits Where ItemID=" + ItemID);
                        Lip.ExecututeSQL("Delete from Stc_Items Where ItemID=" + ItemID);
                        Lip.ExecututeSQL("Delete from Sales_PurchaseInvoiceDetails Where ItemID=" + ItemID);
                        Messages.MsgInfo("يرجى التاكد من بيانات الصنف ", BarCode);
                        return;
                    }
                }

               
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                if (Comon.ConvertToDecimalPrice(lblNetBalance.Text) < Comon.ConvertToDecimalPrice(txtNetAmount.Text))
                {
                    txtNetAmount.Focus();
                    txtNetAmount.ToolTip = "مبلغ الشبكة  اكبر من الصافي ";
                    Validations.ErrorText(txtNetAmount, txtNetAmount.ToolTip);
                    return;
                }
             
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

                    dt = Sales_PurchaseInvoicesDAL.frmGetDataDetalByID(InvoiceID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        button1.Visible = true;
                        //Validate
                       txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();

                       txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        StopSomeCode = true;
                        cmbMethodID.EditValue = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());
                        StopSomeCode = false;

                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        cmbNetType.EditValue = Comon.cDbl(dt.Rows[0]["NetType"].ToString());

                        txtSupplierID.Text = dt.Rows[0]["SupplierID"].ToString();
                        txtSupplierID_Validating(null, null);
                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"].ToString());
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
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();
                        //txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();
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
                        if (Comon.cInt(dt.Rows[0]["TypeGold"]) != 1)
                        {
                            gridControl.DataSource = dt;
                            lstDetail.AllowNew = true;
                            lstDetail.AllowEdit = true;
                            lstDetail.AllowRemove = true;
                            comboTypeInvoice.SelectedIndex = 0;
                        }
                        else
                        {
                            gridControl1.DataSource = dt;
                            lstDetail2.AllowNew = true;
                            lstDetail2.AllowEdit = true;
                            lstDetail2.AllowRemove = true;
                            comboTypeInvoice.SelectedIndex = 1;
                        }

                        
                       
                        
                        
                        SumTotalBalanceAndDiscountread();
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
        private void Save()
        {

            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            Sales_PurchaseInvoiceMaster objRecord = new Sales_PurchaseInvoiceMaster();
            objRecord.InvoiceID = 0;
            if (PostToServer == true)
            {
                objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                IsNewRecord = true;
            }

            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.MethodeID = Comon.cInt(cmbMethodID.EditValue);
            objRecord.CurencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            objRecord.NetType = Comon.cDbl(cmbNetType.EditValue);
            
            objRecord.SupplierInvoiceID = Comon.cInt(txtSupplierInvoiceID.Text);
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SupplierID = Comon.cDbl(txtSupplierID.Text);
            objRecord.SupplierName =lblSupplierName.Text;
         
            //objRecord.SellerID = Comon.cInt(cmbSellerID.EditValue);
           
           objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Purchase  Invoice" : "فاتوره  مشتريات ذهب ");
           txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Purchase  Invoice" : " فاتوره  مشتريات ذهب  "));
            objRecord.Notes = txtNotes.Text;
            //Account
            objRecord.DebitAccount = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            objRecord.DiscountCreditAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
            objRecord.CreditGoldAccountID = Comon.cDbl(txtCreditGoldAccountID.Text);
            objRecord.DebitGoldAccountID = Comon.cDbl(txtDebitGoldAccountID.Text);


            objRecord.InvoiceGoldTotal= Comon.cDec(lblInvoiceTotalGold.Text);
            objRecord.CheckAccount = Comon.cDbl(lblChequeAccountID.Text);
            objRecord.NetAccount = Comon.cDbl(lblNetAccountID.Text);
            objRecord.AdditionalAccount = Comon.cDbl(lblAdditionalAccountID.Text);
            objRecord.NetProcessID = txtNetProcessID.Text;
            objRecord.CheckID = txtCheckID.Text;
            objRecord.VATID = txtVatID.Text;
            objRecord.GoldUsing = 1;


            //Date
            objRecord.CheckSpendDate = Comon.ConvertDateToSerial(txtCheckSpendDate.Text).ToString();
            objRecord.WarningDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();
            objRecord.ReceiveDate = Comon.ConvertDateToSerial(txtWarningDate.Text).ToString();

            //Ammount

            objRecord.NetAmount = Comon.cDbl(txtNetAmount.Text);
            objRecord.DiscountOnTotal = Comon.cDbl(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = (Comon.cDec(lblInvoiceTotalBeforeDiscount.Text));
            objRecord.AdditionaAmountTotal = Comon.cDec(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Comon.cDbl(lblNetBalance.Text);
            objRecord.Mobile = txtCustomerMobile.Text;
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

            Sales_PurchaseInvoiceDetails returned;
            List<Sales_PurchaseInvoiceDetails> listreturned = new List<Sales_PurchaseInvoiceDetails>();
            if (comboTypeInvoice.SelectedIndex == 0)
            {
                objRecord.TypeGold = 0;
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    returned = new Sales_PurchaseInvoiceDetails();
                    returned.ID = i;
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                    returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                    returned.QTY = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
                    returned.STONE_W = Comon.cDbl(gridView1.GetRowCellValue(i, "STONE_W").ToString());


                    returned.Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                    returned.Equivalen = Comon.ConvertTo21Caliber(returned.QTY, Comon.cInt(returned.Caliber), 18);

                    returned.CostPrice = Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice").ToString()); ;
                    returned.Bones = Comon.cInt(gridView1.GetRowCellValue(i, "Bones").ToString());
                    returned.Description = gridView1.GetRowCellValue(i, "Description").ToString();
                    returned.StoreID = Comon.cInt(txtStoreID.Text);
                    returned.Discount = Comon.cDec(gridView1.GetRowCellValue(i, "Discount").ToString());
                    returned.CurrencyID = Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString());
                    returned.CurrencyName = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                    returned.CurrencyPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                    returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                    returned.ExpiryDateStr = 0;
                    returned.SalePrice = Comon.cDec(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    returned.AdditionalValue = Comon.cDec(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());

                    returned.CurrencyID = Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString());
                    returned.CurrencyName = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                    returned.CurrencyPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                    returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());

                    if (returned.BarCode.Trim() == "24")
                        returned.AdditionalValue = 0;

                    returned.Net = Comon.cDec(gridView1.GetRowCellValue(i, "Net").ToString());
                    returned.Total = Comon.cDec(gridView1.GetRowCellValue(i, "Total").ToString());

                    if (returned.AdditionalValue == 0)
                        returned.HavVat = false;
                    else
                        returned.HavVat = true;
                    returned.Cancel = 0;
                    returned.Serials = "";
                    if (returned.QTY <= 0 || returned.StoreID <= 0 || (returned.CostPrice <= 0 && returned.SalePrice <= 0) || returned.SizeID <= 0 || returned.ItemID <= 0)
                        continue;
                    listreturned.Add(returned);
                }

            }
            else
                if (comboTypeInvoice.SelectedIndex == 1)
                {
                    objRecord.TypeGold = 1;
                    for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                    {

                        returned = new Sales_PurchaseInvoiceDetails();
                        returned.ID = i;
                        returned.FacilityID = UserInfo.FacilityID;
                        returned.BarCode = gridView2.GetRowCellValue(i, "BarCode").ToString();
                        returned.ItemID = Comon.cInt(gridView2.GetRowCellValue(i, "ItemID").ToString());
                        returned.SizeID = Comon.cInt(gridView2.GetRowCellValue(i, "SizeID").ToString());
                        returned.QTY = Comon.cDec(gridView2.GetRowCellValue(i, "QTY").ToString());
                        returned.STONE_W = Comon.cDbl(gridView2.GetRowCellValue(i, "STONE_W").ToString());

                        returned.Caliber = Comon.cInt(gridView2.GetRowCellValue(i, SizeName).ToString());
                        returned.Equivalen = Comon.ConvertTo21Caliber(returned.QTY, Comon.cInt(returned.Caliber), 18);

                        returned.CostPrice = Comon.cDec(gridView2.GetRowCellValue(i, "CostPrice").ToString()); ;
                        returned.Bones = Comon.cInt(gridView2.GetRowCellValue(i, "Bones").ToString());
                        returned.Description = gridView2.GetRowCellValue(i, "Description").ToString();
                        returned.StoreID = Comon.cDbl(txtStoreID.Text);
                        returned.Discount = Comon.cDec(gridView2.GetRowCellValue(i, "Discount").ToString());

                        returned.ExpiryDateStr = 0;
                        returned.SalePrice = Comon.cDec(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                        returned.CaratPrice = Comon.cDec(gridView2.GetRowCellValue(i, "CaratPrice").ToString());
                        returned.AdditionalValue = Comon.cDec(gridView2.GetRowCellValue(i, "AdditionalValue").ToString());
 

                        returned.Net = Comon.cDec(gridView2.GetRowCellValue(i, "Net").ToString());
                        returned.Total = Comon.cDec(gridView2.GetRowCellValue(i, "Total").ToString());
                        returned.CurrencyID = Comon.cInt(gridView2.GetRowCellValue(i, "CurrencyID").ToString());
                        returned.CurrencyName = gridView2.GetRowCellValue(i, "CurrencyName").ToString();
                        returned.CurrencyPrice = Comon.cDbl(gridView2.GetRowCellValue(i, "CurrencyPrice").ToString());
                        returned.CurrencyEquivalent = Comon.cDbl(gridView2.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                        
                        returned.Cancel = 0;
                        returned.Serials = "";

                        if (returned.QTY <= 0 || returned.StoreID <= 0 || (returned.CostPrice <= 0 && returned.SalePrice <= 0) || returned.SizeID <= 0 || returned.ItemID <= 0)
                            continue;
                        listreturned.Add(returned);
                    }
                }

            if (listreturned.Count > 0)
            {
                objRecord.PurchaseDatails = listreturned;
                string Result = Sales_PurchaseInvoicesDAL.InsertUsingXML(objRecord, Comon.cInt(txtEnteredByUserID.Text), IsNewRecord).ToString();
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
                        Lip.ExecututeSQL("Update " + Sales_SaleInvoicesDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sales_SaleInvoicesDAL.PremaryKey + " = " + txtInvoiceID.Text);

                }
                SplashScreenManager.CloseForm(false);
                if (PostToServer == true)
                    return;
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

                    if (Comon.cLong(Result) >= 0)
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
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired );
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

                Sales_PurchaseInvoiceMaster model = new Sales_PurchaseInvoiceMaster();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                int Result = Sales_PurchaseInvoicesDAL.DeleteSales_PurchaseInvoiceMaster(model);
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
            
                ReportName = "rptPurchaseInvoice";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["SupplierName"].Value = lblSupplierName.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = cmbBranchesID.Text.Trim().ToString();
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
                rptForm.Parameters["TotalGold"].Value = Comon.ConvertToDecimalQty(lblInvoiceTotalGold.Text);
                rptForm.Parameters["TotalCost"].Value =Comon.ConvertToDecimalQty(lblTotalCost.Text);
                rptForm.Parameters["TotalSale"].Value =  Comon.ConvertToDecimalQty(lblTotalSalePrice.Text);
                rptForm.Parameters["TotalWightGold"].Value = (Comon.ConvertToDecimalQty(lbl18.Text) + Comon.ConvertToDecimalQty(lbl21.Text) + Comon.ConvertToDecimalQty(lbl22.Text) + Comon.ConvertToDecimalQty(lbl24.Text)).ToString();


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
                    row["Bones"] = gridView1.GetRowCellValue(i, "Bones").ToString();
                    row["ExpiryDate"] = DateTime.Now.ToShortDateString();
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
  
        //protected override void DoPrint()
        //{
        //    try
        //    {
        //      MySession.VAtCompnyGlobal  ="23334455655656";
        //      MySession.VAtCompnyGlobal = "23334455655656";
        //        if (IsNewRecord)
        //        {
        //            Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforePrinting);
        //            return;
        //        }
        //        Application.DoEvents();
        //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
        //        /******************** Report Body *************************/
        //        if (Comon.cInt(cmbFormPrinting.EditValue) == 2)
        //        { 
        //            PrintDot();
        //            return;
        //        }
        //        ReportName = "rptSalesInvoice";
        //        bool IncludeHeader = true;
        //        string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
        //        if (UserInfo.Language == iLanguage.English)
        //            rptFormName = ReportName + "Arb";
        //        if (gridView1.Columns["Description"].Visible == true)
        //            rptFormName = "rptSalesInvoiceArb";
        //        XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
        //        /********************** Master *****************************/
        //        rptForm.RequestParameters = false;
        //        rptForm.Parameters["InvoiceID"].Value = txtDailyID.Text.Trim().ToString();
        //        rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
        //        rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
        //        rptForm.Parameters["RemaindAmount"].Value = lblRemaindAmount.Text.Trim().ToString();
        //        rptForm.Parameters["PaidAmount"].Value = txtPaidAmount.Text.Trim().ToString();
        //        if (Comon.cInt(cmbMethodID.EditValue) == 2)
        //        {
        //            rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.ToString();
        //        }
        //        else //if (Comon.cInt(cmbMethodID.EditValue) == 2)
        //        {
        //            rptForm.Parameters["CustomerName"].Value = txtSupplierName.Text.ToString();
        //        }
        //        if (Comon.cInt(cmbMethodID.EditValue) == 5)
        //        {
        //            rptForm.Parameters["CashTotal"].Value = Comon.ConvertToDecimalPrice(lblNetBalance.Text.Trim().ToString()) - Comon.ConvertToDecimalPrice(txtNetAmount.Text.Trim().ToString());
        //            rptForm.Parameters["NetTotal"].Value = txtNetAmount.Text.Trim().ToString();
        //        }
        //        else if (Comon.cInt(cmbMethodID.EditValue) == 3)
        //        {

        //            rptForm.Parameters["CashTotal"].Value = 0;
        //            rptForm.Parameters["NetTotal"].Value = lblNetBalance.Text.Trim().ToString();

        //        }
        //        else
        //        {

        //            rptForm.Parameters["NetTotal"].Value = 0;
        //            rptForm.Parameters["CashTotal"].Value = lblNetBalance.Text.Trim().ToString();


        //        }

        //        rptForm.Parameters["MethodName"].Value = "فاتورة مبيعات " + cmbMethodID.Text.Trim().ToString();
        //        rptForm.Parameters["VATCOMPANY"].Value = MySession.VAtCompnyGlobal;
        //        rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
        //        rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
        //        rptForm.Parameters["VatID"].Value = "";
        //        rptForm.Parameters["footer"].Value = MySession.footer;
        //        rptForm.Parameters["Notes"].Value = txtNotes.Text.Trim().ToString();
        //        rptForm.Parameters["CustomerMobile"].Value = txtCustomerMobile.Text.ToString();
        //        string Date = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
        //        int year = Convert.ToInt32(Date.Substring(0, 4));
        //        int month = Convert.ToInt32(Date.Substring(4, 2));
        //        int day = Convert.ToInt32(Date.Substring(6, 2));
        //        DateTime tempDate = new DateTime(year, month, day);
        //        rptForm.Parameters["HDate"].Value = Comon.ConvertFromEngDateToHijriDate(tempDate).Substring(0, 10);
        //        rptForm.Parameters["NumbToWord"].Value = Lip.ToWords(Convert.ToDecimal(lblNetBalance.Text.Trim().ToString()), 2);

        //        /********Total*********/
        //        rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotalBeforeDiscount.Text.Trim().ToString();
        //        rptForm.Parameters["UnitDiscount"].Value = lblUnitDiscount.Text.Trim().ToString();
        //        rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
        //        rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
        //        rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
        //        rptForm.Parameters["Tel"].Value = txtTel.Text.Trim().ToString();
        //        rptForm.Parameters["Mobile"].Value = txtCustomerMobile.Text.ToString(); ;
        //        for (int i = 0; i < rptForm.Parameters.Count; i++)
        //            rptForm.Parameters[i].Visible = false;
        //        /********************** Details ****************************/
        //        var dataTable = new dsReports.rptSalesInvoiceDataTable();

        //        for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
        //        {
        //            var row = dataTable.NewRow();
        //            row["ItemName"] = gridView1.GetRowCellValue(i, "ArbItemName").ToString() + " - " + gridView1.GetRowCellValue(i, "Description").ToString(); ;
                 
                      
                     
        //            row["#"] = i + 1;
        //            row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();

        //            row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
        //            row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
        //            row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
        //            row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
        //            row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
        //            row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
        //            row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
        //            row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
        //            row["Bones"] = gridView1.GetRowCellValue(i, "Bones").ToString();
        //            row["ExpiryDate"] = Comon.ConvertSerialToDate(Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString()).ToString());
        //            dataTable.Rows.Add(row);
        //        }
        //        rptForm.DataSource = dataTable;


        //        rptForm.DataMember = ReportName;
        //        XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
        //        subreport.Visible = true;
        //        /******************** Report Binding ************************/
               
        //        subreport.ReportSource = ReportComponent.CompanyHeader();
                
        //        rptForm.ShowPrintStatusDialog = false;
        //        rptForm.ShowPrintMarginsWarning = false;
        //        rptForm.CreateDocument();

        //        SplashScreenManager.CloseForm(false);
        //        ShowReportInReportViewer = true;
        //        if (ShowReportInReportViewer)
        //        {
        //            frmReportViewer frmRptViewer = new frmReportViewer();
        //            frmRptViewer.documentViewer1.DocumentSource = rptForm;
        //            frmRptViewer.ShowDialog();
        //        }
        //        else
        //        {
        //            bool IsSelectedPrinter = false;
        //            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
        //            DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
        //            if (dt.Rows.Count > 0)
        //                for (int i = 1; i < 6; i++)
        //                {
        //                    string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
        //                    if (!string.IsNullOrEmpty(PrinterName))
        //                    {
        //                        rptForm.PrinterName = PrinterName;
        //                        rptForm.Print(PrinterName);
        //                        IsSelectedPrinter = true;
        //                    }
        //                }
        //            SplashScreenManager.CloseForm(false);
        //            if (!IsSelectedPrinter)
        //                Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
        //        }

        //        if (MySession.PrintBuildPill == 1)
        //        {
        //            PrintBill();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);
        //        Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        //    }

        //}
        private void PrintBill()
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
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                rptFormName = "rptSalesInvoiceArbBuild";
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["RemaindAmount"].Value = lblRemaindAmount.Text.Trim().ToString();
                rptForm.Parameters["PaidAmount"].Value = txtPaidAmount.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value = lblSupplierName.Text.ToString();


                
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

                rptForm.Parameters["MethodName"].Value = "فاتورة تركيب ";// +cmbMethodID.Text.Trim().ToString();
                rptForm.Parameters["VATCOMPANY"].Value = MySession.VAtCompnyGlobal;
                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text.Trim().ToString();
                rptForm.Parameters["footer"].Value = MySession.footer;
                rptForm.Parameters["Notes"].Value = txtNotes.Text.Trim().ToString();

                rptForm.Parameters["CustomerMobile"].Value = txtCustomerMobile.Text.ToString();
                string Date = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
                int year = Convert.ToInt32(Date.Substring(0, 4));
                int month = Convert.ToInt32(Date.Substring(4, 2));
                int day = Convert.ToInt32(Date.Substring(6, 2));
                DateTime tempDate = new DateTime(year, month, day);
                rptForm.Parameters["HDate"].Value = Comon.ConvertFromEngDateToHijriDate(tempDate).Substring(0, 10);
                rptForm.Parameters["NumbToWord"].Value = Lip.ToWords(Convert.ToDecimal(lblNetBalance.Text.Trim().ToString()), 2);

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
                    row["ItemName"] = gridView1.GetRowCellValue(i, "ArbItemName").ToString();
                    if (Comon.cInt(cmbLanguagePrint.EditValue) == 2)
                        row["ItemName"] = gridView1.GetRowCellValue(i, "EngItemName").ToString();
                    else if (Comon.cInt(cmbLanguagePrint.EditValue) == 3)
                        row["ItemName"] = gridView1.GetRowCellValue(i, "EngItemName").ToString() + gridView1.GetRowCellValue(i, "ArbItemName").ToString();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();

                    row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                    row["Bones"] = gridView1.GetRowCellValue(i, "Bones").ToString();
                    row["ExpiryDate"] = DateTime.Now.ToShortDateString();
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = true;
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
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
      
        /*
        /************************Event From **************************/
        private void frmSaleInvoice_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3)
                Find();
              if (e.KeyCode == Keys.F2)
                ShortcutOpen();
        }
        /*******************Event CheckBoc***************************/
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
            decimal SalePriceRow = 0;
            decimal TotalCostRow = 0;
            decimal TotalSaletRow = 0;

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
            dtItem.Columns.Add("STONE_W", System.Type.GetType("System.Decimal"));
              dtItem.Columns.Add("CaratPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Height", System.Type.GetType("System.Decimal"));
           if (comboTypeInvoice.SelectedIndex ==0)
           {
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {

                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem.Rows[i]["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["STONE_W"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();
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
                dtItem.Rows[i]["ExpiryDateStr"] =  "0";
          
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView1.GetRowCellValue(i, "HavVat").ToString());
                dtItem.Rows[i]["HavVat"] = chkForVat.Checked;
                dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                dtItem.Rows[i]["Cancel"] = 0;
                SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                CostPriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                TotalCostRow = CostPriceRow * QTYRow;
                TotalSaletRow = SalePriceRow * QTYRow;
                if (chkForVat.Checked == true)
                {

                    if(chkForVatCostOnly.Checked==true)
                    AdditionalAmountRow = (TotalCostRow - DiscountRow) / 100 * MySession.GlobalPercentVat;
                    else
                        AdditionalAmountRow = (TotalCostRow + TotalSaletRow - DiscountRow) / 100 * MySession.GlobalPercentVat;

                    NetRow = Comon.ConvertToDecimalPrice((TotalSaletRow + TotalCostRow - DiscountRow) + AdditionalAmountRow);
                    dtItem.Rows[i]["AdditionalValue"] = AdditionalAmountRow.ToString("N" + MySession.GlobalPriceDigits);
                    dtItem.Rows[i]["Net"] = NetRow.ToString("N" + MySession.GlobalPriceDigits);

                    AdditionalAmount += AdditionalAmountRow;
                    DiscountTotal += DiscountRow;
                    Total += TotalSaletRow + TotalCostRow;
                    Net += NetRow;

                }
                else
                {
                    AdditionalAmountRow = 0;
                    //NetRow = TotalCostRow - DiscountRow;
                    NetRow = (TotalSaletRow + TotalCostRow - DiscountRow);
                    dtItem.Rows[i]["AdditionalValue"] = 0;
                    dtItem.Rows[i]["Net"] = NetRow.ToString("N" + MySession.GlobalPriceDigits);

                    AdditionalAmountRow = 0;
                    DiscountTotal += DiscountRow;
                    Total += TotalCostRow;
                    Net += NetRow;
                }
            }
            gridView1.Columns["HavVat"].OptionsColumn.ReadOnly = !chkForVat.Checked;
            gridControl.DataSource = dtItem;
           }
                else if (comboTypeInvoice.SelectedIndex == 1)
            {
                dtItem.Columns.Add(GroupName, System.Type.GetType("System.String"));
                dtItem.Columns.Add("GroupID", System.Type.GetType("System.Decimal"));
                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    dtItem.Rows.Add();
                    dtItem.Rows[i]["ID"] = i;
                    dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; 
                    dtItem.Rows[i]["BarCode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                    dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView2.GetRowCellValue(i, "ItemID").ToString());
                    dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView2.GetRowCellValue(i, "SizeID").ToString());
                    
                    dtItem.Rows[i][ItemName] = gridView2.GetRowCellValue(i, ItemName).ToString();
                    dtItem.Rows[i][SizeName] = gridView2.GetRowCellValue(i, SizeName).ToString();
                    dtItem.Rows[i][GroupName] = gridView2.GetRowCellValue(i, GroupName).ToString();
                    dtItem.Rows[i]["GroupID"] = gridView2.GetRowCellValue(i, "GroupID").ToString();
                    dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                    dtItem.Rows[i]["STONE_W"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "STONE_W").ToString());

                    dtItem.Rows[i]["Bones"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Bones").ToString());
                    dtItem.Rows[i]["Description"] = gridView2.GetRowCellValue(i, "Description").ToString();
                    dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView2.GetRowCellValue(i, "StoreID").ToString());
                    dtItem.Rows[i]["Discount"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount").ToString());
                    dtItem.Rows[i]["ExpiryDateStr"] = "0";
                    dtItem.Rows[i]["ExpiryDate"] = DateTime.Now.ToShortDateString();
                    dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());
                    dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView2.GetRowCellValue(i, "HavVat").ToString());
                    dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Total").ToString());
                    dtItem.Rows[i]["AdditionalValue"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "AdditionalValue").ToString());
                    dtItem.Rows[i]["Net"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Net").ToString());
                    //dtItem.Rows[i]["Height"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Height").ToString());
                    //dtItem.Rows[i]["Width"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Width").ToString());
                    dtItem.Rows[i]["SalePrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                    dtItem.Rows[i]["CaratPrice"] = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CaratPrice").ToString());
                    dtItem.Rows[i]["Cancel"] = 0;
               
                
                     SalePriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "SalePrice").ToString());
                CostPriceRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "CostPrice").ToString());
                QTYRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "QTY").ToString());
                DiscountRow = Comon.ConvertToDecimalPrice(gridView2.GetRowCellValue(i, "Discount").ToString());
                TotalCostRow = CostPriceRow  ;
                TotalSaletRow = SalePriceRow  ;
                if (chkForVat.Checked == true)
                {

                    if(chkForVatCostOnly.Checked==true)
                    AdditionalAmountRow = (TotalCostRow - DiscountRow) / 100 * MySession.GlobalPercentVat;
                    else
                        AdditionalAmountRow = (TotalCostRow   - DiscountRow) / 100 * MySession.GlobalPercentVat;

                    NetRow = Comon.ConvertToDecimalPrice(( TotalCostRow - DiscountRow) + AdditionalAmountRow);
                    dtItem.Rows[i]["AdditionalValue"] = AdditionalAmountRow.ToString("N" + MySession.GlobalPriceDigits);
                    dtItem.Rows[i]["Net"] = NetRow.ToString("N" + MySession.GlobalPriceDigits);

                    AdditionalAmount += AdditionalAmountRow;
                    DiscountTotal += DiscountRow;
                    Total +=  TotalCostRow;
                    Net += NetRow;

                }
                else
                {
                    AdditionalAmountRow = 0;
                    //NetRow = TotalCostRow - DiscountRow;
                    NetRow = (  TotalCostRow - DiscountRow);
                    dtItem.Rows[i]["AdditionalValue"] = 0;
                    dtItem.Rows[i]["Net"] = NetRow.ToString("N" + MySession.GlobalPriceDigits);

                    AdditionalAmountRow = 0;
                    DiscountTotal += DiscountRow;
                    Total += TotalCostRow;
                    Net += NetRow;
                }
                }
                gridControl1.DataSource = dtItem;
            }
            DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
            lblAdditionaAmmount.Text = AdditionalAmount.ToString("N" + MySession.GlobalPriceDigits);
            lblNetBalance.Text = Net.ToString("N" + MySession.GlobalPriceDigits);
         
            // CalculateRow();
            //gridView1.Focus();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
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
                lblCheckID.Visible = false;
                txtCheckID.Visible = false;
                txtNetProcessID.Text = "";
                txtCheckID.Text = "";
                txtNetAmount.Text = "";
              
                cmbNetType.ItemIndex = -1;
                txtWarningDate.EditValue = DateTime.Now;
                txtCheckSpendDate.EditValue = DateTime.Now;
                txtNetAmount.Visible = false;
                lblNetAmount.Visible = false;
                lblnetType.Visible = false;
                cmbNetType.Visible = false;
              
                txtCheckID.Tag = "IsNumber";
                cmbBank.Tag = " ";
                txtNetProcessID.Tag = "IsNumber";
                txtNetAmount.Tag = "IsNumber";
                if (value == 1)
                {
                    // حساب الصندوق
                    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
                    if (row.Length > 0)
                    {

                        lblCreditAccountID.Text = row[0]["AccountID"].ToString();
                        lblCreditAccountName.Text = row[0]["AccountName"].ToString();

                    }
                    lblSupplierName.Text = "";
                    txtSupplierID.Text = "";

                    cmbNetType.Tag = ""; 

                   
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    // txtCustomerName.Focus();
                }
                else if (value == 2)
                {
                   
                   
                    txtSupplierID.Visible = true;
                    lblSupplierName.Visible = true;

                    lblSupplierName.Text ="";
                    txtSupplierID.Text = "";
                    cmbNetType.Tag = ""; 
                    
                    lblCheckSpendDate.Visible = true;
                    txtCheckSpendDate.Visible = true;
                    txtWarningDate.Visible = true;
                    lblWarningDate.Visible = true;
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    if (StopSomeCode == false)
                    {
                       
                    }
                    
                }
                else if (value == 3)
                {
                    // حساب الشبكة 
                    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'NetAccount'");
                    if (row.Length > 0)
                    {
                        
                        lblCreditAccountID.Text = row[0]["AccountID"].ToString();
                        lblCreditAccountName.Text = row[0]["AccountName"].ToString();
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
                    //  txtNetProcessID.Tag = "ImportantFieldGreaterThanZero";
                    //  txtNetAmount.Tag = "ImportantFieldGreaterThanZero";
                    cmbNetType.Tag = "ImportantField";
                    cmbNetType.EditValue = Comon.cDbl(MySession.GlobalDefaultPurchaseNetTypeID);
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
                    lblCheckID.Visible = true;
                    txtCheckID.Visible = true;
                    lblBankName.Visible = true;
                    cmbBank.Visible = true;
                    cmbBank.Tag = "ImportantField";
                    cmbNetType.Tag = ""; 
                    cmbBank.EditValue = Comon.cDbl(lblDebitAccountID.Text);
                }
                else if (value == 5)
                {
                    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'PurchaseAccount'");
                    if (row.Length > 0)
                    {

                        lblDebitAccountID.Text = row[0]["AccountID"].ToString();
                        lblDebitAccountName.Text = row[0]["AccountName"].ToString();
                        chkForVat.Checked = true;
                    }
                    lblNetProcessID.Visible = true;
                    txtNetProcessID.Visible = true;
                    txtNetAmount.Visible = true;
                    lblNetAmount.Visible = true;
                    lblnetType.Visible = true;
                    cmbNetType.Visible = true;
                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    cmbNetType.EditValue = Comon.cDbl(MySession.GlobalDefaultPurchaseNetTypeID);
                    txtNetProcessID.Tag = " ";
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
           // ribbonControl1.Visible = false;
        }
        #endregion
        private void frmSalesInvoice_Load(object sender, EventArgs e)
        {
           
            DoNew();
            simpleButton1_Click(null, null);
            txtInvoiceDate.BringToFront();
            txtInvoiceID.BringToFront();
            comboTypeInvoice.SelectedIndex = 0;
            if (UserInfo.ID == 1)
            {
                cmbBranchesID.Visible = true;
                labelControl47.Visible = true;
               
            }
           else
            {
                cmbBranchesID.Visible = false;
                labelControl47.Visible = false;
            }
            //dVat = Lip.SelectRecord(VAt);

        }
        private void button1_Click(object sender, EventArgs e)
        {

            
            if (dt.Rows.Count < 1)
                return;
            strSQL = "Select * from Sales_PurchaseInvoiceReturnMaster where SupplierInvoiceID=" + txtInvoiceID.Text + " And BranchID=" + MySession.GlobalBranchID+" And Cancel=0";
            DataTable dtReturn = new DataTable();
            dtReturn = Lip.SelectRecord(strSQL);
            if (dtReturn.Rows.Count > 0)
            {
                Messages.MsgError(Messages.TitleError, " يوجد فاتورة مردودات سابقة لهذه الفاتورة");
                frmCashierPurchaseReturnGold frm = new frmCashierPurchaseReturnGold();
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
                frm.chkForVat.Checked = chkForVat.Checked;
                frm.lbl18.Text = lbl18.Text;
                frm.lbl24.Text = lbl24.Text;
                frm.lbl22.Text = lbl22.Text;
                frm.lbl21.Text = lbl21.Text;
                frm.lblInvoiceTotalGold.Text = lblInvoiceTotalGold.Text;

            }
            else
            {
                frmCashierPurchaseReturnGold frm = new frmCashierPurchaseReturnGold();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.IsNewRecord = true;
                }
                frm.fillMAsterData(dt);
                frm.lblInvoiceTotalBeforeDiscount.Text = lblInvoiceTotalBeforeDiscount.Text;
                frm.lblNetBalance.Text = lblNetBalance.Text;
                frm.lblAdditionaAmmount.Text = lblAdditionaAmmount.Text;
                frm.txtSupplierInvoiceID.Text = txtInvoiceID.Text;
                frm.cmbBranchesID.EditValue = cmbBranchesID.EditValue;
                frm.lbl18.Text = lbl18.Text;
                frm.lbl24.Text = lbl24.Text;
                frm.lbl22.Text = lbl22.Text;
                frm.lbl21.Text = lbl21.Text;
                frm.lblInvoiceTotalGold.Text = lblInvoiceTotalGold.Text;
                frm.chkForVat.Checked = chkForVat.Checked;
                //frm.txtwhghtGold.Text = txtwhghtGold.Text;

            }
        }
        private void cmbMethodID_EditValueChanged_1(object sender, EventArgs e)
        {

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
            
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
           // showCustomers(false,0);
            
            labelControl6.Visible = true;
            txtVatID.Visible = true;
            labelControl4.Visible = true;
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

        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            /////////////////////////////
           
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            //showCustomers(false,0);
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

        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
           // showCustomers(false,0);
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

        private void frmCashierSales_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.F6)
            { 
                DoSave();
            }
            else if (e.KeyCode == Keys.F6)
               simpleButton1_Click(null, null);
            else if (e.KeyCode == Keys.F7)
                simpleButton2_Click(null, null);
            else if (e.KeyCode == Keys.F8)
                  simpleButton3_Click(null, null);

            if (e.KeyCode == Keys.F9)
            {
                falgPrint=true;
            }

            if (e.KeyCode == Keys.F12)
            {
                btnSendToServer.Visible = true;
            }
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {
           
        }

        private void txtInvoiceDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
                txtInvoiceDate.EditValue = DateTime.Now;
            //if (Comon.ConvertDateToSerial(txtInvoiceDate.Text) < Comon.cLong((Lip.GetServerDateSerial())))
            //    txtInvoiceDate.Text = Lip.GetServerDate();
            //if (Comon.ConvertDateToSerial(txtInvoiceDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
            //    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgTheDateIsGreaterThanToday);

        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            /////////////////////////////
           
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////
            showCustomers(true,1);
            cmbMethodID.EditValue = 2;
            simpleButton12.Appearance.BackColor = Color.Goldenrod;
            simpleButton12.Appearance.BackColor2 = Color.White;
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            simpleButton12.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "آجل" : "Future");
            MethodID = 4;
            txtSupplierID.Visible = true;
            lblSupplierName.Visible = true;
            txtSupplierID.Focus();
            Find();
            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
          
        }

        private void showCustomers(bool p,int f )
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

        private void txtDailyID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
            {
                txtInvoiceID.Text = Lip.GetValue("Select InvoiceID from Sales_SalesInvoiceMaster where DailyID=" + txtDailyID.Text + " And CostCenterID=" + txtCostCenterID.Text);
                ReadRecord(Comon.cLong(txtInvoiceID.Text));
            }
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }

        public void btnUsengGold_Click(object sender, EventArgs e)
        {
            chkForVat.Checked = true;
            GoldUsing = 2;
            chForVat_EditValueChanged(null, null);
        }

        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtSupplierID.Text != string.Empty && txtSupplierID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtSupplierID.Text  ;
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
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtSupplierID.Text  ;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {
                            lblDebitAccountName.Text = dt.Rows[0]["CustomerName"].ToString();
                            lblDebitAccountID.Text = txtSupplierID.Text;
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
        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            txtCostCenterID.Text = "";
            lblCostCenterName.Text = "";
            txtInvoiceID.Text = Sales_PurchaseInvoicesDAL.GetNewID(MySession.GlobalFacilityID, Comon.cInt(cmbBranchesID.EditValue), MySession.UserID).ToString();
            txtRegistrationNo.Text = txtInvoiceID.Text;
            txtStoreID.Text = Lip.GetValue("Select StoreID from Stc_Stores where BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            txtStoreID_Validating(null, null);


            txtCostCenterID.Text = Lip.GetValue("Select CostCenterID from Acc_CostCenters where BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            txtCostCenterID_Validating(null, null);

             


        }
        private void chkForVatCostOnly_CheckedChanged(object sender, EventArgs e)
        {
            chForVat_EditValueChanged(null, null);
        }

        private void btnSendToServer_Click(object sender, EventArgs e)
        {
             
             
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
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1 || Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;
                if (chkForVat.Checked)
                returned.Credit = Comon.cDbl(lblNetBalance.Text);
                else
                    returned.Credit = Comon.cDbl( lblInvoiceTotal.Text);

                returned.Debit = 0;
                returned.DebitGold = 0;
                returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);
                
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
                if (chkForVat.Checked)
                    returned.Credit = Comon.cDbl(lblNetBalance.Text);
                else
                    returned.Credit = Comon.cDbl(lblInvoiceTotal.Text);
                returned.Debit = 0;
                returned.DebitGold = 0;
                returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text); 
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }

            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 4)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblChequeAccountID.Text);
                returned.VoucherID = VoucherID;
                if (chkForVat.Checked)
                    returned.Credit = Comon.cDbl(lblNetBalance.Text);
                else
                    returned.Credit = Comon.cDbl(lblInvoiceTotal.Text);
                returned.Debit = 0;
                returned.DebitGold = 0;
                returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text); 
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);

            }
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 5)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;

                if (chkForVat.Checked)
                {
                    returned.Credit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(txtNetAmount.Text));
                    returned.CreditGold = Comon.cDbl(Comon.cDbl(returned.Credit / Comon.cDbl(lblNetBalance.Text)) * Comon.cDbl(lblInvoiceTotalGold.Text));
                }
                else
                {
                    returned.Credit = Comon.cDbl(Comon.cDbl(lblInvoiceTotal.Text) - Comon.cDbl(txtNetAmount.Text));
                    returned.CreditGold = Comon.cDbl(Comon.cDbl(returned.Credit / Comon.cDbl(lblInvoiceTotal.Text)) * Comon.cDbl(lblInvoiceTotalGold.Text)); 
                }
                returned.Debit = 0;
                returned.DebitGold = 0;
     
               
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

                    returned.Debit = 0;
                    returned.DebitGold = 0;
                    if (chkForVat.Checked)
                      returned.CreditGold = Comon.cDbl(Comon.cDbl(returned.Credit / Comon.cDbl(lblNetBalance.Text)) * Comon.cDbl(lblInvoiceTotalGold.Text)); 
                    else
                        returned.CreditGold = Comon.cDbl(Comon.cDbl(returned.Credit / Comon.cDbl(lblInvoiceTotal.Text)) * Comon.cDbl(lblInvoiceTotalGold.Text)); 

                   
                
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
                returned.Debit = 0;
                returned.DebitGold = 0;
                returned.CreditGold = 0;
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }
            //Debit Purchase
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 3;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;
            returned.DebitGold = 0;
            returned.CreditGold = 0;
            returned.Debit = Comon.cDbl(lblInvoiceTotal.Text);
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
                returned.Credit = 0;
                returned.DebitGold = 0;
                returned.CreditGold = 0;
                returned.Debit = Comon.cDbl(lblAdditionaAmmount.Text);
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }

            //Debit Gold
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 5;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtDebitGoldAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;
            returned.DebitGold = Comon.cDbl(lblInvoiceTotalGold.Text);
            returned.CreditGold = 0;
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
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1 || Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;
                if (chkForVat.Checked)
                    returned.Credit = Comon.cDbl(lblNetBalance.Text);
                else
                    returned.Credit = Comon.cDbl(lblInvoiceTotal.Text);

                returned.Debit = 0; 

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
                if (chkForVat.Checked)
                    returned.Credit = Comon.cDbl(lblNetBalance.Text);
                else
                    returned.Credit = Comon.cDbl(lblInvoiceTotal.Text);
                returned.Debit = 0; 
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }

            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 4)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblChequeAccountID.Text);
                returned.VoucherID = VoucherID;
                if (chkForVat.Checked)
                    returned.Credit = Comon.cDbl(lblNetBalance.Text);
                else
                    returned.Credit = Comon.cDbl(lblInvoiceTotal.Text);
                returned.Debit = 0; 
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);

            }
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 5)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;

                if (chkForVat.Checked)
                {
                    returned.Credit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(txtNetAmount.Text)); 
                }
                else
                {
                    returned.Credit = Comon.cDbl(Comon.cDbl(lblInvoiceTotal.Text) - Comon.cDbl(txtNetAmount.Text)); 
                }
                returned.Debit = 0;
                returned.DebitGold = 0;


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
                    returned.Debit = 0;
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
                returned.Debit = 0; 
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }
            //Debit Purchase
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 3;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0; 
            returned.Debit = Comon.cDbl(lblInvoiceTotal.Text);
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
                returned.Credit = 0;
                returned.DebitGold = 0;
                returned.CreditGold = 0;
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


            strSQL = "Select * from " + Sales_PurchaseInvoicesDAL.TableName + " where cancel=0 and GoldUsing="+GoldUsing;
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
                     long VoucherID=0;
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

        private void comboTypeInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTypeInvoice.SelectedIndex == 0)
            {
                xtraTabControl1.SelectedTabPageIndex = 0;
                btnprintBarcode.Visible = false;
                btnemport.Visible = false;
                
            }
            else if (comboTypeInvoice.SelectedIndex == 1)
            {
                xtraTabControl1.SelectedTabPageIndex = 1;
                btnprintBarcode.Visible = true;
                btnemport.Visible = true;

            }

        }

        private void btnprintBarcode_Click(object sender, EventArgs e)
        {
            if (gridView2.DataRowCount > 0)
            {
                frmPrintItemSticker frm = new frmPrintItemSticker();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    BindingSource bs = new BindingSource();
                    bs.DataSource = gridControl1.DataSource;
                    frm.Show();
                    frm.gridControl.DataSource = bs;

                }
                else
                    frm.Dispose();
            }
            else
                Messages.MsgError("Error Print BarCode", "لا يوجد اصناف لطباعة الباركود الخاص بها.. الرجاء اضافة اصناف .");
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
                    btnemport.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
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
            lstDetail2 = new BindingList<Sales_SalesInvoiceDetails>();
            lstDetail2.AllowNew = true;
            lstDetail2.AllowEdit = true;
            lstDetail2.AllowRemove = true;
            gridControl1.DataSource = lstDetail;

            Sales_SalesInvoiceDetails obj = new Sales_SalesInvoiceDetails();


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                obj = new Sales_SalesInvoiceDetails();
                obj.ArbItemName = dt.Rows[i]["ITEM_NAME"].ToString();
                obj.EngItemName = dt.Rows[i]["ITEM_NAME"].ToString();
                obj.GroupID = Comon.cInt(dt.Rows[i]["GroupID"].ToString());
               
                obj.CostPrice = Comon.ConvertToDecimalPrice(dt.Rows[i]["price"].ToString());
                obj.SalePrice = Comon.ConvertToDecimalPrice(dt.Rows[i]["SalesPrice"].ToString());
                obj.QTY = Comon.ConvertToDecimalPrice(dt.Rows[i]["GOLD_GRAM_W"].ToString());
                obj.Caliber = Comon.cInt(dt.Rows[i]["GOLD_CALIBER"].ToString());
                obj.STONE_W = Comon.cDec(dt.Rows[i]["STONE_W"].ToString());
                obj.Serials = dt.Rows[i]["ITEM_NO"].ToString();
                obj.BarCode = dt.Rows[i]["BarCode"].ToString();
               
                string Barcode = Lip.GetValue("select itemid from Sales_PurchaseInvoiceDetails Where Barcode='" + obj.BarCode + "'");
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

                decimal additonalVAlue = 0;
                if (chkForVat.Checked)
                    additonalVAlue = Comon.ConvertToDecimalPrice((CostPrice * MySession.GlobalPercentVat) / 100);

                //سعر تكلفة مع مصاريف
                decimal SpendPrice = Comon.ConvertToDecimalPrice(CostPrice);
                //سعر تكلفة المحل
                decimal CaratPrice = Comon.ConvertToDecimalPrice(Comon.cDec(obj.SalePrice));
                //سعر الكارت وهو البيع
                decimal SalePrice = Comon.ConvertToDecimalPrice(obj.SalePrice);

                obj.AdditionalValue = additonalVAlue;
                obj.SalePrice = SalePrice;
                obj.SpendPrice = SpendPrice;
                obj.CaratPrice = CaratPrice;
                obj.Total = CostPrice;
                obj.Net = SpendPrice;
                obj.ArbGroupName = Lip.GetValue("Select  ArbName from Stc_ItemsGroups Where GroupID=" + obj.GroupID);
                obj.EngGroupName = obj.ArbGroupName;


                obj.ArbSizeName = obj.Caliber.ToString();
                obj.EngSizeName = obj.Caliber.ToString();


                lstDetail2.Add(obj);

            }
            SumTotalBalanceAndDiscountread();

            gridControl1.DataSource = lstDetail2;
            comboTypeInvoice.SelectedIndex = 1;
            SplashScreenManager.CloseForm(false);
        }

        private void labelControl47_Click(object sender, EventArgs e)
        {

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

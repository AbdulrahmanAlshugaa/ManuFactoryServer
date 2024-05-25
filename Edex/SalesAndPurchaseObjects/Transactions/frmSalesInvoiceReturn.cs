using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.DAL.Accounting;
using Edex.DAL.SalseSystem;
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
//using Edex.StockObjects.Codes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Edex.AccountsObjects.Codes;
using Edex.StockObjects.Codes;
using DevExpress.DataAccess;
using DAL;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.StockObjects.Transactions;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmSalesInvoiceReturn : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        public  int GoldUsing;
        public const int DocumentType = 7;

        DataTable dtDeclaration;
        DataTable dtSize;
        int rowIndex;
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


        public bool IsNewRecord;
        private Sales_SaleInvoicesReturnDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;
        OpenFileDialog OpenFileDialog1 = null;
        DataTable dt = new DataTable();
        GridViewMenu menu;
        //all record master and detail
        BindingList<Sales_SalesInvoiceReturnDetails> AllRecords = new BindingList<Sales_SalesInvoiceReturnDetails>();

        //list detail
        BindingList<Sales_SalesInvoiceReturnDetails> lstDetail = new BindingList<Sales_SalesInvoiceReturnDetails>();

        //Detail
        Sales_SalesInvoiceReturnDetails BoDetail = new Sales_SalesInvoiceReturnDetails();

        #endregion
        public frmSalesInvoiceReturn()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                 CaptionSizeName = "الوحدة";
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
                lblNetBalance.ForeColor = Color.Black;
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
                if (MySession.GlobalHaveVat != "1")
                {
                    labelControl21.Visible = false;
                    lblAdditionaAmmount.Visible = false;
                    lblAdditionalAccountName.Visible = false;
                    lblAdditionalAccountID.Visible = false;
                    labelControl33.Visible = false;
                    lblAdditionalAccountID.Tag = "isNumber";
                    chkForVat.Checked = false;
                    chkForVat.Visible = false;
                }

                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
                FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

               // FillCombo.FillComboBox(StoreID, "Stc_Stores", "AccountID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbBank, "[Acc_Banks]", "ID", PrimaryName, "", " 1=1 ", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                /***********************Component ReadOnly  ****************************/
                if (MySession.GlobalHaveVat == "1")
                FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "ID>1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
                else
                    FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));

                cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
                /*********************** Date Format dd/MM/yyyy ****************************/
                InitializeFormatDate(txtInvoiceDate);
                InitializeFormatDate(txtWarningDate);
                InitializeFormatDate(txtCheckSpendDate);

                /************************  Form Printing ***************************************/
                 
                /*********************** Roles From ****************************/
                txtInvoiceDate.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnInvoiceDate;
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnStoreID;
                comCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnCostCenterID;
                cmbMethodID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnPayMethodID;
                cmbNetType.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnNetTypeID;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnCurencyID;
                txtCustomerID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnCustomerID;
                txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnDelegateID;
                /************TextEdit Account ID ***************/
                lblDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnDebitAccountID;
                lblCreditAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnCreditAccountID;
                lblAdditionalAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnAdditionalAccountID;
                lblChequeAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnChequeAccountID;
                lblDiscountDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnDiscountDebitAccountID;
                lblNetAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleReturnNetAccountID;
                /************ Button Search Account ID ***************/

                /********************* Event For Account Component ****************************/
                this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);
                this.lblDebitAccountName.Click += new System.EventHandler(this.btnDebitSearch_Click);
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


                this.txtCostSalseAccountID.Validating += txtCostSalseID_Validating;
                this.txtSalesRevenueAccountID.Validating += txtSalesRevenueID_Validating;
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
                this.txtCustomerID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCheckID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetProcessID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtNetAmount.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                this.cmbMethodID.EditValueChanged += new System.EventHandler(this.cmbMethodID_EditValueChanged);
                this.cmbNetType.EditValueChanged += new System.EventHandler(this.cmbNetType_EditValueChanged);

                this.cmbBank.EditValueChanged += new System.EventHandler(this.cmbBank_EditValueChanged);


                //this.chkForVat.EditValueChanged += new System.EventHandler(this.chForVat_EditValueChanged);

                this.txtDiscountOnTotal.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountOnTotal_Validating);
                this.txtDiscountPercent.Validating += new System.ComponentModel.CancelEventHandler(this.txtDiscountPercent_Validating);
                this.txtInvoiceID.Validating += new System.ComponentModel.CancelEventHandler(this.txtInvoiceID_Validating);
                this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
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
                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);
                ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                if (MySession.GlobalHaveVat == "1")
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
        

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
      public  void txtCostSalseID_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
            DataRow[] row = dt.Select("AccountID=" + txtCostSalseAccountID.Text);
            if (Comon.cInt(row.Length) > 0)
                lblCostSalseAccountName.Text = row[0]["ArbName"].ToString();

        }

      public  void txtSalesRevenueID_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
            DataRow[] row = dt.Select("AccountID=" + txtSalesRevenueAccountID.Text);
            if (Comon.cInt(row.Length) > 0)
                lblSalesRevenueAccountName.Text = row[0]["ArbName"].ToString();

        }
        #region GridView
        void InitGrid()
        {
            lstDetail = new BindingList<Sales_SalesInvoiceReturnDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/
            gridView1.Columns["BranchID"].Visible = false;
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
            gridView1.Columns["Serials"].Visible = false;
            gridView1.Columns["InvoiceID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["StoreID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["SaleReturnMaster"].Visible = false;

            gridView1.Columns["CaratPrice"].Visible = false;
            gridView1.Columns["SpendPrice"].Visible = false;
            gridView1.Columns["GroupID"].Visible = false;
            gridView1.Columns["ArbGroupName"].Visible = false;
            gridView1.Columns["EngGroupName"].Visible = false;
            gridView1.Columns["Total"].Visible = false;
            if (MySession.GlobalHaveVat != "1")
                gridView1.Columns["AdditionalValue"].Visible = false;
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;

            gridView1.Columns["ExpiryDate"].Visible = MySession.GlobalAllowUsingDateItems;
            gridView1.Columns["Description"].Visible = true;
            /******************* Columns Visible=true *******************/
            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;
            gridView1.Columns["SizeID"].Visible = false;
            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 120;
            gridView1.Columns["Description"].Width = 150;
            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["ExpiryDate"].Visible = false;
            gridView1.Columns["QTY"].Caption = CaptionQTY;
          
            gridView1.Columns["Total"].Caption = CaptionTotal;
            gridView1.Columns["Discount"].Caption = CaptionDiscount;
            gridView1.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView1.Columns["AdditionalValue"].Visible=false;
            gridView1.Columns["Net"].Caption = CaptionNet;
            gridView1.Columns["SalePrice"].Caption = CaptionSalePrice;
            gridView1.Columns["Description"].Caption = CaptionDescription;
            gridView1.Columns["HavVat"].Caption = CaptionHavVat;
            //gridView1.Columns["RemainQty"].Caption = CaptionRemainQty;
            gridView1.Focus();
            /*************************Columns Properties ****************************/

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

            gridView1.Columns["HavVat"].Visible = false;

            gridView1.Columns["STONE_W"].Caption = "وزن الأحجار";
            gridView1.Columns["BAGET_W"].Caption = "وزن الباجيت";
            gridView1.Columns["DIAMOND_W"].Caption = "وزن الألماس";

            gridView1.Columns["Color"].Caption = "اللون";
            gridView1.Columns["CLARITY"].Caption = "النقاء";
           

            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["CurrencyPrice"].Visible = false;
            gridView1.Columns["CurrencyName"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["CurrencyEquivalent"].VisibleIndex = gridView1.Columns["Net"].VisibleIndex + 1;
            DataTable dtCurrncy = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Acc_Currency where Cancel=0 ");
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
                gridView1.Columns["Caliber"].Caption = "Calipar";
                gridView1.Columns["CurrencyPrice"].Caption = "Currency Price  ";
                gridView1.Columns["CurrencyID"].Caption = "Currency ID  ";
                gridView1.Columns["CurrencyName"].Caption = "Currency Name";
                gridView1.Columns["CurrencyEquivalent"].Caption = "Currency Equivalent";
            }
            /************************ Look Up Edit **************************/
            // يتم عمله فيما بعد

            /************************ Auto Number **************************/
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
            ///
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
            gridView1.Columns["Color"].Visible = false;
            gridView1.Columns["CLARITY"].Visible = false;
        }
        private void item_Click(object sender, EventArgs e)
        {


        }

        private void Price_Click(object sender, EventArgs e)
        {
            //frmItemPricesAndCosts frm = new frmItemPricesAndCosts();
            //var ItemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID");
            //var SizeID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SizeID");
            //frm.SizeID = Comon.cInt(SizeID);
            //frm.ItemID = Comon.cLong(ItemID);
            //frm.CustomerID = Comon.cLong(txtCustomerID.Text);
            //frm.ShowDialog();
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], Comon.cDec(frm.CelValue));

        }

        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.HitInfo != null && e.HitInfo.Column.Name == "colSalePrice")
                if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
                    e.Menu = menu;
        }
        private void gridView1_ShownEditor(object sender, EventArgs e)
        {
            if (this.gridView1.ActiveEditor is CheckEdit)
                //if (chkForVat.Checked)
                //{
                //    GridView view = sender as GridView;
                //    view.ActiveEditor.IsModified = true;
                //    view.ActiveEditor.ReadOnly = false;
                //}
            HasColumnErrors = false;

            //CalculateRow();
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
                        else if (!(double.TryParse(val.ToString(), out num)))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.cDec(val.ToString()) <= 0)
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
                    else if (Comon.cDec(val.ToString()) <= 0)
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
                    if (ColName == "BarCode")
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
                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                            gridView1.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);

                            FileItemData(dt);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
                            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
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
                            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                            gridView1.Columns[SizeName].ColumnEdit = rSize;
                            gridControl.RepositoryItems.Add(rSize);
                            FileItemData(dt);
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
                if (ColName == "CurrencyPrice")
                {
                    if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net")) > 0)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net"))).ToString());

                }
                if (ColName == "CurrencyName")
                {
                    DataTable dt = Lip.SelectRecord("Select ID ,ExchangeRate from Acc_Currency Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "')");
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyID", dt.Rows[0]["ID"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice", dt.Rows[0]["ExchangeRate"]);
                    if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net")) > 0)
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net"))).ToString());
                }
                else if (ColName == "Discount")
                {
                    decimal QTY = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString());
                    decimal SalePrice = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SalePrice").ToString());
                    decimal Total = QTY * SalePrice;
                    decimal PercentDiscount = Total * (MySession.GlobalDiscountPercentOnItem / 100);
                    if (!(double.TryParse(val.ToString(), out num)))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.cDec(val.ToString()) > 0 && (MySession.GlobalDiscountPercentOnItem <= 0)) { Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount); return; }

                    else if (Comon.cDec(val.ToString()) > PercentDiscount)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNotAllowedPercentDiscount;
                    }
                }
                if (ColName == "QTY" || ColName == "SalePrice")
                {
                    decimal QTY = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString());
                    decimal SalePrice = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SalePrice").ToString());
                    decimal DiscountRow = Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount"));
                    decimal TotalRow = Comon.cDec(QTY * SalePrice - DiscountRow);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], TotalRow.ToString());
                }
                CalculateRow();
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
                            else if (Comon.cDec(cellValue.ToString()) <= 0)
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

            try
            {


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
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], dt.Rows[0]["CostPrice"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"], cmbCurency.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyEquivalent"], Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(txtCurrncyPrice.Text) * Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString())));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
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
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RemainQty"],   0);
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
            //chkForVat.Enabled = Value;

            //chkForVat.Properties.AppearanceDisabled.ForeColor = Color.Black;
            //chkForVat.Properties.AppearanceDisabled.BackColor = Color.Transparent;
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
                    //gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !chkForVat.Checked;
                }
            }

         
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
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired );
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
        public void CalculateRow(int Row = -1, bool IsHavVat = false)
        {
            try
            {
                SumTotalBalanceAndDiscount(Row, IsHavVat);
                //Remove Icon Validtion
                var Net = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net");
                var Total = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total");
                if ((Total != null && !(string.IsNullOrWhiteSpace(Total.ToString())) && Comon.cDec(Total.ToString()) > 0))
                    gridView1.SetColumnError(gridView1.Columns["Total"], "");
                if ((Net != null && !(string.IsNullOrWhiteSpace(Net.ToString())) && Comon.cDec(Net.ToString()) > 0))
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
                decimal totalqty = 0;

                decimal AdditionalAmountRow = 0;
                bool HavVatRow = false;
                decimal totalDiamond=0;
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {

                    QTYRow = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
                    SalePriceRow = Comon.cDec(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    DiscountRow = Comon.cDec(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));

                    TotalBeforeDiscountRow = Comon.cDec(QTYRow * SalePriceRow);
                    TotalRow = Comon.cDec(QTYRow * SalePriceRow - DiscountRow);
                     if (MySession.GlobalHaveVat == "1")
                        AdditionalAmountRow = HavVatRow == true ? Comon.cDec((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
                    NetRow = Comon.cDec(TotalRow + AdditionalAmountRow);

                    gridView1.SetRowCellValue(i, gridView1.Columns["Total"], TotalRow.ToString());
                   
                        gridView1.SetRowCellValue(i, gridView1.Columns["AdditionalValue"], AdditionalAmountRow.ToString());
                 

                    gridView1.SetRowCellValue(i, gridView1.Columns["Net"], NetRow.ToString());
                   
                    totalqty += QTYRow;
                    //totalDiamond+=Comon.cDec(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
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

                    QTYRow = ResultQTY != null ? Comon.cDec(ResultQTY.ToString()) : 0;
                    SalePriceRow = ResultSalePrice != null ? Comon.cDec(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.cDec(ResultDiscount.ToString()) : 0;
                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    HavVatRow = row == rowIndex ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(rowIndex, "HavVat"));

                    TotalBeforeDiscountRow = Comon.cDec(QTYRow * SalePriceRow);
                    TotalRow = Comon.cDec(QTYRow * SalePriceRow - DiscountRow);
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = HavVatRow == true ? Comon.cDec((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
                    NetRow = Comon.cDec(TotalRow + AdditionalAmountRow);
                    totalqty += QTYRow;
                    //totalDiamond+=Comon.cDec(gridView1.GetRowCellValue(rowIndex, "DIAMOND_W").ToString());
                    gridView1.SetRowCellValue(rowIndex, gridView1.Columns["Total"], TotalRow.ToString());
                    gridView1.SetRowCellValue(rowIndex, gridView1.Columns["AdditionalValue"], AdditionalAmountRow.ToString());
                    gridView1.SetRowCellValue(rowIndex, gridView1.Columns["Net"], NetRow.ToString());

                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                }
                lblUnitDiscount.Text = DiscountTotal.ToString("N" + MySession.GlobalPriceDigits);
                DiscountOnTotal = Comon.cDec(txtDiscountOnTotal.Text);
                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.cDec(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = Comon.cDec(TotalAfterDiscount).ToString("N" + MySession.GlobalPriceDigits);
                if (DiscountOnTotal > 0)
                {
                    decimal Total = TotalAfterDiscount - DiscountOnTotal;
                    AdditionalAmount = (Total) / 100 * MySession.GlobalPercentVat;
                    Net = Comon.cDec(Total + AdditionalAmount);
                }
                lblAdditionaAmmount.Text = Comon.cDec(AdditionalAmount).ToString("N" + MySession.GlobalPriceDigits);
                lblNetBalance.Text = Comon.cDec(Net).ToString("N" + MySession.GlobalPriceDigits);

                txtwhghtGold.Text = Comon.ConvertToDecimalQty(totalqty).ToString("N" + MySession.GlobalQtyDigits);
                lblTotalDiamond.Text =  totalDiamond.ToString();

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
        public void fillMAsterData(DataTable dt)
        {

            txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
            txtStoreID_Validating(null, null);
            comCostCenterID.EditValue = dt.Rows[0]["CostCenterID"].ToString();
            
            StopSomeCode = true;
            cmbMethodID.EditValue = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());
            StopSomeCode = false;
            cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurencyID"].ToString());
            cmbNetType.EditValue = Comon.cDbl(dt.Rows[0]["NetType"].ToString());

            txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
            txtCustomerID_Validating(null, null);

            txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
            cmbSellerID.EditValue = dt.Rows[0]["SellerID"].ToString();
          

            txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
            txtDelegateID_Validating(null, null);

            txtEnteredByUserID.Text = dt.Rows[0]["UserID"].ToString();
            txtEnteredByUserID_Validating(null, null);

            txtEditedByUserID.Text = dt.Rows[0]["EditUserID"].ToString();
            txtEditedByUserID_Validating(null, null);
            decimal totalqtyEquivalen = 0;

            for (int i = 0; i <= dt.Rows.Count-1; i++)
            {
                totalqtyEquivalen += Comon.cDec(dt.Rows[i]["Equivalen"].ToString());
            }
            lblInvoiceTotalGold.Text = Comon.cDec(totalqtyEquivalen.ToString()).ToString();
            ////Account

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
            txtCostSalseAccountID.Text = dt.Rows[0]["CostSalseAccountID"].ToString();
            txtCostSalseID_Validating(null, null);
            txtSalesRevenueAccountID.Text = dt.Rows[0]["SalesRevenueAccountID"].ToString();
            txtSalesRevenueID_Validating(null, null);

            //Masterdata
            txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
            txtNotes.Text = dt.Rows[0]["Notes"].ToString();
            txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
            txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();

            //Date
            
          

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

            if (Comon.cDbl(lblAdditionaAmmount.Text) > 0)
                chkForVat.Checked = true;
            else
                chkForVat.Checked = false;


            gridControl.DataSource = dt;
             
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;

            CalculateRow();


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

            }
            else if (FocusedControl.Trim() == txtStoreID.Name)
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

            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {

            }

            else if (FocusedControl.Trim() == gridControl.Name)
            {

                if (gridView1.FocusedColumn.Name == "colItemID" || gridView1.FocusedColumn.Name == "col" + ItemName)
                {

                }
                else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
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
        #region Other Function
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;
            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleReturnCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtCostSalseAccountID.Name)
            {
               if (!MySession.GlobalAllowChangefrmSaleReturnCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

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
                if (!MySession.GlobalAllowChangefrmSaleReturnDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                 if (!MySession.GlobalAllowChangefrmSaleReturnStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;
                if (gridView1.FocusedColumn.Name == "colDIAMOND_W")
                {
                    frmPurchaseDaimondDetils frm = new frmPurchaseDaimondDetils();
                    frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                    frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                    frm.ReadData(txtInvoiceID.Text, gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BarCode").ToString(), gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DIAMOND_W").ToString(),Comon.cDbl(txtStoreID.Text.ToString())+"", txtCustomerID.Text, Comon.cInt(cmbBranchesID.EditValue));
                    frm.ReadRecord(2, 3);
                    frm.Show();
                }

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
                        frm.SetValueToControl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
                    }
                    else
                        frm.Dispose();
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
                if (FocusedControl == txtInvoiceID.Name)
                {
                    txtInvoiceID.Text = cls.PrimaryKeyValue.ToString();
                    txtInvoiceID_Validating(null, null);
                }
                else if (FocusedControl == txtCustomerID.Name)
                {
                    txtCustomerID.Text = cls.PrimaryKeyValue.ToString();
                    txtCustomerID_Validating(null, null);
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
                        FileItemData(Stc_itemsDAL.GetItemData(Barcode, UserInfo.FacilityID));
                        double ItemID = Comon.cDbl(gridView1.GetRowCellValue(rowIndex, gridView1.Columns["ItemID"]));
                        RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(ItemID);
                        gridView1.Columns[SizeName].ColumnEdit = rSize;
                        gridControl.RepositoryItems.Add(rSize);
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
            }
        }
        public void ReadRecord(long InvoiceID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                    dt = Sales_SaleInvoicesReturnDAL.frmGetDataDetalByID(InvoiceID, UserInfo.BRANCHID, UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        //Validate
                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);
                        comCostCenterID.EditValue = dt.Rows[0]["CostCenterID"].ToString();

                        StopSomeCode = true;
                        cmbMethodID.EditValue = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());
                        StopSomeCode = false;

                        txtSalesRevenueAccountID.Text = dt.Rows[0]["SalesRevenueAccountID"].ToString();
                        txtSalesRevenueID_Validating(null, null);
                        txtCostSalseAccountID.Text = dt.Rows[0]["CostSalseAccountID"].ToString();
                        txtCostSalseID_Validating(null, null);
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurencyID"].ToString());
                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                       
                        cmbNetType.EditValue = Comon.cDbl(dt.Rows[0]["NetType"].ToString());

                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);

                        txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        cmbSellerID.EditValue = dt.Rows[0]["SellerID"].ToString();
                        txtCustomerInvoiceID.Text= dt.Rows[0]["CustomerInvoiceID"].ToString();
                        txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        txtDelegateID_Validating(null, null);

                        txtEnteredByUserID.Text = dt.Rows[0]["UserID"].ToString();
                        txtEnteredByUserID_Validating(null, null);
                        txtEditedByUserID.Text = dt.Rows[0]["EditUserID"].ToString();
                        txtEditedByUserID_Validating(null, null);

                        //Account
                        //Account
                        lblDebitAccountID.Text = dt.Rows[0]["DebitAccount"].ToString();
                        lblDebitAccountID_Validating(null, null);

                        lblCreditAccountID.Text = dt.Rows[0]["CreditAccount"].ToString();
                        lblCreditAccountID_Validating(null, null);

                        lblAdditionalAccountID.Text = dt.Rows[0]["AdditionalAccount"].ToString();
                        lblAdditionalAccountID_Validating(null, null);

                        lblNetAccountID.Text = dt.Rows[0]["NetAccount"].ToString();
                        lblNetAccountID_Validating(null, null);
                        lblDiscountDebitAccountID.Text = dt.Rows[0]["DiscountCreditAccount"].ToString();
                        lblDiscountCreditAccountID_Validating(null, null);

                        //Masterdata
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();


                        //Date
                        txtInvoiceDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString());
                        txtWarningDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["WarningDate"].ToString());
                        txtCheckSpendDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["CheckSpendDate"].ToString());
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
                        lblInvoiceTotalBeforeDiscount.Text = dt.Rows[0]["InvoiceTotal"].ToString();

                        txtwhghtGold.Text = dt.Rows[0]["InvoiceGoldTotal"].ToString();
                        lblInvoiceTotalGold.Text = dt.Rows[0]["InvoiceEquivalenTotal"].ToString();
                        cmbStatus.EditValue = Comon.cIntToBoolean(Comon.cInt(dt.Rows[0]["Posted"].ToString()));


                        //txtDiscountOnTotal_Validating(null, null);

                        lblAdditionaAmmount.Text = dt.Rows[0]["AdditionaAmountTotal"].ToString();
                        lblNetBalance.Text = dt.Rows[0]["NetBalance"].ToString();

                        gridControl.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        CalculateRow();
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void GetAccountsDeclaration()
        {
            try
            {
                  #region get accounts declaration
                if (string.IsNullOrEmpty(MySession.GlobalDefaultSaleReturnCreditAccountID) == false)
                   
                {
                    lblCreditAccountID.Text = MySession.GlobalDefaultSaleReturnCreditAccountID;
                    txtStoreID.Text = MySession.GlobalDefaultSaleReturnCreditAccountID;
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

                txtSalesRevenueAccountID.Text = "";
                txtCostSalseAccountID.Text = "";
                txtRegistrationNo.Text = "";
                txtCustomerInvoiceID.Text = "";
                lblInvoiceTotalBeforeDiscount.Text = "0";
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
                txtInvoiceDate.EditValue = DateTime.Now;
                txtWarningDate.EditValue = DateTime.Now;
                txtCheckSpendDate.EditValue = DateTime.Now;
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
                lblUnitDiscount.Text = "0";
                txtDiscountOnTotal.Text = "0";
                txtDiscountPercent.Text = "0";
                lblDiscountTotal.Text = "0";
                lblAdditionaAmmount.Text = "0";
                lblNetBalance.Text = "0";
                try
                {
                    GetAccountsDeclaration();
                    cmbBranchesID.EditValue = MySession.GlobalBranchID;
                    txtEnteredByUserID.Text = UserInfo.ID.ToString();
                    txtEnteredByUserID_Validating(null, null);
                    txtEditedByUserID.Text = "0";
                    txtEditedByUserID_Validating(null, null);
                    txtDelegateID.Text = MySession.GlobalDefaultSaleDelegateID;
                    txtDelegateID_Validating(null, null);
                    comCostCenterID.EditValue = MySession.GlobalDefaultCostCenterID;
                    cmbSellerID.EditValue = MySession.GlobalDefaultSellerID;
                    txtStoreID.Text = MySession.GlobalDefaultStoreID;

                    cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultSaleCurencyID);
                }
                catch (Exception ex) { }
                lstDetail = new BindingList<Sales_SalesInvoiceReturnDetails>();
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
                    SplashScreenManager.CloseForm(false);
                    strSQL = "SELECT TOP 1 * FROM " + Sales_SaleInvoicesReturnDAL.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_SaleInvoicesReturnDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Sales_SaleInvoicesReturnDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Sales_SaleInvoicesReturnDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Sales_SaleInvoicesReturnDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Sales_SaleInvoicesReturnDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Sales_SaleInvoicesReturnDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Sales_SaleInvoicesReturnDAL();

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
                txtInvoiceID.Text = Sales_SaleInvoicesReturnDAL.GetNewID().ToString();
                txtRegistrationNo.Text = RestrictionsDailyDAL.GetNewID(this.Name).ToString();

                ClearFields();
                EnabledControl(true);

                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
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
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Total", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDateStr", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.DateTime"));
            dtItem.Columns.Add("Bones", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("HavVat", System.Type.GetType("System.Boolean"));
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
                dtItem.Rows[i]["QTY"] = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["SalePrice"] = Comon.cDec(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
                dtItem.Rows[i]["Bones"] = Comon.cDec(gridView1.GetRowCellValue(i, "Bones").ToString());
                dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["Discount"] = Comon.cDec(gridView1.GetRowCellValue(i, "Discount").ToString());
                dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString());
               
                dtItem.Rows[i]["CostPrice"] = Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView1.GetRowCellValue(i, "HavVat").ToString());
                dtItem.Rows[i]["Total"] = Comon.cDec(gridView1.GetRowCellValue(i, "Total").ToString());
               
                dtItem.Rows[i]["AdditionalValue"] = Comon.cDec(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                dtItem.Rows[i]["Net"] = Comon.cDec(gridView1.GetRowCellValue(i, "Net").ToString());
                dtItem.Rows[i]["CurrencyID"] = gridView1.GetRowCellValue(i, "CurrencyID").ToString();
                dtItem.Rows[i]["CurrencyName"] = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                dtItem.Rows[i]["CurrencyPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                dtItem.Rows[i]["CurrencyEquivalent"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
        }
        protected override void DoSave()
        {
            try
            {
                

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
                if (Comon.ConvertToDecimalPrice(lblNetBalance.Text) < Comon.ConvertToDecimalPrice(txtNetAmount.Text))
                {
                    txtNetAmount.Focus();
                    txtNetAmount.ToolTip = "مبلغ الشبكة  اكبر من الصافي ";
                    Validations.ErrorText(txtNetAmount, txtNetAmount.ToolTip);
                    return;
                }

                if (!Lip.CheckTheProcessesIsPosted("Sales_SalesInvoiceReturnMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtInvoiceID.Text)))
                {
                    Messages.MsgWarning(Messages.TitleError, Messages.msgTheProcessIsNotUpdateBecuseIsPosted);
                    return;
                }
                #region Save Diamond  Type
                //Sales_PurchaseDiamondDetails objRecord = new Sales_PurchaseDiamondDetails();
                //objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);

                //objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                //objRecord.FacilityID = UserInfo.FacilityID;

                //objRecord.StoreID = Comon.cInt(txtStoreID.Text);
                //objRecord.SupplierID = Comon.cDbl(txtCustomerID.Text);
                //objRecord.Cancel = 0;

                //for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                //{
                //    if (gridView1.GetRowCellValue(i, "DIAMOND_W").ToString() != "")
                //    {
                //        Sales_PurchaseDiamondDetails returned;
                //        List<Sales_PurchaseDiamondDetails> listreturned = new List<Sales_PurchaseDiamondDetails>();
                //        DataTable dtt = Sales_PurchaseDiamondDetailsDAL.frmGetDataDetalByBarCode(Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, gridView1.GetRowCellValue(i, "BarCode").ToString(), 1);

                //        for (int j = 0; j < dtt.Rows.Count; j++)
                //        {
                 
                //            returned = new Sales_PurchaseDiamondDetails();
                //            returned.FacilityID = UserInfo.FacilityID;
                //            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                //            returned.BarCode = dtt.Rows[j]["BarCode"].ToString();
                //            returned.ItemID = Comon.cInt(dtt.Rows[j]["ItemID"]);
                //            returned.BarCodeItem = dtt.Rows[j]["BarCodeItem"].ToString();
                //            returned.ArbItemName = dtt.Rows[j][ItemName].ToString();
                //            returned.WeightIn = Comon.ConvertToDecimalQty(dtt.Rows[j]["WeightIn"]);
                //            returned.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                //            returned.WeightOut = 0;
                //            returned.TypeOpration = 4;
                //            returned.CaptionOpration = "فاتورة مردود مبيعات";
                //            returned.PriceCarat = Comon.cDec(dtt.Rows[j]["PriceCarat"]);
                //            returned.TotalPrice = Comon.cDec(dtt.Rows[j]["TotalPrice"]);
                //            returned.SupplierID = Comon.cDbl(txtCustomerID.Text);
                //            returned.StoreID = Comon.cInt(txtStoreID.Text);

                //            if (returned.WeightIn <= 0 || returned.StoreID <= 0 || (returned.PriceCarat <= 0 && returned.TotalPrice <= 0) || returned.ItemID <= 0)
                //                continue;
                //            listreturned.Add(returned);
                //        }
                //        if (listreturned.Count > 0)
                //        {
                //            objRecord.DiamondDatails = listreturned;
                //            string Result = Sales_PurchaseDiamondDetailsDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();

                //        }
                //    }
                //}
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
        private void Save()
        {

            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            Sales_SalesInvoiceReturnMaster objRecord = new Sales_SalesInvoiceReturnMaster();
            objRecord.InvoiceID = 0;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.CostSalseAccountID = Comon.cDbl(txtCostSalseAccountID.Text);
            objRecord.SalesRevenueAccountID = Comon.cDbl(txtSalesRevenueAccountID.Text);
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.MethodeID = Comon.cInt(cmbMethodID.EditValue);
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            objRecord.NetType = Comon.cDbl(cmbNetType.EditValue);
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.CustomerName = txtCustomerName.Text.Trim();
            objRecord.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SellerID = Comon.cInt(cmbSellerID.EditValue);
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Return Sale Invoice" : "فاتوره مردود  مبيعات ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Return Sale Invoice" : " فاتوره مردود مبيعات "));
            objRecord.Notes = txtNotes.Text;
            objRecord.CustomerInvoiceID = Comon.cDbl(txtCustomerInvoiceID.Text);
            //Account
            objRecord.DebitGoldAccountID = Comon.cDbl(txtDebitGoldAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);

            objRecord.DebitAccount = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            //objRecord.DiscountDebitAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
            objRecord.DiscountCreditAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
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
            objRecord.DiscountOnTotal = Comon.cDec(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = (Comon.cDec(lblInvoiceTotalBeforeDiscount.Text));
            objRecord.AdditionaAmountTotal = Comon.cDec(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Comon.cDec(lblNetBalance.Text);


            objRecord.InvoiceGoldTotal = Comon.ConvertToDecimalPrice(txtwhghtGold.Text);
            objRecord.InvoiceEquivalenTotal = Comon.ConvertToDecimalPrice(lblInvoiceTotalGold.Text);


            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);

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
                objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }
            Sales_SalesInvoiceReturnDetails returned;
            List<Sales_SalesInvoiceReturnDetails> listreturned = new List<Sales_SalesInvoiceReturnDetails>();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Sales_SalesInvoiceReturnDetails();
                returned.ID = i + 1;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
                //returned.STONE_W = Comon.cDec(gridView1.GetRowCellValue(i, "STONE_W").ToString());
                //returned.DIAMOND_W = Comon.cDec(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
                //returned.BAGET_W = Comon.cDec(gridView1.GetRowCellValue(i, "BAGET_W").ToString());

                returned.Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                returned.Equivalen = Comon.ConvertTo21Caliber(returned.QTY, Comon.cInt(returned.Caliber), 21);

                returned.Color = gridView1.GetRowCellValue(i, "Color").ToString();
                returned.CLARITY = gridView1.GetRowCellValue(i, "CLARITY").ToString();


                returned.SalePrice = Comon.cDec(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
                returned.Bones = Comon.cInt(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.Description = gridView1.GetRowCellValue(i, "Description").ToString();
                returned.StoreID = Comon.cDbl(txtStoreID.Text);
                returned.Discount = Comon.cDec(gridView1.GetRowCellValue(i, "Discount").ToString());
                returned.CurrencyID =gridView1.GetRowCellValue(i, "CurrencyID")!=null? Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString()):Comon.cInt(MySession.GlobalDefaultCurencyID);
                returned.CurrencyName = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                returned.CurrencyPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());

                 returned.ExpiryDateStr = 20220101;
                returned.CostPrice = Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                if (MySession.GlobalHaveVat == "1")
                    returned.AdditionalValue = Comon.cDec(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                else
                    returned.AdditionalValue = 0;
                returned.Net = Comon.cDec(gridView1.GetRowCellValue(i, "Net").ToString());
                returned.Total = Comon.cDec(gridView1.GetRowCellValue(i, "Total").ToString());
                if (returned.AdditionalValue == 0)
                    returned.HavVat = false;
                else
                    returned.HavVat = true;

                returned.Cancel = 0;
                returned.Serials = "";
                if (returned.QTY <= 0 || returned.StoreID <= 0 || returned.SalePrice <= 0 || returned.SizeID <= 0 || returned.ItemID <= 0)
                    continue;
                listreturned.Add(returned);
            }

            if (listreturned.Count > 0)
            {
                objRecord.SalsReturnDatails = listreturned;
                string Result = Sales_SaleInvoicesReturnDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();

                if (Comon.cInt(cmbStatus.EditValue) > 1)
                {
                    // حفظ الحركة المخزنية 
                    if (Comon.cInt(Result) > 0)
                    {
                        int MoveID = SaveStockMoveing(Comon.cInt(Result));
                        if (MoveID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
                    }
                    if (Comon.cInt(Result) > 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = 0;
                        if (MySession.GlobalInventoryType == 2)//جرد دوري 
                            VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result));
                        else if (MySession.GlobalInventoryType == 1)
                            VoucherID = SaveVariousVoucherMachinContinuousInv(Comon.cInt(Result));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update Sales_SalesInvoiceReturnMaster Set RegistrationNo =" + VoucherID + " where " + Sales_SaleInvoicesDAL.PremaryKey + " = " + txtInvoiceID.Text + " AND BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
                    }
                }
                SplashScreenManager.CloseForm(false);
                if (IsNewRecord == true)
                {
                    if ( Comon.cDbl(Result) > 0)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        this.Close();
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                    }
                }
                else
                {
                    if (Comon.cDbl(Result) > 0)
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

                Sales_SalesInvoiceReturnMaster model = new Sales_SalesInvoiceReturnMaster();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                string Result = Sales_SaleInvoicesReturnDAL.DeleteSales_SalesInvoiceReturnMaster(model).ToString();
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
                ReportName = "rptSalesInvoiceReturnDaimond";


                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                 
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = "";
                rptForm.Parameters["CostCenterName"].Value = "";
                rptForm.Parameters["RemaindAmount"].Value = lblRemaindAmount.Text.Trim().ToString();
                rptForm.Parameters["PaidAmount"].Value = txtPaidAmount.Text.Trim().ToString();

                if (txtVatID.Text != string.Empty)
                    rptForm.Parameters["StoreName"].Value = "مردودات ضريبية  ";
                else
                    rptForm.Parameters["StoreName"].Value = "مردودات ضريبية مبسطه ";


                if (Comon.cInt(cmbMethodID.EditValue) == 1)
                    rptForm.Parameters["MethodName"].Value = "نقدا";

                if (Comon.cInt(cmbMethodID.EditValue) == 2)
                {
                    rptForm.Parameters["MethodName"].Value = "اجل";
                    rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.ToString();
                }
                else //if (Comon.cInt(cmbMethodID.EditValue) == 2)
                {
                    rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.ToString();
                }

                if (Comon.cInt(cmbMethodID.EditValue) == 5)
                {
                    rptForm.Parameters["MethodName"].Value = "نقدأ/شبكة";
                    rptForm.Parameters["CashTotal"].Value = Comon.ConvertToDecimalPrice(lblNetBalance.Text.Trim().ToString()) - Comon.ConvertToDecimalPrice(txtNetAmount.Text.Trim().ToString());
                    rptForm.Parameters["NetTotal"].Value = txtNetAmount.Text.Trim().ToString();
                }
                else if (Comon.cInt(cmbMethodID.EditValue) == 3)
                {
                    rptForm.Parameters["MethodName"].Value = "شبكة";
                    rptForm.Parameters["CashTotal"].Value = 0;
                    rptForm.Parameters["NetTotal"].Value = lblNetBalance.Text.Trim().ToString();

                }
                else
                {

                    rptForm.Parameters["NetTotal"].Value = 0;
                    rptForm.Parameters["CashTotal"].Value = lblNetBalance.Text.Trim().ToString();
                }
                rptForm.Parameters["VATCOMPANY"].Value = MySession.VAtCompnyGlobal;
                rptForm.Parameters["InvoiceDate"].Value = txtInvoiceDate.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["VatID"].Value = txtVatID.Text;
                rptForm.Parameters["footer"].Value = MySession.footer;
                rptForm.Parameters["Notes"].Value = txtNotes.Text.Trim().ToString();
                rptForm.Parameters["CustomerMobile"].Value = "";
                string Date = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
                int year = Convert.ToInt32(Date.Substring(0, 4));
                int month = Convert.ToInt32(Date.Substring(4, 2))
                    ;
                int day = Convert.ToInt32(Date.Substring(6, 2));
                DateTime tempDate = new DateTime(year, month, day);
                rptForm.Parameters["HDate"].Value = Comon.ConvertFromEngDateToHijriDate(tempDate).Substring(0, 10);
                rptForm.Parameters["NumbToWord"].Value = Lip.ToWords(Convert.ToDecimal(lblNetBalance.Text.Trim().ToString()), 2);
                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotalBeforeDiscount.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = lblUnitDiscount.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = txtwhghtGold.Text;
                rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["Tel"].Value = cmbSellerID.Text.Trim().ToString();
                rptForm.Parameters["Mobile"].Value = ""; ;
                rptForm.Parameters["TotalDaimond"].Value = ""; ;
                rptForm.Parameters["TotalStone"].Value = ""; ;
                rptForm.Parameters["TotalBagate"].Value = ""; ;
                rptForm.Parameters["Tafqeet"].Value = Lip.ToWords(Comon.ConvertToDecimalPrice(lblNetBalance.Text), 1);
                 
                 
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["ItemName"] = gridView1.GetRowCellValue(i, "ArbItemName").ToString();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["SizeName"] = gridView1.GetRowCellValue(i, SizeName).ToString();
                    row["QTY"] = gridView1.GetRowCellValue(i, "QTY").ToString();
                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    if (MySession.GlobalHaveVat == "1")
                        row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    else
                        row["AdditionalValue"] = 0;
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["SalePrice"] = gridView1.GetRowCellValue(i, "SalePrice").ToString();
                    row["Description"] = gridView1.GetRowCellValue(i, "BAGET_W").ToString();
                    row["Bones"] = gridView1.GetRowCellValue(i, "DIAMOND_W").ToString();
                    row["ExpiryDate"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();
                    row["Color"] = gridView1.GetRowCellValue(i, "Color").ToString();
                    row["CLARITY"] = gridView1.GetRowCellValue(i, "CLARITY").ToString();


                    dataTable.Rows.Add(row);

                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = ReportName;

                InvoiceViewModel x = new InvoiceViewModel();
                // معلومات الضريبة الخمسة الأولى
                x.ArbCompanyName = MySession.GlobalBranchName.ToUpper();
                x.CompanyVatCode = MySession.VAtCompnyGlobal;
                x.InvoiceDate = Comon.cDateTime(txtInvoiceDate.Text);
                x.NetTotal = Comon.cDec(lblNetBalance.Text);
                x.VatAmount = Comon.cDec(lblAdditionaAmmount.Text);
                string Base64 = ZATKAQREncryption.ZATCATLVBase64.GetBase64(x.ArbCompanyName, x.CompanyVatCode, x.InvoiceDate, Convert.ToDouble(x.NetTotal), Convert.ToDouble(x.VatAmount));
                rptForm.Parameters["DelegateName"].Value = Base64;

                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = true;
                subreport.ReportSource = ReportComponent.CompanyHeader();


                /******************** Report Binding ************************/

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

        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                        {

                            lblCreditAccountID.Text = txtCustomerID.Text;
                            lblCreditAccountName.Text = lblCustomerName.Text;

                            if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                            {
                                //chkForVat.Checked = true;
                                txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                            }
                            else
                            {

                                txtVatID.Text = "";
                                //chkForVat.Checked = false;
                            }
                        }
                    }
                    else
                    {
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtCustomerID.Text + " And BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {
                            
                            lblCreditAccountName.Text = dt.Rows[0]["CustomerName"].ToString();
                            lblCreditAccountID.Text = txtCustomerID.Text;
                            lblCreditAccountName.Text = dt.Rows[0]["CustomerName"].ToString();
                            if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                            {
                                //chkForVat.Checked = true;
                                txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                            }
                            else
                            {

                                txtVatID.Text = "";
                                //chkForVat.Checked = false;
                            }


                        }
                        else
                        {
                            lblCustomerName.Text = "";
                            txtCustomerID.Text = "";
                            txtVatID.Text = "";
                            //chkForVat.Checked = false;
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
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
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
                    decimal DiscountOnTotal = Comon.cDec(txtDiscountOnTotal.Text);
                    decimal whole = Comon.cDec(lblInvoiceTotal.Text);
                    decimal TotalUnitDiscount = Comon.cDec(lblUnitDiscount.Text);
                    decimal TotalDiscount = DiscountOnTotal + TotalUnitDiscount;
                    if (Comon.cDec(txtDiscountOnTotal.Text) != 0)
                    {
                        txtDiscountPercent.Text = ((DiscountOnTotal / whole) * 100).ToString("N" + MySession.GlobalPriceDigits);
                        decimal TotalDiscountPercent = Comon.cDec((((TotalDiscount) / whole) * 100).ToString("N" + MySession.GlobalPriceDigits));
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
                    // if (Comon.cDec(txtDiscountOnTotal.Text) > 0 && !MySession.GlobalAllowedPercentDiscount) { Messages.MsgError(Messages.TitleError, Messages.msgNotAllowedPercentDiscount); return; }

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
                    decimal percent = Comon.cDec(txtDiscountPercent.Text);
                    decimal whole = Comon.cDec(lblInvoiceTotal.Text);
                    if (Comon.cDec(txtDiscountOnTotal.Text) != Comon.cDec(Math.Round(((percent * whole) / 100))))
                    {
                        txtDiscountOnTotal.Text = ((percent * whole) / 100).ToString("N" + MySession.GlobalPriceDigits);

                        decimal DiscountOnTotal = Comon.cDec(txtDiscountOnTotal.Text);
                        decimal UnitDiscount = Comon.cDec(lblUnitDiscount.Text);
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
                lblRemaindAmount.Text = (Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(txtPaidAmount.Text)).ToString();

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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblChequeAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblDebitAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblCreditAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblAdditionalAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblDiscountDebitAccountID.Text + ") ";
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblNetAccountID.Text + ") ";
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
        #endregion
        /************************Event From **************************/
        private void frmSaleInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();

            if (e.KeyCode == Keys.F9)
            {
                DoSave();
            }
        }

        /*******************Event CheckBoc***************************/
        //private void chForVat_EditValueChanged(object sender, EventArgs e)
        //{

        //    decimal Total = 0;
        //    decimal Net = 0;
        //    decimal DiscountTotal = 0;
        //    decimal DiscountOnTotal = 0;
        //    decimal AdditionalAmount = 0;


        //    decimal DiscountRow = 0;
        //    decimal QTYRow = 0;
        //    decimal CostPriceRow = 0;
        //    decimal TotalRow = 0;
        //    decimal NetRow = 0;
        //    decimal AdditionalAmountRow = 0;

        //    DataTable dtItem = new DataTable();

        //    dtItem.Columns.Add("ID", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("FacilityID", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("ItemID", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("SizeID", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("Description", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("StoreID", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("Discount", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("AdditionalValue", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("Net", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("Cancel", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
        //    dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));
        //    dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));
        //    dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
        //    dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
        //    dtItem.Columns.Add("Total", System.Type.GetType("System.Decimal"));
        //    dtItem.Columns.Add("ExpiryDateStr", System.Type.GetType("System.Decimal"));
        //    dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.DateTime"));
        //    dtItem.Columns.Add("Bones", System.Type.GetType("System.Decimal"));
        //    dtItem.Columns.Add("SalePrice", System.Type.GetType("System.Decimal"));
        //    dtItem.Columns.Add("HavVat", System.Type.GetType("System.Boolean"));

        //    for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
        //    {

        //        dtItem.Rows.Add();
        //        dtItem.Rows[i]["ID"] = i;
        //        dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
        //        dtItem.Rows[i]["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
        //        dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
        //        dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
        //        dtItem.Rows[i][ItemName] = gridView1.GetRowCellValue(i, ItemName).ToString();
        //        dtItem.Rows[i][SizeName] = gridView1.GetRowCellValue(i, SizeName).ToString();

        //        dtItem.Rows[i]["QTY"] = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
        //        dtItem.Rows[i]["SalePrice"] = Comon.cDec(gridView1.GetRowCellValue(i, "SalePrice").ToString()); ;
        //        dtItem.Rows[i]["Bones"] = Comon.cDec(gridView1.GetRowCellValue(i, "Bones").ToString());
        //        dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
        //        dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());
        //        dtItem.Rows[i]["Discount"] = Comon.cDec(gridView1.GetRowCellValue(i, "Discount").ToString());
        //        dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString());
        //        dtItem.Rows[i]["ExpiryDate"] = gridView1.GetRowCellValue(i, "ExpiryDate");
        //        dtItem.Rows[i]["CostPrice"] = Comon.cDec(gridView1.GetRowCellValue(i, "CostPrice").ToString());
        //        dtItem.Rows[i]["HavVat"] = Comon.cbool(gridView1.GetRowCellValue(i, "HavVat").ToString());
        //        //dtItem.Rows[i]["HavVat"] = chkForVat.Checked;
        //        dtItem.Rows[i]["Total"] = Comon.cDec(gridView1.GetRowCellValue(i, "Total").ToString());
        //        dtItem.Rows[i]["Cancel"] = 0;
        //        CostPriceRow = Comon.cDec(gridView1.GetRowCellValue(i, "SalePrice").ToString());
        //        QTYRow = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
        //        DiscountRow = Comon.cDec(gridView1.GetRowCellValue(i, "Discount").ToString());
        //        TotalRow = CostPriceRow * QTYRow;

        //        if (chkForVat.Checked == true)
        //        {

        //            AdditionalAmountRow = (TotalRow - DiscountRow) / 100 * MySession.GlobalPercentVat;
        //            NetRow = Comon.cDec((TotalRow - DiscountRow) + AdditionalAmountRow);
        //            dtItem.Rows[i]["AdditionalValue"] = AdditionalAmountRow.ToString("N" + MySession.GlobalPriceDigits);
        //            dtItem.Rows[i]["Net"] = NetRow.ToString("N" + MySession.GlobalPriceDigits);

        //            AdditionalAmount += AdditionalAmountRow;
        //            DiscountTotal += DiscountRow;
        //            Total += TotalRow;
        //            Net += NetRow;


        //        }
        //        else
        //        {
        //            AdditionalAmountRow = 0;
        //            NetRow = TotalRow - DiscountRow;
        //            dtItem.Rows[i]["AdditionalValue"] = 0;
        //            dtItem.Rows[i]["Net"] = NetRow.ToString("N" + MySession.GlobalPriceDigits);

        //            AdditionalAmountRow = 0;
        //            DiscountTotal += DiscountRow;
        //            Total += TotalRow;
        //            Net += NetRow;
        //        }


        //    }
        //    DiscountOnTotal = Comon.cDec(txtDiscountOnTotal.Text);
        //    lblAdditionaAmmount.Text = AdditionalAmountRow.ToString("N" + MySession.GlobalPriceDigits);
        //    lblNetBalance.Text = Net.ToString("N" + MySession.GlobalPriceDigits);

        //    gridView1.Columns["HavVat"].OptionsColumn.ReadOnly = !chkForVat.Checked;


        //    gridControl.DataSource = dtItem;

        //    // CalculateRow();

        //    //gridView1.Focus();
        //    //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
        //}

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
            if (Comon.cInt(cmbMethodID.EditValue) != 5)
            {
                lblCreditAccountID.Text = cmbNetType.EditValue.ToString();
                lblCreditAccountID_Validating(null, null);
            }
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
                lblNetAccountID.Tag = "IsNumber";
                if (value == 1)
                {
                    lblCreditAccountID.Tag = "IsNumber";
                    txtCustomerID.Tag = "IsNumber";
                    lblDebitAccountID.Tag = "ImportantFieldGreaterThanZero";
                    
                    if (string.IsNullOrEmpty(MySession.GlobalDefaultSaleDebitAccountID) == false)
                    {
                        lblDebitAccountID.Text = MySession.GlobalDefaultSaleDebitAccountID;
                        lblDebitAccountID_Validating(null, null);
                    }

                    lblBankName.Visible = false;
                    cmbBank.Visible = false;
                    // txtCustomerName.Focus();
                    {
                        lblNetAccountCaption.Enabled = false;
                        lblNetAccountID.Enabled = false;
                        lblNetAccountName.Enabled = false;
                        lblCachCaption.Enabled = true;
                        lblDebitAccountID.Enabled = true;
                        lblDebitAccountName.Enabled = true;
                    }

                }
                else if (value == 2)
                {

                    lblCreditAccountID.Tag = "IsNumber";
                    lblDebitAccountID.Tag = "IsNumber";
                    txtCustomerID.Tag = "ImportantFieldGreaterThanZero";
                    
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
                    {
                        lblNetAccountCaption.Enabled = false;
                        lblNetAccountID.Enabled = false;
                        lblNetAccountName.Enabled = false;
                        lblCachCaption.Enabled = false;
                        lblDebitAccountID.Enabled = false;
                        lblDebitAccountName.Enabled = false;
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

        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.EditValue = DateTime.Now;
        }

        
        #endregion

        private void frmSalesInvoice_Load(object sender, EventArgs e)
        {
            DoNew();
             
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);


        }

        public  void txtCustomerInvoiceID_Validating(object sender, CancelEventArgs e)
        {

            
            if (GoldUsing == 2)
            {
                gridView1.Columns["Color"].Visible = false;
                gridView1.Columns["CLARITY"].Visible = false;
            }
            else
            {
                gridView1.Columns["Color"].Visible = true;
                gridView1.Columns["CLARITY"].Visible = true;
            }
            strSQL = "Select * from Sales_SalesInvoiceReturnMaster where CustomerInvoiceID=" + txtCustomerInvoiceID.Text + " And BranchID=" + MySession.GlobalBranchID +" And Cancel=0";
            DataTable dtReturn = new DataTable();
            dtReturn = Lip.SelectRecord(strSQL);
            if (dtReturn.Rows.Count > 0)
            {
                txtInvoiceID.Text = dtReturn.Rows[0]["InvoiceID"].ToString();
                txtInvoiceID_Validating(null, null);
                return;
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
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
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
            //Credit
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1 || Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
            {
              if(  Comon.cInt(cmbMethodID.EditValue.ToString()) == 1)
                   GetAccountsDeclaration();
              else
              {
                  GetAccountsDeclaration();
                  lblCreditAccountID.Text = txtCustomerID.Text;
                  lblCreditAccountName.Text = txtCustomerName.Text;
                 
              }
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(lblNetBalance.Text);
                returned.Debit = 0;
                returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);

               
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
                listreturned.Add(returned);
            }
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 3)
            {
                GetAccountsDeclaration();
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblNetAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(lblNetBalance.Text);
                returned.Debit = 0;
                returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);
               
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
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
                returned.Credit = Comon.cDbl(lblNetBalance.Text);
                returned.Debit = 0;
                returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);
                 
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
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
                returned.Credit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(txtNetAmount.Text));
 
                returned.CreditGold =  Comon.cDbl(lblInvoiceTotalGold.Text);

                 
                returned.Debit = 0;
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
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
                    returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
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
                returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
                listreturned.Add(returned);
            }
            //Debite Sale Return
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 3;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);          
            returned.VoucherID = VoucherID;
            
            returned.Debit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text) - Comon.cDbl(lblAdditionaAmmount.Text));
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
            listreturned.Add(returned);
            //===
            //Vat Sale
            if (MySession.GlobalHaveVat == "1")
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 4;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblAdditionalAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = 0;

                returned.Debit = Comon.cDbl(lblAdditionaAmmount.Text);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
                listreturned.Add(returned);
            }

            //debit Sale
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 4;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtDebitGoldAccountID.Text);
            returned.VoucherID = VoucherID;
           
            returned.DebitGold = Comon.cDbl(lblInvoiceTotalGold.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(comCostCenterID.Text);
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
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType));
            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
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
            objRecord.Notes = "فاتورة مردود مبيعات";
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);

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
                if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)               
                {
                    //GetAccountsDeclaration();
                    lblCreditAccountID.Text = txtCustomerID.Text;
                    lblCreditAccountName.Text = txtCustomerName.Text;
                }
                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the object's ID, branch ID, facility ID, account ID, credit, debit, declaration, and cost center ID properties based on the available controls.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cLong(lblDebitAccountID.Text);
                returned.VoucherID = VoucherID; 
                returned.Credit = Comon.cDbl(lblNetBalance.Text);
                // Add the object to the list of returned objects.  
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
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

                returned.Credit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text)  );
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
                listreturned.Add(returned);
            }
            // If the selected method ID is 4, create a new Acc_VariousVoucherMachinDetails object and set its properties.
            if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 4)
            {
                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();
                // Set the object's ID, branch ID, facility ID, account ID, and voucher ID properties.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblChequeAccountID.Text);
                returned.VoucherID = VoucherID;
                // Set the object's credit and debit properties based on the lblNetBalance control.
                returned.Credit = Comon.cDbl(Comon.cDbl(lblNetBalance.Text));
                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
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
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
                returned.VoucherID = VoucherID;
                // Set the object's credit and debit properties based on the lblInvoiceTotal and lblNetAmount controls.

                returned.Credit = Comon.cDbl((Comon.cDbl(lblNetBalance.Text) ) - Comon.cDbl(txtNetAmount.Text));
                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
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
                    returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.AccountID = Comon.cDbl(lblNetAccountID.Text);
                    returned.VoucherID = VoucherID;

                    returned.Credit = Comon.cDbl(txtNetAmount.Text);
                    returned.Declaration = txtNotes.Text;
                    returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
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
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDiscountDebitAccountID.Text);
                returned.VoucherID = VoucherID;
                 
                returned.Debit = Comon.cDbl(lblDiscountTotal.Text);
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
                // Add the instance to the list of records.
                listreturned.Add(returned);
            }
            returned = new Acc_VariousVoucherMachinDetails();
            // Set the properties of the instance.
            returned.ID = 3;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtCostSalseAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;
            double TotalCost = 0;
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                TotalCost += Comon.cDbl(gridView1.GetRowCellValue(i, "CostPrice"));
            }
            returned.Credit = Comon.cDbl(TotalCost);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
            //// Add the instance to the list of records.
            listreturned.Add(returned);
            // This code creates a new instance of the "Acc_VariousVoucherMachinDetails" class to represent the credit sale in the accounting records. 
            // It sets the relevant properties of the instance and adds it to the list of records.
            //Credit Sale

            returned = new Acc_VariousVoucherMachinDetails();
            // Set the properties of the instance.
            returned.ID = 4;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.Debit = Comon.cDbl(TotalCost);
            returned.DebitGold = Comon.cDbl(lblInvoiceTotalGold.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);

            // Add the instance to the list of records.
            listreturned.Add(returned);

            //===
            //Vat Sale
            if (MySession.GlobalHaveVat == "1")
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 5;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblAdditionalAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Debit = Comon.cDbl(lblAdditionaAmmount.Text);

                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
                listreturned.Add(returned);
            }
            //=

            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 6;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtSalesRevenueAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Debit =Comon.cDbl(Comon.cDbl(lblNetBalance.Text) -Comon.cDbl(lblDiscountTotal.Text)) - Comon.cDbl(lblAdditionaAmmount.Text);      
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(comCostCenterID.EditValue);
            listreturned.Add(returned);
            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        private int SaveStockMoveing(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(MySession.GlobalBranchID);
            objRecord.DocumentTypeID = DocumentType;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
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
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtCustomerID.Text);
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "Total").ToString());
                returned.Bones = Comon.cDbl(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = 0;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
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
        private void btnMachinResraction_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + Comon.cInt(txtInvoiceID.Text) + " And DocumentType=" + DocumentType).ToString());
            
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
            strSQL = "Select * from "+Sales_SaleInvoicesReturnDAL.TableName +"  where Cancel=0 ";
            DataTable dtSend = new DataTable();
            dtSend = Lip.SelectRecord(strSQL);
            if (dtSend.Rows.Count > 0)
            {
                for (int i = 0; i <= dtSend.Rows.Count - 1; i++)
                {
                    txtInvoiceID.Text = dtSend.Rows[i]["InvoiceID"].ToString();
                    cmbBranchesID.EditValue = Comon.cInt(dtSend.Rows[i]["BranchID"].ToString());
                   txtStoreID.Text = dtSend.Rows[i]["StoreID"].ToString();
                   comCostCenterID.EditValue = dtSend.Rows[i]["StoreID"].ToString();
                    txtInvoiceID_Validating(null, null);
                    IsNewRecord = true;
                    if (Comon.cInt(txtInvoiceID.Text) > 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = 0;
                        if (MySession.GlobalInventoryType == 2)//جرد دوري 
                            VoucherID = SaveVariousVoucherMachin(Comon.cInt(txtInvoiceID.Text));
                        else if (MySession.GlobalInventoryType == 1)
                            VoucherID = SaveVariousVoucherMachinContinuousInv(Comon.cInt(txtInvoiceID.Text));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + Sales_SaleInvoicesReturnDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sales_SaleInvoicesReturnDAL.PremaryKey + " = " + txtInvoiceID.Text + " AND BranchID=" + Comon.cInt(cmbBranchesID.EditValue));

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

        private void txtInvoiceDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Lip.CheckDateISAvilable(txtInvoiceDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                txtInvoiceDate.Text = Lip.GetServerDate();
                return;
            }
        }



    }
}

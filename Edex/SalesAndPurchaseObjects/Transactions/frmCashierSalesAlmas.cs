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
using Edex.DAL.Configuration;
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
using System.Text;
using DevExpress.XtraReports.Parameters;
using DAL;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.SalseSystem.Stc_itemDAL;

using System.Linq;
using Edex.Manufacturing.Codes;
using Edex.StockObjects.Transactions;

namespace Edex.SalesAndSaleObjects.Transactions
{
    public partial class frmCashierSalesAlmas : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        CompanyHeader cmpheader = new CompanyHeader();
        public string ParentAccountID;
        public int AccountLevel;
        public string GetNewID;
        public int DocumentType = 6;
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
        bool falgPrint = false;
        public decimal QtyItem = 0;
        public CultureInfo culture = new CultureInfo("en-US");
        OpenFileDialog OpenFileDialog1 = null;
        DataTable dt = new DataTable();
        GridViewMenu menu;
        private int GoldUsing = 1;
        int StoreItemID = 1;
        //all record master and detail
        BindingList<Sales_SalesInvoiceDetails> AllRecords = new BindingList<Sales_SalesInvoiceDetails>();

        //list detail
        BindingList<Sales_SalesInvoiceDetails> lstDetail = new BindingList<Sales_SalesInvoiceDetails>();

        //Detail
        Sales_SalesInvoiceDetails BoDetail = new Sales_SalesInvoiceDetails();
        string VAt = "Select CompanyVATID from  VATIDCOMPANY ";


        #endregion

        /// <summary>
        /// This function returns a new daily ID for a given FacilityID, BranchID, and USERCREATED
        /// </summary>
        /// <param name="FacilityID"></param>
        /// <param name="BranchID"></param>
        /// <param name="USERCREATED"></param>
        /// <returns></returns>
        public static long GetNewDialyID(int FacilityID, int BranchID, int USERCREATED)
        {
            long ID = 0;
            DataTable dt;
            string strSQL;
            // Construct the SQL query to select the maximum DailyID for the given BranchID and USERCREATED
            strSQL = "SELECT Max(DailyID )+1 FROM  Sales_SalesInvoiceMaster Where  BranchID =" + BranchID + " And CostCenterID=" + USERCREATED;
            // Execute the SQL query and retrieve the results in a DataTable
            dt = Lip.SelectRecord(strSQL);
            // If there are rows in the DataTable, retrieve the value of the first column of the first row as the new ID
            if(dt.Rows.Count > 0)
            {
                ID = Comon.cLong(dt.Rows[0][0].ToString());
                // If the value is zero, set the ID to 1
                if (ID == 0) ID = 1;
            }
            // Return the new ID
            return ID;
        }
        public frmCashierSalesAlmas()
        {
            try
            {
                ShowReportInReportViewer = false;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                PrimaryName = "ArbName";
                CaptionBarCode = "الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "رقم الوحدة";
                CaptionSizeName = "الوحدة ";
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
                    strSQL = "EngName";
                  }
                //Vat Enable Controle
                if (MySession.GlobalHaveVat != "1")
                {
                    labelControl55.Visible = false;
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
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", "1=1 and BranchID= " + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue =MySession.GlobalBranchID;
                FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "ID<3", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
                cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
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
                TextEdit[] txtEdit = new TextEdit[15];
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
                //txtEdit[11] = lblChequeAccountName;
                txtEdit[11] = lblEditedByUserName;
                txtEdit[12] = lblEnteredByUserName;
                txtEdit[13] = txtEditedByUserID;
                txtEdit[14] = lblSellerName;
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
                try
                {
                    /************************  Form Printing ***************************************/
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

                    /************TextEdit Account ID ***************/
                    lblDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDebitAccountID;
                    lblCreditAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCreditAccountID;
                    lblAdditionalAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleAdditionalAccountID;
                    lblChequeAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleChequeAccountID;
                    lblDiscountDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID;
                    lblNetAccountID.ReadOnly = !MySession.GlobalAllowChangefrmSaleNetAccountID;
                    txtSellerID.ReadOnly = !MySession.GlobalAllowChangefrmSaleSellerID;



                    /************ Button Search Account ID ***************/
                    RolesButtonSearchAccountID();
                }
                catch (Exception ex)
                {

                }
                /********************* Event For Account Component ****************************/
          
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
                this.txtStoreID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCustomerID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
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
                this.gridView1.RowCellStyle+=gridView1_RowCellStyle;
                this.gridView1.CustomDrawCell += gridView1_CustomDrawCell;
                    /******************************************/

                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);

                ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
                //ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
                if(MySession.GlobalHaveVat=="1")
                   ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.EditValue = !MySession.GlobalAllowBranchModificationAllScreens;

                DoNew();

                Validations.DoNewRipon(this, ribbonControl1);
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
        void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
             
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                gridView1.Appearance.Row.TextOptions.HAlignment =DevExpress.Utils.HorzAlignment.Center;
                gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

           
        }
        void txtCostSalseID_Validating(object sender, CancelEventArgs e)
        {
           
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + txtCostSalseAccountID.Text + ") and BranchID= " + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtCostSalseAccountID,lblCostSalseAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        void txtSalesRevenueID_Validating(object sender, CancelEventArgs e)
        {
          
            try
            {
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + txtSalesRevenueAccountID.Text + ") and BranchID= " + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtSalesRevenueAccountID, lblSalesRevenueAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
            gridView1.Columns["QTY"].Visible = true;
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
            gridView1.Columns["ExpiryDate"].Visible = false;
            gridView1.Columns["Description"].Visible = false;

            gridView1.Columns["DateFirst"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;
            gridView1.Columns["ItemImage"].Visible = false;
            gridView1.Columns["DateFirstStr"].Visible = false;


            gridView1.Columns["CaratPrice"].Visible = false;
            gridView1.Columns["SpendPrice"].Visible = false;
            gridView1.Columns["GroupID"].Visible = false;
            gridView1.Columns["ArbGroupName"].Visible = false;
            gridView1.Columns["EngGroupName"].Visible = false;
            //gridView1.Columns["Total"].Visible = false;
            RepositoryItemLookUpEdit rSize = Common.LookUpEditSize();
            gridView1.Columns[SizeName].ColumnEdit = rSize;
            gridControl.RepositoryItems.Add(rSize);

            /******************* Columns Visible=true *******************/

            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;
            gridView1.Columns["SizeID"].Visible = false;
            gridView1.Columns["Discount"].Visible = false;
            gridView1.Columns["HavVat"].Visible = false;
            gridView1.Columns["RemainQty"].Visible = false;
            gridView1.Columns["ItemID"].Visible = false;

            if (MySession. GlobalHaveVat!="1")
                gridView1.Columns["AdditionalValue"].Visible = false;
            else
                gridView1.Columns["AdditionalValue"].Visible = true;

            if (UserInfo.Language == iLanguage.Arabic)
                gridView1.Columns["Serials"].Caption = "رقم المرجع";
            else
                gridView1.Columns["Serials"].Caption = "Referance ID";
            gridView1.Columns["BarCode"].Caption = CaptionBarCode;
            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 100;
            gridView1.Columns["Description"].Width = 180;

            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;
            gridView1.Columns["QTY"].Caption = CaptionQTY;
            gridView1.Columns["STONE_W"].Caption = "وزن الأحجار";
            gridView1.Columns["BAGET_W"].Caption = "وزن الباجيت";
            gridView1.Columns["DIAMOND_W"].Caption = "وزن الألماس";
            gridView1.Columns["Total"].Caption = CaptionTotal;
            gridView1.Columns["Discount"].Caption = CaptionDiscount;
            gridView1.Columns["AdditionalValue"].Caption = CaptionAdditionalValue;
            gridView1.Columns["Net"].Caption = CaptionNet;
            gridView1.Columns["SalePrice"].Caption = CaptionSalePrice;
            gridView1.Columns["Description"].Caption = CaptionDescription;
            gridView1.Columns["HavVat"].Caption = CaptionHavVat;
            gridView1.Columns["RemainQty"].Caption = CaptionRemainQty;

            gridView1.Columns["Color"].Caption = "اللون";
            gridView1.Columns["CLARITY"].Caption = "النقاء";
            gridView1.Columns["Color"].Visible = false;
            gridView1.Columns["CLARITY"].Visible = false;
            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["CurrencyPrice"].Visible = false;
            gridView1.Columns["CurrencyName"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;


            gridView1.Columns["AdditionalValue"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["AdditionalValue"].OptionsColumn.AllowFocus = false;


            gridView1.Columns["Total"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["Total"].OptionsColumn.AllowFocus = false;

            gridView1.Columns["CurrencyEquivalent"].VisibleIndex = gridView1.Columns["Net"].VisibleIndex + 1;
            gridView1.Columns["CurrencyEquivalent"].Visible = false;
            DataTable dtCurrncy = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Acc_Currency where Cancel=0 and BranchID= " + MySession.GlobalBranchID);
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
            }
            gridView1.Focus();
            /*************************Columns Properties ****************************/
            //gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
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
            /////////////////////////Item COLOR
            ///
            DataTable dtitems = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM Stc_ItemsColors where BranchID= " + MySession.GlobalBranchID);
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            gridControl.RepositoryItems.Add(riComboBoxitems);
            gridView1.Columns["Color"].ColumnEdit = riComboBoxitems;
            /////////////////////////


            /////////////////////////Item CLARITY
            DataTable dtitemsCLARITY = Lip.SelectRecord("SELECT   " + PrimaryName + "   FROM Stc_ItemsSizes where BranchID= " + MySession.GlobalBranchID);
            string[] companiesitemsCLARITY = new string[dtitemsCLARITY.Rows.Count];
            for (int i = 0; i <= dtitemsCLARITY.Rows.Count - 1; i++)
                companiesitemsCLARITY[i] = dtitemsCLARITY.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitemsCLARITY = new RepositoryItemComboBox();
            riComboBoxitemsCLARITY.Items.AddRange(companiesitemsCLARITY);

            gridControl.RepositoryItems.Add(riComboBoxitemsCLARITY);
            gridView1.Columns["CLARITY"].ColumnEdit = riComboBoxitemsCLARITY;
            /////////////////////////

            /////////////////////////Description
            DataTable dt = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Stc_ItemsGroups WHERE Cancel=0 and AccountTypeID= " + 1 + " and BranchID= " + MySession.GlobalBranchID);
            string[] companies = new string[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                companies[i] = dt.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBox = new RepositoryItemComboBox();
            riComboBox.Items.AddRange(companies);
            gridControl.RepositoryItems.Add(riComboBox);
            gridView1.Columns["Description"].ColumnEdit = riComboBox;
            ///////////////////////////
            gridView1.Columns["Description"].Width = 150;
            gridView1.Columns[ItemName].Width = 150;
            gridView1.Columns["SalePrice"].Width = 90;

            //RepositoryItemLookUpEdit rBarCode = Common.LookUpEditBarCode();
            //gridView1.Columns["BarCode"].ColumnEdit = rBarCode;
            //gridControl.RepositoryItems.Add(rBarCode);

            RepositoryItemLookUpEdit rItemID = Common.LookUpEditItemID();
            gridView1.Columns["ItemID"].ColumnEdit = rItemID;
            gridControl.RepositoryItems.Add(rItemID);

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

        /// <summary>
        /// This function is executed when the "Price" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Price_Click(object sender, EventArgs e)
        {
            // Create a new instance of the frmItemPricesAndCosts form
            frmItemPricesAndCosts frm = new frmItemPricesAndCosts();

            // Get the ItemID and SizeID for the selected row in the gridView1 grid control
            var ItemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID");
            var SizeID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SizeID");
            // Set the SizeID, ItemID, and CustomerID properties of the frmItemPricesAndCosts form
            frm.SizeID = Comon.cInt(SizeID);
            frm.ItemID = Comon.cLong(ItemID);
            frm.CustomerID = Comon.cLong(txtCustomerID.Text);
            // Show the frmItemPricesAndCosts form as a dialog
            frm.ShowDialog();
            // Set the SalePrice cell value of the selected row in the gridView1 grid control to the value returned by frmItemPricesAndCosts
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], Comon.ConvertToDecimalPrice(frm.CelValue));
        }


       /// <summary>
       ///  This function is executed when the popup menu is being displayed
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                // Check if the popup menu is being displayed for the "SalePrice" or "ExpiryDate" columns
                if (e.HitInfo != null && (e.HitInfo.Column.Name == "colSalePrice" || e.HitInfo.Column.Name == "colExpiryDate"))

                    // Check if the popup menu is being displayed for a particular cell in the grid
                    if (e.HitInfo.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
                        e.Menu = menu; // Set the popup menu to be displayed
            }
            catch(Exception ex) { Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message); }
        }

        /// <summary>
        /// This function is executed when the editor for a cell is shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_ShownEditor(object sender, EventArgs e)
        {
            // Check if the editor for the cell is a CheckEdit
            if (this.gridView1.ActiveEditor is CheckEdit)

                // Check if the "For Vat" checkbox is checked
                if (chkForVat.Checked)
                {
                    // Cast the sender as a GridView and modify its ActiveEditor properties
                    GridView view = sender as GridView;
                    view.ActiveEditor.IsModified = true;
                    view.ActiveEditor.ReadOnly = false;
                }

            // Reset the HasColumnErrors member variable
            HasColumnErrors = false;
            // Call the CalculateRow() method to recalculate the row
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
                    if (col.FieldName == "BarCode" || col.FieldName == "ItemID"  || col.FieldName == "SizeID" )
                    {

                        var val = gridView1.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired );
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
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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

                    if (ColName == "BarCode"  || ColName == "SizeID" || ColName == "ItemID" )
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
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && ColName != "BarCode")
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
                        //if (ColName == "SalePrice" && Comon.ConvertToDecimalPrice(val.ToString()) < Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("CostPrice").ToString()))
                        //{

                        //    e.Valid = false;
                        //    HasColumnErrors = true;
                        //    e.ErrorText = "لايمكن البيع باقل من سعر التكلفة ";
                        //    gridView1.SetFocusedRowCellValue("Net", 0.ToString());

                        //}
                       
                        if (ColName == "BarCode")
                        {

                            DataTable dt;
                            var flagb = false;
                          
                            dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                          
                            if (dt.Rows.Count == 0)
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNoFoundThisBarCode;
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
                    if (ColName == "SalePrice")
                    {
                        string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                        decimal QTY = 1;
                        decimal Total =Comon.ConvertToDecimalPrice( Comon.ConvertToDecimalPrice(val.ToString()) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QTY").ToString()));
                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                        if (HasVat == true && chkForVat.Checked && MySession.GlobalHaveVat == "1")
                            additonalVAlue = Comon.ConvertToDecimalPrice(((Total) * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;

                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);
                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("Total", Total.ToString());
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());
                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(Net) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice"))).ToString());

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        gridView1.SetColumnError(gridView1.Columns["QTY"], "");
                        e.ErrorText = "";
                        int IsService = Comon.cInt(Lip.GetValue("SELECT  [IsService] FROM  [Stc_Items] where [Cancel]=0 and BranchID= " + MySession.GlobalBranchID+" and [ItemID]=" + Comon.cLong(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString())));
                        decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SizeID")), Comon.cDbl(txtStoreID.Text));
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Sales_SalesInvoiceDetails", "Sales_SalesInvoiceMaster", "QTY", "InvoiceID", Comon.cInt(txtInvoiceID.Text), gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString(), " ", SizeID: Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SizeID").ToString()));
                        totalQtyBalance += QtyInCommand;
                        decimal qtyCurrent = 0;
                         
                         
                        qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(gridView1, "QTY", Comon.cDec(val.ToString()), gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SizeID")));
                        if (qtyCurrent > totalQtyBalance && IsService != 1)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                            return;
                        }
                        if (MySession.AllowOutQtyNegative == true && IsService!=1)
                        {
                            if (totalQtyBalance > 0)
                            {
                                if (Comon.cDec(val.ToString()) > totalQtyBalance)
                                {
                                    e.Valid = false;
                                    HasColumnErrors = true;
                                    e.ErrorText = Messages.msgQtyisNotAvilable + totalQtyBalance.ToString();
                                    view.SetColumnError(gridView1.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                            }
                        }

                        bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                     
                        if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Serials") != null || Comon.cDbl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Serials")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], CostPriceOrder(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Serials").ToString(), Comon.cDec(e.Value)));

                        decimal PriceUnit = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("SalePrice"));
                        decimal Total = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(PriceUnit) * Comon.cDec(e.Value.ToString()));
                        decimal additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);

                        if (HasVat == true && chkForVat.Checked &&MySession.GlobalHaveVat == "1")
                            additonalVAlue = Comon.ConvertToDecimalPrice((Total * MySession.GlobalPercentVat) / 100);
                        else
                            additonalVAlue = 0;
                        decimal Net = Comon.ConvertToDecimalPrice(Total + additonalVAlue);

                        gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                        gridView1.SetFocusedRowCellValue("SalePrice", PriceUnit.ToString());
                        gridView1.SetFocusedRowCellValue("Total", Total.ToString());
                        gridView1.SetFocusedRowCellValue("Net", Net.ToString());

                        string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                        int ItemID = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]).ToString());
                        int SizeID = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"]).ToString());
                        int ExpiryDate = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"]).ToString());
                        int storeid = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"]).ToString());
                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(Net) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice"))).ToString());

                    }
                    if (ColName == "CurrencyPrice")
                    {
                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net"))).ToString());

                    }
                    if (ColName == "Net")
                    {
                        decimal additonalVAlue = 0;
                        if (Comon.ConvertToDecimalPrice(val.ToString()) > 0)
                        {
                            //    decimal additonalVAlue=(Comon.ConvertToDecimalPrice(val.ToString())*Comon.ConvertToDecimalPrice( 5))/100;
                            // 
                          
                            decimal PriceUnit = 0;
                            bool HasVat = Comon.cbool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"]).ToString());
                            decimal Net = (Comon.ConvertToDecimalPrice(val.ToString()) + Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("Discount")));
                            if (HasVat == true && chkForVat.Checked && MySession.GlobalHaveVat == "1")
                                additonalVAlue = Comon.ConvertToDecimalPrice(Net - ((Net * 100) / (100 + MySession.GlobalPercentVat)));
                            else
                                additonalVAlue = 0;
                            
                             
                            string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                            
                            decimal CostPriceUnit = Comon.ConvertToDecimalPrice(gridView1.GetFocusedRowCellValue("CostPrice").ToString());
                            //if (Comon.ConvertToDecimalPrice(PriceUnit.ToString()) < CostPriceUnit)
                            //{

                            //   // e.Valid = false;
                            //   // HasColumnErrors = true;
                            //   // e.ErrorText = "لايمكن البيع باقل من سعر التكلفة ";
                            //    //gridView1.SetFocusedRowCellValue("Net", 0.ToString());

                            //}
                            //else
                            {
                                decimal Total = Comon.ConvertToDecimalPrice(Net) - additonalVAlue;
                                PriceUnit = Total / Comon.ConvertToDecimalQty(gridView1.GetFocusedRowCellValue("QTY"));
                                 
                                gridView1.SetFocusedRowCellValue("AdditionalValue", additonalVAlue.ToString());
                                gridView1.SetFocusedRowCellValue("SalePrice", PriceUnit.ToString());
                                gridView1.SetFocusedRowCellValue("Total", Total.ToString());
                                gridView1.SetFocusedRowCellValue("Net", val.ToString());
                            }
                        }
                    }
                    else if (ColName == ItemName)
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select ItemID from Stc_Items Where Cancel=0 and BranchID= " + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
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
                                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cInt(dtItemID.Rows[0]["ItemID"].ToString()));
                                //gridView1.Columns[SizeName].ColumnEdit = rSize;
                                //gridControl.RepositoryItems.Add(rSize);
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
                    //else if (ColName == SizeName)
                    //{
                    //    DataTable dtSize = Lip.SelectRecord("Select SizeID, " + PrimaryName + " AS " + SizeName + " from Stc_SizingUnits Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    //    if (dtSize.Rows.Count > 0)
                    //    {
                    //        var ItemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID");
                    //        if (ItemID != null)
                    //        {
                    //            DataTable dt = Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(ItemID.ToString()), Comon.cInt(dtSize.Rows[0]["SizeID"].ToString()), UserInfo.FacilityID);
                    //            if (dt.Rows.Count == 0)
                    //            {
                    //                e.Valid = false;
                    //                HasColumnErrors = true;
                    //                e.ErrorText = Messages.msgNoFoundSizeForItem;
                    //            }
                    //            else
                    //            {
                    //                if (MySession.GlobalAllowUsingDateItems)
                    //                {
                    //                    MySession.GlobalAllowUsingDateItems = false;
                    //                    FileItemData(dt);
                    //                    MySession.GlobalAllowUsingDateItems = true;
                    //                }
                    //                else
                    //                    FileItemData(dt);
                    //                e.Valid = true;
                    //                view.SetColumnError(gridView1.Columns[ColName], "");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            e.Valid = false;
                    //            HasColumnErrors = true;
                    //            e.ErrorText = Messages.msgInputIsRequired;
                    //            view.SetColumnError(gridView1.Columns["ItemID"], Messages.msgNoFoundSizeForItem);
                    //        }

                    //    }
                    //    else
                    //    {
                    //        e.Valid = false;
                    //        HasColumnErrors = true;
                    //        e.ErrorText = Messages.msgNoFoundSizeForItem;
                    //        view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundSizeForItem);

                    //    }
                    //}
                    if (ColName == SizeName)
                    {

                        string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID  and Stc_SizingUnits.BranchID=Stc_ItemUnits.BranchID Where UnitCancel=0 and Stc_ItemUnits.BranchID= " + MySession.GlobalBranchID+" And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                        DataTable dtBarCode = Lip.SelectRecord(Str);
                        if (dtBarCode.Rows.Count > 0)
                        {
                            gridView1.SetFocusedRowCellValue("SizeID", dtBarCode.Rows[0]["SizeID"]);
                            frmCadFactory.SetValuseWhenChangeSizeName(gridView1, Comon.cLong(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtBarCode.Rows[0]["SizeID"]), "Stc_TransferMultipleStoresMatirial_Details", "Stc_TransferMultipleStoresMatirial_Master", Comon.cDbl(txtStoreID.Text), Comon.cInt(txtInvoiceID.Text), "InvoiceID", FildNameQTY: "QTY");
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");
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
                    if (ColName == "CurrencyName")
                    {
                        DataTable dt = Lip.SelectRecord("Select ID ,ExchangeRate from Acc_Currency Where Cancel=0 and BranchID= " + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "')");
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyID", dt.Rows[0]["ID"]);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice", dt.Rows[0]["ExchangeRate"]);
                        if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net")) > 0)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Net"))).ToString());
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
                        if (ColName == "BarCode"  || ColName == "ItemID" || ColName == "SizeID" )
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
                        }
                            if (ColName == "QTY")
                            {
                                string BarCode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"]).ToString();
                                double RemindQty = 0;
                                RemindQty = Comon.cDbl(Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + BarCode + "'," + Comon.cInt(txtStoreID.Text) + ","+MySession.GlobalBranchID+") AS RemainQty").Rows[0]["RemainQty"].ToString());
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
                else if (e.KeyData == Keys.Delete)
                {
                    if (!IsNewRecord)
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                        if (!Yes)
                            return;

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
        /// <summary>
        ///  This method handles the "InvalidRowException" event of the gridView1 control.
        /// We set the "ExceptionMode" property of the event arguments to "NoAction" to prevent any actions when an invalid row is encountered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        /// <summary>
        /// This method handles the "CustomUnboundColumnData" event of the "gridView1" control.
        ///We set the value of the "e.Value" property to the value of "e.ListSourceRowIndex + 1" to display row numbers in the unbound column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                }
            }
        }

        private int GerPackage(string ToBarCode, int pakage, int ItemID, int SizeID, long ExpiryDate, double QtyItem, int CurrentID, string currentBarcode)
        {


            int flag = 0;
            var PackingQty = Lip.GetValue("Select  PackingQty from Stc_ItemUnits Where SizeID = " + SizeID + " And ItemID=" + ItemID + " and BranchID= " + MySession.GlobalBranchID);

            //    strSQL = "Select  Top(1) * from Stc_ItemUnits Where PackingQty > " + PackingQty + " And ItemID=" + ItemID;
            strSQL = "Select  Top(1) * from Stc_ItemUnits Where SizeID > " + SizeID + " And ItemID=" + ItemID + " and BranchID= " + MySession.GlobalBranchID;
            DataTable dtSizeId = new DataTable();
            dtSizeId = Lip.SelectRecord(strSQL);
            if (dtSizeId.Rows.Count > 0)
            {
            label1:
                var remQTy = Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + dtSizeId.Rows[0]["BarCode"].ToString() + "'," + Comon.cInt(txtStoreID.Text) + ","+MySession.GlobalBranchID+") AS RemainQty")).Rows[0]["RemainQty"].ToString());

                if (remQTy > 0)
                {

                    if (CurrentID == SizeID)
                    {
                        var remQTy1 = Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + ToBarCode + "'," + Comon.cInt(txtStoreID.Text) + "," + MySession.GlobalBranchID + ") AS RemainQty")).Rows[0]["RemainQty"].ToString());
                        if (remQTy1 - QtyItem >= 0)
                            flag = Comon.cInt(remQTy1);
                        else
                        {
                            var remQTy2 = Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + dtSizeId.Rows[0]["BarCode"].ToString() + "'," + Comon.cInt(txtStoreID.Text) + "," + MySession.GlobalBranchID + ") AS RemainQty")).Rows[0]["RemainQty"].ToString());
                            if (remQTy2 > 0)
                            { goto label1; }
                            else
                            {
                                GerPackage(dtSizeId.Rows[0]["BarCode"].ToString(), Comon.cInt(dtSizeId.Rows[0]["PackingQty"]), ItemID, Comon.cInt(dtSizeId.Rows[0]["SizeID"].ToString()), 0, QtyItem, CurrentID, currentBarcode);
                            }
                        }
                    }
                    else
                    {
                        GerPackage(currentBarcode, 0, ItemID, CurrentID, 0, QtyItem, CurrentID, currentBarcode);
                    }
                    //var remQTy1 = Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQty]('" + ToBarCode + "'," + Comon.cInt(txtStoreID.Text) + ") AS RemainQty")).Rows[0]["RemainQty"].ToString());
                    //if (remQTy1 - QtyItem > 0)
                    //    flag = Comon.cInt(remQTy1);
                    //else
                    //    goto label1;
                }
                else
                {
                    GerPackage(dtSizeId.Rows[0]["BarCode"].ToString(), Comon.cInt(dtSizeId.Rows[0]["PackingQty"]), ItemID, Comon.cInt(dtSizeId.Rows[0]["SizeID"].ToString()), 0, QtyItem, CurrentID, currentBarcode);
                }
            }
            return flag;


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
            //gridView1.RefreshRow(gridView1.FocusedRowHandle);
            //gridView1.RefreshRow(e.PrevFocusedRowHandle); // refresh the previous row
            //gridView1.RefreshRow(e.FocusedRowHandle); // refresh the current row          
            try
            {
                    if (e.FocusedRowHandle >= 0)
                    {
                        var ItemId = gridView1.GetRowCellValue(e.FocusedRowHandle, "ItemID").ToString();
                        var dtimg = Lip.SelectRecord("Select * from Stc_Items Where ItemID=" + ItemId + " and BranchID= " + MySession.GlobalBranchID);
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
                catch(Exception ex)
                {
                   // Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

                }
              

        }
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }
        decimal CostPriceOrder(string OrderID,decimal QTY)
        {
            decimal PriceGrame = Comon.cDec(Lip.GetValue("select  [QTYGram] FROM  [Menu_ProductionExpensesMaster] where  [OrderID]='" + OrderID + "' and BranchID=" + Comon.cInt(cmbBranchesID.EditValue) + "  and Cancel=0"));
             return   Comon.ConvertToDecimalPrice( Comon.cDec(PriceGrame)*QTY);
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
                int IsService = Comon.cInt(Lip.GetValue("SELECT  [IsService] FROM  [Stc_Items] where [Cancel]=0 and BranchID= " + MySession.GlobalBranchID+" and [ItemID]=" + Comon.cInt(dt.Rows[0]["ItemID"].ToString())));
                decimal totalQtyBalance = 0;
                if ( IsService!=1)
                {
                    totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreID.Text));
                    {
                        decimal qtyCurrent = 0;
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Sales_SalesInvoiceDetails", "Sales_SalesInvoiceMaster", "QTY", "InvoiceID", Comon.cInt(txtInvoiceID.Text), dt.Rows[0]["ItemID"].ToString(), " ", SizeID: Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                        qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(gridView1, "QTY", 0, dt.Rows[0]["ItemID"].ToString(), Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                        totalQtyBalance += QtyInCommand;
                        totalQtyBalance -= qtyCurrent;
                    }
                    if (totalQtyBalance <= 0)
                    {
                        if (MySession.AllowOutQtyNegative)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore);
                            return;
                        }
                        bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore + "هل تريد المتابعة ...");
                        if (!yes)
                            return;
                    }
                }
                if (MySession.AllowNotShowQTYInQtyField == false)
                    totalQtyBalance = 0;
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], totalQtyBalance);               
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], "1");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0][PrimaryName].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngItemName"], dt.Rows[0]["ItemName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SalePrice"], dt.Rows[0]["SalePrice"].ToString());
                        
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], 0);
                //if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Serials")==null|| Comon.cDbl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Serials")) <= 0)
                {
                    decimal AverageCost = Comon.cDec(Lip.AverageUnit(Comon.cInt(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreID.Text)));
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], Comon.cDec(AverageCost));
                }
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Serials") != null && Comon.cDbl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Serials")) > 0)
                {    
                    decimal QtyGram=0;
                    if (Comon.cInt(dt.Rows[0]["SizeID"].ToString()) == 2)//قيراط
                        QtyGram = Comon.cDec(Comon.cDec(dt.Rows[0]["QTY"].ToString()) / 5);
                    else
                        QtyGram = Comon.cDec(dt.Rows[0]["QTY"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CostPrice"], CostPriceOrder(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Serials").ToString(),QtyGram));
                }
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Color"], dt.Rows[0]["Color"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CLARITY"], dt.Rows[0]["CLARITY"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"],cmbCurency.Text.ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyEquivalent"],Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice( txtCurrncyPrice.Text)*Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString())));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
                try
                {
                    if (DBNull.Value != dt.Rows[0]["ItemImage"])
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemImage"], dt.Rows[0]["ItemImage"]);
                }
                catch (Exception ){ }
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Discount"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Bones"], 0);
                decimal AdditionalValue =0;
                if (MySession.GlobalHaveVat == "1")
                    AdditionalValue = Comon.ConvertToDecimalPrice(Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()) / 100 * MySession.GlobalPercentVat);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AdditionalValue"], AdditionalValue);
                decimal Net = Comon.ConvertToDecimalPrice(AdditionalValue + Comon.ConvertToDecimalPrice(dt.Rows[0]["SalePrice"].ToString()));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Cancel"], 0); 
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], dt.Rows[0]["ItemName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PageNo"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemStatus"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Caliber"], dt.Rows[0]["Caliber"]);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Equivalen"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Net"], Net);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["TheCount"], 1);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Height"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Width"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Total"], dt.Rows[0]["SalePrice"].ToString());
              
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HavVat"], true);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RemainQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DIAMOND_W"], dt.Rows[0]["DIAMOND_W"]);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["STONE_W"], dt.Rows[0]["STONE_W"]); 
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BAGET_W"], dt.Rows[0]["BAGET_W"]);
                var ItemId = dt.Rows[0]["ItemID"].ToString();
                var dtimg = Lip.SelectRecord("Select * from Stc_Items Where ItemID=" + ItemId + " and BranchID= " + MySession.GlobalBranchID);
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
        private void EnabledControlMaster(bool Value)
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

            if (Value)
                RolesButtonSearchAccountID();
            cmbFormPrinting.Enabled = true;
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
                    if (col.FieldName == "BarCode"  || col.FieldName == "SizeID" || col.FieldName == "ItemID"  )
                    {

                        var cellValue = gridView1.GetRowCellValue(i, col); ;

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired  );
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
                decimal QTY18 = 0;
                decimal QTY21 = 0;

                decimal QTY22 = 0;
                decimal QTY24 = 0;

                decimal InvoiceTotalGold = 0;
                decimal InvoiceTotalZircon = 0;
                decimal TotalDiamondCustomer = 0;
                decimal SalePriceRow = 0;
                decimal TotalRow = 0;
                decimal NetRow = 0;
                decimal TotalBeforeDiscountRow = 0;
                decimal AdditionalAmountRow = 0;

                decimal TotalDaimond_W = 0;
                decimal TotalStown_W = 0;
                decimal TotalBagat_W = 0;


                bool HavVatRow = false;
              
                MySession.UseNetINInvoiceSales = 1;
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    int Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                    QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);

                    //TotalDaimond_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DIAMOND_W"));
                    //TotalStown_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "STONE_W"));
                    //TotalBagat_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "BAGET_W"));



                    if (Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net")) > 0 && MySession.UseNetINInvoiceSales == 1)
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                        if (MySession.GlobalHaveVat == "1")
                            AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                        TotalBeforeDiscountRow = TotalRow;
                    }
                    else
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                        if (MySession.GlobalHaveVat == "1")
                           AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);
                    }

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

                    int BaseID = Comon.cInt(Lip.GetValue("SELECT  [BaseID] FROM  [Stc_Items] where [ItemID]=" + gridView1.GetRowCellValue(i, "ItemID").ToString() + "  and [Cancel]=0 and BranchID= " + MySession.GlobalBranchID));
                    int TypeID = Comon.cInt(Lip.GetValue("SELECT  [TypeID] FROM  [Stc_Items] where [ItemID]=" + gridView1.GetRowCellValue(i, "ItemID").ToString() + "  and [Cancel]=0 and BranchID= " + MySession.GlobalBranchID));
                    if(BaseID==4&&TypeID!=1)
                        InvoiceTotalZircon += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    else  if (BaseID == 5||(BaseID==4&&TypeID!=1))
                        InvoiceTotalGold += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    else if ((BaseID > 0 && BaseID < 4) || BaseID==11)
                        TotalDaimond_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());

                    int isServec = Comon.cInt(Lip.GetValue("SELECT IsService  FROM   [Stc_Items] where [ItemID]=" + gridView1.GetRowCellValue(i, "ItemID").ToString() + " and BranchID= " + MySession.GlobalBranchID+" and [Cancel]=0"));
                    if(isServec==1&& ((BaseID > 0 && BaseID < 4) || BaseID==11))
                        TotalDiamondCustomer += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                }

                if (rowIndex < 0)
                {
                    var ResultCaliber = Comon.cInt(gridView1.GetRowCellValue(rowIndex, SizeName));
                    var ResultQTY = gridView1.GetRowCellValue(rowIndex, "QTY");
                    var ResultSalePrice = gridView1.GetRowCellValue(rowIndex, "SalePrice");
                    var ResultDiscount = gridView1.GetRowCellValue(rowIndex, "Discount");
                    var ResultHavVat = gridView1.GetRowCellValue(rowIndex, "HavVat");
                
                    TotalDaimond_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "DIAMOND_W"));
                    TotalStown_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "STONE_W"));
                    TotalBagat_W += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "BAGET_W"));


                    QTYRow = ResultQTY != null ? Comon.ConvertToDecimalPrice(ResultQTY.ToString()) : 0;
                    SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    if (Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net")) > 0 && MySession.UseNetINInvoiceSales == 1)
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Total"));
                        if (MySession.GlobalHaveVat == "1")
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
                        QTY18 = QTY18 + Comon.ConvertToDecimalPrice(ResultQTY);
                    if (ResultCaliber == 21)
                        QTY21 = QTY21 + Comon.ConvertToDecimalPrice(ResultQTY); 

                    if (ResultCaliber == 22)
                        QTY22 = QTY22 + Comon.ConvertToDecimalPrice(ResultQTY); 
                    if (ResultCaliber == 24)
                        QTY24 = QTY24 + Comon.ConvertToDecimalPrice(ResultQTY); 


                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscount += TotalBeforeDiscountRow;
                    TotalAfterDiscount += TotalRow;
                    DiscountTotal += DiscountRow;
                    AdditionalAmount += AdditionalAmountRow;
                    Net += NetRow;
                    var totaldiamondindex="";
                    var totalGoldindex = "";
                    var TotalDiamondindex = "";
                    var TotalZirconindex = "";
                    var itemid = "";
                    if (gridView1.GetRowCellValue(rowIndex, "ItemID") != null)
                    {
                        itemid = gridView1.GetRowCellValue(rowIndex, "ItemID").ToString();
                        var BaseID = Comon.cInt(Lip.GetValue("SELECT  [BaseID] FROM  [Stc_Items] where [ItemID]=" + itemid + "  and [Cancel]=0 and BranchID= " + MySession.GlobalBranchID));
                        var TypeID = Comon.cInt(Lip.GetValue("SELECT  [TypeID] FROM  [Stc_Items] where [ItemID]=" + itemid + "  and [Cancel]=0 and BranchID= " + MySession.GlobalBranchID));


                        if (BaseID == 5 || (BaseID == 4 && TypeID != 1))
                            totalGoldindex = gridView1.GetRowCellValue(rowIndex, "QTY").ToString();
                        else if (BaseID == 4 && TypeID != 1)
                            TotalZirconindex = (gridView1.GetRowCellValue(rowIndex, "QTY").ToString());

                        else if ((BaseID > 0 && BaseID < 4) || BaseID == 11)
                            totaldiamondindex = gridView1.GetRowCellValue(rowIndex, "QTY").ToString();
                        int isServec = Comon.cInt(Lip.GetValue("SELECT [IsService]  FROM   [Stc_Items] where [ItemID]=" + itemid + " and [Cancel]=0 and BranchID= " + MySession.GlobalBranchID));
                        if (isServec == 1 && ((BaseID > 0 && BaseID < 4) || BaseID == 11))
                            TotalDiamondindex = gridView1.GetRowCellValue(rowIndex, "QTY").ToString();

                    }
                    TotalDaimond_W += Comon.ConvertToDecimalPrice(totaldiamondindex);
                    InvoiceTotalGold += Comon.ConvertToDecimalPrice(totalGoldindex);
                    TotalDiamondCustomer += Comon.ConvertToDecimalPrice(TotalDiamondindex);
                    InvoiceTotalZircon += Comon.ConvertToDecimalPrice(TotalZirconindex);
                }
                lblUnitDiscount.Text = DiscountTotal.ToString("N" + MySession.GlobalPriceDigits);
                DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
                lblDiscountTotal.Text = (DiscountTotal + DiscountOnTotal).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotalBeforeDiscount.Text = Comon.ConvertToDecimalPrice(TotalBeforeDiscount).ToString("N" + MySession.GlobalPriceDigits);
                lblInvoiceTotal.Text = (Comon.ConvertToDecimalPrice(TotalAfterDiscount) - Comon.ConvertToDecimalPrice(DiscountOnTotal)).ToString("N" + MySession.GlobalPriceDigits);
                if (chkForVat.Checked == false)
                    AdditionalAmount = 0;
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
                Eq = Comon.ConvertTo21Caliber(QTY18, 18);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY21, 21);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY22, 22);
                Eq = Eq + Comon.ConvertTo21Caliber(QTY24, 24);

                lblInvoiceTotalGold.Text = Comon.ConvertToDecimalQty(Eq).ToString("N" + MySession.GlobalQtyDigits);

                lbl18.Text = Comon.ConvertToDecimalQty(QTY18).ToString("N" + MySession.GlobalQtyDigits);
                lbl21.Text = Comon.ConvertToDecimalQty(QTY21).ToString("N" + MySession.GlobalQtyDigits);

                lbl22.Text = Comon.ConvertToDecimalQty(QTY22).ToString("N" + MySession.GlobalQtyDigits);
                lbl24.Text = Comon.ConvertToDecimalQty(QTY24).ToString("N" + MySession.GlobalQtyDigits);


                lblTotalDaimond.Text = TotalDaimond_W.ToString("N" + MySession.GlobalQtyDigits);
                 

                lblInvoiceTotalGold.Text = InvoiceTotalGold.ToString();
                lblTotalDaimond.Text = TotalDaimond_W.ToString();
                lblTotalDiamondCustomer.Text = TotalDiamondCustomer.ToString();
                lblTotalZircon.Text = InvoiceTotalZircon.ToString();
                int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID= " + MySession.GlobalBranchID));
                if (isLocalCurrncy > 1)
                {
                    decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID= " + MySession.GlobalBranchID));
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
        private void SumTotalBalanceAndDiscount1(int row = -1, bool IsHavVat = false)
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
                MySession.UseNetINInvoiceSales = 1;
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                    SalePriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount"));
                    HavVatRow = row == i ? IsHavVat : Comon.cbool(gridView1.GetRowCellValue(i, "HavVat"));
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    if (Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net")) > 0 && MySession.UseNetINInvoiceSales == 1)
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total"));
                        if (MySession.GlobalHaveVat == "1")
                           AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue")) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net"));
                        TotalBeforeDiscountRow = TotalRow;
                    }
                    else
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow - DiscountRow);
                        AdditionalAmountRow = HavVatRow == true ? Comon.ConvertToDecimalPrice((TotalRow) / 100 * MySession.GlobalPercentVat) : 0;
                        NetRow = Comon.ConvertToDecimalPrice(TotalRow + AdditionalAmountRow);
                    }
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
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
                    SalePriceRow = ResultSalePrice != null ? Comon.ConvertToDecimalPrice(ResultSalePrice.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                    HavVatRow = ResultDiscount != null ? Comon.cbool(ResultHavVat.ToString()) : false;
                    if (MySession.GlobalHaveVat == "1")
                       AdditionalAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "AdditionalValue"));
                    NetRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net"));
                    TotalBeforeDiscountRow = Comon.ConvertToDecimalPrice(QTYRow * SalePriceRow);
                    if (Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Net")) > 0 && MySession.UseNetINInvoiceSales == 1)
                    {
                        TotalRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowIndex, "Total"));
                        if (MySession.GlobalHaveVat == "1")
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

            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                frmCustomers frm = new frmCustomers();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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
                        frm.Dispose();
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
                 if (!MySession.GlobalAllowChangefrmSaleCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtOrderID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtOrderID, null, "SalseOrder", "رقم الطلب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtOrderID, null, "SalseOrder", "Invoice ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtCostSalseAccountID.Name)
            {
                // if (!MySession.GlobalAllowChangefrmSaleCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostSalseAccountID,lblCostSalseAccountName  , "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostSalseAccountID, lblCostSalseAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() ==txtSalesRevenueAccountID.Name)
            {
                // if (!MySession.GlobalAllowChangefrmSaleCustomerID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtSalesRevenueAccountID,lblSalesRevenueAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtSalesRevenueAccountID, lblSalesRevenueAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtStoreID.Name)
            { 
                if (!MySession.GlobalAllowChangefrmSaleStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
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
                    PrepareSearchQuery.Find(ref cls, lblDebitAccountID,lblDebitAccountName , "AccountID", "رقم الحساب", MySession.GlobalBranchID);
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
                // if (gridView1.FocusedColumn == null) return;

                if (gridView1.FocusedColumn.Name == "colDIAMOND_W")
                {
                    frmPurchaseDaimondDetils frm = new frmPurchaseDaimondDetils();
                    frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                    frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                    frm.ReadData(txtInvoiceID.Text, gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BarCode").ToString(), gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DIAMOND_W").ToString(), txtStoreID.Text, txtCustomerID.Text, Comon.cInt(cmbBranchesID.EditValue));
                    frm.ReadRecord(2,1);
                    frm.Show();
                }
                if(gridView1.FocusedColumn.Name =="colSerials")
                {
                    
              
                     int commandCastingID = Comon.cInt(Lip.GetValue(" select [CommandID] FROM  [Manu_CastingOrders] where [BranchID]=" + Comon.cInt(cmbBranchesID.EditValue) + "  and [Cancel]=0 and [OrderID]='" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"])+ "'"));
                     if (commandCastingID>0)
                     {
                         frmManuExpencessOrder frm = new frmManuExpencessOrder();
                         if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                         {
                             if (UserInfo.Language == iLanguage.English)
                                 ChangeLanguage.EnglishLanguage(frm);
                             frm.Show();
                             frm.SetDataToShow(commandCastingID.ToString(), gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Serials"]).ToString());

                         }
                         else
                             frm.Dispose();    
                       }
                   
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
                        if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID") != null)
                           frm.SetValueToControl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
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
            GetSelectedSearchValue(cls);
        }
        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

            if ( BarcodeIsValid(e.RowHandle))
            {
                e.Appearance.BackColor = Color.Goldenrod;
            }
            else
            {
                // Set the default background color of the row
                e.Appearance.BackColor = e.Appearance.BackColor;
            }
        }
         

        private bool BarcodeIsValid(int rowHandle)
        {
            string Barcode="";
            object cellValue = gridView1.GetRowCellValue(rowHandle, "BarCode");
            if (cellValue != null)
            {
                Barcode = cellValue.ToString();
                
            }
            if (Barcode.Length < 3) // make sure the barcode is at least 3 characters long
            {
                return false;
            }

            if (Barcode.Substring(0, 1) != "Z") // make sure the first character is "Z"
            {
                return false;
            }

            if (!Char.IsLetter(Barcode.Substring(1, 2)[0])) // make sure the second and third characters are letters
            {
                return false;
            }

            return true; // if all conditions pass, the barcode is valid
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
                else if (FocusedControl == txtOrderID.Name)
                {
                    txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                    txtOrderID_Validating(null, null);
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
                        
                    
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));
                        CalculateRow();
                        gridView1.Columns["SalePrice"].OptionsColumn.ReadOnly = !MySession.GlobalCanChangeInvoicePrice;
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
                    dt = Sales_SaleInvoicesDAL.frmGetDataDetalByID(InvoiceID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        btnMachinResraction.Visible = true;
                        txtDailyID.Text = dt.Rows[0]["DailyID"].ToString();
                        IsNewRecord = false;
                        txtCommandOrder.Text = dt.Rows[0]["CommandOrderID"].ToString();
                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurencyID"].ToString());

                        txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        txtSalesRevenueAccountID.Text = dt.Rows[0]["SalesRevenueAccountID"].ToString();
                        txtSalesRevenueID_Validating(null, null);
                        txtCostSalseAccountID.Text = dt.Rows[0]["CostSalseAccountID"].ToString();
                        txtCostSalseID_Validating(null, null);
                        lblNetAccountID.Text = dt.Rows[0]["NetAccount"].ToString();
                        lblNetAccountID_Validating(null, null);
                        //Validate
                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);

                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        StopSomeCode = true;
                        cmbMethodID.EditValue = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());
                        StopSomeCode = false;
                        MethodID = Comon.cInt(dt.Rows[0]["MethodeID"].ToString());

                        simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                        simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                        simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
                        txtRegTime.Text = Comon.ConvertSerialToTime(dt.Rows[0]["RegTime"].ToString());
                        cmbStatus.EditValue = Comon.cInt(Comon.cInt(dt.Rows[0]["Posted"].ToString()));


                        if (MethodID == 1)
                        {
                            simpleButton1.Appearance.BackColor = Color.Goldenrod;
                            simpleButton1.Appearance.BackColor2 = Color.White;
                            simpleButton1.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
                        }

                        if (MethodID == 2)
                        {
                            simpleButton12.Appearance.BackColor = Color.Goldenrod;
                            simpleButton12.Appearance.BackColor2 = Color.White;
                            simpleButton12.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
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


                        
                        cmbNetType.EditValue = Comon.cDbl(dt.Rows[0]["NetType"].ToString());
                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);
                        txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
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


                        //Masterdata
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();
                        txtCustomerMobile.Text = dt.Rows[0]["CustomerMobile"].ToString();

                        //Date
                        //Date
                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()) == "")
                            txtInvoiceDate.Text = "";
                        else
                            txtInvoiceDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["InvoiceDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

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
                        lblInvoiceTotalBeforeDiscount.Text = dt.Rows[0]["DiscountOnTotal"].ToString();
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
                        cmbStatus.EditValue = Comon.cInt(Comon.cInt(dt.Rows[0]["Posted"].ToString()));

                        //GridVeiw

                        gridControl.DataSource = dt;



                        // gridControl1.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;

                        CalculateRow();

                        Validations.DoReadRipon(this, ribbonControl1);

                        //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtInvoiceID.Text;
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
                if (string.IsNullOrEmpty(MySession.GlobalDefaultSaleCreditAccountID) == false)    
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
                {
                    lblAdditionalAccountID.Text = MySession.GlobalDefaultSalesAddtionalAccountID;
                    lblAdditionalAccountID_Validating(null, null);
                }
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
            catch (Exception ex)
            {

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
                txtOrderID.Text = "";
                lblCurrncyPric.Text = "";
                lblCurrencyEqv.Text = "";
                txtSalesRevenueAccountID.Text = "";
                txtCostSalseAccountID.Text = "";
                lblCostSalseAccountName.Text = "";
                lblSalesRevenueAccountName.Text = "";
                btnMachinResraction.Visible = false;
              
                txtCustomerName.Text = "";
                lblInvoiceTotalGold.Text = "";
                chkNoSale.Checked = false;
                DiscountCustomer = 0;
                txtPaidAmount.Text = "";
                lblRemaindAmount.Text = "";
                txtVatID.Text = "";
                txtDocumentID.Text = "";
                txtCustomerID.Text = "";
                txtDelegateID.Text = "";
                lblCustomerName.Text = "";
                lblDelegateName.Text = "";
                txtNotes.Text = "";
                lbl21.Text = "0";
                lbl18.Text = "0";
                lbl24.Text = "0";
                lbl22.Text = "0";

                lblInvoiceTotalGold.Text = "0";
                /////////////////////////////
                txtCustomerID.Tag = " ";
                txtNetProcessID.Tag = " ";
                cmbBank.Tag = " ";
                cmbNetType.Tag = " ";
                txtNetAmount.Tag = " ";
                txtCheckID.Tag = " ";
                /////////////////////////////////////////////////
               
                InitializeFormatDate(txtInvoiceDate);
                InitializeFormatDate(txtWarningDate);
                InitializeFormatDate(txtCheckSpendDate);
                txtInvoiceDate.ReadOnly = !MySession.GlobalAllowChangefrmSaleInvoiceDate;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                lblNetAccountID.Text = "";
                lblNetAccountID_Validating(null, null);
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
                txtCustomerMobile.Text = "";
                GetAccountsDeclaration();
                txtEnteredByUserID.Text = UserInfo.ID.ToString();
                txtEnteredByUserID_Validating(null, null);
              
                try
                {
                    txtEditedByUserID.Text = UserInfo.ID.ToString();
                    txtEditedByUserID_Validating(null, null);
                    txtDelegateID.Text = MySession.GlobalDefaultSaleDelegateID;
                    txtDelegateID_Validating(null, null);

                    txtCostCenterID.Text = MySession.GlobalDefaultSaleCostCenterID;
                    txtCostCenterID_Validating(null, null);
                    txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmSaleCostCenterID;
                    txtSellerID.Text = MySession.GlobalDefaultSaleSellerID;

                    txtSellerID_Validating(null, null);
                    txtStoreID.Text = MySession.GlobalDefaultSaleStoreID;

                    txtStoreID_Validating(null, null);


                    if (MySession.GlobalDefaultSalePayMethodID != "0")
                        cmbMethodID.EditValue = Comon.cInt(MySession.GlobalDefaultSalePayMethodID);
                    else
                        cmbMethodID.EditValue = 1;
                    if (!MySession.GlobalAllowChangefrmSalePayMethodID)
                        switch (Comon.cInt(MySession.GlobalDefaultSalePayMethodID))
                        {
                            case 1:

                                simpleButton3.Enabled = false;
                                simpleButton2.Enabled = false;
                                simpleButton12.Enabled = false;
                                break;
                            case 2:
                                simpleButton3.Enabled = false;
                                simpleButton2.Enabled = false;
                                simpleButton1.Enabled = false;
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
                picItemImage.Image = null;

                lstDetail = new BindingList<Sales_SalesInvoiceDetails>();
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
                    strSQL = "SELECT TOP 1 * FROM " + Sales_SaleInvoicesDAL.TableName + " Where Cancel =0   And BranchID= " + Comon.cInt(cmbBranchesID.EditValue) + " and GoldUsing=" + GoldUsing;
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
                txtInvoiceID.Text = Sales_SaleInvoicesDAL.GetNewID(MySession.GlobalFacilityID, MySession.GlobalBranchID, MySession.UserID).ToString();
                txtRegistrationNo.Text = txtInvoiceID.Text;
                txtDailyID.Text = GetNewDialyID(MySession.GlobalFacilityID, MySession.GlobalBranchID, Comon.cInt(txtCostCenterID.Text)).ToString();
                ClearFields();
                IdPrint = false;
                EnabledControl(true);
                cmbFormPrinting.EditValue = 1;
                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
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
            EnabledControlMaster(true);
            Validations.DoEditRipon(this, ribbonControl1);
        }
         
        protected override void DoSave()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCommandOrder.Text) )
                {
                    bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لم يتم ادخال رقم الطلبية .. هل تريد ادخال الرقم؟" : "The Order Id is Empty .. Are you Entre ID?");
                    if (yes)
                        return;
                }
            
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
                if (!Validations.IsValidFormCmb(cmbCurency))
                    return;

              
                if (!Lip.CheckTheProcessesIsPosted("Sales_SalesInvoiceMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtInvoiceID.Text)))
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
                //            returned.WeightIn = 0;
                //            returned.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                //            returned.WeightOut = Comon.ConvertToDecimalQty(dtt.Rows[j]["WeightIn"]);
                //            returned.TypeOpration = 3;
                //            returned.CaptionOpration = "فاتورة  مبيعات";
                //            returned.PriceCarat = Comon.cDec(dtt.Rows[j]["PriceCarat"]);
                //            returned.TotalPrice = Comon.cDec(dtt.Rows[j]["TotalPrice"]);
                //            returned.SupplierID = Comon.cDbl(txtCustomerID.Text);
                //            returned.StoreID = Comon.cInt(txtStoreID.Text);

                //            if (returned.WeightOut <= 0 || returned.StoreID <= 0 || (returned.PriceCarat <= 0 && returned.TotalPrice <= 0) || returned.ItemID <= 0)
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

                if (Lip.CheckTheCustomerAllowAgeDebtOrNot(Comon.cDbl(txtCustomerID.Text), MySession.GlobalBranchID) == 1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, "لا يمكن الحفظ لسبب تجاوز عمر المديونية للعميل " + txtCustomerName.Text + " ولم يتم السداد");
                    return;
                }
                else  if (Lip.CheckTheCustomerAllowAgeDebtOrNot(Comon.cDbl(txtCustomerID.Text), MySession.GlobalBranchID) == 2)
                {
                    SplashScreenManager.CloseForm(false);
                   bool Yes= Messages.MsgQuestionYesNo(Messages.TitleInfo, "لقد تجاوز عمر المديونية للعميل " + txtCustomerName.Text + " ولم يتم السداد ... هل تريد متابعة الحفظ ");
                   if(!Yes)
                    return;
                }

                //Customer
                if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(txtCustomerID.Text), MySession.GlobalBranchID, Comon.cDec(lblNetBalance.Text), 1)==1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + txtCustomerName.Text);
                    return;
                }
                else
                    if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(txtCustomerID.Text), MySession.GlobalBranchID, Comon.cDec(lblNetBalance.Text), 1) ==2)
                    {
                        SplashScreenManager.CloseForm(false);
                        bool Yes = Messages.MsgQuestionYesNo(Messages.TitleInfo, Messages.msgAccountMaxLimitSaveOrNot + " " + txtCustomerName.Text);
                        if (!Yes)
                            return;
                    }
                //box Cash
                if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblDebitAccountID.Text), MySession.GlobalBranchID, Comon.cDec(lblNetBalance.Text), 1)==1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + lblDebitAccountName.Text);
                    return;
                }
                else
                    if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblDebitAccountID.Text), MySession.GlobalBranchID, Comon.cDec(lblNetBalance.Text), 1)==2)
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
                else
                    if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblDiscountDebitAccountID.Text), MySession.GlobalBranchID, Comon.cDec(lblDiscountTotal.Text), 1) == 2)
                    {
                        SplashScreenManager.CloseForm(false);
                        bool Yes = Messages.MsgQuestionYesNo(Messages.TitleInfo, Messages.msgAccountMaxLimitSaveOrNot + " " + lblDiscountDebitAccountName.Text);
                        if (!Yes)
                            return;
                    }
                //Net Account
                if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblNetAccountID.Text), MySession.GlobalBranchID, Comon.cDec(lblNetBalance.Text), 1)==1)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + lblNetAccountName.Text);
                    return;
                }
                else if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(lblNetAccountID.Text), MySession.GlobalBranchID, Comon.cDec(lblNetBalance.Text), 1) ==2)
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

                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
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
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                int IsService = Comon.cInt(Lip.GetValue("SELECT  [IsService] FROM  [Stc_Items] where [Cancel]=0 and BranchID= " + MySession.GlobalBranchID+" and [ItemID]=" + Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString())));
                if (IsService != 1)
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
                    returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID+" and BranchID= " + MySession.GlobalBranchID));
                    returned.QTY = Comon.cDbl(gridView1.GetRowCellValue(i, "QTY").ToString());
                    returned.OutPrice = Comon.cDbl(Comon.cDbl(gridView1.GetRowCellValue(i, "CostPrice").ToString()));
                    //returned.Bones = Comon.cDbl(gridView1.GetRowCellValue(i, "Bones").ToString());
                    returned.InPrice = 0;
                    returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                    returned.Cancel = 0;
                    listreturned.Add(returned);
                }
            }
            if (listreturned.Count > 0)
            {
                objRecord.ObjDatails = listreturned;
                string Result = Stc_ItemsMoviingDAL.Insert(objRecord, IsNewRecord);
                return Comon.cInt(Result);
            }
            else
                return -1;
            return 0;
        }
        private void Save()
        {
            gridView1.Focus();
            gridView1.MoveLastVisible();           
            if (DiscountCustomer != 0)
            {
                txtDiscountPercent.Text = DiscountCustomer.ToString();
                txtDiscountPercent_Validating(null, null);
            }
            CalculateRow();
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            txtInvoiceDate_EditValueChanged(null, null);
            Sales_SalesInvoiceMaster objRecord = new Sales_SalesInvoiceMaster();
            objRecord.InvoiceID = 0;
            objRecord.CommandOrderID = txtCommandOrder.Text.ToString();
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.CostSalseAccountID = Comon.cDbl(txtCostSalseAccountID.Text);
            objRecord.SalesRevenueAccountID = Comon.cDbl(txtSalesRevenueAccountID.Text);
            objRecord.CustomerMobile = txtCustomerMobile.Text;
            objRecord.InvoiceDate = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
            objRecord.DailyID = Comon.cInt(txtDailyID.Text);
            objRecord.MethodeID = Comon.cInt(cmbMethodID.EditValue);
            objRecord.OrderID = Comon.cInt(txtOrderID.Text);
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            objRecord.NetType = Comon.cDbl(cmbNetType.EditValue);
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            if (lblCustomerName.Text.Trim()==string.Empty)
                objRecord.CustomerName = txtCustomerName.Text.Trim();
            else
            objRecord.CustomerName = lblCustomerName.Text.Trim();

            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.DelegateID = Comon.cDbl(txtDelegateID.Text);

            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.SellerID = Comon.cInt(txtSellerID.Text);


            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Sale  Invoice" : "فاتوره  مبيعات ");
            txtNotes.Text = (txtNotes.Text.Trim());
            objRecord.Notes = txtNotes.Text;


            //Account
            objRecord.DebitAccount = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.CreditAccount = Comon.cDbl(lblCreditAccountID.Text);
            objRecord.DiscountDebitAccount = Comon.cDbl(lblDiscountDebitAccountID.Text);
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
            objRecord.DiscountOnTotal = Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text);
            objRecord.InvoiceTotal = (Comon.ConvertToDecimalPrice(lblInvoiceTotalBeforeDiscount.Text));
            objRecord.AdditionaAmountTotal = Comon.ConvertToDecimalPrice(lblAdditionaAmmount.Text);
            objRecord.NetBalance = Comon.ConvertToDecimalPrice(lblNetBalance.Text);

            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);

            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;
            objRecord.RemaindAmount = Comon.ConvertToDecimalPrice(lblRemaindAmount.Text);
            objRecord.PaidAmount = Comon.ConvertToDecimalPrice(txtPaidAmount.Text);
            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";
            objRecord.GoldUsing = 1;
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
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Sales_SalesInvoiceDetails();
                returned.ID = i;
                returned.FacilityID = UserInfo.FacilityID;
                returned.Caliber = Comon.cInt(gridView1.GetRowCellValue(i, SizeName).ToString());
                //returned.Serials = gridView1.GetRowCellValue(i, "Serials").ToString();
                //returned.Color = gridView1.GetRowCellValue(i, "Color").ToString();
                //returned.CLARITY = gridView1.GetRowCellValue(i, "CLARITY").ToString();
                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "QTY").ToString());
                //returned.STONE_W = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "STONE_W").ToString());
                //returned.BAGET_W = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "BAGET_W").ToString());
                //returned.DIAMOND_W = Comon.ConvertToDecimalQty(gridView1.GetRowCellValue(i, "DIAMOND_W").ToString());
                returned.Equivalen = Comon.ConvertToDecimalQty(Comon.ConvertTo21Caliber(returned.QTY, returned.Caliber, 18));
                returned.SalePrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString()); 
                returned.Bones = Comon.cInt(gridView1.GetRowCellValue(i, "Bones").ToString());
                returned.Description = gridView1.GetRowCellValue(i, "Description").ToString();
                //returned.CurrencyID = Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString());
                //returned.CurrencyName = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                //returned.CurrencyPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                //returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                if (gridView1.GetRowCellValue(i, "StoreID") == null)
                    returned.StoreID = Comon.cDbl(txtStoreID.Text);
                else
                    returned.StoreID = Comon.cDbl(gridView1.GetRowCellValue(i, "StoreID").ToString());
                returned.Discount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                returned.ExpiryDateStr = 0;
                returned.DateFirstStr = 0;
                returned.CostPrice = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CostPrice").ToString());
                if (MySession.GlobalHaveVat == "1")
                    returned.AdditionalValue = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "AdditionalValue").ToString());
                returned.Net = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Net").ToString());
                returned.Total = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                if (returned.AdditionalValue == 0)
                    returned.HavVat = false;
                else
                    returned.HavVat = true;

                returned.Cancel = 0; 
                if (returned.StoreID <= 0  || returned.SizeID <= 0 || returned.ItemID <= 0)
                {
                    Messages.MsgInfo("يرجى التاكد من بيانات الصنف ", returned.BarCode);
                    return;
                }

                listreturned.Add(returned);

            }
            if (listreturned.Count > 0)
            {
                objRecord.SaleDatails = listreturned;
                string Result = Sales_SaleInvoicesDAL.InsertUsingXML(objRecord, IsNewRecord);

                if (Comon.cInt(cmbStatus.EditValue) > 1)
                {
                    // حفظ الحركة المخزنية 
                    if (Comon.cInt(Result) > 0)
                    {
                        int MoveID = SaveStockMoveing(Comon.cInt(Result));
                        if (MoveID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
                    }
                    txtInvoiceID.Text = Result.ToString();
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
                            Lip.ExecututeSQL("Update " + Sales_SaleInvoicesDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sales_SaleInvoicesDAL.PremaryKey + " = " + txtInvoiceID.Text + " and BranchID= " + MySession.GlobalBranchID);
                        }
                    }
                }

                SplashScreenManager.CloseForm(false);
                if (IsNewRecord == true)
                {
                    if (Comon.cInt(Result) > 0)
                    {

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        Validations.DoLoadRipon(this, ribbonControl1);
                        if (falgPrint == true)
                        {
                            IsNewRecord = false;
                            txtInvoiceID.Text = Result.ToString();
                            DoPrint();
                            DoNew();
                        }
                        DoNew();
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
                        DoNew();
                        //if (Comon.cInt(cmbMethodID.EditValue) == 5)
                        //SaveVariousVoucher();

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
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired  );
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

                Sales_SalesInvoiceMaster model = new Sales_SalesInvoiceMaster();
                model.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
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
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.ToString();

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

                rptForm.Parameters["NumbToWord"].Value = Lip.ToWords(Convert.ToDecimal(lblNetBalance.Text.Trim().ToString()), Comon.cInt(Lip.GetValue("SELECT TAFQEETID FROM Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue))));


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
                    if (MySession.GlobalHaveVat == "1")
                        row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    else
                        row["AdditionalValue"] = 0;
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
                if (Comon.cInt(cmbFormPrinting.EditValue) == 2)
                {
                    PrintDot();
                    return;
                }
                if (chkprintGoldInvoice.Checked == true)
                    ReportName = "rptSalesInvoice";
                else
                    ReportName = "rptSalesInvoiceDaimond";


                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                if (gridView1.Columns["Description"].Visible == true)
                    rptFormName = "rptSalesInvoiceArb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["StoreName"].Value = txtCustomerMobile.Text.ToString();
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["RemaindAmount"].Value = lblRemaindAmount.Text.Trim().ToString();
                rptForm.Parameters["PaidAmount"].Value = txtPaidAmount.Text.Trim().ToString();

                if (txtVatID.Text != string.Empty)
                    rptForm.Parameters["StoreName"].Value = "فاتورة ضريبية  ";
                else
                    rptForm.Parameters["StoreName"].Value = "فاتورة ضريبية مبسطه ";


                if (Comon.cInt(cmbMethodID.EditValue) == 1)
                    rptForm.Parameters["MethodName"].Value = "نقدا";

                if (Comon.cInt(cmbMethodID.EditValue) == 2)
                {
                    rptForm.Parameters["MethodName"].Value = "اجل";
                     
                }
                else //if (Comon.cInt(cmbMethodID.EditValue) == 2)
                {
                    rptForm.Parameters["CustomerName"].Value = txtCustomerName.Text.ToString();
                }

                if(lblCustomerName.Text==string.Empty)
                    rptForm.Parameters["CustomerName"].Value = txtCustomerName.Text.ToString();
                else
                    rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text;

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
                rptForm.Parameters["CustomerMobile"].Value = txtCustomerMobile.Text.ToString();
                string Date = Comon.ConvertDateToSerial(txtInvoiceDate.Text).ToString();
                int year = Convert.ToInt32(Date.Substring(0, 4));
                int month = Convert.ToInt32(Date.Substring(4, 2)) ;
                int day = Convert.ToInt32(Date.Substring(6, 2));
                DateTime tempDate = new DateTime(year, month, day);
                rptForm.Parameters["HDate"].Value = Comon.ConvertFromEngDateToHijriDate(tempDate).Substring(0, 10);
                rptForm.Parameters["NumbToWord"].Value = Lip.ToWords(Convert.ToDecimal(lblNetBalance.Text.Trim().ToString()), Comon.cInt(Lip.GetValue("SELECT TAFQEETID FROM Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and BranchID= " + MySession.GlobalBranchID)));
                /********Total*********/
                rptForm.Parameters["InvoiceTotal"].Value = lblInvoiceTotalBeforeDiscount.Text.Trim().ToString();
                rptForm.Parameters["UnitDiscount"].Value = lblUnitDiscount.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = lblInvoiceTotalGold.Text.ToString();
                    rptForm.Parameters["AdditionalAmount"].Value = lblAdditionaAmmount.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["Tel"].Value = lblSellerName.Text.Trim().ToString();
                rptForm.Parameters["Mobile"].Value = txtCustomerMobile.Text.ToString(); 
                rptForm.Parameters["TotalDaimond"].Value = lblTotalDaimond.Text.ToString(); 
                //rptForm.Parameters["TotalStone"].Value = lblTotalStown.Text.ToString(); ;
                //rptForm.Parameters["TotalBagate"].Value = lblTotalBagat.Text.ToString(); ;
                rptForm.Parameters["Tafqeet"].Value = Lip.ToWords(Comon.ConvertToDecimalPrice(lblNetBalance.Text), Comon.cInt(Lip.GetValue("SELECT TAFQEETID FROM Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and BranchID= " + MySession.GlobalBranchID)));
                Parameter param1 = new Parameter();
                param1.Name = "pic";
                param1.Type = typeof(System.Drawing.Image);
                param1.Value = picItemImage.Image;
                rptForm.Parameters.Add(param1);
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

                    if (chkprintGoldInvoice.Checked)
                        row["ExpiryDate"] = 1;

                    if (chkprintGoldInvoice.Checked)
                        row["Bones"] = gridView1.GetRowCellValue(i, "STONE_W").ToString();


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
                x.InvoiceDate = Comon.cDateTime(txtInvoiceDate.Text + ":" + txtRegTime.Text);
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

                if (MySession.PrintBuildPill == 1)
                {
                    PrintBill();
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
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
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text.ToString();


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

                rptForm.Parameters["MethodName"].Value = cmbMethodID.Text.Trim().ToString();
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
                rptForm.Parameters["NumbToWord"].Value = Lip.ToWords(Convert.ToDecimal(lblNetBalance.Text.Trim().ToString()), Comon.cInt(Lip.GetValue("SELECT TAFQEETID FROM Acc_Currency where ID="+Comon.cInt(cmbCurency.EditValue)+" and BranchID= " + MySession.GlobalBranchID)));
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
                    if (MySession.GlobalHaveVat == "1")
                        row["AdditionalValue"] = gridView1.GetRowCellValue(i, "AdditionalValue").ToString();
                    else
                        row["AdditionalValue"] = 0;
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
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtEditedByUserID.Text) + " And Cancel =0  and BranchID= " + MySession.GlobalBranchID;
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
            try
            {
                string strSql;
                DataTable dt;
                lblCustomerName.Text = "";
                txtCustomerMobile.Text = "";
                txtTel.Text = "";
                txtVatID.Text = "";
                txtCustomerName.Text = "";

                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,* FROM Sales_Customers Where    AccountID =" + txtCustomerID.Text + " and BranchID= " + MySession.GlobalBranchID;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();
                        txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();

                        txtTel.Text = dt.Rows[0]["Tel"].ToString();
                        if (Comon.cLong(dt.Rows[0]["SpecialDiscount"]) > 0)
                            DiscountCustomer = Comon.cInt(dt.Rows[0]["SpecialDiscount"].ToString());
                        if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                        {
                            lblDebitAccountID.Text = txtCustomerID.Text;
                            lblDebitAccountName.Text = lblCustomerName.Text;

                        }

                        if (Comon.cLong(dt.Rows[0]["VATID"]) > 0)
                        {
                            chkForVat.Checked = true;
                            txtVatID.Text = dt.Rows[0]["VATID"].ToString();
                        }
                    }
                    else
                    {
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtCustomerID.Text + " And BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
                    if (Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) != Comon.ConvertToDecimalPrice(Math.Round(((percent * whole) / 100))) && Comon.ConvertToDecimalPrice(txtDiscountOnTotal.Text) == 0)
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + lblChequeAccountID.Text + ") and BranchID= " + MySession.GlobalBranchID;
                //CSearch.ControlValidating(lblChequeAccountID, lblChequeAccountName, strSQL);
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + lblDebitAccountID.Text + ") and BranchID= " + MySession.GlobalBranchID;
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + lblCreditAccountID.Text + ") and BranchID= " + MySession.GlobalBranchID;
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + lblAdditionalAccountID.Text + ") and BranchID= " + MySession.GlobalBranchID;
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE   (Cancel = 0) AND (AccountID = " + lblDiscountDebitAccountID.Text + ") and BranchID= " + MySession.GlobalBranchID;
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
                strSQL = "SELECT " + PrimaryName + " AS AccountName FROM Acc_Accounts WHERE  (Cancel = 0) AND (AccountID = " + lblNetAccountID.Text + ") and BranchID= " + MySession.GlobalBranchID;
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
        private void frmSaleInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F3)
            //    Find();
            //else if (e.KeyCode == Keys.F2)
            //    ShortcutOpen();
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
                dtItem.Rows[i]["HavVat"] = chkForVat.Checked;
                dtItem.Rows[i]["Total"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Total").ToString());
                dtItem.Rows[i]["Cancel"] = 0;
                CostPriceRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "SalePrice").ToString());
                QTYRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                TotalRow = CostPriceRow * QTYRow;

                if (chkForVat.Checked == true)
                {

                    AdditionalAmountRow = (TotalRow - DiscountRow) / 100 * MySession.GlobalPercentVat;
                    NetRow = Comon.ConvertToDecimalPrice((TotalRow - DiscountRow) + AdditionalAmountRow);
                    if (MySession.GlobalHaveVat == "1")
                        dtItem.Rows[i]["AdditionalValue"] = AdditionalAmountRow.ToString("N" + MySession.GlobalPriceDigits);
                    else
                        dtItem.Rows[i]["AdditionalValue"] = 0;
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

            gridView1.Columns["HavVat"].OptionsColumn.ReadOnly = !chkForVat.Checked;


            gridControl.DataSource = dtItem;

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
            lblCachCaption.Text = "حساب النقد";
            try
            {
                lblCachCaption.Visible = true;
                lblDebitAccountID.Visible = true;
                lblDebitAccountName.Visible = true;

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
                if (Comon.cDbl(txtCustomerID.Text) <= 0)
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
                    lblCachCaption.Text = "حساب العميل";
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
                            if (Comon.cDbl(txtCustomerID.Text) <= 0)
                             Find();
                        }
                    }

                    {
                        lblNetAccountCaption.Enabled = false;
                        lblNetAccountID.Enabled = false;
                        lblNetAccountName.Enabled = false;

                        lblCachCaption.Visible = false;
                        lblDebitAccountID.Visible = false;
                        lblDebitAccountName.Visible = false;
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
                    //txtNetProcessID.Tag = "ImportantFieldGreaterThanZero";
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
                    lblCachCaption.Visible = true;
                    lblDebitAccountID.Visible = true;
                    lblDebitAccountName.Visible = true;

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
        /// <summary>
        /// This function is called when the frmSalesInvoice form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSalesInvoice_Load(object sender, EventArgs e)
        {
            // Call the DoNew function to initialize the form
            DoNew();

            // Call the simpleButton1_Click function with null arguments to set some default values
            simpleButton1_Click(null, null);

            // Query the database for the value of VAt and assign the result to dVat (this line is commented out)
            // dVat = Lip.SelectRecord(VAt);

            // Set the global decimal precision value to 2
            MySession.GlobalQtyDigits = 2;




            this.BringToFront();

        }

        /// <summary>
        /// This function is called when button1 is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Check if the data table has any rows
            if (dt.Rows.Count < 1)
                return;

            // Query the sales invoice return master for any previous returns with the same CustomerInvoiceID and BranchID
            strSQL = "Select * from Sales_SalesInvoiceReturnMaster where CustomerInvoiceID=" + txtInvoiceID.Text + " And BranchID=" + MySession.GlobalBranchID+" and Cancel=0";
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
                 
                frm.cmbBranchesID.EditValue = cmbBranchesID.EditValue;
                frm.txtCustomerInvoiceID.Text = txtInvoiceID.Text;
                frm.txtCustomerInvoiceID_Validating(null, null);
               // frm.txtCustomerInvoiceID.Text = txtInvoiceID.Text;
                frm.txtDebitDiamondAccountID.Text = this.txtDebitDiamondAccountID.Text;
                frm.lblDebitDiamondAccountName.Text = this.lblDebitDiamondAccountName.Text;
                frm.lblTotalDiamond.Text = this.lblTotalDaimond.Text;
                frm.lblInvoiceTotalGold.Text = lblInvoiceTotalGold.Text;
            }
            // If no returns for this invoice exist, open the SalesInvoiceReturn form in add mode and load it with the invoice data
            else
            {
                frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormAdd = true;
                frm.FormUpdate = true;
                frm.FormView = true;
                frm.Show();
                frm.fillMAsterData(dt);
                frm.txtCostSalseAccountID.Text = this.txtCostSalseAccountID.Text;
                frm.txtCostSalseID_Validating(null, null);
                frm.txtSalesRevenueAccountID.Text = this.txtSalesRevenueAccountID.Text;
                frm.txtSalesRevenueID_Validating(null, null);
                frm.lblInvoiceTotalBeforeDiscount.Text = lblInvoiceTotalBeforeDiscount.Text;
                frm.lblNetBalance.Text = lblNetBalance.Text;
                frm.lblAdditionaAmmount.Text = lblAdditionaAmmount.Text;
                frm.txtCustomerInvoiceID.Text = txtInvoiceID.Text;
                frm.cmbBranchesID.EditValue = cmbBranchesID.EditValue;
                frm.txtDebitDiamondAccountID.Text = this.txtDebitDiamondAccountID.Text;
                frm.lblDebitDiamondAccountName.Text = this.lblDebitDiamondAccountName.Text;
                frm.lblTotalDiamond.Text = this.lblTotalDaimond.Text;
                frm.lblInvoiceTotalGold.Text = lblInvoiceTotalGold.Text;
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

        /// <summary>
        /// This function is called when simpleButton1 is clicked, and it sets the visibility of several text boxes and labels, and button styles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            // Set the visibility of several items and reset the Tag properties of several text boxes and combo boxes
            txtCustomerName.Visible = true;
            txtCustomerID.Visible = false;
            lblCustomerName.Visible = false;
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////

            // Set the button style of simpleButton12 to Default, set the value of labelControl6 and txtVatID to visible, set the value of cmbMethodID to 1, and change the appearance of simpleButton1 to show that it is selected
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            showCustomers(false, 0);
            labelControl6.Visible = true;
            txtVatID.Visible = true;
            labelControl4.Visible = true;
            cmbMethodID.EditValue = 1;
            simpleButton1.Appearance.BackColor = Color.Goldenrod;
            simpleButton1.Appearance.BackColor2 = Color.White;
            simpleButton1.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;

            // Set the value of MethodName to "Cash" in English or "نقدا" in Arabic, and set MethodID to 1.
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "نقدا" : "Cash");
            MethodID = 1;

            // Set the button style of simpleButton2 and simpleButton3 to Default, move the focus to gridView1, and set the currently focused row to the new item row.
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            lblNetAccountID.Tag = "isNumber";
        }
        /// <summary>
        /// This function is called when simpleButton2 is clicked, and it sets the values of several text boxes and combo boxes to an empty string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            // Set the values of several text boxes and combo boxes to an empty string.
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////

            // Set the button style of simpleButton12 to Default, set the value of cmbMethodID to 3, and change the appearance of simpleButton2 to show that it is selected
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            cmbMethodID.EditValue = 3;
            simpleButton2.Appearance.BackColor = Color.Goldenrod;
            simpleButton2.Appearance.BackColor2 = Color.White;
            simpleButton2.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;

            // Set the value of MethodName to "Net" in English or "شبكة" in Arabic, and set MethodID to 2.
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة" : "Net");
            MethodID = 2;

            // Set the button style of simpleButton1 and simpleButton3 to Default, move the focus to gridView1, and set the currently focused row to the new item row.
            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
        }


        /// <summary>
        ///  This function is called when simpleButton3 is clicked, and it sets the values of several text boxes and combo boxes to an empty string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            // Set the values of several text boxes and combo boxes to an empty string.
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";
            /////////////////////////////////////////////////

            // Set the value of cmbMethodID to 5, change the appearance of simpleButton3, and set the value of MethodName to "Net/Cash" and MethodID to 3.
            cmbMethodID.EditValue = 5;
            simpleButton3.Appearance.BackColor = Color.Goldenrod;
            simpleButton3.Appearance.BackColor2 = Color.White;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            simpleButton3.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة/ نقد" : "Net/Cash");
            MethodID = 3;

            // Set the button styles of simpleButton1, simpleButton2, and simpleButton12 to Default, and move the focus to the gridView1 object.
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;

            // Set the currently focused row to the new item row, and set the currently focused column to the second visible column.
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
        }

        /// <summary>
        /// These functions are called when the corresponding button is clicked and add a number to the strQty variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNine_Click(object sender, EventArgs e)
        {
            strQty = strQty + "9"; // Add the number 9 to the strQty variable.
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            strQty = strQty + "8"; // Add the number 8 to the strQty variable.
        }


        /// <summary>
        /// This function is called when the btnPlus button is clicked, and it adds the value of the strQty variable to the QTY column of the currently focused row in the gridView1 object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlus_Click(object sender, EventArgs e)
        {
            // Update the QTY column with the result of the addition.
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"])) + Comon.ConvertToDecimalPrice(strQty.Trim()));

            // Recalculate the row to reflect the changes and move the focus to the gridView1 object.
            CalculateRow(gridView1.FocusedRowHandle, true);
            gridView1.Focus();

            // Move the focus to the last visible row in the gridView1 object and set the currently focused row to the new item row.
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;

            // Set the currently focused column to the first column (index of 0).
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];

            // Reset the strQty variable to an empty string.
            strQty = "";
        }

        /// <summary>
        ///  This function is called when the btnMinus button is clicked, and it subtracts the value of the strQty variable from the QTY column of the currently focused row in the gridView1 object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinus_Click(object sender, EventArgs e)
        {
            // Update the QTY column with the result of the subtraction.
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"])) - Comon.ConvertToDecimalPrice(strQty.Trim()));

            // Recalculate the row to reflect the changes, and move the focus to the gridView1 object.
            CalculateRow(gridView1.FocusedRowHandle, true);
            gridView1.Focus();

            // Move the focus to the last visible row in the gridView1 object, and set the currently focused row to the new item row.
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;

            // Set the currently focused column to the second column (index of 1).
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];

            // Reset the strQty variable to an empty string.
            strQty = "";
        }


        /// <summary>
        /// This function is called when the btnSeven button is clicked, and it appends the string "7" to the strQty variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeven_Click(object sender, EventArgs e)
        {
            strQty = strQty + "7";
        }

        /// <summary>
        ///  This function is called when the btnFour button is clicked, and it appends the string "4" to the strQty variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFour_Click(object sender, EventArgs e)
        {
            strQty = strQty + "4";
        }

        /// <summary>
        /// This function is called when the btnFive button is clicked, and it appends the string "5" to the strQty variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFive_Click(object sender, EventArgs e)
        {
            strQty = strQty + "5";
        }

        /// <summary>
        ///  This function is called when the btnSix button is clicked, and it appends the string "6" to the strQty variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSix_Click(object sender, EventArgs e)
        {
            strQty = strQty + "6";
        }


        /// <summary>
        ///  This function is called when the btnTow button is clicked, and it appends the string "2" to the strQty variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTow_Click(object sender, EventArgs e)
        {
            strQty = strQty + "2";
        }

        /// <summary>
        /// This function is called when the btnOne button is clicked, and it appends the string "1" to the strQty variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOne_Click(object sender, EventArgs e)
        {
            strQty = strQty + "1";
        }

        /// <summary>
        /// This function is called when the btnZero button is clicked, and it appends the string "0" to the strQty variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZero_Click(object sender, EventArgs e)
        {
            strQty = strQty + "0";
        }


        private void txtPaidAmount_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///  This function is called when a key is pressed while the frmCashierSales form has focus, and it does the following:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCashierSales_KeyDown(object sender, KeyEventArgs e)
        {
            // If the F9 key is pressed, set the falgPrint flag to true and call the DoSave() function.
            if (e.KeyCode == Keys.F9)
            {
                falgPrint = true;
                DoSave();
            }

            // If the F6 key is pressed, call the DoSave() function.
            if (e.KeyCode == Keys.F6)
            {
                DoSave();
            }

            // If the F6 key is pressed, call the simpleButton1_Click() function. Otherwise, if the F7 or F8 key is pressed, call the corresponding function.
            else if (e.KeyCode == Keys.F6)
                simpleButton1_Click(null, null);
            else if (e.KeyCode == Keys.F7)
                simpleButton2_Click(null, null);
            else if (e.KeyCode == Keys.F8)
                simpleButton3_Click(null, null);

            // If the F3 key is pressed, call the Find() function. Otherwise, if the F2 key is pressed, call the ShortcutOpen() function.
            if (e.KeyCode == Keys.F3)
                Find();
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();
        }


        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This function is called when the txtInvoiceDate value is changed, and it does the following:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInvoiceDate_EditValueChanged(object sender, EventArgs e)
        {
            // If the txtInvoiceDate value is null or empty, set it to the current DateTime.
            if (string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
                txtInvoiceDate.EditValue = DateTime.Now;

            // If the converted date value of txtInvoiceDate is greater than the server date, set txtInvoiceDate to the server date.
            //if (Comon.ConvertDateToSerial(txtInvoiceDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
            //    txtInvoiceDate.Text = Lip.GetServerDate();
            if (Lip.CheckDateISAvilable(txtInvoiceDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                txtInvoiceDate.Text = Lip.GetServerDate();
                return;
            }
        }


        /// <summary>
        /// This function is called when the simpleButton12 is clicked, and it does the following:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            // Set the visibility of the txtCustomerName and txtCustomerID controls
            txtCustomerName.Visible = false;
            txtCustomerID.Visible = true;
            lblCustomerName.Visible = true;

            // Clear the text of the txtCustomerName and set the Tag properties of several other controls to an empty string
            txtCustomerName.Text = "";
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
            cmbBank.Tag = " ";
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtCheckID.Tag = " ";

            // Show the customer information fields and set the value of the cmbMethodID variable to 2
            //showCustomers(true, 1);
            cmbMethodID.EditValue = 2;

            // Set the simpleButton12's appearance properties
            simpleButton12.Appearance.BackColor = Color.Goldenrod;
            simpleButton12.Appearance.BackColor2 = Color.White;
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            simpleButton12.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            simpleButton12.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;

            // Set the value of the MethodName and MethodID variables
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "آجل" : "Future");
            MethodID = 4;

            // Change the appearance properties of several other buttons and reset the focus to the gridView1 control
            simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            gridView1.Focus();
            gridView1.MoveLastVisible();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
        }


        /// <summary>
        /// This function is used to show or hide customer information fields based on a boolean value and an integer value
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        private void showCustomers(bool p, int f)
        {
            // Clear the customer information fields
            txtCustomerID.Text = "";
            lblCustomerName.Text = "";
            txtVatID.Text = "";

            // Set the visibility of the labelControl6 control to the value of the boolean parameter p
            labelControl6.Visible = p;

            // Bring the txtCustomerID, lblCustomerName, and labelControl4 controls to the front
            txtCustomerID.BringToFront();
            lblCustomerName.BringToFront();
            labelControl4.BringToFront();

            // Set the visibility of the labelControl4 control to the value of the boolean parameter p
            labelControl4.Visible = p;

            // Set the visibility of the txtVatID control to the value of the boolean parameter p
            txtVatID.Visible = p;
        }



        /// <summary>
        /// This function is called when the value of the check box changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            // If the check box is checked, show the group box and adjust the position of the grid control
            if (checkEdit1.Checked == true)
            {
                groupBox1.Visible = true;
                gridControl.Width = gridControl.Width - groupBox1.Width;
                gridControl.Location = new Point(241, gridControl.Location.Y);
            }
            else
            {
                // If the check box is not checked, hide the group box and adjust the position of the grid control
                groupBox1.Visible = false;
                gridControl.Width = gridControl.Width + groupBox1.Width;
                gridControl.Location = new Point(1, gridControl.Location.Y);
            }
        }


        /// <summary>
        /// This function is called when the value of the check box changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            // If the check box is checked, uncheck the other check box
            if (checkBox2.Checked == true)
                checkBox1.Checked = false;
            else
                checkBox1.Checked = true; // If the check box is not checked, check the other check box
        }

        /// <summary>
        ///This function is called when the value of the check box changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // If the check box is checked, uncheck the other check box
            if (checkBox1.Checked == true)
                checkBox2.Checked = false;
            else
                checkBox2.Checked = true; // If the check box is not checked, check the other check box
        }


        private void labelControl27_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This function is called when the txtDailyID is validated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDailyID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
            {
                // Get the invoice ID for the specified daily ID and cost center ID
                txtInvoiceID.Text = Lip.GetValue("Select InvoiceID from Sales_SalesInvoiceMaster where DailyID=" + txtDailyID.Text + " And CostCenterID=" + txtCostCenterID.Text+" and BranchID= " + MySession.GlobalBranchID);

                // Read the record with the invoice ID
                ReadRecord(Comon.cLong(txtInvoiceID.Text));
            }
            else
            {
                // If the user does not have permission to view records, display an error message and return
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }

        /// <summary>
        /// This function is called when the user clicks the Save and Print button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAndPrint_Click(object sender, EventArgs e)
        {
            // Set a flag to indicate that a print operation will be performed
            falgPrint = true;

            // Call the DoSave function to save the data
            DoSave();
        }


        /// <summary>
        /// This function is called when the user clicks the Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Get the path of the TypeCustomer file in the current directory
            string startupPath = Directory.GetCurrentDirectory() + "\\";

            // Open the TypeCustomer file
            var TypeCustomer = new FileStream(@startupPath + "TypeCustomer.txt", FileMode.Open, FileAccess.Read);
            var text = "";

            // Read the content of the TypeCustomer file
            using (var streamReader = new StreamReader(TypeCustomer, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }

            // If the TypeCustomer text is 1, open the frmCashierSalesGold form
            if (Comon.cInt(text) == 1)
            {
                frmCashierSalesGold frm = new frmCashierSalesGold();

                // Check the user's permission level to access the form
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, 1, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    // Set the language of the form to English if the user's language preference is English
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    // Show the form
                    frm.Show();
                }
                else
                    frm.Dispose();

            }
            // If the TypeCustomer text is not 1, open the frmCashierSales form
            else
            {
                frmCashierSales frm1 = new frmCashierSales();

                // Check the user's permission level to access the form
                if (Permissions.UserPermissionsFrom(frm1, frm1.ribbonControl1, 1, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    // Set the language of the form to English if the user's language preference is English
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm1);
                    // Show the form
                    frm1.Show();
                    frm1.FormAdd = true;
                    frm1.FormView = true;
                    frm1.FormUpdate = true;
                }
                else
                    frm1.Dispose();
            }
        }


        private void txtTel_Validating(object sender, CancelEventArgs e)
        {

        }

        private void txtCustomerMobile_Validating(object sender, CancelEventArgs e)
        {
            if (txtCustomerMobile.Text.Trim() == string.Empty)
                return;
            try
            {
                string strSql;
                DataTable dt;
                lblCustomerName.Text = "";
                txtTel.Text = "";
                txtVatID.Text = "";
                txtCustomerID.Text = "";

                if (txtCustomerName.Text.Trim() == string.Empty && txtCustomerMobile.Text.Trim() == string.Empty)
                {


                    return;

                }
               
                if (txtCustomerMobile.Text.Trim() != string.Empty && txtCustomerMobile.Text.Trim().Length == 10)
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,* FROM Sales_Customers Where    Mobile ='" + txtCustomerMobile.Text.Trim() + "' And BranchID=" + MySession.GlobalBranchID;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtCustomerID.Text = dt.Rows[0]["AccountID"].ToString();
                        txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                        txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();
                        txtTel.Text = dt.Rows[0]["Tel"].ToString();

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

                        if (Comon.cLong(dt.Rows[0]["SpecialDiscount"]) > 0)
                            DiscountCustomer = Comon.cInt(dt.Rows[0]["SpecialDiscount"].ToString());
                        if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                        {
                            lblDebitAccountID.Text = txtCustomerID.Text;
                            lblDebitAccountName.Text = lblCustomerName.Text;
                        }
                        gridView1.Focus();
                        gridView1.MoveNext();
                        gridView1.FocusedColumn = gridView1.VisibleColumns[1];

                    }
                    else
                    {
                        if (txtCustomerName.Text == string.Empty)
                        {
                            txtCustomerName.Focus();
                            Messages.MsgInfo(Messages.TitleInfo, "يرجى اضافة اسم العميل");
                            return;
                        }

                        SaveCust();
                        gridView1.Focus();
                        gridView1.MoveNext();
                        gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                    }
                }
                else
                {
                    lblCustomerName.Text = "";
                    txtCustomerMobile.Text = "";
                    txtTel.Text = "";
                    txtVatID.Text = "";
                    txtCustomerID.Text = "";
                     
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


        }
        void SaveCust()
        {
            Sales_Customers model = new Sales_Customers();
            model.CustomerID = Comon.cInt(txtCustomerID.Text);
            model.AccountID = Comon.cInt(txtCustomerID.Text);
            //Comon.cLong(txtAccountID.Text);

            if (IsNewRecord == true)
            {
                model.CustomerID = 0;
                if (MethodID == 4)
                    model.AccountID = GetNewAccountID();
                else
                    model.AccountID = 0;

            }
            model.ArbName = txtCustomerName.Text;
            model.EngName = txtCustomerName.Text;
            model.SpecialDiscount = 0;
            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;
            model.BranchID = UserInfo.BRANCHID;
            model.FacilityID = UserInfo.FacilityID;
            model.Tel = txtTel.Text;
            model.Mobile = txtCustomerMobile.Text;
            model.Fax = txtCustomerMobile.Text;
            model.Address = "";
            model.VATID = txtVatID.Text;
            model.Notes = txtNotes.Text;
            model.Email = "";
            model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
            model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.Cancel = 0;
            model.ContactPerson = "";
            model.IdentityNumber = "";
            model.CustomerType = "";
            model.BlockingReason = "";
            model.IsInBlackList = 0;
            model.Gender = 0;
            model.NationalityID = 0;

            int StoreID;
            int UpdateID;
            if (IsNewRecord == true)
                StoreID = Sales_CustomersDAL.InsertSales_Customers(model);
            else
                UpdateID = Sales_CustomersDAL.UpdateSales_Customers(model);
           
            if(MethodID==4)
            addAccountID(long.Parse(model.AccountID.ToString()));
            // Messages.MsgInfo(Messages.TitleInfo, "تم اضافة العميل بنجاح");
            txtCustomerID.Text = model.AccountID.ToString();
            lblCustomerName.Text = model.ArbName;
            txtCustomerName.Text = model.ArbName;

        }
        /// <summary>
        /// This method retrieves a new account ID from the Acc_Accounts table in the database
        /// </summary>
        /// <returns></returns>
        public long GetNewAccountID()
        {
            try
            {
                int code;
                MySession.GlobalAccountsLevelDigits = 11;

                int sNode;
                int SumDigitsCountBeforeSelectedLevel;
                int DigitsCountForSelectedLevel;
                long MaxID;
                string str;
                string strDigits = "";

                // Get the parent account ID and account level
                ParentAccountID = Lip.GetValue("SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE DeclareAccountName='CustomerAccount' and BranchID= " + MySession.GlobalBranchID);
                AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID + " and BranchID= " + MySession.GlobalBranchID)) + 1;
                // Get the maximum account ID and calculate the code for the new account
                str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID + " and BranchID= " + MySession.GlobalBranchID);
                strSQL = "SELECT Sum(DigitsNumber) FROM  Acc_AccountsLevels WHERE  BranchID = " + MySession.GlobalBranchID + " And LevelNumber <" + AccountLevel;
                SumDigitsCountBeforeSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                strSQL = "SELECT  DigitsNumber FROM  Acc_AccountsLevels WHERE  BranchID = " + MySession.GlobalBranchID + " And LevelNumber =" + AccountLevel;
                DigitsCountForSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                if (str == "")
                    code = 0;
                else
                    code = int.Parse(str.Substring(SumDigitsCountBeforeSelectedLevel, DigitsCountForSelectedLevel));
                MaxID = 1;
                for (int i = 1; i <= DigitsCountForSelectedLevel; ++i)
                {
                    MaxID = MaxID * 10;
                    strDigits = strDigits + "0";

                }
                if (code < MaxID)
                {

                    code = code + 1;
                    GetNewID = ParentAccountID.Substring(0, SumDigitsCountBeforeSelectedLevel) + code.ToString(strDigits);

                    // GetNewID +=code.ToString(strDigits);

                }
                else
                {
                    if (UserInfo.Language == iLanguage.English)
                        XtraMessageBox.Show("You Cannot Add More Than " + MaxID + " Accounts in This Level");
                    else
                        XtraMessageBox.Show("لا يمكن إضافة اكثر من " + MaxID + " حسابات في هذا المستوى");
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return long.Parse(GetNewID.PadRight(MySession.GlobalAccountsLevelDigits, '0'));




        }
        
        /// <summary>
        /// This method adds the specified AccountID to the Acc_Accounts table in the database, along with other necessary information 
        /// </summary>
        /// <param name="AccountID"></param>
        public void addAccountID(long AccountID)
        {

            // Create a new Acc_Accounts object with the specified AccountID and other attributes
            Acc_Accounts model = new Acc_Accounts();
            model.AccountID = AccountID;
            model.AccountLevel = AccountLevel;
            model.AccountTypeID = 1;
            model.BranchID = UserInfo.BRANCHID;
            model.FacilityID = UserInfo.FacilityID;
            model.StopAccount = 0;
            model.ParentAccountID = long.Parse(ParentAccountID);
            model.MaxLimit = 0;
            model.MinLimit = 0;
            model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
            model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.Cancel = 0;
            model.ArbName = txtCustomerName.Text;
            model.EngName = txtCustomerName.Text;
            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;

            // Set the BranchID to the current user's branch ID
            int StoreID;
            model.BranchID = Comon.cInt(UserInfo.BRANCHID);

            // Insert or update the Acc_Accounts record in the database
            if (IsNewRecord == true)
                StoreID = Acc_AccountsDAL.InsertAcc_Accounts(model);
            else
                Acc_AccountsDAL.UpdateAcc_Accounts(model);

        }

        /// <summary>
        /// This method is called when the txtCustomerName field is being validated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCustomerName_Validating(object sender, CancelEventArgs e)
        {
            // Call the txtCustomerMobile_Validating method to validate the mobile number field as well
            txtCustomerMobile_Validating(null, null);
        }


        /// <summary>
        /// This method handles the KeyDown event for the txtCustomerMobile textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCustomerMobile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Clear the customer information fields
                lblCustomerName.Text = "";
                txtCustomerMobile.Text = "";
                txtCustomerName.Text = "";
                txtTel.Text = "";
                txtVatID.Text = "";
                txtCustomerID.Text = "";
                // Move the focus to the next row in the grid
                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            }
        }


        /// <summary>
        /// This method handles the KeyDown event for the txtCustomerName textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCustomerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Clear the customer information fields
                lblCustomerName.Text = "";
                txtCustomerMobile.Text = "";
                txtCustomerName.Text = "";
                txtTel.Text = "";
                txtVatID.Text = "";
                txtCustomerID.Text = "";
                // Move the focus to the next row in the grid
                gridView1.Focus();
                gridView1.MoveNext();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            }
        }
     
        long  SaveVariousVoucherMachin(int DocumentID)
        {

            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " and BranchID= " + MySession.GlobalBranchID+" And DocumentType=" + objRecord.DocumentType));
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
            objRecord.Notes = "فاتورة مبيعات الماس";
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
                
                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the object's ID, branch ID, facility ID, account ID, credit, debit, declaration, and cost center ID properties based on the available controls.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1)
                      returned.AccountID = Comon.cLong(lblDebitAccountID.Text);
                else if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                    returned.AccountID = Comon.cLong(txtCustomerID.Text);
                returned.VoucherID = VoucherID;
                 
                returned.Debit = Comon.cDbl(lblNetBalance.Text);
           

                // Add the object to the list of returned objects.  

                returned.DebitGold = Comon.cDbl(lblInvoiceTotalGold.Text);
             
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
             
                returned.Debit = Comon.cDbl(lblNetBalance.Text);
                returned.DebitGold = Comon.cDbl(lblInvoiceTotalGold.Text);
              
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
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblChequeAccountID.Text);
                returned.VoucherID = VoucherID;

                // Set the object's credit and debit properties based on the lblNetBalance control.
              
                returned.Debit = returned.Debit = Comon.cDbl(lblNetBalance.Text);
                returned.DebitGold = Comon.cDbl(lblInvoiceTotalGold.Text);
             
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
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
                returned.VoucherID = VoucherID;
    
                // Set the object's credit and debit properties based on the lblInvoiceTotal and lblNetAmount controls.
                returned.Credit = 0;
                returned.Debit = (Comon.cDbl(Comon.cDbl(lblInvoiceTotal.Text)+Comon.cDbl(lblAdditionaAmmount.Text)) - Comon.cDbl(txtNetAmount.Text));
    
                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                 returned.DebitGold =  Comon.cDbl(lblInvoiceTotalGold.Text);
             


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
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
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

            // This code creates a new instance of the "Acc_VariousVoucherMachinDetails" class to represent the credit sale in the accounting records. 
            // It sets the relevant properties of the instance and adds it to the list of records.
            //Credit Sale
            returned = new Acc_VariousVoucherMachinDetails();

            // Set the properties of the instance.
            returned.ID = 3;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblCreditAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(lblInvoiceTotal.Text);
            
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

            // Add the instance to the list of records.
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
                returned.Credit = Comon.cDbl(lblAdditionaAmmount.Text);
                returned.Debit = 0;
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                listreturned.Add(returned);
            }
            //=

            //crdit Gold
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 4;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(lblCreditGoldAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = 0;
            returned.Debit = 0;
            returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);
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
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " and BranchID= " + MySession.GlobalBranchID+" And DocumentType=" + objRecord.DocumentType));
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
            objRecord.Notes = "فاتورة مبيعات سلعية";
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

                // Create a new Acc_VariousVoucherMachinDetails object.
                returned = new Acc_VariousVoucherMachinDetails();

                // Set the object's ID, branch ID, facility ID, account ID, credit, debit, declaration, and cost center ID properties based on the available controls.
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;

                if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 1)
                    returned.AccountID = Comon.cLong(lblDebitAccountID.Text);
                else if (Comon.cInt(cmbMethodID.EditValue.ToString()) == 2)
                    returned.AccountID = Comon.cLong(txtCustomerID.Text);

                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(lblNetBalance.Text);
                returned.DebitDiamond = Comon.cDbl(lblTotalDiamondCustomer.Text);

                // Add the object to the list of returned objects.  
                 
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
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
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(lblNetBalance.Text);
                returned.DebitDiamond = Comon.cDbl(lblTotalDiamondCustomer.Text);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
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
                returned.Credit = 0;
                returned.Debit =  Comon.cDbl(lblNetBalance.Text);
                returned.DebitDiamond = Comon.cDbl(lblTotalDiamondCustomer.Text);
                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
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
                returned.Credit = 0;
                returned.Debit = (Comon.cDbl(Comon.cDbl(lblInvoiceTotal.Text) + Comon.cDbl(lblAdditionaAmmount.Text)) - Comon.cDbl(txtNetAmount.Text));
                returned.DebitDiamond = Comon.cDbl(lblTotalDiamondCustomer.Text);
                // Set the object's declaration and cost center ID properties based on the txtNotes and txtCostCenterID controls, respectively.
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 

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
                    returned.Credit = 0;
                    returned.Debit = Comon.cDbl(txtNetAmount.Text);
                    returned.Declaration = txtNotes.Text;
                    returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                    returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                    returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                    returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
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
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(lblDiscountTotal.Text);
                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
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
            for (int i = 0; i <= gridView1.DataRowCount-1; i++)
            {
                int isServec = Comon.cInt(Lip.GetValue("SELECT IsService  FROM   [Stc_Items] where [ItemID]=" + gridView1.GetRowCellValue(i, "ItemID").ToString() + " and BranchID= " + MySession.GlobalBranchID+" and [Cancel]=0"));
                if (isServec != 1)
                  TotalCost += Comon.cDbl(gridView1.GetRowCellValue(i, "CostPrice"));
            }
            returned.Debit = Comon.cDbl(TotalCost);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //// Add the instance to the list of records.
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);
      
            returned = new Acc_VariousVoucherMachinDetails();
            // Set the properties of the instance.
            returned.ID = 4;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(TotalCost);
            returned.CreditDiamond = Comon.cDbl(lblTotalDiamondCustomer.Text);
            //returned.CreditGold = Comon.cDbl(lblInvoiceTotalGold.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
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
                returned.Credit = Comon.cDbl(lblAdditionaAmmount.Text);

                returned.Declaration = txtNotes.Text;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice));
                listreturned.Add(returned);
            }
            //=

            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 5;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtSalesRevenueAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(lblInvoiceTotal.Text);
            returned.Debit = 0;
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        /// <summary>
        /// This code is executed when the "btnMachinResraction" button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            strSQL = "Select * from " + Sales_SaleInvoicesDAL.TableName + " where cancel=0  and GoldUsing=" + GoldUsing + " and BranchID= " + MySession.GlobalBranchID;
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
                        long VoucherID = 0;
                        if(MySession.GlobalInventoryType==2)//جرد دوري 
                            VoucherID=  SaveVariousVoucherMachin(Comon.cInt(txtInvoiceID.Text));
                        else if(MySession.GlobalInventoryType==1)//جرد مستمر 
                            VoucherID = SaveVariousVoucherMachinContinuousInv(Comon.cInt(txtInvoiceID.Text));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + Sales_SaleInvoicesDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Sales_SaleInvoicesDAL.PremaryKey + " = " + txtInvoiceID.Text + " AND BranchID=" + Comon.cInt(cmbBranchesID.EditValue));

                    }



                }

                this.Close();
            }
        }

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0   and BranchID= " + MySession.GlobalBranchID));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID= " + MySession.GlobalBranchID));
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

        public void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
            try{
                //ClearFields();
                {

                    //DoNew();
                    dt = Sales_SalesOrderDAL.frmGetDataDetalByID(Comon.cInt(txtOrderID.Text.Trim()), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                   
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = true;
                        button1.Visible = true;
                        //Validate                      
                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);
                        StopSomeCode = true;
                        StopSomeCode = false;

                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);

                        //cmbSellerID.EditValue = dt.Rows[0]["SellerID"].ToString();
                        //Masterdata
                        //txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();

                        //txtCustomerMobile.Text = dt.Rows[0]["Mobile"].ToString();


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
                       
                       // Validations.DoReadRipon(this, ribbonControl1);
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

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            frmCashierSalesOrder frm = new frmCashierSalesOrder();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void btnCompond_Click(object sender, EventArgs e)
        {
            frmGoldOutOnBail frm = new frmGoldOutOnBail();
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

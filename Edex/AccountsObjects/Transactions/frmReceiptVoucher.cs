using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.DAL.Accounting;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Edex.DAL.Stc_itemDAL;

namespace Edex.AccountsObjects.Transactions
{
    public partial class frmReceiptVoucher : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        DataTable dtDeclaration;
        public const int DocumentType = 3;
        string FocusedControl = "";
        private ReceiptVoucherDAL cClass;
        public CultureInfo culture = new CultureInfo("en-US");
        private string strSQL;
        public bool editMode = false;
        public bool isNewReg = false;
        private string PrimaryName;
        private string AccountName;
        private string CaptionCreditAmount;
        private string CaptionAccountID;
        private string CaptionAccountName;
        private string CaptionDiscount;
        private string CaptionDeclaration;
        private string CaptionCostCenterID;
        private string CaptionBarcode;
        private string CaptionItemName;
        private string CaptionQtyGold;
        private string CaptionQtyDiamond;

        private string Barcode;
        private string Calipar;
        private string ItemName;
        private string WeightGold;
        private string QtyGoldEqulivent;


        private bool IsNewRecord;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;
        OpenFileDialog OpenFileDialog1 = null;
        frmViewImage frm = null;
        DataTable dt = new DataTable();
        //all record master and detail
        BindingList<Acc_ReceiptVoucherDetails> AllRecords = new BindingList<Acc_ReceiptVoucherDetails>();

        //list detail
        BindingList<Acc_ReceiptVoucherDetails> lstDetail = new BindingList<Acc_ReceiptVoucherDetails>();

        //Detail
        Acc_ReceiptVoucherDetails BoDetail = new Acc_ReceiptVoucherDetails();

        #endregion


        public frmReceiptVoucher()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                lblNetBalance.BackColor = Color.WhiteSmoke;
                lblNetBalance.ForeColor = Color.Black;
                AccountName = "ArbAccountName";
                PrimaryName = "ArbName";
                CaptionCreditAmount = "الـمـبـلـغ";
                CaptionAccountID = "رقم الحساب";
                CaptionAccountName = "اسم الحساب";
                CaptionDiscount = "الخصـم";
                CaptionDeclaration = "الـبـيـــــان";
                CaptionCostCenterID = "مركز تكلفة";
                CaptionBarcode = "كود الصنف";
                CaptionItemName = "اسم الصنف  ";
                CaptionQtyGold = "وزن الذهب ";
                CaptionQtyDiamond = "وزن الألماس ";

                Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    AccountName = "EngAccountName";
                    PrimaryName = "EngName";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");
                    CaptionCreditAmount = "Amount";
                    CaptionAccountID = "Account ID";
                    CaptionAccountName = "Account Name";
                    CaptionDiscount = "Discount";
                    CaptionDeclaration = "Declaration";
                    CaptionCostCenterID = "Cost Center";
                    CaptionQtyDiamond = "Diamond QTY";
                    CaptionQtyGold = "Gold QTY";
                }
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
                FillCombo.FillComboBoxLookUpEdit(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", " BranchID = " + UserInfo.BRANCHID);
                /***********************Component ReadOnly  ****************************/
                TextEdit[] txtEdit = new TextEdit[3];
                txtEdit[0] = lblDelegateName;
                txtEdit[1] = lblDebitAccountName;
                txtEdit[2] = lblDiscountAccountName;
                foreach (TextEdit item in txtEdit)
                {
                    item.ReadOnly = true;
                    item.Enabled = false;
                    item.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    item.Properties.AppearanceDisabled.BackColor = Color.WhiteSmoke;
                }
                /*********************** Date Format dd/MM/yyyy ****************************/
                InitializeFormatDate(txtVoucherDate);
                /*********************** Roles From ****************************/
                //_____Read Only 
               // txtVoucherDate.ReadOnly = !MySession.GlobalAllowChangefrmReceiptVoucherDate;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmReceiptVoucherCurencyID;
                txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmReceiptVoucherSalesDelegateID;
                txtVoucherDate.ReadOnly = !MySession.GlobalAllowChangefrmReceiptVoucherDate;

                //_____ Read Only Account ID 
                lblDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmReceiptVoucherDebitAccountID;
                lblDiscountAccountID.ReadOnly = !MySession.GlobalAllowChangefrmReceiptVoucherDiscountAccountID;
                /************ Button Search Account ID ***************/
                RolesButtonSearchAccountID();
                /********************* Event For Account Component ****************************/
                this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);
                this.btnDiscountSearch.Click += new System.EventHandler(this.btnDiscountSearch_Click);

                this.lblDebitAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblDebitAccountID_Validating);
                this.lblDiscountAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblDiscountAccountID_Validating);
                this.lblDebitAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.lblDiscountAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                /********************* Event For TextEdit Component **************************/
                if (MySession.GlobalAllowWhenEnterOpenPopup)
                {
                    this.txtVoucherDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                    this.cmbCurency.Enter += new System.EventHandler(this.PublicCombox_Enter);
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtVoucherDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                    this.cmbCurency.Click += new System.EventHandler(this.PublicCombox_Click);
                }

                this.txtRegistrationNo.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtInvoiceID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDelegateID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDocumentID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtVoucherID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
            
                //_____ Validating
                this.txtVoucherID.Validating += new System.ComponentModel.CancelEventHandler(this.txtVoucherID_Validating);
                this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
                this.lnkAddImage.Click += new System.EventHandler(this.lnkAddImage_Click);

                this.picItemImage.MouseLeave += new System.EventHandler(this.picItemImage_MouseLeave);
                this.picItemImage.MouseHover += new System.EventHandler(this.picItemImage_MouseHover);
                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmReceiptVoucher_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
                cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;

                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                //DoNew();
                
                
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
            

            lstDetail = new BindingList<Acc_ReceiptVoucherDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView1.BestFitColumns();

            /******************* Columns Visible=false ********************/

            gridView1.Columns["ReceiptVoucherID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FACILITYID"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["ReceiptVoucherMaster"].Visible = false;
            gridView1.Columns["ArbAccountName"].Visible = false;
            gridView1.Columns["EngAccountName"].Visible = false;
            gridView1.Columns["Barcode"].Visible = false;
            gridView1.Columns["ItemName"].Visible = false;
            /******************* Columns Visible=true ********************/
            gridView1.Columns[AccountName].Visible = true;
            /******************* Columns Visible=true *******************/

            gridView1.Columns["CreditAmount"].Caption = CaptionCreditAmount;
            gridView1.Columns["AccountID"].Caption = CaptionAccountID;
            gridView1.Columns[AccountName].Caption = CaptionAccountName;
            gridView1.Columns[AccountName].VisibleIndex = gridView1.Columns["AccountID"].VisibleIndex + 1;
            gridView1.Columns[AccountName].Width = 150;
            gridView1.Columns["Discount"].Caption = CaptionDiscount;
            gridView1.Columns["Declaration"].Caption = CaptionDeclaration;
            gridView1.Columns["Declaration"].Width = 150;
            gridView1.Columns["CostCenterID"].Caption = CaptionCostCenterID;
            gridView1.Columns["Discount"].Visible = false;
            gridView1.Columns["ItemName"].Width = 120;
            gridView1.Columns["AccountID"].Width = 120;

            gridView1.Columns["Barcode"].Caption = CaptionBarcode;
            gridView1.Columns["ItemName"].Caption = CaptionItemName;
            gridView1.Columns["WeightGold"].Caption = CaptionQtyGold;
            gridView1.Columns["QtyGoldEqulivent"].Caption = CaptionQtyDiamond;
            gridView1.Columns["Calipar"].Caption = "العيار";

            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].Visible = false;
            gridView1.Columns["CurrencyPrice"].Visible = false;
            gridView1.Columns["CurrencyName"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;
            DataTable dtitems = Lip.SelectRecord("SELECT " + PrimaryName + " FROM Acc_Currency where Cancel=0 ");
            string[] CurrncyName = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                CurrncyName[i] = dtitems.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(CurrncyName);
            gridControl.RepositoryItems.Add(riComboBoxitems);
            gridView1.Columns["CurrencyName"].ColumnEdit = riComboBoxitems;
            gridView1.Columns["CurrencyPrice"].Caption = "سعر العملة";
            gridView1.Columns["CurrencyID"].Caption = "رقم العملة";
            gridView1.Columns["CurrencyName"].Caption = "اسم العملة";
            gridView1.Columns["CurrencyEquivalent"].Caption = "المقابل بالعملة المحلية ";
            if (UserInfo.Language == iLanguage.English)
            {
                gridView1.Columns["Calipar"].Caption = "Calipar";
                gridView1.Columns["CurrencyPrice"].Caption = "Currency Price  ";
                gridView1.Columns["CurrencyID"].Caption = "Currency ID  ";
                gridView1.Columns["CurrencyName"].Caption = "Currency Name";
                gridView1.Columns["CurrencyEquivalent"].Caption = "Currency Equivalent";
            }


            gridView1.Columns["Calipar"].Visible = false;
            gridView1.Columns["QtyGoldEqulivent"].Visible = false;
            gridView1.Columns["WeightGold"].Visible = false;
            gridView1.Focus();
            /*************************Columns Properties ****************************/

            gridView1.Columns["CostCenterID"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmReceiptVoucherCostCenterID;
            gridView1.Columns["CostCenterID"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmReceiptVoucherCostCenterID;

            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridView1.Columns[AccountName].ColumnEdit = rAccountName;
            gridControl.RepositoryItems.Add(rAccountName);

            RepositoryItemLookUpEdit rCostCenter = new RepositoryItemLookUpEdit();
            gridView1.Columns["CostCenterID"].OptionsColumn.AllowEdit = MySession.GlobalAllowChangefrmReceiptVoucherCostCenterID;
            gridView1.Columns["CostCenterID"].ColumnEdit = rCostCenter;
            gridControl.RepositoryItems.Add(rCostCenter);
            FillCombo.FillComboBoxRepositoryItemLookUpEdit(rCostCenter, "Acc_CostCenters", "CostCenterID", PrimaryName);

        }
        private void gridView1_ShownEditor(object sender, EventArgs e)
        {
            HasColumnErrors = false;
            CalculatTotalBalance();
        }
        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            double num;
            GridView view = sender as GridView;
            view.ClearColumnErrors();
            HasColumnErrors = false;
            string ColName = view.FocusedColumn.FieldName;

            if (ColName == "WeightGold")
            {
                decimal Gold = Comon.cDec(e.Value.ToString());
                int Calpir = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Calipar").ToString());
                decimal Eq = Comon.ConvertTo21Caliber(Gold, Calpir, 18);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QtyGoldEqulivent"], Comon.ConvertToDecimalPrice(Eq));
            }

            if (ColName == "Barcode")
            {
                  Barcode = e.Value.ToString();
                dt = Stc_itemsDAL.GetItemData(Barcode, UserInfo.FacilityID);
                if (dt.Rows.Count > 0)
                {                 
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemName"], dt.Rows[0]["ArbName"].ToString());
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Calipar"], dt.Rows[0]["Caliber"].ToString());
                }
            }


            if (ColName == "AccountID" || ColName == "CurrencyPrice")
            {
                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;
                }
                else if (!(double.TryParse(e.Value.ToString(), out num)))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputShouldBeNumber;
                }
                else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) <= 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsGreaterThanZero;
                }

                /****************************************/
                if (ColName == "AccountID" && e.Valid == true)
                {
                    DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                    {
                        DataRow[] row = dt.Select("AccountID=" + e.Value.ToString());
                        if (row.Length == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisAccountID;
                        }
                        else
                        {
                            if (row[0]["AccountID"].ToString() == lblDebitAccountID.Text.Trim())
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgCanNotChoseSameAccount + " " + lblDebitAccountName.Text.Trim();
                            }
                            else { FileItemData(row[0]); }
                        }
                    }

                }

            }
            else if (ColName == AccountName)
            {
                DataTable dtAccountName = Lip.SelectRecord("Select AccountID, " + PrimaryName + " AS " + AccountName + " from Acc_Accounts Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') And FacilityID=" + UserInfo.FacilityID + "   AND AccountLevel=" + MySession.GlobalNoOfLevels);
                if (dtAccountName == null && dtAccountName.Rows.Count == 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgNoFoundThisAccountID;
                }
                else
                {
                    if (dtAccountName.Rows[0]["AccountID"].ToString() == lblDebitAccountID.Text.Trim())
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgCanNotChoseSameAccount + " " + lblDebitAccountName.Text.Trim();
                    }
                    else
                    {
                        if (Lip.CheckTheAccountIsStope(Comon.cDbl(dtAccountName.Rows[0]["AccountID"]), Comon.cInt(cmbBranchesID.EditValue)))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountIsStope);
                            e.Value = "";
                            return;

                        }

                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], dtAccountName.Rows[0]["AccountID"]);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AccountName], dtAccountName.Rows[0][AccountName]);
                    }
                }

            }
            else if (ColName == "Declaration"  )
            {

                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;

                }
            }
            if (ColName == "CreditAmount")
            {
                if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) > 0)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice"))).ToString());

            }
            if (ColName == "CurrencyPrice")
            {
                if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CreditAmount")) > 0)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CreditAmount"))).ToString());

            }
            if (ColName == "CurrencyName")
            {
                DataTable dt = Lip.SelectRecord("Select ID ,ExchangeRate from Acc_Currency Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "')");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyID", dt.Rows[0]["ID"]);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice", dt.Rows[0]["ExchangeRate"]);
                if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CreditAmount")) > 0)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice")) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CreditAmount"))).ToString());


            }
            else if (ColName == "Discount")
            {
                decimal CreditAmount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CreditAmount").ToString());
                decimal PercentDiscount = Comon.ConvertToDecimalPrice(CreditAmount) * (Comon.ConvertToDecimalPrice(MySession.GlobalDiscountPercentReceiptVoucher) / 100);
                if (!(double.TryParse(e.Value.ToString(), out num)))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputShouldBeNumber;
                }
                else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) > CreditAmount)    //PercentDiscount)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgNotAllowedPercentDiscount;
                }
            }
        }
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                HasColumnErrors = false;
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "AccountID")
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
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }

                    }
                    else if (col.FieldName == "Declaration")
                    {
                        var cellValue = gridView1.GetRowCellValue(e.RowHandle, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["CostCenterID"], MySession.GlobalDefaultReceiptVoucherCostCenterID);
        }
        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {

            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = (e.ListSourceRowIndex + 1);
        }
        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e)
        {
            try
            {
                var grid = sender as GridControl;
                var view = grid.FocusedView as GridView;
                if (view.FocusedColumn == null)
                    return;
                HasColumnErrors = false;
                if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {

                    double num;
                    HasColumnErrors = false;
                    var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                    string ColName = view.FocusedColumn.FieldName;
                    if (ColName == "AccountID"  || ColName == "CreditAmount")
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
                        else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0)
                        {

                            HasColumnErrors = true;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                        }
                    }
                    else if (ColName == "Declaration")
                    {
                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            HasColumnErrors = true;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsRequired);
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
                    CalculatTotalBalance();
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
        bool IsValidGrid()
        {
            double num;
            bool LastRowHasData = false;
            gridView1.MoveLast();
            int length = gridView1.RowCount - 1;
            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }
            else if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            else if (gridView1.FocusedRowHandle < 0)
            {
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "AccountID" || col.FieldName == "Declaration")
                    {
                        var cellValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, col);
                        if (cellValue != null && string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            LastRowHasData = true;
                        }
                    }
                }
                if (LastRowHasData)
                {
                    foreach (GridColumn col in gridView1.Columns)
                    {
                        if (col.FieldName == "AccountID")
                        {
                            var cellValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, col);

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                                return false;
                            }
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
                        else if (col.FieldName == "Declaration")
                        {
                            var cellValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, col);

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                                return false;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "AccountID")
                    {
                        var cellValue = gridView1.GetRowCellValue(i, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
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
                    else if (col.FieldName == "Declaration")
                    {
                        var cellValue = gridView1.GetRowCellValue(i, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #region Calculate
        public void CalculatTotalBalance()
        {
            decimal CreditTotal = 0;
            decimal DiscountRow = 0;
            decimal CreditAmountRow = 0;
            decimal DiscountTotal = 0;

            decimal CreditGoldRow = 0;
            decimal CreditDiamondRow = 0;
            decimal CreditGoldTotal = 0;
            decimal CreditDiamondTotal = 0;

            try
            {

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    CreditAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CreditAmount").ToString());
                    DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());
                    CreditGoldRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "WeightGold").ToString());
                    CreditDiamondRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QtyGoldEqulivent").ToString());
                    CreditGoldTotal += CreditGoldRow;
                    CreditDiamondTotal += CreditDiamondRow;
                    CreditTotal += CreditAmountRow;
                    DiscountTotal += DiscountRow;
                }
                if (gridView1.FocusedRowHandle < 0)
                {
                    var ResultCreditAmount = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CreditAmount");
                    var ResultDiscount = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount");
                    var ResultCreditGold = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "WeightGold");
                    var ResultCreditDiamond = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "QtyGoldEqulivent");

                    CreditAmountRow = ResultCreditAmount != null ? Comon.ConvertToDecimalPrice(ResultCreditAmount.ToString()) : 0;
                    DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;
                    CreditGoldRow = ResultCreditGold != null ? Comon.ConvertToDecimalPrice(ResultCreditGold.ToString()) : 0;
                    CreditDiamondRow = ResultCreditDiamond != null ? Comon.ConvertToDecimalPrice(ResultCreditDiamond.ToString()) : 0;

                    CreditGoldTotal += CreditGoldRow;
                    CreditDiamondTotal += CreditDiamondRow;
                    CreditTotal += CreditAmountRow;
                    DiscountTotal += DiscountRow;
                }

                lblTotal.Text = CreditTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblDiscountTotal.Text = DiscountTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblNetBalance.Text = (CreditTotal - DiscountTotal).ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblTotalGold.Text = CreditGoldTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblTotalDiamond.Text = CreditDiamondTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);

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
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        #endregion
        #endregion
        #region Function
        #region Other Function
        private void FileItemData(DataRow dr)
        {
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], dr["AccountID"]);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ArbAccountName"], dr["ArbName"]);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyName"], cmbCurency.Text.ToString());
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyPrice"], txtCurrncyPrice.Text);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyEquivalent"], 0);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CurrencyID"], Comon.cInt(cmbCurency.EditValue));
            if (UserInfo.Language == iLanguage.English)
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngAccountName"], dr["EngName"].ToString());

        }
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where BranchID=" + UserInfo.BRANCHID;

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl.Trim() == txtVoucherID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "ReceiptVoucher", "رقم السـند", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "ReceiptVoucher", "Voucher ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() ==lblDebitAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmReceiptVoucherDebitAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblDebitAccountID, lblDebitAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblDebitAccountID, lblDebitAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                if (!MySession.GlobalAllowChangefrmReceiptVoucherSalesDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }

             

            else if (FocusedControl.Trim() == gridControl.Name)
            {

                if (gridView1.FocusedColumn == null) return;
                if (gridView1.FocusedColumn.Name == "colAccountID" || gridView1.FocusedColumn.Name == "colAccountName")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "Account ID", MySession.GlobalBranchID);
                }

                else if (gridView1.FocusedColumn.Name == "colBarCode")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemBarcode", "البـاركود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "ItemBarcode", "BarCode", MySession.GlobalBranchID);
                }
            }


            GetSelectedSearchValue(cls);

        }
       
       
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtVoucherID.Name)
                {
                    txtVoucherID.Text = cls.PrimaryKeyValue.ToString();
                    txtVoucherID_Validating(null, null);
                }
             
                else if (FocusedControl == lblDebitAccountID.Name)
                {
                    lblDebitAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblDebitAccountID_Validating(null, null);
                }
                else if (FocusedControl == txtDelegateID.Name)
                {
                    txtDelegateID.Text = cls.PrimaryKeyValue.ToString();
                    txtDelegateID_Validating(null, null);
                }
                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colAccountID" || gridView1.FocusedColumn.Name == "colAccountName")
                    {
                        var ibdex = gridView1.IsNewItemRow(gridView1.FocusedRowHandle);
                        if (ibdex == false)
                        {
                            if (editMode == true)
                            {
                                string Barcode = cls.PrimaryKeyValue.ToString();
                                
                                {
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], Barcode);
                                    DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                                    DataRow[] row = dt.Select("AccountID=" + Barcode);
                                    if (Comon.cInt(row.Length) > 0)
                                        FileItemData(row[0]);
                                    gridView1.FocusedColumn = gridView1.VisibleColumns[3];
                                    gridView1.ShowEditor();
                                    CalculatTotalBalance();
                                }


                            }
                        }
                        else
                        {
                            string Barcode = cls.PrimaryKeyValue.ToString();
                            
                            {
                                gridView1.AddNewRow();
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], Barcode);
                                DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                                DataRow[] row = dt.Select("AccountID=" + Barcode);
                                if (Comon.cInt(row.Length) > 0)
                                    FileItemData(row[0]);
                                gridView1.FocusedColumn = gridView1.VisibleColumns[3];
                                gridView1.ShowEditor();


                                CalculatTotalBalance();
                            }

                        }
                    }
                }
            }

        }
        
        public void ReadRecord(long VoucherID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                   dt = ReceiptVoucherDAL.frmGetDataDetalByID(VoucherID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        //Account
                        lblDebitAccountID.Text = dt.Rows[0]["DebitAccountID"].ToString();
                        lblDebitAccountID_Validating(null, null);

                     
                        lblDiscountAccountID.Text = dt.Rows[0]["DiscountAccountID"].ToString();
                        lblDiscountAccountID_Validating(null, null);
                        if (Comon.cInt(dt.Rows[0]["TypeOpration"]) == 1)
                        {
                            lblRadioEquvilanGold.Checked = true;
                            radioButton1_CheckedChanged(null, null);
                        }
                        else if (Comon.cInt(dt.Rows[0]["TypeOpration"]) == 2)
                        {
                            lblRadioEquvilanDiamond.Checked = true;
                            radioButton2_CheckedChanged(null, null);
                        }
                        //Masterdata
                        txtVoucherID.Text = dt.Rows[0]["ReceiptVoucherID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtInvoiceServiceID.Text = dt.Rows[0]["InvoiceServiceID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();
                        //cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());

                        //Validate
                        txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        txtDelegateID_Validating(null, null);
                        //Date
                         
                        //txtVoucherDate.EditValue = Comon.ConvertSerialDateTo(dt.Rows[0]["ReceiptVoucherDate"].ToString());

                        cmbStatus.EditValue = Comon.cInt(dt.Rows[0]["Posted"].ToString());

                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["ReceiptVoucherDate"].ToString()) == "")
                            txtVoucherDate.Text = "";
                        else
                            txtVoucherDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["ReceiptVoucherDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

                         //   txtVoucherDate.DateTime = Convert.ToDateTime(Comon.ConvertSerialDateTo(dt.Rows[0]["ReceiptVoucherDate"].ToString()));

                        //Ammount
                        lblTotal.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["CreditAmount"].ToString()).ToString();
                        lblDiscountTotal.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["DiscountAmount"].ToString()).ToString(); 
                        lblNetBalance.Text = (Comon.ConvertToDecimalPrice(lblTotal.Text.Trim()) - Comon.ConvertToDecimalPrice(lblDiscountTotal.Text.Trim())).ToString();
                        lblTotalGold.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["WeightGold"].ToString()).ToString("N" + MySession.GlobalPriceDigits);
                        lblTotalDiamond.Text = Comon.ConvertToDecimalPrice(dt.Rows[0]["TotalWeightDiamond"].ToString()).ToString("N" + MySession.GlobalPriceDigits);
                        byte[] imgByte = null;
                        try
                        {
                            if (DBNull.Value != dt.Rows[0]["SpendImage"])
                            {
                                imgByte = (byte[])dt.Rows[0]["SpendImage"];
                                picItemImage.Image = byteArrayToImage(imgByte);
                            }
                            else
                                picItemImage.Image = null;
                        }
                        catch { }
                        //GridVeiw

                        gridControl.DataSource = dt;

                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;

                        
                    }
                }
                RolesButtonSearchAccountID();
                Validations.DoReadRipon(this, ribbonControl1);

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void GetAccountsDeclaration()
        {
            #region get accounts declaration
            lblDebitAccountID.Text = MySession.GlobalDefaultReceiptVoucherDebitAccountID;
            lblDebitAccountID_Validating(null, null);
            //List<Acc_DeclaringMainAccounts> AllAccounts = new List<Acc_DeclaringMainAccounts>();
            //int BRANCHID = UserInfo.BRANCHID;
            //int FacilityID = UserInfo.FacilityID;
      
            //dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(BRANCHID, FacilityID);
            //if (dtDeclaration != null && dtDeclaration.Rows.Count > 0)
            //{
            //    ////حساب الخصم المكتسب
            //    //DataRow[] row3 = dtDeclaration.Select("DeclareAccountName = 'GivenDiscountAccount'");
            //    //if (row3.Length > 0)
            //    //{
            //    //    lblDiscountAccountID.Text = row3[0]["AccountID"].ToString();
            //    //    lblDiscountAccountName.Text = row3[0]["AccountName"].ToString();

            //    //}

            //    //DataRow[] row8 = dtDeclaration.Select("DeclareAccountName = 'DebitGoldAccountID'");
            //    //if (row8.Length > 0)
            //    //{
            //    //    txtDebitGoldAccountID.Text = row8[0]["AccountID"].ToString();
            //    //    lblDebitGoldAccountName.Text = row8[0]["AccountName"].ToString();
            //    //}
            //}
            #endregion
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

        public void ClearFields()
        {
            try
            {
                picItemImage.Image = byteArrayToImage(DefaultImage());
                txtDocumentID.Text = "";
                txtDelegateID.Text = "";
                lblDelegateName.Text = "";
                txtNotes.Text = "";
                txtVoucherDate.EditValue = DateTime.Now;

                txtNotes.Text = "";
                lblDebitAccountID.Text = "";
                lblDebitAccountName.Text = "";
                lblRadioEquvilanDiamond.Checked = false;
                lblRadioEquvilanGold.Checked = false;
                lblTotal.Text = "0";
                lblDiscountTotal.Text = "0";
                lblNetBalance.Text = "0";


                cmbCurency.EditValue = MySession.GlobalDefaultReceiptVoucherCurencyID;
                txtDelegateID.Text = MySession.GlobalDefaultReceiptVoucherSalesDelegateID;
                txtDelegateID_Validating(null, null);

                GetAccountsDeclaration();
                cmbCurency.ItemIndex = 0;
                cmbCurency.EditValue = 13;
                
               

                picItemImage.Image = null;
                lstDetail = new BindingList<Acc_ReceiptVoucherDetails>();

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
                    strSQL = "SELECT TOP 1 * FROM " + ReceiptVoucherDAL.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + ReceiptVoucherDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + ReceiptVoucherDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + ReceiptVoucherDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + ReceiptVoucherDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + ReceiptVoucherDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + ReceiptVoucherDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new ReceiptVoucherDAL();

                    long InvoicIDTemp = Comon.cLong(txtVoucherID.Text);
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
                isNewReg = true;
                IsNewRecord = true;
                txtVoucherID.Text = ReceiptVoucherDAL.GetNewID().ToString();
                txtRegistrationNo.Text = RestrictionsDailyDAL.GetNewID(this.Name).ToString();
                ClearFields();
                EnabledControl(true);
                editMode = false;
                gridView1.Focus();
                gridView1.MoveLast();
                gridView1.FocusedColumn = gridView1.VisibleColumns[1];
                gridView1.ShowEditor();

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
                MoveRec(Comon.cInt(txtVoucherID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtVoucherID.Text), xMovePrev);
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
                txtVoucherID.Enabled = true;
                txtVoucherID.Focus();
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
            if (!IsNewRecord)
                if (!FormUpdate)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                    return;
                }

            Validations.DoEditRipon(this, ribbonControl1);
            EnabledControl(true);
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            gridView1.ShowEditor();


        }
        private void Save()
        {
            gridView1.MoveLastVisible();
            CalculatTotalBalance();
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            int VoucherID = Comon.cInt(txtVoucherID.Text);

            Acc_ReceiptVoucherMaster objRecord = new Acc_ReceiptVoucherMaster();
            objRecord.ReceiptVoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            if(lblRadioEquvilanGold.Checked)
                objRecord.TypeOpration = 1;
            else if (lblRadioEquvilanDiamond.Checked)
                objRecord.TypeOpration = 2; 
            //Date
            objRecord.ReceiptVoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.InvoiceServiceID = Comon.cInt(txtInvoiceServiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Receipt Voucher" : "سند القبض ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Receipt Voucher" : "سند القبض "));
            objRecord.Notes = txtNotes.Text;
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            //Account
            objRecord.DebitGoldAccountID = Comon.cDbl(txtDebitGoldAccountID.Text);
            objRecord.DebitAccountID = Comon.cDbl(lblDebitAccountID.Text);
            objRecord.DiscountAccountID = Comon.cDbl(lblDiscountAccountID.Text);
            //Ammount
            objRecord.DiscountAmount = Comon.cDbl(lblDiscountTotal.Text);
            objRecord.DebitAmount = Comon.cDbl(lblTotal.Text);
            objRecord.TotalGold = Comon.cDbl(lblTotalGold.Text);
            objRecord.TotalWeightDiamond = Comon.cDbl(lblTotalDiamond.Text);
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
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
                objRecord.ReceiptVoucherID = VoucherID;
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }
            //image
            if (OpenFileDialog1 != null && (OpenFileDialog1.FileName != "") || picItemImage.Image != null)
            {
                if (picItemImage.Image != null)
                {
                    byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                    objRecord.SpendImage = Imagebyte;

                }
                else
                {
                    picItemImage.Image = Image.FromFile(OpenFileDialog1.FileName);
                    picItemImage.Visible = true;
                    byte[] Imagebyte = imageToByteArray(picItemImage.Image);
                    objRecord.SpendImage = Imagebyte;
                }
            }
            else
                objRecord.SpendImage = DefaultImage();
            
            Acc_ReceiptVoucherDetails returned;
            List<Acc_ReceiptVoucherDetails> listreturned = new List<Acc_ReceiptVoucherDetails>();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Acc_ReceiptVoucherDetails();
                returned.ID = i;
                returned.BranchID = UserInfo.BRANCHID;
                returned.FACILITYID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                returned.ReceiptVoucherID = VoucherID;
                returned.CreditAmount = Comon.cDbl(gridView1.GetRowCellValue(i, "CreditAmount").ToString());
                returned.Discount = Comon.cDbl(gridView1.GetRowCellValue(i, "Discount").ToString());
                returned.Declaration = gridView1.GetRowCellValue(i, "Declaration").ToString();
                returned.CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                returned.CurrencyID = Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString());
                returned.CurrencyName = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                returned.CurrencyPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                if (gridView1.GetRowCellValue(i, "Barcode") == null)
                    returned.Barcode = "";
                else
                    returned.Barcode = gridView1.GetRowCellValue(i, "Barcode").ToString();
                if (gridView1.GetRowCellValue(i, "ItemName") == null)
                    returned.ItemName = "";
                else
                    returned.ItemName = gridView1.GetRowCellValue(i, "ItemName").ToString();

                if (gridView1.GetRowCellValue(i, "WeightGold") == null)
                    returned.WeightGold = 0;
                else
                    returned.WeightGold = Comon.cDec(gridView1.GetRowCellValue(i, "WeightGold").ToString());

                if (gridView1.GetRowCellValue(i, "QtyGoldEqulivent") == null)
                    returned.QtyGoldEqulivent = 0;
                else
                    returned.QtyGoldEqulivent = Comon.cDec(gridView1.GetRowCellValue(i, "QtyGoldEqulivent").ToString());


                if (gridView1.GetRowCellValue(i, "Calipar") == null)
                    returned.Calipar = 0;
                else
                    returned.Calipar = Comon.cInt(gridView1.GetRowCellValue(i, "Calipar").ToString());


                

                listreturned.Add(returned);
                
            }

            if (listreturned.Count > 0)
            {
                objRecord.ReceiptVoucherDetails = listreturned;
                long Result = ReceiptVoucherDAL.InsertUsingXML(objRecord, IsNewRecord);
                if (Comon.cInt(cmbStatus.EditValue) >1)
                {
                    if (Comon.cInt(Result) > 0)
                    {
                        //حفظ القيد الالي
                        long ID = SaveVariousVoucherMachin(Comon.cInt(Result));
                        if (ID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + ReceiptVoucherDAL.TableName + " Set RegistrationNo =" + ID + " where " + ReceiptVoucherDAL.PremaryKey + " = " + txtVoucherID.Text);

                    }
                }
                SplashScreenManager.CloseForm(false);
               
                if (IsNewRecord == true)
                {
                    if (Result >  0)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                        DoNew();
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }

                }
                else
                {
                    editMode = false;

                    if (Result >0 )
                    {
                        txtVoucherID_Validating(null, null);
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
        protected override void DoSave()
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;
                if (!Validations.IsValidFormCmb(cmbCurency))
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

                if (!Lip.CheckTheProcessesIsPosted("Acc_ReceiptVoucherMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtVoucherID.Text),PrimeryColName:"ReceiptVoucherID"))
                {
                    Messages.MsgWarning(Messages.TitleError, Messages.msgTheProcessIsNotUpdateBecuseIsPosted);
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                txtVoucherDate_EditValueChanged(null, null);
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
                int TempID = Comon.cInt(txtVoucherID.Text);
                int VoucherID = Comon.cInt(Lip.GetValue("SELECT  [VoucherDelyID]  FROM  [Acc_ReceiptVoucherMaster] where [BranchID]=" + Comon.cInt(cmbBranchesID.EditValue) + " and Cancel=0 and [ReceiptVoucherID]=" + Comon.cLong(txtVoucherID.Text)));
               
                Acc_ReceiptVoucherMaster model = new Acc_ReceiptVoucherMaster();
                model.ReceiptVoucherID = Comon.cInt(txtVoucherID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                int Result = ReceiptVoucherDAL.DeleteAcc_ReceiptVoucherMaster(model);
                
                if(Comon.cInt(Result)>0)
                {
                    Acc_VariousVoucherMaster model2 = new Acc_VariousVoucherMaster();
                    model2.VoucherID = Comon.cInt(VoucherID);
                    model2.EditUserID = UserInfo.ID;
                    model2.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    model2.FacilityID = UserInfo.FacilityID;
                    model2.EditComputerInfo = UserInfo.ComputerInfo;
                    model2.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                    model2.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                    int Result2 = VariousVoucherDAL.DeleteAcc_VariousVoucherMaster(model2);
                    //delete Voucher Machin 
                    int VoucherID2 = 0;
                    int Result3 = 0;
                    Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
                    objRecord.DocumentType = 1;
                    VoucherID2 = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + VoucherID + " And DocumentType=" + 1 + " And BranchID=" + Comon.cInt(UserInfo.BRANCHID)));

                    objRecord.VoucherID = VoucherID2;
                    objRecord.EditUserID = UserInfo.ID;
                    objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                    objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                    objRecord.EditComputerInfo = UserInfo.ComputerInfo;
                    objRecord.BranchID = UserInfo.BRANCHID;
                    objRecord.FacilityID = UserInfo.FacilityID;
                    Result3 = VariousVoucherMachinDAL.DeleteAcc_VariousVoucherMachinMaster(objRecord);
                }

                if (Comon.cInt(Result) > 0)
                {
                    //حذف القيد الالي

                    int VoucherID4 = DeleteVariousVoucherMachin(Comon.cInt(Result));
                   
                    if (VoucherID4 == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية");
                }
                if (Comon.cInt(Result) >= 0)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    MoveRec(model.ReceiptVoucherID, xMovePrev);
                }
              
                else
                {
                    Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
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
                ReportName = "rptReceiptVoucher";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["VoucherID"].Value = txtVoucherID.Text.Trim().ToString();
                rptForm.Parameters["VoucherDate"].Value = txtVoucherDate.Text.Trim().ToString();
                rptForm.Parameters["DocumentID"].Value = txtDocumentID.Text.Trim().ToString();
                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["RegistrationNo"].Value = txtRegistrationNo.Text.Trim().ToString();

                /********Total*********/
                rptForm.Parameters["Total"].Value = lblTotal.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();
                rptForm.Parameters["TotalGold"].Value = lblTotalGold.Text.Trim().ToString();


                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;

                /********************** Details ****************************/
                var dataTable = new dsReports.rptReceiptVoucherDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["CreditAmount"] = gridView1.GetRowCellValue(i, "CreditAmount").ToString();
                    row["CreditGold"] = gridView1.GetRowCellValue(i, "WeightGold").ToString();
                    row["CreditDiamond"] = gridView1.GetRowCellValue(i, "QtyGoldEqulivent").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["AccountID"] = gridView1.GetRowCellValue(i, "AccountID").ToString();
                    row["AccountName"] = gridView1.GetRowCellValue(i, AccountName).ToString();
                    row["Declaration"] = gridView1.GetRowCellValue(i, "Declaration").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterName").ToString();
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
        private bool SaveRestrictionsDaily()
        {

            int VoucherID = Comon.cInt(txtVoucherID.Text);
            string Release = "سند قبض رقم";
            int CostCenterID = Comon.cInt(MySession.GlobalDefaultSpendVoucherCostCenterID);
            List<RestrictionsDaily> listRecord = new List<RestrictionsDaily>();
            long MaxRegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            if (IsNewRecord)
            {
                MaxRegistrationNo = RestrictionsDailyDAL.GetNewID(this.Name);
                txtRegistrationNo.Text = MaxRegistrationNo.ToString();
            }

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {

                RestrictionsDaily Record = new RestrictionsDaily();
                Record.ID = i;
                Record.RegistrationNo = MaxRegistrationNo;
                Record.BranchNum = UserInfo.BRANCHID;
                Record.FacilityID = UserInfo.FacilityID;
                Record.TranNo = VoucherID;
                Record.TransType = 3;
                Record.RegistrationDate = Comon.cDbl(Comon.ConvertDateToSerial(txtVoucherDate.Text));
                Record.Acc_code = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                Record.Master_code = 0;
                Record.Debt = 0;
                Record.Credit = Comon.cDbl(gridView1.GetRowCellValue(i, "CreditAmount").ToString()); ;
                Record.Discount = 0;
                if (gridView1.GetRowCellValue(i, "Declaration").ToString() != "")
                    Record.Release = gridView1.GetRowCellValue(i, "Declaration").ToString();
                else
                    Record.Release = Release + VoucherID;
                Record.AccountFinal = 0;
                Record.CurrencyNum = Comon.cInt(cmbCurency.EditValue.ToString());
                Record.SellerNum = 0;
                Record.DelegateNum = 0;
                Record.DocumentNumber = txtDocumentID.Text;
                Record.OperationType = Release;
                Record.Remark = txtNotes.Text.Trim();
                Record.AccountNumCorresponding = lblDebitAccountID.Text;
                Record.Receivables = "";
                CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                Record.CostCenterNo = CostCenterID;
                Record.posted = Comon.cInt(cmbStatus.EditValue);
                Record.Cancel = 0;
                listRecord.Add(Record);


            }
            //***************** Debit AccountID ********************/
            RestrictionsDaily Record2 = new RestrictionsDaily();
            Record2.ID = gridView1.DataRowCount;
            Record2.RegistrationNo = MaxRegistrationNo;
            Record2.BranchNum = UserInfo.BRANCHID;
            Record2.FacilityID = UserInfo.FacilityID;
            Record2.TranNo = VoucherID;
            Record2.TransType = 3;
            Record2.RegistrationDate = Comon.cDbl(Comon.ConvertDateToSerial(txtVoucherDate.Text));
            Record2.Acc_code = Comon.cDbl(lblDebitAccountID.Text);
            Record2.Master_code = 0;
            Record2.Debt = Comon.cDbl(lblNetBalance.Text.ToString());
            Record2.Credit = 0;
            Record2.Discount = 0;
            Record2.Release = txtNotes.Text.Trim();
            Record2.AccountFinal = 1;
            Record2.CurrencyNum = Comon.cInt(cmbCurency.EditValue.ToString());
            Record2.SellerNum = 0;
            Record2.DelegateNum = 0;
            Record2.DocumentNumber = txtDocumentID.Text;
            Record2.OperationType = Release;
            Record2.Remark = txtNotes.Text.Trim();
            Record2.AccountNumCorresponding = "0";
            Record2.Receivables = "";
            Record2.CostCenterNo = CostCenterID;
            Record2.posted = Comon.cInt(cmbStatus.EditValue);
            Record2.Cancel = 0;
            listRecord.Add(Record2);

            //***************** Discount Total ********************/
            if (Comon.cDbl(lblDiscountTotal.Text) > 0)
            {
                RestrictionsDaily Record3 = new RestrictionsDaily();
                Record3.ID = gridView1.DataRowCount;
                Record3.RegistrationNo = MaxRegistrationNo;
                Record3.BranchNum = UserInfo.BRANCHID;
                Record3.FacilityID = UserInfo.FacilityID;
                Record3.TranNo = VoucherID;
                Record3.TransType = 3;
                Record3.RegistrationDate = Comon.cDbl(Comon.ConvertDateToSerial(txtVoucherDate.Text));
                Record3.Acc_code = Comon.cDbl(lblDiscountAccountID.Text);
                Record3.Master_code = 0;
                Record3.Debt = Comon.cDbl(lblDiscountTotal.Text.ToString());
                Record3.Credit = 0;
                Record3.Discount = 0;
                Record3.Release = "ماتم خصمه لمذكورين بسند قبض رقم " + VoucherID;
                Record3.AccountFinal = 1;
                Record3.CurrencyNum = Comon.cInt(cmbCurency.EditValue.ToString());
                Record3.SellerNum = 0;
                Record3.DelegateNum = 0;
                Record3.DocumentNumber = txtDocumentID.Text;
                Record3.OperationType = Release;
                Record3.Remark = txtNotes.Text.Trim();
                Record3.AccountNumCorresponding = "0";
                Record3.Receivables = "";
                Record3.CostCenterNo = CostCenterID;
                Record3.posted = Comon.cInt(cmbStatus.EditValue);
                Record3.Cancel = 0;
                listRecord.Add(Record3);
            }
            if (listRecord.Count > 0)
            {
                int Result = RestrictionsDailyDAL.InsertUsingXML(Comon.cInt(txtVoucherID.Text.Trim()), MySession.GlobalBranchID, listRecord, IsNewRecord);
                if (Result == 1)
                    return true;
                else
                    return false;
            }
            return true;

        }
        #endregion
        #endregion
        #region Event
        /************************Event From **************************/
        private void txtRegistrationNo_Validated(object sender, EventArgs e)
        {
            //if (FormView == true)
            //    ReadRecord(Comon.cLong(txtRegistrationNo.Text), true);

            //else
            //{
            //    Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
            //    return;
            //}
        }

        private void frmReceiptVoucher_Load(object sender, EventArgs e)
        {
            DoNew();
            //gridView1.Focus();
            //gridView1.MoveLast();
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            //gridView1.ShowEditor();
          
        }

        private void frmReceiptVoucher_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3)
                Find();
        }
        #region Validating
        private void txtVoucherID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cLong(txtVoucherID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }

        }
        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
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
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblDebitAccountID.Text + ") ";
                CSearch.ControlValidating(lblDebitAccountID, lblDebitAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void lblDiscountAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblDiscountAccountID.Text + ") ";
                CSearch.ControlValidating(lblDiscountAccountID, lblDiscountAccountName, strSQL);
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
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void btnDiscountSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareSearchQuery.SearchForAccounts(lblDiscountAccountID, lblDiscountAccountName);
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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
        #endregion
        #endregion
        #endregion
        #region InitializeComponent
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

            lnkAddImage.Enabled = Value;

            foreach (GridColumn col in gridView1.Columns)
            {

                if (col.FieldName == AccountName || col.FieldName == "CreditAmount" || col.FieldName == "AccountID" || col.FieldName == "Declaration" || col.FieldName == "Discount" || col.FieldName == "CostCenterID")
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }
            }
            
             RolesButtonSearchAccountID();
 
        }
        private void RolesButtonSearchAccountID()
        {

            btnDebitSearch.Enabled = MySession.GlobalAllowChangefrmReceiptVoucherDebitAccountID;
            btnDiscountSearch.Enabled = MySession.GlobalAllowChangefrmReceiptVoucherDiscountAccountID;
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
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            Path = Path + @"\Images\Default.png";
            System.Drawing.Image img = System.Drawing.Image.FromFile(Path);
            MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();

        }
        private void lnkAddImage_Click(object sender, EventArgs e)
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
                    SaveImage(Imagebyte);

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        private void SaveImage(byte[] data)
        {
            try
            {

                SqlConnection Con = new GlobalConnection().Conn;
                if (Con.State == ConnectionState.Closed)
                    Con.Open();

                SqlCommand sc;
                sc = new SqlCommand("Update  " + ReceiptVoucherDAL.TableName + " Set SpendImage=@p Where " + ReceiptVoucherDAL.PremaryKey + "=" + txtVoucherID.Text + " And BranchID=" + UserInfo.BRANCHID, Con);
                sc.Parameters.AddWithValue("@p", data);
                sc.ExecuteNonQuery();

            }
            catch
            {

            }
        }

        protected string getImageID()
        {
            Double days = 0;
            DateTime StartDate = new DateTime((DateTime.Now.Year), 01, 01);
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - StartDate.Ticks);
            System.Random RandNum = new System.Random();
            int MyRandomNumber = RandNum.Next(0, 99);
            days = ts.Days + 1;
            int intSecondOfDay = 0;
            string strReturn = "";
            strReturn = days.ToString().PadLeft(3, '0');
            strReturn = strReturn + MyRandomNumber.ToString().PadLeft(2, '0');
            intSecondOfDay = (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second;
            return strReturn + intSecondOfDay.ToString().PadLeft(5, '0');
        }
        private void picItemImage_MouseHover(object sender, EventArgs e)
        {
            try
            {
                frm = new frmViewImage();
                frm.picInvoiceImage.Image = picItemImage.Image;
                frm.Refresh();
                frm.Width = frm.picInvoiceImage.Width;
                frm.Height = frm.picInvoiceImage.Height + 30;
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void picItemImage_MouseLeave(object sender, EventArgs e)
        {
            if (frm != null)
                frm.Close();
        }
        #endregion

        private void btnPrintRestrictonDaily_Click(object sender, EventArgs e)
        {
            if (txtRegistrationNo.Text == "")
            {
                Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgEnterRegistrationNo);
            }
            else
            {
                frmPrintRestractionDaily frm = new frmPrintRestractionDaily();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.txtVoucherID.Text = txtRegistrationNo.Text;

                    frm.Show();
                }
                else
                    frm.Dispose();
            }
        }

        private void txtVoucherDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVoucherDate.Text.Trim()))
                txtVoucherDate.EditValue = DateTime.Now;
            if (Lip.CheckDateISAvilable(txtVoucherDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
              txtVoucherDate.Text = Lip.GetServerDate();
                return;
            }
             
        }

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            //DoFirst();
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
            objRecord.FacilityID = UserInfo.FacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
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
           // decimal QtyGoldEqu = 0;
            //Credit
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(gridView1.GetRowCellValue(i, "CreditAmount").ToString());
                if (lblRadioEquvilanGold.Checked)
                returned.CreditGold = Comon.cDbl(gridView1.GetRowCellValue(i, "WeightGold").ToString());
                if(lblRadioEquvilanDiamond.Checked)
                returned.CreditDiamond = Comon.cDbl(gridView1.GetRowCellValue(i, "QtyGoldEqulivent").ToString());
               // QtyGoldEqu +=Comon.cDec(returned.CreditGold);
                returned.Debit = 0;
            
                returned.Declaration = gridView1.GetRowCellValue(i, "Declaration").ToString();
                returned.CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }
            //Discount
            if (Comon.cDbl(lblDiscountTotal.Text) > 0)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 4;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDiscountAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(lblDiscountTotal.Text);

                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }
            //Debit  
            if (Comon.cDbl(lblNetBalance.Text) > 0)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 3;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
                returned.VoucherID = VoucherID;
               
                returned.Credit = 0;
                returned.Debit = Comon.cDbl(lblNetBalance.Text);
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
                listreturned.Add(returned);
            }

            ////Debit  
            //if (Comon.cDbl(QtyGoldEqu) > 0)
            //{
            //    returned = new Acc_VariousVoucherMachinDetails();
            //    returned.ID = 4;
            //    returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            //    returned.FacilityID = UserInfo.FacilityID;
            //    returned.AccountID = Comon.cDbl(txtDebitGoldAccountID.Text);
            //    returned.VoucherID = VoucherID;
            //    returned.DebitGold = Comon.cDbl(QtyGoldEqu);
            //    returned.CreditGold = 0;
            //    returned.Credit = 0;
            //    returned.Debit = 0;
            //    returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            //    returned.CostCenterID = 1;
            //    listreturned.Add(returned);
            //}
            //===
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
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtVoucherID.Text + " And DocumentType=" + DocumentType).ToString());
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


            strSQL = "Select * from Acc_ReceiptVoucherMaster where cancel=0";
            DataTable dtSend = new DataTable();
            dtSend = Lip.SelectRecord(strSQL);
            if (dtSend.Rows.Count > 0)
            {
                for (int i = 0; i <= dtSend.Rows.Count - 1; i++)
                {
                    txtVoucherID.Text = dtSend.Rows[i]["ReceiptVoucherID"].ToString();
                    cmbBranchesID.EditValue = Comon.cInt(dtSend.Rows[i]["BranchID"].ToString());

                    txtVoucherID_Validating(null, null);
                    IsNewRecord = true;
                    if (Comon.cInt(txtVoucherID.Text) > 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(Comon.cInt(txtVoucherID.Text));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update Acc_ReceiptVoucherMaster Set DocumentID =" + VoucherID + " where ReceiptVoucherID = " + txtVoucherID.Text + " AND BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            gridView1.Columns["WeightGold"].Visible = lblRadioEquvilanGold.Checked;
            if (lblRadioEquvilanGold.Checked)
                simpleButton2.Visible = true;
            else
                simpleButton2.Visible = false;
            simpleButton3.Visible = simpleButton2.Visible;
         
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            gridView1.Columns["QtyGoldEqulivent"].Visible = lblRadioEquvilanDiamond.Checked;
            if (lblRadioEquvilanDiamond.Checked)
                simpleButton2.Visible = true;
            else
                simpleButton2.Visible = false;
            simpleButton3.Visible = simpleButton2.Visible;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (lblRadioNormal.Checked)
                simpleButton2.Visible = false;
            simpleButton3.Visible = simpleButton2.Visible;
            if (lblRadioNormal.Checked == true)
            {
                gridView1.Columns["QtyGoldEqulivent"].Visible = false;
                gridView1.Columns["WeightGold"].Visible = false;
            }
            else
            {
                gridView1.Columns["QtyGoldEqulivent"].Visible = lblRadioEquvilanGold.Checked;
                gridView1.Columns["WeightGold"].Visible = lblRadioEquvilanDiamond.Checked;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

            if(IsNewRecord)
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforeTry);
                return;
            }
         
           bool isNew=true;
           
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            int VoucherID = Comon.cInt(Lip.GetValue("SELECT  [VoucherDelyID]  FROM  [Acc_ReceiptVoucherMaster] where [BranchID]="+Comon.cInt( cmbBranchesID.EditValue)+" and Cancel=0 and [ReceiptVoucherID]="+Comon.cLong(txtVoucherID.Text)));
            if (VoucherID > 0)
                isNew = false;
            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();
            objRecord.BranchID =Comon.cInt( cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text;
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);

             objRecord.Cancel = 0;
             objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";

            if (isNew == false)
            {
                objRecord.VoucherID = VoucherID;
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();
     

            if (Comon.cDbl(lblNetBalance.Text) > 0)
            {
                
                returned = new Acc_VariousVoucherDetails();
                returned.ID = gridView1.DataRowCount;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                if (lblRadioEquvilanGold.Checked)
                {
                    if (Comon.cDbl( MySession.GlobalDefaultReceiptVoucherIntermediateGoldAccountID)<=0)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء تحديد الحساب الوسيط للمخزن الذهب في افتراضيات سند القبض " : "Please specify the intermediate account for the gold store in the receivable voucher defaults.");
                        return;
                    }
                    returned.AccountID = Comon.cDbl(MySession.GlobalDefaultReceiptVoucherIntermediateGoldAccountID);
                }
                else
                    if (lblRadioEquvilanDiamond.Checked)
                    {
                        if (Comon.cDbl(MySession.GlobalDefaultReceiptVoucherIntermediateDiamondAccountID) <= 0)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء تحديد الحساب الوسيط للمخزن الالماس في افتراضيات سند القبض " : "Please specify the intermediate account for the Diamond store in the receivable voucher defaults.");
                            return;
                        }
                        returned.AccountID = Comon.cDbl(MySession.GlobalDefaultReceiptVoucherIntermediateDiamondAccountID);
                    }
              
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(lblNetBalance.Text);
                returned.Declaration = UserInfo.Language == iLanguage.Arabic ? "قيد تسكير " : "Voucher closure ";
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

                returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice));
                listreturned.Add(returned);
            }
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Acc_VariousVoucherDetails();
                returned.ID = i;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                returned.VoucherID = VoucherID;
                returned.Debit = Comon.cDbl(gridView1.GetRowCellValue(i, "CreditAmount").ToString());


                returned.Declaration = UserInfo.Language == iLanguage.Arabic ? "قيد تسكير " : "Voucher closure ";
                returned.CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());

                returned.CurrencyID = Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString());
                returned.CurrencyName = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                returned.CurrencyPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());
                listreturned.Add(returned);
              }
           

            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                long Result = VariousVoucherDAL.InsertUsingXML(objRecord, VoucherID);
                Lip.NewFields();
                Lip.Table ="Acc_ReceiptVoucherMaster";
                //  Lip.AddNumericField("StoreID", Store.GetNewID().ToString());
                Lip.AddStringField("VoucherDelyID", Result.ToString());
               
                Lip.sCondition = "ReceiptVoucherID=" + Comon.cInt(txtVoucherID.Text);
                Lip.ExecuteUpdate();
                if (Comon.cInt(Result) > 0)
                {
                    //حفظ القيد الالي
                    int VoucherID2 = 0;
                    long Result2 = 0;
                    Acc_VariousVoucherMachinMaster objRecordVarious = new Acc_VariousVoucherMachinMaster();
                    objRecordVarious.DocumentType = 1;
                    VoucherID2 = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + Comon.cLong(Result) + " And DocumentType=" + objRecordVarious.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

                    objRecordVarious.VoucherID = VoucherID2;
                    objRecordVarious.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    objRecordVarious.FacilityID = UserInfo.FacilityID;
                    //Date
                    objRecordVarious.VoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString();
                    objRecordVarious.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                    objRecordVarious.CurrencyName = cmbCurency.Text.ToString();
                    objRecordVarious.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
                    objRecordVarious.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
                    objRecordVarious.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
                    // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
                    objRecordVarious.DelegateID = Comon.cInt(txtDelegateID.Text);
                    objRecordVarious.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
                    objRecordVarious.DocumentID = Comon.cInt(Result);
                    objRecordVarious.Cancel = 0;
                    objRecordVarious.Posted = Comon.cInt(cmbStatus.EditValue);

                    //user Info
                    objRecordVarious.UserID = UserInfo.ID;
                    objRecordVarious.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                    objRecordVarious.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
                    objRecordVarious.ComputerInfo = UserInfo.ComputerInfo;
                    objRecordVarious.EditUserID = 0;
                    objRecordVarious.EditTime = 0;
                    objRecordVarious.EditDate = 0;
                    objRecordVarious.EditComputerInfo = "";
                    if (isNew == false)
                    {
                        objRecordVarious.EditUserID = UserInfo.ID;
                        objRecordVarious.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                        objRecordVarious.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                        objRecordVarious.EditComputerInfo = UserInfo.ComputerInfo;
                    }
                    Acc_VariousVoucherMachinDetails returnedMachin;
                    List<Acc_VariousVoucherMachinDetails> listreturnedMachin = new List<Acc_VariousVoucherMachinDetails>();
                    // decimal QtyGoldEqu = 0;
                    //Credit
                    for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                    {
                        returnedMachin = new Acc_VariousVoucherMachinDetails();
                        returnedMachin.ID = 1;
                        returnedMachin.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                        returnedMachin.FacilityID = UserInfo.FacilityID;
                        returnedMachin.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                        returnedMachin.VoucherID = VoucherID2;
                        returnedMachin.Debit = Comon.cDbl(gridView1.GetRowCellValue(i, "CreditAmount").ToString());
                        returnedMachin.CreditGold = 0;
                        returnedMachin.CreditDiamond = 0;

                        returnedMachin.Declaration = UserInfo.Language == iLanguage.Arabic ? "قيد تسكير " : "Voucher closure ";
                        returnedMachin.CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                        returnedMachin.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                        returnedMachin.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                        returnedMachin.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returnedMachin.Credit) * Comon.cDbl(returnedMachin.CurrencyPrice));
                        listreturnedMachin.Add(returnedMachin);
                    }

                    //Debit  
                    if (Comon.cDbl(lblNetBalance.Text) > 0)
                    {
                        returnedMachin = new Acc_VariousVoucherMachinDetails();
                        returnedMachin.ID = 3;
                        returnedMachin.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                        returnedMachin.FacilityID = UserInfo.FacilityID;

                        if (lblRadioEquvilanGold.Checked)
                            returnedMachin.AccountID = Comon.cDbl(MySession.GlobalDefaultReceiptVoucherIntermediateGoldAccountID);
                        else
                            if (lblRadioEquvilanDiamond.Checked)
                                returnedMachin.AccountID = Comon.cDbl(MySession.GlobalDefaultReceiptVoucherIntermediateDiamondAccountID);
                        returnedMachin.VoucherID = VoucherID2;

                        returnedMachin.Credit = Comon.cDbl(lblNetBalance.Text);
                        returnedMachin.Declaration = UserInfo.Language == iLanguage.Arabic ? "قيد تسكير " : "Voucher closure ";
                        returnedMachin.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

                        returnedMachin.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                        returnedMachin.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
                        returnedMachin.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returnedMachin.Debit) * Comon.cDbl(returnedMachin.CurrencyPrice));
                        listreturnedMachin.Add(returnedMachin);
                    }


                    //===
                    if (listreturnedMachin.Count > 0)
                    {
                        objRecordVarious.VariousVoucherDetails = listreturnedMachin;
                        Result2 = VariousVoucherMachinDAL.InsertUsingXML(objRecordVarious, isNew);
                    }
                    if (Result2 > 0)
                        Messages.MsgInfo(Messages.TitleInfo, UserInfo.Language == iLanguage.Arabic ? "تم انشاء القيد بنجاح" : "The Voucher was created successfully");
                    else
                        Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "خطأ انشاء القيد " : "Error creating the Voucher");
        
                  
                }
                SplashScreenManager.CloseForm(false);

                if (isNew == true)
                {
                    if (Result >  0 )
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                        DoNew();
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }

                }
                else
                {

                    if (Result > 0 )
                    {
                        txtVoucherID_Validating(null, null);
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                int VoucherID = Comon.cInt(Lip.GetValue("SELECT  [VoucherDelyID]  FROM  [Acc_ReceiptVoucherMaster] where [BranchID]=" + Comon.cInt(cmbBranchesID.EditValue) + " and Cancel=0 and [ReceiptVoucherID]=" + Comon.cLong(txtVoucherID.Text)));
               if(VoucherID>0)
               {
                   frmVariousVoucher frm = new frmVariousVoucher();

                   if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                   {
                       if (UserInfo.Language == iLanguage.English)
                           ChangeLanguage.EnglishLanguage(frm);
                     

                       frm.Show();
                       frm.ReadRecord(VoucherID);
                   }
                   else
                       frm.Dispose();
               }
               else
               {
                   Messages.MsgInfo(Messages.TitleInfo, UserInfo.Language == iLanguage.Arabic ? "لا يوجد قيد تسكير" : "There is no restriction");
                   return;
               }
            }
            catch
            {

            }

        }
    }
}

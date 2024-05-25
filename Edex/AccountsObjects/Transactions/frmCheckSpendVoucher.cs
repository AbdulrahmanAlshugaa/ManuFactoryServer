using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
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

namespace Edex.AccountsObjects.Transactions
{

    public partial class frmCheckSpendVoucher : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        DataTable dtDeclaration;
        public const int DocumentType = 22;
        string FocusedControl = "";
        private CheckSpendVoucherDAL cClass;
        public CultureInfo culture = new CultureInfo("en-US");
        public bool editMode = false;
        private string strSQL;
        private string PrimaryName;
        private string AccountName;
        private string CaptionDebitAmount;
        private string CaptionAccountID;
        private string CaptionAccountName;
        private string CaptionDiscount;
        private string CaptionDeclaration;
        private string CaptionReceiptName;
        private string CaptionCostCenterID;
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
        BindingList<Acc_CheckSpendVoucherDetails> AllRecords = new BindingList<Acc_CheckSpendVoucherDetails>();

        //list detail
        BindingList<Acc_CheckSpendVoucherDetails> lstDetail = new BindingList<Acc_CheckSpendVoucherDetails>();

        //Detail
        Acc_CheckSpendVoucherDetails BoDetail = new Acc_CheckSpendVoucherDetails();

        #endregion
        public frmCheckSpendVoucher()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                lblNetBalance.BackColor = Color.WhiteSmoke;
                lblNetBalance.ForeColor = Color.Black;
                AccountName = "AccountName";
                PrimaryName = "ArbName";
                CaptionDebitAmount = "الـمـبـلـغ";
                CaptionAccountID = "رقم الحساب";
                CaptionAccountName = "اسم الحساب";
                CaptionDiscount = "الخصـم";
                CaptionDeclaration = "الـبـيـــــان";
                CaptionReceiptName = "إســـم المـسـتلم";
                CaptionCostCenterID = "مركز تكلفة";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    AccountName = "AccountName";
                    PrimaryName = "EngName";

                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");
                    CaptionDebitAmount = "Amount";
                    CaptionAccountID = "Account ID";
                    CaptionAccountName = "Account Name";
                    CaptionDiscount = "Discount";
                    CaptionDeclaration = "Declaration";
                    CaptionReceiptName = "Recipient Name";
                    CaptionCostCenterID = "Cost Center";
                }
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
                FillCombo.FillComboBoxLookUpEdit(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", " BranchID = " + UserInfo.BRANCHID);
                //FillCombo.FillComboBoxLookUpEdit(cmbBank, "Acc_Banks", "ID", PrimaryName, "", " 1=1");
                /***********************Component ReadOnly  ****************************/
                TextEdit[] txtEdit = new TextEdit[2];
                txtEdit[0] = lblCreditAccountName;
                txtEdit[1] = lblDiscountAccountName;
                foreach (TextEdit item in txtEdit)
                {
                    item.ReadOnly = true;
                    item.Enabled = false;
                    item.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    item.Properties.AppearanceDisabled.BackColor = Color.WhiteSmoke;
                }
                /*********************** Date Format dd/MM/yyyy ****************************/
                InitializeFormatDate(txtVoucherDate);
                InitializeFormatDate(txtGreDate);
                /*********************** Roles From ****************************/

                //_____Read Only 
                txtVoucherDate.ReadOnly = !MySession.GlobalAllowChangefrmCheckSpendVoucherDate;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmCheckSpendVoucherCurencyID;
                txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmCheckSpendVoucherPurchasesDelegateID;
             //   cmbBank.ReadOnly = !MySession.GlobalAllowChangefrmCheckSpendVoucherBankID;


                //_____ Read Only Account ID 
                lblCreditAccountID.ReadOnly = !MySession.GlobalAllowChangefrmCheckSpendVoucherCreditAccountID;
                lblDiscountAccountID.ReadOnly = !MySession.GlobalAllowChangefrmCheckSpendVoucherDiscountAccountID;

                /************ Button Search Account ID ***************/
                RolesButtonSearchAccountID();
                /********************* Event For Account Component ****************************/
                this.btnCreditSearch.Click += new System.EventHandler(this.btnCreditSearch_Click);
                this.btnDiscountSearch.Click += new System.EventHandler(this.btnDiscountSearch_Click);

                this.lblCreditAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblCreditAccountID_Validating);
                this.lblDiscountAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblDiscountAccountID_Validating);

                this.lblCreditAccountID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
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
                this.txtVoucherDate.EditValueChanged += new System.EventHandler(this.txtGreDate_EditValueChanged);
                this.txtRegistrationNo.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtInvoiceID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDocumentID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtVoucherID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDelegateID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                //_____ Validating
                this.txtVoucherID.Validating += new System.ComponentModel.CancelEventHandler(this.txtVoucherID_Validating);
                this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
                this.txtHijriDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtHijriDate_Validating);

                //View Pic
                this.picItemImage.MouseLeave += new System.EventHandler(this.picItemImage_MouseLeave);
                this.picItemImage.MouseHover += new System.EventHandler(this.picItemImage_MouseHover);

                //Add Pic
                this.lnkAddImage.Click += new System.EventHandler(this.lnkAddImage_Click);
                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCheckSpendVoucher_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);
                DoNew();
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
            lstDetail = new BindingList<Acc_CheckSpendVoucherDetails>();
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

            gridView1.Columns["CheckSpendVoucherID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["CheckSpendVoucherMaster"].Visible = false;
            

            /******************* Columns Visible=true ********************/
            gridView1.Columns[AccountName].Visible = true;
            /******************* Columns Visible=true *******************/

            gridView1.Columns["DebitAmount"].Caption = CaptionDebitAmount;
            gridView1.Columns["AccountID"].Caption = CaptionAccountID;
            gridView1.Columns[AccountName].Caption = CaptionAccountName;
            gridView1.Columns[AccountName].Width = 150;
           
            gridView1.Columns["Declaration"].Caption = CaptionDeclaration;
            gridView1.Columns["Declaration"].Width = 150;
            gridView1.Columns["ReceiptName"].Caption = CaptionReceiptName;
            gridView1.Columns["CostCenterID"].Caption = CaptionCostCenterID;
            gridView1.Focus();
            /*************************Columns Properties ****************************/
         

            gridView1.Columns["CostCenterID"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmCheckSpendVoucherCostCenterID;
            gridView1.Columns["CostCenterID"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmCheckSpendVoucherCostCenterID;

            /************************ Look Up Edit **************************/
            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridView1.Columns[AccountName].ColumnEdit = rAccountName;
            gridControl.RepositoryItems.Add(rAccountName);


            RepositoryItemLookUpEdit rCostCenter = new RepositoryItemLookUpEdit();
            gridView1.Columns["CostCenterID"].OptionsColumn.AllowEdit = MySession.GlobalAllowChangefrmCheckSpendVoucherCostCenterID;
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
            if (ColName == "AccountID"  || ColName == "DebitAmount")
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
                    DataRow[] row = dt.Select("AccountID=" + e.Value.ToString());
                    if (row.Length == 0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisAccountID;
                    }
                    else
                    {
                        if (row[0]["AccountID"].ToString() == lblCreditAccountID.Text.Trim())
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgCanNotChoseSameAccount + " " + lblCreditAccountID.Text.Trim();
                        }
                        else { FileItemData(row[0]); }
                    }

                }

            }
            else if (ColName == "Declaration")
            {

                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;

                }
            }
            else if (ColName == AccountName)
            {
                DataTable dtAccountName = Lip.SelectRecord("Select AccountID, " + PrimaryName + " AS " + AccountName + " from Acc_Accounts Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + e.Value.ToString() + "') And FacilityID=" + UserInfo.FacilityID + "And BranchID = " + MySession.GlobalBranchID + " AND AccountLevel=" + MySession.GlobalNoOfLevels);
                if (dtAccountName == null && dtAccountName.Rows.Count == 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgNoFoundThisAccountID;
                }
                else
                {
                    if (dtAccountName.Rows[0]["AccountID"].ToString() == lblCreditAccountID.Text.Trim())
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgCanNotChoseSameAccount + " " + lblCreditAccountID.Text.Trim();
                    }
                    else
                    {
                        if (Lip.CheckTheAccountIsStope(Comon.cDbl(dtAccountName.Rows[0]["AccountID"]), Comon.cInt(MySession.GlobalBranchID)))
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
            else if (ColName == "Discount")
            {
                decimal DebitAmount = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DebitAmount").ToString());
                decimal PercentDiscount = Comon.ConvertToDecimalPrice(DebitAmount) * (Comon.ConvertToDecimalPrice(MySession.GlobalDiscountPercentCheckSpendVoucher) / 100);
                if (!(double.TryParse(e.Value.ToString(), out num)))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputShouldBeNumber;
                }
                else if (Comon.ConvertToDecimalPrice(e.Value.ToString()) > DebitAmount)
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
                    if (col.FieldName == "AccountID"  || col.FieldName == "DebitAmount")
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
            gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["CostCenterID"], MySession.GlobalDefaultCheckSpendVoucherCostCenterID);
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
                    if (ColName == "AccountID"  || ColName == "DebitAmount")
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
                    if (col.FieldName == "AccountID" || col.FieldName == "Declaration"  || col.FieldName == "DebitAmount")
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
                        if (col.FieldName == "AccountID" || col.FieldName == "DebitAmount")
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
                    if (col.FieldName == "AccountID"  || col.FieldName == "DebitAmount")
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
            //decimal DiscountRow = 0;
            decimal DebitAmountRow = 0;
            decimal DiscountTotal = 0;
            try
            {

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    DebitAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DebitAmount").ToString());
                    //DiscountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Discount").ToString());

                    CreditTotal += DebitAmountRow;
                    //DiscountTotal += DiscountRow;
                }
                if (gridView1.FocusedRowHandle < 0)
                {
                    var ResultDebitAmount = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DebitAmount");
                    //var ResultDiscount = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Discount");

                    DebitAmountRow = ResultDebitAmount != null ? Comon.ConvertToDecimalPrice(ResultDebitAmount.ToString()) : 0;
                    //DiscountRow = ResultDiscount != null ? Comon.ConvertToDecimalPrice(ResultDiscount.ToString()) : 0;

                    CreditTotal += DebitAmountRow;
                    //DiscountTotal += DiscountRow;
                }

                lblTotal.Text = CreditTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblDiscountTotal.Text = DiscountTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblNetBalance.Text = (CreditTotal - DiscountTotal).ToString("N" + MySession.GlobalNumDecimalPlaces);

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
        #region Other Function
        private void FileItemData(DataRow dr)
        {

            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], dr["AccountID"]);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AccountName], dr["ArbName"]);

            if (UserInfo.Language == iLanguage.English)
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AccountName], dr["EngName"].ToString());

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
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "CheckSpendVoucher", "رقـم الـشـيـك", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "CheckSpendVoucher", "Check ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == lblCreditAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSpendVoucherCreditAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, lblCreditAccountID, lblCreditAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, lblCreditAccountID, lblCreditAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCheckSpendVoucherPurchasesDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "PurchaseDelegateID", "رقم المـندوب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "PurchaseDelegateID", "Delegate ID", MySession.GlobalBranchID);
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
                else if (FocusedControl == lblCreditAccountID.Name)
                {
                    lblCreditAccountID.Text = cls.PrimaryKeyValue.ToString();
                    lblCreditAccountID_Validating(null, null);
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
                                // gridView1.AddNewRow();
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], Barcode);
                                DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                                DataRow[] row = dt.Select("AccountID=" + Barcode);
                                if (row.Length > 0)
                                    FileItemData(row[0]);
                                gridView1.FocusedColumn = gridView1.VisibleColumns[3];
                                gridView1.ShowEditor();


                                CalculatTotalBalance();
                            }
                        }
                        else
                        {

                            string Barcode = cls.PrimaryKeyValue.ToString();
                            gridView1.AddNewRow();
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], Barcode);
                            DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                            DataRow[] row = dt.Select("AccountID=" + Barcode);
                            if (row.Length > 0)
                                FileItemData(row[0]);
                            gridView1.FocusedColumn = gridView1.VisibleColumns[3];
                            gridView1.ShowEditor();


                            CalculatTotalBalance();

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
                  
                    dt = CheckSpendVoucherDAL.frmGetDataDetalByID(VoucherID, UserInfo.BRANCHID, UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        //Account

                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());


                        lblCreditAccountID.Text = dt.Rows[0]["BankID"].ToString();
                        lblCreditAccountID_Validating(null, null);

                        lblDiscountAccountID.Text = dt.Rows[0]["DiscountAccountID"].ToString();
                        lblDiscountAccountID_Validating(null, null);

                        //Masterdata
                        txtVoucherID.Text = dt.Rows[0]["CheckSpendVoucherID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();
                        txtWroteInCity.Text = dt.Rows[0]["WroteInCity"].ToString();
                        txtBeneficiaryName.Text = dt.Rows[0]["BeneficiaryName"].ToString();

                        //Combox
                     //   cmbBank.EditValue = Comon.cInt(dt.Rows[0]["BankID"].ToString());
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());

                        //Validate
                        //txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        //txtDelegateID_Validating(null, null);

                        //Date
                        //Date
                        txtVoucherDate.EditValue = Comon.ConvertSerialDateTo(dt.Rows[0]["CheckSpendVoucherDate"].ToString());
                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["CheckSpendVoucherDate"].ToString()) == "")
                            txtVoucherDate.Text = "";
                        else
                            txtVoucherDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["CheckSpendVoucherDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

                        //  txtVoucherDate.DateTime = Convert.ToDateTime(Comon.ConvertSerialDateTo(dt.Rows[0]["CheckReceiptVoucherDate"].ToString()));


                        //   txtGreDate.EditValue = Comon.ConvertSerialDateTo(dt.Rows[0]["GreDate"].ToString());
                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["GreDate"].ToString()) == "")
                            txtGreDate.Text = "";
                        else
                            txtGreDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["GreDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

                      
                        txtHijriDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["HijriDate"].ToString());
                        //Ammount

                        lblTotal.Text = dt.Rows[0]["DebitAmount"].ToString();
                        lblDiscountTotal.Text = dt.Rows[0]["DiscountAmount"].ToString();
                        lblNetBalance.Text = (Comon.ConvertToDecimalPrice(lblTotal.Text.Trim()) - Comon.ConvertToDecimalPrice(lblDiscountTotal.Text.Trim())).ToString();

                        //byte[] imgByte = null;
                        //if (DBNull.Value != dt.Rows[0]["SpendImage"])
                        //{
                        //    imgByte = (byte[])dt.Rows[0]["SpendImage"];
                        //    picItemImage.Image = byteArrayToImage(imgByte);
                        //}
                        //else
                        //    picItemImage.Image = null;

                        //GridVeiw

                        gridControl.DataSource = dt;

                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;

                        ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtVoucherID.Text;
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
            #region get accounts declaration
            lblCreditAccountID.Text = MySession.GlobalDefaultCheckSpendVoucherCrditAccountID;
            lblCreditAccountID_Validating(null, null);
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
                txtNotes.Text = "";

                txtVoucherDate.EditValue = DateTime.Now;
                txtGreDate.EditValue = DateTime.Now;

                txtWroteInCity.Text = "";
                txtBeneficiaryName.Text = "";
                txtNotes.Text = "";
                lblCreditAccountID.Text = "";
                lblCreditAccountName.Text = "";

                lblTotal.Text = "0";
                lblDiscountTotal.Text = "0";
                lblNetBalance.Text = "0";

                txtDelegateID.Text = MySession.GlobalDefaultCheckSpendVoucherPurchasesDelegateID;
                txtDelegateID_Validating(null, null);
                cmbCurency.EditValue = MySession.GlobalDefaultCheckSpendVoucherCurencyID;
               // cmbBank.EditValue = MySession.GlobalDefaultCheckSpendVoucherBankID;
                GetAccountsDeclaration();

                picItemImage.Image = null;
                lstDetail = new BindingList<Acc_CheckSpendVoucherDetails>();

                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;

                dt = new DataTable();

                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtVoucherID.Text;

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
                    strSQL = "SELECT TOP 1 * FROM " + CheckSpendVoucherDAL.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + CheckSpendVoucherDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + CheckSpendVoucherDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + CheckSpendVoucherDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + CheckSpendVoucherDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + CheckSpendVoucherDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + CheckSpendVoucherDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new CheckSpendVoucherDAL();

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
                IsNewRecord = true;
                txtVoucherID.Text = CheckSpendVoucherDAL.GetNewID().ToString();
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
            DataTable dtItem = new DataTable();
            editMode = true;
            dtItem.Columns.Add("BranchID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("AccountID", System.Type.GetType("System.String"));
            dtItem.Columns.Add(AccountName, System.Type.GetType("System.String")); 
            dtItem.Columns.Add("CostCenterID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("DebitAmount", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Declaration", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ReceiptName", System.Type.GetType("System.String"));

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["BranchID"] = MySession.GlobalBranchID;
                dtItem.Rows[i]["FacilityID"] = MySession.GlobalFacilityID;
                dtItem.Rows[i]["AccountID"] = gridView1.GetRowCellValue(i, "AccountID").ToString();
                dtItem.Rows[i][AccountName] = gridView1.GetRowCellValue(i, AccountName).ToString(); 
                dtItem.Rows[i]["CostCenterID"] = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                dtItem.Rows[i]["DebitAmount"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DebitAmount").ToString());
                dtItem.Rows[i]["ReceiptName"] = gridView1.GetRowCellValue(i, "ReceiptName").ToString();
                dtItem.Rows[i]["Declaration"] = gridView1.GetRowCellValue(i, "Declaration").ToString();


            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
            gridView1.Focus();
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            gridView1.ShowEditor();

        }
        private void Save()
        {
            gridView1.MoveLastVisible();
            CalculatTotalBalance();
            txtGreDate_EditValueChanged(null, null);
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            int VoucherID = Comon.cInt(txtVoucherID.Text);

            Acc_CheckSpendVoucherMaster objRecord = new Acc_CheckSpendVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);

            //Date
            objRecord.CheckSpendVoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString();
            objRecord.GreDate = Comon.ConvertDateToSerial(txtGreDate.Text).ToString();
            objRecord.HijriDate = Comon.ConvertDateToSerial(txtHijriDate.Text).ToString();
            //Combox 
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
          //  objRecord.BankID = Comon.cInt(cmbBank.EditValue.ToString());
            //Master Data
            objRecord.BeneficiaryName = txtBeneficiaryName.Text.Trim();
            objRecord.WroteInCity = txtWroteInCity.Text.Trim();
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);

            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Check Spend Voucher" : "شيك الصرف ");
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? " Check Spend Voucher" : "شيك الصرف "));
            objRecord.Notes = txtNotes.Text;
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);

            //Account
            objRecord.BankID = Comon.cDbl(lblCreditAccountID.Text);
            objRecord.DiscountAccountID = Comon.cDbl(lblDiscountAccountID.Text);
            //Ammount
            objRecord.DiscountAmount = Comon.cDbl(lblDiscountTotal.Text);
            objRecord.CreditAmount = Comon.cDbl(lblTotal.Text);

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
                objRecord.CheckSpendVoucherID = VoucherID;
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


            Acc_CheckSpendVoucherDetails returned;
            List<Acc_CheckSpendVoucherDetails> listreturned = new List<Acc_CheckSpendVoucherDetails>();

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Acc_CheckSpendVoucherDetails();
                returned.ID = i;
                returned.BranchID = UserInfo.BRANCHID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                returned.AccountName =  gridView1.GetRowCellValue(i, "AccountName").ToString();
                returned.CheckSpendVoucherID = VoucherID;
                returned.DebitAmount = Comon.cDbl(gridView1.GetRowCellValue(i, "DebitAmount").ToString()); 
                returned.ReceiptName = gridView1.GetRowCellValue(i, "ReceiptName") == null ? " " : gridView1.GetRowCellValue(i, "ReceiptName").ToString();
                returned.Declaration = gridView1.GetRowCellValue(i, "Declaration").ToString();
                returned.CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                listreturned.Add(returned);

            }

            if (listreturned.Count > 0)
            {
                objRecord.CheckSpendVoucherDetails = listreturned;
                int Result = CheckSpendVoucherDAL.InsertUsingXML(objRecord, IsNewRecord);
                if (Comon.cInt(Result) > 0)
                {
                    //حفظ القيد الالي
                    long ID = SaveVariousVoucherMachin(Comon.cInt(txtVoucherID.Text));
                    if (ID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + ReceiptVoucherDAL.TableName + " Set RegistrationNo =" + ID + " where " + ReceiptVoucherDAL.PremaryKey + " = " + txtVoucherID.Text);

                }
                SplashScreenManager.CloseForm(false);

                if (IsNewRecord == true)
                {
                    if (Result >0 )
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
                // Debit Account
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID")), Comon.cInt(MySession.GlobalBranchID), Comon.cDec(gridView1.GetRowCellValue(i, "DebitAmount")), 1)==1)
                    {
                        SplashScreenManager.CloseForm(false);
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgAccountMaxLimit + " " + gridView1.GetRowCellValue(i, AccountName).ToString());
                        return;
                    }
                    else if (Lip.CheckTheAccountMaxLimit(Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID")), Comon.cInt(MySession.GlobalBranchID), Comon.cDec(gridView1.GetRowCellValue(i, "DebitAmount")), 1) == 2)
                    {
                        SplashScreenManager.CloseForm(false);
                        bool Yes = Messages.MsgQuestionYesNo(Messages.TitleInfo, Messages.msgAccountMaxLimitSaveOrNot + " " + gridView1.GetRowCellValue(i, AccountName).ToString());
                        if (!Yes)
                            return;
                    }
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

                Acc_CheckSpendVoucherMaster model = new Acc_CheckSpendVoucherMaster();
                model.CheckSpendVoucherID = Comon.cInt(txtVoucherID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                bool Result = CheckSpendVoucherDAL.DeleteAcc_CheckSpendVoucherMaster(model);
                if (Comon.cInt(Result) >= 0)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    MoveRec(model.CheckSpendVoucherID, xMovePrev);
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

                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");


                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["VoucherID"].Value = txtVoucherID.Text.Trim().ToString();
                rptForm.Parameters["VoucherDate"].Value = txtVoucherDate.Text.Trim().ToString();
                rptForm.Parameters["DocumentID"].Value = txtDocumentID.Text.Trim().ToString();
                rptForm.Parameters["InvoiceID"].Value = txtInvoiceID.Text.Trim().ToString();
                rptForm.Parameters["DelegateName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["RegistrationNo"].Value = txtRegistrationNo.Text.Trim().ToString();
                rptForm.Parameters["txtBeneficiaryName"].Value = txtBeneficiaryName.Text.Trim().ToString();
                rptForm.Parameters["BankName"].Value = lblCreditAccountName.Text.Trim().ToString();
                rptForm.Parameters["WroteInCity"].Value = txtWroteInCity.Text.Trim().ToString();
                rptForm.Parameters["GreDate"].Value = txtGreDate.Text.Trim().ToString();

                /********Total*********/
                rptForm.Parameters["Total"].Value = lblTotal.Text.Trim().ToString();
                rptForm.Parameters["DiscountTotal"].Value = lblDiscountTotal.Text.Trim().ToString();
                rptForm.Parameters["NetBalance"].Value = lblNetBalance.Text.Trim().ToString();


                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptCheckSpendVoucherDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["DebitAmount"] = gridView1.GetRowCellValue(i, "DebitAmount").ToString();
                    //row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["AccountID"] = gridView1.GetRowCellValue(i, "AccountID").ToString();
                    row["AccountName"] = gridView1.GetRowCellValue(i, AccountName).ToString();
                    row["Declaration"] = gridView1.GetRowCellValue(i, "Declaration").ToString();
                    row["ReceiptName"] = gridView1.GetRowCellValue(i, "ReceiptName").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterID").ToString();
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
            string Release = "شيك صرف رقم";
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
                Record.TransType = 1;
                Record.RegistrationDate = Comon.cDbl(Comon.ConvertDateToSerial(txtVoucherDate.Text));
                Record.Acc_code = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                Record.Master_code = 0;
                Record.Debt = Comon.cDbl(gridView1.GetRowCellValue(i, "DebitAmount").ToString());
                Record.Credit = 0;
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
                Record.AccountNumCorresponding = lblCreditAccountID.Text.Trim();
                Record.Receivables = "";
                CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                Record.CostCenterNo = CostCenterID;
                Record.posted = 1;
                Record.Cancel = 0;
                listRecord.Add(Record);


            }
            //***************** Credit AccountID ********************/
            RestrictionsDaily Record2 = new RestrictionsDaily();
            Record2.ID = gridView1.DataRowCount;
            Record2.RegistrationNo = MaxRegistrationNo;
            Record2.BranchNum = UserInfo.BRANCHID;
            Record2.FacilityID = UserInfo.FacilityID;
            Record2.TranNo = VoucherID;
            Record2.TransType = 1;
            Record2.RegistrationDate = Comon.cDbl(Comon.ConvertDateToSerial(txtVoucherDate.Text));
            Record2.Acc_code = Comon.cDbl(lblCreditAccountID.Text);
            Record2.Master_code = 0;
            Record2.Debt = 0;
            Record2.Credit = Comon.cDbl(lblNetBalance.Text.ToString());
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
            Record2.posted = 1;
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
                Record3.TransType = 1;
                Record3.RegistrationDate = Comon.cDbl(Comon.ConvertDateToSerial(txtVoucherDate.Text));
                Record3.Acc_code = Comon.cDbl(lblDiscountAccountID.Text);
                Record3.Master_code = 0;
                Record3.Debt = 0;
                Record3.Credit = Comon.cDbl(lblDiscountTotal.Text.ToString());
                Record3.Discount = 0;
                Record3.Release = "ماتم خصمه لمذكورين شيك صرف رقم " + VoucherID;
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
                Record3.posted = 1;
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
        private void frmCheckSpendVoucher_Load(object sender, EventArgs e)
        {
            gridView1.Focus();
            gridView1.MoveLast();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.ShowEditor();

        }

        private void frmCheckSpendVoucher_KeyDown(object sender, KeyEventArgs e)
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
                strSQL = "SELECT ArbName as DelegateName FROM Sales_PurchasesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
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
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE (BranchID = " + UserInfo.BRANCHID + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblCreditAccountID.Text + ") ";
                CSearch.ControlValidating(lblCreditAccountID, lblCreditAccountName, strSQL);
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

                if (col.FieldName == "DebitAmount" || col.FieldName == "AccountID" || col.FieldName == "Declaration" || col.FieldName == "Discount" || col.FieldName == "CostCenterID" || col.FieldName == "ReceiptName")
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }
            }
            if (Value)
                RolesButtonSearchAccountID();
      
        }
        private void RolesButtonSearchAccountID()
        {

            btnCreditSearch.Enabled = MySession.GlobalAllowChangefrmCheckSpendVoucherCreditAccountID;
            btnDiscountSearch.Enabled = MySession.GlobalAllowChangefrmCheckSpendVoucherDiscountAccountID;
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
                sc = new SqlCommand("Update  " + CheckSpendVoucherDAL.TableName + " Set SpendImage=@p Where " + CheckSpendVoucherDAL.PremaryKey + "=" + txtVoucherID.Text + " And BranchID=" + UserInfo.BRANCHID, Con);
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
        private void txtGreDate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtVoucherDate.Text.Trim()))
                    txtVoucherDate.EditValue = DateTime.Now;
                //if (Comon.ConvertDateToSerial(txtVoucherDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                //    txtVoucherDate.Text = Lip.GetServerDate();
                if (Lip.CheckDateISAvilable(txtVoucherDate.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                   txtVoucherDate.Text = Lip.GetServerDate();
                    return;
                }
                TextEdit obj = (TextEdit)sender;
                string Date = Comon.ConvertDateToSerial(obj.Text).ToString();
                int year = Convert.ToInt32(Date.Substring(0, 4));
                int month = Convert.ToInt32(Date.Substring(4, 2));
                int day = Convert.ToInt32(Date.Substring(6, 2));
                DateTime tempDate = new DateTime(year, month, day);
                txtHijriDate.Text = Comon.ConvertFromEngDateToHijriDate(tempDate).Substring(0, 10);
            }
            catch { }
        }

        private void txtHijriDate_Validating(object sender, CancelEventArgs e)
        {
            try{
            TextEdit obj = (TextEdit)sender;
            string Date = obj.Text;
            if (Date != "" && Date != "0")
                txtGreDate.EditValue = Comon.ConvertFromHijriDateToEngDate(Date);
            }
            catch { }
        }
        #endregion

        private void frmCheckSpendVoucher_Load_1(object sender, EventArgs e)
        {

        }

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

        private void btnMachinResraction_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " +  Comon.cInt(MySession.GlobalBranchID) + " And DocumentID=" + txtVoucherID.Text + " And DocumentType=" + DocumentType).ToString());
            if (ID > 0)
            {
                frmVariousVoucherMachin frm22 = new frmVariousVoucherMachin();
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm22);
                frm22.FormView = true;
                frm22.FormAdd = false;
                frm22.Show();
                frm22.cmbBranchesID.EditValue = Comon.cInt(MySession.GlobalBranchID);
                frm22.ReadRecord(Comon.cLong(ID.ToString()));
            }
            else
                Messages.MsgError("تنبيه", "   لا يوجد قيد - الرجاء اعادة حفظ المستند ");
        }
        long SaveVariousVoucherMachin(int DocumentID)
        {

            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" +  Comon.cInt(MySession.GlobalBranchID)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(MySession.GlobalBranchID);
            objRecord.FacilityID = UserInfo.FacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString();
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cBooleanToInt(chkPoste.Checked);

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
          
            #region normal
            if (txtInvoiceID.Text.Trim() == "" || Comon.cInt(txtInvoiceID.Text) <= 0)
            {
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    returned = new Acc_VariousVoucherMachinDetails();
                    returned.ID = 1;
                    returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                    returned.VoucherID = VoucherID;
                    returned.Credit = 0;       
                    returned.Debit = Comon.cDbl(gridView1.GetRowCellValue(i, "DebitAmount"));           
                    returned.Declaration = gridView1.GetRowCellValue(i, "Declaration").ToString();
                    returned.CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                    listreturned.Add(returned);
                }
                
                //Credit Vat
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 3;
                returned.BranchID = Comon.cInt(MySession.GlobalBranchID);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(lblCreditAccountID.Text);
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(lblNetBalance.Text);
                returned.Debit = 0;
                returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCheckReceiptVoucherCostCenterID);
                listreturned.Add(returned);
                //=

            }
            #endregion
           
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        public void Transaction()
        {


            strSQL = "Select * from Acc_CheckSpendVoucherMaster Where Cancel=0";
            DataTable dtSend = new DataTable();
            dtSend = Lip.SelectRecord(strSQL);
            if (dtSend.Rows.Count > 0)
            {
                for (int i = 0; i <= dtSend.Rows.Count - 1; i++)
                {
                    txtVoucherID.Text = dtSend.Rows[i]["CheckSpendVoucherID"].ToString();
                

                    txtVoucherID_Validating(null, null);
                    IsNewRecord = true;
                    if (Comon.cInt(txtVoucherID.Text) > 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(Comon.cInt(txtVoucherID.Text));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update Acc_SpendVoucherMaster Set DocumentID =" + VoucherID + " where SpendVoucherID = " + txtVoucherID.Text + " AND BranchID=" +  Comon.cInt(MySession.GlobalBranchID));

                    }



                }

                this.Close();
            }
        }

        private void chkPoste_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

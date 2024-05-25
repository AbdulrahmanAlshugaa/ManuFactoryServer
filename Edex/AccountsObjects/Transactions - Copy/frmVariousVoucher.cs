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
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Edex.AccountsObjects.Transactions
{
    public partial class frmVariousVoucher : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        DataTable dtDeclaration;
        string FocusedControl = "";
        public CultureInfo culture = new CultureInfo("en-US");
        private VariousVoucherDAL cClass;
        private string strSQL;
        public bool isNewReg = true;
        private string PrimaryName;
        private string AccountName;
        private string CaptionCredit;
        private string CaptionDebitAmount;
        private string CaptionAccountID;
        private string CaptionAccountName;
        private string CaptionDeclaration;
        private string CaptionCostCenterID;
        private bool IsNewRecord;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
         public const int DocumentType = 6;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;

        DataTable dt = new DataTable();
        //all record master and detail
        BindingList<Acc_VariousVoucherDetails> AllRecords = new BindingList<Acc_VariousVoucherDetails>();

        //list detail
        BindingList<Acc_VariousVoucherDetails> lstDetail = new BindingList<Acc_VariousVoucherDetails>();

        //Detail
        Acc_VariousVoucherDetails BoDetail = new Acc_VariousVoucherDetails();

        #endregion
        public frmVariousVoucher()
        {

            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                lblDifference.BackColor = Color.WhiteSmoke;
                lblDifference.ForeColor = Color.Black;
                AccountName = "ArbAccountName";
                PrimaryName = "ArbName";
                CaptionDebitAmount = "مديـن";
                CaptionCredit = "دائــن";
                CaptionAccountID = "رقم الحساب";
                CaptionAccountName = "اسم الحساب";
                CaptionDeclaration = "الـبـيـــــان";
                CaptionCostCenterID = "مركز تكلفة";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    AccountName = "EngAccountName";
                    PrimaryName = "EngName";

                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");
                    CaptionDebitAmount = "Debit";
                    CaptionCredit = "Credit";
                    CaptionAccountID = "Account ID";
                    CaptionAccountName = "Account Name";
                    CaptionDeclaration = "Declaration";
                    CaptionCostCenterID = "Cost Center";
                }
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
                FillCombo.FillComboBoxLookUpEdit(cmbCurency, "Currency", "CurrencyID", PrimaryName, "", " BranchID = " + Comon.cInt(cmbBranchesID.EditValue));
                /***********************Component ReadOnly  ****************************/
                TextEdit[] txtEdit = new TextEdit[1];
                txtEdit[0] = lblDelegateName;
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
                //txtVoucherDate.ReadOnly = !MySession.GlobalAllowChangefrmVariousVoucherDate;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmVariousVoucherCurencyID;
                txtDelegateID.ReadOnly = !MySession.GlobalAllowChangefrmVariousVoucherSalesDelegateID;



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
                this.txtDocumentID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDocumentID_Validating);
                this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);

                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmVariousVoucher_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);
                DoNew();

                if (UserInfo.ID == 1 || UserInfo.ID == 2)
                {
                    cmbBranchesID.Visible = true;
                    labelControl5.Visible = true;
                }
                else
                {
                    labelControl5.Visible = false;
                    cmbBranchesID.Visible = false;
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
        #region GridView
        void InitGrid()
        {
            MySession.GlobalAccountsLevelDigits = 4;
            MySession.GlobalNoOfLevels = 4;
            lstDetail = new BindingList<Acc_VariousVoucherDetails>();
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
            gridView1.Columns["VoucherID"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["VariousVoucherMaster"].Visible = false;
            gridView1.Columns["ArbAccountName"].Visible = false;
            gridView1.Columns["EngAccountName"].Visible = false;
            gridView1.Columns["AccountAssest"].Visible = false;
            /******************* Columns Visible=true ********************/
            gridView1.Columns[AccountName].Visible = true;
            /******************* Columns Visible=true *******************/
            gridView1.Columns["Credit"].Caption = CaptionCredit;
            gridView1.Columns["Debit"].Caption = CaptionDebitAmount;
            gridView1.Columns["AccountID"].Caption = CaptionAccountID;
            gridView1.Columns[AccountName].Caption = CaptionAccountName;
            gridView1.Columns[AccountName].Width = 150;
            gridView1.Columns["Declaration"].Caption = CaptionDeclaration;
            gridView1.Columns["Declaration"].Width = 150;
            gridView1.Columns["CostCenterID"].Caption = CaptionCostCenterID;
            gridView1.Focus();
            /*************************Columns Properties ****************************/
            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridView1.Columns[AccountName].ColumnEdit = rAccountName;
            gridControl.RepositoryItems.Add(rAccountName);
            gridView1.Columns["CostCenterID"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmVariousVoucherCostCenterID;
            gridView1.Columns["CostCenterID"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmVariousVoucherCostCenterID;
            /************************ Look Up Edit **************************/
            RepositoryItemLookUpEdit rCostCenter = new RepositoryItemLookUpEdit();
            gridView1.Columns["CostCenterID"].OptionsColumn.AllowEdit = MySession.GlobalAllowChangefrmVariousVoucherCostCenterID;
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
            if (ColName == "AccountID"  || ColName == "Credit" || ColName == "Debit")
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
                    if (ColName == "Credit" || ColName == "Debit")
                    {
                        var Credit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Credit"]);
                        var Debit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Debit"]);
                        var AccountID = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["AccountID"]);

                        if (AccountID.ToString() != "0" && Comon.ConvertToDecimalPrice(Credit.ToString()) <= 0 && Comon.ConvertToDecimalPrice(Debit.ToString()) <= 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgInputIsGreaterThanZero;
                        }
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }
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
                        FileItemData(row[0]);
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
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], dtAccountName.Rows[0]["AccountID"]);
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AccountName], dtAccountName.Rows[0][AccountName]);

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
            if (ColName == "Debit")
            {
                decimal Debit = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debit").ToString());
                if (Debit > 0)
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Credit"], 0);
                }

            }
            else if (ColName == "Credit")
            {
                decimal Credit = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit").ToString());
                if (Credit > 0)
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Debit"], 0);
                }
            }

        }
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                HasColumnErrors = false;
                GridView view = sender as GridView;
                view.ClearColumnErrors();
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "AccountID"|| col.FieldName == "Credit" || col.FieldName == "Debit")
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
                            if (col.FieldName == "Credit" || col.FieldName == "Debit")
                            {
                                var Credit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Credit"]);
                                var Debit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Debit"]);
                                if (Comon.ConvertToDecimalPrice(Credit.ToString()) <= 0 && Comon.ConvertToDecimalPrice(Debit.ToString()) <= 0)
                                {
                                    e.Valid = false;
                                    HasColumnErrors = true;
                                    e.ErrorText = Messages.msgInputIsGreaterThanZero;

                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgInputIsGreaterThanZero;
                            }
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
            gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["CostCenterID"], MySession.GlobalDefaultVariousVoucherCostCenterID);
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
                view.ClearColumnErrors();
                if (view.FocusedColumn == null)
                    return;
                HasColumnErrors = false;
                if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {

                    double num;
                    HasColumnErrors = false;
                    var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                    string ColName = view.FocusedColumn.FieldName;
                    if (ColName == "AccountID" || ColName == "Credit" || ColName == "Debit")
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
                            if (ColName == "Credit" || ColName == "Debit")
                            {
                                var Credit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Credit"]);
                                var Debit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Debit"]);
                                if (Comon.ConvertToDecimalPrice(Credit.ToString()) <= 0 && Comon.ConvertToDecimalPrice(Debit.ToString()) <= 0)
                                {
                                    HasColumnErrors = true;
                                    view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);

                                }
                            }
                            else
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                         
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
            GridView view = gridControl.FocusedView as GridView;
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
                    if (col.FieldName == "AccountID" || col.FieldName == "Declaration"  || col.FieldName == "Credit" || col.FieldName == "Debit")
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
                        if (col.FieldName == "AccountID"  || col.FieldName == "Credit" || col.FieldName == "Debit")
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

                                if (col.FieldName == "Credit" || col.FieldName == "Debit")
                                {
                                    var Credit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Credit"]);
                                    var Debit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Debit"]);
                                    if (Comon.ConvertToDecimalPrice(Credit.ToString()) <= 0 && Comon.ConvertToDecimalPrice(Debit.ToString()) <= 0)
                                    {
                                        gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                                        Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                                        return false;

                                    }
                                }
                                else
                                {
                                    gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                                    Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                                    return false;
                                }
                   
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
                    if (col.FieldName == "AccountID"  || col.FieldName == "Credit" || col.FieldName == "Debit")
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
                            if (col.FieldName == "Credit" || col.FieldName == "Debit")
                            {
                                var Credit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Credit"]);
                                var Debit = view.GetRowCellValue(view.FocusedRowHandle, gridView1.Columns["Debit"]);
                                if (Comon.ConvertToDecimalPrice(Credit.ToString()) <= 0 && Comon.ConvertToDecimalPrice(Debit.ToString()) <= 0)
                                {
                                    gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                                    Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                                    return false;

                                }
                            }
                            else
                            {
                                gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                                return false;
                            }
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

            decimal DebitAmountRow = 0;
            decimal CreditRow = 0;
            decimal DebitTotal = 0;
            decimal CreditTotal = 0;
            try
            {

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    CreditRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Credit").ToString());
                    DebitAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Debit").ToString());

                    CreditTotal += CreditRow;
                    DebitTotal += DebitAmountRow;
                }
                if (gridView1.FocusedRowHandle < 0)
                {
                    var ResultCredit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit");
                    var ResultDebitAmount = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debit");

                    CreditRow = ResultCredit != null ? Comon.ConvertToDecimalPrice(ResultCredit.ToString()) : 0;
                    DebitAmountRow = ResultDebitAmount != null ? Comon.ConvertToDecimalPrice(ResultDebitAmount.ToString()) : 0;

                    CreditTotal += CreditRow;
                    DebitTotal += DebitAmountRow;
                }

                lblTotalCredit.Text = CreditTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblTotalDebit.Text = DebitTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblDifference.Text = (DebitTotal - CreditTotal).ToString("N" + MySession.GlobalNumDecimalPlaces);
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

            if (UserInfo.Language == iLanguage.English)
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngAccountName"], dr["EngName"].ToString());

        }
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where BranchID=" + Comon.cInt(cmbBranchesID.EditValue);

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl.Trim() == txtVoucherID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "VariousVoucherID", "رقم السـند", Comon.cInt( cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "VariousVoucherID", "Voucher ID", Comon.cInt(cmbBranchesID.EditValue));
            }

            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                if (!MySession.GlobalAllowChangefrmVariousVoucherSalesDelegateID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", Comon.cInt(cmbBranchesID.EditValue));
            }

            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;
                if (gridView1.FocusedColumn.Name == "colAccountID" || gridView1.FocusedColumn.Name == "colAccountName")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
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
                else if (FocusedControl == txtDelegateID.Name)
                {
                    txtDelegateID.Text = cls.PrimaryKeyValue.ToString();
                    txtDelegateID_Validating(null, null);
                }
                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colAccountID" || gridView1.FocusedColumn.Name == "colAccountName")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], Barcode);
                        DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                        DataRow[] row = dt.Select("AccountID=" + Barcode);
                        FileItemData(row[0]);
                        gridView1.FocusedColumn = gridView1.VisibleColumns[3];
                        gridView1.ShowEditor();
                        

                        CalculatTotalBalance();

                    }
                }
            }

        }
        public void ReadRecord(long VoucherID, bool flag=false)
        {
            try
            { 
                ClearFields();
                {
                    
                  dt = VariousVoucherDAL.frmGetDataDetalByID(VoucherID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
     
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        IsNewRecord = false;
                        //Masterdata
                        txtVoucherID.Text = dt.Rows[0]["VoucherID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        // txtInvoiceID.Text = dt.Rows[0]["InvoiceID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());

                        //Validate
                        txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        txtDelegateID_Validating(null, null);
                        //Date

                        txtVoucherDate.EditValue = Comon.ConvertSerialDateTo(dt.Rows[0]["VoucherDate"].ToString());

                        chkPoste.Checked = Comon.cIntToBoolean(Comon.cInt( dt.Rows[0]["Posted"].ToString()));


                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["VoucherDate"].ToString()) == "")
                            txtVoucherDate.Text = "";

                        else
                            txtVoucherDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["VoucherDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

                           // txtVoucherDate.DateTime = Convert.ToDateTime(Comon.ConvertSerialDateTo(dt.Rows[0]["VoucherDate"].ToString()));

                      
                        //Ammount
                        lblTotalDebit.Text = dt.Rows[0]["Debit"].ToString();
                        lblTotalCredit.Text = dt.Rows[0]["Credit"].ToString();
                        lblDifference.Text = (Comon.ConvertToDecimalPrice(lblTotalDebit.Text.Trim()) - Comon.ConvertToDecimalPrice(lblTotalCredit.Text.Trim())).ToString();

                        //GridVeiw
                        gridControl.DataSource = dt;

                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;

                        CalculatTotalBalance();
                    }
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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
               
                txtDocumentID.Text = "";
                txtDelegateID.Text = "";
                lblDelegateName.Text = "";
                txtNotes.Text = "";
                chkPoste.Checked = false;
                txtVoucherDate.EditValue = DateTime.Now;

                txtNotes.Text = "";


                lblTotalCredit.Text = "0";
                lblTotalDebit.Text = "0";
                lblDifference.Text = "0";


                cmbCurency.EditValue = MySession.GlobalDefaultVariousVoucherCurencyID;
                txtDelegateID.Text = MySession.GlobalDefaultVariousVoucherSalesDelegateID;
                txtDelegateID_Validating(null, null);

                lstDetail = new BindingList<Acc_VariousVoucherDetails>();

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
                    strSQL = "SELECT TOP 1 * FROM " + VariousVoucherDAL.TableName + " Where Cancel =0  and  " + VariousVoucherDAL.PremaryKey +">0";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + VariousVoucherDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + VariousVoucherDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + VariousVoucherDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + VariousVoucherDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + VariousVoucherDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + VariousVoucherDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new VariousVoucherDAL();

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
                txtVoucherID.Text = VariousVoucherDAL.GetNewID().ToString();
                txtRegistrationNo.Text = RestrictionsDailyDAL.GetNewID(this.Name).ToString();
                ClearFields();
                EnabledControl(true);

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

            dtItem.Columns.Add("BranchID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("AccountID", System.Type.GetType("System.String"));
            dtItem.Columns.Add(AccountName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("Debit", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostCenterID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("Credit", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Declaration", System.Type.GetType("System.String"));

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["BranchID"] = MySession.GlobalBranchID;
                dtItem.Rows[i]["FacilityID"] = MySession.GlobalFacilityID;
                dtItem.Rows[i]["AccountID"] = gridView1.GetRowCellValue(i, "AccountID").ToString();
                dtItem.Rows[i][AccountName] = gridView1.GetRowCellValue(i, AccountName).ToString();
                dtItem.Rows[i]["CostCenterID"] = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                dtItem.Rows[i]["Debit"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Debit").ToString());
                dtItem.Rows[i]["Credit"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Credit").ToString());
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
            txtVoucherDate_EditValueChanged(null, null);
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];
            int VoucherID = Comon.cInt(txtVoucherID.Text);
            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();
            objRecord.BranchID =Comon.cInt( cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());

            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text;
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);

             objRecord.Cancel = 0;
             objRecord.Posted =Comon.cBooleanToInt( chkPoste.Checked);
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
                objRecord.VoucherID = VoucherID;
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Acc_VariousVoucherDetails();
                returned.ID = i;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(gridView1.GetRowCellValue(i, "Credit").ToString());
                returned.Debit = Comon.cDbl(gridView1.GetRowCellValue(i, "Debit").ToString());
                returned.Declaration = gridView1.GetRowCellValue(i, "Declaration").ToString();
                returned.CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                listreturned.Add(returned);
              }

            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                long Result = VariousVoucherDAL.InsertUsingXML(objRecord, MySession.UserID);
                SplashScreenManager.CloseForm(false);

                if (IsNewRecord == true)
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
        protected override void DoSave()
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;
                if (!IsValidGrid())
                    return;
                if (Comon.ConvertToDecimalPrice(lblDifference.Text) != 0)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "The Net Balance Must be 0 " : "يجب ان يكون طرفا القيد متعاديلين"));
                    return;
                }
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

                Acc_VariousVoucherMaster model = new Acc_VariousVoucherMaster();
                model.VoucherID = Comon.cInt(txtVoucherID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                bool Result = VariousVoucherDAL.DeleteAcc_VariousVoucherMaster(model);
                if (Comon.cInt(Result) >= 0)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    MoveRec(model.VoucherID, xMovePrev);
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
                ReportName = "rptVariousVoucher";
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
                rptForm.Parameters["TotalCredit"].Value = lblTotalCredit.Text.Trim().ToString();
                rptForm.Parameters["TotalDebit"].Value = lblTotalDebit.Text.Trim().ToString();
                rptForm.Parameters["Difference"].Value = lblDifference.Text.Trim().ToString();

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;

                /********************** Details ****************************/
                var dataTable = new dsReports.rptVariousVoucherDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["Credit"] = gridView1.GetRowCellValue(i, "Credit").ToString();
                    row["Debit"] = gridView1.GetRowCellValue(i, "Debit").ToString();
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

        
        #endregion
        #endregion
        #region Event
        /************************Event From **************************/
        private void txtRegistrationNo_Validated(object sender, EventArgs e)
        {
            //if (FormView == true)
            //    if (   IsNewRecord != true)
            //    ReadRecord(Comon.cLong(txtRegistrationNo.Text),true);
            //else
            //{
            //    Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
            //    return;
            //}
        }
        private void frmVariousVoucher_Load(object sender, EventArgs e)
        {
            gridView1.Focus();
            gridView1.MoveLast();
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];
            gridView1.ShowEditor();
            
        }

        private void frmVariousVoucher_KeyDown(object sender, KeyEventArgs e)
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
        private void txtDocumentID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
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
                strSQL = "SELECT ArbName as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        #region Search
        /***************************Event Search ***************************/

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


            foreach (GridColumn col in gridView1.Columns)
            {

                if (col.FieldName == "Credit" || col.FieldName == "Debit" || col.FieldName == "AccountID" || col.FieldName == "Declaration" || col.FieldName == "CostCenterID")
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }
            }
            btnAttachments.Enabled = Value;

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

        private void btnAttachments_Click(object sender, EventArgs e)
        {
            //frmVariousVoucherAttachment frm = new frmVariousVoucherAttachment();
            //frm.VoucherID = Comon.cInt(txtVoucherID.Text);
            //frm.ShowDialog();
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
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
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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
            
        }

        private void frmVariousVoucher_Load_1(object sender, EventArgs e)
        {
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue =  UserInfo.BRANCHID; ;

        }

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            DoFirst();
        }
    }
}

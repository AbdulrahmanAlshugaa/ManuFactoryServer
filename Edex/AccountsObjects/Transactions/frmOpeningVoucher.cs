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
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Edex.AccountsObjects.Transactions
{
    public partial class frmOpeningVoucher : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        public const int DocumentType = 0;
        DataTable dtDeclaration;
        string FocusedControl = "";
        private OpeningVoucherDAL cClass;
        private string strSQL;
        private string CaptionNo;
        private string CaptionName;
        private string PrimaryName;
        private string AccountName;
        private string CaptionCredit;  // دائن
        private string CaptionDebit;   //مدين
        private string CaptionAccountID;
        private string CaptionAccountName;
        private string CaptionDeclaration;
        private string CaptionCostCenterID;
        private bool IsNewRecord;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        OpenFileDialog OpenFileDialog1 = null;
        public CultureInfo culture = new CultureInfo("en-US");
        DataTable dt = new DataTable();
        //all record master and detail
        BindingList<Acc_VariousVoucherDetails> AllRecords = new BindingList<Acc_VariousVoucherDetails>();

        //list detail
        BindingList<Acc_VariousVoucherDetails> lstDetail = new BindingList<Acc_VariousVoucherDetails>();

        //Detail
        Acc_VariousVoucherDetails BoDetail = new Acc_VariousVoucherDetails();

        #endregion
        public frmOpeningVoucher()
        {
            try
            {
               // SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                lblDifference.BackColor = Color.WhiteSmoke;
                lblDifference.ForeColor = Color.Black;
                AccountName = "ArbAccountName";
                PrimaryName = "ArbName";
                CaptionNo = "الرقم";
                CaptionName = "الاسم";
                CaptionDebit = "مدين";
                CaptionCredit = "دائن";
                CaptionAccountID = "رقم الحساب";
                CaptionAccountName = "اسم الحساب";
                CaptionDeclaration = "الـبـيـــــان";
                CaptionCostCenterID = "مركز تكلفة";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    AccountName = "EngAccountName";
                    PrimaryName = "EngName";
                    CaptionNo = "No";
                    CaptionName = "Name";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");
                    CaptionDebit = "Debit";
                    CaptionCredit = "Credit";
                    CaptionAccountID = "Account ID";
                    CaptionAccountName = "Account Name";
                    CaptionDeclaration = "Declaration";
                    CaptionCostCenterID = "Cost Center";
                }
                InitGrid();
                /*********************** Fill Data ComboBox  ****************************/
               
                /***********************Component ReadOnly  ****************************/
                TextEdit[] txtEdit = new TextEdit[2];
                txtEdit[0] = lblDebitAccountID;
                txtEdit[1] = lblCreditAccountName;
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
               // txtVoucherDate.ReadOnly = !MySession.GlobalAllowChangefrmOpeningVoucherDate;
                //_____ Read Only Account ID 
                lblCreditAccountID.ReadOnly = !MySession.GlobalAllowChangefrmOpeningVoucherCreditAccountID;
                lblDebitAccountID.ReadOnly = !MySession.GlobalAllowChangefrmOpeningVoucherDebitAccountID;
                txtVoucherDate.ReadOnly = !MySession.GlobalAllowChangefrmOpeningVoucherDate;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmOpeningVoucherCurencyID;
                /************ Button Search Account ID ***************/
                RolesButtonSearchAccountID();
                /********************* Event For TextEdit Component **************************/
                if (MySession.GlobalAllowWhenEnterOpenPopup)
                {
                    this.txtVoucherDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                   
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtVoucherDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                   
                }
                this.txtVoucherID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                //_____ Validating
                this.txtVoucherID.Validating += new System.ComponentModel.CancelEventHandler(this.txtVoucherID_Validating);
                /********************* Event For Account Component ****************************/

                this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);
                this.btnCreditSearch.Click += new System.EventHandler(this.btnCreditSearch_Click);

                //_____ Validating
                this.lblDebitAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblDebitAccountID_Validating);
                this.lblCreditAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.lblCreditAccountID_Validating);

                /***************************** Event For GridView *****************************/
                this.txtRegistrationNo.Validated += new System.EventHandler(this.txtRegistrationNo_Validated);
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmOpeningVoucher_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                GetAccountsDeclaration();
                strSQL = "ArbName";
                FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", " BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly=!MySession.GlobalAllowBranchModificationAllScreens;
                FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
                cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
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
            gridView1.Columns["CurrencyID"].Visible = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["CurrencyEquivalent"].OptionsColumn.AllowFocus = false;
          
            if(UserInfo.Language==iLanguage.Arabic)
            {
                gridView1.Columns["CurrencyEquivalent"].Caption = "المقابل";
                gridView1.Columns["CurrencyID"].Caption = "رقم العملة";
                gridView1.Columns["CurrencyName"].Caption = "العملة ";
                gridView1.Columns["CurrencyPrice"].Caption = "سعر العملة ";
            }
            else
            {
                gridView1.Columns["CurrencyEquivalent"].Caption = "Currency Equivalent";
                gridView1.Columns["CurrencyID"].Caption = "Currency ID";
                gridView1.Columns["CurrencyName"].Caption = "Currency Name";
                gridView1.Columns["CurrencyPrice"].Caption = "Currency Price";
            }
           
            /******************* Columns Visible=true ********************/
            gridView1.Columns[AccountName].Visible = true;
            /******************* Columns Visible=true *******************/

            gridView1.Columns["Credit"].Caption = CaptionCredit;
            gridView1.Columns["Debit"].Caption = CaptionDebit;
            gridView1.Columns["AccountID"].Caption = CaptionAccountID;
            gridView1.Columns[AccountName].Caption = CaptionAccountName;
            gridView1.Columns[AccountName].Width = 150;
            gridView1.Columns["Declaration"].Caption = CaptionDeclaration;
            gridView1.Columns["Declaration"].Width = 150;
            gridView1.Columns["CostCenterID"].Caption = CaptionCostCenterID;
            gridView1.Focus();
            /*************************Columns Properties ****************************/

            
            /************************ Look Up Edit **************************/

            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridView1.Columns[AccountName].ColumnEdit = rAccountName;
            gridControl.RepositoryItems.Add(rAccountName);




            strSQL = "SELECT " + "CostCenterID" + " AS [" + CaptionNo + "]," + PrimaryName + "  AS [" + CaptionName + "] FROM " + "Acc_CostCenters where Cancel=0 and BranchID=" + MySession.GlobalBranchID;

            RepositoryItemLookUpEdit rCostCenter = new RepositoryItemLookUpEdit();
            rCostCenter.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
            //gridView1.Columns["CostCenterID"].OptionsColumn.AllowEdit = MySession.GlobalAllowChangefrmOpeningVoucherCostCenterID;
            gridView1.Columns["CostCenterID"].ColumnEdit = rCostCenter;
            gridControl.RepositoryItems.Add(rCostCenter);
            rCostCenter.DataSource = Lip.SelectRecord(strSQL);
            rCostCenter.DisplayMember = CaptionName;
            rCostCenter.ValueMember = CaptionNo;
            rCostCenter.NullText = "";

           string strSQLCu = "SELECT   ID, " + PrimaryName + " FROM  [Acc_Currency] where [Cancel]=0 and BranchID="+MySession.GlobalBranchID;
            RepositoryItemLookUpEdit rCurrncyId = new RepositoryItemLookUpEdit();
            rCurrncyId.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;      
            gridView1.Columns["CurrencyName"].ColumnEdit = rCurrncyId;
            gridControl.RepositoryItems.Add(rCurrncyId);
            DataTable dt = Lip.SelectRecord(strSQLCu);
            rCurrncyId.DataSource = dt;
            rCurrncyId.DisplayMember = PrimaryName;
            rCurrncyId.ValueMember = "ID";
            rCurrncyId.NullText = "";

            //gridView1.Columns["CostCenterID"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmOpeningVoucherCostCenterID;
           
            gridView1.Columns["CostCenterID"].OptionsColumn.AllowFocus =  MySession.GlobalAllowChangefrmOpeningVoucherCostCenterID;
            gridView1.Columns["CostCenterID"].OptionsColumn.AllowEdit = MySession.GlobalAllowChangefrmOpeningVoucherCostCenterID; 
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
            else if (ColName == "Declaration")
            {

                if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsRequired;

                }
            }
            if (ColName == "AccountID" )
            {
                if (Comon.ConvertToDecimalPrice(e.Value.ToString()) <= 0)
                {
                    e.Valid = false;
                    HasColumnErrors = true;
                    e.ErrorText = Messages.msgInputIsGreaterThanZero;
                }

            }
            if (ColName == "Debit")

            {
                 if (Comon.cDec(e.Value) > 0)
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Credit"], 0);
                }
             

            }
            else if (ColName == "Credit")
            {
                
                if (Comon .cDec(e.Value )> 0)
                {
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Debit"], 0);
                }
            }
           
            if (ColName == "CurrencyName")
            {
                string StrSQL = "Select ID ,ExchangeRate from Acc_Currency Where Cancel=0 And ID=" + e.Value.ToString() + " and BranchID=" + MySession.GlobalBranchID;
                DataTable dt = Lip.SelectRecord(StrSQL);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyID", dt.Rows[0]["ID"]);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyPrice", dt.Rows[0]["ExchangeRate"]);
                if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debit")) > 0)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(dt.Rows[0]["ExchangeRate"]) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debit"))).ToString());
                else if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit")) > 0)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(dt.Rows[0]["ExchangeRate"]) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit"))).ToString());


            }
            if (ColName == "CurrencyPrice")
            {
                 if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debit")) > 0)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debit"))).ToString());
                else if (Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit")) > 0)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "CurrencyEquivalent", Comon.ConvertToDecimalPrice(Comon.cDec(e.Value) * Comon.cDec(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit"))).ToString());

            }
        }
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                HasColumnErrors = false;
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "AccountID"  || col.FieldName == "Credit" || col.FieldName == "Debit")
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
                    if (col.FieldName == "AccountID")
                    {
                        var cellValue = gridView1.GetRowCellValue(e.RowHandle, col);
                        if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(gridView1.Columns[col.FieldName], Messages.msgInputIsGreaterThanZero);
                        }

                    }
                    if (col.FieldName == "Credit" || col.FieldName == "Debit")
                    {
                        if (Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debit").ToString()) == Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit").ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            Messages.MsgWarning(Messages.TitleWorning, "لا يمكن ان يكون طرفي الحساب متساووين   ");
                        }
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
            gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["CostCenterID"], MySession.GlobalDefaultOpeningVoucherCostCenterID);
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

                    }
                    else if (ColName == "Declaration")
                    {
                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            HasColumnErrors = true;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsRequired);
                        }
                    }
                    if (ColName == "AccountID" || ColName == "CostCenterID")
                    {
                        if (cellValue != null && Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0)
                        {

                            HasColumnErrors = true;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);
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
                int code = ex.HResult;
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
                        if (col.FieldName == "AccountID")
                        {
                            var cellValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, col);
                            if (Comon.cDbl(cellValue.ToString()) <= 0)
                            {
                                gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
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
                    if (col.FieldName == "AccountID" )
                    {
                        var cellValue = gridView1.GetRowCellValue(i, col);
                        if (Comon.cDbl(cellValue.ToString()) <= 0)
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
        public void CalculatTotalBalance()
        {
            decimal CreditTotal = 0;
            decimal DebitRow = 0;
            decimal CreditRow = 0;
            decimal DebitTotal = 0;
            try
            {
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    CreditRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Credit").ToString());
                    DebitRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Debit").ToString());
                    
                    CreditTotal += CreditRow;
                    DebitTotal += DebitRow;
                }
                if (gridView1.FocusedRowHandle < 0)
                {
                    var ResultCredit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit");
                    var ResultDebit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debit");

                    CreditRow = ResultCredit != null ? Comon.ConvertToDecimalPrice(ResultCredit.ToString()) : 0;
                    DebitRow = ResultDebit != null ? Comon.ConvertToDecimalPrice(ResultDebit.ToString()) : 0;

                    CreditTotal += CreditRow;
                    DebitTotal += DebitRow;
                }

                lblTotalCredit.Text = CreditTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblTotalDebit.Text = DebitTotal.ToString("N" + MySession.GlobalNumDecimalPlaces);
                lblDifference.Text = (DebitTotal - CreditTotal).ToString("N" + MySession.GlobalNumDecimalPlaces);
                int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
                if (isLocalCurrncy > 1)
                {
                    decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
                    lblCurrencyEqv.Text = Comon.cDec(Comon.cDec(lblTotalCredit.Text) * Comon.cDec(txtCurrncyPrice.Text)) + "";
                }
                else
                {
                    txtCurrncyPrice.Text = "1";
                    lblCurrencyEqv.Visible = false;
                    label1.Visible = false;
                    labelControl4.Visible = false;
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

            if ((FocusedControl != txtVoucherID.Name) && (FocusedControl.Trim() != gridControl.Name))
                FocusedControl = txtVoucherID.Name;

            if (FocusedControl.Trim() == txtVoucherID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "VariousVoucherID", "رقم السـند", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "VariousVoucherID", "Voucher ID", MySession.GlobalBranchID);
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

                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colAccountID" || gridView1.FocusedColumn.Name == "colAccountName")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AccountID"], Barcode);
                        DataTable dt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                        DataRow[] row = dt.Select("AccountID=" + Barcode);
                        if (row.Length > 0)
                        {
                            FileItemData(row[0]);
                        }
                        gridView1.FocusedColumn = gridView1.VisibleColumns[3];
                        gridView1.ShowEditor();
                        CalculatTotalBalance();

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
                   dt = OpeningVoucherDAL.frmGetDataDetalByID(VoucherID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;

                        //Date

                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["VoucherDate"].ToString()) == "")
                            txtVoucherDate.Text = "";
                        else
                            txtVoucherDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["VoucherDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);

                 
                        txtCurrncyPrice.Text = dt.Rows[0]["CurrencyPrice"].ToString();
                        lblCurrencyEqv.Text = dt.Rows[0]["CurrencyEquivalent"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        //Masterdata
                        txtVoucherID.Text = dt.Rows[0]["VoucherID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["DocumentID"].ToString();
                        txtRegistrationNo.Text = dt.Rows[0]["RegistrationNo"].ToString();
                        cmbStatus.EditValue= Comon.cIntToBoolean(Comon.cInt(dt.Rows[0]["Posted"].ToString()));


                        //GridVeiw
                        gridControl.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        CalculatTotalBalance();
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
                
                lblCurrencyEqv.Text = ""; 
                txtDocumentID.Text = "";
                txtNotes.Text = "";
                txtVoucherDate.EditValue = DateTime.Now;


                lblTotalCredit.Text = "0";
                lblTotalDebit.Text = "0";
                lblDifference.Text = "0";

                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultOpeningVoucherCurencyID);
                

                GetAccountsDeclaration();

                

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
                    strSQL = "SELECT TOP 1 * FROM " + OpeningVoucherDAL.TableName + " Where Cancel =0 and TypeOpration=1 and BranchID=" + MySession.GlobalBranchID;
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + OpeningVoucherDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + OpeningVoucherDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + OpeningVoucherDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + OpeningVoucherDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + OpeningVoucherDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + OpeningVoucherDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new OpeningVoucherDAL();

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
                ClearFields();
                EnabledControl(true);
                IsNewRecord = true;


                txtVoucherID.Text = OpeningVoucherDAL.GetNewID(1).ToString();

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

        public   void Last()
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


            dtItem.Columns.Add("CurrencyID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("CurrencyName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("CurrencyPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CurrencyEquivalent", System.Type.GetType("System.Decimal"));
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["BranchID"] = MySession.GlobalBranchID;
                dtItem.Rows[i]["FacilityID"] = MySession.GlobalFacilityID;
                dtItem.Rows[i]["AccountID"] = gridView1.GetRowCellValue(i, "AccountID").ToString();
                dtItem.Rows[i][AccountName] = gridView1.GetRowCellValue(i, AccountName).ToString();
                dtItem.Rows[i]["Debit"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Debit").ToString());
                dtItem.Rows[i]["CostCenterID"] = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                dtItem.Rows[i]["Credit"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Credit").ToString());
                dtItem.Rows[i]["Declaration"] = gridView1.GetRowCellValue(i, "Declaration").ToString();


                dtItem.Rows[i]["CurrencyID"] = Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString());
                dtItem.Rows[i]["CurrencyName"] = gridView1.GetRowCellValue(i, "CurrencyName").ToString();
                dtItem.Rows[i]["CurrencyPrice"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                dtItem.Rows[i]["CurrencyEquivalent"] =Comon.ConvertToDecimalPrice( gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString());

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);

        }

        public void DoSaveFromFinance()
        {
            DoSave();
        }

        private void Save()
        {
            gridView1.MoveLastVisible();
            CalculatTotalBalance();
            txtVoucherDate_EditValueChanged(null, null);
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];

            //strSQL = ("SELECT TOP 1 VoucherID FROM Acc_VariousVoucherMaster WHERE (VoucherID = 0) And BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
            //DataTable dt = Lip.SelectRecord(strSQL);
            //if ((dt.Rows.Count > 0))
            //    IsNewRecord = false;
            //else
            //    IsNewRecord = true;

            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

            objRecord.VoucherID =Comon.cInt(txtVoucherID.Text);

            objRecord.BranchID =Comon.cInt( cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;

            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString().Trim();

            objRecord.CurrencyID =Comon.cInt(cmbCurency.EditValue);

            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);

            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Good Opening Voucher" : "سند قيد افتتاحي");
            objRecord.Notes = txtNotes.Text;
            objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
            objRecord.DelegateID = 0;
            objRecord.CanUpdate = 0;
            objRecord.IsExpens = 0;
            objRecord.DocumentType = 0;

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
            objRecord.TypeOpration = 1;

            if (IsNewRecord == false)
            {

                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice =Comon.cDec( txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);
            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Acc_VariousVoucherDetails();
                returned.ID = i;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                returned.VoucherID =Comon.cInt(txtVoucherID.Text);
                returned.Credit = Comon.cDbl(gridView1.GetRowCellValue(i, "Credit").ToString());
                returned.Debit = Comon.cDbl(gridView1.GetRowCellValue(i, "Debit").ToString());
                returned.Declaration = gridView1.GetRowCellValue(i, "Declaration").ToString();
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
                int Result = OpeningVoucherDAL.InsertUsingXML(objRecord, MySession.UserID, IsNewRecord);
                if (Comon.cInt(Result) > 0&&Comon.cInt(cmbStatus.EditValue)>1)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result));
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + VariousVoucherDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + VariousVoucherDAL.PremaryKey + " = " + txtVoucherID.Text + " AND BranchID=" + Comon.cInt(cmbBranchesID.EditValue));

                }
                SplashScreenManager.CloseForm(false);

                txtVoucherID_Validating(null, null);
                EnabledControl(false);

                if (IsNewRecord == true)
                {
                    if (Result> 0)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }

                }
                else
                {

                    if (Result >= 0)
                    {
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
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString();
          
            objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            objRecord.DelegateID = 0;
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);

            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblCurrencyEqv.Text);

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
            //Credit
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Acc_VariousVoucherMachinDetails();
                returned.ID = 1;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.FacilityID = UserInfo.FacilityID;
                returned.AccountID = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                returned.VoucherID = VoucherID;
                returned.Credit = Comon.cDbl(gridView1.GetRowCellValue(i, "Credit").ToString());
                returned.Debit = Comon.cDbl(gridView1.GetRowCellValue(i, "Debit").ToString());
                returned.Declaration = gridView1.GetRowCellValue(i, "Declaration").ToString();
                returned.CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                returned.CurrencyID = Comon.cInt(gridView1.GetRowCellValue(i, "CurrencyID").ToString());
                returned.CurrencyPrice = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyPrice").ToString());
                returned.CurrencyEquivalent = Comon.cDbl(gridView1.GetRowCellValue(i, "CurrencyEquivalent").ToString()); 
                listreturned.Add(returned);
            }


            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        protected override void DoSave()
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;
                //if (!Validations.IsValidFormCmb(cmbCurency))
                //    return;
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
                if (!Lip.CheckTheProcessesIsPosted("Acc_VariousVoucherMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtVoucherID.Text), PrimeryColName: "VoucherID", Where: " and  TypeOpration=1" ))
                {
                    Messages.MsgWarning(Messages.TitleError, Messages.msgTheProcessIsNotUpdateBecuseIsPosted);
                    return;
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
                model.TypeOpration = 1; 
                bool Result = OpeningVoucherDAL.DeleteAcc_VariousVoucherMaster(model);
                if (Comon.cInt(Result) >= 0)
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
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
                ReportName = "rptOpeningVoucher";
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
                rptForm.Parameters["Notes"].Value = txtNotes.Text.Trim().ToString();
                rptForm.Parameters["RegistrationNo"].Value = txtRegistrationNo.Text.Trim().ToString();

                /********Total*********/
                rptForm.Parameters["TotalCredit"].Value = lblTotalCredit.Text.Trim().ToString();
                rptForm.Parameters["TotalDebit"].Value = lblTotalDebit.Text.Trim().ToString();
                rptForm.Parameters["Difference"].Value = lblDifference.Text.Trim().ToString();
                rptForm.Parameters["ReportName"].Value = this.Text;
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptOpeningVoucherDataTable();

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
                    if (dt.Rows.Count > 0) for (int i = 1; i < 6; i++)
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
            string Release = "سند قيد الافتتاحي رقم";
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
                Record.BranchNum = Comon.cInt(cmbBranchesID.EditValue);
                Record.FacilityID = UserInfo.FacilityID;
                Record.TranNo = VoucherID;
                Record.TransType = 4;
                Record.RegistrationDate = Comon.cDbl(Comon.ConvertDateToSerial(txtVoucherDate.Text));
                Record.Acc_code = Comon.cDbl(gridView1.GetRowCellValue(i, "AccountID").ToString());
                Record.Master_code = 0;
                Record.Debt = Comon.cDbl(gridView1.GetRowCellValue(i, "Debit").ToString());
                Record.Credit = Comon.cDbl(gridView1.GetRowCellValue(i, "Credit").ToString());
                Record.Discount = 0;
                if (gridView1.GetRowCellValue(i, "Declaration").ToString() != "")
                    Record.Release = gridView1.GetRowCellValue(i, "Declaration").ToString();
                else
                    Record.Release = Release + VoucherID;
                Record.AccountFinal = 0;
                Record.CurrencyNum = 1;
                Record.SellerNum = 0;
                Record.DelegateNum = 0;
                Record.DocumentNumber = txtDocumentID.Text;
                Record.OperationType = Release;
                Record.Remark = txtNotes.Text.Trim();
                Record.AccountNumCorresponding = "0";
                Record.Receivables = "";
                CostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "CostCenterID").ToString());
                Record.CostCenterNo = CostCenterID;
                Record.posted = Comon.cInt(cmbStatus.EditValue);
                Record.Cancel = 0;
                listRecord.Add(Record);

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
        private void frmOpeningVoucher_Load(object sender, EventArgs e)
        {

            //CreateGoodsOpeningBalanceIfNotCreated();
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Item.Enabled = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Item.Enabled = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Item.Enabled = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Item.Enabled = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Enabled = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Item.Enabled = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Item.Enabled = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Item.Enabled = false;

            //ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;

            DoNew();
            txtVoucherID.Focus();
           
            txtVoucherID_Validating(null, null);
            gridView1.Focus();

          
            //DoFirst();

            ////if (UserInfo.ID == 1 || UserInfo.ID == 2)
            //{
            //    cmbBranchesID.Visible = true;
            //    labelControl1.Visible = true;
            //}
            //else
            //{
            //    labelControl1.Visible = false;
            //    cmbBranchesID.Visible = false;
            //}

        }
        private void frmOpeningVoucher_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3)
                Find();
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
        #endregion
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
        private void lblDebitAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblDebitAccountID.Text + ") ";
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
                strSQL = "SELECT ArbName AS AccountName FROM Acc_Accounts WHERE (BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + " ) AND " + " (Cancel = 0) AND (AccountID = " + lblCreditAccountID.Text + ") ";
                CSearch.ControlValidating(lblCreditAccountID, lblCreditAccountName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #endregion
        #endregion
        #region InitializeComponent
        private void GetAccountsDeclaration()
        {
            //try
            //{

            //    dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(MySession.GlobalBranchID, MySession.GlobalFacilityID);
            //    //حساب راس المال
            //    DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'Capital'");
            //    if ((row.Length > 0))
            //    {
            //        lblCreditAccountID.Text = row[0]["AccountID"].ToString();
            //        lblCreditAccountName.Text = row[0]["AccountName"].ToString();
            //    }

            //    //حساب اول المدة
            //    DataRow[] row2 = dtDeclaration.Select("DeclareAccountName = 'GoodsOpening'");
            //    if ((row2.Length > 0))
            //    {
            //        lblDebitAccountID.Text = row2[0]["AccountID"].ToString();
            //        lblDebitAccountName.Text = row2[0]["AccountName"].ToString();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    SplashScreenManager.CloseForm(false);
            //    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            //}

        }
        private void CreateGoodsOpeningBalanceIfNotCreated()
        {
            try
            {

                string OpenBalance = "0";
                DataTable dtTest;
                // JBHE G0' 'D371 ('D*#C/ EF H,H/ -3'( (6'9) 'HD 'DE/) /'.D 3F/ 'DBJ/ 'D'A**'-J  A%F DE JCF EH,H/' A%FG J*E %F4'$G *DB'&J' EF -3'( (6'9) #HD 'DE/) %DI -3'( 1#3 'DE'D
                strSQL = ("Select Top 1 * From Acc_VariousVoucherDetails Where (VoucherID = 0) AND BranchID =" + MySession.GlobalBranchID);
                dtTest = Lip.SelectRecord(strSQL);
                if ((dtTest.Rows.Count == 0))
                {
                    strSQL = ("Select Top 1 * From Acc_VariousVoucherMaster Where (VoucherID = 0) AND BranchID =" + MySession.GlobalBranchID);
                    dtTest = Lip.SelectRecord(strSQL);
                    if ((dtTest.Rows.Count == 0))
                    {

                        Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

                        objRecord.VoucherID = 0;

                        objRecord.BranchID = MySession.GlobalBranchID;
                        objRecord.FacilityID = MySession.GlobalFacilityID;


                        //Date
                        objRecord.VoucherDate = Comon.ConvertDateToSerial(txtVoucherDate.Text).ToString().Trim();

                        objRecord.CurrencyID = 1;
                        txtRegistrationNo.Text = RestrictionsDailyDAL.GetNewID(this.Name).ToString();
                        objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);

                        objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Good Opening Voucher" : "سند قيد افتتاحي");
                        txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Good Opening Voucher" : "سند قيد افتتاحي"));
                        objRecord.Notes = txtNotes.Text;
                        objRecord.DocumentID = Comon.cInt(txtDocumentID.Text);
                        objRecord.DelegateID = 0;
                        objRecord.CanUpdate = 0;
                        objRecord.IsExpens = 0;
                        objRecord.DocumentType = 0;


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

                        string Str = ("SELECT SUM(QTY * CostPrice)AS Total FROM  Sales_PurchaseInvoiceDetails" + (" WHERE     (InvoiceID = 0) AND (Cancel = 0) AND BranchID =" + MySession.GlobalBranchID));
                        DataTable dtSUM = Lip.SelectRecord(Str);
                        OpenBalance = (DBNull.Value == (dtSUM.Rows[0][0]) ? "0" : dtSUM.Rows[0][0].ToString());

                        Acc_VariousVoucherDetails returned;
                        List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();

                        returned = new Acc_VariousVoucherDetails();
                        returned.ID = 1;
                        returned.FacilityID = UserInfo.FacilityID;
                        returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                        returned.AccountID = Comon.cDbl(lblDebitAccountID.Text);
                        returned.VoucherID = 0;
                        returned.Credit = 0;
                        returned.Debit = Comon.cDbl(OpenBalance);
                        returned.Declaration = (UserInfo.Language == iLanguage.Arabic ? "بضاعة أول المدة" : "First Good Period");
                        returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                        listreturned.Add(returned);

                        returned = new Acc_VariousVoucherDetails();
                        returned.ID = 2;
                        returned.FacilityID = UserInfo.FacilityID;
                        returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                        returned.AccountID = Comon.cDbl(lblCreditAccountID.Text);
                        returned.VoucherID = 0;
                        returned.Credit = Comon.cDbl(OpenBalance);
                        returned.Debit = 0;
                        returned.Declaration = (UserInfo.Language == iLanguage.Arabic ? "بضاعة أول المدة" : "First Good Period");
                        returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                        listreturned.Add(returned);

                        objRecord.VariousVoucherDetails = listreturned;
                        int Result = OpeningVoucherDAL.InsertUsingXML(objRecord, MySession.UserID, true);

                        if (Result ==  0 )
                        {
                            Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                            txtVoucherID.Text = "0";
                            txtVoucherID_Validating(null, null);
                            EnabledControl(false);
                        }
                        else if (Result == 1)
                        {
                            txtVoucherID.Text = "0";
                            txtVoucherID_Validating(null, null);
                            EnabledControl(false);
                        }
                        else
                        {
                            Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                        }
                     


                    }

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

                if (col.FieldName == "Credit" || col.FieldName == "AccountID" || col.FieldName == AccountName || col.FieldName == "Declaration" || col.FieldName == "Debit"  )
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

            btnDebitSearch.Enabled = MySession.GlobalAllowChangefrmOpeningVoucherDebitAccountID;
            btnCreditSearch.Enabled = MySession.GlobalAllowChangefrmOpeningVoucherCreditAccountID;
        }
        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;

            Obj.EditValue = DateTime.Now;


        }
        #endregion

        private void btnPrintRestrictonDaily1_Click(object sender, EventArgs e)
        {
            if (txtRegistrationNo.Text == "")
            {
                Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgEnterRegistrationNo);
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

        private void btnMachinResraction_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtVoucherID.Text + " And DocumentType=" + DocumentType).ToString());
            if (ID !=0)
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

            strSQL = "Select * from " + OpeningVoucherDAL.TableName + " where Cancel=0 ";
            DataTable dtSend = new DataTable();
            dtSend = Lip.SelectRecord(strSQL);
            if (dtSend.Rows.Count > 0)
            {
                for (int i = 0; i <= dtSend.Rows.Count - 1; i++)
                {
                    txtVoucherID.Text = dtSend.Rows[i]["VoucherID"].ToString();
                    cmbBranchesID.EditValue = Comon.cInt(dtSend.Rows[i]["BranchID"].ToString());

                    //txtCostCenterID.Text = dtSend.Rows[i]["CostCenterID"].ToString();
                    //txtStoreID.Text = dtSend.Rows[i]["StoreID"].ToString();
                    txtVoucherID_Validating(null, null);
                    IsNewRecord = true;
                    if (Comon.cInt(txtVoucherID.Text) == 0)
                    {
                        //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(0);
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        else
                            Lip.ExecututeSQL("Update " + VariousVoucherDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + VariousVoucherDAL.PremaryKey + " = " + txtVoucherID.Text + " AND BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
                    }
                }

                this.Close();
            }
        }

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
                txtCurrncyPrice.Text = CurrncyPrice + "";
                lblCurrencyEqv.Visible = true;
                label1.Visible = true;
                labelControl4.Visible = true;
                txtCurrncyPrice.Visible = true;
            }
            else
            {
                txtCurrncyPrice.Text = "1";
                lblCurrencyEqv.Visible = false;
                label1.Visible = false;
                labelControl4.Visible = false;
                txtCurrncyPrice.Visible = false;
            }

        }

        private void lblVoucherName_Click(object sender, EventArgs e)
        {

        }
    }

}

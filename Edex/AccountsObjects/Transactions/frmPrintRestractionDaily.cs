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
using System.Text;
using System.Windows.Forms;

namespace Edex.AccountsObjects.Transactions
{
    public partial class frmPrintRestractionDaily : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        DataTable dtDeclaration;
        string FocusedControl = "";
        private RestrictionsDailyDAL cClass;
        private string strSQL;
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
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;

        DataTable dt = new DataTable();
        //all record master and detail
        BindingList<RestrictionsDaily> AllRecords = new BindingList<RestrictionsDaily>();

        //list detail
        BindingList<RestrictionsDaily> lstDetail = new BindingList<RestrictionsDaily>();

        //Detail
        RestrictionsDaily BoDetail = new RestrictionsDaily();

        #endregion
        public frmPrintRestractionDaily()
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
                FillCombo.FillComboBoxLookUpEdit(cmbCurency, "Currency", "CurrencyID", PrimaryName, "", " BranchID = " + UserInfo.BRANCHID);
                /***********************Component ReadOnly  ****************************/
                TextEdit[] txtEdit = new TextEdit[1];
                txtEdit[0] = lblOperationTypeName;

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
                txtVoucherDate.ReadOnly = !MySession.GlobalAllowChangefrmVariousVoucherDate;
                cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmVariousVoucherCurencyID;
                txtOperationTypeID.ReadOnly = !MySession.GlobalAllowChangefrmVariousVoucherSalesDelegateID;

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

                this.txtOperationTypeID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtDocumentID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtVoucherID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                //_____ Validating
                this.txtVoucherID.Validating += new System.ComponentModel.CancelEventHandler(this.txtVoucherID_Validating);


                /***************************** Event For GridView *****************************/
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);

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
            lstDetail = new BindingList<RestrictionsDaily>();
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


            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["RegistrationNo"].Visible = false;
            gridView1.Columns["TranNo"].Visible = false;
            gridView1.Columns["TransType"].Visible = false;
            gridView1.Columns["BranchNum"].Visible = false;
            gridView1.Columns["RegistrationDate"].Visible = false;
            gridView1.Columns["Master_code"].Visible = false;
            gridView1.Columns["Discount"].Visible = false;
            gridView1.Columns["AccountFinal"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["CurrencyNum"].Visible = false;
            gridView1.Columns["SellerNum"].Visible = false;
            gridView1.Columns["DelegateNum"].Visible = false;
            gridView1.Columns["TransType"].Visible = false;
            gridView1.Columns["DocumentNumber"].Visible = false;
            gridView1.Columns["OperationType"].Visible = false;
            gridView1.Columns["Remark"].Visible = false;
            gridView1.Columns["AccountNumCorresponding"].Visible = false;
            gridView1.Columns["Receivables"].Visible = false;

            gridView1.Columns["posted"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["TransType"].Visible = false;
            gridView1.Columns["ArbAccountName"].Visible = false;
            gridView1.Columns["EngAccountName"].Visible = false;
            /******************* Columns Visible=true ********************/
            gridView1.Columns["Credit"].Visible = true;
            gridView1.Columns["Debt"].Visible = true;
            gridView1.Columns[AccountName].Visible = true;
            gridView1.Columns["Acc_code"].Visible = true;
            gridView1.Columns["Release"].Visible = true;
            gridView1.Columns["CostCenterNo"].Visible = true;
            /******************* Columns Visible=true *******************/

            gridView1.Columns["Credit"].Caption = CaptionCredit;
            gridView1.Columns["Debt"].Caption = CaptionDebitAmount;
            gridView1.Columns["Acc_code"].Caption = CaptionAccountID;
            gridView1.Columns[AccountName].Caption = CaptionAccountName;
            gridView1.Columns[AccountName].Width = 150;
            gridView1.Columns["Release"].Caption = CaptionDeclaration;
            gridView1.Columns["Release"].Width = 150;
            gridView1.Columns["CostCenterNo"].Caption = CaptionCostCenterID;
            gridView1.Focus();
            /*************************Columns Properties ****************************/

            gridView1.Columns[AccountName].OptionsColumn.ReadOnly = true;
            gridView1.Columns[AccountName].OptionsColumn.AllowFocus = false;

            gridView1.Columns["CostCenterNo"].OptionsColumn.ReadOnly = !MySession.GlobalAllowChangefrmVariousVoucherCostCenterID;
            gridView1.Columns["CostCenterNo"].OptionsColumn.AllowFocus = MySession.GlobalAllowChangefrmVariousVoucherCostCenterID;

            /************************ Look Up Edit **************************/
            RepositoryItemLookUpEdit rAccountName = Common.LookUpEditAccountName();
            gridView1.Columns[AccountName].ColumnEdit = rAccountName;
            gridControl.RepositoryItems.Add(rAccountName);

            RepositoryItemLookUpEdit rCostCenter = new RepositoryItemLookUpEdit();
            gridView1.Columns["CostCenterNo"].OptionsColumn.AllowEdit = MySession.GlobalAllowChangefrmVariousVoucherCostCenterID;
            gridView1.Columns["CostCenterNo"].ColumnEdit = rCostCenter;
            gridControl.RepositoryItems.Add(rCostCenter);
            FillCombo.FillComboBoxRepositoryItemLookUpEdit(rCostCenter, "Acc_CostCenters", "CostCenterID", PrimaryName);

        }
        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {

            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = (e.ListSourceRowIndex + 1);
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
                    DebitAmountRow = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "Debt").ToString());

                    CreditTotal += CreditRow;
                    DebitTotal += DebitAmountRow;
                }
                if (gridView1.FocusedRowHandle < 0)
                {
                    var ResultCredit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Credit");
                    var ResultDebitAmount = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Debt");

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
        private void EnabledControl(bool Value)
        {


            foreach (GridColumn col in gridView1.Columns)
            {

                if (col.FieldName == "Debt" || col.FieldName == "Acc_code" || col.FieldName == "Release" || col.FieldName == "Credit" || col.FieldName == "CostCenterNo")
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }
            }


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
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "VariousVoucherID", "رقم السـند", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtVoucherID, lblVoucherName, "VariousVoucherID", "Voucher ID", MySession.GlobalBranchID);
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


            }

        }
        public void ReadRecord(long VoucherID)
        {
            try
            {

                ClearFields();
                {
                    dt = RestrictionsDailyDAL.frmGetDataDetalByID(VoucherID, UserInfo.BRANCHID, UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        //Masterdata
                        txtVoucherID.Text = dt.Rows[0]["VoucherID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtDocumentID.Text = dt.Rows[0]["TranNo"].ToString();
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());

                        //Validate
                        txtOperationTypeID.Text = dt.Rows[0]["TransType"].ToString();
                        lblOperationTypeName.Text = dt.Rows[0]["OperationType"].ToString();
                        //Date
                        txtVoucherDate.EditValue = Comon.ConvertSerialDateTo(dt.Rows[0]["VoucherDate"].ToString());
                        txtVoucherDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["VoucherDate"].ToString());

                        //Ammount

                        //GridVeiw
                        gridControl.DataSource = dt;

                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;

                        ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtVoucherID.Text;
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
                txtOperationTypeID.Text = "";
                lblOperationTypeName.Text = "";
                txtNotes.Text = "";

                txtVoucherDate.EditValue = DateTime.Now;

                txtNotes.Text = "";


                lblTotalCredit.Text = "0";
                lblTotalDebit.Text = "0";
                lblDifference.Text = "0";


                cmbCurency.EditValue = MySession.GlobalDefaultVariousVoucherCurencyID;

                lstDetail = new BindingList<RestrictionsDaily>();

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
                    strSQL = "SELECT TOP 1 * FROM " + RestrictionsDailyDAL.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + RestrictionsDailyDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + RestrictionsDailyDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + RestrictionsDailyDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + RestrictionsDailyDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + RestrictionsDailyDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + RestrictionsDailyDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new RestrictionsDailyDAL();

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
                    SplashScreenManager.CloseForm(false);
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
                ReportName = "rptRestrictionsDaily";
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
                rptForm.Parameters["OperationTypeName"].Value = lblOperationTypeName.Text.Trim().ToString();
                rptForm.Parameters["Notes"].Value = txtNotes.Text.Trim();
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
                    row["Debit"] = gridView1.GetRowCellValue(i, "Debt").ToString();
                    row["AccountID"] = gridView1.GetRowCellValue(i, "Acc_code").ToString();
                    row["AccountName"] = gridView1.GetRowCellValue(i, AccountName).ToString();
                    row["Declaration"] = gridView1.GetRowCellValue(i, "Release").ToString();
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
        /************************Event From **************************/
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
        public void txtVoucherID_Validating(object sender, CancelEventArgs e)
        {
            if (true)
                ReadRecord(Comon.cLong(txtVoucherID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
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

        private void frmPrintRestractionDaily_Load(object sender, EventArgs e)
        {
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = true;
            EnabledControl(false);
            txtVoucherID_Validating(null, null);
        }
    }
}

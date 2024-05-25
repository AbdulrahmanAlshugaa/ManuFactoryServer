using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
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
using System.Windows.Forms;

namespace Edex.AccountsObjects.Codes
{
    public partial class frmCostCenter : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        #region Declare
        DataTable dtDeclaration;
        int rowIndex;
        string FocusedControl = "";
        private string strSQL;
        private string PrimaryName;
        private string CostCenterBranchNo;
        private string CostCenterBranchName;
        private string CaptionNo;
        private string CaptionName;

        private bool IsNewRecord;
        private CostCentersDAL cClass;
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
        BindingList<Acc_CostCentersDetails> AllRecords = new BindingList<Acc_CostCentersDetails>();
        //list detail
        BindingList<Acc_CostCentersDetails> lstDetail = new BindingList<Acc_CostCentersDetails>();
        //Detail
        Acc_CostCentersDetails BoDetail = new Acc_CostCentersDetails();

        #endregion
        public frmCostCenter()
        {
            try
            {
                InitializeComponent();
                ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
                // ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                CostCenterBranchNo = "SubCostCenterID";
                CostCenterBranchName = "ArbCostCenterBranchName";
                CaptionNo = "رقم";
                CaptionName = "اسم مركز التكلفة الفرعي";

                PrimaryName = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {

                    CostCenterBranchName = "EngCostCenterBranchName";
                    PrimaryName = "EngName";
                    CaptionNo = " Branch ID";
                    CaptionName = "Cost Center Branch Name";

                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");

                }
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
                this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
                this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);


                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPurchaseInvoice_KeyDown);
                this.gridControl1.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);

                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);

                InitGrid();
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
            lstDetail = new BindingList<Acc_CostCentersDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl1.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/


            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["CostCenterID"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["SubCostCenterID"].Visible = true;
            gridView1.Columns["SubCostCenterID"].Width = 30;
            gridView1.Columns["ArbCostCenterBranchName"].Visible = false;
            gridView1.Columns["EngCostCenterBranchName"].Visible = false;

            /******************* Columns Visible=true *******************/
            gridView1.Columns["SubCostCenterID"].Caption = CaptionNo;
            gridView1.Columns[CostCenterBranchName].Visible = true;
            gridView1.Columns[CostCenterBranchName].Caption = CaptionName;

            gridView1.Focus();

            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView1.BestFitColumns();


        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (!gridView1.IsLastVisibleRow)
                    gridView1.MoveLast();

                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == CostCenterBranchNo || col.FieldName == CostCenterBranchName)
                    {

                        var val = gridView1.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
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
        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.gridView1.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "SubCostCenterID" || ColName == CostCenterBranchName)
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;

                    }
                    else 
                    {
                        Acc_CostCenters Obj = new Acc_CostCenters();
                        Obj = CostCentersDAL.GetDataByID(Comon.cInt(val), UserInfo.BRANCHID, UserInfo.FacilityID);
                        if (Obj != null)
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[CostCenterBranchName], Obj.ArbName);
                        else
                        {
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[CostCenterBranchName], null);
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgInputIsRequired;
                        }
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
                else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {
                    if (view.ActiveEditor is TextEdit)
                    {
                        double num;
                        HasColumnErrors = false;
                        var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                        string ColName = view.FocusedColumn.FieldName;
                        if (ColName == CostCenterBranchNo || ColName == CostCenterBranchName)
                        {

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsRequired);

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

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void FileItemData(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[CostCenterBranchNo], dt.Rows[0][CostCenterBranchNo].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[CostCenterBranchName], dt.Rows[0][CostCenterBranchName].ToString());
            }
            else
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[CostCenterBranchNo], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[CostCenterBranchName], " ");

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
                if (col.FieldName == CostCenterBranchNo || col.FieldName == CostCenterBranchName)
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }
            }

        }
        bool IsValidGrid()
        {
            double num;
            if (Convert.ToBoolean(tgsHasBranchID.EditValue) == true)
            {
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
                        if (col.FieldName == CostCenterBranchNo || col.FieldName == CostCenterBranchName)
                        {

                            var cellValue = gridView1.GetRowCellValue(i, col); ;

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
            return true;
        }
        #endregion
        #region Function
        #region Other Function
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;


            if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmPurchaseCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblArbName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblEngName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }

            GetSelectedSearchValue(cls);
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }

            }

        }
        public void ReadRecord(long CostCenterID)
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    dt = CostCentersDAL.frmGetDataDetalByID(CostCenterID, UserInfo.BRANCHID, UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {


                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();

                        tgsHasBranchID.EditValue = Comon.cInt(dt.Rows[0]["ContainsSubCenters"].ToString()) == 1 ? true : false;

                        //Masterdata
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        txtArbName.Text = dt.Rows[0]["ArbName"].ToString();
                        txtEngName.Text = dt.Rows[0]["EngName"].ToString();
                        chkStopeCostCenter.Checked = Comon.cbool(dt.Rows[0]["IsStope"].ToString());

                        gridControl1.DataSource = dt;

                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
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

                txtArbName.Text = "";
                txtEngName.Text = "";
                tgsHasBranchID.EditValue = 0;
                txtNotes.Text = "";

                lstDetail = new BindingList<Acc_CostCentersDetails>();

                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl1.DataSource = lstDetail;

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
                    strSQL = "SELECT TOP 1 * FROM " + CostCentersDAL.TableName + " Where IsStope=1  or  Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + CostCentersDAL.PremaryKey + " ASC ";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + CostCentersDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + CostCentersDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + CostCentersDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + CostCentersDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + CostCentersDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new CostCentersDAL();

                    long InvoicIDTemp = Comon.cLong(txtCostCenterID.Text);
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
                txtCostCenterID.Text = CostCentersDAL.GetNewID().ToString();

                ClearFields();
                EnabledControl(true);

                gridView1.Focus();
                gridView1.MoveLast();
                gridView1.FocusedColumn = gridView1.VisibleColumns[2];
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
                MoveRec(Comon.cInt(txtCostCenterID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtCostCenterID.Text), xMovePrev);
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
                txtCostCenterID.Enabled = true;
                txtCostCenterID.Focus();
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

           
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
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

            int CostCenterID = Comon.cInt(txtCostCenterID.Text);
            Acc_CostCenters objRecord = new Acc_CostCenters();

            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;

            objRecord.CostCenterID = CostCenterID;
            objRecord.ArbName = txtArbName.Text;
            objRecord.EngName = txtEngName.Text;
            objRecord.Notes = txtNotes.Text;
            objRecord.ContainsSubCenters = Convert.ToBoolean(tgsHasBranchID.EditValue) == true ? 1 : 0;
            objRecord.IsStope = (chkStopeCostCenter.Checked) ? 1 : 0;
            objRecord.Cancel = (chkStopeCostCenter.Checked) ? 1 : 0; 
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

            Acc_CostCentersDetails returned;
            List<Acc_CostCentersDetails> listreturned = new List<Acc_CostCentersDetails>();

            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {

                if (gridView1.GetRowCellValue(i, CostCenterBranchName) == null)
                    continue;

                returned = new Acc_CostCentersDetails();
                returned.ID = i;
                returned.BranchID = UserInfo.BRANCHID;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                returned.ArbCostCenterBranchName = gridView1.GetRowCellValue(i, CostCenterBranchName).ToString();
                returned.EngCostCenterBranchName = gridView1.GetRowCellValue(i, CostCenterBranchName).ToString();
                returned.SubCostCenterID = Comon.cInt(gridView1.GetRowCellValue(i, "SubCostCenterID").ToString());

                if (returned.CostCenterID <= 0)
                    continue;
                listreturned.Add(returned);

            }
            if (!Comon.cbool(objRecord.ContainsSubCenters))
            {
                objRecord.CostCentersDetails = listreturned;
                int Result = CostCentersDAL.InsertUsingXML(objRecord, IsNewRecord);
               
                SplashScreenManager.CloseForm(false);

                if (IsNewRecord == true)
                {
                    if (Result >= 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                        DoNew();
                    }
                    else if (Result == 0)
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave);

                    }

                }
                else
                {


                    if (Result >= 1)
                    {
                       
                        EnabledControl(false);
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        txtCostCenterID_Validating(null, null);
                    }
                    else if (Result == 0)
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave);

                    }
                }
            }
            else if (listreturned.Count > 0)
            {
                objRecord.CostCentersDetails = listreturned;
                int Result = CostCentersDAL.InsertUsingXML(objRecord, IsNewRecord);
                MessageBox.Show(Result + "");
                SplashScreenManager.CloseForm(false);

                if (IsNewRecord == true)
                {
                    if (Result >= 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                        DoNew();
                    }
                    else if (Result == 0)
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave);

                    }

                }
                else
                {


                    if (Result >= 1)
                    {
                        txtCostCenterID_Validating(null, null);
                        EnabledControl(false);
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                    }
                    else if (Result == 0)
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave);

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
                int TempID = Comon.cInt(txtCostCenterID.Text);

                Acc_CostCenters model = new Acc_CostCenters();
                model.CostCenterID = Comon.cInt(txtCostCenterID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                 
               
                int Result = CostCentersDAL.DeleteAcc_CostCenters(model);
                SplashScreenManager.CloseForm(false);
                if (Result > 0)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                else if (Result == 0)
                    Messages.MsgInfo(Messages.TitleError, Messages.msgErrorSave);
                MoveRec(model.CostCenterID, xMovePrev);


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

        #endregion
        #endregion
        #region Event

        #region Validating
        public void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cLong(txtCostCenterID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }

        }

        #endregion

        /************************Event From **************************/
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void txtArbName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;
            if (UserInfo.Language == iLanguage.Arabic)
                txtEngName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
        private void txtEngName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;

            if (UserInfo.Language == iLanguage.English)
                txtArbName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
        private void frmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
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

        private void frmCostCenter_Load(object sender, EventArgs e)
        {
            DoNew();
        }

        #endregion

        private void gridControl_Click(object sender, EventArgs e)
        {

        }



    }
}

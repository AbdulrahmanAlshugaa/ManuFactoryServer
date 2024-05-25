


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
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model.Language;
using Edex.ModelSystem;
using Edex.Model;
using Edex.StockObjects.StoresClasses;
using Edex.DAL;
using Edex.HR.HRClasses;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;

namespace Edex.HR.Codes
{
    public partial class frmAllowancesAndDeductions : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        /**************** Declare ************************/
        #region Declare
        private cAllowancesAndDeductions cClass = new cAllowancesAndDeductions();

        DataTable dtDeclaration = new DataTable();
        DataTable dtAllowance = new DataTable();
         

    List<SqlExecute> DB = new List<SqlExecute>();
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool IsFromanotherForms = false;
        private string strSQL;
        private bool IsNewRecord;
        private string where = "";
        private string lang = "";
        private string FocusedControl = "";
        private string PrimaryName = "ArbName";
        int rowIndex;
        BindingList<MonthlyAllowance> lstDetailAllowance = new BindingList<MonthlyAllowance>();
        BindingList<MonthlyDeduction> lstDetailDeduction = new BindingList<MonthlyDeduction>();
        #endregion
        /****************Form Event************************/
        #region Form Event
        public frmAllowancesAndDeductions()
        {
            InitializeComponent();
            // ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;

            this.txtID.Validating += new System.ComponentModel.CancelEventHandler(this.txtTypeID_Validating);
            this.txtID.EditValueChanged += new System.EventHandler(this.txtTypeID_EditValueChanged);
            this.lblArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.lblArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
            this.GridViewAllowances.FocusedRowChanged += GridView_FocusedRowChanged;
            lstDetailAllowance = new BindingList<MonthlyAllowance>();
            initGridAllowance();
            initGridDeduction();

        }
        #endregion
        /**********************Function**************************/
        #region Function
        void initGridAllowance()
        {
            lstDetailAllowance = new BindingList<MonthlyAllowance>();
            lstDetailAllowance.AllowNew = true;
            lstDetailAllowance.AllowEdit = true;
            lstDetailAllowance.AllowRemove = true;
            grdControlAllowances.DataSource = lstDetailAllowance;

            GridViewAllowances.Columns["SN"].Visible = false;
            GridViewAllowances.Columns["EmployeeID"].Visible = false;

            GridViewAllowances.Columns["AllowanceID"].Caption = "رقم الاستحقاق";
            GridViewAllowances.Columns["AllowanceName"].Caption = "اسم الاستحقاق";
            GridViewAllowances.Columns["AllowanceAmount"].Caption = "المبلغ";
            GridViewAllowances.Columns["AllowanceValidFromDate"].Caption = "ساري من تاريخ";
            GridViewAllowances.Columns["AllowanceNotes"].Caption = "ملاحظات";


            GridViewAllowances.Columns["AllowanceAccountID"].Caption = "رقم الحساب";
            GridViewAllowances.Columns["AllowanceAccountName"].Caption = "اسم الحساب";

            GridViewAllowances.Columns["AllowanceID"].Width = 50;
            GridViewAllowances.Columns["AllowanceName"].Width = 120;
            GridViewAllowances.Columns["AllowanceAmount"].Width = 60;
            GridViewAllowances.Columns["AllowanceValidFromDate"].Width = 55;
            GridViewAllowances.Columns["AllowanceNotes"].Caption = "ملاحظات";
            ////////////////////////Item
            ///

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName  AS ItemName FROM HR_AllowancesTypes");
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i]["ItemName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            grdControlAllowances.RepositoryItems.Add(riComboBoxitems);
            GridViewAllowances.Columns["AllowanceName"].ColumnEdit = riComboBoxitems;
            ///////////////////////////
            ///


            RepositoryItemDateEdit RepositoryDateEdit = new RepositoryItemDateEdit();
            RepositoryDateEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            RepositoryDateEdit.Mask.EditMask = "dd/MM/yyyy";
            RepositoryDateEdit.Mask.UseMaskAsDisplayFormat = true;
            grdControlAllowances.RepositoryItems.Add(RepositoryDateEdit);
            GridViewAllowances.Columns["AllowanceValidFromDate"].ColumnEdit = RepositoryDateEdit;
            GridViewAllowances.Columns["AllowanceValidFromDate"].UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            GridViewAllowances.Columns["AllowanceValidFromDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            GridViewAllowances.Columns["AllowanceValidFromDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            GridViewAllowances.Columns["AllowanceValidFromDate"].OptionsColumn.AllowEdit = true;
            GridViewAllowances.Columns["AllowanceValidFromDate"].OptionsColumn.ReadOnly = false;
        }
        void initGridDeduction()
        {
            lstDetailDeduction = new BindingList<MonthlyDeduction>();
            lstDetailDeduction.AllowNew = true;
            lstDetailDeduction.AllowEdit = true;
            lstDetailDeduction.AllowRemove = true;
            grdControlDeductions.DataSource = lstDetailDeduction;

            GridVieweductions.Columns["SN"].Visible = false;
            GridVieweductions.Columns["EmployeeID"].Visible = false;
            GridVieweductions.Columns["DeductionID"].Caption = "رقم الاستقطاع";
            GridVieweductions.Columns["DeductionName"].Caption = "اسم الاستقطاع";
            GridVieweductions.Columns["DeductionAmount"].Caption = "المبلغ";
            GridVieweductions.Columns["DeductionValidFromDate"].Caption = "ساري من تاريخ";
            GridVieweductions.Columns["DeductionNotes"].Caption = "ملاحظات";

            GridVieweductions.Columns["DeductionAccountID"].Caption = "رقم الحساب";
            GridVieweductions.Columns["DeductionAccountName"].Caption = "اسم الحساب";

            GridVieweductions.Columns["DeductionID"].Width = 50;
            GridVieweductions.Columns["DeductionName"].Width = 120;
            GridVieweductions.Columns["DeductionAmount"].Width = 60;
            GridVieweductions.Columns["DeductionValidFromDate"].Width = 55;
            GridVieweductions.Columns["DeductionNotes"].Caption = "ملاحظات";
            ////////////////////////Item
            ///

            DataTable dtitems = Lip.SelectRecord("SELECT   ArbName  AS ItemName FROM HR_DeductionsTypes");
            string[] companiesitems = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                companiesitems[i] = dtitems.Rows[i]["ItemName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            grdControlDeductions.RepositoryItems.Add(riComboBoxitems);
            GridVieweductions.Columns["DeductionName"].ColumnEdit = riComboBoxitems;
            ///////////////////////////
            ///


            RepositoryItemDateEdit RepositoryDateEdit = new RepositoryItemDateEdit();
            RepositoryDateEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            RepositoryDateEdit.Mask.EditMask = "dd/MM/yyyy";
            RepositoryDateEdit.Mask.UseMaskAsDisplayFormat = true;
            grdControlDeductions.RepositoryItems.Add(RepositoryDateEdit);
            GridVieweductions.Columns["DeductionValidFromDate"].ColumnEdit = RepositoryDateEdit;
            GridVieweductions.Columns["DeductionValidFromDate"].UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            GridVieweductions.Columns["DeductionValidFromDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            GridVieweductions.Columns["DeductionValidFromDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            GridVieweductions.Columns["DeductionValidFromDate"].OptionsColumn.AllowEdit = true;
            GridVieweductions.Columns["DeductionValidFromDate"].OptionsColumn.ReadOnly = false;
        }
        public void FillGrid()
        {
            try
            {
                strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [الاسم] FROM " + cClass.TableName + " WHERE Cancel =0  ";

                if (UserInfo.Language == iLanguage.English)

                    strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Name] FROM " + cClass.TableName + " WHERE Cancel =0  ";


                DataTable dt = new DataTable();
                dt = Lip.SelectRecord(strSQL);
                GridViewAllowances.GridControl.DataSource = dt;

                GridViewAllowances.Columns[0].Width = 50;
                GridViewAllowances.Columns[1].Width = 100;
            }
            catch { }

        }
        public void Find()
        {
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };
                string SearchSql = "";
                string Condition = "Where 1=1";
                FocusedControl = GetIndexFocusedControl();
                if (FocusedControl == null) return;

                if (FocusedControl.Trim() == txtEmployeeID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtEmployeeID, lblArbName, "EmployeeID", "رقم الموظف", Comon.cInt(UserInfo.BRANCHID));
                    else
                        PrepareSearchQuery.Find(ref cls, txtEmployeeID, lblArbName, "EmployeeID", "Account ID", Comon.cInt(UserInfo.BRANCHID));
                }

                else if (FocusedControl.Trim() == grdControlAllowances.Name)
                {
                    if (GridViewAllowances.FocusedColumn == null) return;

                    if (GridViewAllowances.FocusedColumn.Name == "dgvColAllowanceID")
                    {
                        if (UserInfo.Language == iLanguage.Arabic)
                            PrepareSearchQuery.Find(ref cls, null, null, "AllowanceID", "رقم العلاوة", UserInfo.BRANCHID);
                        else
                            PrepareSearchQuery.Find(ref cls, null, null, "AllowanceID", "Allowance ID", UserInfo.BRANCHID);
                    }

                }
                else if (FocusedControl.Trim() == grdControlDeductions.Name)
                {
                    if (GridVieweductions.FocusedColumn == null) return;


                    if (GridVieweductions.FocusedColumn.Name == "dgvColDeductionID")
                    {
                        if (UserInfo.Language == iLanguage.Arabic)
                            PrepareSearchQuery.Find(ref cls, null, null, "DeductionID", "رقم الاستقطاع", UserInfo.BRANCHID);
                        else
                            PrepareSearchQuery.Find(ref cls, null, null, "DeductionID", "Deduction ID", UserInfo.BRANCHID);
                    }

                }



                GetSelectedSearchValue(cls);
            }
            catch { }
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
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtEmployeeID.Name)
                {
                    txtEmployeeID.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeID_Validating(null, null);
                }

                else if (FocusedControl == grdControlAllowances.Name)
                {
                    if (GridViewAllowances.FocusedColumn.Name == "dgvColAllowanceID")
                    {
                        string AllowanceID = cls.PrimaryKeyValue.ToString();
                        GridViewAllowances.AddNewRow();
                        GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridVieweductions.Columns["AllowanceID"], AllowanceID);
                        DataTable dtAllowanc = Lip.SelectRecord("SELECT AllowanceID, " + PrimaryName + " AS  AllowanceName FROM HR_AllowancesTypes Where Cancel=0 And ID=" + AllowanceID);


                        GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridVieweductions.Columns["AllowanceID"], AllowanceID);
                        GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceName"], dtAllowanc.Rows[0]["AllowanceName"].ToString());

                    }

                    else if (GridVieweductions.FocusedColumn.Name == "dgvColDeductionID")
                    {
                        // AddRow();
                        int DeductionID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        DataTable dtDeductio = Lip.SelectRecord("SELECT DeductionID, " + PrimaryName + " AS  DeductionName FROM HR_DeductionsTypes Where Cancel=0 And ID=" + DeductionID);
                        GridViewAllowances.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridViewAllowances.Columns["DeductionID"], dtDeductio.Rows[0]["DeductionID"].ToString());
                        GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["DeductionName"], dtDeductio.Rows[0]["DeductionName"].ToString());

                    }


                }
            }
        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    txtID.Text = cClass.SN.ToString();
                    txtEmployeeID.Text = cClass.EmployeeID.ToString();
                    txtEmployeeID_Validating(null, null);
                    FillGridViewAllowances(cClass.EmployeeID);
                    FillGridViewDeductions(cClass.EmployeeID);
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void FillGridViewAllowances(long EmployeeID)
        {
            DataTable dtAllowances = new DataTable();
             
            strSQL = @" SELECT   HR_MonthlyAllowance.AllowanceID, HR_MonthlyAllowance.Amount AS AllowanceAmount, HR_MonthlyAllowance.ValidFromDate AS AllowanceValidFromDate, HR_MonthlyAllowance.Notes AS AllowanceNotes, 
                         HR_AllowancesTypes.ArbName AS AllowanceName, HR_MonthlyAllowance.EmployeeID, HR_AllowancesTypes.AccountID AS AllowanceAccountID, Acc_Accounts.ArbName AS AllowanceAccountName
                         FROM   Acc_Accounts RIGHT OUTER JOIN
                         HR_AllowancesTypes ON Acc_Accounts.BranchID = HR_AllowancesTypes.BranchID AND Acc_Accounts.AccountID = HR_AllowancesTypes.AccountID RIGHT OUTER JOIN
                         HR_MonthlyAllowance ON HR_AllowancesTypes.ID = HR_MonthlyAllowance.AllowanceID WHERE        (HR_MonthlyAllowance.EmployeeID = " + EmployeeID + ")   ORDER BY HR_MonthlyAllowance.SN";

            dtAllowances = Lip.SelectRecord(strSQL);

            if (dtAllowances.Rows.Count > 0)
            {
                MonthlyAllowance Object = new MonthlyAllowance();
                lstDetailAllowance = new BindingList<MonthlyAllowance>();
                lstDetailAllowance.AllowNew = true;
                lstDetailAllowance.AllowEdit = true;
                lstDetailAllowance.AllowRemove = true;
                grdControlAllowances.DataSource = lstDetailAllowance;


                for (int i = 0; i < dtAllowances.Rows.Count; i++)
                {
                    Object = new MonthlyAllowance();
                    Object.AllowanceAccountID = Comon.cLong(dtAllowances.Rows[i]["AllowanceAccountID"].ToString());
                    Object.AllowanceAccountName = dtAllowances.Rows[i]["AllowanceAccountName"].ToString();
                    Object.AllowanceAmount = Comon.cDec(dtAllowances.Rows[i]["AllowanceAmount"].ToString());
                    Object.AllowanceID = Comon.cInt(dtAllowances.Rows[i]["AllowanceID"].ToString());
                    Object.AllowanceName = dtAllowances.Rows[i]["AllowanceName"].ToString();
                    Object.AllowanceNotes = dtAllowances.Rows[i]["AllowanceNotes"].ToString();
                    Object.AllowanceValidFromDate = Comon.ConvertSerialToDate(dtAllowances.Rows[i]["AllowanceValidFromDate"].ToString());

                    Object.EmployeeID = Comon.cInt(dtAllowances.Rows[i]["EmployeeID"].ToString());
                    Object.SN = i + 1;
                    lstDetailAllowance.Add(Object);
                }
                grdControlAllowances.DataSource = lstDetailAllowance;
            }
        }
        void FillGridViewDeductions(long EmployeeID)
        {
            DataTable dtDeductions = new DataTable();
            strSQL = @"  SELECT  HR_MonthlyDeduction.DeductionID,HR_MonthlyDeduction.Amount AS DeductionAmount,HR_MonthlyDeduction.ValidFromDate AS DeductionValidFromDate,HR_MonthlyDeduction.Notes AS DeductionNotes, 
                         HR_DeductionsTypes.ArbName AS DeductionName,HR_MonthlyDeduction.EmployeeID, HR_DeductionsTypes.AccountID AS DeductionAccountID, Acc_Accounts.ArbName AS DeductionAccountName
                         FROM   Acc_Accounts RIGHT OUTER JOIN
                         HR_DeductionsTypes ON Acc_Accounts.BranchID = HR_DeductionsTypes.BranchID AND Acc_Accounts.AccountID = HR_DeductionsTypes.AccountID RIGHT OUTER JOIN
                        HR_MonthlyDeduction ON HR_DeductionsTypes.ID =HR_MonthlyDeduction.DeductionID WHERE        (HR_MonthlyDeduction.EmployeeID = " + EmployeeID + ")   ORDER BY HR_MonthlyDeduction.SN ";
 
            
            dtDeductions = Lip.SelectRecord(strSQL);

            if (dtDeductions.Rows.Count > 0)
            {
                MonthlyDeduction Object = new MonthlyDeduction();
                lstDetailDeduction = new BindingList<MonthlyDeduction>();
                lstDetailDeduction.AllowNew = true;
                lstDetailDeduction.AllowEdit = true;
                lstDetailDeduction.AllowRemove = true;
                grdControlDeductions.DataSource = lstDetailDeduction;

                for (int i = 0; i < dtDeductions.Rows.Count; i++)
                {
                    Object = new MonthlyDeduction();
                    Object.DeductionAccountID = Comon.cLong(dtDeductions.Rows[i]["DeductionAccountID"].ToString());
                    Object.DeductionAccountName = dtDeductions.Rows[i]["DeductionAccountName"].ToString();
                    Object.DeductionAmount = Comon.cDec(dtDeductions.Rows[i]["DeductionAmount"].ToString());
                    Object.DeductionID = Comon.cInt(dtDeductions.Rows[i]["DeductionID"].ToString());
                    Object.DeductionName = dtDeductions.Rows[i]["DeductionName"].ToString();
                    Object.DeductionNotes = dtDeductions.Rows[i]["DeductionNotes"].ToString();
                    Object.DeductionValidFromDate = Comon.ConvertSerialToDate(dtDeductions.Rows[i]["DeductionValidFromDate"].ToString());
                    Object.EmployeeID = Comon.cInt(dtDeductions.Rows[i]["EmployeeID"].ToString());
                    Object.SN = i + 1;
                    lstDetailDeduction.Add(Object);
                }
                grdControlDeductions.DataSource = lstDetailDeduction;
            }
        }
        public void ClearFields()
        {
            try
            {
                txtID.Text = cClass.GetNewID().ToString();
                lblArbName.Text = "";
                txtEngName.Text = "";
                txtNotes.Text = "";
                txtEmployeeID.Text = "";
                initGridAllowance();
                initGridDeduction();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        /******************** MoveRec ************************/
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                #region If
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM " + cClass.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + cClass.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + cClass.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + cClass.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + cClass.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + cClass.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + cClass.PremaryKey + " DESC";
                                break;
                            }
                    }

                    cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                        ReadRecord();
                }

                #endregion

                else
                {
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                    return;
                }


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        /*******************Do Functions *************************/
        protected override void DoNew()
        {
            try
            {


                IsNewRecord = true;
                ClearFields();
                txtEmployeeID.Enabled = true;
                txtEmployeeID.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoEdit()
        {
            Validations.DoEditRipon(this, ribbonControl1);
            Validations.EnabledControl(this, true);
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
                MoveRec(Comon.cInt(txtID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtID.Text), xMovePrev);
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
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoSave()
        {
            try
            {
                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;

                }
                if (!IsNewRecord)
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
                if (!Validations.IsValidForm(this))
                    return;
                save();



            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void save()
        {
            DB = new List<SqlExecute>();

            InsertUpdateMasterData();
            SaveToMonthlyAllowance();
            SaveToMonthlyDeduction();
            foreach (var item in DB)
            {
                if(item.StrSQL!=string.Empty)
                Lip.ExecututeSQL(item.StrSQL);
            }
            Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
        }
        public void InsertUpdateMasterData()
        {

            cAdministrations model = new cAdministrations();
            model.TypeID = Comon.cInt(txtID.Text);
            model.ArbName = lblArbName.Text;
            model.EngName = txtEngName.Text;
            model.Notes = txtNotes.Text;
            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.BranchID = UserInfo.BRANCHID;
            model.FacilityID = UserInfo.FacilityID;
            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;
            model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
            model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
            
            if (IsNewRecord == false)
            {
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            }

            else
            {
                model.EditDate = 0;
                model.EditTime = 0;
            }

            model.Cancel = 0;
            Lip.NewFields();
            Lip.Table = cClass.TableName;
            Lip.AddStringField("SN", model.TypeID.ToString());
            Lip.AddStringField("EmployeeID", txtEmployeeID.Text);
            Lip.AddNumericField("BranchID", model.BranchID.ToString());
            Lip.AddNumericField("FacilityID", model.FacilityID.ToString());
            Lip.AddNumericField("UserID", UserInfo.ID);
            Lip.AddNumericField("RegDate", model.RegDate.ToString());
            Lip.AddNumericField("RegTime", model.RegTime.ToString());
            Lip.AddNumericField("EditUserID", UserInfo.ID);
            Lip.AddNumericField("EditDate", model.EditDate.ToString());
            Lip.AddNumericField("EditTime", model.EditDate.ToString());
            Lip.AddStringField("ComputerInfo", model.ComputerInfo);
            Lip.AddStringField("EditComputerInfo", model.ComputerInfo);
            Lip.AddNumericField("Cancel", 0);
            Lip.sCondition = "SN=" + Comon.cInt(txtID.Text);
            DB = new List<SqlExecute>();

            SqlExecute strSql = new SqlExecute();
            strSql.StrSQL = Lip.GetInsertQuary();
            if (!IsNewRecord)
                strSql.StrSQL = Lip.GetUpdateStr();
            DB.Add(strSql);
        }
        public void SaveToMonthlyAllowance()
        {

            Lip.NewFields();
            Lip.Table = "HR_MonthlyAllowance";
            Lip.sCondition = "SN=" + Comon.cInt(txtID.Text);

            //del
            SqlExecute strSql = new SqlExecute();
            strSql.StrSQL = Lip.GetDelete();
            DB.Add(strSql);

            for (int i = 0; i <= GridViewAllowances.DataRowCount - 1; i++)
            {
                Lip.NewFields();
                Lip.Table = "HR_MonthlyAllowance";
                Lip.AddNumericField("SN", Comon.cInt(txtID.Text));
                Lip.AddNumericField("EmployeeID", Comon.cInt(txtEmployeeID.Text));
                Lip.AddNumericField("AllowanceID", GridViewAllowances.GetRowCellValue(i, "AllowanceID").ToString());
                Lip.AddNumericField("Amount", GridViewAllowances.GetRowCellValue(i, "AllowanceAmount").ToString());

                long DeductionValidFromDate = Comon.ConvertDateToSerial(GridViewAllowances.GetRowCellValue(i, "AllowanceValidFromDate").ToString());
                Lip.AddNumericField("ValidFromDate", DeductionValidFromDate.ToString());
                Lip.AddStringField("Notes", GridViewAllowances.GetRowCellValue(i, "AllowanceNotes").ToString());
                
                strSql = new SqlExecute();
                strSql.StrSQL = Lip.GetInsertQuary();

                DB.Add(strSql);
            }
        }
        public void SaveToMonthlyDeduction()
        {

            Lip.NewFields();
            Lip.Table = "HR_MonthlyDeduction";
            Lip.sCondition = "SN=" + Comon.cInt(txtID.Text);

            //del
            SqlExecute strSql = new SqlExecute();
            strSql.StrSQL = Lip.GetDelete();
            DB.Add(strSql);

            for (int i = 0; i <= GridVieweductions.DataRowCount - 1; i++)
            {
                Lip.NewFields();
                Lip.Table = "HR_MonthlyDeduction";
                Lip.AddNumericField("SN", Comon.cInt(txtID.Text));
                Lip.AddNumericField("EmployeeID", Comon.cInt(txtEmployeeID.Text));
                Lip.AddNumericField("DeductionID", GridVieweductions.GetRowCellValue(i, "DeductionID").ToString());
                Lip.AddNumericField("Amount", GridVieweductions.GetRowCellValue(i, "DeductionAmount").ToString());
                long DeductionValidFromDate =Comon.ConvertDateToSerial(GridVieweductions.GetRowCellValue(i, "DeductionValidFromDate").ToString());
                Lip.AddNumericField("ValidFromDate", DeductionValidFromDate.ToString());
                Lip.AddStringField("Notes", GridVieweductions.GetRowCellValue(i, "DeductionNotes").ToString());
                strSql = new SqlExecute();
                strSql.StrSQL = Lip.GetInsertQuary();
                DB.Add(strSql);
            }
        }
        protected override void DoDelete()
        {
            try
            {
                if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }

                int TempID = Comon.cInt(txtID.Text);

                Lip.NewFields();
                Lip.Table = cClass.TableName;
                Lip.AddNumericField("Cancel", 0);
                Lip.sCondition = "ID=" + Comon.cInt(txtID.Text);
                Lip.ExecuteUpdate();
                MoveRec(TempID, xMovePrev);


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoPrint()
        {

            try
            {
                GridViewAllowances.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        #endregion
        /**********************Event**************************/
        #region Event
        private void txtTypeID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;


        }
        private void frmAdministrations_Load(object sender, EventArgs e)
        {
            DoNew();
            strSQL = "SELECT * FROM HR_AllowancesTypes ";
            dtAllowance  = Lip.SelectRecord(strSQL);
            strSQL = "SELECT * FROM HR_DeductionsTypes ";
            dtDeclaration = Lip.SelectRecord(strSQL);
        }
        public void txtTypeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtID.Text));
                    TempUserID = txtID.Text;
                    ClearFields();
                    txtID.Text = TempUserID;
                    if (cClass.FoundResult == true)
                    {
                        if (FormView == true)
                            ReadRecord();
                        else
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);

                            //return;
                        }
                    }
                    else if (FormAdd == true)
                        IsNewRecord = true;
                    else
                        return;
                    Validations.DoReadRipon(this, ribbonControl1);
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int rowIndex = e.FocusedRowHandle;


            }
            catch (Exception)
            {
                return;
            }

        }
        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                int rowIndex = e.RowHandle;
                txtID.Text = GridViewAllowances.GetRowCellValue(rowIndex, GridViewAllowances.Columns[0].FieldName).ToString();
                txtTypeID_Validating(null, null);
            }
            catch { }
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
                lblArbName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
        #endregion
        public void txtEmployeeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as  Name FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cInt(txtEmployeeID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeID, lblArbName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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

        private void GridViewAllowances_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (this.GridViewAllowances.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                string ColName = view.FocusedColumn.FieldName;
                if (GridViewAllowances.FocusedColumn.Name == "colAllowanceName")
                {
                    GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceID"], "");
                    GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceName"], "لا يوجد هذا  الاستقحاق");
                    if (val.ToString() != string.Empty)
                    {
                        DataTable dt = new DataTable();
                        strSQL = "Select ArbName AS  ItemName , ID AS AllowanceID from HR_AllowancesTypes Where ArbName='" + val.ToString().Trim() + "'";
                        dt = Lip.SelectRecord(strSQL);
                        if (dt.Rows.Count > 0)
                        {
                            GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceName"], dt.Rows[0]["ItemName"].ToString());
                            GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceID"], dt.Rows[0]["AllowanceID"].ToString());

                            if (dtAllowance != null && dtAllowance.Rows.Count > 0)
                            {

                                DataRow[] row = dtAllowance.Select("ID =" + Comon.cDbl(dt.Rows[0]["AllowanceID"].ToString()));
                                if (row.Length > 0)
                                {
                                    GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceAccountName"], row[0]["ArbName"].ToString());
                                    GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceAccountID"], row[0]["AccountID"].ToString());
                                }
                            }

                        }
                        else
                        {
                            GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceID"], "");
                            GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceName"], "لا يوجد هذا  الاستحقاق");
                        }
                    }
                }
            }
        }

        private void GridVieweductions_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (this.GridVieweductions.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                string ColName = view.FocusedColumn.FieldName;
                if (GridVieweductions.FocusedColumn.Name == "colDeductionName")
                {
                    GridVieweductions.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridVieweductions.Columns["DeductionID"], "");
                    GridVieweductions.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridVieweductions.Columns["DeductionName"], "لا يوجد هذا  الاستقطاع");
                    if (val.ToString() != string.Empty)
                    {
                        DataTable dt = new DataTable();
                        strSQL = "Select ArbName AS  ItemName , ID AS DeductionID from HR_DeductionsTypes Where ArbName='" + val.ToString().Trim() + "'";
                        dt = Lip.SelectRecord(strSQL);
                        if (dt.Rows.Count > 0)
                        {
                            GridVieweductions.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridVieweductions.Columns["DeductionName"], dt.Rows[0]["ItemName"].ToString());
                            GridVieweductions.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridVieweductions.Columns["DeductionID"], dt.Rows[0]["DeductionID"].ToString());

                            if (dtDeclaration != null && dtDeclaration.Rows.Count > 0)
                            {

                                DataRow[] row = dtDeclaration.Select("ID =" + Comon.cDbl(dt.Rows[0]["DeductionID"].ToString()));
                                if (row.Length > 0)
                                {
                                    GridVieweductions.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridVieweductions.Columns["DeductionAccountName"], row[0]["ArbName"].ToString());
                                    GridVieweductions.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridVieweductions.Columns["DeductionAccountID"], row[0]["AccountID"].ToString());
                                }
                            }
                        }
                        else
                        {
                            GridVieweductions.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridVieweductions.Columns["DeductionID"], "");
                            GridVieweductions.SetRowCellValue(GridVieweductions.FocusedRowHandle, GridVieweductions.Columns["DeductionName"], "لا يوجد هذا  الاستقحاق");
                        }
                    }
                }
            }
        }
    }

    class SqlExecute
    {
        public string StrSQL;
    }
}

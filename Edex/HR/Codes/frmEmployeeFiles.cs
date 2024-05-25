using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Edex.DAL;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.HR.HRClasses;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;
using Edex.SalesAndPurchaseObjects.SalesClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace Edex.HR.Codes
{
    public partial class frmEmployeeFiles : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        private cEmployeeFiles cClass = new cEmployeeFiles();

        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;
        public DataTable dtDeclaration;
        private string strSQL;
        private bool IsNewRecord;
        public string ParentAccountID;
        public int AccountLevel;
        public string ParentID
        {
            get { return ParentAccountID; }
            set { ParentAccountID = value; }
        }
   
        public string ArbName;
        public string EngName;
        public long AccountID;
        public bool IsNew = false;

        BindingList<MonthlyAllowance> lstDetailAllowance = new BindingList<MonthlyAllowance>();
        BindingList<VacationBalance> lstDetailVacation = new BindingList<VacationBalance>();


        List<SqlExecute> DB = new List<SqlExecute>();
        #endregion  
        public frmEmployeeFiles()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            /*****************************************************************************/
            /***************************Initialize Events********************************/
            this.txtEmail.EditValueChanged += new System.EventHandler(this.txtEmail_EditValueChanged);
            this.txtEmail.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
            this.txtEmployeeID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmployeeID_Validating);
            this.txtEmployeeID.EditValueChanged += new System.EventHandler(this.txtEmployeeID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
            try
            {
                FillCombo.FillComboBoxLookUpEdit(cmbParent, "Acc_Accounts", "AccountID", "ArbName", "", "Cancel =0   AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)) + " and BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                cmbParent.EditValue = Comon.cDbl(MySession.GlobalDefaultParentEmployeeAccountID);
                FillCombo.FillComboBoxLookUpEdit(cmbParentAccountID, "Acc_Accounts", "AccountID", "ArbName", "", "Cancel =0   AND AccountLevel=" + (Comon.cInt(MySession.GlobalNoOfLevels) - 1), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cmbAdministration, "HR_Administrations", "ID", "ArbName", "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Account" : "...."));
                FillCombo.FillComboBoxLookUpEdit(cmbDepartment, "HR_Departments", "ID", "ArbName", "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Account" : ".... "));
                FillCombo.FillComboBoxLookUpEdit(cmbJobs, "HR_Jobs", "ID", "ArbName", "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Account" : ".... "));
                FillCombo.FillComboBoxLookUpEdit(cmbScientificDisciplines, "HR_ScientificDisciplines", "ID", "ArbName", "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Account" : "...."));
                FillCombo.FillComboBoxLookUpEdit(cmbReligions, "HR_Religions", "ID", "ArbName", "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Account" : "...."));
                FillCombo.FillComboBoxLookUpEdit(cmbSex, "HR_Sex", "ID", "ArbName", "", "", (UserInfo.Language == iLanguage.English ? "Select Account" : "...."));
                FillCombo.FillComboBoxLookUpEdit(cmbJobs, "HR_Jobs", "ID", "ArbName", "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Account" : "...."));
                FillCombo.FillComboBoxLookUpEdit(cmbWorkType, "HR_WorkingTypes", "ID", "ArbName", "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Account" : "...."));
                FillCombo.FillComboBoxLookUpEdit(cmbNationality, "HR_Nationalities", "ID", "ArbName", "", "Cancel =0   ", (UserInfo.Language == iLanguage.English ? "Select Account" : "...."));
                InitializeFormatDate(txtDateStartWork);
                InitializeFormatDate(txtBeginningDate);
                InitializeFormatDate(txtDateIssuanceID);
                InitializeFormatDate(txtContractEnd);
                initGridAllowance();
                initGridlstDetailVacation();

                List<Acc_DeclaringMainAccounts> AllAccounts = new List<Acc_DeclaringMainAccounts>();
                int BRANCHID = Comon.cInt(UserInfo.BRANCHID);
                int FacilityID = UserInfo.FacilityID;


                strSQL = "SELECT * FROM HR_AllowancesTypes ";
                dtDeclaration =  Lip.SelectRecord(strSQL);

                this.grdControlAllowances.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);

            }
            catch (Exception ex) { }
            }

        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e)
        {
            try
            {
                var grid = sender as GridControl;
                var view = grid.FocusedView as GridView;
                if (view.FocusedColumn == null)
                    return;
                
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
        // Other form-specific code
        #region Function




        /// <summary>
        /// This Function to Get New Account Id
        /// </summary>
        /// <returns></returns>
        public long GetNewAccountID()
        {
            if (Comon.cDbl(cmbParentAccountID.EditValue) > 0)
            {
                try
                {
                    int code;
                    //MySession.GlobalAccountsLevelDigits = 10;
                    int sNode;
                    int SumDigitsCountBeforeSelectedLevel;
                    int DigitsCountForSelectedLevel;
                    long MaxID;
                    string str;
                    string strDigits = "";
                    ParentAccountID = cmbParentAccountID.EditValue + "";
                    AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID)) + 1;
                    str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID);
                    strSQL = "SELECT Sum(DigitsNumber) FROM  Acc_AccountsLevels WHERE  BranchID = " + UserInfo.BRANCHID + " And LevelNumber <" + AccountLevel;
                    SumDigitsCountBeforeSelectedLevel = int.Parse(Lip.GetValue(strSQL));
                    strSQL = "SELECT  DigitsNumber FROM  Acc_AccountsLevels WHERE  BranchID = " + UserInfo.BRANCHID + " And LevelNumber =" + AccountLevel;
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
            return 0;
        }
        /// <summary>
        /// This function to Query to retrieve customer data from the database
        /// </summary>
        public void FillGrid()
        {
            // Query to retrieve customer data from the customers table
            // strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [customer Name] FROM " + cClass.TableName + " WHERE Cancel =0  ";
            
            strSQL = "SELECT " + cClass.PremaryKey + " as [رقم الموظف] , ArbName as [اسم الموظف] FROM " + cClass.TableName + " WHERE Cancel =0 ";

            if (UserInfo.Language == iLanguage.English)
                // Select the table and fields required from it in English
                // Execute the query and save the results in a DataTable
                strSQL = "SELECT  " + cClass.PremaryKey + " as ID, ArbName as [Customer Name] FROM " + cClass.TableName + " WHERE Cancel =0 ";

            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);

            // Display the query results in a GridView
            if (dt.Rows.Count > 0)
            {
                GridView.GridControl.DataSource = dt;
                GridView.Columns[0].Width = 80;
                GridView.Columns[1].Width = 100;
            }
        }
        /// <summary>
        /// this function to select id and name customer
        /// </summary>
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            // Select the table and fields required from it
            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم الموظف] FROM " + cClass.TableName
            + " WHERE Cancel =0 ";

            if (UserInfo.Language == iLanguage.English)
                // Select the table and fields required from it in English
                cls.SQLStr = "SELECT " + cClass.PremaryKey + " as ID, ArbName as [Emp Name] FROM " + cClass.TableName
                + " WHERE Cancel =0 ";
            ColumnWidth = new int[] { 80, 200 };
            if (cls.SQLStr != "")
            {
                frmSearch frm = new frmSearch();
                // Select the search field
                cls.strFilter = "ID";
                if (UserInfo.Language == iLanguage.Arabic)
                    cls.strFilter = "الرقم";
                frm.AddSearchData(cls);
                frm.ColumnWidth = ColumnWidth;
                frm.ShowDialog();
                GetSelectedSearchValue(cls);
            }
        }

        /// <summary>
        /// This function to Get Selected Search Value
        /// </summary>
        /// <param name="cls"></param>
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                txtEmployeeID.Text = cls.PrimaryKeyValue.ToString();
                txtEmployeeID_Validating(null, null);
            }

        }

        /// <summary>
        /// This function to read record from cCustomers class to field
        /// </summary>
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    //set values to field
                    txtEmployeeID.Text = cClass.EmployeeID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtMobile.Text = cClass.Mobile;
                    txtTel.Text = cClass.Tel;
                    txtAddress.Text = cClass.Address;
                    txtFax.Text = cClass.Fax;
                    txtNotes.Text = cClass.Notes;
                    txtEmail.Text = cClass.Email;
                    txtFootprintEmpID.Text = cClass.FootprintEmpID;
                    cmbParentAccountID.EditValue = Comon.cDbl(cClass.ParentAccountID.ToString());
                    txtAccountID.Text = cClass.AccountID.ToString();
                    txtWorkingHours.Text = cClass.WorkingHours.ToString();
                    chkStopAccount.Checked = Comon.cInt(cClass.StopAccount) == 1 ? true : false;
                    txtAge.Text = cClass.TerminationReason.ToString();
                    txtCardID.Text = cClass.CardID;
                    cmbSex.EditValue = cClass.Sex;
                    txtBeginningDate.EditValue = Comon.ConvertSerialToDate(cClass.BirthDate.ToString());
                    txtContractEnd.EditValue = Comon.ConvertSerialToDate(cClass.ContractEnd.ToString());
                    txtDateStartWork.EditValue = Comon.ConvertSerialToDate(cClass.DateStartWork.ToString());
                    txtDateIssuanceID.EditValue = Comon.ConvertSerialToDate(cClass.DateIssuanceID.ToString());
                    cmbNationality.EditValue = cClass.Nationality;
                    cmbReligions.EditValue = cClass.Religions;
                    cmbWorkType.EditValue = cClass.WorkType;
                    txtWorkingHours.Text = cClass.WorkingHours.ToString();
                    cmbScientificDisciplines.EditValue = cClass.ScientificDisciplines;
                    cmbJobs.EditValue = cClass.Occupation;
                    cmbAdministration.EditValue = cClass.Administration;
                    cmbDepartment.EditValue = cClass.Department;
                    txtCurrentSponsorMobile.Text = cClass.WorkAddress;
                    txtSponsorLocation.Text = cClass.CompanyVehicle;
                    txtCurrentSponsor.Text = cClass.CurrentSponsor;
                    txtCurrentSponsorMobile.Text = cClass.CurrentSponsorMobile;
                    cmbParentAccountID.EditValue = cClass.ParentAccountID;
                    cmbParent.EditValue = cClass.AccountMeter;
                    FillGridViewAllowances(Comon.cLong( txtEmployeeID.Text));
                    FillGridViewVacationBalance(Comon.cLong( txtEmployeeID.Text));


                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /// <summary>
        /// This Function For Clear All TextBox
        /// </summary>
        public void ClearFields()
        {
            try
            {
                txtEmployeeID.Text = cClass.GetNewID().ToString();
                txtAccountID.Text = GetNewAccountID().ToString();

                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtMobile.Text = " ";
                txtTel.Text = " ";
                txtAddress.Text = " ";
                txtFax.Text = " ";
                txtNotes.Text = " ";
                txtEmail.Text = "";
                txtFootprintEmpID.Text = "";
                txtWorkingHours.Text = " ";
                chkStopAccount.Checked = false;
                txtMobile.Text = "";
                txtTel.Text = "";
                txtAddress.Text = "";
                txtFax.Text = "";
                txtCardID.Text = "";
                cmbSex.ItemIndex = 0;
                cmbNationality.ItemIndex = 0;
                cmbReligions.ItemIndex = 0;
                cmbWorkType.ItemIndex = 0;
                cmbScientificDisciplines.ItemIndex = 0;
                cmbParentAccountID.ItemIndex = 0;
                cmbAdministration.ItemIndex = 0;
                cmbDepartment.ItemIndex = 0;
                cmbJobs.ItemIndex = 0;
                cmbWorkType.ItemIndex = 0;
                cmbParent.ItemIndex = 0;
                txtWorkingHours.Text = "";
                txtAge.Text = "";
                txtCurrentSponsorMobile.Text = "";
                txtSponsorLocation.Text = "";
                txtCurrentSponsor.Text = "";

                InitializeFormatDate(txtDateStartWork);
                InitializeFormatDate(txtBeginningDate);
                InitializeFormatDate(txtDateIssuanceID);
                InitializeFormatDate(txtContractEnd);
                initGridAllowance();
                initGridlstDetailVacation();
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
                    strSQL = "SELECT TOP 1 * FROM " + cClass.TableName + " Where Cancel =0    and BranchID= " + UserInfo.BRANCHID;
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
                txtArbName.Focus();

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
                MoveRec(Comon.cInt(txtEmployeeID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtEmployeeID.Text), xMovePrev);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /// <summary>
        /// This Function For Show Interface To Search
        /// </summary>
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
        protected override void DoEdit()
        {
            Validations.DoEditRipon(this, ribbonControl1);
            Validations.EnabledControl(this, true);
        }

        protected override void DoSave()
        {
            try
            {
                IsNew = false;
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
                if (Comon.cDbl(txtAccountID.Text)<=0)
                {
                    Messages.MsgWarning(Messages.TitleWorning,UserInfo.Language==iLanguage.Arabic?"الرجاء اختيار رقم الحساب المباشر ":"Please choose your direct account number");
                    return;
                }
                if (IsNewRecord)
                    txtEmployeeID.Text = cClass.GetNewID().ToString();

                HR_EmployeeFile model = new HR_EmployeeFile();
                model.EmployeeID = Comon.cLong(txtEmployeeID.Text);

                model.OnAccountID = Comon.cLong(txtAccountID.Text);
                //Comon.cLong(txtAccountID.Text);
                if (IsNewRecord == true)
                {
                    //model.EmployeeID = 0;
                    IsNew = true;
                    // model.OnAccountID = GetNewAccountID();
                }
                model.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                model.ArbName = txtArbName.Text;
                ArbName = txtArbName.Text;
                EngName = txtEngName.Text;
                model.EngName = txtEngName.Text;
                model.WorkingHours = Comon.cLong(txtWorkingHours.Text);
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.WorkTel = txtTel.Text.Trim();
                model.WorkMobile = txtMobile.Text.Trim();
                model.BankAccountID = txtFax.Text.Trim();
                model.HomeAddress = txtAddress.Text.Trim();
                model.FootprintEmpID = Comon.cInt(txtFootprintEmpID.Text.Trim());
                model.EmpNotes = txtNotes.Text.Trim();
                model.WorkEmail = txtEmail.Text.Trim();

                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;
                model.Termination = 0;

                model.BirthPlace = "";
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.HomeMobil = txtMobile.Text;
                model.HomeTel = txtTel.Text;
                model.HomeAddress = txtAddress.Text;
                model.WorkMobile = txtFax.Text;
                model.CardID = txtCardID.Text;
                model.Sex = Comon.cInt(cmbSex.EditValue);
                model.MaritalStatus = 1;

                model.BirthDate = Comon.ConvertDateToSerial(txtBeginningDate.Text);
                model.ContractEnd = Comon.ConvertDateToSerial(txtContractEnd.Text);
                model.DateStartWork = Comon.ConvertDateToSerial(txtDateStartWork.Text);
                model.DateIssuanceID = Comon.ConvertDateToSerial(txtDateIssuanceID.Text);
                model.BeginningDate = Comon.ConvertDateToSerial(txtBeginningDate.Text);


                model.Nationality = Comon.cInt(cmbNationality.EditValue);
                model.Religions = Comon.cInt(cmbReligions.EditValue);
                model.WorkType = Comon.cInt(cmbWorkType.EditValue);
                model.WorkingHours = Comon.cInt(txtWorkingHours.Text);
                model.ScientificDisciplines = Comon.cInt(cmbScientificDisciplines.EditValue);
                model.Occupation = Comon.cInt(cmbJobs.EditValue);
                model.IqamaOccupation = Comon.cInt(cmbJobs.EditValue);
                model.ContractType = Comon.cInt(cmbWorkType.EditValue);
                model.Administration = Comon.cInt(cmbAdministration.EditValue);
                model.CurrentSponsorMobile = txtCurrentSponsorMobile.Text;

                model.TerminationReason = Comon.cInt(txtAge.Text);
                model.PaymentMethod = 1;
                model.TerminationReason = 1;
                model.Department = Comon.cInt(cmbDepartment.EditValue);
                model.StopSalary = 0;
                model.ClinicID = 0;
                model.Emptype = 0;
                model.CostCenterID = 0;
                model.LeaveNotes = "";
                model.WorkAddress = txtCurrentSponsorMobile.Text;
                model.CompanyVehicle = txtSponsorLocation.Text;
                model.CurrentSponsor = txtCurrentSponsor.Text;
                AccountID = long.Parse(model.OnAccountID.ToString());
                model.ParentAccountID = Comon.cDbl(cmbParentAccountID.EditValue);
                model.AccountMeter = Comon.cDbl(cmbParent.EditValue);

                long StoreID;
                StoreID =  HR_EmployeeFileDAL.InsertHR_EmployeeFile(model, IsNewRecord);
               
                //حفظ المستحقات
                DB = new List<SqlExecute>();
                SaveToEmployeeAllowance();
                SaveToVacationBalance();
                addAccountID();
                if (StoreID > 0)
                    foreach (var item in DB)
                    {
                        if (item.StrSQL != string.Empty)
                            Lip.ExecututeSQL(item.StrSQL);
                    }

                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                if (IsNewRecord == true)
                    DoNew();

                FillGrid();
                if(IsNewRecord ==false)
                FillGridViewAllowances(Comon.cLong(txtEmployeeID.Text));

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public void SaveToEmployeeAllowance()
        {
            //del
            Lip.NewFields();
            Lip.Table = "HR_EmployeeAllowance";
            Lip.sCondition = "EmployeeID=" + Comon.cInt(txtEmployeeID.Text);
            SqlExecute strSql = new SqlExecute();
            strSql.StrSQL = Lip.GetDelete();
            DB.Add(strSql);
            //=================================================



            for (int i = 0; i <= GridViewAllowances.DataRowCount - 1; i++)
            {
                Lip.NewFields();
                Lip.Table = "HR_EmployeeAllowance";
                Lip.AddNumericField("EmployeeID", Comon.cInt(txtEmployeeID.Text));
                Lip.AddNumericField("AllowanceID", GridViewAllowances.GetRowCellValue(i, "AllowanceID").ToString());
                Lip.AddNumericField("Amount", GridViewAllowances.GetRowCellValue(i, "AllowanceAmount").ToString());
                long DeductionValidFromDate = Comon.ConvertDateToSerial(GridViewAllowances.GetRowCellValue(i, "AllowanceValidFromDate").ToString());
                Lip.AddNumericField("ValidFromDate", DeductionValidFromDate.ToString());
                Lip.AddStringField("Notes", GridViewAllowances.GetRowCellValue(i, "AllowanceNotes").ToString());
                Lip.AddNumericField("Cancel",0);
                Lip.AddNumericField("UserID", UserInfo.ID);
                Lip.AddNumericField("BranchID", UserInfo.BRANCHID);
                Lip.AddNumericField("RegDate", Lip.GetServerDateSerial());
                Lip.AddNumericField("RegTime", Lip.GetServerTimeSerial());
                Lip.AddStringField("ComputerInfo", UserInfo.ComputerInfo);
                Lip.AddStringField("AllowanceAccountID", GridViewAllowances.GetRowCellValue(i, "AllowanceAccountID").ToString());
             
                if (IsNewRecord == false)
                {
                    Lip.AddNumericField("EditUserID", UserInfo.ID);
                    Lip.AddNumericField("EditDate", Lip.GetServerDateSerial());
                    Lip.AddNumericField("EditTime", Lip.GetServerDateSerial());
                    Lip.AddStringField("EditComputerInfo", UserInfo.ComputerInfo);
                }
                else
                {
                    Lip.AddNumericField("EditUserID",0);
                    Lip.AddNumericField("EditDate", 0);
                    Lip.AddNumericField("EditTime", 0);
                    Lip.AddStringField("EditComputerInfo","");
                }
                strSql = new SqlExecute();
                strSql.StrSQL = Lip.GetInsertQuary();
                DB.Add(strSql);
            }
        }

        public void SaveToVacationBalance()
        {
            //Del
            Lip.NewFields();
            Lip.Table = "HR_VacationBalance";
            Lip.sCondition = "EmployeeID=" + Comon.cInt(txtEmployeeID.Text);
            SqlExecute strSql = new SqlExecute();
            strSql.StrSQL = Lip.GetDelete();
            DB.Add(strSql);

            //add
            for (int i = 0; i <= GridViewVacationBalance.DataRowCount - 1; i++)
            {
                Lip.NewFields();
                Lip.Table = "HR_VacationBalance";
                Lip.AddNumericField("EmployeeID", Comon.cInt(txtEmployeeID.Text));
                Lip.AddNumericField("Year", GridViewVacationBalance.GetRowCellValue(i, "Year").ToString());
                Lip.AddNumericField("AccuredVacation", GridViewVacationBalance.GetRowCellValue(i, "AccuredVacation").ToString());
                Lip.AddNumericField("Cancel", 0);
                Lip.AddNumericField("UserID", UserInfo.ID);
                Lip.AddNumericField("BranchID", UserInfo.BRANCHID);
                Lip.AddNumericField("RegDate", Lip.GetServerDateSerial());
                Lip.AddNumericField("RegTime", Lip.GetServerTimeSerial());
                Lip.AddStringField("ComputerInfo", UserInfo.ComputerInfo);

                if (IsNewRecord == false)
                {
                    Lip.AddNumericField("EditUserID", UserInfo.ID);
                    Lip.AddNumericField("EditDate", Lip.GetServerDateSerial());
                    Lip.AddNumericField("EditTime", Lip.GetServerDateSerial());
                    Lip.AddStringField("EditComputerInfo", UserInfo.ComputerInfo);
                }
                else
                {

                    Lip.AddNumericField("EditUserID", 0);
                    Lip.AddNumericField("EditDate", 0);
                    Lip.AddNumericField("EditTime", 0);
                    Lip.AddStringField("EditComputerInfo", "");
                }
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
                int TempID = Comon.cInt(txtEmployeeID.Text);
                HR_EmployeeFile model = new HR_EmployeeFile();
                model.EmployeeID = Comon.cInt(txtEmployeeID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());                 
                if (new cCustomers().CheckAccountHasTransactions(Comon.cLong(cClass.AccountID)) == true)
                {
                    XtraMessageBox.Show("الحساب لديه حركة شراء وبيع لايمكن حذفه  ");
                }
                else
                {
                    bool Result = new HR_EmployeeFileDAL().DeleteHR_EmployeeFile(model);
                    bool Result1 = DelAccountID();
                    if (Result == true && Result1 == true)
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    MoveRec(model.EmployeeID, xMovePrev);
                    FillGrid();
                }


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
                /******************** Report Header *************************/
                GridView.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        /************************************ **********************************************/
        //This Function for Add Acc_AccountID 
        public void addAccountID()
        {
             

            Acc_Accounts model = new Acc_Accounts();
            model.AccountID = AccountID;
            model.AccountLevel = AccountLevel;
            model.AccountTypeID = 1;
            model.BranchID = UserInfo.BRANCHID;
            model.FacilityID = UserInfo.FacilityID;
            model.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
            model.ParentAccountID = long.Parse(cmbParentAccountID.EditValue.ToString());
            model.MaxLimit = 0;
            model.MinLimit = 0;
            model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
            model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.Cancel = 0;
            model.ArbName = ArbName;
            model.EngName = EngName;
            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;
            int StoreID;
            //model.BranchID = Comon.cInt(UserInfo.BRANCHID);
            if (IsNewRecord == true)
                StoreID = Acc_AccountsDAL.InsertAcc_Accounts(model);
            else
                Acc_AccountsDAL.UpdateAcc_Accounts(model);



        }

        //This Function For Delete The Acc_AccountID 
        public bool DelAccountID()
        {
            Acc_Accounts model = new Acc_Accounts();
            model.AccountID = Comon.cLong(cClass.AccountID);
            model.BranchID = UserInfo.BRANCHID;
            model.FacilityID = UserInfo.FacilityID;
            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.EditUserID = UserInfo.ID;
            model.EditComputerInfo = UserInfo.ComputerInfo;

            bool Result;
            Result = Acc_AccountsDAL.DeleteAcc_Accounts(model);
            return Result;
        }
        //This Function For Exception the Email 
        private bool EmailAddressChecker(string emailAddress)
        {

            string regExPattern = "^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";
            bool emailAddressMatch = Match.Equals(emailAddress, regExPattern);

            return emailAddressMatch;
        }


        void FillGridViewAllowances(long EmployeeID)

        {
             
            DataTable dtAllowances = new DataTable();
             
             
            strSQL = @"SELECT HR_EmployeeAllowance.AllowanceID, HR_EmployeeAllowance.EmployeeID ,HR_EmployeeAllowance.Amount AS AllowanceAmount, HR_EmployeeAllowance.ValidFromDate AS AllowanceValidFromDate, HR_EmployeeAllowance.Notes AS AllowanceNotes, 
                         HR_AllowancesTypes.ArbName AS AllowanceName, HR_EmployeeAllowance.AllowanceAccountID, Acc_Accounts.ArbName AS AllowanceAccountName
                         FROM   HR_EmployeeAllowance LEFT OUTER JOIN
                         Acc_Accounts ON HR_EmployeeAllowance.AllowanceAccountID = Acc_Accounts.AccountID AND HR_EmployeeAllowance.BranchID = Acc_Accounts.BranchID LEFT OUTER JOIN
                         HR_AllowancesTypes ON HR_EmployeeAllowance.AllowanceID = HR_AllowancesTypes.ID
                        WHERE(HR_EmployeeAllowance.BranchID = " + UserInfo.BRANCHID + ") AND(HR_EmployeeAllowance.Cancel = 0) AND(HR_EmployeeAllowance.EmployeeID = " + EmployeeID + ")";


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
                    Object.AllowanceAccountID = Comon.cLong( dtAllowances.Rows[i]["AllowanceAccountID"].ToString());
                    Object.AllowanceAccountName = dtAllowances.Rows[i]["AllowanceAccountName"].ToString();
                    Object.AllowanceAmount = Comon.cDec( dtAllowances.Rows[i]["AllowanceAmount"].ToString());
                    Object.AllowanceID = Comon.cInt(dtAllowances.Rows[i]["AllowanceID"].ToString());
                    Object.AllowanceName =  dtAllowances.Rows[i]["AllowanceName"].ToString();
                    Object.AllowanceNotes = dtAllowances.Rows[i]["AllowanceNotes"].ToString();
                    Object.AllowanceValidFromDate= Comon.ConvertSerialToDate(dtAllowances.Rows[i]["AllowanceValidFromDate"].ToString());

                    Object.EmployeeID = Comon.cInt(dtAllowances.Rows[i]["EmployeeID"].ToString());
                    Object.SN = i+1;
                    lstDetailAllowance.Add(Object);
                }
                grdControlAllowances.DataSource = lstDetailAllowance;
            }
        }
        void FillGridViewVacationBalance(long EmployeeID)
        {


            DataTable dtVacation = new DataTable();
            strSQL = "SELECT * FROM HR_VacationBalance WHERE (EmployeeID = " + EmployeeID + ") AND (BranchID = " + UserInfo.BRANCHID + ") AND (Cancel = 0)";
            dtVacation = Lip.SelectRecord(strSQL);
            if (dtVacation.Rows.Count > 0)
            {
                grdControlVacationBalance.DataSource = dtVacation;
            }
        }
        #endregion
        #region Event
 




    

        private void frmEmployeeFiles_Load(object sender, EventArgs e)
        {
            FillGrid();
            DoNew();
        
        }

        private void txtEmployeeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (long.Parse(txtEmployeeID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cLong(txtEmployeeID.Text));
                    TempUserID = txtEmployeeID.Text;
                    ClearFields();//clear all field
                    txtEmployeeID.Text = TempUserID;
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
                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtEmployeeID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

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

                txtEmployeeID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtEmployeeID_Validating(null, null);

            }
            catch (Exception)
            {
                return;
            }

        }
        private void txtArbName_EditValueChanged_1(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int rowIndex = e.RowHandle;
            txtEmployeeID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtEmployeeID_Validating(null, null);
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
        // This  Event Validating TextEdit For Email
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
                //{

                //    if (EmailAddressChecker(txtEmail.Text) == false)
                //    {
                //        txtEmail.Focus();
                //        ToolTipController toolTip = new ToolTipController();
                //        txtEmail.ToolTipController = toolTip;
                //        toolTip.Appearance.BackColor = Color.AntiqueWhite;
                //        toolTip.ShowBeak = true;
                //        toolTip.CloseOnClick = DefaultBoolean.True;
                //        toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                //        toolTip.InitialDelay = 500;
                //        toolTip.ShowBeak = true;
                //        toolTip.Rounded = true;
                //        toolTip.ShowShadow = true;
                //        toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                //        toolTip.SetToolTipIconType(txtEmail, ToolTipIconType.Error);
                //        toolTip.ToolTipType = ToolTipType.Standard;
                //        toolTip.SetTitle(txtEmail, "Error");
                //        toolTip.ShowHint(Messages.msgInputEmil, ToolTipLocation.TopLeft, txtEmail.PointToScreen(new Point(0, txtEmail.Height)));
                //        txtEmail.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);

                //    }
                //} 



            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void txtEmail_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        //This Event To Save The Customer By F9 
        private void frmCustomers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
                DoSave();

        }

        #endregion
        private void tabPage1_Click(object sender, EventArgs e)
        {
        }
        private void cmbParentAccountID_EditValueChanged(object sender, EventArgs e)
        {
            if(IsNewRecord)
            txtAccountID.Text = GetNewAccountID().ToString();
        }
        private void txtTel_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnAlwanceAndDecut_Click(object sender, EventArgs e)
        {
            frmAllowancesAndDeductions frm = new frmAllowancesAndDeductions();
            frm.Show();
            frm.FormView = true;
            frm.FormAdd = true;
            frm.FormUpdate = true;

            if (IsNewRecord == false)
            {
                frm.txtEmployeeID.Text = txtEmployeeID.Text;
                frm.txtEmployeeID_Validating(null, null);
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
            GridViewAllowances.Columns["AllowanceAccountName"].Caption = "اسم الحساب";

            GridViewAllowances.Columns["AllowanceID"].Width = 50;
            GridViewAllowances.Columns["AllowanceName"].Width = 70;
            GridViewAllowances.Columns["AllowanceAmount"].Width = 60;
            GridViewAllowances.Columns["AllowanceValidFromDate"].Width = 65;
            GridViewAllowances.Columns["AllowanceAccountID"].Caption = "رقم الحساب";

            ////////////////////////Item
            ///


            DataTable dtAlloance = Lip.SelectRecord("SELECT   ArbName  AS ItemName FROM HR_AllowancesTypes");
            string[] companiesitems = new string[dtAlloance.Rows.Count];
            for (int i = 0; i <= dtAlloance.Rows.Count - 1; i++)
                companiesitems[i] = dtAlloance.Rows[i]["ItemName"].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(companiesitems);

            grdControlAllowances.RepositoryItems.Add(riComboBoxitems);
            GridViewAllowances.Columns["AllowanceName"].ColumnEdit = riComboBoxitems;
            //================


            //DataTable dtAccounts = Lip.SelectRecord("SELECT  ArbName as AccountName FROM Acc_Accounts WHERE Cancel =0   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID = " + UserInfo.BRANCHID);
            //string[] companiesAccounts = new string[dtAccounts.Rows.Count];
            //for (int i = 0; i <= dtAccounts.Rows.Count - 1; i++)
            //    companiesAccounts[i] = dtAccounts.Rows[i]["AccountName"].ToString();

            //RepositoryItemComboBox riComboBoxAccounts = new RepositoryItemComboBox();
            //riComboBoxAccounts.Items.AddRange(companiesAccounts);


            //grdControlAllowances.RepositoryItems.Add(riComboBoxAccounts);
            //GridViewAllowances.Columns["AllowanceAccountName"].ColumnEdit = riComboBoxAccounts;


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
        void initGridlstDetailVacation()
        {
            lstDetailVacation = new BindingList<VacationBalance>();
            lstDetailVacation.AllowNew = true;
            lstDetailVacation.AllowEdit = true;
            lstDetailVacation.AllowRemove = true;
            grdControlVacationBalance.DataSource = lstDetailVacation;

            GridViewVacationBalance.Columns["SN"].Visible = false;
            GridViewVacationBalance.Columns["EmployeeID"].Visible = false;

            GridViewVacationBalance.Columns["Year"].Caption = "السنة";
            GridViewVacationBalance.Columns["AccuredVacation"].Caption = "الأجـازة المستحقة بالأيام";
            GridViewVacationBalance.Columns["Year"].Width = 70;
            GridViewVacationBalance.Columns["AccuredVacation"].Width = 130;
             
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

                            if (dtDeclaration != null && dtDeclaration.Rows.Count > 0)
                            {

                                DataRow[] row = dtDeclaration.Select("ID =" + Comon.cDbl(dt.Rows[0]["AllowanceID"].ToString()));
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
                 if (GridViewAllowances.FocusedColumn.Name == "colAllowanceAccountName")
                {
                    GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceAccountID"], "");
                    GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceAccountName"], "");
                    if (val.ToString() != string.Empty)
                    {
                        DataTable dt = new DataTable();
                        strSQL = "Select  AccountID , ArbName AS AccountName from Acc_Accounts Where ArbName='" + val.ToString().Trim() + "'";
                        dt = Lip.SelectRecord(strSQL);
                        if (dt.Rows.Count > 0)
                        {
                            GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceAccountName"], dt.Rows[0]["AccountName"].ToString());
                            GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceAccountID"], dt.Rows[0]["AccountID"].ToString());
                        }
                        else
                        {
                            GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceAccountID"], "");
                            GridViewAllowances.SetRowCellValue(GridViewAllowances.FocusedRowHandle, GridViewAllowances.Columns["AllowanceAccountName"], "");
                        }
                    }
                }


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chkStopAccount.Checked = true;
            DoSave();
        }

        private void btnRequerstVecation_Click(object sender, EventArgs e)
        {
            frmVacationRequest frm = new frmVacationRequest();
            frm.Show();
            frm.FormView = true;
            frm.FormAdd = true;
            frm.FormUpdate = true;

            if (IsNewRecord == false)
            {
                frm.txtEmployeeID.Text = txtEmployeeID.Text;
                frm.txtEmployeeID_Validating(null, null);
            }
        }

        private void btnRecordAbsent_Click(object sender, EventArgs e)
        {
            frmRecordAbsent frm = new frmRecordAbsent();
            frm.Show();
            frm.FormView = true;
            frm.FormAdd = true;
            frm.FormUpdate = true;

            if (IsNewRecord == false)
            {
                frm.txtEmployeeID.Text = txtEmployeeID.Text;
                frm.txtEmployeeID_Validating(null, null);
            }
        }

        private void btnAddDurationEmp_Click(object sender, EventArgs e)
        {
            frmAddEmployeeDurationManually frm = new frmAddEmployeeDurationManually();
            frm.Show();
            frm.FormView = true;
            frm.FormAdd = true;
            frm.FormUpdate = true;
            if (IsNewRecord == false)
            {
                frm.txtEmployeeID.Text = txtEmployeeID.Text;
                frm.txtEmployeeID_Validating(null, null);
            }
        }
     
        private void frmEmployeeFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();

            //else if (e.KeyCode == Keys.F2)
            //    ShortcutOpen();
            if (e.KeyCode == Keys.F9)
                DoSave();
        }
    }
}

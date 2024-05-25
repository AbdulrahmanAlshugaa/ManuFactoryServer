using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
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
using Edex.Model.Language;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraReports.UI;
using Edex.Reports;
namespace Edex.StockObjects.Codes
{
    public partial class frmStores : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        public static  Stc_Stores cClass = new Stc_Stores();
        string FocusedControl = "";
        public int num = 0;

        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;
        public Boolean OpenFromMain = false;
        private string strSQL;
        private bool IsNewRecord;
        public string ParentAccountID;
        public int AccountLevel;
        public string GetNewID;
        public string ParentID
        {
            get { return ParentAccountID; }
            set { ParentAccountID = value; }
        }
        public string ArbName;
        public string EngName;
        public long AccountID;
        string PrimaryName = "ArbName";
        #endregion
        #region Form Event
        public frmStores()
        {
            InitializeComponent();
            if(UserInfo.Language==iLanguage.English)
                PrimaryName = "EngName";
            /***************************Edit & Print & Export ****************************/
           //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;          
            /*****************************************************************************/
            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtStoreID.EditValueChanged += new System.EventHandler(this.txtStoreID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
            FillCombo.FillComboBoxLookUpEdit(cmbParent, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  and BranchID="+MySession.GlobalBranchID+" AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)) + " and BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
            cmbParent.EditValue = Comon.cDbl(MySession.GlobalDefaultParentStoreAccountID);
            FillCombo.FillComboBoxLookUpEdit(cmbParentAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  and BranchID=" + MySession.GlobalBranchID + "  AND AccountLevel=" + (Comon.cInt(MySession.GlobalNoOfLevels) - 1), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
            txtEmpoleeyID.Validating += TxtEmpoleeyID_Validating;
        }

        private void TxtEmpoleeyID_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmpoleeyID.Text) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                CSearch.ControlValidating(txtEmpoleeyID, txtStoreManager , strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #endregion
        #region

        public void FillGrid()
        {

            if (cClass == null)
                cClass = new Stc_Stores();
            strSQL = "SELECT " + cClass.PremaryKey + " as الرقم,   Stc_Stores.AccountID as [رقم الحساب],  Stc_Stores.ArbName as [اسم المستودع], HR_EmployeeFile.ArbName as [مسؤول المستودع] FROM " + cClass.TableName + " left outer JOIN HR_EmployeeFile ON HR_EmployeeFile.EmployeeID = Stc_Stores.StoreManger and HR_EmployeeFile.BranchID = Stc_Stores.BranchID WHERE Stc_Stores.Cancel = 0   and  Stc_Stores.BranchID=" + MySession.GlobalBranchID ;
                if (UserInfo.Language == iLanguage.English)
                    strSQL = "SELECT  " + cClass.PremaryKey + " as ID,AccountID as [Account ID], EngName as [Unit Name],StoreManger as [Store manager ] FROM " + cClass.TableName + " WHERE Cancel =0   and BranchID=" + MySession.GlobalBranchID ;

                DataTable dt = new DataTable();
                dt = Lip.SelectRecord(strSQL);
                GridView.GridControl.DataSource = dt;
                GridView.Columns[0].Width = 50;
                GridView.Columns[1].Width = 100;
                if (OpenFromMain == true)
                {
                    DoNew();
                }
        }
        string GetIndexFocusedControl()
        {
            // Get the currently active control.
            Control c = this.ActiveControl;

            // If the active control is a DevExpress LayoutControl, get the focused child control.
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            // If the active control is a DevExpress TextBoxMaskBox,
            // set the control to its parent control.
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }

            // If the parent of the active control is a DevExpress GridControl,
            // return its name as the focused control.
            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }

            // Otherwise, return the name of the active control.
            return c.Name;
        }
        protected override void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;
            if (FocusedControl.Trim() == txtStoreID.Name)
            {
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المستودع] FROM " + cClass.TableName
                + " WHERE Cancel =0  and BranchID="+MySession.GlobalBranchID;

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, ArbName as [Store Name] FROM " + cClass.TableName
                + " WHERE Cancel =0   and BranchID=" + MySession.GlobalBranchID;
                if (cls.SQLStr != "")
                {
                    frmSearch frm = new frmSearch();
                    cls.strFilter = "الرقم";
                    if (UserInfo.Language == iLanguage.English)
                        cls.strFilter = "ID";

                    frm.AddSearchData(cls);
                    frm.ColumnWidth = ColumnWidth;
                    frm.ShowDialog();
                    
                }
            }

            else if (FocusedControl.Trim() == txtEmpoleeyID.Name)
            {
                ColumnWidth = new int[] { 80, 200 };
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmpoleeyID, txtStoreManager, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmpoleeyID, txtStoreManager, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                
            }
            GetSelectedSearchValue(cls);
        }
        
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }
            }
            else if (FocusedControl.Trim() == txtEmpoleeyID.Name)
            {
                txtEmpoleeyID.Text = cls.PrimaryKeyValue.ToString();
                TxtEmpoleeyID_Validating(null, null);
            }

        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {

                     
                    txtStoreID.Text = cClass.StoreID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtMobile.Text = cClass.Mobile;
                    txtTel.Text = cClass.Tel;
                    txtAddress.Text = cClass.Address;
                     txtEmpoleeyID.Text = cClass.StoreManger;
                    TxtEmpoleeyID_Validating(null, null);
                    txtFax.Text = cClass.Fax;
                    txtNotes.Text = cClass.Notes;
                    cmbParentAccountID.EditValue =   Comon.cDbl(cClass.ParentAccountID.ToString()) ;                   
                    txtAccountID.Text = cClass.AccountID.ToString(); 
                    num = int.Parse(txtStoreID.Text) - 1058;
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = num + "/" + GridView.RowCount;
                    chkStopAccount.Checked = Comon.cInt(cClass.StopAccount) == 1 ? true : false;

                }



            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public long GetNewAccountID()
        {
            if (Comon.cDbl(cmbParentAccountID.EditValue) > 0)
            {
                try
                {
                    int code;
                    
                    int sNode;
                    int SumDigitsCountBeforeSelectedLevel;
                    int DigitsCountForSelectedLevel;
                    long MaxID;
                    string str;
                    string strDigits = "";
                    //ParentAccountID = Lip.GetValue("SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE DeclareAccountName='StoreAccount'");
                    ParentAccountID = cmbParentAccountID.EditValue + "";
                    AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID+"  and BranchID="+MySession.GlobalBranchID)) + 1;
                    str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID+" and BranchID="+MySession.GlobalBranchID);
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
                    return long.Parse(GetNewID.PadRight(MySession.GlobalAccountsLevelDigits, '0'));
                }
                catch (Exception ex)
                {
                    Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }
                
            }
            return 0;
        }
        public void addAccountID()
        {
            long testID = GetNewAccountID();
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
            strSQL = "Select * from Acc_Accounts where  BRANCHID= " + model.BranchID + " and AccountID=" + txtAccountID.Text;
            DataTable dtAcco = new DataTable();
            dtAcco = Lip.SelectRecord(strSQL);
            if (dtAcco.Rows.Count > 0)
                Acc_AccountsDAL.UpdateAcc_Accounts(model);
            else
                Acc_AccountsDAL.InsertAcc_Accounts(model);
            //strSQL = "SELECT  *   FROM  Branches";
            //DataTable dtBank = Lip.SelectRecord(strSQL);
            //if (dtBank.Rows.Count > 0)
            //{
            //    for (int i = 0; i <= dtBank.Rows.Count - 1; i++)
            //    {
            //        model.BranchID = Comon.cInt(dtBank.Rows[i]["BRANCHID"].ToString());

            //        strSQL = "Select * from Acc_Accounts where  BRANCHID= " + model.BranchID + " and AccountID=" + txtAccountID.Text;
            //        DataTable dtAcco = new DataTable();
            //        dtAcco = Lip.SelectRecord(strSQL);
            //        if (dtAcco.Rows.Count > 0)
            //            Acc_AccountsDAL.UpdateAcc_Accounts(model);
            //        else
            //            Acc_AccountsDAL.InsertAcc_Accounts(model);

            //    }
            //}

        }
        public void ClearFields()
        {
            try
            {
                txtStoreID.Text = STC_STORES_DAL.GetNewID().ToString();
               
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtMobile.Text = " ";
                txtTel.Text = " ";
                txtAddress.Text = " ";
                txtEmpoleeyID.Text = " ";
                TxtEmpoleeyID_Validating(null, null);
                txtFax.Text = " ";
                txtNotes.Text = " ";
                chkStopAccount.Checked = false;
                txtAccountID.Text = GetNewAccountID().ToString();
       
               
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
                if (cClass == null)
                    cClass = new Stc_Stores();
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM " + cClass.TableName + " Where Cancel =0  and BranchID="+MySession.GlobalBranchID;
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
                    cClass = STC_STORES_DAL.GetRecordSetBySQL(strSQL);
                    if (cClass != null)
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
        public void NewRecord() 
        {
            DoNew();
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
        }
        protected override void DoNew()
        {
            try
            {

                IsNewRecord = true;
                 
                ClearFields();
                txtArbName.Focus();
                EnabledControl(true);
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
                MoveRec(Comon.cInt(txtStoreID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtStoreID.Text), xMovePrev);
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


                Stc_Stores model = new Stc_Stores();
                model.StoreID = Comon.cInt(txtStoreID.Text);
                 
                model.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                model.AccountID = Comon.cLong(txtAccountID.Text);
                model.ArbName = txtArbName.Text;
                model.EngName = txtEngName.Text;
                ArbName = txtArbName.Text;
                EngName = txtEngName.Text;
                model.Address = txtAddress.Text;
                model.FacilityID = MySession.GlobalFacilityID;
                model.BranchID = MySession.GlobalBranchID;
                model.Tel = txtTel.Text;
                model.Mobile = txtMobile.Text;
                model.Fax = txtFax.Text;
                model.StoreManger =txtEmpoleeyID.Text;

                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;

                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());

                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                AccountID = long.Parse(model.AccountID.ToString());

                model.ParentAccountID = Comon.cDbl(cmbParentAccountID.EditValue);
                model.Cancel = 0;
                int StoreID;
                bool updateModel;
                if (IsNewRecord)
                    StoreID = STC_STORES_DAL.InsertStc_Stores(model);
                else
                    updateModel = STC_STORES_DAL.UpdateStc_Stores(model);

                addAccountID();
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                if (IsNewRecord == true)
                    DoNew();

                FillGrid();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
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
        protected override void DoDelete()
        {
            if (Lip.CheckAccountingTransactions(Comon.cLong(txtAccountID.Text)))
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

                    int TempID = Comon.cInt(txtStoreID.Text);

                    Stc_Stores model = new Stc_Stores();
                    model.StoreID = Comon.cInt(txtStoreID.Text);
                    model.UserID = UserInfo.ID;
                    model.BranchID = MySession.GlobalBranchID;
                    model.FacilityID = MySession.GlobalFacilityID;
                    model.EditUserID = UserInfo.ID;
                    model.EditComputerInfo = UserInfo.ComputerInfo;
                    model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                    model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                    bool Result = STC_STORES_DAL.DeleteStc_Stores(model);
                    bool result = DelAccountID();
                    if (Result == true)
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                    MoveRec(model.StoreID, xMovePrev);
                    FillGrid();

                }
                catch (Exception ex)
                {
                    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }
            }
            else
            {
                Messages.MsgAsterisk("لا يمكن الحذف", "لا يمكن حذف المخزن بسبب وجود حركات محاسبية علية");

            }
        }
        protected override void DoPrint()
        {

            try
            {
                /******************** Report Header *************************/
                bool IncludeHeader = true;
                XtraReport rptCompanyHeader = new rptCompanyHeaderArb();
                rptCompanyHeader = (UserInfo.Language == iLanguage.English ? new rptCompanyHeaderEng() : rptCompanyHeader);

                /******************** Report Body *************************/
                dynamic rptForm = new rptPurchaseInvoiceArb();
                rptForm = (UserInfo.Language == iLanguage.English ? new rptPurchaseInvoiceEng() : rptForm);

                var dataTable = new dsReports.rptSizingUnitDataTable();
                var query = STC_STORES_DAL.GetAllData(1, 1);

                /********************** Master *****************************/
                rptForm.Parameters["parameter1"].Value = "1455";


                /********************** Details ****************************/
                foreach (var item in query)
                {
                    var row = dataTable.NewRow();
                    row["ArbName"] = item.ArbName;
                    row["EngName"] = item.EngName;
                    row["StoreID"] = item.StoreID;
                    dataTable.Rows.Add(row);

                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptSizingUnitDataTable";

                /******************** Report Footer *************************/


                /******************** Report Binding ************************/
                rptForm.RequestParameters = false;
                rptForm.subRptCompanyHeader.Visible = IncludeHeader;
                rptForm.subRptCompanyHeader.ReportSource = rptCompanyHeader;
                rptForm.CreateDocument();

                frmReportViewer frmRptViewer = new frmReportViewer();
                frmRptViewer.documentViewer1.DocumentSource = rptForm;
                frmRptViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        #endregion
        #region Event
        public  void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtStoreID.Text.Trim()) > 0)
                {
                    
                    TempUserID = txtStoreID.Text;
                    cClass = STC_STORES_DAL.GetDataByID(Comon.cInt(txtStoreID.Text), MySession.GlobalFacilityID, MySession.GlobalBranchID);
                  
                    txtStoreID.Text = TempUserID;
                    if (cClass != null)
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
        private void txtStoreID_EditValueChanged(object sender, EventArgs e)
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

                txtStoreID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtStoreID_Validating(null, null);

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
            txtStoreID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtStoreID_Validating(null, null);
        }

        #endregion
        private void frmStores_Activated(object sender, EventArgs e)
        {
            //rowCount = cClass.GetNewID();
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
        private void frmStores_Load(object sender, EventArgs e)
        {
            ClearFields();
            FillGrid();

        }
        private void frmStores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
                Find();

        }

        private void cmbParentAccountID_EditValueChanged(object sender, EventArgs e)
        {
            txtAccountID.Text = GetNewAccountID().ToString();
        }
    }
}

using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.StockObjects.StoresClasses;
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
using Edex.SalesAndPurchaseObjects.SalesClasses;
using DevExpress.Utils;
using DevExpress.XtraRichEdit.API.Native;
namespace Edex.SalesAndPurchaseObjects.Codes
{
    public partial class frmSuppliers : Edex.GeneralObjects.GeneralForms.BaseForm
    { 
        #region Declare
        public DataTable importData = new DataTable();
        public bool sendFromExel = false;   
        private cSuppliers cClass = new cSuppliers();

        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;
        public string PrimaryName = "";
        private string strSQL;
        private bool IsNewRecord;
        public string ParentAccountID;
        public int AccountLevel;
        public string ArbName;
        public string EngName;
        public long AccountID;
        public bool IsNew = false;
        #endregion
         
        #region Function
      
        /// <summary>
        /// This Function For Get New Account ID
        /// </summary>
        /// <returns></returns>
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
                    //ParentAccountID = Lip.GetValue("SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE DeclareAccountName='SupplierAccount'");
                    ParentAccountID = cmbParentAccountID.EditValue + "";
                    AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID+" and BranchID="+MySession.GlobalBranchID)) + 1;
                    str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID + "  And BranchID =" + MySession.GlobalBranchID);
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

        /// <summary>
        /// This function is to add an account ID to the accounts table
        /// </summary>
        public void addAccountID()
        {
            long testID = GetNewAccountID();
            Acc_Accounts model = new Acc_Accounts();
            model.AccountID = AccountID;
            model.AccountLevel = AccountLevel;
            model.AccountTypeID = 1;
            model.BranchID = MySession.GlobalBranchID;
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
            model.EndType = 1;
            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;
            Acc_AccountsDAL dal = new Acc_AccountsDAL();
            int StoreID;

            strSQL = "Select * from Acc_Accounts where  BranchID= " + model.BranchID + " and AccountID=" + txtAccountID.Text;
            DataTable dtAcco = new DataTable();
            dtAcco = Lip.SelectRecord(strSQL);

            if (dtAcco.Rows.Count > 0)
                Acc_AccountsDAL.UpdateAcc_Accounts(model);
            else
                Acc_AccountsDAL.InsertAcc_Accounts(model);

            //strSQL = "SELECT  *   FROM  Branches";
            //DataTable dtcustomer = Lip.SelectRecord(strSQL);
            //if (dtcustomer.Rows.Count > 0)
            //{
            //    for (int i = 0; i <= dtcustomer.Rows.Count - 1; i++)
            //    {
            //        model.BranchID = Comon.cInt(dtcustomer.Rows[i]["BranchID"].ToString());

            //        strSQL = "Select * from Acc_Accounts where  BranchID= " + model.BranchID + " and AccountID=" + txtAccountID.Text;
            //        DataTable dtAcco = new DataTable();
            //        dtAcco = Lip.SelectRecord(strSQL);
                  
            //        if (dtAcco.Rows.Count > 0)
            //            Acc_AccountsDAL.UpdateAcc_Accounts(model);
            //        else
            //            Acc_AccountsDAL.InsertAcc_Accounts(model);

            //    }
            //}
        }
        /// <summary>
        /// This function is to delete an account by account ID
        /// </summary>
        /// <returns></returns>
        public bool DelAccountID()
        {
            Acc_Accounts model = new Acc_Accounts();
            model.AccountID = Comon.cLong(cClass.AccountID);
            model.BranchID = MySession.GlobalBranchID;
            model.FacilityID = UserInfo.FacilityID;
            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.EditUserID = UserInfo.ID;
            model.EditComputerInfo = UserInfo.ComputerInfo;

            bool Result;
            Result = Acc_AccountsDAL.DeleteAcc_Accounts(model);
            return Result;
        }

        private bool EmailAddressChecker(string emailAddress)
        {

            string regExPattern = "^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";
            bool emailAddressMatch = Match.Equals(emailAddress, regExPattern);

            return emailAddressMatch;
        }
        /// <summary>
        /// This function is to display the data in the  GridVie
        /// </summary>
        public void FillGrid()
        {
            try{
            strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المورد] FROM " + cClass.TableName + " WHERE Cancel =0 and BranchID="+ MySession.GlobalBranchID;

            if (UserInfo.Language == iLanguage.English)

                strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Supplier Name] FROM " + cClass.TableName + " WHERE Cancel =0  and BranchID=" + MySession.GlobalBranchID;


            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            GridView.GridControl.DataSource = dt;

            GridView.Columns[0].Width = 50;
            GridView.Columns[1].Width = 100;
            }
            catch { }
        }

        /// <summary>
        /// This function displays the search screen for search Supplier
        /// </summary>
        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المورد ] FROM " + cClass.TableName
            + " WHERE Cancel =0  and BranchID= " + MySession.GlobalBranchID;

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Supplier Name] FROM " + cClass.TableName
            + " WHERE Cancel =0  and BranchID= " + MySession.GlobalBranchID;


            ColumnWidth = new int[] { 80, 200 };



            if (cls.SQLStr != "")
            {
                frmSearch frm = new frmSearch();

                cls.strFilter = "الرقم";
                if (UserInfo.Language == iLanguage.English)
                    cls.strFilter = "ID";

                frm.AddSearchData(cls);
                frm.ColumnWidth = ColumnWidth;
                frm.ShowDialog();
                GetSelectedSearchValue(cls);
            }
            }
            catch { }
        }

        /// <summary>
        /// This function displays the search value that was chosen
        /// </summary>
        /// <param name="cls"></param>
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                txtSupplierID.Text = cls.PrimaryKeyValue.ToString();
                txtSupplierID_Validating(null, null);
            }

        }

        /**********************This function is to read the record***************************/
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {


                    txtSupplierID.Text = cClass.SupplierID.ToString();
                    cmbNationality.EditValue = cClass.NationalityID;
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtMobile.Text = cClass.Mobile;
                    txtTel.Text = cClass.Tel;
                    txtAddress.Text = cClass.Address;
                    txtFax.Text = cClass.Fax;
                    txtNotes.Text = cClass.Notes;
                    txtEmail.Text = cClass.Email;
                    txtVAT.Text = cClass.VATID;
                    cmbParentAccountID.EditValue =  Comon.cDbl(cClass.ParentAccountID.ToString());
                
                    txtCommercialRegister.Text = cClass.CommercialRegister;
                    chkStopAccount.Checked = Comon.cInt(cClass.StopAccount) == 1 ? true : false;
                    txtAuthorizedPerson.Text = cClass.AuthorizedPerson.ToString();
                    txtBankAccountNo.Text = cClass.BankAccountNo.ToString();
                    txtBankName.Text = cClass.BankName;
                    txtAccountID.Text = cClass.AccountID.ToString();
                    Validations.DoReadRipon(this, ribbonControl1);              
                    EnabledControl(false);
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /*************************This function clear the fields of data*********************/
        public void ClearFields()
        {
            try
            {
                cmbNationality.ItemIndex = 0;
                txtSupplierID.Text = cClass.GetNewID().ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtMobile.Text = " ";
                txtTel.Text = " ";
                txtAddress.Text = " ";
                txtFax.Text = " ";
                txtNotes.Text = " ";
                txtEmail.Text = "";
                txtVAT.Text = "";
                txtAuthorizedPerson.Text = "";
                txtBankAccountNo.Text = "";
                txtBankName.Text = "";

                txtCommercialRegister.Text = "";
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
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM " + cClass.TableName + " Where Cancel =0 and BranchID=" + MySession.GlobalBranchID;
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


        /********************************************Do Functions ********************************************/
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
                MoveRec(Comon.cInt(txtSupplierID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtSupplierID.Text), xMovePrev);
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


                Sales_Suppliers model = new Sales_Suppliers();

                model.SupplierID = Comon.cInt(txtSupplierID.Text);
                model.NationalityID = Comon.cInt(cmbNationality.EditValue);
                model.CommercialRegister = txtCommercialRegister.Text.ToString();
                model.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                model.AccountID = cClass.AccountID;
                //Comon.cLong(txtAccountID.Text);

                if (IsNewRecord == true)
                {
                    model.SupplierID = 0;
                    IsNew = true;
                    model.AccountID = GetNewAccountID();
                }
                model.ArbName = txtArbName.Text;
                ArbName = txtArbName.Text;
                EngName = txtEngName.Text;
                model.EngName = txtEngName.Text;
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.BranchID = MySession.GlobalBranchID;
                model.FacilityID = UserInfo.FacilityID;
                model.Tel = txtTel.Text;
                model.Mobile = txtMobile.Text;
                model.Fax = txtFax.Text;
                model.Address = txtAddress.Text;
                model.VATID = txtVAT.Text;
                model.Notes = txtNotes.Text;
                model.Email = txtEmail.Text;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;
                model.AuthorizedPerson = txtAuthorizedPerson.Text;
                model.BankAccountNo = txtBankAccountNo.Text;
                model.BankName = txtBankName.Text;
                AccountID = long.Parse(model.AccountID.ToString());

                model.ParentAccountID = Comon.cDbl(cmbParentAccountID.EditValue);
                int StoreID;
                int UpdateID;

                if (IsNewRecord == true)
                    StoreID = Sales_SuppliersDAL.InsertSales_Suppliers(model);
                else
                    UpdateID = Sales_SuppliersDAL.UpdateSales_Suppliers(model);




               
                /*************  Add Account to account tree***************/
                addAccountID();
                /////////////////////////////////////////////////////////////////
                if (sendFromExel == false)
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

                int TempID = Comon.cInt(txtSupplierID.Text);
                Sales_Suppliers model = new Sales_Suppliers();
                model.SupplierID = Comon.cInt(txtSupplierID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = MySession.GlobalBranchID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                if (cClass.CheckAccountHasTransactions(Comon.cLong(cClass.AccountID)) == true)
                {
                    XtraMessageBox.Show("الحساب لديه حركة شراء وبيع لايمكن حذفه  ");
                }
                else
                {
                    bool Result = Sales_SuppliersDAL.DeleteSales_Suppliers(model);
                    bool Result1 = DelAccountID();
                    if (Result == true && Result1 == true)
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    MoveRec(model.SupplierID, xMovePrev);
                    FillGrid();



                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            }
             else
             {
                 Messages.MsgAsterisk("لا يمكن الحذف", "لا يمكن حذف حساب المورد  بسبب وجود حركات محاسبية علية");

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

        #endregion

        #region Event
        public frmSuppliers()
        {
            InitializeComponent();
            /***************************Edit & Print & Export ****************************/
            

            /*****************************************************************************/

            this.txtEmail.EditValueChanged += new System.EventHandler(this.txtEmail_EditValueChanged);
            this.txtEmail.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
            this.txtSupplierID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSupplierID_Validating);
            this.txtSupplierID.EditValueChanged += new System.EventHandler(this.txtSupplierID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
             PrimaryName = "ArbName";
             if (UserInfo.Language == iLanguage.English)
             { PrimaryName = "EngName"; }
             FillCombo.FillComboBoxLookUpEdit(cmbParent, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0   AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)) + " and BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
            cmbParent.EditValue = Comon.cDbl(MySession.GlobalDefaultParentSupplierAccountID);
            FillCombo.FillComboBoxLookUpEdit(cmbParentAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0   AND AccountLevel=" + (Comon.cInt(MySession.GlobalNoOfLevels) - 1) + " and BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
             
                }
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtSupplierID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtSupplierID.Text));
                    TempUserID = txtSupplierID.Text;
                    ClearFields();
                    txtSupplierID.Text = TempUserID;
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

        private void txtSupplierID_EditValueChanged(object sender, EventArgs e)
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

                txtSupplierID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtSupplierID_Validating(null, null);

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
            try{
            int rowIndex = e.RowHandle;
            txtSupplierID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtSupplierID_Validating(null, null);
            }
            catch { }
        }

 
        /// <summary>
        /// This Event Validating the Arbic Name  field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtArbName_Validating(object sender, CancelEventArgs e)
        {

            int CustomerID = Comon.cInt(Lip.GetValue("SELECT   SupplierID FROM  [Sales_Suppliers] where " + PrimaryName + "='" + txtArbName.Text + "' and Cancel=0 and BranchID=" + MySession.GlobalBranchID));

            if (CustomerID > 0&& CustomerID!=Comon.cInt(txtSupplierID.Text))
            {
                bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "اسم المورد موجود من قبل... هل تريد المتابعة ؟" : "Customer Name is Find . are you following");
                if (!yes)
                    return;
            }
            TextEdit obj = (TextEdit)sender;
            if (UserInfo.Language == iLanguage.Arabic)
                txtEngName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
      /// <summary>
      /// This Event Validating the English Name  field
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        private void txtEngName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;

            if (UserInfo.Language == iLanguage.English)
                txtArbName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
        /// <summary>
        /// This is Event Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSuppliers_Load(object sender, EventArgs e)
        {
            FillCombo.FillComboBox(cmbNationality, "HR_Nationalities", "ID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillGrid();
            DoNew();

        }
        /// <summary>
        /// This Event Validating the Email field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                //        toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txtEmail.PointToScreen(new Point(0, txtEmail.Height)));
                //        txtEmail.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);

                //   }
                //} 



            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /// <summary>
        /// This is Event txtEmail EditValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEmail_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void btnImbort_Click(object sender, EventArgs e)
        {
            //    Form1 frm = new Form1();

            //    if (frm.ShowDialog(this) == DialogResult.OK)
            //    {
            //        // Read the contents of testDialog's TextBox.
            //        importData = frm.dataTable;
            //    }

            //    frm.Dispose();
            //    DoNew();
            //    sendFromExel = true;
            //    for (int i = 0; i < importData.Rows.Count - 1; ++i)
            //    {
            //        txtArbName.Text = importData.Rows[i][0].ToString();
            //        txtEngName.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][1].ToString() : "");
            //        txtAddress.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][6].ToString() : "");
            //        txtMobile.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][3].ToString() : "");
            //        txtTel.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][2].ToString() : "");
            //        txtFax.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][4].ToString() : "");
            //       // txtSpecialDiscount.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][8].ToString() : "");
            //        txtEmail.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][5].ToString() : "");
            //        txtNotes.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][7].ToString() : "");

            //        txtVAT.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][9].ToString() : "");



            //        txtBankAccountNo.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][12].ToString() : "");
            //        txtBankName.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][13].ToString() : "");

            //        txtAuthorizedPerson.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][14].ToString() : "");
            //        DoSave();


            //    }
            //    sendFromExel = false;

        }
        #endregion
        protected override void DoEdit()
        {
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
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
        private void cmbParentAccountID_EditValueChanged(object sender, EventArgs e)
        {
            txtAccountID.Text = GetNewAccountID().ToString();
        }

       

     
    }

}


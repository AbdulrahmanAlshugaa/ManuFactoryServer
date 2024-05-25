using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;

 
using Edex.Model;

using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
//using Edex.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
 
using System.Text.RegularExpressions;
using Edex;
using Edex.Model.Language;
 
using Edex.DAL;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmFacility : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        #region Declare
        private string PrimaryName;
        private cFacility cClass = new cFacility();
        private DataTable dt = new DataTable();
        private DataTable dtDeclarAccounts = new DataTable();
        private DataTable dtNewTree = new DataTable();
        cStarting defgh;
        BindingList<cStarting> defg = new BindingList<cStarting>();
        public int LevelDigits;
        int rowIndex;
        public readonly string TableName = "GLB_FACILITY";
        public readonly string PrimaryKey = "ID";
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;
        private string strSQL;
        private bool IsNewRecord;

        #endregion
        #region Form Event
        public frmFacility()
        {
            PrimaryName = "ArbName";
            InitializeComponent();

            
         
            this.txtEmail.EditValueChanged += new System.EventHandler(this.txtEmail_EditValueChanged);
            this.txtEmail.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
            this.txtFacilityID.Validating += new System.ComponentModel.CancelEventHandler(this.txtFacilityID_Validating);
            this.txtFacilityID.EditValueChanged += new System.EventHandler(this.txtFacilityID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);

            if (USERPERMATIONS.GET_FORMPERMATION(this) == false)
              this.Dispose();
            
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
            }
        }
        #endregion
        #region Function
        public int InsertingDataToFacilityTable()
        {
            try
            {
                cFacility model = new cFacility();
                model.FacilityID= Comon.cInt(txtFacilityID.Text);
                model.ArbName = txtArbName.Text;
                model.EngName = txtEngName.Text;
                model.Address = txtAddress.Text;
                model.Email = txtEmail.Text;
                model.Tel = txtTel.Text;
                model.IsActive = 0;
                if (tgsCanChangeDocumentsDate.EditValue.ToString() == "True")
                    model.IsActive = 1;


                model.Fax = txtFax.Text;

                model.DELETED = 0;
                model.USERUPDATED = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;



                model.USERCREATED = UserInfo.ID;
                model.DATECREATED = Comon.cInt(Lip.GetServerDateSerial());
                model.CREATEDTIME = Comon.cInt(Lip.GetServerTimeSerial());


                model.USERUPDATED = UserInfo.ID;
                model.DATEUPDATED = Comon.cInt(Lip.GetServerDateSerial());
                model.UPDATEDTIME = Comon.cInt(Lip.GetServerTimeSerial());

                model.USERDELETED = UserInfo.ID;
                model.TIMEDELETED = Comon.cInt(Lip.GetServerTimeSerial());
                model.DATEDELETED = Comon.cInt(Lip.GetServerDateSerial());



                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;


                int Result = 0;

                //if (IsNewRecord == true)
                //    Result = FACILITYDAL.Insert(model, MySession.DBName);
                //else
                //    Result = FACILITYDAL.Update(model, MySession.DBName);

                if (Result >= 1)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                txtFacilityID.Text = Result.ToString();
                return Result;

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return 0;
            }

        }
      
        public void FillDataGrid()
        { 
            FillGrid.FillGridView(GridView, cClass.TableName, PrimaryKey);
        }



        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                txtFacilityID.Text = cls.PrimaryKeyValue.ToString();
                txtFacilityID_Validating(null, null);
            }

        }


        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {

                    txtFacilityID.Text = cClass.BranchID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtTel.Text = cClass.Tel;
                    txtAddress.Text = cClass.Address;
                    txtFax.Text = cClass.Fax;
                    txtEmail.Text = cClass.Email;
                  
                    if (cClass.IsActive == 1)
                        tgsCanChangeDocumentsDate.EditValue = true;
                    else
                        tgsCanChangeDocumentsDate.EditValue = false;

                    lblNoOfAccountLevels.Text = getNewLevel().ToString();
                    txtLevelCounts.Text = GetNoOfLevels().ToString();
  
                      
                  FillGrid.FillGridView(gridVBranchesCompany, "Branches", cClass.PremaryKey, " FacilityID=" + txtFacilityID.Text);

                }
               


            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public int getNewLevel()
        {
            

            strSQL = "SELECT MAX(LEVELNUMBER) FROM ACC_ACCOUNTSLEVELS WHERE FACILITYID =" + Comon.cInt(txtFacilityID.Text);

            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return Comon.cInt(dt.Rows[0][0] == DBNull.Value ? 0 : dt.Rows[0][0]);
            else
                return -1;



        }

        public int GetNoOfLevels()
        {
            try
            {

                string strSQL = "SELECT MAX(LevelNumber) FROM ACC_ACCOUNTSLEVELS WHERE FACILITYID =" + txtFacilityID.Text;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return Comon.cInt(dt.Rows[0][0]);


            }

            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return 0;

        }

        public void ClearFields()
        {
            try
            {
                txtFacilityID.Text = cClass.GetNewID().ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtTel.Text = " ";
                txtAddress.Text = " ";
                txtFax.Text = " ";
                txtEmail.Text = " ";
                tgsCanChangeDocumentsDate.EditValue = true;

                if (IsNewRecord == true)
                {
                    chkDefaultTree.Visible = true;
                    chkNewTree.Visible = true;
                   
                }

                else
                {
                    chkDefaultTree.Visible = false;
                    chkNewTree.Visible = false;
                    chkDefaultTree.Checked = true;
                    txtLevelCounts.Visible = false;
                    lblLevelCounts.Visible = false;
                    gridControl1.Visible = false;
                }
               
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /******************** MoveRec ************************/

        public void MoveRec(long PremaryKeyValue, int Direction)
        {


            string where = " 1=1" ;
            try
            {
                switch (Direction)
                {
                    case xMoveFirst:
                        strSQL = "SELECT * FROM " + TableName + "  WHERE  " + where + " AND " + PrimaryKey + "=(SELECT MIN(" + PrimaryKey + ") FROM " + TableName + "  WHERE  " + where + ")";
                        break;
                    case xMoveNext:
                        strSQL = "SELECT * FROM (SELECT * FROM " + TableName + " WHERE " + PrimaryKey + " > " + PremaryKeyValue + " AND " + where + " ORDER BY " + PrimaryKey + " ASC) Table2 WHERE rownum =1 ORDER BY rownum ASC";
                        break;
                    case xMovePrev:
                        strSQL = "SELECT * FROM (SELECT * FROM " + TableName + " WHERE " + PrimaryKey + " < " + PremaryKeyValue + " AND  " + where + " ORDER BY " + PrimaryKey + " DESC) Table2 WHERE rownum =1 ORDER BY rownum DESC";
                        break;
                    case xMoveLast:
                        strSQL = "SELECT * FROM " + TableName + "  WHERE " + where + " AND " + PrimaryKey + "=(SELECT MAX(" + PrimaryKey + ") FROM " + TableName + " WHERE  " + where + ")";
                        break;
                }
                DataTable dt = new DataTable();
                cClass.GetRecordSetBySQL(strSQL);
                if (cClass.FoundResult == true)
                    ReadRecord();


            }
            catch (Exception ex)
            {

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
                MoveRec(Comon.cInt(txtFacilityID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtFacilityID.Text), xMovePrev);
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
                 Lovs.Find(cClass.TableName, cClass.PremaryKey);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void GetSelectedSearch(CSearch cls)
        {
            GetSelectedSearchValue(cls);
        }

        protected override void DoSave()
        {
            try
            {
                //IsNew = false;
                bool FirstBranch = false;
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

                Application.DoEvents();

                strSQL = "Select   *  From GLB_BRANCH WHERE DELETED=0 and FACILITY=" + txtFacilityID.Text;
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count == 0)
                    FirstBranch = true;

                if (IsNewRecord == true)
                {
                    InsertingDataToFacilityTable();
                    if (FirstBranch == true)
                    {
                        int BranchID = 1;// InsertingDataToBranchsTable();
                        int RoleID=  saveGropFirstRole(BranchID);
                        InsertingAdminUserToUsersTable(BranchID, RoleID);

                        if (chkNewTree.Checked == true)
                            InsertingNewAccountsTreeTamplateIntoAccountsTree(BranchID);

                        if (chkDefaultTree.Checked == true)
                            InsertingDefaultAccountsTreeTamplateIntoAccountsTree(BranchID);
                       
                        //بدلها يكون تعريف المجموعات 
                        // InsertingDeclaringMainAccountsTamplateIntoDeclaringMainAccountsTable(BranchID);

                    }
                       

                      // InsertingStartNumberingTemplateIntoStartNumberingTable();
                  
                    //ViewPermissionMenuForBranch();


                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                }
                else
                {
                    InsertingDataToFacilityTable();
                }
                if (IsNewRecord == true)
                    DoNew();
                FillDataGrid();


          
                if (IsNewRecord == true)
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

        public int saveGropFirstRole(int BranchID)
        {
             
            return 1;

        }
        void SaveFormsPermissions(int BranchID,int FacilityID,int RoleID,string GropName)
        {
             
        }
        void SaveFormsPermissions(int GroupID,int BranchID)
        {
            
        }
     
        public void InsertingNewAccountsTreeTamplateIntoAccountsTree(int BranchID)
        {
           for (int i = 0; i <= dgvAccountsLevel.RowCount - 1; ++i)
            {

                Lip.NewFields();
                Lip.Table = "Acc_AccountsLevels";
                Lip.AddNumericField("FACILITYID", txtFacilityID.Text);
                Lip.AddNumericField("BranchID", BranchID);
                Lip.AddNumericField("LevelNumber", dgvAccountsLevel.GetRowCellValue(i, "LevelNumber").ToString());
                Lip.AddNumericField("DigitsNumber", dgvAccountsLevel.GetRowCellValue(i, "DigitsNumber").ToString());
                Lip.ExecuteInsert();
            }
            DataTable dtNewTree = new DataTable();
            strSQL = "SELECT * FROM  Acc_NewAccountsTreeTamplate ";
            dtNewTree = Lip.SelectRecord(strSQL);

            LevelDigits = GetAccountDigits();
            //'Inserting the new tree tamplete into accounts tree table
            for (int i = 0; i <= dtNewTree.Rows.Count - 1; ++i)
            {
                try
                {
                    Lip.NewFields();
                    Lip.Table = "Acc_Accounts";
                    Lip.AddNumericField("AccountID", dtNewTree.Rows[i]["AccountID"].ToString().PadRight(LevelDigits, '0'));
                    Lip.AddNumericField("BranchID",0);
                    Lip.AddNumericField("FACILITYID", txtFacilityID.Text);
                    Lip.AddStringField("ArbName", dtNewTree.Rows[i]["ArbName"].ToString());
                    Lip.AddStringField("EngName", dtNewTree.Rows[i]["EngName"].ToString());
                    Lip.AddNumericField("ParentAccountID", dtNewTree.Rows[i]["ParentAccountID"].ToString());
                    Lip.AddNumericField("AccountLevel", dtNewTree.Rows[i]["AccountLevel"].ToString());
                    Lip.AddNumericField("AccountTypeID", dtNewTree.Rows[i]["AccountTypeID"].ToString());
                    Lip.AddNumericField("StopAccount", 0);
                    Lip.AddNumericField("MinLimit", 0);
                    Lip.AddNumericField("MaxLimit", 0);
                    Lip.AddNumericField("UserID", UserInfo.ID);
                    Lip.AddNumericField("RegDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("RegTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditUserID", UserInfo.ID);
                    Lip.AddNumericField("EditDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddStringField("ComputerInfo", "ggg");
                    Lip.AddStringField("EditComputerInfo", "dvsdfs");
                    Lip.AddNumericField("Cancel", 0);
                    Lip.ExecuteInsert();

                }
                catch (Exception ex)
                {
                    Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                    CallExit();
                }
            }
        }
        public int GetAccountDigits()
        {
            int sum = 0;
            try
            {
                if (chkNewTree.Checked == true)
                {
                    for (int i = 0; i <= dgvAccountsLevel.RowCount - 1; ++i)
                    {
                        if (dgvAccountsLevel.GetRowCellValue(i, "DigitsNumber").ToString() != "")
                            sum += int.Parse(dgvAccountsLevel.GetRowCellValue(i, "DigitsNumber").ToString());
                    }
                }
                else
                    if (chkDefaultTree.Checked == true)
                {
                    strSQL = "SELECT TOP (1) AccountID FROM Acc_DefaultAccountsTreeTemplate";
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                        sum = dt.Rows[0][0].ToString().Length;
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return sum;



        }
        public void InsertingDeclaringMainAccountsTamplateIntoDeclaringMainAccountsTable(int BranchID)
        {

            strSQL = "SELECT  * FROM  ACC_DEC_MAINACCTAMPLATE ";
            dtDeclarAccounts = Lip.SelectRecord(strSQL);
            for (int i = 0; i <= dtDeclarAccounts.Rows.Count - 1; ++i)
            {

                try
                {
                    Lip.NewFields();

                    Lip.Table = "ACC_DEC_MAINACCOUNTS";
                    if (chkNewTree.Checked == true)
                        Lip.AddNumericField("ACCOUNTID", 0);
                    else
                        Lip.AddNumericField("AccountID", dtDeclarAccounts.Rows[i]["AccountID"].ToString());

                    Lip.AddStringField("ID", Comon.cInt(txtFacilityID.Text).ToString() + i);
                    Lip.AddStringField("THEYEAR", UserInfo.Year);

                    Lip.AddStringField("DECLAREACCOUNTNAME", dtDeclarAccounts.Rows[i]["DeclareAccountName"].ToString());
                    Lip.AddStringField("AccountArbName", dtDeclarAccounts.Rows[i]["AccountArbName"].ToString());
                    Lip.AddStringField("AccountEngName", dtDeclarAccounts.Rows[i]["AccountEngName"].ToString());
                    Lip.AddNumericField("BRANCHID", BranchID);
                    Lip.AddNumericField("FacilityID", Comon.cInt(txtFacilityID.Text).ToString());

                    Lip.AddNumericField("UserID", UserInfo.ID);
                    Lip.AddNumericField("RegDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("RegTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditUserID", UserInfo.ID);
                    Lip.AddNumericField("EditDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddStringField("ComputerInfo", Environment.MachineName);
                    Lip.AddStringField("EditComputerInfo", Environment.MachineName);
                    Lip.ExecuteInsert();
                    XtraMessageBox.Show("تم الحفظ في جدول تعريف الحسابات");

                }
                catch (Exception ex)
                {
                    Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }
            }
        }

        public void InsertingDefaultAccountsTreeTamplateIntoAccountsTree(int BranchID)
        {
            strSQL = "SELECT  * FROM  ACC_DEFAULTACCOUNTSLEVELS ";
            try
            {
                dtDeclarAccounts = Lip.SelectRecord(strSQL);
                if (dtDeclarAccounts.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDeclarAccounts.Rows.Count - 1; ++i)
                    {
                        Lip.NewFields();
                        Lip.Table = "ACC_ACCOUNTSLEVELS";
                        Lip.AddNumericField("THEYEAR", 2020);
                        Lip.AddNumericField("ID", (i+1).ToString());
                        Lip.AddNumericField("FACILITYID", txtFacilityID.Text);
                        Lip.AddNumericField("BRANCHID", BranchID);
                        Lip.AddNumericField("LEVELNUMBER", dtDeclarAccounts.Rows[i]["LevelNumber"].ToString());
                        Lip.AddNumericField("DIGITSNUMBER", dtDeclarAccounts.Rows[i]["DigitsNumber"].ToString());
                        Lip.ExecuteInsert();
                    }
                }

                


                 strSQL = "SELECT * FROM  ACC_DEFAULTACCOUNTTREETEMPLATE ";
                DataTable dtDefaultTree = new DataTable();
                dtDefaultTree = Lip.SelectRecord(strSQL);
                if (dtDefaultTree.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDefaultTree.Rows.Count - 1; ++i)
                    {
                        Lip.NewFields();
                        Lip.Table = "Acc_Accounts";
                        Lip.AddNumericField("BranchID", BranchID);
                        Lip.AddNumericField("FacilityID", txtFacilityID.Text);
                        Lip.AddNumericField("AccountID", dtDefaultTree.Rows[i]["AccountID"].ToString());
                        Lip.AddStringField("ArbName", dtDefaultTree.Rows[i]["ArbName"].ToString());
                        Lip.AddStringField("EngName", dtDefaultTree.Rows[i]["EngName"].ToString());
                        Lip.AddNumericField("PARENTACCOUNTID", dtDefaultTree.Rows[i]["ParentAccountID"].ToString());
                        Lip.AddNumericField("AccountLevelID", dtDefaultTree.Rows[i]["AccountLevelID"].ToString());
                        Lip.AddNumericField("AccountTypeID",dtDefaultTree.Rows[i]["AccountTypeID"].ToString());
                        Lip.AddNumericField("FINALACCTYPE", dtDefaultTree.Rows[i]["FINALACCTYPE"].ToString());
                        Lip.AddNumericField("ACCOUNTNATUREID", dtDefaultTree.Rows[i]["ACCOUNTNATURE"].ToString());

                        Lip.AddNumericField("StopAccount", 0);
                        Lip.AddNumericField("MinLimit", 0);
                        Lip.AddNumericField("MaxLimit", 0);
                        Lip.AddNumericField("USERCREATED", UserInfo.ID);
                        Lip.AddNumericField("DATECREATED", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                        Lip.AddNumericField("DATEUPDATED","0");
                        Lip.AddNumericField("TIMECREATED", Comon.cLong(Lip.GetServerTimeSerial()).ToString());
                        Lip.AddNumericField("USERUPDATED", MySession.UserID);
                        
                        Lip.AddNumericField("TIMEUPDATED", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                        Lip.AddStringField("COMPUTERINFO", (!string.IsNullOrEmpty(MySession.GlobalComputerInfo) ? MySession.GlobalComputerInfo : " "));
                        Lip.AddStringField("EditComputerInfo", (!string.IsNullOrEmpty(MySession.GlobalComputerInfo) ? MySession.GlobalComputerInfo : " "));
                        Lip.AddNumericField("DELETED", 0);
                        Lip.AddNumericField("THEYEAR", UserInfo.Year);
                      


                        Lip.ExecuteInsert();
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                CallExit();
            }
        }
        public void InsertingAdminUserToUsersTable(int BranchID,int ROLEID)
        {
            try
            {
                Users model = new Users();
                model.ArbName = "مدير";
                model.EngName = "Admin";
                model.Password = Security.HashSHA1("123");
                model.IsActive = 1;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.AddByUserID = UserInfo.ID;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.Cancel = 0;
                model.BranchID = BranchID;
                model.FacilityID = Comon.cInt(txtFacilityID.Text);
              
              
                int Result = 1;// UsersManagementDAL.InsertUser(model, IsNewRecord);
                if (Result > 0)
                    XtraMessageBox.Show(" تم حفظ مستخدم جديد برقم    " + Result);

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgErrorSave);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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

                int TempID = Comon.cInt(txtFacilityID.Text);

                cFacility model = new cFacility();
                model.FacilityID = Comon.cInt(txtFacilityID.Text);

                bool Result =true; //FACILITYDAL.DeleteAll(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                MoveRec(model.FacilityID, xMovePrev);
                FillDataGrid();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        protected override void DoRolBack()
        {
            try
            {
                 
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
                GridView.ShowRibbonPrintPreview();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #endregion
        #region Event

        private void dgvAccountsLevel_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;

        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {

                    if (EmailAddressChecker(txtEmail.Text) == false)
                    {
                        txtEmail.Focus();
                        ToolTipController toolTip = new ToolTipController();
                        txtEmail.ToolTipController = toolTip;
                        toolTip.Appearance.BackColor = Color.AntiqueWhite;
                        toolTip.ShowBeak = true;
                        toolTip.CloseOnClick = DefaultBoolean.True;
                        toolTip.ToolTipStyle = ToolTipStyle.Windows7;
                        toolTip.InitialDelay = 500;
                        toolTip.ShowBeak = true;
                        toolTip.Rounded = true;
                        toolTip.ShowShadow = true;
                        toolTip.Appearance.ForeColor = Color.FromArgb(0xFF, 0x6F, 0x6F);
                        toolTip.SetToolTipIconType(txtEmail, ToolTipIconType.Error);
                        toolTip.ToolTipType = ToolTipType.Standard;
                        toolTip.SetTitle(txtEmail, "Error");
                        toolTip.ShowHint(Messages.msgInputShouldBeNumber, ToolTipLocation.TopLeft, txtEmail.PointToScreen(new Point(0, txtEmail.Height)));
                        txtEmail.Properties.Appearance.BorderColor = Color.FromArgb(0xFF, 0x6F, 0x6F);

                    }
                }
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
        private bool EmailAddressChecker(string emailAddress)
        {
            string regExPattern = "^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";
            bool emailAddressMatch = Match.Equals(emailAddress, regExPattern);
            return emailAddressMatch;
        }
        private void txtLevelCounts_Validating(object sender, CancelEventArgs e)
        {
            int RowsCount = Comon.cInt(txtLevelCounts.Text);
            if (RowsCount > 0)
            {
                string strSQL = "SELECT LevelNumber, DigitsNumber FROM Acc_DefaultAccountsLevels";
                DataTable dt = Lip.SelectRecord(strSQL);
                DataTable ddt = new DataTable();
                ddt.Columns.Add("LevelNumber", System.Type.GetType("System.String"));
                ddt.Columns.Add("DigitsNumber", System.Type.GetType("System.String"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= RowsCount - 1; i++)
                    {
                        ddt.Rows.Add();
                        ddt.Rows[i]["LevelNumber"] = i + 1;
                        ddt.Rows[i]["DigitsNumber"] = (i < dt.Rows.Count ? Comon.cInt(dt.Rows[i]["DigitsNumber"]) : 3);
                    }
                    gridControl1.DataSource = ddt;
                }
            }
        }
        private void chkNewTree_CursorChanged(object sender, EventArgs e)
        {

        }
        private void chkDefaultTree_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDefaultTree.Checked == true)
                chkNewTree.Checked = false;
            else
                chkNewTree.Checked = true;


        }

        private void chkNewTree_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewTree.Checked == true)
            {

                chkDefaultTree.Checked = false;
                txtLevelCounts.Visible = true;
                lblLevelCounts.Visible = true;
                gridControl1.Visible = true;

            }

            else
            {
                chkDefaultTree.Checked = true;
                txtLevelCounts.Visible = false;
                lblLevelCounts.Visible = false;
                gridControl1.Visible = false;

            }
        }

        private void gridVBranchesCompany_DoubleClick(object sender, EventArgs e)
        {
            int rowIndex = gridVBranchesCompany.FocusedRowHandle;
            int BranchID = Comon.cInt(gridVBranchesCompany.GetRowCellValue(rowIndex, gridVBranchesCompany.Columns[0].FieldName).ToString());
            frmBranches frm = new frmBranches();
            frm.Show();

          //  frm.txtID.Text = BranchID.ToString();
           // frm.txtID_Validating(null, null);
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
        private void frmBranch_Load(object sender, EventArgs e)
        {
            FillDataGrid();
            FormsPrperties.ColorForm(this);
            DoNew();

            Validations.DoLoadRipon(this, ribbonControl1);
            ribbonControl1.Items[19].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//اضافة من
            ribbonControl1.Items[11].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//اضافة من

        }
        private void txtFacilityID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtFacilityID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtFacilityID.Text));
                    TempUserID = txtFacilityID.Text;
                    ClearFields();
                    txtFacilityID.Text = TempUserID;
                    if (cClass.FoundResult == true)
                    {
                        if (FormView == true)
                            ReadRecord();
                        else
                        {
                         Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                                                       
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
        private void txtFacilityID_EditValueChanged(object sender, EventArgs e)
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
                txtFacilityID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtFacilityID_Validating(null, null);
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
            txtFacilityID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtFacilityID_Validating(null, null);
        }
        #endregion
    }
}

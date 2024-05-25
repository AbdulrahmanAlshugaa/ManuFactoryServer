﻿using System;
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
using Edex.ModelSystem;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraRichEdit.API.Native;
using Edex.DAL.Accounting;
using Edex.DAL;

namespace Edex.AccountsObjects.Codes
{
    public partial class frmBoxes : BaseForm
    {
        public frmBoxes()
        {
            InitializeComponent();

            PrimaryName = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                PrimaryName = "EngName";
            }
              /***************************Edit & Print & Export ****************************/
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            /*****************************************************************************/

            /***************************Initialize Events********************************/         
            this.txtBoxID.Validating += new System.ComponentModel.CancelEventHandler(this.txtBoxID_Validating);
            this.txtBoxID.EditValueChanged += new System.EventHandler(this.txtBoxID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);

            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
            FillCombo.FillComboBoxLookUpEdit(cmbParent, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0   AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)) + " and BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
            cmbParent.EditValue = Comon.cDbl(MySession.GlobalDefaultParentBoxesAccountID);
            FillCombo.FillComboBoxLookUpEdit(cmbParentAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0   AND AccountLevel=" + (Comon.cInt(MySession.GlobalNoOfLevels) - 1) + " and BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
               
        }


        
        #region Declare
        private cBoxes cClass = new cBoxes();

        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;

        private string PrimaryName;
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
        #endregion
        #region Form Event

        #endregion
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
                    
                    int sNode;
                    int SumDigitsCountBeforeSelectedLevel;
                    int DigitsCountForSelectedLevel;
                    long MaxID;
                    string str;
                    string strDigits = "";
                    //ParentAccountID = Lip.GetValue("SELECT AccountID FROM Acc_DeclaringMainAccounts WHERE DeclareAccountName='BoxesAccount'");

                    ParentAccountID = cmbParentAccountID.EditValue + "";
                    AccountLevel = int.Parse(Lip.GetValue("SELECT AccountLevel FROM  Acc_Accounts WHERE AccountID = " + ParentAccountID)) + 1;
                    str = Lip.GetValue("SELECT  MAX(AccountID) FROM  Acc_Accounts Where ParentAccountID=" + ParentAccountID);
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
                    // Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }
               
            }
            return 0;
        }
        /// <summary>
        /// This function to Query to retrieve Box data from the database
        /// </summary>
        public void FillGrid()
        {
            // Query to retrieve Box data from the Boxs table
            strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [إسم الصندوق ] FROM " + cClass.TableName + " WHERE Cancel =0 and BranchID=" + MySession.GlobalBranchID;
            if(UserInfo.Language==iLanguage.English)
                strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Box Name] FROM " + cClass.TableName + " WHERE Cancel =0  and BranchID=" + MySession.GlobalBranchID;

            // Execute the query and save the results in a DataTable
            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);

            // Display the query results in a GridView
            if (dt.Rows.Count > 0)
            {
                GridView.GridControl.DataSource = dt;
                GridView.Columns[0].Width = 50;
                GridView.Columns[1].Width = 100;
            }
        }
        /// <summary>
        /// this function to select id and name Box
        /// </summary>
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            // Select the table and fields required from it
            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, ArbName as [Box Name] FROM " + cClass.TableName
            + " WHERE Cancel =0 and BranchID=" + MySession.GlobalBranchID;

            if (UserInfo.Language == iLanguage.English)
                // Select the table and fields required from it in English
                cls.SQLStr = "SELECT " + cClass.PremaryKey + " as ID, EngName as [Box Name] FROM " + cClass.TableName
                + " WHERE Cancel =0 and BranchID=" + MySession.GlobalBranchID;

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

                txtBoxID.Text = cls.PrimaryKeyValue.ToString();
                txtBoxID_Validating(null, null);
            }

        }

        /// <summary>
        /// This function to read record from cBoxs class to field
        /// </summary>
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    //set values to field
                    txtBoxID.Text = cClass.BoxID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    chkStopAccount.Checked = Comon.cInt(cClass.StopAccount) == 1 ? true : false;
                    txtNotes.Text = cClass.Notes;
                    cmbParentAccountID.EditValue = cClass.ParentAccountID;
                     txtAccountID.Text = cClass.AccountID.ToString();
                    Validations.DoReadRipon(this, ribbonControl1);
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
                txtBoxID.Text = cClass.GetNewID().ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
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
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM " + cClass.TableName + " Where Cancel =0    and BranchID= " + MySession.GlobalBranchID;
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
                MoveRec(Comon.cInt(txtBoxID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtBoxID.Text), xMovePrev);
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
                Acc_Boxes model = new Acc_Boxes();
                model.BoxID = Comon.cInt(txtBoxID.Text);
                model.AccountID = cClass.AccountID;
                //Comon.cLong(txtAccountID.Text);
                if (IsNewRecord == true)
                {
                    model.BoxID = 0;
                    IsNew = true;
                    model.AccountID = GetNewAccountID();
                }
                model.ArbName = txtArbName.Text;
                ArbName = txtArbName.Text;
                EngName = txtEngName.Text;
                model.EngName = txtEngName.Text;
                model.StopAccount = chkStopAccount.Checked == true ? 1 : 0;
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.BranchID = MySession.GlobalBranchID;
                model.FacilityID = UserInfo.FacilityID;
                   
                model.Notes = txtNotes.Text.Trim();
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;
                model.ParentAccountID = Comon.cDbl(cmbParentAccountID.EditValue);
                AccountID = long.Parse(model.AccountID.ToString());
                int StoreID;
                int UpdateID;
                if (IsNewRecord == true)
                    StoreID = Acc_BoxesDAL.Insert_Acc_Boxes(model);
                else
                    UpdateID = Acc_BoxesDAL.Update_Acc_Boxes(model);

                addAccountID();
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                if (IsNewRecord == true)
                    DoNew();
                FillGrid();
                if (Comon.cDbl(this.Text) == 99)
                    this.Close();

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

                     int TempID = Comon.cInt(txtBoxID.Text);

                     Acc_Boxes model = new Acc_Boxes();
                     model.BoxID = Comon.cInt(txtBoxID.Text);
                     model.EditUserID = UserInfo.ID;
                     model.BranchID = MySession.GlobalBranchID;
                     model.FacilityID = UserInfo.FacilityID;
                     model.EditComputerInfo = UserInfo.ComputerInfo;
                     model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                     model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                      
                     {
                         bool Result = Acc_BoxesDAL.DeleteAcc_Boxes(model);
                         bool Result1 = DelAccountID();
                         if (Result == true && Result1 == true)
                             Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                         MoveRec(model.BoxID, xMovePrev);
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
                Messages.MsgAsterisk("لا يمكن الحذف", "لا يمكن حذف الحساب بسبب وجود حركات محاسبية على  الحساب");

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
            //DataTable dtBox = Lip.SelectRecord(strSQL);
            //if (dtBox.Rows.Count > 0)
            //{
            //    for (int i = 0; i <= dtBox.Rows.Count - 1; i++)
            //    {
            //        model.BranchID = Comon.cInt(dtBox.Rows[i]["BRANCHID"].ToString());

            //        strSQL = "Select * from Acc_Accounts where  BRANCHID= " + model.BranchID + " and AccountID=" + txtAccountID.Text ;
            //        DataTable dtAcco = new DataTable();
            //        dtAcco = Lip.SelectRecord(strSQL);
            //        if (dtAcco.Rows.Count > 0)
            //            Acc_AccountsDAL.UpdateAcc_Accounts(model);
            //        else
            //            Acc_AccountsDAL.InsertAcc_Accounts(model);

            //    }
            //}

        }

        //This Function For Delete The Acc_AccountID 
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
        
        #endregion


        #region Event
       
        private void txtBoxID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtBoxID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtBoxID.Text));
                    TempUserID = txtBoxID.Text;
                    ClearFields();//clear all field
                    txtBoxID.Text = TempUserID;
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
        private void txtBoxID_EditValueChanged(object sender, EventArgs e)
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

                txtBoxID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtBoxID_Validating(null, null);

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
            txtBoxID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtBoxID_Validating(null, null);
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
 


      
        //This Event To Save The Box By F9 
        private void frmBoxs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
                DoSave();

        }

        #endregion

        private void frmBoxes_Load(object sender, EventArgs e)
        {
             FillGrid();
            DoNew();

        }

        private void cmbAccountID_EditValueChanged(object sender, EventArgs e)
        {
            txtAccountID.Text = GetNewAccountID().ToString();
        }
        
       
    }
}
using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
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
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using System.Linq;
namespace Edex.StockObjects.Codes
{
    public partial class frmItemsGroups : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        /**************** Declare ************************/
        #region Declare

        private string strSQL;
        private bool IsNewRecord;

        private cItemsGroups cClass = new cItemsGroups();
        public bool IsFromanotherForms=false;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public class MyRecord
        {
            public long ID { get; set; }
            public long ParentID { get; set; }
            public string AcountName { get; set; }

            public MyRecord(long id, long parentID, string _AcountName)
            {
                ID = id;
                ParentID = parentID;
                AcountName = _AcountName;
            }
        }
        #endregion
        /****************Form Event************************/
        #region Form Event
        public frmItemsGroups()
        {
            InitializeComponent();
            strSQL = "ArbName";
            if (UserInfo.Language == iLanguage.English)
            {
                strSQL = "EngName";
            }
            FillCombo.FillComboBox(cmbTypeAcount, "Acc_AccountType", "ID", strSQL);
            FillCombo.FillComboBoxLookUpEdit(cmbAccAccountLevel, "Acc_AccountsLevels", "LevelNumber", "LevelNumber");
             
            this.txtGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating);
            this.txtGroupID.EditValueChanged += new System.EventHandler(this.txtGroupID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
            GetGroupTree();
          
        }
         

        public void GetGroupTree()
        {
            List<Stc_ItemsGroups> ListAccountsTree = new List<Stc_ItemsGroups>();

            ListAccountsTree = STC_ITEMSGROUPS_DAL.GetAllData();
            List<MyRecord> list = new List<MyRecord>();
            if (ListAccountsTree != null)
            {
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    if(UserInfo.Language==iLanguage.Arabic)
                         list.Add(new MyRecord(Comon.cLong(ListAccountsTree[i].GroupID),Comon.cLong( ListAccountsTree[i].ParentAccountID), ListAccountsTree[i].ArbName + "." + ListAccountsTree[i].GroupID.ToString()));
                    else if (UserInfo.Language == iLanguage.English)
                        list.Add(new MyRecord(Comon.cLong(ListAccountsTree[i].GroupID), Comon.cLong(ListAccountsTree[i].ParentAccountID), ListAccountsTree[i].EngName + "." + ListAccountsTree[i].GroupID.ToString()));
               
                }

                list = list.OrderBy(x => x.ParentID.ToString("D" + (x.ParentID.ToString().Length * 2))).ToList();


                treeList1.DataSource = list;
            }
        }
       

        private void frmItemsGroups_Load(object sender, EventArgs e)
        {
            
            DoNew();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "إضافة مجموعة رئيسية " : "New Group");
            
           
        }
        #endregion
        /**********************Function**************************/
        #region Function
       
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المجموعة] FROM " + cClass.TableName + " WHERE Cancel =0  ";

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Group Name] FROM " + cClass.TableName + " WHERE Cancel =0  ";

            ColumnWidth = new int[] { 50, 300 };

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
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                //if (this.ActiveControl.Name == txtGroupID.Name)
                //{
                txtGroupID.Text = cls.PrimaryKeyValue.ToString();
                txtGroupID_Validating(null, null);
                //}
            }

        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    txtGroupID.Text = cClass.GroupID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtNotes.Text = cClass.Notes;
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

         

        private string GetNewAccountID(int FACILITYID, int BranchID, string ParentAccountID)
        {
            string functionReturnValue = "";
            string where = "FACILITYID=" + FACILITYID + " AND BRANCHID=" + BranchID;

            int GlobalAccountsLevelDigits = 3;

            int code = 0;
            long AccountLevel = Comon.cLong(Lip.GetValue("SELECT AccountLevel FROM Stc_ItemsGroups WHERE GroupID = '" + ParentAccountID + "' AND " + where));
            int sNode = Comon.cInt(AccountLevel) + 1;
            int SumDigitsCountBeforeSelectedLevel = 0;
            int DigitsCountForSelectedLevel = 0;
            long MaxID = 0;
            string str = Lip.GetValue("SELECT MAX(GroupID) FROM Stc_ItemsGroups Where ParentAccountID='" + ParentAccountID + "' And " + where);

            if (string.IsNullOrEmpty(str) && (sNode >= 2))
            {
                GlobalAccountsLevelDigits = 4;
                functionReturnValue = ParentAccountID + (Comon.cLong(str) + 1).ToString().PadLeft(GlobalAccountsLevelDigits, '0');
            }
            else if (string.IsNullOrEmpty(str))
            {
                GlobalAccountsLevelDigits = 2;
                functionReturnValue = ParentAccountID +(Comon.cLong(str) + 1).ToString().PadLeft(GlobalAccountsLevelDigits, '0');
            }
            else
            {
                if (sNode >= 2)
                  GlobalAccountsLevelDigits = 4;
                else
                    GlobalAccountsLevelDigits = 2;
                functionReturnValue = "0" + (Comon.cLong(str) + 1).ToString().PadLeft(GlobalAccountsLevelDigits, '0');
            }

            return functionReturnValue;
        }

        public void ClearFields()
        {
            try
            {
                var GroupID = txtGroupID.Text;
                txtGroupID.Text = GetNewAccountID(UserInfo.FacilityID,UserInfo.BRANCHID,  txtGroupID.Text).ToString();

                txtParentID.Text = GroupID;
                cmbAccAccountLevel.EditValue = Comon.cInt(cmbAccAccountLevel.EditValue) + 1;

                txtArbName.Text = "";
                txtEngName.Text = "";
                txtNotes.Text = "";

                DataTable dtLevel = new DataTable();
                dtLevel = Lip.SelectRecord("Select Max(LevelNumber) AS LevelNumber  from Acc_AccountsLevels where BranchID=" + UserInfo.BRANCHID);
                int MaxLevel = Comon.cInt(dtLevel.Rows[0]["LevelNumber"].ToString());
                if (MaxLevel == Comon.cInt(cmbAccAccountLevel.EditValue))
                    cmbTypeAcount.ItemIndex = 1;
                else
                    cmbTypeAcount.ItemIndex = 0;
                txtArbName.Focus();
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
                EnabledControl(true);
                IsNewRecord = true;
                ClearFields();
                txtArbName.Focus();
                ribbonControl1.Items[19].Enabled = true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
          protected override void DoAddFrom()
        {
            try
            {
                string where = "FACILITYID=" + MySession.GlobalFacilityID+ " AND BRANCHID=" + MySession.GlobalBranchID;
                long str =Comon.cLong( Lip.GetValue("SELECT MAX(GroupID) FROM Stc_ItemsGroups Where ParentAccountID=0 And " + where));        
                txtGroupID.Text ="0"+(Comon.cLong( str)+1);
                txtParentID.Text = "0";
                EnabledControl(true);
                cmbAccAccountLevel.EditValue = Comon.cInt(0);
                txtArbName.Text = "";
                txtEngName.Text = "";
                txtNotes.Text = "";
                cmbTypeAcount.ItemIndex = 0;
                txtArbName.Focus();
                IsNewRecord = true;
            }
            catch { }
          }
        protected override void DoEdit()
        {
            Validations.DoEditRipon(this, ribbonControl1);
            EnabledControl(true);
        }
        private void EnabledControl(bool Value)
        {
            // Loop through all controls in the form
            foreach (Control item in this.Controls)
            {
                // For TextEdit controls that don't have "AccountID" or "AccountName" in their name,
                // and don't have "lbl" and "Name" in their name
                if (item is TextEdit && ((!(item.Name.Contains("AccountID"))) && (!(item.Name.Contains("AccountName")))))
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {
                        // Set their Enabled property and specified AppearanceDisabled foreground and background color based on the Value parameter
                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    }
                }
                // For TextEdit controls that have "AccountID" or "AccountName" in their name
                else if (item is TextEdit && (((item.Name.Contains("AccountID"))) || ((item.Name.Contains("AccountName")))))
                {
                    // Set their Enabled property and specified AppearanceDisabled foreground and background color based on the Value parameter
                    item.Enabled = Value;
                    ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                    ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    if (Value)
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                }
                // For SimpleButton controls that have "btn" and "Search" in their name
                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    // Set their Enabled property based on the Value parameter
                    ((SimpleButton)item).Enabled = Value;
                }
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
                MoveRec(Comon.cInt(txtGroupID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtGroupID.Text), xMovePrev);
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
                ribbonControl1.Items[19].Enabled = true;

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void save() {

            Stc_ItemsGroups model = new Stc_ItemsGroups();
            model.GroupID = txtGroupID.Text;
            model.ArbName = txtArbName.Text;
            model.EngName = txtEngName.Text;
            model.Notes = txtGroupID.Text;
            model.ParentAccountID =  txtParentID.Text;
            model.MaxLimit = Comon.cLong(txtNotes.Text);
            model.MinLimit = 0;
            if (chkStopAccount.Checked == false)
                model.StopAccount = 0;
            if (chkStopAccount.Checked == true)
                model.StopAccount = 1;
            model.AccountLevel = Comon.cInt(cmbAccAccountLevel.EditValue);
            model.AccountTypeID = Comon.cInt(cmbTypeAcount.EditValue);
            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.BranchID = UserInfo.BRANCHID;
            model.FacilityID = UserInfo.FacilityID;

            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;

            model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
            model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());

            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.Cancel = 0;
            int GroupID;
            GroupID = STC_ITEMSGROUPS_DAL.InsertStc_Groups(model, IsNewRecord);
            if (IsFromanotherForms == false)
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                GetGroupTree();
                Validations.DoReadRipon(this, ribbonControl1);
                if (IsNewRecord == true)
                    DoNew();   
            }
          


        }
        protected override void DoPrint()
        {

            try
            {
              //  GridView.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoDelete()
        {
            try
            {
                int isTRansParent = Comon.cInt(Lip.GetValue(" select  ParentAccountID from Stc_ItemsGroups where ParentAccountID='" +txtGroupID.Text + "' and cancel=0"));
                int isTRans = Comon.cInt(Lip.GetValue(" select dbo.[GroupItemID](" + Comon.cDbl(txtGroupID.Text) + ")"));
                if (isTRansParent > 0)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, "يوجد مجموعات تنتمي الى هذه المجموعة لذلك لا يمكن حذفها ");
                    return;
                }
                else
                if (isTRans > 0)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, "يوجد اصناف تنتمي الى هذه المجموعة لذلك لا يمكن حذفها ");
                    return;
                }
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


                Stc_ItemsGroups model = new Stc_ItemsGroups();
              
                model.GroupID =  txtGroupID.Text;

                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.Cancel = 0;

                bool Result = STC_ITEMSGROUPS_DAL.DeleteStc_Groups(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                MoveRec(Comon.cLong( model.GroupID), xMovePrev);


                GetGroupTree();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        /**********************Event**************************/
        #region Event
        public void txtGroupID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT * FROM Stc_ItemsGroups WHERE BranchID = " + 1 + "   AND (GroupID ='" + txtGroupID.Text + "') and Cancel=0";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                DataTable dt = Lip.SelectRecord(strSQL);

                if (dt.Rows.Count > 0)
                {
                    txtGroupID.Text = dt.Rows[0]["GroupID"].ToString();
                    txtArbName.Text = dt.Rows[0]["ArbName"].ToString();
                    txtEngName.Text = dt.Rows[0]["EngName"].ToString();
                    txtParentID.Text = dt.Rows[0]["ParentAccountID"].ToString();
                    //txtNotes.Text = dt.Rows[0][""].ToString();
                    cmbTypeAcount.ItemIndex = Comon.cInt(dt.Rows[0]["AccountTypeID"].ToString());
                    // cmbTypeAcount.EditValue = dt.Rows[0]["AccountTypeID"].ToString();
                    cmbAccAccountLevel.ItemIndex = Comon.cInt(dt.Rows[0]["AccountLevel"].ToString());

                    if (Comon.cInt(dt.Rows[0]["StopAccount"].ToString()) == 1)
                        chkStopAccount.Checked = true;
                    else
                        chkStopAccount.Checked = false;
                    TreeListNode node = treeList1.FindNodeByFieldValue("ID", txtGroupID.Text.Trim());
                    treeList1.SetFocusedNode(node);


                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            string[] AcountNameAndID;

            if (e.Node == null) return;
            AcountNameAndID = e.Node.GetValue(0).ToString().Split('.');
           
            string AcountID = AcountNameAndID[1];
            // writ function to get acount data and disbbly it in textbox

            Stc_ItemsGroups Accounts = new Stc_ItemsGroups();
            Accounts = STC_ITEMSGROUPS_DAL.GetDataByID(AcountID, 1, UserInfo.FacilityID);
            if (Accounts != null)
            {
                txtGroupID.Text = Accounts.GroupID.ToString();
                txtArbName.Text = Accounts.ArbName;
                txtEngName.Text = Accounts.EngName;
                txtNotes.Text = Accounts.MaxLimit.ToString();
                txtParentID.Text = Accounts.ParentAccountID.ToString();
                if (Accounts.StopAccount == 0)
                    chkStopAccount.Checked = false;
                else
                    chkStopAccount.Checked = true;

                cmbTypeAcount.EditValue = Accounts.AccountTypeID;
                cmbAccAccountLevel.EditValue = Accounts.AccountLevel;
                if (cmbTypeAcount.EditValue.ToString() == "1")
                {
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                    //ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;
                }
                else
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = true;
                IsNewRecord = false;

                //Validations.DoReadRipon(this, ribbonControl1);
                EnabledControl(true);

            }
         
        }
        private void txtGroupID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;
        }
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
        #endregion

        private void txtGroupID_EditValueChanged_1(object sender, EventArgs e)
        {

        }


    }
}

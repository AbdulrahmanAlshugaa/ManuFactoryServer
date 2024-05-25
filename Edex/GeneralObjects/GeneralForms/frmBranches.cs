using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.DAL.Configuration;
using Edex.DAL.UsersManagement;
using Edex.Model;
using Edex.Model.Language;
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
using Edex.Reports;
//using static Edex.GeneralObjects.GeneralClasses.FillCombo;
using Edex.AccountsObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.SalesAndSaleObjects.Transactions;
using Edex.SalesAndSaleObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.AccountsObjects.Transactions;
using Edex.StockObjects.Transactions;
using Edex.StockObjects.Codes;
using System.Data.SqlClient;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmBranches : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        Control FocusedControl;
        private cBranches cClass = new cBranches();
        private DataTable dt = new DataTable();
        private DataTable dtDeclarAccounts = new DataTable();
        private DataTable dtNewTree = new DataTable();
        cStarting defgh;
        BindingList<cStarting> defg = new BindingList<cStarting>();
        public int LevelDigits;
        int rowIndex;
        public readonly string TableName = "Branches";
        public readonly string PremaryKey = "BranchID";
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public long rowCount = 0;
        private string strSQL;
        private bool IsNewRecord;

        #endregion
        #region Form Event
        public  frmBranches()
        {
            InitializeComponent();

            /***************************Edit & Print & Export ****************************/

            /*****************************************************************************/
            /*****************************************************************************/
            this.txtEmail.EditValueChanged += new System.EventHandler(this.txtEmail_EditValueChanged);
            this.txtEmail.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
            this.txtBranchID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtBranchID.EditValueChanged += new System.EventHandler(this.txtCustomerID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
        }

        #endregion
        #region Function

        public void FillGrid()
        {

            strSQL = "SELECT  BranchID   as الرقم, ArbName as [اسم الفرع] FROM  Branches WHERE Cancel =0  and FacilityID= " + UserInfo.FacilityID;

            if (UserInfo.Language == iLanguage.English)

                strSQL = "SELECT   BranchID  as ID, EngName as [Branch Name] FROM Branches WHERE Cancel =0  and FacilityID= " + UserInfo.FacilityID;
            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            GridView.GridControl.DataSource = dt;
            GridView.Columns[0].Width = 50;
            GridView.Columns[1].Width = 100;

        }



        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                txtBranchID.Text = cls.PrimaryKeyValue.ToString();
                txtCustomerID_Validating(null, null);
            }

        }


        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    txtBranchID.Text = cClass.BranchID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtTel.Text = cClass.Tel;
                    txtAddress.Text = cClass.Address;
                    txtFax.Text = cClass.Fax;
                    txtEmail.Text = cClass.Email;
                    lblNoOfAccountLevels.Text = getNewLevel().ToString();
                    cmbIsActive.EditValue = cClass.IsActive;
                    txtLevelCounts.Text = GetNoOfLevels().ToString();

                }



            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public void ClearFields()
        {
            try
            {
                txtBranchID.Text = cClass.GetNewID().ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtTel.Text = " ";
                txtAddress.Text = " ";
                txtFax.Text = " ";
                txtEmail.Text = " ";
                cmbIsActive.EditValue = 1;
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
                    txtLevelCounts.Visible = false;
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
            try
            {
                #region If
                if (FormView == true)
                {
                    strSQL = "SELECT TOP 1 * FROM " + TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + PremaryKey + " DESC";
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
                Validations.EnabledControl(this, true);
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
                MoveRec(Comon.cInt(txtBranchID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtBranchID.Text), xMovePrev);
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
        protected override void Find()
        {

            FocusedControl = GetIndexFocusedControl();
            CSearch cls = new CSearch();

            try
            {
                if (FocusedControl == txtBranchID)
                {
                    cls = Lovs.FacilitiesList(this);
                    GetSelectedSearch(cls);
                }
                if (FocusedControl == txtBranchID)
                {
                    Lovs.Find(cClass.TableName, cClass.PremaryKey, this);
                }
            }
            catch
            { }
        }
        Control GetIndexFocusedControl()
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
                return c.Parent;
            }

            return c;
        }
        protected override void GetSelectedSearch(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl == txtBranchID)
                {
                    txtBranchID.Text = cls.PrimaryKeyValue.ToString();
                    txtBranchID_Validating(null, null);
                }
                SendKeys.Send("{tab}");
                SendKeys.Send("{tab}");

            }
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

                if (IsNewRecord)
                    simpleButton1_Click(null, null);
                if (chkNewTree.Checked == true && Comon.cInt(txtLevelCounts.Text) <= 1)
                {
                    Messages.MsgError(Messages.TitleError, "الرجاء ادخال عدد المستويات في خانة عدد المستويات للشجرة المخصصة");
                    txtLevelCounts.Focus();
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                strSQL = "Select Top 1 * From Branches WHERE Cancel=0";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count == 0)
                    FirstBranch = true;

                {
                    
                    List<UserOtherPermissions> listUserOtherPermissions = new List<UserOtherPermissions>();
                    UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                    UserOtherPermissions.UserID = UserInfo.ID;
                    UserOtherPermissions.BranchID = Comon.cInt(txtBranchID.Text);
                    UserOtherPermissions.FacilityID = UserInfo.FacilityID;
                    UserOtherPermissions.OtherPermissionName ="NoOfLevels";
                    UserOtherPermissions.OtherPermissionValue = txtLevelCounts.Text;
                    UserOtherPermissions.OtherPermissionIndex = Comon.cInt(txtLevelCounts.Text);
                    listUserOtherPermissions.Add(UserOtherPermissions);
                    int Result = UsersManagementDAL.frmInsertUserOtherPermissions(UserInfo.ID, Comon.cInt(txtBranchID.Text), listUserOtherPermissions);
                }


                if (IsNewRecord == true)
                {
                    InsertingDataToBranchsTable();

                    int UserID = InsertingAdminUserToUsersTable();

                    InsertingDeclaringMainAccountsTamplateIntoDeclaringMainAccountsTable();

                    // InsertingIncomeAccountsTamplateIntoIncomeAccountsTable();

                    InsertingStartNumberingTemplateIntoStartNumberingTable();
                
                    if (chkNewTree.Checked == true)
                        InsertingNewAccountsTreeTamplateIntoAccountsTree();
                    //' في حالة استخدام الشجرة الافتراضيه
                    else
                        if (chkDefaultTree.Checked == true)
                        InsertingDefaultAccountsTreeTamplateIntoAccountsTree();


                    GetPermissionsForFirstUserForBranch(UserID);

                    ViewPermissionMenuForBranch(UserID);
                    SplashScreenManager.CloseForm(false);

                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                }
                else
                {
                    Branches model = new Branches();
                    model.BranchID = Comon.cInt(txtBranchID.Text);
                    model.ArbName = txtArbName.Text;
                    model.EngName = txtEngName.Text;
                    model.Address = txtAddress.Text;
                    model.Email = txtEmail.Text;
                    model.Tel = txtTel.Text;
                    model.IsActive = Comon.cInt(cmbIsActive.EditValue);
                    model.Fax = txtFax.Text;
                    model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                    model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                    model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                    model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                    model.Cancel = 0;
                    model.UserID = UserInfo.ID;
                    model.EditUserID = UserInfo.ID;
                    model.ComputerInfo = UserInfo.ComputerInfo;
                    model.EditComputerInfo = UserInfo.ComputerInfo;
                    model.FacilityID = UserInfo.FacilityID;

                    if (chkNewTree.Checked == true)
                        InsertingNewAccountsTreeTamplateIntoAccountsTree();
                    
                    bool Result = BranchesDAL.UpdateBranches(model);
                    SplashScreenManager.CloseForm(false);

                    if (Result == true)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                        Messages.MsgWarning(Messages.TitleWorning, "سيتم إعادة تشغيل النظام.. ");
                         
                        {
                            Application.Restart();
                            Environment.Exit(0);
                        }
                    }

                }

                if (IsNewRecord == true)
                    DoNew();
                FillGrid();

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
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }

                int TempID = Comon.cInt(txtBranchID.Text);



                Branches model = new Branches();
                model.BranchID = Comon.cInt(txtBranchID.Text);
                model.FacilityID = MySession.GlobalFacilityID;

                model.EditUserID = UserInfo.ID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                bool Result = BranchesDAL.DeleteBranches(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                MoveRec(model.BranchID, xMovePrev);
                FillGrid();




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
                bool IncludeHeader = true;
                XtraReport rptCompanyHeader = new rptCompanyHeaderArb();
                rptCompanyHeader = (UserInfo.Language == iLanguage.English ? new rptCompanyHeaderEng() : rptCompanyHeader);

                /******************** Report Body *************************/
                //dynamic rptForm = new rptPurchaseInvoiceArb();
                //rptForm = (UserInfo.Language == iLanguage.English ? new rptPurchaseInvoiceEng() : rptForm);

                //var dataTable = new dsReports.rptSizingUnitDataTable();
                //var query = STC_STORES_DAL.GetAllData(1, 1);

                ///********************** Master *****************************/
                //rptForm.Parameters["parameter1"].Value = "1455";


                /********************** Details ****************************/
                //foreach (var item in query)
                //{
                //    var row = dataTable.NewRow();
                //    row["ArbName"] = item.ArbName;
                //    row["EngName"] = item.EngName;
                //    row["StoreID"] = item.StoreID;
                //    dataTable.Rows.Add(row);

                //}
                //rptForm.DataSource = dataTable;
                //rptForm.DataMember = "rptSizingUnitDataTable";

                ///******************** Report Footer *************************/


                ///******************** Report Binding ************************/
                //rptForm.RequestParameters = false;
                //rptForm.subRptCompanyHeader.Visible = IncludeHeader;
                //rptForm.subRptCompanyHeader.ReportSource = rptCompanyHeader;
                //rptForm.CreateDocument();


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        #endregion


        #region Event
        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtBranchID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtBranchID.Text));
                    TempUserID = txtBranchID.Text;
                    ClearFields();
                    txtBranchID.Text = TempUserID;
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


        private void txtCustomerID_EditValueChanged(object sender, EventArgs e)
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

                txtBranchID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtCustomerID_Validating(null, null);

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
            txtBranchID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtCustomerID_Validating(null, null);
        }

        #endregion

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
            FillComboBox(cmbIsActive, "YesNo", "ID", (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName"));
            FillGrid();
            defg = new BindingList<cStarting>();
            gridControl1.DataSource = defg;


        }
        public void FillComboBox(DevExpress.XtraEditors.LookUpEdit cmb, string Tablename, string Code, string Name, string OrderByField = "")
        {
            string strSQL = "SELECT " + Code + " AS [الرقم]," + Name + "  AS [الاسم] FROM " + Tablename;
            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;
            cmb.Properties.DataSource = Lip.SelectRecord(strSQL).DefaultView;
            cmb.Properties.DisplayMember = "الاسم";
            cmb.Properties.ValueMember = "الرقم";
        }

        public int getNewLevel()
        {
            strSQL = "SELECT MAX(LevelNumber) FROM Acc_AccountsLevels WHERE BranchID =" + txtBranchID.Text;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
                return Comon.cInt(dt.Rows[0][0] == DBNull.Value ? 0 : dt.Rows[0][0]);
            else
                return -1;
        }

        /***************************************************************************88888888888888888*/
        public void InsertingDeclaringMainAccountsTamplateIntoDeclaringMainAccountsTable()
        {

            strSQL = "SELECT  * FROM  Acc_DeclaringMainAccountsTamplate ";
            dtDeclarAccounts = Lip.SelectRecord(strSQL);

            strSQL = "delete    FROM  Acc_DeclaringMainAccounts where  BranchID=" + txtBranchID.Text;
            Lip.ExecututeSQL(strSQL);

            for (int i = 0; i <= dtDeclarAccounts.Rows.Count - 1; ++i)
            {

                try
                {
                    Lip.NewFields();

                    Lip.Table = "Acc_DeclaringMainAccounts";
                    if (chkNewTree.Checked == true)
                        Lip.AddNumericField("AccountID", 0);
                    else
                        Lip.AddNumericField("AccountID", dtDeclarAccounts.Rows[i]["AccountID"].ToString());

                    Lip.AddStringField("DeclareAccountName", dtDeclarAccounts.Rows[i]["DeclareAccountName"].ToString());
                    Lip.AddStringField("AccountArbName", dtDeclarAccounts.Rows[i]["AccountArbName"].ToString());
                    Lip.AddStringField("AccountEngName", dtDeclarAccounts.Rows[i]["AccountEngName"].ToString());
                    Lip.AddNumericField("BranchID", txtBranchID.Text);

                    Lip.AddNumericField("UserID", UserInfo.ID);
                    Lip.AddNumericField("RegDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("RegTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditUserID", UserInfo.ID);
                    Lip.AddNumericField("EditDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddStringField("ComputerInfo", "mubark");
                    Lip.AddStringField("EditComputerInfo", "mubark");
                    Lip.ExecuteInsert();


                }
                catch (Exception ex)
                {
                    Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }



            }




        }
        public void InsertingStartNumberingTemplateIntoStartNumberingTable()
        {
            strSQL = "SELECT  * FROM  StartNumberingTemplate ";
            dtDeclarAccounts = Lip.SelectRecord(strSQL);

            strSQL = "delete    FROM  StartNumbering where  BranchID=" + txtBranchID.Text;
            Lip.ExecututeSQL(strSQL);

            for (int i = 0; i <= dtDeclarAccounts.Rows.Count - 1; ++i)
            {

                try
                {
                    Lip.NewFields();
                    Lip.Table = "StartNumbering";

                    Lip.AddStringField("FormName", dtDeclarAccounts.Rows[i]["FormName"].ToString());
                    Lip.AddStringField("ArbCaption", dtDeclarAccounts.Rows[i]["ArbCaption"].ToString());
                    Lip.AddStringField("EngCaption", dtDeclarAccounts.Rows[i]["EngCaption"].ToString());
                    Lip.AddNumericField("StartFrom", dtDeclarAccounts.Rows[i]["StartFrom"].ToString());
                    Lip.AddNumericField("AutoNumber", dtDeclarAccounts.Rows[i]["AutoNumber"].ToString());
                    Lip.AddNumericField("BranchID", txtBranchID.Text);

                    Lip.ExecuteInsert();


                }
                catch (Exception ex)
                {
                    Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                    CallExit();
                }


            }



        }
        public void InsertingIncomeAccountsTamplateIntoIncomeAccountsTable()
        {
            strSQL = "SELECT  * FROM  Acc_DeclaringIncomeAccountsTamplate ";
            dtDeclarAccounts = Lip.SelectRecord(strSQL);

            for (int i = 0; i <= dtDeclarAccounts.Rows.Count - 1; ++i)
            {

                try
                {
                    Lip.NewFields();
                    Lip.Table = "Acc_DeclaringIncomeAccounts";
                    if (chkNewTree.Checked == true)
                        Lip.AddNumericField("AccountID", 0);
                    else
                        Lip.AddNumericField("AccountID", dtDeclarAccounts.Rows[i]["AccountID"].ToString());

                    Lip.AddStringField("DeclareAccountName", dtDeclarAccounts.Rows[i]["DeclareAccountName"].ToString());
                    Lip.AddStringField("AccountArbName", dtDeclarAccounts.Rows[i]["AccountArbName"].ToString());
                    Lip.AddStringField("AccountEngName", dtDeclarAccounts.Rows[i]["AccountEngName"].ToString());

                    Lip.AddNumericField("BranchID", txtBranchID.Text);
                    Lip.AddNumericField("UserID", UserInfo.ID);
                    Lip.AddNumericField("RegDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("RegTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditUserID", UserInfo.ID);
                    Lip.AddNumericField("EditDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddNumericField("EditTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                    Lip.AddStringField("ComputerInfo", "mm");
                    Lip.AddStringField("EditComputerInfo", "mubark");
                    Lip.ExecuteInsert();



                }
                catch (Exception ex)
                {
                    Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                    CallExit();
                }


            }

        }
        public void InsertingNewAccountsTreeTamplateIntoAccountsTree()
        {

            for (int i = 0; i <= dgvAccountsLevel.RowCount - 1; ++i)
            {
                Lip.NewFields();
                Lip.Table = "Acc_AccountsLevels";
                Lip.AddNumericField("BranchID", txtBranchID.Text);
                Lip.AddNumericField("FacilityID", UserInfo.FacilityID);
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
                    Lip.AddNumericField("AccountID", dtNewTree.Rows[i]["AccountID"].ToString());
                    Lip.AddNumericField("BranchID", txtBranchID.Text);
                    Lip.AddNumericField("FacilityID", UserInfo.FacilityID);
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
        public void InsertingDefaultAccountsTreeTamplateIntoAccountsTree()
        {
            strSQL = "SELECT  * FROM  Acc_DefaultAccountsLevels ";
            try
            {
                dtDeclarAccounts = Lip.SelectRecord(strSQL);
                if (dtDeclarAccounts.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDeclarAccounts.Rows.Count - 1; ++i)
                    {
                        Lip.NewFields();
                        Lip.Table = "Acc_AccountsLevels";
                        Lip.AddNumericField("BranchID", txtBranchID.Text);
                        Lip.AddNumericField("FacilityID", UserInfo.FacilityID);
                        Lip.AddNumericField("LevelNumber", dtDeclarAccounts.Rows[i]["LevelNumber"].ToString());
                        Lip.AddNumericField("DigitsNumber", dtDeclarAccounts.Rows[i]["DigitsNumber"].ToString());
                   
                        Lip.ExecuteInsert();
                    }
                }
                strSQL = "SELECT * FROM  Acc_DefaultAccountsTreeTemplate ";
                DataTable dtDefaultTree = new DataTable();
                dtDefaultTree = Lip.SelectRecord(strSQL);
                if (dtDefaultTree.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDefaultTree.Rows.Count - 1; ++i)
                    {


                        Lip.NewFields();
                        Lip.Table = "Acc_Accounts";

                        Lip.AddNumericField("BranchID", txtBranchID.Text);
                        Lip.AddNumericField("FacilityID", UserInfo.FacilityID);

                        Lip.AddNumericField("AccountID", dtDefaultTree.Rows[i]["AccountID"].ToString());
                        Lip.AddStringField("ArbName", dtDefaultTree.Rows[i]["ArbName"].ToString());

                        Lip.AddStringField("EngName", dtDefaultTree.Rows[i]["EngName"].ToString());
                        Lip.AddNumericField("ParentAccountID", dtDefaultTree.Rows[i]["ParentAccountID"].ToString());

                        Lip.AddNumericField("AccountLevel", dtDefaultTree.Rows[i]["AccountLevel"].ToString());
                        Lip.AddNumericField("AccountTypeID", dtDefaultTree.Rows[i]["AccountTypeID"].ToString());
                        Lip.AddNumericField("StopAccount", 0);
                        Lip.AddNumericField("MinLimit", 0);
                        Lip.AddNumericField("MaxLimit", 0);
                        Lip.AddNumericField("UserID", UserInfo.ID);

                        Lip.AddNumericField("RegDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                        Lip.AddNumericField("RegTime", Comon.cLong(Lip.GetServerTimeSerial()).ToString());
                        Lip.AddNumericField("EditUserID", MySession.UserID);
                        Lip.AddNumericField("EditDate", Comon.cLong(Lip.GetServerDateSerial()).ToString());

                        Lip.AddNumericField("EditTime", Comon.cLong(Lip.GetServerDateSerial()).ToString());
                        Lip.AddStringField("ComputerInfo", (!string.IsNullOrEmpty(MySession.GlobalComputerInfo) ? MySession.GlobalComputerInfo : " "));
                        Lip.AddStringField("EditComputerInfo", (!string.IsNullOrEmpty(MySession.GlobalComputerInfo) ? MySession.GlobalComputerInfo : " "));

                        Lip.AddNumericField("Cancel", 0);

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
        public int InsertingAdminUserToUsersTable()
        {
            try
            {

                Users model = new Users();
                model.ArbName = "مدير";
                model.EngName = "Admin";
                model.Password = "40BD001563085FC35165329EA1FF5C5ECBDBBEEF";
                model.IsActive = 1;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.AddByUserID = UserInfo.ID;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.Cancel = 0;
                model.BranchID = Comon.cInt(txtBranchID.Text);
                model.FacilityID = MySession.GlobalFacilityID;
                int Result = UsersManagementDAL.InsertUser(model);
                int t = 0;

                if (Result > 0)
                    return Result;
                else
                    return 0;



            }
            catch (Exception ex)
            {
                return 0;
            }


        }
        public void InsertingDataToBranchsTable()
        {
            try
            {
                Branches model = new Branches();
                model.BranchID = Comon.cInt(txtBranchID.Text);
                model.ArbName = txtArbName.Text;
                model.EngName = txtEngName.Text;
                model.Address = txtAddress.Text;
                model.Email = txtEmail.Text;
                model.Tel = txtTel.Text;
                model.IsActive = Comon.cInt(cmbIsActive.EditValue);

                model.Fax = txtFax.Text;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.FacilityID = UserInfo.FacilityID;
                int StoreID = BranchesDAL.InsertBranches(model);
                //   XtraMessageBox.Show("تم الحفظ في جدول الفروع");

            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


        }
        public void ViewPermissionMenuForBranch(int UserID)
        {
            try
            {
                Lip.NewFields();
                Lip.Table = "UserMenusPermissions";
                Lip.AddNumericField("BranchID", txtBranchID.Text);
                Lip.AddNumericField("UserID", UserID);
                Lip.AddStringField("MenuName", "PermissionsScreens");
                Lip.AddNumericField("MenuView", 1);

                Lip.ExecuteInsert();



            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


        }
        public void GetPermissionsForFirstUserForBranch(int UID)
        {
            try
            {
                strSQL = "delete    FROM  UserFormsPermissions where UserID=" + UID+" and  BranchID="+ Comon.cInt( txtBranchID.Text);
                Lip.ExecututeSQL(strSQL);

                Lip.NewFields();
                Lip.Table = "UserFormsPermissions";
                Lip.AddNumericField("BranchID", txtBranchID.Text);
                Lip.AddNumericField("FacilityID", UserInfo.FacilityID);

                Lip.AddNumericField("UserID", UID);
                Lip.AddStringField("FormName", "frmUserPermissions");
                Lip.AddNumericField("FormView", 1);
                Lip.AddNumericField("FormAdd", 1);
                Lip.AddNumericField("FormDelete", 1);
                Lip.AddNumericField("FormUpdate", 1);

                Lip.AddNumericField("DaysAllowedForEdit", 0);
                Lip.ExecuteInsert();





            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
        public int GetNoOfLevels()
        {
            try
            {

                string strSQL = "SELECT MAX(LevelNumber) FROM Acc_AccountsLevels WHERE BranchID =" + txtBranchID.Text;
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


        private void dgvAccountsLevel_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;

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
                txtLevelCounts.Visible = true;
                gridControl1.Visible = true;
                this.Height = 702;
            }


            else
            {

                chkDefaultTree.Checked = true;
                txtLevelCounts.Visible = false;
                txtLevelCounts.Visible = false;
                gridControl1.Visible = false;
                this.Height = 479;

            }

        }

        private void txtLevelCounts_Validating(object sender, CancelEventArgs e)
        {
            int RowsCount = Comon.cInt(txtLevelCounts.Text);
            if (RowsCount > 0)
            {
                string strSQL = "SELECT LevelNumber, DigitsNumber FROM Acc_DefaultAccountsLevels ";
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
                        ddt.Rows[i]["DigitsNumber"] = (i  < dt.Rows.Count ? Comon.cInt(dt.Rows[i]["DigitsNumber"]) : 3);
                    }
                    gridControl1.DataSource = ddt;
                }
            }
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

        private void txtBranchID_Validating(object sender, CancelEventArgs e)
        {

        }
        private void simpleButton19_Click(object sender, EventArgs e)
        {
            ProgressBar.Value = 1;
            ProgressBar.Maximum = 19;
            ProgressBar.Minimum = 1;
            ProgressBar.Visible = true;
            label3.Text = " جاري الترحيل ";
            Application.DoEvents();
            bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, "هل تريد بالتاكيد اعادة الترحيل؟");
            if (!Yes)
                return;
            //حذف
            try
            {
                strSQL = "drop table Acc_VariousVoucherMachinDetails";
                Lip.ExecututeSQL(strSQL);
                strSQL = "drop table Acc_VariousVoucherMachinMaster";
                Lip.ExecututeSQL(strSQL);

                strSQL = "drop Procedure Acc_VariousVoucherMachin_SP";
                Lip.ExecututeSQL(strSQL);
            }
            catch  
            {

                
            }
             //انشاء الجدول 
            try
            {

                strSQL = @"CREATE TABLE [dbo].[Acc_VariousVoucherMachinDetails](
	            [ID] [int] IDENTITY(1,1) NOT NULL,
	            [VoucherID] [int] NOT NULL,
	            [BranchID] [int] NOT NULL,
	            [Debit] [float] NOT NULL,
	            [Credit] [float] NOT NULL,
	            [CreditGold] [float] NULL CONSTRAINT [DF_Acc_VariousVoucherMachinDetails_CreditGold]  DEFAULT ((0)),
	            [DebitGold] [float] NULL CONSTRAINT [DF_Acc_VariousVoucherMachinDetails_DebitGold]  DEFAULT ((0)),
	            [AccountID] [float] NOT NULL,
	            [Declaration] [nvarchar](200) NOT NULL,
	            [CostCenterID] [int] NULL,
	            [AccountAssest] [float] NOT NULL CONSTRAINT [DF_Acc_VariousVoucherMachinDetails_AccountAssest]  DEFAULT ((0)),
	            [FacilityID] [int] NULL,
	            [DocumentType] [int] NULL CONSTRAINT [DF_Acc_VariousVoucherMachinDetails_DocumentType]  DEFAULT ((0)),
	            [DIAMOND_W] [float] NULL,
	            [STONE_W] [float] NULL,
	            [BAGET_W] [float] NULL,
                [DebitDiamond] [float] NULL CONSTRAINT [DF_Acc_VariousVoucherMachinDetails_DebitDiamond]  DEFAULT ((0)),
	            [CreditDiamond] [float] NULL CONSTRAINT [DF_Acc_VariousVoucherMachinDetails_CreditDiamond]  DEFAULT ((0)),
                [DebitMatirial] [float] NULL,
	            [CreditMatirial] [float] NULL,
	            [CurrencyID] [int] NULL,
	            [CurrencyPrice] [float] NULL,
	            [CurrencyEquivalent] [float] NULL
                ) ON [PRIMARY]
                ";
                Lip.ExecututeSQL(strSQL);
            }
            catch (Exception ex)
            {

                throw;
            }

            try
            {
                strSQL = @"CREATE TABLE [dbo].[Acc_VariousVoucherMachinMaster](
	[VoucherID] [int] NOT NULL,
	[BranchID] [int] NOT NULL,
	[VoucherDate] [float] NOT NULL,
	[DocumentID] [int] NOT NULL,
	[Notes] [nvarchar](500) NOT NULL,
	[UserID] [int] NOT NULL,
	[RegDate] [float] NOT NULL,
	[RegTime] [float] NOT NULL,
	[EditUserID] [int] NOT NULL,
	[EditTime] [float] NOT NULL,
	[EditDate] [float] NOT NULL,
	[ComputerInfo] [nvarchar](100) NOT NULL,
	[EditComputerInfo] [nvarchar](100) NOT NULL,
	[Cancel] [int] NOT NULL,
	[DelegateID] [int] NULL,
	[CanUpdate] [int] NULL CONSTRAINT [DF_Acc_VariousVoucherMachinMaster_CanUpdate]  DEFAULT ((0)),
	[IsExpens] [int] NULL CONSTRAINT [DF_Acc_VariousVoucherMachinMaster_IsExpens]  DEFAULT ((0)),
	[RegistrationNo] [float] NOT NULL CONSTRAINT [DF_Acc_VariousVoucherMachinMaster_RegistrationNo]  DEFAULT ((0)),
	[FacilityID] [int] NOT NULL CONSTRAINT [DF_Acc_VariousVoucherMachinMaster_FacilityID]  DEFAULT ((1)),
	[CurrencyID] [int] NULL CONSTRAINT [DF_Acc_VariousVoucherMachinMaster_CurrencyID]  DEFAULT ((1)),
	[DocumentType] [int] NOT NULL CONSTRAINT [DF_Acc_VariousVoucherMachinMaster_DocumentType]  DEFAULT ((0)),
	[Posted] [int] NULL,
 CONSTRAINT [PK_Acc_VariousVoucherMachinMaster] PRIMARY KEY CLUSTERED 
(
	[BranchID] ASC,
	[DocumentID] ASC,
	[FacilityID] ASC,
	[DocumentType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]";
                Lip.ExecututeSQL(strSQL);
            }
            catch  (Exception ex)
            {

                 
            }

            try
            {

                strSQL = @"CREATE PROCEDURE [dbo].[Acc_VariousVoucherMachin_SP] 
                    (
                    @xmlData xml =null,
                    @VoucherID int=0,
                    @VoucherDate float=0,
                    @IsExpens int=0,
                    @CanUpdate int=0,
                    @DocumentID int=0,
                    @DocumentType int=0,
@VariousImage image='',
@RegistrationNo float=0,
@CurrencyID int=0,
@Notes nvarchar(500)='',
@BranchID int=0,
@FacilityID  int=0,
@UserID int=0,
@RegDate float=0,
@RegTime float=0,
@EditUserID int=0,
@EditTime float=0,
@EditDate float=0,
@ComputerInfo nvarchar(100)='',
@EditComputerInfo nvarchar(100)='',
@DelegateID float=0,
@Cancel int=0,
@Posted int=0,
@ProductId           INT=0  OUTPUT,
@CMDTYPE AS int=0)
  As 
 DECLARE 
@VoucherID_p nvarchar(50),
@StartingNumb INT=0,
@i int=1;
 BEGIN
 IF @CMDTYPE=1 
 BEGIN

  SELECT  @ProductId = isnull((max(VoucherID)+1),0) from Acc_VariousVoucherMachinMaster ; 

  Select @StartingNumb =  StartFrom From StartNumbering Where BranchID=@BranchID
                     And FormName='frmVariousVoucher'

if @ProductId=0
  begin 
 set  @ProductId=1
  end 
if @StartingNumb>@ProductId
begin 
set @ProductId=@StartingNumb
end 


 Insert Into [Acc_VariousVoucherMachinMaster] (
 VoucherID,
BranchID,
VoucherDate,
Notes,
DocumentID,
DocumentType,
UserID,
RegDate,
RegTime,
EditUserID,
EditTime,
EditDate,
ComputerInfo,
EditComputerInfo,
DelegateID,
Cancel,
Posted,
RegistrationNo,FacilityID,CurrencyID,IsExpens,CanUpdate) Values (
@ProductId,
@BranchID,
@VoucherDate,
@Notes,
@DocumentID,
@DocumentType,
@UserID,
@RegDate,
@RegTime,
@EditUserID,
@EditTime,
@EditDate,
@ComputerInfo,
@EditComputerInfo,
@DelegateID,
@Cancel,
@Posted,
@RegistrationNo,@FacilityID,@CurrencyID,@IsExpens,@CanUpdate)

  
 INSERT INTO Acc_VariousVoucherMachinDetails
       ( VoucherID,FacilityID,BranchID,AccountID,Debit,Credit,DebitGold,CreditGold,CreditDiamond,DebitDiamond,AccountAssest,CostCenterID,DocumentType,Declaration,DebitMatirial,CreditMatirial,CurrencyID,CurrencyPrice,CurrencyEquivalent)
       SELECT   
                @ProductId,
				@FacilityID,
				@BranchID,
				x.value('AccountID[1]', 'float') AS AccountID,
				x.value('Debit[1]', 'float') AS Debit,
				x.value('Credit[1]', 'float') AS Credit,

				x.value('DebitGold[1]', 'float') AS DebitGold,
				x.value('CreditGold[1]', 'float') AS CreditGold,
                x.value('CreditDiamond[1]', 'float') AS CreditDiamond,
				x.value('DebitDiamond[1]', 'float') AS DebitDiamond,
  			    x.value('AccountAssest[1]', 'float') AS AccountAssest,
				x.value('CostCenterID[1]', 'int') AS CostCenterID,
				@DocumentType,
				x.value('Declaration[1]', 'nvarchar(200)') AS Declaration,
                x.value('DebitMatirial[1]', 'float') AS DebitMatirial,				
				x.value('CreditMatirial[1]', 'float') AS CreditMatirial,				
				x.value('CurrencyID[1]', 'int') AS CurrencyID,				
				x.value('CurrencyPrice[1]', 'float') AS CurrencyPrice,
				x.value('CurrencyEquivalent[1]', 'float') AS CurrencyEquivalent 
               FROM @xmlData.nodes('/ArrayOfAcc_VariousVoucherMachinDetails/Acc_VariousVoucherMachinDetails') XmlData(x)

 end 
 else iF @CMDTYPE=2
 BEGIN
 Update [Acc_VariousVoucherMachinMaster] set 
VoucherDate = @VoucherDate,
Notes = @Notes,
DocumentID = @DocumentID,
DocumentType=@DocumentType,
UserID = @UserID,
RegDate = @RegDate,
RegTime = @RegTime,
EditUserID = @EditUserID,
EditTime = @EditTime,
EditDate = @EditDate,
ComputerInfo = @ComputerInfo,
EditComputerInfo = @EditComputerInfo,
Cancel = @Cancel,
Posted=@Posted,
RegistrationNo = @RegistrationNo ,
CurrencyID=@CurrencyID,
IsExpens=@IsExpens,
CanUpdate=@CanUpdate

where FacilityID=@FacilityID and BranchID = @BranchID and VoucherID=@VoucherID



DELETE FROM Acc_VariousVoucherMachinDetails WHERE BranchID =@BranchID AND VoucherID= @VoucherID   and FacilityID=@FacilityID

INSERT INTO Acc_VariousVoucherMachinDetails
       ( VoucherID,FacilityID,BranchID,AccountID,Debit,Credit,DebitGold,CreditGold,CreditDiamond,DebitDiamond,AccountAssest,CostCenterID,DocumentType,Declaration,DebitMatirial,CreditMatirial,CurrencyID,CurrencyPrice,CurrencyEquivalent)
       SELECT   
                @VoucherID,
				@FacilityID,
				@BranchID,
				x.value('AccountID[1]', 'float') AS AccountID,
				x.value('Debit[1]', 'float') AS Debit,
				x.value('Credit[1]', 'float') AS Credit,
			    x.value('DebitGold[1]', 'float') AS DebitGold,
				x.value('CreditGold[1]', 'float') AS CreditGold,
                x.value('CreditDiamond[1]', 'float') AS CreditDiamond,
				x.value('DebitDiamond[1]', 'float') AS DebitDiamond,
  			    x.value('AccountAssest[1]', 'float') AS AccountAssest,
				x.value('CostCenterID[1]', 'int') AS CostCenterID,
				@DocumentType,
				x.value('Declaration[1]', 'nvarchar(200)') AS Declaration,
                x.value('DebitMatirial[1]', 'float') AS DebitMatirial,				
				x.value('CreditMatirial[1]', 'float') AS CreditMatirial,				
				x.value('CurrencyID[1]', 'int') AS CurrencyID,				
				x.value('CurrencyPrice[1]', 'float') AS CurrencyPrice,
				x.value('CurrencyEquivalent[1]', 'float') AS CurrencyEquivalent 
               FROM @xmlData.nodes('/ArrayOfAcc_VariousVoucherMachinDetails/Acc_VariousVoucherMachinDetails') XmlData(x)

set @ProductId=@VoucherID;
end 
 else iF @CMDTYPE=3
 BEGIN
 SELECT [VoucherID]
      ,[BranchID]
      ,[VoucherDate]
      ,[DocumentID]
      ,[Notes]
      ,[UserID]
      ,[RegDate]
      ,[RegTime]
      ,[EditUserID]
      ,[EditTime]
      ,[EditDate]
      ,[ComputerInfo]
      ,[EditComputerInfo]
      ,[Cancel]
      ,[DelegateID]
      ,[CanUpdate]
      ,[IsExpens]
      ,[RegistrationNo]
      ,[FacilityID]
      ,[CurrencyID]
	  ,IsExpens
      ,CanUpdate
  FROM [Acc_VariousVoucherMachinMaster] where  FacilityID =  @FacilityID and BranchID =  @BranchID AND Cancel =0
 end 
 else iF @CMDTYPE=6
 BEGIN
 Update [Acc_VariousVoucherMachinMaster] set Cancel=1 ,EditUserID=@EditUserID,EditDate=@EditDate
 Where
 VoucherID =  @VoucherID and 
 FacilityID =  @FacilityID and 
 BranchID =  @BranchID 
 end 
  else if @CMDTYPE=7
 begin
 select count(*) cntRegNo from [Acc_VariousVoucherMachinMaster] where RegistrationNo =@RegistrationNo
 end

 else iF @CMDTYPE=5
 BEGIN
SELECT      Acc_VariousVoucherMachinDetails.DebitMatirial,Acc_VariousVoucherMachinDetails.CreditMatirial,Acc_VariousVoucherMachinDetails.CurrencyEquivalent,Acc_VariousVoucherMachinDetails.CurrencyID,Acc_VariousVoucherMachinDetails.CurrencyPrice,  Acc_VariousVoucherMachinDetails.ID, Acc_VariousVoucherMachinDetails.AccountID, Acc_VariousVoucherMachinDetails.Credit, Acc_VariousVoucherMachinDetails.Debit,Acc_VariousVoucherMachinDetails.DebitGold,Acc_VariousVoucherMachinDetails.CreditGold,Acc_VariousVoucherMachinDetails.DebitDiamond,Acc_VariousVoucherMachinDetails.CreditDiamond, Acc_VariousVoucherMachinDetails.Declaration, 
                         Acc_VariousVoucherMachinDetails.CostCenterID, Acc_VariousVoucherMachinMaster.VoucherID, Acc_VariousVoucherMachinMaster.BranchID, Acc_VariousVoucherMachinMaster.FacilityID, 
                         Acc_VariousVoucherMachinMaster.Posted, Acc_VariousVoucherMachinMaster.DocumentType, Acc_VariousVoucherMachinMaster.VoucherDate, Acc_VariousVoucherMachinMaster.Notes, 
                         Acc_VariousVoucherMachinMaster.DocumentID, Acc_VariousVoucherMachinMaster.DelegateID, Acc_VariousVoucherMachinMaster.RegistrationNo, Acc_VariousVoucherMachinMaster.Cancel, 
                         Acc_VariousVoucherMachinMaster.CurrencyID, Acc_Accounts.ArbName AS ArbAccountName, Acc_Accounts.EngName AS EngAccountName, Acc_CostCenters.ArbName AS CostCenterName
FROM            Acc_VariousVoucherMachinMaster RIGHT OUTER JOIN
                         Acc_VariousVoucherMachinDetails ON Acc_VariousVoucherMachinMaster.DocumentType = Acc_VariousVoucherMachinDetails.DocumentType AND 
                         Acc_VariousVoucherMachinMaster.VoucherID = Acc_VariousVoucherMachinDetails.VoucherID AND Acc_VariousVoucherMachinMaster.FacilityID = Acc_VariousVoucherMachinDetails.FacilityID AND 
                         Acc_VariousVoucherMachinMaster.BranchID = Acc_VariousVoucherMachinDetails.BranchID LEFT OUTER JOIN
                         Acc_Accounts ON Acc_VariousVoucherMachinDetails.FacilityID = Acc_Accounts.FacilityID AND Acc_VariousVoucherMachinDetails.BranchID = Acc_Accounts.BranchID AND 
                         Acc_VariousVoucherMachinDetails.AccountID = Acc_Accounts.AccountID LEFT OUTER JOIN
                         Acc_CostCenters ON Acc_VariousVoucherMachinMaster.BranchID = Acc_CostCenters.BranchID AND Acc_VariousVoucherMachinMaster.FacilityID = Acc_CostCenters.FacilityID AND 
                         Acc_VariousVoucherMachinDetails.CostCenterID = Acc_CostCenters.CostCenterID
WHERE        (Acc_VariousVoucherMachinMaster.VoucherID <> 0) AND (Acc_VariousVoucherMachinMaster.BranchID = @BranchID) AND (Acc_VariousVoucherMachinMaster.FacilityID = @FacilityID) AND 
                         (Acc_VariousVoucherMachinMaster.Cancel = 0) AND (Acc_VariousVoucherMachinMaster.VoucherID = @VoucherID) end 
 end 


";
                Lip.ExecututeSQL(strSQL);                
                try
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ" + ex.Message);
               
                }
              

            }
            catch  (Exception ex)
            {
                MessageBox.Show("خطأ"+ex.Message);
                
                
            }
           
            strSQL = "Delete from Acc_VariousVoucherMachinDetails";
            Lip.ExecututeSQL(strSQL);
            strSQL = "Delete from Acc_VariousVoucherMachinMaster";
            Lip.ExecututeSQL(strSQL);
            ProgressBar.Value = 2;

            if (chkPurchase.Checked)
            {
                frmCashierPurchaseGold frm = new frmCashierPurchaseGold();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 3;
            }
            if (chkPurchase.Checked)
            {
                frmCashierPurchaseDaimond frm = new frmCashierPurchaseDaimond();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 4;
            }


            if (chkPurchase.Checked)
            {
                frmCashierPurchaseSaveDaimond frm = new frmCashierPurchaseSaveDaimond();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 5;
            }
            if (chkPurchase.Checked)
            {
                frmCashierPurchaseSaveDaimondReturn frm = new frmCashierPurchaseSaveDaimondReturn();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 6;
            }
            if (chkePurchaseReturn.Checked)
            {
                frmCashierPurchaseReturnGold frm = new frmCashierPurchaseReturnGold();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 7;
            }
            if (chkePurchaseReturn.Checked)
            {
                frmCashierPurchaseReturnDaimond frm = new frmCashierPurchaseReturnDaimond();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 8;
            }
            if (chkSale.Checked)
            {
                frmCashierSalesGold frm = new frmCashierSalesGold();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 9;
            }
            if (chkSale.Checked)
            {
                frmCashierSalesAlmas frm = new frmCashierSalesAlmas();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 10;
            }
            if (chkSaleReturn.Checked)
            {
                frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 11;
            }
            if (chkViriouse.Checked)
            {

                frmOpeningVoucher frm = new frmOpeningVoucher();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 12;

            }
            if (chkViriouse.Checked)
            {
             
                frmVariousVoucher frm = new frmVariousVoucher();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 13;
               
            }
            if (chkSpend.Checked)
            {
                frmSpendVoucher frm = new frmSpendVoucher();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 14;
            }
            if (chekRecipt.Checked)
            {
                frmReceiptVoucher frm = new frmReceiptVoucher();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 15;
            }

            if (chkInBail.Checked)
            {
                frmItemsInonBail frm = new frmItemsInonBail();
              
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 16;
            }

            if (chkOutBail.Checked)
            {
                frmItemsOutOnBail frm = new frmItemsOutOnBail();
                
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 17;
            }

            if (chkGoodsOpening.Checked)
            {
         
       

                frmGoodsOpeningOld frm = new frmGoodsOpeningOld();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 18;
            }
            if (chkInBail.Checked)
            {
                frmGoldInOnBail frm = new frmGoldInOnBail();
                frm.FormView = true;
                frm.FormAdd = true;
                frm.Show();
                frm.Hide();
                frm.Transaction();
                ProgressBar.Value = 19;
            }
            SplashScreenManager.CloseForm(false);
            Messages.MsgInfo("ترحيل البيانات", "تم الترحيل بنجاح");      
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            bool Yes = Messages.MsgWarningYesNo(Messages.TitleWorning, " هل تريد بالتاكيد الغاء دليل الحسابات السابق مع جميع بيانات الفرع ؟");
            if (!Yes)
                return;
            chkNewTree.Visible = true;
            txtLevelCounts.Text = "";
         
       try
        {
            using (SqlConnection objCnn = new GlobalConnection().Conn)
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    objCmd.CommandText = "[DeleteAllTable_SP]";
                    objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));
                    objCmd.Parameters.Add(new SqlParameter("@BranchID", Comon.cInt(txtBranchID.Text)));
                    objCmd.ExecuteNonQuery();
                   
                }
            }
        }
        catch
        {
            Messages.MsgError(Messages.TitleError, "خطأ عملية حذف الدليل مع بيانات الفرع");
        }

            //string StrSqlLevel = "Delete from Acc_AccountsLevels where BranchID=" + txtBranchID.Text;
            //Lip.ExecututeSQL(StrSqlLevel);
            ////To Delete All Other Permissions in this Branch
            //string StrSqlOutherPermessions = "Delete from UserOtherPermissions where BranchID=" + txtBranchID.Text;
            //Lip.ExecututeSQL(StrSqlOutherPermessions);

            //string StrSql = "Delete from Acc_Accounts where BranchID=" + txtBranchID.Text;
            //Lip.ExecututeSQL(StrSql);
             

        }
    }
}

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
using Edex.DAL.UsersManagement;
using DevExpress.XtraSplashScreen;
using DevExpress.Utils;
using DevExpress.XtraRichEdit.API.Native;
namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmUser : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        private cUser cClass = new cUser();

        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;

        private string strSQL;
        private bool IsNewRecord;
        #endregion

        #region   Event
        public frmUser()
        {

            InitializeComponent();
            /***************************Edit & Print & Export ****************************/
           //  this.Name =(UserInfo.Language == iLanguage.Arabic ? "شاشة المستخدمين" : "Users");
            /*****************************************************************************/
          
            this.txtUserID.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserID_Validating);
            this.txtUserID.EditValueChanged += new System.EventHandler(this.txtUserID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
            this.txtEmployeeID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmpolyID_Validating);
            this.txtEmail.EditValueChanged += new System.EventHandler(this.txtEmail_EditValueChanged);
            strSQL = "ArbName";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
            FillComboBox(cmbIsAvtive, "YesNo", "ID", strSQL);
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
        }

        /// <summary>
        /// This Event To Load form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUser_Load(object sender, EventArgs e)
        {
            FillGrid();

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

        /// <summary>
        /// This Email Execute when txtEmail EditValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEmail_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }

        private void txtUserID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtUserID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtUserID.Text),Comon.cInt(cmbBranchesID.EditValue));
                    TempUserID = txtUserID.Text;
                    ClearFields();
                    txtUserID.Text = TempUserID;
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
        private void txtUserID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void txtEmpolyID_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                strSQL = "SELECT ArbName as SalesDelegateName FROM HR_EmployeeFile WHERE (EmployeeID =" + txtEmployeeID.Text + ") And Cancel =0 And (BranchID = " + UserInfo.BRANCHID + " )";
                CSearch.ControlValidating(txtEmployeeID, lblEmpName, strSQL);




            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int rowIndex = e.FocusedRowHandle;

                txtUserID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                cmbBranchesID.EditValue = Comon.cInt(GridView.GetRowCellValue(rowIndex, GridView.Columns[2].FieldName).ToString());
               
                txtUserID_Validating(null, null);
                txtEmpolyID_Validating(null, null);

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
                txtUserID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
              cmbBranchesID.EditValue =Comon.cInt( GridView.GetRowCellValue(rowIndex, GridView.Columns[2].FieldName).ToString());
                txtUserID_Validating(null, null);
                txtEmpolyID_Validating(null, null);
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
                txtArbName.Text = Translator.ConvertNameToOtherLanguage(obj.Text.Trim().ToString(), UserInfo.Language);
        }
        #endregion

        #region Function
       /// <summary>
        /// This function is used to fill the View Grid with data, which is the user ID and name
       /// </summary>
        public void FillGrid()
        {
            try{
               
            strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المستخدم ],BranchID as [ رقم الفرع] FROM " + cClass.TableName
            + " WHERE Cancel =0 ";
           

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [User Name], BranchID [Branch ID] FROM " + cClass.TableName
           + " WHERE Cancel =0 ";
            if (MySession.GlobalBranchID != 1)
                strSQL = strSQL + " and BranchID=" + MySession.GlobalBranchID;

            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            GridView.GridControl.DataSource = dt;

            GridView.Columns[0].Width = 50;
            GridView.Columns[1].Width = 100;
            }
            catch { }

        }
        /// <summary>
        /// This function used to select User ID and Name and show in frmSearch  
        /// </summary>
        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            /****************************stetement with Aribic Languague******************************/
            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المستخدم] FROM " + cClass.TableName
          + " WHERE Cancel =0 ";

            /****************************stetement with English Languague******************************/
            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, ArbName as [user Name] FROM " + cClass.TableName
            +" WHERE Cancel =0 ";

            if (MySession.GlobalBranchID != 1)
                cls.SQLStr = cls.SQLStr + "and BranchID=" + MySession.GlobalBranchID;
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
            catch { Messages.MsgInfo(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                txtUserID.Text = cls.PrimaryKeyValue.ToString();
                txtUserID_Validating(null, null);
            }
        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    TxtFacilityID.Text = cClass.FacilityID.ToString(); ;
                    cmbBranchesID.EditValue =Comon.cInt( cClass.BranchID.ToString()); 
                    txtUserID.Text = cClass.UserID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtPassword.Text = "";
                    txtMobile.Text = cClass.Mobile;
                    txtAddress.Text = cClass.Address;
                    txtNotes.Text = cClass.Notes;
                    txtEmail.Text = cClass.Email;
                    txtEmployeeID.Text = cClass.EmployeeID.ToString();
                    txtEmpolyID_Validating(null, null);
                    cmbIsAvtive.EditValue = cClass.IsActive;
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = txtUserID.Text + "/" + GridView.RowCount;
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
  
        /**************************This Function For Clear The TextEdit*******************/
        public void ClearFields()
        {
            try
            {
                txtUserID.Text = cClass.GetNewID(Comon.cInt(cmbBranchesID.EditValue)).ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtPassword.Text = " ";
                txtEmployeeID.Text = "";
                lblEmpName.Text = " ";
                txtMobile.Text = " ";
                txtAddress.Text = " ";
                txtNotes.Text = " ";
                txtEmail.Text = " ";
                cmbIsAvtive.EditValue = 0;
                txtPassword.Text = " ";
                cmbIsAvtive.ItemIndex = 0;
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
                    if (MySession.GlobalBranchID != 1)
                        strSQL = strSQL + "and BranchID=" + MySession.GlobalBranchID;

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
        /// <summary>
        /// This Function  used to Fill Combobox 
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="Tablename"></param>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="OrderByField"></param>
        public void FillComboBox(DevExpress.XtraEditors.LookUpEdit cmb, string Tablename, string Code, string Name, string OrderByField = "")
        {
            try
            {
                string strSQL = "SELECT " + Code + " AS [الرقم]," + Name + "  AS [الاسم] FROM " + Tablename;
                if (OrderByField != "")
                    strSQL = strSQL + " Order By " + OrderByField;
                cmb.Properties.DataSource = Lip.SelectRecord(strSQL).DefaultView;
                cmb.Properties.DisplayMember = "الاسم";
                cmb.Properties.ValueMember = "الرقم";
            }
            catch { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        private bool EmailAddressChecker(string emailAddress)
        {

            string regExPattern = "^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";
            bool emailAddressMatch = Match.Equals(emailAddress, regExPattern);
            // regExPattern.p
            return emailAddressMatch;
        }
        /*******************Do Functions *************************/
        protected override void DoNew()
        {
            try
            {

                IsNewRecord = true;
                ClearFields();
                txtArbName.Focus();

                txtPassword.Text = Security.CreateRandomPassword(4);

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
                MoveRec(Comon.cInt(txtUserID.Text), xMoveNext);

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
                MoveRec(Comon.cInt(txtUserID.Text), xMovePrev);
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
                if (!Validations.IsValidForm(this))
                    return;
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
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                Users model = new Users();
                model.UserID = Comon.cInt(txtUserID.Text);
                if (IsNewRecord == true)
                    model.UserID = 0;
                model.ArbName = txtArbName.Text;
                model.EngName = txtEngName.Text;
                string hashedPassword = Security.HashSHA1(txtPassword.Text);
                model.Password = hashedPassword;
                model.EmployeeID = Comon.cLong(txtEmployeeID.Text);
                model.Address = txtAddress.Text;
                model.Mobile = txtMobile.Text;
                model.Notes = txtNotes.Text; ;
                model.IsActive = Comon.cInt(cmbIsAvtive.EditValue);
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.AddByUserID = UserInfo.ID;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.Cancel = 0;
                model.Email = txtEmail.Text;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = Comon.cInt(TxtFacilityID.Text);
                model.EditUserID = 0;
                model.EditTime = 0;
                model.EditDate = 0;
                model.EditComputerInfo = "";
                if (IsNewRecord == false)
                {
                    model.EditUserID = UserInfo.ID;
                    model.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                    model.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                    model.EditComputerInfo = UserInfo.ComputerInfo;
                   model.IsActiveAllowedDays = cClass.IsActiveAllowedDays;
                   model.Gender = cClass.Gender;
                   model.pic = null;
                   model.NumberAllowedDays = cClass.NumberAllowedDays;
                   model.AllowedDate = cClass.AllowedDate;

                }
                // model.AddByUserID = UserInfo.ID;
                int Result = UsersManagementDAL.InsertUser(model);
                SplashScreenManager.CloseForm(false);
                if (IsNewRecord == true)
                {
                    if (Result >0)
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                    else
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave);
                    DoNew();
                }
                else
                {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                }
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

                int TempID = Comon.cInt(txtUserID.Text);

                Users model = new Users();
                model.UserID = Comon.cInt(txtUserID.Text);
                model.EditUserID = UserInfo.ID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = Comon.cInt(TxtFacilityID.Text);
                 
                bool Result = UsersManagementDAL.DeleteUser(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                MoveRec(model.UserID, xMovePrev);
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
                GridView.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        

        #endregion

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            ClearFields();
        }
         
        

    }
}

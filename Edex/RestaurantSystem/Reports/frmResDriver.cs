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
using Edex.DAL.SalseSystem;
using DevExpress.Utils;
using DevExpress.XtraRichEdit.API.Native;


namespace Edex.SalesAndPurchaseObjects.Codes
{
    public partial class frmResDriver : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        public DataTable importData = new DataTable();
        public bool sendFromExel = false;
        #region Declare
        private cResDriver cClass = new cResDriver();


        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;

        private string strSQL;
        private bool IsNewRecord;

        public string ArbName;
        public string EngName;
        public long AccountID;

        #endregion


        #region Form Event
        public frmResDriver()
        {
           
            InitializeComponent();
            /***************************Edit & Print & Export ****************************/
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;

            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;

            /*****************************************************************************/

            this.txtEmail.EditValueChanged += new System.EventHandler(this.txtEmail_EditValueChanged);
            this.txtEmail.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmail_Validating);
            this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
            this.txtDelegateID.EditValueChanged += new System.EventHandler(this.txtDelegateID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
        }

        #endregion
        #region Function

        public void FillGrid()
        {
            try{
            strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم السائق] FROM " + cClass.TableName + " WHERE Cancel =0 ";

            if (UserInfo.Language == iLanguage.English)

                strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Driver Name] FROM " + cClass.TableName + " WHERE Cancel =0  and BranchID=";


            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            GridView.GridControl.DataSource = dt;

            GridView.Columns[0].Width = 50;
            GridView.Columns[1].Width = 100;
            }
            catch { }

        }

        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم السائق] FROM " + cClass.TableName
            + " WHERE Cancel =0  and BranchID= " + UserInfo.BRANCHID;

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Driver Name] FROM " + cClass.TableName
            + " WHERE Cancel =0  and BranchID= " + UserInfo.BRANCHID;


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

        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                txtDelegateID.Text = cls.PrimaryKeyValue.ToString();
                txtDelegateID_Validating(null, null);
            }

        }


        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {


                    txtDelegateID.Text = cClass.DelegateID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtMobile.Text = cClass.Mobile;
                    txtTel.Text = cClass.Tel;
                    txtAddress.Text = cClass.Address;
                    txtFax.Text = cClass.Fax;
                    txtNotes.Text = cClass.Notes;
                    txtEmail.Text = cClass.Email;
                    txtTarget.Text = cClass.Target.ToString();
                    txtPercentage.Text = cClass.Percentage.ToString();
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = txtDelegateID.Text + "/" + GridView.RowCount;

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
                txtDelegateID.Text = cClass.GetNewID().ToString();
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = (int.Parse(txtDelegateID.Text)).ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtMobile.Text = " ";
                txtTel.Text = " ";
                txtAddress.Text = " ";
                txtFax.Text = " ";
                txtNotes.Text = " ";
                txtEmail.Text = "";
                txtTarget.Text = "";
                txtPercentage.Text = "";

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
                MoveRec(Comon.cInt(txtDelegateID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtDelegateID.Text), xMovePrev);
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


                Sales_SalesDelegate model = new Sales_SalesDelegate();

                model.DelegateID = Comon.cInt(txtDelegateID.Text);

                if (IsNewRecord == true)
                {
                    model.DelegateID = 0;

                }
                model.ArbName = txtArbName.Text;
                model.EngName = txtEngName.Text;
                model.Target = Comon.cLong(txtTarget.Text);
                model.Percentage = Comon.cLong(txtPercentage.Text);
                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.Tel = txtTel.Text;
                model.Mobile = txtMobile.Text;
                model.Fax = txtFax.Text;
                model.Address = txtAddress.Text;

                model.Notes = txtNotes.Text;
                model.Email = txtEmail.Text;
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;

                int StoreID;
                bool UpdateID;
                if (IsNewRecord == true)
                    StoreID = Sales_SalesDelegateDAL.InsertSales_SalesDelegate2(model);
                else
                    UpdateID = Sales_SalesDelegateDAL.UpdateSales_SalesDelegate2(model);

                if (sendFromExel == false)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
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

                int TempID = Comon.cInt(txtDelegateID.Text);

                Sales_SalesDelegate model = new Sales_SalesDelegate();
                model.DelegateID = Comon.cInt(txtDelegateID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                bool Result = Sales_SalesDelegateDAL.DeleteSales_SalesDelegate2(model);
               
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                MoveRec(model.DelegateID, xMovePrev);
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


        #region Event
        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtDelegateID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtDelegateID.Text));
                    TempUserID = txtDelegateID.Text;
                    ClearFields();
                    txtDelegateID.Text = TempUserID;
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


        private void txtDelegateID_EditValueChanged(object sender, EventArgs e)
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

                txtDelegateID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtDelegateID_Validating(null, null);

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
            try
            {
                int rowIndex = e.RowHandle;
                txtDelegateID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtDelegateID_Validating(null, null);
            }
            catch { }
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
        private void frmSalesDelegates_Load(object sender, EventArgs e)
        {
            FillGrid();
            DoNew();

        }
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

        private void btnImbort_Click(object sender, EventArgs e)
        {
            //Form1 frm = new Form1();

            //if (frm.ShowDialog(this) == DialogResult.OK)
            //{
            //    // Read the contents of testDialog's TextBox.
            //    importData = frm.dataTable;
            //}

            //frm.Dispose();
            //DoNew();
            //sendFromExel = true;
            //for (int i = 0; i < importData.Rows.Count - 1; ++i)
            //{
            //    txtArbName.Text = importData.Rows[i][0].ToString();
            //    txtEngName.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][1].ToString() : "");
            //    txtAddress.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][6].ToString() : "");
            //    txtMobile.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][3].ToString() : "");
            //    txtTel.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][2].ToString() : "");
            //    txtFax.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][4].ToString() : "");
            //  //  txtSpecialDiscount.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][8].ToString() : "");
            //    txtEmail.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][5].ToString() : "");
            //    txtNotes.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][7].ToString() : "");

            //  //  txtVAT.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][9].ToString() : "");

            //    txtPercentage.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][10].ToString() : "");

            //    txtTarget.Text = (DBNull.Value != importData.Rows[0][1] ? importData.Rows[0][11].ToString() : "");
            //    DoSave();


            //}
            //sendFromExel = false;

        }

    }
}


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
using Edex.Model.Language;
using Edex.Model;
using Edex.SalesAndPurchaseObjects.SalesClasses;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.DAL;
namespace Edex.SalesAndPurchaseObjects.Codes
{
    public partial class frmSalesDelegates : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public frmSalesDelegates()
        {
            InitializeComponent();
            /***************************Edit & Print & Export ****************************/
           // ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;

            this.txtSalesDelegateID.EditValueChanged += new System.EventHandler(this.txtSalerDelegateID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);

        }
        #region Declare 
        private cSalesDelegates cClass = new cSalesDelegates();

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
        private void frmSalesDelegates_Load(object sender, EventArgs e)
        {
            FillGrid();
            DoNew();
        }
        #region Event
        /*********************** Eveants ************************/
        private void txtSalerDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtSalesDelegateID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtSalesDelegateID.Text));
                    TempUserID = txtSalesDelegateID.Text;
                    ClearFields();
                    txtSalesDelegateID.Text = TempUserID;
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
        private void txtSalerDelegateID_EditValueChanged(object sender, EventArgs e)
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

        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int rowIndex = e.FocusedRowHandle;

                txtSalesDelegateID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtSalerDelegateID_Validating(null, null);

            }
            catch (Exception)
            {
                return;
            }
        }

        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int rowIndex = e.RowHandle;
            txtSalesDelegateID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtSalerDelegateID_Validating(null, null);
        }
        #endregion
        /*******************This Function To Fill GridView *******************/
        public void FillGrid()
        {

            strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المندوب] FROM " + cClass.TableName + " WHERE Cancel =0  and BranchID="+MySession.GlobalBranchID;

            if (UserInfo.Language == iLanguage.English)
                strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Sales Delegates Name] FROM " + cClass.TableName + " WHERE Cancel =0  and BranchID=" + MySession.GlobalBranchID;


            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            GridView.GridControl.DataSource = dt;

            GridView.Columns[0].Width = 50;
            GridView.Columns[1].Width = 100;

        }
        /*******************This Function To Find ****************************/
        public void Find()
        {
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المندوب  ] FROM " + cClass.TableName
            + " WHERE Cancel =0  and BranchID= " +MySession.GlobalBranchID;

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, ArbName as [Sales Delegate Name] FROM " + cClass.TableName
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
        /*********************Thiis Function To Get Selected Search Value From Screen Search *******************/
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                txtSalesDelegateID.Text = cls.PrimaryKeyValue.ToString();
                txtSalerDelegateID_Validating(null, null);
            }

        }
        /*********************This Function For Clear  Fields ********************/
        public void ClearFields()
        {
            try
            {
                txtSalesDelegateID.Text = cClass.GetNewID().ToString();
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = (int.Parse(txtSalesDelegateID.Text)).ToString();
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
        /****************************This Function For Read Record from cClass****************/
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                    txtSalesDelegateID.Text = cClass.DelegateID.ToString();
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
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = txtSalesDelegateID.Text + "/" + GridView.RowCount;

                }



            }
            catch (Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        /********************Function  Move Record************************/
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
                                strSQL = strSQL + " And " + cClass.PremaryKey + "> " + PremaryKeyValue + " ORDER BY " + cClass.PremaryKey + " ASC";
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
         /**************************************Do Functions*********************************/
        protected override void DoNew()
        {
            try
            {

                IsNewRecord = true;
                ClearFields();

                txtArbName.ReadOnly = false;
                txtEngName.ReadOnly = false;
                txtTel.ReadOnly = false;
                txtAddress.ReadOnly = false;
                txtFax.ReadOnly = false;
                txtEmail.ReadOnly = false;
                txtArbName.Focus();

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

                model.DelegateID = Comon.cInt(txtSalesDelegateID.Text);

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
                    StoreID = Sales_SalesDelegateDAL.InsertSales_SalesDelegate(model);
                else
                    UpdateID = Sales_SalesDelegateDAL.UpdateSales_SalesDelegate(model);
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

        /*************Fincton For Delete Sales Delegate *******/

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

                int TempID = Comon.cInt(txtSalesDelegateID.Text);

                Sales_SalesDelegate model = new Sales_SalesDelegate();
                model.DelegateID = Comon.cInt(txtSalesDelegateID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                bool Result = Sales_SalesDelegateDAL.DeleteSales_SalesDelegate(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                MoveRec(model.DelegateID, xMovePrev);
                FillGrid();

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
                MoveRec(Comon.cInt(txtSalesDelegateID.Text), xMoveNext);
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
                MoveRec(Comon.cInt(txtSalesDelegateID.Text), xMovePrev);
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

       

       


      
    }
}
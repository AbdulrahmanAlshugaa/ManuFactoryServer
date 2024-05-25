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
using Edex.DAL.Configuration;
using DevExpress.XtraReports.UI;
using Edex.Reports;
namespace Edex.StockObjects.Codes
{
    public partial class frmSizingUnits : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        /**************** Declare ************************/
        #region Declare

        private cItemsUnits cClass = new cItemsUnits();


        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool IsFromanotherForms = false;


        private string strSQL;
        private bool IsNewRecord;

        #endregion

        /****************Form Event************************/
        #region Form Event
        public frmSizingUnits()
        {
            InitializeComponent();
           // ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;

            this.txtSizeID.Validating += new System.ComponentModel.CancelEventHandler(this.txtSizeID_Validating);
            this.txtSizeID.EditValueChanged += new System.EventHandler(this.txtSizeID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);

        }
        #endregion
        /**********************Function**************************/
        #region Function
        public void FillGrid()
        {
            try
            {
                strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم الوحدة],BranchID as [رقم الفرع] FROM " + cClass.TableName + " WHERE Cancel =0  and BranchID="+MySession.GlobalBranchID;

                if (UserInfo.Language == iLanguage.English)
                    strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Unit Name],,BranchID as [Branch ID] FROM " + cClass.TableName + " WHERE Cancel =0  and BranchID=" + MySession.GlobalBranchID;


                DataTable dt = new DataTable();
                dt = Lip.SelectRecord(strSQL);
                GridView.GridControl.DataSource = dt;

                GridView.Columns[0].Width = 50;
                GridView.Columns[1].Width = 100;
               }
            catch {}

        }
        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم الوحدة] FROM " + cClass.TableName
            + " WHERE Cancel =0  and BranchID=" + MySession.GlobalBranchID;

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Unit Name] FROM " + cClass.TableName
            + " WHERE Cancel =0  and BranchID=" + MySession.GlobalBranchID;


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

                txtSizeID.Text = cls.PrimaryKeyValue.ToString();
                txtSizeID_Validating(null, null);
            }

        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {

                    txtSizeID.Text = cClass.SizeID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    txtNotes.Text = cClass.Notes;
                    Validations.DoReadRipon(this, ribbonControl1);
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
                txtSizeID.Text = cClass.GetNewID().ToString();
                txtArbName.Text = "";
                txtEngName.Text = "";
                txtNotes.Text = "";
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
        /*******************Do Functions *************************/
        protected override void DoEdit()
        {
            if (Comon.cInt(txtSizeID.Text) > 2)
            {
                EnabledControl(true);
                Validations.DoEditRipon(this, ribbonControl1);
            }
            else
            {
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن تعديل الواحدات المحددة لانها ثوابت" : "Cann't Edit Size this");
                return;
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
                MoveRec(Comon.cInt(txtSizeID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtSizeID.Text), xMovePrev);
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

              

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void save() {

            Stc_SizingUnits model = new Stc_SizingUnits();
            model.SizeID = Comon.cInt(txtSizeID.Text);
            //if (IsNewRecord == true)
            //    model.SizeID = 0;

            model.ArbName = txtArbName.Text;
            model.EngName = txtEngName.Text;
            model.Notes = txtNotes.Text;

            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.BranchID = MySession.GlobalBranchID;
            model.FacilityID = UserInfo.FacilityID;

            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;

            model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
            model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());

            model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
            model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            model.Cancel = 0;

            int StoreID = Stc_SizingUnitsDAL.Insert(model,IsNewRecord);
            if (IsFromanotherForms == false)
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                if (IsNewRecord == true)
                    DoNew();

                FillGrid();
            }
        }
        protected override void DoDelete()
        {
            try
            {
                int isTRans = Comon.cInt(Lip.GetValue(" select dbo.[SizingUnitItemID](" + Comon.cInt(txtSizeID.Text) + ") where  BranchID="+MySession.GlobalBranchID));
                if (isTRans > 0)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
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

                int TempID = Comon.cInt(txtSizeID.Text);

                Stc_SizingUnits model = new Stc_SizingUnits();
                model.SizeID = Comon.cInt(txtSizeID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo; 
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
            
                bool Result = Stc_SizingUnitsDAL.Delete(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                MoveRec(model.SizeID, xMovePrev);
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
                GridView.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        #endregion

        /**********************Event**************************/
        #region Event
        private void txtSizeID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;


        }
        public void txtSizeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtSizeID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtSizeID.Text));
                    TempUserID = txtSizeID.Text;
                    ClearFields();
                    txtSizeID.Text = TempUserID;
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
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int rowIndex = e.FocusedRowHandle;

                txtSizeID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtSizeID_Validating(null, null);

            }
            catch (Exception)
            {
                return;
            }

        }
        private void frmSizingUnits_Activated(object sender, EventArgs e)
        {
            // ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = true;
            // ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Item.Enabled = true;
            // FillGrid();
        }
        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try{
            int rowIndex = e.RowHandle;
            txtSizeID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtSizeID_Validating(null, null);
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

        private void frmSizingUnits_Load(object sender, EventArgs e)
        {

           
        
            FillGrid();
            DoNew();
        }
     

    }
}

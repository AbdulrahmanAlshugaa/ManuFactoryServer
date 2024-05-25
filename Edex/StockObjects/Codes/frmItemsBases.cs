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
namespace Edex.StockObjects.Codes
{
    public partial class frmItemsBases : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        #region Declare
        private cItemsBases cClass = new cItemsBases();

        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;


        private string strSQL;
        private bool IsNewRecord;

        #endregion
        #region Form Event
        public frmItemsBases()
        {
            InitializeComponent();
            /***************************Edit & Print & Export ****************************/
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;

            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;

            /*****************************************************************************/


            this.txtBaseID.Validating += new System.ComponentModel.CancelEventHandler(this.txtBaseID_Validating);
            this.txtBaseID.EditValueChanged += new System.EventHandler(this.txtBaseID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);

        }
        #endregion
        #region Function
        public void FillGrid()
        {
            try{

            strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم الصنف ] FROM " + cClass.TableName
           + " WHERE Cancel =0  ";

            if (UserInfo.Language == iLanguage.English)

                strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Base Name] FROM " + cClass.TableName
          + " WHERE Cancel =0  ";


            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            gridView1.GridControl.DataSource = dt;

            gridView1.Columns[0].Width = 50;
            gridView1.Columns[1].Width = 100;
            }
            catch { }
        }

        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم الصنف] FROM " + cClass.TableName
            + " WHERE Cancel =0  ";

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Base Name] FROM " + cClass.TableName
            + " WHERE Cancel =0  ";


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
            catch {}
        }

        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                txtBaseID.Text = cls.PrimaryKeyValue.ToString();
                txtBaseID_Validating(null, null);
            }

        }


        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {


                    txtBaseID.Text = cClass.BaseID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;

                    ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = txtBaseID.Text + "/" + gridView1.RowCount;

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
                txtBaseID.Text = cClass.GetNewID().ToString();
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = txtBaseID.Text;
                txtArbName.Text = "";
                txtEngName.Text = "";


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
                MoveRec(Comon.cInt(txtBaseID.Text), xMoveNext);

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
                MoveRec(Comon.cInt(txtBaseID.Text), xMovePrev);
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

                Stc_ItemsBases model = new Stc_ItemsBases();
                model.BaseID = Comon.cInt(txtBaseID.Text);
                if (IsNewRecord == true)
                    model.BaseID = 0;

                model.ArbName = txtArbName.Text;
                model.EngName = txtEngName.Text;

                model.UserID = UserInfo.ID;
                model.EditUserID = UserInfo.ID;
                model.ComputerInfo = UserInfo.ComputerInfo;
                model.EditComputerInfo = UserInfo.ComputerInfo;

                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());

                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                model.Cancel = 0;

                int StoreID;
               
                 StoreID = Stc_ItemsBasesDAL.Insert(model);
              if(StoreID==1)
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

                int TempID = Comon.cInt(txtBaseID.Text);

                Stc_ItemsBases model = new Stc_ItemsBases();
                model.BaseID = Comon.cInt(txtBaseID.Text);
                model.EditUserID = UserInfo.ID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                bool Result = Stc_ItemsBasesDAL.Delete(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                MoveRec(model.BaseID, xMovePrev);
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
                gridView1.ShowRibbonPrintPreview();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        #endregion


        #region Event
        public void txtBaseID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtBaseID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtBaseID.Text));
                    TempUserID = txtBaseID.Text;
                    ClearFields();
                    txtBaseID.Text = TempUserID;
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
        private void txtBaseID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void txtArbName_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int rowIndex = e.FocusedRowHandle;

                txtBaseID.Text = gridView1.GetRowCellValue(rowIndex, gridView1.Columns[0].FieldName).ToString();
                txtBaseID_Validating(null, null);

            }
            catch (Exception)
            {
                return;
            }

        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
            int rowIndex = e.RowHandle;
            try{
            txtBaseID.Text = gridView1.GetRowCellValue(rowIndex, gridView1.Columns[0].FieldName).ToString();
            txtBaseID_Validating(null, null);
            }
            catch {}
        }

        private void frmItemsBases_Activated(object sender, EventArgs e)
        {
            //FillGrid();

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
       
        private void frmItemsBases_Load(object sender, EventArgs e)
        {


            /***************************Edit & Print & Export ****************************/
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            /*****************************************************************************/
            FillGrid();
            DoNew();
        }

        #endregion





    }
}
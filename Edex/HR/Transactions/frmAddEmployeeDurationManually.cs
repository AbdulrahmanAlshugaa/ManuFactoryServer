


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
using Edex.GeneralObjects.GeneralForms;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model.Language;
using Edex.ModelSystem;
using Edex.Model;
using Edex.StockObjects.StoresClasses;
using Edex.DAL;

namespace Edex.HR.Codes
{
    public partial class frmAddEmployeeDurationManually : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        /**************** Declare ************************/
        #region Declare
        private cAddEmployeeDurationManually cClass = new cAddEmployeeDurationManually();

     
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
        public frmAddEmployeeDurationManually()
        {
            InitializeComponent();
            // ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            this.txtID.Validating += new System.ComponentModel.CancelEventHandler(this.txtTypeID_Validating);
            this.txtID.EditValueChanged += new System.EventHandler(this.txtTypeID_EditValueChanged);
            this.GridView.FocusedRowChanged += GridView_FocusedRowChanged;
            InitializeFormatDate(txtFingerPrintDate);
        }
        #endregion
        /**********************Function**************************/
        #region Function
        public void FillGrid()
        {
            try
            {
                strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, EmployeeIDInFingerTecDevice as [رقم البصمة], FingerPrintDate AS [التاريخ] , FingerPrintTime AS [الوقت] FROM " + cClass.TableName + " WHERE Cancel =0  ";

                if (UserInfo.Language == iLanguage.English)
                    strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Name] FROM " + cClass.TableName + " WHERE Cancel =0  ";

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
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [الاسم] FROM " + cClass.TableName
                + " WHERE Cancel =0  ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Name] FROM " + cClass.TableName
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
            catch { }
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                txtID.Text = cls.PrimaryKeyValue.ToString();
                txtTypeID_Validating(null, null);
            }

        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                { 
                    txtNotes.Text = cClass.Notes;
                    txtID.Text = cClass.ID.ToString();
                    txtEmployeeIDInFingerTecDevice.Text = cClass.EmployeeIDInFingerTecDevice.ToString();
                    txtEmployeeIDInFingerTecDevice_Validating(null, null);
                    txtFingerPrintTime.Text = cClass.FingerPrintTime.ToString();
                    txtFingerPrintDate.EditValue = Comon.ConvertSerialToDate(cClass.FingerPrintDate.ToString());
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
                txtID.Text = cClass.GetNewID().ToString();
                txtEmployeeID.Text = "";
                txtNotes.Text = "";
                txtEmployeeIDInFingerTecDevice.Text = "";
                lblEmpName.Text = "";
                InitializeFormatDate(txtFingerPrintDate);
                txtFingerPrintTime.Text = "";

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
                txtEmployeeID.Focus();

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        protected override void DoEdit()
        {
            Validations.DoEditRipon(this, ribbonControl1);
            Validations.EnabledControl(this, true);
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
                MoveRec(Comon.cInt(txtID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtID.Text), xMovePrev);
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
        public void save()
        {

            cAddEmployeeDurationManually model = new cAddEmployeeDurationManually();
            model. ID = Comon.cInt(txtID.Text);
             
            model.Notes = txtNotes.Text;
            model.FingerPrintDate =Comon.cLong(  Comon.ConvertDateToSerial(txtFingerPrintDate.EditValue.ToString()));
            model.FingerPrintTime = Comon.cInt( txtFingerPrintTime.Text);
            model.EmployeeIDInFingerTecDevice = Comon.cInt(txtEmployeeIDInFingerTecDevice.Text);
            model.UserID = UserInfo.ID;
            model.EditUserID = UserInfo.ID;
            model.BranchID = UserInfo.BRANCHID;
            model.FacilityID = UserInfo.FacilityID;

            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;

            model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
            model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
            if (IsNewRecord == false)
            {
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerDateSerial());
            }
            else
            {
                model.EditDate = 0;
                model.EditTime = 0;
            }
            model.Cancel = 0;
            Lip.NewFields();
            Lip.Table = cClass.TableName;
            Lip.AddNumericField("BranchID", model.BranchID.ToString());
            Lip.AddNumericField("FacilityID", model.FacilityID.ToString());
            Lip.AddStringField("Notes", model.Notes);
            Lip.AddNumericField("FingerPrintDate", model.FingerPrintDate.ToString());
            Lip.AddNumericField("FingerPrintTime", model.FingerPrintTime.ToString());
            Lip.AddNumericField("FingerPrintSeconds", model.FingerPrintTime.ToString());
            Lip.AddNumericField("EmployeeIDInFingerTecDevice", model.EmployeeIDInFingerTecDevice.ToString());
            Lip.AddNumericField("UserID", UserInfo.ID);
            Lip.AddNumericField("RegDate", model.RegDate.ToString());
            Lip.AddNumericField("RegTime", model.RegTime.ToString());
            Lip.AddNumericField("EditUserID", UserInfo.ID);
            Lip.AddNumericField("EditDate", model.EditDate.ToString());
            Lip.AddNumericField("EditTime", model.EditDate.ToString());
            Lip.AddStringField("ComputerInfo", model.ComputerInfo);
            Lip.AddStringField("EditComputerInfo", model.ComputerInfo);
            Lip.AddNumericField("Cancel", 0);
            Lip.sCondition = "ID=" + Comon.cInt(txtID.Text);
            if(IsNewRecord==true)
            Lip.ExecuteInsert();
            else
            Lip.ExecuteUpdate();

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

                int TempID = Comon.cInt(txtID.Text);

                Lip.NewFields();
                Lip.Table = cClass.TableName;
                Lip.AddNumericField("Cancel", 0);
                Lip.sCondition = "ID=" + Comon.cInt(txtID.Text);
                Lip.ExecuteUpdate();

                MoveRec(TempID, xMovePrev);
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
        private void txtTypeID_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;
        }

        private void frmAdministrations_Load(object sender, EventArgs e)
        {
            FillGrid();
            DoNew();
        }
        public void txtTypeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtID.Text));
                    TempUserID = txtID.Text;
                    ClearFields();
                    txtID.Text = TempUserID;
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
                    Validations.DoReadRipon(this, ribbonControl1);
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

                txtID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtTypeID_Validating(null, null);

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
                txtID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtTypeID_Validating(null, null);
            }
            catch { }
        }
        private void txtArbName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;
        }
        private void txtEngName_Validating(object sender, CancelEventArgs e)
        {
            TextEdit obj = (TextEdit)sender;
        }
        #endregion

        public void txtEmployeeID_Validating(object sender, CancelEventArgs e)
        {

        }
        private void txtEmployeeIDInFingerTecDevice_Validating(object sender, CancelEventArgs e)
        {
            DataTable dt = new DataTable();
            string strSQL = "SELECT FootprintEmpID,ArbName as EmployeeName , EmployeeID FROM HR_EmployeeFile WHERE EmployeeID =" + (txtEmployeeIDInFingerTecDevice.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                lblEmpName.Text = dt.Rows[0]["EmployeeName"].ToString();
                txtEmployeeIDInFingerTecDevice.Text = dt.Rows[0]["FootprintEmpID"].ToString();
                txtEmployeeID.Text = dt.Rows[0]["EmployeeID"].ToString();
            }
            else
            {
                txtEmployeeID.Text = "";
                lblEmpName.Text = "";
                txtEmployeeIDInFingerTecDevice.Text = "";
           }
        }

        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            Obj.EditValue = DateTime.Now;

        }
    }
}

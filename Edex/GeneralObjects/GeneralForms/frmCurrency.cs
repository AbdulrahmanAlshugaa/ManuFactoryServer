using DevExpress.XtraEditors;
using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
 
using Edex.DAL.Common;
using DevExpress.XtraPrinting;
using System.Diagnostics;
using Edex.DAL.Configuration;

namespace Edex.GeneralObjects.GeneralForms
{ 
    public partial class frmCurrency : Edex.GeneralObjects.GeneralForms.BaseForm
    {
     
        /**************** Declare ************************/
        #region Declare

        private string strSQL;
        private bool IsNewRecord;
        string FocusedControl = "";
        private static CURRENCY_BO cClass = new CURRENCY_BO();

        public bool IsFromanotherForms = false;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        PrintingSystem printingSystem1 = new PrintingSystem();
        PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
        #endregion
        /****************Form Event************************/
        #region Form Event
        public frmCurrency()
        {
            InitializeComponent();
           
            this.GridView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GridView_RowClick);
            this.GridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GridView_FocusedRowChanged);
            this.txtID.Validating += new System.ComponentModel.CancelEventHandler(this.txtID_Validating);
            this.txtID.EditValueChanged += new System.EventHandler(this.txtID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
             
             
        }
        private void frmCurrency_Load(object sender, EventArgs e)
        {
            cClass = new CURRENCY_BO();
           
            FormsPrperties.ColorForm(this);
            List<CurrencyInfo> currencies = new List<CurrencyInfo>();

            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Syria));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.UAE));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Bahrain));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Dolar));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Gold));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Qatar));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Yemen));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Kuwait));
            for (int i = 0; i <= currencies.Count - 1; i++)
            {
                cmbCurencyTafqeet.Items.Add(currencies[i].CurrencyID + " " + currencies[i].Arabic1CurrencyName);
            }
            FillDataGrid();
            DoNew();
            Validations.DoLoadRipon(this, ribbonControl1);
            Validations.EnabledControl(this, false);
            ribbonControl1.Items[19].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//اضافة من
        }


        #endregion
        /**********************Function**************************/
        #region Function
        public void FillDataGrid()
        {
            FillGrid.FillGridView(GridView, cClass.TableName, cClass.PremaryKey,Where:("   BranchID= " + MySession.GlobalBranchID).ToString());
        }
        string GetIndexFocusedControl()
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
                return c.Parent.Name;
            }

            return c.Name;
        }
        protected override void Find()
        {
            try
            {
             Lovs.CurrenciesList(this);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void GetSelectedSearch(CSearch cls)
        {
            FocusedControl = GetIndexFocusedControl();

            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                //if (FocusedControl == txtID.Name)
                //{
                txtID.Text = cls.PrimaryKeyValue.ToString();
                txtID_Validating(null, null);
                //}
            }

        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                {
                    txtID.Text = cClass.ID.ToString();
                    txtArbName.Text = cClass.ARBNAME;
                    txtEngName.Text = cClass.ENGNAME;
                    txtNotes.Text = cClass.NOTES;

                    txtCurrncyPart.Text = cClass.CurrncyPart; ; 
                    txtCodeCurrency.Text = cClass.CodeCurrency; 
                    txtTransPricing.Text = cClass.TransPricing.ToString(); 
                    txtMaxTransPricing.Text = cClass.MaxTransPricing.ToString(); 
                    txtMinTransPricing.Text = cClass.MinTransPricing.ToString(); 
                    ChkStoreCurrency.Checked = Comon.cbool(cClass.StoreCurrency);
                    rdLocalCurrency.Checked =  cClass.TypeCurrency==1?true:false;
                    rdForignCurrency.Checked = cClass.TypeCurrency == 2 ? true : false;
                    cmbCurencyTafqeet.SelectedIndex =Comon.cInt(cClass.TAFQEETID);
                    //المستخدم المدخل
                    lbfUserCreatedID.Text = cClass.USERCREATED.ToString();
                 //   txfUserCreatedID_Validating(null, null);

                    //المستخدم المعدل
                    lbfUserUpdatedD.Text = cClass.USERUPDATED.ToString();
                //    txfUserUpdatedD_Validating(null, null);

                    //تاريخ الادخل  
                    string TimeCreated = Comon.ConvertSerialToTime(cClass.TIMECREATED.ToString()).ToString();
                    lblUserDateCreated.Text = Comon.ConvertSerialToDate(cClass.DATECREATED.ToString()).ToString() + " " + TimeCreated;

                    //تاريخ التعديل  
                    if (cClass.DATEUPDATED > 0)
                    {
                        string TimeUpdated = Comon.ConvertSerialToTime(cClass.TIMEUPDATED.ToString()).ToString();
                        lblUserDateUpdated.Text = Comon.ConvertSerialToDate(cClass.DATEUPDATED.ToString()).ToString() + " " + TimeUpdated;
                    }
                    else
                        lblUserDateUpdated.Text = "";

                    lblComputerInfo.Text = cClass.ComputerInfo.ToString();
                    lblCompoterEdit.Text = cClass.EditComputerInfo.ToString();

                    Validations.DoReadRipon(this, ribbonControl1);
                    Validations.EnabledControl(this, false);
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public void ClearFields()
        {
            try
            {
                txtID.Text = CurrencyDAL.GetNewID().ToString();
                txtArbName.Text = "";
                txtEngName.Text = "";
                txtNotes.Text = "";
                 
                txtCurrncyPart.Text = ""; 
                txtCodeCurrency.Text = ""; 
                txtTransPricing.Text = ""; 
                txtMaxTransPricing.Text = ""; 
                txtMinTransPricing.Text = ""; 

                ChkStoreCurrency.Checked = false;
                rdLocalCurrency.Checked = false;
                rdForignCurrency.Checked = false;

                lblMaxPrice.Visible = false;
                lblMinPrice.Visible = false;
                lblTransPrice.Visible = false;

                txtMaxTransPricing.Visible = false;
                txtMinTransPricing.Visible = false;
                txtTransPricing.Visible = false;

                lblUserCreatedID.Text = "";
                lblUserDateCreated.Text = "";

                lblUserDateUpdated.Text = "";
                lblUserUpdatedID.Text = "";

                lbfUserUpdatedD.Text = "";
                lbfUserCreatedID.Text = "";

                lblCompoterEdit.Text = "";
                lblComputerInfo.Text = "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        /******************** MoveRec ************************/
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            if (FormView == false)
            {
                Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
            if (txtArbName.Text != string.Empty && IsNewRecord == true)
            {
                bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "هل التراجع عن الاضافة");
                if (!Yes)
                    return;
            }
            if (cClass == null)
                cClass = new CURRENCY_BO();
            string where = " Cancel=0 " ;
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

                    DataTable dt = new DataTable();
                    cClass = CURRENCY_DAL.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                    {
                        ReadRecord();

                    }
                }

                #endregion
               
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
                Validations.EnabledControl(this, true);
                Validations.DoNewRipon(this, ribbonControl1);
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
                txtID.Focus();
                Find();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoEdit()
        {
            if (Comon.cInt(txtID.Text) != 1)
            {
                Validations.DoEditRipon(this, ribbonControl1);
                Validations.EnabledControl(this, true);
                txtArbName.Focus();
                IsNewRecord = false;
            }
            else
            {
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن تعديل عملة الذهب " : "Can't Edit The Gold Currncy");
                return;
            }
        }

        protected override void DoRolBack()
        {
            try
            {
                Validations.EnabledControl(this, false);
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
                if (rdForignCurrency.Checked==false && rdLocalCurrency.Checked == false)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgMustEnterTypeCurrency);
                    return;
                }

                if (rdForignCurrency.Checked == true )
                {
                    if( Comon.cDec( txtTransPricing.Text)==0)
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgMustEnterTransPrice);
                        txtTransPricing.Focus();
                        return;
                    }
                }

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

                save();
                Validations.EnabledControl(this, false);
                if (IsNewRecord == true)
                    DoNew();
                else
                    Validations.DoSaveRipon(this, ribbonControl1);
                FillDataGrid();
            }
            catch (Exception ex)
            {
              //  Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void save()
        {
            CURRENCY_BO model = new CURRENCY_BO();
            model.ID = Comon.cInt(txtID.Text);
            //if (IsNewRecord == true)
            //    model.ID = 0;

            model.ARBNAME = txtArbName.Text;
            model.ENGNAME = txtEngName.Text;
            model.NOTES = txtNotes.Text;
            
            model.CurrncyPart = txtCurrncyPart.Text;
            model.CodeCurrency = txtCodeCurrency.Text;
            model.TransPricing = Comon.cDec(txtTransPricing.Text);
            model.MaxTransPricing = Comon.cDec(txtMaxTransPricing.Text);
            model.MinTransPricing = Comon.cDec(txtMinTransPricing.Text);

            if(ChkStoreCurrency.Checked==true)
            model.StoreCurrency = 1;
            else
            model.StoreCurrency = 0;

            if(rdLocalCurrency.Checked==true)
            model.TypeCurrency = 1;
            else
            model.TypeCurrency =2;
            model.TAFQEETID = Comon.cInt(cmbCurencyTafqeet.SelectedIndex);
            model.USERCREATED = UserInfo.ID;
            model.DATECREATED = Comon.cInt(Lip.GetServerDateSerial());
            model.TIMECREATED = Comon.cInt(Lip.GetServerTimeSerial());
            

            model.USERUPDATED = UserInfo.ID;
            model.DATEUPDATED = Comon.cInt(Lip.GetServerDateSerial());
            model.TIMEUPDATED = Comon.cInt(Lip.GetServerTimeSerial());

            model.BranchID = MySession.GlobalBranchID;
            model.FacilityID = UserInfo.FacilityID;
            model.ComputerInfo = UserInfo.ComputerInfo;
            model.EditComputerInfo = UserInfo.ComputerInfo;
         
            model.Cancel = 0;
            int SaveTrur=0; 
            SaveTrur = CURRENCY_DAL.InsertUpdate(model, IsNewRecord);

            if (IsNewRecord == true)
            {
                if (SaveTrur > 0)
                {
                  
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                    DoNew();
                    FillDataGrid();
                }
                else
                {
                    Messages.MsgWarning(Messages.TitleInfo, Messages.msgErrorSave);
                }
            }
            else
            {
                //if (SaveTrur > 0)
                //{
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                    FillDataGrid();
             
                //}
                //else
                //{
                //    Messages.MsgWarning(Messages.TitleInfo, Messages.msgErrorSave);
                //}
            }
            FillDataGrid();
        }
        protected override void DoPrint()
        {
            try
            {
                printableComponentLink1.CreateReportHeaderArea += printableComponentLink1_CreateReportHeaderArea;
                printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
                printableComponentLink1.Component = gridControl1;
                printableComponentLink1.ExportToPdf("1.pdf");
                Process.Start("1.pdf");
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        void printableComponentLink1_CreateReportHeaderArea(object sender, CreateAreaEventArgs e)
        {
        }

        protected override void DoDelete()
        {
            if (Comon.cInt(txtID.Text) != 1)
            {
                try
                {

                    if (IsNewRecord == true)
                        return;

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

                    CURRENCY_BO model = new CURRENCY_BO();
                    model.ID = Comon.cInt(txtID.Text);
                    model.USERUPDATED = UserInfo.ID;
                    model.DATEUPDATED = Comon.cInt(Lip.GetServerDateSerial());
                    model.TIMEUPDATED = Comon.cInt(Lip.GetServerTimeSerial());

                    model.BranchID = UserInfo.BRANCHID;
                    model.FacilityID = UserInfo.FacilityID;
                    model.EditComputerInfo = UserInfo.ComputerInfo;
                    model.Cancel = 0;

                    bool Result = CURRENCY_DAL.DeleteByID(model);
                    if (Result)
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    MoveRec(model.ID, xMovePrev);

                    FillDataGrid();

                }
                catch (Exception ex)
                {
                    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }
            }
            else
            {
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن حذف عملة الذهب " : "Can't Delete The Gold Currncy");
                return;
            }
        }

        private void EnabledControl(bool Value)
        {

            foreach (Control item in this.Controls)
            {
                if (item is TextEdit)
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

                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    ((SimpleButton)item).Enabled = Value;
                }


            }

        }
        #endregion
        /**********************Event**************************/
        #region Event
        private void rdForignCurrency_CheckedChanged(object sender, EventArgs e)
        {
            if (rdForignCurrency.Checked == true)
            {
                lblMaxPrice.Visible = true;
                lblMinPrice.Visible = true;
                lblTransPrice.Visible = true;
                txtMaxTransPricing.Visible = true;
                txtMinTransPricing.Visible = true;
                txtTransPricing.Visible = true;

            }
            else
            {
                lblMaxPrice.Visible = false;
                lblMinPrice.Visible = false;
                lblTransPrice.Visible = false;
                txtMaxTransPricing.Visible = false;
                txtMinTransPricing.Visible = false;
                txtTransPricing.Visible = false;
            }
        }

        private void rdLocalCurrency_CheckedChanged(object sender, EventArgs e)
        {
            if (rdLocalCurrency.Checked == true)
            {
                lblMaxPrice.Visible = false;
                lblMinPrice.Visible = false;
                lblTransPrice.Visible = false;
                txtMaxTransPricing.Visible = false;
                txtMinTransPricing.Visible = false;
                txtTransPricing.Visible = false;
            }
            else
            {
                lblMaxPrice.Visible = true;
                lblMinPrice.Visible = true;
                lblTransPrice.Visible = true;
                txtMaxTransPricing.Visible = true;
                txtMinTransPricing.Visible = true;
                txtTransPricing.Visible = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }
        private void txtID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempUserID;
                if (int.Parse(txtID.Text.Trim()) > 0)
                {

                    if (txtArbName.Text != string.Empty && IsNewRecord == true)
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "هل التراجع عن الاضافة");
                        if (!Yes)
                           return;
                    }

                    TempUserID = txtID.Text;
                    ClearFields();
                    txtID.Text = TempUserID;
                    if (cClass == null)
                        cClass = new CURRENCY_BO();
                    cClass = CURRENCY_DAL.GetByID(Comon.cInt(txtID.Text),MySession.GlobalBranchID,UserInfo.FacilityID);
                      
                    if (cClass == null)
                        return;

                    if (cClass.ID > 0)
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

        private void txtID_EditValueChanged(object sender, EventArgs e)
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
                txtID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtID_Validating(null, null);

            }
            catch (Exception)
            {
                return;
            }

        }
        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int rowIndex = e.RowHandle;
            txtID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
            txtID_Validating(null, null);
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
    }
}

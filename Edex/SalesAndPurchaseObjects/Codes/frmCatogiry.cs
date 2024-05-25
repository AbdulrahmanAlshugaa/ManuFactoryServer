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
using Edex.DAL.SalseSystem;
using DevExpress.XtraGrid.Views.Grid;
namespace Edex.SalesAndPurchaseObjects.Codes
{

    public partial class frmCategory : BaseForm
    {
        #region Declare
        private cSales_CustomerCategory cClass = new cSales_CustomerCategory();

        public string GetNewID;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;

        private string strSQL;
        private bool IsNewRecord;
        #endregion

        #region   Event
        public frmCategory()
        {

            InitializeComponent();
            /***************************Edit & Print & Export ****************************/
            //  this.Name =(UserInfo.Language == iLanguage.Arabic ? "شاشة المستخدمين" : "Sales_CustomerCategory");
            /*****************************************************************************/

            this.txtCategoryID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCategoryID_Validating);
            this.txtCategoryID.EditValueChanged += new System.EventHandler(this.txtCategoryID_EditValueChanged);
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);  
            strSQL = "ArbName";
            Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb"); 
        }

        /// <summary>
        /// This Event To Load form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUser_Load(object sender, EventArgs e)
        {
            FillGrid();
            DoNew();

        }
        

        private void txtCategoryID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string TempCategoryID;
                if (int.Parse(txtCategoryID.Text.Trim()) > 0)
                {
                    cClass.GetRecordSet(Comon.cInt(txtCategoryID.Text));
                    TempCategoryID = txtCategoryID.Text;
                    ClearFields();
                    txtCategoryID.Text = TempCategoryID;
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
        private void txtCategoryID_EditValueChanged(object sender, EventArgs e)
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
                txtCategoryID.Text = GridView.GetRowCellValue(rowIndex, GridView.Columns[0].FieldName).ToString();
                txtCategoryID_Validating(null, null); 

            }
            catch (Exception)
            {
                return;
            }
        }

        private void GridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

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
            try
            {
                strSQL = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم الفئة ] FROM " + cClass.TableName
                + " WHERE Cancel =0 and BranchID="+MySession.GlobalBranchID;

                if (UserInfo.Language == iLanguage.English)
                    strSQL = "SELECT  " + cClass.PremaryKey + " as ID, EngName as [Category Name] FROM " + cClass.TableName
               + " WHERE Cancel =0 and BranchID=" + MySession.GlobalBranchID;
              
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
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };

                /****************************stetement with Aribic Languague******************************/
                cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as الرقم, ArbName as [اسم المستخدم] FROM " + cClass.TableName
              + " WHERE Cancel =0 and BranchID=" + MySession.GlobalBranchID;

                /****************************stetement with English Languague******************************/
                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT  " + cClass.PremaryKey + " as ID, ArbName as [user Name] FROM " + cClass.TableName
                + " WHERE Cancel =0 and BranchID=" + MySession.GlobalBranchID;

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
                txtCategoryID.Text = cls.PrimaryKeyValue.ToString();
                txtCategoryID_Validating(null, null);
            }
        }
        public void ReadRecord()
        {
            try
            {
                IsNewRecord = false;
                ClearFields();
                {
                   
                   
                    txtCategoryID.Text = cClass.CategoryID.ToString();
                    txtArbName.Text = cClass.ArbName;
                    txtEngName.Text = cClass.EngName;
                    
                    txtNotes.Text = cClass.Notes;
                   
                    ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Item.Caption = txtCategoryID.Text + "/" + GridView.RowCount;
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
                txtCategoryID.Text = cClass.GetNewID().ToString();
                txtArbName.Text = " ";
                txtEngName.Text = " ";
                txtNotes.Text = " ";            
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
                string strSQL = "SELECT " + Code + " AS [الرقم]," + Name + "  AS [الاسم] FROM " + Tablename + " and BranchID=" + MySession.GlobalBranchID;
                if (OrderByField != "")
                    strSQL = strSQL + " Order By " + OrderByField;
                cmb.Properties.DataSource = Lip.SelectRecord(strSQL).DefaultView;
                cmb.Properties.DisplayMember = "الاسم";
                cmb.Properties.ValueMember = "الرقم";
            }
            catch { }
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
                MoveRec(Comon.cInt(txtCategoryID.Text), xMoveNext);

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
                MoveRec(Comon.cInt(txtCategoryID.Text), xMovePrev);
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
                Sales_CustomerCategory model = new Sales_CustomerCategory();
                model.CategoryID = Comon.cInt(txtCategoryID.Text);
                
                model.ArbName = txtArbName.Text;
                model.EngName = txtEngName.Text;  
                model.Notes = txtNotes.Text; ; 
                model.ComputerInfo = UserInfo.ComputerInfo; 
                model.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                model.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.Cancel = 0; 
                model.BranchID = Comon.cInt(MySession.GlobalBranchID);
                model.FacilityID = Comon.cInt(MySession.GlobalFacilityID);
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
                    
                }
                // model.AddByCategoryID = UserInfo.ID;
                int Result = Sales_CustomerCategoryDAL.InsertUser(model, IsNewRecord);
                SplashScreenManager.CloseForm(false);
                if (IsNewRecord == true)
                {
                    if (Result > 0)
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

                int TempID = Comon.cInt(txtCategoryID.Text);

                Sales_CustomerCategory model = new Sales_CustomerCategory();
                model.CategoryID = Comon.cInt(txtCategoryID.Text);
                model.EditUserID = UserInfo.ID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                model.BranchID = Comon.cInt(MySession.GlobalBranchID);
                model.FacilityID = Comon.cInt(MySession.GlobalFacilityID);

                bool Result = Sales_CustomerCategoryDAL.DeleteUser(model);
                if (Result == true)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);

                MoveRec(model.CategoryID, xMovePrev);
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



    }
}
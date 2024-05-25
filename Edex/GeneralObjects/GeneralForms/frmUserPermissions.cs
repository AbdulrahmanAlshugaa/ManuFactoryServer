using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTab;
using Edex.DAL.UsersManagement;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmUserPermissions : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        private string PrimaryName;
        GridColumn ColFromName;
        private string strSQL;
        private string CaptionFromView;
        private string CaptionFromAdd;
        private string CaptionFromDelete;
        private string CaptionFromUpdate;
        private string CaptionDaysAllowedForEdit;

        private string CaptionReport;
        private string CaptionReportView;
        private string CaptionReportExport;
        private string CaptionShowReportInReportViewer;

        private string CaptionMenu;
        private string CaptionMenuView;
        private static bool IsFirstTime = true;
        public frmUserPermissions()
        {
            try
            {
                if (this.IsDisposed)
                    return;

                IsFirstTime = true;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                PrimaryName = "ArbName";
                CaptionFromView = "عـــرض";
                CaptionFromAdd = "إضافـــة";
                CaptionFromDelete = "حـــذف";
                CaptionFromUpdate = "تعــديـل";
                CaptionDaysAllowedForEdit = "سماحية التعديل بالأيام";

                CaptionReport = "أسم التقرير";
                CaptionReportView = "عــرض";
                CaptionReportExport = "تصــدبـر";
                CaptionShowReportInReportViewer = "معاينة قبل الطباعة";

                CaptionMenu = "أسم القائمة";
                CaptionMenuView = "عــرض";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    PrimaryName = "EngName";
                    CaptionFromView = "From View";
                    CaptionFromAdd = "From Add ";
                    CaptionFromDelete = "From Delete";
                    CaptionFromUpdate = "From Update";
                    CaptionDaysAllowedForEdit = "Days Allowed For Edit";

                    CaptionReport = "Report Name";
                    CaptionReportView = "Report View";
                    CaptionReportExport = "Report Export";
                    CaptionShowReportInReportViewer = "Review Before Print";

                    CaptionMenu = "Menu Report";
                    CaptionMenuView = "Menu View";

                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");
                }
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                //cmbUsersID.Enabled = false;
                InitGrid();
                /***************************** Event  Gridview *****************************/
                dgvForms.InitNewRow += dgvForms_InitNewRow;
                dgvForms.RowCellStyle += dgvForms_RowCellStyle;

                dgvReports.InitNewRow += dgvReports_InitNewRow;
                dgvReports.RowCellStyle += dgvReports_RowCellStyle;

                dgvMenus.InitNewRow += dgvMenus_InitNewRow;
                dgvMenus.RowCellStyle += dgvMenus_RowCellStyle;

                dgvForms.ValidatingEditor+=dgvForms_ValidatingEditor;
              
                this.dgvReports.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.dgvReports_ValidatingEditor);
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

        public frmUserPermissions(int BranchID,int userID)
        {
            try
            {
                IsFirstTime = true;
                //SplashScreenManager.CloseForm(false);
                //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                PrimaryName = "ArbName";
                CaptionFromView = "عـــرض";
                CaptionFromAdd = "إضافـــة";
                CaptionFromDelete = "حـــذف";
                CaptionFromUpdate = "تعــديـل";
                CaptionDaysAllowedForEdit = "سماحية التعديل بالأيام";

                CaptionReport = "أسم التقرير";
                CaptionReportView = "عــرض";
                CaptionReportExport = "تصــدبـر";
                CaptionShowReportInReportViewer = "معاينة قبل الطباعة";

                CaptionMenu = "أسم القائمة";
                CaptionMenuView = "عــرض";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    PrimaryName = "EngName";
                    CaptionFromView = "From View";
                    CaptionFromAdd = "From Add ";
                    CaptionFromDelete = "From Delete";
                    CaptionFromUpdate = "From Update";
                    CaptionDaysAllowedForEdit = "Days Allowed For Edit";

                    CaptionReport = "Report Name";
                    CaptionReportView = "Report View";
                    CaptionReportExport = "Report Export";
                    CaptionShowReportInReportViewer = "Review Before Print";

                    CaptionMenu = "Menu Report";
                    CaptionMenuView = "Menu View";

                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");
                }
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                cmbUsersID.Enabled = false;
                InitGrid();
                /***************************** Event  Gridview *****************************/
                dgvForms.InitNewRow += dgvForms_InitNewRow;
                dgvForms.RowCellStyle += dgvForms_RowCellStyle;

                dgvReports.InitNewRow += dgvReports_InitNewRow;
                dgvReports.RowCellStyle += dgvReports_RowCellStyle;

                dgvMenus.InitNewRow += dgvMenus_InitNewRow;
                dgvMenus.RowCellStyle += dgvMenus_RowCellStyle;
                cmbBranchesID.EditValue = MySession.GlobalBranchID;
                cmbUsersID.EditValue = MySession.UserID;
               // SplashScreenManager.CloseForm(false);
                ReadFormsPermissions();
                SaveFormsPermissions(BranchID, userID);
                ReadReportPermissions();
                SaveReportsPermissions(BranchID, userID);
                ReadMenuPermissions();
                SaveMenusPermissions(BranchID, userID);

                ReadOtherPermissions();
                SaveOtherPermissions(BranchID, userID);

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









        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit obj = (LookUpEdit)sender;
            cmbUsersID.Enabled = true;
            FillCombo.FillComboBoxLookUpEdit(cmbUsersID, "Users", "UserID", PrimaryName, "", " BranchID = " + obj.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select User" : "حدد المستخدم"));
        }
        #region GridView
        private void InitDataTable()
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ColFormName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ColArbCaption", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ColEngCaption", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ColMenuName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ColFormView", System.Type.GetType("System.Boolean"));
            dtItem.Columns.Add("ColFormAdd", System.Type.GetType("System.Boolean"));
            dtItem.Columns.Add("ColFormDelete", System.Type.GetType("System.Boolean"));
            dtItem.Columns.Add("ColFormUpdate", System.Type.GetType("System.Boolean"));
            dtItem.Columns.Add("ColDaysAllowedForEdit", System.Type.GetType("System.Int32"));
            dgcForms.DataSource = dtItem;

            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("ColReportName", System.Type.GetType("System.String"));
            dtReport.Columns.Add("ColReportCaption", System.Type.GetType("System.String"));
            dtReport.Columns.Add("ColReportView", System.Type.GetType("System.Boolean"));
            dtReport.Columns.Add("ColReportExport", System.Type.GetType("System.Boolean"));
            dtReport.Columns.Add("ColShowReportInReportViewer", System.Type.GetType("System.Boolean"));
            dgcReports.DataSource = dtReport;

            DataTable dtMenus = new DataTable();
            dtMenus.Columns.Add("ColMenuName", System.Type.GetType("System.String"));
            dtMenus.Columns.Add("ColMenuCaption", System.Type.GetType("System.String"));
            dtMenus.Columns.Add("ColMenuView", System.Type.GetType("System.Boolean"));
            dgcMenus.DataSource = dtMenus;


        }
        private void InitGrid()
        {
            InitDataTable();
            dgvForms.Columns["ColArbCaption"].Visible = false;
            dgvForms.Columns["ColEngCaption"].Visible = false;

            dgvForms.Columns["ColFormName"].Visible = false;

            dgvReports.Columns["ColReportName"].Visible = false;

            dgvMenus.Columns["ColMenuName"].Visible = false;

            if (PrimaryName == "ArbName")
            {
                ColFromName = dgvForms.Columns["ColArbCaption"];
                dgvForms.Columns["ColArbCaption"].Visible = true;
            }
            else
            {
                ColFromName = dgvForms.Columns["ColEngCaption"];
                dgvForms.Columns["ColEngCaption"].Visible = true;
            }
            dgvForms.Columns["ColFormView"].Caption = CaptionFromView;
            dgvForms.Columns["ColFormAdd"].Caption = CaptionFromAdd;
            dgvForms.Columns["ColFormDelete"].Caption = CaptionFromDelete;
            dgvForms.Columns["ColFormUpdate"].Caption = CaptionFromUpdate;
            dgvForms.Columns["ColDaysAllowedForEdit"].Caption = CaptionDaysAllowedForEdit;

            var RepositoryItemSpinEdit = new RepositoryItemSpinEdit();
            RepositoryItemSpinEdit.ParseEditValue += RepositoryItemSpinEdit_ParseEditValue;
            dgvForms.Columns["ColDaysAllowedForEdit"].ColumnEdit = RepositoryItemSpinEdit;

            dgvReports.Columns["ColReportCaption"].Caption = CaptionReport;
            dgvReports.Columns["ColReportView"].Caption = CaptionFromView;
            dgvReports.Columns["ColReportExport"].Caption = CaptionReportExport;
            dgvReports.Columns["ColShowReportInReportViewer"].Caption = CaptionShowReportInReportViewer;

            dgvMenus.Columns["ColMenuCaption"].Caption = CaptionMenu;
            dgvMenus.Columns["ColMenuView"].Caption = CaptionMenuView;

        }
        private void RepositoryItemSpinEdit_ParseEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
        {
            var value = e.Value;
            var decimalValue = Convert.ToInt32(value);
            if (decimalValue < 0)
            {
                e.Handled = true;
                e.Value = null;
            }
        }
        private void dgvForms_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColFormName"], "");
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColArbCaption"], "");
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColEngCaption"], "");
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColMenuName"], "");
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColFormView"], false);
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColFormAdd"], false);
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColFormDelete"], false);
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColFormUpdate"], false);
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColDaysAllowedForEdit"], 0);
        }
        private void dgvForms_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView gv = sender as GridView;
            // Option 1: use the GridView.GetRowCellValue method to obtain cell values
            if (gv.GetRowCellValue(e.RowHandle, "ColFormName") != null && gv.GetRowCellValue(e.RowHandle, "ColFormName").ToString() == "")
            {
                e.Appearance.BackColor = Color.Chocolate;
                e.Appearance.ForeColor = Color.White;

                return;
            }
        }
        private void dgvReports_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, dgvReports.Columns["ColReportCaption"], "");
            view.SetRowCellValue(e.RowHandle, dgvReports.Columns["ColReportView"], false);
            view.SetRowCellValue(e.RowHandle, dgvReports.Columns["ColReportExport"], false);
            view.SetRowCellValue(e.RowHandle, dgvReports.Columns["ColShowReportInReportViewer"], false);

        }
        private void dgvReports_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                GridView gv = sender as GridView;
                // Option 1: use the GridView.GetRowCellValue method to obtain cell values
                if (gv != null && gv.GetRowCellValue(e.RowHandle, "ColReportName") != null && (string)gv.GetRowCellValue(e.RowHandle, "ColReportName") == "")
                {
                    e.Appearance.BackColor = Color.Chocolate;
                    e.Appearance.ForeColor = Color.White;

                    return;
                }

            }
            catch (Exception ex)
            {
                //SplashScreenManager.CloseForm(false);
                //Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }

        }
        private void dgvMenus_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, dgvMenus.Columns["ColMenuCaption"], "");
            view.SetRowCellValue(e.RowHandle, dgvMenus.Columns["ColMenuView"], false);
        }
        private void dgvMenus_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

            try
            {
                GridView gv = sender as GridView;
                // Option 1: use the GridView.GetRowCellValue method to obtain cell values
                if (gv != null && gv.GetRowCellValue(e.RowHandle, "ColMenuName") != null && (string)gv.GetRowCellValue(e.RowHandle, "ColMenuName") == "")
                {
                    e.Appearance.BackColor = Color.Chocolate;
                    e.Appearance.ForeColor = Color.White;

                    return;
                }
            }
            catch (Exception ex)
            {
                //SplashScreenManager.CloseForm(false);
                //Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }


        }
        #endregion

        #region ReadPermissions
        void ReadFormsPermissions()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                int rowHandle = 0;
                string MenuName = "";
                DataTable dtForms;
                DataTable dtFormPermission;
                DataTable dtMenus;
                for (int i = 0; i < dgvForms.RowCount; )
                    dgvForms.DeleteRow(i);
                strSQL = ("SELECT dbo.Forms.FormName, dbo.Forms.ArbCaption, dbo.Forms.EngCaption, dbo.Forms.MenuName" + (" FROM dbo.Forms LEFT OUTER JOIN" + (" dbo.Menus ON dbo.Forms.MenuName = dbo.Menus.MenuName" + " Where Menus.IsClientPurchaseIt=1 And Forms.IsClientPurchaseIt=1 order by MenuName")));
                dtForms = Lip.SelectRecord(strSQL);
                for (int i = 0; (i <= (dtForms.Rows.Count - 1)); i++)
                {
                    strSQL = ("SELECT FormView, FormAdd, FormDelete, FormUpdate, DaysAllowedForEdit FROM UserFormsPermissions" + (" Where BranchID =" + (cmbBranchesID.EditValue.ToString() + (" And UserID=" + (cmbUsersID.EditValue.ToString() + (" And FormName='" + (dtForms.Rows[i]["FormName"].ToString() + "'")))))));
                    dtFormPermission = Lip.SelectRecord(strSQL);
                    if ((MenuName != dtForms.Rows[i]["MenuName"].ToString()))
                    {
                        dgvForms.AddNewRow();

                        if ((UserInfo.Language == iLanguage.Arabic))
                        {
                            strSQL = ("Select Top 1 ArbCaption as MenuCaption From Menus Where MenuName='" + (dtForms.Rows[i]["MenuName"].ToString() + "'"));
                        }
                        else
                        {
                            strSQL = ("Select Top 1 EngCaption as MenuCaption From Menus Where MenuName='" + (dtForms.Rows[i]["MenuName"].ToString() + "'"));
                        }

                        // strSQL = "Select Top 1 ArbCaption as MenuCaption From Menus Where MenuName='" & dtForms.Rows(i)("MenuName") & "'"
                        dtMenus = Lip.SelectRecord(strSQL);
                        rowHandle = dgvForms.GetRowHandle(dgvForms.DataRowCount);
                        dgvForms.SetRowCellValue(rowHandle, "ColFormName", "");
                        dgvForms.SetRowCellValue(rowHandle, ColFromName, dtMenus.Rows[0]["MenuCaption"].ToString());
                        MenuName = dtForms.Rows[i]["MenuName"].ToString();

                    }

                    dgvForms.AddNewRow();
                    rowHandle = dgvForms.GetRowHandle(dgvForms.DataRowCount);
                    dgvForms.SetRowCellValue(rowHandle, "ColFormName", dtForms.Rows[i]["FormName"].ToString());
                    dgvForms.SetRowCellValue(rowHandle, ColFromName, ((UserInfo.Language == iLanguage.Arabic) ? dtForms.Rows[i]["ArbCaption"].ToString() : dtForms.Rows[i]["EngCaption"].ToString()));
                    if ((dtFormPermission.Rows.Count > 0))
                    {

                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormView"], Comon.cbool(dtFormPermission.Rows[0]["FormView"]));
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormAdd"], Comon.cbool(dtFormPermission.Rows[0]["FormAdd"]));
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormUpdate"], Comon.cbool(dtFormPermission.Rows[0]["FormUpdate"]));
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormDelete"], Comon.cbool(dtFormPermission.Rows[0]["FormDelete"]));
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColDaysAllowedForEdit"], Comon.cInt(dtFormPermission.Rows[0]["DaysAllowedForEdit"]));
                    }
                    else
                    {
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormView"], false);
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormAdd"], false);
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormUpdate"], false);
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormDelete"], false);
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColDaysAllowedForEdit"], "0");

                    }
                    this.dgvForms.FocusedRowHandle = 0;

                }

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
        void ReadReportPermissions()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                DataTable dtReport;
                DataTable dtReportPermission;
                string MenuName = "";
                DataTable dtMenus;
                int rowHandle = 0;
                for (int i = 0; i < dgvReports.RowCount; )
                    dgvReports.DeleteRow(i);
                strSQL = ("SELECT dbo.Reports.ReportName, dbo.Reports.ArbCaption, dbo.Reports.EngCaption, dbo.Reports.MenuName" + (" FROM dbo.Reports LEFT OUTER JOIN" + (" dbo.Menus ON dbo.Reports.MenuName = dbo.Menus.MenuName" + " Where Menus.IsClientPurchaseIt=1 And Reports.IsClientPurchaseIt=1 order by MenuName")));
                dtReport = Lip.SelectRecord(strSQL);
                for (int i = 0; (i <= (dtReport.Rows.Count - 1)); i++)
                {
                    strSQL = ("SELECT ReportName, ReportView,ReportExport,ShowReportInReportViewer  FROM UserReportsPermissions" + (" Where BranchID =" + (cmbBranchesID.EditValue.ToString() + (" And UserID=" + (cmbUsersID.EditValue.ToString() + (" And ReportName='" + (dtReport.Rows[i]["ReportName"].ToString() + "'")))))));
                    dtReportPermission = Lip.SelectRecord(strSQL);
                    if ((MenuName != dtReport.Rows[i]["MenuName"].ToString()))
                    {

                        // strSQL = "Select Top 1 ArbCaption as MenuCaption From Menus Where MenuName='" & dtReport.Rows(i)("MenuName") & "'"
                        if ((UserInfo.Language == iLanguage.Arabic))
                        {
                            strSQL = ("Select Top 1 ArbCaption as MenuCaption From Menus Where MenuName='" + (dtReport.Rows[i]["MenuName"].ToString() + "'"));
                        }
                        else
                        {
                            strSQL = ("Select Top 1 EngCaption as MenuCaption From Menus Where MenuName='" + (dtReport.Rows[i]["MenuName"].ToString() + "'"));
                        }
                        dtMenus = Lip.SelectRecord(strSQL);
                        dgvReports.AddNewRow();
                        rowHandle = dgvReports.GetRowHandle(dgvReports.DataRowCount);
                        dgvReports.SetRowCellValue(rowHandle, "ColReportName", "");
                        dgvReports.SetRowCellValue(rowHandle, "ColReportCaption", dtMenus.Rows[0]["MenuCaption"].ToString());
                        MenuName = dtReport.Rows[i]["MenuName"].ToString();

                    }

                    dgvReports.AddNewRow();
                    rowHandle = dgvReports.GetRowHandle(dgvReports.DataRowCount);
                    dgvReports.SetRowCellValue(rowHandle, "ColReportName", dtReport.Rows[i]["ReportName"].ToString());
                    dgvReports.SetRowCellValue(rowHandle, "ColReportCaption", ((UserInfo.Language == iLanguage.Arabic) ? dtReport.Rows[i]["ArbCaption"].ToString() : dtReport.Rows[i]["EngCaption"].ToString()));

                    if ((dtReportPermission.Rows.Count > 0))
                    {
                        dgvReports.SetRowCellValue(rowHandle, dgvReports.Columns["ColReportView"], Comon.cbool(dtReportPermission.Rows[0]["ReportView"]));
                        dgvReports.SetRowCellValue(rowHandle, dgvReports.Columns["ColReportExport"], Comon.cbool(dtReportPermission.Rows[0]["ReportExport"]));
                        dgvReports.SetRowCellValue(rowHandle, dgvReports.Columns["ColShowReportInReportViewer"], Comon.cbool(dtReportPermission.Rows[0]["ShowReportInReportViewer"]));
                    }
                    else
                    {
                        dgvReports.SetRowCellValue(rowHandle, dgvReports.Columns["ColReportView"], false);
                        dgvReports.SetRowCellValue(rowHandle, dgvReports.Columns["ColReportExport"], false);
                        dgvReports.SetRowCellValue(rowHandle, dgvReports.Columns["ColShowReportInReportViewer"], false);
                    }
                    this.dgvReports.FocusedRowHandle = 0;
                }

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
        void ReadOtherPermissions()
        {
            Application.DoEvents();
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            try
            {
               
                /******************Public********************/
                FillCombo.FillComboBoxLookUpEdit(cboCostPricesTypes, "SalePricesTypes", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBoxLookUpEdit(cboSalePricesTypes, "CostPricesTypes", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSellerID, "Sales_Sellers", "SellerID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Seller" : "حدد البائع "));
                FillCombo.FillComboBoxLookUpEdit(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select StatUs" : "حدد الحالة "));
                cmbStatus.EditValue = 2;
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDebitAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  And BranchID= " + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));

                FillCombo.FillComboBox(cboDefaultFormPrintingID, "FormPrinting", "FormID", PrimaryName, "", " 1=1 ");
                //FillCombo.FillComboBox(cboDefaultPurchaseFormPrintingID, "FormPrinting", "FormID", PrimaryName, "", " 1=1 ");
                //FillCombo.FillComboBox(cboDefaultPurchaseReturnFormPrintingID, "FormPrinting", "FormID", PrimaryName, "", " 1=1 ");
                //FillCombo.FillComboBox(cboDefaultSaleFormPrintingID, "FormPrinting", "FormID", PrimaryName, "", " 1=1 ");
                //FillCombo.FillComboBox(cboDefaultSaleReturnFormPrintingID, "FormPrinting", "FormID", PrimaryName, "", " 1=1 ");
                /****************** Sale  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSalePayMethodID, "Sales_PurchaseMethods", "MethodID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Method " : "حدد طريقة الدفع"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleNetTypeID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
               
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleCustomerID, "Sales_Customers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Customer" : "حدد العميل "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleCreditAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+" AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleDebitAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSalesAddtionalAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));               
               
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSalesRevenueAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+" AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCostSalseAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+" AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDiscountSalseAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleDelegateID, "Sales_SalesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Delegate ID" : " حدد المندوب  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleSellerID, "Sales_Sellers", "SellerID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Seller ID" : " حدد البائع  "));
                /****************** Sale Return **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnPayMethodID, "Sales_PurchaseMethods", "MethodID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Method " : "حدد طريقة الدفع"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnNetTypeID, "NetType", "NetTypeID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Net Type" : "حدد نوع الشبكة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnCustomerID, "Sales_Customers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Customer" : "حدد العميل "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnCreditAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnDelegateID, "Sales_SalesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Delegate ID" : " حدد المندوب  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSaleReturnSellerID, "Sales_Sellers", "SellerID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Seller ID" : " حدد البائع  "));
                /******************order Sale  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderSaleCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderSaleStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderSaleCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderSaleCustomerID, "Sales_Customers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Customer" : "حدد العميل "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderSaleDelegateID, "Sales_PurchasesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Delegate ID" : " حدد المندوب  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderSaleSellerID, "Sales_Sellers", "SellerID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Seller ID" : " حدد البائع  "));


                /****************** Purchase  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchasePayMethodID, "Sales_PurchaseMethods", "MethodID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Method " : "حدد طريقة الدفع"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cobDefaultPurchaseSupplierID, "Sales_Suppliers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Supplier" : "حدد المورد "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseDebitAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseDiscountAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));               
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseAddtionalAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString()+" AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseCrditAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseDelegateID, "Sales_PurchasesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Delegate ID" : " حدد المندوب  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseNetTypeID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
               

                /****************** Purchase Return **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseReturnPayMethodID, "Sales_PurchaseMethods", "MethodID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Method " : "حدد طريقة الدفع"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseReturnCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                 FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseReturnNetTypeID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString()+"  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
               
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseReturnStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseReturnCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cobDefaultPurchaseReturnSupplierID, "Sales_Suppliers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Supplier" : "حدد المورد "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseReturnDebitAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString()+" AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseReturnCrditAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPurchaseReturnDelegateID, "Sales_PurchasesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Delegate ID" : " حدد المندوب  "));

                /****************** Order Purchase  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderPurchaseCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND  BranchID="+cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderPurchaseCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderPurchaseSupplierID, "Sales_Suppliers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Supplier" : "حدد المورد "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderPurchaseStoreID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOrderPurchaseDelegateID, "Sales_PurchasesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Delegate ID" : " حدد المندوب  "));
                
                /****************** Receipt Voucher  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultReceiptVoucherCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND  BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultReceiptVoucherCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultReceiptVoucherSalesDelegateID, "Sales_SalesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select  " : "حدد  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultReceiptVoucherDebitAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultReceiptVoucherIntermediateDiamondAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultReceiptVoucherIntermediateGoldAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));         
                
                /****************** Spend Voucher  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSpendVoucherCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSpendVoucherCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSpendVoucherCrditAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));         
                FillCombo.FillComboBoxLookUpEdit(cboDefaultSpendVoucherPurchasesDelegateID, "Sales_PurchasesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select  " : "حدد  "));

                /****************** OpeningVoucher **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOpeningVoucherCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultOpeningVoucherCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                /******************Check Receipt Voucher  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckReceiptVoucherCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckReceiptVoucherCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckReceiptVoucherDebitAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));         
               
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckReceiptVoucherSalesDelegateID, "Sales_SalesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select  " : "حدد  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckReceiptVoucherBankID, "Acc_Banks", "ID", PrimaryName, "", "  BranchID=" + cmbBranchesID.EditValue.ToString());
                /******************Check Spend Voucher  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckSpendVoucherCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckSpendVoucherCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckSpendVoucherCrditAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels + " AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));         
               
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckSpendVoucherPurchasesDelegateID, "Sales_PurchasesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select  " : "حدد  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCheckSpendVoucherBankID, "Acc_Banks", "ID", PrimaryName, "", " BranchID=" + cmbBranchesID.EditValue.ToString());

                /****************** Various Voucher  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultVariousVoucherCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultVariousVoucherCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultVariousVoucherSalesDelegateID, "Sales_SalesDelegate", "DelegateID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select  " : "حدد  "));

                /******************Items Out On Bail  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultItemsOutOnBailCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultItemsOutOnBailStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultItemsOutOnBailCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                 FillCombo.FillComboBoxLookUpEdit(cboDefaultItemsOutOnBailCustomerID, "Sales_Customers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Customer" : "حدد العميل "));
               
                /******************Gold Multi Transfer  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultGoldMultiTransferCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultGoldMultiTransferStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultGoldMultiTransferCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));

                /******************Matirial Multi Transfer  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialMultiTransferCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialMultiTransferStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialMultiTransferCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                 

                /******************Items in On Bail  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultItemsInOnBailCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultItemsInOnBailStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultItemsInOnBailCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultItemsInOnBailSupplierID, "Sales_Suppliers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Supplier" : "حدد المورد "));

                /******************Matirial in On Bail  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialInOnBailCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialInOnBailStoreAccountID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialInOnBailCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialInOnBailSupplierID, "Sales_Suppliers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Supplier" : "حدد المورد "));

                /******************Matirial Out On Bail  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialOutOnBailCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialOutOnBailStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialOutOnBailCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                  FillCombo.FillComboBoxLookUpEdit(cboDefaultMatirialOutOnBailSupplierID, "Sales_Suppliers", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Supplier" : "حدد المورد "));

                /******************Goods Opening  **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultGoodsOpeningCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaulGoodsOpeningStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المستودع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultGoodsOpeningCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaulGoodsOpeningCrditAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaulGoodsOpeningDebitAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));


                FillCombo.FillComboBoxLookUpEdit(cboDefaultFatherBanksAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" +( (MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultFatherBoxesAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));

                FillCombo.FillComboBoxLookUpEdit(cboDefaultFatherStoreAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultFatherCustomerAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultFatherSupplierAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultFatherEmployeeAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + ((MySession.GlobalNoOfLevels > 4) ? (Comon.cInt(MySession.GlobalNoOfLevels) - 2) : (Comon.cInt(MySession.GlobalNoOfLevels) - 1)), (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));

                /******************Wax Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultWaxCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultWaxBeforeStoreAccontID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultWaxAfterStoreAccontID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultWaxCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultWaxEmployeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));

                /******************Cad Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCadCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCadBeforeStoreAccontID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCadAfterStoreAccontID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCadCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCadEmpolyeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));
                /******************Zircon Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultZirconCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultZirconBeforeStoreAccontID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultZirconAfterStoreAccontID , "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultZirconCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultZirconEmpolyeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));
                /******************Diamond Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDiamondCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDiamondBeforeStoreAccontID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDiamondAfterStoreAccontID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن  "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDiamondCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDiamondEmpolyeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));
                /******************Afforstation Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAfforstationCurencyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAfforstationBeforeStoreAccontID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAfforstationAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAfforstationCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAfforstationBeforeEmpolyeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAfforstationAfterEmpolyeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));

                /******************Restrction Order **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultTypeOrderRestrectionID , "Menu_TypeOrderRestrction", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select ID" : "حدد النوع "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultTypeMatirialOrderRestrectionID, "Menu_TypeMaterialsOrderRestrction", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  النوع"));
               

                /******************Casting Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCastingCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCastingStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCastingAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCastingCostCenterID, "Acc_CostCenters", "CostCenterID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Cost Center" : "حدد مركز التكلفة"));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCastingEmployeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));

                /******************Manufactory Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultManufactoryCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultManufactoryStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultManufactoryAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                 FillCombo.FillComboBoxLookUpEdit(cboDefaultManufatoryEmployeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));

                /******************Commpound Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCommpundCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCompoundStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultCompoundAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                 FillCombo.FillComboBoxLookUpEdit(cboDefaultCompoundEmployeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));

                /******************prntage Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPrntageCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPrntageStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPrntageAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPrntage2StoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPrntage2AccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPrntageEmployeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));

                /******************Polishn  Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPolishinCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPolishinStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPolishinAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPolishin2StoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPolishin2AccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "  AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));

                FillCombo.FillComboBoxLookUpEdit(cboDefaultPolishin3StoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPolishin3AccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + " AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
               
                FillCombo.FillComboBoxLookUpEdit(cboDefaultPolishnEmployeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));

                /******************Addtional  Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAddtionalCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAddtionalStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultAddtionalAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0  AND BranchID=" + cmbBranchesID.EditValue.ToString() + " AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                 FillCombo.FillComboBoxLookUpEdit(cboDefaultAddtionalEmployeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));

                /******************Dismant  Fictory **************************/
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDismantageCurrncyID, "Acc_Currency", "ID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Currency " : "حدد العملة "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDismantageStoreID, "Stc_Stores", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Store" : "حدد المخزن "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDismantageAccountID, "Acc_Accounts", "AccountID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString() + "   AND AccountLevel=" + MySession.GlobalNoOfLevels, (UserInfo.Language == iLanguage.English ? "Select Account" : "حدد الحساب "));
                FillCombo.FillComboBoxLookUpEdit(cboDefaultDismantageEmployeeID, "HR_EmployeeFile", "EmployeeID", PrimaryName, "", "Cancel =0 AND BranchID=" + cmbBranchesID.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select Employee" : "حدد  العامل"));
               


                IsFirstTime = false;

                /*******************************************************************************************************/
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);

                List<UserOtherPermissions> listUserOtherPermissions = new List<UserOtherPermissions>();
                strSQL = "SELECT  [OtherPermissionName] ,[OtherPermissionValue],[OtherPermissionIndex] FROM [UserOtherPermissions] where  [FacilityID] =" + MySession.GlobalFacilityID + " and [UserID]=" + SelectedUserID + " and [BranchID]=" + SelectedBranchID;
                DataTable dtOther = Lip.SelectRecord(strSQL);

                foreach (Control item in tabOthers.Controls)
                {
                    if (item is SpinEdit)
                    {
                        foreach (DataRow row in dtOther.Rows)
                        {
                            string name = row["OtherPermissionName"].ToString();
                            if (item.Name.Substring(3) == name)
                            {
                                ((SpinEdit)item).Text = (row["OtherPermissionValue"] == null ? "0" : row["OtherPermissionValue"].ToString());
                                break;
                                // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                            }
                        }
                    }
                    else if (item is LookUpEdit)
                    {
                        foreach (DataRow row in dtOther.Rows)
                        {
                            string name = row["OtherPermissionName"].ToString();
                            int value = Comon.cInt(row["OtherPermissionIndex"].ToString());
                            if (item.Name.Substring(3) == name)
                            {
                                if (row["OtherPermissionValue"].ToString().Length > int.MaxValue.ToString().Length)
                                {
                                    ((LookUpEdit)item).EditValue = Comon.cDbl(row["OtherPermissionValue"].ToString());
                                }
                                else
                                    ((LookUpEdit)item).EditValue = Comon.cInt(row["OtherPermissionValue"].ToString());

                                break;
                            }
                        }
                    }
                    else if (item is TextEdit)
                    {
                        foreach (DataRow row in dtOther.Rows)
                        {
                            string name = row["OtherPermissionName"].ToString();
                            if (item.Name.Substring(3) == name)
                            {
                                ((TextEdit)item).Text = (row["OtherPermissionValue"] == null ? "0" : row["OtherPermissionValue"].ToString());
                                break;
                                // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                            }
                        }
                    }
                    else if (item is ToggleSwitch)
                    {
                        foreach (DataRow row in dtOther.Rows)
                        {
                            string name = row["OtherPermissionName"].ToString();
                            if (item.Name.Substring(3) == name)
                            {
                                ((ToggleSwitch)item).EditValue = (row["OtherPermissionValue"].ToString() == "True" ? true : false);
                                break;
                                // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                            }
                        }
                    }


                }

                foreach (XtraTabPage page in tabControlSpecific.TabPages)
                {

                    foreach (Control item in page.Controls)
                    {

                        if (item is SpinEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((SpinEdit)item).Text = (row["OtherPermissionValue"] == null ? "0" : row["OtherPermissionValue"].ToString());
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }
                        else if (item is LookUpEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();

                                if (item.Name.Substring(3) == name)
                                {

                                    if (row["OtherPermissionValue"].ToString().Length > int.MaxValue.ToString().Length)
                                    {
                                        ((LookUpEdit)item).EditValue = Comon.cDbl(row["OtherPermissionValue"].ToString());
                                    }
                                    else
                                        ((LookUpEdit)item).EditValue = Comon.cInt(row["OtherPermissionValue"].ToString());

                                    break;
                                }
                            }
                        }
                        else if (item is TextEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((TextEdit)item).Text = (row["OtherPermissionValue"] == null ? "0" : row["OtherPermissionValue"].ToString());
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }
                        else if (item is ToggleSwitch)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((ToggleSwitch)item).EditValue = (row["OtherPermissionValue"].ToString() == "True" ? true : false);
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }


                    }

                }

                foreach (XtraTabPage page in tabControlPreparation.TabPages)
                {

                    foreach (Control item in page.Controls)
                    {

                        if (item is SpinEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((SpinEdit)item).Text = (row["OtherPermissionValue"] == null ? "0" : row["OtherPermissionValue"].ToString());
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }
                        else if (item is LookUpEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();

                                if (item.Name.Substring(3) == name)
                                {

                                    if (row["OtherPermissionValue"].ToString().Length > int.MaxValue.ToString().Length)
                                    {
                                        ((LookUpEdit)item).EditValue = Comon.cDbl(row["OtherPermissionValue"].ToString());
                                    }
                                    else
                                        ((LookUpEdit)item).EditValue = Comon.cInt(row["OtherPermissionValue"].ToString());

                                    break;
                                }
                            }
                        }
                        else if (item is TextEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((TextEdit)item).Text = (row["OtherPermissionValue"] == null ? "0" : row["OtherPermissionValue"].ToString());
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }
                        else if (item is ToggleSwitch)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((ToggleSwitch)item).EditValue = (row["OtherPermissionValue"].ToString() == "True" ? true : false);
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }


                    }

                }
                foreach (XtraTabPage page in tabControlManufactory.TabPages)
                {

                    foreach (Control item in page.Controls)
                    {

                        if (item is SpinEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((SpinEdit)item).Text = (row["OtherPermissionValue"] == null ? "0" : row["OtherPermissionValue"].ToString());
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }
                        else if (item is LookUpEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();

                                if (item.Name.Substring(3) == name)
                                {

                                    if (row["OtherPermissionValue"].ToString().Length > int.MaxValue.ToString().Length)
                                    {
                                        ((LookUpEdit)item).EditValue = Comon.cDbl(row["OtherPermissionValue"].ToString());
                                    }
                                    else
                                        ((LookUpEdit)item).EditValue = Comon.cInt(row["OtherPermissionValue"].ToString());

                                    break;
                                }
                            }
                        }
                        else if (item is TextEdit)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((TextEdit)item).Text = (row["OtherPermissionValue"] == null ? "0" : row["OtherPermissionValue"].ToString());
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }
                        else if (item is ToggleSwitch)
                        {
                            foreach (DataRow row in dtOther.Rows)
                            {
                                string name = row["OtherPermissionName"].ToString();
                                if (item.Name.Substring(3) == name)
                                {
                                    ((ToggleSwitch)item).EditValue = (row["OtherPermissionValue"].ToString() == "True" ? true : false);
                                    break;
                                    // ((ToggleSwitch)item).EditValue= row["OtherPermissionIndex"].ToString();
                                }
                            }
                        }


                    }

                }
                SplashScreenManager.CloseForm(false);
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
        void ReadMenuPermissions()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                DataTable dtMenu;
                DataTable dtMenuPermission;
                int rowHandle = 0;
                for (int i = 0; i < dgvMenus.RowCount; )
                    dgvMenus.DeleteRow(i);
                // dgvFormsMenu    dgvForms
                strSQL = "SELECT MenuName, ArbCaption, EngCaption FROM Menus Where IsClientPurchaseIt=1";
                dtMenu = Lip.SelectRecord(strSQL);
                for (int i = 0; (i <= (dtMenu.Rows.Count - 1)); i++)
                {
                    strSQL = ("SELECT MenuName, MenuView  FROM UserMenusPermissions" + (" Where BranchID =" + (cmbBranchesID.EditValue.ToString() + (" And UserID=" + (cmbUsersID.EditValue.ToString() + (" And MenuName='" + (dtMenu.Rows[i]["MenuName"].ToString() + "'")))))));
                    dtMenuPermission = Lip.SelectRecord(strSQL);
                    dgvMenus.AddNewRow();
                    rowHandle = dgvMenus.GetRowHandle(dgvMenus.DataRowCount);
                    dgvMenus.SetRowCellValue(rowHandle, "ColMenuName", dtMenu.Rows[i]["MenuName"].ToString());
                    dgvMenus.SetRowCellValue(rowHandle, "ColMenuCaption", ((UserInfo.Language == iLanguage.Arabic) ? dtMenu.Rows[i]["ArbCaption"] : dtMenu.Rows[i]["EngCaption"]));

                    if ((dtMenuPermission.Rows.Count > 0))
                    {
                        dgvMenus.SetRowCellValue(rowHandle, dgvMenus.Columns["ColMenuView"], Comon.cbool(dtMenuPermission.Rows[0]["MenuView"]));
                    }
                    else
                    {
                        dgvMenus.SetRowCellValue(rowHandle, dgvMenus.Columns["ColMenuView"], false);
                    }

                }

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
        void SaveFormsPermissions()
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserFormsPermissions> listUserFormsPermissions = new List<UserFormsPermissions>();
                for (int i = 0; i < dgvForms.RowCount; i++)
                {
                    if (dgvForms.GetRowCellValue(i, "ColFormName").ToString() != "")
                    {
                        UserFormsPermissions UserFormsPermissions = new UserFormsPermissions();
                        UserFormsPermissions.UserID = SelectedUserID;
                        UserFormsPermissions.BranchID = SelectedBranchID;
                        UserFormsPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserFormsPermissions.FormName = dgvForms.GetRowCellValue(i, "ColFormName").ToString();
                        UserFormsPermissions.FormView = Comon.cInt((Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormView")) == true ? 1 : 0));
                        UserFormsPermissions.FormAdd = Comon.cInt(Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormAdd")) == true ? 1 : 0);
                        UserFormsPermissions.FormUpdate = Comon.cInt(Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormUpdate")) == true ? 1 : 0);
                        UserFormsPermissions.FormDelete = Comon.cInt(Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormDelete")) == true ? 1 : 0);
                        UserFormsPermissions.DaysAllowedForEdit = Comon.cInt(dgvForms.GetRowCellValue(i, "ColDaysAllowedForEdit").ToString());
                        listUserFormsPermissions.Add(UserFormsPermissions);
                    }
                }
                if (listUserFormsPermissions.Count > 0)
                {
                    int Result = UsersManagementDAL.frmInsertUserFormsPermissions(SelectedUserID, SelectedBranchID, listUserFormsPermissions);
                    SplashScreenManager.CloseForm(false);
                    if (Result ==  1 )
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح.. يرجى اعادة تشغيل النظام لكي يتم يتحدث التغيرات على الصلاحيات  " : "Please restart the system for changes to the permissions"));
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }


                }
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
        void SaveReportsPermissions()
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserReportsPermissions> listUserReportsPermissions = new List<UserReportsPermissions>();
                for (int i = 0; i < dgvReports.RowCount; i++)
                {
                    if (dgvReports.GetRowCellValue(i, "ColReportName").ToString() != "")
                    {
                        UserReportsPermissions UserReportsPermissions = new UserReportsPermissions();
                        UserReportsPermissions.UserID = SelectedUserID;
                        UserReportsPermissions.BranchID = SelectedBranchID;
                        UserReportsPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserReportsPermissions.ReportName = dgvReports.GetRowCellValue(i, "ColReportName").ToString();
                        UserReportsPermissions.ReportView = Comon.cInt((Comon.cbool(dgvReports.GetRowCellValue(i, "ColReportView")) == true ? 1 : 0));
                        UserReportsPermissions.ReportExport = Comon.cInt((Comon.cbool(dgvReports.GetRowCellValue(i, "ColReportExport")) == true ? 1 : 0));
                        UserReportsPermissions.ShowReportInReportViewer = Comon.cInt(Comon.cbool(dgvReports.GetRowCellValue(i, "ColShowReportInReportViewer")) == true ? 1 : 0);
                        listUserReportsPermissions.Add(UserReportsPermissions);
                    }
                }
                if (listUserReportsPermissions.Count > 0)
                {
                    int Result = UsersManagementDAL.frmInsertUserReportsPermissions(SelectedUserID, SelectedBranchID, listUserReportsPermissions);
                    SplashScreenManager.CloseForm(false);
                    if (Result ==  1 )
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح.. يرجى اعادة تشغيل النظام لكي يتم يتحدث التغيرات على الصلاحيات  " : "Please restart the system for changes to the permissions"));
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }
                }
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
        void SaveMenusPermissions()
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserMenusPermissions> listUserMenusPermissions = new List<UserMenusPermissions>();
                for (int i = 0; i < dgvMenus.RowCount; i++)
                {
                    if (dgvMenus.GetRowCellValue(i, "ColMenuName").ToString() != "")
                    {
                       // dgvMenus.GetRowCellValue(22, "ColMenuName").ToString();
                        UserMenusPermissions UserMenusPermissions = new UserMenusPermissions();
                        UserMenusPermissions.UserID = SelectedUserID;
                        UserMenusPermissions.BranchID = SelectedBranchID;
                        UserMenusPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserMenusPermissions.MenuName = dgvMenus.GetRowCellValue(i, "ColMenuName").ToString();
                        UserMenusPermissions.MenuView = Comon.cInt((Comon.cbool(dgvMenus.GetRowCellValue(i, "ColMenuView")) == true ? 1 : 0));
                        listUserMenusPermissions.Add(UserMenusPermissions);
                    }
                }
                if (listUserMenusPermissions.Count > 0)
                {
                    int Result = UsersManagementDAL.frmInsertUserMenusPermissions(SelectedUserID, SelectedBranchID, listUserMenusPermissions);
                    SplashScreenManager.CloseForm(false);
                    if (Result == 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح.. يرجى اعادة تشغيل النظام لكي يتم يتحدث التغيرات على الصلاحيات  " : "Please restart the system for changes to the permissions"));
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }
                }
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
        void SaveOtherPermissions()
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserOtherPermissions> listUserOtherPermissions = new List<UserOtherPermissions>();

                foreach (Control item in tabOthers.Controls)
                {
                    if (item is SpinEdit)
                    {
                        UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                        UserOtherPermissions.UserID = SelectedUserID;
                        UserOtherPermissions.BranchID = SelectedBranchID;
                        UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                        UserOtherPermissions.OtherPermissionValue = ((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue.ToString();
                        UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue);
                        listUserOtherPermissions.Add(UserOtherPermissions);
                    }
                    else if (item is TextEdit)
                    {
                        UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                        UserOtherPermissions.UserID = SelectedUserID;
                        UserOtherPermissions.BranchID = SelectedBranchID;
                        UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                        UserOtherPermissions.OtherPermissionValue = ((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue.ToString();
                        UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue);
                        listUserOtherPermissions.Add(UserOtherPermissions);

                    }
                    else if (item is ToggleSwitch)
                    {
                        UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                        UserOtherPermissions.UserID = SelectedUserID;
                        UserOtherPermissions.BranchID = SelectedBranchID;
                        UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                        UserOtherPermissions.OtherPermissionValue = ((ToggleSwitch)item).EditValue == null ? "False" : ((ToggleSwitch)item).EditValue.ToString();
                        UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((ToggleSwitch)item).EditValue == null ? "0" : ((ToggleSwitch)item).EditValue.ToString() == "False" ? "0" : "1");
                        listUserOtherPermissions.Add(UserOtherPermissions);
                    }
                    else if (item is LookUpEdit)
                    {
                        UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                        UserOtherPermissions.UserID = SelectedUserID;
                        UserOtherPermissions.BranchID = SelectedBranchID;
                        UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                        UserOtherPermissions.OtherPermissionValue = ((LookUpEdit)item).EditValue == null ? "0" : ((LookUpEdit)item).EditValue.ToString();
                        UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((LookUpEdit)item).ItemIndex == null ? "0" : ((LookUpEdit)item).ItemIndex.ToString());
                        listUserOtherPermissions.Add(UserOtherPermissions);

                    }

                }
                foreach (XtraTabPage page in tabControlSpecific.TabPages)
                {

                    foreach (Control item in page.Controls)
                    {

                        if (item is SpinEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue);
                            listUserOtherPermissions.Add(UserOtherPermissions);
                        }
                        else if (item is TextEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue);
                            listUserOtherPermissions.Add(UserOtherPermissions);

                        }
                        else if (item is ToggleSwitch)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((ToggleSwitch)item).EditValue == null ? "False" : ((ToggleSwitch)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((ToggleSwitch)item).EditValue == null ? "0" : ((ToggleSwitch)item).EditValue.ToString() == "False" ? "0" : "1");
                            listUserOtherPermissions.Add(UserOtherPermissions);
                        }
                        else if (item is LookUpEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((LookUpEdit)item).EditValue == null ? "0" : ((LookUpEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((LookUpEdit)item).ItemIndex == null ? "0" : ((LookUpEdit)item).ItemIndex.ToString());
                            listUserOtherPermissions.Add(UserOtherPermissions);

                        }
                    }
                }
                foreach (XtraTabPage page in tabControlPreparation.TabPages)
                {

                    foreach (Control item in page.Controls)
                    {

                        if (item is SpinEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue);
                            listUserOtherPermissions.Add(UserOtherPermissions);
                        }
                        else if (item is TextEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue);
                            listUserOtherPermissions.Add(UserOtherPermissions);

                        }
                        else if (item is ToggleSwitch)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((ToggleSwitch)item).EditValue == null ? "False" : ((ToggleSwitch)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((ToggleSwitch)item).EditValue == null ? "0" : ((ToggleSwitch)item).EditValue.ToString() == "False" ? "0" : "1");
                            listUserOtherPermissions.Add(UserOtherPermissions);
                        }
                        else if (item is LookUpEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((LookUpEdit)item).EditValue == null ? "0" : ((LookUpEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((LookUpEdit)item).ItemIndex == null ? "0" : ((LookUpEdit)item).ItemIndex.ToString());
                            listUserOtherPermissions.Add(UserOtherPermissions);

                        }
                    }
                }
                foreach (XtraTabPage page in tabControlManufactory.TabPages)
                {

                    foreach (Control item in page.Controls)
                    {

                        if (item is SpinEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue);
                            listUserOtherPermissions.Add(UserOtherPermissions);
                        }
                        else if (item is TextEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue);
                            listUserOtherPermissions.Add(UserOtherPermissions);

                        }
                        else if (item is ToggleSwitch)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((ToggleSwitch)item).EditValue == null ? "False" : ((ToggleSwitch)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((ToggleSwitch)item).EditValue == null ? "0" : ((ToggleSwitch)item).EditValue.ToString() == "False" ? "0" : "1");
                            listUserOtherPermissions.Add(UserOtherPermissions);
                        }
                        else if (item is LookUpEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = SelectedUserID;
                            UserOtherPermissions.BranchID = SelectedBranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((LookUpEdit)item).EditValue == null ? "0" : ((LookUpEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((LookUpEdit)item).ItemIndex == null ? "0" : ((LookUpEdit)item).ItemIndex.ToString());
                            listUserOtherPermissions.Add(UserOtherPermissions);

                        }
                    }
                }
                if (listUserOtherPermissions.Count > 0)
                {
                    int Result=  UsersManagementDAL.frmInsertUserOtherPermissions(SelectedUserID, SelectedBranchID, listUserOtherPermissions);
                    SplashScreenManager.CloseForm(false);
                    if (Result == 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح.. يرجى اعادة تشغيل النظام لكي يتم يتحدث التغيرات على الصلاحيات  " : "Please restart the system for changes to the permissions"));
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }
                }
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
        void SaveFormsPermissions(int BranchID,int USerID)
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserFormsPermissions> listUserFormsPermissions = new List<UserFormsPermissions>();
                for (int i = 0; i < dgvForms.RowCount; i++)
                {
                    if (dgvForms.GetRowCellValue(i, "ColFormName").ToString() != "")
                    {
                        UserFormsPermissions UserFormsPermissions = new UserFormsPermissions();
                        UserFormsPermissions.UserID = USerID;
                        UserFormsPermissions.BranchID = BranchID;
                 
                        UserFormsPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserFormsPermissions.FormName = dgvForms.GetRowCellValue(i, "ColFormName").ToString();
                        UserFormsPermissions.FormView = Comon.cInt((Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormView")) == true ? 1 : 0));
                        UserFormsPermissions.FormAdd = Comon.cInt(Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormAdd")) == true ? 1 : 0);
                        UserFormsPermissions.FormUpdate = Comon.cInt(Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormUpdate")) == true ? 1 : 0);
                        UserFormsPermissions.FormDelete = Comon.cInt(Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormDelete")) == true ? 1 : 0);
                        UserFormsPermissions.DaysAllowedForEdit = Comon.cInt(dgvForms.GetRowCellValue(i, "ColDaysAllowedForEdit").ToString());
                        listUserFormsPermissions.Add(UserFormsPermissions);
                    }
                }
                if (listUserFormsPermissions.Count > 0)
                {
                    int Result = UsersManagementDAL.frmInsertUserFormsPermissions(USerID, BranchID, listUserFormsPermissions);
                    SplashScreenManager.CloseForm(false);
                    if (Result == 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح.. يرجى اعادة تشغيل النظام لكي يتم يتحدث التغيرات على الصلاحيات  " : "Please restart the system for changes to the permissions"));
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }


                }
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
        void SaveReportsPermissions(int BranchID, int USerID)
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserReportsPermissions> listUserReportsPermissions = new List<UserReportsPermissions>();
                for (int i = 0; i < dgvReports.RowCount; i++)
                {
                    if (dgvReports.GetRowCellValue(i, "ColReportName").ToString() != "")
                    {
                        UserReportsPermissions UserReportsPermissions = new UserReportsPermissions();
                        UserReportsPermissions.UserID = USerID;
                        UserReportsPermissions.BranchID = BranchID;
                        UserReportsPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserReportsPermissions.ReportName = dgvReports.GetRowCellValue(i, "ColReportName").ToString();
                        UserReportsPermissions.ReportView = Comon.cInt((Comon.cbool(dgvReports.GetRowCellValue(i, "ColReportView")) == true ? 1 : 0));
                        UserReportsPermissions.ReportExport = Comon.cInt((Comon.cbool(dgvReports.GetRowCellValue(i, "ColReportExport")) == true ? 1 : 0));
                        UserReportsPermissions.ShowReportInReportViewer = Comon.cInt(Comon.cbool(dgvReports.GetRowCellValue(i, "ColShowReportInReportViewer")) == true ? 1 : 0);
                        listUserReportsPermissions.Add(UserReportsPermissions);
                    }
                }
                if (listUserReportsPermissions.Count > 0)
                {
                    int Result = UsersManagementDAL.frmInsertUserReportsPermissions(USerID, BranchID, listUserReportsPermissions);
                    SplashScreenManager.CloseForm(false);
                    if (Result == 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح.. يرجى اعادة تشغيل النظام لكي يتم يتحدث التغيرات على الصلاحيات  " : "Please restart the system for changes to the permissions"));
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }
                }
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
        void SaveMenusPermissions(int BranchID, int USerID)
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserMenusPermissions> listUserMenusPermissions = new List<UserMenusPermissions>();
                dgvMenus.MovePrev();
                for (int i = 0; i <= dgvMenus.RowCount-1; i++)
                {
                    if (dgvMenus.GetRowCellValue(i, "ColMenuName").ToString() != "")
                    {
                        UserMenusPermissions UserMenusPermissions = new UserMenusPermissions();
                        UserMenusPermissions.UserID = USerID;
                        UserMenusPermissions.BranchID = BranchID;
                        UserMenusPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserMenusPermissions.MenuName = dgvMenus.GetRowCellValue(i, "ColMenuName").ToString();
                        UserMenusPermissions.MenuView = Comon.cInt((Comon.cbool(dgvMenus.GetRowCellValue(i, "ColMenuView")) == true ? 1 : 0));
                        listUserMenusPermissions.Add(UserMenusPermissions);
                    }
                }
                if (listUserMenusPermissions.Count > 0)
                {
                    int Result = UsersManagementDAL.frmInsertUserMenusPermissions(USerID, BranchID, listUserMenusPermissions);
                    SplashScreenManager.CloseForm(false);
                    if (Result == 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح.. يرجى اعادة تشغيل النظام لكي يتم يتحدث التغيرات على الصلاحيات  " : "Please restart the system for changes to the permissions"));
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }
                }
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
        void SaveOtherPermissions(int BranchID,int USerID)
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserOtherPermissions> listUserOtherPermissions = new List<UserOtherPermissions>();

                foreach (Control item in tabOthers.Controls)
                {
                    if (item is SpinEdit)
                    {
                        UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                        UserOtherPermissions.UserID = USerID;
                        UserOtherPermissions.BranchID = BranchID;
                        UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                        UserOtherPermissions.OtherPermissionValue = ((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue.ToString();
                        UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue);
                        listUserOtherPermissions.Add(UserOtherPermissions);
                    }
                    else if (item is TextEdit)
                    {
                        UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                        UserOtherPermissions.UserID = USerID;
                        UserOtherPermissions.BranchID = BranchID;
                        UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                        UserOtherPermissions.OtherPermissionValue = ((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue.ToString();
                        UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue);
                        listUserOtherPermissions.Add(UserOtherPermissions);

                    }
                    else if (item is ToggleSwitch)
                    {
                        UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                        UserOtherPermissions.UserID = USerID;
                        UserOtherPermissions.BranchID = BranchID;
                        UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                        UserOtherPermissions.OtherPermissionValue = ((ToggleSwitch)item).EditValue == null ? "False" : ((ToggleSwitch)item).EditValue.ToString();
                        UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((ToggleSwitch)item).EditValue == null ? "0" : ((ToggleSwitch)item).EditValue.ToString() == "False" ? "0" : "1");
                        listUserOtherPermissions.Add(UserOtherPermissions);
                    }
                    else if (item is LookUpEdit)
                    {
                        UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                        UserOtherPermissions.UserID = USerID;
                        UserOtherPermissions.BranchID = BranchID;
                        UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                        UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                        UserOtherPermissions.OtherPermissionValue = ((LookUpEdit)item).EditValue == null ? "0" : ((LookUpEdit)item).EditValue.ToString();
                        UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((LookUpEdit)item).ItemIndex == null ? "0" : ((LookUpEdit)item).ItemIndex.ToString());
                        listUserOtherPermissions.Add(UserOtherPermissions);

                    }

                }
                foreach (XtraTabPage page in tabControlSpecific.TabPages)
                {

                    foreach (Control item in page.Controls)
                    {

                        if (item is SpinEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = USerID;
                            UserOtherPermissions.BranchID = BranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((SpinEdit)item).EditValue == null ? "0" : ((SpinEdit)item).EditValue);
                            listUserOtherPermissions.Add(UserOtherPermissions);
                        }
                        else if (item is TextEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = USerID;
                            UserOtherPermissions.BranchID = BranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((TextEdit)item).EditValue == null ? "0" : ((TextEdit)item).EditValue);
                            listUserOtherPermissions.Add(UserOtherPermissions);

                        }
                        else if (item is ToggleSwitch)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = USerID;
                            UserOtherPermissions.BranchID = BranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((ToggleSwitch)item).EditValue == null ? "False" : ((ToggleSwitch)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((ToggleSwitch)item).EditValue == null ? "0" : ((ToggleSwitch)item).EditValue.ToString() == "False" ? "0" : "1");
                            listUserOtherPermissions.Add(UserOtherPermissions);
                        }
                        else if (item is LookUpEdit)
                        {
                            UserOtherPermissions UserOtherPermissions = new UserOtherPermissions();
                            UserOtherPermissions.UserID = USerID;
                            UserOtherPermissions.BranchID = BranchID;
                            UserOtherPermissions.FacilityID = MySession.GlobalFacilityID;
                            UserOtherPermissions.OtherPermissionName = (item.Name.Substring(3));
                            UserOtherPermissions.OtherPermissionValue = ((LookUpEdit)item).EditValue == null ? "0" : ((LookUpEdit)item).EditValue.ToString();
                            UserOtherPermissions.OtherPermissionIndex = Comon.cInt(((LookUpEdit)item).ItemIndex == null ? "0" : ((LookUpEdit)item).ItemIndex.ToString());
                            listUserOtherPermissions.Add(UserOtherPermissions);

                        }
                    }
                }
                if (listUserOtherPermissions.Count > 0)
                {
                    int Result = UsersManagementDAL.frmInsertUserOtherPermissions(USerID, BranchID, listUserOtherPermissions);
                    SplashScreenManager.CloseForm(false);
                    if (Result == 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح.. يرجى اعادة تشغيل النظام لكي يتم يتحدث التغيرات على الصلاحيات  " : "Please restart the system for changes to the permissions"));
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }
                }
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







        #endregion
        #region Event
        private void cmbUsersID_EditValueChanged(object sender, EventArgs e)
        {
            XtraTabPage prevPage = tabControl.SelectedTabPage;
            if (prevPage.Name == "tabFroms")
                ReadFormsPermissions();
            else if (prevPage.Name == "tabReports")
                ReadReportPermissions();
            else if (prevPage.Name == "tabMenus")
                ReadMenuPermissions();
            else if (prevPage.Name == "tabOthers" || prevPage.Name == "tabSpecific")
                ReadOtherPermissions();


        }
        #endregion

        private void gridView1_RowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {

        }

        private void tabControl_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            XtraTabPage prevPage = tabControl.SelectedTabPage;
            if ((cmbUsersID.EditValue != null && cmbBranchesID.EditValue != null) && (cmbUsersID.EditValue.ToString() != "0" && cmbBranchesID.EditValue.ToString() != "0"))
                if (prevPage.Name == "tabFroms")
                    ReadFormsPermissions();
                else if (prevPage.Name == "tabReports")
                    ReadReportPermissions();
                else if (prevPage.Name == "tabMenus")
                    ReadMenuPermissions();
                else if (prevPage.Name == "tabOthers" || prevPage.Name == "tabSpecific")
                    ReadOtherPermissions();

        }
        protected override void DoSave()
        {
            if (!Validations.IsValidForm(this))
                return;
            XtraTabPage prevPage = tabControl.SelectedTabPage;
            if ((cmbUsersID.EditValue != null && cmbBranchesID.EditValue != null) && (cmbUsersID.EditValue.ToString() != "0" && cmbBranchesID.EditValue.ToString() != "0"))
                if (prevPage.Name == "tabFroms")
                    SaveFormsPermissions();
                else if (prevPage.Name == "tabReports")
                    SaveReportsPermissions();
                else if (prevPage.Name == "tabMenus")
                    SaveMenusPermissions();
                else if (prevPage.Name == "tabOthers" || prevPage.Name == "tabSpecific")
                    SaveOtherPermissions();
            frmLoginWeb.SetMySession(Comon.cInt(cmbUsersID.EditValue), Comon.cInt(cmbBranchesID.EditValue));

            //this Code To Resturt After Change The Permissions 
            //Messages.MsgWarning(Messages.TitleWorning, "سيتم إعادة تشغيل النظام.. ");
            //if(Yes)
            //{       
            //    Application.Restart();
            //    Environment.Exit(0);
            //}
        }
        

        private void tabOthers_Paint(object sender, PaintEventArgs e)
        {

        }


        private void tabSpecificControl_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabPage6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPurchaseٌReturn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toggleSwitch2_Toggled(object sender, EventArgs e)
        {

        }

        private void frmUserPermissions_Load(object sender, EventArgs e)
        {
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
        }

        private void dgvForms_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            double num;
            GridView view = sender as GridView;
            view.ClearColumnErrors();
            string ColName = view.FocusedColumn.FieldName;

            if (ColName == "ColFormAdd" || ColName == "ColFormView" || ColName == "ColFormUpdate" || ColName == "ColFormDelete")
            {
                var t = e.Value;
                var row = view.FocusedRowHandle;
                if (row == 0)
                    for (int i = 0; i <= view.RowCount - 1; i++)
                        dgvForms.SetRowCellValue(i, ColName, t);
            }
        }
        private void dgvReports_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            double num;
            GridView view = sender as GridView;
            view.ClearColumnErrors();
            string ColName = view.FocusedColumn.FieldName;

            if (ColName == "ColReportView" || ColName == "ColReportExport" || ColName == "ColShowReportInReportViewer")
            {
                var t = e.Value;
                var row = view.FocusedRowHandle;
                if (row == 0)
                    for (int i = 0; i <= view.RowCount - 1; i++)
                        dgvReports.SetRowCellValue(i, ColName, t);
            }
        }

        private void labelControl306_Click(object sender, EventArgs e)
        {

        }

        private void tgsAllowOutQtyNegative_Toggled(object sender, EventArgs e)
        {

        }

        private void tabSaleReturn_Paint(object sender, PaintEventArgs e)
        {

        }



    }
}

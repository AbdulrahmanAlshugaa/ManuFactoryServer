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
using DevExpress.XtraSplashScreen;
using Edex.ModelSystem;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model;
using Edex.Model.Language;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Edex.DAL.UsersManagement;
using System.Reflection;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmSelectScrrenFovorit : BaseForm
    {
        private string PrimaryName;
        GridColumn ColFromName;
        private string strSQL;
        public frmSelectScrrenFovorit()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                InitializeComponent();
                PrimaryName = "ArbName";
                if(UserInfo.Language==iLanguage.English)
                    PrimaryName = "EngName";
                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                InitDataTable();
                this.Load += frmSelectScrrenFovorit_Load;
                this.dgvForms.RowCellStyle+=dgvForms_RowCellStyle;
                dgvForms.InitNewRow += dgvForms_InitNewRow;
                dgvForms.CellValueChanged+=dgvForms_CellValueChanged;                 
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
     
        private void dgvForms_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ColFormFovorite" && (bool)dgvForms.GetRowCellValue(e.RowHandle, e.Column))
            {
                string formName = dgvForms.GetRowCellValue(e.RowHandle, "ColFormName").ToString();
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type formType = assembly.GetTypes().FirstOrDefault(type => type.Name == formName && type.IsSubclassOf(typeof(Form)));
                if (formType != null )
                {
                    using (BaseForm frm = (BaseForm)Activator.CreateInstance(formType))
                    {
                        bool flage = true;
                        if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            flage = false;                       
                        }
                        if(flage)
                            dgvForms.SetRowCellValue(e.RowHandle, e.Column, false);
                    }
                }
            }
        }         
        protected override void DoSave()
        {
            try
            {
                int SelectedUserID = Comon.cInt(cmbUsersID.EditValue);
                int SelectedBranchID = Comon.cInt(cmbBranchesID.EditValue);
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                List<UserFormsFovorite> listUserScreenFovorite = new List<UserFormsFovorite>();
                for (int i = 0; i < dgvForms.RowCount; i++)
                {
                    if (dgvForms.GetRowCellValue(i, "ColFormName").ToString() != "" && Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormFovorite")) == true)
                    {
                        UserFormsFovorite UserScreenFovorite = new UserFormsFovorite();
                        UserScreenFovorite.UserID = SelectedUserID;
                        UserScreenFovorite.BranchID = SelectedBranchID;
                        UserScreenFovorite.FacilityID = MySession.GlobalFacilityID;
                        UserScreenFovorite.FormName = dgvForms.GetRowCellValue(i, "ColFormName").ToString();
                        UserScreenFovorite.FormFovorite = Comon.cInt((Comon.cbool(dgvForms.GetRowCellValue(i, "ColFormFovorite")) == true ? 1 : 0));
                        UserScreenFovorite.ArbCaption = dgvForms.GetRowCellValue(i, "ColArbCaption").ToString();
                        UserScreenFovorite.EngCaption = dgvForms.GetRowCellValue(i, "ColEngCaption").ToString();
                        listUserScreenFovorite.Add(UserScreenFovorite);
                    }
                }
                if (listUserScreenFovorite.Count > 0)
                {
                    int Result =UserScreenFovoriteDAL.frmInsertUserScreenFovorite(SelectedUserID, SelectedBranchID, listUserScreenFovorite);
                    SplashScreenManager.CloseForm(false);
                    if (Result == 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, ((UserInfo.Language == iLanguage.Arabic) ? "تم الحفظ بنجاح..    " : " The Save is Scsassfully"));
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
        private void dgvForms_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColFormName"], "");
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColArbCaption"], "");
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColEngCaption"], "");       
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColFormFovorite"], false);
            view.SetRowCellValue(e.RowHandle, dgvForms.Columns["ColMenuName"], "");
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
        private void InitDataTable()
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ColFormName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ColArbCaption", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ColEngCaption", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ColMenuName", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ColFormFovorite", System.Type.GetType("System.Boolean"));         
            dgcForms.DataSource = dtItem;
            if (PrimaryName == "ArbName")
            {
                ColFromName = dgvForms.Columns["ColArbCaption"];
                dgvForms.Columns["ColArbCaption"].Visible = true;
                //dgvForms.Columns["ColEngCaption"].Visible = false;
            }
            else
            {
                ColFromName = dgvForms.Columns["ColEngCaption"];
                dgvForms.Columns["ColEngCaption"].Visible = true;
            }
        }
        void frmSelectScrrenFovorit_Load(object sender, EventArgs e)
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
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[13].Visible = false;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[14].Visible = false;
        }
        void ReadForms()
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
                    strSQL = ("SELECT FormFovorite FROM UserFormsFovorite" + (" Where BranchID =" + (cmbBranchesID.EditValue.ToString() + (" And UserID=" + (cmbUsersID.EditValue.ToString() + (" And FormName='" + (dtForms.Rows[i]["FormName"].ToString() + "'")))))));
                    dtFormPermission = Lip.SelectRecord(strSQL);
                    if ((MenuName != dtForms.Rows[i]["MenuName"].ToString()))
                    {
                        dgvForms.AddNewRow();
                        if (UserInfo.Language == iLanguage.Arabic)
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
                    dgvForms.SetRowCellValue(rowHandle, ColFromName, (  dtForms.Rows[i]["ArbCaption"].ToString()  ));
                    dgvForms.SetRowCellValue(rowHandle, "ColEngCaption", (dtForms.Rows[i]["EngCaption"].ToString()));
                    if ((dtFormPermission.Rows.Count > 0))
                    {
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormFovorite"], Comon.cbool(dtFormPermission.Rows[0]["FormFovorite"]));
                    }
                    else
                    {
                        dgvForms.SetRowCellValue(rowHandle, dgvForms.Columns["ColFormFovorite"], false);
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
        private void cmbUsersID_EditValueChanged(object sender, EventArgs e)
        {
            ReadForms();
        }
        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit obj = (LookUpEdit)sender;
            
            FillCombo.FillComboBoxLookUpEdit(cmbUsersID, "Users", "UserID", PrimaryName, "", " BranchID = " + obj.EditValue.ToString(), (UserInfo.Language == iLanguage.English ? "Select User" : "حدد المستخدم"));
            if (UserInfo.ID == 1)
                cmbUsersID.Enabled = true;
            else
                cmbUsersID.EditValue = UserInfo.ID;

        }
    }
}
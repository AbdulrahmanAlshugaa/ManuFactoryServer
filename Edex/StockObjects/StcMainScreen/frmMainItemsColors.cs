using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using Edex.DAL;
using Edex.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using Edex.ModelSystem;
using Edex.StockObjects.Codes;
using Edex.Model.Language;
using DevExpress.XtraSplashScreen;
using Edex.GeneralObjects.GeneralForms;
using DevExpress.XtraReports.UI;
using Edex.DAL.Stc_itemDAL;
namespace Edex.StockObjects.StcMainScreen
{
    public partial class frmMainItemsColors : Edex.GeneralObjects.GeneralForms.BaseFormMain
    {
       
        #region Declare
        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        #endregion

        public frmMainItemsColors()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
        }
        private void frmMainStores_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        #region GridView_Event
        private void GridView_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;


            view.ClearColumnErrors();
            string ColName = view.FocusedColumn.FieldName;

            if (ColName == "Delete")
            {
                if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }
                int index = view.FocusedRowHandle;
                if (index < 0)
                    return;

                int ColorID = Comon.cInt(view.GetRowCellValue(index, "ColorID").ToString());
                Stc_ItemsColors cClass = new Stc_ItemsColors();
                cClass.ColorID = ColorID;
                cClass.UserID = MySession.UserID;
                if (Stc_ItemsColors_DAL.Delete(cClass) == true)
                {
                    view.DeleteSelectedRows();
                    if (index >= 0)
                    {
                        if (index > 0)
                            index = index - 1;
                        else if (index < 0)
                        {
                            index = view.DataRowCount;
                            index = index - 1;
                        }
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                        view.SelectRow(index);
                        view.FocusedRowHandle = index;
                    }
                }
            }

            if (ColName == "ShowRecord")
            {
                if (!FormUpdate)
                {

                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    return;
                }
                else
                {
                    DoEdit();
                }
            }
        }
        private void GridView_DoubleClick(object sender, EventArgs e)
        {
            DoEdit();
        }
        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            GridView view = GridView as GridView;
            view.ClearColumnErrors();
            int index = view.FocusedRowHandle;
            if (index >= 0)
                ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Item.Caption = (index + 1) + "/" + (GridView.RowCount);
        }
        #endregion

        #region Function

        public void ClearFields()
        {
            try
            {

                GridControl.DataSource = _sampleData;
                GridControl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoNew()
        {
            try
            {
                frmItemsColors frm = new frmItemsColors();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.OpenFromMain = true;
                    frm.ShowDialog();
                    FillGrid();
                }
                else
                    frm.Dispose();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoEdit()
        {

            GridView view = GridView as GridView;
            view.ClearColumnErrors();
            int index = view.FocusedRowHandle;

            if (index >= 0)
            {
                int ColorID = Comon.cInt(view.GetRowCellValue(index, "ColorID").ToString());

                if (!FormUpdate)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                    return;
                }

                try
                {
                    frmItemsColors frm = new frmItemsColors();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.txtColorID.Text = ColorID.ToString();
                        frm.txtColorID_Validating(null, null);
                        frm.ShowDialog();
                        FillGrid();
                    }
                    else
                        frm.Dispose();
                }
                catch (Exception ex)
                {
                    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                }

            }
        }
        protected override void DoDelete()
        {

            GridView view = GridView as GridView;
            view.ClearColumnErrors();
            int index = view.FocusedRowHandle;

            if (index >= 0)
            {
                int ColorID = Comon.cInt(view.GetRowCellValue(index, "ColorID").ToString());


                if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }
                Stc_ItemsColors cClass = new Stc_ItemsColors();
                cClass.ColorID = ColorID;
              
                cClass.UserID = MySession.UserID;
                if (Stc_ItemsColors_DAL.Delete(cClass) == true)
                {
                    view.DeleteSelectedRows();
                    if (index >= 0)
                    {
                        if (index > 0)
                            index = index - 1;
                        else if (index < 0)
                        {
                            index = view.DataRowCount;
                            index = index - 1;
                        }
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                        view.SelectRow(index);
                        view.FocusedRowHandle = index;
                    }
                }


            }
        }
        protected override void DoPrint()
        {
            try
            {
                GridView.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void FillGrid()
        {
            
            GridControl.DataSource =Stc_ItemsColors_DAL.GetAllData();
        }
        #endregion

        private void GridControl_Click(object sender, EventArgs e)
        {

        }

    }
}
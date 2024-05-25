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
    public partial class frmMainItemsBrands : Edex.GeneralObjects.GeneralForms.BaseFormMain
    {
        #region Declare
        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        #endregion

        public frmMainItemsBrands()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
        }
        #region GridView_Event

        private void GridView_Click(object sender, EventArgs e)
        {

        }
        private void GridView_DoubleClick(object sender, EventArgs e)
        {
            DoEdit();
        }
        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

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

                frmItemsBrands frm = new frmItemsBrands();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);

                   // frm.OpenFromMain = true;
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
                int BrandID = Comon.cInt(view.GetRowCellValue(index, "BrandID").ToString());

                if (!FormUpdate)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                    return;
                }

                try
                {
                    frmItemsBrands frm = new frmItemsBrands();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.txtBrandID.Text = BrandID.ToString();
                        frm.txtBrandID_Validating(null, null);
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
                int BrandID = Comon.cInt(view.GetRowCellValue(index, "BrandID").ToString());


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
                Stc_ItemsBrands cClass = new Stc_ItemsBrands();


                cClass.BrandID = BrandID;

                cClass.UserID = MySession.UserID;

                if (Stc_ItemsBrandDAL.Delete(cClass) == true)
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
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
                ReportName = "rptStores";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["FromItemID"].Value = txtFromItemNo.Text.Trim().ToString();
                rptForm.Parameters["ToItemID"].Value = txtToItemNo.Text.Trim().ToString();



                /********************** Details ****************************/

                DataTable dt = new DataTable();
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("BrandID", typeof(string)));
                dt.Columns.Add(new DataColumn("ArbName", typeof(string)));
                dt.Columns.Add(new DataColumn("EngName", typeof(string)));



                for (int i = 0; i <= GridView.DataRowCount - 1; i++)
                {
                    var row = dt.NewRow();
                    row["BrandID"] = GridView.GetRowCellValue(i, "BrandID").ToString();
                    row["ArbName"] = GridView.GetRowCellValue(i, "ArbName").ToString();

                    if (Comon.cInt(txtFromItemNo.Text) > 0 && Comon.cInt(txtToItemNo.Text) > 0)
                    {

                        if (Comon.cInt(row["BrandID"]) >= Comon.cInt(txtFromItemNo.Text) && Comon.cInt(row["BrandID"]) <= Comon.cInt(txtToItemNo.Text))
                        {

                            dt.Rows.Add(row);
                            row["EngName"] = dt.Rows.Count;
                        }
                    }
                    else
                    {

                        dt.Rows.Add(row);
                        row["EngName"] = dt.Rows.Count;
                    }



                }
                rptForm.DataSource = dt;
                rptForm.DataMember = ReportName;
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.CreateDocument();
                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = true;
                if (ShowReportInReportViewer)
                {
                    frmReportViewer frmRptViewer = new frmReportViewer();
                    frmRptViewer.documentViewer1.DocumentSource = rptForm;
                    frmRptViewer.ShowDialog();
                }
                else
                {
                    bool IsSelectedPrinter = false;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                    for (int i = 1; i < 6; i++)
                    {
                        string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                        if (!string.IsNullOrEmpty(PrinterName))
                        {
                            rptForm.PrinterName = PrinterName;
                            rptForm.Print(PrinterName);
                            IsSelectedPrinter = true;
                        }
                    }
                    SplashScreenManager.CloseForm(false);
                    if (!IsSelectedPrinter)
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void FillGrid()
        {

            GridControl.DataSource = Stc_ItemsBrandDAL.GetAllData();
        }
        #endregion
        private void frmMainItemsBrands_Load_1(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

    }
}

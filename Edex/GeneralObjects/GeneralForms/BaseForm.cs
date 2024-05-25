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
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using Edex.GeneralObjects.GeneralClasses;

using Edex.ModelSystem;
using Edex.Model;
using Edex.Model.Language;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraTab;
using DevExpress.XtraNavBar;
namespace Edex.GeneralObjects.GeneralForms
{
    public partial   class BaseForm : DevExpress.XtraEditors.XtraForm
    {

        public bool FormAdd;
        public bool FormDelete;
        public bool FormUpdate;
        public bool FormView;
        public int DaysAllowedForEdit;
        public string ReportName;
        public bool ReportView;
        public bool ReportExport;
        public bool ShowReportInReportViewer;
        private string strSQL;

        public bool falgPrint = false;
        public BaseForm()
        {
            if (!this.IsDisposed)
            {
                InitializeComponent();



                this.MdiParent = MySession.DefultMainParent;

                btnEdit.Enabled = false;
                btnSave.Enabled = true;
                btnRolBack.Enabled = true;

                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnLast.Enabled = false;

                btnNew.Enabled = false;
                btnPrint.Enabled = false;
                btnExit.Enabled = false;

                this.FormClosing += BaseForm_FormClosing;
                falgPrint = false;
            }
          
        }


       



        
        

        void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (Application.OpenForms.Count > 0)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmMain))
                    {
                        frmMain mainForm = (frmMain)form;
                        foreach (NavBarItemLink link in mainForm.navBarGroup1.ItemLinks.ToArray())
                        {
                            if (link.Item.Caption == this.Text)
                            {
                                mainForm.navBarGroup1.ItemLinks.Remove(link);
                            }
                        }
                    }
                }
            }
        }

        

        protected virtual void DoNext()
        {


        }

        protected virtual void DoLast()
        {


        }
        protected virtual void DoOpenFormReleted()
        {


        }
        protected virtual void DoNew()
        {

        }
        protected virtual void DoAddFrom()
        {

        }

        protected virtual void DoPrevious()
        {


        }
        protected virtual void DoFirst()
        {


        }
        protected virtual void Find()
        {


        }


        protected virtual void DoSearch()
        {


        }

        protected virtual void DoExit()
        {

        }
        protected virtual void DoReadRecord(long InvoiceID)
        {

        }
        protected virtual void DoSave()
        {

        }
        protected virtual void DoRolBack()
        {

        }

        protected virtual void DoEdit()
        {


        }

        protected virtual void DoDelete()
        {

        }

        protected virtual void DoPrint()
        {

        }
        protected virtual void DoExport()
        {

        }
        public void CallRoleBack()
        { 
            Validations.DoRoolBackRipon(this, ribbonControl1);
            Validations.EnabledControl(this, false);
            DoRolBack();

            
        }
        public void CallAddfrom()
        {
            DoAddFrom();
        }

        public void CallNew()
        {
           


            DoNew();
            Validations.DoNewRipon(this, ribbonControl1);


        }
        public void CalLast()
        {
            DoLast();
        }
        public void CalOpenFormReleted()
        {
            DoOpenFormReleted();
        }
        public void CalFirst()
        {
            DoFirst();
        }
        protected virtual void GetSelectedSearch(CSearch cls)
        {

        }
        public void CalGetSelectSearchList(CSearch cls)
        {
            GetSelectedSearch(cls);
        }
        public void CalPrevious()
        {
            DoPrevious();
        }
        public void CallNext()
        {
            DoNext();
        }
        public void CalSearch()
        {
            DoSearch();
        }
        public void CalFind()
        {
            Find();
        }
        public void CallExit()
        {

            if (Application.OpenForms.Count > 0)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmMain))
                    {
                        frmMain mainForm = (frmMain)form;
                        foreach (NavBarItemLink link in mainForm.navBarGroup1.ItemLinks.ToArray())
                        {
                            if (link.Item.Caption ==this.Text)
                            {
                                mainForm.navBarGroup1.ItemLinks.Remove(link);
                            }
                        }
                    }
                }
            }


            this.Close();
        }
        public void CallDelete()
        {
            DoDelete();
        }
        public void CallSave()
        {
            DoSave();


        }
        public void CallRead(long ID)
        {
            DoReadRecord(ID);
        }
        public void CallEdit()
        {
            DoEdit();
          
        }
        public void CallPrint()
        {
            DoPrint();
        }
        public void CallExport()
        {
            DoExport();
        }
        private void btnSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CalSearch();
        }
        private void btnNew_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CallNew();
        }
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //btnSave.Enabled = true;
            //btnRolBack.Enabled = true;

            //btnNext.Enabled = false;
            //btnPrevious.Enabled = false;
            //btnFirst.Enabled = false;
            //btnLast.Enabled = false;

            //btnNew.Enabled = false;
            //btnPrint.Enabled = false;
            //btnExit.Enabled = false;

            CallEdit();
        }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnSave.Enabled = true;
            //btnRolBack.Enabled = true;

            //btnNext.Enabled = true;
            //btnPrevious.Enabled = true;
            //btnFirst.Enabled = true;
            //btnLast.Enabled = true;

            //btnNew.Enabled = true;
            //btnPrint.Enabled = true;
            //btnExit.Enabled = true;
            CallSave();
        }
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CallDelete();
        }
        private void btnSearch_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CalSearch();
        }
        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CallPrint();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CallExit();
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CallNew();
        }

        private void btnFirst_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CalFirst();
        }

        private void btnLast_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CalLast();
        }

        private void btnPrevious_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CalPrevious();
        }

        private void btnNext_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CallNext();
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            this.CancelButton = null;
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            Language();
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            falgPrint = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = false;
            
            //foreach (GridView gridView in this.Controls.OfType<GridView>())
            //{
            //    gridView.CustomDrawCell += GridView_CustomDrawCell;
            //}

        }


        //private void GridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        //{
        //    if (e.Column.FieldName != "Signature")
        //    {
        //        e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
        //        e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
        //        e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
        //        e.Handled = true;
        //        GridView gridView = (GridView)sender; // Cast the sender object to GridView
        //        gridView.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
        //        gridView.Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
        //    }
        //}

        //void Language()
        //{
        //    for (int i = 0; i < ribbonControl1.Pages.Count; i++)
        //    {
        //        if (UserInfo.Language == iLanguage.English)
        //        {
        //            ChangeLanguage.LTR(ribbonControl1.Pages[i]);
        //        }
        //        foreach (RibbonPageGroup group in ribbonControl1.Pages[i].Groups)
        //        {
        //            if (UserInfo.Language == iLanguage.English)
        //            {
        //                ChangeLanguage.LTR(group);
        //            }
        //            if (group.Name == string.Concat(ribbonControl1.Pages[i].Name, "Group3"))
        //            {
        //                foreach (BarItemLink link in group.ItemLinks)
        //                {
        //                    if (UserInfo.Language == iLanguage.English)
        //                    {
        //                        ChangeLanguage.LTR((BarButtonItem)link.Item);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (BarItemLink link in group.ItemLinks)
        //                {
        //                    if (UserInfo.Language == iLanguage.English)
        //                    {
        //                        if (link.Item is BarStaticItem)
        //                            continue;
        //                        ChangeLanguage.LTR((BarButtonItem)link.Item);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //void ApplyLanguage(Control control)
        //{
        //    foreach (Control childControl in control.Controls)
        //    {
        //        if (childControl is TextEdit || childControl is ComboBoxEdit)
        //            return;
        //        childControl.RightToLeft = RightToLeft.No;
        //        if (UserInfo.Language == iLanguage.English)
        //        {
        //            ChangeLanguage.LTR(childControl);
        //        }
        //        ApplyLanguage(childControl);
        //    }
        //}

        void ApplyLanguage(TileNavPane tileNavPane)
        {
            if (UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR(tileNavPane);
            }

            foreach (TileNavCategory category in tileNavPane.Categories)
            {
                ApplyLanguage(category);
            }
        }

        void ApplyLanguage(RibbonControl ribbonControl)
        {
            for (int i = 0; i < ribbonControl.Pages.Count; i++)
            {
                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(ribbonControl.Pages[i]);
                }

                foreach (RibbonPageGroup group in ribbonControl.Pages[i].Groups)
                {
                    ApplyLanguage(group);
                }
            }
        }

        void ApplyLanguage(TileNavCategory category)
        {
            if (UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR(category);
            }

            foreach (TileNavItem item in category.Items)
            {
                ApplyLanguage(item);
            }
        }

        void ApplyLanguage(RibbonPageGroup group)
        {
            if (UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR(group);
            }

            foreach (BarItemLink link in group.ItemLinks)
            {
                ApplyLanguage(link.Item);
            }
        }

        void ApplyLanguage(BarItem item)
        {
            if (item is BarButtonItem)
            {
                ApplyLanguage((BarButtonItem)item);
            }
        }

        void ApplyLanguage(BarButtonItem barButtonItem)
        {
            if (UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR(barButtonItem);
            }
        }

        void ApplyLanguage(TileNavItem item)
        {
            if (UserInfo.Language == iLanguage.English)
                ChangeLanguage.LTR(item);
        }
        void ApplyLanguage(object element)
        {
            if (element is Control)
            {
                Control control = (Control)element;
                // Apply language logic for controls
                control.RightToLeft = RightToLeft.No;
                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(control);
                }
                foreach (Control childControl in control.Controls)
                {
                    ApplyLanguage(childControl);
                }
            }
         
        }

        void Language()
        {
            if (UserInfo.Language != iLanguage.Arabic)
            {               
                for (int i = 0; i < ribbonControl1.Pages.Count; i++)
                {
                    if (UserInfo.Language == iLanguage.English)
                    {
                        ChangeLanguage.LTR(ribbonControl1.Pages[i]);
                    }
                    foreach (RibbonPageGroup group in ribbonControl1.Pages[i].Groups)
                    {
                        if (UserInfo.Language == iLanguage.English)
                        {
                            ChangeLanguage.LTR(group);
                        }
                        if (group.Name == string.Concat(ribbonControl1.Pages[i].Name, "Group3"))
                        {
                            foreach (BarItemLink link in group.ItemLinks)
                            {
                                if (UserInfo.Language == iLanguage.English)
                                {
                                    ChangeLanguage.LTR((BarButtonItem)link.Item);
                                }
                            }
                        }
                        else
                        {
                            foreach (BarItemLink link in group.ItemLinks)
                            {
                                if (UserInfo.Language == iLanguage.English)
                                {
                                    //if (link.Item is BarStaticItem)
                                    //    continue;
                                    //ChangeLanguage.LTR((BarButtonItem)link.Item);
                                    if (link.Item is BarStaticItem || link.Item is BarButtonGroup)
                                        continue;
                                    if (link.Item is BarButtonItem)
                                    {
                                        ChangeLanguage.LTR((BarButtonItem)link.Item);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (Control childControl in this.Controls)
                {

                    ApplyLanguage(childControl);
                }
               
            }
        }
        void ApplyLanguage(Control control)
        {
            if (control is TextEdit || control is ComboBoxEdit)
                return;
            if (UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR(control);
            }
            ApplyLanguageToElement(control);
        }

        void ApplyLanguageToElement(object element)
        {
            if (element is XtraTabPage)
            {
                XtraTabPage tabPage = (XtraTabPage)element;
                tabPage.RightToLeft = RightToLeft.No;
                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(tabPage);
                }
                foreach (Control childControl in tabPage.Controls)
                {
                    try
                    {
                        childControl.Location = new System.Drawing.Point(tabPage.Size.Width - childControl.Size.Width - childControl.Location.X, childControl.Location.Y);
                        if (childControl is TextEdit || childControl is ComboBoxEdit)
                            continue;
                    }
                    catch 
                    { 
                    }
                    ApplyLanguageToElement(childControl);
                }
            }
            else if (element is GroupBox)
            {
                GroupBox tabPage = (GroupBox)element;
                tabPage.RightToLeft = RightToLeft.No;
                 
                foreach (Control childControl in tabPage.Controls)
                {
                    try
                    {

                        childControl.Location = new System.Drawing.Point(tabPage.Size.Width - childControl.Size.Width - childControl.Location.X, childControl.Location.Y);
                        if (childControl is TextEdit || childControl is ComboBoxEdit)
                            continue;
                    }
                    catch { }
                    ApplyLanguageToElement(childControl);
                }
            }
            else if (element is Panel)
            {
                Panel tabPage = (Panel)element;
                tabPage.RightToLeft = RightToLeft.No;
                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(tabPage);
                }
                foreach (Control childControl in tabPage.Controls)
                {
                    
                    try
                    {
                        childControl.Location = new System.Drawing.Point(tabPage.Size.Width - childControl.Size.Width - childControl.Location.X, childControl.Location.Y);
                        if (childControl is TextEdit || childControl is ComboBoxEdit)
                            continue;
                    }
                    catch { }
                    ApplyLanguageToElement(childControl);
                }
            }
            else if (element is Control)
            {
                Control control = (Control)element;
                if (control is Panel || control is GroupBox || control is XtraTabPage)
                    return;
              
                // Apply language logic for Control
                control.RightToLeft = RightToLeft.No;
                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(control);
                }
               
                foreach (Control childControl in control.Controls)
                {
                    if (childControl is TextEdit || childControl is ComboBoxEdit)
                        return;
                    ApplyLanguageToElement(childControl);
                }
            }
            else if (element is BarItem)
            {
                BarItem barItem = (BarItem)element;
                if (barItem is BarButtonItem)
                {
                    BarButtonItem barButtonItem = (BarButtonItem)barItem;
                    if (UserInfo.Language == iLanguage.English)
                    {
                        ChangeLanguage.LTR(barButtonItem);
                    }
                }
                 
            }
            
        }

      
       

     
        private void btnRolBack_ItemClick(object sender, ItemClickEventArgs e)
        {
            CallRoleBack();

            //btnSave.Enabled = true;
            //btnRolBack.Enabled = true;

            //btnNext.Enabled = true;
            //btnPrevious.Enabled = true;
            //btnFirst.Enabled = true;
            //btnLast.Enabled = true;

            //btnNew.Enabled = true;
            //btnPrint.Enabled = true;
            //btnExit.Enabled = true;


        }


        private void btnExportToExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Tag = "Xlsx";
            CallExport();
        }

        private void btnExportToPDF_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Tag = this.Tag = "pdf";
            CallExport();
        }

        private void btnExportToRichText_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Tag = this.Tag = "txt";
            CallExport();
        }

        private void btnExportToMSWord_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Tag = this.Tag = "docx";
            CallExport();
        }

        private void btnExportToHTML_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Tag = this.Tag = "html";
            CallExport();
        }

        private void btnAddFrom_ItemClick(object sender, ItemClickEventArgs e)
        {
            CallAddfrom();
        }

        
    }
}
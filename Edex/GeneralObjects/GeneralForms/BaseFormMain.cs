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
using Edex.ModelSystem;
using Edex.Model;
using Edex.Model.Language;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class BaseFormMain : DevExpress.XtraEditors.XtraForm
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

        public BaseFormMain()
        {
            InitializeComponent();
        }


        protected virtual void DoNext()
        {


        }

        protected virtual void DoLast()
        {


        }

        protected virtual void DoNew()
        {
            CallNew();

        }

        protected virtual void DoPrevious()
        {


        }
        protected virtual void DoFirst()
        {


        }

        protected virtual void DoSearch()
        {


        }

        protected virtual void DoExit()
        {

        }

        protected virtual void DoSave()
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
        public void CallNew()
        {
            DoNew();
        }

        public void CalLast()
        {
            DoLast();
        }

        public void CalFirst()
        {
            DoFirst();
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

        public void CallExit()
        {
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
            CallEdit();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
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
            CallExport();
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
            Language();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }
        void Language()
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
                                if (link.Item is BarStaticItem)
                                    continue;
                                ChangeLanguage.LTR((BarButtonItem)link.Item);
                            }


                        }


                    }
                }

            }
        }

      
    }
}
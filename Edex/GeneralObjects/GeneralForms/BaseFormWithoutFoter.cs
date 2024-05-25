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

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class BaseFormWithoutFoter : DevExpress.XtraEditors.XtraForm
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
        public BaseFormWithoutFoter()
        {
            InitializeComponent();
            

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



            //btnExport.Visibility = BarItemVisibility.Never;


            falgPrint = false;
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
            Language();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            falgPrint = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = false;

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
       

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
           // Common.HotKey(this, e.KeyCode);
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
using DevExpress.XtraSplashScreen;
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

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmRestoringDeleted : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public bool RestoringDone;
        public string strSQL;
        public string name = "";
        public frmRestoringDeleted()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
            this.txtDocTypeID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDocTypeID_Validating);
            ///////////////////////////////////////////////////////
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            //this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // this.txtToDate.EditValue = DateTime.Now;

        }
        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };

            cls.SQLStr = "SELECT DISTINCT SN AS [الرقم], RecordTypeArb AS [الشاشة]  FROM  dbo.RestoringDeleted WHERE (BranchID = " + UserInfo.BRANCHID + ") ORDER BY [الرقم]";

            if (UserInfo.Language == iLanguage.English)
                cls.SQLStr = "SELECT DISTINCT SN AS Number, RecordTypeEng AS [Form Name] FROM dbo.RestoringDeleted  WHERE (BranchID = " + UserInfo.BRANCHID + ") ORDER BY Number";


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


                txtDocTypeID.Text = cls.PrimaryKeyValue;
                txtDocTypeID_Validating(null, null);











            }
        }



        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                name = "";
                btnShow.Visible = false;

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                Application.DoEvents();
                // gridView1.ClearGrouping();
                gridControl1.DataSource = null;
                string filter = "";
                string strSQL = "";
                DataTable dt;
                if (txtDocTypeID.Text != string.Empty)
                    // filter = " Stc_Items.ItemID >=" + txtFromItemNo.Text + " AND ";
                    strSQL = "SELECT * FROM dbo.RestoringDeleted  WHERE (BranchID = " + UserInfo.BRANCHID + ") AND (SN = " + txtDocTypeID.Text + " ) ";
                else
                    strSQL = "SELECT * FROM dbo.RestoringDeleted  WHERE  BranchID = " + UserInfo.BRANCHID;

                if (txtFromDate.Text != string.Empty)
                    strSQL = strSQL + " AND TheDate >=  " + Comon.ConvertDateToSerial(txtFromDate.Text).ToString();

                if (txtToDate.Text != string.Empty)
                    strSQL = strSQL + "AND TheDate <=  " + Comon.ConvertDateToSerial(txtToDate.Text).ToString();

                strSQL = strSQL + " ORDER BY SN";

                dt = Lip.SelectRecord(strSQL);
                gridControl1.DataSource = dt;
                btnShow.Visible = true;
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
        private void txtDocTypeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSQL = "SELECT DISTINCT RecordTypeArb FROM  dbo.RestoringDeleted WHERE (SN = " + txtDocTypeID.Text + ") AND (BranchID = " + UserInfo.BRANCHID + "  )";
                CSearch.ControlValidating(txtDocTypeID, lblDocTypeName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmRestoringDeleted_KeyDown(object sender, KeyEventArgs e)
        {
            // if (e.KeyCode == Keys.F3)
            //  PubEventType = "KeyDown";
            // Lip.HotKeys(Me, e.KeyCode, e.Shift);
            //   Find();
        }

        private void txtDocTypeID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();

        }
        public void UpdatePurchaseTable(string TableName, string PrimaryKeyName, double PrimaryKeyValue)
        {

            try
            {

                Lip.NewFields();
                Lip.Table = TableName;
                Lip.AddNumericField("Cancel", 0);

                Lip.sCondition = " " + PrimaryKeyName + " = " + PrimaryKeyValue + " AND BranchID = " + UserInfo.BRANCHID + " ";
                Lip.ExecuteUpdate();


                Lip.NewFields();
                Lip.Table = "Sales_PurchaseInvoiceDetails";
                Lip.AddNumericField("Cancel", 0);
                Lip.sCondition = " " + PrimaryKeyName + " = " + PrimaryKeyValue + " AND BranchID = " + UserInfo.BRANCHID + " ";
                Lip.ExecuteUpdate();

                RestoringDone = true;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }





        }
        public void UpdateTable(string TableName, string PrimaryKeyName, double PrimaryKeyValue)
        {
            try
            {

                string strSQL;
                strSQL = "UPDATE " + TableName + " SET Cancel = 0 WHERE (" + PrimaryKeyName + " = " + PrimaryKeyValue + ") AND (BranchID = " + UserInfo.BRANCHID + " )";
                Lip.ExecututeSQL(strSQL);
                RestoringDone = true;


            }
            catch (Exception ex)
            {
                // SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }




        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (var rowHandle in gridView1.GetSelectedRows())
                {
                    if (gridView1.GetRowCellValue(rowHandle, "RecordType").ToString() == "Sales_PurchaseInvoiceMaster")
                        UpdatePurchaseTable(gridView1.GetRowCellValue(rowHandle, "RecordType").ToString(), (gridView1.GetRowCellValue(rowHandle, "PK").ToString()), (Comon.cInt(gridView1.GetRowCellValue(rowHandle, "ID"))));
                    else
                        UpdateTable(gridView1.GetRowCellValue(rowHandle, "RecordType").ToString(), gridView1.GetRowCellValue(rowHandle, "PK").ToString(), Comon.cDbl(gridView1.GetRowCellValue(rowHandle, "ID").ToString()));
                }

                Messages.MsgInfo(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Restoring Selected Doc. Done Successfully" : "تم استعادة المستندات المختارة بنجاح"));
                btnShow.PerformClick();

            }
            catch (Exception ex)
            {
                // SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }



        }

    }
}

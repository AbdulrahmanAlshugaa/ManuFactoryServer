using Edex.Model;
using Edex.Model.Language;
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
    public partial class frmNotifications : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public bool CancelDone;
        
        string RecordTypeSalse = "'فاتورة مبيعات'";
        string RecordTypePurchase = "'فاتورة مشتريات'";


        public frmNotifications()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
         
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;

            if (UserInfo.Language == iLanguage.English)
            {
                RecordTypeSalse = "'Salse Invoice'";
                RecordTypePurchase = "'Purchase Invoice'";
            }
        }
        protected override void DoPrint()
        {
            gridControl1.ShowRibbonPrintPreview();
        }

        protected override void DoSave()
        {
            try
            {
                CancelDone = false;
                for (int i = 0; i <= gridView1.RowCount - 1; i++)
                {
                    if (Comon.cbool(gridView1.GetRowCellValue(i, "gridColumn1").ToString()) == true)
                    {
                        switch (gridView1.GetRowCellValue(i, "dgvColRecordType").ToString())
                        {
                            case "PurchaseInvoice":
                                UpdateTable("Sales_PurchaseInvoiceMaster", "InvoiceID", Comon.cLong(gridView1.GetRowCellValue(i, "ID").ToString()));
                                break;
                            case "SalesInvoice":
                                UpdateTable("Sales_SalesInvoiceMaster", "InvoiceID", Comon.cLong(gridView1.GetRowCellValue(i, "ID").ToString()));
                                break;



                        }



                    }
                }


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        public  void btnShow_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;
            if (cmbDocType.EditValue == null)
                cmbDocType.EditValue = "AllDocs";

            switch (cmbDocType.EditValue.ToString())
            {
                case "PurchaseInvoice":
                    PurchaseInvoice();
                    break;
                case "SalesInvoice":
                    SalesInvoice();
                    break;
                case "AllDocs":
                    AllDoc();
                    break;

                case "StockingDocs":
                    AllDoc();
                    break;


            }


        }
        public void PurchaseInvoice()
        {

            DataTable dt;
            string strSQL;
           

            strSQL = "SELECT " + RecordTypePurchase + " AS RecordType, InvoiceID AS ID , Notes AS Declaration, WarningDate AS NotificationDate, "
            + " CheckSpendDate AS MeritDate FROM dbo.Sales_PurchaseInvoiceMaster WHERE  (BranchID = " + UserInfo.BRANCHID + ") AND (Cancel = 0)"
            + " AND (WarningDate <= " + Comon.cLong(Lip.GetServerDateSerial()) + ") AND (WarningDate <> 0) ORDER BY ID, EditTime";

            //Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL,"Arb");
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                gridControl1.DataSource = dt;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (Comon.cLong(dt.Rows[i]["NotificationDate"].ToString()) > Comon.cLong(Lip.GetServerDateSerial()))
                    { 
                        gridView1.Appearance.FocusedRow.BackColor = Color.FromArgb(225, 116, 101);
                    }
                }


            }

        }
        public void SalesInvoice()
        {
            DataTable dt;
            string strSQL;
           
            strSQL = "SELECT " + RecordTypeSalse + " AS RecordType, InvoiceID AS ID , Notes AS Declaration, WarningDate AS NotificationDate, "
              + " CheckSpendDate AS MeritDate FROM  dbo.Sales_SalesInvoiceMaster WHERE  (BranchID = " + UserInfo.BRANCHID + ") AND (Cancel = 0)"
              + " AND (WarningDate <= " + Comon.cLong(Lip.GetServerDateSerial()) + ") AND (WarningDate <> 0) ORDER BY ID, EditTime";
            
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                gridControl1.DataSource = dt;

            }

        }

        public void AllDoc()
        {
            DataTable dt;
            string strSQL;
             

            strSQL = "SELECT " + RecordTypeSalse + " AS RecordType, InvoiceID AS ID , Notes AS Declaration, WarningDate AS NotificationDate, "
              + " CheckSpendDate AS MeritDate , EditTime FROM  dbo.Sales_SalesInvoiceMaster  ";

            strSQL = strSQL + " Union SELECT " + RecordTypePurchase + " AS RecordType, InvoiceID AS ID , Notes AS Declaration, WarningDate AS NotificationDate, "
           + " CheckSpendDate AS MeritDate , EditTime FROM dbo.Sales_PurchaseInvoiceMaster WHERE  (BranchID = " + UserInfo.BRANCHID + ") AND (Cancel = 0)"
           + " AND (WarningDate <= " + Comon.cLong(Lip.GetServerDateSerial()) + ") AND (WarningDate <> 0) ORDER BY ID, EditTime";



            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                gridControl1.DataSource = dt;

            }


        }

        public void UpdateTable(string TableName, string PrimaryKeyName, double PrimaryKeyValue)
        {
            try
            {

                string strSQL;
                strSQL = "UPDATE " + TableName + " SET WarningDate = 0 WHERE (" + PrimaryKeyName + " = " + PrimaryKeyValue + ") AND (BranchID = " + UserInfo.BRANCHID + " )";
                Lip.ExecututeSQL(strSQL);
                CancelDone = true;


            }
            catch (Exception ex)
            {
                // SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }




        }
        public void FillComboBox(DevExpress.XtraEditors.LookUpEdit cmb, string Tablename, string Code, string Name, string OrderByField = "")
        {
            string strSQL = "SELECT " + Code + " AS [الرقم]," + Name + "  AS [الاسم] FROM " + Tablename;
            if (OrderByField != "")
                strSQL = strSQL + " Order By " + OrderByField;
            cmb.Properties.DataSource = Lip.SelectRecord(strSQL).DefaultView;
            cmb.Properties.DisplayMember = "الاسم";
            cmb.Properties.ValueMember = "الرقم";
        }

        private void frmNotifications_Load(object sender, EventArgs e)
        {
            FillComboBox(cmbDocType, "NotificationTypes", "DocName", (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName"));
            cmbDocType.EditValue = 0;
        }


    }
}

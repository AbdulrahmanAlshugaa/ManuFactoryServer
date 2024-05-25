
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Reports
{
    public partial class frmSalesInPeriodByItem : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string FocusedControl;
        public string primaaryLey;

        private string strSQL = "";
        private string where = "";

        DataTable barc = new DataTable();

        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();
        public frmSalesInPeriodByItem()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Caption = "استعلام جديد";
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
            ///////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // this.txtToDate.EditValue = DateTime.Now;
            this.txtBarCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtOldBarcodeID_Validating);
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;
            if (UserInfo.Language == iLanguage.English)
            {
                dgvColBarCode.Caption = "Bar Code ";
                dgvColItemName.Caption = "Item Name";
                dgvColTotalQty.Caption = "Total  Quntity ";
                dgvColTotalPurchase.Caption = "Total Purchase";
                dgvColTotalDiscount.Caption = "Total Discount  ";

                dgvolSn.Caption = "# ";
                dgvColNet.Caption = "Net";


                btnShow.Text = btnShow.Tag.ToString();
                //  Label8.Text = btnShow.Tag.ToString();

            }

        }
        private void PurchaseInvoice()
        {
            try
            {
                DataRow row;
                dt = Lip.SelectRecord(GetStrSQL());
                _sampleData.Clear();
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();

                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["BarCode"] = dt.Rows[i]["BarCode"].ToString();



                            row["TotalQty"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["TotalQty"]).ToString("N" + 2);
                            row["TotalSales"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["TotalSales"]).ToString("N" + 2));
                            row["TotalDiscount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["TotalDiscount"]).ToString("N" + 2));
                            row["Net"] = (Comon.ConvertToDecimalPrice(row["TotalSales"]) - Comon.ConvertToDecimalPrice(row["TotalDiscount"])).ToString("N" + 2);

                            row["ItemName"] = dt.Rows[i]["ItemName"];


                            _sampleData.Rows.Add(row);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);



            }
        }



        string GetStrSQL()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();
                
                string filter = "(.Sales_TotalSales.BranchID = " + cmbBranchesID.EditValue + ")    AND";


                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));

                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + " .Sales_TotalSales.InvoiceDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " .Sales_TotalSales.InvoiceDate <=" + FromDate + " AND ";
                if (txtBarCode.Text != string.Empty)
                    filter = filter + " .Sales_TotalSales.BarCode  = '" + txtBarCode.Text + "'   AND ";
                filter = filter.Remove(filter.Length - 4, 4);
                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal, غير موجود في جدول مردود المشتريات
                strSQL = "SELECT SUM(dbo.Sales_TotalSales.QTY) AS TotalQTY, SUM(dbo.Sales_TotalSales.TotalSales) AS TotalSales, "

     + " SUM(dbo.Sales_TotalSales.Discount) AS TotalDiscount, dbo.Sales_TotalSales.BarCode, dbo.Stc_Items.ArbName AS ItemName "

   + " FROM dbo.Stc_Items RIGHT OUTER JOIN dbo.Stc_ItemUnits ON dbo.Stc_Items.ItemID = dbo.Stc_ItemUnits.ItemID RIGHT OUTER JOIN"
              + " dbo.Sales_TotalSales ON dbo.Stc_ItemUnits.BarCode = dbo.Sales_TotalSales.BarCode WHERE " + filter
              + "  GROUP BY dbo.Sales_TotalSales.BarCode, dbo.Stc_Items.ArbName ";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return  strSQL ;
        }
        protected override void DoNew()
        {
            try
            {

                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtBarCode.Text = "";
                txtOldBarcodeID_Validating(null, null);
                txtBarCode.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtFromDate.Text = "";
                txtToDate.Text = "";

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }


        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            PurchaseInvoice();
            gridControl1.DataSource = _sampleData;
            if (gridView1.RowCount > 0)
            {
                btnShow.Visible = true;

                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                txtBarCode.Enabled = false;

            }
            else
            {

                Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");

                btnShow.Visible = true;
                DoNew();
            }
        }

        private void frmSalesInPeriodByItem_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("BarCode", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TotalQty", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("TotalSales", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("TotalDiscount", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Net", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("ItemName", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("F1", typeof(string)));

            strSQL = "ArbName";
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = UserInfo.BRANCHID;
            if (UserInfo.ID == 1)
            {
                cmbBranchesID.Visible = true;
                labelControl9.Visible = true;
            }

            else
            {
                cmbBranchesID.Visible = false;
                labelControl9.Visible = false;
            }

            if (UserInfo.BRANCHID == 1)
            {
                cmbBranchesID.Visible = true;
                labelControl9.Visible = true;
            }

            else
            {
                labelControl9.Visible = false;
                cmbBranchesID.Visible = false;
            }


        }
        #region Function
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
                ReportName = "rptSalesInPeriodByItem";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["ItemName"].Value = lblBarCodeName.Text.Trim().ToString();
                rptForm.Parameters["BarCode"].Value = txtBarCode.Text.Trim().ToString();
                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInPeriodByItemDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView1.GetRowCellValue(i, "ItemName").ToString();
                    row["TotalQty"] = gridView1.GetRowCellValue(i, "TotalQty").ToString();
                    row["TotalPurchase"] = gridView1.GetRowCellValue(i, "TotalSales").ToString();
                    row["TotalDiscount"] = gridView1.GetRowCellValue(i, "TotalDiscount").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptSalesInPeriodByItem";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
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
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                   if (dt.Rows.Count > 0)for (int i = 1; i < 6; i++)
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

        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();




            if (UserInfo.Language == iLanguage.Arabic)
                PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "اسـم الـمـادة", "البـاركـود");
            else
                PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "ItemName", "BarCode");



            }
            catch { }


        }
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }

            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }

            return c.Name;
        }


        private void txtOldBarcodeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {


                //      strSQLForBarcode = " SELECT   TOP (1)  Stc_Items.ArbName AS ItemName, Sales_SalesInvoiceDetails.SizeID, Stc_SizingUnits.ArbName AS SizeName, "
                //+ " Sales_SalesInvoiceDetails.BarCode, Sales_SalesInvoiceDetails.ExpiryDate, Sales_SalesInvoiceDetails.InvoiceID, Stc_ItemsSizes.ArbName AS Size, "
                // + " Stc_ItemsBrands.ArbName AS BrandName   FROM  Stc_ItemsSizes RIGHT OUTER JOIN   Stc_Items ON Stc_ItemsSizes.SizeID = Stc_Items.SizeID LEFT OUTER JOIN "
                // + " Stc_ItemsBrands ON Stc_Items.BrandID = Stc_ItemsBrands.BrandID RIGHT OUTER JOIN    Sales_SalesInvoiceDetails LEFT OUTER JOIN "
                //+ " Stc_SizingUnits ON Sales_SalesInvoiceDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Sales_SalesInvoiceDetails.ItemID "
                //+ "  WHERE  (Sales_SalesInvoiceDetails.BarCode ='" + txtBarCode.Text + "') AND (Sales_SalesInvoiceDetails.Cancel = 0)";
                string strSQLForBarcode = " SELECT   TOP (1)  Stc_Items.ArbName AS ItemName   FROM  Stc_Items RIGHT OUTER JOIN       Sales_PurchaseInvoiceDetails LEFT OUTER JOIN "
             + " Stc_SizingUnits ON Sales_PurchaseInvoiceDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Sales_PurchaseInvoiceDetails.ItemID "
             + "  WHERE  (Sales_PurchaseInvoiceDetails.BarCode ='" + txtBarCode.Text + "') AND (Sales_PurchaseInvoiceDetails.Cancel = 0)";

                //    strSQLForBarcode = " SELECT   TOP (1) Sales_PurchaseInvoiceDetails.ItemID, Stc_Items.ArbName AS ItemName, Sales_PurchaseInvoiceDetails.SizeID, Stc_SizingUnits.ArbName AS SizeName, "
                //+ " Sales_PurchaseInvoiceDetails.BarCode, Sales_PurchaseInvoiceDetails.ExpiryDate, Sales_PurchaseInvoiceDetails.InvoiceID, Stc_ItemsSizes.ArbName AS Size, "
                //+ " Stc_ItemsBrands.ArbName AS BrandName   FROM  Stc_ItemsSizes RIGHT OUTER JOIN   Stc_Items ON Stc_ItemsSizes.SizeID = Stc_Items.SizeID LEFT OUTER JOIN "
                //+ " Stc_ItemsBrands ON Stc_Items.BrandID = Stc_ItemsBrands.BrandID RIGHT OUTER JOIN    Sales_PurchaseInvoiceDetails LEFT OUTER JOIN "
                //+ " Stc_SizingUnits ON Sales_PurchaseInvoiceDetails.SizeID = Stc_SizingUnits.SizeID ON Stc_Items.ItemID = Sales_PurchaseInvoiceDetails.ItemID "
                //+ "  WHERE  (Sales_PurchaseInvoiceDetails.BarCode ='" + txtBarCode.Text + "') AND (Sales_PurchaseInvoiceDetails.Cancel = 0)";
                if (UserInfo.Language == iLanguage.English)
                {
                    strSQLForBarcode = strSQLForBarcode.Replace("ArbName", "EngName");

                }
                barc = Lip.SelectRecord(strSQLForBarcode);
                if (barc.Rows.Count > 0)
                    lblBarCodeName.Text = barc.Rows[0]["ItemName"].ToString();
                else
                {
                    lblBarCodeName.Text = "";
                    txtBarCode.Text = "";
                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #endregion
        protected override void DoSearch()
        {
            try
            {
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmSalesInPeriodByItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }
        private void txtFromDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Comon.ConvertDateToSerial(txtFromDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtFromDate.Text = Lip.GetServerDate();
        }

        private void txtToDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Comon.ConvertDateToSerial(txtToDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtToDate.Text = Lip.GetServerDate();
        }
    }
}

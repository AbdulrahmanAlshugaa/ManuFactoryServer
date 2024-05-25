using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.SalesAndSaleObjects.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Reports
{
    public partial class frmNetworkCommission : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string FocusedControl;

        private string strSQL = "";
        private string where = "";
        DataTable dt = new DataTable();

        public DataTable _sampleData = new DataTable();
        public frmNetworkCommission()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ///////////////////////////////////////////////////////
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtFromDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            // this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtToDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            // this.txtToDate.EditValue = DateTime.Now;
            ///////////////////////////////////////////////////////
       
            strSQL = "EngName";
            if (UserInfo.Language == iLanguage.Arabic)
                strSQL = "ArbName";

            FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

            Common.filllookupEDit(ref repositoryItemLookUpEdit2, "NetTypeID", "NetType", strSQL, "0=0");
      
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;




            if (UserInfo.Language == iLanguage.English)
            {
                dgvolSn.Caption = "# ";
                dgvColInvoiceID.Caption = "Invoice NO";
                dgvColInvoiceDate.Caption = "Invoice  Date ";
                dgvColTotal.Caption = "Total ";
                dgvColVatAmount.Caption = "Total VatAmount  ";

                dgvColMethodeName.Caption = "Method Sale";
                dgvColNet.Caption = "Net";


                dgvColDiscount.Caption = "Discount ";


                 dgvColSellerName.Caption = "Seller Name ";
                dgvColVatID.Caption = "Vat  ID";
                dgvColStoreName.Caption = "Stotre   Name ";
                dgvColCostCenterName.Caption = "Cost Center";
               dgvColDelgateName.Caption = "Delgate Name ";

                    dgvColNotes.Caption = "Notes";
                dgvColCloseCashierDate.Caption = "Close  CashierDate ";
                dgvColProfite.Caption = " Profit";
                dgvCustomerName.Caption = "Customer Name  ";



                btnShow.Text = btnShow.Tag.ToString();
                //  Label8.Text = btnShow.Tag.ToString();















            }























        }
        #region Function
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where BranchID=" + UserInfo.BRANCHID;

            FocusedControl = GetIndexFocusedControl();

         

         




        


          






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
        private void Label13_Click(object sender, EventArgs e)
        {

        }

        private void frmSalesInvoiceReport_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("nvoiceDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CloseCashierDate", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("UserID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("OrderTypes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("NetPaid", typeof(string)));


            _sampleData.Columns.Add(new DataColumn("Total", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Profit", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("Discount", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("VatAmount", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Net", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("MethodeName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("SellerName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("VatID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CostCenterName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("DelgateName", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("F1", typeof(string)));
        }
        string GetStrSQL()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                btnShow.Visible = false;
                Application.DoEvents();

                string filter = "( .Sales_SalesInvoiceMaster.BranchID = " + UserInfo.BRANCHID + ") AND (dbo.Sales_SalesInvoiceMaster.MethodeID=5 or dbo.Sales_SalesInvoiceMaster.MethodeID =3 ) and dbo.Sales_SalesInvoiceMaster.InvoiceID >0 AND dbo.Sales_SalesInvoiceMaster.Cancel =0   AND";
                strSQL = "";
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            
                DataTable dt;
                // Dim dtMethodeName As DataTable
                // حسب الرقم
                //if(rdLocall.Checked==true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'محلي'  or Sales_SalesInvoiceMaster.OrderType =  'Local')    AND ";

                //if (rdTakeaway.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'TakeAway'  or Sales_SalesInvoiceMaster.OrderType =  'سـفري')    AND ";

                //if (rdHunger.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'HangerStation' or Sales_SalesInvoiceMaster.OrderType =  'هنجر ستيشن')    AND ";

                //if (rdDelivery.Checked == true)
                //    filter = filter + "( Sales_SalesInvoiceMaster.OrderType =  'Delivery'  or Sales_SalesInvoiceMaster.OrderType =  'توصيل')    AND ";

           

                // حسب التاريخ
                if (FromDate != 0)
                    filter = filter + " .Sales_SalesInvoiceMaster.InvoiceDate >=" + FromDate + " AND ";

                if (ToDate != 0)
                    filter = filter + " .Sales_SalesInvoiceMaster.InvoiceDate <=" + ToDate + " AND ";
            
                // '''البائع''''العميل''''التكلفة''''المستودع





                if (cmbNetType.Text != string.Empty && Comon.cDbl(cmbNetType.EditValue) != 0)
                    filter = filter + " Sales_SalesInvoiceMaster.NetAccount =" + cmbNetType.EditValue + " AND ";

                ////////f
             

            
                
                filter = filter.Remove(filter.Length - 4, 4);

                //  dbo.Sales_PurchaseInvoiceReturnMaster.AdditionaAmmountTotal,dbo.Sales_SalesInvoiceReturnMaster.AdditionaAmmountTotal AS SumVat, غير موجود في جدول مردود المشتريات
                strSQL = "  SELECT  Sales_SalesInvoiceMaster.NetAccount,  Sales_SalesInvoiceMaster.NetAccount,   Sales_SalesInvoiceMaster.UserID,Sales_SalesInvoiceMaster.OrderType, Sales_SalesInvoiceMaster.VATID ,   Sales_SalesInvoiceMaster.CustomerName AS CustomerName1, dbo.Sales_SalesInvoiceMaster.InvoiceID,Sales_SalesInvoiceMaster.NetAmount ,Sales_SalesInvoiceMaster.Notes,Sales_SalesInvoiceMaster.NetProcessID,dbo.Sales_SalesInvoiceMaster.MethodeID,dbo.Sales_SalesInvoiceMaster.CloseCashierDate,Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.InvoiceDate, "
            + "  SUM(dbo.Sales_SalesInvoiceDetails.QTY * dbo.Sales_SalesInvoiceDetails.SalePrice) AS Total , Sales_SalesInvoiceMaster.AdditionaAmountTotal AS SumVat ,Sum(Sales_SalesInvoiceDetails.Discount) As DiscountLines, dbo.Sales_SalesInvoiceMaster.Notes, "
            + "  dbo.Stc_Stores.ArbName AS Storname,dbo.Sales_Customers.VatID ,dbo.Sales_SalesMethodes.ArbName AS MethodeName, dbo.Sales_Customers.ArbName AS CustomerName , dbo.Sales_SalesInvoiceMaster.CustomerID, "
            + "  dbo.Sales_Sellers.ArbName AS SellerName,Sales_SalesDelegate.ArbName As SaleDelegateName, dbo.Acc_CostCenters.ArbName as CostCenterName , dbo.Sales_SalesInvoiceMaster.CostCenterID"
            + "  ,SUM(((dbo.Sales_SalesInvoiceDetails.SalePrice - dbo.Sales_SalesInvoiceDetails.CostPrice) * dbo.Sales_SalesInvoiceDetails.QTY)-Sales_SalesInvoiceDetails.Discount) AS Profit , Clinic_InsuranceCompany.ArbName AS CompanyName"
            + " FROM dbo.Sales_SalesInvoiceDetails INNER JOIN"
            + " dbo.Sales_SalesInvoiceMaster ON dbo.Sales_SalesInvoiceDetails.InvoiceID = dbo.Sales_SalesInvoiceMaster.InvoiceID AND "
            + " dbo.Sales_SalesInvoiceDetails.BranchID = dbo.Sales_SalesInvoiceMaster.BranchID LEFT OUTER JOIN "
            + " Clinic_InsuranceCompany ON Sales_SalesInvoiceMaster.CustomerID = Clinic_InsuranceCompany.CompanyID AND "
            + " Sales_SalesInvoiceMaster.BranchID = Clinic_InsuranceCompany.BranchID "
            + " LEFT OUTER JOIN"
            + " dbo.Sales_SalesDelegate ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_SalesDelegate.BranchID AND "
            + " dbo.Sales_SalesInvoiceMaster.DelegateID = dbo.Sales_SalesDelegate.DelegateID LEFT OUTER JOIN"
            + " dbo.Acc_CostCenters ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Acc_CostCenters.BranchID AND"
            + " dbo.Sales_SalesInvoiceMaster.CostCenterID = dbo.Acc_CostCenters.CostCenterID LEFT OUTER JOIN"
            + " dbo.Sales_Customers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Customers.BranchID AND"
            + " dbo.Sales_SalesInvoiceMaster.CustomerID = dbo.Sales_Customers.CustomerID LEFT OUTER JOIN"
            + " dbo.Sales_Sellers ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Sales_Sellers.BranchID AND"
            + " dbo.Sales_SalesInvoiceMaster.SellerID = dbo.Sales_Sellers.SellerID LEFT OUTER JOIN"
            + " dbo.Stc_Stores ON dbo.Sales_SalesInvoiceMaster.BranchID = dbo.Stc_Stores.BranchID AND"
            + " dbo.Sales_SalesInvoiceMaster.StoreID = dbo.Stc_Stores.StoreID LEFT OUTER JOIN"
            + " dbo.Sales_SalesMethodes ON dbo.Sales_SalesInvoiceMaster.MethodeID = dbo.Sales_SalesMethodes.MethodID"
            + " where " + filter
            + "  GROUP BY Sales_SalesInvoiceMaster.NetAccount,   Sales_SalesInvoiceMaster.UserID,  Sales_SalesInvoiceMaster.OrderType, Sales_SalesInvoiceMaster.VATID ,  Sales_SalesInvoiceMaster.CustomerName, Sales_SalesInvoiceMaster.Notes,Sales_SalesInvoiceMaster.NetAmount , Sales_SalesInvoiceMaster.AdditionaAmountTotal,Sales_SalesInvoiceMaster.NetProcessID,dbo.Sales_SalesInvoiceMaster.MethodeID,  dbo.Sales_SalesInvoiceMaster.InvoiceID,Sales_SalesInvoiceMaster.DiscountOnTotal, dbo.Sales_SalesInvoiceMaster.InvoiceDate, dbo.Sales_SalesInvoiceMaster.Notes, dbo.Stc_Stores.ArbName, "
            + "  dbo.Sales_SalesMethodes.ArbName,dbo.Sales_Customers.VatID,dbo.Sales_SalesInvoiceMaster.CloseCashierDate, dbo.Sales_Customers.ArbName, dbo.Sales_SalesInvoiceMaster.CustomerID, dbo.Sales_Sellers.ArbName,dbo.Sales_SalesDelegate.ArbName, dbo.Acc_CostCenters.ArbName, "
            + "  dbo.Sales_SalesInvoiceMaster.CostCenterID, dbo.Sales_SalesInvoiceMaster.Cancel , Clinic_InsuranceCompany.ArbName HAVING (dbo.Sales_SalesInvoiceMaster.Cancel = 0) ";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            return Lip.ConvertStrSQLLanguage(strSQL, iLanguage.English.ToString());

        }
        private void SalesInvoice()
        {
            try
            {
                decimal netSum = 0;
                        decimal netCashSum = 0;
                        decimal caschPaidWithNet = 0;
                        decimal cash = 0;
                        decimal future = 0;
                        decimal check1 = 0;
                             decimal total = 0;
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
                         
                            row["InvoiceID"] = dt.Rows[i]["InvoiceID"].ToString();
                            row["Notes"] = dt.Rows[i]["Notes"].ToString();
                            row["UserID"] = dt.Rows[i]["NetAccount"].ToString();
                            row["OrderTypes"] = dt.Rows[i]["OrderType"].ToString();
                            row["CloseCashierDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["CloseCashierDate"].ToString());

                            row["nvoiceDate"] = Comon.ConvertSerialDateTo(dt.Rows[i]["InvoiceDate"].ToString());
                            row["Total"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Total"]).ToString("N" + 2);
                            row["Discount"] = (Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountLines"]) + Comon.ConvertToDecimalPrice(dt.Rows[i]["DiscountOnTotal"])).ToString("N" + 2);
                            row["VatAmount"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["SumVat"]).ToString("N" + 2);
                            row["Net"] = (Comon.ConvertToDecimalPrice(row["Total"]) - Comon.ConvertToDecimalPrice(row["Discount"]) + Comon.ConvertToDecimalPrice(row["VatAmount"])).ToString("N" + 2);
                            row["SellerName"] = dt.Rows[i]["SellerName"];
                            row["Profit"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["Profit"]).ToString("N" + 2);
                            row["MethodeName"] = dt.Rows[i]["MethodeID"];
                          //  row["CustomerName"] = (dt.Rows[i]["CustomerName"].ToString() != string.Empty ? dt.Rows[i]["CustomerName"] : "");
                            row["VatID"] = (dt.Rows[i]["VATID"].ToString() != string.Empty ? dt.Rows[i]["VATID"] : "");
                            row["StoreName"] = dt.Rows[i]["StorName"];
                            row["CostCenterName"] = (dt.Rows[i]["CostCenterName"].ToString() != string.Empty ? dt.Rows[i]["CostCenterName"] : "");
                            row["DelgateName"] = (dt.Rows[i]["SaleDelegateName"].ToString() != string.Empty ? dt.Rows[i]["SaleDelegateName"] : "");
                            total += Comon.ConvertToDecimalPrice(row["Net"]);
                            switch (Comon.cInt(dt.Rows[i]["MethodeID"].ToString()))
                            {

                                case (1):
                                    cash += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    //row["CustomerName"] = (dt.Rows[i]["CustomerName1"].ToString() != string.Empty ? dt.Rows[i]["CustomerName1"] : "");
                                    row["NetPaid"] = "-";
                                    break;
                                case (2):
                                    future += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    row["NetPaid"] = "-";
                                    break;
                                case (3):
                                    netSum += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                  //  row["CustomerName"] = (dt.Rows[i]["CustomerName1"].ToString() != string.Empty ? dt.Rows[i]["CustomerName1"] : "");
                                    row["Notes"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (4):
                                    check1 += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                 //   row["CustomerName"] = (dt.Rows[i]["CustomerName1"].ToString() != string.Empty ? dt.Rows[i]["CustomerName1"] : "");
                                   // row["NetPaid"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                                case (5):
                                    netCashSum += ((Comon.ConvertToDecimalPrice(row["Net"])));
                                    caschPaidWithNet += Comon.ConvertToDecimalPrice(dt.Rows[i]["NetAmount"]);
                                    row["NetPaid"] = caschPaidWithNet;
                                  //  row["CustomerName"] = (dt.Rows[i]["CustomerName1"].ToString() != string.Empty ? dt.Rows[i]["CustomerName1"] : "");
                                    row["Notes"] = dt.Rows[i]["NetProcessID"].ToString();
                                    break;
                            }
                            _sampleData.Rows.Add(row);
                        }
                    }
                    netSum=netSum+caschPaidWithNet;

                    lblNet.Text = netSum.ToString();
                  
                    lblCashNet.Text = netCashSum.ToString();
                  
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
        protected override void DoNew()
        {
            try
            {

                _sampleData.Clear();
                gridControl1.RefreshDataSource();
           
          
              
            

               
            

                txtSalesDelegateID.Text = "";
              


                txtSalesDelegateID.Enabled = true;

              
                cmbNetType.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
             

                txtFromDate.Text = "";
                txtToDate.Text = "";

            
                cmbNetType.ItemIndex = -1;
               
                lblNet.Text = "";
            
                lblCashNet.Text = "";
            

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }


        }
        private void btnShow_Click(object sender, EventArgs e)
        {

            SalesInvoice();
            gridControl1.DataSource = _sampleData;
            if (gridView1.RowCount > 0)
            {
                btnShow.Visible = true;

            
             
                txtSalesDelegateID.Enabled = false;

                cmbNetType.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
             

            }
            else
            {

                Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");

                btnShow.Visible = true;
                DoNew();
            }



        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {



        }

        private void frmSalesInvoiceReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();

        }
        ////////////// print_COde//////////////////////////////////
        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/

                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");

                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;
            
                rptForm.Parameters["SalesDelegateName"].Value = lblSalesDelegateName.Text.Trim().ToString();
                rptForm.Parameters["MethodName"].Value = cmbNetType.Text.Trim().ToString();
               
                rptForm.Parameters["NetSum"].Value = lblNet.Text.Trim().ToString();
                rptForm.Parameters["CashNetSum"].Value = lblCashNet.Text.Trim().ToString();
             
        
                rptForm.Parameters["FromDate"].Value = txtFromDate.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptSalesInvoiceReportDataTable();

                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();

                    row["#"] = i + 1;
                    row["InvoiceID"] = gridView1.GetRowCellValue(i, "InvoiceID").ToString();
                    row["nvoiceDate"] = gridView1.GetRowCellValue(i, "nvoiceDate").ToString();
                    row["CloseCashierDate"] = gridView1.GetRowCellValue(i, "CloseCashierDate").ToString();

                    row["Total"] = gridView1.GetRowCellValue(i, "Total").ToString();
                    row["Discount"] = gridView1.GetRowCellValue(i, "Discount").ToString();
                    row["VatAmount"] = gridView1.GetRowCellValue(i, "VatAmount").ToString();
                    row["Net"] = gridView1.GetRowCellValue(i, "Net").ToString();
                    row["Profit"] = gridView1.GetRowCellValue(i, "Profit").ToString();

                    row["MethodeName"] = gridView1.GetRowCellValue(i, "MethodeName").ToString();
                    row["CustomerName"] = gridView1.GetRowCellValue(i, "CustomerName").ToString();
                    row["SellerName"] = gridView1.GetRowCellValue(i, "SellerName").ToString();
                    row["VatID"] = gridView1.GetRowCellValue(i, "VatID").ToString();

                    row["StoreName"] = gridView1.GetRowCellValue(i, "StoreName").ToString();
                    row["CostCenterName"] = gridView1.GetRowCellValue(i, "CostCenterName").ToString();
                    row["DelgateName"] = gridView1.GetRowCellValue(i, "DelgateName").ToString();
                    row["Notes"] = gridView1.GetRowCellValue(i, "Notes").ToString();

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptSalesInvoiceReport";

                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeaderLand();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);
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

        /// <summary>
        /// //////////////////////////////////////////////////////////

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
         }

 

        

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try{
            GridView view = sender as GridView;
            frmSalesInvoice frm = new frmSalesInvoice();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("InvoiceID").ToString()) + 1, 8);
            }
            else
                frm.Dispose();
            }
            catch { }
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

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
      

     

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void labelControl8_Click(object sender, EventArgs e)
        {

        }
        decimal getTotalValueTotal(GridView view, int listSourceRowIndex)
        {
            double CreditAmount = Comon.cDbl(view.GetListSourceRowCellValue(listSourceRowIndex, "Net"));
            double netAccount = Comon.cDbl(view.GetListSourceRowCellValue(listSourceRowIndex, "UserID"));
            if (Comon.cInt(view.GetListSourceRowCellValue(listSourceRowIndex, "MethodeName")) == 5)
                CreditAmount = Comon.cDbl(view.GetListSourceRowCellValue(listSourceRowIndex, "NetPaid"));

            return SaveVouchersDiscount(CreditAmount, netAccount); 
        }

        private decimal SaveVouchersDiscount(double CreditAmount, double InvoiceID)
        {




            double AccountID = 0;
            double PercentDiscount;
            decimal dCreditAmount = 0;
            DataRow[] row;


            var sr = "Select * from NetType where NetTypeID =" + InvoiceID;
            var dt = Lip.SelectRecord(sr);
            if (dt.Rows.Count < 1) return 0;

            if (Comon.cInt(dt.Rows[0]["ISFixed"].ToString()) > 0)
            {

                if (Comon.cInt(dt.Rows[0]["ISamountFixed"].ToString()) > 0)
                {

                    CreditAmount = Comon.cDbl(dt.Rows[0]["amountFixed"].ToString());
                }
                else
                {

                    CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["percentFixed"].ToString()) / 100);

                }


            }
            else if (Comon.cInt(dt.Rows[0]["ISChange"].ToString()) > 0)
            {

                if (CreditAmount < Comon.cDbl(dt.Rows[0]["CostLess"].ToString()))
                {
                    if (Comon.cInt(dt.Rows[0]["ISamountChangeLess"].ToString()) > 0)
                    {
                        CreditAmount = Comon.cDbl(dt.Rows[0]["amountChangeLess"].ToString());

                    }
                    else if (Comon.cInt(dt.Rows[0]["ISPercentChangeLess"].ToString()) > 0)
                    {

                        CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["PercentChangeLess"].ToString()) / 100);

                    }
                }
                else
                {
                    if (Comon.cInt(dt.Rows[0]["ISamountChangeMore"].ToString()) > 0)
                    {

                        CreditAmount = Comon.cDbl(dt.Rows[0]["amountChangeMore"].ToString());

                    }

                    else if (Comon.cInt(dt.Rows[0]["ISPercentChangeMore"].ToString()) > 0)
                    {

                        CreditAmount = CreditAmount * (Comon.cDbl(dt.Rows[0]["PercentChangeMore"].ToString()) / 100);

                    }
                }


            }
            AccountID = Comon.cDbl(cmbNetType.EditValue);
            dCreditAmount = Comon.ConvertToDecimalPrice(CreditAmount);

            return dCreditAmount;

            }
          
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.Column.FieldName == "NetCommis" && e.IsGetData) e.Value =
                  getTotalValueTotal(view, e.ListSourceRowIndex);
               

            }
            catch { }
        }


    }
}

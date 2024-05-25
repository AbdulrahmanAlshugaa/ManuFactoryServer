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
using Edex.GeneralObjects.GeneralForms;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraReports.UI;

namespace Edex.StockObjects.Reports
{
    public partial class frmItemDiamondBalance : BaseForm
    {
        public string FocusedControl;
        private string filter = "";
        private string strSQL = "";
        private string getItemSQL = "";
        private string where = "";
        private string PrimaryName;
        DataTable dt = new DataTable();
        public DataTable _sampleData = new DataTable();

        public frmItemDiamondBalance()
        {
            try
            {
                InitializeComponent();
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Caption = "استعلام جديد";
                // ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
                ///////////////////////////////////////////////////////
                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtBarCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtOldBarcodeID_Validating);
                this.txtSupplierID.Validating += txtSupplierID_Validating;
                //this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);

                gridView1.OptionsView.EnableAppearanceEvenRow = true;
                gridView1.OptionsView.EnableAppearanceOddRow = true;
                gridView1.OptionsBehavior.ReadOnly = true;
                gridView1.OptionsBehavior.Editable = false;
                PrimaryName = "ArbName";
                if (UserInfo.Language == iLanguage.English)
                {
                    dgvColInTotal.Caption = "  IN TOTAL";               
                    dgvColOutTotal.Caption = "OUT  Total ";
                
                    dgvColSN.Caption = "# ";
                    dgvColRecordType.Caption = "Record Type ";
                    dgvColID.Caption = "Trans ID";
       
                    dgvColTempRecordType.Caption = "Item Name ";                     
                    dgvColSizeID.Caption = "Size NO ";
                    btnShow.Text = btnShow.Tag.ToString();
                    Label8.Text = btnShow.Tag.ToString();
                }

            }
            catch { }

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void frmItemDiamondBalance_Load(object sender, EventArgs e)
        {
            _sampleData.Columns.Add(new DataColumn("SN", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("RegTime", typeof(string)));
    
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("BarCode", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("CaratPrice", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("InQtyDaimond_W", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("OutQtyDaimond_W", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("BalanceDaimond_W", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("SupplierName", typeof(string)));

            ///  _sampleData.Columns.Add(new DataColumn("OutTotal", typeof(decimal)));

            _sampleData.Columns.Add(new DataColumn("RecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("TempRecordType", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("F1", typeof(string)));
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
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
        }

        protected override void DoAddFrom()
        {
            try
            {
             
                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtBarCode.Text = "";
                txtStoreID.Text = "";
                txtStoreID_Validating(null, null);
                txtOldBarcodeID_Validating(null, null);
                txtBarCode.Enabled = true;
                txtStoreID.Enabled = true;



            }
            catch (Exception ex)
            {
                
            }
        }

        public void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "arb");
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void txtSupplierID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtSupplierID.Text != string.Empty && txtSupplierID.Text != "0")
                {
                    strSQL = "SELECT " + PrimaryName + " as CustomerName ,VATID,Mobile FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtSupplierID.Text;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());

                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();

                    }
                    else
                    {
                        strSql = "SELECT " + PrimaryName + " as CustomerName,SpecialDiscount,VATID, CustomerID,Mobile   FROM Sales_Customers Where  Cancel =0 And  AccountID =" + txtSupplierID.Text;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSql, UserInfo.Language.ToString());
                        dt = Lip.SelectRecord(strSql);
                        if (dt.Rows.Count > 0)
                        {

                            lblSupplierName.Text = dt.Rows[0]["CustomerName"].ToString();

                        }
                        else
                        {
                            lblSupplierName.Text = "";
                            txtSupplierID.Text = "";

                        }
                    }
                }
                else
                {
                    lblSupplierName.Text = "";
                    txtSupplierID.Text = "";

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void txtOldBarcodeID_Validating(object sender, CancelEventArgs e)
        {
            try
            {



                string strSQLForBarcode = " SELECT   TOP (1)  ArbName ItemName   FROM  Stc_DiamondItemsType   "

             + "  WHERE  (Stc_DiamondItemsType.BarCodeDimond ='" + txtBarCode.Text + "') ";

              
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQLForBarcode, "arb");

                DataTable barc = new DataTable();
                barc = Lip.SelectRecord(strSQLForBarcode);
                if (barc.Rows.Count > 0)
                {
                    lblBarCodeName.Text = barc.Rows[0][0].ToString().ToUpper();
                    txtBarCode.Text = txtBarCode.Text.ToString().ToUpper();
                }
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBarCode.Text == string.Empty && txtStoreID.Text == string.Empty)
                    XtraMessageBox.Show((UserInfo.Language == iLanguage.Arabic ? "يجب ادخال قيمة في الحقول" : "There is no Data to Show it "), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    btnShow.Visible = false;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    Application.DoEvents();
                    ProcessBalance();
                    SortData();
                    gridControl1.DataSource = _sampleData;
                    if (gridView1.RowCount > 0)
                    {
                        btnShow.Visible = true;
                        txtStoreID.Enabled = false;
                        txtBarCode.Enabled = false;
                    }
                    else
                    {
                        Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");
                        btnShow.Visible = true;
                        DoNew();
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

        private void SortData()
        {

            try
            {
                // Copy data from GridView into DataTable----------------------
                DataTable dt = new DataTable(); DataRow row;

                dt = _sampleData.Copy();
                DataView view = dt.DefaultView;
          
                _sampleData.Rows.Clear();
      

      
                decimal QtyInDaimond = 0;
                decimal QtyOutDaimond = 0;


                decimal PriceIn = 0;
                decimal PriceOut = 0;

                for (int i = 0; i <= view.Count - 1; i++)
                {

                    row = _sampleData.NewRow();
                    row = _sampleData.NewRow();
                    row["SN"] = view[i]["SN"]; 
                   
                    row["CaratPrice"] = Comon.ConvertToDecimalPrice(view[i]["CaratPrice"]).ToString("N" + MySession.GlobalPriceDigits);

                    row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(view[i]["InQtyDaimond_W"]);
                    row["OutQtyDaimond_W"] = Comon.ConvertToDecimalPrice(view[i]["OutQtyDaimond_W"]);
                    row["BalanceDaimond_W"] = Comon.ConvertToDecimalPrice(view[i]["BalanceDaimond_W"]);
                    row["ID"] = view[i]["ID"];
                    row["BarCode"] = view[i]["BarCode"];
                    row["RecordType"] = view[i]["RecordType"];
                    row["TempRecordType"] = view[i]["TempRecordType"];
                  
 
                    QtyInDaimond += Comon.ConvertToDecimalQty(view[i]["InQtyDaimond_W"]);
                    QtyOutDaimond += Comon.ConvertToDecimalQty(view[i]["OutQtyDaimond_W"]);

                    

                    PriceIn += Comon.ConvertToDecimalPrice(view[i]["CaratPrice"]);
                    row["SupplierName"] = view[i]["SupplierName"];

                    _sampleData.Rows.Add(row);

                }
   

                decimal QtyBalanceDaimond = Comon.ConvertToDecimalQty(QtyInDaimond - QtyOutDaimond);

        

 

                // PriceIn = Comon.ConvertToDecimalPrice(PriceIn + PriceIn / 100 * MySession.GlobalPercentVat);

                //  double Pricecoste = (Comon.cDbl(PriceIn)*1.74);

                double Pricecoste = (Comon.cDbl(PriceIn));


                decimal BalancePrice = Comon.ConvertToDecimalQty(Comon.cDbl(PriceOut) - Pricecoste);
 

                lblQtyInTotalDaimond.Text = Comon.ConvertToDecimalQty(QtyInDaimond).ToString();
                lblQtyOutTotalDaimond.Text = Comon.ConvertToDecimalQty(QtyOutDaimond).ToString();
                lblQtyBalanceDaimond.Text = Comon.ConvertToDecimalQty(QtyBalanceDaimond).ToString();

                lblTotalInPrice.Text = Comon.ConvertToDecimalQty(Pricecoste).ToString();
                lblTotalOutPrice.Text = Comon.ConvertToDecimalQty(PriceOut).ToString();
                lblBalancePrice.Text = Comon.ConvertToDecimalQty(BalancePrice).ToString();

            }
            catch { }
        }
         
       
        private void ProcessBalance()
        {
            decimal sum = 0;
            decimal sumDIAMOND_W = 0;
          
            try
            {
                DataTable inPrice;
                DataRow row;
               
                dt.Rows.Clear();

                dt = Lip.SelectRecord(TypeDiamond());
                _sampleData.Clear();
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();
                            row["SN"] = _sampleData.Rows.Count + 1;
                         
                           
                            row["CaratPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["CaratPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                            row["InQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["InQtyDaimond_W"]).ToString("N" + 2);
                            row["OutQtyDaimond_W"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["OutQtyDaimond_W"]).ToString("N" + 2);
                            row["BalanceDaimond_W"] = sumDIAMOND_W + (Comon.ConvertToDecimalPrice(row["InQtyDaimond_W"]));
                            sumDIAMOND_W = Comon.ConvertToDecimalPrice(row["BalanceDaimond_W"]);

                            row["CaratPrice"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["CaratPrice"]).ToString("N" + MySession.GlobalPriceDigits);
                         

                            row["OutQtyDaimond_W"] = (Comon.ConvertToDecimalPrice(row["OutQtyDaimond_W"]));

                            DataTable dtSName = Lip.SelectRecord("select ArbName FROM Sales_CustomerAnSublierListArb  Where  AcountID =" + dt.Rows[i]["SupplierID"]);
                            row["SupplierName"] = dtSName.Rows[0]["ArbName"];

                            row["ID"] = dt.Rows[i]["ID"].ToString();
                            row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                            row["RecordType"] = dt.Rows[i]["RecordType"].ToString();
                            _sampleData.Rows.Add(row);

                        }
                    }

                }
            }
            catch  {}
        }

        public string TypeDiamond()
        {
            try
            {
                filter = "";
                filter = "(dbo.Sales_PurchaseDiamondDetails.BranchID = " + Comon.cInt(cmbBranchesID.EditValue) + ") AND dbo.Sales_PurchaseDiamondDetails.InvoiceID > 0 AND dbo.Sales_PurchaseDiamondDetails.Cancel =0   AND";
                strSQL = "";

                if (txtBarCode.Text != string.Empty)
                    filter = filter + " dbo.Sales_PurchaseDiamondDetails.BarCode  ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Sales_PurchaseDiamondDetails.StoreID  =" + Comon.cInt(txtStoreID.Text) + "  AND ";
                if(txtSupplierID.Text!=string.Empty)
                    filter = filter + " dbo.Sales_PurchaseDiamondDetails.SupplierID  ='" + txtSupplierID.Text + "'  AND ";

                filter = filter.Remove(filter.Length - 4, 4);

                strSQL = "SELECT (dbo.Sales_PurchaseDiamondDetails.WeightIn) AS InQtyDaimond_W,Sales_PurchaseDiamondDetails.BarCode,  dbo.Sales_PurchaseDiamondDetails.WeightOut as OutQtyDaimond_W, dbo.Sales_PurchaseDiamondDetails.CaptionOpration as RecordType, dbo.Sales_PurchaseDiamondDetails.TypeOpration, "
                + " dbo.Sales_PurchaseDiamondDetails.TotalPrice CaratPrice,dbo.Sales_PurchaseDiamondDetails.SupplierID,   "
                + " dbo.Sales_PurchaseDiamondDetails.InvoiceID AS ID FROM dbo.Sales_PurchaseDiamondDetails   "
                + " WHERE " + filter;
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);


                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }

        private void frmItemDiamondBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }

        public void Find()
        {
            try
            {
                CSearch cls = new CSearch();
                int[] ColumnWidth = new int[] { 100, 300 };
                string SearchSql = "";
                string Condition = "Where 1=1";

                FocusedControl = GetIndexFocusedControl();
                if (FocusedControl.Trim() == txtSupplierID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtSupplierID, lblSupplierName, "CustomerIDAndSublierID", "SublierID ID", MySession.GlobalBranchID);
                }
                if (FocusedControl.Trim() == txtStoreID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        //  PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسم الـمـســتـودع","رقم الـمـســتـودع");
                        PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسـم الـمـســتـودع", "رقم الـمـســتـودع");
                    else
                        PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "Store Name", "Store ID");
                }
                else if (FocusedControl.Trim() == txtBarCode.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeDimond", "اسـم الـمـادة", "البـاركـود");
                    else
                        PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeDimond", "Item Name", "BarCode");
                }

                GetSelectedSearchValue(cls);
            }
            catch { }
         

        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }

                else if (FocusedControl ==txtSupplierID.Name)
                {
                    txtSupplierID.Text = cls.PrimaryKeyValue.ToString();
                    txtSupplierID_Validating(null, null);
                }
                else if (FocusedControl ==txtBarCode.Name)
                {
                    txtBarCode.Text = cls.PrimaryKeyValue.ToString();
                    txtOldBarcodeID_Validating(null, null);
                }
                 
                
            }

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

        protected override void DoPrint()
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                /******************** Report Body *************************/
                bool IncludeHeader = true;
                ReportName = "rptItemDiamondBalanc";
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;
                rptForm.Parameters["ItemName"].Value = lblBarCodeName.Text.Trim().ToString();
                rptForm.Parameters["BarCode"].Value = txtBarCode.Text.Trim().ToString();
                rptForm.Parameters["storeName"].Value = lblStoreName.Text.Trim().ToString();
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptItemDiamondBalanceDataTable();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    var row = dataTable.NewRow();
                    row["#"] = i + 1;
                  
                    row["ID"] = gridView1.GetRowCellValue(i, "ID").ToString();
                    row["RecordType"] = gridView1.GetRowCellValue(i, "RecordType").ToString();
                    row["BarCode"] = gridView1.GetRowCellValue(i, "BarCode").ToString();
                    row["CaratPrice"] = gridView1.GetRowCellValue(i, "CaratPrice").ToString();
                               
                    row["InDIAMOND_W"] = gridView1.GetRowCellValue(i, "InQtyDaimond_W").ToString();
                    row["OutDIAMOND_W"] = gridView1.GetRowCellValue(i, "OutQtyDaimond_W").ToString();
                    row["BalanceDIAMOND_W"] = gridView1.GetRowCellValue(i, "BalanceDaimond_W").ToString();
 
                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptItemDiamondBalanc";
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("xrSubreport1", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
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
                    if (dt.Rows.Count > 0) for (int i = 1; i < 6; i++)
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

    }
}
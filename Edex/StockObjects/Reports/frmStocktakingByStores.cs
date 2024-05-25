using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Edex.DAL;
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.AccountsObjects.Reports;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using Edex.Reports;
using Edex.StockObjects.Codes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Edex.SalesAndSaleObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Transactions;
using Edex.StockObjects.Transactions;
using DevExpress.XtraPrinting;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Edex.StockObjects.Reports
{
    
    public partial class frmStocktakingByStores :BaseForm
    {
        private string strSQL = "";
        private string filter = "";
        DataTable dt = new DataTable();
        DataTable _nativeData = new DataTable();
        public DataTable _sampleData = new DataTable();
        string FocusedControl;
        private string PrimaryName;

        private string ItemName;
        private string SizeName;
        private string GroupName;

        public frmStocktakingByStores()
        {
            try
            {
                InitializeComponent();
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = true;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Caption = (UserInfo.Language == iLanguage.Arabic ? "استعلام جديد" : "New Query");
                ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
                ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;

             
                InitializeFormatDate(txtFromDate);
                InitializeFormatDate(txtToDate);
                _sampleData.Columns.Add(new DataColumn("Sn", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("GroupName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("ItemName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("BarCode", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QTYSYS", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("SizeName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("AverageCost", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QTYHand", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("StoreName", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("QTYDiff", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("PriceDiff", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("BranchID", typeof(string)));
                _sampleData.Columns.Add(new DataColumn("Notes", typeof(string)));
               
                strSQL = "EngName";

                if (UserInfo.Language == iLanguage.Arabic)
                {
                    strSQL = "ArbName";
                    ItemName = "ArbItemName";
                    SizeName = "ArbSizeName";
                    GroupName = "ArbGroupName";
                    PrimaryName = "ArbName";
                }
                else
                {
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    GroupName = "EngGroupName";
                    PrimaryName = "EngName";
                }


                FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                cmbBranchesID.EditValue =MySession.GlobalBranchID;
                cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                FillCombo.FillComboBox(cmbTypeID, "Stc_ItemTypes", "TypeID", strSQL, "", "BranchID="+MySession.GlobalBranchID);
                this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
                this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
                this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";

                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                this.txtGroupID.Validating += new System.ComponentModel.CancelEventHandler(this.txtGroupID_Validating);

                this.gridView2.RowStyle += gridView2_RowStyle;
                this.gridView2.DoubleClick+=gridView2_DoubleClick;
                //PrimaryName = "ArbName";

                if (UserInfo.Language == iLanguage.English)
                {
                     

                    btnShow.Text = btnShow.Tag.ToString();
                    // Label8.Text = btnShow.Tag.ToString();
                }


            }
            catch { }

        }
        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                {

                    var cellValue = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "BarCode");
                    if (cellValue != null)
                    {

                        frmItemBalanceByStores frm3 = new frmItemBalanceByStores();
                        if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm3);
                            frm3.Show();
                            frm3.ClearFilds();
                            frm3.txtBarCode.Text = cellValue.ToString();

                            if (!string.IsNullOrEmpty(txtStoreID.Text))
                                frm3.txtStoreID.Text = txtStoreID.Text;
                            frm3.btnShow_Click(null, null);
                        }
                        else
                            frm3.Dispose();



                    }
                }
            }
            catch { }

        }

        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            Obj.EditValue = DateTime.Now;
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
                if (FocusedControl.Trim() == txtStoreID.Name)
                {
                    if (!MySession.GlobalAllowChangefrmPurchaseStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", MySession.GlobalBranchID);
                }
                else if (FocusedControl.Trim() == txtCostCenterID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة");
                    else
                        PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID");
                }
                else if (FocusedControl.Trim() == txtBarCode.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "اسـم الـمـادة", "البـاركـود");
                    else
                        PrepareSearchQuery.Search(txtBarCode, lblBarCodeName, "BarCodeForPurchaseInvoice", "Item Name", "BarCode");
                }


                else if (FocusedControl.Trim() == txtGroupID.Name)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Search(txtGroupID, lblGroupID, "GroupID", "اسـم المجـمـوعة", "رقـم المجـمـوعة");
                    else
                        PrepareSearchQuery.Search(txtGroupID, lblGroupID, "GroupID", "Group Name", "Group ID");
                }


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
        private void txtGroupID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "   as GroupName FROM dbo.Stc_ItemsGroups WHERE GroupID =" + Comon.cInt(txtGroupID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtGroupID, lblGroupID, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }

        public void frmItemsList_Load(object sender, EventArgs e)
        {
            DataTable dtQty = new DataTable();
            dtQty.Columns.Add("ID", System.Type.GetType("System.Int16"));
            dtQty.Columns.Add("Name", System.Type.GetType("System.String"));
            dtQty.Rows.Add("0", (UserInfo.Language == iLanguage.Arabic ? "جميع الأرصدة" : "All Qty Balance"));
            dtQty.Rows.Add("1", (UserInfo.Language == iLanguage.Arabic ? "المواد التي رصيدها اكبر من الصفر" : "Qty Balance > 0"));
            dtQty.Rows.Add("2", (UserInfo.Language == iLanguage.Arabic ? "المواد التي رصيدها يساوي الصفر" : "Qty Balance = 0"));
            dtQty.Rows.Add("3", (UserInfo.Language == iLanguage.Arabic ? "المواد التي رصيدها أقل الصفر" : "Qty Balance < 0"));

            cmbQtyBalance.Properties.DataSource = dtQty.DefaultView;
            cmbQtyBalance.Properties.DisplayMember = "Name";
            cmbQtyBalance.Properties.ValueMember = "ID";



            DataTable dtPrice = new DataTable();
            dtPrice.Columns.Add("ID", System.Type.GetType("System.String"));
            dtPrice.Columns.Add("Name", System.Type.GetType("System.String"));

            dtPrice.Rows.Add("CostPrice", (UserInfo.Language == iLanguage.Arabic ? "سعر التكلفة" : "All Qty Balance"));
            dtPrice.Rows.Add("SalePrice", (UserInfo.Language == iLanguage.Arabic ? "سعر البيع" : "Qty Balance > 0"));
            dtPrice.Rows.Add("LastCostPrice", (UserInfo.Language == iLanguage.Arabic ? "أخر سعر شراء" : "Qty Balance = 0"));
            dtPrice.Rows.Add("LastSalePrice", (UserInfo.Language == iLanguage.Arabic ? "اخر سعر بيع " : "Qty Balance < 0"));
            dtPrice.Rows.Add("AverageCostPrice", (UserInfo.Language == iLanguage.Arabic ? "متوسط سعر التلكفة" : "Qty Balance < 0"));

            cmbPriceBy.Properties.DataSource = dtPrice.DefaultView;
            cmbPriceBy.Properties.DisplayMember = "Name";
            cmbPriceBy.Properties.ValueMember = "ID";
            cmbPriceBy.ItemIndex = 0;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = true;

        }

        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT   " + (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName") + "  as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {

            if (UserInfo.ID > 2)
            {
                DoAddFrom();
                return;
            }

            try
            {
                btnShow.Visible = false;
            
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                Application.DoEvents();
                _sampleData.Clear();
                GetStocktaking();
                 

                gridControl1.DataSource = _sampleData;

                if (gridView2.DataRowCount > 0)
                {

                    btnShow.Visible = true;
                    txtGroupID.Enabled = false;
                    txtStoreID.Enabled = false;
                    txtToItemNo.Enabled = false;
                    txtFromItemNo.Enabled = false;
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = false;
                    cmbTypeID.Enabled = false;
                    cmbQtyBalance.Enabled = false;
                    txtCostCenterID.Enabled = false;
                }
                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgInfo(Messages.TitleInfo, MySession.GlobalLanguageName == iLanguage.Arabic ? "لايوجد بيانات لعرضها" : "There is no Data to show it");
                    btnShow.Visible = true;
                    DoNew();
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
   
        public void ClearFilds()
        {
            _sampleData.Clear();
            gridControl1.RefreshDataSource();
            txtBarCode.Text = "";
            txtStoreID.Text = "";
            txtStoreID_Validating(null, null);
            txtToItemNo.Text = "";
            txtFromItemNo.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtCostCenterID.Text = "";
            lblCostCenterName.Text = "";
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtCostCenterID.Enabled = true;
            txtFromItemNo.Enabled = true;
            txtToItemNo.Enabled = true;
            txtGroupID.Enabled = true;
            txtStoreID.Enabled = true;
            cmbTypeID.Enabled = true;
            gridControl1.DataSource = _sampleData;
        }
        protected override void DoAddFrom()
        {
            try
            {
                ClearFilds();
            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        protected override void DoExport()
        {
            try
            {
                if (this.Tag == "Xlsx")
                {

                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    var workbook = excelApp.Workbooks.Add();
                    var worksheet = workbook.ActiveSheet;
                    worksheet.Name = "Sheet";

                    // Set the column headers
                    worksheet.Range["A1"].Value = "GroupName";
                    worksheet.Range["B1"].Value = "BarCode";
                    worksheet.Range["C1"].Value = "ItemName";
                    worksheet.Range["D1"].Value = "StoreName";
                    worksheet.Range["E1"].Value = "BranchID";
                    worksheet.Range["F1"].Value = "QTYSYS";
                    worksheet.Range["G1"].Value = "SizeName";
                    worksheet.Range["H1"].Value = "AverageCost";
                    worksheet.Range["I1"].Value = "TotalPrice";
                    worksheet.Range["J1"].Value = "QTYHand";
                    worksheet.Range["K1"].Value = "QTYDiff";
                    worksheet.Range["L1"].Value = "PriceDiff";
                    worksheet.Range["M1"].Value = "Notes";
                   
                    // Populate the data
                    int row = 2;
                    

                   for (int i = 0; i < gridView2.DataRowCount; i++)                 
                    {
                        worksheet.Cells[row, 1] = gridView2.GetRowCellValue(i, "GroupName");
                        worksheet.Cells[row, 2] = gridView2.GetRowCellValue(i, "BarCode");
                        worksheet.Cells[row, 3] = gridView2.GetRowCellValue(i, "ItemName");
                        worksheet.Cells[row, 4] = gridView2.GetRowCellValue(i, "StoreName");
                        worksheet.Cells[row, 5] = gridView2.GetRowCellValue(i, "BranchID");
                        worksheet.Cells[row, 6] = gridView2.GetRowCellValue(i, "QTYSYS");
                        worksheet.Cells[row, 7] = gridView2.GetRowCellValue(i, "SizeName");
                        worksheet.Cells[row, 8] = gridView2.GetRowCellValue(i, "AverageCost");
                        worksheet.Cells[row, 9] = gridView2.GetRowCellValue(i, "TotalPrice");
                        worksheet.Cells[row, 10] = gridView2.GetRowCellValue(i, "QTYHand");
                        worksheet.Cells[row, 11] = gridView2.GetRowCellValue(i, "QTYDiff");
                        worksheet.Cells[row, 12] = gridView2.GetRowCellValue(i, "PriceDiff");
                        worksheet.Cells[row, 13] = gridView2.GetRowCellValue(i, "Notes");
                        row++;
                    }
                 
                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                        saveFileDialog.FileName = "StockBySYSOmex.xlsx";
                        saveFileDialog.CheckFileExists = false; // تعيين هذا الخيار للسماح بإغلاق النافذة دون تحديد ملف
                        if (saveFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                            workbook.SaveAs(saveFileDialog.FileName);

                            workbook.Close();
                            excelApp.Quit();
                            SplashScreenManager.CloseForm();
                            Messages.MsgInfo(Messages.TitleInfo, (UserInfo.Language == iLanguage.Arabic) ? "تم التصدير بنجاح" : "Export completed successfully");
                        }
                        else
                        {
                            workbook.Close(false); // إغلاق الملف وعدم حفظه
                            excelApp.Quit();
                        }
                    }
                }
 
            }
            catch (Exception ex)
            {
                //SplashScreenManager.CloseForm();
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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
                rptFormName = "rptStocktakingByStoresArb";
                if (UserInfo.Language == iLanguage.English)
                    rptFormName = ReportName + "Arb";
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                rptForm.Parameters["StoreName"].Value = lblStoreName.Text.Trim().ToString();
                rptForm.Parameters["ByPrice"].Value = cmbPriceBy.Text.Trim().ToString();


                rptForm.Parameters["FromItem"].Value = txtFromItemNo.Text.Trim().ToString();
                rptForm.Parameters["ToItem"].Value = txtToItemNo.Text.Trim().ToString();
                rptForm.Parameters["ItemType"].Value = cmbTypeID.Text.Trim().ToString();
                rptForm.Parameters["Group"].Value = lblGroupID.Text.Trim().ToString();

                rptForm.Parameters["CostCenter"].Value = lblCostCenterName.Text.Trim().ToString();
                rptForm.Parameters["ToDate"].Value = txtToDate.Text.Trim().ToString();
                rptForm.Parameters["BalanceType"].Value = lblStoreName.Text.Trim().ToString();

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                var dataTable = new dsReports.rptStocktakingByStoresDataTable();

                for (int i = 0; i <= gridView2.DataRowCount - 2; i++)
                {
                    var row = dataTable.NewRow();

                    row["Sn"] = i + 1;
                    row["Barcode"] = gridView2.GetRowCellValue(i, "BarCode").ToString();
                    row["ItemName"] = gridView2.GetRowCellValue(i, "ItemName").ToString();

                    row["GroupName"] = gridView2.GetRowCellValue(i, "GroupName").ToString();
                    row["TotalPrice"] = gridView2.GetRowCellValue(i, "TotalPrice").ToString();
                    row["Notes"] = gridView2.GetRowCellValue(i, "Notes").ToString();
                    row["StoreName"] = gridView2.GetRowCellValue(i, "StoreName").ToString();
                    row["PriceDiff"] = gridView2.GetRowCellValue(i, "PriceDiff").ToString();
                    row["QTYDiff"] = gridView2.GetRowCellValue(i, "PriceDiff").ToString();
                    row["QTYHand"] = gridView2.GetRowCellValue(i, "QTYHand").ToString();
                    row["AverageCost"] = gridView2.GetRowCellValue(i, "AverageCost").ToString();
                    row["SizeName"] = gridView2.GetRowCellValue(i, "SizeName").ToString();
                    row["QTYSYS"] = gridView2.GetRowCellValue(i, "QTYSYS").ToString();
                    row["BranchID"] = gridView2.GetRowCellValue(i, "BranchID").ToString();
                    

                    dataTable.Rows.Add(row);
                }
                rptForm.DataSource = dataTable;
                rptForm.DataMember = "rptStocktakingByStores";

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

        private void frmStocktaking_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
                Find();
            if (e.KeyCode == Keys.F5)
                DoPrint();
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                switch (view.GetFocusedRowCellValue("DocumentTypeID").ToString())
                {
                    case "10":
                        frmCashierPurchaseGold frm = new frmCashierPurchaseGold();
                        if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm);
                            frm.Show();
                            frm.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm.Dispose();
                        break;
                    case "4":
                        frmCashierPurchaseDaimond frm1 = new frmCashierPurchaseDaimond();
                        if (Permissions.UserPermissionsFrom(frm1, frm1.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm1);
                            frm1.Show();
                            frm1.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm1.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm1.Dispose();
                        break;
                    case "ItemsOutOnBail":
                        frmItemsOutOnBail frm11 = new frmItemsOutOnBail();
                        if (Permissions.UserPermissionsFrom(frm11, frm11.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm11);
                            frm11.Show();
                            frm11.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm11.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm11.Dispose();
                        break;
                    case "ItemsInOnBail":
                        frmItemsInonBail frm12 = new frmItemsInonBail();
                        if (Permissions.UserPermissionsFrom(frm12, frm12.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm12);
                            frm12.Show();
                            frm12.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm12.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm12.Dispose();
                        break;
                    case "GoodsOpening":
                        frmGoodsOpeningOld frm112 = new frmGoodsOpeningOld();
                        if (Permissions.UserPermissionsFrom(frm112, frm112.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm112);
                            frm112.Show();
                            frm112.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm112.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm112.Dispose();
                        break;

                    case "ItemsDismantling":
                        frmItemsDismantling frm10 = new frmItemsDismantling();
                        if (Permissions.UserPermissionsFrom(frm10, frm10.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm10);
                            frm10.Show();
                            frm10.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm10.Dispose();
                        break;

                    case "7":
                        frmSalesInvoiceReturn frm2 = new frmSalesInvoiceReturn();
                        if (Permissions.UserPermissionsFrom(frm2, frm2.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm2);
                            frm2.Show();
                            frm2.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm2.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm2.Dispose();
                        break;

                    case "9":
                        frmCashierSalesGold frm3 = new frmCashierSalesGold();
                        if (Permissions.UserPermissionsFrom(frm3, frm3.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm3);
                            frm3.Show();
                            frm3.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm3.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm3.Dispose();
                        break;
                    case "6":
                        frmCashierSalesAlmas frm30 = new frmCashierSalesAlmas();
                        if (Permissions.UserPermissionsFrom(frm30, frm30.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm30);
                            frm30.Show();
                            frm30.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm30.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm30.Dispose();
                        break;
                    case "5":
                        frmCashierPurchaseReturnDaimond frm4 = new frmCashierPurchaseReturnDaimond();
                        if (Permissions.UserPermissionsFrom(frm4, frm4.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm4);
                            frm4.Show();
                            frm4.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm4.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm4.Dispose();
                        break;
                    case "11":
                        frmCashierPurchaseReturnGold frm15 = new frmCashierPurchaseReturnGold();
                        if (Permissions.UserPermissionsFrom(frm15, frm15.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm15);
                            frm15.Show();
                            frm15.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm15.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm15.Dispose();
                        break;
                    case "13":
                        frmCashierPurchaseSaveDaimond frm201 = new frmCashierPurchaseSaveDaimond();
                        if (Permissions.UserPermissionsFrom(frm201, frm201.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm201);
                            frm201.Show();
                            frm201.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm201.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm201.Dispose();
                        break;
                    case "14":
                        frmGoldInOnBail frm2101 = new frmGoldInOnBail();
                        if (Permissions.UserPermissionsFrom(frm2101, frm2101.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm2101);
                            frm2101.Show();
                            frm2101.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frm2101.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frm2101.Dispose();
                        break;
                    case "15":
                        frmGoodsOpening frmGoodsOp = new frmGoodsOpening();
                        if (Permissions.UserPermissionsFrom(frmGoodsOp, frmGoodsOp.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGoodsOp);

                            frmGoodsOp.Show();
                            frmGoodsOp.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmGoodsOp.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmGoodsOp.Dispose();
                        break;
                    case "16":
                        frmGoldOutOnBail frmGolOut = new frmGoldOutOnBail();
                        if (Permissions.UserPermissionsFrom(frmGolOut, frmGolOut.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGolOut);

                            frmGolOut.Show();
                            frmGolOut.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmGolOut.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmGolOut.Dispose();
                        break;
                    case "17":
                        frmMatirialInOnBail frmMatiralIn = new frmMatirialInOnBail();
                        if (Permissions.UserPermissionsFrom(frmMatiralIn, frmMatiralIn.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatiralIn);

                            frmMatiralIn.Show();
                            frmMatiralIn.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmMatiralIn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmMatiralIn.Dispose();
                        break;
                    case "18":
                        frmMatirialOutOnBail frmMatiralOut = new frmMatirialOutOnBail();
                        if (Permissions.UserPermissionsFrom(frmMatiralOut, frmMatiralOut.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatiralOut);

                            frmMatiralOut.Show();
                            frmMatiralOut.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmMatiralOut.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmMatiralOut.Dispose();
                        break;
                    case "19":
                        frmTransferMultipleStoresGold frmGoldMulti = new frmTransferMultipleStoresGold();
                        if (Permissions.UserPermissionsFrom(frmGoldMulti, frmGoldMulti.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGoldMulti);

                            frmGoldMulti.Show();
                            frmGoldMulti.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmGoldMulti.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmGoldMulti.Dispose();
                        break;
                    case "20":
                        frmTransferMultipleStoreMatirial frmMatiralMulti = new frmTransferMultipleStoreMatirial();
                        if (Permissions.UserPermissionsFrom(frmMatiralMulti, frmMatiralMulti.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatiralMulti);

                            frmMatiralMulti.Show();
                            frmMatiralMulti.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmMatiralMulti.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmMatiralMulti.Dispose();
                        break;
                    case "23":
                        frmCashierPurchaseServicesEqv frmGoldMatirialInvoice = new frmCashierPurchaseServicesEqv();
                        if (Permissions.UserPermissionsFrom(frmGoldMatirialInvoice, frmGoldMatirialInvoice.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmGoldMatirialInvoice);

                            frmGoldMatirialInvoice.Show();
                            frmGoldMatirialInvoice.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmGoldMatirialInvoice.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmGoldMatirialInvoice.Dispose();
                        break;
                    case "24":
                        frmCashierPurchaseReturnMatirial frmMatiralInvReturn = new frmCashierPurchaseReturnMatirial();
                        if (Permissions.UserPermissionsFrom(frmMatiralInvReturn, frmMatiralInvReturn.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmMatiralInvReturn);

                            frmMatiralInvReturn.Show();
                            frmMatiralInvReturn.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmMatiralInvReturn.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmMatiralInvReturn.Dispose();
                        break;
                    case "39":
                        frmCashierSales frmSales = new frmCashierSales();
                        if (Permissions.UserPermissionsFrom(frmSales, frmSales.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frmSales);

                            frmSales.Show();
                            //frmSales.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                            frmSales.MoveRec(Comon.cLong(view.GetFocusedRowCellValue("TranseID").ToString()) + 1, 8);
                        }
                        else
                            frmSales.Dispose();

                        break;

                }
            }
            catch { }
        }


        private void OpenWindow(BaseForm frm)
        {

            try
            {
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            catch { }

        }


        /// <summary>



  

        protected override void DoNew()
        {
            try
            {

                _sampleData.Clear();
                gridControl1.RefreshDataSource();
                txtGroupID.Text = "";
                txtGroupID_Validating(null, null);
                txtStoreID.Text = "";
                txtCostCenterID.Text = "";
                txtCostCenterID_Validating(null, null);
                txtStoreID_Validating(null, null);
                txtGroupID.Enabled = true;
                txtStoreID.Enabled = true;
                txtToItemNo.Enabled = true;
                txtFromItemNo.Enabled = true;
                txtToDate.Enabled = true;
                cmbTypeID.Enabled = true;
                cmbQtyBalance.Enabled = true;
                txtCostCenterID.Enabled = true;
                cmbTypeID.ItemIndex = -1;
                cmbPriceBy.ItemIndex = 0;
                cmbPriceBy.Enabled = true;


            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }


        }

   

        ////////////////////////
        public string StockTransaction()
        {
            try
            {
                //long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                filter = "";
                filter = "( dbo.Stc_ItemsMoviing.BranchID = " +Comon.cInt( cmbBranchesID.EditValue) + ") AND dbo.Stc_ItemsMoviing.Cancel =0   AND  dbo.Stc_ItemsMoviing.ItemID >0  AND";
                strSQL = "";
                if (txtCostCenterID.Text != string.Empty)
                    filter += "  dbo.Stc_ItemsMoviing.CostCenterID=" + Comon.cInt(txtCostCenterID.Text) + " AND ";
                if (txtFromItemNo.Text != string.Empty)
                    filter += "  dbo.Stc_ItemsMoviing.ItemID >=" + Comon.cInt(txtFromItemNo.Text) + " AND ";
                if (txtToItemNo.Text != string.Empty)
                    filter = filter + "  dbo.Stc_ItemsMoviing.ItemID <=" + Comon.cInt(txtToItemNo.Text) + " AND ";
                if (txtGroupID.Text != string.Empty)
                    filter = filter + "  dbo.Stc_ItemsMoviing.GroupID =" + txtGroupID.Text + " AND ";
                if (cmbTypeID.Text != string.Empty)
                    filter = filter + "  dbo.Stc_ItemsMoviing.TypeID =" + cmbTypeID.EditValue + " AND ";
                if (txtBarCode.Text != string.Empty)
                    filter = filter + " dbo.Stc_ItemsMoviing.BarCode  ='" + txtBarCode.Text + "'  AND ";
                if (txtStoreID.Text != string.Empty)
                    filter = filter + " dbo.Stc_ItemsMoviing.StoreID  =" + Comon.cDbl(txtStoreID.Text) + "  AND ";
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
                long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                if (FromDate != 0)
                    filter = filter + "   MoveDate>='" + FromDate + "' AND ";

                if (ToDate != 0)
                    filter = filter + "   MoveDate<='" + ToDate + "' AND ";

                filter += " Stc_ItemsMoviing.Posted=3  And ";

                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = " SELECT  dbo.Stc_ItemsMoviing.BarCode, dbo.Stc_ItemsMoviing.ItemID,dbo.Stc_ItemsMoviing.StoreID,dbo.Stc_ItemsMoviing.BranchID,"
                    + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyOpening, "
                    + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 AND dbo.Stc_ItemsMoviing.DocumentTypeID <> 15 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyIncomming, "
                    + " SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 2 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END) AS QtyOut, "
                    + "   SUM(CASE WHEN dbo.Stc_ItemsMoviing.MoveType = 1 THEN dbo.Stc_ItemsMoviing.QTY ELSE -dbo.Stc_ItemsMoviing.QTY END) AS QtyBalance, "
                    + "  SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 OR dbo.Stc_ItemsMoviing.DocumentTypeID = 23 THEN (dbo.Stc_ItemsMoviing.InPrice + dbo.Stc_ItemsMoviing.Bones) ELSE 0 END) / "
                    + "  NULLIF(SUM(CASE WHEN dbo.Stc_ItemsMoviing.DocumentTypeID = 15 OR dbo.Stc_ItemsMoviing.DocumentTypeID = 23 THEN dbo.Stc_ItemsMoviing.QTY ELSE 0 END), 0) AS AverageCost, "
                    + " dbo.Stc_SizingUnits.ArbName AS SizeName, dbo.Stc_SizingUnits.SizeID  FROM dbo.Stc_ItemsMoviing "
                    + " LEFT OUTER JOIN dbo.Stc_SizingUnits ON dbo.Stc_ItemsMoviing.SizeID = dbo.Stc_SizingUnits.SizeID and dbo.Stc_ItemsMoviing.BranchID = dbo.Stc_SizingUnits.BranchID "
                    + " WHERE  " + filter
                    + " GROUP BY dbo.Stc_ItemsMoviing.BarCode,dbo.Stc_ItemsMoviing.StoreID,dbo.Stc_ItemsMoviing.BranchID, dbo.Stc_SizingUnits.ArbName, dbo.Stc_ItemsMoviing.SizeID,dbo.Stc_ItemsMoviing.ItemID,dbo.Stc_SizingUnits.SizeID  ";



                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, iLanguage.English.ToString());
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            return strSQL;
        }
        private void GetStocktaking()
        {
            try
            {
                DataRow row;

                dt = Lip.SelectRecord(StockTransaction());
                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            row = _sampleData.NewRow();

                            row["Sn"] = _sampleData.Rows.Count + 1;
                            row["BarCode"] = dt.Rows[i]["BarCode"].ToString();
                            row["GroupName"] = Lip.GetValue("SELECT [ArbName] FROM  [Stc_ItemsGroups] where [GroupID] in( select GroupID from Stc_Items where ItemID=" + Comon.cInt(dt.Rows[i]["ItemID"].ToString()) + ") and BranchID="+Comon.cInt(cmbBranchesID.EditValue));
                            row["ItemName"] = Lip.GetValue("SELECT [ArbName] FROM  [Stc_Items] where [ItemID]=" + Comon.cInt(dt.Rows[i]["ItemID"].ToString()) + " and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
                            row["SizeName"] = dt.Rows[i]["SizeName"].ToString();
                            row["StoreName"] = Lip.GetValue("SELECT [ArbName] FROM  [Stc_Stores] where [AccountID]=" + Comon.cLong(dt.Rows[i]["StoreID"].ToString()) + " and BranchID=" + Comon.cInt(cmbBranchesID.EditValue));
                            row["BranchID"] = dt.Rows[i]["BranchID"].ToString();
                            row["QTYSYS"] = Comon.ConvertToDecimalQty(dt.Rows[i]["QtyBalance"].ToString());
                            DataTable dtt = frmItems.GetItemMoving(Comon.cLong(dt.Rows[i]["ItemID"]), Comon.cInt(dt.Rows[i]["SizeID"]), Comon.cDbl(dt.Rows[i]["StoreID"]), Comon.cInt(dt.Rows[i]["BranchID"]),0, Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text)), Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text)));
                            //row["AverageCost"] = Comon.ConvertToDecimalPrice(dt.Rows[i]["AverageCost"]);
                            row["AverageCost"] = dtt.Rows[0]["CurentAverageCostPrice"];
                            row["TotalPrice"] = Comon.cDec(Comon.cDec(row["QTYSYS"]) * Comon.cDec(row["AverageCost"])).ToString();
                             _sampleData.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }


        private void gridView2_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {

                if (View.GetRowCellDisplayText(e.RowHandle, View.Columns["QTYSYS"]).ToString() == "")
                {
                    if (Comon.cInt(View.GetRowCellDisplayText(e.RowHandle, View.Columns["QTYSYS"]).ToString()) > 0)
                    {
                        e.Appearance.BackColor = Color.LightYellow;
                        e.Appearance.BackColor2 = Color.LightYellow;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.LightBlue;
                        e.Appearance.BackColor2 = Color.LightBlue;
                    }
                    e.HighPriority = true;
                }
            }
        }

        private void labelControl46_Click(object sender, EventArgs e)
        {

        }

        private void btnemport_Click(object sender, EventArgs e)
        {
            try
            {
            label1:
                if (txtExcelPath.Text == string.Empty)
                {
                    Messages.MsgError(Messages.TitleConfirm, " يجب تحديد مسار ملف الأكسل ");
                    txtExcelPath.Focus();
                    btnSelectFile_Click(null, null);
                    goto label1;
                }
                EmportAccounts();
                txtExcelPath.Text = "";

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm();
                Messages.MsgError(Messages.TitleError, "خطأ في الإستيراد - الرجاء مراجعة جميع حقول الملف والتأكد أنها حسب القالب المحدد" + ex.Message);
            }

        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "All Files|*.*";
                    openFileDialog.FileName = "";
                    openFileDialog.CheckFileExists = false; // تعيين هذا الخيار للسماح بإغلاق النافذة دون تحديد ملف
                    if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != "")
                    {
                        txtExcelPath.Text = openFileDialog.FileName;
                        btnemport.Enabled = true;
                    }
                    else
                    {
                        btnemport.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {

                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void CalculateDifferenc()
        {
            decimal QTYDiff = 0;
            decimal totalDiff = 0;
            decimal PriceDiff = 0;
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                QTYDiff = Comon.cDec(Comon.cDec(gridView2.GetRowCellValue(i, "QTYHand")) - Comon.cDec(gridView2.GetRowCellValue(i, "QTYSYS")));
                totalDiff = Comon.cDec(Comon.cDec(QTYDiff) * Comon.cDec(gridView2.GetRowCellValue(i, "AverageCost")));
                PriceDiff = Comon.cDec(Comon.cDec(totalDiff) - Comon.cDec(gridView2.GetRowCellValue(i, "TotalPrice")));
                gridView2.SetRowCellValue(i, "QTYDiff", QTYDiff);
                gridView2.SetRowCellValue(i, "PriceDiff", PriceDiff);
            }
        }
        private void EmportAccounts()
        {
            OleDbConnection oledbConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtExcelPath.Text + ";Extended Properties=Excel 12.0");
            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "تأكيد الاسنيراد  ؟");
            if (!Yes)
                return;
            Application.DoEvents();
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
            oledbConn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet$]", oledbConn);
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            oleda.SelectCommand = cmd;
            DataTable dt = new DataTable();
            oleda.Fill(dt);
            oledbConn.Close();
            if (dt.Rows.Count < 1)
                return;
            gridControl1.DataSource=  dt;
            CalculateDifferenc();
                SplashScreenManager.CloseForm();
                Messages.MsgInfo(Messages.TitleConfirm, "تم الاستيراد بنجاح ");
            
        }

    }
}
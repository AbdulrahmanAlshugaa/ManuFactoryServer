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
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Base;
using Edex.Model;
using DevExpress.XtraGrid.Views.Grid;
using ITIN.ModelSystem;
using ITIN.GeneralObjects.GeneralClasses;
using DevExpress.XtraSplashScreen;
using ITIN.GeneralObjects.GeneralForms;
using Edex.Model.Language;
using DevExpress.XtraReports.UI;

namespace ITIN.RestaurantSystem.Transactions
{
    public partial class frmKDSV2 : DevExpress.XtraEditors.XtraForm
    {
        public DataTable _sampleData = new DataTable();
        public DataTable _sampleDataDetials = new DataTable();
        BindingList<MSgSettingsDetials> lstDetail = new BindingList<MSgSettingsDetials>();
        List<MSgSettingsDetials> lstDetailSave = new List<MSgSettingsDetials>();
        public DataTable _sampleDataAbsent = new DataTable();
        public DataTable _sampleDataAttend = new DataTable();
        public DataTable _sampleDataLate = new DataTable();
        public DataTable dtMsgArchive = new DataTable();
        DataTable dtSenderInfo = new DataTable();
        List<MyData> data = new List<MyData>();
        public string languagename = "";
        public frmKDSV2()
        {
            InitializeComponent();
            gridControl1.MainView = gridView2;
            if (UserInfo.Language == iLanguage.English)
                languagename = "EngName";
            else
                languagename = "ArbName";
            FillCombo.FillComboBox(cmbOrderType, "Res_OrderType", "ID", "ArbName", "", "1=1");
            var sr = "     SELECT     OrderOnTabletMaster.DailyID, OrderOnTabletMaster.OrderID,OrderOnTabletMaster.typeID,"
       + "                COUNT(OrderOnTabletDetials.OrderID) AS CountID, SUM(OrderOnTabletDetials.Width) AS Width,OrderOnTabletMaster.OrderType,OrderOnTabletMaster.OrderTable , OrderOnTabletMaster.OrderDate,isnull(OrderOnTabletMaster.SaleType,1) as SaleType "

  + "  FROM            OrderOnTabletMaster INNER JOIN"
               + "             OrderOnTabletDetials ON OrderOnTabletMaster.OrderID = OrderOnTabletDetials.OrderID and OrderOnTabletMaster.typeID = OrderOnTabletDetials.typeID"
  + "  WHERE        (0 = 0) AND (OrderOnTabletMaster.NeedReview <> 2)  and OrderOnTabletMaster.SaleType=0 "
  + " and  OrderOnTabletDetials.Description1<>'0Trans' "

  + " group by   OrderOnTabletMaster.SaleType,OrderOnTabletMaster.DailyID,OrderOnTabletMaster.OrderID,OrderOnTabletMaster.OrderDate,OrderOnTabletMaster.OrderType,OrderOnTabletMaster.OrderTable,OrderOnTabletMaster.typeID"
  + " ORDER BY OrderOnTabletMaster.OrderDate,OrderOnTabletMaster.DailyID ASC";
            _sampleData = Lip.SelectRecord(sr);
            gridControl2.DataSource = _sampleData;
          // " select DailyID,OrderType,InvoiceDate,InvoiceID,OrderTable from [dbo].[OrderOnTabletMaster]  where Cancel=0 and NeedReview<>1 order by InvoiceID ASC ";

        timer1.Enabled = true;

        }
       
        private void layoutView1_CustomFieldValueStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewFieldValueStyleEventArgs e)
        {
            //  return;  // Painting the content of the focused card only if the LayoutView itself has the focus.
            ColumnView view = sender as ColumnView;
            if (view == null || e.RowHandle < 0) return;
            // if(view.get)
            int count = Comon.cInt(view.GetRowCellValue(e.RowHandle, "CountID").ToString());
            int width = Comon.cInt(view.GetRowCellValue(e.RowHandle, "Width").ToString());
            if (count == width)
            {
                e.Appearance.BackColor = Color.Green;
                e.Appearance.BackColor2 = Color.Green;
                e.Appearance.ForeColor = Color.White;
                e.Appearance.Options.UseFont = true;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
            else if (count > width && width != 0)
            {


                e.Appearance.BackColor = Color.Orange;
                e.Appearance.BackColor2 = Color.Orange;
                e.Appearance.ForeColor = Color.White;
                e.Appearance.Options.UseFont = true;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

            }
            else if (width == 0)
            {


                e.Appearance.BackColor = Color.FromArgb(91, 103, 112);
                e.Appearance.BackColor2 = Color.FromArgb(91, 103, 112);
                e.Appearance.ForeColor = Color.White;
                e.Appearance.Options.UseFont = true;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

            }

        }

        private void layoutView1_CustomCardStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewCardStyleEventArgs e)
        {
            // Painting a border for focused card even if LayoutView itself hasn't got the focus
            ColumnView view = sender as ColumnView;
            if (view == null || e.RowHandle<0) return;
            //timer1.Enabled = true;
            int count = Comon.cInt(view.GetRowCellValue(e.RowHandle, "CountID").ToString());
            int width = Comon.cInt(view.GetRowCellValue(e.RowHandle, "Width").ToString());
            if (count == width)
            {
                e.Appearance.BackColor = Color.Green;
                e.Appearance.BackColor2 = Color.Green;
                
            }
            else if (count > width && width != 0)
            {


                e.Appearance.BackColor = Color.Orange;
                e.Appearance.BackColor2 = Color.Orange;
              

            }
            else if (width == 0)
            {


                e.Appearance.BackColor = Color.FromArgb(91, 103, 112);
                e.Appearance.BackColor2 = Color.FromArgb(91, 103, 112);
               

            }

            //if ((e.State & DevExpress.XtraGrid.Views.Base.GridRowCellState.Focused) > 0)
            //{
            //    e.Appearance.BackColor = Color.DarkGray;
            //    e.Appearance.BackColor2 = Color.DarkGray;
            //}
            //else
            //{
            //    e.Appearance.BackColor = Color.Green;
            //    e.Appearance.BackColor2 = Color.Green;
            //}

        }

        private void layoutView1_CustomDrawCardFieldValue(object sender, RowCellCustomDrawEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            if (view == null || e.RowHandle < 0) return;
            if (e.RowHandle == view.FocusedRowHandle && view.FocusedRowHandle > 0 && (view.GridControl.Focused))
            {
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.Options.UseFont = true;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                var sr = "     SELECT     OrderOnTabletMaster.DailyID, OrderOnTabletMaster.OrderID,OrderOnTabletMaster.typeID,"
       + "                COUNT(OrderOnTabletDetials.OrderID) AS CountID, SUM(OrderOnTabletDetials.Width) AS Width,OrderOnTabletMaster.OrderType,OrderOnTabletMaster.OrderTable , OrderOnTabletMaster.OrderDate,isnull(OrderOnTabletMaster.SaleType,1) as SaleType "
  + "  FROM            OrderOnTabletMaster INNER JOIN"
               + "             OrderOnTabletDetials ON OrderOnTabletMaster.OrderID = OrderOnTabletDetials.OrderID and OrderOnTabletMaster.typeID = OrderOnTabletDetials.typeID"
  + "  WHERE        (0 = 0) AND (OrderOnTabletMaster.NeedReview <> 2) and  OrderOnTabletMaster.SaleType=0"
  + " and  OrderOnTabletDetials.Description1<>'0Trans' "

  + " group by  OrderOnTabletMaster.SaleType, OrderOnTabletMaster.DailyID,OrderOnTabletMaster.OrderID,OrderOnTabletMaster.OrderDate,OrderOnTabletMaster.OrderType,OrderOnTabletMaster.OrderTable,OrderOnTabletMaster.typeID"
  + " ORDER BY OrderOnTabletMaster.OrderDate,OrderOnTabletMaster.DailyID ASC";

                var sdr = Lip.SelectRecord(sr);

                sdr.PrimaryKey = new DataColumn[] { sdr.Columns["OrderID"], sdr.Columns["typeID"], sdr.Columns["OrderDate"] };
                _sampleData.PrimaryKey = new DataColumn[] { _sampleData.Columns["OrderID"], _sampleData.Columns["typeID"], _sampleData.Columns["OrderDate"] };
                if (sdr.Rows.Count > _sampleData.Rows.Count)
                
                {
                    _sampleData.Merge(sdr);
                    _sampleData.AcceptChanges();
                //    DataRow dr;


                //    for (int i = 0; i <= sdr.Rows.Count - 1; ++i)
                //    {

                //            dr = sdr.Rows[i];

                //            if (isINGridVIew(Comon.cInt(dr["OrderID"].ToString().Trim()), Comon.cInt(dr["typeID"].ToString().Trim()), Comon.cInt(dr["OrderDate"].ToString().Trim())))
                //                dr.Delete();
                        

                //    }
                     
                //    sdr.AcceptChanges();
                //    for (int i = 0; i <= sdr.Rows.Count - 1; ++i)
                //    {
                        
                //        dr = _sampleData.NewRow();
                //        dr[0] = sdr.Rows[i][0];
                //        dr[1] = sdr.Rows[i][1];
                //        dr[2] = sdr.Rows[i][2];
                //        dr[3] = sdr.Rows[i][3];
                //        dr[4] = sdr.Rows[i][4];
                //        dr[5] = sdr.Rows[i][5];
                //        dr[6] = sdr.Rows[i][6];
                //        dr[7] = sdr.Rows[i][7];
                //        dr[8] = sdr.Rows[i][8];
                //        _sampleData.Rows.Add(dr);


                //        //  lblPageNO.Text=layoutView1.Cou
                //    }


                    gridControl2.RefreshDataSource();

            }

                
                else if (sdr.Rows.Count < _sampleData.Rows.Count)
                {

                    _sampleData.Clear();
                    _sampleData = sdr;
                    gridControl2.DataSource = _sampleData;//();



                }













                timer1.Enabled = true;

            }
            catch { timer1.Enabled = true; }








        }

      
        void layoutView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                gridControl1.DataSource = null;
                labelControl1.Text = "";
                if (layoutView1.RowCount < 1) {
                    timer1.Enabled = true;
                    return;
                }
                
                var hiTinfo = layoutView1.CalcHitInfo(e.Location);
                if (hiTinfo.InFieldValue)
                {

                    long ID = Comon.cLong(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "OrderID").ToString());
                    long Dialy = Comon.cLong(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "DailyID").ToString());
                    long date = Comon.cLong(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "OrderDate").ToString());
                    int order = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "OrderType").ToString());
                    int Table = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "OrderTable").ToString());
                    int count = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "CountID").ToString());
                    int typeID = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "typeID").ToString());
                    int width = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "Width").ToString());
                    int SaleType = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "SaleType").ToString());
                    if (SaleType == 1) {

                        Messages.MsgError(Messages.TitleConfirm, "لم يتم تأكيد الطلب من قبل  الكاشير  ");
                        return;
                    }
                    if (count == width)
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm,"تأكيد تسليم الطلب ");
                        if (Yes)
                        {

                            var sr = " update OrderOnTabletMaster set NeedReview=2 where OrderID= " + ID + " and  typeID=" + typeID;
                            Lip.ExecututeSQL(sr);

                            DataRow[] rows = _sampleData.Select("OrderID=" + ID.ToString() + "  and   typeID=" + typeID.ToString());
                            DataRow row = rows[0];
                            //Then from the DataRow[] rows assign a random row (for example the first row) to a DataRow object

                            int pkIndex = _sampleData.Rows.IndexOf(row);

                         //   layoutView1.DeleteSelectedRows();
                            DataRow dr = _sampleData.Rows[pkIndex];
                            dr.Delete();
                            _sampleData.AcceptChanges();
                            gridControl2.RefreshDataSource();// = sdr1;

                            layoutView1.MovePrevPage();
                            if (radioButton1.Checked == true) {
                                DoPRint(Dialy, cmbOrderType.Text.ToString(), date, Table, ID);
                            
                            }
                            timer1.Enabled = true;

                            return;
                        }
                    }
                    var sr1 = "      SELECT     OrderOnTabletDetials.typeID , OrderOnTabletDetials.BarCode ,   OrderOnTabletDetials.OrderID, OrderOnTabletDetials.QTY,Concat(   Stc_SizingUnits." + languagename + ",' ', Stc_Items." + languagename + ",CHAR(13) ,OrderOnTabletDetials.Description,CHAR(13) ,OrderOnTabletDetials.notes )as ItemName, Stc_Items.GroupID, Stc_SizingUnits.ArbName AS SizeName, Stc_ItemsGroups.ArbName AS GroupName"
+ " ,OrderOnTabletDetials.Width as Status ,'" + hiTinfo.RowHandle + "' as rowLayout FROM            Stc_SizingUnits INNER JOIN"
+ "                    OrderOnTabletDetials ON Stc_SizingUnits.SizeID = OrderOnTabletDetials.SizeID LEFT OUTER JOIN"
  + "               Stc_ItemsGroups INNER JOIN "
   + "               Stc_Items ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID ON OrderOnTabletDetials.ItemID = Stc_Items.ItemID   where Stc_SizingUnits.Notes<>'0' AND    OrderOnTabletDetials.Description<>'0Trans'  and     OrderOnTabletDetials.typeID=" + typeID.ToString() + "  and  OrderOnTabletDetials.OrderID=" + ID.ToString() + "   order by OrderOnTabletDetials.OrderID ASC ";

                    labelControl1.Text = "رقم الطلب : " + Dialy.ToString() + ": رقم الفاتورة :" + ID.ToString() + ": تاريخ الفاتورة :" + Comon.ConvertSerialDateTo(date.ToString()) + ": رقم الطاولة :" + Table.ToString();
                    cmbOrderType.EditValue = order;
                    var sdr1 = Lip.SelectRecord(sr1);

                    gridControl1.DataSource = sdr1;

                }
                timer1.Enabled = true;
            }
            catch { timer1.Enabled = true; }
        }

        private void DoPRint(long Dialy, string p1, long date, int Table, long ID)
        {
            var sr1 = "      SELECT       OrderOnTabletDetials.BarCode ,   OrderOnTabletDetials.OrderID, OrderOnTabletDetials.QTY,CONCAT(Stc_Items.ArbName,' - ',Stc_SizingUnits.ArbName ) as ItemName, Stc_Items.GroupID, Stc_SizingUnits.ArbName AS SizeName, Stc_ItemsGroups.ArbName AS GroupName"
+ " ,OrderOnTabletDetials.Width as Status ,'" +11 + "' as rowLayout FROM            Stc_SizingUnits INNER JOIN"
+ "                    OrderOnTabletDetials ON Stc_SizingUnits.SizeID = OrderOnTabletDetials.SizeID LEFT OUTER JOIN"
+ "               Stc_ItemsGroups INNER JOIN "
 + "               Stc_Items ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID ON OrderOnTabletDetials.ItemID = Stc_Items.ItemID   where Stc_SizingUnits.Notes<>'0' AND         0=0  and  OrderOnTabletDetials.OrderID=" + ID.ToString() + "   order by OrderOnTabletDetials.OrderID ASC ";

            var dtsr = Lip.SelectRecord(sr1);
            if (dtsr.Rows.Count < 1) return;
            Application.DoEvents();
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

            /******************** Report Body *************************/
            //rptForm = "rptCashierPrint";
            bool IncludeHeader = true;
            var ReportName = "rptDeliveryInvoice";
            string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
            rptFormName = "rptSplitResturantInvoiceByItemsGroupsArb";
            XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

            /********************** Master *****************************/
            rptForm.RequestParameters = false;
            var dataTable = new DataTable();
            dataTable.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dataTable.Columns.Add("ItemName", System.Type.GetType("System.String"));
            dataTable.Columns.Add("UnitName", System.Type.GetType("System.String"));
            dataTable.Columns.Add("Qty", System.Type.GetType("System.Decimal"));
            dataTable.Columns.Add("Total", System.Type.GetType("System.Decimal"));

            /********************** Master *****************************/

            rptForm.Parameters["InvoiceID"].Value = ID;// Comon.ConvertDateToSerial(txtInvoiceDate.Text) + "-" + invoiceNo;
            rptForm.Parameters["OrderTable"].Value = Table == 0 ? "" : "رقم الطاولة :" + Table;
            rptForm.Parameters["DailyID"].Value = Dialy;
            rptForm.Parameters["SaleDate"].Value = Lip.GetServerDate() + "-" + Comon.ConvertSerialToTime(Lip.GetServerTimeSerial().ToString().Replace(":", "").Trim());
            rptForm.Parameters["OrderType"].Value = p1;
            rptForm.Parameters["CustomerName"].Value = "";// txtMobile.Text + "  " + lblCustomerName.Text.Trim().ToString();
            rptForm.DataSource = dataTable;
            rptForm.DataMember = ReportName;

            for (int i = 0; i < rptForm.Parameters.Count; i++)
                rptForm.Parameters[i].Visible = false;
            /********************** Details ****************************/
         
            decimal TransCost = 0;

            for (int i = 0; i <= dtsr.Rows.Count - 1; i++)
            {
                var row = dataTable.NewRow();
                row["BarCode"] = dtsr.Rows[i][ "BarCode"].ToString();
                row["ItemName"] = dtsr.Rows[i][ "ItemName"].ToString()+" "+dtsr.Rows[i][ "SizeName"].ToString();
                row["UnitName"] =dtsr.Rows[i][ "BarCode"].ToString();
                row["Qty"] = dtsr.Rows[i][ "QTY"].ToString();
                row["Total"] =dtsr.Rows[i][ "BarCode"].ToString();
                dataTable.Rows.Add(row);
            }


            /******************** Report Binding ************************/
            //    XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
            //   subreport.Visible = false;
            //  subreport.ReportSource = ReportComponent.CompanyHeader();
            rptForm.ShowPrintStatusDialog = false;
            rptForm.ShowPrintMarginsWarning = false;
            rptForm.CreateDocument();
            SplashScreenManager.CloseForm(false);
           var ShowReportInReportViewer = false;
            if (ShowReportInReportViewer = false)
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
                if (dt.Rows.Count > 0)
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
        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            int isConfirm = 0;

            try
            {
                //  GridView detailView = gridView1.GetDetailView(gridView1.FocusedRowHandle, gridView1.GetRelationIndex(gridView1.FocusedRowHandle, "Detilas")) as GridView;
                gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "Status", "1");


                for (int i = 0; i <= gridView2.DataRowCount - 1; i++)
                {
                    if (gridView2.GetRowCellValue(i, "Status").ToString() == "1")
                        isConfirm = isConfirm + 1;
                }
                var srs = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "BarCode").ToString();
                var srs2 = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "OrderID").ToString();
                var typeID = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "typeID").ToString();
                
                //   var srs = gridView2.GetRowCellValue(1"InvoiceID").ToString();

                //var srs = (sender as ButtonEdit).EditValue.ToString();
                // نعدل الفاتورة برقم السائق ونظبع نسختين موجود فيها اسمن السائق والحي والعنوان واسم العميل 
                var sr = " update OrderOnTabletDetials set Width=1,isprint=1 where typeID=" + typeID + " and  OrderID= " + srs2 + "  and  BarCode ='" + srs + "'";
                Lip.ExecututeSQL(sr);
                if (isConfirm <= gridView2.DataRowCount - 1)
                {
                    foreach (System.Data.DataColumn col in _sampleData.Columns) col.ReadOnly = false;
                    gridView2.SetRowCellValue(gridView2.FocusedRowHandle, "Status", "1");
                    DataRow[] rows = _sampleData.Select("OrderID=" + srs2.ToString() + " and typeID=" + typeID.ToString());
                    DataRow row = rows[0];
                    //Then from the DataRow[] rows assign a random row (for example the first row) to a DataRow object
                    int pkIndex = _sampleData.Rows.IndexOf(row);
                    _sampleData.Rows[pkIndex]["Width"] = 1;
                    _sampleData.AcceptChanges();
                    gridControl2.RefreshDataSource();
                }
                else
                {
                    foreach (System.Data.DataColumn col in _sampleData.Columns) col.ReadOnly = false;
                    sr = " update OrderOnTabletMaster set NeedReview=1 where typeID=" + typeID + " and   OrderID= " + srs2;
                    Lip.ExecututeSQL(sr);
                    int rowLayout = Comon.cInt(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "rowLayout").ToString());
                    int Width = Comon.cInt(layoutView1.GetRowCellValue(rowLayout, "CountID").ToString());
                    DataRow[] rows = _sampleData.Select("OrderID=" + srs2.ToString() + " and typeID=" + typeID.ToString());
                    DataRow row = rows[0];
                    //Then from the DataRow[] rows assign a random row (for example the first row) to a DataRow object

                    int pkIndex = _sampleData.Rows.IndexOf(row);
                    _sampleData.Rows[pkIndex]["Width"] = Width;
                    _sampleData.AcceptChanges();
                    gridControl2.RefreshDataSource();


                }
            }
            catch { }
        }

        private void gridView2_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (view.GetRowCellValue(e.RowHandle, "Status").ToString() == "1")
                {
                    e.Appearance.BackColor = System.Drawing.Color.FromArgb(39, 198, 2);// System.Drawing.Color.GreenYellow;//System.Drawing.Color.FromArgb(68, 191, 138);
                    e.Appearance.ForeColor = System.Drawing.Color.White;
                    e.HighPriority = true;
                }
            }
            catch { }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            layoutView1.MovePrevPage();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            layoutView1.MoveNextPage();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var sr = "     SELECT     OrderOnTabletMaster.DailyID, OrderOnTabletMaster.OrderID,OrderOnTabletMaster.typeID,"
       + "                COUNT(OrderOnTabletDetials.OrderID) AS CountID, SUM(OrderOnTabletDetials.Width) AS Width,OrderOnTabletMaster.OrderType,OrderOnTabletMaster.OrderTable , OrderOnTabletMaster.OrderDate,isnull(OrderOnTabletMaster.SaleType,1) as SaleType "
  + "  FROM            OrderOnTabletMaster INNER JOIN"
               + "             OrderOnTabletDetials ON OrderOnTabletMaster.OrderID = OrderOnTabletDetials.OrderID and OrderOnTabletMaster.typeID = OrderOnTabletDetials.typeID"
  + "  WHERE        (0 = 0) AND (OrderOnTabletMaster.NeedReview <> 2) and OrderOnTabletMaster.SaleType=0 "
  + " and  OrderOnTabletDetials.Description1<>'0Trans' "

  + " group by  OrderOnTabletMaster.SaleType,OrderOnTabletMaster.DailyID,OrderOnTabletMaster.OrderID,OrderOnTabletMaster.OrderDate,OrderOnTabletMaster.OrderType,OrderOnTabletMaster.OrderTable,OrderOnTabletMaster.typeID"
  + " ORDER BY OrderOnTabletMaster.OrderDate,OrderOnTabletMaster.DailyID ASC";

            _sampleData.Clear();
            _sampleData = Lip.SelectRecord(sr);
            gridControl2.DataSource = _sampleData;
            // " select DailyID,OrderType,InvoiceDate,InvoiceID,OrderTable from [dbo].[OrderOnTabletMaster]  where Cancel=0 and NeedReview<>1 order by InvoiceID ASC ";

            timer1.Enabled = true;
            ///int oo = timer1.Interval;
        }

        private void frmKDSV2_Load(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount <1)
            {

                Messages.MsgError(this.GetType().Name, "يرجى اختيار احد الطلبات المفتوحة ");
                return;
            }
             bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm,"تأكيد تسليم الطلب ");
             if (!Yes) return;
                        
          
                var srs = gridView2.GetRowCellValue(0, "BarCode").ToString();
                var srs2 = gridView2.GetRowCellValue(0, "OrderID").ToString();
             var typeID = gridView2.GetRowCellValue(0, "typeID").ToString();
          
                //   var srs = gridView2.GetRowCellValue(1"InvoiceID").ToString();

                //var srs = (sender as ButtonEdit).EditValue.ToString();
                // نعدل الفاتورة برقم السائق ونظبع نسختين موجود فيها اسمن السائق والحي والعنوان واسم العميل 
                var sr = " update OrderOnTabletDetials set Width=1 where  typeID="+typeID+" and  OrderID= " + srs2;
                Lip.ExecututeSQL(sr);
                sr = " update OrderOnTabletMaster set NeedReview=2 where  typeID=" + typeID + " and  OrderID= " + srs2;
            
                Lip.ExecututeSQL(sr);
                simpleButton1_Click(null,null);

                gridControl1.DataSource = null;



            














        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, "هل تريد انهاء الطلبات    ؟  ");
            if (!Yes) return;
            var sr = " update OrderOnTabletDetials set Width=1 ";
            Lip.ExecututeSQL(sr);
            sr = " update OrderOnTabletMaster set NeedReview=2 ";

            Lip.ExecututeSQL(sr);
            simpleButton1_Click(null, null);

            gridControl1.DataSource = null;
        }
        
    }
    public class MyData
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }

        public MyData(string name1, string name2)
        {
            Name1 = name1;
            Name2 = name2;
        }
    }
}
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
using DevExpress.ExpressApp;
using DevExpress.XtraGrid.Views.Grid;
using Edex.Model;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class frmKDS : DevExpress.XtraEditors.XtraForm
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
        public frmKDS()
        {
            InitializeComponent();
            _sampleDataDetials.Columns.Add(new DataColumn("ItemName", typeof(string)));
            _sampleDataDetials.Columns.Add(new DataColumn("QTY", typeof(string)));
            _sampleDataDetials.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            _sampleDataDetials.Columns.Add(new DataColumn("Status", typeof(string)));
            _sampleDataDetials.Columns.Add(new DataColumn("Confirm", typeof(string)));
            var sr = " select DailyID,OrderType,InvoiceDate,InvoiceID,TableID from [dbo].[Sales_SalesInvoiceMaster]  where Cancel=0 and NeedReview=0 order by InvoiceID ASC ";
            _sampleData = Lip.SelectRecord(sr);
            gridControl1.DataSource = _sampleData;
            sr = " SELECT        Sales_SalesInvoiceDetails.QTY, CONCAT(Stc_Items.ArbName,' - ',Stc_Items.EngName ) as ItemName , Sales_SalesInvoiceDetails.InvoiceID ,'a ' as Status ,'b'as Confirm"
+ " FROM            Sales_SalesInvoiceDetails INNER JOIN"
   + "                      Stc_Items ON Sales_SalesInvoiceDetails.ItemID = Stc_Items.ItemID INNER JOIN"
   + "                      Stc_ItemUnits ON Stc_Items.ItemID = Stc_ItemUnits.ItemID INNER JOIN"
    + "                     Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID  where Sales_SalesInvoiceDetails.Cancel=0  order by Sales_SalesInvoiceDetails.InvoiceID ASC ";




            sr = "      SELECT       Sales_SalesInvoiceDetails.BarCode ,   Sales_SalesInvoiceDetails.InvoiceID, Sales_SalesInvoiceDetails.QTY,CONCAT(Stc_Items.ArbName,' - ',Stc_Items.EngName ) as ItemName, Stc_Items.GroupID, Stc_SizingUnits.ArbName AS SizeName, Stc_ItemsGroups.ArbName AS GroupName"
+ " ,Sales_SalesInvoiceDetails.Width as Status  FROM            Stc_SizingUnits INNER JOIN"
   +"                    Sales_SalesInvoiceDetails ON Stc_SizingUnits.SizeID = Sales_SalesInvoiceDetails.SizeID LEFT OUTER JOIN"
       +"               Stc_ItemsGroups INNER JOIN "
        +"               Stc_Items ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID ON Sales_SalesInvoiceDetails.ItemID = Stc_Items.ItemID   where Sales_SalesInvoiceDetails.Cancel=0  order by Sales_SalesInvoiceDetails.InvoiceID ASC ";











            _sampleDataDetials = Lip.SelectRecord(sr);
            MSgSettingsDetials msgdetials;
            foreach (DataRow row in _sampleDataDetials.Rows)
            {
                msgdetials = new MSgSettingsDetials();
                msgdetials.InvoiceID = Comon.cInt(row["InvoiceID"].ToString());
                msgdetials.ItemName = row["ItemName"].ToString();
                msgdetials.QTY = row["QTY"].ToString();
                msgdetials.BarCode = row["BarCode"].ToString();
                msgdetials.Status = row["Status"].ToString();
                lstDetail.Add(msgdetials);

            }

            gridControl1.ForceInitialize();
            int i = 0;
            while (gridView1.IsValidRowHandle(i))
            {
                gridView1.ExpandMasterRow(i);
                i += 1;
            }
        }
        MasterDetailHelper helper;
        private void frmKDS_Load(object sender, EventArgs e)
        {




        }
        private void gridView1_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            GridView view = sender as GridView;
            int ID = Comon.cInt(view.GetRowCellValue(e.RowHandle, "InvoiceID"));
            if (ID != 0)
            {
                var dr = _sampleData.Select("InvoiceID=" + ID);
                if (dr.Length > 0)
                    e.IsEmpty = false;
                else
                    e.IsEmpty = true;
                //this.Close();
            }
        }

        private void gridView1_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            GridView view = sender as GridView;
            int ID = Comon.cInt(view.GetRowCellValue(e.RowHandle, "InvoiceID"));
            e.ChildList = lstDetail.Where(x => x.InvoiceID == ID).ToList();

        }

        private void gridView1_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gridView1_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Detilas";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sr = " select DailyID,OrderType,InvoiceDate,InvoiceID,TableID from [dbo].[Sales_SalesInvoiceMaster]  where Cancel=0  and NeedReview=0 order by InvoiceID ASC ";
            var sdr = Lip.SelectRecord(sr);
            if (sdr.Rows.Count > _sampleData.Rows.Count )
            {

                DataRow dr;
                for (int i = _sampleData.Rows.Count; i <= sdr.Rows.Count - 1; ++i)
                {

                    dr = _sampleData.NewRow();
                    dr[0] = sdr.Rows[i][0];
                    dr[1] = sdr.Rows[i][1];
                    dr[2] = sdr.Rows[i][2];
                    dr[3] = sdr.Rows[i][3];
                    dr[4] = sdr.Rows[i][4];
                    _sampleData.Rows.Add(dr);
                    var sr1 = "      SELECT       Sales_SalesInvoiceDetails.BarCode ,   Sales_SalesInvoiceDetails.InvoiceID, Sales_SalesInvoiceDetails.QTY,CONCAT(Stc_Items.ArbName,' - ',Stc_Items.EngName ) as ItemName, Stc_Items.GroupID, Stc_SizingUnits.ArbName AS SizeName, Stc_ItemsGroups.ArbName AS GroupName"
+ " ,Sales_SalesInvoiceDetails.Width as Status  FROM            Stc_SizingUnits INNER JOIN"
+ "                    Sales_SalesInvoiceDetails ON Stc_SizingUnits.SizeID = Sales_SalesInvoiceDetails.SizeID LEFT OUTER JOIN"
    + "               Stc_ItemsGroups INNER JOIN "
     + "               Stc_Items ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID ON Sales_SalesInvoiceDetails.ItemID = Stc_Items.ItemID   where Sales_SalesInvoiceDetails.Cancel=0  and  Sales_SalesInvoiceDetails.InvoiceID=" + dr[3].ToString()+ "   order by Sales_SalesInvoiceDetails.InvoiceID ASC ";


                    var sdr1 = Lip.SelectRecord(sr1);

                    DataRow dr1;
                    MSgSettingsDetials msgdetials;
                    for (int items = 0; items <= sdr1.Rows.Count - 1; ++items)
                    {

                        msgdetials = new MSgSettingsDetials();
                        msgdetials.InvoiceID = Comon.cInt(sdr1.Rows[items]["InvoiceID"].ToString());
                        msgdetials.ItemName = sdr1.Rows[items]["ItemName"].ToString();
                        msgdetials.QTY = sdr1.Rows[items]["QTY"].ToString();
                        msgdetials.Status = sdr1.Rows[items]["Status"].ToString();

                        msgdetials.BarCode = sdr1.Rows[items]["BarCode"].ToString();
                        lstDetail.Add(msgdetials);
                        DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs eq;

                        //eq.ChildList = lstDetail.Where(x => x.InvoiceID == msgdetials.InvoiceID).ToList();
                    }

                }

              
                gridControl1.RefreshDataSource();
                //_sampleData = Lip.SelectRecord(sr);
                    //gridControl1.ForceInitialize();
                    int u = 0;
                    while (gridView1.IsValidRowHandle(u))
                    {
                        gridView1.ExpandMasterRow(u);
                       u += 1;
                    }
               
            }



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled=false;
            button1_Click(null, null);
            timer1.Enabled = true; ;
        }

        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            int isConfirm=0;
          
            GridView detailView = gridView1.GetDetailView(gridView1.FocusedRowHandle, gridView1.GetRelationIndex(gridView1.FocusedRowHandle, "Detilas")) as GridView ;
            detailView.SetRowCellValue(detailView.FocusedRowHandle, "Status", "1");
            
            for (int i = 0; i <= detailView.DataRowCount - 1; i++)
            {
                if (detailView.GetRowCellValue(i, "Status").ToString() == "1")
                    isConfirm = isConfirm + 1;
            }
            var srs = detailView.GetRowCellValue(detailView.FocusedRowHandle, "BarCode").ToString();
            var srs2 = detailView.GetRowCellValue(detailView.FocusedRowHandle, "InvoiceID").ToString();
            //   var srs = gridView2.GetRowCellValue(1"InvoiceID").ToString();

            //var srs = (sender as ButtonEdit).EditValue.ToString();
            // نعدل الفاتورة برقم السائق ونظبع نسختين موجود فيها اسمن السائق والحي والعنوان واسم العميل 
            var sr = " update Sales_SalesInvoiceDetails set Width=1 where InvoiceID= " + srs2 + "  and  BarCode ='" + srs + "'";
            Lip.ExecututeSQL(sr);
            if (isConfirm <= detailView.DataRowCount - 1)
            {
                
                detailView.SetRowCellValue(detailView.FocusedRowHandle, "Status", "1");
            }
            else {

                sr = " update Sales_SalesInvoiceMaster set NeedReview=1 where InvoiceID= " + srs2;
                Lip.ExecututeSQL(sr);

                gridView1.DeleteSelectedRows();
                DataRow dr = _sampleData.Rows[0];
                dr.Delete();
                _sampleData.AcceptChanges();
                 
            }
        }

        private void gridView2_RowClick(object sender, RowClickEventArgs e)
        {
           
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //gridControl1.ForceInitialize();
            int u = 0;
            while (gridView1.IsValidRowHandle(u))
            {
                gridView1.ExpandMasterRow(u);
                u += 1;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //gridControl1.ForceInitialize();
            int u = 0;
            while (gridView1.IsValidRowHandle(u))
            {
                gridView1.CollapseMasterRow(u);
                u += 1;
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            layoutView1.MovePrevPage();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            layoutView1.MoveNextPage();
        }
    }
}
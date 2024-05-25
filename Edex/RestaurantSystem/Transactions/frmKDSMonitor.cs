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
using Edex.DAL.Configuration;
using System.IO;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class frmKDSMonitor : DevExpress.XtraEditors.XtraForm
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
       // List<MyData> data = new List<MyData>();

        public frmKDSMonitor()
        {
            InitializeComponent();
            //var sr = "Select   Stc_Items.GroupID, Stc_Items.ItemID, Stc_Items.ArbName as ItemName,Stc_Items.ItemImage from Stc_Items where Stc_Items.TypeID=1 and Cancel=0   and (CostCenterID=0 or CostCenterID= " + 0 + ")";
            //var dtFillGrid = Lip.SelectRecord(sr);
            //if (dtFillGrid.Rows.Count > 0)
            //{

            //    for (int i = 0; i <= dtFillGrid.Rows.Count - 1; ++i)
            //    {

            //        if (DBNull.Value == dtFillGrid.Rows[i]["ItemImage"])
            //        {
            //            dtFillGrid.Rows[i]["ItemImage"] = null;


            //        }


            //    }

            //}
            //gridControl2.DataSource = dtFillGrid;

            
            //data.Add(new MyData("TestData1","AnotherTestData1"));
            //data.Add(new MyData("TestData2","AnotherTestData2"));
            //data.Add(new MyData("TestData3", "AnotherTestData3"));
            //data.Add(new MyData("TestData4", "AnotherTestData4"));
            //gridControl2.DataSource = data;
            //timer1.Enabled = true;
            var sr = "     SELECT        OrderOnTabletMaster.DailyID, OrderOnTabletMaster.OrderID,"
                + "                COUNT(OrderOnTabletDetials.OrderID) AS CountID, SUM(OrderOnTabletDetials.Width) AS Width"
                +"  FROM            OrderOnTabletMaster INNER JOIN"
                + "             OrderOnTabletDetials ON OrderOnTabletMaster.OrderID = OrderOnTabletDetials.OrderID and OrderOnTabletMaster.typeID = OrderOnTabletDetials.typeID"
                +"  WHERE        (0 = 0) AND (OrderOnTabletMaster.NeedReview <> 2)"
                + " and  OrderOnTabletDetials.Description1<>'0Trans' and OrderOnTabletMaster.OrderType=2 "
                + " group by  OrderOnTabletMaster.DailyID,OrderOnTabletMaster.OrderID"
                + " ORDER BY OrderOnTabletMaster.DailyID";

        _sampleData = Lip.SelectRecord(sr);
        try
        {
            MemoryStream TheImage;
            gridControl2.DataSource = _sampleData;
            CompanyHeader cmpheader = new CompanyHeader();
            cmpheader = CompanyHeaderDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
            // " select DailyID,OrderType,InvoiceDate,InvoiceID,TableID from [dbo].[OrderOnTabletMaster]  where Cancel=0 and NeedReview<>1 order by InvoiceID ASC ";
            TheImage = new MemoryStream(cmpheader.pic);
            if (TheImage.Length > 0)
                picCompanySymbol.Image = Image.FromStream(TheImage, true);
        }
        catch { picCompanySymbol.Visible = false; };
        timer1.Enabled = true;

        }
       
        private void layoutView1_CustomFieldValueStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewFieldValueStyleEventArgs e)
        {
            //  return;  // Painting the content of the focused card only if the LayoutView itself has the focus.
            ColumnView view = sender as ColumnView;
            if (view == null) return;
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
                //System.Media.SoundPlayer sp = new System.Media.SoundPlayer(@"C:\Eatex\WhatsApp Ptt 2019-10-12 at 3.50.59 PM.wav");
                //sp.Play();
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
            if (view == null) return;
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
            if (view == null) return;
            if (e.RowHandle == view.FocusedRowHandle && view.FocusedRowHandle > 0 && (view.GridControl.Focused))
            {
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.Options.UseFont = true;
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            var sr = "     SELECT        OrderOnTabletMaster.DailyID, OrderOnTabletMaster.OrderID,"
    + "                COUNT(OrderOnTabletDetials.OrderID) AS CountID, SUM(OrderOnTabletDetials.Width) AS Width"
+ "  FROM            OrderOnTabletMaster INNER JOIN"
            + "             OrderOnTabletDetials ON OrderOnTabletMaster.OrderID = OrderOnTabletDetials.OrderID and OrderOnTabletMaster.typeID = OrderOnTabletDetials.typeID"
+ "  WHERE        (0 = 0) AND (OrderOnTabletMaster.NeedReview <> 2)"
  + " and  OrderOnTabletDetials.Description1<>'0Trans'  and OrderOnTabletMaster.OrderType=2 "
+ " group by  OrderOnTabletMaster.DailyID,OrderOnTabletMaster.OrderID"
+ " ORDER BY OrderOnTabletMaster.DailyID";
            _sampleData.Clear();
            _sampleData = Lip.SelectRecord(sr);
            gridControl2.DataSource = _sampleData;
            timer1.Enabled = true;
        }
        void layoutView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                if (layoutView1.RowCount < 1)
                    return;
                var hiTinfo = layoutView1.CalcHitInfo(e.Location);
                if (hiTinfo.InFieldValue)
                {
                    
                    //long ID = Comon.cLong(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "InvoiceID").ToString());
                    //int count = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "CountID").ToString());
                    //int width = Comon.cInt(layoutView1.GetRowCellValue(hiTinfo.RowHandle, "Width").ToString());
                    //if (count == width)
                    //{
                    //    var sr = " update OrderOnTabletMaster set NeedReview=2 where InvoiceID= " + ID;
                    //    Lip.ExecututeSQL(sr);
                    //}



                }
                timer1.Enabled = true;
            }
            catch { timer1.Enabled = true; }
        }

        private void frmKDSMonitor_Load(object sender, EventArgs e)
        {

        }
    }
  
}
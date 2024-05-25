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
using Edex.ModelSystem;
using Edex.RestaurantSystem.Code;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class frmKDSGroup : DevExpress.XtraEditors.XtraForm
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

        public frmKDSGroup()
        {
            InitializeComponent();
            refreshData();

          // " select DailyID,OrderType,InvoiceDate,InvoiceID,TableID from [dbo].[Sales_SalesInvoiceMaster]  where Cancel=0 and NeedReview<>1 order by InvoiceID ASC ";

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

       
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            refreshData();

            timer1.Enabled = true;
        }
     

        private void frmKDSMonitor_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //frmItemGroupToOrder frm = new frmItemGroupToOrder();
            //frm.FormClosed += frm_FormClosed;
            //frm.ShowDialog();
        }

        void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            refreshData();
        }

        private void refreshData()
        {
            gridControl1.MainView = gridView2;
            var dtGeneral = ReportComponent.SelectRecord("SELECT GroupID From ItemsGroupID ");
            var sr1 = "      SELECT     Sum(OrderOnTabletDetials.QTY) as QTY ,Concat( Stc_SizingUnits.ArbName,' ', Stc_Items.ArbName,CHAR(13),Stc_SizingUnits.EngName,' ', Stc_Items.EngName,CHAR(13)) as ItemName, Stc_Items.GroupID,CONCAT(Stc_SizingUnits.ArbName,' - ',Stc_SizingUnits.EngName ) AS SizeName, Stc_ItemsGroups.ArbName AS GroupName"
    + "   FROM            Stc_SizingUnits INNER JOIN"
    + "                    OrderOnTabletDetials ON Stc_SizingUnits.SizeID = OrderOnTabletDetials.SizeID LEFT OUTER JOIN"
      + "               Stc_ItemsGroups INNER JOIN "
       + "               Stc_Items ON Stc_ItemsGroups.GroupID = Stc_Items.GroupID ON OrderOnTabletDetials.ItemID = Stc_Items.ItemID   where    OrderOnTabletDetials.Width=0 and OrderOnTabletDetials.Description1<>'0Trans'  and  ";

            if (dtGeneral.Rows.Count > 0)
            {

                sr1 = sr1 + " ( ";
                foreach (DataRow drow in dtGeneral.Rows)
                    sr1 = sr1 + " Stc_ItemsGroups.GroupID =" + drow[0].ToString() + "or ";
                sr1 = sr1.Remove(sr1.Length - 3, 3);
                sr1 = sr1 + ") ";
            }
            sr1 = sr1 + "  group by  Stc_Items.ArbName,Stc_Items.EngName ,Stc_Items.GroupID ,Stc_SizingUnits.ArbName,Stc_SizingUnits.EngName,Stc_ItemsGroups.ArbName   ";
            _sampleData = Lip.SelectRecord(sr1);

            gridControl1.DataSource = _sampleData;
        }
    }
    public class MyData1
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }

        public MyData1(string name1, string name2)
        {
            Name1 = name1;
            Name2 = name2;
        }
    }
}
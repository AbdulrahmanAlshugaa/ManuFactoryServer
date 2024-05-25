using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using Edex.Model;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class uReturnInvoice : UserControl
    {
        public  static string[] extensionvar = new string[3];
        public static string row_handel = "";
        public string notes = "";
        public DataTable _sampleData = new DataTable();
        public uReturnInvoice()
        {
            InitializeComponent();

            

        }

        public uReturnInvoice(int OrderID)
        {
            InitializeComponent();

            _sampleData.Columns.Add(new DataColumn("ItemID", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ItemName", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("Barcode", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("ID", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("ISPrint", typeof(string)));

            _sampleData.Columns.Add(new DataColumn("notes", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("rowhandels", typeof(string)));
            _sampleData.Columns.Add(new DataColumn("Description", typeof(string)));


            _sampleData.Columns.Add(new DataColumn("Price", typeof(decimal)));


            _sampleData.Columns.Add(new DataColumn("Qty", typeof(decimal)));
            _sampleData.Columns.Add(new DataColumn("Spilt", typeof(decimal)));

            var sr1 = "SELECT   OrderOnTabletDetials.Price,OrderOnTabletDetials.ISPrint,OrderOnTabletDetials.notes,OrderOnTabletDetials.rowhandels,OrderOnTabletDetials.Description,  OrderOnTabletDetials.ID,  OrderOnTabletDetials.ItemID,  OrderOnTabletDetials.Qty ,OrderOnTabletDetials.Barcode, (OrderOnTabletDetials.Qty-OrderOnTabletDetials.Qty) as Spilt  , Concat( Stc_SizingUnits.ArbName,' ', Stc_Items.ArbName,Stc_SizingUnits.EngName,' ', Stc_Items.EngName, CHAR(13) ,OrderOnTabletDetials.Description,CHAR(13),OrderOnTabletDetials.notes)as ItemName"

+ " FROM            Stc_SizingUnits LEFT OUTER JOIN"
+ "                     OrderOnTabletDetials ON Stc_SizingUnits.SizeID = OrderOnTabletDetials.SizeID LEFT OUTER JOIN"
+ "                  Stc_Items ON OrderOnTabletDetials.ItemID = Stc_Items.ItemID"
    + " 			 where    OrderOnTabletDetials.OrderID=" + OrderID + "  and  OrderOnTabletDetials.typeID =1";


            var sdr1 = Lip.SelectRecord(sr1);
            if (sdr1.Rows.Count < 1) this.Dispose();
            DataRow row;
            foreach (DataRow drow in sdr1.Rows) {
                row = _sampleData.NewRow();

                row["ItemID"] = drow["ItemID"].ToString();
                row["ItemName"] = drow["ItemName"].ToString();
                row["Barcode"] = drow["Barcode"].ToString();
                row["ID"] = drow["ID"].ToString();
                row["ISPrint"] = drow["ISPrint"].ToString();
                row["notes"] = drow["notes"].ToString();
                row["rowhandels"] = drow["rowhandels"].ToString();
                row["Description"] = drow["Description"].ToString();
                row["Qty"] = Comon.ConvertToDecimalQty(drow["Qty"].ToString());
                row["Price"] = Comon.ConvertToDecimalPrice(drow["Price"].ToString());
                row["Spilt"] = Comon.ConvertToDecimalQty(0);
                _sampleData.Rows.Add(row);
            
            }
            gridControl2.DataSource = _sampleData;
        }

         private void gridControl1_Click(object sender, EventArgs e)
         {

         }

         private void panelControl3_Paint(object sender, PaintEventArgs e)
         {

         }

         private void uAddExtension_Load(object sender, EventArgs e)
         {
            
         }

         private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
         {
             GridView view = sender as GridView;
             if (view.IsRowSelected(e.RowHandle))
             {
                 e.Appearance.BackColor = System.Drawing.Color.Yellow;// System.Drawing.Color.FromArgb(25, 71, 138);
                 e.Appearance.ForeColor = System.Drawing.Color.Black;
                 e.HighPriority = true;
             }
         }

         private void panelControl4_Paint(object sender, PaintEventArgs e)
         {

         }

         public void btnAccept_Click(object sender, EventArgs e)
         {
            
             SendKeys.Send("{ESC}");
         }

         private void panelControl4_Paint_1(object sender, PaintEventArgs e)
         {

         }

         private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
         {
             try
             {
                 decimal qtyBefore = Comon.ConvertToDecimalQty(gridView1.GetFocusedRowCellValue("Qty").ToString());
                 decimal qtyAfter = Comon.ConvertToDecimalQty(gridView1.GetFocusedRowCellValue("Spilt").ToString());
                 string ID = gridView1.GetFocusedRowCellValue("ID").ToString();
                 if (qtyBefore == 0) return;
                 gridView1.SetFocusedRowCellValue("Spilt", (qtyAfter + 1));
                 gridView1.SetFocusedRowCellValue("Qty", (qtyBefore - 1));
                 foreach (System.Data.DataColumn col in _sampleData.Columns) col.ReadOnly = false;
                 DataRow[] rows = _sampleData.Select("ID=" + gridView1.GetFocusedRowCellValue("ID").ToString());
                 DataRow row = rows[0];
                 //Then from the DataRow[] rows assign a random row (for example the first row) to a DataRow object
                 int pkIndex = _sampleData.Rows.IndexOf(row);
                 _sampleData.Rows[pkIndex]["Spilt"] = qtyAfter + 1;
                 _sampleData.Rows[pkIndex]["Qty"] = qtyBefore - 1;
                 _sampleData.AcceptChanges();
             }
             catch { }
         }

         private void repositoryItemButtonEdit2_Click(object sender, EventArgs e)
         {
             try
             {
                 decimal qtyBefore = Comon.ConvertToDecimalQty(gridView1.GetFocusedRowCellValue("Qty").ToString());
                 decimal qtyAfter = Comon.ConvertToDecimalQty(gridView1.GetFocusedRowCellValue("Spilt").ToString());
                 if (qtyAfter == 0) return;
                 gridView1.SetFocusedRowCellValue("Qty", qtyBefore + 1);
                 gridView1.SetFocusedRowCellValue("Spilt", (qtyAfter - 1));
                 foreach (System.Data.DataColumn col in _sampleData.Columns) col.ReadOnly = false;
                 DataRow[] rows = _sampleData.Select("ID=" + gridView1.GetFocusedRowCellValue("ID").ToString());
                 DataRow row = rows[0];
                 //Then from the DataRow[] rows assign a random row (for example the first row) to a DataRow object
                 int pkIndex = _sampleData.Rows.IndexOf(row);
                 _sampleData.Rows[pkIndex]["Spilt"] = qtyAfter - 1;
                 _sampleData.Rows[pkIndex]["Qty"] = qtyBefore + 1;
                 _sampleData.AcceptChanges();
             }
             catch { }
         }
            
    }
}

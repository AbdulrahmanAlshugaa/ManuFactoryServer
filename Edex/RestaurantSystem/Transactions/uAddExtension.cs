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
    public partial class uAddExtension : UserControl
    {
        public  static string[] extensionvar = new string[3];
        public static string row_handel = "";
        public string notes = "";
        public uAddExtension()
        {
            InitializeComponent();
      
        }

         public uAddExtension(string Barcode,string ItemName,string UnitName,string rowhandel,string notes)
        {
            InitializeComponent();
            labelControl2.Text = UnitName + " " + ItemName;
            this.notes = notes;

        var sr = " SELECT ItemsExtension_Details.BarCode,ItemsExtension_Details.QTY as SalePrice,"
+ " Concat ((Select top 1 dbo.Stc_Items.ArbName from Stc_Items Inner join Stc_ItemUnits on Stc_ItemUnits.ItemID=Stc_Items.ItemID  where Stc_ItemUnits.BarCode=ItemsExtension_Details.BarCode),' ',(Select top 1 dbo.Stc_Items.EngName from Stc_Items Inner join Stc_ItemUnits on Stc_ItemUnits.ItemID=Stc_Items.ItemID  where Stc_ItemUnits.BarCode=ItemsExtension_Details.BarCode)) As ItemName"
+"   ,(Select top 1 dbo.Stc_Items.ItemID from Stc_Items Inner join Stc_ItemUnits on Stc_ItemUnits.ItemID=Stc_Items.ItemID  where Stc_ItemUnits.BarCode=ItemsExtension_Details.BarCode) As ItemID"
 +"   ,(Select top 1 dbo.Stc_SizingUnits.SizeID from Stc_SizingUnits Inner join Stc_ItemUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID  where Stc_ItemUnits.BarCode=ItemsExtension_Details.BarCode) As SizeID"

+ "  FROM            ItemsExtension_Details INNER JOIN"
     +"                      ItemsExtension_Master ON ItemsExtension_Details.ManufacturingID = ItemsExtension_Master.ManufacturingID"
        + "  				 where ItemsExtension_Master.BarCode='" + Barcode + "'    order by ItemsExtension_Details.ItemID ASC";
        var dr = Lip.SelectRecord(sr);
        gridControl2.DataSource = dr;
        var spiltindex = rowhandel.Split(':');
      
        if (spiltindex.Length == 1 && spiltindex[0] == "") return;
        for (int i = 0; i <= spiltindex.Length - 1; ++i) {

            gridView1.SelectRow(Comon.cInt(spiltindex[i])); 
            gridView1.MakeRowVisible(i);
        
        }
        //notes.ToString();

        }

         private void gridControl1_Click(object sender, EventArgs e)
         {

         }

         private void panelControl3_Paint(object sender, PaintEventArgs e)
         {

         }

         private void uAddExtension_Load(object sender, EventArgs e)
         {
             txtNotes.Text = notes;
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
             string extension="";
             decimal price =0;
             string notes="";
             row_handel = "";
             foreach (var rowHandle in gridView1.GetSelectedRows())
             {
                 price += Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(rowHandle, "SalePrice").ToString());
                 extension += gridView1.GetRowCellValue(rowHandle, "ItemName").ToString() + "\n ";
                 row_handel += rowHandle + ":";
             }
           
             notes = txtNotes.Text;
             extensionvar[0] = extension;
             extensionvar[1] = price.ToString();
             extensionvar[2] = notes;
             SendKeys.Send("{ESC}");
         }

         private void panelControl4_Paint_1(object sender, PaintEventArgs e)
         {

         }
            
    }
}

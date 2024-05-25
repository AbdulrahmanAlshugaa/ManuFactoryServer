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
using Edex.Model;
using System.IO;
using DevExpress.XtraGrid.Views.Base;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class frmSizeItem : DevExpress.XtraEditors.XtraForm
    {
       
        public frmSizeItem()
        {
            InitializeComponent();

            var sr = "Select  Stc_Items.GroupID, Stc_Items.ArbName as ItemName,Stc_Items.ItemImage,Stc_ItemUnits.BarCode from Stc_Items inner join Stc_ItemUnits on Stc_Items.ItemID=Stc_ItemUnits.ItemID   where ";
           var dt = Lip.SelectRecord(sr);

        }

        public frmSizeItem(DataTable dt)
        {
            InitializeComponent();
         decimal qty=0;
         foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
         int i = 0;
          //  var sr = "Select   Stc_SizingUnits.ArbName  as ItemName, Stc_Items.ItemImage,Stc_ItemUnits.BarCode from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID  left  join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_ItemUnits.SizeID   where  Stc_Items.ItemID= " + ID;
            foreach (DataRow row in dt.Rows)
            {
                row["ItemImage"] = DefaultImage();
                row["PackingQtyParent"] = dt.Rows[dt.Rows.Count - 1]["PackingQty"].ToString();
                row["ArbName"] = dt.Rows[i]["ArbName"].ToString() + " "  + dt.Rows[i]["SalePrice"].ToString() ;

                i++;
            }
            labelControl1.Text = dt.Rows[0]["ItemNAme"].ToString();
            gridControl2.DataSource = dt;
            layoutView1.FocusedRowHandle = 1000;
         
          
        }

        private byte[] DefaultImage()
        {
            try
            {
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                Path = Path + @"\Images\Default.png";
                System.Drawing.Image img = System.Drawing.Image.FromFile(Path);
                MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
            catch { return null; }

        }



        private void frmSizeItem_Load(object sender, EventArgs e)
        {
            //if (layoutView1.RowCount == 1)
            //{
            //    layoutView1.MoveFirst();
            //    layoutView1_MouseDown(layoutView1, null);
            //    this.Close();
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void layoutView1_MouseDown(object sender, MouseEventArgs e)
        {
            //var hiTinfo = layoutView1.CalcHitInfo(e.Location);
            //if (hiTinfo.InFieldValue)
            //{
            //    if (hiTinfo.Column.FieldName == "ItemImage")
            //    {
            //        //long ID = Comon.cLong(layoutView1.GetFocusedRowCellValue("ItemID").ToString());

            //        //// frmSize.layoutView1.CardClick += layoutViewSizing_CardClick;
            //        //// FlyoutAction action = new FlyoutAction();

            //        //// FlyoutProperties properties = new FlyoutProperties();

            //        ////    properties.Style = FlyoutStyle.Popup;
            //        //frmSize = new frmSizeItem(ID);
            //        //frmSize.layoutView1.MouseDown += layoutViewItem_MouseDown;
            //        ////  FlyoutDialog.Show(this, frmSize, action, properties);




            //        //frmSize.ShowDialog();

            //    }
            //}
        }

        private void layoutView1_CustomFieldValueStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewFieldValueStyleEventArgs e)
        {
            //ColumnView cardView = sender as ColumnView;
            //if (cardView == null) return;
            //if (cardView.FocusedRowHandle == e.RowHandle && cardView.IsFocusedView)// && cardView.FocusedColumn == e.Column)
            //{
            //    e.Appearance.BackColor = Color.Gold;
            //    e.Appearance.BackColor = Color.Gold;
            //    e.Appearance.BackColor2 = Color.Gold;
            //    e.Appearance.ForeColor = Color.Black;
            //    return;

            //}
        }

        private void frmSizeItem_Leave(object sender, EventArgs e)
        {
           // this.Close();
        }

        private void frmSizeItem_Deactivate(object sender, EventArgs e)
        {
           // this.Close();
        }
    }
}
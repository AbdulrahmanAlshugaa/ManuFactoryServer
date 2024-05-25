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
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using DevExpress.XtraGrid.Views.Grid;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmExpiryDateItem : DevExpress.XtraEditors.XtraForm
    {
        public int StoreID;
        public string CelValue = "";
        public string Barcode;
        public frmExpiryDateItem(string Barcode,int StoreID)
        {
            InitializeComponent();
            var dt = Stc_itemsDAL.GetItemDataExpiry1(Barcode, UserInfo.FacilityID, StoreID);
            gridControl1.DataSource = dt;
        }


        public frmExpiryDateItem(string Barcode)
        {
            InitializeComponent();
            this.Barcode = Barcode;
            var dt = Stc_itemsDAL.GetItemDataExpiry(Barcode, UserInfo.FacilityID);
            gridControl1.DataSource = dt;
        }
        private void frmExpiryDateItem_Load(object sender, EventArgs e)
        {
          

        }
        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {

                //CelValue = e.CellValue.ToString();
                //this.Close();
            }
            catch { }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView view = sender as GridView;
        try{
            CelValue = view.GetFocusedRowCellValue("ExpiryDate").ToString();
            if (StoreID==null)
            StoreID = Comon.cInt(view.GetFocusedRowCellValue("StoreID").ToString());
            this.Close();

             }
            catch { }
        }
    }
}
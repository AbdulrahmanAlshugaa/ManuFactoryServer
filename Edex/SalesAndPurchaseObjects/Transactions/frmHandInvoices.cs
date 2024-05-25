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
    public partial class frmHandInvoices : DevExpress.XtraEditors.XtraForm
    {
        public int StoreID=0;
        public string CelValue = "";
        public string Barcode;
        public frmHandInvoices()
        {
            InitializeComponent();
        }


        public frmHandInvoices(string Barcode)
        {
            InitializeComponent();
             
        }
        private void frmExpiryDateItem_Load(object sender, EventArgs e)
        {
            string StrSql = "Select InvoiceID  , InvoiceDate    from Sales_SalesInvoiceMasterHand";
            var dt = Lip.SelectRecord(StrSql);
            gridControl1.DataSource = dt;
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
            StoreID = Comon.cInt(view.GetFocusedRowCellValue("InvoiceID").ToString());

            this.Close();

             }
            catch { }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void frmHandInvoices_Shown(object sender, EventArgs e)
        {       
            string StrSql = "Select InvoiceID, CONCAT(SUBSTRING(CAST(RegTime AS varchar(38)), 1, 2) ,':',   SUBSTRING(CAST(RegTime AS varchar(38)), 2, 2) )AS RegTime , CONCAT( SUBSTRING(CAST(CAST(InvoiceDate AS numeric) AS nvarchar),5,2),'-',SUBSTRING(CAST(CAST(InvoiceDate AS numeric) AS nvarchar),7,2))  AS InvoiceDate,  CustomerName    from Sales_SalesInvoiceMasterHand";
            var dt = Lip.SelectRecord(StrSql);
             
            gridControl1.DataSource = dt;
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
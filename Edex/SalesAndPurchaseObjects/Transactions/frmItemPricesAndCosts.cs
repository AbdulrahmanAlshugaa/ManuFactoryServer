using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmItemPricesAndCosts : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string CelValue = "";
        public long ItemID = 0;
        public int SizeID = 0;
        public long SupplierID = 0;
        public long CustomerID = 0;

        public frmItemPricesAndCosts()
        {
            InitializeComponent();
            ribbonControl1.Visible = false;

        }

        private void frmItemPricesAndCosts_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt;
                DataTable dtSublier;
                string strSQL;

                strSQL = "SELECT CostPrice AS [سعر التكلفة], LastCostPrice AS [اخر سعر تكلفة], SpecialCostPrice AS [سعر خاص], SalePrice AS [سعر البيع] , LastSalePrice AS [اخر سعر بيع]  , 0 AS [سعر المورد]"
                    + " FROM Stc_ItemUnits Where ItemID=" + ItemID + " And SizeID=" + SizeID;
                dt = Lip.SelectRecord(strSQL);

                strSQL = "SELECT dbo.Sales_PurchaseInvoiceDetails.ItemID, dbo.Sales_PurchaseInvoiceDetails.SizeID, MAX(dbo.Sales_PurchaseInvoiceDetails.CostPrice) AS CostPrice,dbo.Sales_PurchaseInvoiceMaster.SupplierID "
                + " FROM  dbo.Sales_PurchaseInvoiceDetails LEFT OUTER JOIN dbo.Sales_PurchaseInvoiceMaster ON dbo.Sales_PurchaseInvoiceDetails.FacilityID = dbo.Sales_PurchaseInvoiceMaster.FacilityID AND dbo.Sales_PurchaseInvoiceDetails.BranchID = dbo.Sales_PurchaseInvoiceMaster.BranchID AND "
                + " dbo.Sales_PurchaseInvoiceDetails.InvoiceID = dbo.Sales_PurchaseInvoiceMaster.InvoiceID  GROUP BY dbo.Sales_PurchaseInvoiceDetails.ItemID, dbo.Sales_PurchaseInvoiceDetails.SizeID, dbo.Sales_PurchaseInvoiceMaster.SupplierID "
                + " HAVING  (dbo.Sales_PurchaseInvoiceMaster.SupplierID = " + SupplierID + ") AND (dbo.Sales_PurchaseInvoiceDetails.ItemID = " + ItemID + ") AND "
                + " (dbo.Sales_PurchaseInvoiceDetails.SizeID = " + SizeID + ")  ";


                dtSublier = Lip.SelectRecord(strSQL);
                if (dtSublier.Rows.Count > 0)
                {
                    dt.Columns[5].ReadOnly = false;
                    dt.Rows[0][5] = dtSublier.Rows[0]["CostPrice"].ToString();
                }
                gridControl.DataSource = dt;

            }
            catch (Exception ex)
            {
                //WT.msgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            CelValue = e.CellValue.ToString();
            this.Close();
        }
    }
}

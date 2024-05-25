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
    public partial class frmWrningItemQty : DevExpress.XtraEditors.XtraForm
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
        string strSQL;
        public frmWrningItemQty()
        {
            InitializeComponent();
            GetStocktaking();

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
            //GetStocktaking();
           // timer1.Enabled = true;
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
           

        }

        private void GetStocktaking()
        {
            try
            {
                DataRow row;
                DataTable dt = Lip.SelectRecord("Select ArbName,ItemID  , 1 AS SizeID,  0.0 AS  Qty from Stc_Items where TypeID=5");

                int k = 0;

                if (strSQL != null || strSQL != "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {

                            int PackingQty = Comon.cInt(Lip.SelectRecord("select min (PackingQty) from Stc_ItemUnits where ItemID=" + Comon.cLong(dt.Rows[i]["ItemID"].ToString())).Rows[0][0].ToString());

                            DataTable dtunit = Lip.SelectRecord("Select * from Stc_ItemUnits where ItemID= " + Comon.cLong(dt.Rows[i]["ItemID"].ToString()) + " and PackingQty=" + PackingQty);
                            decimal Qty = Comon.ConvertToDecimalQty(GetItemQty(UserInfo.FacilityID, UserInfo.BRANCHID, 0, Comon.cInt(dt.Rows[i]["ItemID"].ToString()), Comon.cInt(dtunit.Rows[0]["SizeID"].ToString()), dtunit.Rows[0]["Barcode"].ToString()));

                            
                            if (Qty < Comon.cDec(dtunit.Rows[0]["MinLimitQty"].ToString()))
                            {
                                dt.Columns["Qty"].ReadOnly = false;
                                dt.Columns["SizeID"].ReadOnly = false;

                                dt.Rows[i]["Qty"]=  Qty.ToString();
                                dt.Rows[i]["SizeID"] = dtunit.Rows[0]["SizeID"].ToString();
                                k++;
                            } 
                        }

                        if (k > 0)
                        {
                            gridControl1.DataSource = dt;
                            this.Show();
                        }
                        else
                            this.Hide();
                    }
                }

            }
            catch (Exception ex)
            {
                 

            }
        }

        decimal GetItemQty(int FacilityID, int BranchID, int StoreID, int ItemID, int SizeID, string BarCode, int MoveDate = 0)
        {
            try
            {

                Application.DoEvents();
                strSQL = "";
                string filter = "";
                filter = " ItemID >0  AND ";
                long ToDate = 0;
                if (ItemID > 0)
                    filter = " ItemID =" + ItemID + " AND ";


                filter = filter.Remove(filter.Length - 4, 4);
                strSQL = " SELECT * , CostPrice AS Price , 0 AS Total , dbo.RemindQtyStock(BarCode , " + StoreID + "," + ToDate + ") AS Qty from  Sales_BarCodeForPurchaseInvoiceArb_FindStock   where " + filter;

                strSQL += " ORDER BY ItemID, SizeID";




            }
            catch (Exception ex)
            {
               

            }

            decimal qty = 0;
            DataTable dtstcitem = Lip.SelectRecord(strSQL);
            for (int i = 0; i <= dtstcitem.Rows.Count - 1; i++)
            {
                decimal RemindQty = Comon.cDec(dtstcitem.Rows[i]["Qty1"].ToString());
                int PackingQty = Comon.cInt(dtstcitem.Rows[i]["PackingQty"].ToString());

                RemindQty = Comon.cDec(RemindQty * PackingQty);

                qty = Comon.cDec(qty + RemindQty);

            }
            decimal ItemQty = qty;
            return ItemQty;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    
    }
}
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
using DevExpress.XtraCharts;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model.Language;

namespace Edex.RestaurantSystem.Reports
{
    public partial class frmRushHours : DevExpress.XtraEditors.XtraForm
    {
        int PointsCount = 23;
        public string FocusedControl;
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }

            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }

            return c.Name;
        }
        public frmRushHours()
        {
            InitializeComponent();
            ///////////////////////////////////////////////////////
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtFromDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            // this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtToDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            // this.txtToDate.EditValue = DateTime.Now;
            ///////////////////////////////////////////////////////

            FillCombo.FillComboBox(cmbMethodID, "Sales_PurchaseMethods", "MethodID", "ArbName", "", "1=1");
            FillCombo.FillComboBox(cmbOrderType, "Res_OrderType", "ID", "ArbName", "", "1=1");

        }
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where BranchID=" + UserInfo.BRANCHID;

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)

                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "CustomerID", "اسـم الــعـــمـــيــل", "رقم الــعـــمـــيــل");
                else
                    PrepareSearchQuery.Search(txtCustomerID, lblCustomerName, "CustomerID", "Customer Name", "Customer ID");
            }


            if (FocusedControl.Trim() == txtDeliveryID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtDeliveryID, lblDeliveryName, "DriverID", "اسم السائق", "رقم السائق");
                else
                    PrepareSearchQuery.Search(txtDeliveryID, lblDeliveryName, "DriverID", "Customer Name", "Customer ID");
            }





            //else if (FocusedControl.Trim() == txtStoreID.Name)
            //{
            //    if (UserInfo.Language == iLanguage.Arabic)
            //        //  PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسم الـمـســتـودع","رقم الـمـســتـودع");
            //        PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "اسـم الـمـســتـودع", "رقم الـمـســتـودع");
            //    else
            //        PrepareSearchQuery.Search(txtStoreID, lblStoreName, "StoreID", "Store Name", "Store ID");
            //}


            //else if (FocusedControl.Trim() == txtOldBarCode.Name)
            //{
            //    if (UserInfo.Language == iLanguage.Arabic)
            //        PrepareSearchQuery.Search(txtOldBarCode, lblBarCodeName, "BarCodeForSalesInvoice", "اسـم الـمـادة", "البـاركـود");
            //    else
            //        PrepareSearchQuery.Search(txtOldBarCode, lblBarCodeName, "BarCodeForSalesInvoice", "Item Name", "BarCode");
            //}

            else if (FocusedControl.Trim() == txtSellerID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Search(txtSellerID, lblSellerName, "UserID", "اسم المستخدم", "رقم المستخدم");
                else
                    PrepareSearchQuery.Search(txtSellerID, lblSellerName, "SellerID", "Seller Name", "Seller ID");
            }
            //else if (FocusedControl.Trim() == txtCostCenterID.Name)
            //{
            //    if (UserInfo.Language == iLanguage.Arabic)
            //        PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "اسم مركز التكلفة", "رقم مركز التكلفة");
            //    else
            //        PrepareSearchQuery.Search(txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center Name", "Cost Center ID");
            //}

            //else if (FocusedControl.Trim() == txtSalesDelegateID.Name)
            //{
            //    if (UserInfo.Language == iLanguage.Arabic)
            //        PrepareSearchQuery.Search(txtSalesDelegateID, lblSalesDelegateName, "SaleDelegateID", "اسـم مندوب المبيعات", "رقم مندوب المبيعات");
            //    else
            //        PrepareSearchQuery.Search(txtSalesDelegateID, lblSalesDelegateName, "SaleDelegateID", "Delegate Name", "Delegate ID");

            //}







        }
        private void FrmRushHours_Load(object sender, EventArgs e)
        {
            
        }

        private int  GetSumInvoiceByTime(string p1, string p2)
        {
             long FromDate = Comon.cLong(Comon.ConvertDateToSerial(txtFromDate.Text));
                long ToDate = Comon.cLong(Comon.ConvertDateToSerial(txtToDate.Text));
            int sum=0;
            var sr = "Select Count (InvoiceID ) from Sales_SalesInvoiceMaster where Cancel=0 and  RegTime>=" + p1 + " and  RegTime<=" + p2;
            if (FromDate != 0)
                sr = sr + " and  .Sales_SalesReservationsMaster.InvoiceDate >=" + FromDate + "  ";

            if (ToDate != 0)
                sr = sr + " and  .Sales_SalesReservationsMaster.InvoiceDate <=" + ToDate + "  ";

            if (txtDeliveryID.Text != string.Empty)
                sr = sr + "and  .Sales_SalesInvoiceMaster.DeliveryID  =" + Comon.cInt(txtDeliveryID.Text) + "   ";

            if (txtCustomerID.Text != string.Empty)
                sr = sr + " AND .Sales_SalesInvoiceMaster.CustomerID  =" + Comon.cLong(txtCustomerID.Text) + "   ";

            if (txtSellerID.Text != string.Empty)
                sr = sr + "  AND .Sales_SalesInvoiceMaster.UserID  =" + Comon.cInt(txtSellerID.Text) + "   ";
            if (cmbOrderType.Text != string.Empty && Comon.cInt(cmbOrderType.EditValue) != 0)
                sr = sr + "AND  Sales_SalesInvoiceMaster.OrderType =" + cmbOrderType.EditValue + "  ";
            if (cmbMethodID.Text != string.Empty && Comon.cInt(cmbMethodID.EditValue) != 0)
                sr = sr + "  AND Sales_SalesInvoiceMaster.MethodeID =" + cmbMethodID.EditValue + "  ";
            var dt = Lip.SelectRecord(sr);
            if (dt.Rows.Count>0)sum=Comon.cInt(dt.Rows[0][0].ToString());
                return sum;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            chartControl1.Series[0].Points.Clear();
            chartControl1.DataSource = null;
            SeriesPoint[] points = new SeriesPoint[PointsCount];
            string[] s = new string[] { "01:00 AM","01:00 AM", "02:00 AM", "03:00 AM", "04:00 AM", "05:00 AM", "06:00 AM", "07:00 AM", "08:00 AM", "09:00 AM", "10:00 AM", "11:00 AM", "12:00 PM", "13:00 PM", "14:00 PM", "15:00 PM", "16:00 PM", "17:00 PM", "18:00 PM", "19:00 PM", "20:00 PM", "21:00 PM", "22:00 PM", "23:00 PM", "24:00 PM" };
            for (int i = 0; i <= PointsCount-1; i++)
            {
            //   var s1 = s[i];
                if (i ==0)
                    points[i] = new SeriesPoint(i, GetSumInvoiceByTime("0", "59"));

                else
               points[i] = new SeriesPoint(i, GetSumInvoiceByTime(i + "00", i + "59"));
            }
            chartControl1.Series[0].Points.AddRange(points);
        }

        private void frmRushHours_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
        }
    }
}
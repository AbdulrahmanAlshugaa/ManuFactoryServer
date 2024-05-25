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
using Edex.ModelSystem;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class frmAddressCustomer : DevExpress.XtraEditors.XtraForm
    {
        public frmAddressCustomer()
        {
            InitializeComponent();
            //Common.filllookupEDit(ref repositoryItemLookUpEdit1, "ID", "HR_District", "ArbName", "Cancel=0");

        }

        public frmAddressCustomer(string customerID, string customerName)
        {
            InitializeComponent();
            //Common.filllookupEDit(ref repositoryItemLookUpEdit1, "ID", "HR_District", "ArbName", "Cancel=0");
            lblCustomerName.Text = customerName;
            var sr = "Select  Sales_CustomersAddress.* ,HR_District.TransCost from Sales_CustomersAddress   inner join HR_District on HR_District.ID=Sales_CustomersAddress.Location         where Sales_CustomersAddress.CustomerID= " + customerID;
            var dt = Lip.SelectRecord(sr);
            gridControlAddress.DataSource = dt;

        }


        private void frmAddressCustomer_Load(object sender, EventArgs e)
        {

        }

        private void gridViewAddress_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
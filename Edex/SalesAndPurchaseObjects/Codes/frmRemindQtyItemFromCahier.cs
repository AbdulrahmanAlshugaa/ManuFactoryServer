using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Transactions
{
    public partial class frmRemindQtyItemFromCahier : Form
    {
        public frmRemindQtyItemFromCahier()
        {
            InitializeComponent();

            FillCombo.FillComboBox(cmbUnits, "Stc_SizingUnits", "SizeID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
             
        }

        public string GetDateExpire()
        { 
            if (string.IsNullOrEmpty(cmbUnits.Text))
               return   "";
             
            return txtBarCode.Text;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }
         
        private void frmAddNewItemFromCahier_Load(object sender, EventArgs e)
        {
          
            double rqTY = Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQtyStock]('" + txtBarCode.Text + "'," + Comon.cInt(MySession.GlobalDefaultSaleStoreID) + ",0) AS RemainQty")).Rows[0]["RemainQty"].ToString());
            txtSalePrice.Text = rqTY.ToString();
          
        }

        private void txtSalePrice_Validating(object sender, CancelEventArgs e)
        { 
            
        }

        private void txtBarCode_Validating(object sender, CancelEventArgs e)
        {
            double rqTY = Comon.cDbl((Lip.SelectRecord("SELECT [dbo].[RemindQtyStock]('" + txtBarCode.Text + "'," + Comon.cInt(MySession.GlobalDefaultSaleStoreID) + ",0) AS RemainQty")).Rows[0]["RemainQty"].ToString());
            txtSalePrice.Text = rqTY.ToString();
        }

        private void frmRemindQtyItemFromCahier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

    }
}

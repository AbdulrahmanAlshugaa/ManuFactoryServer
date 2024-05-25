using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edex.Model;
using Edex.ModelSystem;
using Edex.Model.Language;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class ctCloseCashier : UserControl
    {
        string TableName = "SalesCashierClose";
        string PremaryKey = "CloseCashierID";

        public ctCloseCashier()
        {
            InitializeComponent();
            if (UserInfo.Language == iLanguage.English) {

                labelControl4.Text = labelControl4.Tag.ToString();
                labelControl1.Text = labelControl1.Tag.ToString();
                simpleButton1.Text = simpleButton1.Tag.ToString();
                btnClose.Text = btnClose.Tag.ToString();
            
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            decimal CloseCashierID = Comon.ConvertToDecimalPrice(txtEnterCost.Text);

            decimal CloseCashierDate = Comon.ConvertToDecimalPrice(txtPervoiusCash.Text);
            simpleButton1.Tag = CloseCashierID.ToString() + "-" + CloseCashierDate.ToString();
            //SalesCashierClose model = new SalesCashierClose();
            //model.CloseCashierID = GetNewID();
            //model.CloseCashierDate = Comon.cLong((Lip.GetServerDateSerial()));
            //model.EnterCost = Comon.ConvertToDecimalPrice(txtEnterCost.Text);
            //model.UserID = UserInfo.ID;

        }

        public int GetNewID()
        {
            try
            {
                DataTable dt;
                string strSQL;
                strSQL = "SELECT Max(" + PremaryKey + ") + 1 FROM " + TableName;
                dt = Lip.SelectRecord(strSQL);
                string GetNewID = dt.Rows[0][0] == DBNull.Value ? "1" : dt.Rows[0][0].ToString();
                return Convert.ToInt32(GetNewID);

            }
            catch (Exception ex)
            {
                return 0;
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
    }
}

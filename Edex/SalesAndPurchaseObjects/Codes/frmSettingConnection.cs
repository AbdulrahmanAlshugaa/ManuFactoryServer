using Edex.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.SalesAndPurchaseObjects.Codes
{
    public partial class frmSettingConnection : Form
    {
        public frmSettingConnection()
        {
            InitializeComponent();
        }

        private void btnUsengGold_Click(object sender, EventArgs e)
        {
            String DevUserName = cConnectionString.UserName;
            String DevPassWord = cConnectionString.PassWordtxt;
            String DevDB = cConnectionString.DataBasename;
            String DevServer = cConnectionString.ServerName;
            cConnectionString.UserName = txtUserName.Text;
            cConnectionString.PassWordtxt = txtPassword.Text;
            cConnectionString.DataBasename = txtDbName.Text;
            cConnectionString.ServerName = txtServer.Text;
        }
    }
}

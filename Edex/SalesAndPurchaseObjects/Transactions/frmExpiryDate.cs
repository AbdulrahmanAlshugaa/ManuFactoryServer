using Edex.Model;
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
    public partial class frmExpiryDate : Form
    {
        public frmExpiryDate()
        {
            InitializeComponent();
            this.txtExpiryDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtExpiryDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtExpiryDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtExpiryDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtExpiryDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtExpiryDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtExpiryDate.EditValue = DateTime.Now;

        }
    
    public long GetDateExpire(){

        if (string.IsNullOrEmpty(txtExpiryDate.Text))
            return -1;

        return Comon.cLong(Comon.ConvertDateToSerial(txtExpiryDate.Text));
        

}

    private  void btnShow_Click(object sender, EventArgs e)
    {
        GetDateExpire();
        this.Close();
    }
   
}
}

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

namespace Edex.GeneralObjects.GeneralUserControls
{
    public partial class ucClockCashier : UserControl
    {
        public ucClockCashier()
        {
            InitializeComponent();
            this.txtToCloseDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToCloseDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToCloseDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToCloseDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToCloseDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToCloseDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.txtToCloseDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtToCloseDate.EditValue = DateTime.Now;
            txtUserName.Text =UserInfo.SYSUSERARBNAME;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
       // lblHours.Text = DateTime.Now.ToString("ss");
            this.txtToCloseDate.EditValue = DateTime.Now;
            lblHours.Text = txtToCloseDate.DateTime.ToString("hh:mm:ss tt");
            lblDate.Text = txtToCloseDate.Text;
       
        
        }
    }
}

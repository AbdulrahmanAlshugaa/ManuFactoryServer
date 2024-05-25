using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.BarCodes;
using System.Drawing;
using Edex.ModelSystem;
namespace Edex.StockObjects.Codes
{
    public partial class frmQrcodeGenreate : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public frmQrcodeGenreate()
        {
            InitializeComponent();
        }

        private void frmQrcodeGenreate_Load(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        { 
            this.pictureBox1.Image =Common.GenratCod(txtNotes.Text);
        }

         


    }
}

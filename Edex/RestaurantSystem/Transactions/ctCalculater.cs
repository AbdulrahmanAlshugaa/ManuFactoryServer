using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class ctCalculater : UserControl
    {
      public  string strQty = "";
        public ctCalculater()
        {
            InitializeComponent();
            pnlCalcuate.Visible = true;
            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            this.btnOne.Click += new System.EventHandler(this.btnOne_Click);
            this.btnTwo.Click += new System.EventHandler(this.btnTow_Click);
            this.btnThree.Click += new System.EventHandler(this.btnThree_Click);
            this.btnFour.Click += new System.EventHandler(this.btnFour_Click);
            this.btnFive.Click += new System.EventHandler(this.btnFive_Click);
            this.btnSix.Click += new System.EventHandler(this.btnSix_Click);
            this.btnSeven.Click += new System.EventHandler(this.btnSeven_Click);
            this.btnEight.Click += new System.EventHandler(this.btnEight_Click);
            this.btnNine.Click += new System.EventHandler(this.btnNine_Click);
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
        }

        private void pnlCalcuate_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnPlus_Click(object sender, EventArgs e)
        {
            btnPlus.Tag = txtTotal.Text.Trim();

          
        }
        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (strQty.Length < 1) return;
            strQty = strQty.Remove(strQty.Length-1, 1);
            txtTotal.Text = strQty;
        }
        private void btnNine_Click(object sender, EventArgs e)
        {
            strQty = strQty + "9";
            txtTotal.Text = strQty;
         //   setValueToField("9");
        }
        private void btnEight_Click(object sender, EventArgs e)
        {
            strQty = strQty + "8";
            txtTotal.Text = strQty;
        }
        private void btnSeven_Click(object sender, EventArgs e)
        {
            strQty = strQty + "7";
            txtTotal.Text = strQty;
        }
        private void btnThree_Click(object sender, EventArgs e)
        {
            strQty = strQty + "3";
            txtTotal.Text = strQty;

        }
        private void btnFour_Click(object sender, EventArgs e)
        {
            strQty = strQty + "4";
            txtTotal.Text = strQty;
        }
        private void btnFive_Click(object sender, EventArgs e)
        {
            strQty = strQty + "5";
            txtTotal.Text = strQty;
        }
        private void btnSix_Click(object sender, EventArgs e)
        {
            strQty = strQty + "6";
            txtTotal.Text = strQty;
        }
        private void btnTow_Click(object sender, EventArgs e)
        {
            strQty = strQty + "2";
            txtTotal.Text = strQty;
        }
        private void btnOne_Click(object sender, EventArgs e)
        {
            strQty = strQty + "1";
            txtTotal.Text = strQty;
        }
        private void btnZero_Click(object sender, EventArgs e)
        {
            strQty = strQty + "0";
            txtTotal.Text = strQty;
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Customization;
using Edex.Model;
using Edex.ModelSystem;
using Edex.TimeStaffScreens;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class confirmSaving : UserControl
    {
        ctAddCustomers ctCustomers = new ctAddCustomers();
        frmNewPos frmPos;
        XtraForm2 frm = new XtraForm2();
        public string languagename = "";
        public string MethodName = "";
        public int MethodID = 1;
        public int cmbMethodID = 1;
        string strQty = "";
     
       Sales_SalesInvoiceMaster objRecord = new Sales_SalesInvoiceMaster();

        public confirmSaving()
        {
            InitializeComponent();
            if (UserInfo.Language == iLanguage.English)
                languagename = "EngName";
            else
                languagename = "ArbName";
            FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", languagename, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

            pnlDeliverContol.Visible = true;
        }
        public confirmSaving(String lblNetBalance, String lblBalanceSum, String lblRemaindBalance,String customName,String CustID,String mobile,Color bgColor)
        {
            InitializeComponent();
            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            this.btnOne.Click += new System.EventHandler(this.btnOne_Click);
            this.btnTwo.Click += new System.EventHandler(this.btnTow_Click);
            this.btnThree.Click += new System.EventHandler(this.btnThree_Click);
            this.btnFour.Click += new System.EventHandler(this.btnFour_Click);
            this.btnFive.Click += new System.EventHandler(this.btnFive_Click);
            this.btnSix.Click += new System.EventHandler(this.btnSix_Click);
            this.btnSeven.Click += new System.EventHandler(this.btnSeven_Click);
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);

            this.btnEight.Click += new System.EventHandler(this.btnEight_Click);
            this.btnNine.Click += new System.EventHandler(this.btnNine_Click);
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            if (UserInfo.Language == iLanguage.English)
                languagename = "EngName";
            else
                languagename = "ArbName";
            strQty = "";
            FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", languagename, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            this.txtCustomerID.Text = CustID;
            this.lblCustomerName.Text = customName;
            this.txtMobile.Text = mobile;
            this.lblNetBalance.Text = lblNetBalance;
            this.lblBalanceSum.Text = lblBalanceSum;
            this.lblBalanceSum.BackColor = bgColor;
            this.lblRemaindBalance.Text = lblRemaindBalance;
            pnlDeliverContol.Visible = true;

        }
        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnTwo_Click(object sender, EventArgs e)
        {
            
        }
        private void simpleButton11_Click(object sender, EventArgs e)
        {
            ctCustomers = new ctAddCustomers();
            ctCustomers.simpleButton1.Click += simpleButton1111_Click;
            ctCustomers.btnClose.Click += simpleButton11111_Click;
            FlyoutAction action = new FlyoutAction();

            FlyoutProperties properties = new FlyoutProperties();

            properties.Style = FlyoutStyle.Popup;

            FlyoutDialog.Show(this.ParentForm, ctCustomers, action, properties);
        }
        private void simpleButton1111_Click(object sender, EventArgs e)
        {

            txtCustomerID.Text = ctCustomers.txtCustomerID.Text;
            lblCustomerName.Text = ctCustomers.txtArbName.Text;
            //  txtCustomerID_Validating(null, null);
            txtAddressID.Text = ctCustomers.CustomerNo.ToString();
            // txtAddressID_Validating(null, null);
            txtAddressID_Validating(null, null);


        }
        private void txtAddressID_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                var sr = "SELECT       Sales_Customers.CustomerID, Sales_Customers.ArbName, Sales_Customers.Tel, Sales_Customers.Mobile, Sales_CustomersAddress.ID, Sales_CustomersAddress.Location, Sales_CustomersAddress.Street, "
                    + "     Sales_CustomersAddress.Building, Sales_CustomersAddress.ArbName as Notes, Sales_CustomersAddress.Floor, Sales_CustomersAddress.Apartment, HR_District.ArbName AS DistrictName, HR_Street.ArbName AS StreetName, HR_District.TransCost"
  + "  FROM            HR_District RIGHT OUTER JOIN"
        + "                     Sales_CustomersAddress ON HR_District.ID = Sales_CustomersAddress.Location LEFT OUTER JOIN"
             + "                HR_Street ON Sales_CustomersAddress.Street = HR_Street.ID RIGHT OUTER JOIN"
               + "              Sales_Customers ON Sales_CustomersAddress.CustomerID = Sales_Customers.CustomerID  where Sales_Customers.Cancel=0 And  Sales_CustomersAddress.ID=" + Comon.cInt(txtAddressID.Text.Trim());
                var dr = Lip.SelectRecord(sr);
                if (dr.Rows.Count > 0)
                {
                    decimal cost = Comon.ConvertToDecimalPrice(dr.Rows[0]["TransCost"].ToString());
                    lblAddressCustomerName.Text = dr.Rows[0]["DistrictName"].ToString() + "-" + dr.Rows[0]["StreetName"].ToString() + "-" + dr.Rows[0]["Notes"].ToString();
                    txtCustomerID.Text = dr.Rows[0]["CustomerID"].ToString();
                    lblCustomerName.Text = dr.Rows[0]["ArbName"].ToString();
                    txtFloor.Text = dr.Rows[0]["Floor"].ToString();
                    txtApartment.Text = dr.Rows[0]["Apartment"].ToString();
                    txtBuilding.Text = dr.Rows[0]["Building"].ToString();
                    txtMobile.Text = "Tel:" + dr.Rows[0]["Tel"].ToString() + "-Mob:" + dr.Rows[0]["Mobile"].ToString();
                    var sr1 = "SELECT        Stc_ItemUnits.BarCode"
+ " FROM   Stc_ItemUnits LEFT OUTER JOIN"
                   + "    Stc_SizingUnits ON Stc_ItemUnits.SizeID = Stc_SizingUnits.SizeID"
+ "   WHERE        (Stc_SizingUnits.Notes = '0')";
                    var dt = Lip.SelectRecord(sr1);
                    if (dt.Rows.Count > 0)
                    {


                        //frmPos.btnCilick1(dt.Rows[0][0].ToString(), cost);
                        //frmPos.CalculateRow();




                    }

                }
                else
                {

                    lblAddressCustomerName.Text = "";
                    txtCustomerID.Text = "";
                    lblCustomerName.Text = "";
                    txtFloor.Text = "";
                    txtApartment.Text = "";
                    txtBuilding.Text = "";
                    txtMobile.Text = "";
                    txtAddressID.Text = "";

                }
                //CalculateRow();



            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void simpleButton11111_Click(object sender, EventArgs e)
        {

            
            SendKeys.Send("{ESC}");
        }
        private void simpleButton10_Click(object sender, EventArgs e)
        {
            frm = new XtraForm2();
            frm.simpleButton2.Click += AddAddress1_Click;
            frm.ShowDialog();
        }
        private void AddAddress1_Click(object sender, EventArgs e)
        {
            txtAddressID.Text = frm.ID.ToString();
            txtAddressID_Validating(null, null);
        }
        private void panelControl3_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnCash_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtNetProcessID.Text = " ";
            // cmbNetType.Text = " ";
            txtNetAmount.Text = " ";
            btnCash.Appearance.BorderColor = Color.FromArgb(83, 68, 63);
          


            btnCash.Appearance.BackColor = Color.Goldenrod;
            btnCash.Appearance.BackColor2 = Color.White;
            btnCash.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            btnCash.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            btnNet.Appearance.BackColor = Color.White;
            btnNet.Appearance.BackColor2 = Color.White;
            btnCash_Net.Appearance.BackColor = Color.White;
            btnCash_Net.Appearance.BackColor2 = Color.White;
          





            labelControl6.Visible = true;
            cmbMethodID = 1;
          
         
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "نقدا" : "Cash");
            MethodID = 1;

          

        }
        private void btnNet_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
           
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtNetProcessID.Text = " ";
            txtNetAmount.Text = " ";
            cmbNetType.EditValue = 0;
            cmbMethodID = 3;
          
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة" : "Net");
            MethodID = 2;
            btnNet.Appearance.BackColor = Color.Goldenrod;
            btnNet.Appearance.BackColor2 = Color.White;
            btnNet.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            btnNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            btnCash.Appearance.BackColor = Color.White;
            btnCash.Appearance.BackColor2 = Color.White;
            btnCash_Net.Appearance.BackColor = Color.White;
            btnCash_Net.Appearance.BackColor2 = Color.White;

          
            
        }
        private void btnCash_Net_Click(object sender, EventArgs e)
        {
            /////////////////////////////
            txtCustomerID.Tag = " ";
            txtNetProcessID.Tag = " ";
         
            cmbNetType.Tag = " ";
            txtNetAmount.Tag = " ";
            txtNetProcessID.Text = " ";
          
            
            txtNetAmount.Text = " ";
            txtNetAmount.Tag = "";

       
            cmbMethodID= 5;
          
          
            
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة/ نقد" : "Net/Cash");
            MethodID = 3;

            btnCash_Net.Appearance.BackColor = Color.Goldenrod;
            btnCash_Net.Appearance.BackColor2 = Color.White;
            btnCash_Net.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            btnCash.Appearance.BackColor = Color.White;
            btnCash.Appearance.BackColor2 = Color.White;
            btnNet.Appearance.BackColor = Color.White;
            btnNet.Appearance.BackColor2 = Color.White;
         
          
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Comon.ConvertToDecimalPrice(txtNetAmount.Text) <= 0 && cmbMethodID == 5)
            {
                txtNetAmount.Focus();
                txtNetAmount.ToolTip = "مبلغ الشبكة = 0 ";
                Validations.ErrorText(txtNetAmount, txtNetAmount.ToolTip);
                return;

            }
            if (Comon.ConvertToDecimalPrice(txtCustomePaidAmount.Text) < 0 )
            {
                txtCustomePaidAmount.Focus();
                txtCustomePaidAmount.ToolTip = "  القيمة اقل من الصفر ";
                Validations.ErrorText(txtCustomePaidAmount, txtCustomePaidAmount.ToolTip);
                return;

            }
          

            
         
            SendKeys.Send("{ESC}");
        }

        private void panelControl6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
        }

        #region Calc

        private void btnPlus_Click(object sender, EventArgs e)
        {
          
            if (strQty.Length < 1) return;
            strQty = strQty.Remove(strQty.Length - 1, 1);
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (strQty.Length < 1) return;
            strQty = strQty.Remove(strQty.Length - 1, 1);
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnNine_Click(object sender, EventArgs e)
        {
            strQty = strQty + "9";
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnEight_Click(object sender, EventArgs e)
        {
            strQty = strQty + "8";
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnSeven_Click(object sender, EventArgs e)
        {
            strQty = strQty + "7";
            txtCustomePaidAmount.Text = strQty;
        }

        private void btnDot_Click(object sender, System.EventArgs e)
        {
            strQty = strQty + ".";
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnThree_Click(object sender, EventArgs e)
        {
            strQty = strQty + "3";
            txtCustomePaidAmount.Text = strQty;

        }
        private void btnFour_Click(object sender, EventArgs e)
        {
            strQty = strQty + "4";
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnFive_Click(object sender, EventArgs e)
        {
            strQty = strQty + "5";
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnSix_Click(object sender, EventArgs e)
        {
            strQty = strQty + "6";
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnTow_Click(object sender, EventArgs e)
        {
            strQty = strQty + "2";
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnOne_Click(object sender, EventArgs e)
        {
            strQty = strQty + "1";
            txtCustomePaidAmount.Text = strQty;
        }
        private void btnZero_Click(object sender, EventArgs e)
        {
            strQty = strQty + "0";
            txtCustomePaidAmount.Text = strQty;
        }




        #endregion




    }
}

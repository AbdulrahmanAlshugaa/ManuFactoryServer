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
using Edex.AccountsObjects.Reports;
using Edex.DAL.Accounting;
using Edex.DAL;
using System.IO;

namespace Edex.RestaurantSystem.Transactions
{
    public partial class confirmSavingReturn : UserControl
    {
        ctAddCustomers ctCustomers = new ctAddCustomers();
        frmNewPos frmPos;
        DataTable dtDeclaration;
        XtraForm2 frm = new XtraForm2();
        public string languagename = "";
        public string MethodName = "";
        public int MethodID = 1;
        public int cmbMethodID = 1;
        public string invoiceID = "";
        public double CustomerAccount=0;
             string strQty = "";

       Sales_SalesInvoiceMaster objRecord = new Sales_SalesInvoiceMaster();

        public confirmSavingReturn()
        {
            InitializeComponent();
            if (UserInfo.Language == iLanguage.English)
                languagename = "EngName";
            else
                languagename = "ArbName";
            FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", languagename, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));

            pnlDeliverContol.Visible = true;
        }
        public confirmSavingReturn(string invoiceID, string netBalance, string insurance, string Account)
        {
            InitializeComponent();
            strQty = "";
            dtDeclaration = new Acc_DeclaringMainAccountsDAL().GetAcc_DeclaringMainAccounts(MySession.GlobalBranchID, MySession.GlobalFacilityID);
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
            FillCombo.FillComboBox(cmbNetType, "NetType", "NetTypeID", languagename, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            CustomerAccount=Comon.cDbl(Account);
            lblNetBalance.Text = netBalance;
            lblRemaindBalance.Text = insurance;
            this.invoiceID = invoiceID;
            if (Comon.cLong(Account) > 0)
            {

               
                frmAccountStatement frm = new frmAccountStatement(Comon.cLong(Account));
                lblBalanceSum.Text = frm.lblBalanceSum.Text;
                lblBalanceSum.BackColor = Color.Transparent;
                lblRequireAmmut.Text = "0";
                //decimal saleNOOK = 0;

                //try
                //{
                //    var ss = "select sum (isnull(InsuranceAmmount,0)) from Sales_SalesInvoiceMaster where isDone=1 or isDone IS NULL";

                //    saleNOOK = Comon.ConvertToDecimalPrice(Lip.GetValue(ss));


                //}
                //catch
                //{ 


                
                // var Total=(Comon.ConvertToDecimalPrice(frm.lblDebit.Text) - Comon.ConvertToDecimalPrice(frm.lblCredit.Text))*-1+saleNOOK;






                if (Comon.cDec(frm.lblDebit.Text) > Comon.cDec(frm.lblCredit.Text))
                {

                    lblBalanceSum.BackColor = Color.Red;
                    lblRequireAmmut.Text = (Comon.ConvertToDecimalPrice(frm.lblDebit.Text) - Comon.ConvertToDecimalPrice(frm.lblCredit.Text)).ToString();


                }



              


            }


            try
            {
                var sr = "Select Sales_SalesInvoiceDetails.Qty as QTY, Concat( Stc_SizingUnits." + languagename + ",' ', Stc_Items." + languagename + ",CHAR(13) ,Sales_SalesInvoiceDetails.Serials)as ItemName"
                 + " FROM        Stc_SizingUnits LEFT OUTER JOIN"
     + "                     Sales_SalesInvoiceDetails ON Stc_SizingUnits.SizeID = Sales_SalesInvoiceDetails.SizeID LEFT OUTER JOIN"
     + "                  Stc_Items ON Sales_SalesInvoiceDetails.ItemID = Stc_Items.ItemID    "
     + " 			 where    Sales_SalesInvoiceDetails.InvoiceID= " + invoiceID;

                var dr = Lip.SelectRecord(sr);
                gridControl1.DataSource = dr;
            }
            catch { }


           

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
            btnNet.Appearance.BackColor = Color.Goldenrod;
            btnNet.Appearance.BackColor2 = Color.White;
            btnNet.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            btnNet.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            btnCash.Appearance.BackColor = Color.White;
            btnCash.Appearance.BackColor2 = Color.White;
            btnCash_Net.Appearance.BackColor = Color.White;
            btnCash_Net.Appearance.BackColor2 = Color.White;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة" : "Net");
            MethodID = 2;
          
         

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

            btnCash_Net.Appearance.BackColor = Color.Goldenrod;
            btnCash_Net.Appearance.BackColor2 = Color.White;
            btnCash_Net.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            btnCash_Net.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            btnCash.Appearance.BackColor = Color.White;
            btnCash.Appearance.BackColor2 = Color.White;
            btnNet.Appearance.BackColor = Color.White;
            btnNet.Appearance.BackColor2 = Color.White;
            MethodName = (UserInfo.Language == iLanguage.Arabic ? "شبكة/ نقد" : "Net/Cash");
            MethodID = 3;

           
          
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
            if (Comon.ConvertToDecimalPrice(txtCustomePaidAmount.Text) < 0)
            {
                txtCustomePaidAmount.Focus();
                txtCustomePaidAmount.ToolTip = "  القيمة اقل من الصفر ";
                Validations.ErrorText(txtCustomePaidAmount, txtCustomePaidAmount.ToolTip);
                return;

            
            }
           
            int DocumentID = 0;
            int RegistrationNo = 0;
            if (Comon.cDbl(txtCustomePaidAmount.Text) > 0)
            {
                switch (MethodID)
                {
                    case (1):
                        DocumentID = SaveRecipt(Comon.cDbl(txtCustomePaidAmount.Text), CustomerAccount);
                        RegistrationNo = 0;
                        break;
                    case (3):
                        DocumentID = SaveRecipt(Comon.cDbl(txtCustomePaidAmount.Text) - Comon.cDbl(txtNetAmount.Text), CustomerAccount);
                        RegistrationNo = SaveVouchers1(Comon.cDbl(txtNetAmount.Text), CustomerAccount);
                        break;
                    case (2):
                        DocumentID = 0;
                        RegistrationNo = SaveVouchers1(Comon.cDbl(txtCustomePaidAmount.Text), CustomerAccount);
                        break;


                }

                //double disc = Math.Truncate(Comon.cDbl(txtCustomePaidAmount.Text) / 100);
                //if (disc > 0)
                //    SaveVouchersDiscount(disc * 30, CustomerAccount);
            }
                var sr = "Update Sales_SalesInvoiceMaster  set InsuranceAmmountAfter=" + Comon.cDbl(txtCustomePaidAmount.Text) + "  ,isDone=1 ,EditUserID=" + UserInfo.ID + " ,EditDate=" + Lip.GetServerDateSerial() + ",EditTime=" + Comon.cDbl(Lip.GetServerTimeSerial()) + "  where InvoiceID= " + invoiceID;
                Lip.ExecututeSQL(sr);

                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
             
                SendKeys.Send("{ESC}");

            
        }

        private byte[] DefaultImage()
        {
            try
            {
                string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                Path = Path + @"\Images\379338-48.png";
                System.Drawing.Image img = System.Drawing.Image.FromFile(Path);
                MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
            catch { return null; }

        }

        private int SaveRecipt(double CreditAmount, double AccountID)
        {

            Acc_ReceiptVoucherMaster objRecord = new Acc_ReceiptVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.ReceiptVoucherDate = Lip.GetServerDateSerial(); ;
            
            objRecord.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);

            objRecord.RegistrationNo = 1;
            objRecord.DelegateID = 0;
            objRecord.Notes = "استلام مبلغ من عميل عن تسليم الفاتورة رقم : " + invoiceID;
            objRecord.DocumentID = 1;

            objRecord.Cancel = 0;

            objRecord.RegistrationNo = 1;
            objRecord.InvoiceID = 0;
            objRecord.DelegateID = 0;
            objRecord.OperationTypeName = (UserInfo.Language == iLanguage.English ? "Receipt Voucher" : "سند القبض ");
            objRecord.Notes = "استلام مبلغ من عميل عن تسليم الفاتورة رقم : " + invoiceID;
            objRecord.DocumentID = 1;
            DataRow[] row = dtDeclaration.Select("DeclareAccountName = 'MainBoxAccount'");
            if (row.Length > 0)
            {
                objRecord.DebitAccountID = Comon.cDbl(row[0]["AccountID"].ToString());

            }

            objRecord.DiscountAccountID = Comon.cDbl(dtDeclaration.Select("DeclareAccountName = 'GivenDiscountAccount'")[0]["AccountID"].ToString());
            //Ammount
            objRecord.DiscountAmount = Comon.cDbl(0);
            objRecord.DebitAmount = CreditAmount;

            objRecord.Cancel = 0;
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";

       
            objRecord.SpendImage = DefaultImage();


            Acc_ReceiptVoucherDetails returned;
            List<Acc_ReceiptVoucherDetails> listreturned = new List<Acc_ReceiptVoucherDetails>();

            returned = new Acc_ReceiptVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FACILITYID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            returned.ReceiptVoucherID = 0;
            returned.CreditAmount = Math.Abs(CreditAmount);
            returned.Discount = Comon.cDbl(0);
            returned.Declaration = "استلام مبلغ من عميل عن تسليم الفاتورة رقم : " + invoiceID;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            int Result = 0;

            if (listreturned.Count > 0)
            {
                objRecord.ReceiptVoucherDetails = listreturned;
               // Result = ReceiptVoucherDAL.InsertUsingXMLRecipt(objRecord, MySession.UserID);


             



            }
            else
            {
                
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }
            return Result;
        }
        private int SaveVouchers1(double CreditAmount, double AccountID)
        {
 
            DataRow[] row;
          
            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.VoucherDate = Lip.GetServerDateSerial(); ;

            objRecord.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);

            objRecord.RegistrationNo = 1;
            objRecord.DelegateID = 0;
            objRecord.Notes = "استلام مبلغ من عميل عن تسليم الفاتورة رقم : " + invoiceID;
            objRecord.DocumentID = 1;
     
            objRecord.Cancel = 0;
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";




            row = dtDeclaration.Select("DeclareAccountName = 'NetAccount'");
            if (row.Length < 1)
                return 0;


            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();



            returned = new Acc_VariousVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            // returned.AccountID = Comon.cDbl(txtCustomerID.Text); ;
            returned.VoucherID = 0;
            returned.Credit = CreditAmount;
            returned.Debit = 0;
            returned.Declaration = "استلام مبلغ من عميل عن تسليم الفاتورة رقم : " + invoiceID;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            returned = new Acc_VariousVoucherDetails();
            returned.ID = 1;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            returned.VoucherID = 0;
            returned.Credit = 0;
            returned.Debit = CreditAmount;
            returned.Declaration = "استلام مبلغ من عميل عن تسليم الفاتورة رقم : " + invoiceID;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            int Result = 0;
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
               // string Result1 = VariousVoucherDAL.InsertUsingXML(objRecord, MySession.UserID);
               // Result = Comon.cInt(Result1);
               



            }
            else
            {
               
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }
            return Result;
        }

          private int SaveVouchersDiscount(double CreditAmount, double AccountID)
        {
 
            DataRow[] row;
          
            Acc_VariousVoucherMaster objRecord = new Acc_VariousVoucherMaster();

            objRecord.BranchID = MySession.GlobalBranchID;
            objRecord.FacilityID = MySession.GlobalFacilityID;


            //Date
            objRecord.VoucherDate = Lip.GetServerDateSerial(); ;

            objRecord.CurrencyID = Comon.cInt(MySession.GlobalDefaultCurencyID);

            objRecord.RegistrationNo = 1;
            objRecord.DelegateID = 0;
            objRecord.Notes = "خصم مسمووح  لعميل عند تسليم الفاتورة رقم : " + invoiceID;
            objRecord.DocumentID = 1;
     
            objRecord.Cancel = 0;
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;

            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";




            row = dtDeclaration.Select("DeclareAccountName = 'GivenDiscountAccount'");
            if (row.Length < 1)
                return 0;
            Acc_VariousVoucherDetails returned;
            List<Acc_VariousVoucherDetails> listreturned = new List<Acc_VariousVoucherDetails>();
            returned = new Acc_VariousVoucherDetails();
            returned.ID = 0;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = AccountID;
            // returned.AccountID = Comon.cDbl(txtCustomerID.Text); ;
            returned.VoucherID = 0;
            returned.Credit = CreditAmount;
            returned.Debit = 0;
            returned.Declaration = "خصم مسمووح  لعميل عند تسليم الفاتورة رقم : " + invoiceID;
            returned.CostCenterID = 0;
            listreturned.Add(returned);
            returned = new Acc_VariousVoucherDetails();
            returned.ID = 1;
            returned.BranchID = UserInfo.BRANCHID;
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(row[0]["AccountID"].ToString());
            returned.VoucherID = 0;
            returned.Credit = 0;
            returned.Debit = CreditAmount;
            returned.Declaration = "خصم مسمووح  لعميل عند تسليم الفاتورة رقم : " + invoiceID;
            returned.CostCenterID = 0;

            listreturned.Add(returned);

            int Result = 0;
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
               // string Result1 = VariousVoucherDAL.InsertUsingXML(objRecord, MySession.UserID);
                //Result = Comon.cInt(Result1);
            }
            else
            {
               
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }
            return Result;
        }

          private void btnClose_Click(object sender, EventArgs e)
          {
            
              SendKeys.Send("{ESC}");

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

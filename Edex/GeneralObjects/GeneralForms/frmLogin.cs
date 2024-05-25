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
using System.Xml;
using Edex.Model;
using Edex.DAL.UsersManagement;
using System.Threading;
using Edex.DAL.Common;
using Edex.Model.Language;
using Edex.ModelSystem;
using Edex.DAL;
using DevExpress.XtraSplashScreen;
using Edex.DAL.Configuration;
using Edex.GeneralObjects.GeneralClasses;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using Microsoft.Win32;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        public long RegActive;
        static int DataBaseID = 0;
        static string DataBaseName = "";
        public CultureInfo culture = new CultureInfo("en-US");
        public frmLogin()
        {

            InitializeComponent();
            this.cmbDataBaseName.EditValueChanged += new System.EventHandler(this.cmbDataBaseName_EditValueChanged);
            GetDatabaseList();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            
            try
            {
                RegistrEdex_Load();
                var st = "SELECT  [Day],DayLife FROM [dbo].[Important] where ID=1";
                var dr = Lip.SelectRecord(st);
                var last = Comon.cLong(dr.Rows[0][0].ToString());
                var now = Comon.cLong((Lip.GetServerDateSerial()));
                var DayLife = Comon.cInt(dr.Rows[0][1].ToString());
                DateTime nowDate = DateTime.ParseExact(Comon.ConvertSerialDateTo(now.ToString()), "dd/MM/yyyy", culture);
                if (last > 0)
                {
                    DateTime lastDate = DateTime.ParseExact(Comon.ConvertSerialDateTo(last.ToString()), "dd/MM/yyyy", culture);
                    if (now < last)
                    {
                        XtraMessageBox.Show("يرجى ضبط التاريخ ");
                        Application.Exit();
                    }
                    if (Math.Abs((nowDate - lastDate).TotalDays) > DayLife)
                    {
                        XtraMessageBox.Show("انتهت  صلاحية النسخة  الرجاء الاتصال بالدعم الفني ");
                        Application.Exit();
                      
                    }
                }
                else
                {
                    Lip.NewFields();
                    Lip.Table = "Important";
                    Lip.AddNumericField("Day", Comon.cLong(now).ToString());
                    Lip.sCondition = "ID=1";
                    Lip.ExecuteUpdate();
                }


            }
            catch { }
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            defaultLookAndFeel1.LookAndFeel.SetSkinStyle(SystemSettings.GetSkinName(Path));
            cmbLangauage.SelectedIndex = 0;
             
        }

        private void cmbLangauage_SelectedIndexChanged(object sender, EventArgs e)
        {

            ComboBoxEdit obj = (ComboBoxEdit)sender;
            if (obj.SelectedIndex == 0)
            {
                if (MySession.GlobalLanguageName != iLanguage.Arabic)
                {
                    MySession.GlobalLanguageName = iLanguage.Arabic;
                    UserInfo.Language = iLanguage.Arabic;
                    ChangeLanguage.ArabicLanguage(this);
                }

            }
            else
            {
                if (MySession.GlobalLanguageName != iLanguage.English)
                {

                    MySession.GlobalLanguageName = iLanguage.English;
                    UserInfo.Language = iLanguage.English;
                    ChangeLanguage.EnglishLanguage(this);
                }

            }
        }
        private void btnlogin_Click(object sender, EventArgs e)
        {

            Messages.initialization(UserInfo.Language);

            if (cmbDataBaseName .EditValue !=null && Comon.cInt(cmbDataBaseName.EditValue.ToString()) == 0)
            {
                Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.English ? " you Shoud Select DataBase Name? " : "? يجب عليك اختيار قاعدة البيانات  "));
                return;
            }
            try
            {
               
                cConnectionString.DataBasename = cmbDataBaseName.Text;
                SplashScreenManager.CloseForm(false);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                UserBO ISVaild = LoginValidation.Login( txtUserName.Text.Trim(), txtPassword.Text.Trim(),MySession.GlobalBranchID);
                if (ISVaild != null)
                {
                    if (ISVaild.IsActiveAllowedDays == 1)
                    {
                        DateTime AllowedDate;
                        int NumberAllowedDays = ISVaild.NumberAllowedDays;
                        AllowedDate = Comon.cDate(ISVaild.AllowedDate.ToString());
                        int RemainDays = Convert.ToInt32(DateTime.Now.Subtract(AllowedDate).TotalDays);
                        if (NumberAllowedDays - RemainDays > 0)
                        {


                            frmMainEdex frm = new frmMainEdex();
                            frm.Show();
                            this.Hide();
                        }
                        else
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                XtraMessageBox.Show("تم إغلاق الحساب - لقد انتهت الفتره المجانيه", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            else
                                XtraMessageBox.Show("Account closed - The free period has expired", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        }
                    }
                    else
                    {
                        if (chkRememberMe.Checked)
                        {
                            
                            Properties.Settings.Default.Save();
                        }

                        /*******************************************/
                        MySession.GlobalBranchID = UserInfo.BRANCHID;
                        MySession.GlobalFacilityID= UserInfo.FacilityID ;
                        UserInfo.ComputerInfo = Environment.MachineName;
                       



                        bool[] arrbool = new bool[10];
                        MySession.GlobalAllowChangefrmPurchaseInvoiceDate = false;
                        arrbool[0] = MySession.GlobalAllowChangefrmPurchaseInvoiceDate;


                        string strSQL = "SELECT  [OtherPermissionName] ,[OtherPermissionValue],[OtherPermissionIndex] FROM [UserOtherPermissions] where  [FacilityID] =" + UserInfo.FacilityID + " and [UserID]=" + UserInfo.ID + " and [BranchID]=" + UserInfo.BRANCHID;
                        DataTable dtOtherPermissions = Lip.SelectRecord(strSQL);
                        foreach (DataRow row in dtOtherPermissions.Rows)
                        {
                            switch (row["OtherPermissionName"].ToString())
                            {
                                case "CostPriceType":
                                    MySession.PubCostPriceType = row["OtherPermissionIndex"].ToString();
                                    break;
                                case "SalePriceType":
                                    MySession.PubSalePriceType = row["OtherPermissionIndex"].ToString();
                                    break;
                                /************ Default **************/
                                case "DefaultStoreID":
                                    MySession.GlobalDefaultStoreID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultCostCenterID":
                                    MySession.GlobalDefaultCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSellerID":
                                    MySession.GlobalDefaultSellerID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultDebitAccountID":
                                    MySession.GlobalDefaultDebitAccountID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultCurencyID":
                                    MySession.GlobalDefaultCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSalesDelegateID":
                                    MySession.GlobalDefaultSalesDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                /************ Discount **************/
                                case "DiscountPercentOnItem":
                                    MySession.GlobalDiscountPercentOnItem = Comon.ConvertToDecimalPrice(row["OtherPermissionValue"].ToString());
                                    break;
                                case "DiscountPercentOnTotal":
                                    MySession.GlobalDiscountPercentOnTotal = Comon.ConvertToDecimalPrice(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ Setting  **************/
                                case "NumDecimalPlaces":
                                    MySession.GlobalNumDecimalPlaces = Comon.cInt(row["OtherPermissionValue"].ToString());
                                    break;
                                case "PercentVat":
                                    MySession.GlobalPercentVat = Comon.cInt(row["OtherPermissionValue"].ToString());
                                    break;
                                case "NoOfLevels":
                                    MySession.GlobalNoOfLevels = Comon.cInt(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ Can Change **************/
                                case "CanChangeDocumentsDate":
                                    MySession.GlobalCanChangeDocumentsDate = Comon.cInt(row["OtherPermissionIndex"].ToString()) == 1 ? true : false;
                                    break;
                                case "CanChangeInvoicePrice":
                                    MySession.GlobalCanChangeInvoicePrice = Comon.cInt(row["OtherPermissionIndex"].ToString())==1?true:false;
                                    break;
                                case "ShowItemQtyInSaleInvoice":
                                    MySession.GlobalShowItemQtyInSaleInvoice = Comon.cInt(row["OtherPermissionIndex"].ToString()) == 1 ? true : false;
                                    break;
                                case "CanDiscountOnCashierScreen":
                                    MySession.GlobalCanDiscountOnCashierScreen = Comon.cInt(row["OtherPermissionIndex"].ToString()) == 1 ? true : false;
                                    break;
                                case "CanCloseCashier":
                                    MySession.GlobalCanCloseCashier = Comon.cInt(row["OtherPermissionIndex"].ToString()) == 1 ? true : false;
                                    break;
                                case "CanChangePriceInCashierScreen":
                                    MySession.GlobalCanChangePriceInCashierScreen = Comon.cbool(row["OtherPermissionIndex"].ToString());
                                    break;
                                case "CanSearchItemInCashierScreen":
                                    MySession.GlobalCanSearchItemInCashierScreen = Comon.cbool(row["OtherPermissionIndex"].ToString());
                                    break;
                                case "CanSaleItemForZeroPrice":
                                    MySession.GlobalCanSaleItemForZeroPrice = Comon.cbool(row["OtherPermissionIndex"].ToString());
                                    break;
                                case "CanOpenCashierDrawerByF12":
                                    MySession.GlobalCanOpenCashierDrawerByF12 = Comon.cInt(row["OtherPermissionIndex"].ToString()) == 1 ? true : false;
                                    break;
                                /************ OpenPopup **************/
                                case "AllowWhenEnterOpenPopup":
                                    MySession.GlobalAllowWhenEnterOpenPopup = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowWhenClickOpenPopup":
                                    MySession.GlobalAllowWhenClickOpenPopup = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;

                                /******************************* Purchase ********************************/
                                /************ Purchase Allow **************/
                                case "AllowChangefrmPurchaseInvoiceDate":
                                    MySession.GlobalAllowChangefrmPurchaseInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseStoreID":
                                    MySession.GlobalAllowChangefrmPurchaseStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseCostCenterID":
                                    MySession.GlobalAllowChangefrmPurchaseCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchasePayMethodID":
                                    MySession.GlobalAllowChangefrmPurchasePayMethodID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseCurencyID":
                                    MySession.GlobalAllowChangefrmPurchaseCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseNetTypeID":
                                    MySession.GlobalAllowChangefrmPurchaseNetTypeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseDelegateID":
                                    MySession.GlobalAllowChangefrmPurchaseDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseDebitAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseCreditAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseAdditionalAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseAdditionalAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseDiscountCreditAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseDiscountCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseChequeAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseChequeAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseNetAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseNetAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseTransportDebitAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseTransportDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseSupplierID":
                                    MySession.GlobalAllowChangefrmPurchaseSupplierID = Comon.cbool(row["OtherPermissionValue"]);
                                    break;
                                case "AllowChangefrmPurchaseInvoiceNetPrice":
                                    MySession.GlobalAllowChangefrmPurchaseInvoiceNetPrice = Comon.cbool(row["OtherPermissionValue"]);
                                    break;

                                /************ Purchase Default **************/

                                case "DefaultPurchaseCurencyID":
                                    MySession.GlobalDefaultPurchaseCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseSupplierID":
                                    MySession.GlobalDefaultPurchaseSupplierID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseStoreID":
                                    MySession.GlobalDefaultPurchaseStoreID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseCostCenterID":
                                    MySession.GlobalDefaultPurchaseCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchasePayMethodID":
                                    MySession.GlobalDefaultPurchasePayMethodID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseNetTypeID":
                                    MySession.GlobalDefaultPurchaseNetTypeID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseDelegateID":
                                    MySession.GlobalDefaultPurchaseDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                /******************************* Purchase Return ********************************/
                                /************ Purchase Return Allow **************/
                                case "AllowChangefrmPurchaseReturnInvoiceDate":
                                    MySession.GlobalAllowChangefrmPurchaseReturnInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnStoreID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnCostCenterID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnPayMethodID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnPayMethodID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnCurencyID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnNetTypeID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnNetTypeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnDelegateID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnDebitAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnCreditAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnAdditionalAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnAdditionalAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnDiscountCreditAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnDiscountCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnChequeAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnChequeAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnNetAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnNetAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnTransportDebitAccountID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnTransportDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmPurchaseReturnSupplierID":
                                    MySession.GlobalAllowChangefrmPurchaseReturnSupplierID = Comon.cbool(row["OtherPermissionValue"]);
                                    break;
                                case "AllowChangefrmPurchaseReturnInvoiceNetPrice":
                                    MySession.GlobalAllowChangefrmPurchaseReturnInvoiceNetPrice = Comon.cbool(row["OtherPermissionValue"]);
                                    break;

                                /************ Purchase  Return Default **************/

                                case "DefaultPurchaseReturnCurencyID":
                                    MySession.GlobalDefaultPurchaseReturnCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseReturnSupplierID":
                                    MySession.GlobalDefaultPurchaseReturnSupplierID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseReturnStoreID":
                                    MySession.GlobalDefaultPurchaseReturnStoreID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseReturnCostCenterID":
                                    MySession.GlobalDefaultPurchaseCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseReturnPayMethodID":
                                    MySession.GlobalDefaultPurchaseReturnPayMethodID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseReturnNetTypeID":
                                    MySession.GlobalDefaultPurchaseReturnNetTypeID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultPurchaseReturnDelegateID":
                                    MySession.GlobalDefaultPurchaseReturnDelegateID = row["OtherPermissionValue"].ToString();
                                    break;

                                /******************************* Sale ********************************/
                                /************ Sale Allow **************/
                                case "AllowChangefrmSaleInvoiceDate":
                                    MySession.GlobalAllowChangefrmSaleInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleStoreID":
                                    MySession.GlobalAllowChangefrmSaleStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleCostCenterID":
                                    MySession.GlobalAllowChangefrmSaleCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSalePayMethodID":
                                    MySession.GlobalAllowChangefrmSalePayMethodID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleCurencyID":
                                    MySession.GlobalAllowChangefrmSaleCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleNetTypeID":
                                    MySession.GlobalAllowChangefrmSaleNetTypeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleDelegateID":
                                    MySession.GlobalAllowChangefrmSaleDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleDebitAccountID":
                                    MySession.GlobalAllowChangefrmSaleDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleCreditAccountID":
                                    MySession.GlobalAllowChangefrmSaleCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleAdditionalAccountID":
                                    MySession.GlobalAllowChangefrmSaleAdditionalAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleDiscountDebitAccountID":
                                    MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleChequeAccountID":
                                    MySession.GlobalAllowChangefrmSaleChequeAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleNetAccountID":
                                    MySession.GlobalAllowChangefrmSaleNetAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleCustomerID":
                                    MySession.GlobalAllowChangefrmSaleCustomerID = Comon.cbool(row["OtherPermissionValue"]);
                                    break;
                                case "AllowChangefrmSaleInvoiceNetPrice":
                                    MySession.GlobalAllowChangefrmSaleInvoiceNetPrice = Comon.cbool(row["OtherPermissionValue"]);
                                    break;
                                case "AllowChangefrmSaleSellerID":
                                    MySession.GlobalAllowChangefrmSaleSellerID = Comon.cbool(row["OtherPermissionValue"]);
                                    break;
                                /************ sale Default **************/

                                case "DefaultSaleCurencyID":
                                    MySession.GlobalDefaultSaleCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleCustomerID":
                                    
                                    MySession.GlobalDefaultSaleCustomerID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleStoreID":
                                    MySession.GlobalDefaultSaleStoreID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleCostCenterID":
                                    MySession.GlobalDefaultSaleCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSalePayMethodID":
                                    MySession.GlobalDefaultSalePayMethodID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleNetTypeID":
                                    MySession.GlobalDefaultSaleNetTypeID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleDelegateID":
                                    MySession.GlobalDefaultSaleDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleSellerID":
                                    MySession.GlobalDefaultSaleSellerID = row["OtherPermissionValue"].ToString();
                                    break;
                                /******************************* Sale Return ********************************/
                                /************ Sale Return Allow **************/
                                case "AllowChangefrmSaleReturnInvoiceDate":
                                    MySession.GlobalAllowChangefrmSaleReturnInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnStoreID":
                                    MySession.GlobalAllowChangefrmSaleReturnStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnCostCenterID":
                                    MySession.GlobalAllowChangefrmSaleReturnCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnPayMethodID":
                                    MySession.GlobalAllowChangefrmSaleReturnPayMethodID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnCurencyID":
                                    MySession.GlobalAllowChangefrmSaleReturnCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnNetTypeID":
                                    MySession.GlobalAllowChangefrmSaleReturnNetTypeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnDelegateID":
                                    MySession.GlobalAllowChangefrmSaleReturnDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnDebitAccountID":
                                    MySession.GlobalAllowChangefrmSaleReturnDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnCreditAccountID":
                                    MySession.GlobalAllowChangefrmSaleReturnCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnAdditionalAccountID":
                                    MySession.GlobalAllowChangefrmSaleReturnAdditionalAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnDiscountDebitAccountID":
                                    MySession.GlobalAllowChangefrmSaleReturnDiscountDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnChequeAccountID":
                                    MySession.GlobalAllowChangefrmSaleReturnChequeAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnNetAccountID":
                                    MySession.GlobalAllowChangefrmSaleReturnNetAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSaleReturnCustomerID":
                                    MySession.GlobalAllowChangefrmSaleReturnCustomerID = Comon.cbool(row["OtherPermissionValue"]);
                                    break;
                                case "AllowChangefrmSaleReturnInvoiceNetPrice":
                                    MySession.GlobalAllowChangefrmSaleReturnInvoiceNetPrice = Comon.cbool(row["OtherPermissionValue"]);
                                    break;
                                case "AllowChangefrmSaleReturnSellerID":
                                    MySession.GlobalAllowChangefrmSaleReturnSellerID = Comon.cbool(row["OtherPermissionValue"]);
                                    break;
                                /************ sale Default **************/

                                case "DefaultSaleReturnCurencyID":
                                    MySession.GlobalDefaultSaleReturnCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleReturnCustomerID":
                                    MySession.GlobalDefaultSaleReturnCustomerID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleReturnStoreID":
                                    MySession.GlobalDefaultSaleReturnStoreID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleReturnCostCenterID":
                                    MySession.GlobalDefaultSaleReturnCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleReturnPayMethodID":
                                    MySession.GlobalDefaultSaleReturnPayMethodID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleReturnNetTypeID":
                                    MySession.GlobalDefaultSaleReturnNetTypeID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleReturnDelegateID":
                                    MySession.GlobalDefaultSaleReturnDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSaleReturnSellerID":
                                    MySession.GlobalDefaultSaleReturnSellerID = row["OtherPermissionValue"].ToString();
                                    break;

                                /***************************** Spend Voucher Allow *******************************/
                                /************ Spend Voucher Allow **************/
                                case "AllowChangefrmSpendVoucherDate":
                                    MySession.GlobalAllowChangefrmSpendVoucherDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSpendVoucherCostCenterID":

                                    MySession.GlobalAllowChangefrmSpendVoucherCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;

                                case "AllowChangefrmSpendVoucherCreditAccountID":
                                    MySession.GlobalAllowChangefrmSpendVoucherCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSpendVoucherDiscountAccountID":
                                    MySession.GlobalAllowChangefrmSpendVoucherDiscountAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSpendVoucherCurencyID":
                                    MySession.GlobalAllowChangefrmSpendVoucherCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmSpendVoucherPurchasesDelegateID":
                                    MySession.GlobalAllowChangefrmSpendVoucherPurchasesDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ Spend Voucher Default **************/
                                case "DefaultSpendVoucherCurencyID":
                                    MySession.GlobalDefaultSpendVoucherCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSpendVoucherCostCenterID":
                                    MySession.GlobalDefaultSpendVoucherCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DiscountPercentSpendVoucher":
                                    MySession.GlobalDiscountPercentSpendVoucher = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultSpendVoucherPurchasesDelegateID":
                                    MySession.GlobalDefaultSpendVoucherPurchasesDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                /***************************** Check Spend Voucher Allow *******************************/
                                /************ Check Spend Voucher Allow **************/
                                case "AllowChangefrmCheckSpendVoucherDate":
                                    MySession.GlobalAllowChangefrmCheckSpendVoucherDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckSpendVoucherCostCenterID":

                                    MySession.GlobalAllowChangefrmCheckSpendVoucherCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckSpendVoucherCreditAccountID":
                                    MySession.GlobalAllowChangefrmCheckSpendVoucherCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckSpendVoucherDiscountAccountID":
                                    MySession.GlobalAllowChangefrmCheckSpendVoucherDiscountAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckSpendVoucherCurencyID":
                                    MySession.GlobalAllowChangefrmCheckSpendVoucherCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckSpendVoucherPurchasesDelegateID":
                                    MySession.GlobalAllowChangefrmCheckSpendVoucherPurchasesDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckSpendVoucherBankID":
                                    MySession.GlobalAllowChangefrmCheckSpendVoucherBankID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ CheckSpend Voucher Default **************/
                                case "DefaultCheckSpendVoucherCurencyID":
                                    MySession.GlobalDefaultCheckSpendVoucherCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultCheckSpendVoucherCostCenterID":
                                    MySession.GlobalDefaultCheckSpendVoucherCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DiscountPercentCheckSpendVoucher":
                                    MySession.GlobalDiscountPercentCheckSpendVoucher = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultCheckSpendVoucherPurchasesDelegateID":
                                    MySession.GlobalDefaultCheckSpendVoucherPurchasesDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultCheckSpendVoucherBankID":
                                    MySession.GlobalDefaultCheckSpendVoucherBankID = row["OtherPermissionValue"].ToString();
                                    break;
                                /******************************* Receipt Voucher ********************************/
                                /************ Receipt Voucher Allow **************/
                                case "AllowChangefrmReceiptVoucherDate":
                                    MySession.GlobalAllowChangefrmReceiptVoucherDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmReceiptVoucherCostCenterID":

                                    MySession.GlobalAllowChangefrmReceiptVoucherCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmReceiptVoucherSalesDelegateID":
                                    MySession.GlobalAllowChangefrmReceiptVoucherSalesDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmReceiptVoucherDebitAccountID":
                                    MySession.GlobalAllowChangefrmReceiptVoucherDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmReceiptVoucherDiscountAccountID":
                                    MySession.GlobalAllowChangefrmReceiptVoucherDiscountAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmReceiptVoucherCurencyID":
                                    MySession.GlobalAllowChangefrmReceiptVoucherCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ Receipt Voucher Default **************/
                                case "DefaultReceiptVoucherCurencyID":
                                    MySession.GlobalDefaultReceiptVoucherCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultReceiptVoucherCostCenterID":
                                    MySession.GlobalDefaultReceiptVoucherCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultReceiptVoucherSalesDelegateID":
                                    MySession.GlobalDefaultReceiptVoucherSalesDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DiscountPercentReceiptVoucher":
                                    MySession.GlobalDiscountPercentReceiptVoucher = row["OtherPermissionValue"].ToString();
                                    break;

                                /******************************* Check Receipt Voucher ********************************/
                                /************Check Receipt Voucher Allow **************/
                                case "AllowChangefrmCheckReceiptVoucherDate":
                                    MySession.GlobalAllowChangefrmCheckReceiptVoucherDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckReceiptVoucherCostCenterID":

                                    MySession.GlobalAllowChangefrmCheckReceiptVoucherCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckReceiptVoucherSalesDelegateID":
                                    MySession.GlobalAllowChangefrmCheckReceiptVoucherSalesDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckReceiptVoucherDebitAccountID":
                                    MySession.GlobalAllowChangefrmCheckReceiptVoucherDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckReceiptVoucherDiscountAccountID":
                                    MySession.GlobalAllowChangefrmCheckReceiptVoucherDiscountAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckReceiptVoucherCurencyID":
                                    MySession.GlobalAllowChangefrmCheckReceiptVoucherCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmCheckReceiptVoucherBankID":
                                    MySession.GlobalAllowChangefrmCheckReceiptVoucherBankID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************Check Receipt Voucher Default **************/
                                case "DefaultCheckReceiptVoucherCurencyID":
                                    MySession.GlobalDefaultCheckReceiptVoucherCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultCheckReceiptVoucherCostCenterID":
                                    MySession.GlobalDefaultCheckReceiptVoucherCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultCheckReceiptVoucherSalesDelegateID":
                                    MySession.GlobalDefaultCheckReceiptVoucherSalesDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DiscountPercentCheckReceiptVoucher":
                                    MySession.GlobalDiscountPercentCheckReceiptVoucher = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultCheckReceiptVoucherBankID":
                                    MySession.GlobalDefaultCheckReceiptVoucherBankID = row["OtherPermissionValue"].ToString();
                                    break;
                                /******************************* Opening Voucher ********************************/
                                /************ Opening Voucher Allow **************/
                                case "AllowChangefrmOpeningVoucherDebitAccountID":
                                    MySession.GlobalAllowChangefrmOpeningVoucherDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmOpeningVoucherCreditAccountID":
                                    MySession.GlobalAllowChangefrmOpeningVoucherCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmOpeningVoucherDate":
                                    MySession.GlobalAllowChangefrmOpeningVoucherDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmOpeningVoucherCostCenterID":
                                    MySession.GlobalAllowChangefrmOpeningVoucherCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmOpeningVoucherCurencyID":
                                    MySession.GlobalAllowChangefrmOpeningVoucherCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ Opening Voucher Allow **************/
                                case "DefaultOpeningVoucherCostCenterID":
                                    MySession.GlobalDefaultOpeningVoucherCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultOpeningVoucherCurencyID":
                                    MySession.GlobalDefaultOpeningVoucherCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                /******************************* Various Voucher ********************************/
                                /************ Various Voucher Allow **************/
                                case "AllowChangefrmVariousVoucherDate":
                                    MySession.GlobalAllowChangefrmVariousVoucherDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmVariousVoucherCurencyID":
                                    MySession.GlobalAllowChangefrmVariousVoucherCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmVariousVoucherCostCenterID":
                                    MySession.GlobalAllowChangefrmVariousVoucherCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmVariousVoucherSalesDelegateID":
                                    MySession.GlobalAllowChangefrmVariousVoucherSalesDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ Various Voucher Default **************/
                                case "DefaultVariousVoucherCurencyID":
                                    MySession.GlobalDefaultVariousVoucherCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultVariousVoucherCostCenterID":
                                    MySession.GlobalDefaultVariousVoucherCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultVariousVoucherSalesDelegateID":
                                    MySession.GlobalDefaultVariousVoucherSalesDelegateID = row["OtherPermissionValue"].ToString();
                                    break;
                                /******Formes Printing *******/
                                case "DefaultFormPrintingID":
                                    MySession.GlobalDefaultFormPrintingID = row["OtherPermissionValue"].ToString();

                                    MySession.GlobalDefaultItemsInOnBailFormPrintingID = row["OtherPermissionValue"].ToString();

                                    MySession.GlobalDefaultItemsOutonBailFormPrintingID = row["OtherPermissionValue"].ToString();

                                    MySession.GlobalDefaultSaleFormPrintingID = row["OtherPermissionValue"].ToString();

                                    MySession.GlobalDefaultPurchaseFormPrintingID = row["OtherPermissionValue"].ToString();

                                    MySession.GlobalDefaultSaleReturnFormPrintingID = row["OtherPermissionValue"].ToString();

                                    MySession.GlobalDefaultPurchaseReturnFormPrintingID = row["OtherPermissionValue"].ToString();

                                    MySession.GlobalDefaultGoodsOpeningFormPrintingID = row["OtherPermissionValue"].ToString();

                                    break;
                                //case "DefaultItemsInOnBailFormPrintingID":
                                //    MySession.GlobalDefaultItemsInOnBailFormPrintingID = row["OtherPermissionValue"].ToString();
                                //    break;
                                //case "DefaultItemsOutonBailFormPrintingID":
                                //    MySession.GlobalDefaultItemsOutonBailFormPrintingID = row["OtherPermissionValue"].ToString();
                                //    break;
                                //case "DefaultSaleFormPrintingID":
                                //    MySession.GlobalDefaultSaleFormPrintingID = row["OtherPermissionValue"].ToString();
                                //    break;
                                //case "DefaultPurchaseFormPrintingID":
                                //    MySession.GlobalDefaultPurchaseFormPrintingID = row["OtherPermissionValue"].ToString();
                                //    break;
                                //case "DefaultSaleReturnFormPrintingID":
                                //    MySession.GlobalDefaultSaleReturnFormPrintingID = row["OtherPermissionValue"].ToString();
                                //    break;
                                //case "DefaultPurchaseReturnFormPrintingID":
                                //    MySession.GlobalDefaultPurchaseReturnFormPrintingID = row["OtherPermissionValue"].ToString();
                                //    break;
                                //case "DefaultGoodsOpeningFormPrintingID":
                                //    MySession.GlobalDefaultGoodsOpeningFormPrintingID = row["OtherPermissionValue"].ToString();
                                //    break;
                                /******************************* Goods Opening ********************************/
                                /************ Goods Opening Allow **************/
                                case "AllowChangefrmGoodsOpeningInvoiceDate":
                                    MySession.GlobalAllowChangefrmGoodsOpeningInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmGoodsOpeningStoreID":
                                    MySession.GlobalAllowChangefrmGoodsOpeningStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmGoodsOpeningCostCenterID":
                                    MySession.GlobalAllowChangefrmGoodsOpeningCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;

                                case "AllowChangefrmGoodsOpeningCurencyID":
                                    MySession.GlobalAllowChangefrmGoodsOpeningCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;

                                case "AllowChangefrmGoodsOpeningDebitAccountID":
                                    MySession.GlobalAllowChangefrmGoodsOpeningDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmGoodsOpeningCreditAccountID":
                                    MySession.GlobalAllowChangefrmGoodsOpeningCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;

                                /************ Goods Opening Default **************/

                                case "DefaultGoodsOpeningCurencyID":
                                    MySession.GlobalDefaultGoodsOpeningCurencyID = row["OtherPermissionValue"].ToString();
                                    break;

                                case "DefaulGoodsOpeningStoreID":
                                    MySession.GlobalDefaultPurchaseStoreID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultGoodsOpeningCostCenterID":
                                    MySession.GlobalDefaultGoodsOpeningCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;

                                /******************************* Items In On Bail ********************************/
                                /************ Items In On Bail Allow **************/

                                case "AllowChangefrmItemsInOnBailInvoiceDate":
                                    MySession.GlobalAllowChangefrmItemsInOnBailInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsInOnBailStoreID":
                                    MySession.GlobalAllowChangefrmItemsInOnBailStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsInOnBailCostCenterID":
                                    MySession.GlobalAllowChangefrmItemsInOnBailCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsInOnBailCurencyID":
                                    MySession.GlobalAllowChangefrmItemsInOnBailCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsInOnBailSupplierID":
                                    MySession.GlobalAllowChangefrmItemsInOnBailSupplierID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsInOnBailCreditAccountID":
                                    MySession.GlobalAllowChangefrmItemsInOnBailCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsInOnBailDebitAccountID":
                                    MySession.GlobalAllowChangefrmItemsInOnBailDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ Items In On Bail Default **************/

                                case "DefaultItemsInOnBailCurencyID":
                                    MySession.GlobalDefaultItemsInOnBailCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultItemsInOnBailCostCenterID":
                                    MySession.GlobalDefaultItemsInOnBailCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultItemsInOnBailSalesSupplierID":
                                    MySession.GlobalDefaultItemsInOnBailSupplierID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultItemsInOnBailStoreID":
                                    MySession.GlobalDefaultItemsInOnBailStoreID = row["OtherPermissionValue"].ToString();
                                    break;

                                /******************************* Items Out On Bail ********************************/
                                /************ Items Out On Bail Allow **************/

                                case "AllowChangefrmItemsOutOnBailInvoiceDate":
                                    MySession.GlobalAllowChangefrmItemsOutOnBailInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsOutOnBailStoreID":
                                    MySession.GlobalAllowChangefrmItemsOutOnBailStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsOutOnBailCostCenterID":
                                    MySession.GlobalAllowChangefrmItemsOutOnBailCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsOutOnBailCurencyID":
                                    MySession.GlobalAllowChangefrmItemsOutOnBailCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsOutOnBailCustomerID":
                                    MySession.GlobalAllowChangefrmItemsOutOnBailCustomerID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsOutOnBailCreditAccountID":
                                    MySession.GlobalAllowChangefrmItemsOutOnBailCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowChangefrmItemsOutOnBailDebitAccountID":
                                    MySession.GlobalAllowChangefrmItemsOutOnBailDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ Items Out On Bail Default **************/

                                case "DefaultItemsOutOnBailCurencyID":
                                    MySession.GlobalDefaultItemsOutOnBailCurencyID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultItemsOutOnBailCostCenterID":
                                    MySession.GlobalDefaultItemsOutOnBailCostCenterID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultItemsOutOnBailCustomerID":
                                    MySession.GlobalDefaultItemsOutOnBailCustomerID = row["OtherPermissionValue"].ToString();
                                    break;
                                case "DefaultItemsOutOnBailStoreID":
                                    MySession.GlobalDefaultItemsOutOnBailStoreID = row["OtherPermissionValue"].ToString();
                                    break;
                                /************ AllowUsingDateItems**************/
                                case "AllowUsingDateItems":
                                    MySession.GlobalUsingExpiryDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    MySession.GlobalAllowUsingDateItems = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                /************ AllowOutItemsWithOutBalance**************/
                                case "AllowOutItemsWithOutBalance":
                                    MySession.GlobalAllowOutItemsWithOutBalance = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                                case "AllowUsingBarcodeInInvoices":
                                    MySession.GlobalAllowUsingBarcodeInInvoices = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;


                                case "AllowBranchModificationAllScreens":
                                    MySession.GlobalAllowBranchModificationAllScreens = Comon.cbool(row["OtherPermissionValue"].ToString());
                                    break;
                            }


                        }
                        //string VAt = "Select CompanyVATID from  VATIDCOMPANY ";
                        //var dtVAt = Lip.SelectRecord(VAt);
                        //if(dtVAt.Rows.Count>0)
                           MySession. VAtCompnyGlobal="1455555555555";
                        MySession.PrintModel = ReportComponent.GettRecord("PrintSaleInvoiceToCashierReport");
                        MySession.PrintBuildPill = ReportComponent.GettRecord("PrintBuildBill");
                        MySession.PrintLAnguage = ReportComponent.GettRecord("PrintLanguage");
                        MySession.UseNetINInvoiceSales = ReportComponent.GettRecord("UseNetINInvoiceSales");
                        GeneralSettings generalSettting = GeneralSettingsDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
                        CompanyHeader cmpheader = CompanyHeaderDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
                        if (cmpheader != null) {



                            MySession.GlobalCompanyName = cmpheader.CompanyArbName;
                            MySession.footer = cmpheader.footer;
                        
                        
                        
                        
                        }
                        if (generalSettting != null)
                        {
                            MySession.defaultBackupPath = generalSettting.BackupPath;
                            MySession.GlobalUsingItemsSerials = Comon.cbool(generalSettting.UsingItemsSerials);
                            MySession.GlobalWayOfOutItems = generalSettting.WayOfOutItems;
                            MySession.GlobalQtyDigits = generalSettting.QtyDigits;
                            MySession.GlobalPriceDigits = generalSettting.PriceDigits;
                            MySession.GlobalMaxBarcodeDigits = generalSettting.MaxBarcodeDigits;
                            MySession.GlobalItemProfit = generalSettting.ItemProfit;
                            MySession.GlobalAutoCalcFixAssetsDepreciation = Comon.cbool(generalSettting.AutoCalcFixAssetsDepreciation);
                            // MySession.GlobalUsingExpiryDate = Comon.cbool(generalSettting.UsingExpiryDate);
                            // MySession.ItemPriceDigits = generalSettting.ItemPriceDigits;
                            // MySession.DepreciationType = generalSettting.DepreciationType;
                            // MySession.ItemDigits = generalSettting.ItemDigits;

                        }
                        else
                        {
                            SplashScreenManager.CloseForm(false);
                            bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Dear User You must adjust the system Settings Do you want to adjust the system Settings Now ? " : "عزيزي المستخدم يجب عليك ضبط عدادات النظام هل تريد ضبط عدادات النظام لان ؟ "));
                            if (Yes)
                            {

                                frmGeneralOptions frmGeneralOptions = new frmGeneralOptions();
                                frmGeneralOptions.ShowDialog();
                            }
                        }
                       
                        /*********************Role For Form ****************************/

                        frmMainEdex frm = new frmMainEdex();
                        SplashScreenManager.CloseForm(false);
                        frm.Show();
                        this.Hide();
                    }

                }
                else
                {
                    SplashScreenManager.CloseForm(false);
                    if (MySession.GlobalLanguageName == iLanguage.Arabic)
                        XtraMessageBox.Show("إسم المستحدم او كلمة المرور غير صحيحة", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        XtraMessageBox.Show("The username or password is incorrect", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                }
            }
            catch (Exception ex)
            {

                SplashScreenManager.CloseForm(false);
                if (ex.HResult == -2146232798)
                    return;

                //Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        void GetDatabaseList()
        {
            try
            {
                DataTable dtListDataBase = new DataTable();
                string text = System.IO.File.ReadAllText(Application.StartupPath+"\\Server.txt");


                cConnectionString.ServerName = text;

                cConnectionString.GetConnectionSetting();
                string ConString = ConfigurationManager.ConnectionStrings["SettingDBConnection"].ConnectionString;
                ConString=  ConString.Replace("IPADDRESS", cConnectionString.ServerName);

                using (SqlConnection con =   cConnectionString.GetConnectionSetting())
                {

                    con.Open();
                    using (SqlCommand objCmd = con.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.Text;
                        objCmd.CommandText = "SELECT database_id as ID,name as Name from sys.databases ";
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        dtListDataBase.Load(myreader);

                    }
                    
                    cmbDataBaseName.Properties.DataSource = dtListDataBase;
                    cmbDataBaseName.Properties.DisplayMember = "Name";
                    cmbDataBaseName.Properties.ValueMember = "ID";
                    cmbDataBaseName.Properties.NullText = "إختار القاعدة البيانات";
                    cmbDataBaseName.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
                }

                using (SqlConnection con = cConnectionString.GetConnectionSetting())
                {
                    DataTable dt = new DataTable();
                    con.Open();
                    using (SqlCommand objCmd = con.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.Text;
                        objCmd.CommandText = "Select DataBaseName from SMSSettings where ProgramName ='AccountSystem'";
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        dt.Load(myreader);

                        foreach (DataRow row in dtListDataBase.Rows)
                        {
                            if (row["Name"].ToString() == dt.Rows[0]["DataBaseName"].ToString())
                            {
                                DataBaseName = dt.Rows[0]["DataBaseName"].ToString();
                                DataBaseID = Comon.cInt(row["ID"].ToString());
                                cmbDataBaseName.EditValue = Comon.cInt(row["ID"].ToString());
                            }
                        }
                    }
                }

                MySession.PubDatabaseName = DataBaseName;
                cConnectionString.DataBasename = DataBaseName;
              
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void cmbDataBaseName_EditValueChanged(object sender, EventArgs e)
        {

            if (DataBaseName != "" && cmbDataBaseName.Properties.GetDisplayText(cmbDataBaseName.EditValue) == "")
                cConnectionString.DataBasename = DataBaseName;
            else
            {
                int value = Comon.cInt(cmbDataBaseName.EditValue.ToString());
                if (value == 0)
                    return;
                cConnectionString.DataBasename = cmbDataBaseName.Properties.GetDisplayText(cmbDataBaseName.EditValue);
            }
            //string strCon = ConfigurationManager.ConnectionStrings["EbtexDBConnection"].ConnectionString;
            //strCon = strCon.Replace("DataBase.mdf", cConnectionString.DataBasename);
            //strCon = strCon.Replace("IPADDRESS", cConnectionString.ServerName);
            //cConnectionString.ConnectionString = strCon;

            ////.............................
            //string strSQL = "SELECT Database_id FROM sys.databases Where Name='" + cConnectionString.DataBasename + "'";
            //DataTable dt = Lip.SelectRecord(strSQL);

            //strSQL = "SELECT Top 1 Name,physical_name FROM sys.master_files "
            //+ " Where Database_id=" + dt.Rows[0]["Database_id"] + " Order By File_id ASC";
            //dt = Lip.SelectRecord(strSQL);


            //string PubCurrentDataBasePath = dt.Rows[0]["physical_name"].ToString().Replace(cConnectionString.DataBasename + ".mdf", " ");

            //MySession.PubCurrentDataBasePath = PubCurrentDataBasePath;
            //MySession.PubCurrentLogicalName = dt.Rows[0]["Name"].ToString();
           
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
        static string ReadSubKeyValue(string subKey, string key)
        {

            string str = string.Empty;

            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(subKey))
            {

                if (registryKey != null)
                {
                    str = registryKey.GetValue(key).ToString();
                    registryKey.Close();
                }
            }
            return str;
        }

        private void RegistrEdex_Load()
        {
            string Active;
            dynamic obj_FSO = Activator.CreateInstance(Type.GetTypeFromProgID("Scripting.FileSystemObject"));
            var obj_Drive = obj_FSO.GetDrive("C:\\");
            long SerialNumber = Comon.cLong(Math.Abs(obj_Drive.SerialNumber()));
            RegActive = SerialNumber + 11245;
            RegActive = RegActive * 20;

            try
            {
                Active = ReadSubKeyValue("EdexAcountSystem", "Activation");
                if (Active == "" || Active != RegActive.ToString())
                {
                  //  RegistrEdex frm = new RegistrEdex();
                    //frm.ShowDialog();
                }
                else {

                    if (Active != RegActive.ToString())
                        Application.Exit();

                }
            }

            catch
            {}

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
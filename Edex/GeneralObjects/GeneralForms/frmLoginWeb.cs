using System;
using System.Net.NetworkInformation;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Edex.Model;
using Edex.DAL.UsersManagement;
using Edex.DAL.Common;
using Edex.Model.Language;
using Edex.ModelSystem;
using Edex.DAL;
using DevExpress.XtraSplashScreen;
using Edex.DAL.Configuration;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Win32;
using Edex.GeneralObjects.GeneralClasses;
namespace Edex.GeneralObjects.GeneralForms
{
    /// <summary>
    /// This class to log inherits from DevExpress.XtraEditors.XtraForm, is to login to the system
    /// </summary>
    public partial class frmLoginWeb : DevExpress.XtraEditors.XtraForm
    {
        #region Declare varaible
        public long RegActive;
        static int DataBaseID = 0;
        static string DataBaseName = "";
        static string CustomerName = "";
        string PrimaryName = "ArbName";
        #endregion
        #region Event
        public frmLoginWeb()
        {

            InitializeComponent();
            if(cmbLangauage.EditValue!=null&& cmbLangauage.EditValue.ToString()!="ar")
            {
                PrimaryName = "EngName";
            }
         
            this.cmbDataBaseName.EditValueChanged += new System.EventHandler(this.cmbDataBaseName_EditValueChanged);
           
        }

        void txtUserID_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
               
                if (string.IsNullOrWhiteSpace(txtUserID.Text)==false)
                {
                    if (cmbBranchesID.EditValue == null || Comon.cInt(cmbBranchesID.EditValue) <= 0)
                    {
                        XtraMessageBox.Show(cmbLangauage.EditValue.ToString() == "ar" ? "الرجاء إختيار الفرع" : " Please Select The Branch! ");
                        cmbBranchesID.Focus();
                        return;
                    }
                    string UserName = "";

                    if (UserInfo.Language == iLanguage.Arabic)
                        UserName = Lip.GetValue("SELECT [ArbName]   FROM  [Users] where BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" and Cancel=0 and [UserID]=" + Comon.cInt(txtUserID.Text)).ToString();
                    else
                        UserName = Lip.GetValue("SELECT EngName   FROM  [Users] where BranchID=" + Comon.cInt(cmbBranchesID.EditValue) + " and  Cancel=0 and [UserID]=" + Comon.cInt(txtUserID.Text)).ToString();
                    if (UserName!="")
                       txtUserName.Text = UserName;
                    else 
                    {
                        XtraMessageBox.Show(cmbLangauage.EditValue.ToString() == "ar" ? "الرجاء ادخال رقم المستخدم الصحيح" : "Please enter the correct user number");
                        txtUserID.Text = "";
                        txtUserName.Text = "";
                        txtUserID.Focus();
                    }
                }
                else 
                     txtUserName.Text ="";
            }
            catch { }
        }
        private bool RegistrEdex_Load()
        {
            string Active;
            dynamic obj_FSO = Activator.CreateInstance(Type.GetTypeFromProgID("Scripting.FileSystemObject"));
            var obj_Drive = obj_FSO.GetDrive("C:\\");
            long SerialNumber = Comon.cLong(Math.Abs(obj_Drive.SerialNumber()));
            RegActive = SerialNumber + 54678431111;
            RegActive = RegActive * 3;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\OurSettings");
                Active = key.GetValue("OmexManuFactoryAcountSystem").ToString();
                if (Active == "" || Active != RegActive.ToString())
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// This Event To Loading Form
        /// </summary>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            RegistrEdex_Load();
            string text = "";
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
         
            defaultLookAndFeel1.LookAndFeel.SetSkinStyle(SystemSettings.GetSkinName(Path));
            cmbLangauage.SelectedIndex = 0;
            
            //try
            //{
            //    string startupPath = Directory.GetCurrentDirectory() + "\\";
            //    var UserID = new FileStream(@startupPath + "UserID.txt", FileMode.Open, FileAccess.Read);
            //    if (UserID == null)
            //        return;
            //    using (var streamReader = new StreamReader(UserID, Encoding.UTF8))
            //    {
            //        text = streamReader.ReadToEnd();
            //    }
            //    txtUserID.Text = text.ToString();
            //    txtPassword.Focus();
              
            //}
            //catch
            //{

            //}
           
        }
        /// <summary>
        ///  This event is executed when the form is Shown and the combobox fills the dataBase with the names of the database 
        ///  on the connected server in order to select one of them,
        ///  And focus on the password field after entering the username automatically from the username file stored in the system files with file Name UserID.text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLoginWeb_Shown(object sender, EventArgs e)
        {
            dynamic obj_FSO = Activator.CreateInstance(Type.GetTypeFromProgID("Scripting.FileSystemObject"));
            var obj_Drive = obj_FSO.GetDrive("C:\\");
            long SerialNumber = Comon.cLong(Math.Abs(obj_Drive.SerialNumber()));
            txtComputerNumber.Text = SerialNumber.ToString();

            if (RegistrEdex_Load() == false)
            {
                panel1.Visible = true;
                return;
            }
            GetDatabaseList();
            if (txtUserID.Text != string.Empty)
                txtPassword.Focus();

            try
            {
                string textexpire = System.IO.File.ReadAllText(Application.StartupPath + "\\Sync33.dll");

                String datnow = (DateTime.Now.ToString("yyyy/MM/dd"));

                int textnowseral = Comon.ConvertDateToSerial(datnow);
                int textexpireseral = Comon.ConvertDateToSerial(textexpire);
                if (textnowseral > textexpireseral)
                {
                    XtraMessageBox.Show("أنتهت صلاحية النسخة, الرجاء الإتصال بالدعم الفني", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }
            catch
            {
            }
            cConnectionString.GetConnectionSetting();
            this.txtUserID.Validating += txtUserID_Validating;
            txtUserID_Validating(null, null);
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
           
        }
        /// <summary>
        /// This Event To select Langugae
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLangauage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit obj = (ComboBoxEdit)sender;
            if (obj.SelectedIndex == 0)
            {
                // Check if the global language name is not already set to Arabic
                if (MySession.GlobalLanguageName != iLanguage.Arabic)
                {
                    MySession.GlobalLanguageName = iLanguage.Arabic;// Set the global language to Arabic
                    ChangeLanguage.ArabicLanguage(this);
                }
            }
            else
            { // Check if the global language name is not already set to English
                if (MySession.GlobalLanguageName != iLanguage.English)
                {
                    MySession.GlobalLanguageName = iLanguage.English;
                    ChangeLanguage.EnglishLanguage(this);
                }
            }
        }
       public static void SetMySession( int UserID,int BranchID)
        {
            string startupPath = Directory.GetCurrentDirectory() + "\\";
            var Type = new FileStream(@startupPath + "typevat.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(Type, Encoding.UTF8))
            {
              MySession.GlobalHaveVat = streamReader.ReadToEnd();
            }
            string strSQL = "SELECT  OtherPermissionName ,OtherPermissionValue,OtherPermissionIndex FROM UserOtherPermissions where    UserID=" + UserID + " and BranchID=" + BranchID;
            DataTable dtOtherPermissions = Lip.SelectRecord(strSQL);
            foreach (DataRow row in dtOtherPermissions.Rows)
            {
                #region switch OtherPermissionName
                switch (row["OtherPermissionName"].ToString())
                {

                    case "CostPriceType":
                        // Set the global cost price type based on the other permission index
                        MySession.PubCostPriceType = row["OtherPermissionIndex"].ToString();
                        break;
                    case "SalePriceType":
                        // Set the global sale price type based on the other permission index
                        MySession.PubSalePriceType = row["OtherPermissionIndex"].ToString();
                        break;
                    /************ Default **************/
                    case "DefaultStoreID":
                        // Set the global default store ID based on the other permission value
                        MySession.GlobalDefaultStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCostCenterID":
                        // Set the global default cost center ID based on the other permission value
                        MySession.GlobalDefaultCostCenterID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultSellerID":
                        // Set the global default seller ID based on the other permission value
                        MySession.GlobalDefaultSellerID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultDebitAccountID":
                        // Set the global default debit account ID based on the other permission value
                        MySession.GlobalDefaultDebitAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCurencyID":
                        // Set the global default currency ID based on the other permission value
                        MySession.GlobalDefaultCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultSalesDelegateID":
                        // Set the global default sales delegate ID
                        MySession.GlobalDefaultSalesDelegateID = row["OtherPermissionValue"].ToString();
                        break;
                    /************ Discount **************/
                    case "DiscountPercentOnItem":
                        // Set the global discount percent on the item to the value specified in the row
                        MySession.GlobalDiscountPercentOnItem = Comon.cDec(row["OtherPermissionValue"].ToString());
                        break;
                    case "DiscountPercentOnTotal":
                        // Set the global discount percent on the total to the value specified in the row
                        MySession.GlobalDiscountPercentOnTotal = Comon.cDec(row["OtherPermissionValue"].ToString());
                        break;
                    /************ Setting **************/
                    case "NumDecimalPlaces":
                        // Set the global number of decimal places to the value specified in the row
                        MySession.GlobalNumDecimalPlaces = Comon.cInt(row["OtherPermissionValue"].ToString());
                        break;
                    case "PercentVat":
                        // Set the global percent VAT to the value specified in the row
                        MySession.GlobalPercentVat = Comon.cInt(row["OtherPermissionValue"].ToString());
                        break;
                    case "NoOfLevels":
                        // Set the global number of levels to the value specified in the row
                        MySession.GlobalNoOfLevels = Comon.cInt(row["OtherPermissionValue"].ToString());
                        break;
                    /************ Can Change **************/
                    case "CanChangeDocumentsDate":
                        // Setting global permission to change document dates
                        MySession.GlobalCanChangeDocumentsDate = Comon.cbool(row["OtherPermissionIndex"].ToString());
                        break;
                    case "CanChangeInvoicePrice":
                        // Setting global permission to change invoice prices
                        MySession.GlobalCanChangeInvoicePrice = Comon.cbool(row["OtherPermissionIndex"].ToString());
                        break;
                    case "ShowItemQtyInSaleInvoice":
                        // Setting global permission to show item quantity in sale invoice
                        MySession.GlobalShowItemQtyInSaleInvoice = Comon.cbool(row["OtherPermissionIndex"].ToString());
                        break;
                    case "CanDiscountOnCashierScreen":
                        // Setting global permission to give discount on cashier screen
                        MySession.GlobalCanDiscountOnCashierScreen = Comon.cbool(row["OtherPermissionIndex"].ToString());
                        break;
                    case "CanCloseCashier":
                        // Setting global permission to close cashier
                        MySession.GlobalCanCloseCashier = Comon.cbool(row["OtherPermissionIndex"].ToString());
                        break;
                    case "CanChangePriceInCashierScreen":
                        // Setting global permission to change item price on cashier screen
                        MySession.GlobalCanChangePriceInCashierScreen = Comon.cbool(row["OtherPermissionIndex"].ToString());
                        break;
                    case "CanSearchItemInCashierScreen":
                        // Setting global permission to search items on cashier screen
                        MySession.GlobalCanSearchItemInCashierScreen = Comon.cbool(row["OtherPermissionIndex"].ToString());
                        break;
                    case "CanSaleItemForZeroPrice":
                        // Setting global permission to allow sale of items for zero price
                        MySession.GlobalCanSaleItemForZeroPrice = Comon.cbool(row["OtherPermissionIndex"].ToString());
                        break;
                    case "CanOpenCashierDrawerByF12":
                        // Setting global permission to open cashier drawer using F12 key
                        MySession.GlobalCanOpenCashierDrawerByF12 = Comon.cbool(row["OtherPermissionIndex"].ToString());
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
                        // Set the global variable for allowing change from purchase invoice date
                        MySession.GlobalAllowChangefrmPurchaseInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmPurchaseStoreID":
                        // Set the global variable for allowing change from purchase store ID
                        MySession.GlobalAllowChangefrmPurchaseStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseCostCenterID":
                        // Set the global variable for allowing change from purchase cost center ID
                        MySession.GlobalAllowChangefrmPurchaseCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchasePayMethodID":
                        // Set the global variable for allowing change from purchase pay method ID
                        MySession.GlobalAllowChangefrmPurchasePayMethodID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseCurencyID":
                        // Set the global variable for allowing change from purchase currency ID
                        MySession.GlobalAllowChangefrmPurchaseCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseNetTypeID":
                        // Set the global variable for allowing change from purchase net type ID
                        MySession.GlobalAllowChangefrmPurchaseNetTypeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseDelegateID":
                        // Set the global variable for allowing change from purchase delegate ID
                        MySession.GlobalAllowChangefrmPurchaseDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseDebitAccountID":
                        // Set the global variable for allowing change from purchase debit account ID
                        MySession.GlobalAllowChangefrmPurchaseDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseCreditAccountID":
                        // Set the global variable for allowing change from purchase credit account ID
                        MySession.GlobalAllowChangefrmPurchaseCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseAdditionalAccountID":
                        // Set the global variable for allowing change from purchase additional account ID
                        MySession.GlobalAllowChangefrmPurchaseAdditionalAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseDiscountCreditAccountID":
                        // Set the global variable for allowing change from purchase discount credit account ID
                        MySession.GlobalAllowChangefrmPurchaseDiscountCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseChequeAccountID":
                        // Set the global variable for allowing change from purchase cheque account ID
                        MySession.GlobalAllowChangefrmPurchaseChequeAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseNetAccountID":
                        // Set the global variable for allowing change from purchase net account ID
                        MySession.GlobalAllowChangefrmPurchaseNetAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseTransportDebitAccountID":
                        // Set the global variable for allowing change from purchase transport debit account ID
                        MySession.GlobalAllowChangefrmPurchaseTransportDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPurchaseSupplierID":
                        // Set the global variable for allowing change from purchase supplier ID 
                        MySession.GlobalAllowChangefrmPurchaseSupplierID = Comon.cbool(row["OtherPermissionValue"]);
                        break;
                    case "AllowChangefrmPurchaseInvoiceNetPrice":
                        // Set the global variable for allowing change from purchase invoice net price 
                        MySession.GlobalAllowChangefrmPurchaseInvoiceNetPrice = Comon.cbool(row["OtherPermissionValue"]);
                        break;


                    /************ Purchase Default **************/
                    case "DefaultPurchaseCurencyID":
                        // Set the global variable for default purchase currency ID 
                        MySession.GlobalDefaultPurchaseCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseSupplierID":
                        // Set the global variable for default purchase supplier ID 
                        MySession.GlobalDefaultPurchaseSupplierID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseStoreID":
                        // Set the global variable for default purchase store ID 
                        MySession.GlobalDefaultPurchaseStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseCostCenterID":
                        // Set the global variable for default purchase cost center ID 
                        MySession.GlobalDefaultPurchaseCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchasePayMethodID":
                        // Set the global variable for default purchase payment method ID 
                        MySession.GlobalDefaultPurchasePayMethodID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseNetTypeID":
                        // Set the global variable for default purchase net type ID 
                        MySession.GlobalDefaultPurchaseNetTypeID = row["OtherPermissionValue"].ToString();
                        break;
                    // Set the global variable for default purchase delegate ID
                    case "DefaultPurchaseDelegateID":
                        MySession.GlobalDefaultPurchaseDelegateID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseAddtionalAccountID":
                        MySession.GlobalDefaultPurchaseAddtionalAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseCrditAccountID":
                        MySession.GlobalDefaultPurchaseCrditAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseDebitAccountID":
                        MySession.GlobalDefaultPurchaseDebitAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseDiscountAccountID":
                        MySession.GlobalDefaultPurchaseDiscountAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    /*******************************Order Purchase  ********************************/
                    case "DefaultOrderPurchaseCurrncyID":
                        MySession.GlobalDefaultOrderPurchaseCurrncyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultOrderPurchaseCostCenterID":
                        MySession.GlobalDefaultOrderPurchaseCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultOrderPurchaseSupplierID":
                        MySession.GlobalDefaultOrderPurchaseSupplierID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultOrderPurchaseStoreID":
                        MySession.GlobalDefaultOrderPurchaseStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultOrderPurchaseDelegateID":
                        MySession.GlobalDefaultOrderPurchaseDelegateID = row["OtherPermissionValue"].ToString();
                        break;
                    /************Order Purchase   Allow **************/
                    case "AllowChangefrmOrderPurchaseCostCenterID":
                        MySession.GlobalAllowChangefrmOrderPurchaseCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmOrderPurchaseCurrncyID":
                        MySession.GlobalAllowChangefrmOrderPurchaseCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmOrderPurchaseDelegateID":
                        MySession.GlobalAllowChangefrmOrderPurchaseDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmOrderPurchaseDate":
                        MySession.GlobalAllowChangefrmOrderPurchaseDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmOrderPurchaseStoreID":
                        MySession.GlobalAllowChangefrmOrderPurchaseStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmOrderPurchaseSupplierID":
                        MySession.GlobalAllowChangefrmOrderPurchaseSupplierID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    /******************************* Purchase Return ********************************/
                    /************ Purchase Return Allow **************/
                    // Allow changing from purchase return invoice date
                    case "AllowChangefrmPurchaseReturnInvoiceDate":
                        MySession.GlobalAllowChangefrmPurchaseReturnInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow changing from purchase return store ID
                    case "AllowChangefrmPurchaseReturnStoreID":
                        MySession.GlobalAllowChangefrmPurchaseReturnStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow changing from purchase return cost center ID
                    case "AllowChangefrmPurchaseReturnCostCenterID":
                        MySession.GlobalAllowChangefrmPurchaseReturnCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow changing from purchase return pay method ID
                    case "AllowChangefrmPurchaseReturnPayMethodID":
                        MySession.GlobalAllowChangefrmPurchaseReturnPayMethodID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow changing from purchase return currency ID
                    case "AllowChangefrmPurchaseReturnCurencyID":
                        MySession.GlobalAllowChangefrmPurchaseReturnCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow changing from purchase return net type ID
                    case "AllowChangefrmPurchaseReturnNetTypeID":
                        MySession.GlobalAllowChangefrmPurchaseReturnNetTypeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow changing from purchase return delegate ID
                    case "AllowChangefrmPurchaseReturnDelegateID":
                        MySession.GlobalAllowChangefrmPurchaseReturnDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow changing from purchase return debit account ID
                    case "AllowChangefrmPurchaseReturnDebitAccountID":
                        MySession.GlobalAllowChangefrmPurchaseReturnDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow changing from purchase return credit account ID
                    case "AllowChangefrmPurchaseReturnCreditAccountID":
                        MySession.GlobalAllowChangefrmPurchaseReturnCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow changing from purchase return additional account ID
                    case "AllowChangefrmPurchaseReturnAdditionalAccountID":
                        MySession.GlobalAllowChangefrmPurchaseReturnAdditionalAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow changing from purchase return discount credit account ID
                    case "AllowChangefrmPurchaseReturnDiscountCreditAccountID":
                        MySession.GlobalAllowChangefrmPurchaseReturnDiscountCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow changing from purchase return cheque account ID
                    case "AllowChangefrmPurchaseReturnChequeAccountID":
                        MySession.GlobalAllowChangefrmPurchaseReturnChequeAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow changing from purchase return net account ID
                    case "AllowChangefrmPurchaseReturnNetAccountID":
                        MySession.GlobalAllowChangefrmPurchaseReturnNetAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow changing from purchase return transport debit account ID
                    case "AllowChangefrmPurchaseReturnTransportDebitAccountID":
                        MySession.GlobalAllowChangefrmPurchaseReturnTransportDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow changing from purchase return supplier ID
                    case "AllowChangefrmPurchaseReturnSupplierID":
                        MySession.GlobalAllowChangefrmPurchaseReturnSupplierID = Comon.cbool(row["OtherPermissionValue"]);
                        break;

                    // Allow changing from purchase return invoice net price
                    case "AllowChangefrmPurchaseReturnInvoiceNetPrice":
                        MySession.GlobalAllowChangefrmPurchaseReturnInvoiceNetPrice = Comon.cbool(row["OtherPermissionValue"]);
                        break;

                    /************ Purchase Return Default **************/

                    // Set default purchase return currency ID
                    case "DefaultPurchaseReturnCurencyID":
                        MySession.GlobalDefaultPurchaseReturnCurencyID = row["OtherPermissionValue"].ToString();
                        break;

                    // Set default purchase return supplier ID
                    case "DefaultPurchaseReturnSupplierID":
                        MySession.GlobalDefaultPurchaseReturnSupplierID = row["OtherPermissionValue"].ToString();
                        break;

                    // Set default purchase return store ID
                    case "DefaultPurchaseReturnStoreID":
                        MySession.GlobalDefaultPurchaseReturnStoreID = row["OtherPermissionValue"].ToString();
                        break;

                    // Set default purchase return cost center ID
                    case "DefaultPurchaseReturnCostCenterID":
                        MySession.GlobalDefaultPurchaseReturnCostCenterID = row["OtherPermissionValue"].ToString();
                        break;

                    // Set default purchase return pay method ID
                    case "DefaultPurchaseReturnPayMethodID":
                        MySession.GlobalDefaultPurchaseReturnPayMethodID = row["OtherPermissionValue"].ToString();
                        break;

                    // Set default purchase return net type ID
                    case "DefaultPurchaseReturnNetTypeID":
                        MySession.GlobalDefaultPurchaseReturnNetTypeID = row["OtherPermissionValue"].ToString();
                        break;
                    // Set default purchase return delegate ID
                    case "DefaultPurchaseReturnDelegateID":
                        MySession.GlobalDefaultPurchaseReturnDelegateID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPurchaseReturnDebitAccountID":
                        MySession.GlobalDefaultPurchaseReturnDebitAccountID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultPurchaseReturnCrditAccountID":
                        MySession.GlobalDefaultPurchaseReturnCrditAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    /******************************* Sale ********************************/
                    /************ Sale Allow **************/
                    // Allow Change form Sale Invoice Date
                    case "AllowChangefrmSaleInvoiceDate":
                        MySession.GlobalAllowChangefrmSaleInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Store ID
                    case "AllowChangefrmSaleStoreID":
                        MySession.GlobalAllowChangefrmSaleStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Cost Center ID
                    case "AllowChangefrmSaleCostCenterID":
                        MySession.GlobalAllowChangefrmSaleCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Pay Method ID
                    case "AllowChangefrmSalePayMethodID":
                        MySession.GlobalAllowChangefrmSalePayMethodID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Curency ID
                    case "AllowChangefrmSaleCurencyID":
                        MySession.GlobalAllowChangefrmSaleCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Net Type ID
                    case "AllowChangefrmSaleNetTypeID":
                        MySession.GlobalAllowChangefrmSaleNetTypeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Delegate ID
                    case "AllowChangefrmSaleDelegateID":
                        MySession.GlobalAllowChangefrmSaleDelegateID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Debit Account ID
                    case "AllowChangefrmSaleDebitAccountID":
                        MySession.GlobalAllowChangefrmSaleDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Credit Account ID
                    case "AllowChangefrmSaleCreditAccountID":
                        MySession.GlobalAllowChangefrmSaleCreditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    /* Allow Change form Sale Additional Account ID */
                    case "AllowChangefrmSaleAdditionalAccountID":
                        MySession.GlobalAllowChangefrmSaleAdditionalAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    // Allow Change form Sale Discount Debit Account ID
                    case "AllowChangefrmSaleDiscountDebitAccountID":
                        MySession.GlobalAllowChangefrmSaleDiscountDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    /* Allow Change form Sale Cheque Account ID */
                    case "AllowChangefrmSaleChequeAccountID":
                        MySession.GlobalAllowChangefrmSaleChequeAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    // Allow Change form Sale Net Account ID
                    case "AllowChangefrmSaleNetAccountID":
                        MySession.GlobalAllowChangefrmSaleNetAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    /* Allow Change form Sale Customer ID */
                    case "AllowChangefrmSaleCustomerID":
                        MySession.GlobalAllowChangefrmSaleCustomerID = Comon.cbool(row["OtherPermissionValue"]);
                        break;
                    // Allow Change form Sale Invoice Net Price
                    case "AllowChangefrmSaleInvoiceNetPrice":
                        MySession.GlobalAllowChangefrmSaleInvoiceNetPrice = Comon.cbool(row["OtherPermissionValue"]);
                        break;
                    /* Allow Change form Sale Seller ID */
                    case "AllowChangefrmSaleSellerID":
                        MySession.GlobalAllowChangefrmSaleSellerID = Comon.cbool(row["OtherPermissionValue"]);
                        break;
                    /************ sale Default **************/
                    // set global default sale currency ID
                    case "DefaultSaleCurencyID":
                        MySession.GlobalDefaultSaleCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    // set global default sale customer ID
                    case "DefaultSaleCustomerID":
                        MySession.GlobalDefaultSaleCustomerID = row["OtherPermissionValue"].ToString();
                        break;
                    // set global default sale store ID
                    case "DefaultSaleStoreID":
                        MySession.GlobalDefaultSaleStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    // set global default sale cost center ID
                    case "DefaultSaleCostCenterID":
                        MySession.GlobalDefaultSaleCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    // set global default sale payment method ID
                    case "DefaultSalePayMethodID":
                        MySession.GlobalDefaultSalePayMethodID = row["OtherPermissionValue"].ToString();
                        break;
                    // set global default sale net type ID
                    case "DefaultSaleNetTypeID":
                        MySession.GlobalDefaultSaleNetTypeID = row["OtherPermissionValue"].ToString();
                        break;
                    // set global default sale delegate ID
                    case "DefaultSaleDelegateID":
                        MySession.GlobalDefaultSaleDelegateID = row["OtherPermissionValue"].ToString();
                        break;
                    // set global default sale seller ID
                    case "DefaultSaleSellerID":
                        MySession.GlobalDefaultSaleSellerID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultSaleCreditAccountID":
                        MySession.GlobalDefaultSaleCreditAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultSaleDebitAccountID":
                        MySession.GlobalDefaultSaleDebitAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultSalesRevenueAccountID":
                        MySession.GlobalDefaultSalesRevenueAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultSalesAddtionalAccountID":
                        MySession.GlobalDefaultSalesAddtionalAccountID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultCostSalseAccountID":
                        MySession.GlobalDefaultCostSalseAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultDiscountSalseAccountID":
                        MySession.GlobalDefaultDiscountSalseAccountID = row["OtherPermissionValue"].ToString();
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
                    /************Order sale Default **************/

                    case "DefaultOrderSaleCurrncyID":
                        MySession.GlobalDefaultOrderSaleCurrncyID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultOrderSaleCustomerID":
                        MySession.GlobalDefaultOrderSaleCustomerID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultOrderSaleStoreID":
                        MySession.GlobalDefaultOrderSaleStoreID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultOrderSaleCostCenterID":
                        MySession.GlobalDefaultOrderSaleCostCenterID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultOrderSaleDelegateID":
                        MySession.GlobalDefaultOrderSaleDelegateID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultSaleReturnCreditAccountID":
                        MySession.GlobalDefaultSaleReturnCreditAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    /*****************Order Sales Allow******/
                    case "AllowChangefrmOrderSaleCurencyID ":
                        MySession.GlobalAllowChangefrmOrderSaleCurencyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmOrderSaleDelegeteID":
                        MySession.GlobalAllowChangefrmOrderSaleDelegeteID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmOrderSaleCustomerID":
                        MySession.GlobalAllowChangefrmOrderSaleCustomerID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmOrderSaleCostCenterID":
                        MySession.GlobalAllowChangefrmOrderSaleCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmOrderSaleDate":
                        MySession.GlobalAllowChangefrmOrderSaleDate = Comon.cbool(row["OtherPermissionValue"]);
                        break;
                    case "AllowChangefrmOrderSaleStoreID":
                        MySession.GlobalAllowChangefrmOrderSaleStoreID = Comon.cbool(row["OtherPermissionValue"]);
                        break;
                    case "AllowChangefrmOrderSaleSellerID":
                        MySession.GlobalAllowChangefrmOrderSaleSellerID = Comon.cbool(row["OtherPermissionValue"]);
                        break;

                    /************ sale return Default **************/

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
                    case "DefaultSpendVoucherCrditAccountID":
                        MySession.GlobalDefaultSpendVoucherCrditAccountID = row["OtherPermissionValue"].ToString();
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
                    case "DefaultCheckSpendVoucherCrditAccountID":
                        MySession.GlobalDefaultCheckSpendVoucherCrditAccountID = row["OtherPermissionValue"].ToString();
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
                    case "DefaultReceiptVoucherDebitAccountID":
                        MySession.GlobalDefaultReceiptVoucherDebitAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultReceiptVoucherIntermediateDiamondAccountID":
                        MySession.GlobalDefaultReceiptVoucherIntermediateDiamondAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultReceiptVoucherIntermediateGoldAccountID":
                        MySession.GlobalDefaultReceiptVoucherIntermediateGoldAccountID = row["OtherPermissionValue"].ToString();
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
                    case "DefaultCheckReceiptVoucherDebitAccountID":
                        MySession.GlobalDefaultCheckReceiptVoucherDebitAccountID = row["OtherPermissionValue"].ToString();
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
                        MySession.GlobalDefaulGoodsOpeningStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultGoodsOpeningCostCenterID":
                        MySession.GlobalDefaultGoodsOpeningCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaulGoodsOpeningCrditAccountID":
                        MySession.GlobalDefaulGoodsOpeningCrditAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaulGoodsOpeningDebitAccountID":
                        MySession.GlobalDefaulGoodsOpeningDebitAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    /******************************* Items In On Bail ********************************/
                    /************ Gold In On Bail Allow **************/

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
                    /************ Gold In On Bail Default **************/

                    case "DefaultItemsInOnBailCurencyID":
                        MySession.GlobalDefaultItemsInOnBailCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultItemsInOnBailCostCenterID":
                        MySession.GlobalDefaultItemsInOnBailCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultItemsInOnBailSupplierID":
                        MySession.GlobalDefaultItemsInOnBailSupplierID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultItemsInOnBailStoreID":
                        MySession.GlobalDefaultItemsInOnBailStoreID = row["OtherPermissionValue"].ToString();
                        break;

                    /************ Gold In On Bail Allow **************/

                    case "AllowChangefrmMatirialInOnBailInvoiceDate":
                        MySession.GlobalAllowChangefrmMatirialInOnBailInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialInOnBailStoreID":
                        MySession.GlobalAllowChangefrmMatirialInOnBailStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialInOnBailCostCenterID":
                        MySession.GlobalAllowChangefrmMatirialInOnBailCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialInOnBailCurrncyID":
                        MySession.GlobalAllowChangefrmMatirialInOnBailCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialInOnBailSupplier":
                        MySession.GlobalAllowChangefrmMatirialInOnBailSupplier = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialInOnBailCrditAccountID":
                        MySession.GlobalAllowChangefrmMatirialInOnBailCrditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialInOnBailDebitAccountID":
                        MySession.GlobalAllowChangefrmMatirialInOnBailDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    /************ Matirial In On Bail Default **************/

                    case "DefaultMatirialInOnBailCurencyID":
                        MySession.GlobalDefaultMatirialInOnBailCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultMatirialInOnBailCostCenterID":
                        MySession.GlobalDefaultMatirialInOnBailCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultMatirialInOnBailSupplierID":
                        MySession.GlobalDefaultMatirialInOnBailSupplierID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultMatirialInOnBailStoreAccountID":
                        MySession.GlobalDefaultMatirialInOnBailStoreAccountID = row["OtherPermissionValue"].ToString();
                        break;

                    /************ Gold Multi Store Default **************/

                    case "DefaultGoldMultiTransferCurencyID":
                        MySession.GlobalDefaultGoldMultiTransferCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultGoldMultiTransferCostCenterID":
                        MySession.GlobalDefaultGoldMultiTransferCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultGoldMultiTransferStoreID":
                        MySession.GlobalDefaultGoldMultiTransferStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    /************  Gold Multi Store Allow **************/

                    case "AllowChangefrmMaltiTransferCurrncyID":
                        MySession.GlobalAllowChangefrmMaltiTransferCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMaltiTransferCostCenterID":
                        MySession.GlobalAllowChangefrmMaltiTransferCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMaltiTransferStoreID":
                        MySession.GlobalAllowChangefrmMaltiTransferStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMaltiTransferInvoiceDate":
                        MySession.GlobalAllowChangefrmMaltiTransferInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    /************ Matirial Multi Store Default **************/

                    case "DefaultMatirialMultiTransferCurencyID":
                        MySession.GlobalDefaultMatirialMultiTransferCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultMatirialMultiTransferCostCenterID":
                        MySession.GlobalDefaultMatirialMultiTransferCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultMatirialMultiTransferStoreID":
                        MySession.GlobalDefaultMatirialMultiTransferStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    /************  Matirial Multi Store Allow **************/

                    case "AllowChangefrmMatirialMaltiTransferCurrncyID":
                        MySession.GlobalAllowChangefrmMatirialMaltiTransferCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialMaltiTransferCostCenterID":
                        MySession.GlobalAllowChangefrmMatirialMaltiTransferCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialMaltiTransferStoreID":
                        MySession.GlobalAllowChangefrmMatirialMaltiTransferStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialMaltiTransferInvoiceDate":
                        MySession.GlobalAllowChangefrmMatirialMaltiTransferInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    /************ Matirial In out on Default **************/

                    case "DefaultMatirialOutOnBailCurencyID":
                        MySession.GlobalDefaultMatirialOutOnBailCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultMatirialOutOnBailCostCenterID":
                        MySession.GlobalDefaultMatirialOutOnBailCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultMatirialOutOnBailStoreID":
                        MySession.GlobalDefaultMatirialOutOnBailStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultMatirialOutOnBailSupplierID":
                        MySession.GlobalDefaultMatirialOutOnBailSupplierID = row["OtherPermissionValue"].ToString();
                        break;
                    /************ matirial out On Bail Allow **************/

                    case "AllowChangefrmMatirialOutOnBailInvoiceDate":
                        MySession.GlobalAllowChangefrmMatirialOutOnBailInvoiceDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialOutOnBailStoreID":
                        MySession.GlobalAllowChangefrmMatirialOutOnBailStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialOutOnBailCostCenterID":
                        MySession.GlobalAllowChangefrmMatirialOutOnBailCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialOutOnBailCurrncyID":
                        MySession.GlobalAllowChangefrmMatirialOutOnBailCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialOutOnBailSupplierID":
                        MySession.GlobalAllowChangefrmMatirialOutOnBailSupplierID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialOutOnBailCrditAccountID":
                        MySession.GlobalAllowChangefrmMatirialOutOnBailCrditAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmMatirialOutOnBailDebitAccountID":
                        MySession.GlobalAllowChangefrmMatirialOutOnBailDebitAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
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
                        MySession.GlobalAllowUsingDateItems = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    /************ AllowOutItemsWithOutBalance**************/
                    case "AllowOutItemsWithOutBalance":
                        MySession.GlobalAllowOutItemsWithOutBalance = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "DefaultFatherSupplierAccountID":
                        MySession.GlobalDefaultParentSupplierAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultFatherCustomerAccountID":
                        MySession.GlobalDefaultParentCustomerAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultFatherStoreAccountID":
                        MySession.GlobalDefaultParentStoreAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultFatherBoxesAccountID":
                        MySession.GlobalDefaultParentBoxesAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultFatherBanksAccountID":
                        MySession.GlobalDefaultParentBanksAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultFatherEmployeeAccountID":
                        MySession.GlobalDefaultParentEmployeeAccountID = row["OtherPermissionValue"].ToString();
                        break;

                    /*******************ManuFactory**********************************/
                    case "DefaultCanRepetUseOrderOnOureMoreBeforeCasting":
                        MySession.GlobalDefaultCanRepetUseOrderOneOureMoreBeforeCasting = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "DefaultCanRepetUseOrderOnOureMoreManufactory":
                        MySession.GlobalDefaultCanRepetUseOrderOneOureMoreManufactory = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    //defult Wax
                    case "DefaultWaxCurencyID":
                        MySession.GlobalDefaultWaxCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultWaxCostCenterID":
                        MySession.GlobalDefaultWaxCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultWaxBeforeStoreAccontID":
                        MySession.GlobalDefaultWaxBeforeStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultWaxAfterStoreAccontID":
                        MySession.GlobalDefaultWaxAfterStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultWaxEmployeeID":
                        MySession.GlobalDefaultWaxEmployeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Wax
                    case "AllowChangefrmWaxAfterStoreID":
                        MySession.GlobalAllowChangefrmWaxAfterStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmWaxCommandDate":
                        MySession.GlobalAllowChangefrmWaxCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmWaxBeforeStoreID":
                        MySession.GlobalAllowChangefrmWaxBeforeStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmWaxBranchID":
                        MySession.GlobalAllowChangefrmWaxBranchID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmWaxEmployeeID":
                        MySession.GlobalAllowChangefrmWaxEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmWaxCostCenterID":
                        MySession.GlobalAllowChangefrmWaxCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmWaxCurrncyID":
                        MySession.GlobalAllowChangefrmWaxCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    //defult Cad
                    case "DefaultCadCurencyID":
                        MySession.GlobalDefaultCadCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCadCostCenterID":
                        MySession.GlobalDefaultCadCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCadBeforeStoreAccontID":
                        MySession.GlobalDefaultCadBeforeStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCadAfterStoreAccontID":
                        MySession.GlobalDefaultCadAfterStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCadEmpolyeeID":
                        MySession.GlobalDefaultCadEmpolyeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Cad
                    case "AllowChangefrmCadAfterStoreID":
                        MySession.GlobalAllowChangefrmCadAfterStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCadCommandDate":
                        MySession.GlobalAllowChangefrmCadCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCadBeforeStoreID":
                        MySession.GlobalAllowChangefrmCadBeforeStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCadBranchID":
                        MySession.GlobalAllowChangefrmCadBranchID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCadEmployeeID":
                        MySession.GlobalAllowChangefrmCadEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCadCostCenterID":
                        MySession.GlobalAllowChangefrmCadCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCadCurrncyID":
                        MySession.GlobalAllowChangefrmCadCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    //defult Zircon
                    case "DefaultZirconCurencyID":
                        MySession.GlobalDefaultZirconCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultZirconCostCenterID":
                        MySession.GlobalDefaultZirconCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultZirconBeforeStoreAccontID":
                        MySession.GlobalDefaultZirconBeforeStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultZirconAfterStoreAccontID":
                        MySession.GlobalDefaultZirconAfterStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultZirconEmpolyeeID":
                        MySession.GlobalDefaultZirconEmpolyeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Zircon
                    case "AllowChangefrmZirconAfterStoreID":
                        MySession.GlobalAllowChangefrmZirconAfterStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmZirconCommandDate":
                        MySession.GlobalAllowChangefrmZirconCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmZirconBeforeStoreID":
                        MySession.GlobalAllowChangefrmZirconBeforeStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmZirconBranchID":
                        MySession.GlobalAllowChangefrmZirconBranchID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmZirconEmployeeID":
                        MySession.GlobalAllowChangefrmZirconEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmZirconCostCenterID":
                        MySession.GlobalAllowChangefrmZirconCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmZirconCurrncyID":
                        MySession.GlobalAllowChangefrmZirconCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    //defult Diamond
                    case "DefaultDiamondCurencyID":
                        MySession.GlobalDefaultDiamondCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultDiamondCostCenterID":
                        MySession.GlobalDefaultDiamondCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultDiamondBeforeStoreAccontID":
                        MySession.GlobalDefaultDiamondBeforeStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultDiamondAfterStoreAccontID":
                        MySession.GlobalDefaultDiamondAfterStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultDiamondEmpolyeeID":
                        MySession.GlobalDefaultDiamondEmpolyeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Diamond
                    case "AllowChangefrmDiamondAfterStoreID":
                        MySession.GlobalAllowChangefrmDiamondAfterStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDiamondCommandDate":
                        MySession.GlobalAllowChangefrmDiamondCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDiamondBeforeStoreID":
                        MySession.GlobalAllowChangefrmDiamondBeforeStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDiamondBranchID":
                        MySession.GlobalAllowChangefrmDiamondBranchID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDiamondEmployeeID":
                        MySession.GlobalAllowChangefrmDiamondEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDiamondCostCenterID":
                        MySession.GlobalAllowChangefrmDiamondCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDiamondCurrncyID":
                        MySession.GlobalAllowChangefrmDiamondCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    //defult Afforstation
                    case "DefaultAfforstationCurencyID":
                        MySession.GlobalDefaultAfforstationCurencyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultAfforstationCostCenterID":
                        MySession.GlobalDefaultAfforstationCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultAfforstationBeforeStoreAccontID":
                        MySession.GlobalDefaultAfforstationBeforeStoreAccontID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultAfforstationAccountID":
                        MySession.GlobalDefaultAfforstationAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultAfforstationBeforeEmpolyeeID":
                        MySession.GlobalDefaultAfforstationBeforeEmpolyeeID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultAfforstationAfterEmpolyeeID":
                        MySession.GlobalDefaultAfforstationAfterEmpolyeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Afforstation
                    case "AllowChangefrmAfforstationAfterStoreID":
                        MySession.GlobalAllowChangefrmAfforstationAfterStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAfforstationCommandDate":
                        MySession.GlobalAllowChangefrmAfforstationCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAfforstationBeforeStoreID":
                        MySession.GlobalAllowChangefrmAfforstationBeforeStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAfforstationBranchID":
                        MySession.GlobalAllowChangefrmAfforstationBranchID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAfforstationBeforeEmployeeID":
                        MySession.GlobalAllowChangefrmAfforstationBeforeEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAfforstationAfterEmployeeID":
                        MySession.GlobalAllowChangefrmAfforstationAfterEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAfforstationCostCenterID":
                        MySession.GlobalAllowChangefrmAfforstationCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAfforstationCurrncyID":
                        MySession.GlobalAllowChangefrmAfforstationCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;


                    //defult Casting
                    case "DefaultCastingCurrncyID":
                        MySession.GlobalDefaultCastingCurrncyID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCastingCostCenterID":
                        MySession.GlobalDefaultCastingCostCenterID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCastingStoreID":
                        MySession.GlobalDefaultCastingStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCastingAccountID":
                        MySession.GlobalDefaultCastingAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCastingEmployeeID":
                        MySession.GlobalDefaultCastingEmployeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Casting
                    case "AllowChangefrmCastingCommandDate":
                        MySession.GlobalAllowChangefrmCastingCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCastingStoreID":
                        MySession.GlobalAllowChangefrmCastingStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCastingAccountID":
                        MySession.GlobalAllowChangefrmCastingAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCastingEmployeeID":
                        MySession.GlobalAllowChangefrmCastingEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCastingCostCenterID":
                        MySession.GlobalAllowChangefrmCastingCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCastingCurrncyID":
                        MySession.GlobalAllowChangefrmCastingCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    //defult Manufactory

                    case "DefaultManufactoryCurrncyID":
                        MySession.GlobalDefaultManufactoryCurrncyID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultManufactoryStoreID":
                        MySession.GlobalDefaultManufactoryStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultManufactoryAccountID":
                        MySession.GlobalDefaultManufactoryAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultManufatoryEmployeeID":
                        MySession.GlobalDefaultManufatoryEmployeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Commpound

                    case "AllowChangefrmManufactoryCommandDate":
                        MySession.GlobalAllowChangefrmManufactoryCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmManufactoryStoreID":
                        MySession.GlobalAllowChangefrmManufactoryStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmManufatoryAccountID":
                        MySession.GlobalAllowChangefrmManufatoryAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmManufactoryEmployeeID":
                        MySession.GlobalAllowChangefrmManufactoryEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmManufactoryCurrncyID":
                        MySession.GlobalAllowChangefrmManufactoryCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    //defult Compound

                    case "DefaultCommpundCurrncyID":
                        MySession.GlobalDefaultCommpundCurrncyID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultCompoundStoreID":
                        MySession.GlobalDefaultCompoundStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCompoundAccountID":
                        MySession.GlobalDefaultCompoundAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultCompoundEmployeeID":
                        MySession.GlobalDefaultCompoundEmployeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Compound

                    case "AllowChangefrmCompundCommandDate":
                        MySession.GlobalAllowChangefrmCompundCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCompoundStoreID":
                        MySession.GlobalAllowChangefrmCompoundStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCompoundAccountID":
                        MySession.GlobalAllowChangefrmCompoundAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmCompoundEmployeeID":
                        MySession.GlobalAllowChangefrmCompoundEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmCompundCurrncyID":
                        MySession.GlobalAllowChangefrmCompundCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;



                    //defult Prntage

                    case "DefaultPrntageCurrncyID":
                        MySession.GlobalDefaultPrntageCurrncyID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultPrntageStoreID":
                        MySession.GlobalDefaultPrntageStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPrntageAccountID":
                        MySession.GlobalDefaultPrntageAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPrntage2StoreID":
                        MySession.GlobalDefaultPrntage2StoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPrntage2AccountID":
                        MySession.GlobalDefaultPrntage2AccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPrntageEmployeeID":
                        MySession.GlobalDefaultPrntageEmployeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Prntage

                    case "AllowChangefrmPrntageCommandDate":
                        MySession.GlobalAllowChangefrmPrntageCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPrntageStoreID":
                        MySession.GlobalAllowChangefrmPrntageStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPrntageAccountID":
                        MySession.GlobalAllowChangefrmPrntageAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPrntage2StoreID":
                        MySession.GlobalAllowChangefrmPrntage2StoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPrntage2AccountID":
                        MySession.GlobalAllowChangefrmPrntage2AccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPrntageEmployeeID":
                        MySession.GlobalAllowChangefrmPrntageEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmPrntageCurrncyID":
                        MySession.GlobalAllowChangefrmPrntageCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    //defult Polishin

                    case "DefaultPolishinCurrncyID":
                        MySession.GlobalDefaultPolishinCurrncyID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultPolishinStoreID":
                        MySession.GlobalDefaultPolishinStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPolishinAccountID":
                        MySession.GlobalDefaultPolishinAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPolishin2StoreID":
                        MySession.GlobalDefaultPolishin2StoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPolishin2AccountID":
                        MySession.GlobalDefaultPolishin2AccountID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultPolishin3StoreID":
                        MySession.GlobalDefaultPolishin3StoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPolishin3AccountID":
                        MySession.GlobalDefaultPolishin3AccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultPolishnEmployeeID":
                        MySession.GlobalDefaultPolishnEmployeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Prntage

                    case "AllowChangefrmPolishnCommandDate":
                        MySession.GlobalAllowChangefrmPolishnCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPolishnStoreID":
                        MySession.GlobalAllowChangefrmPolishnStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPolishnAccountID":
                        MySession.GlobalAllowChangefrmPolishnAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPolishn2StoreID":
                        MySession.GlobalAllowChangefrmPolishn2StoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPolishn2AccountID":
                        MySession.GlobalAllowChangefrmPolishn2AccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPolishn3StoreID":
                        MySession.GlobalAllowChangefrmPolishn3StoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPolishn3AccountID":
                        MySession.GlobalAllowChangefrmPolishn3AccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmPolishnEmployeeID":
                        MySession.GlobalAllowChangefrmPolishnEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmPolishnCurrncyID":
                        MySession.GlobalAllowChangefrmPolishnCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;


                    //defult Addtional

                    case "DefaultAddtionalCurrncyID":
                        MySession.GlobalDefaultAddtionalCurrncyID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultAddtionalStoreID":
                        MySession.GlobalDefaultAddtionalStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultAddtionalAccountID":
                        MySession.GlobalDefaultAddtionalAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultAddtionalEmployeeID":
                        MySession.GlobalDefaultAddtionalEmployeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Addtional

                    case "AllowChangefrmAddtionalCommandDate":
                        MySession.GlobalAllowChangefrmAddtionalCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAddtionalStoreID":
                        MySession.GlobalAllowChangefrmAddtionalStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAddtionalAccountID":
                        MySession.GlobalAllowChangefrmAddtionalAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmAddtionalEmployeeID":
                        MySession.GlobalAllowChangefrmAddtionalEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowChangefrmAddtionalCurrncyID":
                        MySession.GlobalAllowChangefrmAddtionalCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    //defult Dismant
                    case "DefaultDismantageCurrncyID":
                        MySession.GlobalDefaultDismantageCurrncyID = row["OtherPermissionValue"].ToString();
                        break;

                    case "DefaultDismantageStoreID":
                        MySession.GlobalDefaultDismantageStoreID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultDismantageAccountID":
                        MySession.GlobalDefaultDismantageAccountID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultDismantageEmployeeID":
                        MySession.GlobalDefaultDismantageEmployeeID = row["OtherPermissionValue"].ToString();
                        break;
                    //Allow Dismant

                    case "AllowChangefrmDismantageCommandDate":
                        MySession.GlobalAllowChangefrmDismantageCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDismantageStoreID":
                        MySession.GlobalAllowChangefrmDismantageStoreID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDismantageAccountID":
                        MySession.GlobalAllowChangefrmDismantageAccountID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDismantageEmployeeID":
                        MySession.GlobalAllowChangefrmDismantageEmployeeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmItemsDismantlingCostCenterID":
                        MySession.GlobalAllowChangefrmItemsDismantlingCostCenterID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmDismanatgeCurrncyID":
                        MySession.GlobalAllowChangefrmDismanatgeCurrncyID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;


                        //*****************Restriction Order
                    case "AllowChangefrmOrderRestrctionCommandDate":
                        MySession.GlobalAllowChangefrmOrderRestrctionCommandDate = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmOrderRestrctionTypeID":
                        MySession.GlobalAllowChangefrmOrderRestrctionTypeID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                    case "AllowChangefrmOrderRestrctionTypeMatirialID":
                        MySession.GlobalAllowChangefrmOrderRestrctionTypeMatirialID = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;
                        //Defult
                    case "DefaultTypeOrderRestrectionID":
                        MySession.GlobalDefaultTypeOrderRestrectionID = row["OtherPermissionValue"].ToString();
                        break;
                    case "DefaultTypeMatirialOrderRestrectionID":
                        MySession.GlobalDefaultTypeMatirialOrderRestrectionID = row["OtherPermissionValue"].ToString();
                        break;
                     

                    // Allow Out Qty negative
                    case "AllowOutQtyNegative":
                        MySession.AllowOutQtyNegative = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "AllowNotShowQTYInQtyField":
                        MySession.AllowNotShowQTYInQtyField = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "CanChangeBranchModificationAllScreens":
                        MySession.GlobalAllowBranchModificationAllScreens = Comon.cbool(row["OtherPermissionValue"].ToString());
                        break;

                    case "DefaultProcessPostedStatus":
                        MySession.GlobalDefaultProcessPostedStatus = row["OtherPermissionValue"].ToString();
                        break;
                }
                #endregion
            }
        }
      /// <summary>
        /// This is the event that executes at Click time for login Button
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        private void btnlogin_Click(object sender, EventArgs e)
       {
         
            if( cmbBranchesID.EditValue==null|| Comon.cInt(cmbBranchesID.EditValue)<=0)
            {
              XtraMessageBox.Show(  cmbLangauage.EditValue.ToString() == "ar" ? "الرجاء إختيار الفرع" : " Please Select The Branch! ");
                return;
            }
            //Set Values To MySession Proprites
            MySession.GlobalQtyDigits = 2;
            MySession.GlobalPercentVat = 15; 
            MySession.GlobalDefaultSaleCurencyID = "13";
            MySession.GlobalBranchID = Comon.cInt(cmbBranchesID.EditValue) ;
            MySession.GlobalFacilityID = UserInfo.FacilityID;
            UserInfo.ComputerInfo = Environment.MachineName;
            UserBO ISVaild = null;
            if (Comon.cInt(cmbDataBaseName.EditValue.ToString()) == 0)
            {
                Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.English ? " you Shoud Select DataBase Name? " : "? يجب عليك اختيار قاعدة البيانات  "));
                return;
            }
            try
            {

                SplashScreenManager.CloseForm(false);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
               
                ISVaild = LoginValidation.Login(txtUserID.Text.Trim(), txtPassword.Text.Trim(),Comon.cInt( cmbBranchesID.EditValue ));
                if (ISVaild != null)
                {
                    SetMySession(Comon.cInt(txtUserID.Text), Comon.cInt(UserInfo.BRANCHID));

                    // Set the GlobalBranchID and GlobalFacilityID
                    MySession.GlobalBranchID = Comon.cInt(cmbBranchesID.EditValue) ;
                    MySession.GlobalFacilityID = ISVaild.FacilityID;

                    // Initialize the Messages and set GlobalFacilityName and GlobalBranchName
                    Messages.initialization(MySession.GlobalLanguageName);
                    MySession.GlobalFacilityName = ISVaild.FacilityName;
                    MySession.GlobalBranchName = ISVaild.BranchName;

                    // Declare and initialize an array of bool
                    bool[] arrbool = new bool[10];

                    // Set the GlobalAllowChangefrmPurchaseInvoiceDate and assign its value to arrbool[0]
                 
                    arrbool[0] = MySession.GlobalAllowChangefrmPurchaseInvoiceDate;

                    // Get the General Settings by ID and FacilityID and set their values to MySession properties
                    GeneralSettings generalSettting = GeneralSettingsDAL.GetDataByID(UserInfo.FacilityID,MySession.GlobalBranchID, UserInfo.FacilityID);
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
                        MySession.GlobalUsingExpiryDate = Comon.cbool(generalSettting.UsingExpiryDate);
                        MySession.GlobalInventoryType = generalSettting.InventoryType;
                       
                    }

                    // Select CompanyVATID, cost, and sumvalue from VATIDCOMPANY and assign them to MySession properties
                    string VAt = "Select CompanyVATID,cost,sumvalue from VATIDCOMPANY ";
                    DataTable dVat = Lip.SelectRecord(VAt);
                    MySession.VAtCompnyGlobal = dVat.Rows[0][0].ToString();


                    MySession.Cost = dVat.Rows[0]["cost"].ToString();
                    MySession.sumvalue = dVat.Rows[0]["sumvalue"].ToString();

                    //frmMainEdexaimondaimond frm = new frmMainEdexaimondaimond();
                    string startupPath = Directory.GetCurrentDirectory() + "\\";
                    var TypeCustomer = new FileStream(@startupPath + "TypeCustomer.txt", FileMode.Open, FileAccess.Read);
                    var text = "";
                    using (var streamReader = new StreamReader(TypeCustomer, Encoding.UTF8))
                    {
                        text = streamReader.ReadToEnd();
                    }




                    if (text == "1")
                    {

                        frmMainEdexaimond frm = new frmMainEdexaimond();
                        SplashScreenManager.CloseForm(false);
                        frm.Show();
                        this.Hide();
                    }

                    if (text == "2")
                    {
                        UserInfo.MainTyepScreen = Comon.cInt(Lip.GetValue("SELECT  [MainTyepScreen]   FROM  [Users] where UserID=" + Comon.cInt(txtUserID.Text)));
                        if (UserInfo.MainTyepScreen == 1)
                        {
                            frmMainEdexDaimondShop frm = new frmMainEdexDaimondShop();
                            SplashScreenManager.CloseForm(false);
                            frm.Show();
                            this.Hide();
                        }
                        else
                        {
                            frmMain frm = new frmMain();
                            MySession.DefultMainParent = frm;
                            SplashScreenManager.CloseForm(false);
                            frm.Show();
                            this.Hide();
                        }

                    }
                }
                else
                {
                    txtPassword.Text = "";
                    txtPassword.Focus();
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
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        /// <summary>
        /// This Event To Set DataBase Name To cConnectionString.DataBasename
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDataBaseName_EditValueChanged(object sender, EventArgs e)
        {
            // Check if the DataBaseName is not empty and the EditValue of the cmbDataBaseName is empty
            if (DataBaseName != "" && cmbDataBaseName.Properties.GetDisplayText(cmbDataBaseName.EditValue) == "")
                cConnectionString.DataBasename = DataBaseName;
            else
            {
                int value = Comon.cInt(cmbDataBaseName.EditValue.ToString());
                if (value == 0)
                    return;
                cConnectionString.DataBasename = cmbDataBaseName.Properties.GetDisplayText(cmbDataBaseName.EditValue);
            }
       
        }
        /// <summary>
        /// This Event To Closed System
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
       /// <summary>
       /// This Event To Close System
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// // This method is called when the link label is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open the specified URL in the default browser
            System.Diagnostics.Process.Start("http://omexpro4it.com/");
        }
        #endregion 
        #region Function
       /// <summary>
       /// This Function To Get Server Name From File ServerName.text and Set server Name and User name and DataBase Name To Proprites
       /// </summary>
        void GetServerName()
        {
            string text;
            string startupPath = Directory.GetCurrentDirectory() + "\\";
            var fileStream = new FileStream(@startupPath + "Server.txt", FileMode.Open, FileAccess.Read);
            var CustomeNamerFile = new FileStream(@startupPath + "CustomerName.txt", FileMode.Open, FileAccess.Read);
            var CustomeNamerType = new FileStream(@startupPath + "TypeCustomer.txt", FileMode.Open, FileAccess.Read);
            var UserName = new FileStream(@startupPath + "UserName.txt", FileMode.Open, FileAccess.Read);
            var DataBaseName = new FileStream(@startupPath + "DataBaseName.txt", FileMode.Open, FileAccess.Read);
            var PassWord = new FileStream(@startupPath + "PassWord.txt", FileMode.Open, FileAccess.Read);
            string CustomerType = "";
            string User = "";
            string DataBAse = "";

            string PassWordtxt = "";
            using (var streamReader = new StreamReader(PassWord, Encoding.UTF8))
            {
                PassWordtxt = streamReader.ReadToEnd();
            }
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }

            using (var streamReader = new StreamReader(CustomeNamerFile, Encoding.UTF8))
            {
                CustomerName = streamReader.ReadToEnd();
            }

            using (var streamReader = new StreamReader(CustomeNamerType, Encoding.UTF8))
            {
                CustomerType = streamReader.ReadToEnd();
            }

            using (var streamReader = new StreamReader(UserName, Encoding.UTF8))
            {
                User = streamReader.ReadToEnd();
            }

            using (var streamReader = new StreamReader(DataBaseName, Encoding.UTF8))
            {
                DataBAse = streamReader.ReadToEnd();
            }
            ConnectionHelper.ServerNamename = text.Trim();
            cConnectionString.ServerName = text.Trim();
            cConnectionString.DataBasename = DataBAse.Trim();
            cConnectionString.UserName = User.Trim();
            cConnectionString.PassWordtxt = PassWordtxt.Trim();
        }
       
        /// <summary>
        /// this Function To Get All database names on the server that is obtained by the function GetServerName and Set To  cmbDataBaseName
        /// </summary>
        void GetDatabaseList()
        {
            try
            {
                GetServerName();
                DataTable dtListDataBase = new DataTable();
                string ConString = ConnectionHelper.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConString))
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


                using (SqlConnection con = new SqlConnection(ConString))
                {
                    DataTable dt = new DataTable();
                    con.Open();
                    using (SqlCommand objCmd = con.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.Text;
                        objCmd.CommandText = "Select * from SMSSettings where ProgramName ='" + CustomerName + "'";
                        SqlDataReader myreader = objCmd.ExecuteReader();
                        dt.Load(myreader);

                        DataBaseName = cConnectionString.DataBasename;
                        DataBaseID = 1;
                        cmbDataBaseName.EditValue = 1;
                        //labelControl2.Text = cConnectionString.DataBasename;
                        lblDataBaseName.Text = cConnectionString.DataBasename;
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.MsgInfo("خطا في الإتصال", "يرجى التاكد من معلومات الأتصال     ");
                this.Close();
                Application.Exit();
            }
        }
        #endregion

        private void btnActive_Click(object sender, EventArgs e)
        {
            if (txtActivationNumber.Text == "")
                Application.Exit();

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\OurSettings");
            key.SetValue("OmexManuFactoryAcountSystem", txtActivationNumber.Text);
            Application.Exit();
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbBranchesID_EditValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}
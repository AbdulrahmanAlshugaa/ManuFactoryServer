using Edex.SalesAndSaleObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Transactions;
using System;
using System.Windows.Forms;
 
using Edex.DAL.Common;
using System.Configuration;
using Edex.ModelSystem;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralForms;
using Edex.StockObjects.Codes;
using Edex.StockObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.AccountsObjects.Codes;

using System.Diagnostics;
using Edex.GeneralObjects.GeneralClasses;
using Edex.SalesAndPurchaseObjects.Reports;
using Edex.StockObjects.StcMainScreen;
using Edex.StockObjects.Reports;
using Edex.AccountsObjects.Reports;
using Edex.AccountsObjects.Transactions;
using Edex.StockObjects.Transactions;
using Edex.DAL;

namespace Edex
{
    public partial class frmMainEdexaimond : DevExpress.XtraEditors.XtraForm
    {
        public frmMainEdexaimond()
        {
            InitializeComponent();
            InitSkinGallery();
            // GetSkinName();
            SetSkinName("Office 2007 Blue");
            txtCostCenterID.Text = MySession.GlobalDefaultCostCenterID;
            string strSQL = "SELECT ArbName as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
            CSearch.ControlValidating(txtCostCenterID, lblCostCenter, strSQL);
            lblCostCenter.Text =  lblCostCenter.Text;
            MySession.GlobalPercentVat = 15;

            lblCompany.Text = UserInfo.FacilityName;
            lblBranch.Text = UserInfo.BranchName;
            lblDataBaseName.Text = cConnectionString.DataBasename;
            labelControl1.Text = UserInfo.UserName;

        }
        void InitSkinGallery()
        {
            // SkinHelper.InitSkinGallery(mnufrmColorSeting, true);
        }

        void SetSkinName(string SkinName)
        {
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            SystemSettings.SetSkinName(SkinName, Path);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void btnItmfrmSalesInvoice_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

            frmCashierSalesAlmas frm = new frmCashierSalesAlmas();

            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void btnItmfrmPurchaseInvoice_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmSalesInvoice frm = new frmSalesInvoice();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

            frm.Text = "فاتورة مشتريات الماس";
          }
        private void btnItmfrmSalesInvoiceReturn_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmItemsGroups frm = new frmItemsGroups();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        private void frmMainEdex_Load(object sender, EventArgs e)
        {
            tileNavItem20.Visible = false;
            pnlSetting.Top = this.Height - pnlSetting.Height*3 - 30 ;
            pnlSetting.Left= 150;
            mnufrmItemsSizes.Visible = true;
        }

        
        private void mnufrmBranch_TileClick(object sender, EventArgs e)
        {
            frmBranches frm = new frmBranches();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmGeneralOptions_TileClick(object sender, EventArgs e)
        {


            frmGeneralOptions frm = new frmGeneralOptions();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();


        }
        private void mnufrmNotifications_TileClick(object sender, EventArgs e)
        {

            frmNotifications frm = new frmNotifications();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

            
        }
        private void mnufrmRestoringDeleted_TileClick(object sender, EventArgs e)
        { 
            frmRestoringDeleted frm = new frmRestoringDeleted();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmLoginHistory_TileClick(object sender, EventArgs e)
        {
            frmLoginHistory frm = new frmLoginHistory();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }
        private void mnufrmDeclaringMainAccounts_TileClick(object sender, EventArgs e)
        {

            frmDeclaringMainAccounts frm = new frmDeclaringMainAccounts();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
            
        }
        private void mnufrmDeclaringIncomeAccounts_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmColorSeting_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmUser_TileClick(object sender, EventArgs e)
        { 
            frmUser frm = new frmUser();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmUserPermissions_TileClick(object sender, EventArgs e)
        {
            frmUserPermissions frm = new frmUserPermissions();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmPrinterSelecter_TileClick(object sender, EventArgs e)
        {
             
            frmPrinterSelecter frm = new frmPrinterSelecter();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        private void mnufrmChangePassword_TileClick(object sender, EventArgs e)
        {

            frmChangePassword frm = new frmChangePassword();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
            

        }


        private void mnufrmCloseCashier_TileClick(object sender, EventArgs e)
        {


            frmCloseCashier frm = new frmCloseCashier();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
            
        }
        private void mnufrmBackupDataBase_TileClick(object sender, EventArgs e)
        { 

            frmBackupDataBase frm = new frmBackupDataBase();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmPrepareForNewAccountYear_TileClick(object sender, EventArgs e)
        {
            frmPrepareForNewAccountYear frm = new frmPrepareForNewAccountYear();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void tileNavItem49_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmAccountsTree_TileClick(object sender, EventArgs e)
        {
            frmAccountsTree frm = new frmAccountsTree();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmCostCenter_TileClick(object sender, EventArgs e)
        {
            frmCostCenter frm = new frmCostCenter();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();


        }
        private void mnufrmOpeningVoucher_TileClick(object sender, EventArgs e)
        {
            frmOpeningVoucher frm = new frmOpeningVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmReceiptVoucher_TileClick(object sender, EventArgs e)
        {
            frmReceiptVoucher frm = new frmReceiptVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmSpendVoucher_TileClick(object sender, EventArgs e)
        { 
           
            frmSpendVoucher frm = new frmSpendVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmVariousVoucher_TileClick(object sender, EventArgs e)
        { 
            frmVariousVoucher frm = new frmVariousVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }
        private void mnufrmCheckSpendVoucher_TileClick(object sender, EventArgs e)
        {
            frmCheckSpendVoucher frm = new frmCheckSpendVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }
        private void mnufrmCheckReceiptVoucher_TileClick(object sender, EventArgs e)
        {

            frmCheckReceiptVoucher frm = new frmCheckReceiptVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();


        }
        private void mnufrmChequesUnderCollection_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptDetailedDailyTransaction_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptAccountStatementWithVat_TileClick(object sender, EventArgs e)
        { 
            frmAccountStatement frm = new frmAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptCostCenterAccountStatment_TileClick(object sender, EventArgs e)
        {
            frmCostCenterAccountStatment frm = new frmCostCenterAccountStatment();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptCustomersAccountStatement_TileClick(object sender, EventArgs e)
        {
            frmCustomersAccountStatement frm = new frmCustomersAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptSuppliersAccountStatement_TileClick(object sender, EventArgs e)
        {
            frmCustomersAccountStatement frm = new frmCustomersAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptSpecificAccountStatement_TileClick(object sender, EventArgs e)
        {
            frmSpecificAccountStatement frm = new frmSpecificAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptBalanceReview_TileClick(object sender, EventArgs e)
        {
            frmBalanceReview frm = new frmBalanceReview();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptFinancialPositionStatement_TileClick(object sender, EventArgs e)
        {
            frmFinancialPositionStatement frm = new frmFinancialPositionStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptIncomeStatement_TileClick(object sender, EventArgs e)
        {
            frmIncomeStatement frm = new frmIncomeStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptReceiptVouchersReport_TileClick(object sender, EventArgs e)
        {
            frmReceiptVouchersReport frm = new frmReceiptVouchersReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptSpendVouchersReport_TileClick(object sender, EventArgs e)
        {
            frmSpendVouchersReport frm = new frmSpendVouchersReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptCheckReceiptVouchersReport_TileClick(object sender, EventArgs e)
        {
            frmCheckReceiptVouchersReport frm = new frmCheckReceiptVouchersReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptCheckSpendVouchersReport_TileClick(object sender, EventArgs e)
        {
            frmCheckSpendVouchersReport frm = new frmCheckSpendVouchersReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptVariousVouchersReport_TileClick(object sender, EventArgs e)
        {
             
            frmVariousVouchersReport frm = new frmVariousVouchersReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
             
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptAgesDebt_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmRestrictionsDailyReport_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptVatReport_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmStores_TileClick(object sender, EventArgs e)
        {
            frmStores frm = new frmStores();
            frmMainStores frm1 = new frmMainStores();

            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm1);

                frm1.FormUpdate = frm.FormUpdate;
                frm1.FormAdd = frm.FormAdd;
                frm1.FormView = frm.FormView;
                frm1.FormDelete = frm.FormDelete;
              

                frm1.Show();
            }
            else
                frm1.Dispose();
        }

        private void mnufrmItems_TileClick(object sender, EventArgs e)
        { 
            frmSizingUnits frm = new frmSizingUnits();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }


        

        private void mnufrmItemsGroups_TileClick(object sender, EventArgs e)
        {
             
            frmItemsSizes frm = new frmItemsSizes();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                
                frm.Show();
            }
            else
                frm.Dispose();


        }
        private void mnufrmSizingUnits_TileClick(object sender, EventArgs e)
        {
             
            frmItemsColors frm = new frmItemsColors();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);


                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmItemsBases_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmItemsSizes_TileClick(object sender, EventArgs e)
        {
            frmItemsSizes frm = new frmItemsSizes();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmItemsColors_TileClick(object sender, EventArgs e)
        {
            frmItemsColors frm = new frmItemsColors();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmItemsBrands_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmGoodsOpening_TileClick(object sender, EventArgs e)
        {
            frmGoodsOpeningOld frm = new frmGoodsOpeningOld();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }


        private void mnufrmGoodsOpeningInvoice_TileClick(object sender, EventArgs e)
        { 
            frmGoodsOpeningOld frm = new frmGoodsOpeningOld();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }



        private void mnurptPrintItemSticker_TileClick(object sender, EventArgs e)
        {
            frmPrintItemSticker frm = new frmPrintItemSticker();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmBarcodeUpdate_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmItemsDismantling_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }




        private void mnufrmItemsTransfer_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }


        private void mnufrmItemsOutonBail_TileClick(object sender, EventArgs e)
        { 
            frmItemsOutOnBail frm = new frmItemsOutOnBail();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void mnufrmItemsInonBail_TileClick(object sender, EventArgs e)
        {
            frmItemsInonBail frm = new frmItemsInonBail();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptStocktaking_TileClick(object sender, EventArgs e)
        {
            frmStocktaking frm = new frmStocktaking();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptItemsList_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptStocktakingWeight_TileClick(object sender, EventArgs e)
        {
            frmStocktaking frm = new frmStocktaking();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptItemBalance_TileClick(object sender, EventArgs e)
        {
            frmItemBalance frm = new frmItemBalance();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptMinQtyLimitReport_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptMaxQtyLimitReport_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptMaxSoldItems_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptMinSoldItems_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptMostReturnedItems_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptItemProfit_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptCanceledItems_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptItemSN_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmSuppliers_TileClick(object sender, EventArgs e)
        {
            frmSuppliers frm = new frmSuppliers();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmPurchasesDelegates_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptPurchasesInvoiceReport_TileClick(object sender, EventArgs e)
        {
            frmPurchasesInvoiceReport frm = new frmPurchasesInvoiceReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptPurchasesInvoiceReturnReport_TileClick(object sender, EventArgs e)
        {
            frmPurchasesInvoiceReturnReport frm = new frmPurchasesInvoiceReturnReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }


        private void mnufrmPurchaseInvoice_TileClick(object sender, EventArgs e)
        {
            frmCashierPurchaseGold frm = new frmCashierPurchaseGold();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmPurchaseInvoiceReturn_TileClick(object sender, EventArgs e)
        {
            frmCashierPurchaseReturnDaimond frm = new frmCashierPurchaseReturnDaimond();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmPurchaseOrder_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptPurchaseInPeriodByItem_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptMostSuppliersDealing_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptLessSuppliersDealing_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmCustomers_TileClick(object sender, EventArgs e)
        {
            frmCustomers frm = new frmCustomers();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmSellers_TileClick(object sender, EventArgs e)
        {
            frmSellers frm = new frmSellers();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmSalesDelegates_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnufrmSpecialOffers_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptSalesInvoiceReport_TileClick(object sender, EventArgs e)
        {
            frmSalesInvoiceReport frm = new frmSalesInvoiceReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptSalesInvoiceReturn_TileClick(object sender, EventArgs e)
        {
            frmSalesReturnReport frm = new frmSalesReturnReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnufrmSalesInvoice_TileClick(object sender, EventArgs e)
        {
            frmCashierSalesGold frm = new frmCashierSalesGold();
            frm.Show();
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Item.Enabled = true;
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Item.Enabled = true;
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = true;
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;
        }
        private void mnufrmSalesInvoiceReturn_TileClick(object sender, EventArgs e)
        {
            frmSalesInvoiceReturn frm = new frmSalesInvoiceReturn();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void mnufrmCashierSales_TileClick(object sender, EventArgs e)
        {
            frmCashierSalesAlmas frm = new frmCashierSalesAlmas();
            frm.Show();
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Item.Enabled = true;
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Item.Enabled = true;
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = true;
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;
            frm.ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = true;
        }

        private void mnurptMostCustomersBuying_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptLessCustomersBuying_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptMostSellerBuying_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptLessSellerBuying_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptDelegatesSelling_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptItemSalePrice_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptSalesInPeriodByItem_TileClick(object sender, EventArgs e)
        {
            frmSalesInPeriodByItem frm = new frmSalesInPeriodByItem();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void mnurptMostDelegatesSelling_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }
        private void mnurptLessDelegatesSelling_TileClick(object sender, EventArgs e)
        {
            Teeemp frm = new Teeemp();
            frm.Show();
        }

        private void btnItmfrmCustomers_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            
            frmCustomers frm = new frmCustomers();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void TitleSeler_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmOpeningVoucher frm = new frmOpeningVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.Last();
            }
            else
                frm.Dispose();
        }

        private void frmMainEdex_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Do you really want to exit " : "هل تريد اغلاق النظام"));
                if (Yes)
                {
                    System.Windows.Forms.Application.Exit();
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnItmfrmSalesInvoiceReport_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmNetSalesInvoiceReport frm = new frmNetSalesInvoiceReport();
         
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        private void tileItem17_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmUserPermissions frm = new frmUserPermissions();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                 frm.Show();
            }
            else
                frm.Dispose();
        }

        private void tileItem3_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            

            frmSpendVouchersReport frm = new frmSpendVouchersReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        private void btnItmfrmItems_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmReceiptVouchersReport frm = new frmReceiptVouchersReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void mnurptAccountStatement_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmAccountStatement frm = new frmAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void btnItmfrmGoodsOpening_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmGoodsOpeningOld frm = new frmGoodsOpeningOld();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }


        private void tileItem4_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

            frmSalesInvoiceReport frm = new frmSalesInvoiceReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                
            }
            else
                frm.Dispose();

        }

        private void btnItmfrmPurchaseOrder_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
             
            frmSalesReturnReport frm = new frmSalesReturnReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void btnItmfrmPurchaseInvoiceReturn_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmCashierPurchaseDaimond frm = new frmCashierPurchaseDaimond();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void btnItmfrmSpecialOffers_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmItemBalance frm = new frmItemBalance();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }
        private void btnItmfrmCheckReceiptVoucher_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
          
            frmSpendVoucher frm = new frmSpendVoucher();
             
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void btnItmfrmCheckSpendVoucher_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmAccountStatement frm = new frmAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void btnItmfrmReceiptVoucher_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmReceiptVoucher frm = new frmReceiptVoucher();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void btnItmfrmSuppliers_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmSuppliers frm = new frmSuppliers();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void btnItmfrmSpendVoucher_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmCashierPurchaseSaveDaimond frm = new frmCashierPurchaseSaveDaimond();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
             
        }
        private void btnPurchaseeports_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        { 

            frmPurchasesInvoiceReport frm = new frmPurchasesInvoiceReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }
        private void tileItem5_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmPurchasesInvoiceReturnReport frm = new frmPurchasesInvoiceReturnReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void tileItem6_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmSellers frm = new frmSellers();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void btnItems_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmItems frm = new frmItems();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void tileItem6_ItemClick_1(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmCustomersAccountStatement frm = new frmCustomersAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
             
               frm.Show();
                frm.Text = "كشف حساب الموردين";
                frm.chkCustomer.Checked = true;
               
            }
            else
                frm.Dispose();
        }

        private void tileItem7_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmCustomersAccountStatement frm = new frmCustomersAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                 frm.Show();
                frm.Text = "كشف حساب الموردين";
                frm.chkSupliar.Checked = true;
                
            }
            else
                frm.Dispose();
        }

        private void tileItem8_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmBalanceReview frm = new frmBalanceReview();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void tileItem9_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmFinancialPositionStatement frm = new frmFinancialPositionStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                // frm.Show();


            }
            else
                frm.Dispose();
        }

        private void tileItem10_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmIncomeStatement frm = new frmIncomeStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                // frm.Show();


            }
            else
                frm.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmQrcodeGenreate frm = new frmQrcodeGenreate();
            frm.Show();
 
        }

        private void frmMainEdex_KeyDown(object sender, KeyEventArgs e)
        {
            


        }

        private void frmMainEdex_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                btnItmfrmSalesInvoice_ItemClick(null, null);


            if (e.KeyCode == Keys.F6)
                btnItmfrmSpendVoucher_ItemClick(null, null);

            if (e.KeyCode == Keys.F7)
                btnItmfrmReceiptVoucher_ItemClick(null, null);


            if (e.KeyCode == Keys.F8)
                btnItmfrmSpendVoucher_ItemClick(null, null);


        }

        private void mnuHelp_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmProgramShort frm = new frmProgramShort();
            frm.ShowDialog();
        }
    }
}

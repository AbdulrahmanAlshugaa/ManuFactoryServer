using Edex.SalesAndSaleObjects.Transactions;
using Edex.SalesAndPurchaseObjects.Transactions;
using System;
using System.Windows.Forms;
using System.Linq;
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
using DevExpress.XtraEditors;
using System.IO;
using System.Text;
using DevExpress.Utils.Filtering.Internal;
using System.Globalization;
using Edex.AccountsObjects.FinancialStatements;
using Edex.Manufacturing.Codes;
using Edex.GeneralObjects.GeneralForms;
using Edex.HR.Codes;
using Edex.Manufacturing;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraBars;
using DevExpress.XtraTab;
using System.Collections.Generic;
using Edex.Manufacturing.Reports;
using Edex.HR.HRClasses;
using Edex.Archives;


namespace Edex
{
    public partial class frmMainEdexDaimondShop : DevExpress.XtraEditors.XtraForm
    {
        public frmMainEdexDaimondShop()
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
            //frmCashierSalesGold frm = new frmCashierSalesGold();
            frmCashierSales frm = new frmCashierSales();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();           
          }
        private void btnItmfrmSalesInvoiceReturn_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
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
       

       
        IEnumerable<TileNavCategory> GetAllTileGroups(TileNavPane tileControl)
        {
            return tileControl.Categories.Cast<TileNavCategory>();
        }
        IEnumerable<TileNavCategory> GetAllTileNavItems(TileNavPane tileControl)
        {
            return tileControl.Categories.Cast<TileNavCategory>();
        }
        IEnumerable<TileItem> GetAllTileItems(TileGroup tileGroup)
        {
            return tileGroup.Items.Cast<TileItem>();
        }
        IEnumerable<TileNavItem> GetAllTileItems(TileNavCategory tileGroup)
        {
            return tileGroup.Items.Cast<TileNavItem>();
        }


        void ApplyLanguage(Control control)
        {
            
            if (control is TextEdit || control is ComboBoxEdit)
            {
                return;
            }
            control.RightToLeft = RightToLeft.No;
            if (UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR(control);
            }

            foreach (Control childControl in GetAllChildControls(control))
            {
                if (childControl is TextEdit || childControl is ComboBoxEdit)
                {
                    continue;
                }
                childControl.Location = new System.Drawing.Point(control.Size.Width - childControl.Size.Width - childControl.Location.X, childControl.Location.Y);
                ApplyLanguage(childControl);
            }
        }

        void ApplyLanguageToElement(object element)
        {
           
            if (element is BarItem && UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR((BarItem)element);
            }
            else if (element is TileItem && UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR((TileItem)element);
            }
            else if (element is TileNavCategory && UserInfo.Language == iLanguage.English)
            {

                ChangeLanguage.LTR((TileNavCategory)element);
                
            }
            
            else if (element is TileNavPane && UserInfo.Language == iLanguage.English)
            {
               
                ChangeLanguage.LTR((TileNavPane)element);
                foreach (var childElement in GetAllTileNavCategories((TileNavPane)element))
                {
                    MessageBox.Show(((TileNavCategory)childElement).Caption.ToString());
                    ApplyLanguageToElement(childElement);
                }
            }

            else if (element is TileControl && UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR((TileControl)element);
                foreach (TileGroup tileGroup in GetAllTileGroups((TileControl)element))
                {
                    ApplyLanguageToElement(tileGroup);
                }
            }
            else if (element is TileGroup && UserInfo.Language == iLanguage.English)
            {
                ChangeLanguage.LTR((TileGroup)element);
                foreach (TileItem tileItem in GetAllTileItems((TileGroup)element))
                {
                    ApplyLanguageToElement(tileItem);
                }
            }
            else if (element is Control)
            {
                ApplyLanguage((Control)element);
                foreach (Control childControl in GetAllChildControls((Control)element))
                {
                    ApplyLanguageToElement(childControl);
                }
            }
        }

        void Language()
        {
            if (UserInfo.Language != iLanguage.Arabic)
            {
                foreach (Control childControl in GetAllChildControls(this))
                {
                    if (UserInfo.Language == iLanguage.English)
                    {
                        ChangeLanguage.LTR(childControl);
                    }
                    ApplyLanguageToElement(childControl);
                }
            }
        }

     

        IEnumerable<Control> GetAllChildControls(Control control)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAllChildControls(ctrl)).Concat(controls);
        }

        IEnumerable<TileGroup> GetAllTileGroups(TileControl tileControl)
        {
            return tileControl.Groups.Cast<TileGroup>();
        }

        IEnumerable<TileNavCategory> GetAllTileNavCategories(TileNavPane tileControl)
        {
            List<TileNavCategory> allCategories = new List<TileNavCategory>();
            foreach (var category in tileControl.Categories)
            {
                if (category is TileNavCategory)
                {
                    allCategories.Add((TileNavCategory)category);
                }
            }
            return allCategories;
        }



        IEnumerable<TileNavItem> GetAllTileNavItems(TileNavCategory tileCategory)
        {
            return tileCategory.Items.Cast<TileNavItem>();
        }


        private void frmMainEdex_Load(object sender, EventArgs e)
        {
            Language();
            tileNavItem20.Visible = false;
            pnlSetting.Top = this.Height - pnlSetting.Height*3 - 30 ;
            pnlSetting.Left= 150;
            mnufrmItemsSizes.Visible = true;
            frmItemGroup.TileClick += frmItemGroup_TileClick;
            menuFrmMachinReport.TileClick += frmMachinReport_TileClick;
            mnufrmSalesProfitReport.TileClick += mnufrmSalesProfitReport_TileClick;
            mnufrmIncomeList.TileClick+=mnufrmIncomeList_TileClick;
            mnufrmItemDiamondBalance.TileClick += mnufrmItemDiamondBalance_TileClick;
            mnuBanks.TileClick += mnuBanks_TileClick;
            mnuBoxes.TileClick += mnuBoxes_TileClick;
            mnuCurrencies.TileClick += mnuCurrencies_TileClick;
            mnufrmGoldInonBail.TileClick += mnufrmGoldInonBail_TileClick;
            mnufrmGoldOutOnBail.TileClick += mnufrmGoldOutOnBail_TileClick;
            mnufrmMatirialInonBail.TileClick += mnufrmMatirialInonBail_TileClick;
            mnufrmMatirialOutOnBail.TileClick += mnufrmMatirialOutOnBail_TileClick;
            mnuTransferMultipleStoresGold.TileClick += mnuTransferMultipleStoresGold_TileClick;
            mnuTransferMultipleStoresMatirial.TileClick += mnuTransferMultipleStoresMatirial_TileClick;
            mnufrmItemType.TileClick += mnufrmItemType_TileClick;
            mnufrmfrmCashierSalesOrder.TileClick += mnufrmfrmCashierSalesOrder_TileClick;
            mnuCashierPurchaseMatirial.TileClick += mnuCashierPurchaseMatirial_TileClick;
            MnuSalseServiece.TileClick += MnuSalseServiece_TileClick;
            MnuManuFactoryOutOn.TileClick += MnuManuFactoryOutOn_TileClick;
            MnuManuFactoryInOn.TileClick += MnuManuFactoryInOn_TileClick;
            Menu_MachineMenufactory.TileClick += Menu_MachineMenufactory_TileClick;
            Mnu_AccountStatmentSAR.TileClick += MnuAccountStatmentSAR_TileClick;
           
            mnufrmStockTransactions.TileClick += mnufrmStockTransactions_TileClick;
            MnufAccountStatemntGold.TileClick += MnufAccountStatemntGold_TileClick;
            mnufrmItemsReport.TileClick += mnufrmItemsReport_TileClick;
            mnufrmAccountStatemntDiamond.TileClick += mnufrmAccountStatemntDiamond_TileClick;
            mnufrmCategory.TileClick += mnufrmCategory_TileClick;
            MnufrmReligions.TileClick+=MnufrmReligions_TileClick;
            MnufrmDeclaringEstimatedSpends.TileClick += MnufrmDeclaringEstimatedSpends_TileClick;
            //شئون الموظفين
            MnufrmEmployFile.TileClick += MnufrmEmployFile_TileClick;
            MnufrmAdministrations.TileClick += MnufrmAdministrations_TileClick;
            MnufrmDepartment.TileClick += MnufrmDepartment_TileClick;
            MnufrmQualifications.TileClick += MnufrmQualifications_TileClick;
            MnufrmEmpAllowancesAndDeductions.TileClick += frmEmpAllowancesAndDeductions_TileClick;
            MnufrmContracType.TileClick += MnufrmContracType_TileClick;
            MnufrmDeductionsTypes.TileClick += MnufrmDeductionsTypes_TileClick;
            MnuAllowancesTypes.TileClick += MnufrmAllowancesTypes_TileClick;
            MnufrmEmployeeCurrentStatus.TileClick += MnufrmEmployeeCurrentStatus_TileClick;
            MnufrmEndofServiceTypes.TileClick += MnufrmEndofServiceTypes_TileClick;
            MnufrmJobs.TileClick += MnufrmJobs_TileClick;
            MnufrmNationalities.TileClick += MnufrmNationalities_TileClick;
            MnufrmRecordAbsent.TileClick += MnufrmRecordAbsent_TileClick;
            MnufrmReligions.TileClick += MnufrmReligions_TileClick;
            MnufrmScientificDisciplines.TileClick += MnufrmScientificDisciplines_TileClick;
            MnufrmVacationsTypes.TileClick += MnufrmVacationsTypes_TileClick;
            MnufrmWorkingTypes.TileClick += MnufrmWorkingTypes_TileClick;
            MnufrmVacationRequest.TileClick += MnufrmVacationRequest_TileClick;
            MnuAddEmployeeDurationManually.TileClick += MnufrmAddEmployeeDurationManually_TileClick;
            Mnu_frmSalaries.TileClick += Mnu_frmSalaries_TileClick;
            //التصنيع 
            MnufrmOrderRestriction.TileClick += MnufrmOrderRestriction_TileClick;
            MnufrmCadFactory.TileClick += MnufrmCadFactory_TileClick;
            MnufrmWaxFactory.TileClick += MnufrmWaxFactory_TileClick;
            MnufrmZirconeFactory.TileClick += MnufrmZirconeFactory_TileClick;
            MnufrmDiamondFactory.TileClick += MnufrmDiamondFactory_TileClick;
            MnufrmAfforestationFactory.TileClick += MnufrmAfforestationFactory_TileClick;
            MnufrmCasting.TileClick += MnufrmCasting_TileClick;
            MnufrmManufacturingCommand.TileClick += MnufrmManufacturingCommand_TileClick;
            MnufrmManufacturingPrentag.TileClick += MnufrmManufacturingPrentag_TileClick;

            MnufrmManufacturingPrentag2.TileClick += MnufrmManufacturingPrentag2_TileClick;
            MnufrmManufacturingTalmee.TileClick += MnufrmManufacturingTalmee_TileClick;
            MnufrmManufacturingTalmee2.TileClick += MnufrmManufacturingTalmee2_TileClick;
            MnufrmManufacturingTalmee3.TileClick += MnufrmManufacturingTalmee3_TileClick;
            MnufrmManufacturingCompond.TileClick += MnufrmManufacturingCompond_TileClick;
            MnufrmManufacturingDismant.TileClick += MnufrmManufacturingDismant_TileClick;
            MnufrmManufactoryAdditional.TileClick += MnufrmManufactoryAdditional_TileClick;
            mnurptOrdersReport.TileClick += mnurfrmOrdersReport_TileClick;
            MnufrmOrderRunningReport.TileClick += MnufrmOrderRunningReport_TileClick;
            MnufrmManuExpencessOrder.TileClick += MnufrmManuExpencessOrder_TileClick;
            Mnu_frmClosingOrders.TileClick += Mnu_frmClosingOrders_TileClick;

            MnufrmTypeOrders.TileClick += MnufrmTypeOrders_TileClick;
            Mnu_frmLostEmployeeReport.TileClick += Mnu_frmLostEmployeeReport_TileClick;
            Mnu_frmLostAllEmployee.TileClick += Mnu_frmLostAllEmployee_TileClick;
            Mnu_frmLostAllInBrntage.TileClick += Mnu_frmLostAllInBrntage_TileClick;
            Mnu_frmLostBrntageEmployeeReport.TileClick += Mnu_frmLostBrntageEmployeeReport_TileClick;
            Mnu_frmLostAllTalmee.TileClick += Mnu_frmLostAllTalmee_TileClick;
            Mnu_frmLostAllTalmeeEmployeeReport.TileClick += Mnu_frmLostAllTalmeeEmployeeReport_TileClick;
            Mnu_frmLostAllCompound.TileClick += Mnu_frmLostAllCompound_TileClick;
            Mnu_frmLostCompoundEmployeeReport.TileClick += Mnu_frmLostCompoundEmployeeReport_TileClick;
            Mnu_frmMnuReturnFilings.TileClick += Mnu_frmMnuReturnFilings_TileClick;
            Mnu_frmMnuReturnFilingsReport.TileClick += Mnu_frmMnuReturnFilingsReport_TileClick;
            Mnu_frmManufacturingDismantItems.TileClick += Mnu_frmManufacturingDismantItems_TileClick;
            Mnu_frmSummaryLost.TileClick += Mnu_frmSummaryLost_TileClick;
            //======
            Mnu_DesignModel.TileClick += Mnu_DesignModel_TileClick;
            Mnu_frmRemindQtyItem.TileClick += Mnu_frmRemindQtyItem_TileClick;

            Mnu_frmCashierPurchaseServicesEqv.TileClick += Mnu_frmCashierPurchaseServicesEqv_TileClick;

            //الارشيفات 
            Mnu_frmArchivesPurchaseAndSales.TileClick += Mnu_frmArchivesPurchaseAndSales_TileClick;

            try
            {
                string textexpire = System.IO.File.ReadAllText(Application.StartupPath + "\\Sync33.dll");
                String datnow = (DateTime.Now.ToString("yyyy/MM/dd"));
                int textnowseral = Comon.ConvertDateToSerial(datnow);
                int textexpireseral = Comon.ConvertDateToSerial(textexpire);
                int y = Comon.cInt(textexpireseral.ToString().Substring(0, 4));
                int m = Comon.cInt(textexpireseral.ToString().Substring(4, 2));
                int d = Comon.cInt(textexpireseral.ToString().Substring(6, 2));
                var prevDate = new DateTime(y, m, d); //15 July 2021
                var today = DateTime.Now;
                var diffOfDates = prevDate - today;
                lblDays.Text = (diffOfDates.Days).ToString() + " يوم ";
                this.Text = this.Text + " - صلاحية الدعم الفني " + (diffOfDates.Days).ToString() + " يوم ";
                if (Comon.cInt(diffOfDates) < 7)
                {
                    lblDays.Text = "    فترة صلاحية الدعم الفني" + (diffOfDates.Days).ToString() + "يوم ";

                    lblDays.Visible = true;
                }
                else
                    lblDays.Visible = false;
                 
            }
            catch
            {
            }
            if (MySession.GlobalInventoryType <= 0)
            {
                Messages.MsgInfo(Messages.TitleInfo, "الرجاء تهيئة اعدادات النظام- وطريقة الجرد");                
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
        }

        void Mnu_frmArchivesPurchaseAndSales_TileClick(object sender, NavElementEventArgs e)
        {
            frmArchivesPurchaseAndSales frm = new frmArchivesPurchaseAndSales();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void MnufrmManufacturingTalmee3_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 3;
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void Mnu_frmManufacturingDismantItems_TileClick(object sender, NavElementEventArgs e)
        {
            frmManufacturingDismantItems frm = new frmManufacturingDismantItems();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmSalaries_TileClick(object sender, NavElementEventArgs e)
        {
            frmSalaries frm = new frmSalaries();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmCashierPurchaseServicesEqv_TileClick(object sender, NavElementEventArgs e)
        {
            frmCashierPurchaseServicesEqv frm = new frmCashierPurchaseServicesEqv();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmSummaryLost_TileClick(object sender, NavElementEventArgs e)
        {
            frmSummaryLost frm = new frmSummaryLost();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
             
        }

        void Mnu_frmMnuReturnFilingsReport_TileClick(object sender, NavElementEventArgs e)
        {
            frmMnuReturnFilingsReport frm = new frmMnuReturnFilingsReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmMnuReturnFilings_TileClick(object sender, NavElementEventArgs e)
        {
            frmMnuReturnFilings frm = new frmMnuReturnFilings();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmLostCompoundEmployeeReport_TileClick(object sender, NavElementEventArgs e)
        {
            frmLostCompoundEmployeeReport frm = new frmLostCompoundEmployeeReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmLostAllCompound_TileClick(object sender, NavElementEventArgs e)
        {
            frmLostAllCompound frm = new frmLostAllCompound();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmLostAllTalmeeEmployeeReport_TileClick(object sender, NavElementEventArgs e)
        {
            frmLostAllTalmeeEmployeeReport frm = new frmLostAllTalmeeEmployeeReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmLostAllTalmee_TileClick(object sender, NavElementEventArgs e)
        {
            frmLostAllTalmee frm = new frmLostAllTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose();
        
        }

        void Mnu_frmLostBrntageEmployeeReport_TileClick(object sender, NavElementEventArgs e)
        {
            frmLostBrntageEmployeeReport frm = new frmLostBrntageEmployeeReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void Mnu_frmLostAllInBrntage_TileClick(object sender, NavElementEventArgs e)
        {
            frmLostAllInBrntage frm = new frmLostAllInBrntage();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void Mnu_frmRemindQtyItem_TileClick(object sender, NavElementEventArgs e)
        {
            frmRemindQtyItem frm = new frmRemindQtyItem();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmLostAllEmployee_TileClick(object sender, NavElementEventArgs e)
        {
            frmLostAllEmployee frm = new frmLostAllEmployee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_frmLostEmployeeReport_TileClick(object sender, NavElementEventArgs e)
        {
            frmLostEmployeeReport frm = new frmLostEmployeeReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void MnufrmTypeOrders_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmTypeOrders frm = new frmTypeOrders();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void MnufrmDeclaringEstimatedSpends_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmDeclaringEstimatedSpends frm = new frmDeclaringEstimatedSpends();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void Mnu_DesignModel_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmIDsImages frm = new frmIDsImages();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.SCREENNO = 2;
                frm.IDNo = "1";
                frm.Show();
            }
            else
                frm.Dispose(); 
            
        }

        void Mnu_frmClosingOrders_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmClosingOrders frm = new frmClosingOrders();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void MnufrmManuExpencessOrder_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManuExpencessOrder frm = new frmManuExpencessOrder();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void MnufrmManufactoryAdditional_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufactoryAdditional frm = new frmManufactoryAdditional();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void MnufrmOrderRunningReport_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmOrderRunningReport frm = new frmOrderRunningReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.Show();
            }
            else
                frm.Dispose();
             
        }

        void MnufrmManufacturingDismant_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufacturingDismantOrders frm = new frmManufacturingDismantOrders();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void mnurfrmOrdersReport_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmOrdersReportBeforCasting frm = new frmOrdersReportBeforCasting();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }



        void MnufrmManufacturingCompond_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufacturingCompond frm = new frmManufacturingCompond();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmManufacturingTalmee2_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 2;
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmManufacturingTalmee_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 1;
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmManufacturingPrentag2_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufacturingPrentag frm = new frmManufacturingPrentag();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPrntageTypeID.EditValue = 2;
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmManufacturingPrentag_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufacturingPrentag frm = new frmManufacturingPrentag();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPrntageTypeID.EditValue = 1;
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmManufacturingCommand_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManufacturingCommand frm = new frmManufacturingCommand();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
          
        }

        void MnufrmCasting_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmCasting frm = new frmCasting();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void MnufrmAfforestationFactory_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmAfforestationFactory frm = new frmAfforestationFactory();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void MnufrmZirconeFactory_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmZirconeFactory frm = new frmZirconeFactory();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmDiamondFactory_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {

            frmDiamondFactory frm = new frmDiamondFactory();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmCadFactory_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmCadFactory frm = new frmCadFactory();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
            
        }
        void MnufrmWaxFactory_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmWaxFactory frm = new frmWaxFactory();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        void MnufrmOrderRestriction_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmOrderRestriction frm = new frmOrderRestriction();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }
        //شئون الموظفين
        void MnufrmAddEmployeeDurationManually_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmAddEmployeeDurationManually frm = new frmAddEmployeeDurationManually();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmVacationRequest_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmVacationRequest frm = new frmVacationRequest();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmWorkingTypes_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmWorkingTypes frm = new frmWorkingTypes();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmVacationsTypes_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmVacationsTypes frm = new frmVacationsTypes();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmScientificDisciplines_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmScientificDisciplines frm = new frmScientificDisciplines();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmRecordAbsent_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmRecordAbsent frm = new frmRecordAbsent();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmAdministrations_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {

            frmAdministrations frm = new frmAdministrations();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmDepartment_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {

            frmDepartments frm = new frmDepartments();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmQualifications_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmQualifications frm = new frmQualifications();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void frmEmpAllowancesAndDeductions_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmAllowancesAndDeductions frm = new frmAllowancesAndDeductions();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmContracType_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmContracType frm = new frmContracType();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void MnufrmAllowancesTypes_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmAllowancesTypes frm = new frmAllowancesTypes();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void MnufrmDeductionsTypes_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmDeductionsTypes frm = new frmDeductionsTypes();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmEmployeeCurrentStatus_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmEmployeeCurrentStatus frm = new frmEmployeeCurrentStatus();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmEndofServiceTypes_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmEndofServiceTypes frm = new frmEndofServiceTypes();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmJobs_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmJobs frm = new frmJobs();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmNationalities_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmNationalities frm = new frmNationalities();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void MnufrmReligions_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmReligions frm = new frmReligions();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void mnufrmCategory_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {

            frmCategory frm = new frmCategory();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose(); 
        }
        void mnufrmAccountStatemntDiamond_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmAccountStatemntDiamond frm = new frmAccountStatemntDiamond();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void mnufrmItemsReport_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmItemsReport frm = new frmItemsReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose(); 
        }
 
 

        void MnufAccountStatemntGold_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmAccountStatemntGold frm = new frmAccountStatemntGold();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

 
        void mnufrmStockTransactions_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmStockTransactions frm = new frmStockTransactions();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void MnufrmEmployFile_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {

            frmEmployeeFiles frm = new frmEmployeeFiles();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose(); 
        }
        void MnuAccountStatmentSAR_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmAccountStatemntSAR frm = new frmAccountStatemntSAR();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void Menu_MachineMenufactory_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmMachine frm = new frmMachine();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose(); 
        }

        void MnuManuFactoryInOn_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManuFactoryCommandInOnBail frm = new frmManuFactoryCommandInOnBail();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void MnuManuFactoryOutOn_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmManuFactoryCommandOutOnBail frm = new frmManuFactoryCommandOutOnBail();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void MnuSalseServiece_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmCashierSales frm = new frmCashierSales();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
           
        }

        void mnuCashierPurchaseMatirial_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmCashierPurchaseServicesEqv frm = new frmCashierPurchaseServicesEqv();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void mnufrmfrmCashierSalesOrder_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmCashierSalesOrder frm = new frmCashierSalesOrder();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void mnufrmItemType_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmItemType frm = new frmItemType();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
            
        }
        void mnuTransferMultipleStoresMatirial_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmTransferMultipleStoreMatirial frm = new frmTransferMultipleStoreMatirial();           
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void mnuTransferMultipleStoresGold_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmTransferMultipleStoresGold frm = new frmTransferMultipleStoresGold();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void mnufrmMatirialOutOnBail_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmMatirialOutOnBail frm = new frmMatirialOutOnBail();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void mnufrmMatirialInonBail_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmMatirialInOnBail frm = new frmMatirialInOnBail();         
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void mnufrmGoldOutOnBail_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmGoldOutOnBail frm = new frmGoldOutOnBail();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();          
        }
        void mnufrmGoldInonBail_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmGoldInOnBail frm = new frmGoldInOnBail();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        void mnuCurrencies_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmCurrency frm = new frmCurrency();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void mnuBoxes_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmBoxes frm = new frmBoxes();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        void mnuBanks_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmBanks frm = new frmBanks();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }


        private void mnufrmIncomeList_TileClick(object sender, EventArgs e)
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
            frmDeclaringFixedSpends frm = new frmDeclaringFixedSpends();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
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
            frmTaxDeclaratis frm = new frmTaxDeclaratis();      
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
            }
            else
                frm.Dispose();
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
            frmGoodsOpening frm = new frmGoodsOpening();
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
        }
        private void mnurptStocktakingWeight_TileClick(object sender, EventArgs e)
        {
            frmStocktakingByStores frm = new frmStocktakingByStores();
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
            frmItemBalanceByStores frm = new frmItemBalanceByStores();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
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
            frmPurchasesDelegates frm = new frmPurchasesDelegates();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
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

            frmCashierPurchaseReturnMatirial frm = new frmCashierPurchaseReturnMatirial();
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
            frmCashierPurchaseOrder frm = new frmCashierPurchaseOrder();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
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
            frmSalesDelegates frm = new frmSalesDelegates();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

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
                    frmBackupDataBase frm = new frmBackupDataBase();
                    frm.BacKUp();
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
            //frmGoodsOpeningOld frm = new frmGoodsOpeningOld();
            frmGoodsOpening frm = new frmGoodsOpening();
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
            //frmCashierPurchaseDaimond frm = new frmCashierPurchaseDaimond();
            frmCashierPurchaseMatirial frm = new frmCashierPurchaseMatirial();
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
            frmItemBalanceByStores frm = new frmItemBalanceByStores();
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

        private void tileItem6_ItemClick_1(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmCustomersAccountStatement frm = new frmCustomersAccountStatement();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
             
               frm.Show();
                frm.Text = "كشف حساب العملاء";
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
                 frm.Show();


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

                frm.Show();
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
            if (e.KeyCode == Keys.F1 && e.Shift)
                mnufrmAccountsTree_TileClick(null, null);
            else if (e.KeyCode == Keys.F2 && e.Shift)
                btnItmfrmSalesInvoiceReturn_ItemClick(null, null);
            else if (e.KeyCode == Keys.F3 && e.Shift)
                btnItmfrmSalesInvoice_ItemClick(null, null);
            else if (e.KeyCode == Keys.F4 && e.Shift)
                mnufrmCustomers_TileClick(null, null);
            else if (e.KeyCode == Keys.F5 && e.Shift)
                mnuCashierPurchaseMatirial_TileClick(null, null);
            else if (e.KeyCode == Keys.F6 && e.Shift)
                tileItem15_ItemClick_1(null, null);

            else if (e.KeyCode == Keys.F7 && e.Shift)
                mnufrmGoldInonBail_TileClick(null, null);
            else if (e.KeyCode == Keys.F8 && e.Shift)
                mnufrmGoldOutOnBail_TileClick(null, null);

            else if (e.KeyCode == Keys.F9 && e.Shift)
                mnufrmMatirialInonBail_TileClick(null, null);
            else if (e.KeyCode == Keys.F10 && e.Shift)
                mnufrmMatirialOutOnBail_TileClick(null, null);

            else if (e.KeyCode == Keys.F11 && e.Shift)
                mnuTransferMultipleStoresGold_TileClick(null, null);
            else if (e.KeyCode == Keys.F12 && e.Shift)
                mnuTransferMultipleStoresMatirial_TileClick(null, null);
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

        private void tileItem12_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmPurshseSaveReport frm = new frmPurshseSaveReport();
             if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void tileItem13_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
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

        private void tileItem14_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

        }
        void frmItemGroup_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
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

        void mnufrmSalesProfitReport_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmNetProfitReports frm = new frmNetProfitReports();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                    
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);

                    frm.Show();
            }
            else
                frm.Dispose();

        }


        void mnufrmItemDiamondBalance_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            //frmItemDiamondBalance frm = new frmItemDiamondBalance();
            //if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            //{

            //    if (UserInfo.Language == iLanguage.English)
            //        ChangeLanguage.EnglishLanguage(frm);
            //    frm.Show();
            //}
            //else
            //    frm.Dispose();

            frmItemBalanceByStores frm = new frmItemBalanceByStores();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {

                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();


        }

        void frmMachinReport_TileClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            frmMachinVouchersReport frm = new frmMachinVouchersReport();             
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {

                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void tileItem14_ItemClick_1(object sender, TileItemEventArgs e)
        {
            frmPurshseSaveReturnReport frm = new frmPurshseSaveReturnReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {

                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void tileItem15_ItemClick(object sender, TileItemEventArgs e)
        {
            frmManufacturingCommand frm = new frmManufacturingCommand();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }
        private void tileItem16_ItemClick(object sender, TileItemEventArgs e)
        {
            frmManufacturingCoding frm = new frmManufacturingCoding();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void tileItem17_ItemClick_1(object sender, TileItemEventArgs e)
        {
            frmAuxiliaryMaterialsAlcadFactory frm = new frmAuxiliaryMaterialsAlcadFactory();

            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void tileItem19_ItemClick(object sender, TileItemEventArgs e)
        {
            frmCasting frm = new frmCasting();

            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void tileItem20_ItemClick(object sender, TileItemEventArgs e)
        {
            frmAuxiliaryMaterialsZericonFactory frm = new frmAuxiliaryMaterialsZericonFactory();

            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void tileItem11_ItemClick(object sender, TileItemEventArgs e)
        {
            
        }
        private void tileItem15_ItemClick_1(object sender, TileItemEventArgs e)
        {
            //MnufrmManufacturingCommand_TileClick(null, null);
           
         

            frmManufacturingStages frm = new frmManufacturingStages();

            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
             

        }

        private void tileControl1_Click(object sender, EventArgs e)
        {

        }


      

      

    

      

       

        
 

 
    }
}

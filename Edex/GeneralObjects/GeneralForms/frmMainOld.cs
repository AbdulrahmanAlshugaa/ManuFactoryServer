using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using Edex.Model;
using DevExpress.XtraEditors;
using Edex.DAL.UsersManagement;
using System.Xml;
using System.Diagnostics;
using Edex.Properties;
using System.Configuration;
using Edex.Model.Language;
using Edex.DAL.Common;
using Edex.ModelSystem;
using Edex.StockObjects.Codes;
using DevExpress.XtraSplashScreen;
using System.Threading;
 
using Edex.StockObjects.Transactions;
using DevExpress.XtraBars.Alerter;
using Edex.ModelSystem;
using Edex.GeneralObjects.GeneralClasses;
using Edex.DAL;
using DevExpress.XtraTreeList.Nodes;

 
using DevExpress.XtraCharts;
using Edex.GeneralObjects.GeneralForms;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmMainOld : RibbonForm
    {
       public Form alertForm = null;
        public string PrimaryKeyName = "ArbName";
        public string strSQL = "";
        bool isNotLoadFirstTime = false;
        private DevExpress.XtraNavBar.NavBarItem[] navFavorit= new DevExpress.XtraNavBar.NavBarItem[30];
        public frmMainOld()
        {
          
            bool instanceCountOne = false;

            using (Mutex mtex = new Mutex(true, "Accouting System", out instanceCountOne))
            {
                if (instanceCountOne)
                {
                    Application.DoEvents();
                    InitializeComponent();
                    InitSkinGallery();
                    lblBranchName.Tag = "";
                    if (UserInfo.Language == iLanguage.English)
                    {
                        ChangeLanguage.EnglishLanguage(this);
                    }
                    GetSkinName();
                  // GetToolBar();

                  
                  //  Lip.MySession.Start();
                    AddItemNavBarControl();


                    //if (UserInfo.Language == iLanguage.Arabic)
                    //{
                    //    this.RightToLeftLayout = true;
                    //    this.RightToLeft = RightToLeft.Yes;
                    //    ribbonStatusBar.ItemLinks[1].Item.Caption = UserInfo.SYSUSERARBNAME;
                    //    ribbonStatusBar.ItemLinks[3].Item.Caption = UserInfo.BranchName;
                    //    ribbonStatusBar.ItemLinks[5].Item.Caption = UserInfo.FacilityName;

                    //}
                    //else
                    //{
                    //    this.RightToLeftLayout = false;
                    //    this.RightToLeft = RightToLeft.No;
                    //    ribbonStatusBar.ItemLinks[1].Item.Caption = UserInfo.SYSUSERENGNAME;
                    //    ribbonStatusBar.ItemLinks[3].Item.Caption = UserInfo.BranchName;
                    //    ribbonStatusBar.ItemLinks[5].Item.Caption = UserInfo.FacilityName;
                    //}
                    this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
                    AlertButton btn1 = new AlertButton();
                    btn1.Hint = "Open file";
                    btn1.Name = "buttonOpen";
                    alertControl1.Buttons.Add(btn1);
                    GetListTaskToCurrentUser();
                }
                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.English ? "An application instance is already running " : "النظام قيد التشغيل بالفعل لايمكن فتحة اكثر من مره "));
                    this.Close();
                }
            }

        }

        public void GetListTaskToCurrentUser()
        {
            


            navFavorit[0] = new DevExpress.XtraNavBar.NavBarItem();
            navFavorit[0].Caption = "1- لم يتم اإنشاء نسخة احتياطيه منذو اسبوع";
            navFavorit[0].ImageOptions.LargeImageIndex = 72;
            navFavorit[0].ImageOptions.SmallImageIndex = 72;
            navFavorit[0].Name = "navfrmFavoritItems";
            navFavorit[0].Tag = "StockObjects.Codes.frmFavoritItems";
            navFavorit[0].LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.OpenFavoritForm);
            WorningGroup.ItemLinks.Add(navFavorit[0]);
           


            navFavorit[1] = new DevExpress.XtraNavBar.NavBarItem();
            navFavorit[1].Caption = "2-هناك تنبيه من إدارة الموارد البشرية بتغير كلمة المرور";
            navFavorit[1].ImageOptions.LargeImageIndex = 72;
            navFavorit[1].ImageOptions.SmallImageIndex = 72;
            navFavorit[1].Name = "navfrmFavoritItems1";
            navFavorit[1].Tag = "StockObjects.Codes.frmFavoritItems1";
            navFavorit[1].LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.OpenFavoritForm);
            WorningGroup.ItemLinks.Add(navFavorit[1]);

            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);
            WorningGroup.ItemLinks.Add(navFavorit[1]);

        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Do you really want to exit " : "هل تريد اغلاق النظام"));
                if (Yes)
                {
                    SaveLoginHistory(3);//خروج من النظام
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
        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(mnufrmColorSeting, true);
        }

        private void SaveLoginHistory(int Statues)
        {
            Lip.NewFields();
            Lip.Table = "GLB_LOGINHISTORY";

            int ID = Comon.cInt(Lip.GetValue("Select max(ID) + 1 from GLB_LOGINHISTORY"));
            if (ID == 0)
                ID = 1;

            Lip.AddNumericField("ID", ID);
            Lip.AddStringField("DBNAME", cConnectionString.DataBasename);
            Lip.AddNumericField("BRANCHID", UserInfo.BRANCHID.ToString());
            Lip.AddNumericField("FACILITYID", UserInfo.FacilityID.ToString());
            Lip.AddNumericField("USERID", UserInfo.ID);
            Lip.AddNumericField("STATUS", Statues);
            Lip.AddNumericField("REGDATE", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
            Lip.AddNumericField("REGTIME", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
            Lip.AddStringField("COMPUTERINFO", "");
            Lip.ExecuteInsert();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            GetAcountsTree();
            lblNameUser.Text = UserInfo.SYSUSERARBNAME;
            lblNameFacility.Text = UserInfo.FacilityName;
          //  lblActiveName.Text = MySession.DBName.Substring(2, MySession.DBName.Length - 2);
            lblDate.Text = DateTime.Now.ToShortDateString();
             cmbBranch.SelectedValue = UserInfo.BRANCHID;

            cmbLangauage.SelectedIndex = UserInfo.Language == iLanguage.English ? 1 : 0;
            RTL();



            navFavorit[0] = new DevExpress.XtraNavBar.NavBarItem();
            navFavorit[0].Caption = "الأصناف المفضلة";
            navFavorit[0].ImageOptions.LargeImageIndex = 72;
            navFavorit[0].ImageOptions.SmallImageIndex = 72;
            navFavorit[0].Name = "navfrmFavoritItems";
            navFavorit[0].Tag = "StockObjects.Codes.frmFavoritItems";
            navFavorit[0].LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.OpenFavoritForm);
            mailGroup.ItemLinks.Add(navFavorit[0]);



            navFavorit[1] = new DevExpress.XtraNavBar.NavBarItem();
            navFavorit[1].Caption = "مجموعات الأصناف";
            navFavorit[1].ImageOptions.LargeImageIndex = 70;
            navFavorit[1].ImageOptions.SmallImageIndex = 70;
            navFavorit[1].Name = "navfrmItemsGroups";
            navFavorit[1].Tag = "StockObjects.Codes.frmItemsGroups";
            navFavorit[1].LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.OpenFavoritForm);
            mailGroup.ItemLinks.Add(navFavorit[1]);

           
            strSQL = "Select ArbName AS Argument, ItemID AS Value from STC_ITEMS";
            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            //عدد السجلات + الداتات + العنصر الذي سيتم عمل فيه التشارت في النموذج
          if(dt.Rows.Count>0)
            ChartLip.Chart(10,dt,pnlCharts);


          
          treeList1.OptionsView.EnableAppearanceEvenRow = true;
          treeList2.OptionsView.EnableAppearanceOddRow = true;
       
        }

        void AddItemNavBarControl() 
        {

            //DevExpress.XtraNavBar.NavBarItem obj = (DevExpress.XtraNavBar.NavBarItem)navBarControl.Items["outboxItem"];
            //obj.LinkClicked += navCalculator_LinkClicked;
            //DevExpress.XtraNavBar.NavBarItem items = new DevExpress.XtraNavBar.NavBarItem();
            //items.Name = "dffdf";
            //items.Caption = "aaassa";
            //items.LinkClicked += navCalculator_LinkClicked;
            //navBarControl.Groups["mailGroup"].ItemLinks.Add(items);


           
        }
        /****************************** Language *************************************/
        void Language()
        {

        }
        /****************************** User Permissions Menu & Froms & Reports ********************************************/
        void UserPermissions(int UserID, int BranshID, int FacilityID)
        {

            var Menu = UsersManagementDAL.frmGetAllUserMenusPermissions(UserID, BranshID, FacilityID);
            var Froms = UsersManagementDAL.frmGetAllUserFormsPermissions(UserID, BranshID, FacilityID);
            var Reports = UsersManagementDAL.frmGetAllUserReportsPermissions(UserID, BranshID, FacilityID);

            for (int i = 0; i < ribbonControl.Pages.Count; i++)
            {
                var itemMenu = Menu.FirstOrDefault(o => o.MenuName == ribbonControl.Pages[i].Name.Substring(3));
                ribbonControl.Pages[i].Visible = true;

                if (UserInfo.Language == iLanguage.English)
                {
                    ChangeLanguage.LTR(ribbonControl.Pages[i]);
                }
                if (itemMenu == null)
                {
                    ribbonControl.Pages[i].Visible = false;
                    continue;
                }
                else
                    if (itemMenu.MenuView == 0)
                    {
                        ribbonControl.Pages[i].Visible = false;
                        continue;
                    }
                foreach (RibbonPageGroup group in ribbonControl.Pages[i].Groups)
                {
                    if (UserInfo.Language == iLanguage.English)
                    {
                        ChangeLanguage.LTR(group);
                    }
                    if (group.Name == string.Concat(ribbonControl.Pages[i].Name, "Group3"))
                    {


                        foreach (BarItemLink link in group.ItemLinks)
                        {

                            if (UserInfo.Language == iLanguage.English)
                            {
                                ChangeLanguage.LTR((BarButtonItem)link.Item);
                            }

                            var itemReports = Reports.FirstOrDefault(o => o.ReportName == link.Item.Name.Substring(3));
                            if (itemReports == null)
                            {
                                link.Visible = false;
                                if (link.Item.Name.Contains("PopUpMenu"))
                                {
                                    link.Visible = true;
                                }

                                continue;
                            }
                            else
                                if (itemReports.ReportView == 0)
                                {
                                    link.Visible = false;
                                    continue;
                                }
                        }
                    }
                    else
                    {
                        foreach (BarItemLink link in group.ItemLinks)
                        {

                            if (UserInfo.Language == iLanguage.English)
                            {
                                ChangeLanguage.LTR((BarButtonItem)link.Item);
                            }

                            var itemFroms = Froms.FirstOrDefault(o => o.FormName.ToLower() == link.Item.Name.Substring(3).ToLower());

                            if (itemFroms == null)
                            {
                                link.Visible = false;
                                if (link.Item.Name.Contains("PopUpMenu"))
                                {
                                    link.Visible = true;
                                }
                                continue;
                            }
                            else
                                if (itemFroms.FormView == 0)
                                {
                                    link.Visible = false;
                                    continue;
                                }
                        }


                    }


                }

            }

        }

        private void mnuCalculator_ItemClick(object sender, ItemClickEventArgs e)
        {
            Process.Start(@"c:\WINDOWS\system32\calc.exe");
        }
        public void OpenFavoritForm(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
             

            string t1 = GetType().Namespace.Split('.')[0].ToString();
            string ScreenName = t1 + "." + e.Link.Item.Tag.ToString().Trim();
            Type t = Type.GetType(ScreenName);
            if (t != null)
            { 
                Form frm = Activator.CreateInstance(t) as Form;
                if (frm.IsDisposed == false)
                {
                    BaseForm Form = new BaseForm();
                    Form = (BaseForm)frm;
                   
                   
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        else
                            ChangeLanguage.ArabicLanguage(frm);
                        frm.Show();
                        frm.Text = e.Link.Item.Caption.ToString().Trim();
                        frm.Tag = 1;
                    }

                }
            }

        }

        private void navCalculator_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            DevExpress.XtraNavBar.NavBarItem obj = (DevExpress.XtraNavBar.NavBarItem)sender;
            var aa = obj.Name;
            Process.Start(@"c:\WINDOWS\system32\calc.exe");
        }

        /************************** ToolBar *********************************/
        private void btnSaveChangeToolBar_ItemClick(object sender, ItemClickEventArgs e)
        {
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            Path = Path + @"\DataXml\ToolBar.xml";
            ribbonControl.Toolbar.SaveLayoutToXml(Path);

        }
        void GetToolBar()
        {
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            Path = Path + @"\DataXml\ToolBar.xml";
            ribbonControl.Toolbar.RestoreLayoutFromXml(Path);
        }

        /************************** SkinName *********************************/
        private void mnufrmColorSeting_Gallery_ItemClick(object sender, GalleryItemClickEventArgs e)
        {
            SetSkinName(e.Item.Caption);
        }
        void GetSkinName()
        {
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            UserLookAndFeel.Default.SetSkinStyle(SystemSettings.GetSkinName(Path));
        }
        void SetSkinName(string SkinName)
        {
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            SystemSettings.SetSkinName(SkinName, Path);
            ConfigurationManager.RefreshSection("appSettings");
        }
        /************************************************************************************/
        private void mnufrmBranch_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void mnufrmItemsGroups_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void mnufrmPurchaseInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
           

        }

        private void mnufrmSizingUnits_ItemClick(object sender, ItemClickEventArgs e)
        {
             

        }

        private void mnufrmAccountsTree_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmSuppliers_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmPurchasesDelegates_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmCustomers_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmSalesDelegates_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmSellers_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptItemsList_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmChangePassword_ItemClick(object sender, ItemClickEventArgs e)
        {

             
        }

        private void mnufrmGoodsOpening_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptPurchaseInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmDeclaringMainAccounts_ItemClick(object sender, ItemClickEventArgs e)
        {
            

        }

        private void mnufrmBackupDataBase_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmGeneralOptions_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        


        private void mnufrmReceiptVoucher_ItemClick(object sender, ItemClickEventArgs e)
        {

             
        }

        private void mnufrmOpeningVoucher_ItemClick(object sender, ItemClickEventArgs e)
        {
            

        }

        private void mnufrmPrinterSelecter_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void mnufrmRestoringDeleted_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmLoginHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptStocktaking_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

      

        private void mnufrmUserPermissions_ItemClick(object sender, ItemClickEventArgs e)
        {
           

        }

        private void mnufrmSpendVoucher_ItemClick(object sender, ItemClickEventArgs e)
        {
             

        }

        private void mnufrmCheckSpendVoucher_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmCheckReceiptVoucher_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void mnufrmVariousVoucher_ItemClick(object sender, ItemClickEventArgs e)
        {

             
        }

        private void mnufrmChequesUnderCollection_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            

        }

        private void mnufrmSalesInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptLessCustomersBuying_ItemClick(object sender, ItemClickEventArgs e)
        {



            
        }

        private void mnurptSalesInPeriodByItem_ItemClick(object sender, ItemClickEventArgs e)
        {
 


        }

        private void mnurptLessSellerBuying_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void mnurptMostSellerBuying_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptDelegatesSelling_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptDetailedDelegateSales_ItemClick(object sender, ItemClickEventArgs e)
        {

             
        }

        private void mnurptLessDelegatesSelling_ItemClick(object sender, ItemClickEventArgs e)
        {

             
        }

        private void mnurptMostDelegatesSelling_ItemClick(object sender, ItemClickEventArgs e)
        {

             

        }

        private void mnurptItemSalePrice_ItemClick(object sender, ItemClickEventArgs e)
        {
            


        }

        private void mnurptSalesReturn_ItemClick(object sender, ItemClickEventArgs e)
        {
             


        }

        private void mnurptSalesInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            


        }

        private void mnurptMostCustomersBuying_ItemClick(object sender, ItemClickEventArgs e)
        {
            


        }

        private void mnurptPurchaseInvoiceReturn_ItemClick(object sender, ItemClickEventArgs e)
        {
             

        }

        private void mnurptPurchaseInPeriodByItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptMostSuppliersDealing_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurpLessSuppliersDealing_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void mnufrmPurchaseInvoiceReturn_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void mnufrmSalesInvoiceReturn_ItemClick(object sender, ItemClickEventArgs e)
        {


            
        }

        private void mnurptMinSoldItems_ItemClick(object sender, ItemClickEventArgs e)
        {

             
        }

        private void mnurptMaxSoldItems_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptMostReturnedItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptItemProfit_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptCanceledItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

      

        private void mnufrmEmployeeFiles_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmNationalities_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        

        private void mnufrmItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmItemsBases_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmItemsBrands_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmItemsSizes_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmItemsColors_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void mnufrmBarcodeUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmDeleteItemBarcodeExpiryDate_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptItemBalance_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptItemSN_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmStores_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnuPopUpMenuGoods_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void mnufrmItemsInOnBail_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmItemsOutOnBail_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void mnurptCostCenterAccountStatment_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptAccountStatement_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptSuppliersAccountStatement_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptSpecificAccountStatement_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void mnurptFinancialPositionStatement_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptIncomeStatement_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptBalanceReview_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptCustomersAccountStatement_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void mnufrmPrepareForNewAccountYear_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmCloseCashier_ItemClick(object sender, ItemClickEventArgs e)
        {

           
        }

        private void mnufrmCostCenter_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptReceiptVouchersReport_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptSpendVouchersReport_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptVariousVouchersReport_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptCheckSpendVouchersReport_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptCheckReceiptVouchersReport_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptPrintItemSticker_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

     

        private void mnufrmItemsDismantling_ItemClick(object sender, ItemClickEventArgs e)
        {

             
        }

        private void Exit_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Do you really want to exit " : "هل تريد اغلاق النظام"));
            if (Yes)
            {

                System.Windows.Forms.Application.Exit();
                Process.GetCurrentProcess().Kill();
            }
          
        }

        private void mnurptMinQtyLimitReport_ItemClick(object sender, ItemClickEventArgs e)
        {
              
        }

        private void mnurptMaxQtyLimitReport_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptDetailedDailyTransaction_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmCashierSales_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmNotifications_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmPriceOffersStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmPriceOffers_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmPriceOffersCustomers_ItemClick(object sender, ItemClickEventArgs e)
        {
           
         
        }

        private void mnurptStocktakingByQty_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnurptTaxReturnReport_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptStocktakingByStores_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnufrmItemsTransfer_ItemClick(object sender, ItemClickEventArgs e)
        {
              
        }

        private void mnufrmServicesRent_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            
        }
        private void alertControl1_BeforeFormShow(object sender, DevExpress.XtraBars.Alerter.AlertFormEventArgs e)
        {
            //Make the Alert Window opaque
            alertForm = e.AlertForm;
            
        }

        private void alertControl1_AlertClick(object sender, DevExpress.XtraBars.Alerter.AlertClickEventArgs e)
        {
          //  alertForm.Close();
           
        }

        private void alertControl1_ButtonClick(object sender, AlertButtonClickEventArgs e)
        {
             
            
            
            
            
            }

        private void btnExit_Click(object sender, EventArgs e)
        {

        }

      


   

       


       
        private void frmMain_Activated(object sender, EventArgs e)
        {
             
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        { 
            if (e.Node.GetValue(0) == null) return;
            var AcountNameAndID = e.Node.GetValue(0).ToString();
           // int AcountID = Comon.cInt(AcountNameAndID[0]);
             int AcountID = Comon.cInt(Lip.GetValue("Select MENUID from MAINMENU where ARBNAME ='" + AcountNameAndID.Trim() + "' or ENGCAPTION='" + AcountNameAndID.Trim() + "'"));
            if (AcountID > 0)
                GeSubAcountsTree(AcountID);
            treeList1.Appearance.SelectedRow.BackColor = Color.LightSteelBlue;
        }

        private void treeList2_DoubleClick(object sender, EventArgs e)
        {
            string t1 = GetType().Namespace.Split('.')[0].ToString();
            string ScreenName = t1 + "." + treeList2.FocusedNode.Tag.ToString().Trim();
            Type t = Type.GetType(ScreenName);
            if (t != null)
            {
                Form frm = Activator.CreateInstance(t) as Form;
                if (frm.IsDisposed == false)
                {
                    BaseForm Form = new BaseForm();
                    Form = (BaseForm)frm;
                    if (USERPERMATIONS.GET_FORMPERMATION(Form) == false)
                        Form.Dispose();
                    else
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                       
                        frm.Show();
                        frm.Text = treeList2.FocusedNode.GetValue(0).ToString();

                    }

                }
            }
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetAcountsTree();
        }

        private void btnSerch_Click(object sender, EventArgs e)
        {
           
        }

        private void btnShowSerchScreen_Click(object sender, EventArgs e)
        {
            
        }

        private void btnChangPassword_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword();
            frm.Show();
        }

        private void btnReturnLogin_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.Show();
            this.Hide();
        }


        public void RTL()
        {

            if (MySession.GlobalLanguageName == iLanguage.Arabic)
            {
                this.RightToLeftLayout = true;
                this.RightToLeft = RightToLeft.Yes;
                treeList1.RightToLeft = RightToLeft.Yes;
                treeList2.RightToLeft = RightToLeft.Yes;
               
              //  panel3.RightToLeft = RightToLeft.Yes;
                panel2.RightToLeft = RightToLeft.Yes;

               treeList1.Columns[0].Caption = "القائمة الرئيسية";
                this.Text = "نظام اومكس جولد ";
                lblBranch.Text = "الفرع";
                lblLanguage.Text = "اللغة";
                lblTheDate.Text = "التاريخ";
                capUserName.Text = " المستخدم";
                CapActiveName.Text = "النشاط";
                CapCompanyName.Text = "الشركة";
                
               // lblScreen.Text = "الشاشات المفضلة";

            }
            else
            {

                this.RightToLeftLayout = false;
                this.RightToLeft = RightToLeft.No;
                cmbLangauage.SelectedIndex = 1;
                treeList1.RightToLeft = RightToLeft.No;
                treeList2.RightToLeft = RightToLeft.No;
               //panel3.RightToLeft = RightToLeft.No;
                panel2.RightToLeft = RightToLeft.No;
                this.Text = "Omex   Gold";
                lblLanguage.Text = "Language";
                lblBranch.Text = "Branch";
                lblTheDate.Text = "Date";
                capUserName.Text = "User Name";
                CapActiveName.Text = "Active Name";
                CapCompanyName.Text = "Company Name";
              // lblScreen.Text = "Screen Vafurite";
                treeList1.Columns[0].Caption = "List Main";

            }
        }
        private void frmMain_Shown(object sender, EventArgs e)
        {
             
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public void GetAcountsTree()
        {
            List<MAINMENU> ListAccountsTree = new List<MAINMENU>();
            ListAccountsTree = Acc_AccountsDAL.GetMainMenu(UserInfo.BRANCHID, UserInfo.FacilityID);
            List<MyRecord> list = new List<MyRecord>();
            DataTable dtMenuPermission = new DataTable();

            if (txtSerchInTree.Text.ToString().Trim() != string.Empty)
                ListAccountsTree = ListAccountsTree.Where(n => n.ARBNAME.Contains(txtSerchInTree.Text.Trim())).ToList();
            if (ListAccountsTree != null)
            {
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    strSQL = ("SELECT     MENUVIEW FROM  MenusPermissions" + (" Where MENUVIEW=1 And ROLID=" + 1 + " And MENUID=" + ListAccountsTree[i].MENUID + ""));
                    dtMenuPermission = Lip.SelectRecord(strSQL);
                    if (dtMenuPermission.Rows.Count > 0)
                    {
                        if (Comon.cInt(dtMenuPermission.Rows[0]["MENUVIEW"].ToString()) == 1)
                        {
                            if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, /*ListAccountsTree[i].MENUID + "-" +*/ ListAccountsTree[i].ARBNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));
                            else
                                list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, /*ListAccountsTree[i].MENUID + "-" +*/ ListAccountsTree[i].ENGNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));
                        }
                    }
                }
                treeList1.DataSource = list;
                treeList1.SelectImageList = imageList1;
                treeList1.ImageIndexFieldName = "ImageIndex";
                treeList1.StateImageList = imageList1;
                int j = 0;
                foreach (TreeListNode node in treeList1.Nodes)
                {
                    // The left image displayed when the node is NOT focused.
                    node.ImageIndex = 0;
                    // The left image displayed when the node is focused.
                    node.SelectImageIndex = 1;
                    // The right image that does not depend on the focus.
                    node.StateImageIndex = 2;
                    treeList1.Nodes[j].Tag = ListAccountsTree[j].MENUID;
                    j++;
                }
            }
        }
        public void GeSubAcountsTree(int PARENTMENUID)
        {
            List<MAINMENU> ListAccountsTree = new List<MAINMENU>();
            ListAccountsTree =   Acc_AccountsDAL.GetByParent(PARENTMENUID);
            List<MyRecord> list = new List<MyRecord>();
            MyRecord Obj;
            foreach (var item in ListAccountsTree)
            {
                Obj = new MyRecord(item.MENUID, item.PARENTMENUID, item.ARBNAME, item.FORMNAME);
                list.Add(Obj);
            }
            if (ListAccountsTree != null)
            {
                treeList2.DataSource = list;
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    treeList2.Nodes[i].Tag = ListAccountsTree[i].FORMNAME;
                }
                treeList2.Columns[0].Caption = "القائمة الفرعية";
            }
        }
    }
}

   public class MyRecord
    {
        public long ID { get; set; }
        public long ParentID { get; set; }
        public string AcountName { get; set; }
        public string FORMNAME { get; set; }
        public MyRecord(long id, long parentID, string _AcountName,string _FORMNAME)
        {
            ID = id;
            ParentID = parentID;
            AcountName = _AcountName;
            FORMNAME = _FORMNAME;
        }
    }

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
using Edex.AccountsObjects.Codes;
using Edex.Manufacturing.Codes;
using System.Threading.Tasks;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
//using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using DevExpress.XtraNavBar;
using Edex.AccountsObjects.Reports;
using DevExpress.XtraTreeList;
using DevExpress.XtraWaitForm;
using DevExpress.XtraTab;
using DevExpress.XtraBars.Navigation;
using System.Reflection;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmMain : RibbonForm
    {
       public Form alertForm = null;
        public string PrimaryKeyName = "ArbName";
        public string strSQL = "";
        bool isNotLoadFirstTime = false;
        private DevExpress.XtraNavBar.NavBarItem[] navFavorit= new DevExpress.XtraNavBar.NavBarItem[30];
        public frmMain()
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
                    strSQL = "ArbName";
                    if (UserInfo.Language == iLanguage.English)
                    {
                        strSQL = "EngName";
                        PrimaryKeyName = "EngName";
                    }
                    this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
                    FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
                     
                    cmbBranchesID.EditValue = MySession.GlobalBranchID;
                    cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;
                    AlertButton btn1 = new AlertButton();
                    btn1.Hint = "Open file";
                    btn1.Name = "buttonOpen";
                    alertControl1.Buttons.Add(btn1);
                    GetListTaskToCurrentUser();
                    //Language();
                    //if (UserInfo.Language == iLanguage.English)
                    //    cmbLangauage.SelectedIndex = 0;
                    //else
                    //    cmbLangauage.SelectedIndex = 1;

                }
                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgWarning(Messages.TitleWorning, (UserInfo.Language == iLanguage.English ? "An application instance is already running " : "النظام قيد التشغيل بالفعل لايمكن فتحة اكثر من مره "));
                    this.Close();
                }
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
            Lip.Table = "LoginHistory";
            Lip.AddStringField("DBNAME", cConnectionString.DataBasename);
            Lip.AddNumericField("BRANCHID",  Comon.cInt(cmbBranchesID.EditValue).ToString());
            Lip.AddStringField("Password", "");
            Lip.AddNumericField("USERID", UserInfo.ID);
            Lip.AddNumericField("STATUS", Statues);
            Lip.AddNumericField("REGDATE", Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate())).ToString());
            Lip.AddNumericField("REGTIME", Comon.cDbl(Lip.GetServerTimeSerial()).ToString());
            Lip.AddStringField("COMPUTERINFO", UserInfo.ComputerInfo);
            Lip.ExecuteInsert();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
          
            xtraTabbedMdiManager1.MdiParent = this;

            GetAcountsTree();
            frmDashBourd frm = new frmDashBourd();
            frm.MdiParent = this;
            frm.ControlBox = false;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frm.Show();
            treeList1.Columns[1].Visible=false;
            //treeList2.Columns[1].Visible = false;
            
            lblNameUser.Text =UserInfo.Language==iLanguage.Arabic ? UserInfo.SYSUSERARBNAME:UserInfo.SYSUSERENGNAME;
            lblNameFacility.Text = UserInfo.FacilityName;
          //  lblActiveName.Text = MySession.DBName.Substring(2, MySession.DBName.Length - 2);
            lblDate.Text = DateTime.Now.ToShortDateString();
             cmbBranchesID.EditValue =MySession.GlobalBranchID;
            cmbLangauage.SelectedIndex = UserInfo.Language == iLanguage.English ? 1 : 0;
            RTL();

            DataTable dtFormPermission;
            strSQL = ("SELECT FormName,ArbCaption,EngCaption, FormFovorite FROM UserFormsFovorite Where BranchID =" +   Comon.cInt(cmbBranchesID.EditValue) + " And UserID=" + UserInfo.ID + " And FormFovorite=1");
            dtFormPermission = Lip.SelectRecord(strSQL);
            for (int i = 0; i < dtFormPermission.Rows.Count; i++)
            {
                 navFavorit[i] = new DevExpress.XtraNavBar.NavBarItem();
                 navFavorit[i].ImageOptions.LargeImageIndex = 72;
                 navFavorit[i].ImageOptions.SmallImageIndex = 72;
                 navFavorit[i].Caption = dtFormPermission.Rows[i]["ArbCaption"].ToString();
                 navFavorit[i].Name = dtFormPermission.Rows[i]["FormName"].ToString();
                 navFavorit[i].Tag = dtFormPermission.Rows[i]["EngCaption"].ToString();              
                 navFavorit[i].LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.OpenFavoritForm);
                 mailGroup.ItemLinks.Add(navFavorit[i]);
            }
            
            if (UserInfo.Language == iLanguage.English)
            {
                navBarControl.RightToLeft=RightToLeft.No;
                mailGroup.Caption = "Favorite";
            }
            for (int i = 0; i < mailGroup.ItemLinks.Count; i++)
            {
                if (UserInfo.Language == iLanguage.English && mailGroup.ItemLinks[i].Item.Tag!=null)
                    mailGroup.ItemLinks[i].Item.Caption = mailGroup.ItemLinks[i].Item.Tag.ToString();
               
            }
            //foreach (Type formType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsSubclassOf(typeof(Form))))
            //{
            //    MessageBox.Show(formType.Name);
            //}
            


          
          treeList1.OptionsView.EnableAppearanceEvenRow = true;
          //treeList2.OptionsView.EnableAppearanceOddRow = true;
       
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
             
            string formName = e.Link.Item.Name.ToString().Trim();
            foreach (Form form in Application.OpenForms)
            {
                if (form.Text == formName)
                {
                    form.Activate();
                    return;
                }
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type formType = assembly.GetTypes().FirstOrDefault(type => type.Name == formName && type.IsSubclassOf(typeof(Form)));
            if (formType != null)
            {
                BaseForm frm = (BaseForm)Activator.CreateInstance(formType);
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);

                    frm.ControlBox = false;
                    frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    frm.Show();
                }
              else
                frm.Dispose();
            }
 
            //string t1 = GetType().Namespace.Split('.')[0].ToString();
            //string ScreenName = t1 + "." + e.Link.Item.Name.ToString().Trim();
            //Type t = Type.GetType(ScreenName);
            //if (t != null)
            //{ 
            //    Form frm = Activator.CreateInstance(t) as Form;
            //    if (frm.IsDisposed == false)
            //    {
            //        BaseForm Form = new BaseForm();
            //        Form = (BaseForm)frm;
                   
                   
            //        {
            //            if (UserInfo.Language == iLanguage.English)
            //                ChangeLanguage.EnglishLanguage(frm);
            //            else
            //                ChangeLanguage.ArabicLanguage(frm);
            //            frm.Show();
            //            frm.Text = e.Link.Item.Caption.ToString().Trim();
            //            frm.Tag = 1;
            //        }

            //    }
            //}

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
            frmBranches frm = new frmBranches();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

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
    public static    bool CheckCurrntForm( BaseForm frm)
        {
            foreach (Form form1 in Application.OpenForms)
            {
                if (form1.GetType().Name == frm.GetType().Name  )
                {
                    form1.Activate();

                    return true ;
                }
            }
            return false;
        }
        private void mnufrmUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmUser frm = new frmUser();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void mnufrmChangePassword_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmChangePassword frm = new frmChangePassword();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        private void mnufrmGoodsOpening_ItemClick(object sender, ItemClickEventArgs e)
        {
             
        }

        private void mnurptPurchaseInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void mnufrmDeclaringMainAccounts_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmDeclaringMainAccounts frm = new frmDeclaringMainAccounts();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        private void mnufrmBackupDataBase_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBackupDataBase frm = new frmBackupDataBase();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void mnufrmGeneralOptions_ItemClick(object sender, ItemClickEventArgs e)
        {

            frmGeneralOptions frm = new frmGeneralOptions();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }




        private void mnufrmReceiptVoucher_ItemClick(object sender, ItemClickEventArgs e)
        {

             
        }

         

        private void mnufrmPrinterSelecter_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmPrinterSelecter frm = new frmPrinterSelecter();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

        }

        private void mnufrmRestoringDeleted_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmRestoringDeleted frm = new frmRestoringDeleted();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void mnufrmLoginHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLoginHistory frm = new frmLoginHistory();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void mnurptStocktaking_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

      

        private void mnufrmUserPermissions_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmUserPermissions frm = new frmUserPermissions();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

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
            frmUser frm = new frmUser();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();

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
            frmNotifications frm = new frmNotifications();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
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
            try
            {
                if (e.Node.Nodes.Count == 1) return;

                var AcountNameAndID = Comon.cInt(e.Node.GetValue(0).ToString().Split('-')[0]);
                int AcountID = Comon.cInt(AcountNameAndID);
                //if (AcountID > 0)
                //    GeSubAcountsTree(AcountID);
                treeList1.Appearance.SelectedRow.BackColor = Color.LightSteelBlue;
            }
            catch(Exception ex)
            {
                Messages.MsgWarning(Messages.TitleWorning, ex.Message);
            }
        }

        private void treeList2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeListNode focusedNode = ((TreeList)sender).FocusedNode;
                int nodeIndex = focusedNode.Level;
                if (nodeIndex == 2)
                {
                    string t1 = GetType().Namespace.Split('.')[0].ToString();
                    string ScreenName = t1 + "." + ((TreeList)sender).FocusedNode.GetValue(1).ToString().Trim();
                    Type t = Type.GetType(ScreenName);
                    if (t != null)
                    {
                        BaseForm frm = Activator.CreateInstance(t) as BaseForm;

                        if (frm.IsDisposed == false)
                        {

                            BaseForm Form = new BaseForm();
                            Form = (BaseForm)frm;
                            if (Permissions.UserPermissionsFrom(Form, Form.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                            {
                                if (UserInfo.Language == iLanguage.English)
                                    ChangeLanguage.EnglishLanguage(frm);

                                if (((TreeList)sender).FocusedNode.GetValue(0).ToString() == (UserInfo.Language == iLanguage.Arabic ? "بوليشن 1" : "Polytion1"))
                                {
                                    ((frmManufacturingTalmee)frm).cmbPollutionTypeID.EditValue = 1;
                                }
                                else if (((TreeList)sender).FocusedNode.GetValue(0).ToString() == (UserInfo.Language == iLanguage.Arabic ? "بوليشن 2" : "Polytion2"))
                                {
                                    ((frmManufacturingTalmee)frm).cmbPollutionTypeID.EditValue = 2;
                                }
                                else if (((TreeList)sender).FocusedNode.GetValue(0).ToString() == (UserInfo.Language == iLanguage.Arabic ? "بوليشن 3" : "Polytion3"))
                                {
                                    ((frmManufacturingTalmee)frm).cmbPollutionTypeID.EditValue = 3;
                                }

                                else if (((TreeList)sender).FocusedNode.GetValue(0).ToString() == (UserInfo.Language == iLanguage.Arabic ? "البرنتـــاج 1" : "Albernage1"))
                                {
                                    ((frmManufacturingPrentag)frm).cmbPrntageTypeID.EditValue = 1;
                                }
                                else if (((TreeList)sender).FocusedNode.GetValue(0).ToString() == (UserInfo.Language == iLanguage.Arabic ? "البرنتـــاج 2" : "Albernage2"))
                                {
                                    ((frmManufacturingPrentag)frm).cmbPrntageTypeID.EditValue = 2;
                                }


                                else if (((TreeList)sender).FocusedNode.GetValue(0).ToString() == (UserInfo.Language == iLanguage.Arabic ? "كشف حساب عملاء" : "Customer account statement"))
                                {


                                    ((frmCustomersAccountStatement)frm).chkCustomer.Checked = true;
                                }

                                else if (((TreeList)sender).FocusedNode.GetValue(0).ToString() == (UserInfo.Language == iLanguage.Arabic ? "كشف حساب موردين" : "Suppliers account statement"))
                                {


                                    ((frmCustomersAccountStatement)frm).chkSupliar.Checked = true;
                                }


                                string linkText = ((TreeList)sender).FocusedNode.GetValue(0).ToString();
                                foreach (Form form in Application.OpenForms)
                                {
                                    if (form.Text == linkText)
                                    {
                                        form.Activate();
                                        return;
                                    }
                                }

                                frm.MdiParent = this;
                                frm.Show();
                                frm.CancelButton = null;
                                //frm.ControlBox = false;
                                frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                                frm.Text = ((TreeList)sender).FocusedNode.GetValue(0).ToString();
                                NavBarItem link = new NavBarItem(linkText);
                                link.LinkClicked += Link_LinkClicked;
                                link.SmallImage = new Icon(frm.Icon, new Size(32, 32)).ToBitmap();
                                navBarGroup1.ItemLinks.Add(link);

                            }
                            else
                                frm.Dispose();
                            //if (USERPERMATIONS.GET_FORMPERMATION(Form) == false)
                            //    Form.Dispose();
                            //else
                            //{
                            //    if (UserInfo.Language == iLanguage.English)
                            //        ChangeLanguage.EnglishLanguage(frm);

                            //    frm.Show();
                            //    frm.Text = ((TreeList) sender).FocusedNode.GetValue(0).ToString();
                            //}


                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Messages.MsgError(Messages.TitleError, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void Link_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            NavBarItem link = e.Link.Item as NavBarItem;
            string formName = link.Caption;

            foreach (Form form in Application.OpenForms)
            {
                if (form.Text == formName)
                {
                    form.Activate();
                    return;
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
            frmScreenSearch frm = new frmScreenSearch();
            frm.Show();
           
        }

        private void btnShowSerchScreen_Click(object sender, EventArgs e)
        {

            frmScreenManagement frm = new frmScreenManagement();
            frm.FormUpdate = true;
            frm.FormAdd = true;
            frm.FormDelete = true;
            frm.Show();
            
        }

        private void btnChangPassword_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void btnReturnLogin_Click(object sender, EventArgs e)
        {
            frmLoginWeb frm = new frmLoginWeb();
            frm.Show();
            this.Hide();
        }


        public void RTL()
        {

            if (UserInfo.Language == iLanguage.Arabic)
            {
                this.RightToLeftLayout = true;
                this.RightToLeft = RightToLeft.Yes;
                treeList1.RightToLeft = RightToLeft.Yes;
                
              //  panel3.RightToLeft = RightToLeft.Yes;
                panel2.RightToLeft = RightToLeft.Yes;

               treeList1.Columns[0].Caption = "القائمة الرئيسية";
                this.Text = "نظام اومكس جولد ";
                
                lblLanguage.Text = ":اللغة";
                lblTheDate.Text = ":التاريخ";
                capUserName.Text = " :المستخدم";
                CapActiveName.Text = ":السنة المالية";
                CapCompanyName.Text = "الشركة";
                lblBranch.Text = "الفرع ";

                
               // lblScreen.Text = "الشاشات المفضلة";
            }
            else
            {

                this.RightToLeftLayout = false;
                this.RightToLeft = RightToLeft.No;
                cmbLangauage.SelectedIndex = 1;
                treeList1.RightToLeft = RightToLeft.No; 
               //panel3.RightToLeft = RightToLeft.No;
                panel2.RightToLeft = RightToLeft.No;
                this.Text = "Omex   Gold";
                lblLanguage.Text = "Language:";
                lblTheDate.Text = "Date:";
                capUserName.Text = "User Name:";
                CapActiveName.Text = "Fiscal year:";
                CapCompanyName.Text = "Company Name:";
              // lblScreen.Text = "Screen Vafurite";
                treeList1.Columns[0].Caption = "List Main";
                lblBranch.Text = "Branch ";

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
            ListAccountsTree = Acc_AccountsDAL.GetMainMenu( Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
            List<MyRecord> list = new List<MyRecord>();
            DataTable dtMenuPermission = new DataTable();

            //if (txtSerchInTree.Text.ToString().Trim() != string.Empty)
            //    ListAccountsTree = ListAccountsTree.Where(n => n.ARBNAME.Contains(txtSerchInTree.Text.Trim())).ToList();
            if (ListAccountsTree != null)
            {
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    if (ListAccountsTree[i].MENULEVELID >= 1)
                    {
                        if (ListAccountsTree[i].MENULEVELID == 3)
                           list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID,   (UserInfo.Language == iLanguage.Arabic ? ListAccountsTree[i].ARBNAME.ToString().Trim() : ListAccountsTree[i].ENGNAME.ToString().Trim()), ListAccountsTree[i].FORMNAME));
                        else list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].MENUID + "-" + (UserInfo.Language == iLanguage.Arabic ? ListAccountsTree[i].ARBNAME.ToString().Trim() : ListAccountsTree[i].ENGNAME.ToString().Trim()), ListAccountsTree[i].FORMNAME));
                          
                    }
                    else
                    {
                        strSQL = ("SELECT     MENUVIEW FROM  UserMenusPermissions" + " Where MENUVIEW=1 And BranchID="+Comon.cInt(cmbBranchesID.EditValue)+" USERID=" + UserInfo.ID + " And MENUNAME='" + ListAccountsTree[i].MENUNAMEPERMATION + "'");
                        dtMenuPermission = Lip.SelectRecord(strSQL);
                        if (dtMenuPermission.Rows.Count > 0)
                        {
                            if (Comon.cInt(dtMenuPermission.Rows[0]["MENUVIEW"].ToString()) == 1)
                            {
                                if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                {
                                    if (ListAccountsTree[i].MENULEVELID == 3)
                                        list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].ARBNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));
                                    else
                                        list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].MENUID + "-" + ListAccountsTree[i].ARBNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));
                                }
                                else
                                {
                                    if (ListAccountsTree[i].MENULEVELID == 3)
                                        list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID,   ListAccountsTree[i].ENGNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));
                                    else
                                      list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].MENUID + "-" + ListAccountsTree[i].ENGNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));
                                }
                                }

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
                Obj = new MyRecord(item.MENUID, item.PARENTMENUID, UserInfo.Language == iLanguage.Arabic ? item.ARBNAME : item.ENGNAME, item.FORMNAME);
                list.Add(Obj);
            }
            //if (ListAccountsTree != null)
            //{
            //    treeList2.DataSource = list;
            //    for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
            //    {
            //        treeList2.Nodes[i].Tag = ListAccountsTree[i].FORMNAME;                   
            //    }
            //    treeList2.Columns[0].Caption = "القائمة الفرعية";
            //}
        }
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

        private void navBarControl_Click(object sender, EventArgs e)
        {

        }

        private void navBarItemStageManufactory_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            frmManufacturingStages frm = new frmManufacturingStages();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                            
                frm.Show();
            }
            else
                frm.Dispose();
        }
        //private static readonly HttpClient client = new HttpClient();
        public static async Task Main1(string CurrncyCode)
        {
            string apiKey = "goldapi-7qsi91sltw42cf2-io";
            //string apiKey = "goldapi-1hvdo4sltwgqtfh-io";
            string symbol = "XAU";
            string curr = CurrncyCode;
            string date = "";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-access-token", apiKey);

                string url = "https://www.goldapi.io/api/" + symbol + "/" + curr + date;

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                   
                    dynamic data = JsonConvert.DeserializeObject(responseBody);
                    MySession.GlobalDefaultPricePerGram24k = data.price_gram_24k;
                    MySession.GlobalDefaultPricePerGram21k = data.price_gram_21k;
                    MySession.GlobalDefaultPricePerGram16k = data.price_gram_16k;
                    MySession.GlobalDefaultPricePerGram18k = data.price_gram_18k;
                    MySession.GlobalDefaultPricePerGram22k = data.price_gram_22k;
                    MySession.GlobalDefaultPricePerGram14k = data.price_gram_14k;
                }
                catch (Exception ex)
                {
                    Messages.MsgWarning(Messages.TitleWorning, "Error: " + ex.Message);
                }
            }
        }

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
         
        }

           
    private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count > 0)
            {
                Form[] openForms = Application.OpenForms.Cast<Form>().ToArray();
                foreach (Form form in openForms)
                {

                    if (form is frmDashBourd)
                        continue;
                    if (form != this )
                    {
                        foreach (NavBarItemLink link in navBarGroup1.ItemLinks.ToArray())
                        {
                            if (link.Item.Caption == form.Text)
                            {
                                navBarGroup1.ItemLinks.Remove(link);
                            }
                        }
                        if (form is WaitForm)
                            continue;
                        else
                            form.Close();
                    }
                }
            }
        }

        
        

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSelectScrrenFovorit frm = new frmSelectScrrenFovorit();
            if (CheckCurrntForm(frm))
            { frm.Dispose(); return; }
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID,  Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);


                frm.Show();
            }
            else
                frm.Dispose();
        }
    }
}

   //public class MyRecord
   // {
   //     public long ID { get; set; }
   //     public long ParentID { get; set; }
   //     public string AcountName { get; set; }
   //     public MyRecord(long id, long parentID, string _AcountName)
   //     {
   //         ID = id;
   //         ParentID = parentID;
   //         AcountName = _AcountName;
   //     }
   // }

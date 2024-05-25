﻿using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList.Nodes;
using Edex.DAL;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace   Edex.GeneralObjects.GeneralForms
{
    public partial class frmScreenManagement :  Edex.GeneralObjects.GeneralForms.BaseForm
    {


        /**************** Declare ************************/
        #region Declare

        private string strSQL;
        private bool IsNewRecord;
        string FocusedControl = "";

        string PrimaryName = "ArbName";
        public bool IsFromanotherForms = false;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public int LISTID = 0;
        XtraTabPage prevPage;
        #endregion
        /****************Form Event************************/
        #region Form Event

        public frmScreenManagement()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
        }



        #endregion
        /**********************Function**************************/
        #region Function


        //public void GetSelectedSearchValue(CSearch cls)
        //{

        //}

        public void ClearFields()
        {
            try
            {
                if (txtNoMainScreen.Text == string.Empty) return;
                txtOrder.Text = Lip.GetValue("Select Max(ID) + 1 From MAINMENU");
                txtNameScreen.Text = "";
                txtNoScreen.Text = Lip.GetValue("Select Max(MENUID) + 1 From MAINMENU Where PARENTMENUID=" + txtNoMainScreen.Text); ;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        /******************** MoveRec ************************/



        /*******************Do Functions *************************/

        protected override void DoSave()
        {
            try
            {

                if (!Validations.IsValidForm(this))
                    return;
                
                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;
                }
                else if (!IsNewRecord)
                {
                    if (!FormUpdate)
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                        return;
                    }
                    else
                    {
                        bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, Messages.msgConfirmUpdate);
                        if (!Yes)
                            return;
                    }

                }

                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                save();

                Validations.EnabledControl(this, false);
                if (IsNewRecord == true)
                    DoNew();
                else
                    Validations.DoSaveRipon(this, ribbonControl1);
            }
            catch (Exception ex)
            {

                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        protected override void DoEdit()
        {
            Validations.DoEditRipon(this, ribbonControl1);
            Validations.EnabledControl(this, true);
             
        }
        public void save()
        {

            Lip.NewFields(); 
            int ID = Comon.cInt(txtOrder.Text);
            Lip.Table = "MAINMENU";
            Lip.AddNumericField("ID", Comon.cInt(txtOrder.Text));
            Lip.AddNumericField("MENUID", txtNoScreen.Text);
            Lip.AddStringField("ARBNAME", txtNameScreen.Text.Trim());
            Lip.AddStringField("ENGNAME", txtFile.Text.Trim());
            Lip.AddNumericField("PARENTMENUID", txtNoMainScreen.Text.Trim());
            Lip.AddNumericField("DELETED", 0);
            Lip.AddStringField("COMPUTERINFO", UserInfo.ComputerInfo);
            Lip.AddNumericField("MENULEVELID",txtLevelID.Text);
            Lip.AddNumericField("MENUTYPEID", "1");
            Lip.AddStringField("FORMNAME", txtFormName.Text.Trim());
            Lip.AddStringField("ORDERBYID", "0");

            Lip.AddStringField("FACILITYID",MySession.GlobalFacilityID.ToString());
            Lip.AddStringField("BRANCHID", MySession.GlobalBranchID.ToString());

            //User Data
            Lip.AddNumericField("USERCREATED", UserInfo.ID);
            Lip.AddNumericField("DATECREATED", Comon.cInt(Lip.GetServerDateSerial()));
            Lip.AddNumericField("TIMECREATED", Comon.cInt(Lip.GetServerTimeSerial()));
            if (IsNewRecord == true)
            {
                Lip.AddNumericField("USERUPDATED", 0);
                Lip.AddNumericField("DATEUPDATED", 0);
                Lip.AddNumericField("TIMEUPDATED", 0);
                Lip.AddStringField("EDITCOMPUTERINFO", "");
            }

            if (IsNewRecord == false)
            {
                Lip.AddNumericField("USERUPDATED", UserInfo.ID);
                Lip.AddNumericField("DATEUPDATED", Comon.cInt(Lip.GetServerDateSerial()));
                Lip.AddNumericField("TIMEUPDATED", Comon.cInt(Lip.GetServerTimeSerial()));
                Lip.AddStringField("EDITCOMPUTERINFO", UserInfo.ComputerInfo);
                Lip.sCondition = " MenuID=" + txtNoScreen.Text;
            }

            if (IsNewRecord == true)
                Lip.ExecuteInsert();
            else
                Lip.ExecuteUpdate();

            if (IsFromanotherForms == false)
            {

                SplashScreenManager.CloseForm();
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                if (IsNewRecord == true)
                    DoNew();


            }
        }
        #endregion
        /**********************Event**************************/
        #region Event

        #endregion

        private void frmScreenManagement_Load(object sender, EventArgs e)
        {
           
            Validations.DoLoadRipon(this, ribbonControl1);
            Validations.EnabledControl(this, false);
            FormsPrperties.ColorForm(this);
            ribbonControl1.Items[19].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//اضافة من
            ribbonControl1.Items[11].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//التصدير  

            GetAcountsTree();
            treeList1.Columns[1].Visible = false;
            //treeList2.Columns[1].Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tab_Click(object sender, EventArgs e)
        {

        }
        public void GetAcountsTree()
        {
            try
            {
                List<MAINMENU> ListAccountsTree = new List<MAINMENU>();
                ListAccountsTree = Acc_AccountsDAL.GetMainMenu(UserInfo.BRANCHID, UserInfo.FacilityID, 2);
                List<MyRecord> list = new List<MyRecord>();
                DataTable dtMenuPermission = new DataTable();

                if (txtSerchInTree.Text.ToString().Trim() != string.Empty)
                    ListAccountsTree = ListAccountsTree.Where(n => n.ARBNAME.Contains(txtSerchInTree.Text.Trim())).ToList();
                if (ListAccountsTree != null)
                {
                    for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                    {
                        if (ListAccountsTree[i].MENULEVELID <= 2)
                        {
                            list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].MENUID + "-" + ListAccountsTree[i].ARBNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));

                        }
                        else
                        {

                            if (Comon.cInt(dtMenuPermission.Rows[0]["MENUVIEW"].ToString()) == 1)
                            {
                                if (MySession.GlobalLanguageName == iLanguage.Arabic)
                                    list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].MENUID + "-" + ListAccountsTree[i].ARBNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));
                                else
                                    list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].MENUID + "-" + ListAccountsTree[i].ENGNAME.ToString().Trim(), ListAccountsTree[i].FORMNAME));

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
            catch { }
        }
        public void GeSubAcountsTree(int PARENTMENUID)
        {
            List<MAINMENU> ListAccountsTree = new List<MAINMENU>();
            ListAccountsTree = Acc_AccountsDAL.GetByParent(PARENTMENUID);
            List<MyRecord> list = new List<MyRecord>();
            MyRecord Obj;
            foreach (var item in ListAccountsTree)
            {
                Obj = new MyRecord(item.MENUID, item.PARENTMENUID, item.ARBNAME,item.FORMNAME);
                list.Add(Obj);
            }
            if (ListAccountsTree != null)
            {
                treeList2.DataSource = list;
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    treeList2.Nodes[i].Tag = ListAccountsTree[i].MENUID;
                }
                treeList2.Columns[0].Caption = "القائمة الفرعية";
            }
            treeList2.Columns[1].Visible = false;
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (e.Node.Nodes.Count == 1) return;
                var AcountNameAndID = Comon.cInt(e.Node.GetValue(0).ToString().Split('-')[0]);

                int AcountID = Comon.cInt(AcountNameAndID);
                if (AcountID > 0)
                    GeSubAcountsTree(AcountID);
                treeList1.Appearance.SelectedRow.BackColor = Color.LightSteelBlue;

            }
            catch
            {

            }


        }
        public void ReadRecord(int MenuID)
        {
            try
            {
                IsNewRecord = false;
                {


                    txtNoScreen.Text = MenuID.ToString();


                    DataTable dt = Lip.SelectRecord("Select * from MAINMENU where MENUID =" + MenuID);
                    if (dt.Rows.Count > 0)
                    {
                        txtNoMainScreen.Text = dt.Rows[0]["PARENTMENUID"].ToString();
                        string ScreenMain = Lip.GetValue("Select ARBNAME from MAINMENU where MENUID =" + txtNoMainScreen.Text);
                        txtScreenMain.Text = ScreenMain;


                        string ScreenArbName = dt.Rows[0]["ARBNAME"].ToString();
                        txtNameScreen.Text = ScreenArbName;

                        txtFile.Text = dt.Rows[0]["ENGNAME"].ToString();

                        string ScreenMenuID = Lip.GetValue("Select PARENTMENUID from MAINMENU where MENUID =" + txtNoMainScreen.Text);
                        txtMenuID.Text = ScreenMenuID;


                        string ScreenMenuName = Lip.GetValue("Select ARBNAME from MAINMENU where MENUID =" + txtMenuID.Text);
                        txtMenuName.Text = ScreenMenuName;

                        if (Comon.cInt(dt.Rows[0]["Deleted"].ToString()) == 0)
                            chkStopMenu.Checked = false;
                        else
                            chkStopMenu.Checked = true;



                        txtFormName.Text = dt.Rows[0]["FORMNAME"].ToString();
                        txtLevelID.Text = dt.Rows[0]["MENULEVELID"].ToString();


                        IsNewRecord = false;

                        txtOrder.Text = dt.Rows[0]["ID"].ToString();


                        Validations.EnabledControl(this, false);
                        Validations.DoReadRipon(this, ribbonControl1);
                    }

                }
            }
            catch (Exception ex)
            {
                //Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void treeList2_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            ClearFields();
            if (treeList2.FocusedNode.Tag != null)
            {
                string t1 = GetType().Namespace.Split('.')[0].ToString();
                string ScreenName = t1 + "." + treeList2.FocusedNode.Tag.ToString().Trim();
                Type t = Type.GetType(ScreenName);
            

                string MenuID = treeList2.FocusedNode.Tag.ToString().Trim();

                ReadRecord(Comon.cInt(MenuID));


                ribbonControl1.Items[2].Enabled = true;//اضافة من
               


            }
        }

        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                ClearFields();
                Validations.EnabledControl(this, true);
                Validations.DoNewRipon(this, ribbonControl1);
                
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void treeList2_RowClick(object sender, DevExpress.XtraTreeList.RowClickEventArgs e)
        {
            ClearFields();
            if (treeList2.FocusedNode.Tag != null)
            {
                string t1 = GetType().Namespace.Split('.')[0].ToString();
                string ScreenName = t1 + "." + treeList2.FocusedNode.Tag.ToString().Trim();
                Type t = Type.GetType(ScreenName);
                string name = treeList2.FocusedNode.GetValue(0).ToString();
                string h = treeList2.FocusedNode.GetDisplayText(0).ToString();
                string MenuID = h.Split('-')[0].ToString();
                txtNoScreen.Text = MenuID;
                ReadRecord(Comon.cInt(MenuID));
                IsNewRecord = false;
            }
        }
    }
}

     
 
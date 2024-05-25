 
using DevExpress.XtraTreeList;
using Edex.AccountsObjects.Reports;
using Edex.DAL;
using Edex.Manufacturing.Codes;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmScreenSearch : Form
    {
        public frmScreenSearch()
        {
            InitializeComponent();
            if(UserInfo.Language==iLanguage.English)
            {
                treeListColumn2.Caption = "Submenu ";
                
                ChangeLanguage.LTR(this);
                treeList2.RightToLeft = RightToLeft.No;
            }
            this.Load += frmScreenSearch_Load;
        }

        void frmScreenSearch_Load(object sender, EventArgs e)
        {
            txtSerchInTree.Focus();
        }

        private void txtSerchInTree_Validated(object sender, EventArgs e)
        {
            ////GeSubAcountsTree(0);
        }
        public void GeSubAcountsTree(int PARENTMENUID)
        {
            List<MAINMENU> ListAccountsTree = new List<MAINMENU>();
            ListAccountsTree = Acc_AccountsDAL.GetMainMenuSub(MySession.GlobalBranchID, UserInfo.FacilityID);
            List<MyRecord> list = new List<MyRecord>();

            if (txtSerchInTree.Text.ToString().Trim() != string.Empty)
                if(UserInfo.Language==iLanguage.Arabic)
                    ListAccountsTree = ListAccountsTree.Where(n => n.ARBNAME.Contains(txtSerchInTree.Text.Trim())).ToList();
                else if (UserInfo.Language == iLanguage.English)
                    ListAccountsTree = ListAccountsTree.Where(n => n.ENGNAME.Contains(txtSerchInTree.Text.Trim())).ToList();

            if (ListAccountsTree != null)
            {
                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    if (MySession.GlobalLanguageName == iLanguage.Arabic)
                        list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].MENUID + " - " + ListAccountsTree[i].ARBNAME.ToString(), ListAccountsTree[i].FORMNAME));
                    else
                        list.Add(new MyRecord(ListAccountsTree[i].MENUID, ListAccountsTree[i].PARENTMENUID, ListAccountsTree[i].MENUID + " - " + ListAccountsTree[i].ENGNAME.ToString(), ListAccountsTree[i].FORMNAME));



                }


                treeList2.DataSource = list;

                for (int i = 0; i <= ListAccountsTree.Count - 1; i++)
                {
                    treeList2.Nodes[i].Tag = ListAccountsTree[i].FORMNAME;
                }

                treeList2.Appearance.OddRow.BackColor = Color.LightPink;
                treeList2.OptionsView.EnableAppearanceOddRow = true;

            }

        }

        private void treeList2_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

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
                    if (Permissions.UserPermissionsFrom(Form, Form.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
 
                        if (((TreeList)sender).FocusedNode.GetValue(0).ToString().Split('-')[1].Trim() == (UserInfo.Language == iLanguage.Arabic ? "بوليشن 1" : "Polytion1"))
                        {
                            ((frmManufacturingTalmee)frm).cmbPollutionTypeID.EditValue = 1;
                        }
                        else if (((TreeList)sender).FocusedNode.GetValue(0).ToString().Split('-')[1].Trim() == (UserInfo.Language == iLanguage.Arabic ? "بوليشن 2" : "Polytion2"))
                        {
                            ((frmManufacturingTalmee)frm).cmbPollutionTypeID.EditValue = 2;
                        }
                        else if (((TreeList)sender).FocusedNode.GetValue(0).ToString().Split('-')[1].Trim() == (UserInfo.Language == iLanguage.Arabic ? "بوليشن 3" : "Polytion3"))
                        {
                            ((frmManufacturingTalmee)frm).cmbPollutionTypeID.EditValue = 3;
                        }

                        else if (((TreeList)sender).FocusedNode.GetValue(0).ToString().Split('-')[1].Trim() == (UserInfo.Language == iLanguage.Arabic ? "البرنتـــاج 1" : "Albernage1"))
                        {
                            ((frmManufacturingPrentag)frm).cmbPrntageTypeID.EditValue = 1;
                        }
                        else if (((TreeList)sender).FocusedNode.GetValue(0).ToString().Split('-')[1].Trim() == (UserInfo.Language == iLanguage.Arabic ? "البرنتـــاج 2" : "Albernage2"))
                        {
                            ((frmManufacturingPrentag)frm).cmbPrntageTypeID.EditValue = 2;
                        }


                        else if (((TreeList)sender).FocusedNode.GetValue(0).ToString().Split('-')[1].Trim() == (UserInfo.Language == iLanguage.Arabic ? "كشف حساب عملاء" : "Customer account statement"))
                        {


                            ((frmCustomersAccountStatement)frm).chkCustomer.Checked = true;
                        }

                        else if (((TreeList)sender).FocusedNode.GetValue(0).ToString().Split('-')[1].Trim() == (UserInfo.Language == iLanguage.Arabic ? "كشف حساب موردين" : "Suppliers account statement"))
                        {
                            ((frmCustomersAccountStatement)frm).chkSupliar.Checked = true;
                        }
                    }

                    //if (USERPERMATIONS.GET_FORMPERMATION(Form) == false)
                    //    Form.Dispose();
                    //else
                    string linkText = ((TreeList)sender).FocusedNode.GetValue(0).ToString();
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form.Text == linkText)
                        {
                            form.Activate();
                            return;
                        }
                    }

                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        this.Dispose();
                        this.Close();
                        frm.Show();
                        //frm.Text = treeList2.FocusedNode.GetValue(0).ToString();
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSerchInTree_Validated(null, null);
        }

        private void txtSerchInTree_EditValueChanged(object sender, EventArgs e)
        {
            GeSubAcountsTree(0);
        }
    }
}

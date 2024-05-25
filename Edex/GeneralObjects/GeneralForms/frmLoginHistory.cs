using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmLoginHistory : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string strSQL;
        DataTable dt;
        public string FocusedControl;
        public frmLoginHistory()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            this.txtUserID.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserID_Validating);
            this.txtBranchID.Validating += new System.ComponentModel.CancelEventHandler(this.txtBranchID_Validating);
            ///////////////////////////////////////////////////////
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // this.txtFromDate.EditValue = DateTime.Now;
            /////////////////////////////////////////////////////////////////
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            // this.txtToDate.EditValue = DateTime.Now;
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.OptionsView.EnableAppearanceOddRow = true;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;
              if (UserInfo.Language == iLanguage.English)
            {

                dgvColTime.Caption = "Voucher ID ";
                dgvColStatus.Caption = "Voucher Date";
                dgvColUserID.Caption = "Amount";
                dgvColDB.Caption = "Declartion  ";
                dgvColBranch.Caption = "# ";
                dgvColPassword.Caption = "Doc NO";
                   dgvColPC.Caption = "Record Type";


                   dgvColUserName.Caption = "User";








               
                btnShow.Text = "show";
                //  Label8.Text = btnShow.Tag.ToString();

            }

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }
        private void txtUserID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (txtUserID.Text != string.Empty)
                {
                    string strSQL;
                    strSQL = "SELECT ArbName FROM  Users WHERE UserID = " + txtUserID.Text + " AND Cancel=0 ";
                    CSearch.ControlValidating(txtUserID, lblUserName, strSQL);

                }




            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtBranchID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (txtBranchID.Text != string.Empty)
                {
                    string strSQL;
                    strSQL = "SELECT ArbName FROM  Branches WHERE BranchID = " + txtBranchID.Text + " AND Cancel=0 ";
                    CSearch.ControlValidating(txtBranchID, lblBranchName, strSQL);

                }




            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }

            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }

            return c.Name;
        }

        public void Find()
        {
            try{
            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl.Trim() == txtUserID.Name)
            {
                cls.SQLStr = "SELECT  UserID as الرقم, ArbName as [اسم المستخدم] FROM  Users"
             + " WHERE Cancel =0  ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT  UserID as ID, ArbName as [user Name] FROM  Users"
                + " WHERE Cancel =0  ";


            }

            if (FocusedControl.Trim() == txtBranchID.Name)
            {

                cls.SQLStr = "SELECT  BranchID as الرقم, ArbName as [اسم الفرع] FROM  Branches"
          + " WHERE Cancel =0  ";

                if (UserInfo.Language == iLanguage.English)
                    cls.SQLStr = "SELECT  BranchID as ID, ArbName as [Branch Name] FROM  Branches"
                + " WHERE Cancel =0  ";



            }


            ColumnWidth = new int[] { 80, 200 };



            if (cls.SQLStr != "")
            {
                frmSearch frm = new frmSearch();

                cls.strFilter = "الرقم";
                if (UserInfo.Language == iLanguage.English)
                    cls.strFilter = "ID";

                frm.AddSearchData(cls);
                frm.ColumnWidth = ColumnWidth;
                frm.ShowDialog();
                GetSelectedSearchValue(cls);
            }
            }
            catch { }
        }

        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl.Trim() == txtUserID.Name)
                {

                    txtUserID.Text = cls.PrimaryKeyValue;
                    txtUserID_Validating(null, null);

                }
                else
                {
                    txtBranchID.Text = cls.PrimaryKeyValue;
                    txtBranchID_Validating(null, null);
                }


            }

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try{
            gridControl1.DataSource = null;
            DataTable dtCloned;
            string strSQL;
            strSQL = "SELECT dbo.LoginHistory.RegDate, dbo.LoginHistory.RegTime, dbo.LoginStatus.ArbName AS Status, dbo.LoginHistory.UserID, dbo.Users.ArbName AS UserName, "
            + " dbo.LoginHistory.DBName, dbo.Branches.ArbName AS BranchName, dbo.LoginHistory.Password, dbo.LoginHistory.ComputerInfo FROM dbo.LoginHistory LEFT OUTER JOIN"
            + " dbo.Users ON dbo.LoginHistory.UserID = dbo.Users.UserID LEFT OUTER JOIN dbo.LoginStatus ON dbo.LoginHistory.Status = dbo.LoginStatus.ID LEFT OUTER JOIN"
            + " dbo.Branches ON dbo.LoginHistory.BranchID = dbo.Branches.BranchID WHERE (1 = 1)";

            if (txtFromDate.Text != string.Empty)
                strSQL = strSQL + " AND LoginHistory.RegDate >=  " + Comon.ConvertDateToSerial(txtFromDate.Text).ToString();

            if (txtToDate.Text != string.Empty)
                strSQL = strSQL + "AND LoginHistory.RegDate <=  " + Comon.ConvertDateToSerial(txtToDate.Text).ToString();


            if (txtUserID.Text != string.Empty)
                strSQL = strSQL + " AND LoginHistory.UserID =  " + txtUserID.Text;

            if (txtBranchID.Text != string.Empty)
                strSQL = strSQL + " AND LoginHistory.BranchID =  " + txtBranchID.Text;

            if (txtPC.Text != string.Empty)
                strSQL = strSQL + " AND LoginHistory.ComputerInfo Like '%" + txtPC.Text + "%' ";

            if (txtDB.Text != string.Empty)
                strSQL = strSQL + " AND LoginHistory.DBName Like '%" + txtDB.Text + "%' ";


            if (Comon.cInt(cmbStatus.EditValue) != 3)
                strSQL = strSQL + " AND LoginHistory.Status = " + cmbStatus.EditValue.ToString();

            strSQL = strSQL + " ORDER BY LoginHistory.ID";

            dtCloned = Lip.SelectRecord(strSQL);
            // dtCloned = dt.Clone()
            if (dtCloned.Rows.Count > 0)
                gridControl1.DataSource = dtCloned;

            }
            catch { }
        }

        private void frmLoginHistory_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3)
                Find();

        }

        private void frmLoginHistory_KeyPress(object sender, KeyPressEventArgs e)
        {



        }

        private void frmLoginHistory_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3)
                Find();

        }

        private void txtUserID_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3)
                Find();


        }

        private void txtBranchID_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F3)
                Find();


        }

        private void frmLoginHistory_Load(object sender, EventArgs e)
        {
            try{
            strSQL = " SELECT * FROM LoginStatus";
            dt = Lip.SelectRecord(strSQL);
            dt.Rows.Add("3", "...", "...");
            cmbStatus.Properties.DataSource = dt;
            cmbStatus.Properties.DisplayMember = (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName");
            cmbStatus.Properties.ValueMember = "ID";
            cmbStatus.EditValue = 3;
            }
            catch { }
        }

        private void txtDB_EditValueChanged(object sender, EventArgs e)
        {

        }

    }
}

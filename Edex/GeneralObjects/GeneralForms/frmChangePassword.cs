using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
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
    public partial class frmChangePassword : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string loc;
        public string strSQL;
        DataTable dt = new DataTable();
        public frmChangePassword()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            this.txtConfirmPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtConfirmPassword_Validating);

        }

        private void txtOldPassword_EditValueChanged(object sender, EventArgs e)
        {


        }
        protected override void DoSave()
        {
            try
            {
                //if (1 == 1)
                //{
                //    bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, Messages.msgConfirmUpdate);
                //    if (!Yes)
                //        return;
                //}

                string hashedPassword = Security.HashSHA1(txtOldPassword.Text.ToString());

                strSQL = "SELECT ArbName FROM dbo.Users WHERE (Cancel = 0)  AND (UserID = " + UserInfo.ID + ") AND (Password ='" + hashedPassword + "')";
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count < 1)
                {
                    Messages.MsgInfo(Messages.TitleInfo, "كلمة المرور القديمة غير صحيحة" );
                    txtOldPassword.Focus();
                }
                else
                {
                    string NewhashedPassword = Security.HashSHA1(txtNewPassword.Text.ToString());

                    strSQL = "UPDATE Users SET Password = '" + NewhashedPassword + "' WHERE (UserID = " + UserInfo.ID + ")  "
                      + " AND (Password = '" + hashedPassword + "')  AND (Cancel = 0)";
                    Lip.ExecututeSQL(strSQL);
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                    txtOldPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtConfirmPassword.Text = "";

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtConfirmPassword.Text) && txtConfirmPassword.Text != txtNewPassword.Text)
                {
                    Messages.MsgInfo(Messages.TitleInfo, (UserInfo.Language == iLanguage.Arabic ? "كلمة المرور التي ادخلتها غير متطابقة" : "Not match Password"));
                    txtNewPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    txtNewPassword.Focus();

                }
            }
            else {
                txtNewPassword.Focus();
            
            }
        }

        private void txtConfirmPassword_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            txtOldPassword.Focus();
        }

    }
}

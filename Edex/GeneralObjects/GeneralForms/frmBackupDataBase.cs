using Edex.DAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmBackupDataBase : Edex.GeneralObjects.GeneralForms.BaseForm
    {
        public string strSQL;
        public frmBackupDataBase()
        {
            try
            {
                InitializeComponent();
                ribbonControl1.Visible = false;
                txtBackupName.Text = "Omex" + Lip.GetServerDateSerial()+"_"+ Lip.GetServerTimeSerial();
                strSQL = "Select BackupPath From GeneralSettings";
                string ss = Lip.GetValue(strSQL);
                if (ss != "")
                    txtBackupPath.Text = ss;
                txtBackupPath.Text = MySession.defaultBackupPath;
                progressBarControl1.Properties.PercentView = true;
                progressBarControl1.Properties.Step = 5;
                progressBarControl1.Properties.Maximum = 100;
                progressBarControl1.Properties.Minimum = 0;
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
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBarControl1.Position == 100)
            {
                timer1.Enabled = false;
                progressBarControl1.Visible = false;
                Messages.MsgInfo(Messages.TitleInfo, (UserInfo.Language == iLanguage.Arabic ? "تم النسخ بنجاح" : "Successfully Done"));

            }
            else
                // progressBarControl1.Properties.Step  = progressBarControl1.Properties.Step + 5;
                progressBarControl1.PerformStep();
            progressBarControl1.Update();
        }

        private void frmBackupDataBase_Load(object sender, EventArgs e)
        {
            
        }
        public void DoClick() {

            btnBackup_Click(null, null);
        
        
        
        }

        public void BacKUp()
        {
             if(cConnectionString.ServerName==".")
            if (Directory.Exists(txtBackupPath.Text))
            {
                strSQL = "BACKUP DATABASE " + cConnectionString.DataBasename + " TO DISK ='" + txtBackupPath.Text + "\\" + txtBackupName.Text + ".bak'";
              

                textEdit1.Text = strSQL;
                timer1.Enabled = true;
                progressBarControl1.Visible = true;
                Lip.ExecututeSQL(strSQL);
            }
            else
            {
                Messages.MsgWarning(Messages.TitleError, (UserInfo.Language == iLanguage.Arabic ? "الرجاء اختيار مسار صحيح للنسخ الاحتياطي" : "select path Backup"));
                timer1.Enabled = false;
                return;
            }

        }
        public void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {


                if (txtBackupName.Text == "")
                    //MsgBoxInformation(IIf(Language = iLanguage.Arabic, "ادخل اسم النسخة الإحتياطية", "Type Backup Name"))
                    Messages.MsgInfo(Messages.TitleInfo, (UserInfo.Language == iLanguage.Arabic ? "ادخل اسم النسخة الإحتياطية" : "Type Backup Name"));
               
                else
                {
                    progressBarControl1.EditValue = 0;
                  
                    BacKUp();
                }
                
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
         
    
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            SaveFileDialog1.FileName = txtBackupName.Text;
            DialogResult result = SaveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                txtBackupPath.Text = SaveFileDialog1.FileName;

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

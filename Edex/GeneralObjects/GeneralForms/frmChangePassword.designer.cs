namespace Edex.GeneralObjects.GeneralForms
{
    partial class frmChangePassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtOldPassword = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNewPassword = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.pnlUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserCreatedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserUpdatedD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserCreatedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComputerInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserUpdatedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompoterEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateCreated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateUpdated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(581, 116);
            // 
            // pnlUsers
            // 
            this.pnlUsers.Location = new System.Drawing.Point(0, 178);
            this.pnlUsers.Size = new System.Drawing.Size(581, 51);
            this.pnlUsers.Visible = false;
            // 
            // lblUserCreatedID
            // 
            this.lblUserCreatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserCreatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lbfUserUpdatedD
            // 
            this.lbfUserUpdatedD.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbfUserUpdatedD.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lbfUserCreatedID
            // 
            this.lbfUserCreatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbfUserCreatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblComputerInfo
            // 
            this.lblComputerInfo.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblComputerInfo.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserUpdatedID
            // 
            this.lblUserUpdatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserUpdatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblCompoterEdit
            // 
            this.lblCompoterEdit.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCompoterEdit.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserDateCreated
            // 
            this.lblUserDateCreated.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserDateCreated.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserDateUpdated
            // 
            this.lblUserDateUpdated.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserDateUpdated.Properties.Appearance.Options.UseBackColor = true;
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 229);
            this.ribbonStatusBar1.Size = new System.Drawing.Size(581, 27);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(12, 132);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(120, 14);
            this.lblPassword.TabIndex = 373;
            this.lblPassword.Tag = "Old password";
            this.lblPassword.Text = "كلـمــة الـمــرور القديمة";
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.EnterMoveNextControl = true;
            this.txtOldPassword.Location = new System.Drawing.Point(171, 126);
            this.txtOldPassword.MenuManager = this.ribbonControl1;
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtOldPassword.Properties.Mask.EditMask = "f0";
            this.txtOldPassword.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtOldPassword.Size = new System.Drawing.Size(167, 20);
            this.txtOldPassword.TabIndex = 372;
            this.txtOldPassword.Tag = "";
            this.txtOldPassword.EditValueChanged += new System.EventHandler(this.txtOldPassword_EditValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 14);
            this.label1.TabIndex = 375;
            this.label1.Tag = " New password";
            this.label1.Text = "كلـمــة الـمــرور الجديدة";
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.EnterMoveNextControl = true;
            this.txtNewPassword.Location = new System.Drawing.Point(171, 152);
            this.txtNewPassword.MenuManager = this.ribbonControl1;
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtNewPassword.Properties.Mask.EditMask = "f0";
            this.txtNewPassword.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtNewPassword.Size = new System.Drawing.Size(167, 20);
            this.txtNewPassword.TabIndex = 374;
            this.txtNewPassword.Tag = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 14);
            this.label2.TabIndex = 377;
            this.label2.Tag = "Confirm New password";
            this.label2.Text = "تأكيد كلـمــة الـمــرور الجديدة";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.EnterMoveNextControl = true;
            this.txtConfirmPassword.Location = new System.Drawing.Point(171, 178);
            this.txtConfirmPassword.MenuManager = this.ribbonControl1;
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtConfirmPassword.Properties.Mask.EditMask = "f0";
            this.txtConfirmPassword.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtConfirmPassword.Size = new System.Drawing.Size(167, 20);
            this.txtConfirmPassword.TabIndex = 376;
            this.txtConfirmPassword.Tag = "";
            this.txtConfirmPassword.EditValueChanged += new System.EventHandler(this.txtConfirmPassword_EditValueChanged);
            this.txtConfirmPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtConfirmPassword_Validating);
            // 
            // frmChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(581, 256);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtOldPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChangePassword";
            this.Tag = "Form Change Password";
            this.Text = "شاشة تغيير كلمة المرور للمستخدم";
            this.Load += new System.EventHandler(this.frmChangePassword_Load);
            this.Controls.SetChildIndex(this.ribbonStatusBar1, 0);
            this.Controls.SetChildIndex(this.pnlUsers, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.txtOldPassword, 0);
            this.Controls.SetChildIndex(this.lblPassword, 0);
            this.Controls.SetChildIndex(this.txtNewPassword, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtConfirmPassword, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.pnlUsers.ResumeLayout(false);
            this.pnlUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserCreatedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserUpdatedD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserCreatedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComputerInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserUpdatedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompoterEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateCreated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateUpdated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOldPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmPassword.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblPassword;
        private DevExpress.XtraEditors.TextEdit txtOldPassword;
        internal System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtNewPassword;
        internal System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtConfirmPassword;
    }
}

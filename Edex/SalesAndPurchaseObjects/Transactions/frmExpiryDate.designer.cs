namespace Edex.SalesAndPurchaseObjects.Transactions
{
    partial class frmExpiryDate
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
            this.txtExpiryDate = new DevExpress.XtraEditors.DateEdit();
            this.btnShow = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpiryDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpiryDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtExpiryDate
            // 
            this.txtExpiryDate.EditValue = null;
            this.txtExpiryDate.EnterMoveNextControl = true;
            this.txtExpiryDate.Location = new System.Drawing.Point(24, 37);
            this.txtExpiryDate.Name = "txtExpiryDate";
            this.txtExpiryDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtExpiryDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtExpiryDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtExpiryDate.Properties.DisplayFormat.FormatString = "";
            this.txtExpiryDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtExpiryDate.Properties.EditFormat.FormatString = "";
            this.txtExpiryDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtExpiryDate.Properties.Mask.BeepOnError = true;
            this.txtExpiryDate.Size = new System.Drawing.Size(155, 20);
            this.txtExpiryDate.TabIndex = 2;
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(58, 72);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(85, 29);
            this.btnShow.TabIndex = 4;
            this.btnShow.Text = "مـــــــــوافـــــق";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // frmExpiryDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 122);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.txtExpiryDate);
            this.Name = "frmExpiryDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "تاريخ";
            ((System.ComponentModel.ISupportInitialize)(this.txtExpiryDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpiryDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtExpiryDate;
        private DevExpress.XtraEditors.SimpleButton btnShow;
    }
}
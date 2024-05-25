namespace Edex.GeneralObjects.GeneralForms
{
    partial class frmViewImage
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
            this.picInvoiceImage = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.picInvoiceImage.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // picInvoiceImage
            // 
            this.picInvoiceImage.Location = new System.Drawing.Point(1, 2);
            this.picInvoiceImage.Name = "picInvoiceImage";
            this.picInvoiceImage.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picInvoiceImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picInvoiceImage.Size = new System.Drawing.Size(414, 331);
            this.picInvoiceImage.TabIndex = 772;
            // 
            // frmViewImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 336);
            this.Controls.Add(this.picInvoiceImage);
            this.Name = "frmViewImage";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "Show Image";
            this.Text = "عارض الصور";
            ((System.ComponentModel.ISupportInitialize)(this.picInvoiceImage.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.PictureEdit picInvoiceImage;

    }
}
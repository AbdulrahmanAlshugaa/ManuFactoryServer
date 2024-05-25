namespace Edex.SalesAndPurchaseObjects.Transactions
{
    partial class frmRemindQtyItemFromCahier
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
            this.btnShow = new DevExpress.XtraEditors.SimpleButton();
            this.txtArbName = new DevExpress.XtraEditors.TextEdit();
            this.lblArbName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBarCode = new DevExpress.XtraEditors.TextEdit();
            this.Label3 = new System.Windows.Forms.Label();
            this.cmbUnits = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSalePrice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtArbName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnits.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(12, 98);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(98, 48);
            this.btnShow.TabIndex = 2;
            this.btnShow.Text = "مـــــــــوافـــــق";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtArbName
            // 
            this.txtArbName.EnterMoveNextControl = true;
            this.txtArbName.Location = new System.Drawing.Point(12, 12);
            this.txtArbName.Name = "txtArbName";
            this.txtArbName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArbName.Properties.Appearance.Options.UseFont = true;
            this.txtArbName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtArbName.Size = new System.Drawing.Size(570, 32);
            this.txtArbName.TabIndex = 0;
            this.txtArbName.Tag = "ImportantField";
            // 
            // lblArbName
            // 
            this.lblArbName.AutoSize = true;
            this.lblArbName.BackColor = System.Drawing.Color.Transparent;
            this.lblArbName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArbName.Location = new System.Drawing.Point(621, 15);
            this.lblArbName.Name = "lblArbName";
            this.lblArbName.Size = new System.Drawing.Size(64, 14);
            this.lblArbName.TabIndex = 220;
            this.lblArbName.Tag = "Arabic Name";
            this.lblArbName.Text = "اسم الصنف";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(646, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 14);
            this.label1.TabIndex = 221;
            this.label1.Tag = "Arabic Name";
            this.label1.Text = "الوحدة";
            // 
            // txtBarCode
            // 
            this.txtBarCode.EnterMoveNextControl = true;
            this.txtBarCode.Location = new System.Drawing.Point(427, 75);
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtBarCode.Properties.Mask.EditMask = "f0";
            this.txtBarCode.Size = new System.Drawing.Size(155, 20);
            this.txtBarCode.TabIndex = 223;
            this.txtBarCode.Tag = "";
            this.txtBarCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtBarCode_Validating);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(604, 78);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(81, 14);
            this.Label3.TabIndex = 222;
            this.Label3.Tag = "New Barcode";
            this.Label3.Text = "الباركـــــــــــــود";
            // 
            // cmbUnits
            // 
            this.cmbUnits.EnterMoveNextControl = true;
            this.cmbUnits.Location = new System.Drawing.Point(427, 49);
            this.cmbUnits.Name = "cmbUnits";
            this.cmbUnits.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbUnits.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUnits.Properties.NullText = "";
            this.cmbUnits.Properties.PopupSizeable = false;
            this.cmbUnits.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbUnits.Size = new System.Drawing.Size(155, 20);
            this.cmbUnits.TabIndex = 3;
            this.cmbUnits.Tag = "ImportantField";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(588, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 19);
            this.label2.TabIndex = 226;
            this.label2.Tag = "New Barcode";
            this.label2.Text = "الكمية المتوفرة";
            // 
            // txtSalePrice
            // 
            this.txtSalePrice.BackColor = System.Drawing.SystemColors.ControlText;
            this.txtSalePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSalePrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSalePrice.ForeColor = System.Drawing.Color.Chartreuse;
            this.txtSalePrice.Location = new System.Drawing.Point(427, 98);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.Size = new System.Drawing.Size(155, 48);
            this.txtSalePrice.TabIndex = 846;
            this.txtSalePrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmRemindQtyItemFromCahier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 156);
            this.Controls.Add(this.txtSalePrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbUnits);
            this.Controls.Add(this.txtBarCode);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtArbName);
            this.Controls.Add(this.lblArbName);
            this.Controls.Add(this.btnShow);
            this.KeyPreview = true;
            this.Name = "frmRemindQtyItemFromCahier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "الكمية المتوفره من صنف";
            this.Load += new System.EventHandler(this.frmAddNewItemFromCahier_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRemindQtyItemFromCahier_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtArbName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnits.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnShow;
        internal System.Windows.Forms.Label lblArbName;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.TextEdit txtBarCode;
        public DevExpress.XtraEditors.TextEdit txtArbName;
        public DevExpress.XtraEditors.LookUpEdit cmbUnits;
        internal System.Windows.Forms.Label txtSalePrice;
    }
}
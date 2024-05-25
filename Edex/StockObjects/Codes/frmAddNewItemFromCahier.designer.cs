namespace Edex.SalesAndPurchaseObjects.Transactions
{
    partial class frmAddNewItemFromCahier
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
            this.txtSalePrice = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtArbName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnits.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalePrice.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(12, 81);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(98, 29);
            this.btnShow.TabIndex = 2;
            this.btnShow.Text = "مـــــــــوافـــــق";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtArbName
            // 
            this.txtArbName.EnterMoveNextControl = true;
            this.txtArbName.Location = new System.Drawing.Point(12, 12);
            this.txtArbName.Name = "txtArbName";
            this.txtArbName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtArbName.Size = new System.Drawing.Size(348, 20);
            this.txtArbName.TabIndex = 0;
            this.txtArbName.Tag = "ImportantField";
            // 
            // lblArbName
            // 
            this.lblArbName.AutoSize = true;
            this.lblArbName.BackColor = System.Drawing.Color.Transparent;
            this.lblArbName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArbName.Location = new System.Drawing.Point(366, 12);
            this.lblArbName.Name = "lblArbName";
            this.lblArbName.Size = new System.Drawing.Size(101, 14);
            this.lblArbName.TabIndex = 220;
            this.lblArbName.Tag = "Arabic Name";
            this.lblArbName.Text = "الاســـــم بالعـــربي";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(428, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 14);
            this.label1.TabIndex = 221;
            this.label1.Tag = "Arabic Name";
            this.label1.Text = "الوحدة";
            // 
            // txtBarCode
            // 
            this.txtBarCode.EnterMoveNextControl = true;
            this.txtBarCode.Location = new System.Drawing.Point(205, 64);
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtBarCode.Properties.Mask.EditMask = "f0";
            this.txtBarCode.Size = new System.Drawing.Size(155, 20);
            this.txtBarCode.TabIndex = 223;
            this.txtBarCode.Tag = "";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(386, 67);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(81, 14);
            this.Label3.TabIndex = 222;
            this.Label3.Tag = "New Barcode";
            this.Label3.Text = "الباركـــــــــــــود";
            // 
            // cmbUnits
            // 
            this.cmbUnits.EnterMoveNextControl = true;
            this.cmbUnits.Location = new System.Drawing.Point(205, 38);
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
            // txtSalePrice
            // 
            this.txtSalePrice.EnterMoveNextControl = true;
            this.txtSalePrice.Location = new System.Drawing.Point(205, 89);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSalePrice.Properties.Mask.EditMask = "f0";
            this.txtSalePrice.Size = new System.Drawing.Size(155, 20);
            this.txtSalePrice.TabIndex = 1;
            this.txtSalePrice.Tag = "";
            this.txtSalePrice.Validating += new System.ComponentModel.CancelEventHandler(this.txtSalePrice_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(412, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 226;
            this.label2.Tag = "New Barcode";
            this.label2.Text = "سعر البيع";
            // 
            // frmAddNewItemFromCahier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 118);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSalePrice);
            this.Controls.Add(this.cmbUnits);
            this.Controls.Add(this.txtBarCode);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtArbName);
            this.Controls.Add(this.lblArbName);
            this.Controls.Add(this.btnShow);
            this.Name = "frmAddNewItemFromCahier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "اضافة صنف جديد";
            this.Load += new System.EventHandler(this.frmAddNewItemFromCahier_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtArbName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnits.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalePrice.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnShow;
        private DevExpress.XtraEditors.TextEdit txtArbName;
        internal System.Windows.Forms.Label lblArbName;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label Label3;
        private DevExpress.XtraEditors.TextEdit txtSalePrice;
        internal System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.TextEdit txtBarCode;
        public DevExpress.XtraEditors.LookUpEdit cmbUnits;
    }
}
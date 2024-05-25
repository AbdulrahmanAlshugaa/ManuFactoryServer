namespace Edex.SalesAndPurchaseObjects.Transactions
{
    partial class frmEditCloseCashierDate
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
            this.txtCloseCashierDate = new DevExpress.XtraEditors.DateEdit();
            this.txtFromInvoiceNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtToInvoiceNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseCashierDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseCashierDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromInvoiceNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToInvoiceNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(476, 116);
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(309, 162);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(85, 29);
            this.btnShow.TabIndex = 842;
            this.btnShow.Text = "تعديــــل";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtCloseCashierDate
            // 
            this.txtCloseCashierDate.EditValue = null;
            this.txtCloseCashierDate.EnterMoveNextControl = true;
            this.txtCloseCashierDate.Location = new System.Drawing.Point(115, 160);
            this.txtCloseCashierDate.Name = "txtCloseCashierDate";
            this.txtCloseCashierDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtCloseCashierDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtCloseCashierDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtCloseCashierDate.Properties.DisplayFormat.FormatString = "";
            this.txtCloseCashierDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtCloseCashierDate.Properties.EditFormat.FormatString = "";
            this.txtCloseCashierDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtCloseCashierDate.Properties.Mask.BeepOnError = true;
            this.txtCloseCashierDate.Size = new System.Drawing.Size(137, 20);
            this.txtCloseCashierDate.TabIndex = 841;
            // 
            // txtFromInvoiceNo
            // 
            this.txtFromInvoiceNo.EnterMoveNextControl = true;
            this.txtFromInvoiceNo.Location = new System.Drawing.Point(98, 121);
            this.txtFromInvoiceNo.Name = "txtFromInvoiceNo";
            this.txtFromInvoiceNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFromInvoiceNo.Size = new System.Drawing.Size(92, 20);
            this.txtFromInvoiceNo.TabIndex = 838;
            this.txtFromInvoiceNo.Tag = "ImportantFieldGreaterThanZero";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(255, 124);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 14);
            this.labelControl1.TabIndex = 844;
            this.labelControl1.Tag = "Debit Account";
            this.labelControl1.Text = "الى فاتورة رقم";
            // 
            // txtToInvoiceNo
            // 
            this.txtToInvoiceNo.EnterMoveNextControl = true;
            this.txtToInvoiceNo.Location = new System.Drawing.Point(341, 121);
            this.txtToInvoiceNo.Name = "txtToInvoiceNo";
            this.txtToInvoiceNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtToInvoiceNo.Size = new System.Drawing.Size(92, 20);
            this.txtToInvoiceNo.TabIndex = 843;
            this.txtToInvoiceNo.Tag = "ImportantFieldGreaterThanZero";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(12, 162);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(97, 14);
            this.labelControl2.TabIndex = 846;
            this.labelControl2.Tag = "Debit Account";
            this.labelControl2.Text = "تاريخ الإغلاق الجديد";
            // 
            // labelControl20
            // 
            this.labelControl20.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl20.Appearance.Options.UseFont = true;
            this.labelControl20.Location = new System.Drawing.Point(12, 124);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(67, 14);
            this.labelControl20.TabIndex = 839;
            this.labelControl20.Tag = "Debit Account";
            this.labelControl20.Text = "من فاتورة رقم";
            // 
            // s
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(476, 254);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtToInvoiceNo);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.txtCloseCashierDate);
            this.Controls.Add(this.labelControl20);
            this.Controls.Add(this.txtFromInvoiceNo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "s";
            this.Controls.SetChildIndex(this.txtFromInvoiceNo, 0);
            this.Controls.SetChildIndex(this.labelControl20, 0);
            this.Controls.SetChildIndex(this.txtCloseCashierDate, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.txtToInvoiceNo, 0);
            this.Controls.SetChildIndex(this.labelControl1, 0);
            this.Controls.SetChildIndex(this.labelControl2, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseCashierDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseCashierDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromInvoiceNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToInvoiceNo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnShow;
        private DevExpress.XtraEditors.DateEdit txtCloseCashierDate;
        private DevExpress.XtraEditors.TextEdit txtFromInvoiceNo;
        private DevExpress.XtraEditors.TextEdit txtToInvoiceNo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl20;


    }
}

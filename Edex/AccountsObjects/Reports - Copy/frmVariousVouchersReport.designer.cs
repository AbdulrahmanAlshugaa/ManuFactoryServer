namespace Edex.AccountsObjects.Reports
{
    partial class frmVariousVouchersReport
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
            this.txtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.txtToDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.txtFromVoucherNo = new DevExpress.XtraEditors.TextEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.GridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvColnSN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColVoucherID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColVoucherDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDeclaration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDocNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColRecordType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTempRecordType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColRegTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtToVoucherNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.cmbBranchesID = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromVoucherNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToVoucherNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBranchesID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(973, 116);
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(732, 136);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(85, 39);
            this.btnShow.TabIndex = 842;
            this.btnShow.Text = "عــــــــرض";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(360, 122);
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFromDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtFromDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtFromDate.Properties.DisplayFormat.FormatString = "";
            this.txtFromDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.EditFormat.FormatString = "";
            this.txtFromDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtFromDate.Properties.Mask.BeepOnError = true;
            this.txtFromDate.Size = new System.Drawing.Size(112, 20);
            this.txtFromDate.TabIndex = 840;
            this.txtFromDate.EditValueChanged += new System.EventHandler(this.txtFromDate_EditValueChanged);
            // 
            // txtToDate
            // 
            this.txtToDate.EditValue = null;
            this.txtToDate.EnterMoveNextControl = true;
            this.txtToDate.Location = new System.Drawing.Point(360, 160);
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtToDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtToDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtToDate.Properties.DisplayFormat.FormatString = "";
            this.txtToDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.EditFormat.FormatString = "";
            this.txtToDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtToDate.Properties.Mask.BeepOnError = true;
            this.txtToDate.Size = new System.Drawing.Size(112, 20);
            this.txtToDate.TabIndex = 841;
            this.txtToDate.EditValueChanged += new System.EventHandler(this.txtToDate_EditValueChanged);
            // 
            // labelControl20
            // 
            this.labelControl20.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl20.Appearance.Options.UseFont = true;
            this.labelControl20.Location = new System.Drawing.Point(12, 124);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(74, 14);
            this.labelControl20.TabIndex = 839;
            this.labelControl20.Tag = "Debit Account";
            this.labelControl20.Text = "من مستند رقم";
            // 
            // txtFromVoucherNo
            // 
            this.txtFromVoucherNo.EnterMoveNextControl = true;
            this.txtFromVoucherNo.Location = new System.Drawing.Point(98, 121);
            this.txtFromVoucherNo.Name = "txtFromVoucherNo";
            this.txtFromVoucherNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFromVoucherNo.Size = new System.Drawing.Size(92, 20);
            this.txtFromVoucherNo.TabIndex = 838;
            this.txtFromVoucherNo.Tag = "ImportantFieldGreaterThanZero";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(0, 212);
            this.gridControl1.MainView = this.GridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(973, 259);
            this.gridControl1.TabIndex = 837;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // GridView1
            // 
            this.GridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dgvColnSN,
            this.dgvColVoucherID,
            this.dgvColVoucherDate,
            this.dgvColAmount,
            this.dgvColDeclaration,
            this.dgvColDocNo,
            this.dgvColRecordType,
            this.dgvColTempRecordType,
            this.dgvColRegTime,
            this.dgvColUserName});
            this.GridView1.GridControl = this.gridControl1;
            this.GridView1.Name = "GridView1";
            this.GridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.GridView1.DoubleClick += new System.EventHandler(this.GridView1_DoubleClick);
            // 
            // dgvColnSN
            // 
            this.dgvColnSN.Caption = "م";
            this.dgvColnSN.FieldName = "SN";
            this.dgvColnSN.Name = "dgvColnSN";
            this.dgvColnSN.Visible = true;
            this.dgvColnSN.VisibleIndex = 0;
            this.dgvColnSN.Width = 43;
            // 
            // dgvColVoucherID
            // 
            this.dgvColVoucherID.Caption = "رقم السند";
            this.dgvColVoucherID.FieldName = "VoucherID";
            this.dgvColVoucherID.Name = "dgvColVoucherID";
            this.dgvColVoucherID.Visible = true;
            this.dgvColVoucherID.VisibleIndex = 1;
            this.dgvColVoucherID.Width = 120;
            // 
            // dgvColVoucherDate
            // 
            this.dgvColVoucherDate.Caption = "التاريخ";
            this.dgvColVoucherDate.FieldName = "VoucherDate";
            this.dgvColVoucherDate.Name = "dgvColVoucherDate";
            this.dgvColVoucherDate.Visible = true;
            this.dgvColVoucherDate.VisibleIndex = 2;
            this.dgvColVoucherDate.Width = 108;
            // 
            // dgvColAmount
            // 
            this.dgvColAmount.Caption = "المبلغ";
            this.dgvColAmount.FieldName = "Amount";
            this.dgvColAmount.Name = "dgvColAmount";
            this.dgvColAmount.Visible = true;
            this.dgvColAmount.VisibleIndex = 3;
            this.dgvColAmount.Width = 108;
            // 
            // dgvColDeclaration
            // 
            this.dgvColDeclaration.Caption = "البيان";
            this.dgvColDeclaration.FieldName = "Declaration";
            this.dgvColDeclaration.Name = "dgvColDeclaration";
            this.dgvColDeclaration.Visible = true;
            this.dgvColDeclaration.VisibleIndex = 4;
            this.dgvColDeclaration.Width = 257;
            // 
            // dgvColDocNo
            // 
            this.dgvColDocNo.Caption = "رقم المستند";
            this.dgvColDocNo.FieldName = "DocNo";
            this.dgvColDocNo.Name = "dgvColDocNo";
            this.dgvColDocNo.Visible = true;
            this.dgvColDocNo.VisibleIndex = 5;
            this.dgvColDocNo.Width = 121;
            // 
            // dgvColRecordType
            // 
            this.dgvColRecordType.Caption = "نوع المستند";
            this.dgvColRecordType.FieldName = "RecordType";
            this.dgvColRecordType.Name = "dgvColRecordType";
            this.dgvColRecordType.Width = 91;
            // 
            // dgvColTempRecordType
            // 
            this.dgvColTempRecordType.Caption = "النوع ";
            this.dgvColTempRecordType.FieldName = "TempRecordType";
            this.dgvColTempRecordType.Name = "dgvColTempRecordType";
            this.dgvColTempRecordType.Width = 103;
            // 
            // dgvColRegTime
            // 
            this.dgvColRegTime.Caption = "الوقت";
            this.dgvColRegTime.FieldName = "RegTime";
            this.dgvColRegTime.Name = "dgvColRegTime";
            this.dgvColRegTime.Width = 136;
            // 
            // dgvColUserName
            // 
            this.dgvColUserName.Caption = "المستخدم";
            this.dgvColUserName.FieldName = "UserName";
            this.dgvColUserName.Name = "dgvColUserName";
            this.dgvColUserName.Visible = true;
            this.dgvColUserName.VisibleIndex = 6;
            this.dgvColUserName.Width = 198;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 161);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 14);
            this.labelControl1.TabIndex = 844;
            this.labelControl1.Tag = "Debit Account";
            this.labelControl1.Text = "الى مستند رقم";
            // 
            // txtToVoucherNo
            // 
            this.txtToVoucherNo.EnterMoveNextControl = true;
            this.txtToVoucherNo.Location = new System.Drawing.Point(98, 158);
            this.txtToVoucherNo.Name = "txtToVoucherNo";
            this.txtToVoucherNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtToVoucherNo.Size = new System.Drawing.Size(92, 20);
            this.txtToVoucherNo.TabIndex = 843;
            this.txtToVoucherNo.Tag = "ImportantFieldGreaterThanZero";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(284, 161);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(59, 14);
            this.labelControl2.TabIndex = 846;
            this.labelControl2.Tag = "Debit Account";
            this.labelControl2.Text = "الى تـــــاريخ";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(284, 124);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(59, 14);
            this.labelControl3.TabIndex = 845;
            this.labelControl3.Tag = "Debit Account";
            this.labelControl3.Text = "من تـــاريـــخ";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(5, 189);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 14);
            this.labelControl9.TabIndex = 882;
            this.labelControl9.Tag = "Branch";
            this.labelControl9.Text = "الفــــــــرع";
            // 
            // cmbBranchesID
            // 
            this.cmbBranchesID.EnterMoveNextControl = true;
            this.cmbBranchesID.Location = new System.Drawing.Point(98, 186);
            this.cmbBranchesID.MenuManager = this.ribbonControl1;
            this.cmbBranchesID.Name = "cmbBranchesID";
            this.cmbBranchesID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbBranchesID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBranchesID.Properties.NullText = "";
            this.cmbBranchesID.Properties.PopupSizeable = false;
            this.cmbBranchesID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbBranchesID.Size = new System.Drawing.Size(374, 20);
            this.cmbBranchesID.TabIndex = 881;
            this.cmbBranchesID.Tag = "ImportantField";
            // 
            // frmVariousVouchersReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(973, 476);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.cmbBranchesID);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtToVoucherNo);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.txtFromDate);
            this.Controls.Add(this.txtToDate);
            this.Controls.Add(this.labelControl20);
            this.Controls.Add(this.txtFromVoucherNo);
            this.Controls.Add(this.gridControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVariousVouchersReport";
            this.Tag = "Daily Voucher ";
            this.Text = "تقرير سندات القيود اليومية";
            this.Load += new System.EventHandler(this.frmCheckReceiptVouchersReport_Load);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            this.Controls.SetChildIndex(this.txtFromVoucherNo, 0);
            this.Controls.SetChildIndex(this.labelControl20, 0);
            this.Controls.SetChildIndex(this.txtToDate, 0);
            this.Controls.SetChildIndex(this.txtFromDate, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.txtToVoucherNo, 0);
            this.Controls.SetChildIndex(this.labelControl1, 0);
            this.Controls.SetChildIndex(this.labelControl3, 0);
            this.Controls.SetChildIndex(this.labelControl2, 0);
            this.Controls.SetChildIndex(this.cmbBranchesID, 0);
            this.Controls.SetChildIndex(this.labelControl9, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromVoucherNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToVoucherNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBranchesID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnShow;
        private DevExpress.XtraEditors.DateEdit txtFromDate;
        private DevExpress.XtraEditors.DateEdit txtToDate;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.TextEdit txtFromVoucherNo;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView1;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColnSN;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColVoucherID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColVoucherDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColAmount;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDeclaration;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDocNo;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColRecordType;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTempRecordType;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColRegTime;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtToVoucherNo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColUserName;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LookUpEdit cmbBranchesID;
    }
}

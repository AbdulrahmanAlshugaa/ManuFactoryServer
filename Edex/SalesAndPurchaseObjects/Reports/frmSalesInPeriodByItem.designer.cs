namespace Edex.SalesAndPurchaseObjects.Reports
{
    partial class frmSalesInPeriodByItem
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
            this.label5 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.txtToDate = new DevExpress.XtraEditors.DateEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvolSn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColBarCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTotalQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTotalPurchase = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTotalDiscount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColNet = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnShow = new DevExpress.XtraEditors.SimpleButton();
            this.txtBarCode = new DevExpress.XtraEditors.TextEdit();
            this.lblBarCodeName = new DevExpress.XtraEditors.TextEdit();
            this.Label6 = new System.Windows.Forms.Label();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.cmbBranchesID = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBarCodeName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBranchesID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(951, 116);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(424, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 14);
            this.label5.TabIndex = 793;
            this.label5.Tag = " From Date";
            this.label5.Text = "من تـــــــــــاريخ";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(655, 128);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 14);
            this.Label2.TabIndex = 794;
            this.Label2.Tag = " To Date";
            this.Label2.Text = "الى تـــــــــــاريخ";
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(511, 127);
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
            this.txtFromDate.TabIndex = 2;
            this.txtFromDate.EditValueChanged += new System.EventHandler(this.txtFromDate_EditValueChanged);
            // 
            // txtToDate
            // 
            this.txtToDate.EditValue = null;
            this.txtToDate.EnterMoveNextControl = true;
            this.txtToDate.Location = new System.Drawing.Point(742, 127);
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
            this.txtToDate.TabIndex = 3;
            this.txtToDate.EditValueChanged += new System.EventHandler(this.txtToDate_EditValueChanged);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(15, 189);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(934, 275);
            this.gridControl1.TabIndex = 5;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.GroupFooter.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.GroupFooter.Options.UseFont = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dgvolSn,
            this.dgvColBarCode,
            this.dgvColItemName,
            this.dgvColTotalQty,
            this.dgvColTotalPurchase,
            this.dgvColTotalDiscount,
            this.dgvColNet});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.GroupPanelText = "بحث";
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            // 
            // dgvolSn
            // 
            this.dgvolSn.Caption = "م";
            this.dgvolSn.FieldName = "Sn";
            this.dgvolSn.Name = "dgvolSn";
            this.dgvolSn.ToolTip = "Sn";
            this.dgvolSn.Visible = true;
            this.dgvolSn.VisibleIndex = 0;
            this.dgvolSn.Width = 50;
            // 
            // dgvColBarCode
            // 
            this.dgvColBarCode.Caption = "كود الصنف";
            this.dgvColBarCode.FieldName = "BarCode";
            this.dgvColBarCode.Name = "dgvColBarCode";
            this.dgvColBarCode.ToolTip = "Invoice  ID";
            this.dgvColBarCode.Visible = true;
            this.dgvColBarCode.VisibleIndex = 1;
            this.dgvColBarCode.Width = 90;
            // 
            // dgvColItemName
            // 
            this.dgvColItemName.Caption = "أسم الصنف";
            this.dgvColItemName.FieldName = "ItemName";
            this.dgvColItemName.Name = "dgvColItemName";
            this.dgvColItemName.ToolTip = "Date";
            this.dgvColItemName.Visible = true;
            this.dgvColItemName.VisibleIndex = 2;
            this.dgvColItemName.Width = 90;
            // 
            // dgvColTotalQty
            // 
            this.dgvColTotalQty.Caption = "اجمالي الكمية";
            this.dgvColTotalQty.FieldName = "TotalQty";
            this.dgvColTotalQty.Name = "dgvColTotalQty";
            this.dgvColTotalQty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TotalQty", "{0:0.##}")});
            this.dgvColTotalQty.ToolTip = "Total";
            this.dgvColTotalQty.Visible = true;
            this.dgvColTotalQty.VisibleIndex = 3;
            this.dgvColTotalQty.Width = 90;
            // 
            // dgvColTotalPurchase
            // 
            this.dgvColTotalPurchase.Caption = "اجمالي المبيعات ";
            this.dgvColTotalPurchase.FieldName = "TotalPurchase";
            this.dgvColTotalPurchase.Name = "dgvColTotalPurchase";
            this.dgvColTotalPurchase.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TotalPurchase", "{0:0.##}")});
            this.dgvColTotalPurchase.UnboundExpression = "Net";
            this.dgvColTotalPurchase.Width = 90;
            // 
            // dgvColTotalDiscount
            // 
            this.dgvColTotalDiscount.Caption = "اجمالي الخصم";
            this.dgvColTotalDiscount.FieldName = "TotalDiscount";
            this.dgvColTotalDiscount.Name = "dgvColTotalDiscount";
            this.dgvColTotalDiscount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TotalDiscount", "{0:0.##}")});
            // 
            // dgvColNet
            // 
            this.dgvColNet.Caption = "صافي المبيعات";
            this.dgvColNet.FieldName = "Net";
            this.dgvColNet.Name = "dgvColNet";
            this.dgvColNet.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Net", "{0:0.##}")});
            this.dgvColNet.Visible = true;
            this.dgvColNet.VisibleIndex = 4;
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(864, 121);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(85, 29);
            this.btnShow.TabIndex = 4;
            this.btnShow.Tag = "Show ";
            this.btnShow.Text = "عــــــــرض";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txtBarCode
            // 
            this.txtBarCode.EnterMoveNextControl = true;
            this.txtBarCode.Location = new System.Drawing.Point(108, 126);
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtBarCode.Size = new System.Drawing.Size(101, 20);
            this.txtBarCode.TabIndex = 1;
            // 
            // lblBarCodeName
            // 
            this.lblBarCodeName.Location = new System.Drawing.Point(208, 126);
            this.lblBarCodeName.Name = "lblBarCodeName";
            this.lblBarCodeName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblBarCodeName.Properties.Appearance.Options.UseBackColor = true;
            this.lblBarCodeName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblBarCodeName.Properties.ReadOnly = true;
            this.lblBarCodeName.Size = new System.Drawing.Size(198, 20);
            this.lblBarCodeName.TabIndex = 787;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(27, 128);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(76, 14);
            this.Label6.TabIndex = 788;
            this.Label6.Tag = "BarCode Item";
            this.Label6.Text = "كــــــود الصنف";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(15, 156);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 14);
            this.labelControl9.TabIndex = 878;
            this.labelControl9.Tag = "Branch";
            this.labelControl9.Text = "الفــــــــرع";
            // 
            // cmbBranchesID
            // 
            this.cmbBranchesID.EnterMoveNextControl = true;
            this.cmbBranchesID.Location = new System.Drawing.Point(108, 153);
            this.cmbBranchesID.MenuManager = this.ribbonControl1;
            this.cmbBranchesID.Name = "cmbBranchesID";
            this.cmbBranchesID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbBranchesID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBranchesID.Properties.NullText = "";
            this.cmbBranchesID.Properties.PopupSizeable = false;
            this.cmbBranchesID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbBranchesID.Size = new System.Drawing.Size(515, 20);
            this.cmbBranchesID.TabIndex = 877;
            this.cmbBranchesID.Tag = "ImportantField";
            // 
            // frmSalesInPeriodByItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(951, 495);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.cmbBranchesID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtFromDate);
            this.Controls.Add(this.txtToDate);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.txtBarCode);
            this.Controls.Add(this.lblBarCodeName);
            this.Controls.Add(this.Label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSalesInPeriodByItem";
            this.Tag = "Sales Peroid By Item";
            this.Text = "مبيعات صنف لفترة زمنية";
            this.Load += new System.EventHandler(this.frmSalesInPeriodByItem_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSalesInPeriodByItem_KeyDown);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.Label6, 0);
            this.Controls.SetChildIndex(this.lblBarCodeName, 0);
            this.Controls.SetChildIndex(this.txtBarCode, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            this.Controls.SetChildIndex(this.txtToDate, 0);
            this.Controls.SetChildIndex(this.txtFromDate, 0);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbBranchesID, 0);
            this.Controls.SetChildIndex(this.labelControl9, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBarCodeName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBranchesID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label Label2;
        private DevExpress.XtraEditors.DateEdit txtFromDate;
        private DevExpress.XtraEditors.DateEdit txtToDate;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn dgvolSn;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColBarCode;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColItemName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTotalQty;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTotalPurchase;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTotalDiscount;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColNet;
        private DevExpress.XtraEditors.SimpleButton btnShow;
        private DevExpress.XtraEditors.TextEdit txtBarCode;
        private DevExpress.XtraEditors.TextEdit lblBarCodeName;
        internal System.Windows.Forms.Label Label6;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LookUpEdit cmbBranchesID;
    }
}

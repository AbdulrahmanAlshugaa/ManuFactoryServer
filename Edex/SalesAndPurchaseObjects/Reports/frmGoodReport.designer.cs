namespace Edex.SalesAndPurchaseObjects.Reports
{
    partial class frmGoodReport
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
            this.txtSupplierID = new DevExpress.XtraEditors.TextEdit();
            this.lblSupplierName = new DevExpress.XtraEditors.TextEdit();
            this.txtCostCenterID = new DevExpress.XtraEditors.TextEdit();
            this.lblCostCenterName = new DevExpress.XtraEditors.TextEdit();
            this.txtStoreID = new DevExpress.XtraEditors.TextEdit();
            this.txtFromInvoiceNo = new DevExpress.XtraEditors.TextEdit();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.lblStoreName = new DevExpress.XtraEditors.TextEdit();
            this.txtToDate = new DevExpress.XtraEditors.DateEdit();
            this.cmbMethodID = new DevExpress.XtraEditors.LookUpEdit();
            this.txtToInvoicNo = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.btnShow = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvolSn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColInvoiceID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColInvoiceDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDiscount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColVatAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColNet = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColMethodeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColSupplierName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColVatID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColStoreName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColCostCenterName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDelgateName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.lblNet = new DevExpress.XtraBars.BarStaticItem();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.cmbBranchesID = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplierID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSupplierName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostCenterID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCostCenterName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStoreID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromInvoiceNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStoreName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMethodID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToInvoicNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBranchesID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticItem1,
            this.lblNet});
            this.ribbonControl1.MaxItemId = 25;
            this.ribbonControl1.Size = new System.Drawing.Size(1258, 116);
            this.ribbonControl1.Click += new System.EventHandler(this.ribbonControl1_Click);
            // 
            // txtSupplierID
            // 
            this.txtSupplierID.EnterMoveNextControl = true;
            this.txtSupplierID.Location = new System.Drawing.Point(952, 128);
            this.txtSupplierID.Name = "txtSupplierID";
            this.txtSupplierID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSupplierID.Size = new System.Drawing.Size(101, 20);
            this.txtSupplierID.TabIndex = 4;
            this.txtSupplierID.Visible = false;
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.Location = new System.Drawing.Point(1052, 128);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSupplierName.Properties.Appearance.Options.UseBackColor = true;
            this.lblSupplierName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblSupplierName.Properties.ReadOnly = true;
            this.lblSupplierName.Size = new System.Drawing.Size(198, 20);
            this.lblSupplierName.TabIndex = 739;
            this.lblSupplierName.Visible = false;
            // 
            // txtCostCenterID
            // 
            this.txtCostCenterID.EnterMoveNextControl = true;
            this.txtCostCenterID.Location = new System.Drawing.Point(574, 165);
            this.txtCostCenterID.Name = "txtCostCenterID";
            this.txtCostCenterID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtCostCenterID.Size = new System.Drawing.Size(39, 20);
            this.txtCostCenterID.TabIndex = 3;
            // 
            // lblCostCenterName
            // 
            this.lblCostCenterName.Location = new System.Drawing.Point(611, 165);
            this.lblCostCenterName.Name = "lblCostCenterName";
            this.lblCostCenterName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCostCenterName.Properties.Appearance.Options.UseBackColor = true;
            this.lblCostCenterName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblCostCenterName.Properties.ReadOnly = true;
            this.lblCostCenterName.Size = new System.Drawing.Size(219, 20);
            this.lblCostCenterName.TabIndex = 747;
            // 
            // txtStoreID
            // 
            this.txtStoreID.EnterMoveNextControl = true;
            this.txtStoreID.Location = new System.Drawing.Point(574, 128);
            this.txtStoreID.Name = "txtStoreID";
            this.txtStoreID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtStoreID.Size = new System.Drawing.Size(39, 20);
            this.txtStoreID.TabIndex = 2;
            this.txtStoreID.Tag = "txtStoreID";
            this.txtStoreID.ToolTip = "StoreID";
            // 
            // txtFromInvoiceNo
            // 
            this.txtFromInvoiceNo.EnterMoveNextControl = true;
            this.txtFromInvoiceNo.Location = new System.Drawing.Point(108, 128);
            this.txtFromInvoiceNo.Name = "txtFromInvoiceNo";
            this.txtFromInvoiceNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFromInvoiceNo.Size = new System.Drawing.Size(102, 20);
            this.txtFromInvoiceNo.TabIndex = 733;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(493, 129);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(73, 14);
            this.Label8.TabIndex = 745;
            this.Label8.Tag = "Store";
            this.Label8.Text = "الـمـســـتـودع";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(493, 166);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(75, 14);
            this.Label7.TabIndex = 744;
            this.Label7.Tag = "Cost Center";
            this.Label7.Text = "مركز الـتكلـفـة";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(856, 130);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(80, 14);
            this.Label6.TabIndex = 743;
            this.Label6.Tag = "Supplier";
            this.Label6.Text = "الــــمـــــــــــورد";
            this.Label6.Visible = false;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(858, 162);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(88, 14);
            this.Label3.TabIndex = 742;
            this.Label3.Tag = "Pur. Method";
            this.Label3.Text = "طـريـقـة الشـراء ";
            this.Label3.Visible = false;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(271, 166);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 14);
            this.Label2.TabIndex = 741;
            this.Label2.Tag = " To Date";
            this.Label2.Text = "الى تـــــــــــاريخ";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(12, 129);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(90, 14);
            this.Label4.TabIndex = 740;
            this.Label4.Tag = " from nvoice ID";
            this.Label4.Text = "من الـفـاتـورة رقم";
            // 
            // lblStoreName
            // 
            this.lblStoreName.Location = new System.Drawing.Point(612, 128);
            this.lblStoreName.Name = "lblStoreName";
            this.lblStoreName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblStoreName.Properties.Appearance.Options.UseBackColor = true;
            this.lblStoreName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblStoreName.Properties.ReadOnly = true;
            this.lblStoreName.Size = new System.Drawing.Size(218, 20);
            this.lblStoreName.TabIndex = 746;
            // 
            // txtToDate
            // 
            this.txtToDate.EditValue = null;
            this.txtToDate.EnterMoveNextControl = true;
            this.txtToDate.Location = new System.Drawing.Point(358, 165);
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
            this.txtToDate.TabIndex = 1;
            // 
            // cmbMethodID
            // 
            this.cmbMethodID.EnterMoveNextControl = true;
            this.cmbMethodID.Location = new System.Drawing.Point(952, 160);
            this.cmbMethodID.MenuManager = this.ribbonControl1;
            this.cmbMethodID.Name = "cmbMethodID";
            this.cmbMethodID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbMethodID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbMethodID.Properties.NullText = "";
            this.cmbMethodID.Properties.PopupSizeable = false;
            this.cmbMethodID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbMethodID.Size = new System.Drawing.Size(101, 20);
            this.cmbMethodID.TabIndex = 5;
            this.cmbMethodID.Visible = false;
            // 
            // txtToInvoicNo
            // 
            this.txtToInvoicNo.EnterMoveNextControl = true;
            this.txtToInvoicNo.Location = new System.Drawing.Point(108, 165);
            this.txtToInvoicNo.Name = "txtToInvoicNo";
            this.txtToInvoicNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtToInvoicNo.Size = new System.Drawing.Size(102, 20);
            this.txtToInvoicNo.TabIndex = 748;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 14);
            this.label1.TabIndex = 749;
            this.label1.Tag = " To Invoice ID";
            this.label1.Text = "الى الـفـاتـورة رقم";
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(358, 128);
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
            this.txtFromDate.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(271, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 14);
            this.label5.TabIndex = 741;
            this.label5.Tag = " From Date";
            this.label5.Text = "من تـــــــــــاريخ";
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(1165, 156);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(85, 29);
            this.btnShow.TabIndex = 6;
            this.btnShow.Tag = "Show ";
            this.btnShow.Text = "عــــــــرض";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(0, 219);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1258, 347);
            this.gridControl1.TabIndex = 7;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.FooterPanel.Options.UseFont = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dgvolSn,
            this.dgvColInvoiceID,
            this.dgvColInvoiceDate,
            this.dgvColTotal,
            this.dgvColDiscount,
            this.dgvColVatAmount,
            this.dgvColNet,
            this.dgvColMethodeName,
            this.dgvColSupplierName,
            this.dgvColVatID,
            this.dgvColStoreName,
            this.dgvColCostCenterName,
            this.dgvColDelgateName,
            this.dgvColNotes});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
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
            // dgvColInvoiceID
            // 
            this.dgvColInvoiceID.Caption = "رقم الفاتورة";
            this.dgvColInvoiceID.FieldName = "InvoiceID";
            this.dgvColInvoiceID.Name = "dgvColInvoiceID";
            this.dgvColInvoiceID.ToolTip = "Invoice  ID";
            this.dgvColInvoiceID.Visible = true;
            this.dgvColInvoiceID.VisibleIndex = 1;
            this.dgvColInvoiceID.Width = 90;
            // 
            // dgvColInvoiceDate
            // 
            this.dgvColInvoiceDate.Caption = "التاريخ";
            this.dgvColInvoiceDate.FieldName = "nvoiceDate";
            this.dgvColInvoiceDate.Name = "dgvColInvoiceDate";
            this.dgvColInvoiceDate.ToolTip = "Date";
            this.dgvColInvoiceDate.Visible = true;
            this.dgvColInvoiceDate.VisibleIndex = 2;
            this.dgvColInvoiceDate.Width = 90;
            // 
            // dgvColTotal
            // 
            this.dgvColTotal.Caption = "الإجمالي";
            this.dgvColTotal.FieldName = "Total";
            this.dgvColTotal.Name = "dgvColTotal";
            this.dgvColTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "{0:0.##}")});
            this.dgvColTotal.ToolTip = "Total";
            this.dgvColTotal.Visible = true;
            this.dgvColTotal.VisibleIndex = 3;
            this.dgvColTotal.Width = 90;
            // 
            // dgvColDiscount
            // 
            this.dgvColDiscount.Caption = "الخصم";
            this.dgvColDiscount.FieldName = "Discount";
            this.dgvColDiscount.Name = "dgvColDiscount";
            this.dgvColDiscount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Discount", "{0:0.##}")});
            this.dgvColDiscount.ToolTip = "Discount";
            this.dgvColDiscount.Visible = true;
            this.dgvColDiscount.VisibleIndex = 4;
            this.dgvColDiscount.Width = 90;
            // 
            // dgvColVatAmount
            // 
            this.dgvColVatAmount.Caption = "القيمة المضافة";
            this.dgvColVatAmount.FieldName = "VatAmount";
            this.dgvColVatAmount.Name = "dgvColVatAmount";
            this.dgvColVatAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "VatAmount", "{0:0.##}")});
            this.dgvColVatAmount.ToolTip = "Vat Amount";
            this.dgvColVatAmount.Visible = true;
            this.dgvColVatAmount.VisibleIndex = 5;
            this.dgvColVatAmount.Width = 90;
            // 
            // dgvColNet
            // 
            this.dgvColNet.Caption = "الصافي";
            this.dgvColNet.FieldName = "Net";
            this.dgvColNet.Name = "dgvColNet";
            this.dgvColNet.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Net", "{0:0.##}")});
            this.dgvColNet.UnboundExpression = "Net";
            this.dgvColNet.Visible = true;
            this.dgvColNet.VisibleIndex = 6;
            this.dgvColNet.Width = 90;
            // 
            // dgvColMethodeName
            // 
            this.dgvColMethodeName.Caption = "طريقة الشراء";
            this.dgvColMethodeName.FieldName = "MethodeName";
            this.dgvColMethodeName.Name = "dgvColMethodeName";
            this.dgvColMethodeName.ToolTip = "Method  Name";
            this.dgvColMethodeName.Visible = true;
            this.dgvColMethodeName.VisibleIndex = 7;
            this.dgvColMethodeName.Width = 90;
            // 
            // dgvColSupplierName
            // 
            this.dgvColSupplierName.Caption = "اسم المورد";
            this.dgvColSupplierName.FieldName = "SupplierName";
            this.dgvColSupplierName.Name = "dgvColSupplierName";
            this.dgvColSupplierName.ToolTip = " Supplier Name";
            this.dgvColSupplierName.Visible = true;
            this.dgvColSupplierName.VisibleIndex = 8;
            this.dgvColSupplierName.Width = 90;
            // 
            // dgvColVatID
            // 
            this.dgvColVatID.Caption = "الرقم الضريبي";
            this.dgvColVatID.FieldName = "VatID";
            this.dgvColVatID.Name = "dgvColVatID";
            this.dgvColVatID.ToolTip = "lVat ID";
            this.dgvColVatID.Visible = true;
            this.dgvColVatID.VisibleIndex = 9;
            this.dgvColVatID.Width = 90;
            // 
            // dgvColStoreName
            // 
            this.dgvColStoreName.Caption = "المستودع";
            this.dgvColStoreName.FieldName = "StoreName";
            this.dgvColStoreName.Name = "dgvColStoreName";
            this.dgvColStoreName.ToolTip = "Store Name";
            this.dgvColStoreName.Visible = true;
            this.dgvColStoreName.VisibleIndex = 10;
            this.dgvColStoreName.Width = 90;
            // 
            // dgvColCostCenterName
            // 
            this.dgvColCostCenterName.Caption = "مركز التكلفة";
            this.dgvColCostCenterName.FieldName = "CostCenterName";
            this.dgvColCostCenterName.Name = "dgvColCostCenterName";
            this.dgvColCostCenterName.ToolTip = "Cost Center Name";
            this.dgvColCostCenterName.Visible = true;
            this.dgvColCostCenterName.VisibleIndex = 11;
            this.dgvColCostCenterName.Width = 90;
            // 
            // dgvColDelgateName
            // 
            this.dgvColDelgateName.Caption = "اسم المندوب";
            this.dgvColDelgateName.FieldName = "DelgateName";
            this.dgvColDelgateName.Name = "dgvColDelgateName";
            this.dgvColDelgateName.Visible = true;
            this.dgvColDelgateName.VisibleIndex = 12;
            this.dgvColDelgateName.Width = 90;
            // 
            // dgvColNotes
            // 
            this.dgvColNotes.Caption = "مــلاحظات";
            this.dgvColNotes.FieldName = "Notes";
            this.dgvColNotes.Name = "dgvColNotes";
            this.dgvColNotes.ToolTip = "Notes";
            this.dgvColNotes.Visible = true;
            this.dgvColNotes.VisibleIndex = 13;
            this.dgvColNotes.Width = 110;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "صافي المشتريات";
            this.barStaticItem1.Id = 23;
            this.barStaticItem1.ItemAppearance.Normal.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.barStaticItem1.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.barStaticItem1.ItemAppearance.Normal.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.barStaticItem1.ItemAppearance.Normal.Options.UseBackColor = true;
            this.barStaticItem1.ItemAppearance.Normal.Options.UseFont = true;
            this.barStaticItem1.ItemAppearance.Normal.Options.UseForeColor = true;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.Tag = "Net Purchase";
            // 
            // lblNet
            // 
            this.lblNet.Caption = "0";
            this.lblNet.Id = 24;
            this.lblNet.ItemAppearance.Normal.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lblNet.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblNet.ItemAppearance.Normal.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblNet.ItemAppearance.Normal.Options.UseBackColor = true;
            this.lblNet.ItemAppearance.Normal.Options.UseFont = true;
            this.lblNet.ItemAppearance.Normal.Options.UseForeColor = true;
            this.lblNet.Name = "lblNet";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(274, 199);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 14);
            this.labelControl9.TabIndex = 876;
            this.labelControl9.Tag = "Branch";
            this.labelControl9.Text = "الفــــــــرع";
            // 
            // cmbBranchesID
            // 
            this.cmbBranchesID.EnterMoveNextControl = true;
            this.cmbBranchesID.Location = new System.Drawing.Point(358, 193);
            this.cmbBranchesID.MenuManager = this.ribbonControl1;
            this.cmbBranchesID.Name = "cmbBranchesID";
            this.cmbBranchesID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbBranchesID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBranchesID.Properties.NullText = "";
            this.cmbBranchesID.Properties.PopupSizeable = false;
            this.cmbBranchesID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbBranchesID.Size = new System.Drawing.Size(472, 20);
            this.cmbBranchesID.TabIndex = 875;
            this.cmbBranchesID.Tag = "ImportantField";
            // 
            // frmGoodReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1258, 596);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.cmbBranchesID);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.txtToInvoicNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSupplierID);
            this.Controls.Add(this.lblSupplierName);
            this.Controls.Add(this.txtCostCenterID);
            this.Controls.Add(this.lblCostCenterName);
            this.Controls.Add(this.txtStoreID);
            this.Controls.Add(this.txtFromInvoiceNo);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.lblStoreName);
            this.Controls.Add(this.txtFromDate);
            this.Controls.Add(this.txtToDate);
            this.Controls.Add(this.cmbMethodID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGoodReport";
            this.Tag = "Purchases Invoices Report";
            this.Text = "دفتر فواتير مشتريات";
            this.Load += new System.EventHandler(this.frmPurchasesInvoiceReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPurchasesInvoiceReport_KeyDown);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.cmbMethodID, 0);
            this.Controls.SetChildIndex(this.txtToDate, 0);
            this.Controls.SetChildIndex(this.txtFromDate, 0);
            this.Controls.SetChildIndex(this.lblStoreName, 0);
            this.Controls.SetChildIndex(this.Label4, 0);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.Label3, 0);
            this.Controls.SetChildIndex(this.Label6, 0);
            this.Controls.SetChildIndex(this.Label7, 0);
            this.Controls.SetChildIndex(this.Label8, 0);
            this.Controls.SetChildIndex(this.txtFromInvoiceNo, 0);
            this.Controls.SetChildIndex(this.txtStoreID, 0);
            this.Controls.SetChildIndex(this.lblCostCenterName, 0);
            this.Controls.SetChildIndex(this.txtCostCenterID, 0);
            this.Controls.SetChildIndex(this.lblSupplierName, 0);
            this.Controls.SetChildIndex(this.txtSupplierID, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtToInvoicNo, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            this.Controls.SetChildIndex(this.cmbBranchesID, 0);
            this.Controls.SetChildIndex(this.labelControl9, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplierID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSupplierName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostCenterID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCostCenterName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStoreID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromInvoiceNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStoreName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMethodID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToInvoicNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBranchesID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtSupplierID;
        private DevExpress.XtraEditors.TextEdit lblSupplierName;
        private DevExpress.XtraEditors.TextEdit txtCostCenterID;
        private DevExpress.XtraEditors.TextEdit lblCostCenterName;
        private DevExpress.XtraEditors.TextEdit txtStoreID;
        private DevExpress.XtraEditors.TextEdit txtFromInvoiceNo;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label4;
        private DevExpress.XtraEditors.TextEdit lblStoreName;
        private DevExpress.XtraEditors.DateEdit txtToDate;
        private DevExpress.XtraEditors.LookUpEdit cmbMethodID;
        private DevExpress.XtraEditors.TextEdit txtToInvoicNo;
        internal System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.DateEdit txtFromDate;
        internal System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton btnShow;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem lblNet;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColInvoiceID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColInvoiceDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTotal;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColVatAmount;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColNet;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDiscount;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColNotes;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColMethodeName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColSupplierName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDelgateName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColStoreName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColVatID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColCostCenterName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvolSn;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LookUpEdit cmbBranchesID;

    }
}

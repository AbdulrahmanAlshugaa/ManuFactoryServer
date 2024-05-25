namespace Edex.SalesAndPurchaseObjects.Reports
{
    partial class frmNetworkCommission
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
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
            this.dgvColSellerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.dgvColVatID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColStoreName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColCostCenterName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDelgateName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.dgvColNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColCloseCashierDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColProfite = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvCustomerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnShow = new DevExpress.XtraEditors.SimpleButton();
            this.label5 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.txtToDate = new DevExpress.XtraEditors.DateEdit();
            this.lblSalesDelegateName = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.txtSalesDelegateID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblNet = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lblCashNet = new DevExpress.XtraEditors.TextEdit();
            this.cmbNetType = new DevExpress.XtraEditors.LookUpEdit();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesDelegateID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCashNet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNetType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(1348, 116);
            this.ribbonControl1.Click += new System.EventHandler(this.ribbonControl1_Click);
            // 
            // gridControl1
            // 
            gridLevelNode1.RelationName = "Level1";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl1.Location = new System.Drawing.Point(0, 172);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2});
            this.gridControl1.Size = new System.Drawing.Size(1352, 430);
            this.gridControl1.TabIndex = 13;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.dgvColSellerName,
            this.dgvColVatID,
            this.dgvColStoreName,
            this.dgvColCostCenterName,
            this.dgvColDelgateName,
            this.dgvColNotes,
            this.dgvColCloseCashierDate,
            this.dgvColProfite,
            this.dgvCustomerName});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsMenu.ShowFooterItem = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
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
            this.dgvolSn.Width = 49;
            // 
            // dgvColInvoiceID
            // 
            this.dgvColInvoiceID.Caption = "رقم الفاتورة";
            this.dgvColInvoiceID.FieldName = "InvoiceID";
            this.dgvColInvoiceID.Name = "dgvColInvoiceID";
            this.dgvColInvoiceID.ToolTip = "Invoice  ID";
            this.dgvColInvoiceID.Visible = true;
            this.dgvColInvoiceID.VisibleIndex = 1;
            this.dgvColInvoiceID.Width = 69;
            // 
            // dgvColInvoiceDate
            // 
            this.dgvColInvoiceDate.Caption = "التاريخ";
            this.dgvColInvoiceDate.FieldName = "nvoiceDate";
            this.dgvColInvoiceDate.Name = "dgvColInvoiceDate";
            this.dgvColInvoiceDate.ToolTip = "Date";
            this.dgvColInvoiceDate.Visible = true;
            this.dgvColInvoiceDate.VisibleIndex = 2;
            this.dgvColInvoiceDate.Width = 117;
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
            this.dgvColTotal.Width = 77;
            // 
            // dgvColDiscount
            // 
            this.dgvColDiscount.Caption = "الخصم";
            this.dgvColDiscount.FieldName = "Discount";
            this.dgvColDiscount.Name = "dgvColDiscount";
            this.dgvColDiscount.ToolTip = "Discount";
            this.dgvColDiscount.Visible = true;
            this.dgvColDiscount.VisibleIndex = 4;
            this.dgvColDiscount.Width = 70;
            // 
            // dgvColVatAmount
            // 
            this.dgvColVatAmount.Caption = "القيمة المضافة";
            this.dgvColVatAmount.FieldName = "VatAmount";
            this.dgvColVatAmount.Name = "dgvColVatAmount";
            this.dgvColVatAmount.ToolTip = "Vat Amount";
            this.dgvColVatAmount.Visible = true;
            this.dgvColVatAmount.VisibleIndex = 5;
            this.dgvColVatAmount.Width = 86;
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
            this.dgvColNet.Width = 69;
            // 
            // dgvColMethodeName
            // 
            this.dgvColMethodeName.Caption = "طريقة البيـــع";
            this.dgvColMethodeName.FieldName = "MethodeName";
            this.dgvColMethodeName.Name = "dgvColMethodeName";
            this.dgvColMethodeName.ToolTip = "Method  Name";
            this.dgvColMethodeName.Width = 86;
            // 
            // dgvColSellerName
            // 
            this.dgvColSellerName.Caption = "الشبكة ";
            this.dgvColSellerName.ColumnEdit = this.repositoryItemLookUpEdit2;
            this.dgvColSellerName.FieldName = "UserID";
            this.dgvColSellerName.Name = "dgvColSellerName";
            this.dgvColSellerName.ToolTip = " Supplier Name";
            this.dgvColSellerName.Visible = true;
            this.dgvColSellerName.VisibleIndex = 9;
            this.dgvColSellerName.Width = 110;
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            // 
            // dgvColVatID
            // 
            this.dgvColVatID.Caption = "الرقم الضريبي";
            this.dgvColVatID.FieldName = "NetPaid";
            this.dgvColVatID.Name = "dgvColVatID";
            this.dgvColVatID.ToolTip = "lVat ID";
            this.dgvColVatID.Width = 74;
            // 
            // dgvColStoreName
            // 
            this.dgvColStoreName.Caption = "المستودع";
            this.dgvColStoreName.FieldName = "StoreName";
            this.dgvColStoreName.Name = "dgvColStoreName";
            this.dgvColStoreName.ToolTip = "Store Name";
            this.dgvColStoreName.Width = 96;
            // 
            // dgvColCostCenterName
            // 
            this.dgvColCostCenterName.Caption = "مركز التكلفة";
            this.dgvColCostCenterName.FieldName = "CostCenterName";
            this.dgvColCostCenterName.Name = "dgvColCostCenterName";
            this.dgvColCostCenterName.ToolTip = "Cost Center Name";
            this.dgvColCostCenterName.Width = 92;
            // 
            // dgvColDelgateName
            // 
            this.dgvColDelgateName.Caption = "نوع الطلب";
            this.dgvColDelgateName.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.dgvColDelgateName.FieldName = "OrderTypes";
            this.dgvColDelgateName.Name = "dgvColDelgateName";
            this.dgvColDelgateName.Width = 95;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // dgvColNotes
            // 
            this.dgvColNotes.Caption = "مــلاحظات";
            this.dgvColNotes.FieldName = "Notes";
            this.dgvColNotes.Name = "dgvColNotes";
            this.dgvColNotes.ToolTip = "Notes";
            this.dgvColNotes.Visible = true;
            this.dgvColNotes.VisibleIndex = 8;
            this.dgvColNotes.Width = 132;
            // 
            // dgvColCloseCashierDate
            // 
            this.dgvColCloseCashierDate.Caption = "تاريخ الاغلاق";
            this.dgvColCloseCashierDate.FieldName = "CloseCashierDate";
            this.dgvColCloseCashierDate.Name = "dgvColCloseCashierDate";
            this.dgvColCloseCashierDate.ToolTip = "Close Date";
            this.dgvColCloseCashierDate.Width = 85;
            // 
            // dgvColProfite
            // 
            this.dgvColProfite.Caption = "الربح";
            this.dgvColProfite.FieldName = "Profit";
            this.dgvColProfite.Name = "dgvColProfite";
            this.dgvColProfite.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Profit", "{0:0.##}")});
            this.dgvColProfite.Width = 67;
            // 
            // dgvCustomerName
            // 
            this.dgvCustomerName.Caption = "عمولة الشبكة";
            this.dgvCustomerName.FieldName = "NetCommis";
            this.dgvCustomerName.Name = "dgvCustomerName";
            this.dgvCustomerName.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.dgvCustomerName.Visible = true;
            this.dgvCustomerName.VisibleIndex = 7;
            this.dgvCustomerName.Width = 101;
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(857, 122);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(94, 44);
            this.btnShow.TabIndex = 12;
            this.btnShow.Tag = "Show ";
            this.btnShow.Text = "عــــــــرض";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(271, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 14);
            this.label5.TabIndex = 810;
            this.label5.Tag = " From Date";
            this.label5.Text = "من تـــــــــــاريخ";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(547, 135);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 14);
            this.Label2.TabIndex = 809;
            this.Label2.Tag = "To Date";
            this.Label2.Text = "الى تـــــــــــاريخ";
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(358, 134);
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
            this.txtFromDate.TabIndex = 3;
            this.txtFromDate.EditValueChanged += new System.EventHandler(this.txtFromDate_EditValueChanged);
            // 
            // txtToDate
            // 
            this.txtToDate.EditValue = null;
            this.txtToDate.EnterMoveNextControl = true;
            this.txtToDate.Location = new System.Drawing.Point(634, 134);
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
            this.txtToDate.TabIndex = 4;
            this.txtToDate.EditValueChanged += new System.EventHandler(this.txtToDate_EditValueChanged);
            // 
            // lblSalesDelegateName
            // 
            this.lblSalesDelegateName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSalesDelegateName.Location = new System.Drawing.Point(1073, 24);
            this.lblSalesDelegateName.Name = "lblSalesDelegateName";
            this.lblSalesDelegateName.Size = new System.Drawing.Size(205, 20);
            this.lblSalesDelegateName.TabIndex = 838;
            this.lblSalesDelegateName.Visible = false;
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.BackColor = System.Drawing.Color.Transparent;
            this.Label13.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label13.Location = new System.Drawing.Point(877, 26);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(84, 14);
            this.Label13.TabIndex = 837;
            this.Label13.Tag = "Sales Delegates";
            this.Label13.Text = "مندوب المبيعات";
            this.Label13.Visible = false;
            this.Label13.Click += new System.EventHandler(this.Label13_Click);
            // 
            // txtSalesDelegateID
            // 
            this.txtSalesDelegateID.EnterMoveNextControl = true;
            this.txtSalesDelegateID.Location = new System.Drawing.Point(973, 24);
            this.txtSalesDelegateID.Name = "txtSalesDelegateID";
            this.txtSalesDelegateID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSalesDelegateID.Size = new System.Drawing.Size(101, 20);
            this.txtSalesDelegateID.TabIndex = 9;
            this.txtSalesDelegateID.Tag = "txtSalesDelegateID";
            this.txtSalesDelegateID.ToolTip = "SalesDelegateID";
            this.txtSalesDelegateID.Visible = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(46, 624);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(79, 13);
            this.labelControl3.TabIndex = 846;
            this.labelControl3.Tag = "Net Total";
            this.labelControl3.Text = "اجمالي الشبكة";
            this.labelControl3.Click += new System.EventHandler(this.labelControl3_Click);
            // 
            // lblNet
            // 
            this.lblNet.Location = new System.Drawing.Point(151, 622);
            this.lblNet.Name = "lblNet";
            this.lblNet.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblNet.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNet.Properties.Appearance.Options.UseBackColor = true;
            this.lblNet.Properties.Appearance.Options.UseFont = true;
            this.lblNet.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblNet.Properties.ReadOnly = true;
            this.lblNet.Size = new System.Drawing.Size(116, 20);
            this.lblNet.TabIndex = 845;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(37, 664);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(90, 13);
            this.labelControl5.TabIndex = 850;
            this.labelControl5.Tag = "Net Total";
            this.labelControl5.Text = "اجمالي العمولات ";
            // 
            // lblCashNet
            // 
            this.lblCashNet.Location = new System.Drawing.Point(151, 662);
            this.lblCashNet.Name = "lblCashNet";
            this.lblCashNet.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCashNet.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashNet.Properties.Appearance.Options.UseBackColor = true;
            this.lblCashNet.Properties.Appearance.Options.UseFont = true;
            this.lblCashNet.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblCashNet.Properties.ReadOnly = true;
            this.lblCashNet.Size = new System.Drawing.Size(143, 20);
            this.lblCashNet.TabIndex = 849;
            // 
            // cmbNetType
            // 
            this.cmbNetType.EnterMoveNextControl = true;
            this.cmbNetType.Location = new System.Drawing.Point(65, 134);
            this.cmbNetType.Name = "cmbNetType";
            this.cmbNetType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbNetType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbNetType.Properties.NullText = "";
            this.cmbNetType.Properties.PopupSizeable = false;
            this.cmbNetType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbNetType.Size = new System.Drawing.Size(151, 20);
            this.cmbNetType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 14);
            this.label1.TabIndex = 811;
            this.label1.Tag = "Sall Method";
            this.label1.Text = "الشبكة ";
            // 
            // frmNetworkCommission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1348, 732);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.lblCashNet);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.lblNet);
            this.Controls.Add(this.txtSalesDelegateID);
            this.Controls.Add(this.lblSalesDelegateName);
            this.Controls.Add(this.Label13);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtFromDate);
            this.Controls.Add(this.txtToDate);
            this.Controls.Add(this.cmbNetType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1368, 2000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1364, 766);
            this.Name = "frmNetworkCommission";
            this.Tag = "Sales Invoice Report ";
            this.Text = "تقرير عمولات الشبكات ";
            this.Load += new System.EventHandler(this.frmSalesInvoiceReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSalesInvoiceReport_KeyDown);
            this.Controls.SetChildIndex(this.cmbNetType, 0);
            this.Controls.SetChildIndex(this.txtToDate, 0);
            this.Controls.SetChildIndex(this.txtFromDate, 0);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            this.Controls.SetChildIndex(this.Label13, 0);
            this.Controls.SetChildIndex(this.lblSalesDelegateName, 0);
            this.Controls.SetChildIndex(this.txtSalesDelegateID, 0);
            this.Controls.SetChildIndex(this.lblNet, 0);
            this.Controls.SetChildIndex(this.labelControl3, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.lblCashNet, 0);
            this.Controls.SetChildIndex(this.labelControl5, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesDelegateID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCashNet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNetType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn dgvolSn;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColInvoiceID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColInvoiceDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTotal;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDiscount;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColVatAmount;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColNet;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColMethodeName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColSellerName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColVatID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColStoreName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColCostCenterName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDelgateName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColNotes;
        private DevExpress.XtraEditors.SimpleButton btnShow;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label Label2;
        private DevExpress.XtraEditors.DateEdit txtFromDate;
        private DevExpress.XtraEditors.DateEdit txtToDate;
        internal System.Windows.Forms.Label lblSalesDelegateName;
        internal System.Windows.Forms.Label Label13;
        private DevExpress.XtraEditors.TextEdit txtSalesDelegateID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColCloseCashierDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColProfite;
        private DevExpress.XtraGrid.Columns.GridColumn dgvCustomerName;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit lblNet;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit lblCashNet;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraEditors.LookUpEdit cmbNetType;
        internal System.Windows.Forms.Label label1;
    }
}

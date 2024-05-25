namespace Edex.AccountsObjects.Reports
{
    partial class frmNetworkCommission1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNetworkCommission1));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.GridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvColn_invoice_serial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDebit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColCredit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDeclaration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTheDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColOppsiteAccountName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColRecordType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTempRecordType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColRegTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.btnDebitSearch = new DevExpress.XtraEditors.SimpleButton();
            this.lblAccountName = new DevExpress.XtraEditors.TextEdit();
            this.txtAccountID = new DevExpress.XtraEditors.TextEdit();
            this.txtCostCenterID = new DevExpress.XtraEditors.TextEdit();
            this.lblCostCenterName = new DevExpress.XtraEditors.TextEdit();
            this.Label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.txtToDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblDebit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lblCredit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblBalanceSum = new DevExpress.XtraEditors.TextEdit();
            this.btnShow = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lblBalanceType = new DevExpress.XtraEditors.TextEdit();
            this.btnCostCenterSearch = new DevExpress.XtraEditors.SimpleButton();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.lblBalanceType1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.lblBalanceSum1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.lblCredit1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.lblDebit1 = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbNetType = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostCenterID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCostCenterName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceSum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceType1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceSum1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNetType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(1184, 116);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 179);
            this.gridControl1.MainView = this.GridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1160, 320);
            this.gridControl1.TabIndex = 752;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // GridView1
            // 
            this.GridView1.ActiveFilterEnabled = false;
            this.GridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dgvColn_invoice_serial,
            this.dgvColID,
            this.dgvColBalance,
            this.dgvColDebit,
            this.dgvColCredit,
            this.dgvColDeclaration,
            this.dgvColTheDate,
            this.dgvColOppsiteAccountName,
            this.dgvColRecordType,
            this.dgvColTempRecordType,
            this.dgvColRegTime});
            this.GridView1.GridControl = this.gridControl1;
            this.GridView1.Name = "GridView1";
            this.GridView1.OptionsCustomization.AllowFilter = false;
            this.GridView1.OptionsCustomization.AllowGroup = false;
            this.GridView1.OptionsCustomization.AllowSort = false;
            this.GridView1.OptionsFilter.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.GridView1.OptionsFilter.AllowFilterEditor = false;
            this.GridView1.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.GridView1.OptionsFilter.AllowMRUFilterList = false;
            this.GridView1.OptionsFilter.AllowMultiSelectInCheckedFilterPopup = false;
            this.GridView1.OptionsFilter.FilterEditorUseMenuForOperandsAndOperators = false;
            this.GridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.GridView1.DataSourceChanged += new System.EventHandler(this.GridView1_DataSourceChanged);
            // 
            // dgvColn_invoice_serial
            // 
            this.dgvColn_invoice_serial.Caption = "م";
            this.dgvColn_invoice_serial.FieldName = "n_invoice_serial";
            this.dgvColn_invoice_serial.Name = "dgvColn_invoice_serial";
            this.dgvColn_invoice_serial.OptionsFilter.AllowAutoFilter = false;
            this.dgvColn_invoice_serial.OptionsFilter.AllowFilter = false;
            this.dgvColn_invoice_serial.Visible = true;
            this.dgvColn_invoice_serial.VisibleIndex = 0;
            this.dgvColn_invoice_serial.Width = 58;
            // 
            // dgvColID
            // 
            this.dgvColID.Caption = "رقم الحركة";
            this.dgvColID.FieldName = "ID";
            this.dgvColID.Name = "dgvColID";
            this.dgvColID.Visible = true;
            this.dgvColID.VisibleIndex = 3;
            this.dgvColID.Width = 92;
            // 
            // dgvColBalance
            // 
            this.dgvColBalance.Caption = "الرصيد";
            this.dgvColBalance.FieldName = "Balance";
            this.dgvColBalance.Name = "dgvColBalance";
            this.dgvColBalance.OptionsFilter.AllowAutoFilter = false;
            this.dgvColBalance.OptionsFilter.AllowFilter = false;
            this.dgvColBalance.Width = 118;
            // 
            // dgvColDebit
            // 
            this.dgvColDebit.Caption = "مدين";
            this.dgvColDebit.FieldName = "Debit";
            this.dgvColDebit.Name = "dgvColDebit";
            this.dgvColDebit.OptionsFilter.AllowAutoFilter = false;
            this.dgvColDebit.OptionsFilter.AllowFilter = false;
            this.dgvColDebit.Visible = true;
            this.dgvColDebit.VisibleIndex = 1;
            this.dgvColDebit.Width = 90;
            // 
            // dgvColCredit
            // 
            this.dgvColCredit.Caption = "العمولة ";
            this.dgvColCredit.FieldName = "Credit";
            this.dgvColCredit.Name = "dgvColCredit";
            this.dgvColCredit.OptionsFilter.AllowAutoFilter = false;
            this.dgvColCredit.OptionsFilter.AllowFilter = false;
            this.dgvColCredit.Visible = true;
            this.dgvColCredit.VisibleIndex = 2;
            this.dgvColCredit.Width = 90;
            // 
            // dgvColDeclaration
            // 
            this.dgvColDeclaration.Caption = "البيان";
            this.dgvColDeclaration.FieldName = "Declaration";
            this.dgvColDeclaration.Name = "dgvColDeclaration";
            this.dgvColDeclaration.OptionsFilter.AllowAutoFilter = false;
            this.dgvColDeclaration.OptionsFilter.AllowFilter = false;
            this.dgvColDeclaration.Visible = true;
            this.dgvColDeclaration.VisibleIndex = 6;
            this.dgvColDeclaration.Width = 229;
            // 
            // dgvColTheDate
            // 
            this.dgvColTheDate.Caption = "التاريخ";
            this.dgvColTheDate.FieldName = "TheDate";
            this.dgvColTheDate.Name = "dgvColTheDate";
            this.dgvColTheDate.OptionsFilter.AllowAutoFilter = false;
            this.dgvColTheDate.OptionsFilter.AllowFilter = false;
            this.dgvColTheDate.Visible = true;
            this.dgvColTheDate.VisibleIndex = 4;
            this.dgvColTheDate.Width = 85;
            // 
            // dgvColOppsiteAccountName
            // 
            this.dgvColOppsiteAccountName.Caption = "الحساب المقابل";
            this.dgvColOppsiteAccountName.FieldName = "OppsiteAccountName";
            this.dgvColOppsiteAccountName.Name = "dgvColOppsiteAccountName";
            this.dgvColOppsiteAccountName.OptionsFilter.AllowAutoFilter = false;
            this.dgvColOppsiteAccountName.OptionsFilter.AllowFilter = false;
            this.dgvColOppsiteAccountName.Visible = true;
            this.dgvColOppsiteAccountName.VisibleIndex = 7;
            this.dgvColOppsiteAccountName.Width = 240;
            // 
            // dgvColRecordType
            // 
            this.dgvColRecordType.Caption = "نوع الحركة";
            this.dgvColRecordType.FieldName = "RecordType";
            this.dgvColRecordType.Name = "dgvColRecordType";
            this.dgvColRecordType.OptionsFilter.AllowAutoFilter = false;
            this.dgvColRecordType.OptionsFilter.AllowFilter = false;
            this.dgvColRecordType.Visible = true;
            this.dgvColRecordType.VisibleIndex = 5;
            this.dgvColRecordType.Width = 140;
            // 
            // dgvColTempRecordType
            // 
            this.dgvColTempRecordType.Caption = "النوع ";
            this.dgvColTempRecordType.FieldName = "TempRecordType";
            this.dgvColTempRecordType.Name = "dgvColTempRecordType";
            this.dgvColTempRecordType.OptionsFilter.AllowAutoFilter = false;
            this.dgvColTempRecordType.OptionsFilter.AllowFilter = false;
            this.dgvColTempRecordType.Width = 103;
            // 
            // dgvColRegTime
            // 
            this.dgvColRegTime.Caption = "الوقت";
            this.dgvColRegTime.FieldName = "RegTime";
            this.dgvColRegTime.Name = "dgvColRegTime";
            this.dgvColRegTime.OptionsFilter.AllowAutoFilter = false;
            this.dgvColRegTime.OptionsFilter.AllowFilter = false;
            this.dgvColRegTime.Width = 136;
            // 
            // labelControl20
            // 
            this.labelControl20.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl20.Appearance.Options.UseFont = true;
            this.labelControl20.Location = new System.Drawing.Point(44, 123);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(40, 14);
            this.labelControl20.TabIndex = 822;
            this.labelControl20.Tag = "Account No";
            this.labelControl20.Text = "الشبكة ";
            this.labelControl20.Visible = false;
            // 
            // btnDebitSearch
            // 
            this.btnDebitSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDebitSearch.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnDebitSearch.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDebitSearch.ImageOptions.Image")));
            this.btnDebitSearch.Location = new System.Drawing.Point(392, 114);
            this.btnDebitSearch.Name = "btnDebitSearch";
            this.btnDebitSearch.Size = new System.Drawing.Size(25, 23);
            this.btnDebitSearch.TabIndex = 821;
            this.btnDebitSearch.Visible = false;
            this.btnDebitSearch.Click += new System.EventHandler(this.btnDebitSearch_Click);
            // 
            // lblAccountName
            // 
            this.lblAccountName.Location = new System.Drawing.Point(209, 115);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblAccountName.Properties.Appearance.Options.UseBackColor = true;
            this.lblAccountName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblAccountName.Size = new System.Drawing.Size(182, 20);
            this.lblAccountName.TabIndex = 820;
            this.lblAccountName.Visible = false;
            // 
            // txtAccountID
            // 
            this.txtAccountID.EnterMoveNextControl = true;
            this.txtAccountID.Location = new System.Drawing.Point(118, 115);
            this.txtAccountID.Name = "txtAccountID";
            this.txtAccountID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtAccountID.Size = new System.Drawing.Size(92, 20);
            this.txtAccountID.TabIndex = 819;
            this.txtAccountID.Tag = "ImportantFieldGreaterThanZero";
            this.txtAccountID.Visible = false;
            this.txtAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.txtAccountID_Validating);
            // 
            // txtCostCenterID
            // 
            this.txtCostCenterID.EnterMoveNextControl = true;
            this.txtCostCenterID.Location = new System.Drawing.Point(622, 34);
            this.txtCostCenterID.Name = "txtCostCenterID";
            this.txtCostCenterID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtCostCenterID.Size = new System.Drawing.Size(56, 20);
            this.txtCostCenterID.TabIndex = 823;
            this.txtCostCenterID.Visible = false;
            this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
            // 
            // lblCostCenterName
            // 
            this.lblCostCenterName.Location = new System.Drawing.Point(676, 34);
            this.lblCostCenterName.Name = "lblCostCenterName";
            this.lblCostCenterName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCostCenterName.Properties.Appearance.Options.UseBackColor = true;
            this.lblCostCenterName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblCostCenterName.Size = new System.Drawing.Size(217, 20);
            this.lblCostCenterName.TabIndex = 829;
            this.lblCostCenterName.Visible = false;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(503, 37);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(75, 14);
            this.Label7.TabIndex = 828;
            this.Label7.Tag = "Cost Center";
            this.Label7.Text = "مركز الـتكلـفـة";
            this.Label7.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(428, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 14);
            this.label5.TabIndex = 826;
            this.label5.Tag = "From Date";
            this.label5.Text = "من تـــــــــــاريخ";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(425, 153);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 14);
            this.Label2.TabIndex = 827;
            this.Label2.Tag = " To Date";
            this.Label2.Text = "الى تـــــــــــاريخ";
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(512, 114);
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
            this.txtFromDate.TabIndex = 824;
            this.txtFromDate.EditValueChanged += new System.EventHandler(this.txtFromDate_EditValueChanged);
            // 
            // txtToDate
            // 
            this.txtToDate.EditValue = null;
            this.txtToDate.EnterMoveNextControl = true;
            this.txtToDate.Location = new System.Drawing.Point(512, 151);
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
            this.txtToDate.TabIndex = 825;
            this.txtToDate.EditValueChanged += new System.EventHandler(this.txtToDate_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(23, 507);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 14);
            this.labelControl1.TabIndex = 831;
            this.labelControl1.Tag = "Debit Account";
            this.labelControl1.Text = "اجمالي الشبكة";
            // 
            // lblDebit
            // 
            this.lblDebit.Location = new System.Drawing.Point(128, 505);
            this.lblDebit.Name = "lblDebit";
            this.lblDebit.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDebit.Properties.Appearance.Options.UseBackColor = true;
            this.lblDebit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblDebit.Size = new System.Drawing.Size(116, 20);
            this.lblDebit.TabIndex = 830;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(251, 507);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 14);
            this.labelControl2.TabIndex = 833;
            this.labelControl2.Tag = "Debit Account";
            this.labelControl2.Text = "اجمالي العمولة";
            // 
            // lblCredit
            // 
            this.lblCredit.Location = new System.Drawing.Point(326, 505);
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCredit.Properties.Appearance.Options.UseBackColor = true;
            this.lblCredit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblCredit.Size = new System.Drawing.Size(116, 20);
            this.lblCredit.TabIndex = 832;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(448, 509);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 14);
            this.labelControl3.TabIndex = 835;
            this.labelControl3.Tag = "Debit Account";
            this.labelControl3.Text = "إجمالي الرصيد";
            this.labelControl3.Visible = false;
            // 
            // lblBalanceSum
            // 
            this.lblBalanceSum.Location = new System.Drawing.Point(526, 507);
            this.lblBalanceSum.Name = "lblBalanceSum";
            this.lblBalanceSum.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblBalanceSum.Properties.Appearance.Options.UseBackColor = true;
            this.lblBalanceSum.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblBalanceSum.Size = new System.Drawing.Size(116, 20);
            this.lblBalanceSum.TabIndex = 834;
            this.lblBalanceSum.Visible = false;
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(639, 122);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(119, 45);
            this.btnShow.TabIndex = 836;
            this.btnShow.Tag = "Show ";
            this.btnShow.Text = "عــــــــرض";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(654, 509);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(50, 14);
            this.labelControl4.TabIndex = 838;
            this.labelControl4.Tag = "Debit Account";
            this.labelControl4.Text = "نوع الرصيد";
            this.labelControl4.Visible = false;
            // 
            // lblBalanceType
            // 
            this.lblBalanceType.Location = new System.Drawing.Point(710, 508);
            this.lblBalanceType.Name = "lblBalanceType";
            this.lblBalanceType.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblBalanceType.Properties.Appearance.Options.UseBackColor = true;
            this.lblBalanceType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblBalanceType.Size = new System.Drawing.Size(311, 20);
            this.lblBalanceType.TabIndex = 839;
            this.lblBalanceType.Visible = false;
            // 
            // btnCostCenterSearch
            // 
            this.btnCostCenterSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCostCenterSearch.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnCostCenterSearch.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCostCenterSearch.ImageOptions.Image")));
            this.btnCostCenterSearch.Location = new System.Drawing.Point(894, 32);
            this.btnCostCenterSearch.Name = "btnCostCenterSearch";
            this.btnCostCenterSearch.Size = new System.Drawing.Size(25, 23);
            this.btnCostCenterSearch.TabIndex = 840;
            this.btnCostCenterSearch.Visible = false;
            this.btnCostCenterSearch.Click += new System.EventHandler(this.btnCostCenterSearch_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(1053, 505);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(119, 23);
            this.ProgressBar.TabIndex = 864;
            this.ProgressBar.Click += new System.EventHandler(this.ProgressBar_Click);
            // 
            // lblBalanceType1
            // 
            this.lblBalanceType1.Location = new System.Drawing.Point(710, 551);
            this.lblBalanceType1.Name = "lblBalanceType1";
            this.lblBalanceType1.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblBalanceType1.Properties.Appearance.Options.UseBackColor = true;
            this.lblBalanceType1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblBalanceType1.Size = new System.Drawing.Size(311, 20);
            this.lblBalanceType1.TabIndex = 872;
            this.lblBalanceType1.Visible = false;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(654, 552);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(50, 14);
            this.labelControl5.TabIndex = 871;
            this.labelControl5.Tag = "Debit Account";
            this.labelControl5.Text = "نوع الرصيد";
            this.labelControl5.Visible = false;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(448, 552);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(72, 14);
            this.labelControl6.TabIndex = 870;
            this.labelControl6.Tag = "Debit Account";
            this.labelControl6.Text = "إجمالي الرصيد";
            this.labelControl6.Visible = false;
            // 
            // lblBalanceSum1
            // 
            this.lblBalanceSum1.Location = new System.Drawing.Point(526, 550);
            this.lblBalanceSum1.Name = "lblBalanceSum1";
            this.lblBalanceSum1.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblBalanceSum1.Properties.Appearance.Options.UseBackColor = true;
            this.lblBalanceSum1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblBalanceSum1.Size = new System.Drawing.Size(116, 20);
            this.lblBalanceSum1.TabIndex = 869;
            this.lblBalanceSum1.Visible = false;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(251, 550);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(69, 14);
            this.labelControl7.TabIndex = 868;
            this.labelControl7.Tag = "Debit Account";
            this.labelControl7.Text = "إجمالي الدائن";
            this.labelControl7.Visible = false;
            // 
            // lblCredit1
            // 
            this.lblCredit1.Location = new System.Drawing.Point(326, 548);
            this.lblCredit1.Name = "lblCredit1";
            this.lblCredit1.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCredit1.Properties.Appearance.Options.UseBackColor = true;
            this.lblCredit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblCredit1.Size = new System.Drawing.Size(116, 20);
            this.lblCredit1.TabIndex = 867;
            this.lblCredit1.Visible = false;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(23, 550);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(95, 14);
            this.labelControl8.TabIndex = 866;
            this.labelControl8.Tag = "Debit Account";
            this.labelControl8.Text = "حـســاب الـمـــديـن";
            this.labelControl8.Visible = false;
            // 
            // lblDebit1
            // 
            this.lblDebit1.Location = new System.Drawing.Point(128, 548);
            this.lblDebit1.Name = "lblDebit1";
            this.lblDebit1.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDebit1.Properties.Appearance.Options.UseBackColor = true;
            this.lblDebit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblDebit1.Size = new System.Drawing.Size(116, 20);
            this.lblDebit1.TabIndex = 865;
            this.lblDebit1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(97, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 14);
            this.label1.TabIndex = 874;
            this.label1.Tag = "Sall Method";
            this.label1.Text = "الشبكة ";
            // 
            // cmbNetType
            // 
            this.cmbNetType.EnterMoveNextControl = true;
            this.cmbNetType.Location = new System.Drawing.Point(150, 153);
            this.cmbNetType.Name = "cmbNetType";
            this.cmbNetType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbNetType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbNetType.Properties.NullText = "";
            this.cmbNetType.Properties.PopupSizeable = false;
            this.cmbNetType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbNetType.Size = new System.Drawing.Size(151, 20);
            this.cmbNetType.TabIndex = 873;
            // 
            // frmNetworkCommission1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1184, 601);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbNetType);
            this.Controls.Add(this.lblBalanceType1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.lblBalanceSum1);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.lblCredit1);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.lblDebit1);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.btnCostCenterSearch);
            this.Controls.Add(this.lblBalanceType);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.lblBalanceSum);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lblCredit);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lblDebit);
            this.Controls.Add(this.txtCostCenterID);
            this.Controls.Add(this.lblCostCenterName);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtFromDate);
            this.Controls.Add(this.txtToDate);
            this.Controls.Add(this.labelControl20);
            this.Controls.Add(this.btnDebitSearch);
            this.Controls.Add(this.lblAccountName);
            this.Controls.Add(this.txtAccountID);
            this.Controls.Add(this.gridControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNetworkCommission1";
            this.Tag = "AcountStatement";
            this.Text = "تقرير عمولات الشبكات ";
            this.Load += new System.EventHandler(this.frmAccountStatement_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAccountStatement_KeyDown);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            this.Controls.SetChildIndex(this.txtAccountID, 0);
            this.Controls.SetChildIndex(this.lblAccountName, 0);
            this.Controls.SetChildIndex(this.btnDebitSearch, 0);
            this.Controls.SetChildIndex(this.labelControl20, 0);
            this.Controls.SetChildIndex(this.txtToDate, 0);
            this.Controls.SetChildIndex(this.txtFromDate, 0);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.Label7, 0);
            this.Controls.SetChildIndex(this.lblCostCenterName, 0);
            this.Controls.SetChildIndex(this.txtCostCenterID, 0);
            this.Controls.SetChildIndex(this.lblDebit, 0);
            this.Controls.SetChildIndex(this.labelControl1, 0);
            this.Controls.SetChildIndex(this.lblCredit, 0);
            this.Controls.SetChildIndex(this.labelControl2, 0);
            this.Controls.SetChildIndex(this.lblBalanceSum, 0);
            this.Controls.SetChildIndex(this.labelControl3, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.labelControl4, 0);
            this.Controls.SetChildIndex(this.lblBalanceType, 0);
            this.Controls.SetChildIndex(this.btnCostCenterSearch, 0);
            this.Controls.SetChildIndex(this.ProgressBar, 0);
            this.Controls.SetChildIndex(this.lblDebit1, 0);
            this.Controls.SetChildIndex(this.labelControl8, 0);
            this.Controls.SetChildIndex(this.lblCredit1, 0);
            this.Controls.SetChildIndex(this.labelControl7, 0);
            this.Controls.SetChildIndex(this.lblBalanceSum1, 0);
            this.Controls.SetChildIndex(this.labelControl6, 0);
            this.Controls.SetChildIndex(this.labelControl5, 0);
            this.Controls.SetChildIndex(this.lblBalanceType1, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.cmbNetType, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostCenterID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCostCenterName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceSum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceType1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceSum1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNetType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView1;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.SimpleButton btnDebitSearch;
        private DevExpress.XtraEditors.TextEdit lblAccountName;
        private DevExpress.XtraEditors.TextEdit txtCostCenterID;
        private DevExpress.XtraEditors.TextEdit lblCostCenterName;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label Label2;
        public DevExpress.XtraEditors.DateEdit txtFromDate;
        public DevExpress.XtraEditors.DateEdit txtToDate;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColn_invoice_serial;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColBalance;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDebit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColCredit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDeclaration;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTheDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColOppsiteAccountName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColRecordType;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTempRecordType;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColRegTime;
        private DevExpress.XtraEditors.TextEdit lblBalanceType;
        private DevExpress.XtraEditors.SimpleButton btnCostCenterSearch;
        private System.Windows.Forms.ProgressBar ProgressBar;
        public DevExpress.XtraEditors.TextEdit txtAccountID;
        public DevExpress.XtraEditors.TextEdit lblBalanceSum;
        public DevExpress.XtraEditors.SimpleButton btnShow;
        public DevExpress.XtraEditors.TextEdit lblDebit;
        public DevExpress.XtraEditors.TextEdit lblCredit;
        private DevExpress.XtraEditors.TextEdit lblBalanceType1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        public DevExpress.XtraEditors.TextEdit lblBalanceSum1;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        public DevExpress.XtraEditors.TextEdit lblCredit1;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        public DevExpress.XtraEditors.TextEdit lblDebit1;
        internal System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.LookUpEdit cmbNetType;
    }
}

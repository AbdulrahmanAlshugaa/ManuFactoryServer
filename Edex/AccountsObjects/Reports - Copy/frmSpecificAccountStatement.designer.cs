namespace Edex.AccountsObjects.Reports
{
  

    partial class frmSpecificAccountStatement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSpecificAccountStatement));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.GridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvColn_invoice_serial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColAccountID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDebit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColCredit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColAccountName = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.chkSupliar = new DevExpress.XtraEditors.CheckEdit();
            this.chkCustomer = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.btnToAcountID = new DevExpress.XtraEditors.SimpleButton();
            this.lblToAccountID = new DevExpress.XtraEditors.TextEdit();
            this.txtToAccountID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnFromAcountID = new DevExpress.XtraEditors.SimpleButton();
            this.lblFromAccountID = new DevExpress.XtraEditors.TextEdit();
            this.txtFromAccountID = new DevExpress.XtraEditors.TextEdit();
            this.lblParentAcountName = new DevExpress.XtraEditors.TextEdit();
            this.txtParentAcountID = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCostCenterSearch = new DevExpress.XtraEditors.SimpleButton();
            this.btnParentAcountIDSerach = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.chkSupliar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToAccountID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToAccountID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFromAccountID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromAccountID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentAcountName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentAcountID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(1210, 116);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(9, 191);
            this.gridControl1.MainView = this.GridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1177, 337);
            this.gridControl1.TabIndex = 752;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // GridView1
            // 
            this.GridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dgvColn_invoice_serial,
            this.dgvColAccountID,
            this.dgvColBalance,
            this.dgvColDebit,
            this.dgvColCredit,
            this.dgvColAccountName});
            this.GridView1.GridControl = this.gridControl1;
            this.GridView1.Name = "GridView1";
            this.GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsBehavior.Editable = false;
            this.GridView1.OptionsBehavior.ReadOnly = true;
            this.GridView1.OptionsCustomization.AllowFilter = false;
            this.GridView1.OptionsCustomization.AllowGroup = false;
            this.GridView1.OptionsCustomization.AllowSort = false;
            // 
            // dgvColn_invoice_serial
            // 
            this.dgvColn_invoice_serial.Caption = "م";
            this.dgvColn_invoice_serial.FieldName = "n_invoice_serial";
            this.dgvColn_invoice_serial.Name = "dgvColn_invoice_serial";
            this.dgvColn_invoice_serial.Visible = true;
            this.dgvColn_invoice_serial.VisibleIndex = 0;
            this.dgvColn_invoice_serial.Width = 58;
            // 
            // dgvColAccountID
            // 
            this.dgvColAccountID.Caption = "رقم الحساب";
            this.dgvColAccountID.FieldName = "AccountID";
            this.dgvColAccountID.Name = "dgvColAccountID";
            this.dgvColAccountID.Visible = true;
            this.dgvColAccountID.VisibleIndex = 1;
            this.dgvColAccountID.Width = 92;
            // 
            // dgvColBalance
            // 
            this.dgvColBalance.Caption = "الرصيد";
            this.dgvColBalance.FieldName = "Balance";
            this.dgvColBalance.Name = "dgvColBalance";
            this.dgvColBalance.Visible = true;
            this.dgvColBalance.VisibleIndex = 5;
            this.dgvColBalance.Width = 117;
            // 
            // dgvColDebit
            // 
            this.dgvColDebit.Caption = "مدين";
            this.dgvColDebit.FieldName = "Debit";
            this.dgvColDebit.Name = "dgvColDebit";
            this.dgvColDebit.Visible = true;
            this.dgvColDebit.VisibleIndex = 3;
            this.dgvColDebit.Width = 90;
            // 
            // dgvColCredit
            // 
            this.dgvColCredit.Caption = "دائن";
            this.dgvColCredit.FieldName = "Credit";
            this.dgvColCredit.Name = "dgvColCredit";
            this.dgvColCredit.Visible = true;
            this.dgvColCredit.VisibleIndex = 4;
            this.dgvColCredit.Width = 90;
            // 
            // dgvColAccountName
            // 
            this.dgvColAccountName.Caption = "اسم العميل";
            this.dgvColAccountName.FieldName = "CustomerName";
            this.dgvColAccountName.Name = "dgvColAccountName";
            this.dgvColAccountName.Visible = true;
            this.dgvColAccountName.VisibleIndex = 2;
            this.dgvColAccountName.Width = 256;
            // 
            // txtCostCenterID
            // 
            this.txtCostCenterID.EnterMoveNextControl = true;
            this.txtCostCenterID.Location = new System.Drawing.Point(736, 160);
            this.txtCostCenterID.Name = "txtCostCenterID";
            this.txtCostCenterID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtCostCenterID.Size = new System.Drawing.Size(56, 20);
            this.txtCostCenterID.TabIndex = 823;
            this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
            // 
            // lblCostCenterName
            // 
            this.lblCostCenterName.Location = new System.Drawing.Point(790, 160);
            this.lblCostCenterName.Name = "lblCostCenterName";
            this.lblCostCenterName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCostCenterName.Properties.Appearance.Options.UseBackColor = true;
            this.lblCostCenterName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblCostCenterName.Size = new System.Drawing.Size(219, 20);
            this.lblCostCenterName.TabIndex = 829;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(652, 165);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(75, 14);
            this.Label7.TabIndex = 828;
            this.Label7.Tag = "Cost Center";
            this.Label7.Text = "مركز الـتكلـفـة";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(446, 128);
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
            this.Label2.Location = new System.Drawing.Point(442, 162);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 14);
            this.Label2.TabIndex = 827;
            this.Label2.Tag = "To Date";
            this.Label2.Text = "الى تـــــــــــاريخ";
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(533, 127);
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
            this.txtToDate.Location = new System.Drawing.Point(533, 160);
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
            this.labelControl1.Location = new System.Drawing.Point(23, 536);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(95, 14);
            this.labelControl1.TabIndex = 831;
            this.labelControl1.Tag = "Debit Account";
            this.labelControl1.Text = "حـســاب الـمـــديـن";
            // 
            // lblDebit
            // 
            this.lblDebit.Location = new System.Drawing.Point(128, 534);
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
            this.labelControl2.Location = new System.Drawing.Point(251, 536);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(69, 14);
            this.labelControl2.TabIndex = 833;
            this.labelControl2.Tag = "Debit Account";
            this.labelControl2.Text = "إجمالي الدائن";
            // 
            // lblCredit
            // 
            this.lblCredit.Location = new System.Drawing.Point(326, 534);
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
            this.labelControl3.Location = new System.Drawing.Point(519, 538);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 14);
            this.labelControl3.TabIndex = 835;
            this.labelControl3.Tag = "Debit Account";
            this.labelControl3.Text = "إجمالي الرصيد";
            // 
            // lblBalanceSum
            // 
            this.lblBalanceSum.Location = new System.Drawing.Point(597, 536);
            this.lblBalanceSum.Name = "lblBalanceSum";
            this.lblBalanceSum.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblBalanceSum.Properties.Appearance.Options.UseBackColor = true;
            this.lblBalanceSum.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblBalanceSum.Size = new System.Drawing.Size(116, 20);
            this.lblBalanceSum.TabIndex = 834;
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(1114, 146);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(72, 29);
            this.btnShow.TabIndex = 836;
            this.btnShow.Tag = "Show";
            this.btnShow.Text = "عــــــــرض";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(719, 538);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(68, 14);
            this.labelControl4.TabIndex = 838;
            this.labelControl4.Tag = "Debit Account";
            this.labelControl4.Text = "اسم الحساب";
            // 
            // lblBalanceType
            // 
            this.lblBalanceType.Location = new System.Drawing.Point(793, 537);
            this.lblBalanceType.Name = "lblBalanceType";
            this.lblBalanceType.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblBalanceType.Properties.Appearance.Options.UseBackColor = true;
            this.lblBalanceType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblBalanceType.Size = new System.Drawing.Size(299, 20);
            this.lblBalanceType.TabIndex = 839;
            // 
            // chkSupliar
            // 
            this.chkSupliar.EditValue = true;
            this.chkSupliar.EnterMoveNextControl = true;
            this.chkSupliar.Location = new System.Drawing.Point(1041, 162);
            this.chkSupliar.MenuManager = this.ribbonControl1;
            this.chkSupliar.Name = "chkSupliar";
            this.chkSupliar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSupliar.Properties.Appearance.Options.UseFont = true;
            this.chkSupliar.Properties.Caption = "موردين";
            this.chkSupliar.Size = new System.Drawing.Size(53, 19);
            this.chkSupliar.TabIndex = 844;
            this.chkSupliar.Tag = "Credit";
            // 
            // chkCustomer
            // 
            this.chkCustomer.EditValue = true;
            this.chkCustomer.EnterMoveNextControl = true;
            this.chkCustomer.Location = new System.Drawing.Point(1041, 128);
            this.chkCustomer.MenuManager = this.ribbonControl1;
            this.chkCustomer.Name = "chkCustomer";
            this.chkCustomer.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCustomer.Properties.Appearance.Options.UseFont = true;
            this.chkCustomer.Properties.Caption = "عملاء";
            this.chkCustomer.Size = new System.Drawing.Size(53, 19);
            this.chkCustomer.TabIndex = 843;
            this.chkCustomer.Tag = "Customer";
            // 
            // labelControl20
            // 
            this.labelControl20.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl20.Appearance.Options.UseFont = true;
            this.labelControl20.Location = new System.Drawing.Point(16, 164);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(78, 14);
            this.labelControl20.TabIndex = 848;
            this.labelControl20.Tag = "To Account";
            this.labelControl20.Text = "الى رقم حساب";
            // 
            // btnToAcountID
            // 
            this.btnToAcountID.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnToAcountID.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnToAcountID.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnToAcountID.ImageOptions.Image")));
            this.btnToAcountID.Location = new System.Drawing.Point(406, 160);
            this.btnToAcountID.Name = "btnToAcountID";
            this.btnToAcountID.Size = new System.Drawing.Size(25, 23);
            this.btnToAcountID.TabIndex = 847;
            this.btnToAcountID.Click += new System.EventHandler(this.btnToAcountID_Click);
            // 
            // lblToAccountID
            // 
            this.lblToAccountID.Location = new System.Drawing.Point(187, 162);
            this.lblToAccountID.Name = "lblToAccountID";
            this.lblToAccountID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblToAccountID.Properties.Appearance.Options.UseBackColor = true;
            this.lblToAccountID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblToAccountID.Size = new System.Drawing.Size(219, 20);
            this.lblToAccountID.TabIndex = 846;
            // 
            // txtToAccountID
            // 
            this.txtToAccountID.EnterMoveNextControl = true;
            this.txtToAccountID.Location = new System.Drawing.Point(97, 162);
            this.txtToAccountID.Name = "txtToAccountID";
            this.txtToAccountID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtToAccountID.Size = new System.Drawing.Size(92, 20);
            this.txtToAccountID.TabIndex = 845;
            this.txtToAccountID.Tag = "ImportantFieldGreaterThanZero";
            this.txtToAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.txtToAccountID_Validating);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(18, 128);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(75, 14);
            this.labelControl5.TabIndex = 852;
            this.labelControl5.Tag = "From Account";
            this.labelControl5.Text = "من رقم حساب";
            // 
            // btnFromAcountID
            // 
            this.btnFromAcountID.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFromAcountID.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnFromAcountID.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnFromAcountID.ImageOptions.Image")));
            this.btnFromAcountID.Location = new System.Drawing.Point(406, 125);
            this.btnFromAcountID.Name = "btnFromAcountID";
            this.btnFromAcountID.Size = new System.Drawing.Size(25, 23);
            this.btnFromAcountID.TabIndex = 851;
            this.btnFromAcountID.Click += new System.EventHandler(this.btnFromAcountID_Click);
            // 
            // lblFromAccountID
            // 
            this.lblFromAccountID.Location = new System.Drawing.Point(190, 126);
            this.lblFromAccountID.Name = "lblFromAccountID";
            this.lblFromAccountID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblFromAccountID.Properties.Appearance.Options.UseBackColor = true;
            this.lblFromAccountID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblFromAccountID.Size = new System.Drawing.Size(216, 20);
            this.lblFromAccountID.TabIndex = 850;
            // 
            // txtFromAccountID
            // 
            this.txtFromAccountID.EnterMoveNextControl = true;
            this.txtFromAccountID.Location = new System.Drawing.Point(99, 126);
            this.txtFromAccountID.Name = "txtFromAccountID";
            this.txtFromAccountID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFromAccountID.Size = new System.Drawing.Size(92, 20);
            this.txtFromAccountID.TabIndex = 849;
            this.txtFromAccountID.Tag = "ImportantFieldGreaterThanZero";
            this.txtFromAccountID.Validating += new System.ComponentModel.CancelEventHandler(this.txtFromAccountID_Validating);
            // 
            // lblParentAcountName
            // 
            this.lblParentAcountName.Location = new System.Drawing.Point(827, 125);
            this.lblParentAcountName.Name = "lblParentAcountName";
            this.lblParentAcountName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblParentAcountName.Properties.Appearance.Options.UseBackColor = true;
            this.lblParentAcountName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblParentAcountName.Size = new System.Drawing.Size(182, 20);
            this.lblParentAcountName.TabIndex = 854;
            // 
            // txtParentAcountID
            // 
            this.txtParentAcountID.EnterMoveNextControl = true;
            this.txtParentAcountID.Location = new System.Drawing.Point(736, 125);
            this.txtParentAcountID.Name = "txtParentAcountID";
            this.txtParentAcountID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtParentAcountID.Size = new System.Drawing.Size(92, 20);
            this.txtParentAcountID.TabIndex = 853;
            this.txtParentAcountID.Tag = "ImportantFieldGreaterThanZero";
            this.txtParentAcountID.EditValueChanged += new System.EventHandler(this.textEdit4_EditValueChanged);
            this.txtParentAcountID.Validating += new System.ComponentModel.CancelEventHandler(this.txtParentAcountID_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(652, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 14);
            this.label1.TabIndex = 857;
            this.label1.Tag = "Primary Account";
            this.label1.Text = "حساب رئيسي";
            // 
            // btnCostCenterSearch
            // 
            this.btnCostCenterSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCostCenterSearch.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnCostCenterSearch.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCostCenterSearch.ImageOptions.Image")));
            this.btnCostCenterSearch.Location = new System.Drawing.Point(1010, 160);
            this.btnCostCenterSearch.Name = "btnCostCenterSearch";
            this.btnCostCenterSearch.Size = new System.Drawing.Size(25, 23);
            this.btnCostCenterSearch.TabIndex = 863;
            this.btnCostCenterSearch.Click += new System.EventHandler(this.btnCostCenterSearch_Click);
            // 
            // btnParentAcountIDSerach
            // 
            this.btnParentAcountIDSerach.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnParentAcountIDSerach.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.btnParentAcountIDSerach.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnParentAcountIDSerach.ImageOptions.Image")));
            this.btnParentAcountIDSerach.Location = new System.Drawing.Point(1010, 123);
            this.btnParentAcountIDSerach.Name = "btnParentAcountIDSerach";
            this.btnParentAcountIDSerach.Size = new System.Drawing.Size(25, 23);
            this.btnParentAcountIDSerach.TabIndex = 864;
            this.btnParentAcountIDSerach.Click += new System.EventHandler(this.btnParentAcountIDSerach_Click);
            // 
            // frmSpecificAccountStatement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1210, 586);
            this.Controls.Add(this.btnParentAcountIDSerach);
            this.Controls.Add(this.btnCostCenterSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblParentAcountName);
            this.Controls.Add(this.txtParentAcountID);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.btnFromAcountID);
            this.Controls.Add(this.lblFromAccountID);
            this.Controls.Add(this.txtFromAccountID);
            this.Controls.Add(this.labelControl20);
            this.Controls.Add(this.btnToAcountID);
            this.Controls.Add(this.lblToAccountID);
            this.Controls.Add(this.txtToAccountID);
            this.Controls.Add(this.chkSupliar);
            this.Controls.Add(this.chkCustomer);
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
            this.Controls.Add(this.gridControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSpecificAccountStatement";
            this.Tag = "Specific AcountStatment";
            this.Text = "كشف حسابات مخصصة";
            this.Load += new System.EventHandler(this.frmAccountStatement_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSpecificAccountStatement_KeyDown);
            this.Controls.SetChildIndex(this.gridControl1, 0);
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
            this.Controls.SetChildIndex(this.chkCustomer, 0);
            this.Controls.SetChildIndex(this.chkSupliar, 0);
            this.Controls.SetChildIndex(this.txtToAccountID, 0);
            this.Controls.SetChildIndex(this.lblToAccountID, 0);
            this.Controls.SetChildIndex(this.btnToAcountID, 0);
            this.Controls.SetChildIndex(this.labelControl20, 0);
            this.Controls.SetChildIndex(this.txtFromAccountID, 0);
            this.Controls.SetChildIndex(this.lblFromAccountID, 0);
            this.Controls.SetChildIndex(this.btnFromAcountID, 0);
            this.Controls.SetChildIndex(this.labelControl5, 0);
            this.Controls.SetChildIndex(this.txtParentAcountID, 0);
            this.Controls.SetChildIndex(this.lblParentAcountName, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnCostCenterSearch, 0);
            this.Controls.SetChildIndex(this.btnParentAcountIDSerach, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.chkSupliar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToAccountID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToAccountID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFromAccountID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromAccountID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentAcountName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentAcountID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView1;
        private DevExpress.XtraEditors.TextEdit txtCostCenterID;
        private DevExpress.XtraEditors.TextEdit lblCostCenterName;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label Label2;
        private DevExpress.XtraEditors.DateEdit txtFromDate;
        private DevExpress.XtraEditors.DateEdit txtToDate;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit lblDebit;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit lblCredit;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit lblBalanceSum;
        private DevExpress.XtraEditors.SimpleButton btnShow;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColn_invoice_serial;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColBalance;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDebit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColCredit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColAccountName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColAccountID;
        private DevExpress.XtraEditors.TextEdit lblBalanceType;
        public DevExpress.XtraEditors.CheckEdit chkSupliar;
        public DevExpress.XtraEditors.CheckEdit chkCustomer;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.SimpleButton btnToAcountID;
        private DevExpress.XtraEditors.TextEdit lblToAccountID;
        private DevExpress.XtraEditors.TextEdit txtToAccountID;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnFromAcountID;
        private DevExpress.XtraEditors.TextEdit lblFromAccountID;
        private DevExpress.XtraEditors.TextEdit txtFromAccountID;
        private DevExpress.XtraEditors.TextEdit lblParentAcountName;
        private DevExpress.XtraEditors.TextEdit txtParentAcountID;
        internal System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnCostCenterSearch;
        private DevExpress.XtraEditors.SimpleButton btnParentAcountIDSerach;
    }
}

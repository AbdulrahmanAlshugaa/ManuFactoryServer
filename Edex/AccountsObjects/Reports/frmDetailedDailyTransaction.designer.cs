namespace Edex.AccountsObjects.Reports
{
    partial class frmDetailedDailyTransaction
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
            this.label5 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.txtToDate = new DevExpress.XtraEditors.DateEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.GridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvColID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTheDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDebit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColCredit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColPostPonedDebit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColPostPonedCredit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColNetDebit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColNetCredit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColChequeDebit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColChequeCredit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColVariousVoucher = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColAccountName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColOppsiteAccountName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColRecordType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDeclaration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColMethodeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTempRecordType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblBalanceSum = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lblCredit = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblDebit = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceSum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(1292, 116);
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(828, 136);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(119, 22);
            this.btnShow.TabIndex = 841;
            this.btnShow.Text = "عــــــــرض";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(323, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 14);
            this.label5.TabIndex = 839;
            this.label5.Tag = "Date";
            this.label5.Text = "من تـــــــــــاريخ";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(573, 140);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 14);
            this.Label2.TabIndex = 840;
            this.Label2.Tag = "Date";
            this.Label2.Text = "الى تـــــــــــاريخ";
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(407, 137);
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
            this.txtFromDate.TabIndex = 837;
            this.txtFromDate.EditValueChanged += new System.EventHandler(this.txtFromDate_EditValueChanged);
            // 
            // txtToDate
            // 
            this.txtToDate.EditValue = null;
            this.txtToDate.EnterMoveNextControl = true;
            this.txtToDate.Location = new System.Drawing.Point(660, 137);
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
            this.txtToDate.TabIndex = 838;
            this.txtToDate.EditValueChanged += new System.EventHandler(this.txtToDate_EditValueChanged);
            // 
            // panelControl1
            // 
            this.panelControl1.Location = new System.Drawing.Point(12, 122);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1268, 53);
            this.panelControl1.TabIndex = 1032;
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 181);
            this.gridControl1.MainView = this.GridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1268, 382);
            this.gridControl1.TabIndex = 1033;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // GridView1
            // 
            this.GridView1.ActiveFilterEnabled = false;
            this.GridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dgvColID,
            this.dgvColTheDate,
            this.dgvColDebit,
            this.dgvColCredit,
            this.dgvColPostPonedDebit,
            this.dgvColPostPonedCredit,
            this.dgvColNetDebit,
            this.dgvColNetCredit,
            this.dgvColChequeDebit,
            this.dgvColChequeCredit,
            this.dgvColVariousVoucher,
            this.dgvColAccountName,
            this.dgvColOppsiteAccountName,
            this.dgvColRecordType,
            this.dgvColDeclaration,
            this.dgvColMethodeID,
            this.dgvColTempRecordType,
            this.dgvColUserName});
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
            this.GridView1.DoubleClick += new System.EventHandler(this.GridView1_DoubleClick);
            // 
            // dgvColID
            // 
            this.dgvColID.Caption = "الــرقم";
            this.dgvColID.FieldName = "ID";
            this.dgvColID.Name = "dgvColID";
            this.dgvColID.Visible = true;
            this.dgvColID.VisibleIndex = 0;
            this.dgvColID.Width = 59;
            // 
            // dgvColTheDate
            // 
            this.dgvColTheDate.Caption = "التاريخ";
            this.dgvColTheDate.FieldName = "TheDate";
            this.dgvColTheDate.Name = "dgvColTheDate";
            this.dgvColTheDate.OptionsFilter.AllowAutoFilter = false;
            this.dgvColTheDate.OptionsFilter.AllowFilter = false;
            this.dgvColTheDate.Visible = true;
            this.dgvColTheDate.VisibleIndex = 1;
            this.dgvColTheDate.Width = 54;
            // 
            // dgvColDebit
            // 
            this.dgvColDebit.Caption = "مدين";
            this.dgvColDebit.FieldName = "Debit";
            this.dgvColDebit.Name = "dgvColDebit";
            this.dgvColDebit.OptionsFilter.AllowAutoFilter = false;
            this.dgvColDebit.OptionsFilter.AllowFilter = false;
            this.dgvColDebit.Visible = true;
            this.dgvColDebit.VisibleIndex = 2;
            this.dgvColDebit.Width = 57;
            // 
            // dgvColCredit
            // 
            this.dgvColCredit.Caption = "دائن";
            this.dgvColCredit.FieldName = "Credit";
            this.dgvColCredit.Name = "dgvColCredit";
            this.dgvColCredit.OptionsFilter.AllowAutoFilter = false;
            this.dgvColCredit.OptionsFilter.AllowFilter = false;
            this.dgvColCredit.Visible = true;
            this.dgvColCredit.VisibleIndex = 3;
            this.dgvColCredit.Width = 57;
            // 
            // dgvColPostPonedDebit
            // 
            this.dgvColPostPonedDebit.Caption = "الآجل مدين";
            this.dgvColPostPonedDebit.FieldName = "PostPonedDebit";
            this.dgvColPostPonedDebit.Name = "dgvColPostPonedDebit";
            this.dgvColPostPonedDebit.Visible = true;
            this.dgvColPostPonedDebit.VisibleIndex = 5;
            this.dgvColPostPonedDebit.Width = 47;
            // 
            // dgvColPostPonedCredit
            // 
            this.dgvColPostPonedCredit.Caption = "الآجل دائن";
            this.dgvColPostPonedCredit.FieldName = "PostPonedCredit";
            this.dgvColPostPonedCredit.Name = "dgvColPostPonedCredit";
            this.dgvColPostPonedCredit.UnboundExpression = "Postponed Credit";
            this.dgvColPostPonedCredit.Visible = true;
            this.dgvColPostPonedCredit.VisibleIndex = 4;
            this.dgvColPostPonedCredit.Width = 47;
            // 
            // dgvColNetDebit
            // 
            this.dgvColNetDebit.Caption = "شـبكة مدين";
            this.dgvColNetDebit.FieldName = "NetDebit";
            this.dgvColNetDebit.Name = "dgvColNetDebit";
            this.dgvColNetDebit.UnboundExpression = "Net Debit";
            this.dgvColNetDebit.Visible = true;
            this.dgvColNetDebit.VisibleIndex = 6;
            this.dgvColNetDebit.Width = 67;
            // 
            // dgvColNetCredit
            // 
            this.dgvColNetCredit.Caption = "شـبكة دائن";
            this.dgvColNetCredit.FieldName = "NetCredit";
            this.dgvColNetCredit.Name = "dgvColNetCredit";
            this.dgvColNetCredit.Tag = "Network Credit";
            this.dgvColNetCredit.Visible = true;
            this.dgvColNetCredit.VisibleIndex = 7;
            this.dgvColNetCredit.Width = 47;
            // 
            // dgvColChequeDebit
            // 
            this.dgvColChequeDebit.Caption = "شيـكات مدين";
            this.dgvColChequeDebit.FieldName = "ChequeDebit";
            this.dgvColChequeDebit.Name = "dgvColChequeDebit";
            this.dgvColChequeDebit.Tag = "Cheque Debit";
            this.dgvColChequeDebit.Visible = true;
            this.dgvColChequeDebit.VisibleIndex = 8;
            this.dgvColChequeDebit.Width = 47;
            // 
            // dgvColChequeCredit
            // 
            this.dgvColChequeCredit.Caption = "شيـكات دائن";
            this.dgvColChequeCredit.FieldName = "ChequeCredit";
            this.dgvColChequeCredit.Name = "dgvColChequeCredit";
            this.dgvColChequeCredit.Tag = "Cheque Credit";
            this.dgvColChequeCredit.Visible = true;
            this.dgvColChequeCredit.VisibleIndex = 9;
            this.dgvColChequeCredit.Width = 47;
            // 
            // dgvColVariousVoucher
            // 
            this.dgvColVariousVoucher.Caption = "القيود اليومية";
            this.dgvColVariousVoucher.FieldName = "VariousVoucher";
            this.dgvColVariousVoucher.Name = "dgvColVariousVoucher";
            this.dgvColVariousVoucher.Tag = "VariousVoucher";
            this.dgvColVariousVoucher.Visible = true;
            this.dgvColVariousVoucher.VisibleIndex = 10;
            this.dgvColVariousVoucher.Width = 47;
            // 
            // dgvColAccountName
            // 
            this.dgvColAccountName.Caption = "الحساب";
            this.dgvColAccountName.FieldName = "AccountName";
            this.dgvColAccountName.Name = "dgvColAccountName";
            this.dgvColAccountName.Tag = "Account Name";
            this.dgvColAccountName.Visible = true;
            this.dgvColAccountName.VisibleIndex = 11;
            this.dgvColAccountName.Width = 158;
            // 
            // dgvColOppsiteAccountName
            // 
            this.dgvColOppsiteAccountName.Caption = "الحساب المقابل";
            this.dgvColOppsiteAccountName.FieldName = "OppsiteAccountName";
            this.dgvColOppsiteAccountName.Name = "dgvColOppsiteAccountName";
            this.dgvColOppsiteAccountName.OptionsFilter.AllowAutoFilter = false;
            this.dgvColOppsiteAccountName.OptionsFilter.AllowFilter = false;
            this.dgvColOppsiteAccountName.Visible = true;
            this.dgvColOppsiteAccountName.VisibleIndex = 12;
            this.dgvColOppsiteAccountName.Width = 124;
            // 
            // dgvColRecordType
            // 
            this.dgvColRecordType.Caption = "نوع المستند";
            this.dgvColRecordType.FieldName = "RecordType";
            this.dgvColRecordType.Name = "dgvColRecordType";
            this.dgvColRecordType.OptionsFilter.AllowAutoFilter = false;
            this.dgvColRecordType.OptionsFilter.AllowFilter = false;
            this.dgvColRecordType.Visible = true;
            this.dgvColRecordType.VisibleIndex = 13;
            this.dgvColRecordType.Width = 73;
            // 
            // dgvColDeclaration
            // 
            this.dgvColDeclaration.Caption = "البيان";
            this.dgvColDeclaration.FieldName = "Declaration";
            this.dgvColDeclaration.Name = "dgvColDeclaration";
            this.dgvColDeclaration.OptionsFilter.AllowAutoFilter = false;
            this.dgvColDeclaration.OptionsFilter.AllowFilter = false;
            this.dgvColDeclaration.Visible = true;
            this.dgvColDeclaration.VisibleIndex = 14;
            this.dgvColDeclaration.Width = 65;
            // 
            // dgvColMethodeID
            // 
            this.dgvColMethodeID.Caption = "طريقة الدفع";
            this.dgvColMethodeID.FieldName = "MethodeID";
            this.dgvColMethodeID.Name = "dgvColMethodeID";
            this.dgvColMethodeID.Tag = "Methode ID";
            // 
            // dgvColTempRecordType
            // 
            this.dgvColTempRecordType.Caption = "النوع ";
            this.dgvColTempRecordType.FieldName = "TempRecordType";
            this.dgvColTempRecordType.Name = "dgvColTempRecordType";
            this.dgvColTempRecordType.OptionsFilter.AllowAutoFilter = false;
            this.dgvColTempRecordType.OptionsFilter.AllowFilter = false;
            this.dgvColTempRecordType.Tag = "Temp Record Type";
            this.dgvColTempRecordType.Width = 103;
            // 
            // dgvColUserName
            // 
            this.dgvColUserName.Caption = "المستخدم";
            this.dgvColUserName.FieldName = "UserName";
            this.dgvColUserName.Name = "dgvColUserName";
            this.dgvColUserName.Visible = true;
            this.dgvColUserName.VisibleIndex = 15;
            this.dgvColUserName.Width = 60;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(462, 591);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 14);
            this.labelControl3.TabIndex = 1039;
            this.labelControl3.Tag = "Debit Account";
            this.labelControl3.Text = "إجمالي الرصيد";
            // 
            // lblBalanceSum
            // 
            this.lblBalanceSum.Location = new System.Drawing.Point(540, 589);
            this.lblBalanceSum.Name = "lblBalanceSum";
            this.lblBalanceSum.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblBalanceSum.Properties.Appearance.Options.UseBackColor = true;
            this.lblBalanceSum.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblBalanceSum.Size = new System.Drawing.Size(116, 20);
            this.lblBalanceSum.TabIndex = 1038;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(265, 589);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(69, 14);
            this.labelControl2.TabIndex = 1037;
            this.labelControl2.Tag = "Debit Account";
            this.labelControl2.Text = "إجمالي الدائن";
            // 
            // lblCredit
            // 
            this.lblCredit.Location = new System.Drawing.Point(340, 587);
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCredit.Properties.Appearance.Options.UseBackColor = true;
            this.lblCredit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblCredit.Size = new System.Drawing.Size(116, 20);
            this.lblCredit.TabIndex = 1036;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(37, 589);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(95, 14);
            this.labelControl1.TabIndex = 1035;
            this.labelControl1.Tag = "Debit Account";
            this.labelControl1.Text = "حـســاب الـمـــديـن";
            // 
            // lblDebit
            // 
            this.lblDebit.Location = new System.Drawing.Point(142, 587);
            this.lblDebit.Name = "lblDebit";
            this.lblDebit.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDebit.Properties.Appearance.Options.UseBackColor = true;
            this.lblDebit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblDebit.Size = new System.Drawing.Size(116, 20);
            this.lblDebit.TabIndex = 1034;
            // 
            // frmDetailedDailyTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1292, 646);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.lblBalanceSum);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lblCredit);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lblDebit);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtFromDate);
            this.Controls.Add(this.txtToDate);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDetailedDailyTransaction";
            this.Tag = "Detailed Daily Transaction Form";
            this.Text = "الحـركة الـيـومية الـمفـصلة";
            this.Load += new System.EventHandler(this.frmDetailedDailyTransaction_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.txtToDate, 0);
            this.Controls.SetChildIndex(this.txtFromDate, 0);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            this.Controls.SetChildIndex(this.lblDebit, 0);
            this.Controls.SetChildIndex(this.labelControl1, 0);
            this.Controls.SetChildIndex(this.lblCredit, 0);
            this.Controls.SetChildIndex(this.labelControl2, 0);
            this.Controls.SetChildIndex(this.lblBalanceSum, 0);
            this.Controls.SetChildIndex(this.labelControl3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBalanceSum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnShow;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label Label2;
        private DevExpress.XtraEditors.DateEdit txtFromDate;
        private DevExpress.XtraEditors.DateEdit txtToDate;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView1;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDebit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColCredit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDeclaration;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTheDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColOppsiteAccountName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColRecordType;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTempRecordType;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColPostPonedDebit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColPostPonedCredit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColNetDebit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColNetCredit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColChequeDebit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColChequeCredit;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColVariousVoucher;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColAccountName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColUserName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColMethodeID;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        public DevExpress.XtraEditors.TextEdit lblBalanceSum;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit lblCredit;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit lblDebit;
    }
}

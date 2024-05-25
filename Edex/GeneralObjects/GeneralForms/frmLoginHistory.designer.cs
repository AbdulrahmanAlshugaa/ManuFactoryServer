namespace Edex.GeneralObjects.GeneralForms
{
    partial class frmLoginHistory
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
            this.Label3 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.lblBranchName = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtToDate = new DevExpress.XtraEditors.DateEdit();
            this.txtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.txtUserID = new DevExpress.XtraEditors.TextEdit();
            this.txtBranchID = new DevExpress.XtraEditors.TextEdit();
            this.txtPC = new DevExpress.XtraEditors.TextEdit();
            this.txtDB = new DevExpress.XtraEditors.TextEdit();
            this.cmbStatus = new DevExpress.XtraEditors.LookUpEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvColTheDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDB = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColBranch = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColPassword = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColPC = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(1053, 116);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(903, 131);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(50, 14);
            this.Label3.TabIndex = 296;
            this.Label3.Tag = "Status";
            this.Label3.Text = "الـحـــالـة";
            this.Label3.Click += new System.EventHandler(this.Label3_Click);
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(570, 164);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(75, 14);
            this.Label7.TabIndex = 294;
            this.Label7.Tag = "Data Base";
            this.Label7.Text = "قاعدة البيانات";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(570, 131);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(74, 14);
            this.Label6.TabIndex = 293;
            this.Label6.Tag = "PC Name";
            this.Label6.Text = "اسم الكمبيوتر";
            // 
            // lblBranchName
            // 
            this.lblBranchName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBranchName.Location = new System.Drawing.Point(358, 164);
            this.lblBranchName.Name = "lblBranchName";
            this.lblBranchName.Size = new System.Drawing.Size(137, 20);
            this.lblBranchName.TabIndex = 292;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(249, 167);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(55, 14);
            this.Label4.TabIndex = 291;
            this.Label4.Tag = "Branch";
            this.Label4.Text = "الــفــــــرع";
            // 
            // lblUserName
            // 
            this.lblUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUserName.Location = new System.Drawing.Point(358, 128);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(137, 20);
            this.lblUserName.TabIndex = 290;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(249, 131);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(59, 14);
            this.Label2.TabIndex = 289;
            this.Label2.Tag = "User";
            this.Label2.Text = "المستخدم";
            // 
            // btnShow
            // 
            this.btnShow.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Location = new System.Drawing.Point(967, 152);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(74, 33);
            this.btnShow.TabIndex = 288;
            this.btnShow.Tag = "Show";
            this.btnShow.Text = "عـــرض";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            this.Label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(12, 164);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(57, 14);
            this.Label5.TabIndex = 287;
            this.Label5.Tag = " To Date";
            this.Label5.Text = "الى تـاريـخ";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(12, 128);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(57, 14);
            this.Label1.TabIndex = 286;
            this.Label1.Tag = "From Date";
            this.Label1.Text = "مـن تـاريـخ";
            // 
            // txtToDate
            // 
            this.txtToDate.EditValue = null;
            this.txtToDate.EnterMoveNextControl = true;
            this.txtToDate.Location = new System.Drawing.Point(88, 161);
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
            this.txtToDate.TabIndex = 298;
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(88, 125);
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
            this.txtFromDate.TabIndex = 297;
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(314, 128);
            this.txtUserID.MenuManager = this.ribbonControl1;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtUserID.Size = new System.Drawing.Size(48, 20);
            this.txtUserID.TabIndex = 299;
            this.txtUserID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserID_KeyDown);
            // 
            // txtBranchID
            // 
            this.txtBranchID.Location = new System.Drawing.Point(310, 164);
            this.txtBranchID.MenuManager = this.ribbonControl1;
            this.txtBranchID.Name = "txtBranchID";
            this.txtBranchID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtBranchID.Size = new System.Drawing.Size(48, 20);
            this.txtBranchID.TabIndex = 300;
            this.txtBranchID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBranchID_KeyDown);
            // 
            // txtPC
            // 
            this.txtPC.Location = new System.Drawing.Point(651, 161);
            this.txtPC.MenuManager = this.ribbonControl1;
            this.txtPC.Name = "txtPC";
            this.txtPC.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtPC.Size = new System.Drawing.Size(233, 20);
            this.txtPC.TabIndex = 301;
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(650, 130);
            this.txtDB.MenuManager = this.ribbonControl1;
            this.txtDB.Name = "txtDB";
            this.txtDB.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtDB.Size = new System.Drawing.Size(234, 20);
            this.txtDB.TabIndex = 302;
            this.txtDB.EditValueChanged += new System.EventHandler(this.txtDB_EditValueChanged);
            // 
            // cmbStatus
            // 
            this.cmbStatus.Location = new System.Drawing.Point(959, 129);
            this.cmbStatus.MenuManager = this.ribbonControl1;
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbStatus.Size = new System.Drawing.Size(82, 20);
            this.cmbStatus.TabIndex = 303;
            // 
            // gridControl1
            // 
            gridLevelNode1.RelationName = "Level1";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl1.Location = new System.Drawing.Point(0, 190);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1050, 515);
            this.gridControl1.TabIndex = 304;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dgvColTheDate,
            this.dgvColTime,
            this.dgvColStatus,
            this.dgvColUserID,
            this.dgvColUserName,
            this.dgvColDB,
            this.dgvColBranch,
            this.dgvColPassword,
            this.dgvColPC});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // dgvColTheDate
            // 
            this.dgvColTheDate.Caption = "التاريخ";
            this.dgvColTheDate.FieldName = "RegDate";
            this.dgvColTheDate.Name = "dgvColTheDate";
            this.dgvColTheDate.Visible = true;
            this.dgvColTheDate.VisibleIndex = 0;
            // 
            // dgvColTime
            // 
            this.dgvColTime.Caption = "الوقت";
            this.dgvColTime.FieldName = "RegTime";
            this.dgvColTime.Name = "dgvColTime";
            this.dgvColTime.Visible = true;
            this.dgvColTime.VisibleIndex = 1;
            // 
            // dgvColStatus
            // 
            this.dgvColStatus.Caption = "الحالة";
            this.dgvColStatus.FieldName = "Status";
            this.dgvColStatus.Name = "dgvColStatus";
            this.dgvColStatus.Visible = true;
            this.dgvColStatus.VisibleIndex = 2;
            // 
            // dgvColUserID
            // 
            this.dgvColUserID.Caption = "رقم المستخدم";
            this.dgvColUserID.FieldName = "UserID";
            this.dgvColUserID.Name = "dgvColUserID";
            this.dgvColUserID.Visible = true;
            this.dgvColUserID.VisibleIndex = 3;
            // 
            // dgvColUserName
            // 
            this.dgvColUserName.Caption = "اسم المستخدم";
            this.dgvColUserName.FieldName = "UserName";
            this.dgvColUserName.Name = "dgvColUserName";
            this.dgvColUserName.Visible = true;
            this.dgvColUserName.VisibleIndex = 4;
            // 
            // dgvColDB
            // 
            this.dgvColDB.Caption = "قاعدة البيانات";
            this.dgvColDB.FieldName = "DBName";
            this.dgvColDB.Name = "dgvColDB";
            this.dgvColDB.Visible = true;
            this.dgvColDB.VisibleIndex = 5;
            // 
            // dgvColBranch
            // 
            this.dgvColBranch.Caption = "الفرع";
            this.dgvColBranch.FieldName = "BranchName";
            this.dgvColBranch.Name = "dgvColBranch";
            this.dgvColBranch.Visible = true;
            this.dgvColBranch.VisibleIndex = 6;
            // 
            // dgvColPassword
            // 
            this.dgvColPassword.Caption = "كلمة المرور";
            this.dgvColPassword.FieldName = "Password";
            this.dgvColPassword.Name = "dgvColPassword";
            this.dgvColPassword.Visible = true;
            this.dgvColPassword.VisibleIndex = 7;
            // 
            // dgvColPC
            // 
            this.dgvColPC.Caption = "اسم الكمبيوتر";
            this.dgvColPC.FieldName = "ComputerInfo";
            this.dgvColPC.Name = "dgvColPC";
            this.dgvColPC.Visible = true;
            this.dgvColPC.VisibleIndex = 8;
            // 
            // frmLoginHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1053, 738);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.txtDB);
            this.Controls.Add(this.txtPC);
            this.Controls.Add(this.txtBranchID);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.txtToDate);
            this.Controls.Add(this.txtFromDate);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.lblBranchName);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoginHistory";
            this.Text = "ارشيف تسجيل الدخول";
            this.Tag = "Login Archives ";
            this.Load += new System.EventHandler(this.frmLoginHistory_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLoginHistory_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmLoginHistory_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmLoginHistory_KeyUp);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.Label1, 0);
            this.Controls.SetChildIndex(this.Label5, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.lblUserName, 0);
            this.Controls.SetChildIndex(this.Label4, 0);
            this.Controls.SetChildIndex(this.lblBranchName, 0);
            this.Controls.SetChildIndex(this.Label6, 0);
            this.Controls.SetChildIndex(this.Label7, 0);
            this.Controls.SetChildIndex(this.Label3, 0);
            this.Controls.SetChildIndex(this.txtFromDate, 0);
            this.Controls.SetChildIndex(this.txtToDate, 0);
            this.Controls.SetChildIndex(this.txtUserID, 0);
            this.Controls.SetChildIndex(this.txtBranchID, 0);
            this.Controls.SetChildIndex(this.txtPC, 0);
            this.Controls.SetChildIndex(this.txtDB, 0);
            this.Controls.SetChildIndex(this.cmbStatus, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label lblBranchName;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label lblUserName;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button btnShow;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label1;
        private DevExpress.XtraEditors.DateEdit txtToDate;
        private DevExpress.XtraEditors.DateEdit txtFromDate;
        private DevExpress.XtraEditors.TextEdit txtUserID;
        private DevExpress.XtraEditors.TextEdit txtBranchID;
        private DevExpress.XtraEditors.TextEdit txtPC;
        private DevExpress.XtraEditors.TextEdit txtDB;
        private DevExpress.XtraEditors.LookUpEdit cmbStatus;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTheDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTime;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColStatus;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColUserID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColUserName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDB;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColBranch;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColPassword;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColPC;
    }
}

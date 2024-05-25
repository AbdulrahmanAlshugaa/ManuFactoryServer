namespace Edex.GeneralObjects.GeneralForms
{
    partial class frmNotifications
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
            this.Label2 = new System.Windows.Forms.Label();
            this.cmbDocType = new DevExpress.XtraEditors.LookUpEdit();
            this.btnShow = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.dgvColRecordType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColDeclaration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColNotificationDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColMeritDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColTempRecordType = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.pnlUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserCreatedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserUpdatedD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserCreatedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComputerInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserUpdatedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompoterEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateCreated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateUpdated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDocType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(974, 116);
            // 
            // pnlUsers
            // 
            this.pnlUsers.Location = new System.Drawing.Point(0, 448);
            this.pnlUsers.Size = new System.Drawing.Size(974, 51);
            // 
            // lblUserCreatedID
            // 
            this.lblUserCreatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserCreatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lbfUserUpdatedD
            // 
            this.lbfUserUpdatedD.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbfUserUpdatedD.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lbfUserCreatedID
            // 
            this.lbfUserCreatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbfUserCreatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblComputerInfo
            // 
            this.lblComputerInfo.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblComputerInfo.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserUpdatedID
            // 
            this.lblUserUpdatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserUpdatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblCompoterEdit
            // 
            this.lblCompoterEdit.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCompoterEdit.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserDateCreated
            // 
            this.lblUserDateCreated.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserDateCreated.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserDateUpdated
            // 
            this.lblUserDateUpdated.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserDateUpdated.Properties.Appearance.Options.UseBackColor = true;
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 499);
            this.ribbonStatusBar1.Size = new System.Drawing.Size(974, 27);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(29, 128);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(76, 14);
            this.Label2.TabIndex = 275;
            this.Label2.Tag = "Doc. Type";
            this.Label2.Text = "نـوع الـمسـتند";
            // 
            // cmbDocType
            // 
            this.cmbDocType.Location = new System.Drawing.Point(111, 126);
            this.cmbDocType.MenuManager = this.ribbonControl1;
            this.cmbDocType.Name = "cmbDocType";
            this.cmbDocType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDocType.Size = new System.Drawing.Size(150, 20);
            this.cmbDocType.TabIndex = 277;
            // 
            // btnShow
            // 
            this.btnShow.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnShow.Appearance.Options.UseFont = true;
            this.btnShow.Location = new System.Drawing.Point(276, 114);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(155, 41);
            this.btnShow.TabIndex = 278;
            this.btnShow.Tag = "Show";
            this.btnShow.Text = "عــرض";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(-2, 158);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.ribbonControl1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl1.Size = new System.Drawing.Size(974, 288);
            this.gridControl1.TabIndex = 279;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gridView1.Appearance.Row.Options.UseBackColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.dgvColRecordType,
            this.dgvColID,
            this.dgvColDeclaration,
            this.dgvColNotificationDate,
            this.dgvColMeritDate,
            this.dgvColTempRecordType});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridColumn1.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn1.Caption = "الغاء الشعار";
            this.gridColumn1.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn1.Name = "gridColumn1";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // dgvColRecordType
            // 
            this.dgvColRecordType.Caption = "نوع الحركة";
            this.dgvColRecordType.FieldName = "RecordType";
            this.dgvColRecordType.Name = "dgvColRecordType";
            this.dgvColRecordType.Visible = true;
            this.dgvColRecordType.VisibleIndex = 0;
            // 
            // dgvColID
            // 
            this.dgvColID.Caption = "رقم الحركة";
            this.dgvColID.FieldName = "ID";
            this.dgvColID.Name = "dgvColID";
            this.dgvColID.Visible = true;
            this.dgvColID.VisibleIndex = 1;
            // 
            // dgvColDeclaration
            // 
            this.dgvColDeclaration.Caption = "البيان";
            this.dgvColDeclaration.FieldName = "Declaration";
            this.dgvColDeclaration.Name = "dgvColDeclaration";
            this.dgvColDeclaration.Visible = true;
            this.dgvColDeclaration.VisibleIndex = 2;
            // 
            // dgvColNotificationDate
            // 
            this.dgvColNotificationDate.Caption = "تاريخ الإشعار";
            this.dgvColNotificationDate.FieldName = "NotificationDate";
            this.dgvColNotificationDate.Name = "dgvColNotificationDate";
            this.dgvColNotificationDate.Visible = true;
            this.dgvColNotificationDate.VisibleIndex = 3;
            // 
            // dgvColMeritDate
            // 
            this.dgvColMeritDate.Caption = "تاريخ الإستحقاق";
            this.dgvColMeritDate.FieldName = "MeritDate";
            this.dgvColMeritDate.Name = "dgvColMeritDate";
            this.dgvColMeritDate.Visible = true;
            this.dgvColMeritDate.VisibleIndex = 4;
            // 
            // dgvColTempRecordType
            // 
            this.dgvColTempRecordType.Caption = "RecordType";
            this.dgvColTempRecordType.FieldName = "RecordType";
            this.dgvColTempRecordType.Name = "dgvColTempRecordType";
            // 
            // frmNotifications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(974, 526);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.cmbDocType);
            this.Controls.Add(this.Label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNotifications";
            this.Tag = "Notifications";
            this.Text = "الاشعارات";
            this.Load += new System.EventHandler(this.frmNotifications_Load);
            this.Controls.SetChildIndex(this.ribbonStatusBar1, 0);
            this.Controls.SetChildIndex(this.pnlUsers, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.cmbDocType, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.pnlUsers.ResumeLayout(false);
            this.pnlUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserCreatedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserUpdatedD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserCreatedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComputerInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserUpdatedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompoterEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateCreated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateUpdated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDocType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColRecordType;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColDeclaration;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColNotificationDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColMeritDate;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColTempRecordType;
        public DevExpress.XtraEditors.SimpleButton btnShow;
        public DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        public DevExpress.XtraEditors.LookUpEdit cmbDocType;
    }
}

namespace Edex.RestaurantSystem.Transactions
{
    partial class frmAddressCustomer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddressCustomer));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.gridControlAddress = new DevExpress.XtraGrid.GridControl();
            this.gridViewAddress = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblCustomerName = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(-1, 0);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(41, 29);
            this.simpleButton1.TabIndex = 14;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // gridControlAddress
            // 
            gridLevelNode1.RelationName = "Level1";
            this.gridControlAddress.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControlAddress.Location = new System.Drawing.Point(-1, 31);
            this.gridControlAddress.MainView = this.gridViewAddress;
            this.gridControlAddress.Name = "gridControlAddress";
            this.gridControlAddress.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
            this.gridControlAddress.Size = new System.Drawing.Size(668, 227);
            this.gridControlAddress.TabIndex = 363;
            this.gridControlAddress.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAddress});
            // 
            // gridViewAddress
            // 
            this.gridViewAddress.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gridViewAddress.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn5});
            this.gridViewAddress.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewAddress.GridControl = this.gridControlAddress;
            this.gridViewAddress.Name = "gridViewAddress";
            this.gridViewAddress.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewAddress.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewAddress.OptionsBehavior.Editable = false;
            this.gridViewAddress.OptionsBehavior.ReadOnly = true;
            this.gridViewAddress.OptionsFind.FindNullPrompt = " ... أدخل النص للبحث";
            this.gridViewAddress.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridViewAddress.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewAddress.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewAddress.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridViewAddress.OptionsView.ShowFooter = true;
            this.gridViewAddress.RowHeight = 40;
            this.gridViewAddress.RowSeparatorHeight = 2;
            this.gridViewAddress.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridViewAddress_RowClick);
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "اسم الحي ";
            this.gridColumn4.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.gridColumn4.FieldName = "Location";
            this.gridColumn4.MinWidth = 17;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 64;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "اسم المكان عربي";
            this.gridColumn1.FieldName = "ArbName";
            this.gridColumn1.MinWidth = 17;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 64;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "اسم المكان انجليزي ";
            this.gridColumn2.FieldName = "EngName";
            this.gridColumn2.MinWidth = 17;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 64;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "رقم العميل ";
            this.gridColumn3.FieldName = "ID";
            this.gridColumn3.MinWidth = 17;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Width = 64;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "التكلفة";
            this.gridColumn5.FieldName = "TransCost";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Appearance.Font = new System.Drawing.Font("Nahdi", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(96)))), ((int)(((byte)(147)))));
            this.lblCustomerName.Appearance.Options.UseFont = true;
            this.lblCustomerName.Appearance.Options.UseForeColor = true;
            this.lblCustomerName.Location = new System.Drawing.Point(474, 8);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(0, 17);
            this.lblCustomerName.TabIndex = 364;
            // 
            // frmAddressCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 261);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.gridControlAddress);
            this.Controls.Add(this.simpleButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAddressCustomer";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAddressCustomer";
            this.Load += new System.EventHandler(this.frmAddressCustomer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        public DevExpress.XtraGrid.GridControl gridControlAddress;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        public DevExpress.XtraGrid.Views.Grid.GridView gridViewAddress;
        private DevExpress.XtraEditors.LabelControl lblCustomerName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}
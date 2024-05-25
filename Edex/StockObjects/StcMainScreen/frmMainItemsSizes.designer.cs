namespace Edex.StockObjects.StcMainScreen
{
    partial class frmMainItemsSizes
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainItemsSizes));
            this.GridControl = new DevExpress.XtraGrid.GridControl();
            this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dgvColSizeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColArbName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvColEngName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dgvcolUnitDelete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.dgvColShowRecord = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.repositoryItemPictureEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.repositoryItemImageEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemButtonEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemHyperLinkEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.repositoryItemToggleSwitch2 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtToItemNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtFromItemNo = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToItemNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromItemNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(897, 116);
            // 
            // GridControl
            // 
            gridLevelNode1.RelationName = "Level1";
            gridLevelNode2.RelationName = "Level2";
            this.GridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2});
            this.GridControl.Location = new System.Drawing.Point(0, 148);
            this.GridControl.MainView = this.GridView;
            this.GridControl.Name = "GridControl";
            this.GridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemPictureEdit2,
            this.repositoryItemImageEdit2,
            this.repositoryItemCheckEdit2,
            this.repositoryItemButtonEdit2,
            this.repositoryItemHyperLinkEdit5,
            this.repositoryItemHyperLinkEdit4,
            this.repositoryItemHyperLinkEdit6,
            this.repositoryItemToggleSwitch2});
            this.GridControl.Size = new System.Drawing.Size(897, 323);
            this.GridControl.TabIndex = 187;
            this.GridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView});
            // 
            // GridView
            // 
            this.GridView.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.GridView.Appearance.SelectedRow.Options.UseBackColor = true;
            this.GridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.GridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.dgvColSizeID,
            this.dgvColArbName,
            this.dgvColEngName,
            this.dgvcolUnitDelete,
            this.dgvColShowRecord});
            this.GridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.GridView.GridControl = this.GridControl;
            this.GridView.Name = "GridView";
            this.GridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView.OptionsBehavior.Editable = false;
            this.GridView.OptionsFind.AlwaysVisible = true;
            this.GridView.OptionsFind.FindNullPrompt = " ... أدخل النص للبحث";
            this.GridView.OptionsNavigation.EnterMoveNextColumn = true;
            this.GridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.GridView.OptionsView.EnableAppearanceEvenRow = true;
            this.GridView.OptionsView.EnableAppearanceOddRow = true;
            this.GridView.OptionsView.ShowFooter = true;
            this.GridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GridView_FocusedRowChanged);
            this.GridView.Click += new System.EventHandler(this.GridView_Click);
            this.GridView.DoubleClick += new System.EventHandler(this.GridView_DoubleClick);
            // 
            // dgvColSizeID
            // 
            this.dgvColSizeID.Caption = "الــرقم";
            this.dgvColSizeID.FieldName = "SizeID";
            this.dgvColSizeID.Name = "dgvColSizeID";
            this.dgvColSizeID.OptionsColumn.AllowEdit = false;
            this.dgvColSizeID.OptionsColumn.AllowMove = false;
            this.dgvColSizeID.OptionsColumn.AllowSize = false;
            this.dgvColSizeID.OptionsColumn.ReadOnly = true;
            this.dgvColSizeID.Visible = true;
            this.dgvColSizeID.VisibleIndex = 0;
            // 
            // dgvColArbName
            // 
            this.dgvColArbName.Caption = "الاسم باللغة العربية";
            this.dgvColArbName.FieldName = "ArbName";
            this.dgvColArbName.Name = "dgvColArbName";
            this.dgvColArbName.Visible = true;
            this.dgvColArbName.VisibleIndex = 1;
            // 
            // dgvColEngName
            // 
            this.dgvColEngName.Caption = "الاسم باللغة الأنجليزية";
            this.dgvColEngName.FieldName = "EngName";
            this.dgvColEngName.Name = "dgvColEngName";
            this.dgvColEngName.Visible = true;
            this.dgvColEngName.VisibleIndex = 2;
            // 
            // dgvcolUnitDelete
            // 
            this.dgvcolUnitDelete.Caption = "جــذف";
            this.dgvcolUnitDelete.ColumnEdit = this.repositoryItemHyperLinkEdit5;
            this.dgvcolUnitDelete.FieldName = "Delete";
            this.dgvcolUnitDelete.Name = "dgvcolUnitDelete";
            this.dgvcolUnitDelete.Visible = true;
            this.dgvcolUnitDelete.VisibleIndex = 3;
            // 
            // repositoryItemHyperLinkEdit5
            // 
            this.repositoryItemHyperLinkEdit5.AutoHeight = false;
           
            this.repositoryItemHyperLinkEdit5.ImageAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.repositoryItemHyperLinkEdit5.Name = "repositoryItemHyperLinkEdit5";
            // 
            // dgvColShowRecord
            // 
            this.dgvColShowRecord.Caption = "عرض";
            this.dgvColShowRecord.ColumnEdit = this.repositoryItemHyperLinkEdit6;
            this.dgvColShowRecord.FieldName = "ShowRecord";
            this.dgvColShowRecord.Name = "dgvColShowRecord";
            this.dgvColShowRecord.Visible = true;
            this.dgvColShowRecord.VisibleIndex = 4;
            // 
            // repositoryItemHyperLinkEdit6
            // 
            this.repositoryItemHyperLinkEdit6.AutoHeight = false;
        
            this.repositoryItemHyperLinkEdit6.ImageAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.repositoryItemHyperLinkEdit6.Name = "repositoryItemHyperLinkEdit6";
            // 
            // repositoryItemPictureEdit2
            // 
            this.repositoryItemPictureEdit2.Name = "repositoryItemPictureEdit2";
            // 
            // repositoryItemImageEdit2
            // 
            this.repositoryItemImageEdit2.Appearance.Options.UseImage = true;
            this.repositoryItemImageEdit2.AutoHeight = false;
            this.repositoryItemImageEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageEdit2.Name = "repositoryItemImageEdit2";
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            this.repositoryItemCheckEdit2.ValueChecked = 1;
            // 
            // repositoryItemButtonEdit2
            // 
         

            this.repositoryItemButtonEdit2.Appearance.Options.UseImage = true;
            this.repositoryItemButtonEdit2.AutoHeight = false;
            this.repositoryItemButtonEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit2.Name = "repositoryItemButtonEdit2";
            // 
            // repositoryItemHyperLinkEdit4
            // 
            this.repositoryItemHyperLinkEdit4.AutoHeight = false;
         

            this.repositoryItemHyperLinkEdit4.ImageAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.repositoryItemHyperLinkEdit4.Name = "repositoryItemHyperLinkEdit4";
            // 
            // repositoryItemToggleSwitch2
            // 
            this.repositoryItemToggleSwitch2.AutoHeight = false;
            this.repositoryItemToggleSwitch2.Name = "repositoryItemToggleSwitch2";
            this.repositoryItemToggleSwitch2.OffText = "Off";
            this.repositoryItemToggleSwitch2.OnText = "On";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(132, 125);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(39, 14);
            this.labelControl1.TabIndex = 814;
            this.labelControl1.Tag = "Supplier";
            this.labelControl1.Text = "الى رقم";
            // 
            // txtToItemNo
            // 
            this.txtToItemNo.EnterMoveNextControl = true;
            this.txtToItemNo.Location = new System.Drawing.Point(177, 122);
            this.txtToItemNo.MenuManager = this.ribbonControl1;
            this.txtToItemNo.Name = "txtToItemNo";
            this.txtToItemNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtToItemNo.Size = new System.Drawing.Size(75, 20);
            this.txtToItemNo.TabIndex = 813;
            this.txtToItemNo.Tag = "ImportantField";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(3, 126);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 812;
            this.labelControl6.Tag = "Supplier";
            this.labelControl6.Text = "من رقم";
            // 
            // txtFromItemNo
            // 
            this.txtFromItemNo.EnterMoveNextControl = true;
            this.txtFromItemNo.Location = new System.Drawing.Point(45, 122);
            this.txtFromItemNo.MenuManager = this.ribbonControl1;
            this.txtFromItemNo.Name = "txtFromItemNo";
            this.txtFromItemNo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFromItemNo.Size = new System.Drawing.Size(81, 20);
            this.txtFromItemNo.TabIndex = 811;
            this.txtFromItemNo.Tag = "ImportantField";
            // 
            // frmMainItemsSizes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(897, 495);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtToItemNo);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.txtFromItemNo);
            this.Controls.Add(this.GridControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMainItemsSizes";
            this.Text = "قياسات الاصناف ";
            this.Load += new System.EventHandler(this.frmMainStores_Load);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.GridControl, 0);
            this.Controls.SetChildIndex(this.txtFromItemNo, 0);
            this.Controls.SetChildIndex(this.labelControl6, 0);
            this.Controls.SetChildIndex(this.txtToItemNo, 0);
            this.Controls.SetChildIndex(this.labelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToItemNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromItemNo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl GridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColSizeID;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColArbName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColEngName;
        private DevExpress.XtraGrid.Columns.GridColumn dgvcolUnitDelete;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit5;
        private DevExpress.XtraGrid.Columns.GridColumn dgvColShowRecord;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit6;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageEdit repositoryItemImageEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit4;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtToItemNo;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txtFromItemNo;
    }
}

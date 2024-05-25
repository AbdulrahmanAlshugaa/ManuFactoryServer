namespace Edex.AccountsObjects.Codes
{
    partial class frmDeclaringFixedSpends
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
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateUpdated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateCreated.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompoterEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserUpdatedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComputerInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserCreatedID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserUpdatedD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserCreatedID.Properties)).BeginInit();
            this.pnlUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(6);
            this.ribbonControl1.Size = new System.Drawing.Size(724, 116);
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 529);
            this.ribbonStatusBar1.Size = new System.Drawing.Size(724, 27);
            // 
            // lblUserDateUpdated
            // 
            this.lblUserDateUpdated.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserDateUpdated.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserDateCreated
            // 
            this.lblUserDateCreated.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserDateCreated.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblCompoterEdit
            // 
            this.lblCompoterEdit.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCompoterEdit.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserUpdatedID
            // 
            this.lblUserUpdatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserUpdatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblComputerInfo
            // 
            this.lblComputerInfo.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblComputerInfo.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lbfUserCreatedID
            // 
            this.lbfUserCreatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbfUserCreatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lbfUserUpdatedD
            // 
            this.lbfUserUpdatedD.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbfUserUpdatedD.Properties.Appearance.Options.UseBackColor = true;
            // 
            // lblUserCreatedID
            // 
            this.lblUserCreatedID.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserCreatedID.Properties.Appearance.Options.UseBackColor = true;
            // 
            // pnlUsers
            // 
            this.pnlUsers.Location = new System.Drawing.Point(0, 478);
            this.pnlUsers.Size = new System.Drawing.Size(724, 51);
            // 
            // gridControl
            // 
            gridLevelNode1.RelationName = "Level1";
            this.gridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl.Location = new System.Drawing.Point(12, 125);
            this.gridControl.MainView = this.gridView2;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(700, 347);
            this.gridControl.TabIndex = 1048;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gridView2.GridControl = this.gridControl;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView2.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView2.OptionsCustomization.AllowFilter = false;
            this.gridView2.OptionsCustomization.AllowGroup = false;
            this.gridView2.OptionsCustomization.AllowSort = false;
            this.gridView2.OptionsFind.FindNullPrompt = " ... أدخل النص للبحث";
            this.gridView2.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView2.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView2.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView2.OptionsView.EnableAppearanceOddRow = true;
            this.gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridView2.OptionsView.ShowFooter = true;
            this.gridView2.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView2_ValidatingEditor);
            // 
            // frmDeclaringFixedSpends
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(724, 556);
            this.Controls.Add(this.gridControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDeclaringFixedSpends";
            this.Text = "تعريف حسابات المصاريف الثابتة";
            this.Load += new System.EventHandler(this.frmDeclaringMainAccounts_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDeclaringMainAccounts_KeyDown);
            this.Controls.SetChildIndex(this.ribbonStatusBar1, 0);
            this.Controls.SetChildIndex(this.pnlUsers, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.gridControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateUpdated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserDateCreated.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompoterEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserUpdatedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComputerInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserCreatedID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbfUserUpdatedD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserCreatedID.Properties)).EndInit();
            this.pnlUsers.ResumeLayout(false);
            this.pnlUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    }
}

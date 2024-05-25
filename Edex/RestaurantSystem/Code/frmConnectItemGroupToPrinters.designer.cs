namespace Edex.RestaurantSystem.Code
{
    partial class frmConnectItemGroupToPrinters
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
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ribbonControl1.Size = new System.Drawing.Size(928, 116);
            // 
            // pnlUsers
            // 
            this.pnlUsers.Controls.Add(this.btnSave);
            this.pnlUsers.Location = new System.Drawing.Point(0, 483);
            this.pnlUsers.Size = new System.Drawing.Size(928, 51);
            this.pnlUsers.Visible = false;
            this.pnlUsers.Controls.SetChildIndex(this.lblUserDateUpdated, 0);
            this.pnlUsers.Controls.SetChildIndex(this.lblUserDateCreated, 0);
            this.pnlUsers.Controls.SetChildIndex(this.lblCompoterEdit, 0);
            this.pnlUsers.Controls.SetChildIndex(this.lblUserUpdatedID, 0);
            this.pnlUsers.Controls.SetChildIndex(this.lblComputerInfo, 0);
            this.pnlUsers.Controls.SetChildIndex(this.lbfUserCreatedID, 0);
            this.pnlUsers.Controls.SetChildIndex(this.lbfUserUpdatedD, 0);
            this.pnlUsers.Controls.SetChildIndex(this.lblUserCreatedID, 0);
            this.pnlUsers.Controls.SetChildIndex(this.btnSave, 0);
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
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 534);
            this.ribbonStatusBar1.Size = new System.Drawing.Size(928, 27);
            // 
            // gridControl
            // 
            gridLevelNode1.RelationName = "Level1";
            gridLevelNode2.RelationName = "Level2";
            this.gridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2});
            this.gridControl.Location = new System.Drawing.Point(21, 114);
            this.gridControl.MainView = this.gridView1;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(885, 388);
            this.gridControl.TabIndex = 21;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gridView1.GridControl = this.gridControl;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsFind.FindNullPrompt = " ... أدخل النص للبحث";
            this.gridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView1.OptionsView.EnableAppearanceOddRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(411, 22);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(162, 27);
            this.btnSave.TabIndex = 22;
            this.btnSave.Tag = "Save";
            this.btnSave.Text = "حفـــــــظ";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmConnectItemGroupToPrinters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(928, 561);
            this.Controls.Add(this.gridControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConnectItemGroupToPrinters";
            this.Controls.SetChildIndex(this.ribbonStatusBar1, 0);
            this.Controls.SetChildIndex(this.pnlUsers, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.gridControl, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
    }
}

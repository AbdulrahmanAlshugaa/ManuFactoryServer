namespace Edex.GeneralObjects.GeneralForms
{
    partial class frmScreenSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScreenSearch));
            this.btnSearch = new System.Windows.Forms.Button();
            this.treeList2 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.txtSerchInTree = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSerchInTree.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(24, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(73, 33);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "......";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // treeList2
            // 
            this.treeList2.Appearance.Row.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeList2.Appearance.Row.Options.UseFont = true;
            this.treeList2.Caption = "بحث عن شاشة";
            this.treeList2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.treeList2.CustomizationFormBounds = new System.Drawing.Rectangle(626, 276, 260, 232);
            this.treeList2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeList2.Location = new System.Drawing.Point(5, 46);
            this.treeList2.Name = "treeList2";
            this.treeList2.OptionsBehavior.Editable = false;
            this.treeList2.OptionsBehavior.ReadOnly = true;
            this.treeList2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.treeList2.Size = new System.Drawing.Size(454, 502);
            this.treeList2.TabIndex = 334;
            this.treeList2.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList2_FocusedNodeChanged);
            this.treeList2.DoubleClick += new System.EventHandler(this.treeList2_DoubleClick);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "القائمة الفرعية";
            this.treeListColumn2.FieldName = "AcountName";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            // 
            // txtSerchInTree
            // 
            this.txtSerchInTree.EnterMoveNextControl = true;
            this.txtSerchInTree.Location = new System.Drawing.Point(105, 8);
            this.txtSerchInTree.Name = "txtSerchInTree";
            this.txtSerchInTree.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerchInTree.Properties.Appearance.Options.UseFont = true;
            this.txtSerchInTree.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSerchInTree.Properties.Mask.EditMask = "f0";
            this.txtSerchInTree.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSerchInTree.Size = new System.Drawing.Size(350, 30);
            this.txtSerchInTree.TabIndex = 339;
            this.txtSerchInTree.Tag = "ImportantFieldGreaterThanZero";
            this.txtSerchInTree.EditValueChanged += new System.EventHandler(this.txtSerchInTree_EditValueChanged);
            this.txtSerchInTree.Validated += new System.EventHandler(this.txtSerchInTree_Validated);
            // 
            // frmScreenSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 549);
            this.Controls.Add(this.txtSerchInTree);
            this.Controls.Add(this.treeList2);
            this.Controls.Add(this.btnSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScreenSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "Search for screen";
            this.Text = "بحث عن شاشة";
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSerchInTree.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private DevExpress.XtraTreeList.TreeList treeList2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        public DevExpress.XtraEditors.TextEdit txtSerchInTree;
    }
}
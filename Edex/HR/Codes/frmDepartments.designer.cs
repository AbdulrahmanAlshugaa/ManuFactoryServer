
namespace Edex.HR.Codes
{
    partial class frmDepartments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDepartments));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtID = new DevExpress.XtraEditors.TextEdit();
            this.txtNotes = new DevExpress.XtraEditors.TextEdit();
            this.txtEngName = new DevExpress.XtraEditors.TextEdit();
            this.txtArbName = new DevExpress.XtraEditors.TextEdit();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNotes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEngName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArbName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(892, 116);
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 387);
            this.ribbonStatusBar1.Size = new System.Drawing.Size(892, 27);
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
            this.pnlUsers.Size = new System.Drawing.Size(892, 51);
            // 
            // gridControl1
            // 
            gridLevelNode1.RelationName = "Level1";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl1.Location = new System.Drawing.Point(353, 126);
            this.gridControl1.MainView = this.GridView;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(535, 255);
            this.gridControl1.TabIndex = 1065;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView});
            // 
            // GridView
            // 
            this.GridView.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.GridView.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridView.Appearance.SelectedRow.Options.UseBackColor = true;
            this.GridView.Appearance.SelectedRow.Options.UseFont = true;
            this.GridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.GridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.GridView.GridControl = this.gridControl1;
            this.GridView.Name = "GridView";
            this.GridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.GridView.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.GridView.OptionsBehavior.Editable = false;
            this.GridView.OptionsBehavior.ReadOnly = true;
            this.GridView.ViewCaption = "وحدات المواد";
            // 
            // txtID
            // 
            this.txtID.EnterMoveNextControl = true;
            this.txtID.Location = new System.Drawing.Point(120, 122);
            this.txtID.Name = "txtID";
            this.txtID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtID.Properties.Mask.EditMask = "f0";
            this.txtID.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtID.Size = new System.Drawing.Size(96, 20);
            this.txtID.TabIndex = 1057;
            this.txtID.Tag = "ImportantFieldGreaterThanZero";
            this.txtID.Validating += new System.ComponentModel.CancelEventHandler(this.txtTypeID_Validating);
            // 
            // txtNotes
            // 
            this.txtNotes.EnterMoveNextControl = true;
            this.txtNotes.Location = new System.Drawing.Point(120, 202);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtNotes.Size = new System.Drawing.Size(231, 20);
            this.txtNotes.TabIndex = 1060;
            // 
            // txtEngName
            // 
            this.txtEngName.EnterMoveNextControl = true;
            this.txtEngName.Location = new System.Drawing.Point(120, 174);
            this.txtEngName.Name = "txtEngName";
            this.txtEngName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtEngName.Size = new System.Drawing.Size(231, 20);
            this.txtEngName.TabIndex = 1059;
            this.txtEngName.Validating += new System.ComponentModel.CancelEventHandler(this.txtEngName_Validating);
            // 
            // txtArbName
            // 
            this.txtArbName.EnterMoveNextControl = true;
            this.txtArbName.Location = new System.Drawing.Point(120, 148);
            this.txtArbName.Name = "txtArbName";
            this.txtArbName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtArbName.Size = new System.Drawing.Size(231, 20);
            this.txtArbName.TabIndex = 1058;
            this.txtArbName.Tag = "ImportantField";
            this.txtArbName.EditValueChanged += new System.EventHandler(this.txtArbName_EditValueChanged);
            this.txtArbName.Validating += new System.ComponentModel.CancelEventHandler(this.txtArbName_Validating);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(14, 204);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(82, 14);
            this.Label1.TabIndex = 1064;
            this.Label1.Tag = "Notes";
            this.Label1.Text = "مـلاحـظــــــــات";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(13, 150);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(83, 14);
            this.Label3.TabIndex = 1063;
            this.Label3.Tag = "Arabic Name";
            this.Label3.Text = "الاسـم بالعـربي";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(13, 176);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(92, 14);
            this.Label6.TabIndex = 1062;
            this.Label6.Tag = "English Name";
            this.Label6.Text = "الاسـم بالإنجليزي";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(14, 123);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(46, 14);
            this.Label4.TabIndex = 1061;
            this.Label4.Tag = "Type ID";
            this.Label4.Text = "الـــــرقم";
            // 
            // frmDepartments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 465);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.txtEngName);
            this.Controls.Add(this.txtArbName);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDepartments";
            this.Tag = "Departments";
            this.Text = "الاقـســــام";
            this.Load += new System.EventHandler(this.frmAdministrations_Load);
            this.Controls.SetChildIndex(this.pnlUsers, 0);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.ribbonStatusBar1, 0);
            this.Controls.SetChildIndex(this.Label4, 0);
            this.Controls.SetChildIndex(this.Label6, 0);
            this.Controls.SetChildIndex(this.Label3, 0);
            this.Controls.SetChildIndex(this.Label1, 0);
            this.Controls.SetChildIndex(this.txtArbName, 0);
            this.Controls.SetChildIndex(this.txtEngName, 0);
            this.Controls.SetChildIndex(this.txtNotes, 0);
            this.Controls.SetChildIndex(this.txtID, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNotes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEngName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArbName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        public DevExpress.XtraEditors.TextEdit txtID;
        private DevExpress.XtraEditors.TextEdit txtNotes;
        public DevExpress.XtraEditors.TextEdit txtEngName;
        public DevExpress.XtraEditors.TextEdit txtArbName;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label4;
    }
}

namespace Edex.SalesAndPurchaseObjects.Codes
{
    partial class frmResDriver
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new DevExpress.XtraEditors.TextEdit();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblFax = new System.Windows.Forms.Label();
            this.lblMobile = new System.Windows.Forms.Label();
            this.lblTel = new System.Windows.Forms.Label();
            this.txtEmail = new DevExpress.XtraEditors.TextEdit();
            this.txtTel = new DevExpress.XtraEditors.TextEdit();
            this.txtMobile = new DevExpress.XtraEditors.TextEdit();
            this.txtFax = new DevExpress.XtraEditors.TextEdit();
            this.txtAddress = new DevExpress.XtraEditors.TextEdit();
            this.txtDelegateID = new DevExpress.XtraEditors.TextEdit();
            this.txtEngName = new DevExpress.XtraEditors.TextEdit();
            this.txtArbName = new DevExpress.XtraEditors.TextEdit();
            this.lblArbName = new System.Windows.Forms.Label();
            this.lblEngName = new System.Windows.Forms.Label();
            this.lblCustomerID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPercentage = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTarget = new DevExpress.XtraEditors.TextEdit();
            this.btnImbort = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNotes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMobile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDelegateID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEngName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArbName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPercentage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTarget.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Size = new System.Drawing.Size(739, 116);
            // 
            // gridControl1
            // 
            gridLevelNode2.RelationName = "Level1";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gridControl1.Location = new System.Drawing.Point(435, 119);
            this.gridControl1.MainView = this.GridView;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(304, 272);
            this.gridControl1.TabIndex = 377;
            this.gridControl1.Tag = "";
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
            this.GridView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GridView_RowClick);
            this.GridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GridView_FocusedRowChanged);
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.BackColor = System.Drawing.Color.Transparent;
            this.lblNotes.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotes.Location = new System.Drawing.Point(12, 369);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(106, 14);
            this.lblNotes.TabIndex = 376;
            this.lblNotes.Tag = "Notes";
            this.lblNotes.Text = "ملاحظــــــــــــــــــات";
            // 
            // txtNotes
            // 
            this.txtNotes.EnterMoveNextControl = true;
            this.txtNotes.Location = new System.Drawing.Point(133, 367);
            this.txtNotes.MenuManager = this.ribbonControl1;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtNotes.Size = new System.Drawing.Size(231, 20);
            this.txtNotes.TabIndex = 10;
            this.txtNotes.Tag = "";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.BackColor = System.Drawing.Color.Transparent;
            this.lblAddress.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(12, 343);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(84, 14);
            this.lblAddress.TabIndex = 374;
            this.lblAddress.Tag = "Address";
            this.lblAddress.Text = "الــعـــنــــــــــوان";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(12, 317);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(89, 14);
            this.lblEmail.TabIndex = 373;
            this.lblEmail.Tag = "Email";
            this.lblEmail.Text = "بـريـد إلـيـكتـروني";
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.BackColor = System.Drawing.Color.Transparent;
            this.lblFax.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFax.Location = new System.Drawing.Point(12, 290);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(84, 14);
            this.lblFax.TabIndex = 372;
            this.lblFax.Tag = "Fax";
            this.lblFax.Text = "الـــفـــاكـــــــس";
            // 
            // lblMobile
            // 
            this.lblMobile.AutoSize = true;
            this.lblMobile.BackColor = System.Drawing.Color.Transparent;
            this.lblMobile.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMobile.Location = new System.Drawing.Point(12, 264);
            this.lblMobile.Name = "lblMobile";
            this.lblMobile.Size = new System.Drawing.Size(85, 14);
            this.lblMobile.TabIndex = 371;
            this.lblMobile.Tag = "Mobile";
            this.lblMobile.Text = "مــــــــوبـــــــايـل";
            // 
            // lblTel
            // 
            this.lblTel.AutoSize = true;
            this.lblTel.BackColor = System.Drawing.Color.Transparent;
            this.lblTel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTel.Location = new System.Drawing.Point(12, 237);
            this.lblTel.Name = "lblTel";
            this.lblTel.Size = new System.Drawing.Size(74, 14);
            this.lblTel.TabIndex = 370;
            this.lblTel.Tag = "Telefone";
            this.lblTel.Text = "الهاتــــــــــــف";
            // 
            // txtEmail
            // 
            this.txtEmail.EnterMoveNextControl = true;
            this.txtEmail.Location = new System.Drawing.Point(133, 315);
            this.txtEmail.MenuManager = this.ribbonControl1;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtEmail.Size = new System.Drawing.Size(231, 20);
            this.txtEmail.TabIndex = 8;
            this.txtEmail.Tag = "";
            // 
            // txtTel
            // 
            this.txtTel.EnterMoveNextControl = true;
            this.txtTel.Location = new System.Drawing.Point(133, 235);
            this.txtTel.MenuManager = this.ribbonControl1;
            this.txtTel.Name = "txtTel";
            this.txtTel.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtTel.Properties.Mask.EditMask = "f0";
            this.txtTel.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTel.Size = new System.Drawing.Size(96, 20);
            this.txtTel.TabIndex = 5;
            this.txtTel.Tag = "ISNumber";
            // 
            // txtMobile
            // 
            this.txtMobile.EnterMoveNextControl = true;
            this.txtMobile.Location = new System.Drawing.Point(133, 261);
            this.txtMobile.MenuManager = this.ribbonControl1;
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtMobile.Properties.Mask.EditMask = "f0";
            this.txtMobile.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtMobile.Size = new System.Drawing.Size(96, 20);
            this.txtMobile.TabIndex = 6;
            this.txtMobile.Tag = "ISNumber";
            // 
            // txtFax
            // 
            this.txtFax.EnterMoveNextControl = true;
            this.txtFax.Location = new System.Drawing.Point(133, 288);
            this.txtFax.MenuManager = this.ribbonControl1;
            this.txtFax.Name = "txtFax";
            this.txtFax.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtFax.Properties.Mask.EditMask = "f0";
            this.txtFax.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtFax.Size = new System.Drawing.Size(96, 20);
            this.txtFax.TabIndex = 7;
            this.txtFax.Tag = "ISNumber";
            // 
            // txtAddress
            // 
            this.txtAddress.EnterMoveNextControl = true;
            this.txtAddress.Location = new System.Drawing.Point(133, 341);
            this.txtAddress.MenuManager = this.ribbonControl1;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtAddress.Size = new System.Drawing.Size(231, 20);
            this.txtAddress.TabIndex = 9;
            this.txtAddress.Tag = "";
            // 
            // txtDelegateID
            // 
            this.txtDelegateID.EnterMoveNextControl = true;
            this.txtDelegateID.Location = new System.Drawing.Point(133, 118);
            this.txtDelegateID.MenuManager = this.ribbonControl1;
            this.txtDelegateID.Name = "txtDelegateID";
            this.txtDelegateID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtDelegateID.Properties.Mask.EditMask = "f0";
            this.txtDelegateID.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtDelegateID.Size = new System.Drawing.Size(96, 20);
            this.txtDelegateID.TabIndex = 0;
            this.txtDelegateID.Tag = "ImportantFieldGreaterThanZero";
            // 
            // txtEngName
            // 
            this.txtEngName.EnterMoveNextControl = true;
            this.txtEngName.Location = new System.Drawing.Point(133, 170);
            this.txtEngName.MenuManager = this.ribbonControl1;
            this.txtEngName.Name = "txtEngName";
            this.txtEngName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtEngName.Size = new System.Drawing.Size(231, 20);
            this.txtEngName.TabIndex = 2;
            // 
            // txtArbName
            // 
            this.txtArbName.EnterMoveNextControl = true;
            this.txtArbName.Location = new System.Drawing.Point(133, 144);
            this.txtArbName.MenuManager = this.ribbonControl1;
            this.txtArbName.Name = "txtArbName";
            this.txtArbName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtArbName.Size = new System.Drawing.Size(231, 20);
            this.txtArbName.TabIndex = 1;
            this.txtArbName.Tag = "ImportantField";
            // 
            // lblArbName
            // 
            this.lblArbName.AutoSize = true;
            this.lblArbName.BackColor = System.Drawing.Color.Transparent;
            this.lblArbName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArbName.Location = new System.Drawing.Point(12, 146);
            this.lblArbName.Name = "lblArbName";
            this.lblArbName.Size = new System.Drawing.Size(101, 14);
            this.lblArbName.TabIndex = 364;
            this.lblArbName.Tag = "Arabic Name";
            this.lblArbName.Text = "الاســـــم بالعـــربي";
            // 
            // lblEngName
            // 
            this.lblEngName.AutoSize = true;
            this.lblEngName.BackColor = System.Drawing.Color.Transparent;
            this.lblEngName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEngName.Location = new System.Drawing.Point(11, 172);
            this.lblEngName.Name = "lblEngName";
            this.lblEngName.Size = new System.Drawing.Size(98, 14);
            this.lblEngName.TabIndex = 363;
            this.lblEngName.Tag = "English Name";
            this.lblEngName.Text = "الاســم بالإنجلـيزي";
            // 
            // lblCustomerID
            // 
            this.lblCustomerID.AutoSize = true;
            this.lblCustomerID.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerID.Location = new System.Drawing.Point(12, 119);
            this.lblCustomerID.Name = "lblCustomerID";
            this.lblCustomerID.Size = new System.Drawing.Size(68, 14);
            this.lblCustomerID.TabIndex = 362;
            this.lblCustomerID.Tag = "Delegate ID";
            this.lblCustomerID.Text = "رقـم السائق";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 14);
            this.label1.TabIndex = 379;
            this.label1.Tag = "Percentage";
            this.label1.Text = "الــنــســــــــبــة";
            // 
            // txtPercentage
            // 
            this.txtPercentage.EnterMoveNextControl = true;
            this.txtPercentage.Location = new System.Drawing.Point(133, 204);
            this.txtPercentage.MenuManager = this.ribbonControl1;
            this.txtPercentage.Name = "txtPercentage";
            this.txtPercentage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtPercentage.Properties.Mask.EditMask = "f0";
            this.txtPercentage.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPercentage.Size = new System.Drawing.Size(96, 20);
            this.txtPercentage.TabIndex = 3;
            this.txtPercentage.Tag = "ISNumber";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(235, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 14);
            this.label2.TabIndex = 381;
            this.label2.Tag = "Target";
            this.label2.Text = "الـتــارجـت";
            // 
            // txtTarget
            // 
            this.txtTarget.EnterMoveNextControl = true;
            this.txtTarget.Location = new System.Drawing.Point(301, 204);
            this.txtTarget.MenuManager = this.ribbonControl1;
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtTarget.Properties.Mask.EditMask = "f0";
            this.txtTarget.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTarget.Size = new System.Drawing.Size(63, 20);
            this.txtTarget.TabIndex = 4;
            this.txtTarget.Tag = "ISNumber";
            // 
            // btnImbort
            // 
            this.btnImbort.Location = new System.Drawing.Point(260, 246);
            this.btnImbort.Name = "btnImbort";
            this.btnImbort.Size = new System.Drawing.Size(85, 32);
            this.btnImbort.TabIndex = 382;
            this.btnImbort.Tag = "Import";
            this.btnImbort.Text = "استيراد";
            this.btnImbort.Click += new System.EventHandler(this.btnImbort_Click);
            // 
            // frmResDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(739, 424);
            this.Controls.Add(this.btnImbort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPercentage);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblFax);
            this.Controls.Add(this.lblMobile);
            this.Controls.Add(this.lblTel);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtTel);
            this.Controls.Add(this.txtMobile);
            this.Controls.Add(this.txtFax);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtDelegateID);
            this.Controls.Add(this.txtEngName);
            this.Controls.Add(this.txtArbName);
            this.Controls.Add(this.lblArbName);
            this.Controls.Add(this.lblEngName);
            this.Controls.Add(this.lblCustomerID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmResDriver";
            this.Tag = "Resturant Driver ";
            this.Text = "شاشة اضافة بيانات السائق ";
            this.Load += new System.EventHandler(this.frmSalesDelegates_Load);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.lblCustomerID, 0);
            this.Controls.SetChildIndex(this.lblEngName, 0);
            this.Controls.SetChildIndex(this.lblArbName, 0);
            this.Controls.SetChildIndex(this.txtArbName, 0);
            this.Controls.SetChildIndex(this.txtEngName, 0);
            this.Controls.SetChildIndex(this.txtDelegateID, 0);
            this.Controls.SetChildIndex(this.txtAddress, 0);
            this.Controls.SetChildIndex(this.txtFax, 0);
            this.Controls.SetChildIndex(this.txtMobile, 0);
            this.Controls.SetChildIndex(this.txtTel, 0);
            this.Controls.SetChildIndex(this.txtEmail, 0);
            this.Controls.SetChildIndex(this.lblTel, 0);
            this.Controls.SetChildIndex(this.lblMobile, 0);
            this.Controls.SetChildIndex(this.lblFax, 0);
            this.Controls.SetChildIndex(this.lblEmail, 0);
            this.Controls.SetChildIndex(this.lblAddress, 0);
            this.Controls.SetChildIndex(this.txtNotes, 0);
            this.Controls.SetChildIndex(this.lblNotes, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            this.Controls.SetChildIndex(this.txtPercentage, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtTarget, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnImbort, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNotes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMobile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDelegateID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEngName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArbName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPercentage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTarget.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        internal System.Windows.Forms.Label lblNotes;
        private DevExpress.XtraEditors.TextEdit txtNotes;
        internal System.Windows.Forms.Label lblAddress;
        internal System.Windows.Forms.Label lblEmail;
        internal System.Windows.Forms.Label lblFax;
        internal System.Windows.Forms.Label lblMobile;
        internal System.Windows.Forms.Label lblTel;
        private DevExpress.XtraEditors.TextEdit txtEmail;
        private DevExpress.XtraEditors.TextEdit txtTel;
        private DevExpress.XtraEditors.TextEdit txtMobile;
        private DevExpress.XtraEditors.TextEdit txtFax;
        private DevExpress.XtraEditors.TextEdit txtAddress;
        private DevExpress.XtraEditors.TextEdit txtDelegateID;
        private DevExpress.XtraEditors.TextEdit txtEngName;
        private DevExpress.XtraEditors.TextEdit txtArbName;
        internal System.Windows.Forms.Label lblArbName;
        internal System.Windows.Forms.Label lblEngName;
        internal System.Windows.Forms.Label lblCustomerID;
        internal System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtPercentage;
        internal System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtTarget;
        private DevExpress.XtraEditors.SimpleButton btnImbort;
    }
}

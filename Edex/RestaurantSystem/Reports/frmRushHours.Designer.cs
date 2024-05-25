namespace Edex.RestaurantSystem.Reports
{
    partial class frmRushHours
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRushHours));
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.txtDeliveryID = new DevExpress.XtraEditors.TextEdit();
            this.lblDeliveryName = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtSellerID = new DevExpress.XtraEditors.TextEdit();
            this.lblSellerName = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCustomerID = new DevExpress.XtraEditors.TextEdit();
            this.lblCustomerName = new DevExpress.XtraEditors.TextEdit();
            this.Label6 = new System.Windows.Forms.Label();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbOrderType = new DevExpress.XtraEditors.LookUpEdit();
            this.Label3 = new System.Windows.Forms.Label();
            this.cmbMethodID = new DevExpress.XtraEditors.LookUpEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.label5 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtFromDate = new DevExpress.XtraEditors.DateEdit();
            this.txtToDate = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeliveryID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSellerID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSellerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMethodID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.Name = "Default Legend";
            this.chartControl1.Location = new System.Drawing.Point(0, 112);
            this.chartControl1.Name = "chartControl1";
            series1.Name = "Series 1";
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControl1.Size = new System.Drawing.Size(1096, 251);
            this.chartControl1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.panelControl4);
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1096, 112);
            this.panelControl1.TabIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.BackColor = System.Drawing.Color.White;
            this.simpleButton1.Appearance.BackColor2 = System.Drawing.Color.White;
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Nahdi", 14F);
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.simpleButton1.Location = new System.Drawing.Point(2, 2);
            this.simpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(164, 108);
            this.simpleButton1.TabIndex = 882;
            this.simpleButton1.Text = "عـــــــرض";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.txtDeliveryID);
            this.panelControl4.Controls.Add(this.lblDeliveryName);
            this.panelControl4.Controls.Add(this.label14);
            this.panelControl4.Controls.Add(this.txtSellerID);
            this.panelControl4.Controls.Add(this.lblSellerName);
            this.panelControl4.Controls.Add(this.label9);
            this.panelControl4.Controls.Add(this.txtCustomerID);
            this.panelControl4.Controls.Add(this.lblCustomerName);
            this.panelControl4.Controls.Add(this.Label6);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl4.Location = new System.Drawing.Point(166, 2);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(426, 108);
            this.panelControl4.TabIndex = 881;
            // 
            // txtDeliveryID
            // 
            this.txtDeliveryID.EnterMoveNextControl = true;
            this.txtDeliveryID.Location = new System.Drawing.Point(208, 81);
            this.txtDeliveryID.Name = "txtDeliveryID";
            this.txtDeliveryID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtDeliveryID.Size = new System.Drawing.Size(101, 20);
            this.txtDeliveryID.TabIndex = 886;
            this.txtDeliveryID.Tag = "txtSalesDelegateID";
            this.txtDeliveryID.ToolTip = "SalesDelegateID";
            // 
            // lblDeliveryName
            // 
            this.lblDeliveryName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDeliveryName.Location = new System.Drawing.Point(4, 81);
            this.lblDeliveryName.Name = "lblDeliveryName";
            this.lblDeliveryName.Size = new System.Drawing.Size(205, 20);
            this.lblDeliveryName.TabIndex = 888;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(361, 83);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(48, 14);
            this.label14.TabIndex = 887;
            this.label14.Tag = "Sales Delegates";
            this.label14.Text = "السائق ";
            // 
            // txtSellerID
            // 
            this.txtSellerID.EnterMoveNextControl = true;
            this.txtSellerID.Location = new System.Drawing.Point(208, 53);
            this.txtSellerID.Name = "txtSellerID";
            this.txtSellerID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtSellerID.Size = new System.Drawing.Size(101, 20);
            this.txtSellerID.TabIndex = 881;
            this.txtSellerID.Tag = "txtSellerID";
            this.txtSellerID.ToolTip = "SellerID";
            // 
            // lblSellerName
            // 
            this.lblSellerName.Location = new System.Drawing.Point(5, 53);
            this.lblSellerName.Name = "lblSellerName";
            this.lblSellerName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSellerName.Properties.Appearance.Options.UseBackColor = true;
            this.lblSellerName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblSellerName.Properties.ReadOnly = true;
            this.lblSellerName.Size = new System.Drawing.Size(205, 20);
            this.lblSellerName.TabIndex = 884;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(324, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 14);
            this.label9.TabIndex = 885;
            this.label9.Tag = "Seller";
            this.label9.Text = "الــــــــبائــــــــــع";
            // 
            // txtCustomerID
            // 
            this.txtCustomerID.EnterMoveNextControl = true;
            this.txtCustomerID.Location = new System.Drawing.Point(208, 27);
            this.txtCustomerID.Name = "txtCustomerID";
            this.txtCustomerID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtCustomerID.Size = new System.Drawing.Size(101, 20);
            this.txtCustomerID.TabIndex = 880;
            this.txtCustomerID.Tag = "txtCustomerID";
            this.txtCustomerID.ToolTip = "CustomerID";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Location = new System.Drawing.Point(5, 27);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCustomerName.Properties.Appearance.Options.UseBackColor = true;
            this.lblCustomerName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.lblCustomerName.Properties.ReadOnly = true;
            this.lblCustomerName.Size = new System.Drawing.Size(206, 20);
            this.lblCustomerName.TabIndex = 882;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(324, 29);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(84, 14);
            this.Label6.TabIndex = 883;
            this.Label6.Tag = "Customer";
            this.Label6.Text = "الــــعمـــــــــــيل";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.label12);
            this.panelControl3.Controls.Add(this.cmbOrderType);
            this.panelControl3.Controls.Add(this.Label3);
            this.panelControl3.Controls.Add(this.cmbMethodID);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl3.Location = new System.Drawing.Point(592, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(261, 108);
            this.panelControl3.TabIndex = 880;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(198, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 14);
            this.label12.TabIndex = 885;
            this.label12.Tag = "Sall Method";
            this.label12.Text = "نوع الطلب";
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.EnterMoveNextControl = true;
            this.cmbOrderType.Location = new System.Drawing.Point(12, 58);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbOrderType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbOrderType.Properties.NullText = "";
            this.cmbOrderType.Properties.PopupSizeable = false;
            this.cmbOrderType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbOrderType.Size = new System.Drawing.Size(101, 20);
            this.cmbOrderType.TabIndex = 884;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(158, 25);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(95, 14);
            this.Label3.TabIndex = 883;
            this.Label3.Tag = "Sall Method";
            this.Label3.Text = "طـريـقـة البيـــــــع ";
            // 
            // cmbMethodID
            // 
            this.cmbMethodID.EnterMoveNextControl = true;
            this.cmbMethodID.Location = new System.Drawing.Point(12, 23);
            this.cmbMethodID.Name = "cmbMethodID";
            this.cmbMethodID.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cmbMethodID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbMethodID.Properties.NullText = "";
            this.cmbMethodID.Properties.PopupSizeable = false;
            this.cmbMethodID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cmbMethodID.Size = new System.Drawing.Size(101, 20);
            this.cmbMethodID.TabIndex = 882;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.label5);
            this.panelControl2.Controls.Add(this.Label2);
            this.panelControl2.Controls.Add(this.txtFromDate);
            this.panelControl2.Controls.Add(this.txtToDate);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl2.Location = new System.Drawing.Point(853, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(241, 108);
            this.panelControl2.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(155, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 14);
            this.label5.TabIndex = 879;
            this.label5.Tag = " From Date";
            this.label5.Text = "من تـــــــــــاريخ";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(152, 54);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 14);
            this.Label2.TabIndex = 878;
            this.Label2.Tag = "To Date";
            this.Label2.Text = "الى تـــــــــــاريخ";
            // 
            // txtFromDate
            // 
            this.txtFromDate.EditValue = null;
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(18, 21);
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
            this.txtFromDate.TabIndex = 876;
            // 
            // txtToDate
            // 
            this.txtToDate.EditValue = null;
            this.txtToDate.EnterMoveNextControl = true;
            this.txtToDate.Location = new System.Drawing.Point(18, 52);
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
            this.txtToDate.TabIndex = 877;
            // 
            // frmRushHours
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 363);
            this.Controls.Add(this.chartControl1);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Name = "frmRushHours";
            this.Text = "تقرير الذروة ";
            this.Load += new System.EventHandler(this.FrmRushHours_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRushHours_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeliveryID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSellerID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSellerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMethodID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label Label2;
        private DevExpress.XtraEditors.DateEdit txtFromDate;
        private DevExpress.XtraEditors.DateEdit txtToDate;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        internal System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.LookUpEdit cmbOrderType;
        internal System.Windows.Forms.Label Label3;
        private DevExpress.XtraEditors.LookUpEdit cmbMethodID;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.TextEdit txtSellerID;
        private DevExpress.XtraEditors.TextEdit lblSellerName;
        internal System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtCustomerID;
        private DevExpress.XtraEditors.TextEdit lblCustomerName;
        internal System.Windows.Forms.Label Label6;
        private DevExpress.XtraEditors.TextEdit txtDeliveryID;
        internal System.Windows.Forms.Label lblDeliveryName;
        internal System.Windows.Forms.Label label14;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}
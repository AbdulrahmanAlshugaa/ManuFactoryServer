using DevExpress.XtraEditors.Controls;
using Edex.DAL.Configuration;
using Edex.Model;
using Edex.Model.Language;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraGrid.Views.Grid;


namespace Edex.GeneralObjects.GeneralForms
{
    public partial class frmGeneralOptions : Edex.GeneralObjects.GeneralForms.BaseForm
    {

        #region Declare
        public bool HasColumnErrors = false;
        string FocusedControl = "";
        //  private cItemsBrands cClass = new cItemsBrands();
        public string loc;
        public string strSQL;
        string VAt = "Select CompanyVATID,Cost,sumvalue from  VATIDCOMPANY ";
        float CompanyVATID = 0;
        public MemoryStream TheImage;
        CompanyHeader cmpheader = new CompanyHeader();
        CompanyHeader cmpheaderSave = new CompanyHeader();
        GeneralSettings generalSettting = new GeneralSettings();
        GeneralSettings generalSetttingSave = new GeneralSettings();
        BindingList<StartNumbering> startNumering = new BindingList<StartNumbering>();
        public byte[] data;
        public string MenuName = "";
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        OpenFileDialog OpenFileDialog1 = null;

        // private string strSQL;
        private bool IsNewRecord;

        #endregion
        public frmGeneralOptions()
        {
            InitializeComponent();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = true;
            //ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = true;
            gridView1.ValidatingEditor += gridView1_ValidatingEditor;
        }


        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void frmGeneralOptions_Load(object sender, EventArgs e)
        { 
            // DoInit();
          try{
                cmpheader = CompanyHeaderDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
                generalSettting = GeneralSettingsDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
                DataTable dVat =Lip.SelectRecord(VAt);
                if(dVat.Rows.Count > 0)
                {
                    txtVATID.Text = Comon.cLong(dVat.Rows[0][0]).ToString();
                    txtsumvalue.Text = Comon.cDec(dVat.Rows[0]["sumvalue"]).ToString();
                    txtCost.Text = Comon.cDec(dVat.Rows[0]["Cost"]).ToString();
                }
                else
                    txtVATID.Text = "0";
           if (UserInfo.Language == iLanguage.English)
            {
               ColScreenName.FieldName = "EngCaption";
            }
            cmpheader = CompanyHeaderDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
            generalSettting = GeneralSettingsDAL.GetDataByID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.FacilityID);
            FillComboBox(cmbUsingExpiryDate, "YesNo", "ID", (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName"));
            FillComboBox(cmbUsingItemsSerials, "YesNo", "ID", (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName"));
            FillComboBox(cmbAutoCalcFixAssetsDepreciation, "YesNo", "ID", (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName"));
            FillComboBox(cboCalcStockBy, "Stc_StocktakingAccordingTo", "SalePriceType", (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName"), "ID");
            FillComboBox(cmbWayOfOutItems, "Stc_WaysOfOutItems", "way", (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName"), "ID");
            DataTable dt = new DataTable();
             
            dt.Columns.Add("ID", System.Type.GetType("System.String"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Rows.Add();
            dt.Rows[0]["ID"] = 1;
            dt.Rows[0]["Name"] = (UserInfo.Language == iLanguage.Arabic ? " يــومـي" : "Daily");

            dt.Rows.Add();
            dt.Rows[1]["ID"] = 2;
            dt.Rows[1]["Name"] = (UserInfo.Language == iLanguage.Arabic ? "شـهـري" : "Monthly");

            dt.Rows.Add();
            dt.Rows[2]["ID"] = 3;
            dt.Rows[2]["Name"] = (UserInfo.Language == iLanguage.Arabic ? "ســـنوي" : "Annual");

            cmbDepreciationType.Properties.DataSource = dt;
            cmbDepreciationType.Properties.DisplayMember = "Name";
            cmbDepreciationType.Properties.ValueMember = "ID";
            if (generalSettting == null)
                InitDGeneralSsetting();
            else
                InitGeneralSsetting();
          ///  if (generalSettting != null)
            InitCompanyHeader();
          

            FileItemData();
            gridView1.AddNewRow();
            EnabledControl(false);
            if (UserInfo.Language == iLanguage.English) {

                xtraTabPage1.Text = "General Settings ";
                xtraTabPage2.Text = "Report Header ";
                xtraTabPage4.Text = "Document StartNumbernig ";
                ColScreenName.Caption = "Screen Name";
                gridColumn2.Caption = "Number Starting";
                gridColumn3.Caption = "Auto Numbering";
                gridColumn4.Caption = " N.O";
                Label1.Text = Label1.Tag.ToString();
                Label2.Text = Label2.Tag.ToString();
                Label3.Text = Label3.Tag.ToString();
                Label4.Text = Label4.Tag.ToString();
                Label5.Text = Label5.Tag.ToString();
                Label6.Text = Label6.Tag.ToString();
                Label7.Text = Label7.Tag.ToString();
                Label10.Text = Label10.Tag.ToString();
                Label12.Text = Label12.Tag.ToString();
                label8.Text = label8.Tag.ToString(); 
                label9.Text = label9.Tag.ToString(); 
                label11.Text = label11.Tag.ToString();
                layoutControlGroup2.Text = layoutControlGroup2.Tag.ToString();
                layoutControlGroup3.Text = layoutControlGroup3.Tag.ToString();
                label13.Text = label13.Tag.ToString();
                layoutControlItem3.Text = layoutControlGroup3.Tag.ToString();
                layoutControlGroup4.Text = layoutControlGroup4.Tag.ToString();
                layoutControlGroup5.Text = layoutControlGroup5.Tag.ToString();
                layoutControlGroup6.Text = layoutControlGroup6.Tag.ToString();
                layoutControlGroup7.Text = layoutControlGroup7.Tag.ToString();
                layoutControlItem11.Text = layoutControlItem11.Tag.ToString();
                layoutControlItem10.Text = layoutControlItem10.Tag.ToString();
              //  layoutControlItem9.Text = layoutControlItem9.Tag.ToString();
                layoutControlItem8.Text = layoutControlItem8.Tag.ToString();
               // layoutControlItem7.Text = layoutControlItem7.Tag.ToString();
                layoutControlItem6.Text = layoutControlItem6.Tag.ToString();
                layoutControlItem5.Text = layoutControlItem5.Tag.ToString();
                layoutControlItem4.Text = layoutControlItem4.Tag.ToString();
               // layoutControlItem3.Text = layoutControlItem3.Tag.ToString();
                layoutControlItem2.Text = layoutControlItem2.Tag.ToString();
                layoutControlItem1.Text = layoutControlItem1.Tag.ToString();
                layoutControlItem12.Text = layoutControlItem12.Tag.ToString();
               // layoutControlItem13.Text = layoutControlItem13.Tag.ToString();
              //  layoutControlItem14.Text = layoutControlItem14.Tag.ToString();
              //  layoutControlItem15.Text = layoutControlItem15.Tag.ToString();
                layoutControlItem16.Text = layoutControlItem16.Tag.ToString();
                layoutControlItem17.Text = layoutControlItem17.Tag.ToString();
                layoutControlItem18.Text = layoutControlItem18.Tag.ToString();
               // layoutControlItem19.Text = layoutControlItem19.Tag.ToString();
                layoutControlItem20.Text = layoutControlItem20.Tag.ToString();
                
                btnAddCompanySymbol.Text = btnAddCompanySymbol.Tag.ToString();
            }
            if (MySession.GlobalInventoryType > 0)
                groupBox1.Enabled = false;
              
            }
            catch { }
        }


        private void EnabledControl(bool Value)
        {
            //foreach (Control item in this.Controls)
            //{
            //    if (item is TextEdit )
            //    {
            //            item.Enabled = Value;
            //            ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
            //            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
            //            if (Value == true)
            //                ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    
            //    }
                

            //}
           

        }

        protected override void DoEdit() {
            EnabledControl(true);
        }

        
        public void InitDGeneralSsetting()
        {
            try
            {
                /***********************************************************************/
                txtAllowedPercentDiscount.Text = MySession.GlobalDiscountPercentOnItem.ToString();
                txtItemBarcodeWeightDigits.Text = MySession.GlobalItemBarcodeWeightDigits.ToString();
                txtPriceBarcodeWeightDigits.Text = MySession.GlobalPriceBarcodeWeightDigits.ToString();
                txtQtyDigits.Text = MySession.GlobalQtyDigits.ToString();
                txtPriceGigits.Text = MySession.GlobalPriceDigits.ToString();
                txtBackupPath.Text = "D:\\EdexSystem\\BackUp";
                txtMaxBarcodeDigits.Text = MySession.GlobalMaxBarcodeDigits.ToString();
                txtItemProfit.Text = MySession.GlobalItemProfit.ToString();
                cmbDepreciationType.EditValue = 2;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        public void InitGeneralSsetting()
        {
            try
            {
                /***********************************************************************/
                txtAllowedPercentDiscount.Text = generalSettting.AllowedPercentDiscount.ToString();
              
                txtItemBarcodeWeightDigits.Text = generalSettting.ItemDigits.ToString();
                txtPriceBarcodeWeightDigits.Text = generalSettting.ItemPriceDigits.ToString();
                txtQtyDigits.Text = generalSettting.QtyDigits.ToString();
                txtPriceGigits.Text = generalSettting.PriceDigits.ToString();
                if (!string.IsNullOrEmpty(generalSettting.BackupPath))
                {
                    txtBackupPath.Text = generalSettting.BackupPath;
                    MySession.defaultBackupPath = generalSettting.BackupPath;
                }
                else
                    txtBackupPath.Text = "D:\\EdexSystem\\BackUp";

                txtMaxBarcodeDigits.Text = generalSettting.MaxBarcodeDigits.ToString();
                txtItemProfit.Text = generalSettting.ItemProfit.ToString();
                if (generalSettting.DepreciationType >= 0)
                    cmbDepreciationType.EditValue = generalSettting.DepreciationType;
                else
                    cmbDepreciationType.EditValue = 3;
                cmbUsingExpiryDate.EditValue = generalSettting.UsingExpiryDate;
                cmbUsingItemsSerials.EditValue = generalSettting.UsingItemsSerials;
                cmbWayOfOutItems.EditValue = generalSettting.WayOfOutItems;
                cmbAutoCalcFixAssetsDepreciation.EditValue = generalSettting.AutoCalcFixAssetsDepreciation;
                cboCalcStockBy.EditValue = generalSettting.CalcStockBy;
              
                if (generalSettting.InventoryType == 1)
                    radioInventory.Checked = true;
                else if (generalSettting.InventoryType == 2)
                    radioButton2.Checked = true;

                if (generalSettting.CostCalculationType == 1)
                    radioButton3.Checked = true;
                else if (generalSettting.CostCalculationType == 2)
                    radioButton4.Checked = true;
                else if (generalSettting.CostCalculationType == 3)
                    radioButton5.Checked = true;
                     
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


        }
        public void InitGeneralSsettingSave()
        {
            try
            {
                cmbWayOfOutItems.ItemIndex = 0;
                /***********************************************************************/
                generalSetttingSave.AllowedPercentDiscount = Comon.cDbl(txtAllowedPercentDiscount.Text);
                generalSetttingSave.ItemDigits = Comon.cInt(txtItemBarcodeWeightDigits.Text);
                generalSetttingSave.ItemPriceDigits = Comon.cInt(txtPriceBarcodeWeightDigits.Text);
                generalSetttingSave.QtyDigits = Comon.cInt(txtQtyDigits.Text);
                generalSetttingSave.PriceDigits = Comon.cInt(txtPriceGigits.Text);
                generalSetttingSave.BackupPath = txtBackupPath.Text;


                generalSetttingSave.MaxBarcodeDigits = Comon.cInt(txtMaxBarcodeDigits.Text);
                generalSetttingSave.ItemProfit = Comon.cInt(txtItemProfit.Text);
                generalSetttingSave.DepreciationType = Comon.cInt(cmbDepreciationType.EditValue);

                generalSetttingSave.BranchID = MySession.GlobalBranchID;
                generalSetttingSave.FacilityID = UserInfo.FacilityID;
                generalSetttingSave.UsingExpiryDate = Comon.cInt(cmbUsingExpiryDate.EditValue);
                generalSetttingSave.UsingItemsSerials = Comon.cInt(cmbUsingItemsSerials.EditValue);
                if (cmbWayOfOutItems.EditValue!=null)



                generalSetttingSave.WayOfOutItems = cmbWayOfOutItems.EditValue.ToString();
                generalSetttingSave.AutoCalcFixAssetsDepreciation = Comon.cInt(cmbAutoCalcFixAssetsDepreciation.EditValue);
                if (cboCalcStockBy.EditValue != null)
                generalSetttingSave.CalcStockBy = cboCalcStockBy.EditValue.ToString();
                if (radioInventory.Checked)
                    generalSetttingSave.InventoryType = 1;
                else if(radioButton2.Checked)
                    generalSetttingSave.InventoryType = 2;

                if(radioButton3.Checked)
                    generalSetttingSave.CostCalculationType = 1;
                else if (radioButton4.Checked)
                        generalSetttingSave.CostCalculationType = 2;
                if(radioButton5.Checked)
                    generalSetttingSave.CostCalculationType = 3;

                bool result = GeneralSettingsDAL.UpdateGeneralSettings(generalSetttingSave);

                GlobalSession();


            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


        }

        public void InitCompanyHeader()
        {
            try
            {
                if (cmpheader != null)
                {
                    txtActivityArbName.Text = cmpheader.ActivityArbName;
                    txtActivityEngName.Text = cmpheader.ActivityEngName;
                    txtArbAddress.Text = cmpheader.ArbAddress;
                    txtArbFax.Text = cmpheader.ArbFax;
                    txtArbTel.Text = cmpheader.ArbTel;
                    txtCompanyArbName.Text = cmpheader.CompanyArbName;
                    txtCompanyEngName.Text = cmpheader.CompanyEngName;
                    txtEngAddress.Text = cmpheader.EngAddress;
                    txtEngFax.Text = cmpheader.EngFax;
                    txtEngTel.Text = cmpheader.EngTel;
                    txtFooter.Text = cmpheader.footer;
                    TheImage = new MemoryStream(cmpheader.pic);
                    if (TheImage.Length > 0)
                        picCompanySymbol.Image = Image.FromStream(TheImage, true);

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        public void InitCompanyHeaderSave()
        {
            try
            {
                cmpheaderSave.ID = UserInfo.BRANCHID;
                cmpheaderSave.RegDate = Comon.cLong(Lip.GetServerDateSerial());
                cmpheaderSave.RegTime = Comon.cLong(Lip.GetServerTimeSerial());
                cmpheaderSave.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                cmpheaderSave.EditTime = Comon.cLong(Lip.GetServerDateSerial());
                cmpheaderSave.Cancel = 0;
                cmpheaderSave.UserID = UserInfo.ID;
                cmpheaderSave.EditUserID = UserInfo.ID;
                cmpheaderSave.ComputerInfo = UserInfo.ComputerInfo;
                cmpheaderSave.EditComputerInfo = UserInfo.ComputerInfo;
             ///   txtFooter.RightToLeft = RightToLeft.No;
                cmpheaderSave.footer = txtFooter.Text;
                cmpheaderSave.ActivityArbName = txtActivityArbName.Text;
                cmpheaderSave.ActivityEngName = txtActivityEngName.Text;
                cmpheaderSave.ArbAddress = txtArbAddress.Text;
                cmpheaderSave.ArbFax = txtArbFax.Text;
                cmpheaderSave.ArbTel = txtArbTel.Text;
                cmpheaderSave.CompanyArbName = txtCompanyArbName.Text;
                cmpheaderSave.CompanyEngName = txtCompanyEngName.Text;
                cmpheaderSave.EngAddress = txtEngAddress.Text;
                cmpheaderSave.EngFax = txtEngFax.Text;
                cmpheaderSave.EngTel = txtEngTel.Text;
                cmpheaderSave.BranchID = MySession.GlobalBranchID;
                if (cmpheaderSave.pic == null)
                    if (cmpheader != null)
                    cmpheaderSave.pic = cmpheader.pic;

                cmpheaderSave.FacilityID = UserInfo.FacilityID;
                bool result = CompanyHeaderDAL.DeleteCompanyHeader(cmpheaderSave);
                int boio = CompanyHeaderDAL.InsertCompanyHeader(cmpheaderSave);
                MySession.VAtCompnyGlobal = txtVATID.Text;
                MySession.footer = txtFooter.Text;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


            // cmpheaderSave.pic =(byte[]) picCompanySymbol.EditValue;
            // MemoryStream ms = new MemoryStream();
            ///  picCompanySymbol.Image.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
            //ImageConverter imcon=new ImageConverter();
            //cmpheaderSave.pic = (byte[])imcon.ConvertTo(picCompanySymbol.Image,typeof(byte[ ]));
            // bool bstoreID = CompanyHeaderDAL.UpdateCompanyHeader(cmpheaderSave);

        }   
 
        public void FillComboBox(DevExpress.XtraEditors.LookUpEdit cmb, string Tablename, string Code, string Name, string OrderByField = "")
        {
            try
            {
                string strSQL = "SELECT " + Code + " AS [الرقم]," + Name + "  AS [الاسم] FROM " + Tablename;
                if (OrderByField != "")
                    strSQL = strSQL + " Order By " + OrderByField;
                cmb.Properties.DataSource = Lip.SelectRecord(strSQL).DefaultView;
                cmb.Properties.DisplayMember = "الاسم";
                cmb.Properties.ValueMember = "الرقم";

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
       
        void GlobalSession()
        {
            try{
            MySession.GlobalItemBarcodeWeightDigits = Comon.cInt(txtItemBarcodeWeightDigits.Text);
            MySession.GlobalPriceBarcodeWeightDigits = Comon.cInt(txtPriceBarcodeWeightDigits.Text);
            MySession.GlobalPriceDigits = Comon.cInt(txtPriceGigits.Text);
            MySession.GlobalQtyDigits = Comon.cInt(txtQtyDigits.Text);
           // MySession.GlobalAllowedPercentDiscount = txtAllowedPercentDiscount.Text;
            MySession.GlobalMaxBarcodeDigits = Comon.cLong(txtMaxBarcodeDigits.Text);
            MySession.GlobalItemProfit = Comon.cDbl(txtItemProfit.Text);
            MySession.GlobalUsingExpiryDate = (Comon.cInt(cmbUsingExpiryDate.EditValue) == 1 ? true : false);
            MySession.GlobalUsingItemsSerials = (Comon.cInt(cmbUsingItemsSerials.EditValue) == 1 ? true : false);
            MySession.GlobalAutoCalcFixAssetsDepreciation = (Comon.cInt(cmbAutoCalcFixAssetsDepreciation.EditValue) == 1 ? true : false);
            MySession.GlobalCalcStockBy = cboCalcStockBy.Text;
            MySession.GlobalWayOfOutItems = cmbWayOfOutItems.Text;
            if (radioInventory.Checked)
                MySession.GlobalInventoryType = 1;
            else if (radioButton2.Checked)
                MySession.GlobalInventoryType = 2;

            if (radioButton3.Checked)
                MySession.GlobalCostCalculationType = 1;
            else if (radioButton4.Checked)
                MySession.GlobalCostCalculationType = 2;
            else if (radioButton5.Checked)
                MySession.GlobalCostCalculationType = 3;
            }
            catch 
            {
                Messages.MsgError(Messages.msgErrorSave, "خطأ حفظ بيانات الجلسة العامة ");
            }
        }
        private void xtraTabPage2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void FileItemData()
        {
            try
            {
                DataTable det = new DataTable();

                det = StartNumberingDAL.Get_DeclaringMainAccounts(UserInfo.BRANCHID, UserInfo.FacilityID);
              
                gridControl1.DataSource = det;
                DevExpress.XtraEditors.LookUpEdit cmb = new DevExpress.XtraEditors.LookUpEdit();
                FillComboBox(cmb, "YesNo", "ID", (UserInfo.Language == iLanguage.Arabic ? "ArbName" : "EngName"));
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


        }
        private void saveFileItemData()
        {
            try
            {
                 
                StartNumbering det = new StartNumbering();
                StartNumbering returned;
                List<StartNumbering> listreturned = new List<StartNumbering>();
                det.FacilityID =MySession.GlobalFacilityID;
                det.BranchID = UserInfo.BRANCHID;
                gridView1.MoveNext();
                for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                {
                    returned = new StartNumbering();
                    returned.ID = Comon.cInt(gridView1.GetRowCellValue(i, "ID").ToString());

                    returned.ArbCaption = gridView1.GetRowCellValue(i, "ArbCaption").ToString();
                    returned.EngCaption = gridView1.GetRowCellValue(i, "EngCaption").ToString();
                    returned.BranchID =Comon.cInt( gridView1.GetRowCellValue(i, "BranchID").ToString());
                    returned.FacilityID =Comon.cInt( gridView1.GetRowCellValue(i, "FacilityID").ToString());
                    returned.FormName = gridView1.GetRowCellValue(i, "FormName").ToString();
                    returned.AutoNumber = Comon.cInt(gridView1.GetRowCellValue(i, "AutoNumber").ToString());
                    returned.StartFrom = Comon.cInt(gridView1.GetRowCellValue(i, "StartFrom").ToString());
                   
                    listreturned.Add(returned);
                }
                det.SaleDatails = listreturned;
                bool Result = StartNumberingDAL.Update_StartNumbering(det);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }


        }

        protected override void DoSave()
        {
            int num;
            //Messages.MsgError(Messages.msgErrorSave,int.TryParse(txtActivityArbName.Text, out num) + "");
            try
            {
                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;
                }
                if (!IsNewRecord)
                {
                    if (!FormUpdate)
                    {
                        Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToUpdateRecord);
                        return;
                    }
                    else
                    {
                        bool Yes = Messages.MsgWarningYesNo(Messages.TitleInfo, Messages.msgConfirmUpdate);
                        if (!Yes)
                            return;
                    }
                }
                if (Validations.Important(this))
                {
                    //Messages.MsgExclamationk(Messages.TitleWorning, Messages.msgShouldCompleteData);
                    //return;
                }
                Lip.NewFields();
                Lip.Table = "VATIDCOMPANY";
                Lip.AddNumericField("CompanyVATID", txtVATID.Text);
                Lip.AddNumericField("Cost", txtCost.Text);
                Lip.AddNumericField("sumvalue", txtsumvalue.Text);
                Lip.sCondition = "BranchID =" + MySession.GlobalBranchID+ " AND FacilityID =" + UserInfo.FacilityID;
                Lip.ExecuteUpdate();
           
                saveFileItemData();
                InitCompanyHeaderSave();
                InitGeneralSsettingSave();

                Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
              
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void btnAddCompanySymbol_Click(object sender, EventArgs e)
        {

            try
            {
                try
                {
                    OpenFileDialog1 = new OpenFileDialog();
                    OpenFileDialog1.Filter = "All Files|*.*|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg";
                    OpenFileDialog1.FileName = "";
                    OpenFileDialog1.ShowDialog();
                    if ((OpenFileDialog1.FileName != "") && OpenFileDialog1.FileName!=null && (OpenFileDialog1.ShowDialog() == DialogResult.OK))
                    {
                        picCompanySymbol.Image = Image.FromFile(OpenFileDialog1.FileName);
                        loc = OpenFileDialog1.FileName;                     

                    }

                }
                catch (Exception ex)
                {
                    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

                }

                if (loc != ""&&loc!=null)
                {                    
                    byte[] data = null;
                    FileStream fs = new FileStream(loc, FileMode.Open, FileAccess.Read);
                    FileInfo fInfo = new FileInfo(loc);
                    long numBytes = fInfo.Length;
                    BinaryReader br = new BinaryReader(fs);
                    data = br.ReadBytes((int)numBytes);//(int)fs.Length); 
                    cmpheaderSave.pic = data;
                    InitCompanyHeaderSave();

                    Messages.MsgInfo(Messages.TitleInfo, (UserInfo.Language == iLanguage.Arabic ? "تم حفظ الصورة بنجاح" : "Image save successfuly"));
                }

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            // data = new byte[fs.Length];
            // fs.ReadByte(data, 0, System.Convert.ToInt32(fs.Length));
            //fs.ReadByte(fs.Length)
            //fs.Close();

            //When you use BinaryReader, you need to supply number of bytes 
            //to read from file.
            //In this case we want to read entire file. 
            //So supplying total number of bytes.

            // Image img = Image.FromFile(xtraOpenFileDialog1.FileName);
            //byte[] data = ByteImageConverter.ToByteArray(img, img.RawFormat);
            //picCompanySymbol.EditValue = data;
            // object ev = picCompanySymbol.EditValue;

        }

         

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
          
        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridControl1.RefreshDataSource();

        }

        private void frmGeneralOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeneralSettings generalSettting = GeneralSettingsDAL.GetDataByID(UserInfo.FacilityID, MySession.GlobalBranchID, UserInfo.FacilityID);
            if (generalSettting != null)
            {
                if (generalSettting.InventoryType == null || generalSettting.InventoryType == 0)
                {
                    
                    Messages.MsgInfo(Messages.TitleInfo, "الرجاء تهيئة اعدادات النظام- وطريقة الجرد");
                    if (e.CloseReason == CloseReason.UserClosing)
                       e.Cancel = true;

                    this.Focus();
                }
            }
        }

        private void frmGeneralOptions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                Find();
        }
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "";

            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl.Trim() == gridControl1.Name)
            {

                if (gridView1.FocusedColumn == null) return;
                if (gridView1.FocusedColumn.Name == "ColScreenName")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "Forms", "الاسم البرمجي", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "Forms", "Program Name", MySession.GlobalBranchID);
                }
            }
            GetSelectedSearchValue(cls);
        }
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c is DevExpress.XtraLayout.LayoutControl)
            {
                if (!(((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl == null))
                {
                    c = ((DevExpress.XtraLayout.LayoutControl)ActiveControl).ActiveControl;
                }
            }
            if (c is DevExpress.XtraEditors.TextBoxMaskBox)
            {
                c = c.Parent;
            }

            if (c.Parent is DevExpress.XtraGrid.GridControl)
            {
                return c.Parent.Name;
            }

            return c.Name;
        }
        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (this.gridView1.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;

                string ColName = view.FocusedColumn.FieldName;

                if (ColName == "FormName" || ColName == "StartFrom" || ColName == ColScreenName.Name)
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else
                    {
                        e.Valid = true;
                        view.SetColumnError(gridView1.Columns[ColName], "");

                    }


                }
            }
                 
        }
        bool checkIfFindInGrid(string FormName)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                if (gridView1.GetRowCellValue(i, "FormName").ToString() == FormName)
                    return false;
                
            }
            return true;
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            try
            {
                if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
                {
                    if (FocusedControl == gridControl1.Name)
                    {
                        if (gridView1.FocusedColumn.Name == "ColScreenName")
                        {
                            string Barcode = cls.PrimaryKeyValue.ToString();
                            DataTable dt = Lip.SelectRecord("SELECT *   FROM  [Forms] WHERE  BranchID =" + MySession.GlobalBranchID + "  and FormName='" + Barcode + "'");
                            if (checkIfFindInGrid(Barcode))
                            {
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FormName"], Barcode);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ArbCaption"], dt.Rows[0]["ArbCaption"].ToString());
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EngCaption"], dt.Rows[0]["EngCaption"].ToString());
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AutoNumber"], true);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StartFrom"], "1");
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ID"], gridView1.DataRowCount);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BranchID"], UserInfo.BRANCHID);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FacilityID"], UserInfo.FacilityID);
                                gridView1.AddNewRow();
                            }
                            else
                            {
                                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "المستند الذي تم اختيارة موجود بالفعل " : "The selected document already exists");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
      


    }


}


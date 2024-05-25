using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Edex.GeneralObjects.GeneralForms;
using Edex.DAL.ManufacturingDAL;
using Edex.Model;
using Edex.ModelSystem;
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model.Language;
using DevExpress.XtraSplashScreen;
using System.Globalization;
using DevExpress.XtraReports.UI;
using System.IO;

namespace Edex.Manufacturing.Codes
{
    public partial class frmManufacturingStages : BaseForm
    {
        #region
         private bool IsNewRecord;
         private string strSQL;
         private string PrimaryName;
         string FocusedControl = "";
         public const int xMoveFirst = 7;
         public const int xMovePrev = 8;
         public const int xMoveNext = 9;
         public const int xMoveLast = 10;
         private Manu_OrderRestrictionDAL cClass;
         public CultureInfo culture = new CultureInfo("en-US");
         private DataTable dt;
        #endregion
        public frmManufacturingStages()
        {
            InitializeComponent();

            PrimaryName = "ArbName";
            if(UserInfo.Language==iLanguage.English)
                PrimaryName = "EngName";
            /*********************** Date Format dd/MM/yyyy ****************************/
            InitializeFormatDate(txtOrderDate);

            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
            txtGuidanceID.Validating+=txtGuidanceID_Validating;
            FillCombo.FillComboBox(cmbTypeOrders, "Manu_TypeOrders", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            txtOrderID.Validating += txtOrderID_Validating;
        }

        void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
           
            if (FormView == true)
                ReadRecord( txtOrderID.Text);
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }
        private void InitializeFormatDate(DateEdit Obj)
        {
            Obj.Properties.Mask.UseMaskAsDisplayFormat = true;
            Obj.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            Obj.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            Obj.Properties.Mask.EditMask = "dd/MM/yyyy";
            Obj.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            Obj.EditValue = DateTime.Now;
        }

        private void frmOrderRestriction_Load(object sender, EventArgs e)
        {
            DoNew();
            ribbonControl1.Pages[0].Groups[0].ItemLinks[0].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[2].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[4].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[5].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[7].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[8].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[9].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[10].Visible = true;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[12].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[13].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[14].Visible = true;

        }
        void ChangeBarCode()
        {
            barCodeControl1.AutoModule = true;
            barCodeControl1.Text =  txtOrderID.Text;
            //barCodeControl1.ShowText = true;
            barCodeControl1.Show();
        }
        public void ClearFields()
        {
            try
            {
                txtOrderID.Text = Manu_OrderRestrictionDAL.GetNewID(MySession.GlobalFacilityID, MySession.GlobalBranchID).ToString().PadLeft(6,'0');
                txtDelegateID.Text = "";
                txtNotes.Text = "";
                lblDelegateName.Text = "";
                cmbTypeOrders.ItemIndex = 0;
                txtDelegateID_Validating(null, null);
                txtCustomerID.Text ="";
                txtCustomerID_Validating(null, null);
                txtGuidanceID.Text = UserInfo.ID.ToString();
                lblGuidanceName.Text = UserInfo.UserName;
                rdioCad.Checked = false;
                radioWax.Checked = false;
                radioZircon.Checked = false;
                radioDiamond.Checked = false;
                radioGold.Checked = false;
                radioCompound.Checked = false;
                picItemImage.Image = null;  
            }
            catch
            {}
        }

        private void EnabledControl(bool Value)
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextEdit && ((!(item.Name.Contains("AccountID"))) && (!(item.Name.Contains("AccountName")))))
                {
                    if (!(item.Name.Contains("lbl") && item.Name.Contains("Name")))
                    {
                        item.Enabled = Value;
                        ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                        if (Value == true)
                            ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    }
                }
                else if (item is TextEdit && (((item.Name.Contains("AccountID"))) || ((item.Name.Contains("AccountName")))))
                {
                    item.Enabled = Value;
                    ((TextEdit)item).Properties.AppearanceDisabled.ForeColor = Color.Black;
                    ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                    if (Value)
                        ((TextEdit)item).Properties.AppearanceDisabled.BackColor = Color.White;
                }
                else if (item is SimpleButton && (((item.Name.Contains("btn"))) && ((item.Name.Contains("Search")))))
                {
                    ((SimpleButton)item).Enabled = Value;
                }
            }
        }
        public void MoveRec(string PremaryKeyValue, int Direction)
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                #region If
                if (FormView == true)
                {
                    SplashScreenManager.CloseForm(false);
                    strSQL = "SELECT TOP 1 *  FROM " + Manu_OrderRestrictionDAL.TableName + " Where Cancel =0    And BranchID= " + Comon.cInt(MySession.GlobalBranchID);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_OrderRestrictionDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Manu_OrderRestrictionDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Manu_OrderRestrictionDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Manu_OrderRestrictionDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Manu_OrderRestrictionDAL.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_OrderRestrictionDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Manu_OrderRestrictionDAL();

                    string InvoicIDTemp =  txtOrderID.Text;
                    InvoicIDTemp = cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                    {
                        ReadRecord(InvoicIDTemp);
                        //EnabledControl(false);
                    }
                    SendKeys.Send("{Escape}");
                }
                #endregion
                else
                {
                    SplashScreenManager.CloseForm(false);
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                    return;
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void ChangColerStateOrder()
        {
            //int MaxTypeStageID = Comon.cInt(Lip.GetValue("SELECT MAX([TypeStageID])  FROM  [Manu_AllOrdersDetails] where [Cancel]=0  and [OrderID]=" + txtOrderID.Text));

            int MaxTypeStageID = Comon.cInt(Lip.GetValue("SELECT  [StageID]   FROM  [Manu_ArrangingClosingOrders] where [BranchID]=" + MySession.GlobalBranchID + " and [Cancel]=0   and [OrderID]=" + txtOrderID.Text + " and  ID=(select max(ID) FROM  [Manu_ArrangingClosingOrders] where [BranchID]=" + MySession.GlobalBranchID + " and [Cancel]=0   and [OrderID]=" + txtOrderID.Text+")"));
                            
            Dictionary<int, SimpleButton> buttonColors = new Dictionary<int, SimpleButton>()
            {
                { 1, btnCad },
                { 2, btnWax },
                { 3, btnZircon },
                { 4, btnDiamond },
                { 5, btnAfforstation },
                { 6, btnManufactory },
                { 7, btnPrntage },
                { 8, btnPolishn1 },
                { 9, btncompound },
                { 10, btnDismant },
                { 11, btnAddtional },
                { 12, btnPrntage2 },
                { 13, btnPolishn2 },
                { 14, btnPolishn3 }
            };
            
            foreach (var button in buttonColors.Values)
            {
                button.Appearance.BackColor = Color.White;          
            }

            if (buttonColors.ContainsKey(MaxTypeStageID))
            {
                buttonColors[MaxTypeStageID].Appearance.BackColor = Color.Green;
             
            }
        }

        public void ReadRecord(string OrderID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                    dt = Manu_OrderRestrictionDAL.frmGetDataDetalByID(OrderID, Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        ChangColerStateOrder();
                        cmbTypeOrders.EditValue = Comon.cInt(dt.Rows[0]["TypeOrdersID"].ToString());
                        txtOrderDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["OrderDate"].ToString()), "dd/MM/yyyy", culture);
                        //Validate
                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);
                        txtGuidanceID.Text = dt.Rows[0]["GuidanceID"].ToString();
                        txtGuidanceID_Validating(null, null);
                        txtDelegateID.Text = dt.Rows[0]["DelegateID"].ToString();
                        txtDelegateID_Validating(null, null);
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        if (Comon.cInt(dt.Rows[0]["TypeAuxiliaryMatirialID"].ToString()) == 1)
                            rdioCad.Checked = true;
                        if (Comon.cInt(dt.Rows[0]["TypeAuxiliaryMatirialID"].ToString()) == 2)
                            radioWax.Checked = true;
                        if (Comon.cInt(dt.Rows[0]["TypeAuxiliaryMatirialID"].ToString()) == 3)
                            radioCompound.Checked = true;
                        if (Comon.cInt(dt.Rows[0]["TypeID"].ToString()) == 1)
                            radioZircon.Checked = true;
                        if (Comon.cInt(dt.Rows[0]["TypeID"].ToString()) == 2)
                            radioDiamond.Checked = true;
                        if (Comon.cInt(dt.Rows[0]["TypeID"].ToString()) == 3)
                            radioGold.Checked = true;
                        ChangeBarCode();

                        txtImageCode.Text = dt.Rows[0]["ImageCode"].ToString();

                        txtImageGode_Validating(null, null);
                        Validations.DoReadRipon(this, ribbonControl1);
                    }
                }
            }
            catch
            {

            }
        }
        private void txtImageGode_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtImageCode.Text) == false)
                FileImage(txtImageCode.Text);
        }
        private void FileImage(string ImageCode)
        {
            strSQL = "Select ImageID,TheImage,ImageCode From  MNG_ARCHIVINGDOCUMENTSIMAGES where BranchID =" + UserInfo.BRANCHID + " And ImageCode ='" + ImageCode + "'   Order By ID";
            DataTable dt = Lip.SelectRecord(strSQL);
            picItemImage.Image = null;
            txtImageCode.Text = "";
            if (dt.Rows.Count > 0)
            {
                PictureBox pic = new PictureBox();
                Byte[] imgByte = new Byte[] { };
                imgByte = (Byte[])(dt.Rows[0]["TheImage"]);
                txtImageCode.Text = dt.Rows[0]["ImageCode"].ToString();

                pic.Image = byteArrayToImage(imgByte);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                picItemImage.Image = pic.Image;
            }
            else
            {
                Messages.MsgError(this.GetType().Name, " لا يوجد صورة بهذا الكود");
            }
        }
        protected override void DoPrint()
        {
            {
               
            try
            {
                if (IsNewRecord)
                {
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforePrinting);
                    return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                /******************** Report Body *************************/
                ReportName = "‏rptOrderRestriction";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                   
                     

                    /********************** Master *****************************/
                    rptForm.RequestParameters = false;
                rptForm.Parameters["OrderID"].Value = txtOrderID.Text.Trim().ToString();
                rptForm.Parameters["CustomerName"].Value =lblCustomerName.Text.Trim().ToString();             
                rptForm.Parameters["OrderDate"].Value = txtOrderDate.Text.Trim().ToString();
                rptForm.Parameters["DelegeteID"].Value =lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["GuidanceName"].Value = lblDelegateName.Text.Trim().ToString();
                rptForm.Parameters["TypeOrdersName"].Value =cmbTypeOrders.Text.Trim().ToString();

                    if (rdioCad.Checked)
                   rptForm.Parameters["TypeAuxiliaryMatirialName"].Value ="كاد";
                else if (radioWax.Checked)
                    rptForm.Parameters["TypeAuxiliaryMatirialName"].Value = "شمع";
                else if (radioCompound.Checked)
                    rptForm.Parameters["TypeAuxiliaryMatirialName"].Value = "مركب"; 
                 if(radioZircon.Checked)
                     rptForm.Parameters["TypeName"].Value = "زركون";
                else if (radioDiamond.Checked)
                     rptForm.Parameters["TypeName"].Value = "الماس";
                 if (radioGold.Checked)
                     rptForm.Parameters["TypeName"].Value = "صافي ";
                 var dataTable = new dsReports.rptOrderRestrictionDataTable();
                  
                  dataTable.Rows.Clear();
                  var row = dataTable.NewRow();
                   row["BarCode"] = txtOrderID.Text;
                    if (picItemImage.Image != null)
                    {
                        // تحميل الصورة إلى التقرير
                        byte[] imageBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            picItemImage.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            imageBytes = ms.ToArray();
                        }
                        row["Pic"] = imageBytes;
                    }
                /********Total*********/
                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;
                /********************** Details ****************************/
                 dataTable.Rows.Add(row);
                        rptForm.DataSource = dataTable;
                        rptForm.DataMember = ReportName;
                
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();
                rptForm.ShowPrintStatusDialog = false;
                rptForm.ShowPrintMarginsWarning = false;
                rptForm.CreateDocument();

                SplashScreenManager.CloseForm(false);
                ShowReportInReportViewer = true;
                if (ShowReportInReportViewer)
                {
                    frmReportViewer frmRptViewer = new frmReportViewer();
                    frmRptViewer.documentViewer1.DocumentSource = rptForm;
                    frmRptViewer.ShowDialog();
                }
                else
                {
                    bool IsSelectedPrinter = false;
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                    DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
                    if (dt.Rows.Count > 0)
                        for (int i = 1; i < 6; i++)
                        {
                            string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
                            if (!string.IsNullOrEmpty(PrinterName))
                            {
                                rptForm.PrinterName = PrinterName;
                                rptForm.Print(PrinterName);
                                IsSelectedPrinter = true;
                            }
                        }
                    SplashScreenManager.CloseForm(false);
                    if (!IsSelectedPrinter)
                        Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
           
        }
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                ClearFields();
                txtOrderID.Focus();
                ChangeBarCode();
                EnabledControl(true);
                Validations.DoReadRipon(this, ribbonControl1);
            }
            catch { }
        }
        protected override void DoLast()
        {
            try
            {
                MoveRec("0", xMoveLast);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoFirst()
        {
            try
            {
                MoveRec("0", xMoveFirst);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoNext()
        {
            try
            {
                MoveRec(txtOrderID.Text, xMoveNext);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoPrevious()
        {
            try
            {
                MoveRec(txtOrderID.Text, xMovePrev);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoSearch()
        {
            Find();
        }
        protected override void DoEdit()
        {
            Validations.DoEditRipon(this, ribbonControl1);
            Validations.EnabledControl(this, true);
            txtOrderID.Enabled = false;
        }
        protected override void DoDelete()
        {
            try
            {

                if (!FormDelete)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToDeleteRecord);
                    return;
                }
                else
                {
                    bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                    if (!Yes)
                        return;
                }
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                int TempID = Comon.cInt(txtOrderID.Text);

                Manu_OrderRestriction model = new Manu_OrderRestriction();
                model.OrderID = txtOrderID.Text;
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(MySession.GlobalBranchID);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                string Result = Manu_OrderRestrictionDAL.Delete(model);
                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) > 0)
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    ClearFields();
                    txtOrderID.Text = model.OrderID.ToString();
                    MoveRec(model.OrderID, xMovePrev);
                }
                else
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgErrorSave + " " + Result);
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        protected override void DoSave()
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;
                if (rdioCad.Checked == false && radioWax.Checked == false && radioCompound.Checked == false)
               {
                   Messages.MsgWarning("تنبية ", UserInfo.Language == iLanguage.Arabic ? "لم يتم تحدد نوع الطلبية كاد أو شمع أو مركب .. الرجاء تحديد النوع ومن ثم الحفظ" : "The type of order, cad or wax, has not been specified. Please select the type and then save.");
                   return;
               }
                if (IsNewRecord && !FormAdd)
                {
                    Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToAddNewRecord);
                    return;
                }
                else if (!IsNewRecord)
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

                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                Save();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void Save()
        {

            Manu_OrderRestriction objRecord = new Manu_OrderRestriction();
            objRecord.OrderID = txtOrderID.Text.ToString();
            objRecord.BranchID = Comon.cInt(MySession.GlobalBranchID);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.OrderDate = Comon.ConvertDateToSerial(txtOrderDate.Text).ToString();
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.DelegateID = Comon.cLong(txtDelegateID.Text); 
            objRecord.TypeOrdersID = Comon.cInt(cmbTypeOrders.EditValue); 
            objRecord.GuidanceID = Comon.cInt(txtGuidanceID.Text);  
            objRecord.Notes = txtNotes.Text;
            if (rdioCad.Checked)
                objRecord.TypeAuxiliaryMatirialID = 1;
            else if (radioWax.Checked)
                objRecord.TypeAuxiliaryMatirialID = 2;
            else if (radioCompound.Checked)
                objRecord.TypeAuxiliaryMatirialID = 3;
            if (radioZircon.Checked)
                objRecord.TypeID = 1;
            else if (radioDiamond.Checked)
                objRecord.TypeID = 2;
            else if (radioGold.Checked)
                objRecord.TypeID = 3;
            objRecord.Cancel = 0;
            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecord.ComputerInfo = UserInfo.ComputerInfo;
            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";
            if (IsNewRecord == false)
            {
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }


            string Result = Manu_OrderRestrictionDAL.InsertUsingXML(objRecord, IsNewRecord);
                SplashScreenManager.CloseForm(false);
                if (IsNewRecord == true)
                {
                    if (Comon.cInt(Result) > 0)
                    {

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        Validations.DoLoadRipon(this, ribbonControl1);
                        if (falgPrint == true)
                        {
                            IsNewRecord = false;
                            // txtCommandID.Text = Result.ToString();
                            DoPrint();
                        }
                        DoNew();
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                    }
                }
                else
                {
                    if (Result != "0")
                    {
                        
                      //  EnabledControl(false);
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                       
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);

                    }
                }
        }
        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(MySession.GlobalBranchID);
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtGuidanceID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                 strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0 and [UserID]=" + txtGuidanceID.Text.ToString();
                 CSearch.ControlValidating(txtGuidanceID,lblGuidanceName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtCustomerID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtCustomerID.Text != string.Empty && txtCustomerID.Text != "0")
                {
                    strSQL = "SELECT ArbName as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text;
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                    dt = Lip.SelectRecord(strSQL);
                    if (dt.Rows.Count > 0)
                    {
                        lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                    }
                }
                else
                {
                    lblCustomerName.Text = "";
                    txtCustomerID.Text = "";

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmOrderRestriction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                falgPrint = true;
                DoSave();
            }
            if (e.KeyCode == Keys.F6)
            {
                DoSave();
            }
            if (e.KeyCode == Keys.F3)
                Find();
        }
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;
            else if (FocusedControl.Trim() == txtOrderID.Name)
            {
                //if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "رقم الطلب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "Order ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtCustomerID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID", MySession.GlobalBranchID);
            }
          
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }

            GetSelectedSearchValue(cls);
        }
        string GetIndexFocusedControl()
        {
            Control c = this.ActiveControl;
            if (c == null) return null;
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
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {
                if (FocusedControl == txtOrderID.Name)
                {
                    txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                    txtOrderID_Validating(null, null);
                }              
                else if (FocusedControl == txtCustomerID.Name)
                {
                    txtCustomerID.Text = cls.PrimaryKeyValue.ToString();
                    txtCustomerID_Validating(null, null);
                }
                else if (FocusedControl == txtDelegateID.Name)
                {
                    txtDelegateID.Text = cls.PrimaryKeyValue.ToString();
                    txtDelegateID_Validating(null, null);
                }  
            }
        }
        private void btnFactory_Click(object sender, EventArgs e)
        {
            if (rdioCad.Checked)
            {
                frmCadFactory frm = new frmCadFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
            else if(radioWax.Checked)
            {
                frmWaxFactory frm = new frmWaxFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
            else if (radioCompound.Checked)
            {

                frmAfforestationFactory frm = new frmAfforestationFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lnkAddImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmIDsImages frm = new frmIDsImages();
            frm.SCREENNO = 1;
            frm.IDNo =  txtOrderID.Text.Trim();
            frm.ShowDialog();
        }

        private void FileImage(long SreenID, string IDNo)
        {
            strSQL = "Select ImageID,TheImage From  MNG_ARCHIVINGDOCUMENTSIMAGES where BranchID =" + UserInfo.BRANCHID + " And SCREENNO =" + SreenID + " And IDNo ='" + IDNo + "' Order By ID";
            DataTable dt = Lip.SelectRecord(strSQL);

            if (dt.Rows.Count > 0)
            {
                PictureBox pic = new PictureBox();
                Byte[] imgByte = new Byte[] { };
                imgByte = (Byte[])(dt.Rows[0]["TheImage"]);
                pic.Image = byteArrayToImage(imgByte);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                picItemImage.Image = pic.Image;

            }
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        private void btnAddImage_Click(object sender, EventArgs e)
        {
            lnkAddImage_LinkClicked(null, null);
        }

        private void btnDelImage_Click(object sender, EventArgs e)
        {
            
            Lip.NewFields();
            Lip.Table = "MNG_ARCHIVINGDOCUMENTSIMAGES";
            Lip.sCondition = "BranchID =" + UserInfo.BRANCHID + " AND SCREENNO =" + 1 + " AND IDNo ='" + txtOrderID.Text + "' AND ImageID =" + 1;
            Lip.ExecuteDelete();
        }

        private void radioCompound_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnPrentage_Click(object sender, EventArgs e)
        {
            if (rdioCad.Checked)
            {
                frmCadFactory frm = new frmCadFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
            else if (radioWax.Checked)
            {
                frmWaxFactory frm = new frmWaxFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
            else if (radioCompound.Checked)
            {

                frmAfforestationFactory frm = new frmAfforestationFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
        }

        private void btnPolisheingOne_Click(object sender, EventArgs e)
        {
            if (rdioCad.Checked)
            {
                frmCadFactory frm = new frmCadFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
            else if (radioWax.Checked)
            {
                frmWaxFactory frm = new frmWaxFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
            else if (radioCompound.Checked)
            {

                frmAfforestationFactory frm = new frmAfforestationFactory();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int TypeID = Comon.cInt(Lip.GetValue("SELECT  [TypeID] FROM  [Manu_OrderRestriction] where OrderID='" + txtOrderID.Text + "'  and Cancel=0 and BranchID=" + UserInfo.BRANCHID));
            if (TypeID == 1)
            {
                frmZirconeFactory frm = new frmZirconeFactory();
                if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
            else if (TypeID == 2)
            {
                frmDiamondFactory frm = new frmDiamondFactory();
                if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int TypeID = Comon.cInt(Lip.GetValue("SELECT  [TypeID] FROM  [Manu_OrderRestriction] where OrderID='" + txtOrderID.Text + "'  and Cancel=0 and BranchID=" +  UserInfo.BRANCHID));
            if (TypeID == 1)
            {
                frmZirconeFactory frm = new frmZirconeFactory();
                if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
            else if (TypeID == 2)
            {
                frmDiamondFactory frm = new frmDiamondFactory();
                if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmAfforestationFactory frm = new frmAfforestationFactory();
            if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
            }
            else
                frm.Dispose();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmCasting frm = new frmCasting();
            strSQL = " SELECT CommandID from    dbo.Manu_CastingOrders   where   dbo.Manu_CastingOrders.OrderID = " + txtOrderID.Text;
            DataTable dt = new DataTable();
            dt = Lip.SelectRecord(strSQL);
            if (dt.Rows.Count > 0)
            {
                if (ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtCommandID.Text = dt.Rows[0]["CommandID"].ToString();
                    frm.txtCommandID_Validating(null, null);
                }
                else
                    frm.Dispose();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string strSql;
            DataTable dt;
            string txtOrder = "";
            txtOrder = txtOrderID.Text;
            if (txtOrderID.Text != string.Empty && txtOrderID.Text != "0")
            {
                int CommandID = 0;
                CommandID = Comon.cInt(Lip.GetValue("select ComandID from Menu_FactoryRunCommandMaster where Cancel=0  and Barcode='" + txtOrderID.Text + "'"));
                if ((MySession.GlobalDefaultCanRepetUseOrderOneOureMoreManufactory == true && CommandID > 0))
                {
                    if (CommandID > 0)
                    {
                        frmManufacturingCommand frm = new frmManufacturingCommand();
                        if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                        {
                            if (UserInfo.Language == iLanguage.English)
                                ChangeLanguage.EnglishLanguage(frm);
                            frm.Show();
                            frm.txtCommandID.Text = CommandID.ToString();
                            frm.txtCommandID_Validating(null, null);
                            return;
                        }
                        else
                            frm.Dispose();
                    }
                }
                else if (CommandID <= 0)
                {
                    frmManufacturingCommand frm = new frmManufacturingCommand();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                        txtOrder = txtOrderID.Text;
                        string OrderID = txtOrder;
                        txtOrderID.Text = OrderID;
                        frm.SetDetilOrder(txtOrderID.Text);
                    }
                    else
                        frm.Dispose();
                }
                
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmManufacturingPrentag frm = new frmManufacturingPrentag();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPrntageTypeID.EditValue = 2;
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
            }
            else
                frm.Dispose();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 1;
                frm.Show();
                frm.GetValueORderID(this.txtOrderID.Text);
            }
            else
                frm.Dispose();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            frmManufacturingCompond frm = new frmManufacturingCompond();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null,null);
            }
            else
                frm.Dispose();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 2;
                frm.Show();
                frm.GetValueORderID(this.txtOrderID.Text);
            }
            else
                frm.Dispose();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            frmManufacturingPrentag frm = new frmManufacturingPrentag();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPrntageTypeID.EditValue = 1;
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
            }
            else
                frm.Dispose();
        }

        private void btnCost_Click(object sender, EventArgs e)
        {
            frmManuExpencessOrder frm = new frmManuExpencessOrder();

            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                
                strSQL = " SELECT CommandID from    dbo.Manu_CastingOrders   where   dbo.Manu_CastingOrders.OrderID = " + txtOrderID.Text;
                DataTable dt = new DataTable();
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                {
                    
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtCastingID.Text = dt.Rows[0]["CommandID"].ToString();
                    frm.TxtCastingID_Validating(null, null);
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                }
                else
                {
                    Messages.MsgInfo(Messages.TitleInfo, "لا يوجد امر صب مرتبط بالطلبية");
                }
            }
            else
                frm.Dispose();
        }

        private void btnEndOrderReport_Click(object sender, EventArgs e)
        {
            frmOrderRunningReport frm = new frmOrderRunningReport();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void btnReciveOrder_Click(object sender, EventArgs e)
        {
            frmClosingOrders frm = new frmClosingOrders();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.FormView = true;
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
                frm.Show();
            }
            else
                frm.Dispose();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            frmManufacturingTalmee frm = new frmManufacturingTalmee();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.cmbPollutionTypeID.EditValue = 3;
                frm.Show();
                frm.GetValueORderID(this.txtOrderID.Text);
            }
            else
                frm.Dispose();
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            frmManufacturingDismantOrders frm = new frmManufacturingDismantOrders();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
               
                frm.Show();
                frm.GetValueORderID(this.txtOrderID.Text);
            }
            else
                frm.Dispose();
        }

        private void btnAddtional_Click(object sender, EventArgs e)
        {
            frmManufactoryAdditional frm = new frmManufactoryAdditional();
            if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);

                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
            }
            else
                frm.Dispose();
        }
    }
}
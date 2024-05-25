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
using Edex.SalesAndPurchaseObjects.Codes;

namespace Edex.Manufacturing.Codes
{
    public partial class frmOrderRestriction : BaseForm
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
        public frmOrderRestriction()
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
            EnableControlDefult();
        }
       void EnableControlDefult()
        {
            txtOrderDate.ReadOnly = !MySession.GlobalAllowChangefrmOrderRestrctionCommandDate;
            groupBox2.Enabled =  MySession.GlobalAllowChangefrmOrderRestrctionTypeID;
            groupBox1.Enabled =  MySession.GlobalAllowChangefrmOrderRestrctionTypeMatirialID;
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
            {
            }
        }
        void SetDefultValue()
        {
            if (Comon.cInt(MySession.GlobalDefaultTypeOrderRestrectionID) == 1)
                radioZircon.Checked = true;
            else if (Comon.cInt(MySession.GlobalDefaultTypeOrderRestrectionID) == 2)
                radioDiamond.Checked = true;
            else if (Comon.cInt(MySession.GlobalDefaultTypeOrderRestrectionID) == 3)
                radioGold.Checked = true;
            else if (Comon.cInt(MySession.GlobalDefaultTypeOrderRestrectionID) == 4)
                radioMentince.Checked = true;


            if (Comon.cInt(MySession.GlobalDefaultTypeMatirialOrderRestrectionID) == 1)
                rdioCad.Checked = true;
            else if (Comon.cInt(MySession.GlobalDefaultTypeMatirialOrderRestrectionID) == 2)
                radioWax.Checked = true;
            else if (Comon.cInt(MySession.GlobalDefaultTypeMatirialOrderRestrectionID) == 3)
                radioCompound.Checked = true;

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
                ChangeBarCode();
                SetDefultValue();
                //EnabledControl(true);
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

                Lip.ExecututeSQL("Update Manu_OrderRestriction Set ImageCode='" + txtImageCode.Text + "' where OrderID=" + Comon.cInt(txtOrderID.Text) + " and BranchID=" + MySession.GlobalBranchID);
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
            objRecord.ImageCode = txtImageCode.Text;
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
                 strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0 and BranchID=" + MySession.GlobalBranchID+" and [UserID]=" + txtGuidanceID.Text.ToString();
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
                    strSQL = "SELECT " + PrimaryName + " as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + " and BranchID=" + MySession.GlobalBranchID;
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
            else if (e.KeyCode == Keys.F2)
                ShortcutOpen();
        }
        private void ShortcutOpen()
        {
            FocusedControl = GetIndexFocusedControl();
            if (FocusedControl == null) return;


            if (FocusedControl.Trim() == txtCustomerID.Name )
            {
                frmCustomers frm = new frmCustomers();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(UserInfo.BRANCHID), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            if (FocusedControl.Trim() ==txtDelegateID.Name)
            {
                frmSalesDelegates frm = new frmSalesDelegates();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(UserInfo.BRANCHID), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            if (FocusedControl.Trim() == cmbTypeOrders.Name)
            {
                frmTypeOrders frm = new frmTypeOrders();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(UserInfo.BRANCHID), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            
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

            else if (FocusedControl.Trim() == txtImageCode.Name)
            {
                //if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };
                 
              
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtImageCode, null, "ImageCode", "كود", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtImageCode, null, "ImageCode", "ImageCode", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtCustomerID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                { 
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "Customer ID", MySession.GlobalBranchID);
                }
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
                    //txtOrderID_Validating(null, null);
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
                else if (FocusedControl == txtImageCode.Name)
                {
                    txtImageCode.Text = cls.PrimaryKeyValue.ToString();
                    txtImageGode_Validating(null, null);
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
            frm.SCREENNO = 2;
            frm.IDNo =  "1";
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
            Lip.sCondition = " BranchID =" + UserInfo.BRANCHID + " AND SCREENNO =" + 1 + " AND IDNo ='" + txtOrderID.Text + "' AND ImageID =" + 1;
            Lip.ExecuteDelete();
        }

        private void radioCompound_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtImageGode_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtImageCode.Text)==false)
            FileImage(txtImageCode.Text);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            txtImageCode.Focus();
            Find();
        }

        private void txtOrderDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Lip.CheckDateISAvilable(txtOrderDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
               txtOrderDate.Text = Lip.GetServerDate();
                return;
            }
        }
    }
}
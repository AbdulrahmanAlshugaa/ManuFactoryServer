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
using Edex.ModelSystem;
using Edex.Model;
using Edex.DAL.ManufacturingDAL;
using Edex.Model.Language;
using System.Globalization;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.Utils;
using Edex.ModelSystem;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.DAL.Accounting;
using Edex.AccountsObjects.Transactions;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Application = System.Windows.Forms.Application;
using DevExpress.DashboardCommon.Viewer;
using DevExpress.XtraExport.Helpers;
using Edex.AccountsObjects.Codes;
using Edex.SalesAndPurchaseObjects.Codes;
using Edex.StockObjects.Codes;
using Permissions = Edex.ModelSystem.Permissions;
using Edex.HR.Codes;
using DevExpress.XtraReports.UI;
using Edex.StockObjects.Transactions;
using DevExpress.XtraTreeList.Data;
using DevExpress.Data;

namespace Edex.Manufacturing.Codes
{
    public partial class frmCadFactory : BaseForm
    {
        #region DECLARE 
        public int DocumentTypeCadFactory = 25;
        BindingList<Manu_CadWaxFactoryDetails> lstDetail = new BindingList<Manu_CadWaxFactoryDetails>();
        private bool IsNewRecord;
        private string strSQL;
        private string PrimaryName;
        string FocusedControl = "";
        private Manu_CadWaxFactoryDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        private string ItemName; 
        private string SizeName;
        private string CaptionItemName;
        public CultureInfo culture = new CultureInfo("en-US");
        public bool HasColumnErrors = false;
        private DataTable dt;
        #endregion
        public frmCadFactory()
        {
            InitializeComponent(); 
            ItemName = "ArbItemName";
            PrimaryName = "ArbName";
            SizeName = "ArbSizeName";

            CaptionItemName = "اسم الصنف";
            if (UserInfo.Language == iLanguage.English)
            {
                ItemName = "EngItemName"; 
                SizeName = "EngSizeName";
                PrimaryName = "EngName";
                CaptionItemName = "Item Name";
            }
            /*********************** Date Format dd/MM/yyyy ****************************/
            InitializeFormatDate(txtOrderDate);
            InitializeFormatDate(txtBeforeDate);
            InitializeFormatDate(txtAfterDate);
            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
            this.GridCad.CustomDrawCell += GridCadWax_CustomDrawCell;
            txtGuidanceID.Validating += txtGuidanceID_Validating;
            txtOrderID.Validating += txtOrderID_Validating;
            txtAfterStoreID.Validating+=txtAfterStoreID_Validating;
            txtBeforeStoreID.Validating+=txtBeforeStoreID_Validating;
            txtFactorID.Validating+=txtFactorID_Validating;
            txtCostCenterID.Validating+=txtCostCenterID_Validating;
            this.gridControl1.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl11_ProcessGridKey);
            this.GridCad.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridCadWax_ValidatingEditor);
            FillCombo.FillComboBox(cmbTypeOrders, "Manu_TypeOrders", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "  BranchID=" + MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
           

            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
            cmbStatus.EditValueChanging += cmbStatus_EditValueChanging;
            this.GridCad.RowUpdated += GridCadWax_RowUpdated;

            FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbTypeStage.EditValue = 1;
            cmbTypeStage.ReadOnly = true;
            ControlExtensions.SetTabIndexForControls(this);     
            GridCad.CellValueChanging+=GridCad_CellValueChanging;

            EnableControlDefult();
        }
        void EnableControlDefult()
        {
            txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmCadCostCenterID;
            cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmCadCurrncyID;
            txtBeforeDate.ReadOnly = !MySession.GlobalAllowChangefrmCadCommandDate;
            txtAfterStoreID.ReadOnly = !MySession.GlobalAllowChangefrmCadAfterStoreID;
            txtBeforeStoreID.ReadOnly = !MySession.GlobalAllowChangefrmCadBeforeStoreID;
            txtFactorID.ReadOnly = !MySession.GlobalAllowChangefrmCadEmployeeID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowChangefrmCadBranchID;

            
        }
        void SetDefultValue()
        {
            txtCostCenterID.Text = MySession.GlobalDefaultCadCostCenterID;
            txtCostCenterID_Validating(null, null);
            cmbCurency.EditValue = MySession.GlobalDefaultCadCurencyID;
            txtAfterStoreID.Text = MySession.GlobalDefaultCadAfterStoreAccontID;
            txtAfterStoreID_Validating(null, null);
            txtBeforeStoreID.Text = MySession.GlobalDefaultCadBeforeStoreAccontID;
            txtBeforeStoreID_Validating(null, null);
            txtFactorID.Text = MySession.GlobalDefaultCadEmpolyeeID;
            txtFactorID_Validating(null, null);
        }
        void GridCad_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (((view.GetRowCellValue(e.RowHandle, "ItemID") == null) || Comon.cInt(view.GetRowCellValue(e.RowHandle, "ItemID")) <= 0) && e.Column.FieldName == "ShownInNext")

                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء اضافة صنف ومن ثم تفعيل الخيار" : "Please Add Item and selcet option");
                    return;
                }
                if (e.Column.FieldName == "ShownInNext" )
                {
                    if (Comon.cbool(e.Value) == true)
                    {

                        int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils] FROM [Stc_Items] WHERE [ItemID] = " + view.GetRowCellValue(e.RowHandle, "ItemID") + " AND Cancel = 0  and  BranchID=" + MySession.GlobalBranchID));

                        if (isShow != 1)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotSelectShowInDetilsOrder);

                            view.SetRowCellValue(e.RowHandle, "ShownInNext", false);
                        }
                    }
                    SendKeys.Send("\t");
                }
            }
            catch { }
            
        }
        void GridCadWax_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculateQTY();
        }
        void CalculateQTY()
        {
            try
            {
                decimal ToatlQty = 0;
                decimal ToatlCostPrice = 0;
                for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
                {   
                    ToatlQty += Comon.cDec(GridCad.GetRowCellValue(i, "QTY").ToString());
                    ToatlCostPrice += Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "TotalCost").ToString());
                }
                txtTotalQTY.Text = ToatlQty.ToString();
                txtTotalCostPrice.Text = ToatlCostPrice.ToString();
             }
            catch (Exception ex) { }
        }
        void cmbStatus_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Comon.cInt(cmbStatus.EditValue) <= 1)
                txtAfterStoreID.Tag = "isNumber";
            else
                txtAfterStoreID.Tag = "ImportantFieldGreaterThanZero";
        }

        void GridCadWax_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "ShownInNext")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                GridCad.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridCad.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
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
        public void MoveRec(long PremaryKeyValue, int Direction)
        {
            try
            {
                Application.DoEvents();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
                #region If
                if (FormView == true)
                {
                    SplashScreenManager.CloseForm(false);
                    strSQL = "SELECT TOP 1 *  FROM " + Manu_CadWaxFactoryDAL.TableName + " Where Cancel =0 and TypeStageID="+Comon.cInt(cmbTypeStage.EditValue)+" And BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_CadWaxFactoryDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Manu_CadWaxFactoryDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Manu_CadWaxFactoryDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Manu_CadWaxFactoryDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Manu_CadWaxFactoryDAL.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_CadWaxFactoryDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Manu_CadWaxFactoryDAL();

                    int InvoicIDTemp = Comon.cInt(txtCommandID.Text);
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
     public   void txtOrderID_Validating(object sender, CancelEventArgs e)
        {

            if (FormView == true)
            {
                if (String.IsNullOrEmpty(txtOrderID.Text) == false)
                {
                    string txtOrder = txtOrderID.Text;
                    int i = Comon.cInt(Lip.GetValue("SELECT [TypeAuxiliaryMatirialID]  FROM  [Manu_OrderRestriction] where [OrderID]='" + txtOrderID.Text + "' and Cancel=0 and  BranchID=" + MySession.GlobalBranchID));
                    if (i != Comon.cInt(cmbTypeStage.EditValue) && i > 0)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "نوع الطلبية المدخلة لا يتناسب مع الامر الحالي " : "The type of order entered does not match the current order");
                        txtOrderID.Text = "";
                        return;
                    }

                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt( Lip.GetValue("select CommandID from Manu_CadWaxFactoryMaster where Cancel=0 and  BranchID=" + MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and CommandID<>" + Comon.cInt(txtCommandID.Text) +" and OrderID='"+txtOrderID.Text+"'"));
                    int CommandIDThis = Comon.cInt(Lip.GetValue("select CommandID from Manu_CadWaxFactoryMaster where Cancel=0 and  BranchID=" + MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and CommandID=" + Comon.cInt(txtCommandID.Text) + " and OrderID='" + txtOrderID.Text + "'"));

                    if (MySession.GlobalDefaultCanRepetUseOrderOneOureMoreBeforeCasting == true && CommandIDTemp > 0)
                    {
                       if (CommandIDTemp > 0)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgDontRepetTheOrderinMoreCommend);
                            txtCommandID.Text = CommandIDTemp.ToString();
                            txtCommandID_Validating(null, null);
                            return;
                        }
                    }
                    else if (IsNewRecord == false && CommandIDTemp > 0 && CommandIDThis != Comon.cInt(txtCommandID.Text))
                    {
                        if (CommandIDTemp > 0)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheOrderAlreadyExists);
                      
                        txtOrder = txtOrderID.Text;
                        ////ClearFields();
                        //string OrderID = txtOrder;
                        //DoNew();
                        //txtOrderID.Text = OrderID;
                        ReadTopInfo(txtOrderID.Text);
                        //IsNewRecord = true;
                        Validations.DoEditRipon(this, ribbonControl1);
                    }
                    //&& CommandIDTemp <= 0
                    if ((IsNewRecord ))
                    {
                        if(CommandIDTemp>0)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheOrderAlreadyExists);
                      
                        string OrderID = txtOrder;
                        strSQL = "SELECT * FROM Manu_OrderRestriction WHERE  OrderID ='" + OrderID.Trim() + "' and  BranchID=" + MySession.GlobalBranchID;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                        System.Data.DataTable dtt = Lip.SelectRecord(strSQL);
                        if (dtt.Rows.Count > 0)
                        {
                            ReadTopInfo(txtOrderID.Text);
                        }
                        else
                        {
                            txtOrderID.Text = "";
                            txtCustomerID.Text = "";
                            lblCustomerName.Text = "";
                            txtDelegateID.Text = "";
                            lblDelegateName.Text = "";
                            txtGuidanceID.Text = "";
                            lblGuidanceName.Text = "";
                            txtOrderID.Focus();
                            InitializeFormatDate(txtOrderDate); 
                            Messages.MsgError("تنبيه", " لا يوجد طلبية بهذا الرقم ");
                            ClearFields();
                        }
                        return;
                    }


                }
            }
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }         
        }
        public void ClearFieldsTop()
        {
            try
            {
                txtCustomerID.ReadOnly = true;
                txtDelegateID.ReadOnly = true;
                txtOrderDate.ReadOnly = true; 
                txtGuidanceID.ReadOnly = true;
                cmbTypeOrders.ReadOnly = true;
                txtDelegateID.Text = "";
                txtDelegateID_Validating(null, null);
                txtCustomerID.Text = "";
                txtCustomerID_Validating(null, null);
                txtGuidanceID.Text = "";
                txtGuidanceID_Validating(null, null);             
            }
            catch
            {

            }
        }
        public void ClearFields()
        {
            try
            {
                txtTotalCostPrice.Text = "";
                txtTotalQTY.Text = "";
                lblPeriod.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";
                txtNotes.Text = ""; 
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultCadCurencyID);
                txtBeforeStoreID.Text = "";
                txtBeforeStoreID_Validating(null, null);
                lstDetail = new BindingList<Manu_CadWaxFactoryDetails>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl1.DataSource = lstDetail;
                ClearFieldsTop();
                dt = new DataTable();
                txtOrderID.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void initGrid()
        {

            lstDetail = new BindingList<Manu_CadWaxFactoryDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;

            gridControl1.DataSource = lstDetail;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits where  BranchID=" + MySession.GlobalBranchID);
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameUnit[i] = dtitems.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControl1.RepositoryItems.Add(riComboBoxitems);
            GridCad.Columns[SizeName].ColumnEdit = riComboBoxitems;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and  BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl1.RepositoryItems.Add(riComboBoxitems4);
            GridCad.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridCad.Columns["CommandID"].Visible = false;
            GridCad.Columns["BranchID"].Visible = false;
            GridCad.Columns["FacilityID"].Visible = false; 
            GridCad.Columns["ArbItemName"].Visible = GridCad.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridCad.Columns["EngItemName"].Visible = GridCad.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
      
            GridCad.Columns["TotalCost"].OptionsColumn.ReadOnly = false;
            GridCad.Columns[ItemName].Visible = true;
            GridCad.Columns[ItemName].Caption = CaptionItemName;
            GridCad.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            GridCad.Columns["TotalCost"].OptionsColumn.AllowFocus = false; 
            GridCad.Columns[ItemName].Width = 150;
            GridCad.Columns[SizeName].Width = 120;
            GridCad.Columns["SizeID"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridCad.Columns["EngItemName"].Visible = false;
                GridCad.Columns["EngSizeName"].Visible = false;
                GridCad.Columns["BarCode"].Caption = "باركود الصنف";
                GridCad.Columns["SizeID"].Caption = "رقم الوحدة";
                GridCad.Columns["ItemID"].Caption = "رقم الصنــف";

                GridCad.Columns[SizeName].Caption = "إسم الوحدة"; 
                GridCad.Columns["QTY"].Caption = "الكمية ";
                GridCad.Columns["CostPrice"].Caption = "القيمة";
                GridCad.Columns["TotalCost"].Caption = "الإجمالي ";
                GridCad.Columns["Fingerprint"].Caption = "البصمــة";
                GridCad.Columns["ShownInNext"].Caption = "يظهر في التفاصيل"; 
            }
            else
            {
                GridCad.Columns["ArbItemName"].Visible = false; 
                GridCad.Columns["ArbSizeName"].Visible = false;
                GridCad.Columns["BarCode"].Caption = "BarCode"; 
                GridCad.Columns["SizeID"].Caption = "Unit ID";
                GridCad.Columns["ItemID"].Caption = "Item ID";
                GridCad.Columns[SizeName].Caption = "Unit Name ";
                GridCad.Columns["CostPrice"].Caption = "Cost Price";
                GridCad.Columns["QTY"].Caption = "QTY";
                GridCad.Columns["TotalCost"].Caption = "Total Cost "; 
                GridCad.Columns["Fingerprint"].Caption = "Fingerprint";
                GridCad.Columns["ShownInNext"].Caption = "Shown In Next"; 
            }

        }
        

        public void ReadTopInfo(string OrderID, bool flag = false)
        {
                try
                {
                    ClearFieldsTop();
                    {
                        dt = Manu_OrderRestrictionDAL.frmGetDataDetalByID(OrderID, Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
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
                        }
                        else
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد طلبية تمتلك هذا الرقم .. الرجاء ادخال رقم الطلبية الصحيح" : "There is no order that has this number. Please enter the correct order number");
                            txtOrderID.Text = "";
                        }
                    }
                }
                catch
                {
                }
        }
        public void ReadRecord(int CommendID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                    dt = Manu_CadWaxFactoryDAL.frmGetDataDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,Comon.cInt(cmbTypeStage.EditValue));                 
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        txtCommandID.Text = dt.Rows[0]["CommandID"].ToString();                    
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        cmbStatus.EditValue = Comon.cInt(dt.Rows[0]["Posted"].ToString());
                        txtBeforeStoreID.Text =Comon.cDbl( dt.Rows[0]["StoreIDBefore"]).ToString();
                        txtBeforeStoreID_Validating(null, null);
                        txtAfterStoreID.Text =Comon.cDbl(dt.Rows[0]["StoreIDAfter"]).ToString();                      
                        txtAfterStoreID_Validating(null, null);
                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        txtFactorID.Text = dt.Rows[0]["FactorID"].ToString();
                        txtFactorID_Validating(null, null);
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"]);
                        txtBeforeDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["DateBefore"].ToString()), "dd/MM/yyyy", culture);
                        txtAfterDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["DateAfter"].ToString()), "dd/MM/yyyy", culture);
                        gridControl1.DataSource = dt;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;
                        lblAfterStoreManger.Text = dt.Rows[0]["StoreMangerAfter"].ToString();
                        lblBeforeStoreManger.Text = dt.Rows[0]["StoreMangerBefore"].ToString();
                        txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        //txtOrderID_Validating(null, null);
                        ReadTopInfo(txtOrderID.Text);
                        //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtCommandID.Text;
                        txtAfterDate_EditValueChanged(null, null);
                        CalculateQTY();
                        Validations.DoReadRipon(this, ribbonControl1);
                        EnabledControl(false);
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public static void SetValuseWhenChangeSizeName(GridView Grid, long ItemID, int SizeID, string TableNameDetails, string TableNameMaster,double StoreID,int CommandID, string FildNameCommmand = "CommandID", string FildNameBarCode = "BarCode", string FildNameQTY = "QTY", string FildNameTotalCost = "", string Where = "")
        {
            string BarCode = Lip.GetValue("SELECT  [BarCode]  FROM  [Stc_ItemUnits] where [UnitCancel]=0 and  BranchID=" + MySession.GlobalBranchID+" and [ItemID]=" + ItemID + " and [SizeID]=" + SizeID);
            Grid.SetRowCellValue(Grid.FocusedRowHandle, FildNameBarCode, BarCode);
            decimal AverageCost = Comon.cDec(Lip.AverageUnit(ItemID, SizeID, StoreID));
            Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns["CostPrice"], AverageCost);


            decimal totalQtyBalance = 0;
            totalQtyBalance = Comon.ConvertToDecimalPrice(Lip.RemindQtyItemByMinUnit(ItemID, SizeID, StoreID));
            {
                decimal qtyCurrent = 0;
                decimal QtyInCommand = Lip.GetQTYinCommandToThisItem(TableNameDetails, TableNameMaster, FildNameQTY, FildNameCommmand, CommandID, ItemID.ToString(), Where, SizeID: SizeID,BarCode:FildNameBarCode);
                qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(Grid, FildNameQTY, 0, ItemID.ToString(), SizeID, FildNameBarCode);
                totalQtyBalance += QtyInCommand;
                totalQtyBalance -= qtyCurrent;
            }
            if (totalQtyBalance <= 0)
            {
                if (MySession.AllowOutQtyNegative)
                {
                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore);
                    Grid.DeleteRow(Grid.FocusedRowHandle);
                    return;
                }
                bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore + "هل تريد المتابعة ...");
                if (!yes)
                    return;
            }



            if (MySession.AllowNotShowQTYInQtyField == false)
                totalQtyBalance = 0;
            Grid.SetRowCellValue(Grid.FocusedRowHandle, Grid.Columns[FildNameQTY], totalQtyBalance);
            if (FildNameTotalCost.Trim() != "")
                Grid.SetRowCellValue(Grid.FocusedRowHandle, FildNameTotalCost, Comon.cDec(totalQtyBalance * AverageCost));

        }
        
         public static decimal GetQTYToItemFromGridView(GridView gridView, string fieldName, decimal thisQty, string itemID, int SizeID, string FildNameBarCode = "BarCode")
        {
            decimal totalqty = 0;
            int OrderSize = 0;
            decimal p = 0;
 

            DataTable mergedTable = new DataTable();
            mergedTable.Columns.Add("PackingQty", typeof(decimal));
            mergedTable.Columns.Add("SizeID", typeof(int));
            mergedTable.Columns.Add(fieldName, typeof(decimal));
            mergedTable.Columns.Add("ItemID", typeof(long));
            for (int i = 0; i < gridView.DataRowCount; i++)
            {
                if (i != gridView.FocusedRowHandle)
                mergedTable.Rows.Add(
                    Comon.cDec(0),
                    Comon.cInt(gridView.GetRowCellValue(i, "SizeID")),
                    Comon.cDec(gridView.GetRowCellValue(i, fieldName)),
                     Comon.cLong(gridView.GetRowCellValue(i, "ItemID"))
                );
            }
           
            DataTable aggregatedTable = new DataTable();
            aggregatedTable.Columns.Add("SizeID", typeof(int));
            aggregatedTable.Columns.Add( fieldName, typeof(decimal));
            aggregatedTable.Columns.Add("PackingQty", typeof(decimal));  
         
            var groups = mergedTable.AsEnumerable()
                .Where(row => Comon.cLong(row["ItemID"]) == Comon.cLong(itemID))
                .GroupBy(row => row.Field<int>("SizeID"))
                .Select(g =>
                {
                    var newRow = aggregatedTable.NewRow();
                    newRow["SizeID"] = g.Key;
                    newRow[fieldName] = g.Sum(row => row.Field<decimal>(fieldName));
                    newRow["PackingQty"] = g.First().Field<decimal>("PackingQty");  
                    return newRow;
                });
           
         
            foreach (var row in groups)
            {
                aggregatedTable.Rows.Add(row);
            }
           
            DataView dv = aggregatedTable.DefaultView;
            dv.Sort = "PackingQty ASC";
            DataTable sortedDT = dv.ToTable();

            string StrSQL = @"SELECT ItemID,SizeID,PackingQty , 0.0 AS "+fieldName+"  from  dbo.Stc_ItemUnits   WHERE  ItemID=" + itemID + " and  BranchID=" + MySession.GlobalBranchID+" and BranchID= " + MySession.GlobalBranchID+"    Order by     PackingQty  ";
            DataTable ItemdtBalance = Lip.SelectRecord(StrSQL);
            ItemdtBalance.Columns[fieldName].ReadOnly = false;
         
            for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
            {
                int SiziunitID = Comon.cInt(ItemdtBalance.Rows[j]["SizeID"].ToString());
                for (int k = 0; k <= sortedDT.Rows.Count - 1; k++)
                {
                    int sortedSizeID = Comon.cInt(sortedDT.Rows[k]["SizeID"].ToString());
                    if (SiziunitID == sortedSizeID)
                    {
                        p = Comon.cDec(sortedDT.Rows[k][fieldName].ToString());
                        ItemdtBalance.Rows[j][fieldName] = p.ToString();
                        
                        break;
                    }
                    if (SizeID == SiziunitID )
                        OrderSize = j;
                }
            }

            
            decimal Qty = 0;
            decimal Pakeg = 0;
            decimal rqTY = 0;
            decimal REmindQty = 0;
            decimal PakegUnits = 0;
         
            for (int j = 0; j <= ItemdtBalance.Rows.Count - 1; j++)
            {
                rqTY = Comon.cDec(ItemdtBalance.Rows[j][fieldName]);
                p = rqTY;
                int SiziunitID = Comon.cInt(ItemdtBalance.Rows[j]["SizeID"].ToString());
                if (SizeID == SiziunitID&&p!=0)
                {
                    totalqty += p;

                    return Comon.ConvertToDecimalPrice(totalqty + thisQty);
                }

                for (int i = j + 1; i <=  ItemdtBalance.Rows.Count - 1; i++)
                {

                    Pakeg = Comon.cDec(ItemdtBalance.Rows[i]["PackingQty"]);
                    p = p * Pakeg;
                    PakegUnits = Comon.cDec(ItemdtBalance.Rows[j]["PackingQty"]);
                }
              
                totalqty = Comon.cDec(totalqty + p);
               

            }
            for (int j = ItemdtBalance.Rows.Count - 1; j >= OrderSize + 1; j--)
            {

                int SiziunitID = Comon.cInt(ItemdtBalance.Rows[j]["SizeID"].ToString());
                 
                PakegUnits = Comon.cDec(ItemdtBalance.Rows[j]["PackingQty"]);
                totalqty = totalqty / PakegUnits;
            }

            REmindQty = Comon.ConvertToDecimalPrice(totalqty);
            return Comon.ConvertToDecimalPrice(REmindQty + thisQty);
            
        }

        #region Event
       
        private void GridCadWax_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if(this.GridCad.ActiveEditor is CheckEdit)
            {
                GridView view = sender as GridView;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "ShownInNext" &&Comon.cbool( e.Value)==true)
                {

                    int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils]  FROM  [Stc_Items] where [ItemID]=" + view.GetFocusedRowCellValue("ItemID") + " and  BranchID=" + MySession.GlobalBranchID+" and Cancel=0"));

                    if (isShow != 1)
                    {
                        //Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotSelectShowInDetilsOrder);
                        e.Value = false;
                        return;
                    }

                }
            }
            if (this.GridCad.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
               
                if (ColName == "BarCode" || ColName == "SizeID" || ColName == "CostPrice" || ColName == "ItemID" || ColName == "QTY")
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else if (!(double.TryParse(val.ToString(), out num)) && ColName != "BarCode")
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && ColName != "BarCode")
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }
                    else
                    {
                        e.Valid = true;
                        view.SetColumnError(GridCad.Columns[ColName], "");

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridCad.SetColumnError(GridCad.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "SizeID").ToString()), Comon.cDbl(txtBeforeStoreID.Text));
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Manu_CadWaxFactoryDetails", "Manu_CadWaxFactoryMaster", "QTY", "CommandID", Comon.cInt(txtCommandID.Text), GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "ItemID").ToString(), " and Manu_CadWaxFactoryMaster.TypeStageID=1","BarCode",Comon.cInt(GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "SizeID").ToString()));
                       totalQtyBalance += QtyInCommand;                      
                       decimal qtyCurrent = 0;
                       qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridCad, "QTY", Comon.cDec(val.ToString()), GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "SizeID").ToString()));
                       
                        if (qtyCurrent > totalQtyBalance)
                       {
                           Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                           e.Valid = false;
                           HasColumnErrors = true;
                           e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                           view.SetColumnError(GridCad.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                           return;
                               
                       }
                        if (MySession.AllowOutQtyNegative == true)
                        {
                            if (totalQtyBalance > 0)
                            {
                                if (Comon.cDec(val.ToString()) > totalQtyBalance)
                                {
                                    e.Valid = false;
                                    HasColumnErrors = true;
                                    e.ErrorText = Messages.msgQtyisNotAvilable + totalQtyBalance.ToString();
                                    view.SetColumnError(GridCad.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                                view.SetColumnError(GridCad.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                            }
                        }
                        decimal PriceUnit = Comon.cDec(GridCad.GetFocusedRowCellValue("CostPrice"));
                        decimal Qty = Comon.cDec(val.ToString());
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridCad.SetFocusedRowCellValue("TotalCost", Total.ToString());
                    }

                    if (ColName == "CostPrice")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridCad.SetColumnError(GridCad.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal PriceUnit = Comon.cDec(val.ToString());
                        decimal Qty = Comon.cDec(GridCad.GetFocusedRowCellValue("QTY"));
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridCad.SetFocusedRowCellValue("TotalCost", Total.ToString());

                    }
                  
                    if (ColName == "BarCode")
                    {
                        DataTable dt;
                        var flagb = false;
                        dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisBarCode;
                        }
                        else
                        {

                            if (flagb == true)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                            {
                               
                                FileItemData(dt);
                            }
                            if (HasColumnErrors == false)
                            {
                                e.Valid = true;
                                view.SetColumnError(GridCad.Columns[ColName], "");
                                GridCad.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                GridCad.FocusedColumn = GridCad.VisibleColumns[0];
                            }
                        }
                    }
                    else if (ColName == "ItemID")
                    {
                        DataTable dt = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(val.ToString()), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisBarCode;
                        }
                        else
                        {

                            if (MySession.GlobalAllowUsingDateItems)
                            {
                                MySession.GlobalAllowUsingDateItems = false;
                                FileItemData(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dt);
                            e.Valid = true;
                            view.SetColumnError(GridCad.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 and  BranchID=" + MySession.GlobalBranchID +" And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridCad.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridCad.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridCad.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }                    
                }
        
                else if (ColName == SizeName)
                {

                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  BranchID=" + MySession.GlobalBranchID+" and  Stc_Items.ItemID=" + Comon.cLong(GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtItemID = Lip.SelectRecord(Str);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridCad.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                         SetValuseWhenChangeSizeName(GridCad, Comon.cLong(GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtItemID.Rows[0]["SizeID"]), "Manu_CadWaxFactoryDetails", "Manu_CadWaxFactoryMaster",Comon.cDbl(txtBeforeStoreID.Text),Comon.cInt(txtCommandID.Text), "CommandID", Where: " and Manu_CadWaxFactoryMaster.TypeStageID=1", FildNameTotalCost: "TotalCost");
                        e.Valid = true;
                        view.SetColumnError(GridCad.Columns[ColName], "");

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridCad.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
              
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  ItemID from Stc_Items  Where Cancel =0 and LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  BranchID=" + MySession.GlobalBranchID);
                    DataTable dt = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(dtItemID.Rows[0]["ItemID"].ToString()), UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {

                        FileItemData(dt);

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود";
                    }
                }
               

            }
        }
        private void gridControl11_ProcessGridKey(object sender, KeyEventArgs e)
        {
            try
            {
                var grid = sender as GridControl;
                var view = grid.FocusedView as GridView;
                if (view.FocusedColumn == null)
                    return;
                if (e.KeyCode == Keys.Escape)
                {
                    HasColumnErrors = false;
                }

                if (e.KeyValue == 107)
                {
                    if (this.GridCad.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridCadWax.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));

                    }
                }
                else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {
                    if (view.ActiveEditor is TextEdit)
                    {
                        if (HasColumnErrors == true)
                            return;
                        double num;
                        HasColumnErrors = false;
                        var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                        string ColName = view.FocusedColumn.FieldName;
                        if (ColName == "BarCode" || ColName == "CostPrice" || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID")
                        {

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridCad.Columns[ColName], Messages.msgInputIsRequired);
                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridCad.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridCad.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridCad.Columns[ColName], "");
                            }
                        }

                    }
                }
                else if (e.KeyData == Keys.Delete)
                {
                    if (!IsNewRecord)
                    {
                        bool Yes = Messages.MsgStopYesNo(Messages.TitleConfirm, Messages.msgConfirmDelete);
                        if (!Yes)
                            return;
                    }
                    int index = view.FocusedRowHandle;
                    view.DeleteSelectedRows();
                    e.Handled = true;
                    if (index > 0)
                    {
                        if (index > 0)
                            index = index - 1;
                        else if (index < 0)
                        {
                            index = view.DataRowCount;
                            index = index - 1;
                        }
                        view.SelectRow(index);
                        view.FocusedRowHandle = index;
                    }
                    //CalculateRow();
                }
                else if (e.KeyData == Keys.F5)
                    grid.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                e.Handled = false;
                Messages.MsgError(Messages.TitleInfo, this.GetType().Name + " " + System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0 and  BranchID=" + MySession.GlobalBranchID+" and [UserID]=" + txtGuidanceID.Text.ToString();
                CSearch.ControlValidating(txtGuidanceID, lblGuidanceName, strSQL);
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
                    strSQL = "SELECT " + PrimaryName + " as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + " and  BranchID=" + MySession.GlobalBranchID;
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
        private void txtFactorID_Validating(object sender, CancelEventArgs e)
        {

            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtFactorID.Text) + " And Cancel =0 and  BranchID=" + MySession.GlobalBranchID;
                CSearch.ControlValidating(txtFactorID, lblFactorName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtCostCenterID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void txtCommandID_Validating(object sender, CancelEventArgs e)
        {

            if (FormView == true)
                ReadRecord(Comon.cInt(txtCommandID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }
        }
        private void txtBeforeStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtBeforeStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtBeforeStoreID, lblBeforeStoreName, strSQL);
                strSQL = "SELECT "+PrimaryName+" as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtBeforeStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
                
                string StoreManger = Lip.GetValue(strSQL).ToString();
                lblBeforeStoreManger.Text = StoreManger;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtAfterStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {   if(txtAfterStoreID.Text.Trim()!= ""&&Comon.cDbl(txtAfterStoreID.Text)>0)
                if (Comon.cDbl(txtBeforeStoreID.Text) == Comon.cDbl(txtAfterStoreID.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن التحويل الى نفس المخزن ": "Cann't transefer Between Him self Store");
                    return;
                }
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtAfterStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtAfterStoreID, lblAfterStoreName, strSQL);
                strSQL = "SELECT "+PrimaryName+" as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtAfterStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
                string StoreManger = Lip.GetValue(strSQL).ToString();
                lblAfterStoreManger.Text = StoreManger;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        #endregion

        #region Do Function 
        protected override void DoLast()
        {
            try
            {
                MoveRec(0, xMoveLast);
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
                MoveRec(0, xMoveFirst);
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
                MoveRec(Comon.cInt(txtCommandID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtCommandID.Text), xMovePrev);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoNew()
        {
            
            try
            {
                IsNewRecord = true;
                txtCommandID.Text = Manu_CadWaxFactoryDAL.GetNewID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue),Comon.cInt(cmbTypeStage.EditValue)).ToString();
                ClearFields();
                SetDefultValue();
                EnabledControl(true);
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        bool IsValidGrid()
        {
            double num;

            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            GridCad.MoveLast();

            int length = GridCad.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in GridCad.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
                    {

                        var cellValue = GridCad.GetRowCellValue(i, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            GridCad.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;
                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            GridCad.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            GridCad.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        protected override void DoSearch()
        {
            try
            {
                Find();
            }
            catch { }
        }
        protected override void DoSave()
        {
            try
            {

                if (!Validations.IsValidForm(this))
                    return;
                if (!IsValidGrid())
                    return;
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

                if (!Lip.CheckTheProcessesIsPosted("Manu_CadWaxFactoryMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtCommandID.Text),PrimeryColName:"CommandID",Where:" and  TypeStageID="+Comon.cInt(cmbTypeStage.EditValue)))
                   {
                        Messages.MsgWarning(Messages.TitleError, Messages.msgTheProcessIsNotUpdateBecuseIsPosted);
                        return;
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
                int TempID = Comon.cInt(txtCommandID.Text);

                Manu_CadWaxFactoryMaster model = new Manu_CadWaxFactoryMaster();
                model.CommandID = Comon.cInt(txtCommandID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                model.OrderID = txtOrderID.Text; 
                string Result = Manu_CadWaxFactoryDAL.Delete(model);
                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                    int MoveID = 0;
                    MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeCadFactory); 

                    if (MoveID < 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                }

                #region Delete Voucher Machin
                //حذف القيد الالي
                if (Comon.cInt(Result) > 0)
                {
                    int VoucherID = 0;

                    VoucherID = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeCadFactory);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية   ");
                }
                #endregion
                SplashScreenManager.CloseForm(false);
                if (Comon.cInt(Result) > 0)
                {
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                    ClearFields();
                    txtCommandID.Text = model.CommandID.ToString();
                    MoveRec(model.CommandID, xMovePrev);
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

       

        int DeleteStockMoving(int DocumentID, int DocumentType)
        {
            int Result = -1;
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.DocumentTypeID = DocumentType;
            objRecord.TranseID = DocumentID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            Result = Stc_ItemsMoviingDAL.Delete(objRecord);
            return Result;

        }
        int DeleteVariousVoucherMachin(int DocumentID, int DocumentType)
        {
            int VoucherID = 0;
            int Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentType;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(Comon.cInt(cmbBranchesID.EditValue))));
            objRecord.VoucherID = VoucherID;
            objRecord.EditUserID = UserInfo.ID;
            objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            Result = VariousVoucherMachinDAL.DeleteAcc_VariousVoucherMachinMaster(objRecord);
            return Result;

        }
        protected override void DoEdit()
        {

            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("CommandID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BranchID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("Fingerprint", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));


            dtItem.Columns.Add("SizeID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("ShownInNext", System.Type.GetType("System.Boolean"));

            for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["CommandID"] = Comon.cInt(txtCommandID.Text);
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                dtItem.Rows[i]["BarCode"] = GridCad.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(GridCad.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i][ItemName] = GridCad.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtItem.Rows[i][SizeName] = GridCad.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["SizeID"] = Comon.cInt(GridCad.GetRowCellValue(i, "SizeID").ToString());
                //dtItem.Rows[i]["TypeOpration"] = Comon.cInt(GridCad.GetRowCellValue(i, "TypeOpration").ToString());
                dtItem.Rows[i]["Fingerprint"] = Comon.cInt(GridCad.GetRowCellValue(i, "Fingerprint").ToString());
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "QTY").ToString());

                dtItem.Rows[i]["TotalCost"] = Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "TotalCost").ToString());
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["ShownInNext"] = Comon.cbool(GridCad.GetRowCellValue(i, "ShownInNext").ToString());
            }
            gridControl1.DataSource = dtItem;

            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
        }
        #endregion

        #region Function 
        private void FileItemData(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                if (Stc_itemsDAL.CheckIfStopItemUnit(dt.Rows[0]["BarCode"].ToString(), MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                {

                    Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                    GridCad.DeleteRow(GridCad.FocusedRowHandle);
                    return;
                }
                decimal totalQtyBalance = 0;
            
                totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtBeforeStoreID.Text));
                {
                    decimal qtyCurrent = 0;
                    decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Manu_CadWaxFactoryDetails", "Manu_CadWaxFactoryMaster", "QTY", "CommandID", Comon.cInt(txtCommandID.Text), dt.Rows[0]["ItemID"].ToString(), " and Manu_CadWaxFactoryMaster.TypeStageID=1","BarCode",Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                    qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridCad, "QTY", 0, dt.Rows[0]["ItemID"].ToString(), Comon.cInt(dt.Rows[0]["SizeID"].ToString()));

                    totalQtyBalance += QtyInCommand;
                    totalQtyBalance -= qtyCurrent;
                }   
                if (totalQtyBalance <= 0)
                    {
                        if (MySession.AllowOutQtyNegative)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore);
                            GridCad.DeleteRow(GridCad.FocusedRowHandle);
                            return;
                        }
                        bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore + "هل تريد المتابعة ...");
                        if (!yes)
                            return;
                    }
                 
                
                
                if (MySession.AllowNotShowQTYInQtyField == false)
                    totalQtyBalance = 0;
                
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["QTY"], totalQtyBalance);  
           
                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                //GridCad.Columns[SizeName].ColumnEdit = rSize;
                //gridControl1.RepositoryItems.Add(rSize);

                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                if(UserInfo.Language==iLanguage.English)
                     GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns[SizeName], dt.Rows[0][SizeName].ToString());
                else
                    GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns[ItemName], dt.Rows[0][PrimaryName].ToString());
                decimal AverageCost = Comon.cDec(Lip.AverageUnit(Comon.cInt(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtBeforeStoreID.Text)));
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["CostPrice"], AverageCost);
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["TotalCost"], AverageCost * totalQtyBalance);
            }
            else
            {
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["Qty"], "0");
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["SizeID"], "");
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns[SizeName], "");
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["BarCode"], "");
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["ItemID"], "");
                GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns[ItemName], "");
 
            }
        }
        List<Manu_AllOrdersDetails> SaveOrderDetials()
        {
            
            Manu_AllOrdersDetails returned = new Manu_AllOrdersDetails();
            List<Manu_AllOrdersDetails> listreturned = new List<Manu_AllOrdersDetails>();
            for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
            {
                returned = new Manu_AllOrdersDetails();
                returned.ID = i+1;
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.BarCode = GridCad.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridCad.GetRowCellValue(i, "ItemID").ToString());
                returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                returned.SizeID = Comon.cInt(GridCad.GetRowCellValue(i, "SizeID").ToString());
                returned.ArbSizeName = GridCad.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridCad.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridCad.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridCad.GetRowCellValue(i, ItemName).ToString();
                returned.QTY = Comon.ConvertToDecimalQty(GridCad.GetRowCellValue(i, "QTY").ToString());
                returned.CostPrice = Comon.cDbl(GridCad.GetRowCellValue(i, "CostPrice").ToString());
                returned.TotalCost = Comon.cDbl(GridCad.GetRowCellValue(i, "TotalCost").ToString());
                returned.ShownInNext =Comon.cbool( GridCad.GetRowCellValue(i, "ShownInNext").ToString());
                listreturned.Add(returned);
            }
            return listreturned;
        }

        private void Save()
        {
            GridCad.Focus();
            GridCad.MoveLastVisible();
            GridCad.FocusedColumn = GridCad.VisibleColumns[1];
            Manu_CadWaxFactoryMaster objRecord = new Manu_CadWaxFactoryMaster();
            objRecord.CommandID = Comon.cInt(txtCommandID.Text);
            objRecord.OrderID = txtOrderID.Text;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.DateBefore = Comon.ConvertDateToSerial(txtBeforeDate.Text);
            objRecord.DateAfter = Comon.ConvertDateToSerial(txtAfterDate.Text);
            objRecord.StoreIDBefore = Comon.cDbl(txtBeforeStoreID.Text);
            objRecord.StoreIDAfter = Comon.cDbl(txtAfterStoreID.Text);
            objRecord.StoreMangerBefore = lblBeforeStoreManger.Text;
            objRecord.StoreMangerAfter = lblAfterStoreManger.Text;
            objRecord.PeriodDay = Comon.cInt(lblPeriod.Text);
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            objRecord.FactorID = Comon.cDbl(txtFactorID.Text);
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text); 
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            
            txtNotes.Text = (txtNotes.Text.Trim());
            objRecord.Notes = txtNotes.Text; 
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
            objRecord.TypeStageID =  Comon.cInt(cmbTypeStage.EditValue);
            if (IsNewRecord == false)
            {
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }
            Manu_CadWaxFactoryDetails returned;
            List<Manu_CadWaxFactoryDetails> listreturned = new List<Manu_CadWaxFactoryDetails>();
            for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
            {
                returned = new Manu_CadWaxFactoryDetails();
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue); 
                returned.BarCode = GridCad.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridCad.GetRowCellValue(i, "ItemID").ToString());
                 
                returned.SizeID = Comon.cInt(GridCad.GetRowCellValue(i, "SizeID").ToString());
                returned.ArbSizeName = GridCad.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridCad.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridCad.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridCad.GetRowCellValue(i, ItemName).ToString();
               
                returned.QTY = Comon.ConvertToDecimalQty(GridCad.GetRowCellValue(i, "QTY").ToString());
                returned.CostPrice = Comon.cDbl(GridCad.GetRowCellValue(i, "CostPrice").ToString());
                returned.TotalCost = Comon.cDbl(GridCad.GetRowCellValue(i, "TotalCost").ToString());
                returned.ShownInNext = Comon.cbool(GridCad.GetRowCellValue(i, "ShownInNext").ToString());
                listreturned.Add(returned);
            }
            int lengthPrentage = GridCad.DataRowCount;
            if (listreturned.Count > 0)
            {
                objRecord.Manu_CadWaxFactorys = listreturned;
                objRecord.Manu_OrderDetils = SaveOrderDetials();
                string Result = Manu_CadWaxFactoryDAL.InsertUsingXML(objRecord, IsNewRecord);
                if (Comon.cInt(Result) > 0 &&Comon.cInt(cmbStatus.EditValue)>1)     
                {   
                    if (lengthPrentage > 0)
                    {
                        bool isNew = true;
                        DataTable dtCount = null;
                        dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeCadFactory);
                        if (Comon.cInt(dtCount.Rows[0][0]) > 0)
                            isNew = false;
                        // حفظ الحركة المخزنية 
                        int MoveID = SaveStockMoveing(Comon.cInt(Result));
                        if (MoveID == 0)
                             Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
                        //حفظ القيد الالي
                        if (Comon.cInt(Result) > 0)
                        {
                            //حفظ القيد الالي
                            long VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result), isNew);
                            if (VoucherID == 0)
                                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        }
                    }                  
                }
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
                        Validations.DoSaveRipon(this, ribbonControl1);
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
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        Validations.DoReadRipon(this, ribbonControl1);
                    }
                    else
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave + " " + Result);
                    }
                }
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);
            }
        }
        long SaveVariousVoucherMachin(int DocumentID,bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeCadFactory;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtOrderDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            //objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            //objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            // objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);

            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial()); ;
            objRecord.ComputerInfo = UserInfo.ComputerInfo;
            objRecord.EditUserID = 0;
            objRecord.EditTime = 0;
            objRecord.EditDate = 0;
            objRecord.EditComputerInfo = "";
            if (isNew == false)
            {
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }
            Acc_VariousVoucherMachinDetails returned;
            List<Acc_VariousVoucherMachinDetails> listreturned = new List<Acc_VariousVoucherMachinDetails>();

            //Debit Gold         

            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAfterStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.DebitMatirial = Comon.cDbl(txtTotalQTY.Text);
            returned.Debit = Comon.cDbl(txtTotalCostPrice.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);


            //Credit Gold      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtBeforeStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(txtTotalCostPrice.Text);
            returned.CreditMatirial = Comon.cDbl(txtTotalQTY.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);


             
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        private int SaveStockMoveing(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeCadFactory;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtOrderDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeCadFactory;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtBeforeStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAfterStoreID.Text);
                returned.BarCode = GridCad.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridCad.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridCad.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and  BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridCad.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = 0;
          
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = Comon.cDbl(GridCad.GetRowCellValue(i, "CostPrice").ToString());
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                returned.Cancel = 0;
                listreturned.Add(returned);
            }
            for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtOrderDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeCadFactory;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtAfterStoreID.Text);
                returned.AccountID = Comon.cDbl(txtBeforeStoreID.Text.ToString());
                returned.BarCode = GridCad.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridCad.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridCad.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and  BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridCad.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = Comon.cDbl(GridCad.GetRowCellValue(i, "CostPrice").ToString()); ;
       
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = 0;
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                returned.Cancel = 0;
                listreturned.Add(returned);
            }
            if (listreturned.Count > 0)
            {

                objRecord.ObjDatails = listreturned;
                string Result = Stc_ItemsMoviingDAL.Insert(objRecord, IsNewRecord);

                return Comon.cInt(Result);
            }
            return 0;
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
            foreach (GridColumn col in GridCad.Columns)
            {
                //if (col.FieldName == "BarCode")
                {

                    GridCad.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridCad.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridCad.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }

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
            else if (FocusedControl.Trim() == txtCommandID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CadCommend", "رقـم الأمر", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CadCommend", "Commend ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == txtOrderID.Name)
            {
                if (MySession.GlobalDefaultCanRepetUseOrderOneOureMoreBeforeCasting == true)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderIDCadWithCondtion", "رقم الطلب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderIDCadWithCondtion", "Order ID", MySession.GlobalBranchID);
                }
                else
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderIDCad", "رقم الطلب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderIDCad", "Order ID", MySession.GlobalBranchID);
                }
            }
             
            else if (FocusedControl.Trim() == txtFactorID.Name)
            {
                
                if (!MySession.GlobalAllowChangefrmCadEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCadCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }

            else if (FocusedControl.Trim() == txtBeforeStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCadBeforeStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBeforeStoreID, lblBeforeStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtBeforeStoreID, lblBeforeStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtAfterStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCadAfterStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAfterStoreID, lblAfterStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtAfterStoreID, lblAfterStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            
            


            else if (FocusedControl.Trim() == gridControl1.Name)
            {
                if (GridCad.FocusedColumn == null) return;
                if (GridCad.FocusedColumn.Name == "colBarCode" || GridCad.FocusedColumn.Name == "colItemName" || GridCad.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }              
                else if (GridCad.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridCad.FocusedColumn.Name == "colQTY")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem(); 
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                      
                        frm.Show();
                        frm.SetValueToControl(GridCad.GetRowCellValue(GridCad.FocusedRowHandle, "ItemID").ToString(), txtBeforeStoreID.Text.ToString());
                    }
                    else
                        frm.Dispose();
                }
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

                if (FocusedControl == txtCommandID.Name)
                {
                    txtCommandID.Text = cls.PrimaryKeyValue.ToString();
                    txtCommandID_Validating(null, null);
                }
                else if (FocusedControl == txtGuidanceID.Name)
                {
                    txtGuidanceID.Text = cls.PrimaryKeyValue.ToString();
                    txtGuidanceID_Validating(null, null);
                }

                else if (FocusedControl ==txtOrderID.Name)
                    {
                        txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                        txtOrderID_Validating(null, null);
                    }

                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }
                else if (FocusedControl == txtFactorID.Name)
                {
                    txtFactorID.Text = cls.PrimaryKeyValue.ToString();
                    txtFactorID_Validating(null, null);
                }
                
                if (FocusedControl == txtBeforeStoreID.Name)
                {
                    txtBeforeStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtBeforeStoreID_Validating(null, null);
                }
                if (FocusedControl == txtAfterStoreID.Name)
                {
                    txtAfterStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtAfterStoreID_Validating(null, null);
                }
                else if (FocusedControl == gridControl1.Name)
                {
                    if (GridCad.FocusedColumn.Name == "colBarCode" || GridCad.FocusedColumn.Name == "colItemName" || GridCad.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridCad.AddNewRow();

                        GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));

                        // CalculateRow();
                    }
                     
                    if (GridCad.FocusedColumn.Name == "colSizeID")
                    {
                        GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridCad.SetRowCellValue(GridCad.FocusedRowHandle, GridCad.Columns[SizeName], Lip.GetValue(strSQL));
                    }
                }
            }
        }
        #endregion
        private void frmCadWaxFactory_Load(object sender, EventArgs e)
        {
            try
            {
                initGrid();
                DoNew();
       
                txtCustomerID.ReadOnly = true;
                txtDelegateID.ReadOnly = true;
                txtOrderDate.ReadOnly = true;
                txtGuidanceID.ReadOnly = true;
                cmbTypeOrders.ReadOnly = true;
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        

        private void frmCadWaxFactory_KeyDown(object sender, KeyEventArgs e)
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


            if (FocusedControl.Trim() == txtBeforeStoreID.Name|| FocusedControl.Trim() ==txtAfterStoreID.Name)
            {
                frmStores frm = new frmStores();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                frmCostCenter frm = new frmCostCenter();
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
            else if (FocusedControl.Trim() == txtFactorID.Name)
            {
                frmEmployeeFiles frm = new frmEmployeeFiles();              
                if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }

            else if (FocusedControl.Trim() == gridControl1.Name)
            {

                if (GridCad.FocusedColumn.Name == "colItemID" || GridCad.FocusedColumn.Name == "col" + ItemName || GridCad.FocusedColumn.Name == "colBarCode")
                {
                    frmItems frm = new frmItems();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                         frm.Show();
                        {
                            bool b = true;
                        };
                        //frm.Dispose();
                        if (frm.IsDisposed)
                        {
                            RepositoryItemLookUpEdit rItem = Common.LookUpEditItemName();
                            GridCad.Columns[ItemName].ColumnEdit = rItem;
                            gridControl1.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridCad.FocusedColumn.Name == "colSizeName" || GridCad.FocusedColumn.Name == "colSizeID")
                {
                    frmSizingUnits frm = new frmSizingUnits();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);
                        frm.Show();
                    }
                    else
                        frm.Dispose();
                }
            }
        }
        private void txtAfterDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Lip.CheckDateISAvilable(txtAfterDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                txtBeforeDate.Text = Lip.GetServerDate();
                return;
            }
            if (string.IsNullOrEmpty(txtAfterDate.Text) == false && string.IsNullOrEmpty(txtBeforeDate.Text) == false)
            {
                long dateDiff = Comon.DateDiff(Comon.DateInterval.Day, txtBeforeDate.DateTime, txtAfterDate.DateTime);
                lblPeriod.Text = Comon.cLong(dateDiff).ToString();
            }
            else
            { lblPeriod.Text = ""; }
        }

        private void txtBeforeDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Lip.CheckDateISAvilable(txtBeforeDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                txtBeforeDate.Text = Lip.GetServerDate();
                return;
            }

            if (string.IsNullOrEmpty(txtBeforeDate.Text) == false && string.IsNullOrEmpty(txtAfterDate.Text) == false)
            {
                long dateDiff = Comon.DateDiff(Comon.DateInterval.Day, txtBeforeDate.DateTime, txtAfterDate.DateTime);
                lblPeriod.Text = Comon.cLong(dateDiff).ToString();
            }
            else
            { lblPeriod.Text = ""; }
        }

        private void btnFactory_Click(object sender, EventArgs e)
        {
            int TypeID = Comon.cInt(Lip.GetValue("SELECT  [TypeID] FROM  [Manu_OrderRestriction] where OrderID='"+txtOrderID.Text +"'  and Cancel=0 and BranchID="+Comon.cInt(cmbBranchesID.EditValue)));
            if ( TypeID == 1)
            {
                frmZirconeFactory frm = new frmZirconeFactory();
                if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                    frm.txtOrderID.Text = txtOrderID.Text;
                    frm.txtOrderID_Validating(null, null);
                    frm.txtBeforeDate.DateTime = txtAfterDate.DateTime;
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
                    frm.txtBeforeDate.DateTime = txtAfterDate.DateTime;
                }
                else
                    frm.Dispose();
            }
        }

        private void btnMachinResractionFactoryBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypeCadFactory).ToString());
            if (ID > 0)
            {
                frmVariousVoucherMachin frm22 = new frmVariousVoucherMachin();
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm22);
                frm22.FormView = true;
                frm22.FormAdd = false;
                frm22.Show();
                frm22.cmbBranchesID.EditValue = Comon.cInt(cmbBranchesID.EditValue);
                frm22.ReadRecord(Comon.cLong(ID.ToString()));
            }
            else
                Messages.MsgError("تنبيه", "   لا يوجد قيد - الرجاء اعادة حفظ المستند ");
        }

        public XtraReport Manu_CadStage(GridView Grid)
        {
            string rptrptManu_FactoryFactorCommendName = "‏‏‏‏rptManu_FactoryBeforeCastingStage";
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\1\";
            //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
            rptrptManu_FactoryFactorCommendName += "Arb";
            XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);

            var dataTable = new dsReports.rptManu_FactoryBeforeCastingStageDataTable();
            for (int i = 0; i <= Grid.DataRowCount - 1; i++)
            {
                var row = dataTable.NewRow();
                row["#"] = i + 1;
                row["QTY"] = Grid.GetRowCellValue(i, "QTY");
                row["StoreName"] = Grid.GetRowCellValue(i, "StoreName");
                row["ItemID"] = Grid.GetRowCellValue(i, "ItemID");
                row["ItemName"] = Grid.GetRowCellValue(i, ItemName);
                row["CostPrice"] = Grid.GetRowCellValue(i, "CostPrice");
                row["SizeName"] = Grid.GetRowCellValue(i, SizeName);
                row["DateBefore"] = Grid.GetRowCellValue(i, "BarCode");
                row["DateAfter"] = Grid.GetRowCellValue(i, "DateAfter");
                row["EmpName"] = Grid.GetRowCellValue(i, "TotalCost");
                dataTable.Rows.Add(row);
            }
            rptFactoryFactor.DataSource = dataTable;
            rptFactoryFactor.DataMember = "rptManu_FactoryBeforeCastingStage";
            return rptFactoryFactor;
        }
        protected override void DoPrint()
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
                ReportName = "rptManu_FactoryCadOpretion";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;

                rptForm.Parameters["OrderID"].Value = txtOrderID.Text;
                rptForm.Parameters["OrderDate"].Value = txtOrderDate.Text;
                rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text;
                rptForm.Parameters["DelegetName"].Value = lblDelegateName.Text;
                rptForm.Parameters["GuidanceName"].Value = lblGuidanceName.Text;
                rptForm.Parameters["TypeOrder"].Value = cmbTypeOrders.Text;

                rptForm.Parameters["BranchesID"].Value = cmbBranchesID.Text;
                rptForm.Parameters["BeforeStoreName"].Value = lblBeforeStoreName.Text;
                rptForm.Parameters["BeforeStoreManger"].Value = lblBeforeStoreManger.Text;
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text;

                rptForm.Parameters["FactorName"].Value = lblFactorName.Text;
                rptForm.Parameters["Curency"].Value = cmbCurency.Text;
                rptForm.Parameters["TypeStage"].Value = cmbTypeStage.Text;
                rptForm.Parameters["BeforeDate"].Value = txtBeforeDate.Text.ToString();
                rptForm.Parameters["Posted"].Value = cmbStatus.Text;
                rptForm.Parameters["Notes"].Value = txtNotes.Text;
                rptForm.Parameters["AfterStoreName"].Value = lblAfterStoreName.Text;
                rptForm.Parameters["AfterStoreManger"].Value = lblAfterStoreManger.Text;
                rptForm.Parameters["AfterDate"].Value = txtAfterDate.Text;
                rptForm.Parameters["Period"].Value = lblPeriod.Text;
                rptForm.Parameters["TotalQTY"].Value = txtTotalQTY.Text;
                rptForm.Parameters["TotalCostPrice"].Value = txtTotalCostPrice.Text;

                /********************** Details ****************************/
                decimal TotalDiamond = 0;
                decimal TotalZircon = 0;
                decimal TotalBagit = 0;
                int Base = 0;

                rptForm.Parameters["Daimond"].Value = TotalDiamond;
                rptForm.Parameters["Zircone"].Value = TotalZircon;
                rptForm.Parameters["BAGET"].Value = TotalBagit;
                rptForm.DataMember = ReportName;
                /******************** Report Binding ************************/
                XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
                subreport.Visible = IncludeHeader;
                subreport.ReportSource = ReportComponent.CompanyHeader();

                /******************** Report Before Casting Stages ************************/
                XRSubreport subreportBeforeCasting = (XRSubreport)rptForm.FindControl("rptManu_FactoryCadStagesArb", true);
                subreportBeforeCasting.Visible = IncludeHeader;
                subreportBeforeCasting.ReportSource = Manu_CadStage(GridCad);


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

        private void txtAfterStoreID_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and  BranchID=" + MySession.GlobalBranchID));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and  BranchID=" + MySession.GlobalBranchID));
                txtCurrncyPrice.Text = CurrncyPrice + "";
                lblCurrncyPric.Visible = true;
                txtCurrncyPrice.Visible = true;

            }
            else
            {
                txtCurrncyPrice.Text = "1"; 
                lblCurrncyPric.Visible = false; 
                txtCurrncyPrice.Visible = false;
            }
        }
    }
}
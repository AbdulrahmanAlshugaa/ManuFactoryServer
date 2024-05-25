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
using Edex.Model.Language;
using Edex.DAL.ManufacturingDAL;
using System.Globalization;
using Edex.Model;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.Utils;
using DevExpress.XtraSplashScreen;
using Edex.ModelSystem;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.DAL.Accounting;
using Edex.AccountsObjects.Transactions;
using DevExpress.XtraGrid.Views.BandedGrid;
using Edex.AccountsObjects.Codes;
using Edex.HR.Codes;
using Edex.StockObjects.Codes;
using Permissions = Edex.ModelSystem.Permissions;
using DevExpress.XtraReports.UI;
using Edex.StockObjects.Transactions;

namespace Edex.Manufacturing.Codes
{
   
    public partial class frmDiamondFactory : BaseForm
    {
        #region
        public int DocumentTypeDiamondFactory = 28;
        BindingList<Manu_ZirconDiamondFactoryDetails> lstDetail = new BindingList<Manu_ZirconDiamondFactoryDetails>();
        BindingList<Menu_FactoryOrderDetails> lstOrderDetails = new BindingList<Menu_FactoryOrderDetails>();

        private bool IsNewRecord;
        private string strSQL;
        private string PrimaryName;
        string FocusedControl = "";
        private Manu_ZirconDiamondFactoryDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        private string ItemName;
        private string SizeName;
        int rowIndex = 0;
        private string CaptionItemName;
        public CultureInfo culture = new CultureInfo("en-US");
        public bool HasColumnErrors = false;
        private DataTable dt;
        int BaseID = 0;
        decimal TotalDiamond = 0;
        decimal TotalMatirial = 0;
        
        #endregion
        public frmDiamondFactory()
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
            this.GridDiamondFactory.CustomDrawCell += GridDiamondFactory_CustomDrawCell;
            txtGuidanceID.Validating += txtGuidanceID_Validating;
            txtOrderID.Validating += txtOrderID_Validating;
            txtAfterStoreID.Validating += txtAfterStoreID_Validating;
            txtBeforeStoreID.Validating += txtBeforeStoreID_Validating;
            txtFactorID.Validating += txtFactorID_Validating;
            txtCostCenterID.Validating += txtCostCenterID_Validating;
            this.gridControl1.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl1_ProcessGridKey);
            this.GridDiamondFactory.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridDiamondFactory_ValidatingEditor);
            FillCombo.FillComboBox(cmbTypeOrders, "Manu_TypeOrders", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", "BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;

            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
            cmbStatus.EditValueChanging += cmbStatus_EditValueChanging;
            this.GridDiamondFactory.RowUpdated += GridDiamondFactory_RowUpdated;

            FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", PrimaryName, "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbTypeStage.EditValue = 4;
            cmbTypeStage.ReadOnly = true;

           
            txtCustomerID.ReadOnly = true;
            txtDelegateID.ReadOnly = true;
            txtOrderDate.ReadOnly = true;
            txtGuidanceID.ReadOnly = true;
            cmbTypeOrders.ReadOnly = true;
            this.GridDiamondFactory.CellValueChanging+=GridDiamondFactory_CellValueChanging;
            EnableControlDefult();
        }
        void EnableControlDefult()
        {
            txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmDiamondCostCenterID;
            cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmDiamondCurrncyID;
            txtBeforeDate.ReadOnly = !MySession.GlobalAllowChangefrmDiamondCommandDate;
            txtAfterStoreID.ReadOnly = !MySession.GlobalAllowChangefrmDiamondAfterStoreID;
            txtBeforeStoreID.ReadOnly = !MySession.GlobalAllowChangefrmDiamondBeforeStoreID;
            txtFactorID.ReadOnly = !MySession.GlobalAllowChangefrmDiamondEmployeeID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowChangefrmDiamondBranchID;
        }
        void SetDefultValue()
        {
            txtCostCenterID.Text = MySession.GlobalDefaultDiamondCostCenterID;
            txtCostCenterID_Validating(null, null);
            cmbCurency.EditValue = MySession.GlobalDefaultDiamondCurencyID;
            txtAfterStoreID.Text = MySession.GlobalDefaultDiamondAfterStoreAccontID;
            txtAfterStoreID_Validating(null, null);
            txtBeforeStoreID.Text = MySession.GlobalDefaultDiamondBeforeStoreAccontID;
            txtBeforeStoreID_Validating(null, null);
            txtFactorID.Text = MySession.GlobalDefaultDiamondEmpolyeeID;
            txtFactorID_Validating(null, null);
        }
        void GridDiamondFactory_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (((view.GetRowCellValue(e.RowHandle, "ItemID") == null) || Comon.cInt(view.GetRowCellValue(e.RowHandle, "ItemID")) <= 0) && e.Column.FieldName == "ShownInNext")
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء اضافة صنف ومن ثم تفعيل الخيار" : "Please Add Item and selcet option");
                    return;
                }
                if (e.Column.FieldName == "ShownInNext")
                {
                    if (Comon.cbool(e.Value) == true)
                    {
                        int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils] FROM [Stc_Items] WHERE [ItemID] = " + view.GetRowCellValue(e.RowHandle, "ItemID") + " AND Cancel = 0 and BranchID=" + MySession.GlobalBranchID));
                        if (isShow != 1)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotSelectShowInDetilsOrder);
                            view.SetRowCellValue(e.RowHandle, "ShownInNext", false);
                        }
                    }
                    SendKeys.Send("\t");
                }
            }
            catch (Exception ex) { Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message); }

        }
        void GridDiamondFactory_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            CalculateTotal();
        }
        void CalculateTotal()
        {
            try
            {
                decimal ToatlQty = 0;
                decimal ToatlCostPrice = 0;
                TotalMatirial = 0;
                TotalDiamond = 0;
                for (int i = 0; i <= GridDiamondFactory.DataRowCount - 1; i++)
                {
                    ToatlQty += Comon.cDec(GridDiamondFactory.GetRowCellValue(i, "QTY").ToString());
                    ToatlCostPrice += Comon.ConvertToDecimalPrice(GridDiamondFactory.GetRowCellValue(i, "TotalCost").ToString());
                    BaseID = Comon.cInt(Lip.GetValue("SELECT [BaseID]  FROM  [Stc_Items] where  Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  and ItemID =" + GridDiamondFactory.GetRowCellValue(i, "ItemID")));
                    decimal TempQTY = 0;
                    if (BaseID > 1 && BaseID <= 3)
                    {
                        TempQTY = 0;
                        if (Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "SizeID").ToString()) == 2)
                            TempQTY = Comon.cDec(GridDiamondFactory.GetRowCellValue(i, "QTY").ToString()) / 5;
                        else
                            TempQTY = Comon.cDec(GridDiamondFactory.GetRowCellValue(i, "QTY").ToString());
                       
                        TotalDiamond += TempQTY;// Comon.cDec(GridDiamondFactory.GetRowCellValue(i, "QTY"));
                    }
                    else
                        TotalMatirial += Comon.cDec(GridDiamondFactory.GetRowCellValue(i, "QTY"));
                }
                txtMatiralQTY.Text = TotalMatirial.ToString();
                txtDiamondQTY.Text = TotalDiamond.ToString();
                txtTotalQTY.Text = ToatlQty.ToString();
                txtTotalCostPrice.Text = ToatlCostPrice.ToString();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void cmbStatus_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Comon.cInt(cmbStatus.EditValue) <=1)
                txtAfterStoreID.Tag = "isNumber";
            else
                txtAfterStoreID.Tag = "ImportantFieldGreaterThanZero";


        }

        void GridDiamondFactory_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "ShownInNext")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                GridDiamondFactory.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridDiamondFactory.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;

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
                    strSQL = "SELECT TOP 1 *  FROM " + Manu_ZirconDiamondFactoryDAL.TableName + " Where Cancel =0  and TypeStageID="+Comon.cInt(cmbTypeStage.EditValue)+"  And BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_ZirconDiamondFactoryDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Manu_ZirconDiamondFactoryDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Manu_ZirconDiamondFactoryDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Manu_ZirconDiamondFactoryDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Manu_ZirconDiamondFactoryDAL.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_ZirconDiamondFactoryDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Manu_ZirconDiamondFactoryDAL();

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
        private void GetOrderDetail(string OrderID)
        {

            DataTable dt = Manu_ZirconDiamondFactoryDAL.frmGetDataDetailByOrderID(OrderID, Comon.cInt(MySession.GlobalBranchID), UserInfo.FacilityID,Comon.cInt(cmbTypeStage.EditValue));

            if (dt.Rows.Count > 0)
            {
                gridControlOrderDetails.DataSource = lstOrderDetails;
                if (dt.Rows.Count > 0)
                {
                    gridControlOrderDetails.DataSource = dt;
                }
            }
        }
        public void txtOrderID_Validating(object sender, CancelEventArgs e)
        {

            if (FormView == true)
            {
                if (String.IsNullOrEmpty(txtOrderID.Text) == false)
                {
                    string txtOrder = txtOrderID.Text;
                    int i = Comon.cInt(Lip.GetValue("SELECT [TypeID]  FROM  [Manu_OrderRestriction] where [OrderID]='" + txtOrderID.Text + "' and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
                    if (i != 2 && i>0)
                    {
                        Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "نوع الطلبية المدخلة لا يتناسب مع الامر الحالي " : "The type of order entered does not match the current order");
                        txtOrderID.Text = "";
                        return;
                    }

                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt(Lip.GetValue("select CommandID from Manu_ZirconDiamondFactoryMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and CommandID<>" + Comon.cInt(txtCommandID.Text) + " and OrderID='" + txtOrderID.Text + "'"));
                    int CommandIDThis = Comon.cInt(Lip.GetValue("select CommandID from Manu_ZirconDiamondFactoryMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and CommandID=" + Comon.cInt(txtCommandID.Text) + " and OrderID='" + txtOrderID.Text + "'"));

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
                        //txtOrder = txtOrderID.Text;
                        //ClearFields();
                        //string OrderID = txtOrder;
                        //DoNew();
                        //txtOrderID.Text = OrderID;
                        if (CommandIDTemp > 0)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheOrderAlreadyExists);

                        ReadTopInfo(txtOrderID.Text);
                        GetOrderDetail(txtOrderID.Text);
                        //IsNewRecord = true;
                        Validations.DoEditRipon(this, ribbonControl1);
                    }
                   // && CommandIDTemp <= 0
                    if (IsNewRecord)
                    {
                        if (CommandIDTemp > 0)
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheOrderAlreadyExists);

                        string OrderID = txtOrder;
                        strSQL = "SELECT * FROM Manu_OrderRestriction WHERE  OrderID ='" + OrderID.Trim() + "' and BranchID=" + MySession.GlobalBranchID;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                        System.Data.DataTable dtt = Lip.SelectRecord(strSQL);
                        if (dtt.Rows.Count > 0)
                        {
                            ReadTopInfo(txtOrderID.Text);
                            GetOrderDetail(txtOrderID.Text);
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
                            Messages.MsgError("تنبيه", "   لا يوجد طلبية  بهذا الرقم   ");
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
        //public void txtOrderID_Validating(object sender, CancelEventArgs e)
        //{

        //    if (FormView == true)
        //    {
        //        if (String.IsNullOrEmpty(txtOrderID.Text) == false)
        //        {
        //            if (MySession.GlobalDefaultCanRepetUseOrderOneOureMoreBeforeCasting == true && IsNewRecord)
        //            {
        //                string OrderID = "";
        //                OrderID = Lip.GetValue("select OrderID from Manu_ZirconDiamondFactoryMaster where Cancel=0 and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + " and CommandID<>" + Comon.cInt(txtCommandID.Text) + " and OrderID='" + txtOrderID.Text + "'"); 
        //                if (OrderID.Trim() != "")
        //                {
        //                    Messages.MsgWarning(Messages.TitleWorning, Messages.msgDontRepetTheOrderinMoreCommend);
        //                    txtOrderID.Text = "";
        //                    return;
        //                }
        //            }
        //            int i = Comon.cInt(Lip.GetValue("SELECT [TypeID]  FROM  [Manu_OrderRestriction] where [OrderID]='" + txtOrderID.Text + "' and Cancel=0"));
        //            if (i != 2)
        //            {
        //                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "نوع الطلبية المدخلة لا يتناسب مع الامر الحالي " : "The type of order entered does not match the current order");
        //                txtOrderID.Text = "";
        //                return;
        //            }
        //            ReadTopInfo(txtOrderID.Text);
        //            GetOrderDetail(txtOrderID.Text);

        //        }
        //    }
        //    else
        //    {
        //        Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
        //        return;
        //    }
        //}
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
            catch (Exception ex)
            { Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message); }
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
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultDiamondCurencyID);
                txtBeforeStoreID.Text = "";
                txtBeforeStoreID_Validating(null, null);
                lstDetail = new BindingList<Manu_ZirconDiamondFactoryDetails>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl1.DataSource = lstDetail;
                ClearFieldsTop();
                txtOrderID.Text = "";
                lstOrderDetails.AllowNew = true;
                lstOrderDetails.AllowEdit = true;
                lstOrderDetails.AllowRemove = true;
                gridControlOrderDetails.DataSource = lstOrderDetails;
                dt = new DataTable();
                txtOrderID.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void initGridOrderDetails()
        {

            lstOrderDetails = new BindingList<Menu_FactoryOrderDetails>();
            lstOrderDetails.AllowNew = true;
            lstOrderDetails.AllowEdit = true;
            lstOrderDetails.AllowRemove = true;
            gridControlOrderDetails.DataSource = lstOrderDetails;


            GridViewOrderDetails.Columns["ID"].Visible = false;
            GridViewOrderDetails.Columns["ComandID"].Visible = false;
            GridViewOrderDetails.Columns["BarCode"].Visible = false;
            GridViewOrderDetails.Columns["EmpPolishnID"].Visible = false;
            GridViewOrderDetails.Columns["EmpPrentagID"].Visible = false;
            GridViewOrderDetails.Columns["Cancel"].Visible = false;
            GridViewOrderDetails.Columns["BranchID"].Visible = false;
            GridViewOrderDetails.Columns["FacilityID"].Visible = false;
            GridViewOrderDetails.Columns["SizeID"].Visible = false;
            GridViewOrderDetails.Columns["EditUserID"].Visible = false;
            GridViewOrderDetails.Columns["EditDate"].Visible = false;
            GridViewOrderDetails.Columns["EditTime"].Visible = false;
            GridViewOrderDetails.Columns["RegDate"].Visible = false;
            GridViewOrderDetails.Columns["UserID"].Visible = false;

            GridViewOrderDetails.Columns["ComputerInfo"].Visible = false;
            GridViewOrderDetails.Columns["EditComputerInfo"].Visible = false;
            GridViewOrderDetails.Columns["RegTime"].Visible = false;

            GridViewOrderDetails.Columns["Credit"].Visible = false;
            //GridViewBeforfactory.Columns["SizeID"].Visible = false;
            GridViewOrderDetails.Columns["CostPrice"].Visible = false;

            GridViewOrderDetails.Columns["EmpName"].Width = 120;
            GridViewOrderDetails.Columns["StoreName"].Width = 120;
            GridViewOrderDetails.Columns["EmpID"].Width = 120;
            GridViewOrderDetails.Columns["Signature"].Width = 120; 

            GridViewOrderDetails.Columns["EmpID"].Visible = false;
            GridViewOrderDetails.Columns["EmpName"].Visible = false;
            GridViewOrderDetails.Columns["StoreID"].Visible = false;
            GridViewOrderDetails.Columns["StoreName"].Visible = false; 
            GridViewOrderDetails.Columns["Signature"].Visible = false;
            GridViewOrderDetails.Columns["DIAMOND_WG"].Visible = false;
            GridViewOrderDetails.Columns["DIAMOND_WC"].Visible = false;
            GridViewOrderDetails.Columns["DebitDate"].Visible = false;
            GridViewOrderDetails.Columns["DebitTime"].Visible = false;
            GridViewOrderDetails.Columns["TypeOpration"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridViewOrderDetails.Columns["EngItemName"].Visible = false;
                GridViewOrderDetails.Columns["EngSizeName"].Visible = false;
                GridViewOrderDetails.Columns["EngStateName"].Visible = false;
                GridViewOrderDetails.Columns["ArbItemName"].Width = 150;
                GridViewOrderDetails.Columns["StoreID"].Caption = "رقم المخزن";
                GridViewOrderDetails.Columns["StoreName"].Caption = "إسم المخزن";
                GridViewOrderDetails.Columns["EmpID"].Caption = "رقم العامل";
                GridViewOrderDetails.Columns["EmpName"].Caption = "إسم العامل";

                GridViewOrderDetails.Columns["QTY"].Caption = "الوزن";

                GridViewOrderDetails.Columns["Credit"].Caption = "دائــن";
                GridViewOrderDetails.Columns["Signature"].Caption = "التوقيع";

                GridViewOrderDetails.Columns["ItemID"].Caption = "رقم الصنف";
                GridViewOrderDetails.Columns["ArbItemName"].Caption = "اسم الصنف";
                GridViewOrderDetails.Columns["SizeID"].Caption = "رقم الوحده";
                GridViewOrderDetails.Columns["ArbSizeName"].Caption = "الوحده";
                GridViewOrderDetails.Columns["CostPrice"].Caption = "التكلفة"; 
                GridViewOrderDetails.Columns["PeriodDay"].Caption = "الأيام  ";
                GridViewOrderDetails.Columns["StateName"].Caption = "المرحلة ";
            }
            else
            {
                GridViewOrderDetails.Columns["EngItemName"].Visible = true;
                GridViewOrderDetails.Columns["EngSizeName"].Visible = true;
                GridViewOrderDetails.Columns["StateName"].Visible = false;
                GridViewOrderDetails.Columns["ArbItemName"].Visible = false;
                GridViewOrderDetails.Columns["ArbSizeName"].Visible = false;
                GridViewOrderDetails.Columns["StoreID"].Caption = "Store ID";
                GridViewOrderDetails.Columns["StoreName"].Caption = "Store Name";

            }
            GridViewOrderDetails.OptionsBehavior.ReadOnly = true;
            GridViewOrderDetails.OptionsBehavior.Editable = false;
        }
        void initGrid()
        {

            lstDetail = new BindingList<Manu_ZirconDiamondFactoryDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;

            gridControl1.DataSource = lstDetail;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits  where BranchID=" + MySession.GlobalBranchID);
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameUnit[i] = dtitems.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControl1.RepositoryItems.Add(riComboBoxitems);
            GridDiamondFactory.Columns[SizeName].ColumnEdit = riComboBoxitems;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl1.RepositoryItems.Add(riComboBoxitems4);
            GridDiamondFactory.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridDiamondFactory.Columns["CommandID"].Visible = false;
            GridDiamondFactory.Columns["BranchID"].Visible = false;
            GridDiamondFactory.Columns["FacilityID"].Visible = false;
            GridDiamondFactory.Columns["ArbItemName"].Visible = GridDiamondFactory.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridDiamondFactory.Columns["EngItemName"].Visible = GridDiamondFactory.Columns["EngItemName"].Name == "col" + ItemName ? true : false;

            GridDiamondFactory.Columns["TotalCost"].OptionsColumn.ReadOnly = false;

            GridDiamondFactory.Columns[ItemName].Visible = true;
            GridDiamondFactory.Columns[ItemName].Caption = CaptionItemName;
            GridDiamondFactory.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            GridDiamondFactory.Columns["TotalCost"].OptionsColumn.AllowFocus = false;
            GridDiamondFactory.Columns["SizeID"].Visible = false;

            GridDiamondFactory.Columns[ItemName].Width = 150;
            GridDiamondFactory.Columns[SizeName].Width = 120;

            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridDiamondFactory.Columns["EngItemName"].Visible = false;
                GridDiamondFactory.Columns["EngSizeName"].Visible = false;
                GridDiamondFactory.Columns["BarCode"].Caption = "باركود الصنف";
                GridDiamondFactory.Columns["SizeID"].Caption = "رقم الوحدة";
                GridDiamondFactory.Columns["ItemID"].Caption = "رقم الصنــف";

                GridDiamondFactory.Columns[SizeName].Caption = "إسم الوحدة";
                GridDiamondFactory.Columns["QTY"].Caption = "الكمية ";
                GridDiamondFactory.Columns["CostPrice"].Caption = "القيمة";
                GridDiamondFactory.Columns["TotalCost"].Caption = "الإجمالي ";
                GridDiamondFactory.Columns["Fingerprint"].Caption = "البصمــة";
                GridDiamondFactory.Columns["ShownInNext"].Caption = "يظهر في التفاصيل"; 
            }
            else
            {
                GridDiamondFactory.Columns["ArbItemName"].Visible = false;
                GridDiamondFactory.Columns["ArbSizeName"].Visible = false;
                GridDiamondFactory.Columns["BarCode"].Caption = "BarCode";
                GridDiamondFactory.Columns["SizeID"].Caption = "Unit ID";
                GridDiamondFactory.Columns["ItemID"].Caption = "Item ID";
                GridDiamondFactory.Columns[SizeName].Caption = "Unit Name ";
                GridDiamondFactory.Columns["CostPrice"].Caption = "Cost Price";
                GridDiamondFactory.Columns["QTY"].Caption = "QTY";
                GridDiamondFactory.Columns["TotalCost"].Caption = "Total Cost ";
                GridDiamondFactory.Columns["Fingerprint"].Caption = "Fingerprint";
                GridDiamondFactory.Columns["ShownInNext"].Caption = "Shown In Next"; 
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
                        string DateBefore = Lip.GetValue("SELECT [DateAfter]    FROM  [Manu_CadWaxFactoryMaster] where  [OrderID]=" + txtOrderID.Text + "  and [Posted]=3 and BranchID=" + MySession.GlobalBranchID);
                        if (DateBefore.Trim() != "")
                            txtBeforeDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(DateBefore.ToString()), "dd/MM/yyyy", culture);
                        else
                            txtBeforeDate.DateTime = DateTime.Now; 
                    }
                    else
                    {
                        Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد طلبية تمتلك هذا الرقم .. الرجاء ادخال رقم الطلبية الصحيح" : "There is no order that has this number. Please enter the correct order number");
                        txtOrderID.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void ReadRecord(int CommendID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                    dt = Manu_ZirconDiamondFactoryDAL.frmGetDataDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,Comon.cInt(cmbTypeStage.EditValue));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;

                        txtCommandID.Text = dt.Rows[0]["CommandID"].ToString();

                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        cmbStatus.EditValue = Comon.cInt(dt.Rows[0]["Posted"].ToString());
                        txtBeforeStoreID.Text = Comon.cDbl(dt.Rows[0]["StoreIDBefore"]).ToString();
                        txtBeforeStoreID_Validating(null, null);
                        txtAfterStoreID.Text = Comon.cDbl(dt.Rows[0]["StoreIDAfter"]).ToString();

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
                        GetOrderDetail(txtOrderID.Text);
                        Validations.DoReadRipon(this, ribbonControl1);
                        txtAfterDate_EditValueChanged(null, null);
                        CalculateTotal();
                        EnabledControl(false);
                        //ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtCommandID.Text;
                    }
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #region Event
        private void GridDiamondFactory_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (this.GridDiamondFactory.ActiveEditor is CheckEdit)
            {
                GridView view = sender as GridView;
                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "ShownInNext" && Comon.cbool(e.Value) == true)
                {

                    int isShow = Comon.cInt(Lip.GetValue("SELECT [ShowInOrderDetils]  FROM  [Stc_Items] where [ItemID]=" + view.GetFocusedRowCellValue("ItemID") + "   and Cancel=0 and BranchID=" + MySession.GlobalBranchID));

                    if (isShow != 1)
                    {
                        //Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotSelectShowInDetilsOrder);
                        e.Value = false;
                        return;
                    }

                }
            }
            if (this.GridDiamondFactory.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "BarCode" || ColName == "SizeID" || ColName == "ItemID" || ColName == "QTY" || ColName == "CostPrice")
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
                        view.SetColumnError(GridDiamondFactory.Columns[ColName], "");
                    }
                    if (ColName == "CostPrice")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridDiamondFactory.SetColumnError(GridDiamondFactory.Columns["QTY"], "");
                        e.ErrorText = "";
                        decimal PriceUnit = Comon.cDec(val.ToString());
                        decimal Qty = Comon.cDec(GridDiamondFactory.GetFocusedRowCellValue("QTY"));
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridDiamondFactory.SetFocusedRowCellValue("TotalCost", Total.ToString());

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridDiamondFactory.SetColumnError(GridDiamondFactory.Columns["QTY"], "");
                        e.ErrorText = "";
                        decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "SizeID")), Comon.cDbl(txtBeforeStoreID.Text));
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Manu_ZirconDiamondFactoryDetails", "Manu_ZirconDiamondFactoryMaster", "QTY", "CommandID", Comon.cInt(txtCommandID.Text), GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "ItemID").ToString(), " and Manu_ZirconDiamondFactoryMaster.TypeStageID=4",SizeID:Comon.cInt(GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "SizeID").ToString()));
                        totalQtyBalance += QtyInCommand;
                        decimal qtyCurrent = 0;
                        qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridDiamondFactory, "QTY", Comon.cDec(val.ToString()), GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "SizeID")));
                        
                        if (qtyCurrent > totalQtyBalance)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                            view.SetColumnError(GridDiamondFactory.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
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
                                    view.SetColumnError(GridDiamondFactory.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                                view.SetColumnError(GridDiamondFactory.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                            }
                        }
                        decimal PriceUnit = Comon.cDec(GridDiamondFactory.GetFocusedRowCellValue("CostPrice"));
                        decimal Qty = Comon.cDec(val.ToString());
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridDiamondFactory.SetFocusedRowCellValue("TotalCost", Total.ToString());
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
                                FileItemData(dt);
                            if (HasColumnErrors == false)
                            {
                                e.Valid = true;
                                view.SetColumnError(GridDiamondFactory.Columns[ColName], "");
                                GridDiamondFactory.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                GridDiamondFactory.FocusedColumn = GridDiamondFactory.VisibleColumns[0];
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
                            view.SetColumnError(GridDiamondFactory.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridDiamondFactory.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridDiamondFactory.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridDiamondFactory.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
                
                else if (ColName == SizeName)
                {

                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and Stc_Items.BranchID=Stc_ItemUnits.BranchID  left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID  Where UnitCancel=0 and Stc_Items.BranchID=" + MySession.GlobalBranchID + " And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtItemID = Lip.SelectRecord(Str);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridDiamondFactory.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                        frmCadFactory.SetValuseWhenChangeSizeName(GridDiamondFactory, Comon.cLong(GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtItemID.Rows[0]["SizeID"]), "Manu_ZirconDiamondFactoryDetails", "Manu_ZirconDiamondFactoryMaster", Comon.cDbl(txtBeforeStoreID.Text), Comon.cInt(txtCommandID.Text), "CommandID", Where: " and Manu_ZirconDiamondFactoryMaster.TypeStageID=4", FildNameTotalCost: "TotalCost");
                        e.Valid = true;
                        view.SetColumnError(GridDiamondFactory.Columns[ColName], "");

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridDiamondFactory.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  ItemID from Stc_Items  Where Cancel =0 and BranchID=" + MySession.GlobalBranchID+" and LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
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
        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.GridDiamondFactory.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridDiamondFactory.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));

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
                        if (ColName == "BarCode"  || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID")
                        {

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridDiamondFactory.Columns[ColName], Messages.msgInputIsRequired);
                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridDiamondFactory.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridDiamondFactory.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridDiamondFactory.Columns[ColName], "");
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
                strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0 and BranchID=" + MySession.GlobalBranchID+" and [UserID]=" + txtGuidanceID.Text.ToString();
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
        private void txtFactorID_Validating(object sender, CancelEventArgs e)
        {

            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtFactorID.Text) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID;
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
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtBeforeStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
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
            {
                if (txtAfterStoreID.Text.Trim() != ""&&Comon.cDbl(txtAfterStoreID.Text)>0)
                    if (Comon.cDbl(txtBeforeStoreID.Text) == Comon.cDbl(txtAfterStoreID.Text))
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن التحويل الى نفس المخزن ": "Cann't transefer Between Him self Store");
                    return;
                }
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtAfterStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtAfterStoreID, lblAfterStoreName, strSQL);
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtAfterStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
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
                txtCommandID.Text = Manu_ZirconDiamondFactoryDAL.GetNewID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbTypeStage.EditValue)).ToString();
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

            GridDiamondFactory.MoveLast();

            int length = GridDiamondFactory.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in GridDiamondFactory.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
                    {

                        var cellValue = GridDiamondFactory.GetRowCellValue(i, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            GridDiamondFactory.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;
                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            GridDiamondFactory.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            GridDiamondFactory.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
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
            catch (Exception ex) { Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message); }
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
                if (!Lip.CheckTheProcessesIsPosted("Manu_ZirconDiamondFactoryMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtCommandID.Text), PrimeryColName: "CommandID", Where: " and  TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue)))
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

                Manu_ZirconDiamondFactoryMaster model = new Manu_ZirconDiamondFactoryMaster();
                model.CommandID = Comon.cInt(txtCommandID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                model.OrderID = txtOrderID.Text; 
                string Result = Manu_ZirconDiamondFactoryDAL.Delete(model);
                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                    int MoveID = 0;
                    MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text),DocumentTypeDiamondFactory );

                    if (MoveID < 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                }

                #region Delete Voucher Machin
                //حذف القيد الالي
                if (Comon.cInt(Result) > 0)
                {
                    int VoucherID = 0;

                    VoucherID = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeDiamondFactory);
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
            for (int i = 0; i <= GridDiamondFactory.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["CommandID"] = Comon.cInt(txtCommandID.Text);
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                dtItem.Rows[i]["BarCode"] = GridDiamondFactory.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i][ItemName] = GridDiamondFactory.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtItem.Rows[i][SizeName] = GridDiamondFactory.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["SizeID"] = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "SizeID").ToString());
                //dtItem.Rows[i]["TypeOpration"] = Comon.cInt(GridZircon.GetRowCellValue(i, "TypeOpration").ToString());
                dtItem.Rows[i]["Fingerprint"] = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "Fingerprint").ToString());
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(GridDiamondFactory.GetRowCellValue(i, "QTY").ToString());

                dtItem.Rows[i]["TotalCost"] = Comon.ConvertToDecimalPrice(GridDiamondFactory.GetRowCellValue(i, "TotalCost").ToString());
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(GridDiamondFactory.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["ShownInNext"] = Comon.cbool(GridDiamondFactory.GetRowCellValue(i, "ShownInNext").ToString());
            }
            gridControl1.DataSource = dtItem;

            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
        }
        #endregion

        #region Function
        private void FileItemData(DataTable dt,string QTY="")
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                if (Stc_itemsDAL.CheckIfStopItemUnit(dt.Rows[0]["BarCode"].ToString(), MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                {

                    Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                    GridDiamondFactory.DeleteRow(GridDiamondFactory.FocusedRowHandle);
                    return;
                }
                decimal totalQtyBalance = 0;
                totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtBeforeStoreID.Text));
                //This Script To Update QTY
                {
                    decimal qtyCurrent = 0;
                    decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Manu_ZirconDiamondFactoryDetails", "Manu_ZirconDiamondFactoryMaster ", "QTY", "CommandID", Comon.cInt(txtCommandID.Text), dt.Rows[0]["ItemID"].ToString(), " and Manu_ZirconDiamondFactoryMaster.TypeStageID=4",SizeID:Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                    qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridDiamondFactory, "QTY", 0, dt.Rows[0]["ItemID"].ToString(), Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                    totalQtyBalance += QtyInCommand;
                    totalQtyBalance -= qtyCurrent;
                }
                if (totalQtyBalance <= 0)
                    {
                        if (MySession.AllowOutQtyNegative)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore);
                            GridDiamondFactory.DeleteRow(GridDiamondFactory.FocusedRowHandle);
                            return;
                        }
                        bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore + "هل تريد المتابعة ...");
                        if (!yes)
                            return;
                    }
                 if (MySession.AllowNotShowQTYInQtyField == false)
                    totalQtyBalance = 0;
                if (QTY != "")
                    totalQtyBalance = Comon.cDec(QTY);              
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["QTY"], totalQtyBalance);              
                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                //GridDiamondFactory.Columns[SizeName].ColumnEdit = rSize;
                //gridControl1.RepositoryItems.Add(rSize);
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                if(UserInfo.Language==iLanguage.English)
                   GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns[SizeName], dt.Rows[0][SizeName].ToString());
                else
                    GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns[ItemName], dt.Rows[0][PrimaryName].ToString());
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["CommandID"], txtCommandID.Text.ToString());
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["BranchID"],cmbBranchesID.EditValue.ToString());
                decimal AverageCost = Comon.cDec(Lip.AverageUnit(Comon.cInt(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtBeforeStoreID.Text)));
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["CostPrice"], AverageCost);
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["TotalCost"], AverageCost * totalQtyBalance);
            }
            else
            {
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["Qty"], "0");
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["SizeID"], "");
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns[SizeName], "");
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["BarCode"], "");
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["ItemID"], "");
                GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns[ItemName], "");

            }
        }
        List<Manu_AllOrdersDetails> SaveOrderDetials()
        {

            Manu_AllOrdersDetails returned = new Manu_AllOrdersDetails();
            List<Manu_AllOrdersDetails> listreturned = new List<Manu_AllOrdersDetails>();
            for (int i = 0; i <= GridDiamondFactory.DataRowCount - 1; i++)
            {
                returned = new Manu_AllOrdersDetails();
                returned.ID = i + 1;
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.BarCode = GridDiamondFactory.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "ItemID").ToString());
                returned.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                returned.SizeID = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "SizeID").ToString());
                returned.ArbSizeName = GridDiamondFactory.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridDiamondFactory.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridDiamondFactory.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridDiamondFactory.GetRowCellValue(i, ItemName).ToString();
                returned.QTY = Comon.ConvertToDecimalQty(GridDiamondFactory.GetRowCellValue(i, "QTY").ToString());
                returned.CostPrice = Comon.cDbl(GridDiamondFactory.GetRowCellValue(i, "CostPrice").ToString());
                returned.TotalCost = Comon.cDbl(GridDiamondFactory.GetRowCellValue(i, "TotalCost").ToString());
                if (GridDiamondFactory.GetRowCellValue(i, "ShownInNext") != null)
                    returned.ShownInNext = Comon.cbool(GridDiamondFactory.GetRowCellValue(i, "ShownInNext").ToString());
                listreturned.Add(returned);
            }
            return listreturned;
        }
        private void Save()
        {
            GridDiamondFactory.Focus();
            GridDiamondFactory.MoveLastVisible();
            GridDiamondFactory.FocusedColumn = GridDiamondFactory.VisibleColumns[1];
            Manu_ZirconDiamondFactoryMaster objRecord = new Manu_ZirconDiamondFactoryMaster();
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
            objRecord.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
            if (IsNewRecord == false)
            {
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }
            Manu_ZirconDiamondFactoryDetails returned;
            List<Manu_ZirconDiamondFactoryDetails> listreturned = new List<Manu_ZirconDiamondFactoryDetails>();
            for (int i = 0; i <= GridDiamondFactory.DataRowCount - 1; i++)
            {
                returned = new Manu_ZirconDiamondFactoryDetails();
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.BarCode = GridDiamondFactory.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "ItemID").ToString());

                returned.SizeID = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "SizeID").ToString());
                returned.ArbSizeName = GridDiamondFactory.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridDiamondFactory.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridDiamondFactory.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridDiamondFactory.GetRowCellValue(i, ItemName).ToString();
               
                returned.QTY = Comon.ConvertToDecimalQty(GridDiamondFactory.GetRowCellValue(i, "QTY").ToString());
                returned.CostPrice = Comon.cDbl(GridDiamondFactory.GetRowCellValue(i, "CostPrice").ToString());
                returned.TotalCost = Comon.cDbl(GridDiamondFactory.GetRowCellValue(i, "TotalCost").ToString());
                if (GridDiamondFactory.GetRowCellValue(i, "ShownInNext") != null)
                    returned.ShownInNext = Comon.cbool(GridDiamondFactory.GetRowCellValue(i, "ShownInNext").ToString());
                listreturned.Add(returned);
            }
            int lengthDiamond = GridDiamondFactory.DataRowCount;
            if (listreturned.Count > 0)
            {
                objRecord.Manu_CadWaxFactorys = listreturned;

                objRecord.Manu_OrderDetils = SaveOrderDetials();
                string Result = Manu_ZirconDiamondFactoryDAL.InsertUsingXML(objRecord, IsNewRecord);
                if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                {
                    if (lengthDiamond > 0)
                    {
                        bool isNew = true;
                        DataTable dtCount = null;
                        dtCount = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeDiamondFactory);
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
                        // txtCommandID_Validating(null, null);
                        //EnabledControl(false);
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        //if (Comon.cInt(cmbMethodID.EditValue) == 5)
                        //SaveVariousVoucher();
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
        long SaveVariousVoucherMachin(int DocumentID, bool IsNewRecord)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeDiamondFactory;
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
            if (IsNewRecord == false)
            {
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }
            Acc_VariousVoucherMachinDetails returned;
            List<Acc_VariousVoucherMachinDetails> listreturned = new List<Acc_VariousVoucherMachinDetails>();

            //Debit        
            
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAfterStoreID.Text);
            returned.VoucherID = VoucherID;

            returned.DebitMatirial = Comon.cDbl(TotalMatirial);
            returned.DebitDiamond = Comon.cDbl(TotalDiamond);
            returned.Debit = Comon.cDbl(txtTotalCostPrice.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);


            //Credit    
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtBeforeStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(txtTotalCostPrice.Text);
            returned.CreditMatirial = Comon.cDbl(TotalMatirial);
            returned.CreditDiamond = Comon.cDbl(TotalDiamond);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);


            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, IsNewRecord);
            }
            return Result;
        }
        private int SaveStockMoveing(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeDiamondFactory;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridDiamondFactory.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtOrderDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeDiamondFactory;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtBeforeStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAfterStoreID.Text);
                returned.BarCode = GridDiamondFactory.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridDiamondFactory.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = 0;
            
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = Comon.cDbl(GridDiamondFactory.GetRowCellValue(i, "CostPrice").ToString());
                returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);
                returned.Cancel = 0;
                listreturned.Add(returned);
            }
            for (int i = 0; i <= GridDiamondFactory.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtOrderDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeDiamondFactory;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtAfterStoreID.Text);
                returned.AccountID = Comon.cDbl(txtBeforeStoreID.Text.ToString());
                returned.BarCode = GridDiamondFactory.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridDiamondFactory.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID));
                returned.QTY = Comon.cDbl(GridDiamondFactory.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = Comon.cDbl(GridDiamondFactory.GetRowCellValue(i, "CostPrice").ToString());
               
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
            foreach (GridColumn col in GridDiamondFactory.Columns)
            {
                //if (col.FieldName == "BarCode")
                {

                    GridDiamondFactory.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridDiamondFactory.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridDiamondFactory.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
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
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "DiamondCommendFactory", "رقـم الأمر", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "DiamondCommendFactory", "Commend ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == txtOrderID.Name)
            {
                if (MySession.GlobalDefaultCanRepetUseOrderOneOureMoreBeforeCasting == true)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "رقم الطلب", MySession.GlobalBranchID, " and TypeID="+2+" and OrderID not in(select OrderID from Manu_ZirconDiamondFactoryMaster where Cancel=0 and BranchID="+MySession.GlobalBranchID+"  and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ")");
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "Order ID", MySession.GlobalBranchID, " and TypeID=" + 2+ " and OrderID not in(select OrderID from Manu_ZirconDiamondFactoryMaster where Cancel=0 and BranchID="+MySession.GlobalBranchID+"  and TypeStageID=" + Comon.cInt(cmbTypeStage.EditValue) + ")");
                }
                else
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "رقم الطلب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "Order ID", MySession.GlobalBranchID);
                }
            }

            else if (FocusedControl.Trim() == txtFactorID.Name)
            {
                if (!MySession.GlobalAllowChangefrmDiamondEmployeeID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmDiamondCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == txtBeforeStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmDiamondBeforeStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBeforeStoreID, lblBeforeStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtBeforeStoreID, lblBeforeStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtAfterStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmDiamondAfterStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAfterStoreID,lblAfterStoreName , "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtAfterStoreID, lblAfterStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == gridControl1.Name)
            {
                if (GridDiamondFactory.FocusedColumn == null) return;
                if (GridDiamondFactory.FocusedColumn.Name == "colBarCode" || GridDiamondFactory.FocusedColumn.Name == "colItemName" || GridDiamondFactory.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                else if (GridDiamondFactory.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridDiamondFactory.FocusedColumn.Name == "colQTY")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        frm.SetValueToControl(GridDiamondFactory.GetRowCellValue(GridDiamondFactory.FocusedRowHandle, "ItemID").ToString(), txtBeforeStoreID.Text.ToString());
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
                else if (FocusedControl == txtOrderID.Name)
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
                    if (GridDiamondFactory.FocusedColumn.Name == "colBarCode" || GridDiamondFactory.FocusedColumn.Name == "colItemName" || GridDiamondFactory.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {
                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridDiamondFactory.AddNewRow();

                        GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));

                        // CalculateRow();
                    }

                    if (GridDiamondFactory.FocusedColumn.Name == "colSizeID")
                    {
                        GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridDiamondFactory.SetRowCellValue(GridDiamondFactory.FocusedRowHandle, GridDiamondFactory.Columns[SizeName], Lip.GetValue(strSQL));
                    }
                }
            }
        }
        #endregion
        private void frmZirconeDiamondFactory_Load(object sender, EventArgs e)
        {
            try
            {
            initGrid();
            initGridOrderDetails();
            DoNew();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        private void frmZirconeDiamondFactory_KeyDown(object sender, KeyEventArgs e)
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


            if (FocusedControl.Trim() == txtBeforeStoreID.Name || FocusedControl.Trim() == txtAfterStoreID.Name)
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

                if (GridDiamondFactory.FocusedColumn.Name == "colItemID" || GridDiamondFactory.FocusedColumn.Name == "col" + ItemName || GridDiamondFactory.FocusedColumn.Name == "colBarCode")
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
                            GridDiamondFactory.Columns[ItemName].ColumnEdit = rItem;
                            gridControl1.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (GridDiamondFactory.FocusedColumn.Name == "colSizeName" || GridDiamondFactory.FocusedColumn.Name == "colSizeID")
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
                txtAfterDate.Text = Lip.GetServerDate();
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

            frmAfforestationFactory frm = new frmAfforestationFactory();
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

        private void btnMachinResractionFactoryBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + txtCommandID.Text + " And DocumentType=" + DocumentTypeDiamondFactory).ToString());
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

        private void btnDims_Click(object sender, EventArgs e)
        {
            frmManufacturingDismantOrders frm = new frmManufacturingDismantOrders();
            if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
            {
                if (UserInfo.Language == iLanguage.English)
                    ChangeLanguage.EnglishLanguage(frm);
                frm.Show();
                frm.txtOrderID.Text = txtOrderID.Text;
                frm.txtOrderID_Validating(null, null);
                frm.SetDataFromStageBefore(txtBeforeStoreID.Text, Comon.cInt(cmbTypeStage.EditValue), Comon.cInt(txtCommandID.Text), Comon.cInt(cmbCurency.EditValue));

            }
            else
                frm.Dispose();
        }

        public XtraReport Manu_DiamondStage(GridView Grid)
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
                ReportName = "rptManu_FactoryDaimondOpretion";
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
                subreportBeforeCasting.ReportSource = Manu_DiamondStage(GridDiamondFactory);

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

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID));
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
        bool ChekOrderIsFoundInGrid(string OrderID)
        {
            for (int i = 0; i <= GridDiamondFactory.DataRowCount - 1; i++)
            {
                if (GridDiamondFactory.GetRowCellValue(i, "BrCode") != null && GridDiamondFactory.GetRowCellValue(i, "BrCode").ToString().Trim() != "")
                if (GridDiamondFactory.GetRowCellValue(i, "BarCode").ToString() == OrderID)
                    return true;
            }
            if (rowIndex < 0)
            {
                if (GridDiamondFactory.GetRowCellValue(rowIndex, "BrCode") != null && GridDiamondFactory.GetRowCellValue(rowIndex, "BrCode").ToString().Trim() != "")
                {
                    object BarCode = GridDiamondFactory.GetRowCellValue(rowIndex, "BarCode");
                    if (((string)BarCode) == OrderID)
                        return true;
                }
            }
            return false;
        }
        private void GridViewOrderDetails_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (view.GetRowCellValue(view.FocusedRowHandle, "BarCode").ToString().Trim() != "")
                {
                    string BarCode = view.GetRowCellValue(view.FocusedRowHandle, "BarCode").ToString().Trim();
                    DataTable dt;
                    dt = Stc_itemsDAL.GetItemData(BarCode, UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {
                        if (ChekOrderIsFoundInGrid(BarCode))
                        {
                            Messages.MsgAsterisk(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الصنف موجود  لذلك لا يمكن انزاله  اكثر من مرة " : "This Item is Found Table");
                            return;
                        }
                        
                        string QTY = view.GetRowCellValue(view.FocusedRowHandle, "QTY").ToString();
                        GridDiamondFactory.AddNewRow();
                        FileItemData(dt,QTY);
                        //SendKeys.Send("\t");

                    }

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }

        

        private void GridDiamondFactory_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }
    }
}
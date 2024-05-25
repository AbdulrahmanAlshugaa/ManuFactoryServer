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
using DevExpress.XtraSplashScreen;
using Edex.ModelSystem;
using DevExpress.XtraEditors.Repository;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraGrid.Views.Grid;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraGrid;
using Edex.DAL;
using DevExpress.XtraGrid.Columns;
using Edex.DAL.ManufacturingDAL;
using DevExpress.Utils;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.DAL.Accounting;
using DevExpress.ExpressApp;
using System.Security.Policy;
using DevExpress.XtraExport.Helpers;
using Edex.HR.Codes;
using Edex.StockObjects.Codes;
using Permissions = Edex.ModelSystem.Permissions;
using DevExpress.XtraReports.UI;
using Edex.StockObjects.Transactions;
using System.Globalization;

namespace Edex.Manufacturing.Codes
{
    public partial class frmCasting : BaseForm
    {
        #region Declare
        BindingList<Manu_ManufacturingCastingDetails> lstDetailCastingBefore = new BindingList<Manu_ManufacturingCastingDetails>();
        BindingList<Manu_ManufacturingCastingDetails> lstDetailCastingAfter = new BindingList<Manu_ManufacturingCastingDetails>();
        BindingList<Manu_OrderRestriction> lstDetailOrders = new BindingList<Manu_OrderRestriction>();
        public int DocumentTypeBefore = 30;
        public int DocumentTypeAfter = 31;
        string FocusedControl = "";
        private string strSQL = "";
        private string PrimaryName;
        private DataTable dt;
        private DataTable dt1;
        private DataTable dt2;
        private bool IsNewRecord;
        public bool HasColumnErrors = false;
        int rowIndex;
        private Manu_ManufacturingCastingDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public CultureInfo culture = new CultureInfo("en-US");
        private string ItemName;
        private string CaptionItemName;
        private string SizeName;
        #endregion
        public frmCasting()
        {
            InitializeComponent();

            ItemName = "ArbItemName";
            PrimaryName = "ArbName";
            SizeName = "ArbSizeName";
            CaptionItemName = "اسم الصنف";
            if (UserInfo.Language == iLanguage.English)
            {
                ItemName = "EngItemName";
                CaptionItemName = "Item Name";
                SizeName = "EngSizeName";
                PrimaryName = "EngName";
            }
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            FillCombo.FillComboBoxLookUpEdit(cmbCurency, "Acc_Currency", "ID", PrimaryName, "", " BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد العملة"));

            cmbBranchesID.EditValue = MySession.GlobalBranchID;
            cmbBranchesID.ReadOnly = !MySession.GlobalAllowBranchModificationAllScreens;

            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  الحالة"));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;
            /*********************** Date Format dd/MM/yyyy ****************************/
            InitializeFormatDate(txtCommandDate);

            txtCommandDate.ReadOnly = false;
            this.KeyDown += frmZericonFactory_KeyDown;

            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);
            this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating);
            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtZircon.Validating += txtZircon_Validating;
            //this.txtEmployeeStokID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmployeeStokID_Validating);
            this.txtCommandID.Validating += txtCommandID_Validating;

            this.GridCastingBefore.InitNewRow += GridCastingBefore_InitNewRow;
            this.gridControlCastingBefore.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControlCastingBefore_ProcessGridKey);
            this.GridCastingBefore.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridCastingBefore_ValidatingEditor);
            this.GridCastingBefore.ValidateRow += GridCastingBefore_ValidateRow;
            this.gridControlCastingAfter.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControlCastingAfter_ProcessGridKey);
            this.GridCastingAfter.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridCastingAfter_ValidatingEditor);
            this.gridView6.ValidatingEditor += GridView6_ValidatingEditor;
            this.gridControl3.ProcessGridKey += GridControl3_ProcessGridKey;
            this.GridCastingBefore.RowUpdated += GridCastingBefore_RowUpdated;
            this.GridCastingAfter.RowUpdated += GridCastingAfter_RowUpdated;
            this.txtAccountID.Validating += txtAccountID_Validating;
            this.gridView6.CustomDrawCell += GridCastingAfter_CustomDrawCell;

            this.txtAllQTYCalc.Validating += txtAllQTYCalc_Validating;
            EnableControlDefult();
        }

        void txtAllQTYCalc_Validating(object sender, CancelEventArgs e)
        {
           if(Comon.cDec(txtAllQTYCalc.Text)>0)
           {
               txtGold24.Text=Comon.cDec((Comon.cDec(txtAllQTYCalc.Text)*18)/24).ToString();
               txtGold22.Text = Comon.cDec((Comon.cDec(txtAllQTYCalc.Text) * 18) / 22).ToString();
               txtGold21.Text = Comon.cDec((Comon.cDec(txtAllQTYCalc.Text) * 18) / 21).ToString();
               txtMatirial24.Text = Comon.cDec(Comon.cDec(txtAllQTYCalc.Text) - Comon.cDec(txtGold24.Text)).ToString();
               txtMatirial22.Text = Comon.cDec(Comon.cDec(txtAllQTYCalc.Text) - Comon.cDec(txtGold22.Text)).ToString();
               txtMatirial21.Text = Comon.cDec(Comon.cDec(txtAllQTYCalc.Text) - Comon.cDec(txtGold21.Text)).ToString();
           }
           else
           {
               txtGold24.Text = "";
               txtGold22.Text = "";
               txtGold21.Text = "";
               txtMatirial24.Text = "";
               txtMatirial22.Text = "";
               txtMatirial21.Text = "";


           }
        }
        void EnableControlDefult()
        {
            txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmCastingCostCenterID;
            cmbCurency.ReadOnly = !MySession.GlobalAllowChangefrmCastingCurrncyID;
           txtCommandDate.ReadOnly = !MySession.GlobalAllowChangefrmCastingCommandDate;
            txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmCastingStoreID;
            txtAccountID.ReadOnly = !MySession.GlobalAllowChangefrmCastingAccountID;
            txtFactorID.ReadOnly = !MySession.GlobalAllowChangefrmCastingEmployeeID;
        }
        void SetDefultValue()
        {
            txtCostCenterID.Text = MySession.GlobalDefaultCastingCostCenterID;
            txtCostCenterID_Validating(null, null);
            cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultCastingCurrncyID);
            cmbCurency_EditValueChanged(null, null);
            txtStoreID.Text = MySession.GlobalDefaultCastingStoreID;
            txtStoreID_Validating(null, null);
            txtAccountID.Text = MySession.GlobalDefaultCastingAccountID;
            txtAccountID_Validating(null, null);
            txtFactorID.Text = MySession.GlobalDefaultCastingEmployeeID;
            txtFactorID_Validating(null, null);
        }
        private void GridControl3_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.gridView6.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridCastingAfter.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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
                        if (ColName == "OrderID")
                        {
                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView6.Columns[ColName], Messages.msgInputIsRequired);
                            }
                            else
                            {
                                view.SetColumnError(gridView6.Columns[ColName], "");
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

        bool ChekOrderIsFoundInGrid(string OrderID)
        {
            for (int i = 0; i <= gridView6.DataRowCount - 1; i++)
            {
                if (i != gridView6.FocusedRowHandle)
                    if (gridView6.GetRowCellValue(i, "OrderID").ToString() == OrderID)
                        return true;
            }
            return false;
        }
        int CheckOrderIsCastingBeforByOntherCommand(string OrderID)
        {
            int ComandID = Comon.cInt(Lip.GetValue("Select CommandID From Manu_CastingOrders where CommandID<>" + Comon.cInt(txtCommandID.Text) + " and  Cancel=0 and OrderID='" + OrderID + "' and BranchID=" + MySession.GlobalBranchID));
            
                return ComandID;
           

        }
        private void GridView6_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (this.gridView6.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "OrderID" )
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    int ComandID= CheckOrderIsCastingBeforByOntherCommand(e.Value.ToString());
                    if (ComandID>0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = UserInfo.Language == iLanguage.Arabic ? "لقد تم الصب للطلبية في أمر سابق .. رقم امر الصب لهذه الطلبية:  "+ ComandID : "The order has been cast in a previous order..Casting order number for this order:"+ ComandID;
                        return;
                    }
                    if (ChekOrderIsFoundInGrid(e.Value.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = UserInfo.Language == iLanguage.Arabic ? "الطلبية موجودة لذلك لا يمكن انزالها اكثر من مرة " : "This Order is Found Table";
                        return;
                    }
                    DataTable dt = Manu_ManufacturingCastingDAL.GetDataOrderID(e.Value.ToString(), Comon.cInt(cmbBranchesID.EditValue), MySession.GlobalFacilityID);
                    if(dt.Rows.Count>0)
                      FileItemDataOrder(dt);
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = UserInfo.Language==iLanguage.Arabic?"الطلبية غير موجودة ":"This Order is not Found";
                    }
                }
            }
        }

        void txtZircon_Validating(object sender, CancelEventArgs e)
        {
            SumTotalBalance(GridCastingAfter, 2);
            SumTotalBalance(GridCastingBefore, 1);
        }

        void GridCastingBefore_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            SumTotalBalance(GridCastingBefore, 1);
        }
        void GridCastingAfter_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            SumTotalBalance(GridCastingAfter, 2);

           
        }
       
        private void frmCasting_Load(object sender, EventArgs e)
        {
            try
            {
                initGridCastingBefore();
                initGridCastingAfter();
                initGlstDetailOrders();

                 DoNew();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        void initGlstDetailOrders()
        {
            lstDetailOrders = new BindingList<Manu_OrderRestriction>();
            lstDetailOrders.AllowNew = true;
            lstDetailOrders.AllowEdit = true;
            lstDetailOrders.AllowRemove = true;
            gridControl3.DataSource = lstDetailOrders;

            gridView6.Columns["Cancel"].Visible = false;
            gridView6.Columns["TypeAuxiliaryMatirialID"].Visible = false;
            gridView6.Columns["BranchID"].Visible = false;
            gridView6.Columns["FacilityID"].Visible = false;

            gridView6.Columns["EditUserID"].Visible = false;
            gridView6.Columns["EditDate"].Visible = false;
            gridView6.Columns["EditTime"].Visible = false;
            gridView6.Columns["RegDate"].Visible = false;
            gridView6.Columns["UserID"].Visible = false;

            gridView6.Columns["ComputerInfo"].Visible = false;
            gridView6.Columns["EditComputerInfo"].Visible = false;
            gridView6.Columns["RegTime"].Visible = false;
            gridView6.Columns["TypeID"].Visible = false;
            gridView6.Columns["Notes"].Visible = false;
            gridView6.Columns["CustomerID"].Visible = false;
            gridView6.Columns["TypeOrdersID"].Visible = false;
            gridView6.Columns["DelegateID"].Visible = false;

            gridView6.Columns["TypeOrdersID"].Visible = false;
            gridView6.Columns["DelegateID"].Visible = false;
            gridView6.Columns["GuidanceID"].Visible = false;
            gridView6.Columns["TypeOrdersID"].Visible = false;
            gridView6.Columns["DelegateID"].Visible = false;
            gridView6.Columns["ImageCode"].Visible = false;
            gridView6.Columns["GoldQTYCloves"].OptionsColumn.AllowEdit = false;
            gridView6.Columns["GoldQTYCloves"].OptionsColumn.AllowFocus = false;
            gridView6.Columns["GoldQTYCloves"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView6.Columns["GoldQTYCloves"].SummaryItem.DisplayFormat = "{0:0.00}";

            gridView6.Columns["BonesPriceOrder"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView6.Columns["BonesPriceOrder"].SummaryItem.DisplayFormat = "{0:0.00}";
            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView6.Columns["OrderID"].Caption = "رقم الطلبية  ";
                gridView6.Columns["OrderDate"].Caption = "تاريخ ";
                gridView6.Columns["CustomerName"].Caption = "العميل ";
                gridView6.Columns["GoldQTYCloves"].Caption = "الوزن ";
                gridView6.Columns["BonesPriceOrder"].Caption = "الزركون ";
            }
            else
            {
                gridView6.Columns["OrderID"].Caption = "Order ID";
                gridView6.Columns["OrderDate"].Caption = "Order Date";
                gridView6.Columns["CustomerID"].Caption = "Customer ID";
                gridView6.Columns["GoldQTYCloves"].Caption = "Qty ";
                gridView6.Columns["BonesPriceOrder"].Caption = "Zircon ";
            }

        }
        #region Event
        public void txtCommandID_Validating(object sender, CancelEventArgs e)
        {

            if (FormView == true)
                ReadRecord(Comon.cInt(txtCommandID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }

        }
        private void GridCastingBefore_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "Fingerprint")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;

                GridCastingBefore.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridCastingBefore.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;

            }
        }

        private void GridCastingAfter_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "Fingerprint")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;

                ((GridView)sender).Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                ((GridView)sender).Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;

            }
        }
        void GridCastingBefore_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {

            try
            {

                foreach (GridColumn col in GridCastingBefore.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY"  )
                    {

                        var val = GridCastingBefore.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridCastingBefore.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                        else if (!(double.TryParse(val.ToString(), out num)) && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridCastingBefore.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridCastingBefore.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }
                        else
                        {
                            e.Valid = true;
                            GridCastingBefore.SetColumnError(col, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        private void GridCastingBefore_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridCastingBefore.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "BarCode" || ColName == "SizeID"   || ColName == "ItemID" || ColName == "StoreID" || ColName == "QTY")
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
                        view.SetColumnError(GridCastingBefore.Columns[ColName], "");

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridCastingBefore.SetColumnError(GridCastingBefore.Columns["QTY"], "");
                        e.ErrorText = "";
                        decimal totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "SizeID")), Comon.cDbl(txtStoreID.Text));
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Manu_ManufacturingCastingDetails", "Manu_ManufacturingCastingMaster", "QTY", "CommandID", Comon.cInt(txtCommandID.Text), GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "ItemID").ToString(), " and Manu_ManufacturingCastingDetails.TypeOpration=1",SizeID:Comon.cInt(GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "SizeID").ToString()));
                        totalQtyBalance += QtyInCommand;
                        decimal qtyCurrent = 0;
                         qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridCastingBefore, "QTY", Comon.cDec(val.ToString()), GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "ItemID").ToString(), Comon.cInt(GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "SizeID")));
                    
                        if (qtyCurrent > totalQtyBalance)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheQTyinOrderisExceed);
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgQtyisNotAvilable + (totalQtyBalance - (qtyCurrent - Comon.cDec(val.ToString())));
                            view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
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
                                    view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgQtyisNotAvilable + totalQtyBalance.ToString());
                                }
                            }
                            else
                            {
                                e.Valid = false;
                                HasColumnErrors = true;
                                e.ErrorText = Messages.msgNotFoundAnyQtyInStore;
                                view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNotFoundAnyQtyInStore);
                            }
                        }
                        
                        
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
                                view.SetColumnError(GridCastingBefore.Columns[ColName], "");
                                GridCastingBefore.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                GridCastingBefore.FocusedColumn = GridCastingBefore.VisibleColumns[0];
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
                            view.SetColumnError(GridCastingBefore.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridCastingBefore.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridCastingBefore.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "EmpFactorID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridCastingBefore.SetFocusedRowCellValue("EmpFactorName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
               
                if (ColName == SizeName)
                {

                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID and  Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and Stc_Items.BranchID=" + MySession.GlobalBranchID + " And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtBarCode = Lip.SelectRecord(Str);
                    if (dtBarCode.Rows.Count > 0)
                    {
                        GridCastingBefore.SetFocusedRowCellValue("SizeID", dtBarCode.Rows[0]["SizeID"]);
                        frmCadFactory.SetValuseWhenChangeSizeName(GridCastingBefore, Comon.cLong(GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "ItemID").ToString()), Comon.cInt(dtBarCode.Rows[0]["SizeID"]), "Manu_ManufacturingCastingDetails", "Manu_ManufacturingCastingMaster", Comon.cDbl(txtStoreID.Text), Comon.cInt(txtCommandID.Text), "CommandID", Where: " and Manu_ManufacturingCastingDetails.TypeOpration=1", FildNameQTY: "QTY");
                        e.Valid = true;
                        view.SetColumnError(GridCastingBefore.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
                else if(ColName=="DateROrD")
                {
                    {
                        //string formattedDate = dateValue.ToString("yyyy/MM/dd");
                        string formattedDate =((DateTime) e.Value).ToString("yyyy/MM/dd");
                        if (Lip.CheckDateISAvilable(formattedDate))
                        {
                            //Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
                            string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                            GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, "DateROrD", serverDate);
                            return;
                        }
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridCastingBefore.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridCastingBefore.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                else if (ColName == "EmpFactorName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpFactorID from HR_EmployeeFile Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridCastingBefore.SetFocusedRowCellValue("EmpFactorID", dtItemID.Rows[0]["EmpFactorID"]);
                        e.Valid = true;
                        view.SetColumnError(GridCastingBefore.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  ItemID from Stc_Items  Where Cancel =0 and BranchID=" + MySession.GlobalBranchID+"  and LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    DataTable dt = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(dtItemID.Rows[0]["ItemID"].ToString()), UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {
                        FileItemData(dt);
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود  ";
                    }
                }


            }
            SumTotalBalance(GridCastingBefore, 1);

        }
        private void GridCastingAfter_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridCastingAfter.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "BarCode" || ColName == "SizeID" || ColName == "ItemID" || ColName == "StoreID" || ColName == "EmpFactorID" || ColName == "QTY")
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
                        view.SetColumnError(GridCastingAfter.Columns[ColName], "");

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridCastingAfter.SetColumnError(GridCastingAfter.Columns["QTY"], "");
                        e.ErrorText = "";
 
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
                                FileItemData2(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData2(dt);
                            if (HasColumnErrors == false)
                            {
                                e.Valid = true;
                                view.SetColumnError(GridCastingAfter.Columns[ColName], "");
                                GridCastingAfter.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                GridCastingAfter.FocusedColumn = GridCastingAfter.VisibleColumns[0];
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
                                FileItemData2(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData2(dt);
                            e.Valid = true;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridCastingBefore.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+" And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridCastingAfter.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridCastingAfter.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridCastingAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                    else if (ColName == "EmpFactorID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 and BranchID=" + MySession.GlobalBranchID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridCastingAfter.SetFocusedRowCellValue("EmpFactorName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridCastingAfter.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridCastingAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
                else if (ColName == SizeName)
                {
                    string Str = "Select Stc_ItemUnits.SizeID from Stc_ItemUnits inner join Stc_Items on Stc_Items.ItemID=Stc_ItemUnits.ItemID   and Stc_Items.BranchID=Stc_ItemUnits.BranchID left outer join  Stc_SizingUnits on Stc_ItemUnits.SizeID=Stc_SizingUnits.SizeID and Stc_ItemUnits.BranchID=Stc_SizingUnits.BranchID Where UnitCancel=0 and Stc_Items.BranchID=" + MySession.GlobalBranchID + "  And LOWER (Stc_SizingUnits." + PrimaryName + ")=LOWER ('" + val.ToString() + "') and  Stc_Items.ItemID=" + Comon.cLong(GridCastingAfter.GetRowCellValue(GridCastingAfter.FocusedRowHandle, "ItemID").ToString()) + "  And Stc_ItemUnits.FacilityID=" + UserInfo.FacilityID;
                    DataTable dtItemID = Lip.SelectRecord(Str);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridCastingAfter.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                        e.Valid = true;
                        view.SetColumnError(GridCastingAfter.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundSizeForItem;
                        view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgNoFoundSizeForItem);
                    }
                }
                else if (ColName == "DateROrD")
                {
                    {
                     
                        string formattedDate = ((DateTime)e.Value).ToString("yyyy/MM/dd");
                        if (Lip.CheckDateISAvilable(formattedDate))
                        {
                           
                            string serverDate = Lip.GetServerDate(); e.Value = serverDate;
                            GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, "DateROrD", serverDate);
                            return;
                        }
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridCastingAfter.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridCastingAfter.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridCastingAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }

                else if (ColName == "EmpFactorName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpFactorID from HR_EmployeeFile Where Cancel=0 and BranchID=" + MySession.GlobalBranchID+"  And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridCastingAfter.SetFocusedRowCellValue("EmpFactorID", dtItemID.Rows[0]["EmpFactorID"]);
                        e.Valid = true;
                        view.SetColumnError(GridCastingAfter.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridCastingAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  ItemID from Stc_Items  Where Cancel =0 and BranchID=" + MySession.GlobalBranchID +"  and LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    DataTable dt = Stc_itemsDAL.GetTopItemDataByItemID(Comon.cInt(dtItemID.Rows[0]["ItemID"].ToString()), UserInfo.FacilityID);
                    if (dt.Rows.Count > 0)
                    {
                        FileItemData2(dt);
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = " الصنف غير موجود  ";
                    }
                }

            }
            SumTotalBalance(GridCastingAfter, 2);
        }
        private void gridControlCastingAfter_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.GridCastingBefore.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridCastingBefore.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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
                        if (ColName == "BarCode"   || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID")
                        {
                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgInputIsRequired);
                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridCastingBefore.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridCastingBefore.Columns[ColName], "");
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

        private void gridControlCastingBefore_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.GridCastingAfter.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridCastingAfter.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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
                        if (ColName == "BarCode"   || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID")
                        {

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridCastingAfter.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridCastingAfter.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridCastingAfter.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridCastingAfter.Columns[ColName], "");
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
        void GridCastingBefore_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                CSearch.ControlValidating(txtDelegateID, lblDelegateName, strSQL);
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
                    strSQL = "SELECT " + PrimaryName + " as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + " and BranchID=" + MySession.GlobalBranchID ;
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
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(Comon.cInt(cmbBranchesID.EditValue));
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
                strSQL = "SELECT "+PrimaryName+" as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
                string StoreManger = Lip.GetValue(strSQL).ToString();
                lblBeforeStoreManger.Text = StoreManger;
                lblStoreManger.Text = StoreManger;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
       
        private void txtEmployeeStokID_Validating(object sender, CancelEventArgs e)
        {
            //try
            //{
            //    strSQL = "SELECT "+PrimaryName+" as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokID.Text) + " And Cancel =0 ";
            //    CSearch.ControlValidating(txtEmployeeStokID, lblEmployeeStokName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            //}
            //catch (Exception ex)
            //{
            //    Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            //}

        }


        private void frmZericonFactory_KeyDown(object sender, KeyEventArgs e)
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


            if (FocusedControl.Trim() == txtStoreID.Name)
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

           
            else if (FocusedControl.Trim() == gridControlCastingBefore.Name)
            {
                
                if (GridCastingBefore.FocusedColumn.Name == "colItemID" || GridCastingBefore.FocusedColumn.Name == "col" + ItemName || GridCastingBefore.FocusedColumn.Name == "colBarCode")
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
                            GridCastingBefore.Columns[ItemName].ColumnEdit = rItem;
                            gridControlCastingBefore.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }

                else if (GridCastingBefore.FocusedColumn.Name == "colSizeName" || GridCastingBefore.FocusedColumn.Name == "colSizeID")
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

            
            else if (FocusedControl.Trim() == gridControlCastingAfter.Name)
            {

                if (GridCastingAfter.FocusedColumn.Name == "colItemID" || GridCastingAfter.FocusedColumn.Name == "col" + ItemName || GridCastingAfter.FocusedColumn.Name == "colBarCode")
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
                            GridCastingAfter.Columns[ItemName].ColumnEdit = rItem;
                            gridControlCastingAfter.RepositoryItems.Add(rItem);
                        }
                    }
                    else
                        frm.Dispose();
                }
                else if (GridCastingAfter.FocusedColumn.Name == "colSizeName" || GridCastingAfter.FocusedColumn.Name == "colSizeID")
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
        #endregion

        #region InitGrids
        void initGridCastingBefore()
        {

            lstDetailCastingBefore = new BindingList<Manu_ManufacturingCastingDetails>();
            lstDetailCastingBefore.AllowNew = true;
            lstDetailCastingBefore.AllowEdit = true;
            lstDetailCastingBefore.AllowRemove = true;
            gridControlCastingBefore.DataSource = lstDetailCastingBefore;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID, " + PrimaryName + "   FROM Stc_SizingUnits  where  BranchID=" + MySession.GlobalBranchID );
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                    NameUnit[i] = dtitems.Rows[i][PrimaryName].ToString();

            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);

            gridControlCastingBefore.RepositoryItems.Add(riComboBoxitems);

            GridCastingBefore.Columns[SizeName].ColumnEdit = riComboBoxitems;



            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and   BranchID=" + MySession.GlobalBranchID );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlCastingBefore.RepositoryItems.Add(riComboBoxitems2);
            GridCastingBefore.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + MySession.GlobalBranchID );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlCastingBefore.RepositoryItems.Add(riComboBoxitems3);
            GridCastingBefore.Columns["EmpFactorName"].ColumnEdit = riComboBoxitems3;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0 and BranchID=" + MySession.GlobalBranchID );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlCastingBefore.RepositoryItems.Add(riComboBoxitems4);
            GridCastingBefore.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridCastingBefore.Columns["CommandID"].Visible = false;
            GridCastingBefore.Columns["BranchID"].Visible = false;
            GridCastingBefore.Columns["FacilityID"].Visible = false;                   
            GridCastingBefore.Columns["ArbItemName"].Visible = GridCastingBefore.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridCastingBefore.Columns["EngItemName"].Visible = GridCastingBefore.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            GridCastingBefore.Columns["TypeOpration"].Visible = false;
            GridCastingBefore.Columns[ItemName].Visible = true;
       
            GridCastingBefore.Columns[ItemName].Caption = CaptionItemName;

            GridCastingBefore.Columns["EmpFactorName"].Width = 150;
            GridCastingBefore.Columns[ItemName].Width = 150;
            GridCastingBefore.Columns[SizeName].Width = 120;
            GridCastingBefore.Columns["StoreName"].Width = 120;
            GridCastingBefore.Columns["EmpFactorID"].Width = 130;
            GridCastingBefore.Columns["EmpFactorID"].Visible = false;
            GridCastingBefore.Columns["EmpFactorName"].Visible = false;
            GridCastingBefore.Columns["StoreName"].Visible = false;
            GridCastingBefore.Columns["StoreID"].Visible = false;
            GridCastingBefore.Columns["SizeID"].Visible = false;
            //GridCastingBefore.Columns["DateROrD"].OptionsColumn.ReadOnly = false;
            //GridCastingBefore.Columns["DateROrD"].OptionsColumn.AllowFocus = true;

            //GridCastingBefore.Columns["TimeROrD"].OptionsColumn.ReadOnly = false;
            //GridCastingBefore.Columns["TimeROrD"].OptionsColumn.AllowFocus = true;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridCastingBefore.Columns["EngItemName"].Visible = false;
                GridCastingBefore.Columns["EngSizeName"].Visible = false;
                GridCastingBefore.Columns["Fingerprint"].Visible = false;

                GridCastingBefore.Columns["SizeID"].Caption = "رقم الوحدة";
                GridCastingBefore.Columns["ItemID"].Caption = "رقم الصنــف";

                GridCastingBefore.Columns["BarCode"].Caption = "باركود الصنف";
                GridCastingBefore.Columns[SizeName].Caption = "الوحدة ";
                GridCastingBefore.Columns["QTY"].Caption = "الوزن";
           
                GridCastingBefore.Columns["Fingerprint"].Caption = "البصمــة";
                GridCastingBefore.Columns["DateROrD"].Caption = "التاريــخ";

                GridCastingBefore.Columns["TimeROrD"].Caption = "الوقـــت";
                GridCastingBefore.Columns["StoreID"].Caption = "رقم المخزن ";
                GridCastingBefore.Columns["StoreName"].Caption = "إسم المخزن";

                GridCastingBefore.Columns["EmpFactorID"].Caption = " رقم العامل";
                GridCastingBefore.Columns["EmpFactorName"].Caption = "إسم العــامل ";
            }
            else
            {
                GridCastingBefore.Columns["ArbItemName"].Visible = false;
                GridCastingBefore.Columns["ArbSizeName"].Visible = false;
                GridCastingBefore.Columns["Fingerprint"].Visible = false;

                GridCastingBefore.Columns["SizeID"].Caption = "Unit ID";
                GridCastingBefore.Columns["ItemID"].Caption = "Item ID";
                GridCastingBefore.Columns["BarCode"].Caption = "BarCode"; 
                GridCastingBefore.Columns[SizeName].Caption = "Unit Name ";
                 GridCastingBefore.Columns["QTY"].Caption = "QTY";
                GridCastingBefore.Columns["DateROrD"].Caption = "Date";
                GridCastingBefore.Columns["Fingerprint"].Caption = "Fingerprint";


                GridCastingBefore.Columns["TimeROrD"].Caption = "Time";
                GridCastingBefore.Columns["StoreID"].Caption = "Store ID";
                GridCastingBefore.Columns["StoreName"].Caption = "Store Name";

                GridCastingBefore.Columns["EmpFactorID"].Caption = "Emp Factor ID";
                GridCastingBefore.Columns["EmpFactorName"].Caption = "Emp Factor Name";
            }

        }
        void initGridCastingAfter()
        {

            lstDetailCastingAfter = new BindingList<Manu_ManufacturingCastingDetails>();
            lstDetailCastingAfter.AllowNew = true;
            lstDetailCastingAfter.AllowEdit = true;
            lstDetailCastingAfter.AllowRemove = true;

            gridControlCastingAfter.DataSource = lstDetailCastingAfter;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID, " + PrimaryName + "  FROM Stc_SizingUnits  where   BranchID=" + MySession.GlobalBranchID );
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameUnit[i] = dtitems.Rows[i][PrimaryName].ToString();


            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControlCastingAfter.RepositoryItems.Add(riComboBoxitems);
            GridCastingAfter.Columns[SizeName].ColumnEdit = riComboBoxitems;
            GridCastingAfter.Columns["ArbItemName"].Visible = GridCastingAfter.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridCastingAfter.Columns["EngItemName"].Visible = GridCastingAfter.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            GridCastingAfter.Columns[ItemName].Visible = true;
            GridCastingAfter.Columns[ItemName].Caption = CaptionItemName;
            GridCastingAfter.Columns[SizeName].ColumnEdit = riComboBoxitems;

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0  and BranchID=" + MySession.GlobalBranchID );
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                if (UserInfo.Language == iLanguage.Arabic)
                    StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
                else
                    StoreName[i] = dtStore.Rows[i]["EngName"].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlCastingAfter.RepositoryItems.Add(riComboBoxitems2);
            GridCastingAfter.Columns["StoreName"].ColumnEdit = riComboBoxitems2;

            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0  and BranchID=" + MySession.GlobalBranchID );
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlCastingAfter.RepositoryItems.Add(riComboBoxitems3);
            GridCastingAfter.Columns["EmpFactorName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0  and BranchID=" + MySession.GlobalBranchID );
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlCastingAfter.RepositoryItems.Add(riComboBoxitems4);
            GridCastingAfter.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridCastingAfter.Columns["CommandID"].Visible = false;
            GridCastingAfter.Columns["BranchID"].Visible = false;
            GridCastingAfter.Columns["FacilityID"].Visible = false;
            
            GridCastingAfter.Columns["TypeOpration"].Visible = false;
            GridCastingAfter.Columns["EmpFactorName"].Width = 150;
            GridCastingAfter.Columns[ItemName].Width = 150;
            GridCastingAfter.Columns[SizeName].Width = 120;
            GridCastingAfter.Columns["StoreName"].Width = 120;
            GridCastingAfter.Columns["EmpFactorID"].Width = 130;
            GridCastingAfter.Columns["SizeID"].Visible = false;
            //GridCastingAfter.Columns["DateROrD"].OptionsColumn.ReadOnly = false;
            //GridCastingAfter.Columns["DateROrD"].OptionsColumn.AllowFocus = true;

            //GridCastingAfter.Columns["TimeROrD"].OptionsColumn.ReadOnly = false;
            //GridCastingAfter.Columns["TimeROrD"].OptionsColumn.AllowFocus = true;


            GridCastingAfter.Columns["EmpFactorID"].Visible = false;
            GridCastingAfter.Columns["EmpFactorName"].Visible = false;
            GridCastingAfter.Columns["StoreName"].Visible = false;
            GridCastingAfter.Columns["StoreID"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridCastingAfter.Columns["EngItemName"].Visible = false;
                GridCastingAfter.Columns["EngSizeName"].Visible = false;
                GridCastingAfter.Columns["Fingerprint"].Visible = false;

                GridCastingAfter.Columns["SizeID"].Caption = "رقم الوحدة";
                GridCastingAfter.Columns["ItemID"].Caption = "رقم الصنــف";
                GridCastingAfter.Columns["BarCode"].Caption = "باركود الصنف";
                GridCastingAfter.Columns[SizeName].Caption = "الوحدة ";
                GridCastingAfter.Columns["QTY"].Caption = "الوزن ";             
                GridCastingAfter.Columns["Fingerprint"].Caption = "البصمــة";
                

                GridCastingAfter.Columns["DateROrD"].Caption = "التاريــخ";
                GridCastingAfter.Columns["TimeROrD"].Caption = "الوقـــت";
                GridCastingAfter.Columns["StoreID"].Caption = "رقم المخزن ";
                GridCastingAfter.Columns["StoreName"].Caption = "إسم المخزن";
                GridCastingAfter.Columns["EmpFactorID"].Caption = " رقم العامل";
                GridCastingAfter.Columns["EmpFactorName"].Caption = "إسم العــامل ";
            }
            else
            {
                GridCastingAfter.Columns["ArbItemName"].Visible = false;
                GridCastingAfter.Columns["ArbSizeName"].Visible = false;
                GridCastingAfter.Columns["SizeID"].Caption = "Unit ID";
                GridCastingAfter.Columns["ItemID"].Caption = "Item ID";
                GridCastingAfter.Columns["BarCode"].Caption = "BarCode"; 
                GridCastingAfter.Columns[SizeName].Caption = "Unit Name ";               
                GridCastingAfter.Columns["QTY"].Caption = "QTY";
                GridCastingAfter.Columns["DateROrD"].Caption = "Date";
                GridCastingAfter.Columns["Fingerprint"].Caption = "Fingerprint";
                GridCastingAfter.Columns["TimeROrD"].Caption = "Time";
                GridCastingAfter.Columns["StoreID"].Caption = "Store ID";
                GridCastingAfter.Columns["StoreName"].Caption = "Store Name";
                GridCastingAfter.Columns["EmpFactorID"].Caption = "Emp Factor ID";
                GridCastingAfter.Columns["EmpFactorName"].Caption = "Emp Factor Name";
            }

        }
        #endregion



        #region Function

        private void FileItemData(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                decimal totalQtyBalance = 0;
           
                {
                    totalQtyBalance = Lip.RemindQtyItemByMinUnit(Comon.cLong(dt.Rows[0]["ItemID"].ToString()), Comon.cInt(dt.Rows[0]["SizeID"]), Comon.cDbl(txtStoreID.Text));
                    {
                        decimal qtyCurrent = 0;
                        decimal QtyInCommand = Lip.GetQTYinCommandToThisItem("Manu_ManufacturingCastingDetails", "Manu_ManufacturingCastingMaster ", "QTY", "CommandID", Comon.cInt(txtCommandID.Text), dt.Rows[0]["ItemID"].ToString(), " and Manu_ManufacturingCastingDetails.TypeOpration=1",SizeID:Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                        qtyCurrent = frmCadFactory.GetQTYToItemFromGridView(GridCastingBefore, "QTY", 0, dt.Rows[0]["ItemID"].ToString(), Comon.cInt(dt.Rows[0]["SizeID"].ToString()));
                        totalQtyBalance += QtyInCommand;
                        totalQtyBalance -= qtyCurrent;
                    }
                    if (totalQtyBalance <= 0)
                    {
                        if (MySession.AllowOutQtyNegative)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore);
                            return;
                        }
                        bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore + "هل تريد المتابعة ...");
                        if (!yes)
                            return;
                    }
                }
                if (MySession.AllowNotShowQTYInQtyField == false)
                    totalQtyBalance = 0;
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["QTY"], totalQtyBalance);
                
                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                //GridCastingBefore.Columns[SizeName].ColumnEdit = rSize;
                //gridControlCastingBefore.RepositoryItems.Add(rSize);
                GridCastingBefore.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["CommandID"], txtCommandID.Text);
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                if (UserInfo.Language == iLanguage.English)
                   GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns[SizeName], dt.Rows[0][SizeName].ToString());
                else
                    GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns[ItemName], dt.Rows[0][PrimaryName].ToString());
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["TimeROrD"], DateTime.Now.ToString("hh:mm:tt"));
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["DateROrD"], DateTime.Now.ToString("yyyy/MM/dd")); 
            }
            else
            {
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["QTY"], "0");
                GridCastingBefore.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["CommandID"], txtCommandID.Text);
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["SizeID"], "");
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns[SizeName],"");
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["BarCode"], "");
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["ItemID"], "");
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns[ItemName], "");
                GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["DateROrD"], DateTime.Now.ToString("yyyy/MM/dd")); 
            }
        }

        private void FileItemData2(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                //RepositoryItemLookUpEdit rSize = Common.LookUpEditSize(Comon.cDbl(dt.Rows[0]["ItemID"].ToString()));
                //GridCastingAfter.Columns[SizeName].ColumnEdit = rSize;
                //gridControlCastingAfter.RepositoryItems.Add(rSize);

                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["CommandID"], txtCommandID.Text);
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["Qty"], 0);
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns[SizeName], dt.Rows[0][SizeName].ToString());
                else
                    GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns[ItemName], dt.Rows[0][PrimaryName].ToString());
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["TimeROrD"], DateTime.Now.ToString("hh:mm:tt"));
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["DateROrD"], DateTime.Now.ToString("yyyy/MM/dd")); 
            }
            else
            {
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["Qty"], "0");
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["CommandID"], txtCommandID.Text);
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["SizeID"],"");
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns[SizeName], "");
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["BarCode"], "");
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["ItemID"], "");
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns[ItemName], "");
                GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["DateROrD"], DateTime.Now.ToString("yyyy/MM/dd")); 
            }
        }
        public void ClearFields()
        {
            try
            {
                txtDelegateID.Text = "";
                txtNumberCups.Text = "";
                txtNumberCrews.Text = "";
                txtCostCenterID.Text = "";
                lblCostCenterName.Text = "";
                txtZircon.Text = "";
                txtTotalQty_CastingAfter.Text = "";
                txtTotalQty_CastingBefore.Text = "";

                txtEstimatedLoss.Text = "";

                lblDelegateName.Text = "";
                txtNotes.Text = "";
                txtCommandDate.EditValue = DateTime.Now;
                cmbCurency.EditValue = Comon.cInt(MySession.GlobalDefaultCastingCurrncyID);
                 
                txtCustomerID.Text = "";
                txtCustomerID_Validating(null, null);

           
               

                //txtEmployeeStokID.Text = "";
                //txtEmployeeStokID_Validating(null, null);

                // GetAccountsDeclaration();

                txtDelegateID.Text = MySession.GlobalDefaultSaleDelegateID;
                txtDelegateID_Validating(null, null);



                txtStoreID.Text = "";
                txtStoreID_Validating(null, null);
                lstDetailCastingBefore = new BindingList<Manu_ManufacturingCastingDetails>();
                lstDetailCastingBefore.AllowNew = true;
                lstDetailCastingBefore.AllowEdit = true;
                lstDetailCastingBefore.AllowRemove = true;
                gridControlCastingBefore.DataSource = lstDetailCastingBefore;

                lstDetailCastingAfter = new BindingList<Manu_ManufacturingCastingDetails>();
                lstDetailCastingAfter.AllowNew = true;
                lstDetailCastingAfter.AllowEdit = true;
                lstDetailCastingAfter.AllowRemove = true;
                gridControlCastingAfter.DataSource = lstDetailCastingAfter;
                dt = new DataTable();
                


                lstDetailOrders = new BindingList<Manu_OrderRestriction>();
                lstDetailOrders.AllowNew = true;
                lstDetailOrders.AllowEdit = true;
                lstDetailOrders.AllowRemove = true;
                gridControl3.DataSource = lstDetailOrders;

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CastingCommend", "رقـم الأمر", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CastingCommend", "Commend ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            //else if (FocusedControl.Trim() == txtEmployeeStokID.Name)
            //{
            //    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

            //    if (UserInfo.Language == iLanguage.Arabic)
            //        PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, lblBeforeStoreManger, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            //    else
            //        PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, lblBeforeStoreManger, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            //}
            else if (FocusedControl.Trim() == txtFactorID.Name)
            {
                 
               if (!MySession.GlobalAllowChangefrmCastingEmployeeID ) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCastingCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            if (FocusedControl.Trim() == txtCustomerID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }

            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCastingStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }

            else if (FocusedControl.Trim() == txtAccountID.Name)
            {
                if (!MySession.GlobalAllowChangefrmCastingAccountID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };                
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "Account ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }

            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == gridControl3.Name)
            {
              
                if (gridView6.FocusedColumn == null) return;
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "CastingOrderID", "رقم الطلب", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "CastingOrderID", "Order ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }
            }

            else if (FocusedControl.Trim() == gridControlCastingBefore.Name)
            {
                if (GridCastingBefore.FocusedColumn == null) return;
                if (GridCastingBefore.FocusedColumn.Name == "colBarCode" || GridCastingBefore.FocusedColumn.Name == "colItemName" || GridCastingBefore.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }

                else if (GridCastingBefore.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }

                else if (GridCastingBefore.FocusedColumn.Name == "colSizeID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }

                else if (GridCastingBefore.FocusedColumn.Name == "colEmpFactorID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }
                else if (GridCastingBefore.FocusedColumn.Name == "colQTY")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "ItemID") != null)
                            frm.SetValueToControl(GridCastingBefore.GetRowCellValue(GridCastingBefore.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
                       else
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "ارجاء اختيار صنف ومن  ثم اعادة عرض الكمية المتبقية" : "Please select an item and re-display the remaining quantity");
                            frm.Close();
                            return;
                        }
                    }
                    else
                        frm.Dispose();
                }
            }
            else if (FocusedControl.Trim() == gridControlCastingAfter.Name)
            {
                if (GridCastingAfter.FocusedColumn == null) return;
                if (GridCastingAfter.FocusedColumn.Name == "colBarCode" || GridCastingAfter.FocusedColumn.Name == "colItemName" || GridCastingAfter.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }
                else if (GridCastingAfter.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }

                else if (GridCastingAfter.FocusedColumn.Name == "colSizeID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }

                else if (GridCastingAfter.FocusedColumn.Name == "colEmpFactorID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                }
                else if (GridCastingAfter.FocusedColumn.Name == "colQTY")
                {
                    frmRemindQtyItem frm = new frmRemindQtyItem();
                    if (Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, UserInfo.BRANCHID, UserInfo.FacilityID))
                    {
                        if (UserInfo.Language == iLanguage.English)
                            ChangeLanguage.EnglishLanguage(frm);

                        frm.Show();
                        if (GridCastingAfter.GetRowCellValue(GridCastingAfter.FocusedRowHandle, "ItemID") != null)
                          frm.SetValueToControl(GridCastingAfter.GetRowCellValue(GridCastingAfter.FocusedRowHandle, "ItemID").ToString(), txtStoreID.Text.ToString());
                        else
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "ارجاء اختيار صنف ومن  ثم اعادة عرض الكمية المتبقية" : "Please select an item and re-display the remaining quantity");
                            frm.Close();
                            return;
                        }
                    }
                    else
                        frm.Dispose();
                }

            }
            GetSelectedSearchValue(cls);
        }
        private void txtFactorID_Validating(object sender, CancelEventArgs e)
        {

            try
            {
                strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtFactorID.Text) + " And Cancel =0   and BranchID=" + MySession.GlobalBranchID ;
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
        void FileItemDataOrder(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {

                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["OrderID"], dt.Rows[0]["OrderID"].ToString());
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["CustomerName"], dt.Rows[0][PrimaryName].ToString());
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["OrderDate"], dt.Rows[0]["OrderDate"].ToString());
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["GoldQTYCloves"], dt.Rows[0]["TotalQTY"].ToString());
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["BranchID"],cmbBranchesID.EditValue);
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["BonesPriceOrder"],Comon.cDec( dt.Rows[0]["BonesPriceOrder"].ToString()));
            }
            else
            {
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["OrderID"], "");
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["CustomerName"], "");
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["OrderDate"], "");
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["GoldQTYCloves"],"");
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["BranchID"], "");
                gridView6.SetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns["BonesPriceOrder"], "");
            }
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
                //else if (FocusedControl == txtEmployeeStokID.Name)
                //{
                //    txtEmployeeStokID.Text = cls.PrimaryKeyValue.ToString();
                //    txtEmployeeStokID_Validating(null, null);
                //}
                if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }
                if (FocusedControl == txtAccountID.Name)
                {
                    txtAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountID_Validating(null, null);
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
                else if (FocusedControl == gridControl3.Name)
                {
                    if (gridView6.FocusedColumn.Name == "colOrderID")
                    {
                        string OrderID = cls.PrimaryKeyValue.ToString();

                        int ComandID = CheckOrderIsCastingBeforByOntherCommand(OrderID);
                        if (ComandID > 0)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لقد تم الصب للطلبية في أمر سابق .. رقم امر الصب لهذه الطلبية:  " + ComandID : "The order has been cast in a previous order..Casting order number for this order:" + ComandID) ;
                            return;
                        }

                        if (ChekOrderIsFoundInGrid(OrderID))
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الطلبية موجودة لذلك لا يمكن انزالها اكثر من مرة " : "This Order is Found Table");
                            return;
                        }
                        DataTable dt = Manu_ManufacturingCastingDAL.GetDataOrderID(OrderID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(MySession.GlobalFacilityID));
                        if (dt.Rows.Count > 0) 
                        {
                            gridView6.AddNewRow();
                            FileItemDataOrder(dt);
                        }
                    }
                }


                else if (FocusedControl == gridControlCastingBefore.Name)
                {
                    if (GridCastingBefore.FocusedColumn.Name == "colBarCode" || GridCastingBefore.FocusedColumn.Name == "colItemName" || GridCastingBefore.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, Comon.cInt(cmbBranchesID.EditValue.ToString()), MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridCastingBefore.AddNewRow();
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));

                        // CalculateRow();
                    }
                    if (GridCastingBefore.FocusedColumn.Name == "colStoreID")
                    {
                        GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["StoreName"], Lip.GetValue(strSQL));

                    }

                    if (GridCastingBefore.FocusedColumn.Name == "colSizeID")
                    {
                        GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridCastingBefore.FocusedColumn.Name == "colEmpFactorID")
                    {
                        GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["EmpFactorID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                        GridCastingBefore.SetRowCellValue(GridCastingBefore.FocusedRowHandle, GridCastingBefore.Columns["EmpFactorName"], Lip.GetValue(strSQL));
                    }
                }

                else if (FocusedControl == gridControlCastingAfter.Name)
                {
                    if (GridCastingAfter.FocusedColumn.Name == "colBarCode" || GridCastingAfter.FocusedColumn.Name == "colItemName" || GridCastingAfter.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, Comon.cInt(cmbBranchesID.EditValue.ToString()), MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridCastingAfter.AddNewRow();
                        FileItemData2(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));
                        // CalculateRow();
                    }
                    if (GridCastingAfter.FocusedColumn.Name == "colStoreID")
                    {
                        GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["StoreName"], Lip.GetValue(strSQL));
                    }

                    if (GridCastingAfter.FocusedColumn.Name == "colSizeID")
                    {
                        GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridCastingAfter.FocusedColumn.Name == "colEmpFactorID")
                    {
                        GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["EmpFactorID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0  and BranchID=" + MySession.GlobalBranchID ;
                        GridCastingAfter.SetRowCellValue(GridCastingAfter.FocusedRowHandle, GridCastingAfter.Columns["EmpFactorName"], Lip.GetValue(strSQL));
                    }
                }
            }

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



            foreach (GridColumn col in GridCastingBefore.Columns)
            {
                //if (col.FieldName == "BarCode")
                {

                    GridCastingBefore.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridCastingBefore.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridCastingBefore.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }

            }
            foreach (GridColumn col in GridCastingAfter.Columns)
            {
                //if (col.FieldName == "BarCode")
                {
                    GridCastingAfter.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridCastingAfter.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridCastingAfter.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }

            }
        }
        #endregion
        #region Do Function
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                txtCommandID.Text = Manu_ManufacturingCastingDAL.GetNewID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue)) + "";
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

        protected override void DoSave()
        {
            try
            {
                if (GridCastingAfter.DataRowCount > 0)
                {
                    txtNumberCrews.Tag = "ImportantFieldGreaterThanZero";
                    txtNumberCups.Tag = "ImportantFieldGreaterThanZero";
                }
                else
                {
                    txtNumberCrews.Tag = "isNumber";
                    txtNumberCups.Tag = "isNumber";
                }
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
                if (!Lip.CheckTheProcessesIsPosted("Manu_ManufacturingCastingMaster", Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbStatus.EditValue), Comon.cLong(txtCommandID.Text), PrimeryColName: "CommandID"))
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
            objRecord.BranchID = UserInfo.BRANCHID;
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
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(UserInfo.BRANCHID)));
            objRecord.VoucherID = VoucherID;
            objRecord.EditUserID = UserInfo.ID;
            objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            Result = VariousVoucherMachinDAL.DeleteAcc_VariousVoucherMachinMaster(objRecord);
            return Result;

        }
        //هذه الدالة لحذف امر الصرف او التوريد من الارشيف الخاص باوامر الصرف والتوريد الخاصة بالتصنيع
        int DeleteInOnOROutOnBil(int DocumentID, int DocumentType)
        {
            int Result = 0;
            Stc_ManuFactoryCommendOutOnBail_Master objRecord = new Stc_ManuFactoryCommendOutOnBail_Master();
            objRecord.InvoiceID = DocumentID;
            objRecord.DocumentType = DocumentType;
            //objRecord.TypeCommand = TypeCommand;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            Result = Stc_ManuFactoryCommendOutOnBailDAL.Delete(objRecord);
            return Result;

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

                Manu_ManufacturingCastingMaster model = new Manu_ManufacturingCastingMaster();
                model.CommandID = Comon.cInt(txtCommandID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                
                string Result = Manu_ManufacturingCastingDAL.Delete(model);
                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                    int MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeBefore);
                    MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
                    if (MoveID<0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                }
                if (Comon.cInt(Result) > 0)
                {
                    int VoucherID = 0;
                    //حذف القيد الالي
                    DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBefore);
                    if (dtInvoiceID.Rows.Count > 0)
                    {
                        VoucherID = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceID.Rows[0][0]), DocumentTypeBefore);
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية ");
                    }
                    int VoucherIDAfter = 0;
                    DataTable dtInvoiceIDAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
                    if (dtInvoiceIDAfter.Rows.Count > 0)
                    {
                        VoucherIDAfter = DeleteVariousVoucherMachin(Comon.cInt(dtInvoiceIDAfter.Rows[0][0]), DocumentTypeAfter);
                        if (VoucherIDAfter == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية");
                    }
                }

                if (Comon.cInt(Result) > 0)
                {
                    int InID = 0;
                    //حذف التوريد والصرف من الارشيف
                    DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBefore);
                    if (dtInvoiceID.Rows.Count > 0)
                    {
                        InID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceID.Rows[0][0]), DocumentTypeBefore);
                        if (InID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حذف الصرف من الارشيف  ");
                    }
                    int OutID = 0;
                    DataTable dtInvoiceIDAfter = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
                    if (dtInvoiceIDAfter.Rows.Count > 0)
                    {
                        OutID = DeleteInOnOROutOnBil(Comon.cInt(dtInvoiceIDAfter.Rows[0][0]), DocumentTypeAfter);
                        if (OutID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حذف التوريد من الارشيف ");
                    }
                }
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
        protected override void DoEdit()
        {  
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("CommandID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BranchID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("DateROrD", System.Type.GetType("System.DateTime"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("Fingerprint", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));
           
            
            dtItem.Columns.Add("TypeOpration", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("SizeID", System.Type.GetType("System.Int32"));

            dtItem.Columns.Add("StoreID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("StoreName", System.Type.GetType("System.String"));

            dtItem.Columns.Add("TimeROrD", System.Type.GetType("System.String"));
            dtItem.Columns.Add("EmpFactorID", System.Type.GetType("System.Int64"));
            dtItem.Columns.Add("EmpFactorName", System.Type.GetType("System.String"));
            for (int i = 0; i <= GridCastingBefore.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["CommandID"] = Comon.cInt(txtCommandID.Text);
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; 
                dtItem.Rows[i]["BarCode"] = GridCastingBefore.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i][ItemName] = GridCastingBefore.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtItem.Rows[i][SizeName] = GridCastingBefore.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["SizeID"] = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i]["TypeOpration"] = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "TypeOpration").ToString());
                dtItem.Rows[i]["Fingerprint"] = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "Fingerprint").ToString());
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(GridCastingBefore.GetRowCellValue(i, "QTY").ToString());
                

                dtItem.Rows[i]["DateROrD"] = GridCastingBefore.GetRowCellValue(i, "DateROrD").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["StoreName"] = GridCastingBefore.GetRowCellValue(i, "StoreName").ToString();
                dtItem.Rows[i]["TimeROrD"] = GridCastingBefore.GetRowCellValue(i, "TimeROrD").ToString();
                dtItem.Rows[i]["EmpFactorID"] = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "EmpFactorID").ToString());
                dtItem.Rows[i]["EmpFactorName"] = GridCastingBefore.GetRowCellValue(i, "EmpFactorName").ToString();


            }

            gridControlCastingBefore.DataSource = dtItem;

            DataTable dtItem1 = new DataTable();
            dtItem1.Columns.Add("CommandID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem1.Columns.Add("BranchID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem1.Columns.Add("DateROrD", System.Type.GetType("System.DateTime"));

            dtItem1.Columns.Add("FacilityID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("Fingerprint", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("ItemID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("QTY", System.Type.GetType("System.Decimal"));

            dtItem1.Columns.Add(SizeName, System.Type.GetType("System.String"));

       
        
            dtItem1.Columns.Add("TypeOpration", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("SizeID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("StoreID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("StoreName", System.Type.GetType("System.String"));
            dtItem1.Columns.Add("TimeROrD", System.Type.GetType("System.String"));
            dtItem1.Columns.Add("EmpFactorID", System.Type.GetType("System.Int64"));
            dtItem1.Columns.Add("EmpFactorName", System.Type.GetType("System.String"));
            for (int i = 0; i <= GridCastingAfter.DataRowCount - 1; i++)
            {
                dtItem1.Rows.Add();
                dtItem1.Rows[i]["CommandID"] = Comon.cInt(txtCommandID.Text);
                dtItem1.Rows[i]["FacilityID"] = UserInfo.FacilityID; 
                dtItem1.Rows[i]["BarCode"] = GridCastingAfter.GetRowCellValue(i, "BarCode").ToString();
                dtItem1.Rows[i]["ItemID"] = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "ItemID").ToString());
                dtItem1.Rows[i][ItemName] = GridCastingAfter.GetRowCellValue(i, ItemName).ToString();
                dtItem1.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtItem1.Rows[i][SizeName] = GridCastingAfter.GetRowCellValue(i, SizeName).ToString();
                dtItem1.Rows[i]["SizeID"] = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "SizeID").ToString());
                dtItem1.Rows[i]["TypeOpration"] = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "TypeOpration").ToString());
                dtItem1.Rows[i]["Fingerprint"] = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "Fingerprint").ToString());
                dtItem1.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(GridCastingAfter.GetRowCellValue(i, "QTY").ToString());              
                dtItem1.Rows[i]["DateROrD"] = GridCastingAfter.GetRowCellValue(i, "DateROrD").ToString();
                dtItem1.Rows[i]["StoreID"] = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "StoreID").ToString());
                dtItem1.Rows[i]["StoreName"] = GridCastingAfter.GetRowCellValue(i, "StoreName").ToString();
                dtItem1.Rows[i]["TimeROrD"] =  GridCastingAfter.GetRowCellValue(i, "TimeROrD").ToString() ;
                dtItem1.Rows[i]["EmpFactorID"] = Comon.cDbl(GridCastingAfter.GetRowCellValue(i, "EmpFactorID").ToString());
                dtItem1.Rows[i]["EmpFactorName"] = GridCastingAfter.GetRowCellValue(i, "EmpFactorName").ToString();
            }
            gridControlCastingAfter.DataSource = dtItem1;


            DataTable dtitem2 = new DataTable();
            dtitem2.Columns.Add("OrderID", System.Type.GetType("System.String"));
            dtitem2.Columns.Add("GoldQTYCloves", System.Type.GetType("System.Decimal"));
            dtitem2.Columns.Add("BonesPriceOrder", System.Type.GetType("System.Decimal")); 
            dtitem2.Columns.Add("CustomerName", System.Type.GetType("System.String"));
            dtitem2.Columns.Add("OrderDate", System.Type.GetType("System.String"));
            dtitem2.Columns.Add("FacilityID", System.Type.GetType("System.Int32"));
            dtitem2.Columns.Add("BranchID", System.Type.GetType("System.Int32"));
            for (int i = 0; i <gridView6.DataRowCount; i++)
            {

                dtitem2.Rows.Add(); 
                dtitem2.Rows[i]["FacilityID"] = UserInfo.FacilityID;
                dtitem2.Rows[i]["OrderID"] = gridView6.GetRowCellValue(i, "OrderID").ToString();
                dtitem2.Rows[i]["GoldQTYCloves"] = Comon.cDec(gridView6.GetRowCellValue(i, "GoldQTYCloves").ToString());
                dtitem2.Rows[i]["BonesPriceOrder"] = Comon.cDec(gridView6.GetRowCellValue(i, "BonesPriceOrder").ToString());
                dtitem2.Rows[i]["CustomerName"] = gridView6.GetRowCellValue(i, "CustomerName").ToString();
                dtitem2.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtitem2.Rows[i]["OrderDate"] = gridView6.GetRowCellValue(i, "OrderDate").ToString();
            }
            gridControl3.DataSource = dtitem2;
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);

        
        }

        protected override void DoSearch()
        {
            try
            {
                txtCommandID.Enabled = true;
                txtCommandID.Focus();
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        //public XtraReport Manu_FactoryCasting(GridView Grid, string ColQTy)
        //{
        //    string rptrptManu_FactoryFactorCommendName = "rptManurptManu_FactoryCastting";
        //    string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\";
        //    //rptCompanyHeaderName += (UserInfo.Language == iLanguage.English ? "Eng" : "Arb");
        //    rptrptManu_FactoryFactorCommendName += "Arb";
        //    XtraReport rptFactoryFactor = XtraReport.FromFile(Path + rptrptManu_FactoryFactorCommendName + ".repx", true);


        //    var dataTable = new dsReports.rptManu_FactoryBeforeCastingStageDataTable();
        //    for (int i = 0; i <= Grid.DataRowCount - 1; i++)
        //    {
        //        var row = dataTable.NewRow();
        //        row["#"] = i + 1;
        //        //row["MachinID"] = Grid.GetRowCellValue(i, "MachinID");
        //        //row["MachineName"] = Grid.GetRowCellValue(i, "MachineName");
        //        row["QTY"] = Grid.GetRowCellValue(i, ColQTy);
        //        row["StoreName"] = Grid.GetRowCellValue(i, "StoreName");
        //        row["ItemID"] = Grid.GetRowCellValue(i, "ItemID");
        //        row["ItemName"] = Grid.GetRowCellValue(i, ItemName);
        //        row["SizeName"] = Grid.GetRowCellValue(i, SizeName);
        //        row["DateBefore"] = Grid.GetRowCellValue(i, "DateROrD");
        //        row["DateAfter"] = Grid.GetRowCellValue(i, "DateROrD");
        //        row["Time"] = Grid.GetRowCellValue(i, "TimeROrD");
        //        row["EmpName"] = Grid.GetRowCellValue(i, "EmpName");
        //        dataTable.Rows.Add(row);
        //    }
        //    rptFactoryFactor.DataSource = dataTable;
        //    rptFactoryFactor.DataMember = "rptManu_FactoryBeforeCastingStage";
        //    return rptFactoryFactor;
        //}
        //protected override void DoPrint()
        //{

        //    try
        //    {
        //        if (IsNewRecord)
        //        {
        //            Messages.MsgWarning(Messages.TitleWorning, Messages.msgYouShouldSaveDataBeforePrinting);
        //            return;
        //        }
        //        Application.DoEvents();
        //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
        //        /******************** Report Body *************************/
        //        ReportName = "rptManurptManu_FactoryAddtional";
        //        bool IncludeHeader = true;
        //        string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
        //        XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);

        //        /********************** Master *****************************/
        //        rptForm.RequestParameters = false;

        //        for (int i = 0; i < rptForm.Parameters.Count; i++)
        //            rptForm.Parameters[i].Visible = false;


        //        rptForm.Parameters["ReferanceID"].Value = txtReferanceID.Text;
        //        rptForm.Parameters["CustomerName"].Value = lblCustomerName.Text;
        //        rptForm.Parameters["DelegetName"].Value = lblDelegateName.Text;
        //        rptForm.Parameters["BrandID"].Value = cmbBranchesID.EditValue.ToString();
        //        rptForm.Parameters["BrandName"].Value = cmbBranchesID.Text;
        //        rptForm.Parameters["StoreName"].Value = lblStoreName.Text;
        //        rptForm.Parameters["AccountName"].Value = lblAccountName.Text;
        //        rptForm.Parameters["EmployeeStokName"].Value = lblBeforeStoreManger.Text;
        //        rptForm.Parameters["StoreManger"].Value = lblStoreManger.Text;
        //        rptForm.Parameters["EmpName"].Value = lblFactorName.Text;
        //        rptForm.Parameters["Curencyr"].Value = cmbCurency.Text;


        //        /********************** Details ****************************/
        //        rptForm.DataMember = ReportName;
        //        /******************** Report Binding ************************/
        //        XRSubreport subreport = (XRSubreport)rptForm.FindControl("subRptCompanyHeader", true);
        //        subreport.Visible = IncludeHeader;
        //        subreport.ReportSource = ReportComponent.CompanyHeader();


        //        /******************** Report Addtional ************************/
        //        //قبل العمل
        //        XRSubreport subreportAddtional = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryCastingBefore", true);
        //        subreportAddtional.Visible = IncludeHeader;
        //        subreportAddtional.ReportSource = Manu_FactoryCasting(GridCastingBefore, "QTY");

        //        //بعد العمل
        //        XRSubreport subreportAddtionalAfter = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryCastingAfter", true);
        //        subreportAddtionalAfter.Visible = IncludeHeader;
        //        subreportAddtionalAfter.ReportSource = Manu_FactoryCasting(GridCastingAfter, "QTY");

        //        rptForm.ShowPrintStatusDialog = false;
        //        rptForm.ShowPrintMarginsWarning = false;
        //        rptForm.CreateDocument();

        //        SplashScreenManager.CloseForm(false);
        //        ShowReportInReportViewer = true;
        //        if (ShowReportInReportViewer)
        //        {
        //            frmReportViewer frmRptViewer = new frmReportViewer();
        //            frmRptViewer.documentViewer1.DocumentSource = rptForm;
        //            frmRptViewer.ShowDialog();
        //        }
        //        else
        //        {
        //            bool IsSelectedPrinter = false;
        //            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);
        //            DataTable dt = ReportComponent.SelectRecord("SELECT *  from Printers where ReportName='" + ReportName + "'");
        //            if (dt.Rows.Count > 0)
        //                for (int i = 1; i < 6; i++)
        //                {
        //                    string PrinterName = dt.Rows[0]["PrinterName" + i.ToString()].ToString().ToUpper();
        //                    if (!string.IsNullOrEmpty(PrinterName))
        //                    {
        //                        rptForm.PrinterName = PrinterName;
        //                        rptForm.Print(PrinterName);
        //                        IsSelectedPrinter = true;
        //                    }
        //                }
        //            SplashScreenManager.CloseForm(false);
        //            if (!IsSelectedPrinter)
        //                Messages.MsgWarning(Messages.TitleWorning, Messages.msgThereIsNotPrinterSelected);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);
        //        Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
        //    }
        //}
        long SaveVariousVoucherMachin(int DocumentID, bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeBefore;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridCastingBefore.GetRowCellValue(GridCastingBefore.DataRowCount - 1, "DateROrD")).ToString("dd/MM/yyyy")).ToString(); 
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);

            //objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            // objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);

            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
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

            //Debit Matirial
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 2;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            if (Comon.cInt(returned.CurrencyID) == 1)
                returned.DebitGold = Comon.cDbl(txtTotalQty_CastingBefore.Text); 
            else
                returned.DebitMatirial = Comon.cDbl(txtTotalQty_CastingBefore.Text); 
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            if (Comon.cInt(returned.CurrencyID)==1)
                returned.CreditGold = Comon.cDbl(txtTotalQty_CastingBefore.Text);
            else
                returned.CreditMatirial = Comon.cDbl(txtTotalQty_CastingBefore.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);

            
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        long SaveVariousVoucherMachinInOn(int DocumentID, bool isNew)
        {
            int VoucherID = 0;
            long Result = 0;
            Acc_VariousVoucherMachinMaster objRecord = new Acc_VariousVoucherMachinMaster();
            objRecord.DocumentType = DocumentTypeAfter;
            VoucherID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster where DocumentID=" + DocumentID + " And DocumentType=" + objRecord.DocumentType + " And BranchID=" + Comon.cInt(cmbBranchesID.EditValue)));

            objRecord.VoucherID = VoucherID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = MySession.GlobalFacilityID;
            //Date
            objRecord.VoucherDate = Comon.ConvertDateToSerial(((DateTime)GridCastingAfter.GetRowCellValue(GridCastingAfter.DataRowCount - 1, "DateROrD")).ToString("dd/MM/yyyy")).ToString(); 
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.CurrencyName = cmbCurency.Text.ToString();
            objRecord.CurrencyPrice = Comon.cDec(txtCurrncyPrice.Text);
            objRecord.CurrencyEquivalent = Comon.cDec(lblcurrncyEquvilant.Text);

            //objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            // objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes = txtNotes.Text == "" ? this.Text : txtNotes.Text;
            objRecord.DocumentID = DocumentID;
            objRecord.Cancel = 0;
            objRecord.Posted =   Comon.cInt(cmbStatus.EditValue);

            //user Info
            objRecord.UserID = UserInfo.ID;
            objRecord.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecord.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
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

            //Debit Matirial
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 2;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            if(Comon.cInt(returned.CurrencyID)==1)
                returned.DebitGold = Comon.cDbl(txtTotalQty_CastingAfter.Text); 
            else
                returned.DebitMatirial = Comon.cDbl(txtTotalQty_CastingAfter.Text); 
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);        
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            if (Comon.cInt(returned.CurrencyID) == 1)
                returned.CreditGold = Comon.cDbl(txtTotalQty_CastingAfter.Text);
            else
                returned.CreditMatirial = Comon.cDbl(txtTotalQty_CastingAfter.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Credit) * Comon.cDbl(returned.CurrencyPrice)); 
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        //private void SaveOutOn()
        //{
        //    #region Save Out On
        //    //Save Out On
        //    bool isNew = IsNewRecord;
        //    Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
        //    if (IsNewRecord)
        //        objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
        //    else
        //    {
        //        DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBefore);
        //        if (dtInvoiceID.Rows.Count > 0)
        //            objRecordOutOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
        //        else
        //        {
        //            objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
        //            isNew = true;

        //        }

        //    }
        //    objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
        //    objRecordOutOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
        //    objRecordOutOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
        //    objRecordOutOnMaster.FacilityID = UserInfo.FacilityID;
        //    objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
        //    objRecordOutOnMaster.CurrencyID = Comon.cInt(cmbCurency.EditValue);
        //    objRecordOutOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
        //    objRecordOutOnMaster.TypeCommand = 1;
        //    objRecordOutOnMaster.DocumentType = DocumentTypeBefore;
        //    objRecordOutOnMaster.Cancel = 0;
        //    objRecordOutOnMaster.DebitAccount = Comon.cDbl(txtAccountID.Text);
        //    objRecordOutOnMaster.StoreID = Comon.cDbl(txtStoreID.Text);
        //    objRecordOutOnMaster.Notes = txtNotes.Text;
        //    objRecordOutOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
        //    //user Info
        //    objRecordOutOnMaster.UserID = UserInfo.ID;
        //    objRecordOutOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
        //    objRecordOutOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
        //    objRecordOutOnMaster.ComputerInfo = UserInfo.ComputerInfo;
        //    objRecordOutOnMaster.EditUserID = 0;
        //    objRecordOutOnMaster.EditTime = 0;
        //    objRecordOutOnMaster.EditDate = 0;
        //    objRecordOutOnMaster.EditComputerInfo = "";
        //    Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
        //    List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
        //    for (int i = 0; i <= GridCastingBefore.DataRowCount - 1; i++)
        //    {
        //        returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
        //        returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
        //        returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
        //        returnedOutOn.FacilityID = UserInfo.FacilityID;
        //        returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
        //        returnedOutOn.CommandDate = Comon.cDate(GridCastingBefore.GetRowCellValue(i, "DateROrD").ToString());
        //        returnedOutOn.CommandTime = (Comon.cDateTime(GridCastingBefore.GetRowCellValue(i, "TimeROrD")).ToShortTimeString());
        //        returnedOutOn.BarCode = GridCastingBefore.GetRowCellValue(i, "BarCode").ToString();
        //        returnedOutOn.ItemID = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "ItemID").ToString());
        //        returnedOutOn.SizeID = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "SizeID").ToString());
        //        returnedOutOn.QTY = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "QTY").ToString());
        //        returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
        //        listreturnedOutOn.Add(returnedOutOn);
        //    }
        //    if (listreturnedOutOn.Count > 0)
        //    {
        //        objRecordOutOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
        //        int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordOutOnMaster, isNew);
        //        if (Result > 0)
        //        {
        //            //حفظ القيد الالي
        //            long VoucherID = SaveVariousVoucherMachin(Comon.cInt(objRecordOutOnMaster.InvoiceID), isNew);
        //            if (VoucherID == 0)
        //                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
        //            else
        //                Lip.ExecututeSQL("Update " + Manu_ManufacturingCastingDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Manu_ManufacturingCastingDAL.PremaryKey + " = " + txtCommandID.Text);
        //        }
        //    }
        //    #endregion
        //}
        //private void SaveInOn()
        //{
        //    #region Save Out On
        //    //Save Out On
        //    bool isNew = IsNewRecord;
        //    Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
        //    if (IsNewRecord)
        //        objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
        //    else
        //    {
        //        DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
        //        if (dtInvoiceID.Rows.Count > 0)
        //            objRecordInOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
        //        else
        //        {
        //            objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
        //            isNew = true;
        //        }
        //    }
        //    objRecordInOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
        //    objRecordInOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
        //    objRecordInOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
        //    objRecordInOnMaster.FacilityID = UserInfo.FacilityID;
       
        //    objRecordInOnMaster.CurrencyID = Comon.cInt(cmbCurency.EditValue);
        //    objRecordInOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
        //    objRecordInOnMaster.TypeCommand = 2;
        //    objRecordInOnMaster.DocumentType = DocumentTypeAfter;
        //    objRecordInOnMaster.Cancel = 0;
        //    objRecordInOnMaster.DebitAccount = Comon.cDbl(txtAccountID.Text);
        //    objRecordInOnMaster.StoreID = Comon.cDbl(txtStoreID.Text);
        //    objRecordInOnMaster.Notes = txtNotes.Text;
        //    objRecordInOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
        //    //user Info
        //    objRecordInOnMaster.UserID = UserInfo.ID;
        //    objRecordInOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
        //    objRecordInOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
        //    objRecordInOnMaster.ComputerInfo = UserInfo.ComputerInfo;
        //    objRecordInOnMaster.EditUserID = 0;
        //    objRecordInOnMaster.EditTime = 0;
        //    objRecordInOnMaster.EditDate = 0;
        //    objRecordInOnMaster.EditComputerInfo = "";
        //    Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
        //    List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
        //    for (int i = 0; i <= GridCastingAfter.DataRowCount - 1; i++)
        //    {
        //        returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
        //        returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
        //        returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
        //        returnedOutOn.FacilityID = UserInfo.FacilityID;
        //        returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
        //        returnedOutOn.CommandDate = Comon.cDate(GridCastingAfter.GetRowCellValue(i, "DateROrD").ToString());
        //        returnedOutOn.CommandTime = (Comon.cDateTime(GridCastingAfter.GetRowCellValue(i, "TimeROrD")).ToShortTimeString());
        //        returnedOutOn.BarCode = GridCastingAfter.GetRowCellValue(i, "BarCode").ToString();
        //        returnedOutOn.ItemID = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "ItemID").ToString());
        //        returnedOutOn.SizeID = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "SizeID").ToString());
        //        returnedOutOn.QTY = Comon.cDbl(GridCastingAfter.GetRowCellValue(i, "QTY").ToString());
        //        returnedOutOn.CostPrice = Comon.cDbl(0.0001.ToString());
        //        listreturnedOutOn.Add(returnedOutOn);
        //    }
        //    if (listreturnedOutOn.Count > 0)
        //    {
        //        objRecordInOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
        //        int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordInOnMaster, isNew);
        //        if (Result > 0)
        //        {
        //            //حفظ القيد الالي
        //            long VoucherID = SaveVariousVoucherMachinInOn(Comon.cInt(objRecordInOnMaster.InvoiceID),isNew);
        //            if (VoucherID == 0)
        //                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
        //            else
        //                Lip.ExecututeSQL("Update " + Manu_ManufacturingCastingDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Manu_ManufacturingCastingDAL.PremaryKey + " = " + txtCommandID.Text);
        //        }
        //    }
        //    #endregion
        //}
        private int SaveStockMoveingOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeBefore;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            objRecord.Posted= Comon.cInt(cmbStatus.EditValue);
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridCastingBefore.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridCastingBefore.GetRowCellValue(i, "DateROrD")).ToString("dd/MM/yyyy")).ToString(); 

                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeBefore;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridCastingBefore.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID ));
                returned.QTY = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = 0;
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = Comon.cDbl(Lip.AverageUnit(Comon.cInt(returned.ItemID), Comon.cInt(returned.SizeID), Comon.cDbl(txtStoreID.Text)));
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
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
        private int SaveStockMoveingIn(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeAfter;
            objRecord.MoveType = 1;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridCastingAfter.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(((DateTime)GridCastingAfter.GetRowCellValue(i, "DateROrD")).ToString("dd/MM/yyyy")).ToString(); 

                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeAfter;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridCastingAfter.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + " and BranchID=" + MySession.GlobalBranchID ));
                returned.QTY = Comon.cDbl(GridCastingAfter.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = Comon.cDbl(Lip.AverageUnit(Comon.cInt(returned.ItemID), Comon.cInt(returned.SizeID), Comon.cDbl(txtStoreID.Text)));
                //returned.Bones = Comon.cDbl(GridCastingBefore.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = 0;
                returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
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
        private void Save()
        {
            GridCastingBefore.Focus();
            GridCastingBefore.MoveLastVisible();
            GridCastingBefore.MoveLast();
            GridCastingAfter.MoveLast();
            GridCastingBefore.FocusedColumn = GridCastingBefore.VisibleColumns[1];
            Manu_ManufacturingCastingMaster objRecord = new Manu_ManufacturingCastingMaster();
            objRecord.CommandID = Comon.cInt(txtCommandID.Text);
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.CommandDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
            objRecord.AccountID = Comon.cDbl(txtAccountID.Text);
            objRecord.FactorID = Comon.cDbl(txtFactorID.Text);
            //objRecord.EmployeeStokID = Comon.cDbl(txtEmployeeStokID.Text);
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.DelegetID = Comon.cInt(txtDelegateID.Text);
            objRecord.ReferanceID = Comon.cInt(txtReferanceID.Text);
            txtNotes.Text = (txtNotes.Text.Trim());
            objRecord.Notes = txtNotes.Text;
            objRecord.NumberCrews = Comon.cInt(txtNumberCrews.Text);
            objRecord.NumberCups = Comon.cInt(txtNumberCups.Text);
            objRecord.Zircon_W = Comon.cDbl(txtZircon.Text);
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
            Manu_ManufacturingCastingDetails returned;
            List<Manu_ManufacturingCastingDetails> listreturned = new List<Manu_ManufacturingCastingDetails>();
            for (int i = 0; i <= GridCastingBefore.DataRowCount - 1; i++)
            {
                returned = new Manu_ManufacturingCastingDetails();
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DateROrD = Comon.cDate(GridCastingBefore.GetRowCellValue(i, "DateROrD").ToString());
                returned.TimeROrD = Comon.cDateTime(GridCastingBefore.GetRowCellValue(i, "TimeROrD")).ToShortTimeString();
                returned.BarCode = GridCastingBefore.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "ItemID").ToString());
                returned.StoreID = Comon.cLong(txtStoreID.Text);
                returned.EmpFactorID = Comon.cDbl(txtFactorID.Text);
                returned.SizeID = Comon.cInt(GridCastingBefore.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.ConvertToDecimalQty(GridCastingBefore.GetRowCellValue(i, "QTY").ToString());
                returned.ArbSizeName = GridCastingBefore.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridCastingBefore.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridCastingBefore.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridCastingBefore.GetRowCellValue(i, ItemName).ToString();
                returned.StoreName = lblStoreName.Text.ToString();
                returned.EmpFactorName = lblFactorName.Text;
                returned.TypeOpration = 1;
                listreturned.Add(returned);
            }
            if (GridCastingAfter.DataRowCount > 0)
            {
                for (int i = 0; i <= GridCastingAfter.DataRowCount - 1; i++)
                {
                    returned = new Manu_ManufacturingCastingDetails();
                    returned.CommandID = Comon.cInt(txtCommandID.Text);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    returned.DateROrD = Comon.cDate( GridCastingAfter.GetRowCellValue(i, "DateROrD").ToString());
                    returned.TimeROrD =  Comon.cDateTime(GridCastingAfter.GetRowCellValue(i, "TimeROrD")).ToShortTimeString();
                    returned.StoreID = Comon.cLong(txtStoreID.Text);
                    returned.EmpFactorID = Comon.cDbl(txtFactorID.Text);
                    returned.BarCode = GridCastingAfter.GetRowCellValue(i, "BarCode").ToString();
                    returned.ItemID = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "ItemID").ToString());
                    returned.SizeID = Comon.cInt(GridCastingAfter.GetRowCellValue(i, "SizeID").ToString());
                    returned.QTY = Comon.ConvertToDecimalQty(GridCastingAfter.GetRowCellValue(i, "QTY").ToString());
                    returned.ArbSizeName = GridCastingAfter.GetRowCellValue(i, SizeName).ToString();
                    returned.EngSizeName = GridCastingAfter.GetRowCellValue(i, SizeName).ToString();
                    returned.ArbItemName = GridCastingAfter.GetRowCellValue(i, ItemName).ToString();
                    returned.EngItemName = GridCastingAfter.GetRowCellValue(i, ItemName).ToString();
                    returned.StoreName = lblStoreName.Text.ToString();
                    returned.EmpFactorName = lblFactorName.Text.ToString();
                    returned.TypeOpration = 2;
                    listreturned.Add(returned);
                }
            }
            Manu_OrderRestriction returnedOrderDetail;
            List<Manu_OrderRestriction> listreturnedOrder = new List<Manu_OrderRestriction>();
            if (gridView6.DataRowCount>0)
            {
              
                for (int i = 0; i <= gridView6.DataRowCount-1; i++)
                {
                    returnedOrderDetail = new Manu_OrderRestriction();
                    returnedOrderDetail.OrderID = gridView6.GetRowCellValue(i,"OrderID").ToString();
                    returnedOrderDetail.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    returnedOrderDetail.FacilityID = UserInfo.FacilityID;
                    returnedOrderDetail.CustomerName = gridView6.GetRowCellValue(i, "CustomerName").ToString();
                    returnedOrderDetail.Cancel = 0; 
                    returnedOrderDetail.GoldQTYCloves =Comon.cDec( gridView6.GetRowCellValue(i, "GoldQTYCloves").ToString());
                    if (Comon.cDec(gridView6.GetRowCellValue(i, "BonesPriceOrder").ToString()) > 0)
                        returnedOrderDetail.BonesPriceOrder = Comon.cDec(gridView6.GetRowCellValue(i, "BonesPriceOrder").ToString());
                    else
                        returnedOrderDetail.BonesPriceOrder = 0;
                    returnedOrderDetail.OrderDate = Comon.ConvertDateToSerial(gridView6.GetRowCellValue(i, "OrderDate").ToString()).ToString();
                    listreturnedOrder.Add(returnedOrderDetail);
                }
            }

            if (listreturned.Count > 0)
            {
                objRecord.Menu_F_AuxiliaryMaterials = listreturned;
                objRecord.Menu_F_DetialOrder = listreturnedOrder;
             
                string Result = Manu_ManufacturingCastingDAL.InsertUsingXML(objRecord, IsNewRecord);
                if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                {
                    
                    //SaveOutOn(); //حفظ   الصرف المخزني
                    // حفظ الحركة المخزنية 
                    if (Comon.cInt(Result) > 0)
                    {
                        int MoveID = SaveStockMoveingOut(Comon.cInt(Result));
                        if (MoveID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
                        if (Comon.cInt(Result) > 0)
                        {
                            //حفظ القيد الالي
                            long VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result), IsNewRecord);
                            if (VoucherID == 0)
                                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                            else
                                Lip.ExecututeSQL("Update " + Manu_ManufacturingCastingDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Manu_ManufacturingCastingDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID );
                        }
                    }
                    if (GridCastingAfter.DataRowCount > 0)
                    {
                        //SaveInOn(); //حفظ   التوريد المخزني
                        // حفظ الحركة المخزنية 
                        if (Comon.cInt(Result) > 0)
                        {
                            bool isNew = true;
                            DataTable dtInvoiceID = Stc_ItemsMoviingDAL.GetCountElementID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(Result), DocumentTypeAfter);
                            if (Comon.cInt( dtInvoiceID.Rows[0][0]) > 0)
                                isNew = false;
                            int MoveID = SaveStockMoveingIn(Comon.cInt(Result));
                            if (MoveID == 0)
                                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");

                            if (Comon.cInt(Result) > 0)
                            {
                                //حفظ القيد الالي
                                long VoucherID = SaveVariousVoucherMachinInOn(Comon.cInt(Result), isNew);
                                if (VoucherID == 0)
                                    Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                                else
                                    Lip.ExecututeSQL("Update " + Manu_ManufacturingCastingDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Manu_ManufacturingCastingDAL.PremaryKey + " = " + Result + " and BranchID=" + MySession.GlobalBranchID );
                            }
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
                        EnabledControl(false);
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                         
                        Validations.DoSaveRipon(this, ribbonControl1);
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

        bool IsValidGrid()
        {
            double num;

            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            GridCastingBefore.MoveLast();

            int length = GridCastingBefore.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in GridCastingBefore.Columns)
                {
                    if (col.FieldName == "BarCode"   || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
                    {

                        var cellValue = GridCastingBefore.GetRowCellValue(i, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            GridCastingBefore.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;
                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            GridCastingBefore.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            GridCastingBefore.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                }
            }
            return true;
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
                    strSQL = "SELECT TOP 1 * FROM " + Manu_ManufacturingCastingDAL.TableName + " Where Cancel =0  And BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_ManufacturingCastingDAL.PremaryKey + " ASC";
                                break;
                            }
                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Manu_ManufacturingCastingDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Manu_ManufacturingCastingDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Manu_ManufacturingCastingDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Manu_ManufacturingCastingDAL.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_ManufacturingCastingDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Manu_ManufacturingCastingDAL();

                    int InvoicIDTemp = Comon.cInt(txtCommandID.Text);
                    InvoicIDTemp = cClass.GetRecordSetBySQL(strSQL);
                    if (cClass.FoundResult == true)
                    {
                        ReadRecord(InvoicIDTemp);
                        EnabledControl(false);
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
        private void SumTotalBalance(GridView Grid, int flage)
        {
            try
            {
                decimal QTY = 0;
                
                decimal TotalQtTYBefore = 0;
               
                for (int i = 0; i <= Grid.DataRowCount - 1; i++)
                {
                    QTY = Comon.ConvertToDecimalPrice(Grid.GetRowCellValue(i, "QTY").ToString());
                     
                    TotalQtTYBefore += QTY;
                   
                }
                if (flage == 1)
                {
                    txtTotalQty_CastingBefore.Text =Comon.cDec(Comon.cDec(txtZircon.Text)+Comon.cDec( TotalQtTYBefore)) + "";
             
                }
                else
                {
                    txtTotalQty_CastingAfter.Text = TotalQtTYBefore + "";
              
                }
                txtEstimatedLoss.Text = Comon.ConvertToDecimalPrice(Comon.cDec(txtTotalQty_CastingBefore.Text) - Comon.cDec(txtTotalQty_CastingAfter.Text)) + "";
                if(Comon.cDec(txtNumberCups.Text)!=0)
                txtCupsLost.Text = Comon.ConvertToDecimalPrice(Comon.cDec(txtEstimatedLoss.Text) / Comon.cDec(txtNumberCups.Text)) + "";
                decimal QTYZircone = 0;
                for (int i = 0; i < gridView6.DataRowCount; i++)
                {
                    QTYZircone += Comon.cDec(gridView6.GetRowCellValue(i, "BonesPriceOrder").ToString());
                }
                txtZircon.Text = QTYZircone.ToString();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtAccountID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as AccountName FROM Acc_Accounts WHERE AccountID =" + Comon.cLong(txtAccountID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtAccountID, lblAccountName , strSQL);


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
                    dt = Manu_ManufacturingCastingDAL.frmGetDataDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                    DataTable dtOrders = Manu_ManufacturingCastingDAL.frmGetOrdersDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;

                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["CommandDate"].ToString()) == "")
                            InitializeFormatDate(txtCommandDate);
                        else
                            txtCommandDate.EditValue = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["CommandDate"].ToString()), "dd/MM/yyyy", culture);


                        txtCommandID.Text = dt.Rows[0]["CommandID"].ToString();
                        txtReferanceID.Text = dt.Rows[0]["ReferanceID"].ToString();

                        txtNumberCrews.Text = dt.Rows[0]["NumberCrews"].ToString();

                        txtNumberCups.Text = dt.Rows[0]["NumberCups"].ToString();
                        //Validate
                        cmbCurency.EditValue =Comon.cInt( dt.Rows[0]["CurrencyID"].ToString());
                        cmbStatus.EditValue = Comon.cInt(dt.Rows[0]["Posted"].ToString());
                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);

                        txtDelegateID.Text = dt.Rows[0]["DelegetID"].ToString();
                        txtDelegateID_Validating(null, null);

                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);   
      
                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);


                        txtFactorID.Text =dt.Rows[0]["FactorID"].ToString();
                        txtFactorID_Validating(null, null);
                        
                        //txtEmployeeStokID.Text = dt.Rows[0]["EmployeeStokID"].ToString();
                        //txtEmployeeStokID_Validating(null, null);

                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();

                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"]);
                      
                        dt1 = dt.Clone();
                        txtAccountID.Text = dt.Rows[0]["AccountID"].ToString();
                        txtAccountID_Validating(null, null);
                        foreach (DataRow row in dt.Rows)
                        {
                            if (Convert.ToInt32(row["TypeOpration"]) == 1)
                            {
                                DataRow newRow = dt1.NewRow();
                                newRow.ItemArray = row.ItemArray;                               
                                dt1.Rows.Add(newRow);
                            }
                        }
                        gridControlCastingBefore.DataSource = dt1;
                        lstDetailCastingBefore.AllowNew = true;
                        lstDetailCastingBefore.AllowEdit = true;
                        lstDetailCastingBefore.AllowRemove = true;
 
                        dt2 = dt.Clone();
                        
                        foreach (DataRow row in dt.Rows)
                        {
                            if (Convert.ToInt32(row["TypeOpration"]) == 2)
                            {

                                DataRow newRow = dt2.NewRow();
                                newRow.ItemArray = row.ItemArray;           
                                dt2.Rows.Add(newRow);
                            }
                        }

                        gridControlCastingAfter.DataSource = dt2;
                        lstDetailCastingAfter.AllowNew = true;
                        lstDetailCastingAfter.AllowEdit = true;
                        lstDetailCastingAfter.AllowRemove = true;

                        gridControl3.DataSource = dtOrders;
                        lstDetailOrders.AllowNew = true;
                        lstDetailOrders.AllowEdit = true;
                        lstDetailOrders.AllowRemove = true;

                        txtZircon.Text = dt.Rows[0]["Zircon_W"].ToString();
                        txtZircon_Validating(null, null);

                        SumTotalBalance(GridCastingBefore, 1);
                        SumTotalBalance(GridCastingAfter, 2);

                        Validations.DoReadRipon(this, ribbonControl1);
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
        #endregion

        private void cmbCurency_EditValueChanged(object sender, EventArgs e)
        {
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID ));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0 and BranchID=" + MySession.GlobalBranchID ));
                txtCurrncyPrice.Text = CurrncyPrice + "";
                //lblCurrencyEqvAfter.Visible = true;
                //lblCurrencyEqvBfore.Visible = true;
                lblCurrncyPric.Visible = true;
                //lblcurrncyEquvilant.Visible = true;
                //labelControl2.Visible = true;
                txtCurrncyPrice.Visible = true;
            }
            else
            {
                txtCurrncyPrice.Text = "1";
                //lblCurrencyEqvBfore.Visible = false;
                //lblCurrencyEqvAfter.Visible = false;
                //lblcurrncyEquvilant.Visible = false;
                lblCurrncyPric.Visible = false;
                //labelControl2.Visible = false;
                txtCurrncyPrice.Visible = false;
            }
        }

        private void btnMachinResractionBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
             int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + Comon.cInt(txtCommandID.Text) + " And DocumentType=" + DocumentTypeBefore).ToString());
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

        private void btnMachinResractionAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
             int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + Comon.cInt(txtCommandID.Text) + " And DocumentType=" + DocumentTypeAfter).ToString());
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
            string Path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\Reports\2\";
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
                row["DateAfter"] = Grid.GetRowCellValue(i, "DateROrD");
                row["EmpName"] = Grid.GetRowCellValue(i, "TimeROrD");
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
                ReportName = "rptManu_FactoryCastingOpretion";
                bool IncludeHeader = true;
                string rptFormName = (UserInfo.Language == iLanguage.English ? ReportName + "Eng" : ReportName + "Arb");
                XtraReport rptForm = XtraReport.FromFile(ReportComponent.GetReportPath() + rptFormName + ".repx", true);
                /********************** Master *****************************/
                rptForm.RequestParameters = false;

                for (int i = 0; i < rptForm.Parameters.Count; i++)
                    rptForm.Parameters[i].Visible = false;

                rptForm.Parameters["OrderID"].Value = txtCommandID.Text;
                rptForm.Parameters["OrderDate"].Value = txtCommandDate.Text;
                rptForm.Parameters["CustomerName"].Value = lblAccountName.Text;
                rptForm.Parameters["DelegetName"].Value = txtCurrncyPrice.Text;
                rptForm.Parameters["GuidanceName"].Value = txtReferanceID.Text;
                rptForm.Parameters["TypeOrder"].Value = "";

                rptForm.Parameters["BranchesID"].Value = cmbBranchesID.Text;
                rptForm.Parameters["BeforeStoreName"].Value = lblStoreName.Text;
                rptForm.Parameters["BeforeStoreManger"].Value = lblStoreManger.Text;
                rptForm.Parameters["CostCenterName"].Value = lblCostCenterName.Text;

                rptForm.Parameters["FactorName"].Value = lblFactorName.Text;
                rptForm.Parameters["Curency"].Value = cmbCurency.Text;
                rptForm.Parameters["TypeStage"].Value = "مرحلة الصب";
                rptForm.Parameters["BeforeDate"].Value = txtTotalQty_CastingBefore.Text.ToString();
                rptForm.Parameters["Posted"].Value = txtTotalQty_CastingAfter.Text;
                rptForm.Parameters["Notes"].Value = txtNotes.Text;
                rptForm.Parameters["AfterStoreName"].Value = lblStoreManger.Text;
                rptForm.Parameters["AfterStoreManger"].Value = lblStoreManger.Text;


                rptForm.Parameters["NumberCups"].Value = txtNumberCups.Text;
                rptForm.Parameters["Zircone"].Value = txtZircon.Text;
                rptForm.Parameters["TotalQty_CastingAfter"].Value = txtTotalQty_CastingAfter.Text;
                rptForm.Parameters["NumberCrews"].Value = txtNumberCrews.Text;
                rptForm.Parameters["CupsLost"].Value = txtCupsLost.Text;
                rptForm.Parameters["EstimatedLoss"].Value = txtEstimatedLoss.Text;
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
                subreportBeforeCasting.ReportSource = Manu_CadStage(GridCastingBefore);

                /******************** Report Factory ************************/
                XRSubreport subreportFactor = (XRSubreport)rptForm.FindControl("subRptrptManu_FactoryFactorCommendBefore", true);
                subreportFactor.Visible = IncludeHeader;
                subreportFactor.ReportSource = Manu_CadStage(GridCastingAfter);


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

        private void btnFactory_Click(object sender, EventArgs e)
        {
            string StrSql = @" SELECT 
                             (SELECT   case when  sum(dbo.Manu_AllOrdersDetails.QTY ) >0 then sum( dbo.Manu_AllOrdersDetails.QTY ) else 0 end
                               FROM    dbo.Manu_AllOrdersDetails inner  JOIN
                                       dbo.Manu_TypeStages ON dbo.Manu_AllOrdersDetails.TypeStageID = dbo.Manu_TypeStages.ID INNER JOIN
                                        dbo.Stc_Items ON dbo.Manu_AllOrdersDetails.ItemID = dbo.Stc_Items.ItemID and dbo.Manu_AllOrdersDetails.BranchID = dbo.Stc_Items.BranchID
                              WHERE       Manu_AllOrdersDetails.TypeStageID<6   AND  dbo.Manu_AllOrdersDetails.Cancel = 0 and  Manu_AllOrdersDetails.ShownInNext=1 and  Stc_Items.BaseID=4 AND dbo.Manu_AllOrdersDetails.OrderID = dbo.Manu_AfforestationFactoryMaster.OrderID ) BonesPriceOrder ,
                              CASE WHEN  dbo.Manu_AfforestationFactoryMaster.DateAfter = 0 THEN '0' ELSE SUBSTRING(ltrim(str(DateAfter)) , 1 , 4) + '/' + SUBSTRING(ltrim(str(DateAfter)) , 5 , 2) + '/' + SUBSTRING(ltrim(str(DateAfter)) , 7 , 2) END as OrderDate
	                              , dbo.Manu_AfforestationFactoryMaster.TotalQTY as GoldQTYCloves,Manu_AfforestationFactoryMaster.EquQty, dbo.Manu_OrderRestriction.OrderID, dbo.Manu_OrderRestriction.BranchID, dbo.Acc_Accounts." + PrimaryName + @" as CustomerName
	                            FROM   dbo.Manu_AfforestationFactoryMaster INNER JOIN
                                          dbo.Manu_OrderRestriction ON dbo.Manu_AfforestationFactoryMaster.OrderID = dbo.Manu_OrderRestriction.OrderID and dbo.Manu_AfforestationFactoryMaster.BranchID = dbo.Manu_OrderRestriction.BranchID INNER JOIN
                                          dbo.Acc_Accounts ON dbo.Manu_OrderRestriction.BranchID = dbo.Acc_Accounts.BranchID AND dbo.Manu_OrderRestriction.CustomerID = dbo.Acc_Accounts.AccountID
						                  where Manu_OrderRestriction.Cancel=0 and dbo.Manu_AfforestationFactoryMaster.Cancel=0  and dbo.Manu_AfforestationFactoryMaster.Posted=3 and Manu_AfforestationFactoryMaster.OrderID not in(select OrderID from Manu_CastingOrders where Cancel=0) and Manu_AfforestationFactoryMaster.BranchID=" + MySession.GlobalBranchID;
            DataTable dt = Lip.SelectRecord(StrSql);
        if(dt.Rows.Count>0)
        {
            gridControl3.DataSource = dt;
            lstDetailOrders.AllowNew = true;
            lstDetailOrders.AllowEdit = true;
            lstDetailOrders.AllowRemove = true;
        }
        else
        {
            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد طلبيات منتظرة " : "There are no pending orders");
            return;
        }
        }

        private void txtCommandDate_EditValueChanged(object sender, EventArgs e)
        {
            if (Lip.CheckDateISAvilable(txtCommandDate.Text))
            {
                Messages.MsgWarning(Messages.TitleWorning, Messages.msgTheDateGreaterCurrntDate);
               txtCommandDate.Text = Lip.GetServerDate();
                return;
            }
        }

        
    }
}
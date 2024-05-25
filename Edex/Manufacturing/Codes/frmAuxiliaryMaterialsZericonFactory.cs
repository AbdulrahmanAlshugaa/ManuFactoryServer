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
using Edex.Model;
using Edex.Model.Language;
using DevExpress.XtraSplashScreen;
using Edex.ModelSystem;
using DevExpress.XtraEditors.Repository;
using Edex.DAL.ManufacturingDAL;
using Edex.GeneralObjects.GeneralClasses;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Edex.DAL;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.Accounting;
namespace Edex.Manufacturing.Codes
{
    public partial class frmAuxiliaryMaterialsZericonFactory : BaseForm
    {

        #region Declare

        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailZirconBefore = new BindingList<Manu_AuxiliaryMaterialsDetails>();
        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailZirconAfter = new BindingList<Manu_AuxiliaryMaterialsDetails>();
        public int DocumentTypeBefore = 27;
        public int DocumentTypeAfter = 28;
        string FocusedControl = "";
        private string strSQL = "";
        private string PrimaryName;
        private DataTable dt;
        private DataTable dt1;
        private DataTable dt2;
        private bool IsNewRecord;
        public bool HasColumnErrors = false;
        int rowIndex;
        private AuxiliaryMaterialsDAl cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
     
        private string ItemName;
        private string CaptionItemName;
        private string SizeName;
        #endregion
        public frmAuxiliaryMaterialsZericonFactory()
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
                PrimaryName = "EngName";
                SizeName = "EngSizeName";
            }
            strSQL = "ArbName";
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));            
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = UserInfo.BRANCHID;
            /*********************** Date Format dd/MM/yyyy ****************************/
            InitializeFormatDate(txtCommandDate);

            txtCommandDate.ReadOnly = false;
            this.KeyDown+=frmZericonFactory_KeyDown;

            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);           
            this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating); 
            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
            this.txtEmployeeStokID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmployeeStokID_Validating);
            this.txtCommandID.Validating += txtCommandID_Validating;

            this.GridZirconBefore.InitNewRow += GridZirconBefore_InitNewRow;
            this.gridControlZirconBefore.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControlZirconBefore_ProcessGridKey);
            this.GridZirconBefore.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridZirconBefore_ValidatingEditor);
            this.GridZirconBefore.ValidateRow += GridZirconBefore_ValidateRow;
            this.gridControlZirconAfter.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControlZirconAfter_ProcessGridKey);
            this.GridZirconAfter.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridZirconAfter_ValidatingEditor);

            this.GridZirconBefore.RowUpdated += GridZirconBefore_RowUpdated;
            this.GridZirconAfter.RowUpdated += GridZirconAfter_RowUpdated;
            this.txtAccountID.Validating += txtAccountID_Validating;


            FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", "ArbName", "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbTypeStage.EditValue = 3;
            cmbTypeStage.ReadOnly = true;
        }
        void GridZirconBefore_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            SumTotalBalance(GridZirconBefore, 1);
        }
        void GridZirconAfter_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            SumTotalBalance(GridZirconAfter, 2);
        }
       
        private void frmZericonFactory_Load(object sender, EventArgs e)
        {
            try
            {
               
                initGridZirconBefore();
                initGridZirconAfter();

                 DoNew();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #region Event
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
        private void GridZirconBefore_CustomDrawCell_1(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "Fingerprint")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;

                GridZirconBefore.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridZirconBefore.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;

            }

        }

        private void GridZirconAfter_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "Fingerprint")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;

                GridZirconAfter.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridZirconAfter.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;

            }
        }
        void GridZirconBefore_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {

            try
            {

                foreach (GridColumn col in GridZirconBefore.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "CostPrice")
                    {

                        var val = GridZirconBefore.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridZirconBefore.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                        else if (!(double.TryParse(val.ToString(), out num)) && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridZirconBefore.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridZirconBefore.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }
                        else
                        {
                            e.Valid = true;
                            GridZirconBefore.SetColumnError(col, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        private void GridZirconBefore_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridZirconBefore.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "BarCode" || ColName == "SizeID" || ColName == "CostPrice" || ColName == "ItemID" || ColName == "StoreID"|| ColName=="EmpFactorID" || ColName == "QTY")
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
                        view.SetColumnError(GridZirconBefore.Columns[ColName], "");

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridZirconBefore.SetColumnError(GridZirconBefore.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal PriceUnit = Comon.cDec(GridZirconBefore.GetFocusedRowCellValue("CostPrice"));
                        decimal Qty = Comon.cDec(val.ToString());
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridZirconBefore.SetFocusedRowCellValue("TotalCost", Total.ToString());
                    }
                    if (ColName == "CostPrice")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridZirconBefore.SetColumnError(GridZirconBefore.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal PriceUnit = Comon.cDec(val.ToString());
                        decimal Qty = Comon.cDec(GridZirconBefore.GetFocusedRowCellValue("QTY"));
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridZirconBefore.SetFocusedRowCellValue("TotalCost", Total.ToString());

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
                                view.SetColumnError(GridZirconBefore.Columns[ColName], "");
                                GridZirconBefore.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                GridZirconBefore.FocusedColumn = GridZirconBefore.VisibleColumns[0];
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
                            view.SetColumnError(GridZirconBefore.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridZirconBefore.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridZirconBefore.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridZirconBefore.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridZirconBefore.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "EmpFactorID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 ");
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridZirconBefore.SetFocusedRowCellValue("EmpFactorName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridZirconBefore.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select SizeID from Stc_SizingUnits Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {

                        GridZirconBefore.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                        e.Valid = true;
                        view.SetColumnError(GridZirconBefore.Columns[ColName], "");

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridZirconBefore.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridZirconBefore.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }

                else if (ColName == "EmpFactorName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpFactorID from HR_EmployeeFile Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridZirconBefore.SetFocusedRowCellValue("EmpFactorID", dtItemID.Rows[0]["EmpFactorID"]);
                        e.Valid = true;
                        view.SetColumnError(GridZirconBefore.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  ItemID from Stc_Items  Where Cancel =0 and LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
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
            SumTotalBalance(GridZirconBefore, 1);

        }
        private void GridZirconAfter_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridZirconAfter.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "BarCode" || ColName == "SizeID" || ColName == "CostPrice" || ColName == "ItemID" || ColName == "StoreID" || ColName == "QTY")
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
                        view.SetColumnError(GridZirconAfter.Columns[ColName], "");
                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridZirconAfter.SetColumnError(GridZirconAfter.Columns["QTY"], "");
                        e.ErrorText = "";
                        decimal PriceUnit = Comon.cDec(GridZirconAfter.GetFocusedRowCellValue("CostPrice"));
                        decimal Qty = Comon.cDec(val.ToString());
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridZirconAfter.SetFocusedRowCellValue("TotalCost", Total.ToString());
                    }
                    if (ColName == "CostPrice")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridZirconAfter.SetColumnError(GridZirconAfter.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal PriceUnit = Comon.cDec(val.ToString());
                        decimal Qty = Comon.cDec(GridZirconAfter.GetFocusedRowCellValue("QTY"));
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridZirconAfter.SetFocusedRowCellValue("TotalCost", Total.ToString());

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
                                view.SetColumnError(GridZirconAfter.Columns[ColName], "");
                                GridZirconAfter.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                GridZirconAfter.FocusedColumn = GridZirconAfter.VisibleColumns[0];
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
                            view.SetColumnError(GridZirconBefore.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridZirconAfter.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridZirconAfter.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridZirconAfter.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridZirconAfter.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "EmpFactorID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 ");
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridZirconAfter.SetFocusedRowCellValue("EmpFactorName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridZirconAfter.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select SizeID from Stc_SizingUnits Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {

                        GridZirconAfter.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                        e.Valid = true;
                        view.SetColumnError(GridZirconAfter.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridZirconAfter.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridZirconAfter.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }

                else if (ColName == "EmpFactorName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpFactorID from HR_EmployeeFile Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridZirconAfter.SetFocusedRowCellValue("EmpFactorID", dtItemID.Rows[0]["EmpFactorID"]);
                        e.Valid = true;
                        view.SetColumnError(GridZirconAfter.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  ItemID from Stc_Items  Where Cancel =0 and LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
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
            SumTotalBalance(GridZirconAfter, 2);
        }
        private void gridControlZirconAfter_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.GridZirconBefore.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridZirconBefore.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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
                                view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgInputIsRequired);
                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridZirconBefore.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridZirconBefore.Columns[ColName], "");
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

        private void gridControlZirconBefore_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.GridZirconAfter.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridZirconAfter.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));


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
                                view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridZirconAfter.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridZirconAfter.Columns[ColName], "");
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
        void GridZirconBefore_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }
        
        private void txtDelegateID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as DelegateName FROM Sales_SalesDelegate WHERE DelegateID =" + txtDelegateID.Text + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
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
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string strSql;
                DataTable dt;
                if (txtStoreID.Text != string.Empty && txtStoreID.Text != "0")
                {
                    DataTable dtt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                    DataRow[] row = dtt.Select("AccountID=" + txtStoreID.Text);

                    if (row.Length > 0)
                    {
                        lblStoreName.Text = row[0]["ArbName"].ToString();
                    }
                }
                else
                {
                    lblStoreName.Text = "";
                    txtStoreID.Text = "";

                }
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
         
        private void txtEmployeeStokID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokID, lblEmployeeStokName, strSQL);//This Call  Function For Set  TypeName to txttypeName when The user Select TypeID
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

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
        }
        #endregion
        #region InitGrids
        void initGridZirconBefore()
        {
            lstDetailZirconBefore = new BindingList<Manu_AuxiliaryMaterialsDetails>();
            lstDetailZirconBefore.AllowNew = true;
            lstDetailZirconBefore.AllowEdit = true;
            lstDetailZirconBefore.AllowRemove = true;
            gridControlZirconBefore.DataSource = lstDetailZirconBefore;
            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID, " + PrimaryName + "   FROM Stc_SizingUnits ");
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameUnit[i] = dtitems.Rows[i][PrimaryName].ToString();
                 
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControlZirconBefore.RepositoryItems.Add(riComboBoxitems);

            GridZirconBefore.Columns["ArbItemName"].Visible = GridZirconBefore.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridZirconBefore.Columns["EngItemName"].Visible = GridZirconBefore.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            GridZirconBefore.Columns[ItemName].Visible = true;
            GridZirconBefore.Columns[ItemName].Caption = CaptionItemName;
            GridZirconBefore.Columns[SizeName].ColumnEdit = riComboBoxitems;

            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlZirconBefore.RepositoryItems.Add(riComboBoxitems2);
            GridZirconBefore.Columns["StoreName"].ColumnEdit = riComboBoxitems2;
           
            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlZirconBefore.RepositoryItems.Add(riComboBoxitems3);
            GridZirconBefore.Columns["EmpFactorName"].ColumnEdit = riComboBoxitems3;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlZirconBefore.RepositoryItems.Add(riComboBoxitems4);
            GridZirconBefore.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridZirconBefore.Columns["CommandID"].Visible = false;
            GridZirconBefore.Columns["BranchID"].Visible = false;
            GridZirconBefore.Columns["FacilityID"].Visible = false;
            GridZirconBefore.Columns["TypeOpration"].Visible = false;          
           
            GridZirconBefore.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            GridZirconBefore.Columns["TotalCost"].OptionsColumn.AllowFocus = false;

            GridZirconBefore.Columns["DateROrD"].OptionsColumn.ReadOnly = true;
            GridZirconBefore.Columns["DateROrD"].OptionsColumn.AllowFocus = false;
            GridZirconBefore.Columns["TimeROrD"].OptionsColumn.ReadOnly = true;
            GridZirconBefore.Columns["TimeROrD"].OptionsColumn.AllowFocus = false;
            GridZirconBefore.Columns["EmpFactorName"].Visible = false;
            GridZirconBefore.Columns["StoreName"].Visible = false;
            GridZirconBefore.Columns["EmpFactorID"].Visible = false;
            GridZirconBefore.Columns["StoreID"].Visible = false;

            GridZirconBefore.Columns["EmpFactorName"].Width = 150;
            GridZirconBefore.Columns[ItemName].Width = 150;
            GridZirconBefore.Columns[SizeName].Width = 120;
            GridZirconBefore.Columns["StoreName"].Width = 120;
            GridZirconBefore.Columns["EmpFactorID"].Width = 130;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridZirconBefore.Columns["EngItemName"].Visible = false;
                GridZirconBefore.Columns["EngSizeName"].Visible = false;
                GridZirconBefore.Columns["SizeID"].Caption = "رقم الوحدة";
                GridZirconBefore.Columns["ItemID"].Caption = "رقم الصنــف";
                GridZirconBefore.Columns["BarCode"].Caption = "باركود الصنف";
                GridZirconBefore.Columns[SizeName].Caption = "الوحدة ";
                GridZirconBefore.Columns["QTY"].Caption = "الكمية ";
                GridZirconBefore.Columns["CostPrice"].Caption = "القيمة";
                GridZirconBefore.Columns["TotalCost"].Caption = "الإجمالي ";
                GridZirconBefore.Columns["Fingerprint"].Caption = "البصمــة";
                GridZirconBefore.Columns["DateROrD"].Caption = "التاريــخ";

                GridZirconBefore.Columns["TimeROrD"].Caption = "الوقـــت";
                GridZirconBefore.Columns["StoreID"].Caption = "رقم المخزن ";
                GridZirconBefore.Columns["StoreName"].Caption = "إسم المخزن";
                GridZirconBefore.Columns["EmpFactorID"].Caption = " رقم العامل";
                GridZirconBefore.Columns["EmpFactorName"].Caption = "إسم العــامل ";
            }
            else
            {
                GridZirconBefore.Columns["ArbItemName"].Visible = false;
                GridZirconBefore.Columns["ArbSizeName"].Visible = false;
                GridZirconBefore.Columns["SizeID"].Caption = "Unit ID";
                GridZirconBefore.Columns["ItemID"].Caption = "Item ID";
                GridZirconBefore.Columns["BarCode"].Caption = "BarCode";
                GridZirconBefore.Columns["MachineName"].Caption = "Machin Name";
                GridZirconBefore.Columns[SizeName].Caption = "Unit Name ";
                GridZirconBefore.Columns["CostPrice"].Caption = "Cost Price";
                GridZirconBefore.Columns["QTY"].Caption = "QTY";
                GridZirconBefore.Columns["TotalCost"].Caption = "Total Cost ";
                GridZirconBefore.Columns["DateRorD"].Caption = "Date";
                GridZirconBefore.Columns["Fingerprint"].Caption = "Fingerprint";
                GridZirconBefore.Columns["TimeROrD"].Caption = "Time";
                GridZirconBefore.Columns["StoreID"].Caption = "Store ID";
                GridZirconBefore.Columns["StoreName"].Caption = "Store Name";
                GridZirconBefore.Columns["EmpFactorID"].Caption = "Emp Factor ID";
                GridZirconBefore.Columns["EmpFactorName"].Caption = "Emp Factor Name";

            }

        }

        void initGridZirconAfter()
        {

            lstDetailZirconAfter = new BindingList<Manu_AuxiliaryMaterialsDetails>();
            lstDetailZirconAfter.AllowNew = true;
            lstDetailZirconAfter.AllowEdit = true;
            lstDetailZirconAfter.AllowRemove = true;

            gridControlZirconAfter.DataSource = lstDetailZirconAfter;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits ");
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                    NameUnit[i] = dtitems.Rows[i][PrimaryName].ToString();
               
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControlZirconAfter.RepositoryItems.Add(riComboBoxitems);



            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                if (UserInfo.Language == iLanguage.Arabic)
                    StoreName[i] = dtStore.Rows[i]["ArbName"].ToString();
                else
                    StoreName[i] = dtStore.Rows[i]["EngName"].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlZirconAfter.RepositoryItems.Add(riComboBoxitems2);
            GridZirconAfter.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlZirconAfter.RepositoryItems.Add(riComboBoxitems3);
            GridZirconAfter.Columns["EmpFactorName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlZirconAfter.RepositoryItems.Add(riComboBoxitems4);
            GridZirconAfter.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridZirconAfter.Columns["ArbItemName"].Visible = GridZirconAfter.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridZirconAfter.Columns["EngItemName"].Visible = GridZirconAfter.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            GridZirconAfter.Columns[ItemName].Visible = true;
            GridZirconAfter.Columns[ItemName].Caption = CaptionItemName;

            GridZirconAfter.Columns[SizeName].ColumnEdit = riComboBoxitems;
            GridZirconAfter.Columns["CommandID"].Visible = false;

            GridZirconAfter.Columns["BranchID"].Visible = false;
            GridZirconAfter.Columns["FacilityID"].Visible = false;
         
            GridZirconAfter.Columns["TypeOpration"].Visible = false;
         
            GridZirconAfter.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            GridZirconAfter.Columns["TotalCost"].OptionsColumn.AllowFocus = false;

            GridZirconAfter.Columns["DateROrD"].OptionsColumn.ReadOnly = true;
            GridZirconAfter.Columns["DateROrD"].OptionsColumn.AllowFocus = false;
            GridZirconAfter.Columns["TimeROrD"].OptionsColumn.ReadOnly = true;
            GridZirconAfter.Columns["TimeROrD"].OptionsColumn.AllowFocus = false;

            GridZirconAfter.Columns["EmpFactorName"].Visible = false;
            GridZirconAfter.Columns["StoreName"].Visible = false;
            GridZirconAfter.Columns["EmpFactorID"].Visible = false;
            GridZirconAfter.Columns["StoreID"].Visible = false;

            GridZirconAfter.Columns["EmpFactorName"].Width = 150;
            GridZirconAfter.Columns[ItemName].Width = 150;
            GridZirconAfter.Columns[SizeName].Width = 120;
            GridZirconAfter.Columns["StoreName"].Width = 120;
            GridZirconAfter.Columns["EmpFactorID"].Width = 130;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridZirconAfter.Columns["EngItemName"].Visible = false;
                GridZirconAfter.Columns["EngSizeName"].Visible = false;
                GridZirconAfter.Columns["SizeID"].Caption = "رقم الوحدة";
                GridZirconAfter.Columns["ItemID"].Caption = "رقم الصنــف";
                GridZirconAfter.Columns["BarCode"].Caption = "باركود الصنف";
                GridZirconAfter.Columns[SizeName].Caption = "الوحدة ";
                GridZirconAfter.Columns["QTY"].Caption = "الكمية ";
                GridZirconAfter.Columns["CostPrice"].Caption = "القيمة";
                GridZirconAfter.Columns["TotalCost"].Caption = "الإجمالي ";
                GridZirconAfter.Columns["Fingerprint"].Caption = "البصمــة";
                GridZirconAfter.Columns["DateROrD"].Caption = "التاريــخ";
                GridZirconAfter.Columns["TimeROrD"].Caption = "الوقـــت";
                GridZirconAfter.Columns["StoreID"].Caption = "رقم المخزن ";
                GridZirconAfter.Columns["StoreName"].Caption = "إسم المخزن";
                GridZirconAfter.Columns["EmpFactorID"].Caption = " رقم العامل";
                GridZirconAfter.Columns["EmpFactorName"].Caption = "إسم العــامل ";
            }
            else
            {
                GridZirconAfter.Columns["ArbItemName"].Visible = false;
                GridZirconAfter.Columns["ArbSizeName"].Visible = false;
                GridZirconAfter.Columns["SizeID"].Caption = "Unit ID";
                GridZirconAfter.Columns["ItemID"].Caption = "Item ID";
                GridZirconAfter.Columns["BarCode"].Caption = "BarCode";
                GridZirconAfter.Columns["MachineName"].Caption = "Machin Name";
                GridZirconAfter.Columns[SizeName].Caption = "Unit Name ";
                GridZirconAfter.Columns["CostPrice"].Caption = "Cost Price";
                GridZirconAfter.Columns["QTY"].Caption = "QTY";
                GridZirconAfter.Columns["TotalCost"].Caption = "Total Cost ";
                GridZirconAfter.Columns["DateRorD"].Caption = "Date";
                GridZirconAfter.Columns["Fingerprint"].Caption = "Fingerprint";

                GridZirconAfter.Columns["TimeROrD"].Caption = "Time";
                GridZirconAfter.Columns["StoreID"].Caption = "Store ID ";
                GridZirconAfter.Columns["StoreName"].Caption = "Store Name";
                GridZirconAfter.Columns["EmpFactorID"].Caption = "Emp Factor ID";
                GridZirconAfter.Columns["EmpFactorName"].Caption = "Emp Factor Name";
            }

        }

       #endregion 
        #region Function

        private void FileItemData(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["QTY"], 0);
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["DateRorD"], DateTime.Now.ToString("yyyy/MM/dd"));
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["TimeROrD"], DateTime.Now.ToString("hh:mm:tt"));

            }
            else
            {
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["QTY"], "0");                 
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["SizeID"], "");
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns[SizeName],"");
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["BarCode"], "");
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["ItemID"], "");
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns[ItemName], "");
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["DateRorD"], "");
                GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["TimeROrD"], "");
            }
        }

        private void FileItemData2(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["Qty"], dt.Rows[0]["QTY"].ToString());
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["DateRorD"], DateTime.Now.ToString("yyyy/MM/dd"));
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["TimeROrD"], DateTime.Now.ToString("hh:mm:tt"));
            }
            else
            {
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["Qty"], "0");
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["SizeID"],"");
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns[SizeName], "");
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["BarCode"], "");
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["ItemID"], "");
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns[ItemName], "");
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["DateRorD"], "");
                GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["TimeROrD"], "");
            }
        }
        public void ClearFields()
        {
            try
            {
                txtDelegateID.Text = "";

                lblDelegateName.Text = "";
                txtNotes.Text = "";
                txtCommandDate.EditValue = DateTime.Now;

                 
                txtCustomerID.Text = "";
                txtCustomerID_Validating(null, null);
                txtReferanceID.Text = "";

                txtEmployeeStokID.Text = "";
                txtEmployeeStokID_Validating(null, null);

                // GetAccountsDeclaration();

                txtDelegateID.Text = MySession.GlobalDefaultSaleDelegateID;
                txtDelegateID_Validating(null, null);

                 

                txtStoreID.Text = "";
                txtStoreID_Validating(null, null);
                lstDetailZirconBefore = new BindingList<Manu_AuxiliaryMaterialsDetails>();
                lstDetailZirconBefore.AllowNew = true;
                lstDetailZirconBefore.AllowEdit = true;
                lstDetailZirconBefore.AllowRemove = true;
                gridControlZirconBefore.DataSource = lstDetailZirconBefore;

                lstDetailZirconAfter = new BindingList<Manu_AuxiliaryMaterialsDetails>();
                lstDetailZirconAfter.AllowNew = true;
                lstDetailZirconAfter.AllowEdit = true;
                lstDetailZirconAfter.AllowRemove = true;
                gridControlZirconAfter.DataSource = lstDetailZirconAfter;
                dt = new DataTable();



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
                //if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "ZericonCommend", "رقـم الأمر", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "ZericonCommend", "Commend ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmployeeStokID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, lblEmployeeStokName, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, lblEmployeeStokName, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            
            if (FocusedControl.Trim() == txtCustomerID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtAccountID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtFactorID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
            
            else if (FocusedControl.Trim() == gridControlZirconBefore.Name)
            {
                if (GridZirconBefore.FocusedColumn == null) return;
                if (GridZirconBefore.FocusedColumn.Name == "colBarCode" || GridZirconBefore.FocusedColumn.Name == "colItemName" || GridZirconBefore.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                }

                else if (GridZirconBefore.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                else if (GridZirconBefore.FocusedColumn.Name == "colSizeID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }

                else if (GridZirconBefore.FocusedColumn.Name == "colEmpFactorID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlZirconAfter.Name)
            {
                if (GridZirconAfter.FocusedColumn == null) return;
                if (GridZirconAfter.FocusedColumn.Name == "colBarCode" || GridZirconAfter.FocusedColumn.Name == "colItemName" || GridZirconAfter.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                }
                else if (GridZirconAfter.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }

                else if (GridZirconAfter.FocusedColumn.Name == "colSizeID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridZirconAfter.FocusedColumn.Name == "colEmpFactorID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
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
        private void txtFactorID_Validating(object sender, CancelEventArgs e)
        {

            try
            {
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtFactorID.Text) + " And Cancel =0 ";
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
                else if (FocusedControl == txtEmployeeStokID.Name)
                {
                    txtEmployeeStokID.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokID_Validating(null, null);
                }
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
 
                else if (FocusedControl == gridControlZirconBefore.Name)
                {
                    if (GridZirconBefore.FocusedColumn.Name == "colBarCode" || GridZirconBefore.FocusedColumn.Name == "colItemName" || GridZirconBefore.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridZirconBefore.AddNewRow();

                        GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));

                        // CalculateRow();
                    }
                    if (GridZirconBefore.FocusedColumn.Name == "colStoreID")
                    {
                        GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["StoreName"], Lip.GetValue(strSQL));

                    }
                    if (GridZirconBefore.FocusedColumn.Name == "colSizeID")
                    {
                        GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as "+SizeName+" FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridZirconBefore.FocusedColumn.Name == "colEmpFactorID")
                    {
                        GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["EmpFactorID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridZirconBefore.SetRowCellValue(GridZirconBefore.FocusedRowHandle, GridZirconBefore.Columns["EmpFactorName"], Lip.GetValue(strSQL));
                    }
                }

                else if (FocusedControl == gridControlZirconAfter.Name)
                {
                    if (GridZirconAfter.FocusedColumn.Name == "colBarCode" || GridZirconAfter.FocusedColumn.Name == "colItemName" || GridZirconAfter.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridZirconAfter.AddNewRow();

                        GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["BarCode"], Barcode);
                        FileItemData2(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));
                        // CalculateRow();
                    }
                    if (GridZirconAfter.FocusedColumn.Name == "colStoreID")
                    {
                        GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["StoreName"], Lip.GetValue(strSQL));
                    }

                    if (GridZirconAfter.FocusedColumn.Name == "colSizeID")
                    {
                        GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as "+SizeName+" FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridZirconAfter.FocusedColumn.Name == "colEmpFactorID")
                    {
                        GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["EmpFactorID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridZirconAfter.SetRowCellValue(GridZirconAfter.FocusedRowHandle, GridZirconAfter.Columns["EmpFactorName"], Lip.GetValue(strSQL));
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



            foreach (GridColumn col in GridZirconBefore.Columns)
            {
                if (col.FieldName == "BarCode")
                {

                    GridZirconBefore.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridZirconBefore.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridZirconBefore.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }

            }
            foreach (GridColumn col in GridZirconAfter.Columns)
            {
                if (col.FieldName == "BarCode")
                {
                    GridZirconAfter.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridZirconAfter.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridZirconAfter.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
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
                txtCommandID.Text = AuxiliaryMaterialsDAl.GetNewID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), 2) + "";
                ClearFields();
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

                Manu_AuxiliaryMaterialsMaster model = new Manu_AuxiliaryMaterialsMaster();
                model.CommandID = Comon.cInt(txtCommandID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());
                model.TypeCommand = 2;
                string Result = AuxiliaryMaterialsDAl.Delete(model);
                 
                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                    int MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeBefore);
                    MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
                    if (MoveID <0)
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
            dtItem.Columns.Add("DateROrD", System.Type.GetType("System.String"));
            dtItem.Columns.Add("FacilityID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("Fingerprint", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("TypeOpration", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("SizeID", System.Type.GetType("System.Int32"));

            dtItem.Columns.Add("StoreID", System.Type.GetType("System.Int32"));
            dtItem.Columns.Add("StoreName", System.Type.GetType("System.String"));

            dtItem.Columns.Add("EmpFactorID", System.Type.GetType("System.Int64"));
            dtItem.Columns.Add("EmpFactorName", System.Type.GetType("System.String"));

            dtItem.Columns.Add("TimeROrD", System.Type.GetType("System.String"));
            for (int i = 0; i <= GridZirconBefore.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["CommandID"] = Comon.cInt(txtCommandID.Text);
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem.Rows[i]["BarCode"] = GridZirconBefore.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i][ItemName] = GridZirconBefore.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtItem.Rows[i][SizeName] = GridZirconBefore.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["SizeID"] = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i]["TypeOpration"] = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "TypeOpration").ToString());
                dtItem.Rows[i]["Fingerprint"] = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "Fingerprint").ToString());
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(GridZirconBefore.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["TotalCost"] = Comon.ConvertToDecimalPrice(GridZirconBefore.GetRowCellValue(i, "TotalCost").ToString());

                dtItem.Rows[i]["DateROrD"] = GridZirconBefore.GetRowCellValue(i, "DateROrD").ToString();
                dtItem.Rows[i]["StoreID"] = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["StoreName"] = GridZirconBefore.GetRowCellValue(i, "StoreName").ToString();
                dtItem.Rows[i]["TimeROrD"] =Comon.cDbl( GridZirconBefore.GetRowCellValue(i, "TimeROrD").ToString());
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(GridZirconBefore.GetRowCellValue(i, "CostPrice").ToString());

                dtItem.Rows[i]["EmpFactorID"] = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "EmpFactorID").ToString());
                dtItem.Rows[i]["EmpFactorName"] = GridZirconBefore.GetRowCellValue(i, "EmpFactorName").ToString();


            }

            gridControlZirconBefore.DataSource = dtItem;

            DataTable dtItem1 = new DataTable();
            dtItem1.Columns.Add("CommandID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("BarCode", System.Type.GetType("System.String"));
            dtItem1.Columns.Add("BranchID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem1.Columns.Add("DateROrD", System.Type.GetType("System.String"));

            dtItem1.Columns.Add("FacilityID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("Fingerprint", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("ItemID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("QTY", System.Type.GetType("System.Decimal"));

            dtItem1.Columns.Add(SizeName, System.Type.GetType("System.String"));
            dtItem1.Columns.Add("CostPrice", System.Type.GetType("System.Decimal"));
            dtItem1.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
            dtItem1.Columns.Add("TypeOpration", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("SizeID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("StoreID", System.Type.GetType("System.Int32"));
            dtItem1.Columns.Add("StoreName", System.Type.GetType("System.String"));
            dtItem1.Columns.Add("TimeROrD", System.Type.GetType("System.String"));
            dtItem1.Columns.Add("EmpFactorID", System.Type.GetType("System.Int64"));
            dtItem1.Columns.Add("EmpFactorName", System.Type.GetType("System.String"));
            for (int i = 0; i <= GridZirconAfter.DataRowCount - 1; i++)
            {
                dtItem1.Rows.Add();
                dtItem1.Rows[i]["CommandID"] = Comon.cInt(txtCommandID.Text);
                dtItem1.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem1.Rows[i]["BarCode"] = GridZirconAfter.GetRowCellValue(i, "BarCode").ToString();
                dtItem1.Rows[i]["ItemID"] = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "ItemID").ToString());
                dtItem1.Rows[i][ItemName] = GridZirconAfter.GetRowCellValue(i, ItemName).ToString();
                dtItem1.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtItem1.Rows[i][SizeName] = GridZirconAfter.GetRowCellValue(i, SizeName).ToString();
                dtItem1.Rows[i]["SizeID"] = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "SizeID").ToString());
                dtItem1.Rows[i]["TypeOpration"] = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "TypeOpration").ToString());
                dtItem1.Rows[i]["Fingerprint"] = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "Fingerprint").ToString());
                dtItem1.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(GridZirconAfter.GetRowCellValue(i, "QTY").ToString());
                dtItem1.Rows[i]["TotalCost"] = Comon.ConvertToDecimalPrice(GridZirconAfter.GetRowCellValue(i, "TotalCost").ToString());
                dtItem1.Rows[i]["DateROrD"] = GridZirconAfter.GetRowCellValue(i, "DateROrD").ToString();
                dtItem1.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(GridZirconAfter.GetRowCellValue(i, "CostPrice").ToString());
                dtItem1.Rows[i]["StoreID"] = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "StoreID").ToString());
                dtItem1.Rows[i]["StoreName"] = GridZirconAfter.GetRowCellValue(i, "StoreName").ToString();
                dtItem1.Rows[i]["TimeROrD"] =Comon.cDbl( GridZirconAfter.GetRowCellValue(i, "TimeROrD").ToString());

                dtItem1.Rows[i]["EmpFactorID"] = Comon.cDbl(GridZirconAfter.GetRowCellValue(i, "EmpFactorID").ToString());
                dtItem1.Rows[i]["EmpFactorName"] = GridZirconAfter.GetRowCellValue(i, "EmpFactorName").ToString();
            }
            gridControlZirconAfter.DataSource = dtItem1;
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);

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
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_ZirconBefore.Text);
            returned.Debit = Comon.cDbl(txtTotalPrice_ZirconBefore.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtStoreID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(txtTotalPrice_ZirconBefore.Text);
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_ZirconBefore.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        long SaveVariousVoucherMachinInOn(int DocumentID,bool isNew)
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
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
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
            objRecord.Posted = 1;

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
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_ZirconAfter.Text);
            returned.Debit = Comon.cDbl(txtTotalPrice_ZirconAfter.Text);
            returned.Declaration = txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //Credit Matirial      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAccountID.Text);
            returned.VoucherID = VoucherID;
            returned.Credit = Comon.cDbl(txtTotalPrice_ZirconAfter.Text);
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_ZirconAfter.Text);
            returned.Declaration = txtNotes.Text == string.Empty ? this.Text : txtNotes.Text;
            returned.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            listreturned.Add(returned);

            //=
            if (listreturned.Count > 0)
            {
                objRecord.VariousVoucherDetails = listreturned;
                Result = VariousVoucherMachinDAL.InsertUsingXML(objRecord, isNew);
            }
            return Result;
        }
        private void SaveOutOn()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();

            if (IsNewRecord)
                objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBefore);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
                    isNew = true;

                }

            }
           
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordOutOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordOutOnMaster.FacilityID = UserInfo.FacilityID;
            objRecordOutOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordOutOnMaster.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecordOutOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordOutOnMaster.TypeCommand = 1;
            objRecordOutOnMaster.DocumentType = DocumentTypeBefore;
            objRecordOutOnMaster.Cancel = 0;
            objRecordOutOnMaster.DebitAccount = Comon.cDbl(txtAccountID.Text);
            objRecordOutOnMaster.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecordOutOnMaster.Notes = txtNotes.Text;
            objRecordOutOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordOutOnMaster.UserID = UserInfo.ID;
            objRecordOutOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordOutOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordOutOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordOutOnMaster.EditUserID = 0;
            objRecordOutOnMaster.EditTime = 0;
            objRecordOutOnMaster.EditDate = 0;
            objRecordOutOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= GridZirconBefore.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridZirconBefore.GetRowCellValue(i, "DateROrD").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridZirconBefore.GetRowCellValue(i, "TimeROrD")).ToShortTimeString());
                returnedOutOn.BarCode = GridZirconBefore.GetRowCellValue(i, "BarCode").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "QTY").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "CostPrice").ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordOutOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordOutOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachin(Comon.cInt(objRecordOutOnMaster.InvoiceID), isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + AuxiliaryMaterialsDAl.TableName + " Set RegistrationNo =" + VoucherID + " where " + AuxiliaryMaterialsDAl.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }
        private void SaveInOn()
        {
            #region Save Out On
            //Save Out On
            bool isNew = IsNewRecord;
            Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
                if (dtInvoiceID.Rows.Count > 0)
                    objRecordInOnMaster.InvoiceID = Comon.cInt(dtInvoiceID.Rows[0][0]);
                else
                {
                    objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
                    isNew = true;
                }
            }
            objRecordInOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordInOnMaster.InvoiceDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecordInOnMaster.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecordInOnMaster.FacilityID = UserInfo.FacilityID;
            objRecordInOnMaster.CommandID = Comon.cInt(txtCommandID.Text);
            objRecordInOnMaster.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecordInOnMaster.ReferanceID = Comon.cInt(txtReferanceID.Text);
            objRecordInOnMaster.TypeCommand = 2;
            objRecordInOnMaster.DocumentType = DocumentTypeAfter;
            objRecordInOnMaster.Cancel = 0;
            objRecordInOnMaster.DebitAccount = Comon.cDbl(txtAccountID.Text);
            objRecordInOnMaster.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecordInOnMaster.Notes = txtNotes.Text;
            objRecordInOnMaster.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            //user Info
            objRecordInOnMaster.UserID = UserInfo.ID;
            objRecordInOnMaster.RegDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
            objRecordInOnMaster.RegTime = Comon.cDbl(Lip.GetServerTimeSerial());
            objRecordInOnMaster.ComputerInfo = UserInfo.ComputerInfo;
            objRecordInOnMaster.EditUserID = 0;
            objRecordInOnMaster.EditTime = 0;
            objRecordInOnMaster.EditDate = 0;
            objRecordInOnMaster.EditComputerInfo = "";
            Stc_ManuFactoryCommendOutOnBail_Details returnedOutOn;
            List<Stc_ManuFactoryCommendOutOnBail_Details> listreturnedOutOn = new List<Stc_ManuFactoryCommendOutOnBail_Details>();
            for (int i = 0; i <= GridZirconAfter.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridZirconAfter.GetRowCellValue(i, "DateROrD").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridZirconAfter.GetRowCellValue(i, "TimeROrD")).ToShortTimeString());
                returnedOutOn.BarCode = GridZirconAfter.GetRowCellValue(i, "BarCode").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridZirconAfter.GetRowCellValue(i, "QTY").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(GridZirconAfter.GetRowCellValue(i, "CostPrice").ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordInOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordInOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinInOn(Comon.cInt(objRecordInOnMaster.InvoiceID),isNew);
                    if (VoucherID == 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                    else
                        Lip.ExecututeSQL("Update " + AuxiliaryMaterialsDAl.TableName + " Set RegistrationNo =" + VoucherID + " where " + AuxiliaryMaterialsDAl.PremaryKey + " = " + txtCommandID.Text);
                }
            }
            #endregion
        }
        private int SaveStockMoveingOut(int DocumentID)
        {
            Stc_ItemsMoviing objRecord = new Stc_ItemsMoviing();
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.DocumentTypeID = DocumentTypeBefore;
            objRecord.MoveType = 2;
            objRecord.MoveID = 0;
            objRecord.TranseID = DocumentID;
            Stc_ItemsMoviing returned;
            List<Stc_ItemsMoviing> listreturned = new List<Stc_ItemsMoviing>();
            for (int i = 0; i <= GridZirconBefore.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeBefore;
                returned.MoveType = 2;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridZirconBefore.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = 0;
                //returned.Bones = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "TotalCost").ToString());
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
            for (int i = 0; i <= GridZirconAfter.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsMoviing();
                returned.ID = i + 1;
                returned.MoveDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
                returned.MoveID = 0;
                returned.TranseID = DocumentID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DocumentTypeID = DocumentTypeAfter;
                returned.MoveType = 1;
                returned.StoreID = Comon.cDbl(txtStoreID.Text.ToString());
                returned.AccountID = Comon.cDbl(txtAccountID.Text);
                returned.BarCode = GridZirconAfter.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "SizeID").ToString());

                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridZirconAfter.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = Comon.cDbl(GridZirconAfter.GetRowCellValue(i, "TotalCost").ToString());
                //returned.Bones = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "Bones").ToString());
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
            GridZirconBefore.Focus();
            GridZirconBefore.MoveLastVisible();
            GridZirconBefore.FocusedColumn = GridZirconBefore.VisibleColumns[1];
            Manu_AuxiliaryMaterialsMaster objRecord = new Manu_AuxiliaryMaterialsMaster();
            objRecord.CommandID = Comon.cInt(txtCommandID.Text);
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.CommandDate = Comon.ConvertDateToSerial(txtCommandDate.Text);
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.AccountID = Comon.cDbl(txtAccountID.Text);
            objRecord.FactorID = Comon.cDbl(txtFactorID.Text);
            objRecord.EmployeeStokID = Comon.cDbl(txtEmployeeStokID.Text);
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);  
            objRecord.DelegetID = Comon.cInt(txtDelegateID.Text);
            objRecord.ReferanceID = Comon.cInt(txtReferanceID.Text);
            txtNotes.Text = (txtNotes.Text.Trim());
            objRecord.Notes = txtNotes.Text;
            objRecord.TypeCommand = 2;
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
            Manu_AuxiliaryMaterialsDetails returned;
            List<Manu_AuxiliaryMaterialsDetails> listreturned = new List<Manu_AuxiliaryMaterialsDetails>();
            for (int i = 0; i <= GridZirconBefore.DataRowCount - 1; i++)
            {
                returned = new Manu_AuxiliaryMaterialsDetails();
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returned.DateROrD = Comon.cDate(GridZirconBefore.GetRowCellValue(i, "DateROrD").ToString());
                returned.TimeROrD = (Comon.cDateTime(GridZirconBefore.GetRowCellValue(i, "TimeROrD")).ToShortTimeString()); 
                returned.BarCode = GridZirconBefore.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "ItemID").ToString());
                
                returned.StoreID = Comon.cLong(txtStoreID.Text.ToString());
                returned.SizeID = Comon.cInt(GridZirconBefore.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.ConvertToDecimalQty(GridZirconBefore.GetRowCellValue(i, "QTY").ToString());
                returned.CostPrice = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "CostPrice").ToString());
                returned.EmpFactorID = Comon.cDbl(txtFactorID.Text.ToString());
                returned.ArbSizeName = GridZirconBefore.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridZirconBefore.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridZirconBefore.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridZirconBefore.GetRowCellValue(i, ItemName).ToString();
                returned.StoreName = lblStoreName.Text.ToString();
                returned.TotalCost = Comon.cDbl(GridZirconBefore.GetRowCellValue(i, "TotalCost").ToString());
                returned.EmpFactorName = lblFactorName.Text.ToString();
                returned.TypeOpration = 1;
                listreturned.Add(returned);
            }
            if (GridZirconAfter.DataRowCount > 0)
            {
                for (int i = 0; i <= GridZirconAfter.DataRowCount - 1; i++)
                {
                    returned = new Manu_AuxiliaryMaterialsDetails();
                    returned.CommandID = Comon.cInt(txtCommandID.Text);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    returned.DateROrD = Comon.cDate(GridZirconAfter.GetRowCellValue(i, "DateROrD").ToString());
                    returned.TimeROrD = (Comon.cDateTime(GridZirconAfter.GetRowCellValue(i, "TimeROrD")).ToShortTimeString());
                    returned.StoreID = Comon.cLong(txtStoreID.Text.ToString()); 
                    returned.EmpFactorID = Comon.cDbl(txtFactorID.Text.ToString());
                    returned.BarCode = GridZirconAfter.GetRowCellValue(i, "BarCode").ToString();
                    returned.ItemID = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "ItemID").ToString());
                    returned.SizeID = Comon.cInt(GridZirconAfter.GetRowCellValue(i, "SizeID").ToString());
                    returned.QTY = Comon.ConvertToDecimalQty(GridZirconAfter.GetRowCellValue(i, "QTY").ToString());
                    returned.CostPrice = Comon.cDbl(GridZirconAfter.GetRowCellValue(i, "CostPrice").ToString());
                    returned.ArbSizeName = GridZirconAfter.GetRowCellValue(i, SizeName).ToString();
                    returned.EngSizeName = GridZirconAfter.GetRowCellValue(i, SizeName).ToString();
                    returned.ArbItemName = GridZirconAfter.GetRowCellValue(i, ItemName).ToString();
                    returned.EngItemName = GridZirconAfter.GetRowCellValue(i, ItemName).ToString();
                    returned.StoreName = lblStoreName.Text.ToString();
                    returned.TotalCost = Comon.cDbl(GridZirconAfter.GetRowCellValue(i, "TotalCost").ToString());
                    returned.EmpFactorName = lblFactorName.Text.ToString();
                    returned.TypeOpration = 2;
                    listreturned.Add(returned);
                }
            }
            if (listreturned.Count > 0)
            {
                objRecord.Menu_F_AuxiliaryMaterials = listreturned;
                string Result = AuxiliaryMaterialsDAl.InsertUsingXML(objRecord, IsNewRecord);

                if (Comon.cInt(Result) > 0)
                {
                  
                    SaveOutOn(); //حفظ   الصرف المخزني
                    // حفظ الحركة المخزنية 
                    if (Comon.cInt(Result) > 0)
                    {
                        int MoveID = SaveStockMoveingOut(Comon.cInt(txtCommandID.Text));
                        if (MoveID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
                    }
                    
                    if (GridZirconAfter.DataRowCount > 0)
                    {
                        SaveInOn(); //حفظ   التوريد المخزني
                        // حفظ الحركة المخزنية 
                        if (Comon.cInt(Result) > 0)
                        {
                            int MoveID = SaveStockMoveingIn(Comon.cInt(txtCommandID.Text));
                            if (MoveID == 0)
                                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
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

        bool IsValidGrid()
        {
            double num;

            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            GridZirconBefore.MoveLast();

            int length = GridZirconBefore.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in GridZirconBefore.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "CostPrice" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
                    {

                        var cellValue = GridZirconBefore.GetRowCellValue(i, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            GridZirconBefore.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;

                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            GridZirconBefore.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            GridZirconBefore.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
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
                    strSQL = "SELECT TOP 1 * FROM " + AuxiliaryMaterialsDAl.TableName + " Where Cancel =0 and TypeCommand=2  And BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + AuxiliaryMaterialsDAl.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + AuxiliaryMaterialsDAl.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + AuxiliaryMaterialsDAl.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + AuxiliaryMaterialsDAl.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + AuxiliaryMaterialsDAl.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + AuxiliaryMaterialsDAl.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new AuxiliaryMaterialsDAl();

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
                decimal CostPrice = 0;
                decimal TotalQtTYBefore = 0;
                decimal TotalCostBefor = 0;
                for (int i = 0; i <= Grid.DataRowCount - 1; i++)
                {
                    QTY = Comon.ConvertToDecimalPrice(Grid.GetRowCellValue(i, "QTY").ToString());
                    CostPrice = Comon.ConvertToDecimalPrice(Grid.GetRowCellValue(i, "CostPrice"));
                    TotalQtTYBefore += QTY;
                    TotalCostBefor += (QTY * CostPrice);
                }
                if (flage == 1)
                {
                    txtTotalQty_ZirconBefore.Text = TotalQtTYBefore + "";
                    txtTotalPrice_ZirconBefore.Text = TotalCostBefor + "";
                    int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                    if (isLocalCurrncy > 1)
                    {

                        lblCurrencyEqvBfore.Text = Comon.cDec(Comon.cDec(txtTotalPrice_ZirconBefore.Text) * Comon.cDec(txtCurrncyPrice.Text)) + "";
                    }
                    else
                    {
                        txtCurrncyPrice.Text = "1";
                        lblCurrencyEqvBfore.Visible = false;
                        lblCurrncyPric.Visible = false;
                        lblcurrncyEquvilant.Visible = false;
                        txtCurrncyPrice.Visible = false;
                    }
                }
                else
                {
                    txtTotalQty_ZirconAfter.Text = TotalQtTYBefore + "";
                    txtTotalPrice_ZirconAfter.Text = TotalCostBefor + "";
                    int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                    if (isLocalCurrncy > 1)
                    {
                        lblCurrencyEqvAfter.Text = Comon.cDec(Comon.cDec(txtTotalPrice_ZirconAfter.Text) * Comon.cDec(txtCurrncyPrice.Text)) + "";
                    }
                    else
                    {
                        txtCurrncyPrice.Text = "1";
                        lblCurrencyEqvAfter.Visible = false;
                        lblCurrncyPric.Visible = false;
                        labelControl2.Visible = false;
                        txtCurrncyPrice.Visible = false;
                    }
                }
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
                string strSql;
                DataTable dt;
                if (txtAccountID.Text != string.Empty && txtAccountID.Text != "0")
                {
                    DataTable dtt = new Acc_AccountsDAL().GetAcc_AccountsByLevel(MySession.GlobalBranchID, MySession.GlobalFacilityID);
                    DataRow[] row = dtt.Select("AccountID=" + txtAccountID.Text);

                    if (row.Length > 0)
                    {
                        lblAccountName.Text = row[0]["ArbName"].ToString();
                    }
                }
                else
                {
                    lblAccountName.Text = "";
                    txtAccountID.Text = "";

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
                    dt = AuxiliaryMaterialsDAl.frmGetDataDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        txtCommandID.Text = dt.Rows[0]["CommandID"].ToString();
                        //Validate
                        cmbCurency.EditValue=Comon.cInt( dt.Rows[0]["CurrencyID"].ToString());                      
                        txtReferanceID.Text = dt.Rows[0]["ReferanceID"].ToString();
                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);

                        txtDelegateID.Text = dt.Rows[0]["DelegetID"].ToString();
                        txtDelegateID_Validating(null, null);

                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);
                        txtFactorID.Text = dt.Rows[0]["FactorID"].ToString();
                        txtFactorID_Validating(null, null);
                        txtEmployeeStokID.Text = dt.Rows[0]["EmployeeStokID"].ToString();
                        txtEmployeeStokID_Validating(null, null);
                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"]);
                        //  var dtr1 = dt.AsEnumerable().Where(row => row.Field<int>("TypeOpration") == 1).ToArray();
                        // dt1 = dt.AsEnumerable().Where(row => row.Field<int>("TypeOpration") == 1).CopyToDataTable();
                        txtAccountID.Text = dt.Rows[0]["AccountID"].ToString();
                        txtAccountID_Validating(null, null);
                        dt1 = dt.Clone();  
                        foreach (DataRow row in dt.Rows)
                        {
                            if (Convert.ToInt32(row["TypeOpration"]) == 1)
                            {
                                //dt1.ImportRow(row);
                                DataRow newRow = dt1.NewRow();
                                newRow.ItemArray = row.ItemArray;                              
                                dt1.Rows.Add(newRow);

                            }
                        }
                        gridControlZirconBefore.DataSource = dt1;
                        lstDetailZirconBefore.AllowNew = true;
                        lstDetailZirconBefore.AllowEdit = true;
                        lstDetailZirconBefore.AllowRemove = true;
                        //dt2 = dt.AsEnumerable().Where(row => row.Field<int>("TypeOpration") == 2).CopyToDataTable();
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


                        gridControlZirconAfter.DataSource = dt2;
                        lstDetailZirconAfter.AllowNew = true;
                        lstDetailZirconAfter.AllowEdit = true;
                        lstDetailZirconAfter.AllowRemove = true;



                        SumTotalBalance(GridZirconBefore, 1);
                        SumTotalBalance(GridZirconAfter, 2);

                        Validations.DoReadRipon(this, ribbonControl1);

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
            int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
            if (isLocalCurrncy > 1)
            {
                decimal CurrncyPrice = Comon.cDec(Lip.GetValue("select ExchangeRate from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                txtCurrncyPrice.Text = CurrncyPrice + "";
                lblCurrencyEqvAfter.Visible = true;
                lblCurrencyEqvBfore.Visible = true;
                lblCurrncyPric.Visible = true;
                lblcurrncyEquvilant.Visible = true;
                labelControl2.Visible = true;
                txtCurrncyPrice.Visible = true;
            }
            else
            {
                txtCurrncyPrice.Text = "1";
                lblCurrencyEqvBfore.Visible = false;
                lblCurrencyEqvAfter.Visible = false;
                lblcurrncyEquvilant.Visible = false;
                lblCurrncyPric.Visible = false;
                labelControl2.Visible = false;
                txtCurrncyPrice.Visible = false;
            }
        }

        private void btnMachinResractionBefore_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBefore);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypeBefore).ToString());
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
        }

        private void btnMachinResractionAfter_Click(object sender, EventArgs e)
        {
            if (IsNewRecord == true) return;
            DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
            if (dtInvoiceID.Rows.Count > 0)
            {
                int ID = Comon.cInt(Lip.GetValue("Select VoucherID From Acc_VariousVoucherMachinMaster Where BranchID= " + Comon.cInt(cmbBranchesID.EditValue.ToString()) + " And DocumentID=" + dtInvoiceID.Rows[0][0] + " And DocumentType=" + DocumentTypeAfter).ToString());
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
        }

        

       
    }
}
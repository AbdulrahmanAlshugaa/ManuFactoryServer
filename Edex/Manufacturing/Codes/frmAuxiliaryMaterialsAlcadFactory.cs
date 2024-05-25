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
using Edex.DAL;
using DevExpress.XtraGrid.Columns;
using Edex.DAL.ManufacturingDAL;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Edex.DAL.Stc_itemDAL;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using Edex.AccountsObjects.Transactions;
using Edex.DAL.Accounting;
using Edex.DAL.SalseSystem.Stc_itemDAL; 
namespace Edex.Manufacturing.Codes
{
    public partial class frmAuxiliaryMaterialsAlcadFactory : BaseForm
    {

        #region Declare 
        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailAlcadBefore = new BindingList<Manu_AuxiliaryMaterialsDetails>();
        BindingList<Manu_AuxiliaryMaterialsDetails> lstDetailAlcadAfter= new BindingList<Manu_AuxiliaryMaterialsDetails>();
        private string ItemName;

        public int DocumentTypeBefore = 25;
        public int DocumentTypeAfter = 26;
        private string CaptionItemName;
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
        private string SizeName;
        #endregion
        public frmAuxiliaryMaterialsAlcadFactory()
        {
            InitializeComponent();
            ItemName = "ArbItemName";
            SizeName = "ArbSizeName";
            PrimaryName = "ArbName";
            CaptionItemName = "اسم الصنف";
            if(UserInfo.Language==iLanguage.English)
            {
                ItemName = "EngItemName";
                CaptionItemName = "Item Name";
                SizeName = "EngSizeName";
                PrimaryName = "EngName";
            }
            strSQL = "ArbName";
            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", strSQL, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));            
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = UserInfo.BRANCHID;
            txtCostCenterID.Text = MySession.GlobalDefaultCostCenterID;

            /*********************** Date Format dd/MM/yyyy ****************************/
            InitializeFormatDate(txtCommandDate);

            txtCommandDate.ReadOnly = false;

            this.txtCustomerID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCustomerID_Validating);        
            this.txtDelegateID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDelegateID_Validating); 
            this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating); 
            this.txtEmployeeStokID.Validating += new System.ComponentModel.CancelEventHandler(this.txtEmployeeStokID_Validating);
            this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
            this.txtCommandID.Validating += txtCommandID_Validating;

            this.GridAlcadBefore.InitNewRow += GridAlcadBefore_InitNewRow;
            this.gridControlAlcadBefore.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControlAlcadBefore_ProcessGridKey);
            this.GridAlcadBefore.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridAlcadBefore_ValidatingEditor);
            this.GridAlcadBefore.ValidateRow += GridAlcadBefore_ValidateRow;
            this.gridControlAlcadAfter.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControlAlcadAfter_ProcessGridKey);
            this.GridAlcadAfter.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridAlcadAfter_ValidatingEditor);
            this.GridAlcadBefore.RowUpdated += GridAlcadBefore_RowUpdated;
            this.GridAlcadAfter.RowUpdated += GridAlcadAfter_RowUpdated;
            this.txtAccountID.Validating+=txtAccountID_Validating;

            FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", "ArbName", "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            cmbTypeStage.EditValue = 2;
            cmbTypeStage.ReadOnly = true;

            
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
        void GridAlcadBefore_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            SumTotalBalance(GridAlcadBefore, 1);
        }
        void GridAlcadAfter_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            SumTotalBalance(GridAlcadAfter, 2);
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


        private void GridAlcadAfter_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            if (e.Column.FieldName != "Fingerprint")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;

                GridAlcadAfter.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridAlcadAfter.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;

            }

        }
        private void GridAlcadBefore_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName != "Fingerprint")
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;

                GridAlcadBefore.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridAlcadBefore.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;

            }
        }
        void GridAlcadBefore_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {

            try
            {

                foreach (GridColumn col in GridAlcadBefore.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "CostPrice")
                    {

                        var val = GridAlcadBefore.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridAlcadBefore.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                        else if (!(double.TryParse(val.ToString(), out num)) && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridAlcadBefore.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 && col.FieldName != "BarCode")
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            GridAlcadBefore.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }
                        else
                        {
                            e.Valid = true;
                            GridAlcadBefore.SetColumnError(col, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

         
        private void GridAlcadBefore_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
        
             if (this.GridAlcadBefore.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "BarCode" || ColName == "SizeID" || ColName == "CostPrice" || ColName == "ItemID" || ColName == "StoreID" || ColName == "EmpFactorID" || ColName == "QTY")
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
                        view.SetColumnError(GridAlcadBefore.Columns[ColName], "");

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridAlcadBefore.SetColumnError(GridAlcadBefore.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal PriceUnit = Comon.cDec(GridAlcadBefore.GetFocusedRowCellValue("CostPrice"));
                        decimal Qty = Comon.cDec(val.ToString());
                        decimal Total = Comon.cDec(Qty * PriceUnit);                 
                        GridAlcadBefore.SetFocusedRowCellValue("TotalCost", Total.ToString());                  
                    }

                    if (ColName == "CostPrice")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridAlcadBefore.SetColumnError(GridAlcadBefore.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal PriceUnit = Comon.cDec(val.ToString());
                        decimal Qty = Comon.cDec(GridAlcadBefore.GetFocusedRowCellValue("QTY"));
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridAlcadBefore.SetFocusedRowCellValue("TotalCost", Total.ToString());
                    
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
                                view.SetColumnError(GridAlcadBefore.Columns[ColName], "");
                                GridAlcadBefore.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                GridAlcadBefore.FocusedColumn = GridAlcadBefore.VisibleColumns[0];
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
                            view.SetColumnError(GridAlcadBefore.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridAlcadBefore.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridAlcadBefore.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridAlcadBefore.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridAlcadBefore.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }

                    else if (ColName == "EmpFactorID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT   " + PrimaryName + "  FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 ");
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridAlcadBefore.SetFocusedRowCellValue("EmpFactorName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridAlcadBefore.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select SizeID from Stc_SizingUnits Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
            
                            GridAlcadBefore.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                            e.Valid = true;
                            view.SetColumnError(GridAlcadBefore.Columns[ColName], "");                    
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER ("+ PrimaryName+")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridAlcadBefore.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridAlcadBefore.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }

                else if (ColName == "EmpFactorName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpFactorID from HR_EmployeeFile Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridAlcadBefore.SetFocusedRowCellValue("EmpFactorID", dtItemID.Rows[0]["EmpFactorID"]);
                        e.Valid = true;
                        view.SetColumnError(GridAlcadBefore.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgNoFoundThisItem);
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
                        e.ErrorText = " الصنف غير موجود";
                    }
                }
                
            }
             SumTotalBalance(GridAlcadBefore, 1);

        }
        private void GridAlcadAfter_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (this.GridAlcadAfter.ActiveEditor is TextEdit)
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
                        view.SetColumnError(GridAlcadAfter.Columns[ColName], "");

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridAlcadAfter.SetColumnError(GridAlcadAfter.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal PriceUnit = Comon.cDec(GridAlcadAfter.GetFocusedRowCellValue("CostPrice"));
                        decimal Qty = Comon.cDec(val.ToString());
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridAlcadAfter.SetFocusedRowCellValue("TotalCost", Total.ToString());
                    }
                    if (ColName == "CostPrice")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        GridAlcadAfter.SetColumnError(GridAlcadAfter.Columns["QTY"], "");
                        e.ErrorText = "";

                        decimal PriceUnit = Comon.cDec(val.ToString());
                        decimal Qty = Comon.cDec(GridAlcadAfter.GetFocusedRowCellValue("QTY"));
                        decimal Total = Comon.cDec(Qty * PriceUnit);
                        GridAlcadAfter.SetFocusedRowCellValue("TotalCost", Total.ToString());

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
                                view.SetColumnError(GridAlcadAfter.Columns[ColName], "");
                                GridAlcadAfter.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                GridAlcadAfter.FocusedColumn = GridAlcadAfter.VisibleColumns[0];
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
                            view.SetColumnError(GridAlcadAfter.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0 And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            GridAlcadAfter.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridAlcadAfter.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "StoreID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select  " + PrimaryName + " from  Stc_Stores Where Cancel=0 And StoreID=" + val.ToString() + " And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridAlcadAfter.SetFocusedRowCellValue("StoreName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridAlcadAfter.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                    else if (ColName == "EmpFactorID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("SELECT  " + PrimaryName + "  FROM HR_EmployeeFile  WHERE EmployeeID =" + Comon.cDbl(val.ToString()) + " And Cancel =0 ");
                        if (dtItemID.Rows.Count > 0)
                        {
                            GridAlcadAfter.SetFocusedRowCellValue("EmpFactorName", dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(GridAlcadAfter.Columns[ColName], "");
                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select SizeID from Stc_SizingUnits Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {

                        GridAlcadAfter.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                        e.Valid = true;
                        view.SetColumnError(GridAlcadAfter.Columns[ColName], "");

                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                else if (ColName == "StoreName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select StoreID from Stc_Stores Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridAlcadAfter.SetFocusedRowCellValue("StoreID", dtItemID.Rows[0]["StoreID"]);
                        e.Valid = true;
                        view.SetColumnError(GridAlcadAfter.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }
                else if (ColName == "EmpFactorName")
                {
                    DataTable dtItemID = Lip.SelectRecord("Select EmployeeID as EmpFactorID from HR_EmployeeFile Where Cancel=0 And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
                    if (dtItemID.Rows.Count > 0)
                    {
                        GridAlcadAfter.SetFocusedRowCellValue("EmpFactorID", dtItemID.Rows[0]["EmpFactorID"]);
                        e.Valid = true;
                        view.SetColumnError(GridAlcadAfter.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgNoFoundThisItem);
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
            SumTotalBalance(GridAlcadAfter, 2);
        }
        private void gridControlAlcadAfter_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.GridAlcadBefore.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                      //  CalculateRow(GridAlcadBefore.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));
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
                        if (ColName == "BarCode" || ColName == "CostPrice" || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID" )
                        {
                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgInputIsRequired);
                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(GridAlcadBefore.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridAlcadBefore.Columns[ColName], "");
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

        private void gridControlAlcadBefore_ProcessGridKey(object sender, KeyEventArgs e)
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
                    if (this.GridAlcadAfter.ActiveEditor is CheckEdit)
                    {
                        view.SetFocusedValue(!Comon.cbool(view.GetFocusedValue()));
                        //  CalculateRow(GridAlcadAfter.FocusedRowHandle, Comon.cbool(view.GetFocusedValue()));


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
                                view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(GridAlcadAfter.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(GridAlcadAfter.Columns[ColName], "");
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
        void GridAlcadBefore_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
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

                    if (row.Length> 0)
                    {
                        lblStoreNameName.Text = row[0]["ArbName"].ToString();
                        strSQL = "Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        string StoreManger = Lip.GetValue(strSQL).ToString();
                        lblBeforeStoreManger.Text = StoreManger;
                    }
                }
                else
                {
                    lblStoreNameName.Text = "";
                    txtStoreID.Text = "";

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
        private void txtEmployeeStokID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(txtEmployeeStokID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEmployeeStokID, lblEmployeeStokName, strSQL); 
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void frmProcessingStage_Load(object sender, EventArgs e)
        {
            try
            {           
                initGridAlcadBefore();
                initGridAlcadAfter();        
                DoNew();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void frmAuxiliaryMaterials_KeyDown(object sender, KeyEventArgs e)
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
        void initGridAlcadBefore()
        {

            lstDetailAlcadBefore = new BindingList<Manu_AuxiliaryMaterialsDetails>();
            lstDetailAlcadBefore.AllowNew = true;
            lstDetailAlcadBefore.AllowEdit = true;
            lstDetailAlcadBefore.AllowRemove = true;

            gridControlAlcadBefore.DataSource = lstDetailAlcadBefore;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits ");
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                    NameUnit[i] = dtitems.Rows[i][ PrimaryName  ].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControlAlcadBefore.RepositoryItems.Add(riComboBoxitems);
            GridAlcadBefore.Columns[SizeName].ColumnEdit = riComboBoxitems;



            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                    StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();
            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAlcadBefore.RepositoryItems.Add(riComboBoxitems2);
            GridAlcadBefore.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAlcadBefore.RepositoryItems.Add(riComboBoxitems3);
            GridAlcadBefore.Columns["EmpFactorName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAlcadBefore.RepositoryItems.Add(riComboBoxitems4);
            GridAlcadBefore.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            GridAlcadBefore.Columns["CommandID"].Visible = false;

            GridAlcadBefore.Columns["BranchID"].Visible = false;
            GridAlcadBefore.Columns["FacilityID"].Visible = false;       
            GridAlcadBefore.Columns["ArbItemName"].Visible = GridAlcadBefore.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridAlcadBefore.Columns["EngItemName"].Visible = GridAlcadBefore.Columns["EngItemName"].Name == "col" + ItemName ? true : false;   
            GridAlcadBefore.Columns["TypeOpration"].Visible = false;
            GridAlcadBefore.Columns["TotalCost"].OptionsColumn.ReadOnly = false;
          
            GridAlcadBefore.Columns[ItemName].Visible = true;
            GridAlcadBefore.Columns[ItemName].Caption = CaptionItemName;
            GridAlcadBefore.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            GridAlcadBefore.Columns["TotalCost"].OptionsColumn.AllowFocus = false;

            //GridAlcadBefore.Columns["DateROrD"].OptionsColumn.ReadOnly = true;
            //GridAlcadBefore.Columns["DateROrD"].OptionsColumn.AllowFocus = false;
            //GridAlcadBefore.Columns["TimeROrD"].OptionsColumn.ReadOnly = true;
            //GridAlcadBefore.Columns["TimeROrD"].OptionsColumn.AllowFocus = false;
            GridAlcadBefore.Columns["EmpFactorName"].Width = 150;
            GridAlcadBefore.Columns[ItemName].Width = 150;
            GridAlcadBefore.Columns[SizeName].Width = 120;
            GridAlcadBefore.Columns["StoreName"].Width = 120;
            GridAlcadBefore.Columns["EmpFactorID"].Width = 130;
            GridAlcadBefore.Columns["EmpFactorID"].Visible = false;
            GridAlcadBefore.Columns["EmpFactorName"].Visible = false;
            GridAlcadBefore.Columns["StoreID"].Visible = false;
            GridAlcadBefore.Columns["StoreName"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridAlcadBefore.Columns["EngItemName"].Visible = false;
                GridAlcadBefore.Columns["EngSizeName"].Visible = false;
                GridAlcadBefore.Columns["BarCode"].Caption = "باركود الصنف";
                GridAlcadBefore.Columns["SizeID"].Caption = "رقم الوحدة";
                GridAlcadBefore.Columns["ItemID"].Caption = "رقم الصنــف";
                
                GridAlcadBefore.Columns[SizeName].Caption = "إسم الوحدة";
                GridAlcadBefore.Columns["StoreID"].Caption = "رقم المخزن ";
                GridAlcadBefore.Columns["StoreName"].Caption = "إسم المخزن";
                GridAlcadBefore.Columns["EmpFactorID"].Caption = " رقم العامل";
                GridAlcadBefore.Columns["EmpFactorName"].Caption = "إسم العــامل ";
                GridAlcadBefore.Columns["QTY"].Caption = "الكمية ";
                GridAlcadBefore.Columns["CostPrice"].Caption = "القيمة";
                GridAlcadBefore.Columns["TotalCost"].Caption = "الإجمالي ";
                GridAlcadBefore.Columns["Fingerprint"].Caption = "البصمــة";
                GridAlcadBefore.Columns["DateROrD"].Caption = "التاريــخ";
                GridAlcadBefore.Columns["TimeROrD"].Caption = "الوقــت ";
            }
            else
            {
                GridAlcadBefore.Columns["ArbItemName"].Visible = false;
                GridAlcadBefore.Columns["ArbSizeName"].Visible = false;
                GridAlcadBefore.Columns["BarCode"].Caption = "BarCode";                
                GridAlcadBefore.Columns["StoreID"].Caption = "Store ID";
                GridAlcadBefore.Columns["StoreName"].Caption = "Store Name";
                GridAlcadBefore.Columns["SizeID"].Caption = "Unit ID";
                GridAlcadBefore.Columns["ItemID"].Caption = "Item ID";
                GridAlcadBefore.Columns[SizeName].Caption = "Unit Name ";
                GridAlcadBefore.Columns["CostPrice"].Caption = "Cost Price";      
                GridAlcadBefore.Columns["QTY"].Caption = "QTY";
                GridAlcadBefore.Columns["TotalCost"].Caption = "Total Cost ";
                GridAlcadBefore.Columns["DateRorD"].Caption = "Date";
                GridAlcadBefore.Columns["Fingerprint"].Caption = "Fingerprint";
                GridAlcadBefore.Columns["TimeROrD"].Caption = "Time";
                GridAlcadBefore.Columns["EmpFactorID"].Caption = "Emp Factor ID";
                GridAlcadBefore.Columns["EmpFactorName"].Caption = "Emp Factor Name";
            }

        }
        void initGridAlcadAfter()
        {
            lstDetailAlcadAfter = new BindingList<Manu_AuxiliaryMaterialsDetails>();
            lstDetailAlcadAfter.AllowNew = true;
            lstDetailAlcadAfter.AllowEdit = true;
            lstDetailAlcadAfter.AllowRemove = true;

            gridControlAlcadAfter.DataSource = lstDetailAlcadAfter;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "  FROM Stc_SizingUnits ");
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                    NameUnit[i] = dtitems.Rows[i][ PrimaryName ].ToString();


            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControlAlcadAfter.RepositoryItems.Add(riComboBoxitems);
            GridAlcadAfter.Columns["ArbItemName"].Visible = GridAlcadAfter.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridAlcadAfter.Columns["EngItemName"].Visible = GridAlcadAfter.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            GridAlcadAfter.Columns[ItemName].Visible = true;
            GridAlcadAfter.Columns[ItemName].Caption = CaptionItemName;
            GridAlcadAfter.Columns[SizeName].ColumnEdit = riComboBoxitems;



            DataTable dtStore = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM Stc_Stores WHERE  Cancel =0 ");
            string[] StoreName = new string[dtStore.Rows.Count];
            for (int i = 0; i <= dtStore.Rows.Count - 1; i++)
                    StoreName[i] = dtStore.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems2 = new RepositoryItemComboBox();

            riComboBoxitems2.Items.AddRange(StoreName);
            gridControlAlcadAfter.RepositoryItems.Add(riComboBoxitems2);
            GridAlcadAfter.Columns["StoreName"].ColumnEdit = riComboBoxitems2;


            DataTable dtEmp = Lip.SelectRecord("SELECT " + PrimaryName + "  FROM HR_EmployeeFile WHERE  Cancel =0 ");
            string[] EmpName = new string[dtEmp.Rows.Count];
            for (int i = 0; i <= dtEmp.Rows.Count - 1; i++)
                EmpName[i] = dtEmp.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems3 = new RepositoryItemComboBox();
            riComboBoxitems3.Items.AddRange(EmpName);
            gridControlAlcadAfter.RepositoryItems.Add(riComboBoxitems3);
            GridAlcadAfter.Columns["EmpFactorName"].ColumnEdit = riComboBoxitems3;

            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0");
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControlAlcadAfter.RepositoryItems.Add(riComboBoxitems4);
            GridAlcadAfter.Columns[ItemName].ColumnEdit = riComboBoxitems4;


            GridAlcadAfter.Columns["CommandID"].Visible = false;
            GridAlcadAfter.Columns["BranchID"].Visible = false;
            GridAlcadAfter.Columns["FacilityID"].Visible = false;               
            GridAlcadAfter.Columns["TypeOpration"].Visible = false;
            GridAlcadAfter.Columns["TotalCost"].OptionsColumn.ReadOnly = false;
            GridAlcadAfter.Columns["TotalCost"].OptionsColumn.AllowFocus = false;
          
            //GridAlcadAfter.Columns["DateROrD"].OptionsColumn.ReadOnly = true;
            //GridAlcadAfter.Columns["DateROrD"].OptionsColumn.AllowFocus = false;
            //GridAlcadAfter.Columns["TimeROrD"].OptionsColumn.ReadOnly = true;
            //GridAlcadAfter.Columns["TimeROrD"].OptionsColumn.AllowFocus = false;
            GridAlcadAfter.Columns["EmpFactorName"].Width = 150;
            GridAlcadAfter.Columns[ItemName].Width = 150;
            GridAlcadAfter.Columns[SizeName].Width = 120;
            GridAlcadAfter.Columns["StoreName"].Width = 120;
            GridAlcadAfter.Columns["EmpFactorID"].Width = 130;
            GridAlcadAfter.Columns["EmpFactorID"].Visible = false;
            GridAlcadAfter.Columns["EmpFactorName"].Visible = false;
            GridAlcadAfter.Columns["StoreID"].Visible = false;
            GridAlcadAfter.Columns["StoreName"].Visible = false;
            if (UserInfo.Language == iLanguage.Arabic)
            {
                GridAlcadAfter.Columns["EngItemName"].Visible = false;
                GridAlcadAfter.Columns["EngSizeName"].Visible = false;
                GridAlcadAfter.Columns["BarCode"].Caption = "باركود الصنف";
                GridAlcadAfter.Columns["SizeID"].Caption = "رقم الوحدة";
                GridAlcadAfter.Columns["ItemID"].Caption = "رقم الصنــف";
                GridAlcadAfter.Columns["StoreID"].Caption = "رقم المخزن";
                GridAlcadAfter.Columns[SizeName].Caption = "الوحدة ";
                GridAlcadAfter.Columns["StoreID"].Caption = "رقم المخزن ";
                GridAlcadAfter.Columns["StoreName"].Caption = "إسم المخزن";
                GridAlcadAfter.Columns["EmpFactorID"].Caption = " رقم العامل";
                GridAlcadAfter.Columns["EmpFactorName"].Caption = "إسم العــامل ";
                GridAlcadAfter.Columns["QTY"].Caption = "الكمية ";
                GridAlcadAfter.Columns["TotalCost"].Caption = "الإجمالي ";
                GridAlcadAfter.Columns["CostPrice"].Caption = "القيمة";
                GridAlcadAfter.Columns["Fingerprint"].Caption = "البصمــة";
                GridAlcadAfter.Columns["DateROrD"].Caption = "التاريــخ";
                GridAlcadAfter.Columns["TimeROrD"].Caption = "الوقـــت";
            }
            else
            {
                GridAlcadAfter.Columns["ArbItemName"].Visible = false;
                GridAlcadAfter.Columns["ArbSizeName"].Visible = false;
                GridAlcadAfter.Columns["BarCode"].Caption = "BarCode";
                GridAlcadAfter.Columns["StoreID"].Caption = "  Store ID ";
                GridAlcadAfter.Columns["StoreName"].Caption = "Store Name";
                GridAlcadAfter.Columns["SizeID"].Caption = "Unit ID";
                GridAlcadAfter.Columns["ItemID"].Caption = "Item ID";
                 
                GridAlcadAfter.Columns[SizeName].Caption = "Unit Name ";
                GridAlcadAfter.Columns["CostPrice"].Caption = "Cost Price";
                GridAlcadAfter.Columns["QTY"].Caption = "QTY";
                GridAlcadAfter.Columns["TotalCost"].Caption = "Total Cost ";
                GridAlcadAfter.Columns["DateRorD"].Caption = "Date";
                GridAlcadAfter.Columns["Fingerprint"].Caption = "Fingerprint";
                GridAlcadAfter.Columns["TimeROrD"].Caption = "Time";
                GridAlcadAfter.Columns["EmpFactorID"].Caption = "Emp Factor ID";
                GridAlcadAfter.Columns["EmpFactorName"].Caption = "Emp Factor Name";
            }

        }
       #endregion 


        #region Function 
        
        private void FileItemData(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["Qty"], 0);
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["DateRorD"], DateTime.Now.ToString("yyyy/MM/dd"));
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["TimeROrD"], DateTime.Now.ToString("hh:mm:tt"));                        
            }
            else
            {
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["Qty"], "0");

                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["SizeID"], "");
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns[SizeName], "");
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["BarCode"], "");
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["ItemID"], "");
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns[ItemName],"");
              
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["DateRorD"], DateTime.Now.ToString("yyyy/MM/dd"));
                GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["TimeROrD"], DateTime.Now.ToString("hh:mm:tt"));
            }
        }

        private void FileItemData2(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["Qty"], dt.Rows[0]["QTY"].ToString());
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["DateRorD"], DateTime.Now.ToString("yyyy/MM/dd"));
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["TimeROrD"], DateTime.Now.ToString("hh:mm:tt"));

            }
            else
            {
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["Qty"], "0");
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["SizeID"],"");
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns[SizeName], "");
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["BarCode"], "");
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["ItemID"], "");
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns[ItemName], "");          
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["DateRorD"], DateTime.Now.ToString("yyyy/MM/dd"));
                GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["TimeROrD"], DateTime.Now.ToString("hh:mm:tt"));
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
                txtReferanceID.Text = "";
                 
                txtCustomerID.Text = "";
                txtCustomerID_Validating(null, null);
       
                txtEmployeeStokID.Text = "";
                txtEmployeeStokID_Validating(null, null);

                txtFactorID.Text = "";
                txtFactorID_Validating(null,null);

               // GetAccountsDeclaration();
                
                txtDelegateID.Text = MySession.GlobalDefaultSaleDelegateID;
                txtDelegateID_Validating(null, null);
                
              

                txtStoreID.Text = "";
                txtStoreID_Validating(null, null);
                lstDetailAlcadBefore = new BindingList<Manu_AuxiliaryMaterialsDetails>();
                lstDetailAlcadBefore.AllowNew = true;
                lstDetailAlcadBefore.AllowEdit = true;
                lstDetailAlcadBefore.AllowRemove = true;
                gridControlAlcadBefore.DataSource = lstDetailAlcadBefore;

                lstDetailAlcadAfter = new BindingList<Manu_AuxiliaryMaterialsDetails>();
                lstDetailAlcadAfter.AllowNew = true;
                lstDetailAlcadAfter.AllowEdit = true;
                lstDetailAlcadAfter.AllowRemove = true;
                gridControlAlcadAfter.DataSource = lstDetailAlcadAfter;
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
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "AlcadCommend", "رقـم الأمر", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "AlcadCommend", "Commend ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtEmployeeStokID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, lblEmployeeStokName, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtEmployeeStokID, lblEmployeeStokName, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmSaleCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }
            if (FocusedControl.Trim() == txtCustomerID.Name)
            {
                 
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "رقم الــعـــمـــيــل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCustomerID, lblCustomerName, "CustomerIDAndSublierID", "SublierID ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() ==txtStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls,txtStoreID, lblStoreNameName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreNameName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtAccountID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "رقم الحساب", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtAccountID, lblAccountName, "AccountID", "Account ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtDelegateID.Name)
            {                 
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "رقم مندوب المبيعات", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDelegateID, lblDelegateName, "SaleDelegateID", "Delegate ID", MySession.GlobalBranchID);
            }
           
            else if (FocusedControl.Trim() == txtFactorID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtFactorID, lblFactorName, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
            }
             

            else if (FocusedControl.Trim() == gridControlAlcadBefore.Name)
            {
                if (GridAlcadBefore.FocusedColumn == null) return;
                if (GridAlcadBefore.FocusedColumn.Name == "colBarCode" || GridAlcadBefore.FocusedColumn.Name == "colItemName" || GridAlcadBefore.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                }

                else if (GridAlcadBefore.FocusedColumn.Name =="colStoreID" )
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                else if (GridAlcadBefore.FocusedColumn.Name == "colSizeID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridAlcadBefore.FocusedColumn.Name == "colEmpFactorID")
                {
                    if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; }

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "رقـم العامل", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "EmployeeID", "Worker ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == gridControlAlcadAfter.Name)
            {
                if (GridAlcadAfter.FocusedColumn == null) return;
                if (GridAlcadAfter.FocusedColumn.Name == "colBarCode" || GridAlcadAfter.FocusedColumn.Name == "colItemName" || GridAlcadAfter.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                }
                else if (GridAlcadAfter.FocusedColumn.Name == "colStoreID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "StoreID", "Store ID", MySession.GlobalBranchID);
                }
                else if (GridAlcadAfter.FocusedColumn.Name == "colSizeID")
                {

                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
                else if (GridAlcadAfter.FocusedColumn.Name == "colEmpFactorID")
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
        public void  GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtCommandID.Name)
                {
                    txtCommandID.Text = cls.PrimaryKeyValue.ToString();
                    txtCommandID_Validating(null, null);
                }   
                else if (FocusedControl == txtEmployeeStokID.Name)
                {
                    txtEmployeeStokID.Text = cls.PrimaryKeyValue.ToString();
                    txtEmployeeStokID_Validating(null, null);
                }
                if (FocusedControl ==txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }
                if (FocusedControl == txtAccountID.Name)
                {
                    txtAccountID.Text = cls.PrimaryKeyValue.ToString();
                    txtAccountID_Validating(null, null);
                }
                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
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
                 
                else if (FocusedControl == txtFactorID.Name)
                {
                    txtFactorID.Text = cls.PrimaryKeyValue.ToString();
                    txtFactorID_Validating(null, null);
                }
                else if (FocusedControl ==gridControlAlcadBefore.Name)
                {
                    if (GridAlcadBefore.FocusedColumn.Name == "colBarCode" || GridAlcadBefore.FocusedColumn.Name == "colItemName" || GridAlcadBefore.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridAlcadBefore.AddNewRow();

                        GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));

                       // CalculateRow();
                    }

                    if (GridAlcadBefore.FocusedColumn.Name == "colStoreID")
                    {
                        GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["StoreName"], Lip.GetValue(strSQL));
                      
                    }
                    if (GridAlcadBefore.FocusedColumn.Name == "colSizeID")
                    {
                        GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " +SizeName+ " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns[SizeName], Lip.GetValue(strSQL));

                    }
                    if (GridAlcadBefore.FocusedColumn.Name == "colEmpFactorID")
                    {
                        GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["EmpFactorID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridAlcadBefore.SetRowCellValue(GridAlcadBefore.FocusedRowHandle, GridAlcadBefore.Columns["EmpFactorName"], Lip.GetValue(strSQL));
                    }

                }

                else if (FocusedControl == gridControlAlcadAfter.Name)
                {
                    if (GridAlcadAfter.FocusedColumn.Name == "colBarCode" || GridAlcadAfter.FocusedColumn.Name == "colItemName" || GridAlcadAfter.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        GridAlcadAfter.AddNewRow();

                        GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["BarCode"], Barcode);
                        FileItemData2(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));
                        // CalculateRow();
                    }
                    if (GridAlcadAfter.FocusedColumn.Name == "colStoreID")
                    {
                        GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["StoreID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["StoreName"], Lip.GetValue(strSQL));

                    }
                    if (GridAlcadAfter.FocusedColumn.Name == "colSizeID")
                    {
                        GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as "+SizeName+" FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns[SizeName], Lip.GetValue(strSQL));
                    }
                    if (GridAlcadAfter.FocusedColumn.Name == "colEmpFactorID")
                    {
                        GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["EmpFactorID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID =" + Comon.cDbl(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 ";
                        GridAlcadAfter.SetRowCellValue(GridAlcadAfter.FocusedRowHandle, GridAlcadAfter.Columns["EmpFactorName"], Lip.GetValue(strSQL));
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
          

            
            foreach (GridColumn col in GridAlcadBefore.Columns)
            {
                if (col.FieldName == "BarCode")
                {

                    GridAlcadBefore.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridAlcadBefore.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridAlcadBefore.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }

            }
            foreach (GridColumn col in GridAlcadAfter.Columns)
            {
                if (col.FieldName == "BarCode")
                {
                    GridAlcadAfter.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    GridAlcadAfter.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    GridAlcadAfter.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
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
                //في جدول المراحل معمل  الكاد 2 وهنا 1 ما الفرق 
                txtCommandID.Text = AuxiliaryMaterialsDAl.GetNewID(UserInfo.FacilityID,Comon.cInt( cmbBranchesID.EditValue), 1).ToString();
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
        //لحذف الحركة المخزنية 
        int DeleteStockMoving(int DocumentID,int DocumentType)
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
        //هذه الدالة لحذف امر الصرف او التوريد من الارشيف الخاص باوامر الصرف والتوريد الخاصة بالتصنيع
        int DeleteInOnOROutOnBil(int DocumentID,int DocumentType)
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
                model.TypeCommand = 1;
                string Result =AuxiliaryMaterialsDAl.Delete(model);

                //حذف الحركة المخزنية 
                if (Comon.cInt(Result) > 0)
                {
                    int MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text),DocumentTypeBefore);
                      MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
                    if (MoveID < 0)
                        Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                }
                if (Comon.cInt(Result) > 0)
                {
                    int VoucherID=0;
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
                if (Comon.cInt(Result) >0)
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
            for (int i = 0; i <= GridAlcadBefore.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["CommandID"] = Comon.cInt(txtCommandID.Text);
                dtItem.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem.Rows[i]["BarCode"] = GridAlcadBefore.GetRowCellValue(i, "BarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(GridAlcadBefore.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i][ItemName] = GridAlcadBefore.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtItem.Rows[i][SizeName] = GridAlcadBefore.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["SizeID"] =Comon.cInt( GridAlcadBefore.GetRowCellValue(i, "SizeID").ToString());
                 dtItem.Rows[i]["TypeOpration"] =Comon.cInt( GridAlcadBefore.GetRowCellValue(i, "TypeOpration").ToString());
                dtItem.Rows[i]["Fingerprint"] =Comon.cInt( GridAlcadBefore.GetRowCellValue(i, "Fingerprint").ToString());
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(GridAlcadBefore.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["TotalCost"] = Comon.ConvertToDecimalPrice(GridAlcadBefore.GetRowCellValue(i, "TotalCost").ToString()); 
             
                dtItem.Rows[i]["DateROrD"] = GridAlcadBefore.GetRowCellValue(i, "DateROrD").ToString();
                dtItem.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(GridAlcadBefore.GetRowCellValue(i, "CostPrice").ToString());
                dtItem.Rows[i]["StoreID"] = Comon.cInt(GridAlcadBefore.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["StoreName"] = GridAlcadBefore.GetRowCellValue(i, "StoreName").ToString();
                dtItem.Rows[i]["EmpFactorID"] = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "EmpFactorID").ToString());
                dtItem.Rows[i]["EmpFactorName"] = GridAlcadBefore.GetRowCellValue(i, "EmpFactorName").ToString();
                dtItem.Rows[i]["TimeROrD"] =  GridAlcadBefore.GetRowCellValue(i, "TimeROrD").ToString() ;            
            }
            gridControlAlcadBefore.DataSource = dtItem;
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
            dtItem1.Columns.Add("EmpFactorID", System.Type.GetType("System.Int64"));
            dtItem1.Columns.Add("EmpFactorName", System.Type.GetType("System.String"));
            dtItem1.Columns.Add("TimeROrD", System.Type.GetType("System.String"));
            for (int i = 0; i <= GridAlcadAfter.DataRowCount - 1; i++)
            {
                dtItem1.Rows.Add();
                dtItem1.Rows[i]["CommandID"] = Comon.cInt(txtCommandID.Text);
                dtItem1.Rows[i]["FacilityID"] = UserInfo.FacilityID; ;
                dtItem1.Rows[i]["BarCode"] = GridAlcadAfter.GetRowCellValue(i, "BarCode").ToString();
                dtItem1.Rows[i]["ItemID"] = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "ItemID").ToString());
                dtItem1.Rows[i][ItemName] = GridAlcadAfter.GetRowCellValue(i, ItemName).ToString();
                dtItem1.Rows[i]["BranchID"] = Comon.cInt(cmbBranchesID.EditValue);
                dtItem1.Rows[i][SizeName] = GridAlcadAfter.GetRowCellValue(i, SizeName).ToString();
                dtItem1.Rows[i]["SizeID"] = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "SizeID").ToString());
                dtItem1.Rows[i]["TypeOpration"] = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "TypeOpration").ToString());
                dtItem1.Rows[i]["Fingerprint"] = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "Fingerprint").ToString());
                dtItem1.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(GridAlcadAfter.GetRowCellValue(i, "QTY").ToString());
                dtItem1.Rows[i]["TotalCost"] = Comon.ConvertToDecimalPrice(GridAlcadAfter.GetRowCellValue(i, "TotalCost").ToString());
                dtItem1.Rows[i]["DateROrD"] = GridAlcadAfter.GetRowCellValue(i, "DateROrD").ToString();
                dtItem1.Rows[i]["CostPrice"] = Comon.ConvertToDecimalPrice(GridAlcadAfter.GetRowCellValue(i, "CostPrice").ToString());
                dtItem1.Rows[i]["StoreID"] = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "StoreID").ToString());
                dtItem1.Rows[i]["StoreName"] = GridAlcadAfter.GetRowCellValue(i, "StoreName").ToString();
                dtItem1.Rows[i]["EmpFactorID"] = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "EmpFactorID").ToString());
                dtItem1.Rows[i]["EmpFactorName"] = GridAlcadBefore.GetRowCellValue(i, "EmpFactorName").ToString();
                dtItem1.Rows[i]["TimeROrD"] =  GridAlcadAfter.GetRowCellValue(i, "TimeROrD").ToString() ;
            }
            gridControlAlcadAfter.DataSource = dtItem1;
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
        private void SaveOutOn()
        {
            #region Save Out On
            bool isNew = IsNewRecord;
            //Save Out On
            Stc_ManuFactoryCommendOutOnBail_Master objRecordOutOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
         
            if (IsNewRecord)
                objRecordOutOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 1));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeBefore);
                if (dtInvoiceID.Rows.Count>0)
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
            objRecordOutOnMaster.CostCenterID=Comon.cInt( txtCostCenterID.Text);
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
            for (int i = 0; i <= GridAlcadBefore.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordOutOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridAlcadBefore.GetRowCellValue(i, "DateROrD").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridAlcadBefore.GetRowCellValue(i, "TimeROrD")).ToShortTimeString());
                returnedOutOn.BarCode = GridAlcadBefore.GetRowCellValue(i, "BarCode").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridAlcadBefore.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridAlcadBefore.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "QTY").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "CostPrice").ToString());
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
            bool isNew = IsNewRecord;
            //Save Out On
            Stc_ManuFactoryCommendOutOnBail_Master objRecordInOnMaster = new Stc_ManuFactoryCommendOutOnBail_Master();
            if (IsNewRecord)
                objRecordInOnMaster.InvoiceID = Comon.cInt(Stc_ManuFactoryCommendOutOnBailDAL.GetNewID(UserInfo.FacilityID, UserInfo.BRANCHID, UserInfo.ID, 2));
            else
            {
                DataTable dtInvoiceID = Stc_ManuFactoryCommendOutOnBailDAL.GetInvoiceID(UserInfo.FacilityID, UserInfo.BRANCHID, Comon.cInt(txtCommandID.Text), DocumentTypeAfter);
                if (dtInvoiceID.Rows.Count>0)
                  objRecordInOnMaster.InvoiceID =Comon.cInt(dtInvoiceID.Rows[0][0]);
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
            for (int i = 0; i <= GridAlcadAfter.DataRowCount - 1; i++)
            {
                returnedOutOn = new Stc_ManuFactoryCommendOutOnBail_Details();
                returnedOutOn.InvoiceID = objRecordInOnMaster.InvoiceID;
                returnedOutOn.CommandID = Comon.cInt(txtCommandID.Text);
                returnedOutOn.FacilityID = UserInfo.FacilityID;
                returnedOutOn.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                returnedOutOn.CommandDate = Comon.cDate(GridAlcadAfter.GetRowCellValue(i, "DateROrD").ToString());
                returnedOutOn.CommandTime = (Comon.cDateTime(GridAlcadAfter.GetRowCellValue(i, "TimeROrD")).ToShortTimeString());
                returnedOutOn.BarCode = GridAlcadAfter.GetRowCellValue(i, "BarCode").ToString();
                returnedOutOn.ItemID = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "ItemID").ToString());
                returnedOutOn.SizeID = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "SizeID").ToString());
                returnedOutOn.QTY = Comon.cDbl(GridAlcadAfter.GetRowCellValue(i, "QTY").ToString());
                returnedOutOn.CostPrice = Comon.cDbl(GridAlcadAfter.GetRowCellValue(i, "CostPrice").ToString());
                listreturnedOutOn.Add(returnedOutOn);
            }
            if (listreturnedOutOn.Count > 0)
            {
                objRecordInOnMaster.CommandOutOnBailDatails = listreturnedOutOn;
                int Result = Stc_ManuFactoryCommendOutOnBailDAL.InsertUsingXML(objRecordInOnMaster, isNew);
                if (Result > 0)
                {
                    //حفظ القيد الالي
                    long VoucherID = SaveVariousVoucherMachinInOn(Comon.cInt(objRecordInOnMaster.InvoiceID), isNew);
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
            for (int i = 0; i <= GridAlcadBefore.DataRowCount - 1; i++)
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
                returned.BarCode = GridAlcadBefore.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridAlcadBefore.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(GridAlcadBefore.GetRowCellValue(i, "SizeID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.QTY = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = 0;
                //returned.Bones = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "TotalCost").ToString());
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
            for (int i = 0; i <= GridAlcadAfter.DataRowCount - 1; i++)
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
                returned.BarCode = GridAlcadAfter.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "ItemID").ToString());
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID));
                returned.SizeID = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "SizeID").ToString());
                returned.QTY = Comon.cDbl(GridAlcadAfter.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = Comon.cDbl(GridAlcadAfter.GetRowCellValue(i, "TotalCost").ToString());
                //returned.Bones = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "Bones").ToString());
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
            GridAlcadBefore.Focus();
            GridAlcadBefore.MoveLastVisible();           
            GridAlcadBefore.FocusedColumn = GridAlcadBefore.VisibleColumns[1];
            Manu_AuxiliaryMaterialsMaster objRecord = new Manu_AuxiliaryMaterialsMaster();
            objRecord.CommandID = Comon.cInt(txtCommandID.Text);
            objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            objRecord.FacilityID = UserInfo.FacilityID;  
            objRecord.CommandDate = Comon.ConvertDateToSerial(txtCommandDate.Text);    
            objRecord.CustomerID = Comon.cDbl(txtCustomerID.Text);
            objRecord.StoreID = Comon.cDbl(txtStoreID.Text);
            objRecord.FactorID = Comon.cDbl(txtFactorID.Text);
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.EmployeeStokID = Comon.cDbl(txtEmployeeStokID.Text);
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue);
            objRecord.AccountID = Comon.cDbl(txtAccountID.Text);
            objRecord.DelegetID = Comon.cInt(txtDelegateID.Text);
            objRecord.ReferanceID = Comon.cInt(txtReferanceID.Text);
            txtNotes.Text = (txtNotes.Text.Trim());
            objRecord.Notes = txtNotes.Text;
            objRecord.TypeCommand = 1;
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
            for (int i = 0; i <= GridAlcadBefore.DataRowCount - 1; i++)
            {
                returned = new Manu_AuxiliaryMaterialsDetails();
                returned.CommandID = Comon.cInt(txtCommandID.Text);
                returned.FacilityID = UserInfo.FacilityID;
                returned.BranchID =Comon.cInt( cmbBranchesID.EditValue);
                returned.DateROrD =  Comon.cDate(GridAlcadBefore.GetRowCellValue(i, "DateROrD").ToString());
                returned.TimeROrD =  (Comon.cDateTime(GridAlcadBefore.GetRowCellValue(i, "TimeROrD")).ToShortTimeString()); 
                returned.BarCode = GridAlcadBefore.GetRowCellValue(i, "BarCode").ToString();
                returned.ItemID = Comon.cInt(GridAlcadBefore.GetRowCellValue(i, "ItemID").ToString());
                returned.StoreID = Comon.cLong(txtStoreID.Text.ToString());
                returned.EmpFactorID = Comon.cDbl(txtFactorID.Text.ToString());
                returned.SizeID = Comon.cInt(GridAlcadBefore.GetRowCellValue(i, "SizeID").ToString());
                returned.ArbSizeName =  GridAlcadBefore.GetRowCellValue(i, SizeName).ToString();
                returned.EngSizeName = GridAlcadBefore.GetRowCellValue(i, SizeName).ToString();
                returned.ArbItemName = GridAlcadBefore.GetRowCellValue(i, ItemName).ToString();
                returned.EngItemName = GridAlcadBefore.GetRowCellValue(i, ItemName).ToString();
                returned.StoreName = lblStoreNameName.Text.ToString();
                returned.QTY = Comon.ConvertToDecimalQty(GridAlcadBefore.GetRowCellValue(i, "QTY").ToString());             
                returned.CostPrice = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "CostPrice").ToString());
                returned.TotalCost = Comon.cDbl(GridAlcadBefore.GetRowCellValue(i, "TotalCost").ToString());
                returned.EmpFactorName = lblFactorName.Text.ToString();
                returned.TypeOpration = 1;
                listreturned.Add(returned);
            }
            
            if (GridAlcadAfter.DataRowCount>0)
            {
                for (int i = 0; i <= GridAlcadAfter.DataRowCount - 1; i++)
                {
                    returned = new Manu_AuxiliaryMaterialsDetails();
                    returned.CommandID = Comon.cInt(txtCommandID.Text);
                    returned.FacilityID = UserInfo.FacilityID;
                    returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                    returned.DateROrD = Comon.cDate(GridAlcadAfter.GetRowCellValue(i, "DateROrD").ToString());
                    returned.TimeROrD =  (Comon.cDateTime(GridAlcadAfter.GetRowCellValue(i, "TimeROrD")).ToShortTimeString()); 
                    returned.BarCode = GridAlcadAfter.GetRowCellValue(i, "BarCode").ToString();
                    returned.ItemID = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "ItemID").ToString());
                    returned.EmpFactorID = Comon.cDbl(txtFactorID.Text.ToString());
                    returned.StoreID = Comon.cLong(txtStoreID.Text.ToString());
                    returned.SizeID = Comon.cInt(GridAlcadAfter.GetRowCellValue(i, "SizeID").ToString());
                    returned.ArbSizeName =  GridAlcadAfter.GetRowCellValue(i, SizeName).ToString();
                    returned.EngSizeName =  GridAlcadAfter.GetRowCellValue(i, SizeName).ToString();
                    returned.ArbItemName = GridAlcadAfter.GetRowCellValue(i, ItemName).ToString();
                    returned.EngItemName = GridAlcadAfter.GetRowCellValue(i, ItemName).ToString();
                    returned.StoreName = lblStoreNameName.Text.ToString();
                    returned.QTY = Comon.ConvertToDecimalQty(GridAlcadAfter.GetRowCellValue(i, "QTY").ToString());
                    returned.CostPrice = Comon.cDbl(GridAlcadAfter.GetRowCellValue(i, "CostPrice").ToString());
                    returned.TotalCost = Comon.cDbl(GridAlcadAfter.GetRowCellValue(i, "TotalCost").ToString());
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
                    
                    if (GridAlcadAfter.DataRowCount > 0)
                    {
                        SaveInOn();
                        // حفظ الحركة المخزنية 
                        if (Comon.cInt(Result) > 0)
                        {
                            int MoveID = SaveStockMoveingIn(Comon.cInt(txtCommandID.Text));
                            if (MoveID == 0)
                                Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");
                        }
                    }//حفظ   التوريد المخزني   
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
        long SaveVariousVoucherMachin(int DocumentID,bool isNew)
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
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_AlcadBefore.Text);
            returned.Debit = Comon.cDbl(txtTotalPrice_AlcadBefore.Text);
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
            returned.Credit = Comon.cDbl(txtTotalPrice_AlcadBefore.Text);
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_AlcadBefore.Text);
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
            returned.DebitMatirial = Comon.cDbl(txtTotalQty_AlcadAfter.Text);
            returned.Debit = Comon.cDbl(txtTotalPrice_AlcadAfter.Text);
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
            returned.Credit = Comon.cDbl(txtTotalPrice_AlcadAfter.Text);
            returned.CreditMatirial = Comon.cDbl(txtTotalQty_AlcadAfter.Text);
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
        bool IsValidGrid()
        {
            double num;

            if (HasColumnErrors)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                return !HasColumnErrors;
            }

            GridAlcadBefore.MoveLast();

            int length = GridAlcadBefore.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in GridAlcadBefore.Columns)
                {
                    if (col.FieldName == "BarCode" || col.FieldName == "CostPrice" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
                    {

                        var cellValue = GridAlcadBefore.GetRowCellValue(i, col); 

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            GridAlcadBefore.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        if (col.FieldName == "BarCode")
                            return true;

                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            GridAlcadBefore.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0)
                        {
                            GridAlcadBefore.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
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
                    strSQL = "SELECT TOP 1 *  FROM " + AuxiliaryMaterialsDAl.TableName + " Where Cancel =0 and TypeCommand=1  And BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
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
       private void SumTotalBalance(GridView Grid,int flage)
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
                    txtTotalQty_AlcadBefore.Text = TotalQtTYBefore + "";
                    txtTotalPrice_AlcadBefore.Text = TotalCostBefor + "";

                    int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                    if (isLocalCurrncy > 1)
                    {
                      
                        lblCurrencyEqvBfore.Text = Comon.cDec(Comon.cDec(txtTotalPrice_AlcadBefore.Text) * Comon.cDec(txtCurrncyPrice.Text)) + "";
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
                    txtTotalQty_AlcadAfter.Text = TotalQtTYBefore + "";
                    txtTotalPrice_AlcadAfter.Text = TotalCostBefor + "";
                    int isLocalCurrncy = Comon.cInt(Lip.GetValue("select TypeCurrency from Acc_Currency where ID=" + Comon.cInt(cmbCurency.EditValue) + " and Cancel=0"));
                    if (isLocalCurrncy > 1)
                    {
                        lblCurrencyEqvAfter.Text = Comon.cDec(Comon.cDec(txtTotalPrice_AlcadAfter.Text) * Comon.cDec(txtCurrncyPrice.Text)) + "";
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
        public void ReadRecord(int CommendID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                    dt = AuxiliaryMaterialsDAl.frmGetDataDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        txtCommandID.Text = dt.Rows[0]["CommandID"].ToString();
                        //Validate

                        txtReferanceID.Text = dt.Rows[0]["ReferanceID"].ToString();

                      cmbCurency.EditValue = Comon.cInt( dt.Rows[0]["CurrencyID"].ToString());
                        

                        txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                        txtCustomerID_Validating(null, null);

                        txtDelegateID.Text = dt.Rows[0]["DelegetID"].ToString();
                        txtDelegateID_Validating(null, null);

                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);

                        txtFactorID.Text = dt.Rows[0]["FactorID"].ToString();
                        txtFactorID_Validating(null, null);

                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);
                        
                        txtEmployeeStokID.Text = dt.Rows[0]["EmployeeStokID"].ToString();
                        txtEmployeeStokID_Validating(null, null);

                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();

                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"]);
                        txtAccountID.Text = dt.Rows[0]["AccountID"].ToString();
                        txtAccountID_Validating(null, null);
                      //  var dtr1 = dt.AsEnumerable().Where(row => row.Field<int>("TypeOpration") == 1).ToArray();
                       // dt1 = dt.AsEnumerable().Where(row => row.Field<int>("TypeOpration") == 1).CopyToDataTable();

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
                        gridControlAlcadBefore.DataSource = dt1;
                        lstDetailAlcadBefore.AllowNew = true;
                        lstDetailAlcadBefore.AllowEdit = true;
                        lstDetailAlcadBefore.AllowRemove = true;

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
                        gridControlAlcadAfter.DataSource = dt2;
                        lstDetailAlcadAfter.AllowNew = true;
                        lstDetailAlcadAfter.AllowEdit = true;
                        lstDetailAlcadAfter.AllowRemove = true;
                        SumTotalBalance(GridAlcadBefore,1);
                        SumTotalBalance(GridAlcadAfter,2);

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

        private void btnMachinResraction_Click(object sender, EventArgs e)
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
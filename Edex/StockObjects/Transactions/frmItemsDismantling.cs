using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using Edex.DAL.Stc_itemDAL;
using Edex.Model;
using Edex.Model.Language;
using Edex.GeneralObjects.GeneralClasses;
using Edex.GeneralObjects.GeneralForms;
using Edex.ModelSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Edex.StockObjects.Transactions
{
    public partial class frmItemsDismantling : Edex.GeneralObjects.GeneralForms.BaseForm
    {


        #region Declare
        DataTable dtDeclaration;
        int rowIndex;
        string FocusedControl = "";
        private string strSQL;
        private string PrimaryName;
        private string ItemName;
        private string SizeName;
        private string AnotherSizeName;
        private string CaptionFromBarCode;
        private string CaptionToBarCode;
        private string CaptionItemID;
        private string CaptionItemName;
        private string CaptionSizeID;
        private string CaptionSizeName;
        private string CaptionAnotherSizeID;
        private string CaptionAnotherSizeName;
        private string CaptionExpiryDate;
        private string CaptionQTY;
        private string CaptionPackingQty;
        private string CaptionAnotherPackingQty;
        private string CaptionDismantledQTY;
        private string CaptionDescription;
        private bool IsNewRecord;
        private Stc_ItemsDismantlingDAL cClass;
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public bool HasColumnErrors = false;
        Boolean StopSomeCode = false;
        OpenFileDialog OpenFileDialog1 = null;
        DataTable dt = new DataTable();
        GridViewMenu menu;
        //all record master and detail
        BindingList<Stc_ItemsDismantlingDetails> AllRecords = new BindingList<Stc_ItemsDismantlingDetails>();

        //list detail
        BindingList<Stc_ItemsDismantlingDetails> lstDetail = new BindingList<Stc_ItemsDismantlingDetails>();

        //Detail
        Stc_ItemsDismantlingDetails BoDetail = new Stc_ItemsDismantlingDetails();

        public CultureInfo culture = new CultureInfo("en-US");
        #endregion
        public frmItemsDismantling()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, true);

                InitializeComponent();

                ItemName = "ArbItemName";
                SizeName = "ArbSizeName";
                AnotherSizeName = "ArbAnotherSizeName";
                PrimaryName = "ArbName";
                CaptionFromBarCode = "من الباركود";
                CaptionToBarCode = " الى الباركود";
                CaptionItemID = "رقم الصنف";
                CaptionItemName = "اسم الصنف";
                CaptionSizeID = "من رقم الوحدة";
                CaptionSizeName = "من اسم الوحدة";
                CaptionExpiryDate = "تاريخ الصلاحية";
                CaptionQTY = "الكمية المراد تحويلها";
                CaptionDismantledQTY = "الكمية الناتجة";
                CaptionAnotherSizeID = "الي رقم الوحدة";
                CaptionAnotherSizeName = "الي اسم الوحدة";
                CaptionDescription = "البيان";
                CaptionPackingQty = "الـتـعـبـئـة";
                CaptionAnotherPackingQty = "الـتـعـبـئـة";
                strSQL = "ArbName";
                Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, "Arb");
                if (UserInfo.Language == iLanguage.English)
                {
                    ItemName = "EngItemName";
                    SizeName = "EngSizeName";
                    AnotherSizeName = "EngAnotherSizeName";
                    PrimaryName = "EngName";
                    CaptionFromBarCode = "From Bar Code";
                    CaptionToBarCode = "To Bar Code";
                    CaptionItemID = "Item ID";
                    CaptionItemName = "Item Name";
                    CaptionSizeID = "Size ID ";
                    CaptionSizeName = "Size Name";
                    CaptionExpiryDate = "Expiry Date";
                    CaptionQTY = "Quantity to be converted";
                    CaptionDismantledQTY = "Result Quantity";
                    CaptionAnotherSizeID = "Another Size ID";
                    CaptionAnotherSizeName = "Another Size Name";
                    CaptionDescription = "Description";
                    CaptionPackingQty = "Packing Qty";
                    CaptionAnotherPackingQty = "Packing Qty";
                    Lip.ConvertStrSQLToEnglishOrArabicLanguage(PrimaryName, "Eng");

                }
                InitGrid();
                /***********************Component ReadOnly  ****************************/
                TextEdit[] txtEdit = new TextEdit[2];
                txtEdit[0] = lblStoreName;
                txtEdit[1] = lblCostCenterName;

                foreach (TextEdit item in txtEdit)
                {
                    item.ReadOnly = true;
                    item.Enabled = false;
                    item.Properties.AppearanceDisabled.ForeColor = Color.Black;
                    item.Properties.AppearanceDisabled.BackColor = Color.WhiteSmoke;
                }
                /*********************** Date Format dd/MM/yyyy ****************************/
                InitializeFormatDate(txtDismantleDate);
                /*********************** Roles From ****************************/
                txtDismantleDate.ReadOnly = !MySession.GlobalAllowChangefrmItemsDismantlingDate;
                txtStoreID.ReadOnly = !MySession.GlobalAllowChangefrmItemsDismantlingStoreID;
                txtCostCenterID.ReadOnly = !MySession.GlobalAllowChangefrmItemsDismantlingCostCenterID;


                /********************* Event For TextEdit Component **************************/
                if (MySession.GlobalAllowWhenEnterOpenPopup)
                {
                    this.txtDismantleDate.Enter += new System.EventHandler(this.PublicTextEdit_Enter);
                }
                if (MySession.GlobalAllowWhenClickOpenPopup)
                {
                    this.txtDismantleDate.Click += new System.EventHandler(this.PublicTextEdit_Click);
                }
                this.txtDismantleID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtStoreID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);
                this.txtCostCenterID.EditValueChanged += new System.EventHandler(this.PublicTextEdit_EditValueChanged);

                this.txtDismantleID.Validating += new System.ComponentModel.CancelEventHandler(this.txtDismantleID_Validating);
                this.txtStoreID.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreID_Validating);
                this.txtCostCenterID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostCenterID_Validating);
                /***************************** Event For GridView *****************************/
                this.KeyPreview = true;
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItemsDismantling_KeyDown);
                this.gridControl.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl_ProcessGridKey);
                this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
                this.gridView1.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
                this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
                this.gridView1.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gridView1_InvalidRowException);
                this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
                this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
                /******************************************/
                DoNew();
                SplashScreenManager.CloseForm(false);
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


        #region GridView
        void InitGrid()
        {
            lstDetail = new BindingList<Stc_ItemsDismantlingDetails>();
            lstDetail.AllowNew = true;
            lstDetail.AllowEdit = true;
            lstDetail.AllowRemove = true;
            gridControl.DataSource = lstDetail;

            /******************* Columns Visible=false ********************/
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["ArbSizeName"].Visible = gridView1.Columns["ArbSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["EngSizeName"].Visible = gridView1.Columns["EngSizeName"].Name == "col" + SizeName ? true : false;
            gridView1.Columns["FromBarCode"].Visible = MySession.GlobalAllowUsingBarcodeInInvoices;
            gridView1.Columns["ExpiryDate"].Visible = MySession.GlobalAllowUsingDateItems;
            gridView1.Columns["ToBarCode"].Visible = false;
            gridView1.Columns["ArbAnotherSizeName"].Visible = false;
            gridView1.Columns["EngAnotherSizeName"].Visible = false;
            gridView1.Columns["Description"].Visible = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["DismantleID"].Visible = false;
            gridView1.Columns["StoreID"].Visible = false;
            gridView1.Columns["Cancel"].Visible = false;
            gridView1.Columns["ItemsDismantlingMaster"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["ExpiryDateStr"].Visible = false;

            /******************* Columns Visible=true *******************/

            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[SizeName].Visible = true;
            gridView1.Columns[AnotherSizeName].Visible = true;

            gridView1.Columns["FromBarCode"].Caption = CaptionFromBarCode;

            gridView1.Columns["ItemID"].Caption = CaptionItemID;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns[ItemName].Width = 200;
            gridView1.Columns["SizeID"].Caption = CaptionSizeID;
            gridView1.Columns[SizeName].Caption = CaptionSizeName;
            gridView1.Columns["PackingQty"].Caption = CaptionPackingQty;
            gridView1.Columns["QTY"].Caption = CaptionQTY;

            gridView1.Columns["ExpiryDate"].Caption = CaptionExpiryDate;

            gridView1.Columns["ToBarCode"].Caption = CaptionToBarCode;

            gridView1.Columns["AnotherSizeID"].Caption = CaptionAnotherSizeID;
            gridView1.Columns[AnotherSizeName].Caption = CaptionAnotherSizeName;
            gridView1.Columns["AnotherPackingQty"].Caption = CaptionAnotherPackingQty;
            gridView1.Columns["DismantledQTY"].Caption = CaptionDismantledQTY;

            gridView1.Columns["Description"].Caption = CaptionDescription;

            gridView1.Focus();
            /*************************Columns Properties ****************************/
            gridView1.Columns[ItemName].OptionsColumn.ReadOnly = true;
            gridView1.Columns[ItemName].OptionsColumn.AllowFocus = false;
            gridView1.Columns[SizeName].OptionsColumn.ReadOnly = true;
            gridView1.Columns[SizeName].OptionsColumn.AllowFocus = false;
            gridView1.Columns[AnotherSizeName].OptionsColumn.ReadOnly = true;
            gridView1.Columns[AnotherSizeName].OptionsColumn.AllowFocus = false;
            gridView1.Columns["DismantledQTY"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["DismantledQTY"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["PackingQty"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["PackingQty"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["AnotherPackingQty"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["AnotherPackingQty"].OptionsColumn.AllowFocus = false;
            /************************ Date Time **************************/

            RepositoryItemDateEdit RepositoryDateEdit = new RepositoryItemDateEdit();
            RepositoryDateEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            RepositoryDateEdit.Mask.EditMask = "dd/MM/yyyy";
            RepositoryDateEdit.Mask.UseMaskAsDisplayFormat = true;
            gridControl.RepositoryItems.Add(RepositoryDateEdit);
            gridView1.Columns["ExpiryDate"].ColumnEdit = RepositoryDateEdit;
            gridView1.Columns["ExpiryDate"].UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            gridView1.Columns["ExpiryDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.Columns["ExpiryDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridView1.Columns["ExpiryDate"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["ExpiryDate"].OptionsColumn.ReadOnly = false;


            /************************ Auto Number **************************/
            DevExpress.XtraGrid.Columns.GridColumn col = gridView1.Columns.AddVisible("#");
            col.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.ReadOnly = true;
            col.OptionsColumn.AllowFocus = false;
            col.Width = 20;
            gridView1.BestFitColumns();

        }

        private void gridView1_ShownEditor(object sender, EventArgs e)
        {
            HasColumnErrors = false;
            CalculateRow();
        }
        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (!gridView1.IsLastVisibleRow)
                    gridView1.MoveLast();

                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "FromBarCode" || col.FieldName == "DismantledQTY" || col.FieldName == "colAnotherSizeName" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
                    {

                        var val = gridView1.GetRowCellValue(e.RowHandle, col);
                        double num;
                        if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                        }
                        if (col.FieldName == "BarCode")
                            return;
                        else if (!(double.TryParse(val.ToString(), out num)))
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                        }
                        else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0 )
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
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
                if (ColName == "FromBarCode" || ColName == "DismantledQTY" || ColName == "SizeID" || ColName == "AnotherSizeID" || ColName == "ItemID" || ColName == "QTY")
                {
                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsRequired;
                    }
                    else if (!(double.TryParse(val.ToString(), out num)))
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputShouldBeNumber;
                    }
                    else if (Comon.ConvertToDecimalPrice(val.ToString()) <= 0)
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgInputIsGreaterThanZero;
                    }

                    /****************************************/
                    if (ColName == "FromBarCode")
                    {
                        DataTable dt = Stc_itemsDAL.GetItemData(val.ToString(), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisBarCode;
                        }
                        else
                            FileItemData(dt);

                    }
                    else if (ColName == "SizeID")
                    {
                        int ItemID = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]).ToString());
                        DataTable dt = Stc_itemsDAL.GetItemDataByItemID_SizeID(ItemID, Comon.cInt(val.ToString()), UserInfo.FacilityID);
                        if (dt.Rows.Count == 0)
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundSizeForItem;
                        }
                        else
                            FileItemData(dt);
                    }
                    else if (ColName == "AnotherSizeID")
                    {
                        int SizeID = Comon.cInt(val.ToString());
                     //   int ItemID = Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]).ToString());

                        var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                        var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FromBarCode"]);
                        if (itemID != null && Barcode != null)
                        {
                            string SQL = @"SELECT  dbo.Stc_SizingUnits.ArbName, dbo.Stc_SizingUnits.EngName, dbo.Stc_ItemUnits.PackingQty, dbo.Stc_ItemUnits.SizeID
                                         FROM      dbo.Stc_ItemUnits INNER JOIN dbo.Stc_SizingUnits ON dbo.Stc_ItemUnits.SizeID = dbo.Stc_SizingUnits.SizeID  Where dbo.Stc_ItemUnits.UnitCancel=0  AND dbo.Stc_ItemUnits.itemID=" + itemID + "  AND dbo.Stc_SizingUnits.Cancel=0 And dbo.Stc_ItemUnits.SizeID=" + SizeID;
                            DataTable dt = Lip.SelectRecord(SQL);
                            if (dt.Rows.Count == 0)
                            {
                                Messages.MsgStop(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Unit is not Correct" : "الوحدة غير صحيحة"));
                                return;
                            }
                            if (Stc_itemsDAL.CheckIfStopItemUnit(Comon.cInt(itemID), SizeID, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                            {
                                Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                                return;
                            }
                            if (Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"])) == SizeID)
                            {
                                Messages.MsgStop(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "You Can not Select The Same Unit" : "لا يمكن اختيار نفس الوحدة"));
                                return;
                            }

                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherSizeID"], SizeID);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherPackingQty"], dt.Rows[0]["PackingQty"].ToString());
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AnotherSizeName], dt.Rows[0]["ArbName"].ToString());
                            if (UserInfo.Language == iLanguage.English)
                            {
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AnotherSizeName], dt.Rows[0]["EngName"].ToString());

                            }
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ToBarCode"], GenerateBarcode(gridView1.FocusedRowHandle, SizeID));

                            CalculateRow();
                        }
                    }
                }
            }

        }
        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e)
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

                else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                {
                    if (view.ActiveEditor is TextEdit)
                    {

                        double num;
                        HasColumnErrors = false;
                        var cellValue = view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn.FieldName);
                        string ColName = view.FocusedColumn.FieldName;
                        if (ColName == "FromBarCode" || ColName == "DismantledQTY" || ColName == "AnotherSizeID" || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID")
                        {

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsRequired);

                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)))
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0)
                            {

                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }

                        }

                    }
                }
                else if (e.KeyData == Keys.Delete)
                {

                    if (!IsNewRecord)
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
                    CalculateRow();
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
        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = (e.ListSourceRowIndex + 1);
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            rowIndex = e.RowHandle;
        }

        private void FileItemData(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {

                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FromBarCode"], dt.Rows[0]["BarCode"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ItemName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                if (UserInfo.Language == iLanguage.English)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherSizeID"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AnotherSizeName], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], Comon.ConvertSerialToDate(dt.Rows[0]["ExpiryDate"].ToString()));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], dt.Rows[0]["PackingQty"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherPackingQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DismantledQTY"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");

            }
            else
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AnotherSizeName], " ");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ExpiryDate"], DateTime.Now.ToShortDateString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DismantledQTY"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherSizeID"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FromBarCode"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ToBarCode"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["StoreID"], txtStoreID.Text);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Description"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherPackingQty"], 0);
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

            foreach (GridColumn col in gridView1.Columns)
            {
                if (col.FieldName == "FromBarCode" || col.FieldName == "DismantledQTY" || col.FieldName == "AnotherSizeID" || col.FieldName == "Description" || col.FieldName == "ExpiryDate" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY")
                {
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowEdit = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.AllowFocus = Value;
                    gridView1.Columns[col.FieldName].OptionsColumn.ReadOnly = !Value;
                }

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

            gridView1.MoveLast();

            int length = gridView1.RowCount - 1;
            if (length <= 0)
            {
                Messages.MsgError(Messages.TitleError, Messages.msgThereIsNoRecordInput);
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                foreach (GridColumn col in gridView1.Columns)
                {
                    if (col.FieldName == "FromBarCode" || col.FieldName == "DismantledQTY" || col.FieldName == "colAnotherSizeName" || col.FieldName == "SizeID" || col.FieldName == "ItemID" || col.FieldName == "QTY" || col.FieldName == "SizeID")
                    {

                        var cellValue = gridView1.GetRowCellValue(i, col);

                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsRequired);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }


                        if (col.FieldName == "BarCode")
                            return true;
                        else if (!(double.TryParse(cellValue.ToString(), out num)))
                        {
                            gridView1.SetColumnError(col, Messages.msgInputShouldBeNumber);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                        else if (Comon.cDbl(cellValue.ToString()) <= 0 )
                        {
                            gridView1.SetColumnError(col, Messages.msgInputIsGreaterThanZero);
                            Messages.MsgError(Messages.TitleError, Messages.msgThereIsErrorInput);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #region Calculate
        private void CalculateRow()
        {
            try
            {
                double DismantledQTY = Comon.cDbl((Comon.cDbl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"])) * Comon.cDbl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PackingQty"])) / Comon.cDbl(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherPackingQty"]))));
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DismantledQTY"], DismantledQTY);
            }

            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        #endregion

        #region Other Function
        public void Find()
        {

            CSearch cls = new CSearch();
            int[] ColumnWidth = new int[] { 100, 300 };
            string SearchSql = "";
            string Condition = "Where 1=1";

            FocusedControl = GetIndexFocusedControl();

            if (FocusedControl == null) return;



            else if (FocusedControl.Trim() == txtStoreID.Name)
            {
                if (!MySession.GlobalAllowChangefrmItemsDismantlingStoreID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "رقم الـمـســتـودع", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtStoreID, lblStoreName, "StoreID", "Store ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == txtDismantleID.Name)
            {
                if (!FormView) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtDismantleID, null, "ItemsDismantling", "رقـم العملية", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtDismantleID, null, "ItemsDismantling", "Dismantle ID", MySession.GlobalBranchID);
            }

            else if (FocusedControl.Trim() == txtCostCenterID.Name)
            {
                if (!MySession.GlobalAllowChangefrmItemsDismantlingCostCenterID) { Messages.MsgExclamationk(Messages.TitleInfo, Messages.msgNoPermissionToChange); return; };

                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "رقم مركز التكلفة", MySession.GlobalBranchID);
                else
                    PrepareSearchQuery.Find(ref cls, txtCostCenterID, lblCostCenterName, "CostCenterID", "Cost Center ID", MySession.GlobalBranchID);
            }
            else if (FocusedControl.Trim() == gridControl.Name)
            {
                if (gridView1.FocusedColumn == null) return;

                if (gridView1.FocusedColumn.Name == "colFromBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "BarCode", MySession.GlobalBranchID);
                }
                if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                {
                    var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                    var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FromBarCode"]);
                    if (itemID != null && Barcode != null)
                    {

                        Condition += " And ItemID=" + Comon.cInt(itemID);
                        if (UserInfo.Language == iLanguage.Arabic)
                            PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "رقـم الـوحـــده", MySession.GlobalBranchID, Condition);
                        else
                            PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "Size ID", MySession.GlobalBranchID, Condition);
                    }
                }
                else if (gridView1.FocusedColumn.Name == "colAnotherSizeName" || gridView1.FocusedColumn.Name == "colAnotherSizeID")
                {

                    var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                    var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FromBarCode"]);
                    if (itemID != null && Barcode != null)
                    {
                        Condition += " And ItemID=" + Comon.cInt(itemID);
                        if (UserInfo.Language == iLanguage.Arabic)
                            PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "رقـم الـوحـــده", MySession.GlobalBranchID, Condition);
                        else
                            PrepareSearchQuery.Find(ref cls, null, null, "ItemBySize", "Size ID", MySession.GlobalBranchID, Condition);
                    }
                }
            }
            GetSelectedSearchValue(cls);
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtStoreID.Name)
                {
                    txtStoreID.Text = cls.PrimaryKeyValue.ToString();
                    txtStoreID_Validating(null, null);
                }

                else if (FocusedControl == txtCostCenterID.Name)
                {
                    txtCostCenterID.Text = cls.PrimaryKeyValue.ToString();
                    txtCostCenterID_Validating(null, null);
                }

                else if (FocusedControl == txtDismantleID.Name)
                {
                    txtDismantleID.Text = cls.PrimaryKeyValue.ToString();
                    txtDismantleID_Validating(null, null);
                }

                else if (FocusedControl == gridControl.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colFromBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemDataWithPackingQty(Barcode, UserInfo.FacilityID));
                        CalculateRow();
                        Find();
                    }
                    else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                    {

                        int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                        var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FromBarCode"]);
                        if (itemID != null && Barcode != null)
                        {

                            if (Stc_itemsDAL.CheckIfStopItemUnit(Comon.cInt(itemID), SizeID, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                            {
                                Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                                return;
                            }
                            FileItemData(Stc_itemsDAL.GetItemDataByItemID_SizeID(Comon.cInt(itemID), SizeID, UserInfo.FacilityID));
                            CalculateRow();
                        }

                    }
                    else if (gridView1.FocusedColumn.Name == "colAnotherSizeName" || gridView1.FocusedColumn.Name == "colAnotherSizeID")
                    {

                        int SizeID = Comon.cInt(cls.PrimaryKeyValue.ToString());
                        var itemID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"]);
                        var Barcode = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["FromBarCode"]);
                        if (itemID != null && Barcode != null)
                        {
                            string SQL = @"SELECT  dbo.Stc_SizingUnits.ArbName, dbo.Stc_SizingUnits.EngName, dbo.Stc_ItemUnits.PackingQty, dbo.Stc_ItemUnits.SizeID
                                         FROM      dbo.Stc_ItemUnits INNER JOIN dbo.Stc_SizingUnits ON dbo.Stc_ItemUnits.SizeID = dbo.Stc_SizingUnits.SizeID  Where dbo.Stc_ItemUnits.UnitCancel=0  AND dbo.Stc_ItemUnits.itemID=" + itemID + "  AND dbo.Stc_SizingUnits.Cancel=0 And dbo.Stc_ItemUnits.SizeID=" + SizeID ;
                            DataTable dt = Lip.SelectRecord(SQL);
                            if (dt.Rows.Count == 0)
                            {
                                Messages.MsgStop(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "Unit is not Correct" : "الوحدة غير صحيحة"));
                                return;
                            }
                            if (Stc_itemsDAL.CheckIfStopItemUnit(Comon.cInt(itemID), SizeID, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                            {
                                Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                                return;
                            }
                            if (Comon.cInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"])) == SizeID)
                            {
                                Messages.MsgStop(Messages.TitleInfo, (UserInfo.Language == iLanguage.English ? "You Can not Select The Same Unit" : "لا يمكن اختيار نفس الوحدة"));
                                return;
                            }

                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherSizeID"], SizeID);
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["AnotherPackingQty"], dt.Rows[0]["PackingQty"].ToString());
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AnotherSizeName], dt.Rows[0]["ArbName"].ToString());
                            if (UserInfo.Language == iLanguage.English)
                            {
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[AnotherSizeName], dt.Rows[0]["EngName"].ToString());

                            }
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ToBarCode"], GenerateBarcode(gridView1.FocusedRowHandle, SizeID));
                       
                            CalculateRow();
                        }
                    }
                }
            }

        }
        public void ReadRecord(long DismantleID)
        {
            try
            {
                
                ClearFields();
                {
                    dt = Stc_ItemsDismantlingDAL.frmGetDataDetalByID(DismantleID, UserInfo.BRANCHID, UserInfo.FacilityID);
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        //Validate
                        IsNewRecord = false;
                        txtStoreID.Text = dt.Rows[0]["StoreID"].ToString();
                        txtStoreID_Validating(null, null);

                        txtCostCenterID.Text = dt.Rows[0]["CostCenterID"].ToString();
                        txtCostCenterID_Validating(null, null);


                        txtEnteredByUserID.Text = dt.Rows[0]["UserID"].ToString();
                        txtEnteredByUserID_Validating(null, null);

                        txtEditedByUserID.Text = dt.Rows[0]["EditUserID"].ToString();
                        txtEditedByUserID_Validating(null, null);


                        //Masterdata
                        txtDismantleID.Text = dt.Rows[0]["DismantleID"].ToString();
                        txtNotes.Text = dt.Rows[0]["Notes"].ToString();

                        //Date

                        if (Comon.ConvertSerialDateTo(dt.Rows[0]["DismantleDate"].ToString()) == "")
                            txtDismantleDate.Text = "";

                        else
                        //    txtDismantleDate.DateTime = Convert.ToDateTime(Comon.ConvertSerialDateTo(dt.Rows[0]["DismantleDate"].ToString()));
                        txtDismantleDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["DismantleDate"].ToString()), "dd/MM/yyyy", culture);//CultureInfo.InvariantCulture);



                      //  txtDismantleDate.Text = Comon.ConvertSerialDateTo(dt.Rows[0]["DismantleDate"].ToString());

                        gridControl.DataSource = dt;

                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;

                        CalculateRow();

                        ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtDismantleID.Text;
                    }
                }


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
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
        public void ClearFields()
        {
            try
            {



                txtNotes.Text = "";
                txtDismantleDate.EditValue = DateTime.Now;

                txtNotes.Text = "";


                txtEnteredByUserID.Text = UserInfo.ID.ToString();
                txtEnteredByUserID_Validating(null, null);

                txtEditedByUserID.Text = "0";
                txtEditedByUserID_Validating(null, null);



                txtCostCenterID.Text = MySession.GlobalDefaultCostCenterID;
                txtCostCenterID_Validating(null, null);


                txtStoreID.Text = MySession.GlobalDefaultStoreID;
                txtStoreID_Validating(null, null);


                lstDetail = new BindingList<Stc_ItemsDismantlingDetails>();

                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl.DataSource = lstDetail;

                dt = new DataTable();


                ribbonControl1.Pages[0].Groups[0].ItemLinks[6].Caption = txtDismantleID.Text;

            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
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
                    strSQL = "SELECT TOP 1 * FROM " + Stc_ItemsDismantlingDAL.TableName + " Where Cancel =0 ";
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Stc_ItemsDismantlingDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Stc_ItemsDismantlingDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Stc_ItemsDismantlingDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Stc_ItemsDismantlingDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Stc_ItemsDismantlingDAL.PremaryKey + " desc";
                                break;
                            }

                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Stc_ItemsDismantlingDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Stc_ItemsDismantlingDAL();

                    long InvoicIDTemp = Comon.cLong(txtDismantleID.Text);
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
                    Messages.MsgStop(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                    return;
                }
                SplashScreenManager.CloseForm(false);

            }

            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
        }
        #endregion
        #region Do Function
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                txtDismantleID.Text = Stc_ItemsDismantlingDAL.GetNewID().ToString();

                ClearFields();
                EnabledControl(true);
                gridView1.Focus();
                gridView1.MoveLast();
                gridView1.FocusedColumn = gridView1.VisibleColumns[2];
                gridView1.ShowEditor();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                MoveRec(Comon.cInt(txtDismantleID.Text), xMoveNext);


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
                MoveRec(Comon.cInt(txtDismantleID.Text), xMovePrev);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        protected override void DoSearch()
        {
            try
            {
                txtDismantleID.Enabled = true;
                txtDismantleID.Focus();
                Find();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        protected override void DoEdit()
        {

            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("DismantleID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("FromBarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ToBarCode", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ItemID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("SizeID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("AnotherSizeID", System.Type.GetType("System.String"));
            dtItem.Columns.Add(ItemName, System.Type.GetType("System.String"));
            dtItem.Columns.Add(SizeName, System.Type.GetType("System.String"));
            dtItem.Columns.Add(AnotherSizeName, System.Type.GetType("System.String"));
            dtItem.Columns.Add("QTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("DismantledQTY", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("ExpiryDateStr", System.Type.GetType("System.Decimal"));
            dtItem.Columns.Add("Description", System.Type.GetType("System.String"));
            dtItem.Columns.Add("ExpiryDate", System.Type.GetType("System.DateTime"));
            dtItem.Columns.Add("StoreID", System.Type.GetType("System.String"));
            dtItem.Columns.Add("Cancel", System.Type.GetType("System.String"));
            dtItem.Columns.Add("BranchID", System.Type.GetType("System.String"));
            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                dtItem.Rows.Add();
                dtItem.Rows[i]["ID"] = i;
                dtItem.Rows[i]["BranchID"] = UserInfo.BRANCHID;
                dtItem.Rows[i]["FromBarCode"] = gridView1.GetRowCellValue(i, "FromBarCode").ToString();
                dtItem.Rows[i]["ToBarCode"] = gridView1.GetRowCellValue(i, "ToBarCode").ToString();
                dtItem.Rows[i]["ItemID"] = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                dtItem.Rows[i]["SizeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                dtItem.Rows[i]["AnotherSizeID"] = Comon.cInt(gridView1.GetRowCellValue(i, "AnotherSizeID").ToString());
                dtItem.Rows[i][ItemName] = gridView1.GetRowCellValue(i, ItemName).ToString();
                dtItem.Rows[i][SizeName] = gridView1.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i][AnotherSizeName] = gridView1.GetRowCellValue(i, SizeName).ToString();
                dtItem.Rows[i]["QTY"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "QTY").ToString());
                dtItem.Rows[i]["DismantledQTY"] = Comon.ConvertToDecimalPrice(gridView1.GetRowCellValue(i, "DismantledQTY").ToString());
                dtItem.Rows[i]["Description"] = gridView1.GetRowCellValue(i, "Description").ToString();
                dtItem.Rows[i]["ExpiryDateStr"] = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString());
                dtItem.Rows[i]["ExpiryDate"] = gridView1.GetRowCellValue(i, "ExpiryDate");
                dtItem.Rows[i]["StoreID"] = Comon.cInt(gridView1.GetRowCellValue(i, "StoreID").ToString());
                dtItem.Rows[i]["Cancel"] = 0;

            }
            gridControl.DataSource = dtItem;
            EnabledControl(true);
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
        private void Save()
        {
            gridView1.MoveLastVisible();
            CalculateRow();
       
            gridView1.FocusedColumn = gridView1.VisibleColumns[1];

            txtDismantleDate_EditValueChanged(null, null);

            Stc_ItemsDismantlingMaster objRecord = new Stc_ItemsDismantlingMaster();
            objRecord.DismantleID = 0;
            objRecord.BranchID = UserInfo.BRANCHID;
            objRecord.FacilityID = UserInfo.FacilityID;
            objRecord.DismantleDate = Comon.ConvertDateToSerial(txtDismantleDate.Text).ToString();
            objRecord.CostCenterID = Comon.cInt(txtCostCenterID.Text);
            objRecord.StoreID = Comon.cInt(txtStoreID.Text);
            txtNotes.Text = (txtNotes.Text.Trim() != "" ? txtNotes.Text.Trim() : (UserInfo.Language == iLanguage.English ? "Dismantle  Invoice" : " فاتوره  تفكيك وتجميع "));
            objRecord.Notes = txtNotes.Text;
            //Ammount
            objRecord.Cancel = 0;
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
                objRecord.DismantleID = Comon.cInt(txtDismantleID.Text);
                objRecord.EditUserID = UserInfo.ID;
                objRecord.EditTime = Comon.cDbl(Lip.GetServerTimeSerial());
                objRecord.EditDate = Comon.cDbl(Comon.ConvertDateToSerial(Lip.GetServerDate()));
                objRecord.EditComputerInfo = UserInfo.ComputerInfo;
            }

            Stc_ItemsDismantlingDetails returned;
            List<Stc_ItemsDismantlingDetails> listreturned = new List<Stc_ItemsDismantlingDetails>();


            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
            {
                returned = new Stc_ItemsDismantlingDetails();
                returned.ID = i;
                returned.BranchID = UserInfo.BRANCHID;
                returned.FacilityID = UserInfo.FacilityID;
                returned.FromBarCode = gridView1.GetRowCellValue(i, "FromBarCode").ToString();
                returned.ToBarCode = gridView1.GetRowCellValue(i, "ToBarCode").ToString();
                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString());
                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                returned.AnotherSizeID = Comon.cInt(gridView1.GetRowCellValue(i, "AnotherSizeID").ToString());
                returned.QTY = Comon.cDbl(gridView1.GetRowCellValue(i, "QTY").ToString());
                returned.DismantledQTY = Comon.cDbl(gridView1.GetRowCellValue(i, "DismantledQTY").ToString());
                returned.Description = gridView1.GetRowCellValue(i, "Description").ToString();
                returned.StoreID = Comon.cInt(txtStoreID.Text);
                returned.ExpiryDateStr = Comon.ConvertDateToSerial(gridView1.GetRowCellValue(i, "ExpiryDate").ToString().Substring(0, 10));
                returned.Cancel = 0;

                if (returned.QTY <= 0 || returned.ToBarCode == "" || returned.StoreID <= 0 || returned.SizeID <= 0 || returned.ItemID <= 0)
                    continue;
                listreturned.Add(returned);

            }

            if (listreturned.Count > 0)
            {
                objRecord.ItemsDismantlingDetails = listreturned;
                int Result = Stc_ItemsDismantlingDAL.InsertUsingXML(objRecord, IsNewRecord);
                SplashScreenManager.CloseForm(false);

                if (IsNewRecord == true)
                {
                    if (Result == 1)
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                        DoNew();
                    }
                    else if (Result == 0)
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave);

                    }

                }
                else
                {

                    if (Result == 1)
                    {
                        txtDismantleID_Validating(null, null);
                        EnabledControl(false);
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);

                    }
                    else if (Result == 0)
                    {
                        Messages.MsgError(Messages.TitleError, Messages.msgErrorSave);

                    }
                }

            }
            else
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.TitleError, Messages.msgInputIsRequired);

            }

        }
        private string GenerateBarcode(int i, int SizeID)
        {
            try
            {
                string barcode="";

                // نبحث برقم الماده والوحده وسعر البيع وتاريخ الانتهاء
                int ItemId = Comon.cInt(gridView1.GetRowCellValue(i, gridView1.Columns["ItemID"]));
                long ExpiryDate = Comon.ConvertDateToSerial(Convert.ToDateTime(gridView1.GetRowCellValue(i, "ExpiryDate").ToString()).ToString("dd/MM/yyyy"));
                
                string strSQL;
                DataTable dt;


                strSQL = "SELECT TOP 1 BarCode FROM  Sales_PurchaseInvoiceDetails "
                + " WHERE ItemID =" + ItemId + "  And SizeId = " + SizeID
                + " AND  ExpiryDate = " + ExpiryDate; // SalePrice = " & SalePrice & "  AND 
                dt = Lip.SelectRecord(strSQL);
                if (dt.Rows.Count > 0)
                    return barcode = dt.Rows[0]["BarCode"].ToString();
                else
                {
                    // نبحث برقم الماده والوحده مرتب حسب اخر باركود بحيث التاريخ لايساوي صفر ثم نزيد الباركود بواحد

                    long code;
                    string strSQL2 = "SELECT TOP 1 BarCode FROM  Sales_PurchaseInvoiceDetails "
                    + " WHERE ExpiryDate<>0 And ItemID =" + ItemId
                    + " ORDER BY  CONVERT(float,BarCode)  DESC ";
                    DataTable dt2 = Lip.SelectRecord(strSQL2);
                    if (dt2.Rows.Count > 0)
                    {
                        barcode = dt2.Rows[0]["BarCode"].ToString();
                        code = Comon.cLong(barcode) + 1;
                        return barcode = Microsoft.VisualBasic.Strings.Format(code, "000000000");
                       

                    }
                    else
                        // ننشأ باركود مكون من رقم الماده خمس خانات + 0001 بحيث يصبح الباركود مكون من ثمان خانات
                        return barcode = Microsoft.VisualBasic.Strings.Format(ItemId, "00000") + "0001";
                }
              
            }
            catch (Exception ex)
            {

                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return "";
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
                int TempID = Comon.cInt(txtDismantleID.Text);

                Stc_ItemsDismantlingMaster model = new Stc_ItemsDismantlingMaster();
                model.DismantleID = Comon.cInt(txtDismantleID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = UserInfo.BRANCHID;
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial());

                int Result = Stc_ItemsDismantlingDAL.DeleteStc_ItemsDismantlingMaster(model);
                SplashScreenManager.CloseForm(false);
                if (Result > 0)
                    Messages.MsgInfo(Messages.TitleInfo, Messages.msgDeleteComplete);
                else if (Result == 0)
                    Messages.MsgInfo(Messages.TitleError, Messages.msgErrorSave);

                MoveRec(model.DismantleID, xMovePrev);


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
        #endregion
        #region Validating

        private void txtDismantleID_Validating(object sender, CancelEventArgs e)
        {
            if (FormView == true)
                ReadRecord(Comon.cLong(txtDismantleID.Text));
            else
            {
                Messages.MsgInfo(Messages.TitleInfo, Messages.msgNoPermissionToViewRecord);
                return;
            }

        }
        private void txtStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE StoreID =" + Comon.cInt(txtStoreID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtStoreID, lblStoreName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void txtEnteredByUserID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtEnteredByUserID.Text) + " And Cancel =0 And BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtEnteredByUserID, lblEnteredByUserName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

        }
        private void txtEditedByUserID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as UserName FROM Users WHERE UserID =" + Comon.cInt(txtEditedByUserID.Text) + " And Cancel =0 ";
                CSearch.ControlValidating(txtEditedByUserID, lblEditedByUserName, strSQL);
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
                strSQL = "SELECT " + PrimaryName + " as CostCenterName FROM Acc_CostCenters WHERE CostCenterID =" + Comon.cInt(txtCostCenterID.Text) + " And Cancel =0 And  BranchID =" + UserInfo.BRANCHID;
                CSearch.ControlValidating(txtCostCenterID, lblCostCenterName, strSQL);
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }

        #endregion
        #region Event TextEdit
        private void PublicTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            ((TextEdit)sender).Properties.Appearance.BorderColor = Color.Black;

        }
        private void PublicTextEdit_Enter(object sender, EventArgs e)
        {
            (sender as DateEdit).ShowPopup();
        }
        private void PublicTextEdit_Click(object sender, EventArgs e)
        {
            (sender as DateEdit).ShowPopup();
        }
        private void frmItemsDismantling_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == Keys.F4.ToString())
                Find();
        }

        #endregion
        #region InitializeComponent
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
        #endregion

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void frmItemsDismantling_Load(object sender, EventArgs e)
        {
            ribbonControl1.Pages[0].Groups[0].ItemLinks[1].Visible = false;
            ribbonControl1.Pages[0].Groups[0].ItemLinks[3].Visible = false;
        
            ribbonControl1.Pages[0].Groups[0].ItemLinks[11].Visible = false;

        }
        protected override void DoPrint()
        {
            gridView1.ShowRibbonPrintPreview();
        }

        private void txtDismantleDate_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDismantleDate.Text.Trim()))
                txtDismantleDate.EditValue = DateTime.Now;
            if (Comon.ConvertDateToSerial(txtDismantleDate.Text) > Comon.cLong((Lip.GetServerDateSerial())))
                txtDismantleDate.Text = Lip.GetServerDate();
        }

    }
}

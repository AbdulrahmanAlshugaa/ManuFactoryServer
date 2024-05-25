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
using Edex.GeneralObjects.GeneralClasses;
using Edex.Model.Language;
using Edex.ModelSystem;
using Edex.DAL.ManufacturingDAL;
using DevExpress.XtraSplashScreen;
using System.Globalization;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using Edex.DAL;
using DevExpress.Utils;
using Edex.DAL.Stc_itemDAL;
using Edex.StockObjects.Codes;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using Edex.DAL.SalseSystem.Stc_itemDAL;
using Edex.DAL.Accounting;
using Edex.AccountsObjects.Transactions;

namespace Edex.Manufacturing.Codes
{
    public partial class frmClosingOrders :BaseForm
    {
        #region 
        BindingList<Manu_CloseOrdersDetails> lstDetail = new BindingList<Manu_CloseOrdersDetails>();
        BindingList<Manu_CloseOrdersDetails> lstDetailAfter = new BindingList<Manu_CloseOrdersDetails>();
        private bool IsNewRecord;
        private string strSQL;
        private string PrimaryName;
        string FocusedControl = ""; 
        public const int xMoveFirst = 7;
        public const int xMovePrev = 8;
        public const int xMoveNext = 9;
        public const int xMoveLast = 10;
        public int DocumentTypeCadFactory = 52;
        private string ItemName;
        private string SizeName;
        private DataTable dt;
        DataTable DataRecord;
        DataTable DataRecordAfter;
        private Manu_CloseOrdersDAL cClass;
        private string CaptionItemName;
        public bool HasColumnErrors = false;
        public CultureInfo culture = new CultureInfo("en-US");
        #endregion
        public frmClosingOrders()
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

            FillCombo.FillComboBox(cmbCurency, "Acc_Currency", "ID", "ArbName", "", "BranchID="+MySession.GlobalBranchID, (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
               
            FillCombo.FillComboBox(cmbTypeStage, "Manu_TypeStages", "ID", "ArbName", "", "", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBox(cmbTypeOrders, "Manu_TypeOrders", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد  "));
            FillCombo.FillComboBoxLookUpEdit(cmbBranchesID, "Branches", "BranchID", "ArbName", "", "1=1", (UserInfo.Language == iLanguage.English ? "Select Branch" : "حدد الفرع"));
            cmbBranchesID.EditValue = MySession.GlobalBranchID;

            FillCombo.FillComboBox(cmbStatus, "Manu_TypeStatus", "ID", PrimaryName, "", "1=1", (UserInfo.Language == iLanguage.English ? "Select " : "حدد الحالة  "));
            cmbStatus.EditValue = MySession.GlobalDefaultProcessPostedStatus;

            txtOrderID.Validating+=txtOrderID_Validating;
            txtBeforeStoreID.Validating+=txtBeforeStoreID_Validating;
            txtAfterStoreID.Validating += txtAfterStoreID_Validating;
            InitializeFormatDate(txtOrderDate);
            InitializeFormatDate(txtCommandDate);
            this.txtCommandStage.Validating += txtCommandStage_Validating;
            this.cmbTypeStage.Validating += txtCommandStage_Validating;
            this.GridCad.CustomDrawCell += GridCadWax_CustomDrawCell;
            this.gridView1.CustomDrawCell += GridCadWax_CustomDrawCell;
            this.KeyDown += frmClosingOrders_KeyDown;
            this.gridControl2.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl11_ProcessGridKey);
            this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
            this.txtCommandID.Validating+=txtCommandID_Validating;
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
        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (this.GridCad.ActiveEditor is TextEdit)
            {
                GridView view = sender as GridView;
                double num;
                object val = e.Value;
                HasColumnErrors = false;


                string ColName = view.FocusedColumn.FieldName;
                if (ColName == "BarCode" || ColName == "SizeID"   || ColName == "ItemID" || ColName == "QTY")
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
                        view.SetColumnError(gridView1.Columns[ColName], "");

                    }
                    if (ColName == "QTY")
                    {
                        HasColumnErrors = false;
                        e.Valid = true;
                        gridView1.SetColumnError(gridView1.Columns["QTY"], "");
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
                                FileItemData(dt);
                                MySession.GlobalAllowUsingDateItems = true;
                            }
                            else
                                FileItemData(dt);
                            if (HasColumnErrors == false)
                            {
                                e.Valid = true;
                                view.SetColumnError(gridView1.Columns[ColName], "");
                                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle;
                                gridView1.FocusedColumn = gridView1.VisibleColumns[0];
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
                            view.SetColumnError(gridView1.Columns[ColName], "");
                        }
                    }
                    else if (ColName == "SizeID")
                    {
                        DataTable dtItemID = Lip.SelectRecord("Select " + PrimaryName + " from Stc_SizingUnits Where Cancel=0  and BranchID="+MySession.GlobalBranchID+"  And LOWER (SizeID)=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                        if (dtItemID.Rows.Count > 0)
                        {

                            gridView1.SetFocusedRowCellValue(SizeName, dtItemID.Rows[0][PrimaryName]);
                            e.Valid = true;
                            view.SetColumnError(gridView1.Columns[ColName], "");

                        }
                        else
                        {
                            e.Valid = false;
                            HasColumnErrors = true;
                            e.ErrorText = Messages.msgNoFoundThisItem;
                            view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                        }
                    }
                }
                else if (ColName == SizeName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select SizeID from Stc_SizingUnits Where Cancel=0 and BranchID="+MySession.GlobalBranchID+" And LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') And FacilityID=" + UserInfo.FacilityID);
                    if (dtItemID.Rows.Count > 0)
                    {

                        gridView1.SetFocusedRowCellValue("SizeID", dtItemID.Rows[0]["SizeID"]);
                        e.Valid = true;
                        view.SetColumnError(gridView1.Columns[ColName], "");
                    }
                    else
                    {
                        e.Valid = false;
                        HasColumnErrors = true;
                        e.ErrorText = Messages.msgNoFoundThisItem;
                        view.SetColumnError(gridView1.Columns[ColName], Messages.msgNoFoundThisItem);
                    }
                }

                if (ColName == ItemName)
                {
                    DataTable dtItemID = Lip.SelectRecord("Select  ItemID from Stc_Items  Where Cancel =0 and BranchID="+MySession.GlobalBranchID+" and LOWER (" + PrimaryName + ")=LOWER ('" + val.ToString() + "') ");
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
                    if (this.gridView1.ActiveEditor is CheckEdit)
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
                        if (ColName == "BarCode"   || ColName == "ItemID" || ColName == "QTY" || ColName == "SizeID")
                        {

                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsRequired);
                            }
                            else if (!(double.TryParse(cellValue.ToString(), out num)) && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputShouldBeNumber);
                            }
                            else if (Comon.ConvertToDecimalPrice(cellValue.ToString()) <= 0 && ColName != "BarCode")
                            {
                                HasColumnErrors = true;
                                view.SetColumnError(gridView1.Columns[ColName], Messages.msgInputIsGreaterThanZero);
                            }
                            else
                            {
                                view.SetColumnError(gridView1.Columns[ColName], "");
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
        void frmClosingOrders_KeyDown(object sender, KeyEventArgs e)
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
                if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
                {
                    if (UserInfo.Language == iLanguage.English)
                        ChangeLanguage.EnglishLanguage(frm);
                    frm.Show();
                }
                else
                    frm.Dispose();
            }
          

            else if (FocusedControl.Trim() == gridControl2.Name)
            {

                if (gridView1.FocusedColumn.Name == "colItemID" || gridView1.FocusedColumn.Name == "col" + ItemName || gridView1.FocusedColumn.Name == "colBarCode")
                {
                    frmItems frm = new frmItems();
                    if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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
                            gridView1.Columns[ItemName].ColumnEdit = rItem;
                            gridControl2.RepositoryItems.Add(rItem);

                        };
                    }
                    else
                        frm.Dispose();
                }
                else if (gridView1.FocusedColumn.Name == "colSizeName" || gridView1.FocusedColumn.Name == "colSizeID")
                {
                    frmSizingUnits frm = new frmSizingUnits();
                    if (Edex.ModelSystem.Permissions.UserPermissionsFrom(frm, frm.ribbonControl1, UserInfo.ID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID))
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
        void GridCadWax_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
             
            {
                e.Graphics.FillRectangle(e.Cache.GetSolidBrush(Color.Beige), e.Bounds);
                e.Graphics.DrawRectangle(e.Cache.GetPen(Color.Black, 1), e.Bounds);
                e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                e.Handled = true;
                GridCad.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
                GridCad.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;

            }
        }
        void txtCommandStage_Validating(object sender, CancelEventArgs e)
        {
            if (Comon.cInt(cmbTypeStage.EditValue) > 0)
            {
                if (Comon.cInt(txtCommandStage.Text) > 0)
                {
                    ClearFields();
                    if (Comon.cInt(cmbTypeStage.EditValue) >= 6)
                    {
                        int CommandStageID = Comon.cInt(Lip.GetValue("SELECT [ComandID]  FROM  [Menu_FactoryRunCommandMaster] where [TypeStageID]=" + cmbTypeStage.EditValue + "  and Cancel=0 and BranchID="+MySession.GlobalBranchID+" and [ComandID]=" + Comon.cInt(txtCommandStage.Text)));
                        if (CommandStageID > 0)
                        {
                            DataRecord = Menu_FactoryRunCommandMasterDAL.frmGetDataDetalByID(Comon.cInt(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));
                            txtBeforeStoreID.Text = DataRecord.Rows[0]["StoreIDFactory"].ToString();
                            txtBeforeStoreID_Validating(null, null);
                            //txtOrderID.Text = DataRecord.Rows[0]["Barcode"].ToString();
                            //txtOrderID_Validating(null, null);
                            cmbCurency.EditValue = Comon.cInt(DataRecord.Rows[0]["CurrencyID"].ToString());
                            cmbCurency_EditValueChanged(null, null);

                            DataRecordAfter = Manu_CloseOrdersDAL.frmGetDataDetalByID(Comon.cLong(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2, Comon.cInt(cmbTypeStage.EditValue) + 2);
                            if (Comon.cInt(cmbTypeStage.EditValue) == 14)
                                DataRecordAfter = Manu_CloseOrdersDAL.frmGetDataDetalByID(Comon.cLong(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2, 21);
                            if (DataRecordAfter != null)
                                if (DataRecordAfter.Rows.Count > 0)
                                {
                                    gridControl1.DataSource = DataRecordAfter;
                                    lstDetail.AllowNew = true;
                                    lstDetail.AllowEdit = true;
                                    lstDetail.AllowRemove = true;
                                    GridCad.RefreshData();
                                }
                        }
                        else
                        {
                            Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد أمر بهذا الرقم الرجاء ادخال رقم امر صحيح " : "The Command id is not Found.. Enter the Command Id Correct ");
                            txtCommandStage.Text = "";
                            txtCommandStage.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (Comon.cInt(cmbTypeStage.EditValue) <=2)
                        {
                            int CommandStageID = Comon.cInt(Lip.GetValue("SELECT [CommandID]  FROM  [Manu_CadWaxFactoryMaster] where [TypeStageID]=" + cmbTypeStage.EditValue + "  and Cancel=0 and BranchID="+MySession.GlobalBranchID+"  and [CommandID]=" + Comon.cInt(txtCommandStage.Text)));
                            if (CommandStageID > 0)
                            {
                                DataRecord = Manu_CadWaxFactoryDAL.frmGetDataDetalByID(Comon.cInt(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));

                                txtBeforeStoreID.Text = DataRecord.Rows[0]["StoreIDBefore"].ToString();
                                txtBeforeStoreID_Validating(null, null);
                                //txtOrderID.Text = DataRecord.Rows[0]["OrderID"].ToString();
                                //txtOrderID_Validating(null, null);
                                if (Comon.cInt(cmbTypeStage.EditValue)==1)
                                DataRecordAfter = Manu_CloseOrdersDAL.frmGetDataDetalByID(Comon.cLong(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2, 16);
                                if (Comon.cInt(cmbTypeStage.EditValue) ==2)
                                    DataRecordAfter = Manu_CloseOrdersDAL.frmGetDataDetalByID(Comon.cLong(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2, 17);
                                if (DataRecordAfter != null)
                                    if (DataRecordAfter.Rows.Count > 0)
                                    {
                                        gridControl1.DataSource = DataRecordAfter;
                                        lstDetail.AllowNew = true;
                                        lstDetail.AllowEdit = true;
                                        lstDetail.AllowRemove = true;
                                        GridCad.RefreshData();
                                    }
                            }

                            else
                            {
                                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد أمر بهذا الرقم الرجاء ادخال رقم امر صحيح " : "The Command id is not Found.. Enter the Command Id Correct ");
                                txtCommandStage.Text = "";
                                txtCommandStage.Focus();
                                return;
                            }
                        }
                        if (Comon.cInt(cmbTypeStage.EditValue) >= 3 && Comon.cInt(cmbTypeStage.EditValue) <= 4)
                        {
                            int CommandStageID = Comon.cInt(Lip.GetValue("SELECT [CommandID]  FROM  [Manu_ZirconDiamondFactoryMaster] where [TypeStageID]=" + cmbTypeStage.EditValue + "  and Cancel=0  and BranchID="+MySession.GlobalBranchID+"  and [CommandID]=" + Comon.cInt(txtCommandStage.Text)));
                            if (CommandStageID > 0)
                            {
                                DataRecord = Manu_ZirconDiamondFactoryDAL.frmGetDataDetalByID(Comon.cInt(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));

                                txtBeforeStoreID.Text = DataRecord.Rows[0]["StoreIDBefore"].ToString();
                                txtBeforeStoreID_Validating(null, null);
                                //txtOrderID.Text = DataRecord.Rows[0]["OrderID"].ToString();
                                //txtOrderID_Validating(null, null);
                                if (Comon.cInt(cmbTypeStage.EditValue) == 3)
                                    DataRecordAfter = Manu_CloseOrdersDAL.frmGetDataDetalByID(Comon.cLong(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2, 18);
                                if (Comon.cInt(cmbTypeStage.EditValue) == 4)
                                    DataRecordAfter = Manu_CloseOrdersDAL.frmGetDataDetalByID(Comon.cLong(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2, 19);
                                if (DataRecordAfter != null)
                                    if (DataRecordAfter.Rows.Count > 0)
                                    {
                                        gridControl1.DataSource = DataRecordAfter;
                                        lstDetail.AllowNew = true;
                                        lstDetail.AllowEdit = true;
                                        lstDetail.AllowRemove = true;
                                        GridCad.RefreshData();
                                    }
                            }
                            else
                            {
                                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد أمر بهذا الرقم الرجاء ادخال رقم امر صحيح " : "The Command id is not Found.. Enter the Command Id Correct ");
                                txtCommandStage.Text = "";
                                txtCommandStage.Focus();
                                return;
                            }
                        }
                        else    if (Comon.cInt(cmbTypeStage.EditValue) ==5)
                        {
                            int CommandStageID = Comon.cInt(Lip.GetValue("SELECT [CommandID]  FROM  [Manu_AfforestationFactoryMaster] where [TypeStageID]=" + cmbTypeStage.EditValue + "  and Cancel=0 and BranchID="+MySession.GlobalBranchID+" and [CommandID]=" + Comon.cInt(txtCommandStage.Text)));
                            if (CommandStageID > 0)
                            {
                                DataRecord = Manu_AfforestationFactoryDAL.frmGetDataDetalByID(Comon.cInt(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));

                                txtBeforeStoreID.Text = DataRecord.Rows[0]["StoreIDBefore"].ToString();
                                txtBeforeStoreID_Validating(null, null);
                                //txtOrderID.Text = DataRecord.Rows[0]["OrderID"].ToString();
                                //txtOrderID_Validating(null, null);                             
                              
                                    DataRecordAfter = Manu_CloseOrdersDAL.frmGetDataDetalByID(Comon.cLong(txtCommandStage.Text), Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2, 20);
                                if (DataRecordAfter != null)
                                    if (DataRecordAfter.Rows.Count > 0)
                                    {
                                        gridControl1.DataSource = DataRecordAfter;
                                        lstDetail.AllowNew = true;
                                        lstDetail.AllowEdit = true;
                                        lstDetail.AllowRemove = true;
                                        GridCad.RefreshData();
                                    }
                            }
                            else
                            {
                                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يوجد أمر بهذا الرقم الرجاء ادخال رقم امر صحيح " : "The Command id is not Found.. Enter the Command Id Correct ");
                                txtCommandStage.Text = "";
                                txtCommandStage.Focus();
                                return;
                            }
                        }
                    }
                    CalucalateRow();
                }
                else
                {
                    Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء ادخال رقم امر المرحلة التي  انتهت فيها الطلبية" : "Enter the Command Id For Stage ");
                    txtCommandStage.Focus();
                    return;
                }
            }
            else
            {
                Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "الرجاء إختيار نوع المرحلة التي  انتهت فيها الطلبية" : "Enter the Type Id For Stage ");
                cmbTypeStage.Focus();
                return;
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
                strSQL = "SELECT " + PrimaryName + " as UserName  FROM  [Users] where [Cancel]=0 and BranchID="+MySession.GlobalBranchID+"  and [UserID]=" + txtGuidanceID.Text.ToString();
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
                    strSQL = "SELECT ArbName as CustomerName  FROM Sales_CustomerAnSublierListArb Where    AcountID =" + txtCustomerID.Text + " and BranchID=" + MySession.GlobalBranchID;
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
        private void txtAfterStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (txtAfterStoreID.Text.Trim() != "")
                    if (Comon.cDbl(txtBeforeStoreID.Text) == Comon.cDbl(txtAfterStoreID.Text))
                    {
                        Messages.MsgWarning(Messages.TitleWorning, UserInfo.Language == iLanguage.Arabic ? "لا يمكن التحويل الى نفس المخزن " : "Cann't transefer Between Him self Store");
                        return;
                    }
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtAfterStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtAfterStoreID, lblAfterStoreName, strSQL);
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtAfterStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
                string StoreManger = Lip.GetValue(strSQL).ToString();
                lblAfterStoreManger.Text = StoreManger;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        private void txtBeforeStoreID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                strSQL = "SELECT " + PrimaryName + " as StoreName FROM Stc_Stores WHERE AccountID =" + Comon.cLong(txtBeforeStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                CSearch.ControlValidating(txtBeforeStoreID, lblBeforeStoreName, strSQL);
                strSQL = "SELECT ArbName as EmployeeName FROM HR_EmployeeFile WHERE EmployeeID in( Select StoreManger from Stc_Stores WHERE AccountID =" + Comon.cLong(txtBeforeStoreID.Text) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue) + ") And Cancel =0 ";
                string StoreManger = Lip.GetValue(strSQL).ToString();
                lblBeforeStoreManger.Text = StoreManger;
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                 
             
                txtBeforeStoreID.Text = "";
                txtBeforeStoreID_Validating(null, null);
                //lstDetail = new BindingList<Manu_CloseOrdersDetails>();
                lstDetail.AllowNew = true;
                lstDetail.AllowEdit = true;
                lstDetail.AllowRemove = true;
                gridControl1.DataSource = lstDetail;
                ClearFieldsTop();
                //txtOrderID.Text = "";
           
                lstDetailAfter.AllowNew = true;
                lstDetailAfter.AllowEdit = true;
                lstDetailAfter.AllowRemove = true;
                gridControl2.DataSource = lstDetailAfter;
                dt = new DataTable();
                DataRecord = new DataTable();
                DataRecordAfter = new DataTable();
                //txtOrderID.Focus();
            }
            catch (Exception ex)
            {
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
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
                        //txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
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
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CadCommend", "رقـم الأمر", Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtCommandID, null, "CadCommend", "Commend ID", Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == txtRepetID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtRepetID, null, "RepetID", "رقم تكرار الطلبية", Comon.cInt(cmbBranchesID.EditValue.ToString()), Condition: " Manu_ArrangingClosingOrders.OrderID="+txtOrderID.Text+"  and Menu_FactoryRunCommandMaster.Cancel=0 and dbo.Menu_FactoryRunCommandMaster.BranchID=" + Comon.cInt(cmbBranchesID.EditValue.ToString()));
                else
                    PrepareSearchQuery.Find(ref cls, txtRepetID, null, "RepetID", "Repet ID", Comon.cInt(cmbBranchesID.EditValue.ToString()),Condition: " Manu_ArrangingClosingOrders.OrderID="+txtOrderID.Text+"  and Menu_FactoryRunCommandMaster.Cancel=0 and dbo.Menu_FactoryRunCommandMaster.BranchID=" + Comon.cInt(cmbBranchesID.EditValue.ToString()));
            }
            else if (FocusedControl.Trim() == txtOrderID.Name)
            {
                if (MySession.GlobalDefaultCanRepetUseOrderOneOureMoreBeforeCasting == true)
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "رقم الطلب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "Order ID", MySession.GlobalBranchID);
                }
                else
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "رقم الطلب", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, txtOrderID, null, "OrderID", "Order ID", MySession.GlobalBranchID);
                }
            }
            else if (FocusedControl.Trim() == txtBeforeStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtBeforeStoreID, lblBeforeStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtBeforeStoreID, lblBeforeStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }
            else if (FocusedControl.Trim() == txtAfterStoreID.Name)
            {
                if (UserInfo.Language == iLanguage.Arabic)
                    PrepareSearchQuery.Find(ref cls, txtAfterStoreID, lblAfterStoreName, "StoreID", "رقم الحساب", Comon.cInt(cmbBranchesID.EditValue));
                else
                    PrepareSearchQuery.Find(ref cls, txtAfterStoreID, lblAfterStoreName, "StoreID", "Account ID", Comon.cInt(cmbBranchesID.EditValue));
            }

            else if (FocusedControl.Trim() == gridControl2.Name)
            {
                if (gridView1.FocusedColumn == null) return;
                if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "BarCodeForPurchaseInvoice", "البـاركـود", MySession.GlobalBranchID);
                }
                else if (gridView1.FocusedColumn.Name == "colSizeID")
                {
                    if (UserInfo.Language == iLanguage.Arabic)
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "رقـم الـوحـــده", MySession.GlobalBranchID);
                    else
                        PrepareSearchQuery.Find(ref cls, null, null, "SizeID", "Size ID", MySession.GlobalBranchID);
                }
            }

            GetSelectedSearchValue(cls);
        }
        private void FileItemData(DataTable dt)
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                //DataTable dtt = Lip.ChekRemidQTY(dt.Rows[0]["BarCode"].ToString(), Comon.cDbl(txtBeforeStoreID.Text), Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(txtCostCenterID.Text));
                //decimal totalQtyBalance = dtt.AsEnumerable().Where(row => row["QtyBalance"] != DBNull.Value).Sum(row => Convert.ToDecimal(row["QtyBalance"]));

                //if (dtt.Rows.Count > 0)
                //{
                //    if (totalQtyBalance <= 0)
                //    {
                //        if (MySession.AllowOutQtyNegative)
                //        {
                //            Messages.MsgWarning(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore);
                //            return;

                //        }
                //        bool yes = Messages.MsgQuestionYesNo(Messages.TitleWorning, Messages.msgNotFoundAnyQtyInStore + "هل تريد المتابعة ...");
                //        if (!yes)
                //            return;
                //    }
                //}
                //if (dtt.Rows.Count > 0)
                //    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], totalQtyBalance);
                //else
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QTY"], 0);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], dt.Rows[0]["SizeID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], dt.Rows[0]["SizeName"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], dt.Rows[0]["BarCode"].ToString().ToUpper());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], dt.Rows[0]["ItemID"].ToString());
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], dt.Rows[0]["ArbName"].ToString());
            }
            else
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Qty"], "0");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ItemID"], "");
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[ItemName], "");

            }
        }
        public void GetSelectedSearchValue(CSearch cls)
        {
            if (cls.PrimaryKeyValue != null && cls.PrimaryKeyValue.ToString() != "")
            {

                if (FocusedControl == txtCommandID.Name)
                {
                    txtCommandID.Text = cls.PrimaryKeyValue.ToString();
                    //txtCommandID_Validating(null, null);
                }
                if (FocusedControl == txtRepetID.Name)
                {
                    txtRepetID.Text = cls.PrimaryKeyValue.ToString();
                    //txtCommandID_Validating(null, null);
                }
                else if (FocusedControl == txtGuidanceID.Name)
                {
                    txtGuidanceID.Text = cls.PrimaryKeyValue.ToString();
                    txtGuidanceID_Validating(null, null);
                }

                else if (FocusedControl == txtOrderID.Name)
                {
                    txtOrderID.Text = cls.PrimaryKeyValue.ToString();
                    txtOrderID_Validating(null, null);
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
                else if (FocusedControl == gridControl2.Name)
                {
                    if (gridView1.FocusedColumn.Name == "colBarCode" || gridView1.FocusedColumn.Name == "colItemName" || gridView1.FocusedColumn.Name == "colItemID")
                    {
                        string Barcode = cls.PrimaryKeyValue.ToString();
                        if (Stc_itemsDAL.CheckIfStopItemUnit(Barcode, MySession.GlobalBranchID, MySession.GlobalFacilityID) == 1)
                        {

                            Messages.MsgStop(Messages.TitleInfo, Messages.msgWorningThisUnitIsStop);
                            return;
                        }
                        gridView1.AddNewRow();
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["BarCode"], Barcode);
                        FileItemData(Stc_itemsDAL.GetItemData1(Barcode, UserInfo.FacilityID));
                        // CalculateRow();
                    }

                    if (GridCad.FocusedColumn.Name == "colSizeID")
                    {
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SizeID"], cls.PrimaryKeyValue.ToString());
                        strSQL = "SELECT " + PrimaryName + " as " + SizeName + " FROM Stc_SizingUnits WHERE SizeID =" + Comon.cInt(cls.PrimaryKeyValue.ToString()) + " And Cancel =0 And  BranchID =" + Comon.cInt(cmbBranchesID.EditValue);
                        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[SizeName], Lip.GetValue(strSQL));
                    }
                }
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

                Manu_CloseOrdersMaster model = new Manu_CloseOrdersMaster();
                model.CommandID = Comon.cInt(txtCommandID.Text);
                model.EditUserID = UserInfo.ID;
                model.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                model.FacilityID = UserInfo.FacilityID;
                model.EditComputerInfo = UserInfo.ComputerInfo;
                model.EditDate = Comon.cLong(Lip.GetServerDateSerial());
                model.EditTime = Comon.cLong(Lip.GetServerTimeSerial()); 
                string Result = Manu_CloseOrdersDAL.Delete(model);
                ////حذف الحركة المخزنية 
                //if (Comon.cInt(Result) > 0)
                //{
                //    int MoveID = 0;
                //    MoveID = DeleteStockMoving(Comon.cInt(txtCommandID.Text), DocumentTypeCadFactory);

                //    if (MoveID < 0)
                //        Messages.MsgError(Messages.TitleInfo, "خطا في حذف الحركة  المخزنية");
                //}

                #region Delete Voucher Machin
                ////حذف القيد الالي
                //if (Comon.cInt(Result) > 0)
                //{
                //    int VoucherID = 0;

                //    VoucherID = DeleteVariousVoucherMachin(Comon.cInt(txtCommandID.Text), DocumentTypeCadFactory);
                //    if (VoucherID == 0)
                //        Messages.MsgError(Messages.TitleInfo, "خطا في حذف قيد العملية   ");
                //}
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
            EnabledControl(true);
            Validations.DoEditRipon(this, ribbonControl1);
        }
        protected override void DoSave()
        {
            try
            {
                if (!Validations.IsValidForm(this))
                    return;

                if (!Validations.IsValidFormCmb(cmbStatus))
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
                Save();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(Messages.msgErrorSave, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void Save()
        {

            {

                Manu_CloseOrdersMaster objRecord = new Manu_CloseOrdersMaster();
                objRecord.OrderID = txtOrderID.Text.ToString();
                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.BranchID = Comon.cInt(cmbBranchesID.EditValue);
                objRecord.Cancel = 0;
                objRecord.TypeStageID = Comon.cInt(cmbTypeStage.EditValue);
                objRecord.CommandStageID = Comon.cInt(txtCommandStage.Text);
                objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
                objRecord.CommandID = Comon.cInt(txtCommandID.Text);
                objRecord.TotalOrderQTY = Comon.cDec(txtTotalQTY.Text);
                objRecord.RepetID =Comon.cInt( txtRepetID.Text);
                objRecord.FacilityID = UserInfo.FacilityID;
                objRecord.Posted = Comon.cInt(cmbStatus.EditValue);
                //الحسابات 
                objRecord.BeforeStoreID = Comon.cDbl(txtBeforeStoreID.Text);
                objRecord.AfterStoreID = Comon.cDbl(txtAfterStoreID.Text);
              
   
                objRecord.CommandDate = Comon.ConvertDateToSerial(txtCommandDate.EditValue.ToString());
                

                #region Save Additional

                Manu_CloseOrdersDetails returned;
                List<Manu_CloseOrdersDetails> listreturned = new List<Manu_CloseOrdersDetails>();

          
                int lengthBefore =GridCad.DataRowCount;
                int lengthAfter =gridView1.DataRowCount;
                if (lengthBefore > 0)
                {

                    {
                        for (int i = 0; i < lengthBefore; i++)
                        {
                            returned = new Manu_CloseOrdersDetails();
                          
                            returned.CommandID = Comon.cInt(txtCommandID.Text.ToString());

                            returned.QTY = Comon.cDec(GridCad.GetRowCellValue(i, "QTY").ToString());
                            returned.TypeOprationID = 1;
                            returned.BarCode = GridCad.GetRowCellValue(i, "BarCode").ToString();
                             
                            returned.SizeID = Comon.cInt(GridCad.GetRowCellValue(i, "SizeID").ToString());
                            returned.ItemID = Comon.cInt(GridCad.GetRowCellValue(i, "ItemID").ToString()); 
                            returned.ArbItemName = GridCad.GetRowCellValue(i, ItemName).ToString();
                            returned.EngItemName = GridCad.GetRowCellValue(i, ItemName).ToString();
                            returned.ArbSizeName = GridCad.GetRowCellValue(i, SizeName).ToString();
                            returned.EngSizeName = GridCad.GetRowCellValue(i, SizeName).ToString();
                            returned.BranchID =MySession.GlobalBranchID;
                         
                            returned.FacilityID = UserInfo.FacilityID;
                            
                            listreturned.Add(returned);
                        }
                        if (lengthAfter > 0)
                        {

                            for (int i = 0; i < lengthAfter; i++)
                            {
                                returned = new Manu_CloseOrdersDetails();
                              
                                returned.CommandID = Comon.cInt(txtCommandID.Text.ToString());

                                returned.QTY = Comon.cDec(gridView1.GetRowCellValue(i, "QTY").ToString());
                                returned.TypeOprationID = 2;

                                returned.BarCode = gridView1.GetRowCellValue(i, "BarCode").ToString();
                                returned.SizeID = Comon.cInt(gridView1.GetRowCellValue(i, "SizeID").ToString());
                                returned.ItemID = Comon.cInt(gridView1.GetRowCellValue(i, "ItemID").ToString()); 
                                returned.ArbItemName = gridView1.GetRowCellValue(i, ItemName).ToString();
                                returned.EngItemName = gridView1.GetRowCellValue(i, ItemName).ToString();
                                returned.ArbSizeName = gridView1.GetRowCellValue(i, SizeName).ToString();
                                returned.EngSizeName = gridView1.GetRowCellValue(i, SizeName).ToString();
                                returned.BranchID = UserInfo.BRANCHID;
                               
                                returned.FacilityID = UserInfo.FacilityID;
                               
                                listreturned.Add(returned);
                            }


                        }
                    }
                }
                #endregion

                if (listreturned.Count > 0)
                {
                    objRecord.Manu_Detils = listreturned;

                    //objRecord.Manu_OrderDetils = SaveOrderDetials();

                    string Result = Manu_CloseOrdersDAL.InsertUsingXML(objRecord, IsNewRecord).ToString();

                    if (Comon.cInt(Result) > 0 && Comon.cInt(cmbStatus.EditValue)>1)
                    {
                        // حفظ الحركة المخزنية 
                        int MoveID = SaveStockMoveing(Comon.cInt(Result));
                        if (MoveID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ الحركة المخزنية");

                      //حفظ القيد الالي
                        long VoucherID = SaveVariousVoucherMachin(Comon.cInt(Result));
                        if (VoucherID == 0)
                            Messages.MsgError(Messages.TitleInfo, "خطا في حفظ قيد العملية");
                        //else
                        //    Lip.ExecututeSQL("Update " + Stc_GoldInonBailDAL.TableName + " Set RegistrationNo =" + VoucherID + " where " + Stc_GoldInonBailDAL.PremaryKey + " = " + txtInvoiceID.Text);

                    }
                    
                    if (Comon.cInt(Result) > 0)
                    {

                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgSaveComplete);
                        ClearFields();
                        DoNew();
                    }
                    else
                    {
                        Messages.MsgInfo(Messages.TitleInfo, Messages.msgErrorSave + " " + Result);
                    }
                }
            }
        }
        long SaveVariousVoucherMachin(int DocumentID)
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
            objRecord.VoucherDate = Comon.ConvertDateToSerial(txtCommandDate.Text).ToString();
            objRecord.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
            //objRecord.RegistrationNo = Comon.cInt(txtRegistrationNo.Text);
            // objRecord.InvoiceID = Comon.cInt(txtInvoiceID.Text);
            // objRecord.DelegateID = Comon.cInt(txtDelegateID.Text);
            objRecord.Notes =   this.Text  ;
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
            if (IsNewRecord == false)
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
            returned.ID = 2;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cDbl(txtAfterStoreID.Text);
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
           
            returned.VoucherID = VoucherID;
            //if (returned.CurrencyID == 1)
                returned.DebitGold = Comon.cDbl(txtTotalQTY.Text);

            returned.Declaration = this.Text + "   " + txtOrderID.Text == string.Empty ? this.Text : this.Text + "   " + txtOrderID.Text;

            returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);


            returned.CurrencyPrice = Comon.cDbl(txtCurrncyPrice.Text);
            returned.CurrencyEquivalent = Comon.cDbl(Comon.cDbl(returned.Debit) * Comon.cDbl(returned.CurrencyPrice));
            listreturned.Add(returned);

            //Credit Gold      
            returned = new Acc_VariousVoucherMachinDetails();
            returned.ID = 1;
            returned.BranchID = Comon.cInt(cmbBranchesID.EditValue);
            returned.FacilityID = UserInfo.FacilityID;
            returned.AccountID = Comon.cLong(txtBeforeStoreID.Text);
            returned.VoucherID = VoucherID;
            //  returned.Credit = Comon.cDbl(lblNetBalance.Text) + Comon.cDbl(lblAdditionaAmmount.Text);
            returned.CurrencyID = Comon.cInt(cmbCurency.EditValue.ToString());
             
            //if (returned.CurrencyID == 1)
                returned.CreditGold = Comon.cDbl(txtTotalQTY.Text);
            returned.Declaration = this.Text+"   "+txtOrderID.Text == string.Empty ? this.Text :this.Text+"   "+txtOrderID.Text;
            returned.CostCenterID = Comon.cInt(MySession.GlobalDefaultCostCenterID);

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
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + "  and BranchID=" + MySession.GlobalBranchID ));
                returned.QTY = Comon.cDbl(GridCad.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = 0;
                returned.OutPrice = 0;
                //returned.Bones = Comon.cDbl(GridViewBeforfactory.GetRowCellValue(i, "Bones").ToString());
                returned.OutPrice = 0;
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
                returned.GroupID = Comon.cDbl(Lip.GetValue("SELECT [GroupID] FROM   Stc_Items where [ItemID]=" + returned.ItemID + "  and BranchID=" + MySession.GlobalBranchID ));
                returned.QTY = Comon.cDbl(GridCad.GetRowCellValue(i, "QTY").ToString());
                returned.InPrice = 0;
                returned.OutPrice = 0;
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
        protected override void DoSearch()
        {
            try
            {
                Find();
            }
            catch { }
        }
        protected override void DoNew()
        {
            try
            {
                IsNewRecord = true;
                txtCommandID.Text = Manu_CloseOrdersDAL.GetNewID(UserInfo.FacilityID, Comon.cInt(cmbBranchesID.EditValue), Comon.cInt(cmbTypeStage.EditValue)).ToString();
                ClearFields();
                EnabledControl(true);
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                Messages.MsgError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }
        }
        public void txtOrderID_Validating(object sender, CancelEventArgs e)
        {

            if (FormView == true)
            {
                if (String.IsNullOrEmpty(txtOrderID.Text) == false)
                {
                    string txtOrder = txtOrderID.Text;


                       DataTable dtRepet = Lip.SelectRecord("SELECT [RepetID]   FROM  [Manu_ArrangingClosingOrders] where [BranchID]=" + MySession.GlobalBranchID + " and [Cancel]=0   and [OrderID]=" + txtOrderID.Text + " group by [RepetID]");                  
                        if (dtRepet.Rows.Count >= 1)
                        {
                            //ReadTopInfo(txtOrderID.Text); 
                            if (dtRepet.Rows.Count > 1)
                            {
                                txtRepetID.Focus();
                                Find();
                            }
                            else
                            {
                                txtRepetID.Text = dtRepet.Rows[0][0].ToString();
                            }
                        }
                    int CommandIDTemp = 0;
                    CommandIDTemp = Comon.cInt(Lip.GetValue("select CommandID from Manu_CloseOrdersMaster where Cancel=0 and BranchID=" + MySession.GlobalBranchID + "  and CommandID<>" + Comon.cInt(txtCommandID.Text) + " and RepetID="+txtRepetID.Text + "  and OrderID='" + txtOrderID.Text + "'"));
                    int CommandIDThis = Comon.cInt(Lip.GetValue("select CommandID from Manu_CloseOrdersMaster where Cancel=0  and BranchID=" + MySession.GlobalBranchID + "  and CommandID=" + Comon.cInt(txtCommandID.Text) + " and RepetID=" + txtRepetID.Text + " and OrderID='" + txtOrderID.Text + "'"));
                    if ( CommandIDTemp > 0)
                    {
                        if (CommandIDTemp > 0)
                        {
                            Messages.MsgWarning(Messages.TitleWorning, Messages.msgDontRepetTheOrderinMoreCommend);
                            txtCommandID.Text = CommandIDTemp.ToString();
                            txtCommandID_Validating(null, null);
                            return;
                        }
                    }
                    else if (IsNewRecord == false && CommandIDTemp <= 0 && CommandIDThis != Comon.cInt(txtCommandID.Text))
                    {
                        txtOrder = txtOrderID.Text;
                        ClearFields();
                        string OrderID = txtOrder;
                        txtOrderID.Text = OrderID;
                        ReadTopInfo(txtOrderID.Text); 
                        IsNewRecord = true;
                        Validations.DoNewRipon(this, ribbonControl1);
                    }
                    if ((IsNewRecord && CommandIDTemp <= 0))
                    {
                          string OrderID = txtOrder;
                        strSQL = "SELECT * FROM Manu_OrderRestriction WHERE  OrderID ='" + OrderID.Trim() + "'  and BranchID=" + MySession.GlobalBranchID;
                        Lip.ConvertStrSQLToEnglishOrArabicLanguage(strSQL, UserInfo.Language.ToString());
                        System.Data.DataTable dtt = Lip.SelectRecord(strSQL);
                        if (dtt.Rows.Count > 0  )
                        {
                            
                            if (Comon.cInt(txtRepetID.Text) > 0)
                            {
                                int ID = Comon.cInt(Lip.GetValue("select max(ID) from [Manu_ArrangingClosingOrders] where [BranchID]=" + MySession.GlobalBranchID + " and [Cancel]=0   and [OrderID]=" + txtOrderID.Text + " and RepetID=" + txtRepetID.Text));
                                DataTable dtDataOrder = Lip.SelectRecord("SELECT [StageID],CommandID   FROM  [Manu_ArrangingClosingOrders] where [BranchID]=" + MySession.GlobalBranchID + " and [Cancel]=0   and [OrderID]=" + txtOrderID.Text + " and RepetID=" + txtRepetID.Text+" and ID="+ID);
                                cmbTypeStage.EditValue = Comon.cInt(dtDataOrder.Rows[0]["StageID"].ToString());
                                txtCommandStage.Text = dtDataOrder.Rows[0]["CommandID"].ToString();
                                DoNew();
                                txtCommandStage_Validating(null, null);
                                ReadTopInfo(txtOrderID.Text); 
                            }
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
                            Messages.MsgError("تنبيه", "   لا يوجد طلب بهذا الرقم   ");
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
                    strSQL = "SELECT TOP 1 *  FROM " + Manu_CloseOrdersDAL.TableName + " Where Cancel =0    And BranchID= " + Comon.cInt(cmbBranchesID.EditValue);
                    switch (Direction)
                    {
                        case xMoveFirst:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_CloseOrdersDAL.PremaryKey + " ASC";
                                break;
                            }

                        case xMoveNext:
                            {
                                strSQL = strSQL + " And " + Manu_CloseOrdersDAL.PremaryKey + ">" + PremaryKeyValue + " ORDER BY " + Manu_CloseOrdersDAL.PremaryKey + " asc";
                                break;
                            }

                        case xMovePrev:
                            {
                                strSQL = strSQL + " And " + Manu_CloseOrdersDAL.PremaryKey + "<" + PremaryKeyValue + " ORDER BY " + Manu_CloseOrdersDAL.PremaryKey + " desc";
                                break;
                            }
                        case xMoveLast:
                            {
                                strSQL = strSQL + " ORDER BY " + Manu_CloseOrdersDAL.PremaryKey + " DESC";
                                break;
                            }
                    }
                    cClass = new Manu_CloseOrdersDAL();

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
        public void ReadRecord(int CommendID, bool flag = false)
        {
            try
            {
                ClearFields();
                {
                    dt = Manu_CloseOrdersDAL.frmGetDataDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, Comon.cInt(cmbTypeStage.EditValue));
                    DataRecord = Manu_CloseOrdersDAL.frmGetDataDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID,1, 7);
                    DataRecordAfter = Manu_CloseOrdersDAL.frmGetDataDetalByID(CommendID, Comon.cInt(cmbBranchesID.EditValue), UserInfo.FacilityID, 2, 7);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        IsNewRecord = false;
                        txtCommandID.Text = dt.Rows[0]["CommandID"].ToString();

                        txtRepetID.Text = dt.Rows[0]["RepetID"].ToString();

                        txtCommandStage.Text = Comon.cInt(dt.Rows[0]["CommandStageID"]).ToString();
                        txtBeforeStoreID.Text = Comon.cDbl(dt.Rows[0]["BeforeStoreID"]).ToString();
                        txtBeforeStoreID_Validating(null, null);
                        txtAfterStoreID.Text = Comon.cDbl(dt.Rows[0]["AfterStoreID"]).ToString();
                        txtAfterStoreID_Validating(null, null);
                        cmbTypeStage.EditValue = Comon.cInt(dt.Rows[0]["TypeStageID"]);
                        cmbCurency.EditValue = Comon.cInt(dt.Rows[0]["CurrencyID"].ToString());
                        cmbCurency_EditValueChanged(null, null);
                        cmbBranchesID.EditValue = Comon.cInt(dt.Rows[0]["BranchID"]);

                        cmbStatus.EditValue = Comon.cInt(dt.Rows[0]["Posted"]);

                        txtCommandDate.DateTime = DateTime.ParseExact(Comon.ConvertSerialDateTo(dt.Rows[0]["CommandDate"].ToString()), "dd/MM/yyyy", culture);

                        gridControl1.DataSource = DataRecord;
                        lstDetail.AllowNew = true;
                        lstDetail.AllowEdit = true;
                        lstDetail.AllowRemove = true;

                        gridControl2.DataSource = DataRecordAfter;
                        lstDetailAfter.AllowNew = true;
                        lstDetailAfter.AllowEdit = true;
                        lstDetailAfter.AllowRemove = true;

                        txtOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                        //txtOrderID_Validating(null, null);
                        ReadTopInfo(txtOrderID.Text);
                        CalucalateRow();
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
        void CalucalateRow()
        {
            decimal InvoiceTotalGold = 0;
            decimal InvoiceTotalZircon = 0;
            decimal TotalDiamondCustomer = 0;
            decimal TotalDaimond_W = 0;
            decimal TotalQTY = 0;
            for (int i = 0; i <= GridCad.DataRowCount - 1; i++)
            {
                int BaseID = Comon.cInt(Lip.GetValue("SELECT  [BaseID] FROM  [Stc_Items] where [ItemID]=" + GridCad.GetRowCellValue(i, "ItemID").ToString() + "  and [Cancel]=0  and BranchID=" + MySession.GlobalBranchID));
                int TypeID = Comon.cInt(Lip.GetValue("SELECT  [TypeID] FROM  [Stc_Items] where [ItemID]=" + GridCad.GetRowCellValue(i, "ItemID").ToString() + "  and [Cancel]=0  and BranchID=" + MySession.GlobalBranchID));
                if (BaseID == 4 && TypeID != 1)
                    InvoiceTotalZircon += Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "QTY").ToString());
                else if (BaseID == 5 || (BaseID == 4 && TypeID != 1))
                    InvoiceTotalGold += Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "QTY").ToString());
                else if ((BaseID > 0 && BaseID < 4) || BaseID == 11)
                    TotalDaimond_W += Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "QTY").ToString());

                int isServec = Comon.cInt(Lip.GetValue("SELECT IsService  FROM   [Stc_Items] where [ItemID]=" + GridCad.GetRowCellValue(i, "ItemID").ToString() + " and [Cancel]=0  and BranchID=" + MySession.GlobalBranchID));
                if (isServec == 1 && ((BaseID > 0 && BaseID < 4) || BaseID == 11))
                    TotalDiamondCustomer += Comon.ConvertToDecimalPrice(GridCad.GetRowCellValue(i, "QTY").ToString());

                if (Comon.cInt(GridCad.GetRowCellValue(i, "SizeID").ToString()) == 2)
                    TotalQTY += Comon.cDec(Comon.cDec(GridCad.GetRowCellValue(i, "QTY").ToString()) / 5);
                else 
                     TotalQTY += Comon.cDec(GridCad.GetRowCellValue(i, "QTY").ToString());
            
            }
            lblInvoiceTotalGold.Text = InvoiceTotalGold.ToString();
            lblTotalDaimond.Text = TotalDaimond_W.ToString();
            lblTotalDiamondCustomer.Text = TotalDiamondCustomer.ToString();
            lblTotalZircon.Text = InvoiceTotalZircon.ToString();
            txtTotalQTY.Text = TotalQTY.ToString();
        }
        void initGrid()
        {

            lstDetail = new BindingList<Manu_CloseOrdersDetails>();
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
            GridCad.Columns[SizeName].ColumnEdit = riComboBoxitems;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0  and BranchID=" + MySession.GlobalBranchID );
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
            GridCad.Columns["TypeOprationID"].Visible = false;
            GridCad.Columns["ArbItemName"].Visible = GridCad.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            GridCad.Columns["EngItemName"].Visible = GridCad.Columns["EngItemName"].Name == "col" + ItemName ? true : false;

            GridCad.Columns["TotalCost"].OptionsColumn.ReadOnly = false;

            GridCad.Columns[ItemName].Visible = true;
            GridCad.Columns[ItemName].Caption = CaptionItemName;
            GridCad.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            GridCad.Columns["TotalCost"].OptionsColumn.AllowFocus = false;

            GridCad.Columns["CostPrice"].Visible = false;
            GridCad.Columns["TotalCost"].Visible = false;
            GridCad.Columns[ItemName].Width = 150;
            GridCad.Columns[SizeName].Width = 120;

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
            }

        }
        void initGridAfter()
        {

            lstDetailAfter = new BindingList<Manu_CloseOrdersDetails>();
            lstDetailAfter.AllowNew = true;
            lstDetailAfter.AllowEdit = true;
            lstDetailAfter.AllowRemove = true;

            gridControl2.DataSource = lstDetailAfter;

            DataTable dtitems = Lip.SelectRecord("SELECT   SizeID," + PrimaryName + "   FROM Stc_SizingUnits  where  BranchID=" + MySession.GlobalBranchID);
            string[] NameUnit = new string[dtitems.Rows.Count];
            for (int i = 0; i <= dtitems.Rows.Count - 1; i++)
                NameUnit[i] = dtitems.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems = new RepositoryItemComboBox();
            riComboBoxitems.Items.AddRange(NameUnit);
            gridControl2.RepositoryItems.Add(riComboBoxitems);
            gridView1.Columns[SizeName].ColumnEdit = riComboBoxitems;


            DataTable dtItemname = Lip.SelectRecord("Select " + PrimaryName + " from Stc_Items  Where Cancel=0  and BranchID=" + MySession.GlobalBranchID);
            string[] ItemNames = new string[dtItemname.Rows.Count];
            for (int i = 0; i <= dtItemname.Rows.Count - 1; i++)
                ItemNames[i] = dtItemname.Rows[i][PrimaryName].ToString();
            RepositoryItemComboBox riComboBoxitems4 = new RepositoryItemComboBox();
            riComboBoxitems4.Items.AddRange(ItemNames);
            gridControl2.RepositoryItems.Add(riComboBoxitems4);
            gridView1.Columns[ItemName].ColumnEdit = riComboBoxitems4;

            gridView1.Columns["CommandID"].Visible = false;
            gridView1.Columns["BranchID"].Visible = false;
            gridView1.Columns["FacilityID"].Visible = false;
            gridView1.Columns["TypeOprationID"].Visible = false;
            gridView1.Columns["ArbItemName"].Visible = gridView1.Columns["ArbItemName"].Name == "col" + ItemName ? true : false;
            gridView1.Columns["EngItemName"].Visible = gridView1.Columns["EngItemName"].Name == "col" + ItemName ? true : false;

            gridView1.Columns["TotalCost"].OptionsColumn.ReadOnly = false;

            gridView1.Columns[ItemName].Visible = true;
            gridView1.Columns[ItemName].Caption = CaptionItemName;
            gridView1.Columns["TotalCost"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["TotalCost"].OptionsColumn.AllowFocus = false;

            gridView1.Columns["CostPrice"].Visible = false;
            gridView1.Columns["TotalCost"].Visible = false;
            gridView1.Columns[ItemName].Width = 150;
            gridView1.Columns[SizeName].Width = 120;

            if (UserInfo.Language == iLanguage.Arabic)
            {
                gridView1.Columns["EngItemName"].Visible = false;
                gridView1.Columns["EngSizeName"].Visible = false;
                gridView1.Columns["BarCode"].Caption = "باركود الصنف";
                gridView1.Columns["SizeID"].Caption = "رقم الوحدة";
                gridView1.Columns["ItemID"].Caption = "رقم الصنــف";

                gridView1.Columns[SizeName].Caption = "إسم الوحدة";
                gridView1.Columns["QTY"].Caption = "الكمية ";
                gridView1.Columns["CostPrice"].Caption = "القيمة";
                gridView1.Columns["TotalCost"].Caption = "الإجمالي ";
            }
            else
            {
                gridView1.Columns["ArbItemName"].Visible = false;
                gridView1.Columns["ArbSizeName"].Visible = false;
                gridView1.Columns["BarCode"].Caption = "BarCode";
                gridView1.Columns["SizeID"].Caption = "Unit ID";
                gridView1.Columns["ItemID"].Caption = "Item ID";
                gridView1.Columns[SizeName].Caption = "Unit Name ";
                gridView1.Columns["CostPrice"].Caption = "Cost Price";
                gridView1.Columns["QTY"].Caption = "QTY";
                gridView1.Columns["TotalCost"].Caption = "Total Cost ";
            }

        }
        private void frmClosingOrders_Load(object sender, EventArgs e)
        {
            try
            {
                initGrid();
                initGridAfter();
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

        private void btnMachinResraction_Click(object sender, EventArgs e)
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